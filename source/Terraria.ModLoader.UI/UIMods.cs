using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.Localization;
using Terraria.ModLoader.Config;
using Terraria.ModLoader.Core;
using Terraria.ModLoader.UI.ModBrowser;
using Terraria.UI;
using Terraria.UI.Gamepad;

namespace Terraria.ModLoader.UI;

internal class UIMods : UIState, IHaveBackButtonCommand
{
	private UIElement uIElement;

	private UIPanel uIPanel;

	private UILoaderAnimatedImage uiLoader;

	private bool needToRemoveLoading;

	private UIList modList;

	private float modListViewPosition;

	private readonly List<UIModItem> items = new List<UIModItem>();

	private bool updateNeeded;

	public bool loading;

	private UIInputTextField filterTextBox;

	public UICycleImage SearchFilterToggle;

	public ModsMenuSortMode sortMode;

	public EnabledFilter enabledFilterMode;

	public ModSideFilter modSideFilterMode;

	public SearchFilter searchFilterMode;

	internal readonly List<UICycleImage> _categoryButtons = new List<UICycleImage>();

	internal string filter;

	private UIAutoScaleTextTextPanel<LocalizedText> buttonEA;

	private UIAutoScaleTextTextPanel<LocalizedText> buttonDA;

	private UIAutoScaleTextTextPanel<LocalizedText> buttonRM;

	private UIAutoScaleTextTextPanel<LocalizedText> buttonB;

	private UIAutoScaleTextTextPanel<LocalizedText> buttonOMF;

	private UIAutoScaleTextTextPanel<LocalizedText> buttonCL;

	private CancellationTokenSource _cts;

	public UIState PreviousUIState { get; set; }

	private bool forceReloadHidden
	{
		get
		{
			if (ModLoader.autoReloadRequiredModsLeavingModsScreen)
			{
				return !ModCompile.DeveloperMode;
			}
			return false;
		}
	}

