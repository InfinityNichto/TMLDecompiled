using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader.Config;
using Terraria.ModLoader.Core;
using Terraria.ModLoader.UI.ModBrowser;
using Terraria.Social.Base;
using Terraria.UI;
using Terraria.UI.Chat;

namespace Terraria.ModLoader.UI;

internal class UIModItem : UIPanel
{
	private const float PADDING = 5f;

	private float left2ndLine;

	private UIImage _moreInfoButton;

	private UIImage _modIcon;

	private UIImageFramed updatedModDot;

	private Version previousVersionHint;

	private UIHoverImage _keyImage;

	private UIImage _configButton;

	private UIText _modName;

	private UIModStateText _uiModStateText;

	private UIAutoScaleTextTextPanel<string> tMLUpdateRequired;

	private UIImage _modReferenceIcon;

	private UIImage _translationModIcon;

	private UIImage _deleteModButton;

	private UIAutoScaleTextTextPanel<string> _dialogYesButton;

	private UIAutoScaleTextTextPanel<string> _dialogNoButton;

	private UIText _dialogText;

	private UIImage _blockInput;

	private UIPanel _deleteModDialog;

	private readonly LocalMod _mod;

	private bool modFromLocalModFolder;

	private bool _configChangesRequireReload;

	private bool _loaded;

	private int _modIconAdjust;

	private string _tooltip;

	private string[] _modReferences;

	public readonly string DisplayNameClean;

	private string ToggleModStateText
	{
		get
		{
			if (!_mod.Enabled)
			{
				return Language.GetTextValue("tModLoader.ModsEnable");
			}
			return Language.GetTextValue("tModLoader.ModsDisable");
		}
	}

	public string ModName => _mod.Name;

	public bool NeedsReload
	{
		get
		{
			if (_mod.properties.side != ModSide.Server)
			{
				if (_mod.Enabled == _loaded)
				{
					return _configChangesRequireReload;
				}
				return true;
			}
			return false;
		}
	}

