using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader.UI;
using Terraria.UI;
using Terraria.UI.Gamepad;

namespace Terraria.ModLoader.Config.UI;

internal class UIModConfigList : UIState
{
	public Mod SelectedMod;

	private UIElement uIElement;

	private UIPanel uIPanel;

	private UITextPanel<LocalizedText> backButton;

	private UIList modList;

	private UIList configList;

	public override void OnInitialize()
	{
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_047d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0483: Unknown result type (might be due to invalid IL or missing references)
		//IL_0487: Unknown result type (might be due to invalid IL or missing references)
		//IL_048d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0491: Unknown result type (might be due to invalid IL or missing references)
		//IL_0497: Unknown result type (might be due to invalid IL or missing references)
		//IL_049b: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a1: Unknown result type (might be due to invalid IL or missing references)
		uIElement = new UIElement
		{
			Width = 
			{
				Percent = 0.8f
			},
			MaxWidth = 
			{
				Pixels = 800f,
				Percent = 0f
			},
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
		Append(uIElement);
		uIPanel = new UIPanel
		{
			Width = 
			{
				Percent = 1f
			},
			Height = 
			{
				Pixels = -110f,
				Percent = 1f
			},
			BackgroundColor = UICommon.MainPanelBackground
		};
		uIElement.Append(uIPanel);
		UITextPanel<LocalizedText> uIHeaderTextPanel = new UITextPanel<LocalizedText>(Language.GetText("tModLoader.ModConfiguration"), 0.8f, large: true)
		{
			HAlign = 0.5f,
			Top = 
			{
				Pixels = -35f
			},
			BackgroundColor = UICommon.DefaultUIBlue
		}.WithPadding(15f);
		uIElement.Append(uIHeaderTextPanel);
		UIPanel modListPanel = new UIPanel
		{
			Width = 
			{
				Pixels = uIPanel.PaddingTop / -2f,
				Percent = 0.5f
			},
			Height = 
			{
				Percent = 1f
			}
		};
		uIPanel.Append(modListPanel);
		UIPanel configListPanel = new UIPanel
		{
			Width = 
			{
				Pixels = uIPanel.PaddingTop / -2f,
				Percent = 0.5f
			},
			Height = 
			{
				Percent = 1f
			},
			HAlign = 1f
		};
		uIPanel.Append(configListPanel);
		float headerHeight = 35f;
		UIText modListHeader = new UIText(Language.GetText("tModLoader.MenuMods"), 0.5f, large: true)
		{
			Top = 
			{
				Pixels = 5f
			},
			Left = 
			{
				Pixels = 12.5f
			},
			HAlign = 0.5f
		};
		modListPanel.Append(modListHeader);
		UIText configListHeader = new UIText(Language.GetText("tModLoader.ModConfigs"), 0.5f, large: true)
		{
			Top = 
			{
				Pixels = 5f
			},
			Left = 
			{
				Pixels = -12.5f
			},
			HAlign = 0.5f
		};
		configListPanel.Append(configListHeader);
		modList = new UIList
		{
			Top = 
			{
				Pixels = headerHeight
			},
			Width = 
			{
				Pixels = -25f,
				Percent = 1f
			},
			Height = 
			{
				Pixels = 0f - headerHeight,
				Percent = 1f
			},
			ListPadding = 5f,
			HAlign = 1f
		};
		modListPanel.Append(modList);
		configList = new UIList
		{
			Top = 
			{
				Pixels = headerHeight
			},
			Width = 
			{
				Pixels = -25f,
				Percent = 1f
			},
			Height = 
			{
				Pixels = 0f - headerHeight,
				Percent = 1f
			},
			ListPadding = 5f,
			HAlign = 0f
		};
		configListPanel.Append(configList);
		UIScrollbar modListScrollbar = new UIScrollbar
		{
			Top = 
			{
				Pixels = headerHeight
			},
			Height = 
			{
				Pixels = 0f - headerHeight,
				Percent = 1f
			}
		};
		modListScrollbar.SetView(100f, 1000f);
		modList.SetScrollbar(modListScrollbar);
		modListPanel.Append(modListScrollbar);
		UIScrollbar configListScrollbar = new UIScrollbar
		{
			Top = 
			{
				Pixels = headerHeight
			},
			Height = 
			{
				Pixels = 0f - headerHeight,
				Percent = 1f
			},
			HAlign = 1f
		};
		configListScrollbar.SetView(100f, 1000f);
		configList.SetScrollbar(configListScrollbar);
		configListPanel.Append(configListScrollbar);
		backButton = new UITextPanel<LocalizedText>(Language.GetText("UI.Back"), 0.7f, large: true)
		{
			Width = 
			{
				Pixels = -10f,
				Percent = 0.5f
			},
			Height = 
			{
				Pixels = 50f
			},
			Top = 
			{
				Pixels = -45f
			},
			VAlign = 1f,
			HAlign = 0.5f
		}.WithFadedMouseOver();
		backButton.OnLeftClick += delegate
		{
			SoundEngine.PlaySound(in SoundID.MenuClose);
			SelectedMod = null;
			if (Main.gameMenu)
			{
				Main.menuMode = 10000;
			}
			else
			{
				IngameFancyUI.Close();
			}
		};
		uIElement.Append(backButton);
	}

	internal void Unload()
	{
		modList?.Clear();
		configList?.Clear();
		SelectedMod = null;
	}

	public override void OnActivate()
	{
		modList?.Clear();
		configList?.Clear();
		PopulateMods();
		if (SelectedMod != null)
		{
			PopulateConfigs();
		}
	}

	private void PopulateMods()
	{
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		modList?.Clear();
		List<Mod> list = ModLoader.Mods.ToList();
		list.Sort((Mod x, Mod y) => x.DisplayName.CompareTo(y.DisplayName));
		foreach (Mod mod in list)
		{
			if (ConfigManager.Configs.TryGetValue(mod, out List<ModConfig> _))
			{
				UIButton<string> modPanel = new UIButton<string>(mod.DisplayName)
				{
					MaxWidth = 
					{
						Percent = 0.95f
					},
					HAlign = 0.5f,
					ScalePanel = true,
					AltPanelColor = UICommon.MainPanelBackground,
					AltHoverPanelColor = UICommon.MainPanelBackground * 1.25f,
					UseAltColors = () => SelectedMod != mod,
					ClickSound = SoundID.MenuTick
				};
				modPanel.OnLeftClick += delegate
				{
					SelectedMod = mod;
					PopulateConfigs();
				};
				modList.Add(modPanel);
			}
		}
	}

	private void PopulateConfigs()
	{
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		configList?.Clear();
		if (SelectedMod == null || !ConfigManager.Configs.TryGetValue(SelectedMod, out List<ModConfig> configs))
		{
			return;
		}
		foreach (ModConfig config in configs.OrderBy((ModConfig x) => x.DisplayName.Value).ToList())
		{
			float indicatorOffset = 20f;
			UIButton<LocalizedText> configPanel = new UIButton<LocalizedText>(config.DisplayName)
			{
				MaxWidth = 
				{
					Percent = 0.95f
				},
				HAlign = 0.5f,
				ScalePanel = true,
				UseInnerDimensions = true,
				ClickSound = SoundID.MenuOpen
			};
			configPanel.PaddingRight += indicatorOffset;
			configPanel.OnLeftClick += delegate
			{
				Interface.modConfig.SetMod(SelectedMod, config);
				if (Main.gameMenu)
				{
					Main.menuMode = 10024;
				}
				else
				{
					Main.InGameUI.SetState(Interface.modConfig);
				}
			};
			configList.Add(configPanel);
			Asset<Texture2D> indicatorTexture = Main.Assets.Request<Texture2D>("Images/UI/Settings_Toggle");
			Rectangle indicatorFrame = indicatorTexture.Frame(2, 1, 1);
			Color serverColor = Colors.RarityRed;
			Color clientColor = Colors.RarityCyan;
			UIImageFramed sideIndicator = new UIImageFramed(indicatorTexture, indicatorFrame)
			{
				VAlign = 0.5f,
				HAlign = 1f,
				Color = ((config.Mode == ConfigScope.ServerSide) ? serverColor : clientColor),
				MarginRight = 0f - indicatorOffset
			};
			sideIndicator.OnUpdate += delegate
			{
				//IL_0030: Unknown result type (might be due to invalid IL or missing references)
				//IL_0023: Unknown result type (might be due to invalid IL or missing references)
				if (sideIndicator.IsMouseHovering)
				{
					string value = ((config.Mode == ConfigScope.ServerSide) ? serverColor.Hex3() : clientColor.Hex3());
					string textValue = Language.GetTextValue((config.Mode == ConfigScope.ServerSide) ? "tModLoader.ModConfigServerSide" : "tModLoader.ModConfigClientSide");
					Main.instance.MouseText($"[c/{value}:{textValue}]", 0, 0);
				}
			};
			configPanel.Append(sideIndicator);
		}
	}

	public override void Draw(SpriteBatch spriteBatch)
	{
		base.Draw(spriteBatch);
		UILinkPointNavigator.Shortcuts.BackButtonCommand = 100;
		UILinkPointNavigator.Shortcuts.BackButtonGoto = 10000;
	}
}
