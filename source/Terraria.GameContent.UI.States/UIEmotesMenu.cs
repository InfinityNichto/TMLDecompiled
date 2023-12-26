using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.GameContent.Events;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.UI.Gamepad;

namespace Terraria.GameContent.UI.States;

public class UIEmotesMenu : UIState
{
	private UIElement _outerContainer;

	private UIElement _backPanel;

	private UIElement _container;

	private UIList _list;

	private UIScrollbar _scrollBar;

	private bool _isScrollbarAttached;

	private int _totalGroups = 7;

	public override void OnActivate()
	{
		InitializePage();
		if (Main.gameMenu)
		{
			_outerContainer.Top.Set(220f, 0f);
			_outerContainer.Height.Set(-220f, 1f);
		}
		else
		{
			_outerContainer.Top.Set(120f, 0f);
			_outerContainer.Height.Set(-120f, 1f);
		}
	}

	public void InitializePage()
	{
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		RemoveAllChildren();
		UIElement uIElement = new UIElement();
		uIElement.Width.Set(590f, 0f);
		uIElement.Top.Set(220f, 0f);
		uIElement.Height.Set(-220f, 1f);
		uIElement.HAlign = 0.5f;
		_outerContainer = uIElement;
		Append(uIElement);
		UIPanel uIPanel = new UIPanel();
		uIPanel.Width.Set(0f, 1f);
		uIPanel.Height.Set(-110f, 1f);
		uIPanel.BackgroundColor = new Color(33, 43, 79) * 0.8f;
		uIPanel.PaddingTop = 0f;
		uIElement.Append(uIPanel);
		_container = uIPanel;
		UIList uIList = new UIList();
		uIList.Width.Set(-25f, 1f);
		uIList.Height.Set(-50f, 1f);
		uIList.Top.Set(50f, 0f);
		uIList.HAlign = 0.5f;
		uIList.ListPadding = 14f;
		uIPanel.Append(uIList);
		_list = uIList;
		UIScrollbar uIScrollbar = new UIScrollbar();
		uIScrollbar.SetView(100f, 1000f);
		uIScrollbar.Height.Set(-20f, 1f);
		uIScrollbar.HAlign = 1f;
		uIScrollbar.VAlign = 1f;
		uIScrollbar.Top = StyleDimension.FromPixels(-5f);
		uIList.SetScrollbar(uIScrollbar);
		_scrollBar = uIScrollbar;
		UITextPanel<LocalizedText> uITextPanel = new UITextPanel<LocalizedText>(Language.GetText("UI.Back"), 0.7f, large: true);
		uITextPanel.Width.Set(-10f, 0.5f);
		uITextPanel.Height.Set(50f, 0f);
		uITextPanel.VAlign = 1f;
		uITextPanel.HAlign = 0.5f;
		uITextPanel.Top.Set(-45f, 0f);
		uITextPanel.OnMouseOver += FadedMouseOver;
		uITextPanel.OnMouseOut += FadedMouseOut;
		uITextPanel.OnLeftClick += GoBackClick;
		uITextPanel.SetSnapPoint("Back", 0);
		uIElement.Append(uITextPanel);
		_backPanel = uITextPanel;
		int currentGroupIndex = 0;
		TryAddingList(Language.GetText("UI.EmoteCategoryGeneral"), ref currentGroupIndex, 10, GetEmotesGeneral());
		TryAddingList(Language.GetText("UI.EmoteCategoryRPS"), ref currentGroupIndex, 10, GetEmotesRPS());
		TryAddingList(Language.GetText("UI.EmoteCategoryItems"), ref currentGroupIndex, 11, GetEmotesItems());
		TryAddingList(Language.GetText("UI.EmoteCategoryBiomesAndEvents"), ref currentGroupIndex, 8, GetEmotesBiomesAndEvents());
		TryAddingList(Language.GetText("UI.EmoteCategoryTownNPCs"), ref currentGroupIndex, 9, GetEmotesTownNPCs());
		TryAddingList(Language.GetText("UI.EmoteCategoryCritters"), ref currentGroupIndex, 7, GetEmotesCritters());
		TryAddingList(Language.GetText("UI.EmoteCategoryBosses"), ref currentGroupIndex, 8, GetEmotesBosses());
		foreach (var (mod, emotes) in EmoteBubbleLoader.GetAllUnlockedModEmotes())
		{
			TryAddingList(new LocalizedText(mod.Name, mod.DisplayName), ref currentGroupIndex, -1, emotes);
		}
		_totalGroups = currentGroupIndex;
	}

