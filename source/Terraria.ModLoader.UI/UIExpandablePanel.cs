using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.GameContent.UI.Elements;
using Terraria.Localization;
using Terraria.UI;

namespace Terraria.ModLoader.UI;

internal class UIExpandablePanel : UIPanel
{
	private bool pendingChanges;

	private bool expanded;

	private float defaultHeight = 40f;

	private UIHoverImage expandButton;

	public List<UIElement> VisibleWhenExpanded = new List<UIElement>();

	protected Asset<Texture2D> CollapsedTexture { get; set; } = UICommon.ButtonCollapsedTexture;


	protected Asset<Texture2D> ExpandedTexture { get; set; } = UICommon.ButtonExpandedTexture;


	public event Action OnExpanded;

	public event Action OnCollapsed;

	public UIExpandablePanel()
	{
		Width.Set(0f, 1f);
		Height.Set(defaultHeight, 0f);
		SetPadding(6f);
		expandButton = new UIHoverImage(CollapsedTexture, Language.GetTextValue("tModLoader.ModConfigExpand"));
		expandButton.Top.Set(3f, 0f);
		expandButton.Left.Set(-25f, 1f);
		expandButton.OnLeftClick += delegate
		{
			expanded = !expanded;
			pendingChanges = true;
		};
		Append(expandButton);
	}

	public override void Update(GameTime gameTime)
	{
		base.Update(gameTime);
		if (!pendingChanges)
		{
			return;
		}
		pendingChanges = false;
		float newHeight = defaultHeight;
		if (expanded)
		{
			foreach (UIElement item in VisibleWhenExpanded)
			{
				Append(item);
				CalculatedStyle innerDimensions = item.GetInnerDimensions();
				if (innerDimensions.Height > newHeight)
				{
					newHeight = 30f + innerDimensions.Height + PaddingBottom + PaddingTop;
				}
			}
			expandButton.HoverText = Language.GetTextValue("tModLoader.ModConfigCollapse");
			expandButton.SetImage(ExpandedTexture);
			this.OnExpanded?.Invoke();
		}
		else
		{
			foreach (UIElement item2 in VisibleWhenExpanded)
			{
				RemoveChild(item2);
			}
			this.OnCollapsed?.Invoke();
			expandButton.HoverText = Language.GetTextValue("tModLoader.ModConfigExpand");
			expandButton.SetImage(CollapsedTexture);
		}
		Height.Set(newHeight, 0f);
	}
}
