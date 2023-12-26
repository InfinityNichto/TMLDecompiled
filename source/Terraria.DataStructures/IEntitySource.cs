using System;

namespace Terraria.DataStructures;

/// <summary>
/// This object encapsulates context information about the source of a particular spawning event of an Item/Projectile/NPC/etc. Aids in facilitating many modding situations and used in various OnSpawn hooks.<br />
/// The <see href="https://github.com/tModLoader/tModLoader/wiki/IEntitySource">IEntitySource Guide</see> teaches how and why to use this.
/// </summary>
public interface IEntitySource
{
	internal struct FallbackSourceRef : IDisposable
	{
		private IEntitySource? prev;

		public FallbackSourceRef(IEntitySource source)
		{
			prev = _goreFallback;
			_goreFallback = source;
		}

		public void Dispose()
		{
			_goreFallback = prev;
		}
	}

	private static IEntitySource? _goreFallback;

	/// <summary>
	/// Additional context identifier, particularly useful for set bonuses or accessory affects. See <see cref="T:Terraria.ID.ItemSourceID" /> and <see cref="T:Terraria.ID.ProjectileSourceID" /> for vanilla uses
	/// </summary>
	string? Context { get; }

	internal static IEntitySource? GetGoreFallback()
	{
		return _goreFallback;
	}

	internal static FallbackSourceRef PushFallback(IEntitySource source)
	{
		return new FallbackSourceRef(source);
	}
}
