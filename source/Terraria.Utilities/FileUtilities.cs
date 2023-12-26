using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using ReLogic.OS;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.Social;
using Terraria.Social.Steam;

namespace Terraria.Utilities;

public static class FileUtilities
{
	private static Regex FileNameRegex = new Regex("^(?<path>.*[\\\\\\/])?(?:$|(?<fileName>.+?)(?:(?<extension>\\.[^.]*$)|$))", RegexOptions.IgnoreCase | RegexOptions.Compiled);

	public static bool Exists(string path, bool cloud)
	{
		if (cloud && SocialAPI.Cloud != null)
		{
			return SocialAPI.Cloud.HasFile(path);
		}
		return File.Exists(path);
	}

	public static void Delete(string path, bool cloud, bool forceDeleteFile = false)
	{
		if (cloud && SocialAPI.Cloud != null)
		{
			SocialAPI.Cloud.Delete(path);
		}
		else if (forceDeleteFile)
		{
			File.Delete(path);
		}
		else
		{
			Platform.Get<IPathService>().MoveToRecycleBin(path);
		}
	}

	public static string GetFullPath(string path, bool cloud)
	{
		if (!cloud)
		{
			return Path.GetFullPath(path);
		}
		return path;
	}

	public static void Copy(string source, string destination, bool cloud, bool overwrite = true)
	{
		CopyExtended(source, destination, cloud, overwrite);
	}

	public static void Move(string source, string destination, bool cloud, bool overwrite = true, bool forceDeleteSourceFile = false)
	{
		Copy(source, destination, cloud, overwrite);
		Delete(source, cloud, forceDeleteSourceFile);
	}

	public static int GetFileSize(string path, bool cloud)
	{
		if (cloud && SocialAPI.Cloud != null)
		{
			return SocialAPI.Cloud.GetFileSize(path);
		}
		return (int)new FileInfo(path).Length;
	}

	public static void Read(string path, byte[] buffer, int length, bool cloud)
	{
		if (cloud && SocialAPI.Cloud != null)
		{
			SocialAPI.Cloud.Read(path, buffer, length);
			return;
		}
		using FileStream fileStream = File.OpenRead(path);
		fileStream.Read(buffer, 0, length);
	}

	public static byte[] ReadAllBytes(string path, bool cloud)
	{
		if (cloud && SocialAPI.Cloud != null)
		{
			return SocialAPI.Cloud.Read(path);
		}
		return File.ReadAllBytes(path);
	}

	public static void WriteAllBytes(string path, byte[] data, bool cloud)
	{
		Write(path, data, data.Length, cloud);
	}

	public static void Write(string path, byte[] data, int length, bool cloud)
	{
		if (cloud && SocialAPI.Cloud != null)
		{
			SocialAPI.Cloud.Write(path, data, length);
			return;
		}
		string parentFolderPath = GetParentFolderPath(path);
		if (parentFolderPath != "")
		{
			Utils.TryCreatingDirectory(parentFolderPath);
		}
		RemoveReadOnlyAttribute(path);
		using FileStream fileStream = File.Open(path, FileMode.Create);
		while (fileStream.Position < length)
		{
			fileStream.Write(data, (int)fileStream.Position, Math.Min(length - (int)fileStream.Position, 2048));
		}
	}

