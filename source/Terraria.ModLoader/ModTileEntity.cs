using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Terraria.DataStructures;

namespace Terraria.ModLoader;

/// <summary>
/// Tile Entities are Entities tightly coupled with tiles, allowing the possibility of tiles to exhibit cool behavior. TileEntity.Update is called in SP and on Server, not on Clients.
/// </summary>
/// <seealso cref="T:Terraria.DataStructures.TileEntity" />
public abstract class ModTileEntity : TileEntity, IModType, ILoadable
{
	public static readonly int NumVanilla = Assembly.GetExecutingAssembly().GetTypes().Count((Type t) => !t.IsAbstract && t.IsSubclassOf(typeof(TileEntity)) && !typeof(ModTileEntity).IsAssignableFrom(t));

	/// <summary>
	/// The mod that added this ModTileEntity.
	/// </summary>
	public Mod Mod { get; internal set; }

	/// <summary>
	/// The internal name of this ModTileEntity.
	/// </summary>
	public virtual string Name => GetType().Name;

	public string FullName => Mod.Name + "/" + Name;

	/// <summary>
	/// The numeric type used to identify this kind of tile entity.
	/// </summary>
	public int Type { get; internal set; }

	public ModTileEntity()
	{
	}

	/// <summary>
	/// Returns the number of modded tile entities that exist in the world currently being played.
	/// </summary>
	public static int CountInWorld()
	{
		return TileEntity.ByID.Count((KeyValuePair<int, TileEntity> pair) => pair.Value.type >= NumVanilla);
	}

	internal static void Initialize()
	{
		TileEntity._UpdateStart += UpdateStartInternal;
		TileEntity._UpdateEnd += UpdateEndInternal;
	}

	private static void UpdateStartInternal()
	{
		foreach (ModTileEntity item in TileEntity.manager.EnumerateEntities().Values.OfType<ModTileEntity>())
		{
			item.PreGlobalUpdate();
		}
	}

	private static void UpdateEndInternal()
	{
		foreach (ModTileEntity item in TileEntity.manager.EnumerateEntities().Values.OfType<ModTileEntity>())
		{
			item.PostGlobalUpdate();
		}
	}

	/// <summary>
	/// You should never use this. It is only included here for completion's sake.
	/// </summary>
	public override void NetPlaceEntityAttempt(int i, int j)
	{
		if (TileEntity.manager.TryGetTileEntity<ModTileEntity>(Type, out var modTileEntity))
		{
			int id = modTileEntity.Place(i, j);
			((ModTileEntity)TileEntity.ByID[id]).OnNetPlace();
			NetMessage.SendData(86, -1, -1, null, id, i, j);
		}
	}

	/// <summary>
	/// Returns a new ModTileEntity with the same class, mod, name, and type as the ModTileEntity with the given type. It is very rare that you should have to use this.
	/// </summary>
	public static ModTileEntity ConstructFromType(int type)
	{
		if (!TileEntity.manager.TryGetTileEntity<ModTileEntity>(type, out var modTileEntity))
		{
			return null;
		}
		return ConstructFromBase(modTileEntity);
	}

	/// <summary>
	/// Returns a new ModTileEntity with the same class, mod, name, and type as the parameter. It is very rare that you should have to use this.
	/// </summary>
	public static ModTileEntity ConstructFromBase(ModTileEntity tileEntity)
	{
		ModTileEntity obj = (ModTileEntity)Activator.CreateInstance(tileEntity.GetType(), nonPublic: true);
		obj.Mod = tileEntity.Mod;
		obj.Type = tileEntity.Type;
		return obj;
	}

	/// <summary>
	/// A helper method that places this kind of tile entity in the given coordinates for you.
	/// </summary>
	public int Place(int i, int j)
	{
		ModTileEntity newEntity = ConstructFromBase(this);
		newEntity.Position = new Point16(i, j);
		newEntity.ID = TileEntity.AssignNewID();
		newEntity.type = (byte)Type;
		lock (TileEntity.EntityCreationLock)
		{
			TileEntity.ByID[newEntity.ID] = newEntity;
			TileEntity.ByPosition[newEntity.Position] = newEntity;
		}
		return newEntity.ID;
	}

