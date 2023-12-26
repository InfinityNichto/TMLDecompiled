using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Terraria.GameContent;

public class SpelunkerProjectileHelper
{
	private HashSet<Vector2> _positionsChecked = new HashSet<Vector2>();

	private HashSet<Point> _tilesChecked = new HashSet<Point>();

	private Rectangle _clampBox;

	private int _frameCounter;

	public void OnPreUpdateAllProjectiles()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		_clampBox = new Rectangle(2, 2, Main.maxTilesX - 2, Main.maxTilesY - 2);
		if (++_frameCounter >= 10)
		{
			_frameCounter = 0;
			_tilesChecked.Clear();
			_positionsChecked.Clear();
		}
	}

	public void AddSpotToCheck(Vector2 spot)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		if (_positionsChecked.Add(spot))
		{
			CheckSpot(spot);
		}
	}

	private void CheckSpot(Vector2 Center)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_0158: Unknown result type (might be due to invalid IL or missing references)
		//IL_0174: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0193: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
		int num = (int)Center.X / 16;
		int num2 = (int)Center.Y / 16;
		int num6 = Utils.Clamp(num - 30, ((Rectangle)(ref _clampBox)).Left, ((Rectangle)(ref _clampBox)).Right);
		int num3 = Utils.Clamp(num + 30, ((Rectangle)(ref _clampBox)).Left, ((Rectangle)(ref _clampBox)).Right);
		int num4 = Utils.Clamp(num2 - 30, ((Rectangle)(ref _clampBox)).Top, ((Rectangle)(ref _clampBox)).Bottom);
		int num5 = Utils.Clamp(num2 + 30, ((Rectangle)(ref _clampBox)).Top, ((Rectangle)(ref _clampBox)).Bottom);
		Point item = default(Point);
		Vector2 position = default(Vector2);
		for (int i = num6; i <= num3; i++)
		{
			for (int j = num4; j <= num5; j++)
			{
				Tile tile = Main.tile[i, j];
				if (!(tile != null) || !tile.active() || !Main.IsTileSpelunkable(i, j, tile))
				{
					continue;
				}
				Vector2 val = new Vector2((float)(num - i), (float)(num2 - j));
				if (!(((Vector2)(ref val)).Length() > 30f))
				{
					item.X = i;
					item.Y = j;
					if (_tilesChecked.Add(item) && Main.rand.Next(4) == 0)
					{
						position.X = i * 16;
						position.Y = j * 16;
						Dust dust = Dust.NewDustDirect(position, 16, 16, 204, 0f, 0f, 150, default(Color), 0.3f);
						dust.fadeIn = 0.75f;
						dust.velocity *= 0.1f;
						dust.noLight = true;
					}
				}
			}
		}
	}
}
