using System.IO;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using ReLogic.Content.Readers;
using Terraria.ModLoader.IO;

namespace Terraria.ModLoader.Assets;

public class RawImgReader : IAssetReader
{
	private readonly GraphicsDevice _graphicsDevice;

	public RawImgReader(GraphicsDevice graphicsDevice)
	{
		_graphicsDevice = graphicsDevice;
	}

	public async ValueTask<T> FromStream<T>(Stream stream, MainThreadCreationContext mainThreadCtx) where T : class
	{
		if (typeof(T) != typeof(Texture2D))
		{
			throw AssetLoadException.FromInvalidReader<RawImgReader, T>();
		}
		int width;
		int height;
		byte[] data = ImageIO.ReadRaw(stream, out width, out height);
		await mainThreadCtx;
		Texture2D val = new Texture2D(_graphicsDevice, width, height);
		val.SetData<byte>(data);
		return val as T;
	}
}
