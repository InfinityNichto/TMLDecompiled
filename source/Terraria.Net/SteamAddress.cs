using Steamworks;

namespace Terraria.Net;

public class SteamAddress : RemoteAddress
{
	public readonly CSteamID SteamId;

	private string _friendlyName;

	public SteamAddress(CSteamID steamId)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		Type = AddressType.Steam;
		SteamId = steamId;
	}

	public override string ToString()
	{
		string text = (SteamId.m_SteamID % 2).ToString();
		string text2 = ((SteamId.m_SteamID - (76561197960265728L + SteamId.m_SteamID % 2)) / 2).ToString();
		return "STEAM_0:" + text + ":" + text2;
	}

	public override string GetIdentifier()
	{
		return ToString();
	}

	public override bool IsLocalHost()
	{
		if (Program.LaunchParameters.ContainsKey("-localsteamid"))
		{
			return Program.LaunchParameters["-localsteamid"].Equals(SteamId.m_SteamID.ToString());
		}
		return false;
	}

	public override string GetFriendlyName()
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		if (_friendlyName == null)
		{
			_friendlyName = SteamFriends.GetFriendPersonaName(SteamId);
		}
		return _friendlyName;
	}
}
