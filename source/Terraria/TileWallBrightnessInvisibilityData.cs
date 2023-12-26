namespace Terraria;

public struct TileWallBrightnessInvisibilityData : ITileData
{
	private BitsByte bitpack;

	public byte Data => bitpack;

	public bool IsTileInvisible
	{
		get
		{
			return bitpack[0];
		}
		set
		{
			bitpack[0] = value;
		}
	}

	public bool IsWallInvisible
	{
		get
		{
			return bitpack[1];
		}
		set
		{
			bitpack[1] = value;
		}
	}

	public bool IsTileFullbright
	{
		get
		{
			return bitpack[2];
		}
		set
		{
			bitpack[2] = value;
		}
	}

	public bool IsWallFullbright
	{
		get
		{
			return bitpack[3];
		}
		set
		{
			bitpack[3] = value;
		}
	}
}
