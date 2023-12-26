using System;
using Microsoft.Xna.Framework;
using ReLogic.Threading;
using Terraria.GameContent;
using Terraria.GameContent.Liquid;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace Terraria.Graphics.Light;

public class TileLightScanner
{
	private FastRandom _random = FastRandom.CreateWithRandomSeed();

	private bool _drawInvisibleWalls;

	public void ExportTo(Rectangle area, LightMap outputMap, TileLightScannerOptions options)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		_drawInvisibleWalls = options.DrawInvisibleWalls;
		FastParallel.For(((Rectangle)(ref area)).Left, ((Rectangle)(ref area)).Right, delegate(int start, int end, object context)
		{
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			for (int i = start; i < end; i++)
			{
				for (int j = ((Rectangle)(ref area)).Top; j <= ((Rectangle)(ref area)).Bottom; j++)
				{
					if (IsTileNullOrTouchingNull(i, j))
					{
						outputMap.SetMaskAt(i - area.X, j - area.Y, LightMaskMode.None);
						outputMap[i - area.X, j - area.Y] = Vector3.Zero;
					}
					else
					{
						LightMaskMode tileMask = GetTileMask(Main.tile[i, j]);
						outputMap.SetMaskAt(i - area.X, j - area.Y, tileMask);
						GetTileLight(i, j, out var outputColor);
						outputMap[i - area.X, j - area.Y] = outputColor;
					}
				}
			}
		});
	}

	private bool IsTileNullOrTouchingNull(int x, int y)
	{
		if (WorldGen.InWorld(x, y, 1))
		{
			if (Main.tile[x, y] != null && Main.tile[x + 1, y] != null && Main.tile[x - 1, y] != null && Main.tile[x, y - 1] != null)
			{
				return Main.tile[x, y + 1] == null;
			}
			return true;
		}
		return true;
	}

	public void Update()
	{
		_random.NextSeed();
	}

	public LightMaskMode GetMaskMode(int x, int y)
	{
		return GetTileMask(Main.tile[x, y]);
	}

	private LightMaskMode GetTileMask(Tile tile)
	{
		if (LightIsBlocked(tile) && tile.type != 131 && !tile.inActive() && tile.slope() == 0)
		{
			return LightMaskMode.Solid;
		}
		if (!tile.lava() && tile.liquid > 128)
		{
			if (!tile.honey())
			{
				return LightMaskMode.Water;
			}
			return LightMaskMode.Honey;
		}
		return LightMaskMode.None;
	}

	public void GetTileLight(int x, int y, out Vector3 outputColor)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		outputColor = Vector3.Zero;
		Tile tile = Main.tile[x, y];
		FastRandom localRandom = _random.WithModifier(x, y);
		if (y <= (int)Main.worldSurface)
		{
			ApplySurfaceLight(tile, x, y, ref outputColor);
		}
		else if (y > Main.UnderworldLayer)
		{
			ApplyHellLight(tile, x, y, ref outputColor);
		}
		ApplyWallLight(tile, x, y, ref localRandom, ref outputColor);
		if (tile.active())
		{
			ApplyTileLight(tile, x, y, ref localRandom, ref outputColor);
		}
		ApplyLiquidLight(tile, ref outputColor);
	}

	private void ApplyLiquidLight(Tile tile, ref Vector3 lightColor)
	{
		if (tile.liquid <= 0)
		{
			return;
		}
		if (tile.lava())
		{
			float num = 0.55f;
			num += (float)(270 - Main.mouseTextColor) / 900f;
			if (lightColor.X < num)
			{
				lightColor.X = num;
			}
			if (lightColor.Y < num)
			{
				lightColor.Y = num * 0.6f;
			}
			if (lightColor.Z < num)
			{
				lightColor.Z = num * 0.2f;
			}
		}
		else if (tile.shimmer())
		{
			float num2 = 0.7f;
			float num3 = 0.7f;
			num2 += (float)(270 - Main.mouseTextColor) / 900f;
			num3 += (float)(270 - Main.mouseTextColor) / 125f;
			if (lightColor.X < num2)
			{
				lightColor.X = num2 * 0.6f;
			}
			if (lightColor.Y < num3)
			{
				lightColor.Y = num3 * 0.25f;
			}
			if (lightColor.Z < num2)
			{
				lightColor.Z = num2 * 0.9f;
			}
		}
	}

	private bool LightIsBlocked(Tile tile)
	{
		if (tile.active() && Main.tileBlockLight[tile.type])
		{
			if (tile.invisibleBlock())
			{
				return _drawInvisibleWalls;
			}
			return true;
		}
		return false;
	}

	private void ApplyWallLight(Tile tile, int x, int y, ref FastRandom localRandom, ref Vector3 lightColor)
	{
		float num = 0f;
		float num2 = 0f;
		float num3 = 0f;
		switch (tile.wall)
		{
		case 182:
			if (!LightIsBlocked(tile))
			{
				num = 0.24f;
				num2 = 0.12f;
				num3 = 0.089999996f;
			}
			break;
		case 33:
			if (!LightIsBlocked(tile))
			{
				num = 0.089999996f;
				num2 = 0.052500002f;
				num3 = 0.24f;
			}
			break;
		case 174:
			if (!LightIsBlocked(tile))
			{
				num = 0.2975f;
			}
			break;
		case 175:
			if (!LightIsBlocked(tile))
			{
				num = 0.075f;
				num2 = 0.15f;
				num3 = 0.4f;
			}
			break;
		case 176:
			if (!LightIsBlocked(tile))
			{
				num = 0.1f;
				num2 = 0.1f;
				num3 = 0.1f;
			}
			break;
		case 137:
			if (!LightIsBlocked(tile))
			{
				float num4 = 0.4f;
				num4 += (float)(270 - Main.mouseTextColor) / 1500f;
				num4 += (float)localRandom.Next(0, 50) * 0.0005f;
				num = 1f * num4;
				num2 = 0.5f * num4;
				num3 = 0.1f * num4;
			}
			break;
		case 44:
			if (!LightIsBlocked(tile))
			{
				num = (float)Main.DiscoR / 255f * 0.15f;
				num2 = (float)Main.DiscoG / 255f * 0.15f;
				num3 = (float)Main.DiscoB / 255f * 0.15f;
			}
			break;
		case 154:
			num = 0.6f;
			num3 = 0.6f;
			break;
		case 166:
			num = 0.6f;
			num2 = 0.6f;
			break;
		case 165:
			num3 = 0.6f;
			break;
		case 156:
			num2 = 0.6f;
			break;
		case 164:
			num = 0.6f;
			break;
		case 155:
			num = 0.6f;
			num2 = 0.6f;
			num3 = 0.6f;
			break;
		case 153:
			num = 0.6f;
			num2 = 0.3f;
			break;
		case 341:
			if (!LightIsBlocked(tile))
			{
				num = 0.25f;
				num2 = 0.1f;
				num3 = 0f;
			}
			break;
		case 343:
			if (!LightIsBlocked(tile))
			{
				num = 0f;
				num2 = 0.25f;
				num3 = 0f;
			}
			break;
		case 344:
			if (!LightIsBlocked(tile))
			{
				num = 0f;
				num2 = 0.16f;
				num3 = 0.34f;
			}
			break;
		case 342:
			if (!LightIsBlocked(tile))
			{
				num = 0.3f;
				num2 = 0f;
				num3 = 0.17f;
			}
			break;
		case 345:
			if (!LightIsBlocked(tile))
			{
				num = 0.3f;
				num2 = 0f;
				num3 = 0.35f;
			}
			break;
		case 346:
			if (!LightIsBlocked(tile))
			{
				num = (float)Main.DiscoR / 255f * 0.25f;
				num2 = (float)Main.DiscoG / 255f * 0.25f;
				num3 = (float)Main.DiscoB / 255f * 0.25f;
			}
			break;
		}
		WallLoader.ModifyLight(x, y, tile.wall, ref num, ref num2, ref num3);
		if (lightColor.X < num)
		{
			lightColor.X = num;
		}
		if (lightColor.Y < num2)
		{
			lightColor.Y = num2;
		}
		if (lightColor.Z < num3)
		{
			lightColor.Z = num3;
		}
	}

	private void ApplyTileLight(Tile tile, int x, int y, ref FastRandom localRandom, ref Vector3 lightColor)
	{
		//IL_0c15: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c1a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c21: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f24: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f29: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f2e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f30: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f37: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f3c: Unknown result type (might be due to invalid IL or missing references)
		//IL_076f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0774: Unknown result type (might be due to invalid IL or missing references)
		//IL_0778: Unknown result type (might be due to invalid IL or missing references)
		//IL_0782: Unknown result type (might be due to invalid IL or missing references)
		//IL_0787: Unknown result type (might be due to invalid IL or missing references)
		//IL_078e: Unknown result type (might be due to invalid IL or missing references)
		//IL_3051: Unknown result type (might be due to invalid IL or missing references)
		//IL_3056: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f9f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2fa4: Unknown result type (might be due to invalid IL or missing references)
		//IL_193a: Unknown result type (might be due to invalid IL or missing references)
		//IL_193f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1943: Unknown result type (might be due to invalid IL or missing references)
		//IL_194d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1952: Unknown result type (might be due to invalid IL or missing references)
		//IL_1959: Unknown result type (might be due to invalid IL or missing references)
		//IL_227c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2281: Unknown result type (might be due to invalid IL or missing references)
		//IL_2285: Unknown result type (might be due to invalid IL or missing references)
		//IL_228f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2294: Unknown result type (might be due to invalid IL or missing references)
		//IL_229b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2dc9: Unknown result type (might be due to invalid IL or missing references)
		//IL_2dce: Unknown result type (might be due to invalid IL or missing references)
		//IL_2dd2: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ddc: Unknown result type (might be due to invalid IL or missing references)
		//IL_2de1: Unknown result type (might be due to invalid IL or missing references)
		//IL_2de8: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_07cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_07da: Unknown result type (might be due to invalid IL or missing references)
		//IL_07df: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e0e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e13: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e17: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e21: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e26: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e2d: Unknown result type (might be due to invalid IL or missing references)
		//IL_26d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_26dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_26e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_26eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_26f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_26f7: Unknown result type (might be due to invalid IL or missing references)
		float R = 0f;
		float G = 0f;
		float B = 0f;
		if (Main.tileLighted[tile.type])
		{
			Color val;
			switch (tile.type)
			{
			case 658:
				if (!tile.invisibleBlock())
				{
					TorchID.TorchColor(23, out R, out G, out B);
					switch (tile.frameY / 54)
					{
					default:
						R *= 0.2f;
						G *= 0.2f;
						B *= 0.2f;
						break;
					case 1:
						R *= 0.3f;
						G *= 0.3f;
						B *= 0.3f;
						break;
					case 2:
						R *= 0.1f;
						G *= 0.1f;
						B *= 0.1f;
						break;
					}
				}
				break;
			case 356:
				if (Main.sundialCooldown == 0)
				{
					R = 0.45f;
					G = 0.25f;
					B = 0f;
				}
				break;
			case 663:
				if (Main.moondialCooldown == 0)
				{
					R = 0f;
					G = 0.25f;
					B = 0.45f;
				}
				break;
			case 656:
				R = 0.2f;
				G = 0.55f;
				B = 0.5f;
				break;
			case 20:
			{
				int num13 = tile.frameX / 18;
				if (num13 >= 30 && num13 <= 32)
				{
					R = 0.325f;
					G = 0.15f;
					B = 0.05f;
				}
				break;
			}
			case 634:
				R = 0.65f;
				G = 0.3f;
				B = 0.1f;
				break;
			case 633:
			case 637:
			case 638:
				R = 0.325f;
				G = 0.15f;
				B = 0.05f;
				break;
			case 463:
				R = 0.2f;
				G = 0.4f;
				B = 0.8f;
				break;
			case 491:
				R = 0.5f;
				G = 0.4f;
				B = 0.7f;
				break;
			case 209:
				if (tile.frameX == 234 || tile.frameX == 252)
				{
					val = PortalHelper.GetPortalColor(Main.myPlayer, 0);
					Vector3 val7 = ((Color)(ref val)).ToVector3() * 0.65f;
					R = val7.X;
					G = val7.Y;
					B = val7.Z;
				}
				else if (tile.frameX == 306 || tile.frameX == 324)
				{
					val = PortalHelper.GetPortalColor(Main.myPlayer, 1);
					Vector3 val8 = ((Color)(ref val)).ToVector3() * 0.65f;
					R = val8.X;
					G = val8.Y;
					B = val8.Z;
				}
				break;
			case 415:
				R = 0.7f;
				G = 0.5f;
				B = 0.1f;
				break;
			case 500:
				R = 0.525f;
				G = 0.375f;
				B = 0.075f;
				break;
			case 416:
				R = 0f;
				G = 0.6f;
				B = 0.7f;
				break;
			case 501:
				R = 0f;
				G = 0.45f;
				B = 0.525f;
				break;
			case 417:
				R = 0.6f;
				G = 0.2f;
				B = 0.6f;
				break;
			case 502:
				R = 0.45f;
				G = 0.15f;
				B = 0.45f;
				break;
			case 418:
				R = 0.6f;
				G = 0.6f;
				B = 0.9f;
				break;
			case 503:
				R = 0.45f;
				G = 0.45f;
				B = 0.675f;
				break;
			case 390:
				R = 0.4f;
				G = 0.2f;
				B = 0.1f;
				break;
			case 597:
				switch (tile.frameX / 54)
				{
				case 0:
					R = 0.05f;
					G = 0.8f;
					B = 0.3f;
					break;
				case 1:
					R = 0.7f;
					G = 0.8f;
					B = 0.05f;
					break;
				case 2:
					R = 0.7f;
					G = 0.5f;
					B = 0.9f;
					break;
				case 3:
					R = 0.6f;
					G = 0.6f;
					B = 0.8f;
					break;
				case 4:
					R = 0.4f;
					G = 0.4f;
					B = 1.15f;
					break;
				case 5:
					R = 0.85f;
					G = 0.45f;
					B = 0.1f;
					break;
				case 6:
					R = 0.8f;
					G = 0.8f;
					B = 1f;
					break;
				case 7:
					R = 0.5f;
					G = 0.8f;
					B = 1.2f;
					break;
				}
				R *= 0.75f;
				G *= 0.75f;
				B *= 0.75f;
				break;
			case 564:
				if (tile.frameX < 36)
				{
					R = 0.05f;
					G = 0.3f;
					B = 0.55f;
				}
				break;
			case 568:
				R = 1f;
				G = 0.61f;
				B = 0.65f;
				break;
			case 569:
				R = 0.12f;
				G = 1f;
				B = 0.66f;
				break;
			case 570:
				R = 0.57f;
				G = 0.57f;
				B = 1f;
				break;
			case 580:
				R = 0.7f;
				G = 0.3f;
				B = 0.2f;
				break;
			case 391:
				R = 0.3f;
				G = 0.1f;
				B = 0.25f;
				break;
			case 381:
			case 517:
			case 687:
				R = 0.25f;
				G = 0.1f;
				B = 0f;
				break;
			case 534:
			case 535:
			case 689:
				R = 0f;
				G = 0.25f;
				B = 0f;
				break;
			case 536:
			case 537:
			case 690:
				R = 0f;
				G = 0.16f;
				B = 0.34f;
				break;
			case 539:
			case 540:
			case 688:
				R = 0.3f;
				G = 0f;
				B = 0.17f;
				break;
			case 625:
			case 626:
			case 691:
				R = 0.3f;
				G = 0f;
				B = 0.35f;
				break;
			case 627:
			case 628:
			case 692:
				R = (float)Main.DiscoR / 255f * 0.25f;
				G = (float)Main.DiscoG / 255f * 0.25f;
				B = (float)Main.DiscoB / 255f * 0.25f;
				break;
			case 184:
				if (tile.frameX == 110)
				{
					R = 0.25f;
					G = 0.1f;
					B = 0f;
				}
				if (tile.frameX == 132)
				{
					R = 0f;
					G = 0.25f;
					B = 0f;
				}
				if (tile.frameX == 154)
				{
					R = 0f;
					G = 0.16f;
					B = 0.34f;
				}
				if (tile.frameX == 176)
				{
					R = 0.3f;
					G = 0f;
					B = 0.17f;
				}
				if (tile.frameX == 198)
				{
					R = 0.3f;
					G = 0f;
					B = 0.35f;
				}
				if (tile.frameX == 220)
				{
					R = (float)Main.DiscoR / 255f * 0.25f;
					G = (float)Main.DiscoG / 255f * 0.25f;
					B = (float)Main.DiscoB / 255f * 0.25f;
				}
				break;
			case 370:
				R = 0.32f;
				G = 0.16f;
				B = 0.12f;
				break;
			case 659:
			case 667:
			{
				Vector4 shimmerBaseColor = LiquidRenderer.GetShimmerBaseColor(x, y);
				R = shimmerBaseColor.X;
				G = shimmerBaseColor.Y;
				B = shimmerBaseColor.Z;
				break;
			}
			case 27:
				if (tile.frameY < 36)
				{
					R = 0.3f;
					G = 0.27f;
				}
				break;
			case 336:
				R = 0.85f;
				G = 0.5f;
				B = 0.3f;
				break;
			case 340:
				R = 0.45f;
				G = 1f;
				B = 0.45f;
				break;
			case 341:
				R = 0.4f * Main.demonTorch + 0.6f * (1f - Main.demonTorch);
				G = 0.35f;
				B = 1f * Main.demonTorch + 0.6f * (1f - Main.demonTorch);
				break;
			case 342:
				R = 0.5f;
				G = 0.5f;
				B = 1.1f;
				break;
			case 343:
				R = 0.85f;
				G = 0.85f;
				B = 0.3f;
				break;
			case 344:
				R = 0.6f;
				G = 1.026f;
				B = 0.96000004f;
				break;
			case 327:
			{
				float num8 = 0.5f;
				num8 += (float)(270 - Main.mouseTextColor) / 1500f;
				num8 += (float)localRandom.Next(0, 50) * 0.0005f;
				R = 1f * num8;
				G = 0.5f * num8;
				B = 0.1f * num8;
				break;
			}
			case 316:
			case 317:
			case 318:
			{
				int num21 = x - tile.frameX / 18;
				int num4 = y - tile.frameY / 18;
				int num5 = num21 / 2 * (num4 / 3);
				num5 %= Main.cageFrames;
				bool flag4 = Main.jellyfishCageMode[tile.type - 316, num5] == 2;
				if (tile.type == 316)
				{
					if (flag4)
					{
						R = 0.2f;
						G = 0.3f;
						B = 0.8f;
					}
					else
					{
						R = 0.1f;
						G = 0.2f;
						B = 0.5f;
					}
				}
				if (tile.type == 317)
				{
					if (flag4)
					{
						R = 0.2f;
						G = 0.7f;
						B = 0.3f;
					}
					else
					{
						R = 0.05f;
						G = 0.45f;
						B = 0.1f;
					}
				}
				if (tile.type == 318)
				{
					if (flag4)
					{
						R = 0.7f;
						G = 0.2f;
						B = 0.5f;
					}
					else
					{
						R = 0.4f;
						G = 0.1f;
						B = 0.25f;
					}
				}
				break;
			}
			case 429:
			{
				int num19 = tile.frameX / 18;
				bool flag = num19 % 2 >= 1;
				bool flag2 = num19 % 4 >= 2;
				bool flag3 = num19 % 8 >= 4;
				bool num20 = num19 % 16 >= 8;
				if (flag)
				{
					R += 0.5f;
				}
				if (flag2)
				{
					G += 0.5f;
				}
				if (flag3)
				{
					B += 0.5f;
				}
				if (num20)
				{
					R += 0.2f;
					G += 0.2f;
				}
				break;
			}
			case 286:
			case 619:
				R = 0.1f;
				G = 0.2f;
				B = 0.7f;
				break;
			case 620:
			{
				Color color = Utils.MultiplyRGBA(new Color(230, 230, 230, 0), Main.hslToRgb(Main.GlobalTimeWrappedHourly * 0.5f % 1f, 1f, 0.5f));
				color *= 0.4f;
				R = (float)(int)((Color)(ref color)).R / 255f;
				G = (float)(int)((Color)(ref color)).G / 255f;
				B = (float)(int)((Color)(ref color)).B / 255f;
				break;
			}
			case 582:
			case 598:
				R = 0.7f;
				G = 0.2f;
				B = 0.1f;
				break;
			case 270:
				R = 0.73f;
				G = 1f;
				B = 0.41f;
				break;
			case 271:
				R = 0.45f;
				G = 0.95f;
				B = 1f;
				break;
			case 581:
				R = 1f;
				G = 0.75f;
				B = 0.5f;
				break;
			case 660:
				TorchID.TorchColor(23, out R, out G, out B);
				break;
			case 572:
				switch (tile.frameY / 36)
				{
				case 0:
					R = 0.9f;
					G = 0.5f;
					B = 0.7f;
					break;
				case 1:
					R = 0.7f;
					G = 0.55f;
					B = 0.96f;
					break;
				case 2:
					R = 0.45f;
					G = 0.96f;
					B = 0.95f;
					break;
				case 3:
					R = 0.5f;
					G = 0.96f;
					B = 0.62f;
					break;
				case 4:
					R = 0.47f;
					G = 0.69f;
					B = 0.95f;
					break;
				case 5:
					R = 0.92f;
					G = 0.57f;
					B = 0.51f;
					break;
				}
				break;
			case 262:
				R = 0.75f;
				B = 0.75f;
				break;
			case 263:
				R = 0.75f;
				G = 0.75f;
				break;
			case 264:
				B = 0.75f;
				break;
			case 265:
				G = 0.75f;
				break;
			case 266:
				R = 0.75f;
				break;
			case 267:
				R = 0.75f;
				G = 0.75f;
				B = 0.75f;
				break;
			case 268:
				R = 0.75f;
				G = 0.375f;
				break;
			case 237:
				R = 0.1f;
				G = 0.1f;
				break;
			case 238:
				if ((double)lightColor.X < 0.5)
				{
					lightColor.X = 0.5f;
				}
				if ((double)lightColor.Z < 0.5)
				{
					lightColor.Z = 0.5f;
				}
				break;
			case 235:
				if ((double)lightColor.X < 0.6)
				{
					lightColor.X = 0.6f;
				}
				if ((double)lightColor.Y < 0.6)
				{
					lightColor.Y = 0.6f;
				}
				break;
			case 405:
				if (tile.frameX < 54)
				{
					float num12 = (float)localRandom.Next(28, 42) * 0.005f;
					num12 += (float)(270 - Main.mouseTextColor) / 700f;
					switch (tile.frameX / 54)
					{
					case 1:
						R = 0.7f;
						G = 1f;
						B = 0.5f;
						break;
					case 2:
						R = 0.5f * Main.demonTorch + 1f * (1f - Main.demonTorch);
						G = 0.3f;
						B = 1f * Main.demonTorch + 0.5f * (1f - Main.demonTorch);
						break;
					case 3:
						R = 0.45f;
						G = 0.75f;
						B = 1f;
						break;
					case 4:
						R = 1.15f;
						G = 1.15f;
						B = 0.5f;
						break;
					case 5:
						R = (float)Main.DiscoR / 255f;
						G = (float)Main.DiscoG / 255f;
						B = (float)Main.DiscoB / 255f;
						break;
					default:
						R = 0.9f;
						G = 0.3f;
						B = 0.1f;
						break;
					}
					R += num12;
					G += num12;
					B += num12;
				}
				break;
			case 215:
				if (tile.frameY < 36)
				{
					float num11 = (float)localRandom.Next(28, 42) * 0.005f;
					num11 += (float)(270 - Main.mouseTextColor) / 700f;
					switch (tile.frameX / 54)
					{
					case 1:
						R = 0.7f;
						G = 1f;
						B = 0.5f;
						break;
					case 2:
						R = 0.5f * Main.demonTorch + 1f * (1f - Main.demonTorch);
						G = 0.3f;
						B = 1f * Main.demonTorch + 0.5f * (1f - Main.demonTorch);
						break;
					case 3:
						R = 0.45f;
						G = 0.75f;
						B = 1f;
						break;
					case 4:
						R = 1.15f;
						G = 1.15f;
						B = 0.5f;
						break;
					case 5:
						R = (float)Main.DiscoR / 255f;
						G = (float)Main.DiscoG / 255f;
						B = (float)Main.DiscoB / 255f;
						break;
					case 6:
						R = 0.75f;
						G = 1.2824999f;
						B = 1.2f;
						break;
					case 7:
						R = 0.95f;
						G = 0.65f;
						B = 1.3f;
						break;
					case 8:
						R = 1.4f;
						G = 0.85f;
						B = 0.55f;
						break;
					case 9:
						R = 0.25f;
						G = 1.3f;
						B = 0.8f;
						break;
					case 10:
						R = 0.95f;
						G = 0.4f;
						B = 1.4f;
						break;
					case 11:
						R = 1.4f;
						G = 0.7f;
						B = 0.5f;
						break;
					case 12:
						R = 1.25f;
						G = 0.6f;
						B = 1.2f;
						break;
					case 13:
						R = 0.75f;
						G = 1.45f;
						B = 0.9f;
						break;
					case 14:
						R = 0.25f;
						G = 0.65f;
						B = 1f;
						break;
					case 15:
						TorchID.TorchColor(23, out R, out G, out B);
						break;
					default:
						R = 0.9f;
						G = 0.3f;
						B = 0.1f;
						break;
					}
					R += num11;
					G += num11;
					B += num11;
				}
				break;
			case 92:
				if (tile.frameY <= 18 && tile.frameX == 0)
				{
					R = 1f;
					G = 1f;
					B = 1f;
				}
				break;
			case 592:
				if (tile.frameY > 0)
				{
					float num9 = (float)localRandom.Next(28, 42) * 0.005f;
					num9 += (float)(270 - Main.mouseTextColor) / 700f;
					R = 1.35f;
					G = 0.45f;
					B = 0.15f;
					R += num9;
					G += num9;
					B += num9;
				}
				break;
			case 593:
				if (tile.frameX < 18)
				{
					R = 0.8f;
					G = 0.3f;
					B = 0.1f;
				}
				break;
			case 594:
				if (tile.frameX < 36)
				{
					R = 0.8f;
					G = 0.3f;
					B = 0.1f;
				}
				break;
			case 548:
				if (tile.frameX / 54 >= 7)
				{
					R = 0.7f;
					G = 0.3f;
					B = 0.2f;
				}
				break;
			case 613:
			case 614:
				R = 0.7f;
				G = 0.3f;
				B = 0.2f;
				break;
			case 93:
				if (tile.frameX == 0)
				{
					switch (tile.frameY / 54)
					{
					case 1:
						R = 0.95f;
						G = 0.95f;
						B = 0.5f;
						break;
					case 2:
						R = 0.85f;
						G = 0.6f;
						B = 1f;
						break;
					case 3:
						R = 0.75f;
						G = 1f;
						B = 0.6f;
						break;
					case 4:
					case 5:
						R = 0.75f;
						G = 0.85f;
						B = 1f;
						break;
					case 6:
						R = 1f;
						G = 0.95f;
						B = 0.65f;
						break;
					case 7:
						R = 1f;
						G = 0.97f;
						B = 0.85f;
						break;
					case 9:
						R = 1f;
						G = 1f;
						B = 0.7f;
						break;
					case 10:
						R = 1f;
						G = 0.95f;
						B = 0.65f;
						break;
					case 12:
						R = 1f;
						G = 0.95f;
						B = 0.65f;
						break;
					case 13:
						R = 1f;
						G = 1f;
						B = 0.6f;
						break;
					case 14:
						R = 1f;
						G = 0.95f;
						B = 0.65f;
						break;
					case 18:
						R = 1f;
						G = 0.95f;
						B = 0.65f;
						break;
					case 19:
						R = 0.37f;
						G = 0.8f;
						B = 1f;
						break;
					case 20:
						R = 0f;
						G = 0.9f;
						B = 1f;
						break;
					case 21:
						R = 0.25f;
						G = 0.7f;
						B = 1f;
						break;
					case 23:
						R = 0.5f * Main.demonTorch + 1f * (1f - Main.demonTorch);
						G = 0.3f;
						B = 1f * Main.demonTorch + 0.5f * (1f - Main.demonTorch);
						break;
					case 24:
						R = 0.35f;
						G = 0.5f;
						B = 0.3f;
						break;
					case 25:
						R = 0.34f;
						G = 0.4f;
						B = 0.31f;
						break;
					case 26:
						R = 0.25f;
						G = 0.32f;
						B = 0.5f;
						break;
					case 29:
						R = 0.9f;
						G = 0.75f;
						B = 1f;
						break;
					case 30:
						R = 1f;
						G = 0.95f;
						B = 0.65f;
						break;
					case 31:
					{
						val = Main.hslToRgb(Main.demonTorch * 0.12f + 0.69f, 1f, 0.75f);
						Vector3 val6 = ((Color)(ref val)).ToVector3() * 1.2f;
						R = val6.X;
						G = val6.Y;
						B = val6.Z;
						break;
					}
					case 32:
						R = 1f;
						G = 0.97f;
						B = 0.85f;
						break;
					case 33:
						R = 0.55f;
						G = 0.45f;
						B = 0.95f;
						break;
					case 34:
						R = 1f;
						G = 0.6f;
						B = 0.1f;
						break;
					case 35:
						R = 0.3f;
						G = 0.75f;
						B = 0.55f;
						break;
					case 36:
						R = 0.9f;
						G = 0.55f;
						B = 0.7f;
						break;
					case 37:
						R = 0.55f;
						G = 0.85f;
						B = 1f;
						break;
					case 38:
						R = 1f;
						G = 0.95f;
						B = 0.65f;
						break;
					case 39:
						R = 1f;
						G = 0.95f;
						B = 0.65f;
						break;
					case 40:
						R = 0.4f;
						G = 0.8f;
						B = 0.9f;
						break;
					case 41:
						R = 1f;
						G = 1f;
						B = 1f;
						break;
					case 42:
						R = 0.95f;
						G = 0.5f;
						B = 0.4f;
						break;
					default:
						R = 1f;
						G = 0.97f;
						B = 0.85f;
						break;
					}
				}
				break;
			case 96:
				if (tile.frameX >= 36)
				{
					R = 0.5f;
					G = 0.35f;
					B = 0.1f;
				}
				break;
			case 98:
				if (tile.frameY == 0)
				{
					R = 1f;
					G = 0.97f;
					B = 0.85f;
				}
				break;
			case 4:
				if (tile.frameX < 66)
				{
					TorchID.TorchColor(tile.frameY / 22, out R, out G, out B);
				}
				break;
			case 372:
				if (tile.frameX == 0)
				{
					R = 0.9f;
					G = 0.1f;
					B = 0.75f;
				}
				break;
			case 646:
				if (tile.frameX == 0)
				{
					R = 0.2f;
					G = 0.3f;
					B = 0.32f;
				}
				break;
			case 33:
				if (tile.frameX == 0)
				{
					switch (tile.frameY / 22)
					{
					case 0:
						R = 1f;
						G = 0.95f;
						B = 0.65f;
						break;
					case 1:
						R = 0.55f;
						G = 0.85f;
						B = 0.35f;
						break;
					case 2:
						R = 0.65f;
						G = 0.95f;
						B = 0.5f;
						break;
					case 3:
						R = 0.2f;
						G = 0.75f;
						B = 1f;
						break;
					case 5:
						R = 0.85f;
						G = 0.6f;
						B = 1f;
						break;
					case 7:
					case 8:
						R = 0.75f;
						G = 0.85f;
						B = 1f;
						break;
					case 9:
						R = 1f;
						G = 0.95f;
						B = 0.65f;
						break;
					case 10:
						R = 1f;
						G = 0.97f;
						B = 0.85f;
						break;
					case 14:
						R = 1f;
						G = 1f;
						B = 0.6f;
						break;
					case 15:
						R = 1f;
						G = 0.95f;
						B = 0.65f;
						break;
					case 18:
						R = 1f;
						G = 0.95f;
						B = 0.65f;
						break;
					case 19:
						R = 0.37f;
						G = 0.8f;
						B = 1f;
						break;
					case 20:
						R = 0f;
						G = 0.9f;
						B = 1f;
						break;
					case 21:
						R = 0.25f;
						G = 0.7f;
						B = 1f;
						break;
					case 23:
						R = 1f;
						G = 0.95f;
						B = 0.65f;
						break;
					case 24:
						R = 1f;
						G = 0.95f;
						B = 0.65f;
						break;
					case 25:
						R = 0.5f * Main.demonTorch + 1f * (1f - Main.demonTorch);
						G = 0.3f;
						B = 1f * Main.demonTorch + 0.5f * (1f - Main.demonTorch);
						break;
					case 28:
						R = 0.9f;
						G = 0.75f;
						B = 1f;
						break;
					case 29:
						R = 1f;
						G = 0.95f;
						B = 0.65f;
						break;
					case 30:
					{
						val = Main.hslToRgb(Main.demonTorch * 0.12f + 0.69f, 1f, 0.75f);
						Vector3 val5 = ((Color)(ref val)).ToVector3() * 1.2f;
						R = val5.X;
						G = val5.Y;
						B = val5.Z;
						break;
					}
					case 31:
						R = 1f;
						G = 0.97f;
						B = 0.85f;
						break;
					case 32:
						R = 0.55f;
						G = 0.45f;
						B = 0.95f;
						break;
					case 33:
						R = 1f;
						G = 0.6f;
						B = 0.1f;
						break;
					case 34:
						R = 0.3f;
						G = 0.75f;
						B = 0.55f;
						break;
					case 35:
						R = 0.9f;
						G = 0.55f;
						B = 0.7f;
						break;
					case 36:
						R = 0.55f;
						G = 0.85f;
						B = 1f;
						break;
					case 37:
						R = 1f;
						G = 0.95f;
						B = 0.65f;
						break;
					case 38:
						R = 1f;
						G = 0.95f;
						B = 0.65f;
						break;
					case 39:
						R = 0.4f;
						G = 0.8f;
						B = 0.9f;
						break;
					case 40:
						R = 1f;
						G = 1f;
						B = 1f;
						break;
					case 41:
						R = 0.95f;
						G = 0.5f;
						B = 0.4f;
						break;
					default:
						R = 1f;
						G = 0.95f;
						B = 0.65f;
						break;
					}
				}
				break;
			case 174:
				if (tile.frameX == 0)
				{
					R = 1f;
					G = 0.95f;
					B = 0.65f;
				}
				break;
			case 100:
			case 173:
				if (tile.frameX < 36)
				{
					switch (tile.frameY / 36)
					{
					case 1:
						R = 0.95f;
						G = 0.95f;
						B = 0.5f;
						break;
					case 2:
						R = 0.85f;
						G = 0.6f;
						B = 1f;
						break;
					case 3:
						R = 1f;
						G = 0.6f;
						B = 0.6f;
						break;
					case 5:
						R = 1f;
						G = 0.95f;
						B = 0.65f;
						break;
					case 6:
					case 7:
						R = 1f;
						G = 0.95f;
						B = 0.65f;
						break;
					case 8:
						R = 1f;
						G = 0.97f;
						B = 0.85f;
						break;
					case 9:
						R = 0.75f;
						G = 0.85f;
						B = 1f;
						break;
					case 11:
						R = 1f;
						G = 1f;
						B = 0.7f;
						break;
					case 12:
						R = 1f;
						G = 0.95f;
						B = 0.65f;
						break;
					case 13:
						R = 1f;
						G = 1f;
						B = 0.6f;
						break;
					case 14:
						R = 1f;
						G = 0.95f;
						B = 0.65f;
						break;
					case 18:
						R = 1f;
						G = 0.95f;
						B = 0.65f;
						break;
					case 19:
						R = 0.37f;
						G = 0.8f;
						B = 1f;
						break;
					case 20:
						R = 0f;
						G = 0.9f;
						B = 1f;
						break;
					case 21:
						R = 0.25f;
						G = 0.7f;
						B = 1f;
						break;
					case 25:
						R = 0.5f * Main.demonTorch + 1f * (1f - Main.demonTorch);
						G = 0.3f;
						B = 1f * Main.demonTorch + 0.5f * (1f - Main.demonTorch);
						break;
					case 22:
						R = 0.35f;
						G = 0.5f;
						B = 0.3f;
						break;
					case 23:
						R = 0.34f;
						G = 0.4f;
						B = 0.31f;
						break;
					case 24:
						R = 0.25f;
						G = 0.32f;
						B = 0.5f;
						break;
					case 29:
						R = 0.9f;
						G = 0.75f;
						B = 1f;
						break;
					case 30:
						R = 1f;
						G = 0.95f;
						B = 0.65f;
						break;
					case 31:
					{
						val = Main.hslToRgb(Main.demonTorch * 0.12f + 0.69f, 1f, 0.75f);
						Vector3 val4 = ((Color)(ref val)).ToVector3() * 1.2f;
						R = val4.X;
						G = val4.Y;
						B = val4.Z;
						break;
					}
					case 32:
						R = 1f;
						G = 0.97f;
						B = 0.85f;
						break;
					case 33:
						R = 0.55f;
						G = 0.45f;
						B = 0.95f;
						break;
					case 34:
						R = 1f;
						G = 0.6f;
						B = 0.1f;
						break;
					case 35:
						R = 0.3f;
						G = 0.75f;
						B = 0.55f;
						break;
					case 36:
						R = 0.9f;
						G = 0.55f;
						B = 0.7f;
						break;
					case 37:
						R = 0.55f;
						G = 0.85f;
						B = 1f;
						break;
					case 38:
						R = 1f;
						G = 0.95f;
						B = 0.65f;
						break;
					case 39:
						R = 1f;
						G = 0.95f;
						B = 0.65f;
						break;
					case 40:
						R = 0.4f;
						G = 0.8f;
						B = 0.9f;
						break;
					case 41:
						R = 1f;
						G = 1f;
						B = 1f;
						break;
					case 42:
						R = 0.95f;
						G = 0.5f;
						B = 0.4f;
						break;
					default:
						R = 1f;
						G = 0.95f;
						B = 0.65f;
						break;
					}
				}
				break;
			case 34:
				if (tile.frameX % 108 < 54)
				{
					switch (tile.frameY / 54 + 37 * (tile.frameX / 108))
					{
					case 7:
						R = 0.95f;
						G = 0.95f;
						B = 0.5f;
						break;
					case 8:
						R = 0.85f;
						G = 0.6f;
						B = 1f;
						break;
					case 9:
						R = 1f;
						G = 0.6f;
						B = 0.6f;
						break;
					case 12:
						R = 1f;
						G = 0.95f;
						B = 0.65f;
						break;
					case 13:
						R = 1f;
						G = 0.97f;
						B = 0.85f;
						break;
					case 11:
					case 17:
						R = 0.75f;
						G = 0.85f;
						B = 1f;
						break;
					case 15:
						R = 1f;
						G = 1f;
						B = 0.7f;
						break;
					case 16:
						R = 1f;
						G = 0.95f;
						B = 0.65f;
						break;
					case 18:
						R = 1f;
						G = 1f;
						B = 0.6f;
						break;
					case 19:
						R = 1f;
						G = 0.95f;
						B = 0.65f;
						break;
					case 23:
						R = 1f;
						G = 0.95f;
						B = 0.65f;
						break;
					case 24:
						R = 0.37f;
						G = 0.8f;
						B = 1f;
						break;
					case 25:
						R = 0f;
						G = 0.9f;
						B = 1f;
						break;
					case 26:
						R = 0.25f;
						G = 0.7f;
						B = 1f;
						break;
					case 27:
						R = 0.55f;
						G = 0.85f;
						B = 0.35f;
						break;
					case 28:
						R = 0.65f;
						G = 0.95f;
						B = 0.5f;
						break;
					case 29:
						R = 0.2f;
						G = 0.75f;
						B = 1f;
						break;
					case 30:
						R = 1f;
						G = 0.95f;
						B = 0.65f;
						break;
					case 32:
						R = 0.5f * Main.demonTorch + 1f * (1f - Main.demonTorch);
						G = 0.3f;
						B = 1f * Main.demonTorch + 0.5f * (1f - Main.demonTorch);
						break;
					case 35:
						R = 0.9f;
						G = 0.75f;
						B = 1f;
						break;
					case 36:
						R = 1f;
						G = 0.95f;
						B = 0.65f;
						break;
					case 37:
					{
						val = Main.hslToRgb(Main.demonTorch * 0.12f + 0.69f, 1f, 0.75f);
						Vector3 val3 = ((Color)(ref val)).ToVector3() * 1.2f;
						R = val3.X;
						G = val3.Y;
						B = val3.Z;
						break;
					}
					case 38:
						R = 1f;
						G = 0.97f;
						B = 0.85f;
						break;
					case 39:
						R = 0.55f;
						G = 0.45f;
						B = 0.95f;
						break;
					case 40:
						R = 1f;
						G = 0.6f;
						B = 0.1f;
						break;
					case 41:
						R = 0.3f;
						G = 0.75f;
						B = 0.55f;
						break;
					case 42:
						R = 0.9f;
						G = 0.55f;
						B = 0.7f;
						break;
					case 43:
						R = 0.55f;
						G = 0.85f;
						B = 1f;
						break;
					case 44:
						R = 1f;
						G = 0.95f;
						B = 0.65f;
						break;
					case 45:
						R = 1f;
						G = 0.95f;
						B = 0.65f;
						break;
					case 46:
						R = 0.4f;
						G = 0.8f;
						B = 0.9f;
						break;
					case 47:
						R = 1f;
						G = 1f;
						B = 1f;
						break;
					case 48:
						R = 0.95f;
						G = 0.5f;
						B = 0.4f;
						break;
					default:
						R = 1f;
						G = 0.95f;
						B = 0.8f;
						break;
					}
				}
				break;
			case 35:
				if (tile.frameX < 36)
				{
					R = 0.75f;
					G = 0.6f;
					B = 0.3f;
				}
				break;
			case 95:
				if (tile.frameX < 36)
				{
					R = 1f;
					G = 0.95f;
					B = 0.8f;
				}
				break;
			case 17:
			case 133:
			case 302:
				R = 0.83f;
				G = 0.6f;
				B = 0.5f;
				break;
			case 77:
				R = 0.75f;
				G = 0.45f;
				B = 0.25f;
				break;
			case 37:
				R = 0.56f;
				G = 0.43f;
				B = 0.15f;
				break;
			case 22:
			case 140:
				R = 0.12f;
				G = 0.07f;
				B = 0.32f;
				break;
			case 171:
				if (tile.frameX < 10)
				{
					x -= tile.frameX;
					y -= tile.frameY;
				}
				switch ((Main.tile[x, y].frameY & 0x3C00) >> 10)
				{
				case 1:
					R = 0.1f;
					G = 0.1f;
					B = 0.1f;
					break;
				case 2:
					R = 0.2f;
					break;
				case 3:
					G = 0.2f;
					break;
				case 4:
					B = 0.2f;
					break;
				case 5:
					R = 0.125f;
					G = 0.125f;
					break;
				case 6:
					R = 0.2f;
					G = 0.1f;
					break;
				case 7:
					R = 0.125f;
					G = 0.125f;
					break;
				case 8:
					R = 0.08f;
					G = 0.175f;
					break;
				case 9:
					G = 0.125f;
					B = 0.125f;
					break;
				case 10:
					R = 0.125f;
					B = 0.125f;
					break;
				case 11:
					R = 0.1f;
					G = 0.1f;
					B = 0.2f;
					break;
				default:
					R = (G = (B = 0f));
					break;
				}
				R *= 0.5f;
				G *= 0.5f;
				B *= 0.5f;
				break;
			case 204:
			case 347:
				R = 0.35f;
				break;
			case 42:
				if (tile.frameX == 0)
				{
					switch (tile.frameY / 36)
					{
					case 0:
						R = 0.7f;
						G = 0.65f;
						B = 0.55f;
						break;
					case 1:
						R = 0.9f;
						G = 0.75f;
						B = 0.6f;
						break;
					case 2:
						R = 0.8f;
						G = 0.6f;
						B = 0.6f;
						break;
					case 3:
						R = 0.65f;
						G = 0.5f;
						B = 0.2f;
						break;
					case 4:
						R = 0.5f;
						G = 0.7f;
						B = 0.4f;
						break;
					case 5:
						R = 0.9f;
						G = 0.4f;
						B = 0.2f;
						break;
					case 6:
						R = 0.7f;
						G = 0.75f;
						B = 0.3f;
						break;
					case 7:
					{
						float num7 = Main.demonTorch * 0.2f;
						R = 0.9f - num7;
						G = 0.9f - num7;
						B = 0.7f + num7;
						break;
					}
					case 8:
						R = 0.75f;
						G = 0.6f;
						B = 0.3f;
						break;
					case 9:
						R = 1f;
						G = 0.3f;
						B = 0.5f;
						B += Main.demonTorch * 0.2f;
						R -= Main.demonTorch * 0.1f;
						G -= Main.demonTorch * 0.2f;
						break;
					case 11:
						R = 0.85f;
						G = 0.6f;
						B = 1f;
						break;
					case 14:
						R = 1f;
						G = 0.95f;
						B = 0.65f;
						break;
					case 15:
					case 16:
						R = 1f;
						G = 0.95f;
						B = 0.65f;
						break;
					case 17:
						R = 1f;
						G = 0.97f;
						B = 0.85f;
						break;
					case 18:
						R = 0.75f;
						G = 0.85f;
						B = 1f;
						break;
					case 21:
						R = 1f;
						G = 0.95f;
						B = 0.65f;
						break;
					case 22:
						R = 1f;
						G = 1f;
						B = 0.6f;
						break;
					case 23:
						R = 1f;
						G = 0.95f;
						B = 0.65f;
						break;
					case 27:
						R = 1f;
						G = 0.95f;
						B = 0.65f;
						break;
					case 28:
						R = 0.37f;
						G = 0.8f;
						B = 1f;
						break;
					case 29:
						R = 0f;
						G = 0.9f;
						B = 1f;
						break;
					case 30:
						R = 0.25f;
						G = 0.7f;
						B = 1f;
						break;
					case 32:
						R = 0.5f * Main.demonTorch + 1f * (1f - Main.demonTorch);
						G = 0.3f;
						B = 1f * Main.demonTorch + 0.5f * (1f - Main.demonTorch);
						break;
					case 35:
						R = 0.7f;
						G = 0.6f;
						B = 0.9f;
						break;
					case 36:
						R = 1f;
						G = 0.95f;
						B = 0.65f;
						break;
					case 37:
					{
						val = Main.hslToRgb(Main.demonTorch * 0.12f + 0.69f, 1f, 0.75f);
						Vector3 val2 = ((Color)(ref val)).ToVector3() * 1.2f;
						R = val2.X;
						G = val2.Y;
						B = val2.Z;
						break;
					}
					case 38:
						R = 1f;
						G = 0.97f;
						B = 0.85f;
						break;
					case 39:
						R = 0.55f;
						G = 0.45f;
						B = 0.95f;
						break;
					case 40:
						R = 1f;
						G = 0.6f;
						B = 0.1f;
						break;
					case 41:
						R = 0.3f;
						G = 0.75f;
						B = 0.55f;
						break;
					case 42:
						R = 0.9f;
						G = 0.55f;
						B = 0.7f;
						break;
					case 43:
						R = 0.55f;
						G = 0.85f;
						B = 1f;
						break;
					case 44:
						R = 1f;
						G = 0.95f;
						B = 0.65f;
						break;
					case 45:
						R = 1f;
						G = 0.95f;
						B = 0.65f;
						break;
					case 46:
						R = 0.4f;
						G = 0.8f;
						B = 0.9f;
						break;
					case 47:
						R = 1f;
						G = 1f;
						B = 1f;
						break;
					case 48:
						R = 0.95f;
						G = 0.5f;
						B = 0.4f;
						break;
					default:
						R = 1f;
						G = 1f;
						B = 1f;
						break;
					}
				}
				break;
			case 49:
				if (tile.frameX == 0)
				{
					R = 0f;
					G = 0.35f;
					B = 0.8f;
				}
				break;
			case 519:
				if (tile.frameY == 90)
				{
					if (tile.color() == 0)
					{
						float num6 = (float)localRandom.Next(28, 42) * 0.005f;
						num6 += (float)(270 - Main.mouseTextColor) / 1000f;
						R = 0.1f;
						G = 0.2f + num6 / 2f;
						B = 0.7f + num6;
					}
					else
					{
						Color color3 = WorldGen.paintColor(tile.color());
						R = (float)(int)((Color)(ref color3)).R / 255f;
						G = (float)(int)((Color)(ref color3)).G / 255f;
						B = (float)(int)((Color)(ref color3)).B / 255f;
					}
				}
				break;
			case 70:
			case 71:
			case 72:
			case 190:
			case 348:
			case 349:
			case 528:
			case 578:
				if (tile.type != 349 || tile.frameX >= 36)
				{
					float num3 = (float)localRandom.Next(28, 42) * 0.005f;
					num3 += (float)(270 - Main.mouseTextColor) / 1000f;
					if (tile.color() == 0)
					{
						R = 0f;
						G = 0.2f + num3 / 2f;
						B = 1f;
					}
					else
					{
						Color color2 = WorldGen.paintColor(tile.color());
						R = (float)(int)((Color)(ref color2)).R / 255f;
						G = (float)(int)((Color)(ref color2)).G / 255f;
						B = (float)(int)((Color)(ref color2)).B / 255f;
					}
				}
				break;
			case 350:
			{
				double num2 = Main.timeForVisualEffects * 0.08;
				B = (G = (R = (float)((0.0 - Math.Cos(((int)(num2 / 6.283) % 3 == 1) ? num2 : 0.0)) * 0.1 + 0.1)));
				break;
			}
			case 61:
				if (tile.frameX == 144)
				{
					float num17 = 1f + (float)(270 - Main.mouseTextColor) / 400f;
					float num18 = 0.8f - (float)(270 - Main.mouseTextColor) / 400f;
					R = 0.42f * num18;
					G = 0.81f * num17;
					B = 0.52f * num18;
				}
				break;
			case 26:
			case 31:
				if ((tile.type == 31 && tile.frameX >= 36) || (tile.type == 26 && tile.frameX >= 54))
				{
					float num15 = (float)localRandom.Next(-5, 6) * 0.0025f;
					R = 0.5f + num15 * 2f;
					G = 0.2f + num15;
					B = 0.1f;
				}
				else
				{
					float num16 = (float)localRandom.Next(-5, 6) * 0.0025f;
					R = 0.31f + num16;
					G = 0.1f;
					B = 0.44f + num16 * 2f;
				}
				break;
			case 84:
			{
				int num10 = tile.frameX / 18;
				float num14 = 0f;
				switch (num10)
				{
				case 2:
					num14 = (float)(270 - Main.mouseTextColor) / 800f;
					if (num14 > 1f)
					{
						num14 = 1f;
					}
					else if (num14 < 0f)
					{
						num14 = 0f;
					}
					R = num14 * 0.7f;
					G = num14;
					B = num14 * 0.1f;
					break;
				case 5:
					num14 = 0.9f;
					R = num14;
					G = num14 * 0.8f;
					B = num14 * 0.2f;
					break;
				case 6:
					num14 = 0.08f;
					G = num14 * 0.8f;
					B = num14;
					break;
				}
				break;
			}
			case 83:
				if (tile.frameX == 18 && !Main.dayTime)
				{
					R = 0.1f;
					G = 0.4f;
					B = 0.6f;
				}
				if (tile.frameX == 90 && !Main.raining && Main.time > 40500.0)
				{
					R = 0.9f;
					G = 0.72f;
					B = 0.18f;
				}
				break;
			case 126:
				if (tile.frameX < 36)
				{
					R = (float)Main.DiscoR / 255f;
					G = (float)Main.DiscoG / 255f;
					B = (float)Main.DiscoB / 255f;
				}
				break;
			case 125:
			{
				float num = (float)localRandom.Next(28, 42) * 0.01f;
				num += (float)(270 - Main.mouseTextColor) / 800f;
				G = (lightColor.Y = 0.3f * num);
				B = (lightColor.Z = 0.6f * num);
				break;
			}
			case 129:
				switch (tile.frameX / 18 % 3)
				{
				case 0:
					R = 0f;
					G = 0.05f;
					B = 0.25f;
					break;
				case 1:
					R = 0.2f;
					G = 0f;
					B = 0.15f;
					break;
				case 2:
					R = 0.1f;
					G = 0f;
					B = 0.2f;
					break;
				}
				break;
			case 149:
				if (tile.frameX <= 36)
				{
					switch (tile.frameX / 18)
					{
					case 0:
						R = 0.1f;
						G = 0.2f;
						B = 0.5f;
						break;
					case 1:
						R = 0.5f;
						G = 0.1f;
						B = 0.1f;
						break;
					case 2:
						R = 0.2f;
						G = 0.5f;
						B = 0.1f;
						break;
					}
					R *= (float)localRandom.Next(970, 1031) * 0.001f;
					G *= (float)localRandom.Next(970, 1031) * 0.001f;
					B *= (float)localRandom.Next(970, 1031) * 0.001f;
				}
				break;
			case 160:
				R = (float)Main.DiscoR / 255f * 0.25f;
				G = (float)Main.DiscoG / 255f * 0.25f;
				B = (float)Main.DiscoB / 255f * 0.25f;
				break;
			case 354:
				R = 0.65f;
				G = 0.35f;
				B = 0.15f;
				break;
			}
			TileLoader.ModifyLight(x, y, tile.type, ref R, ref G, ref B);
		}
		if (lightColor.X < R)
		{
			lightColor.X = R;
		}
		if (lightColor.Y < G)
		{
			lightColor.Y = G;
		}
		if (lightColor.Z < B)
		{
			lightColor.Z = B;
		}
	}

	private void ApplySurfaceLight(Tile tile, int x, int y, ref Vector3 lightColor)
	{
		float num = 0f;
		float num4 = 0f;
		float num5 = 0f;
		float num6 = (float)(int)((Color)(ref Main.tileColor)).R / 255f;
		float num7 = (float)(int)((Color)(ref Main.tileColor)).G / 255f;
		float num8 = (float)(int)((Color)(ref Main.tileColor)).B / 255f;
		float num9 = (num6 + num7 + num8) / 3f;
		if (tile.active() && TileID.Sets.AllowLightInWater[tile.type])
		{
			if (lightColor.X < num9 && (Main.wallLight[tile.wall] || tile.wall == 73 || tile.wall == 227 || (tile.invisibleWall() && !_drawInvisibleWalls)))
			{
				num = num6;
				num4 = num7;
				num5 = num8;
			}
		}
		else if ((!tile.active() || !Main.tileNoSunLight[tile.type] || ((tile.slope() != 0 || tile.halfBrick() || (tile.invisibleBlock() && !_drawInvisibleWalls)) && Main.tile[x, y - 1].liquid == 0 && Main.tile[x, y + 1].liquid == 0 && Main.tile[x - 1, y].liquid == 0 && Main.tile[x + 1, y].liquid == 0)) && lightColor.X < num9 && (Main.wallLight[tile.wall] || tile.wall == 73 || tile.wall == 227 || (tile.invisibleWall() && !_drawInvisibleWalls)))
		{
			if (tile.liquid < 200)
			{
				if (!tile.halfBrick() || Main.tile[x, y - 1].liquid < 200)
				{
					num = num6;
					num4 = num7;
					num5 = num8;
				}
			}
			else if (Main.liquidAlpha[13] > 0f)
			{
				if (Main.rand == null)
				{
					Main.rand = new UnifiedRandom();
				}
				num5 = num8 * 0.175f * (1f + Main.rand.NextFloat() * 0.13f) * Main.liquidAlpha[13];
			}
		}
		if ((!tile.active() || tile.halfBrick() || !Main.tileNoSunLight[tile.type]) && ((tile.wall >= 88 && tile.wall <= 93) || tile.wall == 241) && tile.liquid < byte.MaxValue)
		{
			num = num6;
			num4 = num7;
			num5 = num8;
			int num10 = tile.wall - 88;
			if (tile.wall == 241)
			{
				num10 = 6;
			}
			switch (num10)
			{
			case 0:
				num *= 0.9f;
				num4 *= 0.15f;
				num5 *= 0.9f;
				break;
			case 1:
				num *= 0.9f;
				num4 *= 0.9f;
				num5 *= 0.15f;
				break;
			case 2:
				num *= 0.15f;
				num4 *= 0.15f;
				num5 *= 0.9f;
				break;
			case 3:
				num *= 0.15f;
				num4 *= 0.9f;
				num5 *= 0.15f;
				break;
			case 4:
				num *= 0.9f;
				num4 *= 0.15f;
				num5 *= 0.15f;
				break;
			case 5:
			{
				float num11 = 0.2f;
				float num2 = 0.7f - num11;
				num *= num2 + (float)Main.DiscoR / 255f * num11;
				num4 *= num2 + (float)Main.DiscoG / 255f * num11;
				num5 *= num2 + (float)Main.DiscoB / 255f * num11;
				break;
			}
			case 6:
				num *= 0.9f;
				num4 *= 0.5f;
				num5 *= 0f;
				break;
			}
		}
		float num3 = 1f - Main.shimmerDarken;
		num *= num3;
		num4 *= num3;
		num5 *= num3;
		if (lightColor.X < num)
		{
			lightColor.X = num;
		}
		if (lightColor.Y < num4)
		{
			lightColor.Y = num4;
		}
		if (lightColor.Z < num5)
		{
			lightColor.Z = num5;
		}
	}

	private void ApplyHellLight(Tile tile, int x, int y, ref Vector3 lightColor)
	{
		float num = 0f;
		float num2 = 0f;
		float num3 = 0f;
		float num4 = 0.55f + (float)Math.Sin(Main.GlobalTimeWrappedHourly * 2f) * 0.08f;
		if ((!tile.active() || !Main.tileNoSunLight[tile.type] || ((tile.slope() != 0 || tile.halfBrick()) && Main.tile[x, y - 1].liquid == 0 && Main.tile[x, y + 1].liquid == 0 && Main.tile[x - 1, y].liquid == 0 && Main.tile[x + 1, y].liquid == 0)) && lightColor.X < num4 && (Main.wallLight[tile.wall] || tile.wall == 73 || tile.wall == 227 || (tile.invisibleWall() && !_drawInvisibleWalls)) && tile.liquid < 200 && (!tile.halfBrick() || Main.tile[x, y - 1].liquid < 200))
		{
			num = num4;
			num2 = num4 * 0.6f;
			num3 = num4 * 0.2f;
		}
		if ((!tile.active() || tile.halfBrick() || !Main.tileNoSunLight[tile.type]) && tile.wall >= 88 && tile.wall <= 93 && tile.liquid < byte.MaxValue)
		{
			num = num4;
			num2 = num4 * 0.6f;
			num3 = num4 * 0.2f;
			switch (tile.wall)
			{
			case 88:
				num *= 0.9f;
				num2 *= 0.15f;
				num3 *= 0.9f;
				break;
			case 89:
				num *= 0.9f;
				num2 *= 0.9f;
				num3 *= 0.15f;
				break;
			case 90:
				num *= 0.15f;
				num2 *= 0.15f;
				num3 *= 0.9f;
				break;
			case 91:
				num *= 0.15f;
				num2 *= 0.9f;
				num3 *= 0.15f;
				break;
			case 92:
				num *= 0.9f;
				num2 *= 0.15f;
				num3 *= 0.15f;
				break;
			case 93:
			{
				float num5 = 0.2f;
				float num6 = 0.7f - num5;
				num *= num6 + (float)Main.DiscoR / 255f * num5;
				num2 *= num6 + (float)Main.DiscoG / 255f * num5;
				num3 *= num6 + (float)Main.DiscoB / 255f * num5;
				break;
			}
			}
		}
		if (lightColor.X < num)
		{
			lightColor.X = num;
		}
		if (lightColor.Y < num2)
		{
			lightColor.Y = num2;
		}
		if (lightColor.Z < num3)
		{
			lightColor.Z = num3;
		}
	}
}
