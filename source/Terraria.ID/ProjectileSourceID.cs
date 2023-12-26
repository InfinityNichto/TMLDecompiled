namespace Terraria.ID;

internal static class ProjectileSourceID
{
	public const int None = 0;

	public const int SetBonus_SolarExplosion_WhenTakingDamage = 1;

	public const int SetBonus_SolarExplosion_WhenDashing = 2;

	public const int SetBonus_ForbiddenStorm = 3;

	public const int SetBonus_Titanium = 4;

	public const int SetBonus_Orichalcum = 5;

	public const int SetBonus_Chlorophyte = 6;

	public const int SetBonus_Stardust = 7;

	public const int WeaponEnchantment_Confetti = 8;

	public const int PlayerDeath_TombStone = 9;

	public const int TorchGod = 10;

	public const int FallingStar = 11;

	public const int PlayerHurt_DropFootball = 12;

	public const int StormTigerTierSwap = 13;

	public const int AbigailTierSwap = 14;

	public const int SetBonus_GhostHeal = 15;

	public const int SetBonus_GhostHurt = 16;

	public const int VampireKnives = 18;

	public static readonly int Count = 19;

	public static string? ToContextString(int itemSourceId)
	{
		return itemSourceId switch
		{
			1 => "SetBonus_SolarExplosion_WhenTakingDamage", 
			2 => "SetBonus_SolarExplosion_WhenDashing", 
			3 => "SetBonus_ForbiddenStorm", 
			4 => "SetBonus_Titanium", 
			5 => "SetBonus_Orichalcum", 
			6 => "SetBonus_Chlorophyte", 
			7 => "SetBonus_Stardust", 
			8 => "WeaponEnchantment_Confetti", 
			9 => "PlayerDeath_TombStone", 
			11 => "FallingStar", 
			12 => "PlayerHurt_DropFootball", 
			13 => "StormTigerTierSwap", 
			14 => "AbigailTierSwap", 
			15 => "SetBonus_GhostHeal", 
			16 => "SetBonus_GhostHurt", 
			18 => "VampireKnives", 
			10 => "TorchGod", 
			_ => null, 
		};
	}
}
