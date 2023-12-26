using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria.ID;

namespace Terraria.WorldBuilding;

public class StructureMap
{
	private readonly List<Rectangle> _structures = new List<Rectangle>(2048);

	private readonly List<Rectangle> _protectedStructures = new List<Rectangle>(2048);

	private readonly object _lock = new object();

	public bool CanPlace(Rectangle area, int padding = 0)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		return CanPlace(area, TileID.Sets.GeneralPlacementTiles, padding);
	}

	public bool CanPlace(Rectangle area, bool[] validTiles, int padding = 0)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		lock (_lock)
		{
			if (area.X < 0 || area.Y < 0 || area.X + area.Width > Main.maxTilesX - 1 || area.Y + area.Height > Main.maxTilesY - 1)
			{
				return false;
			}
			Rectangle rectangle = default(Rectangle);
			((Rectangle)(ref rectangle))._002Ector(area.X - padding, area.Y - padding, area.Width + padding * 2, area.Height + padding * 2);
			for (int i = 0; i < _protectedStructures.Count; i++)
			{
				if (((Rectangle)(ref rectangle)).Intersects(_protectedStructures[i]))
				{
					return false;
				}
			}
			for (int j = rectangle.X; j < rectangle.X + rectangle.Width; j++)
			{
				for (int k = rectangle.Y; k < rectangle.Y + rectangle.Height; k++)
				{
					if (Main.tile[j, k].active())
					{
						ushort type = Main.tile[j, k].type;
						if (!validTiles[type])
						{
							return false;
						}
					}
				}
			}
			return true;
		}
	}

	public Rectangle GetBoundingBox()
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		lock (_lock)
		{
			if (_structures.Count == 0)
			{
				return Rectangle.Empty;
			}
			Point point = default(Point);
			((Point)(ref point))._002Ector(_structures.Min((Rectangle rect) => ((Rectangle)(ref rect)).Left), _structures.Min((Rectangle rect) => ((Rectangle)(ref rect)).Top));
			Point point2 = default(Point);
			((Point)(ref point2))._002Ector(_structures.Max((Rectangle rect) => ((Rectangle)(ref rect)).Right), _structures.Max((Rectangle rect) => ((Rectangle)(ref rect)).Bottom));
			return new Rectangle(point.X, point.Y, point2.X - point.X, point2.Y - point.Y);
		}
	}

	public void AddStructure(Rectangle area, int padding = 0)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		lock (_lock)
		{
			((Rectangle)(ref area)).Inflate(padding, padding);
			_structures.Add(area);
		}
	}

	public void AddProtectedStructure(Rectangle area, int padding = 0)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		lock (_lock)
		{
			((Rectangle)(ref area)).Inflate(padding, padding);
			_structures.Add(area);
			_protectedStructures.Add(area);
		}
	}

	public void Reset()
	{
		lock (_lock)
		{
			_protectedStructures.Clear();
		}
	}
}
