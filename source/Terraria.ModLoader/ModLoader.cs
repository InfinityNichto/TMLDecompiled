using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using ReLogic.Content;
using ReLogic.OS;
using Terraria.Initializers;
using Terraria.Localization;
using Terraria.ModLoader.Assets;
using Terraria.ModLoader.Core;
using Terraria.ModLoader.Default;
using Terraria.ModLoader.Engine;
using Terraria.ModLoader.Exceptions;
using Terraria.ModLoader.UI;
using Terraria.ModLoader.UI.ModBrowser;
using Terraria.Social.Steam;

namespace Terraria.ModLoader;

/// <summary>
/// This serves as the central class which loads mods. It contains many static fields and methods related to mods and their contents.
/// </summary>
public static class ModLoader
{
	public static Version LastLaunchedTModLoaderVersion;

	public static string LastLaunchedTModLoaderAlphaSha;

	public static bool ShowWhatsNew;

	public static bool PreviewFreezeNotification;

	public static bool DownloadedDependenciesOnStartup;

	public static bool ShowFirstLaunchWelcomeMessage;

	public static bool SeenFirstLaunchModderWelcomeMessage;

	public static bool WarnedFamilyShare;

	public static Version LastPreviewFreezeNotificationSeen;

	public static bool BetaUpgradeWelcomed144;

	private static readonly IDictionary<string, Mod> modsByName = new Dictionary<string, Mod>(StringComparer.OrdinalIgnoreCase);

	internal static readonly string modBrowserPublicKey = "<RSAKeyValue><Modulus>oCZObovrqLjlgTXY/BKy72dRZhoaA6nWRSGuA+aAIzlvtcxkBK5uKev3DZzIj0X51dE/qgRS3OHkcrukqvrdKdsuluu0JmQXCv+m7sDYjPQ0E6rN4nYQhgfRn2kfSvKYWGefp+kqmMF9xoAq666YNGVoERPm3j99vA+6EIwKaeqLB24MrNMO/TIf9ysb0SSxoV8pC/5P/N6ViIOk3adSnrgGbXnFkNQwD0qsgOWDks8jbYyrxUFMc4rFmZ8lZKhikVR+AisQtPGUs3ruVh4EWbiZGM2NOkhOCOM4k1hsdBOyX2gUliD0yjK5tiU3LBqkxoi2t342hWAkNNb4ZxLotw==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";

	internal static string modBrowserPassphrase = "";

	internal static bool autoReloadAndEnableModsLeavingModBrowser = true;

	internal static bool autoReloadRequiredModsLeavingModsScreen = true;

	internal static bool removeForcedMinimumZoom;

	internal static int attackSpeedScalingTooltipVisibility = 1;

	internal static bool showMemoryEstimates = true;

	internal static bool notifyNewMainMenuThemes = true;

	internal static bool showNewUpdatedModsInfo = true;

	internal static bool skipLoad;

	internal static Action OnSuccessfulLoad;

	private static bool isLoading;

	/// <summary>A cached list of enabled mods (not necessarily currently loaded or even installed), mirroring the enabled.json file.</summary>
	private static HashSet<string> _enabledMods;

	public static string versionedName
	{
		get
		{
			if (BuildInfo.Purpose == BuildInfo.BuildPurpose.Stable)
			{
				return BuildInfo.versionedName;
			}
			return BuildInfo.versionedNameDevFriendly;
		}
	}

	public static string CompressedPlatformRepresentation => (Platform.IsWindows ? "w" : (Platform.IsLinux ? "l" : "m")) + ((InstallVerifier.DistributionPlatform == DistributionPlatform.GoG) ? "g" : "s") + "c";

	public static string ModPath => ModOrganizer.modPath;

	public static Mod[] Mods { get; private set; } = new Mod[0];


	internal static AssetRepository ManifestAssets { get; set; }

	internal static AssemblyResourcesContentSource ManifestContentSource { get; set; }

	internal static HashSet<string> EnabledMods => _enabledMods ?? (_enabledMods = ModOrganizer.LoadEnabledMods());

