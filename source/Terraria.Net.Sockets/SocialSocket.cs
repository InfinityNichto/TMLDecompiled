using System.Threading;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria.Social;

namespace Terraria.Net.Sockets;

public class SocialSocket : ISocket
{
	private delegate void InternalReadCallback(byte[] data, int offset, int size, SocketReceiveCallback callback, object state);

	private RemoteAddress _remoteAddress;

	public SocialSocket()
	{
	}

	public SocialSocket(RemoteAddress remoteAddress)
	{
		_remoteAddress = remoteAddress;
	}

	void ISocket.Close()
	{
		if (_remoteAddress != null)
		{
			ModNet.Log(_remoteAddress, "Closing SocialSocket");
			SocialAPI.Network.Close(_remoteAddress);
			_remoteAddress = null;
		}
	}

	bool ISocket.IsConnected()
	{
		return SocialAPI.Network.IsConnected(_remoteAddress);
	}

	void ISocket.Connect(RemoteAddress address)
	{
		_remoteAddress = address;
		SocialAPI.Network.Connect(address);
	}

	void ISocket.AsyncSend(byte[] data, int offset, int size, SocketSendCallback callback, object state)
	{
		if (_remoteAddress == null)
		{
			ModNet.Warn("SocialSocket, AsyncSend after connection closed.");
			return;
		}
		if (ModNet.DetailedLogging)
		{
			ModNet.Debug(_remoteAddress, $"send {size}");
		}
		SocialAPI.Network.Send(_remoteAddress, data, size);
		Task.Run(delegate
		{
			callback(state);
		});
	}

	private void ReadCallback(byte[] data, int offset, int size, SocketReceiveCallback callback, object state)
	{
		int size2;
		while ((size2 = SocialAPI.Network.Receive(_remoteAddress, data, offset, size)) == 0)
		{
			Thread.Sleep(1);
		}
		callback(state, size2);
	}

	void ISocket.AsyncReceive(byte[] data, int offset, int size, SocketReceiveCallback callback, object state)
	{
		Task.Run(delegate
		{
			new InternalReadCallback(ReadCallback)(data, offset, size, callback, state);
		});
	}

	void ISocket.SendQueuedPackets()
	{
	}

	bool ISocket.IsDataAvailable()
	{
		return SocialAPI.Network.IsDataAvailable(_remoteAddress);
	}

	RemoteAddress ISocket.GetRemoteAddress()
	{
		return _remoteAddress;
	}

	bool ISocket.StartListening(SocketConnectionAccepted callback)
	{
		return SocialAPI.Network.StartListening(callback);
	}

	void ISocket.StopListening()
	{
		SocialAPI.Network.StopListening();
	}
}
