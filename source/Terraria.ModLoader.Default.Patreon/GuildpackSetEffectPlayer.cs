namespace Terraria.ModLoader.Default.Patreon;

internal class GuildpackSetEffectPlayer : ModPlayer
{
	public bool IsActive;

	public override void ResetEffects()
	{
		IsActive = false;
	}
}
