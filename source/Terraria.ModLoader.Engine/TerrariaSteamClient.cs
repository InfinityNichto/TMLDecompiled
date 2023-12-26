using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Reflection;
using System.Threading;
using log4net;
using Steamworks;
using Terraria.Social.Steam;

namespace Terraria.ModLoader.Engine;

internal class TerrariaSteamClient
{
	public enum LaunchResult
	{
		ErrClientProcDied,
		ErrSteamInitFailed,
		ErrNotInstalled,
		ErrInstallOutOfDate,
		Ok
	}

	private const int LatestTerrariaBuildID = 9653812;

	private static AnonymousPipeServerStream serverPipe;

	private static string MsgInitFailed = "init_failed";

	private static string MsgInitSuccess = "init_success";

	private static string MsgFamilyShared = "family_shared";

	private static string MsgNotInstalled = "not_installed";

	private static string MsgInstallOutOfDate = "install_out_of_date";

	private static string MsgGrant = "grant:";

	private static string MsgAck = "acknowledged";

	private static string MsgShutdown = "shutdown";

	private static string MsgInvalidateTerrariaInstall = "corrupt_install";

	private static ILog Logger { get; } = LogManager.GetLogger("TerrariaSteamClient");


	internal static LaunchResult Launch()
	{
		if (Environment.GetEnvironmentVariable("SteamClientLaunch") != "1")
		{
			Logger.Debug((object)"Disabled. Launched outside steam client.");
			return LaunchResult.Ok;
		}
		serverPipe = new AnonymousPipeServerStream(PipeDirection.Out, HandleInheritability.Inheritable);
		string tMLName = Path.GetFileName(Assembly.GetExecutingAssembly().Location);
		Process proc = new Process
		{
			StartInfo = 
			{
				FileName = Environment.ProcessPath,
				Arguments = tMLName + " -terrariasteamclient " + serverPipe.GetClientHandleAsString(),
				UseShellExecute = false,
				RedirectStandardOutput = true
			}
		};
		string[] array = ((IEnumerable<string>)proc.StartInfo.EnvironmentVariables.Keys).ToArray();
		foreach (string i in array)
		{
			if (i.StartsWith("steam", StringComparison.InvariantCultureIgnoreCase))
			{
				proc.StartInfo.EnvironmentVariables.Remove(i);
			}
		}
		proc.Start();
		while (true)
		{
			string line = proc.StandardOutput.ReadLine()?.Trim();
			if (line == null)
			{
				if (proc.HasExited)
				{
					return LaunchResult.ErrClientProcDied;
				}
				continue;
			}
			Logger.Debug((object)("Recv: " + line));
			if (line == MsgInitFailed)
			{
				return LaunchResult.ErrSteamInitFailed;
			}
			if (line == MsgNotInstalled)
			{
				return LaunchResult.ErrNotInstalled;
			}
			if (line == MsgInstallOutOfDate)
			{
				return LaunchResult.ErrInstallOutOfDate;
			}
			if (line == MsgInitSuccess)
			{
				break;
			}
			if (line == MsgFamilyShared)
			{
				SteamedWraps.FamilyShared = true;
			}
		}
		SendCmd(MsgAck);
		Thread.Sleep(300);
		return LaunchResult.Ok;
	}

	private static void SendCmd(string cmd)
	{
		if (serverPipe == null)
		{
			return;
		}
		Logger.Debug((object)("Send: " + cmd));
		using StreamWriter sw = new StreamWriter(serverPipe, null, -1, leaveOpen: true);
		sw.WriteLine(cmd);
	}

