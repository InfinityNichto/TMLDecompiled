using System;

namespace Terraria.ModLoader.Core;

internal class LocalMod
{
	public readonly TmodFile modFile;

	public readonly BuildProperties properties;

	public DateTime lastModified;

	public string Name => modFile.Name;

	public string DisplayName
	{
		get
		{
			if (!string.IsNullOrEmpty(properties.displayName))
			{
				return properties.displayName;
			}
			return Name;
		}
	}

	public Version tModLoaderVersion => properties.buildVersion;

	public bool Enabled
	{
		get
		{
			return ModLoader.IsEnabled(Name);
		}
		set
		{
			ModLoader.SetModEnabled(Name, value);
		}
	}

	public override string ToString()
	{
		return Name;
	}

	public LocalMod(TmodFile modFile, BuildProperties properties)
	{
		this.modFile = modFile;
		this.properties = properties;
	}

	public LocalMod(TmodFile modFile)
		: this(modFile, BuildProperties.ReadModFile(modFile))
	{
	}
}
