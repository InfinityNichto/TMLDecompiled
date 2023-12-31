using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader.Core;
using Terraria.UI;
using Terraria.UI.Gamepad;

namespace Terraria.ModLoader.UI;

public class UICreateMod : UIState, IHaveBackButtonCommand
{
	private UIElement _baseElement;

	private UITextPanel<string> _messagePanel;

	private UIFocusInputTextField _modName;

	private UIFocusInputTextField _modDiplayName;

	private UIFocusInputTextField _modAuthor;

	private UIFocusInputTextField _basicSword;

	private string lastKnownMessage = "";

	private readonly byte[] ExampleSwordPNG = new byte[356]
	{
		137, 80, 78, 71, 13, 10, 26, 10, 0, 0,
		0, 13, 73, 72, 68, 82, 0, 0, 0, 40,
		0, 0, 0, 40, 8, 6, 0, 0, 0, 140,
		254, 184, 109, 0, 0, 0, 1, 115, 82, 71,
		66, 0, 174, 206, 28, 233, 0, 0, 0, 6,
		98, 75, 71, 68, 0, 255, 0, 255, 0, 255,
		160, 189, 167, 147, 0, 0, 0, 9, 112, 72,
		89, 115, 0, 0, 11, 19, 0, 0, 11, 19,
		1, 0, 154, 156, 24, 0, 0, 0, 7, 116,
		73, 77, 69, 7, 223, 7, 8, 2, 58, 4,
		121, 104, 62, 171, 0, 0, 0, 29, 105, 84,
		88, 116, 67, 111, 109, 109, 101, 110, 116, 0,
		0, 0, 0, 0, 67, 114, 101, 97, 116, 101,
		100, 32, 119, 105, 116, 104, 32, 71, 73, 77,
		80, 100, 46, 101, 7, 0, 0, 0, 187, 73,
		68, 65, 84, 88, 195, 237, 214, 201, 13, 196,
		32, 12, 5, 208, 239, 40, 85, 184, 38, 68,
		29, 212, 135, 168, 137, 54, 50, 151, 68, 98,
		34, 38, 139, 8, 12, 14, 246, 209, 230, 244,
		228, 5, 66, 39, 193, 204, 75, 46, 63, 161,
		243, 160, 94, 228, 188, 247, 0, 128, 24, 35,
		0, 192, 57, 39, 67, 112, 238, 77, 142, 153,
		181, 7, 139, 228, 172, 181, 91, 158, 84, 176,
		68, 78, 123, 176, 84, 78, 5, 75, 229, 198,
		21, 124, 74, 110, 60, 193, 167, 229, 198, 17,
		172, 37, 247, 126, 193, 218, 114, 242, 4, 247,
		63, 217, 77, 228, 95, 114, 34, 5, 151, 92,
		45, 17, 106, 42, 39, 87, 208, 24, 3, 0,
		8, 33, 124, 61, 108, 45, 39, 70, 112, 78,
		166, 150, 86, 185, 172, 100, 107, 57, 249, 151,
		228, 172, 39, 107, 203, 201, 21, 252, 37, 151,
		145, 164, 163, 139, 163, 123, 48, 153, 94, 186,
		82, 31, 87, 240, 174, 204, 254, 102, 171, 96,
		107, 145, 215, 9, 126, 0, 100, 140, 196, 171,
		13, 196, 202, 8, 0, 0, 0, 0, 73, 69,
		78, 68, 174, 66, 96, 130
	};

	public UIState PreviousUIState { get; set; }

