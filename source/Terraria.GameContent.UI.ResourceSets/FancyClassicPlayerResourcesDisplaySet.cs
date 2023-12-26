using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using ReLogic.Graphics;
using Terraria.DataStructures;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Terraria.GameContent.UI.ResourceSets;

public class FancyClassicPlayerResourcesDisplaySet : IPlayerResourcesDisplaySet, IConfigKeyHolder
{
	private PlayerStatsSnapshot preparedSnapshot;

	private ResourceDrawSettings defaultResourceDrawSettings;

	private float _currentPlayerLife;

	private float _lifePerHeart;

	private int _playerLifeFruitCount;

	private int _lastHeartFillingIndex;

	private int _lastHeartPanelIndex;

	private int _heartCountRow1;

	private int _heartCountRow2;

	private int _starCount;

	private int _lastStarFillingIndex;

	private float _manaPerStar;

	private float _currentPlayerMana;

	private Asset<Texture2D> _heartLeft;

	private Asset<Texture2D> _heartMiddle;

	private Asset<Texture2D> _heartRight;

	private Asset<Texture2D> _heartRightFancy;

	private Asset<Texture2D> _heartFill;

	private Asset<Texture2D> _heartFillHoney;

	private Asset<Texture2D> _heartSingleFancy;

	private Asset<Texture2D> _starTop;

	private Asset<Texture2D> _starMiddle;

	private Asset<Texture2D> _starBottom;

	private Asset<Texture2D> _starSingle;

	private Asset<Texture2D> _starFill;

	private bool _hoverLife;

	private bool _hoverMana;

	private bool _drawText;

	public string DisplayedName => Language.GetTextValue("UI.HealthManaStyle_" + NameKey);

	public string NameKey { get; private set; }

	public string ConfigKey { get; private set; }

	public FancyClassicPlayerResourcesDisplaySet(string nameKey, string configKey, string resourceFolderName, AssetRequestMode mode)
	{
		NameKey = nameKey;
		ConfigKey = configKey;
		if (configKey == "NewWithText")
		{
			_drawText = true;
		}
		else
		{
			_drawText = false;
		}
		string text = "Images\\UI\\PlayerResourceSets\\" + resourceFolderName;
		_heartLeft = Main.Assets.Request<Texture2D>(text + "\\Heart_Left", mode);
		_heartMiddle = Main.Assets.Request<Texture2D>(text + "\\Heart_Middle", mode);
		_heartRight = Main.Assets.Request<Texture2D>(text + "\\Heart_Right", mode);
		_heartRightFancy = Main.Assets.Request<Texture2D>(text + "\\Heart_Right_Fancy", mode);
		_heartFill = Main.Assets.Request<Texture2D>(text + "\\Heart_Fill", mode);
		_heartFillHoney = Main.Assets.Request<Texture2D>(text + "\\Heart_Fill_B", mode);
		_heartSingleFancy = Main.Assets.Request<Texture2D>(text + "\\Heart_Single_Fancy", mode);
		_starTop = Main.Assets.Request<Texture2D>(text + "\\Star_A", mode);
		_starMiddle = Main.Assets.Request<Texture2D>(text + "\\Star_B", mode);
		_starBottom = Main.Assets.Request<Texture2D>(text + "\\Star_C", mode);
		_starSingle = Main.Assets.Request<Texture2D>(text + "\\Star_Single", mode);
		_starFill = Main.Assets.Request<Texture2D>(text + "\\Star_Fill", mode);
	}

	public void Draw()
	{
		Player localPlayer = Main.LocalPlayer;
		SpriteBatch spriteBatch = Main.spriteBatch;
		PrepareFields(localPlayer);
		DrawLifeBar(spriteBatch);
		DrawManaBar(spriteBatch);
	}

