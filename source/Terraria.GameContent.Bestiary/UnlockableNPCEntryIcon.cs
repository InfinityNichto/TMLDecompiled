using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Terraria.GameContent.Bestiary;

public class UnlockableNPCEntryIcon : IEntryIcon
{
	private int _npcNetId;

	private NPC _npcCache;

	private bool _firstUpdateDone;

	private Asset<Texture2D> _customTexture;

	private Vector2 _positionOffsetCache;

	private string _overrideNameKey;

	public UnlockableNPCEntryIcon(int npcNetId, float ai0 = 0f, float ai1 = 0f, float ai2 = 0f, float ai3 = 0f, string overrideNameKey = null)
	{
		_npcNetId = npcNetId;
		_npcCache = new NPC();
		_npcCache.IsABestiaryIconDummy = true;
		_npcCache.SetDefaults(_npcNetId);
		_firstUpdateDone = false;
		_npcCache.ai[0] = ai0;
		_npcCache.ai[1] = ai1;
		_npcCache.ai[2] = ai2;
		_npcCache.ai[3] = ai3;
		_customTexture = null;
		_overrideNameKey = overrideNameKey;
	}

	public IEntryIcon CreateClone()
	{
		return new UnlockableNPCEntryIcon(_npcNetId, 0f, 0f, 0f, 0f, _overrideNameKey);
	}

