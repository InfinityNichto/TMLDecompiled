using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Ionic.Zip;
using Ionic.Zlib;
using Terraria.Social;
using Terraria.Utilities;

namespace Terraria.ModLoader;

internal static class BackupIO
{
	/// <summary>
	/// Responsible for archiving world backups
	/// </summary>
	public static class World
	{
		public static readonly string WorldDir = Path.Combine(Main.SavePath, "Worlds");

		public static readonly string WorldBackupDir = Path.Combine(WorldDir, "Backups");

		internal static void ArchiveWorld(string path, bool isCloudSave)
		{
			RunArchiving(WriteArchive, isCloudSave, WorldBackupDir, Path.GetFileNameWithoutExtension(path), path);
		}

		private static void WriteArchive(ZipFile zip, bool isCloudSave, string path)
		{
			if (FileUtilities.Exists(path, isCloudSave))
			{
				zip.AddZipEntry(path, isCloudSave);
			}
			path = Path.ChangeExtension(path, ".twld");
			if (FileUtilities.Exists(path, isCloudSave))
			{
				zip.AddZipEntry(path, isCloudSave);
			}
		}
	}

	/// <summary>
	/// Responsible for archiving player backups
	/// </summary>
	public static class Player
	{
		public static readonly string PlayerDir = Path.Combine(Main.SavePath, "Players");

		public static readonly string PlayerBackupDir = Path.Combine(PlayerDir, "Backups");

		public static void ArchivePlayer(string path, bool isCloudSave)
		{
			RunArchiving(WriteArchive, isCloudSave, PlayerBackupDir, Path.GetFileNameWithoutExtension(path), path);
		}

		/// <summary>
		/// Write the archive. Writes the .plr and .tplr files, then writes the player directory
		/// </summary>
		private static void WriteArchive(ZipFile zip, bool isCloudSave, string path)
		{
			if (FileUtilities.Exists(path, isCloudSave))
			{
				zip.AddZipEntry(path, isCloudSave);
			}
			path = Path.ChangeExtension(path, ".tplr");
			if (FileUtilities.Exists(path, isCloudSave))
			{
				zip.AddZipEntry(path, isCloudSave);
			}
			if (isCloudSave)
			{
				WriteCloudFiles(zip, path);
			}
			else
			{
				WriteLocalFiles(zip, path);
			}
		}

		/// <summary>
		/// Write cloud files, which will get the relevant part of the path and write map &amp; tmap files
		/// </summary>
		private static void WriteCloudFiles(ZipFile zip, string path)
		{
			string name = Path.GetFileNameWithoutExtension(path);
			path = Path.ChangeExtension(path, "");
			path = path.Substring(0, path.Length - 1);
			foreach (string cloudPath in from p in SocialAPI.Cloud.GetFiles()
				where p.StartsWith(path, StringComparison.CurrentCultureIgnoreCase) && (p.EndsWith(".map", StringComparison.CurrentCultureIgnoreCase) || p.EndsWith(".tmap", StringComparison.CurrentCultureIgnoreCase))
				select p)
			{
				zip.AddEntry(name + "/" + Path.GetFileName(cloudPath), FileUtilities.ReadAllBytes(cloudPath, cloud: true));
			}
		}

		/// <summary>
		/// Write local files, which simply writes the entire player dir
		/// </summary>
		private static void WriteLocalFiles(ZipFile zip, string path)
		{
			string plrDir = Path.Combine(Path.GetDirectoryName(path), Path.GetFileNameWithoutExtension(path));
			if (Directory.Exists(plrDir))
			{
				zip.AddZipEntry(plrDir);
			}
		}
	}

	public static bool archiveLock = false;

	private static readonly Regex dateRegex = new Regex("\\d+-\\d\\d*-\\d\\d*", RegexOptions.Compiled);

	private static bool IsArchiveOlder(DateTime time, TimeSpan thresholdAge)
	{
		return DateTime.Now - time > thresholdAge;
	}

	private static string GetArchiveName(string name, bool isCloudSave)
	{
		return name + (isCloudSave ? "-cloud" : "");
	}

