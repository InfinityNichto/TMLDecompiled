using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.GameContent.Liquid;
using Terraria.ID;
using Terraria.IO;
using Terraria.ModLoader;

namespace Terraria;

public class WaterfallManager
{
	public struct WaterfallData
	{
		public int x;

		public int y;

		public int type;

		public int stopAtStep;
	}

	private const int minWet = 160;

	private const int maxWaterfallCountDefault = 1000;

	private const int maxLength = 100;

	internal const int maxTypes = 26;

	public int maxWaterfallCount = 1000;

	private int qualityMax;

	private int currentMax;

	private WaterfallData[] waterfalls = new WaterfallData[1000];

	internal Asset<Texture2D>[] waterfallTexture = new Asset<Texture2D>[26];

	private int wFallFrCounter;

	private int regularFrame;

	private int wFallFrCounter2;

	private int slowFrame;

	private int rainFrameCounter;

	private int rainFrameForeground;

	private int rainFrameBackground;

	private int snowFrameCounter;

	private int snowFrameForeground;

	private int findWaterfallCount;

	private int waterfallDist = 100;

	public void BindTo(Preferences preferences)
	{
		preferences.OnLoad += Configuration_OnLoad;
	}

	private void Configuration_OnLoad(Preferences preferences)
	{
		maxWaterfallCount = Math.Max(0, preferences.Get("WaterfallDrawLimit", 1000));
		waterfalls = new WaterfallData[maxWaterfallCount];
	}

	public void LoadContent()
	{
		for (int i = 0; i < 26; i++)
		{
			waterfallTexture[i] = Main.Assets.Request<Texture2D>("Images/Waterfall_" + i, AssetRequestMode.AsyncLoad);
		}
	}

	public bool CheckForWaterfall(int i, int j)
	{
		for (int k = 0; k < currentMax; k++)
		{
			if (waterfalls[k].x == i && waterfalls[k].y == j)
			{
				return true;
			}
		}
		return false;
	}

	public void FindWaterfalls(bool forced = false)
	{
		findWaterfallCount++;
		if (findWaterfallCount < 30 && !forced)
		{
			return;
		}
		findWaterfallCount = 0;
		waterfallDist = (int)(75f * Main.gfxQuality) + 25;
		qualityMax = (int)((float)maxWaterfallCount * Main.gfxQuality);
		currentMax = 0;
		int num = (int)(Main.screenPosition.X / 16f - 1f);
		int num2 = (int)((Main.screenPosition.X + (float)Main.screenWidth) / 16f) + 2;
		int num3 = (int)(Main.screenPosition.Y / 16f - 1f);
		int num4 = (int)((Main.screenPosition.Y + (float)Main.screenHeight) / 16f) + 2;
		num -= waterfallDist;
		num2 += waterfallDist;
		num3 -= waterfallDist;
		num4 += 20;
		if (num < 0)
		{
			num = 0;
		}
		if (num2 > Main.maxTilesX)
		{
			num2 = Main.maxTilesX;
		}
		if (num3 < 0)
		{
			num3 = 0;
		}
		if (num4 > Main.maxTilesY)
		{
			num4 = Main.maxTilesY;
		}
		for (int i = num; i < num2; i++)
		{
			for (int j = num3; j < num4; j++)
			{
				Tile tile = Main.tile[i, j];
				if (tile == null)
				{
					tile = (Main.tile[i, j] = default(Tile));
				}
				if (!tile.active())
				{
					continue;
				}
				if (tile.halfBrick())
				{
					Tile tile2 = Main.tile[i, j - 1];
					if (tile2 == null)
					{
						tile2 = (Main.tile[i, j - 1] = default(Tile));
					}
					if (tile2.liquid < 16 || WorldGen.SolidTile(tile2))
					{
						Tile tile3 = Main.tile[i - 1, j];
						if (tile3 == null)
						{
							tile3 = (Main.tile[i - 1, j] = default(Tile));
						}
						Tile tile4 = Main.tile[i + 1, j];
						if (tile4 == null)
						{
							tile4 = (Main.tile[i + 1, j] = default(Tile));
						}
						if ((tile3.liquid > 160 || tile4.liquid > 160) && ((tile3.liquid == 0 && !WorldGen.SolidTile(tile3) && tile3.slope() == 0) || (tile4.liquid == 0 && !WorldGen.SolidTile(tile4) && tile4.slope() == 0)) && currentMax < qualityMax)
						{
							waterfalls[currentMax].type = 0;
							if (tile2.lava() || tile4.lava() || tile3.lava())
							{
								waterfalls[currentMax].type = 1;
							}
							else if (tile2.honey() || tile4.honey() || tile3.honey())
							{
								waterfalls[currentMax].type = 14;
							}
							else if (tile2.shimmer() || tile4.shimmer() || tile3.shimmer())
							{
								waterfalls[currentMax].type = 25;
							}
							else
							{
								waterfalls[currentMax].type = 0;
							}
							waterfalls[currentMax].x = i;
							waterfalls[currentMax].y = j;
							currentMax++;
						}
					}
				}
				if (tile.type == 196)
				{
					Tile tile5 = Main.tile[i, j + 1];
					if (tile5 == null)
					{
						tile5 = (Main.tile[i, j + 1] = default(Tile));
					}
					if (!WorldGen.SolidTile(tile5) && tile5.slope() == 0 && currentMax < qualityMax)
					{
						waterfalls[currentMax].type = 11;
						waterfalls[currentMax].x = i;
						waterfalls[currentMax].y = j + 1;
						currentMax++;
					}
				}
				if (tile.type == 460)
				{
					Tile tile6 = Main.tile[i, j + 1];
					if (tile6 == null)
					{
						tile6 = (Main.tile[i, j + 1] = default(Tile));
					}
					if (!WorldGen.SolidTile(tile6) && tile6.slope() == 0 && currentMax < qualityMax)
					{
						waterfalls[currentMax].type = 22;
						waterfalls[currentMax].x = i;
						waterfalls[currentMax].y = j + 1;
						currentMax++;
					}
				}
			}
		}
	}

