using System;
using System.Collections.Generic;

namespace Terraria.GameContent.ItemDropRules;

/// <summary>
/// Re-runs all drop rules if none succeded.
/// </summary>
public class AlwaysAtleastOneSuccessDropRule : IItemDropRule, INestedItemDropRule
{
	private class PersonalDropRateReportingRule : IItemDropRuleChainAttempt
	{
		private readonly Action<float> report;

		public IItemDropRule RuleToChain
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public PersonalDropRateReportingRule(Action<float> report)
		{
			this.report = report;
		}

		public bool CanChainIntoRule(ItemDropAttemptResult parentResult)
		{
			throw new NotImplementedException();
		}

		public void ReportDroprates(float personalDropRate, List<DropRateInfo> drops, DropRateInfoChainFeed ratesInfo)
		{
			report(personalDropRate);
		}
	}

	public IItemDropRule[] rules;

	public List<IItemDropRuleChainAttempt> ChainedRules { get; private set; }

	public AlwaysAtleastOneSuccessDropRule(params IItemDropRule[] rules)
	{
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
		bool anyDropped;
		do
		{
			anyDropped = false;
			IItemDropRule[] array = rules;
			foreach (IItemDropRule rule in array)
			{
				if (resolveAction(rule, info).State == ItemDropAttemptResultState.Success)
				{
					anyDropped = true;
				}
			}
		}
		while (!anyDropped);
		ItemDropAttemptResult result = default(ItemDropAttemptResult);
		result.State = ItemDropAttemptResultState.Success;
		return result;
	}

	public void ReportDroprates(List<DropRateInfo> drops, DropRateInfoChainFeed ratesInfo)
	{
		float reroll = 1f;
		IItemDropRule[] array = rules;
		foreach (IItemDropRule rule in array)
		{
			reroll *= 1f - GetPersonalDropRate(rule);
		}
		float scale = ((reroll == 1f) ? 1f : (1f / (1f - reroll)));
		array = rules;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].ReportDroprates(drops, ratesInfo.With(scale));
		}
		Chains.ReportDroprates(ChainedRules, 1f, drops, ratesInfo);
	}

	public static float GetPersonalDropRate(IItemDropRule rule)
	{
		IItemDropRuleChainAttempt[] chained = rule.ChainedRules.ToArray();
		rule.ChainedRules.Clear();
		float dropRate = 0f;
		rule.ChainedRules.Add(new PersonalDropRateReportingRule(delegate(float f)
		{
			dropRate = f;
		}));
		rule.ReportDroprates(new List<DropRateInfo>(), new DropRateInfoChainFeed(1f));
		rule.ChainedRules.Clear();
		rule.ChainedRules.AddRange(chained);
		return dropRate;
	}
}
