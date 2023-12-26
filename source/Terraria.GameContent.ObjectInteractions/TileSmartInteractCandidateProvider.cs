using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace Terraria.GameContent.ObjectInteractions;

public class TileSmartInteractCandidateProvider : ISmartInteractCandidateProvider
{
	private class ReusableCandidate : ISmartInteractCandidate
	{
		private bool _strictSettings;

		private int _aimedX;

		private int _aimedY;

		private int _hx;

		private int _hy;

		private int _lx;

		private int _ly;

		public float DistanceFromCursor { get; private set; }

		public void Reuse(bool strictSettings, float distanceFromCursor, int AimedX, int AimedY, int LX, int LY, int HX, int HY)
		{
			DistanceFromCursor = distanceFromCursor;
			_strictSettings = strictSettings;
			_aimedX = AimedX;
			_aimedY = AimedY;
			_lx = LX;
			_ly = LY;
			_hx = HX;
			_hy = HY;
		}

		public void WinCandidacy()
		{
			Main.SmartInteractX = _aimedX;
			Main.SmartInteractY = _aimedY;
			if (_strictSettings)
			{
				Main.SmartInteractShowingFake = Main.SmartInteractTileCoords.Count > 0;
			}
			else
			{
				Main.SmartInteractShowingGenuine = true;
			}
			Main.TileInteractionLX = _lx - 10;
			Main.TileInteractionLY = _ly - 10;
			Main.TileInteractionHX = _hx + 10;
			Main.TileInteractionHY = _hy + 10;
		}
	}

	private List<Tuple<int, int>> targets = new List<Tuple<int, int>>();

	private ReusableCandidate _candidate = new ReusableCandidate();

	public void ClearSelfAndPrepareForCheck()
	{
		Main.SmartInteractX = -1;
		Main.SmartInteractY = -1;
		Main.TileInteractionLX = -1;
		Main.TileInteractionHX = -1;
		Main.TileInteractionLY = -1;
		Main.TileInteractionHY = -1;
		Main.SmartInteractTileCoords.Clear();
		Main.SmartInteractTileCoordsSelected.Clear();
		targets.Clear();
	}

	public bool ProvideCandidate(SmartInteractScanSettings settings, out ISmartInteractCandidate candidate)
	{
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_0714: Unknown result type (might be due to invalid IL or missing references)
		//IL_071a: Unknown result type (might be due to invalid IL or missing references)
		//IL_071f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0725: Unknown result type (might be due to invalid IL or missing references)
		//IL_05cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_05fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0637: Unknown result type (might be due to invalid IL or missing references)
		//IL_0649: Unknown result type (might be due to invalid IL or missing references)
		//IL_0661: Unknown result type (might be due to invalid IL or missing references)
		candidate = null;
		Point point = settings.mousevec.ToTileCoordinates();
		FillPotentialTargetTiles(settings);
		int num = -1;
		int num6 = -1;
		int num7 = -1;
		int num8 = -1;
		if (targets.Count > 0)
		{
			float num9 = -1f;
			Tuple<int, int> tuple = targets[0];
			for (int i = 0; i < targets.Count; i++)
			{
				float num10 = Vector2.Distance(new Vector2((float)targets[i].Item1, (float)targets[i].Item2) * 16f + Vector2.One * 8f, settings.mousevec);
				if (num9 == -1f || num10 <= num9)
				{
					num9 = num10;
					tuple = targets[i];
				}
			}
			if (Collision.InTileBounds(tuple.Item1, tuple.Item2, settings.LX, settings.LY, settings.HX, settings.HY))
			{
				num = tuple.Item1;
				num6 = tuple.Item2;
			}
		}
		bool flag = false;
		Point item3 = default(Point);
		for (int j = 0; j < targets.Count; j++)
		{
			int item = targets[j].Item1;
			int item2 = targets[j].Item2;
			Tile tile = Main.tile[item, item2];
			int num11 = 0;
			int num12 = 0;
			int num13 = 18;
			int num2 = 18;
			int num3 = 2;
			switch (tile.type)
			{
			case 136:
			case 144:
			case 494:
				num11 = 1;
				num12 = 1;
				num3 = 0;
				break;
			case 216:
			case 338:
				num11 = 1;
				num12 = 2;
				break;
			case 15:
			case 497:
				num11 = 1;
				num12 = 2;
				num3 = 4;
				break;
			case 10:
				num11 = 1;
				num12 = 3;
				num3 = 0;
				break;
			case 388:
			case 389:
				num11 = 1;
				num12 = 5;
				break;
			case 29:
			case 387:
				num11 = 2;
				num12 = 1;
				break;
			case 21:
			case 55:
			case 85:
			case 97:
			case 125:
			case 132:
			case 287:
			case 335:
			case 386:
			case 411:
			case 425:
			case 441:
			case 467:
			case 468:
			case 573:
			case 621:
				num11 = 2;
				num12 = 2;
				break;
			case 79:
			case 139:
			case 510:
			case 511:
				num11 = 2;
				num12 = 2;
				num3 = 0;
				break;
			case 11:
			case 356:
			case 410:
			case 470:
			case 480:
			case 509:
			case 657:
			case 658:
			case 663:
				num11 = 2;
				num12 = 3;
				num3 = 0;
				break;
			case 207:
				num11 = 2;
				num12 = 4;
				num3 = 0;
				break;
			case 104:
				num11 = 2;
				num12 = 5;
				break;
			case 88:
				num11 = 3;
				num12 = 1;
				num3 = 0;
				break;
			case 89:
			case 215:
			case 237:
			case 377:
				num11 = 3;
				num12 = 2;
				break;
			case 354:
			case 455:
			case 491:
				num11 = 3;
				num12 = 3;
				num3 = 0;
				break;
			case 487:
				num11 = 4;
				num12 = 2;
				num3 = 0;
				break;
			case 212:
				num11 = 4;
				num12 = 3;
				break;
			case 209:
				num11 = 4;
				num12 = 3;
				num3 = 0;
				break;
			case 102:
			case 463:
			case 475:
			case 597:
				num11 = 3;
				num12 = 4;
				break;
			case 464:
				num11 = 5;
				num12 = 4;
				break;
			}
			TileLoader.ModifySmartInteractCoords(tile.type, ref num11, ref num12, ref num13, ref num2, ref num3);
			if (num11 == 0 || num12 == 0)
			{
				continue;
			}
			int num4 = item - tile.frameX % (num13 * num11) / num13;
			int num5 = item2 - tile.frameY % (num2 * num12 + num3) / num2;
			bool flag2 = Collision.InTileBounds(num, num6, num4, num5, num4 + num11 - 1, num5 + num12 - 1);
			bool flag3 = Collision.InTileBounds(point.X, point.Y, num4, num5, num4 + num11 - 1, num5 + num12 - 1);
			if (flag3)
			{
				num7 = point.X;
				num8 = point.Y;
			}
			if (!settings.FullInteraction)
			{
				flag2 = flag2 && flag3;
			}
			if (flag)
			{
				flag2 = false;
			}
			for (int k = num4; k < num4 + num11; k++)
			{
				for (int l = num5; l < num5 + num12; l++)
				{
					((Point)(ref item3))._002Ector(k, l);
					if (!Main.SmartInteractTileCoords.Contains(item3))
					{
						if (flag2)
						{
							Main.SmartInteractTileCoordsSelected.Add(item3);
						}
						if (flag2 || settings.FullInteraction)
						{
							Main.SmartInteractTileCoords.Add(item3);
						}
					}
				}
			}
			if (!flag && flag2)
			{
				flag = true;
			}
		}
		if (settings.DemandOnlyZeroDistanceTargets)
		{
			if (num7 != -1 && num8 != -1)
			{
				_candidate.Reuse(strictSettings: true, 0f, num7, num8, settings.LX - 10, settings.LY - 10, settings.HX + 10, settings.HY + 10);
				candidate = _candidate;
				return true;
			}
			return false;
		}
		if (num != -1 && num6 != -1)
		{
			float distanceFromCursor = Utils.ClosestPointInRect(new Rectangle(num * 16, num6 * 16, 16, 16), settings.mousevec).Distance(settings.mousevec);
			_candidate.Reuse(strictSettings: false, distanceFromCursor, num, num6, settings.LX - 10, settings.LY - 10, settings.HX + 10, settings.HY + 10);
			candidate = _candidate;
			return true;
		}
		return false;
	}

