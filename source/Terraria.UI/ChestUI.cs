using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI.Chat;
using Terraria.UI.Gamepad;

namespace Terraria.UI;

public class ChestUI
{
	public class ButtonID
	{
		public const int LootAll = 0;

		public const int DepositAll = 1;

		public const int QuickStack = 2;

		public const int Restock = 3;

		public const int Sort = 4;

		public const int RenameChest = 5;

		public const int RenameChestCancel = 6;

		public const int ToggleVacuum = 7;

		public static readonly int Count = 7;
	}

	public const float buttonScaleMinimum = 0.75f;

	public const float buttonScaleMaximum = 1f;

	public static float[] ButtonScale = new float[ButtonID.Count];

	public static bool[] ButtonHovered = new bool[ButtonID.Count];

	public static void UpdateHover(int ID, bool hovering)
	{
		if (hovering)
		{
			if (!ButtonHovered[ID])
			{
				SoundEngine.PlaySound(12);
			}
			ButtonHovered[ID] = true;
			ButtonScale[ID] += 0.05f;
			if (ButtonScale[ID] > 1f)
			{
				ButtonScale[ID] = 1f;
			}
		}
		else
		{
			ButtonHovered[ID] = false;
			ButtonScale[ID] -= 0.05f;
			if (ButtonScale[ID] < 0.75f)
			{
				ButtonScale[ID] = 0.75f;
			}
		}
	}

	public static void Draw(SpriteBatch spritebatch)
	{
		if (Main.player[Main.myPlayer].chest != -1 && !Main.recBigList)
		{
			Main.inventoryScale = 0.755f;
			if (Utils.FloatIntersect(Main.mouseX, Main.mouseY, 0f, 0f, 73f, Main.instance.invBottom, 560f * Main.inventoryScale, 224f * Main.inventoryScale))
			{
				Main.player[Main.myPlayer].mouseInterface = true;
			}
			DrawName(spritebatch);
			DrawButtons(spritebatch);
			DrawSlots(spritebatch);
		}
		else
		{
			for (int i = 0; i < ButtonID.Count; i++)
			{
				ButtonScale[i] = 0.75f;
				ButtonHovered[i] = false;
			}
		}
	}

	private static void DrawName(SpriteBatch spritebatch)
	{
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_032e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0333: Unknown result type (might be due to invalid IL or missing references)
		//IL_0339: Unknown result type (might be due to invalid IL or missing references)
		//IL_033e: Unknown result type (might be due to invalid IL or missing references)
		//IL_034d: Unknown result type (might be due to invalid IL or missing references)
		Player player = Main.player[Main.myPlayer];
		string text = string.Empty;
		if (Main.editChest)
		{
			text = Main.npcChatText;
			Main.instance.textBlinkerCount++;
			if (Main.instance.textBlinkerCount >= 20)
			{
				if (Main.instance.textBlinkerState == 0)
				{
					Main.instance.textBlinkerState = 1;
				}
				else
				{
					Main.instance.textBlinkerState = 0;
				}
				Main.instance.textBlinkerCount = 0;
			}
			if (Main.instance.textBlinkerState == 1)
			{
				text += "|";
			}
			Main.instance.DrawWindowsIMEPanel(new Vector2(120f, 518f));
		}
		else if (player.chest > -1)
		{
			if (Main.chest[player.chest] == null)
			{
				Main.chest[player.chest] = new Chest();
			}
			Chest chest = Main.chest[player.chest];
			if (chest.name != "")
			{
				text = chest.name;
			}
			else
			{
				Tile tile = Main.tile[player.chestX, player.chestY];
				if (tile.type == 21)
				{
					text = Lang.chestType[tile.frameX / 36].Value;
				}
				else if (tile.type == 467 && tile.frameX / 36 == 4)
				{
					text = Lang.GetItemNameValue(3988);
				}
				else if (tile.type == 467)
				{
					text = Lang.chestType2[tile.frameX / 36].Value;
				}
				else if (tile.type == 88)
				{
					text = Lang.dresserType[tile.frameX / 54].Value;
				}
				else if (TileID.Sets.BasicChest[Main.tile[player.chestX, player.chestY].type] || TileID.Sets.BasicDresser[Main.tile[player.chestX, player.chestY].type])
				{
					text = TileLoader.DefaultContainerName(tile.type, tile.TileFrameX, tile.TileFrameY);
				}
			}
		}
		else if (player.chest == -2)
		{
			text = Lang.inter[32].Value;
		}
		else if (player.chest == -3)
		{
			text = Lang.inter[33].Value;
		}
		else if (player.chest == -4)
		{
			text = Lang.GetItemNameValue(3813);
		}
		else if (player.chest == -5)
		{
			text = Lang.GetItemNameValue(4076);
		}
		Color color = default(Color);
		((Color)(ref color))._002Ector((int)Main.mouseTextColor, (int)Main.mouseTextColor, (int)Main.mouseTextColor, (int)Main.mouseTextColor);
		color = Color.White * (1f - (255f - (float)(int)Main.mouseTextColor) / 255f * 0.5f);
		((Color)(ref color)).A = byte.MaxValue;
		Utils.WordwrapString(text, FontAssets.MouseText.Value, 200, 1, out var lineAmount);
		lineAmount++;
		for (int i = 0; i < lineAmount; i++)
		{
			ChatManager.DrawColorCodedStringWithShadow(spritebatch, FontAssets.MouseText.Value, text, new Vector2(504f, (float)(Main.instance.invBottom + i * 26)), color, 0f, Vector2.Zero, Vector2.One, -1f, 1.5f);
		}
	}

