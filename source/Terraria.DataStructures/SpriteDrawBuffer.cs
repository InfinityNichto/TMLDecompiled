using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Graphics;

namespace Terraria.DataStructures;

public class SpriteDrawBuffer
{
	private GraphicsDevice graphicsDevice;

	private DynamicVertexBuffer vertexBuffer;

	private IndexBuffer indexBuffer;

	private VertexPositionColorTexture[] vertices = (VertexPositionColorTexture[])(object)new VertexPositionColorTexture[0];

	private Texture[] textures = (Texture[])(object)new Texture[0];

	private int maxSprites;

	private int vertexCount;

	private VertexBufferBinding[] preBindVertexBuffers;

	private IndexBuffer preBindIndexBuffer;

	public SpriteDrawBuffer(GraphicsDevice graphicsDevice, int defaultSize)
	{
		this.graphicsDevice = graphicsDevice;
		maxSprites = defaultSize;
		CreateBuffers();
	}

	public void CheckGraphicsDevice(GraphicsDevice graphicsDevice)
	{
		if (this.graphicsDevice != graphicsDevice)
		{
			this.graphicsDevice = graphicsDevice;
			CreateBuffers();
		}
	}

	private void CreateBuffers()
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Expected O, but got Unknown
		if (vertexBuffer != null)
		{
			((GraphicsResource)vertexBuffer).Dispose();
		}
		vertexBuffer = new DynamicVertexBuffer(graphicsDevice, typeof(VertexPositionColorTexture), maxSprites * 4, (BufferUsage)1);
		if (indexBuffer != null)
		{
			((GraphicsResource)indexBuffer).Dispose();
		}
		indexBuffer = new IndexBuffer(graphicsDevice, typeof(ushort), maxSprites * 6, (BufferUsage)1);
		indexBuffer.SetData<ushort>(GenIndexBuffer(maxSprites));
		Array.Resize(ref vertices, maxSprites * 6);
		Array.Resize(ref textures, maxSprites);
	}

	private static ushort[] GenIndexBuffer(int maxSprites)
	{
		ushort[] array = new ushort[maxSprites * 6];
		int num = 0;
		ushort num2 = 0;
		while (num < maxSprites)
		{
			array[num++] = num2;
			array[num++] = (ushort)(num2 + 1);
			array[num++] = (ushort)(num2 + 2);
			array[num++] = (ushort)(num2 + 3);
			array[num++] = (ushort)(num2 + 2);
			array[num++] = (ushort)(num2 + 1);
			num2 += 4;
		}
		return array;
	}

	public void UploadAndBind()
	{
		if (vertexCount > 0)
		{
			vertexBuffer.SetData<VertexPositionColorTexture>(vertices, 0, vertexCount, (SetDataOptions)1);
		}
		vertexCount = 0;
		Bind();
	}

	public void Bind()
	{
		preBindVertexBuffers = graphicsDevice.GetVertexBuffers();
		preBindIndexBuffer = graphicsDevice.Indices;
		graphicsDevice.SetVertexBuffer((VertexBuffer)(object)vertexBuffer);
		graphicsDevice.Indices = indexBuffer;
	}

	public void Unbind()
	{
		graphicsDevice.SetVertexBuffers(preBindVertexBuffers);
		graphicsDevice.Indices = preBindIndexBuffer;
		preBindVertexBuffers = null;
		preBindIndexBuffer = null;
	}

	public void DrawRange(int index, int count)
	{
		graphicsDevice.Textures[0] = textures[index];
		graphicsDevice.DrawIndexedPrimitives((PrimitiveType)0, index * 4, 0, count * 4, 0, count * 2);
	}

	public void DrawSingle(int index)
	{
		DrawRange(index, 1);
	}

	public void Draw(Texture2D texture, Vector2 position, VertexColors colors)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		Draw(texture, position, null, colors, 0f, Vector2.Zero, 1f, (SpriteEffects)0);
	}

	public void Draw(Texture2D texture, Rectangle destination, VertexColors colors)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		Draw(texture, destination, null, colors);
	}

	public void Draw(Texture2D texture, Rectangle destination, Rectangle? sourceRectangle, VertexColors colors)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		Draw(texture, destination, sourceRectangle, colors, 0f, Vector2.Zero, (SpriteEffects)0);
	}

	public void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, VertexColors color, float rotation, Vector2 origin, float scale, SpriteEffects effects)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		Draw(texture, position, sourceRectangle, color, rotation, origin, new Vector2(scale, scale), effects);
	}

	public void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, VertexColors colors, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects)
	{
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		float z;
		float w;
		if (sourceRectangle.HasValue)
		{
			z = (float)sourceRectangle.Value.Width * scale.X;
			w = (float)sourceRectangle.Value.Height * scale.Y;
		}
		else
		{
			z = (float)texture.Width * scale.X;
			w = (float)texture.Height * scale.Y;
		}
		Draw(texture, new Vector4(position.X, position.Y, z, w), sourceRectangle, colors, rotation, origin, effects, 0f);
	}

	public void Draw(Texture2D texture, Rectangle destination, Rectangle? sourceRectangle, VertexColors colors, float rotation, Vector2 origin, SpriteEffects effects)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		Draw(texture, new Vector4((float)destination.X, (float)destination.Y, (float)destination.Width, (float)destination.Height), sourceRectangle, colors, rotation, origin, effects, 0f);
	}

	public void Draw(Texture2D texture, Vector4 destinationRectangle, Rectangle? sourceRectangle, VertexColors colors, float rotation, Vector2 origin, SpriteEffects effect, float depth)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		Vector4 sourceRectangle2 = default(Vector4);
		if (sourceRectangle.HasValue)
		{
			sourceRectangle2.X = sourceRectangle.Value.X;
			sourceRectangle2.Y = sourceRectangle.Value.Y;
			sourceRectangle2.Z = sourceRectangle.Value.Width;
			sourceRectangle2.W = sourceRectangle.Value.Height;
		}
		else
		{
			sourceRectangle2.X = 0f;
			sourceRectangle2.Y = 0f;
			sourceRectangle2.Z = texture.Width;
			sourceRectangle2.W = texture.Height;
		}
		Vector2 texCoordTL = default(Vector2);
		texCoordTL.X = sourceRectangle2.X / (float)texture.Width;
		texCoordTL.Y = sourceRectangle2.Y / (float)texture.Height;
		Vector2 texCoordBR = default(Vector2);
		texCoordBR.X = (sourceRectangle2.X + sourceRectangle2.Z) / (float)texture.Width;
		texCoordBR.Y = (sourceRectangle2.Y + sourceRectangle2.W) / (float)texture.Height;
		if ((effect & 2) != 0)
		{
			float y = texCoordBR.Y;
			texCoordBR.Y = texCoordTL.Y;
			texCoordTL.Y = y;
		}
		if ((effect & 1) != 0)
		{
			float x = texCoordBR.X;
			texCoordBR.X = texCoordTL.X;
			texCoordTL.X = x;
		}
		QueueSprite(destinationRectangle, -origin, colors, sourceRectangle2, texCoordTL, texCoordBR, texture, depth, rotation);
	}

	private void QueueSprite(Vector4 destinationRect, Vector2 origin, VertexColors colors, Vector4 sourceRectangle, Vector2 texCoordTL, Vector2 texCoordBR, Texture2D texture, float depth, float rotation)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0190: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		float num = origin.X / sourceRectangle.Z;
		float num6 = origin.Y / sourceRectangle.W;
		float x = destinationRect.X;
		float y = destinationRect.Y;
		float z = destinationRect.Z;
		float w = destinationRect.W;
		float num2 = num * z;
		float num3 = num6 * w;
		float num4;
		float num5;
		if (rotation != 0f)
		{
			num4 = (float)Math.Cos(rotation);
			num5 = (float)Math.Sin(rotation);
		}
		else
		{
			num4 = 1f;
			num5 = 0f;
		}
		if (vertexCount + 4 >= maxSprites * 4)
		{
			maxSprites *= 2;
			CreateBuffers();
		}
		textures[vertexCount / 4] = (Texture)(object)texture;
		PushVertex(new Vector3(x + num2 * num4 - num3 * num5, y + num2 * num5 + num3 * num4, depth), colors.TopLeftColor, texCoordTL);
		PushVertex(new Vector3(x + (num2 + z) * num4 - num3 * num5, y + (num2 + z) * num5 + num3 * num4, depth), colors.TopRightColor, new Vector2(texCoordBR.X, texCoordTL.Y));
		PushVertex(new Vector3(x + num2 * num4 - (num3 + w) * num5, y + num2 * num5 + (num3 + w) * num4, depth), colors.BottomLeftColor, new Vector2(texCoordTL.X, texCoordBR.Y));
		PushVertex(new Vector3(x + (num2 + z) * num4 - (num3 + w) * num5, y + (num2 + z) * num5 + (num3 + w) * num4, depth), colors.BottomRightColor, texCoordBR);
	}

	private void PushVertex(Vector3 pos, Color color, Vector2 texCoord)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		SetVertex(ref vertices[vertexCount++], pos, color, texCoord);
	}

	private static void SetVertex(ref VertexPositionColorTexture vertex, Vector3 pos, Color color, Vector2 texCoord)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		vertex.Position = pos;
		vertex.Color = color;
		vertex.TextureCoordinate = texCoord;
	}
}
