using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Localization;
using Terraria.UI.Chat;

namespace Terraria.GameContent.UI;

public class CustomCurrencySingleCoin : CustomCurrencySystem
{
	public float CurrencyDrawScale = 0.8f;

	public string CurrencyTextKey = "Currency.DefenderMedals";

	public Color CurrencyTextColor = new Color(240, 100, 120);

	public CustomCurrencySingleCoin(int coinItemID, long currencyCap)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		Include(coinItemID, 1);
		SetCurrencyCap(currencyCap);
	}

	public override bool TryPurchasing(long price, List<Item[]> inv, List<Point> slotCoins, List<Point> slotsEmpty, List<Point> slotEmptyBank, List<Point> slotEmptyBank2, List<Point> slotEmptyBank3, List<Point> slotEmptyBank4)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		List<Tuple<Point, Item>> cache = ItemCacheCreate(inv);
		long num = price;
		for (int i = 0; i < slotCoins.Count; i++)
		{
			Point item = slotCoins[i];
			long num2 = num;
			if (inv[item.X][item.Y].stack < num2)
			{
				num2 = inv[item.X][item.Y].stack;
			}
			num -= num2;
			inv[item.X][item.Y].stack -= (int)num2;
			if (inv[item.X][item.Y].stack == 0)
			{
				switch (item.X)
				{
				case 0:
					slotsEmpty.Add(item);
					break;
				case 1:
					slotEmptyBank.Add(item);
					break;
				case 2:
					slotEmptyBank2.Add(item);
					break;
				case 3:
					slotEmptyBank3.Add(item);
					break;
				case 4:
					slotEmptyBank4.Add(item);
					break;
				}
				slotCoins.Remove(item);
				i--;
			}
			if (num == 0L)
			{
				break;
			}
		}
		if (num != 0L)
		{
			ItemCacheRestore(cache, inv);
			return false;
		}
		return true;
	}

	public override void DrawSavingsMoney(SpriteBatch sb, string text, float shopx, float shopy, long totalCoins, bool horizontal = false)
	{
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_0158: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		int num = _valuePerUnit.Keys.ElementAt(0);
		Main.instance.LoadItem(num);
		Texture2D value = TextureAssets.Item[num].Value;
		if (horizontal)
		{
			Vector2 position = default(Vector2);
			((Vector2)(ref position))._002Ector(shopx + ChatManager.GetStringSize(FontAssets.MouseText.Value, text, Vector2.One).X + 45f, shopy + 50f);
			sb.Draw(value, position, (Rectangle?)null, Color.White, 0f, value.Size() / 2f, CurrencyDrawScale, (SpriteEffects)0, 0f);
			Utils.DrawBorderStringFourWay(sb, FontAssets.ItemStack.Value, totalCoins.ToString(), position.X - 11f, position.Y, Color.White, Color.Black, new Vector2(0.3f), 0.75f);
		}
		else
		{
			int num2 = ((totalCoins > 99) ? (-6) : 0);
			sb.Draw(value, new Vector2(shopx + 11f, shopy + 75f), (Rectangle?)null, Color.White, 0f, value.Size() / 2f, CurrencyDrawScale, (SpriteEffects)0, 0f);
			Utils.DrawBorderStringFourWay(sb, FontAssets.ItemStack.Value, totalCoins.ToString(), shopx + (float)num2, shopy + 75f, Color.White, Color.Black, new Vector2(0.3f), 0.75f);
		}
	}

	public override void GetPriceText(string[] lines, ref int currentLine, long price)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		Color color = CurrencyTextColor * ((float)(int)Main.mouseTextColor / 255f);
		lines[currentLine++] = $"[c/{((Color)(ref color)).R:X2}{((Color)(ref color)).G:X2}{((Color)(ref color)).B:X2}:{Lang.tip[50].Value} {price} {Language.GetTextValue(CurrencyTextKey).ToLower()}]";
	}
}
