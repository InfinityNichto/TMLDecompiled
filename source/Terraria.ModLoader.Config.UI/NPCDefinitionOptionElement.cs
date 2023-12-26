using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.ModLoader.Default;
using Terraria.ModLoader.UI;
using Terraria.UI;

namespace Terraria.ModLoader.Config.UI;

internal class NPCDefinitionOptionElement : DefinitionOptionElement<NPCDefinition>
{
	public NPCDefinitionOptionElement(NPCDefinition definition, float scale = 0.75f)
		: base(definition, scale)
	{
	}

	protected override void DrawSelf(SpriteBatch spriteBatch)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0165: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Unknown result type (might be due to invalid IL or missing references)
		//IL_0173: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0182: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		CalculatedStyle dimensions = GetInnerDimensions();
		spriteBatch.Draw(base.BackgroundTexture.Value, dimensions.Position(), (Rectangle?)null, Color.White, 0f, Vector2.Zero, base.Scale, (SpriteEffects)0, 0f);
		if (base.Definition != null)
		{
			int type = ((!base.Unloaded) ? base.Type : 0);
			Main.instance.LoadNPC(type);
			Texture2D npcTexture = TextureAssets.Npc[type].Value;
			int num = Interface.modConfig.UpdateCount / 8;
			int frames = Main.npcFrameCount[type];
			if (base.Unloaded)
			{
				npcTexture = TextureAssets.Item[ModContent.ItemType<UnloadedItem>()].Value;
				frames = 1;
			}
			int height = npcTexture.Height / frames;
			int width = npcTexture.Width;
			int frame = num % frames;
			int y = height * frame;
			Rectangle rectangle2 = default(Rectangle);
			((Rectangle)(ref rectangle2))._002Ector(0, y, width, height);
			float drawScale = 1f;
			float availableWidth = (float)DefinitionOptionElement<NPCDefinition>.DefaultBackgroundTexture.Width() * base.Scale;
			if ((float)width > availableWidth || (float)height > availableWidth)
			{
				drawScale = ((width <= height) ? (availableWidth / (float)height) : (availableWidth / (float)width));
			}
			drawScale *= base.Scale;
			Vector2 vector = base.BackgroundTexture.Size() * base.Scale;
			Vector2 position2 = dimensions.Position() + vector / 2f - rectangle2.Size() * drawScale / 2f;
			Vector2 origin = rectangle2.Size() * 0f;
			spriteBatch.Draw(npcTexture, position2, (Rectangle?)rectangle2, Color.White, 0f, origin, drawScale, (SpriteEffects)0, 0f);
		}
		if (base.IsMouseHovering)
		{
			UIModConfig.Tooltip = base.Tooltip;
		}
	}
}
