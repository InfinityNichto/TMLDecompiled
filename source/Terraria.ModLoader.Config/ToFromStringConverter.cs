using System;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using Newtonsoft.Json;

namespace Terraria.ModLoader.Config;

/// <summary>
/// This TypeConverter facilitates converting to and from the string Type. This is necessary for Objects that are to be used as Dictionary keys, since the JSON for keys needs to be a string. Classes annotated with this TypeConverter need to implement a static FromString method that returns T.
/// </summary>
/// <typeparam name="T">The Type that implements the static FromString method that returns Type T.</typeparam>
public class ToFromStringConverter<T> : TypeConverter
{
	public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
	{
		return destinationType != typeof(string);
	}

	public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
	{
		if (sourceType == typeof(string))
		{
			return true;
		}
		return base.CanConvertFrom(context, sourceType);
	}

	public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
	{
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		if (value is string)
		{
			MethodInfo parse = typeof(T).GetMethod("FromString", new Type[1] { typeof(string) });
			if (parse != null && parse.IsStatic && parse.ReturnType == typeof(T))
			{
				return parse.Invoke(null, new object[1] { value });
			}
			throw new JsonException($"The {typeof(T).Name} type does not have a public static FromString(string) method that returns a {typeof(T).Name}.");
		}
		return base.ConvertFrom(context, culture, value);
	}
}
