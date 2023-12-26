using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.Localization;

namespace Terraria.ModLoader.Default.Patreon;

internal abstract class PatreonItem : ModLoaderModItem
{
	public string InternalSetName { get; set; }

	public string SetSuffix { get; set; }

	public override LocalizedText Tooltip => LocalizedText.Empty;

	public PatreonItem()
	{
		InternalSetName = GetType().Name;
		int lastUnderscoreIndex = InternalSetName.LastIndexOf('_');
		if (lastUnderscoreIndex != -1)
		{
			InternalSetName = InternalSetName.Substring(0, lastUnderscoreIndex);
		}
		SetSuffix = (InternalSetName.EndsWith('s') ? "'" : "'s");
	}

	public override void SetStaticDefaults()
	{
	}

	public override void SetDefaults()
	{
		base.Item.rare = 9;
		base.Item.vanity = true;
	}

	public override void ModifyTooltips(List<TooltipLine> tooltips)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		TooltipLine line = new TooltipLine(base.Mod, "PatreonThanks", Language.GetTextValue("tModLoader.PatreonSetTooltip"))
		{
			OverrideColor = Color.Aquamarine
		};
		tooltips.Add(line);
	}
}
