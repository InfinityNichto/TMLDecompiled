using System;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace Terraria;

public abstract class Entity
{
	/// <summary>
	/// The index of this Entity within its specific array. These arrays track the entities in the world.
	/// <br /> Item: unused
	/// <br /> Projectile: <see cref="F:Terraria.Main.projectile" />
	/// <br /> NPC: <see cref="F:Terraria.Main.npc" />
	/// <br /> Player: <see cref="F:Terraria.Main.player" />
	/// </summary>
	public int whoAmI;

	/// <summary>
	/// If true, the Entity actually exists within the game world. Within the specific entity array, if active is false, the entity is junk data. Always check active if iterating over the entity array.
	/// </summary>
	public bool active;

	internal long entityId;

	/// <summary>
	/// The position of this Entity in world coordinates.
	/// </summary>
	public Vector2 position;

	/// <summary>
	/// The velocity of this Entity in world coordinates per tick.
	/// </summary>
	public Vector2 velocity;

	public Vector2 oldPosition;

	public Vector2 oldVelocity;

	public int oldDirection;

	public int direction = 1;

	/// <summary>
	/// The width of this Entity's hitbox, in pixels.
	/// </summary>
	public int width;

	/// <summary>
	/// The height of this Entity's hitbox, in pixels.
	/// </summary>
	public int height;

	/// <summary>
	/// The Entity is currently in water.
	/// <br /> Projectile: Affects movement speed and some projectiles die when wet. <see cref="F:Terraria.Projectile.ignoreWater" /> prevents this.
	/// </summary>
	public bool wet;

	public bool shimmerWet;

	public bool honeyWet;

	public byte wetCount;

	public bool lavaWet;

	public virtual Vector2 VisualPosition => position;

	public Vector2 Center
	{
		get
		{
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			return new Vector2(position.X + (float)(width / 2), position.Y + (float)(height / 2));
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			position = new Vector2(value.X - (float)(width / 2), value.Y - (float)(height / 2));
		}
	}

	public Vector2 Left
	{
		get
		{
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			return new Vector2(position.X, position.Y + (float)(height / 2));
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			position = new Vector2(value.X, value.Y - (float)(height / 2));
		}
	}

	public Vector2 Right
	{
		get
		{
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			return new Vector2(position.X + (float)width, position.Y + (float)(height / 2));
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			position = new Vector2(value.X - (float)width, value.Y - (float)(height / 2));
		}
	}

	public Vector2 Top
	{
		get
		{
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			return new Vector2(position.X + (float)(width / 2), position.Y);
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			position = new Vector2(value.X - (float)(width / 2), value.Y);
		}
	}

	public Vector2 TopLeft
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return position;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			position = value;
		}
	}

	public Vector2 TopRight
	{
		get
		{
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			return new Vector2(position.X + (float)width, position.Y);
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			position = new Vector2(value.X - (float)width, value.Y);
		}
	}

	public Vector2 Bottom
	{
		get
		{
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			return new Vector2(position.X + (float)(width / 2), position.Y + (float)height);
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			position = new Vector2(value.X - (float)(width / 2), value.Y - (float)height);
		}
	}

	public Vector2 BottomLeft
	{
		get
		{
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			return new Vector2(position.X, position.Y + (float)height);
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			position = new Vector2(value.X, value.Y - (float)height);
		}
	}

	public Vector2 BottomRight
	{
		get
		{
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			return new Vector2(position.X + (float)width, position.Y + (float)height);
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			position = new Vector2(value.X - (float)width, value.Y - (float)height);
		}
	}

	public Vector2 Size
	{
		get
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			return new Vector2((float)width, (float)height);
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			width = (int)value.X;
			height = (int)value.Y;
		}
	}

	public Rectangle Hitbox
	{
		get
		{
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			return new Rectangle((int)position.X, (int)position.Y, width, height);
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			position = new Vector2((float)value.X, (float)value.Y);
			width = value.Width;
			height = value.Height;
		}
	}

	public float AngleTo(Vector2 Destination)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		return (float)Math.Atan2(Destination.Y - Center.Y, Destination.X - Center.X);
	}

	public float AngleFrom(Vector2 Source)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		return (float)Math.Atan2(Center.Y - Source.Y, Center.X - Source.X);
	}

	public float Distance(Vector2 Other)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		return Vector2.Distance(Center, Other);
	}

	public float DistanceSQ(Vector2 Other)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		return Vector2.DistanceSquared(Center, Other);
	}

	public Vector2 DirectionTo(Vector2 Destination)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		return Vector2.Normalize(Destination - Center);
	}

	public Vector2 DirectionFrom(Vector2 Source)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		return Vector2.Normalize(Center - Source);
	}

	public bool WithinRange(Vector2 Target, float MaxRange)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		return Vector2.DistanceSquared(Center, Target) <= MaxRange * MaxRange;
	}

	public IEntitySource GetSource_FromThis(string? context = null)
	{
		return new EntitySource_Parent(this, context);
	}

	public IEntitySource GetSource_FromAI(string? context = null)
	{
		return new EntitySource_Parent(this, context);
	}

	public IEntitySource GetSource_DropAsItem(string? context = null)
	{
		return new EntitySource_DropAsItem(this, context);
	}

	public IEntitySource GetSource_Loot(string? context = null)
	{
		return new EntitySource_Loot(this, context);
	}

	public IEntitySource GetSource_GiftOrReward(string? context = null)
	{
		return new EntitySource_Gift(this, context);
	}

	public IEntitySource GetSource_OnHit(Entity victim, string? context = null)
	{
		return new EntitySource_OnHit(this, victim, context);
	}

	public IEntitySource GetSource_OnHurt(Entity? attacker, string? context = null)
	{
		return new EntitySource_OnHurt(this, attacker, context);
	}

	public IEntitySource GetSource_Death(string? context = null)
	{
		return new EntitySource_Death(this, context);
	}

	public IEntitySource GetSource_Misc(string context)
	{
		return new EntitySource_Misc(context);
	}

	public IEntitySource GetSource_TileInteraction(int tileCoordsX, int tileCoordsY, string? context = null)
	{
		return new EntitySource_TileInteraction(this, tileCoordsX, tileCoordsY, context);
	}

	public IEntitySource GetSource_ReleaseEntity(string? context = null)
	{
		return new EntitySource_Parent(this, context);
	}

	public IEntitySource GetSource_CatchEntity(Entity caughtEntity, string? context = null)
	{
		return new EntitySource_Caught(this, caughtEntity, context);
	}

	public static IEntitySource? GetSource_None()
	{
		return null;
	}

	public static IEntitySource? InheritSource(Entity entity)
	{
		if (entity == null)
		{
			return GetSource_None();
		}
		return entity.GetSource_FromThis();
	}

	public static IEntitySource GetSource_NaturalSpawn()
	{
		return new EntitySource_SpawnNPC();
	}

	public static IEntitySource GetSource_TownSpawn()
	{
		return new EntitySource_SpawnNPC();
	}
}
