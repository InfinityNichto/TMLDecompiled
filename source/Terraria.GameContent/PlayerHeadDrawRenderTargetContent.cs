using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;

namespace Terraria.GameContent;

public class PlayerHeadDrawRenderTargetContent : AnOutlinedDrawRenderTargetContent
{
	private Player _player;

	private readonly List<DrawData> _drawData = new List<DrawData>();

	private readonly List<int> _dust = new List<int>();

	private readonly List<int> _gore = new List<int>();

	public void UsePlayer(Player player)
	{
		_player = player;
	}

	internal override void DrawTheContent(SpriteBatch spriteBatch)
	{
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		if (_player != null && !_player.ShouldNotDraw)
		{
			Main.PlayerRenderer.DrawPlayerHead(Main.Camera, _player, new Vector2((float)width * 0.5f, (float)height * 0.5f));
		}
	}
}
