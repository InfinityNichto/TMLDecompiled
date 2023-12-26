using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Graphics;
using Terraria.IO;
using Terraria.Localization;
using Terraria.Map;
using Terraria.ModLoader.Core;
using Terraria.UI;
using Terraria.WorldBuilding;

namespace Terraria.ModLoader;

/// <summary>
/// This is where all <see cref="T:Terraria.ModLoader.ModSystem" /> hooks are gathered and called.
/// </summary>
public static class SystemLoader
{
	internal class HookList
	{
		public readonly MethodInfo method;

		private ModSystem[] arr = Array.Empty<ModSystem>();

		public HookList(MethodInfo method)
		{
			this.method = method;
		}

		public ReadOnlySpan<ModSystem> Enumerate()
		{
			return arr;
		}

		public void Update(IEnumerable<ModSystem> systems)
		{
			arr = systems.WhereMethodIsOverridden(method).ToArray();
		}
	}

	private delegate void DelegateModifyTransformMatrix(ref SpriteViewMatrix Transform);

	private delegate void DelegateModifySunLightColor(ref Color tileColor, ref Color backgroundColor);

	private delegate void DelegateModifyLightingBrightness(ref float scale);

	private delegate void DelegatePreDrawMapIconOverlay(IReadOnlyList<IMapLayer> layers, MapOverlayDrawContext mapOverlayDrawContext);

	private delegate void DelegatePostDrawFullscreenMap(ref string mouseText);

	private delegate void DelegateModifyTimeRate(ref double timeRate, ref double tileUpdateRate, ref double eventUpdateRate);

	private delegate void DelegateModifyWorldGenTasks(List<GenPass> passes, ref double totalWeight);

	private delegate bool DelegateHijackGetData(ref byte messageType, ref BinaryReader reader, int playerNumber);

	private delegate void DelegateTileCountsAvailable(ReadOnlySpan<int> tileCounts);

	internal static readonly List<ModSystem> Systems = new List<ModSystem>();

	internal static readonly Dictionary<Mod, List<ModSystem>> SystemsByMod = new Dictionary<Mod, List<ModSystem>>();

	private static readonly List<HookList> hooks = new List<HookList>();

	private static HookList HookOnLocalizationsLoaded = AddHook((Expression<Func<ModSystem, Action>>)((ModSystem s) => s.OnLocalizationsLoaded));

	private static HookList HookOnWorldLoad = AddHook((Expression<Func<ModSystem, Action>>)((ModSystem s) => s.OnWorldLoad));

	private static HookList HookOnWorldUnload = AddHook((Expression<Func<ModSystem, Action>>)((ModSystem s) => s.OnWorldUnload));

	private static HookList HookClearWorld = AddHook((Expression<Func<ModSystem, Action>>)((ModSystem s) => s.ClearWorld));

	private static HookList HookCanWorldBePlayed = AddHook((Expression<Func<ModSystem, Func<PlayerFileData, WorldFileData, bool>>>)((ModSystem s) => s.CanWorldBePlayed));

	private static HookList HookModifyScreenPosition = AddHook((Expression<Func<ModSystem, Action>>)((ModSystem s) => s.ModifyScreenPosition));

	private static HookList HookModifyTransformMatrix = AddHook((Expression<Func<ModSystem, DelegateModifyTransformMatrix>>)((ModSystem s) => s.ModifyTransformMatrix));

	private static HookList HookModifySunLightColor = AddHook((Expression<Func<ModSystem, DelegateModifySunLightColor>>)((ModSystem s) => s.ModifySunLightColor));

	private static HookList HookModifyLightingBrightness = AddHook((Expression<Func<ModSystem, DelegateModifyLightingBrightness>>)((ModSystem s) => s.ModifyLightingBrightness));

	private static HookList HookPreDrawMapIconOverlay = AddHook((Expression<Func<ModSystem, DelegatePreDrawMapIconOverlay>>)((ModSystem s) => s.PreDrawMapIconOverlay));

	private static HookList HookPostDrawFullscreenMap = AddHook((Expression<Func<ModSystem, DelegatePostDrawFullscreenMap>>)((ModSystem s) => s.PostDrawFullscreenMap));

	private static HookList HookUpdateUI = AddHook((Expression<Func<ModSystem, Action<GameTime>>>)((ModSystem s) => s.UpdateUI));

