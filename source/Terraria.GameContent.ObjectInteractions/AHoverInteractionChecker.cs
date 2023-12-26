using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.GameInput;

namespace Terraria.GameContent.ObjectInteractions;

public abstract class AHoverInteractionChecker
{
	internal enum HoverStatus
	{
		NotSelectable,
		SelectableButNotSelected,
		Selected
	}

	internal HoverStatus AttemptInteraction(Player player, Rectangle Hitbox)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		Point point = Hitbox.ClosestPointInRect(player.Center).ToTileCoordinates();
		if (!player.IsInTileInteractionRange(point.X, point.Y, TileReachCheckSettings.Simple))
		{
			return HoverStatus.NotSelectable;
		}
		Matrix matrix = Matrix.Invert(Main.GameViewMatrix.ZoomMatrix);
		Vector2 val = Main.ReverseGravitySupport(Main.MouseScreen);
		Vector2.Transform(Main.screenPosition, matrix);
		Vector2 v = val + Main.screenPosition;
		bool flag = ((Rectangle)(ref Hitbox)).Contains(v.ToPoint());
		bool flag2 = flag;
		bool? flag3 = AttemptOverridingHoverStatus(player, Hitbox);
		if (flag3.HasValue)
		{
			flag2 = flag3.Value;
		}
		flag2 &= !player.lastMouseInterface;
		bool flag4 = !Main.SmartCursorIsUsed && !PlayerInput.UsingGamepad;
		if (!flag2)
		{
			if (!flag4)
			{
				return HoverStatus.SelectableButNotSelected;
			}
			return HoverStatus.NotSelectable;
		}
		Main.HasInteractibleObjectThatIsNotATile = true;
		if (flag)
		{
			DoHoverEffect(player, Hitbox);
		}
		if (PlayerInput.UsingGamepad)
		{
			player.GamepadEnableGrappleCooldown();
		}
		bool flag5 = ShouldBlockInteraction(player, Hitbox);
		if (Main.mouseRight && Main.mouseRightRelease && !flag5)
		{
			Main.mouseRightRelease = false;
			player.tileInteractAttempted = true;
			player.tileInteractionHappened = true;
			player.releaseUseTile = false;
			PerformInteraction(player, Hitbox);
		}
		if (!Main.SmartCursorIsUsed && !PlayerInput.UsingGamepad)
		{
			return HoverStatus.NotSelectable;
		}
		if (!flag4)
		{
			return HoverStatus.Selected;
		}
		return HoverStatus.NotSelectable;
	}

	internal abstract bool? AttemptOverridingHoverStatus(Player player, Rectangle rectangle);

	internal abstract void DoHoverEffect(Player player, Rectangle hitbox);

	internal abstract bool ShouldBlockInteraction(Player player, Rectangle hitbox);

	internal abstract void PerformInteraction(Player player, Rectangle hitbox);
}
