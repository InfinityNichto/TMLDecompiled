using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using ReLogic.OS;
using Steamworks;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader.Core;
using Terraria.ModLoader.UI.ModBrowser;
using Terraria.Social.Steam;
using Terraria.UI;

namespace Terraria.ModLoader.UI;

internal class UIModSourceItem : UIPanel
{
	private readonly string _mod;

	internal readonly string modName;

	private readonly Asset<Texture2D> _dividerTexture;

	private readonly UIText _modName;

	private readonly UIAutoScaleTextTextPanel<string> needRebuildButton;

	private readonly LocalMod _builtMod;

	private bool _upgradePotentialChecked;

	private Stopwatch uploadTimer;

	private int contextButtonsLeft = -26;

	public UIModSourceItem(string mod, LocalMod builtMod)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_0158: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0201: Unknown result type (might be due to invalid IL or missing references)
		//IL_0207: Unknown result type (might be due to invalid IL or missing references)
		//IL_020b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0211: Unknown result type (might be due to invalid IL or missing references)
		//IL_0215: Unknown result type (might be due to invalid IL or missing references)
		//IL_021b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0318: Unknown result type (might be due to invalid IL or missing references)
		//IL_031e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0322: Unknown result type (might be due to invalid IL or missing references)
		//IL_0328: Unknown result type (might be due to invalid IL or missing references)
		//IL_032c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0332: Unknown result type (might be due to invalid IL or missing references)
		//IL_0336: Unknown result type (might be due to invalid IL or missing references)
		//IL_033c: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b7: Unknown result type (might be due to invalid IL or missing references)
		_mod = mod;
		BorderColor = new Color(89, 116, 213) * 0.7f;
		_dividerTexture = UICommon.DividerTexture;
		Height.Pixels = 90f;
		Width.Percent = 1f;
		SetPadding(6f);
		string addendum = (Path.GetFileName(mod).Contains(" ") ? ("  [c/FF0000:" + Language.GetTextValue("tModLoader.MSModSourcesCantHaveSpaces") + "]") : "");
		modName = Path.GetFileName(mod);
		_modName = new UIText(modName + addendum)
		{
			Left = 
			{
				Pixels = 10f
			},
			Top = 
			{
				Pixels = 5f
			}
		};
		Append(_modName);
		UIAutoScaleTextTextPanel<string> buildButton = new UIAutoScaleTextTextPanel<string>(Language.GetTextValue("tModLoader.MSBuild"))
		{
			Width = 
			{
				Pixels = 100f
			},
			Height = 
			{
				Pixels = 36f
			},
			Left = 
			{
				Pixels = 10f
			},
			Top = 
			{
				Pixels = 40f
			}
		}.WithFadedMouseOver();
		buildButton.PaddingTop -= 2f;
		buildButton.PaddingBottom -= 2f;
		buildButton.OnLeftClick += BuildMod;
		Append(buildButton);
		UIAutoScaleTextTextPanel<string> buildReloadButton = new UIAutoScaleTextTextPanel<string>(Language.GetTextValue("tModLoader.MSBuildReload"));
		buildReloadButton.CopyStyle(buildButton);
		buildReloadButton.Width.Pixels = 200f;
		buildReloadButton.Left.Pixels = 150f;
		buildReloadButton.WithFadedMouseOver();
		buildReloadButton.OnLeftClick += BuildAndReload;
		Append(buildReloadButton);
		_builtMod = builtMod;
		if (builtMod != null && LocalizationLoader.changedMods.Contains(modName))
		{
			needRebuildButton = new UIAutoScaleTextTextPanel<string>(Language.GetTextValue("tModLoader.MSRebuildRequired"));
			needRebuildButton.CopyStyle(buildReloadButton);
			needRebuildButton.Width.Pixels = 180f;
			needRebuildButton.Left.Pixels = 360f;
			needRebuildButton.BackgroundColor = Color.Red;
			Append(needRebuildButton);
		}
		else if (builtMod != null)
		{
			UIAutoScaleTextTextPanel<string> publishButton = new UIAutoScaleTextTextPanel<string>(Language.GetTextValue("tModLoader.MSPublish"));
			publishButton.CopyStyle(buildReloadButton);
			publishButton.Width.Pixels = 100f;
			publishButton.Left.Pixels = 390f;
			publishButton.WithFadedMouseOver();
			if (builtMod.properties.side == ModSide.Server)
			{
				publishButton.OnLeftClick += PublishServerSideMod;
				Append(publishButton);
			}
			else if (builtMod.Enabled)
			{
				publishButton.OnLeftClick += PublishMod;
				Append(publishButton);
			}
		}
		base.OnLeftDoubleClick += BuildAndReload;
		string modFolderName = Path.GetFileName(_mod);
		string csprojFile = Path.Combine(_mod, modFolderName + ".csproj");
		if (!File.Exists(csprojFile))
		{
			return;
		}
		UIHoverImage openCSProjButton = new UIHoverImage(UICommon.CopyCodeButtonTexture, Language.GetTextValue("tModLoader.MSOpenCSProj"))
		{
			Left = 
			{
				Pixels = contextButtonsLeft,
				Percent = 1f
			},
			Top = 
			{
				Pixels = 4f
			}
		};
		openCSProjButton.OnLeftClick += delegate
		{
			if (Platform.IsWindows)
			{
				Process.Start(new ProcessStartInfo("explorer", csprojFile)
				{
					UseShellExecute = true
				});
			}
			else
			{
				Process.Start(new ProcessStartInfo(csprojFile)
				{
					UseShellExecute = true
				});
			}
		};
		Append(openCSProjButton);
		contextButtonsLeft -= 26;
	}

	protected override void DrawChildren(SpriteBatch spriteBatch)
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		base.DrawChildren(spriteBatch);
		UIAutoScaleTextTextPanel<string> uIAutoScaleTextTextPanel = needRebuildButton;
		if (uIAutoScaleTextTextPanel != null && uIAutoScaleTextTextPanel.IsMouseHovering)
		{
			UICommon.DrawHoverStringInBounds(spriteBatch, Language.GetTextValue("tModLoader.MSLocalizationFilesChangedCantPublish"), GetOuterDimensions().ToRectangle());
		}
	}

	protected override void DrawSelf(SpriteBatch spriteBatch)
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		base.DrawSelf(spriteBatch);
		CalculatedStyle innerDimensions = GetInnerDimensions();
		Vector2 drawPos = default(Vector2);
		((Vector2)(ref drawPos))._002Ector(innerDimensions.X + 5f, innerDimensions.Y + 30f);
		spriteBatch.Draw(_dividerTexture.Value, drawPos, (Rectangle?)null, Color.White, 0f, Vector2.Zero, new Vector2((innerDimensions.Width - 10f) / 8f, 1f), (SpriteEffects)0, 0f);
		if (_upgradePotentialChecked)
		{
			return;
		}
		_upgradePotentialChecked = true;
		string modFolderName = Path.GetFileName(_mod);
		string csprojFile = Path.Combine(_mod, modFolderName + ".csproj");
		bool projNeedsUpdate = false;
		if (!File.Exists(csprojFile) || Interface.createMod.CsprojUpdateNeeded(File.ReadAllText(csprojFile)))
		{
			Asset<Texture2D> icon2 = UICommon.ButtonExclamationTexture;
			UIHoverImage upgradeCSProjButton = new UIHoverImage(icon2, Language.GetTextValue("tModLoader.MSUpgradeCSProj"))
			{
				Left = 
				{
					Pixels = contextButtonsLeft,
					Percent = 1f
				},
				Top = 
				{
					Pixels = 4f
				}
			};
			upgradeCSProjButton.OnLeftClick += delegate
			{
				File.WriteAllText(csprojFile, Interface.createMod.GetModCsproj(modFolderName));
				string text2 = Path.Combine(_mod, "Properties");
				string path = Path.Combine(text2, "AssemblyInfo.cs");
				if (File.Exists(path))
				{
					File.Delete(path);
				}
				try
				{
					string path2 = Path.Combine(_mod, "obj");
					if (Directory.Exists(path2))
					{
						Directory.Delete(path2, recursive: true);
					}
					string path3 = Path.Combine(_mod, "bin");
					if (Directory.Exists(path3))
					{
						Directory.Delete(path3, recursive: true);
					}
				}
				catch (Exception)
				{
				}
				Directory.CreateDirectory(text2);
				File.WriteAllText(Path.Combine(text2, "launchSettings.json"), Interface.createMod.GetLaunchSettings());
				SoundEngine.PlaySound(in SoundID.MenuOpen);
				Main.menuMode = 10001;
				upgradeCSProjButton.Remove();
				_upgradePotentialChecked = false;
			};
			Append(upgradeCSProjButton);
			contextButtonsLeft -= 26;
			projNeedsUpdate = true;
		}
		string[] files = Directory.GetFiles(_mod, "*.lang", SearchOption.AllDirectories);
		if (files.Length != 0)
		{
			Asset<Texture2D> icon = UICommon.ButtonExclamationTexture;
			UIHoverImage upgradeLangFilesButton = new UIHoverImage(icon, Language.GetTextValue("tModLoader.MSUpgradeLangFiles"))
			{
				Left = 
				{
					Pixels = contextButtonsLeft,
					Percent = 1f
				},
				Top = 
				{
					Pixels = 4f
				}
			};
			upgradeLangFilesButton.OnLeftClick += delegate
			{
				string[] array = files;
				for (int i = 0; i < array.Length; i++)
				{
					LocalizationLoader.UpgradeLangFile(array[i], modName);
				}
				upgradeLangFilesButton.Remove();
			};
			Append(upgradeLangFilesButton);
			contextButtonsLeft -= 26;
		}
		if (Platform.IsWindows && !projNeedsUpdate)
		{
			UIHoverImage portModButton = new UIHoverImage(UICommon.ButtonExclamationTexture, Language.GetTextValue("tModLoader.MSPortToLatest"))
			{
				Left = 
				{
					Pixels = contextButtonsLeft,
					Percent = 1f
				},
				Top = 
				{
					Pixels = 4f
				}
			};
			portModButton.OnLeftClick += delegate
			{
				string fileName = Path.GetFileName(_mod);
				string text = Path.Combine(_mod, fileName + ".csproj");
				string arguments = "\"" + text + "\"";
				string fileName2 = Path.Combine(Path.GetDirectoryName(Path.GetFileName(Assembly.GetExecutingAssembly().Location)), "tModPorter", "tModPorter.bat");
				Process.Start(new ProcessStartInfo
				{
					FileName = fileName2,
					Arguments = arguments,
					UseShellExecute = true
				});
			};
			Append(portModButton);
			contextButtonsLeft -= 26;
		}
	}

	public override void MouseOver(UIMouseEvent evt)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		base.MouseOver(evt);
		BackgroundColor = UICommon.DefaultUIBlue;
		BorderColor = new Color(89, 116, 213);
	}

	public override void MouseOut(UIMouseEvent evt)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		base.MouseOut(evt);
		BackgroundColor = new Color(63, 82, 151) * 0.7f;
		BorderColor = new Color(89, 116, 213) * 0.7f;
	}

	public override int CompareTo(object obj)
	{
		if (!(obj is UIModSourceItem uIModSourceItem))
		{
			return base.CompareTo(obj);
		}
		if (uIModSourceItem._builtMod == null && _builtMod == null)
		{
			return _modName.Text.CompareTo(uIModSourceItem._modName.Text);
		}
		if (uIModSourceItem._builtMod == null)
		{
			return -1;
		}
		if (_builtMod == null)
		{
			return 1;
		}
		return uIModSourceItem._builtMod.lastModified.CompareTo(_builtMod.lastModified);
	}

	private void BuildMod(UIMouseEvent evt, UIElement listeningElement)
	{
		SoundEngine.PlaySound(10);
		Interface.buildMod.Build(_mod, reload: false);
	}

	private void BuildAndReload(UIMouseEvent evt, UIElement listeningElement)
	{
		SoundEngine.PlaySound(10);
		Interface.buildMod.Build(_mod, reload: true);
	}

	private void PublishMod(UIMouseEvent evt, UIElement listeningElement)
	{
		SoundEngine.PlaySound(10);
		try
		{
			Mod result;
			if (!SteamedWraps.SteamClient)
			{
				Utils.ShowFancyErrorMessage(Language.GetTextValue("tModLoader.SteamPublishingLimit"), 10001);
			}
			else if (!ModLoader.TryGetMod(_builtMod.Name, out result))
			{
				if (!_builtMod.Enabled)
				{
					_builtMod.Enabled = true;
				}
				Main.menuMode = 10006;
				ModLoader.OnSuccessfulLoad = (Action)Delegate.Combine(ModLoader.OnSuccessfulLoad, (Action)delegate
				{
					Main.QueueMainThreadAction(delegate
					{
						PublishMod(null, null);
					});
				});
			}
			else
			{
				string icon = Path.Combine(_mod, "icon_workshop.png");
				if (!File.Exists(icon))
				{
					icon = Path.Combine(_mod, "icon.png");
				}
				WorkshopHelper.PublishMod(_builtMod, icon);
			}
		}
		catch (WebException e)
		{
			UIModBrowser.LogModBrowserException(e, 10001);
		}
	}

	private void PublishServerSideMod(UIMouseEvent evt, UIElement listeningElement)
	{
		SoundEngine.PlaySound(10);
		try
		{
			if (!SteamedWraps.SteamClient)
			{
				Utils.ShowFancyErrorMessage(Language.GetTextValue("tModLoader.SteamPublishingLimit"), 10001);
				return;
			}
			Process.Start(new ProcessStartInfo
			{
				UseShellExecute = true,
				FileName = Process.GetCurrentProcess().MainModule.FileName,
				Arguments = "tModLoader.dll -server -steam -publish " + _builtMod.modFile.path.Remove(_builtMod.modFile.path.LastIndexOf(".tmod"))
			}).WaitForExit();
		}
		catch (WebException e)
		{
			UIModBrowser.LogModBrowserException(e, 10001);
		}
	}

	internal static void PublishModCommandLine(string modName)
	{
		try
		{
			TmodFile modFile = new TmodFile(Path.Combine(ModLoader.ModPath, modName + ".tmod"));
			LocalMod localMod;
			using (modFile.Open())
			{
				localMod = new LocalMod(modFile);
			}
			string icon = Path.Combine(ModCompile.ModSourcePath, modName, "icon_workshop.png");
			if (!File.Exists(icon))
			{
				icon = Path.Combine(ModCompile.ModSourcePath, modName, "icon.png");
			}
			WorkshopHelper.PublishMod(localMod, icon);
		}
		catch (Exception ex)
		{
			Console.WriteLine("Something went wrong with command line mod publishing.");
			Console.WriteLine(ex.ToString());
			SteamAPI.Shutdown();
			Environment.Exit(1);
		}
		Console.WriteLine("exiting ");
		SteamAPI.Shutdown();
		Environment.Exit(0);
	}
}
