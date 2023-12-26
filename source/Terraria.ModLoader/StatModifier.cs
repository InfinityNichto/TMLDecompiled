namespace Terraria.ModLoader;

public struct StatModifier
{
	public static readonly StatModifier Default = new StatModifier();

	/// <summary>
	/// An offset to the base value of the stat. Directly applied to the base stat before multipliers are applied.
	/// </summary>
	public float Base;

	/// <summary>
	/// Increase to the final value of the stat. Directly added to the stat after multipliers are applied.
	/// </summary>
	public float Flat;

	/// <summary>
	/// The combination of all additive multipliers. Starts at 1
	/// </summary>
	public float Additive { get; private set; }

	/// <summary>
	/// The combination of all multiplicative multipliers. Starts at 1. Applies 'after' all additive bonuses have been accumulated.
	/// </summary>
	public float Multiplicative { get; private set; }

	public StatModifier()
	{
		Base = 0f;
		Additive = 1f;
		Multiplicative = 1f;
		Flat = 0f;
	}

	public StatModifier(float additive, float multiplicative, float flat = 0f, float @base = 0f)
	{
		Base = 0f;
		Additive = 1f;
		Multiplicative = 1f;
		Flat = 0f;
		Additive = additive;
		Multiplicative = multiplicative;
		Flat = flat;
		Base = @base;
	}

	public override bool Equals(object obj)
	{
		if (!(obj is StatModifier i))
		{
			return false;
		}
		return this == i;
	}

	public override int GetHashCode()
	{
		return (((1713062080 * -1521134295 + Additive.GetHashCode()) * -1521134295 + Multiplicative.GetHashCode()) * -1521134295 + Flat.GetHashCode()) * -1521134295 + Base.GetHashCode();
	}

	/// <summary>
	/// By using the add operator, the supplied additive modifier is combined with the existing modifiers. For example, adding 0.12f would be equivalent to a typical 12% damage boost. For 99% of effects used in the game, this approach is used.
	/// </summary>
	/// <param name="m"></param>
	/// <param name="add">The additive modifier to add, where 0.01f is equivalent to 1%</param>
	/// <returns></returns>
	public static StatModifier operator +(StatModifier m, float add)
	{
		return new StatModifier(m.Additive + add, m.Multiplicative, m.Flat, m.Base);
	}

	/// <summary>
	/// By using the subtract operator, the supplied subtractive modifier is combined with the existing modifiers. For example, subtracting 0.12f would be equivalent to a typical 12% damage decrease. For 99% of effects used in the game, this approach is used.
	/// </summary>
	/// <param name="m"></param>
	/// <param name="sub">The additive modifier to subtract, where 0.01f is equivalent to 1%</param>
	/// <returns></returns>
	public static StatModifier operator -(StatModifier m, float sub)
	{
		return new StatModifier(m.Additive - sub, m.Multiplicative, m.Flat, m.Base);
	}

	/// <summary>
	/// The multiply operator applies a multiplicative effect to the resulting multiplicative modifier. This effect is very rarely used, typical effects use the add operator.
	/// </summary>
	/// <param name="m"></param>
	/// <param name="mul">The factor by which the multiplicative modifier is scaled</param>
	/// <returns></returns>
	public static StatModifier operator *(StatModifier m, float mul)
	{
		return new StatModifier(m.Additive, m.Multiplicative * mul, m.Flat, m.Base);
	}

	public static StatModifier operator /(StatModifier m, float div)
	{
		return new StatModifier(m.Additive, m.Multiplicative / div, m.Flat, m.Base);
	}

	public static StatModifier operator +(float add, StatModifier m)
	{
		return m + add;
	}

	public static StatModifier operator *(float mul, StatModifier m)
	{
		return m * mul;
	}

	public static bool operator ==(StatModifier m1, StatModifier m2)
	{
		if (m1.Additive == m2.Additive && m1.Multiplicative == m2.Multiplicative && m1.Flat == m2.Flat)
		{
			return m1.Base == m2.Base;
		}
		return false;
	}

	public static bool operator !=(StatModifier m1, StatModifier m2)
	{
		if (m1.Additive == m2.Additive && m1.Multiplicative == m2.Multiplicative && m1.Flat == m2.Flat)
		{
			return m1.Base != m2.Base;
		}
		return true;
	}

	/// <summary>
	/// Use this to apply the modifiers of this <see cref="T:Terraria.ModLoader.StatModifier" /> to the <paramref name="baseValue" />. You should assign
	/// the value passed in to the return result. For example:
	/// <para><br><c>damage = CritDamage.ApplyTo(damage)</c></br></para>
	/// <br></br>could be used to apply a crit damage modifier to a base damage value 
	/// </summary>
	/// <remarks>For help understanding the meanings of the applied values please make note of documentation for:
	/// <list type="bullet">
	/// <item><description><see cref="F:Terraria.ModLoader.StatModifier.Base" /></description></item>
	/// <item><description><see cref="P:Terraria.ModLoader.StatModifier.Additive" /></description></item>
	/// <item><description><see cref="P:Terraria.ModLoader.StatModifier.Multiplicative" /></description></item>
	/// <item><description><see cref="F:Terraria.ModLoader.StatModifier.Flat" /></description></item>
	/// </list>
	/// The order of operations of the modifiers are as follows:
	/// <list type="number">
	/// <item><description>The <paramref name="baseValue" /> is added to <see cref="F:Terraria.ModLoader.StatModifier.Base" /></description></item>
	/// <item><description>That result is multiplied by <see cref="P:Terraria.ModLoader.StatModifier.Additive" /></description></item>
	/// <item><description>The previous result is then multiplied by <see cref="P:Terraria.ModLoader.StatModifier.Multiplicative" /></description></item>
	/// <item><description>Finally, <see cref="F:Terraria.ModLoader.StatModifier.Flat" /> as added to the result of all previous calculations</description></item>
	/// </list>
	/// </remarks>
	/// <param name="baseValue">The starting value to apply modifiers to</param>
	/// <returns>The result of <paramref name="baseValue" /> after all modifiers are applied</returns>
	public float ApplyTo(float baseValue)
	{
		return (baseValue + Base) * Additive * Multiplicative + Flat;
	}

	public StatModifier CombineWith(StatModifier m)
	{
		return new StatModifier(Additive + m.Additive - 1f, Multiplicative * m.Multiplicative, Flat + m.Flat, Base + m.Base);
	}

	public StatModifier Scale(float scale)
	{
		return new StatModifier(1f + (Additive - 1f) * scale, 1f + (Multiplicative - 1f) * scale, Flat * scale, Base * scale);
	}
}
