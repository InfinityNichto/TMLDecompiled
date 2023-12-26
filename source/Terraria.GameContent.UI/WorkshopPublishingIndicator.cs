using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using ReLogic.Graphics;
using Terraria.Audio;
using Terraria.Social;
using Terraria.Social.Base;

namespace Terraria.GameContent.UI;

public class WorkshopPublishingIndicator
{
	private float _displayUpPercent;

	private int _frameCounter;

	private bool _shouldPlayEndingSound;

	private Asset<Texture2D> _indicatorTexture;

	private int _timesSoundWasPlayed;

	public void Hide()
	{
		_displayUpPercent = 0f;
		_frameCounter = 0;
		_timesSoundWasPlayed = 0;
		_shouldPlayEndingSound = false;
	}

	public void Draw(SpriteBatch spriteBatch)
	{
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0185: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
		WorkshopSocialModule workshop = SocialAPI.Workshop;
		if (workshop == null)
		{
			return;
		}
		AWorkshopProgressReporter progressReporter = workshop.ProgressReporter;
		bool hasOngoingTasks = progressReporter.HasOngoingTasks;
		bool num6 = _displayUpPercent == 1f;
		_displayUpPercent = MathHelper.Clamp(_displayUpPercent + (float)hasOngoingTasks.ToDirectionInt() / 60f, 0f, 1f);
		bool flag = _displayUpPercent == 1f;
		if (num6 && !flag)
		{
			_shouldPlayEndingSound = true;
		}
		if (_displayUpPercent == 0f)
		{
			return;
		}
		if (_indicatorTexture == null)
		{
			_indicatorTexture = Main.Assets.Request<Texture2D>("Images/UI/Workshop/InProgress");
		}
		Texture2D value = _indicatorTexture.Value;
		int num2 = 6;
		_frameCounter++;
		int num3 = 5;
		int num4 = _frameCounter / num3 % num2;
		Vector2 val = Main.ScreenSize.ToVector2() + new Vector2(-40f, 40f);
		Vector2 value2 = val + new Vector2(0f, -80f);
		Vector2 position = Vector2.Lerp(val, value2, _displayUpPercent);
		Rectangle rectangle = value.Frame(1, 6, 0, num4);
		Vector2 origin = rectangle.Size() / 2f;
		spriteBatch.Draw(value, position, (Rectangle?)rectangle, Color.White, 0f, origin, 1f, (SpriteEffects)0, 0f);
		if (progressReporter.TryGetProgress(out var progress) && !float.IsNaN(progress))
		{
			string text = progress.ToString("P");
			DynamicSpriteFont value3 = FontAssets.ItemStack.Value;
			int num5 = 1;
			Vector2 origin2 = value3.MeasureString(text) * (float)num5 * new Vector2(0.5f, 1f);
			Utils.DrawBorderStringFourWay(spriteBatch, value3, text, position.X, position.Y - 10f, Color.White, Color.Black, origin2, num5);
		}
		if (num4 == 3 && _frameCounter % num3 == 0)
		{
			if (_shouldPlayEndingSound)
			{
				_shouldPlayEndingSound = false;
				_timesSoundWasPlayed = 0;
				SoundEngine.PlaySound(64);
			}
			if (hasOngoingTasks)
			{
				float volumeScale = Utils.Remap(_timesSoundWasPlayed, 0f, 10f, 1f, 0f);
				SoundEngine.PlaySound(21, -1, -1, 1, volumeScale);
				_timesSoundWasPlayed++;
			}
		}
	}
}
