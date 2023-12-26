using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace Terraria.GameContent;

public class TownRoomManager
{
	public static object EntityCreationLock = new object();

	internal List<Tuple<int, Point>> _roomLocationPairs = new List<Tuple<int, Point>>();

	internal bool[] _hasRoom = new bool[NPCLoader.NPCCount];

	public void AddOccupantsToList(int x, int y, List<int> occupantsList)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		AddOccupantsToList(new Point(x, y), occupantsList);
	}

	public void AddOccupantsToList(Point tilePosition, List<int> occupants)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		foreach (Tuple<int, Point> roomLocationPair in _roomLocationPairs)
		{
			if (roomLocationPair.Item2 == tilePosition)
			{
				occupants.Add(roomLocationPair.Item1);
			}
		}
	}

	public bool HasRoomQuick(int npcID)
	{
		return _hasRoom[npcID];
	}

	public bool HasRoom(int npcID, out Point roomPosition)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		if (!_hasRoom[npcID])
		{
			roomPosition = new Point(0, 0);
			return false;
		}
		foreach (Tuple<int, Point> roomLocationPair in _roomLocationPairs)
		{
			if (roomLocationPair.Item1 == npcID)
			{
				roomPosition = roomLocationPair.Item2;
				return true;
			}
		}
		roomPosition = new Point(0, 0);
		return false;
	}

	public void SetRoom(int npcID, int x, int y)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		_hasRoom[npcID] = true;
		SetRoom(npcID, new Point(x, y));
	}

	public void SetRoom(int npcID, Point pt)
	{
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		lock (EntityCreationLock)
		{
			_roomLocationPairs.RemoveAll((Tuple<int, Point> x) => x.Item1 == npcID);
			_roomLocationPairs.Add(Tuple.Create<int, Point>(npcID, pt));
		}
	}

	public void KickOut(NPC n)
	{
		KickOut(n.type);
		_hasRoom[n.type] = false;
	}

	public void KickOut(int npcType)
	{
		lock (EntityCreationLock)
		{
			_roomLocationPairs.RemoveAll((Tuple<int, Point> x) => x.Item1 == npcType);
		}
	}

	public void DisplayRooms()
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		foreach (Tuple<int, Point> roomLocationPair in _roomLocationPairs)
		{
			Dust.QuickDust(roomLocationPair.Item2, Main.hslToRgb((float)roomLocationPair.Item1 * 0.05f % 1f, 1f, 0.5f));
		}
	}

	public void Save(BinaryWriter writer)
	{
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		lock (EntityCreationLock)
		{
			int count = 0;
			foreach (Tuple<int, Point> roomLocationPair2 in _roomLocationPairs)
			{
				if (roomLocationPair2.Item1 < NPCID.Count)
				{
					count++;
				}
			}
			writer.Write(count);
			foreach (Tuple<int, Point> roomLocationPair in _roomLocationPairs)
			{
				if (roomLocationPair.Item1 < NPCID.Count)
				{
					writer.Write(roomLocationPair.Item1);
					writer.Write(roomLocationPair.Item2.X);
					writer.Write(roomLocationPair.Item2.Y);
				}
			}
		}
	}

	public void Load(BinaryReader reader)
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		Clear();
		int num = reader.ReadInt32();
		Point item = default(Point);
		for (int i = 0; i < num; i++)
		{
			int num2 = reader.ReadInt32();
			((Point)(ref item))._002Ector(reader.ReadInt32(), reader.ReadInt32());
			_roomLocationPairs.Add(Tuple.Create<int, Point>(num2, item));
			_hasRoom[num2] = true;
		}
	}

	public void Clear()
	{
		_roomLocationPairs.Clear();
		for (int i = 0; i < _hasRoom.Length; i++)
		{
			_hasRoom[i] = false;
		}
	}

	public byte GetHouseholdStatus(NPC n)
	{
		byte result = 0;
		if (n.homeless)
		{
			result = 1;
		}
		else if (HasRoomQuick(n.type))
		{
			result = 2;
		}
		return result;
	}

	public bool CanNPCsLiveWithEachOther(int npc1ByType, NPC npc2)
	{
		if (!ContentSamples.NpcsByNetId.TryGetValue(npc1ByType, out var value))
		{
			return true;
		}
		return CanNPCsLiveWithEachOther(value, npc2);
	}

	public bool CanNPCsLiveWithEachOther(NPC npc1, NPC npc2)
	{
		return npc1.housingCategory != npc2.housingCategory;
	}

	public bool CanNPCsLiveWithEachOther_ShopHelper(NPC npc1, NPC npc2)
	{
		return CanNPCsLiveWithEachOther(npc1, npc2);
	}
}
