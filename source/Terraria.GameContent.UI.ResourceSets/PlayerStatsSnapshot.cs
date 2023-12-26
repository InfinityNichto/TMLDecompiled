namespace Terraria.GameContent.UI.ResourceSets;

public struct PlayerStatsSnapshot
{
	public int Life;

	public int LifeMax;

	public int LifeFruitCount;

	public int Mana;

	public int ManaMax;

	private int numLifeHearts;

	private int numManaStars;

	public float LifePerSegment => (float)LifeMax / (float)AmountOfLifeHearts;

	public float ManaPerSegment => (float)ManaMax / (float)AmountOfManaStars;

	/// <summary>
	/// How many life hearts should be drawn
	/// </summary>
	public int AmountOfLifeHearts
	{
		get
		{
			return numLifeHearts;
		}
		set
		{
			numLifeHearts = Utils.Clamp(value, int.MinValue, 20);
		}
	}

	/// <summary>
	/// How many mana stars should be drawn
	/// </summary>
	public int AmountOfManaStars
	{
		get
		{
			return numManaStars;
		}
		set
		{
			numManaStars = Utils.Clamp(value, int.MinValue, 20);
		}
	}

	public PlayerStatsSnapshot(Player player)
	{
		Life = player.statLife;
		Mana = player.statMana;
		LifeMax = player.statLifeMax2;
		ManaMax = player.statManaMax2;
		float num = 20f;
		int num2 = player.statLifeMax / 20;
		int num3 = player.ConsumedLifeFruit;
		if (num3 < 0)
		{
			num3 = 0;
		}
		if (num3 > 0)
		{
			num2 = player.statLifeMax / (20 + num3 / 4);
			num = (float)player.statLifeMax / 20f;
		}
		int num4 = player.statLifeMax2 - player.statLifeMax;
		num += (float)(num4 / num2);
		LifeFruitCount = num3;
		numLifeHearts = (int)((float)LifeMax / num);
		numManaStars = (int)((double)ManaMax / 20.0);
	}
}