	public override void OnInitialize()
	{
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_024d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0252: Unknown result type (might be due to invalid IL or missing references)
		//IL_0285: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02db: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_035b: Unknown result type (might be due to invalid IL or missing references)
		//IL_037d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0383: Unknown result type (might be due to invalid IL or missing references)
		//IL_0387: Unknown result type (might be due to invalid IL or missing references)
		//IL_038d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0391: Unknown result type (might be due to invalid IL or missing references)
		//IL_0397: Unknown result type (might be due to invalid IL or missing references)
		//IL_039b: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_042f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0435: Unknown result type (might be due to invalid IL or missing references)
		//IL_0439: Unknown result type (might be due to invalid IL or missing references)
		//IL_043f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0443: Unknown result type (might be due to invalid IL or missing references)
		//IL_0449: Unknown result type (might be due to invalid IL or missing references)
		//IL_044d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0453: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0505: Unknown result type (might be due to invalid IL or missing references)
		//IL_057d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0583: Unknown result type (might be due to invalid IL or missing references)
		//IL_0587: Unknown result type (might be due to invalid IL or missing references)
		//IL_058d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0591: Unknown result type (might be due to invalid IL or missing references)
		//IL_0597: Unknown result type (might be due to invalid IL or missing references)
		//IL_059b: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0952: Unknown result type (might be due to invalid IL or missing references)
		//IL_0958: Unknown result type (might be due to invalid IL or missing references)
		//IL_095c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0962: Unknown result type (might be due to invalid IL or missing references)
		//IL_0966: Unknown result type (might be due to invalid IL or missing references)
		//IL_096c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0970: Unknown result type (might be due to invalid IL or missing references)
		//IL_0976: Unknown result type (might be due to invalid IL or missing references)
		uIElement = new UIElement
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
			BackgroundColor = UICommon.MainPanelBackground,
			PaddingTop = 0f
		};
		uIElement.Append(uIPanel);
		uiLoader = new UILoaderAnimatedImage(0.5f, 0.5f);
		modList = new UIList
		{
			Width = 
			{
				Pixels = -25f,
				Percent = 1f
			},
			Height = 
			{
				Pixels = (ModLoader.showMemoryEstimates ? (-72) : (-50)),
				Percent = 1f
			},
			Top = 
			{
				Pixels = (ModLoader.showMemoryEstimates ? 72 : 50)
			},
			ListPadding = 5f
		};
		uIPanel.Append(modList);
		if (ModLoader.showMemoryEstimates)
		{
			UIMemoryBar ramUsage = new UIMemoryBar
			{
				Top = 
				{
					Pixels = 45f
				}
			};
			ramUsage.Width.Pixels = -25f;
			uIPanel.Append(ramUsage);
		}
		UIScrollbar uIScrollbar = new UIScrollbar
		{
			Height = 
			{
				Pixels = (ModLoader.showMemoryEstimates ? (-72) : (-50)),
				Percent = 1f
			},
			Top = 
			{
				Pixels = (ModLoader.showMemoryEstimates ? 72 : 50)
			},
			HAlign = 1f
		}.WithView(100f, 1000f);
		uIPanel.Append(uIScrollbar);
		modList.SetScrollbar(uIScrollbar);
		UITextPanel<LocalizedText> uIHeaderTexTPanel = new UITextPanel<LocalizedText>(Language.GetText("tModLoader.ModsModsList"), 0.8f, large: true)
		{
			HAlign = 0.5f,
			Top = 
			{
				Pixels = -35f
			},
			BackgroundColor = UICommon.DefaultUIBlue
		}.WithPadding(15f);
		uIElement.Append(uIHeaderTexTPanel);
		buttonEA = new UIAutoScaleTextTextPanel<LocalizedText>(Language.GetText("tModLoader.ModsEnableAll"))
		{
			TextColor = Color.Green,
			Width = new StyleDimension(-10f, 1f / 3f),
			Height = 
			{
				Pixels = 40f
			},
			VAlign = 1f,
			Top = 
			{
				Pixels = -65f
			}
		}.WithFadedMouseOver();
		buttonEA.OnLeftClick += EnableAll;
		uIElement.Append(buttonEA);
		buttonDA = new UIAutoScaleTextTextPanel<LocalizedText>(Language.GetText("tModLoader.ModsDisableAll"));
		buttonDA.CopyStyle(buttonEA);
		buttonDA.TextColor = Color.Red;
		buttonDA.HAlign = 0.5f;
		buttonDA.WithFadedMouseOver();
		buttonDA.OnLeftClick += DisableAll;
		uIElement.Append(buttonDA);
		buttonRM = new UIAutoScaleTextTextPanel<LocalizedText>(Language.GetText("tModLoader.ModsForceReload"));
		buttonRM.CopyStyle(buttonEA);
		buttonRM.Width = new StyleDimension(-10f, 1f / 3f);
		buttonRM.HAlign = 1f;
		buttonRM.WithFadedMouseOver();
		buttonRM.OnLeftClick += ReloadMods;
		uIElement.Append(buttonRM);
		UpdateTopRowButtons();
		buttonB = new UIAutoScaleTextTextPanel<LocalizedText>(Language.GetText("UI.Back"))
		{
			Width = new StyleDimension(-10f, 1f / 3f),
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
		buttonB.OnLeftClick += BackClick;
		uIElement.Append(buttonB);
		buttonOMF = new UIAutoScaleTextTextPanel<LocalizedText>(Language.GetText("tModLoader.ModsOpenModsFolders"));
		buttonOMF.CopyStyle(buttonB);
		buttonOMF.HAlign = 0.5f;
		buttonOMF.WithFadedMouseOver();
		buttonOMF.OnLeftClick += OpenModsFolder;
		uIElement.Append(buttonOMF);
		Asset<Texture2D> texture = UICommon.ModBrowserIconsTexture;
		UIElement upperMenuContainer = new UIElement
		{
			Width = 
			{
				Percent = 1f
			},
			Height = 
			{
				Pixels = 32f
			},
			Top = 
			{
				Pixels = 10f
			}
		};
		for (int i = 0; i < 3; i++)
		{
			UICycleImage toggleImage;
			switch (i)
			{
			case 0:
				toggleImage = new UICycleImage(texture, 3, 32, 32, 102, 0);
				toggleImage.SetCurrentState((int)sortMode);
				toggleImage.OnLeftClick += delegate
				{
					sortMode = sortMode.NextEnum();
					updateNeeded = true;
				};
				toggleImage.OnRightClick += delegate
				{
					sortMode = sortMode.PreviousEnum();
					updateNeeded = true;
				};
				break;
			case 1:
				toggleImage = new UICycleImage(texture, 3, 32, 32, 136, 0);
				toggleImage.SetCurrentState((int)enabledFilterMode);
				toggleImage.OnLeftClick += delegate
				{
					enabledFilterMode = enabledFilterMode.NextEnum();
					updateNeeded = true;
				};
				toggleImage.OnRightClick += delegate
				{
					enabledFilterMode = enabledFilterMode.PreviousEnum();
					updateNeeded = true;
				};
				break;
			default:
				toggleImage = new UICycleImage(texture, 5, 32, 32, 170, 0);
				toggleImage.SetCurrentState((int)modSideFilterMode);
				toggleImage.OnLeftClick += delegate
				{
					modSideFilterMode = modSideFilterMode.NextEnum();
					updateNeeded = true;
				};
				toggleImage.OnRightClick += delegate
				{
					modSideFilterMode = modSideFilterMode.PreviousEnum();
					updateNeeded = true;
				};
				break;
			}
			toggleImage.Left.Pixels = i * 36 + 8;
			_categoryButtons.Add(toggleImage);
			upperMenuContainer.Append(toggleImage);
		}
		UIPanel filterTextBoxBackground = new UIPanel
		{
			Top = 
			{
				Percent = 0f
			},
			Left = 
			{
				Pixels = -185f,
				Percent = 1f
			},
			Width = 
			{
				Pixels = 150f
			},
			Height = 
			{
				Pixels = 40f
			}
		};
		filterTextBoxBackground.SetPadding(0f);
		filterTextBoxBackground.OnRightClick += ClearSearchField;
		upperMenuContainer.Append(filterTextBoxBackground);
		filterTextBox = new UIInputTextField(Language.GetTextValue("tModLoader.ModsTypeToSearch"))
		{
			Top = 
			{
				Pixels = 5f
			},
			Height = 
			{
				Percent = 1f
			},
			Width = 
			{
				Percent = 1f
			},
			Left = 
			{
				Pixels = 5f
			},
			VAlign = 0.5f
		};
		filterTextBox.OnTextChange += delegate
		{
			updateNeeded = true;
		};
		filterTextBoxBackground.Append(filterTextBox);
		UIImageButton clearSearchButton = new UIImageButton(Main.Assets.Request<Texture2D>("Images/UI/SearchCancel"))
		{
			HAlign = 1f,
			VAlign = 0.5f,
			Left = new StyleDimension(-2f, 0f)
		};
		clearSearchButton.OnLeftClick += ClearSearchField;
		filterTextBoxBackground.Append(clearSearchButton);
		SearchFilterToggle = new UICycleImage(texture, 2, 32, 32, 68, 0)
		{
			Left = 
			{
				Pixels = 545f
			}
		};
		SearchFilterToggle.SetCurrentState((int)searchFilterMode);
		SearchFilterToggle.OnLeftClick += delegate
		{
			searchFilterMode = searchFilterMode.NextEnum();
			updateNeeded = true;
		};
		SearchFilterToggle.OnRightClick += delegate
		{
			searchFilterMode = searchFilterMode.PreviousEnum();
			updateNeeded = true;
		};
		_categoryButtons.Add(SearchFilterToggle);
		upperMenuContainer.Append(SearchFilterToggle);
		buttonCL = new UIAutoScaleTextTextPanel<LocalizedText>(Language.GetText("tModLoader.ModConfiguration"));
		buttonCL.CopyStyle(buttonOMF);
		buttonCL.HAlign = 1f;
		buttonCL.WithFadedMouseOver();
		buttonCL.OnLeftClick += GotoModConfigList;
		uIElement.Append(buttonCL);
		uIPanel.Append(upperMenuContainer);
		Append(uIElement);
	}

