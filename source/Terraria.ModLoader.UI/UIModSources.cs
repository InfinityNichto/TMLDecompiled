using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader.Core;
using Terraria.UI;
using Terraria.UI.Gamepad;

namespace Terraria.ModLoader.UI;

internal class UIModSources : UIState, IHaveBackButtonCommand
{
	private readonly List<UIModSourceItem> _items = new List<UIModSourceItem>();

	private UIList _modList;

	private float modListViewPosition;

	private bool _updateNeeded;

	private UIElement _uIElement;

	private UIPanel _uIPanel;

	private UIInputTextField filterTextBox;

	private UILoaderAnimatedImage _uiLoader;

	private UIElement _links;

	private CancellationTokenSource _cts;

	private static bool dotnetSDKFound;

	public UIState PreviousUIState { get; set; }

	public override void OnInitialize()
	{
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_033d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0342: Unknown result type (might be due to invalid IL or missing references)
		//IL_050b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0511: Unknown result type (might be due to invalid IL or missing references)
		//IL_0515: Unknown result type (might be due to invalid IL or missing references)
		//IL_051b: Unknown result type (might be due to invalid IL or missing references)
		//IL_051f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0525: Unknown result type (might be due to invalid IL or missing references)
		//IL_0529: Unknown result type (might be due to invalid IL or missing references)
		//IL_052f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0575: Unknown result type (might be due to invalid IL or missing references)
		//IL_057b: Unknown result type (might be due to invalid IL or missing references)
		//IL_057f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0585: Unknown result type (might be due to invalid IL or missing references)
		//IL_0589: Unknown result type (might be due to invalid IL or missing references)
		//IL_058f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0593: Unknown result type (might be due to invalid IL or missing references)
		//IL_0599: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_05fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0603: Unknown result type (might be due to invalid IL or missing references)
		//IL_0607: Unknown result type (might be due to invalid IL or missing references)
		//IL_060d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0611: Unknown result type (might be due to invalid IL or missing references)
		//IL_0617: Unknown result type (might be due to invalid IL or missing references)
		//IL_0674: Unknown result type (might be due to invalid IL or missing references)
		//IL_067a: Unknown result type (might be due to invalid IL or missing references)
		//IL_067e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0684: Unknown result type (might be due to invalid IL or missing references)
		//IL_0688: Unknown result type (might be due to invalid IL or missing references)
		//IL_068e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0692: Unknown result type (might be due to invalid IL or missing references)
		//IL_0698: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_06fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0700: Unknown result type (might be due to invalid IL or missing references)
		//IL_0704: Unknown result type (might be due to invalid IL or missing references)
		//IL_070a: Unknown result type (might be due to invalid IL or missing references)
		//IL_070e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0714: Unknown result type (might be due to invalid IL or missing references)
		_uIElement = new UIElement
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
		_uIPanel = new UIPanel
		{
			Width = 
			{
				Percent = 1f
			},
			Height = 
			{
				Pixels = -65f,
				Percent = 1f
			},
			BackgroundColor = UICommon.MainPanelBackground,
			PaddingTop = 0f
		};
		_uIElement.Append(_uIPanel);
		_uiLoader = new UILoaderAnimatedImage(0.5f, 0.5f);
		UIElement upperMenuContainer = new UIElement
		{
			Width = 
			{
				Percent = 1f
			},
			Height = 
			{
				Pixels = 82f
			},
			Top = 
			{
				Pixels = 10f
			}
		};
		UIPanel filterTextBoxBackground = new UIPanel
		{
			Top = 
			{
				Percent = 0f
			},
			Left = 
			{
				Pixels = -135f,
				Percent = 1f
			},
			Width = 
			{
				Pixels = 135f
			},
			Height = 
			{
				Pixels = 32f
			}
		};
		filterTextBoxBackground.OnRightClick += delegate
		{
			filterTextBox.Text = "";
		};
		upperMenuContainer.Append(filterTextBoxBackground);
		filterTextBox = new UIInputTextField(Language.GetTextValue("tModLoader.ModsTypeToSearch"))
		{
			Top = 
			{
				Pixels = 5f
			},
			Left = 
			{
				Pixels = -125f,
				Percent = 1f
			},
			Width = 
			{
				Pixels = 120f
			},
			Height = 
			{
				Pixels = 20f
			}
		};
		filterTextBox.OnRightClick += delegate
		{
			filterTextBox.Text = "";
		};
		filterTextBox.OnTextChange += delegate
		{
			_updateNeeded = true;
		};
		upperMenuContainer.Append(filterTextBox);
		_uIPanel.Append(upperMenuContainer);
		_modList = new UIList
		{
			Width = 
			{
				Pixels = -25f,
				Percent = 1f
			},
			Height = 
			{
				Pixels = -134f,
				Percent = 1f
			},
			Top = 
			{
				Pixels = 134f
			},
			ListPadding = 5f
		};
		_uIPanel.Append(_modList);
		UIScrollbar uIScrollbar = new UIScrollbar
		{
			Height = 
			{
				Pixels = -134f,
				Percent = 1f
			},
			Top = 
			{
				Pixels = 134f
			},
			HAlign = 1f
		}.WithView(100f, 1000f);
		_uIPanel.Append(uIScrollbar);
		_modList.SetScrollbar(uIScrollbar);
		UITextPanel<string> uIHeaderTextPanel = new UITextPanel<string>(Language.GetTextValue("tModLoader.MenuModSources"), 0.8f, large: true)
		{
			HAlign = 0.5f,
			Top = 
			{
				Pixels = -35f
			},
			BackgroundColor = UICommon.DefaultUIBlue
		}.WithPadding(15f);
		_uIElement.Append(uIHeaderTextPanel);
		_links = new UIPanel
		{
			Width = 
			{
				Percent = 1f
			},
			Height = 
			{
				Pixels = 78f
			},
			Top = 
			{
				Pixels = 46f
			}
		};
		_links.SetPadding(8f);
		_uIPanel.Append(_links);
		AddLink(Language.GetText("tModLoader.VersionUpgrade"), 0.5f, 0f, "https://github.com/tModLoader/tModLoader/wiki/Update-Migration-Guide");
		AddLink(Language.GetText("tModLoader.WikiLink"), 0f, 0.5f, "https://github.com/tModLoader/tModLoader/wiki/");
		string exampleModBranch = (BuildInfo.IsStable ? "stable" : (BuildInfo.IsPreview ? "preview" : "1.4.4"));
		AddLink(Language.GetText("tModLoader.ExampleModLink"), 1f, 0.5f, "https://github.com/tModLoader/tModLoader/tree/" + exampleModBranch + "/ExampleMod");
		string docsURL = (BuildInfo.IsStable ? "stable" : "preview");
		AddLink(Language.GetText("tModLoader.DocumentationLink"), 0f, 1f, "https://docs.tmodloader.net/docs/" + docsURL + "/annotated.html");
		AddLink(Language.GetText("tModLoader.DiscordLink"), 1f, 1f, "https://tmodloader.net/discord");
		UIAutoScaleTextTextPanel<string> buttonBA = new UIAutoScaleTextTextPanel<string>(Language.GetTextValue("tModLoader.MSBuildAll"))
		{
			Width = 
			{
				Pixels = -10f,
				Percent = 1f / 3f
			},
			Height = 
			{
				Pixels = 40f
			},
			VAlign = 1f,
			Top = 
			{
				Pixels = -65f
			}
		};
		buttonBA.WithFadedMouseOver();
		buttonBA.OnLeftClick += BuildMods;
		UIAutoScaleTextTextPanel<string> uIAutoScaleTextTextPanel = new UIAutoScaleTextTextPanel<string>(Language.GetTextValue("tModLoader.MSBuildReloadAll"));
		uIAutoScaleTextTextPanel.CopyStyle(buttonBA);
		uIAutoScaleTextTextPanel.HAlign = 0.5f;
		uIAutoScaleTextTextPanel.WithFadedMouseOver();
		uIAutoScaleTextTextPanel.OnLeftClick += BuildAndReload;
		UIAutoScaleTextTextPanel<string> buttonCreateMod = new UIAutoScaleTextTextPanel<string>(Language.GetTextValue("tModLoader.MSCreateMod"));
		buttonCreateMod.CopyStyle(buttonBA);
		buttonCreateMod.HAlign = 1f;
		buttonCreateMod.Top.Pixels = -20f;
		buttonCreateMod.WithFadedMouseOver();
		buttonCreateMod.OnLeftClick += ButtonCreateMod_OnClick;
		_uIElement.Append(buttonCreateMod);
		UIAutoScaleTextTextPanel<string> buttonB = new UIAutoScaleTextTextPanel<string>(Language.GetTextValue("UI.Back"));
		buttonB.CopyStyle(buttonBA);
		buttonB.Top.Pixels = -20f;
		buttonB.WithFadedMouseOver();
		buttonB.OnLeftClick += BackClick;
		_uIElement.Append(buttonB);
		UIAutoScaleTextTextPanel<string> buttonOS = new UIAutoScaleTextTextPanel<string>(Language.GetTextValue("tModLoader.MSOpenSources"));
		buttonOS.CopyStyle(buttonB);
		buttonOS.HAlign = 0.5f;
		buttonOS.WithFadedMouseOver();
		buttonOS.OnLeftClick += OpenSources;
		_uIElement.Append(buttonOS);
		Append(_uIElement);
	}

