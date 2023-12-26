using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader.Config.UI;
using Terraria.ModLoader.Core;
using Terraria.ModLoader.UI.DownloadManager;
using Terraria.ModLoader.UI.ModBrowser;
using Terraria.Social.Steam;

namespace Terraria.ModLoader.UI;

internal static class Interface
{
	internal const int modsMenuID = 10000;

	internal const int modSourcesID = 10001;

	internal const int loadModsID = 10002;

	internal const int buildModID = 10003;

	internal const int errorMessageID = 10005;

	internal const int reloadModsID = 10006;

	internal const int modBrowserID = 10007;

	internal const int modInfoID = 10008;

	internal const int updateMessageID = 10012;

	internal const int infoMessageID = 10013;

	internal const int infoMessageDelayedID = 10014;

	internal const int modPacksMenuID = 10016;

	internal const int tModLoaderSettingsID = 10017;

	internal const int extractModID = 10019;

	internal const int downloadProgressID = 10020;

	internal const int progressID = 10023;

	internal const int modConfigID = 10024;

	internal const int createModID = 10025;

	internal const int exitID = 10026;

	internal const int modConfigListID = 10027;

	internal static UIMods modsMenu = new UIMods();

	internal static UILoadMods loadMods = new UILoadMods();

	internal static UIModSources modSources = new UIModSources();

	internal static UIBuildMod buildMod = new UIBuildMod();

	internal static UIErrorMessage errorMessage = new UIErrorMessage();

	internal static UIModBrowser modBrowser = new UIModBrowser(WorkshopBrowserModule.Instance);

	internal static UIModInfo modInfo = new UIModInfo();

	internal static UIForcedDelayInfoMessage infoMessageDelayed = new UIForcedDelayInfoMessage();

	internal static UIUpdateMessage updateMessage = new UIUpdateMessage();

	internal static UIInfoMessage infoMessage = new UIInfoMessage();

	internal static UIModPacks modPacksMenu = new UIModPacks();

	internal static UIExtractMod extractMod = new UIExtractMod();

	internal static UIModConfig modConfig = new UIModConfig();

	internal static UIModConfigList modConfigList = new UIModConfigList();

	internal static UICreateMod createMod = new UICreateMod();

	internal static UIProgress progress = new UIProgress();

	internal static UIDownloadProgress downloadProgress = new UIDownloadProgress();

	internal static void AddMenuButtons(Main main, int selectedMenu, string[] buttonNames, float[] buttonScales, ref int offY, ref int spacing, ref int buttonIndex, ref int numButtons)
	{
	}

