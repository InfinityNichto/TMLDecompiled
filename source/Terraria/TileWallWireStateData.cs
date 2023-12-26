using Terraria.ID;

namespace Terraria;

public struct TileWallWireStateData : ITileData
{
	public short TileFrameX;

	public short TileFrameY;

	private int bitpack;

	public bool HasTile
	{
		get
		{
			return TileDataPacking.GetBit(bitpack, 0);
		}
		set
		{
			bitpack = TileDataPacking.SetBit(value, bitpack, 0);
		}
	}

	public bool IsActuated
	{
		get
		{
			return TileDataPacking.GetBit(bitpack, 1);
		}
		set
		{
			bitpack = TileDataPacking.SetBit(value, bitpack, 1);
		}
	}

	public bool HasActuator
	{
		get
		{
			return TileDataPacking.GetBit(bitpack, 2);
		}
		set
		{
			bitpack = TileDataPacking.SetBit(value, bitpack, 2);
		}
	}

	public byte TileColor
	{
		get
		{
			return (byte)TileDataPacking.Unpack(bitpack, 3, 5);
		}
		set
		{
			bitpack = TileDataPacking.Pack(value, bitpack, 3, 5);
		}
	}

	public byte WallColor
	{
		get
		{
			return (byte)TileDataPacking.Unpack(bitpack, 8, 5);
		}
		set
		{
			bitpack = TileDataPacking.Pack(value, bitpack, 8, 5);
		}
	}

	public int TileFrameNumber
	{
		get
		{
			return TileDataPacking.Unpack(bitpack, 13, 2);
		}
		set
		{
			bitpack = TileDataPacking.Pack(value, bitpack, 13, 2);
		}
	}

	public int WallFrameNumber
	{
		get
		{
			return TileDataPacking.Unpack(bitpack, 15, 2);
		}
		set
		{
			bitpack = TileDataPacking.Pack(value, bitpack, 15, 2);
		}
	}

	public int WallFrameX
	{
		get
		{
			return TileDataPacking.Unpack(bitpack, 17, 4) * 36;
		}
		set
		{
			bitpack = TileDataPacking.Pack(value / 36, bitpack, 17, 4);
		}
	}

	public int WallFrameY
	{
		get
		{
			return TileDataPacking.Unpack(bitpack, 21, 3) * 36;
		}
		set
		{
			bitpack = TileDataPacking.Pack(value / 36, bitpack, 21, 3);
		}
	}

	public bool IsHalfBlock
	{
		get
		{
			return TileDataPacking.GetBit(bitpack, 24);
		}
		set
		{
			bitpack = TileDataPacking.SetBit(value, bitpack, 24);
		}
	}

	public SlopeType Slope
	{
		get
		{
			return (SlopeType)TileDataPacking.Unpack(bitpack, 25, 3);
		}
		set
		{
			bitpack = TileDataPacking.Pack((int)value, bitpack, 25, 3);
		}
	}

	public int WireData
	{
		get
		{
			return TileDataPacking.Unpack(bitpack, 28, 4);
		}
		set
		{
			bitpack = TileDataPacking.Pack(value, bitpack, 28, 4);
		}
	}

	public bool RedWire
	{
		get
		{
			return TileDataPacking.GetBit(bitpack, 28);
		}
		set
		{
			bitpack = TileDataPacking.SetBit(value, bitpack, 28);
		}
	}

	public bool BlueWire
	{
		get
		{
			return TileDataPacking.GetBit(bitpack, 29);
		}
		set
		{
			bitpack = TileDataPacking.SetBit(value, bitpack, 29);
		}
	}

	public bool GreenWire
	{
		get
		{
			return TileDataPacking.GetBit(bitpack, 30);
		}
		set
		{
			bitpack = TileDataPacking.SetBit(value, bitpack, 30);
		}
	}

	public bool YellowWire
	{
		get
		{
			return TileDataPacking.GetBit(bitpack, 31);
		}
		set
		{
			bitpack = TileDataPacking.SetBit(value, bitpack, 31);
		}
	}

	public int NonFrameBits => (int)(bitpack & 0xFF001FFFu);

	public BlockType BlockType
	{
		get
		{
			if (IsHalfBlock)
			{
				return BlockType.HalfBlock;
			}
			SlopeType slope = Slope;
			if (slope != 0)
			{
				return (BlockType)(slope + 1);
			}
			return BlockType.Solid;
		}
		set
		{
			IsHalfBlock = value == BlockType.HalfBlock;
			Slope = (SlopeType)((value > BlockType.HalfBlock) ? (value - 1) : BlockType.Solid);
		}
	}

	/// <summary>
	/// Intended to be used to set all the persistent data about a tile. For example, when loading a schematic from serialized NonFrameBits.
	/// </summary>
	public void SetAllBitsClearFrame(int nonFrameBits)
	{
		bitpack = (int)(nonFrameBits & 0xFF001FFFu);
	}
}
