using Microsoft.Xna.Framework;
using Terraria.Graphics.Shaders;

namespace Terraria.GameContent.Shaders;

public class BlizzardShaderData : ScreenShaderData
{
	private Vector2 _texturePosition = Vector2.Zero;

	private float windSpeed = 0.1f;

	public BlizzardShaderData(string passName)
		: base(passName)
	{
	}//IL_0001: Unknown result type (might be due to invalid IL or missing references)
	//IL_0006: Unknown result type (might be due to invalid IL or missing references)


	public override void Update(GameTime gameTime)
	{
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		float num = Main.windSpeedCurrent;
		if (num >= 0f && num <= 0.1f)
		{
			num = 0.1f;
		}
		else if (num <= 0f && num >= -0.1f)
		{
			num = -0.1f;
		}
		windSpeed = num * 0.05f + windSpeed * 0.95f;
		Vector2 vector = new Vector2(0f - windSpeed, -1f) * new Vector2(10f, 2f);
		((Vector2)(ref vector)).Normalize();
		vector *= new Vector2(0.8f, 0.6f);
		if (!Main.gamePaused && Main.hasFocus)
		{
			_texturePosition += vector * (float)gameTime.ElapsedGameTime.TotalSeconds;
		}
		_texturePosition.X %= 10f;
		_texturePosition.Y %= 10f;
		UseDirection(vector);
		UseTargetPosition(_texturePosition);
		base.Update(gameTime);
	}

	public override void Apply()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		UseTargetPosition(_texturePosition);
		base.Apply();
	}
}
