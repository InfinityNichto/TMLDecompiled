using System.Collections.Generic;
using Terraria.ID;
using Terraria.ModLoader;

namespace Terraria.GameContent.ItemDropRules;

public class SlimeBodyItemDropRule : IItemDropRule
{
	public List<IItemDropRuleChainAttempt> ChainedRules { get; private set; }

	public SlimeBodyItemDropRule()
	{
		ChainedRules = new List<IItemDropRuleChainAttempt>();
	}

	public bool CanDrop(DropAttemptInfo info)
	{
		if (info.npc.type == 1 && info.npc.ai[1] > 0f)
		{
			return info.npc.ai[1] < (float)ItemLoader.ItemCount;
		}
		return false;
	}

	public ItemDropAttemptResult TryDroppingItem(DropAttemptInfo info)
	{
		int itemId = (int)info.npc.ai[1];
		GetDropInfo(itemId, out var amountDroppedMinimum, out var amountDroppedMaximum);
		CommonCode.DropItem(info, itemId, info.rng.Next(amountDroppedMinimum, amountDroppedMaximum + 1));
		ItemDropAttemptResult result = default(ItemDropAttemptResult);
		result.State = ItemDropAttemptResultState.Success;
		return result;
	}

	public void GetDropInfo(int itemId, out int amountDroppedMinimum, out int amountDroppedMaximum)
	{
		amountDroppedMinimum = 1;
		amountDroppedMaximum = 1;
		switch (itemId)
		{
		case 8:
			amountDroppedMinimum = 5;
			amountDroppedMaximum = 10;
			break;
		case 166:
			amountDroppedMinimum = 2;
			amountDroppedMaximum = 6;
			break;
		case 965:
			amountDroppedMinimum = 20;
			amountDroppedMaximum = 45;
			break;
		case 71:
			amountDroppedMinimum = 50;
			amountDroppedMaximum = 99;
			break;
		case 72:
			amountDroppedMinimum = 20;
			amountDroppedMaximum = 99;
			break;
		case 73:
			amountDroppedMinimum = 1;
			amountDroppedMaximum = 2;
			break;
		case 4343:
		case 4344:
			amountDroppedMinimum = 2;
			amountDroppedMaximum = 5;
			break;
		}
		if (ItemID.Sets.OreDropsFromSlime.TryGetValue(itemId, out (int, int) minMaxStack))
		{
			(amountDroppedMinimum, amountDroppedMaximum) = minMaxStack;
		}
	}

	public void ReportDroprates(List<DropRateInfo> drops, DropRateInfoChainFeed ratesInfo)
	{
		Chains.ReportDroprates(ChainedRules, 1f, drops, ratesInfo);
	}
}
