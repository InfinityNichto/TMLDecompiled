using System;

namespace Terraria.ModLoader.IO;

public class VersionSerializer : TagSerializer<Version, string>
{
	public override string Serialize(Version value)
	{
		return value.ToString();
	}

	public override Version Deserialize(string tag)
	{
		return new Version(tag);
	}
}
