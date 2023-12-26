using System.Collections.Generic;

namespace Terraria.DataStructures;

public class RecipeItemCreationContext : ItemCreationContext
{
	public readonly Recipe Recipe;

	/// <summary>
	/// An item stack that the created item will be combined with (via OnStack). For normal crafting, this is Main.mouseItem
	/// </summary>
	public Item DestinationStack;

	/// <summary>
	/// Cloned list of Items consumed when crafting.
	/// </summary>
	public readonly List<Item> ConsumedItems;

	public RecipeItemCreationContext(Recipe recipe, List<Item> consumedItems, Item destinationStack)
	{
		Recipe = recipe;
		ConsumedItems = consumedItems;
		DestinationStack = destinationStack;
	}
}
