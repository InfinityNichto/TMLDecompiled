using System;
using Microsoft.Xna.Framework;
using Terraria.Localization;

namespace Terraria.ModLoader;

internal struct MapEntry
{
	internal Color color;

	internal LocalizedText name;

	internal Func<string, int, int, string> getName;

	internal MapEntry(Color color, LocalizedText name = null)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		if (name == null)
		{
			name = LocalizedText.Empty;
		}
		this.color = color;
		this.name = name;
		getName = sameName;
	}

	internal MapEntry(Color color, LocalizedText name, Func<string, int, int, string> getName)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		this.color = color;
		this.name = name;
		this.getName = getName;
	}

	private static string sameName(string name, int x, int y)
	{
		return name;
	}
}
