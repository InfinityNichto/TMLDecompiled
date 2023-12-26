using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.Enums;

namespace Terraria.GameContent.Events;

public class MysticLogFairiesEvent
{
	private bool _canSpawnFairies;

	private double _delayUntilNextAttempt;

	private const int DELAY_BETWEEN_ATTEMPTS = 60;

	private List<Point> _stumpCoords = new List<Point>();

	public void WorldClear()
	{
		_canSpawnFairies = false;
		_delayUntilNextAttempt = 0.0;
		_stumpCoords.Clear();
	}

	public void StartWorld()
	{
		if (Main.netMode != 1)
		{
			ScanWholeOverworldForLogs();
		}
	}

	public void StartNight()
	{
		if (Main.netMode != 1)
		{
			_canSpawnFairies = true;
			_delayUntilNextAttempt = 0.0;
			ScanWholeOverworldForLogs();
		}
	}

	public void UpdateTime()
	{
		if (Main.netMode != 1 && _canSpawnFairies && IsAGoodTime())
		{
			_delayUntilNextAttempt = Math.Max(0.0, _delayUntilNextAttempt - Main.desiredWorldEventsUpdateRate);
			if (_delayUntilNextAttempt == 0.0)
			{
				_delayUntilNextAttempt = 60.0;
				TrySpawningFairies();
			}
		}
	}

	private bool IsAGoodTime()
	{
		if (Main.dayTime)
		{
			return false;
		}
		if (!Main.remixWorld)
		{
			if (Main.time < 6480.0000965595245)
			{
				return false;
			}
			if (Main.time > 25920.000386238098)
			{
				return false;
			}
		}
		return true;
	}

	private void TrySpawningFairies()
	{
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		if (Main.maxRaining > 0f || Main.bloodMoon || NPC.MoonLordCountdown > 0 || Main.snowMoon || Main.pumpkinMoon || Main.invasionType > 0 || _stumpCoords.Count == 0)
		{
			return;
		}
		int oneOverSpawnChance = GetOneOverSpawnChance();
		bool flag = false;
		for (int i = 0; i < Main.worldEventUpdates; i++)
		{
			if (Main.rand.Next(oneOverSpawnChance) == 0)
			{
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			return;
		}
		int index = Main.rand.Next(_stumpCoords.Count);
		Point p = _stumpCoords[index];
		Vector2 vector = p.ToWorldCoordinates(24f);
		vector.Y -= 50f;
		if (WorldGen.PlayerLOS(p.X, p.Y))
		{
			return;
		}
		int num = Main.rand.Next(1, 4);
		if (Main.rand.Next(7) == 0)
		{
			num++;
		}
		int num2 = Utils.SelectRandom(Main.rand, new short[3] { 585, 584, 583 });
		for (int j = 0; j < num; j++)
		{
			num2 = Utils.SelectRandom(Main.rand, new short[3] { 585, 584, 583 });
			if (Main.tenthAnniversaryWorld && Main.rand.Next(4) != 0)
			{
				num2 = 583;
			}
			int num3 = NPC.NewNPC(new EntitySource_WorldEvent(), (int)vector.X, (int)vector.Y, num2);
			if (Main.netMode == 2 && num3 < 200)
			{
				NetMessage.SendData(23, -1, -1, null, num3);
			}
		}
		_canSpawnFairies = false;
	}

	public void FallenLogDestroyed()
	{
		if (Main.netMode != 1)
		{
			ScanWholeOverworldForLogs();
		}
	}

	private void ScanWholeOverworldForLogs()
	{
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		_stumpCoords.Clear();
		NPC.fairyLog = false;
		int num = (int)Main.worldSurface - 10;
		int num2 = 100;
		int num3 = Main.maxTilesX - 100;
		if (Main.remixWorld)
		{
			num = Main.maxTilesY - 350;
			num2 = (int)Main.rockLayer;
		}
		int num4 = 3;
		int num5 = 2;
		List<Point> list = new List<Point>();
		for (int i = 100; i < num3; i += num4)
		{
			for (int num6 = num; num6 >= num2; num6 -= num5)
			{
				Tile tile = Main.tile[i, num6];
				if (tile.active() && tile.type == 488 && tile.liquid == 0)
				{
					list.Add(new Point(i, num6));
					NPC.fairyLog = true;
				}
			}
		}
		foreach (Point item in list)
		{
			_stumpCoords.Add(GetStumpTopLeft(item));
		}
	}

	private Point GetStumpTopLeft(Point stumpRandomPoint)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		Tile tile = Main.tile[stumpRandomPoint.X, stumpRandomPoint.Y];
		Point result = stumpRandomPoint;
		result.X -= tile.frameX / 18;
		result.Y -= tile.frameY / 18;
		return result;
	}

	private int GetOneOverSpawnChance()
	{
		MoonPhase moonPhase = Main.GetMoonPhase();
		return ((moonPhase != 0 && moonPhase != MoonPhase.Empty) ? 10800 : 3600) / 60;
	}
}
