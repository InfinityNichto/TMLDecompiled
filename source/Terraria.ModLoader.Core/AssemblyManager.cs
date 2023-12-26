using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Loader;
using System.Threading;
using log4net;
using Microsoft.Xna.Framework;
using Terraria.Localization;
using Terraria.ModLoader.Exceptions;
using Terraria.ModLoader.UI;

namespace Terraria.ModLoader.Core;

public static class AssemblyManager
{
	private class ModLoadContext : AssemblyLoadContext
	{
		private class MetadataResolver : MetadataAssemblyResolver
		{
			private readonly ModLoadContext mod;

			public MetadataResolver(ModLoadContext mod)
			{
				this.mod = mod;
			}

			public override Assembly Resolve(MetadataLoadContext context, AssemblyName assemblyName)
			{
				Assembly existing = context.GetAssemblies().SingleOrDefault((Assembly a) => a.GetName().FullName == assemblyName.FullName);
				if (existing != null)
				{
					return existing;
				}
				Assembly runtime = mod.LoadFromAssemblyName(assemblyName);
				if (string.IsNullOrEmpty(runtime.Location))
				{
					return context.LoadFromByteArray(((ModLoadContext)AssemblyLoadContext.GetLoadContext(runtime)).assemblyBytes[assemblyName.Name]);
				}
				return context.LoadFromAssemblyPath(runtime.Location);
			}
		}

		public readonly TmodFile modFile;

		public readonly BuildProperties properties;

		public List<ModLoadContext> dependencies = new List<ModLoadContext>();

		public Assembly assembly;

		public IDictionary<string, Assembly> assemblies = new Dictionary<string, Assembly>();

		public IDictionary<string, byte[]> assemblyBytes = new Dictionary<string, byte[]>();

		public IDictionary<Assembly, Type[]> loadableTypes = new Dictionary<Assembly, Type[]>();

		public long bytesLoaded;

		public ModLoadContext(LocalMod mod)
			: base(mod.Name, isCollectible: true)
		{
			modFile = mod.modFile;
			properties = mod.properties;
			base.Unloading += ModLoadContext_Unloading;
		}

		private void ModLoadContext_Unloading(AssemblyLoadContext obj)
		{
			dependencies = null;
			assembly = null;
			assemblies = null;
			loadableTypes = null;
		}

		public void AddDependency(ModLoadContext dep)
		{
			dependencies.Add(dep);
		}

		public void LoadAssemblies()
		{
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Expected O, but got Unknown
			try
			{
				using (modFile.Open())
				{
					string[] dllReferences = properties.dllReferences;
					foreach (string dll in dllReferences)
					{
						LoadAssembly(modFile.GetBytes("lib/" + dll + ".dll"));
					}
					assembly = ((Debugger.IsAttached && File.Exists(properties.eacPath)) ? LoadAssembly(modFile.GetModAssembly(), File.ReadAllBytes(properties.eacPath)) : LoadAssembly(modFile.GetModAssembly(), modFile.GetModPdb()));
				}
				MetadataLoadContext mlc = new MetadataLoadContext((MetadataAssemblyResolver)(object)new MetadataResolver(this), (string)null);
				loadableTypes = GetLoadableTypes(this, mlc);
			}
			catch (Exception ex)
			{
				ex.Data["mod"] = base.Name;
				throw;
			}
		}

		private Assembly LoadAssembly(byte[] code, byte[] pdb = null)
		{
			using MemoryStream codeStrm = new MemoryStream(code, writable: false);
			using MemoryStream pdbStrm = ((pdb == null) ? null : new MemoryStream(pdb, writable: false));
			Assembly asm = LoadFromStream(codeStrm, pdbStrm);
			string name = asm.GetName().Name;
			assemblyBytes[name] = code;
			assemblies[name] = asm;
			bytesLoaded += code.LongLength + ((pdb != null) ? pdb.LongLength : 0);
			if (Program.LaunchParameters.ContainsKey("-dumpasm"))
			{
				string dumpdir = Path.Combine(Main.SavePath, "asmdump");
				Directory.CreateDirectory(dumpdir);
				File.WriteAllBytes(Path.Combine(dumpdir, asm.FullName + ".dll"), code);
				if (pdb != null)
				{
					File.WriteAllBytes(Path.Combine(dumpdir, asm.FullName + ".pdb"), code);
				}
			}
			return asm;
		}

