using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader.Core;

namespace Terraria.ModLoader;

/// <summary>
/// This is where all Recipe hooks are gathered and called.
/// </summary>
public static class RecipeLoader
{
	internal static Recipe[] FirstRecipeForItem = new Recipe[ItemID.Count];

	/// <summary>
	/// Cloned list of Items consumed when crafting.  Cleared after the OnCreate and OnCraft hooks.
	/// </summary>
	internal static List<Item> ConsumedItems = new List<Item>();

	/// <summary>
	/// Set when tML sets up modded recipes. Used to detect misuse of CreateRecipe
	/// </summary>
	internal static bool setupRecipes = false;

	/// <summary>
	/// The mod currently adding recipes.
	/// </summary>
	internal static Mod CurrentMod { get; private set; }

	internal static void Unload()
	{
		setupRecipes = false;
		FirstRecipeForItem = new Recipe[Recipe.maxRecipes];
	}

	internal static void AddRecipes()
	{
		MethodInfo addRecipesMethod = typeof(Mod).GetMethod("AddRecipes", BindingFlags.Instance | BindingFlags.Public);
		Mod[] mods = ModLoader.Mods;
		for (int i = 0; i < mods.Length; i++)
		{
			Mod mod = (CurrentMod = mods[i]);
			try
			{
				addRecipesMethod.Invoke(mod, Array.Empty<object>());
				SystemLoader.AddRecipes(mod);
				LoaderUtils.ForEachAndAggregateExceptions(mod.GetContent<ModItem>(), delegate(ModItem item)
				{
					item.AddRecipes();
				});
				LoaderUtils.ForEachAndAggregateExceptions(mod.GetContent<GlobalItem>(), delegate(GlobalItem global)
				{
					global.AddRecipes();
				});
			}
			catch (Exception ex)
			{
				ex.Data["mod"] = mod.Name;
				throw;
			}
			finally
			{
				CurrentMod = null;
			}
		}
	}

	internal static void PostAddRecipes()
	{
		MethodInfo postAddRecipesMethod = typeof(Mod).GetMethod("PostAddRecipes", BindingFlags.Instance | BindingFlags.Public);
		Mod[] mods = ModLoader.Mods;
		for (int i = 0; i < mods.Length; i++)
		{
			Mod mod = (CurrentMod = mods[i]);
			try
			{
				postAddRecipesMethod.Invoke(mod, Array.Empty<object>());
				SystemLoader.PostAddRecipes(mod);
			}
			catch (Exception ex)
			{
				ex.Data["mod"] = mod.Name;
				throw;
			}
			finally
			{
				CurrentMod = null;
			}
		}
	}

	internal static void PostSetupRecipes()
	{
		Mod[] mods = ModLoader.Mods;
		for (int i = 0; i < mods.Length; i++)
		{
			Mod mod = (CurrentMod = mods[i]);
			try
			{
				SystemLoader.PostSetupRecipes(mod);
			}
			catch (Exception ex)
			{
				ex.Data["mod"] = mod.Name;
				throw;
			}
			finally
			{
				CurrentMod = null;
			}
		}
	}

	/// <summary>
	/// Orders everything in the recipe according to their Ordering.
	/// </summary>
	internal static void OrderRecipes()
	{
		Dictionary<Recipe, List<Recipe>> sortBefore = new Dictionary<Recipe, List<Recipe>>();
		Dictionary<Recipe, List<Recipe>> sortAfter = new Dictionary<Recipe, List<Recipe>>();
		List<Recipe> baseOrder = new List<Recipe>(Main.recipe.Length);
		Recipe[] recipe = Main.recipe;
		foreach (Recipe r2 in recipe)
		{
			(Recipe, bool) ordering = r2.Ordering;
			var (target2, _) = ordering;
			if (target2 != null)
			{
				if (!ordering.Item2)
				{
					if (!sortBefore.TryGetValue(target2, out var before))
					{
						List<Recipe> list2 = (sortBefore[target2] = new List<Recipe>());
						before = list2;
					}
					before.Add(r2);
					continue;
				}
				Recipe target = target2;
				if (!sortAfter.TryGetValue(target, out var after))
				{
					List<Recipe> list2 = (sortAfter[target] = new List<Recipe>());
					after = list2;
				}
				after.Add(r2);
			}
			else
			{
				baseOrder.Add(r2);
			}
		}
		if (!sortBefore.Any() && !sortAfter.Any())
		{
			return;
		}
		int i = 0;
		foreach (Recipe item in baseOrder)
		{
			Sort(item);
		}
		if (i != Main.recipe.Length)
		{
			throw new Exception("Sorting code is broken?");
		}
		void Sort(Recipe r)
		{
			if (sortBefore.TryGetValue(r, out var before2))
			{
				foreach (Recipe item2 in before2)
				{
					Sort(item2);
				}
			}
			r.RecipeIndex = i;
			Main.recipe[i++] = r;
			if (sortAfter.TryGetValue(r, out var after2))
			{
				foreach (Recipe item3 in after2)
				{
					Sort(item3);
				}
			}
		}
	}

	/// <summary>
	/// Returns whether or not the conditions are met for this recipe to be available for the player to use.
	/// </summary>
	/// <param name="recipe">The recipe to check.</param>
	/// <returns>Whether or not the conditions are met for this recipe.</returns>
	public static bool RecipeAvailable(Recipe recipe)
	{
		return recipe.Conditions.All((Condition c) => c.IsMet());
	}

	/// <summary>
	/// Returns whether or not the conditions are met for this recipe to be shimmered/decrafted.
	/// </summary>
	/// <param name="recipe">The recipe to check.</param>
	/// <returns>Whether or not the conditions are met for this recipe.</returns>
	public static bool DecraftAvailable(Recipe recipe)
	{
		return recipe.DecraftConditions.All((Condition c) => c.IsMet());
	}

	/// <summary>
	/// recipe.OnCraftHooks followed by Calls ItemLoader.OnCreate with a RecipeCreationContext
	/// </summary>
	/// <param name="item">The item crafted.</param>
	/// <param name="recipe">The recipe used to craft the item.</param>
	/// <param name="consumedItems">Materials used to craft the item.</param>
	/// <param name="destinationStack">The stack that the crafted item will be combined with</param>
	public static void OnCraft(Item item, Recipe recipe, List<Item> consumedItems, Item destinationStack)
	{
		recipe.OnCraftHooks?.Invoke(recipe, item, consumedItems, destinationStack);
		item.OnCreated(new RecipeItemCreationContext(recipe, consumedItems, destinationStack));
	}

	/// <summary>
	/// Helper version of OnCraft, used in combination with Recipe.Create and the internal ConsumedItems list
	/// </summary>
	/// <param name="item"></param>
	/// <param name="recipe"></param>
	/// <param name="destinationStack">The stack that the crafted item will be combined with</param>
	public static void OnCraft(Item item, Recipe recipe, Item destinationStack)
	{
		OnCraft(item, recipe, ConsumedItems, destinationStack);
		ConsumedItems.Clear();
	}

	/// <summary>
	/// Allows to edit the amount of item the player uses in a recipe.
	/// </summary>
	/// <param name="recipe">The recipe used for the craft.</param>
	/// <param name="type">Type of the ingredient.</param>
	/// <param name="amount">Modifiable amount of the item consumed.</param>
	public static void ConsumeItem(Recipe recipe, int type, ref int amount)
	{
		recipe.ConsumeItemHooks?.Invoke(recipe, type, ref amount);
	}
}
