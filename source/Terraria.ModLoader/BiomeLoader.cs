using System.Collections;
using System.IO;

namespace Terraria.ModLoader;

public class BiomeLoader : Loader<ModBiome>
{
	internal void SetupPlayer(Player player)
	{
		player.modBiomeFlags = new BitArray(list.Count);
	}

	public void UpdateBiomes(Player player)
	{
		for (int i = 0; i < player.modBiomeFlags.Length; i++)
		{
			bool prev = player.modBiomeFlags[i];
			bool flag2 = (player.modBiomeFlags[i] = list[i].IsBiomeActive(player));
			bool value = flag2;
			if (!prev && value)
			{
				list[i].OnEnter(player);
			}
			else if (!value && prev)
			{
				list[i].OnLeave(player);
			}
			if (value)
			{
				list[i].OnInBiome(player);
			}
		}
	}

	public static bool CustomBiomesMatch(Player player, Player other)
	{
		for (int i = 0; i < player.modBiomeFlags.Length; i++)
		{
			if (player.modBiomeFlags[i] != other.modBiomeFlags[i])
			{
				return false;
			}
		}
		return true;
	}

	public static void CopyCustomBiomesTo(Player player, Player other)
	{
		other.modBiomeFlags = (BitArray)player.modBiomeFlags.Clone();
	}

	public static void SendCustomBiomes(Player player, BinaryWriter writer)
	{
		Utils.SendBitArray(player.modBiomeFlags, writer);
	}

	public static void ReceiveCustomBiomes(Player player, BinaryReader reader)
	{
		player.modBiomeFlags = Utils.ReceiveBitArray(player.modBiomeFlags.Length, reader);
	}
}
