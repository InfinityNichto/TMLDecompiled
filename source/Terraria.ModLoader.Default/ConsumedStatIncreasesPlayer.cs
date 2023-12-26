using System.IO;

namespace Terraria.ModLoader.Default;

internal class ConsumedStatIncreasesPlayer : ModPlayer
{
	internal static class NetHandler
	{
		public const byte SyncConsumedProperties = 1;

		public static void SendConsumedState(int toClient, Player player)
		{
			ModPacket packet = ModLoaderMod.GetPacket(1);
			packet.Write((byte)1);
			if (Main.netMode == 2)
			{
				packet.Write((byte)player.whoAmI);
			}
			packet.Write((byte)player.ConsumedLifeCrystals);
			packet.Write((byte)player.ConsumedLifeFruit);
			packet.Write((byte)player.ConsumedManaCrystals);
			packet.Send(toClient, player.whoAmI);
		}

		private static void HandleConsumedState(BinaryReader reader, int sender)
		{
			if (Main.netMode == 1)
			{
				sender = reader.ReadByte();
			}
			Player player = Main.player[sender];
			player.ConsumedLifeCrystals = reader.ReadByte();
			player.ConsumedLifeFruit = reader.ReadByte();
			player.ConsumedManaCrystals = reader.ReadByte();
			if (Main.netMode == 2)
			{
				SendConsumedState(-1, player);
			}
		}

		public static void HandlePacket(BinaryReader reader, int sender)
		{
			if (reader.ReadByte() == 1)
			{
				HandleConsumedState(reader, sender);
			}
		}
	}

	public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
	{
		NetHandler.SendConsumedState(toWho, base.Player);
	}

	public override void CopyClientState(ModPlayer targetCopy)
	{
		Player source = base.Player;
		Player player = targetCopy.Player;
		player.ConsumedLifeCrystals = source.ConsumedLifeCrystals;
		player.ConsumedLifeFruit = source.ConsumedLifeFruit;
		player.ConsumedManaCrystals = source.ConsumedManaCrystals;
	}

	public override void SendClientChanges(ModPlayer clientPlayer)
	{
		Player player = base.Player;
		Player client = clientPlayer.Player;
		if (player.ConsumedLifeCrystals != client.ConsumedLifeCrystals || player.ConsumedLifeFruit != client.ConsumedLifeFruit || player.ConsumedManaCrystals != client.ConsumedManaCrystals)
		{
			NetHandler.SendConsumedState(-1, player);
		}
	}
}
