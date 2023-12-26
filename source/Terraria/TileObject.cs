using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ObjectData;

namespace Terraria;

public struct TileObject
{
	public int xCoord;

	public int yCoord;

	public int type;

	public int style;

	public int alternate;

	public int random;

	public static TileObject Empty = default(TileObject);

	public static TileObjectPreviewData objectPreview = new TileObjectPreviewData();

	public static bool Place(TileObject toBePlaced)
	{
		TileObjectData tileData = TileObjectData.GetTileData(toBePlaced.type, toBePlaced.style, toBePlaced.alternate);
		if (tileData == null)
		{
			return false;
		}
		if (tileData.HookPlaceOverride.hook != null)
		{
			int arg;
			int arg2;
			if (tileData.HookPlaceOverride.processedCoordinates)
			{
				arg = toBePlaced.xCoord;
				arg2 = toBePlaced.yCoord;
			}
			else
			{
				arg = toBePlaced.xCoord + tileData.Origin.X;
				arg2 = toBePlaced.yCoord + tileData.Origin.Y;
			}
			if (tileData.HookPlaceOverride.hook(arg, arg2, toBePlaced.type, toBePlaced.style, 1, toBePlaced.alternate) == tileData.HookPlaceOverride.badReturn)
			{
				return false;
			}
		}
		else
		{
			ushort num = (ushort)toBePlaced.type;
			int num8 = 0;
			int num9 = 0;
			int num10 = tileData.CalculatePlacementStyle(toBePlaced.style, toBePlaced.alternate, toBePlaced.random);
			int num11 = 0;
			if (tileData.StyleWrapLimit > 0)
			{
				num11 = num10 / tileData.StyleWrapLimit * tileData.StyleLineSkip;
				num10 %= tileData.StyleWrapLimit;
			}
			if (tileData.StyleHorizontal)
			{
				num8 = tileData.CoordinateFullWidth * num10;
				num9 = tileData.CoordinateFullHeight * num11;
			}
			else
			{
				num8 = tileData.CoordinateFullWidth * num11;
				num9 = tileData.CoordinateFullHeight * num10;
			}
			int num12 = toBePlaced.xCoord;
			int num13 = toBePlaced.yCoord;
			for (int i = 0; i < tileData.Width; i++)
			{
				for (int j = 0; j < tileData.Height; j++)
				{
					Tile tileSafely = Framing.GetTileSafely(num12 + i, num13 + j);
					if (tileSafely.active() && tileSafely.type != 484 && (Main.tileCut[tileSafely.type] || TileID.Sets.BreakableWhenPlacing[tileSafely.type]))
					{
						WorldGen.KillTile(num12 + i, num13 + j);
						if (!Main.tile[num12 + i, num13 + j].active() && Main.netMode != 0)
						{
							NetMessage.SendData(17, -1, -1, null, 0, num12 + i, num13 + j);
						}
					}
				}
			}
			for (int k = 0; k < tileData.Width; k++)
			{
				int num14 = num8 + k * (tileData.CoordinateWidth + tileData.CoordinatePadding);
				int num15 = num9;
				for (int l = 0; l < tileData.Height; l++)
				{
					Tile tileSafely2 = Framing.GetTileSafely(num12 + k, num13 + l);
					if (!tileSafely2.active())
					{
						tileSafely2.active(active: true);
						tileSafely2.frameX = (short)num14;
						tileSafely2.frameY = (short)num15;
						tileSafely2.type = num;
					}
					num15 += tileData.CoordinateHeights[l] + tileData.CoordinatePadding;
				}
			}
		}
		if (tileData.FlattenAnchors)
		{
			AnchorData anchorBottom = tileData.AnchorBottom;
			if (anchorBottom.tileCount != 0 && (anchorBottom.type & AnchorType.SolidTile) == AnchorType.SolidTile)
			{
				int num2 = toBePlaced.xCoord + anchorBottom.checkStart;
				int j2 = toBePlaced.yCoord + tileData.Height;
				for (int m = 0; m < anchorBottom.tileCount; m++)
				{
					Tile tileSafely3 = Framing.GetTileSafely(num2 + m, j2);
					if (Main.tileSolid[tileSafely3.type] && !Main.tileSolidTop[tileSafely3.type] && tileSafely3.blockType() != 0)
					{
						WorldGen.SlopeTile(num2 + m, j2);
					}
				}
			}
			anchorBottom = tileData.AnchorTop;
			if (anchorBottom.tileCount != 0 && (anchorBottom.type & AnchorType.SolidTile) == AnchorType.SolidTile)
			{
				int num3 = toBePlaced.xCoord + anchorBottom.checkStart;
				int j3 = toBePlaced.yCoord - 1;
				for (int n = 0; n < anchorBottom.tileCount; n++)
				{
					Tile tileSafely4 = Framing.GetTileSafely(num3 + n, j3);
					if (Main.tileSolid[tileSafely4.type] && !Main.tileSolidTop[tileSafely4.type] && tileSafely4.blockType() != 0)
					{
						WorldGen.SlopeTile(num3 + n, j3);
					}
				}
			}
			anchorBottom = tileData.AnchorRight;
			if (anchorBottom.tileCount != 0 && (anchorBottom.type & AnchorType.SolidTile) == AnchorType.SolidTile)
			{
				int i2 = toBePlaced.xCoord + tileData.Width;
				int num4 = toBePlaced.yCoord + anchorBottom.checkStart;
				for (int num5 = 0; num5 < anchorBottom.tileCount; num5++)
				{
					Tile tileSafely5 = Framing.GetTileSafely(i2, num4 + num5);
					if (Main.tileSolid[tileSafely5.type] && !Main.tileSolidTop[tileSafely5.type] && tileSafely5.blockType() != 0)
					{
						WorldGen.SlopeTile(i2, num4 + num5);
					}
				}
			}
			anchorBottom = tileData.AnchorLeft;
			if (anchorBottom.tileCount != 0 && (anchorBottom.type & AnchorType.SolidTile) == AnchorType.SolidTile)
			{
				int i3 = toBePlaced.xCoord - 1;
				int num6 = toBePlaced.yCoord + anchorBottom.checkStart;
				for (int num7 = 0; num7 < anchorBottom.tileCount; num7++)
				{
					Tile tileSafely6 = Framing.GetTileSafely(i3, num6 + num7);
					if (Main.tileSolid[tileSafely6.type] && !Main.tileSolidTop[tileSafely6.type] && tileSafely6.blockType() != 0)
					{
						WorldGen.SlopeTile(i3, num6 + num7);
					}
				}
			}
		}
		return true;
	}