	private static void DrawButtons(SpriteBatch spritebatch)
	{
		for (int i = 0; i < ButtonID.Count; i++)
		{
			DrawButton(spritebatch, i, 506, Main.instance.invBottom + 40);
		}
	}

	private static void DrawButton(SpriteBatch spriteBatch, int ID, int X, int Y)
	{
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0165: Unknown result type (might be due to invalid IL or missing references)
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01da: Unknown result type (might be due to invalid IL or missing references)
		//IL_020e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0227: Unknown result type (might be due to invalid IL or missing references)
		//IL_025b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0260: Unknown result type (might be due to invalid IL or missing references)
		//IL_0267: Unknown result type (might be due to invalid IL or missing references)
		//IL_026e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0274: Unknown result type (might be due to invalid IL or missing references)
		//IL_0283: Unknown result type (might be due to invalid IL or missing references)
		//IL_0289: Unknown result type (might be due to invalid IL or missing references)
		//IL_028c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0291: Unknown result type (might be due to invalid IL or missing references)
		//IL_0244: Unknown result type (might be due to invalid IL or missing references)
		//IL_0249: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_030b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0321: Unknown result type (might be due to invalid IL or missing references)
		//IL_0339: Unknown result type (might be due to invalid IL or missing references)
		//IL_037c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0394: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0351: Unknown result type (might be due to invalid IL or missing references)
		//IL_0369: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ea: Unknown result type (might be due to invalid IL or missing references)
		Player player = Main.player[Main.myPlayer];
		if ((ID == 5 && player.chest < -1) || (ID == 6 && !Main.editChest))
		{
			UpdateHover(ID, hovering: false);
			return;
		}
		if (ID == 7 && player.chest != -5)
		{
			UpdateHover(ID, hovering: false);
			return;
		}
		int num = ID;
		if (ID == 7)
		{
			num = 5;
		}
		Y += num * 26;
		float num2 = ButtonScale[ID];
		string text = "";
		switch (ID)
		{
		case 0:
			text = Lang.inter[29].Value;
			break;
		case 1:
			text = Lang.inter[30].Value;
			break;
		case 2:
			text = Lang.inter[31].Value;
			break;
		case 3:
			text = Lang.inter[82].Value;
			break;
		case 5:
			text = Lang.inter[Main.editChest ? 47 : 61].Value;
			break;
		case 6:
			text = Lang.inter[63].Value;
			break;
		case 4:
			text = Lang.inter[122].Value;
			break;
		case 7:
			text = ((!player.IsVoidVaultEnabled) ? Language.GetTextValue("UI.ToggleBank4VacuumIsOff") : Language.GetTextValue("UI.ToggleBank4VacuumIsOn"));
			break;
		}
		Vector2 vector = FontAssets.MouseText.Value.MeasureString(text);
		Color color = new Color((int)Main.mouseTextColor, (int)Main.mouseTextColor, (int)Main.mouseTextColor, (int)Main.mouseTextColor) * num2;
		color = Color.White * 0.97f * (1f - (255f - (float)(int)Main.mouseTextColor) / 255f * 0.5f);
		((Color)(ref color)).A = byte.MaxValue;
		X += (int)(vector.X * num2 / 2f);
		bool flag = Utils.FloatIntersect(Main.mouseX, Main.mouseY, 0f, 0f, (float)X - vector.X / 2f, Y - 12, vector.X, 24f);
		if (ButtonHovered[ID])
		{
			flag = Utils.FloatIntersect(Main.mouseX, Main.mouseY, 0f, 0f, (float)X - vector.X / 2f - 10f, Y - 12, vector.X + 16f, 24f);
		}
		if (flag)
		{
			color = Main.OurFavoriteColor;
		}
		ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.MouseText.Value, text, new Vector2((float)X, (float)Y), color, 0f, vector / 2f, new Vector2(num2), -1f, 1.5f);
		vector *= num2;
		switch (ID)
		{
		case 0:
			UILinkPointNavigator.SetPosition(500, new Vector2((float)X - vector.X * num2 / 2f * 0.8f, (float)Y));
			break;
		case 1:
			UILinkPointNavigator.SetPosition(501, new Vector2((float)X - vector.X * num2 / 2f * 0.8f, (float)Y));
			break;
		case 2:
			UILinkPointNavigator.SetPosition(502, new Vector2((float)X - vector.X * num2 / 2f * 0.8f, (float)Y));
			break;
		case 5:
			UILinkPointNavigator.SetPosition(504, new Vector2((float)X, (float)Y));
			break;
		case 6:
			UILinkPointNavigator.SetPosition(504, new Vector2((float)X, (float)Y));
			break;
		case 3:
			UILinkPointNavigator.SetPosition(503, new Vector2((float)X - vector.X * num2 / 2f * 0.8f, (float)Y));
			break;
		case 4:
			UILinkPointNavigator.SetPosition(505, new Vector2((float)X - vector.X * num2 / 2f * 0.8f, (float)Y));
			break;
		case 7:
			UILinkPointNavigator.SetPosition(506, new Vector2((float)X - vector.X * num2 / 2f * 0.8f, (float)Y));
			break;
		}
		if (!flag)
		{
			UpdateHover(ID, hovering: false);
			return;
		}
		UpdateHover(ID, hovering: true);
		if (PlayerInput.IgnoreMouseInterface)
		{
			return;
		}
		player.mouseInterface = true;
		if (Main.mouseLeft && Main.mouseLeftRelease)
		{
			switch (ID)
			{
			case 0:
				LootAll();
				break;
			case 1:
				DepositAll(ContainerTransferContext.FromUnknown(player));
				break;
			case 2:
				QuickStack(ContainerTransferContext.FromUnknown(player));
				break;
			case 5:
				RenameChest();
				break;
			case 6:
				RenameChestCancel();
				break;
			case 3:
				Restock();
				break;
			case 4:
				ItemSorting.SortChest();
				break;
			case 7:
				ToggleVacuum();
				break;
			}
			Recipe.FindRecipes();
		}
	}

	private static void ToggleVacuum()
	{
		Player obj = Main.player[Main.myPlayer];
		obj.IsVoidVaultEnabled = !obj.IsVoidVaultEnabled;
	}

	private static void DrawSlots(SpriteBatch spriteBatch)
	{
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
		Player player = Main.player[Main.myPlayer];
		int context = 0;
		Item[] inv = null;
		if (player.chest > -1)
		{
			context = 3;
			inv = Main.chest[player.chest].item;
		}
		if (player.chest == -2)
		{
			context = 4;
			inv = player.bank.item;
		}
		if (player.chest == -3)
		{
			context = 4;
			inv = player.bank2.item;
		}
		if (player.chest == -4)
		{
			context = 4;
			inv = player.bank3.item;
		}
		if (player.chest == -5)
		{
			context = 32;
			inv = player.bank4.item;
		}
		Main.inventoryScale = 0.755f;
		if (Utils.FloatIntersect(Main.mouseX, Main.mouseY, 0f, 0f, 73f, Main.instance.invBottom, 560f * Main.inventoryScale, 224f * Main.inventoryScale) && !PlayerInput.IgnoreMouseInterface)
		{
			player.mouseInterface = true;
		}
		for (int i = 0; i < 10; i++)
		{
			for (int j = 0; j < 4; j++)
			{
				int num = (int)(73f + (float)(i * 56) * Main.inventoryScale);
				int num2 = (int)((float)Main.instance.invBottom + (float)(j * 56) * Main.inventoryScale);
				int slot = i + j * 10;
				new Color(100, 100, 100, 100);
				if (Utils.FloatIntersect(Main.mouseX, Main.mouseY, 0f, 0f, num, num2, (float)TextureAssets.InventoryBack.Width() * Main.inventoryScale, (float)TextureAssets.InventoryBack.Height() * Main.inventoryScale) && !PlayerInput.IgnoreMouseInterface)
				{
					player.mouseInterface = true;
					ItemSlot.Handle(inv, context, slot);
				}
				ItemSlot.Draw(spriteBatch, inv, context, slot, new Vector2((float)num, (float)num2));
			}
		}
	}

	public static void LootAll()
	{
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0246: Unknown result type (might be due to invalid IL or missing references)
		//IL_024b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
		GetItemSettings lootAllSettings = GetItemSettings.LootAllSettings;
		Player player = Main.player[Main.myPlayer];
		if (player.chest > -1)
		{
			GetItemSettings lootAllSettingsRegularChest = GetItemSettings.LootAllSettingsRegularChest;
			Chest chest = Main.chest[player.chest];
			for (int i = 0; i < 40; i++)
			{
				if (chest.item[i].type > 0)
				{
					chest.item[i].position = player.Center;
					chest.item[i] = player.GetItem(Main.myPlayer, chest.item[i], lootAllSettingsRegularChest);
					if (Main.netMode == 1)
					{
						NetMessage.SendData(32, -1, -1, null, player.chest, i);
					}
				}
			}
			return;
		}
		if (player.chest == -3)
		{
			for (int j = 0; j < 40; j++)
			{
				if (player.bank2.item[j].type > 0)
				{
					player.bank2.item[j].position = player.Center;
					player.bank2.item[j] = player.GetItem(Main.myPlayer, player.bank2.item[j], lootAllSettings);
				}
			}
			return;
		}
		if (player.chest == -4)
		{
			for (int k = 0; k < 40; k++)
			{
				if (player.bank3.item[k].type > 0)
				{
					player.bank3.item[k].position = player.Center;
					player.bank3.item[k] = player.GetItem(Main.myPlayer, player.bank3.item[k], lootAllSettings);
				}
			}
			return;
		}
		if (player.chest == -5)
		{
			for (int l = 0; l < 40; l++)
			{
				if (player.bank4.item[l].type > 0 && !player.bank4.item[l].favorited)
				{
					player.bank4.item[l].position = player.Center;
					player.bank4.item[l] = player.GetItem(Main.myPlayer, player.bank4.item[l], lootAllSettings);
				}
			}
			return;
		}
		for (int m = 0; m < 40; m++)
		{
			if (player.bank.item[m].type > 0)
			{
				player.bank.item[m].position = player.Center;
				player.bank.item[m] = player.GetItem(Main.myPlayer, player.bank.item[m], lootAllSettings);
			}
		}
	}

	public static void DepositAll(ContainerTransferContext context)
	{
		Player player = Main.player[Main.myPlayer];
		if (player.chest > -1)
		{
			MoveCoins(player.inventory, Main.chest[player.chest].item, context);
		}
		else if (player.chest == -3)
		{
			MoveCoins(player.inventory, player.bank2.item, context);
		}
		else if (player.chest == -4)
		{
			MoveCoins(player.inventory, player.bank3.item, context);
		}
		else if (player.chest == -5)
		{
			MoveCoins(player.inventory, player.bank4.item, context);
		}
		else
		{
			MoveCoins(player.inventory, player.bank.item, context);
		}
		for (int num = 49; num >= 10; num--)
		{
			if (player.inventory[num].stack > 0 && player.inventory[num].type > 0 && !player.inventory[num].favorited)
			{
				if (player.inventory[num].maxStack > 1)
				{
					for (int i = 0; i < 40; i++)
					{
						int numTransferred;
						if (player.chest > -1)
						{
							Chest chest = Main.chest[player.chest];
							if (chest.item[i].stack >= chest.item[i].maxStack || !player.inventory[num].IsTheSameAs(chest.item[i]) || !ItemLoader.TryStackItems(chest.item[i], player.inventory[num], out numTransferred))
							{
								continue;
							}
							SoundEngine.PlaySound(7);
							if (player.inventory[num].stack <= 0)
							{
								player.inventory[num].SetDefaults();
								if (Main.netMode == 1)
								{
									NetMessage.SendData(32, -1, -1, null, player.chest, i);
								}
								break;
							}
							if (chest.item[i].type == 0)
							{
								chest.item[i] = player.inventory[num].Clone();
								player.inventory[num].SetDefaults();
							}
							if (Main.netMode == 1)
							{
								NetMessage.SendData(32, -1, -1, null, player.chest, i);
							}
						}
						else if (player.chest == -3)
						{
							if (player.bank2.item[i].stack < player.bank2.item[i].maxStack && player.inventory[num].IsTheSameAs(player.bank2.item[i]) && ItemLoader.TryStackItems(player.bank2.item[i], player.inventory[num], out numTransferred))
							{
								SoundEngine.PlaySound(7);
								if (player.inventory[num].stack <= 0)
								{
									player.inventory[num].SetDefaults();
									break;
								}
								if (player.bank2.item[i].type == 0)
								{
									player.bank2.item[i] = player.inventory[num].Clone();
									player.inventory[num].SetDefaults();
								}
							}
						}
						else if (player.chest == -4)
						{
							if (player.bank3.item[i].stack < player.bank3.item[i].maxStack && player.inventory[num].IsTheSameAs(player.bank3.item[i]) && ItemLoader.TryStackItems(player.bank3.item[i], player.inventory[num], out numTransferred))
							{
								SoundEngine.PlaySound(7);
								if (player.inventory[num].stack <= 0)
								{
									player.inventory[num].SetDefaults();
									break;
								}
								if (player.bank3.item[i].type == 0)
								{
									player.bank3.item[i] = player.inventory[num].Clone();
									player.inventory[num].SetDefaults();
								}
							}
						}
						else if (player.chest == -5)
						{
							if (player.bank4.item[i].stack < player.bank4.item[i].maxStack && player.inventory[num].IsTheSameAs(player.bank4.item[i]) && ItemLoader.TryStackItems(player.bank4.item[i], player.inventory[num], out numTransferred))
							{
								SoundEngine.PlaySound(7);
								if (player.inventory[num].stack <= 0)
								{
									player.inventory[num].SetDefaults();
									break;
								}
								if (player.bank4.item[i].type == 0)
								{
									player.bank4.item[i] = player.inventory[num].Clone();
									player.inventory[num].SetDefaults();
								}
							}
						}
						else if (player.bank.item[i].stack < player.bank.item[i].maxStack && player.inventory[num].IsTheSameAs(player.bank.item[i]) && ItemLoader.TryStackItems(player.bank.item[i], player.inventory[num], out numTransferred))
						{
							SoundEngine.PlaySound(7);
							if (player.inventory[num].stack <= 0)
							{
								player.inventory[num].SetDefaults();
								break;
							}
							if (player.bank.item[i].type == 0)
							{
								player.bank.item[i] = player.inventory[num].Clone();
								player.inventory[num].SetDefaults();
							}
						}
					}
				}
				if (player.inventory[num].stack > 0)
				{
					if (player.chest > -1)
					{
						for (int j = 0; j < 40; j++)
						{
							if (Main.chest[player.chest].item[j].stack == 0)
							{
								SoundEngine.PlaySound(7);
								Main.chest[player.chest].item[j] = player.inventory[num].Clone();
								player.inventory[num].SetDefaults();
								if (Main.netMode == 1)
								{
									NetMessage.SendData(32, -1, -1, null, player.chest, j);
								}
								break;
							}
						}
					}
					else if (player.chest == -3)
					{
						for (int k = 0; k < 40; k++)
						{
							if (player.bank2.item[k].stack == 0)
							{
								SoundEngine.PlaySound(7);
								player.bank2.item[k] = player.inventory[num].Clone();
								player.inventory[num].SetDefaults();
								break;
							}
						}
					}
					else if (player.chest == -4)
					{
						for (int l = 0; l < 40; l++)
						{
							if (player.bank3.item[l].stack == 0)
							{
								SoundEngine.PlaySound(7);
								player.bank3.item[l] = player.inventory[num].Clone();
								player.inventory[num].SetDefaults();
								break;
							}
						}
					}
					else if (player.chest == -5)
					{
						for (int m = 0; m < 40; m++)
						{
							if (player.bank4.item[m].stack == 0)
							{
								SoundEngine.PlaySound(7);
								player.bank4.item[m] = player.inventory[num].Clone();
								player.inventory[num].SetDefaults();
								break;
							}
						}
					}
					else
					{
						for (int n = 0; n < 40; n++)
						{
							if (player.bank.item[n].stack == 0)
							{
								SoundEngine.PlaySound(7);
								player.bank.item[n] = player.inventory[num].Clone();
								player.inventory[num].SetDefaults();
								break;
							}
						}
					}
				}
			}
		}
	}

	public static void QuickStack(ContainerTransferContext context, bool voidStack = false)
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_0359: Unknown result type (might be due to invalid IL or missing references)
		//IL_035a: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0555: Unknown result type (might be due to invalid IL or missing references)
		//IL_0556: Unknown result type (might be due to invalid IL or missing references)
		Player player = Main.player[Main.myPlayer];
		Item[] array = player.inventory;
		if (voidStack)
		{
			array = player.bank4.item;
		}
		Vector2 center = player.Center;
		Vector2 containerWorldPosition = context.GetContainerWorldPosition();
		bool canVisualizeTransfers = context.CanVisualizeTransfers;
		if (!voidStack && player.chest == -5)
		{
			long coinsMoved = MoveCoins(array, player.bank4.item, context);
			if (canVisualizeTransfers)
			{
				Chest.VisualizeChestTransfer_CoinsBatch(center, containerWorldPosition, coinsMoved);
			}
		}
		else if (player.chest == -4)
		{
			long coinsMoved2 = MoveCoins(array, player.bank3.item, context);
			if (canVisualizeTransfers)
			{
				Chest.VisualizeChestTransfer_CoinsBatch(center, containerWorldPosition, coinsMoved2);
			}
		}
		else if (player.chest == -3)
		{
			long coinsMoved3 = MoveCoins(array, player.bank2.item, context);
			if (canVisualizeTransfers)
			{
				Chest.VisualizeChestTransfer_CoinsBatch(center, containerWorldPosition, coinsMoved3);
			}
		}
		else if (player.chest == -2)
		{
			long coinsMoved4 = MoveCoins(array, player.bank.item, context);
			if (canVisualizeTransfers)
			{
				Chest.VisualizeChestTransfer_CoinsBatch(center, containerWorldPosition, coinsMoved4);
			}
		}
		Item[] item = player.bank.item;
		if (player.chest > -1)
		{
			item = Main.chest[player.chest].item;
		}
		else if (player.chest == -2)
		{
			item = player.bank.item;
		}
		else if (player.chest == -3)
		{
			item = player.bank2.item;
		}
		else if (player.chest == -4)
		{
			item = player.bank3.item;
		}
		else if (!voidStack && player.chest == -5)
		{
			item = player.bank4.item;
		}
		List<int> list = new List<int>();
		List<int> list2 = new List<int>();
		List<int> list3 = new List<int>();
		Dictionary<int, int> dictionary = new Dictionary<int, int>();
		List<int> list4 = new List<int>();
		bool[] array2 = new bool[item.Length];
		for (int i = 0; i < 40; i++)
		{
			if (item[i].type > 0 && item[i].stack > 0 && (item[i].type < 71 || item[i].type > 74))
			{
				list2.Add(i);
				list.Add(item[i].netID);
			}
			if (item[i].type == 0 || item[i].stack <= 0)
			{
				list3.Add(i);
			}
		}
		int num = 50;
		int num2 = 10;
		if (player.chest <= -2)
		{
			num += 4;
		}
		if (voidStack)
		{
			num2 = 0;
			num = 40;
		}
		for (int j = num2; j < num; j++)
		{
			if (list.Contains(array[j].netID) && !array[j].favorited)
			{
				dictionary.Add(j, array[j].netID);
			}
		}
		for (int k = 0; k < list2.Count; k++)
		{
			int num3 = list2[k];
			int netID = item[num3].netID;
			foreach (KeyValuePair<int, int> item2 in dictionary)
			{
				if (item2.Value == netID && array[item2.Key].netID == netID)
				{
					int num4 = array[item2.Key].stack;
					int num5 = item[num3].maxStack - item[num3].stack;
					if (num5 == 0)
					{
						break;
					}
					if (num4 > num5)
					{
						num4 = num5;
					}
					SoundEngine.PlaySound(7);
					ItemLoader.TryStackItems(item[num3], array[item2.Key], out num4);
					if (canVisualizeTransfers && num4 > 0)
					{
						Chest.VisualizeChestTransfer(center, containerWorldPosition, item[num3], num4);
					}
					array2[num3] = true;
				}
			}
		}
		foreach (KeyValuePair<int, int> item3 in dictionary)
		{
			if (array[item3.Key].stack == 0)
			{
				list4.Add(item3.Key);
			}
		}
		foreach (int item4 in list4)
		{
			dictionary.Remove(item4);
		}
		for (int l = 0; l < list3.Count; l++)
		{
			int num6 = list3[l];
			bool flag = true;
			int num7 = item[num6].netID;
			if (num7 >= 71 && num7 <= 74)
			{
				continue;
			}
			foreach (KeyValuePair<int, int> item5 in dictionary)
			{
				if ((item5.Value != num7 || array[item5.Key].netID != num7) && (!flag || array[item5.Key].stack <= 0))
				{
					continue;
				}
				SoundEngine.PlaySound(7);
				if (flag)
				{
					num7 = item5.Value;
					item[num6] = array[item5.Key];
					array[item5.Key] = new Item();
					if (canVisualizeTransfers)
					{
						Chest.VisualizeChestTransfer(center, containerWorldPosition, item[num6], item[num6].stack);
					}
				}
				else
				{
					int num8 = array[item5.Key].stack;
					int num9 = item[num6].maxStack - item[num6].stack;
					if (num9 == 0)
					{
						break;
					}
					if (num8 > num9)
					{
						num8 = num9;
					}
					ItemLoader.TryStackItems(item[num6], array[item5.Key], out num8);
					if (canVisualizeTransfers && num8 > 0)
					{
						Chest.VisualizeChestTransfer(center, containerWorldPosition, item[num6], num8);
					}
					if (array[item5.Key].stack == 0)
					{
						array[item5.Key] = new Item();
					}
				}
				array2[num6] = true;
				flag = false;
			}
		}
		if (Main.netMode == 1 && player.chest >= 0)
		{
			for (int m = 0; m < array2.Length; m++)
			{
				NetMessage.SendData(32, -1, -1, null, player.chest, m);
			}
		}
		list.Clear();
		list2.Clear();
		list3.Clear();
		dictionary.Clear();
		list4.Clear();
	}

	public static void RenameChest()
	{
		Player player = Main.player[Main.myPlayer];
		if (!Main.editChest)
		{
			IngameFancyUI.OpenVirtualKeyboard(2);
		}
		else
		{
			RenameChestSubmit(player);
		}
	}

	public static void RenameChestSubmit(Player player)
	{
		SoundEngine.PlaySound(12);
		Main.editChest = false;
		int chest = player.chest;
		if (chest < 0)
		{
			return;
		}
		if (Main.npcChatText == Main.defaultChestName)
		{
			Main.npcChatText = "";
		}
		if (Main.chest[chest].name != Main.npcChatText)
		{
			Main.chest[chest].name = Main.npcChatText;
			if (Main.netMode == 1)
			{
				player.editedChestName = true;
			}
		}
	}

	public static void RenameChestCancel()
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		SoundEngine.PlaySound(12);
		Main.editChest = false;
		Main.npcChatText = string.Empty;
		Keys val = (Keys)27;
		Main.blockKey = ((object)(Keys)(ref val)).ToString();
	}

	public static void Restock()
	{
		Player player = Main.player[Main.myPlayer];
		Item[] inventory = player.inventory;
		Item[] item = player.bank.item;
		if (player.chest > -1)
		{
			item = Main.chest[player.chest].item;
		}
		else if (player.chest == -2)
		{
			item = player.bank.item;
		}
		else if (player.chest == -3)
		{
			item = player.bank2.item;
		}
		else if (player.chest == -4)
		{
			item = player.bank3.item;
		}
		else if (player.chest == -5)
		{
			item = player.bank4.item;
		}
		HashSet<int> hashSet = new HashSet<int>();
		List<int> list = new List<int>();
		List<int> list2 = new List<int>();
		for (int num = 57; num >= 0; num--)
		{
			if ((num < 50 || num >= 54) && (inventory[num].type < 71 || inventory[num].type > 74))
			{
				if (inventory[num].stack > 0 && inventory[num].maxStack > 1)
				{
					hashSet.Add(inventory[num].netID);
					if (inventory[num].stack < inventory[num].maxStack)
					{
						list.Add(num);
					}
				}
				else if (inventory[num].stack == 0 || inventory[num].netID == 0 || inventory[num].type == 0)
				{
					list2.Add(num);
				}
			}
		}
		bool flag = false;
		for (int i = 0; i < item.Length; i++)
		{
			if (item[i].stack < 1 || !hashSet.Contains(item[i].netID))
			{
				continue;
			}
			bool flag2 = false;
			for (int j = 0; j < list.Count; j++)
			{
				int num2 = list[j];
				int context = 0;
				if (num2 >= 50)
				{
					context = 2;
				}
				if (inventory[num2].netID != item[i].netID || ItemSlot.PickItemMovementAction(inventory, context, num2, item[i]) == -1 || !ItemLoader.TryStackItems(inventory[num2], item[i], out var _))
				{
					continue;
				}
				flag = true;
				if (inventory[num2].stack == inventory[num2].maxStack)
				{
					if (Main.netMode == 1 && Main.player[Main.myPlayer].chest > -1)
					{
						NetMessage.SendData(32, -1, -1, null, Main.player[Main.myPlayer].chest, i);
					}
					list.RemoveAt(j);
					j--;
				}
				if (item[i].stack == 0)
				{
					item[i] = new Item();
					flag2 = true;
					if (Main.netMode == 1 && Main.player[Main.myPlayer].chest > -1)
					{
						NetMessage.SendData(32, -1, -1, null, Main.player[Main.myPlayer].chest, i);
					}
					break;
				}
			}
			if (flag2 || list2.Count <= 0 || item[i].ammo == 0)
			{
				continue;
			}
			for (int k = 0; k < list2.Count; k++)
			{
				int context2 = 0;
				if (list2[k] >= 50)
				{
					context2 = 2;
				}
				if (ItemSlot.PickItemMovementAction(inventory, context2, list2[k], item[i]) != -1)
				{
					Utils.Swap(ref inventory[list2[k]], ref item[i]);
					if (Main.netMode == 1 && Main.player[Main.myPlayer].chest > -1)
					{
						NetMessage.SendData(32, -1, -1, null, Main.player[Main.myPlayer].chest, i);
					}
					list.Add(list2[k]);
					list2.RemoveAt(k);
					flag = true;
					break;
				}
			}
		}
		if (flag)
		{
			SoundEngine.PlaySound(7);
		}
	}

	public static long MoveCoins(Item[] pInv, Item[] cInv, ContainerTransferContext context)
	{
		bool flag = false;
		int[] array = new int[4];
		List<int> list = new List<int>();
		List<int> list2 = new List<int>();
		bool flag2 = false;
		int[] array2 = new int[40];
		bool overFlowing;
		long num = Utils.CoinsCount(out overFlowing, pInv);
		for (int i = 0; i < cInv.Length; i++)
		{
			array2[i] = -1;
			if (cInv[i].stack < 1 || cInv[i].type < 1)
			{
				list2.Add(i);
				cInv[i] = new Item();
			}
			if (cInv[i] != null && cInv[i].stack > 0)
			{
				int num5 = 0;
				if (cInv[i].type == 71)
				{
					num5 = 1;
				}
				if (cInv[i].type == 72)
				{
					num5 = 2;
				}
				if (cInv[i].type == 73)
				{
					num5 = 3;
				}
				if (cInv[i].type == 74)
				{
					num5 = 4;
				}
				array2[i] = num5 - 1;
				if (num5 > 0)
				{
					array[num5 - 1] += cInv[i].stack;
					list2.Add(i);
					cInv[i] = new Item();
					flag2 = true;
				}
			}
		}
		if (!flag2)
		{
			return 0L;
		}
		for (int j = 0; j < pInv.Length; j++)
		{
			if (j != 58 && pInv[j] != null && pInv[j].stack > 0 && !pInv[j].favorited)
			{
				int num6 = 0;
				if (pInv[j].type == 71)
				{
					num6 = 1;
				}
				if (pInv[j].type == 72)
				{
					num6 = 2;
				}
				if (pInv[j].type == 73)
				{
					num6 = 3;
				}
				if (pInv[j].type == 74)
				{
					num6 = 4;
				}
				if (num6 > 0)
				{
					flag = true;
					array[num6 - 1] += pInv[j].stack;
					list.Add(j);
					pInv[j] = new Item();
				}
			}
		}
		for (int k = 0; k < 3; k++)
		{
			while (array[k] >= 100)
			{
				array[k] -= 100;
				array[k + 1]++;
			}
		}
		for (int l = 0; l < 40; l++)
		{
			if (array2[l] < 0 || cInv[l].type != 0)
			{
				continue;
			}
			int num7 = l;
			int num8 = array2[l];
			if (array[num8] > 0)
			{
				cInv[num7].SetDefaults(71 + num8);
				cInv[num7].stack = array[num8];
				if (cInv[num7].stack > cInv[num7].maxStack)
				{
					cInv[num7].stack = cInv[num7].maxStack;
				}
				array[num8] -= cInv[num7].stack;
				array2[l] = -1;
			}
			if (Main.netMode == 1 && Main.player[Main.myPlayer].chest > -1)
			{
				NetMessage.SendData(32, -1, -1, null, Main.player[Main.myPlayer].chest, num7);
			}
			list2.Remove(num7);
		}
		for (int m = 0; m < 40; m++)
		{
			if (array2[m] < 0 || cInv[m].type != 0)
			{
				continue;
			}
			int num9 = m;
			int num10 = 3;
			while (num10 >= 0)
			{
				if (array[num10] > 0)
				{
					cInv[num9].SetDefaults(71 + num10);
					cInv[num9].stack = array[num10];
					if (cInv[num9].stack > cInv[num9].maxStack)
					{
						cInv[num9].stack = cInv[num9].maxStack;
					}
					array[num10] -= cInv[num9].stack;
					array2[m] = -1;
					break;
				}
				if (array[num10] == 0)
				{
					num10--;
				}
			}
			if (Main.netMode == 1 && Main.player[Main.myPlayer].chest > -1)
			{
				NetMessage.SendData(32, -1, -1, null, Main.player[Main.myPlayer].chest, num9);
			}
			list2.Remove(num9);
		}
		while (list2.Count > 0)
		{
			int num11 = list2[0];
			int num12 = 3;
			while (num12 >= 0)
			{
				if (array[num12] > 0)
				{
					cInv[num11].SetDefaults(71 + num12);
					cInv[num11].stack = array[num12];
					if (cInv[num11].stack > cInv[num11].maxStack)
					{
						cInv[num11].stack = cInv[num11].maxStack;
					}
					array[num12] -= cInv[num11].stack;
					break;
				}
				if (array[num12] == 0)
				{
					num12--;
				}
			}
			if (Main.netMode == 1 && Main.player[Main.myPlayer].chest > -1)
			{
				NetMessage.SendData(32, -1, -1, null, Main.player[Main.myPlayer].chest, list2[0]);
			}
			list2.RemoveAt(0);
		}
		int num2 = 3;
		while (num2 >= 0 && list.Count > 0)
		{
			int num3 = list[0];
			if (array[num2] > 0)
			{
				pInv[num3].SetDefaults(71 + num2);
				pInv[num3].stack = array[num2];
				if (pInv[num3].stack > pInv[num3].maxStack)
				{
					pInv[num3].stack = pInv[num3].maxStack;
				}
				array[num2] -= pInv[num3].stack;
				flag = false;
				list.RemoveAt(0);
			}
			if (array[num2] == 0)
			{
				num2--;
			}
		}
		if (flag)
		{
			SoundEngine.PlaySound(7);
		}
		bool overFlowing2;
		long num4 = Utils.CoinsCount(out overFlowing2, pInv);
		if (overFlowing || overFlowing2)
		{
			return 0L;
		}
		return num - num4;
	}

	public static bool TryPlacingInChest(Item I, bool justCheck, int itemSlotContext)
	{
		GetContainerUsageInfo(out var sync, out var chestinv);
		if (IsBlockedFromTransferIntoChest(I, chestinv))
		{
			return false;
		}
		Player player = Main.player[Main.myPlayer];
		bool flag = false;
		if (I.maxStack > 1)
		{
			for (int i = 0; i < 40; i++)
			{
				if (chestinv[i].stack >= chestinv[i].maxStack || !I.IsTheSameAs(chestinv[i]) || !ItemLoader.CanStack(chestinv[i], I))
				{
					continue;
				}
				int num = I.stack;
				if (I.stack + chestinv[i].stack > chestinv[i].maxStack)
				{
					num = chestinv[i].maxStack - chestinv[i].stack;
				}
				if (justCheck)
				{
					flag = flag || num > 0;
					break;
				}
				ItemLoader.StackItems(chestinv[i], I, out var _);
				SoundEngine.PlaySound(7);
				if (I.stack <= 0)
				{
					I.SetDefaults();
					if (sync)
					{
						NetMessage.SendData(32, -1, -1, null, player.chest, i);
					}
					break;
				}
				if (chestinv[i].type == 0)
				{
					chestinv[i] = I.Clone();
					I.SetDefaults();
				}
				if (sync)
				{
					NetMessage.SendData(32, -1, -1, null, player.chest, i);
				}
			}
		}
		if (I.stack > 0)
		{
			for (int j = 0; j < 40; j++)
			{
				if (chestinv[j].stack != 0)
				{
					continue;
				}
				if (justCheck)
				{
					flag = true;
					break;
				}
				SoundEngine.PlaySound(7);
				chestinv[j] = I.Clone();
				I.SetDefaults();
				ItemSlot.AnnounceTransfer(new ItemSlot.ItemTransferInfo(chestinv[j], 0, 3));
				if (sync)
				{
					NetMessage.SendData(32, -1, -1, null, player.chest, j);
				}
				break;
			}
		}
		return flag;
	}

	public static void GetContainerUsageInfo(out bool sync, out Item[] chestinv)
	{
		sync = false;
		Player player = Main.player[Main.myPlayer];
		chestinv = player.bank.item;
		if (player.chest > -1)
		{
			chestinv = Main.chest[player.chest].item;
			sync = Main.netMode == 1;
		}
		else if (player.chest == -2)
		{
			chestinv = player.bank.item;
		}
		else if (player.chest == -3)
		{
			chestinv = player.bank2.item;
		}
		else if (player.chest == -4)
		{
			chestinv = player.bank3.item;
		}
		else if (player.chest == -5)
		{
			chestinv = player.bank4.item;
		}
	}

	public static bool IsBlockedFromTransferIntoChest(Item item, Item[] container)
	{
		if (item.type == 3213 && item.favorited && container == Main.LocalPlayer.bank.item)
		{
			return true;
		}
		if ((item.type == 4131 || item.type == 5325) && item.favorited && container == Main.LocalPlayer.bank4.item)
		{
			return true;
		}
		return false;
	}
}