	private static HookList HookPreUpdateEntities = AddHook((Expression<Func<ModSystem, Action>>)((ModSystem s) => s.PreUpdateEntities));

	private static HookList HookPreUpdatePlayers = AddHook((Expression<Func<ModSystem, Action>>)((ModSystem s) => s.PreUpdatePlayers));

	private static HookList HookPostUpdatePlayers = AddHook((Expression<Func<ModSystem, Action>>)((ModSystem s) => s.PostUpdatePlayers));

	private static HookList HookPreUpdateNPCs = AddHook((Expression<Func<ModSystem, Action>>)((ModSystem s) => s.PreUpdateNPCs));

	private static HookList HookPostUpdateNPCs = AddHook((Expression<Func<ModSystem, Action>>)((ModSystem s) => s.PostUpdateNPCs));

	private static HookList HookPreUpdateGores = AddHook((Expression<Func<ModSystem, Action>>)((ModSystem s) => s.PreUpdateGores));

	private static HookList HookPostUpdateGores = AddHook((Expression<Func<ModSystem, Action>>)((ModSystem s) => s.PostUpdateGores));

	private static HookList HookPreUpdateProjectiles = AddHook((Expression<Func<ModSystem, Action>>)((ModSystem s) => s.PreUpdateProjectiles));

	private static HookList HookPostUpdateProjectiles = AddHook((Expression<Func<ModSystem, Action>>)((ModSystem s) => s.PostUpdateProjectiles));

	private static HookList HookPreUpdateItems = AddHook((Expression<Func<ModSystem, Action>>)((ModSystem s) => s.PreUpdateItems));

	private static HookList HookPostUpdateItems = AddHook((Expression<Func<ModSystem, Action>>)((ModSystem s) => s.PostUpdateItems));

	private static HookList HookPreUpdateDusts = AddHook((Expression<Func<ModSystem, Action>>)((ModSystem s) => s.PreUpdateDusts));

	private static HookList HookPostUpdateDusts = AddHook((Expression<Func<ModSystem, Action>>)((ModSystem s) => s.PostUpdateDusts));

	private static HookList HookPreUpdateTime = AddHook((Expression<Func<ModSystem, Action>>)((ModSystem s) => s.PreUpdateTime));

	private static HookList HookPostUpdateTime = AddHook((Expression<Func<ModSystem, Action>>)((ModSystem s) => s.PostUpdateTime));

	private static HookList HookPreUpdateWorld = AddHook((Expression<Func<ModSystem, Action>>)((ModSystem s) => s.PreUpdateWorld));

	private static HookList HookPostUpdateWorld = AddHook((Expression<Func<ModSystem, Action>>)((ModSystem s) => s.PostUpdateWorld));

	private static HookList HookPreUpdateInvasions = AddHook((Expression<Func<ModSystem, Action>>)((ModSystem s) => s.PreUpdateInvasions));

	private static HookList HookPostUpdateInvasions = AddHook((Expression<Func<ModSystem, Action>>)((ModSystem s) => s.PostUpdateInvasions));

	private static HookList HookPostUpdateEverything = AddHook((Expression<Func<ModSystem, Action>>)((ModSystem s) => s.PostUpdateEverything));

	private static HookList HookModifyInterfaceLayers = AddHook((Expression<Func<ModSystem, Action<List<GameInterfaceLayer>>>>)((ModSystem s) => s.ModifyInterfaceLayers));

	private static HookList HookModifyGameTipVisibility = AddHook((Expression<Func<ModSystem, Action<IReadOnlyList<GameTipData>>>>)((ModSystem s) => s.ModifyGameTipVisibility));

	private static HookList HookPostDrawInterface = AddHook((Expression<Func<ModSystem, Action<SpriteBatch>>>)((ModSystem s) => s.PostDrawInterface));

	private static HookList HookPostUpdateInput = AddHook((Expression<Func<ModSystem, Action>>)((ModSystem s) => s.PostUpdateInput));

	private static HookList HookPreSaveAndQuit = AddHook((Expression<Func<ModSystem, Action>>)((ModSystem s) => s.PreSaveAndQuit));

	private static HookList HookPostDrawTiles = AddHook((Expression<Func<ModSystem, Action>>)((ModSystem s) => s.PostDrawTiles));