	public void UpdateFrame()
	{
		wFallFrCounter++;
		if (wFallFrCounter > 2)
		{
			wFallFrCounter = 0;
			regularFrame++;
			if (regularFrame > 15)
			{
				regularFrame = 0;
			}
		}
		wFallFrCounter2++;
		if (wFallFrCounter2 > 6)
		{
			wFallFrCounter2 = 0;
			slowFrame++;
			if (slowFrame > 15)
			{
				slowFrame = 0;
			}
		}
		rainFrameCounter++;
		if (rainFrameCounter > 0)
		{
			rainFrameForeground++;
			if (rainFrameForeground > 7)
			{
				rainFrameForeground -= 8;
			}
			if (rainFrameCounter > 2)
			{
				rainFrameCounter = 0;
				rainFrameBackground--;
				if (rainFrameBackground < 0)
				{
					rainFrameBackground = 7;
				}
			}
		}
		if (++snowFrameCounter > 3)
		{
			snowFrameCounter = 0;
			if (++snowFrameForeground > 7)
			{
				snowFrameForeground = 0;
			}
		}
	}

	internal void DrawWaterfall(int Style = 0, float Alpha = 1f)
	{
		//IL_04d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04db: Unknown result type (might be due to invalid IL or missing references)
		//IL_029a: Unknown result type (might be due to invalid IL or missing references)
		//IL_029f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0279: Unknown result type (might be due to invalid IL or missing references)
		//IL_027e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0283: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_032c: Unknown result type (might be due to invalid IL or missing references)
		//IL_032f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0334: Unknown result type (might be due to invalid IL or missing references)
		//IL_0338: Unknown result type (might be due to invalid IL or missing references)
		//IL_033d: Unknown result type (might be due to invalid IL or missing references)
		//IL_038f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0391: Unknown result type (might be due to invalid IL or missing references)
		//IL_0398: Unknown result type (might be due to invalid IL or missing references)
		//IL_039c: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_0358: Unknown result type (might be due to invalid IL or missing references)
		//IL_035a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0361: Unknown result type (might be due to invalid IL or missing references)
		//IL_0368: Unknown result type (might be due to invalid IL or missing references)
		//IL_082a: Unknown result type (might be due to invalid IL or missing references)
		//IL_082f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0862: Unknown result type (might be due to invalid IL or missing references)
		//IL_0864: Unknown result type (might be due to invalid IL or missing references)
		//IL_0869: Unknown result type (might be due to invalid IL or missing references)
		//IL_083d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ab1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ab6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0abb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ace: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ad3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a6a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a6f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a74: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a87: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a8c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b01: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b06: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c5c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c61: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c66: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c77: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c7c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b6c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b71: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b76: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b86: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b8b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b28: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b2d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b32: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b42: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b47: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c18: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c1d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c22: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c33: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c38: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bd1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bd6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bdb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bec: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bf1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e98: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e9d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ea2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eb2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eb7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e5a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e5f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e64: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e74: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e79: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e13: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e18: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e1d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e2d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e32: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dcf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dd4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dd9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0de9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dee: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d78: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d7d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d82: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d8e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d93: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d3b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d40: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d45: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d51: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d56: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cf8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cfd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d02: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d0e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d13: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cbb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cc0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cc5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cd1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cd6: Unknown result type (might be due to invalid IL or missing references)
		//IL_13f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_13f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_138a: Unknown result type (might be due to invalid IL or missing references)
		//IL_138f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1394: Unknown result type (might be due to invalid IL or missing references)
		//IL_13a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_13a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fc8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fcd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fd2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fe1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fe6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ef9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0efe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f03: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f12: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f17: Unknown result type (might be due to invalid IL or missing references)
		//IL_1312: Unknown result type (might be due to invalid IL or missing references)
		//IL_1317: Unknown result type (might be due to invalid IL or missing references)
		//IL_131c: Unknown result type (might be due to invalid IL or missing references)
		//IL_132b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1330: Unknown result type (might be due to invalid IL or missing references)
		//IL_11c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_11c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_11ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_11d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_11de: Unknown result type (might be due to invalid IL or missing references)
		//IL_1055: Unknown result type (might be due to invalid IL or missing references)
		//IL_105a: Unknown result type (might be due to invalid IL or missing references)
		//IL_105f: Unknown result type (might be due to invalid IL or missing references)
		//IL_106f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1074: Unknown result type (might be due to invalid IL or missing references)
		//IL_1011: Unknown result type (might be due to invalid IL or missing references)
		//IL_1016: Unknown result type (might be due to invalid IL or missing references)
		//IL_101b: Unknown result type (might be due to invalid IL or missing references)
		//IL_102b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f80: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f85: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f8a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f9a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f9f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f3f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f44: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f49: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f59: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f5e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1271: Unknown result type (might be due to invalid IL or missing references)
		//IL_1276: Unknown result type (might be due to invalid IL or missing references)
		//IL_127b: Unknown result type (might be due to invalid IL or missing references)
		//IL_128f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1294: Unknown result type (might be due to invalid IL or missing references)
		//IL_111f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1124: Unknown result type (might be due to invalid IL or missing references)
		//IL_1129: Unknown result type (might be due to invalid IL or missing references)
		//IL_113d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1142: Unknown result type (might be due to invalid IL or missing references)
		Main.tileSolid[546] = false;
		float num = 0f;
		float num12 = 99999f;
		float num23 = 99999f;
		int num34 = -1;
		int num45 = -1;
		float num47 = 0f;
		float num48 = 99999f;
		float num49 = 99999f;
		int num50 = -1;
		int num2 = -1;
		Rectangle value = default(Rectangle);
		Rectangle value2 = default(Rectangle);
		Vector2 origin = default(Vector2);
		for (int i = 0; i < currentMax; i++)
		{
			int num3 = 0;
			int num4 = waterfalls[i].type;
			int num5 = waterfalls[i].x;
			int num6 = waterfalls[i].y;
			int num7 = 0;
			int num8 = 0;
			int num9 = 0;
			int num10 = 0;
			int num11 = 0;
			int num13 = 0;
			int num14;
			int num15;
			if (num4 == 1 || num4 == 14 || num4 == 25)
			{
				if (Main.drewLava || waterfalls[i].stopAtStep == 0)
				{
					continue;
				}
				num14 = 32 * slowFrame;
			}
			else
			{
				switch (num4)
				{
				case 11:
				case 22:
				{
					if (Main.drewLava)
					{
						continue;
					}
					num15 = waterfallDist / 4;
					if (num4 == 22)
					{
						num15 = waterfallDist / 2;
					}
					if (waterfalls[i].stopAtStep > num15)
					{
						waterfalls[i].stopAtStep = num15;
					}
					if (waterfalls[i].stopAtStep == 0 || (float)(num6 + num15) < Main.screenPosition.Y / 16f || (float)num5 < Main.screenPosition.X / 16f - 20f || (float)num5 > (Main.screenPosition.X + (float)Main.screenWidth) / 16f + 20f)
					{
						continue;
					}
					int num16;
					int num17;
					if (num5 % 2 == 0)
					{
						num16 = rainFrameForeground + 3;
						if (num16 > 7)
						{
							num16 -= 8;
						}
						num17 = rainFrameBackground + 2;
						if (num17 > 7)
						{
							num17 -= 8;
						}
						if (num4 == 22)
						{
							num16 = snowFrameForeground + 3;
							if (num16 > 7)
							{
								num16 -= 8;
							}
						}
					}
					else
					{
						num16 = rainFrameForeground;
						num17 = rainFrameBackground;
						if (num4 == 22)
						{
							num16 = snowFrameForeground;
						}
					}
					((Rectangle)(ref value))._002Ector(num17 * 18, 0, 16, 16);
					((Rectangle)(ref value2))._002Ector(num16 * 18, 0, 16, 16);
					((Vector2)(ref origin))._002Ector(8f, 8f);
					Vector2 position = ((num6 % 2 != 0) ? (new Vector2((float)(num5 * 16 + 8), (float)(num6 * 16 + 8)) - Main.screenPosition) : (new Vector2((float)(num5 * 16 + 9), (float)(num6 * 16 + 8)) - Main.screenPosition));
					Tile tile = Main.tile[num5, num6 - 1];
					if (tile.active() && tile.bottomSlope())
					{
						position.Y -= 16f;
					}
					bool flag = false;
					float rotation = 0f;
					for (int j = 0; j < num15; j++)
					{
						Color color6 = Lighting.GetColor(num5, num6);
						float num18 = 0.6f;
						float num19 = 0.3f;
						if (j > num15 - 8)
						{
							float num20 = (float)(num15 - j) / 8f;
							num18 *= num20;
							num19 *= num20;
						}
						Color color2 = color6 * num18;
						Color color3 = color6 * num19;
						if (num4 == 22)
						{
							Main.spriteBatch.Draw(waterfallTexture[22].Value, position, (Rectangle?)value2, color2, 0f, origin, 1f, (SpriteEffects)0, 0f);
						}
						else
						{
							Main.spriteBatch.Draw(waterfallTexture[12].Value, position, (Rectangle?)value, color3, rotation, origin, 1f, (SpriteEffects)0, 0f);
							Main.spriteBatch.Draw(waterfallTexture[11].Value, position, (Rectangle?)value2, color2, rotation, origin, 1f, (SpriteEffects)0, 0f);
						}
						if (flag)
						{
							break;
						}
						num6++;
						Tile tile2 = Main.tile[num5, num6];
						if (WorldGen.SolidTile(tile2))
						{
							flag = true;
						}
						if (tile2.liquid > 0)
						{
							int num21 = (int)(16f * ((float)(int)tile2.liquid / 255f)) & 0xFE;
							if (num21 >= 15)
							{
								break;
							}
							value2.Height -= num21;
							value.Height -= num21;
						}
						if (num6 % 2 == 0)
						{
							position.X += 1f;
						}
						else
						{
							position.X -= 1f;
						}
						position.Y += 16f;
					}
					waterfalls[i].stopAtStep = 0;
					continue;
				}
				case 0:
					num4 = Style;
					break;
				case 2:
					if (Main.drewLava)
					{
						continue;
					}
					break;
				}
				num14 = 32 * regularFrame;
			}
			int num22 = 0;
			num15 = waterfallDist;
			Color color4 = Color.White;
			for (int k = 0; k < num15; k++)
			{
				if (num22 >= 2)
				{
					break;
				}
				AddLight(num4, num5, num6);
				Tile tile3 = Main.tile[num5, num6];
				if (tile3 == null)
				{
					tile3 = (Main.tile[num5, num6] = default(Tile));
				}
				if (tile3.nactive() && Main.tileSolid[tile3.type] && !Main.tileSolidTop[tile3.type] && !TileID.Sets.Platforms[tile3.type] && tile3.blockType() == 0)
				{
					break;
				}
				Tile tile4 = Main.tile[num5 - 1, num6];
				if (tile4 == null)
				{
					tile4 = (Main.tile[num5 - 1, num6] = default(Tile));
				}
				Tile tile5 = Main.tile[num5, num6 + 1];
				if (tile5 == null)
				{
					tile5 = (Main.tile[num5, num6 + 1] = default(Tile));
				}
				Tile tile6 = Main.tile[num5 + 1, num6];
				if (tile6 == null)
				{
					tile6 = (Main.tile[num5 + 1, num6] = default(Tile));
				}
				if (WorldGen.SolidTile(tile5) && !tile3.halfBrick())
				{
					num3 = 8;
				}
				else if (num8 != 0)
				{
					num3 = 0;
				}
				int num24 = 0;
				int num25 = num10;
				int num26 = 0;
				int num27 = 0;
				bool flag2 = false;
				if (tile5.topSlope() && !tile3.halfBrick() && tile5.type != 19)
				{
					flag2 = true;
					if (tile5.slope() == 1)
					{
						num24 = 1;
						num26 = 1;
						num9 = 1;
						num10 = num9;
					}
					else
					{
						num24 = -1;
						num26 = -1;
						num9 = -1;
						num10 = num9;
					}
					num27 = 1;
				}
				else if ((!WorldGen.SolidTile(tile5) && !tile5.bottomSlope() && !tile3.halfBrick()) || (!tile5.active() && !tile3.halfBrick()))
				{
					num22 = 0;
					num27 = 1;
					num26 = 0;
				}
				else if ((WorldGen.SolidTile(tile4) || tile4.topSlope() || tile4.liquid > 0) && !WorldGen.SolidTile(tile6) && tile6.liquid == 0)
				{
					if (num9 == -1)
					{
						num22++;
					}
					num26 = 1;
					num27 = 0;
					num9 = 1;
				}
				else if ((WorldGen.SolidTile(tile6) || tile6.topSlope() || tile6.liquid > 0) && !WorldGen.SolidTile(tile4) && tile4.liquid == 0)
				{
					if (num9 == 1)
					{
						num22++;
					}
					num26 = -1;
					num27 = 0;
					num9 = -1;
				}
				else if (((!WorldGen.SolidTile(tile6) && !tile3.topSlope()) || tile6.liquid == 0) && !WorldGen.SolidTile(tile4) && !tile3.topSlope() && tile4.liquid == 0)
				{
					num27 = 0;
					num26 = num9;
				}
				else
				{
					num22++;
					num27 = 0;
					num26 = 0;
				}
				if (num22 >= 2)
				{
					num9 *= -1;
					num26 *= -1;
				}
				int num28 = -1;
				if (num4 != 1 && num4 != 14 && num4 != 25)
				{
					if (tile5.active())
					{
						num28 = tile5.type;
					}
					if (tile3.active())
					{
						num28 = tile3.type;
					}
				}
				switch (num28)
				{
				case 160:
					num4 = 2;
					break;
				case 262:
				case 263:
				case 264:
				case 265:
				case 266:
				case 267:
				case 268:
					num4 = 15 + num28 - 262;
					break;
				}
				if (num28 != -1)
				{
					TileLoader.ChangeWaterfallStyle(num28, ref num4);
				}
				Color color5 = Lighting.GetColor(num5, num6);
				if (k > 50)
				{
					TrySparkling(num5, num6, num9, color5);
				}
				float alpha = GetAlpha(Alpha, num15, num4, num6, k, tile3);
				color5 = StylizeColor(alpha, num15, num4, num6, k, tile3, color5);
				if (num4 == 1)
				{
					float num29 = Math.Abs((float)(num5 * 16 + 8) - (Main.screenPosition.X + (float)(Main.screenWidth / 2)));
					float num30 = Math.Abs((float)(num6 * 16 + 8) - (Main.screenPosition.Y + (float)(Main.screenHeight / 2)));
					if (num29 < (float)(Main.screenWidth * 2) && num30 < (float)(Main.screenHeight * 2))
					{
						float num31 = (float)Math.Sqrt(num29 * num29 + num30 * num30);
						float num32 = 1f - num31 / ((float)Main.screenWidth * 0.75f);
						if (num32 > 0f)
						{
							num47 += num32;
						}
					}
					if (num29 < num48)
					{
						num48 = num29;
						num50 = num5 * 16 + 8;
					}
					if (num30 < num49)
					{
						num49 = num29;
						num2 = num6 * 16 + 8;
					}
				}
				else if (num4 != 1 && num4 != 14 && num4 != 25 && num4 != 11 && num4 != 12 && num4 != 22)
				{
					float num33 = Math.Abs((float)(num5 * 16 + 8) - (Main.screenPosition.X + (float)(Main.screenWidth / 2)));
					float num35 = Math.Abs((float)(num6 * 16 + 8) - (Main.screenPosition.Y + (float)(Main.screenHeight / 2)));
					if (num33 < (float)(Main.screenWidth * 2) && num35 < (float)(Main.screenHeight * 2))
					{
						float num36 = (float)Math.Sqrt(num33 * num33 + num35 * num35);
						float num37 = 1f - num36 / ((float)Main.screenWidth * 0.75f);
						if (num37 > 0f)
						{
							num += num37;
						}
					}
					if (num33 < num12)
					{
						num12 = num33;
						num34 = num5 * 16 + 8;
					}
					if (num35 < num23)
					{
						num23 = num33;
						num45 = num6 * 16 + 8;
					}
				}
				int num38 = tile3.liquid / 16;
				if (flag2 && num9 != num25)
				{
					int num39 = 2;
					if (num25 == 1)
					{
						DrawWaterfall(num4, num5, num6, alpha, new Vector2((float)(num5 * 16 - 16), (float)(num6 * 16 + 16 - num39)) - Main.screenPosition, new Rectangle(num14, 24, 32, 16 - num38 - num39), color5, (SpriteEffects)1);
					}
					else
					{
						DrawWaterfall(num4, num5, num6, alpha, new Vector2((float)(num5 * 16), (float)(num6 * 16 + 16 - num39)) - Main.screenPosition, new Rectangle(num14, 24, 32, 16 - num38 - num39), color5, (SpriteEffects)0);
					}
				}
				if (num7 == 0 && num24 != 0 && num8 == 1 && num9 != num10)
				{
					num24 = 0;
					num9 = num10;
					color5 = Color.White;
					if (num9 == 1)
					{
						DrawWaterfall(num4, num5, num6, alpha, new Vector2((float)(num5 * 16 - 16), (float)(num6 * 16 + 16)) - Main.screenPosition, new Rectangle(num14, 24, 32, 16 - num38), color5, (SpriteEffects)1);
					}
					else
					{
						DrawWaterfall(num4, num5, num6, alpha, new Vector2((float)(num5 * 16 - 16), (float)(num6 * 16 + 16)) - Main.screenPosition, new Rectangle(num14, 24, 32, 16 - num38), color5, (SpriteEffects)1);
					}
				}
				if (num11 != 0 && num26 == 0 && num27 == 1)
				{
					if (num9 == 1)
					{
						if (num13 != num4)
						{
							DrawWaterfall(num4, num5, num6, alpha, new Vector2((float)(num5 * 16), (float)(num6 * 16 + num3 + 8)) - Main.screenPosition, new Rectangle(num14, 0, 16, 16 - num38 - 8), color4, (SpriteEffects)1);
						}
						else
						{
							DrawWaterfall(num4, num5, num6, alpha, new Vector2((float)(num5 * 16), (float)(num6 * 16 + num3 + 8)) - Main.screenPosition, new Rectangle(num14, 0, 16, 16 - num38 - 8), color5, (SpriteEffects)1);
						}
					}
					else
					{
						DrawWaterfall(num4, num5, num6, alpha, new Vector2((float)(num5 * 16), (float)(num6 * 16 + num3 + 8)) - Main.screenPosition, new Rectangle(num14, 0, 16, 16 - num38 - 8), color5, (SpriteEffects)0);
					}
				}
				if (num3 == 8 && num8 == 1 && num11 == 0)
				{
					if (num10 == -1)
					{
						if (num13 != num4)
						{
							DrawWaterfall(num13, num5, num6, alpha, new Vector2((float)(num5 * 16), (float)(num6 * 16)) - Main.screenPosition, new Rectangle(num14, 24, 32, 8), color4, (SpriteEffects)0);
						}
						else
						{
							DrawWaterfall(num4, num5, num6, alpha, new Vector2((float)(num5 * 16), (float)(num6 * 16)) - Main.screenPosition, new Rectangle(num14, 24, 32, 8), color5, (SpriteEffects)0);
						}
					}
					else if (num13 != num4)
					{
						DrawWaterfall(num13, num5, num6, alpha, new Vector2((float)(num5 * 16 - 16), (float)(num6 * 16)) - Main.screenPosition, new Rectangle(num14, 24, 32, 8), color4, (SpriteEffects)1);
					}
					else
					{
						DrawWaterfall(num4, num5, num6, alpha, new Vector2((float)(num5 * 16 - 16), (float)(num6 * 16)) - Main.screenPosition, new Rectangle(num14, 24, 32, 8), color5, (SpriteEffects)1);
					}
				}
				if (num24 != 0 && num7 == 0)
				{
					if (num25 == 1)
					{
						if (num13 != num4)
						{
							DrawWaterfall(num13, num5, num6, alpha, new Vector2((float)(num5 * 16 - 16), (float)(num6 * 16)) - Main.screenPosition, new Rectangle(num14, 24, 32, 16 - num38), color4, (SpriteEffects)1);
						}
						else
						{
							DrawWaterfall(num4, num5, num6, alpha, new Vector2((float)(num5 * 16 - 16), (float)(num6 * 16)) - Main.screenPosition, new Rectangle(num14, 24, 32, 16 - num38), color5, (SpriteEffects)1);
						}
					}
					else if (num13 != num4)
					{
						DrawWaterfall(num13, num5, num6, alpha, new Vector2((float)(num5 * 16), (float)(num6 * 16)) - Main.screenPosition, new Rectangle(num14, 24, 32, 16 - num38), color4, (SpriteEffects)0);
					}
					else
					{
						DrawWaterfall(num4, num5, num6, alpha, new Vector2((float)(num5 * 16), (float)(num6 * 16)) - Main.screenPosition, new Rectangle(num14, 24, 32, 16 - num38), color5, (SpriteEffects)0);
					}
				}
				if (num27 == 1 && num24 == 0 && num11 == 0)
				{
					if (num9 == -1)
					{
						if (num8 == 0)
						{
							DrawWaterfall(num4, num5, num6, alpha, new Vector2((float)(num5 * 16), (float)(num6 * 16 + num3)) - Main.screenPosition, new Rectangle(num14, 0, 16, 16 - num38), color5, (SpriteEffects)0);
						}
						else if (num13 != num4)
						{
							DrawWaterfall(num13, num5, num6, alpha, new Vector2((float)(num5 * 16), (float)(num6 * 16)) - Main.screenPosition, new Rectangle(num14, 24, 32, 16 - num38), color4, (SpriteEffects)0);
						}
						else
						{
							DrawWaterfall(num4, num5, num6, alpha, new Vector2((float)(num5 * 16), (float)(num6 * 16)) - Main.screenPosition, new Rectangle(num14, 24, 32, 16 - num38), color5, (SpriteEffects)0);
						}
					}
					else if (num8 == 0)
					{
						DrawWaterfall(num4, num5, num6, alpha, new Vector2((float)(num5 * 16), (float)(num6 * 16 + num3)) - Main.screenPosition, new Rectangle(num14, 0, 16, 16 - num38), color5, (SpriteEffects)1);
					}
					else if (num13 != num4)
					{
						DrawWaterfall(num13, num5, num6, alpha, new Vector2((float)(num5 * 16 - 16), (float)(num6 * 16)) - Main.screenPosition, new Rectangle(num14, 24, 32, 16 - num38), color4, (SpriteEffects)1);
					}
					else
					{
						DrawWaterfall(num4, num5, num6, alpha, new Vector2((float)(num5 * 16 - 16), (float)(num6 * 16)) - Main.screenPosition, new Rectangle(num14, 24, 32, 16 - num38), color5, (SpriteEffects)1);
					}
				}
				else
				{
					switch (num26)
					{
					case 1:
						if (Main.tile[num5, num6].liquid > 0 && !Main.tile[num5, num6].halfBrick())
						{
							break;
						}
						if (num24 == 1)
						{
							for (int m = 0; m < 8; m++)
							{
								int num43 = m * 2;
								int num44 = 14 - m * 2;
								int num46 = num43;
								num3 = 8;
								if (num7 == 0 && m < 2)
								{
									num46 = 4;
								}
								DrawWaterfall(num4, num5, num6, alpha, new Vector2((float)(num5 * 16 + num43), (float)(num6 * 16 + num3 + num46)) - Main.screenPosition, new Rectangle(16 + num14 + num44, 0, 2, 16 - num3), color5, (SpriteEffects)1);
							}
						}
						else
						{
							int height2 = 16;
							if (TileID.Sets.BlocksWaterDrawingBehindSelf[Main.tile[num5, num6].type])
							{
								height2 = 8;
							}
							else if (TileID.Sets.BlocksWaterDrawingBehindSelf[Main.tile[num5, num6 + 1].type])
							{
								height2 = 8;
							}
							DrawWaterfall(num4, num5, num6, alpha, new Vector2((float)(num5 * 16), (float)(num6 * 16 + num3)) - Main.screenPosition, new Rectangle(16 + num14, 0, 16, height2), color5, (SpriteEffects)1);
						}
						break;
					case -1:
						if (Main.tile[num5, num6].liquid > 0 && !Main.tile[num5, num6].halfBrick())
						{
							break;
						}
						if (num24 == -1)
						{
							for (int l = 0; l < 8; l++)
							{
								int num40 = l * 2;
								int num41 = l * 2;
								int num42 = 14 - l * 2;
								num3 = 8;
								if (num7 == 0 && l > 5)
								{
									num42 = 4;
								}
								DrawWaterfall(num4, num5, num6, alpha, new Vector2((float)(num5 * 16 + num40), (float)(num6 * 16 + num3 + num42)) - Main.screenPosition, new Rectangle(16 + num14 + num41, 0, 2, 16 - num3), color5, (SpriteEffects)1);
							}
						}
						else
						{
							int height = 16;
							if (TileID.Sets.BlocksWaterDrawingBehindSelf[Main.tile[num5, num6].type])
							{
								height = 8;
							}
							else if (TileID.Sets.BlocksWaterDrawingBehindSelf[Main.tile[num5, num6 + 1].type])
							{
								height = 8;
							}
							DrawWaterfall(num4, num5, num6, alpha, new Vector2((float)(num5 * 16), (float)(num6 * 16 + num3)) - Main.screenPosition, new Rectangle(16 + num14, 0, 16, height), color5, (SpriteEffects)0);
						}
						break;
					case 0:
						if (num27 == 0)
						{
							if (Main.tile[num5, num6].liquid <= 0 || Main.tile[num5, num6].halfBrick())
							{
								DrawWaterfall(num4, num5, num6, alpha, new Vector2((float)(num5 * 16), (float)(num6 * 16 + num3)) - Main.screenPosition, new Rectangle(16 + num14, 0, 16, 16), color5, (SpriteEffects)0);
							}
							k = 1000;
						}
						break;
					}
				}
				if (tile3.liquid > 0 && !tile3.halfBrick())
				{
					k = 1000;
				}
				num8 = num27;
				num10 = num9;
				num7 = num26;
				num5 += num26;
				num6 += num27;
				num11 = num24;
				color4 = color5;
				if (num13 != num4)
				{
					num13 = num4;
				}
				if ((tile4.active() && (tile4.type == 189 || tile4.type == 196)) || (tile6.active() && (tile6.type == 189 || tile6.type == 196)) || (tile5.active() && (tile5.type == 189 || tile5.type == 196)))
				{
					num15 = (int)(40f * ((float)Main.maxTilesX / 4200f) * Main.gfxQuality);
				}
			}
		}
		Main.ambientWaterfallX = num34;
		Main.ambientWaterfallY = num45;
		Main.ambientWaterfallStrength = num;
		Main.ambientLavafallX = num50;
		Main.ambientLavafallY = num2;
		Main.ambientLavafallStrength = num47;
		Main.tileSolid[546] = true;
	}

