using System;
using System.IO;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria.ModLoader.IO;

public class ImageIO
{
	public const int VERSION = 1;

	public unsafe static bool ToRaw(Stream src, Stream dst)
	{
		int width = default(int);
		int height = default(int);
		int len = default(int);
		IntPtr img = FNA3D.ReadImageStream(src, ref width, ref height, ref len, -1, -1, false);
		if (img == IntPtr.Zero)
		{
			return false;
		}
		byte* colors = (byte*)img.ToPointer();
		using BinaryWriter w = new BinaryWriter(dst);
		w.Write(1);
		w.Write(width);
		w.Write(height);
		for (int i = 0; i < len; i += 4)
		{
			if (colors[i + 3] == 0)
			{
				w.Write(0);
				continue;
			}
			w.Write(colors[i]);
			w.Write(colors[i + 1]);
			w.Write(colors[i + 2]);
			w.Write(colors[i + 3]);
		}
		FNA3D.FNA3D_Image_Free(img);
		return true;
	}

	public unsafe static void RawToPng(Stream src, Stream dst)
	{
		int width;
		int height;
		fixed (byte* pixels = ReadRaw(src, out width, out height))
		{
			FNA3D.WritePNGStream(dst, width, height, width, height, (IntPtr)pixels);
		}
	}

	public static byte[] ReadRaw(Stream stream, out int width, out int height)
	{
		using BinaryReader r = new BinaryReader(stream);
		int v = r.ReadInt32();
		if (v != 1)
		{
			throw new Exception("Unknown RawImg Format Version: " + v);
		}
		width = r.ReadInt32();
		height = r.ReadInt32();
		return r.ReadBytes(width * height * 4);
	}
}
