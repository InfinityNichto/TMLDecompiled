using System;
using Microsoft.Xna.Framework;

namespace Terraria.GameContent.Biomes.Desert;

public static class SandMound
{
	public static void Place(DesertDescription description)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_021b: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_019b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01af: Unknown result type (might be due to invalid IL or missing references)
		Rectangle desert = description.Desert;
		desert.Height = Math.Min(description.Desert.Height, description.Hive.Height / 2);
		Rectangle desert2 = description.Desert;
		desert2.Y = ((Rectangle)(ref desert)).Bottom;
		Rectangle desert3 = description.Desert;
		desert2.Height = Math.Max(0, ((Rectangle)(ref desert3)).Bottom - ((Rectangle)(ref desert)).Bottom);
		SurfaceMap surface = description.Surface;
		int num = 0;
		int num2 = 0;
		for (int i = -5; i < desert.Width + 5; i++)
		{
			double value = Math.Abs((double)(i + 5) / (double)(desert.Width + 10)) * 2.0 - 1.0;
			value = Utils.Clamp(value, -1.0, 1.0);
			if (i % 3 == 0)
			{
				num += WorldGen.genRand.Next(-1, 2);
				num = Utils.Clamp(num, -10, 10);
			}
			num2 += WorldGen.genRand.Next(-1, 2);
			num2 = Utils.Clamp(num2, -10, 10);
			double num3 = Math.Sqrt(1.0 - value * value * value * value);
			int num4 = ((Rectangle)(ref desert)).Bottom - (int)(num3 * (double)desert.Height) + num;
			if (Math.Abs(value) < 1.0)
			{
				double num5 = Utils.UnclampedSmoothStep(0.5, 0.8, Math.Abs(value));
				num5 = num5 * num5 * num5;
				int val = 10 + (int)((double)((Rectangle)(ref desert)).Top - num5 * 20.0) + num2;
				val = Math.Min(val, num4);
				for (int j = surface[i + desert.X] - 1; j < val; j++)
				{
					int num6 = i + desert.X;
					int num7 = j;
					Main.tile[num6, num7].active(active: false);
					Main.tile[num6, num7].wall = 0;
				}
			}
			PlaceSandColumn(i + desert.X, num4, ((Rectangle)(ref desert2)).Bottom - num4);
		}
	}

	private static void PlaceSandColumn(int startX, int startY, int height)
	{
		for (int num = startY + height - 1; num >= startY; num--)
		{
			int num2 = num;
			Tile tile = Main.tile[startX, num2];
			if (!WorldGen.remixWorldGen)
			{
				tile.liquid = 0;
			}
			_ = Main.tile[startX, num2 + 1];
			_ = Main.tile[startX, num2 + 2];
			tile.type = 53;
			tile.slope(0);
			tile.halfBrick(halfBrick: false);
			tile.active(active: true);
			if (num < startY)
			{
				tile.active(active: false);
			}
			WorldGen.SquareWallFrame(startX, num2);
		}
	}
}