	private void DrawWaterfall(int waterfallType, int x, int y, float opacity, Vector2 position, Rectangle sourceRect, Color color, SpriteEffects effects)
	{
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		Texture2D value = waterfallTexture[waterfallType].Value;
		if (waterfallType == 25)
		{
			Lighting.GetCornerColors(x, y, out var vertices);
			LiquidRenderer.SetShimmerVertexColors(ref vertices, opacity, x, y);
			Main.tileBatch.Draw(value, position + new Vector2(0f, 0f), sourceRect, vertices, default(Vector2), 1f, effects);
			sourceRect.Y += 42;
			LiquidRenderer.SetShimmerVertexColors_Sparkle(ref vertices, opacity, x, y, top: true);
			Main.tileBatch.Draw(value, position + new Vector2(0f, 0f), sourceRect, vertices, default(Vector2), 1f, effects);
		}
		else
		{
			Main.spriteBatch.Draw(value, position, (Rectangle?)sourceRect, color, 0f, default(Vector2), 1f, effects, 0f);
		}
	}

	private static Color StylizeColor(float alpha, int maxSteps, int waterfallType, int y, int s, Tile tileCache, Color aColor)
	{
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		float num = (float)(int)((Color)(ref aColor)).R * alpha;
		float num2 = (float)(int)((Color)(ref aColor)).G * alpha;
		float num3 = (float)(int)((Color)(ref aColor)).B * alpha;
		float num4 = (float)(int)((Color)(ref aColor)).A * alpha;
		switch (waterfallType)
		{
		case 1:
			if (num < 190f * alpha)
			{
				num = 190f * alpha;
			}
			if (num2 < 190f * alpha)
			{
				num2 = 190f * alpha;
			}
			if (num3 < 190f * alpha)
			{
				num3 = 190f * alpha;
			}
			break;
		case 2:
			num = (float)Main.DiscoR * alpha;
			num2 = (float)Main.DiscoG * alpha;
			num3 = (float)Main.DiscoB * alpha;
			break;
		case 15:
		case 16:
		case 17:
		case 18:
		case 19:
		case 20:
		case 21:
			num = 255f * alpha;
			num2 = 255f * alpha;
			num3 = 255f * alpha;
			break;
		}
		if (waterfallType >= 26)
		{
			LoaderManager.Get<WaterFallStylesLoader>().Get(waterfallType).ColorMultiplier(ref num, ref num2, ref num3, alpha);
		}
		((Color)(ref aColor))._002Ector((int)num, (int)num2, (int)num3, (int)num4);
		return aColor;
	}

