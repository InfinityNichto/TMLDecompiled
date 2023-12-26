using System;
using Microsoft.Xna.Framework;
using ReLogic.Utilities;

namespace Terraria.WorldBuilding;

public static class Shapes
{
	public class Circle : GenShape
	{
		private int _verticalRadius;

		private int _horizontalRadius;

		public Circle(int radius)
		{
			_verticalRadius = radius;
			_horizontalRadius = radius;
		}

		public Circle(int horizontalRadius, int verticalRadius)
		{
			_horizontalRadius = horizontalRadius;
			_verticalRadius = verticalRadius;
		}

		public void SetRadius(int radius)
		{
			_verticalRadius = radius;
			_horizontalRadius = radius;
		}

		public override bool Perform(Point origin, GenAction action)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			int num = (_horizontalRadius + 1) * (_horizontalRadius + 1);
			for (int i = origin.Y - _verticalRadius; i <= origin.Y + _verticalRadius; i++)
			{
				double num2 = (double)_horizontalRadius / (double)_verticalRadius * (double)(i - origin.Y);
				int num3 = Math.Min(_horizontalRadius, (int)Math.Sqrt((double)num - num2 * num2));
				for (int j = origin.X - num3; j <= origin.X + num3; j++)
				{
					if (!UnitApply(action, origin, j, i) && _quitOnFail)
					{
						return false;
					}
				}
			}
			return true;
		}
	}

	public class HalfCircle : GenShape
	{
		private int _radius;

		public HalfCircle(int radius)
		{
			_radius = radius;
		}

		public override bool Perform(Point origin, GenAction action)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			int num = (_radius + 1) * (_radius + 1);
			for (int i = origin.Y - _radius; i <= origin.Y; i++)
			{
				int num2 = Math.Min(_radius, (int)Math.Sqrt(num - (i - origin.Y) * (i - origin.Y)));
				for (int j = origin.X - num2; j <= origin.X + num2; j++)
				{
					if (!UnitApply(action, origin, j, i) && _quitOnFail)
					{
						return false;
					}
				}
			}
			return true;
		}
	}

	public class Slime : GenShape
	{
		private int _radius;

		private double _xScale;

		private double _yScale;

		public Slime(int radius)
		{
			_radius = radius;
			_xScale = 1.0;
			_yScale = 1.0;
		}

		public Slime(int radius, double xScale, double yScale)
		{
			_radius = radius;
			_xScale = xScale;
			_yScale = yScale;
		}

		public override bool Perform(Point origin, GenAction action)
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_014d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0108: Unknown result type (might be due to invalid IL or missing references)
			//IL_013a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0117: Unknown result type (might be due to invalid IL or missing references)
			double num = _radius;
			int num2 = (_radius + 1) * (_radius + 1);
			for (int i = origin.Y - (int)(num * _yScale); i <= origin.Y; i++)
			{
				double num3 = (double)(i - origin.Y) / _yScale;
				int num4 = (int)Math.Min((double)_radius * _xScale, _xScale * Math.Sqrt((double)num2 - num3 * num3));
				for (int j = origin.X - num4; j <= origin.X + num4; j++)
				{
					if (!UnitApply(action, origin, j, i) && _quitOnFail)
					{
						return false;
					}
				}
			}
			for (int k = origin.Y + 1; k <= origin.Y + (int)(num * _yScale * 0.5) - 1; k++)
			{
				double num5 = (double)(k - origin.Y) * (2.0 / _yScale);
				int num6 = (int)Math.Min((double)_radius * _xScale, _xScale * Math.Sqrt((double)num2 - num5 * num5));
				for (int l = origin.X - num6; l <= origin.X + num6; l++)
				{
					if (!UnitApply(action, origin, l, k) && _quitOnFail)
					{
						return false;
					}
				}
			}
			return true;
		}
	}

	public class Rectangle : GenShape
	{
		private Rectangle _area;

		public Rectangle(Rectangle area)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			_area = area;
		}

		public Rectangle(int width, int height)
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			_area = new Rectangle(0, 0, width, height);
		}

		public void SetArea(Rectangle area)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			_area = area;
		}

		public override bool Perform(Point origin, GenAction action)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			for (int i = origin.X + ((Rectangle)(ref _area)).Left; i < origin.X + ((Rectangle)(ref _area)).Right; i++)
			{
				for (int j = origin.Y + ((Rectangle)(ref _area)).Top; j < origin.Y + ((Rectangle)(ref _area)).Bottom; j++)
				{
					if (!UnitApply(action, origin, i, j) && _quitOnFail)
					{
						return false;
					}
				}
			}
			return true;
		}
	}

	public class Tail : GenShape
	{
		private double _width;

		private Vector2D _endOffset;

		public Tail(double width, Vector2D endOffset)
		{
			_width = width * 16.0;
			_endOffset = endOffset * 16.0;
		}

		public override bool Perform(Point origin, GenAction action)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			Vector2D vector2D = new Vector2D(origin.X << 4, origin.Y << 4);
			return Utils.PlotTileTale(vector2D, vector2D + _endOffset, _width, (int x, int y) => UnitApply(action, origin, x, y) || !_quitOnFail);
		}
	}

	public class Mound : GenShape
	{
		private int _halfWidth;

		private int _height;

		public Mound(int halfWidth, int height)
		{
			_halfWidth = halfWidth;
			_height = height;
		}

		public override bool Perform(Point origin, GenAction action)
		{
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			_ = _height;
			double num = _halfWidth;
			for (int i = -_halfWidth; i <= _halfWidth; i++)
			{
				int num2 = Math.Min(_height, (int)((0.0 - (double)(_height + 1) / (num * num)) * ((double)i + num) * ((double)i - num)));
				for (int j = 0; j < num2; j++)
				{
					if (!UnitApply(action, origin, i + origin.X, origin.Y - j) && _quitOnFail)
					{
						return false;
					}
				}
			}
			return true;
		}
	}
}
