using System;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.Localization;
using Terraria.UI;

namespace Terraria.ModLoader.UI;

internal class UIErrorMessage : UIState
{
	private UIMessageBox messageBox;

	private UIElement area;

	private UITextPanel<string> continueButton;

	private UITextPanel<string> exitAndDisableAllButton;

	private UITextPanel<string> webHelpButton;

	private UITextPanel<string> skipLoadButton;

	private UITextPanel<string> retryButton;

	private string message;

	private int gotoMenu;

	private UIState gotoState;

	private string webHelpURL;

	private bool continueIsRetry;

	private bool showSkip;

	private Action retryAction;

	public override void OnInitialize()
	{
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_023b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0241: Unknown result type (might be due to invalid IL or missing references)
		//IL_0244: Unknown result type (might be due to invalid IL or missing references)
		//IL_024a: Unknown result type (might be due to invalid IL or missing references)
		//IL_024d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0253: Unknown result type (might be due to invalid IL or missing references)
		//IL_0256: Unknown result type (might be due to invalid IL or missing references)
		//IL_025c: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0368: Unknown result type (might be due to invalid IL or missing references)
		//IL_036e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0371: Unknown result type (might be due to invalid IL or missing references)
		//IL_0377: Unknown result type (might be due to invalid IL or missing references)
		//IL_037a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0380: Unknown result type (might be due to invalid IL or missing references)
		//IL_0383: Unknown result type (might be due to invalid IL or missing references)
		//IL_0389: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0402: Unknown result type (might be due to invalid IL or missing references)
		//IL_0405: Unknown result type (might be due to invalid IL or missing references)
		//IL_040b: Unknown result type (might be due to invalid IL or missing references)
		//IL_040e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0414: Unknown result type (might be due to invalid IL or missing references)
		//IL_0417: Unknown result type (might be due to invalid IL or missing references)
		//IL_041d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0484: Unknown result type (might be due to invalid IL or missing references)
		//IL_048a: Unknown result type (might be due to invalid IL or missing references)
		//IL_048d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0493: Unknown result type (might be due to invalid IL or missing references)
		//IL_0496: Unknown result type (might be due to invalid IL or missing references)
		//IL_049c: Unknown result type (might be due to invalid IL or missing references)
		//IL_049f: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a5: Unknown result type (might be due to invalid IL or missing references)
		area = new UIElement
		{
			Width = 
			{
				Percent = 0.8f
			},
			Top = 
			{
				Pixels = 200f
			},
			Height = 
			{
				Pixels = -210f,
				Percent = 1f
			},
			HAlign = 0.5f
		};
		UIPanel uIPanel = new UIPanel
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
			BackgroundColor = UICommon.MainPanelBackground
		};
		area.Append(uIPanel);
		messageBox = new UIMessageBox(string.Empty)
		{
			Width = 
			{
				Pixels = -25f,
				Percent = 1f
			},
			Height = 
			{
				Percent = 1f
			}
		};
		uIPanel.Append(messageBox);
		UIScrollbar uIScrollbar = new UIScrollbar
		{
			Height = 
			{
				Pixels = -12f,
				Percent = 1f
			},
			VAlign = 0.5f,
			HAlign = 1f
		}.WithView(100f, 1000f);
		uIPanel.Append(uIScrollbar);
		messageBox.SetScrollbar(uIScrollbar);
		continueButton = new UITextPanel<string>("", 0.7f, large: true)
		{
			Width = 
			{
				Pixels = -10f,
				Percent = 0.5f
			},
			Height = 
			{
				Pixels = 50f
			},
			Top = 
			{
				Pixels = -108f,
				Percent = 1f
			}
		};
		continueButton.WithFadedMouseOver();
		continueButton.OnLeftClick += ContinueClick;
		area.Append(continueButton);
		UITextPanel<string> openLogsButton = new UITextPanel<string>(Language.GetTextValue("tModLoader.OpenLogs"), 0.7f, large: true);
		openLogsButton.CopyStyle(continueButton);
		openLogsButton.HAlign = 1f;
		openLogsButton.WithFadedMouseOver();
		openLogsButton.OnLeftClick += OpenFile;
		area.Append(openLogsButton);
		webHelpButton = new UITextPanel<string>(Language.GetTextValue("tModLoader.OpenWebHelp"), 0.7f, large: true);
		webHelpButton.CopyStyle(openLogsButton);
		webHelpButton.Top.Set(-55f, 1f);
		webHelpButton.WithFadedMouseOver();
		webHelpButton.OnLeftClick += VisitRegisterWebpage;
		area.Append(webHelpButton);
		skipLoadButton = new UITextPanel<string>(Language.GetTextValue("tModLoader.SkipToMainMenu"), 0.7f, large: true);
		skipLoadButton.CopyStyle(continueButton);
		skipLoadButton.Top.Set(-55f, 1f);
		skipLoadButton.WithFadedMouseOver();
		skipLoadButton.OnLeftClick += SkipLoad;
		area.Append(skipLoadButton);
		exitAndDisableAllButton = new UITextPanel<string>(Language.GetTextValue("tModLoader.ExitAndDisableAll"), 0.7f, large: true);
		exitAndDisableAllButton.CopyStyle(skipLoadButton);
		exitAndDisableAllButton.TextColor = Color.Red;
		exitAndDisableAllButton.WithFadedMouseOver();
		exitAndDisableAllButton.OnLeftClick += ExitAndDisableAll;
		retryButton = new UITextPanel<string>("Retry", 0.7f, large: true);
		retryButton.CopyStyle(continueButton);
		retryButton.Top.Set(-50f, 1f);
		retryButton.WithFadedMouseOver();
		retryButton.OnLeftClick += delegate
		{
			retryAction();
		};
		Append(area);
	}

	public override void OnActivate()
	{
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		Netplay.Disconnect = true;
		messageBox.SetText(message);
		string continueKey = ((gotoMenu < 0) ? "Exit" : (continueIsRetry ? "Retry" : "Continue"));
		continueButton.SetText(Language.GetTextValue("tModLoader." + continueKey));
		continueButton.TextColor = ((gotoMenu >= 0) ? Color.White : Color.Red);
		area.AddOrRemoveChild(webHelpButton, !string.IsNullOrEmpty(webHelpURL));
		area.AddOrRemoveChild(skipLoadButton, showSkip);
		area.AddOrRemoveChild(exitAndDisableAllButton, gotoMenu < 0);
		area.AddOrRemoveChild(retryButton, retryAction != null);
	}

	public override void OnDeactivate()
	{
		retryAction = null;
	}

	internal void Show(string message, int gotoMenu, UIState gotoState = null, string webHelpURL = "", bool continueIsRetry = false, bool showSkip = false, Action retryAction = null)
	{
		if (!Program.IsMainThread)
		{
			Main.QueueMainThreadAction(delegate
			{
				Show(message, gotoMenu, gotoState, webHelpURL, continueIsRetry, showSkip, retryAction);
			});
			return;
		}
		this.message = message;
		this.gotoMenu = gotoMenu;
		this.gotoState = gotoState;
		this.webHelpURL = webHelpURL;
		this.continueIsRetry = continueIsRetry;
		this.showSkip = showSkip;
		this.retryAction = retryAction;
		Main.gameMenu = true;
		Main.menuMode = 10005;
	}

	private void ContinueClick(UIMouseEvent evt, UIElement listeningElement)
	{
		SoundEngine.PlaySound(in SoundID.MenuOpen);
		if (gotoMenu < 0)
		{
			((Game)Main.instance).Exit();
		}
		Main.menuMode = gotoMenu;
		if (gotoState != null)
		{
			Main.MenuUI.SetState(gotoState);
		}
	}

	private void ExitAndDisableAll(UIMouseEvent evt, UIElement listeningElement)
	{
		ModLoader.DisableAllMods();
		((Game)Main.instance).Exit();
	}

	private void OpenFile(UIMouseEvent evt, UIElement listeningElement)
	{
		SoundEngine.PlaySound(in SoundID.MenuOpen);
		Utils.OpenFolder(Logging.LogDir);
	}

	private void VisitRegisterWebpage(UIMouseEvent evt, UIElement listeningElement)
	{
		SoundEngine.PlaySound(in SoundID.MenuOpen);
		Utils.OpenToURL(webHelpURL);
	}

	private void SkipLoad(UIMouseEvent evt, UIElement listeningElement)
	{
		ContinueClick(evt, listeningElement);
		ModLoader.skipLoad = true;
	}
}
