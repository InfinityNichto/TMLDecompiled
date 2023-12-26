using System;
using System.IO;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria.Utilities;

public static class PlatformUtilities
{
	public unsafe static void SavePng(Stream stream, int width, int height, int imgWidth, int imgHeight, byte[] data)
	{
		if (width * height * 4 > data.Length)
		{
			throw new IndexOutOfRangeException($"Data length {data.Length} must be >= width({width})*height({height})*4");
		}
		fixed (byte* ptr = data)
		{
			FNA3D.WritePNGStream(stream, width, height, imgWidth, imgHeight, (IntPtr)ptr);
		}
	}
}
