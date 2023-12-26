using Microsoft.Xna.Framework;
using Terraria.Graphics.Shaders;

namespace Terraria.GameContent.Shaders;

public class SandstormShaderData : ScreenShaderData
{
	private Vector2 _texturePosition = Vector2.Zero;

	public SandstormShaderData(string passName)
		: base(passName)
	{
	}//IL_0001: Unknown result type (might be due to invalid IL or missing references)
	//IL_0006: Unknown result type (might be due to invalid IL or missing references)


	public override void Update(GameTime gameTime)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		Vector2 vector = new Vector2(0f - Main.windSpeedCurrent, -1f) * new Vector2(20f, 0.1f);
		((Vector2)(ref vector)).Normalize();
		vector *= new Vector2(2f, 0.2f);
		if (!Main.gamePaused && Main.hasFocus)
		{
			_texturePosition += vector * (float)gameTime.ElapsedGameTime.TotalSeconds;
		}
		_texturePosition.X %= 10f;
		_texturePosition.Y %= 10f;
		UseDirection(vector);
		base.Update(gameTime);
	}

	public override void Apply()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		UseTargetPosition(_texturePosition);
		base.Apply();
	}
}
