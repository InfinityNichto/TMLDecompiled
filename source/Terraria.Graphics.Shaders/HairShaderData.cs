using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.DataStructures;

namespace Terraria.Graphics.Shaders;

public class HairShaderData : ShaderData
{
	protected Vector3 _uColor = Vector3.One;

	protected Vector3 _uSecondaryColor = Vector3.One;

	protected float _uSaturation = 1f;

	protected float _uOpacity = 1f;

	protected Asset<Texture2D> _uImage;

	protected bool _shaderDisabled;

	private Vector2 _uTargetPosition = Vector2.One;

	public bool ShaderDisabled => _shaderDisabled;

	public HairShaderData(Ref<Effect> shader, string passName)
		: base(shader, passName)
	{
	}//IL_0001: Unknown result type (might be due to invalid IL or missing references)
	//IL_0006: Unknown result type (might be due to invalid IL or missing references)
	//IL_000c: Unknown result type (might be due to invalid IL or missing references)
	//IL_0011: Unknown result type (might be due to invalid IL or missing references)
	//IL_002d: Unknown result type (might be due to invalid IL or missing references)
	//IL_0032: Unknown result type (might be due to invalid IL or missing references)


	public virtual void Apply(Player player, DrawData? drawData = null)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_019f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0233: Unknown result type (might be due to invalid IL or missing references)
		if (!_shaderDisabled)
		{
			base.Shader.Parameters["uColor"].SetValue(_uColor);
			base.Shader.Parameters["uSaturation"].SetValue(_uSaturation);
			base.Shader.Parameters["uSecondaryColor"].SetValue(_uSecondaryColor);
			base.Shader.Parameters["uTime"].SetValue(Main.GlobalTimeWrappedHourly);
			base.Shader.Parameters["uOpacity"].SetValue(_uOpacity);
			base.Shader.Parameters["uTargetPosition"].SetValue(_uTargetPosition);
			if (drawData.HasValue)
			{
				DrawData value = drawData.Value;
				Vector4 value2 = default(Vector4);
				((Vector4)(ref value2))._002Ector((float)value.sourceRect.Value.X, (float)value.sourceRect.Value.Y, (float)value.sourceRect.Value.Width, (float)value.sourceRect.Value.Height);
				base.Shader.Parameters["uSourceRect"].SetValue(value2);
				base.Shader.Parameters["uWorldPosition"].SetValue(Main.screenPosition + value.position);
				base.Shader.Parameters["uImageSize0"].SetValue(new Vector2((float)value.texture.Width, (float)value.texture.Height));
			}
			else
			{
				base.Shader.Parameters["uSourceRect"].SetValue(new Vector4(0f, 0f, 4f, 4f));
			}
			if (_uImage != null)
			{
				Main.graphics.GraphicsDevice.Textures[1] = (Texture)(object)_uImage.Value;
				base.Shader.Parameters["uImageSize1"].SetValue(new Vector2((float)_uImage.Width(), (float)_uImage.Height()));
			}
			if (player != null)
			{
				base.Shader.Parameters["uDirection"].SetValue((float)player.direction);
			}
			Apply();
		}
	}

	public virtual Color GetColor(Player player, Color lightColor)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		return new Color(((Color)(ref lightColor)).ToVector4() * ((Color)(ref player.hairColor)).ToVector4());
	}

	public HairShaderData UseColor(float r, float g, float b)
	{
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		return this.UseColor(new Vector3(r, g, b));
	}

	public HairShaderData UseColor(Color color)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		return UseColor(((Color)(ref color)).ToVector3());
	}

	public HairShaderData UseColor(Vector3 color)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		_uColor = color;
		return this;
	}

	public HairShaderData UseImage(string path)
	{
		_uImage = Main.Assets.Request<Texture2D>(path);
		return this;
	}

	public HairShaderData UseImage(Asset<Texture2D> asset)
	{
		_uImage = asset;
		return this;
	}

	public HairShaderData UseOpacity(float alpha)
	{
		_uOpacity = alpha;
		return this;
	}

	public HairShaderData UseSecondaryColor(float r, float g, float b)
	{
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		return this.UseSecondaryColor(new Vector3(r, g, b));
	}

	public HairShaderData UseSecondaryColor(Color color)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		return UseSecondaryColor(((Color)(ref color)).ToVector3());
	}

	public HairShaderData UseSecondaryColor(Vector3 color)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		_uSecondaryColor = color;
		return this;
	}

	public HairShaderData UseSaturation(float saturation)
	{
		_uSaturation = saturation;
		return this;
	}

	public HairShaderData UseTargetPosition(Vector2 position)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		_uTargetPosition = position;
		return this;
	}
}
