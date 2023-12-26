using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.Localization;
using Terraria.ModLoader.Core;
using Terraria.UI;

namespace Terraria.ModLoader.UI;

internal class UIMemoryBar : UIElement
{
	private class MemoryBarItem
	{
		internal readonly string Tooltip;

		internal readonly long Memory;

		internal readonly Color DrawColor;

		public MemoryBarItem(string tooltip, long memory, Color drawColor)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			Tooltip = tooltip;
			Memory = memory;
			DrawColor = drawColor;
		}
	}

	internal static bool RecalculateMemoryNeeded = true;

	private readonly List<MemoryBarItem> _memoryBarItems = new List<MemoryBarItem>();

	private long _maxMemory;

	private readonly Color[] _colors = (Color[])(object)new Color[6]
	{
		new Color(232, 76, 61),
		new Color(155, 88, 181),
		new Color(27, 188, 155),
		new Color(243, 156, 17),
		new Color(45, 204, 112),
		new Color(241, 196, 15)
	};

	private static readonly string[] SizeSuffixes = new string[9] { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };

	public override void OnInitialize()
	{
		Width.Set(0f, 1f);
		Height.Set(20f, 0f);
	}

	public override void OnActivate()
	{
		base.OnActivate();
		RecalculateMemoryNeeded = true;
		Task.Run((Action)RecalculateMemory);
	}

	protected override void DrawSelf(SpriteBatch spriteBatch)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		if (RecalculateMemoryNeeded)
		{
			return;
		}
		Rectangle rectangle = GetInnerDimensions().ToRectangle();
		Point mouse = default(Point);
		((Point)(ref mouse))._002Ector(Main.mouseX, Main.mouseY);
		int xOffset = 0;
		bool drawHover = false;
		MemoryBarItem hoverData = null;
		Rectangle drawArea = default(Rectangle);
		for (int i = 0; i < _memoryBarItems.Count; i++)
		{
			MemoryBarItem memoryBarData = _memoryBarItems[i];
			int width = (int)((float)rectangle.Width * ((float)memoryBarData.Memory / (float)_maxMemory));
			if (i == _memoryBarItems.Count - 1)
			{
				width = ((Rectangle)(ref rectangle)).Right - xOffset - rectangle.X;
			}
			((Rectangle)(ref drawArea))._002Ector(rectangle.X + xOffset, rectangle.Y, width, rectangle.Height);
			xOffset += width;
			Main.spriteBatch.Draw(TextureAssets.MagicPixel.Value, drawArea, memoryBarData.DrawColor);
			if (!drawHover && ((Rectangle)(ref drawArea)).Contains(mouse))
			{
				drawHover = true;
				hoverData = memoryBarData;
			}
		}
		if (drawHover && hoverData != null)
		{
			UICommon.TooltipMouseText(hoverData.Tooltip);
		}
	}

	private void RecalculateMemory()
	{
		//IL_02c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0310: Unknown result type (might be due to invalid IL or missing references)
		//IL_0259: Unknown result type (might be due to invalid IL or missing references)
		_memoryBarItems.Clear();
		_maxMemory = GetTotalMemory();
		long availableMemory = _maxMemory;
		long totalModMemory = 0L;
		int i = 0;
		foreach (KeyValuePair<string, ModMemoryUsage> entry in MemoryTracking.modMemoryUsageEstimates.OrderBy((KeyValuePair<string, ModMemoryUsage> v) => -v.Value.total))
		{
			string modName = entry.Key;
			ModMemoryUsage usage = entry.Value;
			if (usage.total > 0 && !(modName == "tModLoader"))
			{
				totalModMemory += usage.total;
				StringBuilder sb = new StringBuilder();
				sb.Append(ModLoader.GetMod(modName).DisplayName);
				StringBuilder stringBuilder = sb;
				StringBuilder stringBuilder2 = stringBuilder;
				StringBuilder.AppendInterpolatedStringHandler handler = new StringBuilder.AppendInterpolatedStringHandler(1, 1, stringBuilder);
				handler.AppendLiteral("\n");
				handler.AppendFormatted(Language.GetTextValue("tModLoader.LastLoadRamUsage", SizeSuffix(usage.total)));
				stringBuilder2.Append(ref handler);
				if (usage.managed > 0)
				{
					stringBuilder = sb;
					StringBuilder stringBuilder3 = stringBuilder;
					handler = new StringBuilder.AppendInterpolatedStringHandler(2, 1, stringBuilder);
					handler.AppendLiteral("\n ");
					handler.AppendFormatted(Language.GetTextValue("tModLoader.ManagedMemory", SizeSuffix(usage.managed)));
					stringBuilder3.Append(ref handler);
				}
				if (usage.managed > 0)
				{
					stringBuilder = sb;
					StringBuilder stringBuilder4 = stringBuilder;
					handler = new StringBuilder.AppendInterpolatedStringHandler(2, 1, stringBuilder);
					handler.AppendLiteral("\n ");
					handler.AppendFormatted(Language.GetTextValue("tModLoader.CodeMemory", SizeSuffix(usage.code)));
					stringBuilder4.Append(ref handler);
				}
				if (usage.sounds > 0)
				{
					stringBuilder = sb;
					StringBuilder stringBuilder5 = stringBuilder;
					handler = new StringBuilder.AppendInterpolatedStringHandler(2, 1, stringBuilder);
					handler.AppendLiteral("\n ");
					handler.AppendFormatted(Language.GetTextValue("tModLoader.SoundMemory", SizeSuffix(usage.sounds)));
					stringBuilder5.Append(ref handler);
				}
				if (usage.textures > 0)
				{
					stringBuilder = sb;
					StringBuilder stringBuilder6 = stringBuilder;
					handler = new StringBuilder.AppendInterpolatedStringHandler(2, 1, stringBuilder);
					handler.AppendLiteral("\n ");
					handler.AppendFormatted(Language.GetTextValue("tModLoader.TextureMemory", SizeSuffix(usage.textures)));
					stringBuilder6.Append(ref handler);
				}
				_memoryBarItems.Add(new MemoryBarItem(sb.ToString(), usage.total, _colors[i++ % _colors.Length]));
			}
		}
		long allocatedMemory = Process.GetCurrentProcess().WorkingSet64;
		long nonModMemory = allocatedMemory - totalModMemory;
		_memoryBarItems.Add(new MemoryBarItem(Language.GetTextValue("tModLoader.TerrariaMemory", SizeSuffix(nonModMemory)) + "\n " + Language.GetTextValue("tModLoader.TotalMemory", SizeSuffix(allocatedMemory)), nonModMemory, Color.DeepSkyBlue));
		long remainingMemory = availableMemory - allocatedMemory;
		_memoryBarItems.Add(new MemoryBarItem(Language.GetTextValue("tModLoader.AvailableMemory", SizeSuffix(remainingMemory)) + "\n " + Language.GetTextValue("tModLoader.TotalMemory", SizeSuffix(availableMemory)), remainingMemory, Color.Gray));
		RecalculateMemoryNeeded = false;
	}

	internal static string SizeSuffix(long value, int decimalPlaces = 1)
	{
		if (value < 0)
		{
			return "-" + SizeSuffix(-value);
		}
		if (value == 0L)
		{
			return "0.0 bytes";
		}
		int mag = (int)Math.Log(value, 1024.0);
		decimal adjustedSize = (decimal)value / (decimal)(1L << mag * 10);
		if (Math.Round(adjustedSize, decimalPlaces) >= 1000m)
		{
			mag++;
			adjustedSize /= 1024m;
		}
		return string.Format("{0:n" + decimalPlaces + "} {1}", adjustedSize, SizeSuffixes[mag]);
	}

	public static long GetTotalMemory()
	{
		return GC.GetGCMemoryInfo().TotalAvailableMemoryBytes;
	}
}
