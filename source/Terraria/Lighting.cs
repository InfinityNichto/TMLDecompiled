using System;
using Microsoft.Xna.Framework;
using Terraria.Graphics;
using Terraria.Graphics.Light;
using Terraria.ID;
using Terraria.Utilities;

namespace Terraria;

public class Lighting
{
	private const float DEFAULT_GLOBAL_BRIGHTNESS = 1.2f;

	private const float BLIND_GLOBAL_BRIGHTNESS = 1f;

	[Old]
	public static int OffScreenTiles = 45;

	private static LightMode _mode = LightMode.Color;

	internal static readonly LightingEngine NewEngine = new LightingEngine();

	public static readonly LegacyLighting LegacyEngine = new LegacyLighting(Main.Camera);

	private static ILightingEngine _activeEngine;

	public static float GlobalBrightness { get; set; }

	public static LightMode Mode
	{
		get
		{
			return _mode;
		}
		set
		{
			_mode = value;
			switch (_mode)
			{
			case LightMode.Color:
				_activeEngine = NewEngine;
				LegacyEngine.Mode = 0;
				OffScreenTiles = 35;
				break;
			case LightMode.White:
				_activeEngine = LegacyEngine;
				LegacyEngine.Mode = 1;
				break;
			case LightMode.Retro:
				_activeEngine = LegacyEngine;
				LegacyEngine.Mode = 2;
				break;
			case LightMode.Trippy:
				_activeEngine = LegacyEngine;
				LegacyEngine.Mode = 3;
				break;
			}
			Main.renderCount = 0;
			Main.renderNow = false;
		}
	}

	public static bool NotRetro
	{
		get
		{
			if (Mode != LightMode.Retro)
			{
				return Mode != LightMode.Trippy;
			}
			return false;
		}
	}

	public static bool UsingNewLighting => Mode == LightMode.Color;

	public static bool UpdateEveryFrame
	{
		get
		{
			if (Main.LightingEveryFrame && !Main.RenderTargetsRequired)
			{
				return !NotRetro;
			}
			return false;
		}
	}

	public static void Initialize()
	{
		GlobalBrightness = 1.2f;
		NewEngine.Rebuild();
		LegacyEngine.Rebuild();
		if (_activeEngine == null)
		{
			Mode = LightMode.Color;
		}
	}

