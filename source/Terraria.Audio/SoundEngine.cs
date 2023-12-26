using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using ReLogic.Utilities;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Terraria.Audio;

public static class SoundEngine
{
	internal static LegacySoundPlayer LegacySoundPlayer;

	public static SoundPlayer SoundPlayer;

	public static bool AreSoundsPaused;

	public static bool IsAudioSupported { get; private set; }

	public static void Initialize()
	{
		IsAudioSupported = TestAudioSupport();
		if (!IsAudioSupported)
		{
			Utils.ShowFancyErrorMessage(Language.GetTextValue("tModLoader.AudioNotSupported"), 10002);
		}
	}

	public static void Load(IServiceProvider services)
	{
		if (IsAudioSupported)
		{
			LegacySoundPlayer = new LegacySoundPlayer(services);
			SoundPlayer = new SoundPlayer();
		}
	}

	public static void Update()
	{
		if (IsAudioSupported)
		{
			if (Main.audioSystem != null)
			{
				Main.audioSystem.UpdateAudioEngine();
			}
			SoundInstanceGarbageCollector.Update();
			bool flag = (!Main.hasFocus || Main.gamePaused) && Main.netMode == 0;
			if (!AreSoundsPaused && flag)
			{
				SoundPlayer.PauseAll();
			}
			else if (AreSoundsPaused && !flag)
			{
				SoundPlayer.ResumeAll();
			}
			AreSoundsPaused = flag;
			SoundPlayer.Update();
		}
	}

	public static void Reload()
	{
		if (IsAudioSupported)
		{
			if (LegacySoundPlayer != null)
			{
				LegacySoundPlayer.Reload();
			}
			if (SoundPlayer != null)
			{
				SoundPlayer.Reload();
			}
		}
	}

	public static void StopTrackedSounds()
	{
		if (!Main.dedServ && IsAudioSupported)
		{
			SoundPlayer.StopAll();
		}
	}

	public static void StopAmbientSounds()
	{
		if (!Main.dedServ && IsAudioSupported && LegacySoundPlayer != null)
		{
			LegacySoundPlayer.StopAmbientSounds();
		}
	}

	public static ActiveSound FindActiveSound(in SoundStyle style)
	{
		if (Main.dedServ || !IsAudioSupported)
		{
			return null;
		}
		return SoundPlayer.FindActiveSound(in style);
	}

	private static bool TestAudioSupport()
	{
		byte[] buffer = new byte[166]
		{
			82, 73, 70, 70, 158, 0, 0, 0, 87, 65,
			86, 69, 102, 109, 116, 32, 16, 0, 0, 0,
			1, 0, 1, 0, 68, 172, 0, 0, 136, 88,
			1, 0, 2, 0, 16, 0, 76, 73, 83, 84,
			26, 0, 0, 0, 73, 78, 70, 79, 73, 83,
			70, 84, 14, 0, 0, 0, 76, 97, 118, 102,
			53, 54, 46, 52, 48, 46, 49, 48, 49, 0,
			100, 97, 116, 97, 88, 0, 0, 0, 0, 0,
			126, 4, 240, 8, 64, 13, 95, 17, 67, 21,
			217, 24, 23, 28, 240, 30, 94, 33, 84, 35,
			208, 36, 204, 37, 71, 38, 64, 38, 183, 37,
			180, 36, 58, 35, 79, 33, 1, 31, 86, 28,
			92, 25, 37, 22, 185, 18, 42, 15, 134, 11,
			222, 7, 68, 4, 196, 0, 112, 253, 86, 250,
			132, 247, 6, 245, 230, 242, 47, 241, 232, 239,
			25, 239, 194, 238, 231, 238, 139, 239, 169, 240,
			61, 242, 67, 244, 180, 246
		};
		try
		{
			using MemoryStream stream = new MemoryStream(buffer);
			SoundEffect.FromStream((Stream)stream);
		}
		catch (NoAudioHardwareException)
		{
			Logging.tML.Warn((object)"No audio hardware found. Disabling all audio.");
			return false;
		}
		catch
		{
			return false;
		}
		return true;
	}

