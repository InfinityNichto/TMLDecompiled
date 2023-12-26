namespace Terraria.GameContent.Personalities;

public class NPCPreferenceTrait : IShopPersonalityTrait
{
	public AffectionLevel Level;

	public int NpcId;

	public void ModifyShopPrice(HelperInfo info, ShopHelper shopHelperInstance)
	{
		if (info.nearbyNPCsByType[NpcId])
		{
			shopHelperInstance.ApplyNpcRelationshipEffect(NpcId, Level);
		}
	}
}
