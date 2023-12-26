using System;
using Terraria.Localization;

namespace Terraria.ModLoader.UI.ModBrowser;

internal class TimeHelper
{
	private const int SECOND = 1;

	private const int MINUTE = 60;

	private const int HOUR = 3600;

	private const int DAY = 86400;

	private const int MONTH = 2592000;

	public static string HumanTimeSpanString(DateTime yourDate)
	{
		TimeSpan ts = new TimeSpan(DateTime.UtcNow.Ticks - yourDate.Ticks);
		double delta = Math.Abs(ts.TotalSeconds);
		if (delta < 60.0)
		{
			return Language.GetTextValue("tModLoader.XSecondsAgo", ts.Seconds);
		}
		if (delta < 3600.0)
		{
			return Language.GetTextValue("tModLoader.XMinutesAgo", ts.Minutes);
		}
		if (delta < 86400.0)
		{
			return Language.GetTextValue("tModLoader.XHoursAgo", ts.Hours);
		}
		if (delta < 2592000.0)
		{
			return Language.GetTextValue("tModLoader.XDaysAgo", ts.Days);
		}
		if (delta < 31104000.0)
		{
			int months = Convert.ToInt32(Math.Floor((double)ts.Days / 30.0));
			return Language.GetTextValue("tModLoader.XMonthsAgo", months);
		}
		int years = Convert.ToInt32(Math.Floor((double)ts.Days / 365.0));
		return Language.GetTextValue("tModLoader.XYearsAgo", years);
	}
}