	public static void RemoveReadOnlyAttribute(string path)
	{
		if (!File.Exists(path))
		{
			return;
		}
		try
		{
			FileAttributes attributes = File.GetAttributes(path);
			if ((attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
			{
				attributes &= ~FileAttributes.ReadOnly;
				File.SetAttributes(path, attributes);
			}
		}
		catch (Exception)
		{
		}
	}

	public static bool MoveToCloud(string localPath, string cloudPath)
	{
		if (SocialAPI.Cloud == null)
		{
			return false;
		}
		WriteAllBytes(cloudPath, ReadAllBytes(localPath, cloud: false), cloud: true);
		Delete(localPath, cloud: false);
		return true;
	}

	public static bool MoveToLocal(string cloudPath, string localPath)
	{
		if (SocialAPI.Cloud == null)
		{
			return false;
		}
		WriteAllBytes(localPath, ReadAllBytes(cloudPath, cloud: true), cloud: false);
		Delete(cloudPath, cloud: true);
		return true;
	}

	public static bool CopyToLocal(string cloudPath, string localPath)
	{
		if (SocialAPI.Cloud == null)
		{
			return false;
		}
		WriteAllBytes(localPath, ReadAllBytes(cloudPath, cloud: true), cloud: false);
		return true;
	}

	public static string GetFileName(string path, bool includeExtension = true)
	{
		Match match = FileNameRegex.Match(path);
		if (match == null || match.Groups["fileName"] == null)
		{
			return "";
		}
		includeExtension &= match.Groups["extension"] != null;
		return match.Groups["fileName"].Value + (includeExtension ? match.Groups["extension"].Value : "");
	}

	public static string GetParentFolderPath(string path, bool includeExtension = true)
	{
		Match match = FileNameRegex.Match(path);
		if (match == null || match.Groups["path"] == null)
		{
			return "";
		}
		return match.Groups["path"].Value;
	}

	public static void CopyFolder(string sourcePath, string destinationPath)
	{
		CopyFolderEXT(sourcePath, destinationPath, isCloud: false, null, overwriteAlways: true);
	}

	public static void ProtectedInvoke(Action action)
	{
		bool isBackground = Thread.CurrentThread.IsBackground;
		try
		{
			Thread.CurrentThread.IsBackground = false;
			action();
		}
		finally
		{
			Thread.CurrentThread.IsBackground = isBackground;
		}
	}

	public static void CopyExtended(string source, string destination, bool cloud, bool overwriteAlways, bool overwriteOld = true)
	{
		bool overwrite = DetermineIfShouldOverwrite(overwriteAlways, overwriteOld, source, destination);
		if (!overwrite && File.Exists(destination))
		{
			return;
		}
		if (!cloud)
		{
			try
			{
				File.Copy(source, destination, overwrite);
				return;
			}
			catch (IOException ex)
			{
				if (ex.GetType() != typeof(IOException))
				{
					throw;
				}
				using FileStream inputstream = File.OpenRead(source);
				using FileStream outputstream = File.Create(destination);
				inputstream.CopyTo(outputstream);
				return;
			}
		}
		string cloudSaveLocation = CoreSocialModule.GetCloudSaveLocation();
		destination = ConvertToRelativePath(cloudSaveLocation, destination);
		source = ConvertToRelativePath(cloudSaveLocation, source);
		if (SocialAPI.Cloud != null && (overwrite || !SocialAPI.Cloud.HasFile(destination)))
		{
			byte[] bytes = SocialAPI.Cloud.Read(source);
			SocialAPI.Cloud.Write(destination, bytes);
		}
	}

	public static void CopyFolderEXT(string sourcePath, string destinationPath, bool isCloud = false, Regex excludeFilter = null, bool overwriteAlways = false, bool overwriteOld = false)
	{
		Directory.CreateDirectory(destinationPath);
		string[] directories = Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories);
		for (int i = 0; i < directories.Length; i++)
		{
			string relativePath2 = ConvertToRelativePath(sourcePath, directories[i]);
			if (excludeFilter == null || !excludeFilter.IsMatch(relativePath2))
			{
				Directory.CreateDirectory(directories[i].Replace(sourcePath, destinationPath));
			}
		}
		directories = Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories);
		Logging.tML.Info((object)$"Number of files to Copy: {directories.Length}. Estimated time for HDD @15 MB/s: {directories.Length / 30} seconds");
		string[] array = directories;
		foreach (string obj in array)
		{
			string relativePath = ConvertToRelativePath(sourcePath, obj);
			if (excludeFilter == null || !excludeFilter.IsMatch(relativePath))
			{
				CopyExtended(obj, obj.Replace(sourcePath, destinationPath), isCloud, overwriteAlways, overwriteOld);
			}
		}
	}

