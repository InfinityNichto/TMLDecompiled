using System.Collections;

namespace NATUPNPLib;

public interface IStaticPortMappingCollection : IEnumerable
{
	void Remove(int lExternalPort, string bstrProtocol);

	IStaticPortMapping Add(int lExternalPort, string bstrProtocol, int lInternalPort, string bstrInternalClient, bool bEnabled, string bstrDescription);
}