	public override void OnInitialize()
	{
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0200: Unknown result type (might be due to invalid IL or missing references)
		//IL_0204: Unknown result type (might be due to invalid IL or missing references)
		//IL_020a: Unknown result type (might be due to invalid IL or missing references)
		//IL_025b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0261: Unknown result type (might be due to invalid IL or missing references)
		//IL_0265: Unknown result type (might be due to invalid IL or missing references)
		//IL_026b: Unknown result type (might be due to invalid IL or missing references)
		//IL_026f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0275: Unknown result type (might be due to invalid IL or missing references)
		//IL_0279: Unknown result type (might be due to invalid IL or missing references)
		//IL_027f: Unknown result type (might be due to invalid IL or missing references)
		_baseElement = new UIElement
		{
			Width = 
			{
				Percent = 0.8f
			},
			MaxWidth = UICommon.MaxPanelWidth,
			Top = 
			{
				Pixels = 220f
			},
			Height = 
			{
				Pixels = -220f,
				Percent = 1f
			},
			HAlign = 0.5f
		};
		Append(_baseElement);
		UIPanel mainPanel = new UIPanel
		{
			Width = 
			{
				Percent = 1f
			},
			Height = 
			{
				Pixels = -110f,
				Percent = 1f
			},
			BackgroundColor = UICommon.MainPanelBackground,
			PaddingTop = 0f
		};
		_baseElement.Append(mainPanel);
		UITextPanel<string> uITextPanel = new UITextPanel<string>(Language.GetTextValue("tModLoader.MSCreateMod"), 0.8f, large: true)
		{
			HAlign = 0.5f,
			Top = 
			{
				Pixels = -35f
			},
			BackgroundColor = UICommon.DefaultUIBlue
		}.WithPadding(15f);
		_baseElement.Append(uITextPanel);
		_messagePanel = new UITextPanel<string>(Language.GetTextValue(""))
		{
			Width = 
			{
				Percent = 1f
			},
			Height = 
			{
				Pixels = 25f
			},
			VAlign = 1f,
			Top = 
			{
				Pixels = -20f
			}
		};
		UITextPanel<string> buttonBack = new UITextPanel<string>(Language.GetTextValue("UI.Back"))
		{
			Width = 
			{
				Pixels = -10f,
				Percent = 0.5f
			},
			Height = 
			{
				Pixels = 25f
			},
			VAlign = 1f,
			Top = 
			{
				Pixels = -65f
			}
		}.WithFadedMouseOver();
		buttonBack.OnLeftClick += BackClick;
		_baseElement.Append(buttonBack);
		UITextPanel<string> buttonCreate = new UITextPanel<string>(Language.GetTextValue("LegacyMenu.28"));
		buttonCreate.CopyStyle(buttonBack);
		buttonCreate.HAlign = 1f;
		buttonCreate.WithFadedMouseOver();
		buttonCreate.OnLeftClick += OKClick;
		_baseElement.Append(buttonCreate);
		float top = 16f;
		_modName = createAndAppendTextInputWithLabel("ModName (no spaces)", "Type here");
		_modName.OnTextChange += delegate
		{
			_modName.SetText(_modName.CurrentString.Replace(" ", ""));
		};
		_modDiplayName = createAndAppendTextInputWithLabel("Mod DisplayName", "Type here");
		_modAuthor = createAndAppendTextInputWithLabel("Mod Author", "Type here");
		_basicSword = createAndAppendTextInputWithLabel("BasicSword (no spaces)", "Leave Blank to Skip");
		_modName.OnTab += delegate
		{
			_modDiplayName.Focused = true;
		};
		_modDiplayName.OnTab += delegate
		{
			_modAuthor.Focused = true;
		};
		_modAuthor.OnTab += delegate
		{
			_basicSword.Focused = true;
		};
		_basicSword.OnTab += delegate
		{
			_modName.Focused = true;
		};
		UIFocusInputTextField createAndAppendTextInputWithLabel(string label, string hint)
		{
			UIPanel panel = new UIPanel();
			panel.SetPadding(0f);
			panel.Width.Set(0f, 1f);
			panel.Height.Set(40f, 0f);
			panel.Top.Set(top, 0f);
			top += 46f;
			UIText modNameText = new UIText(label)
			{
				Left = 
				{
					Pixels = 10f
				},
				Top = 
				{
					Pixels = 10f
				}
			};
			panel.Append(modNameText);
			UIPanel textBoxBackground = new UIPanel();
			textBoxBackground.SetPadding(0f);
			textBoxBackground.Top.Set(6f, 0f);
			textBoxBackground.Left.Set(0f, 0.5f);
			textBoxBackground.Width.Set(0f, 0.5f);
			textBoxBackground.Height.Set(30f, 0f);
			panel.Append(textBoxBackground);
			UIFocusInputTextField uIInputTextField = new UIFocusInputTextField(hint)
			{
				UnfocusOnTab = true
			};
			uIInputTextField.Top.Set(5f, 0f);
			uIInputTextField.Left.Set(10f, 0f);
			uIInputTextField.Width.Set(-20f, 1f);
			uIInputTextField.Height.Set(20f, 0f);
			textBoxBackground.Append(uIInputTextField);
			mainPanel.Append(panel);
			return uIInputTextField;
		}
	}

	public override void OnActivate()
	{
		base.OnActivate();
		_modName.SetText("");
		_basicSword.SetText("");
		_modDiplayName.SetText("");
		_modAuthor.SetText("");
		_messagePanel.SetText("");
		_modName.Focused = true;
	}

