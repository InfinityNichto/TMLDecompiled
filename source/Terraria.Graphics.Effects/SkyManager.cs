using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria.Graphics.Effects;

public class SkyManager : EffectManager<CustomSky>
{
	public static SkyManager Instance = new SkyManager();

	private float _lastDepth;

	private LinkedList<CustomSky> _activeSkies = new LinkedList<CustomSky>();

	public void Reset()
	{
		foreach (CustomSky value in _effects.Values)
		{
			value.Reset();
		}
		_activeSkies.Clear();
	}

	public void Update(GameTime gameTime)
	{
		for (int i = 0; i < Main.worldEventUpdates; i++)
		{
			LinkedListNode<CustomSky> linkedListNode = _activeSkies.First;
			while (linkedListNode != null)
			{
				CustomSky value = linkedListNode.Value;
				LinkedListNode<CustomSky> next = linkedListNode.Next;
				value.Update(gameTime);
				if (!value.IsActive())
				{
					_activeSkies.Remove(linkedListNode);
				}
				linkedListNode = next;
			}
		}
	}

	public void Draw(SpriteBatch spriteBatch)
	{
		DrawDepthRange(spriteBatch, float.MinValue, float.MaxValue);
	}

	public void DrawToDepth(SpriteBatch spriteBatch, float minDepth)
	{
		if (!(_lastDepth <= minDepth))
		{
			DrawDepthRange(spriteBatch, minDepth, _lastDepth);
			_lastDepth = minDepth;
		}
	}

	public void DrawDepthRange(SpriteBatch spriteBatch, float minDepth, float maxDepth)
	{
		foreach (CustomSky activeSky in _activeSkies)
		{
			activeSky.Draw(spriteBatch, minDepth, maxDepth);
		}
	}

	public void DrawRemainingDepth(SpriteBatch spriteBatch)
	{
		DrawDepthRange(spriteBatch, float.MinValue, _lastDepth);
		_lastDepth = float.MinValue;
	}

	public void ResetDepthTracker()
	{
		_lastDepth = float.MaxValue;
	}

	public void SetStartingDepth(float depth)
	{
		_lastDepth = depth;
	}

	public override void OnActivate(CustomSky effect, Vector2 position)
	{
		_activeSkies.Remove(effect);
		_activeSkies.AddLast(effect);
	}

	public Color ProcessTileColor(Color color)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		foreach (CustomSky activeSky in _activeSkies)
		{
			color = activeSky.OnTileColor(color);
		}
		return color;
	}

	public float ProcessCloudAlpha()
	{
		float num = 1f;
		foreach (CustomSky activeSky in _activeSkies)
		{
			num *= activeSky.GetCloudAlpha();
		}
		return MathHelper.Clamp(num, 0f, 1f);
	}

	internal void DeactivateAll()
	{
		foreach (string key in _effects.Keys)
		{
			if (base[key].IsActive())
			{
				base[key].Deactivate();
			}
		}
	}
}
