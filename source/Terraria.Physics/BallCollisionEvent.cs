using Microsoft.Xna.Framework;

namespace Terraria.Physics;

public struct BallCollisionEvent
{
	public readonly Vector2 Normal;

	public readonly Vector2 ImpactPoint;

	public readonly Tile Tile;

	public readonly Entity Entity;

	public readonly float TimeScale;

	public BallCollisionEvent(float timeScale, Vector2 normal, Vector2 impactPoint, Tile tile, Entity entity)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		Normal = normal;
		ImpactPoint = impactPoint;
		Tile = tile;
		Entity = entity;
		TimeScale = timeScale;
	}
}
