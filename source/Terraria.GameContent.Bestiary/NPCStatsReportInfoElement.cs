using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.GameContent.UI.Elements;
using Terraria.Localization;
using Terraria.UI;

namespace Terraria.GameContent.Bestiary;

public class NPCStatsReportInfoElement : IBestiaryInfoElement, IUpdateBeforeSorting
{
	public delegate void StatAdjustmentStep(NPCStatsReportInfoElement element);

	public int NpcId;

	public int Damage;

	public int LifeMax;

	public float MonetaryValue;

	public int Defense;

	public float KnockbackResist;

	private NPC _instance;

	public event StatAdjustmentStep OnRefreshStats;

	public NPCStatsReportInfoElement(int npcNetId)
	{
		NpcId = npcNetId;
		_instance = new NPC();
		RefreshStats(Main.GameModeInfo, _instance);
	}

	public void UpdateBeforeSorting()
	{
		RefreshStats(Main.GameModeInfo, _instance);
	}

	private void RefreshStats(GameModeData gameModeFound, NPC instance)
	{
		instance.SetDefaults(NpcId, new NPCSpawnParams
		{
			gameModeData = gameModeFound,
			strengthMultiplierOverride = null
		});
		Damage = instance.damage;
		LifeMax = instance.lifeMax;
		MonetaryValue = instance.value;
		Defense = instance.defense;
		KnockbackResist = instance.knockBackResist;
		if (this.OnRefreshStats != null)
		{
			this.OnRefreshStats(this);
		}
	}

