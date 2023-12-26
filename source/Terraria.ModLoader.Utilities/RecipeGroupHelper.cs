using System;
using System.Reflection;

namespace Terraria.ModLoader.Utilities;

internal static class RecipeGroupHelper
{
	internal static void ResetRecipeGroups()
	{
		RecipeGroup.recipeGroups.Clear();
		RecipeGroup.recipeGroupIDs.Clear();
		RecipeGroup.nextRecipeGroupIndex = 0;
	}

	internal static void AddRecipeGroups()
	{
		MethodInfo addRecipeGroupsMethod = typeof(Mod).GetMethod("AddRecipeGroups", BindingFlags.Instance | BindingFlags.Public);
		Mod[] mods = ModLoader.Mods;
		foreach (Mod mod in mods)
		{
			try
			{
				addRecipeGroupsMethod.Invoke(mod, Array.Empty<object>());
				SystemLoader.AddRecipeGroups(mod);
			}
			catch (Exception ex)
			{
				ex.Data["mod"] = mod.Name;
				throw;
			}
		}
		CreateRecipeGroupLookups();
	}

	internal static void CreateRecipeGroupLookups()
	{
		for (int i = 0; i < RecipeGroup.nextRecipeGroupIndex; i++)
		{
			RecipeGroup rec = RecipeGroup.recipeGroups[i];
			rec.ValidItemsLookup = new bool[ItemLoader.ItemCount];
			foreach (int type in rec.ValidItems)
			{
				rec.ValidItemsLookup[type] = true;
			}
		}
	}
}
