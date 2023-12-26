using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.GameContent;

namespace Terraria.Graphics.Renderers;

public class NPCHeadRenderer : INeedRenderTargetContent
{
	private NPCHeadDrawRenderTargetContent[] _contents;

	private Asset<Texture2D>[] _matchingArray;

	public bool IsReady => false;

	public NPCHeadRenderer(Asset<Texture2D>[] matchingArray)
	{
		_matchingArray = matchingArray;
		Reset();
	}

	public void Reset()
	{
		_contents = new NPCHeadDrawRenderTargetContent[_matchingArray.Length];
	}

	public void DrawWithOutlines(Entity entity, int headId, Vector2 position, Color color, float rotation, float scale, SpriteEffects effects)
	{
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		if (_contents[headId] == null)
		{
			_contents[headId] = new NPCHeadDrawRenderTargetContent();
			_contents[headId].SetTexture(_matchingArray[headId].Value);
		}
		NPCHeadDrawRenderTargetContent nPCHeadDrawRenderTargetContent = _contents[headId];
		if (nPCHeadDrawRenderTargetContent.IsReady)
		{
			RenderTarget2D target = nPCHeadDrawRenderTargetContent.GetTarget();
			Main.spriteBatch.Draw((Texture2D)(object)target, position, (Rectangle?)null, color, rotation, ((Texture2D)(object)target).Size() / 2f, scale, effects, 0f);
		}
		else
		{
			nPCHeadDrawRenderTargetContent.Request();
		}
	}

	public void PrepareRenderTarget(GraphicsDevice device, SpriteBatch spriteBatch)
	{
		for (int i = 0; i < _contents.Length; i++)
		{
			if (_contents[i] != null && !_contents[i].IsReady)
			{
				_contents[i].PrepareRenderTarget(device, spriteBatch);
			}
		}
	}
}
