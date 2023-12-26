using System;

namespace Terraria.ModLoader.Config.UI;

internal class FuncStringWrapper
{
	public Func<string> Func { get; }

	public FuncStringWrapper(Func<string> func)
	{
		Func = func;
	}

	public override string ToString()
	{
		return Func();
	}
}
