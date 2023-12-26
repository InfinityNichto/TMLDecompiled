using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using ReLogic.OS;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader.Core;
using Terraria.ModLoader.UI.ModBrowser;
using Terraria.Social.Base;
using Terraria.Social.Steam;
using Terraria.UI;

namespace Terraria.ModLoader.UI;

internal class UIModPackItem : UIPanel
{
	private readonly Asset<Texture2D> _dividerTexture;

	private readonly Asset<Texture2D> _innerPanelTexture;

	private readonly UIText _modName;

	private readonly string[] _mods;

	private readonly List<string> _missing = new List<string>();

	private readonly int _numMods;

	private readonly int _numModsEnabled;

	private readonly int _numModsDisabled;

	private readonly UIAutoScaleTextTextPanel<string> _enableListButton;

	private readonly UIAutoScaleTextTextPanel<string> _enableListOnlyButton;

	private readonly UIAutoScaleTextTextPanel<string> _viewInModBrowserButton;

	private readonly UIAutoScaleTextTextPanel<string> _updateListWithEnabledButton;

	private readonly UIAutoScaleTextTextPanel<string> _playInstanceButton;

	private readonly UIAutoScaleTextTextPanel<string> _exportPackInstanceButton;

	private readonly UIAutoScaleTextTextPanel<string> _removePackInstanceButton;

	private readonly UIAutoScaleTextTextPanel<string> _importFromPackLocalButton;

	private readonly UIAutoScaleTextTextPanel<string> _removePackLocalButton;

	private readonly UIImageButton _deleteButton;

	private readonly string _filename;

	private readonly string _filepath;

	private readonly bool _legacy;

	private string _tooltip;