	private void AddLink(LocalizedText text, float hAlign, float vAlign, string url)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		UIText link = new UIText(text)
		{
			TextColor = Color.White,
			HAlign = hAlign,
			VAlign = vAlign
		};
		link.OnMouseOver += delegate
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			SoundEngine.PlaySound(in SoundID.MenuTick);
			link.TextColor = Main.OurFavoriteColor;
		};
		link.OnMouseOut += delegate
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			link.TextColor = Color.White;
		};
		link.OnLeftClick += delegate
		{
			SoundEngine.PlaySound(in SoundID.MenuOpen);
			Utils.OpenToURL(url);
		};
		_links.Append(link);
	}

	private void ButtonCreateMod_OnClick(UIMouseEvent evt, UIElement listeningElement)
	{
		SoundEngine.PlaySound(11);
		Main.menuMode = 10025;
	}

	private void BackClick(UIMouseEvent evt, UIElement listeningElement)
	{
		((IHaveBackButtonCommand)this).HandleBackButtonUsage();
	}

	private void OpenSources(UIMouseEvent evt, UIElement listeningElement)
	{
		SoundEngine.PlaySound(10);
		try
		{
			Directory.CreateDirectory(ModCompile.ModSourcePath);
			Utils.OpenFolder(ModCompile.ModSourcePath);
		}
		catch (Exception e)
		{
			Logging.tML.Error((object)e);
		}
	}

	private void BuildMods(UIMouseEvent evt, UIElement listeningElement)
	{
		SoundEngine.PlaySound(10);
		if (_modList.Count > 0)
		{
			Interface.buildMod.BuildAll(reload: false);
		}
	}

	private void BuildAndReload(UIMouseEvent evt, UIElement listeningElement)
	{
		SoundEngine.PlaySound(10);
		if (_modList.Count > 0)
		{
			Interface.buildMod.BuildAll(reload: true);
		}
	}

	public override void Draw(SpriteBatch spriteBatch)
	{
		UILinkPointNavigator.Shortcuts.BackButtonCommand = 7;
		base.Draw(spriteBatch);
	}

	public override void OnActivate()
	{
		_cts = new CancellationTokenSource();
		ModCompile.UpdateReferencesFolder();
		_uIPanel.Append(_uiLoader);
		_modList.Clear();
		_items.Clear();
		if (!ShowInfoMessages())
		{
			Populate();
		}
	}

	public override void OnDeactivate()
	{
		_cts?.Cancel(throwOnFirstException: false);
		_cts?.Dispose();
		_cts = null;
		modListViewPosition = _modList.ViewPosition;
	}

	private bool ShowInfoMessages()
	{
		if (!ModLoader.SeenFirstLaunchModderWelcomeMessage)
		{
			ShowWelcomeMessage("tModLoader.ViewOnGitHub", "https://github.com/tModLoader/tModLoader/wiki/Update-Migration-Guide");
			ModLoader.SeenFirstLaunchModderWelcomeMessage = true;
			Main.SaveSettings();
			return true;
		}
		if (!IsCompatibleDotnetSdkAvailable())
		{
			if (IsRunningInSandbox())
			{
				Utils.ShowFancyErrorMessage(Language.GetTextValue("tModLoader.DevModsInSandbox"), 888, PreviousUIState);
			}
			else
			{
				ShowWelcomeMessage("tModLoader.DownloadNetSDK", "https://github.com/tModLoader/tModLoader/wiki/tModLoader-guide-for-developers#developing-with-tmodloader", 888, PreviousUIState);
			}
			return true;
		}
		return false;
	}

	private void ShowWelcomeMessage(string altButtonTextKey, string url, int gotoMenu = 10001, UIState state = null)
	{
		Interface.infoMessage.Show(Language.GetTextValue("tModLoader.MSFirstLaunchModderWelcomeMessage"), gotoMenu, state, Language.GetTextValue(altButtonTextKey), delegate
		{
			Utils.OpenToURL(url);
		});
	}

	private static string GetCommandToFindPathOfExecutable()
	{
		if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
		{
			return "where";
		}
		if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) || RuntimeInformation.IsOSPlatform(OSPlatform.OSX) || RuntimeInformation.IsOSPlatform(OSPlatform.FreeBSD))
		{
			return "which";
		}
		Logging.tML.Debug((object)"Getting command for finding path of the executable failed due to an unsupported operating system");
		return null;
	}

	private static IEnumerable<string> GetPossibleSystemDotnetPaths()
	{
		string cmd = GetCommandToFindPathOfExecutable();
		if (cmd != null)
		{
			yield return Process.Start(new ProcessStartInfo
			{
				FileName = cmd,
				Arguments = "dotnet",
				UseShellExecute = false,
				RedirectStandardOutput = true
			}).StandardOutput.ReadToEnd().Trim();
		}
		string pathsFile = "/etc/paths.d/dotnet";
		if (File.Exists(pathsFile))
		{
			string contents = File.ReadAllText(pathsFile).Trim();
			Logging.tML.Debug((object)("Reading " + pathsFile + ": " + contents));
			yield return contents + "/dotnet";
		}
		string dotnetRoot = Environment.GetEnvironmentVariable("DOTNET_ROOT");
		if (dotnetRoot != null)
		{
			Logging.tML.Debug((object)("Found env var DOTNET_ROOT: " + dotnetRoot));
			yield return dotnetRoot + "/dotnet";
		}
		yield return "/usr/bin/dotnet";
	}

	private static string GetSystemDotnetPath()
	{
		try
		{
			string path = GetPossibleSystemDotnetPaths().FirstOrDefault(File.Exists);
			if (path != null)
			{
				Logging.tML.Debug((object)("System dotnet install located at: " + path));
				return path;
			}
		}
		catch (Exception)
		{
		}
		Logging.tML.Debug((object)"Finding dotnet on PATH failed");
		return null;
	}

	private static bool IsCompatibleDotnetSdkAvailable()
	{
		if (dotnetSDKFound)
		{
			return true;
		}
		try
		{
			string output = Process.Start(new ProcessStartInfo
			{
				FileName = (GetSystemDotnetPath() ?? "dotnet"),
				Arguments = "--list-sdks",
				UseShellExecute = false,
				RedirectStandardOutput = true
			}).StandardOutput.ReadToEnd();
			Logging.tML.Info((object)("\n" + output));
			string[] array = output.Split('\n');
			foreach (string line in array)
			{
				if (new Version(new Regex("([0-9.]+).*").Match(line).Groups[1].Value).Major == Environment.Version.Major)
				{
					dotnetSDKFound = true;
					return true;
				}
			}
		}
		catch (Exception e)
		{
			Logging.tML.Debug((object)"'dotnet --list-sdks' check failed: ", e);
		}
		return dotnetSDKFound;
	}

	private static bool IsRunningInSandbox()
	{
		if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("FLATPAK_SANDBOX_DIR")))
		{
			Logging.tML.Debug((object)"Flatpak sandbox detected");
			return true;
		}
		return false;
	}

	internal void Populate()
	{
		Task.Run(delegate
		{
			string[] array = ModCompile.FindModSources();
			IReadOnlyList<LocalMod> source = ModOrganizer.FindDevFolderMods();
			string[] array2 = array;
			foreach (string sourcePath in array2)
			{
				LocalMod builtMod = source.SingleOrDefault((LocalMod m) => m.Name == Path.GetFileName(sourcePath));
				_items.Add(new UIModSourceItem(sourcePath, builtMod));
			}
			_updateNeeded = true;
		});
	}

	public override void Update(GameTime gameTime)
	{
		base.Update(gameTime);
		if (_updateNeeded)
		{
			_updateNeeded = false;
			_uIPanel.RemoveChild(_uiLoader);
			_modList.Clear();
			string filter = filterTextBox.Text;
			_modList.AddRange(_items.Where((UIModSourceItem item) => filter.Length <= 0 || item.modName.IndexOf(filter, StringComparison.OrdinalIgnoreCase) != -1));
			Recalculate();
			_modList.ViewPosition = modListViewPosition;
		}
	}
}
