using Terraria.ID;
using Terraria.ModLoader;

namespace Terraria.GameContent;

public static class ShimmerTransforms
{
	public static class RecipeSets
	{
		public static bool[] PostSkeletron;

		public static bool[] PostGolem;
	}

	public static int GetDecraftingRecipeIndex(int type)
	{
		int[] array = ItemID.Sets.CraftingRecipeIndices[type];
		foreach (int recipeIndex in array)
		{
			if (RecipeLoader.DecraftAvailable(Main.recipe[recipeIndex]))
			{
				return recipeIndex;
			}
		}
		return -1;
	}

	public static bool IsItemTransformLocked(int type)
	{
		int decraftingRecipeIndex = GetDecraftingRecipeIndex(type);
		if (decraftingRecipeIndex < 0)
		{
			return false;
		}
		if (!NPC.downedBoss3 && RecipeSets.PostSkeletron[decraftingRecipeIndex])
		{
			return true;
		}
		if (!NPC.downedGolemBoss && RecipeSets.PostGolem[decraftingRecipeIndex])
		{
			return true;
		}
		if (!RecipeLoader.DecraftAvailable(Main.recipe[decraftingRecipeIndex]))
		{
			return true;
		}
		return false;
	}

	public static void UpdateRecipeSets()
	{
		RecipeSets.PostSkeletron = Utils.MapArray(Main.recipe, (Recipe r) => r.ContainsIngredient(154));
		RecipeSets.PostGolem = Utils.MapArray(Main.recipe, (Recipe r) => r.ContainsIngredient(1101));
	}
}
