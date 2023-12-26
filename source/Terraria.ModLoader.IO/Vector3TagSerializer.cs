using Microsoft.Xna.Framework;

namespace Terraria.ModLoader.IO;

public class Vector3TagSerializer : TagSerializer<Vector3, TagCompound>
{
	public override TagCompound Serialize(Vector3 value)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		return new TagCompound
		{
			["x"] = value.X,
			["y"] = value.Y,
			["z"] = value.Z
		};
	}

	public override Vector3 Deserialize(TagCompound tag)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		return new Vector3(tag.GetFloat("x"), tag.GetFloat("y"), tag.GetFloat("z"));
	}
}
