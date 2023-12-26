using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using log4net;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using ReLogic.Content.Sources;
using ReLogic.Utilities;
using Terraria.GameContent.Bestiary;
using Terraria.Localization;
using Terraria.ModLoader.Assets;
using Terraria.ModLoader.Config;
using Terraria.ModLoader.Core;
using Terraria.ModLoader.Exceptions;
using Terraria.ModLoader.UI;
using Terraria.UI;

namespace Terraria.ModLoader;

/// <summary>
/// Mod is an abstract class that you will override. It serves as a central place from which the mod's contents are stored. It provides methods for you to use or override.
/// </summary>
public class Mod
{
	internal short netID = -1;

	private IDisposable fileHandle;

	public ModSourceBestiaryInfoElement ModSourceBestiaryInfoElement;

	internal bool loading;

	private readonly Queue<Task> AsyncLoadQueue = new Queue<Task>();

	internal readonly IDictionary<Tuple<string, EquipType>, EquipTexture> equipTextures = new Dictionary<Tuple<string, EquipType>, EquipTexture>();

	internal readonly IList<ILoadable> content = new List<ILoadable>();

	internal bool initialTransferComplete;

	internal List<Exception> AssetExceptions = new List<Exception>();

	/// <summary>
	/// The TmodFile object created when tModLoader reads this mod.
	/// </summary>
	internal TmodFile File { get; set; }

	/// <summary>
	/// The assembly code this is loaded when tModLoader loads this mod. <br />
	/// Do NOT call <see cref="M:System.Reflection.Assembly.GetTypes" /> on this as it will error out if the mod uses the <see cref="T:Terraria.ModLoader.ExtendsFromModAttribute" /> attribute to inherit from weakly referenced mods. Use <see cref="M:Terraria.ModLoader.Core.AssemblyManager.GetLoadableTypes(System.Reflection.Assembly)" /> instead.
	/// </summary>
	public Assembly Code { get; internal set; }

	/// <summary>
	/// A logger with this mod's name for easy logging.
	/// </summary>
	public ILog Logger { get; internal set; }

	/// <summary>
	/// Stores the name of the mod. This name serves as the mod's identification, and also helps with saving everything your mod adds. By default this returns the name of the folder that contains all your code and stuff.
	/// </summary>
	public virtual string Name => File.Name;

	/// <summary>
	/// The version of tModLoader that was being used when this mod was built.
	/// </summary>
	public Version TModLoaderVersion { get; internal set; }

	/// <summary>
	/// This version number of this mod.
	/// </summary>
	public virtual Version Version => File.Version;

	public List<string> TranslationForMods { get; internal set; }

	/// <summary>
	/// Whether or not this mod will autoload content by default. Autoloading content means you do not need to manually add content through methods.
	/// </summary>
	public bool ContentAutoloadingEnabled { get; init; } = true;


	/// <summary>
	/// Whether or not this mod will automatically add images in the Gores folder as gores to the game, along with any ModGore classes that share names with the images. This means you do not need to manually call Mod.AddGore.
	/// </summary>
	public bool GoreAutoloadingEnabled { get; init; } = true;


	/// <summary>
	/// Whether or not this mod will automatically add music to the game. All supported audio files in a folder or subfolder of a folder named "Music" will be autoloaded as music.
	/// </summary>
	public bool MusicAutoloadingEnabled { get; init; } = true;


	/// <summary>
	/// Whether or not this mod will automatically add images in the Backgrounds folder as background textures to the game. This means you do not need to manually call Mod.AddBackgroundTexture.
	/// </summary>
	public bool BackgroundAutoloadingEnabled { get; init; } = true;


	/// <summary>
	/// The ModSide that controls how this mod is synced between client and server.
	/// </summary>
	public ModSide Side { get; internal set; }

	/// <summary>
	/// The display name of this mod in the Mods menu.
	/// </summary>
	public string DisplayName { get; internal set; }

	public AssetRepository Assets { get; private set; }

	public IContentSource RootContentSource { get; private set; }

	public short NetID => netID;

	public bool IsNetSynced => netID >= 0;

	public PreJITFilter PreJITFilter { get; protected set; } = new PreJITFilter();


	/// <summary>
	/// The amount of extra buff slots this mod desires for Players. This value is checked after Mod.Load but before Mod.PostSetupContent. The actual number of buffs the player can use will be 22 plus the max value of all enabled mods. In-game use Player.MaxBuffs to check the maximum number of buffs.
	/// </summary>
	public virtual uint ExtraPlayerBuffSlots { get; }

