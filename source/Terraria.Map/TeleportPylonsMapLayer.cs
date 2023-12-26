using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Tile_Entities;
using Terraria.GameInput;
using Terraria.ModLoader;
using Terraria.UI;

namespace Terraria.Map;

public class TeleportPylonsMapLayer : IMapLayer
{
	public bool Visible { get; set; } = true;


	public void Draw(ref MapOverlayDrawContext context, ref string text)
	{
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		IReadOnlyList<TeleportPylonInfo> pylons = Main.PylonSystem.Pylons;
		Texture2D value = TextureAssets.Extra[182].Value;
		bool num2 = TeleportPylonsSystem.IsPlayerNearAPylon(Main.LocalPlayer) && (Main.DroneCameraTracker == null || !Main.DroneCameraTracker.IsInUse());
		for (int i = 0; i < pylons.Count; i++)
		{
			TeleportPylonInfo info = pylons[i];
			float num = 1f;
			float scaleIfSelected = num * 2f;
			Color color = Color.White;
			if (!num2)
			{
				color = Color.Gray * 0.5f;
			}
			if (!PylonLoader.PreDrawMapIcon(ref context, ref text, ref info, ref num2, ref color, ref num, ref scaleIfSelected))
			{
				continue;
			}
			ModPylon pylon = info.ModPylon;
			if (pylon != null)
			{
				pylon.DrawMapIcon(ref context, ref text, info, num2, color, num, scaleIfSelected);
			}
			else if (context.Draw(value, info.PositionInTiles.ToVector2() + new Vector2(1.5f, 2f), color, new SpriteFrame(9, 1, (byte)info.TypeOfPylon, 0)
			{
				PaddingY = 0
			}, num, scaleIfSelected, Alignment.Center).IsMouseOver)
			{
				Main.cancelWormHole = true;
				string itemNameValue = Lang.GetItemNameValue(TETeleportationPylon.GetPylonItemTypeFromTileStyle((int)info.TypeOfPylon));
				text = itemNameValue;
				if (Main.mouseLeft && Main.mouseLeftRelease)
				{
					Main.mouseLeftRelease = false;
					Main.mapFullscreen = false;
					PlayerInput.LockGamepadButtons("MouseLeft");
					Main.PylonSystem.RequestTeleportation(info, Main.LocalPlayer);
					SoundEngine.PlaySound(11);
				}
			}
		}
	}
}
