using Microsoft.Xna.Framework;
using Terraria.GameContent;
using Terraria.Localization;
using Terraria.UI;

namespace Terraria.Map;

public class SpawnMapLayer : IMapLayer
{
	public bool Visible { get; set; } = true;


	public void Draw(ref MapOverlayDrawContext context, ref string text)
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		Player localPlayer = Main.LocalPlayer;
		Vector2 position = default(Vector2);
		((Vector2)(ref position))._002Ector((float)localPlayer.SpawnX, (float)localPlayer.SpawnY);
		Vector2 position2 = default(Vector2);
		((Vector2)(ref position2))._002Ector((float)Main.spawnTileX, (float)Main.spawnTileY);
		if (context.Draw(TextureAssets.SpawnPoint.Value, position2, Alignment.Bottom).IsMouseOver)
		{
			text = Language.GetTextValue("UI.SpawnPoint");
		}
		if (localPlayer.SpawnX != -1 && context.Draw(TextureAssets.SpawnBed.Value, position, Alignment.Bottom).IsMouseOver)
		{
			text = Language.GetTextValue("UI.SpawnBed");
		}
	}
}
