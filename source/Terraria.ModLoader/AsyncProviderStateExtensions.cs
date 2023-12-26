namespace Terraria.ModLoader;

public static class AsyncProviderStateExtensions
{
	public static bool IsFinished(this AsyncProviderState s)
	{
		if (s != AsyncProviderState.Completed && s != AsyncProviderState.Canceled)
		{
			return s == AsyncProviderState.Aborted;
		}
		return true;
	}
}
