using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.Enums;
using Terraria.GameContent.UI;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;

namespace Terraria.GameContent;

public class SmartCursorHelper
{
	private class SmartCursorUsageInfo
	{
		public Player player;

		public Item item;

		public Vector2 mouse;

		public Vector2 position;

		public Vector2 Center;

		public int screenTargetX;

		public int screenTargetY;

		public int reachableStartX;

		public int reachableEndX;

		public int reachableStartY;

		public int reachableEndY;

		public int paintLookup;

		public int paintCoatingLookup;
	}

	private static List<Tuple<int, int>> _targets = new List<Tuple<int, int>>();

	private static List<Tuple<int, int>> _grappleTargets = new List<Tuple<int, int>>();

	private static List<Tuple<int, int>> _points = new List<Tuple<int, int>>();

	private static List<Tuple<int, int>> _endpoints = new List<Tuple<int, int>>();

	private static List<Tuple<int, int>> _toRemove = new List<Tuple<int, int>>();

	private static List<Tuple<int, int>> _targets2 = new List<Tuple<int, int>>();

	public static void SmartCursorLookup(Player player)
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0243: Unknown result type (might be due to invalid IL or missing references)
		//IL_0253: Unknown result type (might be due to invalid IL or missing references)
		Main.SmartCursorShowing = false;
		if (!Main.SmartCursorIsUsed)
		{
			return;
		}
		SmartCursorUsageInfo smartCursorUsageInfo = new SmartCursorUsageInfo
		{
			player = player,
			item = player.inventory[player.selectedItem],
			mouse = Main.MouseWorld,
			position = player.position,
			Center = player.Center
		};
		_ = player.gravDir;
		int tileTargetX = Player.tileTargetX;
		int tileTargetY = Player.tileTargetY;
		int tileRangeX = Player.tileRangeX;
		int tileRangeY = Player.tileRangeY;
		smartCursorUsageInfo.screenTargetX = Utils.Clamp(tileTargetX, 10, Main.maxTilesX - 10);
		smartCursorUsageInfo.screenTargetY = Utils.Clamp(tileTargetY, 10, Main.maxTilesY - 10);
		if (Main.tile[smartCursorUsageInfo.screenTargetX, smartCursorUsageInfo.screenTargetY] == null)
		{
			return;
		}
		bool num = IsHoveringOverAnInteractibleTileThatBlocksSmartCursor(smartCursorUsageInfo);
		TryFindingPaintInplayerInventory(smartCursorUsageInfo, out smartCursorUsageInfo.paintLookup, out smartCursorUsageInfo.paintCoatingLookup);
		int tileBoost = smartCursorUsageInfo.item.tileBoost;
		smartCursorUsageInfo.reachableStartX = (int)(player.position.X / 16f) - tileRangeX - tileBoost + 1;
		smartCursorUsageInfo.reachableEndX = (int)((player.position.X + (float)player.width) / 16f) + tileRangeX + tileBoost - 1;
		smartCursorUsageInfo.reachableStartY = (int)(player.position.Y / 16f) - tileRangeY - tileBoost + 1;
		smartCursorUsageInfo.reachableEndY = (int)((player.position.Y + (float)player.height) / 16f) + tileRangeY + tileBoost - 2;
		smartCursorUsageInfo.reachableStartX = Utils.Clamp(smartCursorUsageInfo.reachableStartX, 10, Main.maxTilesX - 10);
		smartCursorUsageInfo.reachableEndX = Utils.Clamp(smartCursorUsageInfo.reachableEndX, 10, Main.maxTilesX - 10);
		smartCursorUsageInfo.reachableStartY = Utils.Clamp(smartCursorUsageInfo.reachableStartY, 10, Main.maxTilesY - 10);
		smartCursorUsageInfo.reachableEndY = Utils.Clamp(smartCursorUsageInfo.reachableEndY, 10, Main.maxTilesY - 10);
		if (!num || smartCursorUsageInfo.screenTargetX < smartCursorUsageInfo.reachableStartX || smartCursorUsageInfo.screenTargetX > smartCursorUsageInfo.reachableEndX || smartCursorUsageInfo.screenTargetY < smartCursorUsageInfo.reachableStartY || smartCursorUsageInfo.screenTargetY > smartCursorUsageInfo.reachableEndY)
		{
			_grappleTargets.Clear();
			int[] grappling = player.grappling;
			int grapCount = player.grapCount;
			for (int i = 0; i < grapCount; i++)
			{
				Projectile obj = Main.projectile[grappling[i]];
				int item = (int)obj.Center.X / 16;
				int item2 = (int)obj.Center.Y / 16;
				_grappleTargets.Add(new Tuple<int, int>(item, item2));
			}
			int fX = -1;
			int fY = -1;
			if (!Player.SmartCursorSettings.SmartAxeAfterPickaxe)
			{
				Step_Axe(smartCursorUsageInfo, ref fX, ref fY);
			}
			Step_ForceCursorToAnyMinableThing(smartCursorUsageInfo, ref fX, ref fY);
			Step_Pickaxe_MineShinies(smartCursorUsageInfo, ref fX, ref fY);
			Step_Pickaxe_MineSolids(player, smartCursorUsageInfo, _grappleTargets, ref fX, ref fY);
			if (Player.SmartCursorSettings.SmartAxeAfterPickaxe)
			{
				Step_Axe(smartCursorUsageInfo, ref fX, ref fY);
			}
			Step_ColoredWrenches(smartCursorUsageInfo, ref fX, ref fY);
			Step_MulticolorWrench(smartCursorUsageInfo, ref fX, ref fY);
			Step_Hammers(smartCursorUsageInfo, ref fX, ref fY);
			Step_ActuationRod(smartCursorUsageInfo, ref fX, ref fY);
			Step_WireCutter(smartCursorUsageInfo, ref fX, ref fY);
			Step_Platforms(smartCursorUsageInfo, ref fX, ref fY);
			Step_MinecartTracks(smartCursorUsageInfo, ref fX, ref fY);
			Step_Walls(smartCursorUsageInfo, ref fX, ref fY);
			Step_PumpkinSeeds(smartCursorUsageInfo, ref fX, ref fY);
			Step_GrassSeeds(smartCursorUsageInfo, ref fX, ref fY);
			Step_Pigronata(smartCursorUsageInfo, ref fX, ref fY);
			Step_Boulders(smartCursorUsageInfo, ref fX, ref fY);
			Step_Torch(smartCursorUsageInfo, ref fX, ref fY);
			Step_LawnMower(smartCursorUsageInfo, ref fX, ref fY);
			Step_BlocksFilling(smartCursorUsageInfo, ref fX, ref fY);
			Step_BlocksLines(smartCursorUsageInfo, ref fX, ref fY);
			Step_PaintRoller(smartCursorUsageInfo, ref fX, ref fY);
			Step_PaintBrush(smartCursorUsageInfo, ref fX, ref fY);
			Step_PaintScrapper(smartCursorUsageInfo, ref fX, ref fY);
			Step_Acorns(smartCursorUsageInfo, ref fX, ref fY);
			Step_GemCorns(smartCursorUsageInfo, ref fX, ref fY);
			Step_EmptyBuckets(smartCursorUsageInfo, ref fX, ref fY);
			Step_Actuators(smartCursorUsageInfo, ref fX, ref fY);
			Step_AlchemySeeds(smartCursorUsageInfo, ref fX, ref fY);
			Step_PlanterBox(smartCursorUsageInfo, ref fX, ref fY);
			Step_ClayPots(smartCursorUsageInfo, ref fX, ref fY);
			Step_StaffOfRegrowth(smartCursorUsageInfo, ref fX, ref fY);
			if (fX != -1 && fY != -1)
			{
				Main.SmartCursorX = (Player.tileTargetX = fX);
				Main.SmartCursorY = (Player.tileTargetY = fY);
				Main.SmartCursorShowing = true;
			}
			_grappleTargets.Clear();
		}
	}

	private static void TryFindingPaintInplayerInventory(SmartCursorUsageInfo providedInfo, out int paintLookup, out int coatingLookup)
	{
		_ = providedInfo.player.inventory;
		paintLookup = 0;
		coatingLookup = 0;
		if (providedInfo.item.type == 1071 || providedInfo.item.type == 1543 || providedInfo.item.type == 1072 || providedInfo.item.type == 1544)
		{
			Item item = providedInfo.player.FindPaintOrCoating();
			if (item != null)
			{
				coatingLookup = item.paintCoating;
				paintLookup = item.paint;
			}
		}
	}

	private static bool IsHoveringOverAnInteractibleTileThatBlocksSmartCursor(SmartCursorUsageInfo providedInfo)
	{
		bool result = false;
		if (Main.tile[providedInfo.screenTargetX, providedInfo.screenTargetY].active())
		{
			if (Main.tile[providedInfo.screenTargetX, providedInfo.screenTargetY].type != 314)
			{
				result = TileID.Sets.DisableSmartCursor[Main.tile[providedInfo.screenTargetX, providedInfo.screenTargetY].type];
			}
			else if (providedInfo.player.gravDir == 1f)
			{
				result = true;
			}
		}
		return result;
	}

	private static void Step_StaffOfRegrowth(SmartCursorUsageInfo providedInfo, ref int focusedX, ref int focusedY)
	{
		//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0204: Unknown result type (might be due to invalid IL or missing references)
		//IL_020a: Unknown result type (might be due to invalid IL or missing references)
		if ((providedInfo.item.type != 213 && providedInfo.item.type != 5295) || focusedX != -1 || focusedY != -1)
		{
			return;
		}
		_targets.Clear();
		for (int i = providedInfo.reachableStartX; i <= providedInfo.reachableEndX; i++)
		{
			for (int j = providedInfo.reachableStartY; j <= providedInfo.reachableEndY; j++)
			{
				Tile tile = Main.tile[i, j];
				bool flag = !Main.tile[i - 1, j].active() || !Main.tile[i, j + 1].active() || !Main.tile[i + 1, j].active() || !Main.tile[i, j - 1].active();
				bool flag2 = !Main.tile[i - 1, j - 1].active() || !Main.tile[i - 1, j + 1].active() || !Main.tile[i + 1, j + 1].active() || !Main.tile[i + 1, j - 1].active();
				if (tile.active() && !tile.inActive() && tile.type == 0 && (flag || (tile.type == 0 && flag2)))
				{
					_targets.Add(new Tuple<int, int>(i, j));
				}
			}
		}
		if (_targets.Count > 0)
		{
			float num = -1f;
			Tuple<int, int> tuple = _targets[0];
			for (int k = 0; k < _targets.Count; k++)
			{
				float num2 = Vector2.Distance(new Vector2((float)_targets[k].Item1, (float)_targets[k].Item2) * 16f + Vector2.One * 8f, providedInfo.mouse);
				if (num == -1f || num2 < num)
				{
					num = num2;
					tuple = _targets[k];
				}
			}
			if (Collision.InTileBounds(tuple.Item1, tuple.Item2, providedInfo.reachableStartX, providedInfo.reachableStartY, providedInfo.reachableEndX, providedInfo.reachableEndY))
			{
				focusedX = tuple.Item1;
				focusedY = tuple.Item2;
			}
		}
		_targets.Clear();
	}

	private static void Step_GrassSeeds(SmartCursorUsageInfo providedInfo, ref int focusedX, ref int focusedY)
	{
		//IL_024a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0254: Unknown result type (might be due to invalid IL or missing references)
		//IL_0259: Unknown result type (might be due to invalid IL or missing references)
		//IL_0263: Unknown result type (might be due to invalid IL or missing references)
		//IL_0268: Unknown result type (might be due to invalid IL or missing references)
		//IL_026e: Unknown result type (might be due to invalid IL or missing references)
		if (focusedX > -1 || focusedY > -1)
		{
			return;
		}
		int type = providedInfo.item.type;
		if (type < 0 || !ItemID.Sets.GrassSeeds[type])
		{
			return;
		}
		_targets.Clear();
		for (int i = providedInfo.reachableStartX; i <= providedInfo.reachableEndX; i++)
		{
			for (int j = providedInfo.reachableStartY; j <= providedInfo.reachableEndY; j++)
			{
				Tile tile = Main.tile[i, j];
				bool flag = !Main.tile[i - 1, j].active() || !Main.tile[i, j + 1].active() || !Main.tile[i + 1, j].active() || !Main.tile[i, j - 1].active();
				bool flag2 = !Main.tile[i - 1, j - 1].active() || !Main.tile[i - 1, j + 1].active() || !Main.tile[i + 1, j + 1].active() || !Main.tile[i + 1, j - 1].active();
				if (tile.active() && !tile.inActive() && (flag || flag2))
				{
					bool flag3 = false;
					switch (type)
					{
					default:
						flag3 = tile.type == 0;
						break;
					case 59:
					case 2171:
						flag3 = tile.type == 0 || tile.type == 59;
						break;
					case 194:
					case 195:
						flag3 = tile.type == 59;
						break;
					case 5214:
						flag3 = tile.type == 57;
						break;
					}
					if (flag3)
					{
						_targets.Add(new Tuple<int, int>(i, j));
					}
				}
			}
		}
		if (_targets.Count > 0)
		{
			float num = -1f;
			Tuple<int, int> tuple = _targets[0];
			for (int k = 0; k < _targets.Count; k++)
			{
				float num2 = Vector2.Distance(new Vector2((float)_targets[k].Item1, (float)_targets[k].Item2) * 16f + Vector2.One * 8f, providedInfo.mouse);
				if (num == -1f || num2 < num)
				{
					num = num2;
					tuple = _targets[k];
				}
			}
			if (Collision.InTileBounds(tuple.Item1, tuple.Item2, providedInfo.reachableStartX, providedInfo.reachableStartY, providedInfo.reachableEndX, providedInfo.reachableEndY))
			{
				focusedX = tuple.Item1;
				focusedY = tuple.Item2;
			}
		}
		_targets.Clear();
	}

	private static void Step_ClayPots(SmartCursorUsageInfo providedInfo, ref int focusedX, ref int focusedY)
	{
		//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
		if (providedInfo.item.createTile != 78 || focusedX != -1 || focusedY != -1)
		{
			return;
		}
		_targets.Clear();
		bool flag = false;
		if (Main.tile[providedInfo.screenTargetX, providedInfo.screenTargetY].active())
		{
			flag = true;
		}
		if (!Collision.InTileBounds(providedInfo.screenTargetX, providedInfo.screenTargetY, providedInfo.reachableStartX, providedInfo.reachableStartY, providedInfo.reachableEndX, providedInfo.reachableEndY))
		{
			flag = true;
		}
		if (!flag)
		{
			for (int i = providedInfo.reachableStartX; i <= providedInfo.reachableEndX; i++)
			{
				for (int j = providedInfo.reachableStartY; j <= providedInfo.reachableEndY; j++)
				{
					Tile tile = Main.tile[i, j];
					Tile tile2 = Main.tile[i, j + 1];
					if ((!tile.active() || Main.tileCut[tile.type] || TileID.Sets.BreakableWhenPlacing[tile.type]) && tile2.nactive() && !tile2.halfBrick() && tile2.slope() == 0 && Main.tileSolid[tile2.type])
					{
						_targets.Add(new Tuple<int, int>(i, j));
					}
				}
			}
		}
		if (_targets.Count > 0)
		{
			float num = -1f;
			Tuple<int, int> tuple = _targets[0];
			for (int k = 0; k < _targets.Count; k++)
			{
				if (Collision.EmptyTile(_targets[k].Item1, _targets[k].Item2, ignoreTiles: true))
				{
					float num2 = Vector2.Distance(new Vector2((float)_targets[k].Item1, (float)_targets[k].Item2) * 16f + Vector2.One * 8f, providedInfo.mouse);
					if (num == -1f || num2 < num)
					{
						num = num2;
						tuple = _targets[k];
					}
				}
			}
			if (Collision.InTileBounds(tuple.Item1, tuple.Item2, providedInfo.reachableStartX, providedInfo.reachableStartY, providedInfo.reachableEndX, providedInfo.reachableEndY) && num != -1f)
			{
				focusedX = tuple.Item1;
				focusedY = tuple.Item2;
			}
		}
		_targets.Clear();
	}

	private static void Step_PlanterBox(SmartCursorUsageInfo providedInfo, ref int focusedX, ref int focusedY)
	{
		//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0204: Unknown result type (might be due to invalid IL or missing references)
		//IL_0209: Unknown result type (might be due to invalid IL or missing references)
		//IL_0213: Unknown result type (might be due to invalid IL or missing references)
		//IL_0218: Unknown result type (might be due to invalid IL or missing references)
		//IL_021e: Unknown result type (might be due to invalid IL or missing references)
		if (providedInfo.item.createTile != 380 || focusedX != -1 || focusedY != -1)
		{
			return;
		}
		_targets.Clear();
		bool flag = false;
		if (Main.tile[providedInfo.screenTargetX, providedInfo.screenTargetY].active() && Main.tile[providedInfo.screenTargetX, providedInfo.screenTargetY].type == 380)
		{
			flag = true;
		}
		if (!flag)
		{
			for (int i = providedInfo.reachableStartX; i <= providedInfo.reachableEndX; i++)
			{
				for (int j = providedInfo.reachableStartY; j <= providedInfo.reachableEndY; j++)
				{
					Tile tile = Main.tile[i, j];
					if (tile.active() && tile.type == 380)
					{
						if (!Main.tile[i - 1, j].active() || Main.tileCut[Main.tile[i - 1, j].type] || TileID.Sets.BreakableWhenPlacing[Main.tile[i - 1, j].type])
						{
							_targets.Add(new Tuple<int, int>(i - 1, j));
						}
						if (!Main.tile[i + 1, j].active() || Main.tileCut[Main.tile[i + 1, j].type] || TileID.Sets.BreakableWhenPlacing[Main.tile[i + 1, j].type])
						{
							_targets.Add(new Tuple<int, int>(i + 1, j));
						}
					}
				}
			}
		}
		if (_targets.Count > 0)
		{
			float num = -1f;
			Tuple<int, int> tuple = _targets[0];
			for (int k = 0; k < _targets.Count; k++)
			{
				float num2 = Vector2.Distance(new Vector2((float)_targets[k].Item1, (float)_targets[k].Item2) * 16f + Vector2.One * 8f, providedInfo.mouse);
				if (num == -1f || num2 < num)
				{
					num = num2;
					tuple = _targets[k];
				}
			}
			if (Collision.InTileBounds(tuple.Item1, tuple.Item2, providedInfo.reachableStartX, providedInfo.reachableStartY, providedInfo.reachableEndX, providedInfo.reachableEndY) && num != -1f)
			{
				focusedX = tuple.Item1;
				focusedY = tuple.Item2;
			}
		}
		_targets.Clear();
	}

	private static void Step_AlchemySeeds(SmartCursorUsageInfo providedInfo, ref int focusedX, ref int focusedY)
	{
		//IL_0408: Unknown result type (might be due to invalid IL or missing references)
		//IL_0412: Unknown result type (might be due to invalid IL or missing references)
		//IL_0417: Unknown result type (might be due to invalid IL or missing references)
		//IL_0421: Unknown result type (might be due to invalid IL or missing references)
		//IL_0426: Unknown result type (might be due to invalid IL or missing references)
		//IL_042c: Unknown result type (might be due to invalid IL or missing references)
		if (providedInfo.item.createTile != 82 || focusedX != -1 || focusedY != -1)
		{
			return;
		}
		int placeStyle = providedInfo.item.placeStyle;
		_targets.Clear();
		for (int i = providedInfo.reachableStartX; i <= providedInfo.reachableEndX; i++)
		{
			for (int j = providedInfo.reachableStartY; j <= providedInfo.reachableEndY; j++)
			{
				Tile tile = Main.tile[i, j];
				Tile tile2 = Main.tile[i, j + 1];
				bool num4 = !tile.active() || TileID.Sets.BreakableWhenPlacing[tile.type] || (Main.tileCut[tile.type] && tile.type != 82 && tile.type != 83) || WorldGen.IsHarvestableHerbWithSeed(tile.type, tile.frameX / 18);
				bool flag = tile2.nactive() && !tile2.halfBrick() && tile2.slope() == 0;
				if (!num4 || !flag)
				{
					continue;
				}
				switch (placeStyle)
				{
				case 0:
					if ((tile2.type != 78 && tile2.type != 380 && tile2.type != 2 && tile2.type != 477 && tile2.type != 109 && tile2.type != 492) || tile.liquid > 0)
					{
						continue;
					}
					break;
				case 1:
					if ((tile2.type != 78 && tile2.type != 380 && tile2.type != 60) || tile.liquid > 0)
					{
						continue;
					}
					break;
				case 2:
					if ((tile2.type != 78 && tile2.type != 380 && tile2.type != 0 && tile2.type != 59) || tile.liquid > 0)
					{
						continue;
					}
					break;
				case 3:
					if ((tile2.type != 78 && tile2.type != 380 && tile2.type != 203 && tile2.type != 199 && tile2.type != 23 && tile2.type != 25) || tile.liquid > 0)
					{
						continue;
					}
					break;
				case 4:
					if ((tile2.type != 78 && tile2.type != 380 && tile2.type != 53 && tile2.type != 116) || (tile.liquid > 0 && tile.lava()))
					{
						continue;
					}
					break;
				case 5:
					if ((tile2.type != 78 && tile2.type != 380 && tile2.type != 57 && tile2.type != 633) || (tile.liquid > 0 && !tile.lava()))
					{
						continue;
					}
					break;
				case 6:
					if ((tile2.type != 78 && tile2.type != 380 && tile2.type != 147 && tile2.type != 161 && tile2.type != 163 && tile2.type != 164 && tile2.type != 200) || (tile.liquid > 0 && tile.lava()))
					{
						continue;
					}
					break;
				}
				_targets.Add(new Tuple<int, int>(i, j));
			}
		}
		if (_targets.Count > 0)
		{
			float num2 = -1f;
			Tuple<int, int> tuple = _targets[0];
			for (int k = 0; k < _targets.Count; k++)
			{
				float num3 = Vector2.Distance(new Vector2((float)_targets[k].Item1, (float)_targets[k].Item2) * 16f + Vector2.One * 8f, providedInfo.mouse);
				if (num2 == -1f || num3 < num2)
				{
					num2 = num3;
					tuple = _targets[k];
				}
			}
			if (Collision.InTileBounds(tuple.Item1, tuple.Item2, providedInfo.reachableStartX, providedInfo.reachableStartY, providedInfo.reachableEndX, providedInfo.reachableEndY))
			{
				focusedX = tuple.Item1;
				focusedY = tuple.Item2;
			}
		}
		_targets.Clear();
	}

	private static void Step_Actuators(SmartCursorUsageInfo providedInfo, ref int focusedX, ref int focusedY)
	{
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		if (providedInfo.item.type != 849 || focusedX != -1 || focusedY != -1)
		{
			return;
		}
		_targets.Clear();
		for (int i = providedInfo.reachableStartX; i <= providedInfo.reachableEndX; i++)
		{
			for (int j = providedInfo.reachableStartY; j <= providedInfo.reachableEndY; j++)
			{
				Tile tile = Main.tile[i, j];
				if ((tile.wire() || tile.wire2() || tile.wire3() || tile.wire4()) && !tile.actuator() && tile.active())
				{
					_targets.Add(new Tuple<int, int>(i, j));
				}
			}
		}
		if (_targets.Count > 0)
		{
			float num = -1f;
			Tuple<int, int> tuple = _targets[0];
			for (int k = 0; k < _targets.Count; k++)
			{
				float num2 = Vector2.Distance(new Vector2((float)_targets[k].Item1, (float)_targets[k].Item2) * 16f + Vector2.One * 8f, providedInfo.mouse);
				if (num == -1f || num2 < num)
				{
					num = num2;
					tuple = _targets[k];
				}
			}
			if (Collision.InTileBounds(tuple.Item1, tuple.Item2, providedInfo.reachableStartX, providedInfo.reachableStartY, providedInfo.reachableEndX, providedInfo.reachableEndY))
			{
				focusedX = tuple.Item1;
				focusedY = tuple.Item2;
			}
		}
		_targets.Clear();
	}

	private static void Step_EmptyBuckets(SmartCursorUsageInfo providedInfo, ref int focusedX, ref int focusedY)
	{
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		if (providedInfo.item.type != 205 || focusedX != -1 || focusedY != -1)
		{
			return;
		}
		_targets.Clear();
		for (int i = providedInfo.reachableStartX; i <= providedInfo.reachableEndX; i++)
		{
			for (int j = providedInfo.reachableStartY; j <= providedInfo.reachableEndY; j++)
			{
				Tile tile = Main.tile[i, j];
				if (tile.liquid <= 0)
				{
					continue;
				}
				int num = tile.liquidType();
				int num2 = 0;
				for (int k = i - 1; k <= i + 1; k++)
				{
					for (int l = j - 1; l <= j + 1; l++)
					{
						if (Main.tile[k, l].liquidType() == num)
						{
							num2 += Main.tile[k, l].liquid;
						}
					}
				}
				if (num2 > 100)
				{
					_targets.Add(new Tuple<int, int>(i, j));
				}
			}
		}
		if (_targets.Count > 0)
		{
			float num3 = -1f;
			Tuple<int, int> tuple = _targets[0];
			for (int m = 0; m < _targets.Count; m++)
			{
				float num4 = Vector2.Distance(new Vector2((float)_targets[m].Item1, (float)_targets[m].Item2) * 16f + Vector2.One * 8f, providedInfo.mouse);
				if (num3 == -1f || num4 < num3)
				{
					num3 = num4;
					tuple = _targets[m];
				}
			}
			if (Collision.InTileBounds(tuple.Item1, tuple.Item2, providedInfo.reachableStartX, providedInfo.reachableStartY, providedInfo.reachableEndX, providedInfo.reachableEndY))
			{
				focusedX = tuple.Item1;
				focusedY = tuple.Item2;
			}
		}
		_targets.Clear();
	}

	private static void Step_PaintScrapper(SmartCursorUsageInfo providedInfo, ref int focusedX, ref int focusedY)
	{
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		if (!ItemID.Sets.IsPaintScraper[providedInfo.item.type] || focusedX != -1 || focusedY != -1)
		{
			return;
		}
		_targets.Clear();
		for (int i = providedInfo.reachableStartX; i <= providedInfo.reachableEndX; i++)
		{
			for (int j = providedInfo.reachableStartY; j <= providedInfo.reachableEndY; j++)
			{
				Tile tile = Main.tile[i, j];
				bool flag = false;
				if (tile.active())
				{
					flag |= tile.color() > 0;
					flag |= tile.type == 184;
					flag |= tile.fullbrightBlock();
					flag |= tile.invisibleBlock();
				}
				if (tile.wall > 0)
				{
					flag |= tile.wallColor() > 0;
					flag |= tile.fullbrightWall();
					flag |= tile.invisibleWall();
				}
				if (flag)
				{
					_targets.Add(new Tuple<int, int>(i, j));
				}
			}
		}
		if (_targets.Count > 0)
		{
			float num = -1f;
			Tuple<int, int> tuple = _targets[0];
			for (int k = 0; k < _targets.Count; k++)
			{
				float num2 = Vector2.Distance(new Vector2((float)_targets[k].Item1, (float)_targets[k].Item2) * 16f + Vector2.One * 8f, providedInfo.mouse);
				if (num == -1f || num2 < num)
				{
					num = num2;
					tuple = _targets[k];
				}
			}
			if (Collision.InTileBounds(tuple.Item1, tuple.Item2, providedInfo.reachableStartX, providedInfo.reachableStartY, providedInfo.reachableEndX, providedInfo.reachableEndY))
			{
				focusedX = tuple.Item1;
				focusedY = tuple.Item2;
			}
		}
		_targets.Clear();
	}

	private static void Step_PaintBrush(SmartCursorUsageInfo providedInfo, ref int focusedX, ref int focusedY)
	{
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		if ((providedInfo.item.type != 1071 && providedInfo.item.type != 1543) || (providedInfo.paintLookup == 0 && providedInfo.paintCoatingLookup == 0) || focusedX != -1 || focusedY != -1)
		{
			return;
		}
		_targets.Clear();
		int paintLookup = providedInfo.paintLookup;
		int paintCoatingLookup = providedInfo.paintCoatingLookup;
		if (paintLookup != 0 || paintCoatingLookup != 0)
		{
			for (int i = providedInfo.reachableStartX; i <= providedInfo.reachableEndX; i++)
			{
				for (int j = providedInfo.reachableStartY; j <= providedInfo.reachableEndY; j++)
				{
					Tile tile = Main.tile[i, j];
					if (tile.active() && ((paintLookup != 0 && tile.color() != paintLookup) | (paintCoatingLookup == 1 && !tile.fullbrightBlock()) | (paintCoatingLookup == 2 && !tile.invisibleBlock())))
					{
						_targets.Add(new Tuple<int, int>(i, j));
					}
				}
			}
		}
		if (_targets.Count > 0)
		{
			float num = -1f;
			Tuple<int, int> tuple = _targets[0];
			for (int k = 0; k < _targets.Count; k++)
			{
				float num2 = Vector2.Distance(new Vector2((float)_targets[k].Item1, (float)_targets[k].Item2) * 16f + Vector2.One * 8f, providedInfo.mouse);
				if (num == -1f || num2 < num)
				{
					num = num2;
					tuple = _targets[k];
				}
			}
			if (Collision.InTileBounds(tuple.Item1, tuple.Item2, providedInfo.reachableStartX, providedInfo.reachableStartY, providedInfo.reachableEndX, providedInfo.reachableEndY))
			{
				focusedX = tuple.Item1;
				focusedY = tuple.Item2;
			}
		}
		_targets.Clear();
	}

	private static void Step_PaintRoller(SmartCursorUsageInfo providedInfo, ref int focusedX, ref int focusedY)
	{
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_018c: Unknown result type (might be due to invalid IL or missing references)
		if ((providedInfo.item.type != 1072 && providedInfo.item.type != 1544) || (providedInfo.paintLookup == 0 && providedInfo.paintCoatingLookup == 0) || focusedX != -1 || focusedY != -1)
		{
			return;
		}
		_targets.Clear();
		int paintLookup = providedInfo.paintLookup;
		int paintCoatingLookup = providedInfo.paintCoatingLookup;
		for (int i = providedInfo.reachableStartX; i <= providedInfo.reachableEndX; i++)
		{
			for (int j = providedInfo.reachableStartY; j <= providedInfo.reachableEndY; j++)
			{
				Tile tile = Main.tile[i, j];
				if (tile.wall > 0 && (!tile.active() || !Main.tileSolid[tile.type] || Main.tileSolidTop[tile.type]) && ((paintLookup != 0 && tile.wallColor() != paintLookup) | (paintCoatingLookup == 1 && !tile.fullbrightWall()) | (paintCoatingLookup == 2 && !tile.invisibleWall())))
				{
					_targets.Add(new Tuple<int, int>(i, j));
				}
			}
		}
		if (_targets.Count > 0)
		{
			float num = -1f;
			Tuple<int, int> tuple = _targets[0];
			for (int k = 0; k < _targets.Count; k++)
			{
				float num2 = Vector2.Distance(new Vector2((float)_targets[k].Item1, (float)_targets[k].Item2) * 16f + Vector2.One * 8f, providedInfo.mouse);
				if (num == -1f || num2 < num)
				{
					num = num2;
					tuple = _targets[k];
				}
			}
			if (Collision.InTileBounds(tuple.Item1, tuple.Item2, providedInfo.reachableStartX, providedInfo.reachableStartY, providedInfo.reachableEndX, providedInfo.reachableEndY))
			{
				focusedX = tuple.Item1;
				focusedY = tuple.Item2;
			}
		}
		_targets.Clear();
	}

	private static void Step_BlocksLines(SmartCursorUsageInfo providedInfo, ref int focusedX, ref int focusedY)
	{
		//IL_035d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0367: Unknown result type (might be due to invalid IL or missing references)
		//IL_036c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0376: Unknown result type (might be due to invalid IL or missing references)
		//IL_037b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0381: Unknown result type (might be due to invalid IL or missing references)
		int type = providedInfo.item.type;
		if (type < 0 || !Player.SmartCursorSettings.SmartBlocksEnabled || providedInfo.item.createTile <= -1 || type == 213 || type == 5295 || ItemID.Sets.GrassSeeds[type] || !Main.tileSolid[providedInfo.item.createTile] || Main.tileSolidTop[providedInfo.item.createTile] || Main.tileFrameImportant[providedInfo.item.createTile] || focusedX != -1 || focusedY != -1)
		{
			return;
		}
		_targets.Clear();
		bool flag = false;
		if (Main.tile[providedInfo.screenTargetX, providedInfo.screenTargetY].active())
		{
			flag = true;
		}
		if (!Collision.InTileBounds(providedInfo.screenTargetX, providedInfo.screenTargetY, providedInfo.reachableStartX, providedInfo.reachableStartY, providedInfo.reachableEndX, providedInfo.reachableEndY))
		{
			flag = true;
		}
		if (!flag)
		{
			for (int i = providedInfo.reachableStartX; i <= providedInfo.reachableEndX; i++)
			{
				for (int j = providedInfo.reachableStartY; j <= providedInfo.reachableEndY; j++)
				{
					Tile tile = Main.tile[i, j];
					if (!tile.active() || Main.tileCut[tile.type] || TileID.Sets.BreakableWhenPlacing[tile.type])
					{
						bool flag2 = false;
						if (Main.tile[i - 1, j].active() && Main.tileSolid[Main.tile[i - 1, j].type] && !Main.tileSolidTop[Main.tile[i - 1, j].type])
						{
							flag2 = true;
						}
						if (Main.tile[i + 1, j].active() && Main.tileSolid[Main.tile[i + 1, j].type] && !Main.tileSolidTop[Main.tile[i + 1, j].type])
						{
							flag2 = true;
						}
						if (Main.tile[i, j - 1].active() && Main.tileSolid[Main.tile[i, j - 1].type] && !Main.tileSolidTop[Main.tile[i, j - 1].type])
						{
							flag2 = true;
						}
						if (Main.tile[i, j + 1].active() && Main.tileSolid[Main.tile[i, j + 1].type] && !Main.tileSolidTop[Main.tile[i, j + 1].type])
						{
							flag2 = true;
						}
						if (flag2)
						{
							_targets.Add(new Tuple<int, int>(i, j));
						}
					}
				}
			}
		}
		if (_targets.Count > 0)
		{
			float num = -1f;
			Tuple<int, int> tuple = _targets[0];
			for (int k = 0; k < _targets.Count; k++)
			{
				if (Collision.EmptyTile(_targets[k].Item1, _targets[k].Item2))
				{
					float num2 = Vector2.Distance(new Vector2((float)_targets[k].Item1, (float)_targets[k].Item2) * 16f + Vector2.One * 8f, providedInfo.mouse);
					if (num == -1f || num2 < num)
					{
						num = num2;
						tuple = _targets[k];
					}
				}
			}
			if (Collision.InTileBounds(tuple.Item1, tuple.Item2, providedInfo.reachableStartX, providedInfo.reachableStartY, providedInfo.reachableEndX, providedInfo.reachableEndY) && num != -1f)
			{
				focusedX = tuple.Item1;
				focusedY = tuple.Item2;
			}
		}
		_targets.Clear();
	}

	private static void Step_Boulders(SmartCursorUsageInfo providedInfo, ref int focusedX, ref int focusedY)
	{
		//IL_0256: Unknown result type (might be due to invalid IL or missing references)
		//IL_0260: Unknown result type (might be due to invalid IL or missing references)
		//IL_0265: Unknown result type (might be due to invalid IL or missing references)
		//IL_026f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0274: Unknown result type (might be due to invalid IL or missing references)
		//IL_027a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
		if (providedInfo.item.createTile <= -1 || !TileID.Sets.Boulders[providedInfo.item.createTile] || focusedX != -1 || focusedY != -1)
		{
			return;
		}
		_targets.Clear();
		Rectangle value = default(Rectangle);
		for (int i = providedInfo.reachableStartX; i <= providedInfo.reachableEndX; i++)
		{
			for (int j = providedInfo.reachableStartY; j <= providedInfo.reachableEndY; j++)
			{
				Tile tile = Main.tile[i, j + 1];
				Tile tile2 = Main.tile[i + 1, j + 1];
				bool flag = true;
				if (!tile2.nactive() || !tile.nactive())
				{
					flag = false;
				}
				if (tile2.slope() > 0 || tile.slope() > 0 || tile2.halfBrick() || tile.halfBrick())
				{
					flag = false;
				}
				if ((!Main.tileSolid[tile2.type] && !Main.tileTable[tile2.type]) || (!Main.tileSolid[tile.type] && !Main.tileTable[tile.type]))
				{
					flag = false;
				}
				if (Main.tileNoAttach[tile2.type] || Main.tileNoAttach[tile.type])
				{
					flag = false;
				}
				for (int k = i; k <= i + 1; k++)
				{
					for (int l = j - 1; l <= j; l++)
					{
						Tile tile3 = Main.tile[k, l];
						if (tile3.active() && !Main.tileCut[tile3.type])
						{
							flag = false;
						}
					}
				}
				int x = i * 16;
				int y = j * 16 - 16;
				int width = 32;
				int height = 32;
				((Rectangle)(ref value))._002Ector(x, y, width, height);
				for (int m = 0; m < 255; m++)
				{
					Player player = Main.player[m];
					if (player.active && !player.dead)
					{
						Rectangle hitbox = player.Hitbox;
						if (((Rectangle)(ref hitbox)).Intersects(value))
						{
							flag = false;
							break;
						}
					}
				}
				if (flag)
				{
					_targets.Add(new Tuple<int, int>(i, j));
				}
			}
		}
		if (_targets.Count > 0)
		{
			float num = -1f;
			Tuple<int, int> tuple = _targets[0];
			for (int n = 0; n < _targets.Count; n++)
			{
				float num2 = Vector2.Distance(new Vector2((float)_targets[n].Item1, (float)_targets[n].Item2) * 16f + Vector2.One * 8f, providedInfo.mouse);
				if (num == -1f || num2 < num)
				{
					num = num2;
					tuple = _targets[n];
				}
			}
			if (Collision.InTileBounds(tuple.Item1, tuple.Item2, providedInfo.reachableStartX, providedInfo.reachableStartY, providedInfo.reachableEndX, providedInfo.reachableEndY))
			{
				focusedX = tuple.Item1;
				focusedY = tuple.Item2;
			}
		}
		_targets.Clear();
	}

	private static void Step_Pigronata(SmartCursorUsageInfo providedInfo, ref int focusedX, ref int focusedY)
	{
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		if (providedInfo.item.createTile != 454 || focusedX != -1 || focusedY != -1)
		{
			return;
		}
		_targets.Clear();
		for (int i = providedInfo.reachableStartX; i <= providedInfo.reachableEndX; i++)
		{
			for (int j = providedInfo.reachableStartY; j <= providedInfo.reachableEndY && !((double)j > Main.worldSurface - 2.0); j++)
			{
				bool flag = true;
				for (int k = i - 2; k <= i + 1; k++)
				{
					for (int l = j - 1; l <= j + 2; l++)
					{
						Tile tile = Main.tile[k, l];
						if (l == j - 1)
						{
							if (!WorldGen.SolidTile(tile))
							{
								flag = false;
							}
						}
						else if (tile.active() && (!Main.tileCut[tile.type] || tile.type == 454))
						{
							flag = false;
						}
					}
				}
				if (flag)
				{
					_targets.Add(new Tuple<int, int>(i, j));
				}
			}
		}
		if (_targets.Count > 0)
		{
			float num = -1f;
			Tuple<int, int> tuple = _targets[0];
			for (int m = 0; m < _targets.Count; m++)
			{
				float num2 = Vector2.Distance(new Vector2((float)_targets[m].Item1, (float)_targets[m].Item2) * 16f + Vector2.One * 8f, providedInfo.mouse);
				if (num == -1f || num2 < num)
				{
					num = num2;
					tuple = _targets[m];
				}
			}
			if (Collision.InTileBounds(tuple.Item1, tuple.Item2, providedInfo.reachableStartX, providedInfo.reachableStartY, providedInfo.reachableEndX, providedInfo.reachableEndY))
			{
				focusedX = tuple.Item1;
				focusedY = tuple.Item2;
			}
		}
		_targets.Clear();
	}

	private static void Step_PumpkinSeeds(SmartCursorUsageInfo providedInfo, ref int focusedX, ref int focusedY)
	{
		//IL_020f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0219: Unknown result type (might be due to invalid IL or missing references)
		//IL_021e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0228: Unknown result type (might be due to invalid IL or missing references)
		//IL_022d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0233: Unknown result type (might be due to invalid IL or missing references)
		if (providedInfo.item.createTile != 254 || focusedX != -1 || focusedY != -1)
		{
			return;
		}
		_targets.Clear();
		for (int i = providedInfo.reachableStartX; i <= providedInfo.reachableEndX; i++)
		{
			for (int j = providedInfo.reachableStartY; j <= providedInfo.reachableEndY; j++)
			{
				Tile tile = Main.tile[i, j + 1];
				Tile tile2 = Main.tile[i + 1, j + 1];
				if ((double)j > Main.worldSurface - 2.0)
				{
					break;
				}
				bool flag = true;
				if (!tile2.active() || !tile.active())
				{
					flag = false;
				}
				if (tile2.slope() > 0 || tile.slope() > 0 || tile2.halfBrick() || tile.halfBrick())
				{
					flag = false;
				}
				if (tile2.type != 2 && tile2.type != 477 && tile2.type != 109 && tile2.type != 492)
				{
					flag = false;
				}
				if (tile.type != 2 && tile.type != 477 && tile.type != 109 && tile.type != 492)
				{
					flag = false;
				}
				for (int k = i; k <= i + 1; k++)
				{
					for (int l = j - 1; l <= j; l++)
					{
						Tile tile3 = Main.tile[k, l];
						if (tile3.active() && (tile3.type < 0 || Main.tileSolid[tile3.type] || !WorldGen.CanCutTile(k, l, TileCuttingContext.TilePlacement)))
						{
							flag = false;
						}
					}
				}
				if (flag)
				{
					_targets.Add(new Tuple<int, int>(i, j));
				}
			}
		}
		if (_targets.Count > 0)
		{
			float num = -1f;
			Tuple<int, int> tuple = _targets[0];
			for (int m = 0; m < _targets.Count; m++)
			{
				float num2 = Vector2.Distance(new Vector2((float)_targets[m].Item1, (float)_targets[m].Item2) * 16f + Vector2.One * 8f, providedInfo.mouse);
				if (num == -1f || num2 < num)
				{
					num = num2;
					tuple = _targets[m];
				}
			}
			if (Collision.InTileBounds(tuple.Item1, tuple.Item2, providedInfo.reachableStartX, providedInfo.reachableStartY, providedInfo.reachableEndX, providedInfo.reachableEndY))
			{
				focusedX = tuple.Item1;
				focusedY = tuple.Item2;
			}
		}
		_targets.Clear();
	}

	private static void Step_Walls(SmartCursorUsageInfo providedInfo, ref int focusedX, ref int focusedY)
	{
		//IL_023c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0246: Unknown result type (might be due to invalid IL or missing references)
		//IL_024b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0255: Unknown result type (might be due to invalid IL or missing references)
		//IL_025a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0260: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		int width = providedInfo.player.width;
		int height = providedInfo.player.height;
		if (providedInfo.item.createWall <= 0 || focusedX != -1 || focusedY != -1)
		{
			return;
		}
		_targets.Clear();
		for (int i = providedInfo.reachableStartX; i <= providedInfo.reachableEndX; i++)
		{
			for (int j = providedInfo.reachableStartY; j <= providedInfo.reachableEndY; j++)
			{
				Tile tile = Main.tile[i, j];
				if (tile.wall == 0 && (!tile.active() || !Main.tileSolid[tile.type] || Main.tileSolidTop[tile.type]) && Collision.CanHitWithCheck(providedInfo.position, width, height, new Vector2((float)i, (float)j) * 16f, 16, 16, DelegateMethods.NotDoorStand))
				{
					bool flag = false;
					if (Main.tile[i - 1, j].active() || Main.tile[i - 1, j].wall > 0)
					{
						flag = true;
					}
					if (Main.tile[i + 1, j].active() || Main.tile[i + 1, j].wall > 0)
					{
						flag = true;
					}
					if (Main.tile[i, j - 1].active() || Main.tile[i, j - 1].wall > 0)
					{
						flag = true;
					}
					if (Main.tile[i, j + 1].active() || Main.tile[i, j + 1].wall > 0)
					{
						flag = true;
					}
					if (WorldGen.IsOpenDoorAnchorFrame(i, j))
					{
						flag = false;
					}
					if (flag)
					{
						_targets.Add(new Tuple<int, int>(i, j));
					}
				}
			}
		}
		if (_targets.Count > 0)
		{
			float num = -1f;
			Tuple<int, int> tuple = _targets[0];
			for (int k = 0; k < _targets.Count; k++)
			{
				float num2 = Vector2.Distance(new Vector2((float)_targets[k].Item1, (float)_targets[k].Item2) * 16f + Vector2.One * 8f, providedInfo.mouse);
				if (num == -1f || num2 < num)
				{
					num = num2;
					tuple = _targets[k];
				}
			}
			if (Collision.InTileBounds(tuple.Item1, tuple.Item2, providedInfo.reachableStartX, providedInfo.reachableStartY, providedInfo.reachableEndX, providedInfo.reachableEndY))
			{
				focusedX = tuple.Item1;
				focusedY = tuple.Item2;
			}
		}
		_targets.Clear();
	}

	private static void Step_MinecartTracks(SmartCursorUsageInfo providedInfo, ref int focusedX, ref int focusedY)
	{
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aed: Unknown result type (might be due to invalid IL or missing references)
		//IL_0af7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0afc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b06: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b0b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b11: Unknown result type (might be due to invalid IL or missing references)
		//IL_0721: Unknown result type (might be due to invalid IL or missing references)
		//IL_072b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0730: Unknown result type (might be due to invalid IL or missing references)
		//IL_073a: Unknown result type (might be due to invalid IL or missing references)
		//IL_073f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0745: Unknown result type (might be due to invalid IL or missing references)
		if ((providedInfo.item.type == 2340 || providedInfo.item.type == 2739) && focusedX == -1 && focusedY == -1)
		{
			_targets.Clear();
			Vector2 val = (Main.MouseWorld - providedInfo.Center).SafeNormalize(Vector2.UnitY);
			float num7 = Vector2.Dot(val, -Vector2.UnitY);
			bool flag = num7 >= 0.5f;
			bool flag7 = num7 <= -0.5f;
			float num8 = Vector2.Dot(val, Vector2.UnitX);
			bool flag8 = num8 >= 0.5f;
			bool flag9 = num8 <= -0.5f;
			bool flag10 = flag && flag9;
			bool flag11 = flag && flag8;
			bool flag12 = flag7 && flag9;
			bool flag13 = flag7 && flag8;
			if (flag10)
			{
				flag9 = false;
			}
			if (flag11)
			{
				flag8 = false;
			}
			if (flag12)
			{
				flag9 = false;
			}
			if (flag13)
			{
				flag8 = false;
			}
			bool flag14 = false;
			if (Main.tile[providedInfo.screenTargetX, providedInfo.screenTargetY].active() && Main.tile[providedInfo.screenTargetX, providedInfo.screenTargetY].type == 314)
			{
				flag14 = true;
			}
			if (!flag14)
			{
				for (int i = providedInfo.reachableStartX; i <= providedInfo.reachableEndX; i++)
				{
					for (int j = providedInfo.reachableStartY; j <= providedInfo.reachableEndY; j++)
					{
						Tile tile = Main.tile[i, j];
						if (tile.active() && tile.type == 314)
						{
							bool flag2 = Main.tile[i + 1, j + 1].active() && Main.tile[i + 1, j + 1].type == 314;
							bool flag3 = Main.tile[i + 1, j - 1].active() && Main.tile[i + 1, j - 1].type == 314;
							bool flag4 = Main.tile[i - 1, j + 1].active() && Main.tile[i - 1, j + 1].type == 314;
							bool flag5 = Main.tile[i - 1, j - 1].active() && Main.tile[i - 1, j - 1].type == 314;
							if (flag10 && (!Main.tile[i - 1, j - 1].active() || Main.tileCut[Main.tile[i - 1, j - 1].type] || TileID.Sets.BreakableWhenPlacing[Main.tile[i - 1, j - 1].type]) && !(!flag2 && flag3) && !flag4)
							{
								_targets.Add(new Tuple<int, int>(i - 1, j - 1));
							}
							if (flag9 && (!Main.tile[i - 1, j].active() || Main.tileCut[Main.tile[i - 1, j].type] || TileID.Sets.BreakableWhenPlacing[Main.tile[i - 1, j].type]))
							{
								_targets.Add(new Tuple<int, int>(i - 1, j));
							}
							if (flag12 && (!Main.tile[i - 1, j + 1].active() || Main.tileCut[Main.tile[i - 1, j + 1].type] || TileID.Sets.BreakableWhenPlacing[Main.tile[i - 1, j + 1].type]) && !(!flag3 && flag2) && !flag5)
							{
								_targets.Add(new Tuple<int, int>(i - 1, j + 1));
							}
							if (flag11 && (!Main.tile[i + 1, j - 1].active() || Main.tileCut[Main.tile[i + 1, j - 1].type] || TileID.Sets.BreakableWhenPlacing[Main.tile[i + 1, j - 1].type]) && !(!flag4 && flag5) && !flag2)
							{
								_targets.Add(new Tuple<int, int>(i + 1, j - 1));
							}
							if (flag8 && (!Main.tile[i + 1, j].active() || Main.tileCut[Main.tile[i + 1, j].type] || TileID.Sets.BreakableWhenPlacing[Main.tile[i + 1, j].type]))
							{
								_targets.Add(new Tuple<int, int>(i + 1, j));
							}
							if (flag13 && (!Main.tile[i + 1, j + 1].active() || Main.tileCut[Main.tile[i + 1, j + 1].type] || TileID.Sets.BreakableWhenPlacing[Main.tile[i + 1, j + 1].type]) && !(!flag5 && flag4) && !flag3)
							{
								_targets.Add(new Tuple<int, int>(i + 1, j + 1));
							}
						}
					}
				}
			}
			if (_targets.Count > 0)
			{
				float num3 = -1f;
				Tuple<int, int> tuple = _targets[0];
				for (int k = 0; k < _targets.Count; k++)
				{
					if ((!Main.tile[_targets[k].Item1, _targets[k].Item2 - 1].active() || Main.tile[_targets[k].Item1, _targets[k].Item2 - 1].type != 314) && (!Main.tile[_targets[k].Item1, _targets[k].Item2 + 1].active() || Main.tile[_targets[k].Item1, _targets[k].Item2 + 1].type != 314))
					{
						float num4 = Vector2.Distance(new Vector2((float)_targets[k].Item1, (float)_targets[k].Item2) * 16f + Vector2.One * 8f, providedInfo.mouse);
						if (num3 == -1f || num4 < num3)
						{
							num3 = num4;
							tuple = _targets[k];
						}
					}
				}
				if (Collision.InTileBounds(tuple.Item1, tuple.Item2, providedInfo.reachableStartX, providedInfo.reachableStartY, providedInfo.reachableEndX, providedInfo.reachableEndY) && num3 != -1f)
				{
					focusedX = tuple.Item1;
					focusedY = tuple.Item2;
				}
			}
			_targets.Clear();
		}
		if (providedInfo.item.type != 2492 || focusedX != -1 || focusedY != -1)
		{
			return;
		}
		_targets.Clear();
		bool flag6 = false;
		if (Main.tile[providedInfo.screenTargetX, providedInfo.screenTargetY].active() && Main.tile[providedInfo.screenTargetX, providedInfo.screenTargetY].type == 314)
		{
			flag6 = true;
		}
		if (!flag6)
		{
			for (int l = providedInfo.reachableStartX; l <= providedInfo.reachableEndX; l++)
			{
				for (int m = providedInfo.reachableStartY; m <= providedInfo.reachableEndY; m++)
				{
					Tile tile2 = Main.tile[l, m];
					if (tile2.active() && tile2.type == 314)
					{
						if (!Main.tile[l - 1, m].active() || Main.tileCut[Main.tile[l - 1, m].type] || TileID.Sets.BreakableWhenPlacing[Main.tile[l - 1, m].type])
						{
							_targets.Add(new Tuple<int, int>(l - 1, m));
						}
						if (!Main.tile[l + 1, m].active() || Main.tileCut[Main.tile[l + 1, m].type] || TileID.Sets.BreakableWhenPlacing[Main.tile[l + 1, m].type])
						{
							_targets.Add(new Tuple<int, int>(l + 1, m));
						}
					}
				}
			}
		}
		if (_targets.Count > 0)
		{
			float num5 = -1f;
			Tuple<int, int> tuple2 = _targets[0];
			for (int n = 0; n < _targets.Count; n++)
			{
				if ((!Main.tile[_targets[n].Item1, _targets[n].Item2 - 1].active() || Main.tile[_targets[n].Item1, _targets[n].Item2 - 1].type != 314) && (!Main.tile[_targets[n].Item1, _targets[n].Item2 + 1].active() || Main.tile[_targets[n].Item1, _targets[n].Item2 + 1].type != 314))
				{
					float num6 = Vector2.Distance(new Vector2((float)_targets[n].Item1, (float)_targets[n].Item2) * 16f + Vector2.One * 8f, providedInfo.mouse);
					if (num5 == -1f || num6 < num5)
					{
						num5 = num6;
						tuple2 = _targets[n];
					}
				}
			}
			if (Collision.InTileBounds(tuple2.Item1, tuple2.Item2, providedInfo.reachableStartX, providedInfo.reachableStartY, providedInfo.reachableEndX, providedInfo.reachableEndY) && num5 != -1f)
			{
				focusedX = tuple2.Item1;
				focusedY = tuple2.Item2;
			}
		}
		_targets.Clear();
	}

	private static void Step_Platforms(SmartCursorUsageInfo providedInfo, ref int focusedX, ref int focusedY)
	{
		//IL_036f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0379: Unknown result type (might be due to invalid IL or missing references)
		//IL_037e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0388: Unknown result type (might be due to invalid IL or missing references)
		//IL_038d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0393: Unknown result type (might be due to invalid IL or missing references)
		if (providedInfo.item.createTile < 0 || !TileID.Sets.Platforms[providedInfo.item.createTile] || focusedX != -1 || focusedY != -1)
		{
			return;
		}
		_targets.Clear();
		bool flag = false;
		if (Main.tile[providedInfo.screenTargetX, providedInfo.screenTargetY].active() && TileID.Sets.Platforms[Main.tile[providedInfo.screenTargetX, providedInfo.screenTargetY].type])
		{
			flag = true;
		}
		if (!flag)
		{
			for (int i = providedInfo.reachableStartX; i <= providedInfo.reachableEndX; i++)
			{
				for (int j = providedInfo.reachableStartY; j <= providedInfo.reachableEndY; j++)
				{
					Tile tile = Main.tile[i, j];
					if (tile.active() && TileID.Sets.Platforms[tile.type])
					{
						byte num6 = tile.slope();
						if (num6 != 2 && !Main.tile[i - 1, j - 1].active())
						{
							_targets.Add(new Tuple<int, int>(i - 1, j - 1));
						}
						if (!Main.tile[i - 1, j].active())
						{
							_targets.Add(new Tuple<int, int>(i - 1, j));
						}
						if (num6 != 1 && !Main.tile[i - 1, j + 1].active())
						{
							_targets.Add(new Tuple<int, int>(i - 1, j + 1));
						}
						if (num6 != 1 && !Main.tile[i + 1, j - 1].active())
						{
							_targets.Add(new Tuple<int, int>(i + 1, j - 1));
						}
						if (!Main.tile[i + 1, j].active())
						{
							_targets.Add(new Tuple<int, int>(i + 1, j));
						}
						if (num6 != 2 && !Main.tile[i + 1, j + 1].active())
						{
							_targets.Add(new Tuple<int, int>(i + 1, j + 1));
						}
					}
					if (!tile.active())
					{
						int num2 = 0;
						int num3 = 0;
						num2 = 0;
						num3 = 1;
						Tile tile2 = Main.tile[i + num2, j + num3];
						if (tile2.active() && Main.tileSolid[tile2.type] && !Main.tileSolidTop[tile2.type])
						{
							_targets.Add(new Tuple<int, int>(i, j));
						}
						num2 = -1;
						num3 = 0;
						tile2 = Main.tile[i + num2, j + num3];
						if (tile2.active() && Main.tileSolid[tile2.type] && !Main.tileSolidTop[tile2.type])
						{
							_targets.Add(new Tuple<int, int>(i, j));
						}
						num2 = 1;
						num3 = 0;
						tile2 = Main.tile[i + num2, j + num3];
						if (tile2.active() && Main.tileSolid[tile2.type] && !Main.tileSolidTop[tile2.type])
						{
							_targets.Add(new Tuple<int, int>(i, j));
						}
					}
				}
			}
		}
		if (_targets.Count > 0)
		{
			float num4 = -1f;
			Tuple<int, int> tuple = _targets[0];
			for (int k = 0; k < _targets.Count; k++)
			{
				float num5 = Vector2.Distance(new Vector2((float)_targets[k].Item1, (float)_targets[k].Item2) * 16f + Vector2.One * 8f, providedInfo.mouse);
				if (num4 == -1f || num5 < num4)
				{
					num4 = num5;
					tuple = _targets[k];
				}
			}
			if (Collision.InTileBounds(tuple.Item1, tuple.Item2, providedInfo.reachableStartX, providedInfo.reachableStartY, providedInfo.reachableEndX, providedInfo.reachableEndY))
			{
				focusedX = tuple.Item1;
				focusedY = tuple.Item2;
			}
		}
		_targets.Clear();
	}

	private static void Step_WireCutter(SmartCursorUsageInfo providedInfo, ref int focusedX, ref int focusedY)
	{
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		if (providedInfo.item.type != 510 || focusedX != -1 || focusedY != -1)
		{
			return;
		}
		_targets.Clear();
		for (int i = providedInfo.reachableStartX; i <= providedInfo.reachableEndX; i++)
		{
			for (int j = providedInfo.reachableStartY; j <= providedInfo.reachableEndY; j++)
			{
				Tile tile = Main.tile[i, j];
				if (tile.wire() || tile.wire2() || tile.wire3() || tile.wire4() || tile.actuator())
				{
					_targets.Add(new Tuple<int, int>(i, j));
				}
			}
		}
		if (_targets.Count > 0)
		{
			float num = -1f;
			Tuple<int, int> tuple = _targets[0];
			for (int k = 0; k < _targets.Count; k++)
			{
				float num2 = Vector2.Distance(new Vector2((float)_targets[k].Item1, (float)_targets[k].Item2) * 16f + Vector2.One * 8f, providedInfo.mouse);
				if (num == -1f || num2 < num)
				{
					num = num2;
					tuple = _targets[k];
				}
			}
			if (Collision.InTileBounds(tuple.Item1, tuple.Item2, providedInfo.reachableStartX, providedInfo.reachableStartY, providedInfo.reachableEndX, providedInfo.reachableEndY))
			{
				focusedX = tuple.Item1;
				focusedY = tuple.Item2;
			}
		}
		_targets.Clear();
	}

	private static void Step_ActuationRod(SmartCursorUsageInfo providedInfo, ref int focusedX, ref int focusedY)
	{
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		bool actuationRodLock = providedInfo.player.ActuationRodLock;
		bool actuationRodLockSetting = providedInfo.player.ActuationRodLockSetting;
		if (providedInfo.item.type != 3620 || focusedX != -1 || focusedY != -1)
		{
			return;
		}
		_targets.Clear();
		for (int i = providedInfo.reachableStartX; i <= providedInfo.reachableEndX; i++)
		{
			for (int j = providedInfo.reachableStartY; j <= providedInfo.reachableEndY; j++)
			{
				Tile tile = Main.tile[i, j];
				if (tile.active() && tile.actuator() && (!actuationRodLock || actuationRodLockSetting == tile.inActive()))
				{
					_targets.Add(new Tuple<int, int>(i, j));
				}
			}
		}
		if (_targets.Count > 0)
		{
			float num = -1f;
			Tuple<int, int> tuple = _targets[0];
			for (int k = 0; k < _targets.Count; k++)
			{
				float num2 = Vector2.Distance(new Vector2((float)_targets[k].Item1, (float)_targets[k].Item2) * 16f + Vector2.One * 8f, providedInfo.mouse);
				if (num == -1f || num2 < num)
				{
					num = num2;
					tuple = _targets[k];
				}
			}
			if (Collision.InTileBounds(tuple.Item1, tuple.Item2, providedInfo.reachableStartX, providedInfo.reachableStartY, providedInfo.reachableEndX, providedInfo.reachableEndY))
			{
				focusedX = tuple.Item1;
				focusedY = tuple.Item2;
			}
		}
		_targets.Clear();
	}

	private static void Step_Hammers(SmartCursorUsageInfo providedInfo, ref int focusedX, ref int focusedY)
	{
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_07cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_058f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0599: Unknown result type (might be due to invalid IL or missing references)
		//IL_059e: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b3: Unknown result type (might be due to invalid IL or missing references)
		int width = providedInfo.player.width;
		int height = providedInfo.player.height;
		if (providedInfo.item.hammer > 0 && focusedX == -1 && focusedY == -1)
		{
			Vector2 vector = providedInfo.mouse - providedInfo.Center;
			int num = Math.Sign(vector.X);
			int num10 = Math.Sign(vector.Y);
			if (Math.Abs(vector.X) > Math.Abs(vector.Y) * 3f)
			{
				num10 = 0;
				providedInfo.mouse.Y = providedInfo.Center.Y;
			}
			if (Math.Abs(vector.Y) > Math.Abs(vector.X) * 3f)
			{
				num = 0;
				providedInfo.mouse.X = providedInfo.Center.X;
			}
			_ = (int)providedInfo.Center.X / 16;
			_ = (int)providedInfo.Center.Y / 16;
			_points.Clear();
			_endpoints.Clear();
			int num11 = 1;
			if (num10 == -1 && num != 0)
			{
				num11 = -1;
			}
			int num12 = (int)((providedInfo.position.X + (float)(width / 2) + (float)((width / 2 - 1) * num)) / 16f);
			int num13 = (int)(((double)providedInfo.position.Y + 0.1) / 16.0);
			if (num11 == -1)
			{
				num13 = (int)((providedInfo.position.Y + (float)height - 1f) / 16f);
			}
			int num14 = width / 16 + ((width % 16 != 0) ? 1 : 0);
			int num15 = height / 16 + ((height % 16 != 0) ? 1 : 0);
			if (num != 0)
			{
				for (int i = 0; i < num15; i++)
				{
					if (Main.tile[num12, num13 + i * num11] != null)
					{
						_points.Add(new Tuple<int, int>(num12, num13 + i * num11));
					}
				}
			}
			if (num10 != 0)
			{
				for (int j = 0; j < num14; j++)
				{
					if (Main.tile[(int)(providedInfo.position.X / 16f) + j, num13] != null)
					{
						_points.Add(new Tuple<int, int>((int)(providedInfo.position.X / 16f) + j, num13));
					}
				}
			}
			int num16 = (int)((providedInfo.mouse.X + (float)((width / 2 - 1) * num)) / 16f);
			int num17 = (int)(((double)providedInfo.mouse.Y + 0.1 - (double)(height / 2 + 1)) / 16.0);
			if (num11 == -1)
			{
				num17 = (int)((providedInfo.mouse.Y + (float)(height / 2) - 1f) / 16f);
			}
			if (providedInfo.player.gravDir == -1f && num10 == 0)
			{
				num17++;
			}
			if (num17 < 10)
			{
				num17 = 10;
			}
			if (num17 > Main.maxTilesY - 10)
			{
				num17 = Main.maxTilesY - 10;
			}
			int num2 = width / 16 + ((width % 16 != 0) ? 1 : 0);
			int num3 = height / 16 + ((height % 16 != 0) ? 1 : 0);
			if (num != 0)
			{
				for (int k = 0; k < num3; k++)
				{
					if (Main.tile[num16, num17 + k * num11] != null)
					{
						_endpoints.Add(new Tuple<int, int>(num16, num17 + k * num11));
					}
				}
			}
			if (num10 != 0)
			{
				for (int l = 0; l < num2; l++)
				{
					if (Main.tile[(int)((providedInfo.mouse.X - (float)(width / 2)) / 16f) + l, num17] != null)
					{
						_endpoints.Add(new Tuple<int, int>((int)((providedInfo.mouse.X - (float)(width / 2)) / 16f) + l, num17));
					}
				}
			}
			_targets.Clear();
			while (_points.Count > 0)
			{
				Tuple<int, int> tuple = _points[0];
				Tuple<int, int> tuple2 = _endpoints[0];
				Tuple<int, int> tuple3 = Collision.TupleHitLineWall(tuple.Item1, tuple.Item2, tuple2.Item1, tuple2.Item2);
				if (tuple3.Item1 == -1 || tuple3.Item2 == -1)
				{
					_points.Remove(tuple);
					_endpoints.Remove(tuple2);
					continue;
				}
				if (tuple3.Item1 != tuple2.Item1 || tuple3.Item2 != tuple2.Item2)
				{
					_targets.Add(tuple3);
				}
				_ = Main.tile[tuple3.Item1, tuple3.Item2];
				if (Collision.HitWallSubstep(tuple3.Item1, tuple3.Item2))
				{
					_targets.Add(tuple3);
				}
				_points.Remove(tuple);
				_endpoints.Remove(tuple2);
			}
			if (_targets.Count > 0)
			{
				float num4 = -1f;
				Tuple<int, int> tuple4 = null;
				for (int m = 0; m < _targets.Count; m++)
				{
					if (!Main.tile[_targets[m].Item1, _targets[m].Item2].active() || Main.tile[_targets[m].Item1, _targets[m].Item2].type != 26)
					{
						float num5 = Vector2.Distance(new Vector2((float)_targets[m].Item1, (float)_targets[m].Item2) * 16f + Vector2.One * 8f, providedInfo.Center);
						if (num4 == -1f || num5 < num4)
						{
							num4 = num5;
							tuple4 = _targets[m];
						}
					}
				}
				if (tuple4 != null && Collision.InTileBounds(tuple4.Item1, tuple4.Item2, providedInfo.reachableStartX, providedInfo.reachableStartY, providedInfo.reachableEndX, providedInfo.reachableEndY))
				{
					providedInfo.player.poundRelease = false;
					focusedX = tuple4.Item1;
					focusedY = tuple4.Item2;
				}
			}
			_targets.Clear();
			_points.Clear();
			_endpoints.Clear();
		}
		if (providedInfo.item.hammer <= 0 || focusedX != -1 || focusedY != -1)
		{
			return;
		}
		_targets.Clear();
		for (int n = providedInfo.reachableStartX; n <= providedInfo.reachableEndX; n++)
		{
			for (int num6 = providedInfo.reachableStartY; num6 <= providedInfo.reachableEndY; num6++)
			{
				if (Main.tile[n, num6].wall > 0 && Collision.HitWallSubstep(n, num6))
				{
					_targets.Add(new Tuple<int, int>(n, num6));
				}
			}
		}
		if (_targets.Count > 0)
		{
			float num7 = -1f;
			Tuple<int, int> tuple5 = null;
			for (int num8 = 0; num8 < _targets.Count; num8++)
			{
				if (!Main.tile[_targets[num8].Item1, _targets[num8].Item2].active() || Main.tile[_targets[num8].Item1, _targets[num8].Item2].type != 26)
				{
					float num9 = Vector2.Distance(new Vector2((float)_targets[num8].Item1, (float)_targets[num8].Item2) * 16f + Vector2.One * 8f, providedInfo.mouse);
					if (num7 == -1f || num9 < num7)
					{
						num7 = num9;
						tuple5 = _targets[num8];
					}
				}
			}
			if (tuple5 != null && Collision.InTileBounds(tuple5.Item1, tuple5.Item2, providedInfo.reachableStartX, providedInfo.reachableStartY, providedInfo.reachableEndX, providedInfo.reachableEndY))
			{
				providedInfo.player.poundRelease = false;
				focusedX = tuple5.Item1;
				focusedY = tuple5.Item2;
			}
		}
		_targets.Clear();
	}

	private static void Step_MulticolorWrench(SmartCursorUsageInfo providedInfo, ref int focusedX, ref int focusedY)
	{
		//IL_055a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0564: Unknown result type (might be due to invalid IL or missing references)
		//IL_0569: Unknown result type (might be due to invalid IL or missing references)
		//IL_0573: Unknown result type (might be due to invalid IL or missing references)
		//IL_0578: Unknown result type (might be due to invalid IL or missing references)
		//IL_057e: Unknown result type (might be due to invalid IL or missing references)
		if (providedInfo.item.type != 3625 || focusedX != -1 || focusedY != -1)
		{
			return;
		}
		_targets.Clear();
		WiresUI.Settings.MultiToolMode toolMode = WiresUI.Settings.ToolMode;
		WiresUI.Settings.MultiToolMode multiToolMode = (WiresUI.Settings.MultiToolMode)0;
		if (Main.tile[providedInfo.screenTargetX, providedInfo.screenTargetY].wire())
		{
			multiToolMode |= WiresUI.Settings.MultiToolMode.Red;
		}
		if (Main.tile[providedInfo.screenTargetX, providedInfo.screenTargetY].wire2())
		{
			multiToolMode |= WiresUI.Settings.MultiToolMode.Blue;
		}
		if (Main.tile[providedInfo.screenTargetX, providedInfo.screenTargetY].wire3())
		{
			multiToolMode |= WiresUI.Settings.MultiToolMode.Green;
		}
		if (Main.tile[providedInfo.screenTargetX, providedInfo.screenTargetY].wire4())
		{
			multiToolMode |= WiresUI.Settings.MultiToolMode.Yellow;
		}
		toolMode &= ~WiresUI.Settings.MultiToolMode.Cutter;
		bool num4 = toolMode == multiToolMode;
		toolMode = WiresUI.Settings.ToolMode;
		if (!num4)
		{
			bool flag = toolMode.HasFlag(WiresUI.Settings.MultiToolMode.Red);
			bool flag2 = toolMode.HasFlag(WiresUI.Settings.MultiToolMode.Blue);
			bool flag3 = toolMode.HasFlag(WiresUI.Settings.MultiToolMode.Green);
			bool flag4 = toolMode.HasFlag(WiresUI.Settings.MultiToolMode.Yellow);
			bool flag5 = toolMode.HasFlag(WiresUI.Settings.MultiToolMode.Cutter);
			for (int i = providedInfo.reachableStartX; i <= providedInfo.reachableEndX; i++)
			{
				for (int j = providedInfo.reachableStartY; j <= providedInfo.reachableEndY; j++)
				{
					Tile tile = Main.tile[i, j];
					if (flag5)
					{
						if ((tile.wire() && flag) || (tile.wire2() && flag2) || (tile.wire3() && flag3) || (tile.wire4() && flag4))
						{
							_targets.Add(new Tuple<int, int>(i, j));
						}
					}
					else
					{
						if (!(tile.wire() && flag) && !(tile.wire2() && flag2) && !(tile.wire3() && flag3) && !(tile.wire4() && flag4))
						{
							continue;
						}
						if (flag)
						{
							if (!Main.tile[i - 1, j].wire())
							{
								_targets.Add(new Tuple<int, int>(i - 1, j));
							}
							if (!Main.tile[i + 1, j].wire())
							{
								_targets.Add(new Tuple<int, int>(i + 1, j));
							}
							if (!Main.tile[i, j - 1].wire())
							{
								_targets.Add(new Tuple<int, int>(i, j - 1));
							}
							if (!Main.tile[i, j + 1].wire())
							{
								_targets.Add(new Tuple<int, int>(i, j + 1));
							}
						}
						if (flag2)
						{
							if (!Main.tile[i - 1, j].wire2())
							{
								_targets.Add(new Tuple<int, int>(i - 1, j));
							}
							if (!Main.tile[i + 1, j].wire2())
							{
								_targets.Add(new Tuple<int, int>(i + 1, j));
							}
							if (!Main.tile[i, j - 1].wire2())
							{
								_targets.Add(new Tuple<int, int>(i, j - 1));
							}
							if (!Main.tile[i, j + 1].wire2())
							{
								_targets.Add(new Tuple<int, int>(i, j + 1));
							}
						}
						if (flag3)
						{
							if (!Main.tile[i - 1, j].wire3())
							{
								_targets.Add(new Tuple<int, int>(i - 1, j));
							}
							if (!Main.tile[i + 1, j].wire3())
							{
								_targets.Add(new Tuple<int, int>(i + 1, j));
							}
							if (!Main.tile[i, j - 1].wire3())
							{
								_targets.Add(new Tuple<int, int>(i, j - 1));
							}
							if (!Main.tile[i, j + 1].wire3())
							{
								_targets.Add(new Tuple<int, int>(i, j + 1));
							}
						}
						if (flag4)
						{
							if (!Main.tile[i - 1, j].wire4())
							{
								_targets.Add(new Tuple<int, int>(i - 1, j));
							}
							if (!Main.tile[i + 1, j].wire4())
							{
								_targets.Add(new Tuple<int, int>(i + 1, j));
							}
							if (!Main.tile[i, j - 1].wire4())
							{
								_targets.Add(new Tuple<int, int>(i, j - 1));
							}
							if (!Main.tile[i, j + 1].wire4())
							{
								_targets.Add(new Tuple<int, int>(i, j + 1));
							}
						}
					}
				}
			}
		}
		if (_targets.Count > 0)
		{
			float num2 = -1f;
			Tuple<int, int> tuple = _targets[0];
			for (int k = 0; k < _targets.Count; k++)
			{
				float num3 = Vector2.Distance(new Vector2((float)_targets[k].Item1, (float)_targets[k].Item2) * 16f + Vector2.One * 8f, providedInfo.mouse);
				if (num2 == -1f || num3 < num2)
				{
					num2 = num3;
					tuple = _targets[k];
				}
			}
			if (Collision.InTileBounds(tuple.Item1, tuple.Item2, providedInfo.reachableStartX, providedInfo.reachableStartY, providedInfo.reachableEndX, providedInfo.reachableEndY))
			{
				focusedX = tuple.Item1;
				focusedY = tuple.Item2;
			}
		}
		_targets.Clear();
	}

	private static void Step_ColoredWrenches(SmartCursorUsageInfo providedInfo, ref int focusedX, ref int focusedY)
	{
		//IL_050d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0517: Unknown result type (might be due to invalid IL or missing references)
		//IL_051c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0526: Unknown result type (might be due to invalid IL or missing references)
		//IL_052b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0531: Unknown result type (might be due to invalid IL or missing references)
		if ((providedInfo.item.type != 509 && providedInfo.item.type != 850 && providedInfo.item.type != 851 && providedInfo.item.type != 3612) || focusedX != -1 || focusedY != -1)
		{
			return;
		}
		_targets.Clear();
		int num = 0;
		if (providedInfo.item.type == 509)
		{
			num = 1;
		}
		if (providedInfo.item.type == 850)
		{
			num = 2;
		}
		if (providedInfo.item.type == 851)
		{
			num = 3;
		}
		if (providedInfo.item.type == 3612)
		{
			num = 4;
		}
		bool flag = false;
		if (Main.tile[providedInfo.screenTargetX, providedInfo.screenTargetY].wire() && num == 1)
		{
			flag = true;
		}
		if (Main.tile[providedInfo.screenTargetX, providedInfo.screenTargetY].wire2() && num == 2)
		{
			flag = true;
		}
		if (Main.tile[providedInfo.screenTargetX, providedInfo.screenTargetY].wire3() && num == 3)
		{
			flag = true;
		}
		if (Main.tile[providedInfo.screenTargetX, providedInfo.screenTargetY].wire4() && num == 4)
		{
			flag = true;
		}
		if (!flag)
		{
			for (int i = providedInfo.reachableStartX; i <= providedInfo.reachableEndX; i++)
			{
				for (int j = providedInfo.reachableStartY; j <= providedInfo.reachableEndY; j++)
				{
					Tile tile = Main.tile[i, j];
					if ((!tile.wire() || num != 1) && (!tile.wire2() || num != 2) && (!tile.wire3() || num != 3) && (!tile.wire4() || num != 4))
					{
						continue;
					}
					if (num == 1)
					{
						if (!Main.tile[i - 1, j].wire())
						{
							_targets.Add(new Tuple<int, int>(i - 1, j));
						}
						if (!Main.tile[i + 1, j].wire())
						{
							_targets.Add(new Tuple<int, int>(i + 1, j));
						}
						if (!Main.tile[i, j - 1].wire())
						{
							_targets.Add(new Tuple<int, int>(i, j - 1));
						}
						if (!Main.tile[i, j + 1].wire())
						{
							_targets.Add(new Tuple<int, int>(i, j + 1));
						}
					}
					if (num == 2)
					{
						if (!Main.tile[i - 1, j].wire2())
						{
							_targets.Add(new Tuple<int, int>(i - 1, j));
						}
						if (!Main.tile[i + 1, j].wire2())
						{
							_targets.Add(new Tuple<int, int>(i + 1, j));
						}
						if (!Main.tile[i, j - 1].wire2())
						{
							_targets.Add(new Tuple<int, int>(i, j - 1));
						}
						if (!Main.tile[i, j + 1].wire2())
						{
							_targets.Add(new Tuple<int, int>(i, j + 1));
						}
					}
					if (num == 3)
					{
						if (!Main.tile[i - 1, j].wire3())
						{
							_targets.Add(new Tuple<int, int>(i - 1, j));
						}
						if (!Main.tile[i + 1, j].wire3())
						{
							_targets.Add(new Tuple<int, int>(i + 1, j));
						}
						if (!Main.tile[i, j - 1].wire3())
						{
							_targets.Add(new Tuple<int, int>(i, j - 1));
						}
						if (!Main.tile[i, j + 1].wire3())
						{
							_targets.Add(new Tuple<int, int>(i, j + 1));
						}
					}
					if (num == 4)
					{
						if (!Main.tile[i - 1, j].wire4())
						{
							_targets.Add(new Tuple<int, int>(i - 1, j));
						}
						if (!Main.tile[i + 1, j].wire4())
						{
							_targets.Add(new Tuple<int, int>(i + 1, j));
						}
						if (!Main.tile[i, j - 1].wire4())
						{
							_targets.Add(new Tuple<int, int>(i, j - 1));
						}
						if (!Main.tile[i, j + 1].wire4())
						{
							_targets.Add(new Tuple<int, int>(i, j + 1));
						}
					}
				}
			}
		}
		if (_targets.Count > 0)
		{
			float num2 = -1f;
			Tuple<int, int> tuple = _targets[0];
			for (int k = 0; k < _targets.Count; k++)
			{
				float num3 = Vector2.Distance(new Vector2((float)_targets[k].Item1, (float)_targets[k].Item2) * 16f + Vector2.One * 8f, providedInfo.mouse);
				if (num2 == -1f || num3 < num2)
				{
					num2 = num3;
					tuple = _targets[k];
				}
			}
			if (Collision.InTileBounds(tuple.Item1, tuple.Item2, providedInfo.reachableStartX, providedInfo.reachableStartY, providedInfo.reachableEndX, providedInfo.reachableEndY))
			{
				focusedX = tuple.Item1;
				focusedY = tuple.Item2;
			}
		}
		_targets.Clear();
	}

	private static void Step_Acorns(SmartCursorUsageInfo providedInfo, ref int focusedX, ref int focusedY)
	{
		//IL_0558: Unknown result type (might be due to invalid IL or missing references)
		//IL_0562: Unknown result type (might be due to invalid IL or missing references)
		//IL_0567: Unknown result type (might be due to invalid IL or missing references)
		//IL_0571: Unknown result type (might be due to invalid IL or missing references)
		//IL_0576: Unknown result type (might be due to invalid IL or missing references)
		//IL_057c: Unknown result type (might be due to invalid IL or missing references)
		if (providedInfo.item.type != 27 || focusedX != -1 || focusedY != -1 || providedInfo.reachableStartY <= 20)
		{
			return;
		}
		_targets.Clear();
		for (int i = providedInfo.reachableStartX; i <= providedInfo.reachableEndX; i++)
		{
			for (int j = providedInfo.reachableStartY; j <= providedInfo.reachableEndY; j++)
			{
				Tile tile = Main.tile[i, j];
				Tile tile3 = Main.tile[i, j - 1];
				Tile tile4 = Main.tile[i, j + 1];
				Tile tile5 = Main.tile[i - 1, j];
				Tile tile6 = Main.tile[i + 1, j];
				Tile tile7 = Main.tile[i - 2, j];
				Tile tile8 = Main.tile[i + 2, j];
				Tile tile9 = Main.tile[i - 3, j];
				Tile tile10 = Main.tile[i + 3, j];
				if ((tile.active() && !Main.tileCut[tile.type] && !TileID.Sets.BreakableWhenPlacing[tile.type]) || (tile3.active() && !Main.tileCut[tile3.type] && !TileID.Sets.BreakableWhenPlacing[tile3.type]) || (tile5.active() && TileID.Sets.CommonSapling[tile5.type]) || (tile6.active() && TileID.Sets.CommonSapling[tile6.type]) || (tile7.active() && TileID.Sets.CommonSapling[tile7.type]) || (tile8.active() && TileID.Sets.CommonSapling[tile8.type]) || (tile9.active() && TileID.Sets.CommonSapling[tile9.type]) || (tile10.active() && TileID.Sets.CommonSapling[tile10.type]) || !tile4.active() || !WorldGen.SolidTile2(tile4))
				{
					continue;
				}
				bool tileTypeValid = false;
				switch (tile4.type)
				{
				case 60:
					if (WorldGen.EmptyTileCheck(i - 2, i + 2, j - 20, j, 20))
					{
						_targets.Add(new Tuple<int, int>(i, j));
					}
					continue;
				case 2:
				case 23:
				case 53:
				case 109:
				case 112:
				case 116:
				case 147:
				case 199:
				case 234:
				case 477:
				case 492:
				case 633:
				case 661:
				case 662:
					tileTypeValid = true;
					break;
				}
				if (!tileTypeValid)
				{
					tileTypeValid = TileLoader.CanGrowModTree(tile4.type) || TileLoader.CanGrowModPalmTree(tile4.type);
				}
				if (tileTypeValid && tile5.liquid == 0 && tile.liquid == 0 && tile6.liquid == 0 && WorldGen.EmptyTileCheck(i - 2, i + 2, j - 20, j, 20))
				{
					_targets.Add(new Tuple<int, int>(i, j));
				}
			}
		}
		_toRemove.Clear();
		for (int k = 0; k < _targets.Count; k++)
		{
			bool flag = false;
			for (int l = -1; l < 2; l += 2)
			{
				Tile tile2 = Main.tile[_targets[k].Item1 + l, _targets[k].Item2 + 1];
				if (!tile2.active())
				{
					continue;
				}
				if (TileLoader.CanGrowModTree(tile2.type) || TileLoader.CanGrowModPalmTree(tile2.type))
				{
					flag = true;
					continue;
				}
				switch (tile2.type)
				{
				case 2:
				case 23:
				case 53:
				case 60:
				case 109:
				case 112:
				case 116:
				case 147:
				case 199:
				case 234:
				case 477:
				case 492:
				case 633:
				case 661:
				case 662:
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				_toRemove.Add(_targets[k]);
			}
		}
		for (int m = 0; m < _toRemove.Count; m++)
		{
			_targets.Remove(_toRemove[m]);
		}
		_toRemove.Clear();
		if (_targets.Count > 0)
		{
			float num = -1f;
			Tuple<int, int> tuple = _targets[0];
			for (int n = 0; n < _targets.Count; n++)
			{
				float num2 = Vector2.Distance(new Vector2((float)_targets[n].Item1, (float)_targets[n].Item2) * 16f + Vector2.One * 8f, providedInfo.mouse);
				if (num == -1f || num2 < num)
				{
					num = num2;
					tuple = _targets[n];
				}
			}
			if (Collision.InTileBounds(tuple.Item1, tuple.Item2, providedInfo.reachableStartX, providedInfo.reachableStartY, providedInfo.reachableEndX, providedInfo.reachableEndY))
			{
				focusedX = tuple.Item1;
				focusedY = tuple.Item2;
			}
		}
		_targets.Clear();
	}

	private static void Step_GemCorns(SmartCursorUsageInfo providedInfo, ref int focusedX, ref int focusedY)
	{
		//IL_0398: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bc: Unknown result type (might be due to invalid IL or missing references)
		if (!WorldGen.GrowTreeSettings.Profiles.TryGetFromItemId(providedInfo.item.type, out var profile) || focusedX != -1 || focusedY != -1 || providedInfo.reachableStartY <= 20)
		{
			return;
		}
		_targets.Clear();
		for (int i = providedInfo.reachableStartX; i <= providedInfo.reachableEndX; i++)
		{
			for (int j = providedInfo.reachableStartY; j <= providedInfo.reachableEndY; j++)
			{
				Tile tile = Main.tile[i, j];
				Tile tile3 = Main.tile[i, j - 1];
				Tile tile4 = Main.tile[i, j + 1];
				Tile tile5 = Main.tile[i - 1, j];
				Tile tile6 = Main.tile[i + 1, j];
				Tile tile7 = Main.tile[i - 2, j];
				Tile tile8 = Main.tile[i + 2, j];
				Tile tile9 = Main.tile[i - 3, j];
				Tile tile10 = Main.tile[i + 3, j];
				if (profile.GroundTest(tile4.type) && (!tile.active() || Main.tileCut[tile.type] || TileID.Sets.BreakableWhenPlacing[tile.type]) && (!tile3.active() || Main.tileCut[tile3.type] || TileID.Sets.BreakableWhenPlacing[tile3.type]) && (!tile5.active() || !TileID.Sets.CommonSapling[tile5.type]) && (!tile6.active() || !TileID.Sets.CommonSapling[tile6.type]) && (!tile7.active() || !TileID.Sets.CommonSapling[tile7.type]) && (!tile8.active() || !TileID.Sets.CommonSapling[tile8.type]) && (!tile9.active() || !TileID.Sets.CommonSapling[tile9.type]) && (!tile10.active() || !TileID.Sets.CommonSapling[tile10.type]) && tile4.active() && WorldGen.SolidTile2(tile4) && tile5.liquid == 0 && tile.liquid == 0 && tile6.liquid == 0 && WorldGen.EmptyTileCheck(i - 2, i + 2, j - profile.TreeHeightMax, j, profile.SaplingTileType))
				{
					_targets.Add(new Tuple<int, int>(i, j));
				}
			}
		}
		_toRemove.Clear();
		for (int k = 0; k < _targets.Count; k++)
		{
			bool flag = false;
			for (int l = -1; l < 2; l += 2)
			{
				Tile tile2 = Main.tile[_targets[k].Item1 + l, _targets[k].Item2 + 1];
				if (tile2.active() && profile.GroundTest(tile2.type))
				{
					flag = true;
				}
			}
			if (!flag)
			{
				_toRemove.Add(_targets[k]);
			}
		}
		for (int m = 0; m < _toRemove.Count; m++)
		{
			_targets.Remove(_toRemove[m]);
		}
		_toRemove.Clear();
		if (_targets.Count > 0)
		{
			float num = -1f;
			Tuple<int, int> tuple = _targets[0];
			for (int n = 0; n < _targets.Count; n++)
			{
				float num2 = Vector2.Distance(new Vector2((float)_targets[n].Item1, (float)_targets[n].Item2) * 16f + Vector2.One * 8f, providedInfo.mouse);
				if (num == -1f || num2 < num)
				{
					num = num2;
					tuple = _targets[n];
				}
			}
			if (Collision.InTileBounds(tuple.Item1, tuple.Item2, providedInfo.reachableStartX, providedInfo.reachableStartY, providedInfo.reachableEndX, providedInfo.reachableEndY))
			{
				focusedX = tuple.Item1;
				focusedY = tuple.Item2;
			}
		}
		_targets.Clear();
	}

	private static void Step_ForceCursorToAnyMinableThing(SmartCursorUsageInfo providedInfo, ref int fX, ref int fY)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		int reachableStartX = providedInfo.reachableStartX;
		int reachableStartY = providedInfo.reachableStartY;
		int reachableEndX = providedInfo.reachableEndX;
		int reachableEndY = providedInfo.reachableEndY;
		_ = providedInfo.screenTargetX;
		_ = providedInfo.screenTargetY;
		Vector2 mouse = providedInfo.mouse;
		Item item = providedInfo.item;
		if (fX != -1 || fY != -1 || PlayerInput.UsingGamepad)
		{
			return;
		}
		Point val = mouse.ToTileCoordinates();
		int x = val.X;
		int y = val.Y;
		if (Collision.InTileBounds(x, y, reachableStartX, reachableStartY, reachableEndX, reachableEndY))
		{
			Tile tile = Main.tile[x, y];
			bool flag = tile.active() && WorldGen.CanKillTile(x, y) && (!Main.tileSolid[tile.type] || Main.tileSolidTop[tile.type]);
			if (flag && Main.tileAxe[tile.type] && item.axe < 1)
			{
				flag = false;
			}
			if (flag && Main.tileHammer[tile.type] && item.hammer < 1)
			{
				flag = false;
			}
			if (flag && !Main.tileHammer[tile.type] && !Main.tileAxe[tile.type] && item.pick < 1)
			{
				flag = false;
			}
			if (flag)
			{
				fX = x;
				fY = y;
			}
		}
	}

	private static void Step_Pickaxe_MineShinies(SmartCursorUsageInfo providedInfo, ref int fX, ref int fY)
	{
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0229: Unknown result type (might be due to invalid IL or missing references)
		//IL_0233: Unknown result type (might be due to invalid IL or missing references)
		//IL_0238: Unknown result type (might be due to invalid IL or missing references)
		//IL_0242: Unknown result type (might be due to invalid IL or missing references)
		//IL_0247: Unknown result type (might be due to invalid IL or missing references)
		//IL_024c: Unknown result type (might be due to invalid IL or missing references)
		int reachableStartX = providedInfo.reachableStartX;
		int reachableStartY = providedInfo.reachableStartY;
		int reachableEndX = providedInfo.reachableEndX;
		int reachableEndY = providedInfo.reachableEndY;
		_ = providedInfo.screenTargetX;
		_ = providedInfo.screenTargetY;
		Item item = providedInfo.item;
		Vector2 mouse = providedInfo.mouse;
		if (item.pick <= 0 || fX != -1 || fY != -1)
		{
			return;
		}
		_targets.Clear();
		if (item.type != 1333 && item.type != 523)
		{
			_ = item.type;
		}
		int num = 0;
		for (int i = reachableStartX; i <= reachableEndX; i++)
		{
			for (int j = reachableStartY; j <= reachableEndY; j++)
			{
				Tile tile = Main.tile[i, j];
				_ = Main.tile[i - 1, j];
				_ = Main.tile[i + 1, j];
				_ = Main.tile[i, j + 1];
				if (!tile.active())
				{
					continue;
				}
				int num2 = (num2 = TileID.Sets.SmartCursorPickaxePriorityOverride[tile.type]);
				if (num2 > 0)
				{
					if (num < num2)
					{
						num = num2;
					}
					_targets.Add(new Tuple<int, int>(i, j));
				}
			}
		}
		_targets2.Clear();
		foreach (Tuple<int, int> item2 in _targets2)
		{
			Tile tile2 = Main.tile[item2.Item1, item2.Item2];
			if (TileID.Sets.SmartCursorPickaxePriorityOverride[tile2.type] < num)
			{
				_targets2.Add(item2);
			}
		}
		foreach (Tuple<int, int> item3 in _targets2)
		{
			_targets.Remove(item3);
		}
		if (_targets.Count > 0)
		{
			float num3 = -1f;
			Tuple<int, int> tuple = _targets[0];
			for (int k = 0; k < _targets.Count; k++)
			{
				float num4 = Vector2.Distance(new Vector2((float)_targets[k].Item1, (float)_targets[k].Item2) * 16f + Vector2.One * 8f, mouse);
				if (num3 == -1f || num4 < num3)
				{
					num3 = num4;
					tuple = _targets[k];
				}
			}
			if (Collision.InTileBounds(tuple.Item1, tuple.Item2, reachableStartX, reachableStartY, reachableEndX, reachableEndY))
			{
				fX = tuple.Item1;
				fY = tuple.Item2;
			}
		}
		_targets.Clear();
	}

	private static void Step_Pickaxe_MineSolids(Player player, SmartCursorUsageInfo providedInfo, List<Tuple<int, int>> grappleTargets, ref int focusedX, ref int focusedY)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0183: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0259: Unknown result type (might be due to invalid IL or missing references)
		//IL_027e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0630: Unknown result type (might be due to invalid IL or missing references)
		//IL_0631: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_07fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0805: Unknown result type (might be due to invalid IL or missing references)
		//IL_080a: Unknown result type (might be due to invalid IL or missing references)
		//IL_080f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0687: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_06be: Unknown result type (might be due to invalid IL or missing references)
		//IL_0726: Unknown result type (might be due to invalid IL or missing references)
		int width = player.width;
		int height = player.height;
		int direction = player.direction;
		Vector2 center = player.Center;
		Vector2 position = player.position;
		float gravDir = player.gravDir;
		int whoAmI = player.whoAmI;
		if (providedInfo.item.pick <= 0 || focusedX != -1 || focusedY != -1)
		{
			return;
		}
		if (PlayerInput.UsingGamepad)
		{
			Vector2 navigatorDirections = PlayerInput.Triggers.Current.GetNavigatorDirections();
			Vector2 gamepadThumbstickLeft = PlayerInput.GamepadThumbstickLeft;
			Vector2 gamepadThumbstickRight = PlayerInput.GamepadThumbstickRight;
			if (navigatorDirections == Vector2.Zero && ((Vector2)(ref gamepadThumbstickLeft)).Length() < 0.05f && ((Vector2)(ref gamepadThumbstickRight)).Length() < 0.05f)
			{
				providedInfo.mouse = center + new Vector2((float)(direction * 1000), 0f);
			}
		}
		Vector2 vector = providedInfo.mouse - center;
		int num = Math.Sign(vector.X);
		int num12 = Math.Sign(vector.Y);
		if (Math.Abs(vector.X) > Math.Abs(vector.Y) * 3f)
		{
			num12 = 0;
			providedInfo.mouse.Y = center.Y;
		}
		if (Math.Abs(vector.Y) > Math.Abs(vector.X) * 3f)
		{
			num = 0;
			providedInfo.mouse.X = center.X;
		}
		_ = (int)center.X / 16;
		_ = (int)center.Y / 16;
		_points.Clear();
		_endpoints.Clear();
		int num13 = 1;
		if (num12 == -1 && num != 0)
		{
			num13 = -1;
		}
		int num14 = (int)((position.X + (float)(width / 2) + (float)((width / 2 - 1) * num)) / 16f);
		int num15 = (int)(((double)position.Y + 0.1) / 16.0);
		if (num13 == -1)
		{
			num15 = (int)((position.Y + (float)height - 1f) / 16f);
		}
		int num16 = width / 16 + ((width % 16 != 0) ? 1 : 0);
		int num17 = height / 16 + ((height % 16 != 0) ? 1 : 0);
		if (num != 0)
		{
			for (int i = 0; i < num17; i++)
			{
				if (Main.tile[num14, num15 + i * num13] != null)
				{
					_points.Add(new Tuple<int, int>(num14, num15 + i * num13));
				}
			}
		}
		if (num12 != 0)
		{
			for (int j = 0; j < num16; j++)
			{
				if (Main.tile[(int)(position.X / 16f) + j, num15] != null)
				{
					_points.Add(new Tuple<int, int>((int)(position.X / 16f) + j, num15));
				}
			}
		}
		int num18 = (int)((providedInfo.mouse.X + (float)((width / 2 - 1) * num)) / 16f);
		int num19 = (int)(((double)providedInfo.mouse.Y + 0.1 - (double)(height / 2 + 1)) / 16.0);
		if (num13 == -1)
		{
			num19 = (int)((providedInfo.mouse.Y + (float)(height / 2) - 1f) / 16f);
		}
		if (gravDir == -1f && num12 == 0)
		{
			num19++;
		}
		if (num19 < 10)
		{
			num19 = 10;
		}
		if (num19 > Main.maxTilesY - 10)
		{
			num19 = Main.maxTilesY - 10;
		}
		int num2 = width / 16 + ((width % 16 != 0) ? 1 : 0);
		int num3 = height / 16 + ((height % 16 != 0) ? 1 : 0);
		if (WorldGen.InWorld(num18, num19, 40))
		{
			if (num != 0)
			{
				for (int k = 0; k < num3; k++)
				{
					if (Main.tile[num18, num19 + k * num13] != null)
					{
						_endpoints.Add(new Tuple<int, int>(num18, num19 + k * num13));
					}
				}
			}
			if (num12 != 0)
			{
				for (int l = 0; l < num2; l++)
				{
					if (Main.tile[(int)((providedInfo.mouse.X - (float)(width / 2)) / 16f) + l, num19] != null)
					{
						_endpoints.Add(new Tuple<int, int>((int)((providedInfo.mouse.X - (float)(width / 2)) / 16f) + l, num19));
					}
				}
			}
		}
		_targets.Clear();
		while (_points.Count > 0 && _endpoints.Count > 0)
		{
			Tuple<int, int> tuple = _points[0];
			Tuple<int, int> tuple2 = _endpoints[0];
			if (!Collision.TupleHitLine(tuple.Item1, tuple.Item2, tuple2.Item1, tuple2.Item2, num * (int)gravDir, -num12 * (int)gravDir, grappleTargets, out var col))
			{
				_points.Remove(tuple);
				_endpoints.Remove(tuple2);
				continue;
			}
			if (col.Item1 != tuple2.Item1 || col.Item2 != tuple2.Item2)
			{
				_targets.Add(col);
			}
			Tile tile = Main.tile[col.Item1, col.Item2];
			if (!tile.inActive() && tile.active() && Main.tileSolid[tile.type] && !Main.tileSolidTop[tile.type] && !grappleTargets.Contains(col))
			{
				_targets.Add(col);
			}
			_points.Remove(tuple);
			_endpoints.Remove(tuple2);
		}
		_toRemove.Clear();
		for (int m = 0; m < _targets.Count; m++)
		{
			if (!WorldGen.CanKillTile(_targets[m].Item1, _targets[m].Item2))
			{
				_toRemove.Add(_targets[m]);
			}
		}
		for (int n = 0; n < _toRemove.Count; n++)
		{
			_targets.Remove(_toRemove[n]);
		}
		_toRemove.Clear();
		if (_targets.Count > 0)
		{
			float num4 = -1f;
			Tuple<int, int> tuple3 = _targets[0];
			Vector2 value = center;
			if (Main.netMode == 1)
			{
				int num5 = 0;
				int num6 = 0;
				int num7 = 0;
				for (int num8 = 0; num8 < whoAmI; num8++)
				{
					Player player2 = Main.player[num8];
					if (player2.active && !player2.dead && player2.HeldItem.pick > 0 && player2.itemAnimation > 0)
					{
						if (player.Distance(player2.Center) <= 8f)
						{
							num5++;
						}
						if (player.Distance(player2.Center) <= 80f && Math.Abs(player2.Center.Y - center.Y) <= 12f)
						{
							num6++;
						}
					}
				}
				for (int num9 = whoAmI + 1; num9 < 255; num9++)
				{
					Player player3 = Main.player[num9];
					if (player3.active && !player3.dead && player3.HeldItem.pick > 0 && player3.itemAnimation > 0 && player.Distance(player3.Center) <= 8f)
					{
						num7++;
					}
				}
				if (num5 > 0)
				{
					if (num5 % 2 == 1)
					{
						value.X += 12f;
					}
					else
					{
						value.X -= 12f;
					}
					if (num6 % 2 == 1)
					{
						value.Y -= 12f;
					}
				}
				if (num7 > 0 && num5 == 0)
				{
					if (num7 % 2 == 1)
					{
						value.X -= 12f;
					}
					else
					{
						value.X += 12f;
					}
				}
			}
			for (int num10 = 0; num10 < _targets.Count; num10++)
			{
				float num11 = Vector2.Distance(new Vector2((float)_targets[num10].Item1, (float)_targets[num10].Item2) * 16f + Vector2.One * 8f, value);
				if (num4 == -1f || num11 < num4)
				{
					num4 = num11;
					tuple3 = _targets[num10];
				}
			}
			if (Collision.InTileBounds(tuple3.Item1, tuple3.Item2, providedInfo.reachableStartX, providedInfo.reachableStartY, providedInfo.reachableEndX, providedInfo.reachableEndY))
			{
				focusedX = tuple3.Item1;
				focusedY = tuple3.Item2;
			}
		}
		_points.Clear();
		_endpoints.Clear();
		_targets.Clear();
	}

	private static void Step_Axe(SmartCursorUsageInfo providedInfo, ref int fX, ref int fY)
	{
		//IL_056c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0576: Unknown result type (might be due to invalid IL or missing references)
		//IL_057b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0585: Unknown result type (might be due to invalid IL or missing references)
		//IL_058a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0590: Unknown result type (might be due to invalid IL or missing references)
		int reachableStartX = providedInfo.reachableStartX;
		int reachableStartY = providedInfo.reachableStartY;
		int reachableEndX = providedInfo.reachableEndX;
		int reachableEndY = providedInfo.reachableEndY;
		_ = providedInfo.screenTargetX;
		_ = providedInfo.screenTargetY;
		if (providedInfo.item.axe <= 0 || fX != -1 || fY != -1)
		{
			return;
		}
		float num = -1f;
		for (int i = reachableStartX; i <= reachableEndX; i++)
		{
			for (int j = reachableStartY; j <= reachableEndY; j++)
			{
				if (!Main.tile[i, j].active())
				{
					continue;
				}
				Tile tile = Main.tile[i, j];
				if (!Main.tileAxe[tile.type] || TileID.Sets.IgnoreSmartCursorPriorityAxe[tile.type])
				{
					continue;
				}
				int num2 = i;
				int k = j;
				int type = tile.type;
				if (TileID.Sets.IsATreeTrunk[type])
				{
					if (Collision.InTileBounds(num2 + 1, k, reachableStartX, reachableStartY, reachableEndX, reachableEndY))
					{
						if (Main.tile[num2, k].frameY >= 198 && Main.tile[num2, k].frameX == 44)
						{
							num2++;
						}
						if (Main.tile[num2, k].frameX == 66 && Main.tile[num2, k].frameY <= 44)
						{
							num2++;
						}
						if (Main.tile[num2, k].frameX == 44 && Main.tile[num2, k].frameY >= 132 && Main.tile[num2, k].frameY <= 176)
						{
							num2++;
						}
					}
					if (Collision.InTileBounds(num2 - 1, k, reachableStartX, reachableStartY, reachableEndX, reachableEndY))
					{
						if (Main.tile[num2, k].frameY >= 198 && Main.tile[num2, k].frameX == 66)
						{
							num2--;
						}
						if (Main.tile[num2, k].frameX == 88 && Main.tile[num2, k].frameY >= 66 && Main.tile[num2, k].frameY <= 110)
						{
							num2--;
						}
						if (Main.tile[num2, k].frameX == 22 && Main.tile[num2, k].frameY >= 132 && Main.tile[num2, k].frameY <= 176)
						{
							num2--;
						}
					}
					for (; Main.tile[num2, k].active() && Main.tile[num2, k].type == type && Main.tile[num2, k + 1].type == type && Collision.InTileBounds(num2, k + 1, reachableStartX, reachableStartY, reachableEndX, reachableEndY); k++)
					{
					}
				}
				if (tile.type == 80)
				{
					if (Collision.InTileBounds(num2 + 1, k, reachableStartX, reachableStartY, reachableEndX, reachableEndY))
					{
						if (Main.tile[num2, k].frameX == 54)
						{
							num2++;
						}
						if (Main.tile[num2, k].frameX == 108 && Main.tile[num2, k].frameY == 36)
						{
							num2++;
						}
					}
					if (Collision.InTileBounds(num2 - 1, k, reachableStartX, reachableStartY, reachableEndX, reachableEndY))
					{
						if (Main.tile[num2, k].frameX == 36)
						{
							num2--;
						}
						if (Main.tile[num2, k].frameX == 108 && Main.tile[num2, k].frameY == 18)
						{
							num2--;
						}
					}
					for (; Main.tile[num2, k].active() && Main.tile[num2, k].type == 80 && Main.tile[num2, k + 1].type == 80 && Collision.InTileBounds(num2, k + 1, reachableStartX, reachableStartY, reachableEndX, reachableEndY); k++)
					{
					}
				}
				if (tile.type == 323 || tile.type == 72)
				{
					for (; Main.tile[num2, k].active() && ((Main.tile[num2, k].type == 323 && Main.tile[num2, k + 1].type == 323) || (Main.tile[num2, k].type == 72 && Main.tile[num2, k + 1].type == 72)) && Collision.InTileBounds(num2, k + 1, reachableStartX, reachableStartY, reachableEndX, reachableEndY); k++)
					{
					}
				}
				float num3 = Vector2.Distance(new Vector2((float)num2, (float)k) * 16f + Vector2.One * 8f, providedInfo.mouse);
				if (num == -1f || num3 < num)
				{
					num = num3;
					fX = num2;
					fY = k;
				}
			}
		}
	}

	private static void Step_BlocksFilling(SmartCursorUsageInfo providedInfo, ref int fX, ref int fY)
	{
		//IL_0397: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ca: Unknown result type (might be due to invalid IL or missing references)
		if (!Player.SmartCursorSettings.SmartBlocksEnabled)
		{
			return;
		}
		int type = providedInfo.item.type;
		if (type < 0)
		{
			return;
		}
		int reachableStartX = providedInfo.reachableStartX;
		int reachableStartY = providedInfo.reachableStartY;
		int reachableEndX = providedInfo.reachableEndX;
		int reachableEndY = providedInfo.reachableEndY;
		int screenTargetX = providedInfo.screenTargetX;
		int screenTargetY = providedInfo.screenTargetY;
		if (Player.SmartCursorSettings.SmartBlocksEnabled || providedInfo.item.createTile <= -1 || type == 213 || type == 5295 || ItemID.Sets.GrassSeeds[type] || !Main.tileSolid[providedInfo.item.createTile] || Main.tileSolidTop[providedInfo.item.createTile] || Main.tileFrameImportant[providedInfo.item.createTile] || fX != -1 || fY != -1)
		{
			return;
		}
		_targets.Clear();
		bool flag = false;
		if (Main.tile[screenTargetX, screenTargetY].active())
		{
			flag = true;
		}
		if (!Collision.InTileBounds(screenTargetX, screenTargetY, reachableStartX, reachableStartY, reachableEndX, reachableEndY))
		{
			flag = true;
		}
		if (!flag)
		{
			for (int i = reachableStartX; i <= reachableEndX; i++)
			{
				for (int j = reachableStartY; j <= reachableEndY; j++)
				{
					Tile tile = Main.tile[i, j];
					if (!tile.active() || Main.tileCut[tile.type] || TileID.Sets.BreakableWhenPlacing[tile.type])
					{
						int num = 0;
						if (Main.tile[i - 1, j].active() && Main.tileSolid[Main.tile[i - 1, j].type] && !Main.tileSolidTop[Main.tile[i - 1, j].type])
						{
							num++;
						}
						if (Main.tile[i + 1, j].active() && Main.tileSolid[Main.tile[i + 1, j].type] && !Main.tileSolidTop[Main.tile[i + 1, j].type])
						{
							num++;
						}
						if (Main.tile[i, j - 1].active() && Main.tileSolid[Main.tile[i, j - 1].type] && !Main.tileSolidTop[Main.tile[i, j - 1].type])
						{
							num++;
						}
						if (Main.tile[i, j + 1].active() && Main.tileSolid[Main.tile[i, j + 1].type] && !Main.tileSolidTop[Main.tile[i, j + 1].type])
						{
							num++;
						}
						if (num >= 2)
						{
							_targets.Add(new Tuple<int, int>(i, j));
						}
					}
				}
			}
		}
		if (_targets.Count > 0)
		{
			float num2 = -1f;
			float num3 = float.PositiveInfinity;
			Tuple<int, int> tuple = _targets[0];
			for (int k = 0; k < _targets.Count; k++)
			{
				if (Collision.EmptyTile(_targets[k].Item1, _targets[k].Item2, ignoreTiles: true))
				{
					Vector2 vector = new Vector2((float)_targets[k].Item1, (float)_targets[k].Item2) * 16f + Vector2.One * 8f - providedInfo.mouse;
					bool flag2 = false;
					float num4 = Math.Abs(vector.X);
					float num5 = ((Vector2)(ref vector)).Length();
					if (num4 < num3)
					{
						flag2 = true;
					}
					if (num4 == num3 && (num2 == -1f || num5 < num2))
					{
						flag2 = true;
					}
					if (flag2)
					{
						num2 = num5;
						num3 = num4;
						tuple = _targets[k];
					}
				}
			}
			if (Collision.InTileBounds(tuple.Item1, tuple.Item2, reachableStartX, reachableStartY, reachableEndX, reachableEndY) && num2 != -1f)
			{
				fX = tuple.Item1;
				fY = tuple.Item2;
			}
		}
		_targets.Clear();
	}

	private static void Step_Torch(SmartCursorUsageInfo providedInfo, ref int fX, ref int fY)
	{
		//IL_0459: Unknown result type (might be due to invalid IL or missing references)
		//IL_0463: Unknown result type (might be due to invalid IL or missing references)
		//IL_0468: Unknown result type (might be due to invalid IL or missing references)
		//IL_0472: Unknown result type (might be due to invalid IL or missing references)
		//IL_0477: Unknown result type (might be due to invalid IL or missing references)
		//IL_047d: Unknown result type (might be due to invalid IL or missing references)
		int reachableStartX = providedInfo.reachableStartX;
		int reachableStartY = providedInfo.reachableStartY;
		int reachableEndX = providedInfo.reachableEndX;
		int reachableEndY = providedInfo.reachableEndY;
		_ = providedInfo.screenTargetX;
		_ = providedInfo.screenTargetY;
		if (providedInfo.item.createTile < 0 || !TileID.Sets.Torch[providedInfo.item.createTile] || fX != -1 || fY != -1)
		{
			return;
		}
		_targets.Clear();
		bool flag = !ItemID.Sets.WaterTorches[providedInfo.player.BiomeTorchHoldStyle(providedInfo.item.type)];
		for (int i = reachableStartX; i <= reachableEndX; i++)
		{
			for (int j = reachableStartY; j <= reachableEndY; j++)
			{
				Tile tile = Main.tile[i, j];
				Tile tile2 = Main.tile[i - 1, j];
				Tile tile3 = Main.tile[i + 1, j];
				Tile tile4 = Main.tile[i, j + 1];
				if (tile.active() && !TileID.Sets.BreakableWhenPlacing[tile.type] && (!Main.tileCut[tile.type] || tile.type == 82 || tile.type == 83))
				{
					continue;
				}
				bool flag2 = false;
				for (int k = i - 8; k <= i + 8; k++)
				{
					for (int l = j - 8; l <= j + 8; l++)
					{
						if (Main.tile[k, l] != null && TileID.Sets.Torch[Main.tile[k, l].type])
						{
							flag2 = true;
							break;
						}
					}
					if (flag2)
					{
						break;
					}
				}
				if (!flag2 && (!flag || tile.liquid <= 0) && (tile.wall > 0 || (tile2.active() && (tile2.slope() == 0 || tile2.slope() % 2 != 1) && ((Main.tileSolid[tile2.type] && !Main.tileNoAttach[tile2.type] && !Main.tileSolidTop[tile2.type] && !TileID.Sets.NotReallySolid[tile2.type]) || TileID.Sets.IsBeam[tile2.type] || (WorldGen.IsTreeType(tile2.type) && WorldGen.IsTreeType(Main.tile[i - 1, j - 1].type) && WorldGen.IsTreeType(Main.tile[i - 1, j + 1].type)))) || (tile3.active() && (tile3.slope() == 0 || tile3.slope() % 2 != 0) && ((Main.tileSolid[tile3.type] && !Main.tileNoAttach[tile3.type] && !Main.tileSolidTop[tile3.type] && !TileID.Sets.NotReallySolid[tile3.type]) || TileID.Sets.IsBeam[tile3.type] || (WorldGen.IsTreeType(tile3.type) && WorldGen.IsTreeType(Main.tile[i + 1, j - 1].type) && WorldGen.IsTreeType(Main.tile[i + 1, j + 1].type)))) || (tile4.active() && Main.tileSolid[tile4.type] && !Main.tileNoAttach[tile4.type] && (!Main.tileSolidTop[tile4.type] || (TileID.Sets.Platforms[tile4.type] && tile4.slope() == 0)) && !TileID.Sets.NotReallySolid[tile4.type] && !tile4.halfBrick() && tile4.slope() == 0)) && !TileID.Sets.Torch[tile.type])
				{
					_targets.Add(new Tuple<int, int>(i, j));
				}
			}
		}
		if (_targets.Count > 0)
		{
			float num = -1f;
			Tuple<int, int> tuple = _targets[0];
			for (int m = 0; m < _targets.Count; m++)
			{
				float num2 = Vector2.Distance(new Vector2((float)_targets[m].Item1, (float)_targets[m].Item2) * 16f + Vector2.One * 8f, providedInfo.mouse);
				if (num == -1f || num2 < num)
				{
					num = num2;
					tuple = _targets[m];
				}
			}
			if (Collision.InTileBounds(tuple.Item1, tuple.Item2, reachableStartX, reachableStartY, reachableEndX, reachableEndY))
			{
				fX = tuple.Item1;
				fY = tuple.Item2;
			}
		}
		_targets.Clear();
	}

	private static void Step_LawnMower(SmartCursorUsageInfo providedInfo, ref int fX, ref int fY)
	{
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		int reachableStartX = providedInfo.reachableStartX;
		int reachableStartY = providedInfo.reachableStartY;
		int reachableEndX = providedInfo.reachableEndX;
		int reachableEndY = providedInfo.reachableEndY;
		_ = providedInfo.screenTargetX;
		_ = providedInfo.screenTargetY;
		if (providedInfo.item.type != 4049 || fX != -1 || fY != -1)
		{
			return;
		}
		_targets.Clear();
		for (int i = reachableStartX; i <= reachableEndX; i++)
		{
			for (int j = reachableStartY; j <= reachableEndY; j++)
			{
				Tile tile = Main.tile[i, j];
				if (tile.active() && (tile.type == 2 || tile.type == 109))
				{
					_targets.Add(new Tuple<int, int>(i, j));
				}
			}
		}
		if (_targets.Count > 0)
		{
			float num = -1f;
			Tuple<int, int> tuple = _targets[0];
			for (int k = 0; k < _targets.Count; k++)
			{
				float num2 = Vector2.Distance(new Vector2((float)_targets[k].Item1, (float)_targets[k].Item2) * 16f + Vector2.One * 8f, providedInfo.mouse);
				if (num == -1f || num2 < num)
				{
					num = num2;
					tuple = _targets[k];
				}
			}
			if (Collision.InTileBounds(tuple.Item1, tuple.Item2, reachableStartX, reachableStartY, reachableEndX, reachableEndY))
			{
				fX = tuple.Item1;
				fY = tuple.Item2;
			}
		}
		_targets.Clear();
	}
}