	public UIModPackItem(string name, string[] mods, bool legacy, IEnumerable<LocalMod> localMods)
	{
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_028d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0293: Unknown result type (might be due to invalid IL or missing references)
		//IL_0297: Unknown result type (might be due to invalid IL or missing references)
		//IL_029d: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0366: Unknown result type (might be due to invalid IL or missing references)
		//IL_036c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0370: Unknown result type (might be due to invalid IL or missing references)
		//IL_0376: Unknown result type (might be due to invalid IL or missing references)
		//IL_037a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0380: Unknown result type (might be due to invalid IL or missing references)
		//IL_0384: Unknown result type (might be due to invalid IL or missing references)
		//IL_038a: Unknown result type (might be due to invalid IL or missing references)
		//IL_043f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0445: Unknown result type (might be due to invalid IL or missing references)
		//IL_0449: Unknown result type (might be due to invalid IL or missing references)
		//IL_044f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0453: Unknown result type (might be due to invalid IL or missing references)
		//IL_0459: Unknown result type (might be due to invalid IL or missing references)
		//IL_045d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0463: Unknown result type (might be due to invalid IL or missing references)
		//IL_0518: Unknown result type (might be due to invalid IL or missing references)
		//IL_051e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0522: Unknown result type (might be due to invalid IL or missing references)
		//IL_0528: Unknown result type (might be due to invalid IL or missing references)
		//IL_052c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0532: Unknown result type (might be due to invalid IL or missing references)
		//IL_0536: Unknown result type (might be due to invalid IL or missing references)
		//IL_053c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0647: Unknown result type (might be due to invalid IL or missing references)
		//IL_064d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0651: Unknown result type (might be due to invalid IL or missing references)
		//IL_0657: Unknown result type (might be due to invalid IL or missing references)
		//IL_065b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0661: Unknown result type (might be due to invalid IL or missing references)
		//IL_0665: Unknown result type (might be due to invalid IL or missing references)
		//IL_066b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0720: Unknown result type (might be due to invalid IL or missing references)
		//IL_0726: Unknown result type (might be due to invalid IL or missing references)
		//IL_072a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0730: Unknown result type (might be due to invalid IL or missing references)
		//IL_0734: Unknown result type (might be due to invalid IL or missing references)
		//IL_073a: Unknown result type (might be due to invalid IL or missing references)
		//IL_073e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0744: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0803: Unknown result type (might be due to invalid IL or missing references)
		//IL_0809: Unknown result type (might be due to invalid IL or missing references)
		//IL_080d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0813: Unknown result type (might be due to invalid IL or missing references)
		//IL_0817: Unknown result type (might be due to invalid IL or missing references)
		//IL_081d: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_08f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_08f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_08fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0900: Unknown result type (might be due to invalid IL or missing references)
		//IL_0906: Unknown result type (might be due to invalid IL or missing references)
		//IL_090a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0910: Unknown result type (might be due to invalid IL or missing references)
		_legacy = legacy;
		_filename = (_legacy ? name : Path.GetFileNameWithoutExtension(name));
		_filepath = name;
		_numModsEnabled = 0;
		_numModsDisabled = 0;
		_mods = mods;
		_numMods = mods.Length;
		foreach (string mod in mods)
		{
			LocalMod localMod = localMods.SingleOrDefault((LocalMod m) => m.Name == mod);
			if (localMod != null)
			{
				if (localMod.Enabled)
				{
					_numModsEnabled++;
				}
				else
				{
					_numModsDisabled++;
				}
			}
			else
			{
				_missing.Add(mod);
			}
		}
		BorderColor = new Color(89, 116, 213) * 0.7f;
		_dividerTexture = UICommon.DividerTexture;
		_innerPanelTexture = UICommon.InnerPanelTexture;
		Height.Pixels = (_legacy ? 126 : 210);
		Width.Percent = 1f;
		SetPadding(6f);
		_modName = new UIText(_filename)
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
		UIAutoScaleTextTextPanel<string> viewListButton = new UIAutoScaleTextTextPanel<string>(Language.GetTextValue("tModLoader.ModPackViewList"))
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
				Pixels = 407f
			},
			Top = 
			{
				Pixels = 40f
			}
		}.WithFadedMouseOver();
		viewListButton.PaddingTop -= 2f;
		viewListButton.PaddingBottom -= 2f;
		viewListButton.OnLeftClick += ViewListInfo;
		Append(viewListButton);
		_enableListButton = new UIAutoScaleTextTextPanel<string>(Language.GetTextValue("tModLoader.ModPackEnableThisList"))
		{
			Width = 
			{
				Pixels = 151f
			},
			Height = 
			{
				Pixels = 36f
			},
			Left = 
			{
				Pixels = 248f
			},
			Top = 
			{
				Pixels = 40f
			}
		}.WithFadedMouseOver();
		_enableListButton.PaddingTop -= 2f;
		_enableListButton.PaddingBottom -= 2f;
		_enableListButton.OnLeftClick += EnableList;
		Append(_enableListButton);
		_enableListOnlyButton = new UIAutoScaleTextTextPanel<string>(Language.GetTextValue("tModLoader.ModPackEnableOnlyThisList"))
		{
			Width = 
			{
				Pixels = 190f
			},
			Height = 
			{
				Pixels = 36f
			},
			Left = 
			{
				Pixels = 50f
			},
			Top = 
			{
				Pixels = 40f
			}
		}.WithFadedMouseOver();
		_enableListOnlyButton.PaddingTop -= 2f;
		_enableListOnlyButton.PaddingBottom -= 2f;
		_enableListOnlyButton.OnLeftClick += EnabledListOnly;
		Append(_enableListOnlyButton);
		_viewInModBrowserButton = new UIAutoScaleTextTextPanel<string>(Language.GetTextValue("tModLoader.ModPackViewModsInModBrowser"))
		{
			Width = 
			{
				Pixels = 246f
			},
			Height = 
			{
				Pixels = 36f
			},
			Left = 
			{
				Pixels = 50f
			},
			Top = 
			{
				Pixels = 80f
			}
		}.WithFadedMouseOver();
		_viewInModBrowserButton.PaddingTop -= 2f;
		_viewInModBrowserButton.PaddingBottom -= 2f;
		_viewInModBrowserButton.OnLeftClick += DownloadMissingMods;
		Append(_viewInModBrowserButton);
		_updateListWithEnabledButton = new UIAutoScaleTextTextPanel<string>(Language.GetTextValue("tModLoader.ModPackUpdateListWithEnabled"))
		{
			Width = 
			{
				Pixels = 225f
			},
			Height = 
			{
				Pixels = 36f
			},
			Left = 
			{
				Pixels = 304f
			},
			Top = 
			{
				Pixels = 80f
			}
		}.WithFadedMouseOver();
		_updateListWithEnabledButton.PaddingTop -= 2f;
		_updateListWithEnabledButton.PaddingBottom -= 2f;
		_updateListWithEnabledButton.OnLeftClick += UpdateModPack;
		Append(_updateListWithEnabledButton);
		_deleteButton = new UIImageButton(Main.Assets.Request<Texture2D>("Images/UI/ButtonDelete"))
		{
			Top = 
			{
				Pixels = 40f
			}
		};
		_deleteButton.OnLeftClick += DeleteButtonClick;
		Append(_deleteButton);
		if (!_legacy)
		{
			_importFromPackLocalButton = new UIAutoScaleTextTextPanel<string>(Language.GetTextValue("tModLoader.InstallPackLocal"))
			{
				Width = 
				{
					Pixels = 225f
				},
				Height = 
				{
					Pixels = 36f
				},
				Left = 
				{
					Pixels = 50f
				},
				Top = 
				{
					Pixels = 120f
				}
			}.WithFadedMouseOver();
			_importFromPackLocalButton.PaddingTop -= 2f;
			_importFromPackLocalButton.PaddingBottom -= 2f;
			_importFromPackLocalButton.OnLeftClick += ImportModPackLocal;
			Append(_importFromPackLocalButton);
			_removePackLocalButton = new UIAutoScaleTextTextPanel<string>(Language.GetTextValue("tModLoader.RemovePackLocal"))
			{
				Width = 
				{
					Pixels = 225f
				},
				Height = 
				{
					Pixels = 36f
				},
				Left = 
				{
					Pixels = 280f
				},
				Top = 
				{
					Pixels = 120f
				}
			}.WithFadedMouseOver();
			_removePackLocalButton.PaddingTop -= 2f;
			_removePackLocalButton.PaddingBottom -= 2f;
			_removePackLocalButton.OnLeftClick += RemoveModPackLocal;
			Append(_removePackLocalButton);
			_exportPackInstanceButton = new UIAutoScaleTextTextPanel<string>(Language.GetTextValue("tModLoader.ExportPackInstance"))
			{
				Width = 
				{
					Pixels = 200f
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
					Pixels = 160f
				}
			}.WithFadedMouseOver();
			_exportPackInstanceButton.PaddingTop -= 2f;
			_exportPackInstanceButton.PaddingBottom -= 2f;
			_exportPackInstanceButton.OnLeftClick += ExportInstance;
			Append(_exportPackInstanceButton);
			if (Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), _filename)))
			{
				_removePackInstanceButton = new UIAutoScaleTextTextPanel<string>(Language.GetTextValue("tModLoader.DeletePackInstance"))
				{
					Width = 
					{
						Pixels = 140f
					},
					Height = 
					{
						Pixels = 36f
					},
					Left = 
					{
						Pixels = 370f
					},
					Top = 
					{
						Pixels = 160f
					}
				}.WithFadedMouseOver();
				_removePackInstanceButton.PaddingTop -= 2f;
				_removePackInstanceButton.PaddingBottom -= 2f;
				_removePackInstanceButton.OnLeftClick += DeleteInstance;
				Append(_removePackInstanceButton);
			}
		}
	}

	public override void Draw(SpriteBatch spriteBatch)
	{
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		_tooltip = null;
		base.Draw(spriteBatch);
		if (!string.IsNullOrEmpty(_tooltip))
		{
			byte mouseTextColor = Main.mouseTextColor;
			Main.mouseTextColor = 160;
			Rectangle bounds = GetOuterDimensions().ToRectangle();
			bounds.Height += 16;
			UICommon.DrawHoverStringInBounds(spriteBatch, _tooltip, bounds);
			Main.mouseTextColor = mouseTextColor;
		}
	}

	private void DrawPanel(SpriteBatch spriteBatch, Vector2 position, float width)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		spriteBatch.Draw(_innerPanelTexture.Value, position, (Rectangle?)new Rectangle(0, 0, 8, _innerPanelTexture.Height()), Color.White);
		spriteBatch.Draw(_innerPanelTexture.Value, new Vector2(position.X + 8f, position.Y), (Rectangle?)new Rectangle(8, 0, 8, _innerPanelTexture.Height()), Color.White, 0f, Vector2.Zero, new Vector2((width - 16f) / 8f, 1f), (SpriteEffects)0, 0f);
		spriteBatch.Draw(_innerPanelTexture.Value, new Vector2(position.X + width - 8f, position.Y), (Rectangle?)new Rectangle(16, 0, 8, _innerPanelTexture.Height()), Color.White);
	}

	private void DrawEnabledText(SpriteBatch spriteBatch, Vector2 drawPos)
	{
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		string text = Language.GetTextValue("tModLoader.ModPackModsAvailableStatus", _numMods, _numModsEnabled, _numModsDisabled, _missing.Count);
		Color color = ((_missing.Count > 0) ? Color.Red : ((_numModsDisabled > 0) ? Color.Yellow : Color.Green));
		Utils.DrawBorderString(spriteBatch, text, drawPos, color);
	}

	protected override void DrawSelf(SpriteBatch spriteBatch)
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		base.DrawSelf(spriteBatch);
		CalculatedStyle innerDimensions = GetInnerDimensions();
		Vector2 drawPos = default(Vector2);
		((Vector2)(ref drawPos))._002Ector(innerDimensions.X + 5f, innerDimensions.Y + 30f);
		spriteBatch.Draw(_dividerTexture.Value, drawPos, (Rectangle?)null, Color.White, 0f, Vector2.Zero, new Vector2((innerDimensions.Width - 10f) / 8f, 1f), (SpriteEffects)0, 0f);
		((Vector2)(ref drawPos))._002Ector(innerDimensions.X + innerDimensions.Width - 355f, innerDimensions.Y);
		DrawPanel(spriteBatch, drawPos, 350f);
		DrawEnabledText(spriteBatch, drawPos + new Vector2(10f, 5f));
		UIAutoScaleTextTextPanel<string> enableListOnlyButton = _enableListOnlyButton;
		if (enableListOnlyButton != null && enableListOnlyButton.IsMouseHovering)
		{
			_tooltip = Language.GetTextValue("tModLoader.ModPackEnableOnlyThisListDesc");
			return;
		}
		UIAutoScaleTextTextPanel<string> enableListButton = _enableListButton;
		if (enableListButton != null && enableListButton.IsMouseHovering)
		{
			_tooltip = Language.GetTextValue("tModLoader.ModPackEnableThisListDesc");
			return;
		}
		UIAutoScaleTextTextPanel<string> exportPackInstanceButton = _exportPackInstanceButton;
		if (exportPackInstanceButton != null && exportPackInstanceButton.IsMouseHovering)
		{
			_tooltip = Language.GetTextValue("tModLoader.ExportPackInstanceDesc");
			return;
		}
		UIAutoScaleTextTextPanel<string> removePackInstanceButton = _removePackInstanceButton;
		if (removePackInstanceButton != null && removePackInstanceButton.IsMouseHovering)
		{
			_tooltip = Language.GetTextValue("tModLoader.DeletePackInstanceDesc");
			return;
		}
		UIAutoScaleTextTextPanel<string> playInstanceButton = _playInstanceButton;
		if (playInstanceButton != null && playInstanceButton.IsMouseHovering)
		{
			_tooltip = "Play tModLoader using InstallDirectory/<ModPackName>";
			return;
		}
		UIAutoScaleTextTextPanel<string> importFromPackLocalButton = _importFromPackLocalButton;
		if (importFromPackLocalButton != null && importFromPackLocalButton.IsMouseHovering)
		{
			_tooltip = Language.GetTextValue("tModLoader.InstallPackLocalDesc");
			return;
		}
		UIAutoScaleTextTextPanel<string> removePackLocalButton = _removePackLocalButton;
		if (removePackLocalButton != null && removePackLocalButton.IsMouseHovering)
		{
			_tooltip = Language.GetTextValue("tModLoader.RemovePackLocalDesc");
			return;
		}
		UIAutoScaleTextTextPanel<string> viewInModBrowserButton = _viewInModBrowserButton;
		if (viewInModBrowserButton != null && viewInModBrowserButton.IsMouseHovering)
		{
			_tooltip = Language.GetTextValue("tModLoader.ModPackViewModsInModBrowserDesc");
			return;
		}
		UIAutoScaleTextTextPanel<string> updateListWithEnabledButton = _updateListWithEnabledButton;
		if (updateListWithEnabledButton != null && updateListWithEnabledButton.IsMouseHovering)
		{
			_tooltip = Language.GetTextValue("tModLoader.ModPackUpdateListWithEnabledDesc");
		}
	}

	public override void MouseOver(UIMouseEvent evt)
	{
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		base.MouseOver(evt);
		if (Path.GetFileNameWithoutExtension(ModOrganizer.ModPackActive) == _filename)
		{
			BackgroundColor = Color.MediumPurple * 0.4f;
		}
		else
		{
			BackgroundColor = UICommon.DefaultUIBlue;
		}
		BorderColor = new Color(89, 116, 213);
	}

	public override void MouseOut(UIMouseEvent evt)
	{
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		base.MouseOut(evt);
		if (Path.GetFileNameWithoutExtension(ModOrganizer.ModPackActive) == _filename)
		{
			BackgroundColor = Color.MediumPurple * 0.7f;
		}
		else
		{
			BackgroundColor = new Color(63, 82, 151) * 0.7f;
		}
		BorderColor = new Color(89, 116, 213) * 0.7f;
	}

	private void DeleteButtonClick(UIMouseEvent evt, UIElement listeningElement)
	{
		UIModPackItem modPackItem = (UIModPackItem)listeningElement.Parent;
		if (_legacy)
		{
			string path = UIModPacks.ModPacksDirectory + Path.DirectorySeparatorChar + modPackItem._filename + ".json";
			if (File.Exists(path))
			{
				File.Delete(path);
			}
		}
		else
		{
			string path2 = Path.Combine(UIModPacks.ModPacksDirectory, _filename);
			if (Directory.Exists(path2))
			{
				Directory.Delete(path2, recursive: true);
			}
		}
		Logging.tML.Info((object)("Deleted Mod Pack " + modPackItem._filename));
		Interface.modPacksMenu.OnDeactivate();
		Interface.modPacksMenu.OnActivate();
	}

	private static void EnableList(UIMouseEvent evt, UIElement listeningElement)
	{
		UIModPackItem modListItem = (UIModPackItem)listeningElement.Parent;
		LocalMod[] array = ModOrganizer.FindMods();
		foreach (LocalMod mod in array)
		{
			mod.Enabled = mod.Enabled || modListItem._mods.Contains(mod.Name);
		}
		if (modListItem._missing.Count > 0)
		{
			Interface.infoMessage.Show(Language.GetTextValue("tModLoader.ModPackModsMissing", string.Join("\n", modListItem._missing)), 10016);
		}
		Logging.tML.Info((object)("Enabled Collection of mods defined in  Mod Pack " + modListItem._filename));
		ModLoader.OnSuccessfulLoad = (Action)Delegate.Combine(ModLoader.OnSuccessfulLoad, (Action)delegate
		{
			Main.menuMode = 10016;
		});
		ModLoader.Reload();
	}

	private List<ModPubId_t> GetModPackBrowserIds()
	{
		if (!_legacy)
		{
			return Array.ConvertAll(File.ReadAllLines(Path.Combine(UIModPacks.ModPackModsPath(_filename), "install.txt")), delegate(string x)
			{
				ModPubId_t result = default(ModPubId_t);
				result.m_ModPubId = x;
				return result;
			}).ToList();
		}
		QueryParameters query = default(QueryParameters);
		query.searchModSlugs = _mods;
		if (!WorkshopHelper.TryGetPublishIdByInternalName(query, out var modIds))
		{
			return new List<ModPubId_t>();
		}
		List<ModPubId_t> output = new List<ModPubId_t>();
		foreach (string item in modIds)
		{
			if (item != "0")
			{
				output.Add(new ModPubId_t
				{
					m_ModPubId = item
				});
			}
		}
		return output;
	}

	private static void DownloadMissingMods(UIMouseEvent evt, UIElement listeningElement)
	{
		UIModPackItem modpack = (UIModPackItem)listeningElement.Parent;
		Interface.modBrowser.Activate();
		Interface.modBrowser.FilterTextBox.Text = "";
		Interface.modBrowser.SpecialModPackFilter = modpack.GetModPackBrowserIds();
		Interface.modBrowser.SpecialModPackFilterTitle = Language.GetTextValue("tModLoader.MBFilterModlist");
		Interface.modBrowser.UpdateFilterMode = UpdateFilter.All;
		Interface.modBrowser.ModSideFilterMode = ModSideFilter.All;
		SoundEngine.PlaySound(in SoundID.MenuOpen);
		Interface.modBrowser.PreviousUIState = Interface.modPacksMenu;
		Main.menuMode = 10007;
	}

	private static void EnabledListOnly(UIMouseEvent evt, UIElement listeningElement)
	{
		UIModPackItem modpack = (UIModPackItem)listeningElement.Parent;
		ModLoader.DisableAllMods();
		EnableList(evt, listeningElement);
		Logging.tML.Info((object)("Enabled only mods defined in Collection " + modpack._filename));
	}

	private static void UpdateModPack(UIMouseEvent evt, UIElement listeningElement)
	{
		UIModPackItem modpack = (UIModPackItem)listeningElement.Parent;
		UIModPacks.SaveModPack(modpack._filename);
		if (modpack._filepath == ModOrganizer.ModPackActive)
		{
			ModLoader.DisableAllMods();
			Logging.tML.Info((object)("Cleaning up removed tmods " + modpack._filename));
			ModLoader.OnSuccessfulLoad = (Action)Delegate.Combine(ModLoader.OnSuccessfulLoad, (Action)delegate
			{
				foreach (string current in Directory.EnumerateFiles(UIModPacks.ModPackModsPath(modpack._filename), "*.tmod"))
				{
					if (!modpack._mods.Contains<string>(Path.GetFileNameWithoutExtension(current)))
					{
						File.Delete(current);
					}
				}
				EnableList(evt, listeningElement);
			});
			ModLoader.Reload();
		}
		else
		{
			foreach (string file in Directory.EnumerateFiles(UIModPacks.ModPackModsPath(modpack._filename), "*.tmod"))
			{
				if (!modpack._mods.Contains<string>(Path.GetFileNameWithoutExtension(file)))
				{
					File.Delete(file);
				}
			}
		}
		Interface.modPacksMenu.OnDeactivate();
		Interface.modPacksMenu.OnActivate();
	}

	private static void ImportModPackLocal(UIMouseEvent evt, UIElement listeningElement)
	{
		UIModPackItem modpack = (UIModPackItem)listeningElement.Parent;
		ModOrganizer.ModPackActive = modpack._filepath;
		Logging.tML.Info((object)("Enabled Frozen Mod Pack " + modpack._filename));
		EnabledListOnly(evt, listeningElement);
	}

	private static void RemoveModPackLocal(UIMouseEvent evt, UIElement listeningElement)
	{
		UIModPackItem modpack = (UIModPackItem)listeningElement.Parent;
		ModOrganizer.ModPackActive = null;
		ModLoader.DisableAllMods();
		Logging.tML.Info((object)("Disabled Frozen Mod Pack " + modpack._filename));
		ModLoader.OnSuccessfulLoad = (Action)Delegate.Combine(ModLoader.OnSuccessfulLoad, (Action)delegate
		{
			Main.menuMode = 10016;
		});
		ModLoader.Reload();
	}

	private static void ExportInstance(UIMouseEvent evt, UIElement listeningElement)
	{
		UIModPacks.ExportSnapshot(((UIModPackItem)listeningElement.Parent)._filename);
		Interface.modPacksMenu.OnDeactivate();
		Interface.modPacksMenu.OnActivate();
	}

	private static void PlayInstance(UIMouseEvent evt, UIElement listeningElement)
	{
		UIModPackItem modpack = (UIModPackItem)listeningElement.Parent;
		string launchScript = Path.Combine(Path.Combine(Directory.GetCurrentDirectory(), modpack._filename), Platform.IsWindows ? "start-tModLoader.bat" : "start-tModLoader.sh");
		Process.Start(new ProcessStartInfo
		{
			FileName = launchScript,
			UseShellExecute = true
		});
	}

	private static void DeleteInstance(UIMouseEvent evt, UIElement listeningElement)
	{
		UIModPackItem modpack = (UIModPackItem)listeningElement.Parent;
		Directory.Delete(Path.Combine(Directory.GetCurrentDirectory(), modpack._filename), recursive: true);
		Interface.modPacksMenu.OnDeactivate();
		Interface.modPacksMenu.OnActivate();
	}

	private static void ViewListInfo(UIMouseEvent evt, UIElement listeningElement)
	{
		UIModPackItem modListItem = (UIModPackItem)listeningElement.Parent;
		SoundEngine.PlaySound(10);
		string message = "";
		string[] mods = modListItem._mods;
		foreach (string mod in mods)
		{
			message = message + mod + (modListItem._missing.Contains(mod) ? Language.GetTextValue("tModLoader.ModPackMissing") : (ModLoader.IsEnabled(mod) ? "" : Language.GetTextValue("tModLoader.ModPackDisabled"))) + "\n";
		}
		Interface.infoMessage.Show(Language.GetTextValue("tModLoader.ModPackModsContained", message), 10016);
	}

	public override int CompareTo(object obj)
	{
		if (!(obj is UIModPackItem item))
		{
			return base.CompareTo(obj);
		}
		return string.Compare(_filename, item._filename, StringComparison.Ordinal);
	}
}