		protected override Assembly Load(AssemblyName assemblyName)
		{
			if (assemblies.TryGetValue(assemblyName.Name, out var asm))
			{
				return asm;
			}
			return dependencies.Select((ModLoadContext dep) => dep.Load(assemblyName)).FirstOrDefault((Assembly a) => a != null);
		}

		internal bool IsModDependencyPresent(string name)
		{
			if (!(name == base.Name))
			{
				return dependencies.Any((ModLoadContext d) => d.IsModDependencyPresent(name));
			}
			return true;
		}

		internal void ClearAssemblyBytes()
		{
			assemblyBytes.Clear();
		}
	}

	private static readonly List<WeakReference<AssemblyLoadContext>> oldLoadContexts = new List<WeakReference<AssemblyLoadContext>>();

	private static readonly Dictionary<string, ModLoadContext> loadedModContexts = new Dictionary<string, ModLoadContext>();

	private static bool assemblyResolverAdded;

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static void Unload()
	{
		foreach (ModLoadContext alc in loadedModContexts.Values)
		{
			oldLoadContexts.Add(new WeakReference<AssemblyLoadContext>(alc));
			alc.Unload();
		}
		loadedModContexts.Clear();
		for (int i = 0; i < 10; i++)
		{
			GC.Collect();
			GC.WaitForPendingFinalizers();
		}
	}

	internal static IEnumerable<string> OldLoadContexts()
	{
		foreach (WeakReference<AssemblyLoadContext> oldLoadContext in oldLoadContexts)
		{
			if (oldLoadContext.TryGetTarget(out var alc))
			{
				yield return alc.Name;
			}
		}
	}

	private static void AddAssemblyResolver()
	{
		if (!assemblyResolverAdded)
		{
			assemblyResolverAdded = true;
			AppDomain.CurrentDomain.AssemblyResolve += TmlCustomResolver;
		}
	}

	private static Assembly TmlCustomResolver(object sender, ResolveEventArgs args)
	{
		string name = new AssemblyName(args.Name).Name;
		if (name.Contains("tModLoader") || name == "Terraria")
		{
			return Assembly.GetExecutingAssembly();
		}
		if (name == "FNA")
		{
			return typeof(Vector2).Assembly;
		}
		return null;
	}

	private static Mod Instantiate(ModLoadContext mod)
	{
		try
		{
			VerifyMod(mod.Name, mod.assembly, out var modType);
			Mod obj = (Mod)Activator.CreateInstance(modType, nonPublic: true);
			obj.File = mod.modFile;
			obj.Code = mod.assembly;
			obj.Logger = LogManager.GetLogger(obj.Name);
			obj.Side = mod.properties.side;
			obj.DisplayName = mod.properties.displayName;
			obj.TModLoaderVersion = mod.properties.buildVersion;
			obj.TranslationForMods = (mod.properties.translationMod ? mod.properties.RefNames(includeWeak: true).ToList() : null);
			return obj;
		}
		catch (Exception ex)
		{
			ex.Data["mod"] = mod.Name;
			throw;
		}
		finally
		{
			MemoryTracking.Update(mod.Name).code += mod.bytesLoaded;
		}
	}

	private static void VerifyMod(string modName, Assembly assembly, out Type modType)
	{
		string asmName = new AssemblyName(assembly.FullName).Name;
		if (asmName != modName)
		{
			throw new Exception(Language.GetTextValue("tModLoader.BuildErrorModNameDoesntMatchAssemblyName", modName, asmName));
		}
		if (!GetLoadableTypes(assembly).Any((Type t) => t.Namespace?.StartsWith(modName) ?? false))
		{
			throw new Exception(Language.GetTextValue("tModLoader.BuildErrorNamespaceFolderDontMatch"));
		}
		Type[] modTypes = (from t in GetLoadableTypes(assembly)
			where t.IsSubclassOf(typeof(Mod))
			select t).ToArray();
		if (modTypes.Length > 1)
		{
			throw new Exception(modName + " has multiple classes extending Mod. Only one Mod per mod is supported at the moment");
		}
		modType = modTypes.SingleOrDefault() ?? typeof(Mod);
	}

