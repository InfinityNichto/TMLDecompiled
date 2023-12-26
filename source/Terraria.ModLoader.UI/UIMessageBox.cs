using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace Terraria.ModLoader.UI;

internal class UIMessageBox : UIPanel
{
	private string _text;

	private UIScrollbar _scrollbar;

	private UIList _list;

	private UIText _textElement;

	public UIMessageBox(string text)
	{
		_text = text;
	}

	public void SetText(string text)
	{
		_text = text;
		_textElement?.SetText(_text);
		ResetScrollbar();
	}

	private void ResetScrollbar()
	{
		if (_scrollbar != null)
		{
			_scrollbar.ViewPosition = 0f;
		}
	}

	public override void OnInitialize()
	{
		UIList obj = new UIList
		{
			Left = StyleDimension.Empty,
			Top = StyleDimension.Empty,
			Width = StyleDimension.Fill,
			Height = StyleDimension.Fill
		};
		UIList element = obj;
		_list = obj;
		Append(element);
		_list.SetScrollbar(_scrollbar);
		UIList list = _list;
		UIText obj2 = new UIText(_text)
		{
			Width = StyleDimension.Fill,
			IsWrapped = true,
			MinWidth = StyleDimension.Empty,
			TextOriginX = 0f
		};
		UIText item = obj2;
		_textElement = obj2;
		list.Add(item);
	}

	public void SetScrollbar(UIScrollbar scrollbar)
	{
		_scrollbar = scrollbar;
	}
}
