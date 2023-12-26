using System;
using Terraria.Localization;
using Terraria.Map;

namespace Terraria.ModLoader;

public class MapLegend
{
	private LocalizedText[] legend;

	public int Length => legend.Length;

	public LocalizedText this[int i]
	{
		get
		{
			return legend[i];
		}
		set
		{
			legend[i] = value;
		}
	}

	public MapLegend(int size)
	{
		legend = new LocalizedText[size];
	}

	internal void Resize(int newSize)
	{
		Array.Resize(ref legend, newSize);
	}

	public LocalizedText FromType(int type)
	{
		return this[MapHelper.TileToLookup(type, 0)];
	}

	public string FromTile(MapTile mapTile, int x, int y)
	{
		string name = legend[mapTile.Type].Value;
		if (MapLoader.nameFuncs.ContainsKey(mapTile.Type))
		{
			name = MapLoader.nameFuncs[mapTile.Type](name, x, y);
		}
		return name;
	}
}
