using System;

namespace Terraria.ModLoader.Exceptions;

internal class ResourceLoadException : Exception
{
	public ResourceLoadException(string message, Exception inner = null)
		: base(message, inner)
	{
	}
}
