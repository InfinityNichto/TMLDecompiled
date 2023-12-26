using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace Terraria.Audio;

public class ActiveSound
{
	public Vector2? Position;

	public float Volume;

	public float Pitch;

	public SoundUpdateCallback? Callback;

	public SoundEffectInstance? Sound { get; private set; }

	public SoundStyle Style { get; private set; }

	public bool IsPlaying
	{
		get
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Invalid comparison between Unknown and I4
			SoundEffectInstance? sound = Sound;
			if (sound != null && !sound.IsDisposed)
			{
				return (int)Sound.State == 0;
			}
			return false;
		}
	}

	public ActiveSound(SoundStyle style, Vector2? position = null, SoundUpdateCallback? updateCallback = null)
	{
		Position = position;
		Volume = 1f;
		Pitch = style.PitchVariance;
		Style = style;
		Callback = updateCallback;
		Play();
	}

	private void Play()
	{
		if (!Program.IsMainThread)
		{
			RunOnMainThreadAndWait(Play);
			return;
		}
		SoundEffectInstance soundEffectInstance = Style.GetRandomSound().CreateInstance();
		soundEffectInstance.Pitch += Style.GetRandomPitch();
		Pitch = soundEffectInstance.Pitch;
		soundEffectInstance.IsLooped = Style.IsLooped;
		soundEffectInstance.Play();
		SoundInstanceGarbageCollector.Track(soundEffectInstance);
		Sound = soundEffectInstance;
		Update();
	}

	public void Stop()
	{
		if (!Program.IsMainThread)
		{
			RunOnMainThreadAndWait(Stop);
		}
		else if (Sound != null)
		{
			Sound.Stop();
		}
	}

	public void Pause()
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		if (!Program.IsMainThread)
		{
			RunOnMainThreadAndWait(Pause);
		}
		else if (Sound != null && (int)Sound.State == 0)
		{
			Sound.Pause();
		}
	}

	public void Resume()
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Invalid comparison between Unknown and I4
		if (!Program.IsMainThread)
		{
			RunOnMainThreadAndWait(Resume);
		}
		else if (Sound != null && (int)Sound.State == 1)
		{
			Sound.Resume();
		}
	}

	public void Update()
	{
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		if (Sound == null)
		{
			return;
		}
		if (!Program.IsMainThread)
		{
			RunOnMainThreadAndWait(Update);
		}
		else
		{
			if (Sound.IsDisposed)
			{
				return;
			}
			SoundUpdateCallback? callback = Callback;
			if (callback != null && !callback(this))
			{
				Sound.Stop(true);
				return;
			}
			Vector2 value = Main.screenPosition + new Vector2((float)(Main.screenWidth / 2), (float)(Main.screenHeight / 2));
			float num = 1f;
			if (Position.HasValue)
			{
				float value2 = (Position.Value.X - value.X) / ((float)Main.screenWidth * 0.5f);
				value2 = MathHelper.Clamp(value2, -1f, 1f);
				Sound.Pan = value2;
				float num2 = Vector2.Distance(Position.Value, value);
				num = 1f - num2 / ((float)Main.screenWidth * 1.5f);
			}
			num *= Style.Volume * Volume;
			switch (Style.Type)
			{
			case SoundType.Sound:
				num *= Main.soundVolume;
				break;
			case SoundType.Ambient:
				num *= Main.ambientVolume;
				if (Main.gameInactive)
				{
					num = 0f;
				}
				break;
			case SoundType.Music:
				num *= Main.musicVolume;
				break;
			}
			num = MathHelper.Clamp(num, 0f, 1f);
			Sound.Volume = num;
			Sound.Pitch = Pitch;
		}
	}

	private static void RunOnMainThreadAndWait(Action action)
	{
		Main.RunOnMainThread(action).GetAwaiter().GetResult();
	}
}
