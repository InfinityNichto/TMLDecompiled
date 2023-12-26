using System;
using System.Collections.Generic;
using System.Linq;
using Terraria.Localization;
using Terraria.ModLoader.Core;

namespace Terraria.ModLoader;

/// <summary>
/// Enables content instances to register with <see cref="M:Terraria.ModLoader.ModTypeLookup`1.Register(`0)" /> towards retrieval via <see cref="M:Terraria.ModLoader.ModContent.Find``1(System.String)" /> and similar methods.
/// </summary>
public static class ModTypeLookup<T> where T : IModType
{
	private static readonly Dictionary<string, T> dict;

	private static readonly Dictionary<string, Dictionary<string, T>> tieredDict;

	static ModTypeLookup()
	{
		dict = new Dictionary<string, T>();
		tieredDict = new Dictionary<string, Dictionary<string, T>>();
		if (typeof(T).Assembly == typeof(ModTypeLookup<>).Assembly)
		{
			TypeCaching.OnClear += delegate
			{
				dict.Clear();
				tieredDict.Clear();
			};
		}
	}

	/// <summary>
	/// Registers the instance towards lookup via <see cref="M:Terraria.ModLoader.ModContent.Find``1(System.String)" /> and similar methods.
	/// <br />Should only be called once per instance. Registers legacy names specified via <see cref="T:Terraria.ModLoader.LegacyNameAttribute" /> on the instance's type automatically.
	/// </summary>
	public static void Register(T instance)
	{
		RegisterWithName(instance, instance.Name, instance.FullName);
		RegisterLegacyNames(instance, LegacyNameAttribute.GetLegacyNamesOfType(instance.GetType()).ToArray());
	}

	/// <summary>
	/// Registers the instance towards lookup via <see cref="M:Terraria.ModLoader.ModContent.Find``1(System.String)" /> and similar methods using any number of specified <paramref name="legacyNames" />.
	/// <br />Also see <seealso cref="T:Terraria.ModLoader.LegacyNameAttribute" /> which may be more convenient.
	/// </summary>
	public static void RegisterLegacyNames(T instance, params string[] legacyNames)
	{
		foreach (string legacyName in legacyNames)
		{
			RegisterWithName(instance, legacyName, (instance.Mod?.Name ?? "Terraria") + "/" + legacyName);
		}
	}

	private static void RegisterWithName(T instance, string name, string fullName)
	{
		if (dict.ContainsKey(fullName))
		{
			throw new Exception(Language.GetTextValue("tModLoader.LoadErrorDuplicateName", typeof(T).Name, fullName));
		}
		dict[fullName] = instance;
		string modName = instance.Mod?.Name ?? "Terraria";
		if (!tieredDict.TryGetValue(modName, out var subDictionary))
		{
			subDictionary = (tieredDict[modName] = new Dictionary<string, T>());
		}
		subDictionary[name] = instance;
	}

	internal static T Get(string fullName)
	{
		return dict[fullName];
	}

	internal static T Get(string modName, string contentName)
	{
		return tieredDict[modName][contentName];
	}

	internal static bool TryGetValue(string fullName, out T value)
	{
		return dict.TryGetValue(fullName, out value);
	}

	internal static bool TryGetValue(string modName, string contentName, out T value)
	{
		if (!tieredDict.TryGetValue(modName, out var subDictionary))
		{
			value = default(T);
			return false;
		}
		return subDictionary.TryGetValue(contentName, out value);
	}
}
