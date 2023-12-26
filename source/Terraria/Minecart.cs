using System;
using Microsoft.Xna.Framework;
using Terraria.GameContent;

namespace Terraria;

public static class Minecart
{
	private enum TrackState
	{
		NoTrack = -1,
		AboveTrack,
		OnTrack,
		BelowTrack,
		AboveFront,
		AboveBack,
		OnFront,
		OnBack
	}

	private const int TotalFrames = 36;

	public const int LeftDownDecoration = 36;

	public const int RightDownDecoration = 37;

	public const int BouncyBumperDecoration = 38;

	public const int RegularBumperDecoration = 39;

	public const int Flag_OnTrack = 0;

	public const int Flag_BouncyBumper = 1;

	public const int Flag_UsedRamp = 2;

	public const int Flag_HitSwitch = 3;

	public const int Flag_BoostLeft = 4;

	public const int Flag_BoostRight = 5;

	private const int NoConnection = -1;

	private const int TopConnection = 0;

	private const int MiddleConnection = 1;

	private const int BottomConnection = 2;

	private const int BumperEnd = -1;

	private const int BouncyEnd = -2;

	private const int RampEnd = -3;

	private const int OpenEnd = -4;

	public const float BoosterSpeed = 4f;

	private const int Type_Normal = 0;

	private const int Type_Pressure = 1;

	private const int Type_Booster = 2;

	private static Vector2 _trackMagnetOffset = new Vector2(25f, 26f);

	private const float MinecartTextureWidth = 50f;

	private static int[] _leftSideConnection;

	private static int[] _rightSideConnection;

	private static int[] _trackType;

	private static bool[] _boostLeft;

	private static Vector2[] _texturePosition;

	private static short _firstPressureFrame;

	private static short _firstLeftBoostFrame;

	private static short _firstRightBoostFrame;

	private static int[][] _trackSwitchOptions;

	private static int[][] _tileHeight;

