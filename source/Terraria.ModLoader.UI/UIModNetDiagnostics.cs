using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using ReLogic.Graphics;
using Terraria.GameContent;
using Terraria.ModLoader.Default;
using Terraria.UI;

namespace Terraria.ModLoader.UI;

public class UIModNetDiagnostics : INetDiagnosticsUI
{
	private struct CounterForMessage
	{
		public int TimesReceived;

		public int TimesSent;

		public int BytesReceived;

		public int BytesSent;

		public void Reset()
		{
			TimesReceived = 0;
			TimesSent = 0;
			BytesReceived = 0;
			BytesSent = 0;
		}

		public void CountReadMessage(int messageLength)
		{
			TimesReceived++;
			BytesReceived += messageLength;
		}

		public void CountSentMessage(int messageLength)
		{
			TimesSent++;
			BytesSent += messageLength;
		}
	}

	private const float TextScale = 0.7f;

	private const string Suffix = ": ";

	private const string ModString = "Mod";

	private const string RxTxString = "Received(#, Bytes)       Sent(#, Bytes)";

	private Asset<DynamicSpriteFont> fontAsset;

	private readonly Mod[] Mods;

	private CounterForMessage[] CounterByModNetId;

	private int HighestFoundSentBytes = 1;

	private int HighestFoundReadBytes = 1;

	private float FirstColumnWidth;

	private Asset<DynamicSpriteFont> FontAsset => fontAsset ?? (fontAsset = FontAssets.MouseText);

	public UIModNetDiagnostics(IEnumerable<Mod> mods)
	{
		Mods = mods.Where((Mod mod) => mod != ModContent.GetInstance<ModLoaderMod>()).ToArray();
		Reset();
	}

	public void Reset()
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		CounterByModNetId = new CounterForMessage[Mods.Length];
		DynamicSpriteFont font = FontAsset.Value;
		FirstColumnWidth = font.MeasureString("Mod").X;
		for (int i = 0; i < Mods.Length; i++)
		{
			float length = font.MeasureString(Mods[i].Name).X;
			if (FirstColumnWidth < length)
			{
				FirstColumnWidth = length;
			}
		}
		FirstColumnWidth += font.MeasureString(": ").X + 2f;
		FirstColumnWidth *= 0.7f;
	}

	public void CountReadMessage(int messageId, int messageLength)
	{
		int index = Array.FindIndex(Mods, (Mod mod) => mod.netID == messageId);
		if (index > -1)
		{
			CounterByModNetId[index].CountReadMessage(messageLength);
		}
	}

	public void CountSentMessage(int messageId, int messageLength)
	{
		int index = Array.FindIndex(Mods, (Mod mod) => mod.netID == messageId);
		if (index > -1)
		{
			CounterByModNetId[index].CountSentMessage(messageLength);
		}
	}

	public void Draw(SpriteBatch spriteBatch)
	{
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		int count = CounterByModNetId.Length;
		int numCols = (count - 1) / 50;
		int x = 190;
		int xBuf = x + 10;
		int y = 110;
		int yBuf = y + 10;
		int width = 232;
		width += (int)(FirstColumnWidth + fontAsset.Value.MeasureString(HighestFoundSentBytes.ToString()).X * 0.7f);
		int widthBuf = width + 10;
		int lineHeight = 13;
		Vector2 modPos = default(Vector2);
		for (int i = 0; i <= numCols; i++)
		{
			int lineCountInCol = ((i == numCols) ? (1 + (count - 1) % 50) : 50);
			int heightBuf = lineHeight * (lineCountInCol + 2) + 10;
			Utils.DrawInvBG(spriteBatch, x + widthBuf * i, y, width, heightBuf);
			((Vector2)(ref modPos))._002Ector((float)(xBuf + widthBuf * i), (float)yBuf);
			Vector2 headerPos = modPos + new Vector2(FirstColumnWidth, 0f);
			DrawText(spriteBatch, "Received(#, Bytes)       Sent(#, Bytes)", headerPos, Color.White);
			DrawText(spriteBatch, "Mod", modPos, Color.White);
		}
		Vector2 position = default(Vector2);
		for (int j = 0; j < count; j++)
		{
			int colNum = j / 50;
			int lineNum = j - colNum * 50;
			position.X = xBuf + colNum * widthBuf;
			position.Y = yBuf + lineHeight + lineNum * lineHeight;
			DrawCounter(spriteBatch, CounterByModNetId[j], Mods[j].Name, position);
		}
	}

	private void DrawCounter(SpriteBatch spriteBatch, CounterForMessage counter, string title, Vector2 position)
	{
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		if (HighestFoundSentBytes < counter.BytesSent)
		{
			HighestFoundSentBytes = counter.BytesSent;
		}
		if (HighestFoundReadBytes < counter.BytesReceived)
		{
			HighestFoundReadBytes = counter.BytesReceived;
		}
		Vector2 pos = position;
		string text = title + ": ";
		float num = Utils.Remap(counter.BytesReceived, 0f, HighestFoundReadBytes, 0f, 1f);
		Color color = Main.hslToRgb(0.3f * (1f - num), 1f, 0.5f);
		string drawText = text;
		DrawText(spriteBatch, drawText, pos, color);
		pos.X += FirstColumnWidth;
		drawText = "rx:" + $"{counter.TimesReceived}";
		DrawText(spriteBatch, drawText, pos, color);
		pos.X += 70f;
		drawText = $"{counter.BytesReceived}";
		DrawText(spriteBatch, drawText, pos, color);
		pos.X += 70f;
		drawText = "tx:" + $"{counter.TimesSent}";
		DrawText(spriteBatch, drawText, pos, color);
		pos.X += 70f;
		drawText = $"{counter.BytesSent}";
		DrawText(spriteBatch, drawText, pos, color);
	}

	private void DrawText(SpriteBatch spriteBatch, string text, Vector2 pos, Color color)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		spriteBatch.DrawString(FontAsset.Value, text, pos, color, 0f, Vector2.Zero, 0.7f, (SpriteEffects)0, 0f);
	}

	public void CountReadModuleMessage(int moduleMessageId, int messageLength)
	{
		throw new NotImplementedException();
	}

	public void CountSentModuleMessage(int moduleMessageId, int messageLength)
	{
		throw new NotImplementedException();
	}
}
