using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Enums;

namespace Terraria.ModLoader;

public static class PlantLoader
{
	internal static Dictionary<Vector2, IPlant> plantLookup = new Dictionary<Vector2, IPlant>();

	internal static List<IPlant> plantList = new List<IPlant>();

	internal static Dictionary<int, int> plantIdToStyleLimit = new Dictionary<int, int>();

	internal static void FinishSetup()
	{
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		Vector2 id = default(Vector2);
		foreach (IPlant plant in plantList)
		{
			plant.SetStaticDefaults();
			for (int i = 0; i < plant.GrowsOnTileId.Length; i++)
			{
				((Vector2)(ref id))._002Ector((float)plant.PlantTileId, (float)plant.GrowsOnTileId[i]);
				if (plantLookup.TryGetValue(id, out var existing))
				{
					Logging.tML.Error((object)$"The new plant {plant.GetType()} conflicts with the existing plant {existing.GetType()}. New plant not added");
				}
				else
				{
					if (!plantIdToStyleLimit.ContainsKey((int)id.X))
					{
						plantIdToStyleLimit.Add((int)id.X, plant.VanillaCount);
					}
					plantLookup.Add(id, plant);
				}
			}
		}
	}

	internal static void UnloadPlants()
	{
		plantList.Clear();
		plantLookup.Clear();
	}

	public static T Get<T>(int plantTileID, int growsOnTileID) where T : IPlant
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		if (!plantLookup.TryGetValue(new Vector2((float)plantTileID, (float)growsOnTileID), out var plant))
		{
			return default(T);
		}
		return (T)plant;
	}

	public static bool Exists(int plantTileID, int growsOnTileID)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		return plantLookup.ContainsKey(new Vector2((float)plantTileID, (float)growsOnTileID));
	}

	public static Asset<Texture2D> GetCactusFruitTexture(int type)
	{
		return Get<ModCactus>(80, type)?.GetFruitTexture();
	}

	public static Asset<Texture2D> GetTexture(int plantId, int tileType)
	{
		return Get<IPlant>(plantId, tileType)?.GetTexture();
	}

	public static ITree GetTree(int type)
	{
		ModTree tree = Get<ModTree>(5, type);
		if (tree != null)
		{
			return tree;
		}
		ModPalmTree palm = Get<ModPalmTree>(323, type);
		if (palm != null)
		{
			return palm;
		}
		return null;
	}

	public static TreeTypes GetModTreeType(int type)
	{
		return GetTree(type)?.CountsAsTreeType ?? TreeTypes.None;
	}

	public static bool ShakeTree(int x, int y, int type, ref bool createLeaves)
	{
		return GetTree(type)?.Shake(x, y, ref createLeaves) ?? true;
	}

	public static void GetTreeLeaf(int type, ref int leafGoreType)
	{
		ITree tree = GetTree(type);
		if (tree != null)
		{
			leafGoreType = tree.TreeLeaf();
		}
	}

	public static void CheckAndInjectModSapling(int x, int y, ref int tileToCreate, ref int previewPlaceStyle)
	{
		if (tileToCreate == 20)
		{
			Tile soil = Main.tile[x, y + 1];
			if (soil.active())
			{
				TileLoader.SaplingGrowthType(soil.type, ref tileToCreate, ref previewPlaceStyle);
			}
		}
	}
}
