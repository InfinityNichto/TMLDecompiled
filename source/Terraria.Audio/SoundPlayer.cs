using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using ReLogic.Utilities;

namespace Terraria.Audio;

public class SoundPlayer
{
	private readonly SlotVector<ActiveSound> _trackedSounds = new SlotVector<ActiveSound>(4096);

	public SlotId Play(in SoundStyle style, Vector2? position = null, SoundUpdateCallback? updateCallback = null)
	{
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		SoundUpdateCallback updateCallback2 = updateCallback;
		if (Main.dedServ)
		{
			return SlotId.Invalid;
		}
		if (position.HasValue && Vector2.DistanceSquared(Main.screenPosition + new Vector2((float)(Main.screenWidth / 2), (float)(Main.screenHeight / 2)), position.Value) > 100000000f)
		{
			return SlotId.Invalid;
		}
		if (style.PlayOnlyIfFocused && !Main.hasFocus)
		{
			return SlotId.Invalid;
		}
		if (!Program.IsMainThread)
		{
			SoundStyle styleCopy = style;
			return Main.RunOnMainThread(() => Play_Inner(in styleCopy, position, updateCallback2)).GetAwaiter().GetResult();
		}
		return Play_Inner(in style, position, updateCallback2);
	}

	private SlotId Play_Inner(in SoundStyle style, Vector2? position, SoundUpdateCallback? updateCallback)
	{
		int maxInstances = style.MaxInstances;
		if (maxInstances > 0)
		{
			int instanceCount = 0;
			foreach (SlotVector<ActiveSound>.ItemPair item in (IEnumerable<SlotVector<ActiveSound>.ItemPair>)_trackedSounds)
			{
				ActiveSound activeSound = item.Value;
				if (activeSound.IsPlaying && style.IsTheSameAs(activeSound.Style) && ++instanceCount >= maxInstances)
				{
					if (style.SoundLimitBehavior != SoundLimitBehavior.ReplaceOldest)
					{
						return SlotId.Invalid;
					}
					SoundEffectInstance? sound = activeSound.Sound;
					if (sound != null)
					{
						sound.Stop(true);
					}
				}
			}
		}
		SoundStyle styleCopy = style;
		if (style.UsesMusicPitch)
		{
			styleCopy.Pitch += Main.musicPitch;
		}
		ActiveSound value = new ActiveSound(styleCopy, position, updateCallback);
		return _trackedSounds.Add(value);
	}

	public void Reload()
	{
		StopAll();
	}

	internal ActiveSound? GetActiveSound(SlotId id)
	{
		if (!TryGetActiveSound(id, out ActiveSound result))
		{
			return null;
		}
		return result;
	}

	public void PauseAll()
	{
		foreach (SlotVector<ActiveSound>.ItemPair item in (IEnumerable<SlotVector<ActiveSound>.ItemPair>)_trackedSounds)
		{
			item.Value.Pause();
		}
	}

	public void ResumeAll()
	{
		foreach (SlotVector<ActiveSound>.ItemPair item in (IEnumerable<SlotVector<ActiveSound>.ItemPair>)_trackedSounds)
		{
			item.Value.Resume();
		}
	}

	public void StopAll()
	{
		foreach (SlotVector<ActiveSound>.ItemPair item in (IEnumerable<SlotVector<ActiveSound>.ItemPair>)_trackedSounds)
		{
			item.Value.Stop();
		}
		_trackedSounds.Clear();
	}

	public void Update()
	{
		foreach (SlotVector<ActiveSound>.ItemPair item in (IEnumerable<SlotVector<ActiveSound>.ItemPair>)_trackedSounds)
		{
			try
			{
				item.Value.Update();
				if (!item.Value.IsPlaying)
				{
					_trackedSounds.Remove(item.Id);
				}
			}
			catch
			{
				_trackedSounds.Remove(item.Id);
			}
		}
	}

	public ActiveSound? FindActiveSound(in SoundStyle style)
	{
		foreach (SlotVector<ActiveSound>.ItemPair item in (IEnumerable<SlotVector<ActiveSound>.ItemPair>)_trackedSounds)
		{
			if (item.Value.Style == style)
			{
				return item.Value;
			}
		}
		return null;
	}

	/// <summary>
	/// Safely attempts to get a currently playing <see cref="T:Terraria.Audio.ActiveSound" /> instance, tied to the provided <see cref="T:ReLogic.Utilities.SlotId" />.
	/// </summary>
	public bool TryGetActiveSound(SlotId id, [NotNullWhen(true)] out ActiveSound? result)
	{
		return _trackedSounds.TryGetValue(id, out result);
	}

	public void StopAll(in SoundStyle style)
	{
		List<SlotVector<ActiveSound>.ItemPair> stopped = new List<SlotVector<ActiveSound>.ItemPair>();
		foreach (SlotVector<ActiveSound>.ItemPair item2 in (IEnumerable<SlotVector<ActiveSound>.ItemPair>)_trackedSounds)
		{
			if (style.IsTheSameAs(item2.Value.Style))
			{
				item2.Value.Stop();
				stopped.Add(item2);
			}
		}
		foreach (SlotVector<ActiveSound>.ItemPair item in stopped)
		{
			_trackedSounds.Remove(item.Id);
		}
	}
}