	private static string TodaysBackup(string name, bool isCloudSave)
	{
		return $"{DateTime.Now:yyyy-MM-dd}-{GetArchiveName(name, isCloudSave)}.zip";
	}

	private static bool TryGetTime(string file, out DateTime result)
	{
		Match match = dateRegex.Match(file);
		result = default(DateTime);
		if (match.Success)
		{
			return DateTime.TryParse(match.Value, out result);
		}
		return false;
	}

	/// <summary>
	/// Run a given archiving task, which will archive to a backup .zip file
	/// Zip entries added will be compressed
	/// </summary>
	private static void RunArchiving(Action<ZipFile, bool, string> saveAction, bool isCloudSave, string dir, string name, string path)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Expected O, but got Unknown
		try
		{
			Directory.CreateDirectory(dir);
			DeleteOldArchives(dir, isCloudSave, name);
			ZipFile zip = new ZipFile(Path.Combine(dir, TodaysBackup(name, isCloudSave)), Encoding.UTF8);
			try
			{
				zip.UseZip64WhenSaving = (Zip64Option)1;
				zip.ZipErrorAction = (ZipErrorAction)0;
				saveAction(zip, isCloudSave, path);
				zip.Save();
			}
			finally
			{
				((IDisposable)zip)?.Dispose();
			}
		}
		catch (Exception e)
		{
			Logging.tML.Error((object)"A problem occurred when trying to create a backup file.", e);
		}
	}

	/// <summary>
	/// Adds a new entry to the archive .zip file
	/// Will use the best compression level using Deflate
	/// Some files are already compressed and will not be compressed further
	/// </summary>
	private static void AddZipEntry(this ZipFile zip, string path, bool isCloud = false)
	{
		zip.CompressionMethod = (CompressionMethod)8;
		zip.CompressionLevel = (CompressionLevel)9;
		zip.Comment = $"Archived on ${DateTime.Now} by tModLoader";
		if (!isCloud && (File.GetAttributes(path) & FileAttributes.Directory) == FileAttributes.Directory)
		{
			zip.AddFiles((IEnumerable<string>)Directory.GetFiles(path), false, Path.GetFileNameWithoutExtension(path));
		}
		else if (isCloud)
		{
			zip.AddEntry(Path.GetFileName(path), FileUtilities.ReadAllBytes(path, cloud: true));
		}
		else
		{
			zip.AddFile(path, "");
		}
	}

	/// <summary>
	/// Will delete old archive files
	/// Algorithm details:
	/// - One backup per day for the last week
	/// - One backup per week for the last month
	/// - One backup per month for all time
	/// </summary>
	private static void DeleteOldArchives(string dir, bool isCloudSave, string name)
	{
		string path = Path.Combine(dir, TodaysBackup(name, isCloudSave));
		if (File.Exists(path))
		{
			DeleteArchive(path);
		}
		DateTime result;
		(FileInfo f, DateTime)[] array = (from f in new DirectoryInfo(dir).GetFiles("*" + GetArchiveName(name, isCloudSave) + "*.zip", SearchOption.TopDirectoryOnly)
			select (f: f, TryGetTime(f.Name, out result) ? result : default(DateTime)) into tuple
			where tuple.Item2 != default(DateTime)
			orderby tuple.Item2
			select tuple).ToArray();
		(FileInfo, DateTime)? previous = null;
		(FileInfo, DateTime)[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			(FileInfo, DateTime) archived = array2[i];
			if (!previous.HasValue)
			{
				previous = archived;
				continue;
			}
			int freshness = (IsArchiveOlder(archived.Item2, TimeSpan.FromDays(30.0)) ? 30 : ((!IsArchiveOlder(archived.Item2, TimeSpan.FromDays(7.0))) ? 1 : 7));
			if ((archived.Item2 - previous.Value.Item2).Days < freshness)
			{
				DeleteArchive(previous.Value.Item1.FullName);
			}
			previous = archived;
		}
	}

	private static void DeleteArchive(string path)
	{
		try
		{
			File.Delete(path);
		}
		catch (Exception e)
		{
			Logging.tML.Error((object)"Problem deleting old archive file", e);
		}
	}
}