	public static bool CanPlace(int x, int y, int type, int style, int dir, out TileObject objectData, bool onlyCheck = false, int? forcedRandom = null, bool checkStay = false)
	{
		//IL_0170: Unknown result type (might be due to invalid IL or missing references)
		//IL_0226: Unknown result type (might be due to invalid IL or missing references)
		//IL_022d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_025a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_026c: Unknown result type (might be due to invalid IL or missing references)
		//IL_027a: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0329: Unknown result type (might be due to invalid IL or missing references)
		//IL_0412: Unknown result type (might be due to invalid IL or missing references)
		//IL_033b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0349: Unknown result type (might be due to invalid IL or missing references)
		//IL_0424: Unknown result type (might be due to invalid IL or missing references)
		//IL_0432: Unknown result type (might be due to invalid IL or missing references)
		//IL_038b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0392: Unknown result type (might be due to invalid IL or missing references)
		//IL_0474: Unknown result type (might be due to invalid IL or missing references)
		//IL_047b: Unknown result type (might be due to invalid IL or missing references)
		TileObjectData tileData = TileObjectData.GetTileData(type, style);
		objectData = Empty;
		if (tileData == null)
		{
			return false;
		}
		int num = x - tileData.Origin.X;
		int num12 = y - tileData.Origin.Y;
		if (num < 0 || num + tileData.Width >= Main.maxTilesX || num12 < 0 || num12 + tileData.Height >= Main.maxTilesY)
		{
			return false;
		}
		bool flag = tileData.RandomStyleRange > 0;
		if (TileObjectPreviewData.placementCache == null)
		{
			TileObjectPreviewData.placementCache = new TileObjectPreviewData();
		}
		TileObjectPreviewData.placementCache.Reset();
		int num24 = 0;
		if (tileData.AlternatesCount != 0)
		{
			num24 = tileData.AlternatesCount;
		}
		float num35 = -1f;
		float num43 = -1f;
		int num44 = 0;
		TileObjectData tileObjectData = null;
		int num45 = -1;
		Rectangle rectangle = default(Rectangle);
		while (num45 < num24)
		{
			num45++;
			TileObjectData tileData2 = TileObjectData.GetTileData(type, style, num45);
			if (tileData2.Direction != 0 && ((tileData2.Direction == TileObjectDirection.PlaceLeft && dir == 1) || (tileData2.Direction == TileObjectDirection.PlaceRight && dir == -1)))
			{
				continue;
			}
			int num46 = x - tileData2.Origin.X;
			int num47 = y - tileData2.Origin.Y;
			if (num46 < 5 || num46 + tileData2.Width > Main.maxTilesX - 5 || num47 < 5 || num47 + tileData2.Height > Main.maxTilesY - 5)
			{
				return false;
			}
			((Rectangle)(ref rectangle))._002Ector(0, 0, tileData2.Width, tileData2.Height);
			int num2 = 0;
			int num3 = 0;
			if (tileData2.AnchorTop.tileCount != 0)
			{
				if (rectangle.Y == 0)
				{
					rectangle.Y = -1;
					rectangle.Height++;
					num3++;
				}
				int checkStart = tileData2.AnchorTop.checkStart;
				if (checkStart < rectangle.X)
				{
					rectangle.Width += rectangle.X - checkStart;
					num2 += rectangle.X - checkStart;
					rectangle.X = checkStart;
				}
				int num4 = checkStart + tileData2.AnchorTop.tileCount - 1;
				int num5 = rectangle.X + rectangle.Width - 1;
				if (num4 > num5)
				{
					rectangle.Width += num4 - num5;
				}
			}
			if (tileData2.AnchorBottom.tileCount != 0)
			{
				if (rectangle.Y + rectangle.Height == tileData2.Height)
				{
					rectangle.Height++;
				}
				int checkStart2 = tileData2.AnchorBottom.checkStart;
				if (checkStart2 < rectangle.X)
				{
					rectangle.Width += rectangle.X - checkStart2;
					num2 += rectangle.X - checkStart2;
					rectangle.X = checkStart2;
				}
				int num6 = checkStart2 + tileData2.AnchorBottom.tileCount - 1;
				int num7 = rectangle.X + rectangle.Width - 1;
				if (num6 > num7)
				{
					rectangle.Width += num6 - num7;
				}
			}
			if (tileData2.AnchorLeft.tileCount != 0)
			{
				if (rectangle.X == 0)
				{
					rectangle.X = -1;
					rectangle.Width++;
					num2++;
				}
				int num8 = tileData2.AnchorLeft.checkStart;
				if ((tileData2.AnchorLeft.type & AnchorType.Tree) == AnchorType.Tree)
				{
					num8--;
				}
				if (num8 < rectangle.Y)
				{
					rectangle.Width += rectangle.Y - num8;
					num3 += rectangle.Y - num8;
					rectangle.Y = num8;
				}
				int num9 = num8 + tileData2.AnchorLeft.tileCount - 1;
				if ((tileData2.AnchorLeft.type & AnchorType.Tree) == AnchorType.Tree)
				{
					num9 += 2;
				}
				int num10 = rectangle.Y + rectangle.Height - 1;
				if (num9 > num10)
				{
					rectangle.Height += num9 - num10;
				}
			}
			if (tileData2.AnchorRight.tileCount != 0)
			{
				if (rectangle.X + rectangle.Width == tileData2.Width)
				{
					rectangle.Width++;
				}
				int num11 = tileData2.AnchorLeft.checkStart;
				if ((tileData2.AnchorRight.type & AnchorType.Tree) == AnchorType.Tree)
				{
					num11--;
				}
				if (num11 < rectangle.Y)
				{
					rectangle.Width += rectangle.Y - num11;
					num3 += rectangle.Y - num11;
					rectangle.Y = num11;
				}
				int num13 = num11 + tileData2.AnchorRight.tileCount - 1;
				if ((tileData2.AnchorRight.type & AnchorType.Tree) == AnchorType.Tree)
				{
					num13 += 2;
				}
				int num14 = rectangle.Y + rectangle.Height - 1;
				if (num13 > num14)
				{
					rectangle.Height += num13 - num14;
				}
			}
			if (onlyCheck)
			{
				objectPreview.Reset();
				objectPreview.Active = true;
				objectPreview.Type = (ushort)type;
				objectPreview.Style = (short)style;
				objectPreview.Alternate = num45;
				objectPreview.Size = new Point16(rectangle.Width, rectangle.Height);
				objectPreview.ObjectStart = new Point16(num2, num3);
				objectPreview.Coordinates = new Point16(num46 - num2, num47 - num3);
			}
			float num15 = 0f;
			float num16 = tileData2.Width * tileData2.Height;
			float num17 = 0f;
			float num18 = 0f;
			for (int i = 0; i < tileData2.Width; i++)
			{
				for (int j = 0; j < tileData2.Height; j++)
				{
					Tile tileSafely5 = Framing.GetTileSafely(num46 + i, num47 + j);
					bool flag3 = !tileData2.LiquidPlace(tileSafely5, checkStay);
					bool flag4 = false;
					if (tileData2.AnchorWall)
					{
						num18 += 1f;
						if (!tileData2.isValidWallAnchor(tileSafely5.wall))
						{
							flag4 = true;
						}
						else
						{
							num17 += 1f;
						}
					}
					bool flag5 = false;
					if (tileSafely5.active() && (!Main.tileCut[tileSafely5.type] || tileSafely5.type == 484 || tileSafely5.type == 654) && !TileID.Sets.BreakableWhenPlacing[tileSafely5.type] && !checkStay)
					{
						flag5 = true;
					}
					if (flag5 || flag3 || flag4)
					{
						if (onlyCheck)
						{
							objectPreview[i + num2, j + num3] = 2;
						}
						continue;
					}
					if (onlyCheck)
					{
						objectPreview[i + num2, j + num3] = 1;
					}
					num15 += 1f;
				}
			}
			AnchorData anchorBottom = tileData2.AnchorBottom;
			if (anchorBottom.tileCount != 0)
			{
				num18 += (float)anchorBottom.tileCount;
				int height = tileData2.Height;
				for (int k = 0; k < anchorBottom.tileCount; k++)
				{
					int num19 = anchorBottom.checkStart + k;
					Tile tileSafely4 = Framing.GetTileSafely(num46 + num19, num47 + height);
					bool flag6 = false;
					if (tileSafely4.nactive())
					{
						if ((anchorBottom.type & AnchorType.SolidTile) == AnchorType.SolidTile && Main.tileSolid[tileSafely4.type] && !Main.tileSolidTop[tileSafely4.type] && !Main.tileNoAttach[tileSafely4.type] && (tileData2.FlattenAnchors || tileSafely4.blockType() == 0))
						{
							flag6 = tileData2.isValidTileAnchor(tileSafely4.type);
						}
						if (!flag6 && ((anchorBottom.type & AnchorType.SolidWithTop) == AnchorType.SolidWithTop || (anchorBottom.type & AnchorType.Table) == AnchorType.Table))
						{
							if (TileID.Sets.Platforms[tileSafely4.type])
							{
								_ = tileSafely4.frameX / TileObjectData.PlatformFrameWidth();
								if (!tileSafely4.halfBrick() && WorldGen.PlatformProperTopFrame(tileSafely4.frameX))
								{
									flag6 = true;
								}
							}
							else if (Main.tileSolid[tileSafely4.type] && Main.tileSolidTop[tileSafely4.type])
							{
								flag6 = true;
							}
						}
						if (!flag6 && (anchorBottom.type & AnchorType.Table) == AnchorType.Table && !TileID.Sets.Platforms[tileSafely4.type] && Main.tileTable[tileSafely4.type] && tileSafely4.blockType() == 0)
						{
							flag6 = true;
						}
						if (!flag6 && (anchorBottom.type & AnchorType.SolidSide) == AnchorType.SolidSide && Main.tileSolid[tileSafely4.type] && !Main.tileSolidTop[tileSafely4.type] && (uint)(tileSafely4.blockType() - 4) <= 1u)
						{
							flag6 = tileData2.isValidTileAnchor(tileSafely4.type);
						}
						if (!flag6 && (anchorBottom.type & AnchorType.AlternateTile) == AnchorType.AlternateTile && tileData2.isValidAlternateAnchor(tileSafely4.type))
						{
							flag6 = true;
						}
					}
					else if (!flag6 && (anchorBottom.type & AnchorType.EmptyTile) == AnchorType.EmptyTile)
					{
						flag6 = true;
					}
					if (!flag6)
					{
						if (onlyCheck)
						{
							objectPreview[num19 + num2, height + num3] = 2;
						}
						continue;
					}
					if (onlyCheck)
					{
						objectPreview[num19 + num2, height + num3] = 1;
					}
					num17 += 1f;
				}
			}
			anchorBottom = tileData2.AnchorTop;
			if (anchorBottom.tileCount != 0)
			{
				num18 += (float)anchorBottom.tileCount;
				int num22 = -1;
				for (int l = 0; l < anchorBottom.tileCount; l++)
				{
					int num23 = anchorBottom.checkStart + l;
					Tile tileSafely3 = Framing.GetTileSafely(num46 + num23, num47 + num22);
					bool flag7 = false;
					if (tileSafely3.nactive())
					{
						if (Main.tileSolid[tileSafely3.type] && !Main.tileSolidTop[tileSafely3.type] && !Main.tileNoAttach[tileSafely3.type] && (tileData2.FlattenAnchors || tileSafely3.blockType() == 0))
						{
							flag7 = tileData2.isValidTileAnchor(tileSafely3.type);
						}
						if (!flag7 && (anchorBottom.type & AnchorType.SolidBottom) == AnchorType.SolidBottom && ((Main.tileSolid[tileSafely3.type] && (!Main.tileSolidTop[tileSafely3.type] || (TileID.Sets.Platforms[tileSafely3.type] && (tileSafely3.halfBrick() || tileSafely3.topSlope())))) || tileSafely3.halfBrick() || tileSafely3.topSlope()) && !TileID.Sets.NotReallySolid[tileSafely3.type] && !tileSafely3.bottomSlope())
						{
							flag7 = tileData2.isValidTileAnchor(tileSafely3.type);
						}
						if (!flag7 && (anchorBottom.type & AnchorType.Platform) == AnchorType.Platform && TileID.Sets.Platforms[tileSafely3.type])
						{
							flag7 = tileData2.isValidTileAnchor(tileSafely3.type);
						}
						if (!flag7 && (anchorBottom.type & AnchorType.PlatformNonHammered) == AnchorType.PlatformNonHammered && TileID.Sets.Platforms[tileSafely3.type] && tileSafely3.slope() == 0 && !tileSafely3.halfBrick())
						{
							flag7 = tileData2.isValidTileAnchor(tileSafely3.type);
						}
						if (!flag7 && (anchorBottom.type & AnchorType.PlanterBox) == AnchorType.PlanterBox && tileSafely3.type == 380)
						{
							flag7 = tileData2.isValidTileAnchor(tileSafely3.type);
						}
						if (!flag7 && (anchorBottom.type & AnchorType.SolidSide) == AnchorType.SolidSide && Main.tileSolid[tileSafely3.type] && !Main.tileSolidTop[tileSafely3.type] && (uint)(tileSafely3.blockType() - 2) <= 1u)
						{
							flag7 = tileData2.isValidTileAnchor(tileSafely3.type);
						}
						if (!flag7 && (anchorBottom.type & AnchorType.AlternateTile) == AnchorType.AlternateTile && tileData2.isValidAlternateAnchor(tileSafely3.type))
						{
							flag7 = true;
						}
					}
					else if (!flag7 && (anchorBottom.type & AnchorType.EmptyTile) == AnchorType.EmptyTile)
					{
						flag7 = true;
					}
					if (!flag7)
					{
						if (onlyCheck)
						{
							objectPreview[num23 + num2, num22 + num3] = 2;
						}
						continue;
					}
					if (onlyCheck)
					{
						objectPreview[num23 + num2, num22 + num3] = 1;
					}
					num17 += 1f;
				}
			}
			anchorBottom = tileData2.AnchorRight;
			if (anchorBottom.tileCount != 0)
			{
				num18 += (float)anchorBottom.tileCount;
				int width = tileData2.Width;
				for (int m = 0; m < anchorBottom.tileCount; m++)
				{
					int num25 = anchorBottom.checkStart + m;
					Tile tileSafely2 = Framing.GetTileSafely(num46 + width, num47 + num25);
					bool flag8 = false;
					if (tileSafely2.nactive())
					{
						if (Main.tileSolid[tileSafely2.type] && !Main.tileSolidTop[tileSafely2.type] && !Main.tileNoAttach[tileSafely2.type] && (tileData2.FlattenAnchors || tileSafely2.blockType() == 0))
						{
							flag8 = tileData2.isValidTileAnchor(tileSafely2.type);
						}
						if (!flag8 && (anchorBottom.type & AnchorType.SolidSide) == AnchorType.SolidSide && Main.tileSolid[tileSafely2.type] && !Main.tileSolidTop[tileSafely2.type])
						{
							int num21 = tileSafely2.blockType();
							if (num21 == 2 || num21 == 4)
							{
								flag8 = tileData2.isValidTileAnchor(tileSafely2.type);
							}
						}
						if (!flag8 && (anchorBottom.type & AnchorType.Tree) == AnchorType.Tree && TileID.Sets.IsATreeTrunk[tileSafely2.type])
						{
							flag8 = true;
							if (m == 0)
							{
								num18 += 1f;
								Tile tileSafely6 = Framing.GetTileSafely(num46 + width, num47 + num25 - 1);
								if (tileSafely6.nactive() && TileID.Sets.IsATreeTrunk[tileSafely6.type])
								{
									num17 += 1f;
									if (onlyCheck)
									{
										objectPreview[width + num2, num25 + num3 - 1] = 1;
									}
								}
								else if (onlyCheck)
								{
									objectPreview[width + num2, num25 + num3 - 1] = 2;
								}
							}
							if (m == anchorBottom.tileCount - 1)
							{
								num18 += 1f;
								Tile tileSafely7 = Framing.GetTileSafely(num46 + width, num47 + num25 + 1);
								if (tileSafely7.nactive() && TileID.Sets.IsATreeTrunk[tileSafely7.type])
								{
									num17 += 1f;
									if (onlyCheck)
									{
										objectPreview[width + num2, num25 + num3 + 1] = 1;
									}
								}
								else if (onlyCheck)
								{
									objectPreview[width + num2, num25 + num3 + 1] = 2;
								}
							}
						}
						if (!flag8 && (anchorBottom.type & AnchorType.AlternateTile) == AnchorType.AlternateTile && tileData2.isValidAlternateAnchor(tileSafely2.type))
						{
							flag8 = true;
						}
					}
					else if (!flag8 && (anchorBottom.type & AnchorType.EmptyTile) == AnchorType.EmptyTile)
					{
						flag8 = true;
					}
					if (!flag8)
					{
						if (onlyCheck)
						{
							objectPreview[width + num2, num25 + num3] = 2;
						}
						continue;
					}
					if (onlyCheck)
					{
						objectPreview[width + num2, num25 + num3] = 1;
					}
					num17 += 1f;
				}
			}
			anchorBottom = tileData2.AnchorLeft;
			if (anchorBottom.tileCount != 0)
			{
				num18 += (float)anchorBottom.tileCount;
				int num26 = -1;
				for (int n = 0; n < anchorBottom.tileCount; n++)
				{
					int num27 = anchorBottom.checkStart + n;
					Tile tileSafely = Framing.GetTileSafely(num46 + num26, num47 + num27);
					bool flag9 = false;
					if (tileSafely.nactive())
					{
						if (Main.tileSolid[tileSafely.type] && !Main.tileSolidTop[tileSafely.type] && !Main.tileNoAttach[tileSafely.type] && (tileData2.FlattenAnchors || tileSafely.blockType() == 0))
						{
							flag9 = tileData2.isValidTileAnchor(tileSafely.type);
						}
						if (!flag9 && (anchorBottom.type & AnchorType.SolidSide) == AnchorType.SolidSide && Main.tileSolid[tileSafely.type] && !Main.tileSolidTop[tileSafely.type])
						{
							int num20 = tileSafely.blockType();
							if (num20 == 3 || num20 == 5)
							{
								flag9 = tileData2.isValidTileAnchor(tileSafely.type);
							}
						}
						if (!flag9 && (anchorBottom.type & AnchorType.Tree) == AnchorType.Tree && TileID.Sets.IsATreeTrunk[tileSafely.type])
						{
							flag9 = true;
							if (n == 0)
							{
								num18 += 1f;
								Tile tileSafely8 = Framing.GetTileSafely(num46 + num26, num47 + num27 - 1);
								if (tileSafely8.nactive() && TileID.Sets.IsATreeTrunk[tileSafely8.type])
								{
									num17 += 1f;
									if (onlyCheck)
									{
										objectPreview[num26 + num2, num27 + num3 - 1] = 1;
									}
								}
								else if (onlyCheck)
								{
									objectPreview[num26 + num2, num27 + num3 - 1] = 2;
								}
							}
							if (n == anchorBottom.tileCount - 1)
							{
								num18 += 1f;
								Tile tileSafely9 = Framing.GetTileSafely(num46 + num26, num47 + num27 + 1);
								if (tileSafely9.nactive() && TileID.Sets.IsATreeTrunk[tileSafely9.type])
								{
									num17 += 1f;
									if (onlyCheck)
									{
										objectPreview[num26 + num2, num27 + num3 + 1] = 1;
									}
								}
								else if (onlyCheck)
								{
									objectPreview[num26 + num2, num27 + num3 + 1] = 2;
								}
							}
						}
						if (!flag9 && (anchorBottom.type & AnchorType.AlternateTile) == AnchorType.AlternateTile && tileData2.isValidAlternateAnchor(tileSafely.type))
						{
							flag9 = true;
						}
					}
					else if (!flag9 && (anchorBottom.type & AnchorType.EmptyTile) == AnchorType.EmptyTile)
					{
						flag9 = true;
					}
					if (!flag9)
					{
						if (onlyCheck)
						{
							objectPreview[num26 + num2, num27 + num3] = 2;
						}
						continue;
					}
					if (onlyCheck)
					{
						objectPreview[num26 + num2, num27 + num3] = 1;
					}
					num17 += 1f;
				}
			}
			if (tileData2.HookCheckIfCanPlace.hook != null)
			{
				if (tileData2.HookCheckIfCanPlace.processedCoordinates)
				{
					_ = tileData2.Origin;
					_ = tileData2.Origin;
				}
				if (tileData2.HookCheckIfCanPlace.hook(x, y, type, style, dir, num45) == tileData2.HookCheckIfCanPlace.badReturn && tileData2.HookCheckIfCanPlace.badResponse == 0)
				{
					num17 = 0f;
					num15 = 0f;
					objectPreview.AllInvalid();
				}
			}
			float num28 = num17 / num18;
			if (num18 == 0f)
			{
				num28 = 1f;
			}
			float num29 = num15 / num16;
			if (num29 == 1f && num18 == 0f)
			{
				num16 = 1f;
				num18 = 1f;
				num28 = 1f;
				num29 = 1f;
			}
			if (num28 == 1f && num29 == 1f)
			{
				num35 = 1f;
				num43 = 1f;
				num44 = num45;
				tileObjectData = tileData2;
				break;
			}
			if (num28 > num35 || (num28 == num35 && num29 > num43))
			{
				TileObjectPreviewData.placementCache.CopyFrom(objectPreview);
				num35 = num28;
				num43 = num29;
				tileObjectData = tileData2;
				num44 = num45;
			}
		}
		int num30 = -1;
		if (flag)
		{
			if (TileObjectPreviewData.randomCache == null)
			{
				TileObjectPreviewData.randomCache = new TileObjectPreviewData();
			}
			bool flag10 = false;
			if (TileObjectPreviewData.randomCache.Type == type)
			{
				Point16 coordinates = TileObjectPreviewData.randomCache.Coordinates;
				Point16 objectStart = TileObjectPreviewData.randomCache.ObjectStart;
				int num31 = coordinates.X + objectStart.X;
				int num32 = coordinates.Y + objectStart.Y;
				int num33 = x - tileData.Origin.X;
				int num34 = y - tileData.Origin.Y;
				if (num31 != num33 || num32 != num34)
				{
					flag10 = true;
				}
			}
			else
			{
				flag10 = true;
			}
			int randomStyleRange = tileData.RandomStyleRange;
			int num36 = Main.rand.Next(tileData.RandomStyleRange);
			if (forcedRandom.HasValue)
			{
				num36 = (forcedRandom.Value % randomStyleRange + randomStyleRange) % randomStyleRange;
			}
			num30 = ((!flag10 && !forcedRandom.HasValue) ? TileObjectPreviewData.randomCache.Random : num36);
		}
		if (tileData.SpecificRandomStyles != null)
		{
			if (TileObjectPreviewData.randomCache == null)
			{
				TileObjectPreviewData.randomCache = new TileObjectPreviewData();
			}
			bool flag2 = false;
			if (TileObjectPreviewData.randomCache.Type == type)
			{
				Point16 coordinates2 = TileObjectPreviewData.randomCache.Coordinates;
				Point16 objectStart2 = TileObjectPreviewData.randomCache.ObjectStart;
				int num37 = coordinates2.X + objectStart2.X;
				int num38 = coordinates2.Y + objectStart2.Y;
				int num39 = x - tileData.Origin.X;
				int num40 = y - tileData.Origin.Y;
				if (num37 != num39 || num38 != num40)
				{
					flag2 = true;
				}
			}
			else
			{
				flag2 = true;
			}
			int num41 = tileData.SpecificRandomStyles.Length;
			int num42 = Main.rand.Next(num41);
			if (forcedRandom.HasValue)
			{
				num42 = (forcedRandom.Value % num41 + num41) % num41;
			}
			num30 = ((!flag2 && !forcedRandom.HasValue) ? TileObjectPreviewData.randomCache.Random : (tileData.SpecificRandomStyles[num42] - style));
		}
		if (onlyCheck)
		{
			if (num35 != 1f || num43 != 1f)
			{
				objectPreview.CopyFrom(TileObjectPreviewData.placementCache);
				num45 = num44;
			}
			objectPreview.Random = num30;
			if (tileData.RandomStyleRange > 0 || tileData.SpecificRandomStyles != null)
			{
				TileObjectPreviewData.randomCache.CopyFrom(objectPreview);
			}
		}
		if (!onlyCheck)
		{
			objectData.xCoord = x - tileObjectData.Origin.X;
			objectData.yCoord = y - tileObjectData.Origin.Y;
			objectData.type = type;
			objectData.style = style;
			objectData.alternate = num45;
			objectData.random = num30;
		}
		if (num35 == 1f)
		{
			return num43 == 1f;
		}
		return false;
	}

