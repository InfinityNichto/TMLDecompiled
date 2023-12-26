using System;
using System.Collections.Generic;
using Terraria.ModLoader.IO;

namespace Terraria.ModLoader.Default;

public class UnloadedGlobalItem : GlobalItem
{
	[CloneByReference]
	internal IList<TagCompound> data = new List<TagCompound>();

	public override bool InstancePerEntity => true;

	public override void SaveData(Item item, TagCompound tag)
	{
		throw new NotSupportedException("UnloadedGlobalItem data is meant to be flattened and saved transparently via ItemIO");
	}

	public override void LoadData(Item item, TagCompound tag)
	{
		if (tag.ContainsKey("modData"))
		{
			ItemIO.LoadGlobals(item, tag.GetList<TagCompound>("modData"));
		}
	}
}
