using System.Collections.Generic;

namespace Terraria.GameContent.ItemDropRules;

public class CoinsRule : IItemDropRule
{
	public long value;

	public bool withRandomBonus;

	public List<IItemDropRuleChainAttempt> ChainedRules { get; private set; } = new List<IItemDropRuleChainAttempt>();


	public CoinsRule(long value, bool withRandomBonus = false)
	{
		this.value = value;
		this.withRandomBonus = withRandomBonus;
	}

	public bool CanDrop(DropAttemptInfo info)
	{
		return true;
	}

	public ItemDropAttemptResult TryDroppingItem(DropAttemptInfo info)
	{
		double scale = 1.0;
		if (withRandomBonus)
		{
			scale += (double)((float)info.rng.Next(-20, 21) * 0.01f);
			if (info.rng.Next(5) == 0)
			{
				scale += (double)((float)info.rng.Next(5, 11) * 0.01f);
			}
			if (info.rng.Next(10) == 0)
			{
				scale += (double)((float)info.rng.Next(10, 21) * 0.01f);
			}
			if (info.rng.Next(15) == 0)
			{
				scale += (double)((float)info.rng.Next(15, 31) * 0.01f);
			}
			if (info.rng.Next(20) == 0)
			{
				scale += (double)((float)info.rng.Next(20, 41) * 0.01f);
			}
		}
		foreach (var (itemId, count) in ToCoins((long)((double)value * scale)))
		{
			CommonCode.DropItem(info, itemId, count);
		}
		ItemDropAttemptResult result = default(ItemDropAttemptResult);
		result.State = ItemDropAttemptResultState.Success;
		return result;
	}

	public static IEnumerable<(int itemId, int count)> ToCoins(long money)
	{
		int copper = (int)(money % 100);
		money /= 100;
		int silver = (int)(money % 100);
		money /= 100;
		int gold = (int)(money % 100);
		money /= 100;
		int plat = (int)money;
		if (copper > 0)
		{
			yield return (itemId: 71, count: copper);
		}
		if (silver > 0)
		{
			yield return (itemId: 72, count: silver);
		}
		if (gold > 0)
		{
			yield return (itemId: 73, count: gold);
		}
		if (plat > 0)
		{
			yield return (itemId: 74, count: plat);
		}
	}

	public void ReportDroprates(List<DropRateInfo> drops, DropRateInfoChainFeed ratesInfo)
	{
		foreach (var (itemId, count) in ToCoins(value))
		{
			drops.Add(new DropRateInfo(itemId, count, count, ratesInfo.parentDroprateChance, ratesInfo.conditions));
		}
		Chains.ReportDroprates(ChainedRules, 1f, drops, ratesInfo);
	}
}
