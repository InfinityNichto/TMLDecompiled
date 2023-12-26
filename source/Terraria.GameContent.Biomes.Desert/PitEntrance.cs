using System;
using Microsoft.Xna.Framework;

namespace Terraria.GameContent.Biomes.Desert;

public static class PitEntrance
{
	public static void Place(DesertDescription description)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		int holeRadius = WorldGen.genRand.Next(6, 9);
		Rectangle combinedArea = description.CombinedArea;
		Point center = ((Rectangle)(ref combinedArea)).Center;
		center.Y = description.Surface[center.X];
		PlaceAt(description, center, holeRadius);
	}

	private static void PlaceAt(DesertDescription description, Point position, int holeRadius)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0165: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		for (int i = -holeRadius - 3; i < holeRadius + 3; i++)
		{
			int j = description.Surface[i + position.X];
			while (true)
			{
				int num4 = j;
				Rectangle val = description.Hive;
				if (num4 > ((Rectangle)(ref val)).Top + 10)
				{
					break;
				}
				double num5 = j - description.Surface[i + position.X];
				val = description.Hive;
				int top = ((Rectangle)(ref val)).Top;
				val = description.Desert;
				double value = num5 / (double)(top - ((Rectangle)(ref val)).Top);
				value = Utils.Clamp(value, 0.0, 1.0);
				int num = (int)(GetHoleRadiusScaleAt(value) * (double)holeRadius);
				if (Math.Abs(i) < num)
				{
					Main.tile[i + position.X, j].ClearEverything();
				}
				else if (Math.Abs(i) < num + 3 && value > 0.35)
				{
					Main.tile[i + position.X, j].ResetToType(397);
				}
				double num2 = Math.Abs((double)i / (double)holeRadius);
				num2 *= num2;
				if (Math.Abs(i) < num + 3 && (double)(j - position.Y) > 15.0 - 3.0 * num2)
				{
					Main.tile[i + position.X, j].wall = 187;
					WorldGen.SquareWallFrame(i + position.X, j - 1);
					WorldGen.SquareWallFrame(i + position.X, j);
				}
				j++;
			}
		}
		holeRadius += 4;
		for (int k = -holeRadius; k < holeRadius; k++)
		{
			int num3 = holeRadius - Math.Abs(k);
			num3 = Math.Min(10, num3 * num3);
			for (int l = 0; l < num3; l++)
			{
				Main.tile[k + position.X, l + description.Surface[k + position.X]].ClearEverything();
			}
		}
	}

	private static double GetHoleRadiusScaleAt(double yProgress)
	{
		if (yProgress < 0.6)
		{
			return 1.0;
		}
		return (1.0 - SmootherStep((yProgress - 0.6) / 0.4)) * 0.5 + 0.5;
	}

	private static double SmootherStep(double delta)
	{
		delta = Utils.Clamp(delta, 0.0, 1.0);
		return 1.0 - Math.Cos(delta * 3.1415927410125732) * 0.5 - 0.5;
	}
}
