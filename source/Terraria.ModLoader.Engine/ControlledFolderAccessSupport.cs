using System;
using System.Runtime.Versioning;
using Microsoft.Win32;

namespace Terraria.ModLoader.Engine;

internal class ControlledFolderAccessSupport
{
	internal static bool ControlledFolderAccessDetected;

	internal static bool ControlledFolderAccessDetectionPrevented;

	internal static void CheckFileSystemAccess()
	{
		try
		{
			if (OperatingSystem.IsWindows() && Environment.OSVersion.Version.Major >= 10)
			{
				CheckRegistryValues();
			}
		}
		catch
		{
			ControlledFolderAccessDetectionPrevented = true;
		}
	}

	[SupportedOSPlatform("windows")]
	private static void CheckRegistryValues()
	{
		if (Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows Defender\\Windows Defender Exploit Guard\\Controlled Folder Access", "EnableControlledFolderAccess", -1) is int EnableControlledFolderAccessValue)
		{
			ControlledFolderAccessDetected = EnableControlledFolderAccessValue == 1;
		}
	}
}
