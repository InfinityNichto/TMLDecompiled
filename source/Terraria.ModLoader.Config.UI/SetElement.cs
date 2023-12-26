using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Terraria.Localization;
using Terraria.ModLoader.UI;
using Terraria.UI;

namespace Terraria.ModLoader.Config.UI;

internal class SetElement : CollectionElement
{
	private Type setType;

	public List<ISetElementWrapper> DataWrapperList { get; set; }

	protected override void PrepareTypes()
	{
		setType = base.MemberInfo.Type.GetGenericArguments()[0];
		base.JsonDefaultListValueAttribute = ConfigManager.GetCustomAttributeFromCollectionMemberThenElementType<JsonDefaultListValueAttribute>(base.MemberInfo.MemberInfo, setType);
	}

	protected override void AddItem()
	{
		base.Data.GetType().GetMethods().FirstOrDefault((MethodInfo m) => m.Name == "Add")
			.Invoke(base.Data, new object[1] { CreateCollectionElementInstance(setType) });
	}

	protected override void InitializeCollection()
	{
		base.Data = Activator.CreateInstance(typeof(HashSet<>).MakeGenericType(setType));
		SetObject(base.Data);
	}

	protected override void ClearCollection()
	{
		base.Data.GetType().GetMethods().FirstOrDefault((MethodInfo m) => m.Name == "Clear")
			.Invoke(base.Data, new object[0]);
	}

	protected override void SetupList()
	{
		base.DataList.Clear();
		int top = 0;
		Type genericType = typeof(SetElementWrapper<>).MakeGenericType(setType);
		DataWrapperList = new List<ISetElementWrapper>();
		if (base.Data == null)
		{
			return;
		}
		IEnumerator valuesEnumerator = ((IEnumerable)base.Data).GetEnumerator();
		int i = 0;
		while (valuesEnumerator.MoveNext())
		{
			ISetElementWrapper proxy = (ISetElementWrapper)Activator.CreateInstance(genericType, valuesEnumerator.Current, base.Data);
			DataWrapperList.Add(proxy);
			PropertyFieldWrapper wrappermemberInfo = ConfigManager.GetFieldsAndProperties(this).ToList().First((PropertyFieldWrapper x) => x.Name == "DataWrapperList");
			Tuple<UIElement, UIElement> tuple = UIModConfig.WrapIt(base.DataList, ref top, wrappermemberInfo, this, 0, DataWrapperList, genericType, i);
			tuple.Item2.Left.Pixels += 24f;
			tuple.Item2.Width.Pixels -= 24f;
			UIModConfigHoverImage deleteButton = new UIModConfigHoverImage(base.DeleteTexture, Language.GetTextValue("tModLoader.ModConfigRemove"))
			{
				VAlign = 0.5f
			};
			object o = valuesEnumerator.Current;
			deleteButton.OnLeftClick += delegate
			{
				base.Data.GetType().GetMethods().FirstOrDefault((MethodInfo m) => m.Name == "Remove")
					.Invoke(base.Data, new object[1] { o });
				SetupList();
				Interface.modConfig.SetPendingChanges();
			};
			tuple.Item1.Append(deleteButton);
			i++;
		}
	}
}