	internal static List<Mod> InstantiateMods(List<LocalMod> modsToLoad, CancellationToken token)
	{
		AddAssemblyResolver();
		List<ModLoadContext> modList = modsToLoad.Select((LocalMod m) => new ModLoadContext(m)).ToList();
		foreach (ModLoadContext mod4 in modList)
		{
			loadedModContexts.Add(mod4.Name, mod4);
		}
		foreach (ModLoadContext mod3 in modList)
		{
			foreach (string depName in mod3.properties.RefNames(includeWeak: true))
			{
				if (loadedModContexts.TryGetValue(depName, out var dep))
				{
					mod3.AddDependency(dep);
				}
			}
		}
		if (Debugger.IsAttached)
		{
			ModCompile.activelyModding = true;
		}
		try
		{
			Interface.loadMods.SetLoadStage("tModLoader.MSSandboxing", modsToLoad.Count);
			int i = 0;
			foreach (ModLoadContext mod2 in modList)
			{
				token.ThrowIfCancellationRequested();
				Interface.loadMods.SetCurrentMod(i++, mod2.Name, mod2.properties?.displayName ?? "", mod2.modFile.Version);
				mod2.LoadAssemblies();
			}
			foreach (ModLoadContext item in modList)
			{
				item.ClearAssemblyBytes();
			}
			Interface.loadMods.SetLoadStage("tModLoader.MSInstantiating");
			MemoryTracking.Checkpoint();
			return modList.Select(delegate(ModLoadContext mod)
			{
				token.ThrowIfCancellationRequested();
				return Instantiate(mod);
			}).ToList();
		}
		catch (AggregateException ae)
		{
			ae.Data["mods"] = ae.InnerExceptions.Select((Exception e) => (string)e.Data["mod"]).ToArray();
			throw;
		}
	}

	private static string GetModAssemblyFileName(this TmodFile modFile)
	{
		return modFile.Name + ".dll";
	}

	public static byte[] GetModAssembly(this TmodFile modFile)
	{
		return modFile.GetBytes(modFile.GetModAssemblyFileName());
	}

	public static byte[] GetModPdb(this TmodFile modFile)
	{
		return modFile.GetBytes(Path.ChangeExtension(modFile.GetModAssemblyFileName(), "pdb"));
	}

	private static ModLoadContext GetLoadContext(string name)
	{
		if (!loadedModContexts.TryGetValue(name, out var value))
		{
			throw new KeyNotFoundException(name);
		}
		return value;
	}

	public static IEnumerable<Assembly> GetModAssemblies(string name)
	{
		return GetLoadContext(name).assemblies.Values;
	}

	public static bool GetAssemblyOwner(Assembly assembly, out string modName)
	{
		modName = null;
		if (!(AssemblyLoadContext.GetLoadContext(assembly) is ModLoadContext mlc))
		{
			return false;
		}
		modName = mlc.Name;
		if (loadedModContexts[modName] != mlc)
		{
			throw new Exception("Attempt to retrieve owner for mod assembly from a previous load");
		}
		return true;
	}

	internal static bool FirstModInStackTrace(StackTrace stack, out string modName)
	{
		for (int i = 0; i < stack.FrameCount; i++)
		{
			Assembly assembly = stack.GetFrame(i).GetMethod()?.DeclaringType?.Assembly;
			if (assembly != null && GetAssemblyOwner(assembly, out modName))
			{
				return true;
			}
		}
		modName = null;
		return false;
	}

	public static IEnumerable<Mod> GetDependencies(Mod mod)
	{
		return GetLoadContext(mod.Name).dependencies.Select((ModLoadContext m) => ModLoader.GetMod(mod.Name));
	}

	public static Type[] GetLoadableTypes(Assembly assembly)
	{
		if (!(AssemblyLoadContext.GetLoadContext(assembly) is ModLoadContext mlc))
		{
			return assembly.GetTypes();
		}
		return mlc.loadableTypes[assembly];
	}

