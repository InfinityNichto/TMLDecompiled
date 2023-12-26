using System;
using Terraria.Graphics.Capture;

namespace Terraria.ModLoader;

/// <summary>
/// ModSceneEffect is an abstract class that your classes can derive from. It serves as a container for handling exclusive SceneEffect content such as backgrounds, music, and water styling.
/// </summary>
public abstract class ModSceneEffect : ModType
{
	public int Type { get; internal set; }

	/// <summary>
	/// The ModWaterStyle that will apply to water.
	/// </summary>
	public virtual ModWaterStyle WaterStyle => null;

	/// <summary>
	/// The ModSurfaceBackgroundStyle that will draw its background when the player is on the surface.
	/// </summary>
	public virtual ModSurfaceBackgroundStyle SurfaceBackgroundStyle => null;

	/// <summary>
	/// The ModUndergroundBackgroundStyle that will draw its background when the player is underground.
	/// </summary>
	public virtual ModUndergroundBackgroundStyle UndergroundBackgroundStyle => null;

	/// <summary>
	/// The music that will play. -1 for letting other music play, 0 for no music, &gt;0 for the given music to play. Defaults to -1.
	/// </summary>
	public virtual int Music => -1;

	/// <summary>
	/// The path to the texture that will display behind the map. Should be 115x65.
	/// </summary>
	public virtual string MapBackground => null;

	/// <summary>
	/// The <see cref="T:Terraria.ModLoader.SceneEffectPriority" /> of this SceneEffect layer. Determines the relative position compared to a vanilla SceneEffect.
	/// Analogously, if SceneEffect were competing in a wrestling match, this would be the 'Weight Class' that this SceneEffect is competing in.
	/// </summary>
	public virtual SceneEffectPriority Priority => SceneEffectPriority.None;

	/// <summary>
	/// Used to apply secondary color shading for the capture camera. For example, darkening the background with the GlowingMushroom style.
	/// </summary>
	public virtual CaptureBiome.TileColorStyle TileColorStyle => CaptureBiome.TileColorStyle.Normal;

	protected override void Register()
	{
		Type = LoaderManager.Get<SceneEffectLoader>().Register(this);
	}

	/// <summary>
	/// Forcefully registers the provided ModSceneEffect to LoaderManager.
	/// ModBiome and direct implementations call this.
	/// Does NOT cache the return type.
	/// </summary>
	internal void RegisterSceneEffect(ModSceneEffect modSceneEffect)
	{
		LoaderManager.Get<SceneEffectLoader>().Register(modSceneEffect);
	}

	/// <summary>
	/// Is invoked when two or more modded SceneEffect layers are active within the same <see cref="P:Terraria.ModLoader.ModSceneEffect.Priority" /> group to attempt to determine which one should take precedence, if it matters.
	/// It's uncommon to have the need to assign a weight - you'd have to specifically believe that you don't need higher SceneEffectPriority, but do need to be the active SceneEffect within the priority you designated.
	/// Analogously, if SceneEffect were competing in a wrestling match, this would be how likely the SceneEffect should win within its weight class.
	/// Is intentionally bounded at a max of 100% (1) to reduce complexity. Defaults to 50% (0.5).
	/// Typical calculations may include: 1) how many tiles are present as a percentage of target amount; 2) how far away you are from the cause of the SceneEffect
	/// </summary>
	public virtual float GetWeight(Player player)
	{
		return 0.5f;
	}

	/// <summary>
	/// Combines Priority and Weight to determine what SceneEffect should be active.
	/// Priority is used to do primary sorting with respect to vanilla SceneEffect.
	/// Weight will be used if multiple SceneEffect have the same SceneEffectPriority so as to attempt to distinguish them based on their needs.
	/// </summary>
	internal float GetCorrWeight(Player player)
	{
		return Math.Max(Math.Min(GetWeight(player), 1f), 0f) + (float)Priority;
	}

	/// <summary>
	/// Return true to make the SceneEffect apply its effects (as long as its priority and weight allow that).
	/// </summary>
	public virtual bool IsSceneEffectActive(Player player)
	{
		return false;
	}

	/// <summary>
	/// Allows you to create special visual effects in the area around the player. For example, the Blood Moon's red filter on the screen or the Slime Rain's falling slime in the background. You must create classes that override <see cref="T:Terraria.Graphics.Shaders.ScreenShaderData" /> or <see cref="T:Terraria.Graphics.Effects.CustomSky" />, add them in a Load hook, then call <see cref="M:Terraria.Player.ManageSpecialBiomeVisuals(System.String,System.Boolean,Microsoft.Xna.Framework.Vector2)" />. See the ExampleMod if you do not have access to the source code.
	/// <br /> This runs even if <see cref="M:Terraria.ModLoader.ModSceneEffect.IsSceneEffectActive(Terraria.Player)" /> returns false. Check <paramref name="isActive" /> for the active status.
	/// </summary>
	public virtual void SpecialVisuals(Player player, bool isActive)
	{
	}
}
