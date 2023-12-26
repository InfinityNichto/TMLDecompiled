using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria.Chat;
using Terraria.Localization;

namespace Terraria.ModLoader.Default;

internal class ModlistCommand : ModCommand
{
	public override string Command => "modlist";

	public override CommandType Type => CommandType.Chat | CommandType.Server | CommandType.Console;

	public override string Description => Language.GetTextValue("tModLoader.CommandModListDescription");

	public override void Action(CommandCaller caller, string input, string[] args)
	{
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		IEnumerable<Mod> mods = ModLoader.Mods.Skip(1);
		if (Main.netMode == 1)
		{
			ChatHelper.SendChatMessageFromClient(new ChatMessage(input));
			Mod[] client = mods.Where((Mod m) => m.Side == ModSide.Client || m.Side == ModSide.NoSync).ToArray();
			if (client.Length != 0)
			{
				caller.Reply(Language.GetTextValue("tModLoader.CommandModListClientMods"), Color.Yellow);
				Mod[] array = client;
				foreach (Mod mod in array)
				{
					caller.Reply(mod.DisplayName);
				}
			}
			return;
		}
		if (caller.CommandType == CommandType.Server)
		{
			Mod[] server = mods.Where((Mod m) => m.Side == ModSide.Server || m.Side == ModSide.NoSync).ToArray();
			if (server.Length != 0)
			{
				caller.Reply(Language.GetTextValue("tModLoader.CommandModListServerMods"), Color.Yellow);
				Mod[] array = server;
				foreach (Mod mod4 in array)
				{
					caller.Reply(mod4.DisplayName);
				}
			}
			caller.Reply(Language.GetTextValue("tModLoader.CommandModListSyncedMods"), Color.Yellow);
			{
				foreach (Mod mod2 in mods.Where((Mod m) => m.Side == ModSide.Both))
				{
					caller.Reply(mod2.DisplayName);
				}
				return;
			}
		}
		if (caller.CommandType == CommandType.Chat)
		{
			caller.Reply(Language.GetTextValue("tModLoader.CommandModListModlist"), Color.Yellow);
		}
		foreach (Mod mod3 in mods)
		{
			caller.Reply(mod3.DisplayName);
		}
	}
}
