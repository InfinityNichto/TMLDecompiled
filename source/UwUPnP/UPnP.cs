using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace UwUPnP;

/// <summary>
/// A simple UPnP library
/// See: https://github.com/Rartrin/UwUPnP
/// </summary>
public static class UPnP
{
	private static bool gatewayNotYetRequested = true;

	private static bool searching = false;

	private static Gateway defaultGateway = null;

	private static Gateway Gateway
	{
		get
		{
			if (gatewayNotYetRequested)
			{
				gatewayNotYetRequested = false;
				FindGateway();
			}
			while (searching)
			{
				Thread.Sleep(1);
			}
			return defaultGateway;
		}
	}

	public static bool IsAvailable => Gateway != null;

	public static IPAddress ExternalIP => Gateway?.ExternalIPAddress;

	public static IPAddress LocalIP => Gateway?.InternalClient;

	public static void Open(Protocol protocol, ushort externalPort, ushort? internalPort = null, string description = null)
	{
		Gateway?.AddPortMapping(externalPort, protocol, internalPort, description);
	}

	public static void Close(Protocol protocol, ushort externalPort)
	{
		Gateway?.DeletePortMapping(externalPort, protocol);
	}

	public static bool IsOpen(Protocol protocol, ushort externalPort)
	{
		return Gateway?.SpecificPortMappingExists(externalPort, protocol) ?? false;
	}

	public static Dictionary<string, string> GetGenericPortMappingEntry(int portMappingIndex)
	{
		return Gateway?.GetGenericPortMappingEntry(portMappingIndex);
	}

	private static void FindGateway()
	{
		searching = true;
		List<Task> listeners = new List<Task>();
		foreach (IPAddress ip in GetLocalIPs())
		{
			listeners.Add(Task.Run(delegate
			{
				StartListener(ip);
			}));
		}
		Task.WhenAll(listeners).ContinueWith((Task t) => searching = false);
	}

	private static void StartListener(IPAddress ip)
	{
		if (Gateway.TryNew(ip, out var gateway))
		{
			Interlocked.CompareExchange(ref defaultGateway, gateway, null);
			searching = false;
		}
	}

	private static IEnumerable<IPAddress> GetLocalIPs()
	{
		return NetworkInterface.GetAllNetworkInterfaces().Where(IsValidInterface).SelectMany(GetValidNetworkIPs);
	}

	private static bool IsValidInterface(NetworkInterface network)
	{
		if (network.OperationalStatus == OperationalStatus.Up && network.NetworkInterfaceType != NetworkInterfaceType.Loopback)
		{
			return network.NetworkInterfaceType != NetworkInterfaceType.Ppp;
		}
		return false;
	}

	private static IEnumerable<IPAddress> GetValidNetworkIPs(NetworkInterface network)
	{
		return from a in network.GetIPProperties().UnicastAddresses
			select a.Address into a
			where a.AddressFamily == AddressFamily.InterNetwork || a.AddressFamily == AddressFamily.InterNetworkV6
			select a;
	}
}
