using System;
using Mono.Cecil;
using MonoMod.Cil;

namespace Terraria.ModLoader;

public class ILPatchFailureException : Exception
{
	public ILPatchFailureException(Mod mod, ILContext il, Exception innerException)
		: base($"Mod \"{mod.Name}\" failed to IL edit method \"{((MemberReference)il.Method).FullName}\"", innerException)
	{
		MonoModHooks.DumpIL(mod, il);
	}
}
