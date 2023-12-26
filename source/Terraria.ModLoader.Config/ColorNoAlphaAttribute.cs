using System;

namespace Terraria.ModLoader.Config;

/// <summary>
/// Add this attribute to a Color item and Alpha will not be presented in the UI and will remain as 255 unless manually edited.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class ColorNoAlphaAttribute : Attribute
{
}
