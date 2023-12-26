namespace Terraria.ModLoader.IO;

public class BoolTagSerializer : TagSerializer<bool, byte>
{
	public override byte Serialize(bool value)
	{
		return (byte)(value ? 1u : 0u);
	}

	public override bool Deserialize(byte tag)
	{
		return tag != 0;
	}
}
