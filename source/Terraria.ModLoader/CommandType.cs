using System;

namespace Terraria.ModLoader;

/// <summary>A flag enum representing context where this command operates.</summary>
[Flags]
public enum CommandType
{
	/// <summary>Command can be used in Chat in SP and MP.</summary>
	Chat = 1,
	/// <summary>Command is executed by server in MP.</summary>
	Server = 2,
	/// <summary>Command can be used in server console during MP.</summary>
	Console = 4,
	/// <summary>Command can be used in Chat in SP and MP, but executes on the Server in MP. (SinglePlayer ? Chat : Server)</summary>
	World = 8
}
