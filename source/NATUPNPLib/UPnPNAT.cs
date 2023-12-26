using System;
using System.Collections;
using System.Collections.Generic;
using UwUPnP;

namespace NATUPNPLib;

public sealed class UPnPNAT : IUPnPNAT
{
	private record StaticPortMapping(int InternalPort, string Protocol, string InternalClient) : IStaticPortMapping;

	private class MappingCollection : IStaticPortMappingCollection, IEnumerable
	{
		public IStaticPortMapping Add(int lExternalPort, string bstrProtocol, int lInternalPort, string bstrInternalClient, bool bEnabled, string bstrDescription)
		{
			UPnP.Open(Enum.Parse<Protocol>(bstrProtocol), (ushort)lExternalPort, (ushort)lInternalPort, bstrDescription);
			return new StaticPortMapping(lInternalPort, bstrProtocol, bstrInternalClient);
		}

		public void Remove(int lExternalPort, string bstrProtocol)
		{
			UPnP.Close(Enum.Parse<Protocol>(bstrProtocol), (ushort)lExternalPort);
		}

		public IEnumerator GetEnumerator()
		{
			int i = 0;
			while (true)
			{
				Dictionary<string, string> args;
				try
				{
					args = UPnP.GetGenericPortMappingEntry(i);
				}
				catch
				{
					break;
				}
				if (args == null || !args.TryGetValue("NewInternalPort", out var s_port) || !int.TryParse(s_port, out var port) || !args.TryGetValue("NewProtocol", out var protocol) || !args.TryGetValue("NewInternalClient", out var client))
				{
					break;
				}
				yield return new StaticPortMapping(port, protocol, client);
				i++;
			}
		}
	}

	public IStaticPortMappingCollection StaticPortMappingCollection { get; } = new MappingCollection();

}
