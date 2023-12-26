using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Terraria.ModLoader.IO;

/// <summary>
/// Tag compounds contained named values, serialisable as per the NBT spec: <see href="https://minecraft.wiki/w/NBT_format">NBT spec wiki page</see> <br />
/// All primitive data types are supported as well as byte[], int[] and Lists of other supported data types <br />
/// Lists of Lists are internally stored as IList&lt;IList&gt; <br />
/// Modification of lists stored in a TagCompound will only work if there were no type conversions involved and is not advised <br />
/// bool is supported using TagConverter, serialised as a byte. IList&lt;bool&gt; will serialise as IList&lt;byte&gt; (quite inefficient) <br />
/// Additional conversions can be added using TagConverter <br />
/// </summary>
public class TagCompound : IEnumerable<KeyValuePair<string, object>>, IEnumerable, ICloneable
{
	private Dictionary<string, object> dict = new Dictionary<string, object>();

	public object this[string key]
	{
		get
		{
			return Get<object>(key);
		}
		set
		{
			Set(key, value, replace: true);
		}
	}

	public int Count => dict.Count;

	public T Get<T>(string key)
	{
		if (!TryGet<T>(key, out var value) && value == null)
		{
			try
			{
				return TagIO.Deserialize<T>(null);
			}
			catch (Exception e)
			{
				throw new IOException($"NBT Deserialization (type={typeof(T)},entry={TagPrinter.Print(new KeyValuePair<string, object>(key, null))})", e);
			}
		}
		return value;
	}

	public bool TryGet<T>(string key, out T value)
	{
		if (!dict.TryGetValue(key, out var tag))
		{
			value = default(T);
			return false;
		}
		try
		{
			value = TagIO.Deserialize<T>(tag);
			return true;
		}
		catch (Exception e)
		{
			throw new IOException($"NBT Deserialization (type={typeof(T)},entry={TagPrinter.Print(new KeyValuePair<string, object>(key, tag))})", e);
		}
	}

	public void Set(string key, object value, bool replace = false)
	{
		if (value == null)
		{
			Remove(key);
			return;
		}
		object serialized;
		try
		{
			serialized = TagIO.Serialize(value);
		}
		catch (IOException e)
		{
			string valueInfo = "value=" + value;
			if (value.GetType().ToString() != value.ToString())
			{
				valueInfo = "type=" + value.GetType()?.ToString() + "," + valueInfo;
			}
			throw new IOException($"NBT Serialization (key={key},{valueInfo})", e);
		}
		if (replace)
		{
			dict[key] = serialized;
		}
		else
		{
			dict.Add(key, serialized);
		}
	}

	public bool ContainsKey(string key)
	{
		return dict.ContainsKey(key);
	}

	public bool Remove(string key)
	{
		return dict.Remove(key);
	}

	public byte GetByte(string key)
	{
		return Get<byte>(key);
	}

	public short GetShort(string key)
	{
		return Get<short>(key);
	}

	public int GetInt(string key)
	{
		return Get<int>(key);
	}

	public long GetLong(string key)
	{
		return Get<long>(key);
	}

	public float GetFloat(string key)
	{
		return Get<float>(key);
	}

	public double GetDouble(string key)
	{
		return Get<double>(key);
	}

	public byte[] GetByteArray(string key)
	{
		return Get<byte[]>(key);
	}

	public int[] GetIntArray(string key)
	{
		return Get<int[]>(key);
	}

	public string GetString(string key)
	{
		return Get<string>(key);
	}

	public IList<T> GetList<T>(string key)
	{
		return Get<List<T>>(key);
	}

	public TagCompound GetCompound(string key)
	{
		return Get<TagCompound>(key);
	}

	public bool GetBool(string key)
	{
		return Get<bool>(key);
	}

	public short GetAsShort(string key)
	{
		object o = Get<object>(key);
		return (o as short?) ?? (o as byte?).GetValueOrDefault();
	}

	public int GetAsInt(string key)
	{
		object o = Get<object>(key);
		return (o as int?) ?? (o as short?) ?? (o as byte?).GetValueOrDefault();
	}

	public long GetAsLong(string key)
	{
		object o = Get<object>(key);
		return (o as long?) ?? (o as int?) ?? (o as short?) ?? (o as byte?).GetValueOrDefault();
	}

	public double GetAsDouble(string key)
	{
		object o = Get<object>(key);
		return (o as double?) ?? ((double)(o as float?).GetValueOrDefault());
	}

	public object Clone()
	{
		TagCompound copy = new TagCompound();
		using IEnumerator<KeyValuePair<string, object>> enumerator = GetEnumerator();
		while (enumerator.MoveNext())
		{
			KeyValuePair<string, object> entry = enumerator.Current;
			copy.Set(entry.Key, TagIO.Clone(entry.Value));
		}
		return copy;
	}

	public override string ToString()
	{
		return TagPrinter.Print(this);
	}

	public void Add(string key, object value)
	{
		Set(key, value);
	}

	public void Add(KeyValuePair<string, object> entry)
	{
		Set(entry.Key, entry.Value);
	}

	public void Clear()
	{
		dict.Clear();
	}

	public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
	{
		return dict.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}
}
