using Terraria.ID;
using Terraria.Localization;

namespace Terraria.ModLoader.Default.Patreon;

[AutoloadEquip(new EquipType[] { EquipType.Face })]
internal class xAqult_Lens : PatreonItem
{
	public override LocalizedText Tooltip => this.GetLocalization("Tooltip", () => "");

	public override void Load()
	{
		if (Main.netMode != 2)
		{
			EquipLoader.AddEquipTexture(base.Mod, $"{Texture}_Blue_{12}", EquipType.Face, null, Name + "_Blue");
		}
	}

	public override void SetStaticDefaults()
	{
		base.SetStaticDefaults();
		if (Main.netMode != 2)
		{
			ArmorIDs.Face.Sets.DrawInFaceUnderHairLayer[base.Item.faceSlot] = true;
			ArmorIDs.Face.Sets.DrawInFaceUnderHairLayer[EquipLoader.GetEquipSlot(base.Mod, "xAqult_Lens_Blue", EquipType.Face)] = true;
		}
	}

	public override void SetDefaults()
	{
		base.SetDefaults();
		base.Item.width = 18;
		base.Item.height = 20;
		base.Item.accessory = true;
	}
}
