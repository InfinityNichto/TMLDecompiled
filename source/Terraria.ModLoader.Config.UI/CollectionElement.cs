using System;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.GameContent.UI.States;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader.UI;
using Terraria.UI;

namespace Terraria.ModLoader.Config.UI;

internal abstract class CollectionElement : ConfigElement
{
	private UIModConfigHoverImage initializeButton;

	private UIModConfigHoverImage addButton;

	private UIModConfigHoverImage deleteButton;

	private UIModConfigHoverImage expandButton;

	private UIModConfigHoverImageSplit upDownButton;

	private bool expanded = true;

	private bool pendingChanges;

	protected object Data { get; set; }

	protected UIElement DataListElement { get; set; }

	protected NestedUIList DataList { get; set; }

	protected float Scale { get; set; } = 1f;


	protected DefaultListValueAttribute DefaultListValueAttribute { get; set; }

	protected JsonDefaultListValueAttribute JsonDefaultListValueAttribute { get; set; }

	protected virtual bool CanAdd => true;

	public override void OnBind()
	{
		base.OnBind();
		ExpandAttribute expandAttribute = ConfigManager.GetCustomAttributeFromMemberThenMemberType<ExpandAttribute>(base.MemberInfo, base.Item, base.List);
		if (expandAttribute != null)
		{
			expanded = expandAttribute.Expand;
		}
		Data = base.MemberInfo.GetValue(base.Item);
		DefaultListValueAttribute = ConfigManager.GetCustomAttributeFromMemberThenMemberType<DefaultListValueAttribute>(base.MemberInfo, null, null);
		MaxHeight.Set(300f, 0f);
		DataListElement = new UIElement();
		DataListElement.Width.Set(-10f, 1f);
		DataListElement.Left.Set(10f, 0f);
		DataListElement.Height.Set(-30f, 1f);
		DataListElement.Top.Set(30f, 0f);
		if (Data != null && expanded)
		{
			Append(DataListElement);
		}
		DataListElement.OverflowHidden = true;
		DataList = new NestedUIList();
		DataList.Width.Set(-20f, 1f);
		DataList.Left.Set(0f, 0f);
		DataList.Height.Set(0f, 1f);
		DataList.ListPadding = 5f;
		DataListElement.Append(DataList);
		UIScrollbar scrollbar = new UIScrollbar();
		scrollbar.SetView(100f, 1000f);
		scrollbar.Height.Set(-16f, 1f);
		scrollbar.Top.Set(6f, 0f);
		scrollbar.Left.Pixels -= 3f;
		scrollbar.HAlign = 1f;
		DataList.SetScrollbar(scrollbar);
		DataListElement.Append(scrollbar);
		PrepareTypes();
		SetupList();
		if (CanAdd)
		{
			initializeButton = new UIModConfigHoverImage(base.PlayTexture, Language.GetTextValue("tModLoader.ModConfigInitialize"));
			initializeButton.Top.Pixels += 4f;
			initializeButton.Left.Pixels -= 3f;
			initializeButton.HAlign = 1f;
			initializeButton.OnLeftClick += delegate
			{
				SoundEngine.PlaySound(in SoundID.Tink);
				InitializeCollection();
				SetupList();
				Interface.modConfig.RecalculateChildren();
				Interface.modConfig.SetPendingChanges();
				expanded = true;
				pendingChanges = true;
			};
			addButton = new UIModConfigHoverImage(base.PlusTexture, Language.GetTextValue("tModLoader.ModConfigAdd"));
			addButton.Top.Set(4f, 0f);
			addButton.Left.Set(-52f, 1f);
			addButton.OnLeftClick += delegate
			{
				SoundEngine.PlaySound(in SoundID.Tink);
				AddItem();
				SetupList();
				Interface.modConfig.RecalculateChildren();
				Interface.modConfig.SetPendingChanges();
				expanded = true;
				pendingChanges = true;
			};
			deleteButton = new UIModConfigHoverImage(base.DeleteTexture, Language.GetTextValue("tModLoader.ModConfigClear"));
			deleteButton.Top.Set(4f, 0f);
			deleteButton.Left.Set(-25f, 1f);
			deleteButton.OnLeftClick += delegate
			{
				SoundEngine.PlaySound(in SoundID.Tink);
				if (base.NullAllowed)
				{
					NullCollection();
				}
				else
				{
					ClearCollection();
				}
				SetupList();
				Interface.modConfig.RecalculateChildren();
				Interface.modConfig.SetPendingChanges();
				pendingChanges = true;
			};
		}
		expandButton = new UIModConfigHoverImage(expanded ? base.ExpandedTexture : base.CollapsedTexture, expanded ? Language.GetTextValue("tModLoader.ModConfigCollapse") : Language.GetTextValue("tModLoader.ModConfigExpand"));
		expandButton.Top.Set(4f, 0f);
		expandButton.Left.Set(-79f, 1f);
		expandButton.OnLeftClick += delegate
		{
			expanded = !expanded;
			pendingChanges = true;
		};
		upDownButton = new UIModConfigHoverImageSplit(base.UpDownTexture, Language.GetTextValue("tModLoader.ModConfigScaleUp"), Language.GetTextValue("tModLoader.ModConfigScaleDown"));
		upDownButton.Top.Set(4f, 0f);
		upDownButton.Left.Set(-106f, 1f);
		upDownButton.OnLeftClick += delegate(UIMouseEvent a, UIElement b)
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			Rectangle val = b.GetDimensions().ToRectangle();
			if (a.MousePosition.Y < (float)(val.Y + val.Height / 2))
			{
				Scale = Math.Min(2f, Scale + 0.5f);
			}
			else
			{
				Scale = Math.Max(1f, Scale - 0.5f);
			}
		};
		pendingChanges = true;
		Recalculate();
	}

	protected object CreateCollectionElementInstance(Type type)
	{
		object toAdd;
		if (DefaultListValueAttribute != null)
		{
			toAdd = DefaultListValueAttribute.Value;
		}
		else
		{
			toAdd = ConfigManager.AlternateCreateInstance(type);
			if (!type.IsValueType && type != typeof(string))
			{
				JsonConvert.PopulateObject(JsonDefaultListValueAttribute?.Json ?? "{}", toAdd, ConfigManager.serializerSettings);
			}
		}
		return toAdd;
	}

	protected abstract void PrepareTypes();

	protected abstract void AddItem();

	protected abstract void InitializeCollection();

	protected virtual void NullCollection()
	{
		Data = null;
		SetObject(Data);
	}

	protected abstract void ClearCollection();

	protected abstract void SetupList();

	public override void Update(GameTime gameTime)
	{
		base.Update(gameTime);
		if (!pendingChanges)
		{
			return;
		}
		pendingChanges = false;
		if (CanAdd)
		{
			RemoveChild(initializeButton);
			RemoveChild(addButton);
			RemoveChild(deleteButton);
		}
		RemoveChild(expandButton);
		RemoveChild(upDownButton);
		RemoveChild(DataListElement);
		if (Data == null)
		{
			Append(initializeButton);
			return;
		}
		if (CanAdd)
		{
			Append(addButton);
			Append(deleteButton);
		}
		Append(expandButton);
		if (expanded)
		{
			Append(upDownButton);
			Append(DataListElement);
			expandButton.HoverText = Language.GetTextValue("tModLoader.ModConfigCollapse");
			expandButton.SetImage(base.ExpandedTexture);
		}
		else
		{
			expandButton.HoverText = Language.GetTextValue("tModLoader.ModConfigExpand");
			expandButton.SetImage(base.CollapsedTexture);
		}
	}

	public override void Recalculate()
	{
		base.Recalculate();
		float defaultHeight = 30f;
		float h = ((DataListElement.Parent != null) ? (DataList.GetTotalHeight() + defaultHeight) : defaultHeight);
		h = Utils.Clamp(h, 30f, 300f * Scale);
		MaxHeight.Set(300f * Scale, 0f);
		Height.Set(h, 0f);
		if (base.Parent != null && base.Parent is UISortableElement)
		{
			base.Parent.Height.Set(h, 0f);
		}
	}
}
