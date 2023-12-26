using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Terraria.ModLoader.Core;

internal class BuildProperties
{
	internal struct ModReference
	{
		public string mod;

		public Version target;

		public ModReference(string mod, Version target)
		{
			this.mod = mod;
			this.target = target;
		}

		public override string ToString()
		{
			if (!(target == null))
			{
				return mod + "@" + target;
			}
			return mod;
		}

		public static ModReference Parse(string spec)
		{
			string[] split = spec.Split('@');
			if (split.Length == 1)
			{
				return new ModReference(split[0], null);
			}
			if (split.Length > 2)
			{
				throw new Exception("Invalid mod reference: " + spec);
			}
			try
			{
				return new ModReference(split[0], new Version(split[1]));
			}
			catch
			{
				throw new Exception("Invalid mod reference: " + spec);
			}
		}
	}

	internal string[] dllReferences = new string[0];

	internal ModReference[] modReferences = new ModReference[0];

	internal ModReference[] weakReferences = new ModReference[0];

	internal string[] sortAfter = new string[0];

	internal string[] sortBefore = new string[0];

	internal string[] buildIgnores = new string[0];

	internal string author = "";

	internal Version version = new Version(1, 0);

	internal string displayName = "";

	internal bool noCompile;

	internal bool hideCode;

	internal bool hideResources;

	internal bool includeSource;

	internal string eacPath = "";

	internal bool beta;

	internal Version buildVersion = BuildInfo.tMLVersion;

	internal string homepage = "";

	internal string description = "";

	internal ModSide side;

	internal bool playableOnPreview = true;

	internal bool translationMod;

	public IEnumerable<ModReference> Refs(bool includeWeak)
	{
		if (!includeWeak)
		{
			return modReferences;
		}
		return modReferences.Concat(weakReferences);
	}

	public IEnumerable<string> RefNames(bool includeWeak)
	{
		return from dep in Refs(includeWeak)
			select dep.mod;
	}

	private static IEnumerable<string> ReadList(string value)
	{
		return from s in value.Split(',')
			select s.Trim() into s
			where s.Length > 0
			select s;
	}

	private static IEnumerable<string> ReadList(BinaryReader reader)
	{
		List<string> list = new List<string>();
		string item = reader.ReadString();
		while (item.Length > 0)
		{
			list.Add(item);
			item = reader.ReadString();
		}
		return list;
	}

	private static void WriteList<T>(IEnumerable<T> list, BinaryWriter writer)
	{
		foreach (T item in list)
		{
			writer.Write(item.ToString());
		}
		writer.Write("");
	}

