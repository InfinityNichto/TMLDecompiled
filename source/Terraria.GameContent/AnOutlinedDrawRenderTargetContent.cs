using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria.GameContent;

public abstract class AnOutlinedDrawRenderTargetContent : ARenderTargetContentByRequest
{
	protected int width = 84;

	protected int height = 84;

	public Color _borderColor = Color.White;

	private EffectPass _coloringShader;

	private RenderTarget2D _helperTarget;

	public void UseColor(Color color)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		_borderColor = color;
	}

	protected override void HandleUseReqest(GraphicsDevice device, SpriteBatch spriteBatch)
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_01af: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		Effect pixelShader = Main.pixelShader;
		if (_coloringShader == null)
		{
			_coloringShader = pixelShader.CurrentTechnique.Passes["ColorOnly"];
		}
		new Rectangle(0, 0, width, height);
		PrepareARenderTarget_AndListenToEvents(ref _target, device, width, height, (RenderTargetUsage)1);
		PrepareARenderTarget_WithoutListeningToEvents(ref _helperTarget, device, width, height, (RenderTargetUsage)0);
		device.SetRenderTarget(_helperTarget);
		device.Clear(Color.Transparent);
		spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, (Effect)null);
		DrawTheContent(spriteBatch);
		spriteBatch.End();
		device.SetRenderTarget(_target);
		device.Clear(Color.Transparent);
		spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, (Effect)null);
		_coloringShader.Apply();
		int num = 2;
		int num2 = num * 2;
		for (int i = -num2; i <= num2; i += num)
		{
			for (int j = -num2; j <= num2; j += num)
			{
				if (Math.Abs(i) + Math.Abs(j) == num2)
				{
					spriteBatch.Draw((Texture2D)(object)_helperTarget, new Vector2((float)i, (float)j), Color.Black);
				}
			}
		}
		num2 = num;
		for (int k = -num2; k <= num2; k += num)
		{
			for (int l = -num2; l <= num2; l += num)
			{
				if (Math.Abs(k) + Math.Abs(l) == num2)
				{
					spriteBatch.Draw((Texture2D)(object)_helperTarget, new Vector2((float)k, (float)l), _borderColor);
				}
			}
		}
		pixelShader.CurrentTechnique.Passes[0].Apply();
		spriteBatch.Draw((Texture2D)(object)_helperTarget, Vector2.Zero, Color.White);
		spriteBatch.End();
		device.SetRenderTarget((RenderTarget2D)null);
		_wasPrepared = true;
	}

	internal abstract void DrawTheContent(SpriteBatch spriteBatch);
}
