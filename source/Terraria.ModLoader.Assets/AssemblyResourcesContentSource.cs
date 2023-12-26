using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using ReLogic.Content.Sources;

namespace Terraria.ModLoader.Assets;

public sealed class AssemblyResourcesContentSource : ContentSource
{
	private readonly string rootPath;

	private readonly Assembly assembly;

	public AssemblyResourcesContentSource(Assembly assembly, string rootPath = null)
	{
		this.assembly = assembly;
		IEnumerable<string> resourceNames = assembly.GetManifestResourceNames();
		if (rootPath != null)
		{
			resourceNames = from p in resourceNames
				where p.StartsWith(rootPath)
				select p.Substring(rootPath.Length);
		}
		this.rootPath = rootPath ?? "";
		SetAssetNames(resourceNames);
	}

	public override Stream OpenStream(string assetName)
	{
		return assembly.GetManifestResourceStream(rootPath + assetName);
	}
}
