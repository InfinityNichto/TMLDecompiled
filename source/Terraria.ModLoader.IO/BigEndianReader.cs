using System.Buffers.Binary;
using System.IO;

namespace Terraria.ModLoader.IO;

public class BigEndianReader : BinaryReader
{
	public BigEndianReader(Stream input)
		: base(input)
	{
	}

	public override short ReadInt16()
	{
		return BinaryPrimitives.ReadInt16BigEndian(BaseStream.ReadByteSpan(2));
	}

	public override ushort ReadUInt16()
	{
		return BinaryPrimitives.ReadUInt16BigEndian(BaseStream.ReadByteSpan(2));
	}

	public override int ReadInt32()
	{
		return BinaryPrimitives.ReadInt32BigEndian(BaseStream.ReadByteSpan(4));
	}

	public override uint ReadUInt32()
	{
		return BinaryPrimitives.ReadUInt32BigEndian(BaseStream.ReadByteSpan(4));
	}

	public override long ReadInt64()
	{
		return BinaryPrimitives.ReadInt64BigEndian(BaseStream.ReadByteSpan(8));
	}

	public override ulong ReadUInt64()
	{
		return BinaryPrimitives.ReadUInt64BigEndian(BaseStream.ReadByteSpan(8));
	}

	public override float ReadSingle()
	{
		return BinaryPrimitives.ReadSingleBigEndian(BaseStream.ReadByteSpan(4));
	}

	public override double ReadDouble()
	{
		return BinaryPrimitives.ReadDoubleBigEndian(BaseStream.ReadByteSpan(8));
	}
}