	private static IDictionary<Assembly, Type[]> GetLoadableTypes(ModLoadContext mod, MetadataLoadContext mlc)
	{
		try
		{
			return mod.Assemblies.ToDictionary((Assembly a) => a, (Assembly asm) => (from mType in mlc.LoadFromAssemblyName(asm.GetName()).GetTypes()
				where IsLoadable(mod, mType)
				select asm.GetType(mType.FullName, throwOnError: true, ignoreCase: false)).ToArray());
		}
		catch (Exception e)
		{
			throw new GetLoadableTypesException("This mod seems to inherit from classes in another mod. Use the [ExtendsFromMod] attribute to allow this mod to load when that mod is not enabled.\n" + e.Message, e);
		}
	}

	private static bool IsLoadable(ModLoadContext mod, Type type)
	{
		foreach (CustomAttributeData attr in type.GetCustomAttributesData())
		{
			if (attr.AttributeType.AssemblyQualifiedName == typeof(ExtendsFromModAttribute).AssemblyQualifiedName && !((IEnumerable<CustomAttributeTypedArgument>)attr.ConstructorArguments[0].Value).All((CustomAttributeTypedArgument v) => mod.IsModDependencyPresent((string)v.Value)))
			{
				return false;
			}
		}
		if (type.BaseType != null && !IsLoadable(mod, type.BaseType))
		{
			return false;
		}
		return type.GetInterfaces().All((Type i) => IsLoadable(mod, i));
	}

	internal static void JITMod(Mod mod)
	{
		JITAssemblies(GetModAssemblies(mod.Name), mod.PreJITFilter);
	}

	public static void JITAssemblies(IEnumerable<Assembly> assemblies, PreJITFilter filter)
	{
		ConcurrentQueue<(Exception exception, MethodBase method)> exceptions = new ConcurrentQueue<(Exception, MethodBase)>();
		foreach (Assembly assembly in assemblies)
		{
			MethodBase[] methodsToJIT = GetLoadableTypes(assembly).Where(filter.ShouldJIT).SelectMany((Type type) => (from m in ((IEnumerable<MethodBase>)(from m in type.GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
					where !m.IsSpecialName
					select m)).Concat((IEnumerable<MethodBase>)type.GetConstructors(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)).Concat(type.GetProperties(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).Where(filter.ShouldJIT).SelectMany((PropertyInfo p) => p.GetAccessors()))
				where !m.IsAbstract && !m.ContainsGenericParameters && m.DeclaringType == type
				select m).Where(filter.ShouldJIT)).ToArray();
			if (Environment.ProcessorCount > 1)
			{
				methodsToJIT.AsParallel().AsUnordered().ForAll(delegate(MethodBase method)
				{
					try
					{
						ForceJITOnMethod(method);
					}
					catch (Exception item)
					{
						exceptions.Enqueue((item, method));
					}
				});
				continue;
			}
			MethodBase[] array = methodsToJIT;
			foreach (MethodBase method2 in array)
			{
				try
				{
					ForceJITOnMethod(method2);
				}
				catch (Exception e)
				{
					exceptions.Enqueue((e, method2));
				}
			}
		}
		if (exceptions.Count > 0)
		{
			throw new JITException("\n" + string.Join("\n", exceptions.Select(((Exception exception, MethodBase method) x) => $"In {x.method.DeclaringType.FullName}.{x.method.Name}, {x.exception.Message}")) + "\n");
		}
	}

	private static void ForceJITOnMethod(MethodBase method)
	{
		if (method.GetMethodBody() != null)
		{
			RuntimeHelpers.PrepareMethod(method.MethodHandle);
		}
		if (!(method is MethodInfo methodInfo))
		{
			return;
		}
		bool isNewSlot = (methodInfo.Attributes & MethodAttributes.VtableLayoutMask) == MethodAttributes.VtableLayoutMask;
		if (methodInfo.IsVirtual && !isNewSlot)
		{
			bool? flag = methodInfo.DeclaringType?.IsInterface;
			if (flag.HasValue && !flag.GetValueOrDefault() && methodInfo.GetBaseDefinition() == methodInfo)
			{
				throw new Exception($"{method} overrides a method which doesn't exist in any base class");
			}
		}
	}
}
