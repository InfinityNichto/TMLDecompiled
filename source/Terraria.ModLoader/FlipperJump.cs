namespace Terraria.ModLoader;

public sealed class FlipperJump : VanillaExtraJump
{
	public override float GetDurationMultiplier(Player player)
	{
		return 1f;
	}

	public override bool CanStart(Player player)
	{
		if (!player.mount.Active || !player.mount.Cart)
		{
			return player.wet;
		}
		return false;
	}

	public override void OnStarted(Player player, ref bool playSound)
	{
		if (player.swimTime == 0)
		{
			player.swimTime = 30;
		}
		if (player.sliding)
		{
			player.velocity.X = 3 * -player.slideDir;
		}
		playSound = false;
		player.GetJumpState(this).Available = true;
	}
}
