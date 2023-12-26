namespace Terraria.ModLoader.Default;

[Autoload(false)]
public class UnloadedAccessorySlot : ModAccessorySlot
{
	public override string Name { get; }

	internal UnloadedAccessorySlot(int slot, string oldName)
	{
		base.Type = slot;
		Name = oldName;
	}

	public override bool IsEnabled()
	{
		return false;
	}
}
