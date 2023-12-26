using System;
using Microsoft.Xna.Framework;

namespace Terraria.ModLoader.Config;

/// <summary>
/// Specifies a background color to be used for the property, field, or class in the ModConfig UI.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field)]
public class BackgroundColorAttribute : Attribute
{
	public Color Color { get; }

	public BackgroundColorAttribute(int r, int g, int b, int a = 255)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		Color = new Color(r, g, b, a);
	}
}
