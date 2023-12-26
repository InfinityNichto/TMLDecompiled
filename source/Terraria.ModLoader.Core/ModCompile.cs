using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Basic.Reference.Assemblies;
using log4net.Core;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using Terraria.Localization;
using Terraria.ModLoader.Exceptions;
using Terraria.Social.Steam;

namespace Terraria.ModLoader.Core;

internal class ModCompile
{
	public interface IBuildStatus
	{
		void SetProgress(int i, int n = -1);

		void SetStatus(string msg);

		void LogCompilerLine(string msg, Level level);
	}

	private class ConsoleBuildStatus : IBuildStatus
	{
		public void SetProgress(int i, int n)
		{
		}

		public void SetStatus(string msg)
		{
			Console.WriteLine(msg);
		}

		public void LogCompilerLine(string msg, Level level)
		{
			((level == Level.Error) ? Console.Error : Console.Out).WriteLine(msg);
		}
	}

	private class BuildingMod : LocalMod
	{
		public string path;

		public BuildingMod(TmodFile modFile, BuildProperties properties, string path)
			: base(modFile, properties)
		{
			this.path = path;
		}
	}

	public static readonly string ModSourcePath = Path.Combine(Program.SavePathShared, "ModSources");

	public static bool activelyModding;

	private static readonly string tMLDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

	private static readonly string oldModReferencesPath = Path.Combine(Program.SavePath, "references");

	private static readonly string modTargetsPath = Path.Combine(ModSourcePath, "tModLoader.targets");

	private static readonly string tMLModTargetsPath = Path.Combine(tMLDir, "tMLMod.targets");

	private static bool referencesUpdated = false;

	internal static IList<string> sourceExtensions = new List<string> { ".csproj", ".cs", ".sln" };

	private IBuildStatus status;

	private int packedResourceCount;

	public static bool DeveloperMode
	{
		get
		{
			if (!Debugger.IsAttached)
			{
				if (Directory.Exists(ModSourcePath))
				{
					return FindModSources().Length != 0;
				}
				return false;
			}
			return true;
		}
	}

