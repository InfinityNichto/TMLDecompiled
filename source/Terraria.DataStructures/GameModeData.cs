namespace Terraria.DataStructures;

public record struct GameModeData
{
	public int Id { get; init; }

	public bool IsExpertMode { get; set; }

	public bool IsMasterMode { get; set; }

	public bool IsJourneyMode { get; set; }

	public float EnemyMaxLifeMultiplier { get; set; }

	public float EnemyDamageMultiplier { get; set; }

	public float DebuffTimeMultiplier { get; set; }

	public float KnockbackToEnemiesMultiplier { get; set; }

	public float TownNPCDamageMultiplier { get; set; }

	public float EnemyDefenseMultiplier { get; set; }

	public float EnemyMoneyDropMultiplier { get; set; }

	public static readonly GameModeData NormalMode = new GameModeData
	{
		Id = 0,
		EnemyMaxLifeMultiplier = 1f,
		EnemyDamageMultiplier = 1f,
		DebuffTimeMultiplier = 1f,
		KnockbackToEnemiesMultiplier = 1f,
		TownNPCDamageMultiplier = 1f,
		EnemyDefenseMultiplier = 1f,
		EnemyMoneyDropMultiplier = 1f
	};

	public static readonly GameModeData ExpertMode = new GameModeData
	{
		Id = 1,
		IsExpertMode = true,
		EnemyMaxLifeMultiplier = 2f,
		EnemyDamageMultiplier = 2f,
		DebuffTimeMultiplier = 2f,
		KnockbackToEnemiesMultiplier = 0.9f,
		TownNPCDamageMultiplier = 1.5f,
		EnemyDefenseMultiplier = 1f,
		EnemyMoneyDropMultiplier = 2.5f
	};

	public static readonly GameModeData MasterMode = new GameModeData
	{
		Id = 2,
		IsExpertMode = true,
		IsMasterMode = true,
		EnemyMaxLifeMultiplier = 3f,
		EnemyDamageMultiplier = 3f,
		DebuffTimeMultiplier = 2.5f,
		KnockbackToEnemiesMultiplier = 0.8f,
		TownNPCDamageMultiplier = 1.75f,
		EnemyDefenseMultiplier = 1f,
		EnemyMoneyDropMultiplier = 2.5f
	};

	public static readonly GameModeData CreativeMode = new GameModeData
	{
		Id = 3,
		IsJourneyMode = true,
		EnemyMaxLifeMultiplier = 1f,
		EnemyDamageMultiplier = 1f,
		DebuffTimeMultiplier = 1f,
		KnockbackToEnemiesMultiplier = 1f,
		TownNPCDamageMultiplier = 2f,
		EnemyDefenseMultiplier = 1f,
		EnemyMoneyDropMultiplier = 1f
	};
}