	public void Update(BestiaryUICollectionInfo providedInfo, Rectangle hitbox, EntryIconDrawSettings settings)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_0185: Unknown result type (might be due to invalid IL or missing references)
		Vector2 positionOffsetCache = default(Vector2);
		int? num = null;
		int? num2 = null;
		int? num3 = null;
		bool wet = false;
		float num4 = 0f;
		Asset<Texture2D> asset = null;
		if (NPCID.Sets.NPCBestiaryDrawOffset.TryGetValue(_npcNetId, out var value))
		{
			_npcCache.rotation = value.Rotation;
			_npcCache.scale = value.Scale;
			if (value.PortraitScale.HasValue && settings.IsPortrait)
			{
				_npcCache.scale = value.PortraitScale.Value;
			}
			positionOffsetCache = value.Position;
			num = value.Frame;
			num2 = value.Direction;
			num3 = value.SpriteDirection;
			num4 = value.Velocity;
			wet = value.IsWet;
			if (value.PortraitPositionXOverride.HasValue && settings.IsPortrait)
			{
				positionOffsetCache.X = value.PortraitPositionXOverride.Value;
			}
			if (value.PortraitPositionYOverride.HasValue && settings.IsPortrait)
			{
				positionOffsetCache.Y = value.PortraitPositionYOverride.Value;
			}
			if (value.CustomTexturePath != null)
			{
				asset = ModContent.Request<Texture2D>(value.CustomTexturePath);
			}
			if (asset != null && asset.IsLoaded)
			{
				_customTexture = asset;
			}
		}
		_positionOffsetCache = positionOffsetCache;
		UpdatePosition(settings);
		if (NPCID.Sets.TrailingMode[_npcCache.type] != -1)
		{
			for (int i = 0; i < _npcCache.oldPos.Length; i++)
			{
				_npcCache.oldPos[i] = _npcCache.position;
			}
		}
		_npcCache.direction = (_npcCache.spriteDirection = (num2.HasValue ? num2.Value : (-1)));
		if (num3.HasValue)
		{
			_npcCache.spriteDirection = num3.Value;
		}
		_npcCache.wet = wet;
		AdjustSpecialSpawnRulesForVisuals(settings);
		SimulateFirstHover(num4);
		if (!num.HasValue && (settings.IsPortrait || settings.IsHovered))
		{
			_npcCache.velocity.X = (float)_npcCache.direction * num4;
			_npcCache.FindFrame();
		}
		else if (num.HasValue)
		{
			_npcCache.FindFrame();
			_npcCache.frame.Y = _npcCache.frame.Height * num.Value;
		}
	}

	private void UpdatePosition(EntryIconDrawSettings settings)
	{
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		if (_npcCache.noGravity)
		{
			_npcCache.Center = ((Rectangle)(ref settings.iconbox)).Center.ToVector2() + _positionOffsetCache;
		}
		else
		{
			_npcCache.Bottom = settings.iconbox.TopLeft() + settings.iconbox.Size() * new Vector2(0.5f, 1f) + new Vector2(0f, -8f) + _positionOffsetCache;
		}
		_npcCache.position = _npcCache.position.Floor();
	}

	private void AdjustSpecialSpawnRulesForVisuals(EntryIconDrawSettings settings)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		if (NPCID.Sets.SpecialSpawningRules.TryGetValue(_npcNetId, out var value) && value == 0)
		{
			Point point = (_npcCache.position - _npcCache.rotation.ToRotationVector2() * -1600f).ToTileCoordinates();
			_npcCache.ai[0] = point.X;
			_npcCache.ai[1] = point.Y;
		}
		switch (_npcNetId)
		{
		case 244:
			_npcCache.AI_001_SetRainbowSlimeColor();
			break;
		case 356:
			_npcCache.ai[2] = 1f;
			break;
		case 330:
		case 372:
		case 586:
		case 587:
		case 619:
		case 620:
			_npcCache.alpha = 0;
			break;
		case 299:
		case 538:
		case 539:
		case 639:
		case 640:
		case 641:
		case 642:
		case 643:
		case 644:
		case 645:
			if (settings.IsPortrait && _npcCache.frame.Y == 0)
			{
				_npcCache.frame.Y = _npcCache.frame.Height;
			}
			break;
		case 636:
			_npcCache.Opacity = 1f;
			if ((_npcCache.localAI[0] += 1f) >= 44f)
			{
				_npcCache.localAI[0] = 0f;
			}
			break;
		case 656:
			_npcCache.townNpcVariationIndex = 1;
			break;
		case 670:
			_npcCache.townNpcVariationIndex = 0;
			break;
		}
	}

	private void SimulateFirstHover(float velocity)
	{
		if (!_firstUpdateDone)
		{
			_firstUpdateDone = true;
			_npcCache.SetFrameSize();
			_npcCache.velocity.X = (float)_npcCache.direction * velocity;
			for (int i = 0; i < 1; i++)
			{
				_npcCache.FindFrame();
			}
		}
	}

	public void Draw(BestiaryUICollectionInfo providedInfo, SpriteBatch spriteBatch, EntryIconDrawSettings settings)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		UpdatePosition(settings);
		if (_customTexture != null)
		{
			spriteBatch.Draw(_customTexture.Value, _npcCache.Center, (Rectangle?)null, Color.White, 0f, _customTexture.Size() / 2f, _npcCache.scale, (SpriteEffects)0, 0f);
			return;
		}
		if (_npcCache.townNPC && TownNPCProfiles.Instance.GetProfile(_npcCache, out var profile))
		{
			TextureAssets.Npc[_npcCache.type] = profile.GetTextureNPCShouldUse(_npcCache);
		}
		Main.instance.DrawNPCDirect(spriteBatch, _npcCache, _npcCache.behindTiles, Vector2.Zero);
	}

	public string GetHoverText(BestiaryUICollectionInfo providedInfo)
	{
		string result = Lang.GetNPCNameValue(_npcCache.netID);
		if (!string.IsNullOrWhiteSpace(_overrideNameKey))
		{
			result = Language.GetTextValue(_overrideNameKey);
		}
		if (GetUnlockState(providedInfo))
		{
			return result;
		}
		return "???";
	}

	public bool GetUnlockState(BestiaryUICollectionInfo providedInfo)
	{
		return providedInfo.UnlockState > BestiaryEntryUnlockState.NotKnownAtAll_0;
	}
}
