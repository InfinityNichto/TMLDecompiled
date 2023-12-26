using Microsoft.Xna.Framework;

namespace Terraria.GameContent;

public class ShimmerHelper
{
	public static Vector2? FindSpotWithoutShimmer(Entity entity, int startX, int startY, int expand, bool allowSolidTop)
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0158: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		//IL_018c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0191: Unknown result type (might be due to invalid IL or missing references)
		//IL_0194: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Unknown result type (might be due to invalid IL or missing references)
		//IL_019f: Unknown result type (might be due to invalid IL or missing references)
		Vector2 vector = default(Vector2);
		((Vector2)(ref vector))._002Ector((float)(-entity.width / 2), (float)(-entity.height));
		for (int i = 0; i < expand; i++)
		{
			int num4 = startX - i;
			int num2 = startY - expand;
			Vector2 vector2 = new Vector2((float)(num4 * 16), (float)(num2 * 16)) + vector;
			if (IsSpotShimmerFree(entity, vector2, allowSolidTop))
			{
				return vector2;
			}
			vector2 = new Vector2((float)((startX + i) * 16), (float)(num2 * 16)) + vector;
			if (IsSpotShimmerFree(entity, vector2, allowSolidTop))
			{
				return vector2;
			}
			int num5 = startX - i;
			num2 = startY + expand;
			vector2 = new Vector2((float)(num5 * 16), (float)(num2 * 16)) + vector;
			if (IsSpotShimmerFree(entity, vector2, allowSolidTop))
			{
				return vector2;
			}
			vector2 = new Vector2((float)((startX + i) * 16), (float)(num2 * 16)) + vector;
			if (IsSpotShimmerFree(entity, vector2, allowSolidTop))
			{
				return vector2;
			}
		}
		for (int j = 0; j < expand; j++)
		{
			int num6 = startX - expand;
			int num3 = startY - j;
			Vector2 vector3 = new Vector2((float)(num6 * 16), (float)(num3 * 16)) + vector;
			if (IsSpotShimmerFree(entity, vector3, allowSolidTop))
			{
				return vector3;
			}
			vector3 = new Vector2((float)((startX + expand) * 16), (float)(num3 * 16)) + vector;
			if (IsSpotShimmerFree(entity, vector3, allowSolidTop))
			{
				return vector3;
			}
			int num7 = startX - expand;
			num3 = startY + j;
			vector3 = new Vector2((float)(num7 * 16), (float)(num3 * 16)) + vector;
			if (IsSpotShimmerFree(entity, vector3, allowSolidTop))
			{
				return vector3;
			}
			vector3 = new Vector2((float)((startX + expand) * 16), (float)(num3 * 16)) + vector;
			if (IsSpotShimmerFree(entity, vector3, allowSolidTop))
			{
				return vector3;
			}
		}
		return null;
	}

	private static bool IsSpotShimmerFree(Entity entity, Vector2 landingPosition, bool allowSolidTop)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		if (Collision.SolidCollision(landingPosition, entity.width, entity.height))
		{
			return false;
		}
		if (!Collision.SolidCollision(landingPosition + new Vector2(0f, (float)entity.height), entity.width, 100, allowSolidTop))
		{
			return false;
		}
		if (Collision.WetCollision(landingPosition, entity.width, entity.height + 100) && Collision.shimmer)
		{
			return false;
		}
		return true;
	}
}
