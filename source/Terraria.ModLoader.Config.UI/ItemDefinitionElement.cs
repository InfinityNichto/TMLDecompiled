using System;
using System.Collections.Generic;
using Terraria.ID;

namespace Terraria.ModLoader.Config.UI;

internal class ItemDefinitionElement : DefinitionElement<ItemDefinition>
{
	protected override DefinitionOptionElement<ItemDefinition> CreateDefinitionOptionElement()
	{
		return new ItemDefinitionOptionElement(Value, 0.5f);
	}

	protected override List<DefinitionOptionElement<ItemDefinition>> CreateDefinitionOptionElementList()
	{
		List<DefinitionOptionElement<ItemDefinition>> options = new List<DefinitionOptionElement<ItemDefinition>>();
		for (int i = 0; i < ItemLoader.ItemCount; i++)
		{
			ItemDefinitionOptionElement optionElement = new ItemDefinitionOptionElement(new ItemDefinition(i), base.OptionScale);
			optionElement.OnLeftClick += delegate
			{
				Value = optionElement.Definition;
				base.UpdateNeeded = true;
				base.SelectionExpanded = false;
			};
			options.Add(optionElement);
		}
		return options;
	}

	protected override List<DefinitionOptionElement<ItemDefinition>> GetPassedOptionElements()
	{
		List<DefinitionOptionElement<ItemDefinition>> passed = new List<DefinitionOptionElement<ItemDefinition>>();
		foreach (DefinitionOptionElement<ItemDefinition> option in base.Options)
		{
			if (!ItemID.Sets.Deprecated[option.Type] && Lang.GetItemNameValue(option.Type).Contains(base.ChooserFilter.CurrentString, StringComparison.OrdinalIgnoreCase))
			{
				string modname = "Terraria";
				if (option.Type >= ItemID.Count)
				{
					modname = ItemLoader.GetItem(option.Type).Mod.DisplayName;
				}
				if (modname.IndexOf(base.ChooserFilterMod.CurrentString, StringComparison.OrdinalIgnoreCase) != -1)
				{
					passed.Add(option);
				}
			}
		}
		return passed;
	}
}