	/// <summary>
	/// Converts the full 'path' to remove the base path component.
	/// Example: C://My Documents//Help Me I'm Hungry.txt is full 'path'
	/// 	basePath is C://My Documents
	/// 	Thus returns 'Help Me I'm Hungry.txt'
	/// </summary>
	public static string ConvertToRelativePath(string basePath, string fullPath)
	{
		if (!fullPath.StartsWith(basePath))
		{
			Logging.tML.Debug((object)$"string {fullPath} does not contain string {basePath}. Is this correct?");
			return fullPath;
		}
		return fullPath.Substring(basePath.Length + 1);
	}

	/// <summary>
	/// DEtermines if should overwrite the file at Destination with the file at Source
	/// </summary>
	private static bool DetermineIfShouldOverwrite(bool overwriteAlways, bool overwriteOld, string source, string destination)
	{
		if (overwriteAlways)
		{
			return true;
		}
		if (!File.Exists(destination))
		{
			return overwriteAlways;
		}
		if (!overwriteOld)
		{
			return false;
		}
		DateTime srcFile = File.GetLastWriteTimeUtc(source);
		return File.GetLastWriteTimeUtc(destination) < srcFile;
	}

	public static (string path, string message, int stabilityLevel)[] GetAlternateSavePathFiles(string folderName)
	{
		return new(string, string, int)[6]
		{
			(Path.Combine(Platform.Get<IPathService>().GetStoragePath("Terraria"), folderName ?? ""), "Click to copy \"{0}\" over from Terraria", 0),
			(Path.Combine(Platform.Get<IPathService>().GetStoragePath("Terraria"), "ModLoader", folderName ?? ""), "Click to copy \"{0}\" over from 1.3 tModLoader", 0),
			(Path.Combine(Main.SavePath, "..", "tModLoader", folderName ?? ""), "Click to copy \"{0}\" over from stable", 1),
			(Path.Combine(Main.SavePath, "..", "tModLoader-preview", folderName ?? ""), "Click to copy \"{0}\" over from preview", 2),
			(Path.Combine(Main.SavePath, "..", "tModLoader-dev", folderName ?? ""), "Click to copy \"{0}\" over from dev", 3),
			(Path.Combine(Main.SavePath, "..", "tModLoader-1.4.3", folderName ?? ""), "Click to copy \"{0}\" over from 1.4.3-Legacy", 0)
		};
	}

	internal static bool WriteTagCompound(string path, bool isCloud, TagCompound tag)
	{
		MemoryStream stream = new MemoryStream();
		TagIO.ToStream(tag, stream);
		byte[] data = stream.ToArray();
		string fileName = Path.GetFileName(path);
		if (data[0] != 31 || data[1] != 139)
		{
			Write(path + ".corr", data, data.Length, isCloud);
			throw new IOException("Detected Corrupted Save Stream attempt.\nAborting to avoid " + fileName + " corruption.\nYour last successful save will be kept. ERROR: Stream Missing NBT Header.");
		}
		Write(path, data, data.Length, isCloud);
		if (ReadAllBytes(path, isCloud).SequenceEqual(data))
		{
			return true;
		}
		Logging.tML.Warn((object)("Detected failed save for " + fileName + ". Re-attempting after 2 seconds"));
		Thread.Sleep(2000);
		Write(path, data, data.Length, isCloud);
		if (!ReadAllBytes(path, isCloud).SequenceEqual(data))
		{
			throw new IOException("Unable to save current progress.\nAborting to avoid " + fileName + " corruption.\nYour last successful save will be kept. ERROR: Stream Missing NBT Header.");
		}
		return true;
	}
}
