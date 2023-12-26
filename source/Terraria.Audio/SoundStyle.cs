using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using ReLogic.Content;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace Terraria.Audio;

/// <summary>
/// This data type describes in detail how a sound should be played.
/// <br /> Passable to the <see cref="M:Terraria.Audio.SoundEngine.PlaySound(Terraria.Audio.SoundStyle@,System.Nullable{Microsoft.Xna.Framework.Vector2},Terraria.Audio.SoundUpdateCallback)" /> method.
/// </summary>
public record struct SoundStyle
{
	/// <summary> The sound effect to play. </summary>
	public string SoundPath { get; set; }

	/// <summary>
	/// Controls which volume setting will this be affected by.
	/// <br /> Ambience sounds also don't play when the game is out of focus.
	/// </summary>
	public SoundType Type { get; set; }

	/// <summary> If defined, this string will be the only thing used to determine which styles should instances be shared with. </summary>
	public string? Identifier { get; set; }

	/// <summary>
	/// The max amount of sound instances that this style will allow creating, before stopping a playing sound or refusing to play a new one.
	/// <br /> Set to 0 for no limits.
	/// </summary>
	public int MaxInstances { get; set; }

	/// <summary> Determines what the action taken when the max amount of sound instances is reached. </summary>
	public SoundLimitBehavior SoundLimitBehavior { get; set; }

	/// <summary> If true, this sound won't play if the game's window isn't selected. </summary>
	public bool PlayOnlyIfFocused { get; set; }

	/// <summary> Whether or not to loop played sounds. </summary>
	public bool IsLooped { get; set; }

	/// <summary>
	/// Whether or not this sound obeys the <see cref="F:Terraria.Main.musicPitch" /> field to decide its pitch.<br />
	/// Defaults to false. Used in vanilla by the sounds for the Bell, the (Magical) Harp, and The Axe.<br />
	/// Could prove useful, but is kept internal for the moment.
	/// </summary>
	internal bool UsesMusicPitch { get; set; }

	/// <summary>
	/// An array of possible suffixes to randomly append to after <see cref="P:Terraria.Audio.SoundStyle.SoundPath" />.
	/// <br /> Setting this property resets <see cref="P:Terraria.Audio.SoundStyle.VariantsWeights" />.
	/// </summary>
	public ReadOnlySpan<int> Variants
	{
		get
		{
			return variants;
		}
		set
		{
			variantsWeights = null;
			totalVariantWeight = null;
			if (value.IsEmpty)
			{
				variants = null;
			}
			else
			{
				variants = value.ToArray();
			}
		}
	}

	/// <summary>
	/// An array of randomization weights to optionally go with <see cref="P:Terraria.Audio.SoundStyle.Variants" />.
	/// <br /> Set this last, if at all, as the <see cref="P:Terraria.Audio.SoundStyle.Variants" />'s setter resets all weights data.
	/// </summary>
	public ReadOnlySpan<float> VariantsWeights
	{
		get
		{
			return variantsWeights;
		}
		set
		{
			if (value.Length == 0)
			{
				variantsWeights = null;
				totalVariantWeight = null;
				return;
			}
			if (variants == null)
			{
				throw new ArgumentException("Variants weights must be set after variants.");
			}
			if (value.Length != variants.Length)
			{
				throw new ArgumentException("Variants and their weights must have the same length.");
			}
			variantsWeights = value.ToArray();
			totalVariantWeight = null;
		}
	}

	/// <summary> The volume multiplier to play sounds with. </summary>
	public float Volume
	{
		get
		{
			return volume;
		}
		set
		{
			volume = MathHelper.Clamp(value, 0f, 1f);
		}
	}

	/// <summary>
	/// The pitch <b>offset</b> to play sounds with.
	/// <para />In XNA and FNA, Pitch ranges from -1.0f (down one octave) to 1.0f (up one octave). 0.0f is unity (normal) pitch.
	/// </summary>
	public float Pitch
	{
		get
		{
			return pitch;
		}
		set
		{
			pitch = MathHelper.Clamp(value, -1f, 1f);
		}
	}

	/// <summary>
	/// The pitch offset randomness value. Cannot be negative.
	/// <br />With Pitch at 0.0, and PitchVariance at 1.0, used pitch will range from -0.5 to 0.5. 
	/// <para />In XNA and FNA, Pitch ranges from -1.0f (down one octave) to 1.0f (up one octave). 0.0f is unity (normal) pitch.
	/// </summary>
	public float PitchVariance
	{
		get
		{
			return pitchVariance;
		}
		set
		{
			if (value < 0f)
			{
				throw new ArgumentException("Pitch variance cannot be negative.", "value");
			}
			pitchVariance = value;
		}
	}

