using System;
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.GameContent;
using Terraria.Localization;
using Terraria.ModLoader.UI;
using Terraria.UI;
using Terraria.UI.Chat;

namespace Terraria.ModLoader.Config.UI;

public abstract class ConfigElement<T> : ConfigElement
{
	protected virtual T Value
	{
		get
		{
			return (T)GetObject();
		}
		set
		{
			SetObject(value);
		}
	}
}
public abstract class ConfigElement : UIElement
{
	private Color backgroundColor;

	protected LabelKeyAttribute LabelAttribute;

	protected string Label;

	protected TooltipKeyAttribute TooltipAttribute;

	protected BackgroundColorAttribute BackgroundColorAttribute;

	protected RangeAttribute RangeAttribute;

	protected IncrementAttribute IncrementAttribute;

	protected JsonDefaultValueAttribute JsonDefaultValueAttribute;

	public int Index { get; set; }

	protected Asset<Texture2D> PlayTexture { get; set; } = Main.Assets.Request<Texture2D>("Images/UI/ButtonPlay");


	protected Asset<Texture2D> DeleteTexture { get; set; } = Main.Assets.Request<Texture2D>("Images/UI/ButtonDelete");


	protected Asset<Texture2D> PlusTexture { get; set; } = UICommon.ButtonPlusTexture;


	protected Asset<Texture2D> UpDownTexture { get; set; } = UICommon.ButtonUpDownTexture;


	protected Asset<Texture2D> CollapsedTexture { get; set; } = UICommon.ButtonCollapsedTexture;


	protected Asset<Texture2D> ExpandedTexture { get; set; } = UICommon.ButtonExpandedTexture;


	protected PropertyFieldWrapper MemberInfo { get; set; }

	protected object Item { get; set; }

	protected IList List { get; set; }

	protected bool NullAllowed { get; set; }

	protected internal Func<string> TextDisplayFunction { get; set; }

	protected Func<string> TooltipFunction { get; set; }

	protected bool DrawLabel { get; set; } = true;


	protected bool ReloadRequired { get; set; }

	protected bool ShowReloadRequiredTooltip { get; set; }

	protected object OldValue { get; set; }

	protected bool ValueChanged => !ConfigManager.ObjectEquals(OldValue, GetObject());

	public ConfigElement()
	{
		Width.Set(0f, 1f);
		Height.Set(30f, 0f);
	}

