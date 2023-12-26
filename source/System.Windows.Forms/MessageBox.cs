using SDL2;

namespace System.Windows.Forms;

public static class MessageBox
{
	private static SDL_MessageBoxButtonData OKButton = new SDL_MessageBoxButtonData
	{
		flags = (SDL_MessageBoxButtonFlags)1,
		buttonid = 1,
		text = "OK"
	};

	private static SDL_MessageBoxButtonData CancelButton = new SDL_MessageBoxButtonData
	{
		flags = (SDL_MessageBoxButtonFlags)2,
		buttonid = 2,
		text = "Cancel"
	};

	private static SDL_MessageBoxButtonData YesButton = new SDL_MessageBoxButtonData
	{
		flags = (SDL_MessageBoxButtonFlags)1,
		buttonid = 6,
		text = "Yes"
	};

	private static SDL_MessageBoxButtonData NoButton = new SDL_MessageBoxButtonData
	{
		buttonid = 7,
		text = "No"
	};

	private static SDL_MessageBoxButtonData RetryButton = new SDL_MessageBoxButtonData
	{
		flags = (SDL_MessageBoxButtonFlags)1,
		buttonid = 4,
		text = "Retry"
	};

	public static DialogResult Show(string msg, string title, MessageBoxButtons buttons = MessageBoxButtons.OK, MessageBoxIcon icon = MessageBoxIcon.None)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		SDL_MessageBoxData val = default(SDL_MessageBoxData);
		val.flags = (SDL_MessageBoxFlags)icon;
		val.message = msg;
		val.title = title;
		val.buttons = buttons switch
		{
			MessageBoxButtons.OK => (SDL_MessageBoxButtonData[])(object)new SDL_MessageBoxButtonData[1] { OKButton }, 
			MessageBoxButtons.OKCancel => (SDL_MessageBoxButtonData[])(object)new SDL_MessageBoxButtonData[2] { CancelButton, OKButton }, 
			MessageBoxButtons.YesNo => (SDL_MessageBoxButtonData[])(object)new SDL_MessageBoxButtonData[2] { NoButton, YesButton }, 
			MessageBoxButtons.YesNoCancel => (SDL_MessageBoxButtonData[])(object)new SDL_MessageBoxButtonData[3] { CancelButton, NoButton, YesButton }, 
			MessageBoxButtons.RetryCancel => (SDL_MessageBoxButtonData[])(object)new SDL_MessageBoxButtonData[2] { CancelButton, RetryButton }, 
			_ => throw new NotImplementedException(), 
		};
		SDL_MessageBoxData msgBox = val;
		msgBox.numbuttons = msgBox.buttons.Length;
		int buttonid = default(int);
		SDL.SDL_ShowMessageBox(ref msgBox, ref buttonid);
		return (DialogResult)buttonid;
	}
}