	private void DrawLifeBar(SpriteBatch spriteBatch)
	{
		//IL_02e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0165: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0198: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0204: Unknown result type (might be due to invalid IL or missing references)
		//IL_0233: Unknown result type (might be due to invalid IL or missing references)
		//IL_023e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0243: Unknown result type (might be due to invalid IL or missing references)
		//IL_0252: Unknown result type (might be due to invalid IL or missing references)
		//IL_0257: Unknown result type (might be due to invalid IL or missing references)
		//IL_025c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0276: Unknown result type (might be due to invalid IL or missing references)
		//IL_0280: Unknown result type (might be due to invalid IL or missing references)
		//IL_0285: Unknown result type (might be due to invalid IL or missing references)
		//IL_028c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0291: Unknown result type (might be due to invalid IL or missing references)
		//IL_0298: Unknown result type (might be due to invalid IL or missing references)
		//IL_029d: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		Color color = default(Color);
		((Color)(ref color))._002Ector((int)Main.mouseTextColor, (int)Main.mouseTextColor, (int)Main.mouseTextColor, (int)Main.mouseTextColor);
		if (ResourceOverlayLoader.PreDrawResourceDisplay(preparedSnapshot, this, drawingLife: true, ref color, out var drawText))
		{
			Vector2 vector = default(Vector2);
			((Vector2)(ref vector))._002Ector((float)(Main.screenWidth - 300 + 4), 15f);
			if (_drawText)
			{
				vector.Y += 6f;
				DrawLifeBarText(spriteBatch, vector + new Vector2(-4f, 3f));
			}
			bool isHovered = false;
			ResourceDrawSettings resourceDrawSettings = defaultResourceDrawSettings;
			resourceDrawSettings.ElementCount = _heartCountRow1;
			resourceDrawSettings.ElementIndexOffset = 0;
			resourceDrawSettings.TopLeftAnchor = vector;
			resourceDrawSettings.GetTextureMethod = HeartPanelDrawer;
			resourceDrawSettings.OffsetPerDraw = Vector2.Zero;
			resourceDrawSettings.OffsetPerDrawByTexturePercentile = Vector2.UnitX;
			resourceDrawSettings.OffsetSpriteAnchor = Vector2.Zero;
			resourceDrawSettings.OffsetSpriteAnchorByTexturePercentile = Vector2.Zero;
			resourceDrawSettings.Draw(spriteBatch, ref isHovered);
			resourceDrawSettings = defaultResourceDrawSettings;
			resourceDrawSettings.ElementCount = _heartCountRow2;
			resourceDrawSettings.ElementIndexOffset = 10;
			resourceDrawSettings.TopLeftAnchor = vector + new Vector2(0f, 28f);
			resourceDrawSettings.GetTextureMethod = HeartPanelDrawer;
			resourceDrawSettings.OffsetPerDraw = Vector2.Zero;
			resourceDrawSettings.OffsetPerDrawByTexturePercentile = Vector2.UnitX;
			resourceDrawSettings.OffsetSpriteAnchor = Vector2.Zero;
			resourceDrawSettings.OffsetSpriteAnchorByTexturePercentile = Vector2.Zero;
			resourceDrawSettings.Draw(spriteBatch, ref isHovered);
			resourceDrawSettings = defaultResourceDrawSettings;
			resourceDrawSettings.ElementCount = _heartCountRow1;
			resourceDrawSettings.ElementIndexOffset = 0;
			resourceDrawSettings.TopLeftAnchor = vector + new Vector2(15f, 15f);
			resourceDrawSettings.GetTextureMethod = HeartFillingDrawer;
			resourceDrawSettings.OffsetPerDraw = Vector2.UnitX * 2f;
			resourceDrawSettings.OffsetPerDrawByTexturePercentile = Vector2.UnitX;
			resourceDrawSettings.OffsetSpriteAnchor = Vector2.Zero;
			resourceDrawSettings.OffsetSpriteAnchorByTexturePercentile = new Vector2(0.5f, 0.5f);
			resourceDrawSettings.Draw(spriteBatch, ref isHovered);
			resourceDrawSettings = defaultResourceDrawSettings;
			resourceDrawSettings.ElementCount = _heartCountRow2;
			resourceDrawSettings.ElementIndexOffset = 10;
			resourceDrawSettings.TopLeftAnchor = vector + new Vector2(15f, 15f) + new Vector2(0f, 28f);
			resourceDrawSettings.GetTextureMethod = HeartFillingDrawer;
			resourceDrawSettings.OffsetPerDraw = Vector2.UnitX * 2f;
			resourceDrawSettings.OffsetPerDrawByTexturePercentile = Vector2.UnitX;
			resourceDrawSettings.OffsetSpriteAnchor = Vector2.Zero;
			resourceDrawSettings.OffsetSpriteAnchorByTexturePercentile = new Vector2(0.5f, 0.5f);
			resourceDrawSettings.Draw(spriteBatch, ref isHovered);
			_hoverLife = isHovered && ResourceOverlayLoader.DisplayHoverText(preparedSnapshot, this, drawingLife: true);
		}
		ResourceOverlayLoader.PostDrawResourceDisplay(preparedSnapshot, this, drawingLife: true, color, drawText);
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
		Vector2 vector = topLeftAnchor + new Vector2(130f, -24f);
		Player localPlayer = Main.LocalPlayer;
		Color color = default(Color);
		((Color)(ref color))._002Ector((int)Main.mouseTextColor, (int)Main.mouseTextColor, (int)Main.mouseTextColor, (int)Main.mouseTextColor);
		string text = Lang.inter[0].Value + " " + localPlayer.statLifeMax2 + "/" + localPlayer.statLifeMax2;
		Vector2 vector2 = FontAssets.MouseText.Value.MeasureString(text);
		spriteBatch.DrawString(FontAssets.MouseText.Value, Lang.inter[0].Value, vector + new Vector2((0f - vector2.X) * 0.5f, 0f), color, 0f, default(Vector2), 1f, (SpriteEffects)0, 0f);
		spriteBatch.DrawString(FontAssets.MouseText.Value, localPlayer.statLife + "/" + localPlayer.statLifeMax2, vector + new Vector2(vector2.X * 0.5f, 0f), color, 0f, new Vector2(FontAssets.MouseText.Value.MeasureString(localPlayer.statLife + "/" + localPlayer.statLifeMax2).X, 0f), 1f, (SpriteEffects)0, 0f);
	}

