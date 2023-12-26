using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria.Graphics;

public class TileBatch : IDisposable
{
	private class TextureComparer : IComparer<BatchDrawInfo>
	{
		public static TextureComparer Instance = new TextureComparer();

		public int Compare(BatchDrawInfo info1, BatchDrawInfo info2)
		{
			return ((object)info1.Texture).GetHashCode().CompareTo(((object)info2.Texture).GetHashCode());
		}
	}

	private struct BatchDrawInfo
	{
		public static readonly BatchDrawInfo Empty = new BatchDrawInfo(null, 0, 0);

		public readonly Texture2D Texture;

		public readonly int Index;

		public int Count;

		public BatchDrawInfo(Texture2D texture, int index, int count)
		{
			Texture = texture;
			Index = index;
			Count = count;
		}

		public BatchDrawInfo(Texture2D texture)
		{
			Texture = texture;
			Index = 0;
			Count = 0;
		}
	}

	private class BatchDrawGroup
	{
		public VertexPositionColorTexture[] VertexArray;

		public BatchDrawInfo[] BatchDraws;

		public int BatchDrawCount;

		public int SpriteCount;

		public int VertexCount => SpriteCount * 4;

		public BatchDrawGroup()
		{
			VertexArray = (VertexPositionColorTexture[])(object)new VertexPositionColorTexture[8192];
			BatchDraws = new BatchDrawInfo[2048];
			BatchDrawCount = 0;
			SpriteCount = 0;
		}

		public void Reset()
		{
			BatchDrawCount = 0;
			SpriteCount = 0;
		}

		public void AddBatch(BatchDrawInfo batch)
		{
			BatchDraws[BatchDrawCount++] = batch;
		}
	}

	private SpriteSortMode _sortMode;

	private const int MAX_SPRITES = 2048;

	private const int MAX_VERTICES = 8192;

	private const int MAX_INDICES = 12288;

	private const int INITIAL_BATCH_DRAW_GROUP_COUNT = 32;

	private short[] _sortedIndexData = new short[12288];

	private DynamicIndexBuffer _sortedIndexBuffer;

	private int _lastBatchDrawGroupIndex;

	private BatchDrawInfo _currentBatchDrawInfo;

	private List<BatchDrawGroup> _batchDrawGroups = new List<BatchDrawGroup>();

	private readonly GraphicsDevice _graphicsDevice;

	private SpriteBatch _spriteBatch;

