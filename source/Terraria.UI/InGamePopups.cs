using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Achievements;
using Terraria.GameContent;
using Terraria.GameInput;
using Terraria.Social.Base;

namespace Terraria.UI;

public class InGamePopups
{
	public class AchievementUnlockedPopup : IInGameNotification
	{
		private Achievement _theAchievement;

		private Asset<Texture2D> _achievementTexture;

		private Asset<Texture2D> _achievementBorderTexture;

		private const int _iconSize = 64;

		private const int _iconSizeWithSpace = 66;

		private const int _iconsPerRow = 8;

		private int _iconIndex;

		private Rectangle _achievementIconFrame;

		private string _title;

		private int _ingameDisplayTimeLeft;

		public bool ShouldBeRemoved { get; private set; }

		public object CreationObject { get; private set; }

		private float Scale
		{
			get
			{
				if (_ingameDisplayTimeLeft < 30)
				{
					return MathHelper.Lerp(0f, 1f, (float)_ingameDisplayTimeLeft / 30f);
				}
				if (_ingameDisplayTimeLeft > 285)
				{
					return MathHelper.Lerp(1f, 0f, ((float)_ingameDisplayTimeLeft - 285f) / 15f);
				}
				return 1f;
			}
		}

		private float Opacity
		{
			get
			{
				float scale = Scale;
				if (scale <= 0.5f)
				{
					return 0f;
				}
				return (scale - 0.5f) / 0.5f;
			}
		}

		public AchievementUnlockedPopup(Achievement achievement)
		{
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			CreationObject = achievement;
			_ingameDisplayTimeLeft = 300;
			_theAchievement = achievement;
			_title = achievement.FriendlyName.Value;
			int num = (_iconIndex = Main.Achievements.GetIconIndex(achievement.Name));
			_achievementIconFrame = new Rectangle(num % 8 * 66, num / 8 * 66, 64, 64);
			_achievementTexture = Main.Assets.Request<Texture2D>("Images/UI/Achievements", AssetRequestMode.AsyncLoad);
			_achievementBorderTexture = Main.Assets.Request<Texture2D>("Images/UI/Achievement_Borders", AssetRequestMode.AsyncLoad);
		}

		public void Update()
		{
			_ingameDisplayTimeLeft--;
			if (_ingameDisplayTimeLeft < 0)
			{
				_ingameDisplayTimeLeft = 0;
			}
		}

		public void PushAnchor(ref Vector2 anchorPosition)
		{
			float num = 50f * Opacity;
			anchorPosition.Y -= num;
		}

		public void DrawInGame(SpriteBatch sb, Vector2 bottomAnchorPosition)
		{
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0101: Unknown result type (might be due to invalid IL or missing references)
			//IL_0106: Unknown result type (might be due to invalid IL or missing references)
			//IL_0114: Unknown result type (might be due to invalid IL or missing references)
			//IL_0117: Unknown result type (might be due to invalid IL or missing references)
			//IL_0121: Unknown result type (might be due to invalid IL or missing references)
			//IL_0127: Unknown result type (might be due to invalid IL or missing references)
			//IL_0144: Unknown result type (might be due to invalid IL or missing references)
			//IL_0162: Unknown result type (might be due to invalid IL or missing references)
			//IL_016e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0174: Unknown result type (might be due to invalid IL or missing references)
			//IL_0191: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01de: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
			float opacity = Opacity;
			if (opacity > 0f)
			{
				float num = Scale * 1.1f;
				Vector2 size = (FontAssets.ItemStack.Value.MeasureString(_title) + new Vector2(58f, 10f)) * num;
				Rectangle r = Utils.CenteredRectangle(bottomAnchorPosition + new Vector2(0f, (0f - size.Y) * 0.5f), size);
				Vector2 mouseScreen = Main.MouseScreen;
				bool num3 = ((Rectangle)(ref r)).Contains(mouseScreen.ToPoint());
				Utils.DrawInvBG(c: num3 ? (new Color(64, 109, 164) * 0.75f) : (new Color(64, 109, 164) * 0.5f), sb: sb, R: r);
				float num2 = num * 0.3f;
				Vector2 vector = r.Right() - Vector2.UnitX * num * (12f + num2 * (float)_achievementIconFrame.Width);
				sb.Draw(_achievementTexture.Value, vector, (Rectangle?)_achievementIconFrame, Color.White * opacity, 0f, new Vector2(0f, (float)(_achievementIconFrame.Height / 2)), num2, (SpriteEffects)0, 0f);
				sb.Draw(_achievementBorderTexture.Value, vector, (Rectangle?)null, Color.White * opacity, 0f, new Vector2(0f, (float)(_achievementIconFrame.Height / 2)), num2, (SpriteEffects)0, 0f);
				Utils.DrawBorderString(color: new Color((int)Main.mouseTextColor, (int)Main.mouseTextColor, Main.mouseTextColor / 5, (int)Main.mouseTextColor) * opacity, sb: sb, text: _title, pos: vector - Vector2.UnitX * 10f, scale: num * 0.9f, anchorx: 1f, anchory: 0.4f);
				if (num3)
				{
					OnMouseOver();
				}
			}
		}

