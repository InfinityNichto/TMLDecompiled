namespace Terraria.ID;

public static class PlayerVariantID
{
	public class Sets
	{
		public static SetFactory Factory = new SetFactory(Count);

		/// <summary>
		/// If <see langword="true" /> for a given skin variant (<see cref="F:Terraria.Player.skinVariant" />), then that variant is for a male (<see cref="P:Terraria.Player.Male" />) player.
		/// <br /> Defaults to <see langword="false" />.
		/// </summary>
		public static bool[] Male = Factory.CreateBoolSet(0, 1, 2, 3, 8, 10);

		/// <summary>
		/// Links a skin variant (<see cref="F:Terraria.Player.skinVariant" />) to the corresponding skin variant of the opposite gender.
		/// </summary>
		public static int[] AltGenderReference = Factory.CreateIntSet(0, 0, 4, 4, 0, 1, 5, 5, 1, 2, 6, 6, 2, 3, 7, 7, 3, 8, 9, 9, 8, 10, 11, 11, 10);

		/// <summary>
		/// The order of skin variants (<see cref="F:Terraria.Player.skinVariant" />) for male players.
		/// </summary>
		public static int[] VariantOrderMale = new int[6] { 0, 1, 2, 3, 8, 10 };

		/// <summary>
		/// The order of skin variants (<see cref="F:Terraria.Player.skinVariant" />) for female players.
		/// </summary>
		public static int[] VariantOrderFemale = new int[6] { 4, 5, 6, 7, 9, 11 };
	}

	public const int MaleStarter = 0;

	public const int MaleSticker = 1;

	public const int MaleGangster = 2;

	public const int MaleCoat = 3;

	public const int FemaleStarter = 4;

	public const int FemaleSticker = 5;

	public const int FemaleGangster = 6;

	public const int FemaleCoat = 7;

	public const int MaleDress = 8;

	public const int FemaleDress = 9;

	public const int MaleDisplayDoll = 10;

	public const int FemaleDisplayDoll = 11;

	public static readonly int Count = 12;
}
