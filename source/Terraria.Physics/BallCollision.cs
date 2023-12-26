using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace Terraria.Physics;

public static class BallCollision
{
	[Flags]
	private enum TileEdges : uint
	{
		None = 0u,
		Top = 1u,
		Bottom = 2u,
		Left = 4u,
		Right = 8u,
		TopLeftSlope = 0x10u,
		TopRightSlope = 0x20u,
		BottomLeftSlope = 0x40u,
		BottomRightSlope = 0x80u
	}

	public static BallStepResult Step(PhysicsProperties physicsProperties, Entity entity, ref float entityAngularVelocity, IBallContactListener listener)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_020a: Unknown result type (might be due to invalid IL or missing references)
		//IL_020d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0212: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0266: Unknown result type (might be due to invalid IL or missing references)
		//IL_0267: Unknown result type (might be due to invalid IL or missing references)
		//IL_026d: Unknown result type (might be due to invalid IL or missing references)
		//IL_026e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0223: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0230: Unknown result type (might be due to invalid IL or missing references)
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_016d: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0174: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_017b: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_018c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0191: Unknown result type (might be due to invalid IL or missing references)
		//IL_0197: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_023d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_024a: Unknown result type (might be due to invalid IL or missing references)
		Vector2 position = entity.position;
		Vector2 velocity = entity.velocity;
		Vector2 size = entity.Size;
		float num = entityAngularVelocity;
		float num2 = size.X * 0.5f;
		num *= physicsProperties.Drag;
		velocity *= physicsProperties.Drag;
		float num3 = ((Vector2)(ref velocity)).Length();
		if (num3 > 1000f)
		{
			velocity = 1000f * Vector2.Normalize(velocity);
			num3 = 1000f;
		}
		int num4 = Math.Max(1, (int)Math.Ceiling(num3 / 2f));
		float num5 = 1f / (float)num4;
		velocity *= num5;
		num *= num5;
		float num6 = physicsProperties.Gravity / (float)(num4 * num4);
		bool flag = false;
		for (int i = 0; i < num4; i++)
		{
			velocity.Y += num6;
			if (CheckForPassThrough(position + size * 0.5f, out var type, out var contactTile))
			{
				if (type == BallPassThroughType.Tile && Main.tileSolid[contactTile.type] && !Main.tileSolidTop[contactTile.type])
				{
					velocity *= 0f;
					num *= 0f;
					flag = true;
				}
				else
				{
					BallPassThroughEvent passThrough = new BallPassThroughEvent(num5, contactTile, entity, type);
					listener.OnPassThrough(physicsProperties, ref position, ref velocity, ref num, ref passThrough);
				}
			}
			position += velocity;
			if (!IsBallInWorld(position, size))
			{
				return BallStepResult.OutOfBounds();
			}
			if (GetClosestEdgeToCircle(position, size, velocity, out var collisionPoint, out contactTile))
			{
				Vector2 vector = Vector2.Normalize(position + size * 0.5f - collisionPoint);
				position = collisionPoint + vector * (num2 + 0.0001f) - size * 0.5f;
				BallCollisionEvent collision = new BallCollisionEvent(num5, vector, collisionPoint, contactTile, entity);
				flag = true;
				velocity = Vector2.Reflect(velocity, collision.Normal);
				listener.OnCollision(physicsProperties, ref position, ref velocity, ref collision);
				num = (collision.Normal.X * velocity.Y - collision.Normal.Y * velocity.X) / num2;
			}
		}
		velocity /= num5;
		num /= num5;
		BallStepResult result = BallStepResult.Moving();
		if (flag && velocity.X > -0.01f && velocity.X < 0.01f && velocity.Y <= 0f && velocity.Y > 0f - physicsProperties.Gravity)
		{
			result = BallStepResult.Resting();
		}
		entity.position = position;
		entity.velocity = velocity;
		entityAngularVelocity = num;
		return result;
	}

	private static bool CheckForPassThrough(Vector2 center, out BallPassThroughType type, out Tile contactTile)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		Point tileCoordinates = center.ToTileCoordinates();
		Tile tile = (contactTile = Main.tile[tileCoordinates.X, tileCoordinates.Y]);
		type = BallPassThroughType.None;
		if (tile == null)
		{
			return false;
		}
		if (tile.nactive())
		{
			type = BallPassThroughType.Tile;
			return IsPositionInsideTile(center, tileCoordinates, tile);
		}
		if (tile.liquid > 0)
		{
			float num = (float)(tileCoordinates.Y + 1) * 16f - (float)(int)tile.liquid / 255f * 16f;
			switch (tile.liquidType())
			{
			case 1:
				type = BallPassThroughType.Lava;
				break;
			case 2:
				type = BallPassThroughType.Honey;
				break;
			default:
				type = BallPassThroughType.Water;
				break;
			}
			return num < center.Y;
		}
		return false;
	}

	private static bool IsPositionInsideTile(Vector2 position, Point tileCoordinates, Tile tile)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		if (tile.slope() == 0 && !tile.halfBrick())
		{
			return true;
		}
		Vector2 vector = position / 16f - new Vector2((float)tileCoordinates.X, (float)tileCoordinates.Y);
		return tile.slope() switch
		{
			0 => vector.Y > 0.5f, 
			1 => vector.Y > vector.X, 
			2 => vector.Y > 1f - vector.X, 
			3 => vector.Y < 1f - vector.X, 
			4 => vector.Y < vector.X, 
			_ => false, 
		};
	}

	private static bool IsBallInWorld(Vector2 position, Vector2 size)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		if (position.X > 32f && position.Y > 32f && position.X + size.X < (float)Main.maxTilesX * 16f - 32f)
		{
			return position.Y + size.Y < (float)Main.maxTilesY * 16f - 32f;
		}
		return false;
	}

	private static bool GetClosestEdgeToCircle(Vector2 position, Vector2 size, Vector2 velocity, out Vector2 collisionPoint, out Tile collisionTile)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		Rectangle tileBounds = GetTileBounds(position, size);
		Vector2 vector = position + size * 0.5f;
		TileEdges tileEdges = TileEdges.None;
		tileEdges = ((!(velocity.Y < 0f)) ? (tileEdges | TileEdges.Top) : (tileEdges | TileEdges.Bottom));
		tileEdges = ((!(velocity.X < 0f)) ? (tileEdges | TileEdges.Left) : (tileEdges | TileEdges.Right));
		tileEdges = ((!(velocity.Y > velocity.X)) ? (tileEdges | TileEdges.TopRightSlope) : (tileEdges | TileEdges.BottomLeftSlope));
		tileEdges = ((!(velocity.Y > 0f - velocity.X)) ? (tileEdges | TileEdges.TopLeftSlope) : (tileEdges | TileEdges.BottomRightSlope));
		collisionPoint = Vector2.Zero;
		collisionTile = default(Tile);
		float num = float.MaxValue;
		Vector2 closestPointOut = default(Vector2);
		float distanceSquaredOut = 0f;
		for (int i = ((Rectangle)(ref tileBounds)).Left; i < ((Rectangle)(ref tileBounds)).Right; i++)
		{
			for (int j = ((Rectangle)(ref tileBounds)).Top; j < ((Rectangle)(ref tileBounds)).Bottom; j++)
			{
				if (GetCollisionPointForTile(tileEdges, i, j, vector, ref closestPointOut, ref distanceSquaredOut) && !(distanceSquaredOut >= num) && !(Vector2.Dot(velocity, vector - closestPointOut) > 0f))
				{
					num = distanceSquaredOut;
					collisionPoint = closestPointOut;
					collisionTile = Main.tile[i, j];
				}
			}
		}
		float num2 = size.X / 2f;
		return num < num2 * num2;
	}

	private static bool GetCollisionPointForTile(TileEdges edgesToTest, int x, int y, Vector2 center, ref Vector2 closestPointOut, ref float distanceSquaredOut)
	{
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		Tile tile = Main.tile[x, y];
		if (tile == null || !tile.nactive() || (!Main.tileSolid[tile.type] && !Main.tileSolidTop[tile.type]))
		{
			return false;
		}
		if (!Main.tileSolid[tile.type] && Main.tileSolidTop[tile.type] && tile.frameY != 0)
		{
			return false;
		}
		if (Main.tileSolidTop[tile.type])
		{
			edgesToTest &= TileEdges.Top | TileEdges.BottomLeftSlope | TileEdges.BottomRightSlope;
		}
		Vector2 tilePosition = default(Vector2);
		((Vector2)(ref tilePosition))._002Ector((float)x * 16f, (float)y * 16f);
		bool flag = false;
		LineSegment edge = default(LineSegment);
		if (GetSlopeEdge(ref edgesToTest, tile, tilePosition, ref edge))
		{
			closestPointOut = ClosestPointOnLineSegment(center, edge);
			distanceSquaredOut = Vector2.DistanceSquared(closestPointOut, center);
			flag = true;
		}
		if (GetTopOrBottomEdge(edgesToTest, x, y, tilePosition, ref edge))
		{
			Vector2 vector = ClosestPointOnLineSegment(center, edge);
			float num = Vector2.DistanceSquared(vector, center);
			if (!flag || num < distanceSquaredOut)
			{
				distanceSquaredOut = num;
				closestPointOut = vector;
			}
			flag = true;
		}
		if (GetLeftOrRightEdge(edgesToTest, x, y, tilePosition, ref edge))
		{
			Vector2 vector2 = ClosestPointOnLineSegment(center, edge);
			float num2 = Vector2.DistanceSquared(vector2, center);
			if (!flag || num2 < distanceSquaredOut)
			{
				distanceSquaredOut = num2;
				closestPointOut = vector2;
			}
			flag = true;
		}
		return flag;
	}

	private static bool GetSlopeEdge(ref TileEdges edgesToTest, Tile tile, Vector2 tilePosition, ref LineSegment edge)
	{
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		switch (tile.slope())
		{
		case 0:
			return false;
		case 1:
			edgesToTest &= TileEdges.Bottom | TileEdges.Left | TileEdges.BottomLeftSlope;
			if ((edgesToTest & TileEdges.BottomLeftSlope) == 0)
			{
				return false;
			}
			edge.Start = tilePosition;
			edge.End = new Vector2(tilePosition.X + 16f, tilePosition.Y + 16f);
			return true;
		case 2:
			edgesToTest &= TileEdges.Bottom | TileEdges.Right | TileEdges.BottomRightSlope;
			if ((edgesToTest & TileEdges.BottomRightSlope) == 0)
			{
				return false;
			}
			edge.Start = new Vector2(tilePosition.X, tilePosition.Y + 16f);
			edge.End = new Vector2(tilePosition.X + 16f, tilePosition.Y);
			return true;
		case 3:
			edgesToTest &= TileEdges.Top | TileEdges.Left | TileEdges.TopLeftSlope;
			if ((edgesToTest & TileEdges.TopLeftSlope) == 0)
			{
				return false;
			}
			edge.Start = new Vector2(tilePosition.X, tilePosition.Y + 16f);
			edge.End = new Vector2(tilePosition.X + 16f, tilePosition.Y);
			return true;
		case 4:
			edgesToTest &= TileEdges.Top | TileEdges.Right | TileEdges.TopRightSlope;
			if ((edgesToTest & TileEdges.TopRightSlope) == 0)
			{
				return false;
			}
			edge.Start = tilePosition;
			edge.End = new Vector2(tilePosition.X + 16f, tilePosition.Y + 16f);
			return true;
		default:
			return false;
		}
	}

	private static bool GetTopOrBottomEdge(TileEdges edgesToTest, int x, int y, Vector2 tilePosition, ref LineSegment edge)
	{
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Unknown result type (might be due to invalid IL or missing references)
		if ((edgesToTest & TileEdges.Bottom) != 0)
		{
			Tile tile = Main.tile[x, y + 1];
			if (IsNeighborSolid(tile) && tile.slope() != 1 && tile.slope() != 2 && !tile.halfBrick())
			{
				return false;
			}
			edge.Start = new Vector2(tilePosition.X, tilePosition.Y + 16f);
			edge.End = new Vector2(tilePosition.X + 16f, tilePosition.Y + 16f);
			return true;
		}
		if ((edgesToTest & TileEdges.Top) != 0)
		{
			Tile tile2 = Main.tile[x, y - 1];
			if (!Main.tile[x, y].halfBrick() && IsNeighborSolid(tile2) && tile2.slope() != 3 && tile2.slope() != 4)
			{
				return false;
			}
			if (Main.tile[x, y].halfBrick())
			{
				tilePosition.Y += 8f;
			}
			edge.Start = new Vector2(tilePosition.X, tilePosition.Y);
			edge.End = new Vector2(tilePosition.X + 16f, tilePosition.Y);
			return true;
		}
		return false;
	}

	private static bool GetLeftOrRightEdge(TileEdges edgesToTest, int x, int y, Vector2 tilePosition, ref LineSegment edge)
	{
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		if ((edgesToTest & TileEdges.Left) != 0)
		{
			Tile tile = Main.tile[x, y];
			Tile tile2 = Main.tile[x - 1, y];
			if (IsNeighborSolid(tile2) && tile2.slope() != 1 && tile2.slope() != 3 && (!tile2.halfBrick() || tile.halfBrick()))
			{
				return false;
			}
			edge.Start = new Vector2(tilePosition.X, tilePosition.Y);
			edge.End = new Vector2(tilePosition.X, tilePosition.Y + 16f);
			if (tile.halfBrick())
			{
				edge.Start.Y += 8f;
			}
			return true;
		}
		if ((edgesToTest & TileEdges.Right) != 0)
		{
			Tile tile3 = Main.tile[x, y];
			Tile tile4 = Main.tile[x + 1, y];
			if (IsNeighborSolid(tile4) && tile4.slope() != 2 && tile4.slope() != 4 && (!tile4.halfBrick() || tile3.halfBrick()))
			{
				return false;
			}
			edge.Start = new Vector2(tilePosition.X + 16f, tilePosition.Y);
			edge.End = new Vector2(tilePosition.X + 16f, tilePosition.Y + 16f);
			if (tile3.halfBrick())
			{
				edge.Start.Y += 8f;
			}
			return true;
		}
		return false;
	}

	private static Rectangle GetTileBounds(Vector2 position, Vector2 size)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		int num = (int)Math.Floor(position.X / 16f);
		int num2 = (int)Math.Floor(position.Y / 16f);
		int num3 = (int)Math.Floor((position.X + size.X) / 16f);
		int num4 = (int)Math.Floor((position.Y + size.Y) / 16f);
		return new Rectangle(num, num2, num3 - num + 1, num4 - num2 + 1);
	}

	private static bool IsNeighborSolid(Tile tile)
	{
		if (tile != null && tile.nactive() && Main.tileSolid[tile.type])
		{
			return !Main.tileSolidTop[tile.type];
		}
		return false;
	}

	private static Vector2 ClosestPointOnLineSegment(Vector2 point, LineSegment lineSegment)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		Vector2 val = point - lineSegment.Start;
		Vector2 vector = lineSegment.End - lineSegment.Start;
		float num = ((Vector2)(ref vector)).LengthSquared();
		float num2 = Vector2.Dot(val, vector) / num;
		if (num2 < 0f)
		{
			return lineSegment.Start;
		}
		if (num2 > 1f)
		{
			return lineSegment.End;
		}
		return lineSegment.Start + vector * num2;
	}

	[Conditional("DEBUG")]
	private static void DrawEdge(LineSegment edge)
	{
	}
}
