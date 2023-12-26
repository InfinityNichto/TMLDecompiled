using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.ID;

namespace Terraria.GameContent;

public class PortalHelper
{
	public const int PORTALS_PER_PERSON = 2;

	private static int[,] FoundPortals;

	private static int[] PortalCooldownForPlayers;

	private static int[] PortalCooldownForNPCs;

	private static readonly Vector2[] EDGES;

	private static readonly Vector2[] SLOPE_EDGES;

	private static readonly Point[] SLOPE_OFFSETS;

	private static bool anyPortalAtAll;

	static PortalHelper()
	{
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		FoundPortals = new int[256, 2];
		PortalCooldownForPlayers = new int[256];
		PortalCooldownForNPCs = new int[200];
		EDGES = (Vector2[])(object)new Vector2[4]
		{
			new Vector2(0f, 1f),
			new Vector2(0f, -1f),
			new Vector2(1f, 0f),
			new Vector2(-1f, 0f)
		};
		SLOPE_EDGES = (Vector2[])(object)new Vector2[4]
		{
			new Vector2(1f, -1f),
			new Vector2(-1f, -1f),
			new Vector2(1f, 1f),
			new Vector2(-1f, 1f)
		};
		SLOPE_OFFSETS = (Point[])(object)new Point[4]
		{
			new Point(1, -1),
			new Point(-1, -1),
			new Point(1, 1),
			new Point(-1, 1)
		};
		anyPortalAtAll = false;
		for (int i = 0; i < SLOPE_EDGES.Length; i++)
		{
			((Vector2)(ref SLOPE_EDGES[i])).Normalize();
		}
		for (int j = 0; j < FoundPortals.GetLength(0); j++)
		{
			FoundPortals[j, 0] = -1;
			FoundPortals[j, 1] = -1;
		}
	}

	public static void UpdatePortalPoints()
	{
		anyPortalAtAll = false;
		for (int i = 0; i < FoundPortals.GetLength(0); i++)
		{
			FoundPortals[i, 0] = -1;
			FoundPortals[i, 1] = -1;
		}
		for (int j = 0; j < PortalCooldownForPlayers.Length; j++)
		{
			if (PortalCooldownForPlayers[j] > 0)
			{
				PortalCooldownForPlayers[j]--;
			}
		}
		for (int k = 0; k < PortalCooldownForNPCs.Length; k++)
		{
			if (PortalCooldownForNPCs[k] > 0)
			{
				PortalCooldownForNPCs[k]--;
			}
		}
		for (int l = 0; l < 1000; l++)
		{
			Projectile projectile = Main.projectile[l];
			if (projectile.active && projectile.type == 602 && projectile.ai[1] >= 0f && projectile.ai[1] <= 1f && projectile.owner >= 0 && projectile.owner <= 255)
			{
				FoundPortals[projectile.owner, (int)projectile.ai[1]] = l;
				if (FoundPortals[projectile.owner, 0] != -1 && FoundPortals[projectile.owner, 1] != -1)
				{
					anyPortalAtAll = true;
				}
			}
		}
	}

