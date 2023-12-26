using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Ionic.Zlib;

namespace Terraria.ModLoader.IO;

public static class TagIO
{
	private abstract class PayloadHandler
	{
		public abstract Type PayloadType { get; }

		public abstract object Default();

		public abstract object Read(BinaryReader r);

		public abstract void Write(BinaryWriter w, object v);

		public abstract IList ReadList(BinaryReader r, int size);

		public abstract void WriteList(BinaryWriter w, IList list);

		public abstract object Clone(object o);

		public abstract IList CloneList(IList list);
	}

	private class PayloadHandler<T> : PayloadHandler where T : notnull
	{
		internal Func<BinaryReader, T> reader;

		internal Action<BinaryWriter, T> writer;

		public override Type PayloadType => typeof(T);

		public PayloadHandler(Func<BinaryReader, T> reader, Action<BinaryWriter, T> writer)
		{
			this.reader = reader;
			this.writer = writer;
		}

		public override object Read(BinaryReader r)
		{
			return reader(r);
		}

		public override void Write(BinaryWriter w, object v)
		{
			writer(w, (T)v);
		}

		public override IList ReadList(BinaryReader r, int size)
		{
			List<T> list = new List<T>(size);
			for (int i = 0; i < size; i++)
			{
				list.Add(reader(r));
			}
			return list;
		}

		public override void WriteList(BinaryWriter w, IList list)
		{
			foreach (T t in list)
			{
				writer(w, t);
			}
		}

		public override object Clone(object o)
		{
			return o;
		}

		public override IList CloneList(IList list)
		{
			return CloneList((IList<T>)list);
		}

		public virtual IList CloneList(IList<T> list)
		{
			return new List<T>(list);
		}

		public override object Default()
		{
			return default(T);
		}
	}

	private class ClassPayloadHandler<T> : PayloadHandler<T> where T : class
	{
		private Func<T, T> clone;

		private Func<T>? makeDefault;

		public ClassPayloadHandler(Func<BinaryReader, T> reader, Action<BinaryWriter, T> writer, Func<T, T> clone, Func<T>? makeDefault = null)
			: base(reader, writer)
		{
			this.clone = clone;
			this.makeDefault = makeDefault;
		}

		public override object Clone(object o)
		{
			return clone((T)o);
		}

		public override IList CloneList(IList<T> list)
		{
			return list.Select(clone).ToList();
		}

		public override object Default()
		{
			return makeDefault();
		}
	}