	internal static BuildProperties ReadBuildFile(string modDir)
	{
		string propertiesFile = modDir + Path.DirectorySeparatorChar + "build.txt";
		string descriptionfile = modDir + Path.DirectorySeparatorChar + "description.txt";
		BuildProperties properties = new BuildProperties();
		if (!File.Exists(propertiesFile))
		{
			return properties;
		}
		if (File.Exists(descriptionfile))
		{
			properties.description = File.ReadAllText(descriptionfile);
		}
		string[] array = File.ReadAllLines(propertiesFile);
		foreach (string line in array)
		{
			if (string.IsNullOrWhiteSpace(line))
			{
				continue;
			}
			int split = line.IndexOf('=');
			string property = line.Substring(0, split).Trim();
			string value = line.Substring(split + 1).Trim();
			if (value.Length == 0)
			{
				continue;
			}
			switch (property)
			{
			case "dllReferences":
				properties.dllReferences = ReadList(value).ToArray();
				break;
			case "modReferences":
				properties.modReferences = ReadList(value).Select(ModReference.Parse).ToArray();
				break;
			case "weakReferences":
				properties.weakReferences = ReadList(value).Select(ModReference.Parse).ToArray();
				break;
			case "sortBefore":
				properties.sortBefore = ReadList(value).ToArray();
				break;
			case "sortAfter":
				properties.sortAfter = ReadList(value).ToArray();
				break;
			case "author":
				properties.author = value;
				break;
			case "version":
				properties.version = new Version(value);
				break;
			case "displayName":
				properties.displayName = value;
				break;
			case "homepage":
				properties.homepage = value;
				break;
			case "noCompile":
				properties.noCompile = string.Equals(value, "true", StringComparison.OrdinalIgnoreCase);
				break;
			case "playableOnPreview":
				properties.playableOnPreview = string.Equals(value, "true", StringComparison.OrdinalIgnoreCase);
				break;
			case "translationMod":
				properties.translationMod = string.Equals(value, "true", StringComparison.OrdinalIgnoreCase);
				break;
			case "hideCode":
				properties.hideCode = string.Equals(value, "true", StringComparison.OrdinalIgnoreCase);
				break;
			case "hideResources":
				properties.hideResources = string.Equals(value, "true", StringComparison.OrdinalIgnoreCase);
				break;
			case "includeSource":
				properties.includeSource = string.Equals(value, "true", StringComparison.OrdinalIgnoreCase);
				break;
			case "buildIgnore":
				properties.buildIgnores = (from s in value.Split(',')
					select s.Trim().Replace('\\', Path.DirectorySeparatorChar).Replace('/', Path.DirectorySeparatorChar) into s
					where s.Length > 0
					select s).ToArray();
				break;
			case "side":
				if (!Enum.TryParse<ModSide>(value, ignoreCase: true, out properties.side))
				{
					throw new Exception("side is not one of (Both, Client, Server, NoSync): " + value);
				}
				break;
			}
		}
		List<string> refs = properties.RefNames(includeWeak: true).ToList();
		if (refs.Count != refs.Distinct().Count())
		{
			throw new Exception("Duplicate mod/weak reference");
		}
		properties.sortAfter = (from dep in properties.RefNames(includeWeak: true)
			where !properties.sortBefore.Contains(dep)
			select dep).Concat(properties.sortAfter).Distinct().ToArray();
		return properties;
	}

	internal byte[] ToBytes()
	{
		using MemoryStream memoryStream = new MemoryStream();
		using (BinaryWriter writer = new BinaryWriter(memoryStream))
		{
			if (dllReferences.Length != 0)
			{
				writer.Write("dllReferences");
				WriteList(dllReferences, writer);
			}
			if (modReferences.Length != 0)
			{
				writer.Write("modReferences");
				WriteList(modReferences, writer);
			}
			if (weakReferences.Length != 0)
			{
				writer.Write("weakReferences");
				WriteList(weakReferences, writer);
			}
			if (sortAfter.Length != 0)
			{
				writer.Write("sortAfter");
				WriteList(sortAfter, writer);
			}
			if (sortBefore.Length != 0)
			{
				writer.Write("sortBefore");
				WriteList(sortBefore, writer);
			}
			if (author.Length > 0)
			{
				writer.Write("author");
				writer.Write(author);
			}
			writer.Write("version");
			writer.Write(version.ToString());
			if (displayName.Length > 0)
			{
				writer.Write("displayName");
				writer.Write(displayName);
			}
			if (homepage.Length > 0)
			{
				writer.Write("homepage");
				writer.Write(homepage);
			}
			if (description.Length > 0)
			{
				writer.Write("description");
				writer.Write(description);
			}
			if (noCompile)
			{
				writer.Write("noCompile");
			}
			if (!playableOnPreview)
			{
				writer.Write("!playableOnPreview");
			}
			if (translationMod)
			{
				writer.Write("translationMod");
			}
			if (!hideCode)
			{
				writer.Write("!hideCode");
			}
			if (!hideResources)
			{
				writer.Write("!hideResources");
			}
			if (includeSource)
			{
				writer.Write("includeSource");
			}
			if (eacPath.Length > 0)
			{
				writer.Write("eacPath");
				writer.Write(eacPath);
			}
			if (side != 0)
			{
				writer.Write("side");
				writer.Write((byte)side);
			}
			writer.Write("buildVersion");
			writer.Write(buildVersion.ToString());
			writer.Write("");
		}
		return memoryStream.ToArray();
	}

