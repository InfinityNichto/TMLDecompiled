using System;
using System.Collections.Generic;
using Terraria.ModLoader.IO;

namespace Terraria.ModLoader.Default;

public class UnloadedGlobalNPC : GlobalNPC
{
	internal IList<TagCompound> data = new List<TagCompound>();

	public override bool InstancePerEntity => true;

	public override bool NeedSaving(NPC npc)
	{
		return data.Count > 0;
	}

	public override void SaveData(NPC npc, TagCompound tag)
	{
		throw new NotSupportedException("UnloadedGlobalNPC data is meant to be flattened and saved transparently via ItemIO");
	}

	public override void LoadData(NPC npc, TagCompound tag)
	{
	}
}
