using System;

namespace Terraria.ModLoader.Config;

/// <summary>
/// Specifies a range for primitive data values. Without this, default min and max are as follows: float: 0, 1 - int/uint: 0, 100 - byte: 0, 255
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class RangeAttribute : Attribute
{
	public object Min { get; }

	public object Max { get; }

	public RangeAttribute(int min, int max)
	{
		Min = min;
		Max = max;
	}

	public RangeAttribute(float min, float max)
	{
		Min = min;
		Max = max;
	}

	public RangeAttribute(uint min, uint max)
	{
		Min = min;
		Max = max;
	}

	public RangeAttribute(byte min, byte max)
	{
		Min = min;
		Max = max;
	}
}
