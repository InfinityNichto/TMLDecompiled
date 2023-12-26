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

public class UIWorldSelect : UIState
{
	private UIList _worldList;

	private UITextPanel<LocalizedText> _backPanel;

	private UITextPanel<LocalizedText> _newPanel;

	private UITextPanel<LocalizedText> _workshopPanel;

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
		uIElement.Append(uIPanel);
		_containerPanel = uIPanel;
		_worldList = new UIList();
		_worldList.Width.Set(0f, 1f);
		_worldList.Height.Set(0f, 1f);
		_worldList.ListPadding = 5f;
		uIPanel.Append(_worldList);
		_scrollbar = new UIScrollbar();
		_scrollbar.SetView(100f, 1000f);
		_scrollbar.Height.Set(0f, 1f);
		_scrollbar.HAlign = 1f;
		_worldList.SetScrollbar(_scrollbar);
		UITextPanel<LocalizedText> uITextPanel = new UITextPanel<LocalizedText>(Language.GetText("UI.SelectWorld"), 0.8f, large: true);
		uITextPanel.HAlign = 0.5f;
		uITextPanel.Top.Set(-40f, 0f);
		uITextPanel.SetPadding(15f);
		uITextPanel.BackgroundColor = new Color(73, 94, 171);
		uIElement.Append(uITextPanel);
		UITextPanel<LocalizedText> uITextPanel2 = new UITextPanel<LocalizedText>(Language.GetText("UI.Back"), 0.7f, large: true);
		uITextPanel2.Width.Set(-10f, 0.5f);
		uITextPanel2.Height.Set(50f, 0f);
		uITextPanel2.VAlign = 1f;
		uITextPanel2.HAlign = 0f;
		uITextPanel2.Top.Set(-45f, 0f);
		uITextPanel2.OnMouseOver += FadedMouseOver;
		uITextPanel2.OnMouseOut += FadedMouseOut;
		uITextPanel2.OnLeftClick += GoBackClick;
		uIElement.Append(uITextPanel2);
		_backPanel = uITextPanel2;
		UITextPanel<LocalizedText> uITextPanel3 = new UITextPanel<LocalizedText>(Language.GetText("UI.New"), 0.7f, large: true);
		uITextPanel3.CopyStyle(uITextPanel2);
		uITextPanel3.HAlign = 1f;
		uITextPanel3.OnMouseOver += FadedMouseOver;
		uITextPanel3.OnMouseOut += FadedMouseOut;
		uITextPanel3.OnLeftClick += NewWorldClick;
		uIElement.Append(uITextPanel3);
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
				_worldList.Width.Set(0f, 1f);
			}
			else if (!_isScrollbarAttached && _scrollbar.CanScroll)
			{
				_containerPanel.Append(_scrollbar);
				_isScrollbarAttached = true;
				_worldList.Width.Set(-25f, 1f);
			}
		}
		base.Recalculate();
	}

	private void NewWorldClick(UIMouseEvent evt, UIElement listeningElement)
	{
		SoundEngine.PlaySound(10);
		Main.newWorldName = Lang.gen[57].Value + " " + (Main.WorldList.Count + 1);
		Main.menuMode = 888;
		Main.MenuUI.SetState(new UIWorldCreation());
	}

	private void GoBackClick(UIMouseEvent evt, UIElement listeningElement)
	{
		SoundEngine.PlaySound(11);
		Main.menuMode = ((!Main.menuMultiplayer) ? 1 : 12);
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
		Main.LoadWorlds();
		LoadMigratableWorlds();
		UpdateWorldsList();
		if (PlayerInput.UsingGamepadUI)
		{
			UILinkPointNavigator.ChangePoint(3000 + ((_worldList.Count == 0) ? 1 : 2));
		}
	}

	private void UpdateWorldsList()
	{
		_worldList.Clear();
		IOrderedEnumerable<WorldFileData> orderedEnumerable = new List<WorldFileData>(Main.WorldList).OrderByDescending(CanWorldBePlayed).ThenByDescending((WorldFileData x) => x.IsFavorite).ThenBy((WorldFileData x) => x.Name)
			.ThenBy((WorldFileData x) => x.GetFileName());
		int num = 0;
		foreach (WorldFileData item in orderedEnumerable)
		{
			_worldList.Add(new UIWorldListItem(item, num++, CanWorldBePlayed(item)));
		}
		_worldList.Add(_migrationPanel);
		if (!orderedEnumerable.Any())
		{
			AddAutomaticWorldMigrationButtons();
		}
	}

	internal static bool CanWorldBePlayed(WorldFileData file)
	{
		bool num = Main.ActivePlayerFileData.Player.difficulty == 3;
		bool flag = file.GameMode == 3;
		ModSystem rejector;
		if (num == flag)
		{
			return SystemLoader.CanWorldBePlayed(Main.ActivePlayerFileData, file, out rejector);
		}
		return false;
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
		List<WorldFileData> list = new List<WorldFileData>(Main.WorldList);
		list.Sort(delegate(WorldFileData x, WorldFileData y)
		{
			if (x.IsFavorite && !y.IsFavorite)
			{
				return -1;
			}
			if (!x.IsFavorite && y.IsFavorite)
			{
				return 1;
			}
			if (x.Name == null)
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
			foreach (WorldFileData item in list)
			{
				favoritesCache.Add(Tuple.Create(item.Name, item.IsFavorite));
			}
			UpdateWorldsList();
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
		//IL_03de: Unknown result type (might be due to invalid IL or missing references)
		UILinkPointNavigator.Shortcuts.BackButtonCommand = 2;
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
		SnapPoint[,] array = new SnapPoint[_worldList.Count, 6];
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
		foreach (SnapPoint item4 in snapPoints.Where((SnapPoint a) => a.Name == "Seed"))
		{
			array[item4.Id, 3] = item4;
		}
		foreach (SnapPoint item5 in snapPoints.Where((SnapPoint a) => a.Name == "Rename"))
		{
			array[item5.Id, 4] = item5;
		}
		foreach (SnapPoint item6 in snapPoints.Where((SnapPoint a) => a.Name == "Delete"))
		{
			array[item6.Id, 5] = item6;
		}
		num2 = num + 2;
		int[] array2 = new int[_worldList.Count];
		for (int j = 0; j < array2.Length; j++)
		{
			array2[j] = -1;
		}
		for (int k = 0; k < array.GetLength(1); k++)
		{
			int num4 = -1;
			for (int l = 0; l < array.GetLength(0); l++)
			{
				if (array[l, k] != null)
				{
					uILinkPoint = UILinkPointNavigator.Points[num2];
					uILinkPoint.Unlink();
					UILinkPointNavigator.SetPosition(num2, array[l, k].Position);
					if (num4 != -1)
					{
						uILinkPoint.Up = num4;
						UILinkPointNavigator.Points[num4].Down = num2;
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
					num4 = num2;
					array2[l] = num2;
					UILinkPointNavigator.Shortcuts.FANCYUI_HIGHEST_INDEX = num2;
					num2++;
				}
			}
		}
		if (PlayerInput.UsingGamepadUI && _worldList.Count == 0 && UILinkPointNavigator.CurrentPoint > 3001)
		{
			UILinkPointNavigator.ChangePoint(3001);
		}
	}

	private void LoadMigratableWorlds()
	{
		_migrationPanel = new UIExpandablePanel();
		_worldList.Add(_migrationPanel);
		UIText playerMigrationPanelTitle = new UIText(Language.GetTextValue("tModLoader.MigrateIndividualWorldsHeader"));
		playerMigrationPanelTitle.Top.Set(4f, 0f);
		_migrationPanel.Append(playerMigrationPanelTitle);
		NestedUIList migrateWorldList = new NestedUIList();
		migrateWorldList.Width.Set(-22f, 1f);
		migrateWorldList.Left.Set(0f, 0f);
		migrateWorldList.Top.Set(30f, 0f);
		migrateWorldList.MinHeight.Set(300f, 0f);
		migrateWorldList.ListPadding = 5f;
		_migrationPanel.VisibleWhenExpanded.Add(migrateWorldList);
		UIScrollbar scrollbar = new UIScrollbar();
		scrollbar.SetView(100f, 1000f);
		scrollbar.Height.Set(-42f, 1f);
		scrollbar.Top.Set(36f, 0f);
		scrollbar.Left.Pixels -= 0f;
		scrollbar.HAlign = 1f;
		migrateWorldList.SetScrollbar(scrollbar);
		_migrationPanel.VisibleWhenExpanded.Add(scrollbar);
		(string, string, int)[] otherPaths = FileUtilities.GetAlternateSavePathFiles("Worlds");
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
			string[] files = Directory.GetFiles(otherSaveFolderPath, "*.wld");
			int num2 = Math.Min(1000, files.Length);
			for (int i = 0; i < num2; i++)
			{
				string worldInThisWorldsPath = Path.Combine(Main.WorldPath, Path.GetFileName(files[i]));
				if (File.Exists(worldInThisWorldsPath) && File.GetLastWriteTime(worldInThisWorldsPath) == File.GetLastWriteTime(files[i]))
				{
					continue;
				}
				WorldFileData fileData = WorldFile.GetAllMetadata(files[i], cloudSave: false);
				if (fileData == null)
				{
					continue;
				}
				UIPanel migrateIndividualWorldPanel = new UIPanel();
				migrateIndividualWorldPanel.Width.Set(0f, 1f);
				migrateIndividualWorldPanel.Height.Set(50f, 0f);
				float left = 0f;
				if (stabilityLevel > currentStabilityLevel)
				{
					UIHoverImage warningImage2 = new UIHoverImage(UICommon.ButtonErrorTexture, Language.GetTextValue("tModLoader.WorldFromNewerTModMightNotWork"))
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
					migrateIndividualWorldPanel.Append(warningImage2);
					left += warningImage2.Width.Pixels + 6f;
				}
				WorldFileData worldWithSameName = Main.WorldList.FirstOrDefault((WorldFileData x) => x.Name == fileData.Name);
				if (worldWithSameName != null)
				{
					UIHoverImage warningImage = new UIHoverImage(UICommon.ButtonExclamationTexture, Language.GetTextValue("tModLoader.WorldWithThisNameExistsWillBeOverwritten"))
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
					migrateIndividualWorldPanel.Append(warningImage);
					left += warningImage.Width.Pixels + 6f;
					if (File.GetLastWriteTime(worldWithSameName.Path) > File.GetLastWriteTime(files[i]))
					{
						warningImage = new UIHoverImage(UICommon.ButtonExclamationTexture, Language.GetTextValue("tModLoader.ExistingWorldPlayedMoreRecently"))
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
						migrateIndividualWorldPanel.Append(warningImage);
						left += warningImage.Width.Pixels + 6f;
					}
				}
				UIText migrateIndividualWorldText = new UIText(string.Format(message, fileData.Name));
				migrateIndividualWorldText.Width.Set(0f - left, 1f);
				migrateIndividualWorldText.Left.Set(left, 0f);
				migrateIndividualWorldText.Height.Set(0f, 1f);
				migrateIndividualWorldText.OnLeftClick += delegate
				{
					if (!_currentlyMigratingFiles)
					{
						_currentlyMigratingFiles = true;
						migrateIndividualWorldText.SetText(Language.GetText("tModLoader.MigratingWorldsText"));
						Task.Factory.StartNew(delegate
						{
							ExecuteIndividualWorldMigration(fileData, otherSaveFolderPath);
						}, TaskCreationOptions.PreferFairness);
					}
				};
				migrateIndividualWorldPanel.Append(migrateIndividualWorldText);
				migrateWorldList.Add(migrateIndividualWorldPanel);
			}
		}
	}

	private static void ExecuteIndividualWorldMigration(WorldFileData fileData, string otherSaveFolderPath)
	{
		try
		{
			string worldFileName = Path.GetFileNameWithoutExtension(fileData.Path);
			foreach (string item in from s in Directory.GetFiles(Main.WorldPath, worldFileName + ".*")
				where s.EndsWith(".wld") || s.EndsWith(".twld") || s.EndsWith(".bak")
				select s)
			{
				File.Delete(item);
			}
			foreach (string otherWorldFile in from s in Directory.GetFiles(otherSaveFolderPath, worldFileName + ".*")
				where s.EndsWith(".wld") || s.EndsWith(".twld") || s.EndsWith(".bak")
				select s)
			{
				File.Copy(otherWorldFile, Path.Combine(Main.WorldPath, Path.GetFileName(otherWorldFile)), overwrite: true);
			}
		}
		catch (Exception e)
		{
			Logging.tML.Error((object)Language.GetText("tModLoader.MigratePlayersException"), e);
		}
		_currentlyMigratingFiles = false;
		Main.menuMode = 6;
	}

	private void AddAutomaticWorldMigrationButtons()
	{
		string vanillaWorldsPath = Path.Combine(Platform.Get<IPathService>().GetStoragePath("Terraria"), "Worlds");
		if (!Directory.Exists(vanillaWorldsPath) || !Directory.GetFiles(vanillaWorldsPath, "*.wld").Any())
		{
			return;
		}
		UIPanel autoMigrateButton = new UIPanel();
		autoMigrateButton.Width.Set(0f, 1f);
		autoMigrateButton.Height.Set(50f, 0f);
		UIText migrateText = new UIText((!_currentlyMigratingFiles) ? Language.GetText("tModLoader.MigrateWorldsText") : Language.GetText("tModLoader.MigratingWorldsText"));
		autoMigrateButton.OnLeftClick += delegate
		{
			if (!_currentlyMigratingFiles)
			{
				_currentlyMigratingFiles = true;
				migrateText.SetText(Language.GetText("tModLoader.MigratingWorldsText"));
				Task.Factory.StartNew(delegate
				{
					ExecuteAutomaticWorldMigration(vanillaWorldsPath);
				}, TaskCreationOptions.PreferFairness);
			}
		};
		autoMigrateButton.Append(migrateText);
		_worldList.Add(autoMigrateButton);
		UIText noWorldsMessage = new UIText(Language.GetTextValue("tModLoader.MigrateWorldsMessage", Program.SaveFolderName));
		noWorldsMessage.Width.Set(0f, 1f);
		noWorldsMessage.Height.Set(300f, 0f);
		noWorldsMessage.MarginTop = 20f;
		noWorldsMessage.OnLeftClick += delegate
		{
			Utils.OpenFolder(Main.WorldPath);
			Utils.OpenFolder(vanillaWorldsPath);
		};
		_worldList.Add(noWorldsMessage);
	}

	private static void ExecuteAutomaticWorldMigration(string vanillaWorldsPath)
	{
		foreach (string file in from s in Directory.GetFiles(vanillaWorldsPath, "*.*")
			where s.EndsWith(".wld") || s.EndsWith(".twld") || s.EndsWith(".bak")
			select s)
		{
			File.Copy(file, Path.Combine(Main.WorldPath, Path.GetFileName(file)), overwrite: true);
		}
		_currentlyMigratingFiles = false;
		Main.menuMode = 6;
	}
}
