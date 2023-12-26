using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;
using ReLogic.OS;
using Terraria.Audio;
using Terraria.GameContent.Creative;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Initializers;
using Terraria.IO;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.UI.Gamepad;

namespace Terraria.GameContent.UI.States;

public class UICharacterCreation : UIState
{
	private enum CategoryId
	{
		CharInfo,
		Clothing,
		HairStyle,
		HairColor,
		Eye,
		Skin,
		Shirt,
		Undershirt,
		Pants,
		Shoes,
		Count
	}

	private enum HSLSliderId
	{
		Hue,
		Saturation,
		Luminance
	}

	private int[] _validClothStyles = new int[10] { 0, 2, 1, 3, 8, 4, 6, 5, 7, 9 };

	private readonly Player _player;

	private UIColoredImageButton[] _colorPickers;

	private CategoryId _selectedPicker;

	private Vector3 _currentColorHSL;

	private UIColoredImageButton _clothingStylesCategoryButton;

	private UIColoredImageButton _hairStylesCategoryButton;

	private UIColoredImageButton _charInfoCategoryButton;

	private UIElement _topContainer;

	private UIElement _middleContainer;

	private UIElement _hslContainer;

	private UIElement _hairstylesContainer;

	private UIElement _clothStylesContainer;

	private UIElement _infoContainer;

	private UIText _hslHexText;

	private UIText _difficultyDescriptionText;

	private UIElement _copyHexButton;

	private UIElement _pasteHexButton;

	private UIElement _randomColorButton;

	private UIElement _copyTemplateButton;

	private UIElement _pasteTemplateButton;

	private UIElement _randomizePlayerButton;

	private UIColoredImageButton _genderMale;

	private UIColoredImageButton _genderFemale;

	private UICharacterNameButton _charName;

	private UIText _helpGlyphLeft;

	private UIText _helpGlyphRight;

	public const int MAX_NAME_LENGTH = 20;

	private UIGamepadHelper _helper;

	private List<int> _foundPoints = new List<int>();

	public UICharacterCreation(Player player)
	{
		_player = player;
		_player.difficulty = 0;
		BuildPage();
	}

	private void BuildPage()
	{
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		RemoveAllChildren();
		int num = 4;
		UIElement uIElement = new UIElement
		{
			Width = StyleDimension.FromPixels(500f),
			Height = StyleDimension.FromPixels(380 + num),
			Top = StyleDimension.FromPixels(220f),
			HAlign = 0.5f,
			VAlign = 0f
		};
		uIElement.SetPadding(0f);
		Append(uIElement);
		UIPanel uIPanel = new UIPanel
		{
			Width = StyleDimension.FromPercent(1f),
			Height = StyleDimension.FromPixels(uIElement.Height.Pixels - 150f - (float)num),
			Top = StyleDimension.FromPixels(50f),
			BackgroundColor = new Color(33, 43, 79) * 0.8f
		};
		uIPanel.SetPadding(0f);
		uIElement.Append(uIPanel);
		MakeBackAndCreatebuttons(uIElement);
		MakeCharPreview(uIPanel);
		UIElement uIElement2 = new UIElement
		{
			Width = StyleDimension.FromPixelsAndPercent(0f, 1f),
			Height = StyleDimension.FromPixelsAndPercent(50f, 0f)
		};
		uIElement2.SetPadding(0f);
		uIElement2.PaddingTop = 4f;
		uIElement2.PaddingBottom = 0f;
		uIPanel.Append(uIElement2);
		UIElement uIElement3 = new UIElement
		{
			Top = StyleDimension.FromPixelsAndPercent(uIElement2.Height.Pixels + 6f, 0f),
			Width = StyleDimension.FromPixelsAndPercent(0f, 1f),
			Height = StyleDimension.FromPixelsAndPercent(uIPanel.Height.Pixels - 70f, 0f)
		};
		uIElement3.SetPadding(0f);
		uIElement3.PaddingTop = 3f;
		uIElement3.PaddingBottom = 0f;
		uIPanel.Append(uIElement3);
		_topContainer = uIElement2;
		_middleContainer = uIElement3;
		MakeInfoMenu(uIElement3);
		MakeHSLMenu(uIElement3);
		MakeHairsylesMenu(uIElement3);
		MakeClothStylesMenu(uIElement3);
		MakeCategoriesBar(uIElement2);
		Click_CharInfo(null, null);
	}

	private void MakeCharPreview(UIPanel container)
	{
		float num = 70f;
		for (float num2 = 0f; num2 <= 1f; num2 += 1f)
		{
			UICharacter element = new UICharacter(_player, animated: true, hasBackPanel: false, 1.5f)
			{
				Width = StyleDimension.FromPixels(80f),
				Height = StyleDimension.FromPixelsAndPercent(80f, 0f),
				Top = StyleDimension.FromPixelsAndPercent(0f - num, 0f),
				VAlign = 0f,
				HAlign = 0.5f
			};
			container.Append(element);
		}
	}

	private void MakeHairsylesMenu(UIElement middleInnerPanel)
	{
		Main.Hairstyles.UpdateUnlocks();
		UIElement uIElement = new UIElement
		{
			Width = StyleDimension.FromPixelsAndPercent(-10f, 1f),
			Height = StyleDimension.FromPixelsAndPercent(0f, 1f),
			HAlign = 0.5f,
			VAlign = 0.5f,
			Top = StyleDimension.FromPixels(6f)
		};
		middleInnerPanel.Append(uIElement);
		uIElement.SetPadding(0f);
		UIList uIList = new UIList
		{
			Width = StyleDimension.FromPixelsAndPercent(-18f, 1f),
			Height = StyleDimension.FromPixelsAndPercent(-6f, 1f)
		};
		uIList.SetPadding(4f);
		uIElement.Append(uIList);
		UIScrollbar uIScrollbar = new UIScrollbar
		{
			HAlign = 1f,
			Height = StyleDimension.FromPixelsAndPercent(-30f, 1f),
			Top = StyleDimension.FromPixels(10f)
		};
		uIScrollbar.SetView(100f, 1000f);
		uIList.SetScrollbar(uIScrollbar);
		uIElement.Append(uIScrollbar);
		int count = Main.Hairstyles.AvailableHairstyles.Count;
		UIElement uIElement2 = new UIElement
		{
			Width = StyleDimension.FromPixelsAndPercent(0f, 1f),
			Height = StyleDimension.FromPixelsAndPercent(48 * (count / 10 + ((count % 10 != 0) ? 1 : 0)), 0f)
		};
		uIList.Add(uIElement2);
		uIElement2.SetPadding(0f);
		for (int i = 0; i < count; i++)
		{
			UIHairStyleButton uIHairStyleButton = new UIHairStyleButton(_player, Main.Hairstyles.AvailableHairstyles[i])
			{
				Left = StyleDimension.FromPixels((float)(i % 10) * 46f + 6f),
				Top = StyleDimension.FromPixels((float)(i / 10) * 48f + 1f)
			};
			uIHairStyleButton.SetSnapPoint("Middle", i);
			uIHairStyleButton.SkipRenderingContent(i);
			uIElement2.Append(uIHairStyleButton);
		}
		_hairstylesContainer = uIElement;
	}

