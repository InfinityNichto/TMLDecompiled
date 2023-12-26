using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;

namespace Terraria.GameContent.UI;

public class ItemRarity
{
	private static Dictionary<int, Color> _rarities = new Dictionary<int, Color>();

	public static void Initialize()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		_rarities.Clear();
		_rarities.Add(-11, Colors.RarityAmber);
		_rarities.Add(-1, Colors.RarityTrash);
		_rarities.Add(1, Colors.RarityBlue);
		_rarities.Add(2, Colors.RarityGreen);
		_rarities.Add(3, Colors.RarityOrange);
		_rarities.Add(4, Colors.RarityRed);
		_rarities.Add(5, Colors.RarityPink);
		_rarities.Add(6, Colors.RarityPurple);
		_rarities.Add(7, Colors.RarityLime);
		_rarities.Add(8, Colors.RarityYellow);
		_rarities.Add(9, Colors.RarityCyan);
		_rarities.Add(10, Colors.RarityDarkRed);
		_rarities.Add(11, Colors.RarityDarkPurple);
		for (int i = 12; i < RarityLoader.RarityCount; i++)
		{
			_rarities.Add(i, RarityLoader.GetRarity(i).RarityColor);
		}
	}

	public static Color GetColor(int rarity)
	{
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		Color result = default(Color);
		((Color)(ref result))._002Ector((int)Main.mouseTextColor, (int)Main.mouseTextColor, (int)Main.mouseTextColor, (int)Main.mouseTextColor);
		if (_rarities.ContainsKey(rarity))
		{
			return _rarities[rarity];
		}
		return result;
	}
}
