using System;
using System.ComponentModel;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader.IO;

namespace Terraria.ModLoader.Config;

/// <summary>
/// ItemDefinition represents an Item identity. A typical use for this class is usage in ModConfig, perhaps to facilitate an Item tweaking mod.
/// </summary>
[TypeConverter(typeof(ToFromStringConverter<ItemDefinition>))]
public class ItemDefinition : EntityDefinition
{
	public static readonly Func<TagCompound, ItemDefinition> DESERIALIZER = Load;

	public override int Type
	{
		get
		{
			if (!ItemID.Search.TryGetId((Mod != "Terraria") ? (Mod + "/" + Name) : Name, out var id))
			{
				return -1;
			}
			return id;
		}
	}

	public override string DisplayName
	{
		get
		{
			if (!base.IsUnloaded)
			{
				return Lang.GetItemNameValue(Type);
			}
			return Language.GetTextValue("Mods.ModLoader.Items.UnloadedItem.DisplayName");
		}
	}

	public ItemDefinition()
	{
	}

	/// <summary><b>Note: </b>As ModConfig loads before other content, make sure to only use <see cref="M:Terraria.ModLoader.Config.ItemDefinition.#ctor(System.String,System.String)" /> for modded content in ModConfig classes. </summary>
	public ItemDefinition(int type)
		: base(ItemID.Search.GetName(type))
	{
	}

	public ItemDefinition(string key)
		: base(key)
	{
	}

	public ItemDefinition(string mod, string name)
		: base(mod, name)
	{
	}

	public static ItemDefinition FromString(string s)
	{
		return new ItemDefinition(s);
	}

	public static ItemDefinition Load(TagCompound tag)
	{
		return new ItemDefinition(tag.GetString("mod"), tag.GetString("name"));
	}
}
