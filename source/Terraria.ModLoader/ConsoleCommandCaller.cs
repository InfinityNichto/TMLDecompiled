using System;
using Microsoft.Xna.Framework;

namespace Terraria.ModLoader;

internal class ConsoleCommandCaller : CommandCaller
{
	public CommandType CommandType => CommandType.Console;

	public Player Player => null;

	public void Reply(string text, Color color = default(Color))
	{
		string[] array = text.Split('\n');
		for (int i = 0; i < array.Length; i++)
		{
			Console.WriteLine(array[i]);
		}
	}
}
