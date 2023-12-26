using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Audio;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements;

public class UIHairStyleButton : UIImageButton
{
	private readonly Player _player;

	public readonly int HairStyleId;

	private readonly Asset<Texture2D> _selectedBorderTexture;

	private readonly Asset<Texture2D> _hoveredBorderTexture;

	private bool _hovered;

	private bool _soundedHover;

	private int _framesToSkip;

	public UIHairStyleButton(Player player, int hairStyleId)
		: base(Main.Assets.Request<Texture2D>("Images/UI/CharCreation/CategoryPanel"))
	{
		_player = player;
		HairStyleId = hairStyleId;
		Width = StyleDimension.FromPixels(44f);
		Height = StyleDimension.FromPixels(44f);
		_selectedBorderTexture = Main.Assets.Request<Texture2D>("Images/UI/CharCreation/CategoryPanelHighlight");
		_hoveredBorderTexture = Main.Assets.Request<Texture2D>("Images/UI/CharCreation/CategoryPanelBorder");
		UseImmediateMode = true;
	}

	public void SkipRenderingContent(int timeInFrames)
	{
		_framesToSkip = timeInFrames;
	}

	protected override void DrawSelf(SpriteBatch spriteBatch)
	{
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		if (_hovered)
		{
			if (!_soundedHover)
			{
				SoundEngine.PlaySound(12);
			}
			_soundedHover = true;
		}
		else
		{
			_soundedHover = false;
		}
		Vector2 vector = default(Vector2);
		((Vector2)(ref vector))._002Ector(-5f, -5f);
		base.DrawSelf(spriteBatch);
		if (_player.hair == HairStyleId)
		{
			spriteBatch.Draw(_selectedBorderTexture.Value, GetDimensions().Center() - _selectedBorderTexture.Size() / 2f, Color.White);
		}
		if (_hovered)
		{
			spriteBatch.Draw(_hoveredBorderTexture.Value, GetDimensions().Center() - _hoveredBorderTexture.Size() / 2f, Color.White);
		}
		if (_framesToSkip > 0)
		{
			_framesToSkip--;
			return;
		}
		int hair = _player.hair;
		_player.hair = HairStyleId;
		Main.PlayerRenderer.DrawPlayerHead(Main.Camera, _player, GetDimensions().Center() + vector);
		_player.hair = hair;
	}

	public override void LeftMouseDown(UIMouseEvent evt)
	{
		_player.hair = HairStyleId;
		SoundEngine.PlaySound(12);
		base.LeftMouseDown(evt);
	}

	public override void MouseOver(UIMouseEvent evt)
	{
		base.MouseOver(evt);
		_hovered = true;
	}

	public override void MouseOut(UIMouseEvent evt)
	{
		base.MouseOut(evt);
		_hovered = false;
	}
}
