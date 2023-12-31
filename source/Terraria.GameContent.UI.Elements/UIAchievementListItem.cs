using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Achievements;
using Terraria.Localization;
using Terraria.UI;
using Terraria.UI.Chat;

namespace Terraria.GameContent.UI.Elements;

public class UIAchievementListItem : UIPanel
{
	private Achievement _achievement;

	private UIImageFramed _achievementIcon;

	private UIImage _achievementIconBorders;

	private const int _iconSize = 64;

	private const int _iconSizeWithSpace = 66;

	private const int _iconsPerRow = 8;

	private int _iconIndex;

	private Rectangle _iconFrame;

	private Rectangle _iconFrameUnlocked;

	private Rectangle _iconFrameLocked;

	private Asset<Texture2D> _innerPanelTopTexture;

	private Asset<Texture2D> _innerPanelBottomTexture;

	private Asset<Texture2D> _categoryTexture;

	private bool _locked;

	private bool _large;

	public UIAchievementListItem(Achievement achievement, bool largeForOtherLanguages)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		_large = largeForOtherLanguages;
		BackgroundColor = new Color(26, 40, 89) * 0.8f;
		BorderColor = new Color(13, 20, 44) * 0.8f;
		float num = 16 + _large.ToInt() * 20;
		float num2 = _large.ToInt() * 6;
		float num3 = _large.ToInt() * 12;
		_achievement = achievement;
		Height.Set(66f + num, 0f);
		Width.Set(0f, 1f);
		PaddingTop = 8f;
		PaddingLeft = 9f;
		int num4 = (_iconIndex = Main.Achievements.GetIconIndex(achievement.Name));
		_iconFrameUnlocked = new Rectangle(num4 % 8 * 66, num4 / 8 * 66, 64, 64);
		_iconFrameLocked = _iconFrameUnlocked;
		_iconFrameLocked.X += 528;
		_iconFrame = _iconFrameLocked;
		UpdateIconFrame();
		_achievementIcon = new UIImageFramed(Main.Assets.Request<Texture2D>("Images/UI/Achievements"), _iconFrame);
		_achievementIcon.Left.Set(num2, 0f);
		_achievementIcon.Top.Set(num3, 0f);
		Append(_achievementIcon);
		_achievementIconBorders = new UIImage(Main.Assets.Request<Texture2D>("Images/UI/Achievement_Borders"));
		_achievementIconBorders.Left.Set(-4f + num2, 0f);
		_achievementIconBorders.Top.Set(-4f + num3, 0f);
		Append(_achievementIconBorders);
		_innerPanelTopTexture = Main.Assets.Request<Texture2D>("Images/UI/Achievement_InnerPanelTop");
		if (_large)
		{
			_innerPanelBottomTexture = Main.Assets.Request<Texture2D>("Images/UI/Achievement_InnerPanelBottom_Large");
		}
		else
		{
			_innerPanelBottomTexture = Main.Assets.Request<Texture2D>("Images/UI/Achievement_InnerPanelBottom");
		}
		_categoryTexture = Main.Assets.Request<Texture2D>("Images/UI/Achievement_Categories");
	}

	protected override void DrawSelf(SpriteBatch spriteBatch)
	{
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_017b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0191: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01af: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01db: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0204: Unknown result type (might be due to invalid IL or missing references)
		//IL_0209: Unknown result type (might be due to invalid IL or missing references)
		//IL_021a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0213: Unknown result type (might be due to invalid IL or missing references)
		//IL_021f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0221: Unknown result type (might be due to invalid IL or missing references)
		//IL_0222: Unknown result type (might be due to invalid IL or missing references)
		//IL_022c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0231: Unknown result type (might be due to invalid IL or missing references)
		//IL_0236: Unknown result type (might be due to invalid IL or missing references)
		//IL_0237: Unknown result type (might be due to invalid IL or missing references)
		//IL_023c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0240: Unknown result type (might be due to invalid IL or missing references)
		//IL_0244: Unknown result type (might be due to invalid IL or missing references)
		//IL_0284: Unknown result type (might be due to invalid IL or missing references)
		//IL_0293: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0306: Unknown result type (might be due to invalid IL or missing references)
		//IL_0308: Unknown result type (might be due to invalid IL or missing references)
		//IL_030f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0314: Unknown result type (might be due to invalid IL or missing references)
		//IL_031d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0333: Unknown result type (might be due to invalid IL or missing references)
		//IL_033d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0342: Unknown result type (might be due to invalid IL or missing references)
		//IL_0347: Unknown result type (might be due to invalid IL or missing references)
		//IL_0348: Unknown result type (might be due to invalid IL or missing references)
		//IL_034d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0351: Unknown result type (might be due to invalid IL or missing references)
		//IL_0355: Unknown result type (might be due to invalid IL or missing references)
		//IL_0389: Unknown result type (might be due to invalid IL or missing references)
		//IL_038b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0392: Unknown result type (might be due to invalid IL or missing references)
		//IL_0397: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03be: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_041d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0424: Unknown result type (might be due to invalid IL or missing references)
		//IL_0429: Unknown result type (might be due to invalid IL or missing references)
		//IL_0465: Unknown result type (might be due to invalid IL or missing references)
		//IL_0467: Unknown result type (might be due to invalid IL or missing references)
		//IL_0471: Unknown result type (might be due to invalid IL or missing references)
		//IL_0476: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_04af: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_050b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0522: Unknown result type (might be due to invalid IL or missing references)
		//IL_0524: Unknown result type (might be due to invalid IL or missing references)
		//IL_0535: Unknown result type (might be due to invalid IL or missing references)
		//IL_053a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0546: Unknown result type (might be due to invalid IL or missing references)
		//IL_0496: Unknown result type (might be due to invalid IL or missing references)
		//IL_0498: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a7: Unknown result type (might be due to invalid IL or missing references)
		base.DrawSelf(spriteBatch);
		int num = _large.ToInt() * 6;
		Vector2 vector = default(Vector2);
		((Vector2)(ref vector))._002Ector((float)num, 0f);
		_locked = !_achievement.IsCompleted;
		UpdateIconFrame();
		CalculatedStyle innerDimensions = GetInnerDimensions();
		CalculatedStyle dimensions = _achievementIconBorders.GetDimensions();
		float num2 = dimensions.X + dimensions.Width;
		Vector2 val = new Vector2(num2 + 7f, innerDimensions.Y);
		Tuple<decimal, decimal> trackerValues = GetTrackerValues();
		bool flag = false;
		if ((!(trackerValues.Item1 == 0m) || !(trackerValues.Item2 == 0m)) && _locked)
		{
			flag = true;
		}
		float num3 = innerDimensions.Width - dimensions.Width + 1f - (float)(num * 2);
		Vector2 baseScale = default(Vector2);
		((Vector2)(ref baseScale))._002Ector(0.85f);
		Vector2 baseScale2 = default(Vector2);
		((Vector2)(ref baseScale2))._002Ector(0.92f);
		string text = FontAssets.ItemStack.Value.CreateWrappedText(_achievement.Description.Value, (num3 - 20f) * (1f / baseScale2.X), Language.ActiveCulture.CultureInfo);
		Vector2 stringSize = ChatManager.GetStringSize(FontAssets.ItemStack.Value, text, baseScale2, num3);
		if (!_large)
		{
			stringSize = ChatManager.GetStringSize(FontAssets.ItemStack.Value, _achievement.Description.Value, baseScale2, num3);
		}
		float num4 = 38f + (float)(_large ? 20 : 0);
		if (stringSize.Y > num4)
		{
			baseScale2.Y *= num4 / stringSize.Y;
		}
		Color value = (_locked ? Color.Silver : Color.Gold);
		value = Color.Lerp(value, Color.White, base.IsMouseHovering ? 0.5f : 0f);
		Color value2 = (_locked ? Color.DarkGray : Color.Silver);
		value2 = Color.Lerp(value2, Color.White, base.IsMouseHovering ? 1f : 0f);
		Color color = (base.IsMouseHovering ? Color.White : Color.Gray);
		Vector2 vector2 = val - Vector2.UnitY * 2f + vector;
		DrawPanelTop(spriteBatch, vector2, num3, color);
		AchievementCategory category = _achievement.Category;
		vector2.Y += 2f;
		vector2.X += 4f;
		spriteBatch.Draw(_categoryTexture.Value, vector2, (Rectangle?)_categoryTexture.Frame(4, 2, (int)category), base.IsMouseHovering ? Color.White : Color.Silver, 0f, Vector2.Zero, 0.5f, (SpriteEffects)0, 0f);
		vector2.X += 4f;
		vector2.X += 17f;
		ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.ItemStack.Value, _achievement.FriendlyName.Value, vector2, value, 0f, Vector2.Zero, baseScale, num3);
		vector2.X -= 17f;
		Vector2 position = val + Vector2.UnitY * 27f + vector;
		DrawPanelBottom(spriteBatch, position, num3, color);
		position.X += 8f;
		position.Y += 4f;
		ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.ItemStack.Value, text, position, value2, 0f, Vector2.Zero, baseScale2);
		if (flag)
		{
			Vector2 vector3 = vector2 + Vector2.UnitX * num3 + Vector2.UnitY;
			string text2 = (int)trackerValues.Item1 + "/" + (int)trackerValues.Item2;
			Vector2 baseScale3 = default(Vector2);
			((Vector2)(ref baseScale3))._002Ector(0.75f);
			Vector2 stringSize2 = ChatManager.GetStringSize(FontAssets.ItemStack.Value, text2, baseScale3);
			float progress = (float)(trackerValues.Item1 / trackerValues.Item2);
			float num5 = 80f;
			Color color2 = default(Color);
			((Color)(ref color2))._002Ector(100, 255, 100);
			if (!base.IsMouseHovering)
			{
				color2 = Color.Lerp(color2, Color.Black, 0.25f);
			}
			Color color3 = default(Color);
			((Color)(ref color3))._002Ector(255, 255, 255);
			if (!base.IsMouseHovering)
			{
				color3 = Color.Lerp(color3, Color.Black, 0.25f);
			}
			DrawProgressBar(spriteBatch, progress, vector3 - Vector2.UnitX * num5 * 0.7f, num5, color3, color2, color2.MultiplyRGBA(new Color(new Vector4(1f, 1f, 1f, 0.5f))));
			vector3.X -= num5 * 1.4f + stringSize2.X;
			ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.ItemStack.Value, text2, vector3, value, 0f, new Vector2(0f, 0f), baseScale3, 90f);
		}
	}

	private void UpdateIconFrame()
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		if (!_locked)
		{
			_iconFrame = _iconFrameUnlocked;
		}
		else
		{
			_iconFrame = _iconFrameLocked;
		}
		if (_achievementIcon != null)
		{
			_achievementIcon.SetFrame(_iconFrame);
		}
	}

	private void DrawPanelTop(SpriteBatch spriteBatch, Vector2 position, float width, Color color)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		spriteBatch.Draw(_innerPanelTopTexture.Value, position, (Rectangle?)new Rectangle(0, 0, 2, _innerPanelTopTexture.Height()), color);
		spriteBatch.Draw(_innerPanelTopTexture.Value, new Vector2(position.X + 2f, position.Y), (Rectangle?)new Rectangle(2, 0, 2, _innerPanelTopTexture.Height()), color, 0f, Vector2.Zero, new Vector2((width - 4f) / 2f, 1f), (SpriteEffects)0, 0f);
		spriteBatch.Draw(_innerPanelTopTexture.Value, new Vector2(position.X + width - 2f, position.Y), (Rectangle?)new Rectangle(4, 0, 2, _innerPanelTopTexture.Height()), color);
	}

	private void DrawPanelBottom(SpriteBatch spriteBatch, Vector2 position, float width, Color color)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		spriteBatch.Draw(_innerPanelBottomTexture.Value, position, (Rectangle?)new Rectangle(0, 0, 6, _innerPanelBottomTexture.Height()), color);
		spriteBatch.Draw(_innerPanelBottomTexture.Value, new Vector2(position.X + 6f, position.Y), (Rectangle?)new Rectangle(6, 0, 7, _innerPanelBottomTexture.Height()), color, 0f, Vector2.Zero, new Vector2((width - 12f) / 7f, 1f), (SpriteEffects)0, 0f);
		spriteBatch.Draw(_innerPanelBottomTexture.Value, new Vector2(position.X + width - 6f, position.Y), (Rectangle?)new Rectangle(13, 0, 6, _innerPanelBottomTexture.Height()), color);
	}

	public override void MouseOver(UIMouseEvent evt)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		base.MouseOver(evt);
		BackgroundColor = new Color(46, 60, 119);
		BorderColor = new Color(20, 30, 56);
	}

	public override void MouseOut(UIMouseEvent evt)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		base.MouseOut(evt);
		BackgroundColor = new Color(26, 40, 89) * 0.8f;
		BorderColor = new Color(13, 20, 44) * 0.8f;
	}

	public Achievement GetAchievement()
	{
		return _achievement;
	}

	private Tuple<decimal, decimal> GetTrackerValues()
	{
		if (!_achievement.HasTracker)
		{
			return Tuple.Create(0m, 0m);
		}
		IAchievementTracker tracker = _achievement.GetTracker();
		if (tracker.GetTrackerType() == TrackerType.Int)
		{
			AchievementTracker<int> achievementTracker = (AchievementTracker<int>)tracker;
			return Tuple.Create((decimal)achievementTracker.Value, (decimal)achievementTracker.MaxValue);
		}
		if (tracker.GetTrackerType() == TrackerType.Float)
		{
			AchievementTracker<float> achievementTracker2 = (AchievementTracker<float>)tracker;
			return Tuple.Create((decimal)achievementTracker2.Value, (decimal)achievementTracker2.MaxValue);
		}
		return Tuple.Create(0m, 0m);
	}

	private void DrawProgressBar(SpriteBatch spriteBatch, float progress, Vector2 spot, float Width = 169f, Color BackColor = default(Color), Color FillingColor = default(Color), Color BlipColor = default(Color))
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0165: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_017c: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01de: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0202: Unknown result type (might be due to invalid IL or missing references)
		//IL_020c: Unknown result type (might be due to invalid IL or missing references)
		//IL_021d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0227: Unknown result type (might be due to invalid IL or missing references)
		//IL_0280: Unknown result type (might be due to invalid IL or missing references)
		//IL_0286: Unknown result type (might be due to invalid IL or missing references)
		//IL_0290: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0241: Unknown result type (might be due to invalid IL or missing references)
		//IL_0247: Unknown result type (might be due to invalid IL or missing references)
		//IL_0251: Unknown result type (might be due to invalid IL or missing references)
		//IL_0262: Unknown result type (might be due to invalid IL or missing references)
		//IL_026e: Unknown result type (might be due to invalid IL or missing references)
		if (BlipColor == Color.Transparent)
		{
			((Color)(ref BlipColor))._002Ector(255, 165, 0, 127);
		}
		if (FillingColor == Color.Transparent)
		{
			((Color)(ref FillingColor))._002Ector(255, 241, 51);
		}
		if (BackColor == Color.Transparent)
		{
			((Color)(ref FillingColor))._002Ector(255, 255, 255);
		}
		Texture2D value = TextureAssets.ColorBar.Value;
		_ = TextureAssets.ColorBlip.Value;
		Texture2D value2 = TextureAssets.MagicPixel.Value;
		float num = MathHelper.Clamp(progress, 0f, 1f);
		float num2 = Width * 1f;
		float num3 = 8f;
		float num4 = num2 / 169f;
		Vector2 position = spot + Vector2.UnitY * num3 + Vector2.UnitX * 1f;
		spriteBatch.Draw(value, spot, (Rectangle?)new Rectangle(5, 0, value.Width - 9, value.Height), BackColor, 0f, new Vector2(84.5f, 0f), new Vector2(num4, 1f), (SpriteEffects)0, 0f);
		spriteBatch.Draw(value, spot + new Vector2((0f - num4) * 84.5f - 5f, 0f), (Rectangle?)new Rectangle(0, 0, 5, value.Height), BackColor, 0f, Vector2.Zero, Vector2.One, (SpriteEffects)0, 0f);
		spriteBatch.Draw(value, spot + new Vector2(num4 * 84.5f, 0f), (Rectangle?)new Rectangle(value.Width - 4, 0, 4, value.Height), BackColor, 0f, Vector2.Zero, Vector2.One, (SpriteEffects)0, 0f);
		position += Vector2.UnitX * (num - 0.5f) * num2;
		position.X -= 1f;
		spriteBatch.Draw(value2, position, (Rectangle?)new Rectangle(0, 0, 1, 1), FillingColor, 0f, new Vector2(1f, 0.5f), new Vector2(num2 * num, num3), (SpriteEffects)0, 0f);
		if (progress != 0f)
		{
			spriteBatch.Draw(value2, position, (Rectangle?)new Rectangle(0, 0, 1, 1), BlipColor, 0f, new Vector2(1f, 0.5f), new Vector2(2f, num3), (SpriteEffects)0, 0f);
		}
		spriteBatch.Draw(value2, position, (Rectangle?)new Rectangle(0, 0, 1, 1), Color.Black, 0f, new Vector2(0f, 0.5f), new Vector2(num2 * (1f - num), num3), (SpriteEffects)0, 0f);
	}

	public override int CompareTo(object obj)
	{
		if (!(obj is UIAchievementListItem uIAchievementListItem))
		{
			return 0;
		}
		if (_achievement.IsCompleted && !uIAchievementListItem._achievement.IsCompleted)
		{
			return -1;
		}
		if (!_achievement.IsCompleted && uIAchievementListItem._achievement.IsCompleted)
		{
			return 1;
		}
		return _achievement.Id.CompareTo(uIAchievementListItem._achievement.Id);
	}
}
