using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria.ModLoader;

/// <summary>
/// Represents a builder toggle button shown in the top left corner of the screen while the inventory is shown. These toggles typically control wiring-related visiblility or other building-related quality of life features.<para />
/// The <see cref="M:Terraria.ModLoader.BuilderToggle.Active" /> property determines if the BuilderToggle should be shown to the user and is usually reliant on player-specific values. The <see cref="P:Terraria.ModLoader.BuilderToggle.CurrentState" /> property represents the current state of the toggle. For vanilla toggles a value of 0 is off and a value of 1 is on, but modded toggles can have <see cref="P:Terraria.ModLoader.BuilderToggle.NumberOfStates" /> values.
/// </summary>
public abstract class BuilderToggle : ModTexturedType, ILocalizedModType, IModType
{
	public static BuilderToggle RulerLine { get; private set; } = new RulerLineBuilderToggle();


	public static BuilderToggle RulerGrid { get; private set; } = new RulerGridBuilderToggle();


	public static BuilderToggle AutoActuate { get; private set; } = new AutoActuateBuilderToggle();


	public static BuilderToggle AutoPaint { get; private set; } = new AutoPaintBuilderToggle();


	public static BuilderToggle RedWireVisibility { get; private set; } = new RedWireVisibilityBuilderToggle();


	public static BuilderToggle BlueWireVisibility { get; private set; } = new BlueWireVisibilityBuilderToggle();


	public static BuilderToggle GreenWireVisibility { get; private set; } = new GreenWireVisibilityBuilderToggle();


	public static BuilderToggle YellowWireVisibility { get; private set; } = new YellowWireVisibilityBuilderToggle();


	public static BuilderToggle HideAllWires { get; private set; } = new HideAllWiresBuilderToggle();


	public static BuilderToggle ActuatorsVisibility { get; private set; } = new ActuatorsVisibilityBuilderToggle();


	public static BuilderToggle BlockSwap { get; private set; } = new BlockSwapBuilderToggle();


	public static BuilderToggle TorchBiome { get; private set; } = new TorchBiomeBuilderToggle();


	/// <summary>
	/// The path to the texture vanilla info displays use when hovering over an info display.
	/// </summary>
	public static string VanillaHoverTexture => "Terraria/Images/UI/InfoIcon_13";

	/// <summary>
	/// The outline texture drawn when the icon is hovered. By default a circular outline texture is used. Override this method and return <c>Texture + "_Hover"</c> or any other texture path to specify a custom outline texture for use with icons that are not circular.
	/// </summary>
	public virtual string HoverTexture => VanillaHoverTexture;

	/// <summary>
	/// This is the internal ID of this builder toggle.<para />
	/// Also serves as the index for <see cref="F:Terraria.Player.builderAccStatus" />.
	/// </summary>
	public int Type { get; internal set; }

	public virtual string LocalizationCategory => "BuilderToggles";

	/// <summary>
	/// This is the number of different functionalities your builder toggle will have.<br />
	/// For a toggle that has an On and Off state, you'd need 2 states!<para />
	/// </summary>
	/// <value>Default value is 2</value>
	public virtual int NumberOfStates { get; internal set; } = 2;


	/// <summary>
	/// This is the current state of this builder toggle. Every time the toggle is clicked, it will change.<para />
	/// The default state is 0. The state will be saved and loaded for the player to be consistent.
	/// </summary>
	public int CurrentState => Main.player[Main.myPlayer].builderAccStatus[Type];

	/// <summary>
	/// This dictates whether or not this builder toggle should be active (displayed).<para />
	/// This is usually determined by player-specific values, typically set in <see cref="M:Terraria.ModLoader.ModItem.UpdateInventory(Terraria.Player)" />.
	/// </summary>
	public virtual bool Active()
	{
		return false;
	}

	/// <summary>
	/// This is the overlay color that is drawn on top of the texture.
	/// </summary>
	/// <value>Default value is <see cref="P:Microsoft.Xna.Framework.Color.White" /></value>
	public virtual Color DisplayColorTexture()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		return Color.White;
	}

	/// <summary>
	/// This is the value that will show up when hovering on the toggle icon.
	/// You can specify different values per each available <see cref="P:Terraria.ModLoader.BuilderToggle.CurrentState" />
	/// </summary>
	public abstract string DisplayValue();

	public sealed override void SetupContent()
	{
		ModContent.Request<Texture2D>(Texture);
		SetStaticDefaults();
	}

	protected override void Register()
	{
		ModTypeLookup<BuilderToggle>.Register(this);
		Type = BuilderToggleLoader.Add(this);
	}
}
