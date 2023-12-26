using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.GameContent.UI.States;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader.UI;
using Terraria.UI;
using Terraria.UI.Gamepad;

namespace Terraria.ModLoader.Config.UI;

internal class UIModConfig : UIState
{
	private UIElement uIElement;

	private UITextPanel<string> headerTextPanel;

	private UITextPanel<string> message;

	private UITextPanel<string> previousConfigButton;

	private UITextPanel<string> nextConfigButton;

	private UITextPanel<string> saveConfigButton;

	private UITextPanel<string> backButton;

	private UITextPanel<string> revertConfigButton;

	private UITextPanel<string> restoreDefaultsConfigButton;

	private UIPanel uIPanel;

	private readonly List<Tuple<UIElement, UIElement>> mainConfigItems = new List<Tuple<UIElement, UIElement>>();

	private UIList mainConfigList;

	private UIScrollbar uIScrollbar;

	private readonly Stack<UIPanel> configPanelStack = new Stack<UIPanel>();

	private readonly Stack<string> subPageStack = new Stack<string>();

	private Mod mod;

	private List<ModConfig> sortedModConfigs;

	private ModConfig modConfig;

	internal ModConfig pendingConfig;

	private bool updateNeeded;

	private UIFocusInputTextField filterTextField;

	private bool pendingChanges;

	private bool pendingChangesUIUpdate;

	private bool netUpdate;

	private static bool pendingRevertDefaults;

	public int UpdateCount { get; set; }

	public static string Tooltip { get; set; }

