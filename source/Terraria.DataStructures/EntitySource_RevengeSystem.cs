namespace Terraria.DataStructures;

/// <summary>
/// Used when an NPC is respawned from the <see cref="T:Terraria.GameContent.CoinLossRevengeSystem" />
/// </summary>
public class EntitySource_RevengeSystem : IEntitySource
{
	public string? Context { get; }

	public EntitySource_RevengeSystem(string? context = null)
	{
		Context = context;
	}
}
