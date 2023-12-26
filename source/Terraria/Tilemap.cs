using System;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;

namespace Terraria;

public readonly struct Tilemap
{
	public readonly ushort Width;

	public readonly ushort Height;

	public Tile this[int x, int y]
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		get
		{
			if ((uint)x >= (uint)Width || (uint)y >= (uint)Height)
			{
				throw new IndexOutOfRangeException();
			}
			return new Tile((uint)(y + x * Height));
		}
		internal set
		{
			throw new InvalidOperationException("Cannot set Tilemap tiles. Only used to init null tiles in Vanilla (which don't exist anymore)");
		}
	}

	public Tile this[Point pos] => this[pos.X, pos.Y];

	internal Tilemap(ushort width, ushort height)
	{
		Width = width;
		Height = height;
		TileData.SetLength((uint)(width * height));
	}

	public void ClearEverything()
	{
		TileData.ClearEverything();
	}

	public T[] GetData<T>() where T : unmanaged, ITileData
	{
		return TileData<T>.data;
	}
}
