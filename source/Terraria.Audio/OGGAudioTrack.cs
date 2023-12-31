using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework.Audio;
using NVorbis;

namespace Terraria.Audio;

public class OGGAudioTrack : ASoundEffectBasedAudioTrack
{
	private VorbisReader _vorbisReader;

	private int _loopStart;

	private int _loopEnd;

	public OGGAudioTrack(Stream streamToRead)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		_vorbisReader = new VorbisReader(streamToRead, true);
		FindLoops();
		CreateSoundEffect(_vorbisReader.SampleRate, (AudioChannels)_vorbisReader.Channels);
	}

	protected override void ReadAheadPutAChunkIntoTheBuffer()
	{
		PrepareBufferToSubmit();
		_soundEffectInstance.SubmitBuffer(_bufferToSubmit);
	}

	private void PrepareBufferToSubmit()
	{
		byte[] bufferToSubmit = _bufferToSubmit;
		float[] temporaryBuffer = _temporaryBuffer;
		VorbisReader vorbisReader = _vorbisReader;
		int num = vorbisReader.ReadSamples(temporaryBuffer, 0, temporaryBuffer.Length);
		bool num2 = _loopEnd > 0 && vorbisReader.DecodedPosition >= _loopEnd;
		bool flag = num < temporaryBuffer.Length;
		if (num2 || flag)
		{
			vorbisReader.DecodedPosition = _loopStart;
			vorbisReader.ReadSamples(temporaryBuffer, num, temporaryBuffer.Length - num);
		}
		ApplyTemporaryBufferTo(temporaryBuffer, bufferToSubmit);
	}

	private static void ApplyTemporaryBufferTo(float[] temporaryBuffer, byte[] samplesBuffer)
	{
		for (int i = 0; i < temporaryBuffer.Length; i++)
		{
			short num = (short)(temporaryBuffer[i] * 32767f);
			samplesBuffer[i * 2] = (byte)num;
			samplesBuffer[i * 2 + 1] = (byte)(num >> 8);
		}
	}

	public override void Reuse()
	{
		_vorbisReader.SeekTo(0L, SeekOrigin.Begin);
	}

	private void FindLoops()
	{
		IDictionary<string, IList<string>> tags = _vorbisReader.Tags.All;
		TryReadInteger("LOOPSTART", ref _loopStart);
		TryReadInteger("LOOPEND", ref _loopEnd);
		void TryReadInteger(string entryName, ref int result)
		{
			if (tags.TryGetValue(entryName, out var values) && values.Count > 0 && int.TryParse(values[0], out var potentialResult))
			{
				result = potentialResult;
			}
		}
	}

	public override void Dispose()
	{
		((SoundEffectInstance)_soundEffectInstance).Dispose();
		_vorbisReader.Dispose();
	}
}
