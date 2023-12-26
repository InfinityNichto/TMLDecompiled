using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.UI;
using Terraria.UI.Chat;

namespace Terraria.GameContent.UI.Chat;

public class ItemTagHandler : ITagHandler
{
	private class ItemSnippet : TextSnippet
	{
		private Item _item;

		public ItemSnippet(Item item)
		{
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			_item = item;
			Color = ItemRarity.GetColor(item.rare);
		}

		public override void OnHover()
		{
			Main.HoverItem = _item.Clone();
			Main.instance.MouseText(_item.Name, _item.rare, 0);
		}

		public override bool UniqueDraw(bool justCheckingString, out Vector2 size, SpriteBatch spriteBatch, Vector2 position = default(Vector2), Color color = default(Color), float scale = 1f)
		{
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			float num = 1f;
			if (Main.netMode != 2 && !Main.dedServ)
			{
				Main.instance.LoadItem(_item.type);
				Texture2D value = TextureAssets.Item[_item.type].Value;
				if (Main.itemAnimations[_item.type] != null)
				{
					Main.itemAnimations[_item.type].GetFrame(value);
				}
				else
				{
					value.Frame();
				}
			}
			num = scale * 0.75f;
			if (!justCheckingString && (((Color)(ref color)).R != 0 || ((Color)(ref color)).G != 0 || ((Color)(ref color)).B != 0))
			{
				float inventoryScale = Main.inventoryScale;
				Main.inventoryScale = num;
				ItemSlot.Draw(spriteBatch, ref _item, 14, position - new Vector2(10f) * num, Color.White);
				Main.inventoryScale = inventoryScale;
			}
			size = new Vector2(32f) * num;
			return true;
		}

		public override float GetStringLength(DynamicSpriteFont font)
		{
			return 32f * Scale * 0.65f;
		}
	}

	TextSnippet ITagHandler.Parse(string text, Color baseColor, string options)
	{
		Item item = new Item();
		if (int.TryParse(text, out var result) && result < ItemLoader.ItemCount)
		{
			item.netDefaults(result);
		}
		if (ItemID.Search.TryGetId(text, out result))
		{
			item.netDefaults(result);
		}
		if (item.type <= 0)
		{
			return new TextSnippet(text);
		}
		item.stack = 1;
		if (options != null)
		{
			string[] array = options.Split(',');
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].Length == 0)
				{
					continue;
				}
				switch (array[i][0])
				{
				case 'd':
					item = ItemIO.FromBase64(array[i].Substring(1));
					break;
				case 's':
				case 'x':
				{
					if (int.TryParse(array[i].Substring(1), out var result3))
					{
						item.stack = Utils.Clamp(result3, 1, item.maxStack);
					}
					break;
				}
				case 'p':
				{
					if (int.TryParse(array[i].Substring(1), out var result2))
					{
						item.Prefix((byte)Utils.Clamp(result2, 0, PrefixLoader.PrefixCount));
					}
					break;
				}
				}
			}
		}
		string text2 = "";
		if (item.stack > 1)
		{
			text2 = " (" + item.stack + ")";
		}
		return new ItemSnippet(item)
		{
			Text = "[" + item.AffixName() + text2 + "]",
			CheckForHover = true,
			DeleteWhole = true
		};
	}

	public static string GenerateTag(Item I)
	{
		string text = "[i";
		if (ItemLoader.NeedsModSaving(I) || ItemIO.SaveGlobals(I) != null)
		{
			text = text + "/d" + ItemIO.ToBase64(I);
		}
		else
		{
			if (I.prefix != 0)
			{
				text = text + "/p" + I.prefix;
			}
			if (I.stack != 1)
			{
				text = text + "/s" + I.stack;
			}
		}
		return text + ":" + I.netID + "]";
	}
}
