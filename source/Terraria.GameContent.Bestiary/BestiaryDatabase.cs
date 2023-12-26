using System.Collections.Generic;
using System.Linq;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace Terraria.GameContent.Bestiary;

public class BestiaryDatabase
{
	public delegate void BestiaryEntriesPass(BestiaryEntry entry);

	private List<BestiaryEntry> _entries = new List<BestiaryEntry>();

	private List<IBestiaryEntryFilter> _filters = new List<IBestiaryEntryFilter>();

	private List<IBestiarySortStep> _sortSteps = new List<IBestiarySortStep>();

	private Dictionary<int, BestiaryEntry> _byNpcId = new Dictionary<int, BestiaryEntry>();

	private BestiaryEntry _trashEntry = new BestiaryEntry();

	private List<BestiaryEntry> _vanillaEntries = new List<BestiaryEntry>();

	private Dictionary<Mod, List<BestiaryEntry>> _byMod = new Dictionary<Mod, List<BestiaryEntry>>();

	public List<BestiaryEntry> Entries => _entries;

	public List<IBestiaryEntryFilter> Filters => _filters;

	public List<IBestiarySortStep> SortSteps => _sortSteps;

	public BestiaryEntry Register(BestiaryEntry entry)
	{
		_entries.Add(entry);
		for (int i = 0; i < entry.Info.Count; i++)
		{
			if (entry.Info[i] is NPCNetIdBestiaryInfoElement nPCNetIdBestiaryInfoElement)
			{
				_byNpcId[nPCNetIdBestiaryInfoElement.NetId] = entry;
			}
		}
		Mod mod = ContentSamples.NpcsByNetId[((NPCNetIdBestiaryInfoElement)entry.Info[0]).NetId].ModNPC?.Mod;
		if (mod == null)
		{
			_vanillaEntries.Add(entry);
		}
		else if (_byMod.ContainsKey(mod))
		{
			_byMod[mod].Add(entry);
		}
		else
		{
			_byMod.Add(mod, new List<BestiaryEntry> { entry });
		}
		return entry;
	}

	public IBestiaryEntryFilter Register(IBestiaryEntryFilter filter)
	{
		_filters.Add(filter);
		return filter;
	}

	public IBestiarySortStep Register(IBestiarySortStep sortStep)
	{
		_sortSteps.Add(sortStep);
		return sortStep;
	}

	public BestiaryEntry FindEntryByNPCID(int npcNetId)
	{
		if (_byNpcId.TryGetValue(npcNetId, out var value))
		{
			return value;
		}
		_trashEntry.Info.Clear();
		return _trashEntry;
	}

	public void Merge(ItemDropDatabase dropsDatabase)
	{
		for (int i = -65; i < NPCLoader.NPCCount; i++)
		{
			ExtractDropsForNPC(dropsDatabase, i);
		}
	}

	private void ExtractDropsForNPC(ItemDropDatabase dropsDatabase, int npcId)
	{
		BestiaryEntry bestiaryEntry = FindEntryByNPCID(npcId);
		if (bestiaryEntry == null)
		{
			return;
		}
		List<IItemDropRule> rulesForNPCID = dropsDatabase.GetRulesForNPCID(npcId, includeGlobalDrops: false);
		List<DropRateInfo> list = new List<DropRateInfo>();
		DropRateInfoChainFeed ratesInfo = new DropRateInfoChainFeed(1f);
		foreach (IItemDropRule item3 in rulesForNPCID)
		{
			item3.ReportDroprates(list, ratesInfo);
		}
		foreach (DropRateInfo item2 in list)
		{
			bestiaryEntry.Info.Add(new ItemDropBestiaryInfoElement(item2));
		}
	}

	public void ApplyPass(BestiaryEntriesPass pass)
	{
		for (int i = 0; i < _entries.Count; i++)
		{
			pass(_entries[i]);
		}
	}

	/// <summary>
	/// Gets entries from the database created by the mod specified
	/// </summary>
	/// <param name="mod">The mod to find entries from (null for Terraria)</param>
	/// <returns>A list of the entries created by the mod specified or null if it created none</returns>
	public List<BestiaryEntry> GetBestiaryEntriesByMod(Mod mod)
	{
		if (mod == null)
		{
			return _vanillaEntries;
		}
		_byMod.TryGetValue(mod, out var value);
		return value;
	}

	/// <summary>
	/// Gets the completed percent of the given mod's bestiary
	/// </summary>
	/// <param name="mod">The mod to calculate bestiary completeness (null for Terraria)</param>
	/// <returns>A float ranging from 0 to 1 representing the completeness of the bestiary or returns -1 if the mod has no entries</returns>
	public float GetCompletedPercentByMod(Mod mod)
	{
		if (mod == null)
		{
			return (float)_vanillaEntries.Count((BestiaryEntry e) => e.UIInfoProvider.GetEntryUICollectionInfo().UnlockState > BestiaryEntryUnlockState.NotKnownAtAll_0) / (float)_vanillaEntries.Count;
		}
		if (_byMod.TryGetValue(mod, out var value))
		{
			return (float)value.Count((BestiaryEntry e) => e.UIInfoProvider.GetEntryUICollectionInfo().UnlockState > BestiaryEntryUnlockState.NotKnownAtAll_0) / (float)value.Count;
		}
		return -1f;
	}
}