	private void DrawManaBar(SpriteBatch spriteBatch)
	{
		//IL_017c: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		Color color = default(Color);
		((Color)(ref color))._002Ector((int)Main.mouseTextColor, (int)Main.mouseTextColor, (int)Main.mouseTextColor, (int)Main.mouseTextColor);
		if (ResourceOverlayLoader.PreDrawResourceDisplay(preparedSnapshot, this, drawingLife: false, ref color, out var drawText))
		{
			Vector2 vector = default(Vector2);
			((Vector2)(ref vector))._002Ector((float)(Main.screenWidth - 40), 22f);
			_ = _starCount;
			bool isHovered = false;
			ResourceDrawSettings resourceDrawSettings = defaultResourceDrawSettings;
			resourceDrawSettings.ElementCount = _starCount;
			resourceDrawSettings.ElementIndexOffset = 0;
			resourceDrawSettings.TopLeftAnchor = vector;
			resourceDrawSettings.GetTextureMethod = StarPanelDrawer;
			resourceDrawSettings.OffsetPerDraw = Vector2.Zero;
			resourceDrawSettings.OffsetPerDrawByTexturePercentile = Vector2.UnitY;
			resourceDrawSettings.OffsetSpriteAnchor = Vector2.Zero;
			resourceDrawSettings.OffsetSpriteAnchorByTexturePercentile = Vector2.Zero;
			resourceDrawSettings.Draw(spriteBatch, ref isHovered);
			resourceDrawSettings = defaultResourceDrawSettings;
			resourceDrawSettings.ElementCount = _starCount;
			resourceDrawSettings.ElementIndexOffset = 0;
			resourceDrawSettings.TopLeftAnchor = vector + new Vector2(15f, 16f);
			resourceDrawSettings.GetTextureMethod = StarFillingDrawer;
			resourceDrawSettings.OffsetPerDraw = Vector2.UnitY * -2f;
			resourceDrawSettings.OffsetPerDrawByTexturePercentile = Vector2.UnitY;
			resourceDrawSettings.OffsetSpriteAnchor = Vector2.Zero;
			resourceDrawSettings.OffsetSpriteAnchorByTexturePercentile = new Vector2(0.5f, 0.5f);
			resourceDrawSettings.Draw(spriteBatch, ref isHovered);
			_hoverMana = isHovered && ResourceOverlayLoader.DisplayHoverText(preparedSnapshot, this, drawingLife: false);
		}
		ResourceOverlayLoader.PostDrawResourceDisplay(preparedSnapshot, this, drawingLife: false, color, drawText);
	}