	public void Begin(RasterizerState rasterizer, Matrix transformation)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		Begin((SpriteSortMode)0, null, null, null, rasterizer, null, transformation);
	}

	public void Begin(Matrix transformation)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		Begin((SpriteSortMode)0, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, transformation);
	}

	public void Begin()
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		Begin((SpriteSortMode)0, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Matrix.Identity);
	}

	public void Begin(SpriteSortMode sortMode, BlendState blendState)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		Begin(sortMode, blendState, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Matrix.Identity);
	}

	public void Begin(SpriteSortMode sortMode, BlendState blendState, SamplerState samplerState, DepthStencilState depthStencilState, RasterizerState rasterizerState)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		Begin(sortMode, blendState, samplerState, depthStencilState, rasterizerState, null, Matrix.Identity);
	}

	public void Begin(SpriteSortMode sortMode, BlendState blendState, SamplerState samplerState, DepthStencilState depthStencilState, RasterizerState rasterizerState, Effect effect)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		Begin(sortMode, blendState, samplerState, depthStencilState, rasterizerState, effect, Matrix.Identity);
	}

	public void Begin(SpriteSortMode sortMode, BlendState blendState, SamplerState samplerState, DepthStencilState depthStencilState, RasterizerState rasterizerState, Effect effect, Matrix transformationMatrix)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Invalid comparison between Unknown and I4
		if ((int)sortMode != 0 && (int)sortMode != 2)
		{
			throw new NotImplementedException("TileBatch only supports SpriteSortMode.Deferred and SpriteSortMode.Texture.");
		}
		_sortMode = sortMode;
		_spriteBatch.Begin(sortMode, blendState, samplerState, depthStencilState, rasterizerState, effect, transformationMatrix);
		_spriteBatch.End();
	}

	public void End()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Invalid comparison between Unknown and I4
		if ((int)_sortMode == 0)
		{
			DrawBatch();
		}
		else if ((int)_sortMode == 2)
		{
			SortedDrawBatch();
		}
	}

	public void Draw(Texture2D texture, Vector2 position, VertexColors colors)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		Draw(texture, position, null, colors, Vector2.Zero, 1f, (SpriteEffects)0);
	}

	public void Draw(Texture2D texture, Vector4 destination, VertexColors colors)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		InternalDraw(texture, destination, null, colors, 0f, Vector2.Zero, (SpriteEffects)0, 0f);
	}

	public void Draw(Texture2D texture, Vector4 destination, Rectangle? sourceRectangle, VertexColors colors)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		InternalDraw(texture, destination, sourceRectangle, colors, 0f, Vector2.Zero, (SpriteEffects)0, 0f);
	}

	public void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, VertexColors colors, Vector2 origin, float scale, SpriteEffects effects)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		float z;
		float w;
		if (sourceRectangle.HasValue)
		{
			z = (float)sourceRectangle.Value.Width * scale;
			w = (float)sourceRectangle.Value.Height * scale;
		}
		else
		{
			z = (float)texture.Width * scale;
			w = (float)texture.Height * scale;
		}
		InternalDraw(texture, new Vector4(position.X, position.Y, z, w), sourceRectangle, colors, 0f, origin * scale, effects, 0f);
	}

	public void Draw(Texture2D texture, Vector4 destination, Rectangle? sourceRectangle, VertexColors colors, Vector2 origin, SpriteEffects effects, float rotation)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		InternalDraw(texture, destination, sourceRectangle, colors, rotation, origin, effects, 0f);
	}

	internal void InternalDraw(Texture2D texture, Vector4 destinationRectangle, Rectangle? sourceRectangle, VertexColors colors, float rotation, Vector2 origin, SpriteEffects effect, float depth)
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

	public TileBatch(GraphicsDevice graphicsDevice)
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Expected O, but got Unknown
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Expected O, but got Unknown
		_spriteBatch = new SpriteBatch(graphicsDevice);
		_graphicsDevice = graphicsDevice;
		for (int i = 0; i < 32; i++)
		{
			_batchDrawGroups.Add(new BatchDrawGroup());
		}
		_sortedIndexBuffer = new DynamicIndexBuffer(graphicsDevice, (IndexElementSize)0, 12288, (BufferUsage)1);
	}

	~TileBatch()
	{
		Dispose();
	}

	public void Dispose()
	{
		((GraphicsResource)_sortedIndexBuffer).Dispose();
		((GraphicsResource)_spriteBatch).Dispose();
	}

	private void SortAndApplyIndexData(BatchDrawGroup batchDrawGroup)
	{
		Array.Sort(batchDrawGroup.BatchDraws, 0, batchDrawGroup.BatchDrawCount, TextureComparer.Instance);
		int num = 0;
		for (int i = 0; i < batchDrawGroup.BatchDrawCount; i++)
		{
			BatchDrawInfo batchDrawInfo = batchDrawGroup.BatchDraws[i];
			for (int j = 0; j < batchDrawInfo.Count; j++)
			{
				int num2 = batchDrawInfo.Index * 4 + j * 4;
				_sortedIndexData[num] = (short)num2;
				_sortedIndexData[num + 1] = (short)(num2 + 1);
				_sortedIndexData[num + 2] = (short)(num2 + 2);
				_sortedIndexData[num + 3] = (short)(num2 + 3);
				_sortedIndexData[num + 4] = (short)(num2 + 2);
				_sortedIndexData[num + 5] = (short)(num2 + 1);
				num += 6;
			}
		}
		((IndexBuffer)_sortedIndexBuffer).SetData<short>(_sortedIndexData, 0, num);
	}

	private void SortedDrawBatch()
	{
		if (_lastBatchDrawGroupIndex == 0 && _batchDrawGroups[_lastBatchDrawGroupIndex].SpriteCount == 0)
		{
			return;
		}
		FlushRemainingBatch();
		VertexBuffer vertexBuffer = ((VertexBufferBinding)(ref _graphicsDevice.GetVertexBuffers()[0])).VertexBuffer;
		_graphicsDevice.Indices = (IndexBuffer)(object)_sortedIndexBuffer;
		for (int i = 0; i <= _lastBatchDrawGroupIndex; i++)
		{
			BatchDrawGroup batchDrawGroup = _batchDrawGroups[i];
			int vertexCount = batchDrawGroup.VertexCount;
			vertexBuffer.SetData<VertexPositionColorTexture>(_batchDrawGroups[i].VertexArray, 0, vertexCount, (SetDataOptions)0);
			SortAndApplyIndexData(batchDrawGroup);
			int num = 0;
			for (int j = 0; j < batchDrawGroup.BatchDrawCount; j++)
			{
				BatchDrawInfo batchDrawInfo = batchDrawGroup.BatchDraws[j];
				_graphicsDevice.Textures[0] = (Texture)(object)batchDrawInfo.Texture;
				int num2 = batchDrawInfo.Count;
				for (; j + 1 < batchDrawGroup.BatchDrawCount && batchDrawInfo.Texture == batchDrawGroup.BatchDraws[j + 1].Texture; j++)
				{
					num2 += batchDrawGroup.BatchDraws[j + 1].Count;
				}
				_graphicsDevice.DrawIndexedPrimitives((PrimitiveType)0, 0, 0, num2 * 4, num, num2 * 2);
				num += num2 * 6;
			}
			batchDrawGroup.Reset();
		}
		_currentBatchDrawInfo = BatchDrawInfo.Empty;
		_lastBatchDrawGroupIndex = 0;
	}

	private void DrawBatch()
	{
		if (_lastBatchDrawGroupIndex == 0 && _batchDrawGroups[_lastBatchDrawGroupIndex].SpriteCount == 0)
		{
			return;
		}
		FlushRemainingBatch();
		VertexBuffer vertexBuffer = ((VertexBufferBinding)(ref _graphicsDevice.GetVertexBuffers()[0])).VertexBuffer;
		for (int i = 0; i <= _lastBatchDrawGroupIndex; i++)
		{
			BatchDrawGroup batchDrawGroup = _batchDrawGroups[i];
			int vertexCount = batchDrawGroup.VertexCount;
			vertexBuffer.SetData<VertexPositionColorTexture>(_batchDrawGroups[i].VertexArray, 0, vertexCount, (SetDataOptions)1);
			for (int j = 0; j < batchDrawGroup.BatchDrawCount; j++)
			{
				BatchDrawInfo batchDrawInfo = batchDrawGroup.BatchDraws[j];
				_graphicsDevice.Textures[0] = (Texture)(object)batchDrawInfo.Texture;
				_graphicsDevice.DrawIndexedPrimitives((PrimitiveType)0, 0, 0, batchDrawInfo.Count * 4, batchDrawInfo.Index * 6, batchDrawInfo.Count * 2);
			}
			batchDrawGroup.Reset();
		}
		_currentBatchDrawInfo = BatchDrawInfo.Empty;
		_lastBatchDrawGroupIndex = 0;
	}

	private void QueueSprite(Vector4 destinationRect, Vector2 origin, VertexColors colors, Vector4 sourceRectangle, Vector2 texCoordTL, Vector2 texCoordBR, Texture2D texture, float depth, float rotation)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01de: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0283: Unknown result type (might be due to invalid IL or missing references)
		//IL_0288: Unknown result type (might be due to invalid IL or missing references)
		//IL_029f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0348: Unknown result type (might be due to invalid IL or missing references)
		//IL_034d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0364: Unknown result type (might be due to invalid IL or missing references)
		//IL_0382: Unknown result type (might be due to invalid IL or missing references)
		UpdateCurrentBatchDraw(texture);
		float num = origin.X / sourceRectangle.Z;
		float num6 = origin.Y / sourceRectangle.W;
		float x = destinationRect.X;
		float y = destinationRect.Y;
		float z = destinationRect.Z;
		float w = destinationRect.W;
		float num2 = num * z;
		float num3 = num6 * w;
		BatchDrawGroup batchDrawGroup = _batchDrawGroups[_lastBatchDrawGroupIndex];
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
		int vertexCount = batchDrawGroup.VertexCount;
		batchDrawGroup.VertexArray[vertexCount].Position.X = x + num2 * num4 - num3 * num5;
		batchDrawGroup.VertexArray[vertexCount].Position.Y = y + num2 * num5 + num3 * num4;
		batchDrawGroup.VertexArray[vertexCount].Position.Z = depth;
		batchDrawGroup.VertexArray[vertexCount].Color = colors.TopLeftColor;
		batchDrawGroup.VertexArray[vertexCount].TextureCoordinate.X = texCoordTL.X;
		batchDrawGroup.VertexArray[vertexCount].TextureCoordinate.Y = texCoordTL.Y;
		vertexCount++;
		batchDrawGroup.VertexArray[vertexCount].Position.X = x + (num2 + z) * num4 - num3 * num5;
		batchDrawGroup.VertexArray[vertexCount].Position.Y = y + (num2 + z) * num5 + num3 * num4;
		batchDrawGroup.VertexArray[vertexCount].Position.Z = depth;
		batchDrawGroup.VertexArray[vertexCount].Color = colors.TopRightColor;
		batchDrawGroup.VertexArray[vertexCount].TextureCoordinate.X = texCoordBR.X;
		batchDrawGroup.VertexArray[vertexCount].TextureCoordinate.Y = texCoordTL.Y;
		vertexCount++;
		batchDrawGroup.VertexArray[vertexCount].Position.X = x + num2 * num4 - (num3 + w) * num5;
		batchDrawGroup.VertexArray[vertexCount].Position.Y = y + num2 * num5 + (num3 + w) * num4;
		batchDrawGroup.VertexArray[vertexCount].Position.Z = depth;
		batchDrawGroup.VertexArray[vertexCount].Color = colors.BottomLeftColor;
		batchDrawGroup.VertexArray[vertexCount].TextureCoordinate.X = texCoordTL.X;
		batchDrawGroup.VertexArray[vertexCount].TextureCoordinate.Y = texCoordBR.Y;
		vertexCount++;
		batchDrawGroup.VertexArray[vertexCount].Position.X = x + (num2 + z) * num4 - (num3 + w) * num5;
		batchDrawGroup.VertexArray[vertexCount].Position.Y = y + (num2 + z) * num5 + (num3 + w) * num4;
		batchDrawGroup.VertexArray[vertexCount].Position.Z = depth;
		batchDrawGroup.VertexArray[vertexCount].Color = colors.BottomRightColor;
		batchDrawGroup.VertexArray[vertexCount].TextureCoordinate.X = texCoordBR.X;
		batchDrawGroup.VertexArray[vertexCount].TextureCoordinate.Y = texCoordBR.Y;
		batchDrawGroup.SpriteCount++;
	}

	private void UpdateCurrentBatchDraw(Texture2D texture)
	{
		BatchDrawGroup batchDrawGroup = _batchDrawGroups[_lastBatchDrawGroupIndex];
		if (batchDrawGroup.SpriteCount >= 2048)
		{
			_currentBatchDrawInfo.Count = 2048 - _currentBatchDrawInfo.Index;
			_batchDrawGroups[_lastBatchDrawGroupIndex].AddBatch(_currentBatchDrawInfo);
			_currentBatchDrawInfo = new BatchDrawInfo(texture);
			_lastBatchDrawGroupIndex++;
			if (_lastBatchDrawGroupIndex >= _batchDrawGroups.Count)
			{
				_batchDrawGroups.Add(new BatchDrawGroup());
			}
		}
		else if (texture != _currentBatchDrawInfo.Texture)
		{
			if (batchDrawGroup.SpriteCount != 0 || _lastBatchDrawGroupIndex != 0)
			{
				_currentBatchDrawInfo.Count = batchDrawGroup.SpriteCount - _currentBatchDrawInfo.Index;
				batchDrawGroup.AddBatch(_currentBatchDrawInfo);
			}
			_currentBatchDrawInfo = new BatchDrawInfo(texture, batchDrawGroup.SpriteCount, 0);
		}
	}

	private void FlushRemainingBatch()
	{
		BatchDrawGroup batchDrawGroup = _batchDrawGroups[_lastBatchDrawGroupIndex];
		if (_currentBatchDrawInfo.Index != batchDrawGroup.SpriteCount)
		{
			_currentBatchDrawInfo.Count = batchDrawGroup.SpriteCount - _currentBatchDrawInfo.Index;
			batchDrawGroup.AddBatch(_currentBatchDrawInfo);
		}
	}
}
