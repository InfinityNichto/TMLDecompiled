using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.DataStructures;

namespace Terraria.Graphics.Shaders;

public class MiscShaderData : ShaderData
{
	private Vector3 _uColor = Vector3.One;

	private Vector3 _uSecondaryColor = Vector3.One;

	private float _uSaturation = 1f;

	private float _uOpacity = 1f;

	private Asset<Texture2D> _uImage0;

	private Asset<Texture2D> _uImage1;

	private Asset<Texture2D> _uImage2;

	private bool _useProjectionMatrix;

	private Vector4 _shaderSpecificData = Vector4.Zero;

	private SamplerState _customSamplerState;

	public MiscShaderData(Ref<Effect> shader, string passName)
		: base(shader, passName)
	{
	}//IL_0001: Unknown result type (might be due to invalid IL or missing references)
	//IL_0006: Unknown result type (might be due to invalid IL or missing references)
	//IL_000c: Unknown result type (might be due to invalid IL or missing references)
	//IL_0011: Unknown result type (might be due to invalid IL or missing references)
	//IL_002d: Unknown result type (might be due to invalid IL or missing references)
	//IL_0032: Unknown result type (might be due to invalid IL or missing references)


	public virtual void Apply(DrawData? drawData = null)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0174: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_026f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0359: Unknown result type (might be due to invalid IL or missing references)
		base.Shader.Parameters["uColor"].SetValue(_uColor);
		base.Shader.Parameters["uSaturation"].SetValue(_uSaturation);
		base.Shader.Parameters["uSecondaryColor"].SetValue(_uSecondaryColor);
		base.Shader.Parameters["uTime"].SetValue(Main.GlobalTimeWrappedHourly);
		base.Shader.Parameters["uOpacity"].SetValue(_uOpacity);
		base.Shader.Parameters["uShaderSpecificData"].SetValue(_shaderSpecificData);
		if (drawData.HasValue)
		{
			DrawData value = drawData.Value;
			Vector4 value2 = Vector4.Zero;
			if (drawData.Value.sourceRect.HasValue)
			{
				((Vector4)(ref value2))._002Ector((float)value.sourceRect.Value.X, (float)value.sourceRect.Value.Y, (float)value.sourceRect.Value.Width, (float)value.sourceRect.Value.Height);
			}
			base.Shader.Parameters["uSourceRect"].SetValue(value2);
			base.Shader.Parameters["uWorldPosition"].SetValue(Main.screenPosition + value.position);
			base.Shader.Parameters["uImageSize0"].SetValue(new Vector2((float)value.texture.Width, (float)value.texture.Height));
		}
		else
		{
			base.Shader.Parameters["uSourceRect"].SetValue(new Vector4(0f, 0f, 4f, 4f));
		}
		SamplerState value3 = SamplerState.LinearWrap;
		if (_customSamplerState != null)
		{
			value3 = _customSamplerState;
		}
		if (_uImage0 != null)
		{
			Main.graphics.GraphicsDevice.Textures[0] = (Texture)(object)_uImage0.Value;
			Main.graphics.GraphicsDevice.SamplerStates[0] = value3;
			base.Shader.Parameters["uImageSize0"].SetValue(new Vector2((float)_uImage0.Width(), (float)_uImage0.Height()));
		}
		if (_uImage1 != null)
		{
			Main.graphics.GraphicsDevice.Textures[1] = (Texture)(object)_uImage1.Value;
			Main.graphics.GraphicsDevice.SamplerStates[1] = value3;
			base.Shader.Parameters["uImageSize1"].SetValue(new Vector2((float)_uImage1.Width(), (float)_uImage1.Height()));
		}
		if (_uImage2 != null)
		{
			Main.graphics.GraphicsDevice.Textures[2] = (Texture)(object)_uImage2.Value;
			Main.graphics.GraphicsDevice.SamplerStates[2] = value3;
			base.Shader.Parameters["uImageSize2"].SetValue(new Vector2((float)_uImage2.Width(), (float)_uImage2.Height()));
		}
		_ = _useProjectionMatrix;
		base.Apply();
	}

	public MiscShaderData UseColor(float r, float g, float b)
	{
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		return this.UseColor(new Vector3(r, g, b));
	}

	public MiscShaderData UseColor(Color color)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		return UseColor(((Color)(ref color)).ToVector3());
	}

	public MiscShaderData UseColor(Vector3 color)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		_uColor = color;
		return this;
	}

	public MiscShaderData UseSamplerState(SamplerState state)
	{
		_customSamplerState = state;
		return this;
	}

	public MiscShaderData UseImage0(string path)
	{
		_uImage0 = Main.Assets.Request<Texture2D>(path);
		return this;
	}

	public MiscShaderData UseImage1(string path)
	{
		_uImage1 = Main.Assets.Request<Texture2D>(path);
		return this;
	}

	public MiscShaderData UseImage2(string path)
	{
		_uImage2 = Main.Assets.Request<Texture2D>(path);
		return this;
	}

	public MiscShaderData UseImage0(Asset<Texture2D> asset)
	{
		_uImage0 = asset;
		return this;
	}

	public MiscShaderData UseImage1(Asset<Texture2D> asset)
	{
		_uImage1 = asset;
		return this;
	}

	public MiscShaderData UseImage2(Asset<Texture2D> asset)
	{
		_uImage2 = asset;
		return this;
	}

	private static bool IsPowerOfTwo(int n)
	{
		return (int)Math.Ceiling(Math.Log(n) / Math.Log(2.0)) == (int)Math.Floor(Math.Log(n) / Math.Log(2.0));
	}

	public MiscShaderData UseOpacity(float alpha)
	{
		_uOpacity = alpha;
		return this;
	}

	public MiscShaderData UseSecondaryColor(float r, float g, float b)
	{
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		return this.UseSecondaryColor(new Vector3(r, g, b));
	}

	public MiscShaderData UseSecondaryColor(Color color)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		return UseSecondaryColor(((Color)(ref color)).ToVector3());
	}

	public MiscShaderData UseSecondaryColor(Vector3 color)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		_uSecondaryColor = color;
		return this;
	}

	public MiscShaderData UseProjectionMatrix(bool doUse)
	{
		_useProjectionMatrix = doUse;
		return this;
	}

	public MiscShaderData UseSaturation(float saturation)
	{
		_uSaturation = saturation;
		return this;
	}

	public virtual MiscShaderData GetSecondaryShader(Entity entity)
	{
		return this;
	}

	public MiscShaderData UseShaderSpecificData(Vector4 specificData)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		_shaderSpecificData = specificData;
		return this;
	}
}
