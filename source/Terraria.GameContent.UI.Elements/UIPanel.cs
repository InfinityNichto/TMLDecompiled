using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements;

public class UIPanel : UIElement
{
	private int _cornerSize = 12;

	private int _barSize = 4;

	private Asset<Texture2D> _borderTexture;

	private Asset<Texture2D> _backgroundTexture;

	public Color BorderColor = Color.Black;

	public Color BackgroundColor = new Color(63, 82, 151) * 0.7f;

	private bool _needsTextureLoading;

	private void LoadTextures()
	{
		if (_borderTexture == null)
		{
			_borderTexture = Main.Assets.Request<Texture2D>("Images/UI/PanelBorder");
		}
		if (_backgroundTexture == null)
		{
			_backgroundTexture = Main.Assets.Request<Texture2D>("Images/UI/PanelBackground");
		}
	}

	public UIPanel()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		SetPadding(_cornerSize);
		_needsTextureLoading = true;
	}

	public UIPanel(Asset<Texture2D> customBackground, Asset<Texture2D> customborder, int customCornerSize = 12, int customBarSize = 4)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		if (_borderTexture == null)
		{
			_borderTexture = customborder;
		}
		if (_backgroundTexture == null)
		{
			_backgroundTexture = customBackground;
		}
		_cornerSize = customCornerSize;
		_barSize = customBarSize;
		SetPadding(_cornerSize);
	}

	private void DrawPanel(SpriteBatch spriteBatch, Texture2D texture, Color color)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_018c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		//IL_019e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01da: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0220: Unknown result type (might be due to invalid IL or missing references)
		//IL_022a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0232: Unknown result type (might be due to invalid IL or missing references)
		//IL_0238: Unknown result type (might be due to invalid IL or missing references)
		//IL_024d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0265: Unknown result type (might be due to invalid IL or missing references)
		//IL_026f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0277: Unknown result type (might be due to invalid IL or missing references)
		//IL_027d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0292: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0302: Unknown result type (might be due to invalid IL or missing references)
		//IL_030c: Unknown result type (might be due to invalid IL or missing references)
		CalculatedStyle dimensions = GetDimensions();
		Point point = default(Point);
		((Point)(ref point))._002Ector((int)dimensions.X, (int)dimensions.Y);
		Point point2 = default(Point);
		((Point)(ref point2))._002Ector(point.X + (int)dimensions.Width - _cornerSize, point.Y + (int)dimensions.Height - _cornerSize);
		int width = point2.X - point.X - _cornerSize;
		int height = point2.Y - point.Y - _cornerSize;
		spriteBatch.Draw(texture, new Rectangle(point.X, point.Y, _cornerSize, _cornerSize), (Rectangle?)new Rectangle(0, 0, _cornerSize, _cornerSize), color);
		spriteBatch.Draw(texture, new Rectangle(point2.X, point.Y, _cornerSize, _cornerSize), (Rectangle?)new Rectangle(_cornerSize + _barSize, 0, _cornerSize, _cornerSize), color);
		spriteBatch.Draw(texture, new Rectangle(point.X, point2.Y, _cornerSize, _cornerSize), (Rectangle?)new Rectangle(0, _cornerSize + _barSize, _cornerSize, _cornerSize), color);
		spriteBatch.Draw(texture, new Rectangle(point2.X, point2.Y, _cornerSize, _cornerSize), (Rectangle?)new Rectangle(_cornerSize + _barSize, _cornerSize + _barSize, _cornerSize, _cornerSize), color);
		spriteBatch.Draw(texture, new Rectangle(point.X + _cornerSize, point.Y, width, _cornerSize), (Rectangle?)new Rectangle(_cornerSize, 0, _barSize, _cornerSize), color);
		spriteBatch.Draw(texture, new Rectangle(point.X + _cornerSize, point2.Y, width, _cornerSize), (Rectangle?)new Rectangle(_cornerSize, _cornerSize + _barSize, _barSize, _cornerSize), color);
		spriteBatch.Draw(texture, new Rectangle(point.X, point.Y + _cornerSize, _cornerSize, height), (Rectangle?)new Rectangle(0, _cornerSize, _cornerSize, _barSize), color);
		spriteBatch.Draw(texture, new Rectangle(point2.X, point.Y + _cornerSize, _cornerSize, height), (Rectangle?)new Rectangle(_cornerSize + _barSize, _cornerSize, _cornerSize, _barSize), color);
		spriteBatch.Draw(texture, new Rectangle(point.X + _cornerSize, point.Y + _cornerSize, width, height), (Rectangle?)new Rectangle(_cornerSize, _cornerSize, _barSize, _barSize), color);
	}

	protected override void DrawSelf(SpriteBatch spriteBatch)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		if (_needsTextureLoading)
		{
			_needsTextureLoading = false;
			LoadTextures();
		}
		if (_backgroundTexture != null)
		{
			DrawPanel(spriteBatch, _backgroundTexture.Value, BackgroundColor);
		}
		if (_borderTexture != null)
		{
			DrawPanel(spriteBatch, _borderTexture.Value, BorderColor);
		}
	}
}
