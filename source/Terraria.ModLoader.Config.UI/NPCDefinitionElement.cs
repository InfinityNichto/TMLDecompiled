using System;
using System.Collections.Generic;
using Terraria.ID;

namespace Terraria.ModLoader.Config.UI;

internal class NPCDefinitionElement : DefinitionElement<NPCDefinition>
{
	protected override DefinitionOptionElement<NPCDefinition> CreateDefinitionOptionElement()
	{
		return new NPCDefinitionOptionElement(Value, 0.5f);
	}

	protected override List<DefinitionOptionElement<NPCDefinition>> CreateDefinitionOptionElementList()
	{
		base.OptionScale = 0.8f;
		List<DefinitionOptionElement<NPCDefinition>> options = new List<DefinitionOptionElement<NPCDefinition>>();
		for (int i = 0; i < NPCLoader.NPCCount; i++)
		{
			NPCDefinitionOptionElement optionElement = new NPCDefinitionOptionElement(new NPCDefinition(i), base.OptionScale);
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

	protected override List<DefinitionOptionElement<NPCDefinition>> GetPassedOptionElements()
	{
		List<DefinitionOptionElement<NPCDefinition>> passed = new List<DefinitionOptionElement<NPCDefinition>>();
		foreach (DefinitionOptionElement<NPCDefinition> option in base.Options)
		{
			if (Lang.GetNPCName(option.Type).Value.IndexOf(base.ChooserFilter.CurrentString, StringComparison.OrdinalIgnoreCase) != -1)
			{
				string modname = option.Definition.Mod;
				if (option.Type >= NPCID.Count)
				{
					modname = NPCLoader.GetNPC(option.Type).Mod.DisplayName;
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