	private void ClearSearchField(UIMouseEvent evt, UIElement listeningElement)
	{
		filterTextBox.Text = "";
	}

	private void UpdateTopRowButtons()
	{
		StyleDimension buttonWidth = new StyleDimension(-10f, 1f / (forceReloadHidden ? 2f : 3f));
		buttonEA.Width = buttonWidth;
		buttonDA.Width = buttonWidth;
		buttonDA.HAlign = (forceReloadHidden ? 1f : 0.5f);
		uIElement.AddOrRemoveChild(buttonRM, ModCompile.DeveloperMode || !forceReloadHidden);
	}

	internal void BackClick(UIMouseEvent evt, UIElement listeningElement)
	{
		if (ConfigManager.AnyModNeedsReload())
		{
			Main.menuMode = 10006;
			return;
		}
		if (ModLoader.autoReloadRequiredModsLeavingModsScreen && items.Count((UIModItem i) => i.NeedsReload) > 0)
		{
			Main.menuMode = 10006;
			return;
		}
		ConfigManager.OnChangedAll();
		((IHaveBackButtonCommand)this).HandleBackButtonUsage();
	}

	private void ReloadMods(UIMouseEvent evt, UIElement listeningElement)
	{
		SoundEngine.PlaySound(10);
		if (items.Count > 0)
		{
			ModLoader.Reload();
		}
	}

	private static void OpenModsFolder(UIMouseEvent evt, UIElement listeningElement)
	{
		SoundEngine.PlaySound(10);
		Directory.CreateDirectory(ModLoader.ModPath);
		Utils.OpenFolder(ModLoader.ModPath);
		if (ModOrganizer.WorkshopFileFinder.ModPaths.Any())
		{
			Utils.OpenFolder(Directory.GetParent(ModOrganizer.WorkshopFileFinder.ModPaths[0]).ToString());
		}
	}

