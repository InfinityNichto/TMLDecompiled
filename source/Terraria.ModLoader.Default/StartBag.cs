using System.Collections.Generic;
using Terraria.DataStructures;
using Terraria.ModLoader.IO;

namespace Terraria.ModLoader.Default;

public class StartBag : ModLoaderModItem
{
	[CloneByReference]
	private List<Item> items = new List<Item>();

	public override void SetDefaults()
	{
		base.Item.width = 20;
		base.Item.height = 20;
		base.Item.rare = 1;
	}

	internal void AddItem(Item item)
	{
		items.Add(item);
	}

	public override bool CanRightClick()
	{
		return true;
	}

	public override void RightClick(Player player)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		IEntitySource itemSource = player.GetItemSource_OpenItem(base.Type);
		foreach (Item item in items)
		{
			int i = Item.NewItem(itemSource, player.getRect(), item.type, item.stack, noBroadcast: false, item.prefix);
			if (Main.netMode == 1)
			{
				NetMessage.SendData(21, -1, -1, null, i, 1f);
			}
		}
	}

	public override void SaveData(TagCompound tag)
	{
		tag["items"] = items;
	}

	public override void LoadData(TagCompound tag)
	{
		items = tag.Get<List<Item>>("items");
	}
}