	public static void TryGoingThroughPortals(Entity ent)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_0169: Unknown result type (might be due to invalid IL or missing references)
		//IL_0170: Unknown result type (might be due to invalid IL or missing references)
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		//IL_017c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0191: Unknown result type (might be due to invalid IL or missing references)
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0206: Unknown result type (might be due to invalid IL or missing references)
		//IL_020b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0217: Unknown result type (might be due to invalid IL or missing references)
		//IL_021c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0226: Unknown result type (might be due to invalid IL or missing references)
		//IL_022b: Unknown result type (might be due to invalid IL or missing references)
		//IL_022d: Unknown result type (might be due to invalid IL or missing references)
		//IL_022f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0231: Unknown result type (might be due to invalid IL or missing references)
		//IL_0236: Unknown result type (might be due to invalid IL or missing references)
		//IL_023d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0242: Unknown result type (might be due to invalid IL or missing references)
		//IL_0263: Unknown result type (might be due to invalid IL or missing references)
		//IL_0268: Unknown result type (might be due to invalid IL or missing references)
		//IL_0284: Unknown result type (might be due to invalid IL or missing references)
		//IL_028b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0290: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02df: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_030c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0311: Unknown result type (might be due to invalid IL or missing references)
		//IL_039c: Unknown result type (might be due to invalid IL or missing references)
		//IL_041f: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_043d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0444: Unknown result type (might be due to invalid IL or missing references)
		if (!anyPortalAtAll)
		{
			return;
		}
		float collisionPoint = 0f;
		_ = ent.velocity;
		int width = ent.width;
		int height = ent.height;
		int num = 1;
		if (ent is Player)
		{
			num = (int)((Player)ent).gravDir;
		}
		for (int i = 0; i < FoundPortals.GetLength(0); i++)
		{
			if (FoundPortals[i, 0] == -1 || FoundPortals[i, 1] == -1 || (ent is Player && (i >= PortalCooldownForPlayers.Length || PortalCooldownForPlayers[i] > 0)) || (ent is NPC && (i >= PortalCooldownForNPCs.Length || PortalCooldownForNPCs[i] > 0)))
			{
				continue;
			}
			for (int j = 0; j < 2; j++)
			{
				Projectile projectile = Main.projectile[FoundPortals[i, j]];
				GetPortalEdges(projectile.Center, projectile.ai[0], out var start, out var end);
				if (!Collision.CheckAABBvLineCollision(ent.position + ent.velocity, ent.Size, start, end, 2f, ref collisionPoint))
				{
					continue;
				}
				Projectile projectile2 = Main.projectile[FoundPortals[i, 1 - j]];
				float num2 = ent.Hitbox.Distance(projectile.Center);
				int bonusX;
				int bonusY;
				Vector2 vector = GetPortalOutingPoint(ent.Size, projectile2.Center, projectile2.ai[0], out bonusX, out bonusY) + Vector2.Normalize(new Vector2((float)bonusX, (float)bonusY)) * num2;
				Vector2 vector2 = Vector2.UnitX * 16f;
				if (Collision.TileCollision(vector - vector2, vector2, width, height, fallThrough: true, fall2: true, num) != vector2)
				{
					continue;
				}
				vector2 = -Vector2.UnitX * 16f;
				if (Collision.TileCollision(vector - vector2, vector2, width, height, fallThrough: true, fall2: true, num) != vector2)
				{
					continue;
				}
				vector2 = Vector2.UnitY * 16f;
				if (Collision.TileCollision(vector - vector2, vector2, width, height, fallThrough: true, fall2: true, num) != vector2)
				{
					continue;
				}
				vector2 = -Vector2.UnitY * 16f;
				if (Collision.TileCollision(vector - vector2, vector2, width, height, fallThrough: true, fall2: true, num) != vector2)
				{
					continue;
				}
				float num3 = 0.1f;
				if (bonusY == -num)
				{
					num3 = 0.1f;
				}
				if (ent.velocity == Vector2.Zero)
				{
					ent.velocity = (projectile.ai[0] - (float)Math.PI / 2f).ToRotationVector2() * num3;
				}
				if (((Vector2)(ref ent.velocity)).Length() < num3)
				{
					((Vector2)(ref ent.velocity)).Normalize();
					ent.velocity *= num3;
				}
				Vector2 vector3 = Vector2.Normalize(new Vector2((float)bonusX, (float)bonusY));
				if (vector3.HasNaNs() || vector3 == Vector2.Zero)
				{
					vector3 = Vector2.UnitX * (float)ent.direction;
				}
				ent.velocity = vector3 * ((Vector2)(ref ent.velocity)).Length();
				if ((bonusY == -num && Math.Sign(ent.velocity.Y) != -num) || Math.Abs(ent.velocity.Y) < 0.1f)
				{
					ent.velocity.Y = (float)(-num) * 0.1f;
				}
				int num4 = (int)((float)(projectile2.owner * 2) + projectile2.ai[1]);
				int lastPortalColorIndex = num4 + ((num4 % 2 == 0) ? 1 : (-1));
				if (ent is Player)
				{
					Player player = (Player)ent;
					player.lastPortalColorIndex = lastPortalColorIndex;
					player.Teleport(vector, 4, num4);
					if (Main.netMode == 1)
					{
						NetMessage.SendData(96, -1, -1, null, player.whoAmI, vector.X, vector.Y, num4);
						NetMessage.SendData(13, -1, -1, null, player.whoAmI);
					}
					PortalCooldownForPlayers[i] = 10;
				}
				else if (ent is NPC)
				{
					NPC nPC = (NPC)ent;
					nPC.lastPortalColorIndex = lastPortalColorIndex;
					nPC.Teleport(vector, 4, num4);
					if (Main.netMode == 2)
					{
						NetMessage.SendData(100, -1, -1, null, nPC.whoAmI, vector.X, vector.Y, num4);
						NetMessage.SendData(23, -1, -1, null, nPC.whoAmI);
					}
					PortalCooldownForPlayers[i] = 10;
					if (bonusY == -1 && ent.velocity.Y > -3f)
					{
						ent.velocity.Y = -3f;
					}
				}
				return;
			}
		}
	}

	public static int TryPlacingPortal(Projectile theBolt, Vector2 velocity, Vector2 theCrashVelocity)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0212: Unknown result type (might be due to invalid IL or missing references)
		//IL_0213: Unknown result type (might be due to invalid IL or missing references)
		//IL_021b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		Vector2 vector = velocity / ((Vector2)(ref velocity)).Length();
		Point position = FindCollision(theBolt.position, theBolt.position + velocity + vector * 32f).ToTileCoordinates();
		Tile tile = Main.tile[position.X, position.Y];
		Vector2 vector2 = default(Vector2);
		((Vector2)(ref vector2))._002Ector((float)(position.X * 16 + 8), (float)(position.Y * 16 + 8));
		if (!WorldGen.SolidOrSlopedTile(tile))
		{
			return -1;
		}
		int num = tile.slope();
		bool flag = tile.halfBrick();
		for (int i = 0; i < (flag ? 2 : EDGES.Length); i++)
		{
			if (Vector2.Dot(EDGES[i], vector) > 0f && FindValidLine(position, (int)EDGES[i].Y, (int)(0f - EDGES[i].X), out var bestPosition))
			{
				((Vector2)(ref vector2))._002Ector((float)(bestPosition.X * 16 + 8), (float)(bestPosition.Y * 16 + 8));
				return AddPortal(theBolt, vector2 - EDGES[i] * (flag ? 0f : 8f), (float)Math.Atan2(EDGES[i].Y, EDGES[i].X) + (float)Math.PI / 2f, (int)theBolt.ai[0], theBolt.direction);
			}
		}
		if (num != 0)
		{
			Vector2 value = SLOPE_EDGES[num - 1];
			if (Vector2.Dot(value, -vector) > 0f && FindValidLine(position, -SLOPE_OFFSETS[num - 1].Y, SLOPE_OFFSETS[num - 1].X, out var bestPosition2))
			{
				((Vector2)(ref vector2))._002Ector((float)(bestPosition2.X * 16 + 8), (float)(bestPosition2.Y * 16 + 8));
				return AddPortal(theBolt, vector2, (float)Math.Atan2(value.Y, value.X) - (float)Math.PI / 2f, (int)theBolt.ai[0], theBolt.direction);
			}
		}
		return -1;
	}

	private static bool FindValidLine(Point position, int xOffset, int yOffset, out Point bestPosition)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		bestPosition = position;
		if (IsValidLine(position, xOffset, yOffset))
		{
			return true;
		}
		Point point = default(Point);
		((Point)(ref point))._002Ector(position.X - xOffset, position.Y - yOffset);
		if (IsValidLine(point, xOffset, yOffset))
		{
			bestPosition = point;
			return true;
		}
		Point point2 = default(Point);
		((Point)(ref point2))._002Ector(position.X + xOffset, position.Y + yOffset);
		if (IsValidLine(point2, xOffset, yOffset))
		{
			bestPosition = point2;
			return true;
		}
		return false;
	}

	private static bool IsValidLine(Point position, int xOffset, int yOffset)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		Tile tile = Main.tile[position.X, position.Y];
		Tile tile2 = Main.tile[position.X - xOffset, position.Y - yOffset];
		Tile tile3 = Main.tile[position.X + xOffset, position.Y + yOffset];
		if (BlockPortals(Main.tile[position.X + yOffset, position.Y - xOffset]) || BlockPortals(Main.tile[position.X + yOffset - xOffset, position.Y - xOffset - yOffset]) || BlockPortals(Main.tile[position.X + yOffset + xOffset, position.Y - xOffset + yOffset]))
		{
			return false;
		}
		if (CanPlacePortalOn(tile) && CanPlacePortalOn(tile2) && CanPlacePortalOn(tile3) && tile2.HasSameSlope(tile) && tile3.HasSameSlope(tile))
		{
			return true;
		}
		return false;
	}

	private static bool CanPlacePortalOn(Tile t)
	{
		if (!DoesTileTypeSupportPortals(t.type))
		{
			return false;
		}
		return WorldGen.SolidOrSlopedTile(t);
	}

	private static bool DoesTileTypeSupportPortals(ushort tileType)
	{
		if (tileType == 496)
		{
			return false;
		}
		return true;
	}

	private static bool BlockPortals(Tile t)
	{
		if (t.active() && !Main.tileCut[t.type] && !TileID.Sets.BreakableWhenPlacing[t.type] && Main.tileSolid[t.type])
		{
			return true;
		}
		return false;
	}

	private static Vector2 FindCollision(Vector2 startPosition, Vector2 stopPosition)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		int lastX = 0;
		int lastY = 0;
		Utils.PlotLine(startPosition.ToTileCoordinates(), stopPosition.ToTileCoordinates(), delegate(int x, int y)
		{
			lastX = x;
			lastY = y;
			return !WorldGen.SolidOrSlopedTile(x, y);
		}, jump: false);
		return new Vector2((float)lastX * 16f, (float)lastY * 16f);
	}

	private static int AddPortal(Projectile sourceProjectile, Vector2 position, float angle, int form, int direction)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		if (!SupportedTilesAreFine(position, angle))
		{
			return -1;
		}
		RemoveMyOldPortal(form);
		RemoveIntersectingPortals(position, angle);
		int num = Projectile.NewProjectile(Entity.InheritSource(sourceProjectile), position.X, position.Y, 0f, 0f, 602, 0, 0f, Main.myPlayer, angle, form);
		Main.projectile[num].direction = direction;
		Main.projectile[num].netUpdate = true;
		return num;
	}

	private static void RemoveMyOldPortal(int form)
	{
		for (int i = 0; i < 1000; i++)
		{
			Projectile projectile = Main.projectile[i];
			if (projectile.active && projectile.type == 602 && projectile.owner == Main.myPlayer && projectile.ai[1] == (float)form)
			{
				projectile.Kill();
				break;
			}
		}
	}

	private static void RemoveIntersectingPortals(Vector2 position, float angle)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		GetPortalEdges(position, angle, out var start, out var end);
		for (int i = 0; i < 1000; i++)
		{
			Projectile projectile = Main.projectile[i];
			if (!projectile.active || projectile.type != 602)
			{
				continue;
			}
			GetPortalEdges(projectile.Center, projectile.ai[0], out var start2, out var end2);
			if (Collision.CheckLinevLine(start, end, start2, end2).Length != 0)
			{
				if (projectile.owner != Main.myPlayer && Main.netMode != 2)
				{
					NetMessage.SendData(95, -1, -1, null, projectile.owner, (int)projectile.ai[1]);
				}
				projectile.Kill();
			}
		}
	}

	public static Color GetPortalColor(int colorIndex)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		return GetPortalColor(colorIndex / 2, colorIndex % 2);
	}

	public static Color GetPortalColor(int player, int portal)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		Color white = Color.White;
		if (Main.netMode == 0)
		{
			white = ((portal != 0) ? Main.hslToRgb(0.52f, 1f, 0.6f) : Main.hslToRgb(0.12f, 1f, 0.5f));
		}
		else
		{
			float num = 0.08f;
			white = Main.hslToRgb((0.5f + (float)player * (num * 2f) + (float)portal * num) % 1f, 1f, 0.5f);
		}
		((Color)(ref white)).A = 66;
		return white;
	}

	private static void GetPortalEdges(Vector2 position, float angle, out Vector2 start, out Vector2 end)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		Vector2 vector = angle.ToRotationVector2();
		start = position + vector * -22f;
		end = position + vector * 22f;
	}

	private static Vector2 GetPortalOutingPoint(Vector2 objectSize, Vector2 portalPosition, float portalAngle, out int bonusX, out int bonusY)
	{
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		int num = (int)Math.Round(MathHelper.WrapAngle(portalAngle) / ((float)Math.PI / 4f));
		switch (num)
		{
		case -2:
		case 2:
			bonusX = ((num != 2) ? 1 : (-1));
			bonusY = 0;
			return portalPosition + new Vector2((num == 2) ? (0f - objectSize.X) : 0f, (0f - objectSize.Y) / 2f);
		case 0:
		case 4:
			bonusX = 0;
			bonusY = ((num == 0) ? 1 : (-1));
			return portalPosition + new Vector2((0f - objectSize.X) / 2f, (num == 0) ? 0f : (0f - objectSize.Y));
		case -3:
		case 3:
			bonusX = ((num == -3) ? 1 : (-1));
			bonusY = -1;
			return portalPosition + new Vector2((num == -3) ? 0f : (0f - objectSize.X), 0f - objectSize.Y);
		case -1:
		case 1:
			bonusX = ((num == -1) ? 1 : (-1));
			bonusY = 1;
			return portalPosition + new Vector2((num == -1) ? 0f : (0f - objectSize.X), 0f);
		default:
			bonusX = 0;
			bonusY = 0;
			return portalPosition;
		}
	}

	public static void SyncPortalsOnPlayerJoin(int plr, int fluff, List<Point> dontInclude, out List<Point> portalSections)
	{
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		portalSections = new List<Point>();
		for (int i = 0; i < 1000; i++)
		{
			Projectile projectile = Main.projectile[i];
			if (!projectile.active || (projectile.type != 602 && projectile.type != 601))
			{
				continue;
			}
			Vector2 center = projectile.Center;
			int sectionX = Netplay.GetSectionX((int)(center.X / 16f));
			int sectionY = Netplay.GetSectionY((int)(center.Y / 16f));
			for (int j = sectionX - fluff; j < sectionX + fluff + 1; j++)
			{
				for (int k = sectionY - fluff; k < sectionY + fluff + 1; k++)
				{
					if (j >= 0 && j < Main.maxSectionsX && k >= 0 && k < Main.maxSectionsY && !Netplay.Clients[plr].TileSections[j, k] && !dontInclude.Contains(new Point(j, k)))
					{
						portalSections.Add(new Point(j, k));
					}
				}
			}
		}
	}

	public static void SyncPortalSections(Vector2 portalPosition, int fluff)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < 255; i++)
		{
			if (Main.player[i].active)
			{
				RemoteClient.CheckSection(i, portalPosition, fluff);
			}
		}
	}

	public static bool SupportedTilesAreFine(Vector2 portalCenter, float portalAngle)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_0158: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_0165: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_020b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0213: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		Point point = portalCenter.ToTileCoordinates();
		int num = (int)Math.Round(MathHelper.WrapAngle(portalAngle) / ((float)Math.PI / 4f));
		int num7;
		int num2;
		int num3;
		int num6;
		int num5;
		switch (num)
		{
		case 2:
			num7 = -1;
			goto IL_004e;
		case -2:
			num7 = 1;
			goto IL_004e;
		case 0:
		case 4:
			num2 = 0;
			num3 = ((num == 0) ? 1 : (-1));
			break;
		case 3:
			num6 = -1;
			goto IL_0068;
		case -3:
			num6 = 1;
			goto IL_0068;
		case 1:
			num5 = -1;
			goto IL_0075;
		case -1:
			num5 = 1;
			goto IL_0075;
		default:
			{
				Main.NewText("Broken portal! (over4s = " + num + " , " + portalAngle + ")");
				return false;
			}
			IL_0075:
			num2 = num5;
			num3 = 1;
			break;
			IL_0068:
			num2 = num6;
			num3 = -1;
			break;
			IL_004e:
			num2 = num7;
			num3 = 0;
			break;
		}
		if (num2 != 0 && num3 != 0)
		{
			int num4 = 3;
			if (num2 == -1 && num3 == 1)
			{
				num4 = 5;
			}
			if (num2 == 1 && num3 == -1)
			{
				num4 = 2;
			}
			if (num2 == 1 && num3 == 1)
			{
				num4 = 4;
			}
			num4--;
			if (SupportedSlope(point.X, point.Y, num4) && SupportedSlope(point.X + num2, point.Y - num3, num4))
			{
				return SupportedSlope(point.X - num2, point.Y + num3, num4);
			}
			return false;
		}
		if (num2 != 0)
		{
			if (num2 == 1)
			{
				point.X--;
			}
			if (SupportedNormal(point.X, point.Y) && SupportedNormal(point.X, point.Y - 1))
			{
				return SupportedNormal(point.X, point.Y + 1);
			}
			return false;
		}
		if (num3 != 0)
		{
			if (num3 == 1)
			{
				point.Y--;
			}
			if (!SupportedNormal(point.X, point.Y) || !SupportedNormal(point.X + 1, point.Y) || !SupportedNormal(point.X - 1, point.Y))
			{
				if (SupportedHalfbrick(point.X, point.Y) && SupportedHalfbrick(point.X + 1, point.Y))
				{
					return SupportedHalfbrick(point.X - 1, point.Y);
				}
				return false;
			}
			return true;
		}
		return true;
	}

	private static bool SupportedSlope(int x, int y, int slope)
	{
		Tile tile = Main.tile[x, y];
		if (tile != null && tile.nactive() && !Main.tileCut[tile.type] && !TileID.Sets.BreakableWhenPlacing[tile.type] && Main.tileSolid[tile.type] && tile.slope() == slope)
		{
			return DoesTileTypeSupportPortals(tile.type);
		}
		return false;
	}

	private static bool SupportedHalfbrick(int x, int y)
	{
		Tile tile = Main.tile[x, y];
		if (tile != null && tile.nactive() && !Main.tileCut[tile.type] && !TileID.Sets.BreakableWhenPlacing[tile.type] && Main.tileSolid[tile.type] && tile.halfBrick())
		{
			return DoesTileTypeSupportPortals(tile.type);
		}
		return false;
	}

	private static bool SupportedNormal(int x, int y)
	{
		Tile tile = Main.tile[x, y];
		if (tile != null && tile.nactive() && !Main.tileCut[tile.type] && !TileID.Sets.BreakableWhenPlacing[tile.type] && Main.tileSolid[tile.type] && !TileID.Sets.NotReallySolid[tile.type] && !tile.halfBrick() && tile.slope() == 0)
		{
			return DoesTileTypeSupportPortals(tile.type);
		}
		return false;
	}
}
