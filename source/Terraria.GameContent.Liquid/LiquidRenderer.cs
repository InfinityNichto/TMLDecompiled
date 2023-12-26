using System;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Graphics;
using Terraria.ID;
using Terraria.Utilities;

namespace Terraria.GameContent.Liquid;

public class LiquidRenderer
{
	private struct LiquidCache
	{
		public float LiquidLevel;

		public float VisibleLiquidLevel;

		public float Opacity;

		public bool IsSolid;

		public bool IsHalfBrick;

		public bool HasLiquid;

		public bool HasVisibleLiquid;

		public bool HasWall;

		public Point FrameOffset;

		public bool HasLeftEdge;

		public bool HasRightEdge;

		public bool HasTopEdge;

		public bool HasBottomEdge;

		public float LeftWall;

		public float RightWall;

		public float BottomWall;

		public float TopWall;

		public float VisibleLeftWall;

		public float VisibleRightWall;

		public float VisibleBottomWall;

		public float VisibleTopWall;

		public byte Type;

		public byte VisibleType;
	}

	private struct LiquidDrawCache
	{
		public Rectangle SourceRectangle;

		public Vector2 LiquidOffset;

		public bool IsVisible;

		public float Opacity;

		public byte Type;

		public bool IsSurfaceLiquid;

		public bool HasWall;
	}

	private struct SpecialLiquidDrawCache
	{
		public int X;

		public int Y;

		public Rectangle SourceRectangle;

		public Vector2 LiquidOffset;

		public bool IsVisible;

		public float Opacity;

		public byte Type;

		public bool IsSurfaceLiquid;

		public bool HasWall;
	}

	private const int ANIMATION_FRAME_COUNT = 16;

	private const int CACHE_PADDING = 2;

	private const int CACHE_PADDING_2 = 4;

	private static readonly int[] WATERFALL_LENGTH = new int[4] { 10, 3, 2, 10 };

	private static readonly float[] DEFAULT_OPACITY = new float[4] { 0.6f, 0.95f, 0.95f, 0.75f };

	private static readonly byte[] WAVE_MASK_STRENGTH = new byte[5];

	private static readonly byte[] VISCOSITY_MASK = new byte[5] { 0, 200, 240, 0, 0 };

	public const float MIN_LIQUID_SIZE = 0.25f;

	public static LiquidRenderer Instance;

	public Asset<Texture2D>[] _liquidTextures = new Asset<Texture2D>[15];

	private LiquidCache[] _cache = new LiquidCache[1];

	private LiquidDrawCache[] _drawCache = new LiquidDrawCache[1];

	private SpecialLiquidDrawCache[] _drawCacheForShimmer = new SpecialLiquidDrawCache[1];

	private int _animationFrame;

	private Rectangle _drawArea = new Rectangle(0, 0, 1, 1);

	private readonly UnifiedRandom _random = new UnifiedRandom();

	private Color[] _waveMask = (Color[])(object)new Color[1];

	private float _frameState;

	public event Action<Color[], Rectangle> WaveFilters;

	public static void LoadContent()
	{
		Instance = new LiquidRenderer();
		Instance.PrepareAssets();
	}

	private void PrepareAssets()
	{
		if (!Main.dedServ)
		{
			for (int i = 0; i < _liquidTextures.Length; i++)
			{
				_liquidTextures[i] = Main.Assets.Request<Texture2D>("Images/Misc/water_" + i);
			}
		}
	}