	public UIModItem(LocalMod mod)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		_mod = mod;
		BorderColor = new Color(89, 116, 213) * 0.7f;
		Height.Pixels = 90f;
		Width.Percent = 1f;
		SetPadding(6f);
		DisplayNameClean = string.Join("", from x in ChatManager.ParseMessage(_mod.DisplayName, Color.White)
			where x.GetType() == typeof(TextSnippet)
			select x.Text);
	}

	public override void OnInitialize()
	{
		//IL_0209: Unknown result type (might be due to invalid IL or missing references)
		//IL_020e: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0304: Unknown result type (might be due to invalid IL or missing references)
		//IL_0308: Unknown result type (might be due to invalid IL or missing references)
		//IL_030e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0320: Unknown result type (might be due to invalid IL or missing references)
		//IL_0327: Unknown result type (might be due to invalid IL or missing references)
		//IL_032c: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0803: Unknown result type (might be due to invalid IL or missing references)
		//IL_0808: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a9c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0abb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ac0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b1f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b18: Unknown result type (might be due to invalid IL or missing references)
		base.OnInitialize();
		string text = _mod.DisplayName + " v" + _mod.modFile.Version;
		Asset<Texture2D> modIcon = Main.Assets.Request<Texture2D>("Images/UI/DefaultResourcePackIcon", AssetRequestMode.ImmediateLoad);
		_modIconAdjust += 85;
		if (_mod.modFile.HasFile("icon.png"))
		{
			try
			{
				using (_mod.modFile.Open())
				{
					using Stream s = _mod.modFile.GetStream("icon.png");
					Asset<Texture2D> iconTexture = Main.Assets.CreateUntracked<Texture2D>(s, ".png");
					if (iconTexture.Width() == 80 && iconTexture.Height() == 80)
					{
						modIcon = iconTexture;
					}
				}
			}
			catch (Exception e2)
			{
				Logging.tML.Error((object)"Unknown error", e2);
			}
		}
		_modIcon = new UIImage(modIcon)
		{
			Left = 
			{
				Percent = 0f
			},
			Top = 
			{
				Percent = 0f
			},
			Width = 
			{
				Pixels = 80f
			},
			Height = 
			{
				Pixels = 80f
			},
			ScaleToFit = true
		};
		Append(_modIcon);
		_modName = new UIText(text)
		{
			Left = new StyleDimension(_modIconAdjust, 0f),
			Top = 
			{
				Pixels = 5f
			}
		};
		Append(_modName);
		_uiModStateText = new UIModStateText(_mod.Enabled)
		{
			Top = 
			{
				Pixels = 40f
			},
			Left = 
			{
				Pixels = _modIconAdjust
			}
		};
		_uiModStateText.OnLeftClick += ToggleEnabled;
		string updateVersion = null;
		string updateURL = "https://github.com/tModLoader/tModLoader/wiki/tModLoader-guide-for-players#beta-branches";
		Color updateColor = Color.Orange;
		if (BuildInfo.tMLVersion.MajorMinorBuild() < _mod.tModLoaderVersion.MajorMinorBuild())
		{
			updateVersion = $"v{_mod.tModLoaderVersion}";
			if (_mod.tModLoaderVersion.MajorMinor() > BuildInfo.stableVersion)
			{
				updateVersion = "Preview " + updateVersion;
			}
		}
		if (!CheckIfPublishedForThisBrowserVersion(out var modBrowserVersion))
		{
			updateVersion = $"{modBrowserVersion} v{_mod.tModLoaderVersion}";
			updateColor = Color.Yellow;
		}
		if (updateVersion != null)
		{
			tMLUpdateRequired = new UIAutoScaleTextTextPanel<string>(Language.GetTextValue("tModLoader.MBRequiresTMLUpdate", updateVersion)).WithFadedMouseOver(updateColor, updateColor * 0.7f);
			tMLUpdateRequired.BackgroundColor = updateColor * 0.7f;
			tMLUpdateRequired.Top.Pixels = 40f;
			tMLUpdateRequired.Width.Pixels = 280f;
			tMLUpdateRequired.Height.Pixels = 36f;
			tMLUpdateRequired.Left.Pixels += _uiModStateText.Width.Pixels + _uiModStateText.Left.Pixels + 5f;
			tMLUpdateRequired.OnLeftClick += delegate
			{
				Utils.OpenToURL(updateURL);
			};
			Append(tMLUpdateRequired);
		}
		else
		{
			Append(_uiModStateText);
		}
		int bottomRightRowOffset = -36;
		_moreInfoButton = new UIImage(UICommon.ButtonModInfoTexture)
		{
			Width = 
			{
				Pixels = 36f
			},
			Height = 
			{
				Pixels = 36f
			},
			Left = 
			{
				Pixels = bottomRightRowOffset,
				Precent = 1f
			},
			Top = 
			{
				Pixels = 40f
			}
		};
		_moreInfoButton.OnLeftClick += ShowMoreInfo;
		Append(_moreInfoButton);
		if (ModLoader.TryGetMod(ModName, out var loadedMod) && ConfigManager.Configs.ContainsKey(loadedMod))
		{
			bottomRightRowOffset -= 36;
			_configButton = new UIImage(UICommon.ButtonModConfigTexture)
			{
				Width = 
				{
					Pixels = 36f
				},
				Height = 
				{
					Pixels = 36f
				},
				Left = 
				{
					Pixels = (float)bottomRightRowOffset - 5f,
					Precent = 1f
				},
				Top = 
				{
					Pixels = 40f
				}
			};
			_configButton.OnLeftClick += OpenConfig;
			Append(_configButton);
			if (ConfigManager.ModNeedsReload(loadedMod))
			{
				_configChangesRequireReload = true;
			}
		}
		_modReferences = _mod.properties.modReferences.Select((BuildProperties.ModReference x) => x.mod).ToArray();
		if (_modReferences.Length != 0 && !_mod.Enabled)
		{
			Asset<Texture2D> icon2 = UICommon.ButtonExclamationTexture;
			_modReferenceIcon = new UIImage(icon2)
			{
				Left = new StyleDimension(_uiModStateText.Left.Pixels + _uiModStateText.Width.Pixels + 5f + left2ndLine, 0f),
				Top = 
				{
					Pixels = 42.5f
				}
			};
			left2ndLine += 28f;
			Append(_modReferenceIcon);
		}
		if (_mod.properties.RefNames(includeWeak: true).Any() && _mod.properties.translationMod)
		{
			Asset<Texture2D> icon = UICommon.ButtonTranslationModTexture;
			_translationModIcon = new UIImage(icon)
			{
				Left = new StyleDimension(_uiModStateText.Left.Pixels + _uiModStateText.Width.Pixels + 5f + left2ndLine, 0f),
				Top = 
				{
					Pixels = 42.5f
				}
			};
			left2ndLine += 28f;
			Append(_translationModIcon);
		}
		if (BuildInfo.IsDev && ModCompile.DeveloperMode && ModLoader.IsUnloadedModStillAlive(ModName))
		{
			_keyImage = new UIHoverImage(UICommon.ButtonErrorTexture, Language.GetTextValue("tModLoader.ModDidNotFullyUnloadWarning"))
			{
				Left = 
				{
					Pixels = (float)_modIconAdjust + 5f
				},
				Top = 
				{
					Pixels = 3f
				}
			};
			Append(_keyImage);
			_modName.Left.Pixels += _keyImage.Width.Pixels + 10f;
			_modName.Recalculate();
		}
		if (ModOrganizer.CheckStableBuildOnPreview(_mod))
		{
			_keyImage = new UIHoverImage(Main.Assets.Request<Texture2D>(TextureAssets.Item[3999].Name), Language.GetTextValue("tModLoader.ModStableOnPreviewWarning"))
			{
				Left = 
				{
					Pixels = 4f,
					Percent = 0.2f
				},
				Top = 
				{
					Pixels = 0f,
					Percent = 0.5f
				}
			};
			Append(_keyImage);
		}
		if (_mod.modFile.path.StartsWith(ModLoader.ModPath))
		{
			BackgroundColor = Color.MediumPurple * 0.7f;
			modFromLocalModFolder = true;
		}
		else
		{
			UIImage steamIcon = new UIImage(TextureAssets.Extra[243])
			{
				Left = 
				{
					Pixels = -22f,
					Percent = 1f
				}
			};
			Append(steamIcon);
		}
		if (loadedMod != null)
		{
			_loaded = true;
			int[] values = new int[6]
			{
				loadedMod.GetContent<ModItem>().Count(),
				loadedMod.GetContent<ModNPC>().Count(),
				loadedMod.GetContent<ModTile>().Count(),
				loadedMod.GetContent<ModWall>().Count(),
				loadedMod.GetContent<ModBuff>().Count(),
				loadedMod.GetContent<ModMount>().Count()
			};
			string[] localizationKeys = new string[6] { "ModsXItems", "ModsXNPCs", "ModsXTiles", "ModsXWalls", "ModsXBuffs", "ModsXMounts" };
			int xOffset = -40;
			for (int i = 0; i < values.Length; i++)
			{
				if (values[i] > 0)
				{
					_keyImage = new UIHoverImage(Main.Assets.Request<Texture2D>(TextureAssets.InfoIcon[i].Name), Language.GetTextValue("tModLoader." + localizationKeys[i], values[i]))
					{
						Left = 
						{
							Pixels = xOffset,
							Percent = 1f
						}
					};
					Append(_keyImage);
					xOffset -= 18;
				}
			}
		}
		base.OnLeftDoubleClick += delegate(UIMouseEvent e, UIElement el)
		{
			if (e.Target.GetType() != typeof(UIModStateText))
			{
				_uiModStateText.LeftClick(e);
			}
		};
		if (!_loaded)
		{
			bottomRightRowOffset -= 36;
			_deleteModButton = new UIImage(TextureAssets.Trash)
			{
				Width = 
				{
					Pixels = 36f
				},
				Height = 
				{
					Pixels = 36f
				},
				Left = 
				{
					Pixels = (float)bottomRightRowOffset - 5f,
					Precent = 1f
				},
				Top = 
				{
					Pixels = 42.5f
				}
			};
			_deleteModButton.OnLeftClick += QuickModDelete;
			Append(_deleteModButton);
		}
		(string, Version) oldModVersionData = ModOrganizer.modsThatUpdatedSinceLastLaunch.FirstOrDefault(((string ModName, Version previousVersion) x) => x.ModName == ModName);
		(string, Version) tuple = oldModVersionData;
		if (tuple.Item1 != null || tuple.Item2 != null)
		{
			previousVersionHint = oldModVersionData.Item2;
			Asset<Texture2D> toggleImage = Main.Assets.Request<Texture2D>("Images/UI/Settings_Toggle");
			UIImageFramed uIImageFramed = new UIImageFramed(toggleImage, toggleImage.Frame(2, 1, 1));
			ref StyleDimension left = ref uIImageFramed.Left;
			Rectangle val = _modName.GetInnerDimensions().ToRectangle();
			left.Pixels = ((Rectangle)(ref val)).Right + 8;
			uIImageFramed.Left.Percent = 0f;
			uIImageFramed.Top.Pixels = 5f;
			uIImageFramed.Top.Percent = 0f;
			uIImageFramed.Color = (Color)((previousVersionHint == null) ? Color.Green : new Color(6, 95, 212));
			updatedModDot = uIImageFramed;
			Append(updatedModDot);
		}
	}

	public override void Draw(SpriteBatch spriteBatch)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		_tooltip = null;
		base.Draw(spriteBatch);
		if (!string.IsNullOrEmpty(_tooltip))
		{
			Rectangle bounds = GetOuterDimensions().ToRectangle();
			bounds.Height += 16;
			UICommon.DrawHoverStringInBounds(spriteBatch, _tooltip, bounds);
		}
	}

	protected override void DrawSelf(SpriteBatch spriteBatch)
	{
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		//IL_0183: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0197: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01be: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		base.DrawSelf(spriteBatch);
		CalculatedStyle innerDimensions = GetInnerDimensions();
		Vector2 drawPos = default(Vector2);
		((Vector2)(ref drawPos))._002Ector(innerDimensions.X + 5f + (float)_modIconAdjust, innerDimensions.Y + 30f);
		spriteBatch.Draw(UICommon.DividerTexture.Value, drawPos, (Rectangle?)null, Color.White, 0f, Vector2.Zero, new Vector2((innerDimensions.Width - 10f - (float)_modIconAdjust) / 8f, 1f), (SpriteEffects)0, 0f);
		((Vector2)(ref drawPos))._002Ector(innerDimensions.X + 10f + (float)_modIconAdjust, innerDimensions.Y + 45f);
		if (_mod.properties.side != ModSide.Server && (_mod.Enabled != _loaded || _configChangesRequireReload))
		{
			drawPos += new Vector2(_uiModStateText.Width.Pixels + left2ndLine, 0f);
			Utils.DrawBorderString(spriteBatch, _configChangesRequireReload ? Language.GetTextValue("tModLoader.ModReloadForced") : Language.GetTextValue("tModLoader.ModReloadRequired"), drawPos, Color.White);
		}
		if (_mod.properties.side == ModSide.Server)
		{
			drawPos += new Vector2(90f, -2f);
			spriteBatch.Draw(UICommon.ModBrowserIconsTexture.Value, drawPos, (Rectangle?)new Rectangle(170, 102, 32, 32), Color.White, 0f, Vector2.Zero, 1f, (SpriteEffects)0, 0f);
			Rectangle val = new Rectangle((int)drawPos.X, (int)drawPos.Y, 32, 32);
			if (((Rectangle)(ref val)).Contains(Main.MouseScreen.ToPoint()))
			{
				UICommon.DrawHoverStringInBounds(spriteBatch, Language.GetTextValue("tModLoader.ModIsServerSide"));
			}
		}
		UIImage moreInfoButton = _moreInfoButton;
		if (moreInfoButton != null && moreInfoButton.IsMouseHovering)
		{
			_tooltip = Language.GetTextValue("tModLoader.ModsMoreInfo");
			return;
		}
		UIImage deleteModButton = _deleteModButton;
		if (deleteModButton != null && deleteModButton.IsMouseHovering)
		{
			_tooltip = Language.GetTextValue("UI.Delete");
			return;
		}
		UIText modName = _modName;
		if (modName != null && modName.IsMouseHovering)
		{
			LocalMod mod = _mod;
			if (mod != null && mod.properties.author.Length > 0)
			{
				_tooltip = Language.GetTextValue("tModLoader.ModsByline", _mod.properties.author);
				return;
			}
		}
		UIModStateText uiModStateText = _uiModStateText;
		if (uiModStateText != null && uiModStateText.IsMouseHovering)
		{
			_tooltip = ToggleModStateText;
			return;
		}
		UIImage configButton = _configButton;
		if (configButton != null && configButton.IsMouseHovering)
		{
			_tooltip = Language.GetTextValue("tModLoader.ModsOpenConfig");
			return;
		}
		UIImageFramed uIImageFramed = updatedModDot;
		if (uIImageFramed != null && uIImageFramed.IsMouseHovering)
		{
			if (previousVersionHint == null)
			{
				_tooltip = Language.GetTextValue("tModLoader.ModAddedSinceLastLaunchMessage");
			}
			else
			{
				_tooltip = Language.GetTextValue("tModLoader.ModUpdatedSinceLastLaunchMessage", previousVersionHint);
			}
			return;
		}
		UIAutoScaleTextTextPanel<string> uIAutoScaleTextTextPanel = tMLUpdateRequired;
		if (uIAutoScaleTextTextPanel != null && uIAutoScaleTextTextPanel.IsMouseHovering)
		{
			_tooltip = Language.GetTextValue("tModLoader.SwitchVersionInfoButton");
			return;
		}
		UIImage modReferenceIcon = _modReferenceIcon;
		if (modReferenceIcon != null && modReferenceIcon.IsMouseHovering)
		{
			string refs2 = string.Join(", ", _mod.properties.modReferences);
			_tooltip = Language.GetTextValue("tModLoader.ModDependencyTooltip", refs2);
			return;
		}
		UIImage translationModIcon = _translationModIcon;
		if (translationModIcon != null && translationModIcon.IsMouseHovering)
		{
			string refs = string.Join(", ", _mod.properties.RefNames(includeWeak: true));
			_tooltip = Language.GetTextValue("tModLoader.TranslationModTooltip", refs);
		}
	}

	public override void MouseOver(UIMouseEvent evt)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		base.MouseOver(evt);
		BackgroundColor = UICommon.DefaultUIBlue;
		BorderColor = new Color(89, 116, 213);
		if (modFromLocalModFolder)
		{
			BackgroundColor = Color.MediumPurple;
		}
	}

	public override void MouseOut(UIMouseEvent evt)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		base.MouseOut(evt);
		BackgroundColor = new Color(63, 82, 151) * 0.7f;
		BorderColor = new Color(89, 116, 213) * 0.7f;
		if (modFromLocalModFolder)
		{
			BackgroundColor = Color.MediumPurple * 0.7f;
		}
	}

	private void ToggleEnabled(UIMouseEvent evt, UIElement listeningElement)
	{
		SoundEngine.PlaySound(in SoundID.MenuTick);
		_mod.Enabled = !_mod.Enabled;
		if (_mod.Enabled)
		{
			EnableDependencies();
		}
	}

	internal void Enable()
	{
		if (!_mod.Enabled)
		{
			SoundEngine.PlaySound(in SoundID.MenuTick);
			_mod.Enabled = true;
			_uiModStateText.SetEnabled();
		}
	}

	internal void Disable()
	{
		if (_mod.Enabled)
		{
			SoundEngine.PlaySound(in SoundID.MenuTick);
			_mod.Enabled = false;
			_uiModStateText.SetDisabled();
		}
	}

	internal void EnableDependencies()
	{
		List<string> missingRefs = new List<string>();
		EnableDepsRecursive(missingRefs);
		if (missingRefs.Any())
		{
			Interface.infoMessage.Show(Language.GetTextValue("tModLoader.ModDependencyModsNotFound", string.Join(", ", missingRefs)), 10000);
		}
	}

	private void EnableDepsRecursive(List<string> missingRefs)
	{
		string[] modReferences = _modReferences;
		foreach (string name in modReferences)
		{
			UIModItem dep = Interface.modsMenu.FindUIModItem(name);
			if (dep == null)
			{
				missingRefs.Add(name);
				continue;
			}
			dep.EnableDepsRecursive(missingRefs);
			dep.Enable();
		}
	}

	internal void ShowMoreInfo(UIMouseEvent evt, UIElement listeningElement)
	{
		SoundEngine.PlaySound(in SoundID.MenuOpen);
		Interface.modInfo.Show(ModName, _mod.DisplayName, 10000, _mod, _mod.properties.description, _mod.properties.homepage);
	}

	internal void OpenConfig(UIMouseEvent evt, UIElement listeningElement)
	{
		SoundEngine.PlaySound(in SoundID.MenuOpen);
		Interface.modConfigList.SelectedMod = ModLoader.GetMod(ModName);
		Main.menuMode = 10027;
	}

	public override int CompareTo(object obj)
	{
		if (!(obj is UIModItem item))
		{
			return 1;
		}
		string name = DisplayNameClean;
		string othername = item.DisplayNameClean;
		return Interface.modsMenu.sortMode switch
		{
			ModsMenuSortMode.RecentlyUpdated => -1 * _mod.lastModified.CompareTo(item._mod.lastModified), 
			ModsMenuSortMode.DisplayNameAtoZ => string.Compare(name, othername, StringComparison.Ordinal), 
			ModsMenuSortMode.DisplayNameZtoA => -1 * string.Compare(name, othername, StringComparison.Ordinal), 
			_ => base.CompareTo(obj), 
		};
	}

	public bool PassFilters(UIModsFilterResults filterResults)
	{
		if (Interface.modsMenu.filter.Length > 0)
		{
			if (Interface.modsMenu.searchFilterMode == SearchFilter.Author)
			{
				if (_mod.properties.author.IndexOf(Interface.modsMenu.filter, StringComparison.OrdinalIgnoreCase) == -1)
				{
					filterResults.filteredBySearch++;
					return false;
				}
			}
			else if (DisplayNameClean.IndexOf(Interface.modsMenu.filter, StringComparison.OrdinalIgnoreCase) == -1 && ModName.IndexOf(Interface.modsMenu.filter, StringComparison.OrdinalIgnoreCase) == -1)
			{
				filterResults.filteredBySearch++;
				return false;
			}
		}
		if (Interface.modsMenu.modSideFilterMode != 0 && _mod.properties.side != (ModSide)(Interface.modsMenu.modSideFilterMode - 1))
		{
			filterResults.filteredByModSide++;
			return false;
		}
		switch (Interface.modsMenu.enabledFilterMode)
		{
		default:
			return true;
		case EnabledFilter.EnabledOnly:
			if (!_mod.Enabled)
			{
				filterResults.filteredByEnabled++;
			}
			return _mod.Enabled;
		case EnabledFilter.DisabledOnly:
			if (_mod.Enabled)
			{
				filterResults.filteredByEnabled++;
			}
			return !_mod.Enabled;
		}
	}

	private void QuickModDelete(UIMouseEvent evt, UIElement listeningElement)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_017b: Unknown result type (might be due to invalid IL or missing references)
		//IL_017e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0190: Unknown result type (might be due to invalid IL or missing references)
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0227: Unknown result type (might be due to invalid IL or missing references)
		//IL_022d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0230: Unknown result type (might be due to invalid IL or missing references)
		//IL_0236: Unknown result type (might be due to invalid IL or missing references)
		//IL_0239: Unknown result type (might be due to invalid IL or missing references)
		//IL_023f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0242: Unknown result type (might be due to invalid IL or missing references)
		//IL_0248: Unknown result type (might be due to invalid IL or missing references)
		if (!Main.keyState.PressingShift())
		{
			SoundEngine.PlaySound(10);
			_blockInput = new UIImage(TextureAssets.Extra[190])
			{
				Width = 
				{
					Percent = 1f
				},
				Height = 
				{
					Percent = 1f
				},
				Color = new Color(0, 0, 0, 0),
				ScaleToFit = true
			};
			_blockInput.OnLeftMouseDown += CloseDialog;
			Interface.modsMenu.Append(_blockInput);
			_deleteModDialog = new UIPanel
			{
				Width = 
				{
					Percent = 0.3f
				},
				Height = 
				{
					Percent = 0.3f
				},
				HAlign = 0.5f,
				VAlign = 0.5f,
				BackgroundColor = new Color(63, 82, 151),
				BorderColor = Color.Black
			};
			_deleteModDialog.SetPadding(6f);
			Interface.modsMenu.Append(_deleteModDialog);
			_dialogYesButton = new UIAutoScaleTextTextPanel<string>(Language.GetTextValue("LegacyMenu.104"))
			{
				TextColor = Color.White,
				Width = new StyleDimension(-10f, 1f / 3f),
				Height = 
				{
					Pixels = 40f
				},
				VAlign = 0.85f,
				HAlign = 0.15f
			}.WithFadedMouseOver();
			_dialogYesButton.OnLeftClick += DeleteMod;
			_deleteModDialog.Append(_dialogYesButton);
			_dialogNoButton = new UIAutoScaleTextTextPanel<string>(Language.GetTextValue("LegacyMenu.105"))
			{
				TextColor = Color.White,
				Width = new StyleDimension(-10f, 1f / 3f),
				Height = 
				{
					Pixels = 40f
				},
				VAlign = 0.85f,
				HAlign = 0.85f
			}.WithFadedMouseOver();
			_dialogNoButton.OnLeftClick += CloseDialog;
			_deleteModDialog.Append(_dialogNoButton);
			_dialogText = new UIText(Language.GetTextValue("tModLoader.DeleteModConfirm"))
			{
				Width = 
				{
					Percent = 0.75f
				},
				HAlign = 0.5f,
				VAlign = 0.3f,
				IsWrapped = true
			};
			_deleteModDialog.Append(_dialogText);
		}
		else
		{
			DeleteMod(evt, listeningElement);
		}
	}

	private void CloseDialog(UIMouseEvent evt, UIElement listeningElement)
	{
		SoundEngine.PlaySound(in SoundID.MenuClose);
		_blockInput?.Remove();
		_deleteModDialog?.Remove();
	}

	private void DeleteMod(UIMouseEvent evt, UIElement listeningElement)
	{
		ModOrganizer.DeleteMod(_mod);
		CloseDialog(evt, listeningElement);
		Interface.modsMenu.Activate();
	}

	private bool CheckIfPublishedForThisBrowserVersion(out string recommendedModBrowserVersion)
	{
		recommendedModBrowserVersion = SocialBrowserModule.GetBrowserVersionNumber(_mod.tModLoaderVersion);
		return recommendedModBrowserVersion == SocialBrowserModule.GetBrowserVersionNumber(BuildInfo.tMLVersion);
	}
}
