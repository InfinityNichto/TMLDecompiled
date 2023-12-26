using Microsoft.Xna.Framework.Graphics;

namespace Terraria.GameContent.UI.BigProgressBar;

public interface IBigProgressBar
{
	bool ValidateAndCollectNecessaryInfo(ref BigProgressBarInfo info);

	void Draw(ref BigProgressBarInfo info, SpriteBatch spriteBatch);
}
