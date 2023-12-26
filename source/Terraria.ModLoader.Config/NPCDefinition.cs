using System;
using System.ComponentModel;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader.IO;

namespace Terraria.ModLoader.Config;

[TypeConverter(typeof(ToFromStringConverter<NPCDefinition>))]
public class NPCDefinition : EntityDefinition
{
	public static readonly Func<TagCompound, NPCDefinition> DESERIALIZER = Load;

	public override int Type
	{
		get
		{
			if (!NPCID.Search.TryGetId((Mod != "Terraria") ? (Mod + "/" + Name) : Name, out var id))
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
				return Lang.GetNPCNameValue(Type);
			}
			return Language.GetTextValue("Mods.ModLoader.Unloaded");
		}
	}

	public NPCDefinition()
	{
	}

	/// <summary><b>Note: </b>As ModConfig loads before other content, make sure to only use <see cref="M:Terraria.ModLoader.Config.NPCDefinition.#ctor(System.String,System.String)" /> for modded content in ModConfig classes. </summary>
	public NPCDefinition(int type)
		: base(NPCID.Search.GetName(type))
	{
	}

	public NPCDefinition(string key)
		: base(key)
	{
	}

	public NPCDefinition(string mod, string name)
		: base(mod, name)
	{
	}

	public static NPCDefinition FromString(string s)
	{
		return new NPCDefinition(s);
	}

	public static NPCDefinition Load(TagCompound tag)
	{
		return new NPCDefinition(tag.GetString("mod"), tag.GetString("name"));
	}
}
