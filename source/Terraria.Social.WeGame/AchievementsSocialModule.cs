using System;
using System.Threading;
using rail;
using Terraria.Social.Base;

namespace Terraria.Social.WeGame;

public class AchievementsSocialModule : Terraria.Social.Base.AchievementsSocialModule
{
	private const string FILE_NAME = "/achievements-wegame.dat";

	private bool _areStatsReceived;

	private bool _areAchievementReceived;

	private RailCallBackHelper _callbackHelper = new RailCallBackHelper();

	private IRailPlayerAchievement _playerAchievement;

	private IRailPlayerStats _playerStats;

	public override void Initialize()
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		_callbackHelper.RegisterCallback((RAILEventID)2001, new RailEventCallBackHandler(RailEventCallBack));
		_callbackHelper.RegisterCallback((RAILEventID)2101, new RailEventCallBackHandler(RailEventCallBack));
		IRailPlayerStats myPlayerStats = GetMyPlayerStats();
		IRailPlayerAchievement myPlayerAchievement = GetMyPlayerAchievement();
		if (myPlayerStats != null && myPlayerAchievement != null)
		{
			myPlayerStats.AsyncRequestStats("");
			myPlayerAchievement.AsyncRequestAchievement("");
			while (!_areStatsReceived && !_areAchievementReceived)
			{
				CoreSocialModule.RailEventTick();
				Thread.Sleep(10);
			}
		}
	}

	public override void Shutdown()
	{
		StoreStats();
	}

	private IRailPlayerStats GetMyPlayerStats()
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		if (_playerStats == null)
		{
			IRailStatisticHelper railStatisticHelper = rail_api.RailFactory().RailStatisticHelper();
			if (railStatisticHelper != null)
			{
				RailID railID = new RailID();
				((RailComparableID)railID).id_ = 0uL;
				_playerStats = railStatisticHelper.CreatePlayerStats(railID);
			}
		}
		return _playerStats;
	}

	private IRailPlayerAchievement GetMyPlayerAchievement()
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		if (_playerAchievement == null)
		{
			IRailAchievementHelper railAchievementHelper = rail_api.RailFactory().RailAchievementHelper();
			if (railAchievementHelper != null)
			{
				RailID railID = new RailID();
				((RailComparableID)railID).id_ = 0uL;
				_playerAchievement = railAchievementHelper.CreatePlayerAchievement(railID);
			}
		}
		return _playerAchievement;
	}

	public void RailEventCallBack(RAILEventID eventId, EventBase data)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Invalid comparison between Unknown and I4
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Invalid comparison between Unknown and I4
		if ((int)eventId != 2001)
		{
			if ((int)eventId == 2101)
			{
				_areAchievementReceived = true;
			}
		}
		else
		{
			_areStatsReceived = true;
		}
	}

	public override bool IsAchievementCompleted(string name)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Invalid comparison between Unknown and I4
		bool achieved = false;
		RailResult railResult = (RailResult)1;
		IRailPlayerAchievement myPlayerAchievement = GetMyPlayerAchievement();
		if (myPlayerAchievement != null)
		{
			railResult = myPlayerAchievement.HasAchieved(name, ref achieved);
		}
		if (achieved)
		{
			return (int)railResult == 0;
		}
		return false;
	}

	public override byte[] GetEncryptionKey()
	{
		RailID railID = rail_api.RailFactory().RailPlayer().GetRailID();
		byte[] array = new byte[16];
		byte[] bytes = BitConverter.GetBytes(((RailComparableID)railID).id_);
		Array.Copy(bytes, array, 8);
		Array.Copy(bytes, 0, array, 8, 8);
		return array;
	}

	public override string GetSavePath()
	{
		return "/achievements-wegame.dat";
	}

	private int GetIntStat(string name)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		int data = 0;
		IRailPlayerStats myPlayerStats = GetMyPlayerStats();
		if (myPlayerStats != null)
		{
			myPlayerStats.GetStatValue(name, ref data);
		}
		return data;
	}

	private float GetFloatStat(string name)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		double data = 0.0;
		IRailPlayerStats myPlayerStats = GetMyPlayerStats();
		if (myPlayerStats != null)
		{
			myPlayerStats.GetStatValue(name, ref data);
		}
		return (float)data;
	}

	private bool SetFloatStat(string name, float value)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Invalid comparison between Unknown and I4
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		IRailPlayerStats myPlayerStats = GetMyPlayerStats();
		RailResult railResult = (RailResult)1;
		if (myPlayerStats != null)
		{
			railResult = myPlayerStats.SetStatValue(name, (double)value);
		}
		return (int)railResult == 0;
	}

	public override void UpdateIntStat(string name, int value)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		IRailPlayerStats myPlayerStats = GetMyPlayerStats();
		if (myPlayerStats != null)
		{
			int data = 0;
			if ((int)myPlayerStats.GetStatValue(name, ref data) == 0 && data < value)
			{
				myPlayerStats.SetStatValue(name, value);
			}
		}
	}

	private bool SetIntStat(string name, int value)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Invalid comparison between Unknown and I4
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		IRailPlayerStats myPlayerStats = GetMyPlayerStats();
		RailResult railResult = (RailResult)1;
		if (myPlayerStats != null)
		{
			railResult = myPlayerStats.SetStatValue(name, value);
		}
		return (int)railResult == 0;
	}

	public override void UpdateFloatStat(string name, float value)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		IRailPlayerStats myPlayerStats = GetMyPlayerStats();
		if (myPlayerStats != null)
		{
			double data = 0.0;
			if ((int)myPlayerStats.GetStatValue(name, ref data) == 0 && (float)data < value)
			{
				myPlayerStats.SetStatValue(name, (double)value);
			}
		}
	}

	public override void StoreStats()
	{
		SaveStats();
		SaveAchievement();
	}

	private void SaveStats()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		IRailPlayerStats myPlayerStats = GetMyPlayerStats();
		if (myPlayerStats != null)
		{
			myPlayerStats.AsyncStoreStats("");
		}
	}

	private void SaveAchievement()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		IRailPlayerAchievement myPlayerAchievement = GetMyPlayerAchievement();
		if (myPlayerAchievement != null)
		{
			myPlayerAchievement.AsyncStoreAchievement("");
		}
	}

	public override void CompleteAchievement(string name)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		IRailPlayerAchievement myPlayerAchievement = GetMyPlayerAchievement();
		if (myPlayerAchievement != null)
		{
			myPlayerAchievement.MakeAchievement(name);
		}
	}
}
