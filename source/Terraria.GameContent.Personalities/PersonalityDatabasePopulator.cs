using Terraria.ModLoader;

namespace Terraria.GameContent.Personalities;

public class PersonalityDatabasePopulator
{
	private PersonalityDatabase _currentDatabase;

	public void Populate(PersonalityDatabase database)
	{
		_currentDatabase = database;
		Populate_BiomePreferences(database);
	}

	private void Populate_BiomePreferences(PersonalityDatabase database)
	{
		OceanBiome biome = ModContent.GetInstance<OceanBiome>();
		ForestBiome biome2 = ModContent.GetInstance<ForestBiome>();
		SnowBiome biome3 = ModContent.GetInstance<SnowBiome>();
		DesertBiome biome4 = ModContent.GetInstance<DesertBiome>();
		JungleBiome biome5 = ModContent.GetInstance<JungleBiome>();
		UndergroundBiome biome6 = ModContent.GetInstance<UndergroundBiome>();
		HallowBiome biome7 = ModContent.GetInstance<HallowBiome>();
		MushroomBiome biome8 = ModContent.GetInstance<MushroomBiome>();
		AffectionLevel level = AffectionLevel.Love;
		AffectionLevel level2 = AffectionLevel.Like;
		AffectionLevel level3 = AffectionLevel.Dislike;
		AffectionLevel level4 = AffectionLevel.Hate;
		database.Register(22, new BiomePreferenceListTrait
		{
			{ level2, biome2 },
			{ level3, biome }
		});
		database.Register(17, new BiomePreferenceListTrait
		{
			{ level2, biome2 },
			{ level3, biome4 }
		});
		database.Register(588, new BiomePreferenceListTrait
		{
			{ level2, biome2 },
			{ level3, biome6 }
		});
		database.Register(633, new BiomePreferenceListTrait
		{
			{ level2, biome2 },
			{ level3, biome4 }
		});
		database.Register(441, new BiomePreferenceListTrait
		{
			{ level2, biome3 },
			{ level3, biome7 }
		});
		database.Register(124, new BiomePreferenceListTrait
		{
			{ level2, biome3 },
			{ level3, biome6 }
		});
		database.Register(209, new BiomePreferenceListTrait
		{
			{ level2, biome3 },
			{ level3, biome5 }
		});
		database.Register(142, new BiomePreferenceListTrait
		{
			{ level, biome3 },
			{ level4, biome4 }
		});
		database.Register(207, new BiomePreferenceListTrait
		{
			{ level2, biome4 },
			{ level3, biome2 }
		});
		database.Register(19, new BiomePreferenceListTrait
		{
			{ level2, biome4 },
			{ level3, biome3 }
		});
		database.Register(178, new BiomePreferenceListTrait
		{
			{ level2, biome4 },
			{ level3, biome5 }
		});
		database.Register(20, new BiomePreferenceListTrait
		{
			{ level2, biome5 },
			{ level3, biome4 }
		});
		database.Register(228, new BiomePreferenceListTrait
		{
			{ level2, biome5 },
			{ level3, biome7 }
		});
		database.Register(227, new BiomePreferenceListTrait
		{
			{ level2, biome5 },
			{ level3, biome2 }
		});
		database.Register(369, new BiomePreferenceListTrait
		{
			{ level2, biome },
			{ level3, biome4 }
		});
		database.Register(229, new BiomePreferenceListTrait
		{
			{ level2, biome },
			{ level3, biome6 }
		});
		database.Register(353, new BiomePreferenceListTrait
		{
			{ level2, biome },
			{ level3, biome3 }
		});
		database.Register(38, new BiomePreferenceListTrait
		{
			{ level2, biome6 },
			{ level3, biome }
		});
		database.Register(107, new BiomePreferenceListTrait
		{
			{ level2, biome6 },
			{ level3, biome5 }
		});
		database.Register(54, new BiomePreferenceListTrait
		{
			{ level2, biome6 },
			{ level3, biome7 }
		});
		database.Register(108, new BiomePreferenceListTrait
		{
			{ level2, biome7 },
			{ level3, biome }
		});
		database.Register(18, new BiomePreferenceListTrait
		{
			{ level2, biome7 },
			{ level3, biome3 }
		});
		database.Register(208, new BiomePreferenceListTrait
		{
			{ level2, biome7 },
			{ level3, biome6 }
		});
		database.Register(550, new BiomePreferenceListTrait
		{
			{ level2, biome7 },
			{ level3, biome3 }
		});
		database.Register(160, new BiomePreferenceListTrait { { level2, biome8 } });
	}
}
