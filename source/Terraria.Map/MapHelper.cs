using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using Ionic.Zlib;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.Social;
using Terraria.Utilities;

namespace Terraria.Map;

public static class MapHelper
{
	private struct OldMapHelper
	{
		public byte misc;

		public byte misc2;

		public bool active()
		{
			if ((misc & 1) == 1)
			{
				return true;
			}
			return false;
		}

		public bool water()
		{
			if ((misc & 2) == 2)
			{
				return true;
			}
			return false;
		}

		public bool lava()
		{
			if ((misc & 4) == 4)
			{
				return true;
			}
			return false;
		}

		public bool honey()
		{
			if ((misc2 & 0x40) == 64)
			{
				return true;
			}
			return false;
		}

		public bool changed()
		{
			if ((misc & 8) == 8)
			{
				return true;
			}
			return false;
		}

		public bool wall()
		{
			if ((misc & 0x10) == 16)
			{
				return true;
			}
			return false;
		}

		public byte option()
		{
			byte b = 0;
			if ((misc & 0x20) == 32)
			{
				b++;
			}
			if ((misc & 0x40) == 64)
			{
				b += 2;
			}
			if ((misc & 0x80) == 128)
			{
				b += 4;
			}
			if ((misc2 & 1) == 1)
			{
				b += 8;
			}
			return b;
		}

		public byte color()
		{
			return (byte)((misc2 & 0x1E) >> 1);
		}
	}

	public const int drawLoopMilliseconds = 5;

	private const int HeaderEmpty = 0;

	private const int HeaderTile = 1;

	private const int HeaderWall = 2;

	private const int HeaderWater = 3;

	private const int HeaderLava = 4;

	private const int HeaderHoney = 5;

	private const int HeaderHeavenAndHell = 6;

	private const int HeaderBackground = 7;

	private const int Header2_ReadHeader3Bit = 1;

	private const int Header2Color1 = 2;

	private const int Header2Color2 = 4;

	private const int Header2Color3 = 8;

	private const int Header2Color4 = 16;

	private const int Header2Color5 = 32;

	private const int Header2ShimmerBit = 64;

	private const int Header2_UnusedBit8 = 128;

	private const int Header3_ReservedForHeader4Bit = 1;

	private const int Header3_UnusudBit2 = 2;

	private const int Header3_UnusudBit3 = 4;

	private const int Header3_UnusudBit4 = 8;

	private const int Header3_UnusudBit5 = 16;

	private const int Header3_UnusudBit6 = 32;

	private const int Header3_UnusudBit7 = 64;

	private const int Header3_UnusudBit8 = 128;

	private const int maxTileOptions = 12;

	private const int maxWallOptions = 2;

	private const int maxLiquidTypes = 4;

	private const int maxSkyGradients = 256;

	private const int maxDirtGradients = 256;

	private const int maxRockGradients = 256;

	public static int maxUpdateTile = 1000;

	public static int numUpdateTile = 0;

	public static short[] updateTileX = new short[maxUpdateTile];

	public static short[] updateTileY = new short[maxUpdateTile];

	private static object IOLock = new object();

	public static int[] tileOptionCounts;

	public static int[] wallOptionCounts;

	public static ushort[] tileLookup;

	public static ushort[] wallLookup;

	private static ushort tilePosition;

	private static ushort wallPosition;

	private static ushort liquidPosition;

	private static ushort skyPosition;

	private static ushort dirtPosition;

	private static ushort rockPosition;

	private static ushort hellPosition;

	internal static ushort modPosition;

	internal static Color[] colorLookup;

	private static ushort[] snowTypes;

	private static ushort wallRangeStart;

	private static ushort wallRangeEnd;

	public static bool noStatusText = false;