	private void MakeClothStylesMenu(UIElement middleInnerPanel)
	{
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_0191: Unknown result type (might be due to invalid IL or missing references)
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		UIElement uIElement = new UIElement
		{
			Width = StyleDimension.FromPixelsAndPercent(-10f, 1f),
			Height = StyleDimension.FromPixelsAndPercent(0f, 1f),
			HAlign = 0.5f,
			VAlign = 0.5f
		};
		middleInnerPanel.Append(uIElement);
		uIElement.SetPadding(0f);
		int num = 15;
		for (int i = 0; i < _validClothStyles.Length; i++)
		{
			int num2 = 0;
			if (i >= _validClothStyles.Length / 2)
			{
				num2 = 20;
			}
			UIClothStyleButton uIClothStyleButton = new UIClothStyleButton(_player, _validClothStyles[i])
			{
				Left = StyleDimension.FromPixels((float)i * 46f + (float)num2 + 6f),
				Top = StyleDimension.FromPixels(num)
			};
			uIClothStyleButton.OnLeftMouseDown += Click_CharClothStyle;
			uIClothStyleButton.SetSnapPoint("Middle", i);
			uIElement.Append(uIClothStyleButton);
		}
		for (int j = 0; j < 2; j++)
		{
			int num3 = 0;
			if (j >= 1)
			{
				num3 = 20;
			}
			UIHorizontalSeparator element = new UIHorizontalSeparator
			{
				Left = StyleDimension.FromPixels((float)j * 230f + (float)num3 + 6f),
				Top = StyleDimension.FromPixels(num + 86),
				Width = StyleDimension.FromPixelsAndPercent(230f, 0f),
				Color = Color.Lerp(Color.White, new Color(63, 65, 151, 255), 0.85f) * 0.9f
			};
			uIElement.Append(element);
			UIColoredImageButton uIColoredImageButton = CreatePickerWithoutClick(CategoryId.Clothing, "Images/UI/CharCreation/" + ((j == 0) ? "ClothStyleMale" : "ClothStyleFemale"), 0f, 0f);
			uIColoredImageButton.Top = StyleDimension.FromPixelsAndPercent(num + 92, 0f);
			uIColoredImageButton.Left = StyleDimension.FromPixels((float)j * 230f + 92f + (float)num3 + 6f);
			uIColoredImageButton.HAlign = 0f;
			uIColoredImageButton.VAlign = 0f;
			uIElement.Append(uIColoredImageButton);
			if (j == 0)
			{
				uIColoredImageButton.OnLeftMouseDown += Click_CharGenderMale;
				_genderMale = uIColoredImageButton;
			}
			else
			{
				uIColoredImageButton.OnLeftMouseDown += Click_CharGenderFemale;
				_genderFemale = uIColoredImageButton;
			}
			uIColoredImageButton.SetSnapPoint("Low", j * 4);
		}
		UIElement uIElement2 = new UIElement
		{
			Width = StyleDimension.FromPixels(130f),
			Height = StyleDimension.FromPixels(50f),
			HAlign = 0.5f,
			VAlign = 1f
		};
		uIElement.Append(uIElement2);
		UIColoredImageButton uIColoredImageButton2 = new UIColoredImageButton(Main.Assets.Request<Texture2D>("Images/UI/CharCreation/Copy"), isSmall: true)
		{
			VAlign = 0.5f,
			HAlign = 0f,
			Left = StyleDimension.FromPixelsAndPercent(0f, 0f)
		};
		uIColoredImageButton2.OnLeftMouseDown += Click_CopyPlayerTemplate;
		uIElement2.Append(uIColoredImageButton2);
		_copyTemplateButton = uIColoredImageButton2;
		UIColoredImageButton uIColoredImageButton3 = new UIColoredImageButton(Main.Assets.Request<Texture2D>("Images/UI/CharCreation/Paste"), isSmall: true)
		{
			VAlign = 0.5f,
			HAlign = 0.5f
		};
		uIColoredImageButton3.OnLeftMouseDown += Click_PastePlayerTemplate;
		uIElement2.Append(uIColoredImageButton3);
		_pasteTemplateButton = uIColoredImageButton3;
		UIColoredImageButton uIColoredImageButton4 = new UIColoredImageButton(Main.Assets.Request<Texture2D>("Images/UI/CharCreation/Randomize"), isSmall: true)
		{
			VAlign = 0.5f,
			HAlign = 1f
		};
		uIColoredImageButton4.OnLeftMouseDown += Click_RandomizePlayer;
		uIElement2.Append(uIColoredImageButton4);
		_randomizePlayerButton = uIColoredImageButton4;
		uIColoredImageButton2.SetSnapPoint("Low", 1);
		uIColoredImageButton3.SetSnapPoint("Low", 2);
		uIColoredImageButton4.SetSnapPoint("Low", 3);
		_clothStylesContainer = uIElement;
	}

	private void MakeCategoriesBar(UIElement categoryContainer)
	{
		//IL_0239: Unknown result type (might be due to invalid IL or missing references)
		//IL_024c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0256: Unknown result type (might be due to invalid IL or missing references)
		//IL_0260: Unknown result type (might be due to invalid IL or missing references)
		//IL_0265: Unknown result type (might be due to invalid IL or missing references)
		float xPositionStart = -240f;
		float xPositionPerId = 48f;
		_colorPickers = new UIColoredImageButton[10];
		categoryContainer.Append(CreateColorPicker(CategoryId.HairColor, "Images/UI/CharCreation/ColorHair", xPositionStart, xPositionPerId));
		categoryContainer.Append(CreateColorPicker(CategoryId.Eye, "Images/UI/CharCreation/ColorEye", xPositionStart, xPositionPerId));
		categoryContainer.Append(CreateColorPicker(CategoryId.Skin, "Images/UI/CharCreation/ColorSkin", xPositionStart, xPositionPerId));
		categoryContainer.Append(CreateColorPicker(CategoryId.Shirt, "Images/UI/CharCreation/ColorShirt", xPositionStart, xPositionPerId));
		categoryContainer.Append(CreateColorPicker(CategoryId.Undershirt, "Images/UI/CharCreation/ColorUndershirt", xPositionStart, xPositionPerId));
		categoryContainer.Append(CreateColorPicker(CategoryId.Pants, "Images/UI/CharCreation/ColorPants", xPositionStart, xPositionPerId));
		categoryContainer.Append(CreateColorPicker(CategoryId.Shoes, "Images/UI/CharCreation/ColorShoes", xPositionStart, xPositionPerId));
		_colorPickers[4].SetMiddleTexture(Main.Assets.Request<Texture2D>("Images/UI/CharCreation/ColorEyeBack"));
		_clothingStylesCategoryButton = CreatePickerWithoutClick(CategoryId.Clothing, "Images/UI/CharCreation/ClothStyleMale", xPositionStart, xPositionPerId);
		_clothingStylesCategoryButton.OnLeftMouseDown += Click_ClothStyles;
		_clothingStylesCategoryButton.SetSnapPoint("Top", 1);
		categoryContainer.Append(_clothingStylesCategoryButton);
		_hairStylesCategoryButton = CreatePickerWithoutClick(CategoryId.HairStyle, "Images/UI/CharCreation/HairStyle_Hair", xPositionStart, xPositionPerId);
		_hairStylesCategoryButton.OnLeftMouseDown += Click_HairStyles;
		_hairStylesCategoryButton.SetMiddleTexture(Main.Assets.Request<Texture2D>("Images/UI/CharCreation/HairStyle_Arrow"));
		_hairStylesCategoryButton.SetSnapPoint("Top", 2);
		categoryContainer.Append(_hairStylesCategoryButton);
		_charInfoCategoryButton = CreatePickerWithoutClick(CategoryId.CharInfo, "Images/UI/CharCreation/CharInfo", xPositionStart, xPositionPerId);
		_charInfoCategoryButton.OnLeftMouseDown += Click_CharInfo;
		_charInfoCategoryButton.SetSnapPoint("Top", 0);
		categoryContainer.Append(_charInfoCategoryButton);
		UpdateColorPickers();
		UIHorizontalSeparator element = new UIHorizontalSeparator
		{
			Width = StyleDimension.FromPixelsAndPercent(-20f, 1f),
			Top = StyleDimension.FromPixels(6f),
			VAlign = 1f,
			HAlign = 0.5f,
			Color = Color.Lerp(Color.White, new Color(63, 65, 151, 255), 0.85f) * 0.9f
		};
		categoryContainer.Append(element);
		int num = 21;
		UIText uIText = new UIText(PlayerInput.GenerateInputTag_ForCurrentGamemode(tagForGameplay: false, "HotbarMinus"))
		{
			Left = new StyleDimension(-num, 0f),
			VAlign = 0.5f,
			Top = new StyleDimension(-4f, 0f)
		};
		categoryContainer.Append(uIText);
		UIText uIText2 = new UIText(PlayerInput.GenerateInputTag_ForCurrentGamemode(tagForGameplay: false, "HotbarMinus"))
		{
			HAlign = 1f,
			Left = new StyleDimension(12 + num, 0f),
			VAlign = 0.5f,
			Top = new StyleDimension(-4f, 0f)
		};
		categoryContainer.Append(uIText2);
		_helpGlyphLeft = uIText;
		_helpGlyphRight = uIText2;
		categoryContainer.OnUpdate += UpdateHelpGlyphs;
	}

	private void UpdateHelpGlyphs(UIElement element)
	{
		string text = "";
		string text2 = "";
		if (PlayerInput.UsingGamepad)
		{
			text = PlayerInput.GenerateInputTag_ForCurrentGamemode(tagForGameplay: false, "HotbarMinus");
			text2 = PlayerInput.GenerateInputTag_ForCurrentGamemode(tagForGameplay: false, "HotbarPlus");
		}
		_helpGlyphLeft.SetText(text);
		_helpGlyphRight.SetText(text2);
	}

