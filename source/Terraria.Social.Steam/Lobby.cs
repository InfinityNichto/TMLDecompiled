using System;
using System.Collections.Generic;
using Steamworks;

namespace Terraria.Social.Steam;

public class Lobby
{
	private HashSet<CSteamID> _usersSeen = new HashSet<CSteamID>();

	private byte[] _messageBuffer = new byte[1024];

	public CSteamID Id = CSteamID.Nil;

	public CSteamID Owner = CSteamID.Nil;

	public LobbyState State;

	private CallResult<LobbyEnter_t> _lobbyEnter;

	private APIDispatchDelegate<LobbyEnter_t> _lobbyEnterExternalCallback;

	private CallResult<LobbyCreated_t> _lobbyCreated;

	private APIDispatchDelegate<LobbyCreated_t> _lobbyCreatedExternalCallback;

	public Lobby()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		_lobbyEnter = CallResult<LobbyEnter_t>.Create((APIDispatchDelegate<LobbyEnter_t>)OnLobbyEntered);
		_lobbyCreated = CallResult<LobbyCreated_t>.Create((APIDispatchDelegate<LobbyCreated_t>)OnLobbyCreated);
	}

	public void Create(bool inviteOnly, APIDispatchDelegate<LobbyCreated_t> callResult)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		SteamAPICall_t hAPICall = SteamMatchmaking.CreateLobby((ELobbyType)(!inviteOnly), 256);
		_lobbyCreatedExternalCallback = callResult;
		_lobbyCreated.Set(hAPICall, (APIDispatchDelegate<LobbyCreated_t>)null);
		State = LobbyState.Creating;
	}

	public void OpenInviteOverlay()
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		if (State == LobbyState.Inactive)
		{
			SteamFriends.ActivateGameOverlayInviteDialog(new CSteamID(Main.LobbyId));
		}
		else
		{
			SteamFriends.ActivateGameOverlayInviteDialog(Id);
		}
	}

	public void Join(CSteamID lobbyId, APIDispatchDelegate<LobbyEnter_t> callResult)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		if (State == LobbyState.Inactive)
		{
			State = LobbyState.Connecting;
			_lobbyEnterExternalCallback = callResult;
			SteamAPICall_t hAPICall = SteamMatchmaking.JoinLobby(lobbyId);
			_lobbyEnter.Set(hAPICall, (APIDispatchDelegate<LobbyEnter_t>)null);
		}
	}

	public byte[] GetMessage(int index)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		CSteamID pSteamIDUser = default(CSteamID);
		EChatEntryType peChatEntryType = default(EChatEntryType);
		int lobbyChatEntry = SteamMatchmaking.GetLobbyChatEntry(Id, index, ref pSteamIDUser, _messageBuffer, _messageBuffer.Length, ref peChatEntryType);
		byte[] array = new byte[lobbyChatEntry];
		Array.Copy(_messageBuffer, array, lobbyChatEntry);
		return array;
	}

	public int GetUserCount()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		return SteamMatchmaking.GetNumLobbyMembers(Id);
	}

	public CSteamID GetUserByIndex(int index)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		return SteamMatchmaking.GetLobbyMemberByIndex(Id, index);
	}

	public bool SendMessage(byte[] data)
	{
		return SendMessage(data, data.Length);
	}

	public bool SendMessage(byte[] data, int length)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		if (State != LobbyState.Active)
		{
			return false;
		}
		return SteamMatchmaking.SendLobbyChatMsg(Id, data, length);
	}

	public void Set(CSteamID lobbyId)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		Id = lobbyId;
		State = LobbyState.Active;
		Owner = SteamMatchmaking.GetLobbyOwner(lobbyId);
	}

	public void SetPlayedWith(CSteamID userId)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		if (!_usersSeen.Contains(userId))
		{
			SteamFriends.SetPlayedWith(userId);
			_usersSeen.Add(userId);
		}
	}

	public void Leave()
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		if (State == LobbyState.Active)
		{
			SteamMatchmaking.LeaveLobby(Id);
		}
		State = LobbyState.Inactive;
		_usersSeen.Clear();
	}

	private void OnLobbyEntered(LobbyEnter_t result, bool failure)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		if (State == LobbyState.Connecting)
		{
			if (failure)
			{
				State = LobbyState.Inactive;
			}
			else
			{
				State = LobbyState.Active;
			}
			Id = new CSteamID(result.m_ulSteamIDLobby);
			Owner = SteamMatchmaking.GetLobbyOwner(Id);
			_lobbyEnterExternalCallback.Invoke(result, failure);
		}
	}

	private void OnLobbyCreated(LobbyCreated_t result, bool failure)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		if (State == LobbyState.Creating)
		{
			if (failure)
			{
				State = LobbyState.Inactive;
			}
			else
			{
				State = LobbyState.Active;
			}
			Id = new CSteamID(result.m_ulSteamIDLobby);
			Owner = SteamMatchmaking.GetLobbyOwner(Id);
			_lobbyCreatedExternalCallback.Invoke(result, failure);
		}
	}
}