	public static void DrawPreview(SpriteBatch sb, TileObjectPreviewData op, Vector2 position)
	{
		//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_023a: Unknown result type (might be due to invalid IL or missing references)
		//IL_024a: Unknown result type (might be due to invalid IL or missing references)
		//IL_024d: Unknown result type (might be due to invalid IL or missing references)
		//IL_024e: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_025e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0261: Unknown result type (might be due to invalid IL or missing references)
		//IL_0262: Unknown result type (might be due to invalid IL or missing references)
		Point16 coordinates = op.Coordinates;
		Texture2D value = TextureAssets.Tile[op.Type].Value;
		TileObjectData tileData = TileObjectData.GetTileData(op.Type, op.Style, op.Alternate);
		int num = 0;
		int num5 = 0;
		int num6 = tileData.CalculatePlacementStyle(op.Style, op.Alternate, op.Random);
		int num7 = 0;
		int num8 = tileData.DrawYOffset;
		int drawXOffset = tileData.DrawXOffset;
		num6 += tileData.DrawStyleOffset;
		int num9 = tileData.StyleWrapLimit;
		int num10 = tileData.StyleLineSkip;
		if (tileData.StyleWrapLimitVisualOverride.HasValue)
		{
			num9 = tileData.StyleWrapLimitVisualOverride.Value;
		}
		if (tileData.styleLineSkipVisualOverride.HasValue)
		{
			num10 = tileData.styleLineSkipVisualOverride.Value;
		}
		if (num9 > 0)
		{
			num7 = num6 / num9 * num10;
			num6 %= num9;
		}
		if (tileData.StyleHorizontal)
		{
			num = tileData.CoordinateFullWidth * num6;
			num5 = tileData.CoordinateFullHeight * num7;
		}
		else
		{
			num = tileData.CoordinateFullWidth * num7;
			num5 = tileData.CoordinateFullHeight * num6;
		}
		for (int i = 0; i < op.Size.X; i++)
		{
			int x = num + (i - op.ObjectStart.X) * (tileData.CoordinateWidth + tileData.CoordinatePadding);
			int num11 = num5;
			for (int j = 0; j < op.Size.Y; j++)
			{
				int num12 = coordinates.X + i;
				int num2 = coordinates.Y + j;
				if (j == 0 && tileData.DrawStepDown != 0 && WorldGen.SolidTile(Framing.GetTileSafely(num12, num2 - 1)))
				{
					num8 += tileData.DrawStepDown;
				}
				if (op.Type == 567)
				{
					num8 = ((j != 0) ? tileData.DrawYOffset : (tileData.DrawYOffset - 2));
				}
				int num3 = op[i, j];
				Color color;
				if (num3 != 1)
				{
					if (num3 != 2)
					{
						continue;
					}
					color = Color.Red * 0.7f;
				}
				else
				{
					color = Color.White;
				}
				color *= 0.5f;
				if (i >= op.ObjectStart.X && i < op.ObjectStart.X + tileData.Width && j >= op.ObjectStart.Y && j < op.ObjectStart.Y + tileData.Height)
				{
					SpriteEffects spriteEffects = (SpriteEffects)0;
					if (tileData.DrawFlipHorizontal && num12 % 2 == 0)
					{
						spriteEffects = (SpriteEffects)(spriteEffects | 1);
					}
					if (tileData.DrawFlipVertical && num2 % 2 == 0)
					{
						spriteEffects = (SpriteEffects)(spriteEffects | 2);
					}
					int coordinateWidth = tileData.CoordinateWidth;
					int num4 = tileData.CoordinateHeights[j - op.ObjectStart.Y];
					if (op.Type == 114 && j == 1)
					{
						num4 += 2;
					}
					Rectangle? val = new Rectangle(x, num11, coordinateWidth, num4);
					sb.Draw(value, new Vector2((float)(num12 * 16 - (int)(position.X + (float)(coordinateWidth - 16) / 2f) + drawXOffset), (float)(num2 * 16 - (int)position.Y + num8)), val, color, 0f, Vector2.Zero, 1f, spriteEffects, 0f);
					num11 += num4 + tileData.CoordinatePadding;
				}
			}
		}
	}
}
