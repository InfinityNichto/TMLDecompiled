using System;
using System.Collections.Generic;
using System.Linq;
using Terraria.Audio;
using Terraria.DataStructures;

namespace Terraria.ModLoader;

public static class ExtraJumpLoader
{
	internal static readonly List<ExtraJump> ExtraJumps;

	private static readonly int DefaultExtraJumpCount;

	private static ExtraJump[] orderedJumps;

	public static int ExtraJumpCount => ExtraJumps.Count;

	private static IEnumerable<ExtraJump> ModdedExtraJumps => ExtraJumps.Skip(DefaultExtraJumpCount);

	public static IReadOnlyList<ExtraJump> OrderedJumps => orderedJumps;

	static ExtraJumpLoader()
	{
		ExtraJumps = new List<ExtraJump>
		{
			ExtraJump.Flipper,
			ExtraJump.BasiliskMount,
			ExtraJump.GoatMount,
			ExtraJump.SantankMount,
			ExtraJump.UnicornMount,
			ExtraJump.SandstormInABottle,
			ExtraJump.BlizzardInABottle,
			ExtraJump.FartInAJar,
			ExtraJump.TsunamiInABottle,
			ExtraJump.CloudInABottle
		};
		DefaultExtraJumpCount = ExtraJumps.Count;
		RegisterDefaultJumps();
	}

	internal static int Add(ExtraJump jump)
	{
		ExtraJumps.Add(jump);
		return ExtraJumps.Count - 1;
	}

	public static ExtraJump Get(int index)
	{
		if (index >= 0 && index < ExtraJumpCount)
		{
			return ExtraJumps[index];
		}
		return null;
	}

	internal static void Unload()
	{
		ExtraJumps.RemoveRange(DefaultExtraJumpCount, ExtraJumpCount - DefaultExtraJumpCount);
	}

	internal static void ResizeArrays()
	{
		if (!ModdedExtraJumps.Any())
		{
			orderedJumps = ExtraJumps.ToArray();
			return;
		}
		List<ExtraJump>[] sortingSlots = new List<ExtraJump>[DefaultExtraJumpCount + 1];
		for (int k = 0; k < sortingSlots.Length; k++)
		{
			sortingSlots[k] = new List<ExtraJump>();
		}
		foreach (ExtraJump jump2 in ModdedExtraJumps)
		{
			ExtraJump.Position position = jump2.GetDefaultPosition();
			if (!(position is ExtraJump.After { Target: var target } after))
			{
				if (!(position is ExtraJump.Before { Target: var target2 } before))
				{
					throw new ArgumentException($"ExtraJump {jump2} has unknown Position {position}");
				}
				if (target2 != null && !(target2 is VanillaExtraJump))
				{
					throw new ArgumentException($"ExtraJump {jump2} did not refer to a vanilla ExtraJump in GetDefaultPosition()");
				}
				int? num = before.Target?.Type;
				int num2;
				if (num.HasValue)
				{
					int beforeType = num.GetValueOrDefault();
					num2 = beforeType;
				}
				else
				{
					num2 = sortingSlots.Length - 1;
				}
				int beforeParent = num2;
				sortingSlots[beforeParent].Add(jump2);
			}
			else
			{
				if (target != null && !(target is VanillaExtraJump))
				{
					throw new ArgumentException($"ExtraJump {jump2} did not refer to a vanilla ExtraJump in GetDefaultPosition()");
				}
				int? num = after.Target?.Type;
				int num3;
				if (num.HasValue)
				{
					int afterType = num.GetValueOrDefault();
					num3 = afterType + 1;
				}
				else
				{
					num3 = 0;
				}
				int afterParent = num3;
				sortingSlots[afterParent].Add(jump2);
			}
		}
		List<ExtraJump> sorted = new List<ExtraJump>();
		for (int i = 0; i < DefaultExtraJumpCount + 1; i++)
		{
			List<ExtraJump> elements = sortingSlots[i];
			foreach (ExtraJump jump in new TopoSort<ExtraJump>(elements, (ExtraJump j) => (from a in j.GetModdedConstraints()?.OfType<ExtraJump.After>()
				select a.Target).Where(elements.Contains) ?? Array.Empty<ExtraJump>(), (ExtraJump j) => (from b in j.GetModdedConstraints()?.OfType<ExtraJump.Before>()
				select b.Target).Where(elements.Contains) ?? Array.Empty<ExtraJump>()).Sort())
			{
				sorted.Add(jump);
			}
			if (i < DefaultExtraJumpCount)
			{
				sorted.Add(ExtraJumps[i]);
			}
		}
		orderedJumps = sorted.ToArray();
	}

