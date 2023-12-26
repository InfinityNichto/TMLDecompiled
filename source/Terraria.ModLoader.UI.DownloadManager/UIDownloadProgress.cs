using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Terraria.Localization;

namespace Terraria.ModLoader.UI.DownloadManager;

internal class UIDownloadProgress : UIProgress
{
	private DownloadFile _downloadFile;

	private readonly List<DownloadFile> _downloads = new List<DownloadFile>();

	internal CancellationTokenSource _cts;

	private Stopwatch downloadTimer;

	public event Action OnDownloadsComplete;

	public override void OnActivate()
	{
		base.OnActivate();
		downloadTimer = new Stopwatch();
		if (_downloads.Count <= 0)
		{
			Logging.tML.Warn((object)"UIDownloadProgress was activated but no downloads were present.");
			Main.menuMode = gotoMenu;
			return;
		}
		_cts = new CancellationTokenSource();
		base.OnCancel += delegate
		{
			_cts.Cancel();
		};
		downloadTimer.Restart();
		DownloadMods();
	}

	public override void OnDeactivate()
	{
		base.OnDeactivate();
		foreach (DownloadFile download in _downloads)
		{
			Logging.tML.Warn((object)("UIDownloadProgress was deactivated but download [" + download.FilePath + "] was still present."));
		}
		_downloadFile = null;
		this.OnDownloadsComplete = null;
		_cts?.Dispose();
		_downloads.Clear();
		_progressBar.UpdateProgress(0f);
	}

	public void HandleDownloads(params DownloadFile[] downloads)
	{
		foreach (DownloadFile download in downloads)
		{
			if (download.Verify())
			{
				_downloads.Add(download);
			}
		}
		Show();
	}

	public void Show()
	{
		Main.menuMode = 10020;
	}

	private void DownloadMods()
	{
		downloadTimer.Start();
		_downloadFile = _downloads.First();
		if (_downloadFile != null)
		{
			_progressBar.UpdateProgress(0f);
			_progressBar.DisplayText = Language.GetTextValue("tModLoader.MBDownloadingMod", _downloadFile.DisplayText);
			_downloadFile.Download(_cts.Token, UpdateDownloadProgress).ContinueWith(HandleNextDownload, _cts.Token);
		}
	}

	private void UpdateDownloadProgress(float progress, long bytesReceived, long totalBytesNeeded)
	{
		_progressBar.UpdateProgress(progress);
		double elapsedSeconds = downloadTimer.Elapsed.TotalSeconds;
		double speed = ((elapsedSeconds > 0.0) ? ((double)bytesReceived / elapsedSeconds) : 0.0);
		base.SubProgressText = $"{UIMemoryBar.SizeSuffix(bytesReceived, 2)} / {UIMemoryBar.SizeSuffix(totalBytesNeeded, 2)} ({UIMemoryBar.SizeSuffix((long)speed, 2)}/s)";
	}

	private void HandleNextDownload(Task<DownloadFile> task)
	{
		bool hasError = task.Exception != null;
		_downloads.Remove(_downloadFile);
		if (_downloads.Count <= 0 || hasError)
		{
			if (hasError)
			{
				Logging.tML.Error((object)("There was a problem downloading the mod " + _downloadFile.DisplayText), (Exception)task.Exception);
			}
			Main.menuMode = gotoMenu;
			this.OnDownloadsComplete?.Invoke();
		}
		else
		{
			downloadTimer.Restart();
			DownloadMods();
		}
	}
}
