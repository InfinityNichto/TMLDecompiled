using Microsoft.Xna.Framework.Graphics;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements;

public class FixedUIScrollbar : UIScrollbar
{
	private UserInterface userInterface;

	public FixedUIScrollbar(UserInterface userInterface)
	{
		this.userInterface = userInterface;
	}

	protected override void DrawSelf(SpriteBatch spriteBatch)
	{
		UserInterface activeInstance = UserInterface.ActiveInstance;
		UserInterface.ActiveInstance = userInterface;
		base.DrawSelf(spriteBatch);
		UserInterface.ActiveInstance = activeInstance;
	}

	public override void LeftMouseDown(UIMouseEvent evt)
	{
		UserInterface activeInstance = UserInterface.ActiveInstance;
		UserInterface.ActiveInstance = userInterface;
		base.LeftMouseDown(evt);
		UserInterface.ActiveInstance = activeInstance;
	}
}
