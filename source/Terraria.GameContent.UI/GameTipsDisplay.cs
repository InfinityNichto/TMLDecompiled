using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria.GameInput;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI.Chat;

namespace Terraria.GameContent.UI;

public class GameTipsDisplay
{
	private class GameTip
	{
		private const float APPEAR_FROM = 2.5f;

		private const float APPEAR_TO = 0.5f;

		private const float DISAPPEAR_TO = -1.5f;

		private const float APPEAR_TIME = 0.5f;

		private const float DISAPPEAR_TIME = 1f;

		private const float DURATION = 11.5f;

		private LocalizedText _textKey;

		private string _formattedText;

		public float ScreenAnchorX;

		public readonly float Duration;

		public readonly double SpawnTime;

		public string Text
		{
			get
			{
				if (_textKey == null)
				{
					return "What?!";
				}
				return _formattedText;
			}
		}

		public bool IsExpired(double currentTime)
		{
			return currentTime >= SpawnTime + (double)Duration;
		}

		public bool IsExpiring(double currentTime)
		{
			return currentTime >= SpawnTime + (double)Duration - 1.0;
		}

		public GameTip(string textKey, double spawnTime)
		{
			_textKey = Language.GetText(textKey);
			SpawnTime = spawnTime;
			ScreenAnchorX = 2.5f;
			Duration = 11.5f;
			object obj = Lang.CreateDialogSubstitutionObject();
			_formattedText = _textKey.FormatWith(obj);
			_formattedText = Lang.SupportGlyphs(_formattedText);
		}

		public void Update(double currentTime)
		{
			double num = currentTime - SpawnTime;
			if (num < 0.5)
			{
				ScreenAnchorX = MathHelper.SmoothStep(2.5f, 0.5f, (float)Utils.GetLerpValue(0.0, 0.5, num, clamped: true));
			}
			else if (num >= (double)(Duration - 1f))
			{
				ScreenAnchorX = MathHelper.SmoothStep(0.5f, -1.5f, (float)Utils.GetLerpValue(Duration - 1f, Duration, num, clamped: true));
			}
			else
			{
				ScreenAnchorX = 0.5f;
			}
		}
	}

	private LocalizedText[] _tipsDefault;

	private LocalizedText[] _tipsGamepad;

	private LocalizedText[] _tipsKeyboard;

	private readonly List<GameTip> _currentTips = new List<GameTip>();

	private LocalizedText _lastTip;

	internal List<GameTipData> allTips;

	public GameTipsDisplay()
	{
		_tipsDefault = Language.FindAll(Lang.CreateDialogFilter("LoadingTips_Default."));
		_tipsGamepad = Language.FindAll(Lang.CreateDialogFilter("LoadingTips_GamePad."));
		_tipsKeyboard = Language.FindAll(Lang.CreateDialogFilter("LoadingTips_Keyboard."));
		_lastTip = null;
		Initialize();
	}

	public void Update()
	{
		double time = Main.gameTimeCache.TotalGameTime.TotalSeconds;
		_currentTips.RemoveAll((GameTip x) => x.IsExpired(time));
		bool flag = true;
		foreach (GameTip currentTip in _currentTips)
		{
			if (!currentTip.IsExpiring(time))
			{
				flag = false;
				break;
			}
		}
		if (flag)
		{
			AddNewTip(time);
		}
		foreach (GameTip currentTip2 in _currentTips)
		{
			currentTip2.Update(time);
		}
	}

	public void ClearTips()
	{
		_currentTips.Clear();
	}

	public void Draw()
	{
		//IL_0165: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_0202: Unknown result type (might be due to invalid IL or missing references)
		//IL_0204: Unknown result type (might be due to invalid IL or missing references)
		//IL_020e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0217: Unknown result type (might be due to invalid IL or missing references)
		//IL_0226: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
		SpriteBatch spriteBatch = Main.spriteBatch;
		float num = Main.screenWidth;
		float y = Main.screenHeight - 150;
		float num2 = (float)Main.screenWidth * 0.5f;
		Vector2 position = default(Vector2);
		foreach (GameTip currentTip in _currentTips)
		{
			if (currentTip.ScreenAnchorX < -0.5f || currentTip.ScreenAnchorX > 1.5f)
			{
				continue;
			}
			DynamicSpriteFont value = FontAssets.MouseText.Value;
			string text = value.CreateWrappedText(currentTip.Text, num2, Language.ActiveCulture.CultureInfo);
			if (text.Split('\n').Length > 2)
			{
				text = value.CreateWrappedText(currentTip.Text, num2 * 1.5f - 50f, Language.ActiveCulture.CultureInfo);
			}
			if (WorldGen.getGoodWorldGen)
			{
				string text2 = "";
				for (int num3 = text.Length - 1; num3 >= 0; num3--)
				{
					text2 += text.Substring(num3, 1);
				}
				text = text2;
			}
			else if (WorldGen.drunkWorldGenText)
			{
				text = string.Concat(Main.rand.Next(999999999));
				for (int i = 0; i < 14; i++)
				{
					if (Main.rand.Next(2) == 0)
					{
						text += Main.rand.Next(999999999);
					}
				}
			}
			Vector2 vector = value.MeasureString(text);
			float num4 = 1.1f;
			float num5 = 110f;
			if (vector.Y > num5)
			{
				num4 = num5 / vector.Y;
			}
			((Vector2)(ref position))._002Ector(num * currentTip.ScreenAnchorX, y);
			position -= vector * num4 * 0.5f;
			if (WorldGen.tenthAnniversaryWorldGen && !WorldGen.remixWorldGen)
			{
				ChatManager.DrawColorCodedStringWithShadow(spriteBatch, value, text, position, Color.HotPink, 0f, Vector2.Zero, new Vector2(num4, num4));
			}
			else
			{
				ChatManager.DrawColorCodedStringWithShadow(spriteBatch, value, text, position, Color.White, 0f, Vector2.Zero, new Vector2(num4, num4));
			}
		}
	}

	private void AddNewTip(double currentTime)
	{
		string textKey = "UI.Back";
		List<GameTipData> list = new List<GameTipData>(allTips);
		if (PlayerInput.UsingGamepad)
		{
			list.RemoveAll((GameTipData tip) => tip.Mod == null && tip.TipText.Key.StartsWith("LoadingTips_Keyboard"));
		}
		else
		{
			list.RemoveAll((GameTipData tip) => tip.Mod == null && tip.TipText.Key.StartsWith("LoadingTips_GamePad"));
		}
		if (_lastTip != null)
		{
			list.RemoveAll((GameTipData tip) => tip.TipText == _lastTip);
		}
		list.RemoveAll((GameTipData tip) => !tip.isVisible);
		string key = (_lastTip = ((list.Count != 0) ? list[Main.rand.Next(list.Count)].TipText : LocalizedText.Empty)).Key;
		if (Language.Exists(key))
		{
			textKey = key;
		}
		_currentTips.Add(new GameTip(textKey, currentTime));
	}

	internal void Initialize()
	{
		allTips = (from localizedText in _tipsDefault.Concat(_tipsKeyboard).Concat(_tipsGamepad)
			select new GameTipData(localizedText)).ToList();
	}

	internal void Reset()
	{
		ClearTips();
		allTips = allTips.Where((GameTipData tip) => tip.Mod == null).ToList();
		allTips.ForEach(delegate(GameTipData tip)
		{
			tip.isVisible = true;
		});
		_lastTip = null;
	}
}
