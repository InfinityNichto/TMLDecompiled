using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader.Default;
using Terraria.UI;

namespace Terraria.ModLoader.Config.UI;

internal class ItemDefinitionOptionElement : DefinitionOptionElement<ItemDefinition>
{
	public Item Item { get; set; }

	public ItemDefinitionOptionElement(ItemDefinition definition, float scale = 0.75f)
		: base(definition, scale)
	{
	}

	public override void SetItem(ItemDefinition definition)
	{
		base.SetItem(definition);
		Item = new Item();
		Item.SetDefaults(base.Type);
	}

	protected override void DrawSelf(SpriteBatch spriteBatch)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0173: Unknown result type (might be due to invalid IL or missing references)
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0190: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_0197: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01db: Unknown result type (might be due to invalid IL or missing references)
		//IL_0268: Unknown result type (might be due to invalid IL or missing references)
		//IL_026a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0272: Unknown result type (might be due to invalid IL or missing references)
		//IL_0274: Unknown result type (might be due to invalid IL or missing references)
		//IL_027f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0284: Unknown result type (might be due to invalid IL or missing references)
		//IL_0289: Unknown result type (might be due to invalid IL or missing references)
		//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0206: Unknown result type (might be due to invalid IL or missing references)
		//IL_021e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0223: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02de: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_0231: Unknown result type (might be due to invalid IL or missing references)
		//IL_0233: Unknown result type (might be due to invalid IL or missing references)
		//IL_0240: Unknown result type (might be due to invalid IL or missing references)
		//IL_0245: Unknown result type (might be due to invalid IL or missing references)
		//IL_024f: Unknown result type (might be due to invalid IL or missing references)
		if (Item != null)
		{
			CalculatedStyle dimensions = GetInnerDimensions();
			spriteBatch.Draw(base.BackgroundTexture.Value, dimensions.Position(), (Rectangle?)null, Color.White, 0f, Vector2.Zero, base.Scale, (SpriteEffects)0, 0f);
			if (!Item.IsAir || base.Unloaded)
			{
				int type = (base.Unloaded ? ModContent.ItemType<UnloadedItem>() : Item.type);
				Main.instance.LoadItem(Item.type);
				Texture2D itemTexture = TextureAssets.Item[type].Value;
				Rectangle rectangle2 = ((Main.itemAnimations[type] == null) ? itemTexture.Frame() : Main.itemAnimations[type].GetFrame(itemTexture));
				Color newColor = Color.White;
				float pulseScale = 1f;
				ItemSlot.GetItemLight(ref newColor, ref pulseScale, Item);
				int height = rectangle2.Height;
				int width = rectangle2.Width;
				float drawScale = 1f;
				float availableWidth = (float)DefinitionOptionElement<ItemDefinition>.DefaultBackgroundTexture.Width() * base.Scale;
				if ((float)width > availableWidth || (float)height > availableWidth)
				{
					drawScale = ((width <= height) ? (availableWidth / (float)height) : (availableWidth / (float)width));
				}
				drawScale *= base.Scale;
				Vector2 vector = base.BackgroundTexture.Size() * base.Scale;
				Vector2 position2 = dimensions.Position() + vector / 2f - rectangle2.Size() * drawScale / 2f;
				Vector2 origin = rectangle2.Size() * (pulseScale / 2f - 0.5f);
				if (ItemLoader.PreDrawInInventory(Item, spriteBatch, position2, rectangle2, Item.GetAlpha(newColor), Item.GetColor(Color.White), origin, drawScale * pulseScale))
				{
					spriteBatch.Draw(itemTexture, position2, (Rectangle?)rectangle2, Item.GetAlpha(newColor), 0f, origin, drawScale * pulseScale, (SpriteEffects)0, 0f);
					if (Item.color != Color.Transparent)
					{
						spriteBatch.Draw(itemTexture, position2, (Rectangle?)rectangle2, Item.GetColor(Color.White), 0f, origin, drawScale * pulseScale, (SpriteEffects)0, 0f);
					}
				}
				ItemLoader.PostDrawInInventory(Item, spriteBatch, position2, rectangle2, Item.GetAlpha(newColor), Item.GetColor(Color.White), origin, drawScale * pulseScale);
				if (ItemID.Sets.TrapSigned[type])
				{
					spriteBatch.Draw(TextureAssets.Wire.Value, dimensions.Position() + new Vector2(40f, 40f) * base.Scale, (Rectangle?)new Rectangle(4, 58, 8, 8), Color.White, 0f, new Vector2(4f), 1f, (SpriteEffects)0, 0f);
				}
			}
		}
		if (base.IsMouseHovering)
		{
			UIModConfig.Tooltip = base.Tooltip;
		}
	}
}
