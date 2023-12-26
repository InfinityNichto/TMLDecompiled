using System;
using Terraria.ModLoader.Default;

namespace Terraria.ModLoader.IO;

internal class WallEntry : ModEntry
{
	public static Func<TagCompound, WallEntry> DESERIALIZER = (TagCompound tag) => new WallEntry(tag);

	public override string DefaultUnloadedType => ModContent.GetInstance<UnloadedWall>().FullName;

	public WallEntry(ModWall wall)
		: base(wall)
	{
	}

	public WallEntry(TagCompound tag)
		: base(tag)
	{
	}

	protected override string GetUnloadedType(ushort type)
	{
		return DefaultUnloadedType;
	}
}
