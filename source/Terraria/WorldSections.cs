using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace Terraria;

public class WorldSections
{
	private struct IterationState
	{
		public Vector2 centerPos;

		public int X;

		public int Y;

		public int leg;

		public int xDir;

		public int yDir;

		public void Reset()
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			centerPos = new Vector2(-3200f, -2400f);
			X = 0;
			Y = 0;
			leg = 0;
			xDir = 0;
			yDir = 0;
		}
	}

	public const int BitIndex_SectionLoaded = 0;

	public const int BitIndex_SectionFramed = 1;

	public const int BitIndex_SectionMapDrawn = 2;

	public const int BitIndex_SectionNeedsRefresh = 3;

	private int width;

	private int height;

	private BitsByte[] data;

	private int mapSectionsLeft;

	private int frameSectionsLeft;

	private int _sectionsNeedingRefresh;

	private IterationState prevFrame;

	private IterationState prevMap;

	public bool AnyUnfinishedSections => frameSectionsLeft > 0;

	public bool AnyNeedRefresh => _sectionsNeedingRefresh > 0;

	public WorldSections(int numSectionsX, int numSectionsY)
	{
		width = numSectionsX;
		height = numSectionsY;
		data = new BitsByte[width * height];
		mapSectionsLeft = width * height;
		prevFrame.Reset();
		prevMap.Reset();
	}

	public void SetSectionAsRefreshed(int x, int y)
	{
		if (x >= 0)
		{
			_ = width;
		}
		if (y >= 0)
		{
			_ = height;
		}
		if (data[y * width + x][3])
		{
			data[y * width + x][3] = false;
			_sectionsNeedingRefresh--;
		}
	}

	public bool SectionNeedsRefresh(int x, int y)
	{
		if (x < 0 || x >= width)
		{
			return false;
		}
		if (y < 0 || y >= height)
		{
			return false;
		}
		return data[y * width + x][3];
	}

	public void SetAllFramedSectionsAsNeedingRefresh()
	{
		for (int i = 0; i < data.Length; i++)
		{
			if (data[i][1])
			{
				data[i][3] = true;
				_sectionsNeedingRefresh++;
			}
		}
	}

	public bool SectionLoaded(int x, int y)
	{
		if (x < 0 || x >= width)
		{
			return false;
		}
		if (y < 0 || y >= height)
		{
			return false;
		}
		return data[y * width + x][0];
	}

	public bool SectionFramed(int x, int y)
	{
		if (x < 0 || x >= width)
		{
			return false;
		}
		if (y < 0 || y >= height)
		{
			return false;
		}
		return data[y * width + x][1];
	}

	public bool MapSectionDrawn(int x, int y)
	{
		if (x < 0 || x >= width)
		{
			return false;
		}
		if (y < 0 || y >= height)
		{
			return false;
		}
		return data[y * width + x][2];
	}

	public void ClearMapDraw()
	{
		for (int i = 0; i < data.Length; i++)
		{
			data[i][2] = false;
		}
		prevMap.Reset();
		mapSectionsLeft = data.Length;
	}

	public void SetSectionFramed(int x, int y)
	{
		if (x >= 0 && x < width && y >= 0 && y < height)
		{
			BitsByte bitsByte = data[y * width + x];
			if (bitsByte[0] && !bitsByte[1])
			{
				bitsByte[1] = true;
				data[y * width + x] = bitsByte;
				frameSectionsLeft--;
			}
		}
	}

	public void SetSectionLoaded(int x, int y)
	{
		if (x >= 0 && x < width && y >= 0 && y < height)
		{
			SetSectionLoaded(ref data[y * width + x]);
		}
	}

	private void SetSectionLoaded(ref BitsByte section)
	{
		if (!section[0])
		{
			section[0] = true;
			frameSectionsLeft++;
		}
		else if (section[1])
		{
			section[1] = false;
			frameSectionsLeft++;
		}
	}

	public void SetAllSectionsLoaded()
	{
		for (int i = 0; i < data.Length; i++)
		{
			SetSectionLoaded(ref data[i]);
		}
	}

	public void SetTilesLoaded(int startX, int startY, int endXInclusive, int endYInclusive)
	{
		int sectionX3 = Netplay.GetSectionX(startX);
		int sectionY = Netplay.GetSectionY(startY);
		int sectionX2 = Netplay.GetSectionX(endXInclusive);
		int sectionY2 = Netplay.GetSectionY(endYInclusive);
		for (int i = sectionX3; i <= sectionX2; i++)
		{
			for (int j = sectionY; j <= sectionY2; j++)
			{
				SetSectionLoaded(i, j);
			}
		}
	}

	public bool GetNextMapDraw(Vector2 playerPos, out int x, out int y)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d3: Unknown result type (might be due to invalid IL or missing references)
		if (mapSectionsLeft <= 0)
		{
			x = -1;
			y = -1;
			return false;
		}
		Stopwatch stopwatch = new Stopwatch();
		stopwatch.Start();
		int num = 0;
		int num3 = 0;
		Vector2 vector = prevMap.centerPos;
		playerPos *= 0.0625f;
		int sectionX = Netplay.GetSectionX((int)playerPos.X);
		int sectionY = Netplay.GetSectionY((int)playerPos.Y);
		int num4 = Netplay.GetSectionX((int)vector.X);
		int num5 = Netplay.GetSectionY((int)vector.Y);
		int num6;
		if (num4 != sectionX || num5 != sectionY)
		{
			vector = playerPos;
			num4 = sectionX;
			num5 = sectionY;
			num6 = 4;
			x = sectionX;
			y = sectionY;
		}
		else
		{
			num6 = prevMap.leg;
			x = prevMap.X;
			y = prevMap.Y;
			num = prevMap.xDir;
			num3 = prevMap.yDir;
		}
		int num7 = (int)(playerPos.X - ((float)num4 + 0.5f) * 200f);
		int num8 = (int)(playerPos.Y - ((float)num5 + 0.5f) * 150f);
		if (num == 0)
		{
			num = ((num7 <= 0) ? 1 : (-1));
			num3 = ((num8 <= 0) ? 1 : (-1));
		}
		int num9 = 0;
		bool flag = false;
		bool flag2 = false;
		while (true)
		{
			if (num9 == 4)
			{
				if (flag2)
				{
					throw new Exception("Infinite loop in WorldSections.GetNextMapDraw");
				}
				flag2 = true;
				x = num4;
				y = num5;
				num7 = (int)(vector.X - ((float)num4 + 0.5f) * 200f);
				num8 = (int)(vector.Y - ((float)num5 + 0.5f) * 150f);
				num = ((num7 <= 0) ? 1 : (-1));
				num3 = ((num8 <= 0) ? 1 : (-1));
				num6 = 4;
				num9 = 0;
			}
			if (y >= 0 && y < height && x >= 0 && x < width)
			{
				flag = false;
				if (!data[y * width + x][2])
				{
					break;
				}
			}
			int num10 = x - num4;
			int num2 = y - num5;
			if (num10 == 0 || num2 == 0)
			{
				if (num6 == 4)
				{
					if (num10 == 0 && num2 == 0)
					{
						if (Math.Abs(num7) > Math.Abs(num8))
						{
							y -= num3;
						}
						else
						{
							x -= num;
						}
					}
					else
					{
						if (num10 != 0)
						{
							x += num10 / Math.Abs(num10);
						}
						if (num2 != 0)
						{
							y += num2 / Math.Abs(num2);
						}
					}
					num6 = 0;
					num9 = -2;
					flag = true;
				}
				else
				{
					if (num10 != 0)
					{
						num = ((num10 <= 0) ? 1 : (-1));
					}
					else
					{
						num3 = ((num2 <= 0) ? 1 : (-1));
					}
					x += num;
					y += num3;
					num6++;
				}
				if (flag)
				{
					num9++;
				}
				else
				{
					flag = true;
				}
			}
			else
			{
				x += num;
				y += num3;
			}
		}
		data[y * width + x][2] = true;
		mapSectionsLeft--;
		prevMap.centerPos = playerPos;
		prevMap.X = x;
		prevMap.Y = y;
		prevMap.leg = num6;
		prevMap.xDir = num;
		prevMap.yDir = num3;
		stopwatch.Stop();
		return true;
	}

	public bool TileLoaded(int tileX, int tileY)
	{
		return SectionLoaded(Netplay.GetSectionX(tileX), Netplay.GetSectionY(tileY));
	}

	public bool TilesLoaded(int startX, int startY, int endXInclusive, int endYInclusive)
	{
		int sectionX = Netplay.GetSectionX(startX);
		int sY = Netplay.GetSectionY(startY);
		int eX = Netplay.GetSectionX(endXInclusive);
		int eY = Netplay.GetSectionY(endYInclusive);
		for (int x = sectionX; x <= eX; x++)
		{
			for (int y = sY; y <= eY; y++)
			{
				if (!SectionLoaded(x, y))
				{
					return false;
				}
			}
		}
		return true;
	}
}
