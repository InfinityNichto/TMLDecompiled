using System;
using System.Reflection;

namespace Terraria.ModLoader.Config.UI;

public class PropertyFieldWrapper
{
	private readonly FieldInfo fieldInfo;

	private readonly PropertyInfo propertyInfo;

	public bool IsField => fieldInfo != null;

	public bool IsProperty => propertyInfo != null;

	public MemberInfo MemberInfo => (MemberInfo)(((object)fieldInfo) ?? ((object)propertyInfo));

	public string Name => fieldInfo?.Name ?? propertyInfo.Name;

	public Type Type => fieldInfo?.FieldType ?? propertyInfo.PropertyType;

	public bool CanWrite
	{
		get
		{
			if (!(fieldInfo != null))
			{
				return propertyInfo.CanWrite;
			}
			return true;
		}
	}

	public PropertyFieldWrapper(FieldInfo fieldInfo)
	{
		this.fieldInfo = fieldInfo;
	}

	public PropertyFieldWrapper(PropertyInfo propertyInfo)
	{
		this.propertyInfo = propertyInfo;
	}

	public object GetValue(object obj)
	{
		if (fieldInfo != null)
		{
			return fieldInfo.GetValue(obj);
		}
		return propertyInfo.GetValue(obj, null);
	}

	public void SetValue(object obj, object value)
	{
		if (fieldInfo != null)
		{
			fieldInfo.SetValue(obj, value);
		}
		else if (propertyInfo.CanWrite)
		{
			propertyInfo.SetValue(obj, value, null);
		}
	}
}
