using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace Terraria.Graphics.Shaders;

public class HairShaderDataSet
{
	protected internal List<HairShaderData> _shaderData = new List<HairShaderData>();

	protected internal Dictionary<int, int> _shaderLookupDictionary = new Dictionary<int, int>();

	protected internal int _shaderDataCount;

	internal Dictionary<int, int> _reverseShaderLookupDictionary = new Dictionary<int, int>();

	public T BindShader<T>(int itemId, T shaderData) where T : HairShaderData
	{
		_shaderLookupDictionary[itemId] = ++_shaderDataCount;
		_reverseShaderLookupDictionary[_shaderLookupDictionary[itemId]] = itemId;
		_shaderData.Add(shaderData);
		return shaderData;
	}

	public void Apply(int shaderId, Player player, DrawData? drawData = null)
	{
		if (shaderId != 0 && shaderId <= _shaderDataCount)
		{
			_shaderData[shaderId - 1].Apply(player, drawData);
		}
		else
		{
			Main.pixelShader.CurrentTechnique.Passes[0].Apply();
		}
	}

	public Color GetColor(int shaderId, Player player, Color lightColor)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		if (shaderId != 0 && shaderId <= _shaderDataCount)
		{
			return _shaderData[shaderId - 1].GetColor(player, lightColor);
		}
		return new Color(((Color)(ref lightColor)).ToVector4() * ((Color)(ref player.hairColor)).ToVector4());
	}

	public HairShaderData GetShaderFromItemId(int type)
	{
		if (_shaderLookupDictionary.ContainsKey(type))
		{
			return _shaderData[_shaderLookupDictionary[type] - 1];
		}
		return null;
	}

	public int GetShaderIdFromItemId(int type)
	{
		if (_shaderLookupDictionary.ContainsKey(type))
		{
			return _shaderLookupDictionary[type];
		}
		return -1;
	}
}
