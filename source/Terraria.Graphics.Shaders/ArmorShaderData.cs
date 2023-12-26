using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.DataStructures;

namespace Terraria.Graphics.Shaders;

public class ArmorShaderData : ShaderData
{
	private Vector3 _uColor = Vector3.One;

	private Vector3 _uSecondaryColor = Vector3.One;

	private float _uSaturation = 1f;

	private float _uOpacity = 1f;

	private Asset<Texture2D> _uImage;

	private Vector2 _uTargetPosition = Vector2.One;

	public ArmorShaderData(Ref<Effect> shader, string passName)
		: base(shader, passName)
	{
	}//IL_0001: Unknown result type (might be due to invalid IL or missing references)
	//IL_0006: Unknown result type (might be due to invalid IL or missing references)
	//IL_000c: Unknown result type (might be due to invalid IL or missing references)
	//IL_0011: Unknown result type (might be due to invalid IL or missing references)
	//IL_002d: Unknown result type (might be due to invalid IL or missing references)
	//IL_0032: Unknown result type (might be due to invalid IL or missing references)


	public virtual void Apply(Entity entity, DrawData? drawData = null)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_0368: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_016d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_021c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0242: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0406: Unknown result type (might be due to invalid IL or missing references)
		//IL_0281: Unknown result type (might be due to invalid IL or missing references)
		base.Shader.Parameters["uColor"].SetValue(_uColor);
		base.Shader.Parameters["uSaturation"].SetValue(_uSaturation);
		base.Shader.Parameters["uSecondaryColor"].SetValue(_uSecondaryColor);
		base.Shader.Parameters["uTime"].SetValue(Main.GlobalTimeWrappedHourly);
		base.Shader.Parameters["uOpacity"].SetValue(_uOpacity);
		base.Shader.Parameters["uTargetPosition"].SetValue(_uTargetPosition);
		if (drawData.HasValue)
		{
			DrawData value = drawData.Value;
			Vector4 value2 = ((!value.sourceRect.HasValue) ? new Vector4(0f, 0f, (float)value.texture.Width, (float)value.texture.Height) : new Vector4((float)value.sourceRect.Value.X, (float)value.sourceRect.Value.Y, (float)value.sourceRect.Value.Width, (float)value.sourceRect.Value.Height));
			base.Shader.Parameters["uSourceRect"].SetValue(value2);
			base.Shader.Parameters["uLegacyArmorSourceRect"].SetValue(value2);
			base.Shader.Parameters["uWorldPosition"].SetValue(Main.screenPosition + value.position);
			base.Shader.Parameters["uImageSize0"].SetValue(new Vector2((float)value.texture.Width, (float)value.texture.Height));
			base.Shader.Parameters["uLegacyArmorSheetSize"].SetValue(new Vector2((float)value.texture.Width, (float)value.texture.Height));
			base.Shader.Parameters["uRotation"].SetValue(value.rotation * (((Enum)value.effect).HasFlag((Enum)(object)(SpriteEffects)1) ? (-1f) : 1f));
			base.Shader.Parameters["uDirection"].SetValue((!((Enum)value.effect).HasFlag((Enum)(object)(SpriteEffects)1)) ? 1 : (-1));
		}
		else
		{
			Vector4 value3 = default(Vector4);
			((Vector4)(ref value3))._002Ector(0f, 0f, 4f, 4f);
			base.Shader.Parameters["uSourceRect"].SetValue(value3);
			base.Shader.Parameters["uLegacyArmorSourceRect"].SetValue(value3);
			base.Shader.Parameters["uRotation"].SetValue(0f);
		}
		if (_uImage != null)
		{
			Main.graphics.GraphicsDevice.Textures[1] = (Texture)(object)_uImage.Value;
			base.Shader.Parameters["uImageSize1"].SetValue(new Vector2((float)_uImage.Width(), (float)_uImage.Height()));
		}
		if (entity != null)
		{
			base.Shader.Parameters["uDirection"].SetValue((float)entity.direction);
		}
		if (entity is Player { bodyFrame: var bodyFrame })
		{
			base.Shader.Parameters["uLegacyArmorSourceRect"].SetValue(new Vector4((float)bodyFrame.X, (float)bodyFrame.Y, (float)bodyFrame.Width, (float)bodyFrame.Height));
			base.Shader.Parameters["uLegacyArmorSheetSize"].SetValue(new Vector2(40f, 1120f));
		}
		Apply();
	}

	public ArmorShaderData UseColor(float r, float g, float b)
	{
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		return this.UseColor(new Vector3(r, g, b));
	}

	public ArmorShaderData UseColor(Color color)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		return UseColor(((Color)(ref color)).ToVector3());
	}

	public ArmorShaderData UseColor(Vector3 color)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		_uColor = color;
		return this;
	}

	public ArmorShaderData UseImage(string path)
	{
		_uImage = Main.Assets.Request<Texture2D>(path);
		return this;
	}

	public ArmorShaderData UseImage(Asset<Texture2D> asset)
	{
		_uImage = asset;
		return this;
	}

	public ArmorShaderData UseOpacity(float alpha)
	{
		_uOpacity = alpha;
		return this;
	}

	public ArmorShaderData UseTargetPosition(Vector2 position)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		_uTargetPosition = position;
		return this;
	}

	public ArmorShaderData UseSecondaryColor(float r, float g, float b)
	{
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		return this.UseSecondaryColor(new Vector3(r, g, b));
	}

	public ArmorShaderData UseSecondaryColor(Color color)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		return UseSecondaryColor(((Color)(ref color)).ToVector3());
	}

	public ArmorShaderData UseSecondaryColor(Vector3 color)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		_uSecondaryColor = color;
		return this;
	}

	public ArmorShaderData UseSaturation(float saturation)
	{
		_uSaturation = saturation;
		return this;
	}

	public virtual ArmorShaderData GetSecondaryShader(Entity entity)
	{
		return this;
	}
}
