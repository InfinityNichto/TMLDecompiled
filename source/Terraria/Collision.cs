using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.ID;

namespace Terraria;

public class Collision
{
	public struct HurtTile
	{
		public int type;

		public int x;

		public int y;
	}

	public static bool stair;

	public static bool stairFall;

	public static bool honey;

	public static bool shimmer;

	public static bool sloping;

	public static bool landMine = false;

	public static bool up;

	public static bool down;

	public static float Epsilon = (float)Math.E;

	private static List<Point> _cacheForConveyorBelts = new List<Point>();

	public static Vector2[] CheckLinevLine(Vector2 a1, Vector2 a2, Vector2 b1, Vector2 b2)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		if (((Vector2)(ref a1)).Equals(a2) && ((Vector2)(ref b1)).Equals(b2))
		{
			if (((Vector2)(ref a1)).Equals(b1))
			{
				return (Vector2[])(object)new Vector2[1] { a1 };
			}
			return (Vector2[])(object)new Vector2[0];
		}
		if (((Vector2)(ref b1)).Equals(b2))
		{
			if (PointOnLine(b1, a1, a2))
			{
				return (Vector2[])(object)new Vector2[1] { b1 };
			}
			return (Vector2[])(object)new Vector2[0];
		}
		if (((Vector2)(ref a1)).Equals(a2))
		{
			if (PointOnLine(a1, b1, b2))
			{
				return (Vector2[])(object)new Vector2[1] { a1 };
			}
			return (Vector2[])(object)new Vector2[0];
		}
		float num = (b2.X - b1.X) * (a1.Y - b1.Y) - (b2.Y - b1.Y) * (a1.X - b1.X);
		float num2 = (a2.X - a1.X) * (a1.Y - b1.Y) - (a2.Y - a1.Y) * (a1.X - b1.X);
		float num3 = (b2.Y - b1.Y) * (a2.X - a1.X) - (b2.X - b1.X) * (a2.Y - a1.Y);
		if (!(0f - Epsilon < num3) || !(num3 < Epsilon))
		{
			float num4 = num / num3;
			float num5 = num2 / num3;
			if (0f <= num4 && num4 <= 1f && 0f <= num5 && num5 <= 1f)
			{
				return (Vector2[])(object)new Vector2[1]
				{
					new Vector2(a1.X + num4 * (a2.X - a1.X), a1.Y + num4 * (a2.Y - a1.Y))
				};
			}
			return (Vector2[])(object)new Vector2[0];
		}
		if ((0f - Epsilon < num && num < Epsilon) || (0f - Epsilon < num2 && num2 < Epsilon))
		{
			if (((Vector2)(ref a1)).Equals(a2))
			{
				return OneDimensionalIntersection(b1, b2, a1, a2);
			}
			return OneDimensionalIntersection(a1, a2, b1, b2);
		}
		return (Vector2[])(object)new Vector2[0];
	}

	private static double DistFromSeg(Vector2 p, Vector2 q0, Vector2 q1, double radius, ref float u)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		double num6 = q1.X - q0.X;
		double num2 = q1.Y - q0.Y;
		double num3 = q0.X - p.X;
		double num4 = q0.Y - p.Y;
		double num5 = Math.Sqrt(num6 * num6 + num2 * num2);
		if (num5 < (double)Epsilon)
		{
			throw new Exception("Expected line segment, not point.");
		}
		return Math.Abs(num6 * num4 - num3 * num2) / num5;
	}

	private static bool PointOnLine(Vector2 p, Vector2 a1, Vector2 a2)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		float u = 0f;
		return DistFromSeg(p, a1, a2, Epsilon, ref u) < (double)Epsilon;
	}

	private static Vector2[] OneDimensionalIntersection(Vector2 a1, Vector2 a2, Vector2 b1, Vector2 b2)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		float num = a2.X - a1.X;
		float num2 = a2.Y - a1.Y;
		float relativePoint;
		float relativePoint2;
		if (Math.Abs(num) > Math.Abs(num2))
		{
			relativePoint = (b1.X - a1.X) / num;
			relativePoint2 = (b2.X - a1.X) / num;
		}
		else
		{
			relativePoint = (b1.Y - a1.Y) / num2;
			relativePoint2 = (b2.Y - a1.Y) / num2;
		}
		List<Vector2> list = new List<Vector2>();
		float[] array = FindOverlapPoints(relativePoint, relativePoint2);
		foreach (float num3 in array)
		{
			float x = a2.X * num3 + a1.X * (1f - num3);
			float y = a2.Y * num3 + a1.Y * (1f - num3);
			list.Add(new Vector2(x, y));
		}
		return list.ToArray();
	}

	private static float[] FindOverlapPoints(float relativePoint1, float relativePoint2)
	{
		float val = Math.Min(relativePoint1, relativePoint2);
		float val2 = Math.Max(relativePoint1, relativePoint2);
		float num = Math.Max(0f, val);
		float num2 = Math.Min(1f, val2);
		if (num > num2)
		{
			return new float[0];
		}
		if (num == num2)
		{
			return new float[1] { num };
		}
		return new float[2] { num, num2 };
	}

	public static bool CheckAABBvAABBCollision(Vector2 position1, Vector2 dimensions1, Vector2 position2, Vector2 dimensions2)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		if (position1.X < position2.X + dimensions2.X && position1.Y < position2.Y + dimensions2.Y && position1.X + dimensions1.X > position2.X)
		{
			return position1.Y + dimensions1.Y > position2.Y;
		}
		return false;
	}

	private static int collisionOutcode(Vector2 aabbPosition, Vector2 aabbDimensions, Vector2 point)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		float num = aabbPosition.X + aabbDimensions.X;
		float num2 = aabbPosition.Y + aabbDimensions.Y;
		int num3 = 0;
		if (aabbDimensions.X <= 0f)
		{
			num3 |= 5;
		}
		else if (point.X < aabbPosition.X)
		{
			num3 |= 1;
		}
		else if (point.X - num > 0f)
		{
			num3 |= 4;
		}
		if (aabbDimensions.Y <= 0f)
		{
			num3 |= 0xA;
		}
		else if (point.Y < aabbPosition.Y)
		{
			num3 |= 2;
		}
		else if (point.Y - num2 > 0f)
		{
			num3 |= 8;
		}
		return num3;
	}

	public static bool CheckAABBvLineCollision(Vector2 aabbPosition, Vector2 aabbDimensions, Vector2 lineStart, Vector2 lineEnd)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		int num;
		if ((num = collisionOutcode(aabbPosition, aabbDimensions, lineEnd)) == 0)
		{
			return true;
		}
		int num2;
		while ((num2 = collisionOutcode(aabbPosition, aabbDimensions, lineStart)) != 0)
		{
			if ((num2 & num) != 0)
			{
				return false;
			}
			if (((uint)num2 & 5u) != 0)
			{
				float num3 = aabbPosition.X;
				if (((uint)num2 & 4u) != 0)
				{
					num3 += aabbDimensions.X;
				}
				lineStart.Y += (num3 - lineStart.X) * (lineEnd.Y - lineStart.Y) / (lineEnd.X - lineStart.X);
				lineStart.X = num3;
			}
			else
			{
				float num4 = aabbPosition.Y;
				if (((uint)num2 & 8u) != 0)
				{
					num4 += aabbDimensions.Y;
				}
				lineStart.X += (num4 - lineStart.Y) * (lineEnd.X - lineStart.X) / (lineEnd.Y - lineStart.Y);
				lineStart.Y = num4;
			}
		}
		return true;
	}

	public static bool CheckAABBvLineCollision2(Vector2 aabbPosition, Vector2 aabbDimensions, Vector2 lineStart, Vector2 lineEnd)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		float collisionPoint = 0f;
		if (!Utils.RectangleLineCollision(aabbPosition, aabbPosition + aabbDimensions, lineStart, lineEnd))
		{
			return CheckAABBvLineCollision(aabbPosition, aabbDimensions, lineStart, lineEnd, 0.0001f, ref collisionPoint);
		}
		return true;
	}

	public static bool CheckAABBvLineCollision(Vector2 objectPosition, Vector2 objectDimensions, Vector2 lineStart, Vector2 lineEnd, float lineWidth, ref float collisionPoint)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		//IL_017b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Unknown result type (might be due to invalid IL or missing references)
		//IL_0183: Unknown result type (might be due to invalid IL or missing references)
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_026a: Unknown result type (might be due to invalid IL or missing references)
		//IL_026c: Unknown result type (might be due to invalid IL or missing references)
		//IL_026e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0273: Unknown result type (might be due to invalid IL or missing references)
		//IL_0279: Unknown result type (might be due to invalid IL or missing references)
		//IL_0282: Unknown result type (might be due to invalid IL or missing references)
		//IL_0287: Unknown result type (might be due to invalid IL or missing references)
		//IL_028c: Unknown result type (might be due to invalid IL or missing references)
		//IL_028e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0295: Unknown result type (might be due to invalid IL or missing references)
		//IL_029d: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0359: Unknown result type (might be due to invalid IL or missing references)
		//IL_035b: Unknown result type (might be due to invalid IL or missing references)
		//IL_035d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0362: Unknown result type (might be due to invalid IL or missing references)
		//IL_0364: Unknown result type (might be due to invalid IL or missing references)
		//IL_036b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0373: Unknown result type (might be due to invalid IL or missing references)
		//IL_037a: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0395: Unknown result type (might be due to invalid IL or missing references)
		//IL_039a: Unknown result type (might be due to invalid IL or missing references)
		//IL_039c: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0303: Unknown result type (might be due to invalid IL or missing references)
		//IL_030a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0312: Unknown result type (might be due to invalid IL or missing references)
		//IL_0319: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_0341: Unknown result type (might be due to invalid IL or missing references)
		//IL_034a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0417: Unknown result type (might be due to invalid IL or missing references)
		//IL_0420: Unknown result type (might be due to invalid IL or missing references)
		float num = lineWidth * 0.5f;
		Vector2 position = lineStart;
		Vector2 dimensions = lineEnd - lineStart;
		if (dimensions.X > 0f)
		{
			dimensions.X += lineWidth;
			position.X -= num;
		}
		else
		{
			position.X += dimensions.X - num;
			dimensions.X = 0f - dimensions.X + lineWidth;
		}
		if (dimensions.Y > 0f)
		{
			dimensions.Y += lineWidth;
			position.Y -= num;
		}
		else
		{
			position.Y += dimensions.Y - num;
			dimensions.Y = 0f - dimensions.Y + lineWidth;
		}
		if (!CheckAABBvAABBCollision(objectPosition, objectDimensions, position, dimensions))
		{
			return false;
		}
		Vector2 vector = objectPosition - lineStart;
		Vector2 spinningpoint = vector + objectDimensions;
		Vector2 spinningpoint2 = default(Vector2);
		((Vector2)(ref spinningpoint2))._002Ector(vector.X, spinningpoint.Y);
		Vector2 spinningpoint3 = default(Vector2);
		((Vector2)(ref spinningpoint3))._002Ector(spinningpoint.X, vector.Y);
		Vector2 vector3 = lineEnd - lineStart;
		float num2 = ((Vector2)(ref vector3)).Length();
		float num3 = (float)Math.Atan2(vector3.Y, vector3.X);
		Vector2[] array = (Vector2[])(object)new Vector2[4]
		{
			vector.RotatedBy(0f - num3),
			spinningpoint3.RotatedBy(0f - num3),
			spinningpoint.RotatedBy(0f - num3),
			spinningpoint2.RotatedBy(0f - num3)
		};
		collisionPoint = num2;
		bool result = false;
		for (int i = 0; i < array.Length; i++)
		{
			if (Math.Abs(array[i].Y) < num && array[i].X < collisionPoint && array[i].X >= 0f)
			{
				collisionPoint = array[i].X;
				result = true;
			}
		}
		Vector2 vector4 = default(Vector2);
		((Vector2)(ref vector4))._002Ector(0f, num);
		Vector2 vector5 = default(Vector2);
		((Vector2)(ref vector5))._002Ector(num2, num);
		Vector2 vector6 = default(Vector2);
		((Vector2)(ref vector6))._002Ector(0f, 0f - num);
		Vector2 vector7 = default(Vector2);
		((Vector2)(ref vector7))._002Ector(num2, 0f - num);
		for (int j = 0; j < array.Length; j++)
		{
			int num4 = (j + 1) % array.Length;
			Vector2 vector8 = vector5 - vector4;
			Vector2 vector9 = array[num4] - array[j];
			float num5 = vector8.X * vector9.Y - vector8.Y * vector9.X;
			if (num5 != 0f)
			{
				Vector2 vector10 = array[j] - vector4;
				float num6 = (vector10.X * vector9.Y - vector10.Y * vector9.X) / num5;
				if (num6 >= 0f && num6 <= 1f)
				{
					float num7 = (vector10.X * vector8.Y - vector10.Y * vector8.X) / num5;
					if (num7 >= 0f && num7 <= 1f)
					{
						result = true;
						collisionPoint = Math.Min(collisionPoint, vector4.X + num6 * vector8.X);
					}
				}
			}
			vector8 = vector7 - vector6;
			num5 = vector8.X * vector9.Y - vector8.Y * vector9.X;
			if (num5 == 0f)
			{
				continue;
			}
			Vector2 vector2 = array[j] - vector6;
			float num8 = (vector2.X * vector9.Y - vector2.Y * vector9.X) / num5;
			if (num8 >= 0f && num8 <= 1f)
			{
				float num9 = (vector2.X * vector8.Y - vector2.Y * vector8.X) / num5;
				if (num9 >= 0f && num9 <= 1f)
				{
					result = true;
					collisionPoint = Math.Min(collisionPoint, vector6.X + num8 * vector8.X);
				}
			}
		}
		return result;
	}

	public static bool CanHit(Entity source, Entity target)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		return CanHit(source.position, source.width, source.height, target.position, target.width, target.height);
	}

	public static bool CanHit(Entity source, NPCAimedTarget target)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		return CanHit(source.position, source.width, source.height, target.Position, target.Width, target.Height);
	}

	public static bool CanHit(Vector2 Position1, int Width1, int Height1, Vector2 Position2, int Width2, int Height2)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		return CanHit(Position1.ToPoint(), Width1, Height1, Position2.ToPoint(), Width2, Height2);
	}

	public static bool CanHit(Point Position1, int Width1, int Height1, Point Position2, int Width2, int Height2)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		int num = (Position1.X + Width1 / 2) / 16;
		int num2 = (Position1.Y + Height1 / 2) / 16;
		int num3 = (Position2.X + Width2 / 2) / 16;
		int num4 = (Position2.Y + Height2 / 2) / 16;
		if (num <= 1)
		{
			num = 1;
		}
		if (num >= Main.maxTilesX)
		{
			num = Main.maxTilesX - 1;
		}
		if (num3 <= 1)
		{
			num3 = 1;
		}
		if (num3 >= Main.maxTilesX)
		{
			num3 = Main.maxTilesX - 1;
		}
		if (num2 <= 1)
		{
			num2 = 1;
		}
		if (num2 >= Main.maxTilesY)
		{
			num2 = Main.maxTilesY - 1;
		}
		if (num4 <= 1)
		{
			num4 = 1;
		}
		if (num4 >= Main.maxTilesY)
		{
			num4 = Main.maxTilesY - 1;
		}
		try
		{
			do
			{
				int num5 = Math.Abs(num - num3);
				int num6 = Math.Abs(num2 - num4);
				if (num == num3 && num2 == num4)
				{
					return true;
				}
				if (num5 > num6)
				{
					num = ((num >= num3) ? (num - 1) : (num + 1));
					if (Main.tile[num, num2 - 1] == null)
					{
						return false;
					}
					if (Main.tile[num, num2 + 1] == null)
					{
						return false;
					}
					if (!Main.tile[num, num2 - 1].inActive() && Main.tile[num, num2 - 1].active() && Main.tileSolid[Main.tile[num, num2 - 1].type] && !Main.tileSolidTop[Main.tile[num, num2 - 1].type] && Main.tile[num, num2 - 1].slope() == 0 && !Main.tile[num, num2 - 1].halfBrick() && !Main.tile[num, num2 + 1].inActive() && Main.tile[num, num2 + 1].active() && Main.tileSolid[Main.tile[num, num2 + 1].type] && !Main.tileSolidTop[Main.tile[num, num2 + 1].type] && Main.tile[num, num2 + 1].slope() == 0 && !Main.tile[num, num2 + 1].halfBrick())
					{
						return false;
					}
				}
				else
				{
					num2 = ((num2 >= num4) ? (num2 - 1) : (num2 + 1));
					if (Main.tile[num - 1, num2] == null)
					{
						return false;
					}
					if (Main.tile[num + 1, num2] == null)
					{
						return false;
					}
					if (!Main.tile[num - 1, num2].inActive() && Main.tile[num - 1, num2].active() && Main.tileSolid[Main.tile[num - 1, num2].type] && !Main.tileSolidTop[Main.tile[num - 1, num2].type] && Main.tile[num - 1, num2].slope() == 0 && !Main.tile[num - 1, num2].halfBrick() && !Main.tile[num + 1, num2].inActive() && Main.tile[num + 1, num2].active() && Main.tileSolid[Main.tile[num + 1, num2].type] && !Main.tileSolidTop[Main.tile[num + 1, num2].type] && Main.tile[num + 1, num2].slope() == 0 && !Main.tile[num + 1, num2].halfBrick())
					{
						return false;
					}
				}
				if (Main.tile[num, num2] == null)
				{
					return false;
				}
			}
			while (Main.tile[num, num2].inActive() || !Main.tile[num, num2].active() || !Main.tileSolid[Main.tile[num, num2].type] || Main.tileSolidTop[Main.tile[num, num2].type]);
			return false;
		}
		catch
		{
			return false;
		}
	}

	public static bool CanHitWithCheck(Vector2 Position1, int Width1, int Height1, Vector2 Position2, int Width2, int Height2, Utils.TileActionAttempt check)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		int num = (int)((Position1.X + (float)(Width1 / 2)) / 16f);
		int num2 = (int)((Position1.Y + (float)(Height1 / 2)) / 16f);
		int num3 = (int)((Position2.X + (float)(Width2 / 2)) / 16f);
		int num4 = (int)((Position2.Y + (float)(Height2 / 2)) / 16f);
		if (num <= 1)
		{
			num = 1;
		}
		if (num >= Main.maxTilesX)
		{
			num = Main.maxTilesX - 1;
		}
		if (num3 <= 1)
		{
			num3 = 1;
		}
		if (num3 >= Main.maxTilesX)
		{
			num3 = Main.maxTilesX - 1;
		}
		if (num2 <= 1)
		{
			num2 = 1;
		}
		if (num2 >= Main.maxTilesY)
		{
			num2 = Main.maxTilesY - 1;
		}
		if (num4 <= 1)
		{
			num4 = 1;
		}
		if (num4 >= Main.maxTilesY)
		{
			num4 = Main.maxTilesY - 1;
		}
		try
		{
			do
			{
				int num5 = Math.Abs(num - num3);
				int num6 = Math.Abs(num2 - num4);
				if (num == num3 && num2 == num4)
				{
					return true;
				}
				if (num5 > num6)
				{
					num = ((num >= num3) ? (num - 1) : (num + 1));
					if (Main.tile[num, num2 - 1] == null)
					{
						return false;
					}
					if (Main.tile[num, num2 + 1] == null)
					{
						return false;
					}
					if (!Main.tile[num, num2 - 1].inActive() && Main.tile[num, num2 - 1].active() && Main.tileSolid[Main.tile[num, num2 - 1].type] && !Main.tileSolidTop[Main.tile[num, num2 - 1].type] && Main.tile[num, num2 - 1].slope() == 0 && !Main.tile[num, num2 - 1].halfBrick() && !Main.tile[num, num2 + 1].inActive() && Main.tile[num, num2 + 1].active() && Main.tileSolid[Main.tile[num, num2 + 1].type] && !Main.tileSolidTop[Main.tile[num, num2 + 1].type] && Main.tile[num, num2 + 1].slope() == 0 && !Main.tile[num, num2 + 1].halfBrick())
					{
						return false;
					}
				}
				else
				{
					num2 = ((num2 >= num4) ? (num2 - 1) : (num2 + 1));
					if (Main.tile[num - 1, num2] == null)
					{
						return false;
					}
					if (Main.tile[num + 1, num2] == null)
					{
						return false;
					}
					if (!Main.tile[num - 1, num2].inActive() && Main.tile[num - 1, num2].active() && Main.tileSolid[Main.tile[num - 1, num2].type] && !Main.tileSolidTop[Main.tile[num - 1, num2].type] && Main.tile[num - 1, num2].slope() == 0 && !Main.tile[num - 1, num2].halfBrick() && !Main.tile[num + 1, num2].inActive() && Main.tile[num + 1, num2].active() && Main.tileSolid[Main.tile[num + 1, num2].type] && !Main.tileSolidTop[Main.tile[num + 1, num2].type] && Main.tile[num + 1, num2].slope() == 0 && !Main.tile[num + 1, num2].halfBrick())
					{
						return false;
					}
				}
				if (Main.tile[num, num2] == null)
				{
					return false;
				}
				if (!Main.tile[num, num2].inActive() && Main.tile[num, num2].active() && Main.tileSolid[Main.tile[num, num2].type] && !Main.tileSolidTop[Main.tile[num, num2].type])
				{
					return false;
				}
			}
			while (check(num, num2));
			return false;
		}
		catch
		{
			return false;
		}
	}

	public static bool CanHitLine(Vector2 Position1, int Width1, int Height1, Vector2 Position2, int Width2, int Height2)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		int num = (int)((Position1.X + (float)(Width1 / 2)) / 16f);
		int num10 = (int)((Position1.Y + (float)(Height1 / 2)) / 16f);
		int num11 = (int)((Position2.X + (float)(Width2 / 2)) / 16f);
		int num12 = (int)((Position2.Y + (float)(Height2 / 2)) / 16f);
		if (num <= 1)
		{
			num = 1;
		}
		if (num >= Main.maxTilesX)
		{
			num = Main.maxTilesX - 1;
		}
		if (num11 <= 1)
		{
			num11 = 1;
		}
		if (num11 >= Main.maxTilesX)
		{
			num11 = Main.maxTilesX - 1;
		}
		if (num10 <= 1)
		{
			num10 = 1;
		}
		if (num10 >= Main.maxTilesY)
		{
			num10 = Main.maxTilesY - 1;
		}
		if (num12 <= 1)
		{
			num12 = 1;
		}
		if (num12 >= Main.maxTilesY)
		{
			num12 = Main.maxTilesY - 1;
		}
		float num13 = Math.Abs(num - num11);
		float num14 = Math.Abs(num10 - num12);
		if (num13 == 0f && num14 == 0f)
		{
			return true;
		}
		float num15 = 1f;
		float num16 = 1f;
		if (num13 == 0f || num14 == 0f)
		{
			if (num13 == 0f)
			{
				num15 = 0f;
			}
			if (num14 == 0f)
			{
				num16 = 0f;
			}
		}
		else if (num13 > num14)
		{
			num15 = num13 / num14;
		}
		else
		{
			num16 = num14 / num13;
		}
		float num17 = 0f;
		float num2 = 0f;
		int num3 = 1;
		if (num10 < num12)
		{
			num3 = 2;
		}
		int num4 = (int)num13;
		int num5 = (int)num14;
		int num6 = Math.Sign(num11 - num);
		int num7 = Math.Sign(num12 - num10);
		bool flag = false;
		bool flag2 = false;
		try
		{
			do
			{
				switch (num3)
				{
				case 2:
				{
					num17 += num15;
					int num9 = (int)num17;
					num17 %= 1f;
					for (int j = 0; j < num9; j++)
					{
						if (Main.tile[num, num10 - 1] == null)
						{
							return false;
						}
						if (Main.tile[num, num10] == null)
						{
							return false;
						}
						if (Main.tile[num, num10 + 1] == null)
						{
							return false;
						}
						Tile tile4 = Main.tile[num, num10 - 1];
						Tile tile5 = Main.tile[num, num10 + 1];
						Tile tile6 = Main.tile[num, num10];
						if ((!tile4.inActive() && tile4.active() && Main.tileSolid[tile4.type] && !Main.tileSolidTop[tile4.type]) || (!tile5.inActive() && tile5.active() && Main.tileSolid[tile5.type] && !Main.tileSolidTop[tile5.type]) || (!tile6.inActive() && tile6.active() && Main.tileSolid[tile6.type] && !Main.tileSolidTop[tile6.type]))
						{
							return false;
						}
						if (num4 == 0 && num5 == 0)
						{
							flag = true;
							break;
						}
						num += num6;
						num4--;
						if (num4 == 0 && num5 == 0 && num9 == 1)
						{
							flag2 = true;
						}
					}
					if (num5 != 0)
					{
						num3 = 1;
					}
					break;
				}
				case 1:
				{
					num2 += num16;
					int num8 = (int)num2;
					num2 %= 1f;
					for (int i = 0; i < num8; i++)
					{
						if (Main.tile[num - 1, num10] == null)
						{
							return false;
						}
						if (Main.tile[num, num10] == null)
						{
							return false;
						}
						if (Main.tile[num + 1, num10] == null)
						{
							return false;
						}
						Tile tile = Main.tile[num - 1, num10];
						Tile tile2 = Main.tile[num + 1, num10];
						Tile tile3 = Main.tile[num, num10];
						if ((!tile.inActive() && tile.active() && Main.tileSolid[tile.type] && !Main.tileSolidTop[tile.type]) || (!tile2.inActive() && tile2.active() && Main.tileSolid[tile2.type] && !Main.tileSolidTop[tile2.type]) || (!tile3.inActive() && tile3.active() && Main.tileSolid[tile3.type] && !Main.tileSolidTop[tile3.type]))
						{
							return false;
						}
						if (num4 == 0 && num5 == 0)
						{
							flag = true;
							break;
						}
						num10 += num7;
						num5--;
						if (num4 == 0 && num5 == 0 && num8 == 1)
						{
							flag2 = true;
						}
					}
					if (num4 != 0)
					{
						num3 = 2;
					}
					break;
				}
				}
				if (Main.tile[num, num10] == null)
				{
					return false;
				}
				Tile tile7 = Main.tile[num, num10];
				if (!tile7.inActive() && tile7.active() && Main.tileSolid[tile7.type] && !Main.tileSolidTop[tile7.type])
				{
					return false;
				}
			}
			while (!(flag || flag2));
			return true;
		}
		catch
		{
			return false;
		}
	}

	public static bool TupleHitLine(int x1, int y1, int x2, int y2, int ignoreX, int ignoreY, List<Tuple<int, int>> ignoreTargets, out Tuple<int, int> col)
	{
		int value = x1;
		int value2 = y1;
		int value3 = x2;
		int value4 = y2;
		value = Utils.Clamp(value, 1, Main.maxTilesX - 1);
		value3 = Utils.Clamp(value3, 1, Main.maxTilesX - 1);
		value2 = Utils.Clamp(value2, 1, Main.maxTilesY - 1);
		value4 = Utils.Clamp(value4, 1, Main.maxTilesY - 1);
		float num = Math.Abs(value - value3);
		float num6 = Math.Abs(value2 - value4);
		if (num == 0f && num6 == 0f)
		{
			col = new Tuple<int, int>(value, value2);
			return true;
		}
		float num7 = 1f;
		float num8 = 1f;
		if (num == 0f || num6 == 0f)
		{
			if (num == 0f)
			{
				num7 = 0f;
			}
			if (num6 == 0f)
			{
				num8 = 0f;
			}
		}
		else if (num > num6)
		{
			num7 = num / num6;
		}
		else
		{
			num8 = num6 / num;
		}
		float num9 = 0f;
		float num10 = 0f;
		int num11 = 1;
		if (value2 < value4)
		{
			num11 = 2;
		}
		int num12 = (int)num;
		int num13 = (int)num6;
		int num2 = Math.Sign(value3 - value);
		int num3 = Math.Sign(value4 - value2);
		bool flag = false;
		bool flag2 = false;
		try
		{
			do
			{
				switch (num11)
				{
				case 2:
				{
					num9 += num7;
					int num5 = (int)num9;
					num9 %= 1f;
					for (int j = 0; j < num5; j++)
					{
						if (Main.tile[value, value2 - 1] == null)
						{
							col = new Tuple<int, int>(value, value2 - 1);
							return false;
						}
						if (Main.tile[value, value2 + 1] == null)
						{
							col = new Tuple<int, int>(value, value2 + 1);
							return false;
						}
						Tile tile4 = Main.tile[value, value2 - 1];
						Tile tile5 = Main.tile[value, value2 + 1];
						Tile tile6 = Main.tile[value, value2];
						if (!ignoreTargets.Contains(new Tuple<int, int>(value, value2)) && !ignoreTargets.Contains(new Tuple<int, int>(value, value2 - 1)) && !ignoreTargets.Contains(new Tuple<int, int>(value, value2 + 1)))
						{
							if (ignoreY != -1 && num3 < 0 && !tile4.inActive() && tile4.active() && Main.tileSolid[tile4.type] && !Main.tileSolidTop[tile4.type])
							{
								col = new Tuple<int, int>(value, value2 - 1);
								return true;
							}
							if (ignoreY != 1 && num3 > 0 && !tile5.inActive() && tile5.active() && Main.tileSolid[tile5.type] && !Main.tileSolidTop[tile5.type])
							{
								col = new Tuple<int, int>(value, value2 + 1);
								return true;
							}
							if (!tile6.inActive() && tile6.active() && Main.tileSolid[tile6.type] && !Main.tileSolidTop[tile6.type])
							{
								col = new Tuple<int, int>(value, value2);
								return true;
							}
						}
						if (num12 == 0 && num13 == 0)
						{
							flag = true;
							break;
						}
						value += num2;
						num12--;
						if (num12 == 0 && num13 == 0 && num5 == 1)
						{
							flag2 = true;
						}
					}
					if (num13 != 0)
					{
						num11 = 1;
					}
					break;
				}
				case 1:
				{
					num10 += num8;
					int num4 = (int)num10;
					num10 %= 1f;
					for (int i = 0; i < num4; i++)
					{
						if (Main.tile[value - 1, value2] == null)
						{
							col = new Tuple<int, int>(value - 1, value2);
							return false;
						}
						if (Main.tile[value + 1, value2] == null)
						{
							col = new Tuple<int, int>(value + 1, value2);
							return false;
						}
						Tile tile = Main.tile[value - 1, value2];
						Tile tile2 = Main.tile[value + 1, value2];
						Tile tile3 = Main.tile[value, value2];
						if (!ignoreTargets.Contains(new Tuple<int, int>(value, value2)) && !ignoreTargets.Contains(new Tuple<int, int>(value - 1, value2)) && !ignoreTargets.Contains(new Tuple<int, int>(value + 1, value2)))
						{
							if (ignoreX != -1 && num2 < 0 && !tile.inActive() && tile.active() && Main.tileSolid[tile.type] && !Main.tileSolidTop[tile.type])
							{
								col = new Tuple<int, int>(value - 1, value2);
								return true;
							}
							if (ignoreX != 1 && num2 > 0 && !tile2.inActive() && tile2.active() && Main.tileSolid[tile2.type] && !Main.tileSolidTop[tile2.type])
							{
								col = new Tuple<int, int>(value + 1, value2);
								return true;
							}
							if (!tile3.inActive() && tile3.active() && Main.tileSolid[tile3.type] && !Main.tileSolidTop[tile3.type])
							{
								col = new Tuple<int, int>(value, value2);
								return true;
							}
						}
						if (num12 == 0 && num13 == 0)
						{
							flag = true;
							break;
						}
						value2 += num3;
						num13--;
						if (num12 == 0 && num13 == 0 && num4 == 1)
						{
							flag2 = true;
						}
					}
					if (num12 != 0)
					{
						num11 = 2;
					}
					break;
				}
				}
				if (Main.tile[value, value2] == null)
				{
					col = new Tuple<int, int>(value, value2);
					return false;
				}
				Tile tile7 = Main.tile[value, value2];
				if (!ignoreTargets.Contains(new Tuple<int, int>(value, value2)) && !tile7.inActive() && tile7.active() && Main.tileSolid[tile7.type] && !Main.tileSolidTop[tile7.type])
				{
					col = new Tuple<int, int>(value, value2);
					return true;
				}
			}
			while (!(flag || flag2));
			col = new Tuple<int, int>(value, value2);
			return true;
		}
		catch
		{
			col = new Tuple<int, int>(x1, y1);
			return false;
		}
	}

	public static Tuple<int, int> TupleHitLineWall(int x1, int y1, int x2, int y2)
	{
		int num = x1;
		int num10 = y1;
		int num11 = x2;
		int num12 = y2;
		if (num <= 1)
		{
			num = 1;
		}
		if (num >= Main.maxTilesX)
		{
			num = Main.maxTilesX - 1;
		}
		if (num11 <= 1)
		{
			num11 = 1;
		}
		if (num11 >= Main.maxTilesX)
		{
			num11 = Main.maxTilesX - 1;
		}
		if (num10 <= 1)
		{
			num10 = 1;
		}
		if (num10 >= Main.maxTilesY)
		{
			num10 = Main.maxTilesY - 1;
		}
		if (num12 <= 1)
		{
			num12 = 1;
		}
		if (num12 >= Main.maxTilesY)
		{
			num12 = Main.maxTilesY - 1;
		}
		float num13 = Math.Abs(num - num11);
		float num14 = Math.Abs(num10 - num12);
		if (num13 == 0f && num14 == 0f)
		{
			return new Tuple<int, int>(num, num10);
		}
		float num15 = 1f;
		float num16 = 1f;
		if (num13 == 0f || num14 == 0f)
		{
			if (num13 == 0f)
			{
				num15 = 0f;
			}
			if (num14 == 0f)
			{
				num16 = 0f;
			}
		}
		else if (num13 > num14)
		{
			num15 = num13 / num14;
		}
		else
		{
			num16 = num14 / num13;
		}
		float num17 = 0f;
		float num2 = 0f;
		int num3 = 1;
		if (num10 < num12)
		{
			num3 = 2;
		}
		int num4 = (int)num13;
		int num5 = (int)num14;
		int num6 = Math.Sign(num11 - num);
		int num7 = Math.Sign(num12 - num10);
		bool flag = false;
		bool flag2 = false;
		try
		{
			do
			{
				switch (num3)
				{
				case 2:
				{
					num17 += num15;
					int num9 = (int)num17;
					num17 %= 1f;
					for (int j = 0; j < num9; j++)
					{
						_ = Main.tile[num, num10];
						if (HitWallSubstep(num, num10))
						{
							return new Tuple<int, int>(num, num10);
						}
						if (num4 == 0 && num5 == 0)
						{
							flag = true;
							break;
						}
						num += num6;
						num4--;
						if (num4 == 0 && num5 == 0 && num9 == 1)
						{
							flag2 = true;
						}
					}
					if (num5 != 0)
					{
						num3 = 1;
					}
					break;
				}
				case 1:
				{
					num2 += num16;
					int num8 = (int)num2;
					num2 %= 1f;
					for (int i = 0; i < num8; i++)
					{
						_ = Main.tile[num, num10];
						if (HitWallSubstep(num, num10))
						{
							return new Tuple<int, int>(num, num10);
						}
						if (num4 == 0 && num5 == 0)
						{
							flag = true;
							break;
						}
						num10 += num7;
						num5--;
						if (num4 == 0 && num5 == 0 && num8 == 1)
						{
							flag2 = true;
						}
					}
					if (num4 != 0)
					{
						num3 = 2;
					}
					break;
				}
				}
				if (Main.tile[num, num10] == null)
				{
					return new Tuple<int, int>(-1, -1);
				}
				_ = Main.tile[num, num10];
				if (HitWallSubstep(num, num10))
				{
					return new Tuple<int, int>(num, num10);
				}
			}
			while (!(flag || flag2));
			return new Tuple<int, int>(num, num10);
		}
		catch
		{
			return new Tuple<int, int>(-1, -1);
		}
	}

	public static bool HitWallSubstep(int x, int y)
	{
		if (Main.tile[x, y].wall == 0)
		{
			return false;
		}
		bool flag = false;
		if (Main.wallHouse[Main.tile[x, y].wall])
		{
			flag = true;
		}
		if (!flag)
		{
			for (int i = -1; i < 2; i++)
			{
				for (int j = -1; j < 2; j++)
				{
					if ((i != 0 || j != 0) && Main.tile[x + i, y + j].wall == 0)
					{
						flag = true;
					}
				}
			}
		}
		if (Main.tile[x, y].active() && flag)
		{
			bool flag2 = true;
			for (int k = -1; k < 2; k++)
			{
				for (int l = -1; l < 2; l++)
				{
					if (k != 0 || l != 0)
					{
						Tile tile = Main.tile[x + k, y + l];
						if (!tile.active() || !Main.tileSolid[tile.type] || Main.tileSolidTop[tile.type])
						{
							flag2 = false;
						}
					}
				}
			}
			if (flag2)
			{
				flag = false;
			}
		}
		return flag;
	}

	public static bool EmptyTile(int i, int j, bool ignoreTiles = false)
	{
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		Rectangle rectangle = default(Rectangle);
		((Rectangle)(ref rectangle))._002Ector(i * 16, j * 16, 16, 16);
		if (Main.tile[i, j].active() && !ignoreTiles)
		{
			return false;
		}
		for (int k = 0; k < 255; k++)
		{
			if (Main.player[k].active && !Main.player[k].dead && !Main.player[k].ghost && ((Rectangle)(ref rectangle)).Intersects(new Rectangle((int)Main.player[k].position.X, (int)Main.player[k].position.Y, Main.player[k].width, Main.player[k].height)))
			{
				return false;
			}
		}
		for (int l = 0; l < 200; l++)
		{
			if (Main.npc[l].active && ((Rectangle)(ref rectangle)).Intersects(new Rectangle((int)Main.npc[l].position.X, (int)Main.npc[l].position.Y, Main.npc[l].width, Main.npc[l].height)))
			{
				return false;
			}
		}
		return true;
	}

	public static bool DrownCollision(Vector2 Position, int Width, int Height, float gravDir = -1f, bool includeSlopes = false)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0204: Unknown result type (might be due to invalid IL or missing references)
		//IL_020d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0216: Unknown result type (might be due to invalid IL or missing references)
		//IL_021c: Unknown result type (might be due to invalid IL or missing references)
		//IL_022b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0234: Unknown result type (might be due to invalid IL or missing references)
		//IL_023d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0243: Unknown result type (might be due to invalid IL or missing references)
		Vector2 vector = default(Vector2);
		((Vector2)(ref vector))._002Ector(Position.X + (float)(Width / 2), Position.Y + (float)(Height / 2));
		int num = 10;
		int num2 = 12;
		if (num > Width)
		{
			num = Width;
		}
		if (num2 > Height)
		{
			num2 = Height;
		}
		((Vector2)(ref vector))._002Ector(vector.X - (float)(num / 2), Position.Y + -2f);
		if (gravDir == -1f)
		{
			vector.Y += Height / 2 - 6;
		}
		int value5 = (int)(Position.X / 16f) - 1;
		int value2 = (int)((Position.X + (float)Width) / 16f) + 2;
		int value3 = (int)(Position.Y / 16f) - 1;
		int value4 = (int)((Position.Y + (float)Height) / 16f) + 2;
		int num6 = Utils.Clamp(value5, 0, Main.maxTilesX - 1);
		value2 = Utils.Clamp(value2, 0, Main.maxTilesX - 1);
		value3 = Utils.Clamp(value3, 0, Main.maxTilesY - 1);
		value4 = Utils.Clamp(value4, 0, Main.maxTilesY - 1);
		int num3 = ((gravDir == 1f) ? value3 : (value4 - 1));
		Vector2 vector2 = default(Vector2);
		for (int i = num6; i < value2; i++)
		{
			for (int j = value3; j < value4; j++)
			{
				Tile tile = Main.tile[i, j];
				if (tile != null && tile.liquid > 0 && !tile.lava() && !tile.shimmer() && (j != num3 || !tile.active() || !Main.tileSolid[tile.type] || Main.tileSolidTop[tile.type] || (includeSlopes && tile.blockType() != 0)))
				{
					vector2.X = i * 16;
					vector2.Y = j * 16;
					int num4 = 16;
					float num5 = 256 - Main.tile[i, j].liquid;
					num5 /= 32f;
					vector2.Y += num5 * 2f;
					num4 -= (int)(num5 * 2f);
					if (vector.X + (float)num > vector2.X && vector.X < vector2.X + 16f && vector.Y + (float)num2 > vector2.Y && vector.Y < vector2.Y + (float)num4)
					{
						return true;
					}
				}
			}
		}
		return false;
	}

	public static bool IsWorldPointSolid(Vector2 pos, bool treatPlatformsAsNonSolid = false)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_0165: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_018c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_019e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		Point point = pos.ToTileCoordinates();
		if (!WorldGen.InWorld(point.X, point.Y, 1))
		{
			return false;
		}
		Tile tile = Main.tile[point.X, point.Y];
		if (tile == null || !tile.active() || tile.inActive() || !Main.tileSolid[tile.type])
		{
			return false;
		}
		if (treatPlatformsAsNonSolid && tile.type > 0 && (TileID.Sets.Platforms[tile.type] || tile.type == 380))
		{
			return false;
		}
		int num = tile.blockType();
		switch (num)
		{
		case 0:
			if (pos.X >= (float)(point.X * 16) && pos.X <= (float)(point.X * 16 + 16) && pos.Y >= (float)(point.Y * 16))
			{
				return pos.Y <= (float)(point.Y * 16 + 16);
			}
			return false;
		case 1:
			if (pos.X >= (float)(point.X * 16) && pos.X <= (float)(point.X * 16 + 16) && pos.Y >= (float)(point.Y * 16 + 8))
			{
				return pos.Y <= (float)(point.Y * 16 + 16);
			}
			return false;
		case 2:
		case 3:
		case 4:
		case 5:
		{
			if (pos.X < (float)(point.X * 16) && pos.X > (float)(point.X * 16 + 16) && pos.Y < (float)(point.Y * 16) && pos.Y > (float)(point.Y * 16 + 16))
			{
				return false;
			}
			float num2 = pos.X % 16f;
			float num3 = pos.Y % 16f;
			switch (num)
			{
			case 3:
				return num2 + num3 >= 16f;
			case 2:
				return num3 >= num2;
			case 5:
				return num3 <= num2;
			case 4:
				return num2 + num3 <= 16f;
			}
			break;
		}
		}
		return false;
	}

	public static bool GetWaterLine(Point pt, out float waterLineHeight)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		return GetWaterLine(pt.X, pt.Y, out waterLineHeight);
	}

	public static bool GetWaterLine(int X, int Y, out float waterLineHeight)
	{
		waterLineHeight = 0f;
		if (Main.tile[X, Y - 2] == null)
		{
			Main.tile[X, Y - 2] = default(Tile);
		}
		if (Main.tile[X, Y - 1] == null)
		{
			Main.tile[X, Y - 1] = default(Tile);
		}
		if (Main.tile[X, Y] == null)
		{
			Main.tile[X, Y] = default(Tile);
		}
		if (Main.tile[X, Y + 1] == null)
		{
			Main.tile[X, Y + 1] = default(Tile);
		}
		if (Main.tile[X, Y - 2].liquid > 0)
		{
			return false;
		}
		if (Main.tile[X, Y - 1].liquid > 0)
		{
			waterLineHeight = Y * 16;
			waterLineHeight -= Main.tile[X, Y - 1].liquid / 16;
			return true;
		}
		if (Main.tile[X, Y].liquid > 0)
		{
			waterLineHeight = (Y + 1) * 16;
			waterLineHeight -= Main.tile[X, Y].liquid / 16;
			return true;
		}
		if (Main.tile[X, Y + 1].liquid > 0)
		{
			waterLineHeight = (Y + 2) * 16;
			waterLineHeight -= Main.tile[X, Y + 1].liquid / 16;
			return true;
		}
		return false;
	}

	public static bool GetWaterLineIterate(Point pt, out float waterLineHeight)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		return GetWaterLineIterate(pt.X, pt.Y, out waterLineHeight);
	}

	public static bool GetWaterLineIterate(int X, int Y, out float waterLineHeight)
	{
		waterLineHeight = 0f;
		while (Y > 0 && Framing.GetTileSafely(X, Y).liquid > 0)
		{
			Y--;
		}
		Y++;
		if (Main.tile[X, Y] == null)
		{
			Main.tile[X, Y] = default(Tile);
		}
		if (Main.tile[X, Y].liquid > 0)
		{
			waterLineHeight = Y * 16;
			waterLineHeight -= Main.tile[X, Y - 1].liquid / 16;
			return true;
		}
		return false;
	}

	public static bool WetCollision(Vector2 Position, int Width, int Height)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0197: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0304: Unknown result type (might be due to invalid IL or missing references)
		//IL_030a: Unknown result type (might be due to invalid IL or missing references)
		honey = false;
		shimmer = false;
		Vector2 vector = default(Vector2);
		((Vector2)(ref vector))._002Ector(Position.X + (float)(Width / 2), Position.Y + (float)(Height / 2));
		int num = 10;
		int num2 = Height / 2;
		if (num > Width)
		{
			num = Width;
		}
		if (num2 > Height)
		{
			num2 = Height;
		}
		((Vector2)(ref vector))._002Ector(vector.X - (float)(num / 2), vector.Y - (float)(num2 / 2));
		int value5 = (int)(Position.X / 16f) - 1;
		int value2 = (int)((Position.X + (float)Width) / 16f) + 2;
		int value3 = (int)(Position.Y / 16f) - 1;
		int value4 = (int)((Position.Y + (float)Height) / 16f) + 2;
		int num6 = Utils.Clamp(value5, 0, Main.maxTilesX - 1);
		value2 = Utils.Clamp(value2, 0, Main.maxTilesX - 1);
		value3 = Utils.Clamp(value3, 0, Main.maxTilesY - 1);
		value4 = Utils.Clamp(value4, 0, Main.maxTilesY - 1);
		Vector2 vector2 = default(Vector2);
		for (int i = num6; i < value2; i++)
		{
			for (int j = value3; j < value4; j++)
			{
				if (Main.tile[i, j] == null)
				{
					continue;
				}
				if (Main.tile[i, j].liquid > 0)
				{
					vector2.X = i * 16;
					vector2.Y = j * 16;
					int num3 = 16;
					float num4 = 256 - Main.tile[i, j].liquid;
					num4 /= 32f;
					vector2.Y += num4 * 2f;
					num3 -= (int)(num4 * 2f);
					if (vector.X + (float)num > vector2.X && vector.X < vector2.X + 16f && vector.Y + (float)num2 > vector2.Y && vector.Y < vector2.Y + (float)num3)
					{
						if (Main.tile[i, j].honey())
						{
							honey = true;
						}
						if (Main.tile[i, j].shimmer())
						{
							shimmer = true;
						}
						return true;
					}
				}
				else
				{
					if (!Main.tile[i, j].active() || Main.tile[i, j].slope() == 0 || j <= 0 || Main.tile[i, j - 1] == null || Main.tile[i, j - 1].liquid <= 0)
					{
						continue;
					}
					vector2.X = i * 16;
					vector2.Y = j * 16;
					int num5 = 16;
					if (vector.X + (float)num > vector2.X && vector.X < vector2.X + 16f && vector.Y + (float)num2 > vector2.Y && vector.Y < vector2.Y + (float)num5)
					{
						if (Main.tile[i, j - 1].honey())
						{
							honey = true;
						}
						else if (Main.tile[i, j - 1].shimmer())
						{
							shimmer = true;
						}
						return true;
					}
				}
			}
		}
		return false;
	}

	public static bool LavaCollision(Vector2 Position, int Width, int Height)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Unknown result type (might be due to invalid IL or missing references)
		//IL_0189: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		int value5 = (int)(Position.X / 16f) - 1;
		int value2 = (int)((Position.X + (float)Width) / 16f) + 2;
		int value3 = (int)(Position.Y / 16f) - 1;
		int value4 = (int)((Position.Y + (float)Height) / 16f) + 2;
		int num4 = Utils.Clamp(value5, 0, Main.maxTilesX - 1);
		value2 = Utils.Clamp(value2, 0, Main.maxTilesX - 1);
		value3 = Utils.Clamp(value3, 0, Main.maxTilesY - 1);
		value4 = Utils.Clamp(value4, 0, Main.maxTilesY - 1);
		Vector2 vector = default(Vector2);
		for (int i = num4; i < value2; i++)
		{
			for (int j = value3; j < value4; j++)
			{
				if (Main.tile[i, j] != null && Main.tile[i, j].liquid > 0 && Main.tile[i, j].lava())
				{
					vector.X = i * 16;
					vector.Y = j * 16;
					int num2 = 16;
					float num3 = 256 - Main.tile[i, j].liquid;
					num3 /= 32f;
					vector.Y += num3 * 2f;
					num2 -= (int)(num3 * 2f);
					if (Position.X + (float)Width > vector.X && Position.X < vector.X + 16f && Position.Y + (float)Height > vector.Y && Position.Y < vector.Y + (float)num2)
					{
						return true;
					}
				}
			}
		}
		return false;
	}

	public static Vector4 WalkDownSlope(Vector2 Position, Vector2 Velocity, int Width, int Height, float gravity = 0f)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0393: Unknown result type (might be due to invalid IL or missing references)
		//IL_0394: Unknown result type (might be due to invalid IL or missing references)
		//IL_039a: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_034b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0351: Unknown result type (might be due to invalid IL or missing references)
		//IL_035b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0364: Unknown result type (might be due to invalid IL or missing references)
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		//IL_017e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0185: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0370: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0305: Unknown result type (might be due to invalid IL or missing references)
		//IL_0311: Unknown result type (might be due to invalid IL or missing references)
		//IL_031a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0386: Unknown result type (might be due to invalid IL or missing references)
		//IL_0326: Unknown result type (might be due to invalid IL or missing references)
		//IL_033c: Unknown result type (might be due to invalid IL or missing references)
		if (Velocity.Y != gravity)
		{
			return new Vector4(Position, Velocity.X, Velocity.Y);
		}
		int value = (int)(Position.X / 16f);
		int value2 = (int)((Position.X + (float)Width) / 16f);
		int value3 = (int)((Position.Y + (float)Height + 4f) / 16f);
		value = Utils.Clamp(value, 0, Main.maxTilesX - 1);
		value2 = Utils.Clamp(value2, 0, Main.maxTilesX - 1);
		value3 = Utils.Clamp(value3, 0, Main.maxTilesY - 3);
		float num = (value3 + 3) * 16;
		int num2 = -1;
		int num3 = -1;
		int num4 = 1;
		if (Velocity.X < 0f)
		{
			num4 = 2;
		}
		for (int i = value; i <= value2; i++)
		{
			for (int j = value3; j <= value3 + 1; j++)
			{
				if (Main.tile[i, j] == null)
				{
					Main.tile[i, j] = default(Tile);
				}
				if (!Main.tile[i, j].nactive() || (!Main.tileSolid[Main.tile[i, j].type] && !Main.tileSolidTop[Main.tile[i, j].type]))
				{
					continue;
				}
				int num5 = j * 16;
				if (Main.tile[i, j].halfBrick())
				{
					num5 += 8;
				}
				Rectangle val = new Rectangle(i * 16, j * 16 - 17, 16, 16);
				if (!((Rectangle)(ref val)).Intersects(new Rectangle((int)Position.X, (int)Position.Y, Width, Height)) || !((float)num5 <= num))
				{
					continue;
				}
				if (num == (float)num5)
				{
					if (Main.tile[i, j].slope() == 0)
					{
						continue;
					}
					if (num2 != -1 && num3 != -1 && Main.tile[num2, num3] != null && Main.tile[num2, num3].slope() != 0)
					{
						if (Main.tile[i, j].slope() == num4)
						{
							num = num5;
							num2 = i;
							num3 = j;
						}
					}
					else
					{
						num = num5;
						num2 = i;
						num3 = j;
					}
				}
				else
				{
					num = num5;
					num2 = i;
					num3 = j;
				}
			}
		}
		int num6 = num2;
		int num7 = num3;
		if (num2 != -1 && num3 != -1 && Main.tile[num6, num7] != null && Main.tile[num6, num7].slope() > 0)
		{
			int num8 = Main.tile[num6, num7].slope();
			Vector2 vector2 = default(Vector2);
			vector2.X = num6 * 16;
			vector2.Y = num7 * 16;
			switch (num8)
			{
			case 2:
			{
				float num9 = vector2.X + 16f - (Position.X + (float)Width);
				if (Position.Y + (float)Height >= vector2.Y + num9 && Velocity.X < 0f)
				{
					Velocity.Y += Math.Abs(Velocity.X);
				}
				break;
			}
			case 1:
			{
				float num10 = Position.X - vector2.X;
				if (Position.Y + (float)Height >= vector2.Y + num10 && Velocity.X > 0f)
				{
					Velocity.Y += Math.Abs(Velocity.X);
				}
				break;
			}
			}
		}
		return new Vector4(Position, Velocity.X, Velocity.Y);
	}

	public static Vector4 SlopeCollision(Vector2 Position, Vector2 Velocity, int Width, int Height, float gravity = 0f, bool fall = false)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_06bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_06bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_06be: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_06cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_06da: Unknown result type (might be due to invalid IL or missing references)
		//IL_0753: Unknown result type (might be due to invalid IL or missing references)
		//IL_075a: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_06fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_07cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_07cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_07db: Unknown result type (might be due to invalid IL or missing references)
		//IL_0763: Unknown result type (might be due to invalid IL or missing references)
		//IL_076a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0776: Unknown result type (might be due to invalid IL or missing references)
		//IL_077c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0710: Unknown result type (might be due to invalid IL or missing references)
		//IL_0790: Unknown result type (might be due to invalid IL or missing references)
		//IL_0725: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0203: Unknown result type (might be due to invalid IL or missing references)
		//IL_020f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0215: Unknown result type (might be due to invalid IL or missing references)
		//IL_024b: Unknown result type (might be due to invalid IL or missing references)
		//IL_025b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0364: Unknown result type (might be due to invalid IL or missing references)
		//IL_036d: Unknown result type (might be due to invalid IL or missing references)
		//IL_026c: Unknown result type (might be due to invalid IL or missing references)
		//IL_027a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0379: Unknown result type (might be due to invalid IL or missing references)
		//IL_037f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0391: Unknown result type (might be due to invalid IL or missing references)
		//IL_039a: Unknown result type (might be due to invalid IL or missing references)
		//IL_02af: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_048a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0490: Unknown result type (might be due to invalid IL or missing references)
		//IL_040e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0414: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_0499: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0429: Unknown result type (might be due to invalid IL or missing references)
		//IL_0436: Unknown result type (might be due to invalid IL or missing references)
		//IL_0442: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_050a: Unknown result type (might be due to invalid IL or missing references)
		//IL_04bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0453: Unknown result type (might be due to invalid IL or missing references)
		//IL_0461: Unknown result type (might be due to invalid IL or missing references)
		//IL_0469: Unknown result type (might be due to invalid IL or missing references)
		//IL_0522: Unknown result type (might be due to invalid IL or missing references)
		//IL_052b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0628: Unknown result type (might be due to invalid IL or missing references)
		//IL_0634: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0607: Unknown result type (might be due to invalid IL or missing references)
		//IL_0613: Unknown result type (might be due to invalid IL or missing references)
		//IL_053a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0541: Unknown result type (might be due to invalid IL or missing references)
		//IL_0550: Unknown result type (might be due to invalid IL or missing references)
		//IL_0682: Unknown result type (might be due to invalid IL or missing references)
		//IL_059e: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b4: Unknown result type (might be due to invalid IL or missing references)
		stair = false;
		stairFall = false;
		bool[] array = new bool[5];
		float y = Position.Y;
		float y2 = Position.Y;
		sloping = false;
		Vector2 vector2 = Position;
		Vector2 vector3 = Velocity;
		int value5 = (int)(Position.X / 16f) - 1;
		int value2 = (int)((Position.X + (float)Width) / 16f) + 2;
		int value3 = (int)(Position.Y / 16f) - 1;
		int value4 = (int)((Position.Y + (float)Height) / 16f) + 2;
		int num19 = Utils.Clamp(value5, 0, Main.maxTilesX - 1);
		value2 = Utils.Clamp(value2, 0, Main.maxTilesX - 1);
		value3 = Utils.Clamp(value3, 0, Main.maxTilesY - 1);
		value4 = Utils.Clamp(value4, 0, Main.maxTilesY - 1);
		Vector2 vector4 = default(Vector2);
		for (int i = num19; i < value2; i++)
		{
			for (int j = value3; j < value4; j++)
			{
				if (Main.tile[i, j] == null || !Main.tile[i, j].active() || Main.tile[i, j].inActive() || (!Main.tileSolid[Main.tile[i, j].type] && (!Main.tileSolidTop[Main.tile[i, j].type] || Main.tile[i, j].frameY != 0)))
				{
					continue;
				}
				vector4.X = i * 16;
				vector4.Y = j * 16;
				int num11 = 16;
				if (Main.tile[i, j].halfBrick())
				{
					vector4.Y += 8f;
					num11 -= 8;
				}
				if (!(Position.X + (float)Width > vector4.X) || !(Position.X < vector4.X + 16f) || !(Position.Y + (float)Height > vector4.Y) || !(Position.Y < vector4.Y + (float)num11))
				{
					continue;
				}
				bool flag = true;
				if (TileID.Sets.Platforms[Main.tile[i, j].type])
				{
					if (Velocity.Y < 0f)
					{
						flag = false;
					}
					if (Position.Y + (float)Height < (float)(j * 16) || Position.Y + (float)Height - (1f + Math.Abs(Velocity.X)) > (float)(j * 16 + 16))
					{
						flag = false;
					}
					if (((Main.tile[i, j].slope() == 1 && Velocity.X >= 0f) || (Main.tile[i, j].slope() == 2 && Velocity.X <= 0f)) && (Position.Y + (float)Height) / 16f - 1f == (float)j)
					{
						flag = false;
					}
				}
				if (!flag)
				{
					continue;
				}
				bool flag2 = false;
				if (fall && TileID.Sets.Platforms[Main.tile[i, j].type])
				{
					flag2 = true;
				}
				int num12 = Main.tile[i, j].slope();
				vector4.X = i * 16;
				vector4.Y = j * 16;
				if (!(Position.X + (float)Width > vector4.X) || !(Position.X < vector4.X + 16f) || !(Position.Y + (float)Height > vector4.Y) || !(Position.Y < vector4.Y + 16f))
				{
					continue;
				}
				float num13 = 0f;
				if (num12 == 3 || num12 == 4)
				{
					if (num12 == 3)
					{
						num13 = Position.X - vector4.X;
					}
					if (num12 == 4)
					{
						num13 = vector4.X + 16f - (Position.X + (float)Width);
					}
					if (num13 >= 0f)
					{
						if (Position.Y <= vector4.Y + 16f - num13)
						{
							float num14 = vector4.Y + 16f - Position.Y - num13;
							if (Position.Y + num14 > y2)
							{
								vector2.Y = Position.Y + num14;
								y2 = vector2.Y;
								if (vector3.Y < 0.0101f)
								{
									vector3.Y = 0.0101f;
								}
								array[num12] = true;
							}
						}
					}
					else if (Position.Y > vector4.Y)
					{
						float num15 = vector4.Y + 16f;
						if (vector2.Y < num15)
						{
							vector2.Y = num15;
							if (vector3.Y < 0.0101f)
							{
								vector3.Y = 0.0101f;
							}
						}
					}
				}
				if (num12 != 1 && num12 != 2)
				{
					continue;
				}
				if (num12 == 1)
				{
					num13 = Position.X - vector4.X;
				}
				if (num12 == 2)
				{
					num13 = vector4.X + 16f - (Position.X + (float)Width);
				}
				if (num13 >= 0f)
				{
					if (!(Position.Y + (float)Height >= vector4.Y + num13))
					{
						continue;
					}
					float num16 = vector4.Y - (Position.Y + (float)Height) + num13;
					if (!(Position.Y + num16 < y))
					{
						continue;
					}
					if (flag2)
					{
						stairFall = true;
						continue;
					}
					if (TileID.Sets.Platforms[Main.tile[i, j].type])
					{
						stair = true;
					}
					else
					{
						stair = false;
					}
					vector2.Y = Position.Y + num16;
					y = vector2.Y;
					if (vector3.Y > 0f)
					{
						vector3.Y = 0f;
					}
					array[num12] = true;
					continue;
				}
				if (TileID.Sets.Platforms[Main.tile[i, j].type] && !(Position.Y + (float)Height - 4f - Math.Abs(Velocity.X) <= vector4.Y))
				{
					if (flag2)
					{
						stairFall = true;
					}
					continue;
				}
				float num17 = vector4.Y - (float)Height;
				if (!(vector2.Y > num17))
				{
					continue;
				}
				if (flag2)
				{
					stairFall = true;
					continue;
				}
				if (TileID.Sets.Platforms[Main.tile[i, j].type])
				{
					stair = true;
				}
				else
				{
					stair = false;
				}
				vector2.Y = num17;
				if (vector3.Y > 0f)
				{
					vector3.Y = 0f;
				}
			}
		}
		Vector2 velocity = vector2 - Position;
		Vector2 vector5 = TileCollision(Position, velocity, Width, Height);
		if (vector5.Y > velocity.Y)
		{
			float num18 = velocity.Y - vector5.Y;
			vector2.Y = Position.Y + vector5.Y;
			if (array[1])
			{
				vector2.X = Position.X - num18;
			}
			if (array[2])
			{
				vector2.X = Position.X + num18;
			}
			vector3.X = 0f;
			vector3.Y = 0f;
			up = false;
		}
		else if (vector5.Y < velocity.Y)
		{
			float num10 = vector5.Y - velocity.Y;
			vector2.Y = Position.Y + vector5.Y;
			if (array[3])
			{
				vector2.X = Position.X - num10;
			}
			if (array[4])
			{
				vector2.X = Position.X + num10;
			}
			vector3.X = 0f;
			vector3.Y = 0f;
		}
		return new Vector4(vector2, vector3.X, vector3.Y);
	}

	public static Vector2 noSlopeCollision(Vector2 Position, Vector2 Velocity, int Width, int Height, bool fallThrough = false, bool fall2 = false)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_042b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_020a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0213: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_0324: Unknown result type (might be due to invalid IL or missing references)
		//IL_032a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0262: Unknown result type (might be due to invalid IL or missing references)
		//IL_0248: Unknown result type (might be due to invalid IL or missing references)
		//IL_039a: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_03dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0369: Unknown result type (might be due to invalid IL or missing references)
		//IL_0376: Unknown result type (might be due to invalid IL or missing references)
		//IL_0314: Unknown result type (might be due to invalid IL or missing references)
		//IL_028d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0294: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0402: Unknown result type (might be due to invalid IL or missing references)
		//IL_038d: Unknown result type (might be due to invalid IL or missing references)
		up = false;
		down = false;
		Vector2 result = Velocity;
		Vector2 vector2 = Position + Velocity;
		int value5 = (int)(Position.X / 16f) - 1;
		int value2 = (int)((Position.X + (float)Width) / 16f) + 2;
		int value3 = (int)(Position.Y / 16f) - 1;
		int value4 = (int)((Position.Y + (float)Height) / 16f) + 2;
		int num = -1;
		int num2 = -1;
		int num3 = -1;
		int num4 = -1;
		int num7 = Utils.Clamp(value5, 0, Main.maxTilesX - 1);
		value2 = Utils.Clamp(value2, 0, Main.maxTilesX - 1);
		value3 = Utils.Clamp(value3, 0, Main.maxTilesY - 1);
		value4 = Utils.Clamp(value4, 0, Main.maxTilesY - 1);
		float num5 = (value4 + 3) * 16;
		Vector2 vector3 = default(Vector2);
		for (int i = num7; i < value2; i++)
		{
			for (int j = value3; j < value4; j++)
			{
				if (Main.tile[i, j] == null || !Main.tile[i, j].active() || (!Main.tileSolid[Main.tile[i, j].type] && (!Main.tileSolidTop[Main.tile[i, j].type] || Main.tile[i, j].frameY != 0)))
				{
					continue;
				}
				vector3.X = i * 16;
				vector3.Y = j * 16;
				int num6 = 16;
				if (Main.tile[i, j].halfBrick())
				{
					vector3.Y += 8f;
					num6 -= 8;
				}
				if (!(vector2.X + (float)Width > vector3.X) || !(vector2.X < vector3.X + 16f) || !(vector2.Y + (float)Height > vector3.Y) || !(vector2.Y < vector3.Y + (float)num6))
				{
					continue;
				}
				if (Position.Y + (float)Height <= vector3.Y)
				{
					down = true;
					if ((!(Main.tileSolidTop[Main.tile[i, j].type] && fallThrough) || !(Velocity.Y <= 1f || fall2)) && num5 > vector3.Y)
					{
						num3 = i;
						num4 = j;
						if (num6 < 16)
						{
							num4++;
						}
						if (num3 != num)
						{
							result.Y = vector3.Y - (Position.Y + (float)Height);
							num5 = vector3.Y;
						}
					}
				}
				else if (Position.X + (float)Width <= vector3.X && !Main.tileSolidTop[Main.tile[i, j].type])
				{
					num = i;
					num2 = j;
					if (num2 != num4)
					{
						result.X = vector3.X - (Position.X + (float)Width);
					}
					if (num3 == num)
					{
						result.Y = Velocity.Y;
					}
				}
				else if (Position.X >= vector3.X + 16f && !Main.tileSolidTop[Main.tile[i, j].type])
				{
					num = i;
					num2 = j;
					if (num2 != num4)
					{
						result.X = vector3.X + 16f - Position.X;
					}
					if (num3 == num)
					{
						result.Y = Velocity.Y;
					}
				}
				else if (Position.Y >= vector3.Y + (float)num6 && !Main.tileSolidTop[Main.tile[i, j].type])
				{
					up = true;
					num3 = i;
					num4 = j;
					result.Y = vector3.Y + (float)num6 - Position.Y + 0.01f;
					if (num4 == num2)
					{
						result.X = Velocity.X;
					}
				}
			}
		}
		return result;
	}

	public static Vector2 TileCollision(Vector2 Position, Vector2 Velocity, int Width, int Height, bool fallThrough = false, bool fall2 = false, int gravDir = 1)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0204: Unknown result type (might be due to invalid IL or missing references)
		//IL_0210: Unknown result type (might be due to invalid IL or missing references)
		//IL_0216: Unknown result type (might be due to invalid IL or missing references)
		//IL_0263: Unknown result type (might be due to invalid IL or missing references)
		//IL_0269: Unknown result type (might be due to invalid IL or missing references)
		//IL_0275: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0328: Unknown result type (might be due to invalid IL or missing references)
		//IL_0331: Unknown result type (might be due to invalid IL or missing references)
		//IL_033d: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_027e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0284: Unknown result type (might be due to invalid IL or missing references)
		//IL_047a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0483: Unknown result type (might be due to invalid IL or missing references)
		//IL_0376: Unknown result type (might be due to invalid IL or missing references)
		//IL_037f: Unknown result type (might be due to invalid IL or missing references)
		//IL_038b: Unknown result type (might be due to invalid IL or missing references)
		//IL_034a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0350: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_056d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0573: Unknown result type (might be due to invalid IL or missing references)
		//IL_0412: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0398: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0659: Unknown result type (might be due to invalid IL or missing references)
		//IL_065f: Unknown result type (might be due to invalid IL or missing references)
		//IL_069c: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_053c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0543: Unknown result type (might be due to invalid IL or missing references)
		//IL_0444: Unknown result type (might be due to invalid IL or missing references)
		//IL_044b: Unknown result type (might be due to invalid IL or missing references)
		//IL_055d: Unknown result type (might be due to invalid IL or missing references)
		//IL_06cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0628: Unknown result type (might be due to invalid IL or missing references)
		//IL_0635: Unknown result type (might be due to invalid IL or missing references)
		//IL_046c: Unknown result type (might be due to invalid IL or missing references)
		//IL_064c: Unknown result type (might be due to invalid IL or missing references)
		up = false;
		down = false;
		Vector2 result = Velocity;
		Vector2 vector2 = Position + Velocity;
		int value5 = (int)(Position.X / 16f) - 1;
		int value2 = (int)((Position.X + (float)Width) / 16f) + 2;
		int value3 = (int)(Position.Y / 16f) - 1;
		int value4 = (int)((Position.Y + (float)Height) / 16f) + 2;
		int num = -1;
		int num2 = -1;
		int num3 = -1;
		int num4 = -1;
		int num7 = Utils.Clamp(value5, 0, Main.maxTilesX - 1);
		value2 = Utils.Clamp(value2, 0, Main.maxTilesX - 1);
		value3 = Utils.Clamp(value3, 0, Main.maxTilesY - 1);
		value4 = Utils.Clamp(value4, 0, Main.maxTilesY - 1);
		float num5 = (value4 + 3) * 16;
		Vector2 vector3 = default(Vector2);
		for (int i = num7; i < value2; i++)
		{
			for (int j = value3; j < value4; j++)
			{
				if (Main.tile[i, j] == null || !Main.tile[i, j].active() || Main.tile[i, j].inActive() || (!Main.tileSolid[Main.tile[i, j].type] && (!Main.tileSolidTop[Main.tile[i, j].type] || Main.tile[i, j].frameY != 0)))
				{
					continue;
				}
				vector3.X = i * 16;
				vector3.Y = j * 16;
				int num6 = 16;
				if (Main.tile[i, j].halfBrick())
				{
					vector3.Y += 8f;
					num6 -= 8;
				}
				if (!(vector2.X + (float)Width > vector3.X) || !(vector2.X < vector3.X + 16f) || !(vector2.Y + (float)Height > vector3.Y) || !(vector2.Y < vector3.Y + (float)num6))
				{
					continue;
				}
				bool flag = false;
				bool flag2 = false;
				if (Main.tile[i, j].slope() > 2)
				{
					if (Main.tile[i, j].slope() == 3 && Position.Y + Math.Abs(Velocity.X) >= vector3.Y && Position.X >= vector3.X)
					{
						flag2 = true;
					}
					if (Main.tile[i, j].slope() == 4 && Position.Y + Math.Abs(Velocity.X) >= vector3.Y && Position.X + (float)Width <= vector3.X + 16f)
					{
						flag2 = true;
					}
				}
				else if (Main.tile[i, j].slope() > 0)
				{
					flag = true;
					if (Main.tile[i, j].slope() == 1 && Position.Y + (float)Height - Math.Abs(Velocity.X) <= vector3.Y + (float)num6 && Position.X >= vector3.X)
					{
						flag2 = true;
					}
					if (Main.tile[i, j].slope() == 2 && Position.Y + (float)Height - Math.Abs(Velocity.X) <= vector3.Y + (float)num6 && Position.X + (float)Width <= vector3.X + 16f)
					{
						flag2 = true;
					}
				}
				if (flag2)
				{
					continue;
				}
				if (Position.Y + (float)Height <= vector3.Y)
				{
					down = true;
					if ((!(Main.tileSolidTop[Main.tile[i, j].type] && fallThrough) || !(Velocity.Y <= 1f || fall2)) && num5 > vector3.Y)
					{
						num3 = i;
						num4 = j;
						if (num6 < 16)
						{
							num4++;
						}
						if (num3 != num && !flag)
						{
							result.Y = vector3.Y - (Position.Y + (float)Height) + ((gravDir == -1) ? (-0.01f) : 0f);
							num5 = vector3.Y;
						}
					}
				}
				else if (Position.X + (float)Width <= vector3.X && !Main.tileSolidTop[Main.tile[i, j].type])
				{
					if (i >= 1 && Main.tile[i - 1, j] == null)
					{
						Main.tile[i - 1, j] = default(Tile);
					}
					if (i < 1 || (Main.tile[i - 1, j].slope() != 2 && Main.tile[i - 1, j].slope() != 4))
					{
						num = i;
						num2 = j;
						if (num2 != num4)
						{
							result.X = vector3.X - (Position.X + (float)Width);
						}
						if (num3 == num)
						{
							result.Y = Velocity.Y;
						}
					}
				}
				else if (Position.X >= vector3.X + 16f && !Main.tileSolidTop[Main.tile[i, j].type])
				{
					if (Main.tile[i + 1, j] == null)
					{
						Main.tile[i + 1, j] = default(Tile);
					}
					if (Main.tile[i + 1, j].slope() != 1 && Main.tile[i + 1, j].slope() != 3)
					{
						num = i;
						num2 = j;
						if (num2 != num4)
						{
							result.X = vector3.X + 16f - Position.X;
						}
						if (num3 == num)
						{
							result.Y = Velocity.Y;
						}
					}
				}
				else if (Position.Y >= vector3.Y + (float)num6 && !Main.tileSolidTop[Main.tile[i, j].type])
				{
					up = true;
					num3 = i;
					num4 = j;
					result.Y = vector3.Y + (float)num6 - Position.Y + ((gravDir == 1) ? 0.01f : 0f);
					if (num4 == num2)
					{
						result.X = Velocity.X;
					}
				}
			}
		}
		return result;
	}

	public static bool IsClearSpotTest(Vector2 position, float testMagnitude, int Width, int Height, bool fallThrough = false, bool fall2 = false, int gravDir = 1, bool checkCardinals = true, bool checkSlopes = false)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0165: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0170: Unknown result type (might be due to invalid IL or missing references)
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0193: Unknown result type (might be due to invalid IL or missing references)
		//IL_0198: Unknown result type (might be due to invalid IL or missing references)
		if (checkCardinals)
		{
			Vector2 vector2 = Vector2.UnitX * testMagnitude;
			if (TileCollision(position - vector2, vector2, Width, Height, fallThrough, fall2, gravDir) != vector2)
			{
				return false;
			}
			vector2 = -Vector2.UnitX * testMagnitude;
			if (TileCollision(position - vector2, vector2, Width, Height, fallThrough, fall2, gravDir) != vector2)
			{
				return false;
			}
			vector2 = Vector2.UnitY * testMagnitude;
			if (TileCollision(position - vector2, vector2, Width, Height, fallThrough, fall2, gravDir) != vector2)
			{
				return false;
			}
			vector2 = -Vector2.UnitY * testMagnitude;
			if (TileCollision(position - vector2, vector2, Width, Height, fallThrough, fall2, gravDir) != vector2)
			{
				return false;
			}
		}
		if (checkSlopes)
		{
			Vector2 vector = Vector2.UnitX * testMagnitude;
			Vector4 vector3 = default(Vector4);
			((Vector4)(ref vector3))._002Ector(position, testMagnitude, 0f);
			if (SlopeCollision(position, vector, Width, Height, gravDir, fallThrough) != vector3)
			{
				return false;
			}
			vector = -Vector2.UnitX * testMagnitude;
			((Vector4)(ref vector3))._002Ector(position, 0f - testMagnitude, 0f);
			if (SlopeCollision(position, vector, Width, Height, gravDir, fallThrough) != vector3)
			{
				return false;
			}
			vector = Vector2.UnitY * testMagnitude;
			((Vector4)(ref vector3))._002Ector(position, 0f, testMagnitude);
			if (SlopeCollision(position, vector, Width, Height, gravDir, fallThrough) != vector3)
			{
				return false;
			}
			vector = -Vector2.UnitY * testMagnitude;
			((Vector4)(ref vector3))._002Ector(position, 0f, 0f - testMagnitude);
			if (SlopeCollision(position, vector, Width, Height, gravDir, fallThrough) != vector3)
			{
				return false;
			}
		}
		return true;
	}

	public static List<Point> FindCollisionTile(int Direction, Vector2 position, float testMagnitude, int Width, int Height, bool fallThrough = false, bool fall2 = false, int gravDir = 1, bool checkCardinals = true, bool checkSlopes = false)
	{
		//IL_028c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0292: Unknown result type (might be due to invalid IL or missing references)
		//IL_027a: Unknown result type (might be due to invalid IL or missing references)
		//IL_027f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0285: Unknown result type (might be due to invalid IL or missing references)
		//IL_0297: Unknown result type (might be due to invalid IL or missing references)
		//IL_029b: Unknown result type (might be due to invalid IL or missing references)
		//IL_029c: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02af: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0301: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_032d: Unknown result type (might be due to invalid IL or missing references)
		//IL_032e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0339: Unknown result type (might be due to invalid IL or missing references)
		//IL_033e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0343: Unknown result type (might be due to invalid IL or missing references)
		//IL_0345: Unknown result type (might be due to invalid IL or missing references)
		//IL_030d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0317: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0357: Unknown result type (might be due to invalid IL or missing references)
		//IL_0361: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0374: Unknown result type (might be due to invalid IL or missing references)
		//IL_0375: Unknown result type (might be due to invalid IL or missing references)
		//IL_0377: Unknown result type (might be due to invalid IL or missing references)
		//IL_037c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0383: Unknown result type (might be due to invalid IL or missing references)
		//IL_0388: Unknown result type (might be due to invalid IL or missing references)
		//IL_038d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0398: Unknown result type (might be due to invalid IL or missing references)
		//IL_039d: Unknown result type (might be due to invalid IL or missing references)
		//IL_043d: Unknown result type (might be due to invalid IL or missing references)
		//IL_043e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0440: Unknown result type (might be due to invalid IL or missing references)
		//IL_0445: Unknown result type (might be due to invalid IL or missing references)
		//IL_044c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0451: Unknown result type (might be due to invalid IL or missing references)
		//IL_0456: Unknown result type (might be due to invalid IL or missing references)
		//IL_0461: Unknown result type (might be due to invalid IL or missing references)
		//IL_0466: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_048f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0490: Unknown result type (might be due to invalid IL or missing references)
		//IL_0497: Unknown result type (might be due to invalid IL or missing references)
		//IL_049c: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0472: Unknown result type (might be due to invalid IL or missing references)
		//IL_047f: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_040a: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_016d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		//IL_017c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Unknown result type (might be due to invalid IL or missing references)
		//IL_0182: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0223: Unknown result type (might be due to invalid IL or missing references)
		//IL_0224: Unknown result type (might be due to invalid IL or missing references)
		//IL_022b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0230: Unknown result type (might be due to invalid IL or missing references)
		//IL_0235: Unknown result type (might be due to invalid IL or missing references)
		//IL_023f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0244: Unknown result type (might be due to invalid IL or missing references)
		//IL_0249: Unknown result type (might be due to invalid IL or missing references)
		//IL_024a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_020d: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_025a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0268: Unknown result type (might be due to invalid IL or missing references)
		List<Point> list = new List<Point>();
		if ((uint)Direction > 1u)
		{
			if ((uint)(Direction - 2) <= 1u)
			{
				Vector2 vector = ((Direction == 2) ? (Vector2.UnitY * testMagnitude) : (-Vector2.UnitY * testMagnitude));
				Vector4 vec = default(Vector4);
				((Vector4)(ref vec))._002Ector(position, vector.X, vector.Y);
				int y = (int)(position.Y + (float)((Direction == 2) ? Height : 0)) / 16;
				float num = Math.Min(16f - position.X % 16f, Width);
				float num2 = num;
				if (checkCardinals && TileCollision(position - vector, vector, (int)num, Height, fallThrough, fall2, gravDir) != vector)
				{
					list.Add(new Point((int)position.X / 16, y));
				}
				else if (checkSlopes && SlopeCollision(position, vector, (int)num, Height, gravDir, fallThrough).YZW() != vec.YZW())
				{
					list.Add(new Point((int)position.X / 16, y));
				}
				for (; num2 + 16f <= (float)(Width - 16); num2 += 16f)
				{
					if (checkCardinals && TileCollision(position - vector + Vector2.UnitX * num2, vector, 16, Height, fallThrough, fall2, gravDir) != vector)
					{
						list.Add(new Point((int)(position.X + num2) / 16, y));
					}
					else if (checkSlopes && SlopeCollision(position + Vector2.UnitX * num2, vector, 16, Height, gravDir, fallThrough).YZW() != vec.YZW())
					{
						list.Add(new Point((int)(position.X + num2) / 16, y));
					}
				}
				int width = Width - (int)num2;
				if (checkCardinals && TileCollision(position - vector + Vector2.UnitX * num2, vector, width, Height, fallThrough, fall2, gravDir) != vector)
				{
					list.Add(new Point((int)(position.X + num2) / 16, y));
				}
				else if (checkSlopes && SlopeCollision(position + Vector2.UnitX * num2, vector, width, Height, gravDir, fallThrough).YZW() != vec.YZW())
				{
					list.Add(new Point((int)(position.X + num2) / 16, y));
				}
			}
		}
		else
		{
			Vector2 vector2 = ((Direction == 0) ? (Vector2.UnitX * testMagnitude) : (-Vector2.UnitX * testMagnitude));
			Vector4 vec2 = default(Vector4);
			((Vector4)(ref vec2))._002Ector(position, vector2.X, vector2.Y);
			int y2 = (int)(position.X + (float)((Direction == 0) ? Width : 0)) / 16;
			float num3 = Math.Min(16f - position.Y % 16f, Height);
			float num4 = num3;
			if (checkCardinals && TileCollision(position - vector2, vector2, Width, (int)num3, fallThrough, fall2, gravDir) != vector2)
			{
				list.Add(new Point(y2, (int)position.Y / 16));
			}
			else if (checkSlopes && SlopeCollision(position, vector2, Width, (int)num3, gravDir, fallThrough).XZW() != vec2.XZW())
			{
				list.Add(new Point(y2, (int)position.Y / 16));
			}
			for (; num4 + 16f <= (float)(Height - 16); num4 += 16f)
			{
				if (checkCardinals && TileCollision(position - vector2 + Vector2.UnitY * num4, vector2, Width, 16, fallThrough, fall2, gravDir) != vector2)
				{
					list.Add(new Point(y2, (int)(position.Y + num4) / 16));
				}
				else if (checkSlopes && SlopeCollision(position + Vector2.UnitY * num4, vector2, Width, 16, gravDir, fallThrough).XZW() != vec2.XZW())
				{
					list.Add(new Point(y2, (int)(position.Y + num4) / 16));
				}
			}
			int height = Height - (int)num4;
			if (checkCardinals && TileCollision(position - vector2 + Vector2.UnitY * num4, vector2, Width, height, fallThrough, fall2, gravDir) != vector2)
			{
				list.Add(new Point(y2, (int)(position.Y + num4) / 16));
			}
			else if (checkSlopes && SlopeCollision(position + Vector2.UnitY * num4, vector2, Width, height, gravDir, fallThrough).XZW() != vec2.XZW())
			{
				list.Add(new Point(y2, (int)(position.Y + num4) / 16));
			}
		}
		return list;
	}

	public static bool FindCollisionDirection(out int Direction, Vector2 position, int Width, int Height, bool fallThrough = false, bool fall2 = false, int gravDir = 1)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		Vector2 vector = Vector2.UnitX * 16f;
		if (TileCollision(position - vector, vector, Width, Height, fallThrough, fall2, gravDir) != vector)
		{
			Direction = 0;
			return true;
		}
		vector = -Vector2.UnitX * 16f;
		if (TileCollision(position - vector, vector, Width, Height, fallThrough, fall2, gravDir) != vector)
		{
			Direction = 1;
			return true;
		}
		vector = Vector2.UnitY * 16f;
		if (TileCollision(position - vector, vector, Width, Height, fallThrough, fall2, gravDir) != vector)
		{
			Direction = 2;
			return true;
		}
		vector = -Vector2.UnitY * 16f;
		if (TileCollision(position - vector, vector, Width, Height, fallThrough, fall2, gravDir) != vector)
		{
			Direction = 3;
			return true;
		}
		Direction = -1;
		return false;
	}

	public static bool SolidCollision(Vector2 Position, int Width, int Height)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0183: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0191: Unknown result type (might be due to invalid IL or missing references)
		//IL_019f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
		int value5 = (int)(Position.X / 16f) - 1;
		int value2 = (int)((Position.X + (float)Width) / 16f) + 2;
		int value3 = (int)(Position.Y / 16f) - 1;
		int value4 = (int)((Position.Y + (float)Height) / 16f) + 2;
		int num3 = Utils.Clamp(value5, 0, Main.maxTilesX - 1);
		value2 = Utils.Clamp(value2, 0, Main.maxTilesX - 1);
		value3 = Utils.Clamp(value3, 0, Main.maxTilesY - 1);
		value4 = Utils.Clamp(value4, 0, Main.maxTilesY - 1);
		Vector2 vector = default(Vector2);
		for (int i = num3; i < value2; i++)
		{
			for (int j = value3; j < value4; j++)
			{
				if (Main.tile[i, j] != null && !Main.tile[i, j].inActive() && Main.tile[i, j].active() && Main.tileSolid[Main.tile[i, j].type] && !Main.tileSolidTop[Main.tile[i, j].type])
				{
					vector.X = i * 16;
					vector.Y = j * 16;
					int num2 = 16;
					if (Main.tile[i, j].halfBrick())
					{
						vector.Y += 8f;
						num2 -= 8;
					}
					if (Position.X + (float)Width > vector.X && Position.X < vector.X + 16f && Position.Y + (float)Height > vector.Y && Position.Y < vector.Y + (float)num2)
					{
						return true;
					}
				}
			}
		}
		return false;
	}

	public static bool SolidCollision(Vector2 Position, int Width, int Height, bool acceptTopSurfaces)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_0189: Unknown result type (might be due to invalid IL or missing references)
		//IL_0191: Unknown result type (might be due to invalid IL or missing references)
		//IL_0197: Unknown result type (might be due to invalid IL or missing references)
		int value5 = (int)(Position.X / 16f) - 1;
		int value2 = (int)((Position.X + (float)Width) / 16f) + 2;
		int value3 = (int)(Position.Y / 16f) - 1;
		int value4 = (int)((Position.Y + (float)Height) / 16f) + 2;
		int num3 = Utils.Clamp(value5, 0, Main.maxTilesX - 1);
		value2 = Utils.Clamp(value2, 0, Main.maxTilesX - 1);
		value3 = Utils.Clamp(value3, 0, Main.maxTilesY - 1);
		value4 = Utils.Clamp(value4, 0, Main.maxTilesY - 1);
		Vector2 vector = default(Vector2);
		for (int i = num3; i < value2; i++)
		{
			for (int j = value3; j < value4; j++)
			{
				Tile tile = Main.tile[i, j];
				if (tile == null || !tile.active() || tile.inActive())
				{
					continue;
				}
				bool flag = Main.tileSolid[tile.type] && !Main.tileSolidTop[tile.type];
				if (acceptTopSurfaces)
				{
					flag |= Main.tileSolidTop[tile.type] && tile.frameY == 0;
				}
				if (flag)
				{
					vector.X = i * 16;
					vector.Y = j * 16;
					int num2 = 16;
					if (tile.halfBrick())
					{
						vector.Y += 8f;
						num2 -= 8;
					}
					if (Position.X + (float)Width > vector.X && Position.X < vector.X + 16f && Position.Y + (float)Height > vector.Y && Position.Y < vector.Y + (float)num2)
					{
						return true;
					}
				}
			}
		}
		return false;
	}

	public static Vector2 WaterCollision(Vector2 Position, Vector2 Velocity, int Width, int Height, bool fallThrough = false, bool fall2 = false, bool lavaWalk = true)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0174: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0189: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_019b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
		Vector2 result = Velocity;
		Vector2 vector = Position + Velocity;
		int value5 = (int)(Position.X / 16f) - 1;
		int value2 = (int)((Position.X + (float)Width) / 16f) + 2;
		int value3 = (int)(Position.Y / 16f) - 1;
		int value4 = (int)((Position.Y + (float)Height) / 16f) + 2;
		int num3 = Utils.Clamp(value5, 0, Main.maxTilesX - 1);
		value2 = Utils.Clamp(value2, 0, Main.maxTilesX - 1);
		value3 = Utils.Clamp(value3, 0, Main.maxTilesY - 1);
		value4 = Utils.Clamp(value4, 0, Main.maxTilesY - 1);
		Vector2 vector2 = default(Vector2);
		for (int i = num3; i < value2; i++)
		{
			for (int j = value3; j < value4; j++)
			{
				if (Main.tile[i, j] != null && Main.tile[i, j].liquid > 0 && Main.tile[i, j - 1].liquid == 0 && (!Main.tile[i, j].lava() || lavaWalk))
				{
					int num2 = Main.tile[i, j].liquid / 32 * 2 + 2;
					vector2.X = i * 16;
					vector2.Y = j * 16 + 16 - num2;
					if (vector.X + (float)Width > vector2.X && vector.X < vector2.X + 16f && vector.Y + (float)Height > vector2.Y && vector.Y < vector2.Y + (float)num2 && Position.Y + (float)Height <= vector2.Y && !fallThrough)
					{
						result.Y = vector2.Y - (Position.Y + (float)Height);
					}
				}
			}
		}
		return result;
	}

	public static Vector2 AnyCollisionWithSpecificTiles(Vector2 Position, Vector2 Velocity, int Width, int Height, bool[] tilesWeCanCollideWithByType, bool evenActuated = false)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_0313: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_017c: Unknown result type (might be due to invalid IL or missing references)
		//IL_018c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_022f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0235: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0292: Unknown result type (might be due to invalid IL or missing references)
		//IL_0298: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_0264: Unknown result type (might be due to invalid IL or missing references)
		//IL_0271: Unknown result type (might be due to invalid IL or missing references)
		//IL_021f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0285: Unknown result type (might be due to invalid IL or missing references)
		Vector2 result = Velocity;
		Vector2 vector2 = Position + Velocity;
		int num = (int)(Position.X / 16f) - 1;
		int num2 = (int)((Position.X + (float)Width) / 16f) + 2;
		int num3 = (int)(Position.Y / 16f) - 1;
		int num4 = (int)((Position.Y + (float)Height) / 16f) + 2;
		int num5 = -1;
		int num6 = -1;
		int num7 = -1;
		int num8 = -1;
		if (num < 0)
		{
			num = 0;
		}
		if (num2 > Main.maxTilesX)
		{
			num2 = Main.maxTilesX;
		}
		if (num3 < 0)
		{
			num3 = 0;
		}
		if (num4 > Main.maxTilesY)
		{
			num4 = Main.maxTilesY;
		}
		Vector2 vector3 = default(Vector2);
		for (int i = num; i < num2; i++)
		{
			for (int j = num3; j < num4; j++)
			{
				Tile tile = Main.tile[i, j];
				if (tile == null || !tile.active() || (!evenActuated && tile.inActive()) || !tilesWeCanCollideWithByType[tile.type])
				{
					continue;
				}
				vector3.X = i * 16;
				vector3.Y = j * 16;
				int num9 = 16;
				if (tile.halfBrick())
				{
					vector3.Y += 8f;
					num9 -= 8;
				}
				if (!(vector2.X + (float)Width > vector3.X) || !(vector2.X < vector3.X + 16f) || !(vector2.Y + (float)Height > vector3.Y) || !(vector2.Y < vector3.Y + (float)num9))
				{
					continue;
				}
				if (Position.Y + (float)Height <= vector3.Y)
				{
					num7 = i;
					num8 = j;
					if (num7 != num5)
					{
						result.Y = vector3.Y - (Position.Y + (float)Height);
					}
				}
				else if (Position.X + (float)Width <= vector3.X && !Main.tileSolidTop[tile.type])
				{
					num5 = i;
					num6 = j;
					if (num6 != num8)
					{
						result.X = vector3.X - (Position.X + (float)Width);
					}
					if (num7 == num5)
					{
						result.Y = Velocity.Y;
					}
				}
				else if (Position.X >= vector3.X + 16f && !Main.tileSolidTop[tile.type])
				{
					num5 = i;
					num6 = j;
					if (num6 != num8)
					{
						result.X = vector3.X + 16f - Position.X;
					}
					if (num7 == num5)
					{
						result.Y = Velocity.Y;
					}
				}
				else if (Position.Y >= vector3.Y + (float)num9 && !Main.tileSolidTop[tile.type])
				{
					num7 = i;
					num8 = j;
					result.Y = vector3.Y + (float)num9 - Position.Y + 0.01f;
					if (num8 == num6)
					{
						result.X = Velocity.X + 0.01f;
					}
				}
			}
		}
		return result;
	}

	public static Vector2 AnyCollision(Vector2 Position, Vector2 Velocity, int Width, int Height, bool evenActuated = false)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_0362: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		//IL_0165: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_0198: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_025b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0261: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_022a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0231: Unknown result type (might be due to invalid IL or missing references)
		//IL_030e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0319: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_024b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0333: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c4: Unknown result type (might be due to invalid IL or missing references)
		Vector2 result = Velocity;
		Vector2 vector2 = Position + Velocity;
		int num = (int)(Position.X / 16f) - 1;
		int num2 = (int)((Position.X + (float)Width) / 16f) + 2;
		int num3 = (int)(Position.Y / 16f) - 1;
		int num4 = (int)((Position.Y + (float)Height) / 16f) + 2;
		int num5 = -1;
		int num6 = -1;
		int num7 = -1;
		int num8 = -1;
		if (num < 0)
		{
			num = 0;
		}
		if (num2 > Main.maxTilesX)
		{
			num2 = Main.maxTilesX;
		}
		if (num3 < 0)
		{
			num3 = 0;
		}
		if (num4 > Main.maxTilesY)
		{
			num4 = Main.maxTilesY;
		}
		Vector2 vector3 = default(Vector2);
		for (int i = num; i < num2; i++)
		{
			for (int j = num3; j < num4; j++)
			{
				if (Main.tile[i, j] == null || !Main.tile[i, j].active() || (!evenActuated && Main.tile[i, j].inActive()))
				{
					continue;
				}
				vector3.X = i * 16;
				vector3.Y = j * 16;
				int num9 = 16;
				if (Main.tile[i, j].halfBrick())
				{
					vector3.Y += 8f;
					num9 -= 8;
				}
				if (!(vector2.X + (float)Width > vector3.X) || !(vector2.X < vector3.X + 16f) || !(vector2.Y + (float)Height > vector3.Y) || !(vector2.Y < vector3.Y + (float)num9))
				{
					continue;
				}
				if (Position.Y + (float)Height <= vector3.Y)
				{
					num7 = i;
					num8 = j;
					if (num7 != num5)
					{
						result.Y = vector3.Y - (Position.Y + (float)Height);
					}
				}
				else if (Position.X + (float)Width <= vector3.X && !Main.tileSolidTop[Main.tile[i, j].type])
				{
					num5 = i;
					num6 = j;
					if (num6 != num8)
					{
						result.X = vector3.X - (Position.X + (float)Width);
					}
					if (num7 == num5)
					{
						result.Y = Velocity.Y;
					}
				}
				else if (Position.X >= vector3.X + 16f && !Main.tileSolidTop[Main.tile[i, j].type])
				{
					num5 = i;
					num6 = j;
					if (num6 != num8)
					{
						result.X = vector3.X + 16f - Position.X;
					}
					if (num7 == num5)
					{
						result.Y = Velocity.Y;
					}
				}
				else if (Position.Y >= vector3.Y + (float)num9 && !Main.tileSolidTop[Main.tile[i, j].type])
				{
					num7 = i;
					num8 = j;
					result.Y = vector3.Y + (float)num9 - Position.Y + 0.01f;
					if (num8 == num6)
					{
						result.X = Velocity.X + 0.01f;
					}
				}
			}
		}
		return result;
	}

	public static void HitTiles(Vector2 Position, Vector2 Velocity, int Width, int Height)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0198: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
		Vector2 vector = Position + Velocity;
		int num = (int)(Position.X / 16f) - 1;
		int num2 = (int)((Position.X + (float)Width) / 16f) + 2;
		int num3 = (int)(Position.Y / 16f) - 1;
		int num4 = (int)((Position.Y + (float)Height) / 16f) + 2;
		if (num < 0)
		{
			num = 0;
		}
		if (num2 > Main.maxTilesX)
		{
			num2 = Main.maxTilesX;
		}
		if (num3 < 0)
		{
			num3 = 0;
		}
		if (num4 > Main.maxTilesY)
		{
			num4 = Main.maxTilesY;
		}
		Vector2 vector2 = default(Vector2);
		for (int i = num; i < num2; i++)
		{
			for (int j = num3; j < num4; j++)
			{
				if (Main.tile[i, j] != null && !Main.tile[i, j].inActive() && Main.tile[i, j].active() && (Main.tileSolid[Main.tile[i, j].type] || (Main.tileSolidTop[Main.tile[i, j].type] && Main.tile[i, j].frameY == 0)))
				{
					vector2.X = i * 16;
					vector2.Y = j * 16;
					int num5 = 16;
					if (Main.tile[i, j].halfBrick())
					{
						vector2.Y += 8f;
						num5 -= 8;
					}
					if (vector.X + (float)Width >= vector2.X && vector.X <= vector2.X + 16f && vector.Y + (float)Height >= vector2.Y && vector.Y <= vector2.Y + (float)num5)
					{
						WorldGen.KillTile(i, j, fail: true, effectOnly: true);
					}
				}
			}
		}
	}

	public static bool AnyHurtingTiles(Vector2 Position, int Width, int Height)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		return HurtTiles(Position, Width, Height, null).type >= 0;
	}

	public static HurtTile HurtTiles(Vector2 Position, int Width, int Height, Player player)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Unknown result type (might be due to invalid IL or missing references)
		//IL_021d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0226: Unknown result type (might be due to invalid IL or missing references)
		int num = (int)(Position.X / 16f) - 1;
		int num2 = (int)((Position.X + (float)Width) / 16f) + 2;
		int num3 = (int)(Position.Y / 16f) - 1;
		int num4 = (int)((Position.Y + (float)Height) / 16f) + 2;
		if (num < 0)
		{
			num = 0;
		}
		if (num2 > Main.maxTilesX)
		{
			num2 = Main.maxTilesX;
		}
		if (num3 < 0)
		{
			num3 = 0;
		}
		if (num4 > Main.maxTilesY)
		{
			num4 = Main.maxTilesY;
		}
		Vector2 vector = default(Vector2);
		HurtTile result;
		for (int i = num; i < num2; i++)
		{
			for (int j = num3; j < num4; j++)
			{
				Tile tile = Main.tile[i, j];
				if (tile == null || tile.inActive() || !tile.active())
				{
					continue;
				}
				vector.X = i * 16;
				vector.Y = j * 16;
				int num5 = 16;
				if (tile.halfBrick())
				{
					vector.Y += 8f;
					num5 -= 8;
				}
				int num6 = 0;
				if (TileID.Sets.Suffocate[tile.type])
				{
					num6 = 2;
				}
				if (Position.X + (float)Width - (float)num6 < vector.X || Position.X + (float)num6 > vector.X + 16f || Position.Y + (float)Height - (float)num6 < vector.Y - 0.5f || Position.Y + (float)num6 > vector.Y + (float)num5 + 0.5f || !CanTileHurt(tile.type, i, j, player))
				{
					continue;
				}
				if (tile.slope() > 0)
				{
					if (num6 > 0)
					{
						continue;
					}
					int num7 = 0;
					if (tile.rightSlope() && Position.X > vector.X)
					{
						num7++;
					}
					if (tile.leftSlope() && Position.X + (float)Width < vector.X + 16f)
					{
						num7++;
					}
					if (tile.bottomSlope() && Position.Y > vector.Y)
					{
						num7++;
					}
					if (tile.topSlope() && Position.Y + (float)Height < vector.Y + (float)num5)
					{
						num7++;
					}
					if (num7 == 2)
					{
						continue;
					}
				}
				result = default(HurtTile);
				result.type = tile.type;
				result.x = i;
				result.y = j;
				return result;
			}
		}
		result = default(HurtTile);
		result.type = -1;
		return result;
	}

	public static bool CanTileHurt(ushort type, int i, int j, Player player)
	{
		if (type == 230 && !Main.getGoodWorld)
		{
			return false;
		}
		if (type == 80 && !Main.dontStarveWorld)
		{
			return false;
		}
		if (TileID.Sets.TouchDamageBleeding[type] || TileID.Sets.Suffocate[type] || TileID.Sets.TouchDamageImmediate[type] > 0)
		{
			return true;
		}
		if (TileID.Sets.TouchDamageHot[type] && (player == null || !player.fireWalk))
		{
			return true;
		}
		return false;
	}

	public static bool SwitchTiles(Vector2 Position, int Width, int Height, Vector2 oldPosition, int objType)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_027e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0287: Unknown result type (might be due to invalid IL or missing references)
		//IL_0293: Unknown result type (might be due to invalid IL or missing references)
		//IL_0299: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0217: Unknown result type (might be due to invalid IL or missing references)
		//IL_021d: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0236: Unknown result type (might be due to invalid IL or missing references)
		//IL_023c: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0308: Unknown result type (might be due to invalid IL or missing references)
		//IL_030e: Unknown result type (might be due to invalid IL or missing references)
		//IL_031d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0326: Unknown result type (might be due to invalid IL or missing references)
		//IL_032f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0336: Unknown result type (might be due to invalid IL or missing references)
		int num = (int)(Position.X / 16f) - 1;
		int num2 = (int)((Position.X + (float)Width) / 16f) + 2;
		int num3 = (int)(Position.Y / 16f) - 1;
		int num4 = (int)((Position.Y + (float)Height) / 16f) + 2;
		if (num < 0)
		{
			num = 0;
		}
		if (num2 > Main.maxTilesX)
		{
			num2 = Main.maxTilesX;
		}
		if (num3 < 0)
		{
			num3 = 0;
		}
		if (num4 > Main.maxTilesY)
		{
			num4 = Main.maxTilesY;
		}
		Vector2 vector = default(Vector2);
		for (int i = num; i < num2; i++)
		{
			for (int j = num3; j < num4; j++)
			{
				if (Main.tile[i, j] == null)
				{
					continue;
				}
				int type = Main.tile[i, j].type;
				if (!Main.tile[i, j].active() || (type != 135 && type != 210 && type != 443 && type != 442))
				{
					continue;
				}
				vector.X = i * 16;
				vector.Y = j * 16 + 12;
				bool flag = false;
				if (type == 442)
				{
					if (objType == 4)
					{
						float r1StartX = 0f;
						float r1StartY = 0f;
						float r1Width = 0f;
						float r1Height = 0f;
						switch (Main.tile[i, j].frameX / 22)
						{
						case 0:
							r1StartX = i * 16;
							r1StartY = j * 16 + 16 - 10;
							r1Width = 16f;
							r1Height = 10f;
							break;
						case 1:
							r1StartX = i * 16;
							r1StartY = j * 16;
							r1Width = 16f;
							r1Height = 10f;
							break;
						case 2:
							r1StartX = i * 16;
							r1StartY = j * 16;
							r1Width = 10f;
							r1Height = 16f;
							break;
						case 3:
							r1StartX = i * 16 + 16 - 10;
							r1StartY = j * 16;
							r1Width = 10f;
							r1Height = 16f;
							break;
						}
						if (Utils.FloatIntersect(r1StartX, r1StartY, r1Width, r1Height, Position.X, Position.Y, Width, Height) && !Utils.FloatIntersect(r1StartX, r1StartY, r1Width, r1Height, oldPosition.X, oldPosition.Y, Width, Height))
						{
							Wiring.HitSwitch(i, j);
							NetMessage.SendData(59, -1, -1, null, i, j);
							return true;
						}
					}
					flag = true;
				}
				if (flag || !(Position.X + (float)Width > vector.X) || !(Position.X < vector.X + 16f) || !(Position.Y + (float)Height > vector.Y) || !((double)Position.Y < (double)vector.Y + 4.01))
				{
					continue;
				}
				if (type == 210)
				{
					WorldGen.ExplodeMine(i, j, fromWiring: false);
				}
				else
				{
					if (oldPosition.X + (float)Width > vector.X && oldPosition.X < vector.X + 16f && oldPosition.Y + (float)Height > vector.Y && (double)oldPosition.Y < (double)vector.Y + 16.01)
					{
						continue;
					}
					if (type == 443)
					{
						if (objType == 1)
						{
							Wiring.HitSwitch(i, j);
							NetMessage.SendData(59, -1, -1, null, i, j);
						}
						continue;
					}
					int num5 = Main.tile[i, j].frameY / 18;
					bool flag2 = true;
					if ((num5 == 4 || num5 == 2 || num5 == 3 || num5 == 6 || num5 == 7) && objType != 1)
					{
						flag2 = false;
					}
					if (num5 == 5 && (objType == 1 || objType == 4))
					{
						flag2 = false;
					}
					if (!flag2)
					{
						continue;
					}
					Wiring.HitSwitch(i, j);
					NetMessage.SendData(59, -1, -1, null, i, j);
					if (num5 == 7)
					{
						WorldGen.KillTile(i, j);
						if (Main.netMode == 1)
						{
							NetMessage.SendData(17, -1, -1, null, 0, i, j);
						}
					}
					return true;
				}
			}
		}
		return false;
	}

	public bool SwitchTilesNew(Vector2 Position, int Width, int Height, Vector2 oldPosition, int objType)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		Point val = Position.ToTileCoordinates();
		Point point2 = (Position + new Vector2((float)Width, (float)Height)).ToTileCoordinates();
		int num = Utils.Clamp(val.X, 0, Main.maxTilesX - 1);
		int num2 = Utils.Clamp(val.Y, 0, Main.maxTilesY - 1);
		int num3 = Utils.Clamp(point2.X, 0, Main.maxTilesX - 1);
		int num4 = Utils.Clamp(point2.Y, 0, Main.maxTilesY - 1);
		for (int i = num; i <= num3; i++)
		{
			for (int j = num2; j <= num4; j++)
			{
				if (Main.tile[i, j] != null)
				{
					_ = ref Main.tile[i, j].type;
				}
			}
		}
		return false;
	}

	public static Vector2 StickyTiles(Vector2 Position, Vector2 Velocity, int Width, int Height)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_0169: Unknown result type (might be due to invalid IL or missing references)
		//IL_02be: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02da: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_020a: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_038e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0320: Unknown result type (might be due to invalid IL or missing references)
		//IL_032b: Unknown result type (might be due to invalid IL or missing references)
		//IL_035d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0375: Unknown result type (might be due to invalid IL or missing references)
		//IL_037b: Unknown result type (might be due to invalid IL or missing references)
		int num = (int)(Position.X / 16f) - 1;
		int num2 = (int)((Position.X + (float)Width) / 16f) + 2;
		int num3 = (int)(Position.Y / 16f) - 1;
		int num4 = (int)((Position.Y + (float)Height) / 16f) + 2;
		if (num < 0)
		{
			num = 0;
		}
		if (num2 > Main.maxTilesX)
		{
			num2 = Main.maxTilesX;
		}
		if (num3 < 0)
		{
			num3 = 0;
		}
		if (num4 > Main.maxTilesY)
		{
			num4 = Main.maxTilesY;
		}
		Vector2 vector2 = default(Vector2);
		for (int i = num; i < num2; i++)
		{
			for (int j = num3; j < num4; j++)
			{
				if (Main.tile[i, j] == null || !Main.tile[i, j].active() || Main.tile[i, j].inActive())
				{
					continue;
				}
				if (Main.tile[i, j].type == 51)
				{
					int num5 = 0;
					vector2.X = i * 16;
					vector2.Y = j * 16;
					if (Position.X + (float)Width > vector2.X - (float)num5 && Position.X < vector2.X + 16f + (float)num5 && Position.Y + (float)Height > vector2.Y && (double)Position.Y < (double)vector2.Y + 16.01)
					{
						if (Main.tile[i, j].type == 51 && (double)(Math.Abs(Velocity.X) + Math.Abs(Velocity.Y)) > 0.7 && Main.rand.Next(30) == 0)
						{
							Dust.NewDust(new Vector2((float)(i * 16), (float)(j * 16)), 16, 16, 30);
						}
						return new Vector2((float)i, (float)j);
					}
				}
				else
				{
					if (Main.tile[i, j].type != 229 || Main.tile[i, j].slope() != 0)
					{
						continue;
					}
					int num6 = 1;
					vector2.X = i * 16;
					vector2.Y = j * 16;
					float num7 = 16.01f;
					if (Main.tile[i, j].halfBrick())
					{
						vector2.Y += 8f;
						num7 -= 8f;
					}
					if (Position.X + (float)Width > vector2.X - (float)num6 && Position.X < vector2.X + 16f + (float)num6 && Position.Y + (float)Height > vector2.Y && Position.Y < vector2.Y + num7)
					{
						if (Main.tile[i, j].type == 51 && (double)(Math.Abs(Velocity.X) + Math.Abs(Velocity.Y)) > 0.7 && Main.rand.Next(30) == 0)
						{
							Dust.NewDust(new Vector2((float)(i * 16), (float)(j * 16)), 16, 16, 30);
						}
						return new Vector2((float)i, (float)j);
					}
				}
			}
		}
		return new Vector2(-1f, -1f);
	}

	public static bool SolidTilesVersatile(int startX, int endX, int startY, int endY)
	{
		if (startX > endX)
		{
			Utils.Swap(ref startX, ref endX);
		}
		if (startY > endY)
		{
			Utils.Swap(ref startY, ref endY);
		}
		return SolidTiles(startX, endX, startY, endY);
	}

	public static bool SolidTiles(Vector2 position, int width, int height)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		return SolidTiles((int)(position.X / 16f), (int)((position.X + (float)width) / 16f), (int)(position.Y / 16f), (int)((position.Y + (float)height) / 16f));
	}

	public static bool SolidTiles(int startX, int endX, int startY, int endY)
	{
		if (startX < 0)
		{
			return true;
		}
		if (endX >= Main.maxTilesX)
		{
			return true;
		}
		if (startY < 0)
		{
			return true;
		}
		if (endY >= Main.maxTilesY)
		{
			return true;
		}
		for (int i = startX; i < endX + 1; i++)
		{
			for (int j = startY; j < endY + 1; j++)
			{
				if (Main.tile[i, j] == null)
				{
					return false;
				}
				if (Main.tile[i, j].active() && !Main.tile[i, j].inActive() && Main.tileSolid[Main.tile[i, j].type] && !Main.tileSolidTop[Main.tile[i, j].type])
				{
					return true;
				}
			}
		}
		return false;
	}

	public static bool SolidTiles(Vector2 position, int width, int height, bool allowTopSurfaces)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		return SolidTiles((int)(position.X / 16f), (int)((position.X + (float)width) / 16f), (int)(position.Y / 16f), (int)((position.Y + (float)height) / 16f), allowTopSurfaces);
	}

	public static bool SolidTiles(int startX, int endX, int startY, int endY, bool allowTopSurfaces)
	{
		if (startX < 0)
		{
			return true;
		}
		if (endX >= Main.maxTilesX)
		{
			return true;
		}
		if (startY < 0)
		{
			return true;
		}
		if (endY >= Main.maxTilesY)
		{
			return true;
		}
		for (int i = startX; i < endX + 1; i++)
		{
			for (int j = startY; j < endY + 1; j++)
			{
				Tile tile = Main.tile[i, j];
				if (tile == null)
				{
					return false;
				}
				if (tile.active() && !Main.tile[i, j].inActive())
				{
					ushort type = tile.type;
					bool flag = Main.tileSolid[type] && !Main.tileSolidTop[type];
					if (allowTopSurfaces)
					{
						flag |= Main.tileSolidTop[type] && tile.frameY == 0;
					}
					if (flag)
					{
						return true;
					}
				}
			}
		}
		return false;
	}

	public static void StepDown(ref Vector2 position, ref Vector2 velocity, int width, int height, ref float stepSpeed, ref float gfxOffY, int gravDir = 1, bool waterWalk = false)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
		Vector2 vector = position;
		vector.X += velocity.X;
		vector.Y = (float)Math.Floor((vector.Y + (float)height) / 16f) * 16f - (float)height;
		bool flag = false;
		int num19 = (int)(vector.X / 16f);
		int num11 = (int)((vector.X + (float)width) / 16f);
		int num12 = (int)((vector.Y + (float)height + 4f) / 16f);
		int num13 = height / 16 + ((height % 16 != 0) ? 1 : 0);
		float num14 = (num12 + num13) * 16;
		float num15 = Main.bottomWorld / 16f - 42f;
		for (int i = num19; i <= num11; i++)
		{
			for (int j = num12; j <= num12 + 1; j++)
			{
				if (!WorldGen.InWorld(i, j, 1))
				{
					continue;
				}
				if (Main.tile[i, j] == null)
				{
					Main.tile[i, j] = default(Tile);
				}
				if (Main.tile[i, j - 1] == null)
				{
					Main.tile[i, j - 1] = default(Tile);
				}
				if (waterWalk && Main.tile[i, j].liquid > 0 && Main.tile[i, j - 1].liquid == 0)
				{
					int num16 = Main.tile[i, j].liquid / 32 * 2 + 2;
					int num17 = j * 16 + 16 - num16;
					Rectangle val = new Rectangle(i * 16, j * 16 - 17, 16, 16);
					if (((Rectangle)(ref val)).Intersects(new Rectangle((int)position.X, (int)position.Y, width, height)) && (float)num17 < num14)
					{
						num14 = num17;
					}
				}
				if ((float)j >= num15 || (Main.tile[i, j].nactive() && (Main.tileSolid[Main.tile[i, j].type] || Main.tileSolidTop[Main.tile[i, j].type])))
				{
					int num18 = j * 16;
					if (Main.tile[i, j].halfBrick())
					{
						num18 += 8;
					}
					if (Utils.FloatIntersect(i * 16, j * 16 - 17, 16f, 16f, position.X, position.Y, width, height) && (float)num18 < num14)
					{
						num14 = num18;
					}
				}
			}
		}
		float num10 = num14 - (position.Y + (float)height);
		if (num10 > 7f && num10 < 17f && !flag)
		{
			stepSpeed = 1.5f;
			if (num10 > 9f)
			{
				stepSpeed = 2.5f;
			}
			gfxOffY += position.Y + (float)height - num14;
			position.Y = num14 - (float)height;
		}
	}

	public static void StepUp(ref Vector2 position, ref Vector2 velocity, int width, int height, ref float stepSpeed, ref float gfxOffY, int gravDir = 1, bool holdsMatching = false, int specialChecksMode = 0)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_06da: Unknown result type (might be due to invalid IL or missing references)
		//IL_0635: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0641: Unknown result type (might be due to invalid IL or missing references)
		int num = 0;
		if (velocity.X < 0f)
		{
			num = -1;
		}
		if (velocity.X > 0f)
		{
			num = 1;
		}
		Vector2 vector = position;
		vector.X += velocity.X;
		int num2 = (int)((vector.X + (float)(width / 2) + (float)((width / 2 + 1) * num)) / 16f);
		int num3 = (int)(((double)vector.Y + 0.1) / 16.0);
		if (gravDir == 1)
		{
			num3 = (int)((vector.Y + (float)height - 1f) / 16f);
		}
		int num4 = height / 16 + ((height % 16 != 0) ? 1 : 0);
		bool flag = true;
		bool flag2 = true;
		if (Main.tile[num2, num3] == null)
		{
			return;
		}
		for (int i = 1; i < num4 + 2; i++)
		{
			if (!WorldGen.InWorld(num2, num3 - i * gravDir) || Main.tile[num2, num3 - i * gravDir] == null)
			{
				return;
			}
		}
		if (!WorldGen.InWorld(num2 - num, num3 - num4 * gravDir) || Main.tile[num2 - num, num3 - num4 * gravDir] == null)
		{
			return;
		}
		Tile tile;
		for (int j = 2; j < num4 + 1; j++)
		{
			if (!WorldGen.InWorld(num2, num3 - j * gravDir) || Main.tile[num2, num3 - j * gravDir] == null)
			{
				return;
			}
			tile = Main.tile[num2, num3 - j * gravDir];
			flag = flag && (!tile.nactive() || !Main.tileSolid[tile.type] || Main.tileSolidTop[tile.type]);
		}
		tile = Main.tile[num2 - num, num3 - num4 * gravDir];
		flag2 = flag2 && (!tile.nactive() || !Main.tileSolid[tile.type] || Main.tileSolidTop[tile.type]);
		bool flag3 = true;
		bool flag4 = true;
		bool flag5 = true;
		Tile tile2;
		if (gravDir == 1)
		{
			if (Main.tile[num2, num3 - gravDir] == null || Main.tile[num2, num3 - (num4 + 1) * gravDir] == null)
			{
				return;
			}
			tile = Main.tile[num2, num3 - gravDir];
			tile2 = Main.tile[num2, num3 - (num4 + 1) * gravDir];
			flag3 = flag3 && (!tile.nactive() || !Main.tileSolid[tile.type] || Main.tileSolidTop[tile.type] || (tile.slope() == 1 && position.X + (float)(width / 2) > (float)(num2 * 16)) || (tile.slope() == 2 && position.X + (float)(width / 2) < (float)(num2 * 16 + 16)) || (tile.halfBrick() && (!tile2.nactive() || !Main.tileSolid[tile2.type] || Main.tileSolidTop[tile2.type])));
			tile = Main.tile[num2, num3];
			tile2 = Main.tile[num2, num3 - 1];
			if (specialChecksMode == 1)
			{
				flag5 = !TileID.Sets.IgnoredByNpcStepUp[tile.type];
			}
			flag4 = flag4 && ((tile.nactive() && (!tile.topSlope() || (tile.slope() == 1 && position.X + (float)(width / 2) < (float)(num2 * 16)) || (tile.slope() == 2 && position.X + (float)(width / 2) > (float)(num2 * 16 + 16))) && (!tile.topSlope() || position.Y + (float)height > (float)(num3 * 16)) && ((Main.tileSolid[tile.type] && !Main.tileSolidTop[tile.type]) || (holdsMatching && ((Main.tileSolidTop[tile.type] && tile.frameY == 0) || TileID.Sets.Platforms[tile.type]) && (!Main.tileSolid[tile2.type] || !tile2.nactive()) && flag5))) || (tile2.halfBrick() && tile2.nactive()));
			flag4 &= !Main.tileSolidTop[tile.type] || !Main.tileSolidTop[tile2.type];
		}
		else
		{
			tile = Main.tile[num2, num3 - gravDir];
			tile2 = Main.tile[num2, num3 - (num4 + 1) * gravDir];
			flag3 = flag3 && (!tile.nactive() || !Main.tileSolid[tile.type] || Main.tileSolidTop[tile.type] || tile.slope() != 0 || (tile.halfBrick() && (!tile2.nactive() || !Main.tileSolid[tile2.type] || Main.tileSolidTop[tile2.type])));
			tile = Main.tile[num2, num3];
			tile2 = Main.tile[num2, num3 + 1];
			flag4 = flag4 && ((tile.nactive() && ((Main.tileSolid[tile.type] && !Main.tileSolidTop[tile.type]) || (holdsMatching && Main.tileSolidTop[tile.type] && tile.frameY == 0 && (!Main.tileSolid[tile2.type] || !tile2.nactive())))) || (tile2.halfBrick() && tile2.nactive()));
		}
		if (!((float)(num2 * 16) < vector.X + (float)width) || !((float)(num2 * 16 + 16) > vector.X))
		{
			return;
		}
		if (gravDir == 1)
		{
			if (!(flag4 && flag3 && flag && flag2))
			{
				return;
			}
			float num5 = num3 * 16;
			if (Main.tile[num2, num3 - 1].halfBrick())
			{
				num5 -= 8f;
			}
			else if (Main.tile[num2, num3].halfBrick())
			{
				num5 += 8f;
			}
			if (!(num5 < vector.Y + (float)height))
			{
				return;
			}
			float num6 = vector.Y + (float)height - num5;
			if ((double)num6 <= 16.1)
			{
				gfxOffY += position.Y + (float)height - num5;
				position.Y = num5 - (float)height;
				if (num6 < 9f)
				{
					stepSpeed = 1f;
				}
				else
				{
					stepSpeed = 2f;
				}
			}
		}
		else
		{
			if (!(flag4 && flag3 && flag && flag2) || Main.tile[num2, num3].bottomSlope() || TileID.Sets.Platforms[tile2.type])
			{
				return;
			}
			float num7 = num3 * 16 + 16;
			if (!(num7 > vector.Y))
			{
				return;
			}
			float num8 = num7 - vector.Y;
			if ((double)num8 <= 16.1)
			{
				gfxOffY -= num7 - position.Y;
				position.Y = num7;
				velocity.Y = 0f;
				if (num8 < 9f)
				{
					stepSpeed = 1f;
				}
				else
				{
					stepSpeed = 2f;
				}
			}
		}
	}

	public static bool InTileBounds(int x, int y, int lx, int ly, int hx, int hy)
	{
		if (x < lx || x > hx || y < ly || y > hy)
		{
			return false;
		}
		return true;
	}

	public static float GetTileRotation(Vector2 position)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		float num = position.Y % 16f;
		int num2 = (int)(position.X / 16f);
		int num3 = (int)(position.Y / 16f);
		Tile tile = Main.tile[num2, num3];
		bool flag = false;
		for (int num4 = 2; num4 >= 0; num4--)
		{
			if (tile.active())
			{
				if (Main.tileSolid[tile.type])
				{
					int num5 = tile.blockType();
					if (!TileID.Sets.Platforms[tile.type])
					{
						return num5 switch
						{
							1 => 0f, 
							2 => (float)Math.PI / 4f, 
							3 => -(float)Math.PI / 4f, 
							_ => 0f, 
						};
					}
					int num6 = tile.frameX / 18;
					if (((num6 >= 0 && num6 <= 7) || (num6 >= 12 && num6 <= 16)) && (num == 0f || flag))
					{
						return 0f;
					}
					switch (num6)
					{
					case 8:
					case 19:
					case 21:
					case 23:
						return -(float)Math.PI / 4f;
					case 10:
					case 20:
					case 22:
					case 24:
						return (float)Math.PI / 4f;
					case 25:
					case 26:
						if (!flag)
						{
							switch (num5)
							{
							case 2:
								return (float)Math.PI / 4f;
							case 3:
								return -(float)Math.PI / 4f;
							}
							break;
						}
						return 0f;
					}
				}
				else if (Main.tileSolidTop[tile.type] && tile.frameY == 0 && flag)
				{
					return 0f;
				}
			}
			num3++;
			tile = Main.tile[num2, num3];
			flag = true;
		}
		return 0f;
	}

	public static void GetEntityEdgeTiles(List<Point> p, Entity entity, bool left = true, bool right = true, bool up = true, bool down = true)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		int num = (int)entity.position.X;
		int num2 = (int)entity.position.Y;
		_ = num % 16;
		_ = num2 % 16;
		int num3 = (int)entity.Right.X;
		int num4 = (int)entity.Bottom.Y;
		if (num % 16 == 0)
		{
			num--;
		}
		if (num2 % 16 == 0)
		{
			num2--;
		}
		if (num3 % 16 == 0)
		{
			num3++;
		}
		if (num4 % 16 == 0)
		{
			num4++;
		}
		int num5 = num3 / 16 - num / 16;
		int num6 = num4 / 16 - num2 / 16;
		num /= 16;
		num2 /= 16;
		for (int i = num; i <= num + num5; i++)
		{
			if (up)
			{
				p.Add(new Point(i, num2));
			}
			if (down)
			{
				p.Add(new Point(i, num2 + num6));
			}
		}
		for (int j = num2; j < num2 + num6; j++)
		{
			if (left)
			{
				p.Add(new Point(num, j));
			}
			if (right)
			{
				p.Add(new Point(num + num5, j));
			}
		}
	}

	public static void StepConveyorBelt(Entity entity, float gravDir)
	{
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_046d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0472: Unknown result type (might be due to invalid IL or missing references)
		//IL_047c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0481: Unknown result type (might be due to invalid IL or missing references)
		//IL_0484: Unknown result type (might be due to invalid IL or missing references)
		//IL_0489: Unknown result type (might be due to invalid IL or missing references)
		//IL_049b: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_022c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0242: Unknown result type (might be due to invalid IL or missing references)
		//IL_0274: Unknown result type (might be due to invalid IL or missing references)
		//IL_029a: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02de: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0371: Unknown result type (might be due to invalid IL or missing references)
		//IL_0376: Unknown result type (might be due to invalid IL or missing references)
		//IL_0378: Unknown result type (might be due to invalid IL or missing references)
		//IL_037e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0383: Unknown result type (might be due to invalid IL or missing references)
		//IL_038a: Unknown result type (might be due to invalid IL or missing references)
		//IL_038f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0394: Unknown result type (might be due to invalid IL or missing references)
		//IL_0396: Unknown result type (might be due to invalid IL or missing references)
		//IL_0322: Unknown result type (might be due to invalid IL or missing references)
		//IL_0300: Unknown result type (might be due to invalid IL or missing references)
		//IL_0340: Unknown result type (might be due to invalid IL or missing references)
		Player player = null;
		if (entity is Player)
		{
			player = (Player)entity;
			if (Math.Abs(player.gfxOffY) > 2f || player.grapCount > 0 || player.pulley)
			{
				return;
			}
			entity.height -= 5;
			entity.position.Y += 5f;
		}
		int num = 0;
		int num2 = 0;
		bool flag = false;
		int num3 = (int)entity.position.Y + entity.height;
		Rectangle hitbox = entity.Hitbox;
		((Rectangle)(ref hitbox)).Inflate(2, 2);
		_ = entity.TopLeft;
		_ = entity.TopRight;
		_ = entity.BottomLeft;
		_ = entity.BottomRight;
		List<Point> cacheForConveyorBelts = _cacheForConveyorBelts;
		cacheForConveyorBelts.Clear();
		GetEntityEdgeTiles(cacheForConveyorBelts, entity, left: false, right: false);
		Vector2 vector = default(Vector2);
		((Vector2)(ref vector))._002Ector(0.0001f);
		Vector2 lineStart = default(Vector2);
		Vector2 lineStart2 = default(Vector2);
		Vector2 lineEnd = default(Vector2);
		Vector2 lineEnd2 = default(Vector2);
		for (int i = 0; i < cacheForConveyorBelts.Count; i++)
		{
			Point point = cacheForConveyorBelts[i];
			if (!WorldGen.InWorld(point.X, point.Y) || (player != null && player.onTrack && point.Y < num3))
			{
				continue;
			}
			Tile tile = Main.tile[point.X, point.Y];
			if (tile == null || !tile.active() || !tile.nactive())
			{
				continue;
			}
			int num4 = TileID.Sets.ConveyorDirection[tile.type];
			if (num4 == 0)
			{
				continue;
			}
			lineStart.X = (lineStart2.X = point.X * 16);
			lineEnd.X = (lineEnd2.X = point.X * 16 + 16);
			switch (tile.slope())
			{
			case 1:
				lineStart2.Y = point.Y * 16;
				lineEnd2.Y = (lineEnd.Y = (lineStart.Y = point.Y * 16 + 16));
				break;
			case 2:
				lineEnd2.Y = point.Y * 16;
				lineStart2.Y = (lineEnd.Y = (lineStart.Y = point.Y * 16 + 16));
				break;
			case 3:
				lineEnd.Y = (lineStart2.Y = (lineEnd2.Y = point.Y * 16));
				lineStart.Y = point.Y * 16 + 16;
				break;
			case 4:
				lineStart.Y = (lineStart2.Y = (lineEnd2.Y = point.Y * 16));
				lineEnd.Y = point.Y * 16 + 16;
				break;
			default:
				if (tile.halfBrick())
				{
					lineStart2.Y = (lineEnd2.Y = point.Y * 16 + 8);
				}
				else
				{
					lineStart2.Y = (lineEnd2.Y = point.Y * 16);
				}
				lineStart.Y = (lineEnd.Y = point.Y * 16 + 16);
				break;
			}
			int num5 = 0;
			if (!TileID.Sets.Platforms[tile.type] && CheckAABBvLineCollision2(entity.position - vector, entity.Size + vector * 2f, lineStart, lineEnd))
			{
				num5--;
			}
			if (CheckAABBvLineCollision2(entity.position - vector, entity.Size + vector * 2f, lineStart2, lineEnd2))
			{
				num5++;
			}
			if (num5 != 0)
			{
				flag = true;
				num += num4 * num5 * (int)gravDir;
				if (tile.leftSlope())
				{
					num2 += (int)gravDir * -num4;
				}
				if (tile.rightSlope())
				{
					num2 -= (int)gravDir * -num4;
				}
			}
		}
		if (entity is Player)
		{
			entity.height += 5;
			entity.position.Y -= 5f;
		}
		if (flag && num != 0)
		{
			num = Math.Sign(num);
			num2 = Math.Sign(num2);
			Vector2 velocity = Vector2.Normalize(new Vector2((float)num * gravDir, (float)num2)) * 2.5f;
			Vector2 vector2 = TileCollision(entity.position, velocity, entity.width, entity.height, fallThrough: false, fall2: false, (int)gravDir);
			entity.position += vector2;
			Vector2 velocity2 = default(Vector2);
			((Vector2)(ref velocity2))._002Ector(0f, 2.5f * gravDir);
			vector2 = TileCollision(entity.position, velocity2, entity.width, entity.height, fallThrough: false, fall2: false, (int)gravDir);
			entity.position += vector2;
		}
	}

	public static List<Point> GetTilesIn(Vector2 TopLeft, Vector2 BottomRight)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		List<Point> list = new List<Point>();
		Point val = TopLeft.ToTileCoordinates();
		Point point2 = BottomRight.ToTileCoordinates();
		int num = Utils.Clamp(val.X, 0, Main.maxTilesX - 1);
		int num2 = Utils.Clamp(val.Y, 0, Main.maxTilesY - 1);
		int num3 = Utils.Clamp(point2.X, 0, Main.maxTilesX - 1);
		int num4 = Utils.Clamp(point2.Y, 0, Main.maxTilesY - 1);
		for (int i = num; i <= num3; i++)
		{
			for (int j = num2; j <= num4; j++)
			{
				if (Main.tile[i, j] != null)
				{
					list.Add(new Point(i, j));
				}
			}
		}
		return list;
	}

	public static void ExpandVertically(int startX, int startY, out int topY, out int bottomY, int maxExpandUp = 100, int maxExpandDown = 100)
	{
		topY = startY;
		bottomY = startY;
		if (!WorldGen.InWorld(startX, startY, 10))
		{
			return;
		}
		for (int i = 0; i < maxExpandUp; i++)
		{
			if (topY <= 0)
			{
				break;
			}
			if (topY < 10)
			{
				break;
			}
			if (Main.tile[startX, topY] == null)
			{
				break;
			}
			if (WorldGen.SolidTile3(startX, topY))
			{
				break;
			}
			topY--;
		}
		for (int j = 0; j < maxExpandDown; j++)
		{
			if (bottomY >= Main.maxTilesY - 10)
			{
				break;
			}
			if (bottomY > Main.maxTilesY - 10)
			{
				break;
			}
			if (Main.tile[startX, bottomY] == null)
			{
				break;
			}
			if (WorldGen.SolidTile3(startX, bottomY))
			{
				break;
			}
			bottomY++;
		}
	}

	public static Vector2 AdvancedTileCollision(bool[] forcedIgnoredTiles, Vector2 Position, Vector2 Velocity, int Width, int Height, bool fallThrough = false, bool fall2 = false, int gravDir = 1)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0602: Unknown result type (might be due to invalid IL or missing references)
		//IL_0182: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0197: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01af: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_020a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0311: Unknown result type (might be due to invalid IL or missing references)
		//IL_031b: Unknown result type (might be due to invalid IL or missing references)
		//IL_028d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0297: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0232: Unknown result type (might be due to invalid IL or missing references)
		//IL_0238: Unknown result type (might be due to invalid IL or missing references)
		//IL_0244: Unknown result type (might be due to invalid IL or missing references)
		//IL_0213: Unknown result type (might be due to invalid IL or missing references)
		//IL_0219: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0250: Unknown result type (might be due to invalid IL or missing references)
		//IL_0259: Unknown result type (might be due to invalid IL or missing references)
		//IL_049c: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_035a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0340: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0575: Unknown result type (might be due to invalid IL or missing references)
		//IL_057b: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_038c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0393: Unknown result type (might be due to invalid IL or missing references)
		//IL_046b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0472: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0547: Unknown result type (might be due to invalid IL or missing references)
		//IL_0554: Unknown result type (might be due to invalid IL or missing references)
		//IL_048c: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0568: Unknown result type (might be due to invalid IL or missing references)
		up = false;
		down = false;
		Vector2 result = Velocity;
		Vector2 vector2 = Position + Velocity;
		int value5 = (int)(Position.X / 16f) - 1;
		int value2 = (int)((Position.X + (float)Width) / 16f) + 2;
		int value3 = (int)(Position.Y / 16f) - 1;
		int value4 = (int)((Position.Y + (float)Height) / 16f) + 2;
		int num = -1;
		int num2 = -1;
		int num3 = -1;
		int num4 = -1;
		int num7 = Utils.Clamp(value5, 0, Main.maxTilesX - 1);
		value2 = Utils.Clamp(value2, 0, Main.maxTilesX - 1);
		value3 = Utils.Clamp(value3, 0, Main.maxTilesY - 1);
		value4 = Utils.Clamp(value4, 0, Main.maxTilesY - 1);
		float num5 = (value4 + 3) * 16;
		Vector2 vector3 = default(Vector2);
		for (int i = num7; i < value2; i++)
		{
			for (int j = value3; j < value4; j++)
			{
				Tile tile = Main.tile[i, j];
				if (tile == null || !tile.active() || tile.inActive() || forcedIgnoredTiles[tile.type] || (!Main.tileSolid[tile.type] && (!Main.tileSolidTop[tile.type] || tile.frameY != 0)))
				{
					continue;
				}
				vector3.X = i * 16;
				vector3.Y = j * 16;
				int num6 = 16;
				if (tile.halfBrick())
				{
					vector3.Y += 8f;
					num6 -= 8;
				}
				if (!(vector2.X + (float)Width > vector3.X) || !(vector2.X < vector3.X + 16f) || !(vector2.Y + (float)Height > vector3.Y) || !(vector2.Y < vector3.Y + (float)num6))
				{
					continue;
				}
				bool flag = false;
				bool flag2 = false;
				if (tile.slope() > 2)
				{
					if (tile.slope() == 3 && Position.Y + Math.Abs(Velocity.X) >= vector3.Y && Position.X >= vector3.X)
					{
						flag2 = true;
					}
					if (tile.slope() == 4 && Position.Y + Math.Abs(Velocity.X) >= vector3.Y && Position.X + (float)Width <= vector3.X + 16f)
					{
						flag2 = true;
					}
				}
				else if (tile.slope() > 0)
				{
					flag = true;
					if (tile.slope() == 1 && Position.Y + (float)Height - Math.Abs(Velocity.X) <= vector3.Y + (float)num6 && Position.X >= vector3.X)
					{
						flag2 = true;
					}
					if (tile.slope() == 2 && Position.Y + (float)Height - Math.Abs(Velocity.X) <= vector3.Y + (float)num6 && Position.X + (float)Width <= vector3.X + 16f)
					{
						flag2 = true;
					}
				}
				if (flag2)
				{
					continue;
				}
				if (Position.Y + (float)Height <= vector3.Y)
				{
					down = true;
					if ((!(Main.tileSolidTop[tile.type] && fallThrough) || !(Velocity.Y <= 1f || fall2)) && num5 > vector3.Y)
					{
						num3 = i;
						num4 = j;
						if (num6 < 16)
						{
							num4++;
						}
						if (num3 != num && !flag)
						{
							result.Y = vector3.Y - (Position.Y + (float)Height) + ((gravDir == -1) ? (-0.01f) : 0f);
							num5 = vector3.Y;
						}
					}
				}
				else if (Position.X + (float)Width <= vector3.X && !Main.tileSolidTop[tile.type])
				{
					if (Main.tile[i - 1, j] == null)
					{
						Main.tile[i - 1, j] = default(Tile);
					}
					if (Main.tile[i - 1, j].slope() != 2 && Main.tile[i - 1, j].slope() != 4)
					{
						num = i;
						num2 = j;
						if (num2 != num4)
						{
							result.X = vector3.X - (Position.X + (float)Width);
						}
						if (num3 == num)
						{
							result.Y = Velocity.Y;
						}
					}
				}
				else if (Position.X >= vector3.X + 16f && !Main.tileSolidTop[tile.type])
				{
					if (Main.tile[i + 1, j] == null)
					{
						Main.tile[i + 1, j] = default(Tile);
					}
					if (Main.tile[i + 1, j].slope() != 1 && Main.tile[i + 1, j].slope() != 3)
					{
						num = i;
						num2 = j;
						if (num2 != num4)
						{
							result.X = vector3.X + 16f - Position.X;
						}
						if (num3 == num)
						{
							result.Y = Velocity.Y;
						}
					}
				}
				else if (Position.Y >= vector3.Y + (float)num6 && !Main.tileSolidTop[tile.type])
				{
					up = true;
					num3 = i;
					num4 = j;
					result.Y = vector3.Y + (float)num6 - Position.Y + ((gravDir == 1) ? 0.01f : 0f);
					if (num4 == num2)
					{
						result.X = Velocity.X;
					}
				}
			}
		}
		return result;
	}

	public static void LaserScan(Vector2 samplingPoint, Vector2 directionUnit, float samplingWidth, float maxDistance, float[] samples)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < samples.Length; i++)
		{
			float num = (float)i / (float)(samples.Length - 1);
			Vector2 center = default(Vector2);
			Vector2 val = samplingPoint + directionUnit.RotatedBy(1.5707963705062866, center) * (num - 0.5f) * samplingWidth;
			int num2 = (int)val.X / 16;
			int num3 = (int)val.Y / 16;
			Vector2 val2 = val + directionUnit * maxDistance;
			int num4 = (int)val2.X / 16;
			int num5 = (int)val2.Y / 16;
			float num6 = 0f;
			float num7;
			if (!TupleHitLine(num2, num3, num4, num5, 0, 0, new List<Tuple<int, int>>(), out var col))
			{
				center = new Vector2((float)Math.Abs(num2 - col.Item1), (float)Math.Abs(num3 - col.Item2));
				num7 = ((Vector2)(ref center)).Length() * 16f;
			}
			else if (col.Item1 == num4 && col.Item2 == num5)
			{
				num7 = maxDistance;
			}
			else
			{
				center = new Vector2((float)Math.Abs(num2 - col.Item1), (float)Math.Abs(num3 - col.Item2));
				num7 = ((Vector2)(ref center)).Length() * 16f;
			}
			num6 = num7;
			samples[i] = num6;
		}
	}

	public static void AimingLaserScan(Vector2 startPoint, Vector2 endPoint, float samplingWidth, int samplesToTake, out Vector2 vectorTowardsTarget, out float[] samples)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		samples = new float[samplesToTake];
		vectorTowardsTarget = endPoint - startPoint;
		LaserScan(startPoint, vectorTowardsTarget.SafeNormalize(Vector2.Zero), samplingWidth, ((Vector2)(ref vectorTowardsTarget)).Length(), samples);
	}
}
