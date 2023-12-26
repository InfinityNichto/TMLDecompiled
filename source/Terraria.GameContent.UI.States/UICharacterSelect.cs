using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.OS;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.IO;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.Config.UI;
using Terraria.ModLoader.UI;
using Terraria.UI;
using Terraria.UI.Gamepad;
using Terraria.Utilities;

namespace Terraria.GameContent.UI.States;

public class UICharacterSelect : UIState
{
	internal UIList _playerList;

	private UITextPanel<LocalizedText> _backPanel;

	private UITextPanel<LocalizedText> _newPanel;

	private UIPanel _containerPanel;

	private UIScrollbar _scrollbar;

	private bool _isScrollbarAttached;

	private List<Tuple<string, bool>> favoritesCache = new List<Tuple<string, bool>>();

	private bool skipDraw;

	private static bool _currentlyMigratingFiles;

	private static UIExpandablePanel _migrationPanel;

	public override void OnInitialize()
	{
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
		UIElement uIElement = new UIElement();
		uIElement.Width.Set(0f, 0.8f);
		uIElement.MaxWidth.Set(650f, 0f);
		uIElement.Top.Set(220f, 0f);
		uIElement.Height.Set(-220f, 1f);
		uIElement.HAlign = 0.5f;
		UIPanel uIPanel = new UIPanel();
		uIPanel.Width.Set(0f, 1f);
		uIPanel.Height.Set(-110f, 1f);
		uIPanel.BackgroundColor = new Color(33, 43, 79) * 0.8f;
		_containerPanel = uIPanel;
		uIElement.Append(uIPanel);
		_playerList = new UIList();
		_playerList.Width.Set(0f, 1f);
		_playerList.Height.Set(0f, 1f);
		_playerList.ListPadding = 5f;
		uIPanel.Append(_playerList);
		_scrollbar = new UIScrollbar();
		_scrollbar.SetView(100f, 1000f);
		_scrollbar.Height.Set(0f, 1f);
		_scrollbar.HAlign = 1f;
		_playerList.SetScrollbar(_scrollbar);
		UITextPanel<LocalizedText> uITextPanel = new UITextPanel<LocalizedText>(Language.GetText("UI.SelectPlayer"), 0.8f, large: true);
		uITextPanel.HAlign = 0.5f;
		uITextPanel.Top.Set(-40f, 0f);
		uITextPanel.SetPadding(15f);
		uITextPanel.BackgroundColor = new Color(73, 94, 171);
		uIElement.Append(uITextPanel);
		UITextPanel<LocalizedText> uITextPanel2 = new UITextPanel<LocalizedText>(Language.GetText("UI.Back"), 0.7f, large: true);
		uITextPanel2.Width.Set(-10f, 0.5f);
		uITextPanel2.Height.Set(50f, 0f);
		uITextPanel2.VAlign = 1f;
		uITextPanel2.Top.Set(-45f, 0f);
		uITextPanel2.OnMouseOver += FadedMouseOver;
		uITextPanel2.OnMouseOut += FadedMouseOut;
		uITextPanel2.OnLeftClick += GoBackClick;
		uITextPanel2.SetSnapPoint("Back", 0);
		uIElement.Append(uITextPanel2);
		_backPanel = uITextPanel2;
		UITextPanel<LocalizedText> uITextPanel3 = new UITextPanel<LocalizedText>(Language.GetText("UI.New"), 0.7f, large: true);
		uITextPanel3.CopyStyle(uITextPanel2);
		uITextPanel3.HAlign = 1f;
		uITextPanel3.OnMouseOver += FadedMouseOver;
		uITextPanel3.OnMouseOut += FadedMouseOut;
		uITextPanel3.OnLeftClick += NewCharacterClick;
		uIElement.Append(uITextPanel3);
		uITextPanel2.SetSnapPoint("New", 0);
		_newPanel = uITextPanel3;
		Append(uIElement);
	}

	public override void Recalculate()
	{
		if (_scrollbar != null)
		{
			if (_isScrollbarAttached && !_scrollbar.CanScroll)
			{
				_containerPanel.RemoveChild(_scrollbar);
				_isScrollbarAttached = false;
				_playerList.Width.Set(0f, 1f);
			}
			else if (!_isScrollbarAttached && _scrollbar.CanScroll)
			{
				_containerPanel.Append(_scrollbar);
				_isScrollbarAttached = true;
				_playerList.Width.Set(-25f, 1f);
			}
		}
		base.Recalculate();
	}

