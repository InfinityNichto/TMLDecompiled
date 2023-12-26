using System.Collections.Generic;
using System.Text.RegularExpressions;
using Terraria.GameInput;
using Terraria.Localization;

namespace Terraria.ModLoader;

/// <summary>
/// Represents a loaded input binding. It is suggested to access the keybind status only in ModPlayer.ProcessTriggers.
/// </summary>
public class ModKeybind
{
	internal Mod Mod { get; set; }

	internal string Name { get; set; }

	internal string FullName => Mod.Name + "/" + Name;

	internal string DefaultBinding { get; set; }

	public LocalizedText DisplayName { get; }

	public bool RetroCurrent
	{
		get
		{
			if (Main.drawingPlayerChat || Main.player[Main.myPlayer].talkNPC != -1 || Main.player[Main.myPlayer].sign != -1)
			{
				return false;
			}
			return Current;
		}
	}

	/// <summary>
	/// Returns true if this keybind is pressed currently. Useful for creating a behavior that relies on the keybind being held down.
	/// </summary>
	public bool Current => PlayerInput.Triggers.Current.KeyStatus[FullName];

	/// <summary>
	/// Returns true if this keybind has just been pressed this update. This is a fire-once-per-press behavior.
	/// </summary>
	public bool JustPressed => PlayerInput.Triggers.JustPressed.KeyStatus[FullName];

	/// <summary>
	/// Returns true if this keybind has just been released this update.
	/// </summary>
	public bool JustReleased => PlayerInput.Triggers.JustReleased.KeyStatus[FullName];

	/// <summary>
	/// Returns true if this keybind has been pressed during the previous update.
	/// </summary>
	public bool Old => PlayerInput.Triggers.Old.KeyStatus[FullName];

	internal ModKeybind(Mod mod, string name, string defaultBinding)
	{
		Mod = mod;
		Name = name;
		DefaultBinding = defaultBinding;
		DisplayName = Language.GetOrRegister($"Mods.{Mod.Name}.Keybinds.{Name}.{"DisplayName"}", () => Regex.Replace(Name, "([A-Z])", " $1").Trim());
	}

	/// <summary>
	/// Gets the currently assigned keybindings. Useful for prompts, tooltips, informing users.
	/// </summary>
	/// <param name="mode"> The InputMode. Choose between InputMode.Keyboard and InputMode.XBoxGamepad </param>
	public List<string> GetAssignedKeys(InputMode mode = InputMode.Keyboard)
	{
		return PlayerInput.CurrentProfile.InputModes[mode].KeyStatus[FullName];
	}
}
