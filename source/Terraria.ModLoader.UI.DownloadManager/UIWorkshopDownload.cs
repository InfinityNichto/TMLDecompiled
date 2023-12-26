using System.Diagnostics;
using Microsoft.Xna.Framework;
using Terraria.Localization;

namespace Terraria.ModLoader.UI.DownloadManager;

internal class UIWorkshopDownload : UIProgress, IDownloadProgress
{
	internal struct ProgressData
	{
		public string displayName;

		public float progress;

		public long bytesReceived;

		public long totalBytesNeeded;

		public bool reset;
	}

	private ProgressData progressData;

	private bool needToUpdateProgressData;

	private Stopwatch downloadTimer;

	public UIWorkshopDownload()
	{
		downloadTimer = new Stopwatch();
	}

	public override void OnInitialize()
	{
		base.OnInitialize();
		_cancelButton.Remove();
	}

	public override void Update(GameTime gameTime)
	{
		if (needToUpdateProgressData)
		{
			ProgressData localProgressData;
			lock (this)
			{
				localProgressData = progressData;
				progressData.reset = false;
				needToUpdateProgressData = false;
			}
			if (localProgressData.reset)
			{
				_progressBar.DisplayText = Language.GetTextValue("tModLoader.MBDownloadingMod", localProgressData.displayName);
				downloadTimer.Restart();
			}
			_progressBar.UpdateProgress(localProgressData.progress);
			double elapsedSeconds = downloadTimer.Elapsed.TotalSeconds;
			double speed = ((elapsedSeconds > 0.0) ? ((double)localProgressData.bytesReceived / elapsedSeconds) : 0.0);
			base.SubProgressText = $"{UIMemoryBar.SizeSuffix(localProgressData.bytesReceived, 2)} / {UIMemoryBar.SizeSuffix(localProgressData.totalBytesNeeded, 2)} ({UIMemoryBar.SizeSuffix((long)speed, 2)}/s)";
		}
		base.Update(gameTime);
	}

	/// <remarks>This will be called from a thread!</remarks>
	public void DownloadStarted(string displayName)
	{
		lock (this)
		{
			progressData.displayName = displayName;
			progressData.progress = 0f;
			progressData.bytesReceived = 0L;
			progressData.totalBytesNeeded = 0L;
			progressData.reset = true;
			needToUpdateProgressData = true;
		}
	}

	/// <remarks>This will be called from a thread!</remarks>
	public void UpdateDownloadProgress(float progress, long bytesReceived, long totalBytesNeeded)
	{
		lock (this)
		{
			progressData.progress = progress;
			progressData.bytesReceived = bytesReceived;
			progressData.totalBytesNeeded = totalBytesNeeded;
			needToUpdateProgressData = true;
		}
	}
}