	internal void AutoloadConfig()
	{
		if (Code == null)
		{
			return;
		}
		foreach (Type type2 in from type in AssemblyManager.GetLoadableTypes(Code)
			orderby type.FullName
			select type)
		{
			if (!type2.IsAbstract && type2.IsSubclassOf(typeof(ModConfig)))
			{
				ModConfig mc = (ModConfig)Activator.CreateInstance(type2, nonPublic: true);
				if (mc.Mode == ConfigScope.ServerSide && (Side == ModSide.Client || Side == ModSide.NoSync))
				{
					throw new Exception("The ModConfig " + mc.Name + " can't be loaded because the config is ServerSide but this Mods ModSide isn't Both or Server");
				}
				if (mc.Mode == ConfigScope.ClientSide && Side == ModSide.Server)
				{
					throw new Exception("The ModConfig " + mc.Name + " can't be loaded because the config is ClientSide but this Mods ModSide is Server");
				}
				mc.Mod = this;
				string name = type2.Name;
				if (mc.Autoload(ref name))
				{
					AddConfig(name, mc);
				}
			}
		}
	}

	public void AddConfig(string name, ModConfig mc)
	{
		mc.Name = name;
		mc.Mod = this;
		ConfigManager.Add(mc);
		ContentInstance.Register(mc);
		ModTypeLookup<ModConfig>.Register(mc);
	}

	/// <summary> Call this to manually add a content instance of the specified type (with a parameterless constructor) to the game. </summary>
	/// <returns> true if the instance was successfully added </returns>
	public bool AddContent<T>() where T : ILoadable, new()
	{
		return AddContent(new T());
	}

	/// <summary> Call this to manually add the given content instance to the game. </summary>
	/// <param name="instance"> The content instance to add </param>
	/// <returns> true if the instance was successfully added </returns>
	public bool AddContent(ILoadable instance)
	{
		if (!loading)
		{
			throw new Exception(Language.GetTextValue("tModLoader.LoadErrorNotLoading"));
		}
		if (!instance.IsLoadingEnabled(this))
		{
			return false;
		}
		instance.Load(this);
		content.Add(instance);
		ContentInstance.Register(instance);
		return true;
	}

	/// <summary>
	/// Returns all registered content instances that are added by this mod.
	/// <br />This only includes the 'template' instance for each piece of content, not all the clones/new instances which get added to Items/Players/NPCs etc. as the game is played
	/// </summary>
	public IEnumerable<ILoadable> GetContent()
	{
		return content;
	}

	/// <summary>
	/// Returns all registered content instances that derive from the provided type that are added by this mod.
	/// <br />This only includes the 'template' instance for each piece of content, not all the clones/new instances which get added to Items/Players/NPCs etc. as the game is played
	/// </summary>
	public IEnumerable<T> GetContent<T>() where T : ILoadable
	{
		return content.OfType<T>();
	}

	/// <summary> Attempts to find the template instance from this mod with the specified name (not the clone/new instance which gets added to Items/Players/NPCs etc. as the game is played). Caching the result is recommended.<para />This will throw exceptions on failure. </summary>
	/// <exception cref="T:System.Collections.Generic.KeyNotFoundException" />
	public T Find<T>(string name) where T : IModType
	{
		return ModContent.Find<T>(Name, name);
	}

	/// <summary> Safely attempts to find the template instance from this mod with the specified name (not the clone/new instance which gets added to Items/Players/NPCs etc. as the game is played). Caching the result is recommended. </summary>
	/// <returns> Whether or not the requested instance has been found. </returns>
	public bool TryFind<T>(string name, out T value) where T : IModType
	{
		return ModContent.TryFind<T>(Name, name, out value);
	}

	/// <summary>
	/// Creates a localization key following the pattern of "Mods.{ModName}.{suffix}". Use this with <see cref="M:Terraria.Localization.Language.GetOrRegister(System.String,System.Func{System.String})" /> to retrieve a <see cref="T:Terraria.Localization.LocalizedText" /> for custom localization keys. Alternatively <see cref="M:Terraria.ModLoader.Mod.GetLocalization(System.String,System.Func{System.String})" /> can be used directly instead. Custom localization keys need to be registered during the mod loading process to appear automtaically in the localization files.
	/// </summary>
	/// <param name="suffix"></param>
	/// <returns></returns>
	public string GetLocalizationKey(string suffix)
	{
		return "Mods." + Name + "." + suffix;
	}

