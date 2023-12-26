using System;
using Microsoft.Xna.Framework;
using Terraria.GameContent;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Terraria;

/// <summary>
/// Represents an in-world floating text object. <br />
/// <VariousTextOptionsSummary>
///         <br />Summary of options to display text to the user:<br />
///         • <see cref="M:Terraria.Main.NewText(System.String,System.Byte,System.Byte,System.Byte)" /> to display a message in chat. <br />
///         • <see cref="T:Terraria.CombatText" /> to display floating damage numbers in-game. Used for damage and healing numbers. <br />
///         • <see cref="T:Terraria.PopupText" /> to display non-overlapping floating in-game text. Used for reforge and item pickup messages. <br />
///     </VariousTextOptionsSummary>
/// </summary>
public class PopupText
{
	/// <summary>
	/// The position of this <see cref="T:Terraria.PopupText" /> in world coordinates.
	/// </summary>
	public Vector2 position;

	/// <summary>
	/// The velocity of this <see cref="T:Terraria.PopupText" /> in world coordinates per tick.
	/// </summary>
	public Vector2 velocity;

	/// <summary>
	/// The opacity of this <see cref="T:Terraria.PopupText" /> in the range [0f, 1f], where <c>0f</c> is transparent and <c>1f</c> is opaque.
	/// </summary>
	public float alpha;

	/// <summary>
	/// The direction this <see cref="T:Terraria.PopupText" />'s <see cref="F:Terraria.PopupText.alpha" /> changes in.
	/// </summary>
	public int alphaDir = 1;

	/// <summary>
	/// The text displayed by this <see cref="T:Terraria.PopupText" />.
	/// </summary>
	public string name;

	/// <summary>
	/// The optional stack size appended to <see cref="F:Terraria.PopupText.name" />.
	/// <br /> Will only be displayed is <c><see cref="F:Terraria.PopupText.stack" /> &gt; 1</c>.
	/// </summary>
	public long stack;

	/// <summary>
	/// The scale this <see cref="T:Terraria.PopupText" /> draws at.
	/// </summary>
	public float scale = 1f;

	/// <summary>
	/// The clockwise rotation of this <see cref="T:Terraria.PopupText" /> in radians.
	/// </summary>
	public float rotation;

	/// <summary>
	/// The color of this <see cref="T:Terraria.PopupText" />'s text.
	/// </summary>
	public Color color;

	/// <summary>
	/// If <see langword="true" />, this <see cref="T:Terraria.PopupText" /> is visible in the world.
	/// </summary>
	public bool active;

	/// <summary>
	/// The time in ticks this <see cref="T:Terraria.PopupText" /> will remain for until it starts to disappear.
	/// </summary>
	public int lifeTime;

	/// <summary>
	/// The default <see cref="F:Terraria.PopupText.lifeTime" /> of a <see cref="T:Terraria.PopupText" />.
	/// </summary>
	public static int activeTime = 60;

	/// <summary>
	/// The number of <see cref="F:Terraria.PopupText.active" /> <see cref="T:Terraria.PopupText" />s in <see cref="F:Terraria.Main.popupText" />.
	/// <br /> Assigned after <see cref="M:Terraria.PopupText.UpdateItemText" /> runs.
	/// </summary>
	public static int numActive;

	/// <summary>
	/// If <see langword="true" />, this <see cref="T:Terraria.PopupText" /> can't be modified when creating a new item <see cref="T:Terraria.PopupText" />.
	/// </summary>
	public bool NoStack;

	/// <summary>
	/// If <see langword="true" />, this <see cref="T:Terraria.PopupText" /> is specifically for coins.
	/// </summary>
	public bool coinText;

	/// <summary>
	/// The value of coins this <see cref="T:Terraria.PopupText" /> represents in the range [0, 999999999].
	/// </summary>
	public long coinValue;