	/// <summary> Gets the instance of the Mod with the specified name. This will throw an exception if the mod cannot be found. </summary>
	/// <exception cref="T:System.Collections.Generic.KeyNotFoundException" />
	public static Mod GetMod(string name)
	{
		return modsByName[name];
	}

	/// <summary> Safely attempts to get the instance of the Mod with the specified name. </summary>
	/// <returns> Whether or not the requested instance has been found. </returns>
	public static bool TryGetMod(string name, out Mod result)
	{
		return modsByName.TryGetValue(name, out result);
	}

	/// <summary> Safely checks whether or not a mod with the specified internal name is currently loaded. </summary>
	/// <returns> Whether or not a mod with the provided internal name has been found. </returns>
	public static bool HasMod(string name)
	{
		return modsByName.ContainsKey(name);
	}

	internal static void EngineInit()
	{
		FileAssociationSupport.UpdateFileAssociation();
		FolderShortcutSupport.UpdateFolderShortcuts();
		MonoModHooks.Initialize();
		ZipExtractFix.Init();
		FNAFixes.Init();
		LoaderManager.AutoLoad();
	}

	internal static void PrepareAssets()
	{
		ManifestContentSource = new AssemblyResourcesContentSource(Assembly.GetExecutingAssembly());
		ManifestAssets = new AssetRepository(AssetInitializer.assetReaderCollection, new AssemblyResourcesContentSource[1] { ManifestContentSource })
		{
			AssetLoadFailHandler = Main.OnceFailedLoadingAnAsset
		};
	}

	internal static void BeginLoad(CancellationToken token)
	{
		Task.Run(delegate
		{
			Load(token);
		});
	}

	private static void Load(CancellationToken token = default(CancellationToken))
	{
		if (isLoading)
		{
			throw new Exception("Load called twice");
		}
		isLoading = true;
		if (!Unload())
		{
			return;
		}
		LocalMod[] availableMods = ModOrganizer.FindMods(logDuplicates: true);
		try
		{
			List<Mod> list = AssemblyManager.InstantiateMods(ModOrganizer.SelectAndSortMods(availableMods, token), token);
			list.Insert(0, new ModLoaderMod());
			Mods = list.ToArray();
			Mod[] mods = Mods;
			foreach (Mod mod2 in mods)
			{
				modsByName[mod2.Name] = mod2;
			}
			ModContent.Load(token);
			if (OnSuccessfulLoad != null)
			{
				OnSuccessfulLoad();
			}
			else
			{
				Main.menuMode = 0;
			}
		}
		catch when (token.IsCancellationRequested)
		{
			skipLoad = true;
			OnSuccessfulLoad = (Action)Delegate.Combine(OnSuccessfulLoad, (Action)delegate
			{
				Main.menuMode = 10000;
			});
			isLoading = false;
			Load();
		}
		catch (Exception e)
		{
			List<string> responsibleMods = new List<string>();
			if (e.Data.Contains("mod"))
			{
				responsibleMods.Add((string)e.Data["mod"]);
			}
			if (e.Data.Contains("mods"))
			{
				responsibleMods.AddRange((IEnumerable<string>)e.Data["mods"]);
			}
			responsibleMods.Remove("ModLoader");
			if (responsibleMods.Count == 0 && AssemblyManager.FirstModInStackTrace(new StackTrace(e), out var stackMod))
			{
				responsibleMods.Add(stackMod);
			}
			string msg = Language.GetTextValue("tModLoader.LoadError", string.Join(", ", responsibleMods));
			if (responsibleMods.Count == 1)
			{
				LocalMod mod = availableMods.FirstOrDefault((LocalMod m) => m.Name == responsibleMods[0]);
				if (mod != null)
				{
					msg += $" v{mod.properties.version}";
				}
				if (mod != null && mod.tModLoaderVersion.MajorMinorBuild() != BuildInfo.tMLVersion.MajorMinorBuild())
				{
					msg = msg + "\n" + Language.GetTextValue("tModLoader.LoadErrorVersionMessage", mod.tModLoaderVersion, versionedName);
				}
				else if (mod != null)
				{
					SteamedWraps.QueueForceValidateSteamInstall();
				}
				if (e is JITException)
				{
					msg = msg + "\nThe mod will need to be updated to match the current tModLoader version, or may be incompatible with the version of some of your other mods. Click the '" + Language.GetTextValue("tModLoader.OpenWebHelp") + "' button to learn more.";
				}
			}
			msg = ((responsibleMods.Count <= 0) ? (msg + "\n" + Language.GetTextValue("tModLoader.LoadErrorCulpritUnknown")) : (msg + "\n" + Language.GetTextValue("tModLoader.LoadErrorDisabled")));
			if (e is ReflectionTypeLoadException reflectionTypeLoadException)
			{
				msg = msg + "\n\n" + string.Join("\n", reflectionTypeLoadException.LoaderExceptions.Select((Exception x) => x.Message));
			}
			if (e.Data.Contains("contentType") && e.Data["contentType"] is Type contentType)
			{
				msg = msg + "\n" + Language.GetTextValue("tModLoader.LoadErrorContentType", contentType.FullName);
			}
			Logging.tML.Error((object)msg, e);
			foreach (string item in responsibleMods)
			{
				DisableMod(item);
			}
			isLoading = false;
			DisplayLoadError(msg, e, e.Data.Contains("fatal"), responsibleMods.Count == 0);
		}
		finally
		{
			isLoading = false;
			OnSuccessfulLoad = null;
			skipLoad = false;
			ModNet.NetReloadActive = false;
		}
	}

