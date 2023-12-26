using System;
using System.Collections.Generic;
using System.Linq;
using Terraria.DataStructures;

namespace Terraria.ModLoader;

public static class PlayerDrawLayerLoader
{
	private static List<PlayerDrawLayer> _layers = new List<PlayerDrawLayer>(PlayerDrawLayers.VanillaLayers);

	private static PlayerDrawLayer[] _drawOrder;

	public static IReadOnlyList<PlayerDrawLayer> Layers => _layers;

	public static IReadOnlyList<PlayerDrawLayer> DrawOrder => _drawOrder;

	internal static void Add(PlayerDrawLayer layer)
	{
		_layers.Add(layer);
	}

	internal static void Unload()
	{
		_layers = new List<PlayerDrawLayer>(PlayerDrawLayers.VanillaLayers);
		foreach (PlayerDrawLayer layer in _layers)
		{
			layer.ClearChildren();
		}
	}

	internal static void ResizeArrays()
	{
		Dictionary<PlayerDrawLayer, PlayerDrawLayer.Position> positions = Layers.ToDictionary((PlayerDrawLayer l) => l, (PlayerDrawLayer l) => l.GetDefaultPosition());
		PlayerLoader.ModifyDrawLayerOrdering(positions);
		KeyValuePair<PlayerDrawLayer, PlayerDrawLayer.Position>[] array = positions.ToArray();
		for (int j = 0; j < array.Length; j++)
		{
			KeyValuePair<PlayerDrawLayer, PlayerDrawLayer.Position> kv = array[j];
			PlayerDrawLayer layer = kv.Key;
			PlayerDrawLayer.Position value = kv.Value;
			if (value is PlayerDrawLayer.Between)
			{
				continue;
			}
			if (!(value is PlayerDrawLayer.BeforeParent b))
			{
				if (!(value is PlayerDrawLayer.AfterParent a))
				{
					if (!(value is PlayerDrawLayer.Multiple i))
					{
						throw new ArgumentException($"PlayerDrawLayer {layer} has unknown Position type {kv.Value}");
					}
					int slot = 0;
					foreach (var (pos, cond) in i.Positions)
					{
						positions.Add(new PlayerDrawLayerSlot(layer, cond, slot++), pos);
					}
				}
				else
				{
					a.Parent.AddChildAfter(layer);
				}
			}
			else
			{
				b.Parent.AddChildBefore(layer);
			}
			positions.Remove(kv.Key);
		}
		_drawOrder = new TopoSort<PlayerDrawLayer>(positions.Keys, (PlayerDrawLayer l) => new PlayerDrawLayer[1] { ((PlayerDrawLayer.Between)positions[l]).Layer1 }.Where((PlayerDrawLayer l) => l != null), (PlayerDrawLayer l) => new PlayerDrawLayer[1] { ((PlayerDrawLayer.Between)positions[l]).Layer2 }.Where((PlayerDrawLayer l) => l != null)).Sort().ToArray();
	}

	/// <summary>
	/// Note, not threadsafe
	/// </summary>
	public static PlayerDrawLayer[] GetDrawLayers(PlayerDrawSet drawInfo)
	{
		PlayerDrawLayer[] drawOrder = _drawOrder;
		for (int i = 0; i < drawOrder.Length; i++)
		{
			drawOrder[i].ResetVisibility(drawInfo);
		}
		PlayerLoader.HideDrawLayers(drawInfo);
		return _drawOrder;
	}
}
