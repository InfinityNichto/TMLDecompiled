using System;
using System.ComponentModel;

namespace Terraria.ModLoader.Config;

/// <summary>
/// Defines the default key value to be added when using the ModConfig UI to add elements to a Dictionary. Works the same as System.ComponentModel.DefaultValueAttribute, but can't inherit from it because it would break when deserializing any data structure annotated with it. This attribute compliments DefaultListValueAttribute when used annotating a Dictionary.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class DefaultDictionaryKeyValueAttribute : Attribute
{
	private object value;

	public virtual object Value => value;

	public DefaultDictionaryKeyValueAttribute(Type type, string value)
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

	public DefaultDictionaryKeyValueAttribute(char value)
	{
		this.value = value;
	}

	public DefaultDictionaryKeyValueAttribute(byte value)
	{
		this.value = value;
	}

	public DefaultDictionaryKeyValueAttribute(short value)
	{
		this.value = value;
	}

	public DefaultDictionaryKeyValueAttribute(int value)
	{
		this.value = value;
	}

	public DefaultDictionaryKeyValueAttribute(long value)
	{
		this.value = value;
	}

	public DefaultDictionaryKeyValueAttribute(float value)
	{
		this.value = value;
	}

	public DefaultDictionaryKeyValueAttribute(double value)
	{
		this.value = value;
	}

	public DefaultDictionaryKeyValueAttribute(bool value)
	{
		this.value = value;
	}

	public DefaultDictionaryKeyValueAttribute(string value)
	{
		this.value = value;
	}

	public DefaultDictionaryKeyValueAttribute(object value)
	{
		this.value = value;
	}

	public override bool Equals(object obj)
	{
		if (obj == this)
		{
			return true;
		}
		if (obj is DefaultDictionaryKeyValueAttribute other)
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