	private static HookList HookModifyTimeRate = AddHook((Expression<Func<ModSystem, DelegateModifyTimeRate>>)((ModSystem s) => s.ModifyTimeRate));

	private static HookList HookPreWorldGen = AddHook((Expression<Func<ModSystem, Action>>)((ModSystem s) => s.PreWorldGen));

	private static HookList HookModifyWorldGenTasks = AddHook((Expression<Func<ModSystem, DelegateModifyWorldGenTasks>>)((ModSystem s) => s.ModifyWorldGenTasks));

	private static HookList HookPostWorldGen = AddHook((Expression<Func<ModSystem, Action>>)((ModSystem s) => s.PostWorldGen));

	private static HookList HookResetNearbyTileEffects = AddHook((Expression<Func<ModSystem, Action>>)((ModSystem s) => s.ResetNearbyTileEffects));

	private static HookList HookTileCountsAvailable = AddHook((Expression<Func<ModSystem, DelegateTileCountsAvailable>>)((ModSystem s) => s.TileCountsAvailable));

	private static HookList HookModifyHardmodeTasks = AddHook((Expression<Func<ModSystem, Action<List<GenPass>>>>)((ModSystem s) => s.ModifyHardmodeTasks));

	private static HookList HookHijackGetData = AddHook((Expression<Func<ModSystem, DelegateHijackGetData>>)((ModSystem s) => s.HijackGetData));

	private static HookList HookHijackSendData = AddHook((Expression<Func<ModSystem, Func<int, int, int, int, NetworkText, int, float, float, float, int, int, int, bool>>>)((ModSystem s) => s.HijackSendData));

	internal static HookList HookNetSend = AddHook((Expression<Func<ModSystem, Action<BinaryWriter>>>)((ModSystem s) => s.NetSend));

	internal static HookList HookNetReceive = AddHook((Expression<Func<ModSystem, Action<BinaryReader>>>)((ModSystem s) => s.NetReceive));

	internal static void Add(ModSystem modSystem)
	{
		if (!SystemsByMod.TryGetValue(modSystem.Mod, out var modsSystems))
		{
			modsSystems = (SystemsByMod[modSystem.Mod] = new List<ModSystem>());
		}
		Systems.Add(modSystem);
		modsSystems.Add(modSystem);
	}

	internal static void Unload()
	{
		Systems.Clear();
		SystemsByMod.Clear();
	}

	internal static void ResizeArrays()
	{
		RebuildHooks();
	}

	internal static void OnModLoad(Mod mod)
	{
		if (!SystemsByMod.TryGetValue(mod, out var systems))
		{
			return;
		}
		foreach (ModSystem item in systems)
		{
			item.OnModLoad();
		}
	}

	internal static void OnModUnload(Mod mod)
	{
		if (!SystemsByMod.TryGetValue(mod, out var systems))
		{
			return;
		}
		foreach (ModSystem item in systems)
		{
			item.OnModUnload();
		}
	}

	internal static void PostSetupContent(Mod mod)
	{
		if (!SystemsByMod.TryGetValue(mod, out var systems))
		{
			return;
		}
		foreach (ModSystem item in systems)
		{
			item.PostSetupContent();
		}
	}

	internal static void OnLocalizationsLoaded()
	{
		ReadOnlySpan<ModSystem> readOnlySpan = HookOnLocalizationsLoaded.Enumerate();
		for (int i = 0; i < readOnlySpan.Length; i++)
		{
			readOnlySpan[i].OnLocalizationsLoaded();
		}
	}

	internal static void AddRecipes(Mod mod)
	{
		if (!SystemsByMod.TryGetValue(mod, out var systems))
		{
			return;
		}
		foreach (ModSystem item in systems)
		{
			item.AddRecipes();
		}
	}

	internal static void PostAddRecipes(Mod mod)
	{
		if (!SystemsByMod.TryGetValue(mod, out var systems))
		{
			return;
		}
		foreach (ModSystem item in systems)
		{
			item.PostAddRecipes();
		}
	}

	internal static void PostSetupRecipes(Mod mod)
	{
		if (!SystemsByMod.TryGetValue(mod, out var systems))
		{
			return;
		}
		foreach (ModSystem item in systems)
		{
			item.PostSetupRecipes();
		}
	}

