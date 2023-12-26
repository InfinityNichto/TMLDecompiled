using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Mono.Cecil;
using MonoMod.Cil;
using MonoMod.RuntimeDetour;
using MonoMod.RuntimeDetour.HookGen;
using MonoMod.Utils;
using Terraria.ModLoader.Core;
using Terraria.ModLoader.Default;

namespace Terraria.ModLoader;

public static class MonoModHooks
{
	private class DetourList
	{
		public readonly List<DetourInfo> detours = new List<DetourInfo>();

		public readonly List<ILHookInfo> ilHooks = new List<ILHookInfo>();
	}

	private static Dictionary<Type, string> defaultAliases = new Dictionary<Type, string>
	{
		{
			typeof(object),
			"object"
		},
		{
			typeof(bool),
			"bool"
		},
		{
			typeof(float),
			"float"
		},
		{
			typeof(double),
			"double"
		},
		{
			typeof(decimal),
			"decimal"
		},
		{
			typeof(byte),
			"byte"
		},
		{
			typeof(sbyte),
			"sbyte"
		},
		{
			typeof(short),
			"short"
		},
		{
			typeof(ushort),
			"ushort"
		},
		{
			typeof(int),
			"int"
		},
		{
			typeof(uint),
			"uint"
		},
		{
			typeof(long),
			"long"
		},
		{
			typeof(ulong),
			"ulong"
		},
		{
			typeof(char),
			"char"
		},
		{
			typeof(string),
			"string"
		}
	};

	private static Dictionary<Assembly, DetourList> assemblyDetours = new Dictionary<Assembly, DetourList>();

	private static bool isInitialized;

	private static ConcurrentDictionary<(MethodBase, Delegate), IDisposable> _hookCache = new ConcurrentDictionary<(MethodBase, Delegate), IDisposable>();

	private const string HookAlreadyAppliedMsg = "Delegate has already been applied to this method as a hook!";

	private static DetourList GetDetourList(Assembly asm)
	{
		if (asm == typeof(Action).Assembly)
		{
			throw new ArgumentException("Cannot identify owning assembly of hook. Make sure there are no delegate type changing wrappers between the method/lambda and the Modify/Add/+= call. Eg `new ILContext.Manipulator(action)` is bad");
		}
		if (!assemblyDetours.TryGetValue(asm, out var list))
		{
			return assemblyDetours[asm] = new DetourList();
		}
		return list;
	}

	[Obsolete("No longer required. NativeDetour is gone. Detour should not be used. Hook is safe to use", true)]
	public static void RequestNativeAccess()
	{
	}

	internal static void Initialize()
	{
		if (isInitialized)
		{
			return;
		}
		DetourManager.DetourApplied += delegate(DetourInfo info)
		{
			Assembly assembly2 = info.Entry.DeclaringType.Assembly;
			GetDetourList(assembly2).detours.Add(info);
			string text = "Hook " + StringRep(((DetourBase)info).Method.Method) + " added by " + assembly2.GetName().Name;
			MethodSignature val = MethodSignature.ForMethod(((DetourBase)info).Method.Method);
			MethodSignature val2 = MethodSignature.ForMethod(info.Entry, true);
			if (val2.ParameterCount != val.ParameterCount + 1 || (object)val2.FirstParameter.GetMethod("Invoke") == null)
			{
				text += " WARNING! No orig delegate, incompatible with other hooks to this method";
			}
			Logging.tML.Debug((object)text);
		};
		DetourManager.ILHookApplied += delegate(ILHookInfo info)
		{
			Assembly assembly = info.ManipulatorMethod.DeclaringType.Assembly;
			GetDetourList(assembly).ilHooks.Add(info);
			Logging.tML.Debug((object)("ILHook " + StringRep(((DetourBase)info).Method.Method) + " added by " + assembly.GetName().Name));
		};
		isInitialized = true;
	}

	private static string StringRep(MethodBase m)
	{
		string paramString = string.Join(", ", m.GetParameters().Select(delegate(ParameterInfo p)
		{
			Type type = p.ParameterType;
			string text = "";
			if (type.IsByRef)
			{
				text = (p.IsOut ? "out " : "ref ");
				type = type.GetElementType();
			}
			string value;
			return text + (defaultAliases.TryGetValue(type, out value) ? value : type.Name);
		}));
		string owner = m.DeclaringType?.FullName ?? ((m is DynamicMethod) ? "dynamic" : "unknown");
		return $"{owner}::{m.Name}({paramString})";
	}

