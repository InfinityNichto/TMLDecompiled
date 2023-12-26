using System;
using System.Reflection;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.GameContent.UI.States;
using Terraria.Localization;
using Terraria.ModLoader.UI;
using Terraria.UI;

namespace Terraria.ModLoader.Config.UI;

internal class ObjectElement : ConfigElement<object>
{
	private readonly bool ignoreSeparatePage;

	private bool separatePage;

	private bool pendingChanges;

	private bool expanded = true;

	private NestedUIList dataList;

	private UIModConfigHoverImage initializeButton;

	private UIModConfigHoverImage deleteButton;

	private UIModConfigHoverImage expandButton;

	private UIPanel separatePagePanel;

	private UITextPanel<FuncStringWrapper> separatePageButton;

	protected Func<string> AbridgedTextDisplayFunction { get; set; }

	public ObjectElement(bool ignoreSeparatePage = false)
	{
		this.ignoreSeparatePage = ignoreSeparatePage;
	}

	public override void OnBind()
	{
		base.OnBind();
		if (base.List != null)
		{
			MethodInfo methodInfo = base.MemberInfo.Type.GetGenericArguments()[0].GetMethod("ToString", Array.Empty<Type>());
			if (methodInfo != null && methodInfo.DeclaringType != typeof(object))
			{
				base.TextDisplayFunction = () => base.Index + 1 + ": " + (base.List[base.Index]?.ToString() ?? "null");
				AbridgedTextDisplayFunction = () => base.List[base.Index]?.ToString() ?? "null";
			}
			else
			{
				base.TextDisplayFunction = () => base.Index + 1 + ": ";
			}
		}
		else if (base.MemberInfo.Type.GetMethod("ToString", Array.Empty<Type>()).DeclaringType != typeof(object))
		{
			base.TextDisplayFunction = () => Label + ((Value == null) ? "" : (": " + Value.ToString()));
			AbridgedTextDisplayFunction = () => Value?.ToString() ?? "";
		}
		if (Value == null && base.List != null)
		{
			object data = Activator.CreateInstance(base.MemberInfo.Type, nonPublic: true);
			JsonConvert.PopulateObject(JsonDefaultValueAttribute?.Json ?? "{}", data, ConfigManager.serializerSettings);
			Value = data;
		}
		separatePage = ConfigManager.GetCustomAttributeFromMemberThenMemberType<SeparatePageAttribute>(base.MemberInfo, base.Item, base.List) != null;
		if (separatePage && !ignoreSeparatePage)
		{
			separatePageButton = new UITextPanel<FuncStringWrapper>(new FuncStringWrapper(base.TextDisplayFunction));
			separatePageButton.HAlign = 0.5f;
			separatePageButton.OnLeftClick += delegate
			{
				UIModConfig.SwitchToSubConfig(separatePagePanel);
			};
		}
		if (base.List == null)
		{
			ExpandAttribute expandAttribute = ConfigManager.GetCustomAttributeFromMemberThenMemberType<ExpandAttribute>(base.MemberInfo, base.Item, base.List);
			if (expandAttribute != null)
			{
				expanded = expandAttribute.Expand;
			}
		}
		else
		{
			ExpandAttribute expandAttribute2 = (ExpandAttribute)Attribute.GetCustomAttribute(base.MemberInfo.Type.GetGenericArguments()[0], typeof(ExpandAttribute), inherit: true);
			if (expandAttribute2 != null)
			{
				expanded = expandAttribute2.Expand;
			}
			expandAttribute2 = (ExpandAttribute)Attribute.GetCustomAttribute(base.MemberInfo.MemberInfo, typeof(ExpandAttribute), inherit: true);
			if (expandAttribute2 != null && expandAttribute2.ExpandListElements.HasValue)
			{
				expanded = expandAttribute2.ExpandListElements.Value;
			}
		}
		dataList = new NestedUIList();
		dataList.Width.Set(-14f, 1f);
		dataList.Left.Set(14f, 0f);
		dataList.Height.Set(-30f, 1f);
		dataList.Top.Set(30f, 0f);
		dataList.ListPadding = 5f;
		if (expanded)
		{
			Append(dataList);
		}
		_ = base.List;
		initializeButton = new UIModConfigHoverImage(base.PlayTexture, Language.GetTextValue("tModLoader.ModConfigInitialize"));
		initializeButton.Top.Pixels += 4f;
		initializeButton.Left.Pixels -= 3f;
		initializeButton.HAlign = 1f;
		initializeButton.OnLeftClick += delegate
		{
			SoundEngine.PlaySound(21);
			object obj = Activator.CreateInstance(base.MemberInfo.Type, nonPublic: true);
			JsonConvert.PopulateObject(JsonDefaultValueAttribute?.Json ?? "{}", obj, ConfigManager.serializerSettings);
			Value = obj;
			pendingChanges = true;
			SetupList();
			Interface.modConfig.RecalculateChildren();
			Interface.modConfig.SetPendingChanges();
		};
		expandButton = new UIModConfigHoverImage(expanded ? base.ExpandedTexture : base.CollapsedTexture, expanded ? Language.GetTextValue("tModLoader.ModConfigCollapse") : Language.GetTextValue("tModLoader.ModConfigExpand"));
		expandButton.Top.Set(4f, 0f);
		expandButton.Left.Set(-52f, 1f);
		expandButton.OnLeftClick += delegate
		{
			expanded = !expanded;
			pendingChanges = true;
		};
		deleteButton = new UIModConfigHoverImage(base.DeleteTexture, Language.GetTextValue("tModLoader.ModConfigClear"));
		deleteButton.Top.Set(4f, 0f);
		deleteButton.Left.Set(-25f, 1f);
		deleteButton.OnLeftClick += delegate
		{
			Value = null;
			pendingChanges = true;
			SetupList();
			Interface.modConfig.SetPendingChanges();
		};
		if (Value != null)
		{
			SetupList();
		}
		else
		{
			Append(initializeButton);
		}
		pendingChanges = true;
		Recalculate();
	}

