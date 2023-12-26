using System;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Shaders;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace Terraria;

public class Gore
{
	public static int goreTime = 600;

	public Vector2 position;

	public Vector2 velocity;

	public float rotation;

	public float scale;

	public int alpha;

	public int type;

	public float light;

	public bool active;

	public bool sticky = true;

	public int timeLeft = goreTime;

	public bool behindTiles;

	public byte frameCounter;

	public SpriteFrame Frame = new SpriteFrame(1, 1);

	public Vector2 drawOffset;

	internal int realType;

	public float Width
	{
		get
		{
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			if (TextureAssets.Gore[type].IsLoaded)
			{
				return scale * (float)Frame.GetSourceRectangle(TextureAssets.Gore[type].Value).Width;
			}
			return 1f;
		}
	}

	public float Height
	{
		get
		{
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			if (TextureAssets.Gore[type].IsLoaded)
			{
				return scale * (float)Frame.GetSourceRectangle(TextureAssets.Gore[type].Value).Height;
			}
			return 1f;
		}
	}

	public Rectangle AABBRectangle
	{
		get
		{
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			if (TextureAssets.Gore[type].IsLoaded)
			{
				Rectangle sourceRectangle = Frame.GetSourceRectangle(TextureAssets.Gore[type].Value);
				return new Rectangle((int)position.X, (int)position.Y, (int)((float)sourceRectangle.Width * scale), (int)((float)sourceRectangle.Height * scale));
			}
			return new Rectangle(0, 0, 1, 1);
		}
	}

	[Old("Please use Frame instead.")]
	public byte frame
	{
		get
		{
			return Frame.CurrentRow;
		}
		set
		{
			Frame.CurrentRow = value;
		}
	}

	[Old("Please use Frame instead.")]
	public byte numFrames
	{
		get
		{
			return Frame.RowCount;
		}
		set
		{
			SpriteFrame spriteFrame = new SpriteFrame(Frame.ColumnCount, value);
			spriteFrame.CurrentColumn = Frame.CurrentColumn;
			spriteFrame.CurrentRow = Frame.CurrentRow;
			SpriteFrame spriteFrame2 = spriteFrame;
			Frame = spriteFrame2;
		}
	}

	public ModGore ModGore { get; private set; }

