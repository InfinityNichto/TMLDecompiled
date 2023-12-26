using System.Text.RegularExpressions;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.Personalities;
using Terraria.Localization;

namespace Terraria.ModLoader;

/// <summary>
/// This class represents a biome added by a mod. It exists to centralize various biome related hooks, handling a lot of biome boilerplate, such as netcode.
/// <br />To check if a player is in the biome, use <see cref="M:Terraria.Player.InModBiome``1" />.
/// <br />Unlike <see cref="T:Terraria.ModLoader.ModSceneEffect" />, this defaults <see cref="P:Terraria.ModLoader.ModBiome.Music" /> to 0 and <see cref="P:Terraria.ModLoader.ModBiome.Priority" /> to <see cref="F:Terraria.ModLoader.SceneEffectPriority.BiomeLow" />.
/// </summary>
public abstract class ModBiome : ModSceneEffect, IShoppingBiome, ILocalizedModType, IModType
{
	public override SceneEffectPriority Priority => SceneEffectPriority.BiomeLow;

	public override int Music => 0;

	/// <summary>
	/// The torch item type that will be placed when under the effect of biome torches
	/// </summary>
	public virtual int BiomeTorchItemType => -1;

	/// <summary>
	/// The campfire item type that will be placed when under the effect of biome torches
	/// </summary>
	public virtual int BiomeCampfireItemType => -1;

	internal int ZeroIndexType => base.Type;

	public virtual string LocalizationCategory => "Biomes";

	/// <summary>
	/// The display name for this biome in the bestiary.
	/// </summary>
	public virtual LocalizedText DisplayName => this.GetLocalization("DisplayName", base.PrettyPrintName);

	/// <summary>
	/// The path to the 30x30 texture that will appear for this biome in the bestiary. Defaults to adding "_Icon" onto the usual namespace+classname derived texture path.
	/// <br /> Vanilla icons use a drop shadow at 40 percent opacity and the texture will be offset 1 pixel left and up from centered in the bestiary filter grid.
	/// </summary>
	public virtual string BestiaryIcon => (GetType().Namespace + "." + Name + "_Icon").Replace('.', '/');

	/// <summary>
	/// The path to the background texture that will appear for this biome behind NPC's in the bestiary. Defaults to adding "_Background" onto the usual namespace+classname derived texture path.
	/// </summary>
	public virtual string BackgroundPath => (GetType().Namespace + "." + Name + "_Background").Replace('.', '/');

	/// <summary>
	/// The color of the bestiary background.
	/// </summary>
	public virtual Color? BackgroundColor => null;

	public ModBiomeBestiaryInfoElement ModBiomeBestiaryInfoElement { get; internal set; }

	string IShoppingBiome.NameKey => this.GetLocalizationKey("TownNPCDialogueName");

	protected sealed override void Register()
	{
		base.Type = LoaderManager.Get<BiomeLoader>().Register(this);
		RegisterSceneEffect(this);
	}

	public sealed override void SetupContent()
	{
		SetStaticDefaults();
		ModBiomeBestiaryInfoElement = new ModBiomeBestiaryInfoElement(base.Mod, DisplayName.Key, BestiaryIcon, BackgroundPath, BackgroundColor);
		Language.GetOrRegister(((IShoppingBiome)this).NameKey, () => "the " + Regex.Replace(Name, "([A-Z])", " $1").Trim());
	}

	/// <summary>
	/// IsSceneEffectActive is auto-forwarded to read the result of IsBiomeActive.
	/// Do not need to implement when creating your ModBiome.
	/// </summary>
	public sealed override bool IsSceneEffectActive(Player player)
	{
		return player.modBiomeFlags[ZeroIndexType];
	}

	/// <summary>
	/// This is where you can set values for DisplayName.
	/// </summary>
	public override void SetStaticDefaults()
	{
	}

	/// <summary>
	/// Return true if the player is in the biome.
	/// </summary>
	/// <returns></returns>
	public virtual bool IsBiomeActive(Player player)
	{
		return false;
	}

	/// <summary>
	/// Override this hook to make things happen when the player enters the biome.
	/// </summary>
	public virtual void OnEnter(Player player)
	{
	}

	/// <summary>
	/// Override this hook to make things happen when the player is in the biome.
	/// </summary>
	public virtual void OnInBiome(Player player)
	{
	}

	/// <summary>
	/// Override this hook to make things happen when the player leaves the biome.
	/// </summary>
	public virtual void OnLeave(Player player)
	{
	}

	bool IShoppingBiome.IsInBiome(Player player)
	{
		return IsSceneEffectActive(player);
	}
}