	internal static void Reload()
	{
		if (Main.dedServ)
		{
			Load();
		}
		else
		{
			Main.menuMode = 10002;
		}
	}

	internal static bool Unload()
	{
		try
		{
			WeakReference<Mod>[] weakModRefs = GetWeakModRefs();
			Mods_Unload();
			WarnModsStillLoaded(weakModRefs);
			return true;
		}
		catch (Exception e)
		{
			string msg = Language.GetTextValue("tModLoader.UnloadError");
			if (e.Data.Contains("mod"))
			{
				msg = msg + "\n" + Language.GetTextValue("tModLoader.DefensiveUnload", e.Data["mod"]);
			}
			Logging.tML.Fatal((object)msg, e);
			DisplayLoadError(msg, e, fatal: true);
			return false;
		}
	}

	internal static bool IsUnloadedModStillAlive(string name)
	{
		return AssemblyManager.OldLoadContexts().Contains(name);
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static WeakReference<Mod>[] GetWeakModRefs()
	{
		return Mods.Select((Mod x) => new WeakReference<Mod>(x)).ToArray();
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void Mods_Unload()
	{
		Interface.loadMods.SetLoadStage("tModLoader.MSUnloading", Mods.Length);
		WorldGen.clearWorld();
		ModContent.UnloadModContent();
		Mods = new Mod[0];
		modsByName.Clear();
		ModContent.Unload();
		MemoryTracking.Clear();
		Thread.MemoryBarrier();
		AssemblyManager.Unload();
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void WarnModsStillLoaded(IReadOnlyList<WeakReference<Mod>> weakModRefs)
	{
		foreach (string alcName in AssemblyManager.OldLoadContexts().Distinct())
		{
			if (weakModRefs.Any((WeakReference<Mod> modRef) => modRef.TryGetTarget(out var target) && target.Name == alcName))
			{
				Logging.tML.WarnFormat(alcName + " mod class still using memory. Some content references have probably not been cleared. Use a heap dump to figure out why.", Array.Empty<object>());
			}
			else
			{
				Logging.tML.WarnFormat(alcName + " AssemblyLoadContext still using memory. Some classes are being held by Terraria or another mod. Use a heap dump to figure out why.", Array.Empty<object>());
			}
		}
	}

	private static void DisplayLoadError(string msg, Exception e, bool fatal, bool continueIsRetry = false)
	{
		msg = msg + "\n\n" + (e.Data.Contains("hideStackTrace") ? e.Message : e.ToString());
		if (Main.dedServ)
		{
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine(msg);
			Console.ResetColor();
			if (fatal)
			{
				Console.WriteLine("Press any key to exit...");
				Console.ReadKey();
				Environment.Exit(-1);
			}
			else
			{
				Reload();
			}
		}
		else
		{
			Interface.errorMessage.Show(msg, fatal ? (-1) : 10006, null, e.HelpLink, continueIsRetry, !fatal);
		}
	}

	public static bool IsSignedBy(TmodFile mod, string xmlPublicKey)
	{
		RSAPKCS1SignatureDeformatter rSAPKCS1SignatureDeformatter = new RSAPKCS1SignatureDeformatter();
		AsymmetricAlgorithm v = AsymmetricAlgorithm.Create("RSA");
		rSAPKCS1SignatureDeformatter.SetHashAlgorithm("SHA1");
		v.FromXmlString(xmlPublicKey);
		rSAPKCS1SignatureDeformatter.SetKey(v);
		return rSAPKCS1SignatureDeformatter.VerifySignature(mod.Hash, mod.Signature);
	}

	internal static bool IsEnabled(string modName)
	{
		return EnabledMods.Contains(modName);
	}

	internal static void EnableMod(string modName)
	{
		SetModEnabled(modName, active: true);
	}

	internal static void DisableMod(string modName)
	{
		SetModEnabled(modName, active: false);
	}

	internal static void SetModEnabled(string modName, bool active)
	{
		if (active != IsEnabled(modName))
		{
			Logging.tML.Info((object)((active ? "Enabling" : "Disabling") + " Mod: " + modName));
			if (active)
			{
				EnabledMods.Add(modName);
			}
			else
			{
				EnabledMods.Remove(modName);
			}
			ModOrganizer.SaveEnabledMods();
		}
	}

	internal static void DisableAllMods()
	{
		Logging.tML.InfoFormat("Disabling All Mods: " + string.Join(", ", EnabledMods), Array.Empty<object>());
		EnabledMods.Clear();
		ModOrganizer.SaveEnabledMods();
	}

	internal static void SaveConfiguration()
	{
		Main.Configuration.Put("ModBrowserPassphrase", modBrowserPassphrase);
		Main.Configuration.Put("DownloadModsFromServers", ModNet.downloadModsFromServers);
		Main.Configuration.Put("OnlyDownloadSignedModsFromServers", ModNet.onlyDownloadSignedMods);
		Main.Configuration.Put("AutomaticallyReloadAndEnableModsLeavingModBrowser", autoReloadAndEnableModsLeavingModBrowser);
		Main.Configuration.Put("AutomaticallyReloadRequiredModsLeavingModsScreen", autoReloadRequiredModsLeavingModsScreen);
		Main.Configuration.Put("RemoveForcedMinimumZoom", removeForcedMinimumZoom);
		Main.Configuration.Put("attackSpeedScalingTooltipVisibility".ToUpperInvariant(), attackSpeedScalingTooltipVisibility);
		Main.Configuration.Put("ShowMemoryEstimates", showMemoryEstimates);
		Main.Configuration.Put("AvoidGithub", UIModBrowser.AvoidGithub);
		Main.Configuration.Put("AvoidImgur", UIModBrowser.AvoidImgur);
		Main.Configuration.Put("EarlyAutoUpdate", UIModBrowser.EarlyAutoUpdate);
		Main.Configuration.Put("ShowModMenuNotifications", notifyNewMainMenuThemes);
		Main.Configuration.Put("ShowNewUpdatedModsInfo", showNewUpdatedModsInfo);
		Main.Configuration.Put("LastSelectedModMenu", MenuLoader.LastSelectedModMenu);
		Main.Configuration.Put("KnownMenuThemes", MenuLoader.KnownMenuSaveString);
		Main.Configuration.Put("BossBarStyle", BossBarLoader.lastSelectedStyle);
		Main.Configuration.Put("SeenFirstLaunchModderWelcomeMessage", SeenFirstLaunchModderWelcomeMessage);
		Main.Configuration.Put("LastLaunchedTModLoaderVersion", BuildInfo.tMLVersion.ToString());
		Main.Configuration.Put("BetaUpgradeWelcomed144", BetaUpgradeWelcomed144);
		Main.Configuration.Put("LastLaunchedTModLoaderAlphaSha", (BuildInfo.Purpose == BuildInfo.BuildPurpose.Dev && BuildInfo.CommitSHA != "unknown") ? BuildInfo.CommitSHA : LastLaunchedTModLoaderAlphaSha);
		Main.Configuration.Put("LastPreviewFreezeNotificationSeen", LastPreviewFreezeNotificationSeen.ToString());
		Main.Configuration.Put("ModPackActive", ModOrganizer.ModPackActive);
	}

	internal static void LoadConfiguration()
	{
		Main.Configuration.Get("ModBrowserPassphrase", ref modBrowserPassphrase);
		Main.Configuration.Get("DownloadModsFromServers", ref ModNet.downloadModsFromServers);
		Main.Configuration.Get("OnlyDownloadSignedModsFromServers", ref ModNet.onlyDownloadSignedMods);
		Main.Configuration.Get("AutomaticallyReloadAndEnableModsLeavingModBrowser", ref autoReloadAndEnableModsLeavingModBrowser);
		Main.Configuration.Get("AutomaticallyReloadRequiredModsLeavingModsScreen", ref autoReloadRequiredModsLeavingModsScreen);
		Main.Configuration.Get("RemoveForcedMinimumZoom", ref removeForcedMinimumZoom);
		Main.Configuration.Get("attackSpeedScalingTooltipVisibility".ToUpperInvariant(), ref attackSpeedScalingTooltipVisibility);
		Main.Configuration.Get("ShowMemoryEstimates", ref showMemoryEstimates);
		Main.Configuration.Get("AvoidGithub", ref UIModBrowser.AvoidGithub);
		Main.Configuration.Get("AvoidImgur", ref UIModBrowser.AvoidImgur);
		Main.Configuration.Get("EarlyAutoUpdate", ref UIModBrowser.EarlyAutoUpdate);
		Main.Configuration.Get("ShowModMenuNotifications", ref notifyNewMainMenuThemes);
		Main.Configuration.Get("ShowNewUpdatedModsInfo", ref showNewUpdatedModsInfo);
		Main.Configuration.Get("LastSelectedModMenu", ref MenuLoader.LastSelectedModMenu);
		Main.Configuration.Get("KnownMenuThemes", ref MenuLoader.KnownMenuSaveString);
		Main.Configuration.Get("BossBarStyle", ref BossBarLoader.lastSelectedStyle);
		Main.Configuration.Get("SeenFirstLaunchModderWelcomeMessage", ref SeenFirstLaunchModderWelcomeMessage);
		Main.Configuration.Get("ModPackActive", ref ModOrganizer.ModPackActive);
		LastLaunchedTModLoaderVersion = new Version(Main.Configuration.Get("LastLaunchedTModLoaderVersion", "0.0"));
		Main.Configuration.Get("BetaUpgradeWelcomed144", ref BetaUpgradeWelcomed144);
		Main.Configuration.Get("LastLaunchedTModLoaderAlphaSha", ref LastLaunchedTModLoaderAlphaSha);
		LastPreviewFreezeNotificationSeen = new Version(Main.Configuration.Get("LastPreviewFreezeNotificationSeen", "0.0"));
	}

	internal static void MigrateSettings()
	{
		if (LastLaunchedTModLoaderVersion < new Version(0, 11, 7, 5))
		{
			showMemoryEstimates = true;
		}
		if (BuildInfo.IsPreview && LastLaunchedTModLoaderVersion != BuildInfo.tMLVersion)
		{
			ShowWhatsNew = true;
		}
		if (LastLaunchedTModLoaderVersion == new Version(0, 0))
		{
			ShowFirstLaunchWelcomeMessage = true;
		}
	}

	/// <summary>
	/// Allows type inference on T and F
	/// </summary>
	internal static void BuildGlobalHook<T, F>(ref F[] list, IList<T> providers, Expression<Func<T, F>> expr) where F : Delegate
	{
		list = BuildGlobalHook(providers, expr).Select(expr.Compile()).ToArray();
	}

	internal static T[] BuildGlobalHook<T, F>(IList<T> providers, Expression<Func<T, F>> expr) where F : Delegate
	{
		return providers.WhereMethodIsOverridden(expr).ToArray();
	}
}
