namespace Terraria.ModLoader;

public static class ModSideExtensions
{
	public static string ToFriendlyString(this ModSide sortmode)
	{
		return sortmode switch
		{
			ModSide.Both => "Both", 
			ModSide.Client => "Client", 
			ModSide.Server => "Server", 
			ModSide.NoSync => "NoSync", 
			_ => "Unknown", 
		};
	}
}
