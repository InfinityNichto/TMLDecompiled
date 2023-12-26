using System;

namespace Terraria.ModLoader.Config;

/// <summary>
/// Add this attribute to a Color item and the UI will present a Hue, Saturation, and Lightness sliders rather than Red, Green, and Blue sliders. Pass in false to skip Saturation and Lightness.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class ColorHSLSliderAttribute : Attribute
{
	public bool ShowSaturationAndLightness { get; }

	public ColorHSLSliderAttribute(bool showSaturationAndLightness = true)
	{
		ShowSaturationAndLightness = showSaturationAndLightness;
	}
}
