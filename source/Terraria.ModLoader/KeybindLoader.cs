using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace Terraria.ModLoader;

public sealed class KeybindLoader : Loader
{
	internal static readonly IDictionary<string, ModKeybind> modKeybinds = new Dictionary<string, ModKeybind>();

	internal static IEnumerable<ModKeybind> Keybinds => modKeybinds.Values;

	internal override void Unload()
	{
		modKeybinds.Clear();
	}

	/// <summary>
	/// Registers a keybind with a <paramref name="name" /> and <paramref name="defaultBinding" />. Use the returned <see cref="T:Terraria.ModLoader.ModKeybind" /> to detect when buttons are pressed.
	/// </summary>
	/// <param name="mod"> The mod that this keybind will belong to. Usually, this would be your mod instance. </param>
	/// <param name="name"> The internal name of the keybind. The localization key "Mods.{ModName}.Keybinds.{KeybindName}.DisplayName" will be used for the display name. <br />It is recommended that this not have any spaces. </param>
	/// <param name="defaultBinding"> The default binding. </param>
	public static ModKeybind RegisterKeybind(Mod mod, string name, Keys defaultBinding)
	{
		return RegisterKeybind(mod, name, ((object)(Keys)(ref defaultBinding)).ToString());
	}

	/// <summary>
	/// Registers a keybind with a <paramref name="name" /> and <paramref name="defaultBinding" />. Use the returned <see cref="T:Terraria.ModLoader.ModKeybind" /> to detect when buttons are pressed.
	/// </summary>
	/// <param name="mod"> The mod that this keybind will belong to. Usually, this would be your mod instance. </param>
	/// <param name="name"> The internal name of the keybind. The localization key "Mods.{ModName}.Keybinds.{KeybindName}.DisplayName" will be used for the display name. <br />It is recommended that this not have any spaces. </param>
	/// <param name="defaultBinding"> The default binding. </param>
	public static ModKeybind RegisterKeybind(Mod mod, string name, string defaultBinding)
	{
		if (mod == null)
		{
			throw new ArgumentNullException("mod");
		}
		if (string.IsNullOrWhiteSpace(name))
		{
			throw new ArgumentNullException("name");
		}
		if (string.IsNullOrWhiteSpace(defaultBinding))
		{
			throw new ArgumentNullException("defaultBinding");
		}
		return RegisterKeybind(new ModKeybind(mod, name, defaultBinding));
	}

	private static ModKeybind RegisterKeybind(ModKeybind keybind)
	{
		modKeybinds[keybind.FullName] = keybind;
		return keybind;
	}
}
