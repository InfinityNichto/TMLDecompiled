using System;
using System.IO;
using Microsoft.Xna.Framework.Audio;
using NVorbis;
using ReLogic.Content;
using ReLogic.Content.Readers;

namespace Terraria.ModLoader.Assets;

public class OggReader : IAssetReader
{
	T IAssetReader.FromStream<T>(Stream stream)
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Expected O, but got Unknown
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Expected O, but got Unknown
		if (typeof(T) != typeof(SoundEffect))
		{
			throw AssetLoadException.FromInvalidReader<OggReader, T>();
		}
		VorbisReader reader = new VorbisReader(stream, true);
		try
		{
			byte[] buffer = new byte[reader.TotalSamples * 2 * reader.Channels];
			float[] floatBuf = new float[buffer.Length / 2];
			reader.ReadSamples(floatBuf, 0, floatBuf.Length);
			Convert(floatBuf, buffer);
			return new SoundEffect(buffer, reader.SampleRate, (AudioChannels)reader.Channels) as T;
		}
		finally
		{
			((IDisposable)reader)?.Dispose();
		}
	}

	public static void Convert(float[] floatBuf, byte[] buffer)
	{
		for (int i = 0; i < floatBuf.Length; i++)
		{
			short val = (short)(floatBuf[i] * 32767f);
			buffer[i * 2] = (byte)val;
			buffer[i * 2 + 1] = (byte)(val >> 8);
		}
	}
}
