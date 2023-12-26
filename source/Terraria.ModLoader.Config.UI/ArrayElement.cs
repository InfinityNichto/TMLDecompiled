using System;

namespace Terraria.ModLoader.Config.UI;

internal class ArrayElement : CollectionElement
{
	private Type itemType;

	protected override bool CanAdd => false;

	protected override void AddItem()
	{
		throw new NotImplementedException();
	}

	protected override void ClearCollection()
	{
		throw new NotImplementedException();
	}

	protected override void InitializeCollection()
	{
		throw new NotImplementedException();
	}

	protected override void NullCollection()
	{
		throw new NotImplementedException();
	}

	protected override void PrepareTypes()
	{
		itemType = base.MemberInfo.Type.GetElementType();
	}

	protected override void SetupList()
	{
		base.DataList.Clear();
		int count = (base.MemberInfo.GetValue(base.Item) as Array).Length;
		int top = 0;
		for (int i = 0; i < count; i++)
		{
			int index = i;
			UIModConfig.WrapIt(base.DataList, ref top, base.MemberInfo, base.Item, 0, base.Data, itemType, index);
		}
	}
}
