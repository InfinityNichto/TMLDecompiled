using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Ionic.Zip;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.GameContent.UI.States;
using Terraria.Localization;
using Terraria.ModLoader.Config;
using Terraria.ModLoader.Core;
using Terraria.UI;
using Terraria.UI.Gamepad;
using Terraria.Utilities;

namespace Terraria.ModLoader.UI;

internal class UIModPacks : UIState, IHaveBackButtonCommand
{
	internal const string MODPACK_REGEX = "[^a-zA-Z0-9_.-]+";

	internal static string ModPacksDirectory = Path.Combine(ModLoader.ModPath, "ModPacks");

	private UIList _modPacks;

	private UILoaderAnimatedImage _uiLoader;

	private UIPanel _scrollPanel;

	private CancellationTokenSource _cts;

	private static UIVirtualKeyboard _virtualKeyboard;

	public UIState PreviousUIState { get; set; }

	private static UIVirtualKeyboard VirtualKeyboard => _virtualKeyboard ?? (_virtualKeyboard = new UIVirtualKeyboard(Language.GetTextValue("tModLoader.ModPacksEnterModPackName"), "", SaveModPack, delegate
	{
		Main.menuMode = 10016;
	}));

	public static string ModPackModsPath(string packName)
	{
		return Path.Combine(ModPacksDirectory, packName, "Mods");
	}

	public static string ModPackConfigPath(string packName)
	{
		return Path.Combine(ModPacksDirectory, packName, "ModConfigs");
	}

	public override void OnInitialize()
	{
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0198: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0216: Unknown result type (might be due to invalid IL or missing references)
		//IL_021c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0220: Unknown result type (might be due to invalid IL or missing references)
		//IL_0226: Unknown result type (might be due to invalid IL or missing references)
		//IL_022a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0230: Unknown result type (might be due to invalid IL or missing references)
		//IL_0234: Unknown result type (might be due to invalid IL or missing references)
		//IL_023a: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02db: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0327: Unknown result type (might be due to invalid IL or missing references)
		//IL_034d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0353: Unknown result type (might be due to invalid IL or missing references)
		//IL_0357: Unknown result type (might be due to invalid IL or missing references)
		//IL_035d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0361: Unknown result type (might be due to invalid IL or missing references)
		//IL_0367: Unknown result type (might be due to invalid IL or missing references)
		//IL_036b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0371: Unknown result type (might be due to invalid IL or missing references)
		UIElement uIElement = new UIElement
		{
			Width = 
			{
				Percent = 0.8f
			},
			MaxWidth = UICommon.MaxPanelWidth,
			Top = 
			{
				Pixels = 220f
			},
			Height = 
			{
				Pixels = -220f,
				Percent = 1f
			},
			HAlign = 0.5f
		};
		_uiLoader = new UILoaderAnimatedImage(0.5f, 0.5f);
		_scrollPanel = new UIPanel
		{
			Width = 
			{
				Percent = 1f
			},
			Height = 
			{
				Pixels = -65f,
				Percent = 0.9f
			},
			BackgroundColor = UICommon.MainPanelBackground
		};
		uIElement.Append(_scrollPanel);
		_modPacks = new UIList
		{
			Width = 
			{
				Pixels = -25f,
				Percent = 1f
			},
			Height = 
			{
				Percent = 0.9f
			},
			ListPadding = 5f
		};
		_scrollPanel.Append(_modPacks);
		UIScrollbar uIScrollbar = new UIScrollbar
		{
			Height = 
			{
				Percent = 0.9f
			},
			HAlign = 1f
		}.WithView(100f, 1000f);
		_scrollPanel.Append(uIScrollbar);
		_modPacks.SetScrollbar(uIScrollbar);
		UITextPanel<LocalizedText> titleTextPanel = new UITextPanel<LocalizedText>(Language.GetText("tModLoader.ModPacksHeader"), 0.8f, large: true)
		{
			HAlign = 0.5f,
			Top = 
			{
				Pixels = -35f
			},
			BackgroundColor = UICommon.DefaultUIBlue
		}.WithPadding(15f);
		uIElement.Append(titleTextPanel);
		UIAutoScaleTextTextPanel<LocalizedText> folderButton = new UIAutoScaleTextTextPanel<LocalizedText>(Language.GetText("tModLoader.OpenModPackFolder"))
		{
			Width = new StyleDimension(-10f, 0.5f),
			Height = 
			{
				Pixels = 40f
			},
			VAlign = 0.9f,
			HAlign = 0f,
			Top = 
			{
				Pixels = -20f
			}
		}.WithFadedMouseOver();
		folderButton.OnLeftClick += OpenFolder;
		uIElement.Append(folderButton);
		UIAutoScaleTextTextPanel<LocalizedText> backButton = new UIAutoScaleTextTextPanel<LocalizedText>(Language.GetText("UI.Back"))
		{
			Width = new StyleDimension(-10f, 0.5f),
			Height = 
			{
				Pixels = 40f
			},
			VAlign = 1f,
			HAlign = 0f,
			Top = 
			{
				Pixels = -20f
			}
		}.WithFadedMouseOver();
		backButton.OnLeftClick += BackClick;
		uIElement.Append(backButton);
		UIAutoScaleTextTextPanel<LocalizedText> saveNewButton = new UIAutoScaleTextTextPanel<LocalizedText>(Language.GetText("tModLoader.ModPacksSaveEnabledAsNewPack"));
		saveNewButton.CopyStyle(backButton);
		saveNewButton.TextColor = Color.Green;
		saveNewButton.VAlign = 1f;
		saveNewButton.HAlign = 1f;
		saveNewButton.WithFadedMouseOver();
		saveNewButton.OnLeftClick += SaveNewModList;
		uIElement.Append(saveNewButton);
		Append(uIElement);
	}

