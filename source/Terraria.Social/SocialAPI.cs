using System;
using System.Collections.Generic;
using ReLogic.OS;
using Terraria.ModLoader;
using Terraria.ModLoader.Engine;
using Terraria.Social.Base;
using Terraria.Social.Steam;
using Terraria.Social.WeGame;

namespace Terraria.Social;

public static class SocialAPI
{
	private static SocialMode _mode;

	public static Terraria.Social.Base.FriendsSocialModule Friends;

	public static Terraria.Social.Base.AchievementsSocialModule Achievements;

	public static Terraria.Social.Base.CloudSocialModule Cloud;

	public static Terraria.Social.Base.NetSocialModule Network;

	public static Terraria.Social.Base.OverlaySocialModule Overlay;

	public static Terraria.Social.Base.WorkshopSocialModule Workshop;

	public static ServerJoinRequestsManager JoinRequests;

	public static Terraria.Social.Base.PlatformSocialModule Platform;

	private static List<ISocialModule> _modules;

	private static bool _steamAPILoaded;

	private static bool _wegameAPILoaded;

	public static SocialMode Mode => _mode;

	public static void Initialize(SocialMode? mode = null)
	{
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		if (!mode.HasValue)
		{
			mode = SocialMode.None;
			if (!Main.dedServ && !SteamedWraps.FamilyShared)
			{
				if (InstallVerifier.DistributionPlatform == DistributionPlatform.Steam)
				{
					mode = SocialMode.Steam;
				}
			}
			else if (Program.LaunchParameters.ContainsKey("-steam"))
			{
				mode = SocialMode.Steam;
			}
		}
		_mode = mode.Value;
		_modules = new List<ISocialModule>();
		JoinRequests = new ServerJoinRequestsManager();
		Main.OnTickForInternalCodeOnly += JoinRequests.Update;
		switch (Mode)
		{
		case SocialMode.Steam:
			LoadSteam();
			break;
		case SocialMode.WeGame:
			LoadWeGame();
			break;
		}
		SteamedWraps.Initialize();
		Logging.tML.Info((object)("Current 1281930 Workshop Folder Directory: " + WorkshopHelper.GetWorkshopFolder(Terraria.ModLoader.Engine.Steam.TMLAppID_t)));
	}

	public static void Shutdown()
	{
		_modules.Reverse();
		foreach (ISocialModule module in _modules)
		{
			module.Shutdown();
		}
	}

	private static T LoadModule<T>() where T : ISocialModule, new()
	{
		T val = new T();
		_modules.Add(val);
		return val;
	}

	private static T LoadModule<T>(T module) where T : ISocialModule
	{
		_modules.Add(module);
		return module;
	}

	private static void LoadDiscord()
	{
		if (!Main.dedServ && (ReLogic.OS.Platform.IsWindows || Environment.Is64BitOperatingSystem))
		{
			_ = Environment.Is64BitProcess;
		}
	}

	internal static void LoadSteam()
	{
		if (_steamAPILoaded)
		{
			return;
		}
		LoadModule<Terraria.Social.Steam.CoreSocialModule>();
		Friends = LoadModule<Terraria.Social.Steam.FriendsSocialModule>();
		Cloud = LoadModule<Terraria.Social.Steam.CloudSocialModule>();
		Overlay = LoadModule<Terraria.Social.Steam.OverlaySocialModule>();
		Workshop = LoadModule<Terraria.Social.Steam.WorkshopSocialModule>();
		Platform = LoadModule<Terraria.Social.Steam.PlatformSocialModule>();
		if (Main.dedServ)
		{
			Network = LoadModule<Terraria.Social.Steam.NetServerSocialModule>();
		}
		else
		{
			Network = LoadModule<Terraria.Social.Steam.NetClientSocialModule>();
		}
		foreach (ISocialModule module in _modules)
		{
			module.Initialize();
		}
		_steamAPILoaded = true;
		Terraria.ModLoader.Engine.Steam.PostInit();
	}

	private static void LoadWeGame()
	{
		if (_wegameAPILoaded)
		{
			return;
		}
		LoadModule<Terraria.Social.WeGame.CoreSocialModule>();
		Cloud = LoadModule<Terraria.Social.WeGame.CloudSocialModule>();
		Friends = LoadModule<Terraria.Social.WeGame.FriendsSocialModule>();
		Overlay = LoadModule<Terraria.Social.WeGame.OverlaySocialModule>();
		if (Main.dedServ)
		{
			Network = LoadModule<Terraria.Social.WeGame.NetServerSocialModule>();
		}
		else
		{
			Network = LoadModule<Terraria.Social.WeGame.NetClientSocialModule>();
		}
		WeGameHelper.WriteDebugString("LoadWeGame modules");
		foreach (ISocialModule module in _modules)
		{
			module.Initialize();
		}
		_wegameAPILoaded = true;
	}
}
