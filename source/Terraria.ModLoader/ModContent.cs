using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using ReLogic.Content;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.Creative;
using Terraria.GameContent.ItemDropRules;
using Terraria.GameContent.Skies;
using Terraria.GameContent.UI;
using Terraria.GameContent.UI.States;
using Terraria.GameInput;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.IO;
using Terraria.Localization;
using Terraria.Map;
using Terraria.ModLoader.Config;
using Terraria.ModLoader.Core;
using Terraria.ModLoader.Engine;
using Terraria.ModLoader.Exceptions;
using Terraria.ModLoader.IO;
using Terraria.ModLoader.UI;
using Terraria.ModLoader.Utilities;
using Terraria.UI;

namespace Terraria.ModLoader;

/// <summary>
/// Manages content added by mods.
/// Liasons between mod content and Terraria's arrays and oversees the Loader classes.
/// </summary>
public static class ModContent
{
	private static readonly char[] nameSplitters = new char[3] { '/', ' ', ':' };

	/// <summary> Returns the template instance of the provided content type (not the clone/new instance which gets added to Items/Players/NPCs etc. as the game is played). </summary>
	public static T GetInstance<T>() where T : class
	{
		return ContentInstance<T>.Instance;
	}

	/// <summary>
	/// Returns all registered content instances that derive from the provided type and that are added by all currently loaded mods.
	/// <br />This only includes the 'template' instance for each piece of content, not all the clones/new instances which get added to Items/Players/NPCs etc. as the game is played
	/// </summary>
	public static IEnumerable<T> GetContent<T>() where T : ILoadable
	{
		return ModLoader.Mods.SelectMany((Mod m) => m.GetContent<T>());
	}

	/// <summary> Attempts to find the template instance with the specified full name (not the clone/new instance which gets added to Items/Players/NPCs etc. as the game is played). Caching the result is recommended.<para />This will throw exceptions on failure. </summary>
	/// <exception cref="T:System.Collections.Generic.KeyNotFoundException" />
	public static T Find<T>(string fullname) where T : IModType
	{
		return ModTypeLookup<T>.Get(fullname);
	}

	/// <summary> Attempts to find the template instance with the specified name and mod name (not the clone/new instance which gets added to Items/Players/NPCs etc. as the game is played). Caching the result is recommended.<para />This will throw exceptions on failure. </summary>
	/// <exception cref="T:System.Collections.Generic.KeyNotFoundException" />
	public static T Find<T>(string modName, string name) where T : IModType
	{
		return ModTypeLookup<T>.Get(modName, name);
	}

	/// <summary> Safely attempts to find the template instance with the specified full name (not the clone/new instance which gets added to Items/Players/NPCs etc. as the game is played). Caching the result is recommended. </summary>
	/// <returns> Whether or not the requested instance has been found. </returns>
	public static bool TryFind<T>(string fullname, out T value) where T : IModType
	{
		return ModTypeLookup<T>.TryGetValue(fullname, out value);
	}

	/// <summary> Safely attempts to find the template instance with the specified name and mod name (not the clone/new instance which gets added to Items/Players/NPCs etc. as the game is played). Caching the result is recommended. </summary>
	/// <returns> Whether or not the requested instance has been found. </returns>
	public static bool TryFind<T>(string modName, string name, out T value) where T : IModType
	{
		return ModTypeLookup<T>.TryGetValue(modName, name, out value);
	}

	public static void SplitName(string name, out string domain, out string subName)
	{
		int slash = name.IndexOfAny(nameSplitters);
		if (slash < 0)
		{
			throw new MissingResourceException(Language.GetTextValue("tModLoader.LoadErrorMissingModQualifier", name));
		}
		domain = name.Substring(0, slash);
		subName = name.Substring(slash + 1);
	}

	/// <summary>
	/// Gets the byte representation of the file with the specified name. The name is in the format of "ModFolder/OtherFolders/FileNameWithExtension". Throws an ArgumentException if the file does not exist.
	/// </summary>
	/// <exception cref="T:Terraria.ModLoader.Exceptions.MissingResourceException"></exception>
	public static byte[] GetFileBytes(string name)
	{
		SplitName(name, out var modName, out var subName);
		if (!ModLoader.TryGetMod(modName, out var mod))
		{
			throw new MissingResourceException(Language.GetTextValue("tModLoader.LoadErrorModNotFoundDuringAsset", modName, name));
		}
		return mod.GetFileBytes(subName);
	}

