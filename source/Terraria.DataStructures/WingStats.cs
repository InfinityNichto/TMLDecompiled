namespace Terraria.DataStructures;

/// <summary>
/// Stores the stats and settings for a <see cref="T:Terraria.ID.ArmorIDs.Wing" /> equip.
/// </summary>
public struct WingStats
{
	public static readonly WingStats Default;

	/// <summary>
	/// The time in ticks that a player can fly for.
	/// </summary>
	public int FlyTime;

	/// <summary>
	/// Overrides the value of <see cref="F:Terraria.Player.accRunSpeed" /> while the player is in the air.
	/// <br /> Only applies if this value is greater than <see cref="F:Terraria.Player.accRunSpeed" />.
	/// <br /> If <c>-1f</c>, then no effect.
	/// </summary>
	public float AccRunSpeedOverride;

	/// <summary>
	/// A multiplier applied to <see cref="F:Terraria.Player.runAcceleration" /> while the player is in the air.
	/// </summary>
	public float AccRunAccelerationMult;

	/// <summary>
	/// If <see langword="true" />, then players can use these wings to hover.
	/// <br /> When hovering, <see cref="F:Terraria.DataStructures.WingStats.DownHoverSpeedOverride" /> and <see cref="F:Terraria.DataStructures.WingStats.DownHoverAccelerationMult" /> apply instead of <see cref="F:Terraria.DataStructures.WingStats.AccRunSpeedOverride" /> and <see cref="F:Terraria.DataStructures.WingStats.AccRunAccelerationMult" />, respectively.
	/// </summary>
	public bool HasDownHoverStats;

	/// <summary>
	/// Overrides the value of <see cref="F:Terraria.DataStructures.WingStats.AccRunSpeedOverride" /> if <see cref="F:Terraria.DataStructures.WingStats.HasDownHoverStats" /> is <see langword="true" /> and the player is hovering.
	/// </summary>
	public float DownHoverSpeedOverride;

	/// <summary>
	/// Overrides the value of <see cref="F:Terraria.DataStructures.WingStats.AccRunAccelerationMult" /> if <see cref="F:Terraria.DataStructures.WingStats.HasDownHoverStats" /> is <see langword="true" /> and the player is hovering.
	/// </summary>
	public float DownHoverAccelerationMult;

	/// <summary>
	/// Create a new <see cref="T:Terraria.DataStructures.WingStats" /> with the provided stats.
	/// </summary>
	public WingStats(int flyTime = 100, float flySpeedOverride = -1f, float accelerationMultiplier = 1f, bool hasHoldDownHoverFeatures = false, float hoverFlySpeedOverride = -1f, float hoverAccelerationMultiplier = 1f)
	{
		FlyTime = flyTime;
		AccRunSpeedOverride = flySpeedOverride;
		AccRunAccelerationMult = accelerationMultiplier;
		HasDownHoverStats = hasHoldDownHoverFeatures;
		DownHoverSpeedOverride = hoverFlySpeedOverride;
		DownHoverAccelerationMult = hoverAccelerationMultiplier;
	}

	/// <summary>
	/// Creates a new <see cref="T:Terraria.DataStructures.WingStats" /> with a speed multiplier applied to <see cref="F:Terraria.DataStructures.WingStats.AccRunSpeedOverride" /> and <see cref="F:Terraria.DataStructures.WingStats.DownHoverSpeedOverride" />.
	/// </summary>
	/// <param name="multiplier">The multiplier.</param>
	/// <returns>The modified <see cref="T:Terraria.DataStructures.WingStats" />.</returns>
	public WingStats WithSpeedBoost(float multiplier)
	{
		return new WingStats(FlyTime, AccRunSpeedOverride * multiplier, AccRunAccelerationMult, HasDownHoverStats, DownHoverSpeedOverride * multiplier, DownHoverAccelerationMult);
	}
}
