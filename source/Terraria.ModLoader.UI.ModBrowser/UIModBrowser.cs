using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader.Core;
using Terraria.ModLoader.UI.DownloadManager;
using Terraria.ModLoader.UI.Elements;
using Terraria.Social.Base;
using Terraria.UI;
using Terraria.UI.Gamepad;

namespace Terraria.ModLoader.UI.ModBrowser;

internal class UIModBrowser : UIState, IHaveBackButtonCommand
{
	public class UIAsyncList_ModDownloadItem : UIAsyncList<ModDownloadItem, UIModDownloadItem>
	{
		protected override UIModDownloadItem GenElement(ModDownloadItem resource)
		{
			return new UIModDownloadItem(resource);
		}
	}

	public SocialBrowserModule SocialBackend;

	public static bool AvoidGithub;

	public static bool AvoidImgur;

	public static bool EarlyAutoUpdate;

	public UIModDownloadItem SelectedItem;

	public bool reloadOnExit;

	public bool newModInstalled;

	private bool _firstLoad = true;

	private string _specialModPackFilterTitle;

	private List<ModPubId_t> _specialModPackFilter;

	private HashSet<string> modSlugsToUpdateInstallInfo = new HashSet<string>();

	public TimeSpan MinTimeBetweenUpdates = TimeSpan.FromMilliseconds(100.0);

	private Stopwatch DebounceTimer;

	internal bool UpdateNeeded;

	private UIElement _rootElement;

	private UIPanel _backgroundElement;

	public UIAsyncList_ModDownloadItem ModList;

	public UIText NoModsFoundText;

	public UITextPanel<LocalizedText> HeaderTextPanel;

	private UIElement _upperMenuContainer;

	internal readonly List<UICycleImage> CategoryButtons = new List<UICycleImage>();

	private UITextPanel<LocalizedText> _reloadButton;

	private UITextPanel<LocalizedText> _backButton;

	private UITextPanel<string> _clearButton;

	private UITextPanel<LocalizedText> _downloadAllButton;

	private UITextPanel<LocalizedText> _updateAllButton;

	private UIPanel _filterTextBoxBackground;

	internal UIInputTextField FilterTextBox;

	private UIBrowserStatus _browserStatus;

	public UIBrowserFilterToggle<ModBrowserSortMode> SortModeFilterToggle;

	public UIBrowserFilterToggle<UpdateFilter> UpdateFilterToggle;

	public UIBrowserFilterToggle<SearchFilter> SearchFilterToggle;

	public UIBrowserFilterToggle<ModSideFilter> ModSideFilterToggle;

	public static bool PlatformSupportsTls12 => true;

	public bool Loading => !ModList.State.IsFinished();

	public UIState PreviousUIState { get; set; }

	private QueryParameters FilterParameters
	{
		get
		{
			QueryParameters result = default(QueryParameters);
			result.searchTags = new string[1] { SocialBrowserModule.GetBrowserVersionNumber(BuildInfo.tMLVersion) };
			result.searchModIds = SpecialModPackFilter?.ToArray();
			result.searchModSlugs = null;
			result.searchGeneric = ((SearchFilterMode == SearchFilter.Name) ? Filter : null);
			result.searchAuthor = ((SearchFilterMode == SearchFilter.Author) ? Filter : null);
			result.sortingParamater = SortMode;
			result.updateStatusFilter = UpdateFilterMode;
			result.modSideFilter = ModSideFilterMode;
			result.queryType = QueryType.SearchAll;
			return result;
		}
	}

	internal string Filter => FilterTextBox.Text;

	public ModBrowserSortMode SortMode
	{
		get
		{
			return SortModeFilterToggle.State;
		}
		set
		{
			SortModeFilterToggle.SetCurrentState(value);
		}
	}

	public UpdateFilter UpdateFilterMode
	{
		get
		{
			return UpdateFilterToggle.State;
		}
		set
		{
			UpdateFilterToggle.SetCurrentState(value);
		}
	}

