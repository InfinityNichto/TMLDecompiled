using Microsoft.Xna.Framework;

namespace Terraria.ModLoader.IO;

public class Vector2TagSerializer : TagSerializer<Vector2, TagCompound>
{
	public override TagCompound Serialize(Vector2 value)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		return new TagCompound
		{
			["x"] = value.X,
			["y"] = value.Y
		};
	}

	public override Vector2 Deserialize(TagCompound tag)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		return new Vector2(tag.GetFloat("x"), tag.GetFloat("y"));
	}
}
