using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace Terraria.GameContent.ObjectInteractions;

public class NPCSmartInteractCandidateProvider : ISmartInteractCandidateProvider
{
	private class ReusableCandidate : ISmartInteractCandidate
	{
		private int _npcIndexToTarget;

		public float DistanceFromCursor { get; private set; }

		public void WinCandidacy()
		{
			Main.SmartInteractNPC = _npcIndexToTarget;
			Main.SmartInteractShowingGenuine = true;
		}

		public void Reuse(int npcIndex, float npcDistanceFromCursor)
		{
			_npcIndexToTarget = npcIndex;
			DistanceFromCursor = npcDistanceFromCursor;
		}
	}

	private ReusableCandidate _candidate = new ReusableCandidate();

	public void ClearSelfAndPrepareForCheck()
	{
		Main.SmartInteractNPC = -1;
	}

	public bool ProvideCandidate(SmartInteractScanSettings settings, out ISmartInteractCandidate candidate)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		candidate = null;
		if (!settings.FullInteraction)
		{
			return false;
		}
		Rectangle value = Utils.CenteredRectangle(settings.player.Center, new Vector2((float)Player.tileRangeX, (float)Player.tileRangeY) * 16f * 2f);
		Vector2 mousevec = settings.mousevec;
		mousevec.ToPoint();
		bool flag = false;
		int num = -1;
		float npcDistanceFromCursor = -1f;
		for (int i = 0; i < 200; i++)
		{
			NPC nPC = Main.npc[i];
			if (!nPC.active || !(NPCLoader.CanChat(nPC) ?? nPC.townNPC))
			{
				continue;
			}
			Rectangle hitbox = nPC.Hitbox;
			if (((Rectangle)(ref hitbox)).Intersects(value) && !flag)
			{
				float num2 = nPC.Hitbox.Distance(mousevec);
				if (num == -1 || Main.npc[num].Hitbox.Distance(mousevec) > num2)
				{
					num = i;
					npcDistanceFromCursor = num2;
				}
				if (num2 == 0f)
				{
					flag = true;
					num = i;
					npcDistanceFromCursor = num2;
					break;
				}
			}
		}
		if (settings.DemandOnlyZeroDistanceTargets && !flag)
		{
			return false;
		}
		if (num != -1)
		{
			_candidate.Reuse(num, npcDistanceFromCursor);
			candidate = _candidate;
			return true;
		}
		return false;
	}
}
