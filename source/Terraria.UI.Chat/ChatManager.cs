using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria.Chat;
using Terraria.GameContent.UI.Chat;

namespace Terraria.UI.Chat;

public static class ChatManager
{
	public static class Regexes
	{
		public static readonly Regex Format = new Regex("(?<!\\\\)\\[(?<tag>[a-zA-Z]{1,10})(\\/(?<options>[^:]+))?:(?<text>.+?)(?<!\\\\)\\]", RegexOptions.Compiled);
	}

	public static readonly ChatCommandProcessor Commands = new ChatCommandProcessor();

	private static ConcurrentDictionary<string, ITagHandler> _handlers = new ConcurrentDictionary<string, ITagHandler>();

	public static readonly Vector2[] ShadowDirections = (Vector2[])(object)new Vector2[4]
	{
		-Vector2.UnitX,
		Vector2.UnitX,
		-Vector2.UnitY,
		Vector2.UnitY
	};

	public static Color WaveColor(Color color)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		float num = (float)(int)Main.mouseTextColor / 255f;
		color = Color.Lerp(color, Color.Black, 1f - num);
		((Color)(ref color)).A = Main.mouseTextColor;
		return color;
	}

	public static void ConvertNormalSnippets(TextSnippet[] snippets)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < snippets.Length; i++)
		{
			TextSnippet textSnippet = snippets[i];
			if (snippets[i].GetType() == typeof(TextSnippet))
			{
				PlainTagHandler.PlainSnippet plainSnippet = new PlainTagHandler.PlainSnippet(textSnippet.Text, textSnippet.Color, textSnippet.Scale);
				snippets[i] = plainSnippet;
			}
		}
	}

	public static void Register<T>(params string[] names) where T : ITagHandler, new()
	{
		T val = new T();
		for (int i = 0; i < names.Length; i++)
		{
			_handlers[names[i].ToLower()] = val;
		}
	}

	private static ITagHandler GetHandler(string tagName)
	{
		string key = tagName.ToLower();
		if (_handlers.ContainsKey(key))
		{
			return _handlers[key];
		}
		return null;
	}

	public static List<TextSnippet> ParseMessage(string text, Color baseColor)
	{
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		text = text.Replace("\r", "");
		MatchCollection matchCollection = Regexes.Format.Matches(text);
		List<TextSnippet> list = new List<TextSnippet>();
		int num = 0;
		foreach (Match item in matchCollection)
		{
			if (item.Index > num)
			{
				list.Add(new TextSnippet(text.Substring(num, item.Index - num), baseColor));
			}
			num = item.Index + item.Length;
			string value4 = item.Groups["tag"].Value;
			string value2 = item.Groups["text"].Value;
			string value3 = item.Groups["options"].Value;
			ITagHandler handler = GetHandler(value4);
			if (handler != null)
			{
				list.Add(handler.Parse(value2, baseColor, value3));
				list[list.Count - 1].TextOriginal = item.ToString();
			}
			else
			{
				list.Add(new TextSnippet(value2, baseColor));
			}
		}
		if (text.Length > num)
		{
			list.Add(new TextSnippet(text.Substring(num, text.Length - num), baseColor));
		}
		return list;
	}

	public static bool AddChatText(DynamicSpriteFont font, string text, Vector2 baseScale)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		int num = 470;
		num = Main.screenWidth - 330;
		if (GetStringSize(font, Main.chatText + text, baseScale).X > (float)num)
		{
			return false;
		}
		Main.chatText += text;
		return true;
	}

	public static Vector2 GetStringSize(DynamicSpriteFont font, string text, Vector2 baseScale, float maxWidth = -1f)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		TextSnippet[] snippets = ParseMessage(text, Color.White).ToArray();
		return GetStringSize(font, snippets, baseScale, maxWidth);
	}

	public static Vector2 GetStringSize(DynamicSpriteFont font, TextSnippet[] snippets, Vector2 baseScale, float maxWidth = -1f)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_0249: Unknown result type (might be due to invalid IL or missing references)
		//IL_0267: Unknown result type (might be due to invalid IL or missing references)
		//IL_0272: Unknown result type (might be due to invalid IL or missing references)
		//IL_0278: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01da: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0207: Unknown result type (might be due to invalid IL or missing references)
		//IL_020d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0213: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_0182: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0193: Unknown result type (might be due to invalid IL or missing references)
		Vector2 vec = default(Vector2);
		((Vector2)(ref vec))._002Ector((float)Main.mouseX, (float)Main.mouseY);
		Vector2 zero = Vector2.Zero;
		Vector2 vector = zero;
		Vector2 result = vector;
		float x = font.MeasureString(" ").X;
		float num = 1f;
		float num2 = 0f;
		foreach (TextSnippet textSnippet in snippets)
		{
			textSnippet.Update();
			num = textSnippet.Scale;
			float scale = baseScale.X * num;
			if (textSnippet.UniqueDraw(justCheckingString: true, out var size, null, default(Vector2), default(Color), scale))
			{
				vector.X += size.X;
				result.X = Math.Max(result.X, vector.X);
				result.Y = Math.Max(result.Y, vector.Y + size.Y);
				continue;
			}
			string[] array = textSnippet.Text.Split('\n');
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				string[] array3 = array2[i].Split(' ');
				for (int j = 0; j < array3.Length; j++)
				{
					if (j != 0)
					{
						vector.X += x * baseScale.X * num;
					}
					if (maxWidth > 0f)
					{
						float num3 = font.MeasureString(array3[j]).X * baseScale.X * num;
						if (vector.X - zero.X + num3 > maxWidth)
						{
							vector.X = zero.X;
							vector.Y += (float)font.LineSpacing * num2 * baseScale.Y;
							result.Y = Math.Max(result.Y, vector.Y);
							num2 = 0f;
						}
					}
					if (num2 < num)
					{
						num2 = num;
					}
					Vector2 vector2 = font.MeasureString(array3[j]);
					vec.Between(vector, vector + vector2);
					vector.X += vector2.X * baseScale.X * num;
					result.X = Math.Max(result.X, vector.X);
					result.Y = Math.Max(result.Y, vector.Y + vector2.Y);
				}
				if (array.Length > 1 && i < array2.Length - 1)
				{
					vector.X = zero.X;
					vector.Y += (float)font.LineSpacing * num2 * baseScale.Y;
					result.Y = Math.Max(result.Y, vector.Y);
					num2 = 0f;
				}
			}
		}
		return result;
	}

	public static void DrawColorCodedStringShadow(SpriteBatch spriteBatch, DynamicSpriteFont font, TextSnippet[] snippets, Vector2 position, Color baseColor, float rotation, Vector2 origin, Vector2 baseScale, float maxWidth = -1f, float spread = 2f)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < ShadowDirections.Length; i++)
		{
			DrawColorCodedString(spriteBatch, font, snippets, position + ShadowDirections[i] * spread, baseColor, rotation, origin, baseScale, out var _, maxWidth, ignoreColors: true);
		}
	}

	public static Vector2 DrawColorCodedString(SpriteBatch spriteBatch, DynamicSpriteFont font, TextSnippet[] snippets, Vector2 position, Color baseColor, float rotation, Vector2 origin, Vector2 baseScale, out int hoveredSnippet, float maxWidth, bool ignoreColors = false)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0335: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_0189: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01af: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_022a: Unknown result type (might be due to invalid IL or missing references)
		//IL_022b: Unknown result type (might be due to invalid IL or missing references)
		//IL_022f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0231: Unknown result type (might be due to invalid IL or missing references)
		//IL_023a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0241: Unknown result type (might be due to invalid IL or missing references)
		//IL_0257: Unknown result type (might be due to invalid IL or missing references)
		//IL_025c: Unknown result type (might be due to invalid IL or missing references)
		//IL_025e: Unknown result type (might be due to invalid IL or missing references)
		//IL_025f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0260: Unknown result type (might be due to invalid IL or missing references)
		//IL_0261: Unknown result type (might be due to invalid IL or missing references)
		//IL_0263: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0202: Unknown result type (might be due to invalid IL or missing references)
		//IL_027b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0282: Unknown result type (might be due to invalid IL or missing references)
		//IL_0291: Unknown result type (might be due to invalid IL or missing references)
		//IL_0297: Unknown result type (might be due to invalid IL or missing references)
		int num = -1;
		Vector2 vec = default(Vector2);
		((Vector2)(ref vec))._002Ector((float)Main.mouseX, (float)Main.mouseY);
		Vector2 vector = position;
		Vector2 result = vector;
		float x = font.MeasureString(" ").X;
		Color color = baseColor;
		float num2 = 1f;
		float num3 = 0f;
		for (int i = 0; i < snippets.Length; i++)
		{
			TextSnippet textSnippet = snippets[i];
			textSnippet.Update();
			if (!ignoreColors)
			{
				color = textSnippet.GetVisibleColor();
			}
			num2 = textSnippet.Scale;
			if (textSnippet.UniqueDraw(justCheckingString: false, out var size, spriteBatch, vector, color, baseScale.X * num2))
			{
				if (vec.Between(vector, vector + size))
				{
					num = i;
				}
				vector.X += size.X;
				result.X = Math.Max(result.X, vector.X);
				continue;
			}
			string[] array = textSnippet.Text.Split('\n');
			array = Regex.Split(textSnippet.Text, "(\n)");
			bool flag = true;
			string[] array3 = array;
			foreach (string obj in array3)
			{
				string[] array2 = Regex.Split(obj, "( )");
				array2 = obj.Split(' ');
				if (obj == "\n")
				{
					vector.Y += (float)font.LineSpacing * num3 * baseScale.Y;
					vector.X = position.X;
					result.Y = Math.Max(result.Y, vector.Y);
					num3 = 0f;
					flag = false;
					continue;
				}
				for (int j = 0; j < array2.Length; j++)
				{
					if (j != 0)
					{
						vector.X += x * baseScale.X * num2;
					}
					if (maxWidth > 0f)
					{
						float num4 = font.MeasureString(array2[j]).X * baseScale.X * num2;
						if (vector.X - position.X + num4 > maxWidth)
						{
							vector.X = position.X;
							vector.Y += (float)font.LineSpacing * num3 * baseScale.Y;
							result.Y = Math.Max(result.Y, vector.Y);
							num3 = 0f;
						}
					}
					if (num3 < num2)
					{
						num3 = num2;
					}
					spriteBatch.DrawString(font, array2[j], vector, color, rotation, origin, baseScale * textSnippet.Scale * num2, (SpriteEffects)0, 0f);
					Vector2 vector2 = font.MeasureString(array2[j]);
					if (vec.Between(vector, vector + vector2))
					{
						num = i;
					}
					vector.X += vector2.X * baseScale.X * num2;
					result.X = Math.Max(result.X, vector.X);
				}
				if (array.Length > 1 && flag)
				{
					vector.Y += (float)font.LineSpacing * num3 * baseScale.Y;
					vector.X = position.X;
					result.Y = Math.Max(result.Y, vector.Y);
					num3 = 0f;
				}
				flag = true;
			}
		}
		hoveredSnippet = num;
		return result;
	}

	public static Vector2 DrawColorCodedStringWithShadow(SpriteBatch spriteBatch, DynamicSpriteFont font, TextSnippet[] snippets, Vector2 position, float rotation, Vector2 origin, Vector2 baseScale, out int hoveredSnippet, float maxWidth = -1f, float spread = 2f)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		DrawColorCodedStringShadow(spriteBatch, font, snippets, position, Color.Black, rotation, origin, baseScale, maxWidth, spread);
		return DrawColorCodedString(spriteBatch, font, snippets, position, Color.White, rotation, origin, baseScale, out hoveredSnippet, maxWidth);
	}

	public static Vector2 DrawColorCodedStringWithShadow(SpriteBatch spriteBatch, DynamicSpriteFont font, TextSnippet[] snippets, Vector2 position, float rotation, Color color, Vector2 origin, Vector2 baseScale, out int hoveredSnippet, float maxWidth = -1f, float spread = 2f)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		DrawColorCodedStringShadow(spriteBatch, font, snippets, position, Color.Black, rotation, origin, baseScale, maxWidth, spread);
		return DrawColorCodedString(spriteBatch, font, snippets, position, color, rotation, origin, baseScale, out hoveredSnippet, maxWidth);
	}

	public static Vector2 DrawColorCodedStringWithShadow(SpriteBatch spriteBatch, DynamicSpriteFont font, TextSnippet[] snippets, Vector2 position, float rotation, Color color, Color shadowColor, Vector2 origin, Vector2 baseScale, out int hoveredSnippet, float maxWidth = -1f, float spread = 2f)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		DrawColorCodedStringShadow(spriteBatch, font, snippets, position, shadowColor, rotation, origin, baseScale, maxWidth, spread);
		return DrawColorCodedString(spriteBatch, font, snippets, position, color, rotation, origin, baseScale, out hoveredSnippet, maxWidth);
	}

	public static void DrawColorCodedStringShadow(SpriteBatch spriteBatch, DynamicSpriteFont font, string text, Vector2 position, Color baseColor, float rotation, Vector2 origin, Vector2 baseScale, float maxWidth = -1f, float spread = 2f)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < ShadowDirections.Length; i++)
		{
			DrawColorCodedString(spriteBatch, font, text, position + ShadowDirections[i] * spread, baseColor, rotation, origin, baseScale, maxWidth, ignoreColors: true);
		}
	}

	public static Vector2 DrawColorCodedString(SpriteBatch spriteBatch, DynamicSpriteFont font, string text, Vector2 position, Color baseColor, float rotation, Vector2 origin, Vector2 baseScale, float maxWidth = -1f, bool ignoreColors = false)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_025f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0207: Unknown result type (might be due to invalid IL or missing references)
		//IL_0225: Unknown result type (might be due to invalid IL or missing references)
		//IL_0231: Unknown result type (might be due to invalid IL or missing references)
		//IL_0237: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		//IL_018c: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0191: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01be: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		Vector2 vector = position;
		Vector2 result = vector;
		string[] array4 = text.Split('\n');
		float x = font.MeasureString(" ").X;
		Color color = baseColor;
		float num = 1f;
		float num2 = 0f;
		string[] array2 = array4;
		for (int i = 0; i < array2.Length; i++)
		{
			string[] array5 = array2[i].Split(':');
			foreach (string text2 in array5)
			{
				if (text2.StartsWith("sss"))
				{
					if (text2.StartsWith("sss1"))
					{
						if (!ignoreColors)
						{
							color = Color.Red;
						}
					}
					else if (text2.StartsWith("sss2"))
					{
						if (!ignoreColors)
						{
							color = Color.Blue;
						}
					}
					else if (text2.StartsWith("sssr") && !ignoreColors)
					{
						color = Color.White;
					}
					continue;
				}
				string[] array3 = text2.Split(' ');
				for (int j = 0; j < array3.Length; j++)
				{
					if (j != 0)
					{
						vector.X += x * baseScale.X * num;
					}
					if (maxWidth > 0f)
					{
						float num3 = font.MeasureString(array3[j]).X * baseScale.X * num;
						if (vector.X - position.X + num3 > maxWidth)
						{
							vector.X = position.X;
							vector.Y += (float)font.LineSpacing * num2 * baseScale.Y;
							result.Y = Math.Max(result.Y, vector.Y);
							num2 = 0f;
						}
					}
					if (num2 < num)
					{
						num2 = num;
					}
					spriteBatch.DrawString(font, array3[j], vector, color, rotation, origin, baseScale * num, (SpriteEffects)0, 0f);
					vector.X += font.MeasureString(array3[j]).X * baseScale.X * num;
					result.X = Math.Max(result.X, vector.X);
				}
			}
			vector.X = position.X;
			vector.Y += (float)font.LineSpacing * num2 * baseScale.Y;
			result.Y = Math.Max(result.Y, vector.Y);
			num2 = 0f;
		}
		return result;
	}

	public static Vector2 DrawColorCodedStringWithShadow(SpriteBatch spriteBatch, DynamicSpriteFont font, string text, Vector2 position, Color baseColor, float rotation, Vector2 origin, Vector2 baseScale, float maxWidth = -1f, float spread = 2f)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		TextSnippet[] snippets = ParseMessage(text, baseColor).ToArray();
		ConvertNormalSnippets(snippets);
		DrawColorCodedStringShadow(spriteBatch, font, snippets, position, new Color(0, 0, 0, (int)((Color)(ref baseColor)).A), rotation, origin, baseScale, maxWidth, spread);
		int hoveredSnippet;
		return DrawColorCodedString(spriteBatch, font, snippets, position, Color.White, rotation, origin, baseScale, out hoveredSnippet, maxWidth);
	}

	public static Vector2 DrawColorCodedStringWithShadow(SpriteBatch spriteBatch, DynamicSpriteFont font, string text, Vector2 position, Color baseColor, Color shadowColor, float rotation, Vector2 origin, Vector2 baseScale, float maxWidth = -1f, float spread = 2f)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		TextSnippet[] snippets = ParseMessage(text, baseColor).ToArray();
		ConvertNormalSnippets(snippets);
		DrawColorCodedStringShadow(spriteBatch, font, snippets, position, shadowColor, rotation, origin, baseScale, maxWidth, spread);
		int hoveredSnippet;
		return DrawColorCodedString(spriteBatch, font, snippets, position, Color.White, rotation, origin, baseScale, out hoveredSnippet, maxWidth);
	}
}
