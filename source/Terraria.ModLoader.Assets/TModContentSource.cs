using System;
using System.IO;
using System.Linq;
using ReLogic.Content.Sources;
using Terraria.Initializers;
using Terraria.ModLoader.Core;

namespace Terraria.ModLoader.Assets;

public class TModContentSource : ContentSource
{
	private readonly TmodFile file;

	public TModContentSource(TmodFile file)
	{
		this.file = file ?? throw new ArgumentNullException("file");
		if (!Main.dedServ)
		{
			SetAssetNames(from fileEntry in file
				select fileEntry.Name into name
				where AssetInitializer.assetReaderCollection.TryGetReader(Path.GetExtension(name), out var _)
				select name);
		}
	}

	public override Stream OpenStream(string assetName)
	{
		return file.GetStream(assetName, newFileStream: true);
	}
}
