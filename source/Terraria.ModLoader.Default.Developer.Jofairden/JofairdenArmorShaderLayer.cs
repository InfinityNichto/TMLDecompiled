using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;

namespace Terraria.ModLoader.Default.Developer.Jofairden;

internal abstract class JofairdenArmorShaderLayer : JofairdenArmorDrawLayer
{
	public const int ShaderNumSegments = 8;

	public const int ShaderDrawOffset = 2;

	protected override void Draw(ref PlayerDrawSet drawInfo)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		DrawDataInfo drawDataInfo = GetData(drawInfo);
		Player drawPlayer = drawInfo.drawPlayer;
		JofairdenArmorEffectPlayer modPlayer = drawPlayer.GetModPlayer<JofairdenArmorEffectPlayer>();
		SpriteEffects effects = (SpriteEffects)0;
		if (drawPlayer.direction == -1)
		{
			effects = (SpriteEffects)(effects | 1);
		}
		if (drawPlayer.gravDir == -1f)
		{
			effects = (SpriteEffects)(effects | 2);
		}
		DrawData data = new DrawData(drawDataInfo.Texture, drawDataInfo.Position, drawDataInfo.Frame, Color.White * Main.essScale * modPlayer.LayerStrength * modPlayer.ShaderStrength, drawDataInfo.Rotation, drawDataInfo.Origin, 1f, effects);
		BeginShaderBatch(Main.spriteBatch);
		int valueOrDefault = JofairdenArmorDrawLayer.ShaderId.GetValueOrDefault();
		if (!JofairdenArmorDrawLayer.ShaderId.HasValue)
		{
			valueOrDefault = GameShaders.Armor.GetShaderIdFromItemId(2870);
			JofairdenArmorDrawLayer.ShaderId = valueOrDefault;
		}
		GameShaders.Armor.Apply(JofairdenArmorDrawLayer.ShaderId.Value, drawPlayer, data);
		Vector2 centerPos = data.position;
		for (int i = 0; i < 8; i++)
		{
			data.position = centerPos + GetDrawOffset(i);
			data.Draw(Main.spriteBatch);
		}
		data.position = centerPos;
	}

	protected static Vector2 GetDrawOffset(int i)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		return Utils.RotatedBy(new Vector2(0f, 2f), (float)i / 8f * ((float)Math.PI * 2f));
	}

	private static void BeginShaderBatch(SpriteBatch batch)
	{
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		batch.End();
		RasterizerState rasterizerState = ((Main.LocalPlayer.gravDir == 1f) ? RasterizerState.CullCounterClockwise : RasterizerState.CullClockwise);
		batch.Begin((SpriteSortMode)1, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, rasterizerState, (Effect)null, Main.GameViewMatrix.TransformationMatrix);
	}
}
