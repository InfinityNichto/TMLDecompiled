using System;
using System.Reflection;
using Terraria.ModLoader.UI;

namespace Terraria.ModLoader.Config.UI;

internal class EnumElement : RangeElement
{
	private Func<object> _getValue;

	private Func<object> _getValueString;

	private Func<int> _getIndex;

	private Action<int> _setValue;

	private int max;

	private string[] valueStrings;

	public override int NumberTicks => valueStrings.Length;

	public override float TickIncrement => 1f / (float)(valueStrings.Length - 1);

	protected override float Proportion
	{
		get
		{
			return (float)_getIndex() / (float)(max - 1);
		}
		set
		{
			_setValue((int)Math.Round(value * (float)(max - 1)));
		}
	}

	public override void OnBind()
	{
		base.OnBind();
		valueStrings = Enum.GetNames(base.MemberInfo.Type);
		for (int i = 0; i < valueStrings.Length; i++)
		{
			FieldInfo enumFieldFieldInfo = base.MemberInfo.Type.GetField(valueStrings[i]);
			if (enumFieldFieldInfo != null)
			{
				string name = ConfigManager.GetLocalizedLabel(new PropertyFieldWrapper(enumFieldFieldInfo));
				valueStrings[i] = name;
			}
		}
		max = valueStrings.Length;
		base.TextDisplayFunction = () => base.MemberInfo.Name + ": " + _getValueString();
		_getValue = () => DefaultGetValue();
		_getValueString = () => DefaultGetStringValue();
		_getIndex = () => DefaultGetIndex();
		_setValue = delegate(int value)
		{
			DefaultSetValue(value);
		};
		if (Label != null)
		{
			base.TextDisplayFunction = () => Label + ": " + _getValueString();
		}
	}

	private void DefaultSetValue(int index)
	{
		if (base.MemberInfo.CanWrite)
		{
			base.MemberInfo.SetValue(base.Item, Enum.GetValues(base.MemberInfo.Type).GetValue(index));
			Interface.modConfig.SetPendingChanges();
		}
	}

	private object DefaultGetValue()
	{
		return base.MemberInfo.GetValue(base.Item);
	}

	private int DefaultGetIndex()
	{
		return Array.IndexOf(Enum.GetValues(base.MemberInfo.Type), _getValue());
	}

	private string DefaultGetStringValue()
	{
		return valueStrings[_getIndex()];
	}
}