	/// <summary>
	/// Returns whether or not a file with the specified name exists.
	/// </summary>
	public static bool FileExists(string name)
	{
		if (!name.Contains('/'))
		{
			return false;
		}
		SplitName(name, out var modName, out var subName);
		if (ModLoader.TryGetMod(modName, out var mod))
		{
			return mod.FileExists(subName);
		}
		return false;
	}

	/// <summary>
	/// Gets the asset with the specified name. Throws an Exception if the asset does not exist.
	/// </summary>
	/// <param name="name">The path to the asset without extension, including the mod name (or Terraria) for vanilla assets. Eg "ModName/Folder/FileNameWithoutExtension"</param>
	/// <param name="mode">The desired timing for when the asset actually loads. Use ImmediateLoad if you need correct dimensions immediately, such as with UI initialization</param>
	public static Asset<T> Request<T>(string name, AssetRequestMode mode = AssetRequestMode.AsyncLoad) where T : class
	{
		SplitName(name, out var modName, out var subName);
		if (Main.dedServ && Main.Assets == null)
		{
			Main.Assets = new AssetRepository(null);
		}
		if (modName == "Terraria")
		{
			return Main.Assets.Request<T>(subName, mode);
		}
		if (!ModLoader.TryGetMod(modName, out var mod))
		{
			throw new MissingResourceException(Language.GetTextValue("tModLoader.LoadErrorModNotFoundDuringAsset", modName, name));
		}
		return mod.Assets.Request<T>(subName, mode);
	}

	/// <summary>
	/// Returns whether or not a asset with the specified name exists.
	/// Includes the mod name prefix like Request
	/// </summary>
	public static bool HasAsset(string name)
	{
		if (Main.dedServ || string.IsNullOrWhiteSpace(name) || !name.Contains('/'))
		{
			return false;
		}
		SplitName(name, out var modName, out var subName);
		if (modName == "Terraria")
		{
			return Main.AssetSourceController.StaticSource.HasAsset(subName);
		}
		if (ModLoader.TryGetMod(modName, out var mod))
		{
			return mod.RootContentSource.HasAsset(subName);
		}
		return false;
	}

	public static bool RequestIfExists<T>(string name, out Asset<T> asset, AssetRequestMode mode = AssetRequestMode.AsyncLoad) where T : class
	{
		if (!HasAsset(name))
		{
			asset = null;
			return false;
		}
		asset = Request<T>(name, mode);
		return true;
	}

	/// <inheritdoc cref="M:Terraria.ModLoader.NPCLoader.GetNPC(System.Int32)" />
	public static ModNPC GetModNPC(int type)
	{
		return NPCLoader.GetNPC(type);
	}

	/// <inheritdoc cref="M:Terraria.ModLoader.NPCHeadLoader.GetBossHeadSlot(System.String)" />
	public static int GetModBossHeadSlot(string texture)
	{
		return NPCHeadLoader.GetBossHeadSlot(texture);
	}

	/// <inheritdoc cref="M:Terraria.ModLoader.NPCHeadLoader.GetHeadSlot(System.String)" />
	public static int GetModHeadSlot(string texture)
	{
		return NPCHeadLoader.GetHeadSlot(texture);
	}

	/// <inheritdoc cref="M:Terraria.ModLoader.ItemLoader.GetItem(System.Int32)" />
	public static ModItem GetModItem(int type)
	{
		return ItemLoader.GetItem(type);
	}

	/// <inheritdoc cref="M:Terraria.ModLoader.DustLoader.GetDust(System.Int32)" />
	public static ModDust GetModDust(int type)
	{
		return DustLoader.GetDust(type);
	}

	/// <inheritdoc cref="M:Terraria.ModLoader.ProjectileLoader.GetProjectile(System.Int32)" />
	public static ModProjectile GetModProjectile(int type)
	{
		return ProjectileLoader.GetProjectile(type);
	}

	/// <inheritdoc cref="M:Terraria.ModLoader.BuffLoader.GetBuff(System.Int32)" />
	public static ModBuff GetModBuff(int type)
	{
		return BuffLoader.GetBuff(type);
	}

