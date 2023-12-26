using System;
using Microsoft.Xna.Framework;
using ReLogic.Utilities;
using Terraria.WorldBuilding;

namespace Terraria.GameContent.Generation;

public class ShapeRunner : GenShape
{
	private double _startStrength;

	private int _steps;

	private Vector2D _startVelocity;

	public ShapeRunner(double strength, int steps, Vector2D velocity)
	{
		_startStrength = strength;
		_steps = steps;
		_startVelocity = velocity;
	}

	public override bool Perform(Point origin, GenAction action)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		double num = _steps;
		double num2 = _steps;
		double num3 = _startStrength;
		Vector2D vector2D = new Vector2D(origin.X, origin.Y);
		Vector2D vector2D2 = ((_startVelocity == Vector2D.Zero) ? Utils.RandomVector2D(GenBase._random, -1.0, 1.0) : _startVelocity);
		while (num > 0.0 && num3 > 0.0)
		{
			num3 = _startStrength * (num / num2);
			num -= 1.0;
			int num8 = Math.Max(1, (int)(vector2D.X - num3 * 0.5));
			int num4 = Math.Max(1, (int)(vector2D.Y - num3 * 0.5));
			int num5 = Math.Min(GenBase._worldWidth, (int)(vector2D.X + num3 * 0.5));
			int num6 = Math.Min(GenBase._worldHeight, (int)(vector2D.Y + num3 * 0.5));
			for (int i = num8; i < num5; i++)
			{
				for (int j = num4; j < num6; j++)
				{
					if (!(Math.Abs((double)i - vector2D.X) + Math.Abs((double)j - vector2D.Y) >= num3 * 0.5 * (1.0 + (double)GenBase._random.Next(-10, 11) * 0.015)))
					{
						UnitApply(action, origin, i, j);
					}
				}
			}
			int num7 = (int)(num3 / 50.0) + 1;
			num -= (double)num7;
			vector2D += vector2D2;
			for (int k = 0; k < num7; k++)
			{
				vector2D += vector2D2;
				vector2D2 += Utils.RandomVector2D(GenBase._random, -0.5, 0.5);
			}
			vector2D2 += Utils.RandomVector2D(GenBase._random, -0.5, 0.5);
			vector2D2 = Vector2D.Clamp(vector2D2, -Vector2D.One, Vector2D.One);
		}
		return true;
	}
}