	private void TryAddingList(LocalizedText title, ref int currentGroupIndex, int maxEmotesPerRow, List<int> emoteIds)
	{
		if (emoteIds != null && emoteIds.Count != 0)
		{
			_list.Add(new EmotesGroupListItem(title, currentGroupIndex++, maxEmotesPerRow, emoteIds.ToArray()));
		}
	}

	public static List<int> GetEmotesGeneral()
	{
		return new List<int>
		{
			0, 1, 2, 3, 15, 136, 94, 16, 135, 134,
			137, 138, 139, 17, 87, 88, 89, 91, 92, 93,
			8, 9, 10, 11, 14, 100, 146, 147, 148
		}.AddEmotesToCategory(0);
	}

	public static List<int> GetEmotesRPS()
	{
		return new List<int> { 36, 37, 38, 33, 34, 35 }.AddEmotesToCategory(1);
	}

	public static List<int> GetEmotesItems()
	{
		return new List<int>
		{
			7, 73, 74, 75, 76, 131, 77, 78, 79, 80,
			81, 82, 83, 84, 85, 86, 90, 132, 126, 127,
			128, 129, 149
		}.AddEmotesToCategory(2);
	}

	public static List<int> GetEmotesBiomesAndEvents()
	{
		return new List<int>
		{
			22, 23, 24, 25, 26, 27, 28, 29, 30, 31,
			32, 18, 19, 20, 21, 99, 4, 5, 6, 95,
			96, 97, 98
		}.AddEmotesToCategory(3);
	}

	public static List<int> GetEmotesTownNPCs()
	{
		return new List<int>
		{
			101, 102, 103, 104, 105, 106, 107, 108, 109, 110,
			111, 112, 113, 114, 115, 116, 117, 118, 119, 120,
			121, 122, 123, 124, 125, 130, 140, 141, 142, 145
		}.AddEmotesToCategory(4);
	}

	public static List<int> GetEmotesCritters()
	{
		List<int> list = new List<int>();
		list.AddRange(new int[5] { 12, 13, 61, 62, 63 });
		list.AddRange(new int[4] { 67, 68, 69, 70 });
		list.Add(72);
		if (NPC.downedGoblins)
		{
			list.Add(64);
		}
		if (NPC.downedFrost)
		{
			list.Add(66);
		}
		if (NPC.downedPirates)
		{
			list.Add(65);
		}
		if (NPC.downedMartians)
		{
			list.Add(71);
		}
		list.AddEmotesToCategory(5);
		return list;
	}

	public static List<int> GetEmotesBosses()
	{
		List<int> list = new List<int>();
		if (NPC.downedBoss1)
		{
			list.Add(39);
		}
		if (NPC.downedBoss2)
		{
			list.Add(40);
			list.Add(41);
		}
		if (NPC.downedSlimeKing)
		{
			list.Add(51);
		}
		if (NPC.downedDeerclops)
		{
			list.Add(150);
		}
		if (NPC.downedQueenBee)
		{
			list.Add(42);
		}
		if (NPC.downedBoss3)
		{
			list.Add(43);
		}
		if (Main.hardMode)
		{
			list.Add(44);
		}
		if (NPC.downedQueenSlime)
		{
			list.Add(144);
		}
		if (NPC.downedMechBoss1)
		{
			list.Add(45);
		}
		if (NPC.downedMechBoss3)
		{
			list.Add(46);
		}
		if (NPC.downedMechBoss2)
		{
			list.Add(47);
		}
		if (NPC.downedPlantBoss)
		{
			list.Add(48);
		}
		if (NPC.downedGolemBoss)
		{
			list.Add(49);
		}
		if (NPC.downedFishron)
		{
			list.Add(50);
		}
		if (NPC.downedEmpressOfLight)
		{
			list.Add(143);
		}
		if (NPC.downedAncientCultist)
		{
			list.Add(52);
		}
		if (NPC.downedMoonlord)
		{
			list.Add(53);
		}
		if (NPC.downedHalloweenTree)
		{
			list.Add(54);
		}
		if (NPC.downedHalloweenKing)
		{
			list.Add(55);
		}
		if (NPC.downedChristmasTree)
		{
			list.Add(56);
		}
		if (NPC.downedChristmasIceQueen)
		{
			list.Add(57);
		}
		if (NPC.downedChristmasSantank)
		{
			list.Add(58);
		}
		if (NPC.downedPirates)
		{
			list.Add(59);
		}
		if (NPC.downedMartians)
		{
			list.Add(60);
		}
		if (DD2Event.DownedInvasionAnyDifficulty)
		{
			list.Add(133);
		}
		list.AddEmotesToCategory(6);
		return list;
	}

