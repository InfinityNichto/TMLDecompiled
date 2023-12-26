using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.GameContent.UI.Elements;
using Terraria.GameContent.UI.States;
using Terraria.Localization;
using Terraria.ModLoader.UI;
using Terraria.ModLoader.UI.Elements;
using Terraria.UI;

namespace Terraria.ModLoader.Config.UI;

internal abstract class DefinitionElement<T> : ConfigElement<T> where T : EntityDefinition
{
	protected bool UpdateNeeded { get; set; }

	protected bool SelectionExpanded { get; set; }

	protected UIPanel ChooserPanel { get; set; }

	protected NestedUIGrid ChooserGrid { get; set; }

	protected UIFocusInputTextField ChooserFilter { get; set; }

	protected UIFocusInputTextField ChooserFilterMod { get; set; }

	protected float OptionScale { get; set; } = 0.5f;


	protected List<DefinitionOptionElement<T>> Options { get; set; }

	protected DefinitionOptionElement<T> OptionChoice { get; set; }

	public override void OnBind()
	{
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		base.OnBind();
		base.TextDisplayFunction = () => Label + ": " + OptionChoice.Tooltip;
		if (base.List != null)
		{
			base.TextDisplayFunction = () => base.Index + 1 + ": " + OptionChoice.Tooltip;
		}
		Height.Set(30f, 0f);
		OptionChoice = CreateDefinitionOptionElement();
		OptionChoice.Top.Set(2f, 0f);
		OptionChoice.Left.Set(-30f, 1f);
		OptionChoice.OnLeftClick += delegate
		{
			SelectionExpanded = !SelectionExpanded;
			UpdateNeeded = true;
		};
		TweakDefinitionOptionElement(OptionChoice);
		Append(OptionChoice);
		ChooserPanel = new UIPanel();
		ChooserPanel.Top.Set(30f, 0f);
		ChooserPanel.Height.Set(200f, 0f);
		ChooserPanel.Width.Set(0f, 1f);
		ChooserPanel.BackgroundColor = Color.CornflowerBlue;
		UIPanel textBoxBackgroundA = new UIPanel();
		textBoxBackgroundA.Width.Set(160f, 0f);
		textBoxBackgroundA.Height.Set(30f, 0f);
		textBoxBackgroundA.Top.Set(-6f, 0f);
		textBoxBackgroundA.PaddingTop = 0f;
		textBoxBackgroundA.PaddingBottom = 0f;
		ChooserFilter = new UIFocusInputTextField("Filter by Name");
		ChooserFilter.OnTextChange += delegate
		{
			UpdateNeeded = true;
		};
		ChooserFilter.OnRightClick += delegate
		{
			ChooserFilter.SetText("");
		};
		ChooserFilter.Width = StyleDimension.Fill;
		ChooserFilter.Height.Set(-6f, 1f);
		ChooserFilter.Top.Set(6f, 0f);
		textBoxBackgroundA.Append(ChooserFilter);
		ChooserPanel.Append(textBoxBackgroundA);
		UIPanel textBoxBackgroundB = new UIPanel();
		textBoxBackgroundB.CopyStyle(textBoxBackgroundA);
		textBoxBackgroundB.Left.Set(180f, 0f);
		ChooserFilterMod = new UIFocusInputTextField("Filter by Mod");
		ChooserFilterMod.OnTextChange += delegate
		{
			UpdateNeeded = true;
		};
		ChooserFilterMod.OnRightClick += delegate
		{
			ChooserFilterMod.SetText("");
		};
		ChooserFilterMod.Width = StyleDimension.Fill;
		ChooserFilterMod.Height.Set(-6f, 1f);
		ChooserFilterMod.Top.Set(6f, 0f);
		textBoxBackgroundB.Append(ChooserFilterMod);
		ChooserPanel.Append(textBoxBackgroundB);
		ChooserGrid = new NestedUIGrid();
		ChooserGrid.Top.Set(30f, 0f);
		ChooserGrid.Height.Set(-30f, 1f);
		ChooserGrid.Width.Set(-12f, 1f);
		ChooserPanel.Append(ChooserGrid);
		UIScrollbar scrollbar = new UIScrollbar();
		scrollbar.SetView(100f, 1000f);
		scrollbar.Height.Set(-30f, 1f);
		scrollbar.Top.Set(30f, 0f);
		scrollbar.Left.Pixels += 8f;
		scrollbar.HAlign = 1f;
		ChooserGrid.SetScrollbar(scrollbar);
		ChooserPanel.Append(scrollbar);
		UIModConfigHoverImageSplit upDownButton = new UIModConfigHoverImageSplit(base.UpDownTexture, Language.GetTextValue("LegacyMenu.168"), Language.GetTextValue("LegacyMenu.169"));
		upDownButton.Recalculate();
		upDownButton.Top.Set(-4f, 0f);
		upDownButton.Left.Set(-18f, 1f);
		upDownButton.OnLeftClick += delegate(UIMouseEvent a, UIElement b)
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			Rectangle val = b.GetDimensions().ToRectangle();
			if (a.MousePosition.Y < (float)(val.Y + val.Height / 2))
			{
				OptionScale = Math.Min(1f, OptionScale + 0.1f);
			}
			else
			{
				OptionScale = Math.Max(0.5f, OptionScale - 0.1f);
			}
			foreach (DefinitionOptionElement<T> option in Options)
			{
				option.SetScale(OptionScale);
			}
		};
		ChooserPanel.Append(upDownButton);
	}

	public override void Update(GameTime gameTime)
	{
		base.Update(gameTime);
		if (UpdateNeeded)
		{
			UpdateNeeded = false;
			if (SelectionExpanded && Options == null)
			{
				Options = CreateDefinitionOptionElementList();
			}
			if (!SelectionExpanded)
			{
				ChooserPanel.Remove();
			}
			else
			{
				Append(ChooserPanel);
			}
			float newHeight = (SelectionExpanded ? 240 : 30);
			Height.Set(newHeight, 0f);
			if (base.Parent != null && base.Parent is UISortableElement)
			{
				base.Parent.Height.Pixels = newHeight;
			}
			if (SelectionExpanded)
			{
				List<DefinitionOptionElement<T>> passed = GetPassedOptionElements();
				ChooserGrid.Clear();
				ChooserGrid.AddRange(passed);
			}
			OptionChoice.SetItem(Value);
		}
	}

	protected abstract List<DefinitionOptionElement<T>> GetPassedOptionElements();

	protected abstract List<DefinitionOptionElement<T>> CreateDefinitionOptionElementList();

	protected abstract DefinitionOptionElement<T> CreateDefinitionOptionElement();

	protected virtual void TweakDefinitionOptionElement(DefinitionOptionElement<T> optionElement)
	{
	}
}
