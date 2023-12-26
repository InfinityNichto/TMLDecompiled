using System;
using System.IO;
using System.Linq;
using System.Reflection;
using ReLogic.Content.Sources;
using Terraria.DataStructures;
using Terraria.ModLoader.Assets;
using Terraria.ModLoader.Default.Developer;
using Terraria.ModLoader.Default.Patreon;

namespace Terraria.ModLoader.Default;

internal class ModLoaderMod : Mod
{
	private static PatreonItem[][] PatronSets;

	private static DeveloperItem[][] DeveloperSets;

	private const int ChanceToGetPatreonArmor = 14;

	private const int ChanceToGetDevArmor = 50;

	internal const byte AccessorySlotPacket = 0;

	internal const byte StatResourcesPacket = 1;

	public override string Name => "ModLoader";

	public override Version Version => BuildInfo.tMLVersion;

	internal ModLoaderMod()
	{
		base.Side = ModSide.NoSync;
		base.DisplayName = "tModLoader";
		base.Code = Assembly.GetExecutingAssembly();
	}

	public override IContentSource CreateDefaultContentSource()
	{
		return new AssemblyResourcesContentSource(Assembly.GetExecutingAssembly(), "Terraria.ModLoader.Default.");
	}

	public override void Load()
	{
		PatronSets = (from t in GetContent<PatreonItem>()
			group t by t.InternalSetName into set
			select set.ToArray()).ToArray();
		DeveloperSets = (from t in GetContent<DeveloperItem>()
			group t by t.InternalSetName into set
			select set.ToArray()).ToArray();
	}

	public override void Unload()
	{
		PatronSets = null;
		DeveloperSets = null;
	}

	internal static bool TryGettingPatreonOrDevArmor(IEntitySource source, Player player)
	{
		if (Main.rand.NextBool(14))
		{
			int randomIndex = Main.rand.Next(PatronSets.Length);
			PatreonItem[] array = PatronSets[randomIndex];
			foreach (PatreonItem patreonItem in array)
			{
				player.QuickSpawnItem(source, patreonItem.Type);
			}
			return true;
		}
		if (Main.rand.NextBool(50))
		{
			int randomIndex2 = Main.rand.Next(DeveloperSets.Length);
			DeveloperItem[] array2 = DeveloperSets[randomIndex2];
			foreach (DeveloperItem developerItem in array2)
			{
				player.QuickSpawnItem(source, developerItem.Type);
			}
			return true;
		}
		return false;
	}

	public override void HandlePacket(BinaryReader reader, int whoAmI)
	{
		switch (reader.ReadByte())
		{
		case 0:
			ModAccessorySlotPlayer.NetHandler.HandlePacket(reader, whoAmI);
			break;
		case 1:
			ConsumedStatIncreasesPlayer.NetHandler.HandlePacket(reader, whoAmI);
			break;
		}
	}

	internal static ModPacket GetPacket(byte packetType)
	{
		ModPacket packet = ModContent.GetInstance<ModLoaderMod>().GetPacket();
		packet.Write(packetType);
		return packet;
	}
}