	public override void Update(GameTime gameTime)
	{
		base.Update(gameTime);
		if (lastKnownMessage != _messagePanel.Text)
		{
			lastKnownMessage = _messagePanel.Text;
			if (string.IsNullOrEmpty(_messagePanel.Text))
			{
				_baseElement.RemoveChild(_messagePanel);
			}
			else
			{
				_baseElement.Append(_messagePanel);
			}
		}
	}

	private void BackClick(UIMouseEvent evt, UIElement listeningElement)
	{
		HandleBackButtonUsage();
	}

	public void HandleBackButtonUsage()
	{
		SoundEngine.PlaySound(in SoundID.MenuClose);
		Main.menuMode = 10001;
	}

	protected override void DrawSelf(SpriteBatch spriteBatch)
	{
		base.DrawSelf(spriteBatch);
		UILinkPointNavigator.Shortcuts.BackButtonCommand = 7;
	}

	private void OKClick(UIMouseEvent evt, UIElement listeningElement)
	{
		try
		{
			string modNameTrimmed = _modName.CurrentString.Trim();
			string basicSwordTrimmed = _basicSword.CurrentString.Trim();
			string sourceFolder = Path.Combine(ModCompile.ModSourcePath, modNameTrimmed);
			CodeDomProvider provider = CodeDomProvider.CreateProvider("C#");
			if (Directory.Exists(sourceFolder))
			{
				_messagePanel.SetText("Folder already exists");
				return;
			}
			if (!provider.IsValidIdentifier(modNameTrimmed))
			{
				_messagePanel.SetText("ModName is invalid C# identifier. Remove spaces.");
				return;
			}
			if (modNameTrimmed.Equals("Mod", StringComparison.InvariantCultureIgnoreCase) || modNameTrimmed.Equals("ModLoader", StringComparison.InvariantCultureIgnoreCase) || modNameTrimmed.Equals("tModLoader", StringComparison.InvariantCultureIgnoreCase))
			{
				_messagePanel.SetText("ModName is a reserved mod name. Choose a different name.");
				return;
			}
			if (!string.IsNullOrEmpty(basicSwordTrimmed) && !provider.IsValidIdentifier(basicSwordTrimmed))
			{
				_messagePanel.SetText("BasicSword is invalid C# identifier. Remove spaces.");
				return;
			}
			if (string.IsNullOrWhiteSpace(_modDiplayName.CurrentString))
			{
				_messagePanel.SetText("DisplayName can't be empty");
				return;
			}
			if (string.IsNullOrWhiteSpace(_modAuthor.CurrentString))
			{
				_messagePanel.SetText("Author can't be empty");
				return;
			}
			Directory.CreateDirectory(sourceFolder);
			File.WriteAllText(Path.Combine(sourceFolder, "build.txt"), GetModBuild());
			File.WriteAllText(Path.Combine(sourceFolder, "description.txt"), GetModDescription());
			using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Terraria.ModLoader.Default.iconTemplate.png"))
			{
				using FileStream fs = File.OpenWrite(Path.Combine(sourceFolder, "icon.png"));
				stream.CopyTo(fs);
			}
			File.WriteAllText(Path.Combine(sourceFolder, modNameTrimmed + ".cs"), GetModClass(modNameTrimmed));
			File.WriteAllText(Path.Combine(sourceFolder, modNameTrimmed + ".csproj"), GetModCsproj(modNameTrimmed));
			string text = Path.Combine(sourceFolder, "Properties");
			Directory.CreateDirectory(text);
			File.WriteAllText(Path.Combine(text, "launchSettings.json"), GetLaunchSettings());
			if (!string.IsNullOrEmpty(basicSwordTrimmed))
			{
				string text2 = Path.Combine(sourceFolder, "Items");
				Directory.CreateDirectory(text2);
				File.WriteAllText(Path.Combine(text2, basicSwordTrimmed + ".cs"), GetBasicSword(modNameTrimmed, basicSwordTrimmed));
				File.WriteAllBytes(Path.Combine(text2, basicSwordTrimmed + ".png"), ExampleSwordPNG);
			}
			string text3 = Path.Combine(sourceFolder, "Localization");
			Directory.CreateDirectory(text3);
			File.WriteAllText(Path.Combine(text3, "en-US_Mods." + modNameTrimmed + ".hjson"), GetLocalizationFile(modNameTrimmed, basicSwordTrimmed));
			Utils.OpenFolder(sourceFolder);
			SoundEngine.PlaySound(in SoundID.MenuOpen);
			Main.menuMode = 10001;
		}
		catch (Exception e)
		{
			Logging.tML.Error((object)e);
			_messagePanel.SetText("There was an issue. Check client.log");
		}
	}

	private string GetModBuild()
	{
		return "displayName = " + _modDiplayName.CurrentString + Environment.NewLine + "author = " + _modAuthor.CurrentString + Environment.NewLine + "version = 0.1";
	}