	public override void Recalculate()
	{
		if (_scrollBar != null)
		{
			if (_isScrollbarAttached && !_scrollBar.CanScroll)
			{
				_container.RemoveChild(_scrollBar);
				_isScrollbarAttached = false;
				_list.Width.Set(0f, 1f);
			}
			else if (!_isScrollbarAttached && _scrollBar.CanScroll)
			{
				_container.Append(_scrollBar);
				_isScrollbarAttached = true;
				_list.Width.Set(-25f, 1f);
			}
		}
		base.Recalculate();
	}

	private void GoBackClick(UIMouseEvent evt, UIElement listeningElement)
	{
		Main.menuMode = 0;
		IngameFancyUI.Close();
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
		((UIPanel)evt.Target).BackgroundColor = new Color(63, 82, 151) * 0.8f;
		((UIPanel)evt.Target).BorderColor = Color.Black;
	}

	public override void Draw(SpriteBatch spriteBatch)
	{
		base.Draw(spriteBatch);
		SetupGamepadPoints2(spriteBatch);
	}

	private void SetupGamepadPoints2(SpriteBatch spriteBatch)
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_017b: Unknown result type (might be due to invalid IL or missing references)
		int num = _totalGroups;
		UILinkPointNavigator.Shortcuts.BackButtonCommand = 1;
		int num5;
		int iD = (num5 = 3001);
		List<SnapPoint> snapPoints = GetSnapPoints();
		RemoveSnapPointsOutOfScreen(spriteBatch, snapPoints);
		Rectangle val = _backPanel.GetInnerDimensions().ToRectangle();
		UILinkPointNavigator.SetPosition(iD, ((Rectangle)(ref val)).Center.ToVector2());
		UILinkPoint uILinkPoint4 = UILinkPointNavigator.Points[num5];
		uILinkPoint4.Unlink();
		uILinkPoint4.Up = num5 + 1;
		UILinkPoint uILinkPoint2 = uILinkPoint4;
		num5++;
		int num6 = 0;
		List<List<SnapPoint>> list = new List<List<SnapPoint>>();
		for (int i = 0; i < num; i++)
		{
			List<SnapPoint> emoteGroup = GetEmoteGroup(snapPoints, i);
			if (emoteGroup.Count > 0)
			{
				list.Add(emoteGroup);
			}
			num6 += (int)Math.Ceiling((float)emoteGroup.Count / 14f);
		}
		SnapPoint[,] array = new SnapPoint[14, num6];
		int num7 = 0;
		for (int j = 0; j < list.Count; j++)
		{
			List<SnapPoint> list2 = list[j];
			for (int k = 0; k < list2.Count; k++)
			{
				int num8 = num7 + k / 14;
				int num9 = k % 14;
				array[num9, num8] = list2[k];
			}
			num7 += (int)Math.Ceiling((float)list2.Count / 14f);
		}
		int[,] array2 = new int[14, num6];
		int up = 0;
		for (int l = 0; l < array.GetLength(1); l++)
		{
			for (int m = 0; m < array.GetLength(0); m++)
			{
				SnapPoint snapPoint = array[m, l];
				if (snapPoint != null)
				{
					UILinkPointNavigator.Points[num5].Unlink();
					UILinkPointNavigator.SetPosition(num5, snapPoint.Position);
					array2[m, l] = num5;
					if (m == 0)
					{
						up = num5;
					}
					num5++;
				}
			}
		}
		uILinkPoint2.Up = up;
		for (int n = 0; n < array.GetLength(1); n++)
		{
			for (int num10 = 0; num10 < array.GetLength(0); num10++)
			{
				int num11 = array2[num10, n];
				if (num11 == 0)
				{
					continue;
				}
				UILinkPoint uILinkPoint3 = UILinkPointNavigator.Points[num11];
				if (TryGetPointOnGrid(array2, num10, n, -1, 0))
				{
					uILinkPoint3.Left = array2[num10 - 1, n];
				}
				else
				{
					uILinkPoint3.Left = uILinkPoint3.ID;
					for (int num12 = num10; num12 < array.GetLength(0); num12++)
					{
						if (TryGetPointOnGrid(array2, num12, n, 0, 0))
						{
							uILinkPoint3.Left = array2[num12, n];
						}
					}
				}
				if (TryGetPointOnGrid(array2, num10, n, 1, 0))
				{
					uILinkPoint3.Right = array2[num10 + 1, n];
				}
				else
				{
					uILinkPoint3.Right = uILinkPoint3.ID;
					for (int num2 = num10; num2 >= 0; num2--)
					{
						if (TryGetPointOnGrid(array2, num2, n, 0, 0))
						{
							uILinkPoint3.Right = array2[num2, n];
						}
					}
				}
				if (TryGetPointOnGrid(array2, num10, n, 0, -1))
				{
					uILinkPoint3.Up = array2[num10, n - 1];
				}
				else
				{
					uILinkPoint3.Up = uILinkPoint3.ID;
					for (int num3 = n - 1; num3 >= 0; num3--)
					{
						if (TryGetPointOnGrid(array2, num10, num3, 0, 0))
						{
							uILinkPoint3.Up = array2[num10, num3];
							break;
						}
					}
				}
				if (TryGetPointOnGrid(array2, num10, n, 0, 1))
				{
					uILinkPoint3.Down = array2[num10, n + 1];
					continue;
				}
				uILinkPoint3.Down = uILinkPoint3.ID;
				for (int num4 = n + 1; num4 < array.GetLength(1); num4++)
				{
					if (TryGetPointOnGrid(array2, num10, num4, 0, 0))
					{
						uILinkPoint3.Down = array2[num10, num4];
						break;
					}
				}
				if (uILinkPoint3.Down == uILinkPoint3.ID)
				{
					uILinkPoint3.Down = uILinkPoint2.ID;
				}
			}
		}
	}

	private bool TryGetPointOnGrid(int[,] grid, int x, int y, int offsetX, int offsetY)
	{
		if (x + offsetX < 0)
		{
			return false;
		}
		if (x + offsetX >= grid.GetLength(0))
		{
			return false;
		}
		if (y + offsetY < 0)
		{
			return false;
		}
		if (y + offsetY >= grid.GetLength(1))
		{
			return false;
		}
		if (grid[x + offsetX, y + offsetY] == 0)
		{
			return false;
		}
		return true;
	}

	private void RemoveSnapPointsOutOfScreen(SpriteBatch spriteBatch, List<SnapPoint> pts)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		float num = 1f / Main.UIScale;
		Rectangle clippingRectangle = _container.GetClippingRectangle(spriteBatch);
		Vector2 minimum = clippingRectangle.TopLeft() * num;
		Vector2 maximum = clippingRectangle.BottomRight() * num;
		for (int i = 0; i < pts.Count; i++)
		{
			if (!pts[i].Position.Between(minimum, maximum))
			{
				pts.Remove(pts[i]);
				i--;
			}
		}
	}

	private void SetupGamepadPoints(SpriteBatch spriteBatch)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		UILinkPointNavigator.Shortcuts.BackButtonCommand = 1;
		int num = 3001;
		int num4 = num;
		List<SnapPoint> snapPoints = GetSnapPoints();
		Rectangle val = _backPanel.GetInnerDimensions().ToRectangle();
		UILinkPointNavigator.SetPosition(num, ((Rectangle)(ref val)).Center.ToVector2());
		UILinkPoint uILinkPoint = UILinkPointNavigator.Points[num4];
		uILinkPoint.Unlink();
		uILinkPoint.Up = num4 + 1;
		UILinkPoint uILinkPoint2 = uILinkPoint;
		num4++;
		float num5 = 1f / Main.UIScale;
		Rectangle clippingRectangle = _container.GetClippingRectangle(spriteBatch);
		Vector2 minimum = clippingRectangle.TopLeft() * num5;
		Vector2 maximum = clippingRectangle.BottomRight() * num5;
		for (int i = 0; i < snapPoints.Count; i++)
		{
			if (!snapPoints[i].Position.Between(minimum, maximum))
			{
				snapPoints.Remove(snapPoints[i]);
				i--;
			}
		}
		int num6 = 0;
		int num7 = 0;
		int num8 = 7;
		List<List<SnapPoint>> list = new List<List<SnapPoint>>();
		for (int j = 0; j < num8; j++)
		{
			List<SnapPoint> emoteGroup = GetEmoteGroup(snapPoints, j);
			if (emoteGroup.Count > 0)
			{
				list.Add(emoteGroup);
			}
		}
		List<SnapPoint>[] array = list.ToArray();
		for (int k = 0; k < array.Length; k++)
		{
			List<SnapPoint> list2 = array[k];
			int num9 = list2.Count / 14;
			if (list2.Count % 14 > 0)
			{
				num9++;
			}
			int num10 = 14;
			if (list2.Count % 14 != 0)
			{
				num10 = list2.Count % 14;
			}
			for (int l = 0; l < list2.Count; l++)
			{
				uILinkPoint = UILinkPointNavigator.Points[num4];
				uILinkPoint.Unlink();
				UILinkPointNavigator.SetPosition(num4, list2[l].Position);
				int num11 = 14;
				if (l / 14 == num9 - 1 && list2.Count % 14 != 0)
				{
					num11 = list2.Count % 14;
				}
				int num2 = l % 14;
				uILinkPoint.Left = num4 - 1;
				uILinkPoint.Right = num4 + 1;
				uILinkPoint.Up = num4 - 14;
				uILinkPoint.Down = num4 + 14;
				if (num2 == num11 - 1)
				{
					uILinkPoint.Right = num4 - num11 + 1;
				}
				if (num2 == 0)
				{
					uILinkPoint.Left = num4 + num11 - 1;
				}
				if (num2 == 0)
				{
					uILinkPoint2.Up = num4;
				}
				if (l < 14)
				{
					if (num6 == 0)
					{
						uILinkPoint.Up = -1;
					}
					else
					{
						uILinkPoint.Up = num4 - 14;
						if (num2 >= num6)
						{
							uILinkPoint.Up -= 14;
						}
						int num3 = k - 1;
						while (num3 > 0 && array[num3].Count <= num2)
						{
							uILinkPoint.Up -= 14;
							num3--;
						}
					}
				}
				int down = num;
				if (k == array.Length - 1)
				{
					if (l / 14 < num9 - 1 && num2 >= list2.Count % 14)
					{
						uILinkPoint.Down = down;
					}
					if (l / 14 == num9 - 1)
					{
						uILinkPoint.Down = down;
					}
				}
				else if (l / 14 == num9 - 1)
				{
					uILinkPoint.Down = num4 + 14;
					for (int m = k + 1; m < array.Length && array[m].Count <= num2; m++)
					{
						uILinkPoint.Down += 14;
					}
					if (k == array.Length - 1)
					{
						uILinkPoint.Down = down;
					}
				}
				else if (num2 >= num10)
				{
					uILinkPoint.Down = num4 + 14 + 14;
					for (int n = k + 1; n < array.Length && array[n].Count <= num2; n++)
					{
						uILinkPoint.Down += 14;
					}
				}
				num4++;
			}
			num6 = num10;
			num7 = 14 - num6;
			num4 += num7;
		}
	}

	private List<SnapPoint> GetEmoteGroup(List<SnapPoint> ptsOnPage, int groupIndex)
	{
		string groupName = "Group " + groupIndex;
		List<SnapPoint> list = ptsOnPage.Where((SnapPoint a) => a.Name == groupName).ToList();
		list.Sort(SortPoints);
		return list;
	}

	private int SortPoints(SnapPoint a, SnapPoint b)
	{
		return a.Id.CompareTo(b.Id);
	}
}
