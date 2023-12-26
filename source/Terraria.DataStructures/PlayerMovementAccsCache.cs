using Terraria.ModLoader;

namespace Terraria.DataStructures;

public struct PlayerMovementAccsCache
{
	private bool _readyToPaste;

	private bool _mountPreventedFlight;

	private bool _mountPreventedExtraJumps;

	private int rocketTime;

	private float wingTime;

	private int rocketDelay;

	private int rocketDelay2;

	private bool[] canJumpAgain;

	public void CopyFrom(Player player)
	{
		if (_readyToPaste)
		{
			return;
		}
		_readyToPaste = true;
		_mountPreventedFlight = true;
		_mountPreventedExtraJumps = player.mount.BlockExtraJumps;
		rocketTime = player.rocketTime;
		rocketDelay = player.rocketDelay;
		rocketDelay2 = player.rocketDelay2;
		wingTime = player.wingTime;
		if (canJumpAgain == null)
		{
			canJumpAgain = new bool[ExtraJumpLoader.ExtraJumpCount];
		}
		foreach (ExtraJump jump in ExtraJumpLoader.ExtraJumps)
		{
			canJumpAgain[jump.Type] = player.GetJumpState(jump).Available;
		}
	}

	public void PasteInto(Player player)
	{
		if (!_readyToPaste)
		{
			return;
		}
		_readyToPaste = false;
		if (_mountPreventedFlight)
		{
			player.rocketTime = rocketTime;
			player.rocketDelay = rocketDelay;
			player.rocketDelay2 = rocketDelay2;
			player.wingTime = wingTime;
		}
		if (!_mountPreventedExtraJumps)
		{
			return;
		}
		foreach (ExtraJump jump in ExtraJumpLoader.ExtraJumps)
		{
			player.GetJumpState(jump).Available = canJumpAgain[jump.Type];
		}
	}
}
