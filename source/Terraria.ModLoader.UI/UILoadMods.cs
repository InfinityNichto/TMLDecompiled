using System;
using System.Threading;
using Terraria.Localization;

namespace Terraria.ModLoader.UI;

internal class UILoadMods : UIProgress
{
	public int modCount;

	private string stageText;

	private CancellationTokenSource _cts;

	public override void OnActivate()
	{
		base.OnActivate();
		_cts = new CancellationTokenSource();
		base.OnCancel += delegate
		{
			SetLoadStage("tModLoader.LoadingCancelled");
			_cts.Cancel();
		};
		gotoMenu = 888;
		ModLoader.BeginLoad(_cts.Token);
	}

	public override void OnDeactivate()
	{
		base.OnDeactivate();
		_cts?.Dispose();
		_cts = null;
	}

	public void SetLoadStage(string stageText, int modCount = -1)
	{
		this.stageText = stageText;
		this.modCount = modCount;
		if (modCount < 0)
		{
			SetProgressText(Language.GetTextValue(stageText));
		}
		base.Progress = 0f;
		base.SubProgressText = "";
	}

	private void SetProgressText(string text, string logText = null)
	{
		Logging.tML.Info((object)(logText ?? text));
		if (Main.dedServ)
		{
			Console.WriteLine(text);
		}
		else
		{
			DisplayText = text;
		}
	}

	public void SetCurrentMod(int i, string modName, string displayName, Version version)
	{
		string display = $"{displayName} v{version}";
		string log = $"{modName} ({displayName}) v{version}";
		SetProgressText(Language.GetTextValue(stageText, display), Language.GetTextValue(stageText, log));
		base.Progress = (float)i / (float)modCount;
	}

	public void SetCurrentMod(int i, Mod mod)
	{
		SetCurrentMod(i, mod.Name, mod.DisplayName, mod.Version);
	}
}
