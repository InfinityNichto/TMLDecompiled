using System;
using Microsoft.Xna.Framework;
using Terraria.GameContent;
using Terraria.GameContent.Events;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace Terraria;

/// <summary>
/// Dust are particles effects used to add visual elements to weapons and other effects. Dust are completely visual and should never be used as a gameplay element. Dust count limits are imposed by the game to keep perfomance consistent, so Dust are not guaranteed to spawn when code attempts to spawn them.<para />
/// Vanilla Dust are enumerated in the <see cref="T:Terraria.ID.DustID" /> class.<para />
/// New Dust can be implemented using the <see cref="T:Terraria.ModLoader.ModDust" /> class.<para />
/// The <see href="https://github.com/tModLoader/tModLoader/wiki/Basic-Dust">Basic Dust Guide</see> teaches the basics on using Dust. In addition, the guide has resources teaching how to discover and use vanilla Dust.
/// </summary>
public class Dust
{
	public static float dCount;

	public static int lavaBubbles;

	public static int SandStormCount;

	public int dustIndex;

	/// <summary>
	/// Current position of this dust.
	/// </summary>
	public Vector2 position;

	/// <summary>
	/// Current velocity of this dust.
	/// </summary>
	public Vector2 velocity;

	/// <summary>
	/// Used by some dust AI to control logic pertaining to the dust fading in. The specific behavior depends on the dust type. Defaults to 0f.
	/// </summary>
	public float fadeIn;

	/// <summary>
	/// Indicates if a dust should be affected by gravity or not. Not all vanilla dust have logic checking this value and modded dust with custom <see cref="M:Terraria.ModLoader.ModDust.Update(Terraria.Dust)" /> code would need to implement this into the logic for it to have effect. Defaults to <see langword="false" />.
	/// </summary>
	public bool noGravity;

	/// <summary>
	/// The draw scale of the dust. Many dust rely on scale to determine when it should despawn. The Scale parameter of <see cref="M:Terraria.Dust.NewDust(Microsoft.Xna.Framework.Vector2,System.Int32,System.Int32,System.Int32,System.Single,System.Single,System.Int32,Microsoft.Xna.Framework.Color,System.Single)" /> will be slightly randomized (multiplied by a number between 80% to 120%) and then assigned to <see cref="F:Terraria.Dust.scale" />.
	/// </summary>
	public float scale;

	/// <summary>
	/// Current rotation of this dust.
	/// </summary>
	public float rotation;

	/// <summary>
	/// If true, the dust will not emit light. The specific behavior depends on the dust type, as many dust might not emit light or honor <see cref="F:Terraria.Dust.noLight" />. Defaults to <see langword="false" />.
	/// </summary>
	public bool noLight;

	public bool noLightEmittence;

	public bool active;

	/// <summary>
	/// The Dust ID of this dust. The Dust ID will be equal to either a <see cref="T:Terraria.ID.DustID" /> entry or <see cref="M:Terraria.ModLoader.ModContent.DustType``1" />.
	/// </summary>
	public int type;

	/// <summary>
	/// A tinting to the color this dust is drawn. Works best with sprites with low saturation, such as greyscale sprites. Not as effective with colored sprites.
	/// </summary>
	public Color color;

	/// <summary>
	/// How transparent to draw this dust. 0 to 255. 255 is completely transparent. Used to fade dust out and sometimes to determine if a dust should despawn.
	/// </summary>
	public int alpha;

	/// <summary>
	/// The portion of the sprite sheet that this dust will be drawn using. Typical vanilla dust spawn with a randomly choosen 8x8 frame from a set of 3 options, but modded dust can use whatever dimensions are desired. 
	/// </summary>
	public Rectangle frame;

	/// <summary>
	/// The shader that will be applied to this dust when drawing. Typically assigned based on the dye attached to the accessory or equipment spawning the dust. This is done using code similar to <c>dust.shader = GameShaders.Armor.GetSecondaryShader(player.cPet, player);</c>
	/// </summary>
	public ArmorShaderData shader;

	/// <summary>
	/// Can be used to store any custom data. Typically used to store a Player, NPC, or Projectile instance that the dust will attempt to follow.
	/// <para /> <see href="https://github.com/tModLoader/tModLoader/blob/stable/ExampleMod/Content/Dusts/ExampleAdvancedDust.cs">ExampleAdvancedDust.cs</see> showcases how customData can be used for advanced movement behavior.
	/// </summary>
	public object customData;

	public bool firstFrame;

	/// <summary>
	/// Used to temporarily store <see cref="F:Terraria.Dust.type" /> when using <see cref="P:Terraria.ModLoader.ModDust.UpdateType" /> to copy the behavior of an existing dust type.
	/// </summary>
	internal int realType = -1;

