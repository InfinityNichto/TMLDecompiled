using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework.Content;
using ReLogic.OS;

namespace Terraria.ModLoader.Engine;

internal class TMLContentManager : ContentManager
{
	internal readonly TMLContentManager overrideContentManager;

	private readonly HashSet<string> ExistingImages = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);

	private int loadedAssets;

	public IEnumerable<string> RootDirectories
	{
		get
		{
			if (overrideContentManager != null)
			{
				yield return ((ContentManager)overrideContentManager).RootDirectory;
			}
			yield return ((ContentManager)this).RootDirectory;
		}
	}

	public TMLContentManager(IServiceProvider serviceProvider, string rootDirectory, TMLContentManager overrideContentManager)
		: base(serviceProvider, rootDirectory)
	{
		TryFixFileCasings(rootDirectory);
		this.overrideContentManager = overrideContentManager;
		CacheImagePaths(rootDirectory);
		if (overrideContentManager != null)
		{
			CacheImagePaths(((ContentManager)overrideContentManager).RootDirectory);
		}
		void CacheImagePaths(string path)
		{
			string basePath = Path.Combine(path, "Images");
			foreach (string file in Directory.EnumerateFiles(basePath, "*.xnb", SearchOption.AllDirectories))
			{
				ExistingImages.Add(Path.GetFileNameWithoutExtension(file.Remove(0, basePath.Length + 1)));
			}
		}
	}

	protected override Stream OpenStream(string assetName)
	{
		if (!assetName.StartsWith("tmod:"))
		{
			if (overrideContentManager != null && File.Exists(Path.Combine(((ContentManager)overrideContentManager).RootDirectory, assetName + ".xnb")))
			{
				try
				{
					using (new Logging.QuietExceptionHandle())
					{
						return ((ContentManager)overrideContentManager).OpenStream(assetName);
					}
				}
				catch
				{
				}
			}
			return ((ContentManager)this).OpenStream(assetName);
		}
		if (!assetName.EndsWith(".xnb"))
		{
			assetName += ".xnb";
		}
		return ModContent.OpenRead(assetName);
	}

	public override T Load<T>(string assetName)
	{
		if (assetName.StartsWith("tmod:"))
		{
			return ((ContentManager)this).ReadAsset<T>(assetName, (Action<IDisposable>)null);
		}
		loadedAssets++;
		if (loadedAssets % 1000 == 0)
		{
			Logging.Terraria.Info((object)$"Loaded {loadedAssets} vanilla assets");
		}
		return ((ContentManager)this).Load<T>(assetName);
	}

	/// <summary> Returns a path to the provided relative asset path, prioritizing overrides in the alternate content manager. Throws exceptions on failure. </summary>
	public string GetPath(string asset)
	{
		if (!TryGetPath(asset, out var result))
		{
			throw new FileNotFoundException("Unable to find asset '" + asset + "'.");
		}
		return result;
	}

	/// <summary> Safely attempts to get a path to the provided relative asset path, prioritizing overrides in the alternate content manager. </summary>
	public bool TryGetPath(string asset, out string result)
	{
		if (overrideContentManager != null && overrideContentManager.TryGetPath(asset, out result))
		{
			return true;
		}
		string path = Path.Combine(((ContentManager)this).RootDirectory, asset);
		result = (File.Exists(path) ? path : null);
		return result != null;
	}

	public bool ImageExists(string assetName)
	{
		return ExistingImages.Contains(assetName);
	}

	private static void TryFixFileCasings(string rootDirectory)
	{
		if (!Platform.IsWindows)
		{
			return;
		}
		string[] array = new string[7] { "Images/NPC_517.xnb", "Images/Gore_240.xnb", "Images/Projectile_179.xnb", "Images/Projectile_189.xnb", "Images/Projectile_618.xnb", "Images/Tiles_650.xnb", "Images/Item_2648" };
		foreach (string problematicAsset in array)
		{
			string expectedName = Path.GetFileName(problematicAsset);
			string expectedFullPath = Path.Combine(rootDirectory, problematicAsset);
			FileInfo faultyAssetInfo = new FileInfo(Path.Combine(rootDirectory, problematicAsset));
			if (faultyAssetInfo.Exists)
			{
				FileSystemInfo assetInfo = faultyAssetInfo.Directory.EnumerateFileSystemInfos(faultyAssetInfo.Name).First();
				string realName = assetInfo.Name;
				if (!(expectedName == realName))
				{
					string relativeRealPath = Path.GetRelativePath(rootDirectory, assetInfo.FullName);
					Logging.tML.Info((object)$"Found vanilla asset with wrong case, renaming: (from {rootDirectory}) {relativeRealPath} -> {problematicAsset}");
					File.Move(assetInfo.FullName, expectedFullPath);
				}
			}
		}
	}
}
