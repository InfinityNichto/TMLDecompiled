using System.Collections.Generic;

namespace Terraria.DataStructures;

/// <summary>
/// Determines the styles of <see cref="F:Terraria.ID.TileID.Plants" /> that an <see cref="T:Terraria.Item" /> creates on certain types of grass.
/// </summary>
public class FlowerPacketInfo
{
	/// <summary>
	/// The tile styles created on pure grass or in planters (<see cref="F:Terraria.ID.TileID.Grass" />, <see cref="F:Terraria.ID.TileID.ClayPot" />, <see cref="F:Terraria.ID.TileID.PlanterBox" />, <see cref="F:Terraria.ID.TileID.GolfGrass" />, <see cref="F:Terraria.ID.TileID.RockGolemHead" />).
	/// </summary>
	public List<int> stylesOnPurity = new List<int>();

	/// <summary>
	/// <strong>Unused.</strong>
	/// </summary>
	public List<int> stylesOnCorruption = new List<int>();

	/// <summary>
	/// <strong>Unused.</strong>
	/// </summary>
	public List<int> stylesOnCrimson = new List<int>();

	/// <summary>
	/// <strong>Unused.</strong>
	/// </summary>
	public List<int> stylesOnHallow = new List<int>();
}
