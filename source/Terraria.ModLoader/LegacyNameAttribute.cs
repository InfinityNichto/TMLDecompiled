using System;
using System.Collections.Generic;
using System.Reflection;

namespace Terraria.ModLoader;

/// <summary>
/// Allows for types to be registered with legacy/alias names for lookup via <see cref="M:Terraria.ModLoader.ModContent.Find``1(System.String)" /> and similar methods.
/// <br />When manually loading content, use <see cref="M:Terraria.ModLoader.ModTypeLookup`1.RegisterLegacyNames(`0,System.String[])" /> instead.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
public sealed class LegacyNameAttribute : Attribute
{
	public readonly string[] Names;

	public LegacyNameAttribute(params string[] names)
	{
		Names = names ?? throw new ArgumentNullException("names");
	}

	public static IEnumerable<string> GetLegacyNamesOfType(Type type)
	{
		foreach (LegacyNameAttribute attribute in type.GetCustomAttributes<LegacyNameAttribute>(inherit: false))
		{
			string[] names = attribute.Names;
			for (int i = 0; i < names.Length; i++)
			{
				yield return names[i];
			}
		}
	}
}
