using Terraria.Localization;

namespace Terraria.GameContent.Items;

/// <summary>
/// Describes a variant of an <see cref="T:Terraria.Item" />.
/// </summary>
public class ItemVariant
{
	/// <summary>
	/// The localized description of this <see cref="T:Terraria.GameContent.Items.ItemVariant" />.
	/// </summary>
	public readonly LocalizedText Description;

	/// <summary>
	/// Creates a new <see cref="T:Terraria.GameContent.Items.ItemVariant" /> with the provided localized description.
	/// </summary>
	/// <param name="description">The localized description to use.</param>
	public ItemVariant(LocalizedText description)
	{
		Description = description;
	}

	public override string ToString()
	{
		return Description.ToString();
	}
}
