using System.IO;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Terraria.DataStructures;

public class PlayerDeathReason
{
	private int _sourcePlayerIndex = -1;

	private int _sourceNPCIndex = -1;

	private int _sourceProjectileLocalIndex = -1;

	private int _sourceOtherIndex = -1;

	private int _sourceProjectileType;

	private Item _sourceItem;

	private string _sourceCustomReason;

	private int _sourceItemType => _sourceItem?.type ?? 0;

	private int _sourceItemPrefix => _sourceItem?.prefix ?? 0;

	public ref int SourcePlayerIndex => ref _sourcePlayerIndex;

	public ref int SourceNPCIndex => ref _sourceNPCIndex;

	public ref int SourceProjectileLocalIndex => ref _sourceProjectileLocalIndex;

	public ref int SourceOtherIndex => ref _sourceOtherIndex;

	public ref int SourceProjectileType => ref _sourceProjectileType;

	public ref Item SourceItem => ref _sourceItem;

	public ref string SourceCustomReason => ref _sourceCustomReason;

	/// <summary>
	/// Only safe for use when the local player is the one taking damage! <br />
	/// Projectile ids are not synchronized across clients, and NPCs may have despawned/died by the time a strike/death packet arrives. <br />
	/// Just because the method returns true, doesn't mean that the _correct_ NPC/Projectile is returned on remote clients.
	/// </summary>
	/// <param name="entity"></param>
	/// <returns></returns>
	public bool TryGetCausingEntity(out Entity entity)
	{
		entity = null;
		if (Main.npc.IndexInRange(_sourceNPCIndex))
		{
			entity = Main.npc[_sourceNPCIndex];
			return true;
		}
		if (Main.projectile.IndexInRange(_sourceProjectileLocalIndex))
		{
			entity = Main.projectile[_sourceProjectileLocalIndex];
			if (Main.player.IndexInRange(_sourcePlayerIndex) && (_sourcePlayerIndex != Main.myPlayer || ((Projectile)entity).owner != _sourcePlayerIndex))
			{
				entity = Main.player[_sourcePlayerIndex];
			}
			return true;
		}
		if (Main.player.IndexInRange(_sourcePlayerIndex))
		{
			entity = Main.player[_sourcePlayerIndex];
			return true;
		}
		return false;
	}

	public static PlayerDeathReason LegacyEmpty()
	{
		return new PlayerDeathReason
		{
			_sourceOtherIndex = 254
		};
	}

	public static PlayerDeathReason LegacyDefault()
	{
		return new PlayerDeathReason
		{
			_sourceOtherIndex = 255
		};
	}

	public static PlayerDeathReason ByNPC(int index)
	{
		return new PlayerDeathReason
		{
			_sourceNPCIndex = index
		};
	}

	public static PlayerDeathReason ByCustomReason(string reasonInEnglish)
	{
		return new PlayerDeathReason
		{
			_sourceCustomReason = reasonInEnglish
		};
	}

	public static PlayerDeathReason ByPlayerItem(int index, Item item)
	{
		return new PlayerDeathReason
		{
			_sourcePlayerIndex = index,
			_sourceItem = item
		};
	}

	public static PlayerDeathReason ByOther(int type, int playerIndex = -1)
	{
		return new PlayerDeathReason
		{
			_sourcePlayerIndex = playerIndex,
			_sourceOtherIndex = type
		};
	}

	public static PlayerDeathReason ByProjectile(int playerIndex, int projectileIndex)
	{
		return new PlayerDeathReason
		{
			_sourcePlayerIndex = playerIndex,
			_sourceProjectileLocalIndex = projectileIndex,
			_sourceProjectileType = Main.projectile[projectileIndex].type
		};
	}

	public NetworkText GetDeathText(string deadPlayerName)
	{
		if (_sourceCustomReason != null)
		{
			return NetworkText.FromLiteral(_sourceCustomReason);
		}
		return Lang.CreateDeathMessage(deadPlayerName, _sourcePlayerIndex, _sourceNPCIndex, _sourceProjectileLocalIndex, _sourceOtherIndex, _sourceProjectileType, _sourceItemType);
	}

	public void WriteSelfTo(BinaryWriter writer)
	{
		BitsByte bitsByte = (byte)0;
		bitsByte[0] = _sourcePlayerIndex != -1;
		bitsByte[1] = _sourceNPCIndex != -1;
		bitsByte[2] = _sourceProjectileLocalIndex != -1;
		bitsByte[3] = _sourceOtherIndex != -1;
		bitsByte[4] = _sourceProjectileType != 0;
		bitsByte[5] = _sourceItemType != 0;
		bitsByte[6] = _sourceItemPrefix != 0;
		bitsByte[7] = _sourceCustomReason != null;
		writer.Write(bitsByte);
		if (bitsByte[0])
		{
			writer.Write((short)_sourcePlayerIndex);
		}
		if (bitsByte[1])
		{
			writer.Write((short)_sourceNPCIndex);
		}
		if (bitsByte[2])
		{
			writer.Write((short)_sourceProjectileLocalIndex);
		}
		if (bitsByte[3])
		{
			writer.Write((byte)_sourceOtherIndex);
		}
		if (bitsByte[4])
		{
			writer.Write((short)_sourceProjectileType);
		}
		if (!ModNet.AllowVanillaClients)
		{
			if (bitsByte[5])
			{
				ItemIO.Send(_sourceItem, writer);
			}
		}
		else
		{
			if (bitsByte[5])
			{
				writer.Write((short)_sourceItemType);
			}
			if (bitsByte[6])
			{
				writer.Write((byte)_sourceItemPrefix);
			}
		}
		if (bitsByte[7])
		{
			writer.Write(_sourceCustomReason);
		}
	}

	public static PlayerDeathReason FromReader(BinaryReader reader)
	{
		PlayerDeathReason playerDeathReason = new PlayerDeathReason();
		BitsByte bitsByte = reader.ReadByte();
		if (bitsByte[0])
		{
			playerDeathReason._sourcePlayerIndex = reader.ReadInt16();
		}
		if (bitsByte[1])
		{
			playerDeathReason._sourceNPCIndex = reader.ReadInt16();
		}
		if (bitsByte[2])
		{
			playerDeathReason._sourceProjectileLocalIndex = reader.ReadInt16();
		}
		if (bitsByte[3])
		{
			playerDeathReason._sourceOtherIndex = reader.ReadByte();
		}
		if (bitsByte[4])
		{
			playerDeathReason._sourceProjectileType = reader.ReadInt16();
		}
		if (ModNet.AllowVanillaClients)
		{
			int itemType = (bitsByte[5] ? reader.ReadInt16() : 0);
			int prefix = (bitsByte[6] ? reader.ReadByte() : 0);
			playerDeathReason._sourceItem = ((itemType == 0) ? null : new Item(itemType, 1, prefix));
		}
		else if (bitsByte[5])
		{
			playerDeathReason._sourceItem = ItemIO.Receive(reader);
		}
		if (bitsByte[7])
		{
			playerDeathReason._sourceCustomReason = reader.ReadString();
		}
		return playerDeathReason;
	}
}
