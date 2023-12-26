using System;
using System.IO;
using Microsoft.Xna.Framework.Audio;
using ReLogic.Content;
using ReLogic.Content.Readers;
using XPT.Core.Audio.MP3Sharp;

namespace Terraria.ModLoader.Assets;

public class MP3Reader : IAssetReader
{
	T IAssetReader.FromStream<T>(Stream stream)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Expected O, but got Unknown
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		if (typeof(T) != typeof(SoundEffect))
		{
			throw AssetLoadException.FromInvalidReader<MP3Reader, T>();
		}
		MP3Stream mp3Stream = new MP3Stream(stream);
		try
		{
			using MemoryStream ms = new MemoryStream();
			((Stream)(object)mp3Stream).CopyTo((Stream)ms);
			return new SoundEffect(ms.ToArray(), mp3Stream.Frequency, (AudioChannels)2) as T;
		}
		finally
		{
			((IDisposable)mp3Stream)?.Dispose();
		}
	}
}
