using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Audio;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements;

public class EmoteButton : UIElement
{
	private Asset<Texture2D> _bubbleTexture;

	private Asset<Texture2D> _texture;

	private Asset<Texture2D> _textureBorder;

	private int _emoteIndex;

	private bool _hovered;

	private int _frameCounter;

	public ref int FrameCounter => ref _frameCounter;

	public bool Hovered => _hovered;

	public Asset<Texture2D> BubbleTexture => _bubbleTexture;

	public Asset<Texture2D> EmoteTexture => _texture;

	public Asset<Texture2D> BorderTexture => _textureBorder;

	public EmoteButton(int emoteIndex)
	{
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		if (emoteIndex >= EmoteID.Count)
		{
			_texture = ModContent.Request<Texture2D>(EmoteBubbleLoader.GetEmoteBubble(emoteIndex).Texture);
		}
		else
		{
			_texture = Main.Assets.Request<Texture2D>("Images/Extra_" + (short)48);
		}
		_bubbleTexture = Main.Assets.Request<Texture2D>("Images/Extra_" + (short)48);
		_textureBorder = Main.Assets.Request<Texture2D>("Images/UI/EmoteBubbleBorder");
		_emoteIndex = emoteIndex;
		Rectangle frame = GetFrame();
		Width.Set(frame.Width, 0f);
		Height.Set(frame.Height, 0f);
	}

	private Rectangle GetFrame()
	{
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		int num = ((_frameCounter >= 10) ? 1 : 0);
		if (_emoteIndex >= EmoteID.Count)
		{
			return (Rectangle)(((_003F?)EmoteBubbleLoader.GetFrameInEmoteMenu(_emoteIndex, num, _frameCounter)) ?? _texture.Frame(2, 1, num));
		}
		return _texture.Frame(8, EmoteBubble.EMOTE_SHEET_VERTICAL_FRAMES, _emoteIndex % 4 * 2 + num, _emoteIndex / 4 + 1);
	}

	private void UpdateFrame()
	{
		if ((_emoteIndex < EmoteID.Count || EmoteBubbleLoader.UpdateFrameInEmoteMenu(_emoteIndex, ref _frameCounter)) && ++_frameCounter >= 20)
		{
			_frameCounter = 0;
		}
	}

	public override void Update(GameTime gameTime)
	{
		UpdateFrame();
		base.Update(gameTime);
	}

	protected override void DrawSelf(SpriteBatch spriteBatch)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		CalculatedStyle dimensions = GetDimensions();
		Vector2 vector = dimensions.Position() + new Vector2(dimensions.Width, dimensions.Height) / 2f;
		Rectangle frame = GetFrame();
		Rectangle value = _bubbleTexture.Frame(8, 39, 1);
		Vector2 origin = frame.Size() / 2f;
		Color white = Color.White;
		Color color = Color.Black;
		if (_hovered)
		{
			color = Main.OurFavoriteColor;
		}
		if (EmoteBubbleLoader.PreDrawInEmoteMenu(_emoteIndex, spriteBatch, this, vector, frame, origin))
		{
			spriteBatch.Draw(_bubbleTexture.Value, vector, (Rectangle?)value, white, 0f, origin, 1f, (SpriteEffects)0, 0f);
			spriteBatch.Draw(_texture.Value, vector, (Rectangle?)frame, white, 0f, origin, 1f, (SpriteEffects)0, 0f);
			spriteBatch.Draw(_textureBorder.Value, vector - Vector2.One * 2f, (Rectangle?)null, color, 0f, origin, 1f, (SpriteEffects)0, 0f);
		}
		EmoteBubbleLoader.PostDrawInEmoteMenu(_emoteIndex, spriteBatch, this, vector, frame, origin);
		if (_hovered)
		{
			if (_emoteIndex >= EmoteID.Count)
			{
				Main.instance.MouseText("/" + Lang.GetEmojiName(_emoteIndex), 0, 0);
				return;
			}
			string name = EmoteID.Search.GetName(_emoteIndex);
			string cursorText = "/" + Language.GetTextValue("EmojiName." + name);
			Main.instance.MouseText(cursorText, 0, 0);
		}
	}

	public override void MouseOver(UIMouseEvent evt)
	{
		base.MouseOver(evt);
		SoundEngine.PlaySound(12);
		_hovered = true;
	}

	public override void MouseOut(UIMouseEvent evt)
	{
		base.MouseOut(evt);
		_hovered = false;
	}

	public override void LeftClick(UIMouseEvent evt)
	{
		base.LeftClick(evt);
		EmoteBubble.MakeLocalPlayerEmote(_emoteIndex);
		IngameFancyUI.Close();
	}
}