	/// <inheritdoc cref="M:Terraria.ModLoader.EquipLoader.GetEquipTexture(Terraria.ModLoader.EquipType,System.Int32)" />
	public static EquipTexture GetEquipTexture(EquipType type, int slot)
	{
		return EquipLoader.GetEquipTexture(type, slot);
	}

	/// <inheritdoc cref="M:Terraria.ModLoader.MountLoader.GetMount(System.Int32)" />
	public static ModMount GetModMount(int type)
	{
		return MountLoader.GetMount(type);
	}

	/// <inheritdoc cref="M:Terraria.ModLoader.TileLoader.GetTile(System.Int32)" />
	public static ModTile GetModTile(int type)
	{
		return TileLoader.GetTile(type);
	}

	/// <inheritdoc cref="M:Terraria.ModLoader.WallLoader.GetWall(System.Int32)" />
	public static ModWall GetModWall(int type)
	{
		return WallLoader.GetWall(type);
	}

	/// <summary>
	/// Returns the ModWaterStyle with the given ID.
	/// </summary>
	public static ModWaterStyle GetModWaterStyle(int style)
	{
		return LoaderManager.Get<WaterStylesLoader>().Get(style);
	}

	/// <summary>
	/// Returns the ModWaterfallStyle with the given ID.
	/// </summary>
	public static ModWaterfallStyle GetModWaterfallStyle(int style)
	{
		return LoaderManager.Get<WaterFallStylesLoader>().Get(style);
	}

	/// <inheritdoc cref="M:Terraria.ModLoader.BackgroundTextureLoader.GetBackgroundSlot(System.String)" />
	public static int GetModBackgroundSlot(string texture)
	{
		return BackgroundTextureLoader.GetBackgroundSlot(texture);
	}

	/// <summary>
	/// Returns the ModSurfaceBackgroundStyle object with the given ID.
	/// </summary>
	public static ModSurfaceBackgroundStyle GetModSurfaceBackgroundStyle(int style)
	{
		return LoaderManager.Get<SurfaceBackgroundStylesLoader>().Get(style);
	}

	/// <summary>
	/// Returns the ModUndergroundBackgroundStyle object with the given ID.
	/// </summary>
	public static ModUndergroundBackgroundStyle GetModUndergroundBackgroundStyle(int style)
	{
		return LoaderManager.Get<UndergroundBackgroundStylesLoader>().Get(style);
	}

	/// <summary>
	/// Get the id (type) of a ModGore by class. Assumes one instance per class.
	/// </summary>
	public static int GoreType<T>() where T : ModGore
	{
		return GetInstance<T>()?.Type ?? 0;
	}

	/// <summary>
	/// Get the id (type) of a ModItem by class. Assumes one instance per class.
	/// </summary>
	public static int ItemType<T>() where T : ModItem
	{
		return GetInstance<T>()?.Type ?? 0;
	}

	/// <summary>
	/// Get the id (type) of a ModPrefix by class. Assumes one instance per class.
	/// </summary>
	public static int PrefixType<T>() where T : ModPrefix
	{
		return GetInstance<T>()?.Type ?? 0;
	}

	/// <summary>
	/// Get the id (type) of a ModRarity by class. Assumes one instance per class.
	/// </summary>
	public static int RarityType<T>() where T : ModRarity
	{
		return GetInstance<T>()?.Type ?? 0;
	}

	/// <summary>
	/// Get the id (type) of a ModDust by class. Assumes one instance per class.
	/// </summary>
	public static int DustType<T>() where T : ModDust
	{
		return GetInstance<T>()?.Type ?? 0;
	}

	/// <summary>
	/// Get the id (type) of a ModTile by class. Assumes one instance per class.
	/// </summary>
	public static int TileType<T>() where T : ModTile
	{
		return GetInstance<T>()?.Type ?? 0;
	}

	/// <summary>
	/// Get the id (type) of a ModPylon by class. Assumes one instance per class.
	/// If nothing is found, returns 0, or the "Forest Pylon" type.
	/// </summary>
	public static TeleportPylonType PylonType<T>() where T : ModPylon
	{
		return GetInstance<T>()?.PylonType ?? TeleportPylonType.SurfacePurity;
	}

	/// <summary>
	/// Get the id (type) of a ModTileEntity by class. Assumes one instance per class.
	/// </summary>
	public static int TileEntityType<T>() where T : ModTileEntity
	{
		return GetInstance<T>()?.Type ?? 0;
	}

