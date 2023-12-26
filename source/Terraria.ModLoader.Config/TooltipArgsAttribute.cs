namespace Terraria.ModLoader.Config;

/// <summary>
/// Use to provide values to be interpolated into the tooltip of the annotated property or field.<br />
/// string arguments starting with "$" are interpreted as localization keys.<br />
/// Interpolating values can be useful for reusing common tooltips to keep localization files clean and organized.<br />
/// For example, if a mod provides toggles for several features, a common tooltip could be used for each with only the provided value being different.<br />
/// The <see href="https://github.com/tModLoader/tModLoader/wiki/Localization#string-formatting">string formatting section of the Localization wiki page</see> explains this concept further.<br />
/// <see href="https://github.com/tModLoader/tModLoader/wiki/Localization#scope-simplification">Scope simplification</see> can be used to shorten localization keys passed in.<br />
/// </summary>
public class TooltipArgsAttribute : ConfigArgsAttribute
{
	public TooltipArgsAttribute(params object[] args)
		: base(args)
	{
	}
}
