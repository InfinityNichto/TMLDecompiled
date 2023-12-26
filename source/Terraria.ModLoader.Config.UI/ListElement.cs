using System;
using System.Collections;
using System.Collections.Generic;
using Terraria.Localization;
using Terraria.ModLoader.UI;
using Terraria.UI;

namespace Terraria.ModLoader.Config.UI;

internal class ListElement : CollectionElement
{
	private Type listType;

	protected override void PrepareTypes()
	{
		listType = base.MemberInfo.Type.GetGenericArguments()[0];
		base.JsonDefaultListValueAttribute = ConfigManager.GetCustomAttributeFromCollectionMemberThenElementType<JsonDefaultListValueAttribute>(base.MemberInfo.MemberInfo, listType);
	}

	protected override void AddItem()
	{
		((IList)base.Data).Add(CreateCollectionElementInstance(listType));
	}

	protected override void InitializeCollection()
	{
		base.Data = Activator.CreateInstance(typeof(List<>).MakeGenericType(listType));
		SetObject(base.Data);
	}

	protected override void ClearCollection()
	{
		((IList)base.Data).Clear();
	}

	protected override void SetupList()
	{
		base.DataList.Clear();
		int top = 0;
		if (base.Data == null)
		{
			return;
		}
		for (int i = 0; i < ((IList)base.Data).Count; i++)
		{
			int index = i;
			Tuple<UIElement, UIElement> tuple = UIModConfig.WrapIt(base.DataList, ref top, base.MemberInfo, base.Item, 0, base.Data, listType, index);
			tuple.Item2.Left.Pixels += 24f;
			tuple.Item2.Width.Pixels -= 30f;
			UIModConfigHoverImage deleteButton = new UIModConfigHoverImage(base.DeleteTexture, Language.GetTextValue("tModLoader.ModConfigRemove"));
			deleteButton.VAlign = 0.5f;
			deleteButton.OnLeftClick += delegate
			{
				((IList)base.Data).RemoveAt(index);
				SetupList();
				Interface.modConfig.SetPendingChanges();
			};
			tuple.Item1.Append(deleteButton);
		}
	}
}