	internal static void RemoveAll(Mod mod)
	{
		if (mod is ModLoaderMod)
		{
			return;
		}
		foreach (Assembly asm in AssemblyManager.GetModAssemblies(mod.Name))
		{
			if (!assemblyDetours.TryGetValue(asm, out var list))
			{
				continue;
			}
			Logging.tML.Debug((object)$"Unloading {list.ilHooks.Count} IL hooks, {list.detours.Count} detours from {asm.GetName().Name} in {mod.DisplayName}");
			foreach (DetourInfo detour in list.detours)
			{
				if (((DetourBase)detour).IsApplied)
				{
					((DetourBase)detour).Undo();
				}
			}
			foreach (ILHookInfo ilHook in list.ilHooks)
			{
				if (((DetourBase)ilHook).IsApplied)
				{
					((DetourBase)ilHook).Undo();
				}
			}
		}
	}

	internal static void Clear()
	{
		HookEndpointManager.Clear();
		assemblyDetours.Clear();
		_hookCache.Clear();
	}

	/// <summary>
	/// Adds a hook (implemented by <paramref name="hookDelegate" />) to <paramref name="method" />.
	/// </summary>
	/// <param name="method">The method to hook.</param>
	/// <param name="hookDelegate">The hook delegate to use.</param>
	public static void Add(MethodBase method, Delegate hookDelegate)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Expected O, but got Unknown
		if (!_hookCache.TryAdd((method, hookDelegate), (IDisposable)new Hook(method, hookDelegate)))
		{
			throw new ArgumentException("Delegate has already been applied to this method as a hook!");
		}
	}

	public static void Modify(MethodBase method, Manipulator callback)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Expected O, but got Unknown
		if (!_hookCache.TryAdd((method, (Delegate)(object)callback), (IDisposable)new ILHook(method, callback)))
		{
			throw new ArgumentException("Delegate has already been applied to this method as a hook!");
		}
	}

	/// <summary>
	/// Dumps the list of currently registered IL hooks to the console. Useful for checking if a hook has been correctly added.
	/// </summary>
	/// <exception cref="T:System.Exception"></exception>
	public static void DumpILHooks()
	{
		object ilHooksFieldValue = typeof(HookEndpointManager).GetField("ILHooks", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null);
		if (ilHooksFieldValue is IReadOnlyDictionary<(MethodBase, Delegate), ILHook> ilHooks)
		{
			Logging.tML.Debug((object)"Dump of registered IL Hooks:");
			{
				foreach (KeyValuePair<(MethodBase, Delegate), ILHook> item in ilHooks)
				{
					Logging.tML.Debug((object)(item.Key.ToString() + ": " + (object)item.Value));
				}
				return;
			}
		}
		throw new Exception($"Failed to get HookEndpointManager.ILHooks: Type is {ilHooksFieldValue.GetType()}");
	}

	/// <summary>
	/// Dumps the list of currently registered On hooks to the console. Useful for checking if a hook has been correctly added.
	/// </summary>
	/// <exception cref="T:System.Exception"></exception>
	public static void DumpOnHooks()
	{
		object hooksFieldValue = typeof(HookEndpointManager).GetField("Hooks", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null);
		if (hooksFieldValue is IReadOnlyDictionary<(MethodBase, Delegate), Hook> detours)
		{
			Logging.tML.Debug((object)"Dump of registered Detours:");
			{
				foreach (KeyValuePair<(MethodBase, Delegate), Hook> item in detours)
				{
					Logging.tML.Debug((object)(item.Key.ToString() + ": " + (object)item.Value));
				}
				return;
			}
		}
		throw new Exception($"Failed to get HookEndpointManager.Hooks: Type is {hooksFieldValue.GetType()}");
	}

	/// <summary>
	/// Dumps the information about the given ILContext to a file in Logs/ILDumps/{Mod Name}/{Method Name}.txt<br />
	/// It may be useful to use a tool such as <see href="https://www.diffchecker.com/" /> to compare the IL before and after edits
	/// </summary>
	/// <param name="mod"></param>
	/// <param name="il"></param>
	public static void DumpIL(Mod mod, ILContext il)
	{
		string methodName = ((MemberReference)il.Method).FullName.Replace(':', '_');
		if (methodName.Contains('?'))
		{
			string text = methodName;
			int num = methodName.LastIndexOf('?') + 1;
			methodName = text.Substring(num, text.Length - num);
		}
		string filePath = Path.Combine(Logging.LogDir, "ILDumps", mod.Name, methodName + ".txt");
		string folderPath = Path.GetDirectoryName(filePath);
		if (!Directory.Exists(folderPath))
		{
			Directory.CreateDirectory(folderPath);
		}
		File.WriteAllText(filePath, ((object)il).ToString());
		Logging.tML.Debug((object)$"Dumped ILContext \"{((MemberReference)il.Method).FullName}\" to \"{filePath}\"");
	}
}
