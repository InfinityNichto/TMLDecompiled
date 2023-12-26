using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Terraria.ModLoader.Default.Developer;

internal abstract class DeveloperItem : ModLoaderModItem
{
	public virtual string TooltipBrief { get; }

	public virtual string SetSuffix => "'s";

	public string InternalSetName => GetType().Name.Split('_')[0];

	public override void SetStaticDefaults()
	{
	}

	public override void SetDefaults()
	{
		base.Item.rare = 11;
		base.Item.vanity = true;
	}

	public override void ModifyTooltips(List<TooltipLine> tooltips)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		TooltipLine line = new TooltipLine(base.Mod, "DeveloperSetNote", TooltipBrief + "Developer Item")
		{
			OverrideColor = Color.OrangeRed
		};
		tooltips.Add(line);
	}
}