	public UIElement ProvideUIElement(BestiaryUICollectionInfo info)
	{
		//IL_0482: Unknown result type (might be due to invalid IL or missing references)
		//IL_048c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0491: Unknown result type (might be due to invalid IL or missing references)
		if (info.UnlockState == BestiaryEntryUnlockState.NotKnownAtAll_0)
		{
			return null;
		}
		RefreshStats(Main.GameModeInfo, _instance);
		UIElement uIElement = new UIElement
		{
			Width = new StyleDimension(0f, 1f),
			Height = new StyleDimension(109f, 0f)
		};
		int num = 99;
		int num5 = 35;
		int num6 = 3;
		int num7 = 0;
		UIImage uIImage = new UIImage(Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Stat_HP"))
		{
			Top = new StyleDimension(num7, 0f),
			Left = new StyleDimension(num6, 0f)
		};
		UIImage uIImage2 = new UIImage(Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Stat_Attack"))
		{
			Top = new StyleDimension(num7 + num5, 0f),
			Left = new StyleDimension(num6, 0f)
		};
		UIImage uIImage3 = new UIImage(Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Stat_Defense"))
		{
			Top = new StyleDimension(num7 + num5, 0f),
			Left = new StyleDimension(num6 + num, 0f)
		};
		UIImage uIImage4 = new UIImage(Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Stat_Knockback"))
		{
			Top = new StyleDimension(num7, 0f),
			Left = new StyleDimension(num6 + num, 0f)
		};
		uIElement.Append(uIImage);
		uIElement.Append(uIImage2);
		uIElement.Append(uIImage3);
		uIElement.Append(uIImage4);
		int num8 = -10;
		int num9 = 0;
		int num12 = (int)MonetaryValue;
		string text = Utils.Clamp(num12 / 1000000, 0, 999).ToString();
		string text2 = Utils.Clamp(num12 % 1000000 / 10000, 0, 99).ToString();
		string text3 = Utils.Clamp(num12 % 10000 / 100, 0, 99).ToString();
		string text4 = Utils.Clamp(num12 % 100 / 1, 0, 99).ToString();
		if (num12 / 1000000 < 1)
		{
			text = "-";
		}
		if (num12 / 10000 < 1)
		{
			text2 = "-";
		}
		if (num12 / 100 < 1)
		{
			text3 = "-";
		}
		if (num12 < 1)
		{
			text4 = "-";
		}
		string text5 = LifeMax.ToString();
		string text6 = Damage.ToString();
		string text7 = Defense.ToString();
		string text8 = ((KnockbackResist > 0.8f) ? Language.GetText("BestiaryInfo.KnockbackHigh").Value : ((KnockbackResist > 0.4f) ? Language.GetText("BestiaryInfo.KnockbackMedium").Value : ((!(KnockbackResist > 0f)) ? Language.GetText("BestiaryInfo.KnockbackNone").Value : Language.GetText("BestiaryInfo.KnockbackLow").Value)));
		if (info.UnlockState < BestiaryEntryUnlockState.CanShowStats_2)
		{
			text = (text2 = (text3 = (text4 = "?")));
			text5 = (text6 = (text7 = (text8 = "???")));
		}
		UIText element = new UIText(text5)
		{
			HAlign = 1f,
			VAlign = 0.5f,
			Left = new StyleDimension(num8, 0f),
			Top = new StyleDimension(num9, 0f),
			IgnoresMouseInteraction = true
		};
		UIText element2 = new UIText(text8)
		{
			HAlign = 1f,
			VAlign = 0.5f,
			Left = new StyleDimension(num8, 0f),
			Top = new StyleDimension(num9, 0f),
			IgnoresMouseInteraction = true
		};
		UIText element3 = new UIText(text6)
		{
			HAlign = 1f,
			VAlign = 0.5f,
			Left = new StyleDimension(num8, 0f),
			Top = new StyleDimension(num9, 0f),
			IgnoresMouseInteraction = true
		};
		UIText element4 = new UIText(text7)
		{
			HAlign = 1f,
			VAlign = 0.5f,
			Left = new StyleDimension(num8, 0f),
			Top = new StyleDimension(num9, 0f),
			IgnoresMouseInteraction = true
		};
		uIImage.Append(element);
		uIImage2.Append(element3);
		uIImage3.Append(element4);
		uIImage4.Append(element2);
		int num10 = 66;
		if (num12 > 0)
		{
			UIHorizontalSeparator element5 = new UIHorizontalSeparator
			{
				Width = StyleDimension.FromPixelsAndPercent(0f, 1f),
				Color = new Color(89, 116, 213, 255) * 0.9f,
				Left = new StyleDimension(0f, 0f),
				Top = new StyleDimension(num9 + num5 * 2, 0f)
			};
			uIElement.Append(element5);
			num10 += 4;
			int num11 = num6;
			int num2 = num10 + 8;
			int num3 = 49;
			UIImage uIImage5 = new UIImage(Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Stat_Platinum"))
			{
				Top = new StyleDimension(num2, 0f),
				Left = new StyleDimension(num11, 0f)
			};
			UIImage uIImage6 = new UIImage(Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Stat_Gold"))
			{
				Top = new StyleDimension(num2, 0f),
				Left = new StyleDimension(num11 + num3, 0f)
			};
			UIImage uIImage7 = new UIImage(Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Stat_Silver"))
			{
				Top = new StyleDimension(num2, 0f),
				Left = new StyleDimension(num11 + num3 * 2 + 1, 0f)
			};
			UIImage uIImage8 = new UIImage(Main.Assets.Request<Texture2D>("Images/UI/Bestiary/Stat_Copper"))
			{
				Top = new StyleDimension(num2, 0f),
				Left = new StyleDimension(num11 + num3 * 3 + 1, 0f)
			};
			if (text != "-")
			{
				uIElement.Append(uIImage5);
			}
			if (text2 != "-")
			{
				uIElement.Append(uIImage6);
			}
			if (text3 != "-")
			{
				uIElement.Append(uIImage7);
			}
			if (text4 != "-")
			{
				uIElement.Append(uIImage8);
			}
			int num4 = num8 + 3;
			float textScale = 0.85f;
			UIText element6 = new UIText(text, textScale)
			{
				HAlign = 1f,
				VAlign = 0.5f,
				Left = new StyleDimension(num4, 0f),
				Top = new StyleDimension(num9, 0f)
			};
			UIText element7 = new UIText(text2, textScale)
			{
				HAlign = 1f,
				VAlign = 0.5f,
				Left = new StyleDimension(num4, 0f),
				Top = new StyleDimension(num9, 0f)
			};
			UIText element8 = new UIText(text3, textScale)
			{
				HAlign = 1f,
				VAlign = 0.5f,
				Left = new StyleDimension(num4, 0f),
				Top = new StyleDimension(num9, 0f)
			};
			UIText element9 = new UIText(text4, textScale)
			{
				HAlign = 1f,
				VAlign = 0.5f,
				Left = new StyleDimension(num4, 0f),
				Top = new StyleDimension(num9, 0f)
			};
			uIImage5.Append(element6);
			uIImage6.Append(element7);
			uIImage7.Append(element8);
			uIImage8.Append(element9);
			num10 += 34;
		}
		num10 += 4;
		uIElement.Height.Pixels = num10;
		uIImage2.OnUpdate += ShowStats_Attack;
		uIImage3.OnUpdate += ShowStats_Defense;
		uIImage.OnUpdate += ShowStats_Life;
		uIImage4.OnUpdate += ShowStats_Knockback;
		return uIElement;
	}

	private void ShowStats_Attack(UIElement element)
	{
		if (element.IsMouseHovering)
		{
			Main.instance.MouseText(Language.GetTextValue("BestiaryInfo.Attack"), 0, 0);
		}
	}

	private void ShowStats_Defense(UIElement element)
	{
		if (element.IsMouseHovering)
		{
			Main.instance.MouseText(Language.GetTextValue("BestiaryInfo.Defense"), 0, 0);
		}
	}

	private void ShowStats_Knockback(UIElement element)
	{
		if (element.IsMouseHovering)
		{
			Main.instance.MouseText(Language.GetTextValue("BestiaryInfo.Knockback"), 0, 0);
		}
	}

	private void ShowStats_Life(UIElement element)
	{
		if (element.IsMouseHovering)
		{
			Main.instance.MouseText(Language.GetTextValue("BestiaryInfo.Life"), 0, 0);
		}
	}
}