	private static float GetAlpha(float Alpha, int maxSteps, int waterfallType, int y, int s, Tile tileCache)
	{
		float num = waterfallType switch
		{
			1 => 1f, 
			14 => 0.8f, 
			25 => 0.75f, 
			_ => (tileCache.wall != 0 || !((double)y < Main.worldSurface)) ? (0.6f * Alpha) : Alpha, 
		};
		if (s > maxSteps - 10)
		{
			num *= (float)(maxSteps - s) / 10f;
		}
		return num;
	}

	private static void TrySparkling(int x, int y, int direction, Color aColor2)
	{
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		if (((Color)(ref aColor2)).R > 20 || ((Color)(ref aColor2)).B > 20 || ((Color)(ref aColor2)).G > 20)
		{
			float num = (int)((Color)(ref aColor2)).R;
			if ((float)(int)((Color)(ref aColor2)).G > num)
			{
				num = (int)((Color)(ref aColor2)).G;
			}
			if ((float)(int)((Color)(ref aColor2)).B > num)
			{
				num = (int)((Color)(ref aColor2)).B;
			}
			if ((float)Main.rand.Next(20000) < num / 30f)
			{
				int num2 = Dust.NewDust(new Vector2((float)(x * 16 - direction * 7), (float)(y * 16 + 6)), 10, 8, 43, 0f, 0f, 254, Color.White, 0.5f);
				Dust obj = Main.dust[num2];
				obj.velocity *= 0f;
			}
		}
	}

