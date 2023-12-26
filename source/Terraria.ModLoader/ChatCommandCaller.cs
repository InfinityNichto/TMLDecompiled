using Microsoft.Xna.Framework;

namespace Terraria.ModLoader;

internal class ChatCommandCaller : CommandCaller
{
	public CommandType CommandType => CommandType.Chat;

	public Player Player => Main.player[Main.myPlayer];

	public void Reply(string text, Color color = default(Color))
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		if (color == default(Color))
		{
			color = Color.White;
		}
		string[] array = text.Split('\n');
		for (int i = 0; i < array.Length; i++)
		{
			Main.NewText(array[i], ((Color)(ref color)).R, ((Color)(ref color)).G, ((Color)(ref color)).B);
		}
	}
}