	/// <summary>
	/// A helper property for controlling both Pitch and PitchVariance at once.
	/// <para />In XNA and FNA, Pitch ranges from -1.0f (down one octave) to 1.0f (up one octave). 0.0f is unity (normal) pitch.
	/// </summary>
	public (float minPitch, float maxPitch) PitchRange
	{
		get
		{
			float halfVariance = PitchVariance;
			float item = Math.Max(-1f, Pitch - halfVariance);
			float maxPitch = Math.Min(1f, Pitch + halfVariance);
			return (minPitch: item, maxPitch: maxPitch);
		}
		set
		{
			float minPitch;
			float maxPitch;
			(minPitch, maxPitch) = value;
			if (minPitch > maxPitch)
			{
				throw new ArgumentException("Min pitch cannot be greater than max pitch.", "value");
			}
			minPitch = Math.Max(-1f, minPitch);
			maxPitch = Math.Min(1f, maxPitch);
			Pitch = (minPitch + maxPitch) * 0.5f;
			PitchVariance = maxPitch - minPitch;
		}
	}

	private const float MinPitchValue = -1f;

	private const float MaxPitchValue = 1f;

	private static readonly UnifiedRandom Random = new UnifiedRandom();

	private int[]? variants;

	private float[]? variantsWeights;

	private float? totalVariantWeight;

	private float volume;

	private float pitch;

	private float pitchVariance;

	private Asset<SoundEffect>? effectCache;

	private Asset<SoundEffect>?[]? variantsEffectCache;

	public SoundStyle(string soundPath, SoundType type = SoundType.Sound)
	{
		variantsWeights = null;
		totalVariantWeight = null;
		volume = 1f;
		pitch = 0f;
		pitchVariance = 0f;
		effectCache = null;
		variantsEffectCache = null;
		Identifier = null;
		MaxInstances = 1;
		SoundLimitBehavior = SoundLimitBehavior.ReplaceOldest;
		PlayOnlyIfFocused = false;
		IsLooped = false;
		UsesMusicPitch = false;
		SoundPath = soundPath;
		Type = type;
		variants = null;
	}

	public SoundStyle(string soundPath, int numVariants, SoundType type = SoundType.Sound)
		: this(soundPath, type)
	{
		if (numVariants > 1)
		{
			variants = CreateVariants(1, numVariants);
		}
	}

	public SoundStyle(string soundPath, int variantSuffixesStart, int numVariants, SoundType type = SoundType.Sound)
		: this(soundPath, type)
	{
		if (numVariants > 1)
		{
			variants = CreateVariants(variantSuffixesStart, numVariants);
		}
	}

	public SoundStyle(string soundPath, ReadOnlySpan<int> variants, SoundType type = SoundType.Sound)
		: this(soundPath, type)
	{
		this.variants = (variants.IsEmpty ? null : variants.ToArray());
	}

	public SoundStyle(string soundPath, ReadOnlySpan<(int variant, float weight)> weightedVariants, SoundType type = SoundType.Sound)
		: this(soundPath, type)
	{
		if (weightedVariants.IsEmpty)
		{
			variants = null;
			return;
		}
		variants = new int[weightedVariants.Length];
		variantsWeights = new float[weightedVariants.Length];
		for (int i = 0; i < weightedVariants.Length; i++)
		{
			var (variant, weight) = weightedVariants[i];
			variants[i] = variant;
			variantsWeights[i] = weight;
		}
	}

	public bool IsTheSameAs(SoundStyle style)
	{
		if (Identifier != null && Identifier == style.Identifier)
		{
			return true;
		}
		if (SoundPath == style.SoundPath)
		{
			return true;
		}
		return false;
	}

	public SoundEffect GetRandomSound()
	{
		Asset<SoundEffect> asset;
		if (variants == null || variants.Length == 0)
		{
			asset = effectCache ?? (effectCache = ModContent.Request<SoundEffect>(SoundPath, AssetRequestMode.ImmediateLoad));
		}
		else
		{
			int variantIndex = GetRandomVariantIndex();
			int variant = variants[variantIndex];
			Array.Resize(ref variantsEffectCache, variants.Length);
			ref Asset<SoundEffect> reference = ref variantsEffectCache[variantIndex];
			asset = reference ?? (reference = ModContent.Request<SoundEffect>(SoundPath + variant, AssetRequestMode.ImmediateLoad));
		}
		return asset.Value;
	}

	public float GetRandomPitch()
	{
		return MathHelper.Clamp(Pitch + (Random.NextFloat() - 0.5f) * PitchVariance, -1f, 1f);
	}

	internal SoundStyle WithVolume(float volume)
	{
		return this with
		{
			Volume = volume
		};
	}

	internal SoundStyle WithPitchVariance(float pitchVariance)
	{
		return this with
		{
			PitchVariance = pitchVariance
		};
	}

	public SoundStyle WithVolumeScale(float scale)
	{
		return this with
		{
			Volume = Volume * scale
		};
	}

	public SoundStyle WithPitchOffset(float offset)
	{
		return this with
		{
			Pitch = Pitch + offset
		};
	}