	private static void SaveNewModList(UIMouseEvent evt, UIElement listeningElement)
	{
		SoundEngine.PlaySound(11);
		VirtualKeyboard.Text = "";
		Main.MenuUI.SetState(VirtualKeyboard);
		Main.menuMode = 888;
	}

	public static void SaveModPack(string filename)
	{
		if (!IsValidModpackName(filename))
		{
			VirtualKeyboard.Text = SanitizeModpackName(filename);
			return;
		}
		string modsPath = ModPackModsPath(filename);
		SaveSnapshot(ModPackConfigPath(filename), modsPath);
		Main.menuMode = 10016;
	}

	private void BackClick(UIMouseEvent evt, UIElement listeningElement)
	{
		SoundEngine.PlaySound(11);
		((IHaveBackButtonCommand)this).HandleBackButtonUsage();
	}

	private void OpenFolder(UIMouseEvent evt, UIElement listeningElement)
	{
		Utils.OpenFolder(ModPacksDirectory);
	}

	public override void Draw(SpriteBatch spriteBatch)
	{
		base.Draw(spriteBatch);
		UILinkPointNavigator.Shortcuts.BackButtonCommand = 7;
	}

	internal static string SanitizeModpackName(string name)
	{
		return Regex.Replace(name, "[^a-zA-Z0-9_.-]+", string.Empty, RegexOptions.Compiled);
	}

	internal static bool IsValidModpackName(string name)
	{
		if (!Regex.Match(name, "[^a-zA-Z0-9_.-]+", RegexOptions.Compiled).Success)
		{
			return name.IndexOfAny(Path.GetInvalidFileNameChars()) < 0;
		}
		return false;
	}

	public override void OnDeactivate()
	{
		_cts?.Cancel(throwOnFirstException: false);
		_cts?.Dispose();
		_cts = null;
	}

	public override void OnActivate()
	{
		_cts = new CancellationTokenSource();
		_scrollPanel.Append(_uiLoader);
		_modPacks.Clear();
		Task.Run(delegate
		{
			Directory.CreateDirectory(ModPacksDirectory);
			string[] directories = Directory.GetDirectories(ModPacksDirectory, "*", SearchOption.TopDirectoryOnly);
			string[] files = Directory.GetFiles(ModPacksDirectory, "*.json", SearchOption.TopDirectoryOnly);
			List<UIElement> ModPacksToAdd = new List<UIElement>();
			foreach (string current in files.Concat(directories))
			{
				try
				{
					if (!IsValidModpackName(Path.GetFileNameWithoutExtension(current)))
					{
						throw new Exception();
					}
					if (Directory.Exists(current))
					{
						ModPacksToAdd.Add(LoadModernModPack(current));
					}
					else
					{
						ModPacksToAdd.Add(LoadLegacyModPack(current));
					}
				}
				catch
				{
					UIAutoScaleTextTextPanel<string> item = new UIAutoScaleTextTextPanel<string>(Language.GetTextValue("tModLoader.ModPackMalformed", Path.GetFileName(current)))
					{
						Width = 
						{
							Percent = 1f
						},
						Height = 
						{
							Pixels = 50f,
							Percent = 0f
						}
					};
					ModPacksToAdd.Add(item);
				}
			}
			Main.QueueMainThreadAction(delegate
			{
				_modPacks.AddRange(ModPacksToAdd);
				_scrollPanel.RemoveChild(_uiLoader);
			});
		});
	}

