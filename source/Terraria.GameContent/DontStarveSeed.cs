using System;
using Microsoft.Xna.Framework;
using Terraria.Enums;

namespace Terraria.GameContent;

public class DontStarveSeed
{
	public static void ModifyNightColor(ref Color bgColorToSet, ref Color moonColor)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		if (Main.GetMoonPhase() != 0)
		{
			float fromValue = (float)(Main.time / 32400.0);
			Color value = bgColorToSet;
			Color black = Color.Black;
			Color value2 = bgColorToSet;
			float amount = Utils.Remap(fromValue, 0f, 0.5f, 0f, 1f);
			float amount2 = Utils.Remap(fromValue, 0.5f, 1f, 0f, 1f);
			Color color = Color.Lerp(Color.Lerp(value, black, amount), value2, amount2);
			bgColorToSet = color;
		}
	}

	public static void ModifyMinimumLightColorAtNight(ref byte minimalLight)
	{
		switch (Main.GetMoonPhase())
		{
		case MoonPhase.Empty:
			minimalLight = 1;
			break;
		case MoonPhase.QuarterAtLeft:
		case MoonPhase.QuarterAtRight:
			minimalLight = 1;
			break;
		case MoonPhase.HalfAtLeft:
		case MoonPhase.HalfAtRight:
			minimalLight = 1;
			break;
		case MoonPhase.ThreeQuartersAtLeft:
		case MoonPhase.ThreeQuartersAtRight:
			minimalLight = 1;
			break;
		case MoonPhase.Full:
			minimalLight = 45;
			break;
		}
		if (Main.bloodMoon)
		{
			minimalLight = Utils.Max(new byte[2] { minimalLight, 35 });
		}
	}

	public static void FixBiomeDarkness(ref Color bgColor, ref int R, ref int G, ref int B)
	{
		if (Main.dontStarveWorld)
		{
			R = (byte)Math.Min(((Color)(ref bgColor)).R, R);
			G = (byte)Math.Min(((Color)(ref bgColor)).G, G);
			B = (byte)Math.Min(((Color)(ref bgColor)).B, B);
		}
	}

	public static void Initialize()
	{
		Player.Hooks.OnEnterWorld += Hook_OnEnterWorld;
	}

	private static void Hook_OnEnterWorld(Player player)
	{
		player.UpdateStarvingState(withEmote: false);
	}
}
