using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader.UI;

namespace Terraria.ModLoader.Config.UI;

internal class PrefixDefinitionOptionElement : DefinitionOptionElement<PrefixDefinition>
{
	private readonly UIAutoScaleTextTextPanel<string> text;

	public PrefixDefinitionOptionElement(PrefixDefinition definition, float scale = 0.75f)
		: base(definition, scale)
	{
		Width.Set(150f * scale, 0f);
		Height.Set(40f * scale, 0f);
		text = new UIAutoScaleTextTextPanel<string>(base.Definition.DisplayName)
		{
			Width = 
			{
				Percent = 1f
			},
			Height = 
			{
				Percent = 1f
			}
		};
		Append(text);
	}

	public override void SetItem(PrefixDefinition item)
	{
		base.SetItem(item);
		text?.SetText(item.DisplayName);
	}

	public override void SetScale(float scale)
	{
		base.SetScale(scale);
		Width.Set(150f * scale, 0f);
		Height.Set(40f * scale, 0f);
	}

	protected override void DrawSelf(SpriteBatch spriteBatch)
	{
		if (base.IsMouseHovering)
		{
			UIModConfig.Tooltip = base.Tooltip;
		}
	}
}