	public static void Initialize()
	{
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_017e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0183: Unknown result type (might be due to invalid IL or missing references)
		//IL_019f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0210: Unknown result type (might be due to invalid IL or missing references)
		//IL_0215: Unknown result type (might be due to invalid IL or missing references)
		//IL_022e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0233: Unknown result type (might be due to invalid IL or missing references)
		//IL_0249: Unknown result type (might be due to invalid IL or missing references)
		//IL_024e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0264: Unknown result type (might be due to invalid IL or missing references)
		//IL_0269: Unknown result type (might be due to invalid IL or missing references)
		//IL_0282: Unknown result type (might be due to invalid IL or missing references)
		//IL_0283: Unknown result type (might be due to invalid IL or missing references)
		//IL_0290: Unknown result type (might be due to invalid IL or missing references)
		//IL_0291: Unknown result type (might be due to invalid IL or missing references)
		//IL_029a: Unknown result type (might be due to invalid IL or missing references)
		//IL_029b: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_030a: Unknown result type (might be due to invalid IL or missing references)
		//IL_030b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0318: Unknown result type (might be due to invalid IL or missing references)
		//IL_0319: Unknown result type (might be due to invalid IL or missing references)
		//IL_0326: Unknown result type (might be due to invalid IL or missing references)
		//IL_0327: Unknown result type (might be due to invalid IL or missing references)
		//IL_0334: Unknown result type (might be due to invalid IL or missing references)
		//IL_0335: Unknown result type (might be due to invalid IL or missing references)
		//IL_0342: Unknown result type (might be due to invalid IL or missing references)
		//IL_0343: Unknown result type (might be due to invalid IL or missing references)
		//IL_0350: Unknown result type (might be due to invalid IL or missing references)
		//IL_0351: Unknown result type (might be due to invalid IL or missing references)
		//IL_035e: Unknown result type (might be due to invalid IL or missing references)
		//IL_035f: Unknown result type (might be due to invalid IL or missing references)
		//IL_036c: Unknown result type (might be due to invalid IL or missing references)
		//IL_036d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0383: Unknown result type (might be due to invalid IL or missing references)
		//IL_0388: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0408: Unknown result type (might be due to invalid IL or missing references)
		//IL_040d: Unknown result type (might be due to invalid IL or missing references)
		//IL_042c: Unknown result type (might be due to invalid IL or missing references)
		//IL_042d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0437: Unknown result type (might be due to invalid IL or missing references)
		//IL_0438: Unknown result type (might be due to invalid IL or missing references)
		//IL_0442: Unknown result type (might be due to invalid IL or missing references)
		//IL_0443: Unknown result type (might be due to invalid IL or missing references)
		//IL_0450: Unknown result type (might be due to invalid IL or missing references)
		//IL_0451: Unknown result type (might be due to invalid IL or missing references)
		//IL_045e: Unknown result type (might be due to invalid IL or missing references)
		//IL_045f: Unknown result type (might be due to invalid IL or missing references)
		//IL_046c: Unknown result type (might be due to invalid IL or missing references)
		//IL_046d: Unknown result type (might be due to invalid IL or missing references)
		//IL_047a: Unknown result type (might be due to invalid IL or missing references)
		//IL_047b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0488: Unknown result type (might be due to invalid IL or missing references)
		//IL_0489: Unknown result type (might be due to invalid IL or missing references)
		//IL_0496: Unknown result type (might be due to invalid IL or missing references)
		//IL_0497: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04df: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0502: Unknown result type (might be due to invalid IL or missing references)
		//IL_051b: Unknown result type (might be due to invalid IL or missing references)
		//IL_051c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0529: Unknown result type (might be due to invalid IL or missing references)
		//IL_052a: Unknown result type (might be due to invalid IL or missing references)
		//IL_053d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0542: Unknown result type (might be due to invalid IL or missing references)
		//IL_0555: Unknown result type (might be due to invalid IL or missing references)
		//IL_055a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0570: Unknown result type (might be due to invalid IL or missing references)
		//IL_0575: Unknown result type (might be due to invalid IL or missing references)
		//IL_0588: Unknown result type (might be due to invalid IL or missing references)
		//IL_058d: Unknown result type (might be due to invalid IL or missing references)
		//IL_059f: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_05bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_05be: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_060f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0610: Unknown result type (might be due to invalid IL or missing references)
		//IL_061a: Unknown result type (might be due to invalid IL or missing references)
		//IL_061b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0628: Unknown result type (might be due to invalid IL or missing references)
		//IL_0629: Unknown result type (might be due to invalid IL or missing references)
		//IL_0636: Unknown result type (might be due to invalid IL or missing references)
		//IL_0637: Unknown result type (might be due to invalid IL or missing references)
		//IL_0657: Unknown result type (might be due to invalid IL or missing references)
		//IL_0658: Unknown result type (might be due to invalid IL or missing references)
		//IL_0662: Unknown result type (might be due to invalid IL or missing references)
		//IL_0663: Unknown result type (might be due to invalid IL or missing references)
		//IL_0670: Unknown result type (might be due to invalid IL or missing references)
		//IL_0671: Unknown result type (might be due to invalid IL or missing references)
		//IL_067e: Unknown result type (might be due to invalid IL or missing references)
		//IL_067f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0699: Unknown result type (might be due to invalid IL or missing references)
		//IL_069a: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_06de: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_070c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0711: Unknown result type (might be due to invalid IL or missing references)
		//IL_0721: Unknown result type (might be due to invalid IL or missing references)
		//IL_0726: Unknown result type (might be due to invalid IL or missing references)
		//IL_0739: Unknown result type (might be due to invalid IL or missing references)
		//IL_073e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0751: Unknown result type (might be due to invalid IL or missing references)
		//IL_0756: Unknown result type (might be due to invalid IL or missing references)
		//IL_0766: Unknown result type (might be due to invalid IL or missing references)
		//IL_076b: Unknown result type (might be due to invalid IL or missing references)
		//IL_077e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0783: Unknown result type (might be due to invalid IL or missing references)
		//IL_0796: Unknown result type (might be due to invalid IL or missing references)
		//IL_079b: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_07db: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0808: Unknown result type (might be due to invalid IL or missing references)
		//IL_080d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0820: Unknown result type (might be due to invalid IL or missing references)
		//IL_0825: Unknown result type (might be due to invalid IL or missing references)
		//IL_083b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0840: Unknown result type (might be due to invalid IL or missing references)
		//IL_0860: Unknown result type (might be due to invalid IL or missing references)
		//IL_0861: Unknown result type (might be due to invalid IL or missing references)
		//IL_086e: Unknown result type (might be due to invalid IL or missing references)
		//IL_086f: Unknown result type (might be due to invalid IL or missing references)
		//IL_087c: Unknown result type (might be due to invalid IL or missing references)
		//IL_087d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0899: Unknown result type (might be due to invalid IL or missing references)
		//IL_089e: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_08cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_08f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_08fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0914: Unknown result type (might be due to invalid IL or missing references)
		//IL_0919: Unknown result type (might be due to invalid IL or missing references)
		//IL_0929: Unknown result type (might be due to invalid IL or missing references)
		//IL_092e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0948: Unknown result type (might be due to invalid IL or missing references)
		//IL_0949: Unknown result type (might be due to invalid IL or missing references)
		//IL_0953: Unknown result type (might be due to invalid IL or missing references)
		//IL_0954: Unknown result type (might be due to invalid IL or missing references)
		//IL_0961: Unknown result type (might be due to invalid IL or missing references)
		//IL_0962: Unknown result type (might be due to invalid IL or missing references)
		//IL_0979: Unknown result type (might be due to invalid IL or missing references)
		//IL_097a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0984: Unknown result type (might be due to invalid IL or missing references)
		//IL_0985: Unknown result type (might be due to invalid IL or missing references)
		//IL_099b: Unknown result type (might be due to invalid IL or missing references)
		//IL_09a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_09b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_09bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_09d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_09e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_09eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a01: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a06: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a1b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a20: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a35: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a3a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a50: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a55: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a68: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a6d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a80: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a85: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a98: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a9d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ab3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ab8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ace: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ad3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aec: Unknown result type (might be due to invalid IL or missing references)
		//IL_0af1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b04: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b09: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b29: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b2a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b34: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b35: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b42: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b43: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b5c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b61: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b78: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b79: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b86: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b87: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b94: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b95: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ba8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bad: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bc3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bc8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bdb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0be0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bfa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bfb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c05: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c06: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c13: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c14: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c31: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c32: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c3c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c3d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c4a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c4b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c65: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c66: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c73: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c74: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c8a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c8f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ca5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0caa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cc0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cc5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cdb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ce0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cf0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cf5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d15: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d16: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d20: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d21: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d3a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d3f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d4f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d54: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d64: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d69: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d79: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d7e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d91: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d96: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dac: Unknown result type (might be due to invalid IL or missing references)
		//IL_0db1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dc7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dcc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0def: Unknown result type (might be due to invalid IL or missing references)
		//IL_0df0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dfd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dfe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e1a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e1f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e38: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e3d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e59: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e5e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e7a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e7f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e92: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e97: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eb4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eb5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ec2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ec3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ed6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0edb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ef4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ef9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f15: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f1a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f36: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f3b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f57: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f5c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f78: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f7d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f90: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f95: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fab: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fb0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fcd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fce: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fdb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fdc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fef: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ff4: Unknown result type (might be due to invalid IL or missing references)
		//IL_1014: Unknown result type (might be due to invalid IL or missing references)
		//IL_1015: Unknown result type (might be due to invalid IL or missing references)
		//IL_1022: Unknown result type (might be due to invalid IL or missing references)
		//IL_1023: Unknown result type (might be due to invalid IL or missing references)
		//IL_1046: Unknown result type (might be due to invalid IL or missing references)
		//IL_1047: Unknown result type (might be due to invalid IL or missing references)
		//IL_1054: Unknown result type (might be due to invalid IL or missing references)
		//IL_1055: Unknown result type (might be due to invalid IL or missing references)
		//IL_106b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1070: Unknown result type (might be due to invalid IL or missing references)
		//IL_1086: Unknown result type (might be due to invalid IL or missing references)
		//IL_108b: Unknown result type (might be due to invalid IL or missing references)
		//IL_10a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_10a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_10bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_10c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_10d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_10dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_10f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_10f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_110b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1110: Unknown result type (might be due to invalid IL or missing references)
		//IL_1125: Unknown result type (might be due to invalid IL or missing references)
		//IL_112a: Unknown result type (might be due to invalid IL or missing references)
		//IL_113f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1144: Unknown result type (might be due to invalid IL or missing references)
		//IL_115c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1161: Unknown result type (might be due to invalid IL or missing references)
		//IL_1179: Unknown result type (might be due to invalid IL or missing references)
		//IL_117e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1193: Unknown result type (might be due to invalid IL or missing references)
		//IL_1198: Unknown result type (might be due to invalid IL or missing references)
		//IL_11ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_11b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_11cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_11d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_11e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_11ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_1204: Unknown result type (might be due to invalid IL or missing references)
		//IL_1209: Unknown result type (might be due to invalid IL or missing references)
		//IL_1222: Unknown result type (might be due to invalid IL or missing references)
		//IL_1227: Unknown result type (might be due to invalid IL or missing references)
		//IL_1240: Unknown result type (might be due to invalid IL or missing references)
		//IL_1245: Unknown result type (might be due to invalid IL or missing references)
		//IL_125b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1260: Unknown result type (might be due to invalid IL or missing references)
		//IL_1279: Unknown result type (might be due to invalid IL or missing references)
		//IL_127e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1297: Unknown result type (might be due to invalid IL or missing references)
		//IL_129c: Unknown result type (might be due to invalid IL or missing references)
		//IL_12b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_12b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_12d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_12d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_12ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_12f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_1309: Unknown result type (might be due to invalid IL or missing references)
		//IL_130e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1324: Unknown result type (might be due to invalid IL or missing references)
		//IL_1329: Unknown result type (might be due to invalid IL or missing references)
		//IL_133f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1344: Unknown result type (might be due to invalid IL or missing references)
		//IL_135a: Unknown result type (might be due to invalid IL or missing references)
		//IL_135f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1375: Unknown result type (might be due to invalid IL or missing references)
		//IL_137a: Unknown result type (might be due to invalid IL or missing references)
		//IL_138f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1394: Unknown result type (might be due to invalid IL or missing references)
		//IL_13a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_13ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_13c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_13cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_13e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_13e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_13fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_1403: Unknown result type (might be due to invalid IL or missing references)
		//IL_141f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1424: Unknown result type (might be due to invalid IL or missing references)
		//IL_1453: Unknown result type (might be due to invalid IL or missing references)
		//IL_1458: Unknown result type (might be due to invalid IL or missing references)
		//IL_1487: Unknown result type (might be due to invalid IL or missing references)
		//IL_148c: Unknown result type (might be due to invalid IL or missing references)
		//IL_14b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_14b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_14ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_14cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_14e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_14e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_14fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_14ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_1512: Unknown result type (might be due to invalid IL or missing references)
		//IL_1517: Unknown result type (might be due to invalid IL or missing references)
		//IL_152c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1531: Unknown result type (might be due to invalid IL or missing references)
		//IL_1544: Unknown result type (might be due to invalid IL or missing references)
		//IL_1549: Unknown result type (might be due to invalid IL or missing references)
		//IL_155e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1563: Unknown result type (might be due to invalid IL or missing references)
		//IL_157b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1580: Unknown result type (might be due to invalid IL or missing references)
		//IL_1597: Unknown result type (might be due to invalid IL or missing references)
		//IL_159c: Unknown result type (might be due to invalid IL or missing references)
		//IL_15b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_15b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_15d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_15d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_15ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_15f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_160d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1612: Unknown result type (might be due to invalid IL or missing references)
		//IL_1628: Unknown result type (might be due to invalid IL or missing references)
		//IL_162d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1649: Unknown result type (might be due to invalid IL or missing references)
		//IL_164e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1667: Unknown result type (might be due to invalid IL or missing references)
		//IL_166c: Unknown result type (might be due to invalid IL or missing references)
		//IL_167f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1684: Unknown result type (might be due to invalid IL or missing references)
		//IL_169a: Unknown result type (might be due to invalid IL or missing references)
		//IL_169f: Unknown result type (might be due to invalid IL or missing references)
		//IL_16b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_16ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_16d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_16d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_16f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_16f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_170f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1714: Unknown result type (might be due to invalid IL or missing references)
		//IL_172a: Unknown result type (might be due to invalid IL or missing references)
		//IL_172f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1742: Unknown result type (might be due to invalid IL or missing references)
		//IL_1747: Unknown result type (might be due to invalid IL or missing references)
		//IL_1760: Unknown result type (might be due to invalid IL or missing references)
		//IL_1765: Unknown result type (might be due to invalid IL or missing references)
		//IL_1778: Unknown result type (might be due to invalid IL or missing references)
		//IL_177d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1796: Unknown result type (might be due to invalid IL or missing references)
		//IL_179b: Unknown result type (might be due to invalid IL or missing references)
		//IL_17b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_17b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_17c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_17ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_17e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_17e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1802: Unknown result type (might be due to invalid IL or missing references)
		//IL_1807: Unknown result type (might be due to invalid IL or missing references)
		//IL_181a: Unknown result type (might be due to invalid IL or missing references)
		//IL_181f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1838: Unknown result type (might be due to invalid IL or missing references)
		//IL_183d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1853: Unknown result type (might be due to invalid IL or missing references)
		//IL_1858: Unknown result type (might be due to invalid IL or missing references)
		//IL_186d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1872: Unknown result type (might be due to invalid IL or missing references)
		//IL_188b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1890: Unknown result type (might be due to invalid IL or missing references)
		//IL_18ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_18b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_18c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_18cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_18df: Unknown result type (might be due to invalid IL or missing references)
		//IL_18e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_18fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_1902: Unknown result type (might be due to invalid IL or missing references)
		//IL_191b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1920: Unknown result type (might be due to invalid IL or missing references)
		//IL_1939: Unknown result type (might be due to invalid IL or missing references)
		//IL_193e: Unknown result type (might be due to invalid IL or missing references)
		//IL_195a: Unknown result type (might be due to invalid IL or missing references)
		//IL_195f: Unknown result type (might be due to invalid IL or missing references)
		//IL_197b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1980: Unknown result type (might be due to invalid IL or missing references)
		//IL_199c: Unknown result type (might be due to invalid IL or missing references)
		//IL_19a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_19b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_19bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_19d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_19da: Unknown result type (might be due to invalid IL or missing references)
		//IL_19ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_19f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a0b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a10: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a29: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a2e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a41: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a46: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a5f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a64: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a7a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a7f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a98: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a9d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ab3: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ab8: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ace: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ad3: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ae9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1aee: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b01: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b06: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b1c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b21: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b3a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b3f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b58: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b5d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b73: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b78: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b8e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b93: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ba9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bae: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bca: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bcf: Unknown result type (might be due to invalid IL or missing references)
		//IL_1be8: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bed: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c03: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c08: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c28: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c29: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c36: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c37: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c44: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c45: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c52: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c53: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c60: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c61: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c6e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c6f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c7c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c7d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c8a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c8b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c98: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c99: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ca6: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ca7: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cb4: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cb5: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cc2: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cc3: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cd0: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cd1: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cde: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cdf: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cec: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ced: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cfa: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cfb: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d08: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d09: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d16: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d17: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d24: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d25: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d32: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d33: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d40: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d41: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d4e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d4f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d5c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d5d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d6a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d6b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d78: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d79: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d86: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d87: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d94: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d95: Unknown result type (might be due to invalid IL or missing references)
		//IL_1da2: Unknown result type (might be due to invalid IL or missing references)
		//IL_1da3: Unknown result type (might be due to invalid IL or missing references)
		//IL_1db0: Unknown result type (might be due to invalid IL or missing references)
		//IL_1db1: Unknown result type (might be due to invalid IL or missing references)
		//IL_1dbe: Unknown result type (might be due to invalid IL or missing references)
		//IL_1dbf: Unknown result type (might be due to invalid IL or missing references)
		//IL_1dcc: Unknown result type (might be due to invalid IL or missing references)
		//IL_1dcd: Unknown result type (might be due to invalid IL or missing references)
		//IL_1dda: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ddb: Unknown result type (might be due to invalid IL or missing references)
		//IL_1de8: Unknown result type (might be due to invalid IL or missing references)
		//IL_1de9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1df6: Unknown result type (might be due to invalid IL or missing references)
		//IL_1df7: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e04: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e05: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e12: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e13: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e20: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e21: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e2e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e2f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e3c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e3d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e4a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e4b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e58: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e59: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e66: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e67: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e74: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e75: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e82: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e83: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e90: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e91: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e9e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e9f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1eac: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ead: Unknown result type (might be due to invalid IL or missing references)
		//IL_1eba: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ebb: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ec8: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ec9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ed6: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ed7: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ee4: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ee5: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ef2: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ef3: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f00: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f01: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f0e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f0f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f1c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f1d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f2a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f2b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f38: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f39: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f46: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f47: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f54: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f55: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f62: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f63: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f70: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f71: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f7e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f7f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f8c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f8d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f9a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f9b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fa8: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fa9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fb6: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fb7: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fc4: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fc5: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fd2: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fd3: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fe0: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fe1: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fee: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fef: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ffc: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ffd: Unknown result type (might be due to invalid IL or missing references)
		//IL_200a: Unknown result type (might be due to invalid IL or missing references)
		//IL_200b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2018: Unknown result type (might be due to invalid IL or missing references)
		//IL_2019: Unknown result type (might be due to invalid IL or missing references)
		//IL_2026: Unknown result type (might be due to invalid IL or missing references)
		//IL_2027: Unknown result type (might be due to invalid IL or missing references)
		//IL_2034: Unknown result type (might be due to invalid IL or missing references)
		//IL_2035: Unknown result type (might be due to invalid IL or missing references)
		//IL_2042: Unknown result type (might be due to invalid IL or missing references)
		//IL_2043: Unknown result type (might be due to invalid IL or missing references)
		//IL_2050: Unknown result type (might be due to invalid IL or missing references)
		//IL_2051: Unknown result type (might be due to invalid IL or missing references)
		//IL_205e: Unknown result type (might be due to invalid IL or missing references)
		//IL_205f: Unknown result type (might be due to invalid IL or missing references)
		//IL_206c: Unknown result type (might be due to invalid IL or missing references)
		//IL_206d: Unknown result type (might be due to invalid IL or missing references)
		//IL_207a: Unknown result type (might be due to invalid IL or missing references)
		//IL_207b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2088: Unknown result type (might be due to invalid IL or missing references)
		//IL_2089: Unknown result type (might be due to invalid IL or missing references)
		//IL_2096: Unknown result type (might be due to invalid IL or missing references)
		//IL_2097: Unknown result type (might be due to invalid IL or missing references)
		//IL_20a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_20a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_20b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_20b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_20c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_20c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_20d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_20dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_20f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_20fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_210c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2111: Unknown result type (might be due to invalid IL or missing references)
		//IL_2127: Unknown result type (might be due to invalid IL or missing references)
		//IL_212c: Unknown result type (might be due to invalid IL or missing references)
		//IL_213f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2144: Unknown result type (might be due to invalid IL or missing references)
		//IL_2157: Unknown result type (might be due to invalid IL or missing references)
		//IL_215c: Unknown result type (might be due to invalid IL or missing references)
		//IL_216f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2174: Unknown result type (might be due to invalid IL or missing references)
		//IL_218a: Unknown result type (might be due to invalid IL or missing references)
		//IL_218f: Unknown result type (might be due to invalid IL or missing references)
		//IL_21a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_21a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_21bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_21c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_21d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_21da: Unknown result type (might be due to invalid IL or missing references)
		//IL_21ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_21f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_2205: Unknown result type (might be due to invalid IL or missing references)
		//IL_220a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2226: Unknown result type (might be due to invalid IL or missing references)
		//IL_222b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2240: Unknown result type (might be due to invalid IL or missing references)
		//IL_2245: Unknown result type (might be due to invalid IL or missing references)
		//IL_225d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2262: Unknown result type (might be due to invalid IL or missing references)
		//IL_227b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2280: Unknown result type (might be due to invalid IL or missing references)
		//IL_2299: Unknown result type (might be due to invalid IL or missing references)
		//IL_229e: Unknown result type (might be due to invalid IL or missing references)
		//IL_22b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_22bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_22d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_22dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_22f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_22f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_230b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2310: Unknown result type (might be due to invalid IL or missing references)
		//IL_2329: Unknown result type (might be due to invalid IL or missing references)
		//IL_232e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2342: Unknown result type (might be due to invalid IL or missing references)
		//IL_2347: Unknown result type (might be due to invalid IL or missing references)
		//IL_235b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2360: Unknown result type (might be due to invalid IL or missing references)
		//IL_237c: Unknown result type (might be due to invalid IL or missing references)
		//IL_237d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2387: Unknown result type (might be due to invalid IL or missing references)
		//IL_2388: Unknown result type (might be due to invalid IL or missing references)
		//IL_2395: Unknown result type (might be due to invalid IL or missing references)
		//IL_2396: Unknown result type (might be due to invalid IL or missing references)
		//IL_23a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_23a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_23ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_23ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_23b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_23ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_23d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_23d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_23dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_23dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_23ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_23eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_23f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_23f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_2416: Unknown result type (might be due to invalid IL or missing references)
		//IL_2417: Unknown result type (might be due to invalid IL or missing references)
		//IL_2424: Unknown result type (might be due to invalid IL or missing references)
		//IL_2425: Unknown result type (might be due to invalid IL or missing references)
		//IL_2432: Unknown result type (might be due to invalid IL or missing references)
		//IL_2433: Unknown result type (might be due to invalid IL or missing references)
		//IL_2446: Unknown result type (might be due to invalid IL or missing references)
		//IL_244b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2458: Unknown result type (might be due to invalid IL or missing references)
		//IL_2459: Unknown result type (might be due to invalid IL or missing references)
		//IL_2466: Unknown result type (might be due to invalid IL or missing references)
		//IL_2467: Unknown result type (might be due to invalid IL or missing references)
		//IL_2471: Unknown result type (might be due to invalid IL or missing references)
		//IL_2472: Unknown result type (might be due to invalid IL or missing references)
		//IL_247c: Unknown result type (might be due to invalid IL or missing references)
		//IL_247d: Unknown result type (might be due to invalid IL or missing references)
		//IL_248a: Unknown result type (might be due to invalid IL or missing references)
		//IL_248b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2495: Unknown result type (might be due to invalid IL or missing references)
		//IL_2496: Unknown result type (might be due to invalid IL or missing references)
		//IL_24a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_24a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_24ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_24b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_24ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_24bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_24c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_24c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_24d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_24d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_24db: Unknown result type (might be due to invalid IL or missing references)
		//IL_24dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_24e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_24e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_24f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_24f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_24fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_24fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_250d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2512: Unknown result type (might be due to invalid IL or missing references)
		//IL_251c: Unknown result type (might be due to invalid IL or missing references)
		//IL_251d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2527: Unknown result type (might be due to invalid IL or missing references)
		//IL_2528: Unknown result type (might be due to invalid IL or missing references)
		//IL_2532: Unknown result type (might be due to invalid IL or missing references)
		//IL_2533: Unknown result type (might be due to invalid IL or missing references)
		//IL_253d: Unknown result type (might be due to invalid IL or missing references)
		//IL_253e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2548: Unknown result type (might be due to invalid IL or missing references)
		//IL_2549: Unknown result type (might be due to invalid IL or missing references)
		//IL_2556: Unknown result type (might be due to invalid IL or missing references)
		//IL_2557: Unknown result type (might be due to invalid IL or missing references)
		//IL_2564: Unknown result type (might be due to invalid IL or missing references)
		//IL_2565: Unknown result type (might be due to invalid IL or missing references)
		//IL_2572: Unknown result type (might be due to invalid IL or missing references)
		//IL_2573: Unknown result type (might be due to invalid IL or missing references)
		//IL_2580: Unknown result type (might be due to invalid IL or missing references)
		//IL_2581: Unknown result type (might be due to invalid IL or missing references)
		//IL_258e: Unknown result type (might be due to invalid IL or missing references)
		//IL_258f: Unknown result type (might be due to invalid IL or missing references)
		//IL_259c: Unknown result type (might be due to invalid IL or missing references)
		//IL_259d: Unknown result type (might be due to invalid IL or missing references)
		//IL_25aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_25ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_25b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_25b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_25c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_25c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_25d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_25d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_25e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_25e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_25f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_25f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_25fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_25ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_2612: Unknown result type (might be due to invalid IL or missing references)
		//IL_2617: Unknown result type (might be due to invalid IL or missing references)
		//IL_262d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2632: Unknown result type (might be due to invalid IL or missing references)
		//IL_2648: Unknown result type (might be due to invalid IL or missing references)
		//IL_264d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2666: Unknown result type (might be due to invalid IL or missing references)
		//IL_266b: Unknown result type (might be due to invalid IL or missing references)
		//IL_268b: Unknown result type (might be due to invalid IL or missing references)
		//IL_268c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2696: Unknown result type (might be due to invalid IL or missing references)
		//IL_2697: Unknown result type (might be due to invalid IL or missing references)
		//IL_26a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_26a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_26ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_26ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_26b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_26b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_26c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_26c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_26d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_26d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_26e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_26e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_26ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_26f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_26fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_26fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_270b: Unknown result type (might be due to invalid IL or missing references)
		//IL_270c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2719: Unknown result type (might be due to invalid IL or missing references)
		//IL_271a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2727: Unknown result type (might be due to invalid IL or missing references)
		//IL_2728: Unknown result type (might be due to invalid IL or missing references)
		//IL_2735: Unknown result type (might be due to invalid IL or missing references)
		//IL_2736: Unknown result type (might be due to invalid IL or missing references)
		//IL_2743: Unknown result type (might be due to invalid IL or missing references)
		//IL_2744: Unknown result type (might be due to invalid IL or missing references)
		//IL_2751: Unknown result type (might be due to invalid IL or missing references)
		//IL_2752: Unknown result type (might be due to invalid IL or missing references)
		//IL_275f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2760: Unknown result type (might be due to invalid IL or missing references)
		//IL_276d: Unknown result type (might be due to invalid IL or missing references)
		//IL_276e: Unknown result type (might be due to invalid IL or missing references)
		//IL_277b: Unknown result type (might be due to invalid IL or missing references)
		//IL_277c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2789: Unknown result type (might be due to invalid IL or missing references)
		//IL_278a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2797: Unknown result type (might be due to invalid IL or missing references)
		//IL_2798: Unknown result type (might be due to invalid IL or missing references)
		//IL_27a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_27a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_27b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_27b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_27c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_27c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_27cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_27d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_27dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_27de: Unknown result type (might be due to invalid IL or missing references)
		//IL_27eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_27ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_2808: Unknown result type (might be due to invalid IL or missing references)
		//IL_280d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2829: Unknown result type (might be due to invalid IL or missing references)
		//IL_282e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2847: Unknown result type (might be due to invalid IL or missing references)
		//IL_284c: Unknown result type (might be due to invalid IL or missing references)
		//IL_285f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2864: Unknown result type (might be due to invalid IL or missing references)
		//IL_287d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2882: Unknown result type (might be due to invalid IL or missing references)
		//IL_289e: Unknown result type (might be due to invalid IL or missing references)
		//IL_28a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_28b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_28bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_28d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_28de: Unknown result type (might be due to invalid IL or missing references)
		//IL_28f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_28f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_2909: Unknown result type (might be due to invalid IL or missing references)
		//IL_290e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2924: Unknown result type (might be due to invalid IL or missing references)
		//IL_2929: Unknown result type (might be due to invalid IL or missing references)
		//IL_293c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2941: Unknown result type (might be due to invalid IL or missing references)
		//IL_2954: Unknown result type (might be due to invalid IL or missing references)
		//IL_2959: Unknown result type (might be due to invalid IL or missing references)
		//IL_296f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2974: Unknown result type (might be due to invalid IL or missing references)
		//IL_298a: Unknown result type (might be due to invalid IL or missing references)
		//IL_298f: Unknown result type (might be due to invalid IL or missing references)
		//IL_29a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_29ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_29bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_29c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_29d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_29dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_29f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_29f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a0b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a10: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a23: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a28: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a41: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a46: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a6f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a74: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a8d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a92: Unknown result type (might be due to invalid IL or missing references)
		//IL_2aa8: Unknown result type (might be due to invalid IL or missing references)
		//IL_2aad: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ac0: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ac5: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ade: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ae3: Unknown result type (might be due to invalid IL or missing references)
		//IL_2af9: Unknown result type (might be due to invalid IL or missing references)
		//IL_2afe: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b11: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b16: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b2f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b34: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b4a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b4f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b62: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b67: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b80: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b85: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b95: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b9a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2bb0: Unknown result type (might be due to invalid IL or missing references)
		//IL_2bb5: Unknown result type (might be due to invalid IL or missing references)
		//IL_2bce: Unknown result type (might be due to invalid IL or missing references)
		//IL_2bd3: Unknown result type (might be due to invalid IL or missing references)
		//IL_2bec: Unknown result type (might be due to invalid IL or missing references)
		//IL_2bf1: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c0a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c0f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c28: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c2d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c40: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c45: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c5b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c60: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c79: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c7e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c91: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c96: Unknown result type (might be due to invalid IL or missing references)
		//IL_2cab: Unknown result type (might be due to invalid IL or missing references)
		//IL_2cb0: Unknown result type (might be due to invalid IL or missing references)
		//IL_2cc5: Unknown result type (might be due to invalid IL or missing references)
		//IL_2cca: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ce6: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ceb: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d04: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d09: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d22: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d27: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d3a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d3f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d5b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d60: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d73: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d78: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d8b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d90: Unknown result type (might be due to invalid IL or missing references)
		//IL_2da9: Unknown result type (might be due to invalid IL or missing references)
		//IL_2dae: Unknown result type (might be due to invalid IL or missing references)
		//IL_2dc4: Unknown result type (might be due to invalid IL or missing references)
		//IL_2dc9: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ddf: Unknown result type (might be due to invalid IL or missing references)
		//IL_2de4: Unknown result type (might be due to invalid IL or missing references)
		//IL_2df9: Unknown result type (might be due to invalid IL or missing references)
		//IL_2dfe: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e14: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e19: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e2f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e34: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e50: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e55: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e71: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e76: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e92: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e97: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ead: Unknown result type (might be due to invalid IL or missing references)
		//IL_2eb2: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ec8: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ecd: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ee6: Unknown result type (might be due to invalid IL or missing references)
		//IL_2eeb: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f07: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f0c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f25: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f2a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f43: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f48: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f5b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f60: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f73: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f78: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f91: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f96: Unknown result type (might be due to invalid IL or missing references)
		//IL_2fa9: Unknown result type (might be due to invalid IL or missing references)
		//IL_2fae: Unknown result type (might be due to invalid IL or missing references)
		//IL_2fca: Unknown result type (might be due to invalid IL or missing references)
		//IL_2fcf: Unknown result type (might be due to invalid IL or missing references)
		//IL_2fe2: Unknown result type (might be due to invalid IL or missing references)
		//IL_2fe7: Unknown result type (might be due to invalid IL or missing references)
		//IL_3003: Unknown result type (might be due to invalid IL or missing references)
		//IL_3008: Unknown result type (might be due to invalid IL or missing references)
		//IL_3022: Unknown result type (might be due to invalid IL or missing references)
		//IL_3023: Unknown result type (might be due to invalid IL or missing references)
		//IL_3030: Unknown result type (might be due to invalid IL or missing references)
		//IL_3031: Unknown result type (might be due to invalid IL or missing references)
		//IL_303e: Unknown result type (might be due to invalid IL or missing references)
		//IL_303f: Unknown result type (might be due to invalid IL or missing references)
		//IL_3058: Unknown result type (might be due to invalid IL or missing references)
		//IL_305d: Unknown result type (might be due to invalid IL or missing references)
		//IL_3079: Unknown result type (might be due to invalid IL or missing references)
		//IL_307e: Unknown result type (might be due to invalid IL or missing references)
		//IL_3097: Unknown result type (might be due to invalid IL or missing references)
		//IL_309c: Unknown result type (might be due to invalid IL or missing references)
		//IL_30b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_30ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_30d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_30db: Unknown result type (might be due to invalid IL or missing references)
		//IL_30f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_30fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_3118: Unknown result type (might be due to invalid IL or missing references)
		//IL_311d: Unknown result type (might be due to invalid IL or missing references)
		//IL_3139: Unknown result type (might be due to invalid IL or missing references)
		//IL_313e: Unknown result type (might be due to invalid IL or missing references)
		//IL_315a: Unknown result type (might be due to invalid IL or missing references)
		//IL_315f: Unknown result type (might be due to invalid IL or missing references)
		//IL_317b: Unknown result type (might be due to invalid IL or missing references)
		//IL_3180: Unknown result type (might be due to invalid IL or missing references)
		//IL_319c: Unknown result type (might be due to invalid IL or missing references)
		//IL_31a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_31bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_31c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_31db: Unknown result type (might be due to invalid IL or missing references)
		//IL_31e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_31fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_3201: Unknown result type (might be due to invalid IL or missing references)
		//IL_3214: Unknown result type (might be due to invalid IL or missing references)
		//IL_3219: Unknown result type (might be due to invalid IL or missing references)
		//IL_3235: Unknown result type (might be due to invalid IL or missing references)
		//IL_323a: Unknown result type (might be due to invalid IL or missing references)
		//IL_3256: Unknown result type (might be due to invalid IL or missing references)
		//IL_325b: Unknown result type (might be due to invalid IL or missing references)
		//IL_3277: Unknown result type (might be due to invalid IL or missing references)
		//IL_327c: Unknown result type (might be due to invalid IL or missing references)
		//IL_3292: Unknown result type (might be due to invalid IL or missing references)
		//IL_3297: Unknown result type (might be due to invalid IL or missing references)
		//IL_32ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_32b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_32cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_32d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_32e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_32ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_3304: Unknown result type (might be due to invalid IL or missing references)
		//IL_3309: Unknown result type (might be due to invalid IL or missing references)
		//IL_331b: Unknown result type (might be due to invalid IL or missing references)
		//IL_3320: Unknown result type (might be due to invalid IL or missing references)
		//IL_3332: Unknown result type (might be due to invalid IL or missing references)
		//IL_3337: Unknown result type (might be due to invalid IL or missing references)
		//IL_3349: Unknown result type (might be due to invalid IL or missing references)
		//IL_334e: Unknown result type (might be due to invalid IL or missing references)
		//IL_3360: Unknown result type (might be due to invalid IL or missing references)
		//IL_3365: Unknown result type (might be due to invalid IL or missing references)
		//IL_3377: Unknown result type (might be due to invalid IL or missing references)
		//IL_337c: Unknown result type (might be due to invalid IL or missing references)
		//IL_338f: Unknown result type (might be due to invalid IL or missing references)
		//IL_3394: Unknown result type (might be due to invalid IL or missing references)
		//IL_33aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_33af: Unknown result type (might be due to invalid IL or missing references)
		//IL_33cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_33cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_33d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_33d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_33e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_33e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_3400: Unknown result type (might be due to invalid IL or missing references)
		//IL_3401: Unknown result type (might be due to invalid IL or missing references)
		//IL_340b: Unknown result type (might be due to invalid IL or missing references)
		//IL_340c: Unknown result type (might be due to invalid IL or missing references)
		//IL_3416: Unknown result type (might be due to invalid IL or missing references)
		//IL_3417: Unknown result type (might be due to invalid IL or missing references)
		//IL_3434: Unknown result type (might be due to invalid IL or missing references)
		//IL_3435: Unknown result type (might be due to invalid IL or missing references)
		//IL_343f: Unknown result type (might be due to invalid IL or missing references)
		//IL_3440: Unknown result type (might be due to invalid IL or missing references)
		//IL_344a: Unknown result type (might be due to invalid IL or missing references)
		//IL_344b: Unknown result type (might be due to invalid IL or missing references)
		//IL_3468: Unknown result type (might be due to invalid IL or missing references)
		//IL_3469: Unknown result type (might be due to invalid IL or missing references)
		//IL_3473: Unknown result type (might be due to invalid IL or missing references)
		//IL_3474: Unknown result type (might be due to invalid IL or missing references)
		//IL_347e: Unknown result type (might be due to invalid IL or missing references)
		//IL_347f: Unknown result type (might be due to invalid IL or missing references)
		//IL_348c: Unknown result type (might be due to invalid IL or missing references)
		//IL_348d: Unknown result type (might be due to invalid IL or missing references)
		//IL_34a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_34a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_34b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_34b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_34bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_34be: Unknown result type (might be due to invalid IL or missing references)
		//IL_34d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_34d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_34e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_34e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_34ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_34ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_350c: Unknown result type (might be due to invalid IL or missing references)
		//IL_350d: Unknown result type (might be due to invalid IL or missing references)
		//IL_3517: Unknown result type (might be due to invalid IL or missing references)
		//IL_3518: Unknown result type (might be due to invalid IL or missing references)
		//IL_3522: Unknown result type (might be due to invalid IL or missing references)
		//IL_3523: Unknown result type (might be due to invalid IL or missing references)
		//IL_353c: Unknown result type (might be due to invalid IL or missing references)
		//IL_3541: Unknown result type (might be due to invalid IL or missing references)
		//IL_355a: Unknown result type (might be due to invalid IL or missing references)
		//IL_355f: Unknown result type (might be due to invalid IL or missing references)
		//IL_3575: Unknown result type (might be due to invalid IL or missing references)
		//IL_357a: Unknown result type (might be due to invalid IL or missing references)
		//IL_3593: Unknown result type (might be due to invalid IL or missing references)
		//IL_3598: Unknown result type (might be due to invalid IL or missing references)
		//IL_35ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_35b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_35cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_35d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_35e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_35ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_3602: Unknown result type (might be due to invalid IL or missing references)
		//IL_3607: Unknown result type (might be due to invalid IL or missing references)
		//IL_3620: Unknown result type (might be due to invalid IL or missing references)
		//IL_3625: Unknown result type (might be due to invalid IL or missing references)
		//IL_3638: Unknown result type (might be due to invalid IL or missing references)
		//IL_363d: Unknown result type (might be due to invalid IL or missing references)
		//IL_3659: Unknown result type (might be due to invalid IL or missing references)
		//IL_365e: Unknown result type (might be due to invalid IL or missing references)
		//IL_3674: Unknown result type (might be due to invalid IL or missing references)
		//IL_3679: Unknown result type (might be due to invalid IL or missing references)
		//IL_3692: Unknown result type (might be due to invalid IL or missing references)
		//IL_3697: Unknown result type (might be due to invalid IL or missing references)
		//IL_36b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_36b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_36ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_36d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_36ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_36f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_3707: Unknown result type (might be due to invalid IL or missing references)
		//IL_370c: Unknown result type (might be due to invalid IL or missing references)
		//IL_3728: Unknown result type (might be due to invalid IL or missing references)
		//IL_372d: Unknown result type (might be due to invalid IL or missing references)
		//IL_3746: Unknown result type (might be due to invalid IL or missing references)
		//IL_374b: Unknown result type (might be due to invalid IL or missing references)
		//IL_3765: Unknown result type (might be due to invalid IL or missing references)
		//IL_3766: Unknown result type (might be due to invalid IL or missing references)
		//IL_3773: Unknown result type (might be due to invalid IL or missing references)
		//IL_3774: Unknown result type (might be due to invalid IL or missing references)
		//IL_3781: Unknown result type (might be due to invalid IL or missing references)
		//IL_3782: Unknown result type (might be due to invalid IL or missing references)
		//IL_378f: Unknown result type (might be due to invalid IL or missing references)
		//IL_3790: Unknown result type (might be due to invalid IL or missing references)
		//IL_379d: Unknown result type (might be due to invalid IL or missing references)
		//IL_379e: Unknown result type (might be due to invalid IL or missing references)
		//IL_37b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_37b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_37c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_37c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_37d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_37d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_37e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_37e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_3803: Unknown result type (might be due to invalid IL or missing references)
		//IL_3804: Unknown result type (might be due to invalid IL or missing references)
		//IL_3811: Unknown result type (might be due to invalid IL or missing references)
		//IL_3812: Unknown result type (might be due to invalid IL or missing references)
		//IL_381f: Unknown result type (might be due to invalid IL or missing references)
		//IL_3820: Unknown result type (might be due to invalid IL or missing references)
		//IL_3843: Unknown result type (might be due to invalid IL or missing references)
		//IL_3844: Unknown result type (might be due to invalid IL or missing references)
		//IL_3851: Unknown result type (might be due to invalid IL or missing references)
		//IL_3852: Unknown result type (might be due to invalid IL or missing references)
		//IL_385f: Unknown result type (might be due to invalid IL or missing references)
		//IL_3860: Unknown result type (might be due to invalid IL or missing references)
		//IL_3883: Unknown result type (might be due to invalid IL or missing references)
		//IL_3884: Unknown result type (might be due to invalid IL or missing references)
		//IL_3891: Unknown result type (might be due to invalid IL or missing references)
		//IL_3892: Unknown result type (might be due to invalid IL or missing references)
		//IL_389f: Unknown result type (might be due to invalid IL or missing references)
		//IL_38a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_38bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_38be: Unknown result type (might be due to invalid IL or missing references)
		//IL_38cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_38cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_38d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_38da: Unknown result type (might be due to invalid IL or missing references)
		//IL_38f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_38f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_3902: Unknown result type (might be due to invalid IL or missing references)
		//IL_3903: Unknown result type (might be due to invalid IL or missing references)
		//IL_3910: Unknown result type (might be due to invalid IL or missing references)
		//IL_3911: Unknown result type (might be due to invalid IL or missing references)
		//IL_392e: Unknown result type (might be due to invalid IL or missing references)
		//IL_392f: Unknown result type (might be due to invalid IL or missing references)
		//IL_393c: Unknown result type (might be due to invalid IL or missing references)
		//IL_393d: Unknown result type (might be due to invalid IL or missing references)
		//IL_394a: Unknown result type (might be due to invalid IL or missing references)
		//IL_394b: Unknown result type (might be due to invalid IL or missing references)
		//IL_3968: Unknown result type (might be due to invalid IL or missing references)
		//IL_3969: Unknown result type (might be due to invalid IL or missing references)
		//IL_3976: Unknown result type (might be due to invalid IL or missing references)
		//IL_3977: Unknown result type (might be due to invalid IL or missing references)
		//IL_3984: Unknown result type (might be due to invalid IL or missing references)
		//IL_3985: Unknown result type (might be due to invalid IL or missing references)
		//IL_39a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_39a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_39af: Unknown result type (might be due to invalid IL or missing references)
		//IL_39b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_39be: Unknown result type (might be due to invalid IL or missing references)
		//IL_39bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_39dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_39de: Unknown result type (might be due to invalid IL or missing references)
		//IL_39ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_39ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_39fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_39fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_3a1a: Unknown result type (might be due to invalid IL or missing references)
		//IL_3a1f: Unknown result type (might be due to invalid IL or missing references)
		//IL_3a4a: Unknown result type (might be due to invalid IL or missing references)
		//IL_3a4f: Unknown result type (might be due to invalid IL or missing references)
		//IL_3a7a: Unknown result type (might be due to invalid IL or missing references)
		//IL_3a7f: Unknown result type (might be due to invalid IL or missing references)
		//IL_3aaa: Unknown result type (might be due to invalid IL or missing references)
		//IL_3aaf: Unknown result type (might be due to invalid IL or missing references)
		//IL_3ad5: Unknown result type (might be due to invalid IL or missing references)
		//IL_3ada: Unknown result type (might be due to invalid IL or missing references)
		//IL_3af0: Unknown result type (might be due to invalid IL or missing references)
		//IL_3af5: Unknown result type (might be due to invalid IL or missing references)
		//IL_3b0b: Unknown result type (might be due to invalid IL or missing references)
		//IL_3b10: Unknown result type (might be due to invalid IL or missing references)
		//IL_3b28: Unknown result type (might be due to invalid IL or missing references)
		//IL_3b2d: Unknown result type (might be due to invalid IL or missing references)
		//IL_3b43: Unknown result type (might be due to invalid IL or missing references)
		//IL_3b48: Unknown result type (might be due to invalid IL or missing references)
		//IL_3b5a: Unknown result type (might be due to invalid IL or missing references)
		//IL_3b5f: Unknown result type (might be due to invalid IL or missing references)
		//IL_3b78: Unknown result type (might be due to invalid IL or missing references)
		//IL_3b7d: Unknown result type (might be due to invalid IL or missing references)
		//IL_3b93: Unknown result type (might be due to invalid IL or missing references)
		//IL_3b98: Unknown result type (might be due to invalid IL or missing references)
		//IL_3bae: Unknown result type (might be due to invalid IL or missing references)
		//IL_3bb3: Unknown result type (might be due to invalid IL or missing references)
		//IL_3bcc: Unknown result type (might be due to invalid IL or missing references)
		//IL_3bd1: Unknown result type (might be due to invalid IL or missing references)
		//IL_3beb: Unknown result type (might be due to invalid IL or missing references)
		//IL_3bf0: Unknown result type (might be due to invalid IL or missing references)
		//IL_3c07: Unknown result type (might be due to invalid IL or missing references)
		//IL_3c0c: Unknown result type (might be due to invalid IL or missing references)
		//IL_3c22: Unknown result type (might be due to invalid IL or missing references)
		//IL_3c27: Unknown result type (might be due to invalid IL or missing references)
		//IL_3c3c: Unknown result type (might be due to invalid IL or missing references)
		//IL_3c41: Unknown result type (might be due to invalid IL or missing references)
		//IL_3c5a: Unknown result type (might be due to invalid IL or missing references)
		//IL_3c5f: Unknown result type (might be due to invalid IL or missing references)
		//IL_3c78: Unknown result type (might be due to invalid IL or missing references)
		//IL_3c7d: Unknown result type (might be due to invalid IL or missing references)
		//IL_3c99: Unknown result type (might be due to invalid IL or missing references)
		//IL_3c9e: Unknown result type (might be due to invalid IL or missing references)
		//IL_3cb4: Unknown result type (might be due to invalid IL or missing references)
		//IL_3cb9: Unknown result type (might be due to invalid IL or missing references)
		//IL_3cd5: Unknown result type (might be due to invalid IL or missing references)
		//IL_3cda: Unknown result type (might be due to invalid IL or missing references)
		//IL_3cf3: Unknown result type (might be due to invalid IL or missing references)
		//IL_3cf8: Unknown result type (might be due to invalid IL or missing references)
		//IL_3d0e: Unknown result type (might be due to invalid IL or missing references)
		//IL_3d13: Unknown result type (might be due to invalid IL or missing references)
		//IL_3d29: Unknown result type (might be due to invalid IL or missing references)
		//IL_3d2e: Unknown result type (might be due to invalid IL or missing references)
		//IL_3d43: Unknown result type (might be due to invalid IL or missing references)
		//IL_3d48: Unknown result type (might be due to invalid IL or missing references)
		//IL_3d58: Unknown result type (might be due to invalid IL or missing references)
		//IL_3d5d: Unknown result type (might be due to invalid IL or missing references)
		//IL_3d6d: Unknown result type (might be due to invalid IL or missing references)
		//IL_3d72: Unknown result type (might be due to invalid IL or missing references)
		//IL_3d88: Unknown result type (might be due to invalid IL or missing references)
		//IL_3d8d: Unknown result type (might be due to invalid IL or missing references)
		//IL_3da9: Unknown result type (might be due to invalid IL or missing references)
		//IL_3dae: Unknown result type (might be due to invalid IL or missing references)
		//IL_3dc7: Unknown result type (might be due to invalid IL or missing references)
		//IL_3dcc: Unknown result type (might be due to invalid IL or missing references)
		//IL_3de8: Unknown result type (might be due to invalid IL or missing references)
		//IL_3ded: Unknown result type (might be due to invalid IL or missing references)
		//IL_3e09: Unknown result type (might be due to invalid IL or missing references)
		//IL_3e0e: Unknown result type (might be due to invalid IL or missing references)
		//IL_3e23: Unknown result type (might be due to invalid IL or missing references)
		//IL_3e28: Unknown result type (might be due to invalid IL or missing references)
		//IL_3e44: Unknown result type (might be due to invalid IL or missing references)
		//IL_3e49: Unknown result type (might be due to invalid IL or missing references)
		//IL_3e62: Unknown result type (might be due to invalid IL or missing references)
		//IL_3e67: Unknown result type (might be due to invalid IL or missing references)
		//IL_3e7d: Unknown result type (might be due to invalid IL or missing references)
		//IL_3e82: Unknown result type (might be due to invalid IL or missing references)
		//IL_3e9b: Unknown result type (might be due to invalid IL or missing references)
		//IL_3ea0: Unknown result type (might be due to invalid IL or missing references)
		//IL_3eb5: Unknown result type (might be due to invalid IL or missing references)
		//IL_3eba: Unknown result type (might be due to invalid IL or missing references)
		//IL_3ed0: Unknown result type (might be due to invalid IL or missing references)
		//IL_3ed5: Unknown result type (might be due to invalid IL or missing references)
		//IL_3eee: Unknown result type (might be due to invalid IL or missing references)
		//IL_3ef3: Unknown result type (might be due to invalid IL or missing references)
		//IL_3f0c: Unknown result type (might be due to invalid IL or missing references)
		//IL_3f11: Unknown result type (might be due to invalid IL or missing references)
		//IL_3f2d: Unknown result type (might be due to invalid IL or missing references)
		//IL_3f32: Unknown result type (might be due to invalid IL or missing references)
		//IL_3f4b: Unknown result type (might be due to invalid IL or missing references)
		//IL_3f50: Unknown result type (might be due to invalid IL or missing references)
		//IL_3f69: Unknown result type (might be due to invalid IL or missing references)
		//IL_3f6e: Unknown result type (might be due to invalid IL or missing references)
		//IL_3f87: Unknown result type (might be due to invalid IL or missing references)
		//IL_3f8c: Unknown result type (might be due to invalid IL or missing references)
		//IL_3fa2: Unknown result type (might be due to invalid IL or missing references)
		//IL_3fa7: Unknown result type (might be due to invalid IL or missing references)
		//IL_3fbd: Unknown result type (might be due to invalid IL or missing references)
		//IL_3fc2: Unknown result type (might be due to invalid IL or missing references)
		//IL_3fd8: Unknown result type (might be due to invalid IL or missing references)
		//IL_3fdd: Unknown result type (might be due to invalid IL or missing references)
		//IL_3ff3: Unknown result type (might be due to invalid IL or missing references)
		//IL_3ff8: Unknown result type (might be due to invalid IL or missing references)
		//IL_400e: Unknown result type (might be due to invalid IL or missing references)
		//IL_4013: Unknown result type (might be due to invalid IL or missing references)
		//IL_4029: Unknown result type (might be due to invalid IL or missing references)
		//IL_402e: Unknown result type (might be due to invalid IL or missing references)
		//IL_404a: Unknown result type (might be due to invalid IL or missing references)
		//IL_404f: Unknown result type (might be due to invalid IL or missing references)
		//IL_4065: Unknown result type (might be due to invalid IL or missing references)
		//IL_406a: Unknown result type (might be due to invalid IL or missing references)
		//IL_4086: Unknown result type (might be due to invalid IL or missing references)
		//IL_408b: Unknown result type (might be due to invalid IL or missing references)
		//IL_40a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_40a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_40bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_40c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_40dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_40e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_40fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_4100: Unknown result type (might be due to invalid IL or missing references)
		//IL_411c: Unknown result type (might be due to invalid IL or missing references)
		//IL_4121: Unknown result type (might be due to invalid IL or missing references)
		//IL_413d: Unknown result type (might be due to invalid IL or missing references)
		//IL_4142: Unknown result type (might be due to invalid IL or missing references)
		//IL_415e: Unknown result type (might be due to invalid IL or missing references)
		//IL_4163: Unknown result type (might be due to invalid IL or missing references)
		//IL_417f: Unknown result type (might be due to invalid IL or missing references)
		//IL_4184: Unknown result type (might be due to invalid IL or missing references)
		//IL_41a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_41a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_41b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_41bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_41d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_41d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_41e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_41ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_4203: Unknown result type (might be due to invalid IL or missing references)
		//IL_4208: Unknown result type (might be due to invalid IL or missing references)
		//IL_421e: Unknown result type (might be due to invalid IL or missing references)
		//IL_4223: Unknown result type (might be due to invalid IL or missing references)
		//IL_423f: Unknown result type (might be due to invalid IL or missing references)
		//IL_4244: Unknown result type (might be due to invalid IL or missing references)
		//IL_425d: Unknown result type (might be due to invalid IL or missing references)
		//IL_4262: Unknown result type (might be due to invalid IL or missing references)
		//IL_427b: Unknown result type (might be due to invalid IL or missing references)
		//IL_4280: Unknown result type (might be due to invalid IL or missing references)
		//IL_4296: Unknown result type (might be due to invalid IL or missing references)
		//IL_429b: Unknown result type (might be due to invalid IL or missing references)
		//IL_42b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_42b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_42d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_42d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_42ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_42f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_430e: Unknown result type (might be due to invalid IL or missing references)
		//IL_4313: Unknown result type (might be due to invalid IL or missing references)
		//IL_432c: Unknown result type (might be due to invalid IL or missing references)
		//IL_4331: Unknown result type (might be due to invalid IL or missing references)
		//IL_4347: Unknown result type (might be due to invalid IL or missing references)
		//IL_434c: Unknown result type (might be due to invalid IL or missing references)
		//IL_435f: Unknown result type (might be due to invalid IL or missing references)
		//IL_4364: Unknown result type (might be due to invalid IL or missing references)
		//IL_4377: Unknown result type (might be due to invalid IL or missing references)
		//IL_437c: Unknown result type (might be due to invalid IL or missing references)
		//IL_438f: Unknown result type (might be due to invalid IL or missing references)
		//IL_4394: Unknown result type (might be due to invalid IL or missing references)
		//IL_43a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_43ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_43c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_43c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_43df: Unknown result type (might be due to invalid IL or missing references)
		//IL_43e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_43fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_43ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_4415: Unknown result type (might be due to invalid IL or missing references)
		//IL_441a: Unknown result type (might be due to invalid IL or missing references)
		//IL_442d: Unknown result type (might be due to invalid IL or missing references)
		//IL_4432: Unknown result type (might be due to invalid IL or missing references)
		//IL_4445: Unknown result type (might be due to invalid IL or missing references)
		//IL_444a: Unknown result type (might be due to invalid IL or missing references)
		//IL_445d: Unknown result type (might be due to invalid IL or missing references)
		//IL_4462: Unknown result type (might be due to invalid IL or missing references)
		//IL_4475: Unknown result type (might be due to invalid IL or missing references)
		//IL_447a: Unknown result type (might be due to invalid IL or missing references)
		//IL_4496: Unknown result type (might be due to invalid IL or missing references)
		//IL_449b: Unknown result type (might be due to invalid IL or missing references)
		//IL_44b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_44bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_44d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_44da: Unknown result type (might be due to invalid IL or missing references)
		//IL_44f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_44fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_450e: Unknown result type (might be due to invalid IL or missing references)
		//IL_4513: Unknown result type (might be due to invalid IL or missing references)
		//IL_452f: Unknown result type (might be due to invalid IL or missing references)
		//IL_4534: Unknown result type (might be due to invalid IL or missing references)
		//IL_454a: Unknown result type (might be due to invalid IL or missing references)
		//IL_454f: Unknown result type (might be due to invalid IL or missing references)
		//IL_456b: Unknown result type (might be due to invalid IL or missing references)
		//IL_4570: Unknown result type (might be due to invalid IL or missing references)
		//IL_4583: Unknown result type (might be due to invalid IL or missing references)
		//IL_4588: Unknown result type (might be due to invalid IL or missing references)
		//IL_459b: Unknown result type (might be due to invalid IL or missing references)
		//IL_45a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_45bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_45c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_45d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_45dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_45f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_45f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_460d: Unknown result type (might be due to invalid IL or missing references)
		//IL_4612: Unknown result type (might be due to invalid IL or missing references)
		//IL_462b: Unknown result type (might be due to invalid IL or missing references)
		//IL_4630: Unknown result type (might be due to invalid IL or missing references)
		//IL_4649: Unknown result type (might be due to invalid IL or missing references)
		//IL_464e: Unknown result type (might be due to invalid IL or missing references)
		//IL_466a: Unknown result type (might be due to invalid IL or missing references)
		//IL_466f: Unknown result type (might be due to invalid IL or missing references)
		//IL_4685: Unknown result type (might be due to invalid IL or missing references)
		//IL_468a: Unknown result type (might be due to invalid IL or missing references)
		//IL_46a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_46a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_46bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_46c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_46d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_46de: Unknown result type (might be due to invalid IL or missing references)
		//IL_46f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_46fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_4718: Unknown result type (might be due to invalid IL or missing references)
		//IL_471d: Unknown result type (might be due to invalid IL or missing references)
		//IL_4733: Unknown result type (might be due to invalid IL or missing references)
		//IL_4738: Unknown result type (might be due to invalid IL or missing references)
		//IL_474e: Unknown result type (might be due to invalid IL or missing references)
		//IL_4753: Unknown result type (might be due to invalid IL or missing references)
		//IL_4769: Unknown result type (might be due to invalid IL or missing references)
		//IL_476e: Unknown result type (might be due to invalid IL or missing references)
		//IL_4787: Unknown result type (might be due to invalid IL or missing references)
		//IL_478c: Unknown result type (might be due to invalid IL or missing references)
		//IL_47a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_47a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_47c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_47ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_47df: Unknown result type (might be due to invalid IL or missing references)
		//IL_47e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_47f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_47fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_4818: Unknown result type (might be due to invalid IL or missing references)
		//IL_481d: Unknown result type (might be due to invalid IL or missing references)
		//IL_4836: Unknown result type (might be due to invalid IL or missing references)
		//IL_483b: Unknown result type (might be due to invalid IL or missing references)
		//IL_4853: Unknown result type (might be due to invalid IL or missing references)
		//IL_4858: Unknown result type (might be due to invalid IL or missing references)
		//IL_4870: Unknown result type (might be due to invalid IL or missing references)
		//IL_4875: Unknown result type (might be due to invalid IL or missing references)
		//IL_488b: Unknown result type (might be due to invalid IL or missing references)
		//IL_4890: Unknown result type (might be due to invalid IL or missing references)
		//IL_48a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_48ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_48c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_48c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_48de: Unknown result type (might be due to invalid IL or missing references)
		//IL_48e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_48ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_4904: Unknown result type (might be due to invalid IL or missing references)
		//IL_491a: Unknown result type (might be due to invalid IL or missing references)
		//IL_491f: Unknown result type (might be due to invalid IL or missing references)
		//IL_4935: Unknown result type (might be due to invalid IL or missing references)
		//IL_493a: Unknown result type (might be due to invalid IL or missing references)
		//IL_4950: Unknown result type (might be due to invalid IL or missing references)
		//IL_4955: Unknown result type (might be due to invalid IL or missing references)
		//IL_4969: Unknown result type (might be due to invalid IL or missing references)
		//IL_496e: Unknown result type (might be due to invalid IL or missing references)
		//IL_4982: Unknown result type (might be due to invalid IL or missing references)
		//IL_4987: Unknown result type (might be due to invalid IL or missing references)
		//IL_499b: Unknown result type (might be due to invalid IL or missing references)
		//IL_49a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_49bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_49c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_49da: Unknown result type (might be due to invalid IL or missing references)
		//IL_49df: Unknown result type (might be due to invalid IL or missing references)
		//IL_49f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_49f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_4a0a: Unknown result type (might be due to invalid IL or missing references)
		//IL_4a0f: Unknown result type (might be due to invalid IL or missing references)
		//IL_4a28: Unknown result type (might be due to invalid IL or missing references)
		//IL_4a2d: Unknown result type (might be due to invalid IL or missing references)
		//IL_4a46: Unknown result type (might be due to invalid IL or missing references)
		//IL_4a4b: Unknown result type (might be due to invalid IL or missing references)
		//IL_4a64: Unknown result type (might be due to invalid IL or missing references)
		//IL_4a69: Unknown result type (might be due to invalid IL or missing references)
		//IL_4a85: Unknown result type (might be due to invalid IL or missing references)
		//IL_4a8a: Unknown result type (might be due to invalid IL or missing references)
		//IL_4aa2: Unknown result type (might be due to invalid IL or missing references)
		//IL_4aa7: Unknown result type (might be due to invalid IL or missing references)
		//IL_4ac0: Unknown result type (might be due to invalid IL or missing references)
		//IL_4ac5: Unknown result type (might be due to invalid IL or missing references)
		//IL_4ade: Unknown result type (might be due to invalid IL or missing references)
		//IL_4ae3: Unknown result type (might be due to invalid IL or missing references)
		//IL_4aff: Unknown result type (might be due to invalid IL or missing references)
		//IL_4b04: Unknown result type (might be due to invalid IL or missing references)
		//IL_4b1c: Unknown result type (might be due to invalid IL or missing references)
		//IL_4b21: Unknown result type (might be due to invalid IL or missing references)
		//IL_4b3a: Unknown result type (might be due to invalid IL or missing references)
		//IL_4b3f: Unknown result type (might be due to invalid IL or missing references)
		//IL_4b55: Unknown result type (might be due to invalid IL or missing references)
		//IL_4b5a: Unknown result type (might be due to invalid IL or missing references)
		//IL_4b73: Unknown result type (might be due to invalid IL or missing references)
		//IL_4b78: Unknown result type (might be due to invalid IL or missing references)
		//IL_4b94: Unknown result type (might be due to invalid IL or missing references)
		//IL_4b99: Unknown result type (might be due to invalid IL or missing references)
		//IL_4bb5: Unknown result type (might be due to invalid IL or missing references)
		//IL_4bba: Unknown result type (might be due to invalid IL or missing references)
		//IL_4bd2: Unknown result type (might be due to invalid IL or missing references)
		//IL_4bd7: Unknown result type (might be due to invalid IL or missing references)
		//IL_4bf0: Unknown result type (might be due to invalid IL or missing references)
		//IL_4bf5: Unknown result type (might be due to invalid IL or missing references)
		//IL_4c0e: Unknown result type (might be due to invalid IL or missing references)
		//IL_4c13: Unknown result type (might be due to invalid IL or missing references)
		//IL_4c29: Unknown result type (might be due to invalid IL or missing references)
		//IL_4c2e: Unknown result type (might be due to invalid IL or missing references)
		//IL_4c4a: Unknown result type (might be due to invalid IL or missing references)
		//IL_4c4f: Unknown result type (might be due to invalid IL or missing references)
		//IL_4c6b: Unknown result type (might be due to invalid IL or missing references)
		//IL_4c70: Unknown result type (might be due to invalid IL or missing references)
		//IL_4c8c: Unknown result type (might be due to invalid IL or missing references)
		//IL_4c91: Unknown result type (might be due to invalid IL or missing references)
		//IL_4caa: Unknown result type (might be due to invalid IL or missing references)
		//IL_4caf: Unknown result type (might be due to invalid IL or missing references)
		//IL_4ccb: Unknown result type (might be due to invalid IL or missing references)
		//IL_4cd0: Unknown result type (might be due to invalid IL or missing references)
		//IL_4cec: Unknown result type (might be due to invalid IL or missing references)
		//IL_4cf1: Unknown result type (might be due to invalid IL or missing references)
		//IL_4d0a: Unknown result type (might be due to invalid IL or missing references)
		//IL_4d0f: Unknown result type (might be due to invalid IL or missing references)
		//IL_4d2b: Unknown result type (might be due to invalid IL or missing references)
		//IL_4d30: Unknown result type (might be due to invalid IL or missing references)
		//IL_4d4c: Unknown result type (might be due to invalid IL or missing references)
		//IL_4d51: Unknown result type (might be due to invalid IL or missing references)
		//IL_4d6a: Unknown result type (might be due to invalid IL or missing references)
		//IL_4d6f: Unknown result type (might be due to invalid IL or missing references)
		//IL_4d85: Unknown result type (might be due to invalid IL or missing references)
		//IL_4d8a: Unknown result type (might be due to invalid IL or missing references)
		//IL_4da3: Unknown result type (might be due to invalid IL or missing references)
		//IL_4da8: Unknown result type (might be due to invalid IL or missing references)
		//IL_4dbe: Unknown result type (might be due to invalid IL or missing references)
		//IL_4dc3: Unknown result type (might be due to invalid IL or missing references)
		//IL_4dd6: Unknown result type (might be due to invalid IL or missing references)
		//IL_4ddb: Unknown result type (might be due to invalid IL or missing references)
		//IL_4df4: Unknown result type (might be due to invalid IL or missing references)
		//IL_4df9: Unknown result type (might be due to invalid IL or missing references)
		//IL_4e12: Unknown result type (might be due to invalid IL or missing references)
		//IL_4e17: Unknown result type (might be due to invalid IL or missing references)
		//IL_4e30: Unknown result type (might be due to invalid IL or missing references)
		//IL_4e35: Unknown result type (might be due to invalid IL or missing references)
		//IL_4e51: Unknown result type (might be due to invalid IL or missing references)
		//IL_4e56: Unknown result type (might be due to invalid IL or missing references)
		//IL_4e6f: Unknown result type (might be due to invalid IL or missing references)
		//IL_4e74: Unknown result type (might be due to invalid IL or missing references)
		//IL_4e91: Unknown result type (might be due to invalid IL or missing references)
		//IL_4e96: Unknown result type (might be due to invalid IL or missing references)
		//IL_4eb0: Unknown result type (might be due to invalid IL or missing references)
		//IL_4eb5: Unknown result type (might be due to invalid IL or missing references)
		//IL_4ecf: Unknown result type (might be due to invalid IL or missing references)
		//IL_4ed4: Unknown result type (might be due to invalid IL or missing references)
		//IL_4ef0: Unknown result type (might be due to invalid IL or missing references)
		//IL_4ef5: Unknown result type (might be due to invalid IL or missing references)
		//IL_4f1d: Unknown result type (might be due to invalid IL or missing references)
		//IL_4f22: Unknown result type (might be due to invalid IL or missing references)
		//IL_4f35: Unknown result type (might be due to invalid IL or missing references)
		//IL_4f3a: Unknown result type (might be due to invalid IL or missing references)
		//IL_4f4d: Unknown result type (might be due to invalid IL or missing references)
		//IL_4f52: Unknown result type (might be due to invalid IL or missing references)
		//IL_4f65: Unknown result type (might be due to invalid IL or missing references)
		//IL_4f6a: Unknown result type (might be due to invalid IL or missing references)
		//IL_4f80: Unknown result type (might be due to invalid IL or missing references)
		//IL_4f85: Unknown result type (might be due to invalid IL or missing references)
		//IL_4f9e: Unknown result type (might be due to invalid IL or missing references)
		//IL_4fa3: Unknown result type (might be due to invalid IL or missing references)
		//IL_4fbc: Unknown result type (might be due to invalid IL or missing references)
		//IL_4fc1: Unknown result type (might be due to invalid IL or missing references)
		//IL_4fda: Unknown result type (might be due to invalid IL or missing references)
		//IL_4fdf: Unknown result type (might be due to invalid IL or missing references)
		//IL_4ffb: Unknown result type (might be due to invalid IL or missing references)
		//IL_5000: Unknown result type (might be due to invalid IL or missing references)
		//IL_5016: Unknown result type (might be due to invalid IL or missing references)
		//IL_501b: Unknown result type (might be due to invalid IL or missing references)
		//IL_5031: Unknown result type (might be due to invalid IL or missing references)
		//IL_5036: Unknown result type (might be due to invalid IL or missing references)
		//IL_504f: Unknown result type (might be due to invalid IL or missing references)
		//IL_5054: Unknown result type (might be due to invalid IL or missing references)
		//IL_506d: Unknown result type (might be due to invalid IL or missing references)
		//IL_5072: Unknown result type (might be due to invalid IL or missing references)
		//IL_508b: Unknown result type (might be due to invalid IL or missing references)
		//IL_5090: Unknown result type (might be due to invalid IL or missing references)
		//IL_50ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_50b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_50cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_50d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_50ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_50f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_510f: Unknown result type (might be due to invalid IL or missing references)
		//IL_5114: Unknown result type (might be due to invalid IL or missing references)
		//IL_5130: Unknown result type (might be due to invalid IL or missing references)
		//IL_5135: Unknown result type (might be due to invalid IL or missing references)
		//IL_514e: Unknown result type (might be due to invalid IL or missing references)
		//IL_5153: Unknown result type (might be due to invalid IL or missing references)
		//IL_516c: Unknown result type (might be due to invalid IL or missing references)
		//IL_5171: Unknown result type (might be due to invalid IL or missing references)
		//IL_518d: Unknown result type (might be due to invalid IL or missing references)
		//IL_5192: Unknown result type (might be due to invalid IL or missing references)
		//IL_51ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_51b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_51c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_51c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_51e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_51e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_51fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_5201: Unknown result type (might be due to invalid IL or missing references)
		//IL_521d: Unknown result type (might be due to invalid IL or missing references)
		//IL_5222: Unknown result type (might be due to invalid IL or missing references)
		//IL_523b: Unknown result type (might be due to invalid IL or missing references)
		//IL_5240: Unknown result type (might be due to invalid IL or missing references)
		//IL_5255: Unknown result type (might be due to invalid IL or missing references)
		//IL_525a: Unknown result type (might be due to invalid IL or missing references)
		//IL_5273: Unknown result type (might be due to invalid IL or missing references)
		//IL_5278: Unknown result type (might be due to invalid IL or missing references)
		//IL_528e: Unknown result type (might be due to invalid IL or missing references)
		//IL_5293: Unknown result type (might be due to invalid IL or missing references)
		//IL_52a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_52ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_52c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_52c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_52d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_52de: Unknown result type (might be due to invalid IL or missing references)
		//IL_52f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_52f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_5309: Unknown result type (might be due to invalid IL or missing references)
		//IL_530e: Unknown result type (might be due to invalid IL or missing references)
		//IL_5327: Unknown result type (might be due to invalid IL or missing references)
		//IL_532c: Unknown result type (might be due to invalid IL or missing references)
		//IL_5345: Unknown result type (might be due to invalid IL or missing references)
		//IL_534a: Unknown result type (might be due to invalid IL or missing references)
		//IL_5363: Unknown result type (might be due to invalid IL or missing references)
		//IL_5368: Unknown result type (might be due to invalid IL or missing references)
		//IL_5384: Unknown result type (might be due to invalid IL or missing references)
		//IL_5389: Unknown result type (might be due to invalid IL or missing references)
		//IL_53a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_53aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_53c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_53cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_53e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_53e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_53ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_5404: Unknown result type (might be due to invalid IL or missing references)
		//IL_541d: Unknown result type (might be due to invalid IL or missing references)
		//IL_5422: Unknown result type (might be due to invalid IL or missing references)
		//IL_543e: Unknown result type (might be due to invalid IL or missing references)
		//IL_5443: Unknown result type (might be due to invalid IL or missing references)
		//IL_545c: Unknown result type (might be due to invalid IL or missing references)
		//IL_5461: Unknown result type (might be due to invalid IL or missing references)
		//IL_547a: Unknown result type (might be due to invalid IL or missing references)
		//IL_547f: Unknown result type (might be due to invalid IL or missing references)
		//IL_549b: Unknown result type (might be due to invalid IL or missing references)
		//IL_54a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_54b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_54bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_54d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_54dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_54f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_54fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_5517: Unknown result type (might be due to invalid IL or missing references)
		//IL_5519: Unknown result type (might be due to invalid IL or missing references)
		//IL_5526: Unknown result type (might be due to invalid IL or missing references)
		//IL_5528: Unknown result type (might be due to invalid IL or missing references)
		//IL_5535: Unknown result type (might be due to invalid IL or missing references)
		//IL_5537: Unknown result type (might be due to invalid IL or missing references)
		//IL_5553: Unknown result type (might be due to invalid IL or missing references)
		//IL_5558: Unknown result type (might be due to invalid IL or missing references)
		//IL_5574: Unknown result type (might be due to invalid IL or missing references)
		//IL_5579: Unknown result type (might be due to invalid IL or missing references)
		//IL_558f: Unknown result type (might be due to invalid IL or missing references)
		//IL_5594: Unknown result type (might be due to invalid IL or missing references)
		//IL_55a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_55ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_55c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_55cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_55e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_55e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_55f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_55fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_560f: Unknown result type (might be due to invalid IL or missing references)
		//IL_5614: Unknown result type (might be due to invalid IL or missing references)
		//IL_5627: Unknown result type (might be due to invalid IL or missing references)
		//IL_562c: Unknown result type (might be due to invalid IL or missing references)
		//IL_5670: Unknown result type (might be due to invalid IL or missing references)
		//IL_5675: Unknown result type (might be due to invalid IL or missing references)
		//IL_568f: Unknown result type (might be due to invalid IL or missing references)
		//IL_5694: Unknown result type (might be due to invalid IL or missing references)
		//IL_56ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_56b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_56c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_56cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_56e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_56e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_56fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_5701: Unknown result type (might be due to invalid IL or missing references)
		//IL_5718: Unknown result type (might be due to invalid IL or missing references)
		//IL_571d: Unknown result type (might be due to invalid IL or missing references)
		//IL_5737: Unknown result type (might be due to invalid IL or missing references)
		//IL_573c: Unknown result type (might be due to invalid IL or missing references)
		//IL_5756: Unknown result type (might be due to invalid IL or missing references)
		//IL_575b: Unknown result type (might be due to invalid IL or missing references)
		//IL_5772: Unknown result type (might be due to invalid IL or missing references)
		//IL_5777: Unknown result type (might be due to invalid IL or missing references)
		//IL_578e: Unknown result type (might be due to invalid IL or missing references)
		//IL_5793: Unknown result type (might be due to invalid IL or missing references)
		//IL_57aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_57af: Unknown result type (might be due to invalid IL or missing references)
		//IL_57cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_57d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_57eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_57f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_5801: Unknown result type (might be due to invalid IL or missing references)
		//IL_5806: Unknown result type (might be due to invalid IL or missing references)
		//IL_581a: Unknown result type (might be due to invalid IL or missing references)
		//IL_581f: Unknown result type (might be due to invalid IL or missing references)
		//IL_5833: Unknown result type (might be due to invalid IL or missing references)
		//IL_5838: Unknown result type (might be due to invalid IL or missing references)
		//IL_584f: Unknown result type (might be due to invalid IL or missing references)
		//IL_5854: Unknown result type (might be due to invalid IL or missing references)
		//IL_5867: Unknown result type (might be due to invalid IL or missing references)
		//IL_586c: Unknown result type (might be due to invalid IL or missing references)
		//IL_5880: Unknown result type (might be due to invalid IL or missing references)
		//IL_5885: Unknown result type (might be due to invalid IL or missing references)
		//IL_5898: Unknown result type (might be due to invalid IL or missing references)
		//IL_589d: Unknown result type (might be due to invalid IL or missing references)
		//IL_58b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_58b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_58ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_58cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_58ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_58f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_5905: Unknown result type (might be due to invalid IL or missing references)
		//IL_590a: Unknown result type (might be due to invalid IL or missing references)
		//IL_5924: Unknown result type (might be due to invalid IL or missing references)
		//IL_5929: Unknown result type (might be due to invalid IL or missing references)
		//IL_5943: Unknown result type (might be due to invalid IL or missing references)
		//IL_5948: Unknown result type (might be due to invalid IL or missing references)
		//IL_595c: Unknown result type (might be due to invalid IL or missing references)
		//IL_5961: Unknown result type (might be due to invalid IL or missing references)
		//IL_5975: Unknown result type (might be due to invalid IL or missing references)
		//IL_597a: Unknown result type (might be due to invalid IL or missing references)
		//IL_5991: Unknown result type (might be due to invalid IL or missing references)
		//IL_5992: Unknown result type (might be due to invalid IL or missing references)
		//IL_599d: Unknown result type (might be due to invalid IL or missing references)
		//IL_599e: Unknown result type (might be due to invalid IL or missing references)
		//IL_59a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_59aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_59b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_59b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_59c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_59c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_59cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_59ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_59d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_59da: Unknown result type (might be due to invalid IL or missing references)
		//IL_59e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_59e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_59f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_59f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_59ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_5a00: Unknown result type (might be due to invalid IL or missing references)
		//IL_5a17: Unknown result type (might be due to invalid IL or missing references)
		//IL_5a18: Unknown result type (might be due to invalid IL or missing references)
		//IL_5a23: Unknown result type (might be due to invalid IL or missing references)
		//IL_5a24: Unknown result type (might be due to invalid IL or missing references)
		//IL_5a2f: Unknown result type (might be due to invalid IL or missing references)
		//IL_5a30: Unknown result type (might be due to invalid IL or missing references)
		//IL_5a40: Unknown result type (might be due to invalid IL or missing references)
		//IL_5a45: Unknown result type (might be due to invalid IL or missing references)
		//IL_5a55: Unknown result type (might be due to invalid IL or missing references)
		//IL_5a5a: Unknown result type (might be due to invalid IL or missing references)
		//IL_5a6a: Unknown result type (might be due to invalid IL or missing references)
		//IL_5a6f: Unknown result type (might be due to invalid IL or missing references)
		//IL_5a86: Unknown result type (might be due to invalid IL or missing references)
		//IL_5a87: Unknown result type (might be due to invalid IL or missing references)
		//IL_5a92: Unknown result type (might be due to invalid IL or missing references)
		//IL_5a93: Unknown result type (might be due to invalid IL or missing references)
		//IL_5aa1: Unknown result type (might be due to invalid IL or missing references)
		//IL_5aa2: Unknown result type (might be due to invalid IL or missing references)
		//IL_5aba: Unknown result type (might be due to invalid IL or missing references)
		//IL_5abb: Unknown result type (might be due to invalid IL or missing references)
		//IL_5ac6: Unknown result type (might be due to invalid IL or missing references)
		//IL_5ac7: Unknown result type (might be due to invalid IL or missing references)
		//IL_5adf: Unknown result type (might be due to invalid IL or missing references)
		//IL_5ae0: Unknown result type (might be due to invalid IL or missing references)
		//IL_5aeb: Unknown result type (might be due to invalid IL or missing references)
		//IL_5aec: Unknown result type (might be due to invalid IL or missing references)
		//IL_5b03: Unknown result type (might be due to invalid IL or missing references)
		//IL_5b04: Unknown result type (might be due to invalid IL or missing references)
		//IL_5b0f: Unknown result type (might be due to invalid IL or missing references)
		//IL_5b10: Unknown result type (might be due to invalid IL or missing references)
		//IL_5b1e: Unknown result type (might be due to invalid IL or missing references)
		//IL_5b1f: Unknown result type (might be due to invalid IL or missing references)
		//IL_5b37: Unknown result type (might be due to invalid IL or missing references)
		//IL_5b38: Unknown result type (might be due to invalid IL or missing references)
		//IL_5b43: Unknown result type (might be due to invalid IL or missing references)
		//IL_5b44: Unknown result type (might be due to invalid IL or missing references)
		//IL_5b5c: Unknown result type (might be due to invalid IL or missing references)
		//IL_5b5d: Unknown result type (might be due to invalid IL or missing references)
		//IL_5b68: Unknown result type (might be due to invalid IL or missing references)
		//IL_5b69: Unknown result type (might be due to invalid IL or missing references)
		//IL_5b81: Unknown result type (might be due to invalid IL or missing references)
		//IL_5b82: Unknown result type (might be due to invalid IL or missing references)
		//IL_5b8d: Unknown result type (might be due to invalid IL or missing references)
		//IL_5b8e: Unknown result type (might be due to invalid IL or missing references)
		//IL_5b9c: Unknown result type (might be due to invalid IL or missing references)
		//IL_5b9d: Unknown result type (might be due to invalid IL or missing references)
		//IL_5bb5: Unknown result type (might be due to invalid IL or missing references)
		//IL_5bb6: Unknown result type (might be due to invalid IL or missing references)
		//IL_5bc1: Unknown result type (might be due to invalid IL or missing references)
		//IL_5bc2: Unknown result type (might be due to invalid IL or missing references)
		//IL_5bda: Unknown result type (might be due to invalid IL or missing references)
		//IL_5bdb: Unknown result type (might be due to invalid IL or missing references)
		//IL_5be6: Unknown result type (might be due to invalid IL or missing references)
		//IL_5be7: Unknown result type (might be due to invalid IL or missing references)
		//IL_5bf8: Unknown result type (might be due to invalid IL or missing references)
		//IL_5bfd: Unknown result type (might be due to invalid IL or missing references)
		//IL_5c11: Unknown result type (might be due to invalid IL or missing references)
		//IL_5c16: Unknown result type (might be due to invalid IL or missing references)
		//IL_5c27: Unknown result type (might be due to invalid IL or missing references)
		//IL_5c2c: Unknown result type (might be due to invalid IL or missing references)
		//IL_5c40: Unknown result type (might be due to invalid IL or missing references)
		//IL_5c45: Unknown result type (might be due to invalid IL or missing references)
		//IL_5c56: Unknown result type (might be due to invalid IL or missing references)
		//IL_5c5b: Unknown result type (might be due to invalid IL or missing references)
		//IL_5c6f: Unknown result type (might be due to invalid IL or missing references)
		//IL_5c74: Unknown result type (might be due to invalid IL or missing references)
		//IL_5c85: Unknown result type (might be due to invalid IL or missing references)
		//IL_5c8a: Unknown result type (might be due to invalid IL or missing references)
		//IL_5c9e: Unknown result type (might be due to invalid IL or missing references)
		//IL_5ca3: Unknown result type (might be due to invalid IL or missing references)
		//IL_5cbb: Unknown result type (might be due to invalid IL or missing references)
		//IL_5cbc: Unknown result type (might be due to invalid IL or missing references)
		//IL_5cca: Unknown result type (might be due to invalid IL or missing references)
		//IL_5ccb: Unknown result type (might be due to invalid IL or missing references)
		//IL_5cd6: Unknown result type (might be due to invalid IL or missing references)
		//IL_5cd7: Unknown result type (might be due to invalid IL or missing references)
		//IL_5ce8: Unknown result type (might be due to invalid IL or missing references)
		//IL_5ced: Unknown result type (might be due to invalid IL or missing references)
		//IL_5cfe: Unknown result type (might be due to invalid IL or missing references)
		//IL_5d03: Unknown result type (might be due to invalid IL or missing references)
		//IL_5d14: Unknown result type (might be due to invalid IL or missing references)
		//IL_5d19: Unknown result type (might be due to invalid IL or missing references)
		//IL_5d2a: Unknown result type (might be due to invalid IL or missing references)
		//IL_5d2f: Unknown result type (might be due to invalid IL or missing references)
		//IL_5d40: Unknown result type (might be due to invalid IL or missing references)
		//IL_5d45: Unknown result type (might be due to invalid IL or missing references)
		//IL_5d59: Unknown result type (might be due to invalid IL or missing references)
		//IL_5d5e: Unknown result type (might be due to invalid IL or missing references)
		//IL_5d6f: Unknown result type (might be due to invalid IL or missing references)
		//IL_5d74: Unknown result type (might be due to invalid IL or missing references)
		//IL_5d88: Unknown result type (might be due to invalid IL or missing references)
		//IL_5d8d: Unknown result type (might be due to invalid IL or missing references)
		//IL_5d9e: Unknown result type (might be due to invalid IL or missing references)
		//IL_5da3: Unknown result type (might be due to invalid IL or missing references)
		//IL_5db4: Unknown result type (might be due to invalid IL or missing references)
		//IL_5db9: Unknown result type (might be due to invalid IL or missing references)
		//IL_5dca: Unknown result type (might be due to invalid IL or missing references)
		//IL_5dcf: Unknown result type (might be due to invalid IL or missing references)
		//IL_5de0: Unknown result type (might be due to invalid IL or missing references)
		//IL_5de5: Unknown result type (might be due to invalid IL or missing references)
		//IL_5df6: Unknown result type (might be due to invalid IL or missing references)
		//IL_5dfb: Unknown result type (might be due to invalid IL or missing references)
		//IL_5e0c: Unknown result type (might be due to invalid IL or missing references)
		//IL_5e11: Unknown result type (might be due to invalid IL or missing references)
		//IL_5e29: Unknown result type (might be due to invalid IL or missing references)
		//IL_5e2a: Unknown result type (might be due to invalid IL or missing references)
		//IL_5e35: Unknown result type (might be due to invalid IL or missing references)
		//IL_5e36: Unknown result type (might be due to invalid IL or missing references)
		//IL_5e47: Unknown result type (might be due to invalid IL or missing references)
		//IL_5e4c: Unknown result type (might be due to invalid IL or missing references)
		//IL_5e5d: Unknown result type (might be due to invalid IL or missing references)
		//IL_5e62: Unknown result type (might be due to invalid IL or missing references)
		//IL_5e73: Unknown result type (might be due to invalid IL or missing references)
		//IL_5e78: Unknown result type (might be due to invalid IL or missing references)
		//IL_5e89: Unknown result type (might be due to invalid IL or missing references)
		//IL_5e8e: Unknown result type (might be due to invalid IL or missing references)
		//IL_5e9f: Unknown result type (might be due to invalid IL or missing references)
		//IL_5ea4: Unknown result type (might be due to invalid IL or missing references)
		//IL_5eb5: Unknown result type (might be due to invalid IL or missing references)
		//IL_5eba: Unknown result type (might be due to invalid IL or missing references)
		//IL_5ecb: Unknown result type (might be due to invalid IL or missing references)
		//IL_5ed0: Unknown result type (might be due to invalid IL or missing references)
		//IL_5ee1: Unknown result type (might be due to invalid IL or missing references)
		//IL_5ee6: Unknown result type (might be due to invalid IL or missing references)
		//IL_5ef7: Unknown result type (might be due to invalid IL or missing references)
		//IL_5efc: Unknown result type (might be due to invalid IL or missing references)
		//IL_5f0d: Unknown result type (might be due to invalid IL or missing references)
		//IL_5f12: Unknown result type (might be due to invalid IL or missing references)
		//IL_5f23: Unknown result type (might be due to invalid IL or missing references)
		//IL_5f28: Unknown result type (might be due to invalid IL or missing references)
		//IL_5f39: Unknown result type (might be due to invalid IL or missing references)
		//IL_5f3e: Unknown result type (might be due to invalid IL or missing references)
		//IL_5f4f: Unknown result type (might be due to invalid IL or missing references)
		//IL_5f54: Unknown result type (might be due to invalid IL or missing references)
		//IL_5f65: Unknown result type (might be due to invalid IL or missing references)
		//IL_5f6a: Unknown result type (might be due to invalid IL or missing references)
		//IL_5f7b: Unknown result type (might be due to invalid IL or missing references)
		//IL_5f80: Unknown result type (might be due to invalid IL or missing references)
		//IL_5f91: Unknown result type (might be due to invalid IL or missing references)
		//IL_5f96: Unknown result type (might be due to invalid IL or missing references)
		//IL_5fa7: Unknown result type (might be due to invalid IL or missing references)
		//IL_5fac: Unknown result type (might be due to invalid IL or missing references)
		//IL_5fbd: Unknown result type (might be due to invalid IL or missing references)
		//IL_5fc2: Unknown result type (might be due to invalid IL or missing references)
		//IL_5fd2: Unknown result type (might be due to invalid IL or missing references)
		//IL_5fd7: Unknown result type (might be due to invalid IL or missing references)
		//IL_5fe8: Unknown result type (might be due to invalid IL or missing references)
		//IL_5fed: Unknown result type (might be due to invalid IL or missing references)
		//IL_5ffe: Unknown result type (might be due to invalid IL or missing references)
		//IL_6003: Unknown result type (might be due to invalid IL or missing references)
		//IL_6014: Unknown result type (might be due to invalid IL or missing references)
		//IL_6019: Unknown result type (might be due to invalid IL or missing references)
		//IL_602a: Unknown result type (might be due to invalid IL or missing references)
		//IL_602f: Unknown result type (might be due to invalid IL or missing references)
		//IL_6042: Unknown result type (might be due to invalid IL or missing references)
		//IL_6047: Unknown result type (might be due to invalid IL or missing references)
		//IL_605b: Unknown result type (might be due to invalid IL or missing references)
		//IL_6060: Unknown result type (might be due to invalid IL or missing references)
		//IL_6073: Unknown result type (might be due to invalid IL or missing references)
		//IL_6078: Unknown result type (might be due to invalid IL or missing references)
		//IL_608b: Unknown result type (might be due to invalid IL or missing references)
		//IL_6090: Unknown result type (might be due to invalid IL or missing references)
		//IL_60a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_60a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_60c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_60c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_60cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_60ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_60d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_60da: Unknown result type (might be due to invalid IL or missing references)
		//IL_60e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_60e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_60fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_60ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_610a: Unknown result type (might be due to invalid IL or missing references)
		//IL_610b: Unknown result type (might be due to invalid IL or missing references)
		//IL_611c: Unknown result type (might be due to invalid IL or missing references)
		//IL_6121: Unknown result type (might be due to invalid IL or missing references)
		//IL_6135: Unknown result type (might be due to invalid IL or missing references)
		//IL_613a: Unknown result type (might be due to invalid IL or missing references)
		//IL_614e: Unknown result type (might be due to invalid IL or missing references)
		//IL_6153: Unknown result type (might be due to invalid IL or missing references)
		//IL_6164: Unknown result type (might be due to invalid IL or missing references)
		//IL_6169: Unknown result type (might be due to invalid IL or missing references)
		//IL_6183: Unknown result type (might be due to invalid IL or missing references)
		//IL_6188: Unknown result type (might be due to invalid IL or missing references)
		//IL_61a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_61a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_61ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_61ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_61be: Unknown result type (might be due to invalid IL or missing references)
		//IL_61c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_61d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_61d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_61ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_61ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_6200: Unknown result type (might be due to invalid IL or missing references)
		//IL_6205: Unknown result type (might be due to invalid IL or missing references)
		//IL_6216: Unknown result type (might be due to invalid IL or missing references)
		//IL_621b: Unknown result type (might be due to invalid IL or missing references)
		//IL_622c: Unknown result type (might be due to invalid IL or missing references)
		//IL_6231: Unknown result type (might be due to invalid IL or missing references)
		//IL_6242: Unknown result type (might be due to invalid IL or missing references)
		//IL_6247: Unknown result type (might be due to invalid IL or missing references)
		//IL_625b: Unknown result type (might be due to invalid IL or missing references)
		//IL_6260: Unknown result type (might be due to invalid IL or missing references)
		//IL_6271: Unknown result type (might be due to invalid IL or missing references)
		//IL_6276: Unknown result type (might be due to invalid IL or missing references)
		//IL_6287: Unknown result type (might be due to invalid IL or missing references)
		//IL_628c: Unknown result type (might be due to invalid IL or missing references)
		//IL_62a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_62a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_62b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_62b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_62cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_62cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_62d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_62d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_62e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_62ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_62ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_6304: Unknown result type (might be due to invalid IL or missing references)
		//IL_6315: Unknown result type (might be due to invalid IL or missing references)
		//IL_631a: Unknown result type (might be due to invalid IL or missing references)
		//IL_632d: Unknown result type (might be due to invalid IL or missing references)
		//IL_6332: Unknown result type (might be due to invalid IL or missing references)
		//IL_6343: Unknown result type (might be due to invalid IL or missing references)
		//IL_6348: Unknown result type (might be due to invalid IL or missing references)
		//IL_6359: Unknown result type (might be due to invalid IL or missing references)
		//IL_635e: Unknown result type (might be due to invalid IL or missing references)
		//IL_636f: Unknown result type (might be due to invalid IL or missing references)
		//IL_6374: Unknown result type (might be due to invalid IL or missing references)
		//IL_6385: Unknown result type (might be due to invalid IL or missing references)
		//IL_638a: Unknown result type (might be due to invalid IL or missing references)
		//IL_639a: Unknown result type (might be due to invalid IL or missing references)
		//IL_639f: Unknown result type (might be due to invalid IL or missing references)
		//IL_63b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_63b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_63c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_63cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_63e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_63ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_63fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_6403: Unknown result type (might be due to invalid IL or missing references)
		//IL_6416: Unknown result type (might be due to invalid IL or missing references)
		//IL_641b: Unknown result type (might be due to invalid IL or missing references)
		//IL_642a: Unknown result type (might be due to invalid IL or missing references)
		//IL_642f: Unknown result type (might be due to invalid IL or missing references)
		//IL_6443: Unknown result type (might be due to invalid IL or missing references)
		//IL_6448: Unknown result type (might be due to invalid IL or missing references)
		//IL_6459: Unknown result type (might be due to invalid IL or missing references)
		//IL_645e: Unknown result type (might be due to invalid IL or missing references)
		//IL_6472: Unknown result type (might be due to invalid IL or missing references)
		//IL_6477: Unknown result type (might be due to invalid IL or missing references)
		//IL_648a: Unknown result type (might be due to invalid IL or missing references)
		//IL_648f: Unknown result type (might be due to invalid IL or missing references)
		//IL_64a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_64ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_64c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_64c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_64db: Unknown result type (might be due to invalid IL or missing references)
		//IL_64e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_64f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_64f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_650d: Unknown result type (might be due to invalid IL or missing references)
		//IL_6512: Unknown result type (might be due to invalid IL or missing references)
		//IL_6529: Unknown result type (might be due to invalid IL or missing references)
		//IL_652e: Unknown result type (might be due to invalid IL or missing references)
		//IL_6545: Unknown result type (might be due to invalid IL or missing references)
		//IL_654a: Unknown result type (might be due to invalid IL or missing references)
		//IL_6561: Unknown result type (might be due to invalid IL or missing references)
		//IL_6566: Unknown result type (might be due to invalid IL or missing references)
		//IL_657d: Unknown result type (might be due to invalid IL or missing references)
		//IL_6582: Unknown result type (might be due to invalid IL or missing references)
		//IL_6596: Unknown result type (might be due to invalid IL or missing references)
		//IL_659b: Unknown result type (might be due to invalid IL or missing references)
		//IL_65af: Unknown result type (might be due to invalid IL or missing references)
		//IL_65b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_65c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_65cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_65e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_65e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_6603: Unknown result type (might be due to invalid IL or missing references)
		//IL_6608: Unknown result type (might be due to invalid IL or missing references)
		//IL_661c: Unknown result type (might be due to invalid IL or missing references)
		//IL_6621: Unknown result type (might be due to invalid IL or missing references)
		//IL_6635: Unknown result type (might be due to invalid IL or missing references)
		//IL_663a: Unknown result type (might be due to invalid IL or missing references)
		//IL_664e: Unknown result type (might be due to invalid IL or missing references)
		//IL_6653: Unknown result type (might be due to invalid IL or missing references)
		//IL_6667: Unknown result type (might be due to invalid IL or missing references)
		//IL_666c: Unknown result type (might be due to invalid IL or missing references)
		//IL_6689: Unknown result type (might be due to invalid IL or missing references)
		//IL_668e: Unknown result type (might be due to invalid IL or missing references)
		//IL_66a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_66a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_66bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_66c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_66d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_66d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_66f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_66f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_6704: Unknown result type (might be due to invalid IL or missing references)
		//IL_6706: Unknown result type (might be due to invalid IL or missing references)
		//IL_671c: Unknown result type (might be due to invalid IL or missing references)
		//IL_6721: Unknown result type (might be due to invalid IL or missing references)
		//IL_6732: Unknown result type (might be due to invalid IL or missing references)
		//IL_6737: Unknown result type (might be due to invalid IL or missing references)
		//IL_6748: Unknown result type (might be due to invalid IL or missing references)
		//IL_674d: Unknown result type (might be due to invalid IL or missing references)
		//IL_6764: Unknown result type (might be due to invalid IL or missing references)
		//IL_6769: Unknown result type (might be due to invalid IL or missing references)
		//IL_677d: Unknown result type (might be due to invalid IL or missing references)
		//IL_6782: Unknown result type (might be due to invalid IL or missing references)
		//IL_6799: Unknown result type (might be due to invalid IL or missing references)
		//IL_679e: Unknown result type (might be due to invalid IL or missing references)
		//IL_67b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_67b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_67cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_67d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_67e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_67ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_6803: Unknown result type (might be due to invalid IL or missing references)
		//IL_6808: Unknown result type (might be due to invalid IL or missing references)
		//IL_681f: Unknown result type (might be due to invalid IL or missing references)
		//IL_6824: Unknown result type (might be due to invalid IL or missing references)
		//IL_6838: Unknown result type (might be due to invalid IL or missing references)
		//IL_683d: Unknown result type (might be due to invalid IL or missing references)
		//IL_6851: Unknown result type (might be due to invalid IL or missing references)
		//IL_6856: Unknown result type (might be due to invalid IL or missing references)
		//IL_686a: Unknown result type (might be due to invalid IL or missing references)
		//IL_686f: Unknown result type (might be due to invalid IL or missing references)
		//IL_6886: Unknown result type (might be due to invalid IL or missing references)
		//IL_688b: Unknown result type (might be due to invalid IL or missing references)
		//IL_689f: Unknown result type (might be due to invalid IL or missing references)
		//IL_68a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_68b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_68bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_68d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_68d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_68ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_68f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_6906: Unknown result type (might be due to invalid IL or missing references)
		//IL_690b: Unknown result type (might be due to invalid IL or missing references)
		//IL_6922: Unknown result type (might be due to invalid IL or missing references)
		//IL_6927: Unknown result type (might be due to invalid IL or missing references)
		//IL_693e: Unknown result type (might be due to invalid IL or missing references)
		//IL_6943: Unknown result type (might be due to invalid IL or missing references)
		//IL_695a: Unknown result type (might be due to invalid IL or missing references)
		//IL_695f: Unknown result type (might be due to invalid IL or missing references)
		//IL_6973: Unknown result type (might be due to invalid IL or missing references)
		//IL_6978: Unknown result type (might be due to invalid IL or missing references)
		//IL_698c: Unknown result type (might be due to invalid IL or missing references)
		//IL_6991: Unknown result type (might be due to invalid IL or missing references)
		//IL_69a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_69aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_69be: Unknown result type (might be due to invalid IL or missing references)
		//IL_69c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_69d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_69dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_69f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_69f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_6a09: Unknown result type (might be due to invalid IL or missing references)
		//IL_6a0e: Unknown result type (might be due to invalid IL or missing references)
		//IL_6a25: Unknown result type (might be due to invalid IL or missing references)
		//IL_6a2a: Unknown result type (might be due to invalid IL or missing references)
		//IL_6a41: Unknown result type (might be due to invalid IL or missing references)
		//IL_6a46: Unknown result type (might be due to invalid IL or missing references)
		//IL_6a5a: Unknown result type (might be due to invalid IL or missing references)
		//IL_6a5f: Unknown result type (might be due to invalid IL or missing references)
		//IL_6a76: Unknown result type (might be due to invalid IL or missing references)
		//IL_6a7b: Unknown result type (might be due to invalid IL or missing references)
		//IL_6a8f: Unknown result type (might be due to invalid IL or missing references)
		//IL_6a94: Unknown result type (might be due to invalid IL or missing references)
		//IL_6aa8: Unknown result type (might be due to invalid IL or missing references)
		//IL_6aad: Unknown result type (might be due to invalid IL or missing references)
		//IL_6ac1: Unknown result type (might be due to invalid IL or missing references)
		//IL_6ac6: Unknown result type (might be due to invalid IL or missing references)
		//IL_6ada: Unknown result type (might be due to invalid IL or missing references)
		//IL_6adf: Unknown result type (might be due to invalid IL or missing references)
		//IL_6af6: Unknown result type (might be due to invalid IL or missing references)
		//IL_6afb: Unknown result type (might be due to invalid IL or missing references)
		//IL_6b15: Unknown result type (might be due to invalid IL or missing references)
		//IL_6b1a: Unknown result type (might be due to invalid IL or missing references)
		//IL_6b34: Unknown result type (might be due to invalid IL or missing references)
		//IL_6b39: Unknown result type (might be due to invalid IL or missing references)
		//IL_6b50: Unknown result type (might be due to invalid IL or missing references)
		//IL_6b55: Unknown result type (might be due to invalid IL or missing references)
		//IL_6b69: Unknown result type (might be due to invalid IL or missing references)
		//IL_6b6e: Unknown result type (might be due to invalid IL or missing references)
		//IL_6b82: Unknown result type (might be due to invalid IL or missing references)
		//IL_6b87: Unknown result type (might be due to invalid IL or missing references)
		//IL_6b9a: Unknown result type (might be due to invalid IL or missing references)
		//IL_6b9f: Unknown result type (might be due to invalid IL or missing references)
		//IL_6bb3: Unknown result type (might be due to invalid IL or missing references)
		//IL_6bb8: Unknown result type (might be due to invalid IL or missing references)
		//IL_6bcc: Unknown result type (might be due to invalid IL or missing references)
		//IL_6bd1: Unknown result type (might be due to invalid IL or missing references)
		//IL_6be5: Unknown result type (might be due to invalid IL or missing references)
		//IL_6bea: Unknown result type (might be due to invalid IL or missing references)
		//IL_6bfc: Unknown result type (might be due to invalid IL or missing references)
		//IL_6c01: Unknown result type (might be due to invalid IL or missing references)
		//IL_6c15: Unknown result type (might be due to invalid IL or missing references)
		//IL_6c1a: Unknown result type (might be due to invalid IL or missing references)
		//IL_6c2e: Unknown result type (might be due to invalid IL or missing references)
		//IL_6c33: Unknown result type (might be due to invalid IL or missing references)
		//IL_6c47: Unknown result type (might be due to invalid IL or missing references)
		//IL_6c4c: Unknown result type (might be due to invalid IL or missing references)
		//IL_6c60: Unknown result type (might be due to invalid IL or missing references)
		//IL_6c65: Unknown result type (might be due to invalid IL or missing references)
		//IL_6c79: Unknown result type (might be due to invalid IL or missing references)
		//IL_6c7e: Unknown result type (might be due to invalid IL or missing references)
		//IL_6c91: Unknown result type (might be due to invalid IL or missing references)
		//IL_6c96: Unknown result type (might be due to invalid IL or missing references)
		//IL_6cac: Unknown result type (might be due to invalid IL or missing references)
		//IL_6cb1: Unknown result type (might be due to invalid IL or missing references)
		//IL_6cc8: Unknown result type (might be due to invalid IL or missing references)
		//IL_6ccd: Unknown result type (might be due to invalid IL or missing references)
		//IL_6ce0: Unknown result type (might be due to invalid IL or missing references)
		//IL_6ce5: Unknown result type (might be due to invalid IL or missing references)
		//IL_6cf9: Unknown result type (might be due to invalid IL or missing references)
		//IL_6cfe: Unknown result type (might be due to invalid IL or missing references)
		//IL_6d12: Unknown result type (might be due to invalid IL or missing references)
		//IL_6d17: Unknown result type (might be due to invalid IL or missing references)
		//IL_6d2b: Unknown result type (might be due to invalid IL or missing references)
		//IL_6d30: Unknown result type (might be due to invalid IL or missing references)
		//IL_6d44: Unknown result type (might be due to invalid IL or missing references)
		//IL_6d49: Unknown result type (might be due to invalid IL or missing references)
		//IL_6d60: Unknown result type (might be due to invalid IL or missing references)
		//IL_6d65: Unknown result type (might be due to invalid IL or missing references)
		//IL_6d79: Unknown result type (might be due to invalid IL or missing references)
		//IL_6d7e: Unknown result type (might be due to invalid IL or missing references)
		//IL_6d98: Unknown result type (might be due to invalid IL or missing references)
		//IL_6d9d: Unknown result type (might be due to invalid IL or missing references)
		//IL_6db3: Unknown result type (might be due to invalid IL or missing references)
		//IL_6db8: Unknown result type (might be due to invalid IL or missing references)
		//IL_6dcc: Unknown result type (might be due to invalid IL or missing references)
		//IL_6dd1: Unknown result type (might be due to invalid IL or missing references)
		//IL_6deb: Unknown result type (might be due to invalid IL or missing references)
		//IL_6df0: Unknown result type (might be due to invalid IL or missing references)
		//IL_6e04: Unknown result type (might be due to invalid IL or missing references)
		//IL_6e09: Unknown result type (might be due to invalid IL or missing references)
		//IL_6e1d: Unknown result type (might be due to invalid IL or missing references)
		//IL_6e22: Unknown result type (might be due to invalid IL or missing references)
		//IL_6e36: Unknown result type (might be due to invalid IL or missing references)
		//IL_6e3b: Unknown result type (might be due to invalid IL or missing references)
		//IL_6e4f: Unknown result type (might be due to invalid IL or missing references)
		//IL_6e54: Unknown result type (might be due to invalid IL or missing references)
		//IL_6e68: Unknown result type (might be due to invalid IL or missing references)
		//IL_6e6d: Unknown result type (might be due to invalid IL or missing references)
		//IL_6e81: Unknown result type (might be due to invalid IL or missing references)
		//IL_6e86: Unknown result type (might be due to invalid IL or missing references)
		//IL_6e9a: Unknown result type (might be due to invalid IL or missing references)
		//IL_6e9f: Unknown result type (might be due to invalid IL or missing references)
		//IL_6eb3: Unknown result type (might be due to invalid IL or missing references)
		//IL_6eb8: Unknown result type (might be due to invalid IL or missing references)
		//IL_6ecc: Unknown result type (might be due to invalid IL or missing references)
		//IL_6ed1: Unknown result type (might be due to invalid IL or missing references)
		//IL_6ee5: Unknown result type (might be due to invalid IL or missing references)
		//IL_6eea: Unknown result type (might be due to invalid IL or missing references)
		//IL_6efe: Unknown result type (might be due to invalid IL or missing references)
		//IL_6f03: Unknown result type (might be due to invalid IL or missing references)
		//IL_6f17: Unknown result type (might be due to invalid IL or missing references)
		//IL_6f1c: Unknown result type (might be due to invalid IL or missing references)
		//IL_6f30: Unknown result type (might be due to invalid IL or missing references)
		//IL_6f35: Unknown result type (might be due to invalid IL or missing references)
		//IL_6f49: Unknown result type (might be due to invalid IL or missing references)
		//IL_6f4e: Unknown result type (might be due to invalid IL or missing references)
		//IL_6f62: Unknown result type (might be due to invalid IL or missing references)
		//IL_6f67: Unknown result type (might be due to invalid IL or missing references)
		//IL_6f7b: Unknown result type (might be due to invalid IL or missing references)
		//IL_6f80: Unknown result type (might be due to invalid IL or missing references)
		//IL_6f94: Unknown result type (might be due to invalid IL or missing references)
		//IL_6f99: Unknown result type (might be due to invalid IL or missing references)
		//IL_6fad: Unknown result type (might be due to invalid IL or missing references)
		//IL_6fb2: Unknown result type (might be due to invalid IL or missing references)
		//IL_6fc6: Unknown result type (might be due to invalid IL or missing references)
		//IL_6fcb: Unknown result type (might be due to invalid IL or missing references)
		//IL_6fdf: Unknown result type (might be due to invalid IL or missing references)
		//IL_6fe4: Unknown result type (might be due to invalid IL or missing references)
		//IL_6ff8: Unknown result type (might be due to invalid IL or missing references)
		//IL_6ffd: Unknown result type (might be due to invalid IL or missing references)
		//IL_7014: Unknown result type (might be due to invalid IL or missing references)
		//IL_7019: Unknown result type (might be due to invalid IL or missing references)
		//IL_702d: Unknown result type (might be due to invalid IL or missing references)
		//IL_7032: Unknown result type (might be due to invalid IL or missing references)
		//IL_7046: Unknown result type (might be due to invalid IL or missing references)
		//IL_704b: Unknown result type (might be due to invalid IL or missing references)
		//IL_705f: Unknown result type (might be due to invalid IL or missing references)
		//IL_7064: Unknown result type (might be due to invalid IL or missing references)
		//IL_7078: Unknown result type (might be due to invalid IL or missing references)
		//IL_707d: Unknown result type (might be due to invalid IL or missing references)
		//IL_7091: Unknown result type (might be due to invalid IL or missing references)
		//IL_7096: Unknown result type (might be due to invalid IL or missing references)
		//IL_70ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_70b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_70c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_70cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_70df: Unknown result type (might be due to invalid IL or missing references)
		//IL_70e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_70fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_7100: Unknown result type (might be due to invalid IL or missing references)
		//IL_7114: Unknown result type (might be due to invalid IL or missing references)
		//IL_7119: Unknown result type (might be due to invalid IL or missing references)
		//IL_712d: Unknown result type (might be due to invalid IL or missing references)
		//IL_7132: Unknown result type (might be due to invalid IL or missing references)
		//IL_7146: Unknown result type (might be due to invalid IL or missing references)
		//IL_714b: Unknown result type (might be due to invalid IL or missing references)
		//IL_715f: Unknown result type (might be due to invalid IL or missing references)
		//IL_7164: Unknown result type (might be due to invalid IL or missing references)
		//IL_717b: Unknown result type (might be due to invalid IL or missing references)
		//IL_7180: Unknown result type (might be due to invalid IL or missing references)
		//IL_7197: Unknown result type (might be due to invalid IL or missing references)
		//IL_719c: Unknown result type (might be due to invalid IL or missing references)
		//IL_71b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_71b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_71cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_71d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_71e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_71ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_71fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_7203: Unknown result type (might be due to invalid IL or missing references)
		//IL_7217: Unknown result type (might be due to invalid IL or missing references)
		//IL_721c: Unknown result type (might be due to invalid IL or missing references)
		//IL_7230: Unknown result type (might be due to invalid IL or missing references)
		//IL_7235: Unknown result type (might be due to invalid IL or missing references)
		//IL_724c: Unknown result type (might be due to invalid IL or missing references)
		//IL_7251: Unknown result type (might be due to invalid IL or missing references)
		//IL_726b: Unknown result type (might be due to invalid IL or missing references)
		//IL_7270: Unknown result type (might be due to invalid IL or missing references)
		//IL_728a: Unknown result type (might be due to invalid IL or missing references)
		//IL_728f: Unknown result type (might be due to invalid IL or missing references)
		//IL_72a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_72ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_72bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_72c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_72d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_72dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_72f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_72f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_7309: Unknown result type (might be due to invalid IL or missing references)
		//IL_730e: Unknown result type (might be due to invalid IL or missing references)
		//IL_7322: Unknown result type (might be due to invalid IL or missing references)
		//IL_7327: Unknown result type (might be due to invalid IL or missing references)
		//IL_733b: Unknown result type (might be due to invalid IL or missing references)
		//IL_7340: Unknown result type (might be due to invalid IL or missing references)
		//IL_7352: Unknown result type (might be due to invalid IL or missing references)
		//IL_7357: Unknown result type (might be due to invalid IL or missing references)
		//IL_736b: Unknown result type (might be due to invalid IL or missing references)
		//IL_7370: Unknown result type (might be due to invalid IL or missing references)
		//IL_7384: Unknown result type (might be due to invalid IL or missing references)
		//IL_7389: Unknown result type (might be due to invalid IL or missing references)
		//IL_739d: Unknown result type (might be due to invalid IL or missing references)
		//IL_73a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_73b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_73bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_73cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_73d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_73eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_73f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_7404: Unknown result type (might be due to invalid IL or missing references)
		//IL_7409: Unknown result type (might be due to invalid IL or missing references)
		//IL_741d: Unknown result type (might be due to invalid IL or missing references)
		//IL_7422: Unknown result type (might be due to invalid IL or missing references)
		//IL_7439: Unknown result type (might be due to invalid IL or missing references)
		//IL_743e: Unknown result type (might be due to invalid IL or missing references)
		//IL_7452: Unknown result type (might be due to invalid IL or missing references)
		//IL_7457: Unknown result type (might be due to invalid IL or missing references)
		//IL_746b: Unknown result type (might be due to invalid IL or missing references)
		//IL_7470: Unknown result type (might be due to invalid IL or missing references)
		//IL_7484: Unknown result type (might be due to invalid IL or missing references)
		//IL_7489: Unknown result type (might be due to invalid IL or missing references)
		//IL_749c: Unknown result type (might be due to invalid IL or missing references)
		//IL_74a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_74b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_74ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_74ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_74d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_74e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_74ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_7500: Unknown result type (might be due to invalid IL or missing references)
		//IL_7505: Unknown result type (might be due to invalid IL or missing references)
		//IL_751c: Unknown result type (might be due to invalid IL or missing references)
		//IL_7521: Unknown result type (might be due to invalid IL or missing references)
		//IL_7538: Unknown result type (might be due to invalid IL or missing references)
		//IL_753d: Unknown result type (might be due to invalid IL or missing references)
		//IL_75da: Unknown result type (might be due to invalid IL or missing references)
		//IL_75df: Unknown result type (might be due to invalid IL or missing references)
		//IL_767e: Unknown result type (might be due to invalid IL or missing references)
		//IL_7683: Unknown result type (might be due to invalid IL or missing references)
		//IL_7722: Unknown result type (might be due to invalid IL or missing references)
		//IL_7727: Unknown result type (might be due to invalid IL or missing references)
		//IL_7779: Unknown result type (might be due to invalid IL or missing references)
		//IL_777e: Unknown result type (might be due to invalid IL or missing references)
		//IL_7826: Unknown result type (might be due to invalid IL or missing references)
		//IL_782b: Unknown result type (might be due to invalid IL or missing references)
		//IL_77d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_77de: Unknown result type (might be due to invalid IL or missing references)
		//IL_7879: Unknown result type (might be due to invalid IL or missing references)
		//IL_787e: Unknown result type (might be due to invalid IL or missing references)
		//IL_7961: Unknown result type (might be due to invalid IL or missing references)
		//IL_7966: Unknown result type (might be due to invalid IL or missing references)
		//IL_7906: Unknown result type (might be due to invalid IL or missing references)
		//IL_790b: Unknown result type (might be due to invalid IL or missing references)
		//IL_7994: Unknown result type (might be due to invalid IL or missing references)
		//IL_7999: Unknown result type (might be due to invalid IL or missing references)
		//IL_79cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_79d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_7a02: Unknown result type (might be due to invalid IL or missing references)
		//IL_7a07: Unknown result type (might be due to invalid IL or missing references)
		//IL_7a30: Unknown result type (might be due to invalid IL or missing references)
		//IL_7a32: Unknown result type (might be due to invalid IL or missing references)
		Color[][] array = new Color[TileID.Count][];
		for (int i = 0; i < TileID.Count; i++)
		{
			array[i] = (Color[])(object)new Color[12];
		}
		array[656][0] = new Color(21, 124, 212);
		array[624][0] = new Color(210, 91, 77);
		array[621][0] = new Color(250, 250, 250);
		array[622][0] = new Color(235, 235, 249);
		array[518][0] = new Color(26, 196, 84);
		array[518][1] = new Color(48, 208, 234);
		array[518][2] = new Color(135, 196, 26);
		array[519][0] = new Color(28, 216, 109);
		array[519][1] = new Color(107, 182, 0);
		array[519][2] = new Color(75, 184, 230);
		array[519][3] = new Color(208, 80, 80);
		array[519][4] = new Color(141, 137, 223);
		array[519][5] = new Color(182, 175, 130);
		array[549][0] = new Color(54, 83, 20);
		array[528][0] = new Color(182, 175, 130);
		array[529][0] = new Color(99, 150, 8);
		array[529][1] = new Color(139, 154, 64);
		array[529][2] = new Color(34, 129, 168);
		array[529][3] = new Color(180, 82, 82);
		array[529][4] = new Color(113, 108, 205);
		Color color = default(Color);
		((Color)(ref color))._002Ector(151, 107, 75);
		array[0][0] = color;
		array[668][0] = color;
		array[5][0] = color;
		array[5][1] = new Color(182, 175, 130);
		Color color3 = default(Color);
		((Color)(ref color3))._002Ector(127, 127, 127);
		array[583][0] = color3;
		array[584][0] = color3;
		array[585][0] = color3;
		array[586][0] = color3;
		array[587][0] = color3;
		array[588][0] = color3;
		array[589][0] = color3;
		array[590][0] = color3;
		array[595][0] = color;
		array[596][0] = color;
		array[615][0] = color;
		array[616][0] = color;
		array[634][0] = new Color(145, 120, 120);
		array[633][0] = new Color(210, 140, 100);
		array[637][0] = new Color(200, 120, 75);
		array[638][0] = new Color(200, 120, 75);
		array[30][0] = color;
		array[191][0] = color;
		array[272][0] = new Color(121, 119, 101);
		((Color)(ref color))._002Ector(128, 128, 128);
		array[1][0] = color;
		array[38][0] = color;
		array[48][0] = color;
		array[130][0] = color;
		array[138][0] = color;
		array[664][0] = color;
		array[273][0] = color;
		array[283][0] = color;
		array[618][0] = color;
		array[654][0] = new Color(200, 44, 28);
		array[2][0] = new Color(28, 216, 94);
		array[477][0] = new Color(28, 216, 94);
		array[492][0] = new Color(78, 193, 227);
		((Color)(ref color))._002Ector(26, 196, 84);
		array[3][0] = color;
		array[192][0] = color;
		array[73][0] = new Color(27, 197, 109);
		array[52][0] = new Color(23, 177, 76);
		array[353][0] = new Color(28, 216, 94);
		array[20][0] = new Color(163, 116, 81);
		array[6][0] = new Color(140, 101, 80);
		((Color)(ref color))._002Ector(150, 67, 22);
		array[7][0] = color;
		array[47][0] = color;
		array[284][0] = color;
		array[682][0] = color;
		array[560][0] = color;
		((Color)(ref color))._002Ector(185, 164, 23);
		array[8][0] = color;
		array[45][0] = color;
		array[680][0] = color;
		array[560][2] = color;
		((Color)(ref color))._002Ector(185, 194, 195);
		array[9][0] = color;
		array[46][0] = color;
		array[681][0] = color;
		array[560][1] = color;
		((Color)(ref color))._002Ector(98, 95, 167);
		array[22][0] = color;
		array[140][0] = color;
		array[23][0] = new Color(141, 137, 223);
		array[24][0] = new Color(122, 116, 218);
		array[636][0] = new Color(122, 116, 218);
		array[25][0] = new Color(109, 90, 128);
		array[37][0] = new Color(104, 86, 84);
		array[39][0] = new Color(181, 62, 59);
		array[40][0] = new Color(146, 81, 68);
		array[41][0] = new Color(66, 84, 109);
		array[677][0] = new Color(66, 84, 109);
		array[481][0] = new Color(66, 84, 109);
		array[43][0] = new Color(84, 100, 63);
		array[678][0] = new Color(84, 100, 63);
		array[482][0] = new Color(84, 100, 63);
		array[44][0] = new Color(107, 68, 99);
		array[679][0] = new Color(107, 68, 99);
		array[483][0] = new Color(107, 68, 99);
		array[53][0] = new Color(186, 168, 84);
		((Color)(ref color))._002Ector(190, 171, 94);
		array[151][0] = color;
		array[154][0] = color;
		array[274][0] = color;
		array[328][0] = new Color(200, 246, 254);
		array[329][0] = new Color(15, 15, 15);
		array[54][0] = new Color(200, 246, 254);
		array[56][0] = new Color(43, 40, 84);
		array[75][0] = new Color(26, 26, 26);
		array[683][0] = new Color(100, 90, 190);
		array[57][0] = new Color(68, 68, 76);
		((Color)(ref color))._002Ector(142, 66, 66);
		array[58][0] = color;
		array[76][0] = color;
		array[684][0] = color;
		((Color)(ref color))._002Ector(92, 68, 73);
		array[59][0] = color;
		array[120][0] = color;
		array[60][0] = new Color(143, 215, 29);
		array[61][0] = new Color(135, 196, 26);
		array[74][0] = new Color(96, 197, 27);
		array[62][0] = new Color(121, 176, 24);
		array[233][0] = new Color(107, 182, 29);
		array[652][0] = array[233][0];
		array[651][0] = array[233][0];
		array[63][0] = new Color(110, 140, 182);
		array[64][0] = new Color(196, 96, 114);
		array[65][0] = new Color(56, 150, 97);
		array[66][0] = new Color(160, 118, 58);
		array[67][0] = new Color(140, 58, 166);
		array[68][0] = new Color(125, 191, 197);
		array[566][0] = new Color(233, 180, 90);
		array[70][0] = new Color(93, 127, 255);
		((Color)(ref color))._002Ector(182, 175, 130);
		array[71][0] = color;
		array[72][0] = color;
		array[190][0] = color;
		array[578][0] = new Color(172, 155, 110);
		((Color)(ref color))._002Ector(73, 120, 17);
		array[80][0] = color;
		array[484][0] = color;
		array[188][0] = color;
		array[80][1] = new Color(87, 84, 151);
		array[80][2] = new Color(34, 129, 168);
		array[80][3] = new Color(130, 56, 55);
		((Color)(ref color))._002Ector(11, 80, 143);
		array[107][0] = color;
		array[121][0] = color;
		array[685][0] = color;
		((Color)(ref color))._002Ector(91, 169, 169);
		array[108][0] = color;
		array[122][0] = color;
		array[686][0] = color;
		((Color)(ref color))._002Ector(128, 26, 52);
		array[111][0] = color;
		array[150][0] = color;
		array[109][0] = new Color(78, 193, 227);
		array[110][0] = new Color(48, 186, 135);
		array[113][0] = new Color(48, 208, 234);
		array[115][0] = new Color(33, 171, 207);
		array[112][0] = new Color(103, 98, 122);
		((Color)(ref color))._002Ector(238, 225, 218);
		array[116][0] = color;
		array[118][0] = color;
		array[117][0] = new Color(181, 172, 190);
		array[119][0] = new Color(107, 92, 108);
		array[123][0] = new Color(106, 107, 118);
		array[124][0] = new Color(73, 51, 36);
		array[131][0] = new Color(52, 52, 52);
		array[145][0] = new Color(192, 30, 30);
		array[146][0] = new Color(43, 192, 30);
		((Color)(ref color))._002Ector(211, 236, 241);
		array[147][0] = color;
		array[148][0] = color;
		array[152][0] = new Color(128, 133, 184);
		array[153][0] = new Color(239, 141, 126);
		array[155][0] = new Color(131, 162, 161);
		array[156][0] = new Color(170, 171, 157);
		array[157][0] = new Color(104, 100, 126);
		((Color)(ref color))._002Ector(145, 81, 85);
		array[158][0] = color;
		array[232][0] = color;
		array[575][0] = new Color(125, 61, 65);
		array[159][0] = new Color(148, 133, 98);
		array[161][0] = new Color(144, 195, 232);
		array[162][0] = new Color(184, 219, 240);
		array[163][0] = new Color(174, 145, 214);
		array[164][0] = new Color(218, 182, 204);
		array[170][0] = new Color(27, 109, 69);
		array[171][0] = new Color(33, 135, 85);
		((Color)(ref color))._002Ector(129, 125, 93);
		array[166][0] = color;
		array[175][0] = color;
		array[167][0] = new Color(62, 82, 114);
		((Color)(ref color))._002Ector(132, 157, 127);
		array[168][0] = color;
		array[176][0] = color;
		((Color)(ref color))._002Ector(152, 171, 198);
		array[169][0] = color;
		array[177][0] = color;
		array[179][0] = new Color(49, 134, 114);
		array[180][0] = new Color(126, 134, 49);
		array[181][0] = new Color(134, 59, 49);
		array[182][0] = new Color(43, 86, 140);
		array[183][0] = new Color(121, 49, 134);
		array[381][0] = new Color(254, 121, 2);
		array[687][0] = new Color(254, 121, 2);
		array[534][0] = new Color(114, 254, 2);
		array[689][0] = new Color(114, 254, 2);
		array[536][0] = new Color(0, 197, 208);
		array[690][0] = new Color(0, 197, 208);
		array[539][0] = new Color(208, 0, 126);
		array[688][0] = new Color(208, 0, 126);
		array[625][0] = new Color(220, 12, 237);
		array[691][0] = new Color(220, 12, 237);
		array[627][0] = new Color(255, 76, 76);
		array[627][1] = new Color(255, 195, 76);
		array[627][2] = new Color(195, 255, 76);
		array[627][3] = new Color(76, 255, 76);
		array[627][4] = new Color(76, 255, 195);
		array[627][5] = new Color(76, 195, 255);
		array[627][6] = new Color(77, 76, 255);
		array[627][7] = new Color(196, 76, 255);
		array[627][8] = new Color(255, 76, 195);
		array[512][0] = new Color(49, 134, 114);
		array[513][0] = new Color(126, 134, 49);
		array[514][0] = new Color(134, 59, 49);
		array[515][0] = new Color(43, 86, 140);
		array[516][0] = new Color(121, 49, 134);
		array[517][0] = new Color(254, 121, 2);
		array[535][0] = new Color(114, 254, 2);
		array[537][0] = new Color(0, 197, 208);
		array[540][0] = new Color(208, 0, 126);
		array[626][0] = new Color(220, 12, 237);
		for (int j = 0; j < array[628].Length; j++)
		{
			array[628][j] = array[627][j];
		}
		for (int k = 0; k < array[692].Length; k++)
		{
			array[692][k] = array[627][k];
		}
		for (int l = 0; l < array[160].Length; l++)
		{
			array[160][l] = array[627][l];
		}
		array[184][0] = new Color(29, 106, 88);
		array[184][1] = new Color(94, 100, 36);
		array[184][2] = new Color(96, 44, 40);
		array[184][3] = new Color(34, 63, 102);
		array[184][4] = new Color(79, 35, 95);
		array[184][5] = new Color(253, 62, 3);
		array[184][6] = new Color(22, 123, 62);
		array[184][7] = new Color(0, 106, 148);
		array[184][8] = new Color(148, 0, 132);
		array[184][9] = new Color(122, 24, 168);
		array[184][10] = new Color(220, 20, 20);
		array[189][0] = new Color(223, 255, 255);
		array[193][0] = new Color(56, 121, 255);
		array[194][0] = new Color(157, 157, 107);
		array[195][0] = new Color(134, 22, 34);
		array[196][0] = new Color(147, 144, 178);
		array[197][0] = new Color(97, 200, 225);
		array[198][0] = new Color(62, 61, 52);
		array[199][0] = new Color(208, 80, 80);
		array[201][0] = new Color(203, 61, 64);
		array[205][0] = new Color(186, 50, 52);
		array[200][0] = new Color(216, 152, 144);
		array[202][0] = new Color(213, 178, 28);
		array[203][0] = new Color(128, 44, 45);
		array[204][0] = new Color(125, 55, 65);
		array[206][0] = new Color(124, 175, 201);
		array[208][0] = new Color(88, 105, 118);
		array[211][0] = new Color(191, 233, 115);
		array[213][0] = new Color(137, 120, 67);
		array[214][0] = new Color(103, 103, 103);
		array[221][0] = new Color(239, 90, 50);
		array[222][0] = new Color(231, 96, 228);
		array[223][0] = new Color(57, 85, 101);
		array[224][0] = new Color(107, 132, 139);
		array[225][0] = new Color(227, 125, 22);
		array[226][0] = new Color(141, 56, 0);
		array[229][0] = new Color(255, 156, 12);
		array[659][0] = new Color(247, 228, 254);
		array[230][0] = new Color(131, 79, 13);
		array[234][0] = new Color(53, 44, 41);
		array[235][0] = new Color(214, 184, 46);
		array[236][0] = new Color(149, 232, 87);
		array[237][0] = new Color(255, 241, 51);
		array[238][0] = new Color(225, 128, 206);
		array[655][0] = new Color(225, 128, 206);
		array[243][0] = new Color(198, 196, 170);
		array[248][0] = new Color(219, 71, 38);
		array[249][0] = new Color(235, 38, 231);
		array[250][0] = new Color(86, 85, 92);
		array[251][0] = new Color(235, 150, 23);
		array[252][0] = new Color(153, 131, 44);
		array[253][0] = new Color(57, 48, 97);
		array[254][0] = new Color(248, 158, 92);
		array[255][0] = new Color(107, 49, 154);
		array[256][0] = new Color(154, 148, 49);
		array[257][0] = new Color(49, 49, 154);
		array[258][0] = new Color(49, 154, 68);
		array[259][0] = new Color(154, 49, 77);
		array[260][0] = new Color(85, 89, 118);
		array[261][0] = new Color(154, 83, 49);
		array[262][0] = new Color(221, 79, 255);
		array[263][0] = new Color(250, 255, 79);
		array[264][0] = new Color(79, 102, 255);
		array[265][0] = new Color(79, 255, 89);
		array[266][0] = new Color(255, 79, 79);
		array[267][0] = new Color(240, 240, 247);
		array[268][0] = new Color(255, 145, 79);
		array[287][0] = new Color(79, 128, 17);
		((Color)(ref color))._002Ector(122, 217, 232);
		array[275][0] = color;
		array[276][0] = color;
		array[277][0] = color;
		array[278][0] = color;
		array[279][0] = color;
		array[280][0] = color;
		array[281][0] = color;
		array[282][0] = color;
		array[285][0] = color;
		array[286][0] = color;
		array[288][0] = color;
		array[289][0] = color;
		array[290][0] = color;
		array[291][0] = color;
		array[292][0] = color;
		array[293][0] = color;
		array[294][0] = color;
		array[295][0] = color;
		array[296][0] = color;
		array[297][0] = color;
		array[298][0] = color;
		array[299][0] = color;
		array[309][0] = color;
		array[310][0] = color;
		array[413][0] = color;
		array[339][0] = color;
		array[542][0] = color;
		array[632][0] = color;
		array[640][0] = color;
		array[643][0] = color;
		array[644][0] = color;
		array[645][0] = color;
		array[358][0] = color;
		array[359][0] = color;
		array[360][0] = color;
		array[361][0] = color;
		array[362][0] = color;
		array[363][0] = color;
		array[364][0] = color;
		array[391][0] = color;
		array[392][0] = color;
		array[393][0] = color;
		array[394][0] = color;
		array[414][0] = color;
		array[505][0] = color;
		array[543][0] = color;
		array[598][0] = color;
		array[521][0] = color;
		array[522][0] = color;
		array[523][0] = color;
		array[524][0] = color;
		array[525][0] = color;
		array[526][0] = color;
		array[527][0] = color;
		array[532][0] = color;
		array[533][0] = color;
		array[538][0] = color;
		array[544][0] = color;
		array[629][0] = color;
		array[550][0] = color;
		array[551][0] = color;
		array[553][0] = color;
		array[554][0] = color;
		array[555][0] = color;
		array[556][0] = color;
		array[558][0] = color;
		array[559][0] = color;
		array[580][0] = color;
		array[582][0] = color;
		array[599][0] = color;
		array[600][0] = color;
		array[601][0] = color;
		array[602][0] = color;
		array[603][0] = color;
		array[604][0] = color;
		array[605][0] = color;
		array[606][0] = color;
		array[607][0] = color;
		array[608][0] = color;
		array[609][0] = color;
		array[610][0] = color;
		array[611][0] = color;
		array[612][0] = color;
		array[619][0] = color;
		array[620][0] = color;
		array[630][0] = new Color(117, 145, 73);
		array[631][0] = new Color(122, 234, 225);
		array[552][0] = array[53][0];
		array[564][0] = new Color(87, 127, 220);
		array[408][0] = new Color(85, 83, 82);
		array[409][0] = new Color(85, 83, 82);
		array[669][0] = new Color(83, 46, 57);
		array[670][0] = new Color(91, 87, 167);
		array[671][0] = new Color(23, 33, 81);
		array[672][0] = new Color(53, 133, 103);
		array[673][0] = new Color(11, 67, 80);
		array[674][0] = new Color(40, 49, 60);
		array[675][0] = new Color(21, 13, 77);
		array[676][0] = new Color(195, 201, 215);
		array[415][0] = new Color(249, 75, 7);
		array[416][0] = new Color(0, 160, 170);
		array[417][0] = new Color(160, 87, 234);
		array[418][0] = new Color(22, 173, 254);
		array[489][0] = new Color(255, 29, 136);
		array[490][0] = new Color(211, 211, 211);
		array[311][0] = new Color(117, 61, 25);
		array[312][0] = new Color(204, 93, 73);
		array[313][0] = new Color(87, 150, 154);
		array[4][0] = new Color(253, 221, 3);
		array[4][1] = new Color(253, 221, 3);
		((Color)(ref color))._002Ector(253, 221, 3);
		array[93][0] = color;
		array[33][0] = color;
		array[174][0] = color;
		array[100][0] = color;
		array[98][0] = color;
		array[173][0] = color;
		((Color)(ref color))._002Ector(119, 105, 79);
		array[11][0] = color;
		array[10][0] = color;
		array[593][0] = color;
		array[594][0] = color;
		((Color)(ref color))._002Ector(191, 142, 111);
		array[14][0] = color;
		array[469][0] = color;
		array[486][0] = color;
		array[488][0] = new Color(127, 92, 69);
		array[487][0] = color;
		array[487][1] = color;
		array[15][0] = color;
		array[15][1] = color;
		array[497][0] = color;
		array[18][0] = color;
		array[19][0] = color;
		array[19][1] = Color.Black;
		array[55][0] = color;
		array[79][0] = color;
		array[86][0] = color;
		array[87][0] = color;
		array[88][0] = color;
		array[89][0] = color;
		array[89][1] = color;
		array[89][2] = new Color(105, 107, 125);
		array[94][0] = color;
		array[101][0] = color;
		array[104][0] = color;
		array[106][0] = color;
		array[114][0] = color;
		array[128][0] = color;
		array[139][0] = color;
		array[172][0] = color;
		array[216][0] = color;
		array[269][0] = color;
		array[334][0] = color;
		array[471][0] = color;
		array[470][0] = color;
		array[475][0] = color;
		array[377][0] = color;
		array[380][0] = color;
		array[395][0] = color;
		array[573][0] = color;
		array[12][0] = new Color(174, 24, 69);
		array[665][0] = new Color(174, 24, 69);
		array[639][0] = new Color(110, 105, 255);
		array[13][0] = new Color(133, 213, 247);
		((Color)(ref color))._002Ector(144, 148, 144);
		array[17][0] = color;
		array[90][0] = color;
		array[96][0] = color;
		array[97][0] = color;
		array[99][0] = color;
		array[132][0] = color;
		array[142][0] = color;
		array[143][0] = color;
		array[144][0] = color;
		array[207][0] = color;
		array[209][0] = color;
		array[212][0] = color;
		array[217][0] = color;
		array[218][0] = color;
		array[219][0] = color;
		array[220][0] = color;
		array[228][0] = color;
		array[300][0] = color;
		array[301][0] = color;
		array[302][0] = color;
		array[303][0] = color;
		array[304][0] = color;
		array[305][0] = color;
		array[306][0] = color;
		array[307][0] = color;
		array[308][0] = color;
		array[567][0] = color;
		array[349][0] = new Color(144, 148, 144);
		array[531][0] = new Color(144, 148, 144);
		array[105][0] = new Color(144, 148, 144);
		array[105][1] = new Color(177, 92, 31);
		array[105][2] = new Color(201, 188, 170);
		array[137][0] = new Color(144, 148, 144);
		array[137][1] = new Color(141, 56, 0);
		array[137][2] = new Color(144, 148, 144);
		array[16][0] = new Color(140, 130, 116);
		array[26][0] = new Color(119, 101, 125);
		array[26][1] = new Color(214, 127, 133);
		array[36][0] = new Color(230, 89, 92);
		array[28][0] = new Color(151, 79, 80);
		array[28][1] = new Color(90, 139, 140);
		array[28][2] = new Color(192, 136, 70);
		array[28][3] = new Color(203, 185, 151);
		array[28][4] = new Color(73, 56, 41);
		array[28][5] = new Color(148, 159, 67);
		array[28][6] = new Color(138, 172, 67);
		array[28][7] = new Color(226, 122, 47);
		array[28][8] = new Color(198, 87, 93);
		for (int m = 0; m < array[653].Length; m++)
		{
			array[653][m] = array[28][m];
		}
		array[29][0] = new Color(175, 105, 128);
		array[51][0] = new Color(192, 202, 203);
		array[31][0] = new Color(141, 120, 168);
		array[31][1] = new Color(212, 105, 105);
		array[32][0] = new Color(151, 135, 183);
		array[42][0] = new Color(251, 235, 127);
		array[50][0] = new Color(170, 48, 114);
		array[85][0] = new Color(192, 192, 192);
		array[69][0] = new Color(190, 150, 92);
		array[77][0] = new Color(238, 85, 70);
		array[81][0] = new Color(245, 133, 191);
		array[78][0] = new Color(121, 110, 97);
		array[141][0] = new Color(192, 59, 59);
		array[129][0] = new Color(255, 117, 224);
		array[129][1] = new Color(255, 117, 224);
		array[126][0] = new Color(159, 209, 229);
		array[125][0] = new Color(141, 175, 255);
		array[103][0] = new Color(141, 98, 77);
		array[95][0] = new Color(255, 162, 31);
		array[92][0] = new Color(213, 229, 237);
		array[91][0] = new Color(13, 88, 130);
		array[215][0] = new Color(254, 121, 2);
		array[592][0] = new Color(254, 121, 2);
		array[316][0] = new Color(157, 176, 226);
		array[317][0] = new Color(118, 227, 129);
		array[318][0] = new Color(227, 118, 215);
		array[319][0] = new Color(96, 68, 48);
		array[320][0] = new Color(203, 185, 151);
		array[321][0] = new Color(96, 77, 64);
		array[574][0] = new Color(76, 57, 44);
		array[322][0] = new Color(198, 170, 104);
		array[635][0] = new Color(145, 120, 120);
		array[149][0] = new Color(220, 50, 50);
		array[149][1] = new Color(0, 220, 50);
		array[149][2] = new Color(50, 50, 220);
		array[133][0] = new Color(231, 53, 56);
		array[133][1] = new Color(192, 189, 221);
		array[134][0] = new Color(166, 187, 153);
		array[134][1] = new Color(241, 129, 249);
		array[102][0] = new Color(229, 212, 73);
		array[35][0] = new Color(226, 145, 30);
		array[34][0] = new Color(235, 166, 135);
		array[136][0] = new Color(213, 203, 204);
		array[231][0] = new Color(224, 194, 101);
		array[239][0] = new Color(224, 194, 101);
		array[240][0] = new Color(120, 85, 60);
		array[240][1] = new Color(99, 50, 30);
		array[240][2] = new Color(153, 153, 117);
		array[240][3] = new Color(112, 84, 56);
		array[240][4] = new Color(234, 231, 226);
		array[241][0] = new Color(77, 74, 72);
		array[244][0] = new Color(200, 245, 253);
		((Color)(ref color))._002Ector(99, 50, 30);
		array[242][0] = color;
		array[245][0] = color;
		array[246][0] = color;
		array[242][1] = new Color(185, 142, 97);
		array[247][0] = new Color(140, 150, 150);
		array[271][0] = new Color(107, 250, 255);
		array[270][0] = new Color(187, 255, 107);
		array[581][0] = new Color(255, 150, 150);
		array[660][0] = new Color(255, 150, 150);
		array[572][0] = new Color(255, 186, 212);
		array[572][1] = new Color(209, 201, 255);
		array[572][2] = new Color(200, 254, 255);
		array[572][3] = new Color(199, 255, 211);
		array[572][4] = new Color(180, 209, 255);
		array[572][5] = new Color(255, 220, 214);
		array[314][0] = new Color(181, 164, 125);
		array[324][0] = new Color(228, 213, 173);
		array[351][0] = new Color(31, 31, 31);
		array[424][0] = new Color(146, 155, 187);
		array[429][0] = new Color(220, 220, 220);
		array[445][0] = new Color(240, 240, 240);
		array[21][0] = new Color(174, 129, 92);
		array[21][1] = new Color(233, 207, 94);
		array[21][2] = new Color(137, 128, 200);
		array[21][3] = new Color(160, 160, 160);
		array[21][4] = new Color(106, 210, 255);
		array[441][0] = array[21][0];
		array[441][1] = array[21][1];
		array[441][2] = array[21][2];
		array[441][3] = array[21][3];
		array[441][4] = array[21][4];
		array[27][0] = new Color(54, 154, 54);
		array[27][1] = new Color(226, 196, 49);
		((Color)(ref color))._002Ector(246, 197, 26);
		array[82][0] = color;
		array[83][0] = color;
		array[84][0] = color;
		((Color)(ref color))._002Ector(76, 150, 216);
		array[82][1] = color;
		array[83][1] = color;
		array[84][1] = color;
		((Color)(ref color))._002Ector(185, 214, 42);
		array[82][2] = color;
		array[83][2] = color;
		array[84][2] = color;
		((Color)(ref color))._002Ector(167, 203, 37);
		array[82][3] = color;
		array[83][3] = color;
		array[84][3] = color;
		array[591][6] = color;
		((Color)(ref color))._002Ector(32, 168, 117);
		array[82][4] = color;
		array[83][4] = color;
		array[84][4] = color;
		((Color)(ref color))._002Ector(177, 69, 49);
		array[82][5] = color;
		array[83][5] = color;
		array[84][5] = color;
		((Color)(ref color))._002Ector(40, 152, 240);
		array[82][6] = color;
		array[83][6] = color;
		array[84][6] = color;
		array[591][1] = new Color(246, 197, 26);
		array[591][2] = new Color(76, 150, 216);
		array[591][3] = new Color(32, 168, 117);
		array[591][4] = new Color(40, 152, 240);
		array[591][5] = new Color(114, 81, 56);
		array[591][6] = new Color(141, 137, 223);
		array[591][7] = new Color(208, 80, 80);
		array[591][8] = new Color(177, 69, 49);
		array[165][0] = new Color(115, 173, 229);
		array[165][1] = new Color(100, 100, 100);
		array[165][2] = new Color(152, 152, 152);
		array[165][3] = new Color(227, 125, 22);
		array[178][0] = new Color(208, 94, 201);
		array[178][1] = new Color(233, 146, 69);
		array[178][2] = new Color(71, 146, 251);
		array[178][3] = new Color(60, 226, 133);
		array[178][4] = new Color(250, 30, 71);
		array[178][5] = new Color(166, 176, 204);
		array[178][6] = new Color(255, 217, 120);
		((Color)(ref color))._002Ector(99, 99, 99);
		array[185][0] = color;
		array[186][0] = color;
		array[187][0] = color;
		array[565][0] = color;
		array[579][0] = color;
		((Color)(ref color))._002Ector(114, 81, 56);
		array[185][1] = color;
		array[186][1] = color;
		array[187][1] = color;
		array[591][0] = color;
		((Color)(ref color))._002Ector(133, 133, 101);
		array[185][2] = color;
		array[186][2] = color;
		array[187][2] = color;
		((Color)(ref color))._002Ector(151, 200, 211);
		array[185][3] = color;
		array[186][3] = color;
		array[187][3] = color;
		((Color)(ref color))._002Ector(177, 183, 161);
		array[185][4] = color;
		array[186][4] = color;
		array[187][4] = color;
		((Color)(ref color))._002Ector(134, 114, 38);
		array[185][5] = color;
		array[186][5] = color;
		array[187][5] = color;
		((Color)(ref color))._002Ector(82, 62, 66);
		array[185][6] = color;
		array[186][6] = color;
		array[187][6] = color;
		((Color)(ref color))._002Ector(143, 117, 121);
		array[185][7] = color;
		array[186][7] = color;
		array[187][7] = color;
		((Color)(ref color))._002Ector(177, 92, 31);
		array[185][8] = color;
		array[186][8] = color;
		array[187][8] = color;
		((Color)(ref color))._002Ector(85, 73, 87);
		array[185][9] = color;
		array[186][9] = color;
		array[187][9] = color;
		((Color)(ref color))._002Ector(26, 196, 84);
		array[185][10] = color;
		array[186][10] = color;
		array[187][10] = color;
		Color[] array2 = array[647];
		for (int n = 0; n < array2.Length; n++)
		{
			array2[n] = array[186][n];
		}
		array2 = array[648];
		for (int num = 0; num < array2.Length; num++)
		{
			array2[num] = array[187][num];
		}
		array2 = array[650];
		for (int num12 = 0; num12 < array2.Length; num12++)
		{
			array2[num12] = array[185][num12];
		}
		array2 = array[649];
		for (int num22 = 0; num22 < array2.Length; num22++)
		{
			array2[num22] = array[185][num22];
		}
		array[227][0] = new Color(74, 197, 155);
		array[227][1] = new Color(54, 153, 88);
		array[227][2] = new Color(63, 126, 207);
		array[227][3] = new Color(240, 180, 4);
		array[227][4] = new Color(45, 68, 168);
		array[227][5] = new Color(61, 92, 0);
		array[227][6] = new Color(216, 112, 152);
		array[227][7] = new Color(200, 40, 24);
		array[227][8] = new Color(113, 45, 133);
		array[227][9] = new Color(235, 137, 2);
		array[227][10] = new Color(41, 152, 135);
		array[227][11] = new Color(198, 19, 78);
		array[373][0] = new Color(9, 61, 191);
		array[374][0] = new Color(253, 32, 3);
		array[375][0] = new Color(255, 156, 12);
		array[461][0] = new Color(212, 192, 100);
		array[461][1] = new Color(137, 132, 156);
		array[461][2] = new Color(148, 122, 112);
		array[461][3] = new Color(221, 201, 206);
		array[323][0] = new Color(182, 141, 86);
		array[325][0] = new Color(129, 125, 93);
		array[326][0] = new Color(9, 61, 191);
		array[327][0] = new Color(253, 32, 3);
		array[507][0] = new Color(5, 5, 5);
		array[508][0] = new Color(5, 5, 5);
		array[330][0] = new Color(226, 118, 76);
		array[331][0] = new Color(161, 172, 173);
		array[332][0] = new Color(204, 181, 72);
		array[333][0] = new Color(190, 190, 178);
		array[335][0] = new Color(217, 174, 137);
		array[336][0] = new Color(253, 62, 3);
		array[337][0] = new Color(144, 148, 144);
		array[338][0] = new Color(85, 255, 160);
		array[315][0] = new Color(235, 114, 80);
		array[641][0] = new Color(235, 125, 150);
		array[340][0] = new Color(96, 248, 2);
		array[341][0] = new Color(105, 74, 202);
		array[342][0] = new Color(29, 240, 255);
		array[343][0] = new Color(254, 202, 80);
		array[344][0] = new Color(131, 252, 245);
		array[345][0] = new Color(255, 156, 12);
		array[346][0] = new Color(149, 212, 89);
		array[642][0] = new Color(149, 212, 89);
		array[347][0] = new Color(236, 74, 79);
		array[348][0] = new Color(44, 26, 233);
		array[350][0] = new Color(55, 97, 155);
		array[352][0] = new Color(238, 97, 94);
		array[354][0] = new Color(141, 107, 89);
		array[355][0] = new Color(141, 107, 89);
		array[463][0] = new Color(155, 214, 240);
		array[491][0] = new Color(60, 20, 160);
		array[464][0] = new Color(233, 183, 128);
		array[465][0] = new Color(51, 84, 195);
		array[466][0] = new Color(205, 153, 73);
		array[356][0] = new Color(233, 203, 24);
		array[663][0] = new Color(24, 203, 233);
		array[357][0] = new Color(168, 178, 204);
		array[367][0] = new Color(168, 178, 204);
		array[561][0] = new Color(148, 158, 184);
		array[365][0] = new Color(146, 136, 205);
		array[366][0] = new Color(223, 232, 233);
		array[368][0] = new Color(50, 46, 104);
		array[369][0] = new Color(50, 46, 104);
		array[576][0] = new Color(30, 26, 84);
		array[370][0] = new Color(127, 116, 194);
		array[49][0] = new Color(89, 201, 255);
		array[372][0] = new Color(252, 128, 201);
		array[646][0] = new Color(108, 133, 140);
		array[371][0] = new Color(249, 101, 189);
		array[376][0] = new Color(160, 120, 92);
		array[378][0] = new Color(160, 120, 100);
		array[379][0] = new Color(251, 209, 240);
		array[382][0] = new Color(28, 216, 94);
		array[383][0] = new Color(221, 136, 144);
		array[384][0] = new Color(131, 206, 12);
		array[385][0] = new Color(87, 21, 144);
		array[386][0] = new Color(127, 92, 69);
		array[387][0] = new Color(127, 92, 69);
		array[388][0] = new Color(127, 92, 69);
		array[389][0] = new Color(127, 92, 69);
		array[390][0] = new Color(253, 32, 3);
		array[397][0] = new Color(212, 192, 100);
		array[396][0] = new Color(198, 124, 78);
		array[577][0] = new Color(178, 104, 58);
		array[398][0] = new Color(100, 82, 126);
		array[399][0] = new Color(77, 76, 66);
		array[400][0] = new Color(96, 68, 117);
		array[401][0] = new Color(68, 60, 51);
		array[402][0] = new Color(174, 168, 186);
		array[403][0] = new Color(205, 152, 186);
		array[404][0] = new Color(212, 148, 88);
		array[405][0] = new Color(140, 140, 140);
		array[406][0] = new Color(120, 120, 120);
		array[407][0] = new Color(255, 227, 132);
		array[411][0] = new Color(227, 46, 46);
		array[494][0] = new Color(227, 227, 227);
		array[421][0] = new Color(65, 75, 90);
		array[422][0] = new Color(65, 75, 90);
		array[425][0] = new Color(146, 155, 187);
		array[426][0] = new Color(168, 38, 47);
		array[430][0] = new Color(39, 168, 96);
		array[431][0] = new Color(39, 94, 168);
		array[432][0] = new Color(242, 221, 100);
		array[433][0] = new Color(224, 100, 242);
		array[434][0] = new Color(197, 193, 216);
		array[427][0] = new Color(183, 53, 62);
		array[435][0] = new Color(54, 183, 111);
		array[436][0] = new Color(54, 109, 183);
		array[437][0] = new Color(255, 236, 115);
		array[438][0] = new Color(239, 115, 255);
		array[439][0] = new Color(212, 208, 231);
		array[440][0] = new Color(238, 51, 53);
		array[440][1] = new Color(13, 107, 216);
		array[440][2] = new Color(33, 184, 115);
		array[440][3] = new Color(255, 221, 62);
		array[440][4] = new Color(165, 0, 236);
		array[440][5] = new Color(223, 230, 238);
		array[440][6] = new Color(207, 101, 0);
		array[419][0] = new Color(88, 95, 114);
		array[419][1] = new Color(214, 225, 236);
		array[419][2] = new Color(25, 131, 205);
		array[423][0] = new Color(245, 197, 1);
		array[423][1] = new Color(185, 0, 224);
		array[423][2] = new Color(58, 240, 111);
		array[423][3] = new Color(50, 107, 197);
		array[423][4] = new Color(253, 91, 3);
		array[423][5] = new Color(254, 194, 20);
		array[423][6] = new Color(174, 195, 215);
		array[420][0] = new Color(99, 255, 107);
		array[420][1] = new Color(99, 255, 107);
		array[420][4] = new Color(99, 255, 107);
		array[420][2] = new Color(218, 2, 5);
		array[420][3] = new Color(218, 2, 5);
		array[420][5] = new Color(218, 2, 5);
		array[476][0] = new Color(160, 160, 160);
		array[410][0] = new Color(75, 139, 166);
		array[480][0] = new Color(120, 50, 50);
		array[509][0] = new Color(50, 50, 60);
		array[657][0] = new Color(35, 205, 215);
		array[658][0] = new Color(200, 105, 230);
		array[412][0] = new Color(75, 139, 166);
		array[443][0] = new Color(144, 148, 144);
		array[442][0] = new Color(3, 144, 201);
		array[444][0] = new Color(191, 176, 124);
		array[446][0] = new Color(255, 66, 152);
		array[447][0] = new Color(179, 132, 255);
		array[448][0] = new Color(0, 206, 180);
		array[449][0] = new Color(91, 186, 240);
		array[450][0] = new Color(92, 240, 91);
		array[451][0] = new Color(240, 91, 147);
		array[452][0] = new Color(255, 150, 181);
		array[453][0] = new Color(179, 132, 255);
		array[453][1] = new Color(0, 206, 180);
		array[453][2] = new Color(255, 66, 152);
		array[454][0] = new Color(174, 16, 176);
		array[455][0] = new Color(48, 225, 110);
		array[456][0] = new Color(179, 132, 255);
		array[457][0] = new Color(150, 164, 206);
		array[457][1] = new Color(255, 132, 184);
		array[457][2] = new Color(74, 255, 232);
		array[457][3] = new Color(215, 159, 255);
		array[457][4] = new Color(229, 219, 234);
		array[458][0] = new Color(211, 198, 111);
		array[459][0] = new Color(190, 223, 232);
		array[460][0] = new Color(141, 163, 181);
		array[462][0] = new Color(231, 178, 28);
		array[467][0] = new Color(129, 56, 121);
		array[467][1] = new Color(255, 249, 59);
		array[467][2] = new Color(161, 67, 24);
		array[467][3] = new Color(89, 70, 72);
		array[467][4] = new Color(233, 207, 94);
		array[467][5] = new Color(254, 158, 35);
		array[467][6] = new Color(34, 221, 151);
		array[467][7] = new Color(249, 170, 236);
		array[467][8] = new Color(35, 200, 254);
		array[467][9] = new Color(190, 200, 200);
		array[467][10] = new Color(230, 170, 100);
		array[467][11] = new Color(165, 168, 26);
		for (int num23 = 0; num23 < 12; num23++)
		{
			array[468][num23] = array[467][num23];
		}
		array[472][0] = new Color(190, 160, 140);
		array[473][0] = new Color(85, 114, 123);
		array[474][0] = new Color(116, 94, 97);
		array[478][0] = new Color(108, 34, 35);
		array[479][0] = new Color(178, 114, 68);
		array[485][0] = new Color(198, 134, 88);
		array[492][0] = new Color(78, 193, 227);
		array[492][0] = new Color(78, 193, 227);
		array[493][0] = new Color(250, 249, 252);
		array[493][1] = new Color(240, 90, 90);
		array[493][2] = new Color(98, 230, 92);
		array[493][3] = new Color(95, 197, 238);
		array[493][4] = new Color(241, 221, 100);
		array[493][5] = new Color(213, 92, 237);
		array[494][0] = new Color(224, 219, 236);
		array[495][0] = new Color(253, 227, 215);
		array[496][0] = new Color(165, 159, 153);
		array[498][0] = new Color(202, 174, 165);
		array[499][0] = new Color(160, 187, 142);
		array[500][0] = new Color(254, 158, 35);
		array[501][0] = new Color(34, 221, 151);
		array[502][0] = new Color(249, 170, 236);
		array[503][0] = new Color(35, 200, 254);
		array[506][0] = new Color(61, 61, 61);
		array[510][0] = new Color(191, 142, 111);
		array[511][0] = new Color(187, 68, 74);
		array[520][0] = new Color(224, 219, 236);
		array[545][0] = new Color(255, 126, 145);
		array[530][0] = new Color(107, 182, 0);
		array[530][1] = new Color(23, 154, 209);
		array[530][2] = new Color(238, 97, 94);
		array[530][3] = new Color(113, 108, 205);
		array[546][0] = new Color(60, 60, 60);
		array[557][0] = new Color(60, 60, 60);
		array[547][0] = new Color(120, 110, 100);
		array[548][0] = new Color(120, 110, 100);
		array[562][0] = new Color(165, 168, 26);
		array[563][0] = new Color(165, 168, 26);
		array[571][0] = new Color(165, 168, 26);
		array[568][0] = new Color(248, 203, 233);
		array[569][0] = new Color(203, 248, 218);
		array[570][0] = new Color(160, 242, 255);
		array[597][0] = new Color(28, 216, 94);
		array[597][1] = new Color(183, 237, 20);
		array[597][2] = new Color(185, 83, 200);
		array[597][3] = new Color(131, 128, 168);
		array[597][4] = new Color(38, 142, 214);
		array[597][5] = new Color(229, 154, 9);
		array[597][6] = new Color(142, 227, 234);
		array[597][7] = new Color(98, 111, 223);
		array[597][8] = new Color(241, 233, 158);
		array[617][0] = new Color(233, 207, 94);
		Color color4 = default(Color);
		((Color)(ref color4))._002Ector(250, 100, 50);
		array[548][1] = color4;
		array[613][0] = color4;
		array[614][0] = color4;
		array[623][0] = new Color(220, 210, 245);
		array[661][0] = new Color(141, 137, 223);
		array[662][0] = new Color(208, 80, 80);
		array[666][0] = new Color(115, 60, 40);
		array[667][0] = new Color(247, 228, 254);
		Color[] array3 = (Color[])(object)new Color[4]
		{
			new Color(9, 61, 191),
			new Color(253, 32, 3),
			new Color(254, 194, 20),
			new Color(161, 127, 255)
		};
		Color[][] array4 = new Color[WallID.Count][];
		for (int num24 = 0; num24 < WallID.Count; num24++)
		{
			array4[num24] = (Color[])(object)new Color[2];
		}
		array4[158][0] = new Color(107, 49, 154);
		array4[163][0] = new Color(154, 148, 49);
		array4[162][0] = new Color(49, 49, 154);
		array4[160][0] = new Color(49, 154, 68);
		array4[161][0] = new Color(154, 49, 77);
		array4[159][0] = new Color(85, 89, 118);
		array4[157][0] = new Color(154, 83, 49);
		array4[154][0] = new Color(221, 79, 255);
		array4[166][0] = new Color(250, 255, 79);
		array4[165][0] = new Color(79, 102, 255);
		array4[156][0] = new Color(79, 255, 89);
		array4[164][0] = new Color(255, 79, 79);
		array4[155][0] = new Color(240, 240, 247);
		array4[153][0] = new Color(255, 145, 79);
		array4[169][0] = new Color(5, 5, 5);
		array4[224][0] = new Color(57, 55, 52);
		array4[323][0] = new Color(55, 25, 33);
		array4[324][0] = new Color(60, 55, 145);
		array4[325][0] = new Color(10, 5, 50);
		array4[326][0] = new Color(30, 105, 75);
		array4[327][0] = new Color(5, 45, 55);
		array4[328][0] = new Color(20, 25, 35);
		array4[329][0] = new Color(15, 10, 50);
		array4[330][0] = new Color(153, 164, 187);
		array4[225][0] = new Color(68, 68, 68);
		array4[226][0] = new Color(148, 138, 74);
		array4[227][0] = new Color(95, 137, 191);
		array4[170][0] = new Color(59, 39, 22);
		array4[171][0] = new Color(59, 39, 22);
		((Color)(ref color))._002Ector(52, 52, 52);
		array4[1][0] = color;
		array4[53][0] = color;
		array4[52][0] = color;
		array4[51][0] = color;
		array4[50][0] = color;
		array4[49][0] = color;
		array4[48][0] = color;
		array4[44][0] = color;
		array4[346][0] = color;
		array4[5][0] = color;
		((Color)(ref color))._002Ector(88, 61, 46);
		array4[2][0] = color;
		array4[16][0] = color;
		array4[59][0] = color;
		array4[3][0] = new Color(61, 58, 78);
		array4[4][0] = new Color(73, 51, 36);
		array4[6][0] = new Color(91, 30, 30);
		((Color)(ref color))._002Ector(27, 31, 42);
		array4[7][0] = color;
		array4[17][0] = color;
		array4[331][0] = color;
		((Color)(ref color))._002Ector(32, 40, 45);
		array4[94][0] = color;
		array4[100][0] = color;
		((Color)(ref color))._002Ector(44, 41, 50);
		array4[95][0] = color;
		array4[101][0] = color;
		((Color)(ref color))._002Ector(31, 39, 26);
		array4[8][0] = color;
		array4[18][0] = color;
		array4[332][0] = color;
		((Color)(ref color))._002Ector(36, 45, 44);
		array4[98][0] = color;
		array4[104][0] = color;
		((Color)(ref color))._002Ector(38, 49, 50);
		array4[99][0] = color;
		array4[105][0] = color;
		((Color)(ref color))._002Ector(41, 28, 36);
		array4[9][0] = color;
		array4[19][0] = color;
		array4[333][0] = color;
		((Color)(ref color))._002Ector(72, 50, 77);
		array4[96][0] = color;
		array4[102][0] = color;
		((Color)(ref color))._002Ector(78, 50, 69);
		array4[97][0] = color;
		array4[103][0] = color;
		array4[10][0] = new Color(74, 62, 12);
		array4[334][0] = new Color(74, 62, 12);
		array4[11][0] = new Color(46, 56, 59);
		array4[335][0] = new Color(46, 56, 59);
		array4[12][0] = new Color(75, 32, 11);
		array4[336][0] = new Color(75, 32, 11);
		array4[13][0] = new Color(67, 37, 37);
		array4[338][0] = new Color(67, 37, 37);
		((Color)(ref color))._002Ector(15, 15, 15);
		array4[14][0] = color;
		array4[337][0] = color;
		array4[20][0] = color;
		array4[15][0] = new Color(52, 43, 45);
		array4[22][0] = new Color(113, 99, 99);
		array4[23][0] = new Color(38, 38, 43);
		array4[24][0] = new Color(53, 39, 41);
		array4[25][0] = new Color(11, 35, 62);
		array4[339][0] = new Color(11, 35, 62);
		array4[26][0] = new Color(21, 63, 70);
		array4[340][0] = new Color(21, 63, 70);
		array4[27][0] = new Color(88, 61, 46);
		array4[27][1] = new Color(52, 52, 52);
		array4[28][0] = new Color(81, 84, 101);
		array4[29][0] = new Color(88, 23, 23);
		array4[30][0] = new Color(28, 88, 23);
		array4[31][0] = new Color(78, 87, 99);
		((Color)(ref color))._002Ector(69, 67, 41);
		array4[34][0] = color;
		array4[37][0] = color;
		array4[32][0] = new Color(86, 17, 40);
		array4[33][0] = new Color(49, 47, 83);
		array4[35][0] = new Color(51, 51, 70);
		array4[36][0] = new Color(87, 59, 55);
		array4[38][0] = new Color(49, 57, 49);
		array4[39][0] = new Color(78, 79, 73);
		array4[45][0] = new Color(60, 59, 51);
		array4[46][0] = new Color(48, 57, 47);
		array4[47][0] = new Color(71, 77, 85);
		array4[40][0] = new Color(85, 102, 103);
		array4[41][0] = new Color(52, 50, 62);
		array4[42][0] = new Color(71, 42, 44);
		array4[43][0] = new Color(73, 66, 50);
		array4[54][0] = new Color(40, 56, 50);
		array4[55][0] = new Color(49, 48, 36);
		array4[56][0] = new Color(43, 33, 32);
		array4[57][0] = new Color(31, 40, 49);
		array4[58][0] = new Color(48, 35, 52);
		array4[60][0] = new Color(1, 52, 20);
		array4[61][0] = new Color(55, 39, 26);
		array4[62][0] = new Color(39, 33, 26);
		array4[69][0] = new Color(43, 42, 68);
		array4[70][0] = new Color(30, 70, 80);
		array4[341][0] = new Color(100, 40, 1);
		array4[342][0] = new Color(92, 30, 72);
		array4[343][0] = new Color(42, 81, 1);
		array4[344][0] = new Color(1, 81, 109);
		array4[345][0] = new Color(56, 22, 97);
		((Color)(ref color))._002Ector(30, 80, 48);
		array4[63][0] = color;
		array4[65][0] = color;
		array4[66][0] = color;
		array4[68][0] = color;
		((Color)(ref color))._002Ector(53, 80, 30);
		array4[64][0] = color;
		array4[67][0] = color;
		array4[78][0] = new Color(63, 39, 26);
		array4[244][0] = new Color(63, 39, 26);
		array4[71][0] = new Color(78, 105, 135);
		array4[72][0] = new Color(52, 84, 12);
		array4[73][0] = new Color(190, 204, 223);
		((Color)(ref color))._002Ector(64, 62, 80);
		array4[74][0] = color;
		array4[80][0] = color;
		array4[75][0] = new Color(65, 65, 35);
		array4[76][0] = new Color(20, 46, 104);
		array4[77][0] = new Color(61, 13, 16);
		array4[79][0] = new Color(51, 47, 96);
		array4[81][0] = new Color(101, 51, 51);
		array4[82][0] = new Color(77, 64, 34);
		array4[83][0] = new Color(62, 38, 41);
		array4[234][0] = new Color(60, 36, 39);
		array4[84][0] = new Color(48, 78, 93);
		array4[85][0] = new Color(54, 63, 69);
		((Color)(ref color))._002Ector(138, 73, 38);
		array4[86][0] = color;
		array4[108][0] = color;
		((Color)(ref color))._002Ector(50, 15, 8);
		array4[87][0] = color;
		array4[112][0] = color;
		array4[109][0] = new Color(94, 25, 17);
		array4[110][0] = new Color(125, 36, 122);
		array4[111][0] = new Color(51, 35, 27);
		array4[113][0] = new Color(135, 58, 0);
		array4[114][0] = new Color(65, 52, 15);
		array4[115][0] = new Color(39, 42, 51);
		array4[116][0] = new Color(89, 26, 27);
		array4[117][0] = new Color(126, 123, 115);
		array4[118][0] = new Color(8, 50, 19);
		array4[119][0] = new Color(95, 21, 24);
		array4[120][0] = new Color(17, 31, 65);
		array4[121][0] = new Color(192, 173, 143);
		array4[122][0] = new Color(114, 114, 131);
		array4[123][0] = new Color(136, 119, 7);
		array4[124][0] = new Color(8, 72, 3);
		array4[125][0] = new Color(117, 132, 82);
		array4[126][0] = new Color(100, 102, 114);
		array4[127][0] = new Color(30, 118, 226);
		array4[128][0] = new Color(93, 6, 102);
		array4[129][0] = new Color(64, 40, 169);
		array4[130][0] = new Color(39, 34, 180);
		array4[131][0] = new Color(87, 94, 125);
		array4[132][0] = new Color(6, 6, 6);
		array4[133][0] = new Color(69, 72, 186);
		array4[134][0] = new Color(130, 62, 16);
		array4[135][0] = new Color(22, 123, 163);
		array4[136][0] = new Color(40, 86, 151);
		array4[137][0] = new Color(183, 75, 15);
		array4[138][0] = new Color(83, 80, 100);
		array4[139][0] = new Color(115, 65, 68);
		array4[140][0] = new Color(119, 108, 81);
		array4[141][0] = new Color(59, 67, 71);
		array4[142][0] = new Color(222, 216, 202);
		array4[143][0] = new Color(90, 112, 105);
		array4[144][0] = new Color(62, 28, 87);
		array4[146][0] = new Color(120, 59, 19);
		array4[147][0] = new Color(59, 59, 59);
		array4[148][0] = new Color(229, 218, 161);
		array4[149][0] = new Color(73, 59, 50);
		array4[151][0] = new Color(102, 75, 34);
		array4[167][0] = new Color(70, 68, 51);
		Color color5 = default(Color);
		((Color)(ref color5))._002Ector(125, 100, 100);
		array4[316][0] = color5;
		array4[317][0] = color5;
		array4[172][0] = new Color(163, 96, 0);
		array4[242][0] = new Color(5, 5, 5);
		array4[243][0] = new Color(5, 5, 5);
		array4[173][0] = new Color(94, 163, 46);
		array4[174][0] = new Color(117, 32, 59);
		array4[175][0] = new Color(20, 11, 203);
		array4[176][0] = new Color(74, 69, 88);
		array4[177][0] = new Color(60, 30, 30);
		array4[183][0] = new Color(111, 117, 135);
		array4[179][0] = new Color(111, 117, 135);
		array4[178][0] = new Color(111, 117, 135);
		array4[184][0] = new Color(25, 23, 54);
		array4[181][0] = new Color(25, 23, 54);
		array4[180][0] = new Color(25, 23, 54);
		array4[182][0] = new Color(74, 71, 129);
		array4[185][0] = new Color(52, 52, 52);
		array4[186][0] = new Color(38, 9, 66);
		array4[216][0] = new Color(158, 100, 64);
		array4[217][0] = new Color(62, 45, 75);
		array4[218][0] = new Color(57, 14, 12);
		array4[219][0] = new Color(96, 72, 133);
		array4[187][0] = new Color(149, 80, 51);
		array4[235][0] = new Color(140, 75, 48);
		array4[220][0] = new Color(67, 55, 80);
		array4[221][0] = new Color(64, 37, 29);
		array4[222][0] = new Color(70, 51, 91);
		array4[188][0] = new Color(82, 63, 80);
		array4[189][0] = new Color(65, 61, 77);
		array4[190][0] = new Color(64, 65, 92);
		array4[191][0] = new Color(76, 53, 84);
		array4[192][0] = new Color(144, 67, 52);
		array4[193][0] = new Color(149, 48, 48);
		array4[194][0] = new Color(111, 32, 36);
		array4[195][0] = new Color(147, 48, 55);
		array4[196][0] = new Color(97, 67, 51);
		array4[197][0] = new Color(112, 80, 62);
		array4[198][0] = new Color(88, 61, 46);
		array4[199][0] = new Color(127, 94, 76);
		array4[200][0] = new Color(143, 50, 123);
		array4[201][0] = new Color(136, 120, 131);
		array4[202][0] = new Color(219, 92, 143);
		array4[203][0] = new Color(113, 64, 150);
		array4[204][0] = new Color(74, 67, 60);
		array4[205][0] = new Color(60, 78, 59);
		array4[206][0] = new Color(0, 54, 21);
		array4[207][0] = new Color(74, 97, 72);
		array4[208][0] = new Color(40, 37, 35);
		array4[209][0] = new Color(77, 63, 66);
		array4[210][0] = new Color(111, 6, 6);
		array4[211][0] = new Color(88, 67, 59);
		array4[212][0] = new Color(88, 87, 80);
		array4[213][0] = new Color(71, 71, 67);
		array4[214][0] = new Color(76, 52, 60);
		array4[215][0] = new Color(89, 48, 59);
		array4[223][0] = new Color(51, 18, 4);
		array4[228][0] = new Color(160, 2, 75);
		array4[229][0] = new Color(100, 55, 164);
		array4[230][0] = new Color(0, 117, 101);
		array4[236][0] = new Color(127, 49, 44);
		array4[231][0] = new Color(110, 90, 78);
		array4[232][0] = new Color(47, 69, 75);
		array4[233][0] = new Color(91, 67, 70);
		array4[237][0] = new Color(200, 44, 18);
		array4[238][0] = new Color(24, 93, 66);
		array4[239][0] = new Color(160, 87, 234);
		array4[240][0] = new Color(6, 106, 255);
		array4[245][0] = new Color(102, 102, 102);
		array4[315][0] = new Color(181, 230, 29);
		array4[246][0] = new Color(61, 58, 78);
		array4[247][0] = new Color(52, 43, 45);
		array4[248][0] = new Color(81, 84, 101);
		array4[249][0] = new Color(85, 102, 103);
		array4[250][0] = new Color(52, 52, 52);
		array4[251][0] = new Color(52, 52, 52);
		array4[252][0] = new Color(52, 52, 52);
		array4[253][0] = new Color(52, 52, 52);
		array4[254][0] = new Color(52, 52, 52);
		array4[255][0] = new Color(52, 52, 52);
		array4[314][0] = new Color(52, 52, 52);
		array4[256][0] = new Color(40, 56, 50);
		array4[257][0] = new Color(49, 48, 36);
		array4[258][0] = new Color(43, 33, 32);
		array4[259][0] = new Color(31, 40, 49);
		array4[260][0] = new Color(48, 35, 52);
		array4[261][0] = new Color(88, 61, 46);
		array4[262][0] = new Color(55, 39, 26);
		array4[263][0] = new Color(39, 33, 26);
		array4[264][0] = new Color(43, 42, 68);
		array4[265][0] = new Color(30, 70, 80);
		array4[266][0] = new Color(78, 105, 135);
		array4[267][0] = new Color(51, 47, 96);
		array4[268][0] = new Color(101, 51, 51);
		array4[269][0] = new Color(62, 38, 41);
		array4[270][0] = new Color(59, 39, 22);
		array4[271][0] = new Color(59, 39, 22);
		array4[272][0] = new Color(111, 117, 135);
		array4[273][0] = new Color(25, 23, 54);
		array4[274][0] = new Color(52, 52, 52);
		array4[275][0] = new Color(149, 80, 51);
		array4[276][0] = new Color(82, 63, 80);
		array4[277][0] = new Color(65, 61, 77);
		array4[278][0] = new Color(64, 65, 92);
		array4[279][0] = new Color(76, 53, 84);
		array4[280][0] = new Color(144, 67, 52);
		array4[281][0] = new Color(149, 48, 48);
		array4[282][0] = new Color(111, 32, 36);
		array4[283][0] = new Color(147, 48, 55);
		array4[284][0] = new Color(97, 67, 51);
		array4[285][0] = new Color(112, 80, 62);
		array4[286][0] = new Color(88, 61, 46);
		array4[287][0] = new Color(127, 94, 76);
		array4[288][0] = new Color(143, 50, 123);
		array4[289][0] = new Color(136, 120, 131);
		array4[290][0] = new Color(219, 92, 143);
		array4[291][0] = new Color(113, 64, 150);
		array4[292][0] = new Color(74, 67, 60);
		array4[293][0] = new Color(60, 78, 59);
		array4[294][0] = new Color(0, 54, 21);
		array4[295][0] = new Color(74, 97, 72);
		array4[296][0] = new Color(40, 37, 35);
		array4[297][0] = new Color(77, 63, 66);
		array4[298][0] = new Color(111, 6, 6);
		array4[299][0] = new Color(88, 67, 59);
		array4[300][0] = new Color(88, 87, 80);
		array4[301][0] = new Color(71, 71, 67);
		array4[302][0] = new Color(76, 52, 60);
		array4[303][0] = new Color(89, 48, 59);
		array4[304][0] = new Color(158, 100, 64);
		array4[305][0] = new Color(62, 45, 75);
		array4[306][0] = new Color(57, 14, 12);
		array4[307][0] = new Color(96, 72, 133);
		array4[308][0] = new Color(67, 55, 80);
		array4[309][0] = new Color(64, 37, 29);
		array4[310][0] = new Color(70, 51, 91);
		array4[311][0] = new Color(51, 18, 4);
		array4[312][0] = new Color(78, 110, 51);
		array4[313][0] = new Color(78, 110, 51);
		array4[319][0] = new Color(105, 51, 108);
		array4[320][0] = new Color(75, 30, 15);
		array4[321][0] = new Color(91, 108, 130);
		array4[322][0] = new Color(91, 108, 130);
		Color[] array5 = (Color[])(object)new Color[256];
		Color color6 = default(Color);
		((Color)(ref color6))._002Ector(50, 40, 255);
		Color color7 = default(Color);
		((Color)(ref color7))._002Ector(145, 185, 255);
		for (int num25 = 0; num25 < array5.Length; num25++)
		{
			float num26 = (float)num25 / (float)array5.Length;
			float num27 = 1f - num26;
			array5[num25] = new Color((int)(byte)((float)(int)((Color)(ref color6)).R * num27 + (float)(int)((Color)(ref color7)).R * num26), (int)(byte)((float)(int)((Color)(ref color6)).G * num27 + (float)(int)((Color)(ref color7)).G * num26), (int)(byte)((float)(int)((Color)(ref color6)).B * num27 + (float)(int)((Color)(ref color7)).B * num26));
		}
		Color[] array6 = (Color[])(object)new Color[256];
		Color color8 = default(Color);
		((Color)(ref color8))._002Ector(88, 61, 46);
		Color color9 = default(Color);
		((Color)(ref color9))._002Ector(37, 78, 123);
		for (int num28 = 0; num28 < array6.Length; num28++)
		{
			float num2 = (float)num28 / 255f;
			float num3 = 1f - num2;
			array6[num28] = new Color((int)(byte)((float)(int)((Color)(ref color8)).R * num3 + (float)(int)((Color)(ref color9)).R * num2), (int)(byte)((float)(int)((Color)(ref color8)).G * num3 + (float)(int)((Color)(ref color9)).G * num2), (int)(byte)((float)(int)((Color)(ref color8)).B * num3 + (float)(int)((Color)(ref color9)).B * num2));
		}
		Color[] array7 = (Color[])(object)new Color[256];
		Color color10 = default(Color);
		((Color)(ref color10))._002Ector(74, 67, 60);
		((Color)(ref color9))._002Ector(53, 70, 97);
		for (int num4 = 0; num4 < array7.Length; num4++)
		{
			float num5 = (float)num4 / 255f;
			float num6 = 1f - num5;
			array7[num4] = new Color((int)(byte)((float)(int)((Color)(ref color10)).R * num6 + (float)(int)((Color)(ref color9)).R * num5), (int)(byte)((float)(int)((Color)(ref color10)).G * num6 + (float)(int)((Color)(ref color9)).G * num5), (int)(byte)((float)(int)((Color)(ref color10)).B * num6 + (float)(int)((Color)(ref color9)).B * num5));
		}
		Color color2 = default(Color);
		((Color)(ref color2))._002Ector(50, 44, 38);
		int num7 = 0;
		tileOptionCounts = new int[TileID.Count];
		for (int num8 = 0; num8 < TileID.Count; num8++)
		{
			Color[] array8 = array[num8];
			int num9;
			for (num9 = 0; num9 < 12 && !(array8[num9] == Color.Transparent); num9++)
			{
			}
			tileOptionCounts[num8] = num9;
			num7 += num9;
		}
		wallOptionCounts = new int[WallID.Count];
		for (int num10 = 0; num10 < WallID.Count; num10++)
		{
			Color[] array9 = array4[num10];
			int num11;
			for (num11 = 0; num11 < 2 && !(array9[num11] == Color.Transparent); num11++)
			{
			}
			wallOptionCounts[num10] = num11;
			num7 += num11;
		}
		num7 += 774;
		colorLookup = (Color[])(object)new Color[num7];
		colorLookup[0] = Color.Transparent;
		ushort num13 = (tilePosition = 1);
		tileLookup = new ushort[TileID.Count];
		for (int num14 = 0; num14 < TileID.Count; num14++)
		{
			if (tileOptionCounts[num14] > 0)
			{
				_ = array[num14];
				tileLookup[num14] = num13;
				for (int num15 = 0; num15 < tileOptionCounts[num14]; num15++)
				{
					colorLookup[num13] = array[num14][num15];
					num13++;
				}
			}
			else
			{
				tileLookup[num14] = 0;
			}
		}
		wallPosition = num13;
		wallLookup = new ushort[WallID.Count];
		wallRangeStart = num13;
		for (int num16 = 0; num16 < WallID.Count; num16++)
		{
			if (wallOptionCounts[num16] > 0)
			{
				_ = array4[num16];
				wallLookup[num16] = num13;
				for (int num17 = 0; num17 < wallOptionCounts[num16]; num17++)
				{
					colorLookup[num13] = array4[num16][num17];
					num13++;
				}
			}
			else
			{
				wallLookup[num16] = 0;
			}
		}
		wallRangeEnd = num13;
		liquidPosition = num13;
		for (int num18 = 0; num18 < 4; num18++)
		{
			colorLookup[num13] = array3[num18];
			num13++;
		}
		skyPosition = num13;
		for (int num19 = 0; num19 < 256; num19++)
		{
			colorLookup[num13] = array5[num19];
			num13++;
		}
		dirtPosition = num13;
		for (int num20 = 0; num20 < 256; num20++)
		{
			colorLookup[num13] = array6[num20];
			num13++;
		}
		rockPosition = num13;
		for (int num21 = 0; num21 < 256; num21++)
		{
			colorLookup[num13] = array7[num21];
			num13++;
		}
		hellPosition = num13;
		colorLookup[num13] = color2;
		modPosition = (ushort)(num13 + 1);
		snowTypes = new ushort[6];
		snowTypes[0] = tileLookup[147];
		snowTypes[1] = tileLookup[161];
		snowTypes[2] = tileLookup[162];
		snowTypes[3] = tileLookup[163];
		snowTypes[4] = tileLookup[164];
		snowTypes[5] = tileLookup[200];
		Lang.BuildMapAtlas();
	}

