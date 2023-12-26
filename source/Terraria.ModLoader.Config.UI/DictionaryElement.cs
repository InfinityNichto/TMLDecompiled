using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using Terraria.GameContent.UI.Elements;
using Terraria.Localization;
using Terraria.ModLoader.UI;
using Terraria.UI;

namespace Terraria.ModLoader.Config.UI;

internal class DictionaryElement : CollectionElement
{
	internal Type keyType;

	internal Type valueType;

	internal UIText save;

	public List<IDictionaryElementWrapper> dataWrapperList;

	protected DefaultDictionaryKeyValueAttribute defaultDictionaryKeyValueAttribute;

	protected JsonDefaultDictionaryKeyValueAttribute jsonDefaultDictionaryKeyValueAttribute;

	protected override void PrepareTypes()
	{
		keyType = base.MemberInfo.Type.GetGenericArguments()[0];
		valueType = base.MemberInfo.Type.GetGenericArguments()[1];
		base.JsonDefaultListValueAttribute = ConfigManager.GetCustomAttributeFromCollectionMemberThenElementType<JsonDefaultListValueAttribute>(base.MemberInfo.MemberInfo, valueType);
		defaultDictionaryKeyValueAttribute = ConfigManager.GetCustomAttributeFromMemberThenMemberType<DefaultDictionaryKeyValueAttribute>(base.MemberInfo, null, null);
		jsonDefaultDictionaryKeyValueAttribute = ConfigManager.GetCustomAttributeFromMemberThenMemberType<JsonDefaultDictionaryKeyValueAttribute>(base.MemberInfo, null, null);
	}

	protected override void AddItem()
	{
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			object keyValue;
			if (defaultDictionaryKeyValueAttribute != null)
			{
				keyValue = defaultDictionaryKeyValueAttribute.Value;
			}
			else
			{
				keyValue = ConfigManager.AlternateCreateInstance(keyType);
				if (!keyType.IsValueType && keyType != typeof(string))
				{
					JsonConvert.PopulateObject(jsonDefaultDictionaryKeyValueAttribute?.Json ?? "{}", keyValue, ConfigManager.serializerSettings);
				}
			}
			((IDictionary)base.Data).Add(keyValue, CreateCollectionElementInstance(valueType));
		}
		catch (Exception e)
		{
			Interface.modConfig.SetMessage("Error: " + e.Message, Color.Red);
		}
	}

	protected override void InitializeCollection()
	{
		base.Data = Activator.CreateInstance(typeof(Dictionary<, >).MakeGenericType(keyType, valueType));
		SetObject(base.Data);
	}

	protected override void ClearCollection()
	{
		((IDictionary)base.Data).Clear();
	}

	protected override void SetupList()
	{
		base.DataList.Clear();
		int top = 0;
		dataWrapperList = new List<IDictionaryElementWrapper>();
		Type genericType = typeof(DictionaryElementWrapper<, >).MakeGenericType(keyType, valueType);
		if (base.Data == null)
		{
			return;
		}
		ICollection keys = ((IDictionary)base.Data).Keys;
		ICollection values = ((IDictionary)base.Data).Values;
		IEnumerator keysEnumerator = keys.GetEnumerator();
		IEnumerator valuesEnumerator = values.GetEnumerator();
		int i = 0;
		while (keysEnumerator.MoveNext())
		{
			valuesEnumerator.MoveNext();
			IDictionaryElementWrapper proxy = (IDictionaryElementWrapper)Activator.CreateInstance(genericType, keysEnumerator.Current, valuesEnumerator.Current, (IDictionary)base.Data);
			dataWrapperList.Add(proxy);
			_ = base.MemberInfo.Type.GetGenericArguments()[0];
			PropertyFieldWrapper wrappermemberInfo = ConfigManager.GetFieldsAndProperties(this).ToList()[0];
			Tuple<UIElement, UIElement> tuple = UIModConfig.WrapIt(base.DataList, ref top, wrappermemberInfo, this, 0, dataWrapperList, genericType, i);
			tuple.Item2.Left.Pixels += 24f;
			tuple.Item2.Width.Pixels -= 24f;
			UIModConfigHoverImage deleteButton = new UIModConfigHoverImage(base.DeleteTexture, Language.GetTextValue("tModLoader.ModConfigRemove"))
			{
				VAlign = 0.5f
			};
			object o = keysEnumerator.Current;
			deleteButton.OnLeftClick += delegate
			{
				((IDictionary)base.Data).Remove(o);
				SetupList();
				Interface.modConfig.SetPendingChanges();
			};
			tuple.Item1.Append(deleteButton);
			i++;
		}
	}
}