	private void EnableAll(UIMouseEvent evt, UIElement listeningElement)
	{
		SoundEngine.PlaySound(12);
		foreach (UIModItem item in items)
		{
			item.Enable();
		}
	}

	private void DisableAll(UIMouseEvent evt, UIElement listeningElement)
	{
		SoundEngine.PlaySound(12);
		foreach (UIModItem item in items)
		{
			item.Disable();
		}
	}

	private void GotoModConfigList(UIMouseEvent evt, UIElement listeningElement)
	{
		SoundEngine.PlaySound(10);
		Main.menuMode = 10027;
	}

	public UIModItem FindUIModItem(string modName)
	{
		return items.SingleOrDefault((UIModItem m) => m.ModName == modName);
	}

	public override void Update(GameTime gameTime)
	{
		base.Update(gameTime);
		if (needToRemoveLoading)
		{
			needToRemoveLoading = false;
			uIPanel.RemoveChild(uiLoader);
		}
		if (!updateNeeded)
		{
			return;
		}
		updateNeeded = false;
		filter = filterTextBox.Text;
		modList.Clear();
		UIModsFilterResults filterResults = new UIModsFilterResults();
		List<UIModItem> visibleItems = items.Where((UIModItem item) => item.PassFilters(filterResults)).ToList();
		if (filterResults.AnyFiltered)
		{
			UIPanel panel = new UIPanel();
			panel.Width.Set(0f, 1f);
			modList.Add(panel);
			List<string> filterMessages = new List<string>();
			if (filterResults.filteredByEnabled > 0)
			{
				filterMessages.Add(Language.GetTextValue("tModLoader.ModsXModsFilteredByEnabled", filterResults.filteredByEnabled));
			}
			if (filterResults.filteredByModSide > 0)
			{
				filterMessages.Add(Language.GetTextValue("tModLoader.ModsXModsFilteredByModSide", filterResults.filteredByModSide));
			}
			if (filterResults.filteredBySearch > 0)
			{
				filterMessages.Add(Language.GetTextValue("tModLoader.ModsXModsFilteredBySearch", filterResults.filteredBySearch));
			}
			UIText text = new UIText(string.Join("\n", filterMessages));
			text.Width.Set(0f, 1f);
			text.IsWrapped = true;
			text.WrappedTextBottomPadding = 0f;
			text.TextOriginX = 0f;
			text.Recalculate();
			panel.Append(text);
			panel.Height.Set(text.MinHeight.Pixels + panel.PaddingTop, 0f);
		}
		modList.AddRange(visibleItems);
		Recalculate();
		modList.ViewPosition = modListViewPosition;
	}

	public override void Draw(SpriteBatch spriteBatch)
	{
		UILinkPointNavigator.Shortcuts.BackButtonCommand = 102;
		base.Draw(spriteBatch);
		for (int i = 0; i < _categoryButtons.Count; i++)
		{
			if (_categoryButtons[i].IsMouseHovering)
			{
				UICommon.DrawHoverStringInBounds(spriteBatch, i switch
				{
					0 => sortMode.ToFriendlyString(), 
					1 => enabledFilterMode.ToFriendlyString(), 
					2 => modSideFilterMode.ToFriendlyString(), 
					3 => searchFilterMode.ToFriendlyString(), 
					_ => "None", 
				});
				return;
			}
		}
		if (buttonOMF.IsMouseHovering)
		{
			UICommon.DrawHoverStringInBounds(spriteBatch, Language.GetTextValue("tModLoader.ModsOpenModsFoldersTooltip"));
		}
	}

	public override void OnActivate()
	{
		_cts = new CancellationTokenSource();
		Main.clrInput();
		modList.Clear();
		items.Clear();
		loading = true;
		uIPanel.Append(uiLoader);
		ConfigManager.LoadAll();
		Populate();
		UpdateTopRowButtons();
	}

	public override void OnDeactivate()
	{
		_cts?.Cancel(throwOnFirstException: false);
		_cts?.Dispose();
		_cts = null;
		modListViewPosition = modList.ViewPosition;
	}

	internal void Populate()
	{
		Task.Run(delegate
		{
			LocalMod[] array = ModOrganizer.FindMods(logDuplicates: true);
			for (int i = 0; i < array.Length; i++)
			{
				UIModItem uIModItem = new UIModItem(array[i]);
				uIModItem.Activate();
				items.Add(uIModItem);
			}
			needToRemoveLoading = true;
			updateNeeded = true;
			loading = false;
		});
	}
}
