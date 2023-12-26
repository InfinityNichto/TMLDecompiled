using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria.Graphics;

public class VertexStrip
{
	public delegate Color StripColorFunction(float progressOnStrip);

	public delegate float StripHalfWidthFunction(float progressOnStrip);

	private struct CustomVertexInfo : IVertexType
	{
		public Vector2 Position;

		public Color Color;

		public Vector2 TexCoord;

		private static VertexDeclaration _vertexDeclaration = new VertexDeclaration((VertexElement[])(object)new VertexElement[3]
		{
			new VertexElement(0, (VertexElementFormat)1, (VertexElementUsage)0, 0),
			new VertexElement(8, (VertexElementFormat)4, (VertexElementUsage)1, 0),
			new VertexElement(12, (VertexElementFormat)1, (VertexElementUsage)2, 0)
		});

		public VertexDeclaration VertexDeclaration => _vertexDeclaration;

		public CustomVertexInfo(Vector2 position, Color color, Vector2 texCoord)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			Position = position;
			Color = color;
			TexCoord = texCoord;
		}
	}

	private CustomVertexInfo[] _vertices = new CustomVertexInfo[1];

	private int _vertexAmountCurrentlyMaintained;

	private short[] _indices = new short[1];

	private int _indicesAmountCurrentlyMaintained;

	private List<Vector2> _temporaryPositionsCache = new List<Vector2>();

	private List<float> _temporaryRotationsCache = new List<float>();

	public void PrepareStrip(Vector2[] positions, float[] rotations, StripColorFunction colorFunction, StripHalfWidthFunction widthFunction, Vector2 offsetForAllPositions = default(Vector2), int? expectedVertexPairsAmount = null, bool includeBacksides = false)
	{
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		int num = positions.Length;
		int num2 = (_vertexAmountCurrentlyMaintained = num * 2);
		if (_vertices.Length < num2)
		{
			Array.Resize(ref _vertices, num2);
		}
		int num3 = num;
		if (expectedVertexPairsAmount.HasValue)
		{
			num3 = expectedVertexPairsAmount.Value;
		}
		for (int i = 0; i < num; i++)
		{
			if (positions[i] == Vector2.Zero)
			{
				num = i - 1;
				_vertexAmountCurrentlyMaintained = num * 2;
				break;
			}
			Vector2 pos = positions[i] + offsetForAllPositions;
			float rot = MathHelper.WrapAngle(rotations[i]);
			int indexOnVertexArray = i * 2;
			float progressOnStrip = (float)i / (float)(num3 - 1);
			AddVertex(colorFunction, widthFunction, pos, rot, indexOnVertexArray, progressOnStrip);
		}
		PrepareIndices(num, includeBacksides);
	}

	public void PrepareStripWithProceduralPadding(Vector2[] positions, float[] rotations, StripColorFunction colorFunction, StripHalfWidthFunction widthFunction, Vector2 offsetForAllPositions = default(Vector2), bool includeBacksides = false, bool tryStoppingOddBug = true)
	{
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0182: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0190: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_0197: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		int num = positions.Length;
		_temporaryPositionsCache.Clear();
		_temporaryRotationsCache.Clear();
		for (int i = 0; i < num && !(positions[i] == Vector2.Zero); i++)
		{
			Vector2 vector = positions[i];
			float num2 = MathHelper.WrapAngle(rotations[i]);
			_temporaryPositionsCache.Add(vector);
			_temporaryRotationsCache.Add(num2);
			if (i + 1 >= num || !(positions[i + 1] != Vector2.Zero))
			{
				continue;
			}
			Vector2 vector2 = positions[i + 1];
			float num3 = MathHelper.WrapAngle(rotations[i + 1]);
			int num4 = (int)(Math.Abs(MathHelper.WrapAngle(num3 - num2)) / ((float)Math.PI / 12f));
			if (num4 != 0)
			{
				float num5 = vector.Distance(vector2);
				Vector2 value = vector + num2.ToRotationVector2() * num5;
				Vector2 value2 = vector2 + num3.ToRotationVector2() * (0f - num5);
				int num6 = num4 + 2;
				float num7 = 1f / (float)num6;
				Vector2 target = vector;
				for (float num8 = num7; num8 < 1f; num8 += num7)
				{
					Vector2 vector3 = Vector2.CatmullRom(value, vector, vector2, value2, num8);
					float item = MathHelper.WrapAngle(vector3.DirectionTo(target).ToRotation());
					_temporaryPositionsCache.Add(vector3);
					_temporaryRotationsCache.Add(item);
					target = vector3;
				}
			}
		}
		int count = _temporaryPositionsCache.Count;
		Vector2 zero = Vector2.Zero;
		for (int j = 0; j < count && (!tryStoppingOddBug || !(_temporaryPositionsCache[j] == zero)); j++)
		{
			Vector2 pos = _temporaryPositionsCache[j] + offsetForAllPositions;
			float rot = _temporaryRotationsCache[j];
			int indexOnVertexArray = j * 2;
			float progressOnStrip = (float)j / (float)(count - 1);
			AddVertex(colorFunction, widthFunction, pos, rot, indexOnVertexArray, progressOnStrip);
		}
		_vertexAmountCurrentlyMaintained = count * 2;
		PrepareIndices(count, includeBacksides);
	}

	private void PrepareIndices(int vertexPaidsAdded, bool includeBacksides)
	{
		int num = vertexPaidsAdded - 1;
		int num2 = 6 + includeBacksides.ToInt() * 6;
		int num3 = (_indicesAmountCurrentlyMaintained = num * num2);
		if (_indices.Length < num3)
		{
			Array.Resize(ref _indices, num3);
		}
		for (short num4 = 0; num4 < num; num4++)
		{
			short num5 = (short)(num4 * num2);
			int num6 = num4 * 2;
			_indices[num5] = (short)num6;
			_indices[num5 + 1] = (short)(num6 + 1);
			_indices[num5 + 2] = (short)(num6 + 2);
			_indices[num5 + 3] = (short)(num6 + 2);
			_indices[num5 + 4] = (short)(num6 + 1);
			_indices[num5 + 5] = (short)(num6 + 3);
			if (includeBacksides)
			{
				_indices[num5 + 6] = (short)(num6 + 2);
				_indices[num5 + 7] = (short)(num6 + 1);
				_indices[num5 + 8] = (short)num6;
				_indices[num5 + 9] = (short)(num6 + 2);
				_indices[num5 + 10] = (short)(num6 + 3);
				_indices[num5 + 11] = (short)(num6 + 1);
			}
		}
	}

	private void AddVertex(StripColorFunction colorFunction, StripHalfWidthFunction widthFunction, Vector2 pos, float rot, int indexOnVertexArray, float progressOnStrip)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		while (indexOnVertexArray + 1 >= _vertices.Length)
		{
			Array.Resize(ref _vertices, _vertices.Length * 2);
		}
		Color color = colorFunction(progressOnStrip);
		float num = widthFunction(progressOnStrip);
		Vector2 vector = MathHelper.WrapAngle(rot - (float)Math.PI / 2f).ToRotationVector2() * num;
		_vertices[indexOnVertexArray].Position = pos + vector;
		_vertices[indexOnVertexArray + 1].Position = pos - vector;
		_vertices[indexOnVertexArray].TexCoord = new Vector2(progressOnStrip, 1f);
		_vertices[indexOnVertexArray + 1].TexCoord = new Vector2(progressOnStrip, 0f);
		_vertices[indexOnVertexArray].Color = color;
		_vertices[indexOnVertexArray + 1].Color = color;
	}

	public void DrawTrail()
	{
		if (_vertexAmountCurrentlyMaintained >= 3)
		{
			((Game)Main.instance).GraphicsDevice.DrawUserIndexedPrimitives<CustomVertexInfo>((PrimitiveType)0, _vertices, 0, _vertexAmountCurrentlyMaintained, _indices, 0, _indicesAmountCurrentlyMaintained / 3);
		}
	}
}
