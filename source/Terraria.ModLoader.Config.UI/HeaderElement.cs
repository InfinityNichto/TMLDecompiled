using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.UI;
using Terraria.UI.Chat;

namespace Terraria.ModLoader.Config.UI;

internal class HeaderElement : UIElement
{
	private readonly string header;

	public HeaderElement(string header)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		this.header = header;
		Vector2 size = ChatManager.GetStringSize(FontAssets.ItemStack.Value, this.header, Vector2.One, 532f);
		Width.Set(0f, 1f);
		Height.Set(size.Y + 6f, 0f);
	}

	protected override void DrawSelf(SpriteBatch spriteBatch)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		base.DrawSelf(spriteBatch);
		CalculatedStyle dimensions = GetDimensions();
		float settingsWidth = dimensions.Width + 1f;
		Vector2 position = new Vector2(dimensions.X, dimensions.Y) + new Vector2(8f);
		spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle((int)dimensions.X + 10, (int)dimensions.Y + (int)dimensions.Height - 2, (int)dimensions.Width - 20, 1), Color.LightGray);
		ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.ItemStack.Value, header, position, Color.White, 0f, Vector2.Zero, new Vector2(1f), settingsWidth - 20f);
	}
}
