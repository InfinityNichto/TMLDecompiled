using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Terraria.ModLoader.Config;

/// <summary>
/// Custom ContractResolver for facilitating reference type defaults.
/// The ShouldSerialize code enables unchanged-by-user reference type defaults to properly not serialize.
/// The ValueProvider code helps during deserialization to not
/// </summary>
internal class ReferenceDefaultsPreservingResolver : DefaultContractResolver
{
	public abstract class ValueProviderDecorator : IValueProvider
	{
		private readonly IValueProvider baseProvider;

		public ValueProviderDecorator(IValueProvider baseProvider)
		{
			this.baseProvider = baseProvider ?? throw new ArgumentNullException();
		}

		public virtual object? GetValue(object target)
		{
			return baseProvider.GetValue(target);
		}

		public virtual void SetValue(object target, object? value)
		{
			baseProvider.SetValue(target, value);
		}
	}

	private class NullToDefaultValueProvider : ValueProviderDecorator
	{
		private readonly Func<object?> defaultValueGenerator;

		public NullToDefaultValueProvider(IValueProvider baseProvider, Func<object?> defaultValueGenerator)
			: base(baseProvider)
		{
			this.defaultValueGenerator = defaultValueGenerator;
		}

		public override void SetValue(object target, object? value)
		{
			base.SetValue(target, value ?? defaultValueGenerator());
		}
	}

	protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		IList<JsonProperty> props = ((DefaultContractResolver)this).CreateProperties(type, memberSerialization);
		if (!type.IsClass)
		{
			return props;
		}
		ConstructorInfo ctor = type.GetConstructor(Type.EmptyTypes);
		if (ctor == null)
		{
			return props;
		}
		object referenceInstance = ctor.Invoke(null);
		foreach (JsonProperty prop in props.Where((JsonProperty p) => p.Readable))
		{
			if (prop.PropertyType == null || prop.PropertyType.IsValueType)
			{
				continue;
			}
			if (prop.Writable)
			{
				if (prop.PropertyType.GetConstructor(Type.EmptyTypes) != null)
				{
					Func<object> defaultValueCreator = () => prop.ValueProvider.GetValue(ctor.Invoke(null));
					prop.ValueProvider = (IValueProvider)(object)new NullToDefaultValueProvider(prop.ValueProvider, defaultValueCreator);
				}
				else if (prop.PropertyType.IsArray)
				{
					Func<object> defaultValueCreator2 = () => (prop.ValueProvider.GetValue(referenceInstance) as Array).Clone();
					prop.ValueProvider = (IValueProvider)(object)new NullToDefaultValueProvider(prop.ValueProvider, defaultValueCreator2);
				}
			}
			JsonProperty val = prop;
			if (val.ShouldSerialize == null)
			{
				Predicate<object> predicate2 = (val.ShouldSerialize = delegate(object instance)
				{
					object value = prop.ValueProvider.GetValue(instance);
					object value2 = prop.ValueProvider.GetValue(referenceInstance);
					return !ConfigManager.ObjectEquals(value, value2);
				});
			}
		}
		return props;
	}
}
