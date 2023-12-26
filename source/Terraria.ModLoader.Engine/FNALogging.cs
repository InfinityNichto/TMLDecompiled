using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SDL2;

namespace Terraria.ModLoader.Engine;

/// <summary>
/// Attempt to track spurious device resets, backbuffer flickers and resizes
/// Also setup some FNA logging
/// </summary>
internal static class FNALogging
{
	private abstract class DeviceParam
	{
		public readonly string name;

		public DeviceParam(string name)
		{
			this.name = name;
		}

		public abstract void LogChange(GraphicsDevice g, StringBuilder sb, bool creating);
	}

	private class DeviceParam<T> : DeviceParam
	{
		private readonly Func<GraphicsDevice, T> getter;

		private readonly Func<T, string> getDescription;

		private T value;

		private string Desc => getDescription?.Invoke(value) ?? value.ToString();

		public DeviceParam(string name, Func<GraphicsDevice, T> getter, Func<T, string> getDescription = null)
			: base(name)
		{
			this.getter = getter;
			this.getDescription = getDescription;
		}

		public override void LogChange(GraphicsDevice g, StringBuilder changes, bool creating)
		{
			if (creating)
			{
				value = getter(g);
			}
			changes.Append(", ").Append(name).Append(": ")
				.Append(Desc);
			T newValue = getter(g);
			if (!EqualityComparer<T>.Default.Equals(value, newValue))
			{
				value = newValue;
				changes.Append(" -> ").Append(Desc);
			}
		}
	}

	private static List<DeviceParam> Params = new List<DeviceParam>
	{
		new DeviceParam<GraphicsAdapter>("Adapter", (GraphicsDevice g) => g.Adapter, (GraphicsAdapter a) => a.Description),
		new DeviceParam<DisplayMode>("DisplayMode", (GraphicsDevice g) => g.Adapter.CurrentDisplayMode),
		new DeviceParam<GraphicsProfile>("Profile", (GraphicsDevice g) => g.GraphicsProfile),
		new DeviceParam<int>("Width", (GraphicsDevice g) => g.PresentationParameters.BackBufferWidth),
		new DeviceParam<int>("Height", (GraphicsDevice g) => g.PresentationParameters.BackBufferHeight),
		new DeviceParam<bool>("Fullscreen", (GraphicsDevice g) => g.PresentationParameters.IsFullScreen),
		new DeviceParam<string>("Display", (GraphicsDevice g) => g.Adapter.DeviceName)
	};

	private static bool creating;

	public static string DriverIdentifier { get; internal set; }

	internal static void RedirectLogs()
	{
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		FNALoggerEXT.LogInfo = delegate(string s)
		{
			if (DriverIdentifier == null && s.StartsWith("FNA3D Driver: "))
			{
				DriverIdentifier = s.Substring("FNA3D Driver: ".Length);
				Logging.FNA.Info((object)("SDL Video Diver: " + SDL.SDL_GetCurrentVideoDriver()));
			}
			Logging.FNA.Info((object)s);
		};
		FNALoggerEXT.LogWarn = delegate(string s)
		{
			Logging.FNA.Warn((object)s);
		};
		FNALoggerEXT.LogError = delegate(string s)
		{
			Logging.FNA.Error((object)s);
		};
		Logging.FNA.Debug((object)"Querying linked library versions...");
		SDL_version sdl_version = default(SDL_version);
		SDL.SDL_GetVersion(ref sdl_version);
		Logging.FNA.Debug((object)$"SDL v{sdl_version.major}.{sdl_version.minor}.{sdl_version.patch}");
		uint fna3d_version = FNA3D.FNA3D_LinkedVersion();
		Logging.FNA.Debug((object)$"FNA3D v{fna3d_version / 10000}.{fna3d_version / 100 % 100}.{fna3d_version % 100}");
		uint faudio_version = FAudio.FAudioLinkedVersion();
		Logging.FNA.Debug((object)$"FAudio v{faudio_version / 10000}.{faudio_version / 100 % 100}.{faudio_version % 100}");
	}

	public static void GraphicsInit(GraphicsDeviceManager graphics)
	{
		graphics.DeviceReset += LogDeviceReset;
		graphics.DeviceCreated += delegate
		{
			creating = true;
		};
	}

	private static void LogDeviceReset(object sender, EventArgs e)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		GraphicsDevice graphicsDevice = ((GraphicsDeviceManager)sender).GraphicsDevice;
		StringBuilder sb = new StringBuilder("Device " + (creating ? "Created" : "Reset"));
		foreach (DeviceParam param in Params)
		{
			param.LogChange(graphicsDevice, sb, creating);
		}
		Logging.Terraria.Debug((object)sb);
		creating = false;
	}

	internal static void PostAudioInit()
	{
		Logging.FNA.Info((object)("SDL Audio Diver: " + SDL.SDL_GetCurrentAudioDriver()));
	}
}
