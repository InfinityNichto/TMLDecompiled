using Microsoft.Xna.Framework;

namespace Terraria.DataStructures;

public struct ItemSyncPersistentStats
{
	private Color color;

	private int type;

	public void CopyFrom(Item item)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		type = item.type;
		color = item.color;
	}

	public void PasteInto(Item item)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		if (type == item.type)
		{
			item.color = color;
		}
	}
}
