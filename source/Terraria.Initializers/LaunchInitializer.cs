using System;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using Terraria.GameContent.UI.States;
using Terraria.IO;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.Core;
using Terraria.ModLoader.Engine;
using Terraria.ModLoader.UI;
using Terraria.Social;

namespace Terraria.Initializers;

public static class LaunchInitializer
{
	public static void LoadParameters(Main game)
	{
		LoadSharedParameters(game);
		if (Main.dedServ)
		{
			LoadServerParameters(game);
		}
		LoadClientParameters(game);
	}

	private static void LoadSharedParameters(Main game)
	{
		string path;
		if ((path = TryParameter("-loadlib")) != null)
		{
			game.loadLib(path);
		}
		string s;
		if ((s = TryParameter("-p", "-port")) != null && int.TryParse(s, out var result))
		{
			Netplay.ListenPort = result;
		}
		string modPack = TryParameter("-modpack");
		if (modPack != null)
		{
			ModOrganizer.commandLineModPack = modPack;
		}
	}

	private static void LoadClientParameters(Main game)
	{
		string iP;
		if ((iP = TryParameter("-j", "-join")) != null)
		{
			game.AutoJoin(iP, TryParameter("-plr", "-player"));
		}
		string arg;
		if ((arg = TryParameter("-pass", "-password")) != null)
		{
			Netplay.ServerPassword = Main.ConvertFromSafeArgument(arg);
			game.AutoPass();
		}
		if (HasParameter("-host"))
		{
			game.AutoHost();
		}
		if (!HasParameter("-skipselect"))
		{
			return;
		}
		string playerName = null;
		string worldName = null;
		string skipSelectArgs = TryParameter("-skipselect");
		if (skipSelectArgs != null)
		{
			Match i = new Regex("(?<name>.*?):(?<val>.*)").Match(skipSelectArgs);
			if (i.Success)
			{
				playerName = i.Groups["name"].Value;
				worldName = i.Groups["val"].Value;
			}
		}
		Terraria.ModLoader.ModLoader.OnSuccessfulLoad = (Action)Delegate.Combine(Terraria.ModLoader.ModLoader.OnSuccessfulLoad, (Action)delegate
		{
			WorldGen.clearWorld();
			Main.LoadPlayers();
			(Main.PlayerList.FirstOrDefault((PlayerFileData x) => x.Name == playerName) ?? Main.PlayerList[0]).SetAsActive();
			Main.LoadWorlds();
			(Main.WorldList.FirstOrDefault((WorldFileData x) => x.Name == worldName) ?? Main.WorldList[0]).SetAsActive();
			if (!UIWorldSelect.CanWorldBePlayed(Main.ActiveWorldFileData))
			{
				throw new Exception($"The selected character {playerName} can not be used with the selected world {worldName}.\n" + "This could be due to mismatched Journey Mode or other mod specific changes.Check in Game Menus for more information.");
			}
			WorldGen.playWorld();
		});
	}

