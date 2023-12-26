namespace NATUPNPLib;

public interface IStaticPortMapping
{
	int InternalPort { get; }

	string Protocol { get; }

	string InternalClient { get; }
}