	/// <summary>
	/// The index in <see cref="F:Terraria.Main.popupText" /> of the last known sonar text.
	/// <br /> Assign and clear using <see cref="M:Terraria.PopupText.AssignAsSonarText(System.Int32)" /> and <see cref="M:Terraria.PopupText.ClearSonarText" />.
	/// </summary>
	public static int sonarText = -1;

	/// <summary>
	/// If <see langword="true" />, this <see cref="T:Terraria.PopupText" /> will draw in the Expert Mode rarity color.
	/// </summary>
	public bool expert;

	/// <summary>
	/// If <see langword="true" />, this <see cref="T:Terraria.PopupText" /> will draw in the Master Mode rarity color.
	/// </summary>
	public bool master;

	/// <summary>
	/// Marks this <see cref="T:Terraria.PopupText" /> as this player's Sonar Potion text.
	/// </summary>
	public bool sonar;

	/// <summary>
	/// The context in which this <see cref="T:Terraria.PopupText" /> was created.
	/// </summary>
	public PopupTextContext context;

	/// <summary>
	/// The NPC type (<see cref="F:Terraria.NPC.type" />) this <see cref="T:Terraria.PopupText" /> is bound to, or <c>0</c> if not bound to an NPC.
	/// </summary>
	public int npcNetID;

	/// <summary>
	/// If <see langword="true" />, this <see cref="T:Terraria.PopupText" /> is not bound to an item or NPC.
	/// </summary>
	public bool freeAdvanced;

	/// <summary>
	/// The <see cref="T:Terraria.ID.ItemRarityID" /> this <see cref="T:Terraria.PopupText" /> uses for its main color.
	/// </summary>
	public int rarity;

	/// <summary>
	/// If <see langword="true" />, this <see cref="T:Terraria.PopupText" /> is not for an item.
	/// </summary>
	public bool notActuallyAnItem
	{
		get
		{
			if (npcNetID == 0)
			{
				return freeAdvanced;
			}
			return true;
		}
	}

	public static float TargetScale => Main.UIScale / Main.GameViewMatrix.Zoom.X;

	/// <summary>
	/// Destroys the <see cref="T:Terraria.PopupText" /> in <see cref="F:Terraria.Main.popupText" /> at the index <see cref="F:Terraria.PopupText.sonarText" /> if that text has <see cref="F:Terraria.PopupText.sonar" /> set to <see langword="true" />.
	/// </summary>
	public static void ClearSonarText()
	{
		if (sonarText >= 0 && Main.popupText[sonarText].sonar)
		{
			Main.popupText[sonarText].active = false;
			sonarText = -1;
		}
	}

	/// <summary>
	/// Resets a <see cref="T:Terraria.PopupText" /> to its default values.
	/// </summary>
	/// <param name="text">The <see cref="T:Terraria.PopupText" /> to reset.</param>
	public static void ResetText(PopupText text)
	{
		text.NoStack = false;
		text.coinText = false;
		text.coinValue = 0L;
		text.sonar = false;
		text.npcNetID = 0;
		text.expert = false;
		text.master = false;
		text.freeAdvanced = false;
		text.scale = 0f;
		text.rotation = 0f;
		text.alpha = 1f;
		text.alphaDir = -1;
		text.rarity = 0;
	}