	internal static string[] FindModSources()
	{
		Directory.CreateDirectory(ModSourcePath);
		return Directory.GetDirectories(ModSourcePath, "*", SearchOption.TopDirectoryOnly).Where(delegate(string dir)
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(dir);
			return directoryInfo.Name[0] != '.' && directoryInfo.Name != "ModAssemblies" && directoryInfo.Name != "Mod Libraries";
		}).ToArray();
	}

	internal static void UpdateReferencesFolder()
	{
		if (referencesUpdated)
		{
			return;
		}
		try
		{
			if (Directory.Exists(oldModReferencesPath))
			{
				Directory.Delete(oldModReferencesPath, recursive: true);
			}
		}
		catch (Exception e)
		{
			Logging.tML.Error((object)"Failed to delete old /references dir", e);
		}
		UpdateFileContents(modTargetsPath, "<Project ToolsVersion=\"14.0\" xmlns=\"http://schemas.microsoft.com/developer/msbuild/2003\">\r\n  <Import Project=\"" + SecurityElement.Escape(tMLModTargetsPath) + "\" />\r\n</Project>");
		referencesUpdated = true;
	}

	private static void UpdateFileContents(string path, string contents)
	{
		Directory.CreateDirectory(Path.GetDirectoryName(path));
		byte[] bytes = Encoding.UTF8.GetBytes(contents);
		if (!File.Exists(path) || !bytes.SequenceEqual(File.ReadAllBytes(path)))
		{
			File.WriteAllBytes(path, bytes);
		}
	}

	public ModCompile(IBuildStatus status)
	{
		this.status = status;
		activelyModding = true;
		Logging.ResetPastExceptions();
	}

	internal void BuildAll()
	{
		List<LocalMod> modList = new List<LocalMod>();
		string[] array = FindModSources();
		foreach (string modFolder in array)
		{
			modList.Add(ReadBuildInfo(modFolder));
		}
		List<LocalMod> installedMods = (from mod in ModOrganizer.FindMods()
			where !modList.Exists((LocalMod m) => m.Name == mod.Name)
			select mod).ToList();
		HashSet<LocalMod> requiredFromInstall = new HashSet<LocalMod>();
		foreach (LocalMod mod3 in modList)
		{
			Require(mod3, includeWeak: true);
		}
		modList.AddRange(requiredFromInstall);
		List<BuildingMod> modsToBuild;
		try
		{
			ModOrganizer.EnsureDependenciesExist(modList, includeWeak: true);
			ModOrganizer.EnsureTargetVersionsMet(modList);
			modsToBuild = ModOrganizer.Sort(modList).OfType<BuildingMod>().ToList();
		}
		catch (ModSortingException ex)
		{
			throw new BuildException(ex.Message);
		}
		int num = 0;
		foreach (BuildingMod mod2 in modsToBuild)
		{
			status.SetProgress(num++, modsToBuild.Count);
			Build(mod2);
		}
		void Require(LocalMod mod, bool includeWeak)
		{
			foreach (string dep in mod.properties.RefNames(includeWeak))
			{
				LocalMod depMod = installedMods.SingleOrDefault((LocalMod m) => m.Name == dep);
				if (depMod != null && requiredFromInstall.Add(depMod))
				{
					Require(depMod, includeWeak: false);
				}
			}
		}
	}

	internal static void BuildModCommandLine(string modFolder)
	{
		UpdateReferencesFolder();
		LanguageManager.Instance.SetLanguage(GameCulture.DefaultCulture);
		Lang.InitializeLegacyLocalization();
		try
		{
			new ModCompile(new ConsoleBuildStatus()).Build(modFolder);
		}
		catch (BuildException e2)
		{
			Console.Error.WriteLine("Error: " + e2.Message);
			if (e2.InnerException != null)
			{
				Console.Error.WriteLine(e2.InnerException);
			}
			Environment.Exit(1);
		}
		catch (Exception e)
		{
			Console.Error.WriteLine(e);
			Environment.Exit(1);
		}
		WorkshopSocialModule.SteamCMDPublishPreparer(modFolder);
		Environment.Exit(0);
	}

	internal void Build(string modFolder)
	{
		Build(ReadBuildInfo(modFolder));
	}

	private BuildingMod ReadBuildInfo(string modFolder)
	{
		if (modFolder.EndsWith("\\") || modFolder.EndsWith("/"))
		{
			modFolder = modFolder.Substring(0, modFolder.Length - 1);
		}
		string modName = Path.GetFileName(modFolder);
		status.SetStatus(Language.GetTextValue("tModLoader.ReadingProperties", modName));
		BuildProperties properties;
		try
		{
			properties = BuildProperties.ReadBuildFile(modFolder);
		}
		catch (Exception e)
		{
			throw new BuildException(Language.GetTextValue("tModLoader.BuildErrorFailedLoadBuildTxt", Path.Combine(modFolder, "build.txt")), e);
		}
		return new BuildingMod(new TmodFile(Path.Combine(ModLoader.ModPath, modName + ".tmod"), modName, properties.version), properties, modFolder);
	}

	private void Build(BuildingMod mod)
	{
		try
		{
			status.SetStatus(Language.GetTextValue("tModLoader.Building", mod.Name));
			BuildMod(mod, out var code, out var pdb);
			mod.modFile.AddFile(mod.Name + ".dll", code);
			if (pdb != null)
			{
				mod.modFile.AddFile(mod.Name + ".pdb", pdb);
			}
			PackageMod(mod);
			if (ModLoader.TryGetMod(mod.Name, out var loadedMod))
			{
				loadedMod.Close();
			}
			mod.modFile.Save();
			ModLoader.EnableMod(mod.Name);
			LocalizationLoader.HandleModBuilt(mod.Name);
		}
		catch (Exception ex)
		{
			ex.Data["mod"] = mod.Name;
			throw;
		}
	}

	private void PackageMod(BuildingMod mod)
	{
		status.SetStatus(Language.GetTextValue("tModLoader.Packaging", mod));
		status.SetProgress(0, 1);
		mod.modFile.AddFile("Info", mod.properties.ToBytes());
		List<string> resources = (from res in Directory.GetFiles(mod.path, "*", SearchOption.AllDirectories)
			where !IgnoreResource(mod, res)
			select res).ToList();
		status.SetProgress(packedResourceCount = 0, resources.Count);
		Parallel.ForEach(resources, delegate(string resource)
		{
			AddResource(mod, resource);
		});
		string libFolder = Path.Combine(mod.path, "lib");
		foreach (string dllPath in mod.properties.dllReferences.Select((string dllName) => DllRefPath(mod, dllName)))
		{
			if (!dllPath.StartsWith(libFolder))
			{
				mod.modFile.AddFile("lib/" + Path.GetFileName(dllPath), File.ReadAllBytes(dllPath));
			}
		}
	}

	private bool IgnoreResource(BuildingMod mod, string resource)
	{
		string relPath = resource.Substring(mod.path.Length + 1);
		if (!IgnoreCompletely(mod, resource) && !(relPath == "build.txt") && (mod.properties.includeSource || !sourceExtensions.Contains(Path.GetExtension(resource))))
		{
			return Path.GetFileName(resource) == "Thumbs.db";
		}
		return true;
	}

	private bool IgnoreCompletely(BuildingMod mod, string resource)
	{
		string relPath = resource.Substring(mod.path.Length + 1);
		if (!mod.properties.ignoreFile(relPath) && relPath[0] != '.' && !relPath.StartsWith("bin" + Path.DirectorySeparatorChar))
		{
			return relPath.StartsWith("obj" + Path.DirectorySeparatorChar);
		}
		return true;
	}

	private void AddResource(BuildingMod mod, string resource)
	{
		string relPath = resource.Substring(mod.path.Length + 1);
		using FileStream src = File.OpenRead(resource);
		using MemoryStream dst = new MemoryStream();
		if (!ContentConverters.Convert(ref relPath, src, dst))
		{
			src.CopyTo(dst);
		}
		mod.modFile.AddFile(relPath, dst.ToArray());
		Interlocked.Increment(ref packedResourceCount);
		status.SetProgress(packedResourceCount);
	}

	private List<LocalMod> FindReferencedMods(BuildProperties properties)
	{
		Dictionary<string, LocalMod> existingMods = ModOrganizer.FindMods().ToDictionary((LocalMod mod) => mod.modFile.Name, (LocalMod mod) => mod);
		Dictionary<string, LocalMod> mods = new Dictionary<string, LocalMod>();
		FindReferencedMods(properties, existingMods, mods, requireWeak: true);
		return mods.Values.ToList();
	}

	private void FindReferencedMods(BuildProperties properties, Dictionary<string, LocalMod> existingMods, Dictionary<string, LocalMod> mods, bool requireWeak)
	{
		foreach (string refName in properties.RefNames(includeWeak: true))
		{
			if (mods.ContainsKey(refName))
			{
				continue;
			}
			bool isWeak = properties.weakReferences.Any((BuildProperties.ModReference r) => r.mod == refName);
			LocalMod mod;
			try
			{
				if (!existingMods.TryGetValue(refName, out mod))
				{
					throw new FileNotFoundException("Could not find \"" + refName + ".tmod\" in your subscribed Workshop mods nor the Mods folder");
				}
			}
			catch (FileNotFoundException) when (isWeak && !requireWeak)
			{
				continue;
			}
			catch (Exception ex)
			{
				throw new BuildException(Language.GetTextValue("tModLoader.BuildErrorModReference", refName), ex);
			}
			mods[refName] = mod;
			FindReferencedMods(mod.properties, existingMods, mods, requireWeak: false);
		}
	}

	private void BuildMod(BuildingMod mod, out byte[] code, out byte[] pdb)
	{
		string dllName = mod.Name + ".dll";
		string dllPath = null;
		string eacValue;
		if (mod.properties.noCompile)
		{
			dllPath = Path.Combine(mod.path, dllName);
		}
		else if (Program.LaunchParameters.TryGetValue("-eac", out eacValue))
		{
			dllPath = eacValue;
			mod.properties.eacPath = pdbPath();
			status.SetStatus(Language.GetTextValue("tModLoader.EnabledEAC", mod.properties.eacPath));
		}
		if (dllPath != null)
		{
			if (!File.Exists(dllPath))
			{
				throw new BuildException(Language.GetTextValue("tModLoader.BuildErrorLoadingPrecompiled", dllPath));
			}
			status.SetStatus(Language.GetTextValue("tModLoader.LoadingPrecompiled", dllName, Path.GetFileName(dllPath)));
			code = File.ReadAllBytes(dllPath);
			pdb = (File.Exists(pdbPath()) ? File.ReadAllBytes(pdbPath()) : null);
		}
		else
		{
			CompileMod(mod, out code, out pdb);
		}
		string pdbPath()
		{
			return Path.ChangeExtension(dllPath, "pdb");
		}
	}

	private void CompileMod(BuildingMod mod, out byte[] code, out byte[] pdb)
	{
		status.SetStatus(Language.GetTextValue("tModLoader.Compiling", mod.Name + ".dll"));
		string tempDir = Path.Combine(mod.path, "compile_temp");
		if (Directory.Exists(tempDir))
		{
			Directory.Delete(tempDir, recursive: true);
		}
		Directory.CreateDirectory(tempDir);
		List<string> refs = new List<string>();
		refs.AddRange(GetTerrariaReferences());
		refs.AddRange(mod.properties.dllReferences.Select((string dllName) => DllRefPath(mod, dllName)));
		foreach (LocalMod refMod in FindReferencedMods(mod.properties))
		{
			using (refMod.modFile.Open())
			{
				string path = Path.Combine(tempDir, refMod?.ToString() + ".dll");
				File.WriteAllBytes(path, refMod.modFile.GetModAssembly());
				refs.Add(path);
				string[] dllReferences = refMod.properties.dllReferences;
				foreach (string refDll in dllReferences)
				{
					path = Path.Combine(tempDir, refDll + ".dll");
					File.WriteAllBytes(path, refMod.modFile.GetBytes("lib/" + refDll + ".dll"));
					refs.Add(path);
				}
			}
		}
		string[] files = (from file in Directory.GetFiles(mod.path, "*.cs", SearchOption.AllDirectories)
			where !IgnoreCompletely(mod, file)
			select file).ToArray();
		string unsafeParam;
		bool _allowUnsafe = default(bool);
		bool allowUnsafe = Program.LaunchParameters.TryGetValue("-unsafe", out unsafeParam) && bool.TryParse(unsafeParam, out _allowUnsafe) && _allowUnsafe;
		List<string> preprocessorSymbols = new List<string> { "FNA" };
		if (Program.LaunchParameters.TryGetValue("-define", out var defineParam))
		{
			preprocessorSymbols.AddRange(defineParam.Split(';', ' '));
		}
		if (BuildInfo.IsStable)
		{
			string tmlVersionPreprocessorSymbol = $"TML_{BuildInfo.tMLVersion.Major}_{BuildInfo.tMLVersion.Minor:D2}";
			preprocessorSymbols.Add(tmlVersionPreprocessorSymbol);
		}
		Diagnostic[] results = RoslynCompile(mod.Name, refs, files, preprocessorSymbols.ToArray(), allowUnsafe, out code, out pdb);
		int numWarnings = results.Count((Diagnostic e) => e.Severity == DiagnosticSeverity.Warning);
		int numErrors = results.Length - numWarnings;
		status.LogCompilerLine(Language.GetTextValue("tModLoader.CompilationResult", numErrors, numWarnings), Level.Info);
		Diagnostic[] array = results;
		foreach (Diagnostic line in array)
		{
			status.LogCompilerLine(line.ToString(), (line.Severity == DiagnosticSeverity.Warning) ? Level.Warn : Level.Error);
		}
		try
		{
			if (Directory.Exists(tempDir))
			{
				Directory.Delete(tempDir, recursive: true);
			}
		}
		catch (Exception)
		{
		}
		if (numErrors > 0)
		{
			Diagnostic firstError = results.First((Diagnostic e) => e.Severity == DiagnosticSeverity.Error);
			throw new BuildException(Language.GetTextValue("tModLoader.CompileError", mod.Name + ".dll", numErrors, numWarnings) + $"\nError: {firstError}");
		}
	}

	private string DllRefPath(BuildingMod mod, string dllName)
	{
		string path = Path.Combine(mod.path, "lib", dllName) + ".dll";
		if (File.Exists(path))
		{
			return path;
		}
		if (Program.LaunchParameters.TryGetValue("-eac", out var eacPath))
		{
			string outputCopiedPath = Path.Combine(Path.GetDirectoryName(eacPath), dllName + ".dll");
			if (File.Exists(outputCopiedPath))
			{
				return outputCopiedPath;
			}
		}
		throw new BuildException("Missing dll reference: " + path);
	}

	private static IEnumerable<string> GetTerrariaReferences()
	{
		Assembly executingAssembly = Assembly.GetExecutingAssembly();
		yield return executingAssembly.Location;
		string libsDir = Path.Combine(Path.GetDirectoryName(executingAssembly.Location), "Libraries");
		foreach (string f in Directory.EnumerateFiles(libsDir, "*.dll", SearchOption.AllDirectories))
		{
			string path = f.Replace('\\', '/');
			if (!path.EndsWith(".resources.dll") && !path.Contains("/Native/") && !path.Contains("/runtime"))
			{
				yield return f;
			}
		}
	}

	/// <summary>
	/// Compile a dll for the mod based on required includes.
	/// </summary>
	private static Diagnostic[] RoslynCompile(string name, List<string> references, string[] files, string[] preprocessorSymbols, bool allowUnsafe, out byte[] code, out byte[] pdb)
	{
		CSharpCompilationOptions options = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary, reportSuppressedDiagnostics: false, null, null, null, null, assemblyIdentityComparer: DesktopAssemblyIdentityComparer.Default, optimizationLevel: (!preprocessorSymbols.Contains("DEBUG")) ? OptimizationLevel.Release : OptimizationLevel.Debug, checkOverflow: false, allowUnsafe: allowUnsafe);
		CSharpParseOptions parseOptions = new CSharpParseOptions(LanguageVersion.Preview, DocumentationMode.Parse, SourceCodeKind.Regular, preprocessorSymbols);
		EmitOptions emitOptions = new EmitOptions(metadataOnly: false, DebugInformationFormat.PortablePdb, null, null, 0, 0uL);
		IEnumerable<PortableExecutableReference> refs = references.Select((string s) => MetadataReference.CreateFromFile(s));
		refs = refs.Concat(Net60.All);
		IEnumerable<SyntaxTree> src = files.Select((string f) => SyntaxFactory.ParseSyntaxTree(File.ReadAllText(f), parseOptions, f, Encoding.UTF8));
		CSharpCompilation comp = CSharpCompilation.Create(name, src, refs, options);
		using MemoryStream peStream = new MemoryStream();
		using MemoryStream pdbStream = new MemoryStream();
		EmitResult emitResult = comp.Emit(peStream, pdbStream, null, null, null, emitOptions);
		code = peStream.ToArray();
		pdb = pdbStream.ToArray();
		return emitResult.Diagnostics.Where((Diagnostic d) => d.Severity >= DiagnosticSeverity.Warning).ToArray();
	}
}
