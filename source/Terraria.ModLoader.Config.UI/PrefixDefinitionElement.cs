using System;
using System.Collections.Generic;
using Terraria.ID;

namespace Terraria.ModLoader.Config.UI;

internal class PrefixDefinitionElement : DefinitionElement<PrefixDefinition>
{
	protected override DefinitionOptionElement<PrefixDefinition> CreateDefinitionOptionElement()
	{
		return new PrefixDefinitionOptionElement(Value, 0.8f);
	}

	protected override void TweakDefinitionOptionElement(DefinitionOptionElement<PrefixDefinition> optionElement)
	{
		optionElement.Top.Set(0f, 0f);
		optionElement.Left.Set(-124f, 1f);
	}

	protected override List<DefinitionOptionElement<PrefixDefinition>> CreateDefinitionOptionElementList()
	{
		base.OptionScale = 0.8f;
		List<DefinitionOptionElement<PrefixDefinition>> options = new List<DefinitionOptionElement<PrefixDefinition>>();
		for (int i = 0; i < PrefixLoader.PrefixCount; i++)
		{
			PrefixDefinitionOptionElement optionElement;
			if (i == 0)
			{
				optionElement = new PrefixDefinitionOptionElement(new PrefixDefinition("Terraria", "None"), base.OptionScale);
			}
			else
			{
				optionElement = new PrefixDefinitionOptionElement(new PrefixDefinition(i), base.OptionScale);
			}
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

	protected override List<DefinitionOptionElement<PrefixDefinition>> GetPassedOptionElements()
	{
		List<DefinitionOptionElement<PrefixDefinition>> passed = new List<DefinitionOptionElement<PrefixDefinition>>();
		foreach (DefinitionOptionElement<PrefixDefinition> option in base.Options)
		{
			if (option.Definition.DisplayName.IndexOf(base.ChooserFilter.CurrentString, StringComparison.OrdinalIgnoreCase) != -1)
			{
				string modname = option.Definition.Mod;
				if (option.Type >= PrefixID.Count)
				{
					modname = PrefixLoader.GetPrefix(option.Type).Mod.DisplayName;
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
