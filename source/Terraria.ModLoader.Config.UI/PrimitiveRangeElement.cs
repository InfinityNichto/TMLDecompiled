using System;
using System.Collections.Generic;

namespace Terraria.ModLoader.Config.UI;

public abstract class PrimitiveRangeElement<T> : RangeElement where T : IComparable<T>
{
	public T Min { get; set; }

	public T Max { get; set; }

	public T Increment { get; set; }

	public IList<T> TList { get; set; }

	public override void OnBind()
	{
		base.OnBind();
		TList = (IList<T>)base.List;
		base.TextDisplayFunction = () => base.MemberInfo.Name + ": " + GetValue();
		if (TList != null)
		{
			base.TextDisplayFunction = () => base.Index + 1 + ": " + TList[base.Index];
		}
		if (Label != null)
		{
			base.TextDisplayFunction = () => Label + ": " + GetValue();
		}
		if (RangeAttribute != null && RangeAttribute.Min is T && RangeAttribute.Max is T)
		{
			Min = (T)RangeAttribute.Min;
			Max = (T)RangeAttribute.Max;
		}
		if (IncrementAttribute != null && IncrementAttribute.Increment is T)
		{
			Increment = (T)IncrementAttribute.Increment;
		}
	}

	protected virtual T GetValue()
	{
		return (T)GetObject();
	}

	protected virtual void SetValue(object value)
	{
		if (value is T t)
		{
			SetObject(Utils.Clamp(t, Min, Max));
		}
	}
}