	/// <summary>
	/// Get the id (type) of a ModWall by class. Assumes one instance per class.
	/// </summary>
	public static int WallType<T>() where T : ModWall
	{
		return GetInstance<T>()?.Type ?? 0;
	}

	/// <summary>
	/// Get the id (type) of a ModProjectile by class. Assumes one instance per class.
	/// </summary>
	public static int ProjectileType<T>() where T : ModProjectile
	{
		return GetInstance<T>()?.Type ?? 0;
	}

	/// <summary>
	/// Get the id (type) of a ModNPC by class. Assumes one instance per class.
	/// </summary>
	public static int NPCType<T>() where T : ModNPC
	{
		return GetInstance<T>()?.Type ?? 0;
	}

	/// <summary>
	/// Get the id (type) of a ModBuff by class. Assumes one instance per class.
	/// </summary>
	public static int BuffType<T>() where T : ModBuff
	{
		return GetInstance<T>()?.Type ?? 0;
	}

	/// <summary>
	/// Get the id (type) of a ModMount by class. Assumes one instance per class.
	/// </summary>
	public static int MountType<T>() where T : ModMount
	{
		return GetInstance<T>()?.Type ?? 0;
	}

	/// <summary>
	/// Get the id (type) of a ModEmoteBubble by class. Assumes one instance per class.
	/// </summary>
	public static int EmoteBubbleType<T>() where T : ModEmoteBubble
	{
		return GetInstance<T>()?.Type ?? 0;
	}

	internal static void Load(CancellationToken token)
	{
		CacheVanillaState();
		Interface.loadMods.SetLoadStage("tModLoader.MSLoading", ModLoader.Mods.Length);
		LoadModContent(token, delegate(Mod mod)
		{
			if (mod.Code != Assembly.GetExecutingAssembly())
			{
				AssemblyManager.JITMod(mod);
			}
			ContentInstance.Register(mod);
			mod.loading = true;
			mod.AutoloadConfig();
			mod.PrepareAssets();
			mod.Autoload();
			mod.Load();
			SystemLoader.OnModLoad(mod);
			mod.loading = false;
		});
		Interface.loadMods.SetLoadStage("tModLoader.MSResizing");
		ResizeArrays();
		RecipeGroupHelper.CreateRecipeGroupLookups();
		Main.ResourceSetsManager.AddModdedDisplaySets();
		Main.ResourceSetsManager.SetActiveFromOriginalConfigKey();
		Interface.loadMods.SetLoadStage("tModLoader.MSSetupContent", ModLoader.Mods.Length);
		LanguageManager.Instance.ReloadLanguage(resetValuesToKeysFirst: false);
		LoadModContent(token, delegate(Mod mod)
		{
			mod.SetupContent();
		});
		ContentSamples.Initialize();
		TileLoader.PostSetupContent();
		BuffLoader.PostSetupContent();
		Interface.loadMods.SetLoadStage("tModLoader.MSPostSetupContent", ModLoader.Mods.Length);
		LoadModContent(token, delegate(Mod mod)
		{
			mod.PostSetupContent();
			SystemLoader.PostSetupContent(mod);
			mod.TransferAllAssets();
		});
		MemoryTracking.Finish();
		if (Main.dedServ)
		{
			ModNet.AssignNetIDs();
		}
		ModNet.SetModNetDiagnosticsUI(ModLoader.Mods);
		Main.player[255] = new Player();
		BuffLoader.FinishSetup();
		ItemLoader.FinishSetup();
		NPCLoader.FinishSetup();
		PrefixLoader.FinishSetup();
		ProjectileLoader.FinishSetup();
		PylonLoader.FinishSetup();
		TileLoader.FinishSetup();
		WallLoader.FinishSetup();
		EmoteBubbleLoader.FinishSetup();
		MapLoader.FinishSetup();
		PlantLoader.FinishSetup();
		RarityLoader.FinishSetup();
		ConfigManager.FinishSetup();
		SystemLoader.ModifyGameTipVisibility(Main.gameTips.allTips);
		PlayerInput.reinitialize = true;
		SetupBestiary();
		NPCShopDatabase.Initialize();
		SetupRecipes(token);
		NPCShopDatabase.FinishSetup();
		ContentSamples.RebuildItemCreativeSortingIDsAfterRecipesAreSetUp();
		ItemSorting.SetupWhiteLists();
		LocalizationLoader.FinishSetup();
		MenuLoader.GotoSavedModMenu();
		BossBarLoader.GotoSavedStyle();
		ModOrganizer.SaveLastLaunchedMods();
	}

