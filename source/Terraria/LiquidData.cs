namespace Terraria;

public struct LiquidData : ITileData
{
	public byte Amount;

	private byte typeAndFlags;

	public int LiquidType
	{
		get
		{
			return TileDataPacking.Unpack(typeAndFlags, 0, 6);
		}
		set
		{
			typeAndFlags = (byte)TileDataPacking.Pack(value, typeAndFlags, 0, 6);
		}
	}

	public bool SkipLiquid
	{
		get
		{
			return TileDataPacking.GetBit(typeAndFlags, 6);
		}
		set
		{
			typeAndFlags = (byte)TileDataPacking.SetBit(value, typeAndFlags, 6);
		}
	}

	public bool CheckingLiquid
	{
		get
		{
			return TileDataPacking.GetBit(typeAndFlags, 7);
		}
		set
		{
			typeAndFlags = (byte)TileDataPacking.SetBit(value, typeAndFlags, 7);
		}
	}
}
