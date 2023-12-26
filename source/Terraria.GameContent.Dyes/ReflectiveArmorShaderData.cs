using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;

namespace Terraria.GameContent.Dyes;

public class ReflectiveArmorShaderData : ArmorShaderData
{
	public ReflectiveArmorShaderData(Ref<Effect> shader, string passName)
		: base(shader, passName)
	{
	}

	public override void Apply(Entity entity, DrawData? drawData)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		//IL_017c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0241: Unknown result type (might be due to invalid IL or missing references)
		if (entity == null)
		{
			base.Shader.Parameters["uLightSource"].SetValue(Vector3.Zero);
		}
		else
		{
			float num = 0f;
			if (drawData.HasValue)
			{
				num = drawData.Value.rotation;
			}
			Vector2 position = entity.position;
			float num2 = entity.width;
			float num3 = entity.height;
			Vector2 val = position + new Vector2(num2, num3) * 0.1f;
			num2 *= 0.8f;
			num3 *= 0.8f;
			Vector3 subLight = Lighting.GetSubLight(val + new Vector2(num2 * 0.5f, 0f));
			Vector3 subLight2 = Lighting.GetSubLight(val + new Vector2(0f, num3 * 0.5f));
			Vector3 subLight3 = Lighting.GetSubLight(val + new Vector2(num2, num3 * 0.5f));
			Vector3 subLight4 = Lighting.GetSubLight(val + new Vector2(num2 * 0.5f, num3));
			float num4 = subLight.X + subLight.Y + subLight.Z;
			float num5 = subLight2.X + subLight2.Y + subLight2.Z;
			float num6 = subLight3.X + subLight3.Y + subLight3.Z;
			float num7 = subLight4.X + subLight4.Y + subLight4.Z;
			Vector2 spinningpoint = default(Vector2);
			((Vector2)(ref spinningpoint))._002Ector(num6 - num5, num7 - num4);
			float num8 = ((Vector2)(ref spinningpoint)).Length();
			if (num8 > 1f)
			{
				num8 = 1f;
				spinningpoint /= num8;
			}
			if (entity.direction == -1)
			{
				spinningpoint.X *= -1f;
			}
			spinningpoint = spinningpoint.RotatedBy(0f - num);
			Vector3 value = default(Vector3);
			((Vector3)(ref value))._002Ector(spinningpoint, 1f - (spinningpoint.X * spinningpoint.X + spinningpoint.Y * spinningpoint.Y));
			value.X *= 2f;
			value.Y -= 0.15f;
			value.Y *= 2f;
			((Vector3)(ref value)).Normalize();
			value.Z *= 0.6f;
			base.Shader.Parameters["uLightSource"].SetValue(value);
		}
		base.Apply(entity, drawData);
	}
}
