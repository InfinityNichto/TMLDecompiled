using System;
using System.IO;
using System.Threading;
using Microsoft.Xna.Framework;
using ReLogic.OS;
using Steamworks;
using Terraria.Localization;
using Terraria.ModLoader.Engine;

namespace Terraria.Social.Steam;

public class CoreSocialModule : ISocialModule
{
	private static CoreSocialModule _instance;

	private bool IsSteamValid;

	private object _steamTickLock = new object();

	private object _steamCallbackLock = new object();

	private Callback<GameOverlayActivated_t> _onOverlayActivated;

	private bool _skipPulsing;

	public static event Action OnTick;

	public static void SetSkipPulsing(bool shouldSkipPausing)
	{
	}

	public void Initialize()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		_instance = this;
		Terraria.ModLoader.Engine.Steam.SetAppId(Terraria.ModLoader.Engine.Steam.TMLAppID_t);
		if (!SteamAPI.Init())
		{
			ErrorReporting.FatalExit(Language.GetTextValue("Error.LaunchFromSteam"));
		}
		PortFilesToCurrentStructure();
		IsSteamValid = true;
		Thread thread = new Thread(SteamCallbackLoop);
		thread.IsBackground = true;
		thread.Start();
		Thread thread2 = new Thread(SteamTickLoop);
		thread2.IsBackground = true;
		thread2.Start();
		Main.OnTickForThirdPartySoftwareOnly += PulseSteamTick;
		Main.OnTickForThirdPartySoftwareOnly += PulseSteamCallback;
		if (Platform.IsOSX && !Main.dedServ)
		{
			_onOverlayActivated = Callback<GameOverlayActivated_t>.Create((DispatchDelegate<GameOverlayActivated_t>)OnOverlayActivated);
		}
	}

	public void PulseSteamTick()
	{
		if (Monitor.TryEnter(_steamTickLock))
		{
			Monitor.Pulse(_steamTickLock);
			Monitor.Exit(_steamTickLock);
		}
	}

	public void PulseSteamCallback()
	{
		if (Monitor.TryEnter(_steamCallbackLock))
		{
			Monitor.Pulse(_steamCallbackLock);
			Monitor.Exit(_steamCallbackLock);
		}
	}

	public static void Pulse()
	{
		_instance.PulseSteamTick();
		_instance.PulseSteamCallback();
	}

	private void SteamTickLoop(object context)
	{
		Monitor.Enter(_steamTickLock);
		while (IsSteamValid)
		{
			if (_skipPulsing)
			{
				Monitor.Wait(_steamCallbackLock);
				continue;
			}
			if (CoreSocialModule.OnTick != null)
			{
				CoreSocialModule.OnTick();
			}
			Monitor.Wait(_steamTickLock);
		}
		Monitor.Exit(_steamTickLock);
	}

	private void SteamCallbackLoop(object context)
	{
		Monitor.Enter(_steamCallbackLock);
		while (IsSteamValid)
		{
			if (_skipPulsing)
			{
				Monitor.Wait(_steamCallbackLock);
				continue;
			}
			SteamAPI.RunCallbacks();
			Monitor.Wait(_steamCallbackLock);
		}
		Monitor.Exit(_steamCallbackLock);
		SteamAPI.Shutdown();
	}

	public void Shutdown()
	{
		IsSteamValid = false;
	}

	public void OnOverlayActivated(GameOverlayActivated_t result)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		((Game)Main.instance).IsMouseVisible = result.m_bActive == 1;
	}

	private static void PortFilesToCurrentStructure()
	{
		Program.PortFilesMaster(GetCloudSaveLocation(), isCloud: true);
	}

	internal static string GetCloudSaveLocation()
	{
		string steamCloudFolder = default(string);
		SteamUser.GetUserDataFolder(ref steamCloudFolder, 512);
		return Path.Combine(Path.GetDirectoryName(Path.GetDirectoryName(steamCloudFolder)), "105600", "remote");
	}
}
