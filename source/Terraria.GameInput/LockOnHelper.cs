using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.ID;

namespace Terraria.GameInput;

public class LockOnHelper
{
	public enum LockOnMode
	{
		FocusTarget,
		TargetClosest,
		ThreeDS
	}

	private const float LOCKON_RANGE = 2000f;

	private const int LOCKON_HOLD_LIFETIME = 40;

	public static LockOnMode UseMode = LockOnMode.ThreeDS;

	private static bool _enabled;

	private static bool _canLockOn;

	private static List<int> _targets = new List<int>();

	private static int _pickedTarget;

	private static int _lifeTimeCounter;

	private static int _lifeTimeArrowDisplay;

	private static int _threeDSTarget = -1;

	private static int _targetClosestTarget = -1;

	public static bool ForceUsability = false;

	private static float[,] _drawProgress = new float[200, 2];

	public static NPC AimedTarget
	{
		get
		{
			if (_pickedTarget == -1 || _targets.Count < 1)
			{
				return null;
			}
			return Main.npc[_targets[_pickedTarget]];
		}
	}

	public static Vector2 PredictedPosition
	{
		get
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_014c: Unknown result type (might be due to invalid IL or missing references)
			//IL_014f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0154: Unknown result type (might be due to invalid IL or missing references)
			//IL_0159: Unknown result type (might be due to invalid IL or missing references)
			//IL_015b: Unknown result type (might be due to invalid IL or missing references)
			//IL_015d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0162: Unknown result type (might be due to invalid IL or missing references)
			//IL_0167: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
			NPC aimedTarget = AimedTarget;
			if (aimedTarget == null)
			{
				return Vector2.Zero;
			}
			Vector2 vector = aimedTarget.Center;
			if (NPC.GetNPCLocation(_targets[_pickedTarget], seekHead: true, averageDirection: false, out var index, out var pos))
			{
				vector = pos;
				vector += Main.npc[index].Distance(Main.player[Main.myPlayer].Center) / 2000f * Main.npc[index].velocity * 45f;
			}
			Player player = Main.player[Main.myPlayer];
			int num = ItemID.Sets.LockOnAimAbove[player.inventory[player.selectedItem].type];
			while (num > 0 && vector.Y > 100f)
			{
				Point point = vector.ToTileCoordinates();
				point.Y -= 4;
				if (!WorldGen.InWorld(point.X, point.Y, 10) || WorldGen.SolidTile(point.X, point.Y))
				{
					break;
				}
				vector.Y -= 16f;
				num--;
			}
			float? num2 = ItemID.Sets.LockOnAimCompensation[player.inventory[player.selectedItem].type];
			if (num2.HasValue)
			{
				vector.Y -= aimedTarget.height / 2;
				Vector2 v = vector - player.Center;
				Vector2 vector2 = v.SafeNormalize(Vector2.Zero);
				vector2.Y -= 1f;
				float num3 = ((Vector2)(ref v)).Length();
				num3 = (float)Math.Pow(num3 / 700f, 2.0) * 700f;
				vector.Y += vector2.Y * num3 * num2.Value * 1f;
				vector.X += (0f - vector2.X) * num3 * num2.Value * 1f;
			}
			return vector;
		}
	}

	public static bool Enabled => _enabled;

	public static void CycleUseModes()
	{
		switch (UseMode)
		{
		case LockOnMode.FocusTarget:
			UseMode = LockOnMode.TargetClosest;
			break;
		case LockOnMode.TargetClosest:
			UseMode = LockOnMode.ThreeDS;
			break;
		case LockOnMode.ThreeDS:
			UseMode = LockOnMode.TargetClosest;
			break;
		}
	}

	public static void Update()
	{
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		_canLockOn = false;
		if (!CanUseLockonSystem())
		{
			SetActive(on: false);
			return;
		}
		if (--_lifeTimeArrowDisplay < 0)
		{
			_lifeTimeArrowDisplay = 0;
		}
		FindMostViableTarget(LockOnMode.ThreeDS, ref _threeDSTarget);
		FindMostViableTarget(LockOnMode.TargetClosest, ref _targetClosestTarget);
		if (PlayerInput.Triggers.JustPressed.LockOn && !PlayerInput.WritingText)
		{
			_lifeTimeCounter = 40;
			_lifeTimeArrowDisplay = 30;
			HandlePressing();
		}
		if (!_enabled)
		{
			return;
		}
		if (UseMode == LockOnMode.FocusTarget && PlayerInput.Triggers.Current.LockOn)
		{
			if (_lifeTimeCounter <= 0)
			{
				SetActive(on: false);
				return;
			}
			_lifeTimeCounter--;
		}
		NPC aimedTarget = AimedTarget;
		if (!ValidTarget(aimedTarget))
		{
			SetActive(on: false);
		}
		if (UseMode == LockOnMode.TargetClosest)
		{
			SetActive(on: false);
			SetActive(CanEnable());
		}
		if (_enabled)
		{
			Player player = Main.player[Main.myPlayer];
			Vector2 predictedPosition = PredictedPosition;
			bool flag = false;
			if (ShouldLockOn(player) && (ItemID.Sets.LockOnIgnoresCollision[player.inventory[player.selectedItem].type] || Collision.CanHit(player.Center, 0, 0, predictedPosition, 0, 0) || Collision.CanHitLine(player.Center, 0, 0, predictedPosition, 0, 0) || Collision.CanHit(player.Center, 0, 0, aimedTarget.Center, 0, 0) || Collision.CanHitLine(player.Center, 0, 0, aimedTarget.Center, 0, 0)))
			{
				flag = true;
			}
			if (flag)
			{
				_canLockOn = true;
			}
		}
	}

	public static bool CanUseLockonSystem()
	{
		if (!ForceUsability)
		{
			return PlayerInput.UsingGamepad;
		}
		return true;
	}

	public static void SetUP()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		if (_canLockOn)
		{
			_ = AimedTarget;
			SetLockPosition(Main.ReverseGravitySupport(PredictedPosition - Main.screenPosition));
		}
	}

	public static void SetDOWN()
	{
		if (_canLockOn)
		{
			ResetLockPosition();
		}
	}

	private static bool ShouldLockOn(Player p)
	{
		if (p.inventory[p.selectedItem].type == 496)
		{
			return false;
		}
		return true;
	}

	public static void Toggle(bool forceOff = false)
	{
		_lifeTimeCounter = 40;
		_lifeTimeArrowDisplay = 30;
		HandlePressing();
		if (forceOff)
		{
			_enabled = false;
		}
	}

	private static void FindMostViableTarget(LockOnMode context, ref int targetVar)
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		targetVar = -1;
		if (UseMode == context && CanUseLockonSystem())
		{
			List<int> t = new List<int>();
			int t2 = -1;
			Utils.Swap(ref t, ref _targets);
			Utils.Swap(ref t2, ref _pickedTarget);
			RefreshTargets(Main.MouseWorld, 2000f);
			GetClosestTarget(Main.MouseWorld);
			Utils.Swap(ref t, ref _targets);
			Utils.Swap(ref t2, ref _pickedTarget);
			if (t2 >= 0)
			{
				targetVar = t[t2];
			}
			t.Clear();
		}
	}

	private static void HandlePressing()
	{
		if (UseMode == LockOnMode.TargetClosest)
		{
			SetActive(!_enabled);
		}
		else if (UseMode == LockOnMode.ThreeDS)
		{
			if (!_enabled)
			{
				SetActive(on: true);
			}
			else
			{
				CycleTargetThreeDS();
			}
		}
		else if (!_enabled)
		{
			SetActive(on: true);
		}
		else
		{
			CycleTargetFocus();
		}
	}

	private static void CycleTargetFocus()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		int num = _targets[_pickedTarget];
		RefreshTargets(Main.MouseWorld, 2000f);
		if (_targets.Count < 1 || (_targets.Count == 1 && num == _targets[0]))
		{
			SetActive(on: false);
			return;
		}
		_pickedTarget = 0;
		for (int i = 0; i < _targets.Count; i++)
		{
			if (_targets[i] > num)
			{
				_pickedTarget = i;
				break;
			}
		}
	}

	private static void CycleTargetThreeDS()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		int num = _targets[_pickedTarget];
		RefreshTargets(Main.MouseWorld, 2000f);
		GetClosestTarget(Main.MouseWorld);
		if (_targets.Count < 1 || (_targets.Count == 1 && num == _targets[0]) || num == _targets[_pickedTarget])
		{
			SetActive(on: false);
		}
	}

	private static bool CanEnable()
	{
		if (Main.player[Main.myPlayer].dead)
		{
			return false;
		}
		return true;
	}

	private static void SetActive(bool on)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		if (on)
		{
			if (CanEnable())
			{
				RefreshTargets(Main.MouseWorld, 2000f);
				GetClosestTarget(Main.MouseWorld);
				if (_pickedTarget >= 0)
				{
					_enabled = true;
				}
			}
		}
		else
		{
			_enabled = false;
			_targets.Clear();
			_lifeTimeCounter = 0;
			_threeDSTarget = -1;
			_targetClosestTarget = -1;
		}
	}

	private static void RefreshTargets(Vector2 position, float radius)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		_targets.Clear();
		Rectangle rectangle = Utils.CenteredRectangle(Main.player[Main.myPlayer].Center, new Vector2(1920f, 1200f));
		_ = Main.player[Main.myPlayer].Center;
		Main.player[Main.myPlayer].DirectionTo(Main.MouseWorld);
		for (int i = 0; i < Main.npc.Length; i++)
		{
			NPC nPC = Main.npc[i];
			if (ValidTarget(nPC) && !(nPC.Distance(position) > radius) && ((Rectangle)(ref rectangle)).Intersects(nPC.Hitbox))
			{
				Vector3 subLight = Lighting.GetSubLight(nPC.Center);
				if (!(((Vector3)(ref subLight)).Length() / 3f < 0.03f))
				{
					_targets.Add(i);
				}
			}
		}
	}

	private static void GetClosestTarget(Vector2 position)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		_pickedTarget = -1;
		float num = -1f;
		if (UseMode == LockOnMode.ThreeDS)
		{
			Vector2 center = Main.player[Main.myPlayer].Center;
			Vector2 value = Main.player[Main.myPlayer].DirectionTo(Main.MouseWorld);
			for (int i = 0; i < _targets.Count; i++)
			{
				int num2 = _targets[i];
				NPC obj = Main.npc[num2];
				float num3 = Vector2.Dot(obj.DirectionFrom(center), value);
				if (ValidTarget(obj) && (_pickedTarget == -1 || !(num3 <= num)))
				{
					_pickedTarget = i;
					num = num3;
				}
			}
			return;
		}
		for (int j = 0; j < _targets.Count; j++)
		{
			int num4 = _targets[j];
			NPC nPC = Main.npc[num4];
			if (ValidTarget(nPC) && (_pickedTarget == -1 || !(nPC.Distance(position) >= num)))
			{
				_pickedTarget = j;
				num = nPC.Distance(position);
			}
		}
	}

	private static bool ValidTarget(NPC n)
	{
		if (n == null || !n.active || n.dontTakeDamage || n.friendly || n.isLikeATownNPC || n.life < 1 || n.immortal)
		{
			return false;
		}
		if (n.aiStyle == 25 && n.ai[0] == 0f)
		{
			return false;
		}
		return true;
	}

	private static void SetLockPosition(Vector2 position)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		PlayerInput.LockOnCachePosition();
		Main.mouseX = (PlayerInput.MouseX = (int)position.X);
		Main.mouseY = (PlayerInput.MouseY = (int)position.Y);
	}

	private static void ResetLockPosition()
	{
		PlayerInput.LockOnUnCachePosition();
		Main.mouseX = PlayerInput.MouseX;
		Main.mouseY = PlayerInput.MouseY;
	}

	public static void Draw(SpriteBatch spriteBatch)
	{
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_024c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0264: Unknown result type (might be due to invalid IL or missing references)
		//IL_026b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0270: Unknown result type (might be due to invalid IL or missing references)
		//IL_0275: Unknown result type (might be due to invalid IL or missing references)
		//IL_027a: Unknown result type (might be due to invalid IL or missing references)
		//IL_027f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0281: Unknown result type (might be due to invalid IL or missing references)
		//IL_0291: Unknown result type (might be due to invalid IL or missing references)
		//IL_0296: Unknown result type (might be due to invalid IL or missing references)
		//IL_029a: Unknown result type (might be due to invalid IL or missing references)
		//IL_029c: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_0301: Unknown result type (might be due to invalid IL or missing references)
		//IL_0303: Unknown result type (might be due to invalid IL or missing references)
		//IL_0309: Unknown result type (might be due to invalid IL or missing references)
		//IL_030d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0314: Unknown result type (might be due to invalid IL or missing references)
		//IL_031e: Unknown result type (might be due to invalid IL or missing references)
		//IL_031f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0329: Unknown result type (might be due to invalid IL or missing references)
		//IL_0338: Unknown result type (might be due to invalid IL or missing references)
		//IL_033f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0346: Unknown result type (might be due to invalid IL or missing references)
		//IL_0353: Unknown result type (might be due to invalid IL or missing references)
		//IL_035d: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_046a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0474: Unknown result type (might be due to invalid IL or missing references)
		//IL_047a: Unknown result type (might be due to invalid IL or missing references)
		//IL_047c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0481: Unknown result type (might be due to invalid IL or missing references)
		//IL_0483: Unknown result type (might be due to invalid IL or missing references)
		//IL_0485: Unknown result type (might be due to invalid IL or missing references)
		//IL_0487: Unknown result type (might be due to invalid IL or missing references)
		//IL_048c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0491: Unknown result type (might be due to invalid IL or missing references)
		//IL_0496: Unknown result type (might be due to invalid IL or missing references)
		//IL_0498: Unknown result type (might be due to invalid IL or missing references)
		//IL_049f: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0434: Unknown result type (might be due to invalid IL or missing references)
		//IL_0436: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04db: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0503: Unknown result type (might be due to invalid IL or missing references)
		//IL_050a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0517: Unknown result type (might be due to invalid IL or missing references)
		//IL_0521: Unknown result type (might be due to invalid IL or missing references)
		//IL_0533: Unknown result type (might be due to invalid IL or missing references)
		//IL_0535: Unknown result type (might be due to invalid IL or missing references)
		//IL_053b: Unknown result type (might be due to invalid IL or missing references)
		//IL_053f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0546: Unknown result type (might be due to invalid IL or missing references)
		//IL_054d: Unknown result type (might be due to invalid IL or missing references)
		//IL_054e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0558: Unknown result type (might be due to invalid IL or missing references)
		//IL_0567: Unknown result type (might be due to invalid IL or missing references)
		//IL_056e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0575: Unknown result type (might be due to invalid IL or missing references)
		//IL_0582: Unknown result type (might be due to invalid IL or missing references)
		//IL_058c: Unknown result type (might be due to invalid IL or missing references)
		if (Main.gameMenu)
		{
			return;
		}
		Texture2D value = TextureAssets.LockOnCursor.Value;
		Rectangle rectangle = default(Rectangle);
		((Rectangle)(ref rectangle))._002Ector(0, 0, value.Width, 12);
		Rectangle rectangle2 = default(Rectangle);
		((Rectangle)(ref rectangle2))._002Ector(0, 16, value.Width, 12);
		Color t = Main.OurFavoriteColor.MultiplyRGBA(new Color(0.75f, 0.75f, 0.75f, 1f));
		((Color)(ref t)).A = 220;
		Color t2 = Main.OurFavoriteColor;
		((Color)(ref t2)).A = 220;
		float num = 0.94f + (float)Math.Sin(Main.GlobalTimeWrappedHourly * ((float)Math.PI * 2f)) * 0.06f;
		t2 *= num;
		t *= num;
		Utils.Swap(ref t, ref t2);
		Color color = t.MultiplyRGBA(new Color(0.8f, 0.8f, 0.8f, 0.8f));
		Color color2 = t.MultiplyRGBA(new Color(0.8f, 0.8f, 0.8f, 0.8f));
		float gravDir = Main.player[Main.myPlayer].gravDir;
		float num7 = 1f;
		float num8 = 0.1f;
		float num9 = 0.8f;
		float num10 = 1f;
		float num11 = 10f;
		float num12 = 10f;
		bool flag = false;
		for (int i = 0; i < _drawProgress.GetLength(0); i++)
		{
			int num13 = 0;
			if (_pickedTarget != -1 && _targets.Count > 0 && i == _targets[_pickedTarget])
			{
				num13 = 2;
			}
			else if ((flag && _targets.Contains(i)) || (UseMode == LockOnMode.ThreeDS && _threeDSTarget == i) || (UseMode == LockOnMode.TargetClosest && _targetClosestTarget == i))
			{
				num13 = 1;
			}
			_drawProgress[i, 0] = MathHelper.Clamp(_drawProgress[i, 0] + ((num13 == 1) ? num8 : (0f - num8)), 0f, 1f);
			_drawProgress[i, 1] = MathHelper.Clamp(_drawProgress[i, 1] + ((num13 == 2) ? num8 : (0f - num8)), 0f, 1f);
			float num14 = _drawProgress[i, 0];
			if (num14 > 0f)
			{
				float num2 = 1f - num14 * num14;
				Vector2 pos = Main.npc[i].Top + new Vector2(0f, 0f - num12 - num2 * num11) * gravDir - Main.screenPosition;
				pos = Main.ReverseGravitySupport(pos, (float)Main.npc[i].height);
				spriteBatch.Draw(value, pos, (Rectangle?)rectangle, color * num14, 0f, rectangle.Size() / 2f, new Vector2(0.58f, 1f) * num7 * num9 * (1f + num14) / 2f, (SpriteEffects)0, 0f);
				spriteBatch.Draw(value, pos, (Rectangle?)rectangle2, color2 * num14 * num14, 0f, rectangle2.Size() / 2f, new Vector2(0.58f, 1f) * num7 * num9 * (1f + num14) / 2f, (SpriteEffects)0, 0f);
			}
			float num3 = _drawProgress[i, 1];
			if (num3 > 0f)
			{
				int num4 = Main.npc[i].width;
				if (Main.npc[i].height > num4)
				{
					num4 = Main.npc[i].height;
				}
				num4 += 20;
				if ((float)num4 < 70f)
				{
					num10 *= (float)num4 / 70f;
				}
				float num5 = 3f;
				Vector2 vector = Main.npc[i].Center;
				if (_targets.Count >= 0 && _pickedTarget >= 0 && _pickedTarget < _targets.Count && i == _targets[_pickedTarget] && NPC.GetNPCLocation(i, seekHead: true, averageDirection: false, out var _, out var pos2))
				{
					vector = pos2;
				}
				for (int j = 0; (float)j < num5; j++)
				{
					float num6 = (float)Math.PI * 2f / num5 * (float)j + Main.GlobalTimeWrappedHourly * ((float)Math.PI * 2f) * 0.25f;
					Vector2 vector2 = Utils.RotatedBy(new Vector2(0f, (float)(num4 / 2)), num6);
					Vector2 pos3 = vector + vector2 - Main.screenPosition;
					pos3 = Main.ReverseGravitySupport(pos3);
					float rotation = num6 * (float)((gravDir == 1f) ? 1 : (-1)) + (float)Math.PI * (float)((gravDir == 1f) ? 1 : 0);
					spriteBatch.Draw(value, pos3, (Rectangle?)rectangle, t * num3, rotation, rectangle.Size() / 2f, new Vector2(0.58f, 1f) * num7 * num10 * (1f + num3) / 2f, (SpriteEffects)0, 0f);
					spriteBatch.Draw(value, pos3, (Rectangle?)rectangle2, t2 * num3 * num3, rotation, rectangle2.Size() / 2f, new Vector2(0.58f, 1f) * num7 * num10 * (1f + num3) / 2f, (SpriteEffects)0, 0f);
				}
			}
		}
	}
}
