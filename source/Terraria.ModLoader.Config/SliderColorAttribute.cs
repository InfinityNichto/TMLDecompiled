using System;
using Microsoft.Xna.Framework;

namespace Terraria.ModLoader.Config;

/// <summary>
/// Specifies a slider color for ModConfig elements that use a slider. The default color is white.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field)]
public class SliderColorAttribute : Attribute
{
	public Color Color { get; }

	public SliderColorAttribute(int r, int g, int b, int a = 255)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		Color = new Color(r, g, b, a);
	}
}