	internal static void AddRecipeGroups(Mod mod)
	{
		if (!SystemsByMod.TryGetValue(mod, out var systems))
		{
			return;
		}
		foreach (ModSystem item in systems)
		{
			item.AddRecipeGroups();
		}
	}

	public static void OnWorldLoad()
	{
		ReadOnlySpan<ModSystem> readOnlySpan = HookOnWorldLoad.Enumerate();
		for (int i = 0; i < readOnlySpan.Length; i++)
		{
			readOnlySpan[i].OnWorldLoad();
		}
	}

	public static void OnWorldUnload()
	{
		ReadOnlySpan<ModSystem> readOnlySpan = HookOnWorldUnload.Enumerate();
		for (int i = 0; i < readOnlySpan.Length; i++)
		{
			readOnlySpan[i].OnWorldUnload();
		}
	}

	public static void ClearWorld()
	{
		ReadOnlySpan<ModSystem> readOnlySpan = HookClearWorld.Enumerate();
		for (int i = 0; i < readOnlySpan.Length; i++)
		{
			readOnlySpan[i].ClearWorld();
		}
	}

	public static bool CanWorldBePlayed(PlayerFileData playerData, WorldFileData worldData, out ModSystem rejector)
	{
		ReadOnlySpan<ModSystem> readOnlySpan = HookCanWorldBePlayed.Enumerate();
		for (int i = 0; i < readOnlySpan.Length; i++)
		{
			ModSystem system = readOnlySpan[i];
			if (!system.CanWorldBePlayed(playerData, worldData))
			{
				rejector = system;
				return false;
			}
		}
		rejector = null;
		return true;
	}

	public static void ModifyScreenPosition()
	{
		ReadOnlySpan<ModSystem> readOnlySpan = HookModifyScreenPosition.Enumerate();
		for (int i = 0; i < readOnlySpan.Length; i++)
		{
			readOnlySpan[i].ModifyScreenPosition();
		}
	}

	public static void ModifyTransformMatrix(ref SpriteViewMatrix Transform)
	{
		ReadOnlySpan<ModSystem> readOnlySpan = HookModifyTransformMatrix.Enumerate();
		for (int i = 0; i < readOnlySpan.Length; i++)
		{
			readOnlySpan[i].ModifyTransformMatrix(ref Transform);
		}
	}

	public static void ModifySunLightColor(ref Color tileColor, ref Color backgroundColor)
	{
		if (!Main.gameMenu)
		{
			ReadOnlySpan<ModSystem> readOnlySpan = HookModifySunLightColor.Enumerate();
			for (int i = 0; i < readOnlySpan.Length; i++)
			{
				readOnlySpan[i].ModifySunLightColor(ref tileColor, ref backgroundColor);
			}
		}
	}

	public static void ModifyLightingBrightness(ref float negLight, ref float negLight2)
	{
		float scale = 1f;
		ReadOnlySpan<ModSystem> readOnlySpan = HookModifyLightingBrightness.Enumerate();
		for (int i = 0; i < readOnlySpan.Length; i++)
		{
			readOnlySpan[i].ModifyLightingBrightness(ref scale);
		}
		if (Lighting.NotRetro)
		{
			negLight *= scale;
			negLight2 *= scale;
		}
		else
		{
			negLight -= (scale - 1f) / 2.3076923f;
			negLight2 -= (scale - 1f) / 0.75f;
		}
		negLight = Math.Max(negLight, 0.001f);
		negLight2 = Math.Max(negLight2, 0.001f);
	}

	public static void PreDrawMapIconOverlay(IReadOnlyList<IMapLayer> layers, MapOverlayDrawContext mapOverlayDrawContext)
	{
		ReadOnlySpan<ModSystem> readOnlySpan = HookPreDrawMapIconOverlay.Enumerate();
		for (int i = 0; i < readOnlySpan.Length; i++)
		{
			readOnlySpan[i].PreDrawMapIconOverlay(layers, mapOverlayDrawContext);
		}
	}

	public static void PostDrawFullscreenMap(ref string mouseText)
	{
		ReadOnlySpan<ModSystem> readOnlySpan = HookPostDrawFullscreenMap.Enumerate();
		for (int i = 0; i < readOnlySpan.Length; i++)
		{
			readOnlySpan[i].PostDrawFullscreenMap(ref mouseText);
		}
	}