	private UIColoredImageButton CreateColorPicker(CategoryId id, string texturePath, float xPositionStart, float xPositionPerId)
	{
		UIColoredImageButton uIColoredImageButton = new UIColoredImageButton(Main.Assets.Request<Texture2D>(texturePath));
		_colorPickers[(int)id] = uIColoredImageButton;
		uIColoredImageButton.VAlign = 0f;
		uIColoredImageButton.HAlign = 0f;
		uIColoredImageButton.Left.Set(xPositionStart + (float)id * xPositionPerId, 0.5f);
		uIColoredImageButton.OnLeftMouseDown += Click_ColorPicker;
		uIColoredImageButton.SetSnapPoint("Top", (int)id);
		return uIColoredImageButton;
	}

	private UIColoredImageButton CreatePickerWithoutClick(CategoryId id, string texturePath, float xPositionStart, float xPositionPerId)
	{
		UIColoredImageButton uIColoredImageButton = new UIColoredImageButton(Main.Assets.Request<Texture2D>(texturePath));
		uIColoredImageButton.VAlign = 0f;
		uIColoredImageButton.HAlign = 0f;
		uIColoredImageButton.Left.Set(xPositionStart + (float)id * xPositionPerId, 0.5f);
		return uIColoredImageButton;
	}

	private void MakeInfoMenu(UIElement parentContainer)
	{
		//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_021c: Unknown result type (might be due to invalid IL or missing references)
		//IL_028d: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_036a: Unknown result type (might be due to invalid IL or missing references)
		UIElement uIElement = new UIElement
		{
			Width = StyleDimension.FromPixelsAndPercent(0f, 1f),
			Height = StyleDimension.FromPixelsAndPercent(0f, 1f),
			HAlign = 0.5f,
			VAlign = 0f
		};
		uIElement.SetPadding(10f);
		uIElement.PaddingBottom = 0f;
		uIElement.PaddingTop = 0f;
		parentContainer.Append(uIElement);
		UICharacterNameButton uICharacterNameButton = new UICharacterNameButton(Language.GetText("UI.WorldCreationName"), Language.GetText("UI.PlayerEmptyName"));
		uICharacterNameButton.Width = StyleDimension.FromPixelsAndPercent(0f, 1f);
		uICharacterNameButton.HAlign = 0.5f;
		uIElement.Append(uICharacterNameButton);
		_charName = uICharacterNameButton;
		uICharacterNameButton.OnLeftMouseDown += Click_Naming;
		uICharacterNameButton.SetSnapPoint("Middle", 0);
		float num = 4f;
		float num2 = 0f;
		float num3 = 0.4f;
		UIElement uIElement2 = new UIElement
		{
			HAlign = 0f,
			VAlign = 1f,
			Width = StyleDimension.FromPixelsAndPercent(0f - num, num3),
			Height = StyleDimension.FromPixelsAndPercent(-50f, 1f)
		};
		uIElement2.SetPadding(0f);
		uIElement.Append(uIElement2);
		UISlicedImage uISlicedImage = new UISlicedImage(Main.Assets.Request<Texture2D>("Images/UI/CharCreation/CategoryPanelHighlight"))
		{
			HAlign = 1f,
			VAlign = 1f,
			Width = StyleDimension.FromPixelsAndPercent((0f - num) * 2f, 1f - num3),
			Left = StyleDimension.FromPixels(0f - num),
			Height = StyleDimension.FromPixelsAndPercent(uIElement2.Height.Pixels, uIElement2.Height.Precent)
		};
		uISlicedImage.SetSliceDepths(10);
		uISlicedImage.Color = Color.LightGray * 0.7f;
		uIElement.Append(uISlicedImage);
		float num4 = 4f;
		UIDifficultyButton uIDifficultyButton = new UIDifficultyButton(_player, Lang.menu[26], Lang.menu[31], 0, Color.Cyan)
		{
			HAlign = 0f,
			VAlign = 1f / (num4 - 1f),
			Width = StyleDimension.FromPixelsAndPercent(0f, 1f),
			Height = StyleDimension.FromPixelsAndPercent(0f - num2, 1f / num4)
		};
		UIDifficultyButton uIDifficultyButton2 = new UIDifficultyButton(_player, Lang.menu[25], Lang.menu[30], 1, Main.mcColor)
		{
			HAlign = 0f,
			VAlign = 2f / (num4 - 1f),
			Width = StyleDimension.FromPixelsAndPercent(0f, 1f),
			Height = StyleDimension.FromPixelsAndPercent(0f - num2, 1f / num4)
		};
		UIDifficultyButton uIDifficultyButton3 = new UIDifficultyButton(_player, Lang.menu[24], Lang.menu[29], 2, Main.hcColor)
		{
			HAlign = 0f,
			VAlign = 1f,
			Width = StyleDimension.FromPixelsAndPercent(0f, 1f),
			Height = StyleDimension.FromPixelsAndPercent(0f - num2, 1f / num4)
		};
		UIDifficultyButton uIDifficultyButton4 = new UIDifficultyButton(_player, Language.GetText("UI.Creative"), Language.GetText("UI.CreativeDescriptionPlayer"), 3, Main.creativeModeColor)
		{
			HAlign = 0f,
			VAlign = 0f,
			Width = StyleDimension.FromPixelsAndPercent(0f, 1f),
			Height = StyleDimension.FromPixelsAndPercent(0f - num2, 1f / num4)
		};
		UIText uIText = new UIText(Lang.menu[26])
		{
			HAlign = 0f,
			VAlign = 0.5f,
			Width = StyleDimension.FromPixelsAndPercent(0f, 1f),
			Height = StyleDimension.FromPixelsAndPercent(0f, 1f),
			Top = StyleDimension.FromPixelsAndPercent(15f, 0f),
			IsWrapped = true
		};
		uIText.PaddingLeft = 20f;
		uIText.PaddingRight = 20f;
		uISlicedImage.Append(uIText);
		uIElement2.Append(uIDifficultyButton4);
		uIElement2.Append(uIDifficultyButton);
		uIElement2.Append(uIDifficultyButton2);
		uIElement2.Append(uIDifficultyButton3);
		_infoContainer = uIElement;
		_difficultyDescriptionText = uIText;
		uIDifficultyButton4.OnLeftMouseDown += UpdateDifficultyDescription;
		uIDifficultyButton.OnLeftMouseDown += UpdateDifficultyDescription;
		uIDifficultyButton2.OnLeftMouseDown += UpdateDifficultyDescription;
		uIDifficultyButton3.OnLeftMouseDown += UpdateDifficultyDescription;
		UpdateDifficultyDescription(null, null);
		uIDifficultyButton4.SetSnapPoint("Middle", 1);
		uIDifficultyButton.SetSnapPoint("Middle", 2);
		uIDifficultyButton2.SetSnapPoint("Middle", 3);
		uIDifficultyButton3.SetSnapPoint("Middle", 4);
	}

	private void UpdateDifficultyDescription(UIMouseEvent evt, UIElement listeningElement)
	{
		LocalizedText text = Lang.menu[31];
		switch (_player.difficulty)
		{
		case 0:
			text = Lang.menu[31];
			break;
		case 1:
			text = Lang.menu[30];
			break;
		case 2:
			text = Lang.menu[29];
			break;
		case 3:
			text = Language.GetText("UI.CreativeDescriptionPlayer");
			break;
		}
		_difficultyDescriptionText.SetText(text);
	}