	internal static BuildProperties ReadModFile(TmodFile modFile)
	{
		return ReadFromStream(modFile.GetStream("Info"));
	}

	internal static BuildProperties ReadFromStream(Stream stream)
	{
		BuildProperties properties = new BuildProperties();
		properties.hideCode = true;
		properties.hideResources = true;
		using BinaryReader reader = new BinaryReader(stream);
		string tag = reader.ReadString();
		while (tag.Length > 0)
		{
			if (tag == "dllReferences")
			{
				properties.dllReferences = ReadList(reader).ToArray();
			}
			if (tag == "modReferences")
			{
				properties.modReferences = ReadList(reader).Select(ModReference.Parse).ToArray();
			}
			if (tag == "weakReferences")
			{
				properties.weakReferences = ReadList(reader).Select(ModReference.Parse).ToArray();
			}
			if (tag == "sortAfter")
			{
				properties.sortAfter = ReadList(reader).ToArray();
			}
			if (tag == "sortBefore")
			{
				properties.sortBefore = ReadList(reader).ToArray();
			}
			if (tag == "author")
			{
				properties.author = reader.ReadString();
			}
			if (tag == "version")
			{
				properties.version = new Version(reader.ReadString());
			}
			if (tag == "displayName")
			{
				properties.displayName = reader.ReadString();
			}
			if (tag == "homepage")
			{
				properties.homepage = reader.ReadString();
			}
			if (tag == "description")
			{
				properties.description = reader.ReadString();
			}
			if (tag == "noCompile")
			{
				properties.noCompile = true;
			}
			if (tag == "!playableOnPreview")
			{
				properties.playableOnPreview = false;
			}
			if (tag == "translationMod")
			{
				properties.translationMod = true;
			}
			if (tag == "!hideCode")
			{
				properties.hideCode = false;
			}
			if (tag == "!hideResources")
			{
				properties.hideResources = false;
			}
			if (tag == "includeSource")
			{
				properties.includeSource = true;
			}
			if (tag == "eacPath")
			{
				properties.eacPath = reader.ReadString();
			}
			if (tag == "side")
			{
				properties.side = (ModSide)reader.ReadByte();
			}
			if (tag == "buildVersion")
			{
				properties.buildVersion = new Version(reader.ReadString());
			}
			tag = reader.ReadString();
		}
		return properties;
	}

