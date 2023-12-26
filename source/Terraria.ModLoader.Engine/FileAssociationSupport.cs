using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using Microsoft.Win32;

namespace Terraria.ModLoader.Engine;

internal class FileAssociationSupport
{
	private class FileAssociation
	{
		public string Extension { get; set; }

		public string ProgId { get; set; }

		public string FileTypeDescription { get; set; }

		public string ExecutableFilePath { get; set; }
	}

	private const int SHCNE_ASSOCCHANGED = 134217728;

	private const int SHCNF_FLUSH = 4096;

	internal static void HandleFileAssociation(string file)
	{
		Console.WriteLine("Attempting to install " + file);
		if (File.Exists(file))
		{
			string modName = Path.GetFileNameWithoutExtension(file);
			if (ModLoader.ModPath != Path.GetDirectoryName(file))
			{
				File.Copy(file, Path.Combine(ModLoader.ModPath, Path.GetFileName(file)), overwrite: true);
				File.Delete(file);
				Console.WriteLine(modName + " installed successfully");
			}
			ModLoader.EnableMod(modName);
			Console.WriteLine(modName + " enabled");
		}
		Console.WriteLine("Press any key to exit...");
		Console.ReadKey();
		Environment.Exit(0);
	}

	internal static void UpdateFileAssociation()
	{
		if (OperatingSystem.IsWindows() && Environment.OSVersion.Version.Major >= 6)
		{
			try
			{
				EnsureAssociationsSet();
			}
			catch (Exception)
			{
			}
		}
	}

	[DllImport("Shell32.dll")]
	private static extern int SHChangeNotify(int eventId, int flags, IntPtr item1, IntPtr item2);

	[SupportedOSPlatform("windows")]
	private static void EnsureAssociationsSet()
	{
		string filePath = Path.Combine(Directory.GetCurrentDirectory(), "tModLoader.dll");
		EnsureAssociationsSet(new FileAssociation
		{
			Extension = ".tmod",
			ProgId = "tModLoader_Mod_File",
			FileTypeDescription = "tModLoader Mod",
			ExecutableFilePath = filePath
		});
	}

	[SupportedOSPlatform("windows")]
	private static void EnsureAssociationsSet(params FileAssociation[] associations)
	{
		bool madeChanges = false;
		foreach (FileAssociation association in associations)
		{
			madeChanges |= SetAssociation(association.Extension, association.ProgId, association.FileTypeDescription, association.ExecutableFilePath);
		}
		if (madeChanges)
		{
			SHChangeNotify(134217728, 4096, IntPtr.Zero, IntPtr.Zero);
		}
	}

	[SupportedOSPlatform("windows")]
	private static bool SetAssociation(string extension, string progId, string fileTypeDescription, string applicationFilePath)
	{
		return false | SetKeyDefaultValue("Software\\Classes\\" + extension, progId) | SetKeyDefaultValue("Software\\Classes\\" + progId, fileTypeDescription) | SetKeyDefaultValue("Software\\Classes\\" + progId + "\\shell\\open\\command", "dotnet \"" + applicationFilePath + "\" -server -install \"%1\"");
	}

	[SupportedOSPlatform("windows")]
	private static bool SetKeyDefaultValue(string keyPath, string value)
	{
		using (RegistryKey key = Registry.CurrentUser.CreateSubKey(keyPath))
		{
			if (key.GetValue(null) as string != value)
			{
				key.SetValue(null, value);
				return true;
			}
		}
		return false;
	}
}
