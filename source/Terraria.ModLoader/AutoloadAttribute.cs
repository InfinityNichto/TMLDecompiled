using System;
using System.Linq;
using Terraria.ModLoader.Core;

namespace Terraria.ModLoader;

/// <summary>
/// Allows for types to be autoloaded and unloaded.
/// True to always autoload, false to never autoload, null to use mod default.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = true, Inherited = true)]
public sealed class AutoloadAttribute : Attribute
{
	private static readonly AutoloadAttribute Default = new AutoloadAttribute();

	public readonly bool Value;

	public ModSide Side { get; set; }

	public bool NeedsAutoloading
	{
		get
		{
			if (Value)
			{
				return ModOrganizer.LoadSide(Side);
			}
			return false;
		}
	}

	public AutoloadAttribute(bool value = true)
	{
		Value = value;
	}

	public static AutoloadAttribute GetValue(Type type)
	{
		return ((AutoloadAttribute)type.GetCustomAttributes(typeof(AutoloadAttribute), inherit: true).FirstOrDefault()) ?? Default;
	}
}