	private int GetRandomVariantIndex()
	{
		if (variantsWeights == null)
		{
			return Random.Next(variants.Length);
		}
		float valueOrDefault = totalVariantWeight.GetValueOrDefault();
		if (!totalVariantWeight.HasValue)
		{
			valueOrDefault = variantsWeights.Sum();
			totalVariantWeight = valueOrDefault;
		}
		float random = (float)Random.NextDouble() * totalVariantWeight.Value;
		float accumulatedWeight = 0f;
		for (int i = 0; i < variantsWeights.Length; i++)
		{
			accumulatedWeight += variantsWeights[i];
			if (random < accumulatedWeight)
			{
				return i;
			}
		}
		return 0;
	}

	private static int[] CreateVariants(int start, int count)
	{
		if (count <= 1)
		{
			return Array.Empty<int>();
		}
		int[] result = new int[count];
		for (int i = 0; i < count; i++)
		{
			result[i] = start + i;
		}
		return result;
	}

	[CompilerGenerated]
	private bool PrintMembers(StringBuilder builder)
	{
		builder.Append("SoundPath = ");
		builder.Append((object?)SoundPath);
		builder.Append(", Type = ");
		builder.Append(Type.ToString());
		builder.Append(", Identifier = ");
		builder.Append((object?)Identifier);
		builder.Append(", MaxInstances = ");
		builder.Append(MaxInstances.ToString());
		builder.Append(", SoundLimitBehavior = ");
		builder.Append(SoundLimitBehavior.ToString());
		builder.Append(", PlayOnlyIfFocused = ");
		builder.Append(PlayOnlyIfFocused.ToString());
		builder.Append(", IsLooped = ");
		builder.Append(IsLooped.ToString());
		builder.Append(", Variants = ");
		builder.Append(Variants.ToString());
		builder.Append(", VariantsWeights = ");
		builder.Append(VariantsWeights.ToString());
		builder.Append(", Volume = ");
		builder.Append(Volume.ToString());
		builder.Append(", Pitch = ");
		builder.Append(Pitch.ToString());
		builder.Append(", PitchVariance = ");
		builder.Append(PitchVariance.ToString());
		builder.Append(", PitchRange = ");
		builder.Append(PitchRange.ToString());
		return true;
	}

	[CompilerGenerated]
	public override readonly int GetHashCode()
	{
		return ((((((((((((((EqualityComparer<int[]>.Default.GetHashCode(variants) * -1521134295 + EqualityComparer<float[]>.Default.GetHashCode(variantsWeights)) * -1521134295 + EqualityComparer<float?>.Default.GetHashCode(totalVariantWeight)) * -1521134295 + EqualityComparer<float>.Default.GetHashCode(volume)) * -1521134295 + EqualityComparer<float>.Default.GetHashCode(pitch)) * -1521134295 + EqualityComparer<float>.Default.GetHashCode(pitchVariance)) * -1521134295 + EqualityComparer<Asset<SoundEffect>>.Default.GetHashCode(effectCache)) * -1521134295 + EqualityComparer<Asset<SoundEffect>[]>.Default.GetHashCode(variantsEffectCache)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(SoundPath)) * -1521134295 + EqualityComparer<SoundType>.Default.GetHashCode(Type)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Identifier)) * -1521134295 + EqualityComparer<int>.Default.GetHashCode(MaxInstances)) * -1521134295 + EqualityComparer<SoundLimitBehavior>.Default.GetHashCode(SoundLimitBehavior)) * -1521134295 + EqualityComparer<bool>.Default.GetHashCode(PlayOnlyIfFocused)) * -1521134295 + EqualityComparer<bool>.Default.GetHashCode(IsLooped)) * -1521134295 + EqualityComparer<bool>.Default.GetHashCode(UsesMusicPitch);
	}

	[CompilerGenerated]
	public readonly bool Equals(SoundStyle other)
	{
		if (EqualityComparer<int[]>.Default.Equals(variants, other.variants) && EqualityComparer<float[]>.Default.Equals(variantsWeights, other.variantsWeights) && EqualityComparer<float?>.Default.Equals(totalVariantWeight, other.totalVariantWeight) && EqualityComparer<float>.Default.Equals(volume, other.volume) && EqualityComparer<float>.Default.Equals(pitch, other.pitch) && EqualityComparer<float>.Default.Equals(pitchVariance, other.pitchVariance) && EqualityComparer<Asset<SoundEffect>>.Default.Equals(effectCache, other.effectCache) && EqualityComparer<Asset<SoundEffect>[]>.Default.Equals(variantsEffectCache, other.variantsEffectCache) && EqualityComparer<string>.Default.Equals(SoundPath, other.SoundPath) && EqualityComparer<SoundType>.Default.Equals(Type, other.Type) && EqualityComparer<string>.Default.Equals(Identifier, other.Identifier) && EqualityComparer<int>.Default.Equals(MaxInstances, other.MaxInstances) && EqualityComparer<SoundLimitBehavior>.Default.Equals(SoundLimitBehavior, other.SoundLimitBehavior) && EqualityComparer<bool>.Default.Equals(PlayOnlyIfFocused, other.PlayOnlyIfFocused) && EqualityComparer<bool>.Default.Equals(IsLooped, other.IsLooped))
		{
			return EqualityComparer<bool>.Default.Equals(UsesMusicPitch, other.UsesMusicPitch);
		}
		return false;
	}
}
