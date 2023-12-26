using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria.GameContent;
using Terraria.ID;

namespace Terraria.UI;

public class LegacyNetDiagnosticsUI : INetDiagnosticsUI
{
	public static bool netDiag;

	public static int txData = 0;

	public static int rxData = 0;

	public static int txMsg = 0;

	public static int rxMsg = 0;

	private static readonly int maxMsg = MessageID.Count + 1;

	public static int[] rxMsgType = new int[maxMsg];

	public static int[] rxDataType = new int[maxMsg];

	public static int[] txMsgType = new int[maxMsg];

	public static int[] txDataType = new int[maxMsg];

	public void Reset()
	{
		rxMsg = 0;
		rxData = 0;
		txMsg = 0;
		txData = 0;
		for (int i = 0; i < maxMsg; i++)
		{
			rxMsgType[i] = 0;
			rxDataType[i] = 0;
			txMsgType[i] = 0;
			txDataType[i] = 0;
		}
	}

	public void CountReadMessage(int messageId, int messageLength)
	{
		rxMsg++;
		rxData += messageLength;
		rxMsgType[messageId]++;
		rxDataType[messageId] += messageLength;
	}

	public void CountSentMessage(int messageId, int messageLength)
	{
		txMsg++;
		txData += messageLength;
		txMsgType[messageId]++;
		txDataType[messageId] += messageLength;
	}

	public void Draw(SpriteBatch spriteBatch)
	{
		DrawTitles(spriteBatch);
		DrawMesageLines(spriteBatch);
	}

	private static void DrawMesageLines(SpriteBatch spriteBatch)
	{
		for (int i = 0; i < maxMsg; i++)
		{
			int num = 200;
			int num2 = 120;
			int num3 = i / 50;
			num += num3 * 400;
			num2 += (i - num3 * 50) * 13;
			PrintNetDiagnosticsLineForMessage(spriteBatch, i, num, num2);
		}
	}

	private static void DrawTitles(SpriteBatch spriteBatch)
	{
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < 4; i++)
		{
			string text = "";
			int num = 20;
			int num2 = 220;
			switch (i)
			{
			case 0:
				text = "RX Msgs: " + $"{rxMsg:0,0}";
				num2 += i * 20;
				break;
			case 1:
				text = "RX Bytes: " + $"{rxData:0,0}";
				num2 += i * 20;
				break;
			case 2:
				text = "TX Msgs: " + $"{txMsg:0,0}";
				num2 += i * 20;
				break;
			case 3:
				text = "TX Bytes: " + $"{txData:0,0}";
				num2 += i * 20;
				break;
			}
			spriteBatch.DrawString(FontAssets.MouseText.Value, text, new Vector2((float)num, (float)num2), Color.White, 0f, default(Vector2), 1f, (SpriteEffects)0, 0f);
		}
	}

	private static void PrintNetDiagnosticsLineForMessage(SpriteBatch spriteBatch, int msgId, int x, int y)
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0206: Unknown result type (might be due to invalid IL or missing references)
		//IL_020b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0217: Unknown result type (might be due to invalid IL or missing references)
		//IL_021d: Unknown result type (might be due to invalid IL or missing references)
		float scale = 0.7f;
		string text = "";
		text = msgId + ": ";
		spriteBatch.DrawString(FontAssets.MouseText.Value, text, new Vector2((float)x, (float)y), Color.White, 0f, default(Vector2), scale, (SpriteEffects)0, 0f);
		x += 30;
		text = "rx:" + $"{rxMsgType[msgId]:0,0}";
		spriteBatch.DrawString(FontAssets.MouseText.Value, text, new Vector2((float)x, (float)y), Color.White, 0f, default(Vector2), scale, (SpriteEffects)0, 0f);
		x += 70;
		text = $"{rxDataType[msgId]:0,0}";
		spriteBatch.DrawString(FontAssets.MouseText.Value, text, new Vector2((float)x, (float)y), Color.White, 0f, default(Vector2), scale, (SpriteEffects)0, 0f);
		x += 70;
		text = msgId + ": ";
		spriteBatch.DrawString(FontAssets.MouseText.Value, text, new Vector2((float)x, (float)y), Color.White, 0f, default(Vector2), scale, (SpriteEffects)0, 0f);
		x += 30;
		text = "tx:" + $"{txMsgType[msgId]:0,0}";
		spriteBatch.DrawString(FontAssets.MouseText.Value, text, new Vector2((float)x, (float)y), Color.White, 0f, default(Vector2), scale, (SpriteEffects)0, 0f);
		x += 70;
		text = $"{txDataType[msgId]:0,0}";
		spriteBatch.DrawString(FontAssets.MouseText.Value, text, new Vector2((float)x, (float)y), Color.White, 0f, default(Vector2), scale, (SpriteEffects)0, 0f);
	}

	public void CountReadModuleMessage(int moduleMessageId, int messageLength)
	{
	}

	public void CountSentModuleMessage(int moduleMessageId, int messageLength)
	{
	}
}