	public static void Initialize()
	{
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_0169: Unknown result type (might be due to invalid IL or missing references)
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_021c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0221: Unknown result type (might be due to invalid IL or missing references)
		//IL_0286: Unknown result type (might be due to invalid IL or missing references)
		//IL_028b: Unknown result type (might be due to invalid IL or missing references)
		//IL_02dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0332: Unknown result type (might be due to invalid IL or missing references)
		//IL_0337: Unknown result type (might be due to invalid IL or missing references)
		//IL_037e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0383: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0427: Unknown result type (might be due to invalid IL or missing references)
		//IL_042c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0484: Unknown result type (might be due to invalid IL or missing references)
		//IL_0489: Unknown result type (might be due to invalid IL or missing references)
		//IL_04eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0552: Unknown result type (might be due to invalid IL or missing references)
		//IL_0557: Unknown result type (might be due to invalid IL or missing references)
		//IL_058b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0590: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0626: Unknown result type (might be due to invalid IL or missing references)
		//IL_062b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0688: Unknown result type (might be due to invalid IL or missing references)
		//IL_068d: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_074c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0751: Unknown result type (might be due to invalid IL or missing references)
		//IL_0798: Unknown result type (might be due to invalid IL or missing references)
		//IL_079d: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_080f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0814: Unknown result type (might be due to invalid IL or missing references)
		//IL_0850: Unknown result type (might be due to invalid IL or missing references)
		//IL_0855: Unknown result type (might be due to invalid IL or missing references)
		//IL_089a: Unknown result type (might be due to invalid IL or missing references)
		//IL_089f: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0942: Unknown result type (might be due to invalid IL or missing references)
		//IL_0947: Unknown result type (might be due to invalid IL or missing references)
		//IL_09a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_09a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a08: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a0d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a70: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a75: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aae: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ab3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aec: Unknown result type (might be due to invalid IL or missing references)
		//IL_0af1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b48: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b4d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ba4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ba9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c00: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c05: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c5c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c61: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c7b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c80: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c96: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c9b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cb1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cb6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ccc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cd1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ce8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cf2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cf7: Unknown result type (might be due to invalid IL or missing references)
		if (!Main.dedServ && (float)TextureAssets.MinecartMount.Width() != 50f)
		{
			throw new Exception("Be sure to update Minecart.textureWidth to match the actual texture size of " + 50f + ".");
		}
		_rightSideConnection = new int[36];
		_leftSideConnection = new int[36];
		_trackType = new int[36];
		_boostLeft = new bool[36];
		_texturePosition = (Vector2[])(object)new Vector2[40];
		_tileHeight = new int[36][];
		for (int i = 0; i < 36; i++)
		{
			int[] array = new int[8];
			for (int j = 0; j < array.Length; j++)
			{
				array[j] = 5;
			}
			_tileHeight[i] = array;
		}
		int num = 0;
		_leftSideConnection[num] = -1;
		_rightSideConnection[num] = -1;
		_tileHeight[num][0] = -4;
		_tileHeight[num][7] = -4;
		_texturePosition[num] = new Vector2(0f, 0f);
		num++;
		_leftSideConnection[num] = 1;
		_rightSideConnection[num] = 1;
		_texturePosition[num] = new Vector2(1f, 0f);
		num++;
		_leftSideConnection[num] = -1;
		_rightSideConnection[num] = 1;
		for (int k = 0; k < 4; k++)
		{
			_tileHeight[num][k] = -1;
		}
		_texturePosition[num] = new Vector2(2f, 1f);
		num++;
		_leftSideConnection[num] = 1;
		_rightSideConnection[num] = -1;
		for (int l = 4; l < 8; l++)
		{
			_tileHeight[num][l] = -1;
		}
		_texturePosition[num] = new Vector2(3f, 1f);
		num++;
		_leftSideConnection[num] = 2;
		_rightSideConnection[num] = 1;
		_tileHeight[num][0] = 1;
		_tileHeight[num][1] = 2;
		_tileHeight[num][2] = 3;
		_tileHeight[num][3] = 3;
		_tileHeight[num][4] = 4;
		_tileHeight[num][5] = 4;
		_texturePosition[num] = new Vector2(0f, 2f);
		num++;
		_leftSideConnection[num] = 1;
		_rightSideConnection[num] = 2;
		_tileHeight[num][2] = 4;
		_tileHeight[num][3] = 4;
		_tileHeight[num][4] = 3;
		_tileHeight[num][5] = 3;
		_tileHeight[num][6] = 2;
		_tileHeight[num][7] = 1;
		_texturePosition[num] = new Vector2(1f, 2f);
		num++;
		_leftSideConnection[num] = 1;
		_rightSideConnection[num] = 0;
		_tileHeight[num][4] = 6;
		_tileHeight[num][5] = 6;
		_tileHeight[num][6] = 7;
		_tileHeight[num][7] = 8;
		_texturePosition[num] = new Vector2(0f, 1f);
		num++;
		_leftSideConnection[num] = 0;
		_rightSideConnection[num] = 1;
		_tileHeight[num][0] = 8;
		_tileHeight[num][1] = 7;
		_tileHeight[num][2] = 6;
		_tileHeight[num][3] = 6;
		_texturePosition[num] = new Vector2(1f, 1f);
		num++;
		_leftSideConnection[num] = 0;
		_rightSideConnection[num] = 2;
		for (int m = 0; m < 8; m++)
		{
			_tileHeight[num][m] = 8 - m;
		}
		_texturePosition[num] = new Vector2(0f, 3f);
		num++;
		_leftSideConnection[num] = 2;
		_rightSideConnection[num] = 0;
		for (int n = 0; n < 8; n++)
		{
			_tileHeight[num][n] = n + 1;
		}
		_texturePosition[num] = new Vector2(1f, 3f);
		num++;
		_leftSideConnection[num] = 2;
		_rightSideConnection[num] = -1;
		_tileHeight[num][0] = 1;
		_tileHeight[num][1] = 2;
		for (int num12 = 2; num12 < 8; num12++)
		{
			_tileHeight[num][num12] = -1;
		}
		_texturePosition[num] = new Vector2(4f, 1f);
		num++;
		_leftSideConnection[num] = -1;
		_rightSideConnection[num] = 2;
		_tileHeight[num][6] = 2;
		_tileHeight[num][7] = 1;
		for (int num23 = 0; num23 < 6; num23++)
		{
			_tileHeight[num][num23] = -1;
		}
		_texturePosition[num] = new Vector2(5f, 1f);
		num++;
		_leftSideConnection[num] = 0;
		_rightSideConnection[num] = -1;
		_tileHeight[num][0] = 8;
		_tileHeight[num][1] = 7;
		_tileHeight[num][2] = 6;
		for (int num26 = 3; num26 < 8; num26++)
		{
			_tileHeight[num][num26] = -1;
		}
		_texturePosition[num] = new Vector2(6f, 1f);
		num++;
		_leftSideConnection[num] = -1;
		_rightSideConnection[num] = 0;
		_tileHeight[num][5] = 6;
		_tileHeight[num][6] = 7;
		_tileHeight[num][7] = 8;
		for (int num27 = 0; num27 < 5; num27++)
		{
			_tileHeight[num][num27] = -1;
		}
		_texturePosition[num] = new Vector2(7f, 1f);
		num++;
		_leftSideConnection[num] = -1;
		_rightSideConnection[num] = 1;
		_tileHeight[num][0] = -4;
		_texturePosition[num] = new Vector2(2f, 0f);
		num++;
		_leftSideConnection[num] = 1;
		_rightSideConnection[num] = -1;
		_tileHeight[num][7] = -4;
		_texturePosition[num] = new Vector2(3f, 0f);
		num++;
		_leftSideConnection[num] = 2;
		_rightSideConnection[num] = -1;
		for (int num28 = 0; num28 < 6; num28++)
		{
			_tileHeight[num][num28] = num28 + 1;
		}
		_tileHeight[num][6] = -3;
		_tileHeight[num][7] = -3;
		_texturePosition[num] = new Vector2(4f, 0f);
		num++;
		_leftSideConnection[num] = -1;
		_rightSideConnection[num] = 2;
		_tileHeight[num][0] = -3;
		_tileHeight[num][1] = -3;
		for (int num29 = 2; num29 < 8; num29++)
		{
			_tileHeight[num][num29] = 8 - num29;
		}
		_texturePosition[num] = new Vector2(5f, 0f);
		num++;
		_leftSideConnection[num] = 0;
		_rightSideConnection[num] = -1;
		for (int num30 = 0; num30 < 6; num30++)
		{
			_tileHeight[num][num30] = 8 - num30;
		}
		_tileHeight[num][6] = -3;
		_tileHeight[num][7] = -3;
		_texturePosition[num] = new Vector2(6f, 0f);
		num++;
		_leftSideConnection[num] = -1;
		_rightSideConnection[num] = 0;
		_tileHeight[num][0] = -3;
		_tileHeight[num][1] = -3;
		for (int num31 = 2; num31 < 8; num31++)
		{
			_tileHeight[num][num31] = num31 + 1;
		}
		_texturePosition[num] = new Vector2(7f, 0f);
		num++;
		_leftSideConnection[num] = -1;
		_rightSideConnection[num] = -1;
		_tileHeight[num][0] = -4;
		_tileHeight[num][7] = -4;
		_trackType[num] = 1;
		_texturePosition[num] = new Vector2(0f, 4f);
		num++;
		_leftSideConnection[num] = 1;
		_rightSideConnection[num] = 1;
		_trackType[num] = 1;
		_texturePosition[num] = new Vector2(1f, 4f);
		num++;
		_leftSideConnection[num] = -1;
		_rightSideConnection[num] = 1;
		_tileHeight[num][0] = -4;
		_trackType[num] = 1;
		_texturePosition[num] = new Vector2(0f, 5f);
		num++;
		_leftSideConnection[num] = 1;
		_rightSideConnection[num] = -1;
		_tileHeight[num][7] = -4;
		_trackType[num] = 1;
		_texturePosition[num] = new Vector2(1f, 5f);
		num++;
		_leftSideConnection[num] = -1;
		_rightSideConnection[num] = 1;
		for (int num2 = 0; num2 < 6; num2++)
		{
			_tileHeight[num][num2] = -2;
		}
		_texturePosition[num] = new Vector2(2f, 2f);
		num++;
		_leftSideConnection[num] = 1;
		_rightSideConnection[num] = -1;
		for (int num3 = 2; num3 < 8; num3++)
		{
			_tileHeight[num][num3] = -2;
		}
		_texturePosition[num] = new Vector2(3f, 2f);
		num++;
		_leftSideConnection[num] = 2;
		_rightSideConnection[num] = -1;
		_tileHeight[num][0] = 1;
		_tileHeight[num][1] = 2;
		for (int num4 = 2; num4 < 8; num4++)
		{
			_tileHeight[num][num4] = -2;
		}
		_texturePosition[num] = new Vector2(4f, 2f);
		num++;
		_leftSideConnection[num] = -1;
		_rightSideConnection[num] = 2;
		_tileHeight[num][6] = 2;
		_tileHeight[num][7] = 1;
		for (int num5 = 0; num5 < 6; num5++)
		{
			_tileHeight[num][num5] = -2;
		}
		_texturePosition[num] = new Vector2(5f, 2f);
		num++;
		_leftSideConnection[num] = 0;
		_rightSideConnection[num] = -1;
		_tileHeight[num][0] = 8;
		_tileHeight[num][1] = 7;
		_tileHeight[num][2] = 6;
		for (int num6 = 3; num6 < 8; num6++)
		{
			_tileHeight[num][num6] = -2;
		}
		_texturePosition[num] = new Vector2(6f, 2f);
		num++;
		_leftSideConnection[num] = -1;
		_rightSideConnection[num] = 0;
		_tileHeight[num][5] = 6;
		_tileHeight[num][6] = 7;
		_tileHeight[num][7] = 8;
		for (int num7 = 0; num7 < 5; num7++)
		{
			_tileHeight[num][num7] = -2;
		}
		_texturePosition[num] = new Vector2(7f, 2f);
		num++;
		_leftSideConnection[num] = 1;
		_rightSideConnection[num] = 1;
		_trackType[num] = 2;
		_boostLeft[num] = false;
		_texturePosition[num] = new Vector2(2f, 3f);
		num++;
		_leftSideConnection[num] = 1;
		_rightSideConnection[num] = 1;
		_trackType[num] = 2;
		_boostLeft[num] = true;
		_texturePosition[num] = new Vector2(3f, 3f);
		num++;
		_leftSideConnection[num] = 0;
		_rightSideConnection[num] = 2;
		for (int num8 = 0; num8 < 8; num8++)
		{
			_tileHeight[num][num8] = 8 - num8;
		}
		_trackType[num] = 2;
		_boostLeft[num] = false;
		_texturePosition[num] = new Vector2(4f, 3f);
		num++;
		_leftSideConnection[num] = 2;
		_rightSideConnection[num] = 0;
		for (int num9 = 0; num9 < 8; num9++)
		{
			_tileHeight[num][num9] = num9 + 1;
		}
		_trackType[num] = 2;
		_boostLeft[num] = true;
		_texturePosition[num] = new Vector2(5f, 3f);
		num++;
		_leftSideConnection[num] = 0;
		_rightSideConnection[num] = 2;
		for (int num10 = 0; num10 < 8; num10++)
		{
			_tileHeight[num][num10] = 8 - num10;
		}
		_trackType[num] = 2;
		_boostLeft[num] = true;
		_texturePosition[num] = new Vector2(6f, 3f);
		num++;
		_leftSideConnection[num] = 2;
		_rightSideConnection[num] = 0;
		for (int num11 = 0; num11 < 8; num11++)
		{
			_tileHeight[num][num11] = num11 + 1;
		}
		_trackType[num] = 2;
		_boostLeft[num] = false;
		_texturePosition[num] = new Vector2(7f, 3f);
		num++;
		_texturePosition[36] = new Vector2(0f, 6f);
		_texturePosition[37] = new Vector2(1f, 6f);
		_texturePosition[39] = new Vector2(0f, 7f);
		_texturePosition[38] = new Vector2(1f, 7f);
		for (int num13 = 0; num13 < _texturePosition.Length; num13++)
		{
			ref Vector2 reference = ref _texturePosition[num13];
			reference *= 18f;
		}
		for (int num14 = 0; num14 < _tileHeight.Length; num14++)
		{
			int[] array2 = _tileHeight[num14];
			for (int num15 = 0; num15 < array2.Length; num15++)
			{
				if (array2[num15] >= 0)
				{
					array2[num15] = (8 - array2[num15]) * 2;
				}
			}
		}
		int[] array3 = new int[36];
		_trackSwitchOptions = new int[64][];
		for (int num16 = 0; num16 < 64; num16++)
		{
			int num17 = 0;
			for (int num18 = 1; num18 < 256; num18 <<= 1)
			{
				if ((num16 & num18) == num18)
				{
					num17++;
				}
			}
			int num19 = 0;
			for (int num20 = 0; num20 < 36; num20++)
			{
				array3[num20] = -1;
				int num21 = 0;
				switch (_leftSideConnection[num20])
				{
				case 0:
					num21 |= 1;
					break;
				case 1:
					num21 |= 2;
					break;
				case 2:
					num21 |= 4;
					break;
				}
				switch (_rightSideConnection[num20])
				{
				case 0:
					num21 |= 8;
					break;
				case 1:
					num21 |= 0x10;
					break;
				case 2:
					num21 |= 0x20;
					break;
				}
				if (num17 < 2)
				{
					if (num16 != num21)
					{
						continue;
					}
				}
				else if (num21 == 0 || (num16 & num21) != num21)
				{
					continue;
				}
				array3[num20] = num20;
				num19++;
			}
			if (num19 == 0)
			{
				continue;
			}
			int[] array4 = new int[num19];
			int num22 = 0;
			for (int num24 = 0; num24 < 36; num24++)
			{
				if (array3[num24] != -1)
				{
					array4[num22] = array3[num24];
					num22++;
				}
			}
			_trackSwitchOptions[num16] = array4;
		}
		_firstPressureFrame = -1;
		_firstLeftBoostFrame = -1;
		_firstRightBoostFrame = -1;
		for (int num25 = 0; num25 < _trackType.Length; num25++)
		{
			switch (_trackType[num25])
			{
			case 1:
				if (_firstPressureFrame == -1)
				{
					_firstPressureFrame = (short)num25;
				}
				break;
			case 2:
				if (_boostLeft[num25])
				{
					if (_firstLeftBoostFrame == -1)
					{
						_firstLeftBoostFrame = (short)num25;
					}
				}
				else if (_firstRightBoostFrame == -1)
				{
					_firstRightBoostFrame = (short)num25;
				}
				break;
			}
		}
	}