	internal static void InfoToBuildTxt(Stream src, Stream dst)
	{
		BuildProperties properties = ReadFromStream(src);
		StringBuilder sb = new StringBuilder();
		StringBuilder stringBuilder;
		StringBuilder.AppendInterpolatedStringHandler handler;
		if (properties.displayName.Length > 0)
		{
			stringBuilder = sb;
			StringBuilder stringBuilder2 = stringBuilder;
			handler = new StringBuilder.AppendInterpolatedStringHandler(14, 1, stringBuilder);
			handler.AppendLiteral("displayName = ");
			handler.AppendFormatted(properties.displayName);
			stringBuilder2.AppendLine(ref handler);
		}
		if (properties.author.Length > 0)
		{
			stringBuilder = sb;
			StringBuilder stringBuilder3 = stringBuilder;
			handler = new StringBuilder.AppendInterpolatedStringHandler(9, 1, stringBuilder);
			handler.AppendLiteral("author = ");
			handler.AppendFormatted(properties.author);
			stringBuilder3.AppendLine(ref handler);
		}
		stringBuilder = sb;
		StringBuilder stringBuilder4 = stringBuilder;
		handler = new StringBuilder.AppendInterpolatedStringHandler(10, 1, stringBuilder);
		handler.AppendLiteral("version = ");
		handler.AppendFormatted(properties.version);
		stringBuilder4.AppendLine(ref handler);
		if (properties.homepage.Length > 0)
		{
			stringBuilder = sb;
			StringBuilder stringBuilder5 = stringBuilder;
			handler = new StringBuilder.AppendInterpolatedStringHandler(11, 1, stringBuilder);
			handler.AppendLiteral("homepage = ");
			handler.AppendFormatted(properties.homepage);
			stringBuilder5.AppendLine(ref handler);
		}
		if (properties.dllReferences.Length != 0)
		{
			stringBuilder = sb;
			StringBuilder stringBuilder6 = stringBuilder;
			handler = new StringBuilder.AppendInterpolatedStringHandler(16, 1, stringBuilder);
			handler.AppendLiteral("dllReferences = ");
			handler.AppendFormatted(string.Join(", ", properties.dllReferences));
			stringBuilder6.AppendLine(ref handler);
		}
		if (properties.modReferences.Length != 0)
		{
			stringBuilder = sb;
			StringBuilder stringBuilder7 = stringBuilder;
			handler = new StringBuilder.AppendInterpolatedStringHandler(16, 1, stringBuilder);
			handler.AppendLiteral("modReferences = ");
			handler.AppendFormatted(string.Join(", ", properties.modReferences));
			stringBuilder7.AppendLine(ref handler);
		}
		if (properties.weakReferences.Length != 0)
		{
			stringBuilder = sb;
			StringBuilder stringBuilder8 = stringBuilder;
			handler = new StringBuilder.AppendInterpolatedStringHandler(17, 1, stringBuilder);
			handler.AppendLiteral("weakReferences = ");
			handler.AppendFormatted(string.Join(", ", properties.weakReferences));
			stringBuilder8.AppendLine(ref handler);
		}
		if (properties.noCompile)
		{
			sb.AppendLine("noCompile = true");
		}
		if (properties.hideCode)
		{
			sb.AppendLine("hideCode = true");
		}
		if (properties.hideResources)
		{
			sb.AppendLine("hideResources = true");
		}
		if (properties.includeSource)
		{
			sb.AppendLine("includeSource = true");
		}
		if (!properties.playableOnPreview)
		{
			sb.AppendLine("playableOnPreview = false");
		}
		if (properties.translationMod)
		{
			sb.AppendLine("translationMod = true");
		}
		if (properties.side != 0)
		{
			stringBuilder = sb;
			StringBuilder stringBuilder9 = stringBuilder;
			handler = new StringBuilder.AppendInterpolatedStringHandler(7, 1, stringBuilder);
			handler.AppendLiteral("side = ");
			handler.AppendFormatted(properties.side);
			stringBuilder9.AppendLine(ref handler);
		}
		if (properties.sortAfter.Length != 0)
		{
			stringBuilder = sb;
			StringBuilder stringBuilder10 = stringBuilder;
			handler = new StringBuilder.AppendInterpolatedStringHandler(12, 1, stringBuilder);
			handler.AppendLiteral("sortAfter = ");
			handler.AppendFormatted(string.Join(", ", properties.sortAfter));
			stringBuilder10.AppendLine(ref handler);
		}
		if (properties.sortBefore.Length != 0)
		{
			stringBuilder = sb;
			StringBuilder stringBuilder11 = stringBuilder;
			handler = new StringBuilder.AppendInterpolatedStringHandler(13, 1, stringBuilder);
			handler.AppendLiteral("sortBefore = ");
			handler.AppendFormatted(string.Join(", ", properties.sortBefore));
			stringBuilder11.AppendLine(ref handler);
		}
		byte[] bytes = Encoding.UTF8.GetBytes(sb.ToString());
		dst.Write(bytes, 0, bytes.Length);
	}

	internal bool ignoreFile(string resource)
	{
		return buildIgnores.Any((string fileMask) => FitsMask(resource, fileMask));
	}

	private bool FitsMask(string fileName, string fileMask)
	{
		return new Regex("^" + Regex.Escape(fileMask.Replace(".", "__DOT__").Replace("*", "__STAR__").Replace("?", "__QM__")).Replace("__DOT__", "[.]").Replace("__STAR__", ".*")
			.Replace("__QM__", ".") + "$", RegexOptions.IgnoreCase).IsMatch(fileName);
	}
}
