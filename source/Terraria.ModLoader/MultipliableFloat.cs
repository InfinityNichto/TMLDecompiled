namespace Terraria.ModLoader;

public readonly struct MultipliableFloat
{
	public static MultipliableFloat One = new MultipliableFloat(1f);

	public float Value { get; }

	public MultipliableFloat()
	{
		Value = 1f;
	}

	private MultipliableFloat(float f)
	{
		Value = 1f;
		Value = f;
	}

	public static MultipliableFloat operator *(MultipliableFloat f1, MultipliableFloat f2)
	{
		return new MultipliableFloat(f1.Value * f2.Value);
	}

	public static MultipliableFloat operator *(MultipliableFloat f1, float f2)
	{
		return new MultipliableFloat(f1.Value * f2);
	}

	public static MultipliableFloat operator /(MultipliableFloat f1, float f2)
	{
		return new MultipliableFloat(f1.Value / f2);
	}
}
