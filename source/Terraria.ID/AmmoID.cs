using System.Collections.Generic;
using Terraria.ModLoader;

namespace Terraria.ID;

/// <summary>
/// AmmoID entries represent ammo types. Ammo items that share the same AmmoID value assigned to <see cref="F:Terraria.Item.ammo" /> can all be used as ammo for weapons using that same value for <see cref="F:Terraria.Item.useAmmo" />. AmmoID values are actually equivalent to the <see cref="T:Terraria.ID.ItemID" /> value of the iconic ammo item.<br />
/// The <see href="https://github.com/tModLoader/tModLoader/wiki/Basic-Ammo">Basic Ammo Guide</see> teaches more about ammo.
/// </summary>
public static class AmmoID
{
	public class Sets
	{
		/// <summary>
		/// Associates a launcher's item type (<see cref="F:Terraria.Item.type" />) and an ammo's item type (<see cref="F:Terraria.Item.type" />) to the projectile type (<see cref="F:Terraria.Projectile.type" />) they will shoot when used together.
		/// <br /> For example, a <see cref="F:Terraria.ID.ItemID.SnowmanCannon" /> used with a <see cref="F:Terraria.ID.ItemID.MiniNukeI" /> will fire the <see cref="F:Terraria.ID.ProjectileID.MiniNukeSnowmanRocketI" />.
		/// </summary>
		public static Dictionary<int, Dictionary<int, int>> SpecificLauncherAmmoProjectileMatches = new Dictionary<int, Dictionary<int, int>>
		{
			{
				759,
				new Dictionary<int, int>
				{
					{ 771, 134 },
					{ 772, 137 },
					{ 773, 140 },
					{ 774, 143 },
					{ 4445, 776 },
					{ 4446, 780 },
					{ 4457, 793 },
					{ 4458, 796 },
					{ 4459, 799 },
					{ 4447, 784 },
					{ 4448, 787 },
					{ 4449, 790 }
				}
			},
			{
				758,
				new Dictionary<int, int>
				{
					{ 771, 133 },
					{ 772, 136 },
					{ 773, 139 },
					{ 774, 142 },
					{ 4445, 777 },
					{ 4446, 781 },
					{ 4457, 794 },
					{ 4458, 797 },
					{ 4459, 800 },
					{ 4447, 785 },
					{ 4448, 788 },
					{ 4449, 791 }
				}
			},
			{
				760,
				new Dictionary<int, int>
				{
					{ 771, 135 },
					{ 772, 138 },
					{ 773, 141 },
					{ 774, 144 },
					{ 4445, 778 },
					{ 4446, 782 },
					{ 4457, 795 },
					{ 4458, 798 },
					{ 4459, 801 },
					{ 4447, 786 },
					{ 4448, 789 },
					{ 4449, 792 }
				}
			},
			{
				1946,
				new Dictionary<int, int>
				{
					{ 771, 338 },
					{ 772, 339 },
					{ 773, 340 },
					{ 774, 341 },
					{ 4445, 803 },
					{ 4446, 804 },
					{ 4457, 808 },
					{ 4458, 809 },
					{ 4459, 810 },
					{ 4447, 805 },
					{ 4448, 806 },
					{ 4449, 807 }
				}
			},
			{
				3930,
				new Dictionary<int, int>
				{
					{ 771, 715 },
					{ 772, 716 },
					{ 773, 717 },
					{ 774, 718 },
					{ 4445, 717 },
					{ 4446, 718 },
					{ 4457, 717 },
					{ 4458, 718 },
					{ 4459, 717 },
					{ 4447, 717 },
					{ 4448, 717 },
					{ 4449, 717 }
				}
			}
		};

		public static SetFactory Factory = new SetFactory(ItemLoader.ItemCount);

		/// <summary>
		/// If <see langword="true" /> for a given item type (<see cref="F:Terraria.Item.type" />), then items of that type are counted as arrows for the purposes of <see cref="F:Terraria.Player.arrowDamage" /> and the <see cref="F:Terraria.ID.ItemID.MagicQuiver" />.
		/// <br /> Defaults to <see langword="false" />.
		/// </summary>
		public static bool[] IsArrow = Factory.CreateBoolSet(false, Arrow, Stake);

		/// <summary>
		/// If <see langword="true" /> for a given item type (<see cref="F:Terraria.Item.type" />), then items of that type are counted as bullets for the purposes of <see cref="F:Terraria.Player.bulletDamage" />.
		/// <br /> Defaults to <see langword="false" />.
		/// </summary>
		public static bool[] IsBullet = Factory.CreateBoolSet(false, Bullet, CandyCorn);

		/// <summary>
		/// If <see langword="true" /> for a given item type (<see cref="F:Terraria.Item.type" />), then items of that type are counted as specialist ammo for the purposes of <see cref="F:Terraria.Player.specialistDamage" />.
		/// <br /> Defaults to <see langword="false" />.
		/// </summary>
		public static bool[] IsSpecialist = Factory.CreateBoolSet(false, Rocket, StyngerBolt, JackOLantern, NailFriendly, Coin, Flare, Dart, Snowball, Sand, FallenStar, Gel);
	}

	public static int None = 0;

	public static int Gel = 23;

	public static int Arrow = 40;

	public static int Coin = 71;

	public static int FallenStar = 75;

	public static int Bullet = 97;

	public static int Sand = 169;

	public static int Dart = 283;

	public static int Rocket = 771;

	public static int Solution = 780;

	public static int Flare = 931;

	public static int Snowball = 949;

	public static int StyngerBolt = 1261;

	public static int CandyCorn = 1783;

	public static int JackOLantern = 1785;

	public static int Stake = 1836;

	public static int NailFriendly = 3108;
}
