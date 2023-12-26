namespace Terraria.DataStructures;

public struct PlayerFishingConditions
{
	public float LevelMultipliers;

	public int FinalFishingLevel;

	public Item Pole;

	public Item Bait;

	public int PolePower => Pole?.fishingPole ?? 0;

	public int PoleItemType => Pole?.type ?? 0;

	public int BaitPower => Bait?.bait ?? 0;

	public int BaitItemType => Bait?.type ?? 0;
}
