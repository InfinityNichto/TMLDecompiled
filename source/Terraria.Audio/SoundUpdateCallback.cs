namespace Terraria.Audio;

/// <summary>
/// <see cref="T:Terraria.Audio.ActiveSound" />'s update callback.
/// <br /> Returning false here will force the sound to end abruptly.
/// <br /> Tip: Use <see cref="M:Terraria.Audio.ProjectileAudioTracker.IsActiveAndInGame" /> to tie sounds to projectiles.
/// </summary>
/// <param name="soundInstance"> The sound object instance. </param>
/// <returns> Whether the sound effect should continue to play. </returns>
public delegate bool SoundUpdateCallback(ActiveSound soundInstance);
