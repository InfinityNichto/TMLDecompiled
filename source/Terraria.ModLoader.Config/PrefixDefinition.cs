using System;
using System.ComponentModel;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader.IO;

namespace Terraria.ModLoader.Config;

[TypeConverter(typeof(ToFromStringConverter<PrefixDefinition>))]
public class PrefixDefinition : EntityDefinition
{
	public static readonly Func<TagCompound, PrefixDefinition> DESERIALIZER = Load;

	public override int Type
	{
		get
		{
			if (Mod == "Terraria" && Name == "None")
			{
				return 0;
			}
			if (!PrefixID.Search.TryGetId((Mod != "Terraria") ? (Mod + "/" + Name) : Name, out var id))
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
			if (base.IsUnloaded)
			{
				return Language.GetTextValue("Mods.ModLoader.Unloaded");
			}
			if (Type == 0)
			{
				return Lang.inter[23].Value;
			}
			return Lang.prefix[Type].Value;
		}
	}

	public PrefixDefinition()
	{
	}

	/// <summary><b>Note: </b>As ModConfig loads before other content, make sure to only use <see cref="M:Terraria.ModLoader.Config.PrefixDefinition.#ctor(System.String,System.String)" /> for modded content in ModConfig classes. </summary>
	public PrefixDefinition(int type)
		: base(PrefixID.Search.GetName(type))
	{
	}

	public PrefixDefinition(string key)
		: base(key)
	{
	}

	public PrefixDefinition(string mod, string name)
		: base(mod, name)
	{
	}

	public static PrefixDefinition FromString(string s)
	{
		return new PrefixDefinition(s);
	}

	public static PrefixDefinition Load(TagCompound tag)
	{
		return new PrefixDefinition(tag.GetString("mod"), tag.GetString("name"));
	}
}
