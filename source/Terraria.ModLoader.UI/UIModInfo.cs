using System.Linq;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader.Core;
using Terraria.Social.Base;
using Terraria.Social.Steam;
using Terraria.UI;
using Terraria.UI.Gamepad;

namespace Terraria.ModLoader.UI;

internal class UIModInfo : UIState
{
	private UIElement _uIElement;

	private UIMessageBox _modInfo;

	private UITextPanel<string> _uITextPanel;

	private UIAutoScaleTextTextPanel<string> _modHomepageButton;

	private UIAutoScaleTextTextPanel<string> _modSteamButton;

	private UIAutoScaleTextTextPanel<string> extractLocalizationButton;

	private UIAutoScaleTextTextPanel<string> fakeExtractLocalizationButton;

	private UIAutoScaleTextTextPanel<string> _extractButton;

	private UIAutoScaleTextTextPanel<string> _deleteButton;

	private UIAutoScaleTextTextPanel<string> _fakeDeleteButton;

	private readonly UILoaderAnimatedImage _loaderElement = new UILoaderAnimatedImage(0.5f, 0.5f);

	private int _gotoMenu;

	private LocalMod _localMod;

	private string _url = string.Empty;

	private string _info = string.Empty;

	private string _modName = string.Empty;

	private string _modDisplayName = string.Empty;

	private ModPubId_t _publishedFileId;

	private bool _loading;

	private bool _ready;

	private CancellationTokenSource _cts;

