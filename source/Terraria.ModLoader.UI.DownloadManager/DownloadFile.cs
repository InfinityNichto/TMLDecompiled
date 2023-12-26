using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;

namespace Terraria.ModLoader.UI.DownloadManager;

internal class DownloadFile
{
	public delegate void ProgressUpdated(float progress, long bytesReceived, long totalBytesNeeded);

	internal const string TEMP_EXTENSION = ".tmp";

	public const int CHUNK_SIZE = 1048576;

	public const SecurityProtocolType Tls12 = SecurityProtocolType.Tls12;

	public readonly string Url;

	public readonly string FilePath;

	public readonly string DisplayText;

	private FileStream _fileStream;

	public SecurityProtocolType SecurityProtocol = SecurityProtocolType.Tls12;

	public Version ProtocolVersion = HttpVersion.Version11;

	private bool _aborted;

	public HttpWebRequest Request { get; private set; }

	public event ProgressUpdated OnUpdateProgress;

	public event Action OnComplete;

	public DownloadFile(string url, string filePath, string displayText)
	{
		Url = url;
		FilePath = filePath;
		DisplayText = displayText;
	}

	public bool Verify()
	{
		if (string.IsNullOrWhiteSpace(Url))
		{
			return false;
		}
		if (string.IsNullOrWhiteSpace(FilePath))
		{
			return false;
		}
		return true;
	}

	public Task<DownloadFile> Download(CancellationToken token, ProgressUpdated updateProgressAction = null)
	{
		SetupDownloadRequest();
		if (updateProgressAction != null)
		{
			this.OnUpdateProgress = updateProgressAction;
		}
		return Task.Factory.FromAsync(Request.BeginGetResponse, (IAsyncResult asyncResult) => Request.EndGetResponse(asyncResult), null).ContinueWith((Task<WebResponse> t) => HandleResponse(t.Result, token), token);
	}

	private void AbortDownload(string filePath)
	{
		_aborted = true;
		Request?.Abort();
		_fileStream?.Flush();
		_fileStream?.Close();
		if (File.Exists(filePath))
		{
			File.Delete(filePath);
		}
	}

	private DownloadFile HandleResponse(WebResponse response, CancellationToken token)
	{
		long contentLength = response.ContentLength;
		if (contentLength < 0)
		{
			string txt = "Could not get a proper content length for DownloadFile[" + DisplayText + "]";
			Logging.tML.Error((object)txt);
			throw new Exception(txt);
		}
		string _downloadPath = $"{new FileInfo(FilePath).Directory.FullName}{Path.DirectorySeparatorChar}{DateTime.Now.Ticks}{".tmp"}";
		_fileStream = new FileStream(_downloadPath, FileMode.Create);
		Stream responseStream = response.GetResponseStream();
		int currentIndex = 0;
		byte[] buf = new byte[1048576];
		try
		{
			int r;
			while ((r = responseStream.Read(buf, 0, buf.Length)) > 0)
			{
				token.ThrowIfCancellationRequested();
				_fileStream.Write(buf, 0, r);
				currentIndex += r;
				this.OnUpdateProgress?.Invoke((float)((double)currentIndex / (double)contentLength), currentIndex, response.ContentLength);
			}
		}
		catch (OperationCanceledException e2)
		{
			AbortDownload(_downloadPath);
			Logging.tML.Info((object)("DownloadFile[" + DisplayText + "] operation was cancelled"), (Exception)e2);
		}
		catch (Exception e)
		{
			AbortDownload(_downloadPath);
			Logging.tML.Info((object)"Unknown error", e);
		}
		if (!_aborted)
		{
			_fileStream?.Close();
			PreCopy();
			File.Copy(_downloadPath, FilePath, overwrite: true);
			File.Delete(_downloadPath);
			this.OnComplete?.Invoke();
		}
		return this;
	}

	private void SetupDownloadRequest()
	{
		ServicePointManager.SecurityProtocol = SecurityProtocol;
		ServicePointManager.ServerCertificateValidationCallback = ServerCertificateValidation;
		Request = WebRequest.CreateHttp(Url);
		Request.ServicePoint.ReceiveBufferSize = 1048576;
		Request.Method = "GET";
		Request.ProtocolVersion = ProtocolVersion;
		Request.UserAgent = "tModLoader/" + BuildInfo.versionTag;
		Request.KeepAlive = true;
	}

	private bool ServerCertificateValidation(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
	{
		return errors == SslPolicyErrors.None;
	}

	internal virtual void PreCopy()
	{
	}
}
