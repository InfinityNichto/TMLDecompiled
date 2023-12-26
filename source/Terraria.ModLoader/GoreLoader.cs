using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader.Core;

namespace Terraria.ModLoader;

public static class GoreLoader
{
	internal static readonly IDictionary<int, ModGore> gores = new Dictionary<int, ModGore>();

	public static int GoreCount { get; private set; } = GoreID.Count;


	/// <summary> Registers a new gore with the provided texture. </summary>
	public static bool AddGoreFromTexture<TGore>(Mod mod, string texture) where TGore : ModGore, new()
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
		return mod.AddContent(new TGore
		{
			nameOverride = Path.GetFileNameWithoutExtension(texture),
			textureOverride = texture
		});
	}

	internal static void RegisterModGore(ModGore modGore)
	{
		int id = (modGore.Type = GoreCount++);
		gores[id] = modGore;
	}

	internal static void AutoloadGores(Mod mod)
	{
		foreach (string item in from t in mod.RootContentSource.EnumerateAssets()
			where t.Contains("Gores/")
			select t)
		{
			string texturePath = Path.ChangeExtension(item, null);
			if (!mod.TryFind<ModGore>(Path.GetFileName(texturePath), out var _))
			{
				string textureKey = mod.Name + "/" + texturePath;
				AddGoreFromTexture<SimpleModGore>(mod, textureKey);
			}
		}
	}

	internal static void ResizeAndFillArrays()
	{
		Array.Resize(ref TextureAssets.Gore, GoreCount);
		LoaderUtils.ResetStaticMembers(typeof(GoreID));
		Array.Resize(ref ChildSafety.SafeGore, GoreCount);
		for (int i = GoreID.Count; i < GoreCount; i++)
		{
			GoreID.Sets.DisappearSpeed[i] = 1;
			GoreID.Sets.DisappearSpeedAlpha[i] = 1;
		}
		foreach (KeyValuePair<int, ModGore> pair in gores)
		{
			TextureAssets.Gore[pair.Key] = ModContent.Request<Texture2D>(pair.Value.Texture);
		}
	}

	internal static void Unload()
	{
		gores.Clear();
		GoreCount = GoreID.Count;
	}

	internal static ModGore GetModGore(int type)
	{
		gores.TryGetValue(type, out var modGore);
		return modGore;
	}

	internal static void SetupUpdateType(Gore gore)
	{
		if (gore.ModGore != null && gore.ModGore.UpdateType > 0)
		{
			gore.realType = gore.type;
			gore.type = gore.ModGore.UpdateType;
		}
	}

	internal static void TakeDownUpdateType(Gore gore)
	{
		if (gore.realType > 0)
		{
			gore.type = gore.realType;
			gore.realType = 0;
		}
	}
}