	private void UpdateAmbientFloorCloud()
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		timeLeft -= GoreID.Sets.DisappearSpeed[type];
		if (timeLeft <= 0)
		{
			active = false;
			return;
		}
		bool flag = false;
		Point point = (position + new Vector2(15f, 0f)).ToTileCoordinates();
		Tile tile = Main.tile[point.X, point.Y];
		Tile tile2 = Main.tile[point.X, point.Y + 1];
		Tile tile3 = Main.tile[point.X, point.Y + 2];
		if (tile == null || tile2 == null || tile3 == null)
		{
			active = false;
			return;
		}
		if (WorldGen.SolidTile(tile) || (!WorldGen.SolidTile(tile2) && !WorldGen.SolidTile(tile3)))
		{
			flag = true;
		}
		if (timeLeft <= 30)
		{
			flag = true;
		}
		velocity.X = 0.4f * Main.WindForVisuals;
		if (!flag)
		{
			if (alpha > 220)
			{
				alpha--;
			}
		}
		else
		{
			alpha++;
			if (alpha >= 255)
			{
				active = false;
				return;
			}
		}
		position += velocity;
	}

	private void UpdateAmbientAirborneCloud()
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		timeLeft -= GoreID.Sets.DisappearSpeed[type];
		if (timeLeft <= 0)
		{
			active = false;
			return;
		}
		bool flag = false;
		Point point = (position + new Vector2(15f, 0f)).ToTileCoordinates();
		rotation = velocity.ToRotation();
		Tile tile = Main.tile[point.X, point.Y];
		if (tile == null)
		{
			active = false;
			return;
		}
		if (WorldGen.SolidTile(tile))
		{
			flag = true;
		}
		if (timeLeft <= 60)
		{
			flag = true;
		}
		if (!flag)
		{
			if (alpha > 240 && Main.rand.Next(5) == 0)
			{
				alpha--;
			}
		}
		else
		{
			if (Main.rand.Next(5) == 0)
			{
				alpha++;
			}
			if (alpha >= 255)
			{
				active = false;
				return;
			}
		}
		position += velocity;
	}

	private void UpdateFogMachineCloud()
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		timeLeft -= GoreID.Sets.DisappearSpeed[type];
		if (timeLeft <= 0)
		{
			active = false;
			return;
		}
		bool flag = false;
		Point point = (position + new Vector2(15f, 0f)).ToTileCoordinates();
		if (WorldGen.SolidTile(Main.tile[point.X, point.Y]))
		{
			flag = true;
		}
		if (timeLeft <= 240)
		{
			flag = true;
		}
		if (!flag)
		{
			if (alpha > 225 && Main.rand.Next(2) == 0)
			{
				alpha--;
			}
		}
		else
		{
			if (Main.rand.Next(2) == 0)
			{
				alpha++;
			}
			if (alpha >= 255)
			{
				active = false;
				return;
			}
		}
		position += velocity;
	}

	private void UpdateLightningBunnySparks()
	{
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		if (frameCounter == 0)
		{
			frameCounter = 1;
			Frame.CurrentRow = (byte)Main.rand.Next(3);
		}
		timeLeft -= GoreID.Sets.DisappearSpeed[type];
		if (timeLeft <= 0)
		{
			active = false;
			return;
		}
		alpha = (int)MathHelper.Lerp(255f, 0f, (float)timeLeft / 15f);
		float num = (255f - (float)alpha) / 255f;
		num *= scale;
		Lighting.AddLight(position + new Vector2(Width / 2f, Height / 2f), num * 0.4f, num, num);
		position += velocity;
	}

	private float ChumFloatingChunk_GetWaterLine(int X, int Y)
	{
		float result = position.Y + Height;
		if (Main.tile[X, Y - 1] == null)
		{
			Main.tile[X, Y - 1] = default(Tile);
		}
		if (Main.tile[X, Y] == null)
		{
			Main.tile[X, Y] = default(Tile);
		}
		if (Main.tile[X, Y + 1] == null)
		{
			Main.tile[X, Y + 1] = default(Tile);
		}
		if (Main.tile[X, Y - 1].liquid > 0)
		{
			result = Y * 16;
			result -= (float)(Main.tile[X, Y - 1].liquid / 16);
		}
		else if (Main.tile[X, Y].liquid > 0)
		{
			result = (Y + 1) * 16;
			result -= (float)(Main.tile[X, Y].liquid / 16);
		}
		else if (Main.tile[X, Y + 1].liquid > 0)
		{
			result = (Y + 2) * 16;
			result -= (float)(Main.tile[X, Y + 1].liquid / 16);
		}
		return result;
	}

	private bool DeactivateIfOutsideOfWorld()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		Point point = position.ToTileCoordinates();
		if (!WorldGen.InWorld(point.X, point.Y))
		{
			active = false;
			return true;
		}
		if (Main.tile[point.X, point.Y] == null)
		{
			active = false;
			return true;
		}
		return false;
	}

	public void Update()
	{
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_06cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_2170: Unknown result type (might be due to invalid IL or missing references)
		//IL_2176: Unknown result type (might be due to invalid IL or missing references)
		//IL_217b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2180: Unknown result type (might be due to invalid IL or missing references)
		//IL_20dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_20e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_20f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_20f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_20fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_2110: Unknown result type (might be due to invalid IL or missing references)
		//IL_2122: Unknown result type (might be due to invalid IL or missing references)
		//IL_2134: Unknown result type (might be due to invalid IL or missing references)
		//IL_1849: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c67: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c6c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c70: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c76: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c82: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c87: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c8d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c92: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d46: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d4c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d51: Unknown result type (might be due to invalid IL or missing references)
		//IL_1826: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c42: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c51: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c56: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e0a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e19: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e1e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f50: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f58: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f5e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f7c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f81: Unknown result type (might be due to invalid IL or missing references)
		//IL_19fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a14: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a1a: Unknown result type (might be due to invalid IL or missing references)
		//IL_14b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e9a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ea4: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ea9: Unknown result type (might be due to invalid IL or missing references)
		//IL_15fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_1616: Unknown result type (might be due to invalid IL or missing references)
		//IL_161c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1633: Unknown result type (might be due to invalid IL or missing references)
		//IL_1646: Unknown result type (might be due to invalid IL or missing references)
		//IL_164b: Unknown result type (might be due to invalid IL or missing references)
		//IL_047b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0481: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_04bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a75: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a86: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a90: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a95: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a9a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a9c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aa9: Unknown result type (might be due to invalid IL or missing references)
		//IL_09b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_09c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_09c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_09fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a08: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a0d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b5f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b6c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a25: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a2f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a34: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c2b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c3b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c57: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cce: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c98: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cb7: Unknown result type (might be due to invalid IL or missing references)
		//IL_1143: Unknown result type (might be due to invalid IL or missing references)
		//IL_1152: Unknown result type (might be due to invalid IL or missing references)
		//IL_1157: Unknown result type (might be due to invalid IL or missing references)
		//IL_116d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1173: Unknown result type (might be due to invalid IL or missing references)
		//IL_11ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_11b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d1e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d23: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d2d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d32: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d37: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d5a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d69: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d73: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d78: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d87: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d8c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d91: Unknown result type (might be due to invalid IL or missing references)
		if (Main.netMode == 2 || !active || (ModGore != null && !ModGore.Update(this)))
		{
			return;
		}
		GoreLoader.SetupUpdateType(this);
		if (sticky)
		{
			if (DeactivateIfOutsideOfWorld())
			{
				return;
			}
			float num = ((Vector2)(ref velocity)).Length();
			if (num > 32f)
			{
				velocity *= 32f / num;
			}
		}
		switch (GoreID.Sets.SpecialAI[type])
		{
		case 4:
			UpdateAmbientFloorCloud();
			return;
		case 5:
			UpdateAmbientAirborneCloud();
			return;
		case 6:
			UpdateFogMachineCloud();
			return;
		case 7:
			UpdateLightningBunnySparks();
			return;
		}
		if ((type == 1217 || type == 1218) && frameCounter == 0)
		{
			frameCounter = 1;
			Frame.CurrentRow = (byte)Main.rand.Next(3);
		}
		bool flag = type >= 1024 && type <= 1026;
		if (type >= 276 && type <= 282)
		{
			velocity.X *= 0.98f;
			velocity.Y *= 0.98f;
			if (velocity.Y < scale)
			{
				velocity.Y += 0.05f;
			}
			if ((double)velocity.Y > 0.1)
			{
				if (velocity.X > 0f)
				{
					rotation += 0.01f;
				}
				else
				{
					rotation -= 0.01f;
				}
			}
		}
		if (type >= 570 && type <= 572)
		{
			scale -= 0.001f;
			if ((double)scale <= 0.01)
			{
				scale = 0.01f;
				timeLeft = 0;
			}
			sticky = false;
			rotation = velocity.X * 0.1f;
		}
		else if ((type >= 706 && type <= 717) || type == 943 || type == 1147 || (type >= 1160 && type <= 1162))
		{
			if (type == 943 || (type >= 1160 && type <= 1162))
			{
				alpha = 0;
			}
			else if ((double)position.Y < Main.worldSurface * 16.0 + 8.0)
			{
				alpha = 0;
			}
			else
			{
				alpha = 100;
			}
			int num12 = 4;
			frameCounter++;
			if (frame <= 4)
			{
				int num23 = (int)(position.X / 16f);
				int num34 = (int)(position.Y / 16f) - 1;
				if (WorldGen.InWorld(num23, num34) && !Main.tile[num23, num34].active())
				{
					active = false;
				}
				if (frame == 0)
				{
					num12 = 24 + Main.rand.Next(256);
				}
				if (frame == 1)
				{
					num12 = 24 + Main.rand.Next(256);
				}
				if (frame == 2)
				{
					num12 = 24 + Main.rand.Next(256);
				}
				if (frame == 3)
				{
					num12 = 24 + Main.rand.Next(96);
				}
				if (frame == 5)
				{
					num12 = 16 + Main.rand.Next(64);
				}
				if (type == 716)
				{
					num12 *= 2;
				}
				if (type == 717)
				{
					num12 *= 4;
				}
				if ((type == 943 || (type >= 1160 && type <= 1162)) && frame < 6)
				{
					num12 = 4;
				}
				if (frameCounter >= num12)
				{
					frameCounter = 0;
					frame++;
					if (frame == 5)
					{
						int num36 = NewGore(position, velocity, type);
						Main.gore[num36].frame = 9;
						Gore obj = Main.gore[num36];
						obj.velocity *= 0f;
					}
				}
			}
			else if (frame <= 6)
			{
				num12 = 8;
				if (type == 716)
				{
					num12 *= 2;
				}
				if (type == 717)
				{
					num12 *= 3;
				}
				if (frameCounter >= num12)
				{
					frameCounter = 0;
					frame++;
					if (frame == 7)
					{
						active = false;
					}
				}
			}
			else if (frame <= 9)
			{
				num12 = 6;
				if (type == 716)
				{
					num12 = (int)((double)num12 * 1.5);
					velocity.Y += 0.175f;
				}
				else if (type == 717)
				{
					num12 *= 2;
					velocity.Y += 0.15f;
				}
				else if (type == 943)
				{
					num12 = (int)((double)num12 * 1.5);
					velocity.Y += 0.2f;
				}
				else
				{
					velocity.Y += 0.2f;
				}
				if ((double)velocity.Y < 0.5)
				{
					velocity.Y = 0.5f;
				}
				if (velocity.Y > 12f)
				{
					velocity.Y = 12f;
				}
				if (frameCounter >= num12)
				{
					frameCounter = 0;
					frame++;
				}
				if (frame > 9)
				{
					frame = 7;
				}
			}
			else
			{
				if (type == 716)
				{
					num12 *= 2;
				}
				else if (type == 717)
				{
					num12 *= 6;
				}
				velocity.Y += 0.1f;
				if (frameCounter >= num12)
				{
					frameCounter = 0;
					frame++;
				}
				velocity *= 0f;
				if (frame > 14)
				{
					active = false;
				}
			}
		}
		else if (type == 11 || type == 12 || type == 13 || type == 61 || type == 62 || type == 63 || type == 99 || type == 220 || type == 221 || type == 222 || (type >= 375 && type <= 377) || (type >= 435 && type <= 437) || (type >= 861 && type <= 862))
		{
			velocity.Y *= 0.98f;
			velocity.X *= 0.98f;
			scale -= 0.007f;
			if ((double)scale < 0.1)
			{
				scale = 0.1f;
				alpha = 255;
			}
		}
		else if (type == 16 || type == 17)
		{
			velocity.Y *= 0.98f;
			velocity.X *= 0.98f;
			scale -= 0.01f;
			if ((double)scale < 0.1)
			{
				scale = 0.1f;
				alpha = 255;
			}
		}
		else if (type == 1201)
		{
			if (frameCounter == 0)
			{
				frameCounter = 1;
				Frame.CurrentRow = (byte)Main.rand.Next(4);
			}
			scale -= 0.002f;
			if ((double)scale < 0.1)
			{
				scale = 0.1f;
				alpha = 255;
			}
			rotation += velocity.X * 0.1f;
			int num37 = (int)(position.X + 6f) / 16;
			int num38 = (int)(position.Y - 6f) / 16;
			if (Main.tile[num37, num38] == null || Main.tile[num37, num38].liquid <= 0)
			{
				velocity.Y += 0.2f;
				if (velocity.Y < 0f)
				{
					velocity *= 0.92f;
				}
			}
			else
			{
				velocity.Y += 0.005f;
				float num39 = ((Vector2)(ref velocity)).Length();
				if (num39 > 1f)
				{
					velocity *= 0.1f;
				}
				else if (num39 > 0.1f)
				{
					velocity *= 0.98f;
				}
			}
		}
		else if (type == 1208)
		{
			if (frameCounter == 0)
			{
				frameCounter = 1;
				Frame.CurrentRow = (byte)Main.rand.Next(4);
			}
			Vector2 vector = position + new Vector2(Width, Height) / 2f;
			int num40 = (int)vector.X / 16;
			int num2 = (int)vector.Y / 16;
			bool flag2 = Main.tile[num40, num2] != null && Main.tile[num40, num2].liquid > 0;
			scale -= 0.0005f;
			if ((double)scale < 0.1)
			{
				scale = 0.1f;
				alpha = 255;
			}
			rotation += velocity.X * 0.1f;
			if (flag2)
			{
				velocity.X *= 0.9f;
				int num3 = (int)vector.X / 16;
				int num4 = (int)(vector.Y / 16f);
				_ = position.Y / 16f;
				int num5 = (int)((position.Y + Height) / 16f);
				if (Main.tile[num3, num4] == null)
				{
					Main.tile[num3, num4] = default(Tile);
				}
				if (Main.tile[num3, num5] == null)
				{
					Main.tile[num3, num5] = default(Tile);
				}
				if (velocity.Y > 0f)
				{
					velocity.Y *= 0.5f;
				}
				num3 = (int)(vector.X / 16f);
				num4 = (int)(vector.Y / 16f);
				float num6 = ChumFloatingChunk_GetWaterLine(num3, num4);
				if (vector.Y > num6)
				{
					velocity.Y -= 0.1f;
					if (velocity.Y < -8f)
					{
						velocity.Y = -8f;
					}
					if (vector.Y + velocity.Y < num6)
					{
						velocity.Y = num6 - vector.Y;
					}
				}
				else
				{
					velocity.Y = num6 - vector.Y;
				}
				bool flag3 = !flag2 && ((Vector2)(ref velocity)).Length() < 0.8f;
				int maxValue = (flag2 ? 270 : 15);
				if (Main.rand.Next(maxValue) == 0 && !flag3)
				{
					Gore gore = NewGoreDirect(position + Vector2.UnitY * 6f, Vector2.Zero, 1201, scale * 0.7f);
					if (flag2)
					{
						gore.velocity = Vector2.UnitX * Main.rand.NextFloatDirection() * 0.5f + Vector2.UnitY * Main.rand.NextFloat();
					}
					else if (gore.velocity.Y < 0f)
					{
						gore.velocity.Y = 0f - gore.velocity.Y;
					}
				}
			}
			else
			{
				if (velocity.Y == 0f)
				{
					velocity.X *= 0.95f;
				}
				velocity.X *= 0.98f;
				velocity.Y += 0.3f;
				if (velocity.Y > 15.9f)
				{
					velocity.Y = 15.9f;
				}
			}
		}
		else if (type == 331)
		{
			alpha += 5;
			velocity.Y *= 0.95f;
			velocity.X *= 0.95f;
			rotation = velocity.X * 0.1f;
		}
		else if (GoreID.Sets.SpecialAI[type] == 3)
		{
			if (++frameCounter >= 8 && velocity.Y > 0.2f)
			{
				frameCounter = 0;
				int num7 = Frame.CurrentRow / 4;
				if (++Frame.CurrentRow >= 4 + num7 * 4)
				{
					Frame.CurrentRow = (byte)(num7 * 4);
				}
			}
		}
		else if (GoreID.Sets.SpecialAI[type] != 1 && GoreID.Sets.SpecialAI[type] != 2)
		{
			if (type >= 907 && type <= 909)
			{
				rotation = 0f;
				velocity.X *= 0.98f;
				if (velocity.Y > 0f && velocity.Y < 0.001f)
				{
					velocity.Y = -0.5f + Main.rand.NextFloat() * -3f;
				}
				if (velocity.Y > -1f)
				{
					velocity.Y -= 0.1f;
				}
				if (scale < 1f)
				{
					scale += 0.1f;
				}
				if (++frameCounter >= 8)
				{
					frameCounter = 0;
					if (++frame >= 3)
					{
						frame = 0;
					}
				}
			}
			else if (type == 1218)
			{
				if (timeLeft > 8)
				{
					timeLeft = 8;
				}
				velocity.X *= 0.95f;
				if (Math.Abs(velocity.X) <= 0.1f)
				{
					velocity.X = 0f;
				}
				if (alpha < 100 && ((Vector2)(ref velocity)).Length() > 0f && Main.rand.Next(5) == 0)
				{
					int num8 = 246;
					switch (Frame.CurrentRow)
					{
					case 0:
						num8 = 246;
						break;
					case 1:
						num8 = 245;
						break;
					case 2:
						num8 = 244;
						break;
					}
					int num9 = Dust.NewDust(position + new Vector2(6f, 4f), 4, 4, num8);
					Main.dust[num9].alpha = 255;
					Main.dust[num9].scale = 0.8f;
					Main.dust[num9].velocity = Vector2.Zero;
				}
				velocity.Y += 0.2f;
				rotation = 0f;
			}
			else if (type < 411 || type > 430)
			{
				velocity.Y += 0.2f;
				rotation += velocity.X * 0.05f;
			}
			else if (GoreID.Sets.SpecialAI[type] != 3)
			{
				rotation += velocity.X * 0.1f;
			}
		}
		if (type >= 580 && type <= 582)
		{
			rotation = 0f;
			velocity.X *= 0.95f;
		}
		if (GoreID.Sets.SpecialAI[type] == 2)
		{
			if (timeLeft < 60)
			{
				alpha += Main.rand.Next(1, 7);
			}
			else if (alpha > 100)
			{
				alpha -= Main.rand.Next(1, 4);
			}
			if (alpha < 0)
			{
				alpha = 0;
			}
			if (alpha > 255)
			{
				timeLeft = 0;
			}
			velocity.X = (velocity.X * 50f + Main.WindForVisuals * 2f + (float)Main.rand.Next(-10, 11) * 0.1f) / 51f;
			float num10 = 0f;
			if (velocity.X < 0f)
			{
				num10 = velocity.X * 0.2f;
			}
			velocity.Y = (velocity.Y * 50f + -0.35f + num10 + (float)Main.rand.Next(-10, 11) * 0.2f) / 51f;
			rotation = velocity.X * 0.6f;
			float num11 = -1f;
			if (TextureAssets.Gore[type].IsLoaded)
			{
				Rectangle rectangle = default(Rectangle);
				((Rectangle)(ref rectangle))._002Ector((int)position.X, (int)position.Y, (int)((float)TextureAssets.Gore[type].Width() * scale), (int)((float)TextureAssets.Gore[type].Height() * scale));
				Rectangle value = default(Rectangle);
				for (int i = 0; i < 255; i++)
				{
					if (Main.player[i].active && !Main.player[i].dead)
					{
						((Rectangle)(ref value))._002Ector((int)Main.player[i].position.X, (int)Main.player[i].position.Y, Main.player[i].width, Main.player[i].height);
						if (((Rectangle)(ref rectangle)).Intersects(value))
						{
							timeLeft = 0;
							num11 = ((Vector2)(ref Main.player[i].velocity)).Length();
							break;
						}
					}
				}
			}
			if (timeLeft > 0)
			{
				if (Main.rand.Next(2) == 0)
				{
					timeLeft--;
				}
				if (Main.rand.Next(50) == 0)
				{
					timeLeft -= 5;
				}
				if (Main.rand.Next(100) == 0)
				{
					timeLeft -= 10;
				}
			}
			else
			{
				alpha = 255;
				if (TextureAssets.Gore[type].IsLoaded && num11 != -1f)
				{
					float num13 = (float)TextureAssets.Gore[type].Width() * scale * 0.8f;
					float x = position.X;
					float y = position.Y;
					float num14 = (float)TextureAssets.Gore[type].Width() * scale;
					float num15 = (float)TextureAssets.Gore[type].Height() * scale;
					int num16 = 31;
					for (int j = 0; (float)j < num13; j++)
					{
						int num17 = Dust.NewDust(new Vector2(x, y), (int)num14, (int)num15, num16);
						Dust obj2 = Main.dust[num17];
						obj2.velocity *= (1f + num11) / 3f;
						Main.dust[num17].noGravity = true;
						Main.dust[num17].alpha = 100;
						Main.dust[num17].scale = scale;
					}
				}
			}
		}
		if (type >= 411 && type <= 430)
		{
			alpha = 50;
			velocity.X = (velocity.X * 50f + Main.WindForVisuals * 2f + (float)Main.rand.Next(-10, 11) * 0.1f) / 51f;
			velocity.Y = (velocity.Y * 50f + -0.25f + (float)Main.rand.Next(-10, 11) * 0.2f) / 51f;
			rotation = velocity.X * 0.3f;
			if (TextureAssets.Gore[type].IsLoaded)
			{
				Rectangle rectangle2 = default(Rectangle);
				((Rectangle)(ref rectangle2))._002Ector((int)position.X, (int)position.Y, (int)((float)TextureAssets.Gore[type].Width() * scale), (int)((float)TextureAssets.Gore[type].Height() * scale));
				Rectangle value2 = default(Rectangle);
				for (int k = 0; k < 255; k++)
				{
					if (Main.player[k].active && !Main.player[k].dead)
					{
						((Rectangle)(ref value2))._002Ector((int)Main.player[k].position.X, (int)Main.player[k].position.Y, Main.player[k].width, Main.player[k].height);
						if (((Rectangle)(ref rectangle2)).Intersects(value2))
						{
							timeLeft = 0;
						}
					}
				}
				if (Collision.SolidCollision(position, (int)((float)TextureAssets.Gore[type].Width() * scale), (int)((float)TextureAssets.Gore[type].Height() * scale)))
				{
					timeLeft = 0;
				}
			}
			if (timeLeft > 0)
			{
				if (Main.rand.Next(2) == 0)
				{
					timeLeft--;
				}
				if (Main.rand.Next(50) == 0)
				{
					timeLeft -= 5;
				}
				if (Main.rand.Next(100) == 0)
				{
					timeLeft -= 10;
				}
			}
			else
			{
				alpha = 255;
				if (TextureAssets.Gore[type].IsLoaded)
				{
					float num18 = (float)TextureAssets.Gore[type].Width() * scale * 0.8f;
					float x2 = position.X;
					float y2 = position.Y;
					float num19 = (float)TextureAssets.Gore[type].Width() * scale;
					float num20 = (float)TextureAssets.Gore[type].Height() * scale;
					int num21 = 176;
					if (type >= 416 && type <= 420)
					{
						num21 = 177;
					}
					if (type >= 421 && type <= 425)
					{
						num21 = 178;
					}
					if (type >= 426 && type <= 430)
					{
						num21 = 179;
					}
					for (int l = 0; (float)l < num18; l++)
					{
						int num22 = Dust.NewDust(new Vector2(x2, y2), (int)num19, (int)num20, num21);
						Main.dust[num22].noGravity = true;
						Main.dust[num22].alpha = 100;
						Main.dust[num22].scale = scale;
					}
				}
			}
		}
		else if (GoreID.Sets.SpecialAI[type] != 3 && GoreID.Sets.SpecialAI[type] != 1)
		{
			if ((type >= 706 && type <= 717) || type == 943 || type == 1147 || (type >= 1160 && type <= 1162))
			{
				if (type == 716)
				{
					float num24 = 1f;
					float num25 = 1f;
					float num26 = 1f;
					float num27 = 0.6f;
					num27 = ((frame == 0) ? (num27 * 0.1f) : ((frame == 1) ? (num27 * 0.2f) : ((frame == 2) ? (num27 * 0.3f) : ((frame == 3) ? (num27 * 0.4f) : ((frame == 4) ? (num27 * 0.5f) : ((frame == 5) ? (num27 * 0.4f) : ((frame == 6) ? (num27 * 0.2f) : ((frame <= 9) ? (num27 * 0.5f) : ((frame == 10) ? (num27 * 0.5f) : ((frame == 11) ? (num27 * 0.4f) : ((frame == 12) ? (num27 * 0.3f) : ((frame == 13) ? (num27 * 0.2f) : ((frame != 14) ? 0f : (num27 * 0.1f))))))))))))));
					num24 = 1f * num27;
					num25 = 0.5f * num27;
					num26 = 0.1f * num27;
					Lighting.AddLight(position + new Vector2(8f, 8f), num24, num25, num26);
				}
				Vector2 vector2 = velocity;
				velocity = Collision.TileCollision(position, velocity, 16, 14);
				if (velocity != vector2)
				{
					if (frame < 10)
					{
						frame = 10;
						frameCounter = 0;
						if (type != 716 && type != 717 && type != 943 && (type < 1160 || type > 1162))
						{
							SoundEngine.PlaySound(39, (int)position.X + 8, (int)position.Y + 8, Main.rand.Next(2));
						}
					}
				}
				else if (Collision.WetCollision(position + velocity, 16, 14))
				{
					if (frame < 10)
					{
						frame = 10;
						frameCounter = 0;
						if (type != 716 && type != 717 && type != 943 && (type < 1160 || type > 1162))
						{
							SoundEngine.PlaySound(39, (int)position.X + 8, (int)position.Y + 8, 2);
						}
						((WaterShaderData)Filters.Scene["WaterDistortion"].GetShader()).QueueRipple(position + new Vector2(8f, 8f));
					}
					int num28 = (int)(position.X + 8f) / 16;
					int num29 = (int)(position.Y + 14f) / 16;
					if (Main.tile[num28, num29] != null && Main.tile[num28, num29].liquid > 0)
					{
						velocity *= 0f;
						position.Y = num29 * 16 - Main.tile[num28, num29].liquid / 16;
					}
				}
			}
			else if (sticky)
			{
				int num30 = 32;
				if (TextureAssets.Gore[type].IsLoaded)
				{
					num30 = TextureAssets.Gore[type].Width();
					if (TextureAssets.Gore[type].Height() < num30)
					{
						num30 = TextureAssets.Gore[type].Height();
					}
				}
				if (flag)
				{
					num30 = 4;
				}
				num30 = (int)((float)num30 * 0.9f);
				_ = velocity;
				velocity = Collision.TileCollision(position, velocity, (int)((float)num30 * scale), (int)((float)num30 * scale));
				if (velocity.Y == 0f)
				{
					if (flag)
					{
						velocity.X *= 0.94f;
					}
					else
					{
						velocity.X *= 0.97f;
					}
					if ((double)velocity.X > -0.01 && (double)velocity.X < 0.01)
					{
						velocity.X = 0f;
					}
				}
				if (timeLeft > 0)
				{
					timeLeft -= GoreID.Sets.DisappearSpeed[type];
				}
				else
				{
					alpha += GoreID.Sets.DisappearSpeedAlpha[type];
				}
			}
			else
			{
				alpha += 2 * GoreID.Sets.DisappearSpeedAlpha[type];
			}
		}
		if (type >= 907 && type <= 909)
		{
			int num31 = 32;
			if (TextureAssets.Gore[type].IsLoaded)
			{
				num31 = TextureAssets.Gore[type].Width();
				if (TextureAssets.Gore[type].Height() < num31)
				{
					num31 = TextureAssets.Gore[type].Height();
				}
			}
			num31 = (int)((float)num31 * 0.9f);
			Vector4 vector3 = Collision.SlopeCollision(position, velocity, num31, num31, 0f, fall: true);
			position.X = vector3.X;
			position.Y = vector3.Y;
			velocity.X = vector3.Z;
			velocity.Y = vector3.W;
		}
		if (GoreID.Sets.SpecialAI[type] == 1)
		{
			Gore_UpdateSail();
		}
		else if (GoreID.Sets.SpecialAI[type] == 3)
		{
			Gore_UpdateLeaf();
		}
		else
		{
			position += velocity;
		}
		if (alpha >= 255)
		{
			active = false;
		}
		if (light > 0f)
		{
			float num32 = light * scale;
			float num33 = light * scale;
			float num35 = light * scale;
			if (type == 16)
			{
				num35 *= 0.3f;
				num33 *= 0.8f;
			}
			else if (type == 17)
			{
				num33 *= 0.6f;
				num32 *= 0.3f;
			}
			if (TextureAssets.Gore[type].IsLoaded)
			{
				Lighting.AddLight((int)((position.X + (float)TextureAssets.Gore[type].Width() * scale / 2f) / 16f), (int)((position.Y + (float)TextureAssets.Gore[type].Height() * scale / 2f) / 16f), num32, num33, num35);
			}
			else
			{
				Lighting.AddLight((int)((position.X + 32f * scale / 2f) / 16f), (int)((position.Y + 32f * scale / 2f) / 16f), num32, num33, num35);
			}
		}
		GoreLoader.TakeDownUpdateType(this);
	}

	private void Gore_UpdateLeaf()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0347: Unknown result type (might be due to invalid IL or missing references)
		//IL_035a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0360: Unknown result type (might be due to invalid IL or missing references)
		//IL_0362: Unknown result type (might be due to invalid IL or missing references)
		//IL_0372: Unknown result type (might be due to invalid IL or missing references)
		//IL_0385: Unknown result type (might be due to invalid IL or missing references)
		//IL_038b: Unknown result type (might be due to invalid IL or missing references)
		//IL_038d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_03dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_03dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_03df: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0403: Unknown result type (might be due to invalid IL or missing references)
		//IL_0415: Unknown result type (might be due to invalid IL or missing references)
		//IL_0423: Unknown result type (might be due to invalid IL or missing references)
		//IL_0431: Unknown result type (might be due to invalid IL or missing references)
		//IL_043f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0444: Unknown result type (might be due to invalid IL or missing references)
		//IL_0445: Unknown result type (might be due to invalid IL or missing references)
		//IL_044a: Unknown result type (might be due to invalid IL or missing references)
		//IL_044f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0450: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0475: Unknown result type (might be due to invalid IL or missing references)
		//IL_047f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0485: Unknown result type (might be due to invalid IL or missing references)
		//IL_048a: Unknown result type (might be due to invalid IL or missing references)
		//IL_048f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0494: Unknown result type (might be due to invalid IL or missing references)
		//IL_0496: Unknown result type (might be due to invalid IL or missing references)
		//IL_049d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_051c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0523: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0566: Unknown result type (might be due to invalid IL or missing references)
		//IL_056b: Unknown result type (might be due to invalid IL or missing references)
		//IL_056c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0571: Unknown result type (might be due to invalid IL or missing references)
		//IL_0577: Unknown result type (might be due to invalid IL or missing references)
		//IL_0217: Unknown result type (might be due to invalid IL or missing references)
		//IL_021e: Unknown result type (might be due to invalid IL or missing references)
		//IL_054b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0259: Unknown result type (might be due to invalid IL or missing references)
		//IL_025a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0263: Unknown result type (might be due to invalid IL or missing references)
		//IL_0268: Unknown result type (might be due to invalid IL or missing references)
		//IL_026f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0295: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_030a: Unknown result type (might be due to invalid IL or missing references)
		Vector2 vector = position + new Vector2(12f) / 2f - new Vector2(4f) / 2f;
		vector.Y -= 4f;
		Vector2 vector2 = position - vector;
		if (velocity.Y < 0f)
		{
			Vector2 vector3 = default(Vector2);
			((Vector2)(ref vector3))._002Ector(velocity.X, -0.2f);
			int num = 4;
			num = (int)((float)num * 0.9f);
			Point point = (new Vector2((float)num, (float)num) / 2f + vector).ToTileCoordinates();
			if (!WorldGen.InWorld(point.X, point.Y))
			{
				active = false;
				return;
			}
			Tile tile = Main.tile[point.X, point.Y];
			if (tile == null)
			{
				active = false;
				return;
			}
			int num2 = 6;
			Rectangle rectangle = default(Rectangle);
			((Rectangle)(ref rectangle))._002Ector(point.X * 16, point.Y * 16 + tile.liquid / 16, 16, 16 - tile.liquid / 16);
			Rectangle value = default(Rectangle);
			((Rectangle)(ref value))._002Ector((int)vector.X, (int)vector.Y + num2, num, num);
			bool flag = tile != null && tile.liquid > 0 && ((Rectangle)(ref rectangle)).Intersects(value);
			if (flag)
			{
				if (tile.honey())
				{
					vector3.X = 0f;
				}
				else if (tile.lava())
				{
					active = false;
					for (int i = 0; i < 5; i++)
					{
						Dust.NewDust(position, num, num, 31, 0f, -0.2f);
					}
				}
				else
				{
					vector3.X = Main.WindForVisuals;
				}
				if ((double)position.Y > Main.worldSurface * 16.0)
				{
					vector3.X = 0f;
				}
			}
			if (!WorldGen.SolidTile(point.X, point.Y + 1) && !flag)
			{
				velocity.Y = 0.1f;
				timeLeft = 0;
				alpha += 20;
			}
			vector3 = Collision.TileCollision(vector, vector3, num, num);
			if (flag)
			{
				rotation = vector3.ToRotation() + (float)Math.PI / 2f;
			}
			vector3.X *= 0.94f;
			if (!flag || ((double)vector3.X > -0.01 && (double)vector3.X < 0.01))
			{
				vector3.X = 0f;
			}
			if (timeLeft > 0)
			{
				timeLeft -= GoreID.Sets.DisappearSpeed[type];
			}
			else
			{
				alpha += GoreID.Sets.DisappearSpeedAlpha[type];
			}
			velocity.X = vector3.X;
			position.X += velocity.X;
			return;
		}
		velocity.Y += (float)Math.PI / 180f;
		Vector2 vector4 = default(Vector2);
		((Vector2)(ref vector4))._002Ector(Vector2.UnitY.RotatedBy(velocity.Y).X * 1f, Math.Abs(Vector2.UnitY.RotatedBy(velocity.Y).Y) * 1f);
		int num3 = 4;
		if ((double)position.Y < Main.worldSurface * 16.0)
		{
			vector4.X += Main.WindForVisuals * 4f;
		}
		Vector2 vector5 = vector4;
		vector4 = Collision.TileCollision(vector, vector4, num3, num3);
		Vector4 vector6 = Collision.SlopeCollision(vector, vector4, num3, num3, 1f);
		position.X = vector6.X;
		position.Y = vector6.Y;
		vector4.X = vector6.Z;
		vector4.Y = vector6.W;
		position += vector2;
		if (vector4 != vector5)
		{
			velocity.Y = -1f;
		}
		Point point2 = (new Vector2(Width, Height) * 0.5f + position).ToTileCoordinates();
		if (!WorldGen.InWorld(point2.X, point2.Y))
		{
			active = false;
			return;
		}
		Tile tile2 = Main.tile[point2.X, point2.Y];
		if (tile2 == null)
		{
			active = false;
			return;
		}
		int num4 = 6;
		Rectangle rectangle2 = default(Rectangle);
		((Rectangle)(ref rectangle2))._002Ector(point2.X * 16, point2.Y * 16 + tile2.liquid / 16, 16, 16 - tile2.liquid / 16);
		Rectangle value2 = default(Rectangle);
		((Rectangle)(ref value2))._002Ector((int)vector.X, (int)vector.Y + num4, num3, num3);
		if (tile2 != null && tile2.liquid > 0 && ((Rectangle)(ref rectangle2)).Intersects(value2))
		{
			velocity.Y = -1f;
		}
		position += vector4;
		rotation = vector4.ToRotation() + (float)Math.PI / 2f;
		if (timeLeft > 0)
		{
			timeLeft -= GoreID.Sets.DisappearSpeed[type];
		}
		else
		{
			alpha += GoreID.Sets.DisappearSpeedAlpha[type];
		}
	}

	private void Gore_UpdateSail()
	{
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_0158: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0183: Unknown result type (might be due to invalid IL or missing references)
		//IL_0185: Unknown result type (might be due to invalid IL or missing references)
		//IL_019f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0201: Unknown result type (might be due to invalid IL or missing references)
		//IL_0219: Unknown result type (might be due to invalid IL or missing references)
		//IL_021e: Unknown result type (might be due to invalid IL or missing references)
		//IL_021f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0220: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_023a: Unknown result type (might be due to invalid IL or missing references)
		//IL_023f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0240: Unknown result type (might be due to invalid IL or missing references)
		//IL_0245: Unknown result type (might be due to invalid IL or missing references)
		//IL_024b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		if (velocity.Y < 0f)
		{
			Vector2 vector = default(Vector2);
			((Vector2)(ref vector))._002Ector(velocity.X, 0.6f);
			int num = 32;
			if (TextureAssets.Gore[type].IsLoaded)
			{
				num = TextureAssets.Gore[type].Width();
				if (TextureAssets.Gore[type].Height() < num)
				{
					num = TextureAssets.Gore[type].Height();
				}
			}
			num = (int)((float)num * 0.9f);
			vector = Collision.TileCollision(position, vector, (int)((float)num * scale), (int)((float)num * scale));
			vector.X *= 0.97f;
			if ((double)vector.X > -0.01 && (double)vector.X < 0.01)
			{
				vector.X = 0f;
			}
			if (timeLeft > 0)
			{
				timeLeft--;
			}
			else
			{
				alpha++;
			}
			velocity.X = vector.X;
			return;
		}
		velocity.Y += (float)Math.PI / 60f;
		Vector2 vector2 = default(Vector2);
		((Vector2)(ref vector2))._002Ector(Vector2.UnitY.RotatedBy(velocity.Y).X * 2f, Math.Abs(Vector2.UnitY.RotatedBy(velocity.Y).Y) * 3f);
		vector2 *= 2f;
		int num2 = 32;
		if (TextureAssets.Gore[type].IsLoaded)
		{
			num2 = TextureAssets.Gore[type].Width();
			if (TextureAssets.Gore[type].Height() < num2)
			{
				num2 = TextureAssets.Gore[type].Height();
			}
		}
		Vector2 vector3 = vector2;
		vector2 = Collision.TileCollision(position, vector2, (int)((float)num2 * scale), (int)((float)num2 * scale));
		if (vector2 != vector3)
		{
			velocity.Y = -1f;
		}
		position += vector2;
		rotation = vector2.ToRotation() + (float)Math.PI;
		if (timeLeft > 0)
		{
			timeLeft--;
		}
		else
		{
			alpha++;
		}
	}

	internal static Gore NewGorePerfect(Vector2 Position, Vector2 Velocity, int Type, float Scale = 1f)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		return NewGorePerfect(IEntitySource.GetGoreFallback(), Position, Velocity, Type, Scale);
	}

	internal static Gore NewGoreDirect(Vector2 Position, Vector2 Velocity, int Type, float Scale = 1f)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		return NewGoreDirect(IEntitySource.GetGoreFallback(), Position, Velocity, Type, Scale);
	}

	internal static int NewGore(Vector2 Position, Vector2 Velocity, int Type, float Scale = 1f)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		return NewGore(IEntitySource.GetGoreFallback(), Position, Velocity, Type, Scale);
	}

	/// <summary>
	/// Spawns a gore with an exact position and velocity, no randomization
	/// </summary>
	/// <param name="source">Recommend using <see cref="M:Terraria.Entity.GetSource_Death(System.String)" /> or <see cref="M:Terraria.Entity.GetSource_FromThis(System.String)" />" as the spawn source</param>
	/// <param name="Position"></param>
	/// <param name="Velocity"></param>
	/// <param name="Type"></param>
	/// <param name="Scale"></param>
	/// <returns></returns>
	public static Gore NewGorePerfect(IEntitySource source, Vector2 Position, Vector2 Velocity, int Type, float Scale = 1f)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		Gore gore = NewGoreDirect(source, Position, Velocity, Type, Scale);
		gore.position = Position;
		gore.velocity = Velocity;
		return gore;
	}

	/// <summary>
	/// Spawns a gore with given properties
	/// </summary>
	/// <param name="source">Recommend using <see cref="M:Terraria.Entity.GetSource_Death(System.String)" /> or <see cref="M:Terraria.Entity.GetSource_FromThis(System.String)" />" as the spawn source</param>
	/// <param name="Position"></param>
	/// <param name="Velocity"></param>
	/// <param name="Type"></param>
	/// <param name="Scale"></param>
	/// <returns>A reference to the gore</returns>
	public static Gore NewGoreDirect(IEntitySource source, Vector2 Position, Vector2 Velocity, int Type, float Scale = 1f)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		return Main.gore[NewGore(source, Position, Velocity, Type, Scale)];
	}

	/// <summary>
	/// Spawns a gore with given properties
	/// </summary>
	/// <param name="source">Recommend using <see cref="M:Terraria.Entity.GetSource_Death(System.String)" /> or <see cref="M:Terraria.Entity.GetSource_FromThis(System.String)" />" as the spawn source</param>
	/// <param name="Position"></param>
	/// <param name="Velocity"></param>
	/// <param name="Type"></param>
	/// <param name="Scale"></param>
	/// <returns>The index of the gore in the <see cref="F:Terraria.Main.gore" /> array</returns>
	public static int NewGore(IEntitySource source, Vector2 Position, Vector2 Velocity, int Type, float Scale = 1f)
	{
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_034d: Unknown result type (might be due to invalid IL or missing references)
		//IL_034e: Unknown result type (might be due to invalid IL or missing references)
		//IL_047f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0484: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0422: Unknown result type (might be due to invalid IL or missing references)
		//IL_0510: Unknown result type (might be due to invalid IL or missing references)
		//IL_0515: Unknown result type (might be due to invalid IL or missing references)
		//IL_054d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0577: Unknown result type (might be due to invalid IL or missing references)
		//IL_070e: Unknown result type (might be due to invalid IL or missing references)
		//IL_070f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0720: Unknown result type (might be due to invalid IL or missing references)
		//IL_074a: Unknown result type (might be due to invalid IL or missing references)
		if (Main.netMode == 2)
		{
			return 600;
		}
		if (Main.gamePaused)
		{
			return 600;
		}
		if (WorldGen.gen)
		{
			return 600;
		}
		if (Main.rand == null)
		{
			Main.rand = new UnifiedRandom();
		}
		if (Type <= 0)
		{
			return 600;
		}
		int num = 600;
		for (int i = 0; i < 600; i++)
		{
			if (!Main.gore[i].active)
			{
				num = i;
				break;
			}
		}
		if (num == 600)
		{
			return num;
		}
		Main.gore[num].Frame = new SpriteFrame(1, 1);
		Main.gore[num].frameCounter = 0;
		Main.gore[num].behindTiles = false;
		Main.gore[num].light = 0f;
		Main.gore[num].position = Position;
		Main.gore[num].velocity = Velocity;
		Main.gore[num].velocity.Y -= (float)Main.rand.Next(10, 31) * 0.1f;
		Main.gore[num].velocity.X += (float)Main.rand.Next(-20, 21) * 0.1f;
		Main.gore[num].type = Type;
		Main.gore[num].active = true;
		Main.gore[num].alpha = 0;
		Main.gore[num].rotation = 0f;
		Main.gore[num].scale = Scale;
		if (!ChildSafety.Disabled && ChildSafety.DangerousGore(Type))
		{
			Main.gore[num].type = Main.rand.Next(11, 14);
			Main.gore[num].scale = Main.rand.NextFloat() * 0.5f + 0.5f;
			Gore obj = Main.gore[num];
			obj.velocity /= 2f;
		}
		if (goreTime == 0 || Type == 11 || Type == 12 || Type == 13 || Type == 16 || Type == 17 || Type == 61 || Type == 62 || Type == 63 || Type == 99 || Type == 220 || Type == 221 || Type == 222 || Type == 435 || Type == 436 || Type == 437 || (Type >= 861 && Type <= 862))
		{
			Main.gore[num].sticky = false;
		}
		else if (Type >= 375 && Type <= 377)
		{
			Main.gore[num].sticky = false;
			Main.gore[num].alpha = 100;
		}
		else
		{
			Main.gore[num].sticky = true;
			Main.gore[num].timeLeft = goreTime;
		}
		if ((Type >= 706 && Type <= 717) || Type == 943 || Type == 1147 || (Type >= 1160 && Type <= 1162))
		{
			Main.gore[num].numFrames = 15;
			Main.gore[num].behindTiles = true;
			Main.gore[num].timeLeft = goreTime * 3;
		}
		if (Type == 16 || Type == 17)
		{
			Main.gore[num].alpha = 100;
			Main.gore[num].scale = 0.7f;
			Main.gore[num].light = 1f;
		}
		if (Type >= 570 && Type <= 572)
		{
			Main.gore[num].velocity = Velocity;
		}
		if (Type == 1201 || Type == 1208)
		{
			Main.gore[num].Frame = new SpriteFrame(1, 4);
		}
		if (Type == 1217 || Type == 1218)
		{
			Main.gore[num].Frame = new SpriteFrame(1, 3);
		}
		if (Type == 1225)
		{
			Main.gore[num].Frame = new SpriteFrame(1, 3);
			Main.gore[num].timeLeft = 10 + Main.rand.Next(6);
			Main.gore[num].sticky = false;
			if (TextureAssets.Gore[Type].IsLoaded)
			{
				Main.gore[num].position.X = Position.X - (float)(TextureAssets.Gore[Type].Width() / 2) * Scale;
				Main.gore[num].position.Y = Position.Y - (float)TextureAssets.Gore[Type].Height() * Scale / 2f;
			}
		}
		int num3 = GoreID.Sets.SpecialAI[Type];
		if (num3 == 3)
		{
			Main.gore[num].velocity = new Vector2((Main.rand.NextFloat() - 0.5f) * 1f, Main.rand.NextFloat() * ((float)Math.PI * 2f));
			bool flag = GoreID.Sets.PaintedFallingLeaf[Type];
			Gore obj2 = Main.gore[num];
			SpriteFrame spriteFrame2 = new SpriteFrame((byte)((!flag) ? 1u : 32u), 8)
			{
				CurrentRow = (byte)Main.rand.Next(8)
			};
			SpriteFrame spriteFrame = spriteFrame2;
			obj2.Frame = spriteFrame;
			Main.gore[num].frameCounter = (byte)Main.rand.Next(8);
		}
		if (num3 == 1)
		{
			Main.gore[num].velocity = new Vector2((Main.rand.NextFloat() - 0.5f) * 3f, Main.rand.NextFloat() * ((float)Math.PI * 2f));
		}
		if (Type >= 411 && Type <= 430 && TextureAssets.Gore[Type].IsLoaded)
		{
			Main.gore[num].position.X = Position.X - (float)(TextureAssets.Gore[Type].Width() / 2) * Scale;
			Main.gore[num].position.Y = Position.Y - (float)TextureAssets.Gore[Type].Height() * Scale;
			Main.gore[num].velocity.Y *= (float)Main.rand.Next(90, 150) * 0.01f;
			Main.gore[num].velocity.X *= (float)Main.rand.Next(40, 90) * 0.01f;
			int num2 = Main.rand.Next(4) * 5;
			Main.gore[num].type += num2;
			Main.gore[num].timeLeft = Main.rand.Next(goreTime / 2, goreTime * 2);
			Main.gore[num].sticky = true;
			if (goreTime == 0)
			{
				Main.gore[num].timeLeft = Main.rand.Next(150, 600);
			}
		}
		if (Type >= 907 && Type <= 909)
		{
			Main.gore[num].sticky = true;
			Main.gore[num].numFrames = 3;
			Main.gore[num].frame = (byte)Main.rand.Next(3);
			Main.gore[num].frameCounter = (byte)Main.rand.Next(5);
			Main.gore[num].rotation = 0f;
		}
		if (num3 == 2)
		{
			Main.gore[num].sticky = false;
			if (TextureAssets.Gore[Type].IsLoaded)
			{
				Main.gore[num].alpha = 150;
				Main.gore[num].velocity = Velocity;
				Main.gore[num].position.X = Position.X - (float)(TextureAssets.Gore[Type].Width() / 2) * Scale;
				Main.gore[num].position.Y = Position.Y - (float)TextureAssets.Gore[Type].Height() * Scale / 2f;
				Main.gore[num].timeLeft = Main.rand.Next(goreTime / 2, goreTime + 1);
			}
		}
		if (num3 == 4)
		{
			Main.gore[num].alpha = 254;
			Main.gore[num].timeLeft = 300;
		}
		if (num3 == 5)
		{
			Main.gore[num].alpha = 254;
			Main.gore[num].timeLeft = 240;
		}
		if (num3 == 6)
		{
			Main.gore[num].alpha = 254;
			Main.gore[num].timeLeft = 480;
		}
		Gore gore = Main.gore[num];
		gore.ResetNewFields();
		gore.ModGore = GoreLoader.GetModGore(gore.type);
		gore.ModGore?.OnSpawn(gore, source ?? IEntitySource.GetGoreFallback());
		if (Main.gore[num].DeactivateIfOutsideOfWorld())
		{
			return 600;
		}
		return num;
	}

	public Color GetAlpha(Color newColor)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		Color? val = ModGore?.GetAlpha(this, newColor);
		if (val.HasValue)
		{
			return val.GetValueOrDefault();
		}
		float num = (float)(255 - alpha) / 255f;
		int r;
		int g;
		int b;
		if (type == 16 || type == 17)
		{
			r = ((Color)(ref newColor)).R;
			g = ((Color)(ref newColor)).G;
			b = ((Color)(ref newColor)).B;
		}
		else
		{
			if (type == 716)
			{
				return new Color(255, 255, 255, 200);
			}
			if (type >= 570 && type <= 572)
			{
				byte b2 = (byte)(255 - alpha);
				return new Color((int)b2, (int)b2, (int)b2, b2 / 2);
			}
			if (type == 331)
			{
				return new Color(255, 255, 255, 50);
			}
			if (type == 1225)
			{
				return new Color(num, num, num, num);
			}
			r = (int)((float)(int)((Color)(ref newColor)).R * num);
			g = (int)((float)(int)((Color)(ref newColor)).G * num);
			b = (int)((float)(int)((Color)(ref newColor)).B * num);
		}
		int num2 = ((Color)(ref newColor)).A - alpha;
		if (num2 < 0)
		{
			num2 = 0;
		}
		if (num2 > 255)
		{
			num2 = 255;
		}
		if (type >= 1202 && type <= 1204)
		{
			return new Color(r, g, b, (num2 < 20) ? num2 : 20);
		}
		return new Color(r, g, b, num2);
	}

	private void ResetNewFields()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		drawOffset = Vector2.Zero;
		realType = 0;
	}
}
