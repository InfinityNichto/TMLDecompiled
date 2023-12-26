using Microsoft.Xna.Framework;

namespace Terraria.DataStructures;

public struct ContainerTransferContext
{
	private Vector2 _position;

	public bool CanVisualizeTransfers;

	public static ContainerTransferContext FromProjectile(Projectile projectile)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		return new ContainerTransferContext(projectile.Center);
	}

	public static ContainerTransferContext FromBlockPosition(int x, int y)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		return new ContainerTransferContext(new Vector2((float)(x * 16 + 16), (float)(y * 16 + 16)));
	}

	public static ContainerTransferContext FromUnknown(Player player)
	{
		ContainerTransferContext result = default(ContainerTransferContext);
		result.CanVisualizeTransfers = false;
		return result;
	}

	public ContainerTransferContext(Vector2 position)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		_position = position;
		CanVisualizeTransfers = true;
	}

	public Vector2 GetContainerWorldPosition()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		return _position;
	}
}
