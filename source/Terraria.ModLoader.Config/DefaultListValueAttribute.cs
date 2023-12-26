using System;
using System.ComponentModel;

namespace Terraria.ModLoader.Config;

/// <summary>
/// Defines the default value to be added when using the ModConfig UI to add elements to a Collection (List, Set, or Dictionary value). Works the same as System.ComponentModel.DefaultValueAttribute, but can't inherit from it because it would break when deserializing any data structure annotated with it.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class DefaultListValueAttribute : Attribute
{
	private object value;

	public virtual object Value => value;

	public DefaultListValueAttribute(Type type, string value)
	{
		try
		{
			this.value = TypeDescriptor.GetConverter(type).ConvertFromInvariantString(value);
		}
		catch
		{
			Logging.tML.Error((object)("Default value attribute of type " + type.FullName + " threw converting from the string '" + value + "'."));
		}
	}

	public DefaultListValueAttribute(char value)
	{
		this.value = value;
	}

	public DefaultListValueAttribute(byte value)
	{
		this.value = value;
	}

	public DefaultListValueAttribute(short value)
	{
		this.value = value;
	}

	public DefaultListValueAttribute(int value)
	{
		this.value = value;
	}

	public DefaultListValueAttribute(long value)
	{
		this.value = value;
	}

	public DefaultListValueAttribute(float value)
	{
		this.value = value;
	}

	public DefaultListValueAttribute(double value)
	{
		this.value = value;
	}

	public DefaultListValueAttribute(bool value)
	{
		this.value = value;
	}

	public DefaultListValueAttribute(string value)
	{
		this.value = value;
	}

	public DefaultListValueAttribute(object value)
	{
		this.value = value;
	}

	public override bool Equals(object obj)
	{
		if (obj == this)
		{
			return true;
		}
		if (obj is DefaultListValueAttribute other)
		{
			if (Value != null)
			{
				return Value.Equals(other.Value);
			}
			return other.Value == null;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	protected void SetValue(object value)
	{
		this.value = value;
	}
}
