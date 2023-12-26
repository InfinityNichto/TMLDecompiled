using Microsoft.Xna.Framework;

namespace Terraria.ModLoader.IO;

public class RectangleSerializer : TagSerializer<Rectangle, TagCompound>
{
	public override TagCompound Serialize(Rectangle value)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		return new TagCompound
		{
			["x"] = value.X,
			["y"] = value.Y,
			["width"] = value.Width,
			["height"] = value.Height
		};
	}

	public override Rectangle Deserialize(TagCompound tag)
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		return new Rectangle(tag.GetInt("x"), tag.GetInt("y"), tag.GetInt("width"), tag.GetInt("height"));
	}
}
