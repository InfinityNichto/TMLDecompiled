namespace Terraria;

public static class TileDataPacking
{
	public static int Unpack(int bits, int offset, int width)
	{
		return (bits >> offset) & ((1 << width) - 1);
	}

	public static int Pack(int value, int bits, int offset, int width)
	{
		return (bits & ~((1 << width) - 1 << offset)) | ((value & ((1 << width) - 1)) << offset);
	}

	public static bool GetBit(int bits, int offset)
	{
		return (bits & (1 << offset)) != 0;
	}

	public static int SetBit(bool value, int bits, int offset)
	{
		if (!value)
		{
			return bits & ~(1 << offset);
		}
		return bits | (1 << offset);
	}
}