	public static bool IsPressurePlate(Tile tile)
	{
		if (tile == null)
		{
			return false;
		}
		if (tile.active() && tile.type == 314 && (tile.frameX == 20 || tile.frameX == 21))
		{
			return true;
		}
		return false;
	}

	public static BitsByte TrackCollision(Player Player, ref Vector2 Position, ref Vector2 Velocity, ref Vector2 lastBoost, int Width, int Height, bool followDown, bool followUp, int fallStart, bool trackOnly, Mount.MountDelegatesData delegatesData)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_025d: Unknown result type (might be due to invalid IL or missing references)
		//IL_025f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0261: Unknown result type (might be due to invalid IL or missing references)
		//IL_0266: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_022e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0230: Unknown result type (might be due to invalid IL or missing references)
		//IL_0232: Unknown result type (might be due to invalid IL or missing references)
		//IL_0237: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0285: Unknown result type (might be due to invalid IL or missing references)
		//IL_0287: Unknown result type (might be due to invalid IL or missing references)
		//IL_0289: Unknown result type (might be due to invalid IL or missing references)
		//IL_028e: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_06fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_071a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0359: Unknown result type (might be due to invalid IL or missing references)
		//IL_034d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0336: Unknown result type (might be due to invalid IL or missing references)
		//IL_032a: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03da: Unknown result type (might be due to invalid IL or missing references)
		//IL_03aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0809: Unknown result type (might be due to invalid IL or missing references)
		//IL_080e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0733: Unknown result type (might be due to invalid IL or missing references)
		//IL_035e: Unknown result type (might be due to invalid IL or missing references)
		//IL_036b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0370: Unknown result type (might be due to invalid IL or missing references)
		//IL_0372: Unknown result type (might be due to invalid IL or missing references)
		//IL_0377: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_074a: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0404: Unknown result type (might be due to invalid IL or missing references)
		//IL_0763: Unknown result type (might be due to invalid IL or missing references)
		//IL_0412: Unknown result type (might be due to invalid IL or missing references)
		//IL_0825: Unknown result type (might be due to invalid IL or missing references)
		//IL_082c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0856: Unknown result type (might be due to invalid IL or missing references)
		//IL_0858: Unknown result type (might be due to invalid IL or missing references)
		//IL_08f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_08fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0791: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_087f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0891: Unknown result type (might be due to invalid IL or missing references)
		//IL_08a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_05df: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_08c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_07db: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_07eb: Unknown result type (might be due to invalid IL or missing references)
		if (followDown && followUp)
		{
			followDown = false;
			followUp = false;
		}
		Vector2 vector = default(Vector2);
		((Vector2)(ref vector))._002Ector((float)(Width / 2) - 25f, (float)(Height / 2));
		Vector2 vector2 = Position + new Vector2((float)(Width / 2) - 25f, (float)(Height / 2)) + _trackMagnetOffset;
		Vector2 vector3 = Velocity;
		float num = ((Vector2)(ref vector3)).Length();
		((Vector2)(ref vector3)).Normalize();
		Vector2 vector4 = vector2;
		Tile tile = default(Tile);
		bool flag = false;
		bool flag2 = true;
		int num12 = -1;
		int num20 = -1;
		int num21 = -1;
		TrackState trackState = TrackState.NoTrack;
		bool flag3 = false;
		bool flag4 = false;
		bool flag5 = false;
		bool flag6 = false;
		Vector2 vector5 = Vector2.Zero;
		Vector2 vector6 = Vector2.Zero;
		BitsByte result = default(BitsByte);
		while (true)
		{
			int num22 = (int)(vector4.X / 16f);
			int num23 = (int)(vector4.Y / 16f);
			int num24 = (int)vector4.X % 16 / 2;
			if (flag2)
			{
				num21 = num24;
			}
			bool flag7 = num24 != num21;
			if ((trackState == TrackState.OnBack || trackState == TrackState.OnTrack || trackState == TrackState.OnFront) && num22 != num12)
			{
				int num25 = ((trackState != TrackState.OnBack) ? tile.FrontTrack() : tile.BackTrack());
				switch ((!(vector3.X < 0f)) ? _rightSideConnection[num25] : _leftSideConnection[num25])
				{
				case 0:
					num23--;
					vector4.Y -= 2f;
					break;
				case 2:
					num23++;
					vector4.Y += 2f;
					break;
				}
			}
			TrackState trackState2 = TrackState.NoTrack;
			bool flag8 = false;
			if (num22 != num12 || num23 != num20)
			{
				if (flag2)
				{
					flag2 = false;
				}
				else
				{
					flag8 = true;
				}
				tile = Main.tile[num22, num23];
				if (tile == null)
				{
					tile = (Main.tile[num22, num23] = default(Tile));
				}
				flag = ((tile.nactive() && tile.type == 314) ? true : false);
			}
			if (flag)
			{
				TrackState trackState3 = TrackState.NoTrack;
				int num26 = tile.FrontTrack();
				int num2 = tile.BackTrack();
				int num3 = _tileHeight[num26][num24];
				switch (num3)
				{
				case -4:
					if (trackState == TrackState.OnFront)
					{
						if (trackOnly)
						{
							vector4 -= vector6;
							num = 0f;
							trackState2 = TrackState.OnFront;
							flag6 = true;
						}
						else
						{
							trackState2 = TrackState.NoTrack;
							flag5 = true;
						}
					}
					break;
				case -1:
					if (trackState == TrackState.OnFront)
					{
						vector4 -= vector6;
						num = 0f;
						trackState2 = TrackState.OnFront;
						flag6 = true;
					}
					break;
				case -2:
					if (trackState != TrackState.OnFront)
					{
						break;
					}
					if (trackOnly)
					{
						vector4 -= vector6;
						num = 0f;
						trackState2 = TrackState.OnFront;
						flag6 = true;
						break;
					}
					if (vector3.X < 0f)
					{
						float num6 = (float)(num22 * 16 + (num24 + 1) * 2) - vector4.X;
						vector4.X += num6;
						num += num6 / vector3.X;
					}
					vector3.X = 0f - vector3.X;
					result[1] = true;
					trackState2 = TrackState.OnFront;
					break;
				case -3:
					if (trackState == TrackState.OnFront)
					{
						trackState = TrackState.NoTrack;
						Matrix val = ((!(Velocity.X > 0f)) ? ((_rightSideConnection[num26] != 2) ? Matrix.CreateRotationZ(-(float)Math.PI / 4f) : Matrix.CreateRotationZ((float)Math.PI / 4f)) : ((_leftSideConnection[num26] != 2) ? Matrix.CreateRotationZ((float)Math.PI / 4f) : Matrix.CreateRotationZ(-(float)Math.PI / 4f)));
						vector5 = Vector2.Transform(new Vector2(Velocity.X, 0f), val);
						vector5.X = Velocity.X;
						flag4 = true;
						num = 0f;
					}
					break;
				default:
				{
					float num4 = num23 * 16 + num3;
					if (num22 != num12 && trackState == TrackState.NoTrack && vector4.Y > num4 && vector4.Y - num4 < 2f)
					{
						flag8 = false;
						trackState = TrackState.AboveFront;
					}
					TrackState trackState4 = ((!(vector4.Y < num4)) ? ((!(vector4.Y > num4)) ? TrackState.OnTrack : TrackState.BelowTrack) : TrackState.AboveTrack);
					if (num2 != -1)
					{
						float num5 = num23 * 16 + _tileHeight[num2][num24];
						trackState3 = ((!(vector4.Y < num5)) ? ((!(vector4.Y > num5)) ? TrackState.OnTrack : TrackState.BelowTrack) : TrackState.AboveTrack);
					}
					switch (trackState4)
					{
					case TrackState.OnTrack:
						trackState2 = ((trackState3 == TrackState.OnTrack) ? TrackState.OnTrack : TrackState.OnFront);
						break;
					case TrackState.AboveTrack:
						trackState2 = trackState3 switch
						{
							TrackState.OnTrack => TrackState.OnBack, 
							TrackState.BelowTrack => TrackState.AboveFront, 
							TrackState.AboveTrack => TrackState.AboveTrack, 
							_ => TrackState.AboveFront, 
						};
						break;
					case TrackState.BelowTrack:
						trackState2 = trackState3 switch
						{
							TrackState.OnTrack => TrackState.OnBack, 
							TrackState.AboveTrack => TrackState.AboveBack, 
							TrackState.BelowTrack => TrackState.BelowTrack, 
							_ => TrackState.BelowTrack, 
						};
						break;
					}
					break;
				}
				}
			}
			if (!flag8)
			{
				if (trackState != trackState2)
				{
					bool flag9 = false;
					if (flag7 || vector3.Y > 0f)
					{
						switch (trackState)
						{
						case TrackState.AboveTrack:
							switch (trackState2)
							{
							case TrackState.AboveFront:
								trackState2 = TrackState.OnBack;
								break;
							case TrackState.AboveBack:
								trackState2 = TrackState.OnFront;
								break;
							case TrackState.AboveTrack:
								trackState2 = TrackState.OnTrack;
								break;
							}
							break;
						case TrackState.AboveFront:
							if (trackState2 == TrackState.BelowTrack)
							{
								trackState2 = TrackState.OnFront;
							}
							break;
						case TrackState.AboveBack:
							if (trackState2 == TrackState.BelowTrack)
							{
								trackState2 = TrackState.OnBack;
							}
							break;
						case TrackState.OnFront:
							trackState2 = TrackState.OnFront;
							flag9 = true;
							break;
						case TrackState.OnBack:
							trackState2 = TrackState.OnBack;
							flag9 = true;
							break;
						case TrackState.OnTrack:
						{
							int num7 = _tileHeight[tile.FrontTrack()][num24];
							int num8 = _tileHeight[tile.BackTrack()][num24];
							trackState2 = ((!followDown) ? ((!followUp) ? TrackState.OnFront : ((num7 >= num8) ? TrackState.OnBack : TrackState.OnFront)) : ((num7 >= num8) ? TrackState.OnFront : TrackState.OnBack));
							flag9 = true;
							break;
						}
						}
						int num9 = -1;
						switch (trackState2)
						{
						case TrackState.OnTrack:
						case TrackState.OnFront:
							num9 = tile.FrontTrack();
							break;
						case TrackState.OnBack:
							num9 = tile.BackTrack();
							break;
						}
						if (num9 != -1)
						{
							if (!flag9 && Velocity.Y > Player.defaultGravity)
							{
								int num10 = (int)(Position.Y / 16f);
								if (fallStart < num10 - 1)
								{
									delegatesData.MinecartLandingSound(Player, Position, Width, Height);
									WheelSparks(delegatesData.MinecartDust, Position, Width, Height, 10);
								}
							}
							if (trackState == TrackState.AboveFront && _trackType[num9] == 1)
							{
								flag3 = true;
							}
							vector3.Y = 0f;
							vector4.Y = num23 * 16 + _tileHeight[num9][num24];
						}
					}
				}
			}
			else if (trackState2 == TrackState.OnFront || trackState2 == TrackState.OnBack || trackState2 == TrackState.OnTrack)
			{
				if (flag && _trackType[tile.FrontTrack()] == 1)
				{
					flag3 = true;
				}
				vector3.Y = 0f;
			}
			if (trackState2 == TrackState.OnFront)
			{
				int num11 = tile.FrontTrack();
				if (_trackType[num11] == 2 && lastBoost.X == 0f && lastBoost.Y == 0f)
				{
					lastBoost = new Vector2((float)num22, (float)num23);
					if (_boostLeft[num11])
					{
						result[4] = true;
					}
					else
					{
						result[5] = true;
					}
				}
			}
			num21 = num24;
			trackState = trackState2;
			num12 = num22;
			num20 = num23;
			if (num > 0f)
			{
				float num13 = vector4.X % 2f;
				float num14 = vector4.Y % 2f;
				float num15 = 3f;
				float num16 = 3f;
				if (vector3.X < 0f)
				{
					num15 = num13 + 0.125f;
				}
				else if (vector3.X > 0f)
				{
					num15 = 2f - num13;
				}
				if (vector3.Y < 0f)
				{
					num16 = num14 + 0.125f;
				}
				else if (vector3.Y > 0f)
				{
					num16 = 2f - num14;
				}
				if (num15 == 3f && num16 == 3f)
				{
					break;
				}
				float num17 = Math.Abs(num15 / vector3.X);
				float num18 = Math.Abs(num16 / vector3.Y);
				float num19 = ((num17 < num18) ? num17 : num18);
				if (num19 > num)
				{
					vector6 = vector3 * num;
					num = 0f;
				}
				else
				{
					vector6 = vector3 * num19;
					num -= num19;
				}
				vector4 += vector6;
				continue;
			}
			if (lastBoost.X != (float)num12 || lastBoost.Y != (float)num20)
			{
				lastBoost = Vector2.Zero;
			}
			break;
		}
		if (flag3)
		{
			result[3] = true;
		}
		if (flag5)
		{
			Velocity.X = vector4.X - vector2.X;
			Velocity.Y = Player.defaultGravity;
		}
		else if (flag4)
		{
			result[2] = true;
			Velocity = vector5;
		}
		else if (result[1])
		{
			Velocity.X = 0f - Velocity.X;
			Position.X = vector4.X - _trackMagnetOffset.X - vector.X - Velocity.X;
			if (vector3.Y == 0f)
			{
				Velocity.Y = 0f;
			}
		}
		else
		{
			if (flag6)
			{
				Velocity.X = vector4.X - vector2.X;
			}
			if (vector3.Y == 0f)
			{
				Velocity.Y = 0f;
			}
		}
		Position.Y += vector4.Y - vector2.Y - Velocity.Y;
		Position.Y = (float)Math.Round(Position.Y, 2);
		if (trackState == TrackState.OnTrack || (uint)(trackState - 5) <= 1u)
		{
			result[0] = true;
		}
		return result;
	}

	public static bool FrameTrack(int i, int j, bool pound, bool mute = false)
	{
		if (_trackType == null)
		{
			return false;
		}
		Tile tile = Main.tile[i, j];
		if (tile == null)
		{
			tile = (Main.tile[i, j] = default(Tile));
		}
		if (mute && tile.type != 314)
		{
			return false;
		}
		int nearbyTilesSetLookupIndex = GetNearbyTilesSetLookupIndex(i, j);
		int num = tile.FrontTrack();
		int num3 = tile.BackTrack();
		int num4 = ((num >= 0 && num < _trackType.Length) ? _trackType[num] : 0);
		int num5 = -1;
		int num6 = -1;
		int[] array = _trackSwitchOptions[nearbyTilesSetLookupIndex];
		if (array == null)
		{
			if (pound)
			{
				return false;
			}
			tile.FrontTrack(0);
			tile.BackTrack(-1);
			return false;
		}
		if (!pound)
		{
			int num7 = -1;
			int num8 = -1;
			bool flag = false;
			for (int k = 0; k < array.Length; k++)
			{
				int num9 = array[k];
				if (num3 == array[k])
				{
					num6 = k;
				}
				if (_trackType[num9] != num4)
				{
					continue;
				}
				if (_leftSideConnection[num9] == -1 || _rightSideConnection[num9] == -1)
				{
					if (num == array[k])
					{
						num5 = k;
						flag = true;
					}
					if (num7 == -1)
					{
						num7 = k;
					}
				}
				else
				{
					if (num == array[k])
					{
						num5 = k;
						flag = false;
					}
					if (num8 == -1)
					{
						num8 = k;
					}
				}
			}
			if (num8 != -1)
			{
				if (num5 == -1 || flag)
				{
					num5 = num8;
				}
			}
			else
			{
				if (num5 == -1)
				{
					switch (num4)
					{
					case 2:
						return false;
					case 1:
						return false;
					}
					num5 = num7;
				}
				num6 = -1;
			}
		}
		else
		{
			for (int l = 0; l < array.Length; l++)
			{
				if (num == array[l])
				{
					num5 = l;
				}
				if (num3 == array[l])
				{
					num6 = l;
				}
			}
			int num10 = 0;
			int num2 = 0;
			for (int m = 0; m < array.Length; m++)
			{
				if (_trackType[array[m]] == num4)
				{
					if (_leftSideConnection[array[m]] == -1 || _rightSideConnection[array[m]] == -1)
					{
						num2++;
					}
					else
					{
						num10++;
					}
				}
			}
			if (num10 < 2 && num2 < 2)
			{
				return false;
			}
			bool flag2 = num10 == 0;
			bool flag3 = false;
			if (!flag2)
			{
				while (!flag3)
				{
					num6++;
					if (num6 >= array.Length)
					{
						num6 = -1;
						break;
					}
					if ((_leftSideConnection[array[num6]] != _leftSideConnection[array[num5]] || _rightSideConnection[array[num6]] != _rightSideConnection[array[num5]]) && _trackType[array[num6]] == num4 && _leftSideConnection[array[num6]] != -1 && _rightSideConnection[array[num6]] != -1)
					{
						flag3 = true;
					}
				}
			}
			if (!flag3)
			{
				do
				{
					num5++;
					if (num5 >= array.Length)
					{
						num5 = -1;
						do
						{
							num5++;
						}
						while (_trackType[array[num5]] != num4 || (_leftSideConnection[array[num5]] == -1 || _rightSideConnection[array[num5]] == -1) != flag2);
						break;
					}
				}
				while (_trackType[array[num5]] != num4 || (_leftSideConnection[array[num5]] == -1 || _rightSideConnection[array[num5]] == -1) != flag2);
			}
		}
		bool flag4 = false;
		switch (num5)
		{
		case -2:
			if (tile.FrontTrack() != _firstPressureFrame)
			{
				flag4 = true;
			}
			break;
		case -1:
			if (tile.FrontTrack() != 0)
			{
				flag4 = true;
			}
			break;
		default:
			if (tile.FrontTrack() != array[num5])
			{
				flag4 = true;
			}
			break;
		}
		if (num6 == -1)
		{
			if (tile.BackTrack() != -1)
			{
				flag4 = true;
			}
		}
		else if (tile.BackTrack() != array[num6])
		{
			flag4 = true;
		}
		switch (num5)
		{
		case -2:
			tile.FrontTrack(_firstPressureFrame);
			break;
		case -1:
			tile.FrontTrack(0);
			break;
		default:
			tile.FrontTrack((short)array[num5]);
			break;
		}
		if (num6 == -1)
		{
			tile.BackTrack(-1);
		}
		else
		{
			tile.BackTrack((short)array[num6]);
		}
		if (pound && flag4 && !mute)
		{
			WorldGen.KillTile(i, j, fail: true);
		}
		return true;
	}

	private static int GetNearbyTilesSetLookupIndex(int i, int j)
	{
		int num = 0;
		if (Main.tile[i - 1, j - 1] != null && Main.tile[i - 1, j - 1].type == 314)
		{
			num++;
		}
		if (Main.tile[i - 1, j] != null && Main.tile[i - 1, j].type == 314)
		{
			num += 2;
		}
		if (Main.tile[i - 1, j + 1] != null && Main.tile[i - 1, j + 1].type == 314)
		{
			num += 4;
		}
		if (Main.tile[i + 1, j - 1] != null && Main.tile[i + 1, j - 1].type == 314)
		{
			num += 8;
		}
		if (Main.tile[i + 1, j] != null && Main.tile[i + 1, j].type == 314)
		{
			num += 16;
		}
		if (Main.tile[i + 1, j + 1] != null && Main.tile[i + 1, j + 1].type == 314)
		{
			num += 32;
		}
		return num;
	}

	public static bool GetOnTrack(int tileX, int tileY, ref Vector2 Position, int Width, int Height)
	{
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		Tile tile = Main.tile[tileX, tileY];
		if (tile.type != 314)
		{
			return false;
		}
		Vector2 vector = default(Vector2);
		((Vector2)(ref vector))._002Ector((float)(Width / 2) - 25f, (float)(Height / 2));
		Vector2 vector2 = Position + vector + _trackMagnetOffset;
		int num = (int)vector2.X % 16 / 2;
		int num2 = -1;
		int num3 = 0;
		for (int i = num; i < 8; i++)
		{
			num3 = _tileHeight[tile.frameX][i];
			if (num3 >= 0)
			{
				num2 = i;
				break;
			}
		}
		if (num2 == -1)
		{
			for (int num4 = num - 1; num4 >= 0; num4--)
			{
				num3 = _tileHeight[tile.frameX][num4];
				if (num3 >= 0)
				{
					num2 = num4;
					break;
				}
			}
		}
		if (num2 == -1)
		{
			return false;
		}
		vector2.X = tileX * 16 + num2 * 2;
		vector2.Y = tileY * 16 + num3;
		vector2 -= _trackMagnetOffset;
		vector2 -= vector;
		Position = vector2;
		return true;
	}

	public static bool OnTrack(Vector2 Position, int Width, int Height)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		Vector2 val = Position + new Vector2((float)(Width / 2) - 25f, (float)(Height / 2)) + _trackMagnetOffset;
		int num = (int)(val.X / 16f);
		int num2 = (int)(val.Y / 16f);
		if (Main.tile[num, num2] == null)
		{
			return false;
		}
		return Main.tile[num, num2].type == 314;
	}

	public static float TrackRotation(Player player, ref float rotation, Vector2 Position, int Width, int Height, bool followDown, bool followUp, Mount.MountDelegatesData delegatesData)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		GetWheelsPositions(player, Position, Width, Height, followDown, followUp, delegatesData, out var leftWheel, out var rightWheel);
		float num = rightWheel.Y - leftWheel.Y;
		float num2 = rightWheel.X - leftWheel.X;
		float num3 = num / num2;
		float num5 = leftWheel.Y + (Position.X - leftWheel.X) * num3;
		float num4 = (Position.X - (float)(int)Position.X) * num3;
		rotation = (float)Math.Atan2(num, num2);
		return num5 - Position.Y + num4;
	}

	public static void GetWheelsPositions(Player player, Vector2 Position, int Width, int Height, bool followDown, bool followUp, Mount.MountDelegatesData delegatesData, out Vector2 leftWheel, out Vector2 rightWheel)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		leftWheel = Position;
		rightWheel = Position;
		Vector2 lastBoost = Vector2.Zero;
		Vector2 Velocity = default(Vector2);
		((Vector2)(ref Velocity))._002Ector(-12f, 0f);
		TrackCollision(player, ref leftWheel, ref Velocity, ref lastBoost, Width, Height, followDown, followUp, 0, trackOnly: true, delegatesData);
		leftWheel += Velocity;
		((Vector2)(ref Velocity))._002Ector(12f, 0f);
		TrackCollision(player, ref rightWheel, ref Velocity, ref lastBoost, Width, Height, followDown, followUp, 0, trackOnly: true, delegatesData);
		rightWheel += Velocity;
	}

	public static void HitTrackSwitch(Vector2 Position, int Width, int Height)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		Vector2 magnetPosition = GetMagnetPosition(Position, Width, Height);
		int num = (int)(magnetPosition.X / 16f);
		int num2 = (int)(magnetPosition.Y / 16f);
		Wiring.HitSwitch(num, num2);
		NetMessage.SendData(59, -1, -1, null, num, num2);
	}

	public static Vector2 GetMagnetPosition(Vector2 Position, int Width, int Height)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		new Vector2((float)(Width / 2) - 25f, (float)(Height / 2));
		return Position + new Vector2((float)(Width / 2) - 25f, (float)(Height / 2)) + _trackMagnetOffset;
	}

	public static void FlipSwitchTrack(int i, int j)
	{
		Tile tileTrack = Main.tile[i, j];
		short num = tileTrack.FrontTrack();
		if (num == -1)
		{
			return;
		}
		switch (_trackType[num])
		{
		case 0:
			if (tileTrack.BackTrack() != -1)
			{
				tileTrack.FrontTrack(tileTrack.BackTrack());
				tileTrack.BackTrack(num);
				NetMessage.SendTileSquare(-1, i, j);
			}
			break;
		case 2:
			FrameTrack(i, j, pound: true, mute: true);
			NetMessage.SendTileSquare(-1, i, j);
			break;
		}
	}

	public static void TrackColors(int i, int j, Tile trackTile, out int frontColor, out int backColor)
	{
		if (trackTile.type == 314)
		{
			frontColor = trackTile.color();
			backColor = frontColor;
			if (trackTile.frameY == -1)
			{
				return;
			}
			int num = _leftSideConnection[trackTile.frameX];
			int num4 = _rightSideConnection[trackTile.frameX];
			int num5 = _leftSideConnection[trackTile.frameY];
			int num6 = _rightSideConnection[trackTile.frameY];
			int num7 = 0;
			int num8 = 0;
			int num9 = 0;
			int num10 = 0;
			for (int k = 0; k < 4; k++)
			{
				int num2 = (k switch
				{
					1 => num4, 
					2 => num5, 
					3 => num6, 
					_ => num, 
				}) switch
				{
					0 => -1, 
					1 => 0, 
					2 => 1, 
					_ => 0, 
				};
				Tile tile = ((k % 2 != 0) ? Main.tile[i + 1, j + num2] : Main.tile[i - 1, j + num2]);
				int num3 = ((tile != null && tile.active() && tile.type == 314) ? tile.color() : 0);
				switch (k)
				{
				default:
					num7 = num3;
					break;
				case 1:
					num8 = num3;
					break;
				case 2:
					num9 = num3;
					break;
				case 3:
					num10 = num3;
					break;
				}
			}
			if (num == num5)
			{
				if (num8 != 0)
				{
					frontColor = num8;
				}
				else if (num7 != 0)
				{
					frontColor = num7;
				}
				if (num10 != 0)
				{
					backColor = num10;
				}
				else if (num9 != 0)
				{
					backColor = num9;
				}
				return;
			}
			if (num4 == num6)
			{
				if (num7 != 0)
				{
					frontColor = num7;
				}
				else if (num8 != 0)
				{
					frontColor = num8;
				}
				if (num9 != 0)
				{
					backColor = num9;
				}
				else if (num10 != 0)
				{
					backColor = num10;
				}
				return;
			}
			if (num8 == 0)
			{
				if (num7 != 0)
				{
					frontColor = num7;
				}
			}
			else if (num7 != 0)
			{
				frontColor = ((num4 <= num) ? num8 : num7);
			}
			if (num10 == 0)
			{
				if (num9 != 0)
				{
					backColor = num9;
				}
			}
			else if (num9 != 0)
			{
				backColor = ((num6 <= num5) ? num10 : num9);
			}
		}
		else
		{
			frontColor = 0;
			backColor = 0;
		}
	}

	public static bool DrawLeftDecoration(int frameID)
	{
		if (frameID < 0 || frameID >= 36)
		{
			return false;
		}
		return _leftSideConnection[frameID] == 2;
	}

	public static bool DrawRightDecoration(int frameID)
	{
		if (frameID < 0 || frameID >= 36)
		{
			return false;
		}
		return _rightSideConnection[frameID] == 2;
	}

	public static bool DrawBumper(int frameID)
	{
		if (frameID < 0 || frameID >= 36)
		{
			return false;
		}
		if (_tileHeight[frameID][0] != -1)
		{
			return _tileHeight[frameID][7] == -1;
		}
		return true;
	}

	public static bool DrawBouncyBumper(int frameID)
	{
		if (frameID < 0 || frameID >= 36)
		{
			return false;
		}
		if (_tileHeight[frameID][0] != -2)
		{
			return _tileHeight[frameID][7] == -2;
		}
		return true;
	}

	public static void PlaceTrack(Tile trackCache, int style)
	{
		trackCache.active(active: true);
		trackCache.type = 314;
		trackCache.frameY = -1;
		switch (style)
		{
		case 0:
			trackCache.frameX = -1;
			break;
		case 1:
			trackCache.frameX = _firstPressureFrame;
			break;
		case 2:
			trackCache.frameX = _firstLeftBoostFrame;
			break;
		case 3:
			trackCache.frameX = _firstRightBoostFrame;
			break;
		}
	}

	public static int GetTrackItem(Tile trackCache)
	{
		return _trackType[trackCache.frameX] switch
		{
			0 => 2340, 
			1 => 2492, 
			2 => 2739, 
			_ => 0, 
		};
	}

	public static Rectangle GetSourceRect(int frameID, int animationFrame = 0)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		if (frameID < 0 || frameID >= 40)
		{
			return new Rectangle(0, 0, 0, 0);
		}
		Vector2 vector = _texturePosition[frameID];
		Rectangle result = default(Rectangle);
		((Rectangle)(ref result))._002Ector((int)vector.X, (int)vector.Y, 16, 16);
		if (frameID < 36 && _trackType[frameID] == 2)
		{
			result.Y += 18 * animationFrame;
		}
		return result;
	}

	public static bool GetAreExpectationsForSidesMet(Point tileCoords, int? expectedYOffsetForLeft, int? expectedYOffsetForRight)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		Tile tileTrack = Main.tile[tileCoords.X, tileCoords.Y];
		if (expectedYOffsetForLeft.HasValue)
		{
			short num = tileTrack.FrontTrack();
			int num2 = ConvertOffsetYToTrackConnectionValue(expectedYOffsetForLeft.Value);
			if (_leftSideConnection[num] != num2)
			{
				return false;
			}
		}
		if (expectedYOffsetForRight.HasValue)
		{
			short num3 = tileTrack.FrontTrack();
			int num4 = ConvertOffsetYToTrackConnectionValue(expectedYOffsetForRight.Value);
			if (_rightSideConnection[num3] != num4)
			{
				return false;
			}
		}
		return true;
	}

	public static void TryFittingTileOrientation(Point tileCoords, int? expectedYOffsetForLeft, int? expectedYOffsetForRight)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		int nearbyTilesSetLookupIndex = GetNearbyTilesSetLookupIndex(tileCoords.X, tileCoords.Y);
		int[] array = _trackSwitchOptions[nearbyTilesSetLookupIndex];
		if (array == null)
		{
			return;
		}
		Tile tileSafely = Framing.GetTileSafely(tileCoords);
		int num = _trackType[tileSafely.FrontTrack()];
		int? num2 = null;
		int[] array2 = array;
		foreach (int num3 in array2)
		{
			_ = _leftSideConnection[num3];
			_ = _rightSideConnection[num3];
			_ = _trackType[num3];
			if (expectedYOffsetForLeft.HasValue)
			{
				int num4 = ConvertOffsetYToTrackConnectionValue(expectedYOffsetForLeft.Value);
				if (_leftSideConnection[num3] != num4)
				{
					continue;
				}
			}
			if (expectedYOffsetForRight.HasValue)
			{
				int num5 = ConvertOffsetYToTrackConnectionValue(expectedYOffsetForRight.Value);
				if (_rightSideConnection[num3] != num5)
				{
					continue;
				}
			}
			if (_trackType[num3] == num)
			{
				num2 = num3;
				break;
			}
		}
		if (num2.HasValue)
		{
			tileSafely.FrontTrack((short)num2.Value);
			NetMessage.SendTileSquare(-1, tileCoords.X, tileCoords.Y);
		}
	}

	private static int ConvertOffsetYToTrackConnectionValue(int offsetY)
	{
		return offsetY switch
		{
			-1 => 0, 
			1 => 2, 
			_ => 1, 
		};
	}

	private static int ConvertTrackConnectionValueToOffsetY(int trackConnectionValue)
	{
		return trackConnectionValue switch
		{
			0 => -1, 
			2 => 1, 
			_ => 0, 
		};
	}

	public static void WheelSparks(Action<Vector2> DustAction, Vector2 Position, int Width, int Height, int sparkCount)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		Vector2 vector = default(Vector2);
		((Vector2)(ref vector))._002Ector((float)(Width / 2) - 25f, (float)(Height / 2));
		Vector2 obj = Position + vector + _trackMagnetOffset;
		for (int i = 0; i < sparkCount; i++)
		{
			DustAction(obj);
		}
	}

	private static short FrontTrack(this Tile tileTrack)
	{
		return tileTrack.frameX;
	}

	private static void FrontTrack(this Tile tileTrack, short trackID)
	{
		tileTrack.frameX = trackID;
	}

	private static short BackTrack(this Tile tileTrack)
	{
		return tileTrack.frameY;
	}

	private static void BackTrack(this Tile tileTrack, short trackID)
	{
		tileTrack.frameY = trackID;
	}
}
