using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using ReLogic.Graphics;
using Terraria.DataStructures;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Terraria.GameContent.UI.ResourceSets;

public class HorizontalBarsPlayerResourcesDisplaySet : IPlayerResourcesDisplaySet, IConfigKeyHolder
{
	private int _maxSegmentCount;

	private int _hpSegmentsCount;

	private int _mpSegmentsCount;

	private int _hpFruitCount;

	private float _hpPercent;

	private float _mpPercent;

	private byte _drawTextStyle;

	private bool _hpHovered;

	private bool _mpHovered;

	private Asset<Texture2D> _hpFill;

	private Asset<Texture2D> _hpFillHoney;

	private Asset<Texture2D> _mpFill;

	private Asset<Texture2D> _panelLeft;

	private Asset<Texture2D> _panelMiddleHP;

	private Asset<Texture2D> _panelRightHP;

	private Asset<Texture2D> _panelMiddleMP;

	private Asset<Texture2D> _panelRightMP;

	private PlayerStatsSnapshot preparedSnapshot;

	public string DisplayedName => Language.GetTextValue("UI.HealthManaStyle_" + NameKey);

	public string NameKey { get; private set; }

	public string ConfigKey { get; private set; }

	public HorizontalBarsPlayerResourcesDisplaySet(string nameKey, string configKey, string resourceFolderName, AssetRequestMode mode)
	{
		NameKey = nameKey;
		ConfigKey = configKey;
		if (configKey == "HorizontalBarsWithFullText")
		{
			_drawTextStyle = 2;
		}
		else if (configKey == "HorizontalBarsWithText")
		{
			_drawTextStyle = 1;
		}
		else
		{
			_drawTextStyle = 0;
		}
		string text = "Images\\UI\\PlayerResourceSets\\" + resourceFolderName;
		_hpFill = Main.Assets.Request<Texture2D>(text + "\\HP_Fill", mode);
		_hpFillHoney = Main.Assets.Request<Texture2D>(text + "\\HP_Fill_Honey", mode);
		_mpFill = Main.Assets.Request<Texture2D>(text + "\\MP_Fill", mode);
		_panelLeft = Main.Assets.Request<Texture2D>(text + "\\Panel_Left", mode);
		_panelMiddleHP = Main.Assets.Request<Texture2D>(text + "\\HP_Panel_Middle", mode);
		_panelRightHP = Main.Assets.Request<Texture2D>(text + "\\HP_Panel_Right", mode);
		_panelMiddleMP = Main.Assets.Request<Texture2D>(text + "\\MP_Panel_Middle", mode);
		_panelRightMP = Main.Assets.Request<Texture2D>(text + "\\MP_Panel_Right", mode);
	}

