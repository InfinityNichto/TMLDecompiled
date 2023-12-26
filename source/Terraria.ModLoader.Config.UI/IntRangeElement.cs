using System;

namespace Terraria.ModLoader.Config.UI;

internal class IntRangeElement : PrimitiveRangeElement<int>
{
	public override int NumberTicks => (base.Max - base.Min) / base.Increment + 1;

	public override float TickIncrement => (float)base.Increment / (float)(base.Max - base.Min);

	protected override float Proportion
	{
		get
		{
			return (float)(GetValue() - base.Min) / (float)(base.Max - base.Min);
		}
		set
		{
			SetValue((int)Math.Round((value * (float)(base.Max - base.Min) + (float)base.Min) * (1f / (float)base.Increment)) * base.Increment);
		}
	}

	public IntRangeElement()
	{
		base.Min = 0;
		base.Max = 100;
		base.Increment = 1;
	}
}
