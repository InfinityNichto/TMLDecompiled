using System.Collections.Generic;
using Terraria.GameContent.Tile_Entities;

namespace Terraria.DataStructures;

public class TileEntitiesManager
{
	private int _nextEntityID;

	private Dictionary<int, TileEntity> _types = new Dictionary<int, TileEntity>();

	public static int VanillaTypeCount;

	private int AssignNewID()
	{
		return _nextEntityID++;
	}

	private bool InvalidEntityID(int id)
	{
		if (id >= 0)
		{
			return id >= _nextEntityID;
		}
		return true;
	}

	public void RegisterAll()
	{
		Register(new TETrainingDummy());
		Register(new TEItemFrame());
		Register(new TELogicSensor());
		Register(new TEDisplayDoll());
		Register(new TEWeaponsRack());
		Register(new TEHatRack());
		Register(new TEFoodPlatter());
		Register(new TETeleportationPylon());
		VanillaTypeCount = _nextEntityID;
	}

	public void Register(TileEntity entity)
	{
		int num = AssignNewID();
		_types[num] = entity;
		entity.RegisterTileEntityID(num);
	}

	public bool CheckValidTile(int id, int x, int y)
	{
		if (InvalidEntityID(id))
		{
			return false;
		}
		return _types[id].IsTileValidForEntity(x, y);
	}

	public void NetPlaceEntity(int id, int x, int y)
	{
		if (!InvalidEntityID(id) && _types[id].IsTileValidForEntity(x, y))
		{
			_types[id].NetPlaceEntityAttempt(x, y);
		}
	}

	public TileEntity GenerateInstance(int id)
	{
		if (InvalidEntityID(id))
		{
			return null;
		}
		return _types[id].GenerateInstance();
	}

	/// <summary> Gets the template TileEntity object with the given id (not the new instance which gets added to the world as the game is played). This method will throw exceptions on failure. </summary>
	/// <exception cref="T:System.Collections.Generic.KeyNotFoundException" />
	public TileEntity GetTileEntity<T>(int id) where T : TileEntity
	{
		return _types[id] as T;
	}

	/// <summary> Attempts to get the template TileEntity object with the given id (not the new instance which gets added to the world as the game is played). </summary>
	public bool TryGetTileEntity<T>(int id, out T tileEntity) where T : TileEntity
	{
		if (!_types.TryGetValue(id, out var entity))
		{
			tileEntity = null;
			return false;
		}
		return (tileEntity = entity as T) != null;
	}

	public IReadOnlyDictionary<int, TileEntity> EnumerateEntities()
	{
		return _types;
	}

	internal void Reset()
	{
		_types.Clear();
		_nextEntityID = 0;
		RegisterAll();
	}
}