	public static void UpdateUI(GameTime gameTime)
	{
		if (!Main.gameMenu)
		{
			ReadOnlySpan<ModSystem> readOnlySpan = HookUpdateUI.Enumerate();
			for (int i = 0; i < readOnlySpan.Length; i++)
			{
				readOnlySpan[i].UpdateUI(gameTime);
			}
		}
	}

	public static void PreUpdateEntities()
	{
		ReadOnlySpan<ModSystem> readOnlySpan = HookPreUpdateEntities.Enumerate();
		for (int i = 0; i < readOnlySpan.Length; i++)
		{
			readOnlySpan[i].PreUpdateEntities();
		}
	}

	public static void PreUpdatePlayers()
	{
		ReadOnlySpan<ModSystem> readOnlySpan = HookPreUpdatePlayers.Enumerate();
		for (int i = 0; i < readOnlySpan.Length; i++)
		{
			readOnlySpan[i].PreUpdatePlayers();
		}
	}

	public static void PostUpdatePlayers()
	{
		ReadOnlySpan<ModSystem> readOnlySpan = HookPostUpdatePlayers.Enumerate();
		for (int i = 0; i < readOnlySpan.Length; i++)
		{
			readOnlySpan[i].PostUpdatePlayers();
		}
	}

	public static void PreUpdateNPCs()
	{
		ReadOnlySpan<ModSystem> readOnlySpan = HookPreUpdateNPCs.Enumerate();
		for (int i = 0; i < readOnlySpan.Length; i++)
		{
			readOnlySpan[i].PreUpdateNPCs();
		}
	}

	public static void PostUpdateNPCs()
	{
		ReadOnlySpan<ModSystem> readOnlySpan = HookPostUpdateNPCs.Enumerate();
		for (int i = 0; i < readOnlySpan.Length; i++)
		{
			readOnlySpan[i].PostUpdateNPCs();
		}
	}

	public static void PreUpdateGores()
	{
		ReadOnlySpan<ModSystem> readOnlySpan = HookPreUpdateGores.Enumerate();
		for (int i = 0; i < readOnlySpan.Length; i++)
		{
			readOnlySpan[i].PreUpdateGores();
		}
	}

	public static void PostUpdateGores()
	{
		ReadOnlySpan<ModSystem> readOnlySpan = HookPostUpdateGores.Enumerate();
		for (int i = 0; i < readOnlySpan.Length; i++)
		{
			readOnlySpan[i].PostUpdateGores();
		}
	}

	public static void PreUpdateProjectiles()
	{
		ReadOnlySpan<ModSystem> readOnlySpan = HookPreUpdateProjectiles.Enumerate();
		for (int i = 0; i < readOnlySpan.Length; i++)
		{
			readOnlySpan[i].PreUpdateProjectiles();
		}
	}

	public static void PostUpdateProjectiles()
	{
		ReadOnlySpan<ModSystem> readOnlySpan = HookPostUpdateProjectiles.Enumerate();
		for (int i = 0; i < readOnlySpan.Length; i++)
		{
			readOnlySpan[i].PostUpdateProjectiles();
		}
	}

	public static void PreUpdateItems()
	{
		ReadOnlySpan<ModSystem> readOnlySpan = HookPreUpdateItems.Enumerate();
		for (int i = 0; i < readOnlySpan.Length; i++)
		{
			readOnlySpan[i].PreUpdateItems();
		}
	}

	public static void PostUpdateItems()
	{
		ReadOnlySpan<ModSystem> readOnlySpan = HookPostUpdateItems.Enumerate();
		for (int i = 0; i < readOnlySpan.Length; i++)
		{
			readOnlySpan[i].PostUpdateItems();
		}
	}

	public static void PreUpdateDusts()
	{
		ReadOnlySpan<ModSystem> readOnlySpan = HookPreUpdateDusts.Enumerate();
		for (int i = 0; i < readOnlySpan.Length; i++)
		{
			readOnlySpan[i].PreUpdateDusts();
		}
	}

	public static void PostUpdateDusts()
	{
		ReadOnlySpan<ModSystem> readOnlySpan = HookPostUpdateDusts.Enumerate();
		for (int i = 0; i < readOnlySpan.Length; i++)
		{
			readOnlySpan[i].PostUpdateDusts();
		}
	}

