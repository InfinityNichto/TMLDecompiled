using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Ionic.Zip;
using ReLogic.OS;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.ModLoader.UI.DownloadManager;
using Terraria.ModLoader.UI.ModBrowser;
using Terraria.Social;
using Terraria.UI;

namespace Terraria.ModLoader.UI;

internal class UIUpdateMessage : UIState
{
	private readonly UIMessageBox _message = new UIMessageBox("");

	private UIElement _area;

	private UITextPanel<string> _autoUpdateButton;

	private int _gotoMenu;

	private string _url;

	private string _autoUpdateUrl;

	public override void OnInitialize()
	{
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_017b: Unknown result type (might be due to invalid IL or missing references)
		//IL_017e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0204: Unknown result type (might be due to invalid IL or missing references)
		//IL_0207: Unknown result type (might be due to invalid IL or missing references)
		//IL_020d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0210: Unknown result type (might be due to invalid IL or missing references)
		//IL_0216: Unknown result type (might be due to invalid IL or missing references)
		//IL_0219: Unknown result type (might be due to invalid IL or missing references)
		//IL_021f: Unknown result type (might be due to invalid IL or missing references)
		_area = new UIElement
		{
			Width = 
			{
				Percent = 0.8f
			},
			Top = 
			{
				Pixels = 200f
			},
			Height = 
			{
				Pixels = -240f,
				Percent = 1f
			},
			HAlign = 0.5f
		};
		_message.Width.Percent = 1f;
		_message.Height.Percent = 0.8f;
		_message.HAlign = 0.5f;
		_area.Append(_message);
		UITextPanel<string> button = new UITextPanel<string>("Ignore", 0.7f, large: true)
		{
			Width = 
			{
				Pixels = -10f,
				Percent = 1f / 3f
			},
			Height = 
			{
				Pixels = 50f
			},
			VAlign = 1f,
			Top = 
			{
				Pixels = -30f
			}
		};
		button.WithFadedMouseOver();
		button.OnLeftClick += IgnoreClick;
		_area.Append(button);
		UITextPanel<string> button2 = new UITextPanel<string>("Download", 0.7f, large: true);
		button2.CopyStyle(button);
		button2.HAlign = 0.5f;
		button2.WithFadedMouseOver();
		button2.OnLeftClick += OpenURL;
		_area.Append(button2);
		if (Platform.IsWindows && SocialAPI.Mode != SocialMode.Steam)
		{
			_autoUpdateButton = new UITextPanel<string>("Auto Update", 0.7f, large: true);
			_autoUpdateButton.CopyStyle(button);
			_autoUpdateButton.HAlign = 1f;
			_autoUpdateButton.WithFadedMouseOver();
			_autoUpdateButton.OnLeftClick += AutoUpdate;
		}
		Append(_area);
	}

	public override void OnActivate()
	{
		base.OnActivate();
		if (Platform.IsWindows && SocialAPI.Mode != SocialMode.Steam && UIModBrowser.PlatformSupportsTls12)
		{
			_area.AddOrRemoveChild(_autoUpdateButton, !string.IsNullOrEmpty(_autoUpdateUrl));
		}
	}

	internal void SetMessage(string text)
	{
		_message.SetText(text);
	}

	internal void SetGotoMenu(int gotoMenu)
	{
		_gotoMenu = gotoMenu;
	}

	internal void SetURL(string url)
	{
		_url = url;
	}

	internal void SetAutoUpdateURL(string autoUpdateURL)
	{
		_autoUpdateUrl = autoUpdateURL;
	}

	private void IgnoreClick(UIMouseEvent evt, UIElement listeningElement)
	{
		SoundEngine.PlaySound(10);
		Main.menuMode = _gotoMenu;
	}

	private void OpenURL(UIMouseEvent evt, UIElement listeningElement)
	{
		SoundEngine.PlaySound(10);
		Process.Start(_url);
	}

	private void AutoUpdate(UIMouseEvent evt, UIElement listeningElement)
	{
		SoundEngine.PlaySound(in SoundID.MenuOpen);
		string installDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
		string zipFileName = Path.GetFileName(new Uri(_autoUpdateUrl).LocalPath);
		string zipFilePath = Path.Combine(installDirectory, zipFileName);
		Logging.tML.Info((object)("AutoUpdate: " + _autoUpdateUrl + " -> " + zipFilePath));
		DownloadFile downloadFile = new DownloadFile(_autoUpdateUrl, zipFilePath, "Auto update: " + zipFileName);
		downloadFile.OnComplete += delegate
		{
			OnAutoUpdateDownloadComplete(installDirectory, zipFilePath);
		};
		Interface.downloadProgress.gotoMenu = 10007;
		Interface.downloadProgress.HandleDownloads(downloadFile);
	}

	private static void OnAutoUpdateDownloadComplete(string installDirectory, string zipFilePath)
	{
		try
		{
			string updateScriptName = (Platform.IsWindows ? "update.bat" : "update.sh");
			string updateScript = Path.Combine(installDirectory, updateScriptName);
			Logging.tML.Info((object)("Writing Script: " + updateScriptName));
			using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Terraria.ModLoader.Core." + updateScriptName))
			{
				using FileStream fs = File.OpenWrite(updateScript);
				stream.CopyTo(fs);
			}
			if (Platform.IsWindows)
			{
				string extractDir = Path.Combine(installDirectory, "tModLoader_Update");
				if (Directory.Exists(extractDir))
				{
					Directory.Delete(extractDir, recursive: true);
				}
				Directory.CreateDirectory(extractDir);
				Logging.tML.Info((object)("Extracting: " + zipFilePath + " -> " + extractDir));
				ZipFile zip = ZipFile.Read(zipFilePath);
				try
				{
					zip.ExtractAll(extractDir);
				}
				finally
				{
					((IDisposable)zip)?.Dispose();
				}
				File.Delete(zipFilePath);
				string executableName = Path.GetFileName(Assembly.GetExecutingAssembly().Location);
				Logging.tML.Info((object)("Renaming Terraria.exe -> " + executableName));
				File.Move(Path.Combine(extractDir, "Terraria.exe"), Path.Combine(extractDir, executableName));
				Process.Start(updateScript, "\"" + executableName + "\" tModLoader_Update");
			}
			else
			{
				Process.Start("bash", "-c 'chmod a+x \"" + updateScript + "\"'").WaitForExit();
				Process.Start(updateScript, $"{Process.GetCurrentProcess().Id} \"{zipFilePath}\"");
			}
			Logging.tML.Info((object)"AutoUpdate script started. Exiting");
			Interface.downloadProgress.gotoMenu = 10026;
			Main.menuMode = 10026;
		}
		catch (Exception e)
		{
			Logging.tML.Error((object)"Problem during autoupdate", e);
		}
	}
}
