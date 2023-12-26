using Terraria.DataStructures;

namespace Terraria.ModLoader.Default.Patreon;

internal class xAqultPlayer : ModPlayer
{
	public override void ModifyDrawInfo(ref PlayerDrawSet drawInfo)
	{
		if (base.Player.head == EquipLoader.GetEquipSlot(base.Mod, "xAqult_Mask", EquipType.Head) && base.Player.face < 0)
		{
			drawInfo.drawPlayer.face = EquipLoader.GetEquipSlot(base.Mod, "xAqult_Lens", EquipType.Face);
		}
		if (base.Player.face == EquipLoader.GetEquipSlot(base.Mod, "xAqult_Lens", EquipType.Face) && base.Player.direction == -1)
		{
			drawInfo.drawPlayer.face = EquipLoader.GetEquipSlot(base.Mod, "xAqult_Lens_Blue", EquipType.Face);
		}
	}
}
