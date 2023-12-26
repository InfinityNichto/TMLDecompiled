using System;
using System.Linq;
using System.Reflection;

namespace Terraria.ModLoader;

public static class BuildInfo
{
	public enum BuildPurpose
	{
		Dev,
		Preview,
		Stable
	}

	public static readonly string BuildIdentifier;

	public static readonly Version tMLVersion;

	public static readonly Version stableVersion;

	public static readonly BuildPurpose Purpose;

	public static readonly string BranchName;

	public static readonly string CommitSHA;

	/// <summary>
	/// local time, for display purposes
	/// </summary>
	public static readonly DateTime BuildDate;

	public static readonly string versionedName;

	public static readonly string versionTag;

	public static readonly string versionedNameDevFriendly;

	public static bool IsStable => Purpose == BuildPurpose.Stable;

	public static bool IsPreview => Purpose == BuildPurpose.Preview;

	public static bool IsDev => Purpose == BuildPurpose.Dev;

	static BuildInfo()
	{
		BuildIdentifier = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;
		string[] array = BuildIdentifier.Substring(BuildIdentifier.IndexOf('+') + 1).Split('|');
		int i = 0;
		tMLVersion = new Version(array[i++]);
		stableVersion = new Version(array[i++]);
		BranchName = array[i++];
		Enum.TryParse<BuildPurpose>(array[i++], ignoreCase: true, out Purpose);
		CommitSHA = array[i++];
		BuildDate = DateTime.FromBinary(long.Parse(array[i++])).ToLocalTime();
		versionedName = $"tModLoader v{tMLVersion}";
		string[] branchNameBlacklist = new string[4] { "unknown", "stable", "preview", "1.4.3-Legacy" };
		if (!string.IsNullOrEmpty(BranchName) && !branchNameBlacklist.Contains(BranchName))
		{
			versionedName = versionedName + " " + BranchName;
		}
		if (Purpose != BuildPurpose.Stable)
		{
			versionedName += $" {Purpose}";
		}
		versionTag = versionedName.Substring("tModLoader ".Length).Replace(' ', '-').ToLower();
		versionedNameDevFriendly = versionedName;
		if (CommitSHA != "unknown")
		{
			versionedNameDevFriendly = versionedNameDevFriendly + " " + CommitSHA.Substring(0, 8);
		}
		versionedNameDevFriendly += $", built {BuildDate:g}";
	}
}
