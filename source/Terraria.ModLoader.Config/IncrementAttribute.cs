using System;

namespace Terraria.ModLoader.Config;

/// <summary>
/// Use this to set an increment for sliders. The slider will move by the amount assigned. Remember that this is just a UI suggestion and manual editing of config files can specify other values, so validate your values.
/// Defaults are: float: 0.01f - byte/int/uint: 1
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class IncrementAttribute : Attribute
{
	public object Increment { get; }

	public IncrementAttribute(int increment)
	{
		Increment = increment;
	}

	public IncrementAttribute(float increment)
	{
		Increment = increment;
	}

	public IncrementAttribute(uint increment)
	{
		Increment = increment;
	}

	public IncrementAttribute(byte increment)
	{
		Increment = increment;
	}
}
