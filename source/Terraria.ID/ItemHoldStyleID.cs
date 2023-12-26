namespace Terraria.ID;

/// <summary>
/// Assign <see cref="F:Terraria.Item.holdStyle" /> to one of these to give your item an animation while not in use, but being held. The <see href="https://terraria.wiki.gg/wiki/Use_Style_IDs">Use Style IDs wiki page</see> has examples and animations of each of these hold styles, make sure to scroll down to the "Hold styles" section.
/// <br /> If none of these hold animations match what you want, consider using the <see cref="M:Terraria.ModLoader.ModItem.HoldStyle(Terraria.Player,Microsoft.Xna.Framework.Rectangle)" /> hook to implement a custom animation.
/// </summary>
public class ItemHoldStyleID
{
	/// <summary>
	/// Default. No specific animation will happen while this item is held.
	/// <br />Used by any item by default. 
	/// </summary>
	public const int None = 0;

	/// <summary>
	/// Holding out in front of player.
	/// <br /> Used for items such as torches, candles, and glowsticks
	/// </summary>
	public const int HoldFront = 1;

	/// <summary>
	/// Holding up above player head.
	/// <br />Used by <see cref="F:Terraria.ID.ItemID.BreathingReed" />, <see cref="F:Terraria.ID.ItemID.Umbrella" />, and <see cref="F:Terraria.ID.ItemID.TragicUmbrella" />
	/// </summary>
	public const int HoldUp = 2;

	/// <summary>
	/// Holding out, but hand placed lower.
	/// <br />Used only by <see cref="F:Terraria.ID.ItemID.MagicalHarp" />
	/// </summary>
	public const int HoldHeavy = 3;

	/// <summary>
	/// Item held by both hands low.
	/// <br /> Used by Golf Clubs.
	/// </summary>
	public const int HoldGolfClub = 4;

	/// <summary>
	/// Item held very low.
	/// <br /> Used by guitar-shaped instruments
	/// </summary>
	public const int HoldGuitar = 5;

	/// <summary>
	/// Item is held high by off hand.
	/// <br /> Used by Nightglow.
	/// </summary>
	public const int HoldLamp = 6;

	/// <summary>
	/// Arm is held close to body, as if holding a remote control joystick.
	/// <br /> Used by Kwad Racer Drone
	/// </summary>
	public const int HoldRadio = 7;
}
