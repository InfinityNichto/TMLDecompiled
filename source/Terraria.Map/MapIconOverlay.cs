using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace Terraria.Map;

public class MapIconOverlay
{
	private readonly List<IMapLayer> _layers = new List<IMapLayer>();

	private IReadOnlyList<IMapLayer> _readOnlyLayers;

	public MapIconOverlay()
	{
		_readOnlyLayers = _layers.AsReadOnly();
	}

	internal MapIconOverlay AddLayer(IMapLayer layer)
	{
		_layers.Add(layer);
		return this;
	}

	public void Draw(Vector2 mapPosition, Vector2 mapOffset, Rectangle? clippingRect, float mapScale, float drawScale, ref string text)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		MapOverlayDrawContext context = new MapOverlayDrawContext(mapPosition, mapOffset, clippingRect, mapScale, drawScale);
		foreach (IMapLayer layer2 in _layers)
		{
			layer2.Visible = true;
		}
		SystemLoader.PreDrawMapIconOverlay(_readOnlyLayers, context);
		foreach (IMapLayer layer in _layers)
		{
			if (layer.Visible)
			{
				layer.Draw(ref context, ref text);
			}
		}
	}
}
