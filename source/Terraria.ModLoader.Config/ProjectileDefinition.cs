using System;
using System.ComponentModel;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader.IO;

namespace Terraria.ModLoader.Config;

[TypeConverter(typeof(ToFromStringConverter<ProjectileDefinition>))]
public class ProjectileDefinition : EntityDefinition
{
	public static readonly Func<TagCompound, ProjectileDefinition> DESERIALIZER = Load;

	public override int Type
	{
		get
		{
			if (!ProjectileID.Search.TryGetId((Mod != "Terraria") ? (Mod + "/" + Name) : Name, out var id))
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
				return Lang.GetProjectileName(Type).Value;
			}
			return Language.GetTextValue("Mods.ModLoader.Unloaded");
		}
	}

	public ProjectileDefinition()
	{
	}

	/// <summary><b>Note: </b>As ModConfig loads before other content, make sure to only use <see cref="M:Terraria.ModLoader.Config.ProjectileDefinition.#ctor(System.String,System.String)" /> for modded content in ModConfig classes. </summary>
	public ProjectileDefinition(int type)
		: base(ProjectileID.Search.GetName(type))
	{
	}

	public ProjectileDefinition(string key)
		: base(key)
	{
	}

	public ProjectileDefinition(string mod, string name)
		: base(mod, name)
	{
	}

	public static ProjectileDefinition FromString(string s)
	{
		return new ProjectileDefinition(s);
	}

	public static ProjectileDefinition Load(TagCompound tag)
	{
		return new ProjectileDefinition(tag.GetString("mod"), tag.GetString("name"));
	}
}