	public override void Update(GameTime gameTime)
	{
		base.Update(gameTime);
		if (!pendingChanges)
		{
			return;
		}
		pendingChanges = false;
		base.DrawLabel = !separatePage || ignoreSeparatePage;
		RemoveChild(deleteButton);
		RemoveChild(expandButton);
		RemoveChild(initializeButton);
		RemoveChild(dataList);
		if (separatePage && !ignoreSeparatePage)
		{
			RemoveChild(separatePageButton);
		}
		if (Value == null)
		{
			Append(initializeButton);
			base.DrawLabel = true;
			return;
		}
		if (base.List == null && (!separatePage || !ignoreSeparatePage) && base.NullAllowed)
		{
			Append(deleteButton);
		}
		if (!separatePage || ignoreSeparatePage)
		{
			if (!ignoreSeparatePage)
			{
				Append(expandButton);
			}
			if (expanded)
			{
				Append(dataList);
				expandButton.HoverText = Language.GetTextValue("tModLoader.ModConfigCollapse");
				expandButton.SetImage(base.ExpandedTexture);
			}
			else
			{
				RemoveChild(dataList);
				expandButton.HoverText = Language.GetTextValue("tModLoader.ModConfigExpand");
				expandButton.SetImage(base.CollapsedTexture);
			}
		}
		else
		{
			Append(separatePageButton);
		}
	}

	private void SetupList()
	{
		dataList.Clear();
		object data = Value;
		if (data == null)
		{
			return;
		}
		if (separatePage && !ignoreSeparatePage)
		{
			separatePagePanel = UIModConfig.MakeSeparateListPanel(base.Item, data, base.MemberInfo, base.List, base.Index, AbridgedTextDisplayFunction);
			return;
		}
		int order = 0;
		foreach (PropertyFieldWrapper variable in ConfigManager.GetFieldsAndProperties(data))
		{
			if (!Attribute.IsDefined(variable.MemberInfo, typeof(JsonIgnoreAttribute)))
			{
				int top = 0;
				UIModConfig.HandleHeader(dataList, ref top, ref order, variable);
				Tuple<UIElement, UIElement> wrapped = UIModConfig.WrapIt(dataList, ref top, variable, data, order++);
				if (base.List != null)
				{
					wrapped.Item1.Width.Pixels += 20f;
				}
			}
		}
	}

	public override void Recalculate()
	{
		base.Recalculate();
		float defaultHeight = (separatePage ? 40 : 30);
		float h = ((dataList.Parent != null) ? (dataList.GetTotalHeight() + defaultHeight) : defaultHeight);
		Height.Set(h, 0f);
		if (base.Parent != null && base.Parent is UISortableElement)
		{
			base.Parent.Height.Set(h, 0f);
		}
	}
}