	public SearchFilter SearchFilterMode
	{
		get
		{
			return SearchFilterToggle.State;
		}
		set
		{
			SearchFilterToggle.SetCurrentState(value);
		}
	}

	public ModSideFilter ModSideFilterMode
	{
		get
		{
			return ModSideFilterToggle.State;
		}
		set
		{
			ModSideFilterToggle.SetCurrentState(value);
		}
	}

	internal string SpecialModPackFilterTitle
	{
		get
		{
			return _specialModPackFilterTitle;
		}
		set
		{
			_clearButton.SetText(Language.GetTextValue("tModLoader.MBClearSpecialFilter", value));
			_specialModPackFilterTitle = value;
		}
	}

	public List<ModPubId_t> SpecialModPackFilter
	{
		get
		{
			return _specialModPackFilter;
		}
		set
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			if (_specialModPackFilter != null && value == null)
			{
				_backgroundElement.BackgroundColor = UICommon.MainPanelBackground;
				_rootElement.RemoveChild(_clearButton);
				_rootElement.RemoveChild(_downloadAllButton);
			}
			else if (_specialModPackFilter == null && value != null)
			{
				_backgroundElement.BackgroundColor = Color.Purple * 0.7f;
				_rootElement.Append(_clearButton);
				_rootElement.Append(_downloadAllButton);
			}
			_specialModPackFilter = value;
			if (!_firstLoad)
			{
				ModList.SetEnumerable(SocialBackend.QueryBrowser(FilterParameters));
				return;
			}
			throw new NotImplementedException("The ModPack 'View In Browser' option is only valid after one-time opening of Mod Browser");
		}
	}

	public UIModBrowser(SocialBrowserModule socialBackend)
	{
		ModOrganizer.PostLocalModsChanged += CbLocalModsChanged;
		SocialBackend = socialBackend;
	}

	private void CheckIfAnyModUpdateIsAvailable()
	{
		_rootElement.RemoveChild(_updateAllButton);
		List<ModDownloadItem> imods = SocialBackend.GetInstalledModDownloadItems();
		foreach (ModDownloadItem item in imods)
		{
			item.UpdateInstallState();
		}
		if (SpecialModPackFilter == null && imods.Any((ModDownloadItem item) => item.NeedUpdate))
		{
			_rootElement.Append(_updateAllButton);
		}
	}

	private void UpdateAllMods(UIMouseEvent @event, UIElement element)
	{
		List<ModPubId_t> relevantMods = (from item in SocialBackend.GetInstalledModDownloadItems()
			where item.NeedUpdate
			select item.PublishId).ToList();
		DownloadModsAndReturnToBrowser(relevantMods);
	}

	private void ClearTextFilters(UIMouseEvent @event, UIElement element)
	{
		PopulateModBrowser();
		SoundEngine.PlaySound(in SoundID.MenuTick);
	}

	private void DownloadAllFilteredMods(UIMouseEvent @event, UIElement element)
	{
		DownloadModsAndReturnToBrowser(SpecialModPackFilter);
	}

	public override void Draw(SpriteBatch spriteBatch)
	{
		UILinkPointNavigator.Shortcuts.BackButtonCommand = 101;
		base.Draw(spriteBatch);
		for (int i = 0; i < CategoryButtons.Count; i++)
		{
			if (CategoryButtons[i].IsMouseHovering)
			{
				UICommon.DrawHoverStringInBounds(spriteBatch, i switch
				{
					0 => SortMode.ToFriendlyString(), 
					1 => UpdateFilterMode.ToFriendlyString(), 
					2 => ModSideFilterMode.ToFriendlyString(), 
					3 => SearchFilterMode.ToFriendlyString(), 
					_ => "None", 
				});
				break;
			}
		}
		if (_browserStatus.IsMouseHovering && ModList.State != AsyncProviderState.Completed)
		{
			UICommon.DrawHoverStringInBounds(spriteBatch, ModList.GetEndItemText());
		}
	}

	public void HandleBackButtonUsage()
	{
		try
		{
			if (reloadOnExit)
			{
				Main.menuMode = 10006;
			}
			else if (newModInstalled && PreviousUIState == null)
			{
				Main.menuMode = 10000;
			}
			else
			{
				IHaveBackButtonCommand.GoBackTo(PreviousUIState);
			}
		}
		finally
		{
			reloadOnExit = false;
			newModInstalled = false;
		}
	}

	private void ReloadList(UIMouseEvent evt, UIElement listeningElement)
	{
		if (Loading)
		{
			SoundEngine.PlaySound(in SoundID.MenuOpen);
			ModList.Cancel();
		}
		else
		{
			SoundEngine.PlaySound(in SoundID.MenuOpen);
			PopulateModBrowser();
		}
	}

	private void ModListStartLoading(AsyncProviderState state)
	{
		_browserStatus.SetCurrentState(state);
		_reloadButton.SetText(Language.GetText("tModLoader.MBCancelLoading"));
	}

	private void ModListFinished(AsyncProviderState state, Exception e)
	{
		_browserStatus.SetCurrentState(state);
		_reloadButton.SetText(Language.GetText("tModLoader.MBReloadBrowser"));
	}

	public override void OnActivate()
	{
		base.OnActivate();
		Main.clrInput();
		if (_firstLoad)
		{
			SocialBackend.Initialize();
			PopulateModBrowser();
		}
		CheckIfAnyModUpdateIsAvailable();
		DebounceTimer = null;
	}

	private void CbLocalModsChanged(HashSet<string> modSlugs, bool isDeletion)
	{
		if (!_firstLoad)
		{
			lock (modSlugsToUpdateInstallInfo)
			{
				modSlugsToUpdateInstallInfo.UnionWith(modSlugs);
			}
			CheckIfAnyModUpdateIsAvailable();
		}
	}

	public override void OnDeactivate()
	{
		DebounceTimer = null;
		base.OnDeactivate();
	}

	public override void Update(GameTime gameTime)
	{
		base.Update(gameTime);
		lock (modSlugsToUpdateInstallInfo)
		{
			foreach (UIModDownloadItem item in ModList.ReceivedItems.Where((UIModDownloadItem d) => modSlugsToUpdateInstallInfo.Contains(d.ModDownload.ModName)))
			{
				item.ModDownload.UpdateInstallState();
				item.UpdateInstallDisplayState();
			}
			modSlugsToUpdateInstallInfo.Clear();
		}
		if (DebounceTimer != null && DebounceTimer.Elapsed >= MinTimeBetweenUpdates)
		{
			DebounceTimer.Stop();
			DebounceTimer = null;
		}
		if (UpdateNeeded && DebounceTimer == null)
		{
			UpdateNeeded = false;
			PopulateModBrowser();
			DebounceTimer = new Stopwatch();
			DebounceTimer.Start();
		}
	}

	internal void PopulateModBrowser()
	{
		_firstLoad = false;
		SpecialModPackFilter = null;
		SpecialModPackFilterTitle = null;
		SetHeading(Language.GetText("tModLoader.MenuModBrowser"));
		ModList.SetEnumerable(SocialBackend.QueryBrowser(FilterParameters));
	}

	/// <summary>
	///     Enqueues a list of mods, if found on the browser (also used for ModPacks)
	/// </summary>
	internal async void DownloadModsAndReturnToBrowser(List<ModPubId_t> modIds)
	{
		List<string> missingMods;
		List<ModDownloadItem> downloadsQueried = SocialBackend.DirectQueryItems(new QueryParameters
		{
			searchModIds = modIds.ToArray()
		}, out missingMods);
		if (!(await DownloadMods(downloadsQueried)))
		{
			return;
		}
		if (missingMods.Any())
		{
			Interface.errorMessage.Show(Language.GetTextValue("tModLoader.MBModsNotFoundOnline", string.Join(",", missingMods)), 10007);
			return;
		}
		Main.QueueMainThreadAction(delegate
		{
			Main.menuMode = 10007;
		});
	}

	internal Task<bool> DownloadMods(IEnumerable<ModDownloadItem> mods)
	{
		return DownloadMods(mods, 10007, delegate
		{
			reloadOnExit = true;
		}, delegate
		{
			newModInstalled = true;
			_ = ModLoader.autoReloadAndEnableModsLeavingModBrowser;
		});
	}

	/// <summary>
	/// Downloads all ModDownloadItems provided.
	/// </summary>
	internal static async Task<bool> DownloadMods(IEnumerable<ModDownloadItem> mods, int previousMenuId, Action setReloadRequred = null, Action<ModDownloadItem> onNewModInstalled = null)
	{
		HashSet<ModDownloadItem> set = mods.ToHashSet();
		Interface.modBrowser.SocialBackend.GetDependenciesRecursive(set);
		List<ModDownloadItem> fullList = ModDownloadItem.NeedsInstallOrUpdate(set).ToList();
		if (!fullList.Any())
		{
			return true;
		}
		HashSet<string> downloadedList = new HashSet<string>();
		try
		{
			UIWorkshopDownload ui = new UIWorkshopDownload();
			Main.menuMode = 888;
			Main.MenuUI.SetState(ui);
			await Task.Yield();
			foreach (ModDownloadItem mod in fullList)
			{
				bool isInstalled = mod.IsInstalled;
				if (ModLoader.TryGetMod(mod.ModName, out var loadedMod))
				{
					loadedMod.Close();
					mod.Installed = null;
					setReloadRequred?.Invoke();
				}
				Interface.modBrowser.SocialBackend.DownloadItem(mod, ui);
				if (!isInstalled)
				{
					onNewModInstalled?.Invoke(mod);
				}
				downloadedList.Add(mod.ModName);
			}
			return true;
		}
		catch (Exception e)
		{
			LogModBrowserException(e, previousMenuId);
			return false;
		}
		finally
		{
			ModOrganizer.LocalModsChanged(downloadedList, isDeletion: false);
		}
	}

	private void SetHeading(LocalizedText heading)
	{
		HeaderTextPanel.SetText(heading, 0.8f, large: true);
		HeaderTextPanel.Recalculate();
	}

	internal static void LogModBrowserException(Exception e, int returnToMenu)
	{
		Utils.ShowFancyErrorMessage($"{Language.GetTextValue("tModLoader.MBBrowserError")}\n\n{e.Message}\n{e.StackTrace}", returnToMenu);
	}

	internal void Reset()
	{
		ModList?.SetEnumerable();
		SearchFilterToggle?.SetCurrentState(SearchFilter.Name);
		UpdateFilterToggle?.SetCurrentState(UpdateFilter.All);
		ModSideFilterToggle?.SetCurrentState(ModSideFilter.All);
		SortModeFilterToggle?.SetCurrentState(ModBrowserSortMode.RecentlyUpdated);
	}

	private void UpdateHandler(object sender, EventArgs e)
	{
		UpdateNeeded = true;
	}

	private void InitializeInteractions()
	{
		_reloadButton.OnLeftClick += ReloadList;
		_backButton.OnLeftClick += delegate
		{
			HandleBackButtonUsage();
		};
		_clearButton.OnLeftClick += ClearTextFilters;
		_downloadAllButton.OnLeftClick += DownloadAllFilteredMods;
		_updateAllButton.OnLeftClick += UpdateAllMods;
		ModList.OnStartLoading += ModListStartLoading;
		ModList.OnFinished += ModListFinished;
		_filterTextBoxBackground.OnRightClick += delegate
		{
			FilterTextBox.Text = "";
		};
		FilterTextBox.OnRightClick += delegate
		{
			FilterTextBox.Text = "";
		};
		FilterTextBox.OnTextChange += UpdateHandler;
		foreach (UICycleImage categoryButton in CategoryButtons)
		{
			categoryButton.OnStateChanged += UpdateHandler;
		}
	}

	public override void OnInitialize()
	{
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0261: Unknown result type (might be due to invalid IL or missing references)
		//IL_0267: Unknown result type (might be due to invalid IL or missing references)
		//IL_026a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0270: Unknown result type (might be due to invalid IL or missing references)
		//IL_0273: Unknown result type (might be due to invalid IL or missing references)
		//IL_0279: Unknown result type (might be due to invalid IL or missing references)
		//IL_027c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0282: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0302: Unknown result type (might be due to invalid IL or missing references)
		//IL_0308: Unknown result type (might be due to invalid IL or missing references)
		//IL_030b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0311: Unknown result type (might be due to invalid IL or missing references)
		//IL_038e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0398: Unknown result type (might be due to invalid IL or missing references)
		//IL_039d: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03be: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_043f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0449: Unknown result type (might be due to invalid IL or missing references)
		//IL_044e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0453: Unknown result type (might be due to invalid IL or missing references)
		//IL_0458: Unknown result type (might be due to invalid IL or missing references)
		//IL_0462: Unknown result type (might be due to invalid IL or missing references)
		//IL_0469: Unknown result type (might be due to invalid IL or missing references)
		//IL_046f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0472: Unknown result type (might be due to invalid IL or missing references)
		//IL_0478: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0504: Unknown result type (might be due to invalid IL or missing references)
		//IL_0509: Unknown result type (might be due to invalid IL or missing references)
		//IL_0513: Unknown result type (might be due to invalid IL or missing references)
		//IL_051a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0520: Unknown result type (might be due to invalid IL or missing references)
		//IL_0523: Unknown result type (might be due to invalid IL or missing references)
		//IL_0529: Unknown result type (might be due to invalid IL or missing references)
		_rootElement = new UIElement
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
		_backgroundElement = new UIPanel
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
		_rootElement.Append(_backgroundElement);
		ModList = new UIAsyncList_ModDownloadItem
		{
			Width = 
			{
				Pixels = -25f,
				Percent = 1f
			},
			Height = 
			{
				Pixels = -50f,
				Percent = 1f
			},
			Top = 
			{
				Pixels = 50f
			},
			ListPadding = 5f
		};
		UIScrollbar listScrollbar = new UIScrollbar
		{
			Height = 
			{
				Pixels = -50f,
				Percent = 1f
			},
			Top = 
			{
				Pixels = 50f
			},
			HAlign = 1f
		}.WithView(100f, 1000f);
		_backgroundElement.Append(listScrollbar);
		_backgroundElement.Append(ModList);
		ModList.SetScrollbar(listScrollbar);
		HeaderTextPanel = new UITextPanel<LocalizedText>(Language.GetText("tModLoader.MenuModBrowser"), 0.8f, large: true)
		{
			HAlign = 0.5f,
			Top = 
			{
				Pixels = -35f
			},
			BackgroundColor = UICommon.DefaultUIBlue
		}.WithPadding(15f);
		_backgroundElement.Append(HeaderTextPanel);
		_reloadButton = new UITextPanel<LocalizedText>(Language.GetText("tModLoader.MBCancelLoading"))
		{
			Width = 
			{
				Pixels = -10f,
				Percent = 0.5f
			},
			Height = 
			{
				Pixels = 25f
			},
			VAlign = 1f,
			Top = 
			{
				Pixels = -65f
			}
		}.WithFadedMouseOver();
		_backButton = new UITextPanel<LocalizedText>(Language.GetText("UI.Back"))
		{
			Width = 
			{
				Pixels = -10f,
				Percent = 0.5f
			},
			Height = 
			{
				Pixels = 25f
			},
			VAlign = 1f,
			Top = 
			{
				Pixels = -20f
			}
		}.WithFadedMouseOver();
		_clearButton = new UITextPanel<string>(Language.GetTextValue("tModLoader.MBClearSpecialFilter", "??"))
		{
			Width = 
			{
				Pixels = -10f,
				Percent = 0.5f
			},
			Height = 
			{
				Pixels = 25f
			},
			HAlign = 1f,
			VAlign = 1f,
			Top = 
			{
				Pixels = -65f
			},
			BackgroundColor = Color.Purple * 0.7f
		}.WithFadedMouseOver(Color.Purple, Color.Purple * 0.7f);
		_updateAllButton = new UITextPanel<LocalizedText>(Language.GetText("tModLoader.MBUpdateAll"))
		{
			Width = 
			{
				Pixels = -10f,
				Percent = 0.5f
			},
			Height = 
			{
				Pixels = 25f
			},
			HAlign = 1f,
			VAlign = 1f,
			Top = 
			{
				Pixels = -20f
			},
			BackgroundColor = Color.Orange * 0.7f
		}.WithFadedMouseOver(Color.Orange, Color.Orange * 0.7f);
		_downloadAllButton = new UITextPanel<LocalizedText>(Language.GetText("tModLoader.MBDownloadAll"))
		{
			Width = 
			{
				Pixels = -10f,
				Percent = 0.5f
			},
			Height = 
			{
				Pixels = 25f
			},
			HAlign = 1f,
			VAlign = 1f,
			Top = 
			{
				Pixels = -20f
			},
			BackgroundColor = Color.Azure * 0.7f
		}.WithFadedMouseOver(Color.Azure, Color.Azure * 0.7f);
		NoModsFoundText = new UIText(Language.GetTextValue("tModLoader.MBNoModsFound"))
		{
			HAlign = 0.5f
		}.WithPadding(15f);
		FilterTextBox = new UIInputTextField(Language.GetTextValue("tModLoader.ModsTypeToSearch"))
		{
			Top = 
			{
				Pixels = 5f
			},
			Left = 
			{
				Pixels = -160f,
				Percent = 1f
			},
			Width = 
			{
				Pixels = 100f
			},
			Height = 
			{
				Pixels = 20f
			}
		};
		_upperMenuContainer = new UIElement
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
		_filterTextBoxBackground = new UIPanel
		{
			Top = 
			{
				Percent = 0f
			},
			Left = 
			{
				Pixels = -170f,
				Percent = 1f
			},
			Width = 
			{
				Pixels = 135f
			},
			Height = 
			{
				Pixels = 40f
			}
		};
		SortModeFilterToggle = new UIBrowserFilterToggle<ModBrowserSortMode>(0, 0)
		{
			Left = new StyleDimension
			{
				Pixels = 8f
			}
		};
		UpdateFilterToggle = new UIBrowserFilterToggle<UpdateFilter>(34, 0)
		{
			Left = new StyleDimension
			{
				Pixels = 44f
			}
		};
		SearchFilterToggle = new UIBrowserFilterToggle<SearchFilter>(68, 0)
		{
			Left = new StyleDimension
			{
				Pixels = 545f
			}
		};
		ModSideFilterToggle = new UIBrowserFilterToggle<ModSideFilter>(170, 0)
		{
			Left = new StyleDimension
			{
				Pixels = 80f
			}
		};
		SearchFilterToggle.SetCurrentState(SearchFilter.Name);
		UpdateFilterToggle.SetCurrentState(UpdateFilter.All);
		ModSideFilterToggle.SetCurrentState(ModSideFilter.All);
		SortModeFilterToggle.SetCurrentState(ModBrowserSortMode.RecentlyUpdated);
		_browserStatus = new UIBrowserStatus
		{
			VAlign = 1f,
			Top = 
			{
				Pixels = -72f
			},
			Left = 
			{
				Pixels = 545f
			}
		};
		_rootElement.Append(_browserStatus);
		_rootElement.Append(_reloadButton);
		_rootElement.Append(_backButton);
		CategoryButtons.Add(SortModeFilterToggle);
		_upperMenuContainer.Append(SortModeFilterToggle);
		CategoryButtons.Add(UpdateFilterToggle);
		_upperMenuContainer.Append(UpdateFilterToggle);
		CategoryButtons.Add(ModSideFilterToggle);
		_upperMenuContainer.Append(ModSideFilterToggle);
		CategoryButtons.Add(SearchFilterToggle);
		_upperMenuContainer.Append(SearchFilterToggle);
		InitializeInteractions();
		_upperMenuContainer.Append(_filterTextBoxBackground);
		_upperMenuContainer.Append(FilterTextBox);
		_backgroundElement.Append(_upperMenuContainer);
		Append(_rootElement);
	}
}