	public static void ResetMapData()
	{
		numUpdateTile = 0;
	}

	public static bool HasOption(int tileType, int option)
	{
		return option < tileOptionCounts[tileType];
	}

	public static int TileToLookup(int tileType, int option)
	{
		return tileLookup[tileType] + option;
	}

	public static int LookupCount()
	{
		return colorLookup.Length;
	}

	private static void MapColor(ushort type, ref Color oldColor, byte colorType)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		Color color = WorldGen.paintColor(colorType);
		float num = (float)(int)((Color)(ref oldColor)).R / 255f;
		float num2 = (float)(int)((Color)(ref oldColor)).G / 255f;
		float num3 = (float)(int)((Color)(ref oldColor)).B / 255f;
		if (num2 > num)
		{
			float num6 = num;
			num = num2;
			num2 = num6;
		}
		if (num3 > num)
		{
			float num7 = num;
			num = num3;
			num3 = num7;
		}
		switch (colorType)
		{
		case 29:
		{
			float num5 = num3 * 0.3f;
			((Color)(ref oldColor)).R = (byte)((float)(int)((Color)(ref color)).R * num5);
			((Color)(ref oldColor)).G = (byte)((float)(int)((Color)(ref color)).G * num5);
			((Color)(ref oldColor)).B = (byte)((float)(int)((Color)(ref color)).B * num5);
			break;
		}
		case 30:
			if (type >= wallRangeStart && type <= wallRangeEnd)
			{
				((Color)(ref oldColor)).R = (byte)((float)(255 - ((Color)(ref oldColor)).R) * 0.5f);
				((Color)(ref oldColor)).G = (byte)((float)(255 - ((Color)(ref oldColor)).G) * 0.5f);
				((Color)(ref oldColor)).B = (byte)((float)(255 - ((Color)(ref oldColor)).B) * 0.5f);
			}
			else
			{
				((Color)(ref oldColor)).R = (byte)(255 - ((Color)(ref oldColor)).R);
				((Color)(ref oldColor)).G = (byte)(255 - ((Color)(ref oldColor)).G);
				((Color)(ref oldColor)).B = (byte)(255 - ((Color)(ref oldColor)).B);
			}
			break;
		default:
		{
			float num4 = num;
			((Color)(ref oldColor)).R = (byte)((float)(int)((Color)(ref color)).R * num4);
			((Color)(ref oldColor)).G = (byte)((float)(int)((Color)(ref color)).G * num4);
			((Color)(ref oldColor)).B = (byte)((float)(int)((Color)(ref color)).B * num4);
			break;
		}
		}
	}

	public static Color GetMapTileXnaColor(ref MapTile tile)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		Color oldColor = colorLookup[tile.Type];
		byte color = tile.Color;
		if (color > 0)
		{
			MapColor(tile.Type, ref oldColor, color);
		}
		if (tile.Light == byte.MaxValue)
		{
			return oldColor;
		}
		float num = (float)(int)tile.Light / 255f;
		((Color)(ref oldColor)).R = (byte)((float)(int)((Color)(ref oldColor)).R * num);
		((Color)(ref oldColor)).G = (byte)((float)(int)((Color)(ref oldColor)).G * num);
		((Color)(ref oldColor)).B = (byte)((float)(int)((Color)(ref oldColor)).B * num);
		return oldColor;
	}

	public static MapTile CreateMapTile(int i, int j, byte Light)
	{
		Tile tile = Main.tile[i, j];
		if (tile == null)
		{
			return default(MapTile);
		}
		int num = 0;
		int num4 = Light;
		_ = Main.Map[i, j];
		int num5 = 0;
		int baseOption = 0;
		if (tile.active())
		{
			int num6 = tile.type;
			num5 = tileLookup[num6];
			bool flag = tile.invisibleBlock();
			if (tile.fullbrightBlock() && !flag)
			{
				num4 = 255;
			}
			if (flag)
			{
				num5 = 0;
			}
			else if (num6 == 5)
			{
				if (WorldGen.IsThisAMushroomTree(i, j))
				{
					baseOption = 1;
				}
				num = tile.color();
			}
			else
			{
				switch (num6)
				{
				case 51:
					if ((i + j) % 2 == 0)
					{
						num5 = 0;
					}
					break;
				case 19:
					if (tile.frameY == 864)
					{
						num5 = 0;
					}
					break;
				case 184:
					if (tile.frameX / 22 == 10)
					{
						num6 = 627;
						num5 = tileLookup[num6];
					}
					break;
				}
				if (num5 != 0)
				{
					GetTileBaseOption(i, j, num6, tile, ref baseOption);
					num = ((num6 != 160) ? tile.color() : 0);
				}
			}
		}
		if (num5 == 0)
		{
			bool flag2 = tile.invisibleWall();
			if (tile.wall > 0 && tile.fullbrightWall() && !flag2)
			{
				num4 = 255;
			}
			if (tile.liquid > 32)
			{
				int num7 = tile.liquidType();
				num5 = liquidPosition + num7;
			}
			else if (!tile.invisibleWall() && tile.wall > 0 && tile.wall < WallLoader.WallCount)
			{
				int wall = tile.wall;
				num5 = wallLookup[wall];
				num = tile.wallColor();
				switch (wall)
				{
				case 21:
				case 88:
				case 89:
				case 90:
				case 91:
				case 92:
				case 93:
				case 168:
				case 241:
					num = 0;
					break;
				case 27:
					baseOption = i % 2;
					break;
				default:
					baseOption = 0;
					break;
				}
			}
		}
		if (num5 == 0)
		{
			if ((double)j < Main.worldSurface)
			{
				if (Main.remixWorld)
				{
					num4 = 5;
					num5 = 100;
				}
				else
				{
					int num8 = (byte)(255.0 * ((double)j / Main.worldSurface));
					num5 = skyPosition + num8;
					num4 = 255;
					num = 0;
				}
			}
			else if (j < Main.UnderworldLayer)
			{
				num = 0;
				byte b = 0;
				float num9 = Main.screenPosition.X / 16f - 5f;
				float num10 = (Main.screenPosition.X + (float)Main.screenWidth) / 16f + 5f;
				float num11 = Main.screenPosition.Y / 16f - 5f;
				float num2 = (Main.screenPosition.Y + (float)Main.screenHeight) / 16f + 5f;
				if (((float)i < num9 || (float)i > num10 || (float)j < num11 || (float)j > num2) && i > 40 && i < Main.maxTilesX - 40 && j > 40 && j < Main.maxTilesY - 40)
				{
					for (int k = i - 36; k <= i + 30; k += 10)
					{
						for (int l = j - 36; l <= j + 30; l += 10)
						{
							int type = Main.Map[k, l].Type;
							for (int m = 0; m < snowTypes.Length; m++)
							{
								if (snowTypes[m] == type)
								{
									b = byte.MaxValue;
									k = i + 31;
									l = j + 31;
									break;
								}
							}
						}
					}
				}
				else
				{
					float num3 = (float)Main.SceneMetrics.SnowTileCount / (float)SceneMetrics.SnowTileMax;
					num3 *= 255f;
					if (num3 > 255f)
					{
						num3 = 255f;
					}
					b = (byte)num3;
				}
				num5 = ((!((double)j < Main.rockLayer)) ? (rockPosition + b) : (dirtPosition + b));
			}
			else
			{
				num5 = hellPosition;
			}
		}
		ushort mapType = (ushort)(num5 + baseOption);
		MapLoader.ModMapOption(ref mapType, i, j);
		return MapTile.Create(mapType, (byte)num4, (byte)num);
	}

	public static void GetTileBaseOption(int x, int y, int tileType, Tile tileCache, ref int baseOption)
	{
		switch (tileType)
		{
		case 89:
			switch (tileCache.frameX / 54)
			{
			case 0:
			case 21:
			case 23:
				baseOption = 0;
				break;
			case 43:
				baseOption = 2;
				break;
			default:
				baseOption = 1;
				break;
			}
			break;
		case 160:
		case 627:
		case 628:
		case 692:
			baseOption = (x + y) % 9;
			break;
		case 461:
			if (Main.player[Main.myPlayer].ZoneCorrupt)
			{
				baseOption = 1;
			}
			else if (Main.player[Main.myPlayer].ZoneCrimson)
			{
				baseOption = 2;
			}
			else if (Main.player[Main.myPlayer].ZoneHallow)
			{
				baseOption = 3;
			}
			break;
		case 80:
		{
			WorldGen.GetCactusType(x, y, tileCache.frameX, tileCache.frameY, out var evil, out var good, out var crimson);
			if (evil)
			{
				baseOption = 1;
			}
			else if (good)
			{
				baseOption = 2;
			}
			else if (crimson)
			{
				baseOption = 3;
			}
			else
			{
				baseOption = 0;
			}
			break;
		}
		case 529:
		{
			int num23 = y + 1;
			WorldGen.GetBiomeInfluence(x, x, num23, num23, out var corruptCount, out var crimsonCount, out var hallowedCount);
			int num24 = corruptCount;
			if (num24 < crimsonCount)
			{
				num24 = crimsonCount;
			}
			if (num24 < hallowedCount)
			{
				num24 = hallowedCount;
			}
			int num25 = 0;
			num25 = ((corruptCount != 0 || crimsonCount != 0 || hallowedCount != 0) ? ((hallowedCount == num24) ? 2 : ((crimsonCount != num24) ? 4 : 3)) : ((x < WorldGen.beachDistance || x > Main.maxTilesX - WorldGen.beachDistance) ? 1 : 0));
			baseOption = num25;
			break;
		}
		case 530:
		{
			int num28 = y - tileCache.frameY % 36 / 18 + 2;
			int num30 = x - tileCache.frameX % 54 / 18;
			WorldGen.GetBiomeInfluence(num30, num30 + 3, num28, num28, out var corruptCount2, out var crimsonCount2, out var hallowedCount2);
			int num18 = corruptCount2;
			if (num18 < crimsonCount2)
			{
				num18 = crimsonCount2;
			}
			if (num18 < hallowedCount2)
			{
				num18 = hallowedCount2;
			}
			int num19 = 0;
			num19 = ((corruptCount2 != 0 || crimsonCount2 != 0 || hallowedCount2 != 0) ? ((hallowedCount2 == num18) ? 1 : ((crimsonCount2 != num18) ? 3 : 2)) : 0);
			baseOption = num19;
			break;
		}
		case 19:
		{
			int num29 = tileCache.frameY / 18;
			baseOption = 0;
			if (num29 == 48)
			{
				baseOption = 1;
			}
			break;
		}
		case 15:
		{
			int num21 = tileCache.frameY / 40;
			baseOption = 0;
			if (num21 == 1 || num21 == 20)
			{
				baseOption = 1;
			}
			break;
		}
		case 518:
		case 519:
			baseOption = tileCache.frameY / 18;
			break;
		case 4:
			if (tileCache.frameX < 66)
			{
				baseOption = 1;
			}
			baseOption = 0;
			break;
		case 572:
			baseOption = tileCache.frameY / 36;
			break;
		case 21:
		case 441:
			switch (tileCache.frameX / 36)
			{
			case 1:
			case 2:
			case 10:
			case 13:
			case 15:
				baseOption = 1;
				break;
			case 3:
			case 4:
				baseOption = 2;
				break;
			case 6:
				baseOption = 3;
				break;
			case 11:
			case 17:
				baseOption = 4;
				break;
			default:
				baseOption = 0;
				break;
			}
			break;
		case 467:
		case 468:
		{
			int num15 = tileCache.frameX / 36;
			switch (num15)
			{
			case 0:
			case 1:
			case 2:
			case 3:
			case 4:
			case 5:
			case 6:
			case 7:
			case 8:
			case 9:
			case 10:
			case 11:
				baseOption = num15;
				break;
			case 12:
			case 13:
				baseOption = 10;
				break;
			case 14:
				baseOption = 0;
				break;
			case 15:
				baseOption = 10;
				break;
			case 16:
				baseOption = 3;
				break;
			default:
				baseOption = 0;
				break;
			}
			break;
		}
		case 560:
		{
			int num16 = tileCache.frameX / 36;
			if ((uint)num16 <= 2u)
			{
				baseOption = num16;
			}
			else
			{
				baseOption = 0;
			}
			break;
		}
		case 28:
		case 653:
			if (tileCache.frameY < 144)
			{
				baseOption = 0;
			}
			else if (tileCache.frameY < 252)
			{
				baseOption = 1;
			}
			else if (tileCache.frameY < 360 || (tileCache.frameY > 900 && tileCache.frameY < 1008))
			{
				baseOption = 2;
			}
			else if (tileCache.frameY < 468)
			{
				baseOption = 3;
			}
			else if (tileCache.frameY < 576)
			{
				baseOption = 4;
			}
			else if (tileCache.frameY < 684)
			{
				baseOption = 5;
			}
			else if (tileCache.frameY < 792)
			{
				baseOption = 6;
			}
			else if (tileCache.frameY < 898)
			{
				baseOption = 8;
			}
			else if (tileCache.frameY < 1006)
			{
				baseOption = 7;
			}
			else if (tileCache.frameY < 1114)
			{
				baseOption = 0;
			}
			else if (tileCache.frameY < 1222)
			{
				baseOption = 3;
			}
			else
			{
				baseOption = 7;
			}
			break;
		case 27:
			if (tileCache.frameY < 34)
			{
				baseOption = 1;
			}
			else
			{
				baseOption = 0;
			}
			break;
		case 31:
			if (tileCache.frameX >= 36)
			{
				baseOption = 1;
			}
			else
			{
				baseOption = 0;
			}
			break;
		case 26:
			if (tileCache.frameX >= 54)
			{
				baseOption = 1;
			}
			else
			{
				baseOption = 0;
			}
			break;
		case 137:
			switch (tileCache.frameY / 18)
			{
			default:
				baseOption = 0;
				break;
			case 1:
			case 2:
			case 3:
			case 4:
				baseOption = 1;
				break;
			case 5:
				baseOption = 2;
				break;
			}
			break;
		case 82:
		case 83:
		case 84:
			if (tileCache.frameX < 18)
			{
				baseOption = 0;
			}
			else if (tileCache.frameX < 36)
			{
				baseOption = 1;
			}
			else if (tileCache.frameX < 54)
			{
				baseOption = 2;
			}
			else if (tileCache.frameX < 72)
			{
				baseOption = 3;
			}
			else if (tileCache.frameX < 90)
			{
				baseOption = 4;
			}
			else if (tileCache.frameX < 108)
			{
				baseOption = 5;
			}
			else
			{
				baseOption = 6;
			}
			break;
		case 591:
			baseOption = tileCache.frameX / 36;
			break;
		case 105:
			if (tileCache.frameX >= 1548 && tileCache.frameX <= 1654)
			{
				baseOption = 1;
			}
			else if (tileCache.frameX >= 1656 && tileCache.frameX <= 1798)
			{
				baseOption = 2;
			}
			else
			{
				baseOption = 0;
			}
			break;
		case 133:
			if (tileCache.frameX < 52)
			{
				baseOption = 0;
			}
			else
			{
				baseOption = 1;
			}
			break;
		case 134:
			if (tileCache.frameX < 28)
			{
				baseOption = 0;
			}
			else
			{
				baseOption = 1;
			}
			break;
		case 149:
			baseOption = y % 3;
			break;
		case 165:
			if (tileCache.frameX < 54)
			{
				baseOption = 0;
			}
			else if (tileCache.frameX < 106)
			{
				baseOption = 1;
			}
			else if (tileCache.frameX >= 216)
			{
				baseOption = 1;
			}
			else if (tileCache.frameX < 162)
			{
				baseOption = 2;
			}
			else
			{
				baseOption = 3;
			}
			break;
		case 178:
			if (tileCache.frameX < 18)
			{
				baseOption = 0;
			}
			else if (tileCache.frameX < 36)
			{
				baseOption = 1;
			}
			else if (tileCache.frameX < 54)
			{
				baseOption = 2;
			}
			else if (tileCache.frameX < 72)
			{
				baseOption = 3;
			}
			else if (tileCache.frameX < 90)
			{
				baseOption = 4;
			}
			else if (tileCache.frameX < 108)
			{
				baseOption = 5;
			}
			else
			{
				baseOption = 6;
			}
			break;
		case 184:
			if (tileCache.frameX < 22)
			{
				baseOption = 0;
			}
			else if (tileCache.frameX < 44)
			{
				baseOption = 1;
			}
			else if (tileCache.frameX < 66)
			{
				baseOption = 2;
			}
			else if (tileCache.frameX < 88)
			{
				baseOption = 3;
			}
			else if (tileCache.frameX < 110)
			{
				baseOption = 4;
			}
			else if (tileCache.frameX < 132)
			{
				baseOption = 5;
			}
			else if (tileCache.frameX < 154)
			{
				baseOption = 6;
			}
			else if (tileCache.frameX < 176)
			{
				baseOption = 7;
			}
			else if (tileCache.frameX < 198)
			{
				baseOption = 8;
			}
			else if (tileCache.frameX < 220)
			{
				baseOption = 9;
			}
			else if (tileCache.frameX < 242)
			{
				baseOption = 10;
			}
			break;
		case 650:
		{
			int num2 = tileCache.frameX / 36;
			int num20 = tileCache.frameY / 18 - 1;
			num2 += num20 * 18;
			if (num2 < 6 || num2 == 19 || num2 == 20 || num2 == 21 || num2 == 22 || num2 == 23 || num2 == 24 || num2 == 33 || num2 == 38 || num2 == 39 || num2 == 40)
			{
				baseOption = 0;
			}
			else if (num2 < 16)
			{
				baseOption = 2;
			}
			else if (num2 < 19 || num2 == 31 || num2 == 32)
			{
				baseOption = 1;
			}
			else if (num2 < 31)
			{
				baseOption = 3;
			}
			else if (num2 < 38)
			{
				baseOption = 4;
			}
			else if (num2 < 59)
			{
				baseOption = 0;
			}
			else if (num2 < 62)
			{
				baseOption = 1;
			}
			break;
		}
		case 649:
		{
			int num = tileCache.frameX / 18;
			if (num < 6 || num == 28 || num == 29 || num == 30 || num == 31 || num == 32)
			{
				baseOption = 0;
			}
			else if (num < 12 || num == 33 || num == 34 || num == 35)
			{
				baseOption = 1;
			}
			else if (num < 28)
			{
				baseOption = 2;
			}
			else if (num < 48)
			{
				baseOption = 3;
			}
			else if (num < 54)
			{
				baseOption = 4;
			}
			else if (num < 72)
			{
				baseOption = 0;
			}
			else if (num == 72)
			{
				baseOption = 1;
			}
			break;
		}
		case 185:
		{
			int num5;
			if (tileCache.frameY < 18)
			{
				num5 = tileCache.frameX / 18;
				if (num5 < 6 || num5 == 28 || num5 == 29 || num5 == 30 || num5 == 31 || num5 == 32)
				{
					baseOption = 0;
				}
				else if (num5 < 12 || num5 == 33 || num5 == 34 || num5 == 35)
				{
					baseOption = 1;
				}
				else if (num5 < 28)
				{
					baseOption = 2;
				}
				else if (num5 < 48)
				{
					baseOption = 3;
				}
				else if (num5 < 54)
				{
					baseOption = 4;
				}
				else if (num5 < 72)
				{
					baseOption = 0;
				}
				else if (num5 == 72)
				{
					baseOption = 1;
				}
				break;
			}
			num5 = tileCache.frameX / 36;
			int num27 = tileCache.frameY / 18 - 1;
			num5 += num27 * 18;
			if (num5 < 6 || num5 == 19 || num5 == 20 || num5 == 21 || num5 == 22 || num5 == 23 || num5 == 24 || num5 == 33 || num5 == 38 || num5 == 39 || num5 == 40)
			{
				baseOption = 0;
			}
			else if (num5 < 16)
			{
				baseOption = 2;
			}
			else if (num5 < 19 || num5 == 31 || num5 == 32)
			{
				baseOption = 1;
			}
			else if (num5 < 31)
			{
				baseOption = 3;
			}
			else if (num5 < 38)
			{
				baseOption = 4;
			}
			else if (num5 < 59)
			{
				baseOption = 0;
			}
			else if (num5 < 62)
			{
				baseOption = 1;
			}
			break;
		}
		case 186:
		case 647:
		{
			int num4 = tileCache.frameX / 54;
			if (num4 < 7)
			{
				baseOption = 2;
			}
			else if (num4 < 22 || num4 == 33 || num4 == 34 || num4 == 35)
			{
				baseOption = 0;
			}
			else if (num4 < 25)
			{
				baseOption = 1;
			}
			else if (num4 == 25)
			{
				baseOption = 5;
			}
			else if (num4 < 32)
			{
				baseOption = 3;
			}
			break;
		}
		case 187:
		case 648:
		{
			int num3 = tileCache.frameX / 54;
			int num26 = tileCache.frameY / 36;
			num3 += num26 * 36;
			if (num3 < 3 || num3 == 14 || num3 == 15 || num3 == 16)
			{
				baseOption = 0;
			}
			else if (num3 < 6)
			{
				baseOption = 6;
			}
			else if (num3 < 9)
			{
				baseOption = 7;
			}
			else if (num3 < 14)
			{
				baseOption = 4;
			}
			else if (num3 < 18)
			{
				baseOption = 4;
			}
			else if (num3 < 23)
			{
				baseOption = 8;
			}
			else if (num3 < 25)
			{
				baseOption = 0;
			}
			else if (num3 < 29)
			{
				baseOption = 1;
			}
			else if (num3 < 47)
			{
				baseOption = 0;
			}
			else if (num3 < 50)
			{
				baseOption = 1;
			}
			else if (num3 < 52)
			{
				baseOption = 10;
			}
			else if (num3 < 55)
			{
				baseOption = 2;
			}
			break;
		}
		case 227:
			baseOption = tileCache.frameX / 34;
			break;
		case 129:
			if (tileCache.frameX >= 324)
			{
				baseOption = 1;
			}
			else
			{
				baseOption = 0;
			}
			break;
		case 240:
		{
			int num7 = tileCache.frameX / 54;
			int num22 = tileCache.frameY / 54;
			num7 += num22 * 36;
			if ((num7 < 0 || num7 > 11) && (num7 < 47 || num7 > 53))
			{
				if ((uint)(num7 - 12) <= 3u)
				{
					baseOption = 1;
					break;
				}
				if ((uint)(num7 - 72) > 1u && num7 != 75)
				{
					switch (num7)
					{
					case 16:
					case 17:
						baseOption = 2;
						return;
					default:
						switch (num7)
						{
						case 41:
						case 42:
						case 43:
						case 44:
						case 45:
							baseOption = 3;
							return;
						default:
							if (num7 == 46)
							{
								baseOption = 4;
							}
							return;
						case 74:
						case 76:
						case 77:
						case 78:
						case 79:
						case 80:
						case 81:
						case 82:
						case 83:
						case 84:
						case 85:
						case 86:
						case 87:
						case 88:
						case 89:
						case 90:
						case 91:
						case 92:
							break;
						}
						break;
					case 18:
					case 19:
					case 20:
					case 21:
					case 22:
					case 23:
					case 24:
					case 25:
					case 26:
					case 27:
					case 28:
					case 29:
					case 30:
					case 31:
					case 32:
					case 33:
					case 34:
					case 35:
					case 63:
					case 64:
					case 65:
					case 66:
					case 67:
					case 68:
					case 69:
					case 70:
					case 71:
						break;
					}
					baseOption = 1;
					break;
				}
			}
			baseOption = 0;
			break;
		}
		case 242:
		{
			int num6 = tileCache.frameY / 72;
			if (tileCache.frameX / 106 == 0 && num6 >= 22 && num6 <= 24)
			{
				baseOption = 1;
			}
			else
			{
				baseOption = 0;
			}
			break;
		}
		case 440:
		{
			int num13 = tileCache.frameX / 54;
			if (num13 > 6)
			{
				num13 = 6;
			}
			baseOption = num13;
			break;
		}
		case 457:
		{
			int num14 = tileCache.frameX / 36;
			if (num14 > 4)
			{
				num14 = 4;
			}
			baseOption = num14;
			break;
		}
		case 453:
		{
			int num12 = tileCache.frameX / 36;
			if (num12 > 2)
			{
				num12 = 2;
			}
			baseOption = num12;
			break;
		}
		case 419:
		{
			int num11 = tileCache.frameX / 18;
			if (num11 > 2)
			{
				num11 = 2;
			}
			baseOption = num11;
			break;
		}
		case 428:
		{
			int num8 = tileCache.frameY / 18;
			if (num8 > 3)
			{
				num8 = 3;
			}
			baseOption = num8;
			break;
		}
		case 420:
		{
			int num10 = tileCache.frameY / 18;
			if (num10 > 5)
			{
				num10 = 5;
			}
			baseOption = num10;
			break;
		}
		case 423:
		{
			int num9 = tileCache.frameY / 18;
			if (num9 > 6)
			{
				num9 = 6;
			}
			baseOption = num9;
			break;
		}
		case 493:
			if (tileCache.frameX < 18)
			{
				baseOption = 0;
			}
			else if (tileCache.frameX < 36)
			{
				baseOption = 1;
			}
			else if (tileCache.frameX < 54)
			{
				baseOption = 2;
			}
			else if (tileCache.frameX < 72)
			{
				baseOption = 3;
			}
			else if (tileCache.frameX < 90)
			{
				baseOption = 4;
			}
			else
			{
				baseOption = 5;
			}
			break;
		case 548:
			if (tileCache.frameX / 54 < 7)
			{
				baseOption = 0;
			}
			else
			{
				baseOption = 1;
			}
			break;
		case 597:
		{
			int num17 = tileCache.frameX / 54;
			if ((uint)num17 <= 8u)
			{
				baseOption = num17;
			}
			else
			{
				baseOption = 0;
			}
			break;
		}
		default:
			baseOption = 0;
			break;
		}
	}

	public static void SaveMap()
	{
		if ((Main.ActivePlayerFileData.IsCloudSave && SocialAPI.Cloud == null) || !Main.mapEnabled || !Monitor.TryEnter(IOLock))
		{
			return;
		}
		try
		{
			FileUtilities.ProtectedInvoke(InternalSaveMap);
		}
		catch (Exception value)
		{
			using StreamWriter streamWriter = new StreamWriter("client-crashlog.txt", append: true);
			streamWriter.WriteLine(DateTime.Now);
			streamWriter.WriteLine(value);
			streamWriter.WriteLine("");
		}
		finally
		{
			Monitor.Exit(IOLock);
		}
	}

	private static void InternalSaveMap()
	{
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Expected O, but got Unknown
		bool isCloudSave = Main.ActivePlayerFileData.IsCloudSave;
		string text = Main.playerPathName.Substring(0, Main.playerPathName.Length - 4);
		if (!isCloudSave)
		{
			Utils.TryCreatingDirectory(text);
		}
		text += Path.DirectorySeparatorChar;
		text = ((!Main.ActiveWorldFileData.UseGuidAsMapName) ? (text + Main.worldID + ".map") : string.Concat(text, Main.ActiveWorldFileData.UniqueId, ".map"));
		new Stopwatch().Start();
		if (!Main.gameMenu)
		{
			noStatusText = true;
		}
		using (MemoryStream memoryStream = new MemoryStream(4000))
		{
			using BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
			DeflateStream deflateStream = new DeflateStream((Stream)memoryStream, (CompressionMode)0);
			try
			{
				int num = 0;
				byte[] array = new byte[16384];
				binaryWriter.Write(279);
				Main.MapFileMetadata.IncrementAndWrite(binaryWriter);
				binaryWriter.Write(Main.worldName);
				binaryWriter.Write(Main.worldID);
				binaryWriter.Write(Main.maxTilesY);
				binaryWriter.Write(Main.maxTilesX);
				binaryWriter.Write((short)TileID.Count);
				binaryWriter.Write((short)WallID.Count);
				binaryWriter.Write((short)4);
				binaryWriter.Write((short)256);
				binaryWriter.Write((short)256);
				binaryWriter.Write((short)256);
				byte b = 1;
				byte b2 = 0;
				int i;
				for (i = 0; i < TileID.Count; i++)
				{
					if (tileOptionCounts[i] != 1)
					{
						b2 |= b;
					}
					if (b == 128)
					{
						binaryWriter.Write(b2);
						b2 = 0;
						b = 1;
					}
					else
					{
						b <<= 1;
					}
				}
				if (b != 1)
				{
					binaryWriter.Write(b2);
				}
				i = 0;
				b = 1;
				b2 = 0;
				for (; i < WallID.Count; i++)
				{
					if (wallOptionCounts[i] != 1)
					{
						b2 |= b;
					}
					if (b == 128)
					{
						binaryWriter.Write(b2);
						b2 = 0;
						b = 1;
					}
					else
					{
						b <<= 1;
					}
				}
				if (b != 1)
				{
					binaryWriter.Write(b2);
				}
				for (i = 0; i < TileID.Count; i++)
				{
					if (tileOptionCounts[i] != 1)
					{
						binaryWriter.Write((byte)tileOptionCounts[i]);
					}
				}
				for (i = 0; i < WallID.Count; i++)
				{
					if (wallOptionCounts[i] != 1)
					{
						binaryWriter.Write((byte)wallOptionCounts[i]);
					}
				}
				binaryWriter.Flush();
				for (int j = 0; j < Main.maxTilesY; j++)
				{
					if (!noStatusText)
					{
						float num6 = (float)j / (float)Main.maxTilesY;
						Main.statusText = Lang.gen[66].Value + " " + (int)(num6 * 100f + 1f) + "%";
					}
					int num7;
					for (num7 = 0; num7 < Main.maxTilesX; num7++)
					{
						MapTile mapTile = Main.Map[num7, j];
						byte b4;
						byte b3;
						byte b5 = (b4 = (b3 = 0));
						int num8 = 0;
						bool flag = true;
						bool flag2 = true;
						int num9 = 0;
						int num10 = 0;
						byte b6 = 0;
						int num11;
						ushort num12;
						if (mapTile.Light <= 18 || mapTile.Type >= modPosition)
						{
							flag2 = false;
							flag = false;
							num11 = 0;
							num12 = 0;
							num8 = 0;
							int num13 = num7 + 1;
							int num2 = Main.maxTilesX - num7 - 1;
							while (num2 > 0 && Main.Map[num13, j].Light <= 18)
							{
								num8++;
								num2--;
								num13++;
							}
						}
						else
						{
							b6 = mapTile.Color;
							num12 = mapTile.Type;
							if (num12 < wallPosition)
							{
								num11 = 1;
								num12 -= tilePosition;
							}
							else if (num12 < liquidPosition)
							{
								num11 = 2;
								num12 -= wallPosition;
							}
							else if (num12 < skyPosition)
							{
								int num5 = num12 - liquidPosition;
								if (num5 == 3)
								{
									b4 = (byte)(b4 | 0x40u);
									num5 = 0;
								}
								num11 = 3 + num5;
								flag = false;
							}
							else if (num12 < dirtPosition)
							{
								num11 = 6;
								flag2 = false;
								flag = false;
							}
							else if (num12 < hellPosition)
							{
								num11 = 7;
								num12 = ((num12 >= rockPosition) ? ((ushort)(num12 - rockPosition)) : ((ushort)(num12 - dirtPosition)));
							}
							else
							{
								num11 = 6;
								flag = false;
							}
							if (mapTile.Light == byte.MaxValue)
							{
								flag2 = false;
							}
							if (flag2)
							{
								num8 = 0;
								int num14 = num7 + 1;
								int num3 = Main.maxTilesX - num7 - 1;
								num9 = num14;
								while (num3 > 0)
								{
									MapTile other = Main.Map[num14, j];
									if (mapTile.EqualsWithoutLight(ref other))
									{
										num3--;
										num8++;
										num14++;
										continue;
									}
									num10 = num14;
									break;
								}
							}
							else
							{
								num8 = 0;
								int num15 = num7 + 1;
								int num4 = Main.maxTilesX - num7 - 1;
								while (num4 > 0)
								{
									MapTile other2 = Main.Map[num15, j];
									if (!mapTile.Equals(ref other2))
									{
										break;
									}
									num4--;
									num8++;
									num15++;
								}
							}
						}
						if (b6 > 0)
						{
							b4 |= (byte)(b6 << 1);
						}
						if (b3 != 0)
						{
							b4 = (byte)(b4 | 1u);
						}
						if (b4 != 0)
						{
							b5 = (byte)(b5 | 1u);
						}
						b5 |= (byte)(num11 << 1);
						if (flag && num12 > 255)
						{
							b5 = (byte)(b5 | 0x10u);
						}
						if (flag2)
						{
							b5 = (byte)(b5 | 0x20u);
						}
						if (num8 > 0)
						{
							b5 = ((num8 <= 255) ? ((byte)(b5 | 0x40u)) : ((byte)(b5 | 0x80u)));
						}
						array[num] = b5;
						num++;
						if (b4 != 0)
						{
							array[num] = b4;
							num++;
						}
						if (b3 != 0)
						{
							array[num] = b3;
							num++;
						}
						if (flag)
						{
							array[num] = (byte)num12;
							num++;
							if (num12 > 255)
							{
								array[num] = (byte)(num12 >> 8);
								num++;
							}
						}
						if (flag2)
						{
							array[num] = mapTile.Light;
							num++;
						}
						if (num8 > 0)
						{
							array[num] = (byte)num8;
							num++;
							if (num8 > 255)
							{
								array[num] = (byte)(num8 >> 8);
								num++;
							}
						}
						for (int k = num9; k < num10; k++)
						{
							array[num] = Main.Map[k, j].Light;
							num++;
						}
						num7 += num8;
						if (num >= 4096)
						{
							((Stream)(object)deflateStream).Write(array, 0, num);
							num = 0;
						}
					}
				}
				if (num > 0)
				{
					((Stream)(object)deflateStream).Write(array, 0, num);
				}
				((Stream)(object)deflateStream).Dispose();
				FileUtilities.WriteAllBytes(text, memoryStream.ToArray(), isCloudSave);
				MapIO.WriteModFile(text, isCloudSave);
			}
			finally
			{
				((IDisposable)deflateStream)?.Dispose();
			}
		}
		noStatusText = false;
	}

	public static void LoadMapVersion1(BinaryReader fileIO, int release)
	{
		Main.MapFileMetadata = FileMetadata.FromCurrentSettings(FileType.Map);
		string text = fileIO.ReadString();
		int num = fileIO.ReadInt32();
		int num5 = fileIO.ReadInt32();
		int num6 = fileIO.ReadInt32();
		if (text != Main.worldName || num != Main.worldID || num6 != Main.maxTilesX || num5 != Main.maxTilesY)
		{
			throw new Exception("Map meta-data is invalid.");
		}
		OldMapHelper oldMapHelper = default(OldMapHelper);
		for (int i = 0; i < Main.maxTilesX; i++)
		{
			float num7 = (float)i / (float)Main.maxTilesX;
			Main.statusText = Lang.gen[67].Value + " " + (int)(num7 * 100f + 1f) + "%";
			for (int j = 0; j < Main.maxTilesY; j++)
			{
				if (fileIO.ReadBoolean())
				{
					int num8 = ((release <= 77) ? fileIO.ReadByte() : fileIO.ReadUInt16());
					byte b = fileIO.ReadByte();
					oldMapHelper.misc = fileIO.ReadByte();
					if (release >= 50)
					{
						oldMapHelper.misc2 = fileIO.ReadByte();
					}
					else
					{
						oldMapHelper.misc2 = 0;
					}
					bool flag = false;
					int num9 = oldMapHelper.option();
					int num10;
					if (oldMapHelper.active())
					{
						num10 = num9 + tileLookup[num8];
					}
					else if (oldMapHelper.water())
					{
						num10 = liquidPosition;
					}
					else if (oldMapHelper.lava())
					{
						num10 = liquidPosition + 1;
					}
					else if (oldMapHelper.honey())
					{
						num10 = liquidPosition + 2;
					}
					else if (oldMapHelper.wall())
					{
						num10 = num9 + wallLookup[num8];
					}
					else if ((double)j < Main.worldSurface)
					{
						flag = true;
						int num11 = (byte)(256.0 * ((double)j / Main.worldSurface));
						num10 = skyPosition + num11;
					}
					else if ((double)j < Main.rockLayer)
					{
						flag = true;
						if (num8 > 255)
						{
							num8 = 255;
						}
						num10 = num8 + dirtPosition;
					}
					else if (j < Main.UnderworldLayer)
					{
						flag = true;
						if (num8 > 255)
						{
							num8 = 255;
						}
						num10 = num8 + rockPosition;
					}
					else
					{
						num10 = hellPosition;
					}
					MapTile tile = MapTile.Create((ushort)num10, b, 0);
					Main.Map.SetTile(i, j, ref tile);
					int num12 = fileIO.ReadInt16();
					if (b == byte.MaxValue)
					{
						while (num12 > 0)
						{
							num12--;
							j++;
							if (flag)
							{
								if ((double)j < Main.worldSurface)
								{
									flag = true;
									int num2 = (byte)(256.0 * ((double)j / Main.worldSurface));
									num10 = skyPosition + num2;
								}
								else if ((double)j < Main.rockLayer)
								{
									flag = true;
									num10 = num8 + dirtPosition;
								}
								else if (j < Main.UnderworldLayer)
								{
									flag = true;
									num10 = num8 + rockPosition;
								}
								else
								{
									flag = true;
									num10 = hellPosition;
								}
								tile.Type = (ushort)num10;
							}
							Main.Map.SetTile(i, j, ref tile);
						}
						continue;
					}
					while (num12 > 0)
					{
						j++;
						num12--;
						b = fileIO.ReadByte();
						if (b <= 18)
						{
							continue;
						}
						tile.Light = b;
						if (flag)
						{
							if ((double)j < Main.worldSurface)
							{
								flag = true;
								int num3 = (byte)(256.0 * ((double)j / Main.worldSurface));
								num10 = skyPosition + num3;
							}
							else if ((double)j < Main.rockLayer)
							{
								flag = true;
								num10 = num8 + dirtPosition;
							}
							else if (j < Main.UnderworldLayer)
							{
								flag = true;
								num10 = num8 + rockPosition;
							}
							else
							{
								flag = true;
								num10 = hellPosition;
							}
							tile.Type = (ushort)num10;
						}
						Main.Map.SetTile(i, j, ref tile);
					}
				}
				else
				{
					int num4 = fileIO.ReadInt16();
					j += num4;
				}
			}
		}
	}

	public static void LoadMapVersion2(BinaryReader fileIO, int release)
	{
		//IL_0382: Unknown result type (might be due to invalid IL or missing references)
		//IL_038c: Expected O, but got Unknown
		if (release >= 135)
		{
			Main.MapFileMetadata = FileMetadata.Read(fileIO, FileType.Map);
		}
		else
		{
			Main.MapFileMetadata = FileMetadata.FromCurrentSettings(FileType.Map);
		}
		fileIO.ReadString();
		int num38 = fileIO.ReadInt32();
		int num20 = fileIO.ReadInt32();
		int num31 = fileIO.ReadInt32();
		if (num38 != Main.worldID || num31 != Main.maxTilesX || num20 != Main.maxTilesY)
		{
			throw new Exception("Map meta-data is invalid.");
		}
		short num32 = fileIO.ReadInt16();
		short num33 = fileIO.ReadInt16();
		short num34 = fileIO.ReadInt16();
		short num35 = fileIO.ReadInt16();
		short num36 = fileIO.ReadInt16();
		short num37 = fileIO.ReadInt16();
		bool[] array = new bool[num32];
		byte b = 0;
		byte b2 = 128;
		for (int i5 = 0; i5 < num32; i5++)
		{
			if (b2 == 128)
			{
				b = fileIO.ReadByte();
				b2 = 1;
			}
			else
			{
				b2 <<= 1;
			}
			if ((b & b2) == b2)
			{
				array[i5] = true;
			}
		}
		bool[] array2 = new bool[num33];
		b = 0;
		b2 = 128;
		for (int i4 = 0; i4 < num33; i4++)
		{
			if (b2 == 128)
			{
				b = fileIO.ReadByte();
				b2 = 1;
			}
			else
			{
				b2 <<= 1;
			}
			if ((b & b2) == b2)
			{
				array2[i4] = true;
			}
		}
		byte[] array3 = new byte[num32];
		ushort num10 = 0;
		for (int i3 = 0; i3 < num32; i3++)
		{
			if (array[i3])
			{
				array3[i3] = fileIO.ReadByte();
			}
			else
			{
				array3[i3] = 1;
			}
			num10 += array3[i3];
		}
		byte[] array4 = new byte[num33];
		ushort num11 = 0;
		for (int i2 = 0; i2 < num33; i2++)
		{
			if (array2[i2])
			{
				array4[i2] = fileIO.ReadByte();
			}
			else
			{
				array4[i2] = 1;
			}
			num11 += array4[i2];
		}
		ushort[] array5 = new ushort[num10 + num11 + num34 + num35 + num36 + num37 + 2];
		array5[0] = 0;
		ushort num12 = 1;
		ushort num13 = 1;
		ushort num14 = num13;
		for (int n = 0; n < TileID.Count; n++)
		{
			if (n < num32)
			{
				int num15 = array3[n];
				int num16 = tileOptionCounts[n];
				for (int j2 = 0; j2 < num16; j2++)
				{
					if (j2 < num15)
					{
						array5[num13] = num12;
						num13++;
					}
					num12++;
				}
			}
			else
			{
				num12 += (ushort)tileOptionCounts[n];
			}
		}
		ushort num17 = num13;
		for (int m = 0; m < WallID.Count; m++)
		{
			if (m < num33)
			{
				int num18 = array4[m];
				int num19 = wallOptionCounts[m];
				for (int k2 = 0; k2 < num19; k2++)
				{
					if (k2 < num18)
					{
						array5[num13] = num12;
						num13++;
					}
					num12++;
				}
			}
			else
			{
				num12 += (ushort)wallOptionCounts[m];
			}
		}
		ushort num21 = num13;
		for (int l = 0; l < 4; l++)
		{
			if (l < num34)
			{
				array5[num13] = num12;
				num13++;
			}
			num12++;
		}
		ushort num22 = num13;
		for (int k = 0; k < 256; k++)
		{
			if (k < num35)
			{
				array5[num13] = num12;
				num13++;
			}
			num12++;
		}
		ushort num23 = num13;
		for (int j = 0; j < 256; j++)
		{
			if (j < num36)
			{
				array5[num13] = num12;
				num13++;
			}
			num12++;
		}
		ushort num24 = num13;
		for (int i = 0; i < 256; i++)
		{
			if (i < num37)
			{
				array5[num13] = num12;
				num13++;
			}
			num12++;
		}
		ushort num25 = num13;
		array5[num13] = num12;
		BinaryReader binaryReader = ((release < 93) ? new BinaryReader(fileIO.BaseStream) : new BinaryReader((Stream)new DeflateStream(fileIO.BaseStream, (CompressionMode)1)));
		for (int l2 = 0; l2 < Main.maxTilesY; l2++)
		{
			float num26 = (float)l2 / (float)Main.maxTilesY;
			Main.statusText = Lang.gen[67].Value + " " + (int)(num26 * 100f + 1f) + "%";
			for (int m2 = 0; m2 < Main.maxTilesX; m2++)
			{
				byte b3 = binaryReader.ReadByte();
				byte b4 = (byte)(((b3 & 1) == 1) ? binaryReader.ReadByte() : 0);
				if ((b4 & 1) == 1)
				{
					binaryReader.ReadByte();
				}
				byte b5 = (byte)((b3 & 0xE) >> 1);
				bool flag;
				switch (b5)
				{
				case 0:
					flag = false;
					break;
				case 1:
				case 2:
				case 7:
					flag = true;
					break;
				case 3:
				case 4:
				case 5:
					flag = false;
					break;
				case 6:
					flag = false;
					break;
				default:
					flag = false;
					break;
				}
				ushort num27 = (ushort)(flag ? (((b3 & 0x10) != 16) ? binaryReader.ReadByte() : binaryReader.ReadUInt16()) : 0);
				byte b6 = (((b3 & 0x20) != 32) ? byte.MaxValue : binaryReader.ReadByte());
				int num28 = (byte)((b3 & 0xC0) >> 6) switch
				{
					0 => 0, 
					1 => binaryReader.ReadByte(), 
					2 => binaryReader.ReadInt16(), 
					_ => 0, 
				};
				switch (b5)
				{
				case 0:
					m2 += num28;
					continue;
				case 1:
					num27 += num14;
					break;
				case 2:
					num27 += num17;
					break;
				case 3:
				case 4:
				case 5:
				{
					int num29 = b5 - 3;
					if ((b4 & 0x40) == 64)
					{
						num29 = 3;
					}
					num27 += (ushort)(num21 + num29);
					break;
				}
				case 6:
					if ((double)l2 < Main.worldSurface)
					{
						ushort num30 = (ushort)((double)num35 * ((double)l2 / Main.worldSurface));
						num27 += (ushort)(num22 + num30);
					}
					else
					{
						num27 = num25;
					}
					break;
				case 7:
					num27 = ((!((double)l2 < Main.rockLayer)) ? ((ushort)(num27 + num24)) : ((ushort)(num27 + num23)));
					break;
				}
				MapTile tile = MapTile.Create(array5[num27], b6, (byte)((uint)(b4 >> 1) & 0x1Fu));
				Main.Map.SetTile(m2, l2, ref tile);
				if (b6 == byte.MaxValue)
				{
					while (num28 > 0)
					{
						m2++;
						Main.Map.SetTile(m2, l2, ref tile);
						num28--;
					}
					continue;
				}
				while (num28 > 0)
				{
					m2++;
					tile = tile.WithLight(binaryReader.ReadByte());
					Main.Map.SetTile(m2, l2, ref tile);
					num28--;
				}
			}
		}
		binaryReader.Close();
	}
}
