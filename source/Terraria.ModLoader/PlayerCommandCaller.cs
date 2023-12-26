using Microsoft.Xna.Framework;
using Terraria.Chat;
using Terraria.Localization;

namespace Terraria.ModLoader;

internal class PlayerCommandCaller : CommandCaller
{
	public CommandType CommandType => CommandType.Server;

	public Player Player { get; }

	public PlayerCommandCaller(Player player)
	{
		Player = player;
	}

	public void Reply(string text, Color color = default(Color))
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		if (color == default(Color))
		{
			color = Color.White;
		}
		string[] array = text.Split('\n');
		for (int i = 0; i < array.Length; i++)
		{
			ChatHelper.SendChatMessageToClient(NetworkText.FromLiteral(array[i]), color, Player.whoAmI);
		}
	}
}
