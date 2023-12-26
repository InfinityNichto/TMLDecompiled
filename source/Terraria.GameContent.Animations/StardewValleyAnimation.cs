using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.DataStructures;

namespace Terraria.GameContent.Animations;

public class StardewValleyAnimation
{
	private List<IAnimationSegment> _segments = new List<IAnimationSegment>();

	public StardewValleyAnimation()
	{
		ComposeAnimation();
	}

	private void ComposeAnimation()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_0165: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0204: Unknown result type (might be due to invalid IL or missing references)
		//IL_020d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0212: Unknown result type (might be due to invalid IL or missing references)
		//IL_021b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0220: Unknown result type (might be due to invalid IL or missing references)
		//IL_0229: Unknown result type (might be due to invalid IL or missing references)
		//IL_022e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0237: Unknown result type (might be due to invalid IL or missing references)
		//IL_023c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0245: Unknown result type (might be due to invalid IL or missing references)
		//IL_024a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0253: Unknown result type (might be due to invalid IL or missing references)
		//IL_0258: Unknown result type (might be due to invalid IL or missing references)
		//IL_0277: Unknown result type (might be due to invalid IL or missing references)
		Asset<Texture2D> asset = TextureAssets.Extra[247];
		Rectangle rectangle = asset.Frame();
		DrawData data = new DrawData(asset.Value, Vector2.Zero, rectangle, Color.White, 0f, rectangle.Size() * new Vector2(0.5f, 0.5f), 1f, (SpriteEffects)0);
		int targetTime = 128;
		int num = 60;
		int num2 = 360;
		int duration = 60;
		int num3 = 4;
		Segments.AnimationSegmentWithActions<Segments.LooseSprite> item = new Segments.SpriteSegment(asset, targetTime, data, Vector2.Zero).UseShaderEffect(new Segments.SpriteSegment.MaskedFadeEffect(GetMatrixForAnimation, "StardewValleyFade", 8, num3).WithPanX(new Segments.Panning
		{
			Delay = 128f,
			Duration = num2 - 120 + num - 60,
			AmountOverTime = 0.55f,
			StartAmount = -0.4f
		}).WithPanY(new Segments.Panning
		{
			StartAmount = 0f
		})).Then(new Actions.Sprites.OutCircleScale(Vector2.Zero)).With(new Actions.Sprites.OutCircleScale(Vector2.One, num))
			.Then(new Actions.Sprites.Wait(num2))
			.Then(new Actions.Sprites.OutCircleScale(Vector2.Zero, duration));
		_segments.Add(item);
		Asset<Texture2D> asset2 = TextureAssets.Extra[249];
		Rectangle rectangle2 = asset2.Frame(1, 8);
		DrawData data2 = new DrawData(asset2.Value, Vector2.Zero, rectangle2, Color.White, 0f, rectangle2.Size() * new Vector2(0.5f, 0.5f), 1f, (SpriteEffects)0);
		Segments.AnimationSegmentWithActions<Segments.LooseSprite> item2 = new Segments.SpriteSegment(asset2, targetTime, data2, Vector2.Zero).Then(new Actions.Sprites.OutCircleScale(Vector2.Zero)).With(new Actions.Sprites.OutCircleScale(Vector2.One, num)).With(new Actions.Sprites.SetFrameSequence(100, (Point[])(object)new Point[8]
		{
			new Point(0, 0),
			new Point(0, 1),
			new Point(0, 2),
			new Point(0, 3),
			new Point(0, 4),
			new Point(0, 5),
			new Point(0, 6),
			new Point(0, 7)
		}, num3, 0, 0))
			.Then(new Actions.Sprites.Wait(num2))
			.Then(new Actions.Sprites.OutCircleScale(Vector2.Zero, duration));
		_segments.Add(item2);
	}

	private Matrix GetMatrixForAnimation()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		return Main.Transform;
	}

	public void Draw(SpriteBatch spriteBatch, int timeInAnimation, Vector2 positionInScreen)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		GameAnimationSegment gameAnimationSegment = default(GameAnimationSegment);
		gameAnimationSegment.SpriteBatch = spriteBatch;
		gameAnimationSegment.AnchorPositionOnScreen = positionInScreen;
		gameAnimationSegment.TimeInAnimation = timeInAnimation;
		gameAnimationSegment.DisplayOpacity = 1f;
		GameAnimationSegment info = gameAnimationSegment;
		for (int i = 0; i < _segments.Count; i++)
		{
			_segments[i].Draw(ref info);
		}
	}
}
