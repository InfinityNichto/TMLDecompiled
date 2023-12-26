using System.IO;
using Microsoft.Xna.Framework;
using ReLogic.Content;
using Terraria.GameContent;

namespace Terraria.IO;

public class ResourcePackContentValidator
{
	public void ValidateResourePack(ResourcePack pack)
	{
		if ((AssetReaderCollection)((Game)Main.instance).Services.GetService(typeof(AssetReaderCollection)) != null)
		{
			pack.GetContentSource().GetAllAssetsStartingWith("Images" + Path.DirectorySeparatorChar);
			VanillaContentValidator.Instance.GetValidImageFilePaths();
		}
	}
}
