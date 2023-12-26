using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Personalities;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Terraria.GameContent;

public class ShopHelper
{
	public const float LowestPossiblePriceMultiplier = 0.75f;

	public const float MaxHappinessAchievementPriceMultiplier = 0.82f;

	public const float HighestPossiblePriceMultiplier = 1.5f;

	private string _currentHappiness;

	private float _currentPriceAdjustment;

	private NPC _currentNPCBeingTalkedTo;

	private Player _currentPlayerTalking;

	internal PersonalityDatabase _database;

	private IShoppingBiome[] _dangerousBiomes = new IShoppingBiome[3]
	{
		new CorruptionBiome(),
		new CrimsonBiome(),
		new DungeonBiome()
	};

	internal const float likeValue = 0.94f;

	internal const float dislikeValue = 1.06f;

	internal const float loveValue = 0.88f;

	internal const float hateValue = 1.12f;

	internal void ReinitializePersonalityDatabase()
	{
		_database = new PersonalityDatabase();
		new PersonalityDatabasePopulator().Populate(_database);
	}

	public ShoppingSettings GetShoppingSettings(Player player, NPC npc)
	{
		ShoppingSettings shoppingSettings = default(ShoppingSettings);
		shoppingSettings.PriceAdjustment = 1.0;
		shoppingSettings.HappinessReport = "";
		ShoppingSettings result = shoppingSettings;
		_currentNPCBeingTalkedTo = npc;
		_currentPlayerTalking = player;
		ProcessMood(player, npc);
		result.PriceAdjustment = _currentPriceAdjustment;
		result.HappinessReport = _currentHappiness;
		return result;
	}

	private float GetSkeletonMerchantPrices(NPC npc)
	{
		float num = 1f;
		if (Main.moonPhase == 1 || Main.moonPhase == 7)
		{
			num = 1.1f;
		}
		if (Main.moonPhase == 2 || Main.moonPhase == 6)
		{
			num = 1.2f;
		}
		if (Main.moonPhase == 3 || Main.moonPhase == 5)
		{
			num = 1.3f;
		}
		if (Main.moonPhase == 4)
		{
			num = 1.4f;
		}
		if (Main.dayTime)
		{
			num += 0.1f;
		}
		return num;
	}

