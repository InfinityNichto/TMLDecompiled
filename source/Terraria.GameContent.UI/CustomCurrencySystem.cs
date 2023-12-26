using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria.GameContent.UI;

public class CustomCurrencySystem
{
	protected Dictionary<int, int> _valuePerUnit = new Dictionary<int, int>();

	private long _currencyCap = 999999999L;

	public long CurrencyCap => _currencyCap;

	public void Include(int coin, int howMuchIsItWorth)
	{
		_valuePerUnit[coin] = howMuchIsItWorth;
	}

	public void SetCurrencyCap(long cap)
	{
		_currencyCap = cap;
	}

	public virtual long CountCurrency(out bool overFlowing, Item[] inv, params int[] ignoreSlots)
	{
		List<int> list = new List<int>(ignoreSlots);
		long num = 0L;
		for (int i = 0; i < inv.Length; i++)
		{
			if (!list.Contains(i))
			{
				if (_valuePerUnit.TryGetValue(inv[i].type, out var value))
				{
					num += value * inv[i].stack;
				}
				if (num >= CurrencyCap)
				{
					overFlowing = true;
					return CurrencyCap;
				}
			}
		}
		overFlowing = false;
		return num;
	}

	public virtual long CombineStacks(out bool overFlowing, params long[] coinCounts)
	{
		long num = 0L;
		foreach (long num2 in coinCounts)
		{
			num += num2;
			if (num >= CurrencyCap)
			{
				overFlowing = true;
				return CurrencyCap;
			}
		}
		overFlowing = false;
		return num;
	}

	public virtual bool TryPurchasing(long price, List<Item[]> inv, List<Point> slotCoins, List<Point> slotsEmpty, List<Point> slotEmptyBank, List<Point> slotEmptyBank2, List<Point> slotEmptyBank3, List<Point> slotEmptyBank4)
	{
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_0618: Unknown result type (might be due to invalid IL or missing references)
		//IL_0629: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_035a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0364: Unknown result type (might be due to invalid IL or missing references)
		//IL_038a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0570: Unknown result type (might be due to invalid IL or missing references)
		//IL_0397: Unknown result type (might be due to invalid IL or missing references)
		//IL_057a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0585: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0255: Unknown result type (might be due to invalid IL or missing references)
		//IL_0261: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02db: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_0306: Unknown result type (might be due to invalid IL or missing references)
		//IL_0318: Unknown result type (might be due to invalid IL or missing references)
		//IL_032d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0332: Unknown result type (might be due to invalid IL or missing references)
		//IL_0280: Unknown result type (might be due to invalid IL or missing references)
		//IL_028c: Unknown result type (might be due to invalid IL or missing references)
		//IL_029c: Unknown result type (might be due to invalid IL or missing references)
		//IL_045d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0469: Unknown result type (might be due to invalid IL or missing references)
		//IL_04af: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_050e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0520: Unknown result type (might be due to invalid IL or missing references)
		//IL_0535: Unknown result type (might be due to invalid IL or missing references)
		//IL_053a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0488: Unknown result type (might be due to invalid IL or missing references)
		//IL_0494: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a4: Unknown result type (might be due to invalid IL or missing references)
		long num = price;
		Dictionary<Point, Item> dictionary = new Dictionary<Point, Item>();
		bool result = true;
		Point item = default(Point);
		while (num > 0)
		{
			long num2 = 1000000L;
			for (int i = 0; i < 4; i++)
			{
				if (num >= num2)
				{
					foreach (Point slotCoin in slotCoins)
					{
						if (inv[slotCoin.X][slotCoin.Y].type == 74 - i)
						{
							long num3 = num / num2;
							dictionary[slotCoin] = inv[slotCoin.X][slotCoin.Y].Clone();
							if (num3 < inv[slotCoin.X][slotCoin.Y].stack)
							{
								inv[slotCoin.X][slotCoin.Y].stack -= (int)num3;
							}
							else
							{
								inv[slotCoin.X][slotCoin.Y].SetDefaults();
								slotsEmpty.Add(slotCoin);
							}
							num -= num2 * (dictionary[slotCoin].stack - inv[slotCoin.X][slotCoin.Y].stack);
						}
					}
				}
				num2 /= 100;
			}
			if (num <= 0)
			{
				continue;
			}
			if (slotsEmpty.Count > 0)
			{
				slotsEmpty.Sort(DelegateMethods.CompareYReverse);
				((Point)(ref item))._002Ector(-1, -1);
				for (int j = 0; j < inv.Count; j++)
				{
					num2 = 10000L;
					for (int k = 0; k < 3; k++)
					{
						if (num >= num2)
						{
							foreach (Point slotCoin2 in slotCoins)
							{
								if (slotCoin2.X == j && inv[slotCoin2.X][slotCoin2.Y].type == 74 - k && inv[slotCoin2.X][slotCoin2.Y].stack >= 1)
								{
									List<Point> list = slotsEmpty;
									if (j == 1 && slotEmptyBank.Count > 0)
									{
										list = slotEmptyBank;
									}
									if (j == 2 && slotEmptyBank2.Count > 0)
									{
										list = slotEmptyBank2;
									}
									if (j == 3 && slotEmptyBank3.Count > 0)
									{
										list = slotEmptyBank3;
									}
									if (j == 4 && slotEmptyBank4.Count > 0)
									{
										list = slotEmptyBank4;
									}
									if (--inv[slotCoin2.X][slotCoin2.Y].stack <= 0)
									{
										inv[slotCoin2.X][slotCoin2.Y].SetDefaults();
										list.Add(slotCoin2);
									}
									dictionary[list[0]] = inv[list[0].X][list[0].Y].Clone();
									inv[list[0].X][list[0].Y].SetDefaults(73 - k);
									inv[list[0].X][list[0].Y].stack = 100;
									item = list[0];
									list.RemoveAt(0);
									break;
								}
							}
						}
						if (item.X != -1 || item.Y != -1)
						{
							break;
						}
						num2 /= 100;
					}
					for (int l = 0; l < 2; l++)
					{
						if (item.X != -1 || item.Y != -1)
						{
							continue;
						}
						foreach (Point slotCoin3 in slotCoins)
						{
							if (slotCoin3.X == j && inv[slotCoin3.X][slotCoin3.Y].type == 73 + l && inv[slotCoin3.X][slotCoin3.Y].stack >= 1)
							{
								List<Point> list2 = slotsEmpty;
								if (j == 1 && slotEmptyBank.Count > 0)
								{
									list2 = slotEmptyBank;
								}
								if (j == 2 && slotEmptyBank2.Count > 0)
								{
									list2 = slotEmptyBank2;
								}
								if (j == 3 && slotEmptyBank3.Count > 0)
								{
									list2 = slotEmptyBank3;
								}
								if (j == 4 && slotEmptyBank4.Count > 0)
								{
									list2 = slotEmptyBank4;
								}
								if (--inv[slotCoin3.X][slotCoin3.Y].stack <= 0)
								{
									inv[slotCoin3.X][slotCoin3.Y].SetDefaults();
									list2.Add(slotCoin3);
								}
								dictionary[list2[0]] = inv[list2[0].X][list2[0].Y].Clone();
								inv[list2[0].X][list2[0].Y].SetDefaults(72 + l);
								inv[list2[0].X][list2[0].Y].stack = 100;
								item = list2[0];
								list2.RemoveAt(0);
								break;
							}
						}
					}
					if (item.X != -1 && item.Y != -1)
					{
						slotCoins.Add(item);
						break;
					}
				}
				slotsEmpty.Sort(DelegateMethods.CompareYReverse);
				slotEmptyBank.Sort(DelegateMethods.CompareYReverse);
				slotEmptyBank2.Sort(DelegateMethods.CompareYReverse);
				slotEmptyBank3.Sort(DelegateMethods.CompareYReverse);
				slotEmptyBank4.Sort(DelegateMethods.CompareYReverse);
				continue;
			}
			foreach (KeyValuePair<Point, Item> item2 in dictionary)
			{
				inv[item2.Key.X][item2.Key.Y] = item2.Value.Clone();
			}
			result = false;
			break;
		}
		return result;
	}

