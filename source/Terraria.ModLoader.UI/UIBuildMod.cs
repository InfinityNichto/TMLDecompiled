using System;
using System.Threading;
using System.Threading.Tasks;
using log4net.Core;
using Terraria.Localization;
using Terraria.ModLoader.Core;
using Terraria.ModLoader.Exceptions;

namespace Terraria.ModLoader.UI;

internal class UIBuildMod : UIProgress, ModCompile.IBuildStatus
{
	private CancellationTokenSource _cts;

	private int numProgressItems;

	public void SetProgress(int i, int n = -1)
	{
		if (n >= 0)
		{
			numProgressItems = n;
		}
		base.Progress = (float)i / (float)numProgressItems;
	}

	public void SetStatus(string msg)
	{
		Logging.tML.Info((object)msg);
		DisplayText = msg;
	}

	public void LogCompilerLine(string msg, Level level)
	{
		((ILoggerWrapper)Logging.tML).Logger.Log((Type)null, level, (object)msg, (Exception)null);
	}

	internal void Build(string mod, bool reload)
	{
		Build(delegate(ModCompile mc)
		{
			mc.Build(mod);
		}, reload);
	}

	internal void BuildAll(bool reload)
	{
		Build(delegate(ModCompile mc)
		{
			mc.BuildAll();
		}, reload);
	}

	internal void Build(Action<ModCompile> buildAction, bool reload)
	{
		Main.menuMode = 10003;
		Task.Run(() => BuildMod(buildAction, reload));
	}

	public override void OnInitialize()
	{
		base.OnInitialize();
		_cancelButton.Remove();
	}

	public override void OnActivate()
	{
		base.OnActivate();
		_cts = new CancellationTokenSource();
		base.OnCancel += delegate
		{
			_cts.Cancel();
		};
	}

	public override void OnDeactivate()
	{
		base.OnDeactivate();
		_cts?.Dispose();
		_cts = null;
	}

	private Task BuildMod(Action<ModCompile> buildAction, bool reload)
	{
		while (_progressBar == null || _cts == null)
		{
			Task.Delay(1);
		}
		try
		{
			buildAction(new ModCompile(this));
			Main.menuMode = (reload ? 10006 : 10001);
		}
		catch (OperationCanceledException)
		{
			Logging.tML.Info((object)"Mod building was cancelled.");
			return Task.FromResult(result: false);
		}
		catch (Exception e)
		{
			Logging.tML.Error((object)e.Message, e);
			object mod = (e.Data.Contains("mod") ? e.Data["mod"] : null);
			string msg = Language.GetTextValue("tModLoader.BuildError", mod ?? "");
			Action retry = null;
			if (e is BuildException)
			{
				msg = msg + "\n" + e.Message + "\n\n" + e.InnerException;
				retry = delegate
				{
					Interface.buildMod.Build(buildAction, reload);
				};
			}
			else
			{
				msg += $"\n\n{e}";
			}
			Interface.errorMessage.Show(msg, 10001, null, e.HelpLink, continueIsRetry: false, showSkip: false, retry);
			return Task.FromResult(result: false);
		}
		return Task.FromResult(result: true);
	}
}
