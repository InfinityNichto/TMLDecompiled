using System.Collections.Generic;
using System.Linq;
using ReLogic.Content;
using Terraria.IO;
using Terraria.ModLoader;

namespace Terraria.GameContent.UI.ResourceSets;

public class PlayerResourceSetsManager
{
	private Dictionary<string, IPlayerResourcesDisplaySet> _sets = new Dictionary<string, IPlayerResourcesDisplaySet>();

	private IPlayerResourcesDisplaySet _activeSet;

	private string _activeSetConfigKey;

	private bool _loadedContent;

	private static readonly string[] vanillaSets = new string[6] { "New", "NewWithText", "HorizontalBars", "HorizontalBarsWithText", "HorizontalBarsWithFullText", "Default" };

	private readonly List<string> accessKeys = new List<string>(vanillaSets);

	private int selectedSet;

	private string _activeSetConfigKeyOriginal;

	public string ActiveSetKeyName { get; private set; }

	public IPlayerResourcesDisplaySet ActiveSet => _activeSet;

	public void BindTo(Preferences preferences)
	{
		preferences.OnLoad += Configuration_OnLoad;
		preferences.OnSave += Configuration_OnSave;
	}

	private void Configuration_OnLoad(Preferences obj)
	{
		_activeSetConfigKey = obj.Get("PlayerResourcesSet", "New");
		_activeSetConfigKeyOriginal = _activeSetConfigKey;
		if (_loadedContent)
		{
			SetActiveFromLoadedConfigKey();
		}
	}

	private void Configuration_OnSave(Preferences obj)
	{
		obj.Put("PlayerResourcesSet", _activeSetConfigKey);
	}

	public void LoadContent(AssetRequestMode mode)
	{
		_sets["New"] = new FancyClassicPlayerResourcesDisplaySet("New", "New", "FancyClassic", mode);
		_sets["Default"] = new ClassicPlayerResourcesDisplaySet("Default", "Default");
		_sets["HorizontalBarsWithFullText"] = new HorizontalBarsPlayerResourcesDisplaySet("HorizontalBarsWithFullText", "HorizontalBarsWithFullText", "HorizontalBars", mode);
		_sets["HorizontalBarsWithText"] = new HorizontalBarsPlayerResourcesDisplaySet("HorizontalBarsWithText", "HorizontalBarsWithText", "HorizontalBars", mode);
		_sets["HorizontalBars"] = new HorizontalBarsPlayerResourcesDisplaySet("HorizontalBars", "HorizontalBars", "HorizontalBars", mode);
		_sets["NewWithText"] = new FancyClassicPlayerResourcesDisplaySet("NewWithText", "NewWithText", "FancyClassic", mode);
		_loadedContent = true;
		SetActiveFromLoadedConfigKey();
	}

	public void SetActiveFromLoadedConfigKey()
	{
		SetActive(_activeSetConfigKey);
	}

	private void SetActive(string name)
	{
		int index = accessKeys.FindIndex((string s) => s == name);
		if (index < 0)
		{
			index = 0;
		}
		SetActiveFrameFromIndex(index);
	}

	private void SetActiveFrame(IPlayerResourcesDisplaySet set)
	{
		_activeSet = set;
		_activeSetConfigKey = set.ConfigKey;
		ActiveSetKeyName = set.NameKey;
	}

	public void TryToHoverOverResources()
	{
		_activeSet.TryToHover();
	}

	public void Draw()
	{
		_activeSet.Draw();
	}

	public void CycleResourceSet()
	{
		SetActiveFrameFromIndex(++selectedSet % accessKeys.Count);
	}

	internal void AddModdedDisplaySets()
	{
		if (Main.dedServ)
		{
			return;
		}
		foreach (ModResourceDisplaySet display in ResourceDisplaySetLoader.moddedDisplaySets)
		{
			string key = display.ConfigKey;
			_sets[key] = display;
			accessKeys.Add(key);
		}
	}

	internal void SetActiveFromOriginalConfigKey()
	{
		if (!Main.dedServ)
		{
			SetActive(_activeSetConfigKeyOriginal);
			_activeSetConfigKeyOriginal = _activeSetConfigKey;
		}
	}

	private void SetActiveFrameFromIndex(int index)
	{
		selectedSet = index;
		SetActiveFrame(_sets[accessKeys[selectedSet]]);
	}

	internal void ResetToVanilla()
	{
		if (Main.dedServ)
		{
			return;
		}
		_activeSetConfigKey = _activeSetConfigKeyOriginal;
		foreach (string key in accessKeys.Skip(vanillaSets.Length))
		{
			_sets.Remove(key);
		}
		accessKeys.Clear();
		accessKeys.AddRange(vanillaSets);
		SetActive(_activeSetConfigKey);
	}

	public IPlayerResourcesDisplaySet GetDisplaySet(string nameKey)
	{
		if (!_sets.TryGetValue(nameKey, out var set))
		{
			return null;
		}
		return set;
	}

	public ModResourceDisplaySet GetDisplaySet<T>() where T : ModResourceDisplaySet
	{
		return GetDisplaySet(ModContent.GetInstance<T>().ConfigKey) as T;
	}
}
