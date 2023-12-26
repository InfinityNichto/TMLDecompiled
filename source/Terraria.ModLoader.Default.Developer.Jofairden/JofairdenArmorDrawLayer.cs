using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;

namespace Terraria.ModLoader.Default.Developer.Jofairden;

internal abstract class JofairdenArmorDrawLayer : PlayerDrawLayer
{
	protected static int? ShaderId;

	public abstract DrawDataInfo GetData(PlayerDrawSet info);

	public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
	{
		Player player = drawInfo.drawPlayer;
		if (drawInfo.shadow == 0f && !player.invis)
		{
			return player.GetModPlayer<JofairdenArmorEffectPlayer>().LayerStrength > 0f;
		}
		return false;
	}

	public static DrawDataInfo GetHeadDrawDataInfo(PlayerDrawSet drawInfo, Texture2D texture)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		Player drawPlayer = drawInfo.drawPlayer;
		Vector2 pos = drawPlayer.headPosition + drawInfo.headVect + new Vector2((float)(int)(drawInfo.Position.X + (float)drawPlayer.width / 2f - (float)drawPlayer.bodyFrame.Width / 2f - Main.screenPosition.X), (float)(int)(drawInfo.Position.Y + (float)drawPlayer.height - (float)drawPlayer.bodyFrame.Height + 4f - Main.screenPosition.Y));
		return new DrawDataInfo
		{
			Position = pos,
			Frame = drawPlayer.bodyFrame,
			Origin = drawInfo.headVect,
			Rotation = drawPlayer.headRotation,
			Texture = texture
		};
	}

	public static DrawDataInfo GetBodyDrawDataInfo(PlayerDrawSet drawInfo, Texture2D texture)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		Player drawPlayer = drawInfo.drawPlayer;
		Vector2 pos = drawPlayer.bodyPosition + drawInfo.bodyVect + new Vector2((float)(int)(drawInfo.Position.X - Main.screenPosition.X - (float)drawPlayer.bodyFrame.Width / 2f + (float)drawPlayer.width / 2f), (float)(int)(drawInfo.Position.Y - Main.screenPosition.Y + (float)drawPlayer.height - (float)drawPlayer.bodyFrame.Height + 4f));
		return new DrawDataInfo
		{
			Position = pos,
			Frame = drawPlayer.bodyFrame,
			Origin = drawInfo.bodyVect,
			Rotation = drawPlayer.bodyRotation,
			Texture = texture
		};
	}

	public static DrawDataInfo GetLegDrawDataInfo(PlayerDrawSet drawInfo, Texture2D texture)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		Player drawPlayer = drawInfo.drawPlayer;
		Vector2 pos = drawPlayer.legPosition + drawInfo.legVect + new Vector2((float)(int)(drawInfo.Position.X - Main.screenPosition.X - (float)drawPlayer.legFrame.Width / 2f + (float)drawPlayer.width / 2f), (float)(int)(drawInfo.Position.Y - Main.screenPosition.Y + (float)drawPlayer.height - (float)drawPlayer.legFrame.Height + 4f));
		return new DrawDataInfo
		{
			Position = pos,
			Frame = drawPlayer.legFrame,
			Origin = drawInfo.legVect,
			Rotation = drawPlayer.legRotation,
			Texture = texture
		};
	}
}