	public override void OnInitialize()
	{
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0407: Unknown result type (might be due to invalid IL or missing references)
		//IL_040c: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_051c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0522: Unknown result type (might be due to invalid IL or missing references)
		//IL_0525: Unknown result type (might be due to invalid IL or missing references)
		//IL_052b: Unknown result type (might be due to invalid IL or missing references)
		//IL_052e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0534: Unknown result type (might be due to invalid IL or missing references)
		//IL_0537: Unknown result type (might be due to invalid IL or missing references)
		//IL_053d: Unknown result type (might be due to invalid IL or missing references)
		//IL_05dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_067f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0685: Unknown result type (might be due to invalid IL or missing references)
		//IL_0688: Unknown result type (might be due to invalid IL or missing references)
		//IL_068e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0691: Unknown result type (might be due to invalid IL or missing references)
		//IL_0697: Unknown result type (might be due to invalid IL or missing references)
		//IL_069a: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0731: Unknown result type (might be due to invalid IL or missing references)
		//IL_0737: Unknown result type (might be due to invalid IL or missing references)
		//IL_073a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0740: Unknown result type (might be due to invalid IL or missing references)
		//IL_0743: Unknown result type (might be due to invalid IL or missing references)
		//IL_0749: Unknown result type (might be due to invalid IL or missing references)
		//IL_074c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0752: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_07bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_07cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_07cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_081a: Unknown result type (might be due to invalid IL or missing references)
		//IL_081f: Unknown result type (might be due to invalid IL or missing references)
		uIElement = new UIElement();
		uIElement.Width.Set(0f, 0.8f);
		uIElement.MaxWidth.Set(600f, 0f);
		uIElement.Top.Set(160f, 0f);
		uIElement.Height.Set(-180f, 1f);
		uIElement.HAlign = 0.5f;
		uIPanel = new UIPanel();
		uIPanel.Width.Set(0f, 1f);
		uIPanel.Height.Set(-140f, 1f);
		uIPanel.Top.Set(30f, 0f);
		uIPanel.BackgroundColor = UICommon.MainPanelBackground;
		uIElement.Append(uIPanel);
		UIPanel textBoxBackground = new UIPanel();
		textBoxBackground.SetPadding(0f);
		filterTextField = new UIFocusInputTextField(Language.GetTextValue("tModLoader.ModConfigFilterOptions"));
		textBoxBackground.Top.Set(2f, 0f);
		textBoxBackground.Left.Set(-190f, 1f);
		textBoxBackground.Width.Set(180f, 0f);
		textBoxBackground.Height.Set(30f, 0f);
		uIElement.Append(textBoxBackground);
		filterTextField.SetText("");
		filterTextField.Top.Set(5f, 0f);
		filterTextField.Left.Set(10f, 0f);
		filterTextField.Width.Set(-20f, 1f);
		filterTextField.Height.Set(20f, 0f);
		filterTextField.OnTextChange += delegate
		{
			updateNeeded = true;
		};
		filterTextField.OnRightClick += delegate
		{
			filterTextField.SetText("");
		};
		textBoxBackground.Append(filterTextField);
		message = new UITextPanel<string>(Language.GetTextValue("tModLoader.ModConfigNotification"));
		message.Width.Set(-80f, 1f);
		message.Height.Set(20f, 0f);
		message.HAlign = 0.5f;
		message.VAlign = 1f;
		message.Top.Set(-65f, 0f);
		uIElement.Append(message);
		mainConfigList = new UIList();
		mainConfigList.Width.Set(-25f, 1f);
		mainConfigList.Height.Set(0f, 1f);
		mainConfigList.ListPadding = 5f;
		uIPanel.Append(mainConfigList);
		configPanelStack.Push(uIPanel);
		uIScrollbar = new UIScrollbar();
		uIScrollbar.SetView(100f, 1000f);
		uIScrollbar.Height.Set(0f, 1f);
		uIScrollbar.HAlign = 1f;
		uIPanel.Append(uIScrollbar);
		mainConfigList.SetScrollbar(uIScrollbar);
		headerTextPanel = new UITextPanel<string>(Language.GetTextValue("tModLoader.ModConfigModConfig"), 0.8f, large: true);
		headerTextPanel.HAlign = 0.5f;
		headerTextPanel.Top.Set(-50f, 0f);
		headerTextPanel.SetPadding(15f);
		headerTextPanel.BackgroundColor = UICommon.DefaultUIBlue;
		uIElement.Append(headerTextPanel);
		previousConfigButton = new UITextPanel<string>("<");
		previousConfigButton.Width.Set(25f, 0f);
		previousConfigButton.Height.Set(25f, 0f);
		previousConfigButton.VAlign = 1f;
		previousConfigButton.Top.Set(-65f, 0f);
		previousConfigButton.HAlign = 0f;
		previousConfigButton.WithFadedMouseOver();
		previousConfigButton.OnLeftClick += PreviousConfig;
		nextConfigButton = new UITextPanel<string>(">");
		nextConfigButton.CopyStyle(previousConfigButton);
		nextConfigButton.WithFadedMouseOver();
		nextConfigButton.HAlign = 1f;
		nextConfigButton.OnLeftClick += NextConfig;
		saveConfigButton = new UITextPanel<string>(Language.GetTextValue("tModLoader.ModConfigSaveConfig"));
		saveConfigButton.Width.Set(-10f, 0.25f);
		saveConfigButton.Height.Set(25f, 0f);
		saveConfigButton.Top.Set(-20f, 0f);
		saveConfigButton.WithFadedMouseOver();
		saveConfigButton.HAlign = 0.33f;
		saveConfigButton.VAlign = 1f;
		saveConfigButton.OnLeftClick += SaveConfig;
		backButton = new UITextPanel<string>(Language.GetTextValue("tModLoader.ModConfigBack"));
		backButton.CopyStyle(saveConfigButton);
		backButton.HAlign = 0f;
		backButton.WithFadedMouseOver();
		backButton.OnMouseOver += delegate
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			if (pendingChanges)
			{
				backButton.BackgroundColor = Color.Red;
			}
		};
		backButton.OnMouseOut += delegate
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			if (pendingChanges)
			{
				backButton.BackgroundColor = Color.Red * 0.7f;
			}
		};
		backButton.OnLeftClick += BackClick;
		uIElement.Append(backButton);
		revertConfigButton = new UITextPanel<string>(Language.GetTextValue("tModLoader.ModConfigRevertChanges"));
		revertConfigButton.CopyStyle(saveConfigButton);
		revertConfigButton.WithFadedMouseOver();
		revertConfigButton.HAlign = 0.66f;
		revertConfigButton.OnLeftClick += RevertConfig;
		restoreDefaultsConfigButton = new UITextPanel<string>(Language.GetTextValue("tModLoader.ModConfigRestoreDefaults"));
		restoreDefaultsConfigButton.CopyStyle(saveConfigButton);
		restoreDefaultsConfigButton.WithFadedMouseOver();
		restoreDefaultsConfigButton.HAlign = 1f;
		restoreDefaultsConfigButton.OnLeftClick += RestoreDefaults;
		uIElement.Append(restoreDefaultsConfigButton);
		uIPanel.BackgroundColor = UICommon.MainPanelBackground;
		Append(uIElement);
	}

	private void BackClick(UIMouseEvent evt, UIElement listeningElement)
	{
		SoundEngine.PlaySound(in SoundID.MenuClose);
		if (!Main.gameMenu)
		{
			Main.InGameUI.SetState(Interface.modConfigList);
		}
		else
		{
			Main.menuMode = 10027;
		}
	}

	internal void Unload()
	{
		mainConfigList?.Clear();
		mainConfigItems?.Clear();
		mod = null;
		sortedModConfigs = null;
		modConfig = null;
		pendingConfig = null;
		while (configPanelStack.Count > 1)
		{
			uIElement.RemoveChild(configPanelStack.Pop());
		}
	}

	private void PreviousConfig(UIMouseEvent evt, UIElement listeningElement)
	{
		SoundEngine.PlaySound(in SoundID.MenuOpen);
		int index = sortedModConfigs.IndexOf(modConfig);
		modConfig = sortedModConfigs[(index - 1 < 0) ? (sortedModConfigs.Count - 1) : (index - 1)];
		DoMenuModeState();
	}

	private void NextConfig(UIMouseEvent evt, UIElement listeningElement)
	{
		SoundEngine.PlaySound(in SoundID.MenuOpen);
		int index = sortedModConfigs.IndexOf(modConfig);
		modConfig = sortedModConfigs[(index + 1 <= sortedModConfigs.Count) ? (index + 1) : 0];
		DoMenuModeState();
	}

	private void DoMenuModeState()
	{
		if (Main.gameMenu)
		{
			Main.MenuUI.SetState(null);
			Main.menuMode = 10024;
		}
		else
		{
			Main.InGameUI.SetState(null);
			Main.InGameUI.SetState(Interface.modConfig);
		}
	}

	private void SaveConfig(UIMouseEvent evt, UIElement listeningElement)
	{
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		if (Main.gameMenu)
		{
			SoundEngine.PlaySound(in SoundID.MenuOpen);
			ConfigManager.Save(pendingConfig);
			ConfigManager.Load(modConfig);
		}
		else
		{
			if (pendingConfig.Mode == ConfigScope.ServerSide && Main.netMode == 1)
			{
				SetMessage(Language.GetTextValue("tModLoader.ModConfigAskingServerToAcceptChanges"), Color.Yellow);
				ModPacket modPacket = new ModPacket(249);
				modPacket.Write(pendingConfig.Mod.Name);
				modPacket.Write(pendingConfig.Name);
				string json = JsonConvert.SerializeObject((object)pendingConfig, ConfigManager.serializerSettingsCompact);
				modPacket.Write(json);
				modPacket.Send();
				return;
			}
			if (ConfigManager.GetLoadTimeConfig(modConfig.Mod, modConfig.Name).NeedsReload(pendingConfig))
			{
				SoundEngine.PlaySound(in SoundID.MenuClose);
				SetMessage(Language.GetTextValue("tModLoader.ModConfigCantSaveBecauseChangesWouldRequireAReload"), Color.Red);
				return;
			}
			SoundEngine.PlaySound(in SoundID.MenuOpen);
			ConfigManager.Save(pendingConfig);
			ConfigManager.Load(modConfig);
			modConfig.OnChanged();
		}
		DoMenuModeState();
	}

	private void RestoreDefaults(UIMouseEvent evt, UIElement listeningElement)
	{
		SoundEngine.PlaySound(in SoundID.MenuOpen);
		pendingRevertDefaults = true;
		DoMenuModeState();
	}

	private void RevertConfig(UIMouseEvent evt, UIElement listeningElement)
	{
		SoundEngine.PlaySound(in SoundID.MenuOpen);
		DiscardChanges();
	}

	private void DiscardChanges()
	{
		DoMenuModeState();
	}

	public void SetPendingChanges(bool changes = true)
	{
		pendingChangesUIUpdate |= changes;
		pendingChanges |= changes;
	}

	public void SetMessage(string text, Color color)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		message.TextScale = 1f;
		message.SetText(Language.GetText("tModLoader.ModConfigNotification")?.ToString() + text);
		float width = FontAssets.MouseText.Value.MeasureString(text).X;
		if (width > 400f)
		{
			message.TextScale = 400f / width;
			message.Recalculate();
		}
		message.TextColor = color;
	}

	public override void Update(GameTime gameTime)
	{
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		base.Update(gameTime);
		UpdateCount++;
		if (pendingChangesUIUpdate)
		{
			uIElement.Append(saveConfigButton);
			uIElement.Append(revertConfigButton);
			backButton.BackgroundColor = Color.Red * 0.7f;
			pendingChangesUIUpdate = false;
		}
		if (netUpdate)
		{
			DoMenuModeState();
			netUpdate = false;
		}
		if (updateNeeded)
		{
			updateNeeded = false;
			mainConfigList.Clear();
			mainConfigList.AddRange(from item in mainConfigItems
				where !(item.Item2 is ConfigElement configElement) || configElement.TextDisplayFunction().IndexOf(filterTextField.CurrentString, StringComparison.OrdinalIgnoreCase) != -1
				select item into x
				select x.Item1);
			Recalculate();
		}
	}

	public override void Draw(SpriteBatch spriteBatch)
	{
		Tooltip = null;
		base.Draw(spriteBatch);
		if (!string.IsNullOrEmpty(Tooltip))
		{
			UICommon.TooltipMouseText(Tooltip);
		}
		UILinkPointNavigator.Shortcuts.BackButtonCommand = 100;
		UILinkPointNavigator.Shortcuts.BackButtonGoto = 10027;
	}

	internal void SetMod(Mod mod, ModConfig config = null)
	{
		this.mod = mod;
		if (ConfigManager.Configs.ContainsKey(mod))
		{
			sortedModConfigs = ConfigManager.Configs[mod].OrderBy((ModConfig x) => x.DisplayName.Value).ToList();
			modConfig = sortedModConfigs[0];
			if (config != null)
			{
				modConfig = ConfigManager.Configs[mod].First((ModConfig x) => x == config);
			}
			return;
		}
		throw new Exception("There are no ModConfig for " + mod.DisplayName + ", how did this happen?");
	}

	public override void OnActivate()
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
		filterTextField.SetText("");
		updateNeeded = false;
		SetMessage("", Color.White);
		string configDisplayName = modConfig.DisplayName.Value;
		headerTextPanel.SetText(string.IsNullOrEmpty(configDisplayName) ? modConfig.Mod.DisplayName : (modConfig.Mod.DisplayName + ": " + configDisplayName));
		pendingConfig = ConfigManager.GeneratePopulatedClone(modConfig);
		pendingChanges = pendingRevertDefaults;
		if (pendingRevertDefaults)
		{
			pendingRevertDefaults = false;
			ConfigManager.Reset(pendingConfig);
			pendingChangesUIUpdate = true;
		}
		int num = sortedModConfigs.IndexOf(modConfig);
		int count = sortedModConfigs.Count;
		backButton.BackgroundColor = UICommon.DefaultUIBlueMouseOver;
		uIElement.RemoveChild(saveConfigButton);
		uIElement.RemoveChild(revertConfigButton);
		uIElement.RemoveChild(previousConfigButton);
		uIElement.RemoveChild(nextConfigButton);
		if (num + 1 < count)
		{
			uIElement.Append(nextConfigButton);
		}
		if (num - 1 >= 0)
		{
			uIElement.Append(previousConfigButton);
		}
		uIElement.RemoveChild(configPanelStack.Peek());
		uIElement.Append(uIPanel);
		mainConfigItems.Clear();
		mainConfigList.Clear();
		configPanelStack.Clear();
		configPanelStack.Push(uIPanel);
		subPageStack.Clear();
		int top = 0;
		uIPanel.BackgroundColor = UICommon.MainPanelBackground;
		BackgroundColorAttribute backgroundColorAttribute = (BackgroundColorAttribute)Attribute.GetCustomAttribute(pendingConfig.GetType(), typeof(BackgroundColorAttribute));
		if (backgroundColorAttribute != null)
		{
			uIPanel.BackgroundColor = backgroundColorAttribute.Color;
		}
		int order = 0;
		foreach (PropertyFieldWrapper variable in ConfigManager.GetFieldsAndProperties(pendingConfig))
		{
			if ((!variable.IsProperty || !(variable.Name == "Mode")) && (!Attribute.IsDefined(variable.MemberInfo, typeof(JsonIgnoreAttribute)) || Attribute.IsDefined(variable.MemberInfo, typeof(ShowDespiteJsonIgnoreAttribute))))
			{
				HandleHeader(mainConfigList, ref top, ref order, variable);
				WrapIt(mainConfigList, ref top, variable, pendingConfig, order++);
			}
		}
	}

	public static Tuple<UIElement, UIElement> WrapIt(UIElement parent, ref int top, PropertyFieldWrapper memberInfo, object item, int order, object list = null, Type arrayType = null, int index = -1)
	{
		Type type = memberInfo.Type;
		if (arrayType != null)
		{
			type = arrayType;
		}
		CustomModConfigItemAttribute customUI = ConfigManager.GetCustomAttributeFromMemberThenMemberType<CustomModConfigItemAttribute>(memberInfo, null, null);
		UIElement e;
		if (customUI != null)
		{
			Type customUIType = customUI.Type;
			if (typeof(ConfigElement).IsAssignableFrom(customUIType))
			{
				ConstructorInfo ctor = customUIType.GetConstructor(Array.Empty<Type>());
				e = ((!(ctor != null)) ? new UIText(customUIType.Name + " specified via CustomModConfigItem for " + memberInfo.Name + " does not have an empty constructor.") : (ctor.Invoke(new object[0]) as UIElement));
			}
			else
			{
				e = new UIText(customUIType.Name + " specified via CustomModConfigItem for " + memberInfo.Name + " does not inherit from ConfigElement.");
			}
		}
		else if (item.GetType() == typeof(HeaderAttribute))
		{
			e = new HeaderElement((string)memberInfo.GetValue(item));
		}
		else if (type == typeof(ItemDefinition))
		{
			e = new ItemDefinitionElement();
		}
		else if (type == typeof(ProjectileDefinition))
		{
			e = new ProjectileDefinitionElement();
		}
		else if (type == typeof(NPCDefinition))
		{
			e = new NPCDefinitionElement();
		}
		else if (type == typeof(PrefixDefinition))
		{
			e = new PrefixDefinitionElement();
		}
		else if (type == typeof(BuffDefinition))
		{
			e = new BuffDefinitionElement();
		}
		else if (type == typeof(Color))
		{
			e = new ColorElement();
		}
		else if (type == typeof(Vector2))
		{
			e = new Vector2Element();
		}
		else if (type == typeof(bool))
		{
			e = new BooleanElement();
		}
		else if (type == typeof(float))
		{
			e = new FloatElement();
		}
		else if (type == typeof(byte))
		{
			e = new ByteElement();
		}
		else if (type == typeof(uint))
		{
			e = new UIntElement();
		}
		else if (type == typeof(int))
		{
			e = ((ConfigManager.GetCustomAttributeFromMemberThenMemberType<SliderAttribute>(memberInfo, item, list) == null) ? ((ConfigElement)new IntInputElement()) : ((ConfigElement)new IntRangeElement()));
		}
		else if (type == typeof(string))
		{
			e = ((ConfigManager.GetCustomAttributeFromMemberThenMemberType<OptionStringsAttribute>(memberInfo, item, list) == null) ? ((ConfigElement)new StringInputElement()) : ((ConfigElement)new StringOptionElement()));
		}
		else if (type.IsEnum)
		{
			e = ((list == null) ? ((UIElement)new EnumElement()) : ((UIElement)new UIText(memberInfo.Name + " not handled yet (" + type.Name + ").")));
		}
		else if (type.IsArray)
		{
			e = new ArrayElement();
		}
		else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
		{
			e = new ListElement();
		}
		else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(HashSet<>))
		{
			e = new SetElement();
		}
		else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Dictionary<, >))
		{
			e = new DictionaryElement();
		}
		else if (type.IsClass)
		{
			e = new ObjectElement();
		}
		else if (type.IsValueType && !type.IsPrimitive)
		{
			e = new UIText(memberInfo.Name + " not handled yet (" + type.Name + ") Structs need special UI.");
			e.Height.Pixels += 6f;
			e.Left.Pixels += 4f;
		}
		else
		{
			e = new UIText(memberInfo.Name + " not handled yet (" + type.Name + ")");
			e.Top.Pixels += 6f;
			e.Left.Pixels += 4f;
		}
		if (e != null)
		{
			if (e is ConfigElement configElement)
			{
				configElement.Bind(memberInfo, item, (IList)list, index);
				configElement.OnBind();
			}
			e.Recalculate();
			int elementHeight = (int)e.GetOuterDimensions().Height;
			UIElement container = GetContainer(e, (index == -1) ? order : index);
			container.Height.Pixels = elementHeight;
			if (parent is UIList uiList)
			{
				uiList.Add(container);
				uiList.GetTotalHeight();
			}
			else
			{
				container.Top.Pixels = top;
				container.Width.Pixels = -20f;
				container.Left.Pixels = 20f;
				top += elementHeight + 4;
				parent.Append(container);
				parent.Height.Set(top, 0f);
			}
			Tuple<UIElement, UIElement> tuple = new Tuple<UIElement, UIElement>(container, e);
			if (parent == Interface.modConfig.mainConfigList)
			{
				Interface.modConfig.mainConfigItems.Add(tuple);
			}
			return tuple;
		}
		return null;
	}

	internal static UIElement GetContainer(UIElement containee, int sortid)
	{
		UISortableElement uISortableElement = new UISortableElement(sortid);
		uISortableElement.Width.Set(0f, 1f);
		uISortableElement.Height.Set(30f, 0f);
		uISortableElement.Append(containee);
		return uISortableElement;
	}

	internal static UIPanel MakeSeparateListPanel(object item, object subitem, PropertyFieldWrapper memberInfo, IList array, int index, Func<string> AbridgedTextDisplayFunction)
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0213: Unknown result type (might be due to invalid IL or missing references)
		//IL_0219: Unknown result type (might be due to invalid IL or missing references)
		//IL_021d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0223: Unknown result type (might be due to invalid IL or missing references)
		//IL_0227: Unknown result type (might be due to invalid IL or missing references)
		//IL_022d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0231: Unknown result type (might be due to invalid IL or missing references)
		//IL_0237: Unknown result type (might be due to invalid IL or missing references)
		UIPanel uIPanel = new UIPanel();
		uIPanel.CopyStyle(Interface.modConfig.uIPanel);
		uIPanel.BackgroundColor = UICommon.MainPanelBackground;
		BackgroundColorAttribute bca = ConfigManager.GetCustomAttributeFromMemberThenMemberType<BackgroundColorAttribute>(memberInfo, subitem, null);
		if (bca != null)
		{
			uIPanel.BackgroundColor = bca.Color;
		}
		UIList separateList = new UIList();
		separateList.CopyStyle(Interface.modConfig.mainConfigList);
		separateList.Height.Set(-40f, 1f);
		separateList.Top.Set(40f, 0f);
		uIPanel.Append(separateList);
		int top = 0;
		UIScrollbar uIScrollbar = new UIScrollbar();
		uIScrollbar.SetView(100f, 1000f);
		uIScrollbar.Height.Set(-40f, 1f);
		uIScrollbar.Top.Set(40f, 0f);
		uIScrollbar.HAlign = 1f;
		uIPanel.Append(uIScrollbar);
		separateList.SetScrollbar(uIScrollbar);
		string name = ConfigManager.GetLocalizedLabel(memberInfo);
		if (index != -1)
		{
			name = name + " #" + (index + 1);
		}
		Interface.modConfig.subPageStack.Push(name);
		name = string.Join(" > ", Interface.modConfig.subPageStack.Reverse());
		UITextPanel<string> heading = new UITextPanel<string>(name);
		heading.HAlign = 0f;
		heading.Top.Set(-6f, 0f);
		heading.Height.Set(40f, 0f);
		uIPanel.Append(heading);
		UITextPanel<string> back = new UITextPanel<string>(Language.GetTextValue("tModLoader.ModConfigBack"))
		{
			HAlign = 1f
		};
		back.Width.Set(50f, 0f);
		back.Top.Set(-6f, 0f);
		back.OnLeftClick += delegate
		{
			Interface.modConfig.uIElement.RemoveChild(uIPanel);
			Interface.modConfig.configPanelStack.Pop();
			Interface.modConfig.uIElement.Append(Interface.modConfig.configPanelStack.Peek());
		};
		back.WithFadedMouseOver();
		uIPanel.Append(back);
		int order = 0;
		if (array != null)
		{
			_ = memberInfo.Type.GetGenericArguments()[0].GetMethod("ToString", new Type[0]).DeclaringType != typeof(object);
		}
		else
		{
			_ = memberInfo.Type.GetMethod("ToString", new Type[0]).DeclaringType != typeof(object);
		}
		if (AbridgedTextDisplayFunction != null)
		{
			UITextPanel<FuncStringWrapper> display = new UITextPanel<FuncStringWrapper>(new FuncStringWrapper(AbridgedTextDisplayFunction))
			{
				DrawPanel = true
			};
			display.Recalculate();
			UIElement container = GetContainer(display, order++);
			container.Height.Pixels = (int)display.GetOuterDimensions().Height;
			separateList.Add(container);
		}
		foreach (PropertyFieldWrapper variable in ConfigManager.GetFieldsAndProperties(subitem))
		{
			if (!Attribute.IsDefined(variable.MemberInfo, typeof(JsonIgnoreAttribute)) || Attribute.IsDefined(variable.MemberInfo, typeof(ShowDespiteJsonIgnoreAttribute)))
			{
				HandleHeader(separateList, ref top, ref order, variable);
				WrapIt(separateList, ref top, variable, subitem, order++);
			}
		}
		Interface.modConfig.subPageStack.Pop();
		return uIPanel;
	}

	public static void HandleHeader(UIElement parent, ref int top, ref int order, PropertyFieldWrapper variable)
	{
		HeaderAttribute header = ConfigManager.GetLocalizedHeader(variable.MemberInfo);
		if (header != null)
		{
			PropertyFieldWrapper wrapper = new PropertyFieldWrapper(typeof(HeaderAttribute).GetProperty("Header"));
			WrapIt(parent, ref top, wrapper, header, order++);
		}
	}

	internal static void SwitchToSubConfig(UIPanel separateListPanel)
	{
		Interface.modConfig.uIElement.RemoveChild(Interface.modConfig.configPanelStack.Peek());
		Interface.modConfig.uIElement.Append(separateListPanel);
		Interface.modConfig.configPanelStack.Push(separateListPanel);
	}
}
