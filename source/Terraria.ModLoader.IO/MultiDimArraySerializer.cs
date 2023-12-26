using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Terraria.ModLoader.IO;

[Autoload(false)]
public class MultiDimArraySerializer : TagSerializer<Array, TagCompound>
{
	public delegate object Converter(object element);

	private const string Ranks = "ranks";

	private const string List = "list";

	public Type ArrayType { get; }

	public Type ElementType { get; }

	public int ArrayRank { get; }

	public MultiDimArraySerializer(Type arrayType)
	{
		ArgumentNullException.ThrowIfNull(arrayType, "arrayType");
		if (!arrayType.IsArray)
		{
			throw new ArgumentException("Must be an array type", "arrayType");
		}
		ArrayType = arrayType;
		ElementType = arrayType.GetElementType();
		ArrayRank = arrayType.GetArrayRank();
	}

	public override TagCompound Serialize(Array array)
	{
		ArgumentNullException.ThrowIfNull(array, "array");
		if (array.Length == 0)
		{
			return ToTagCompound(array);
		}
		Type serializedType = TagIO.Serialize(array.GetValue(new int[array.Rank])).GetType();
		return ToTagCompound(array, serializedType, TagIO.Serialize);
	}

	public override Array Deserialize(TagCompound tag)
	{
		ArgumentNullException.ThrowIfNull(tag, "tag");
		return FromTagCompound(tag, ArrayType, (object e) => TagIO.Deserialize(ElementType, e));
	}

	public override IList SerializeList(IList list)
	{
		ArgumentNullException.ThrowIfNull(list, "list");
		List<TagCompound> serializedList = new List<TagCompound>(list.Count);
		foreach (Array array in list)
		{
			serializedList.Add(Serialize(array));
		}
		return serializedList;
	}

	public override IList DeserializeList(IList list)
	{
		ArgumentNullException.ThrowIfNull(list, "list");
		IList<TagCompound> listT = (IList<TagCompound>)list;
		IList deserializedList = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(ArrayType), listT.Count);
		foreach (TagCompound tagCompound in listT)
		{
			deserializedList.Add(Deserialize(tagCompound));
		}
		return deserializedList;
	}

	public static TagCompound ToTagCompound(Array array, Type? elementType = null, Converter? converter = null)
	{
		ArgumentNullException.ThrowIfNull(array, "array");
		int[] ranks = new int[array.Rank];
		for (int i = 0; i < ranks.Length; i++)
		{
			ranks[i] = array.GetLength(i);
		}
		return new TagCompound
		{
			["ranks"] = ranks,
			["list"] = ToList(array, elementType, converter)
		};
	}

	public static IList ToList(Array array, Type? elementType = null, Converter? converter = null)
	{
		ArgumentNullException.ThrowIfNull(array, "array");
		Type arrayType = array.GetType();
		IList list = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(elementType ?? arrayType.GetElementType()), array.Length);
		foreach (object o in array)
		{
			list.Add((converter != null) ? converter(o) : o);
		}
		return list;
	}

	public static Array FromTagCompound(TagCompound tag, Type arrayType, Converter? converter = null)
	{
		ArgumentNullException.ThrowIfNull(tag, "tag");
		ArgumentNullException.ThrowIfNull(arrayType, "arrayType");
		if (!arrayType.IsArray)
		{
			throw new ArgumentException("Must be an array type", "arrayType");
		}
		Type elementType = arrayType.GetElementType();
		if (!tag.TryGet<int[]>("ranks", out var ranks))
		{
			return Array.CreateInstance(elementType, new int[arrayType.GetArrayRank()]);
		}
		return FromList(tag.Get<List<object>>("list"), ranks, elementType, converter);
	}

	public static Array FromList(IList list, int[] arrayRanks, Type? elementType = null, Converter? converter = null)
	{
		ArgumentNullException.ThrowIfNull(list, "list");
		ArgumentNullException.ThrowIfNull(arrayRanks, "arrayRanks");
		if (arrayRanks.Length == 0)
		{
			throw new ArgumentException("Array rank must be greater than 0");
		}
		if (list.Count != arrayRanks.Aggregate(1, (int current, int length) => current * length))
		{
			throw new ArgumentException("List length does not match array length");
		}
		Type type = list.GetType();
		if ((object)elementType == null)
		{
			elementType = type.GetElementType();
		}
		if ((object)elementType == null)
		{
			Type[] genericArguments = type.GetGenericArguments();
			if (genericArguments == null || genericArguments.Length != 1)
			{
				throw new ArgumentException("IList type must have exactly one generic argument");
			}
			elementType = genericArguments[0];
		}
		Array array = Array.CreateInstance(elementType, arrayRanks);
		int[] indices = new int[arrayRanks.Length];
		foreach (object item in list)
		{
			object value = item;
			int r = indices.Length - 1;
			while (true)
			{
				if (r >= 0 && indices[r] >= arrayRanks[r])
				{
					if (r == 0)
					{
						break;
					}
					indices[r] = 0;
					indices[r - 1]++;
					r--;
					continue;
				}
				if (converter != null)
				{
					value = converter(value);
				}
				array.SetValue(value, indices);
				indices[^1]++;
				goto IL_0115;
			}
			break;
			IL_0115:;
		}
		return array;
	}
}
