using System;
using System.Buffers.Binary;
using System.IO;

namespace Terraria.ModLoader.IO;

public class BigEndianWriter : BinaryWriter
{
	public BigEndianWriter(Stream output)
		: base(output)
	{
	}

	public override void Write(short value)
	{
		Span<byte> buf = stackalloc byte[2];
		BinaryPrimitives.WriteInt16BigEndian(buf, value);
		OutStream.Write(buf);
	}

	public override void Write(ushort value)
	{
		Span<byte> buf = stackalloc byte[2];
		BinaryPrimitives.WriteUInt16BigEndian(buf, value);
		OutStream.Write(buf);
	}

	public override void Write(int value)
	{
		Span<byte> buf = stackalloc byte[4];
		BinaryPrimitives.WriteInt32BigEndian(buf, value);
		OutStream.Write(buf);
	}

	public override void Write(uint value)
	{
		Span<byte> buf = stackalloc byte[4];
		BinaryPrimitives.WriteUInt32BigEndian(buf, value);
		OutStream.Write(buf);
	}

	public override void Write(long value)
	{
		Span<byte> buf = stackalloc byte[8];
		BinaryPrimitives.WriteInt64BigEndian(buf, value);
		OutStream.Write(buf);
	}

	public override void Write(ulong value)
	{
		Span<byte> buf = stackalloc byte[8];
		BinaryPrimitives.WriteUInt64BigEndian(buf, value);
		OutStream.Write(buf);
	}

	public override void Write(float value)
	{
		Span<byte> buf = stackalloc byte[4];
		BinaryPrimitives.WriteSingleBigEndian(buf, value);
		OutStream.Write(buf);
	}

	public override void Write(double value)
	{
		Span<byte> buf = stackalloc byte[8];
		BinaryPrimitives.WriteDoubleBigEndian(buf, value);
		OutStream.Write(buf);
	}
}