	public static void PreUpdateTime()
	{
		ReadOnlySpan<ModSystem> readOnlySpan = HookPreUpdateTime.Enumerate();
		for (int i = 0; i < readOnlySpan.Length; i++)
		{
			readOnlySpan[i].PreUpdateTime();
		}
	}

	public static void PostUpdateTime()
	{
		ReadOnlySpan<ModSystem> readOnlySpan = HookPostUpdateTime.Enumerate();
		for (int i = 0; i < readOnlySpan.Length; i++)
		{
			readOnlySpan[i].PostUpdateTime();
		}
	}

	public static void PreUpdateWorld()
	{
		ReadOnlySpan<ModSystem> readOnlySpan = HookPreUpdateWorld.Enumerate();
		for (int i = 0; i < readOnlySpan.Length; i++)
		{
			readOnlySpan[i].PreUpdateWorld();
		}
	}

	public static void PostUpdateWorld()
	{
		ReadOnlySpan<ModSystem> readOnlySpan = HookPostUpdateWorld.Enumerate();
		for (int i = 0; i < readOnlySpan.Length; i++)
		{
			readOnlySpan[i].PostUpdateWorld();
		}
	}

	public static void PreUpdateInvasions()
	{
		ReadOnlySpan<ModSystem> readOnlySpan = HookPreUpdateInvasions.Enumerate();
		for (int i = 0; i < readOnlySpan.Length; i++)
		{
			readOnlySpan[i].PreUpdateInvasions();
		}
	}

	public static void PostUpdateInvasions()
	{
		ReadOnlySpan<ModSystem> readOnlySpan = HookPostUpdateInvasions.Enumerate();
		for (int i = 0; i < readOnlySpan.Length; i++)
		{
			readOnlySpan[i].PostUpdateInvasions();
		}
	}

	public static void PostUpdateEverything()
	{
		ReadOnlySpan<ModSystem> readOnlySpan = HookPostUpdateEverything.Enumerate();
		for (int i = 0; i < readOnlySpan.Length; i++)
		{
			readOnlySpan[i].PostUpdateEverything();
		}
	}

	public static void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
	{
		foreach (GameInterfaceLayer layer in layers)
		{
			layer.Active = true;
		}
		ReadOnlySpan<ModSystem> readOnlySpan = HookModifyInterfaceLayers.Enumerate();
		for (int i = 0; i < readOnlySpan.Length; i++)
		{
			readOnlySpan[i].ModifyInterfaceLayers(layers);
		}
	}

	public static void ModifyGameTipVisibility(IReadOnlyList<GameTipData> tips)
	{
		ReadOnlySpan<ModSystem> readOnlySpan = HookModifyGameTipVisibility.Enumerate();
		for (int i = 0; i < readOnlySpan.Length; i++)
		{
			readOnlySpan[i].ModifyGameTipVisibility(tips);
		}
	}

	public static void PostDrawInterface(SpriteBatch spriteBatch)
	{
		ReadOnlySpan<ModSystem> readOnlySpan = HookPostDrawInterface.Enumerate();
		for (int i = 0; i < readOnlySpan.Length; i++)
		{
			readOnlySpan[i].PostDrawInterface(spriteBatch);
		}
	}

	public static void PostUpdateInput()
	{
		ReadOnlySpan<ModSystem> readOnlySpan = HookPostUpdateInput.Enumerate();
		for (int i = 0; i < readOnlySpan.Length; i++)
		{
			readOnlySpan[i].PostUpdateInput();
		}
	}

	public static void PreSaveAndQuit()
	{
		ReadOnlySpan<ModSystem> readOnlySpan = HookPreSaveAndQuit.Enumerate();
		for (int i = 0; i < readOnlySpan.Length; i++)
		{
			readOnlySpan[i].PreSaveAndQuit();
		}
	}

	public static void PostDrawTiles()
	{
		ReadOnlySpan<ModSystem> readOnlySpan = HookPostDrawTiles.Enumerate();
		for (int i = 0; i < readOnlySpan.Length; i++)
		{
			readOnlySpan[i].PostDrawTiles();
		}
	}

	public static void ModifyTimeRate(ref double timeRate, ref double tileUpdateRate, ref double eventUpdateRate)
	{
		ReadOnlySpan<ModSystem> readOnlySpan = HookModifyTimeRate.Enumerate();
		for (int i = 0; i < readOnlySpan.Length; i++)
		{
			readOnlySpan[i].ModifyTimeRate(ref timeRate, ref tileUpdateRate, ref eventUpdateRate);
		}
	}