	private void MakeHSLMenu(UIElement parentContainer)
	{
		UIElement uIElement = new UIElement
		{
			Width = StyleDimension.FromPixelsAndPercent(220f, 0f),
			Height = StyleDimension.FromPixelsAndPercent(158f, 0f),
			HAlign = 0.5f,
			VAlign = 0f
		};
		uIElement.SetPadding(0f);
		parentContainer.Append(uIElement);
		UIElement uIElement2 = new UIPanel
		{
			Width = StyleDimension.FromPixelsAndPercent(220f, 0f),
			Height = StyleDimension.FromPixelsAndPercent(104f, 0f),
			HAlign = 0.5f,
			VAlign = 0f,
			Top = StyleDimension.FromPixelsAndPercent(10f, 0f)
		};
		uIElement2.SetPadding(0f);
		uIElement2.PaddingTop = 3f;
		uIElement.Append(uIElement2);
		uIElement2.Append(CreateHSLSlider(HSLSliderId.Hue));
		uIElement2.Append(CreateHSLSlider(HSLSliderId.Saturation));
		uIElement2.Append(CreateHSLSlider(HSLSliderId.Luminance));
		UIPanel uIPanel = new UIPanel
		{
			VAlign = 1f,
			HAlign = 1f,
			Width = StyleDimension.FromPixelsAndPercent(100f, 0f),
			Height = StyleDimension.FromPixelsAndPercent(32f, 0f)
		};
		UIText uIText = new UIText("FFFFFF")
		{
			VAlign = 0.5f,
			HAlign = 0.5f
		};
		uIPanel.Append(uIText);
		uIElement.Append(uIPanel);
		UIColoredImageButton uIColoredImageButton = new UIColoredImageButton(Main.Assets.Request<Texture2D>("Images/UI/CharCreation/Copy"), isSmall: true)
		{
			VAlign = 1f,
			HAlign = 0f,
			Left = StyleDimension.FromPixelsAndPercent(0f, 0f)
		};
		uIColoredImageButton.OnLeftMouseDown += Click_CopyHex;
		uIElement.Append(uIColoredImageButton);
		_copyHexButton = uIColoredImageButton;
		UIColoredImageButton uIColoredImageButton2 = new UIColoredImageButton(Main.Assets.Request<Texture2D>("Images/UI/CharCreation/Paste"), isSmall: true)
		{
			VAlign = 1f,
			HAlign = 0f,
			Left = StyleDimension.FromPixelsAndPercent(40f, 0f)
		};
		uIColoredImageButton2.OnLeftMouseDown += Click_PasteHex;
		uIElement.Append(uIColoredImageButton2);
		_pasteHexButton = uIColoredImageButton2;
		UIColoredImageButton uIColoredImageButton3 = new UIColoredImageButton(Main.Assets.Request<Texture2D>("Images/UI/CharCreation/Randomize"), isSmall: true)
		{
			VAlign = 1f,
			HAlign = 0f,
			Left = StyleDimension.FromPixelsAndPercent(80f, 0f)
		};
		uIColoredImageButton3.OnLeftMouseDown += Click_RandomizeSingleColor;
		uIElement.Append(uIColoredImageButton3);
		_randomColorButton = uIColoredImageButton3;
		_hslContainer = uIElement;
		_hslHexText = uIText;
		uIColoredImageButton.SetSnapPoint("Low", 0);
		uIColoredImageButton2.SetSnapPoint("Low", 1);
		uIColoredImageButton3.SetSnapPoint("Low", 2);
	}

	private UIColoredSlider CreateHSLSlider(HSLSliderId id)
	{
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		UIColoredSlider uIColoredSlider = CreateHSLSliderButtonBase(id);
		uIColoredSlider.VAlign = 0f;
		uIColoredSlider.HAlign = 0f;
		uIColoredSlider.Width = StyleDimension.FromPixelsAndPercent(-10f, 1f);
		uIColoredSlider.Top.Set(30 * (int)id, 0f);
		uIColoredSlider.OnLeftMouseDown += Click_ColorPicker;
		uIColoredSlider.SetSnapPoint("Middle", (int)id, (Vector2?)null, (Vector2?)new Vector2(0f, 20f));
		return uIColoredSlider;
	}

