namespace Terraria.DataStructures;

public struct FishingAttempt
{
	public PlayerFishingConditions playerFishingConditions;

	/// <summary>
	/// The x-coordinate of tile this bobber is on, in tile coordinates.
	/// </summary>
	public int X;

	/// <summary>
	/// The y-coordinate of tile this bobber is on, in tile coordinates.
	/// </summary>
	public int Y;

	/// <summary>
	/// The projectile type (<see cref="F:Terraria.Projectile.type" />) of this bobber.
	/// </summary>
	public int bobberType;

	/// <summary>
	/// If <see langword="true" />, this fishing attempt can catch common items.
	/// </summary>
	public bool common;

	/// <summary>
	/// If <see langword="true" />, this fishing attempt can catch uncommon items.
	/// </summary>
	public bool uncommon;

	/// <summary>
	/// If <see langword="true" />, this fishing attempt can catch rare items.
	/// </summary>
	public bool rare;

	/// <summary>
	/// If <see langword="true" />, this fishing attempt can catch very rare items.
	/// </summary>
	public bool veryrare;

	/// <summary>
	/// If <see langword="true" />, this fishing attempt can catch legendary items.
	/// </summary>
	public bool legendary;

	/// <summary>
	/// If <see langword="true" />, this fishing attempt can catch crates.
	/// </summary>
	public bool crate;

	/// <summary>
	/// If <see langword="true" />, this fishing attempt is in lava.
	/// </summary>
	public bool inLava;

	/// <summary>
	/// If <see langword="true" />, this fishing attempt is in honey.
	/// </summary>
	public bool inHoney;

	/// <summary>
	/// The number of liquid tiles counted for this fishing attempt.
	/// </summary>
	public int waterTilesCount;

	/// <summary>
	/// The number of liquid tiles needed for proper fishing. If <see cref="F:Terraria.DataStructures.FishingAttempt.waterTilesCount" /> is less than this, then the player will recieve a <see cref="F:Terraria.DataStructures.FishingAttempt.waterQuality" /> percent debuff to their fishing power.
	/// <br /> This debuff is automatically applied to <see cref="F:Terraria.DataStructures.FishingAttempt.fishingLevel" />.
	/// </summary>
	public int waterNeededToFish;

	/// <summary>
	/// If positive, the percent decrease in fishing power this attempt has from missing liquid tiles.
	/// <br /> This is <strong>not</strong> how full the body of liquid is.
	/// </summary>
	public float waterQuality;

	/// <summary>
	/// The number of chums applied to this attempt. Fishing power from chum is automatically added to <see cref="F:Terraria.DataStructures.FishingAttempt.fishingLevel" />.
	/// </summary>
	public int chumsInWater;

	/// <summary>
	/// The fishing power of this attempt after all modifications. The higher this number, the better the attempt will go.
	/// </summary>
	public int fishingLevel;

	/// <summary>
	/// If <see langword="true" />, then this attempt can succeed if it is <see cref="F:Terraria.DataStructures.FishingAttempt.inLava" />.
	/// </summary>
	public bool CanFishInLava;

	/// <summary>
	/// How high in the sky this attempt takes place, in the range [<c>0.25f</c>, <c>1f</c>]. Any value below <c>1f</c> takes place approximately in the top 10% of the world.
	/// <br /> The lower this value, the smaller <see cref="F:Terraria.DataStructures.FishingAttempt.waterNeededToFish" /> will be, which is automatically applied.
	/// </summary>
	public float atmo;

	/// <summary>
	/// The item type (<see cref="F:Terraria.Item.type" />) of the quest fish the Angler wants, or <c>-1</c> if this player can't catch that fish today.
	/// </summary>
	public int questFish;

	/// <summary>
	/// A representation of the current height.
	/// <br /> <c>0</c> is space-level (50% of <see cref="F:Terraria.Main.worldSurface" /> or higher).
	/// <br /> <c>1</c> is the surface (<see cref="P:Terraria.Player.ZoneOverworldHeight" />).
	/// <br /> <c>2</c> is underground (<see cref="P:Terraria.Player.ZoneDirtLayerHeight" />).
	/// <br /> <c>3</c> is the caverns (<see cref="P:Terraria.Player.ZoneRockLayerHeight" />).
	/// <br /> <c>4</c> is the underworld (<see cref="P:Terraria.Player.ZoneUnderworldHeight" />).
	/// </summary>
	public int heightLevel;

	/// <summary>
	/// The item type (<see cref="F:Terraria.Item.type" />) of the caught item.
	/// </summary>
	public int rolledItemDrop;

	/// <summary>
	/// The item type (<see cref="F:Terraria.Item.type" />) of the caught NPC.
	/// </summary>
	public int rolledEnemySpawn;
}