	/// <summary>
	/// A helper method that removes this kind of tile entity from the given coordinates for you.
	/// </summary>
	public void Kill(int i, int j)
	{
		Point16 pos = new Point16(i, j);
		if (TileEntity.ByPosition.TryGetValue(pos, out var tileEntity) && tileEntity.type == Type)
		{
			((ModTileEntity)tileEntity).OnKill();
			TileEntity.ByID.Remove(tileEntity.ID);
			TileEntity.ByPosition.Remove(pos);
		}
	}

	/// <summary>
	/// Returns the entity ID of this kind of tile entity at the given coordinates for you.
	/// </summary>
	public int Find(int i, int j)
	{
		Point16 pos = new Point16(i, j);
		if (TileEntity.ByPosition.TryGetValue(pos, out var tileEntity) && tileEntity.type == Type)
		{
			return tileEntity.ID;
		}
		return -1;
	}

	/// <summary>
	/// Should never be called on ModTileEntity. Replaced by NetSend and Save.
	/// Would make the base method internal if not for patch size
	/// </summary>
	public sealed override void WriteExtraData(BinaryWriter writer, bool networkSend)
	{
		throw new NotImplementedException();
	}

	/// <summary>
	/// Should never be called on ModTileEntity. Replaced by NetReceive and Load
	/// Would make the base method internal if not for patch size
	/// </summary>
	public sealed override void ReadExtraData(BinaryReader reader, bool networkSend)
	{
		throw new NotImplementedException();
	}

	public override void NetSend(BinaryWriter writer)
	{
	}

	public override void NetReceive(BinaryReader reader)
	{
	}

	public sealed override TileEntity GenerateInstance()
	{
		return ConstructFromBase(this);
	}

	public sealed override void RegisterTileEntityID(int assignedID)
	{
		Type = assignedID;
	}

	void ILoadable.Load(Mod mod)
	{
		Mod = mod;
		if (!Mod.loading)
		{
			throw new Exception("AddTileEntity can only be called from Mod.Load or Mod.Autoload");
		}
		Load();
		Load_Obsolete(mod);
		TileEntity.manager.Register(this);
		ModTypeLookup<ModTileEntity>.Register(this);
	}

	[Obsolete]
	private void Load_Obsolete(Mod mod)
	{
		Load(mod);
	}

	[Obsolete("Override the parameterless Load() overload instead.", true)]
	public virtual void Load(Mod mod)
	{
	}

	public virtual void Load()
	{
	}

	public virtual bool IsLoadingEnabled(Mod mod)
	{
		return true;
	}

	public virtual void Unload()
	{
	}

	/// <summary>
	/// This method does not get called by tModLoader, and is only included for you convenience so you do not have to cast the result of Mod.GetTileEntity.
	/// </summary>
	public virtual int Hook_AfterPlacement(int i, int j, int type, int style, int direction, int alternate)
	{
		return -1;
	}

	/// <summary>
	/// Code that should be run when this tile entity is placed by means of server-syncing. Called on Server only.
	/// </summary>
	public virtual void OnNetPlace()
	{
	}

	/// <summary>
	/// Code that should be run before all tile entities in the world update.
	/// </summary>
	public virtual void PreGlobalUpdate()
	{
	}

	/// <summary>
	/// Code that should be run after all tile entities in the world update.
	/// </summary>
	public virtual void PostGlobalUpdate()
	{
	}

	/// <summary>
	/// This method only gets called in the Kill method. If you plan to use that, you can put code here to make things happen when it is called.
	/// </summary>
	public virtual void OnKill()
	{
	}

	/// <summary>
	/// Whether or not this tile entity is allowed to survive at the given coordinates. You should check whether the tile is active, as well as the tile's type and frame.
	/// </summary>
	public abstract override bool IsTileValidForEntity(int x, int y);
}