	private UIColoredSlider CreateHSLSliderButtonBase(HSLSliderId id)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		return id switch
		{
			HSLSliderId.Saturation => new UIColoredSlider(LocalizedText.Empty, () => GetHSLSliderPosition(HSLSliderId.Saturation), delegate(float x)
			{
				UpdateHSLValue(HSLSliderId.Saturation, x);
			}, UpdateHSL_S, (float x) => GetHSLSliderColorAt(HSLSliderId.Saturation, x), Color.Transparent), 
			HSLSliderId.Luminance => new UIColoredSlider(LocalizedText.Empty, () => GetHSLSliderPosition(HSLSliderId.Luminance), delegate(float x)
			{
				UpdateHSLValue(HSLSliderId.Luminance, x);
			}, UpdateHSL_L, (float x) => GetHSLSliderColorAt(HSLSliderId.Luminance, x), Color.Transparent), 
			_ => new UIColoredSlider(LocalizedText.Empty, () => GetHSLSliderPosition(HSLSliderId.Hue), delegate(float x)
			{
				UpdateHSLValue(HSLSliderId.Hue, x);
			}, UpdateHSL_H, (float x) => GetHSLSliderColorAt(HSLSliderId.Hue, x), Color.Transparent), 
		};
	}

	private void UpdateHSL_H()
	{
		float value = UILinksInitializer.HandleSliderHorizontalInput(_currentColorHSL.X, 0f, 1f, PlayerInput.CurrentProfile.InterfaceDeadzoneX, 0.35f);
		UpdateHSLValue(HSLSliderId.Hue, value);
	}

	private void UpdateHSL_S()
	{
		float value = UILinksInitializer.HandleSliderHorizontalInput(_currentColorHSL.Y, 0f, 1f, PlayerInput.CurrentProfile.InterfaceDeadzoneX, 0.35f);
		UpdateHSLValue(HSLSliderId.Saturation, value);
	}

	private void UpdateHSL_L()
	{
		float value = UILinksInitializer.HandleSliderHorizontalInput(_currentColorHSL.Z, 0f, 1f, PlayerInput.CurrentProfile.InterfaceDeadzoneX, 0.35f);
		UpdateHSLValue(HSLSliderId.Luminance, value);
	}

	private float GetHSLSliderPosition(HSLSliderId id)
	{
		return id switch
		{
			HSLSliderId.Hue => _currentColorHSL.X, 
			HSLSliderId.Saturation => _currentColorHSL.Y, 
			HSLSliderId.Luminance => _currentColorHSL.Z, 
			_ => 1f, 
		};
	}

	private void UpdateHSLValue(HSLSliderId id, float value)
	{
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		switch (id)
		{
		case HSLSliderId.Hue:
			_currentColorHSL.X = value;
			break;
		case HSLSliderId.Saturation:
			_currentColorHSL.Y = value;
			break;
		case HSLSliderId.Luminance:
			_currentColorHSL.Z = value;
			break;
		}
		Color color = ScaledHslToRgb(_currentColorHSL.X, _currentColorHSL.Y, _currentColorHSL.Z);
		ApplyPendingColor(color);
		_colorPickers[(int)_selectedPicker]?.SetColor(color);
		if (_selectedPicker == CategoryId.HairColor)
		{
			_hairStylesCategoryButton.SetColor(color);
		}
		UpdateHexText(color);
	}

	private Color GetHSLSliderColorAt(HSLSliderId id, float pointAt)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		return (Color)(id switch
		{
			HSLSliderId.Hue => ScaledHslToRgb(pointAt, 1f, 0.5f), 
			HSLSliderId.Saturation => ScaledHslToRgb(_currentColorHSL.X, pointAt, _currentColorHSL.Z), 
			HSLSliderId.Luminance => ScaledHslToRgb(_currentColorHSL.X, _currentColorHSL.Y, pointAt), 
			_ => Color.White, 
		});
	}

	private void ApplyPendingColor(Color pendingColor)
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		switch (_selectedPicker)
		{
		case CategoryId.HairColor:
			_player.hairColor = pendingColor;
			break;
		case CategoryId.Eye:
			_player.eyeColor = pendingColor;
			break;
		case CategoryId.Skin:
			_player.skinColor = pendingColor;
			break;
		case CategoryId.Shirt:
			_player.shirtColor = pendingColor;
			break;
		case CategoryId.Undershirt:
			_player.underShirtColor = pendingColor;
			break;
		case CategoryId.Pants:
			_player.pantsColor = pendingColor;
			break;
		case CategoryId.Shoes:
			_player.shoeColor = pendingColor;
			break;
		}
	}

	private void UpdateHexText(Color pendingColor)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		_hslHexText.SetText(GetHexText(pendingColor));
	}

	private static string GetHexText(Color pendingColor)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		return "#" + pendingColor.Hex3().ToUpper();
	}

	private void MakeBackAndCreatebuttons(UIElement outerContainer)
	{
		UITextPanel<LocalizedText> uITextPanel = new UITextPanel<LocalizedText>(Language.GetText("UI.Back"), 0.7f, large: true)
		{
			Width = StyleDimension.FromPixelsAndPercent(-10f, 0.5f),
			Height = StyleDimension.FromPixels(50f),
			VAlign = 1f,
			HAlign = 0f,
			Top = StyleDimension.FromPixels(-45f)
		};
		uITextPanel.OnMouseOver += FadedMouseOver;
		uITextPanel.OnMouseOut += FadedMouseOut;
		uITextPanel.OnLeftMouseDown += Click_GoBack;
		uITextPanel.SetSnapPoint("Back", 0);
		outerContainer.Append(uITextPanel);
		UITextPanel<LocalizedText> uITextPanel2 = new UITextPanel<LocalizedText>(Language.GetText("UI.Create"), 0.7f, large: true)
		{
			Width = StyleDimension.FromPixelsAndPercent(-10f, 0.5f),
			Height = StyleDimension.FromPixels(50f),
			VAlign = 1f,
			HAlign = 1f,
			Top = StyleDimension.FromPixels(-45f)
		};
		uITextPanel2.OnMouseOver += FadedMouseOver;
		uITextPanel2.OnMouseOut += FadedMouseOut;
		uITextPanel2.OnLeftMouseDown += Click_NamingAndCreating;
		uITextPanel2.SetSnapPoint("Create", 0);
		outerContainer.Append(uITextPanel2);
	}

	private void Click_GoBack(UIMouseEvent evt, UIElement listeningElement)
	{
		SoundEngine.PlaySound(11);
		Main.OpenCharacterSelectUI();
	}

	private void FadedMouseOver(UIMouseEvent evt, UIElement listeningElement)
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		SoundEngine.PlaySound(12);
		((UIPanel)evt.Target).BackgroundColor = new Color(73, 94, 171);
		((UIPanel)evt.Target).BorderColor = Colors.FancyUIFatButtonMouseOver;
	}

	private void FadedMouseOut(UIMouseEvent evt, UIElement listeningElement)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		((UIPanel)evt.Target).BackgroundColor = new Color(63, 82, 151) * 0.8f;
		((UIPanel)evt.Target).BorderColor = Color.Black;
	}

	private void Click_ColorPicker(UIMouseEvent evt, UIElement listeningElement)
	{
		SoundEngine.PlaySound(12);
		for (int i = 0; i < _colorPickers.Length; i++)
		{
			if (_colorPickers[i] == evt.Target)
			{
				SelectColorPicker((CategoryId)i);
				break;
			}
		}
	}

	private void Click_ClothStyles(UIMouseEvent evt, UIElement listeningElement)
	{
		SoundEngine.PlaySound(12);
		UnselectAllCategories();
		_selectedPicker = CategoryId.Clothing;
		_middleContainer.Append(_clothStylesContainer);
		_clothingStylesCategoryButton.SetSelected(selected: true);
		UpdateSelectedGender();
	}

	private void Click_HairStyles(UIMouseEvent evt, UIElement listeningElement)
	{
		SoundEngine.PlaySound(12);
		UnselectAllCategories();
		_selectedPicker = CategoryId.HairStyle;
		_middleContainer.Append(_hairstylesContainer);
		_hairStylesCategoryButton.SetSelected(selected: true);
	}

	private void Click_CharInfo(UIMouseEvent evt, UIElement listeningElement)
	{
		SoundEngine.PlaySound(12);
		UnselectAllCategories();
		_selectedPicker = CategoryId.CharInfo;
		_middleContainer.Append(_infoContainer);
		_charInfoCategoryButton.SetSelected(selected: true);
	}

	private void Click_CharClothStyle(UIMouseEvent evt, UIElement listeningElement)
	{
		_clothingStylesCategoryButton.SetImageWithoutSettingSize(Main.Assets.Request<Texture2D>("Images/UI/CharCreation/" + (_player.Male ? "ClothStyleMale" : "ClothStyleFemale")));
		UpdateSelectedGender();
	}

	private void Click_CharGenderMale(UIMouseEvent evt, UIElement listeningElement)
	{
		SoundEngine.PlaySound(12);
		_player.Male = true;
		Click_CharClothStyle(evt, listeningElement);
		UpdateSelectedGender();
	}

	private void Click_CharGenderFemale(UIMouseEvent evt, UIElement listeningElement)
	{
		SoundEngine.PlaySound(12);
		_player.Male = false;
		Click_CharClothStyle(evt, listeningElement);
		UpdateSelectedGender();
	}

	private void UpdateSelectedGender()
	{
		_genderMale.SetSelected(_player.Male);
		_genderFemale.SetSelected(!_player.Male);
	}

	private void Click_CopyHex(UIMouseEvent evt, UIElement listeningElement)
	{
		SoundEngine.PlaySound(12);
		Platform.Get<IClipboard>().Value = _hslHexText.Text;
	}

	private void Click_PasteHex(UIMouseEvent evt, UIElement listeningElement)
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		SoundEngine.PlaySound(12);
		string value = Platform.Get<IClipboard>().Value;
		if (GetHexColor(value, out var hsl))
		{
			ApplyPendingColor(ScaledHslToRgb(hsl.X, hsl.Y, hsl.Z));
			_currentColorHSL = hsl;
			UpdateHexText(ScaledHslToRgb(hsl.X, hsl.Y, hsl.Z));
			UpdateColorPickers();
		}
	}

	private void Click_CopyPlayerTemplate(UIMouseEvent evt, UIElement listeningElement)
	{
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Expected O, but got Unknown
		SoundEngine.PlaySound(12);
		string text = JsonConvert.SerializeObject((object)new Dictionary<string, object>
		{
			{ "version", 1 },
			{ "hairStyle", _player.hair },
			{ "clothingStyle", _player.skinVariant },
			{
				"hairColor",
				GetHexText(_player.hairColor)
			},
			{
				"eyeColor",
				GetHexText(_player.eyeColor)
			},
			{
				"skinColor",
				GetHexText(_player.skinColor)
			},
			{
				"shirtColor",
				GetHexText(_player.shirtColor)
			},
			{
				"underShirtColor",
				GetHexText(_player.underShirtColor)
			},
			{
				"pantsColor",
				GetHexText(_player.pantsColor)
			},
			{
				"shoeColor",
				GetHexText(_player.shoeColor)
			}
		}, new JsonSerializerSettings
		{
			TypeNameHandling = (TypeNameHandling)4,
			MetadataPropertyHandling = (MetadataPropertyHandling)1,
			Formatting = (Formatting)1
		});
		PlayerInput.PrettyPrintProfiles(ref text);
		Platform.Get<IClipboard>().Value = text;
	}

	private void Click_PastePlayerTemplate(UIMouseEvent evt, UIElement listeningElement)
	{
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_019b: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0201: Unknown result type (might be due to invalid IL or missing references)
		//IL_0203: Unknown result type (might be due to invalid IL or missing references)
		//IL_0208: Unknown result type (might be due to invalid IL or missing references)
		//IL_0234: Unknown result type (might be due to invalid IL or missing references)
		//IL_0236: Unknown result type (might be due to invalid IL or missing references)
		//IL_023b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0267: Unknown result type (might be due to invalid IL or missing references)
		//IL_0269: Unknown result type (might be due to invalid IL or missing references)
		//IL_026e: Unknown result type (might be due to invalid IL or missing references)
		//IL_029a: Unknown result type (might be due to invalid IL or missing references)
		//IL_029c: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a1: Unknown result type (might be due to invalid IL or missing references)
		SoundEngine.PlaySound(12);
		try
		{
			string value = Platform.Get<IClipboard>().Value;
			int num = value.IndexOf("{");
			if (num == -1)
			{
				return;
			}
			value = value.Substring(num);
			int num2 = value.LastIndexOf("}");
			if (num2 == -1)
			{
				return;
			}
			value = value.Substring(0, num2 + 1);
			Dictionary<string, object> dictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(value);
			if (dictionary == null)
			{
				return;
			}
			Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
			foreach (KeyValuePair<string, object> item in dictionary)
			{
				dictionary2[item.Key.ToLower()] = item.Value;
			}
			if (dictionary2.TryGetValue("version", out var value2))
			{
				_ = (long)value2;
			}
			if (dictionary2.TryGetValue("hairstyle", out value2))
			{
				int num3 = (int)(long)value2;
				if (Main.Hairstyles.AvailableHairstyles.Contains(num3))
				{
					_player.hair = num3;
				}
			}
			if (dictionary2.TryGetValue("clothingstyle", out value2))
			{
				int num4 = (int)(long)value2;
				if (_validClothStyles.Contains(num4))
				{
					_player.skinVariant = num4;
				}
			}
			if (dictionary2.TryGetValue("haircolor", out value2) && GetHexColor((string)value2, out var hsl))
			{
				_player.hairColor = ScaledHslToRgb(hsl);
			}
			if (dictionary2.TryGetValue("eyecolor", out value2) && GetHexColor((string)value2, out hsl))
			{
				_player.eyeColor = ScaledHslToRgb(hsl);
			}
			if (dictionary2.TryGetValue("skincolor", out value2) && GetHexColor((string)value2, out hsl))
			{
				_player.skinColor = ScaledHslToRgb(hsl);
			}
			if (dictionary2.TryGetValue("shirtcolor", out value2) && GetHexColor((string)value2, out hsl))
			{
				_player.shirtColor = ScaledHslToRgb(hsl);
			}
			if (dictionary2.TryGetValue("undershirtcolor", out value2) && GetHexColor((string)value2, out hsl))
			{
				_player.underShirtColor = ScaledHslToRgb(hsl);
			}
			if (dictionary2.TryGetValue("pantscolor", out value2) && GetHexColor((string)value2, out hsl))
			{
				_player.pantsColor = ScaledHslToRgb(hsl);
			}
			if (dictionary2.TryGetValue("shoecolor", out value2) && GetHexColor((string)value2, out hsl))
			{
				_player.shoeColor = ScaledHslToRgb(hsl);
			}
			Click_CharClothStyle(null, null);
			UpdateColorPickers();
		}
		catch
		{
		}
	}

	private void Click_RandomizePlayer(UIMouseEvent evt, UIElement listeningElement)
	{
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		SoundEngine.PlaySound(12);
		Player player = _player;
		int index = Main.rand.Next(Main.Hairstyles.AvailableHairstyles.Count);
		player.hair = Main.Hairstyles.AvailableHairstyles[index];
		player.eyeColor = ScaledHslToRgb(GetRandomColorVector());
		while (((Color)(ref player.eyeColor)).R + ((Color)(ref player.eyeColor)).G + ((Color)(ref player.eyeColor)).B > 300)
		{
			player.eyeColor = ScaledHslToRgb(GetRandomColorVector());
		}
		float num = (float)Main.rand.Next(60, 120) * 0.01f;
		if (num > 1f)
		{
			num = 1f;
		}
		((Color)(ref player.skinColor)).R = (byte)((float)Main.rand.Next(240, 255) * num);
		((Color)(ref player.skinColor)).G = (byte)((float)Main.rand.Next(110, 140) * num);
		((Color)(ref player.skinColor)).B = (byte)((float)Main.rand.Next(75, 110) * num);
		player.hairColor = ScaledHslToRgb(GetRandomColorVector());
		player.shirtColor = ScaledHslToRgb(GetRandomColorVector());
		player.underShirtColor = ScaledHslToRgb(GetRandomColorVector());
		player.pantsColor = ScaledHslToRgb(GetRandomColorVector());
		player.shoeColor = ScaledHslToRgb(GetRandomColorVector());
		player.skinVariant = _validClothStyles[Main.rand.Next(_validClothStyles.Length)];
		switch (player.hair + 1)
		{
		case 5:
		case 6:
		case 7:
		case 10:
		case 12:
		case 19:
		case 22:
		case 23:
		case 26:
		case 27:
		case 30:
		case 33:
		case 34:
		case 35:
		case 37:
		case 38:
		case 39:
		case 40:
		case 41:
		case 44:
		case 45:
		case 46:
		case 47:
		case 48:
		case 49:
		case 51:
		case 56:
		case 65:
		case 66:
		case 67:
		case 68:
		case 69:
		case 70:
		case 71:
		case 72:
		case 73:
		case 74:
		case 79:
		case 80:
		case 81:
		case 82:
		case 84:
		case 85:
		case 86:
		case 87:
		case 88:
		case 90:
		case 91:
		case 92:
		case 93:
		case 95:
		case 96:
		case 98:
		case 100:
		case 102:
		case 104:
		case 107:
		case 108:
		case 113:
		case 124:
		case 126:
		case 133:
		case 134:
		case 135:
		case 144:
		case 146:
		case 147:
		case 163:
		case 165:
			player.Male = false;
			break;
		default:
			player.Male = true;
			break;
		}
		if (player.hair >= HairID.Count)
		{
			Player player2 = player;
			player2.Male = HairLoader.GetHair(player.hair).RandomizedCharacterCreationGender switch
			{
				Gender.Male => true, 
				Gender.Female => false, 
				Gender.Unspecified => Main.rand.NextBool(), 
				_ => Main.rand.NextBool(), 
			};
		}
		Click_CharClothStyle(null, null);
		UpdateSelectedGender();
		UpdateColorPickers();
	}

	private void Click_Naming(UIMouseEvent evt, UIElement listeningElement)
	{
		SoundEngine.PlaySound(10);
		_player.name = "";
		Main.clrInput();
		UIVirtualKeyboard uIVirtualKeyboard = new UIVirtualKeyboard(Lang.menu[45].Value, "", OnFinishedNaming, OnCanceledNaming, 0, allowEmpty: true);
		uIVirtualKeyboard.SetMaxInputLength(20);
		Main.MenuUI.SetState(uIVirtualKeyboard);
	}

	private void Click_NamingAndCreating(UIMouseEvent evt, UIElement listeningElement)
	{
		SoundEngine.PlaySound(10);
		if (string.IsNullOrEmpty(_player.name))
		{
			_player.name = "";
			Main.clrInput();
			UIVirtualKeyboard uIVirtualKeyboard = new UIVirtualKeyboard(Lang.menu[45].Value, "", OnFinishedNamingAndCreating, OnCanceledNaming);
			uIVirtualKeyboard.SetMaxInputLength(20);
			Main.MenuUI.SetState(uIVirtualKeyboard);
		}
		else
		{
			FinishCreatingCharacter();
		}
	}

	private void OnFinishedNaming(string name)
	{
		_player.name = name.Trim();
		Main.MenuUI.SetState(this);
		_charName.SetContents(_player.name);
	}

	private void OnCanceledNaming()
	{
		Main.MenuUI.SetState(this);
	}

	private void OnFinishedNamingAndCreating(string name)
	{
		_player.name = name.Trim();
		Main.MenuUI.SetState(this);
		_charName.SetContents(_player.name);
		FinishCreatingCharacter();
	}

	private void FinishCreatingCharacter()
	{
		SetupPlayerStatsAndInventoryBasedOnDifficulty();
		PlayerFileData.CreateAndSave(_player);
		Main.LoadPlayers();
		Main.menuMode = 1;
	}

	private void SetupPlayerStatsAndInventoryBasedOnDifficulty()
	{
		int num = 0;
		if (_player.difficulty == 3)
		{
			PlayerLoader.ModifyMaxStats(_player);
			_player.statLife = _player.statLifeMax;
			_player.statMana = _player.statManaMax;
			_player.inventory[num].SetDefaults(6);
			_player.inventory[num++].Prefix(-1);
			_player.inventory[num].SetDefaults(1);
			_player.inventory[num++].Prefix(-1);
			_player.inventory[num].SetDefaults(10);
			_player.inventory[num++].Prefix(-1);
			_player.inventory[num].SetDefaults(7);
			_player.inventory[num++].Prefix(-1);
			_player.inventory[num].SetDefaults(4281);
			_player.inventory[num++].Prefix(-1);
			_player.inventory[num].SetDefaults(8);
			_player.inventory[num++].stack = 100;
			_player.inventory[num].SetDefaults(965);
			_player.inventory[num++].stack = 100;
			_player.inventory[num++].SetDefaults(50);
			_player.inventory[num++].SetDefaults(84);
			_player.armor[3].SetDefaults(4978);
			_player.armor[3].Prefix(-1);
			if (_player.name == "Wolf Pet" || _player.name == "Wolfpet")
			{
				_player.miscEquips[3].SetDefaults(5130);
			}
			_player.AddBuff(216, 3600);
		}
		else
		{
			_player.inventory[num].SetDefaults(3507);
			_player.inventory[num++].Prefix(-1);
			_player.inventory[num].SetDefaults(3509);
			_player.inventory[num++].Prefix(-1);
			_player.inventory[num].SetDefaults(3506);
			_player.inventory[num++].Prefix(-1);
		}
		if (Main.runningCollectorsEdition)
		{
			_player.inventory[num++].SetDefaults(603);
		}
		_player.savedPerPlayerFieldsThatArentInThePlayerClass = new Player.SavedPlayerDataWithAnnoyingRules();
		CreativePowerManager.Instance.ResetDataForNewPlayer(_player);
		IEnumerable<Item> vanillaItems = from item in _player.inventory
			where !item.IsAir
			select item into x
			select x.Clone();
		List<Item> startingItems = PlayerLoader.GetStartingItems(_player, vanillaItems);
		PlayerLoader.SetStartInventory(_player, startingItems);
	}

	private bool GetHexColor(string hexString, out Vector3 hsl)
	{
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		if (hexString.StartsWith("#"))
		{
			hexString = hexString.Substring(1);
		}
		if (hexString.Length <= 6 && uint.TryParse(hexString, NumberStyles.HexNumber, CultureInfo.CurrentCulture, out var result))
		{
			uint b = result & 0xFFu;
			uint g = (result >> 8) & 0xFFu;
			uint r = (result >> 16) & 0xFFu;
			hsl = RgbToScaledHsl(new Color((int)r, (int)g, (int)b));
			return true;
		}
		hsl = Vector3.Zero;
		return false;
	}

	private void Click_RandomizeSingleColor(UIMouseEvent evt, UIElement listeningElement)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		SoundEngine.PlaySound(12);
		Vector3 randomColorVector = GetRandomColorVector();
		ApplyPendingColor(ScaledHslToRgb(randomColorVector.X, randomColorVector.Y, randomColorVector.Z));
		_currentColorHSL = randomColorVector;
		UpdateHexText(ScaledHslToRgb(randomColorVector.X, randomColorVector.Y, randomColorVector.Z));
		UpdateColorPickers();
	}

	private static Vector3 GetRandomColorVector()
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		return new Vector3(Main.rand.NextFloat(), Main.rand.NextFloat(), Main.rand.NextFloat());
	}

	private void UnselectAllCategories()
	{
		UIColoredImageButton[] colorPickers = _colorPickers;
		for (int i = 0; i < colorPickers.Length; i++)
		{
			colorPickers[i]?.SetSelected(selected: false);
		}
		_clothingStylesCategoryButton.SetSelected(selected: false);
		_hairStylesCategoryButton.SetSelected(selected: false);
		_charInfoCategoryButton.SetSelected(selected: false);
		_hslContainer.Remove();
		_hairstylesContainer.Remove();
		_clothStylesContainer.Remove();
		_infoContainer.Remove();
	}

	private void SelectColorPicker(CategoryId selection)
	{
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		_selectedPicker = selection;
		switch (selection)
		{
		case CategoryId.CharInfo:
			Click_CharInfo(null, null);
			return;
		case CategoryId.Clothing:
			Click_ClothStyles(null, null);
			return;
		case CategoryId.HairStyle:
			Click_HairStyles(null, null);
			return;
		}
		UnselectAllCategories();
		_middleContainer.Append(_hslContainer);
		for (int i = 0; i < _colorPickers.Length; i++)
		{
			if (_colorPickers[i] != null)
			{
				_colorPickers[i].SetSelected(i == (int)selection);
			}
		}
		Vector3 currentColorHSL = Vector3.One;
		switch (_selectedPicker)
		{
		case CategoryId.HairColor:
			currentColorHSL = RgbToScaledHsl(_player.hairColor);
			break;
		case CategoryId.Eye:
			currentColorHSL = RgbToScaledHsl(_player.eyeColor);
			break;
		case CategoryId.Skin:
			currentColorHSL = RgbToScaledHsl(_player.skinColor);
			break;
		case CategoryId.Shirt:
			currentColorHSL = RgbToScaledHsl(_player.shirtColor);
			break;
		case CategoryId.Undershirt:
			currentColorHSL = RgbToScaledHsl(_player.underShirtColor);
			break;
		case CategoryId.Pants:
			currentColorHSL = RgbToScaledHsl(_player.pantsColor);
			break;
		case CategoryId.Shoes:
			currentColorHSL = RgbToScaledHsl(_player.shoeColor);
			break;
		}
		_currentColorHSL = currentColorHSL;
		UpdateHexText(ScaledHslToRgb(currentColorHSL.X, currentColorHSL.Y, currentColorHSL.Z));
	}

	private void UpdateColorPickers()
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		_ = _selectedPicker;
		_colorPickers[3].SetColor(_player.hairColor);
		_hairStylesCategoryButton.SetColor(_player.hairColor);
		_colorPickers[4].SetColor(_player.eyeColor);
		_colorPickers[5].SetColor(_player.skinColor);
		_colorPickers[6].SetColor(_player.shirtColor);
		_colorPickers[7].SetColor(_player.underShirtColor);
		_colorPickers[8].SetColor(_player.pantsColor);
		_colorPickers[9].SetColor(_player.shoeColor);
	}

	public override void Draw(SpriteBatch spriteBatch)
	{
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		base.Draw(spriteBatch);
		string text = null;
		if (_copyHexButton.IsMouseHovering)
		{
			text = Language.GetTextValue("UI.CopyColorToClipboard");
		}
		if (_pasteHexButton.IsMouseHovering)
		{
			text = Language.GetTextValue("UI.PasteColorFromClipboard");
		}
		if (_randomColorButton.IsMouseHovering)
		{
			text = Language.GetTextValue("UI.RandomizeColor");
		}
		if (_copyTemplateButton.IsMouseHovering)
		{
			text = Language.GetTextValue("UI.CopyPlayerToClipboard");
		}
		if (_pasteTemplateButton.IsMouseHovering)
		{
			text = Language.GetTextValue("UI.PastePlayerFromClipboard");
		}
		if (_randomizePlayerButton.IsMouseHovering)
		{
			text = Language.GetTextValue("UI.RandomizePlayer");
		}
		if (text != null)
		{
			float x = FontAssets.MouseText.Value.MeasureString(text).X;
			Vector2 vector = new Vector2((float)Main.mouseX, (float)Main.mouseY) + new Vector2(16f);
			if (vector.Y > (float)(Main.screenHeight - 30))
			{
				vector.Y = Main.screenHeight - 30;
			}
			if (vector.X > (float)Main.screenWidth - x)
			{
				vector.X = Main.screenWidth - 460;
			}
			Utils.DrawBorderStringFourWay(spriteBatch, FontAssets.MouseText.Value, text, vector.X, vector.Y, new Color((int)Main.mouseTextColor, (int)Main.mouseTextColor, (int)Main.mouseTextColor, (int)Main.mouseTextColor), Color.Black, Vector2.Zero);
		}
		SetupGamepadPoints(spriteBatch);
	}

	private void SetupGamepadPoints(SpriteBatch spriteBatch)
	{
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		UILinkPointNavigator.Shortcuts.BackButtonCommand = 1;
		int num = 3000;
		int num2 = num + 20;
		int num3 = num;
		List<SnapPoint> snapPoints = GetSnapPoints();
		SnapPoint snapPoint = snapPoints.First((SnapPoint a) => a.Name == "Back");
		SnapPoint snapPoint2 = snapPoints.First((SnapPoint a) => a.Name == "Create");
		UILinkPoint uILinkPoint = UILinkPointNavigator.Points[num3];
		uILinkPoint.Unlink();
		UILinkPointNavigator.SetPosition(num3, snapPoint.Position);
		num3++;
		UILinkPoint uILinkPoint2 = UILinkPointNavigator.Points[num3];
		uILinkPoint2.Unlink();
		UILinkPointNavigator.SetPosition(num3, snapPoint2.Position);
		num3++;
		uILinkPoint.Right = uILinkPoint2.ID;
		uILinkPoint2.Left = uILinkPoint.ID;
		_foundPoints.Clear();
		_foundPoints.Add(uILinkPoint.ID);
		_foundPoints.Add(uILinkPoint2.ID);
		List<SnapPoint> list = snapPoints.Where((SnapPoint a) => a.Name == "Top").ToList();
		list.Sort(SortPoints);
		for (int i = 0; i < list.Count; i++)
		{
			UILinkPoint uILinkPoint3 = UILinkPointNavigator.Points[num3];
			uILinkPoint3.Unlink();
			UILinkPointNavigator.SetPosition(num3, list[i].Position);
			uILinkPoint3.Left = num3 - 1;
			uILinkPoint3.Right = num3 + 1;
			uILinkPoint3.Down = num2;
			if (i == 0)
			{
				uILinkPoint3.Left = -3;
			}
			if (i == list.Count - 1)
			{
				uILinkPoint3.Right = -4;
			}
			if (_selectedPicker == CategoryId.HairStyle || _selectedPicker == CategoryId.Clothing)
			{
				uILinkPoint3.Down = num2 + i;
			}
			_foundPoints.Add(num3);
			num3++;
		}
		List<SnapPoint> list2 = snapPoints.Where((SnapPoint a) => a.Name == "Middle").ToList();
		list2.Sort(SortPoints);
		num3 = num2;
		switch (_selectedPicker)
		{
		case CategoryId.CharInfo:
		{
			for (int l = 0; l < list2.Count; l++)
			{
				UILinkPoint andSet3 = GetAndSet(num3, list2[l]);
				andSet3.Up = andSet3.ID - 1;
				andSet3.Down = andSet3.ID + 1;
				if (l == 0)
				{
					andSet3.Up = num + 2;
				}
				if (l == list2.Count - 1)
				{
					andSet3.Down = uILinkPoint.ID;
					uILinkPoint.Up = andSet3.ID;
					uILinkPoint2.Up = andSet3.ID;
				}
				_foundPoints.Add(num3);
				num3++;
			}
			break;
		}
		case CategoryId.HairStyle:
		{
			if (list2.Count == 0)
			{
				break;
			}
			_helper.CullPointsOutOfElementArea(spriteBatch, list2, _hairstylesContainer);
			SnapPoint snapPoint3 = list2[list2.Count - 1];
			_ = snapPoint3.Id / 10;
			_ = snapPoint3.Id % 10;
			int count = Main.Hairstyles.AvailableHairstyles.Count;
			for (int m = 0; m < list2.Count; m++)
			{
				SnapPoint snapPoint4 = list2[m];
				UILinkPoint andSet4 = GetAndSet(num3, snapPoint4);
				andSet4.Left = andSet4.ID - 1;
				if (snapPoint4.Id == 0)
				{
					andSet4.Left = -3;
				}
				andSet4.Right = andSet4.ID + 1;
				if (snapPoint4.Id == count - 1)
				{
					andSet4.Right = -4;
				}
				andSet4.Up = andSet4.ID - 10;
				if (m < 10)
				{
					andSet4.Up = num + 2 + m;
				}
				andSet4.Down = andSet4.ID + 10;
				if (snapPoint4.Id + 10 > snapPoint3.Id)
				{
					if (snapPoint4.Id % 10 < 5)
					{
						andSet4.Down = uILinkPoint.ID;
					}
					else
					{
						andSet4.Down = uILinkPoint2.ID;
					}
				}
				if (m == list2.Count - 1)
				{
					uILinkPoint.Up = andSet4.ID;
					uILinkPoint2.Up = andSet4.ID;
				}
				_foundPoints.Add(num3);
				num3++;
			}
			break;
		}
		default:
		{
			List<SnapPoint> list4 = snapPoints.Where((SnapPoint a) => a.Name == "Low").ToList();
			list4.Sort(SortPoints);
			num3 = num2 + 20;
			for (int n = 0; n < list4.Count; n++)
			{
				UILinkPoint andSet5 = GetAndSet(num3, list4[n]);
				andSet5.Up = num2 + 2;
				andSet5.Down = uILinkPoint.ID;
				andSet5.Left = andSet5.ID - 1;
				andSet5.Right = andSet5.ID + 1;
				if (n == 0)
				{
					andSet5.Left = andSet5.ID + 2;
					uILinkPoint.Up = andSet5.ID;
				}
				if (n == list4.Count - 1)
				{
					andSet5.Right = andSet5.ID - 2;
					uILinkPoint2.Up = andSet5.ID;
				}
				_foundPoints.Add(num3);
				num3++;
			}
			num3 = num2;
			for (int num4 = 0; num4 < list2.Count; num4++)
			{
				UILinkPoint andSet6 = GetAndSet(num3, list2[num4]);
				andSet6.Up = andSet6.ID - 1;
				andSet6.Down = andSet6.ID + 1;
				if (num4 == 0)
				{
					andSet6.Up = num + 2 + 5;
				}
				if (num4 == list2.Count - 1)
				{
					andSet6.Down = num2 + 20 + 2;
				}
				_foundPoints.Add(num3);
				num3++;
			}
			break;
		}
		case CategoryId.Clothing:
		{
			List<SnapPoint> list3 = snapPoints.Where((SnapPoint a) => a.Name == "Low").ToList();
			list3.Sort(SortPoints);
			int down = -2;
			int down2 = -2;
			num3 = num2 + 20;
			for (int j = 0; j < list3.Count; j++)
			{
				UILinkPoint andSet = GetAndSet(num3, list3[j]);
				andSet.Up = num2 + j + 2;
				andSet.Down = uILinkPoint.ID;
				if (j >= 3)
				{
					andSet.Up++;
					andSet.Down = uILinkPoint2.ID;
				}
				andSet.Left = andSet.ID - 1;
				andSet.Right = andSet.ID + 1;
				if (j == 0)
				{
					down = andSet.ID;
					andSet.Left = andSet.ID + 4;
					uILinkPoint.Up = andSet.ID;
				}
				if (j == list3.Count - 1)
				{
					down2 = andSet.ID;
					andSet.Right = andSet.ID - 4;
					uILinkPoint2.Up = andSet.ID;
				}
				_foundPoints.Add(num3);
				num3++;
			}
			num3 = num2;
			for (int k = 0; k < list2.Count; k++)
			{
				UILinkPoint andSet2 = GetAndSet(num3, list2[k]);
				andSet2.Up = num + 2 + k;
				andSet2.Left = andSet2.ID - 1;
				andSet2.Right = andSet2.ID + 1;
				if (k == 0)
				{
					andSet2.Left = andSet2.ID + 9;
				}
				if (k == list2.Count - 1)
				{
					andSet2.Right = andSet2.ID - 9;
				}
				andSet2.Down = down;
				if (k >= 5)
				{
					andSet2.Down = down2;
				}
				_foundPoints.Add(num3);
				num3++;
			}
			break;
		}
		}
		if (PlayerInput.UsingGamepadUI && !_foundPoints.Contains(UILinkPointNavigator.CurrentPoint))
		{
			MoveToVisuallyClosestPoint();
		}
	}

	private void MoveToVisuallyClosestPoint()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		Dictionary<int, UILinkPoint> points = UILinkPointNavigator.Points;
		Vector2 mouseScreen = Main.MouseScreen;
		UILinkPoint uILinkPoint = null;
		foreach (int foundPoint in _foundPoints)
		{
			if (!points.TryGetValue(foundPoint, out var value))
			{
				return;
			}
			if (uILinkPoint == null || Vector2.Distance(mouseScreen, uILinkPoint.Position) > Vector2.Distance(mouseScreen, value.Position))
			{
				uILinkPoint = value;
			}
		}
		if (uILinkPoint != null)
		{
			UILinkPointNavigator.ChangePoint(uILinkPoint.ID);
		}
	}

	public void TryMovingCategory(int direction)
	{
		int num = (int)(_selectedPicker + direction) % 10;
		if (num < 0)
		{
			num += 10;
		}
		SelectColorPicker((CategoryId)num);
	}

	private UILinkPoint GetAndSet(int ptid, SnapPoint snap)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		UILinkPoint uILinkPoint = UILinkPointNavigator.Points[ptid];
		uILinkPoint.Unlink();
		UILinkPointNavigator.SetPosition(uILinkPoint.ID, snap.Position);
		return uILinkPoint;
	}

	private bool PointWithName(SnapPoint a, string comp)
	{
		return a.Name == comp;
	}

	private int SortPoints(SnapPoint a, SnapPoint b)
	{
		return a.Id.CompareTo(b.Id);
	}

	private static Color ScaledHslToRgb(Vector3 hsl)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		return ScaledHslToRgb(hsl.X, hsl.Y, hsl.Z);
	}

	private static Color ScaledHslToRgb(float hue, float saturation, float luminosity)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		return Main.hslToRgb(hue, saturation, luminosity * 0.85f + 0.15f);
	}

	private static Vector3 RgbToScaledHsl(Color color)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		Vector3 value = Main.rgbToHsl(color);
		value.Z = (value.Z - 0.15f) / 0.85f;
		return Vector3.Clamp(value, Vector3.Zero, Vector3.One);
	}
}
