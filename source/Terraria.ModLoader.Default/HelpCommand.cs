using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.Chat;

namespace Terraria.ModLoader.Default;

internal class HelpCommand : ModCommand
{
	public override string Command => "help";

	public override string Usage => "/help [name]";

	public override CommandType Type => CommandType.Chat | CommandType.Server;

	public override void Action(CommandCaller caller, string input, string[] args)
	{
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		if (args.Length != 0)
		{
			if (!CommandLoader.GetCommand(caller, args[0], out var mc))
			{
				throw new UsageException("Unknown command: " + args[0], Color.Red);
			}
			if (mc != null)
			{
				caller.Reply(mc.Usage);
				if (!string.IsNullOrEmpty(mc.Description))
				{
					caller.Reply(mc.Description);
				}
			}
			return;
		}
		List<Tuple<string, string>> help = CommandLoader.GetHelp(caller.CommandType);
		caller.Reply(caller.CommandType.ToString() + " Commands:", Color.Yellow);
		foreach (Tuple<string, string> entry in help)
		{
			caller.Reply(entry.Item1 + "   " + entry.Item2);
		}
		if (Main.netMode == 1)
		{
			ChatHelper.SendChatMessageFromClient(new ChatMessage(input));
		}
	}
}
