using System.Collections;
using System.Collections.Generic;

namespace Terraria.GameContent.Personalities;

public class BiomePreferenceListTrait : IShopPersonalityTrait, IEnumerable<BiomePreferenceListTrait.BiomePreference>, IEnumerable
{
	public class BiomePreference
	{
		public AffectionLevel Affection;

		public IShoppingBiome Biome;

		public BiomePreference(AffectionLevel affection, IShoppingBiome biome)
		{
			Affection = affection;
			Biome = biome;
		}
	}

	private List<BiomePreference> _preferences;

	public List<BiomePreference> Preferences => _preferences;

	public BiomePreferenceListTrait()
	{
		_preferences = new List<BiomePreference>();
	}

	public void Add(BiomePreference preference)
	{
		_preferences.Add(preference);
	}

	public void Add(AffectionLevel level, IShoppingBiome biome)
	{
		_preferences.Add(new BiomePreference(level, biome));
	}

	public void ModifyShopPrice(HelperInfo info, ShopHelper shopHelperInstance)
	{
		BiomePreference biomePreference = null;
		for (int i = 0; i < _preferences.Count; i++)
		{
			BiomePreference biomePreference2 = _preferences[i];
			if (biomePreference2.Biome.IsInBiome(info.player) && (biomePreference == null || biomePreference.Affection < biomePreference2.Affection))
			{
				biomePreference = biomePreference2;
			}
		}
		if (biomePreference != null)
		{
			ApplyPreference(biomePreference, info, shopHelperInstance);
		}
	}

	private void ApplyPreference(BiomePreference preference, HelperInfo info, ShopHelper shopHelperInstance)
	{
		string nameKey = preference.Biome.NameKey;
		shopHelperInstance.ApplyBiomeRelationshipEffect(nameKey, preference.Affection);
	}

	public IEnumerator<BiomePreference> GetEnumerator()
	{
		return _preferences.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return _preferences.GetEnumerator();
	}
}
