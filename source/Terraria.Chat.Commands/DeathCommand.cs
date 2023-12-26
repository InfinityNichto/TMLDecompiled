using Microsoft.Xna.Framework;
using Terraria.Localization;

namespace Terraria.Chat.Commands;

[ChatCommand("Death")]
public class DeathCommand : IChatCommand
{
	private static readonly Color RESPONSE_COLOR = new Color(255, 25, 25);

	public void ProcessIncomingMessage(string text, byte clientId)
	{
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		NetworkText text2 = NetworkText.FromKey("LegacyMultiplayer.23", Main.player[clientId].name, Main.player[clientId].numberOfDeathsPVE);
		if (Main.player[clientId].numberOfDeathsPVE == 1)
		{
			text2 = NetworkText.FromKey("LegacyMultiplayer.25", Main.player[clientId].name, Main.player[clientId].numberOfDeathsPVE);
		}
		ChatHelper.BroadcastChatMessage(text2, RESPONSE_COLOR);
	}

	public void ProcessOutgoingMessage(ChatMessage message)
	{
	}
}