	public static void PreWorldGen()
	{
		ReadOnlySpan<ModSystem> readOnlySpan = HookPreWorldGen.Enumerate();
		for (int i = 0; i < readOnlySpan.Length; i++)
		{
			readOnlySpan[i].PreWorldGen();
		}
	}

	public static void ModifyWorldGenTasks(List<GenPass> passes, ref double totalWeight)
	{
		ReadOnlySpan<ModSystem> readOnlySpan = HookModifyWorldGenTasks.Enumerate();
		for (int i = 0; i < readOnlySpan.Length; i++)
		{
			ModSystem system = readOnlySpan[i];
			try
			{
				system.ModifyWorldGenTasks(passes, ref totalWeight);
			}
			catch (Exception e)
			{
				Utils.ShowFancyErrorMessage(string.Join("\n", system.FullName + " : " + Language.GetTextValue("tModLoader.WorldGenError"), e), 0);
				throw;
			}
		}
		passes.RemoveAll((GenPass x) => !x.Enabled);
	}

	public static void PostWorldGen()
	{
		ReadOnlySpan<ModSystem> readOnlySpan = HookPostWorldGen.Enumerate();
		for (int i = 0; i < readOnlySpan.Length; i++)
		{
			readOnlySpan[i].PostWorldGen();
		}
	}

	public static void ResetNearbyTileEffects()
	{
		ReadOnlySpan<ModSystem> readOnlySpan = HookResetNearbyTileEffects.Enumerate();
		for (int i = 0; i < readOnlySpan.Length; i++)
		{
			readOnlySpan[i].ResetNearbyTileEffects();
		}
	}

	public static void TileCountsAvailable(ReadOnlySpan<int> tileCounts)
	{
		ReadOnlySpan<ModSystem> readOnlySpan = HookTileCountsAvailable.Enumerate();
		for (int i = 0; i < readOnlySpan.Length; i++)
		{
			readOnlySpan[i].TileCountsAvailable(tileCounts);
		}
	}

	public static void ModifyHardmodeTasks(List<GenPass> passes)
	{
		ReadOnlySpan<ModSystem> readOnlySpan = HookModifyHardmodeTasks.Enumerate();
		for (int i = 0; i < readOnlySpan.Length; i++)
		{
			readOnlySpan[i].ModifyHardmodeTasks(passes);
		}
		passes.RemoveAll((GenPass x) => !x.Enabled);
	}

	internal static bool HijackGetData(ref byte messageType, ref BinaryReader reader, int playerNumber)
	{
		bool hijacked = false;
		long readerPos = reader.BaseStream.Position;
		long biggestReaderPos = readerPos;
		ReadOnlySpan<ModSystem> readOnlySpan = HookHijackGetData.Enumerate();
		for (int i = 0; i < readOnlySpan.Length; i++)
		{
			if (readOnlySpan[i].HijackGetData(ref messageType, ref reader, playerNumber))
			{
				hijacked = true;
				biggestReaderPos = Math.Max(reader.BaseStream.Position, biggestReaderPos);
			}
			reader.BaseStream.Position = readerPos;
		}
		if (hijacked)
		{
			reader.BaseStream.Position = biggestReaderPos;
		}
		return hijacked;
	}

	internal static bool HijackSendData(int whoAmI, int msgType, int remoteClient, int ignoreClient, NetworkText text, int number, float number2, float number3, float number4, int number5, int number6, int number7)
	{
		bool result = false;
		ReadOnlySpan<ModSystem> readOnlySpan = HookHijackSendData.Enumerate();
		for (int i = 0; i < readOnlySpan.Length; i++)
		{
			ModSystem system = readOnlySpan[i];
			result |= system.HijackSendData(whoAmI, msgType, remoteClient, ignoreClient, text, number, number2, number3, number4, number5, number6, number7);
		}
		return result;
	}

	private static HookList AddHook<F>(Expression<Func<ModSystem, F>> func) where F : Delegate
	{
		HookList hook = new HookList(func.ToMethodInfo());
		hooks.Add(hook);
		return hook;
	}

	private static void RebuildHooks()
	{
		foreach (HookList hook in hooks)
		{
			hook.Update(Systems);
		}
	}
}
