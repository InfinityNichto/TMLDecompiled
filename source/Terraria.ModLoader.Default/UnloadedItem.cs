using System.Collections.Generic;
using System.IO;
using Terraria.Localization;
using Terraria.ModLoader.IO;

namespace Terraria.ModLoader.Default;

[LegacyName(new string[] { "MysteryItem" })]
public sealed class UnloadedItem : ModLoaderModItem
{
	[CloneByReference]
	private TagCompound data;

	public string ModName { get; private set; }

	public string ItemName { get; private set; }

	public override void SetDefaults()
	{
		base.Item.width = 20;
		base.Item.height = 20;
		base.Item.rare = 1;
		base.Item.maxStack = int.MaxValue;
	}

	public override void SetStaticDefaults()
	{
		base.Item.ResearchUnlockCount = 0;
	}

	internal void Setup(TagCompound tag)
	{
		ModName = tag.GetString("mod");
		ItemName = tag.GetString("name");
		data = tag;
	}

	public override void ModifyTooltips(List<TooltipLine> tooltips)
	{
		for (int i = 0; i < tooltips.Count; i++)
		{
			if (tooltips[i].Name == "Tooltip0")
			{
				tooltips[i].Text = Language.GetTextValue(this.GetLocalizationKey("UnloadedItemModTooltip"), ModName);
			}
			else if (tooltips[i].Name == "Tooltip1")
			{
				tooltips[i].Text = Language.GetTextValue(this.GetLocalizationKey("UnloadedItemItemNameTooltip"), ItemName);
			}
		}
	}

	public override bool CanStack(Item source)
	{
		return false;
	}

	public override void SaveData(TagCompound tag)
	{
		foreach (KeyValuePair<string, object> datum in data)
		{
			datum.Deconstruct(out var key2, out var value2);
			string key = key2;
			object value = value2;
			tag[key] = value;
		}
	}

	public override void LoadData(TagCompound tag)
	{
		Setup(tag);
		if (!ModContent.TryFind<ModItem>(ModName, ItemName, out var modItem))
		{
			return;
		}
		if (modItem is UnloadedItem)
		{
			LoadData(tag.GetCompound("data"));
			return;
		}
		TagCompound modData = tag.GetCompound("data");
		base.Item.SetDefaults(modItem.Type);
		if (modData != null && modData.Count > 0)
		{
			base.Item.ModItem.LoadData(modData);
		}
		if (tag.ContainsKey("globalData"))
		{
			ItemIO.LoadGlobals(base.Item, tag.GetList<TagCompound>("globalData"));
		}
	}

	public override void NetSend(BinaryWriter writer)
	{
		TagIO.Write(data ?? new TagCompound(), writer);
	}

	public override void NetReceive(BinaryReader reader)
	{
		Setup(TagIO.Read(reader));
	}
}