	/// <summary>
	/// Bind must always be called after the ctor and serves to facilitate a convenient inheritance workflow for custom ConfigElemets from mods.
	/// </summary>
	public void Bind(PropertyFieldWrapper memberInfo, object item, IList array, int index)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		MemberInfo = memberInfo;
		Item = item;
		List = array;
		Index = index;
		backgroundColor = UICommon.DefaultUIBlue;
	}

	public virtual void OnBind()
	{
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		LabelAttribute = ConfigManager.GetCustomAttributeFromMemberThenMemberType<LabelKeyAttribute>(MemberInfo, Item, List);
		Label = ConfigManager.GetLocalizedLabel(MemberInfo);
		TextDisplayFunction = () => Label;
		TooltipAttribute = ConfigManager.GetCustomAttributeFromMemberThenMemberType<TooltipKeyAttribute>(MemberInfo, Item, List);
		string tooltip = ConfigManager.GetLocalizedTooltip(MemberInfo);
		if (tooltip != null)
		{
			TooltipFunction = () => tooltip;
		}
		BackgroundColorAttribute = ConfigManager.GetCustomAttributeFromMemberThenMemberType<BackgroundColorAttribute>(MemberInfo, Item, List);
		if (BackgroundColorAttribute != null)
		{
			backgroundColor = BackgroundColorAttribute.Color;
		}
		RangeAttribute = ConfigManager.GetCustomAttributeFromMemberThenMemberType<RangeAttribute>(MemberInfo, Item, List);
		IncrementAttribute = ConfigManager.GetCustomAttributeFromMemberThenMemberType<IncrementAttribute>(MemberInfo, Item, List);
		NullAllowed = ConfigManager.GetCustomAttributeFromMemberThenMemberType<NullAllowedAttribute>(MemberInfo, Item, List) != null;
		JsonDefaultValueAttribute = ConfigManager.GetCustomAttributeFromMemberThenMemberType<JsonDefaultValueAttribute>(MemberInfo, Item, List);
		ShowReloadRequiredTooltip = ConfigManager.GetCustomAttributeFromMemberThenMemberType<ReloadRequiredAttribute>(MemberInfo, Item, List) != null;
		if (ShowReloadRequiredTooltip && List == null && Item is ModConfig modConfig)
		{
			ReloadRequired = true;
			ModConfig loadTimeConfig = ConfigManager.GetLoadTimeConfig(modConfig.Mod, modConfig.Name);
			OldValue = MemberInfo.GetValue(loadTimeConfig);
		}
	}

	protected virtual void SetObject(object value)
	{
		if (List != null)
		{
			List[Index] = value;
			Interface.modConfig.SetPendingChanges();
		}
		else if (MemberInfo.CanWrite)
		{
			MemberInfo.SetValue(Item, value);
			Interface.modConfig.SetPendingChanges();
		}
	}

	protected virtual object GetObject()
	{
		if (List != null)
		{
			return List[Index];
		}
		return MemberInfo.GetValue(Item);
	}

	protected override void DrawSelf(SpriteBatch spriteBatch)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		base.DrawSelf(spriteBatch);
		CalculatedStyle dimensions = GetDimensions();
		float settingsWidth = dimensions.Width + 1f;
		Vector2 val = new Vector2(dimensions.X, dimensions.Y);
		Vector2 baseScale = default(Vector2);
		((Vector2)(ref baseScale))._002Ector(0.8f);
		Color color = (base.IsMouseHovering ? Color.White : Color.White);
		if (!MemberInfo.CanWrite)
		{
			color = Color.Gray;
		}
		Color panelColor = (base.IsMouseHovering ? backgroundColor : backgroundColor.MultiplyRGBA(new Color(180, 180, 180)));
		Vector2 position = val;
		DrawPanel2(spriteBatch, position, TextureAssets.SettingsPanel.Value, settingsWidth, dimensions.Height, panelColor);
		if (DrawLabel)
		{
			position.X += 8f;
			position.Y += 8f;
			string label = TextDisplayFunction();
			if (ReloadRequired && ValueChanged)
			{
				label = label + " - [c/FF0000:" + Language.GetTextValue("tModLoader.ModReloadRequired") + "]";
			}
			ChatManager.DrawColorCodedStringWithShadow(spriteBatch, FontAssets.ItemStack.Value, label, position, color, 0f, Vector2.Zero, baseScale, settingsWidth);
		}
		if (base.IsMouseHovering && TooltipFunction != null)
		{
			string tooltip = TooltipFunction();
			if (ShowReloadRequiredTooltip)
			{
				tooltip += (string.IsNullOrEmpty(tooltip) ? "" : "\n");
				tooltip = tooltip + "[c/" + Color.Orange.Hex3() + ":" + Language.GetTextValue("tModLoader.ModReloadRequiredMemberTooltip") + "]";
			}
			UIModConfig.Tooltip = tooltip;
		}
	}

	public static void DrawPanel2(SpriteBatch spriteBatch, Vector2 position, Texture2D texture, float width, float height, Color color)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Unknown result type (might be due to invalid IL or missing references)
		//IL_0193: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
		spriteBatch.Draw(texture, position + new Vector2(0f, 2f), (Rectangle?)new Rectangle(0, 2, 1, 1), color, 0f, Vector2.Zero, new Vector2(2f, height - 4f), (SpriteEffects)0, 0f);
		spriteBatch.Draw(texture, position + new Vector2(width - 2f, 2f), (Rectangle?)new Rectangle(0, 2, 1, 1), color, 0f, Vector2.Zero, new Vector2(2f, height - 4f), (SpriteEffects)0, 0f);
		spriteBatch.Draw(texture, position + new Vector2(2f, 0f), (Rectangle?)new Rectangle(2, 0, 1, 1), color, 0f, Vector2.Zero, new Vector2(width - 4f, 2f), (SpriteEffects)0, 0f);
		spriteBatch.Draw(texture, position + new Vector2(2f, height - 2f), (Rectangle?)new Rectangle(2, 0, 1, 1), color, 0f, Vector2.Zero, new Vector2(width - 4f, 2f), (SpriteEffects)0, 0f);
		spriteBatch.Draw(texture, position + new Vector2(2f, 2f), (Rectangle?)new Rectangle(2, 2, 1, 1), color, 0f, Vector2.Zero, new Vector2(width - 4f, (height - 4f) / 2f), (SpriteEffects)0, 0f);
		spriteBatch.Draw(texture, position + new Vector2(2f, 2f + (height - 4f) / 2f), (Rectangle?)new Rectangle(2, 16, 1, 1), color, 0f, Vector2.Zero, new Vector2(width - 4f, (height - 4f) / 2f), (SpriteEffects)0, 0f);
	}
}