	/// <inheritdoc cref="M:Terraria.Audio.SoundEngine.PlaySound(Terraria.Audio.SoundStyle@,System.Nullable{Microsoft.Xna.Framework.Vector2},Terraria.Audio.SoundUpdateCallback)" />
	/// <summary>
	/// Attempts to play a sound style with the provided sound style (if it's not null), and returns a valid <see cref="T:ReLogic.Utilities.SlotId" /> handle to it on success.
	/// </summary>
	public static SlotId PlaySound(in SoundStyle? style, Vector2? position = null, SoundUpdateCallback? updateCallback = null)
	{
		if (!style.HasValue)
		{
			return SlotId.Invalid;
		}
		SoundStyle style2 = style.Value;
		return PlaySound(in style2, position, updateCallback);
	}

	/// <summary>
	/// Attempts to play a sound with the provided sound style, and returns a valid <see cref="T:ReLogic.Utilities.SlotId" /> handle to it on success.
	/// </summary>
	/// <param name="style"> The sound style that describes everything about the played sound. </param>
	/// <param name="position"> An optional 2D position to play the sound at. When null, this sound will be heard everywhere. </param>
	/// <param name="updateCallback"> A callback for customizing the behavior of the created sound instance, like tying its existence to a projectile using <see cref="T:Terraria.Audio.ProjectileAudioTracker" />. </param>
	public static SlotId PlaySound(in SoundStyle style, Vector2? position = null, SoundUpdateCallback? updateCallback = null)
	{
		if (Main.dedServ || !IsAudioSupported)
		{
			return SlotId.Invalid;
		}
		return SoundPlayer.Play(in style, position, updateCallback);
	}

	/// <inheritdoc cref="M:Terraria.Audio.SoundPlayer.TryGetActiveSound(ReLogic.Utilities.SlotId,Terraria.Audio.ActiveSound@)" />
	public static bool TryGetActiveSound(SlotId slotId, [NotNullWhen(true)] out ActiveSound? result)
	{
		if (Main.dedServ || !IsAudioSupported)
		{
			result = null;
			return false;
		}
		return SoundPlayer.TryGetActiveSound(slotId, out result);
	}

	internal static SoundEffectInstance? PlaySound(SoundStyle? style, Vector2? position = null)
	{
		SlotId slotId = PlaySound(in style, position);
		if (!slotId.IsValid)
		{
			return null;
		}
		return GetActiveSound(slotId)?.Sound;
	}

	internal static SoundEffectInstance? PlaySound(SoundStyle? type, int x, int y)
	{
		return PlaySound(type, XYToOptionalPosition(x, y));
	}

	internal static void PlaySound(int type, Vector2 position, int style = 1)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		PlaySound(type, (int)position.X, (int)position.Y, style);
	}

	internal static SoundEffectInstance? PlaySound(int type, int x = -1, int y = -1, int Style = 1, float volumeScale = 1f, float pitchOffset = 0f)
	{
		if (!SoundID.TryGetLegacyStyle(type, Style, out var soundStyle))
		{
			Logging.tML.Warn((object)$"Failed to get legacy sound style for ({type}, {Style}) input.");
			return null;
		}
		soundStyle = soundStyle with
		{
			Volume = soundStyle.Volume * volumeScale,
			Pitch = soundStyle.Pitch + pitchOffset
		};
		SlotId slotId = PlaySound(in soundStyle, XYToOptionalPosition(x, y));
		if (!slotId.IsValid)
		{
			return null;
		}
		return GetActiveSound(slotId)?.Sound;
	}

	internal static SlotId PlayTrackedSound(in SoundStyle style, Vector2? position = null)
	{
		return PlaySound(in style, position);
	}

	internal static SlotId PlayTrackedLoopedSound(in SoundStyle style, Vector2 position, Func<bool>? loopingCondition = null)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		Func<bool> loopingCondition2 = loopingCondition;
		return PlaySound(in style, position, (ActiveSound _) => loopingCondition2());
	}

	internal static ActiveSound? GetActiveSound(SlotId slotId)
	{
		if (!TryGetActiveSound(slotId, out ActiveSound result))
		{
			return null;
		}
		return result;
	}

	private static Vector2? XYToOptionalPosition(int x, int y)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		if (x == -1 && y == -1)
		{
			return null;
		}
		return new Vector2((float)x, (float)y);
	}
}
