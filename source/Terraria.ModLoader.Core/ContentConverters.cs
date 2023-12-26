using System;
using System.IO;
using Terraria.ModLoader.IO;

namespace Terraria.ModLoader.Core;

internal static class ContentConverters
{
	internal static bool Convert(ref string resourceName, FileStream src, MemoryStream dst)
	{
		if (Path.GetExtension(resourceName).ToLower() == ".png")
		{
			if (resourceName != "icon.png" && ImageIO.ToRaw(src, dst))
			{
				resourceName = Path.ChangeExtension(resourceName, "rawimg");
				return true;
			}
			src.Position = 0L;
			return false;
		}
		return false;
	}

	internal static bool Reverse(ref string resourceName, out Action<Stream, Stream> converter)
	{
		if (resourceName == "Info")
		{
			resourceName = "build.txt";
			converter = BuildProperties.InfoToBuildTxt;
			return true;
		}
		if (Path.GetExtension(resourceName).ToLower() == ".rawimg")
		{
			resourceName = Path.ChangeExtension(resourceName, "png");
			converter = ImageIO.RawToPng;
			return true;
		}
		converter = null;
		return false;
	}
}
