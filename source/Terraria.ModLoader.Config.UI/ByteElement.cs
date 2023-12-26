using System;

namespace Terraria.ModLoader.Config.UI;

internal class ByteElement : PrimitiveRangeElement<byte>
{
	public override int NumberTicks => (base.Max - base.Min) / base.Increment + 1;

	public override float TickIncrement => (float)(int)base.Increment / (float)(base.Max - base.Min);

	protected override float Proportion
	{
		get
		{
			return (float)(GetValue() - base.Min) / (float)(base.Max - base.Min);
		}
		set
		{
			SetValue(Convert.ToByte((int)Math.Round((value * (float)(base.Max - base.Min) + (float)(int)base.Min) * (1f / (float)(int)base.Increment)) * base.Increment));
		}
	}

	public ByteElement()
	{
		base.Min = 0;
		base.Max = byte.MaxValue;
		base.Increment = 1;
	}
}
