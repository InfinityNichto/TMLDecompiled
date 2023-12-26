using System;
using System.Linq;
using System.Reflection;

namespace Terraria.ModLoader.Core;

internal static class AssemblyResolving
{
	private static bool init;

	public static void Init()
	{
		if (init)
		{
			return;
		}
		init = true;
		AssemblyResolveEarly(delegate(object? _, ResolveEventArgs args)
		{
			AssemblyName name = new AssemblyName(args.Name);
			if (Array.Find(typeof(Program).Assembly.GetManifestResourceNames(), (string s) => s.EndsWith(name.Name + ".dll")) == null)
			{
				return (Assembly?)null;
			}
			Assembly assembly = AppDomain.CurrentDomain.GetAssemblies().SingleOrDefault((Assembly a) => a.GetName().Name == name.Name);
			if (assembly != null)
			{
				Logging.tML.Warn((object)$"Upgraded Reference {name.Name} -> Version={name.Version} -> {assembly.GetName().Version}");
				return assembly;
			}
			return (Assembly?)null;
		});
		AssemblyResolveEarly(delegate(object? _, ResolveEventArgs args)
		{
			Logging.tML.DebugFormat("Assembly Resolve: {0} -> {1}", (object)args.RequestingAssembly, (object)args.Name);
			return (Assembly?)null;
		});
	}

	internal static void AssemblyResolveEarly(ResolveEventHandler handler)
	{
		FieldInfo fieldInfo = typeof(AppDomain).GetFields((BindingFlags)(-1)).First((FieldInfo f) => f.Name.EndsWith("AssemblyResolve"));
		ResolveEventHandler a = (ResolveEventHandler)fieldInfo.GetValue(AppDomain.CurrentDomain);
		fieldInfo.SetValue(AppDomain.CurrentDomain, null);
		AppDomain.CurrentDomain.AssemblyResolve += handler;
		AppDomain.CurrentDomain.AssemblyResolve += a;
	}
}
