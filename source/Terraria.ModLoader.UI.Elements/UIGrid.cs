using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Terraria.UI;

namespace Terraria.ModLoader.UI.Elements;

public class UIGrid : UIElement
{
	public delegate bool ElementSearchMethod(UIElement element);

	private class UIInnerList : UIElement
	{
		public override bool ContainsPoint(Vector2 point)
		{
			return true;
		}

		protected override void DrawChildren(SpriteBatch spriteBatch)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			Vector2 position = base.Parent.GetDimensions().Position();
			Vector2 dimensions = default(Vector2);
			((Vector2)(ref dimensions))._002Ector(base.Parent.GetDimensions().Width, base.Parent.GetDimensions().Height);
			Vector2 dimensions2 = default(Vector2);
			foreach (UIElement current in Elements)
			{
				Vector2 position2 = current.GetDimensions().Position();
				((Vector2)(ref dimensions2))._002Ector(current.GetDimensions().Width, current.GetDimensions().Height);
				if (Collision.CheckAABBvAABBCollision(position, dimensions, position2, dimensions2))
				{
					current.Draw(spriteBatch);
				}
			}
		}
	}

	public List<UIElement> _items = new List<UIElement>();

	protected UIScrollbar _scrollbar;

	internal UIElement _innerList = new UIInnerList();

	private float _innerListHeight;

	public float ListPadding = 5f;

	public int Count => _items.Count;

	public UIGrid()
	{
		_innerList.OverflowHidden = false;
		_innerList.Width.Set(0f, 1f);
		_innerList.Height.Set(0f, 1f);
		OverflowHidden = true;
		Append(_innerList);
	}

	public float GetTotalHeight()
	{
		return _innerListHeight;
	}

	public void Goto(ElementSearchMethod searchMethod, bool center = false)
	{
		for (int i = 0; i < _items.Count; i++)
		{
			if (searchMethod(_items[i]))
			{
				_scrollbar.ViewPosition = _items[i].Top.Pixels;
				if (center)
				{
					_scrollbar.ViewPosition = _items[i].Top.Pixels - GetInnerDimensions().Height / 2f + _items[i].GetOuterDimensions().Height / 2f;
				}
				break;
			}
		}
	}

	public virtual void Add(UIElement item)
	{
		_items.Add(item);
		_innerList.Append(item);
		UpdateOrder();
		_innerList.Recalculate();
	}

	public virtual void AddRange(IEnumerable<UIElement> items)
	{
		_items.AddRange(items);
		foreach (UIElement item in items)
		{
			_innerList.Append(item);
		}
		UpdateOrder();
		_innerList.Recalculate();
	}

	public virtual bool Remove(UIElement item)
	{
		_innerList.RemoveChild(item);
		UpdateOrder();
		return _items.Remove(item);
	}

	public virtual void Clear()
	{
		_innerList.RemoveAllChildren();
		_items.Clear();
	}

	public override void Recalculate()
	{
		base.Recalculate();
		UpdateScrollbar();
	}

	public override void MouseOver(UIMouseEvent evt)
	{
		base.MouseOver(evt);
		PlayerInput.LockVanillaMouseScroll("ModLoader/UIGrid");
	}

	public override void ScrollWheel(UIScrollWheelEvent evt)
	{
		base.ScrollWheel(evt);
		if (_scrollbar != null)
		{
			_scrollbar.ViewPosition -= evt.ScrollWheelValue;
		}
	}

	public override void RecalculateChildren()
	{
		float availableWidth = GetInnerDimensions().Width;
		base.RecalculateChildren();
		float top = 0f;
		float left = 0f;
		float maxRowHeight = 0f;
		for (int i = 0; i < _items.Count; i++)
		{
			UIElement uIElement = _items[i];
			CalculatedStyle outerDimensions = uIElement.GetOuterDimensions();
			if (left + outerDimensions.Width > availableWidth && left > 0f)
			{
				top += maxRowHeight + ListPadding;
				left = 0f;
				maxRowHeight = 0f;
			}
			maxRowHeight = Math.Max(maxRowHeight, outerDimensions.Height);
			uIElement.Left.Set(left, 0f);
			left += outerDimensions.Width + ListPadding;
			uIElement.Top.Set(top, 0f);
		}
		_innerListHeight = top + maxRowHeight;
	}

	private void UpdateScrollbar()
	{
		if (_scrollbar != null)
		{
			_scrollbar.SetView(GetInnerDimensions().Height, _innerListHeight);
		}
	}

	public void SetScrollbar(UIScrollbar scrollbar)
	{
		_scrollbar = scrollbar;
		UpdateScrollbar();
	}

	public void UpdateOrder()
	{
		_items.Sort(SortMethod);
		UpdateScrollbar();
	}

	public int SortMethod(UIElement item1, UIElement item2)
	{
		return item1.CompareTo(item2);
	}

	public override List<SnapPoint> GetSnapPoints()
	{
		List<SnapPoint> list = new List<SnapPoint>();
		if (GetSnapPoint(out var item))
		{
			list.Add(item);
		}
		foreach (UIElement current in _items)
		{
			list.AddRange(current.GetSnapPoints());
		}
		return list;
	}

	protected override void DrawSelf(SpriteBatch spriteBatch)
	{
		if (_scrollbar != null)
		{
			_innerList.Top.Set(0f - _scrollbar.GetValue(), 0f);
		}
	}
}
