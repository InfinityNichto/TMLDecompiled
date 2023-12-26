using System;
using System.Collections.Generic;

namespace Terraria.GameContent.ItemDropRules;

public class OneFromRulesRule : IItemDropRule, INestedItemDropRule
{
	public IItemDropRule[] options;

	public int chanceDenominator;

	public int chanceNumerator;

	public List<IItemDropRuleChainAttempt> ChainedRules { get; private set; }

	public OneFromRulesRule(int chanceDenominator, params IItemDropRule[] options)
		: this(chanceDenominator, 1, options)
	{
	}

	public OneFromRulesRule(int chanceDenominator, int chanceNumerator, params IItemDropRule[] options)
	{
		this.chanceDenominator = chanceDenominator;
		this.chanceNumerator = chanceNumerator;
		this.options = options;
		ChainedRules = new List<IItemDropRuleChainAttempt>();
		if (chanceNumerator > chanceDenominator)
		{
			throw new ArgumentOutOfRangeException("chanceNumerator", "chanceNumerator must be lesser or equal to chanceDenominator.");
		}
	}

	public bool CanDrop(DropAttemptInfo info)
	{
		return true;
	}

	public ItemDropAttemptResult TryDroppingItem(DropAttemptInfo info)
	{
		ItemDropAttemptResult result = default(ItemDropAttemptResult);
		result.State = ItemDropAttemptResultState.DidNotRunCode;
		return result;
	}

	public ItemDropAttemptResult TryDroppingItem(DropAttemptInfo info, ItemDropRuleResolveAction resolveAction)
	{
		int num = -1;
		ItemDropAttemptResult result;
		if (info.rng.Next(chanceDenominator) < chanceNumerator)
		{
			num = info.rng.Next(options.Length);
			resolveAction(options[num], info);
			result = default(ItemDropAttemptResult);
			result.State = ItemDropAttemptResultState.Success;
			return result;
		}
		result = default(ItemDropAttemptResult);
		result.State = ItemDropAttemptResultState.FailedRandomRoll;
		return result;
	}

	public void ReportDroprates(List<DropRateInfo> drops, DropRateInfoChainFeed ratesInfo)
	{
		float num = (float)chanceNumerator / (float)chanceDenominator;
		float multiplier = 1f / (float)options.Length * num;
		for (int i = 0; i < options.Length; i++)
		{
			options[i].ReportDroprates(drops, ratesInfo.With(multiplier));
		}
		Chains.ReportDroprates(ChainedRules, num, drops, ratesInfo);
	}
}
