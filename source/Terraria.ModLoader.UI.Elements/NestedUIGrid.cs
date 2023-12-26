using Terraria.UI;

namespace Terraria.ModLoader.UI.Elements;

internal class NestedUIGrid : UIGrid
{
	public override void ScrollWheel(UIScrollWheelEvent evt)
	{
		if (_scrollbar != null)
		{
			float viewPosition = _scrollbar.ViewPosition;
			_scrollbar.ViewPosition -= evt.ScrollWheelValue;
			if (viewPosition == _scrollbar.ViewPosition)
			{
				base.ScrollWheel(evt);
			}
		}
		else
		{
			base.ScrollWheel(evt);
		}
	}
}