	/// <summary>
	/// Returns a <see cref="T:Terraria.Localization.LocalizedText" /> for this Mod with the provided <paramref name="suffix" />. The suffix will be used to generate a key by providing it to <see cref="M:Terraria.ModLoader.Mod.GetLocalizationKey(System.String)" />.
	/// <br />If no existing localization exists for the key, it will be defined so it can be exported to a matching mod localization file.
	/// </summary>
	/// <param name="suffix"></param>
	/// <param name="makeDefaultValue">A factory method for creating the default value, used to update localization files with missing entries</param>
	/// <returns></returns>
	public LocalizedText GetLocalization(string suffix, Func<string> makeDefaultValue = null)
	{
		return Language.GetOrRegister(GetLocalizationKey(suffix), makeDefaultValue);
	}

	/// <summary>
	/// Assigns a head texture to the given town NPC type.
	/// </summary>
	/// <param name="npcType">Type of the NPC.</param>
	/// <param name="texture">The texture.</param>
	/// <returns>The boss head texture slot</returns>
	/// <exception cref="T:Terraria.ModLoader.Exceptions.MissingResourceException"></exception>
	public int AddNPCHeadTexture(int npcType, string texture)
	{
		if (!loading)
		{
			throw new Exception("AddNPCHeadTexture can only be called from Mod.Load or Mod.Autoload");
		}
		int slot = NPCHeadLoader.ReserveHeadSlot();
		NPCHeadLoader.heads[texture] = slot;
		if (!Main.dedServ)
		{
			ModContent.Request<Texture2D>(texture);
		}
		NPCHeadLoader.npcToHead[npcType] = slot;
		return slot;
	}

	/// <summary>
	/// Assigns a head texture that can be used by NPCs on the map.
	/// </summary>
	/// <param name="texture">The texture.</param>
	/// <param name="npcType">An optional npc id for NPCID.Sets.BossHeadTextures</param>
	/// <returns>The boss head texture slot</returns>
	public int AddBossHeadTexture(string texture, int npcType = -1)
	{
		if (!loading)
		{
			throw new Exception("AddBossHeadTexture can only be called from Mod.Load or Mod.Autoload");
		}
		int slot = NPCHeadLoader.ReserveBossHeadSlot(texture);
		NPCHeadLoader.bossHeads[texture] = slot;
		ModContent.Request<Texture2D>(texture);
		if (npcType >= 0)
		{
			NPCHeadLoader.npcToBossHead[npcType] = slot;
		}
		return slot;
	}

	/// <summary>
	/// Retrieves the names of every file packaged into this mod.
	/// Note that this includes extensions, and for images the extension will always be <c>.rawimg</c>.
	/// </summary>
	/// <returns></returns>
	public List<string> GetFileNames()
	{
		return File?.GetFileNames();
	}

	/// <summary>
	/// Retrieve contents of files within the tmod file
	/// </summary>
	/// <param name="name">The name.</param>
	/// <returns></returns>
	public byte[] GetFileBytes(string name)
	{
		return File?.GetBytes(name);
	}

	/// <summary>
	/// Retrieve contents of files within the tmod file
	/// </summary>
	/// <param name="name">The name.</param>
	/// <param name="newFileStream"></param>
	/// <returns></returns>
	public Stream GetFileStream(string name, bool newFileStream = false)
	{
		return File?.GetStream(name, newFileStream);
	}

	public bool FileExists(string name)
	{
		if (File != null)
		{
			return File.HasFile(name);
		}
		return false;
	}

	public bool HasAsset(string assetName)
	{
		return RootContentSource.HasAsset(assetName);
	}

	public bool RequestAssetIfExists<T>(string assetName, out Asset<T> asset) where T : class
	{
		if (!HasAsset(assetName))
		{
			asset = null;
			return false;
		}
		asset = Assets.Request<T>(assetName);
		return true;
	}

	/// <summary>
	/// Used for weak inter-mod communication. This allows you to interact with other mods without having to reference their types or namespaces, provided that they have implemented this method.<br />
	/// The <see href="https://github.com/tModLoader/tModLoader/wiki/Expert-Cross-Mod-Content">Expert Cross Mod Content Guide</see> explains how to use this hook to implement and utilize cross-mod capabilities.
	/// </summary>
	public virtual object Call(params object[] args)
	{
		return null;
	}