	private float GetTravelingMerchantPrices(NPC npc)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		Vector2 val = npc.Center / 16f;
		Vector2 value2 = default(Vector2);
		((Vector2)(ref value2))._002Ector((float)Main.spawnTileX, (float)Main.spawnTileY);
		float num = Vector2.Distance(val, value2) / (float)(Main.maxTilesX / 2);
		num = 1.5f - num;
		return (2f + num) / 3f;
	}

	private void ProcessMood(Player player, NPC npc)
	{
		_currentHappiness = "";
		_currentPriceAdjustment = 1f;
		if (Main.remixWorld)
		{
			return;
		}
		if (npc.type == 368)
		{
			_currentPriceAdjustment = 1f;
		}
		else if (npc.type == 453)
		{
			_currentPriceAdjustment = 1f;
		}
		else
		{
			if (NPCID.Sets.IsTownPet[npc.type])
			{
				return;
			}
			if (IsNotReallyTownNPC(npc))
			{
				_currentPriceAdjustment = 1f;
				return;
			}
			if (RuinMoodIfHomeless(npc))
			{
				_currentPriceAdjustment = 1000f;
			}
			else if (IsFarFromHome(npc))
			{
				_currentPriceAdjustment = 1000f;
			}
			if (IsPlayerInEvilBiomes(player))
			{
				_currentPriceAdjustment = 1000f;
			}
			int npcsWithinHouse;
			int npcsWithinVillage;
			List<NPC> nearbyResidentNPCs = GetNearbyResidentNPCs(npc, out npcsWithinHouse, out npcsWithinVillage);
			bool flag = true;
			float num = 1.05f;
			if (npc.type == 663)
			{
				flag = false;
				num = 1f;
				if (npcsWithinHouse < 2 && npcsWithinVillage < 2)
				{
					AddHappinessReportText("HateLonely");
					_currentPriceAdjustment = 1000f;
				}
			}
			if (npcsWithinHouse > 3)
			{
				for (int i = 3; i < npcsWithinHouse; i++)
				{
					_currentPriceAdjustment *= num;
				}
				if (npcsWithinHouse > 6)
				{
					AddHappinessReportText("HateCrowded");
				}
				else
				{
					AddHappinessReportText("DislikeCrowded");
				}
			}
			if (flag && npcsWithinHouse <= 2 && npcsWithinVillage < 4)
			{
				AddHappinessReportText("LoveSpace");
				_currentPriceAdjustment *= 0.95f;
			}
			bool[] array = new bool[NPCLoader.NPCCount];
			foreach (NPC item in nearbyResidentNPCs)
			{
				array[item.type] = true;
			}
			HelperInfo helperInfo = default(HelperInfo);
			helperInfo.player = player;
			helperInfo.npc = npc;
			helperInfo.NearbyNPCs = nearbyResidentNPCs;
			helperInfo.nearbyNPCsByType = array;
			HelperInfo info = helperInfo;
			if (_database.TryGetProfileByNPCID(npc.type, out var personalityProfile))
			{
				foreach (IShopPersonalityTrait shopModifier in personalityProfile.ShopModifiers)
				{
					shopModifier.ModifyShopPrice(info, this);
				}
			}
			new AllPersonalitiesModifier().ModifyShopPrice(info, this);
			if (_currentHappiness == "")
			{
				AddHappinessReportText("Content");
			}
			_currentPriceAdjustment = LimitAndRoundMultiplier(_currentPriceAdjustment);
		}
	}

	private float LimitAndRoundMultiplier(float priceAdjustment)
	{
		priceAdjustment = MathHelper.Clamp(priceAdjustment, 0.75f, 1.5f);
		priceAdjustment = (float)Math.Round(priceAdjustment * 100f) / 100f;
		return priceAdjustment;
	}

	public static string BiomeNameByKey(string biomeNameKey)
	{
		return Language.GetTextValue(biomeNameKey.StartsWith("Mods.") ? biomeNameKey : ("TownNPCMoodBiomes." + biomeNameKey));
	}

	private void AddHappinessReportText(string textKeyInCategory, object substitutes = null, int otherNPCType = 0)
	{
		string text = "TownNPCMood_" + NPCID.Search.GetName(_currentNPCBeingTalkedTo.netID);
		if (textKeyInCategory == "Princess_LovesNPC")
		{
			text = ModContent.GetModNPC(otherNPCType).GetLocalizationKey("TownNPCMood");
		}
		else
		{
			ModNPC modNPC = _currentNPCBeingTalkedTo.ModNPC;
			if (modNPC != null)
			{
				text = modNPC.GetLocalizationKey("TownNPCMood");
			}
		}
		if (_currentNPCBeingTalkedTo.type == 633 && _currentNPCBeingTalkedTo.altTexture == 2)
		{
			text += "Transformed";
		}
		string textValueWith = Language.GetTextValueWith(text + "." + textKeyInCategory, substitutes);
		_currentHappiness = _currentHappiness + textValueWith + " ";
	}

	internal void ApplyNpcRelationshipEffect(int npcType, AffectionLevel affectionLevel)
	{
		if (affectionLevel != 0 && Enum.IsDefined(affectionLevel))
		{
			AddHappinessReportText($"{affectionLevel}NPC", new
			{
				NPCName = NPC.GetFullnameByID(npcType)
			});
			_currentPriceAdjustment *= NPCHappiness.AffectionLevelToPriceMultiplier[affectionLevel];
		}
	}

	internal void ApplyBiomeRelationshipEffect(string biomeNameKey, AffectionLevel affectionLevel)
	{
		if (affectionLevel != 0 && Enum.IsDefined(affectionLevel))
		{
			AddHappinessReportText($"{affectionLevel}Biome", new
			{
				BiomeName = BiomeNameByKey(biomeNameKey)
			});
			_currentPriceAdjustment *= NPCHappiness.AffectionLevelToPriceMultiplier[affectionLevel];
		}
	}

	internal void LoveNPCByTypeName(int npcType)
	{
		if (npcType >= NPCID.Count && _currentNPCBeingTalkedTo.type == 663)
		{
			AddHappinessReportText("Princess_LovesNPC", new
			{
				NPCName = NPC.GetFullnameByID(npcType)
			}, npcType);
		}
		else
		{
			AddHappinessReportText("LoveNPC_" + NPCID.Search.GetName(npcType), new
			{
				NPCName = NPC.GetFullnameByID(npcType)
			});
		}
		_currentPriceAdjustment *= NPCHappiness.AffectionLevelToPriceMultiplier[AffectionLevel.Love];
	}

	internal void LikePrincess()
	{
		AddHappinessReportText("LikeNPC_Princess", new
		{
			NPCName = NPC.GetFullnameByID(663)
		});
		_currentPriceAdjustment *= NPCHappiness.AffectionLevelToPriceMultiplier[AffectionLevel.Like];
	}

	private List<NPC> GetNearbyResidentNPCs(NPC npc, out int npcsWithinHouse, out int npcsWithinVillage)
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		List<NPC> list = new List<NPC>();
		npcsWithinHouse = 0;
		npcsWithinVillage = 0;
		Vector2 value = default(Vector2);
		((Vector2)(ref value))._002Ector((float)npc.homeTileX, (float)npc.homeTileY);
		if (npc.homeless)
		{
			((Vector2)(ref value))._002Ector(npc.Center.X / 16f, npc.Center.Y / 16f);
		}
		Vector2 value2 = default(Vector2);
		for (int i = 0; i < 200; i++)
		{
			if (i == npc.whoAmI)
			{
				continue;
			}
			NPC nPC = Main.npc[i];
			if (nPC.active && nPC.townNPC && !IsNotReallyTownNPC(nPC) && !WorldGen.TownManager.CanNPCsLiveWithEachOther_ShopHelper(npc, nPC))
			{
				((Vector2)(ref value2))._002Ector((float)nPC.homeTileX, (float)nPC.homeTileY);
				if (nPC.homeless)
				{
					value2 = nPC.Center / 16f;
				}
				float num = Vector2.Distance(value, value2);
				if (num < 25f)
				{
					list.Add(nPC);
					npcsWithinHouse++;
				}
				else if (num < 120f)
				{
					npcsWithinVillage++;
				}
			}
		}
		return list;
	}

	private bool RuinMoodIfHomeless(NPC npc)
	{
		if (npc.homeless)
		{
			AddHappinessReportText("NoHome");
		}
		return npc.homeless;
	}

	private bool IsFarFromHome(NPC npc)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		Vector2 val = new Vector2((float)npc.homeTileX, (float)npc.homeTileY);
		Vector2 value2 = default(Vector2);
		((Vector2)(ref value2))._002Ector(npc.Center.X / 16f, npc.Center.Y / 16f);
		if (Vector2.Distance(val, value2) > 120f)
		{
			AddHappinessReportText("FarFromHome");
			return true;
		}
		return false;
	}

	private bool IsPlayerInEvilBiomes(Player player)
	{
		for (int i = 0; i < _dangerousBiomes.Length; i++)
		{
			IShoppingBiome aShoppingBiome = _dangerousBiomes[i];
			if (aShoppingBiome.IsInBiome(player))
			{
				AddHappinessReportText("HateBiome", new
				{
					BiomeName = BiomeNameByKey(aShoppingBiome.NameKey)
				});
				return true;
			}
		}
		return false;
	}

	private bool IsNotReallyTownNPC(NPC npc)
	{
		int type = npc.type;
		if (NPCID.Sets.NoTownNPCHappiness[type])
		{
			return true;
		}
		return false;
	}
}
