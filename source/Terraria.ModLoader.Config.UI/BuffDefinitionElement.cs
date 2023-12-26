using System;
using System.Collections.Generic;
using Terraria.ID;

namespace Terraria.ModLoader.Config.UI;

internal class BuffDefinitionElement : DefinitionElement<BuffDefinition>
{
	protected override DefinitionOptionElement<BuffDefinition> CreateDefinitionOptionElement()
	{
		return new BuffDefinitionOptionElement(Value);
	}

	protected override List<DefinitionOptionElement<BuffDefinition>> CreateDefinitionOptionElementList()
	{
		List<DefinitionOptionElement<BuffDefinition>> options = new List<DefinitionOptionElement<BuffDefinition>>();
		for (int i = 0; i < BuffLoader.BuffCount; i++)
		{
			BuffDefinition buffDefinition = ((i == 0) ? new BuffDefinition() : new BuffDefinition(i));
			BuffDefinitionOptionElement optionElement = new BuffDefinitionOptionElement(buffDefinition, base.OptionScale);
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

	protected override List<DefinitionOptionElement<BuffDefinition>> GetPassedOptionElements()
	{
		List<DefinitionOptionElement<BuffDefinition>> passed = new List<DefinitionOptionElement<BuffDefinition>>();
		foreach (DefinitionOptionElement<BuffDefinition> option in base.Options)
		{
			if (Lang.GetBuffName(option.Type).Contains(base.ChooserFilter.CurrentString, StringComparison.OrdinalIgnoreCase))
			{
				string modname = "Terraria";
				if (option.Type >= BuffID.Count)
				{
					modname = BuffLoader.GetBuff(option.Type).Mod.DisplayName;
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
