using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameInput;

namespace Terraria.UI;

public class GameInterfaceLayer
{
	public readonly string Name;

	public InterfaceScaleType ScaleType;

	public bool Active;

	public GameInterfaceLayer(string name, InterfaceScaleType scaleType)
	{
		Name = name;
		ScaleType = scaleType;
		Active = true;
	}

	public bool Draw()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		if (!Active)
		{
			return true;
		}
		Matrix transformMatrix;
		if (ScaleType == InterfaceScaleType.Game)
		{
			PlayerInput.SetZoom_World();
			transformMatrix = Main.GameViewMatrix.ZoomMatrix;
		}
		else if (ScaleType == InterfaceScaleType.UI)
		{
			PlayerInput.SetZoom_UI();
			transformMatrix = Main.UIScaleMatrix;
		}
		else
		{
			PlayerInput.SetZoom_Unscaled();
			transformMatrix = Matrix.Identity;
		}
		bool result = false;
		Main.spriteBatch.Begin((SpriteSortMode)0, (BlendState)null, (SamplerState)null, (DepthStencilState)null, (RasterizerState)null, (Effect)null, transformMatrix);
		try
		{
			result = DrawSelf();
		}
		catch (Exception e)
		{
			TimeLogger.DrawException(e);
		}
		Main.spriteBatch.End();
		return result;
	}

	protected virtual bool DrawSelf()
	{
		return true;
	}
}
