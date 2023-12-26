using Microsoft.Xna.Framework;
using ReLogic.Graphics;
using Terraria.GameContent;

namespace Terraria.ModLoader;

/// <summary>
/// This class serves as a way to store information about a line that will be drawn of tooltip for an item. You will create and manipulate objects of this class if you use the draw hooks for tooltips in ModItem and GlobalItem. For examples, see ExampleSword
/// </summary>
public sealed class DrawableTooltipLine : TooltipLine
{
	private int _originalX;

	private int _originalY;

	/// <summary>
	/// The text of this tooltip.
	/// </summary>
	public new readonly string Text;

	/// <summary>
	/// The index of the tooltip in the array
	/// </summary>
	public readonly int Index;

	/// <summary>
	/// Whether or not this tooltip gives prefix information. This will make it so that the tooltip is colored either green or red.
	/// </summary>
	public new readonly bool IsModifier;

	/// <summary>
	/// If isModifier is true, this determines whether the tooltip is colored green or red.
	/// </summary>
	public new readonly bool IsModifierBad;

	/// <summary>
	/// The X position where the tooltip would be drawn.
	/// </summary>
	public int X;

	/// <summary>
	/// The Y position where the tooltip would be drawn.
	/// </summary>
	public int Y;

	/// <summary>
	/// Whether the tooltip is a One Drop logo or not. If it is, the tooltip text will be empty.
	/// </summary>
	public new readonly bool OneDropLogo;

	/// <summary>
	/// The font this tooltip would be drawn with
	/// </summary>
	public DynamicSpriteFont Font = FontAssets.MouseText.Value;

	/// <summary>
	/// The rotation this tooltip would be drawn in
	/// </summary>
	public float Rotation;

	/// <summary>
	/// The origin of this tooltip
	/// </summary>
	public Vector2 Origin = Vector2.Zero;

	/// <summary>
	/// The baseScale of this tooltip. When drawing the One Drop logo the scale is calculated by (baseScale.X + baseScale.Y) / 2
	/// </summary>
	public Vector2 BaseScale = Vector2.One;

	public float MaxWidth = -1f;

	public float Spread = 2f;

	/// <summary>
	/// The X position where the tooltip would be drawn that is not adjusted by mods.
	/// </summary>
	public int OriginalX
	{
		get
		{
			return _originalX;
		}
		internal set
		{
			X = (_originalX = value);
		}
	}

	/// <summary>
	/// The Y position where the tooltip would be drawn that is not adjusted by mods.
	/// </summary>
	public int OriginalY
	{
		get
		{
			return _originalY;
		}
		internal set
		{
			Y = (_originalY = value);
		}
	}

	/// <summary>
	/// The color the tooltip would be drawn in
	/// </summary>
	public Color Color { get; internal set; }

	/// <summary>
	/// If the tooltip line's color was overridden this will hold that color, it will be null otherwise
	/// </summary>
	public new Color? OverrideColor { get; internal set; }

	/// <summary>
	/// Creates a new DrawableTooltipLine object
	/// </summary>
	/// <param name="parent">The TooltipLine to make this DrawableTooltipLine from</param>
	/// <param name="index">The index of the line in the array</param>
	/// <param name="x">The X position where the tooltip would be drawn.</param>
	/// <param name="y">The Y position where the tooltip would be drawn.</param>
	/// <param name="color">The color the tooltip would be drawn in</param>
	public DrawableTooltipLine(TooltipLine parent, int index, int x, int y, Color color)
		: base(parent.Mod, parent.Name, parent.Text)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		IsModifier = parent.IsModifier;
		IsModifierBad = parent.IsModifierBad;
		OverrideColor = parent.OverrideColor;
		OneDropLogo = parent.OneDropLogo;
		Text = parent.Text;
		Index = index;
		OriginalX = x;
		OriginalY = y;
		Color = color;
	}
}