	/// <summary>
	/// Creates a new <see cref="T:Terraria.PopupText" /> in <see cref="F:Terraria.Main.popupText" /> at <paramref name="position" /> using the settings from <paramref name="request" />.
	/// <br /> The new <see cref="T:Terraria.PopupText" /> is not bound to a specific <see cref="T:Terraria.Item" /> or <see cref="T:Terraria.ID.NPCID" />.
	/// <br /> All <see cref="T:Terraria.PopupText" />s created using this method have <c><see cref="F:Terraria.PopupText.context" /> == <see cref="F:Terraria.PopupTextContext.Advanced" /></c> and <see cref="F:Terraria.PopupText.freeAdvanced" /> set to <see langword="true" />.
	/// </summary>
	/// <param name="request">The settings for the new <see cref="T:Terraria.PopupText" />.</param>
	/// <param name="position">The position of the new <see cref="T:Terraria.PopupText" /> in world coordinates.</param>
	/// <returns>
	/// <c>-1</c> if a new <see cref="T:Terraria.PopupText" /> could not be made, if <c><see cref="F:Terraria.Main.netMode" /> == <see cref="F:Terraria.ID.NetmodeID.Server" /></c>, or if the current player has item text disabled (<see cref="F:Terraria.Main.showItemText" />).
	/// <br /> Otherwise, return the index in <see cref="F:Terraria.Main.popupText" /> of the new <see cref="T:Terraria.PopupText" />
	/// </returns>
	public static int NewText(AdvancedPopupRequest request, Vector2 position)
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		if (!Main.showItemText)
		{
			return -1;
		}
		if (Main.netMode == 2)
		{
			return -1;
		}
		int num = FindNextItemTextSlot();
		if (num >= 0)
		{
			string text = request.Text;
			Vector2 vector = FontAssets.MouseText.Value.MeasureString(text);
			PopupText obj = Main.popupText[num];
			ResetText(obj);
			obj.active = true;
			obj.position = position - vector / 2f;
			obj.name = text;
			obj.stack = 1L;
			obj.velocity = request.Velocity;
			obj.lifeTime = request.DurationInFrames;
			obj.context = PopupTextContext.Advanced;
			obj.freeAdvanced = true;
			obj.color = request.Color;
		}
		return num;
	}

	/// <summary>
	/// Creates a new <see cref="T:Terraria.PopupText" /> in <see cref="F:Terraria.Main.popupText" /> at <paramref name="position" /> bound to a given <paramref name="npcNetID" />.
	/// </summary>
	/// <param name="context">
	/// The <see cref="T:Terraria.PopupTextContext" /> in which this <see cref="T:Terraria.PopupText" /> was created.
	/// <br /> If <c><paramref name="context" /> == <see cref="F:Terraria.PopupTextContext.SonarAlert" /></c>, then <see cref="F:Terraria.PopupText.color" /> will be a shade of red.
	/// </param>
	/// <param name="npcNetID">The <see cref="T:Terraria.ID.NPCID" /> this <see cref="T:Terraria.PopupText" /> represents.</param>
	/// <param name="position"></param>
	/// <param name="stay5TimesLonger">If <see langword="true" />, then this <see cref="T:Terraria.PopupText" /> will spawn with <c><see cref="F:Terraria.PopupText.lifeTime" /> == 5 * 60</c>.</param>
	/// <returns>
	/// <inheritdoc cref="M:Terraria.PopupText.NewText(Terraria.AdvancedPopupRequest,Microsoft.Xna.Framework.Vector2)" />
	/// <br /> Also returns <c>-1</c> if <c><paramref name="npcNetID" /> == 0</c>.
	/// </returns>
	public static int NewText(PopupTextContext context, int npcNetID, Vector2 position, bool stay5TimesLonger)
	{
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		if (!Main.showItemText)
		{
			return -1;
		}
		if (npcNetID == 0)
		{
			return -1;
		}
		if (Main.netMode == 2)
		{
			return -1;
		}
		int num = FindNextItemTextSlot();
		if (num >= 0)
		{
			NPC nPC = new NPC();
			nPC.SetDefaults(npcNetID);
			string typeName = nPC.TypeName;
			Vector2 vector = FontAssets.MouseText.Value.MeasureString(typeName);
			PopupText popupText = Main.popupText[num];
			ResetText(popupText);
			popupText.active = true;
			popupText.position = position - vector / 2f;
			popupText.name = typeName;
			popupText.stack = 1L;
			popupText.velocity.Y = -7f;
			popupText.lifeTime = 60;
			popupText.context = context;
			if (stay5TimesLonger)
			{
				popupText.lifeTime *= 5;
			}
			popupText.npcNetID = npcNetID;
			popupText.color = Color.White;
			if (context == PopupTextContext.SonarAlert)
			{
				popupText.color = Color.Lerp(Color.White, Color.Crimson, 0.5f);
			}
		}
		return num;
	}

	/// <summary>
	/// Creates a new <see cref="T:Terraria.PopupText" /> in <see cref="F:Terraria.Main.popupText" /> at the center of the picked-up <paramref name="newItem" />.
	/// <br /> If a <see cref="T:Terraria.PopupText" /> already exists with the <see cref="M:Terraria.Item.AffixName" /> of <paramref name="newItem" />, that text will instead be modified unless <paramref name="noStack" /> is <see langword="true" />.
	/// </summary>
	/// <param name="context">The <see cref="T:Terraria.PopupTextContext" /> in which this <see cref="T:Terraria.PopupText" /> was created.</param>
	/// <param name="newItem">The <see cref="T:Terraria.Item" /> to ceate the new text from.</param>
	/// <param name="stack">The stack of <paramref name="newItem" />.</param>
	/// <param name="noStack">If <see langword="true" />, always create a new <see cref="T:Terraria.PopupText" /> instead of modifying an existing one.</param>
	/// <param name="longText">If <see langword="true" />, then this <see cref="T:Terraria.PopupText" /> will spawn with <c><see cref="F:Terraria.PopupText.lifeTime" /> == 5 * 60</c>.</param>
	/// <returns>
	/// <inheritdoc cref="M:Terraria.PopupText.NewText(Terraria.AdvancedPopupRequest,Microsoft.Xna.Framework.Vector2)" />
	/// <br /> Also returns <c>-1</c> if <see cref="P:Terraria.Item.Name" /> is <see langword="null" />.
	/// </returns>
	public static int NewText(PopupTextContext context, Item newItem, int stack, bool noStack = false, bool longText = false)
	{
		//IL_03ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0409: Unknown result type (might be due to invalid IL or missing references)
		//IL_043c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0451: Unknown result type (might be due to invalid IL or missing references)
		//IL_0456: Unknown result type (might be due to invalid IL or missing references)
		//IL_0475: Unknown result type (might be due to invalid IL or missing references)
		//IL_047a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_049e: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_030a: Unknown result type (might be due to invalid IL or missing references)
		//IL_033c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0519: Unknown result type (might be due to invalid IL or missing references)
		//IL_051e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0556: Unknown result type (might be due to invalid IL or missing references)
		//IL_055b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_057f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0584: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_0206: Unknown result type (might be due to invalid IL or missing references)
		//IL_020b: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_07fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0800: Unknown result type (might be due to invalid IL or missing references)
		//IL_0248: Unknown result type (might be due to invalid IL or missing references)
		//IL_024d: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0841: Unknown result type (might be due to invalid IL or missing references)
		//IL_0846: Unknown result type (might be due to invalid IL or missing references)
		//IL_0287: Unknown result type (might be due to invalid IL or missing references)
		//IL_028c: Unknown result type (might be due to invalid IL or missing references)
		//IL_061a: Unknown result type (might be due to invalid IL or missing references)
		//IL_061f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0884: Unknown result type (might be due to invalid IL or missing references)
		//IL_0889: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_063b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0640: Unknown result type (might be due to invalid IL or missing references)
		//IL_08bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_08c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_065f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0664: Unknown result type (might be due to invalid IL or missing references)
		//IL_0682: Unknown result type (might be due to invalid IL or missing references)
		//IL_0687: Unknown result type (might be due to invalid IL or missing references)
		if (!Main.showItemText)
		{
			return -1;
		}
		if (newItem.Name == null || !newItem.active)
		{
			return -1;
		}
		if (Main.netMode == 2)
		{
			return -1;
		}
		bool flag = newItem.type >= 71 && newItem.type <= 74;
		for (int i = 0; i < 20; i++)
		{
			PopupText popupText = Main.popupText[i];
			if (!popupText.active || popupText.notActuallyAnItem || (!(popupText.name == newItem.AffixName()) && (!flag || !popupText.coinText)) || popupText.NoStack || noStack)
			{
				continue;
			}
			string text = newItem.Name + " (" + (popupText.stack + stack) + ")";
			string text2 = newItem.Name;
			if (popupText.stack > 1)
			{
				text2 = text2 + " (" + popupText.stack + ")";
			}
			Vector2 vector = FontAssets.MouseText.Value.MeasureString(text2);
			vector = FontAssets.MouseText.Value.MeasureString(text);
			if (popupText.lifeTime < 0)
			{
				popupText.scale = 1f;
			}
			if (popupText.lifeTime < 60)
			{
				popupText.lifeTime = 60;
			}
			if (flag && popupText.coinText)
			{
				long num = 0L;
				if (newItem.type == 71)
				{
					num += stack;
				}
				else if (newItem.type == 72)
				{
					num += 100 * stack;
				}
				else if (newItem.type == 73)
				{
					num += 10000 * stack;
				}
				else if (newItem.type == 74)
				{
					num += 1000000 * stack;
				}
				popupText.AddToCoinValue(num);
				text = ValueToName(popupText.coinValue);
				vector = FontAssets.MouseText.Value.MeasureString(text);
				popupText.name = text;
				if (popupText.coinValue >= 1000000)
				{
					if (popupText.lifeTime < 300)
					{
						popupText.lifeTime = 300;
					}
					popupText.color = new Color(220, 220, 198);
				}
				else if (popupText.coinValue >= 10000)
				{
					if (popupText.lifeTime < 240)
					{
						popupText.lifeTime = 240;
					}
					popupText.color = new Color(224, 201, 92);
				}
				else if (popupText.coinValue >= 100)
				{
					if (popupText.lifeTime < 180)
					{
						popupText.lifeTime = 180;
					}
					popupText.color = new Color(181, 192, 193);
				}
				else if (popupText.coinValue >= 1)
				{
					if (popupText.lifeTime < 120)
					{
						popupText.lifeTime = 120;
					}
					popupText.color = new Color(246, 138, 96);
				}
			}
			popupText.stack += stack;
			popupText.scale = 0f;
			popupText.rotation = 0f;
			popupText.position.X = newItem.position.X + (float)newItem.width * 0.5f - vector.X * 0.5f;
			popupText.position.Y = newItem.position.Y + (float)newItem.height * 0.25f - vector.Y * 0.5f;
			popupText.velocity.Y = -7f;
			popupText.context = context;
			popupText.npcNetID = 0;
			if (popupText.coinText)
			{
				popupText.stack = 1L;
			}
			return i;
		}
		int num2 = FindNextItemTextSlot();
		if (num2 >= 0)
		{
			string text3 = newItem.AffixName();
			if (stack > 1)
			{
				text3 = text3 + " (" + stack + ")";
			}
			Vector2 vector2 = FontAssets.MouseText.Value.MeasureString(text3);
			PopupText popupText2 = Main.popupText[num2];
			ResetText(popupText2);
			popupText2.active = true;
			popupText2.position.X = newItem.position.X + (float)newItem.width * 0.5f - vector2.X * 0.5f;
			popupText2.position.Y = newItem.position.Y + (float)newItem.height * 0.25f - vector2.Y * 0.5f;
			popupText2.color = Color.White;
			if (newItem.rare == 1)
			{
				popupText2.color = new Color(150, 150, 255);
			}
			else if (newItem.rare == 2)
			{
				popupText2.color = new Color(150, 255, 150);
			}
			else if (newItem.rare == 3)
			{
				popupText2.color = new Color(255, 200, 150);
			}
			else if (newItem.rare == 4)
			{
				popupText2.color = new Color(255, 150, 150);
			}
			else if (newItem.rare == 5)
			{
				popupText2.color = new Color(255, 150, 255);
			}
			else if (newItem.rare == -13)
			{
				popupText2.master = true;
			}
			else if (newItem.rare == -11)
			{
				popupText2.color = new Color(255, 175, 0);
			}
			else if (newItem.rare == -1)
			{
				popupText2.color = new Color(130, 130, 130);
			}
			else if (newItem.rare == 6)
			{
				popupText2.color = new Color(210, 160, 255);
			}
			else if (newItem.rare == 7)
			{
				popupText2.color = new Color(150, 255, 10);
			}
			else if (newItem.rare == 8)
			{
				popupText2.color = new Color(255, 255, 10);
			}
			else if (newItem.rare == 9)
			{
				popupText2.color = new Color(5, 200, 255);
			}
			else if (newItem.rare == 10)
			{
				popupText2.color = new Color(255, 40, 100);
			}
			else if (newItem.rare == 11)
			{
				popupText2.color = new Color(180, 40, 255);
			}
			else if (newItem.rare >= 12)
			{
				popupText2.color = RarityLoader.GetRarity(newItem.rare).RarityColor;
			}
			popupText2.rarity = newItem.rare;
			popupText2.expert = newItem.expert;
			popupText2.master = newItem.master;
			popupText2.name = newItem.AffixName();
			popupText2.stack = stack;
			popupText2.velocity.Y = -7f;
			popupText2.lifeTime = 60;
			popupText2.context = context;
			if (longText)
			{
				popupText2.lifeTime *= 5;
			}
			popupText2.coinValue = 0L;
			popupText2.coinText = newItem.type >= 71 && newItem.type <= 74;
			if (popupText2.coinText)
			{
				long num3 = 0L;
				if (newItem.type == 71)
				{
					num3 += popupText2.stack;
				}
				else if (newItem.type == 72)
				{
					num3 += 100 * popupText2.stack;
				}
				else if (newItem.type == 73)
				{
					num3 += 10000 * popupText2.stack;
				}
				else if (newItem.type == 74)
				{
					num3 += 1000000 * popupText2.stack;
				}
				popupText2.AddToCoinValue(num3);
				popupText2.ValueToName();
				popupText2.stack = 1L;
				if (popupText2.coinValue >= 1000000)
				{
					if (popupText2.lifeTime < 300)
					{
						popupText2.lifeTime = 300;
					}
					popupText2.color = new Color(220, 220, 198);
				}
				else if (popupText2.coinValue >= 10000)
				{
					if (popupText2.lifeTime < 240)
					{
						popupText2.lifeTime = 240;
					}
					popupText2.color = new Color(224, 201, 92);
				}
				else if (popupText2.coinValue >= 100)
				{
					if (popupText2.lifeTime < 180)
					{
						popupText2.lifeTime = 180;
					}
					popupText2.color = new Color(181, 192, 193);
				}
				else if (popupText2.coinValue >= 1)
				{
					if (popupText2.lifeTime < 120)
					{
						popupText2.lifeTime = 120;
					}
					popupText2.color = new Color(246, 138, 96);
				}
			}
		}
		return num2;
	}

	private void AddToCoinValue(long addedValue)
	{
		long val = coinValue + addedValue;
		coinValue = Math.Min(999999999L, Math.Max(0L, val));
	}

	private static int FindNextItemTextSlot()
	{
		int num = -1;
		for (int i = 0; i < 20; i++)
		{
			if (!Main.popupText[i].active)
			{
				num = i;
				break;
			}
		}
		if (num == -1)
		{
			double num2 = Main.bottomWorld;
			for (int j = 0; j < 20; j++)
			{
				if (num2 > (double)Main.popupText[j].position.Y)
				{
					num = j;
					num2 = Main.popupText[j].position.Y;
				}
			}
		}
		return num;
	}

	/// <summary>
	/// Marks the <see cref="T:Terraria.PopupText" /> in <see cref="F:Terraria.Main.popupText" /> at <paramref name="sonarTextIndex" /> as sonar text, assigning <see cref="F:Terraria.PopupText.sonarText" /> and setting <see cref="F:Terraria.PopupText.sonar" /> to <see langword="true" />.
	/// </summary>
	/// <param name="sonarTextIndex"></param>
	public static void AssignAsSonarText(int sonarTextIndex)
	{
		sonarText = sonarTextIndex;
		if (sonarText > -1)
		{
			Main.popupText[sonarText].sonar = true;
		}
	}

	/// <summary>
	/// Converts a value in copper coins to a formatted string.
	/// </summary>
	/// <param name="coinValue">The value to format in copper coins.</param>
	/// <returns>The formatted text.</returns>
	public static string ValueToName(long coinValue)
	{
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		string text = "";
		long num5 = coinValue;
		while (num5 > 0)
		{
			if (num5 >= 1000000)
			{
				num5 -= 1000000;
				num++;
			}
			else if (num5 >= 10000)
			{
				num5 -= 10000;
				num2++;
			}
			else if (num5 >= 100)
			{
				num5 -= 100;
				num3++;
			}
			else if (num5 >= 1)
			{
				num5--;
				num4++;
			}
		}
		text = "";
		if (num > 0)
		{
			text = text + num + string.Format(" {0} ", Language.GetTextValue("Currency.Platinum"));
		}
		if (num2 > 0)
		{
			text = text + num2 + string.Format(" {0} ", Language.GetTextValue("Currency.Gold"));
		}
		if (num3 > 0)
		{
			text = text + num3 + string.Format(" {0} ", Language.GetTextValue("Currency.Silver"));
		}
		if (num4 > 0)
		{
			text = text + num4 + string.Format(" {0} ", Language.GetTextValue("Currency.Copper"));
		}
		if (text.Length > 1)
		{
			text = text.Substring(0, text.Length - 1);
		}
		return text;
	}

	private void ValueToName()
	{
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		long num5 = coinValue;
		while (num5 > 0)
		{
			if (num5 >= 1000000)
			{
				num5 -= 1000000;
				num++;
			}
			else if (num5 >= 10000)
			{
				num5 -= 10000;
				num2++;
			}
			else if (num5 >= 100)
			{
				num5 -= 100;
				num3++;
			}
			else if (num5 >= 1)
			{
				num5--;
				num4++;
			}
		}
		name = "";
		if (num > 0)
		{
			name = name + num + string.Format(" {0} ", Language.GetTextValue("Currency.Platinum"));
		}
		if (num2 > 0)
		{
			name = name + num2 + string.Format(" {0} ", Language.GetTextValue("Currency.Gold"));
		}
		if (num3 > 0)
		{
			name = name + num3 + string.Format(" {0} ", Language.GetTextValue("Currency.Silver"));
		}
		if (num4 > 0)
		{
			name = name + num4 + string.Format(" {0} ", Language.GetTextValue("Currency.Copper"));
		}
		if (name.Length > 1)
		{
			name = name.Substring(0, name.Length - 1);
		}
	}

	/// <summary>
	/// Updates this <see cref="T:Terraria.PopupText" />.
	/// </summary>
	/// <param name="whoAmI">The index in <see cref="F:Terraria.Main.popupText" /> of this <see cref="T:Terraria.PopupText" />.</param>
	public void Update(int whoAmI)
	{
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cb: Unknown result type (might be due to invalid IL or missing references)
		if (!active)
		{
			return;
		}
		float targetScale = TargetScale;
		alpha += (float)alphaDir * 0.01f;
		if ((double)alpha <= 0.7)
		{
			alpha = 0.7f;
			alphaDir = 1;
		}
		if (alpha >= 1f)
		{
			alpha = 1f;
			alphaDir = -1;
		}
		if (expert)
		{
			color = new Color((int)(byte)Main.DiscoR, (int)(byte)Main.DiscoG, (int)(byte)Main.DiscoB, (int)Main.mouseTextColor);
		}
		else if (master)
		{
			color = new Color(255, (int)(byte)(Main.masterColor * 200f), 0, (int)Main.mouseTextColor);
		}
		if (rarity > 11)
		{
			color = RarityLoader.GetRarity(rarity).RarityColor;
		}
		bool flag = false;
		Vector2 textHitbox = GetTextHitbox();
		Rectangle rectangle = default(Rectangle);
		((Rectangle)(ref rectangle))._002Ector((int)(position.X - textHitbox.X / 2f), (int)(position.Y - textHitbox.Y / 2f), (int)textHitbox.X, (int)textHitbox.Y);
		Rectangle value = default(Rectangle);
		for (int i = 0; i < 20; i++)
		{
			PopupText popupText = Main.popupText[i];
			if (!popupText.active || i == whoAmI)
			{
				continue;
			}
			Vector2 textHitbox2 = popupText.GetTextHitbox();
			((Rectangle)(ref value))._002Ector((int)(popupText.position.X - textHitbox2.X / 2f), (int)(popupText.position.Y - textHitbox2.Y / 2f), (int)textHitbox2.X, (int)textHitbox2.Y);
			if (((Rectangle)(ref rectangle)).Intersects(value) && (position.Y < popupText.position.Y || (position.Y == popupText.position.Y && whoAmI < i)))
			{
				flag = true;
				int num = numActive;
				if (num > 3)
				{
					num = 3;
				}
				popupText.lifeTime = activeTime + 15 * num;
				lifeTime = activeTime + 15 * num;
			}
		}
		if (!flag)
		{
			velocity.Y *= 0.86f;
			if (scale == targetScale)
			{
				velocity.Y *= 0.4f;
			}
		}
		else if (velocity.Y > -6f)
		{
			velocity.Y -= 0.2f;
		}
		else
		{
			velocity.Y *= 0.86f;
		}
		velocity.X *= 0.93f;
		position += velocity;
		lifeTime--;
		if (lifeTime <= 0)
		{
			scale -= 0.03f * targetScale;
			if ((double)scale < 0.1 * (double)targetScale)
			{
				active = false;
				if (sonarText == whoAmI)
				{
					sonarText = -1;
				}
			}
			lifeTime = 0;
		}
		else
		{
			if (scale < targetScale)
			{
				scale += 0.1f * targetScale;
			}
			if (scale > targetScale)
			{
				scale = targetScale;
			}
		}
	}

	private Vector2 GetTextHitbox()
	{
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		string text = name;
		if (stack > 1)
		{
			text = text + " (" + stack + ")";
		}
		Vector2 result = FontAssets.MouseText.Value.MeasureString(text);
		result *= scale;
		result.Y *= 0.8f;
		return result;
	}

	/// <summary>
	/// Calls <see cref="M:Terraria.PopupText.Update(System.Int32)" /> on all <see cref="F:Terraria.PopupText.active" /> <see cref="T:Terraria.PopupText" />s in <see cref="F:Terraria.Main.popupText" /> and assigns  <see cref="F:Terraria.PopupText.numActive" />.
	/// </summary>
	public static void UpdateItemText()
	{
		int num = 0;
		for (int i = 0; i < 20; i++)
		{
			if (Main.popupText[i].active)
			{
				num++;
				Main.popupText[i].Update(i);
			}
		}
		numActive = num;
	}

	/// <summary>
	/// Sets all <see cref="T:Terraria.PopupText" />s in <see cref="F:Terraria.Main.popupText" /> to a new instance and assigns <see cref="F:Terraria.PopupText.numActive" /> to <c>0</c>.
	/// </summary>
	public static void ClearAll()
	{
		for (int i = 0; i < 20; i++)
		{
			Main.popupText[i] = new PopupText();
		}
		numActive = 0;
	}
}
