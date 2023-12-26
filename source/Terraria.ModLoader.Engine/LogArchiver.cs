using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Ionic.Zip;
using Terraria.ModLoader.IO;

namespace Terraria.ModLoader.Engine;

/// <summary>
/// Log archiving is performed after log initialization in a separate class to avoid loading Ionic.Zip before logging initialises and it can be patched
/// Some CLRs will load all required assemblies when the class is entered, not necessarily just the method, so you've got to watch out
/// </summary>
internal static class LogArchiver
{
	private const int MAX_LOGS = 20;

	static LogArchiver()
	{
		Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
	}

	/// <summary>
	/// Attempt archiving logs.
	/// </summary>
	internal static void ArchiveLogs()
	{
		SetupLogDirs();
		MoveZipsToArchiveDir();
		Archive();
		DeleteOldArchives();
	}

	private static IEnumerable<string> GetArchivedLogs()
	{
		try
		{
			return Directory.EnumerateFiles(Logging.LogArchiveDir, "*.zip");
		}
		catch (Exception e)
		{
			Logging.tML.Error((object)e);
			return Enumerable.Empty<string>();
		}
	}

	private static IEnumerable<string> GetOldLogs()
	{
		try
		{
			return Directory.EnumerateFiles(Logging.LogDir, "*.old*");
		}
		catch (Exception e)
		{
			Logging.tML.Error((object)e);
			return Enumerable.Empty<string>();
		}
	}

	private static void SetupLogDirs()
	{
		try
		{
			Directory.CreateDirectory(Logging.LogArchiveDir);
		}
		catch (Exception e)
		{
			Logging.tML.Error((object)e);
		}
	}

	private static void MoveZipsToArchiveDir()
	{
		bool justdelete = GetArchivedLogs().Any();
		foreach (string path in Directory.EnumerateFiles(Logging.LogDir, "*.zip"))
		{
			try
			{
				if (justdelete)
				{
					File.Delete(path);
				}
				else
				{
					File.Move(path, Path.Combine(Logging.LogArchiveDir, Path.GetFileName(path)));
				}
			}
			catch (Exception e)
			{
				Logging.tML.Error((object)e);
			}
		}
	}

	private static void Archive()
	{
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Expected O, but got Unknown
		List<string> logFiles = GetOldLogs().ToList();
		if (!logFiles.Any())
		{
			return;
		}
		DateTime time;
		try
		{
			time = logFiles.Select(File.GetCreationTime).Min();
		}
		catch (Exception e3)
		{
			Logging.tML.Error((object)e3);
			return;
		}
		int i = 1;
		Regex pattern = new Regex($"{time:yyyy-MM-dd}-(\\d+)\\.zip");
		string[] existingLogArchives = new string[0];
		try
		{
			existingLogArchives = (from s in Directory.GetFiles(Logging.LogArchiveDir)
				where pattern.IsMatch(Path.GetFileName(s))
				select s).ToArray();
		}
		catch (Exception e2)
		{
			Logging.tML.Error((object)e2);
			return;
		}
		if (existingLogArchives.Length != 0)
		{
			i = existingLogArchives.Select((string s) => int.Parse(pattern.Match(Path.GetFileName(s)).Groups[1].Value)).Max() + 1;
		}
		try
		{
			ZipFile zip = new ZipFile(Path.Combine(Logging.LogArchiveDir, $"{time:yyyy-MM-dd}-{i}.zip"), Encoding.UTF8);
			try
			{
				foreach (string logFile in logFiles)
				{
					string entryName = Path.GetFileNameWithoutExtension(logFile);
					using (FileStream stream = File.OpenRead(logFile))
					{
						if (stream.Length > 10000000)
						{
							Logging.tML.Warn((object)(logFile + " exceeds 10MB, it will be truncated for the logs archive."));
							zip.AddEntry(entryName, stream.ReadBytes(10000000));
						}
						else
						{
							zip.AddEntry(entryName, (Stream)stream);
						}
						zip.Save();
					}
					File.Delete(logFile);
				}
			}
			finally
			{
				((IDisposable)zip)?.Dispose();
			}
		}
		catch (Exception e)
		{
			Logging.tML.Error((object)e);
		}
	}

	private static void DeleteOldArchives()
	{
		List<string> existingLogs = GetArchivedLogs().OrderBy(File.GetCreationTime).ToList();
		foreach (string f in existingLogs.Take(existingLogs.Count - 20))
		{
			try
			{
				File.Delete(f);
			}
			catch (Exception e)
			{
				Logging.tML.Error((object)e);
			}
		}
	}
}
