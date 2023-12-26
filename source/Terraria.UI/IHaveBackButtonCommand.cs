using Terraria.Audio;
using Terraria.ID;

namespace Terraria.UI;

public interface IHaveBackButtonCommand
{
	UIState PreviousUIState { get; set; }

	void HandleBackButtonUsage()
	{
		GoBackTo(PreviousUIState);
	}

	static void GoBackTo(UIState state)
	{
		if (state == null)
		{
			Main.menuMode = 0;
		}
		else
		{
			Main.menuMode = 888;
			Main.MenuUI.SetState(state);
		}
		SoundEngine.PlaySound(in SoundID.MenuClose);
	}
}