	private static void LoadServerParameters(Main game)
	{
		if (!OperatingSystem.IsWindows())
		{
			if (TryParameter("-forcepriority") != null)
			{
				Logging.tML.Warn((object)"The -forcepriority command line parameter has no effect on non-Windows OS. You'll have to set priority manually within your OS.");
			}
		}
		else
		{
			try
			{
				string s;
				if ((s = TryParameter("-forcepriority")) != null)
				{
					Process currentProcess = Process.GetCurrentProcess();
					if (int.TryParse(s, out var result))
					{
						switch (result)
						{
						case 0:
							currentProcess.PriorityClass = ProcessPriorityClass.RealTime;
							break;
						case 1:
							currentProcess.PriorityClass = ProcessPriorityClass.High;
							break;
						case 2:
							currentProcess.PriorityClass = ProcessPriorityClass.AboveNormal;
							break;
						case 3:
							currentProcess.PriorityClass = ProcessPriorityClass.Normal;
							break;
						case 4:
							currentProcess.PriorityClass = ProcessPriorityClass.BelowNormal;
							break;
						case 5:
							currentProcess.PriorityClass = ProcessPriorityClass.Idle;
							break;
						default:
							currentProcess.PriorityClass = ProcessPriorityClass.High;
							break;
						}
					}
					else
					{
						currentProcess.PriorityClass = ProcessPriorityClass.High;
					}
				}
				else
				{
					Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;
				}
			}
			catch
			{
			}
		}
		string value;
		if ((value = TryParameter("-maxplayers", "-players")) != null)
		{
			int num = Convert.ToInt32(value);
			if (num <= 255 && num >= 1)
			{
				game.SetNetPlayers(num);
			}
		}
		string arg;
		if ((arg = TryParameter("-pass", "-password")) != null)
		{
			Netplay.ServerPassword = Main.ConvertFromSafeArgument(arg);
		}
		string s2;
		if ((s2 = TryParameter("-lang")) != null && int.TryParse(s2, out var result2))
		{
			LanguageManager.Instance.SetLanguage(result2);
		}
		if ((s2 = TryParameter("-language")) != null)
		{
			LanguageManager.Instance.SetLanguage(s2);
		}
		string publish = TryParameter("-publish");
		if (publish != null)
		{
			UIModSourceItem.PublishModCommandLine(publish);
		}
		string install = TryParameter("-install");
		if (install != null)
		{
			FileAssociationSupport.HandleFileAssociation(install);
		}
		string worldName;
		if ((worldName = TryParameter("-worldname")) != null)
		{
			game.SetWorldName(worldName);
		}
		string newMOTD;
		if ((newMOTD = TryParameter("-motd")) != null)
		{
			game.NewMOTD(newMOTD);
		}
		string modPath = TryParameter("-modpath");
		if (modPath != null)
		{
			ModOrganizer.modPath = modPath;
		}
		if (HasParameter("-showserverconsole"))
		{
			Main.showServerConsole = true;
		}
		string banFilePath;
		if ((banFilePath = TryParameter("-banlist")) != null)
		{
			Netplay.BanFilePath = banFilePath;
		}
		if (HasParameter("-autoshutdown"))
		{
			game.EnableAutoShutdown();
		}
		if (HasParameter("-secure"))
		{
			Netplay.SpamCheck = true;
		}
		string serverWorldRollbacks;
		if ((serverWorldRollbacks = TryParameter("-worldrollbackstokeep")) != null)
		{
			game.setServerWorldRollbacks(serverWorldRollbacks);
		}
		string worldSize;
		if ((worldSize = TryParameter("-autocreate")) != null)
		{
			game.autoCreate(worldSize);
		}
		if (HasParameter("-noupnp"))
		{
			Netplay.UseUPNP = false;
		}
		if (HasParameter("-experimental"))
		{
			Main.UseExperimentalFeatures = true;
		}
		string world;
		if ((world = TryParameter("-world")) != null)
		{
			game.SetWorld(world, cloud: false);
		}
		else if (SocialAPI.Mode == SocialMode.Steam && (world = TryParameter("-cloudworld")) != null)
		{
			game.SetWorld(world, cloud: true);
		}
		string configPath;
		if ((configPath = TryParameter("-config")) != null)
		{
			game.LoadDedConfig(configPath);
		}
		string autogenSeedName;
		if ((autogenSeedName = TryParameter("-seed")) != null)
		{
			Main.AutogenSeedName = autogenSeedName;
		}
	}

	private static bool HasParameter(params string[] keys)
	{
		for (int i = 0; i < keys.Length; i++)
		{
			if (Program.LaunchParameters.ContainsKey(keys[i]))
			{
				return true;
			}
		}
		return false;
	}

	public static string TryParameter(params string[] keys)
	{
		for (int i = 0; i < keys.Length; i++)
		{
			if (Program.LaunchParameters.TryGetValue(keys[i], out var value))
			{
				if (value == null)
				{
					return "";
				}
				return value;
			}
		}
		return null;
	}
}
