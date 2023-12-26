using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace Terraria.ModLoader;

/// <summary>
/// This serves as the central class from which ModCommand functions are supported and carried out.
/// </summary>
public static class CommandLoader
{
	internal static readonly IDictionary<string, List<ModCommand>> Commands = new Dictionary<string, List<ModCommand>>(StringComparer.OrdinalIgnoreCase);

	public static bool Matches(CommandType commandType, CommandType callerType)
	{
		if ((commandType & CommandType.World) != 0)
		{
			if (Main.netMode == 2)
			{
				commandType |= CommandType.Server;
			}
			else if (Main.netMode == 0)
			{
				commandType |= CommandType.Chat;
			}
		}
		return (callerType & commandType) != 0;
	}

	internal static void Add(ModCommand cmd)
	{
		if (!Commands.TryGetValue(cmd.Command, out var cmdList))
		{
			Commands.Add(cmd.Command, cmdList = new List<ModCommand>());
		}
		cmdList.Add(cmd);
	}

	internal static void Unload()
	{
		Commands.Clear();
	}

	/// <summary>
	/// Finds a command by name. Handles mod prefixing. Replies with error messages.
	/// </summary>
	/// <param name="caller">Handles relaying the results of the command and narrows down the search by CommandType context</param>
	/// <param name="name">The name of the command to retrieve</param>
	/// <param name="mc">The found command, or null if an error was encountered.</param>
	/// <returns>True if a ModCommand was found, or an error message was replied. False if the command is unrecognized.</returns>
	internal static bool GetCommand(CommandCaller caller, string name, out ModCommand mc)
	{
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		string modName = null;
		if (name.Contains(':'))
		{
			string[] array = name.Split(':');
			modName = array[0];
			name = array[1];
		}
		mc = null;
		if (!Commands.TryGetValue(name, out var cmdList))
		{
			return false;
		}
		cmdList = cmdList.Where((ModCommand c) => Matches(c.Type, caller.CommandType)).ToList();
		if (cmdList.Count == 0)
		{
			return false;
		}
		if (modName != null)
		{
			if (!ModLoader.TryGetMod(modName, out var mod))
			{
				caller.Reply("Unknown Mod: " + modName, Color.Red);
			}
			else
			{
				mc = cmdList.SingleOrDefault((ModCommand c) => c.Mod == mod);
				if (mc == null)
				{
					caller.Reply("Mod: " + modName + " does not have a " + name + " command.", Color.Red);
				}
			}
		}
		else if (cmdList.Count > 1)
		{
			caller.Reply("Multiple definitions of command /" + name + ". Try:", Color.Red);
			foreach (ModCommand c2 in cmdList)
			{
				caller.Reply(c2.Mod.Name + ":" + c2.Command, Color.LawnGreen);
			}
		}
		else
		{
			mc = cmdList[0];
		}
		return true;
	}

	internal static bool HandleCommand(string input, CommandCaller caller)
	{
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		string[] args = input.TrimEnd().Split(' ');
		string name = args[0];
		args = args.Skip(1).ToArray();
		if (caller.CommandType != CommandType.Console)
		{
			if (name == "" || name[0] != '/')
			{
				return false;
			}
			name = name.Substring(1);
		}
		if (!GetCommand(caller, name, out var mc))
		{
			return false;
		}
		if (mc == null)
		{
			return true;
		}
		try
		{
			mc.Action(caller, input, args);
		}
		catch (Exception ex)
		{
			if (ex is UsageException { msg: not null } ue)
			{
				caller.Reply(ue.msg, ue.color);
			}
			else
			{
				caller.Reply("Usage: " + mc.Usage, Color.Red);
			}
		}
		return true;
	}

	public static List<Tuple<string, string>> GetHelp(CommandType type)
	{
		List<Tuple<string, string>> list = new List<Tuple<string, string>>();
		foreach (KeyValuePair<string, List<ModCommand>> command in Commands)
		{
			List<ModCommand> cmdList = command.Value.Where((ModCommand mc) => Matches(mc.Type, type)).ToList();
			foreach (ModCommand mc2 in cmdList)
			{
				string cmd = mc2.Command;
				if (cmdList.Count > 1)
				{
					cmd = mc2.Mod.Name + ":" + cmd;
				}
				list.Add(Tuple.Create(cmd, mc2.Description));
			}
		}
		return list;
	}
}
