using System.ComponentModel;
using Newtonsoft.Json;
using Terraria.ModLoader.IO;

namespace Terraria.ModLoader.Config;

/// <summary>
/// Classes implementing EntityDefinition serve to function as a way to save and load the identities of various Terraria objects. Only the identity is preserved, no other data such as stack size, damage, etc. These classes are well suited for ModConfig, but can be saved and loaded in a TagCompound as well.
/// </summary>
public abstract class EntityDefinition : TagSerializable
{
	[DefaultValue("Terraria")]
	public string Mod;

	[DefaultValue("None")]
	public string Name;

	public bool IsUnloaded
	{
		get
		{
			if (Type <= 0)
			{
				if (!(Mod == "Terraria") || !(Name == "None"))
				{
					if (Mod == "")
					{
						return !(Name == "");
					}
					return true;
				}
				return false;
			}
			return false;
		}
	}

	[JsonIgnore]
	public abstract int Type { get; }

	public virtual string DisplayName => ToString();

	public EntityDefinition()
	{
		Mod = "Terraria";
		Name = "None";
	}

	public EntityDefinition(string mod, string name)
	{
		Mod = mod;
		Name = name;
	}

	public EntityDefinition(string key)
	{
		Mod = "Terraria";
		Name = key;
		string[] parts = key.Split('/', 2);
		if (parts.Length == 2)
		{
			Mod = parts[0];
			Name = parts[1];
		}
	}

	public override bool Equals(object obj)
	{
		if (!(obj is EntityDefinition p))
		{
			return false;
		}
		if (Mod == p.Mod)
		{
			return Name == p.Name;
		}
		return false;
	}

	public override string ToString()
	{
		return Mod + "/" + Name;
	}

	public override int GetHashCode()
	{
		return new { Mod, Name }.GetHashCode();
	}

	public TagCompound SerializeData()
	{
		return new TagCompound
		{
			["mod"] = Mod,
			["name"] = Name
		};
	}
}
