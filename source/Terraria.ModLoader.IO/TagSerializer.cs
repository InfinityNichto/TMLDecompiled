using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Terraria.ModLoader.IO;

public abstract class TagSerializer : ModType
{
	private static IDictionary<Type, TagSerializer> serializers;

	private static IDictionary<string, Type> typeNameCache;

	public abstract Type Type { get; }

	public abstract Type TagType { get; }

	public abstract object Serialize(object value);

	public abstract object Deserialize(object tag);

	public abstract IList SerializeList(IList value);

	public abstract IList DeserializeList(IList value);

	static TagSerializer()
	{
		serializers = new Dictionary<Type, TagSerializer>();
		typeNameCache = new Dictionary<string, Type>();
		Reload();
	}

	internal static void Reload()
	{
		serializers.Clear();
		typeNameCache.Clear();
	}

	public static bool TryGetSerializer(Type type, [NotNullWhen(true)] out TagSerializer? serializer)
	{
		if (serializers.TryGetValue(type, out serializer))
		{
			return true;
		}
		if (type.IsArray && type.GetArrayRank() > 1)
		{
			serializers[type] = (serializer = new MultiDimArraySerializer(type));
			return true;
		}
		if (typeof(TagSerializable).IsAssignableFrom(type))
		{
			Type sType = typeof(TagSerializableSerializer<>).MakeGenericType(type);
			serializers[type] = (serializer = (TagSerializer)Activator.CreateInstance(sType));
			return true;
		}
		return false;
	}

	internal static void AddSerializer(TagSerializer serializer)
	{
		serializers.Add(serializer.Type, serializer);
	}

	public static Type? GetType(string name)
	{
		if (typeNameCache.TryGetValue(name, out Type type))
		{
			return type;
		}
		type = System.Type.GetType(name);
		if (type != null)
		{
			return typeNameCache[name] = type;
		}
		Mod[] mods = ModLoader.Mods;
		for (int i = 0; i < mods.Length; i++)
		{
			type = mods[i].Code?.GetType(name);
			if (type != null)
			{
				return typeNameCache[name] = type;
			}
		}
		return null;
	}

	protected sealed override void Register()
	{
		AddSerializer(this);
	}

	public sealed override void SetupContent()
	{
		SetStaticDefaults();
	}
}
public abstract class TagSerializer<T, S> : TagSerializer where T : notnull where S : notnull
{
	public override Type Type => typeof(T);

	public override Type TagType => typeof(S);

	public abstract S Serialize(T value);

	public abstract T Deserialize(S tag);

	public override object Serialize(object value)
	{
		return Serialize((T)value);
	}

	public override object Deserialize(object tag)
	{
		return Deserialize((S)tag);
	}

	public override IList SerializeList(IList value)
	{
		return ((IList<T>)value).Select(Serialize).ToList();
	}

	public override IList DeserializeList(IList value)
	{
		return ((IList<S>)value).Select(Deserialize).ToList();
	}
}