	public virtual bool Accepts(Item item)
	{
		return _valuePerUnit.ContainsKey(item.type);
	}

	public virtual void DrawSavingsMoney(SpriteBatch sb, string text, float shopx, float shopy, long totalCoins, bool horizontal = false)
	{
	}

	public virtual void GetPriceText(string[] lines, ref int currentLine, long price)
	{
	}

	protected int SortByHighest(Tuple<int, int> valueA, Tuple<int, int> valueB)
	{
		if (valueA.Item2 > valueB.Item2)
		{
			return -1;
		}
		if (valueA.Item2 == valueB.Item2)
		{
			return 0;
		}
		return -1;
	}

	protected List<Tuple<Point, Item>> ItemCacheCreate(List<Item[]> inventories)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		List<Tuple<Point, Item>> list = new List<Tuple<Point, Item>>();
		for (int i = 0; i < inventories.Count; i++)
		{
			for (int j = 0; j < inventories[i].Length; j++)
			{
				Item item = inventories[i][j];
				list.Add(new Tuple<Point, Item>(new Point(i, j), item.DeepClone()));
			}
		}
		return list;
	}

	protected void ItemCacheRestore(List<Tuple<Point, Item>> cache, List<Item[]> inventories)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		foreach (Tuple<Point, Item> item in cache)
		{
			inventories[item.Item1.X][item.Item1.Y] = item.Item2;
		}
	}

	public virtual void GetItemExpectedPrice(Item item, out long calcForSelling, out long calcForBuying)
	{
		int storeValue = item.GetStoreValue();
		calcForSelling = storeValue;
		calcForBuying = storeValue;
	}
}
