using System;
using System.IO;

namespace Terraria.ModLoader.IO;

public static class BinaryIO
{
	[Obsolete("Use Write7BitEncodedInt", true)]
	public static void WriteVarInt(this BinaryWriter writer, int value)
	{
		writer.Write7BitEncodedInt(value);
	}

	[Obsolete("Use Read7BitEncodedInt", true)]
	public static int ReadVarInt(this BinaryReader reader)
	{
		return reader.Read7BitEncodedInt();
	}

	public static void SafeWrite(this BinaryWriter writer, Action<BinaryWriter> write)
	{
		MemoryStream ms = new MemoryStream();
		write(new BinaryWriter(ms));
		writer.Write7BitEncodedInt((int)ms.Length);
		ms.Position = 0L;
		ms.CopyTo(writer.BaseStream);
	}

	public static void SafeRead(this BinaryReader reader, Action<BinaryReader> read)
	{
		int length = reader.Read7BitEncodedInt();
		MemoryStream ms = reader.ReadBytes(length).ToMemoryStream();
		read(new BinaryReader(ms));
		if (ms.Position != length)
		{
			throw new IOException("Read underflow " + ms.Position + " of " + length + " bytes");
		}
	}

	public static void ReadBytes(this Stream stream, byte[] buf)
	{
		int pos = 0;
		int r;
		while ((r = stream.Read(buf, pos, buf.Length - pos)) > 0)
		{
			pos += r;
		}
		if (pos != buf.Length)
		{
			throw new IOException($"Stream did not contain enough bytes ({pos}) < ({buf.Length})");
		}
	}

	public static byte[] ReadBytes(this Stream stream, int len)
	{
		return stream.ReadBytes((long)len);
	}

	public static byte[] ReadBytes(this Stream stream, long len)
	{
		byte[] buf = new byte[len];
		stream.ReadBytes(buf);
		return buf;
	}

	public static MemoryStream ToMemoryStream(this byte[] bytes, bool writeable = false)
	{
		return new MemoryStream(bytes, 0, bytes.Length, writeable, publiclyVisible: true);
	}

	public static ReadOnlySpan<byte> ReadByteSpan(this Stream stream, int len)
	{
		if (stream is MemoryStream ms && ms.TryGetBuffer(out var buf))
		{
			Span<byte> span = buf.AsSpan().Slice((int)ms.Position, len);
			ms.Seek(len, SeekOrigin.Current);
			return span;
		}
		return stream.ReadBytes(len);
	}
}
