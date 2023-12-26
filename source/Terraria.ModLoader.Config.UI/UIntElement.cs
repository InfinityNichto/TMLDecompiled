using System;

namespace Terraria.ModLoader.Config.UI;

internal class UIntElement : PrimitiveRangeElement<uint>
{
	public override int NumberTicks => (int)((base.Max - base.Min) / base.Increment + 1);

	public override float TickIncrement => (float)base.Increment / (float)(base.Max - base.Min);

	protected override float Proportion
	{
		get
		{
			return (float)(GetValue() - base.Min) / (float)(base.Max - base.Min);
		}
		set
		{
			SetValue((uint)Math.Round((value * (float)(base.Max - base.Min) + (float)base.Min) * (1f / (float)base.Increment)) * base.Increment);
		}
	}

	public UIntElement()
	{
		base.Min = 0u;
		base.Max = 100u;
		base.Increment = 1u;
	}
}