	/// <summary>
	/// Attempts to spawn a single Dust into the game world. <paramref name="Position" /> indicates the spawn position and <paramref name="Velocity" /> indicates the initial velocity. Unlike <see cref="M:Terraria.Dust.NewDust(Microsoft.Xna.Framework.Vector2,System.Int32,System.Int32,System.Int32,System.Single,System.Single,System.Int32,Microsoft.Xna.Framework.Color,System.Single)" />, position and velocity will not be slightly randomized, making this method suitable for visual effects that need exact positioning.
	/// </summary>
	/// <param name="Position"></param>
	/// <param name="Type"></param>
	/// <param name="Velocity"></param>
	/// <param name="Alpha"></param>
	/// <param name="newColor"></param>
	/// <param name="Scale"></param>
	/// <returns>The <see cref="T:Terraria.Dust" /> instance spawned</returns>
	public static Dust NewDustPerfect(Vector2 Position, int Type, Vector2? Velocity = null, int Alpha = 0, Color newColor = default(Color), float Scale = 1f)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		Dust dust = Main.dust[NewDust(Position, 0, 0, Type, 0f, 0f, Alpha, newColor, Scale)];
		dust.position = Position;
		if (Velocity.HasValue)
		{
			dust.velocity = Velocity.Value;
		}
		return dust;
	}

	/// <summary>
	/// <inheritdoc cref="M:Terraria.Dust.NewDust(Microsoft.Xna.Framework.Vector2,System.Int32,System.Int32,System.Int32,System.Single,System.Single,System.Int32,Microsoft.Xna.Framework.Color,System.Single)" />
	/// <para /> Unlike <see cref="M:Terraria.Dust.NewDust(Microsoft.Xna.Framework.Vector2,System.Int32,System.Int32,System.Int32,System.Single,System.Single,System.Int32,Microsoft.Xna.Framework.Color,System.Single)" />, this method returns a <see cref="T:Terraria.Dust" /> instance directly.
	/// </summary>
	/// <param name="Position"></param>
	/// <param name="Width"></param>
	/// <param name="Height"></param>
	/// <param name="Type"></param>
	/// <param name="SpeedX"></param>
	/// <param name="SpeedY"></param>
	/// <param name="Alpha"></param>
	/// <param name="newColor"></param>
	/// <param name="Scale"></param>
	/// <returns>The <see cref="T:Terraria.Dust" /> instance spawned</returns>
	public static Dust NewDustDirect(Vector2 Position, int Width, int Height, int Type, float SpeedX = 0f, float SpeedY = 0f, int Alpha = 0, Color newColor = default(Color), float Scale = 1f)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		Dust dust = Main.dust[NewDust(Position, Width, Height, Type, SpeedX, SpeedY, Alpha, newColor, Scale)];
		if (dust.velocity.HasNaNs())
		{
			dust.velocity = Vector2.Zero;
		}
		return dust;
	}

	/// <summary>
	/// Attempts to spawn a single Dust into the game world. The Position, Width, and Height parameters dictate a rectangle, the dust will be spawned randomly within that rectangle. SpeedX and SpeedY dictate the initial velocity, they will be slightly randomized as well.
	/// </summary>
	/// <param name="Position"></param>
	/// <param name="Width"></param>
	/// <param name="Height"></param>
	/// <param name="Type">Either an <see cref="T:Terraria.ID.DustID" /> entry or <see cref="M:Terraria.ModLoader.ModContent.DustType``1" /></param>
	/// <param name="SpeedX"></param>
	/// <param name="SpeedY"></param>
	/// <param name="Alpha"></param>
	/// <param name="newColor"></param>
	/// <param name="Scale"></param>
	/// <returns>The index of the dust within <see cref="F:Terraria.Main.dust" /></returns>
	public static int NewDust(Vector2 Position, int Width, int Height, int Type, float SpeedX = 0f, float SpeedY = 0f, int Alpha = 0, Color newColor = default(Color), float Scale = 1f)
	{
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_0203: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Unknown result type (might be due to invalid IL or missing references)
		//IL_021a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0242: Unknown result type (might be due to invalid IL or missing references)
		//IL_0526: Unknown result type (might be due to invalid IL or missing references)
		//IL_0530: Unknown result type (might be due to invalid IL or missing references)
		//IL_0535: Unknown result type (might be due to invalid IL or missing references)
		//IL_055e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0568: Unknown result type (might be due to invalid IL or missing references)
		//IL_056d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0612: Unknown result type (might be due to invalid IL or missing references)
		//IL_061c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0621: Unknown result type (might be due to invalid IL or missing references)
		//IL_0649: Unknown result type (might be due to invalid IL or missing references)
		//IL_0653: Unknown result type (might be due to invalid IL or missing references)
		//IL_0658: Unknown result type (might be due to invalid IL or missing references)
		//IL_069f: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ed: Unknown result type (might be due to invalid IL or missing references)
		if (Main.gameMenu)
		{
			return 6000;
		}
		if (Main.rand == null)
		{
			Main.rand = new UnifiedRandom((int)DateTime.Now.Ticks);
		}
		if (Main.gamePaused)
		{
			return 6000;
		}
		if (WorldGen.gen)
		{
			return 6000;
		}
		if (Main.netMode == 2)
		{
			return 6000;
		}
		int num = (int)(400f * (1f - dCount));
		Rectangle rectangle = default(Rectangle);
		((Rectangle)(ref rectangle))._002Ector((int)(Main.screenPosition.X - (float)num), (int)(Main.screenPosition.Y - (float)num), Main.screenWidth + num * 2, Main.screenHeight + num * 2);
		Rectangle value = default(Rectangle);
		((Rectangle)(ref value))._002Ector((int)Position.X, (int)Position.Y, 10, 10);
		if (!((Rectangle)(ref rectangle)).Intersects(value))
		{
			return 6000;
		}
		int result = 6000;
		for (int i = 0; i < 6000; i++)
		{
			Dust dust = Main.dust[i];
			if (dust.active)
			{
				continue;
			}
			if ((double)i > (double)Main.maxDustToDraw * 0.9)
			{
				if (Main.rand.Next(4) != 0)
				{
					return 6000;
				}
			}
			else if ((double)i > (double)Main.maxDustToDraw * 0.8)
			{
				if (Main.rand.Next(3) != 0)
				{
					return 6000;
				}
			}
			else if ((double)i > (double)Main.maxDustToDraw * 0.7)
			{
				if (Main.rand.Next(2) == 0)
				{
					return 6000;
				}
			}
			else if ((double)i > (double)Main.maxDustToDraw * 0.6)
			{
				if (Main.rand.Next(4) == 0)
				{
					return 6000;
				}
			}
			else if ((double)i > (double)Main.maxDustToDraw * 0.5)
			{
				if (Main.rand.Next(5) == 0)
				{
					return 6000;
				}
			}
			else
			{
				dCount = 0f;
			}
			int num2 = Width;
			int num3 = Height;
			if (num2 < 5)
			{
				num2 = 5;
			}
			if (num3 < 5)
			{
				num3 = 5;
			}
			result = i;
			dust.fadeIn = 0f;
			dust.active = true;
			dust.type = Type;
			dust.noGravity = false;
			dust.color = newColor;
			dust.alpha = Alpha;
			dust.position.X = Position.X + (float)Main.rand.Next(num2 - 4) + 4f;
			dust.position.Y = Position.Y + (float)Main.rand.Next(num3 - 4) + 4f;
			dust.velocity.X = (float)Main.rand.Next(-20, 21) * 0.1f + SpeedX;
			dust.velocity.Y = (float)Main.rand.Next(-20, 21) * 0.1f + SpeedY;
			dust.frame.X = 10 * Type;
			dust.frame.Y = 10 * Main.rand.Next(3);
			dust.shader = null;
			dust.customData = null;
			dust.noLightEmittence = false;
			int num4 = Type;
			while (num4 >= 100)
			{
				num4 -= 100;
				dust.frame.X -= 1000;
				dust.frame.Y += 30;
			}
			dust.frame.Width = 8;
			dust.frame.Height = 8;
			dust.rotation = 0f;
			dust.scale = 1f + (float)Main.rand.Next(-20, 21) * 0.01f;
			dust.scale *= Scale;
			dust.noLight = false;
			dust.firstFrame = true;
			if (dust.type == 228 || dust.type == 279 || dust.type == 269 || dust.type == 135 || dust.type == 6 || dust.type == 242 || dust.type == 75 || dust.type == 169 || dust.type == 29 || (dust.type >= 59 && dust.type <= 65) || dust.type == 158 || dust.type == 293 || dust.type == 294 || dust.type == 295 || dust.type == 296 || dust.type == 297 || dust.type == 298 || dust.type == 302 || dust.type == 307 || dust.type == 310)
			{
				dust.velocity.Y = (float)Main.rand.Next(-10, 6) * 0.1f;
				dust.velocity.X *= 0.3f;
				dust.scale *= 0.7f;
			}
			if (dust.type == 127 || dust.type == 187)
			{
				dust.velocity *= 0.3f;
				dust.scale *= 0.7f;
			}
			if (dust.type == 308)
			{
				dust.velocity *= 0.5f;
				dust.velocity.Y += 1f;
			}
			if (dust.type == 33 || dust.type == 52 || dust.type == 266 || dust.type == 98 || dust.type == 99 || dust.type == 100 || dust.type == 101 || dust.type == 102 || dust.type == 103 || dust.type == 104 || dust.type == 105)
			{
				dust.alpha = 170;
				dust.velocity *= 0.5f;
				dust.velocity.Y += 1f;
			}
			if (dust.type == 41)
			{
				dust.velocity *= 0f;
			}
			if (dust.type == 80)
			{
				dust.alpha = 50;
			}
			DustLoader.SetupDust(dust);
			if (dust.type == 34 || dust.type == 35 || dust.type == 152)
			{
				dust.velocity *= 0.1f;
				dust.velocity.Y = -0.5f;
				if (dust.type == 34 && !Collision.WetCollision(new Vector2(dust.position.X, dust.position.Y - 8f), 4, 4))
				{
					dust.active = false;
				}
			}
			break;
		}
		return result;
	}

	public static Dust CloneDust(int dustIndex)
	{
		return CloneDust(Main.dust[dustIndex]);
	}

	public static Dust CloneDust(Dust rf)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		if (rf.dustIndex == Main.maxDustToDraw)
		{
			return rf;
		}
		int num = NewDust(rf.position, 0, 0, rf.type);
		Dust obj = Main.dust[num];
		obj.position = rf.position;
		obj.velocity = rf.velocity;
		obj.fadeIn = rf.fadeIn;
		obj.noGravity = rf.noGravity;
		obj.scale = rf.scale;
		obj.rotation = rf.rotation;
		obj.noLight = rf.noLight;
		obj.active = rf.active;
		obj.type = rf.type;
		obj.color = rf.color;
		obj.alpha = rf.alpha;
		obj.frame = rf.frame;
		obj.shader = rf.shader;
		obj.customData = rf.customData;
		return obj;
	}

	/// <inheritdoc cref="M:Terraria.Dust.QuickDust(Microsoft.Xna.Framework.Vector2,Microsoft.Xna.Framework.Color)" />
	public static Dust QuickDust(int x, int y, Color color)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		return Dust.QuickDust(new Point(x, y), color);
	}

	/// <inheritdoc cref="M:Terraria.Dust.QuickDust(Microsoft.Xna.Framework.Vector2,Microsoft.Xna.Framework.Color)" />
	public static Dust QuickDust(Point tileCoords, Color color)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		return QuickDust(tileCoords.ToWorldCoordinates(), color);
	}

	public static void QuickBox(Vector2 topLeft, Vector2 bottomRight, int divisions, Color color, Action<Dust> manipulator)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		float num = divisions + 2;
		for (float num2 = 0f; num2 <= (float)(divisions + 2); num2 += 1f)
		{
			Dust obj = Dust.QuickDust(new Vector2(MathHelper.Lerp(topLeft.X, bottomRight.X, num2 / num), topLeft.Y), color);
			manipulator?.Invoke(obj);
			obj = Dust.QuickDust(new Vector2(MathHelper.Lerp(topLeft.X, bottomRight.X, num2 / num), bottomRight.Y), color);
			manipulator?.Invoke(obj);
			obj = Dust.QuickDust(new Vector2(topLeft.X, MathHelper.Lerp(topLeft.Y, bottomRight.Y, num2 / num)), color);
			manipulator?.Invoke(obj);
			obj = Dust.QuickDust(new Vector2(bottomRight.X, MathHelper.Lerp(topLeft.Y, bottomRight.Y, num2 / num)), color);
			manipulator?.Invoke(obj);
		}
	}

	public static void DrawDebugBox(Rectangle itemRectangle)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		Vector2 vector = itemRectangle.TopLeft();
		itemRectangle.BottomRight();
		for (int i = 0; i <= itemRectangle.Width; i++)
		{
			for (int j = 0; j <= itemRectangle.Height; j++)
			{
				if (i == 0 || j == 0 || i == itemRectangle.Width - 1 || j == itemRectangle.Height - 1)
				{
					QuickDust(vector + new Vector2((float)i, (float)j), Color.White).scale = 1f;
				}
			}
		}
	}

	/// <summary>
	/// Spawns dust 267 tinted to the provided color at the specified position with no velocity or gravity. Used solely for debugging purposes.
	/// </summary>
	/// <param name="pos"></param>
	/// <param name="color"></param>
	/// <returns></returns>
	public static Dust QuickDust(Vector2 pos, Color color)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		Dust obj = Main.dust[NewDust(pos, 0, 0, 267)];
		obj.position = pos;
		obj.velocity = Vector2.Zero;
		obj.fadeIn = 1f;
		obj.noLight = true;
		obj.noGravity = true;
		obj.color = color;
		return obj;
	}

	public static Dust QuickDustSmall(Vector2 pos, Color color, bool floorPositionValues = false)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		Dust dust = QuickDust(pos, color);
		dust.fadeIn = 0f;
		dust.scale = 0.35f;
		if (floorPositionValues)
		{
			dust.position = dust.position.Floor();
		}
		return dust;
	}

	public static void QuickDustLine(Vector2 start, Vector2 end, float splits, Color color)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		QuickDust(start, color).scale = 0.3f;
		QuickDust(end, color).scale = 0.3f;
		float num = 1f / splits;
		for (float num2 = 0f; num2 < 1f; num2 += num)
		{
			QuickDust(Vector2.Lerp(start, end, num2), color).scale = 0.3f;
		}
	}

	public static int dustWater()
	{
		if (Main.waterStyle >= 15)
		{
			return LoaderManager.Get<WaterStylesLoader>().Get(Main.waterStyle).GetSplashDust();
		}
		return Main.waterStyle switch
		{
			2 => 98, 
			3 => 99, 
			4 => 100, 
			5 => 101, 
			6 => 102, 
			7 => 103, 
			8 => 104, 
			9 => 105, 
			10 => 123, 
			12 => 288, 
			_ => 33, 
		};
	}

	public static void UpdateDust()
	{
		//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0203: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_0298: Unknown result type (might be due to invalid IL or missing references)
		//IL_053e: Unknown result type (might be due to invalid IL or missing references)
		//IL_054a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0554: Unknown result type (might be due to invalid IL or missing references)
		//IL_0559: Unknown result type (might be due to invalid IL or missing references)
		//IL_055d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0562: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04af: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04be: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0504: Unknown result type (might be due to invalid IL or missing references)
		//IL_0509: Unknown result type (might be due to invalid IL or missing references)
		//IL_050e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0513: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_05af: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_05bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_075c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0768: Unknown result type (might be due to invalid IL or missing references)
		//IL_0772: Unknown result type (might be due to invalid IL or missing references)
		//IL_0777: Unknown result type (might be due to invalid IL or missing references)
		//IL_077b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0780: Unknown result type (might be due to invalid IL or missing references)
		//IL_062e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0633: Unknown result type (might be due to invalid IL or missing references)
		//IL_063d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0642: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_07af: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_07bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_07cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_067c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0686: Unknown result type (might be due to invalid IL or missing references)
		//IL_068b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0803: Unknown result type (might be due to invalid IL or missing references)
		//IL_080a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0811: Unknown result type (might be due to invalid IL or missing references)
		//IL_0816: Unknown result type (might be due to invalid IL or missing references)
		//IL_081b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0820: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_08f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_08f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_08fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0905: Unknown result type (might be due to invalid IL or missing references)
		//IL_0909: Unknown result type (might be due to invalid IL or missing references)
		//IL_090e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0934: Unknown result type (might be due to invalid IL or missing references)
		//IL_0939: Unknown result type (might be due to invalid IL or missing references)
		//IL_094d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0952: Unknown result type (might be due to invalid IL or missing references)
		//IL_0957: Unknown result type (might be due to invalid IL or missing references)
		//IL_0719: Unknown result type (might be due to invalid IL or missing references)
		//IL_0723: Unknown result type (might be due to invalid IL or missing references)
		//IL_0728: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bf1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c00: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c05: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c0a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bb9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bc3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bc8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b1a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b21: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b28: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b2d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b32: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b37: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b88: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b9c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ba3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dd3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dd8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0de2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0de7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dad: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c4a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c51: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c58: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c5d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c62: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c67: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e5a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e5f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e06: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e10: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e15: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eac: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eb3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eb8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eca: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ece: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ed3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0efb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f02: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f09: Unknown result type (might be due to invalid IL or missing references)
		//IL_133e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1345: Unknown result type (might be due to invalid IL or missing references)
		//IL_134a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1530: Unknown result type (might be due to invalid IL or missing references)
		//IL_1537: Unknown result type (might be due to invalid IL or missing references)
		//IL_1544: Unknown result type (might be due to invalid IL or missing references)
		//IL_1549: Unknown result type (might be due to invalid IL or missing references)
		//IL_154e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1553: Unknown result type (might be due to invalid IL or missing references)
		//IL_4619: Unknown result type (might be due to invalid IL or missing references)
		//IL_45c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_1585: Unknown result type (might be due to invalid IL or missing references)
		//IL_158c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1593: Unknown result type (might be due to invalid IL or missing references)
		//IL_1598: Unknown result type (might be due to invalid IL or missing references)
		//IL_159d: Unknown result type (might be due to invalid IL or missing references)
		//IL_15a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_15ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_15d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_15d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_15db: Unknown result type (might be due to invalid IL or missing references)
		//IL_15dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_15df: Unknown result type (might be due to invalid IL or missing references)
		//IL_15f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_1600: Unknown result type (might be due to invalid IL or missing references)
		//IL_1605: Unknown result type (might be due to invalid IL or missing references)
		//IL_1613: Unknown result type (might be due to invalid IL or missing references)
		//IL_1618: Unknown result type (might be due to invalid IL or missing references)
		//IL_1622: Unknown result type (might be due to invalid IL or missing references)
		//IL_1627: Unknown result type (might be due to invalid IL or missing references)
		//IL_4a97: Unknown result type (might be due to invalid IL or missing references)
		//IL_1dc8: Unknown result type (might be due to invalid IL or missing references)
		//IL_1dcd: Unknown result type (might be due to invalid IL or missing references)
		//IL_1dd7: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ddc: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e77: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e7e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e83: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e87: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e8c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e16: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e20: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e25: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ffc: Unknown result type (might be due to invalid IL or missing references)
		//IL_2006: Unknown result type (might be due to invalid IL or missing references)
		//IL_200b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1eb7: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ebe: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ec3: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ec8: Unknown result type (might be due to invalid IL or missing references)
		//IL_1edd: Unknown result type (might be due to invalid IL or missing references)
		//IL_50e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_50f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_50f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_50fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_21ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_21d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_21d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_206c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2087: Unknown result type (might be due to invalid IL or missing references)
		//IL_208d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f7b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f85: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f8a: Unknown result type (might be due to invalid IL or missing references)
		//IL_514b: Unknown result type (might be due to invalid IL or missing references)
		//IL_5155: Unknown result type (might be due to invalid IL or missing references)
		//IL_515a: Unknown result type (might be due to invalid IL or missing references)
		//IL_5160: Unknown result type (might be due to invalid IL or missing references)
		//IL_516a: Unknown result type (might be due to invalid IL or missing references)
		//IL_516f: Unknown result type (might be due to invalid IL or missing references)
		//IL_5171: Unknown result type (might be due to invalid IL or missing references)
		//IL_517b: Unknown result type (might be due to invalid IL or missing references)
		//IL_5180: Unknown result type (might be due to invalid IL or missing references)
		//IL_2395: Unknown result type (might be due to invalid IL or missing references)
		//IL_239f: Unknown result type (might be due to invalid IL or missing references)
		//IL_23a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_2309: Unknown result type (might be due to invalid IL or missing references)
		//IL_2313: Unknown result type (might be due to invalid IL or missing references)
		//IL_2318: Unknown result type (might be due to invalid IL or missing references)
		//IL_223a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2255: Unknown result type (might be due to invalid IL or missing references)
		//IL_225b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2149: Unknown result type (might be due to invalid IL or missing references)
		//IL_2153: Unknown result type (might be due to invalid IL or missing references)
		//IL_2158: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fba: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fc4: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fc9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fa1: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fab: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fb0: Unknown result type (might be due to invalid IL or missing references)
		//IL_5545: Unknown result type (might be due to invalid IL or missing references)
		//IL_554f: Unknown result type (might be due to invalid IL or missing references)
		//IL_5554: Unknown result type (might be due to invalid IL or missing references)
		//IL_550b: Unknown result type (might be due to invalid IL or missing references)
		//IL_5515: Unknown result type (might be due to invalid IL or missing references)
		//IL_551a: Unknown result type (might be due to invalid IL or missing references)
		//IL_54e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_54ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_54f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_25f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_2601: Unknown result type (might be due to invalid IL or missing references)
		//IL_2606: Unknown result type (might be due to invalid IL or missing references)
		//IL_2408: Unknown result type (might be due to invalid IL or missing references)
		//IL_2423: Unknown result type (might be due to invalid IL or missing references)
		//IL_2429: Unknown result type (might be due to invalid IL or missing references)
		//IL_2188: Unknown result type (might be due to invalid IL or missing references)
		//IL_2192: Unknown result type (might be due to invalid IL or missing references)
		//IL_2197: Unknown result type (might be due to invalid IL or missing references)
		//IL_216f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2179: Unknown result type (might be due to invalid IL or missing references)
		//IL_217e: Unknown result type (might be due to invalid IL or missing references)
		//IL_556e: Unknown result type (might be due to invalid IL or missing references)
		//IL_5af8: Unknown result type (might be due to invalid IL or missing references)
		//IL_5b02: Unknown result type (might be due to invalid IL or missing references)
		//IL_5b07: Unknown result type (might be due to invalid IL or missing references)
		//IL_58dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_58ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_58f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_58f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_58a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_58af: Unknown result type (might be due to invalid IL or missing references)
		//IL_58b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_579d: Unknown result type (might be due to invalid IL or missing references)
		//IL_57ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_57b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_57b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_5765: Unknown result type (might be due to invalid IL or missing references)
		//IL_576f: Unknown result type (might be due to invalid IL or missing references)
		//IL_5774: Unknown result type (might be due to invalid IL or missing references)
		//IL_5a17: Unknown result type (might be due to invalid IL or missing references)
		//IL_5a26: Unknown result type (might be due to invalid IL or missing references)
		//IL_5a2b: Unknown result type (might be due to invalid IL or missing references)
		//IL_5a30: Unknown result type (might be due to invalid IL or missing references)
		//IL_59df: Unknown result type (might be due to invalid IL or missing references)
		//IL_59e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_59ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_53e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_53f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_53f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_293a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2944: Unknown result type (might be due to invalid IL or missing references)
		//IL_2949: Unknown result type (might be due to invalid IL or missing references)
		//IL_257a: Unknown result type (might be due to invalid IL or missing references)
		//IL_255b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2562: Unknown result type (might be due to invalid IL or missing references)
		//IL_2569: Unknown result type (might be due to invalid IL or missing references)
		//IL_256e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2573: Unknown result type (might be due to invalid IL or missing references)
		//IL_5923: Unknown result type (might be due to invalid IL or missing references)
		//IL_592a: Unknown result type (might be due to invalid IL or missing references)
		//IL_5931: Unknown result type (might be due to invalid IL or missing references)
		//IL_5936: Unknown result type (might be due to invalid IL or missing references)
		//IL_593b: Unknown result type (might be due to invalid IL or missing references)
		//IL_5940: Unknown result type (might be due to invalid IL or missing references)
		//IL_57fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_5803: Unknown result type (might be due to invalid IL or missing references)
		//IL_580a: Unknown result type (might be due to invalid IL or missing references)
		//IL_580f: Unknown result type (might be due to invalid IL or missing references)
		//IL_5814: Unknown result type (might be due to invalid IL or missing references)
		//IL_5819: Unknown result type (might be due to invalid IL or missing references)
		//IL_55b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_55bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_55c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_5478: Unknown result type (might be due to invalid IL or missing references)
		//IL_5482: Unknown result type (might be due to invalid IL or missing references)
		//IL_5487: Unknown result type (might be due to invalid IL or missing references)
		//IL_5434: Unknown result type (might be due to invalid IL or missing references)
		//IL_543e: Unknown result type (might be due to invalid IL or missing references)
		//IL_5443: Unknown result type (might be due to invalid IL or missing references)
		//IL_26f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_26fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_2700: Unknown result type (might be due to invalid IL or missing references)
		//IL_5a5d: Unknown result type (might be due to invalid IL or missing references)
		//IL_5a64: Unknown result type (might be due to invalid IL or missing references)
		//IL_5a6b: Unknown result type (might be due to invalid IL or missing references)
		//IL_5a70: Unknown result type (might be due to invalid IL or missing references)
		//IL_5a75: Unknown result type (might be due to invalid IL or missing references)
		//IL_5a7a: Unknown result type (might be due to invalid IL or missing references)
		//IL_5aa7: Unknown result type (might be due to invalid IL or missing references)
		//IL_5aae: Unknown result type (might be due to invalid IL or missing references)
		//IL_5ab5: Unknown result type (might be due to invalid IL or missing references)
		//IL_5aba: Unknown result type (might be due to invalid IL or missing references)
		//IL_5abf: Unknown result type (might be due to invalid IL or missing references)
		//IL_5ac4: Unknown result type (might be due to invalid IL or missing references)
		//IL_280f: Unknown result type (might be due to invalid IL or missing references)
		//IL_282a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2830: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c68: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c72: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c77: Unknown result type (might be due to invalid IL or missing references)
		//IL_2bec: Unknown result type (might be due to invalid IL or missing references)
		//IL_2bf6: Unknown result type (might be due to invalid IL or missing references)
		//IL_2bfb: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b3e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b45: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b4c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b51: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b56: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b5b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b6c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b76: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b7b: Unknown result type (might be due to invalid IL or missing references)
		//IL_304b: Unknown result type (might be due to invalid IL or missing references)
		//IL_3050: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d9c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2da1: Unknown result type (might be due to invalid IL or missing references)
		//IL_32fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_3307: Unknown result type (might be due to invalid IL or missing references)
		//IL_330c: Unknown result type (might be due to invalid IL or missing references)
		//IL_3231: Unknown result type (might be due to invalid IL or missing references)
		//IL_3238: Unknown result type (might be due to invalid IL or missing references)
		//IL_323f: Unknown result type (might be due to invalid IL or missing references)
		//IL_3244: Unknown result type (might be due to invalid IL or missing references)
		//IL_3249: Unknown result type (might be due to invalid IL or missing references)
		//IL_324e: Unknown result type (might be due to invalid IL or missing references)
		//IL_345a: Unknown result type (might be due to invalid IL or missing references)
		//IL_3464: Unknown result type (might be due to invalid IL or missing references)
		//IL_3469: Unknown result type (might be due to invalid IL or missing references)
		//IL_3c91: Unknown result type (might be due to invalid IL or missing references)
		//IL_3c9b: Unknown result type (might be due to invalid IL or missing references)
		//IL_3ca0: Unknown result type (might be due to invalid IL or missing references)
		//IL_3df1: Unknown result type (might be due to invalid IL or missing references)
		//IL_3dfb: Unknown result type (might be due to invalid IL or missing references)
		//IL_3e00: Unknown result type (might be due to invalid IL or missing references)
		//IL_3d4e: Unknown result type (might be due to invalid IL or missing references)
		//IL_3d5e: Unknown result type (might be due to invalid IL or missing references)
		//IL_3d63: Unknown result type (might be due to invalid IL or missing references)
		//IL_3d68: Unknown result type (might be due to invalid IL or missing references)
		//IL_3d20: Unknown result type (might be due to invalid IL or missing references)
		//IL_3d30: Unknown result type (might be due to invalid IL or missing references)
		//IL_3d3a: Unknown result type (might be due to invalid IL or missing references)
		//IL_3d3f: Unknown result type (might be due to invalid IL or missing references)
		//IL_3d44: Unknown result type (might be due to invalid IL or missing references)
		//IL_3fab: Unknown result type (might be due to invalid IL or missing references)
		//IL_3fb5: Unknown result type (might be due to invalid IL or missing references)
		//IL_3fba: Unknown result type (might be due to invalid IL or missing references)
		//IL_4166: Unknown result type (might be due to invalid IL or missing references)
		//IL_4170: Unknown result type (might be due to invalid IL or missing references)
		//IL_4175: Unknown result type (might be due to invalid IL or missing references)
		//IL_419b: Unknown result type (might be due to invalid IL or missing references)
		//IL_41a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_41aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_41af: Unknown result type (might be due to invalid IL or missing references)
		//IL_4267: Unknown result type (might be due to invalid IL or missing references)
		//IL_4271: Unknown result type (might be due to invalid IL or missing references)
		//IL_4276: Unknown result type (might be due to invalid IL or missing references)
		//IL_40eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_40f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_40fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_40ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_42e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_42f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_42f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_41e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_41ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_41f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_4139: Unknown result type (might be due to invalid IL or missing references)
		//IL_4143: Unknown result type (might be due to invalid IL or missing references)
		//IL_4148: Unknown result type (might be due to invalid IL or missing references)
		//IL_43c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_43c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_43cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_43d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_449e: Unknown result type (might be due to invalid IL or missing references)
		//IL_44a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_44ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_4408: Unknown result type (might be due to invalid IL or missing references)
		//IL_4412: Unknown result type (might be due to invalid IL or missing references)
		//IL_4417: Unknown result type (might be due to invalid IL or missing references)
		int num = 0;
		lavaBubbles = 0;
		Main.snowDust = 0;
		SandStormCount = 0;
		bool flag = Sandstorm.ShouldSandstormDustPersist();
		Vector2 vector3 = default(Vector2);
		for (int i = 0; i < 6000; i++)
		{
			Dust dust = Main.dust[i];
			if (i < Main.maxDustToDraw)
			{
				if (!dust.active)
				{
					continue;
				}
				dCount += 1f;
				DustLoader.SetupUpdateType(dust);
				ModDust modDust = DustLoader.GetDust(dust.type);
				if (modDust != null && !modDust.Update(dust))
				{
					DustLoader.TakeDownUpdateType(dust);
					continue;
				}
				if (dust.scale > 10f)
				{
					dust.active = false;
				}
				if (dust.firstFrame && !ChildSafety.Disabled && ChildSafety.DangerousDust(dust.type))
				{
					if (Main.rand.Next(2) == 0)
					{
						dust.firstFrame = false;
						dust.type = 16;
						dust.scale = Main.rand.NextFloat() * 1.6f + 0.3f;
						dust.color = Color.Transparent;
						dust.frame.X = 10 * dust.type;
						dust.frame.Y = 10 * Main.rand.Next(3);
						dust.shader = null;
						dust.customData = null;
						int num27 = dust.type / 100;
						dust.frame.X -= 1000 * num27;
						dust.frame.Y += 30 * num27;
						dust.noGravity = true;
					}
					else
					{
						dust.active = false;
					}
				}
				int num38 = dust.type;
				if ((uint)(num38 - 299) <= 2u || num38 == 305)
				{
					dust.scale *= 0.96f;
					dust.velocity.Y -= 0.01f;
				}
				if (dust.type == 35)
				{
					lavaBubbles++;
				}
				dust.position += dust.velocity;
				if (dust.type == 258)
				{
					dust.noGravity = true;
					dust.scale += 0.015f;
				}
				if (dust.type == 309)
				{
					float r = (float)(int)((Color)(ref dust.color)).R / 255f * dust.scale;
					float g = (float)(int)((Color)(ref dust.color)).G / 255f * dust.scale;
					float b = (float)(int)((Color)(ref dust.color)).B / 255f * dust.scale;
					Lighting.AddLight(dust.position, r, g, b);
					dust.scale *= 0.97f;
				}
				if (((dust.type >= 86 && dust.type <= 92) || dust.type == 286) && !dust.noLight && !dust.noLightEmittence)
				{
					float num49 = dust.scale * 0.6f;
					if (num49 > 1f)
					{
						num49 = 1f;
					}
					int num60 = dust.type - 85;
					float num70 = num49;
					float num81 = num49;
					float num91 = num49;
					switch (num60)
					{
					case 3:
						num70 *= 0f;
						num81 *= 0.1f;
						num91 *= 1.3f;
						break;
					case 5:
						num70 *= 1f;
						num81 *= 0.1f;
						num91 *= 0.1f;
						break;
					case 4:
						num70 *= 0f;
						num81 *= 1f;
						num91 *= 0.1f;
						break;
					case 1:
						num70 *= 0.9f;
						num81 *= 0f;
						num91 *= 0.9f;
						break;
					case 6:
						num70 *= 1.3f;
						num81 *= 1.3f;
						num91 *= 1.3f;
						break;
					case 2:
						num70 *= 0.9f;
						num81 *= 0.9f;
						num91 *= 0f;
						break;
					}
					Lighting.AddLight((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f), num49 * num70, num49 * num81, num49 * num91);
				}
				if ((dust.type >= 86 && dust.type <= 92) || dust.type == 286)
				{
					if (dust.customData != null && dust.customData is Player)
					{
						Player player = (Player)dust.customData;
						dust.position += player.position - player.oldPosition;
					}
					else if (dust.customData != null && dust.customData is Projectile)
					{
						Projectile projectile = (Projectile)dust.customData;
						if (projectile.active)
						{
							dust.position += projectile.position - projectile.oldPosition;
						}
					}
				}
				if (dust.type == 262 && !dust.noLight)
				{
					Vector3 rgb = new Vector3(0.9f, 0.6f, 0f) * dust.scale * 0.6f;
					Lighting.AddLight(dust.position, rgb);
				}
				if (dust.type == 240 && dust.customData != null && dust.customData is Projectile)
				{
					Projectile projectile2 = (Projectile)dust.customData;
					if (projectile2.active)
					{
						dust.position += projectile2.position - projectile2.oldPosition;
					}
				}
				if ((dust.type == 259 || dust.type == 6 || dust.type == 158 || dust.type == 135) && dust.customData != null && dust.customData is int)
				{
					if ((int)dust.customData == 0)
					{
						if (Collision.SolidCollision(dust.position - Vector2.One * 5f, 10, 10) && dust.fadeIn == 0f)
						{
							dust.scale *= 0.9f;
							dust.velocity *= 0.25f;
						}
					}
					else if ((int)dust.customData == 1)
					{
						dust.scale *= 0.98f;
						dust.velocity.Y *= 0.98f;
						if (Collision.SolidCollision(dust.position - Vector2.One * 5f, 10, 10) && dust.fadeIn == 0f)
						{
							dust.scale *= 0.9f;
							dust.velocity *= 0.25f;
						}
					}
				}
				if (dust.type == 263 || dust.type == 264)
				{
					if (!dust.noLight)
					{
						Vector3 rgb2 = ((Color)(ref dust.color)).ToVector3() * dust.scale * 0.4f;
						Lighting.AddLight(dust.position, rgb2);
					}
					if (dust.customData != null && dust.customData is Player)
					{
						Player player3 = (Player)dust.customData;
						dust.position += player3.position - player3.oldPosition;
						dust.customData = null;
					}
					else if (dust.customData != null && dust.customData is Projectile)
					{
						Projectile projectile3 = (Projectile)dust.customData;
						dust.position += projectile3.position - projectile3.oldPosition;
					}
				}
				if (dust.type == 230)
				{
					float num102 = dust.scale * 0.6f;
					float num2 = num102;
					float num13 = num102;
					float num19 = num102;
					num2 *= 0.5f;
					num13 *= 0.9f;
					num19 *= 1f;
					dust.scale += 0.02f;
					Lighting.AddLight((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f), num102 * num2, num102 * num13, num102 * num19);
					if (dust.customData != null && dust.customData is Player)
					{
						Vector2 center = ((Player)dust.customData).Center;
						Vector2 vector = dust.position - center;
						float num20 = ((Vector2)(ref vector)).Length();
						vector /= num20;
						dust.scale = Math.Min(dust.scale, num20 / 24f - 1f);
						dust.velocity -= vector * (100f / Math.Max(50f, num20));
					}
				}
				if (dust.type == 154 || dust.type == 218)
				{
					dust.rotation += dust.velocity.X * 0.3f;
					dust.scale -= 0.03f;
				}
				if (dust.type == 172)
				{
					float num21 = dust.scale * 0.5f;
					if (num21 > 1f)
					{
						num21 = 1f;
					}
					float num22 = num21;
					float num23 = num21;
					float num24 = num21;
					num22 *= 0f;
					num23 *= 0.25f;
					num24 *= 1f;
					Lighting.AddLight((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f), num21 * num22, num21 * num23, num21 * num24);
				}
				if (dust.type == 182)
				{
					dust.rotation += 1f;
					if (!dust.noLight)
					{
						float num25 = dust.scale * 0.25f;
						if (num25 > 1f)
						{
							num25 = 1f;
						}
						float num26 = num25;
						float num28 = num25;
						float num29 = num25;
						num26 *= 1f;
						num28 *= 0.2f;
						num29 *= 0.1f;
						Lighting.AddLight((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f), num25 * num26, num25 * num28, num25 * num29);
					}
					if (dust.customData != null && dust.customData is Player)
					{
						Player player4 = (Player)dust.customData;
						dust.position += player4.position - player4.oldPosition;
						dust.customData = null;
					}
				}
				if (dust.type == 261)
				{
					if (!dust.noLight && !dust.noLightEmittence)
					{
						float num30 = dust.scale * 0.3f;
						if (num30 > 1f)
						{
							num30 = 1f;
						}
						Lighting.AddLight(dust.position, new Vector3(0.4f, 0.6f, 0.7f) * num30);
					}
					if (dust.noGravity)
					{
						dust.velocity *= 0.93f;
						if (dust.fadeIn == 0f)
						{
							dust.scale += 0.0025f;
						}
					}
					dust.velocity *= new Vector2(0.97f, 0.99f);
					dust.scale -= 0.0025f;
					if (dust.customData != null && dust.customData is Player)
					{
						Player player5 = (Player)dust.customData;
						dust.position += player5.position - player5.oldPosition;
					}
				}
				if (dust.type == 254)
				{
					float num31 = dust.scale * 0.35f;
					if (num31 > 1f)
					{
						num31 = 1f;
					}
					float num32 = num31;
					float num33 = num31;
					float num34 = num31;
					num32 *= 0.9f;
					num33 *= 0.1f;
					num34 *= 0.75f;
					Lighting.AddLight((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f), num31 * num32, num31 * num33, num31 * num34);
				}
				if (dust.type == 255)
				{
					float num35 = dust.scale * 0.25f;
					if (num35 > 1f)
					{
						num35 = 1f;
					}
					float num36 = num35;
					float num37 = num35;
					float num39 = num35;
					num36 *= 0.9f;
					num37 *= 0.1f;
					num39 *= 0.75f;
					Lighting.AddLight((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f), num35 * num36, num35 * num37, num35 * num39);
				}
				if (dust.type == 211 && dust.noLight && Collision.SolidCollision(dust.position, 4, 4))
				{
					dust.active = false;
				}
				if (dust.type == 284 && Collision.SolidCollision(dust.position - Vector2.One * 4f, 8, 8) && dust.fadeIn == 0f)
				{
					dust.velocity *= 0.25f;
				}
				if (dust.type == 213 || dust.type == 260)
				{
					dust.rotation = 0f;
					float num40 = dust.scale / 2.5f * 0.2f;
					Vector3 vector2 = Vector3.Zero;
					switch (dust.type)
					{
					case 213:
						((Vector3)(ref vector2))._002Ector(255f, 217f, 48f);
						break;
					case 260:
						((Vector3)(ref vector2))._002Ector(255f, 48f, 48f);
						break;
					}
					vector2 /= 255f;
					if (num40 > 1f)
					{
						num40 = 1f;
					}
					vector2 *= num40;
					Lighting.AddLight((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f), vector2.X, vector2.Y, vector2.Z);
				}
				if (dust.type == 157)
				{
					float num41 = dust.scale * 0.2f;
					float num42 = num41;
					float num43 = num41;
					float num44 = num41;
					num42 *= 0.25f;
					num43 *= 1f;
					num44 *= 0.5f;
					Lighting.AddLight((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f), num41 * num42, num41 * num43, num41 * num44);
				}
				if (dust.type == 206)
				{
					dust.scale -= 0.1f;
					float num45 = dust.scale * 0.4f;
					float num46 = num45;
					float num47 = num45;
					float num48 = num45;
					num46 *= 0.1f;
					num47 *= 0.6f;
					num48 *= 1f;
					Lighting.AddLight((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f), num45 * num46, num45 * num47, num45 * num48);
				}
				if (dust.type == 163)
				{
					float num50 = dust.scale * 0.25f;
					float num51 = num50;
					float num52 = num50;
					float num53 = num50;
					num51 *= 0.25f;
					num52 *= 1f;
					num53 *= 0.05f;
					Lighting.AddLight((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f), num50 * num51, num50 * num52, num50 * num53);
				}
				if (dust.type == 205)
				{
					float num54 = dust.scale * 0.25f;
					float num55 = num54;
					float num56 = num54;
					float num57 = num54;
					num55 *= 1f;
					num56 *= 0.05f;
					num57 *= 1f;
					Lighting.AddLight((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f), num54 * num55, num54 * num56, num54 * num57);
				}
				if (dust.type == 170)
				{
					float num58 = dust.scale * 0.5f;
					float num59 = num58;
					float num61 = num58;
					float num62 = num58;
					num59 *= 1f;
					num61 *= 1f;
					num62 *= 0.05f;
					Lighting.AddLight((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f), num58 * num59, num58 * num61, num58 * num62);
				}
				if (dust.type == 156)
				{
					float num63 = dust.scale * 0.6f;
					_ = dust.type;
					float num64 = num63;
					num64 *= 1f;
					Lighting.AddLight((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f), 12, num63);
				}
				if (dust.type == 234)
				{
					float lightAmount = dust.scale * 0.6f;
					_ = dust.type;
					Lighting.AddLight((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f), 13, lightAmount);
				}
				if (dust.type == 175)
				{
					dust.scale -= 0.05f;
				}
				if (dust.type == 174)
				{
					dust.scale -= 0.01f;
					float num65 = dust.scale * 1f;
					if (num65 > 0.6f)
					{
						num65 = 0.6f;
					}
					Lighting.AddLight((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f), num65, num65 * 0.4f, 0f);
				}
				if (dust.type == 235)
				{
					((Vector2)(ref vector3))._002Ector((float)Main.rand.Next(-100, 101), (float)Main.rand.Next(-100, 101));
					((Vector2)(ref vector3)).Normalize();
					vector3 *= 15f;
					dust.scale -= 0.01f;
				}
				else if (dust.type == 228 || dust.type == 279 || dust.type == 229 || dust.type == 6 || dust.type == 242 || dust.type == 135 || dust.type == 127 || dust.type == 187 || dust.type == 75 || dust.type == 169 || dust.type == 29 || (dust.type >= 59 && dust.type <= 65) || dust.type == 158 || dust.type == 293 || dust.type == 294 || dust.type == 295 || dust.type == 296 || dust.type == 297 || dust.type == 298 || dust.type == 302 || dust.type == 307 || dust.type == 310)
				{
					if (!dust.noGravity)
					{
						dust.velocity.Y += 0.05f;
					}
					if (dust.type == 229 || dust.type == 228 || dust.type == 279)
					{
						if (dust.customData != null && dust.customData is NPC)
						{
							NPC nPC = (NPC)dust.customData;
							dust.position += nPC.position - nPC.oldPos[1];
						}
						else if (dust.customData != null && dust.customData is Player)
						{
							Player player6 = (Player)dust.customData;
							dust.position += player6.position - player6.oldPosition;
						}
						else if (dust.customData != null && dust.customData is Vector2)
						{
							Vector2 vector4 = (Vector2)dust.customData - dust.position;
							if (vector4 != Vector2.Zero)
							{
								((Vector2)(ref vector4)).Normalize();
							}
							dust.velocity = (dust.velocity * 4f + vector4 * ((Vector2)(ref dust.velocity)).Length()) / 5f;
						}
					}
					if (!dust.noLight && !dust.noLightEmittence)
					{
						float num66 = dust.scale * 1.4f;
						if (dust.type == 29)
						{
							if (num66 > 1f)
							{
								num66 = 1f;
							}
							Lighting.AddLight((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f), num66 * 0.1f, num66 * 0.4f, num66);
						}
						else if (dust.type == 75)
						{
							if (num66 > 1f)
							{
								num66 = 1f;
							}
							if (dust.customData is float)
							{
								Lighting.AddLight((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f), 8, num66 * (float)dust.customData);
							}
							else
							{
								Lighting.AddLight((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f), 8, num66);
							}
						}
						else if (dust.type == 169)
						{
							if (num66 > 1f)
							{
								num66 = 1f;
							}
							Lighting.AddLight((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f), 11, num66);
						}
						else if (dust.type == 135)
						{
							if (num66 > 1f)
							{
								num66 = 1f;
							}
							Lighting.AddLight((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f), 9, num66);
						}
						else if (dust.type == 158)
						{
							if (num66 > 1f)
							{
								num66 = 1f;
							}
							Lighting.AddLight((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f), 10, num66);
						}
						else if (dust.type == 228)
						{
							if (num66 > 1f)
							{
								num66 = 1f;
							}
							Lighting.AddLight((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f), num66 * 0.7f, num66 * 0.65f, num66 * 0.3f);
						}
						else if (dust.type == 229)
						{
							if (num66 > 1f)
							{
								num66 = 1f;
							}
							Lighting.AddLight((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f), num66 * 0.3f, num66 * 0.65f, num66 * 0.7f);
						}
						else if (dust.type == 242)
						{
							if (num66 > 1f)
							{
								num66 = 1f;
							}
							Lighting.AddLight((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f), 15, num66);
						}
						else if (dust.type == 293)
						{
							if (num66 > 1f)
							{
								num66 = 1f;
							}
							num66 *= 0.95f;
							Lighting.AddLight((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f), 16, num66);
						}
						else if (dust.type == 294)
						{
							if (num66 > 1f)
							{
								num66 = 1f;
							}
							Lighting.AddLight((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f), 17, num66);
						}
						else if (dust.type >= 59 && dust.type <= 65)
						{
							if (num66 > 0.8f)
							{
								num66 = 0.8f;
							}
							Lighting.AddLight((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f), 1 + dust.type - 59, num66);
						}
						else if (dust.type == 127)
						{
							num66 *= 1.3f;
							if (num66 > 1f)
							{
								num66 = 1f;
							}
							Lighting.AddLight((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f), num66, num66 * 0.45f, num66 * 0.2f);
						}
						else if (dust.type == 187)
						{
							num66 *= 1.3f;
							if (num66 > 1f)
							{
								num66 = 1f;
							}
							Lighting.AddLight((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f), num66 * 0.2f, num66 * 0.45f, num66);
						}
						else if (dust.type == 295)
						{
							if (num66 > 1f)
							{
								num66 = 1f;
							}
							Lighting.AddLight((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f), 18, num66);
						}
						else if (dust.type == 296)
						{
							if (num66 > 1f)
							{
								num66 = 1f;
							}
							Lighting.AddLight((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f), 19, num66);
						}
						else if (dust.type == 297)
						{
							if (num66 > 1f)
							{
								num66 = 1f;
							}
							Lighting.AddLight((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f), 20, num66);
						}
						else if (dust.type == 298)
						{
							if (num66 > 1f)
							{
								num66 = 1f;
							}
							Lighting.AddLight((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f), 21, num66);
						}
						else if (dust.type == 307)
						{
							if (num66 > 1f)
							{
								num66 = 1f;
							}
							Lighting.AddLight((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f), 22, num66);
						}
						else if (dust.type == 310)
						{
							if (num66 > 1f)
							{
								num66 = 1f;
							}
							Lighting.AddLight((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f), 23, num66);
						}
						else
						{
							if (num66 > 0.6f)
							{
								num66 = 0.6f;
							}
							Lighting.AddLight((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f), num66, num66 * 0.65f, num66 * 0.4f);
						}
					}
				}
				else if (dust.type == 306)
				{
					if (!dust.noGravity)
					{
						dust.velocity.Y += 0.05f;
					}
					dust.scale -= 0.04f;
					if (Collision.SolidCollision(dust.position - Vector2.One * 5f, 10, 10) && dust.fadeIn == 0f)
					{
						dust.scale *= 0.9f;
						dust.velocity *= 0.25f;
					}
				}
				else if (dust.type == 269)
				{
					if (!dust.noLight)
					{
						float num67 = dust.scale * 1.4f;
						if (num67 > 1f)
						{
							num67 = 1f;
						}
						Vector3 rgb3 = new Vector3(0.7f, 0.65f, 0.3f) * num67;
						Lighting.AddLight(dust.position, rgb3);
					}
					if (dust.customData != null && dust.customData is Vector2)
					{
						Vector2 vector5 = (Vector2)dust.customData - dust.position;
						dust.velocity.X += 1f * (float)Math.Sign(vector5.X) * dust.scale;
					}
				}
				else if (dust.type == 159)
				{
					float num68 = dust.scale * 1.3f;
					if (num68 > 1f)
					{
						num68 = 1f;
					}
					Lighting.AddLight((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f), num68, num68, num68 * 0.1f);
					if (dust.noGravity)
					{
						if (dust.scale < 0.7f)
						{
							dust.velocity *= 1.075f;
						}
						else if (Main.rand.Next(2) == 0)
						{
							dust.velocity *= -0.95f;
						}
						else
						{
							dust.velocity *= 1.05f;
						}
						dust.scale -= 0.03f;
					}
					else
					{
						dust.scale += 0.005f;
						dust.velocity *= 0.9f;
						dust.velocity.X += (float)Main.rand.Next(-10, 11) * 0.02f;
						dust.velocity.Y += (float)Main.rand.Next(-10, 11) * 0.02f;
						if (Main.rand.Next(5) == 0)
						{
							int num69 = NewDust(dust.position, 4, 4, dust.type);
							Main.dust[num69].noGravity = true;
							Main.dust[num69].scale = dust.scale * 2.5f;
						}
					}
				}
				else if (dust.type == 164)
				{
					float num71 = dust.scale;
					if (num71 > 1f)
					{
						num71 = 1f;
					}
					Lighting.AddLight((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f), num71, num71 * 0.1f, num71 * 0.8f);
					if (dust.noGravity)
					{
						if (dust.scale < 0.7f)
						{
							dust.velocity *= 1.075f;
						}
						else if (Main.rand.Next(2) == 0)
						{
							dust.velocity *= -0.95f;
						}
						else
						{
							dust.velocity *= 1.05f;
						}
						dust.scale -= 0.03f;
					}
					else
					{
						dust.scale -= 0.005f;
						dust.velocity *= 0.9f;
						dust.velocity.X += (float)Main.rand.Next(-10, 11) * 0.02f;
						dust.velocity.Y += (float)Main.rand.Next(-10, 11) * 0.02f;
						if (Main.rand.Next(5) == 0)
						{
							int num72 = NewDust(dust.position, 4, 4, dust.type);
							Main.dust[num72].noGravity = true;
							Main.dust[num72].scale = dust.scale * 2.5f;
						}
					}
				}
				else if (dust.type == 173)
				{
					float num73 = dust.scale;
					if (num73 > 1f)
					{
						num73 = 1f;
					}
					Lighting.AddLight((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f), num73 * 0.4f, num73 * 0.1f, num73);
					if (dust.noGravity)
					{
						dust.velocity *= 0.8f;
						dust.velocity.X += (float)Main.rand.Next(-20, 21) * 0.01f;
						dust.velocity.Y += (float)Main.rand.Next(-20, 21) * 0.01f;
						dust.scale -= 0.01f;
					}
					else
					{
						dust.scale -= 0.015f;
						dust.velocity *= 0.8f;
						dust.velocity.X += (float)Main.rand.Next(-10, 11) * 0.005f;
						dust.velocity.Y += (float)Main.rand.Next(-10, 11) * 0.005f;
						if (Main.rand.Next(10) == 10)
						{
							int num74 = NewDust(dust.position, 4, 4, dust.type);
							Main.dust[num74].noGravity = true;
							Main.dust[num74].scale = dust.scale;
						}
					}
				}
				else if (dust.type == 304)
				{
					dust.velocity.Y = (float)Math.Sin(dust.rotation) / 5f;
					dust.rotation += 0.015f;
					if (dust.scale < 1.15f)
					{
						dust.alpha = Math.Max(0, dust.alpha - 20);
						dust.scale += 0.0015f;
					}
					else
					{
						dust.alpha += 6;
						if (dust.alpha >= 255)
						{
							dust.active = false;
						}
					}
					if (dust.customData != null && dust.customData is Player)
					{
						Player player7 = (Player)dust.customData;
						float num75 = Utils.Remap(dust.scale, 1f, 1.05f, 1f, 0f);
						if (num75 > 0f)
						{
							dust.position += player7.velocity * num75;
						}
						float num76 = player7.Center.X - dust.position.X;
						if (Math.Abs(num76) > 20f)
						{
							float value = num76 * 0.01f;
							dust.velocity.X = MathHelper.Lerp(dust.velocity.X, value, num75 * 0.2f);
						}
					}
				}
				else if (dust.type == 184)
				{
					if (!dust.noGravity)
					{
						dust.velocity *= 0f;
						dust.scale -= 0.01f;
					}
				}
				else if (dust.type == 160 || dust.type == 162)
				{
					float num77 = dust.scale * 1.3f;
					if (num77 > 1f)
					{
						num77 = 1f;
					}
					if (dust.type == 162)
					{
						Lighting.AddLight((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f), num77, num77 * 0.7f, num77 * 0.1f);
					}
					else
					{
						Lighting.AddLight((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f), num77 * 0.1f, num77, num77);
					}
					if (dust.noGravity)
					{
						dust.velocity *= 0.8f;
						dust.velocity.X += (float)Main.rand.Next(-20, 21) * 0.04f;
						dust.velocity.Y += (float)Main.rand.Next(-20, 21) * 0.04f;
						dust.scale -= 0.1f;
					}
					else
					{
						dust.scale -= 0.1f;
						dust.velocity.X += (float)Main.rand.Next(-10, 11) * 0.02f;
						dust.velocity.Y += (float)Main.rand.Next(-10, 11) * 0.02f;
						if ((double)dust.scale > 0.3 && Main.rand.Next(50) == 0)
						{
							int num78 = NewDust(new Vector2(dust.position.X - 4f, dust.position.Y - 4f), 1, 1, dust.type);
							Main.dust[num78].noGravity = true;
							Main.dust[num78].scale = dust.scale * 1.5f;
						}
					}
				}
				else if (dust.type == 168)
				{
					float num79 = dust.scale * 0.8f;
					if ((double)num79 > 0.55)
					{
						num79 = 0.55f;
					}
					Lighting.AddLight((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f), num79, 0f, num79 * 0.8f);
					dust.scale += 0.03f;
					dust.velocity.X += (float)Main.rand.Next(-10, 11) * 0.02f;
					dust.velocity.Y += (float)Main.rand.Next(-10, 11) * 0.02f;
					dust.velocity *= 0.99f;
				}
				else if (dust.type >= 139 && dust.type < 143)
				{
					dust.velocity.X *= 0.98f;
					dust.velocity.Y *= 0.98f;
					if (dust.velocity.Y < 1f)
					{
						dust.velocity.Y += 0.05f;
					}
					dust.scale += 0.009f;
					dust.rotation -= dust.velocity.X * 0.4f;
					if (dust.velocity.X > 0f)
					{
						dust.rotation += 0.005f;
					}
					else
					{
						dust.rotation -= 0.005f;
					}
				}
				else if (dust.type == 14 || dust.type == 16 || dust.type == 31 || dust.type == 46 || dust.type == 124 || dust.type == 186 || dust.type == 188 || dust.type == 303)
				{
					dust.velocity.Y *= 0.98f;
					dust.velocity.X *= 0.98f;
					if (dust.type == 31)
					{
						if (dust.customData != null && dust.customData is float)
						{
							float num80 = (float)dust.customData;
							dust.velocity.Y += num80;
						}
						if (dust.customData != null && dust.customData is NPC)
						{
							NPC nPC2 = (NPC)dust.customData;
							dust.position += nPC2.position - nPC2.oldPosition;
							if (dust.noGravity)
							{
								dust.velocity *= 1.02f;
							}
							dust.alpha -= 70;
							if (dust.alpha < 0)
							{
								dust.alpha = 0;
							}
							dust.scale *= 0.97f;
							if (dust.scale <= 0.01f)
							{
								dust.scale = 0.0001f;
								dust.alpha = 255;
							}
						}
						else if (dust.noGravity)
						{
							dust.velocity *= 1.02f;
							dust.scale += 0.02f;
							dust.alpha += 4;
							if (dust.alpha > 255)
							{
								dust.scale = 0.0001f;
								dust.alpha = 255;
							}
						}
					}
					if (dust.type == 303 && dust.noGravity)
					{
						dust.velocity *= 1.02f;
						dust.scale += 0.03f;
						if (dust.alpha < 90)
						{
							dust.alpha = 90;
						}
						dust.alpha += 4;
						if (dust.alpha > 255)
						{
							dust.scale = 0.0001f;
							dust.alpha = 255;
						}
					}
				}
				else if (dust.type == 32)
				{
					dust.scale -= 0.01f;
					dust.velocity.X *= 0.96f;
					if (!dust.noGravity)
					{
						dust.velocity.Y += 0.1f;
					}
				}
				else if (dust.type >= 244 && dust.type <= 247)
				{
					dust.rotation += 0.1f * dust.scale;
					Color color = Lighting.GetColor((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f));
					byte num113 = (byte)((((Color)(ref color)).R + ((Color)(ref color)).G + ((Color)(ref color)).B) / 3);
					float num82 = ((float)(int)num113 / 270f + 1f) / 2f;
					float num83 = ((float)(int)num113 / 270f + 1f) / 2f;
					float num84 = ((float)(int)num113 / 270f + 1f) / 2f;
					num82 *= dust.scale * 0.9f;
					num83 *= dust.scale * 0.9f;
					num84 *= dust.scale * 0.9f;
					if (dust.alpha < 255)
					{
						dust.scale += 0.09f;
						if (dust.scale >= 1f)
						{
							dust.scale = 1f;
							dust.alpha = 255;
						}
					}
					else
					{
						if ((double)dust.scale < 0.8)
						{
							dust.scale -= 0.01f;
						}
						if ((double)dust.scale < 0.5)
						{
							dust.scale -= 0.01f;
						}
					}
					float num85 = 1f;
					if (dust.type == 244)
					{
						num82 *= 0.8862745f;
						num83 *= 0.4627451f;
						num84 *= 0.29803923f;
						num85 = 0.9f;
					}
					else if (dust.type == 245)
					{
						num82 *= 0.5137255f;
						num83 *= 0.6745098f;
						num84 *= 0.6784314f;
						num85 = 1f;
					}
					else if (dust.type == 246)
					{
						num82 *= 0.8f;
						num83 *= 0.70980394f;
						num84 *= 24f / 85f;
						num85 = 1.1f;
					}
					else if (dust.type == 247)
					{
						num82 *= 0.6f;
						num83 *= 0.6745098f;
						num84 *= 37f / 51f;
						num85 = 1.2f;
					}
					num82 *= num85;
					num83 *= num85;
					num84 *= num85;
					if (!dust.noLightEmittence)
					{
						Lighting.AddLight((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f), num82, num83, num84);
					}
				}
				else if (dust.type == 43)
				{
					dust.rotation += 0.1f * dust.scale;
					Color color2 = Lighting.GetColor((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f));
					float num86 = (float)(int)((Color)(ref color2)).R / 270f;
					float num87 = (float)(int)((Color)(ref color2)).G / 270f;
					float num88 = (float)(int)((Color)(ref color2)).B / 270f;
					float num89 = (float)(int)((Color)(ref dust.color)).R / 255f;
					float num90 = (float)(int)((Color)(ref dust.color)).G / 255f;
					float num92 = (float)(int)((Color)(ref dust.color)).B / 255f;
					num86 *= dust.scale * 1.07f * num89;
					num87 *= dust.scale * 1.07f * num90;
					num88 *= dust.scale * 1.07f * num92;
					if (dust.alpha < 255)
					{
						dust.scale += 0.09f;
						if (dust.scale >= 1f)
						{
							dust.scale = 1f;
							dust.alpha = 255;
						}
					}
					else
					{
						if ((double)dust.scale < 0.8)
						{
							dust.scale -= 0.01f;
						}
						if ((double)dust.scale < 0.5)
						{
							dust.scale -= 0.01f;
						}
					}
					if ((double)num86 < 0.05 && (double)num87 < 0.05 && (double)num88 < 0.05)
					{
						dust.active = false;
					}
					else if (!dust.noLightEmittence)
					{
						Lighting.AddLight((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f), num86, num87, num88);
					}
					if (dust.customData != null && dust.customData is Player)
					{
						Player player8 = (Player)dust.customData;
						dust.position += player8.position - player8.oldPosition;
					}
				}
				else if (dust.type == 15 || dust.type == 57 || dust.type == 58 || dust.type == 274 || dust.type == 292)
				{
					dust.velocity.Y *= 0.98f;
					dust.velocity.X *= 0.98f;
					if (!dust.noLightEmittence)
					{
						float num93 = dust.scale;
						if (dust.type != 15)
						{
							num93 = dust.scale * 0.8f;
						}
						if (dust.noLight)
						{
							dust.velocity *= 0.95f;
						}
						if (num93 > 1f)
						{
							num93 = 1f;
						}
						if (dust.type == 15)
						{
							Lighting.AddLight((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f), num93 * 0.45f, num93 * 0.55f, num93);
						}
						else if (dust.type == 57)
						{
							Lighting.AddLight((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f), num93 * 0.95f, num93 * 0.95f, num93 * 0.45f);
						}
						else if (dust.type == 58)
						{
							Lighting.AddLight((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f), num93, num93 * 0.55f, num93 * 0.75f);
						}
					}
				}
				else if (dust.type == 204)
				{
					if (dust.fadeIn > dust.scale)
					{
						dust.scale += 0.02f;
					}
					else
					{
						dust.scale -= 0.02f;
					}
					dust.velocity *= 0.95f;
				}
				else if (dust.type == 110)
				{
					float num94 = dust.scale * 0.1f;
					if (num94 > 1f)
					{
						num94 = 1f;
					}
					Lighting.AddLight((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f), num94 * 0.2f, num94, num94 * 0.5f);
				}
				else if (dust.type == 111)
				{
					float num95 = dust.scale * 0.125f;
					if (num95 > 1f)
					{
						num95 = 1f;
					}
					Lighting.AddLight((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f), num95 * 0.2f, num95 * 0.7f, num95);
				}
				else if (dust.type == 112)
				{
					float num96 = dust.scale * 0.1f;
					if (num96 > 1f)
					{
						num96 = 1f;
					}
					Lighting.AddLight((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f), num96 * 0.8f, num96 * 0.2f, num96 * 0.8f);
				}
				else if (dust.type == 113)
				{
					float num97 = dust.scale * 0.1f;
					if (num97 > 1f)
					{
						num97 = 1f;
					}
					Lighting.AddLight((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f), num97 * 0.2f, num97 * 0.3f, num97 * 1.3f);
				}
				else if (dust.type == 114)
				{
					float num98 = dust.scale * 0.1f;
					if (num98 > 1f)
					{
						num98 = 1f;
					}
					Lighting.AddLight((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f), num98 * 1.2f, num98 * 0.5f, num98 * 0.4f);
				}
				else if (dust.type == 311)
				{
					float num99 = dust.scale * 0.1f;
					if (num99 > 1f)
					{
						num99 = 1f;
					}
					Lighting.AddLight((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f), 16, num99);
				}
				else if (dust.type == 312)
				{
					float num100 = dust.scale * 0.1f;
					if (num100 > 1f)
					{
						num100 = 1f;
					}
					Lighting.AddLight((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f), 9, num100);
				}
				else if (dust.type == 313)
				{
					float num101 = dust.scale * 0.25f;
					if (num101 > 1f)
					{
						num101 = 1f;
					}
					Lighting.AddLight((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f), num101 * 1f, num101 * 0.8f, num101 * 0.6f);
				}
				else if (dust.type == 66)
				{
					if (dust.velocity.X < 0f)
					{
						dust.rotation -= 1f;
					}
					else
					{
						dust.rotation += 1f;
					}
					dust.velocity.Y *= 0.98f;
					dust.velocity.X *= 0.98f;
					dust.scale += 0.02f;
					float num103 = dust.scale;
					if (dust.type != 15)
					{
						num103 = dust.scale * 0.8f;
					}
					if (num103 > 1f)
					{
						num103 = 1f;
					}
					Lighting.AddLight((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f), num103 * ((float)(int)((Color)(ref dust.color)).R / 255f), num103 * ((float)(int)((Color)(ref dust.color)).G / 255f), num103 * ((float)(int)((Color)(ref dust.color)).B / 255f));
				}
				else if (dust.type == 267)
				{
					if (dust.velocity.X < 0f)
					{
						dust.rotation -= 1f;
					}
					else
					{
						dust.rotation += 1f;
					}
					dust.velocity.Y *= 0.98f;
					dust.velocity.X *= 0.98f;
					dust.scale += 0.02f;
					float num104 = dust.scale * 0.8f;
					if (num104 > 1f)
					{
						num104 = 1f;
					}
					if (dust.noLight)
					{
						dust.noLight = false;
					}
					if (!dust.noLight && !dust.noLightEmittence)
					{
						Lighting.AddLight((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f), num104 * ((float)(int)((Color)(ref dust.color)).R / 255f), num104 * ((float)(int)((Color)(ref dust.color)).G / 255f), num104 * ((float)(int)((Color)(ref dust.color)).B / 255f));
					}
				}
				else if (dust.type == 20 || dust.type == 21 || dust.type == 231)
				{
					dust.scale += 0.005f;
					dust.velocity.Y *= 0.94f;
					dust.velocity.X *= 0.94f;
					float num105 = dust.scale * 0.8f;
					if (num105 > 1f)
					{
						num105 = 1f;
					}
					if (dust.type == 21 && !dust.noLightEmittence)
					{
						num105 = dust.scale * 0.4f;
						Lighting.AddLight((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f), num105 * 0.8f, num105 * 0.3f, num105);
					}
					else if (dust.type == 231)
					{
						num105 = dust.scale * 0.4f;
						Lighting.AddLight((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f), num105, num105 * 0.5f, num105 * 0.3f);
					}
					else
					{
						Lighting.AddLight((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f), num105 * 0.3f, num105 * 0.6f, num105);
					}
				}
				else if (dust.type == 27 || dust.type == 45)
				{
					if (dust.type == 27 && dust.fadeIn >= 100f)
					{
						if ((double)dust.scale >= 1.5)
						{
							dust.scale -= 0.01f;
						}
						else
						{
							dust.scale -= 0.05f;
						}
						if ((double)dust.scale <= 0.5)
						{
							dust.scale -= 0.05f;
						}
						if ((double)dust.scale <= 0.25)
						{
							dust.scale -= 0.05f;
						}
					}
					dust.velocity *= 0.94f;
					dust.scale += 0.002f;
					float num106 = dust.scale;
					if (dust.noLight)
					{
						num106 *= 0.1f;
						dust.scale -= 0.06f;
						if (dust.scale < 1f)
						{
							dust.scale -= 0.06f;
						}
						if (Main.player[Main.myPlayer].wet)
						{
							dust.position += Main.player[Main.myPlayer].velocity * 0.5f;
						}
						else
						{
							dust.position += Main.player[Main.myPlayer].velocity;
						}
					}
					if (num106 > 1f)
					{
						num106 = 1f;
					}
					Lighting.AddLight((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f), num106 * 0.6f, num106 * 0.2f, num106);
				}
				else if (dust.type == 55 || dust.type == 56 || dust.type == 73 || dust.type == 74)
				{
					dust.velocity *= 0.98f;
					if (!dust.noLightEmittence)
					{
						float num107 = dust.scale * 0.8f;
						if (dust.type == 55)
						{
							if (num107 > 1f)
							{
								num107 = 1f;
							}
							Lighting.AddLight((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f), num107, num107, num107 * 0.6f);
						}
						else if (dust.type == 73)
						{
							if (num107 > 1f)
							{
								num107 = 1f;
							}
							Lighting.AddLight((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f), num107, num107 * 0.35f, num107 * 0.5f);
						}
						else if (dust.type == 74)
						{
							if (num107 > 1f)
							{
								num107 = 1f;
							}
							Lighting.AddLight((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f), num107 * 0.35f, num107, num107 * 0.5f);
						}
						else
						{
							num107 = dust.scale * 1.2f;
							if (num107 > 1f)
							{
								num107 = 1f;
							}
							Lighting.AddLight((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f), num107 * 0.35f, num107 * 0.5f, num107);
						}
					}
				}
				else if (dust.type == 71 || dust.type == 72)
				{
					dust.velocity *= 0.98f;
					float num108 = dust.scale;
					if (num108 > 1f)
					{
						num108 = 1f;
					}
					Lighting.AddLight((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f), num108 * 0.2f, 0f, num108 * 0.1f);
				}
				else if (dust.type == 76)
				{
					Main.snowDust++;
					dust.scale += 0.009f;
					float y = Main.player[Main.myPlayer].velocity.Y;
					if (y > 0f && dust.fadeIn == 0f && dust.velocity.Y < y)
					{
						dust.velocity.Y = MathHelper.Lerp(dust.velocity.Y, y, 0.04f);
					}
					if (!dust.noLight && y > 0f)
					{
						dust.position.Y += Main.player[Main.myPlayer].velocity.Y * 0.2f;
					}
					if (Collision.SolidCollision(dust.position - Vector2.One * 5f, 10, 10) && dust.fadeIn == 0f)
					{
						dust.scale *= 0.9f;
						dust.velocity *= 0.25f;
					}
				}
				else if (dust.type == 270)
				{
					dust.velocity *= 1.0050251f;
					dust.scale += 0.01f;
					dust.rotation = 0f;
					if (Collision.SolidCollision(dust.position - Vector2.One * 5f, 10, 10) && dust.fadeIn == 0f)
					{
						dust.scale *= 0.95f;
						dust.velocity *= 0.25f;
					}
					else
					{
						dust.velocity.Y = (float)Math.Sin(dust.position.X * 0.0043982295f) * 2f;
						dust.velocity.Y -= 3f;
						dust.velocity.Y /= 20f;
					}
				}
				else if (dust.type == 271)
				{
					dust.velocity *= 1.0050251f;
					dust.scale += 0.003f;
					dust.rotation = 0f;
					dust.velocity.Y -= 4f;
					dust.velocity.Y /= 6f;
				}
				else if (dust.type == 268)
				{
					SandStormCount++;
					dust.velocity *= 1.0050251f;
					dust.scale += 0.01f;
					if (!flag)
					{
						dust.scale -= 0.05f;
					}
					dust.rotation = 0f;
					float y2 = Main.player[Main.myPlayer].velocity.Y;
					if (y2 > 0f && dust.fadeIn == 0f && dust.velocity.Y < y2)
					{
						dust.velocity.Y = MathHelper.Lerp(dust.velocity.Y, y2, 0.04f);
					}
					if (!dust.noLight && y2 > 0f)
					{
						dust.position.Y += y2 * 0.2f;
					}
					if (Collision.SolidCollision(dust.position - Vector2.One * 5f, 10, 10) && dust.fadeIn == 0f)
					{
						dust.scale *= 0.9f;
						dust.velocity *= 0.25f;
					}
					else
					{
						dust.velocity.Y = (float)Math.Sin(dust.position.X * 0.0043982295f) * 2f;
						dust.velocity.Y += 3f;
					}
				}
				else if (!dust.noGravity && dust.type != 41 && dust.type != 44 && dust.type != 309)
				{
					if (dust.type == 107)
					{
						dust.velocity *= 0.9f;
					}
					else
					{
						dust.velocity.Y += 0.1f;
					}
				}
				if (dust.type == 5 || (dust.type == 273 && dust.noGravity))
				{
					dust.scale -= 0.04f;
				}
				if (dust.type == 308 || dust.type == 33 || dust.type == 52 || dust.type == 266 || dust.type == 98 || dust.type == 99 || dust.type == 100 || dust.type == 101 || dust.type == 102 || dust.type == 103 || dust.type == 104 || dust.type == 105 || dust.type == 123 || dust.type == 288)
				{
					if (dust.velocity.X == 0f)
					{
						if (Collision.SolidCollision(dust.position, 2, 2))
						{
							dust.scale = 0f;
						}
						dust.rotation += 0.5f;
						dust.scale -= 0.01f;
					}
					if (Collision.WetCollision(new Vector2(dust.position.X, dust.position.Y), 4, 4))
					{
						dust.alpha += 20;
						dust.scale -= 0.1f;
					}
					dust.alpha += 2;
					dust.scale -= 0.005f;
					if (dust.alpha > 255)
					{
						dust.scale = 0f;
					}
					if (dust.velocity.Y > 4f)
					{
						dust.velocity.Y = 4f;
					}
					if (dust.noGravity)
					{
						if (dust.velocity.X < 0f)
						{
							dust.rotation -= 0.2f;
						}
						else
						{
							dust.rotation += 0.2f;
						}
						dust.scale += 0.03f;
						dust.velocity.X *= 1.05f;
						dust.velocity.Y += 0.15f;
					}
				}
				if (dust.type == 35 && dust.noGravity)
				{
					dust.scale += 0.03f;
					if (dust.scale < 1f)
					{
						dust.velocity.Y += 0.075f;
					}
					dust.velocity.X *= 1.08f;
					if (dust.velocity.X > 0f)
					{
						dust.rotation += 0.01f;
					}
					else
					{
						dust.rotation -= 0.01f;
					}
					float num109 = dust.scale * 0.6f;
					if (num109 > 1f)
					{
						num109 = 1f;
					}
					Lighting.AddLight((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f + 1f), num109, num109 * 0.3f, num109 * 0.1f);
				}
				else if (dust.type == 152 && dust.noGravity)
				{
					dust.scale += 0.03f;
					if (dust.scale < 1f)
					{
						dust.velocity.Y += 0.075f;
					}
					dust.velocity.X *= 1.08f;
					if (dust.velocity.X > 0f)
					{
						dust.rotation += 0.01f;
					}
					else
					{
						dust.rotation -= 0.01f;
					}
				}
				else if (dust.type == 67 || dust.type == 92)
				{
					float num110 = dust.scale;
					if (num110 > 1f)
					{
						num110 = 1f;
					}
					if (dust.noLight)
					{
						num110 *= 0.1f;
					}
					Lighting.AddLight((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f), 0f, num110 * 0.8f, num110);
				}
				else if (dust.type == 185)
				{
					float num111 = dust.scale;
					if (num111 > 1f)
					{
						num111 = 1f;
					}
					if (dust.noLight)
					{
						num111 *= 0.1f;
					}
					Lighting.AddLight((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f), num111 * 0.1f, num111 * 0.7f, num111);
				}
				else if (dust.type == 107)
				{
					float num112 = dust.scale * 0.5f;
					if (num112 > 1f)
					{
						num112 = 1f;
					}
					if (!dust.noLightEmittence)
					{
						Lighting.AddLight((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f), num112 * 0.1f, num112, num112 * 0.4f);
					}
				}
				else if (dust.type == 34 || dust.type == 35 || dust.type == 152)
				{
					if (!Collision.WetCollision(new Vector2(dust.position.X, dust.position.Y - 8f), 4, 4))
					{
						dust.scale = 0f;
					}
					else
					{
						dust.alpha += Main.rand.Next(2);
						if (dust.alpha > 255)
						{
							dust.scale = 0f;
						}
						dust.velocity.Y = -0.5f;
						if (dust.type == 34)
						{
							dust.scale += 0.005f;
						}
						else
						{
							dust.alpha++;
							dust.scale -= 0.01f;
							dust.velocity.Y = -0.2f;
						}
						dust.velocity.X += (float)Main.rand.Next(-10, 10) * 0.002f;
						if ((double)dust.velocity.X < -0.25)
						{
							dust.velocity.X = -0.25f;
						}
						if ((double)dust.velocity.X > 0.25)
						{
							dust.velocity.X = 0.25f;
						}
					}
					if (dust.type == 35)
					{
						float num3 = dust.scale * 0.3f + 0.4f;
						if (num3 > 1f)
						{
							num3 = 1f;
						}
						Lighting.AddLight((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f), num3, num3 * 0.5f, num3 * 0.3f);
					}
				}
				if (dust.type == 68)
				{
					float num4 = dust.scale * 0.3f;
					if (num4 > 1f)
					{
						num4 = 1f;
					}
					Lighting.AddLight((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f), num4 * 0.1f, num4 * 0.2f, num4);
				}
				if (dust.type == 70)
				{
					float num5 = dust.scale * 0.3f;
					if (num5 > 1f)
					{
						num5 = 1f;
					}
					Lighting.AddLight((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f), num5 * 0.5f, 0f, num5);
				}
				if (dust.type == 41)
				{
					dust.velocity.X += (float)Main.rand.Next(-10, 11) * 0.01f;
					dust.velocity.Y += (float)Main.rand.Next(-10, 11) * 0.01f;
					if ((double)dust.velocity.X > 0.75)
					{
						dust.velocity.X = 0.75f;
					}
					if ((double)dust.velocity.X < -0.75)
					{
						dust.velocity.X = -0.75f;
					}
					if ((double)dust.velocity.Y > 0.75)
					{
						dust.velocity.Y = 0.75f;
					}
					if ((double)dust.velocity.Y < -0.75)
					{
						dust.velocity.Y = -0.75f;
					}
					dust.scale += 0.007f;
					float num6 = dust.scale * 0.7f;
					if (num6 > 1f)
					{
						num6 = 1f;
					}
					Lighting.AddLight((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f), num6 * 0.4f, num6 * 0.9f, num6);
				}
				else if (dust.type == 44)
				{
					dust.velocity.X += (float)Main.rand.Next(-10, 11) * 0.003f;
					dust.velocity.Y += (float)Main.rand.Next(-10, 11) * 0.003f;
					if ((double)dust.velocity.X > 0.35)
					{
						dust.velocity.X = 0.35f;
					}
					if ((double)dust.velocity.X < -0.35)
					{
						dust.velocity.X = -0.35f;
					}
					if ((double)dust.velocity.Y > 0.35)
					{
						dust.velocity.Y = 0.35f;
					}
					if ((double)dust.velocity.Y < -0.35)
					{
						dust.velocity.Y = -0.35f;
					}
					dust.scale += 0.0085f;
					float num7 = dust.scale * 0.7f;
					if (num7 > 1f)
					{
						num7 = 1f;
					}
					Lighting.AddLight((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f), num7 * 0.7f, num7, num7 * 0.8f);
				}
				else if (dust.type != 304 && (modDust == null || !modDust.MidUpdate(dust)))
				{
					dust.velocity.X *= 0.99f;
				}
				if (dust.type == 322 && !dust.noGravity)
				{
					dust.scale *= 0.98f;
				}
				if (dust.type != 79 && dust.type != 268 && dust.type != 304)
				{
					dust.rotation += dust.velocity.X * 0.5f;
				}
				if (dust.fadeIn > 0f && dust.fadeIn < 100f)
				{
					if (dust.type == 235)
					{
						dust.scale += 0.007f;
						int num8 = (int)dust.fadeIn - 1;
						if (num8 >= 0 && num8 <= 255)
						{
							Vector2 vector6 = dust.position - Main.player[num8].Center;
							float num9 = ((Vector2)(ref vector6)).Length();
							num9 = 100f - num9;
							if (num9 > 0f)
							{
								dust.scale -= num9 * 0.0015f;
							}
							((Vector2)(ref vector6)).Normalize();
							float num10 = (1f - dust.scale) * 20f;
							vector6 *= 0f - num10;
							dust.velocity = (dust.velocity * 4f + vector6) / 5f;
						}
					}
					else if (dust.type == 46)
					{
						dust.scale += 0.1f;
					}
					else if (dust.type == 213 || dust.type == 260)
					{
						dust.scale += 0.1f;
					}
					else
					{
						dust.scale += 0.03f;
					}
					if (dust.scale > dust.fadeIn)
					{
						dust.fadeIn = 0f;
					}
				}
				else if (dust.type != 304)
				{
					if (dust.type == 213 || dust.type == 260)
					{
						dust.scale -= 0.2f;
					}
					else
					{
						dust.scale -= 0.01f;
					}
				}
				if (dust.type >= 130 && dust.type <= 134)
				{
					float num11 = dust.scale;
					if (num11 > 1f)
					{
						num11 = 1f;
					}
					if (dust.type == 130)
					{
						Lighting.AddLight((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f), num11 * 1f, num11 * 0.5f, num11 * 0.4f);
					}
					if (dust.type == 131)
					{
						Lighting.AddLight((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f), num11 * 0.4f, num11 * 1f, num11 * 0.6f);
					}
					if (dust.type == 132)
					{
						Lighting.AddLight((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f), num11 * 0.3f, num11 * 0.5f, num11 * 1f);
					}
					if (dust.type == 133)
					{
						Lighting.AddLight((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f), num11 * 0.9f, num11 * 0.9f, num11 * 0.3f);
					}
					if (dust.noGravity)
					{
						dust.velocity *= 0.93f;
						if (dust.fadeIn == 0f)
						{
							dust.scale += 0.0025f;
						}
					}
					else if (dust.type == 131)
					{
						dust.velocity *= 0.98f;
						dust.velocity.Y -= 0.1f;
						dust.scale += 0.0025f;
					}
					else
					{
						dust.velocity *= 0.95f;
						dust.scale -= 0.0025f;
					}
				}
				else if (dust.type == 278)
				{
					float num12 = dust.scale;
					if (num12 > 1f)
					{
						num12 = 1f;
					}
					if (!dust.noLight && !dust.noLightEmittence)
					{
						Lighting.AddLight(dust.position, ((Color)(ref dust.color)).ToVector3() * num12);
					}
					if (dust.noGravity)
					{
						dust.velocity *= 0.93f;
						if (dust.fadeIn == 0f)
						{
							dust.scale += 0.0025f;
						}
					}
					else
					{
						dust.velocity *= 0.95f;
						dust.scale -= 0.0025f;
					}
					if (WorldGen.SolidTile(Framing.GetTileSafely(dust.position)) && dust.fadeIn == 0f && !dust.noGravity)
					{
						dust.scale *= 0.9f;
						dust.velocity *= 0.25f;
					}
				}
				else if (dust.type >= 219 && dust.type <= 223)
				{
					float num14 = dust.scale;
					if (num14 > 1f)
					{
						num14 = 1f;
					}
					if (!dust.noLight)
					{
						if (dust.type == 219)
						{
							Lighting.AddLight((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f), num14 * 1f, num14 * 0.5f, num14 * 0.4f);
						}
						if (dust.type == 220)
						{
							Lighting.AddLight((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f), num14 * 0.4f, num14 * 1f, num14 * 0.6f);
						}
						if (dust.type == 221)
						{
							Lighting.AddLight((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f), num14 * 0.3f, num14 * 0.5f, num14 * 1f);
						}
						if (dust.type == 222)
						{
							Lighting.AddLight((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f), num14 * 0.9f, num14 * 0.9f, num14 * 0.3f);
						}
					}
					if (dust.noGravity)
					{
						dust.velocity *= 0.93f;
						if (dust.fadeIn == 0f)
						{
							dust.scale += 0.0025f;
						}
					}
					dust.velocity *= new Vector2(0.97f, 0.99f);
					dust.scale -= 0.0025f;
					if (dust.customData != null && dust.customData is Player)
					{
						Player player9 = (Player)dust.customData;
						dust.position += player9.position - player9.oldPosition;
					}
				}
				else if (dust.type == 226)
				{
					float num15 = dust.scale;
					if (num15 > 1f)
					{
						num15 = 1f;
					}
					if (!dust.noLight)
					{
						Lighting.AddLight((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f), num15 * 0.2f, num15 * 0.7f, num15 * 1f);
					}
					if (dust.noGravity)
					{
						dust.velocity *= 0.93f;
						if (dust.fadeIn == 0f)
						{
							dust.scale += 0.0025f;
						}
					}
					dust.velocity *= new Vector2(0.97f, 0.99f);
					if (dust.customData != null && dust.customData is Player)
					{
						Player player10 = (Player)dust.customData;
						dust.position += player10.position - player10.oldPosition;
					}
					dust.scale -= 0.01f;
				}
				else if (dust.type == 272)
				{
					float num16 = dust.scale;
					if (num16 > 1f)
					{
						num16 = 1f;
					}
					if (!dust.noLight)
					{
						Lighting.AddLight((int)(dust.position.X / 16f), (int)(dust.position.Y / 16f), num16 * 0.5f, num16 * 0.2f, num16 * 0.8f);
					}
					if (dust.noGravity)
					{
						dust.velocity *= 0.93f;
						if (dust.fadeIn == 0f)
						{
							dust.scale += 0.0025f;
						}
					}
					dust.velocity *= new Vector2(0.97f, 0.99f);
					if (dust.customData != null && dust.customData is Player)
					{
						Player player2 = (Player)dust.customData;
						dust.position += player2.position - player2.oldPosition;
					}
					if (dust.customData != null && dust.customData is NPC)
					{
						NPC nPC3 = (NPC)dust.customData;
						dust.position += nPC3.position - nPC3.oldPosition;
					}
					dust.scale -= 0.01f;
				}
				else if (dust.type != 304 && dust.noGravity)
				{
					dust.velocity *= 0.92f;
					if (dust.fadeIn == 0f)
					{
						dust.scale -= 0.04f;
					}
				}
				if (dust.position.Y > Main.screenPosition.Y + (float)Main.screenHeight)
				{
					dust.active = false;
				}
				float num17 = 0.1f;
				if ((double)dCount == 0.5)
				{
					dust.scale -= 0.001f;
				}
				if ((double)dCount == 0.6)
				{
					dust.scale -= 0.0025f;
				}
				if ((double)dCount == 0.7)
				{
					dust.scale -= 0.005f;
				}
				if ((double)dCount == 0.8)
				{
					dust.scale -= 0.01f;
				}
				if ((double)dCount == 0.9)
				{
					dust.scale -= 0.02f;
				}
				if ((double)dCount == 0.5)
				{
					num17 = 0.11f;
				}
				if ((double)dCount == 0.6)
				{
					num17 = 0.13f;
				}
				if ((double)dCount == 0.7)
				{
					num17 = 0.16f;
				}
				if ((double)dCount == 0.8)
				{
					num17 = 0.22f;
				}
				if ((double)dCount == 0.9)
				{
					num17 = 0.25f;
				}
				if (dust.scale < num17)
				{
					dust.active = false;
				}
				DustLoader.TakeDownUpdateType(dust);
			}
			else
			{
				dust.active = false;
			}
		}
		int num18 = num;
		if ((double)num18 > (double)Main.maxDustToDraw * 0.9)
		{
			dCount = 0.9f;
		}
		else if ((double)num18 > (double)Main.maxDustToDraw * 0.8)
		{
			dCount = 0.8f;
		}
		else if ((double)num18 > (double)Main.maxDustToDraw * 0.7)
		{
			dCount = 0.7f;
		}
		else if ((double)num18 > (double)Main.maxDustToDraw * 0.6)
		{
			dCount = 0.6f;
		}
		else if ((double)num18 > (double)Main.maxDustToDraw * 0.5)
		{
			dCount = 0.5f;
		}
		else
		{
			dCount = 0f;
		}
	}

	public Color GetAlpha(Color newColor)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_016d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0173: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_021b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0238: Unknown result type (might be due to invalid IL or missing references)
		//IL_0291: Unknown result type (might be due to invalid IL or missing references)
		//IL_029b: Unknown result type (might be due to invalid IL or missing references)
		//IL_02be: Unknown result type (might be due to invalid IL or missing references)
		//IL_030a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0340: Unknown result type (might be due to invalid IL or missing references)
		//IL_039c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0375: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_0410: Unknown result type (might be due to invalid IL or missing references)
		//IL_0433: Unknown result type (might be due to invalid IL or missing references)
		//IL_0463: Unknown result type (might be due to invalid IL or missing references)
		//IL_0487: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04db: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0521: Unknown result type (might be due to invalid IL or missing references)
		//IL_0539: Unknown result type (might be due to invalid IL or missing references)
		//IL_0560: Unknown result type (might be due to invalid IL or missing references)
		//IL_05aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_05de: Unknown result type (might be due to invalid IL or missing references)
		//IL_0602: Unknown result type (might be due to invalid IL or missing references)
		//IL_0636: Unknown result type (might be due to invalid IL or missing references)
		//IL_0673: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0814: Unknown result type (might be due to invalid IL or missing references)
		//IL_082b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0836: Unknown result type (might be due to invalid IL or missing references)
		//IL_083b: Unknown result type (might be due to invalid IL or missing references)
		//IL_084e: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_08bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_08c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_08dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_086b: Unknown result type (might be due to invalid IL or missing references)
		//IL_086c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0876: Unknown result type (might be due to invalid IL or missing references)
		//IL_087b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0894: Unknown result type (might be due to invalid IL or missing references)
		//IL_08f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_08f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_08fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0900: Unknown result type (might be due to invalid IL or missing references)
		//IL_0919: Unknown result type (might be due to invalid IL or missing references)
		//IL_0935: Unknown result type (might be due to invalid IL or missing references)
		//IL_0936: Unknown result type (might be due to invalid IL or missing references)
		//IL_0940: Unknown result type (might be due to invalid IL or missing references)
		//IL_0945: Unknown result type (might be due to invalid IL or missing references)
		//IL_095e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0973: Unknown result type (might be due to invalid IL or missing references)
		//IL_0993: Unknown result type (might be due to invalid IL or missing references)
		//IL_0994: Unknown result type (might be due to invalid IL or missing references)
		//IL_099e: Unknown result type (might be due to invalid IL or missing references)
		//IL_09a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_09bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aaf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aba: Unknown result type (might be due to invalid IL or missing references)
		//IL_0abf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0adb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a98: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ce0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c0a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0be4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c3f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c5f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c83: Unknown result type (might be due to invalid IL or missing references)
		ModDust modDust = DustLoader.GetDust(type);
		if (modDust != null)
		{
			Color? modColor = modDust.GetAlpha(this, newColor);
			if (modColor.HasValue)
			{
				return modColor.Value;
			}
		}
		float num = (float)(255 - alpha) / 255f;
		switch (type)
		{
		case 323:
			return Color.White;
		case 308:
		case 309:
			return new Color(225, 200, 250, 190);
		case 299:
		case 300:
		case 301:
		case 305:
		{
			Color color = default(Color);
			switch (type)
			{
			default:
				((Color)(ref color))._002Ector(255, 150, 150, 200);
				break;
			case 299:
				((Color)(ref color))._002Ector(50, 255, 50, 200);
				break;
			case 300:
				((Color)(ref color))._002Ector(50, 200, 255, 255);
				break;
			case 301:
				((Color)(ref color))._002Ector(255, 50, 125, 200);
				break;
			case 305:
				((Color)(ref color))._002Ector(200, 50, 200, 200);
				break;
			}
			return color;
		}
		default:
		{
			if (type == 304)
			{
				return Color.White * num;
			}
			if (type == 306)
			{
				return this.color * num;
			}
			if (type == 292)
			{
				return Color.White;
			}
			if (type == 259)
			{
				return new Color(230, 230, 230, 230);
			}
			if (type == 261)
			{
				return new Color(230, 230, 230, 115);
			}
			if (type == 254 || type == 255)
			{
				return new Color(255, 255, 255, 0);
			}
			if (type == 258)
			{
				return new Color(150, 50, 50, 0);
			}
			if (type == 263 || type == 264)
			{
				return new Color(((Color)(ref this.color)).R / 2 + 127, ((Color)(ref this.color)).G + 127, ((Color)(ref this.color)).B + 127, ((Color)(ref this.color)).A / 8) * 0.5f;
			}
			if (type == 235)
			{
				return new Color(255, 255, 255, 0);
			}
			if (((type >= 86 && type <= 91) || type == 262 || type == 286) && !noLight)
			{
				return new Color(255, 255, 255, 0);
			}
			if (type == 213 || type == 260)
			{
				int num7 = (int)(scale / 2.5f * 255f);
				return new Color(num7, num7, num7, num7);
			}
			if (type == 64 && alpha == 255 && noLight)
			{
				return new Color(255, 255, 255, 0);
			}
			if (type == 197)
			{
				return new Color(250, 250, 250, 150);
			}
			if ((type >= 110 && type <= 114) || type == 311 || type == 312 || type == 313)
			{
				return new Color(200, 200, 200, 0);
			}
			if (type == 204)
			{
				return new Color(255, 255, 255, 0);
			}
			if (type == 181)
			{
				return new Color(200, 200, 200, 0);
			}
			if (type == 182 || type == 206)
			{
				return new Color(255, 255, 255, 0);
			}
			if (type == 159)
			{
				return new Color(250, 250, 250, 50);
			}
			if (type == 163 || type == 205)
			{
				return new Color(250, 250, 250, 0);
			}
			if (type == 170)
			{
				return new Color(200, 200, 200, 100);
			}
			if (type == 180)
			{
				return new Color(200, 200, 200, 0);
			}
			if (type == 175)
			{
				return new Color(200, 200, 200, 0);
			}
			if (type == 183)
			{
				return new Color(50, 0, 0, 0);
			}
			if (type == 172)
			{
				return new Color(250, 250, 250, 150);
			}
			if (type == 160 || type == 162 || type == 164 || type == 173)
			{
				int num8 = (int)(250f * scale);
				return new Color(num8, num8, num8, 0);
			}
			if (type == 92 || type == 106 || type == 107)
			{
				return new Color(255, 255, 255, 0);
			}
			if (type == 185)
			{
				return new Color(200, 200, 255, 125);
			}
			if (type == 127 || type == 187)
			{
				return new Color((int)((Color)(ref newColor)).R, (int)((Color)(ref newColor)).G, (int)((Color)(ref newColor)).B, 25);
			}
			if (type == 156 || type == 230 || type == 234)
			{
				return new Color(255, 255, 255, 0);
			}
			if (type == 270)
			{
				return new Color(((Color)(ref newColor)).R / 2 + 127, ((Color)(ref newColor)).G / 2 + 127, ((Color)(ref newColor)).B / 2 + 127, 25);
			}
			if (type == 271)
			{
				return new Color(((Color)(ref newColor)).R / 2 + 127, ((Color)(ref newColor)).G / 2 + 127, ((Color)(ref newColor)).B / 2 + 127, 127);
			}
			if (type == 6 || type == 242 || type == 174 || type == 135 || type == 75 || type == 20 || type == 21 || type == 231 || type == 169 || (type >= 130 && type <= 134) || type == 158 || type == 293 || type == 294 || type == 295 || type == 296 || type == 297 || type == 298 || type == 307 || type == 310)
			{
				return new Color((int)((Color)(ref newColor)).R, (int)((Color)(ref newColor)).G, (int)((Color)(ref newColor)).B, 25);
			}
			if (type == 278)
			{
				Color result = default(Color);
				((Color)(ref result))._002Ector(((Color)(ref newColor)).ToVector3() * ((Color)(ref this.color)).ToVector3());
				((Color)(ref result)).A = 25;
				return result;
			}
			if (type >= 219 && type <= 223)
			{
				newColor = Color.Lerp(newColor, Color.White, 0.5f);
				return new Color((int)((Color)(ref newColor)).R, (int)((Color)(ref newColor)).G, (int)((Color)(ref newColor)).B, 25);
			}
			if (type == 226 || type == 272)
			{
				newColor = Color.Lerp(newColor, Color.White, 0.8f);
				return new Color((int)((Color)(ref newColor)).R, (int)((Color)(ref newColor)).G, (int)((Color)(ref newColor)).B, 25);
			}
			if (type == 228)
			{
				newColor = Color.Lerp(newColor, Color.White, 0.8f);
				return new Color((int)((Color)(ref newColor)).R, (int)((Color)(ref newColor)).G, (int)((Color)(ref newColor)).B, 25);
			}
			if (type == 279)
			{
				int a = ((Color)(ref newColor)).A;
				newColor = Color.Lerp(newColor, Color.White, 0.8f);
				return new Color((int)((Color)(ref newColor)).R, (int)((Color)(ref newColor)).G, (int)((Color)(ref newColor)).B, a) * MathHelper.Min(scale, 1f);
			}
			if (type == 229 || type == 269)
			{
				newColor = Color.Lerp(newColor, Color.White, 0.6f);
				return new Color((int)((Color)(ref newColor)).R, (int)((Color)(ref newColor)).G, (int)((Color)(ref newColor)).B, 25);
			}
			if ((type == 68 || type == 70) && noGravity)
			{
				return new Color(255, 255, 255, 0);
			}
			int num4;
			int num3;
			int num2;
			if (type == 157)
			{
				num4 = (num3 = (num2 = 255));
				float num5 = (float)(int)Main.mouseTextColor / 100f - 1.6f;
				num4 = (int)((float)num4 * num5);
				num3 = (int)((float)num3 * num5);
				num2 = (int)((float)num2 * num5);
				int a2 = (int)(100f * num5);
				num4 += 50;
				if (num4 > 255)
				{
					num4 = 255;
				}
				num3 += 50;
				if (num3 > 255)
				{
					num3 = 255;
				}
				num2 += 50;
				if (num2 > 255)
				{
					num2 = 255;
				}
				return new Color(num4, num3, num2, a2);
			}
			if (type == 284)
			{
				Color result2 = default(Color);
				((Color)(ref result2))._002Ector(((Color)(ref newColor)).ToVector4() * ((Color)(ref this.color)).ToVector4());
				((Color)(ref result2)).A = ((Color)(ref this.color)).A;
				return result2;
			}
			if (type == 15 || type == 274 || type == 20 || type == 21 || type == 29 || type == 35 || type == 41 || type == 44 || type == 27 || type == 45 || type == 55 || type == 56 || type == 57 || type == 58 || type == 73 || type == 74)
			{
				num = (num + 3f) / 4f;
			}
			else if (type == 43)
			{
				num = (num + 9f) / 10f;
			}
			else
			{
				if (type >= 244 && type <= 247)
				{
					return new Color(255, 255, 255, 0);
				}
				if (type == 66)
				{
					return new Color((int)((Color)(ref newColor)).R, (int)((Color)(ref newColor)).G, (int)((Color)(ref newColor)).B, 0);
				}
				if (type == 267)
				{
					return new Color((int)((Color)(ref this.color)).R, (int)((Color)(ref this.color)).G, (int)((Color)(ref this.color)).B, 0);
				}
				if (type == 71)
				{
					return new Color(200, 200, 200, 0);
				}
				if (type == 72)
				{
					return new Color(200, 200, 200, 200);
				}
			}
			num4 = (int)((float)(int)((Color)(ref newColor)).R * num);
			num3 = (int)((float)(int)((Color)(ref newColor)).G * num);
			num2 = (int)((float)(int)((Color)(ref newColor)).B * num);
			int num6 = ((Color)(ref newColor)).A - alpha;
			if (num6 < 0)
			{
				num6 = 0;
			}
			if (num6 > 255)
			{
				num6 = 255;
			}
			return new Color(num4, num3, num2, num6);
		}
		}
	}

	public Color GetColor(Color newColor)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		if (type == 284)
		{
			return Color.Transparent;
		}
		int num2 = ((Color)(ref color)).R - (255 - ((Color)(ref newColor)).R);
		int num3 = ((Color)(ref color)).G - (255 - ((Color)(ref newColor)).G);
		int num4 = ((Color)(ref color)).B - (255 - ((Color)(ref newColor)).B);
		int num5 = ((Color)(ref color)).A - (255 - ((Color)(ref newColor)).A);
		if (num2 < 0)
		{
			num2 = 0;
		}
		if (num2 > 255)
		{
			num2 = 255;
		}
		if (num3 < 0)
		{
			num3 = 0;
		}
		if (num3 > 255)
		{
			num3 = 255;
		}
		if (num4 < 0)
		{
			num4 = 0;
		}
		if (num4 > 255)
		{
			num4 = 255;
		}
		if (num5 < 0)
		{
			num5 = 0;
		}
		if (num5 > 255)
		{
			num5 = 255;
		}
		return new Color(num2, num3, num4, num5);
	}

	public float GetVisualRotation()
	{
		if (type == 304)
		{
			return 0f;
		}
		return rotation;
	}

	public float GetVisualScale()
	{
		if (type == 304)
		{
			return 1f;
		}
		return scale;
	}
}
