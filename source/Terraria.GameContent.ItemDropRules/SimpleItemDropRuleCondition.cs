using System;
using Terraria.Localization;

namespace Terraria.GameContent.ItemDropRules;

public record SimpleItemDropRuleCondition(LocalizedText Description, Func<bool> Predicate, ShowItemDropInUI ShowItemDropInUI, bool ShowConditionInUI = true) : IItemDropRuleCondition, IProvideItemConditionDescription
{
	public bool CanDrop(DropAttemptInfo info)
	{
		return Predicate();
	}

	public bool CanShowItemDropInUI()
	{
		return ShowItemDropInUI switch
		{
			ShowItemDropInUI.Always => true, 
			ShowItemDropInUI.Never => false, 
			ShowItemDropInUI.WhenConditionSatisfied => Predicate(), 
			_ => throw new NotImplementedException(), 
		};
	}

	public string? GetConditionDescription()
	{
		if (!ShowConditionInUI)
		{
			return null;
		}
		return Description.Value;
	}
}
