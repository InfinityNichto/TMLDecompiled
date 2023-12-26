using Microsoft.Xna.Framework;

namespace Terraria.ModLoader;

public interface CommandCaller
{
	CommandType CommandType { get; }

	/// <summary>
	/// The Player object corresponding to the Player that invoked this command. Use this when the Player is needed. Don't use Main.LocalPlayer because that would be incorrect for various CommandTypes.
	/// </summary>
	Player Player { get; }

	/// <summary>
	/// Use this to respond to the Player that invoked this command. This method handles writing to the console, writing to chat, or sending messages over the network for you depending on the CommandType used. Avoid using Main.NewText, Console.WriteLine, or NetMessage.SendChatMessageToClient directly because the logic would change depending on CommandType.
	/// </summary>
	/// <param name="text"></param>
	/// <param name="color"></param>
	void Reply(string text, Color color = default(Color));
}
