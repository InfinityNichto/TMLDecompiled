using Terraria.DataStructures;

namespace Terraria.ModLoader.IO;

public class Point16Serializer : TagSerializer<Point16, TagCompound>
{
	public override TagCompound Serialize(Point16 value)
	{
		return new TagCompound
		{
			["x"] = value.X,
			["y"] = value.Y
		};
	}

	public override Point16 Deserialize(TagCompound tag)
	{
		return new Point16(tag.GetShort("x"), tag.GetShort("y"));
	}
}
