using System;
using System.Collections.Generic;
using System.Reflection;

namespace Terraria.ModLoader;

public static class LoaderManager
{
	private static readonly Dictionary<Type, ILoader> loadersByType = new Dictionary<Type, ILoader>();

	internal static void AutoLoad()
	{
		Type[] types = Assembly.GetExecutingAssembly().GetTypes();
		foreach (Type type in types)
		{
			if (typeof(ILoader).IsAssignableFrom(type) && !type.IsAbstract && type.IsClass && AutoloadAttribute.GetValue(type).NeedsAutoloading)
			{
				loadersByType.Add(type, (ILoader)Activator.CreateInstance(type, nonPublic: true));
			}
		}
	}

	public static T Get<T>()
	{
		if (!loadersByType.TryGetValue(typeof(T), out var result))
		{
			return (T)Activator.CreateInstance(typeof(T), nonPublic: true);
		}
		return (T)result;
	}

	internal static void Unload()
	{
		foreach (ILoader value in loadersByType.Values)
		{
			value.Unload();
		}
	}

	internal static void ResizeArrays()
	{
		foreach (ILoader value in loadersByType.Values)
		{
			value.ResizeArrays();
		}
	}
}
