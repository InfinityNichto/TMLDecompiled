namespace Terraria.ModLoader.Default.Patreon;

internal class SaetharSetEffectPlayer : ModPlayer
{
	public bool IsActive;

	public override void ResetEffects()
	{
		IsActive = false;
	}
}
