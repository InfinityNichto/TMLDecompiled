using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.GameContent;
using Terraria.UI;
using Terraria.UI.Chat;

namespace Terraria.ModLoader.Config.UI;

internal class BooleanElement : ConfigElement<bool>
{
	private Asset<Texture2D> _toggleTexture;

	public override void OnBind()
	{
		base.OnBind();
		_toggleTexture = Main.Assets.Request<Texture2D>("Images/UI/Settings_Toggle");
		base.OnLeftClick += delegate
		{
			Value = !Value;
		};
	}

	protected override void DrawSelf(SpriteBatch spriteBatch)
	{
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		base.DrawSelf(spriteBatch);
		CalculatedStyle dimensions = GetDimensions();
		ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.ItemStack.Value, Value ? Lang.menu[126].Value : Lang.menu[124].Value, new Vector2(dimensions.X + dimensions.Width - 60f, dimensions.Y + 8f), Color.White, 0f, Vector2.Zero, new Vector2(0.8f));
		Rectangle sourceRectangle = default(Rectangle);
		((Rectangle)(ref sourceRectangle))._002Ector(Value ? ((_toggleTexture.Width() - 2) / 2 + 2) : 0, 0, (_toggleTexture.Width() - 2) / 2, _toggleTexture.Height());
		Vector2 drawPosition = default(Vector2);
		((Vector2)(ref drawPosition))._002Ector(dimensions.X + dimensions.Width - (float)sourceRectangle.Width - 10f, dimensions.Y + 8f);
		spriteBatch.Draw(_toggleTexture.Value, drawPosition, (Rectangle?)sourceRectangle, Color.White, 0f, Vector2.Zero, Vector2.One, (SpriteEffects)0, 0f);
	}
}
