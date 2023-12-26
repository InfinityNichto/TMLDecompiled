using System;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.Localization;
using Terraria.UI;

namespace Terraria.ModLoader.UI;

internal class UIProgress : UIState
{
	public int gotoMenu;

	protected UIProgressBar _progressBar;

	protected UITextPanel<LocalizedText> _cancelButton;

	public string DisplayText;

	protected UIText subProgress;

	public float Progress
	{
		get
		{
			return _progressBar?.Progress ?? 0f;
		}
		set
		{
			_progressBar?.UpdateProgress(value);
		}
	}

	public string SubProgressText
	{
		set
		{
			subProgress?.SetText(value);
		}
	}

	public event Action OnCancel;

	public override void OnInitialize()
	{
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		_progressBar = new UIProgressBar
		{
			Width = 
			{
				Percent = 0.8f
			},
			MaxWidth = UICommon.MaxPanelWidth,
			Height = 
			{
				Pixels = 150f
			},
			HAlign = 0.5f,
			VAlign = 0.5f,
			Top = 
			{
				Pixels = 10f
			}
		};
		Append(_progressBar);
		_cancelButton = new UITextPanel<LocalizedText>(Language.GetText("UI.Cancel"), 0.75f, large: true)
		{
			VAlign = 0.5f,
			HAlign = 0.5f,
			Top = 
			{
				Pixels = 170f
			}
		}.WithFadedMouseOver();
		_cancelButton.OnLeftClick += CancelClick;
		Append(_cancelButton);
		subProgress = new UIText("", 0.5f, large: true)
		{
			Top = 
			{
				Pixels = 65f
			},
			HAlign = 0.5f,
			VAlign = 0.5f
		};
		Append(subProgress);
	}

	private void CancelClick(UIMouseEvent evt, UIElement listeningElement)
	{
		SoundEngine.PlaySound(in SoundID.MenuOpen);
		Main.menuMode = gotoMenu;
		this.OnCancel?.Invoke();
	}

	public void Show(string displayText = "", int gotoMenu = 0, Action cancel = null)
	{
		if (Main.MenuUI.CurrentState == this)
		{
			Main.MenuUI.RefreshState();
		}
		else
		{
			Main.menuMode = 10023;
		}
		DisplayText = displayText;
		this.gotoMenu = gotoMenu;
		if (cancel != null)
		{
			OnCancel += cancel;
		}
	}

	public override void Update(GameTime gameTime)
	{
		base.Update(gameTime);
		_progressBar.DisplayText = DisplayText;
	}

	public override void OnDeactivate()
	{
		DisplayText = null;
		this.OnCancel = null;
		gotoMenu = 0;
		Progress = 0f;
	}
}
