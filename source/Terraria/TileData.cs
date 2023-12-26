using System;
using System.Runtime.InteropServices;
using System.Runtime.Loader;

namespace Terraria;

internal static class TileData
{
	internal static Action OnClearEverything;

	internal static Action<uint> OnSetLength;

	internal static Action<uint> OnClearSingle;

	internal static Action<uint, uint> OnCopySingle;

	internal static uint Length { get; private set; }

	internal static void SetLength(uint len)
	{
		Length = len;
		OnSetLength?.Invoke(len);
	}

	internal static void ClearEverything()
	{
		OnClearEverything?.Invoke();
	}

	internal static void ClearSingle(uint index)
	{
		OnClearSingle?.Invoke(index);
	}

	internal static void CopySingle(uint sourceIndex, uint destinationIndex)
	{
		OnCopySingle?.Invoke(sourceIndex, destinationIndex);
	}
}
internal static class TileData<T> where T : unmanaged, ITileData
{
	private static GCHandle handle;

	public static T[] data { get; private set; }

	public unsafe static T* ptr { get; private set; }

	static TileData()
	{
		TileData.OnSetLength = (Action<uint>)Delegate.Combine(TileData.OnSetLength, new Action<uint>(SetLength));
		TileData.OnClearEverything = (Action)Delegate.Combine(TileData.OnClearEverything, new Action(ClearEverything));
		TileData.OnCopySingle = (Action<uint, uint>)Delegate.Combine(TileData.OnCopySingle, new Action<uint, uint>(CopySingle));
		TileData.OnClearSingle = (Action<uint>)Delegate.Combine(TileData.OnClearSingle, new Action<uint>(ClearSingle));
		AssemblyLoadContext.GetLoadContext(typeof(T).Assembly).Unloading += delegate
		{
			Unload();
		};
		SetLength(TileData.Length);
	}

	private static void Unload()
	{
		TileData.OnSetLength = (Action<uint>)Delegate.Remove(TileData.OnSetLength, new Action<uint>(SetLength));
		TileData.OnClearEverything = (Action)Delegate.Remove(TileData.OnClearEverything, new Action(ClearEverything));
		TileData.OnCopySingle = (Action<uint, uint>)Delegate.Remove(TileData.OnCopySingle, new Action<uint, uint>(CopySingle));
		TileData.OnClearSingle = (Action<uint>)Delegate.Remove(TileData.OnClearSingle, new Action<uint>(ClearSingle));
		if (data != null)
		{
			handle.Free();
			data = null;
		}
	}

	public static void ClearEverything()
	{
		Array.Clear(data);
	}

	private unsafe static void SetLength(uint len)
	{
		if (data != null)
		{
			handle.Free();
		}
		data = new T[len];
		handle = GCHandle.Alloc(data, GCHandleType.Pinned);
		ptr = (T*)handle.AddrOfPinnedObject().ToPointer();
	}

	private unsafe static void ClearSingle(uint index)
	{
		ptr[index] = default(T);
	}

	private unsafe static void CopySingle(uint sourceIndex, uint destinationIndex)
	{
		ptr[destinationIndex] = ptr[sourceIndex];
	}
}
