using Microsoft.Xna.Framework;

namespace Terraria.ModLoader.IO;

public class ColorSerializer : TagSerializer<Color, int>
{
	public override int Serialize(Color value)
	{
		return (int)((Color)(ref value)).PackedValue;
	}

	public override Color Deserialize(int tag)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		return new Color(tag & 0xFF, (tag >> 8) & 0xFF, (tag >> 16) & 0xFF, (tag >> 24) & 0xFF);
	}
}