	/// <summary>
	/// Creates a ModPacket object that you can write to and then send between servers and clients.
	/// </summary>
	/// <param name="capacity">The capacity.</param>
	/// <returns></returns>
	/// <exception cref="T:System.Exception">Cannot get packet for " + Name + " because it does not exist on the other side</exception>
	public ModPacket GetPacket(int capacity = 256)
	{
		if (netID < 0)
		{
			throw new Exception("Cannot get packet for " + Name + " because it does not exist on the other side");
		}
		ModPacket p = new ModPacket(250, capacity + 5);
		if (ModNet.NetModCount < 256)
		{
			p.Write((byte)netID);
		}
		else
		{
			p.Write(netID);
		}
		p.netID = netID;
		return p;
	}

	public ModConfig GetConfig(string name)
	{
		if (ConfigManager.Configs.TryGetValue(this, out List<ModConfig> configs))
		{
			return configs.Single((ModConfig x) => x.Name == name);
		}
		return null;
	}

	[Obsolete("Use Recipe.Create", true)]
	public Recipe CreateRecipe(int result, int amount = 1)
	{
		return Recipe.Create(result, amount);
	}

	[Obsolete("Use Recipe.Clone", true)]
	public Recipe CloneRecipe(Recipe recipe)
	{
		return recipe.Clone();
	}

	public virtual IContentSource CreateDefaultContentSource()
	{
		return new TModContentSource(File);
	}

	/// <summary>
	/// Override this method to run code after all content has been autoloaded. Here additional content can be manually loaded and Mod-wide tasks and setup can be done. For organization, it may be more suitable to split some things into various <see cref="M:Terraria.ModLoader.ModType.Load" /> methods, such as in <see cref="T:Terraria.ModLoader.ModSystem" /> classes, instead of doing everything here. <br />
	/// Beware that mod content has not finished loading here, things like ModContent lookup tables or ID Sets are not fully populated. Use <see cref="M:Terraria.ModLoader.Mod.PostSetupContent" /> for any logic that needs to act on all content being fully loaded.
	/// </summary>
	public virtual void Load()
	{
	}

	/// <summary>
	/// Allows you to load things in your mod after its content has been setup (arrays have been resized to fit the content, etc).
	/// </summary>
	public virtual void PostSetupContent()
	{
	}

	/// <summary>
	/// This is called whenever this mod is unloaded from the game. Use it to undo changes that you've made in Load that aren't automatically handled (for example, modifying the texture of a vanilla item). Mods are guaranteed to be unloaded in the reverse order they were loaded in.
	/// </summary>
	public virtual void Unload()
	{
	}

	/// <summary>
	/// Override this method to add recipe groups to this mod. You must add recipe groups by calling the RecipeGroup.RegisterGroup method here. A recipe group is a set of items that can be used interchangeably in the same recipe.
	/// </summary>
	[Obsolete("Use ModSystem.AddRecipeGroups", true)]
	public virtual void AddRecipeGroups()
	{
	}

	/// <summary>
	/// Override this method to add recipes to the game. It is recommended that you do so through instances of Recipe, since it provides methods that simplify recipe creation.
	/// </summary>
	[Obsolete("Use ModSystem.AddRecipes", true)]
	public virtual void AddRecipes()
	{
	}

	/// <summary>
	/// This provides a hook into the mod-loading process immediately after recipes have been added. You can use this to edit recipes added by other mods.
	/// </summary>
	[Obsolete("Use ModSystem.PostAddRecipes", true)]
	public virtual void PostAddRecipes()
	{
	}

	/// <summary>
	/// Close is called before Unload, and may be called at any time when mod unloading is imminent (such as when downloading an update, or recompiling)
	/// Use this to release any additional file handles, or stop streaming music.
	/// Make sure to call `base.Close()` at the end
	/// May be called multiple times before Unload
	/// </summary>
	public virtual void Close()
	{
		MusicLoader.CloseModStreams(this);
		fileHandle?.Dispose();
		if (File != null && File.IsOpen)
		{
			throw new IOException("TModFile has open handles: " + File.path);
		}
	}

	/// <summary>
	/// Called whenever a net message / packet is received from a client (if this is a server) or the server (if this is a client). whoAmI is the ID of whomever sent the packet (equivalent to the Main.myPlayer of the sender), and reader is used to read the binary data of the packet. <br />
	/// Note that many packets are sent from a client to the server and then relayed to the remaining clients. The whoAmI when the packet arrives at the remaining clients will be the servers <see cref="F:Terraria.Main.myPlayer" />, not the original clients <see cref="F:Terraria.Main.myPlayer" />. For packets only sent from a client to the server, relying on <paramref name="whoAmI" /> to identify the clients player is fine, but for packets that are relayed, the clients player index will need to be part of the packet itself to correctly identify the client that sent the original packet. Use <c>packet.Write((byte) Main.myPlayer);</c> to write and <c>int player = reader.ReadByte();</c> to read.
	/// </summary>
	/// <param name="reader">The reader.</param>
	/// <param name="whoAmI">The player the message is from. Only relevant for server code. For clients it will always be 255, the server. For the server it will be the whoAmI of the client.</param>
	public virtual void HandlePacket(BinaryReader reader, int whoAmI)
	{
	}

