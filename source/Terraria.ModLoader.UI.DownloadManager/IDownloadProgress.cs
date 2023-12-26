namespace Terraria.ModLoader.UI.DownloadManager;

public interface IDownloadProgress
{
	void DownloadStarted(string displayName);

	void UpdateDownloadProgress(float progress, long bytesReceived, long totalBytesNeeded);
}
