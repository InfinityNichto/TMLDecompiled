using System.IO;

namespace Terraria.ModLoader.Default;

internal abstract class NetHandler
{
	internal byte HandlerType { get; set; }

	public abstract void HandlePacket(BinaryReader r, int fromWho);

	protected NetHandler(byte handlerType)
	{
		HandlerType = handlerType;
	}

	protected ModPacket GetPacket(byte packetType, int fromWho)
	{
		ModPacket p = ModContent.GetInstance<ModLoaderMod>().GetPacket();
		p.Write(HandlerType);
		p.Write(packetType);
		if (Main.netMode == 2)
		{
			p.Write((byte)fromWho);
		}
		return p;
	}
}
