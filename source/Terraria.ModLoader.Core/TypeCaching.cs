using System;
using System.Collections.Generic;

namespace Terraria.ModLoader.Core;

internal static class TypeCaching
{
	private static HashSet<Type> _resetsRegistered = new HashSet<Type>();

	public static event Action OnClear;

	public static void Clear()
	{
		TypeCaching.OnClear?.Invoke();
	}

	internal static void ResetStaticMembersOnClear(Type type)
	{
		if (_resetsRegistered.Add(type))
		{
			OnClear += delegate
			{
				LoaderUtils.ResetStaticMembers(type);
			};
		}
	}
}