	public void Draw()
	{
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_021b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0189: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0193: Unknown result type (might be due to invalid IL or missing references)
		//IL_01be: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01db: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02af: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_033f: Unknown result type (might be due to invalid IL or missing references)
		//IL_034b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0350: Unknown result type (might be due to invalid IL or missing references)
		//IL_0355: Unknown result type (might be due to invalid IL or missing references)
		//IL_0380: Unknown result type (might be due to invalid IL or missing references)
		//IL_0385: Unknown result type (might be due to invalid IL or missing references)
		//IL_038c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0391: Unknown result type (might be due to invalid IL or missing references)
		//IL_0398: Unknown result type (might be due to invalid IL or missing references)
		//IL_039d: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a9: Unknown result type (might be due to invalid IL or missing references)
		PrepareFields(Main.LocalPlayer);
		SpriteBatch spriteBatch = Main.spriteBatch;
		int num = 16;
		int num2 = 18;
		int num3 = Main.screenWidth - 300 - 22 + num;
		if (_drawTextStyle == 2)
		{
			num2 += 2;
			DrawLifeBarText(spriteBatch, new Vector2((float)num3, (float)num2));
			DrawManaText(spriteBatch);
		}
		else if (_drawTextStyle == 1)
		{
			num2 += 4;
			DrawLifeBarText(spriteBatch, new Vector2((float)num3, (float)num2));
		}
		Color color = default(Color);
		((Color)(ref color))._002Ector((int)Main.mouseTextColor, (int)Main.mouseTextColor, (int)Main.mouseTextColor, (int)Main.mouseTextColor);
		if (ResourceOverlayLoader.PreDrawResourceDisplay(preparedSnapshot, this, drawingLife: true, ref color, out var drawText))
		{
			Vector2 vector = default(Vector2);
			((Vector2)(ref vector))._002Ector((float)num3, (float)num2);
			vector.X += (_maxSegmentCount - _hpSegmentsCount) * _panelMiddleHP.Width();
			bool isHovered = false;
			ResourceDrawSettings resourceDrawSettings = default(ResourceDrawSettings);
			resourceDrawSettings.ElementCount = _hpSegmentsCount + 2;
			resourceDrawSettings.ElementIndexOffset = 0;
			resourceDrawSettings.TopLeftAnchor = vector;
			resourceDrawSettings.GetTextureMethod = LifePanelDrawer;
			resourceDrawSettings.OffsetPerDraw = Vector2.Zero;
			resourceDrawSettings.OffsetPerDrawByTexturePercentile = Vector2.UnitX;
			resourceDrawSettings.OffsetSpriteAnchor = Vector2.Zero;
			resourceDrawSettings.OffsetSpriteAnchorByTexturePercentile = Vector2.Zero;
			resourceDrawSettings.StatsSnapshot = preparedSnapshot;
			resourceDrawSettings.DisplaySet = this;
			resourceDrawSettings.ResourceIndexOffset = -1;
			resourceDrawSettings.Draw(spriteBatch, ref isHovered);
			resourceDrawSettings = default(ResourceDrawSettings);
			resourceDrawSettings.ElementCount = _hpSegmentsCount;
			resourceDrawSettings.ElementIndexOffset = 0;
			resourceDrawSettings.TopLeftAnchor = vector + new Vector2(6f, 6f);
			resourceDrawSettings.GetTextureMethod = LifeFillingDrawer;
			resourceDrawSettings.OffsetPerDraw = new Vector2((float)_hpFill.Width(), 0f);
			resourceDrawSettings.OffsetPerDrawByTexturePercentile = Vector2.Zero;
			resourceDrawSettings.OffsetSpriteAnchor = Vector2.Zero;
			resourceDrawSettings.OffsetSpriteAnchorByTexturePercentile = Vector2.Zero;
			resourceDrawSettings.StatsSnapshot = preparedSnapshot;
			resourceDrawSettings.DisplaySet = this;
			resourceDrawSettings.Draw(spriteBatch, ref isHovered);
			_hpHovered = isHovered;
		}
		ResourceOverlayLoader.PostDrawResourceDisplay(preparedSnapshot, this, drawingLife: true, color, drawText);
		((Color)(ref color))._002Ector((int)Main.mouseTextColor, (int)Main.mouseTextColor, (int)Main.mouseTextColor, (int)Main.mouseTextColor);
		if (ResourceOverlayLoader.PreDrawResourceDisplay(preparedSnapshot, this, drawingLife: false, ref color, out drawText))
		{
			bool isHovered = false;
			Vector2 vector2 = default(Vector2);
			((Vector2)(ref vector2))._002Ector((float)(num3 - 10), (float)(num2 + 24));
			vector2.X += (_maxSegmentCount - _mpSegmentsCount) * _panelMiddleMP.Width();
			ResourceDrawSettings resourceDrawSettings = default(ResourceDrawSettings);
			resourceDrawSettings.ElementCount = _mpSegmentsCount + 2;
			resourceDrawSettings.ElementIndexOffset = 0;
			resourceDrawSettings.TopLeftAnchor = vector2;
			resourceDrawSettings.GetTextureMethod = ManaPanelDrawer;
			resourceDrawSettings.OffsetPerDraw = Vector2.Zero;
			resourceDrawSettings.OffsetPerDrawByTexturePercentile = Vector2.UnitX;
			resourceDrawSettings.OffsetSpriteAnchor = Vector2.Zero;
			resourceDrawSettings.OffsetSpriteAnchorByTexturePercentile = Vector2.Zero;
			resourceDrawSettings.StatsSnapshot = preparedSnapshot;
			resourceDrawSettings.DisplaySet = this;
			resourceDrawSettings.ResourceIndexOffset = -1;
			resourceDrawSettings.Draw(spriteBatch, ref isHovered);
			resourceDrawSettings = default(ResourceDrawSettings);
			resourceDrawSettings.ElementCount = _mpSegmentsCount;
			resourceDrawSettings.ElementIndexOffset = 0;
			resourceDrawSettings.TopLeftAnchor = vector2 + new Vector2(6f, 6f);
			resourceDrawSettings.GetTextureMethod = ManaFillingDrawer;
			resourceDrawSettings.OffsetPerDraw = new Vector2((float)_mpFill.Width(), 0f);
			resourceDrawSettings.OffsetPerDrawByTexturePercentile = Vector2.Zero;
			resourceDrawSettings.OffsetSpriteAnchor = Vector2.Zero;
			resourceDrawSettings.OffsetSpriteAnchorByTexturePercentile = Vector2.Zero;
			resourceDrawSettings.StatsSnapshot = preparedSnapshot;
			resourceDrawSettings.DisplaySet = this;
			resourceDrawSettings.Draw(spriteBatch, ref isHovered);
			_mpHovered = isHovered;
		}
		ResourceOverlayLoader.PostDrawResourceDisplay(preparedSnapshot, this, drawingLife: false, color, drawText);
	}

