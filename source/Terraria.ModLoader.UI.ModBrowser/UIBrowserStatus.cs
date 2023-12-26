using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;

namespace Terraria.ModLoader.UI.ModBrowser;

public class UIBrowserStatus : UIAnimatedImage
{
	private static Asset<Texture2D> Texture => UICommon.ModBrowserIconsTexture;

	public UIBrowserStatus()
		: base(Texture, 32, 32, 204, 0, 1, 6)
	{
		SetCurrentState(AsyncProviderState.Completed);
	}

	public void SetCurrentState(AsyncProviderState state)
	{
		switch (state)
		{
		case AsyncProviderState.Loading:
			base.FrameStart = 0;
			base.FrameCount = 4;
			break;
		case AsyncProviderState.Canceled:
		case AsyncProviderState.Aborted:
			base.FrameStart = 4;
			base.FrameCount = 1;
			break;
		case AsyncProviderState.Completed:
			base.FrameStart = 5;
			base.FrameCount = 1;
			break;
		}
	}
}
