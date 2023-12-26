using System;
using System.Collections.Generic;
using Terraria.ID;

namespace Terraria.ModLoader.Config.UI;

internal class ProjectileDefinitionElement : DefinitionElement<ProjectileDefinition>
{
	protected override DefinitionOptionElement<ProjectileDefinition> CreateDefinitionOptionElement()
	{
		return new ProjectileDefinitionOptionElement(Value, 0.5f);
	}

	protected override List<DefinitionOptionElement<ProjectileDefinition>> CreateDefinitionOptionElementList()
	{
		List<DefinitionOptionElement<ProjectileDefinition>> options = new List<DefinitionOptionElement<ProjectileDefinition>>();
		for (int i = 0; i < ProjectileLoader.ProjectileCount; i++)
		{
			ProjectileDefinitionOptionElement optionElement = new ProjectileDefinitionOptionElement(new ProjectileDefinition(i), base.OptionScale);
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

	protected override List<DefinitionOptionElement<ProjectileDefinition>> GetPassedOptionElements()
	{
		List<DefinitionOptionElement<ProjectileDefinition>> passed = new List<DefinitionOptionElement<ProjectileDefinition>>();
		foreach (DefinitionOptionElement<ProjectileDefinition> option in base.Options)
		{
			if (Lang.GetProjectileName(option.Type).Value.IndexOf(base.ChooserFilter.CurrentString, StringComparison.OrdinalIgnoreCase) != -1)
			{
				string modname = option.Definition.Mod;
				if (option.Type >= ProjectileID.Count)
				{
					modname = ProjectileLoader.GetProjectile(option.Type).Mod.DisplayName;
				}
				if (modname.Contains(base.ChooserFilterMod.CurrentString, StringComparison.OrdinalIgnoreCase))
				{
					passed.Add(option);
				}
			}
		}
		return passed;
	}
}