	private void FillPotentialTargetTiles(SmartInteractScanSettings settings)
	{
		for (int i = settings.LX; i <= settings.HX; i++)
		{
			for (int j = settings.LY; j <= settings.HY; j++)
			{
				Tile tile = Main.tile[i, j];
				if (tile == null || !tile.active())
				{
					continue;
				}
				switch (tile.type)
				{
				case 10:
				case 11:
				case 21:
				case 29:
				case 55:
				case 79:
				case 85:
				case 88:
				case 89:
				case 97:
				case 102:
				case 104:
				case 125:
				case 132:
				case 136:
				case 139:
				case 144:
				case 207:
				case 209:
				case 215:
				case 216:
				case 287:
				case 335:
				case 338:
				case 354:
				case 377:
				case 386:
				case 387:
				case 388:
				case 389:
				case 410:
				case 411:
				case 425:
				case 441:
				case 455:
				case 463:
				case 464:
				case 467:
				case 468:
				case 470:
				case 475:
				case 480:
				case 487:
				case 491:
				case 494:
				case 509:
				case 510:
				case 511:
				case 573:
				case 597:
				case 621:
				case 657:
				case 658:
					targets.Add(new Tuple<int, int>(i, j));
					break;
				case 15:
				case 497:
					if (settings.player.IsWithinSnappngRangeToTile(i, j, 40))
					{
						targets.Add(new Tuple<int, int>(i, j));
					}
					break;
				case 237:
					if (settings.player.HasItem(1293))
					{
						targets.Add(new Tuple<int, int>(i, j));
					}
					break;
				case 212:
					if (settings.player.HasItem(949))
					{
						targets.Add(new Tuple<int, int>(i, j));
					}
					break;
				case 356:
					if (!Main.fastForwardTimeToDawn && (Main.netMode == 1 || Main.sundialCooldown == 0))
					{
						targets.Add(new Tuple<int, int>(i, j));
					}
					break;
				case 663:
					if (!Main.fastForwardTimeToDusk && (Main.netMode == 1 || Main.moondialCooldown == 0))
					{
						targets.Add(new Tuple<int, int>(i, j));
					}
					break;
				}
				if (TileLoader.HasSmartInteract(i, j, tile.type, settings))
				{
					targets.Add(new Tuple<int, int>(i, j));
				}
			}
		}
	}
}
