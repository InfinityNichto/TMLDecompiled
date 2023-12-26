using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Graphics;
using Terraria.ID;
using Terraria.ModLoader;

namespace Terraria.GameContent.Drawing;

public class WallDrawing
{
	private static VertexColors _glowPaintColors = new VertexColors(Color.White);

	private Tilemap _tileArray;

	private TilePaintSystemV2 _paintSystem;

	private bool _shouldShowInvisibleWalls;

	public void LerpVertexColorsWithColor(ref VertexColors colors, Color lerpColor, float percent)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		colors.TopLeftColor = Color.Lerp(colors.TopLeftColor, lerpColor, percent);
		colors.TopRightColor = Color.Lerp(colors.TopRightColor, lerpColor, percent);
		colors.BottomLeftColor = Color.Lerp(colors.BottomLeftColor, lerpColor, percent);
		colors.BottomRightColor = Color.Lerp(colors.BottomRightColor, lerpColor, percent);
	}

	public WallDrawing(TilePaintSystemV2 paintSystem)
	{
		_paintSystem = paintSystem;
	}

	public void Update()
	{
		if (!Main.dedServ)
		{
			_shouldShowInvisibleWalls = Main.ShouldShowInvisibleWalls();
		}
	}

	public void DrawWalls()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0189: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_0880: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0866: Unknown result type (might be due to invalid IL or missing references)
		//IL_024b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0250: Unknown result type (might be due to invalid IL or missing references)
		//IL_025b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0260: Unknown result type (might be due to invalid IL or missing references)
		//IL_026b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0270: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_050a: Unknown result type (might be due to invalid IL or missing references)
		//IL_051a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0525: Unknown result type (might be due to invalid IL or missing references)
		//IL_052a: Unknown result type (might be due to invalid IL or missing references)
		//IL_052c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0531: Unknown result type (might be due to invalid IL or missing references)
		//IL_0538: Unknown result type (might be due to invalid IL or missing references)
		//IL_053f: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_039e: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0400: Unknown result type (might be due to invalid IL or missing references)
		//IL_0401: Unknown result type (might be due to invalid IL or missing references)
		//IL_0403: Unknown result type (might be due to invalid IL or missing references)
		//IL_0408: Unknown result type (might be due to invalid IL or missing references)
		//IL_040a: Unknown result type (might be due to invalid IL or missing references)
		//IL_040b: Unknown result type (might be due to invalid IL or missing references)
		//IL_040d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0412: Unknown result type (might be due to invalid IL or missing references)
		//IL_0414: Unknown result type (might be due to invalid IL or missing references)
		//IL_0467: Unknown result type (might be due to invalid IL or missing references)
		//IL_0477: Unknown result type (might be due to invalid IL or missing references)
		//IL_0482: Unknown result type (might be due to invalid IL or missing references)
		//IL_0487: Unknown result type (might be due to invalid IL or missing references)
		//IL_0489: Unknown result type (might be due to invalid IL or missing references)
		//IL_048e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0497: Unknown result type (might be due to invalid IL or missing references)
		//IL_043f: Unknown result type (might be due to invalid IL or missing references)
		//IL_06cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_06db: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_06eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0706: Unknown result type (might be due to invalid IL or missing references)
		//IL_0730: Unknown result type (might be due to invalid IL or missing references)
		//IL_0741: Unknown result type (might be due to invalid IL or missing references)
		//IL_074a: Unknown result type (might be due to invalid IL or missing references)
		//IL_074f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0751: Unknown result type (might be due to invalid IL or missing references)
		//IL_075c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0766: Unknown result type (might be due to invalid IL or missing references)
		//IL_076d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0797: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_07bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_07fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0808: Unknown result type (might be due to invalid IL or missing references)
		//IL_0814: Unknown result type (might be due to invalid IL or missing references)
		//IL_0819: Unknown result type (might be due to invalid IL or missing references)
		//IL_081b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0826: Unknown result type (might be due to invalid IL or missing references)
		//IL_0830: Unknown result type (might be due to invalid IL or missing references)
		//IL_0837: Unknown result type (might be due to invalid IL or missing references)
		float gfxQuality = Main.gfxQuality;
		int offScreenRange = Main.offScreenRange;
		bool drawToScreen = Main.drawToScreen;
		Vector2 screenPosition = Main.screenPosition;
		int screenWidth = Main.screenWidth;
		int screenHeight = Main.screenHeight;
		int maxTilesX = Main.maxTilesX;
		int maxTilesY = Main.maxTilesY;
		int[] wallBlend = Main.wallBlend;
		SpriteBatch spriteBatch = Main.spriteBatch;
		TileBatch tileBatch = Main.tileBatch;
		_tileArray = Main.tile;
		Stopwatch stopwatch = new Stopwatch();
		stopwatch.Start();
		int num21 = (int)(120f * (1f - gfxQuality) + 40f * gfxQuality);
		int num13 = (int)((float)num21 * 0.4f);
		int num14 = (int)((float)num21 * 0.35f);
		int num15 = (int)((float)num21 * 0.3f);
		Vector2 vector = default(Vector2);
		((Vector2)(ref vector))._002Ector((float)offScreenRange, (float)offScreenRange);
		if (drawToScreen)
		{
			vector = Vector2.Zero;
		}
		int num16 = (int)((screenPosition.X - vector.X) / 16f - 1f);
		int num17 = (int)((screenPosition.X + (float)screenWidth + vector.X) / 16f) + 2;
		int num18 = (int)((screenPosition.Y - vector.Y) / 16f - 1f);
		int num19 = (int)((screenPosition.Y + (float)screenHeight + vector.Y) / 16f) + 5;
		int num20 = offScreenRange / 16;
		int num10 = offScreenRange / 16;
		if (num16 - num20 < 4)
		{
			num16 = num20 + 4;
		}
		if (num17 + num20 > maxTilesX - 4)
		{
			num17 = maxTilesX - num20 - 4;
		}
		if (num18 - num10 < 4)
		{
			num18 = num10 + 4;
		}
		if (num19 + num10 > maxTilesY - 4)
		{
			num19 = maxTilesY - num10 - 4;
		}
		VertexColors vertices = default(VertexColors);
		Rectangle value = default(Rectangle);
		((Rectangle)(ref value))._002Ector(0, 0, 32, 32);
		int underworldLayer = Main.UnderworldLayer;
		Point screenOverdrawOffset = Main.GetScreenOverdrawOffset();
		for (int j = num16 - num20 + screenOverdrawOffset.X; j < num17 + num20 - screenOverdrawOffset.X; j++)
		{
			for (int i = num18 - num10 + screenOverdrawOffset.Y; i < num19 + num10 - screenOverdrawOffset.Y; i++)
			{
				Tile tile = _tileArray[j, i];
				if (tile == null)
				{
					tile = default(Tile);
					_tileArray[j, i] = tile;
				}
				ushort wall = tile.wall;
				if (wall <= 0 || FullTile(j, i) || (wall == 318 && !_shouldShowInvisibleWalls) || (tile.invisibleWall() && !_shouldShowInvisibleWalls))
				{
					continue;
				}
				if (WallLoader.PreDraw(j, i, wall, spriteBatch))
				{
					Color color = Lighting.GetColor(j, i);
					if (tile.fullbrightWall())
					{
						color = Color.White;
					}
					if (wall == 318)
					{
						color = Color.White;
					}
					if (((Color)(ref color)).R == 0 && ((Color)(ref color)).G == 0 && ((Color)(ref color)).B == 0 && i < underworldLayer)
					{
						continue;
					}
					Main.instance.LoadWall(wall);
					value.X = tile.wallFrameX();
					value.Y = tile.wallFrameY() + Main.wallFrame[wall] * 180;
					if ((uint)(tile.wall - 242) <= 1u)
					{
						int num11 = 20;
						int num12 = (Main.wallFrameCounter[wall] + j * 11 + i * 27) % (num11 * 8);
						value.Y = tile.wallFrameY() + 180 * (num12 / num11);
					}
					if (Lighting.NotRetro && !Main.wallLight[wall] && tile.wall != 241 && (tile.wall < 88 || tile.wall > 93) && !WorldGen.SolidTile(tile))
					{
						Texture2D tileDrawTexture = GetTileDrawTexture(tile, j, i);
						if (tile.wall == 346)
						{
							vertices.TopRightColor = (vertices.TopLeftColor = (vertices.BottomRightColor = (vertices.BottomLeftColor = new Color((int)(byte)Main.DiscoR, (int)(byte)Main.DiscoG, (int)(byte)Main.DiscoB))));
						}
						else if (tile.wall == 44)
						{
							vertices.TopRightColor = (vertices.TopLeftColor = (vertices.BottomRightColor = (vertices.BottomLeftColor = new Color((int)(byte)Main.DiscoR, (int)(byte)Main.DiscoG, (int)(byte)Main.DiscoB))));
						}
						else
						{
							Lighting.GetCornerColors(j, i, out vertices);
							if ((uint)(tile.wall - 341) <= 4u)
							{
								LerpVertexColorsWithColor(ref vertices, Color.White, 0.5f);
							}
							if (tile.fullbrightWall())
							{
								vertices = _glowPaintColors;
							}
						}
						tileBatch.Draw(tileDrawTexture, new Vector2((float)(j * 16 - (int)screenPosition.X - 8), (float)(i * 16 - (int)screenPosition.Y - 8)) + vector, value, vertices, Vector2.Zero, 1f, (SpriteEffects)0);
					}
					else
					{
						Color color2 = color;
						if (wall == 44 || wall == 346)
						{
							((Color)(ref color2))._002Ector(Main.DiscoR, Main.DiscoG, Main.DiscoB);
						}
						if ((uint)(wall - 341) <= 4u)
						{
							color2 = Color.Lerp(color2, Color.White, 0.5f);
						}
						Texture2D tileDrawTexture2 = GetTileDrawTexture(tile, j, i);
						spriteBatch.Draw(tileDrawTexture2, new Vector2((float)(j * 16 - (int)screenPosition.X - 8), (float)(i * 16 - (int)screenPosition.Y - 8)) + vector, (Rectangle?)value, color2, 0f, Vector2.Zero, 1f, (SpriteEffects)0, 0f);
					}
					if (((Color)(ref color)).R > num13 || ((Color)(ref color)).G > num14 || ((Color)(ref color)).B > num15)
					{
						bool num22 = _tileArray[j - 1, i].wall > 0 && wallBlend[_tileArray[j - 1, i].wall] != wallBlend[tile.wall];
						bool flag = _tileArray[j + 1, i].wall > 0 && wallBlend[_tileArray[j + 1, i].wall] != wallBlend[tile.wall];
						bool flag2 = _tileArray[j, i - 1].wall > 0 && wallBlend[_tileArray[j, i - 1].wall] != wallBlend[tile.wall];
						bool flag3 = _tileArray[j, i + 1].wall > 0 && wallBlend[_tileArray[j, i + 1].wall] != wallBlend[tile.wall];
						if (num22)
						{
							spriteBatch.Draw(TextureAssets.WallOutline.Value, new Vector2((float)(j * 16 - (int)screenPosition.X), (float)(i * 16 - (int)screenPosition.Y)) + vector, (Rectangle?)new Rectangle(0, 0, 2, 16), color, 0f, Vector2.Zero, 1f, (SpriteEffects)0, 0f);
						}
						if (flag)
						{
							spriteBatch.Draw(TextureAssets.WallOutline.Value, new Vector2((float)(j * 16 - (int)screenPosition.X + 14), (float)(i * 16 - (int)screenPosition.Y)) + vector, (Rectangle?)new Rectangle(14, 0, 2, 16), color, 0f, Vector2.Zero, 1f, (SpriteEffects)0, 0f);
						}
						if (flag2)
						{
							spriteBatch.Draw(TextureAssets.WallOutline.Value, new Vector2((float)(j * 16 - (int)screenPosition.X), (float)(i * 16 - (int)screenPosition.Y)) + vector, (Rectangle?)new Rectangle(0, 0, 16, 2), color, 0f, Vector2.Zero, 1f, (SpriteEffects)0, 0f);
						}
						if (flag3)
						{
							spriteBatch.Draw(TextureAssets.WallOutline.Value, new Vector2((float)(j * 16 - (int)screenPosition.X), (float)(i * 16 - (int)screenPosition.Y + 14)) + vector, (Rectangle?)new Rectangle(0, 14, 16, 2), color, 0f, Vector2.Zero, 1f, (SpriteEffects)0, 0f);
						}
					}
				}
				WallLoader.PostDraw(j, i, wall, spriteBatch);
			}
		}
		Main.instance.DrawTileCracks(2, Main.LocalPlayer.hitReplace);
		Main.instance.DrawTileCracks(2, Main.LocalPlayer.hitTile);
		TimeLogger.DrawTime(2, stopwatch.Elapsed.TotalMilliseconds);
	}

	private Texture2D GetTileDrawTexture(Tile tile, int tileX, int tileY)
	{
		Texture2D result = TextureAssets.Wall[tile.wall].Value;
		int wall = tile.wall;
		Texture2D texture2D = _paintSystem.TryGetWallAndRequestIfNotReady(wall, tile.wallColor());
		if (texture2D != null)
		{
			result = texture2D;
		}
		return result;
	}

	protected bool FullTile(int x, int y)
	{
		if (_tileArray[x - 1, y] == null || _tileArray[x - 1, y].blockType() != 0 || _tileArray[x + 1, y] == null || _tileArray[x + 1, y].blockType() != 0)
		{
			return false;
		}
		Tile tile = _tileArray[x, y];
		if (tile == null)
		{
			return false;
		}
		if (tile.active())
		{
			if (tile.type < TileID.Sets.DrawsWalls.Length && TileID.Sets.DrawsWalls[tile.type])
			{
				return false;
			}
			if (tile.invisibleBlock() && !_shouldShowInvisibleWalls)
			{
				return false;
			}
			if (Main.tileSolid[tile.type] && !Main.tileSolidTop[tile.type])
			{
				int frameX = tile.frameX;
				int frameY = tile.frameY;
				if (Main.tileLargeFrames[tile.type] > 0)
				{
					if (frameY == 18 || frameY == 108)
					{
						if (frameX >= 18 && frameX <= 54)
						{
							return true;
						}
						if (frameX >= 108 && frameX <= 144)
						{
							return true;
						}
					}
				}
				else if (frameY == 18)
				{
					if (frameX >= 18 && frameX <= 54)
					{
						return true;
					}
					if (frameX >= 108 && frameX <= 144)
					{
						return true;
					}
				}
				else if (frameY >= 90 && frameY <= 196)
				{
					if (frameX <= 70)
					{
						return true;
					}
					if (frameX >= 144 && frameX <= 232)
					{
						return true;
					}
				}
			}
		}
		return false;
	}
}
