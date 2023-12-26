using System;

namespace Terraria.ModLoader.Config;

/// <summary>
/// A label is the text shown to the user in the ModConfig UI. <br />
/// This attribute sets a custom localization key for the label of the annotated property, field, or class. <br />
/// The provided localization key must start with "$". <br />
/// Without this attribute, the localization key "Mods.{ModName}.Configs.{ConfigName}.{MemberName}.Label" will be assumed for members of ModConfig classes. <br />
/// Annotations on members of non-ModConfig classes need to supply a custom localization key using this attribute to be localized, otherwise they will appear as the member name directly. <br />
/// If the translation value of a property or field that is an object is an empty string, the label of the class will be used instead. <br />
/// Values can be interpolated into the resulting label text using <see cref="T:Terraria.ModLoader.Config.LabelArgsAttribute" />. <br />
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class LabelKeyAttribute : ConfigKeyAttribute
{
	public LabelKeyAttribute(string key)
		: base(key)
	{
	}
}