	public override void OnInitialize()
	{
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0194: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_022c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0232: Unknown result type (might be due to invalid IL or missing references)
		//IL_0235: Unknown result type (might be due to invalid IL or missing references)
		//IL_023b: Unknown result type (might be due to invalid IL or missing references)
		//IL_023e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0244: Unknown result type (might be due to invalid IL or missing references)
		//IL_0247: Unknown result type (might be due to invalid IL or missing references)
		//IL_024d: Unknown result type (might be due to invalid IL or missing references)
		//IL_02dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_038e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0394: Unknown result type (might be due to invalid IL or missing references)
		//IL_0397: Unknown result type (might be due to invalid IL or missing references)
		//IL_039d: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03af: Unknown result type (might be due to invalid IL or missing references)
		//IL_0448: Unknown result type (might be due to invalid IL or missing references)
		//IL_044d: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_04bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0568: Unknown result type (might be due to invalid IL or missing references)
		//IL_056e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0571: Unknown result type (might be due to invalid IL or missing references)
		//IL_0577: Unknown result type (might be due to invalid IL or missing references)
		//IL_057a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0580: Unknown result type (might be due to invalid IL or missing references)
		//IL_0583: Unknown result type (might be due to invalid IL or missing references)
		//IL_0589: Unknown result type (might be due to invalid IL or missing references)
		//IL_0619: Unknown result type (might be due to invalid IL or missing references)
		//IL_061f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0622: Unknown result type (might be due to invalid IL or missing references)
		//IL_0628: Unknown result type (might be due to invalid IL or missing references)
		//IL_062b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0631: Unknown result type (might be due to invalid IL or missing references)
		//IL_0634: Unknown result type (might be due to invalid IL or missing references)
		//IL_063a: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d8: Unknown result type (might be due to invalid IL or missing references)
		_uIElement = new UIElement
		{
			Width = 
			{
				Percent = 0.8f
			},
			MaxWidth = new StyleDimension(800f, 0f),
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
		UIPanel uIPanel = new UIPanel
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
		_uIElement.Append(uIPanel);
		_modInfo = new UIMessageBox(string.Empty)
		{
			Width = 
			{
				Pixels = -25f,
				Percent = 1f
			},
			Height = 
			{
				Percent = 1f
			}
		};
		uIPanel.Append(_modInfo);
		UIScrollbar uIScrollbar = new UIScrollbar
		{
			Height = 
			{
				Pixels = -12f,
				Percent = 1f
			},
			VAlign = 0.5f,
			HAlign = 1f
		}.WithView(100f, 1000f);
		uIPanel.Append(uIScrollbar);
		_modInfo.SetScrollbar(uIScrollbar);
		_uITextPanel = new UITextPanel<string>(Language.GetTextValue("tModLoader.ModInfoHeader"), 0.8f, large: true)
		{
			HAlign = 0.5f,
			Top = 
			{
				Pixels = -35f
			},
			BackgroundColor = UICommon.DefaultUIBlue
		}.WithPadding(15f);
		_uIElement.Append(_uITextPanel);
		_modHomepageButton = new UIAutoScaleTextTextPanel<string>(Language.GetTextValue("tModLoader.ModInfoVisitHomepage"))
		{
			Width = 
			{
				Pixels = -10f,
				Percent = 0.333f
			},
			Height = 
			{
				Pixels = 40f
			},
			HAlign = 0.5f,
			VAlign = 1f,
			Top = 
			{
				Pixels = -65f
			}
		}.WithFadedMouseOver();
		_modHomepageButton.OnLeftClick += VisitModHomePage;
		_modSteamButton = new UIAutoScaleTextTextPanel<string>(Language.GetTextValue("tModLoader.ModInfoVisitSteampage"))
		{
			Width = 
			{
				Pixels = -10f,
				Percent = 0.333f
			},
			Height = 
			{
				Pixels = 40f
			},
			HAlign = 0f,
			VAlign = 1f,
			Top = 
			{
				Pixels = -65f
			}
		}.WithFadedMouseOver();
		_modSteamButton.OnLeftClick += VisitModHostPage;
		extractLocalizationButton = new UIAutoScaleTextTextPanel<string>(Language.GetTextValue("tModLoader.ModInfoExtractLocalization"))
		{
			Width = 
			{
				Pixels = -10f,
				Percent = 0.333f
			},
			Height = 
			{
				Pixels = 40f
			},
			HAlign = 1f,
			VAlign = 1f,
			Top = 
			{
				Pixels = -65f
			}
		}.WithFadedMouseOver();
		extractLocalizationButton.OnLeftClick += ExtractLocalization;
		fakeExtractLocalizationButton = new UIAutoScaleTextTextPanel<string>(Language.GetTextValue("tModLoader.ModInfoExtractLocalization"))
		{
			Width = 
			{
				Pixels = -10f,
				Percent = 0.333f
			},
			Height = 
			{
				Pixels = 40f
			},
			HAlign = 1f,
			VAlign = 1f,
			Top = 
			{
				Pixels = -65f
			}
		};
		fakeExtractLocalizationButton.BackgroundColor = Color.Gray;
		UIAutoScaleTextTextPanel<string> backButton = new UIAutoScaleTextTextPanel<string>(Language.GetTextValue("UI.Back"))
		{
			Width = 
			{
				Pixels = -10f,
				Percent = 0.333f
			},
			Height = 
			{
				Pixels = 40f
			},
			VAlign = 1f,
			Top = 
			{
				Pixels = -20f
			}
		}.WithFadedMouseOver();
		backButton.OnLeftClick += BackClick;
		_uIElement.Append(backButton);
		_extractButton = new UIAutoScaleTextTextPanel<string>(Language.GetTextValue("tModLoader.ModInfoExtract"))
		{
			Width = 
			{
				Pixels = -10f,
				Percent = 0.333f
			},
			Height = 
			{
				Pixels = 40f
			},
			VAlign = 1f,
			HAlign = 0.5f,
			Top = 
			{
				Pixels = -20f
			}
		}.WithFadedMouseOver();
		_extractButton.OnLeftClick += ExtractMod;
		_deleteButton = new UIAutoScaleTextTextPanel<string>(Language.GetTextValue("UI.Delete"))
		{
			Width = 
			{
				Pixels = -10f,
				Percent = 0.333f
			},
			Height = 
			{
				Pixels = 40f
			},
			VAlign = 1f,
			HAlign = 1f,
			Top = 
			{
				Pixels = -20f
			}
		}.WithFadedMouseOver();
		_deleteButton.OnLeftClick += DeleteMod;
		_fakeDeleteButton = new UIAutoScaleTextTextPanel<string>(Language.GetTextValue("UI.Delete"))
		{
			Width = 
			{
				Pixels = -10f,
				Percent = 0.333f
			},
			Height = 
			{
				Pixels = 40f
			},
			VAlign = 1f,
			HAlign = 1f,
			Top = 
			{
				Pixels = -20f
			}
		};
		_fakeDeleteButton.BackgroundColor = Color.Gray;
		Append(_uIElement);
	}

	private void ExtractLocalization(UIMouseEvent evt, UIElement listeningElement)
	{
		SoundEngine.PlaySound(in SoundID.MenuOpen);
		if (LocalizationLoader.ExtractLocalizationFiles(_modName))
		{
			extractLocalizationButton.SetText(Language.GetTextValue("tModLoader.ModInfoExtracted"));
		}
	}

	internal void Show(string modName, string displayName, int gotoMenu, LocalMod localMod, string description = "", string url = "")
	{
		_modName = modName;
		_modDisplayName = displayName;
		_gotoMenu = gotoMenu;
		_localMod = localMod;
		_info = description;
		if (_info.Equals(""))
		{
			_info = Language.GetTextValue("tModLoader.ModInfoNoDescriptionAvailable");
		}
		_url = url;
		if (localMod != null && WorkshopHelper.GetPublishIdLocal(localMod.modFile, out var publishId))
		{
			_publishedFileId = new ModPubId_t
			{
				m_ModPubId = publishId.ToString()
			};
		}
		else
		{
			_publishedFileId = default(ModPubId_t);
		}
		Main.gameMenu = true;
		Main.menuMode = 10008;
	}

	public override void OnDeactivate()
	{
		base.OnDeactivate();
		_info = string.Empty;
		_localMod = null;
		_gotoMenu = 0;
		_modName = string.Empty;
		_modDisplayName = string.Empty;
		_url = string.Empty;
		_modHomepageButton.Remove();
		_modSteamButton.Remove();
		extractLocalizationButton.Remove();
		fakeExtractLocalizationButton.Remove();
		_deleteButton.Remove();
		_fakeDeleteButton.Remove();
		_extractButton.Remove();
	}

	private void BackClick(UIMouseEvent evt, UIElement listeningElement)
	{
		SoundEngine.PlaySound(in SoundID.MenuClose);
		Main.menuMode = _gotoMenu;
	}

	private void ExtractMod(UIMouseEvent evt, UIElement listeningElement)
	{
		SoundEngine.PlaySound(in SoundID.MenuOpen);
		Interface.extractMod.Show(_localMod, _gotoMenu);
	}

	private void DeleteMod(UIMouseEvent evt, UIElement listeningElement)
	{
		SoundEngine.PlaySound(in SoundID.MenuClose);
		ModOrganizer.DeleteMod(_localMod);
		Main.menuMode = _gotoMenu;
	}

	private void VisitModHomePage(UIMouseEvent evt, UIElement listeningElement)
	{
		SoundEngine.PlaySound(10);
		Utils.OpenToURL(_url);
	}

	private void VisitModHostPage(UIMouseEvent evt, UIElement listeningElement)
	{
		SoundEngine.PlaySound(10);
		VisitModHostPageInner();
	}

	private void VisitModHostPageInner()
	{
		Utils.OpenToURL(Interface.modBrowser.SocialBackend.GetModWebPage(_publishedFileId));
	}

	public override void Draw(SpriteBatch spriteBatch)
	{
		base.Draw(spriteBatch);
		UILinkPointNavigator.Shortcuts.BackButtonCommand = 100;
		UILinkPointNavigator.Shortcuts.BackButtonGoto = _gotoMenu;
		if (_modHomepageButton.IsMouseHovering)
		{
			UICommon.DrawHoverStringInBounds(spriteBatch, _url);
		}
		if (_fakeDeleteButton.IsMouseHovering)
		{
			UICommon.DrawHoverStringInBounds(spriteBatch, Language.GetTextValue("tModLoader.ModInfoDisableModToDelete"));
		}
		if (fakeExtractLocalizationButton.IsMouseHovering)
		{
			UICommon.DrawHoverStringInBounds(spriteBatch, Language.GetTextValue("tModLoader.ModInfoEnableModToExtractLocalizationFiles"));
		}
	}

	public override void OnActivate()
	{
		_modInfo.SetText(_info);
		_uITextPanel.SetText(Language.GetTextValue("tModLoader.ModInfoHeader") + _modDisplayName, 0.8f, large: true);
		_loading = false;
		_ready = true;
	}

	public override void Update(GameTime gameTime)
	{
		if (!_loading && _ready)
		{
			_modInfo.SetText(_info);
			if (!string.IsNullOrEmpty(_url))
			{
				_uIElement.Append(_modHomepageButton);
			}
			if (!string.IsNullOrEmpty(_publishedFileId.m_ModPubId))
			{
				_uIElement.Append(_modSteamButton);
			}
			if (_localMod != null)
			{
				bool realDeleteButton = ModLoader.Mods.All((Mod x) => x.Name != _localMod.Name);
				_uIElement.AddOrRemoveChild(_deleteButton, realDeleteButton);
				_uIElement.AddOrRemoveChild(_fakeDeleteButton, !realDeleteButton);
				_uIElement.AddOrRemoveChild(extractLocalizationButton, !realDeleteButton);
				_uIElement.AddOrRemoveChild(fakeExtractLocalizationButton, realDeleteButton);
				extractLocalizationButton.SetText(Language.GetTextValue("tModLoader.ModInfoExtractLocalization"));
				_uIElement.Append(_extractButton);
			}
			Recalculate();
			_modInfo.RemoveChild(_loaderElement);
			_ready = false;
		}
		base.Update(gameTime);
	}
}
