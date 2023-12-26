using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria.DataStructures;

/// <summary>
/// Holds data required for boss bar drawing.
/// </summary>
public struct BossBarDrawParams
{
	/// <summary>
	/// The texture with fixed dimensions (516x348) containing all the necessary parts.
	/// </summary>
	public Texture2D BarTexture;

	/// <summary>
	/// The screen position of the center of the bar.
	/// </summary>
	public Vector2 BarCenter;

	/// <summary>
	/// The displayed icon texture.
	/// </summary>
	public Texture2D IconTexture;

	/// <summary>
	/// The icon textures frame.
	/// </summary>
	public Rectangle IconFrame;

	/// <summary>
	/// The tint of the icon.
	/// </summary>
	public Color IconColor;

	/// <summary>
	/// The current life of the boss
	/// </summary>
	public float Life;

	/// <summary>
	/// The max life of the boss (the amount it spawned with)
	/// </summary>
	public float LifeMax;

	/// <summary>
	/// The current shield of the boss
	/// </summary>
	public float Shield;

	/// <summary>
	/// The max shield of the boss (may be 0 if the boss has no shield)
	/// </summary>
	public float ShieldMax;

	/// <summary>
	/// The scale the icon is drawn with. Defaults to 1f, modify if icon is bigger or smaller than 26x28.
	/// </summary>
	public float IconScale;

	/// <summary>
	/// If the current life (or shield) of the boss should be written on the bar.
	/// </summary>
	public bool ShowText;

	/// <summary>
	/// The text offset from the center (<see cref="F:Terraria.DataStructures.BossBarDrawParams.BarCenter" />)
	/// </summary>
	public Vector2 TextOffset;

	public BossBarDrawParams(Texture2D barTexture, Vector2 barCenter, Texture2D iconTexture, Rectangle iconFrame, Color iconColor, float life, float lifeMax, float shield = 0f, float shieldMax = 0f, float iconScale = 1f, bool showText = true, Vector2 textOffset = default(Vector2))
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		BarTexture = barTexture;
		BarCenter = barCenter;
		IconTexture = iconTexture;
		IconFrame = iconFrame;
		IconColor = iconColor;
		Life = life;
		LifeMax = lifeMax;
		Shield = shield;
		ShieldMax = shieldMax;
		IconScale = iconScale;
		ShowText = showText;
		TextOffset = textOffset;
	}

	public void Deconstruct(out Texture2D barTexture, out Vector2 barCenter, out Texture2D iconTexture, out Rectangle iconFrame, out Color iconColor, out float life, out float lifeMax, out float shield, out float shieldMax, out float iconScale, out bool showText, out Vector2 textOffset)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		barTexture = BarTexture;
		barCenter = BarCenter;
		iconTexture = IconTexture;
		iconFrame = IconFrame;
		iconColor = IconColor;
		life = Life;
		lifeMax = LifeMax;
		shield = Shield;
		shieldMax = ShieldMax;
		iconScale = IconScale;
		showText = ShowText;
		textOffset = TextOffset;
	}
}