	public static void LightTiles(int firstX, int lastX, int firstY, int lastY)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		Main.render = true;
		UpdateGlobalBrightness();
		_activeEngine.ProcessArea(new Rectangle(firstX, firstY, lastX - firstX, lastY - firstY));
	}

	private static void UpdateGlobalBrightness()
	{
		GlobalBrightness = 1.2f;
		if (Main.player[Main.myPlayer].blind)
		{
			GlobalBrightness = 1f;
		}
	}

	public static float Brightness(int x, int y)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		Vector3 color = _activeEngine.GetColor(x, y);
		return GlobalBrightness * (color.X + color.Y + color.Z) / 3f;
	}

	public static Vector3 GetSubLight(Vector2 position)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		Vector2 vector = position / 16f - new Vector2(0.5f, 0.5f);
		Vector2 vector2 = default(Vector2);
		((Vector2)(ref vector2))._002Ector(vector.X % 1f, vector.Y % 1f);
		int num = (int)vector.X;
		int num2 = (int)vector.Y;
		Vector3 color5 = _activeEngine.GetColor(num, num2);
		Vector3 color2 = _activeEngine.GetColor(num + 1, num2);
		Vector3 color3 = _activeEngine.GetColor(num, num2 + 1);
		Vector3 color4 = _activeEngine.GetColor(num + 1, num2 + 1);
		Vector3 val = Vector3.Lerp(color5, color2, vector2.X);
		Vector3 value2 = Vector3.Lerp(color3, color4, vector2.X);
		return Vector3.Lerp(val, value2, vector2.Y);
	}

	/// <summary>
	/// Adds light to the world at the specified coordinates.<para />
	/// This overload takes in world coordinates and a Vector3 containing float values typically ranging from 0 to 1. Values greater than 1 will cause the light to propagate farther. A <see cref="T:Microsoft.Xna.Framework.Vector3" /> is used for this method instead of <see cref="T:Microsoft.Xna.Framework.Color" /> to allow these overflow values.
	/// </summary>
	/// <param name="position"></param>
	/// <param name="rgb"></param>
	public static void AddLight(Vector2 position, Vector3 rgb)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		AddLight((int)(position.X / 16f), (int)(position.Y / 16f), rgb.X, rgb.Y, rgb.Z);
	}

	/// <summary>
	/// <summary>
	/// Adds light to the world at the specified coordinates.<para />
	/// This overload takes in world coordinates and float values typically ranging from 0 to 1. Values greater than 1 will cause the light to propagate farther.
	/// </summary>
	/// </summary>
	/// <param name="position"></param>
	/// <param name="r"></param>
	/// <param name="g"></param>
	/// <param name="b"></param>
	public static void AddLight(Vector2 position, float r, float g, float b)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		AddLight((int)(position.X / 16f), (int)(position.Y / 16f), r, g, b);
	}

	public static void AddLight(int i, int j, int torchID, float lightAmount)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		TorchID.TorchColor(torchID, out var R, out var G, out var B);
		_activeEngine.AddLight(i, j, new Vector3(R * lightAmount, G * lightAmount, B * lightAmount));
	}

	public static void AddLight(Vector2 position, int torchID)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		TorchID.TorchColor(torchID, out var R, out var G, out var B);
		AddLight((int)position.X / 16, (int)position.Y / 16, R, G, B);
	}

	/// <summary>
	/// Adds light to the world at the specified coordinates.<para />
	/// This overload takes in tile coordinates and float values typically ranging from 0 to 1. Values greater than 1 will cause the light to propagate farther.
	/// </summary>
	/// <param name="i"></param>
	/// <param name="j"></param>
	/// <param name="r"></param>
	/// <param name="g"></param>
	/// <param name="b"></param>
	public static void AddLight(int i, int j, float r, float g, float b)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		if (!Main.gamePaused && Main.netMode != 2)
		{
			_activeEngine.AddLight(i, j, new Vector3(r, g, b));
		}
	}

	public static void NextLightMode()
	{
		Mode++;
		if (!Enum.IsDefined(typeof(LightMode), Mode))
		{
			Mode = LightMode.White;
		}
		Clear();
	}

	public static void Clear()
	{
		_activeEngine.Clear();
	}

	public static Color GetColor(Point tileCoords)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		if (Main.gameMenu)
		{
			return Color.White;
		}
		return new Color(_activeEngine.GetColor(tileCoords.X, tileCoords.Y) * GlobalBrightness);
	}

	public static Color GetColor(Point tileCoords, Color originalColor)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		if (Main.gameMenu)
		{
			return originalColor;
		}
		return new Color(_activeEngine.GetColor(tileCoords.X, tileCoords.Y) * ((Color)(ref originalColor)).ToVector3());
	}

	public static Color GetColor(int x, int y, Color oldColor)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		if (Main.gameMenu)
		{
			return oldColor;
		}
		return new Color(_activeEngine.GetColor(x, y) * ((Color)(ref oldColor)).ToVector3());
	}

	public static Color GetColorClamped(int x, int y, Color oldColor)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		if (Main.gameMenu)
		{
			return oldColor;
		}
		Vector3 color = _activeEngine.GetColor(x, y);
		color = Vector3.Min(Vector3.One, color);
		return new Color(color * ((Color)(ref oldColor)).ToVector3());
	}

	public static Color GetColor(int x, int y)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		if (Main.gameMenu)
		{
			return Color.White;
		}
		Color result = default(Color);
		Vector3 color = _activeEngine.GetColor(x, y);
		float num = GlobalBrightness * 255f;
		int num2 = (int)(color.X * num);
		int num3 = (int)(color.Y * num);
		int num4 = (int)(color.Z * num);
		if (num2 > 255)
		{
			num2 = 255;
		}
		if (num3 > 255)
		{
			num3 = 255;
		}
		if (num4 > 255)
		{
			num4 = 255;
		}
		num4 <<= 16;
		num3 <<= 8;
		((Color)(ref result)).PackedValue = (uint)(num2 | num3 | num4) | 0xFF000000u;
		return result;
	}

	public static void GetColor9Slice(int centerX, int centerY, ref Color[] slices)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		int num = 0;
		for (int i = centerX - 1; i <= centerX + 1; i++)
		{
			for (int j = centerY - 1; j <= centerY + 1; j++)
			{
				Vector3 color = _activeEngine.GetColor(i, j);
				int num2 = (int)(255f * color.X * GlobalBrightness);
				int num3 = (int)(255f * color.Y * GlobalBrightness);
				int num4 = (int)(255f * color.Z * GlobalBrightness);
				if (num2 > 255)
				{
					num2 = 255;
				}
				if (num3 > 255)
				{
					num3 = 255;
				}
				if (num4 > 255)
				{
					num4 = 255;
				}
				num4 <<= 16;
				num3 <<= 8;
				((Color)(ref slices[num])).PackedValue = (uint)(num2 | num3 | num4) | 0xFF000000u;
				num += 3;
			}
			num -= 8;
		}
	}

	public static void GetColor9Slice(int x, int y, ref Vector3[] slices)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Unknown result type (might be due to invalid IL or missing references)
		slices[0] = _activeEngine.GetColor(x - 1, y - 1) * GlobalBrightness;
		slices[3] = _activeEngine.GetColor(x - 1, y) * GlobalBrightness;
		slices[6] = _activeEngine.GetColor(x - 1, y + 1) * GlobalBrightness;
		slices[1] = _activeEngine.GetColor(x, y - 1) * GlobalBrightness;
		slices[4] = _activeEngine.GetColor(x, y) * GlobalBrightness;
		slices[7] = _activeEngine.GetColor(x, y + 1) * GlobalBrightness;
		slices[2] = _activeEngine.GetColor(x + 1, y - 1) * GlobalBrightness;
		slices[5] = _activeEngine.GetColor(x + 1, y) * GlobalBrightness;
		slices[8] = _activeEngine.GetColor(x + 1, y + 1) * GlobalBrightness;
	}

	public static void GetCornerColors(int centerX, int centerY, out VertexColors vertices, float scale = 1f)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_0170: Unknown result type (might be due to invalid IL or missing references)
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_021d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0223: Unknown result type (might be due to invalid IL or missing references)
		//IL_022b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0232: Unknown result type (might be due to invalid IL or missing references)
		//IL_023f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0245: Unknown result type (might be due to invalid IL or missing references)
		//IL_024d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0254: Unknown result type (might be due to invalid IL or missing references)
		//IL_0261: Unknown result type (might be due to invalid IL or missing references)
		//IL_0267: Unknown result type (might be due to invalid IL or missing references)
		//IL_026f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0276: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02df: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0301: Unknown result type (might be due to invalid IL or missing references)
		//IL_0309: Unknown result type (might be due to invalid IL or missing references)
		//IL_0310: Unknown result type (might be due to invalid IL or missing references)
		//IL_031d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0323: Unknown result type (might be due to invalid IL or missing references)
		//IL_032b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0332: Unknown result type (might be due to invalid IL or missing references)
		vertices = default(VertexColors);
		Vector3 color = _activeEngine.GetColor(centerX, centerY);
		Vector3 color9 = _activeEngine.GetColor(centerX, centerY - 1);
		Vector3 color2 = _activeEngine.GetColor(centerX, centerY + 1);
		Vector3 color3 = _activeEngine.GetColor(centerX - 1, centerY);
		Vector3 color4 = _activeEngine.GetColor(centerX + 1, centerY);
		Vector3 color5 = _activeEngine.GetColor(centerX - 1, centerY - 1);
		Vector3 color6 = _activeEngine.GetColor(centerX + 1, centerY - 1);
		Vector3 color7 = _activeEngine.GetColor(centerX - 1, centerY + 1);
		Vector3 color8 = _activeEngine.GetColor(centerX + 1, centerY + 1);
		float num = GlobalBrightness * scale * 63.75f;
		int num2 = (int)((color9.X + color5.X + color3.X + color.X) * num);
		int num3 = (int)((color9.Y + color5.Y + color3.Y + color.Y) * num);
		int num4 = (int)((color9.Z + color5.Z + color3.Z + color.Z) * num);
		if (num2 > 255)
		{
			num2 = 255;
		}
		if (num3 > 255)
		{
			num3 = 255;
		}
		if (num4 > 255)
		{
			num4 = 255;
		}
		num3 <<= 8;
		num4 <<= 16;
		((Color)(ref vertices.TopLeftColor)).PackedValue = (uint)(num2 | num3 | num4) | 0xFF000000u;
		num2 = (int)((color9.X + color6.X + color4.X + color.X) * num);
		num3 = (int)((color9.Y + color6.Y + color4.Y + color.Y) * num);
		num4 = (int)((color9.Z + color6.Z + color4.Z + color.Z) * num);
		if (num2 > 255)
		{
			num2 = 255;
		}
		if (num3 > 255)
		{
			num3 = 255;
		}
		if (num4 > 255)
		{
			num4 = 255;
		}
		num3 <<= 8;
		num4 <<= 16;
		((Color)(ref vertices.TopRightColor)).PackedValue = (uint)(num2 | num3 | num4) | 0xFF000000u;
		num2 = (int)((color2.X + color7.X + color3.X + color.X) * num);
		num3 = (int)((color2.Y + color7.Y + color3.Y + color.Y) * num);
		num4 = (int)((color2.Z + color7.Z + color3.Z + color.Z) * num);
		if (num2 > 255)
		{
			num2 = 255;
		}
		if (num3 > 255)
		{
			num3 = 255;
		}
		if (num4 > 255)
		{
			num4 = 255;
		}
		num3 <<= 8;
		num4 <<= 16;
		((Color)(ref vertices.BottomLeftColor)).PackedValue = (uint)(num2 | num3 | num4) | 0xFF000000u;
		num2 = (int)((color2.X + color8.X + color4.X + color.X) * num);
		num3 = (int)((color2.Y + color8.Y + color4.Y + color.Y) * num);
		num4 = (int)((color2.Z + color8.Z + color4.Z + color.Z) * num);
		if (num2 > 255)
		{
			num2 = 255;
		}
		if (num3 > 255)
		{
			num3 = 255;
		}
		if (num4 > 255)
		{
			num4 = 255;
		}
		num3 <<= 8;
		num4 <<= 16;
		((Color)(ref vertices.BottomRightColor)).PackedValue = (uint)(num2 | num3 | num4) | 0xFF000000u;
	}

	public static void GetColor4Slice(int centerX, int centerY, ref Color[] slices)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_0158: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_024e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0263: Unknown result type (might be due to invalid IL or missing references)
		//IL_0278: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_023a: Unknown result type (might be due to invalid IL or missing references)
		//IL_023f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0370: Unknown result type (might be due to invalid IL or missing references)
		//IL_0385: Unknown result type (might be due to invalid IL or missing references)
		//IL_039a: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_030b: Unknown result type (might be due to invalid IL or missing references)
		//IL_03eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_035c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0361: Unknown result type (might be due to invalid IL or missing references)
		//IL_048e: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0403: Unknown result type (might be due to invalid IL or missing references)
		//IL_0418: Unknown result type (might be due to invalid IL or missing references)
		//IL_042d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0509: Unknown result type (might be due to invalid IL or missing references)
		//IL_050e: Unknown result type (might be due to invalid IL or missing references)
		//IL_047e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0483: Unknown result type (might be due to invalid IL or missing references)
		Vector3 color = _activeEngine.GetColor(centerX, centerY - 1);
		Vector3 color2 = _activeEngine.GetColor(centerX, centerY + 1);
		Vector3 color3 = _activeEngine.GetColor(centerX - 1, centerY);
		Vector3 color4 = _activeEngine.GetColor(centerX + 1, centerY);
		float num37 = color.X + color.Y + color.Z;
		float num20 = color2.X + color2.Y + color2.Z;
		float num30 = color4.X + color4.Y + color4.Z;
		float num31 = color3.X + color3.Y + color3.Z;
		if (num37 >= num31)
		{
			int num32 = (int)(255f * color3.X * GlobalBrightness);
			int num33 = (int)(255f * color3.Y * GlobalBrightness);
			int num34 = (int)(255f * color3.Z * GlobalBrightness);
			if (num32 > 255)
			{
				num32 = 255;
			}
			if (num33 > 255)
			{
				num33 = 255;
			}
			if (num34 > 255)
			{
				num34 = 255;
			}
			slices[0] = new Color((int)(byte)num32, (int)(byte)num33, (int)(byte)num34, 255);
		}
		else
		{
			int num35 = (int)(255f * color.X * GlobalBrightness);
			int num36 = (int)(255f * color.Y * GlobalBrightness);
			int num10 = (int)(255f * color.Z * GlobalBrightness);
			if (num35 > 255)
			{
				num35 = 255;
			}
			if (num36 > 255)
			{
				num36 = 255;
			}
			if (num10 > 255)
			{
				num10 = 255;
			}
			slices[0] = new Color((int)(byte)num35, (int)(byte)num36, (int)(byte)num10, 255);
		}
		if (num37 >= num30)
		{
			int num11 = (int)(255f * color4.X * GlobalBrightness);
			int num12 = (int)(255f * color4.Y * GlobalBrightness);
			int num13 = (int)(255f * color4.Z * GlobalBrightness);
			if (num11 > 255)
			{
				num11 = 255;
			}
			if (num12 > 255)
			{
				num12 = 255;
			}
			if (num13 > 255)
			{
				num13 = 255;
			}
			slices[1] = new Color((int)(byte)num11, (int)(byte)num12, (int)(byte)num13, 255);
		}
		else
		{
			int num14 = (int)(255f * color.X * GlobalBrightness);
			int num15 = (int)(255f * color.Y * GlobalBrightness);
			int num16 = (int)(255f * color.Z * GlobalBrightness);
			if (num14 > 255)
			{
				num14 = 255;
			}
			if (num15 > 255)
			{
				num15 = 255;
			}
			if (num16 > 255)
			{
				num16 = 255;
			}
			slices[1] = new Color((int)(byte)num14, (int)(byte)num15, (int)(byte)num16, 255);
		}
		if (num20 >= num31)
		{
			int num17 = (int)(255f * color3.X * GlobalBrightness);
			int num18 = (int)(255f * color3.Y * GlobalBrightness);
			int num19 = (int)(255f * color3.Z * GlobalBrightness);
			if (num17 > 255)
			{
				num17 = 255;
			}
			if (num18 > 255)
			{
				num18 = 255;
			}
			if (num19 > 255)
			{
				num19 = 255;
			}
			slices[2] = new Color((int)(byte)num17, (int)(byte)num18, (int)(byte)num19, 255);
		}
		else
		{
			int num21 = (int)(255f * color2.X * GlobalBrightness);
			int num22 = (int)(255f * color2.Y * GlobalBrightness);
			int num23 = (int)(255f * color2.Z * GlobalBrightness);
			if (num21 > 255)
			{
				num21 = 255;
			}
			if (num22 > 255)
			{
				num22 = 255;
			}
			if (num23 > 255)
			{
				num23 = 255;
			}
			slices[2] = new Color((int)(byte)num21, (int)(byte)num22, (int)(byte)num23, 255);
		}
		if (num20 >= num30)
		{
			int num24 = (int)(255f * color4.X * GlobalBrightness);
			int num25 = (int)(255f * color4.Y * GlobalBrightness);
			int num26 = (int)(255f * color4.Z * GlobalBrightness);
			if (num24 > 255)
			{
				num24 = 255;
			}
			if (num25 > 255)
			{
				num25 = 255;
			}
			if (num26 > 255)
			{
				num26 = 255;
			}
			slices[3] = new Color((int)(byte)num24, (int)(byte)num25, (int)(byte)num26, 255);
		}
		else
		{
			int num27 = (int)(255f * color2.X * GlobalBrightness);
			int num28 = (int)(255f * color2.Y * GlobalBrightness);
			int num29 = (int)(255f * color2.Z * GlobalBrightness);
			if (num27 > 255)
			{
				num27 = 255;
			}
			if (num28 > 255)
			{
				num28 = 255;
			}
			if (num29 > 255)
			{
				num29 = 255;
			}
			slices[3] = new Color((int)(byte)num27, (int)(byte)num28, (int)(byte)num29, 255);
		}
	}

	public static void GetColor4Slice(int x, int y, ref Vector3[] slices)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		Vector3 color = _activeEngine.GetColor(x, y - 1);
		Vector3 color2 = _activeEngine.GetColor(x, y + 1);
		Vector3 color3 = _activeEngine.GetColor(x - 1, y);
		Vector3 color4 = _activeEngine.GetColor(x + 1, y);
		float num5 = color.X + color.Y + color.Z;
		float num2 = color2.X + color2.Y + color2.Z;
		float num3 = color4.X + color4.Y + color4.Z;
		float num4 = color3.X + color3.Y + color3.Z;
		if (num5 >= num4)
		{
			slices[0] = color3 * GlobalBrightness;
		}
		else
		{
			slices[0] = color * GlobalBrightness;
		}
		if (num5 >= num3)
		{
			slices[1] = color4 * GlobalBrightness;
		}
		else
		{
			slices[1] = color * GlobalBrightness;
		}
		if (num2 >= num4)
		{
			slices[2] = color3 * GlobalBrightness;
		}
		else
		{
			slices[2] = color2 * GlobalBrightness;
		}
		if (num2 >= num3)
		{
			slices[3] = color4 * GlobalBrightness;
		}
		else
		{
			slices[3] = color2 * GlobalBrightness;
		}
	}
}
