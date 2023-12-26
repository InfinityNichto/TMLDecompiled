using System.Collections.Generic;

namespace Terraria.GameContent.ItemDropRules;

/// <summary>
/// Runs the provided rules in order, stopping after a rule succeeds.<br />
/// Does not use player luck.<br />
/// </summary>
public class SequentialRulesNotScalingWithLuckRule : IItemDropRule, INestedItemDropRule
{
	public IItemDropRule[] rules;

	public int chanceDenominator;

	public int chanceNumerator;

	public List<IItemDropRuleChainAttempt> ChainedRules { get; private set; }

	public SequentialRulesNotScalingWithLuckRule(int chanceDenominator, params IItemDropRule[] rules)
	{
		this.chanceDenominator = chanceDenominator;
		chanceNumerator = 1;
		this.rules = rules;
		ChainedRules = new List<IItemDropRuleChainAttempt>();
	}

	public SequentialRulesNotScalingWithLuckRule(int chanceDenominator, int chanceNumerator, params IItemDropRule[] rules)
	{
		this.chanceDenominator = chanceDenominator;
		this.chanceNumerator = chanceNumerator;
		this.rules = rules;
		ChainedRules = new List<IItemDropRuleChainAttempt>();
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
		ItemDropAttemptResult result = default(ItemDropAttemptResult);
		if (info.rng.Next(chanceDenominator) < chanceNumerator)
		{
			for (int i = 0; i < rules.Length; i++)
			{
				IItemDropRule rule = rules[i];
				result = resolveAction(rule, info);
				if (result.State == ItemDropAttemptResultState.Success)
				{
					return result;
				}
			}
		}
		result.State = ItemDropAttemptResultState.FailedRandomRoll;
		return result;
	}

	public void ReportDroprates(List<DropRateInfo> drops, DropRateInfoChainFeed ratesInfo)
	{
		for (int j = rules.Length - 1; j >= 1; j--)
		{
			rules[j - 1].OnFailedRoll(rules[j]);
		}
		float selfChance = (float)chanceNumerator / (float)chanceDenominator;
		rules[0].ReportDroprates(drops, ratesInfo.With(selfChance));
		Chains.ReportDroprates(ChainedRules, selfChance, drops, ratesInfo);
		for (int i = 0; i < rules.Length - 1; i++)
		{
			rules[i].ChainedRules.RemoveAt(rules[i].ChainedRules.Count - 1);
		}
	}
}