	internal static void Run()
	{
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		Logging.Init(Logging.LogFile.TerrariaSteamClient);
		Logger.InfoFormat("Working Directory: {0}", (object)Path.GetFullPath(Directory.GetCurrentDirectory()));
		Logger.InfoFormat("Args: {0}", (object)string.Join(' ', Environment.GetCommandLineArgs()));
		ILog logger = Logger;
		AppId_t terrariaAppId_t = Steam.TerrariaAppId_t;
		logger.Info((object)("Setting steam app id to " + ((object)(AppId_t)(ref terrariaAppId_t)).ToString()));
		Steam.SetAppId(Steam.TerrariaAppId_t);
		bool steamInit = false;
		try
		{
			using AnonymousPipeClientStream clientPipe = new AnonymousPipeClientStream(PipeDirection.In, Program.LaunchParameters["-terrariasteamclient"]);
			StreamReader sr = new StreamReader(clientPipe, null, detectEncodingFromByteOrderMarks: true, -1, leaveOpen: true);
			try
			{
				Func<string> Recv = delegate
				{
					string text = sr.ReadLine()?.Trim();
					if (text == null)
					{
						throw new EndOfStreamException();
					}
					Logger.Debug((object)("Recv: " + text));
					return text;
				};
				Action<string> Send = delegate(string s)
				{
					Logger.Debug((object)("Send: " + s));
					Console.WriteLine(s);
				};
				Logger.Info((object)"SteamAPI.Init()");
				steamInit = SteamAPI.Init();
				if (!steamInit)
				{
					Logger.Fatal((object)"SteamAPI.Init() failed");
					Send(MsgInitFailed);
					return;
				}
				if (!SteamApps.BIsAppInstalled(Steam.TerrariaAppId_t))
				{
					Logger.Fatal((object)$"SteamApps.BIsAppInstalled({Steam.TerrariaAppId_t}): false");
					Send(MsgNotInstalled);
					SteamShutdown();
					return;
				}
				int TerrariaBuildID = SteamApps.GetAppBuildId();
				Logger.Info((object)("Terraria BuildID: " + TerrariaBuildID));
				if (TerrariaBuildID < 9653812)
				{
					Logger.Fatal((object)"Terraria is out of date, you need to update Terraria in Steam.");
					Send(MsgInstallOutOfDate);
					SteamShutdown();
					return;
				}
				if (SteamApps.BIsSubscribedFromFamilySharing())
				{
					Logger.Info((object)"Terraria is installed via Family Share. Re-pathing tModLoader required");
					Send(MsgFamilyShared);
				}
				Send(MsgInitSuccess);
				while (Recv() != MsgAck)
				{
				}
				bool pbAchieved = default(bool);
				while (true)
				{
					Thread.Sleep(250);
					string nextCMD = Recv();
					if (nextCMD == MsgShutdown)
					{
						break;
					}
					if (nextCMD.StartsWith(MsgGrant))
					{
						string achievement = nextCMD.Substring(MsgGrant.Length);
						SteamUserStats.GetAchievement(achievement, ref pbAchieved);
						if (!pbAchieved)
						{
							SteamUserStats.SetAchievement(achievement);
						}
					}
					if (nextCMD == MsgInvalidateTerrariaInstall)
					{
						SteamApps.MarkContentCorrupt(false);
					}
				}
			}
			finally
			{
				if (sr != null)
				{
					((IDisposable)sr).Dispose();
				}
			}
		}
		catch (EndOfStreamException)
		{
			Logger.Info((object)"The connection to tML was closed unexpectedly. Look in client.log or server.log for details");
		}
		catch (Exception ex)
		{
			Logger.Fatal((object)"Unhandled error", ex);
		}
		if (steamInit)
		{
			SteamShutdown();
		}
	}

	private static void SteamShutdown()
	{
		try
		{
			Logger.Info((object)"SteamAPI.Shutdown()");
			SteamAPI.Shutdown();
		}
		catch (Exception ex)
		{
			Logger.Error((object)"Error shutting down SteamAPI", ex);
		}
	}

	private static void MarkSteamTerrariaInstallCorrupt()
	{
		try
		{
			Logger.Info((object)"Marking Steam Terraria Installation as Corrupt to force 'Verify Local Files' on next run");
			SendCmd(MsgInvalidateTerrariaInstall);
		}
		catch
		{
		}
	}

	internal static void Shutdown()
	{
		try
		{
			SendCmd(MsgShutdown);
		}
		catch
		{
		}
	}
}
