using System.Collections.Generic;
using System.Linq;
using Terraria.GameContent.Personalities;
using Terraria.ID;

namespace Terraria.ModLoader;

/// <summary> This struct provides access to an NPC type's NPC &amp; Biome relationships. </summary>
public readonly struct NPCHappiness
{
	/// <summary> Allows you to modify the shop price multipliers associated with a (biome/npc type) relationship level. </summary>
	public static readonly Dictionary<AffectionLevel, float> AffectionLevelToPriceMultiplier = new Dictionary<AffectionLevel, float>
	{
		{
			AffectionLevel.Hate,
			1.12f
		},
		{
			AffectionLevel.Dislike,
			1.06f
		},
		{
			AffectionLevel.Like,
			0.94f
		},
		{
			AffectionLevel.Love,
			0.88f
		}
	};

	public readonly int NpcType;

	private NPCHappiness(int npcType)
	{
		NpcType = npcType;
	}

	public NPCHappiness SetNPCAffection<T>(AffectionLevel affectionLevel) where T : ModNPC
	{
		return SetNPCAffection(ModContent.GetInstance<T>().Type, affectionLevel);
	}

	public NPCHappiness SetNPCAffection(int npcId, AffectionLevel affectionLevel)
	{
		List<IShopPersonalityTrait> shopModifiers = Main.ShopHelper._database.GetOrCreateProfileByNPCID(NpcType).ShopModifiers;
		NPCPreferenceTrait existingEntry = (NPCPreferenceTrait)shopModifiers.SingleOrDefault((IShopPersonalityTrait t) => t is NPCPreferenceTrait nPCPreferenceTrait && nPCPreferenceTrait.NpcId == npcId);
		if (existingEntry != null)
		{
			if (affectionLevel == (AffectionLevel)0)
			{
				shopModifiers.Remove(existingEntry);
				return this;
			}
			existingEntry.Level = affectionLevel;
			return this;
		}
		shopModifiers.Add(new NPCPreferenceTrait
		{
			NpcId = npcId,
			Level = affectionLevel
		});
		return this;
	}

	public NPCHappiness SetBiomeAffection<T>(AffectionLevel affectionLevel) where T : class, ILoadable, IShoppingBiome
	{
		return SetBiomeAffection(ModContent.GetInstance<T>(), affectionLevel);
	}

	public NPCHappiness SetBiomeAffection(IShoppingBiome biome, AffectionLevel affectionLevel)
	{
		List<IShopPersonalityTrait> shopModifiers = Main.ShopHelper._database.GetOrCreateProfileByNPCID(NpcType).ShopModifiers;
		bool removal = affectionLevel == (AffectionLevel)0;
		BiomePreferenceListTrait biomePreferenceList = (BiomePreferenceListTrait)shopModifiers.SingleOrDefault((IShopPersonalityTrait t) => t is BiomePreferenceListTrait);
		if (biomePreferenceList == null)
		{
			if (removal)
			{
				return this;
			}
			shopModifiers.Add(biomePreferenceList = new BiomePreferenceListTrait());
		}
		List<BiomePreferenceListTrait.BiomePreference> biomePreferences = biomePreferenceList.Preferences;
		BiomePreferenceListTrait.BiomePreference existingEntry = biomePreferenceList.SingleOrDefault((BiomePreferenceListTrait.BiomePreference p) => p.Biome == biome);
		if (existingEntry != null)
		{
			if (removal)
			{
				biomePreferences.Remove(existingEntry);
				return this;
			}
			existingEntry.Affection = affectionLevel;
			return this;
		}
		biomePreferenceList.Add(affectionLevel, biome);
		return this;
	}

	public static NPCHappiness Get(int npcType)
	{
		return new NPCHappiness(npcType);
	}

	internal static void RegisterVanillaNpcRelationships()
	{
		for (int i = 0; i < NPCID.Count; i++)
		{
			AllPersonalitiesModifier.RegisterVanillaNpcRelationships(ContentSamples.NpcsByNetId[i]);
		}
	}
}