	internal void SetupContent()
	{
		LoaderUtils.ForEachAndAggregateExceptions(GetContent<ModType>(), delegate(ModType e)
		{
			e.SetupContent();
		});
	}

	internal void UnloadContent()
	{
		SystemLoader.OnModUnload(this);
		Unload();
		foreach (ILoadable item in content.Reverse())
		{
			item.Unload();
		}
		content.Clear();
		equipTextures.Clear();
		Assets?.Dispose();
	}

	internal void Autoload()
	{
		if (Code == null)
		{
			return;
		}
		LocalizationLoader.Autoload(this);
		Interface.loadMods.SubProgressText = Language.GetTextValue("tModLoader.MSFinishingResourceLoading");
		while (AsyncLoadQueue.Count > 0)
		{
			AsyncLoadQueue.Dequeue().Wait();
		}
		ModSourceBestiaryInfoElement = new ModSourceBestiaryInfoElement(this, DisplayName);
		if (ContentAutoloadingEnabled)
		{
			LoaderUtils.ForEachAndAggregateExceptions((from t in AssemblyManager.GetLoadableTypes(Code)
				where !t.IsAbstract && !t.ContainsGenericParameters
				where t.IsAssignableTo(typeof(ILoadable))
				where t.GetConstructor(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, Type.EmptyTypes) != null
				where AutoloadAttribute.GetValue(t).NeedsAutoloading
				select t).OrderBy<Type, string>((Type type) => type.FullName, StringComparer.InvariantCulture), delegate(Type t)
			{
				AddContent((ILoadable)Activator.CreateInstance(t, nonPublic: true));
			});
		}
		if (!Main.dedServ)
		{
			if (GoreAutoloadingEnabled)
			{
				GoreLoader.AutoloadGores(this);
			}
			if (MusicAutoloadingEnabled)
			{
				MusicLoader.AutoloadMusic(this);
			}
			if (BackgroundAutoloadingEnabled)
			{
				BackgroundTextureLoader.AutoloadBackgrounds(this);
			}
		}
	}

	internal void PrepareAssets()
	{
		fileHandle = File?.Open();
		RootContentSource = CreateDefaultContentSource();
		Assets = new AssetRepository(((IServiceProvider)((Game)Main.instance).Services).Get<AssetReaderCollection>(), new IContentSource[1] { RootContentSource })
		{
			AssetLoadFailHandler = OnceFailedLoadingAnAsset
		};
	}

	internal void TransferAllAssets()
	{
		initialTransferComplete = false;
		Assets.TransferAllAssets();
		initialTransferComplete = true;
		if (AssetExceptions.Count > 0)
		{
			if (AssetExceptions.Count == 1)
			{
				throw AssetExceptions[0];
			}
			if (AssetExceptions.Count > 0)
			{
				throw new MultipleException(AssetExceptions);
			}
		}
	}

	internal void OnceFailedLoadingAnAsset(string assetPath, Exception e)
	{
		if (initialTransferComplete)
		{
			Logging.Terraria.Error((object)("Failed to load asset: \"" + assetPath + "\""), e);
			FancyErrorPrinter.ShowFailedToLoadAssetError(e, assetPath);
		}
		else if (e is AssetLoadException)
		{
			List<string> list = RootContentSource.EnumerateAssets().ToList();
			List<string> cleanKeys = new List<string>();
			foreach (string key in (IEnumerable<string>)list)
			{
				string keyWithoutExtension = key.Substring(0, key.LastIndexOf("."));
				string extension = RootContentSource.GetExtension(keyWithoutExtension);
				if (extension != null)
				{
					cleanKeys.Add(key.Substring(0, key.LastIndexOf(extension)));
				}
			}
			List<string> reasons = new List<string>();
			RootContentSource.Rejections.TryGetRejections(reasons);
			MissingResourceException MissingResourceException = new MissingResourceException(reasons, assetPath.Replace("\\", "/"), cleanKeys);
			AssetExceptions.Add(MissingResourceException);
		}
		else
		{
			AssetExceptions.Add(e);
		}
	}
}
