using System;
using System.Diagnostics;
using System.Threading;
using log4net.Core;

namespace Terraria.ModLoader.Engine;

internal static class ServerHangWatchdog
{
	public static readonly TimeSpan TIMEOUT = TimeSpan.FromSeconds(10.0);

	private static volatile Ref<DateTime> lastCheckin;

	internal static void Checkin()
	{
		if (!Debugger.IsAttached)
		{
			bool num = lastCheckin != null;
			lastCheckin = new Ref<DateTime>(DateTime.Now);
			if (!num)
			{
				Start();
			}
		}
	}

	private static void Start()
	{
		Thread mainThread = Thread.CurrentThread;
		Thread thread = new Thread((ThreadStart)delegate
		{
			Run(mainThread);
		});
		thread.Name = "Server Hang Watchdog";
		thread.IsBackground = true;
		thread.Start();
	}

	private static void Run(Thread mainThread)
	{
		while (true)
		{
			Thread.Sleep(1000);
			if (DateTime.Now - lastCheckin.Value > TIMEOUT)
			{
				Logging.ServerConsoleLine("Server hung for more than 10 seconds. Cannot determine cause from watchdog thread", Level.Warn, null, Logging.tML);
				Checkin();
			}
		}
	}
}
