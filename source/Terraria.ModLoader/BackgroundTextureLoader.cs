using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.GameContent;
using Terraria.Localization;

namespace Terraria.ModLoader;

/// <summary>
/// This is the class that keeps track of all modded background textures and their slots/IDs.
/// </summary>
[Autoload(true, Side = ModSide.Client)]
public sealed class BackgroundTextureLoader : Loader
{
	internal static IDictionary<string, int> backgrounds = new Dictionary<string, int>();

	private static BackgroundTextureLoader Instance => LoaderManager.Get<BackgroundTextureLoader>();

	public BackgroundTextureLoader()
	{
		Initialize(Main.maxBackgrounds);
	}

	/// <summary> Returns the slot/ID of the background texture with the given full path. The path must be prefixed with a mod name. Throws exceptions on failure. </summary>
	public static int GetBackgroundSlot(string texture)
	{
		return backgrounds[texture];
	}

	/// <summary> Returns the slot/ID of the background texture with the given mod and path. Throws exceptions on failure. </summary>
	public static int GetBackgroundSlot(Mod mod, string texture)
	{
		return GetBackgroundSlot(mod.Name + "/" + texture);
	}

	/// <summary> Safely attempts to output the slot/ID of the background texture with the given full path. The path must be prefixed with a mod name. </summary>
	public static bool TryGetBackgroundSlot(string texture, out int slot)
	{
		return backgrounds.TryGetValue(texture, out slot);
	}

	/// <summary> Safely attempts to output the slot/ID of the background texture with the given mod and path. </summary>
	public static bool TryGetBackgroundSlot(Mod mod, string texture, out int slot)
	{
		return TryGetBackgroundSlot(mod.Name + "/" + texture, out slot);
	}

	/// <summary>
	/// Adds a texture to the list of background textures and assigns it a background texture slot.
	/// </summary>
	/// <param name="mod">The mod that owns this background.</param>
	/// <param name="texture">The texture.</param>
	public static void AddBackgroundTexture(Mod mod, string texture)
	{
		if (mod == null)
		{
			throw new ArgumentNullException("mod");
		}
		if (texture == null)
		{
			throw new ArgumentNullException("texture");
		}
		if (!mod.loading)
		{
			throw new Exception(Language.GetTextValue("tModLoader.LoadErrorNotLoading"));
		}
		ModContent.Request<Texture2D>(texture);
		backgrounds[texture] = Instance.Reserve();
	}

	internal override void ResizeArrays()
	{
		Array.Resize(ref TextureAssets.Background, base.TotalCount);
		Array.Resize(ref Main.backgroundHeight, base.TotalCount);
		Array.Resize(ref Main.backgroundWidth, base.TotalCount);
		foreach (string texture in backgrounds.Keys)
		{
			int slot = backgrounds[texture];
			Asset<Texture2D> tex = ModContent.Request<Texture2D>(texture);
			TextureAssets.Background[slot] = tex;
			Main.backgroundWidth[slot] = tex.Width();
			Main.backgroundHeight[slot] = tex.Height();
		}
	}

	internal override void Unload()
	{
		base.Unload();
		backgrounds.Clear();
	}

	internal static void AutoloadBackgrounds(Mod mod)
	{
		foreach (string item in from t in mod.RootContentSource.EnumerateAssets()
			where t.Contains("Backgrounds/")
			select t)
		{
			string texturePath = Path.ChangeExtension(item, null);
			string textureKey = mod.Name + "/" + texturePath;
			AddBackgroundTexture(mod, textureKey);
		}
	}
}
