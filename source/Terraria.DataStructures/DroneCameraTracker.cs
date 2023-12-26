using Microsoft.Xna.Framework;

namespace Terraria.DataStructures;

public class DroneCameraTracker
{
	private Projectile _trackedProjectile;

	private int _lastTrackedType;

	private bool _inUse;

	public void Track(Projectile proj)
	{
		_trackedProjectile = proj;
		_lastTrackedType = proj.type;
	}

	public void Clear()
	{
		_trackedProjectile = null;
	}

	public void WorldClear()
	{
		_lastTrackedType = 0;
		_trackedProjectile = null;
		_inUse = false;
	}

	private void ValidateTrackedProjectile()
	{
		if (_trackedProjectile == null || !_trackedProjectile.active || _trackedProjectile.type != _lastTrackedType || _trackedProjectile.owner != Main.myPlayer || !Main.LocalPlayer.remoteVisionForDrone)
		{
			Clear();
		}
	}

	public bool IsInUse()
	{
		return _inUse;
	}

	public bool TryTracking(out Vector2 cameraPosition)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		ValidateTrackedProjectile();
		cameraPosition = default(Vector2);
		if (_trackedProjectile == null)
		{
			_inUse = false;
			return false;
		}
		cameraPosition = _trackedProjectile.Center;
		_inUse = true;
		return true;
	}
}
