using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;

namespace Terraria.Graphics.Shaders;

public class ScreenShaderData : ShaderData
{
	private Vector3 _uColor = Vector3.One;

	private Vector3 _uSecondaryColor = Vector3.One;

	private float _uOpacity = 1f;

	private float _globalOpacity = 1f;

	private float _uIntensity = 1f;

	private Vector2 _uTargetPosition = Vector2.One;

	private Vector2 _uDirection = new Vector2(0f, 1f);

	private float _uProgress;

	private Vector2 _uImageOffset = Vector2.Zero;

	private Asset<Texture2D>[] _uAssetImages = new Asset<Texture2D>[3];

	private Texture2D[] _uCustomImages = (Texture2D[])(object)new Texture2D[3];

	private SamplerState[] _samplerStates = (SamplerState[])(object)new SamplerState[3];

	private Vector2[] _imageScales = (Vector2[])(object)new Vector2[3]
	{
		Vector2.One,
		Vector2.One,
		Vector2.One
	};

	public float Intensity => _uIntensity;

	public float CombinedOpacity => _uOpacity * _globalOpacity;

	public ScreenShaderData(string passName)
		: base(Main.ScreenShaderRef, passName)
	{
	}//IL_0001: Unknown result type (might be due to invalid IL or missing references)
	//IL_0006: Unknown result type (might be due to invalid IL or missing references)
	//IL_000c: Unknown result type (might be due to invalid IL or missing references)
	//IL_0011: Unknown result type (might be due to invalid IL or missing references)
	//IL_0038: Unknown result type (might be due to invalid IL or missing references)
	//IL_003d: Unknown result type (might be due to invalid IL or missing references)
	//IL_004d: Unknown result type (might be due to invalid IL or missing references)
	//IL_0052: Unknown result type (might be due to invalid IL or missing references)
	//IL_0058: Unknown result type (might be due to invalid IL or missing references)
	//IL_005d: Unknown result type (might be due to invalid IL or missing references)
	//IL_008f: Unknown result type (might be due to invalid IL or missing references)
	//IL_0094: Unknown result type (might be due to invalid IL or missing references)
	//IL_009b: Unknown result type (might be due to invalid IL or missing references)
	//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
	//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
	//IL_00ac: Unknown result type (might be due to invalid IL or missing references)


	public ScreenShaderData(Ref<Effect> shader, string passName)
		: base(shader, passName)
	{
	}//IL_0001: Unknown result type (might be due to invalid IL or missing references)
	//IL_0006: Unknown result type (might be due to invalid IL or missing references)
	//IL_000c: Unknown result type (might be due to invalid IL or missing references)
	//IL_0011: Unknown result type (might be due to invalid IL or missing references)
	//IL_0038: Unknown result type (might be due to invalid IL or missing references)
	//IL_003d: Unknown result type (might be due to invalid IL or missing references)
	//IL_004d: Unknown result type (might be due to invalid IL or missing references)
	//IL_0052: Unknown result type (might be due to invalid IL or missing references)
	//IL_0058: Unknown result type (might be due to invalid IL or missing references)
	//IL_005d: Unknown result type (might be due to invalid IL or missing references)
	//IL_008f: Unknown result type (might be due to invalid IL or missing references)
	//IL_0094: Unknown result type (might be due to invalid IL or missing references)
	//IL_009b: Unknown result type (might be due to invalid IL or missing references)
	//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
	//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
	//IL_00ac: Unknown result type (might be due to invalid IL or missing references)


	public virtual void Update(GameTime gameTime)
	{
	}

