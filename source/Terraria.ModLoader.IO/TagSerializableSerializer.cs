using System;
using System.Reflection;

namespace Terraria.ModLoader.IO;

internal class TagSerializableSerializer<T> : TagSerializer<T, TagCompound> where T : TagSerializable
{
	private Func<TagCompound, T> deserializer;

	public TagSerializableSerializer()
	{
		Type type = typeof(T);
		FieldInfo field = type.GetField("DESERIALIZER");
		if (field != null)
		{
			if (field.FieldType != typeof(Func<TagCompound, T>))
			{
				throw new ArgumentException($"Invalid deserializer field type {field.FieldType} in {type.FullName} expected {typeof(Func<TagCompound, T>)}.");
			}
			deserializer = (Func<TagCompound, T>)field.GetValue(null);
		}
	}

	public override TagCompound Serialize(T value)
	{
		TagCompound tagCompound = value.SerializeData();
		tagCompound["<type>"] = value.GetType().FullName;
		return tagCompound;
	}

	public override T Deserialize(TagCompound tag)
	{
		if (tag.ContainsKey("<type>") && tag.GetString("<type>") != Type.FullName)
		{
			Type instType = TagSerializer.GetType(tag.GetString("<type>"));
			if (instType != null && Type.IsAssignableFrom(instType) && TagSerializer.TryGetSerializer(instType, out TagSerializer instSerializer))
			{
				return (T)instSerializer.Deserialize(tag);
			}
		}
		if (deserializer == null)
		{
			throw new ArgumentException("Missing deserializer for type '" + Type.FullName + "'.");
		}
		return deserializer(tag);
	}
}