	private static void DrawManaText(SpriteBatch spriteBatch)
	{
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		Color color = default(Color);
		((Color)(ref color))._002Ector((int)Main.mouseTextColor, (int)Main.mouseTextColor, (int)Main.mouseTextColor, (int)Main.mouseTextColor);
		int num = 180;
		Player localPlayer = Main.LocalPlayer;
		string text = Lang.inter[2].Value + ":";
		string text2 = localPlayer.statMana + "/" + localPlayer.statManaMax2;
		Vector2 vector = default(Vector2);
		((Vector2)(ref vector))._002Ector((float)(Main.screenWidth - num), 65f);
		string text3 = text + " " + text2;
		Vector2 vector2 = FontAssets.MouseText.Value.MeasureString(text3);
		spriteBatch.DrawString(FontAssets.MouseText.Value, text, vector + new Vector2((0f - vector2.X) * 0.5f, 0f), color, 0f, default(Vector2), 1f, (SpriteEffects)0, 0f);
		spriteBatch.DrawString(FontAssets.MouseText.Value, text2, vector + new Vector2(vector2.X * 0.5f, 0f), color, 0f, new Vector2(FontAssets.MouseText.Value.MeasureString(text2).X, 0f), 1f, (SpriteEffects)0, 0f);
	}

	private static void DrawLifeBarText(SpriteBatch spriteBatch, Vector2 topLeftAnchor)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Unknown result type (might be due to invalid IL or missing references)
		Vector2 vector = topLeftAnchor + new Vector2(130f, -20f);
		Player localPlayer = Main.LocalPlayer;
		Color color = default(Color);
		((Color)(ref color))._002Ector((int)Main.mouseTextColor, (int)Main.mouseTextColor, (int)Main.mouseTextColor, (int)Main.mouseTextColor);
		string text = Lang.inter[0].Value + " " + localPlayer.statLifeMax2 + "/" + localPlayer.statLifeMax2;
		Vector2 vector2 = FontAssets.MouseText.Value.MeasureString(text);
		spriteBatch.DrawString(FontAssets.MouseText.Value, Lang.inter[0].Value, vector + new Vector2((0f - vector2.X) * 0.5f, 0f), color, 0f, default(Vector2), 1f, (SpriteEffects)0, 0f);
		spriteBatch.DrawString(FontAssets.MouseText.Value, localPlayer.statLife + "/" + localPlayer.statLifeMax2, vector + new Vector2(vector2.X * 0.5f, 0f), color, 0f, new Vector2(FontAssets.MouseText.Value.MeasureString(localPlayer.statLife + "/" + localPlayer.statLifeMax2).X, 0f), 1f, (SpriteEffects)0, 0f);
	}

	private void PrepareFields(Player player)
	{
		PlayerStatsSnapshot playerStatsSnapshot = new PlayerStatsSnapshot(player);
		_hpSegmentsCount = playerStatsSnapshot.AmountOfLifeHearts;
		_mpSegmentsCount = playerStatsSnapshot.AmountOfManaStars;
		_maxSegmentCount = 20;
		_hpFruitCount = playerStatsSnapshot.LifeFruitCount;
		_hpPercent = (float)playerStatsSnapshot.Life / (float)playerStatsSnapshot.LifeMax;
		_mpPercent = (float)playerStatsSnapshot.Mana / (float)playerStatsSnapshot.ManaMax;
		preparedSnapshot = playerStatsSnapshot;
	}

	private void LifePanelDrawer(int elementIndex, int firstElementIndex, int lastElementIndex, out Asset<Texture2D> sprite, out Vector2 offset, out float drawScale, out Rectangle? sourceRect)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		sourceRect = null;
		offset = Vector2.Zero;
		sprite = _panelLeft;
		drawScale = 1f;
		if (elementIndex == lastElementIndex)
		{
			sprite = _panelRightHP;
			offset = new Vector2(-16f, -10f);
		}
		else if (elementIndex != firstElementIndex)
		{
			sprite = _panelMiddleHP;
			int drawIndexOffset = lastElementIndex - (elementIndex - firstElementIndex) - elementIndex;
			offset.X = drawIndexOffset * _panelMiddleHP.Width();
		}
	}

	private void ManaPanelDrawer(int elementIndex, int firstElementIndex, int lastElementIndex, out Asset<Texture2D> sprite, out Vector2 offset, out float drawScale, out Rectangle? sourceRect)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		sourceRect = null;
		offset = Vector2.Zero;
		sprite = _panelLeft;
		drawScale = 1f;
		if (elementIndex == lastElementIndex)
		{
			sprite = _panelRightMP;
			offset = new Vector2(-16f, -6f);
		}
		else if (elementIndex != firstElementIndex)
		{
			sprite = _panelMiddleMP;
			int drawIndexOffset = lastElementIndex - (elementIndex - firstElementIndex) - elementIndex;
			offset.X = drawIndexOffset * _panelMiddleMP.Width();
		}
	}

	private void LifeFillingDrawer(int elementIndex, int firstElementIndex, int lastElementIndex, out Asset<Texture2D> sprite, out Vector2 offset, out float drawScale, out Rectangle? sourceRect)
	{
		sprite = _hpFill;
		if (elementIndex < _hpFruitCount)
		{
			sprite = _hpFillHoney;
		}
		FillBarByValues(elementIndex, sprite, _hpSegmentsCount, _hpPercent, out offset, out drawScale, out sourceRect);
		int drawIndexOffset = lastElementIndex - (elementIndex - firstElementIndex) - elementIndex;
		offset.X += drawIndexOffset * sprite.Width();
	}

	public static void FillBarByValues(int elementIndex, Asset<Texture2D> sprite, int segmentsCount, float fillPercent, out Vector2 offset, out float drawScale, out Rectangle? sourceRect)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		sourceRect = null;
		offset = Vector2.Zero;
		float num = 1f;
		float num2 = 1f / (float)segmentsCount;
		num = Utils.GetLerpValue(num2 * (float)elementIndex, num2 * (float)(elementIndex + 1), fillPercent, clamped: true);
		drawScale = 1f;
		Rectangle value = sprite.Frame();
		int num3 = (int)((float)value.Width * (1f - num));
		offset.X += num3;
		value.X += num3;
		value.Width -= num3;
		sourceRect = value;
	}

	private void ManaFillingDrawer(int elementIndex, int firstElementIndex, int lastElementIndex, out Asset<Texture2D> sprite, out Vector2 offset, out float drawScale, out Rectangle? sourceRect)
	{
		sprite = _mpFill;
		FillBarByValues(elementIndex, sprite, _mpSegmentsCount, _mpPercent, out offset, out drawScale, out sourceRect);
		int drawIndexOffset = lastElementIndex - (elementIndex - firstElementIndex) - elementIndex;
		offset.X += drawIndexOffset * sprite.Width();
	}

	public void TryToHover()
	{
		if (_hpHovered)
		{
			CommonResourceBarMethods.DrawLifeMouseOver();
		}
		if (_mpHovered)
		{
			CommonResourceBarMethods.DrawManaMouseOver();
		}
	}
}
