using System;

namespace Terraria.ModLoader.Config.UI;

public class FloatElement : PrimitiveRangeElement<float>
{
	public override int NumberTicks => (int)((base.Max - base.Min) / base.Increment) + 1;

	public override float TickIncrement => base.Increment / (base.Max - base.Min);

	protected override float Proportion
	{
		get
		{
			return (GetValue() - base.Min) / (base.Max - base.Min);
		}
		set
		{
			SetValue((float)Math.Round((value * (base.Max - base.Min) + base.Min) * (1f / base.Increment)) * base.Increment);
		}
	}

	public FloatElement()
	{
		base.Min = 0f;
		base.Max = 1f;
		base.Increment = 0.01f;
	}
}
