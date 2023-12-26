using System.Collections.Generic;
using System.Linq;
using Terraria.ModLoader;

namespace Terraria.GameInput;

public class KeyConfiguration
{
	public readonly Dictionary<string, List<string>> KeyStatus = new Dictionary<string, List<string>>();

	public readonly Dictionary<string, List<string>> UnloadedModKeyStatus = new Dictionary<string, List<string>>();

	public bool DoGrappleAndInteractShareTheSameKey
	{
		get
		{
			if (KeyStatus["Grapple"].Count > 0 && KeyStatus["MouseRight"].Count > 0)
			{
				return KeyStatus["MouseRight"].Contains(KeyStatus["Grapple"][0]);
			}
			return false;
		}
	}

	public void SetupKeys()
	{
		KeyStatus.Clear();
		foreach (string knownTrigger in PlayerInput.KnownTriggers)
		{
			KeyStatus.Add(knownTrigger, new List<string>());
		}
		foreach (ModKeybind current in KeybindLoader.Keybinds)
		{
			KeyStatus.Add(current.FullName, new List<string>());
		}
	}

	public void Processkey(TriggersSet set, string newKey, InputMode mode)
	{
		foreach (KeyValuePair<string, List<string>> item in KeyStatus)
		{
			if (item.Value.Contains(newKey))
			{
				set.KeyStatus[item.Key] = true;
				set.LatestInputMode[item.Key] = mode;
			}
		}
		if (set.Up || set.Down || set.Left || set.Right || set.HotbarPlus || set.HotbarMinus || ((Main.gameMenu || Main.ingameOptionsWindow) && (set.MenuUp || set.MenuDown || set.MenuLeft || set.MenuRight)))
		{
			set.UsedMovementKey = true;
		}
	}

	public void CopyKeyState(TriggersSet oldSet, TriggersSet newSet, string newKey)
	{
		foreach (KeyValuePair<string, List<string>> item in KeyStatus)
		{
			if (item.Value.Contains(newKey))
			{
				newSet.KeyStatus[item.Key] = oldSet.KeyStatus[item.Key];
			}
		}
	}

	public void ReadPreferences(Dictionary<string, List<string>> dict)
	{
		UnloadedModKeyStatus.Clear();
		foreach (KeyValuePair<string, List<string>> item in dict)
		{
			if (KeyStatus.ContainsKey(item.Key))
			{
				KeyStatus[item.Key] = new List<string>(item.Value);
			}
			else if (item.Key.Contains('/'))
			{
				if (!UnloadedModKeyStatus.TryGetValue(item.Key, out var existing) || existing.Count < item.Value.Count)
				{
					UnloadedModKeyStatus[item.Key] = new List<string>(item.Value);
				}
			}
			else if (item.Key.Contains(": "))
			{
				string newKey = item.Key.Replace(": ", "/");
				if (!KeyStatus.ContainsKey(newKey) && !UnloadedModKeyStatus.ContainsKey(newKey))
				{
					UnloadedModKeyStatus[newKey] = new List<string>(item.Value);
				}
			}
		}
	}

	public Dictionary<string, List<string>> WritePreferences()
	{
		Dictionary<string, List<string>> dictionary = new Dictionary<string, List<string>>();
		foreach (KeyValuePair<string, List<string>> item2 in KeyStatus.Where((KeyValuePair<string, List<string>> x) => x.Value.Count > 0))
		{
			dictionary[item2.Key] = item2.Value.ToList();
		}
		foreach (KeyValuePair<string, List<string>> item in UnloadedModKeyStatus.Where((KeyValuePair<string, List<string>> x) => x.Value.Count > 0))
		{
			dictionary[item.Key] = item.Value.ToList();
		}
		if (!dictionary.ContainsKey("MouseLeft") || dictionary["MouseLeft"].Count == 0)
		{
			dictionary["MouseLeft"] = new List<string> { "Mouse1" };
		}
		if (!dictionary.ContainsKey("Inventory") || dictionary["Inventory"].Count == 0)
		{
			dictionary["Inventory"] = new List<string> { "Escape" };
		}
		return dictionary;
	}
}