	private unsafe void InternalPrepareDraw(Rectangle drawArea)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0249: Unknown result type (might be due to invalid IL or missing references)
		//IL_024f: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_022f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0235: Unknown result type (might be due to invalid IL or missing references)
		//IL_0427: Unknown result type (might be due to invalid IL or missing references)
		//IL_0404: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0591: Unknown result type (might be due to invalid IL or missing references)
		//IL_0919: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_08f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b06: Unknown result type (might be due to invalid IL or missing references)
		//IL_0636: Unknown result type (might be due to invalid IL or missing references)
		//IL_064f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ae3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ca8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0979: Unknown result type (might be due to invalid IL or missing references)
		//IL_0992: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c85: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ed5: Unknown result type (might be due to invalid IL or missing references)
		//IL_075c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0761: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b65: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b7e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eb2: Unknown result type (might be due to invalid IL or missing references)
		//IL_1217: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c01: Unknown result type (might be due to invalid IL or missing references)
		//IL_122f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c11: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d07: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d20: Unknown result type (might be due to invalid IL or missing references)
		//IL_11f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_13cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_13d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_123c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c3d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c42: Unknown result type (might be due to invalid IL or missing references)
		//IL_13b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_13b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_14e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c68: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c6d: Unknown result type (might be due to invalid IL or missing references)
		//IL_14cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_1516: Unknown result type (might be due to invalid IL or missing references)
		//IL_103c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1041: Unknown result type (might be due to invalid IL or missing references)
		//IL_10b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_10bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_1068: Unknown result type (might be due to invalid IL or missing references)
		//IL_128f: Unknown result type (might be due to invalid IL or missing references)
		//IL_12a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_1472: Unknown result type (might be due to invalid IL or missing references)
		//IL_1477: Unknown result type (might be due to invalid IL or missing references)
		//IL_148e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1493: Unknown result type (might be due to invalid IL or missing references)
		//IL_12d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_12ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_1306: Unknown result type (might be due to invalid IL or missing references)
		//IL_1310: Unknown result type (might be due to invalid IL or missing references)
		//IL_1315: Unknown result type (might be due to invalid IL or missing references)
		//IL_08a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_08dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_08de: Unknown result type (might be due to invalid IL or missing references)
		//IL_08c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_1161: Unknown result type (might be due to invalid IL or missing references)
		//IL_1166: Unknown result type (might be due to invalid IL or missing references)
		Rectangle rectangle = default(Rectangle);
		((Rectangle)(ref rectangle))._002Ector(drawArea.X - 2, drawArea.Y - 2, drawArea.Width + 4, drawArea.Height + 4);
		_drawArea = drawArea;
		if (_cache.Length < rectangle.Width * rectangle.Height + 1)
		{
			_cache = new LiquidCache[rectangle.Width * rectangle.Height + 1];
		}
		if (_drawCache.Length < drawArea.Width * drawArea.Height + 1)
		{
			_drawCache = new LiquidDrawCache[drawArea.Width * drawArea.Height + 1];
		}
		if (_drawCacheForShimmer.Length < drawArea.Width * drawArea.Height + 1)
		{
			_drawCacheForShimmer = new SpecialLiquidDrawCache[drawArea.Width * drawArea.Height + 1];
		}
		if (_waveMask.Length < drawArea.Width * drawArea.Height)
		{
			_waveMask = (Color[])(object)new Color[drawArea.Width * drawArea.Height];
		}
		Tile tile = default(Tile);
		fixed (LiquidCache* ptr = &_cache[1])
		{
			LiquidCache* ptr3 = ptr;
			int num = rectangle.Height * 2 + 2;
			ptr3 = ptr;
			for (int i = rectangle.X; i < rectangle.X + rectangle.Width; i++)
			{
				for (int j = rectangle.Y; j < rectangle.Y + rectangle.Height; j++)
				{
					tile = Main.tile[i, j];
					if (tile == null)
					{
						tile = default(Tile);
					}
					ptr3->LiquidLevel = (float)(int)tile.liquid / 255f;
					ptr3->IsHalfBrick = tile.halfBrick() && ptr3[-1].HasLiquid && !TileID.Sets.Platforms[tile.type];
					ptr3->IsSolid = WorldGen.SolidOrSlopedTile(tile);
					ptr3->HasLiquid = tile.liquid != 0;
					ptr3->VisibleLiquidLevel = 0f;
					ptr3->HasWall = tile.wall != 0;
					ptr3->Type = tile.liquidType();
					if (ptr3->IsHalfBrick && !ptr3->HasLiquid)
					{
						ptr3->Type = ptr3[-1].Type;
					}
					ptr3++;
				}
			}
			ptr3 = ptr;
			float num12 = 0f;
			ptr3 += num;
			for (int k = 2; k < rectangle.Width - 2; k++)
			{
				for (int l = 2; l < rectangle.Height - 2; l++)
				{
					num12 = 0f;
					if (ptr3->IsHalfBrick && ptr3[-1].HasLiquid)
					{
						num12 = 1f;
					}
					else if (!ptr3->HasLiquid)
					{
						LiquidCache liquidCache4 = ptr3[-1];
						LiquidCache liquidCache8 = ptr3[1];
						LiquidCache liquidCache13 = ptr3[-rectangle.Height];
						LiquidCache liquidCache18 = ptr3[rectangle.Height];
						if (liquidCache4.HasLiquid && liquidCache8.HasLiquid && liquidCache4.Type == liquidCache8.Type && !liquidCache4.IsSolid && !liquidCache8.IsSolid)
						{
							num12 = liquidCache4.LiquidLevel + liquidCache8.LiquidLevel;
							ptr3->Type = liquidCache4.Type;
						}
						if (liquidCache13.HasLiquid && liquidCache18.HasLiquid && liquidCache13.Type == liquidCache18.Type && !liquidCache13.IsSolid && !liquidCache18.IsSolid)
						{
							num12 = Math.Max(num12, liquidCache13.LiquidLevel + liquidCache18.LiquidLevel);
							ptr3->Type = liquidCache13.Type;
						}
						num12 *= 0.5f;
					}
					else
					{
						num12 = ptr3->LiquidLevel;
					}
					ptr3->VisibleLiquidLevel = num12;
					ptr3->HasVisibleLiquid = num12 != 0f;
					ptr3++;
				}
				ptr3 += 4;
			}
			ptr3 = ptr;
			for (int m = 0; m < rectangle.Width; m++)
			{
				for (int n = 0; n < rectangle.Height - 10; n++)
				{
					if (ptr3->HasVisibleLiquid && (!ptr3->IsSolid || ptr3->IsHalfBrick))
					{
						ptr3->Opacity = 1f;
						ptr3->VisibleType = ptr3->Type;
						float num23 = 1f / (float)(WATERFALL_LENGTH[ptr3->Type] + 1);
						float num24 = 1f;
						for (int num25 = 1; num25 <= WATERFALL_LENGTH[ptr3->Type]; num25++)
						{
							num24 -= num23;
							if (ptr3[num25].IsSolid)
							{
								break;
							}
							ptr3[num25].VisibleLiquidLevel = Math.Max(ptr3[num25].VisibleLiquidLevel, ptr3->VisibleLiquidLevel * num24);
							ptr3[num25].Opacity = num24;
							ptr3[num25].VisibleType = ptr3->Type;
						}
					}
					if (ptr3->IsSolid && !ptr3->IsHalfBrick)
					{
						ptr3->VisibleLiquidLevel = 1f;
						ptr3->HasVisibleLiquid = false;
					}
					else
					{
						ptr3->HasVisibleLiquid = ptr3->VisibleLiquidLevel != 0f;
					}
					ptr3++;
				}
				ptr3 += 10;
			}
			ptr3 = ptr;
			ptr3 += num;
			for (int num26 = 2; num26 < rectangle.Width - 2; num26++)
			{
				for (int num27 = 2; num27 < rectangle.Height - 2; num27++)
				{
					if (!ptr3->HasVisibleLiquid)
					{
						ptr3->HasLeftEdge = false;
						ptr3->HasTopEdge = false;
						ptr3->HasRightEdge = false;
						ptr3->HasBottomEdge = false;
					}
					else
					{
						LiquidCache liquidCache3 = ptr3[-1];
						LiquidCache liquidCache7 = ptr3[1];
						LiquidCache liquidCache12 = ptr3[-rectangle.Height];
						LiquidCache liquidCache17 = ptr3[rectangle.Height];
						float num28 = 0f;
						float num29 = 1f;
						float num2 = 0f;
						float num3 = 1f;
						float visibleLiquidLevel = ptr3->VisibleLiquidLevel;
						if (!liquidCache3.HasVisibleLiquid)
						{
							num2 += liquidCache7.VisibleLiquidLevel * (1f - visibleLiquidLevel);
						}
						if (!liquidCache7.HasVisibleLiquid && !liquidCache7.IsSolid && !liquidCache7.IsHalfBrick)
						{
							num3 -= liquidCache3.VisibleLiquidLevel * (1f - visibleLiquidLevel);
						}
						if (!liquidCache12.HasVisibleLiquid && !liquidCache12.IsSolid && !liquidCache12.IsHalfBrick)
						{
							num28 += liquidCache17.VisibleLiquidLevel * (1f - visibleLiquidLevel);
						}
						if (!liquidCache17.HasVisibleLiquid && !liquidCache17.IsSolid && !liquidCache17.IsHalfBrick)
						{
							num29 -= liquidCache12.VisibleLiquidLevel * (1f - visibleLiquidLevel);
						}
						ptr3->LeftWall = num28;
						ptr3->RightWall = num29;
						ptr3->BottomWall = num3;
						ptr3->TopWall = num2;
						Point zero = Point.Zero;
						ptr3->HasTopEdge = (!liquidCache3.HasVisibleLiquid && !liquidCache3.IsSolid) || num2 != 0f;
						ptr3->HasBottomEdge = (!liquidCache7.HasVisibleLiquid && !liquidCache7.IsSolid) || num3 != 1f;
						ptr3->HasLeftEdge = (!liquidCache12.HasVisibleLiquid && !liquidCache12.IsSolid) || num28 != 0f;
						ptr3->HasRightEdge = (!liquidCache17.HasVisibleLiquid && !liquidCache17.IsSolid) || num29 != 1f;
						if (!ptr3->HasLeftEdge)
						{
							if (ptr3->HasRightEdge)
							{
								zero.X += 32;
							}
							else
							{
								zero.X += 16;
							}
						}
						if (ptr3->HasLeftEdge && ptr3->HasRightEdge)
						{
							zero.X = 16;
							zero.Y += 32;
							if (ptr3->HasTopEdge)
							{
								zero.Y = 16;
							}
						}
						else if (!ptr3->HasTopEdge)
						{
							if (!ptr3->HasLeftEdge && !ptr3->HasRightEdge)
							{
								zero.Y += 48;
							}
							else
							{
								zero.Y += 16;
							}
						}
						if (zero.Y == 16 && (ptr3->HasLeftEdge ^ ptr3->HasRightEdge) && (num27 + rectangle.Y) % 2 == 0)
						{
							zero.Y += 16;
						}
						Unsafe.Write(&ptr3->FrameOffset, zero);
					}
					ptr3++;
				}
				ptr3 += 4;
			}
			ptr3 = ptr;
			ptr3 += num;
			for (int num4 = 2; num4 < rectangle.Width - 2; num4++)
			{
				for (int num5 = 2; num5 < rectangle.Height - 2; num5++)
				{
					if (ptr3->HasVisibleLiquid)
					{
						LiquidCache liquidCache2 = ptr3[-1];
						LiquidCache liquidCache6 = ptr3[1];
						LiquidCache liquidCache11 = ptr3[-rectangle.Height];
						LiquidCache liquidCache16 = ptr3[rectangle.Height];
						ptr3->VisibleLeftWall = ptr3->LeftWall;
						ptr3->VisibleRightWall = ptr3->RightWall;
						ptr3->VisibleTopWall = ptr3->TopWall;
						ptr3->VisibleBottomWall = ptr3->BottomWall;
						if (liquidCache2.HasVisibleLiquid && liquidCache6.HasVisibleLiquid)
						{
							if (ptr3->HasLeftEdge)
							{
								ptr3->VisibleLeftWall = (ptr3->LeftWall * 2f + liquidCache2.LeftWall + liquidCache6.LeftWall) * 0.25f;
							}
							if (ptr3->HasRightEdge)
							{
								ptr3->VisibleRightWall = (ptr3->RightWall * 2f + liquidCache2.RightWall + liquidCache6.RightWall) * 0.25f;
							}
						}
						if (liquidCache11.HasVisibleLiquid && liquidCache16.HasVisibleLiquid)
						{
							if (ptr3->HasTopEdge)
							{
								ptr3->VisibleTopWall = (ptr3->TopWall * 2f + liquidCache11.TopWall + liquidCache16.TopWall) * 0.25f;
							}
							if (ptr3->HasBottomEdge)
							{
								ptr3->VisibleBottomWall = (ptr3->BottomWall * 2f + liquidCache11.BottomWall + liquidCache16.BottomWall) * 0.25f;
							}
						}
					}
					ptr3++;
				}
				ptr3 += 4;
			}
			ptr3 = ptr;
			ptr3 += num;
			for (int num6 = 2; num6 < rectangle.Width - 2; num6++)
			{
				for (int num7 = 2; num7 < rectangle.Height - 2; num7++)
				{
					if (ptr3->HasLiquid)
					{
						_ = ptr3[-1];
						LiquidCache liquidCache5 = ptr3[1];
						LiquidCache liquidCache10 = ptr3[-rectangle.Height];
						LiquidCache liquidCache15 = ptr3[rectangle.Height];
						if (ptr3->HasTopEdge && !ptr3->HasBottomEdge && (ptr3->HasLeftEdge ^ ptr3->HasRightEdge))
						{
							if (ptr3->HasRightEdge)
							{
								ptr3->VisibleRightWall = liquidCache5.VisibleRightWall;
								ptr3->VisibleTopWall = liquidCache10.VisibleTopWall;
							}
							else
							{
								ptr3->VisibleLeftWall = liquidCache5.VisibleLeftWall;
								ptr3->VisibleTopWall = liquidCache15.VisibleTopWall;
							}
						}
						else if (liquidCache5.FrameOffset.X == 16 && liquidCache5.FrameOffset.Y == 32)
						{
							if (ptr3->VisibleLeftWall > 0.5f)
							{
								ptr3->VisibleLeftWall = 0f;
								Unsafe.Write(&ptr3->FrameOffset, new Point(0, 0));
							}
							else if (ptr3->VisibleRightWall < 0.5f)
							{
								ptr3->VisibleRightWall = 1f;
								Unsafe.Write(&ptr3->FrameOffset, new Point(32, 0));
							}
						}
					}
					ptr3++;
				}
				ptr3 += 4;
			}
			ptr3 = ptr;
			ptr3 += num;
			for (int num8 = 2; num8 < rectangle.Width - 2; num8++)
			{
				for (int num9 = 2; num9 < rectangle.Height - 2; num9++)
				{
					if (ptr3->HasLiquid)
					{
						LiquidCache liquidCache = ptr3[-1];
						_ = ptr3[1];
						LiquidCache liquidCache9 = ptr3[-rectangle.Height];
						LiquidCache liquidCache14 = ptr3[rectangle.Height];
						if (!ptr3->HasBottomEdge && !ptr3->HasLeftEdge && !ptr3->HasTopEdge && !ptr3->HasRightEdge)
						{
							if (liquidCache9.HasTopEdge && liquidCache.HasLeftEdge)
							{
								((Point)(&ptr3->FrameOffset)).X = Math.Max(4, (int)(16f - liquidCache.VisibleLeftWall * 16f)) - 4;
								((Point)(&ptr3->FrameOffset)).Y = 48 + Math.Max(4, (int)(16f - liquidCache9.VisibleTopWall * 16f)) - 4;
								ptr3->VisibleLeftWall = 0f;
								ptr3->VisibleTopWall = 0f;
								ptr3->VisibleRightWall = 1f;
								ptr3->VisibleBottomWall = 1f;
							}
							else if (liquidCache14.HasTopEdge && liquidCache.HasRightEdge)
							{
								((Point)(&ptr3->FrameOffset)).X = 32 - Math.Min(16, (int)(liquidCache.VisibleRightWall * 16f) - 4);
								((Point)(&ptr3->FrameOffset)).Y = 48 + Math.Max(4, (int)(16f - liquidCache14.VisibleTopWall * 16f)) - 4;
								ptr3->VisibleLeftWall = 0f;
								ptr3->VisibleTopWall = 0f;
								ptr3->VisibleRightWall = 1f;
								ptr3->VisibleBottomWall = 1f;
							}
						}
					}
					ptr3++;
				}
				ptr3 += 4;
			}
			ptr3 = ptr;
			ptr3 += num;
			fixed (LiquidDrawCache* ptr4 = &_drawCache[0])
			{
				fixed (Color* ptr10 = &_waveMask[0])
				{
					LiquidDrawCache* ptr5 = ptr4;
					Color* ptr6 = ptr10;
					for (int num10 = 2; num10 < rectangle.Width - 2; num10++)
					{
						for (int num11 = 2; num11 < rectangle.Height - 2; num11++)
						{
							if (ptr3->HasVisibleLiquid)
							{
								float num13 = Math.Min(0.75f, ptr3->VisibleLeftWall);
								float num14 = Math.Max(0.25f, ptr3->VisibleRightWall);
								float num15 = Math.Min(0.75f, ptr3->VisibleTopWall);
								float num16 = Math.Max(0.25f, ptr3->VisibleBottomWall);
								if (ptr3->IsHalfBrick && ptr3->IsSolid && num16 > 0.5f)
								{
									num16 = 0.5f;
								}
								ptr5->IsVisible = ptr3->HasWall || !ptr3->IsHalfBrick || !ptr3->HasLiquid || !(ptr3->LiquidLevel < 1f);
								Unsafe.Write(&ptr5->SourceRectangle, new Rectangle((int)(16f - num14 * 16f) + ((Point)(&ptr3->FrameOffset)).X, (int)(16f - num16 * 16f) + ((Point)(&ptr3->FrameOffset)).Y, (int)Math.Ceiling((num14 - num13) * 16f), (int)Math.Ceiling((num16 - num15) * 16f)));
								ptr5->IsSurfaceLiquid = ((Point)(&ptr3->FrameOffset)).X == 16 && ((Point)(&ptr3->FrameOffset)).Y == 0 && (double)(num11 + rectangle.Y) > Main.worldSurface - 40.0;
								ptr5->Opacity = ptr3->Opacity;
								Unsafe.Write(&ptr5->LiquidOffset, new Vector2((float)Math.Floor(num13 * 16f), (float)Math.Floor(num15 * 16f)));
								ptr5->Type = ptr3->VisibleType;
								ptr5->HasWall = ptr3->HasWall;
								byte b = WAVE_MASK_STRENGTH[ptr3->VisibleType];
								byte b4 = (((Color)ptr6).R = (byte)(b >> 1));
								byte g = b4;
								((Color)ptr6).G = g;
								((Color)ptr6).B = VISCOSITY_MASK[ptr3->VisibleType];
								((Color)ptr6).A = b;
								LiquidCache* ptr7 = ptr3 - 1;
								if (num11 != 2 && !ptr7->HasVisibleLiquid && !ptr7->IsSolid && !ptr7->IsHalfBrick)
								{
									Unsafe.Write((byte*)ptr6 - Unsafe.SizeOf<Color>(), *ptr6);
								}
							}
							else
							{
								ptr5->IsVisible = false;
								int num17 = ((!ptr3->IsSolid && !ptr3->IsHalfBrick) ? 4 : 3);
								byte b2 = WAVE_MASK_STRENGTH[num17];
								byte b4 = (((Color)ptr6).R = (byte)(b2 >> 1));
								byte g2 = b4;
								((Color)ptr6).G = g2;
								((Color)ptr6).B = VISCOSITY_MASK[num17];
								((Color)ptr6).A = b2;
							}
							ptr3++;
							ptr5++;
							ptr6 = (Color*)((byte*)ptr6 + Unsafe.SizeOf<Color>());
						}
						ptr3 += 4;
					}
				}
			}
			ptr3 = ptr;
			for (int num18 = rectangle.X; num18 < rectangle.X + rectangle.Width; num18++)
			{
				for (int num19 = rectangle.Y; num19 < rectangle.Y + rectangle.Height; num19++)
				{
					if (ptr3->VisibleType == 1 && ptr3->HasVisibleLiquid && Dust.lavaBubbles < 200)
					{
						if (_random.Next(700) == 0)
						{
							Dust.NewDust(new Vector2((float)(num18 * 16), (float)(num19 * 16)), 16, 16, 35, 0f, 0f, 0, Color.White);
						}
						if (_random.Next(350) == 0)
						{
							int num20 = Dust.NewDust(new Vector2((float)(num18 * 16), (float)(num19 * 16)), 16, 8, 35, 0f, 0f, 50, Color.White, 1.5f);
							Dust obj = Main.dust[num20];
							obj.velocity *= 0.8f;
							Main.dust[num20].velocity.X *= 2f;
							Main.dust[num20].velocity.Y -= (float)_random.Next(1, 7) * 0.1f;
							if (_random.Next(10) == 0)
							{
								Main.dust[num20].velocity.Y *= _random.Next(2, 5);
							}
							Main.dust[num20].noGravity = true;
						}
					}
					ptr3++;
				}
			}
			fixed (LiquidDrawCache* ptr8 = &_drawCache[0])
			{
				fixed (SpecialLiquidDrawCache* ptr11 = &_drawCacheForShimmer[0])
				{
					LiquidDrawCache* ptr9 = ptr8;
					SpecialLiquidDrawCache* ptr2 = ptr11;
					for (int num21 = 2; num21 < rectangle.Width - 2; num21++)
					{
						for (int num22 = 2; num22 < rectangle.Height - 2; num22++)
						{
							if (ptr9->IsVisible && ptr9->Type == 3)
							{
								ptr2->X = num21;
								ptr2->Y = num22;
								ptr2->IsVisible = ptr9->IsVisible;
								ptr2->HasWall = ptr9->HasWall;
								ptr2->IsSurfaceLiquid = ptr9->IsSurfaceLiquid;
								Unsafe.Write(&ptr2->LiquidOffset, ptr9->LiquidOffset);
								ptr2->Opacity = ptr9->Opacity;
								Unsafe.Write(&ptr2->SourceRectangle, ptr9->SourceRectangle);
								ptr2->Type = ptr9->Type;
								ptr9->IsVisible = false;
								ptr2++;
							}
							ptr9++;
						}
					}
					ptr2->IsVisible = false;
				}
			}
		}
		if (this.WaveFilters != null)
		{
			this.WaveFilters(_waveMask, GetCachedDrawArea());
		}
	}

	public unsafe void DrawNormalLiquids(SpriteBatch spriteBatch, Vector2 drawOffset, int waterStyle, float globalAlpha, bool isBackgroundDraw)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_016d: Unknown result type (might be due to invalid IL or missing references)
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0173: Unknown result type (might be due to invalid IL or missing references)
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0183: Unknown result type (might be due to invalid IL or missing references)
		Rectangle drawArea = _drawArea;
		Main.tileBatch.Begin();
		fixed (LiquidDrawCache* ptr3 = &_drawCache[0])
		{
			LiquidDrawCache* ptr2 = ptr3;
			for (int i = drawArea.X; i < drawArea.X + drawArea.Width; i++)
			{
				for (int j = drawArea.Y; j < drawArea.Y + drawArea.Height; j++)
				{
					if (ptr2->IsVisible)
					{
						Rectangle sourceRectangle = ptr2->SourceRectangle;
						if (ptr2->IsSurfaceLiquid)
						{
							sourceRectangle.Y = 1280;
						}
						else
						{
							sourceRectangle.Y += _animationFrame * 80;
						}
						Vector2 liquidOffset = ptr2->LiquidOffset;
						float num = ptr2->Opacity * (isBackgroundDraw ? 1f : DEFAULT_OPACITY[ptr2->Type]);
						int num2 = ptr2->Type;
						switch (num2)
						{
						case 0:
							num2 = waterStyle;
							num *= globalAlpha;
							break;
						case 2:
							num2 = 11;
							break;
						}
						num = Math.Min(1f, num);
						Lighting.GetCornerColors(i, j, out var vertices);
						ref Color bottomLeftColor = ref vertices.BottomLeftColor;
						bottomLeftColor *= num;
						ref Color bottomRightColor = ref vertices.BottomRightColor;
						bottomRightColor *= num;
						ref Color topLeftColor = ref vertices.TopLeftColor;
						topLeftColor *= num;
						ref Color topRightColor = ref vertices.TopRightColor;
						topRightColor *= num;
						Main.DrawTileInWater(drawOffset, i, j);
						Main.tileBatch.Draw(_liquidTextures[num2].Value, new Vector2((float)(i << 4), (float)(j << 4)) + drawOffset + liquidOffset, sourceRectangle, vertices, Vector2.Zero, 1f, (SpriteEffects)0);
					}
					ptr2++;
				}
			}
		}
		Main.tileBatch.End();
	}

	public unsafe void DrawShimmer(SpriteBatch spriteBatch, Vector2 drawOffset, bool isBackgroundDraw)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
		Rectangle drawArea = _drawArea;
		Main.tileBatch.Begin();
		fixed (SpecialLiquidDrawCache* ptr3 = &_drawCacheForShimmer[0])
		{
			SpecialLiquidDrawCache* ptr2 = ptr3;
			int num = _drawCacheForShimmer.Length;
			for (int i = 0; i < num; i++)
			{
				if (!ptr2->IsVisible)
				{
					break;
				}
				Rectangle sourceRectangle = ptr2->SourceRectangle;
				if (ptr2->IsSurfaceLiquid)
				{
					sourceRectangle.Y = 1280;
				}
				else
				{
					sourceRectangle.Y += _animationFrame * 80;
				}
				Vector2 liquidOffset = ptr2->LiquidOffset;
				float val = ptr2->Opacity * (isBackgroundDraw ? 1f : 0.75f);
				int num2 = 14;
				val = Math.Min(1f, val);
				int num3 = ptr2->X + drawArea.X - 2;
				int num4 = ptr2->Y + drawArea.Y - 2;
				Lighting.GetCornerColors(num3, num4, out var vertices);
				SetShimmerVertexColors(ref vertices, val, num3, num4);
				Main.DrawTileInWater(drawOffset, num3, num4);
				Main.tileBatch.Draw(_liquidTextures[num2].Value, new Vector2((float)(num3 << 4), (float)(num4 << 4)) + drawOffset + liquidOffset, sourceRectangle, vertices, Vector2.Zero, 1f, (SpriteEffects)0);
				sourceRectangle = ptr2->SourceRectangle;
				bool flag = sourceRectangle.X != 16 || sourceRectangle.Y % 80 != 48;
				if (flag || (num3 + num4) % 2 == 0)
				{
					sourceRectangle.X += 48;
					sourceRectangle.Y += 80 * GetShimmerFrame(flag, num3, num4);
					SetShimmerVertexColors_Sparkle(ref vertices, ptr2->Opacity, num3, num4, flag);
					Main.tileBatch.Draw(_liquidTextures[num2].Value, new Vector2((float)(num3 << 4), (float)(num4 << 4)) + drawOffset + liquidOffset, sourceRectangle, vertices, Vector2.Zero, 1f, (SpriteEffects)0);
				}
				ptr2++;
			}
		}
		Main.tileBatch.End();
	}

	public static VertexColors SetShimmerVertexColors_Sparkle(ref VertexColors colors, float opacity, int x, int y, bool top)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		colors.BottomLeftColor = GetShimmerGlitterColor(top, x, y + 1);
		colors.BottomRightColor = GetShimmerGlitterColor(top, x + 1, y + 1);
		colors.TopLeftColor = GetShimmerGlitterColor(top, x, y);
		colors.TopRightColor = GetShimmerGlitterColor(top, x + 1, y);
		ref Color bottomLeftColor = ref colors.BottomLeftColor;
		bottomLeftColor *= opacity;
		ref Color bottomRightColor = ref colors.BottomRightColor;
		bottomRightColor *= opacity;
		ref Color topLeftColor = ref colors.TopLeftColor;
		topLeftColor *= opacity;
		ref Color topRightColor = ref colors.TopRightColor;
		topRightColor *= opacity;
		return colors;
	}

	public static void SetShimmerVertexColors(ref VertexColors colors, float opacity, int x, int y)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		colors.BottomLeftColor = Color.White;
		colors.BottomRightColor = Color.White;
		colors.TopLeftColor = Color.White;
		colors.TopRightColor = Color.White;
		ref Color bottomLeftColor = ref colors.BottomLeftColor;
		bottomLeftColor *= opacity;
		ref Color bottomRightColor = ref colors.BottomRightColor;
		bottomRightColor *= opacity;
		ref Color topLeftColor = ref colors.TopLeftColor;
		topLeftColor *= opacity;
		ref Color topRightColor = ref colors.TopRightColor;
		topRightColor *= opacity;
		colors.BottomLeftColor = new Color(((Color)(ref colors.BottomLeftColor)).ToVector4() * GetShimmerBaseColor(x, y + 1));
		colors.BottomRightColor = new Color(((Color)(ref colors.BottomRightColor)).ToVector4() * GetShimmerBaseColor(x + 1, y + 1));
		colors.TopLeftColor = new Color(((Color)(ref colors.TopLeftColor)).ToVector4() * GetShimmerBaseColor(x, y));
		colors.TopRightColor = new Color(((Color)(ref colors.TopRightColor)).ToVector4() * GetShimmerBaseColor(x + 1, y));
	}

	public static float GetShimmerWave(ref float worldPositionX, ref float worldPositionY)
	{
		return (float)Math.Sin(((double)((worldPositionX + worldPositionY / 6f) / 10f) - Main.timeForVisualEffects / 360.0) * 6.2831854820251465);
	}

	public static Color GetShimmerGlitterColor(bool top, float worldPositionX, float worldPositionY)
	{
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		Color color = Main.hslToRgb((float)(((double)(worldPositionX + worldPositionY / 6f) + Main.timeForVisualEffects / 30.0) / 6.0) % 1f, 1f, 0.5f);
		((Color)(ref color)).A = 0;
		return new Color(((Color)(ref color)).ToVector4() * GetShimmerGlitterOpacity(top, worldPositionX, worldPositionY));
	}

	public static float GetShimmerGlitterOpacity(bool top, float worldPositionX, float worldPositionY)
	{
		if (top)
		{
			return 0.5f;
		}
		float num3 = Utils.Remap((float)Math.Sin(((double)((worldPositionX + worldPositionY / 6f) / 10f) - Main.timeForVisualEffects / 360.0) * 6.2831854820251465), -0.5f, 1f, 0f, 0.35f);
		float num2 = (float)Math.Sin((double)((float)SimpleWhiteNoise((uint)worldPositionX, (uint)worldPositionY) / 10f) + Main.timeForVisualEffects / 180.0);
		return Utils.Remap(num3 * num2, 0f, 0.5f, 0f, 1f);
	}

	private static uint SimpleWhiteNoise(uint x, uint y)
	{
		x = 36469 * (x & 0xFFFF) + (x >> 16);
		y = 18012 * (y & 0xFFFF) + (y >> 16);
		return (x << 16) + y;
	}

	public int GetShimmerFrame(bool top, float worldPositionX, float worldPositionY)
	{
		worldPositionX += 0.5f;
		worldPositionY += 0.5f;
		double num = (double)((worldPositionX + worldPositionY / 6f) / 10f) - Main.timeForVisualEffects / 360.0;
		if (!top)
		{
			num += (double)(worldPositionX + worldPositionY);
		}
		return ((int)num % 16 + 16) % 16;
	}

	public static Vector4 GetShimmerBaseColor(float worldPositionX, float worldPositionY)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		float shimmerWave = GetShimmerWave(ref worldPositionX, ref worldPositionY);
		return Vector4.Lerp(new Vector4(0.64705884f, 26f / 51f, 14f / 15f, 1f), new Vector4(41f / 51f, 41f / 51f, 1f, 1f), 0.1f + shimmerWave * 0.4f);
	}

	public bool HasFullWater(int x, int y)
	{
		x -= _drawArea.X;
		y -= _drawArea.Y;
		int num = x * _drawArea.Height + y;
		if (num >= 0 && num < _drawCache.Length)
		{
			if (_drawCache[num].IsVisible)
			{
				return !_drawCache[num].IsSurfaceLiquid;
			}
			return false;
		}
		return true;
	}

	public float GetVisibleLiquid(int x, int y)
	{
		x -= _drawArea.X;
		y -= _drawArea.Y;
		if (x < 0 || x >= _drawArea.Width || y < 0 || y >= _drawArea.Height)
		{
			return 0f;
		}
		int num = (x + 2) * (_drawArea.Height + 4) + y + 2;
		if (!_cache[num].HasVisibleLiquid)
		{
			return 0f;
		}
		return _cache[num].VisibleLiquidLevel;
	}

	public void Update(GameTime gameTime)
	{
		if (!Main.gamePaused && Main.hasFocus)
		{
			float num = Main.windSpeedCurrent * 25f;
			num = ((!(num < 0f)) ? (num + 6f) : (num - 6f));
			_frameState += num * (float)gameTime.ElapsedGameTime.TotalSeconds;
			if (_frameState < 0f)
			{
				_frameState += 16f;
			}
			_frameState %= 16f;
			_animationFrame = (int)_frameState;
		}
	}

	public void PrepareDraw(Rectangle drawArea)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		InternalPrepareDraw(drawArea);
	}

	public void SetWaveMaskData(ref Texture2D texture)
	{
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Expected O, but got Unknown
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Expected O, but got Unknown
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (texture == null || texture.Width < _drawArea.Height || texture.Height < _drawArea.Width)
			{
				Console.WriteLine("WaveMaskData texture recreated. {0}x{1}", _drawArea.Height, _drawArea.Width);
				if (texture != null)
				{
					try
					{
						((GraphicsResource)texture).Dispose();
					}
					catch
					{
					}
				}
				texture = new Texture2D(((Game)Main.instance).GraphicsDevice, _drawArea.Height, _drawArea.Width, false, (SurfaceFormat)0);
			}
			texture.SetData<Color>(0, (Rectangle?)new Rectangle(0, 0, _drawArea.Height, _drawArea.Width), _waveMask, 0, _drawArea.Width * _drawArea.Height);
		}
		catch
		{
			texture = new Texture2D(((Game)Main.instance).GraphicsDevice, _drawArea.Height, _drawArea.Width, false, (SurfaceFormat)0);
			texture.SetData<Color>(0, (Rectangle?)new Rectangle(0, 0, _drawArea.Height, _drawArea.Width), _waveMask, 0, _drawArea.Width * _drawArea.Height);
		}
	}

	public Rectangle GetCachedDrawArea()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		return _drawArea;
	}
}