	private void NewCharacterClick(UIMouseEvent evt, UIElement listeningElement)
	{
		SoundEngine.PlaySound(10);
		Main.PendingPlayer = new Player();
		Main.menuMode = 888;
		Main.MenuUI.SetState(new UICharacterCreation(Main.PendingPlayer));
	}

	private void GoBackClick(UIMouseEvent evt, UIElement listeningElement)
	{
		SoundEngine.PlaySound(11);
		Main.menuMode = 0;
	}

	private void FadedMouseOver(UIMouseEvent evt, UIElement listeningElement)
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		SoundEngine.PlaySound(12);
		((UIPanel)evt.Target).BackgroundColor = new Color(73, 94, 171);
		((UIPanel)evt.Target).BorderColor = Colors.FancyUIFatButtonMouseOver;
	}

	private void FadedMouseOut(UIMouseEvent evt, UIElement listeningElement)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		((UIPanel)evt.Target).BackgroundColor = new Color(63, 82, 151) * 0.7f;
		((UIPanel)evt.Target).BorderColor = Color.Black;
	}

	public override void OnActivate()
	{
		Main.LoadPlayers();
		Main.ActivePlayerFileData = new PlayerFileData();
		LoadMigratablePlayers();
		UpdatePlayersList();
		if (PlayerInput.UsingGamepadUI)
		{
			UILinkPointNavigator.ChangePoint(3000 + ((_playerList.Count == 0) ? 1 : 2));
		}
	}

	private void UpdatePlayersList()
	{
		_playerList.Clear();
		List<PlayerFileData> list = new List<PlayerFileData>(Main.PlayerList);
		list.Sort(delegate(PlayerFileData x, PlayerFileData y)
		{
			if (x.IsFavorite && !y.IsFavorite)
			{
				return -1;
			}
			if (!x.IsFavorite && y.IsFavorite)
			{
				return 1;
			}
			return (x.Name.CompareTo(y.Name) == 0) ? x.GetFileName().CompareTo(y.GetFileName()) : x.Name.CompareTo(y.Name);
		});
		int num = 0;
		foreach (PlayerFileData item in list)
		{
			_playerList.Add(new UICharacterListItem(item, num++));
		}
		_playerList.Add(_migrationPanel);
		if (list.Count == 0)
		{
			AddAutomaticPlayerMigrationButtons();
		}
	}

	public override void Draw(SpriteBatch spriteBatch)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Expected O, but got Unknown
		if (skipDraw)
		{
			skipDraw = false;
			return;
		}
		if (UpdateFavoritesCache())
		{
			skipDraw = true;
			Main.MenuUI.Draw(spriteBatch, new GameTime());
		}
		base.Draw(spriteBatch);
		SetupGamepadPoints(spriteBatch);
	}

	private bool UpdateFavoritesCache()
	{
		List<PlayerFileData> list = new List<PlayerFileData>(Main.PlayerList);
		list.Sort(delegate(PlayerFileData x, PlayerFileData y)
		{
			if (x.IsFavorite && !y.IsFavorite)
			{
				return -1;
			}
			if (!x.IsFavorite && y.IsFavorite)
			{
				return 1;
			}
			return (x.Name.CompareTo(y.Name) == 0) ? x.GetFileName().CompareTo(y.GetFileName()) : x.Name.CompareTo(y.Name);
		});
		bool flag = false;
		if (!flag && list.Count != favoritesCache.Count)
		{
			flag = true;
		}
		if (!flag)
		{
			for (int i = 0; i < favoritesCache.Count; i++)
			{
				Tuple<string, bool> tuple = favoritesCache[i];
				if (!(list[i].Name == tuple.Item1) || list[i].IsFavorite != tuple.Item2)
				{
					flag = true;
					break;
				}
			}
		}
		if (flag)
		{
			favoritesCache.Clear();
			foreach (PlayerFileData item in list)
			{
				favoritesCache.Add(Tuple.Create(item.Name, item.IsFavorite));
			}
			UpdatePlayersList();
		}
		return flag;
	}

	private void SetupGamepadPoints(SpriteBatch spriteBatch)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_0382: Unknown result type (might be due to invalid IL or missing references)
		UILinkPointNavigator.Shortcuts.BackButtonCommand = 1;
		int num = 3000;
		Rectangle val = _backPanel.GetInnerDimensions().ToRectangle();
		UILinkPointNavigator.SetPosition(num, ((Rectangle)(ref val)).Center.ToVector2());
		int iD = num + 1;
		val = _newPanel.GetInnerDimensions().ToRectangle();
		UILinkPointNavigator.SetPosition(iD, ((Rectangle)(ref val)).Center.ToVector2());
		int num2 = num;
		UILinkPoint uILinkPoint = UILinkPointNavigator.Points[num2];
		uILinkPoint.Unlink();
		uILinkPoint.Right = num2 + 1;
		num2 = num + 1;
		uILinkPoint = UILinkPointNavigator.Points[num2];
		uILinkPoint.Unlink();
		uILinkPoint.Left = num2 - 1;
		float num3 = 1f / Main.UIScale;
		Rectangle clippingRectangle = _containerPanel.GetClippingRectangle(spriteBatch);
		Vector2 minimum = clippingRectangle.TopLeft() * num3;
		Vector2 maximum = clippingRectangle.BottomRight() * num3;
		List<SnapPoint> snapPoints = GetSnapPoints();
		for (int i = 0; i < snapPoints.Count; i++)
		{
			if (!snapPoints[i].Position.Between(minimum, maximum))
			{
				snapPoints.Remove(snapPoints[i]);
				i--;
			}
		}
		int num4 = 5;
		SnapPoint[,] array = new SnapPoint[_playerList.Count, num4];
		foreach (SnapPoint item in snapPoints.Where((SnapPoint a) => a.Name == "Play"))
		{
			array[item.Id, 0] = item;
		}
		foreach (SnapPoint item2 in snapPoints.Where((SnapPoint a) => a.Name == "Favorite"))
		{
			array[item2.Id, 1] = item2;
		}
		foreach (SnapPoint item3 in snapPoints.Where((SnapPoint a) => a.Name == "Cloud"))
		{
			array[item3.Id, 2] = item3;
		}
		foreach (SnapPoint item4 in snapPoints.Where((SnapPoint a) => a.Name == "Rename"))
		{
			array[item4.Id, 3] = item4;
		}
		foreach (SnapPoint item5 in snapPoints.Where((SnapPoint a) => a.Name == "Delete"))
		{
			array[item5.Id, 4] = item5;
		}
		num2 = num + 2;
		int[] array2 = new int[_playerList.Count];
		for (int j = 0; j < array2.Length; j++)
		{
			array2[j] = -1;
		}
		for (int k = 0; k < num4; k++)
		{
			int num5 = -1;
			for (int l = 0; l < array.GetLength(0); l++)
			{
				if (array[l, k] != null)
				{
					uILinkPoint = UILinkPointNavigator.Points[num2];
					uILinkPoint.Unlink();
					UILinkPointNavigator.SetPosition(num2, array[l, k].Position);
					if (num5 != -1)
					{
						uILinkPoint.Up = num5;
						UILinkPointNavigator.Points[num5].Down = num2;
					}
					if (array2[l] != -1)
					{
						uILinkPoint.Left = array2[l];
						UILinkPointNavigator.Points[array2[l]].Right = num2;
					}
					uILinkPoint.Down = num;
					if (k == 0)
					{
						UILinkPointNavigator.Points[num].Up = (UILinkPointNavigator.Points[num + 1].Up = num2);
					}
					num5 = num2;
					array2[l] = num2;
					UILinkPointNavigator.Shortcuts.FANCYUI_HIGHEST_INDEX = num2;
					num2++;
				}
			}
		}
		if (PlayerInput.UsingGamepadUI && _playerList.Count == 0 && UILinkPointNavigator.CurrentPoint > 3001)
		{
			UILinkPointNavigator.ChangePoint(3001);
		}
	}

	private void LoadMigratablePlayers()
	{
		_migrationPanel = new UIExpandablePanel();
		UIText playerMigrationPanelTitle = new UIText(Language.GetTextValue("tModLoader.MigrateIndividualPlayersHeader"));
		playerMigrationPanelTitle.Top.Set(4f, 0f);
		_migrationPanel.Append(playerMigrationPanelTitle);
		NestedUIList migratePlayerList = new NestedUIList();
		migratePlayerList.Width.Set(-22f, 1f);
		migratePlayerList.Left.Set(0f, 0f);
		migratePlayerList.Top.Set(30f, 0f);
		migratePlayerList.MinHeight.Set(300f, 0f);
		migratePlayerList.ListPadding = 5f;
		_migrationPanel.VisibleWhenExpanded.Add(migratePlayerList);
		UIScrollbar scrollbar = new UIScrollbar();
		scrollbar.SetView(100f, 1000f);
		scrollbar.Height.Set(-42f, 1f);
		scrollbar.Top.Set(36f, 0f);
		scrollbar.Left.Pixels -= 0f;
		scrollbar.HAlign = 1f;
		migratePlayerList.SetScrollbar(scrollbar);
		_migrationPanel.VisibleWhenExpanded.Add(scrollbar);
		(string, string, int)[] otherPaths = FileUtilities.GetAlternateSavePathFiles("Players");
		int currentStabilityLevel = BuildInfo.Purpose switch
		{
			BuildInfo.BuildPurpose.Stable => 1, 
			BuildInfo.BuildPurpose.Preview => 2, 
			_ => 3, 
		};
		(string, string, int)[] array = otherPaths;
		for (int j = 0; j < array.Length; j++)
		{
			var (otherSaveFolderPath, message, stabilityLevel) = array[j];
			if (stabilityLevel == currentStabilityLevel || !Directory.Exists(otherSaveFolderPath))
			{
				continue;
			}
			string[] files = Directory.GetFiles(otherSaveFolderPath, "*.plr");
			int num2 = Math.Min(1000, files.Length);
			for (int i = 0; i < num2; i++)
			{
				string playerInThisPlayersPath = Path.Combine(Main.PlayerPath, Path.GetFileName(files[i]));
				if (File.Exists(playerInThisPlayersPath) && File.GetLastWriteTime(playerInThisPlayersPath) == File.GetLastWriteTime(files[i]))
				{
					continue;
				}
				PlayerFileData fileData = Player.GetFileData(files[i], cloudSave: false);
				if (fileData == null)
				{
					continue;
				}
				UIPanel migrateIndividualPlayerPanel = new UIPanel();
				migrateIndividualPlayerPanel.Width.Set(0f, 1f);
				migrateIndividualPlayerPanel.Height.Set(50f, 0f);
				float left = 0f;
				if (stabilityLevel > currentStabilityLevel)
				{
					UIHoverImage warningImage2 = new UIHoverImage(UICommon.ButtonErrorTexture, Language.GetTextValue("tModLoader.PlayerFromNewerTModMightNotWork"))
					{
						Left = 
						{
							Pixels = left
						},
						Top = 
						{
							Pixels = 3f
						}
					};
					migrateIndividualPlayerPanel.Append(warningImage2);
					left += warningImage2.Width.Pixels + 6f;
				}
				PlayerFileData playerWithSameName = Main.PlayerList.FirstOrDefault((PlayerFileData x) => x.Name == fileData.Name);
				if (playerWithSameName != null)
				{
					UIHoverImage warningImage = new UIHoverImage(UICommon.ButtonExclamationTexture, Language.GetTextValue("tModLoader.PlayerWithThisNameExistsWillBeOverwritten"))
					{
						Left = 
						{
							Pixels = left
						},
						Top = 
						{
							Pixels = 3f
						}
					};
					migrateIndividualPlayerPanel.Append(warningImage);
					left += warningImage.Width.Pixels + 6f;
					if (File.GetLastWriteTime(playerWithSameName.Path) > File.GetLastWriteTime(files[i]))
					{
						warningImage = new UIHoverImage(UICommon.ButtonExclamationTexture, Language.GetTextValue("tModLoader.ExistingPlayerPlayedMoreRecently"))
						{
							Left = 
							{
								Pixels = left
							},
							Top = 
							{
								Pixels = 3f
							}
						};
						migrateIndividualPlayerPanel.Append(warningImage);
						left += warningImage.Width.Pixels + 6f;
					}
				}
				UIText migrateIndividualPlayerText = new UIText(string.Format(message, fileData.Name));
				migrateIndividualPlayerText.Width.Set(0f - left, 1f);
				migrateIndividualPlayerText.Left.Set(left, 0f);
				migrateIndividualPlayerText.Height.Set(0f, 1f);
				migrateIndividualPlayerText.OnLeftClick += delegate
				{
					if (!_currentlyMigratingFiles)
					{
						_currentlyMigratingFiles = true;
						migrateIndividualPlayerText.SetText(Language.GetText("tModLoader.MigratingWorldsText"));
						Task.Factory.StartNew(delegate
						{
							ExecuteIndividualPlayerMigration(fileData, otherSaveFolderPath);
						}, TaskCreationOptions.PreferFairness);
					}
				};
				migrateIndividualPlayerPanel.Append(migrateIndividualPlayerText);
				migratePlayerList.Add(migrateIndividualPlayerPanel);
			}
		}
	}

	private static void ExecuteIndividualPlayerMigration(PlayerFileData fileData, string otherSaveFolderPath)
	{
		try
		{
			string playerFileName = Path.GetFileNameWithoutExtension(fileData.Path);
			foreach (string item in from s in Directory.GetFiles(Main.PlayerPath, playerFileName + ".*")
				where s.EndsWith(".plr") || s.EndsWith(".tplr") || s.EndsWith(".bak")
				select s)
			{
				File.Delete(item);
			}
			string existingPlayerMapPath = Path.Combine(Main.PlayerPath, Path.GetFileNameWithoutExtension(fileData.Path));
			if (Directory.Exists(existingPlayerMapPath))
			{
				Directory.Delete(existingPlayerMapPath, recursive: true);
			}
			foreach (string otherPlayerFile in from s in Directory.GetFiles(otherSaveFolderPath, playerFileName + ".*")
				where s.EndsWith(".plr") || s.EndsWith(".tplr") || s.EndsWith(".bak")
				select s)
			{
				File.Copy(otherPlayerFile, Path.Combine(Main.PlayerPath, Path.GetFileName(otherPlayerFile)), overwrite: true);
			}
			string playerMapPath = Path.Combine(otherSaveFolderPath, Path.GetFileNameWithoutExtension(fileData.Path));
			if (Directory.Exists(playerMapPath))
			{
				FileUtilities.CopyFolder(playerMapPath, existingPlayerMapPath);
			}
		}
		catch (Exception e)
		{
			Logging.tML.Error((object)Language.GetText("tModLoader.MigratePlayersException"), e);
		}
		_currentlyMigratingFiles = false;
		Main.menuMode = 1;
	}

	private void AddAutomaticPlayerMigrationButtons()
	{
		string vanillaPlayersPath = Path.Combine(Platform.Get<IPathService>().GetStoragePath("Terraria"), "Players");
		if (!Directory.Exists(vanillaPlayersPath) || !Directory.GetFiles(vanillaPlayersPath, "*.plr").Any())
		{
			return;
		}
		UIPanel autoMigrateButton = new UIPanel();
		autoMigrateButton.Width.Set(0f, 1f);
		autoMigrateButton.Height.Set(50f, 0f);
		UIText migrateText = new UIText((!_currentlyMigratingFiles) ? Language.GetText("tModLoader.MigratePlayersText") : Language.GetText("tModLoader.MigratingWorldsText"));
		autoMigrateButton.OnLeftClick += delegate
		{
			if (!_currentlyMigratingFiles)
			{
				_currentlyMigratingFiles = true;
				migrateText.SetText(Language.GetText("tModLoader.MigratingWorldsText"));
				Task.Factory.StartNew(delegate
				{
					ExecuteAutomaticPlayerMigration(vanillaPlayersPath);
				}, TaskCreationOptions.PreferFairness);
			}
		};
		autoMigrateButton.Append(migrateText);
		_playerList.Add(autoMigrateButton);
		UIText noPlayersMessage = new UIText(Language.GetTextValue("tModLoader.MigratePlayersMessage", Program.SaveFolderName));
		noPlayersMessage.Width.Set(0f, 1f);
		noPlayersMessage.Height.Set(300f, 0f);
		noPlayersMessage.MarginTop = 20f;
		noPlayersMessage.OnLeftClick += delegate
		{
			Utils.OpenFolder(Main.PlayerPath);
			Utils.OpenFolder(vanillaPlayersPath);
		};
		_playerList.Add(noPlayersMessage);
	}

	private static void ExecuteAutomaticPlayerMigration(string vanillaPlayersPath)
	{
		foreach (string file in from s in Directory.GetFiles(vanillaPlayersPath, "*.*")
			where s.EndsWith(".plr") || s.EndsWith(".tplr") || s.EndsWith(".bak")
			select s)
		{
			File.Copy(file, Path.Combine(Main.PlayerPath, Path.GetFileName(file)), overwrite: true);
		}
		string[] directories = Directory.GetDirectories(vanillaPlayersPath);
		for (int i = 0; i < directories.Length; i++)
		{
			IEnumerable<string> mapFiles = from s in Directory.GetFiles(directories[i], "*.*")
				where s.EndsWith(".map") || s.EndsWith(".tmap")
				select s;
			try
			{
				foreach (string mapFile in mapFiles)
				{
					string mapFileDir = Path.Combine(Main.PlayerPath, Directory.GetParent(mapFile).Name);
					Directory.CreateDirectory(mapFileDir);
					File.Copy(mapFile, Path.Combine(mapFileDir, Path.GetFileName(mapFile)), overwrite: true);
				}
			}
			catch (Exception e)
			{
				Logging.tML.Error((object)Language.GetText("tModLoader.MigratePlayersException"), e);
			}
		}
		_currentlyMigratingFiles = false;
		Main.menuMode = 1;
	}
}
