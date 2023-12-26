using System;
using System.Linq;
using SDL2;

namespace Terraria.ModLoader.Engine;

internal static class FNAFixes
{
	internal static void Init()
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		if (OperatingSystem.IsWindows())
		{
			SDL.SDL_SetHintWithPriority("SDL_VIDEO_MINIMIZE_ON_FOCUS_LOSS", "0", (SDL_HintPriority)2);
		}
		ConfigureDrivers();
	}

	private static void ConfigureDrivers()
	{
		ConfigureDrivers("SDL_VIDEODRIVER", "-videodriver", SDL.SDL_GetNumVideoDrivers(), (Func<int, string>)SDL.SDL_GetVideoDriver);
		ConfigureDrivers("SDL_AUDIODRIVER", "-audiodriver", SDL.SDL_GetNumAudioDrivers(), (Func<int, string>)SDL.SDL_GetAudioDriver);
	}

	private static void ConfigureDrivers(string sdlHintName, string launchArg, int numDrivers, Func<int, string> getDriver)
	{
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		string[] drivers = (from n in Enumerable.Range(0, numDrivers).Select(getDriver)
			where n != null
			select n).ToArray();
		string defaultDriverString = string.Join(",", drivers);
		if (Program.LaunchParameters.TryGetValue(launchArg, out var launchArgValue))
		{
			Environment.SetEnvironmentVariable(sdlHintName, launchArgValue);
		}
		string envVarValue = Environment.GetEnvironmentVariable(sdlHintName);
		if (envVarValue != null)
		{
			Logging.FNA.Info((object)$"Detected {sdlHintName}={envVarValue}. Appending default driver list as fallback: {defaultDriverString}");
			SDL.SDL_SetHintWithPriority(sdlHintName, envVarValue + "," + defaultDriverString, (SDL_HintPriority)2);
		}
	}
}