	internal static void ResetData()
	{
		modBrowser.Reset();
		GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced);
	}

	internal static void ModLoaderMenus(Main main, int selectedMenu, string[] buttonNames, float[] buttonScales, int[] buttonVerticalSpacing, ref int offY, ref int spacing, ref int numButtons, ref bool backButtonDown)
	{
		if (Main.menuMode == 10002)
		{
			if (ModLoader.ShowFirstLaunchWelcomeMessage)
			{
				ModLoader.ShowFirstLaunchWelcomeMessage = false;
				infoMessage.Show(Language.GetTextValue("tModLoader.FirstLaunchWelcomeMessage"), Main.menuMode);
			}
			else if (SteamedWraps.FamilyShared && !ModLoader.WarnedFamilyShare)
			{
				ModLoader.WarnedFamilyShare = true;
				infoMessage.Show(Language.GetTextValue("tModLoader.SteamFamilyShareWarning"), Main.menuMode);
			}
			else if (ModLoader.ShowWhatsNew)
			{
				ModLoader.ShowWhatsNew = false;
				if (File.Exists("RecentGitHubCommits.txt"))
				{
					bool LastLaunchedShaInRecentGitHubCommits = false;
					StringBuilder messages = new StringBuilder();
					foreach (string item2 in File.ReadLines("RecentGitHubCommits.txt"))
					{
						string[] parts = item2.Split(' ', 2);
						if (parts.Length == 2)
						{
							string sha = parts[0];
							string message2 = parts[1];
							if (sha != ModLoader.LastLaunchedTModLoaderAlphaSha)
							{
								messages.Append("\n  " + message2);
							}
							if (sha == ModLoader.LastLaunchedTModLoaderAlphaSha)
							{
								LastLaunchedShaInRecentGitHubCommits = true;
								break;
							}
						}
					}
					if (LastLaunchedShaInRecentGitHubCommits)
					{
						infoMessage.Show(Language.GetTextValue("tModLoader.WhatsNewMessage") + messages.ToString(), Main.menuMode, null, Language.GetTextValue("tModLoader.ViewOnGitHub"), delegate
						{
							Utils.OpenToURL("https://github.com/tModLoader/tModLoader/compare/" + ModLoader.LastLaunchedTModLoaderAlphaSha + "...1.4");
						});
					}
				}
			}
			else if (ModLoader.PreviewFreezeNotification)
			{
				ModLoader.PreviewFreezeNotification = false;
				ModLoader.LastPreviewFreezeNotificationSeen = BuildInfo.tMLVersion.MajorMinor();
				infoMessage.Show(Language.GetTextValue("tModLoader.WelcomeMessagePreview"), Main.menuMode, null, Language.GetTextValue("tModLoader.ModsMoreInfo"), delegate
				{
					Utils.OpenToURL("https://github.com/tModLoader/tModLoader/wiki/tModLoader-Release-Cycle#14");
				});
				Main.SaveSettings();
			}
			else if (!ModLoader.DownloadedDependenciesOnStartup)
			{
				ModLoader.DownloadedDependenciesOnStartup = true;
				List<string> missingDeps = ModOrganizer.IdentifyMissingWorkshopDependencies().ToList();
				bool num = missingDeps.Count != 0;
				string message = string.Concat(ModOrganizer.DetectModChangesForInfoMessage(), "\n", string.Concat(missingDeps)).Trim('\n');
				string cancelButton = (num ? Language.GetTextValue("tModLoader.ContinueAnyway") : null);
				string continueButton = (num ? Language.GetTextValue("tModLoader.InstallDependencies") : "");
				Action downloadAction = delegate
				{
					HashSet<ModDownloadItem> hashSet = new HashSet<ModDownloadItem>();
					foreach (string current in missingDeps)
					{
						if (!WorkshopHelper.TryGetModDownloadItem(current, out var item))
						{
							Logging.tML.Error((object)("Could not find required mod dependency on Workshop: " + current));
						}
						else
						{
							hashSet.Add(item);
						}
					}
					UIModBrowser.DownloadMods(hashSet, 10002);
				};
				if (!string.IsNullOrWhiteSpace(message))
				{
					Logging.tML.Info((object)("Mod Changes since last launch:\n" + message));
					infoMessage.Show(message, Main.menuMode, null, continueButton, downloadAction, cancelButton);
				}
			}
		}
		if (Main.menuMode == 10000)
		{
			Main.MenuUI.SetState(modsMenu);
			Main.menuMode = 888;
		}
		else if (Main.menuMode == 10001)
		{
			Main.menuMode = 888;
			Main.MenuUI.SetState(modSources);
		}
		else if (Main.menuMode == 10025)
		{
			Main.MenuUI.SetState(createMod);
			Main.menuMode = 888;
		}
		else if (Main.menuMode == 10002)
		{
			Main.menuMode = 888;
			Main.MenuUI.SetState(loadMods);
		}
		else if (Main.menuMode == 10003)
		{
			Main.MenuUI.SetState(buildMod);
			Main.menuMode = 888;
		}
		else if (Main.menuMode == 10005)
		{
			Main.MenuUI.SetState(errorMessage);
			Main.menuMode = 888;
		}
		else if (Main.menuMode == 10006)
		{
			ModLoader.Reload();
		}
		else if (Main.menuMode == 10007)
		{
			Main.MenuUI.SetState(modBrowser);
			Main.menuMode = 888;
		}
		else if (Main.menuMode == 10008)
		{
			Main.MenuUI.SetState(modInfo);
			Main.menuMode = 888;
		}
		else if (Main.menuMode == 10012)
		{
			Main.MenuUI.SetState(updateMessage);
			Main.menuMode = 888;
		}
		else if (Main.menuMode == 10013)
		{
			Main.MenuUI.SetState(infoMessage);
			Main.menuMode = 888;
		}
		else if (Main.menuMode == 10016)
		{
			Main.MenuUI.SetState(modPacksMenu);
			Main.menuMode = 888;
		}
		else if (Main.menuMode == 10019)
		{
			Main.MenuUI.SetState(extractMod);
			Main.menuMode = 888;
		}
		else if (Main.menuMode == 10023)
		{
			Main.MenuUI.SetState(progress);
			Main.menuMode = 888;
		}
		else if (Main.menuMode == 10020)
		{
			Main.MenuUI.SetState(downloadProgress);
			Main.menuMode = 888;
		}
		else if (Main.menuMode == 10017)
		{
			offY = 210;
			spacing = 42;
			numButtons = 10;
			buttonVerticalSpacing[numButtons - 1] = 18;
			for (int i = 0; i < numButtons; i++)
			{
				buttonScales[i] = 0.75f;
			}
			int buttonIndex = 0;
			buttonNames[buttonIndex] = (ModNet.downloadModsFromServers ? Language.GetTextValue("tModLoader.DownloadFromServersYes") : Language.GetTextValue("tModLoader.DownloadFromServersNo"));
			if (selectedMenu == buttonIndex)
			{
				SoundEngine.PlaySound(in SoundID.MenuTick);
				ModNet.downloadModsFromServers = !ModNet.downloadModsFromServers;
			}
			buttonIndex++;
			buttonNames[buttonIndex] = (ModNet.onlyDownloadSignedMods ? Language.GetTextValue("tModLoader.DownloadSignedYes") : Language.GetTextValue("tModLoader.DownloadSignedNo"));
			if (selectedMenu == buttonIndex)
			{
				SoundEngine.PlaySound(in SoundID.MenuTick);
				ModNet.onlyDownloadSignedMods = !ModNet.onlyDownloadSignedMods;
			}
			buttonIndex++;
			buttonNames[buttonIndex] = (ModLoader.autoReloadAndEnableModsLeavingModBrowser ? Language.GetTextValue("tModLoader.AutomaticallyReloadAndEnableModsLeavingModBrowserYes") : Language.GetTextValue("tModLoader.AutomaticallyReloadAndEnableModsLeavingModBrowserNo"));
			if (selectedMenu == buttonIndex)
			{
				SoundEngine.PlaySound(in SoundID.MenuTick);
				ModLoader.autoReloadAndEnableModsLeavingModBrowser = !ModLoader.autoReloadAndEnableModsLeavingModBrowser;
			}
			buttonIndex++;
			buttonNames[buttonIndex] = (ModLoader.autoReloadRequiredModsLeavingModsScreen ? Language.GetTextValue("tModLoader.AutomaticallyReloadRequiredModsLeavingModsScreenYes") : Language.GetTextValue("tModLoader.AutomaticallyReloadRequiredModsLeavingModsScreenNo"));
			if (selectedMenu == buttonIndex)
			{
				SoundEngine.PlaySound(in SoundID.MenuTick);
				ModLoader.autoReloadRequiredModsLeavingModsScreen = !ModLoader.autoReloadRequiredModsLeavingModsScreen;
			}
			buttonIndex++;
			buttonNames[buttonIndex] = Language.GetTextValue("tModLoader.RemoveForcedMinimumZoom" + (ModLoader.removeForcedMinimumZoom ? "Yes" : "No"));
			if (selectedMenu == buttonIndex)
			{
				SoundEngine.PlaySound(in SoundID.MenuTick);
				ModLoader.removeForcedMinimumZoom = !ModLoader.removeForcedMinimumZoom;
			}
			buttonIndex++;
			buttonNames[buttonIndex] = Language.GetTextValue($"tModLoader.AttackSpeedScalingTooltipVisibility{ModLoader.attackSpeedScalingTooltipVisibility}");
			if (selectedMenu == buttonIndex)
			{
				SoundEngine.PlaySound(in SoundID.MenuTick);
				ModLoader.attackSpeedScalingTooltipVisibility = (ModLoader.attackSpeedScalingTooltipVisibility + 1) % 3;
			}
			buttonIndex++;
			buttonNames[buttonIndex] = Language.GetTextValue("tModLoader.ShowMemoryEstimates" + (ModLoader.showMemoryEstimates ? "Yes" : "No"));
			if (selectedMenu == buttonIndex)
			{
				SoundEngine.PlaySound(in SoundID.MenuTick);
				ModLoader.showMemoryEstimates = !ModLoader.showMemoryEstimates;
			}
			buttonIndex++;
			buttonNames[buttonIndex] = Language.GetTextValue("tModLoader.ShowModMenuNotifications" + (ModLoader.notifyNewMainMenuThemes ? "Yes" : "No"));
			if (selectedMenu == buttonIndex)
			{
				SoundEngine.PlaySound(in SoundID.MenuTick);
				ModLoader.notifyNewMainMenuThemes = !ModLoader.notifyNewMainMenuThemes;
			}
			buttonIndex++;
			buttonNames[buttonIndex] = Language.GetTextValue("tModLoader.ShowNewUpdatedModsInfo" + (ModLoader.showNewUpdatedModsInfo ? "Yes" : "No"));
			if (selectedMenu == buttonIndex)
			{
				SoundEngine.PlaySound(in SoundID.MenuTick);
				ModLoader.showNewUpdatedModsInfo = !ModLoader.showNewUpdatedModsInfo;
			}
			buttonIndex++;
			buttonNames[buttonIndex] = Lang.menu[5].Value;
			if ((selectedMenu == buttonIndex) | backButtonDown)
			{
				backButtonDown = false;
				Main.menuMode = 11;
				SoundEngine.PlaySound(11);
			}
		}
		else if (Main.menuMode == 10024)
		{
			Main.MenuUI.SetState(modConfig);
			Main.menuMode = 888;
		}
		else if (Main.menuMode == 10027)
		{
			Main.MenuUI.SetState(modConfigList);
			Main.menuMode = 888;
		}
		else if (Main.menuMode == 10026)
		{
			Environment.Exit(0);
		}
	}

	internal static void ServerModMenu(out bool reloadMods)
	{
		bool exit = false;
		reloadMods = false;
		while (!exit)
		{
			Console.WriteLine("Terraria Server " + Main.versionNumber2 + " - " + ModLoader.versionedName);
			Console.WriteLine();
			LocalMod[] mods = ModOrganizer.FindMods(logDuplicates: true);
			for (int i = 0; i < mods.Length; i++)
			{
				Console.Write(i + 1 + "\t\t" + mods[i].DisplayName);
				Console.WriteLine(" (" + (mods[i].Enabled ? "enabled" : "disabled") + ")");
			}
			if (mods.Length == 0)
			{
				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.WriteLine(Language.GetTextValue("tModLoader.ModsNotFoundServer", ModLoader.ModPath));
				Console.ResetColor();
			}
			Console.WriteLine("e\t\t" + Language.GetTextValue("tModLoader.ModsEnableAll"));
			Console.WriteLine("d\t\t" + Language.GetTextValue("tModLoader.ModsDisableAll"));
			Console.WriteLine("r\t\t" + Language.GetTextValue("tModLoader.ModsReloadAndReturn"));
			Console.WriteLine(Language.GetTextValue("tModLoader.AskForModIndex"));
			Console.WriteLine();
			Console.WriteLine(Language.GetTextValue("tModLoader.AskForCommand"));
			string command = Console.ReadLine();
			if (command == null)
			{
				command = "";
			}
			command = command.ToLower();
			Console.Clear();
			switch (command)
			{
			case "e":
			{
				LocalMod[] array = mods;
				for (int j = 0; j < array.Length; j++)
				{
					array[j].Enabled = true;
				}
				continue;
			}
			case "d":
			{
				LocalMod[] array = mods;
				for (int j = 0; j < array.Length; j++)
				{
					array[j].Enabled = false;
				}
				continue;
			}
			case "r":
				reloadMods = true;
				exit = true;
				continue;
			}
			if (!int.TryParse(command, out var value) || value <= 0 || value > mods.Length)
			{
				continue;
			}
			LocalMod mod = mods[value - 1];
			mod.Enabled = !mod.Enabled;
			if (mod.Enabled)
			{
				List<string> missingRefs = new List<string>();
				EnableDepsRecursive(mod, mods, missingRefs);
				if (missingRefs.Any())
				{
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine(Language.GetTextValue("tModLoader.ModDependencyModsNotFound", string.Join(", ", missingRefs)) + "\n");
					Console.ResetColor();
				}
			}
		}
	}

	private static void EnableDepsRecursive(LocalMod mod, LocalMod[] mods, List<string> missingRefs)
	{
		string[] array = mod.properties.modReferences.Select((BuildProperties.ModReference x) => x.mod).ToArray();
		foreach (string name in array)
		{
			LocalMod dep = mods.FirstOrDefault((LocalMod x) => x.Name == name);
			if (dep == null)
			{
				missingRefs.Add(name);
				continue;
			}
			EnableDepsRecursive(dep, mods, missingRefs);
			if (!dep.Enabled)
			{
				Console.ForegroundColor = ConsoleColor.Green;
				Console.WriteLine("Automatically enabling " + dep.DisplayName + " required by " + mod.DisplayName);
				Console.ResetColor();
			}
			dep.Enabled = !dep.Enabled;
		}
	}

	internal static void ServerModBrowserMenu()
	{
	}
}
