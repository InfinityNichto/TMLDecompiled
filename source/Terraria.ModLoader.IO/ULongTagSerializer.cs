namespace Terraria.ModLoader.IO;

public class ULongTagSerializer : TagSerializer<ulong, long>
{
	public override long Serialize(ulong value)
	{
		return (long)value;
	}

	public override ulong Deserialize(long tag)
	{
		return (ulong)tag;
	}
}