		private void OnMouseOver()
		{
			if (!PlayerInput.IgnoreMouseInterface)
			{
				Main.player[Main.myPlayer].mouseInterface = true;
				if (Main.mouseLeft && Main.mouseLeftRelease)
				{
					Main.mouseLeftRelease = false;
					IngameFancyUI.OpenAchievementsAndGoto(_theAchievement);
					_ingameDisplayTimeLeft = 0;
					ShouldBeRemoved = true;
				}
			}
		}

		public void DrawInNotificationsArea(SpriteBatch spriteBatch, Rectangle area, ref int gamepadPointLocalIndexTouse)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			Utils.DrawInvBG(spriteBatch, area, Color.Red);
		}
	}

	public class PlayerWantsToJoinGamePopup : IInGameNotification
	{
		private int _timeLeft;

		private const int _timeLeftMax = 1800;

		private string _displayTextWithoutTime;

		private UserJoinToServerRequest _request;

		private float Scale
		{
			get
			{
				if (_timeLeft < 30)
				{
					return MathHelper.Lerp(0f, 1f, (float)_timeLeft / 30f);
				}
				if (_timeLeft > 1785)
				{
					return MathHelper.Lerp(1f, 0f, ((float)_timeLeft - 1785f) / 15f);
				}
				return 1f;
			}
		}

		private float Opacity
		{
			get
			{
				float scale = Scale;
				if (scale <= 0.5f)
				{
					return 0f;
				}
				return (scale - 0.5f) / 0.5f;
			}
		}

		public object CreationObject { get; private set; }

		public bool ShouldBeRemoved => _timeLeft <= 0;

		public PlayerWantsToJoinGamePopup(UserJoinToServerRequest request)
		{
			_request = request;
			CreationObject = request;
			_timeLeft = 1800;
			switch (Main.rand.Next(5))
			{
			default:
				_displayTextWithoutTime = "This Bloke Wants to Join you";
				break;
			case 1:
				_displayTextWithoutTime = "This Fucker Wants to Join you";
				break;
			case 2:
				_displayTextWithoutTime = "This Weirdo Wants to Join you";
				break;
			case 3:
				_displayTextWithoutTime = "This Great Gal Wants to Join you";
				break;
			case 4:
				_displayTextWithoutTime = "The one guy who beat you up 30 years ago Wants to Join you";
				break;
			}
		}

		public void Update()
		{
			_timeLeft--;
		}

		public void DrawInGame(SpriteBatch spriteBatch, Vector2 bottomAnchorPosition)
		{
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_00da: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_0101: Unknown result type (might be due to invalid IL or missing references)
			//IL_0173: Unknown result type (might be due to invalid IL or missing references)
			//IL_0184: Unknown result type (might be due to invalid IL or missing references)
			//IL_0189: Unknown result type (might be due to invalid IL or missing references)
			//IL_0190: Unknown result type (might be due to invalid IL or missing references)
			//IL_0195: Unknown result type (might be due to invalid IL or missing references)
			//IL_019a: Unknown result type (might be due to invalid IL or missing references)
			//IL_019e: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01af: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0262: Unknown result type (might be due to invalid IL or missing references)
			//IL_0273: Unknown result type (might be due to invalid IL or missing references)
			//IL_0278: Unknown result type (might be due to invalid IL or missing references)
			//IL_027f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0284: Unknown result type (might be due to invalid IL or missing references)
			//IL_0289: Unknown result type (might be due to invalid IL or missing references)
			//IL_028d: Unknown result type (might be due to invalid IL or missing references)
			//IL_028f: Unknown result type (might be due to invalid IL or missing references)
			//IL_029e: Unknown result type (might be due to invalid IL or missing references)
			//IL_02aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_02bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02da: Unknown result type (might be due to invalid IL or missing references)
			//IL_02df: Unknown result type (might be due to invalid IL or missing references)
			//IL_0315: Unknown result type (might be due to invalid IL or missing references)
			//IL_031b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0320: Unknown result type (might be due to invalid IL or missing references)
			//IL_0326: Unknown result type (might be due to invalid IL or missing references)
			//IL_032b: Unknown result type (might be due to invalid IL or missing references)
			//IL_033a: Unknown result type (might be due to invalid IL or missing references)
			//IL_033f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0344: Unknown result type (might be due to invalid IL or missing references)
			//IL_0358: Unknown result type (might be due to invalid IL or missing references)
			float opacity = Opacity;
			if (opacity > 0f)
			{
				string text = Utils.FormatWith(_request.GetUserWrapperText(), new
				{
					DisplayName = _request.UserDisplayName,
					FullId = _request.UserFullIdentifier
				});
				float num = Scale * 1.1f;
				Vector2 size = (FontAssets.ItemStack.Value.MeasureString(text) + new Vector2(58f, 10f)) * num;
				Rectangle r = Utils.CenteredRectangle(bottomAnchorPosition + new Vector2(0f, (0f - size.Y) * 0.5f), size);
				Vector2 mouseScreen = Main.MouseScreen;
				Color c = (((Rectangle)(ref r)).Contains(mouseScreen.ToPoint()) ? (new Color(64, 109, 164) * 0.75f) : (new Color(64, 109, 164) * 0.5f));
				Utils.DrawInvBG(spriteBatch, r, c);
				Vector2 vector = default(Vector2);
				((Vector2)(ref vector))._002Ector((float)((Rectangle)(ref r)).Left, (float)((Rectangle)(ref r)).Center.Y);
				vector.X += 32f;
				Texture2D value = Main.Assets.Request<Texture2D>("Images/UI/ButtonPlay").Value;
				Vector2 vector2 = default(Vector2);
				((Vector2)(ref vector2))._002Ector((float)(((Rectangle)(ref r)).Left + 7), MathHelper.Lerp((float)((Rectangle)(ref r)).Top, (float)((Rectangle)(ref r)).Bottom, 0.5f) - (float)(value.Height / 2) - 1f);
				Rectangle val = Utils.CenteredRectangle(vector2 + new Vector2((float)(value.Width / 2), 0f), value.Size());
				bool flag = ((Rectangle)(ref val)).Contains(mouseScreen.ToPoint());
				spriteBatch.Draw(value, vector2, (Rectangle?)null, Color.White * (flag ? 1f : 0.5f), 0f, new Vector2(0f, 0.5f) * value.Size(), 1f, (SpriteEffects)0, 0f);
				if (flag)
				{
					OnMouseOver();
				}
				value = Main.Assets.Request<Texture2D>("Images/UI/ButtonDelete").Value;
				((Vector2)(ref vector2))._002Ector((float)(((Rectangle)(ref r)).Left + 7), MathHelper.Lerp((float)((Rectangle)(ref r)).Top, (float)((Rectangle)(ref r)).Bottom, 0.5f) + (float)(value.Height / 2) + 1f);
				val = Utils.CenteredRectangle(vector2 + new Vector2((float)(value.Width / 2), 0f), value.Size());
				flag = ((Rectangle)(ref val)).Contains(mouseScreen.ToPoint());
				spriteBatch.Draw(value, vector2, (Rectangle?)null, Color.White * (flag ? 1f : 0.5f), 0f, new Vector2(0f, 0.5f) * value.Size(), 1f, (SpriteEffects)0, 0f);
				if (flag)
				{
					OnMouseOver(reject: true);
				}
				Color color = new Color((int)Main.mouseTextColor, (int)Main.mouseTextColor, Main.mouseTextColor / 5, (int)Main.mouseTextColor) * opacity;
				Utils.DrawBorderString(spriteBatch, text, ((Rectangle)(ref r)).Center.ToVector2() + new Vector2(10f, 0f), color, num * 0.9f, 0.5f, 0.4f);
			}
		}

		private void OnMouseOver(bool reject = false)
		{
			if (PlayerInput.IgnoreMouseInterface)
			{
				return;
			}
			Main.player[Main.myPlayer].mouseInterface = true;
			if (Main.mouseLeft && Main.mouseLeftRelease)
			{
				Main.mouseLeftRelease = false;
				_timeLeft = 0;
				if (reject)
				{
					_request.Reject();
				}
				else
				{
					_request.Accept();
				}
			}
		}

		public void PushAnchor(ref Vector2 positionAnchorBottom)
		{
			float num = 70f * Opacity;
			positionAnchorBottom.Y -= num;
		}

		public void DrawInNotificationsArea(SpriteBatch spriteBatch, Rectangle area, ref int gamepadPointLocalIndexTouse)
		{
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_011b: Unknown result type (might be due to invalid IL or missing references)
			//IL_012c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0131: Unknown result type (might be due to invalid IL or missing references)
			//IL_0138: Unknown result type (might be due to invalid IL or missing references)
			//IL_013d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0142: Unknown result type (might be due to invalid IL or missing references)
			//IL_0146: Unknown result type (might be due to invalid IL or missing references)
			//IL_0147: Unknown result type (might be due to invalid IL or missing references)
			//IL_0156: Unknown result type (might be due to invalid IL or missing references)
			//IL_0162: Unknown result type (might be due to invalid IL or missing references)
			//IL_0177: Unknown result type (might be due to invalid IL or missing references)
			//IL_018b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0192: Unknown result type (might be due to invalid IL or missing references)
			//IL_0197: Unknown result type (might be due to invalid IL or missing references)
			//IL_0209: Unknown result type (might be due to invalid IL or missing references)
			//IL_021a: Unknown result type (might be due to invalid IL or missing references)
			//IL_021f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0226: Unknown result type (might be due to invalid IL or missing references)
			//IL_022b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0230: Unknown result type (might be due to invalid IL or missing references)
			//IL_0234: Unknown result type (might be due to invalid IL or missing references)
			//IL_0235: Unknown result type (might be due to invalid IL or missing references)
			//IL_0244: Unknown result type (might be due to invalid IL or missing references)
			//IL_0250: Unknown result type (might be due to invalid IL or missing references)
			//IL_0265: Unknown result type (might be due to invalid IL or missing references)
			//IL_0279: Unknown result type (might be due to invalid IL or missing references)
			//IL_0280: Unknown result type (might be due to invalid IL or missing references)
			//IL_0285: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e8: Unknown result type (might be due to invalid IL or missing references)
			string userWrapperText = _request.GetUserWrapperText();
			string text = _request.UserDisplayName;
			Utils.TrimTextIfNeeded(ref text, FontAssets.MouseText.Value, 0.9f, area.Width / 4);
			string text2 = Utils.FormatWith(userWrapperText, new
			{
				DisplayName = text,
				FullId = _request.UserFullIdentifier
			});
			Vector2 mouseScreen = Main.MouseScreen;
			Color c = (((Rectangle)(ref area)).Contains(mouseScreen.ToPoint()) ? (new Color(64, 109, 164) * 0.75f) : (new Color(64, 109, 164) * 0.5f));
			Utils.DrawInvBG(spriteBatch, area, c);
			Vector2 pos = default(Vector2);
			((Vector2)(ref pos))._002Ector((float)((Rectangle)(ref area)).Left, (float)((Rectangle)(ref area)).Center.Y);
			pos.X += 32f;
			Texture2D value = Main.Assets.Request<Texture2D>("Images/UI/ButtonPlay").Value;
			Vector2 vector = default(Vector2);
			((Vector2)(ref vector))._002Ector((float)(((Rectangle)(ref area)).Left + 7), MathHelper.Lerp((float)((Rectangle)(ref area)).Top, (float)((Rectangle)(ref area)).Bottom, 0.5f) - (float)(value.Height / 2) - 1f);
			Rectangle val = Utils.CenteredRectangle(vector + new Vector2((float)(value.Width / 2), 0f), value.Size());
			bool flag = ((Rectangle)(ref val)).Contains(mouseScreen.ToPoint());
			spriteBatch.Draw(value, vector, (Rectangle?)null, Color.White * (flag ? 1f : 0.5f), 0f, new Vector2(0f, 0.5f) * value.Size(), 1f, (SpriteEffects)0, 0f);
			if (flag)
			{
				OnMouseOver();
			}
			value = Main.Assets.Request<Texture2D>("Images/UI/ButtonDelete").Value;
			((Vector2)(ref vector))._002Ector((float)(((Rectangle)(ref area)).Left + 7), MathHelper.Lerp((float)((Rectangle)(ref area)).Top, (float)((Rectangle)(ref area)).Bottom, 0.5f) + (float)(value.Height / 2) + 1f);
			val = Utils.CenteredRectangle(vector + new Vector2((float)(value.Width / 2), 0f), value.Size());
			flag = ((Rectangle)(ref val)).Contains(mouseScreen.ToPoint());
			spriteBatch.Draw(value, vector, (Rectangle?)null, Color.White * (flag ? 1f : 0.5f), 0f, new Vector2(0f, 0.5f) * value.Size(), 1f, (SpriteEffects)0, 0f);
			if (flag)
			{
				OnMouseOver(reject: true);
			}
			pos.X += 6f;
			Color color = default(Color);
			((Color)(ref color))._002Ector((int)Main.mouseTextColor, (int)Main.mouseTextColor, Main.mouseTextColor / 5, (int)Main.mouseTextColor);
			Utils.DrawBorderString(spriteBatch, text2, pos, color, 0.9f, 0f, 0.4f);
		}
	}
}