	private static void AddLight(int waterfallType, int x, int y)
	{
		if (waterfallType >= 26)
		{
			LoaderManager.Get<WaterFallStylesLoader>().Get(waterfallType).AddLight(x, y);
		}
		switch (waterfallType)
		{
		case 1:
		{
			float r8;
			float num3 = (r8 = (0.55f + (float)(270 - Main.mouseTextColor) / 900f) * 0.4f);
			float g8 = num3 * 0.3f;
			float b8 = num3 * 0.1f;
			Lighting.AddLight(x, y, r8, g8, b8);
			break;
		}
		case 2:
		{
			float r7 = (float)Main.DiscoR / 255f;
			float g7 = (float)Main.DiscoG / 255f;
			float b7 = (float)Main.DiscoB / 255f;
			r7 *= 0.2f;
			g7 *= 0.2f;
			b7 *= 0.2f;
			Lighting.AddLight(x, y, r7, g7, b7);
			break;
		}
		case 15:
		{
			float r6 = 0f;
			float g6 = 0f;
			float b6 = 0.2f;
			Lighting.AddLight(x, y, r6, g6, b6);
			break;
		}
		case 16:
		{
			float r5 = 0f;
			float g5 = 0.2f;
			float b5 = 0f;
			Lighting.AddLight(x, y, r5, g5, b5);
			break;
		}
		case 17:
		{
			float r4 = 0f;
			float g4 = 0f;
			float b4 = 0.2f;
			Lighting.AddLight(x, y, r4, g4, b4);
			break;
		}
		case 18:
		{
			float r3 = 0f;
			float g3 = 0.2f;
			float b3 = 0f;
			Lighting.AddLight(x, y, r3, g3, b3);
			break;
		}
		case 19:
		{
			float r2 = 0.2f;
			float g2 = 0f;
			float b2 = 0f;
			Lighting.AddLight(x, y, r2, g2, b2);
			break;
		}
		case 20:
			Lighting.AddLight(x, y, 0.2f, 0.2f, 0.2f);
			break;
		case 21:
		{
			float r = 0.2f;
			float g = 0f;
			float b = 0f;
			Lighting.AddLight(x, y, r, g, b);
			break;
		}
		case 25:
		{
			float num = 0.7f;
			float num2 = 0.7f;
			num += (float)(270 - Main.mouseTextColor) / 900f;
			num2 += (float)(270 - Main.mouseTextColor) / 125f;
			Lighting.AddLight(x, y, num * 0.6f, num2 * 0.25f, num * 0.9f);
			break;
		}
		}
	}

