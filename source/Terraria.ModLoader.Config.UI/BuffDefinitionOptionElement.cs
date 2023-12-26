using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.ModLoader.Default;
using Terraria.ModLoader.UI;
using Terraria.UI;

namespace Terraria.ModLoader.Config.UI;

internal class BuffDefinitionOptionElement : DefinitionOptionElement<BuffDefinition>
{
	public BuffDefinitionOptionElement(BuffDefinition definition, float scale = 0.5f)
		: base(definition, scale)
	{
	}

	public override void SetItem(BuffDefinition definition)
	{
		base.SetItem(definition);
	}

	protected override void DrawSelf(SpriteBatch spriteBatch)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Unknown result type (might be due to invalid IL or missing references)
		//IL_0165: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		//IL_0169: Unknown result type (might be due to invalid IL or missing references)
		//IL_0173: Unknown result type (might be due to invalid IL or missing references)
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		//IL_017c: Unknown result type (might be due to invalid IL or missing references)
		//IL_017e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0185: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		CalculatedStyle dimensions = GetInnerDimensions();
		spriteBatch.Draw(base.BackgroundTexture.Value, dimensions.Position(), (Rectangle?)null, Color.White, 0f, Vector2.Zero, base.Scale, (SpriteEffects)0, 0f);
		if (base.Definition != null)
		{
			int type = ((!base.Unloaded) ? base.Type : 0);
			Texture2D buffTexture = ((type != 0) ? TextureAssets.Buff[type].Value : TextureAssets.Item[0].Value);
			int num = Interface.modConfig.UpdateCount / 4;
			int frames = 1;
			if (base.Unloaded)
			{
				buffTexture = TextureAssets.Item[ModContent.ItemType<UnloadedItem>()].Value;
				frames = 1;
			}
			int height = buffTexture.Height / frames;
			int width = buffTexture.Width;
			int frame = num % frames;
			int y = height * frame;
			Rectangle rectangle2 = default(Rectangle);
			((Rectangle)(ref rectangle2))._002Ector(0, y, width, height);
			float drawScale = 1f;
			float availableWidth = (float)DefinitionOptionElement<BuffDefinition>.DefaultBackgroundTexture.Width() * base.Scale;
			if ((float)width > availableWidth || (float)height > availableWidth)
			{
				drawScale = ((width <= height) ? (availableWidth / (float)height) : (availableWidth / (float)width));
			}
			Vector2 vector = base.BackgroundTexture.Size() * base.Scale;
			Vector2 position2 = dimensions.Position() + vector / 2f - rectangle2.Size() * drawScale / 2f;
			Vector2 origin = rectangle2.Size() * 0f;
			spriteBatch.Draw(buffTexture, position2, (Rectangle?)rectangle2, Color.White, 0f, origin, drawScale, (SpriteEffects)0, 0f);
		}
		if (base.IsMouseHovering)
		{
			UIModConfig.Tooltip = base.Tooltip;
		}
	}
}