	private static void CacheVanillaState()
	{
		EffectsTracker.CacheVanillaState();
		DamageClassLoader.RegisterDefaultClasses();
		ExtraJumpLoader.RegisterDefaultJumps();
		InfoDisplayLoader.RegisterDefaultDisplays();
		BuilderToggleLoader.RegisterDefaultToggles();
	}

	private static void LoadModContent(CancellationToken token, Action<Mod> loadAction)
	{
		MemoryTracking.Checkpoint();
		int num = 0;
		Mod[] mods = ModLoader.Mods;
		foreach (Mod mod in mods)
		{
			token.ThrowIfCancellationRequested();
			Interface.loadMods.SetCurrentMod(num++, mod);
			try
			{
				loadAction(mod);
			}
			catch (Exception ex)
			{
				ex.Data["mod"] = mod.Name;
				throw;
			}
			finally
			{
				MemoryTracking.Update(mod.Name);
			}
		}
	}

	private static void SetupBestiary()
	{
		BestiaryDatabase bestiaryDatabase = new BestiaryDatabase();
		new BestiaryDatabaseNPCsPopulator().Populate(bestiaryDatabase);
		Main.BestiaryDB = bestiaryDatabase;
		ContentSamples.RebuildBestiarySortingIDsByBestiaryDatabaseContents(bestiaryDatabase);
		ItemDropDatabase itemDropDatabase = new ItemDropDatabase();
		itemDropDatabase.Populate();
		Main.ItemDropsDB = itemDropDatabase;
		bestiaryDatabase.Merge(Main.ItemDropsDB);
		if (!Main.dedServ)
		{
			Main.BestiaryUI = new UIBestiaryTest(Main.BestiaryDB);
		}
		Main.ItemDropSolver = new ItemDropResolver(itemDropDatabase);
		Main.BestiaryTracker = new BestiaryUnlocksTracker();
	}

	private static void SetupRecipes(CancellationToken token)
	{
		Interface.loadMods.SetLoadStage("tModLoader.MSAddingRecipes");
		for (int i = 0; i < Recipe.maxRecipes; i++)
		{
			token.ThrowIfCancellationRequested();
			Main.recipe[i] = new Recipe();
		}
		Recipe.numRecipes = 0;
		RecipeGroupHelper.ResetRecipeGroups();
		RecipeLoader.setupRecipes = true;
		Recipe.SetupRecipes();
		RecipeLoader.setupRecipes = false;
		ContentSamples.FixItemsAfterRecipesAreAdded();
		RecipeLoader.PostSetupRecipes();
	}

	internal static void UnloadModContent()
	{
		MenuLoader.Unload();
		int i = 0;
		foreach (Mod mod in ModLoader.Mods.Reverse())
		{
			Interface.loadMods.SetCurrentMod(i++, mod);
			try
			{
				MonoModHooks.RemoveAll(mod);
				mod.Close();
				mod.UnloadContent();
			}
			catch (Exception ex)
			{
				ex.Data["mod"] = mod.Name;
				throw;
			}
		}
	}