	internal static void RegisterDefaultJumps()
	{
		int i = 0;
		foreach (ExtraJump extraJump in ExtraJumps)
		{
			extraJump.Type = i++;
			ContentInstance.Register(extraJump);
			ModTypeLookup<ExtraJump>.Register(extraJump);
		}
	}

	public static void UpdateHorizontalSpeeds(Player player)
	{
		ExtraJump[] array = orderedJumps;
		foreach (ExtraJump moddedExtraJump in array)
		{
			if (player.GetJumpState(moddedExtraJump).Active)
			{
				moddedExtraJump.UpdateHorizontalSpeeds(player);
			}
		}
	}

	public static void JumpVisuals(Player player)
	{
		ExtraJump[] array = orderedJumps;
		foreach (ExtraJump jump in array)
		{
			if (player.GetJumpState(jump).Active && jump.CanShowVisuals(player) && PlayerLoader.CanShowExtraJumpVisuals(jump, player))
			{
				jump.ShowVisuals(player);
				PlayerLoader.ExtraJumpVisuals(jump, player);
			}
		}
	}

	public static void ProcessJumps(Player player)
	{
		ExtraJump[] array = orderedJumps;
		foreach (ExtraJump jump in array)
		{
			ref ExtraJumpState state = ref player.GetJumpState(jump);
			if (state.Available && jump.CanStart(player) && PlayerLoader.CanStartExtraJump(jump, player))
			{
				state.Start();
				PerformJump(jump, player);
				break;
			}
		}
	}

	public static void RefreshJumps(Player player)
	{
		ExtraJump[] array = orderedJumps;
		foreach (ExtraJump jump in array)
		{
			ref ExtraJumpState state = ref player.GetJumpState(jump);
			if (state.Enabled)
			{
				jump.OnRefreshed(player);
				PlayerLoader.OnExtraJumpRefreshed(jump, player);
				state.Available = true;
			}
		}
	}

	internal static void StopActiveJump(Player player, out bool anyJumpCancelled)
	{
		anyJumpCancelled = false;
		ExtraJump[] array = orderedJumps;
		foreach (ExtraJump jump in array)
		{
			if (player.GetJumpState(jump).Active)
			{
				StopJump(jump, player);
				anyJumpCancelled = true;
			}
		}
	}

	internal static void ResetEnableFlags(Player player)
	{
		foreach (ExtraJump jump in ExtraJumps)
		{
			player.GetJumpState(jump).ResetEnabled();
		}
	}

	internal static void ConsumeAndStopUnavailableJumps(Player player)
	{
		foreach (ExtraJump jump in ExtraJumps)
		{
			player.GetJumpState(jump).CommitEnabledState(out var jumpEnded);
			if (jumpEnded)
			{
				StopJump(jump, player);
				player.jump = 0;
			}
		}
	}

	internal static void ConsumeAllJumps(Player player)
	{
		foreach (ExtraJump jump in ExtraJumps)
		{
			player.GetJumpState(jump).Available = false;
		}
	}

	private static void PerformJump(ExtraJump jump, Player player)
	{
		float duration = jump.GetDurationMultiplier(player);
		PlayerLoader.ModifyExtraJumpDurationMultiplier(jump, player, ref duration);
		player.velocity.Y = (0f - Player.jumpSpeed) * player.gravDir;
		player.jump = (int)((float)Player.jumpHeight * duration);
		bool playSound = true;
		jump.OnStarted(player, ref playSound);
		PlayerLoader.OnExtraJumpStarted(jump, player, ref playSound);
		if (playSound)
		{
			SoundEngine.PlaySound(16, (int)player.position.X, (int)player.position.Y);
		}
	}

	private static void StopJump(ExtraJump jump, Player player)
	{
		jump.OnEnded(player);
		PlayerLoader.OnExtraJumpEnded(jump, player);
		player.GetJumpState(jump).Stop();
	}
}