	public void Draw(SpriteBatch spriteBatch)
	{
		for (int i = 0; i < currentMax; i++)
		{
			waterfalls[i].stopAtStep = waterfallDist;
		}
		Main.drewLava = false;
		if (Main.liquidAlpha[0] > 0f)
		{
			DrawWaterfall(0, Main.liquidAlpha[0]);
		}
		if (Main.liquidAlpha[2] > 0f)
		{
			DrawWaterfall(3, Main.liquidAlpha[2]);
		}
		if (Main.liquidAlpha[3] > 0f)
		{
			DrawWaterfall(4, Main.liquidAlpha[3]);
		}
		if (Main.liquidAlpha[4] > 0f)
		{
			DrawWaterfall(5, Main.liquidAlpha[4]);
		}
		if (Main.liquidAlpha[5] > 0f)
		{
			DrawWaterfall(6, Main.liquidAlpha[5]);
		}
		if (Main.liquidAlpha[6] > 0f)
		{
			DrawWaterfall(7, Main.liquidAlpha[6]);
		}
		if (Main.liquidAlpha[7] > 0f)
		{
			DrawWaterfall(8, Main.liquidAlpha[7]);
		}
		if (Main.liquidAlpha[8] > 0f)
		{
			DrawWaterfall(9, Main.liquidAlpha[8]);
		}
		if (Main.liquidAlpha[9] > 0f)
		{
			DrawWaterfall(10, Main.liquidAlpha[9]);
		}
		if (Main.liquidAlpha[10] > 0f)
		{
			DrawWaterfall(13, Main.liquidAlpha[10]);
		}
		if (Main.liquidAlpha[12] > 0f)
		{
			DrawWaterfall(23, Main.liquidAlpha[12]);
		}
		if (Main.liquidAlpha[13] > 0f)
		{
			DrawWaterfall(24, Main.liquidAlpha[13]);
		}
		LoaderManager.Get<WaterStylesLoader>().DrawWaterfall(this);
	}
}