	private static readonly PayloadHandler[] PayloadHandlers = new PayloadHandler[12]
	{
		null,
		new PayloadHandler<byte>((BinaryReader r) => r.ReadByte(), delegate(BinaryWriter w, byte v)
		{
			w.Write(v);
		}),
		new PayloadHandler<short>((BinaryReader r) => r.ReadInt16(), delegate(BinaryWriter w, short v)
		{
			w.Write(v);
		}),
		new PayloadHandler<int>((BinaryReader r) => r.ReadInt32(), delegate(BinaryWriter w, int v)
		{
			w.Write(v);
		}),
		new PayloadHandler<long>((BinaryReader r) => r.ReadInt64(), delegate(BinaryWriter w, long v)
		{
			w.Write(v);
		}),
		new PayloadHandler<float>((BinaryReader r) => r.ReadSingle(), delegate(BinaryWriter w, float v)
		{
			w.Write(v);
		}),
		new PayloadHandler<double>((BinaryReader r) => r.ReadDouble(), delegate(BinaryWriter w, double v)
		{
			w.Write(v);
		}),
		new ClassPayloadHandler<byte[]>((BinaryReader r) => r.ReadBytes(r.ReadInt32()), delegate(BinaryWriter w, byte[] v)
		{
			w.Write(v.Length);
			w.Write(v);
		}, (byte[] v) => (byte[])v.Clone(), () => Array.Empty<byte>()),
		new ClassPayloadHandler<string>((BinaryReader r) => Encoding.UTF8.GetString(r.BaseStream.ReadByteSpan(r.ReadInt16())), delegate(BinaryWriter w, string v)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(v);
			w.Write((short)bytes.Length);
			w.Write(bytes);
		}, (string v) => v, () => ""),
		new ClassPayloadHandler<IList>((BinaryReader r) => GetHandler(r.ReadByte()).ReadList(r, r.ReadInt32()), delegate(BinaryWriter w, IList v)
		{
			int payloadId;
			try
			{
				payloadId = GetPayloadId(v.GetType().GetGenericArguments()[0]);
			}
			catch (IOException)
			{
				throw new IOException("Invalid NBT list type: " + v.GetType());
			}
			w.Write((byte)payloadId);
			w.Write(v.Count);
			PayloadHandlers[payloadId].WriteList(w, v);
		}, delegate(IList v)
		{
			try
			{
				return GetHandler(GetPayloadId(v.GetType().GetGenericArguments()[0])).CloneList(v);
			}
			catch (IOException)
			{
				throw new IOException("Invalid NBT list type: " + v.GetType());
			}
		}),
		new ClassPayloadHandler<TagCompound>(delegate(BinaryReader r)
		{
			TagCompound tagCompound = new TagCompound();
			object value2;
			string name;
			while ((value2 = ReadTag(r, out name)) != null)
			{
				tagCompound.Set(name, value2);
			}
			return tagCompound;
		}, delegate(BinaryWriter w, TagCompound v)
		{
			foreach (KeyValuePair<string, object> current in v)
			{
				if (current.Value != null)
				{
					WriteTag(current.Key, current.Value, w);
				}
			}
			w.Write((byte)0);
		}, (TagCompound v) => (TagCompound)v.Clone(), () => new TagCompound()),
		new ClassPayloadHandler<int[]>(delegate(BinaryReader r)
		{
			int[] array = new int[r.ReadInt32()];
			for (int k = 0; k < array.Length; k++)
			{
				array[k] = r.ReadInt32();
			}
			return array;
		}, delegate(BinaryWriter w, int[] v)
		{
			w.Write(v.Length);
			foreach (int value in v)
			{
				w.Write(value);
			}
		}, (int[] v) => (int[])v.Clone(), () => Array.Empty<int>())
	};

	private static readonly Dictionary<Type, int> PayloadIDs = Enumerable.Range(1, PayloadHandlers.Length - 1).ToDictionary((int i) => PayloadHandlers[i].PayloadType);

	private static readonly PayloadHandler<string> StringHandler = (PayloadHandler<string>)PayloadHandlers[8];

	private static PayloadHandler GetHandler(int id)
	{
		if (id < 1 || id >= PayloadHandlers.Length)
		{
			throw new IOException("Invalid NBT payload id: " + id);
		}
		return PayloadHandlers[id];
	}

	private static int GetPayloadId(Type t)
	{
		if (PayloadIDs.TryGetValue(t, out var id))
		{
			return id;
		}
		if (typeof(IList).IsAssignableFrom(t))
		{
			return 9;
		}
		throw new IOException($"Invalid NBT payload type '{t}'");
	}

	public static object Serialize(object value)
	{
		ArgumentNullException.ThrowIfNull(value, "value");
		if (value is string || value is int || value is TagCompound || value is List<TagCompound>)
		{
			return value;
		}
		Type type = value.GetType();
		if (TagSerializer.TryGetSerializer(type, out TagSerializer serializer))
		{
			return serializer.Serialize(value);
		}
		if (GetPayloadId(type) != 9)
		{
			return value;
		}
		IList list = (IList)value;
		Type elemType = type.GetElementType() ?? type.GetGenericArguments()[0];
		if (TagSerializer.TryGetSerializer(elemType, out serializer))
		{
			return serializer.SerializeList(list);
		}
		if (GetPayloadId(elemType) != 9)
		{
			return list;
		}
		List<IList> serializedList = new List<IList>(list.Count);
		foreach (object elem in list)
		{
			serializedList.Add((IList)Serialize(elem));
		}
		return serializedList;
	}

	public static T Deserialize<T>(object? tag)
	{
		if (tag is T)
		{
			return (T)tag;
		}
		return (T)Deserialize(typeof(T), tag);
	}

	public static object Deserialize(Type type, object? tag)
	{
		ArgumentNullException.ThrowIfNull(type, "type");
		if (type.IsInstanceOfType(tag))
		{
			return tag;
		}
		if (TagSerializer.TryGetSerializer(type, out TagSerializer serializer))
		{
			if (tag == null)
			{
				tag = Deserialize(serializer.TagType, null);
			}
			return serializer.Deserialize(tag);
		}
		if (tag == null && !type.IsArray)
		{
			if (type.GetGenericArguments().Length == 0)
			{
				return GetHandler(GetPayloadId(type)).Default();
			}
			if (type.GetGenericTypeDefinition() == typeof(Nullable<>))
			{
				return Activator.CreateInstance(type);
			}
		}
		if (tag == null || tag is IList || type.IsArray)
		{
			if (type.IsArray)
			{
				Type elemType = type.GetElementType();
				if (tag == null)
				{
					return Array.CreateInstance(elemType, 0);
				}
				IList serializedList = (IList)tag;
				if (TagSerializer.TryGetSerializer(elemType, out serializer))
				{
					IList array = Array.CreateInstance(elemType, serializedList.Count);
					for (int i = 0; i < serializedList.Count; i++)
					{
						array[i] = serializer.Deserialize(serializedList[i]);
					}
					return array;
				}
				IList deserializedArray = Array.CreateInstance(elemType, serializedList.Count);
				for (int j = 0; j < serializedList.Count; j++)
				{
					deserializedArray[j] = Deserialize(elemType, serializedList[j]);
				}
				return deserializedArray;
			}
			if (type.GetGenericArguments().Length == 1)
			{
				Type elemType2 = type.GetGenericArguments()[0];
				Type newListType = typeof(List<>).MakeGenericType(elemType2);
				if (type.IsAssignableFrom(newListType))
				{
					if (tag == null)
					{
						return Activator.CreateInstance(newListType);
					}
					if (TagSerializer.TryGetSerializer(elemType2, out serializer))
					{
						return serializer.DeserializeList((IList)tag);
					}
					IList oldList = (IList)tag;
					IList newList = (IList)Activator.CreateInstance(newListType, oldList.Count);
					{
						foreach (object elem in oldList)
						{
							newList.Add(Deserialize(elemType2, elem));
						}
						return newList;
					}
				}
			}
		}
		if (tag == null)
		{
			throw new IOException($"Invalid NBT payload type '{type}'");
		}
		throw new InvalidCastException($"Unable to cast object of type '{tag.GetType()}' to type '{type}'");
	}

	public static T Clone<T>(T o) where T : notnull
	{
		return (T)GetHandler(GetPayloadId(o.GetType())).Clone(o);
	}

	public static object? ReadTag(BinaryReader r, out string? name)
	{
		int id = r.ReadByte();
		if (id == 0)
		{
			name = null;
			return null;
		}
		name = StringHandler.reader(r);
		return ReadTagImpl(id, r);
	}

	public static object? ReadTagImpl(int id, BinaryReader r)
	{
		return PayloadHandlers[id].Read(r);
	}

	public static void WriteTag(string name, object tag, BinaryWriter w)
	{
		int id = GetPayloadId(tag.GetType());
		w.Write((byte)id);
		StringHandler.writer(w, name);
		PayloadHandlers[id].Write(w, tag);
	}

	public static TagCompound FromFile(string path, bool compressed = true)
	{
		try
		{
			using Stream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
			return FromStream(fs, compressed);
		}
		catch (IOException e)
		{
			throw new IOException("Failed to read NBT file: " + path, e);
		}
	}

	public static TagCompound FromStream(Stream stream, bool compressed = true)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		if (compressed)
		{
			stream = (Stream)new GZipStream(stream, (CompressionMode)1);
			MemoryStream ms = new MemoryStream(1048576);
			stream.CopyTo(ms);
			ms.Position = 0L;
			stream = ms;
		}
		return Read(new BigEndianReader(stream));
	}

	public static TagCompound Read(BinaryReader reader)
	{
		string name;
		return (ReadTag(reader, out name) as TagCompound) ?? throw new IOException("Root tag not a TagCompound");
	}

	public static void ToFile(TagCompound root, string path, bool compress = true)
	{
		try
		{
			using Stream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
			ToStream(root, fs, compress);
		}
		catch (IOException e)
		{
			throw new IOException("Failed to read NBT file: " + path, e);
		}
	}

	public static void ToStream(TagCompound root, Stream stream, bool compress = true)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Expected O, but got Unknown
		if (compress)
		{
			stream = (Stream)new GZipStream(stream, (CompressionMode)0, true);
		}
		Write(root, new BigEndianWriter(stream));
		if (compress)
		{
			stream.Close();
		}
	}

	public static void Write(TagCompound root, BinaryWriter writer)
	{
		WriteTag("", root, writer);
	}
}