	internal static void Unload()
	{
		MonoModHooks.Clear();
		TypeCaching.Clear();
		ItemLoader.Unload();
		EquipLoader.Unload();
		PrefixLoader.Unload();
		DustLoader.Unload();
		TileLoader.Unload();
		PylonLoader.Unload();
		WallLoader.Unload();
		ProjectileLoader.Unload();
		NPCLoader.Unload();
		NPCHeadLoader.Unload();
		BossBarLoader.Unload();
		PlayerLoader.Unload();
		BuffLoader.Unload();
		MountLoader.Unload();
		RarityLoader.Unload();
		DamageClassLoader.Unload();
		InfoDisplayLoader.Unload();
		BuilderToggleLoader.Unload();
		ExtraJumpLoader.Unload();
		GoreLoader.Unload();
		PlantLoader.UnloadPlants();
		HairLoader.Unload();
		EmoteBubbleLoader.Unload();
		ResourceOverlayLoader.Unload();
		ResourceDisplaySetLoader.Unload();
		LoaderManager.Unload();
		GlobalBackgroundStyleLoader.Unload();
		PlayerDrawLayerLoader.Unload();
		SystemLoader.Unload();
		ResizeArrays(unloading: true);
		for (int i = 0; i < Recipe.maxRecipes; i++)
		{
			Main.recipe[i] = new Recipe();
		}
		Recipe.numRecipes = 0;
		RecipeGroupHelper.ResetRecipeGroups();
		Recipe.SetupRecipes();
		TileEntity.manager.Reset();
		MapLoader.UnloadModMap();
		ItemSorting.SetupWhiteLists();
		RecipeLoader.Unload();
		CommandLoader.Unload();
		TagSerializer.Reload();
		ModNet.Unload();
		ConfigManager.Unload();
		CustomCurrencyManager.Initialize();
		EffectsTracker.RemoveModEffects();
		Main.MapIcons = new MapIconOverlay().AddLayer(new SpawnMapLayer()).AddLayer(new TeleportPylonsMapLayer()).AddLayer(Main.Pings);
		ItemTrader.ChlorophyteExtractinator = ItemTrader.CreateChlorophyteExtractinator();
		Main.gameTips.Reset();
		CreativeItemSacrificesCatalog.Instance.Initialize();
		ContentSamples.Initialize();
		SetupBestiary();
		LocalizationLoader.Unload();
		CleanupModReferences();
	}

	private static void ResizeArrays(bool unloading = false)
	{
		DamageClassLoader.ResizeArrays();
		ExtraJumpLoader.ResizeArrays();
		ItemLoader.ResizeArrays(unloading);
		EquipLoader.ResizeAndFillArrays();
		PrefixLoader.ResizeArrays();
		DustLoader.ResizeArrays();
		TileLoader.ResizeArrays(unloading);
		WallLoader.ResizeArrays(unloading);
		ProjectileLoader.ResizeArrays(unloading);
		NPCLoader.ResizeArrays(unloading);
		NPCHeadLoader.ResizeAndFillArrays();
		MountLoader.ResizeArrays();
		BuffLoader.ResizeArrays();
		PlayerLoader.ResizeArrays();
		PlayerDrawLayerLoader.ResizeArrays();
		HairLoader.ResizeArrays();
		EmoteBubbleLoader.ResizeArrays();
		SystemLoader.ResizeArrays();
		if (!Main.dedServ)
		{
			GlobalBackgroundStyleLoader.ResizeAndFillArrays(unloading);
			GoreLoader.ResizeAndFillArrays();
		}
		LoaderManager.ResizeArrays();
		if (!Main.dedServ)
		{
			SkyManager.Instance["CreditsRoll"] = new CreditsRollSky();
		}
	}

	/// <summary>
	/// Several arrays and other fields hold references to various classes from mods, we need to clean them up to give properly coded mods a chance to be completely free of references
	/// so that they can be collected by the garbage collection. For most things eventually they will be replaced during gameplay, but we want the old instance completely gone quickly.
	/// </summary>
	internal static void CleanupModReferences()
	{
		for (int i = 0; i < Main.player.Length; i++)
		{
			Main.player[i] = new Player();
		}
		Main.clientPlayer = new Player();
		Main.ActivePlayerFileData = new PlayerFileData();
		Main._characterSelectMenu._playerList?.Clear();
		Main.PlayerList.Clear();
		if (ItemSlot.singleSlotArray[0] != null)
		{
			ItemSlot.singleSlotArray[0] = new Item();
		}
		WorldGen.ClearGenerationPasses();
	}

	public static Stream OpenRead(string assetName, bool newFileStream = false)
	{
		if (!assetName.StartsWith("tmod:"))
		{
			return File.OpenRead(assetName);
		}
		SplitName(assetName.Substring(5).Replace('\\', '/'), out var modName, out var entryPath);
		return ModLoader.GetMod(modName).GetFileStream(entryPath, newFileStream);
	}

	internal static void TransferCompletedAssets()
	{
		Mod[] mods = ModLoader.Mods;
		for (int i = 0; i < mods.Length; i++)
		{
			AssetRepository assetRepo = mods[i].Assets;
			if (assetRepo != null && !assetRepo.IsDisposed)
			{
				assetRepo.TransferCompletedAssets();
			}
		}
	}
}
