using System.Collections.Generic;

namespace Terraria.GameContent.Personalities;

public class PersonalityDatabase
{
	private Dictionary<int, PersonalityProfile> _personalityProfiles;

	public PersonalityDatabase()
	{
		_personalityProfiles = new Dictionary<int, PersonalityProfile>();
	}

	public void Register(int npcId, IShopPersonalityTrait trait)
	{
		if (!_personalityProfiles.ContainsKey(npcId))
		{
			_personalityProfiles[npcId] = new PersonalityProfile();
		}
		_personalityProfiles[npcId].ShopModifiers.Add(trait);
	}

	public void Register(IShopPersonalityTrait trait, params int[] npcIds)
	{
		for (int i = 0; i < npcIds.Length; i++)
		{
			Register(trait, npcIds[i]);
		}
	}

	public bool TryGetProfileByNPCID(int npcId, out PersonalityProfile profile)
	{
		if (_personalityProfiles.TryGetValue(npcId, out profile))
		{
			return true;
		}
		profile = null;
		return false;
	}

	public PersonalityProfile GetOrCreateProfileByNPCID(int npcId)
	{
		if (!_personalityProfiles.TryGetValue(npcId, out var value))
		{
			value = (_personalityProfiles[npcId] = new PersonalityProfile());
		}
		return value;
	}
}