	private string GetModDescription()
	{
		return _modDiplayName.CurrentString + " is a pretty cool mod, it does...this. Modify this file with a description of your mod.";
	}

	private string GetModClass(string modNameTrimmed)
	{
		return $"using Terraria.ModLoader;\r\n\r\nnamespace {modNameTrimmed}\r\n{{\r\n\tpublic class {modNameTrimmed} : Mod\r\n\t{{\r\n\t}}\r\n}}";
	}

	internal string GetModCsproj(string modNameTrimmed)
	{
		return "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<Project Sdk=\"Microsoft.NET.Sdk\">\r\n  <Import Project=\"..\\tModLoader.targets\" />\r\n  <PropertyGroup>\r\n    <AssemblyName>" + modNameTrimmed + "</AssemblyName>\r\n    <TargetFramework>net6.0</TargetFramework>\r\n    <PlatformTarget>AnyCPU</PlatformTarget>\r\n    <LangVersion>latest</LangVersion>\r\n  </PropertyGroup>\r\n  <ItemGroup>\r\n    <PackageReference Include=\"tModLoader.CodeAssist\" Version=\"0.1.*\" />\r\n  </ItemGroup>\r\n</Project>";
	}

	internal bool CsprojUpdateNeeded(string fileContents)
	{
		if (!fileContents.Contains("..\\tModLoader.targets"))
		{
			return true;
		}
		if (!fileContents.Contains("<TargetFramework>net6.0</TargetFramework>"))
		{
			return true;
		}
		return false;
	}

	internal string GetLaunchSettings()
	{
		return "{\r\n  \"profiles\": {\r\n    \"Terraria\": {\r\n      \"commandName\": \"Executable\",\r\n      \"executablePath\": \"dotnet\",\r\n      \"commandLineArgs\": \"$(tMLPath)\",\r\n      \"workingDirectory\": \"$(tMLSteamPath)\"\r\n    },\r\n    \"TerrariaServer\": {\r\n      \"commandName\": \"Executable\",\r\n      \"executablePath\": \"dotnet\",\r\n      \"commandLineArgs\": \"$(tMLServerPath)\",\r\n      \"workingDirectory\": \"$(tMLSteamPath)\"\r\n    }\r\n  }\r\n}";
	}

	internal string GetLocalizationFile(string modNameTrimmed, string basicSwordTrimmed)
	{
		if (string.IsNullOrEmpty(basicSwordTrimmed))
		{
			return "# This file will automatically update with entries for new content after a build and reload";
		}
		return $"# This file will automatically update with entries for new content after a build and reload\r\n\r\nItems: {{\r\n\t{basicSwordTrimmed}: {{\r\n\t\tDisplayName: {Regex.Replace(basicSwordTrimmed, "([A-Z])", " $1").Trim()}\r\n\t\tTooltip: \"\"\r\n\t}}\r\n}} \r\n";
	}

	internal string GetBasicSword(string modNameTrimmed, string basicSwordName)
	{
		return $"using Terraria;\r\nusing Terraria.ID;\r\nusing Terraria.ModLoader;\r\n\r\nnamespace {modNameTrimmed}.Items\r\n{{\r\n\tpublic class {basicSwordName} : ModItem\r\n\t{{\r\n        // The Display Name and Tooltip of this item can be edited in the Localization/en-US_Mods.{modNameTrimmed}.hjson file.\r\n\r\n\t\tpublic override void SetDefaults()\r\n\t\t{{\r\n\t\t\tItem.damage = 50;\r\n\t\t\tItem.DamageType = DamageClass.Melee;\r\n\t\t\tItem.width = 40;\r\n\t\t\tItem.height = 40;\r\n\t\t\tItem.useTime = 20;\r\n\t\t\tItem.useAnimation = 20;\r\n\t\t\tItem.useStyle = 1;\r\n\t\t\tItem.knockBack = 6;\r\n\t\t\tItem.value = 10000;\r\n\t\t\tItem.rare = 2;\r\n\t\t\tItem.UseSound = SoundID.Item1;\r\n\t\t\tItem.autoReuse = true;\r\n\t\t}}\r\n\r\n\t\tpublic override void AddRecipes()\r\n\t\t{{\r\n\t\t\tRecipe recipe = CreateRecipe();\r\n\t\t\trecipe.AddIngredient(ItemID.DirtBlock, 10);\r\n\t\t\trecipe.AddTile(TileID.WorkBenches);\r\n\t\t\trecipe.Register();\r\n\t\t}}\r\n\t}}\r\n}}";
	}
}