	private static void DrawManaText(SpriteBatch spriteBatch)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		Vector2 vector = FontAssets.MouseText.Value.MeasureString(Lang.inter[2].Value);
		Color color = default(Color);
		((Color)(ref color))._002Ector((int)Main.mouseTextColor, (int)Main.mouseTextColor, (int)Main.mouseTextColor, (int)Main.mouseTextColor);
		int num = 50;
		if (vector.X >= 45f)
		{
			num = (int)vector.X + 5;
		}
		spriteBatch.DrawString(FontAssets.MouseText.Value, Lang.inter[2].Value, new Vector2((float)(Main.screenWidth - num), 6f), color, 0f, default(Vector2), 1f, (SpriteEffects)0, 0f);
	}

	private void HeartPanelDrawer(int elementIndex, int firstElementIndex, int lastElementIndex, out Asset<Texture2D> sprite, out Vector2 offset, out float drawScale, out Rectangle? sourceRect)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		sourceRect = null;
		offset = Vector2.Zero;
		sprite = _heartLeft;
		drawScale = 1f;
		if (elementIndex == lastElementIndex && elementIndex == firstElementIndex)
		{
			sprite = _heartSingleFancy;
			offset = new Vector2(-4f, -4f);
		}
		else if (elementIndex == lastElementIndex && lastElementIndex == _lastHeartPanelIndex)
		{
			sprite = _heartRightFancy;
			offset = new Vector2(-8f, -4f);
		}
		else if (elementIndex == lastElementIndex)
		{
			sprite = _heartRight;
		}
		else if (elementIndex != firstElementIndex)
		{
			sprite = _heartMiddle;
		}
	}

	private void HeartFillingDrawer(int elementIndex, int firstElementIndex, int lastElementIndex, out Asset<Texture2D> sprite, out Vector2 offset, out float drawScale, out Rectangle? sourceRect)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		sourceRect = null;
		offset = Vector2.Zero;
		sprite = _heartLeft;
		if (elementIndex < _playerLifeFruitCount)
		{
			sprite = _heartFillHoney;
		}
		else
		{
			sprite = _heartFill;
		}
		float num = (drawScale = Utils.GetLerpValue(_lifePerHeart * (float)elementIndex, _lifePerHeart * (float)(elementIndex + 1), _currentPlayerLife, clamped: true));
		if (elementIndex == _lastHeartFillingIndex && num > 0f)
		{
			drawScale += Main.cursorScale - 1f;
		}
	}

	private void StarPanelDrawer(int elementIndex, int firstElementIndex, int lastElementIndex, out Asset<Texture2D> sprite, out Vector2 offset, out float drawScale, out Rectangle? sourceRect)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		sourceRect = null;
		offset = Vector2.Zero;
		sprite = _starTop;
		drawScale = 1f;
		if (elementIndex == lastElementIndex && elementIndex == firstElementIndex)
		{
			sprite = _starSingle;
		}
		else if (elementIndex == lastElementIndex)
		{
			sprite = _starBottom;
			offset = new Vector2(0f, 0f);
		}
		else if (elementIndex != firstElementIndex)
		{
			sprite = _starMiddle;
		}
	}

	private void StarFillingDrawer(int elementIndex, int firstElementIndex, int lastElementIndex, out Asset<Texture2D> sprite, out Vector2 offset, out float drawScale, out Rectangle? sourceRect)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		sourceRect = null;
		offset = Vector2.Zero;
		sprite = _starFill;
		float num = (drawScale = Utils.GetLerpValue(_manaPerStar * (float)elementIndex, _manaPerStar * (float)(elementIndex + 1), _currentPlayerMana, clamped: true));
		if (elementIndex == _lastStarFillingIndex && num > 0f)
		{
			drawScale += Main.cursorScale - 1f;
		}
	}

	private void PrepareFields(Player player)
	{
		PlayerStatsSnapshot playerStatsSnapshot = new PlayerStatsSnapshot(player);
		_playerLifeFruitCount = playerStatsSnapshot.LifeFruitCount;
		_lifePerHeart = playerStatsSnapshot.LifePerSegment;
		_currentPlayerLife = playerStatsSnapshot.Life;
		_manaPerStar = playerStatsSnapshot.ManaPerSegment;
		_heartCountRow1 = Utils.Clamp(playerStatsSnapshot.AmountOfLifeHearts, 0, 10);
		_heartCountRow2 = Utils.Clamp(playerStatsSnapshot.AmountOfLifeHearts - 10, 0, 10);
		int lastHeartFillingIndex = (int)((float)playerStatsSnapshot.Life / _lifePerHeart);
		_lastHeartFillingIndex = lastHeartFillingIndex;
		_lastHeartPanelIndex = _heartCountRow1 + _heartCountRow2 - 1;
		_starCount = playerStatsSnapshot.AmountOfManaStars;
		_currentPlayerMana = playerStatsSnapshot.Mana;
		_lastStarFillingIndex = (int)(_currentPlayerMana / _manaPerStar);
		preparedSnapshot = playerStatsSnapshot;
		defaultResourceDrawSettings = default(ResourceDrawSettings);
		defaultResourceDrawSettings.StatsSnapshot = preparedSnapshot;
		defaultResourceDrawSettings.DisplaySet = this;
	}

	public void TryToHover()
	{
		if (_hoverLife)
		{
			CommonResourceBarMethods.DrawLifeMouseOver();
		}
		if (_hoverMana)
		{
			CommonResourceBarMethods.DrawManaMouseOver();
		}
	}
}
