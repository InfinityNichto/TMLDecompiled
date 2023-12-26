using Terraria.ID;
using Terraria.ModLoader;

namespace Terraria.DataStructures;

/// <summary>
/// <b>Unused:</b> Replaced by <see cref="F:Terraria.ID.NPCID.Sets.SpecificDebuffImmunity" />, <see cref="F:Terraria.ID.NPCID.Sets.ImmuneToAllBuffs" />, and <see cref="F:Terraria.ID.NPCID.Sets.ImmuneToRegularBuffs" /><br /><br />
/// Determines the default debuff immunities of an <see cref="T:Terraria.NPC" />.
/// </summary>
public class NPCDebuffImmunityData
{
	/// <summary>
	/// If <see langword="true" />, this NPC type (<see cref="F:Terraria.NPC.type" />) will be immune to all tag debuffs (<see cref="F:Terraria.ID.BuffID.Sets.IsATagBuff" />).
	/// </summary>
	public bool ImmuneToWhips;

	/// <summary>
	/// If <see langword="true" />, this NPC type (<see cref="F:Terraria.NPC.type" />) will be immune to all non-tag debuffs (<see cref="F:Terraria.ID.BuffID.Sets.IsATagBuff" />).
	/// </summary>
	public bool ImmuneToAllBuffsThatAreNotWhips;

	/// <summary>
	/// This NPC type (<see cref="F:Terraria.NPC.type" />) will be immune to all <see cref="T:Terraria.ID.BuffID" />s in this array.
	/// </summary>
	public int[] SpecificallyImmuneTo;

	/// <summary>
	/// Sets up <see cref="F:Terraria.NPC.buffImmune" /> to be immune to the stored buffs.
	/// </summary>
	/// <param name="npc">The NPC to apply immunities to.</param>
	public void ApplyToNPC(NPC npc)
	{
		if (ImmuneToWhips || ImmuneToAllBuffsThatAreNotWhips)
		{
			for (int i = 1; i < BuffLoader.BuffCount; i++)
			{
				bool flag = BuffID.Sets.IsATagBuff[i];
				bool flag2 = false;
				flag2 |= flag && ImmuneToWhips;
				flag2 |= !flag && ImmuneToAllBuffsThatAreNotWhips;
				npc.buffImmune[i] = flag2;
			}
		}
		if (SpecificallyImmuneTo != null)
		{
			for (int j = 0; j < SpecificallyImmuneTo.Length; j++)
			{
				int num = SpecificallyImmuneTo[j];
				npc.buffImmune[num] = true;
			}
		}
	}
}
