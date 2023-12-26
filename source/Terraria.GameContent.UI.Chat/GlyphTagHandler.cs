using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ReLogic.Graphics;
using Terraria.UI.Chat;

namespace Terraria.GameContent.UI.Chat;

public class GlyphTagHandler : ITagHandler
{
	private class GlyphSnippet : TextSnippet
	{
		private int _glyphIndex;

		public GlyphSnippet(int index)
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			_glyphIndex = index;
			Color = Color.White;
		}

		public override bool UniqueDraw(bool justCheckingString, out Vector2 size, SpriteBatch spriteBatch, Vector2 position = default(Vector2), Color color = default(Color), float scale = 1f)
		{
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_0003: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			if (!justCheckingString && color != Color.Black)
			{
				int num = _glyphIndex;
				if (_glyphIndex == 25)
				{
					num = ((Main.GlobalTimeWrappedHourly % 0.6f < 0.3f) ? 17 : 18);
				}
				Texture2D value = TextureAssets.TextGlyph[0].Value;
				spriteBatch.Draw(value, position, (Rectangle?)value.Frame(25, 1, num, num / 25), color, 0f, Vector2.Zero, GlyphsScale, (SpriteEffects)0, 0f);
			}
			size = new Vector2(26f) * GlyphsScale;
			return true;
		}

		public override float GetStringLength(DynamicSpriteFont font)
		{
			return 26f * GlyphsScale;
		}
	}

	private const int GlyphsPerLine = 25;

	private const int MaxGlyphs = 26;

	public static float GlyphsScale = 1f;

	private static Dictionary<string, int> GlyphIndexes;

	TextSnippet ITagHandler.Parse(string text, Color baseColor, string options)
	{
		if (!int.TryParse(text, out var result) || result >= 26)
		{
			return new TextSnippet(text);
		}
		return new GlyphSnippet(result)
		{
			DeleteWhole = true,
			Text = "[g:" + result + "]"
		};
	}

	public static string GenerateTag(int index)
	{
		return "[g" + ":" + index + "]";
	}

	public static string GenerateTag(string keyname)
	{
		if (GlyphIndexes.TryGetValue(keyname, out var value))
		{
			return GenerateTag(value);
		}
		return keyname;
	}

	static GlyphTagHandler()
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0174: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0215: Unknown result type (might be due to invalid IL or missing references)
		//IL_022d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0247: Unknown result type (might be due to invalid IL or missing references)
		//IL_0261: Unknown result type (might be due to invalid IL or missing references)
		Dictionary<string, int> dictionary = new Dictionary<string, int>();
		Buttons val = (Buttons)4096;
		dictionary.Add(((object)(Buttons)(ref val)).ToString(), 0);
		val = (Buttons)8192;
		dictionary.Add(((object)(Buttons)(ref val)).ToString(), 1);
		val = (Buttons)32;
		dictionary.Add(((object)(Buttons)(ref val)).ToString(), 4);
		val = (Buttons)2;
		dictionary.Add(((object)(Buttons)(ref val)).ToString(), 15);
		val = (Buttons)4;
		dictionary.Add(((object)(Buttons)(ref val)).ToString(), 14);
		val = (Buttons)8;
		dictionary.Add(((object)(Buttons)(ref val)).ToString(), 13);
		val = (Buttons)1;
		dictionary.Add(((object)(Buttons)(ref val)).ToString(), 16);
		val = (Buttons)256;
		dictionary.Add(((object)(Buttons)(ref val)).ToString(), 6);
		val = (Buttons)64;
		dictionary.Add(((object)(Buttons)(ref val)).ToString(), 10);
		val = (Buttons)536870912;
		dictionary.Add(((object)(Buttons)(ref val)).ToString(), 20);
		val = (Buttons)2097152;
		dictionary.Add(((object)(Buttons)(ref val)).ToString(), 17);
		val = (Buttons)1073741824;
		dictionary.Add(((object)(Buttons)(ref val)).ToString(), 18);
		val = (Buttons)268435456;
		dictionary.Add(((object)(Buttons)(ref val)).ToString(), 19);
		val = (Buttons)8388608;
		dictionary.Add(((object)(Buttons)(ref val)).ToString(), 8);
		val = (Buttons)512;
		dictionary.Add(((object)(Buttons)(ref val)).ToString(), 7);
		val = (Buttons)128;
		dictionary.Add(((object)(Buttons)(ref val)).ToString(), 11);
		val = (Buttons)33554432;
		dictionary.Add(((object)(Buttons)(ref val)).ToString(), 24);
		val = (Buttons)134217728;
		dictionary.Add(((object)(Buttons)(ref val)).ToString(), 21);
		val = (Buttons)67108864;
		dictionary.Add(((object)(Buttons)(ref val)).ToString(), 22);
		val = (Buttons)16777216;
		dictionary.Add(((object)(Buttons)(ref val)).ToString(), 23);
		val = (Buttons)4194304;
		dictionary.Add(((object)(Buttons)(ref val)).ToString(), 9);
		val = (Buttons)16;
		dictionary.Add(((object)(Buttons)(ref val)).ToString(), 5);
		val = (Buttons)16384;
		dictionary.Add(((object)(Buttons)(ref val)).ToString(), 2);
		val = (Buttons)32768;
		dictionary.Add(((object)(Buttons)(ref val)).ToString(), 3);
		dictionary.Add("LR", 25);
		GlyphIndexes = dictionary;
	}
}
