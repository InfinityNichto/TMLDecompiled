using System;
using System.Collections.Generic;
using Terraria.ModLoader.UI;

namespace Terraria.ModLoader.Config.UI;

internal class StringOptionElement : RangeElement
{
	private Func<string> getValue;

	private Func<int> getIndex;

	private Action<int> setValue;

	private string[] options;

	public IList<string> StringList { get; set; }

	public override int NumberTicks => options.Length;

	public override float TickIncrement => 1f / (float)(options.Length - 1);

	protected override float Proportion
	{
		get
		{
			return (float)getIndex() / (float)(options.Length - 1);
		}
		set
		{
			setValue((int)Math.Round(value * (float)(options.Length - 1)));
		}
	}

	public override void OnBind()
	{
		base.OnBind();
		StringList = (IList<string>)base.List;
		OptionStringsAttribute optionsAttribute = ConfigManager.GetCustomAttributeFromMemberThenMemberType<OptionStringsAttribute>(base.MemberInfo, base.Item, StringList);
		options = optionsAttribute.OptionLabels;
		base.TextDisplayFunction = () => base.MemberInfo.Name + ": " + getValue();
		getValue = () => DefaultGetValue();
		getIndex = () => DefaultGetIndex();
		setValue = delegate(int value)
		{
			DefaultSetValue(value);
		};
		if (StringList != null)
		{
			getValue = () => StringList[base.Index];
			setValue = delegate(int value)
			{
				StringList[base.Index] = options[value];
				Interface.modConfig.SetPendingChanges();
			};
			base.TextDisplayFunction = () => base.Index + 1 + ": " + StringList[base.Index];
		}
		if (Label != null)
		{
			base.TextDisplayFunction = () => Label + ": " + getValue();
		}
	}

	private void DefaultSetValue(int index)
	{
		if (base.MemberInfo.CanWrite)
		{
			base.MemberInfo.SetValue(base.Item, options[index]);
			Interface.modConfig.SetPendingChanges();
		}
	}

	private string DefaultGetValue()
	{
		return (string)base.MemberInfo.GetValue(base.Item);
	}

	private int DefaultGetIndex()
	{
		return Array.IndexOf(options, getValue());
	}
}
