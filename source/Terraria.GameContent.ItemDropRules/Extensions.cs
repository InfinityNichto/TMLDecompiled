using Terraria.Localization;

namespace Terraria.GameContent.ItemDropRules;

public static class Extensions
{
	public static SimpleItemDropRuleCondition ToDropCondition(this Condition condition, ShowItemDropInUI showItemDropInUI, bool showConditionInUI = true)
	{
		return new SimpleItemDropRuleCondition(Language.GetText("Bestiary_ItemDropConditions.SimpleCondition").WithFormatArgs(condition.Description), condition.Predicate, showItemDropInUI, showConditionInUI);
	}
}