	public UIModPackItem LoadModernModPack(string folderPath)
	{
		string[] modPackMods = JsonConvert.DeserializeObject<string[]>(File.ReadAllText(Path.Combine(folderPath, "Mods", "enabled.json")));
		if (modPackMods == null)
		{
			Utils.LogAndConsoleInfoMessage("No contents in enabled.json at: " + folderPath + ". Is this correct?");
			modPackMods = new string[0];
		}
		LocalMod[] localMods = ModOrganizer.FindMods();
		return new UIModPackItem(folderPath, modPackMods, legacy: false, localMods);
	}

	public UIModPackItem LoadLegacyModPack(string jsonPath)
	{
		string[] modPackMods = JsonConvert.DeserializeObject<string[]>(File.ReadAllText(jsonPath));
		LocalMod[] localMods = ModOrganizer.FindMods();
		return new UIModPackItem(Path.GetFileNameWithoutExtension(jsonPath), modPackMods, legacy: true, localMods);
	}

	public static void SaveSnapshot(string configsPath, string modsPath)
	{
		if (!Directory.Exists(ConfigManager.ModConfigPath))
		{
			Directory.CreateDirectory(ConfigManager.ModConfigPath);
		}
		Directory.CreateDirectory(configsPath);
		Directory.CreateDirectory(modsPath);
		Directory.EnumerateFiles(ConfigManager.ModConfigPath);
		File.Copy(Path.Combine(ModOrganizer.modPath, "enabled.json"), Path.Combine(modsPath, "enabled.json"), overwrite: true);
		File.WriteAllText(Path.Combine(modsPath, "tmlversion.txt"), BuildInfo.tMLVersion.ToString());
		List<string> workshopIds = new List<string>();
		Mod[] mods = ModLoader.Mods;
		foreach (Mod mod in mods)
		{
			if (mod.File != null)
			{
				if (ModOrganizer.TryReadManifest(ModOrganizer.GetParentDir(mod.File.path), out var info))
				{
					workshopIds.Add(info.workshopEntryId.ToString());
				}
				if (mod.File.path != Path.Combine(modsPath, mod.Name + ".tmod"))
				{
					File.Copy(mod.File.path, Path.Combine(modsPath, mod.Name + ".tmod"), overwrite: true);
				}
			}
		}
		File.WriteAllLines(Path.Combine(modsPath, "install.txt"), workshopIds);
	}

	public static void ExportSnapshot(string modPackName)
	{
		string instancePath = Path.Combine(Directory.GetCurrentDirectory(), modPackName);
		Directory.CreateDirectory(instancePath);
		Directory.CreateDirectory(Path.Combine(instancePath, "SaveData"));
		string sourcePath = ModPackModsPath(modPackName);
		string configPath = ConfigManager.ModConfigPath;
		FileUtilities.CopyFolder(sourcePath, Path.Combine(instancePath, "SaveData", "Mods"));
		FileUtilities.CopyFolder(configPath, Path.Combine(instancePath, "SaveData", "ModConfigs"));
		File.WriteAllText(Path.Combine(instancePath, "cli-argsConfig.txt"), "-tmlsavedirectory " + Path.Combine(instancePath, "SaveData") + "\n-steamworkshopfolder none");
		Logging.tML.Info((object)("Exported instance of Frozen Mod Pack " + modPackName + " to " + instancePath));
		Utils.OpenFolder(instancePath);
	}

	public static void ExtractTmlInstall(string instancePath)
	{
		string zipFilePath = Path.Combine(instancePath, "tModLoader.zip");
		ZipFile zip = ZipFile.Read(zipFilePath);
		try
		{
			zip.ExtractAll(instancePath);
		}
		finally
		{
			((IDisposable)zip)?.Dispose();
		}
		File.Delete(zipFilePath);
	}
}
