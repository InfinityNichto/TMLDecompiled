using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;

namespace Terraria.GameContent.UI;

public class IssueReportsIndicator
{
	private float _displayUpPercent;

	private bool _shouldBeShowing;

	private Asset<Texture2D> _buttonTexture;

	private Asset<Texture2D> _buttonOutlineTexture;

	public void AttemptLettingPlayerKnow()
	{
		Setup();
		_shouldBeShowing = true;
		if (SoundEngine.LegacySoundPlayer != null)
		{
			SoundEngine.PlaySound(in SoundID.DD2_ExplosiveTrapExplode);
		}
	}

	public void Hide()
	{
		_shouldBeShowing = false;
		_displayUpPercent = 0f;
	}

	private void OpenUI()
	{
		Setup();
		Main.OpenReportsMenu();
	}

	private void Setup()
	{
		_buttonTexture = Main.Assets.Request<Texture2D>("Images/UI/Workshop/IssueButton");
		_buttonOutlineTexture = Main.Assets.Request<Texture2D>("Images/UI/Workshop/IssueButton_Outline");
	}

	public void Draw(SpriteBatch spriteBatch)
	{
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		bool shouldBeShowing = _shouldBeShowing;
		_displayUpPercent = MathHelper.Clamp(_displayUpPercent + (float)shouldBeShowing.ToDirectionInt(), 0f, 1f);
		if (_displayUpPercent == 0f)
		{
			return;
		}
		Texture2D value = _buttonTexture.Value;
		Vector2 val = Main.ScreenSize.ToVector2() + new Vector2(40f, -80f);
		Vector2 value2 = val + new Vector2(-80f, 0f);
		Vector2 vector2 = Vector2.Lerp(val, value2, _displayUpPercent);
		Rectangle rectangle = value.Frame();
		Vector2 origin = rectangle.Size() / 2f;
		bool flag = false;
		Rectangle val2 = Utils.CenteredRectangle(vector2, rectangle.Size());
		if (((Rectangle)(ref val2)).Contains(Main.MouseScreen.ToPoint()))
		{
			flag = true;
			string textValue = Language.GetTextValue("UI.IssueReporterHasThingsToShow");
			Main.instance.MouseText(textValue, 0, 0);
			if (Main.mouseLeft)
			{
				OpenUI();
				Hide();
				return;
			}
		}
		float scale = 1f;
		spriteBatch.Draw(value, vector2, (Rectangle?)rectangle, Color.White, 0f, origin, scale, (SpriteEffects)0, 0f);
		if (flag)
		{
			Texture2D value3 = _buttonOutlineTexture.Value;
			Rectangle rectangle2 = value3.Frame();
			spriteBatch.Draw(value3, vector2, (Rectangle?)rectangle2, Color.White, 0f, rectangle2.Size() / 2f, scale, (SpriteEffects)0, 0f);
		}
	}
}
