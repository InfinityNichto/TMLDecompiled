using Microsoft.Xna.Framework;
using Terraria.GameContent.UI.Elements;
using Terraria.Localization;

namespace Terraria.ModLoader.UI;

internal class UIForcedDelayInfoMessage : UIInfoMessage
{
	private int timeLeft = -1;

	private UITextPanel<string> waitPanel;

	internal void Delay(int seconds)
	{
		Main.menuMode = 10014;
		if (timeLeft == -1)
		{
			timeLeft = seconds * 60;
		}
	}

	public override void OnInitialize()
	{
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		base.OnInitialize();
		waitPanel = new UITextPanel<string>(Language.GetTextValue("tModLoader.WaitXSeconds", timeLeft / 60), 0.7f, large: true)
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
			Left = 
			{
				Percent = 0f
			},
			VAlign = 1f,
			Top = 
			{
				Pixels = -30f
			},
			BackgroundColor = Color.Orange
		};
	}

	public override void OnActivate()
	{
		base.OnActivate();
		_area.AddOrRemoveChild(_button, add: false);
		_area.Append(waitPanel);
	}

	public override void Update(GameTime gameTime)
	{
		base.Update(gameTime);
		if (timeLeft > 0)
		{
			timeLeft--;
			waitPanel.SetText(Language.GetTextValue("tModLoader.WaitXSeconds", timeLeft / 60 + 1));
			if (timeLeft == 0)
			{
				_area.AddOrRemoveChild(waitPanel, add: false);
				_area.AddOrRemoveChild(_button, add: true);
			}
		}
	}
}
