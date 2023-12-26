using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.Core;
using Terraria.Social;
using Terraria.Social.Base;
using Terraria.UI;

namespace Terraria.GameContent.UI.States;

public class WorkshopPublishInfoStateForMods : AWorkshopPublishInfoState<TmodFile>
{
	public const string TmlRules = "https://forums.terraria.org/index.php?threads/player-created-game-enhancements-rules-guidelines.286/";

	private readonly NameValueCollection _buildData;

	internal string changeNotes;

	public WorkshopPublishInfoStateForMods(UIState stateToGoBackTo, TmodFile modFile, NameValueCollection buildData)
		: base(stateToGoBackTo, modFile)
	{
		_instructionsTextKey = "Workshop.ModPublishDescription";
		_publishedObjectNameDescriptorTexKey = "Workshop.ModName";
		_buildData = buildData;
		_previewImagePath = buildData["iconpath"];
		changeNotes = buildData["changelog"];
	}

	protected override string GetPublishedObjectDisplayName()
	{
		return _dataObject.Name;
	}

	protected override void GoToPublishConfirmation()
	{
		SocialAPI.Workshop.PublishMod(_dataObject, _buildData, GetPublishSettings());
		if (Main.MenuUI.CurrentState?.GetType() != typeof(UIReportsPage))
		{
			Main.menuMode = 888;
			Main.MenuUI.SetState(_previousUIState);
		}
	}

	protected override List<WorkshopTagOption> GetTagsToShow()
	{
		return SocialAPI.Workshop.SupportedTags.ModTags;
	}

	protected override bool TryFindingTags(out FoundWorkshopEntryInfo info)
	{
		return SocialAPI.Workshop.TryGetInfoForMod(_dataObject, out info);
	}

	internal UIElement CreateTmlDisclaimer(string tagGroup)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		float num = 60f;
		float num2 = 0f + num;
		GroupOptionButton<bool> groupOptionButton = new GroupOptionButton<bool>(option: true, null, null, Color.White, null, 1f, 0.5f, 16f);
		groupOptionButton.HAlign = 0.5f;
		groupOptionButton.VAlign = 0f;
		groupOptionButton.Width = StyleDimension.FromPixelsAndPercent(0f, 1f);
		groupOptionButton.Left = StyleDimension.FromPixels(0f);
		groupOptionButton.Height = StyleDimension.FromPixelsAndPercent(num2 + 4f, 0f);
		groupOptionButton.Top = StyleDimension.FromPixels(0f);
		groupOptionButton.ShowHighlightWhenSelected = false;
		groupOptionButton.SetCurrentOption(option: false);
		groupOptionButton.Width.Set(0f, 1f);
		UIElement uIElement = new UIElement
		{
			HAlign = 0.5f,
			VAlign = 1f,
			Width = new StyleDimension(0f, 1f),
			Height = new StyleDimension(num, 0f)
		};
		groupOptionButton.Append(uIElement);
		UIText uIText = new UIText(Language.GetText("tModLoader.WorkshopDisclaimer"))
		{
			HAlign = 0f,
			VAlign = 0f,
			Width = StyleDimension.FromPixelsAndPercent(-40f, 1f),
			Height = StyleDimension.FromPixelsAndPercent(0f, 1f),
			TextColor = Color.Cyan,
			IgnoresMouseInteraction = true
		};
		uIText.PaddingLeft = 20f;
		uIText.PaddingRight = 20f;
		uIText.PaddingTop = 4f;
		uIText.IsWrapped = true;
		_tMLDisclaimerText = uIText;
		groupOptionButton.OnLeftClick += TmlDisclaimerText_OnClick;
		groupOptionButton.OnMouseOver += TmlDisclaimerText_OnMouseOver;
		groupOptionButton.OnMouseOut += TmlDisclaimerText_OnMouseOut;
		uIElement.Append(uIText);
		uIText.SetSnapPoint(tagGroup, 0);
		_tMLDisclaimerButton = uIText;
		return groupOptionButton;
	}

	private void TmlDisclaimerText_OnMouseOut(UIMouseEvent evt, UIElement listeningElement)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		_tMLDisclaimerText.TextColor = Color.Cyan;
		ClearOptionDescription(evt, listeningElement);
	}

	private void TmlDisclaimerText_OnMouseOver(UIMouseEvent evt, UIElement listeningElement)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		SoundEngine.PlaySound(12);
		_tMLDisclaimerText.TextColor = Color.LightCyan;
		ShowOptionDescription(evt, listeningElement);
	}

	private void TmlDisclaimerText_OnClick(UIMouseEvent evt, UIElement listeningElement)
	{
		Utils.OpenToURL("https://forums.terraria.org/index.php?threads/player-created-game-enhancements-rules-guidelines.286/");
	}

	public override void OnInitialize()
	{
		//IL_036c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0356: Unknown result type (might be due to invalid IL or missing references)
		base.OnInitialize();
		if (!Terraria.ModLoader.ModLoader.TryGetMod(_dataObject.Name, out var mod))
		{
			return;
		}
		Dictionary<GameCulture, int> localizationCounts = LocalizationLoader.GetLocalizationCounts(mod);
		int countMaxEntries = localizationCounts.DefaultIfEmpty().Max((KeyValuePair<GameCulture, int> x) => x.Value);
		Logging.tML.Info((object)("Determining localization progress for " + mod.Name + ":"));
		foreach (GroupOptionButton<WorkshopTagOption> tagOption in _tagOptions)
		{
			if (tagOption.OptionValue.NameKey.StartsWith("tModLoader.TagsLanguage_"))
			{
				GameCulture culture = tagOption.OptionValue.NameKey.Split('_')[1] switch
				{
					"English" => GameCulture.FromName("en-US"), 
					"Spanish" => GameCulture.FromName("es-ES"), 
					"French" => GameCulture.FromName("fr-FR"), 
					"Italian" => GameCulture.FromName("it-IT"), 
					"Russian" => GameCulture.FromName("ru-RU"), 
					"Chinese" => GameCulture.FromName("zh-Hans"), 
					"Portuguese" => GameCulture.FromName("pt-BR"), 
					"German" => GameCulture.FromName("de-DE"), 
					"Polish" => GameCulture.FromName("pl-PL"), 
					_ => throw new NotImplementedException(), 
				};
				localizationCounts.TryGetValue(culture, out var countOtherEntries);
				float localizationProgress = (float)countOtherEntries / (float)countMaxEntries;
				Logging.tML.Info((object)$"{culture.Name}, {countOtherEntries}/{countMaxEntries}, {localizationProgress:P0}, missing {countMaxEntries - countOtherEntries}");
				bool languageMostlyLocalized = localizationProgress > 0.75f;
				tagOption.SetCurrentOption(languageMostlyLocalized ? tagOption.OptionValue : null);
				tagOption.SetColor((Color)(tagOption.IsSelected ? new Color(192, 175, 235) : Colors.InventoryDefaultColor), 1f);
			}
		}
	}
}
