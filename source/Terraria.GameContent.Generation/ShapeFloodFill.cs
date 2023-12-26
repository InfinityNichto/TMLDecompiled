using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.WorldBuilding;

namespace Terraria.GameContent.Generation;

public class ShapeFloodFill : GenShape
{
	private int _maximumActions;

	public ShapeFloodFill(int maximumActions = 100)
	{
		_maximumActions = maximumActions;
	}

	public override bool Perform(Point origin, GenAction action)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		Queue<Point> queue = new Queue<Point>();
		HashSet<Point16> hashSet = new HashSet<Point16>();
		queue.Enqueue(origin);
		int num = _maximumActions;
		while (queue.Count > 0 && num > 0)
		{
			Point point = queue.Dequeue();
			if (!hashSet.Contains(new Point16(point.X, point.Y)) && UnitApply(action, origin, point.X, point.Y))
			{
				hashSet.Add(new Point16(point));
				num--;
				if (point.X + 1 < Main.maxTilesX - 1)
				{
					queue.Enqueue(new Point(point.X + 1, point.Y));
				}
				if (point.X - 1 >= 1)
				{
					queue.Enqueue(new Point(point.X - 1, point.Y));
				}
				if (point.Y + 1 < Main.maxTilesY - 1)
				{
					queue.Enqueue(new Point(point.X, point.Y + 1));
				}
				if (point.Y - 1 >= 1)
				{
					queue.Enqueue(new Point(point.X, point.Y - 1));
				}
			}
		}
		while (queue.Count > 0)
		{
			Point item = queue.Dequeue();
			if (!hashSet.Contains(new Point16(item.X, item.Y)))
			{
				queue.Enqueue(item);
				break;
			}
		}
		return queue.Count == 0;
	}
}
