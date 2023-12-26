using System.IO;
using Microsoft.Xna.Framework.Audio;
using ReLogic.Content;
using ReLogic.Content.Readers;

namespace Terraria.ModLoader.Assets;

public class WavReader : IAssetReader
{
	T IAssetReader.FromStream<T>(Stream stream)
	{
		if (typeof(T) != typeof(SoundEffect))
		{
			throw AssetLoadException.FromInvalidReader<WavReader, T>();
		}
		return SoundEffect.FromStream(stream) as T;
	}
}
