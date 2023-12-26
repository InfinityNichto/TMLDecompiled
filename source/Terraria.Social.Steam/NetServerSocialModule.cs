using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Steamworks;
using Terraria.Localization;
using Terraria.Net;
using Terraria.Net.Sockets;

namespace Terraria.Social.Steam;

public class NetServerSocialModule : NetSocialModule
{
	private ServerMode _mode;

	private Callback<P2PSessionRequest_t> _p2pSessionRequest;

	private bool _acceptingClients;

	private SocketConnectionAccepted _connectionAcceptedCallback;

	public NetServerSocialModule()
		: base(1, 2)
	{
	}

	private void BroadcastConnectedUsers()
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		List<ulong> list = new List<ulong>();
		foreach (KeyValuePair<CSteamID, ConnectionState> item in _connectionStateMap)
		{
			if (item.Value == ConnectionState.Connected)
			{
				list.Add(item.Key.m_SteamID);
			}
		}
		byte[] array = new byte[list.Count * 8 + 1];
		using (MemoryStream output = new MemoryStream(array))
		{
			using BinaryWriter binaryWriter = new BinaryWriter(output);
			binaryWriter.Write((byte)1);
			foreach (ulong item2 in list)
			{
				binaryWriter.Write(item2);
			}
		}
		_lobby.SendMessage(array);
	}

	public override void Initialize()
	{
		base.Initialize();
		_reader.SetReadEvent(OnPacketRead);
		_p2pSessionRequest = Callback<P2PSessionRequest_t>.Create((DispatchDelegate<P2PSessionRequest_t>)OnP2PSessionRequest);
		if (Program.LaunchParameters.ContainsKey("-lobby"))
		{
			_mode |= ServerMode.Lobby;
			string text = Program.LaunchParameters["-lobby"];
			if (!(text == "private"))
			{
				if (text == "friends")
				{
					_mode |= ServerMode.FriendsCanJoin;
					_lobby.Create(inviteOnly: false, OnLobbyCreated);
				}
				else
				{
					Console.WriteLine(Language.GetTextValue("Error.InvalidLobbyFlag", "private", "friends"));
				}
			}
			else
			{
				_lobby.Create(inviteOnly: true, OnLobbyCreated);
			}
		}
		if (Program.LaunchParameters.ContainsKey("-friendsoffriends"))
		{
			_mode |= ServerMode.FriendsOfFriends;
		}
	}

	public override ulong GetLobbyId()
	{
		return _lobby.Id.m_SteamID;
	}

	public override void OpenInviteInterface()
	{
	}

	public override void CancelJoin()
	{
	}

	public override bool CanInvite()
	{
		return false;
	}

	public override void LaunchLocalServer(Process process, ServerMode mode)
	{
	}

	public override bool StartListening(SocketConnectionAccepted callback)
	{
		_acceptingClients = true;
		_connectionAcceptedCallback = callback;
		return true;
	}

	public override void StopListening()
	{
		_acceptingClients = false;
	}

	public override void Connect(RemoteAddress address)
	{
	}

	public override void Close(RemoteAddress address)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		CSteamID user = RemoteAddressToSteamId(address);
		Close(user);
	}

	private void Close(CSteamID user)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		if (_connectionStateMap.ContainsKey(user))
		{
			SteamUser.EndAuthSession(user);
			SteamNetworking.CloseP2PSessionWithUser(user);
			_connectionStateMap[user] = ConnectionState.Inactive;
			_reader.ClearUser(user);
			_writer.ClearUser(user);
		}
	}

	private void OnLobbyCreated(LobbyCreated_t result, bool failure)
	{
		if (!failure)
		{
			SteamFriends.SetRichPresence("status", Language.GetTextValue("Social.StatusInGame"));
			Utils.LogAndConsoleInfoMessage("Currently Hosting LobbyID: " + GetLobbyId());
		}
	}

	private bool OnPacketRead(byte[] data, int length, CSteamID userId)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Expected I4, but got Unknown
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		if (!_connectionStateMap.ContainsKey(userId) || _connectionStateMap[userId] == ConnectionState.Inactive)
		{
			P2PSessionRequest_t result = default(P2PSessionRequest_t);
			result.m_steamIDRemote = userId;
			OnP2PSessionRequest(result);
			if (!_connectionStateMap.ContainsKey(userId) || _connectionStateMap[userId] == ConnectionState.Inactive)
			{
				return false;
			}
		}
		ConnectionState connectionState = _connectionStateMap[userId];
		if (connectionState == ConnectionState.Authenticating)
		{
			if (length < 3)
			{
				return false;
			}
			if (((data[1] << 8) | data[0]) != length)
			{
				return false;
			}
			if (data[2] != 93)
			{
				return false;
			}
			byte[] array = new byte[data.Length - 3];
			Array.Copy(data, 3, array, 0, array.Length);
			EBeginAuthSessionResult val = SteamUser.BeginAuthSession(array, array.Length, userId);
			switch ((int)val)
			{
			case 0:
				_connectionStateMap[userId] = ConnectionState.Connected;
				BroadcastConnectedUsers();
				break;
			case 2:
				Close(userId);
				break;
			case 5:
				Close(userId);
				break;
			case 4:
				Close(userId);
				break;
			case 1:
				Close(userId);
				break;
			case 3:
				Close(userId);
				break;
			}
			return false;
		}
		return connectionState == ConnectionState.Connected;
	}

	private void OnP2PSessionRequest(P2PSessionRequest_t result)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Invalid comparison between Unknown and I4
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		CSteamID steamIDRemote = result.m_steamIDRemote;
		if (_connectionStateMap.ContainsKey(steamIDRemote) && _connectionStateMap[steamIDRemote] != 0)
		{
			SteamNetworking.AcceptP2PSessionWithUser(steamIDRemote);
		}
		else if (_acceptingClients && (_mode.HasFlag(ServerMode.FriendsOfFriends) || (int)SteamFriends.GetFriendRelationship(steamIDRemote) == 3))
		{
			SteamNetworking.AcceptP2PSessionWithUser(steamIDRemote);
			P2PSessionState_t pConnectionState = default(P2PSessionState_t);
			while (SteamNetworking.GetP2PSessionState(steamIDRemote, ref pConnectionState) && pConnectionState.m_bConnecting == 1)
			{
			}
			if (pConnectionState.m_bConnectionActive == 0)
			{
				Close(steamIDRemote);
			}
			_connectionStateMap[steamIDRemote] = ConnectionState.Authenticating;
			_connectionAcceptedCallback(new SocialSocket(new SteamAddress(steamIDRemote)));
		}
		else
		{
			Utils.LogAndConsoleInfoMessage("User " + SteamFriends.GetFriendPersonaName(steamIDRemote) + "connection rejected. Not a friend of lobby Owner.\nSet allow Friends of Friends to accept non-Friends of Host.");
		}
	}
}