	public override void Apply()
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_031e: Unknown result type (might be due to invalid IL or missing references)
		//IL_032b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0330: Unknown result type (might be due to invalid IL or missing references)
		Vector2 vector = default(Vector2);
		((Vector2)(ref vector))._002Ector((float)Main.offScreenRange, (float)Main.offScreenRange);
		Vector2 value = new Vector2((float)Main.screenWidth, (float)Main.screenHeight) / Main.GameViewMatrix.Zoom;
		Vector2 vector2 = new Vector2((float)Main.screenWidth, (float)Main.screenHeight) * 0.5f;
		Vector2 vector3 = Main.screenPosition + vector2 * (Vector2.One - Vector2.One / Main.GameViewMatrix.Zoom);
		base.Shader.Parameters["uColor"].SetValue(_uColor);
		base.Shader.Parameters["uOpacity"].SetValue(CombinedOpacity);
		base.Shader.Parameters["uSecondaryColor"].SetValue(_uSecondaryColor);
		base.Shader.Parameters["uTime"].SetValue(Main.GlobalTimeWrappedHourly);
		base.Shader.Parameters["uScreenResolution"].SetValue(value);
		base.Shader.Parameters["uScreenPosition"].SetValue(vector3 - vector);
		base.Shader.Parameters["uTargetPosition"].SetValue(_uTargetPosition - vector);
		base.Shader.Parameters["uImageOffset"].SetValue(_uImageOffset);
		base.Shader.Parameters["uIntensity"].SetValue(_uIntensity);
		base.Shader.Parameters["uProgress"].SetValue(_uProgress);
		base.Shader.Parameters["uDirection"].SetValue(_uDirection);
		base.Shader.Parameters["uZoom"].SetValue(Main.GameViewMatrix.Zoom);
		for (int i = 0; i < _uAssetImages.Length; i++)
		{
			Texture2D texture2D = _uCustomImages[i];
			if (_uAssetImages[i] != null && _uAssetImages[i].IsLoaded)
			{
				texture2D = _uAssetImages[i].Value;
			}
			if (texture2D != null)
			{
				Main.graphics.GraphicsDevice.Textures[i + 1] = (Texture)(object)texture2D;
				int width = texture2D.Width;
				int height = texture2D.Height;
				if (_samplerStates[i] != null)
				{
					Main.graphics.GraphicsDevice.SamplerStates[i + 1] = _samplerStates[i];
				}
				else if (Utils.IsPowerOfTwo(width) && Utils.IsPowerOfTwo(height))
				{
					Main.graphics.GraphicsDevice.SamplerStates[i + 1] = SamplerState.LinearWrap;
				}
				else
				{
					Main.graphics.GraphicsDevice.SamplerStates[i + 1] = SamplerState.AnisotropicClamp;
				}
				base.Shader.Parameters["uImageSize" + (i + 1)].SetValue(new Vector2((float)width, (float)height) * _imageScales[i]);
			}
		}
		base.Apply();
	}

	public ScreenShaderData UseImageOffset(Vector2 offset)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		_uImageOffset = offset;
		return this;
	}

	public ScreenShaderData UseIntensity(float intensity)
	{
		_uIntensity = intensity;
		return this;
	}

	public ScreenShaderData UseColor(float r, float g, float b)
	{
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		return this.UseColor(new Vector3(r, g, b));
	}

	public ScreenShaderData UseProgress(float progress)
	{
		_uProgress = progress;
		return this;
	}

	public ScreenShaderData UseImage(Asset<Texture2D> image, int index = 0, SamplerState samplerState = null)
	{
		_samplerStates[index] = samplerState;
		_uAssetImages[index] = image;
		_uCustomImages[index] = null;
		return this;
	}

	public ScreenShaderData UseImage(Texture2D image, int index = 0, SamplerState samplerState = null)
	{
		_samplerStates[index] = samplerState;
		_uAssetImages[index] = null;
		_uCustomImages[index] = image;
		return this;
	}

	public ScreenShaderData UseImage(string path, int index = 0, SamplerState samplerState = null)
	{
		_uAssetImages[index] = Main.Assets.Request<Texture2D>(path);
		_uCustomImages[index] = null;
		_samplerStates[index] = samplerState;
		return this;
	}

	public ScreenShaderData UseColor(Color color)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		return UseColor(((Color)(ref color)).ToVector3());
	}

	public ScreenShaderData UseColor(Vector3 color)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		_uColor = color;
		return this;
	}

	public ScreenShaderData UseDirection(Vector2 direction)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		_uDirection = direction;
		return this;
	}

	public ScreenShaderData UseGlobalOpacity(float opacity)
	{
		_globalOpacity = opacity;
		return this;
	}

	public ScreenShaderData UseTargetPosition(Vector2 position)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		_uTargetPosition = position;
		return this;
	}

	public ScreenShaderData UseSecondaryColor(float r, float g, float b)
	{
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		return this.UseSecondaryColor(new Vector3(r, g, b));
	}

	public ScreenShaderData UseSecondaryColor(Color color)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		return UseSecondaryColor(((Color)(ref color)).ToVector3());
	}

	public ScreenShaderData UseSecondaryColor(Vector3 color)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		_uSecondaryColor = color;
		return this;
	}

	public ScreenShaderData UseOpacity(float opacity)
	{
		_uOpacity = opacity;
		return this;
	}

	public ScreenShaderData UseImageScale(Vector2 scale, int index = 0)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		_imageScales[index] = scale;
		return this;
	}

	public virtual ScreenShaderData GetSecondaryShader(Player player)
	{
		return this;
	}
}
