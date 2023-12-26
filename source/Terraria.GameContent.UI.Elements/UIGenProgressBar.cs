using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements;

public class UIGenProgressBar : UIElement
{
	private Asset<Texture2D> _texOuterCrimson;

	private Asset<Texture2D> _texOuterCorrupt;

	private Asset<Texture2D> _texOuterLower;

	private float _visualOverallProgress;

	private float _targetOverallProgress;

	private float _visualCurrentProgress;

	private float _targetCurrentProgress;

	private int _smallBarWidth = 508;

	private int _longBarWidth = 570;

	public UIGenProgressBar()
	{
		if (Main.netMode != 2)
		{
			_texOuterCorrupt = Main.Assets.Request<Texture2D>("Images/UI/WorldGen/Outer_Corrupt");
			_texOuterCrimson = Main.Assets.Request<Texture2D>("Images/UI/WorldGen/Outer_Crimson");
			_texOuterLower = Main.Assets.Request<Texture2D>("Images/UI/WorldGen/Outer_Lower");
		}
		Recalculate();
	}

	public override void Recalculate()
	{
		Width.Precent = 0f;
		Height.Precent = 0f;
		Width.Pixels = 612f;
		Height.Pixels = 70f;
		base.Recalculate();
	}

	public void SetProgress(float overallProgress, float currentProgress)
	{
		_targetCurrentProgress = currentProgress;
		_targetOverallProgress = overallProgress;
	}

	protected override void DrawSelf(SpriteBatch spriteBatch)
	{
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01be: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
		if (_texOuterCorrupt.IsLoaded && _texOuterCrimson.IsLoaded && _texOuterLower.IsLoaded)
		{
			bool flag = WorldGen.crimson;
			if (WorldGen.drunkWorldGen && Main.rand.Next(2) == 0)
			{
				flag = !flag;
			}
			_visualOverallProgress = _targetOverallProgress;
			_visualCurrentProgress = _targetCurrentProgress;
			CalculatedStyle dimensions = GetDimensions();
			int completedWidth = (int)(_visualOverallProgress * (float)_longBarWidth);
			int completedWidth2 = (int)(_visualCurrentProgress * (float)_smallBarWidth);
			Vector2 vector = default(Vector2);
			((Vector2)(ref vector))._002Ector(dimensions.X, dimensions.Y);
			Color color = default(Color);
			((Color)(ref color)).PackedValue = (flag ? 4286836223u : 4283888223u);
			DrawFilling2(spriteBatch, vector + new Vector2(20f, 40f), 16, completedWidth, _longBarWidth, color, Color.Lerp(color, Color.Black, 0.5f), new Color(48, 48, 48));
			((Color)(ref color)).PackedValue = 4290947159u;
			DrawFilling2(spriteBatch, vector + new Vector2(50f, 60f), 8, completedWidth2, _smallBarWidth, color, Color.Lerp(color, Color.Black, 0.5f), new Color(33, 33, 33));
			Rectangle r = GetDimensions().ToRectangle();
			r.X -= 8;
			spriteBatch.Draw(flag ? _texOuterCrimson.Value : _texOuterCorrupt.Value, r.TopLeft(), Color.White);
			spriteBatch.Draw(_texOuterLower.Value, r.TopLeft() + new Vector2(44f, 60f), Color.White);
		}
	}

	private void DrawFilling(SpriteBatch spritebatch, Texture2D tex, Texture2D texShadow, Vector2 topLeft, int completedWidth, int totalWidth, Color separator, Color empty)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		if (completedWidth % 2 != 0)
		{
			completedWidth--;
		}
		Vector2 position = topLeft + (float)completedWidth * Vector2.UnitX;
		int num = completedWidth;
		Rectangle value = tex.Frame();
		while (num > 0)
		{
			if (value.Width > num)
			{
				value.X += value.Width - num;
				value.Width = num;
			}
			spritebatch.Draw(tex, position, (Rectangle?)value, Color.White, 0f, new Vector2((float)value.Width, 0f), 1f, (SpriteEffects)0, 0f);
			position.X -= value.Width;
			num -= value.Width;
		}
		if (texShadow != null)
		{
			spritebatch.Draw(texShadow, topLeft, (Rectangle?)new Rectangle(0, 0, completedWidth, texShadow.Height), Color.White);
		}
		spritebatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle((int)topLeft.X + completedWidth, (int)topLeft.Y, totalWidth - completedWidth, tex.Height), (Rectangle?)new Rectangle(0, 0, 1, 1), empty);
		spritebatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle((int)topLeft.X + completedWidth - 2, (int)topLeft.Y, 2, tex.Height), (Rectangle?)new Rectangle(0, 0, 1, 1), separator);
	}

	private void DrawFilling2(SpriteBatch spritebatch, Vector2 topLeft, int height, int completedWidth, int totalWidth, Color filled, Color separator, Color empty)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		if (completedWidth % 2 != 0)
		{
			completedWidth--;
		}
		spritebatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle((int)topLeft.X, (int)topLeft.Y, completedWidth, height), (Rectangle?)new Rectangle(0, 0, 1, 1), filled);
		spritebatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle((int)topLeft.X + completedWidth, (int)topLeft.Y, totalWidth - completedWidth, height), (Rectangle?)new Rectangle(0, 0, 1, 1), empty);
		spritebatch.Draw(TextureAssets.MagicPixel.Value, new Rectangle((int)topLeft.X + completedWidth - 2, (int)topLeft.Y, 2, height), (Rectangle?)new Rectangle(0, 0, 1, 1), separator);
	}
}
