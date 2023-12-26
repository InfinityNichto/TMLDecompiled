using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using ReLogic.Utilities;
using Terraria.WorldBuilding;

namespace Terraria.GameContent.Generation;

public class ShapeBranch : GenShape
{
	private Point _offset;

	private List<Point> _endPoints;

	public ShapeBranch()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		_offset = new Point(10, -5);
	}

	public ShapeBranch(Point offset)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		_offset = offset;
	}

	public ShapeBranch(double angle, double distance)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		_offset = new Point((int)(Math.Cos(angle) * distance), (int)(Math.Sin(angle) * distance));
	}

	private bool PerformSegment(Point origin, GenAction action, Point start, Point end, int size)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		size = Math.Max(1, size);
		for (int i = -(size >> 1); i < size - (size >> 1); i++)
		{
			for (int j = -(size >> 1); j < size - (size >> 1); j++)
			{
				if (!Utils.PlotLine(new Point(start.X + i, start.Y + j), end, (int tileX, int tileY) => UnitApply(action, origin, tileX, tileY) || !_quitOnFail, jump: false))
				{
					return false;
				}
			}
		}
		return true;
	}

	public override bool Perform(Point origin, GenAction action)
	{
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		//IL_0198: Unknown result type (might be due to invalid IL or missing references)
		//IL_01da: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0204: Unknown result type (might be due to invalid IL or missing references)
		//IL_020b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0212: Unknown result type (might be due to invalid IL or missing references)
		//IL_0219: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
		double num = new Vector2D(_offset.X, _offset.Y).Length();
		int num2 = (int)(num / 6.0);
		if (_endPoints != null)
		{
			_endPoints.Add(new Point(origin.X + _offset.X, origin.Y + _offset.Y));
		}
		if (!PerformSegment(origin, action, origin, new Point(origin.X + _offset.X, origin.Y + _offset.Y), num2))
		{
			return false;
		}
		int num3 = (int)(num / 8.0);
		Point point = default(Point);
		Point point2 = default(Point);
		for (int i = 0; i < num3; i++)
		{
			double num4 = ((double)i + 1.0) / ((double)num3 + 1.0);
			((Point)(ref point))._002Ector((int)(num4 * (double)_offset.X), (int)(num4 * (double)_offset.Y));
			Vector2D spinningpoint = new Vector2D(_offset.X - point.X, _offset.Y - point.Y);
			spinningpoint = spinningpoint.RotatedBy((GenBase._random.NextDouble() * 0.5 + 1.0) * (double)((GenBase._random.Next(2) != 0) ? 1 : (-1))) * 0.75;
			((Point)(ref point2))._002Ector((int)spinningpoint.X + point.X, (int)spinningpoint.Y + point.Y);
			if (_endPoints != null)
			{
				_endPoints.Add(new Point(point2.X + origin.X, point2.Y + origin.Y));
			}
			if (!PerformSegment(origin, action, new Point(point.X + origin.X, point.Y + origin.Y), new Point(point2.X + origin.X, point2.Y + origin.Y), num2 - 1))
			{
				return false;
			}
		}
		return true;
	}

	public ShapeBranch OutputEndpoints(List<Point> endpoints)
	{
		_endPoints = endpoints;
		return this;
	}
}
