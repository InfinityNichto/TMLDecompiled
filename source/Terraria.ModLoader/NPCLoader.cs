using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.Personalities;
using Terraria.GameContent.UI;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader.Core;
using Terraria.ModLoader.IO;
using Terraria.ModLoader.Utilities;

namespace Terraria.ModLoader;

/// <summary>
/// This serves as the central class from which NPC-related functions are carried out. It also stores a list of mod NPCs by ID.
/// </summary>
public static class NPCLoader
{
	private delegate void DelegateSetBestiary(NPC npc, BestiaryDatabase database, BestiaryEntry bestiaryEntry);

	private delegate ITownNPCProfile DelegateModifyTownNPCProfile(NPC npc);

	private delegate void DelegateUpdateLifeRegen(NPC npc, ref int damage);

	private delegate bool DelegateCanHitPlayer(NPC npc, Player target, ref int cooldownSlot);

	private delegate void DelegateModifyHitPlayer(NPC npc, Player target, ref Player.HurtModifiers modifiers);

	private delegate void DelegateModifyHitNPC(NPC npc, NPC target, ref NPC.HitModifiers modifiers);

	private delegate void DelegateModifyHitByItem(NPC npc, Player player, Item item, ref NPC.HitModifiers modifiers);

	private delegate void DelegateModifyHitByProjectile(NPC npc, Projectile projectile, ref NPC.HitModifiers modifiers);

	private delegate void DelegateAddModifyIncomingHit(NPC npc, ref NPC.HitModifiers modifiers);

	private delegate void DelegateBossHeadSlot(NPC npc, ref int index);

	private delegate void DelegateBossHeadRotation(NPC npc, ref float rotation);

	private delegate void DelegateBossHeadSpriteEffects(NPC npc, ref SpriteEffects spriteEffects);

	private delegate void DelegateDrawEffects(NPC npc, ref Color drawColor);

	private delegate bool DelegatePreDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor);

	private delegate void DelegatePostDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor);

	private delegate bool? DelegateDrawHealthBar(NPC npc, byte bhPosition, ref float scale, ref Vector2 position);

	private delegate void DelegateEditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns);

	private delegate void DelegateEditSpawnRange(Player player, ref int spawnRangeX, ref int spawnRangeY, ref int safeRangeX, ref int safeRangeY);

	private delegate void DelegateModifyTypeName(NPC npc, ref string typeName);

	private delegate void DelegateModifyHoverBoundingBox(NPC npc, ref Rectangle boundingBox);

	private delegate void DelegateGetChat(NPC npc, ref string chat);

	private delegate void DelegateOnChatButtonClicked(NPC npc, bool firstButton);

	private delegate void DelegateModifyShop(NPCShop shop);

	private delegate void DelegateModifyActiveShop(NPC npc, string shopName, Item[] items);

	private delegate void DelegateSetupTravelShop(int[] shop, ref int nextSlot);

	private delegate void DelegateBuffTownNPC(ref float damageMult, ref int defense);

	private delegate void DelegateTownNPCAttackStrength(NPC npc, ref int damage, ref float knockback);

	private delegate void DelegateTownNPCAttackCooldown(NPC npc, ref int cooldown, ref int randExtraCooldown);

	private delegate void DelegateTownNPCAttackProj(NPC npc, ref int projType, ref int attackDelay);

	private delegate void DelegateTownNPCAttackProjSpeed(NPC npc, ref float multiplier, ref float gravityCorrection, ref float randomOffset);

	private delegate void DelegateTownNPCAttackShoot(NPC npc, ref bool inBetweenShots);

	private delegate void DelegateTownNPCAttackMagic(NPC npc, ref float auraLightMultiplier);

	private delegate void DelegateTownNPCAttackSwing(NPC npc, ref int itemWidth, ref int itemHeight);

	private delegate void DelegateDrawTownAttackGun(NPC npc, ref Texture2D item, ref Rectangle itemFrame, ref float scale, ref int horizontalHoldoutOffset);

	private delegate void DelegateDrawTownAttackSwing(NPC npc, ref Texture2D item, ref Rectangle itemFrame, ref int itemSize, ref float scale, ref Vector2 offset);

	private delegate bool DelegateModifyCollisionData(NPC npc, Rectangle victimHitbox, ref int immunityCooldownSlot, ref MultipliableFloat damageMultiplier, ref Rectangle npcHitbox);

	private delegate bool DelegateNeedSaving(NPC npc);

	internal static readonly IList<ModNPC> npcs = new List<ModNPC>();

	internal static readonly IDictionary<int, int> bannerToItem = new Dictionary<int, int>();

	/// <summary>
	/// Allows you to stop an NPC from dropping loot by adding item IDs to this list. This list will be cleared whenever NPCLoot ends. Useful for either removing an item or change the drop rate of an item in the NPC's loot table. To change the drop rate of an item, use the PreNPCLoot hook, spawn the item yourself, then add the item's ID to this list.
	/// </summary>
	public static readonly IList<int> blockLoot = new List<int>();

	private static readonly List<GlobalHookList<GlobalNPC>> hooks = new List<GlobalHookList<GlobalNPC>>();

	private static readonly List<GlobalHookList<GlobalNPC>> modHooks = new List<GlobalHookList<GlobalNPC>>();

	private static GlobalHookList<GlobalNPC> HookOnSpawn = AddHook((Expression<Func<GlobalNPC, Action<NPC, IEntitySource>>>)((GlobalNPC g) => g.OnSpawn));

	private static GlobalHookList<GlobalNPC> HookApplyDifficultyAndPlayerScaling = AddHook((Expression<Func<GlobalNPC, Action<NPC, int, float, float>>>)((GlobalNPC g) => g.ApplyDifficultyAndPlayerScaling));

	private static GlobalHookList<GlobalNPC> HookSetBestiary = AddHook((Expression<Func<GlobalNPC, DelegateSetBestiary>>)((GlobalNPC g) => g.SetBestiary));

	private static GlobalHookList<GlobalNPC> HookModifyTownNPCProfile = AddHook((Expression<Func<GlobalNPC, DelegateModifyTownNPCProfile>>)((GlobalNPC g) => g.ModifyTownNPCProfile));

	private static GlobalHookList<GlobalNPC> HookResetEffects = AddHook((Expression<Func<GlobalNPC, Action<NPC>>>)((GlobalNPC g) => g.ResetEffects));

	private static GlobalHookList<GlobalNPC> HookPreAI = AddHook((Expression<Func<GlobalNPC, Func<NPC, bool>>>)((GlobalNPC g) => g.PreAI));

	private static GlobalHookList<GlobalNPC> HookAI = AddHook((Expression<Func<GlobalNPC, Action<NPC>>>)((GlobalNPC g) => g.AI));

	private static GlobalHookList<GlobalNPC> HookPostAI = AddHook((Expression<Func<GlobalNPC, Action<NPC>>>)((GlobalNPC g) => g.PostAI));

	private static GlobalHookList<GlobalNPC> HookSendExtraAI = AddHook((Expression<Func<GlobalNPC, Action<NPC, BitWriter, BinaryWriter>>>)((GlobalNPC g) => g.SendExtraAI));

	private static GlobalHookList<GlobalNPC> HookReceiveExtraAI = AddHook((Expression<Func<GlobalNPC, Action<NPC, BitReader, BinaryReader>>>)((GlobalNPC g) => g.ReceiveExtraAI));

	private static GlobalHookList<GlobalNPC> HookFindFrame = AddHook((Expression<Func<GlobalNPC, Action<NPC, int>>>)((GlobalNPC g) => g.FindFrame));

	private static GlobalHookList<GlobalNPC> HookHitEffect = AddHook((Expression<Func<GlobalNPC, Action<NPC, NPC.HitInfo>>>)((GlobalNPC g) => g.HitEffect));

	private static GlobalHookList<GlobalNPC> HookUpdateLifeRegen = AddHook((Expression<Func<GlobalNPC, DelegateUpdateLifeRegen>>)((GlobalNPC g) => g.UpdateLifeRegen));

	private static GlobalHookList<GlobalNPC> HookCheckActive = AddHook((Expression<Func<GlobalNPC, Func<NPC, bool>>>)((GlobalNPC g) => g.CheckActive));

	private static GlobalHookList<GlobalNPC> HookCheckDead = AddHook((Expression<Func<GlobalNPC, Func<NPC, bool>>>)((GlobalNPC g) => g.CheckDead));

	private static GlobalHookList<GlobalNPC> HookSpecialOnKill = AddHook((Expression<Func<GlobalNPC, Func<NPC, bool>>>)((GlobalNPC g) => g.SpecialOnKill));

	private static GlobalHookList<GlobalNPC> HookPreKill = AddHook((Expression<Func<GlobalNPC, Func<NPC, bool>>>)((GlobalNPC g) => g.PreKill));

	private static GlobalHookList<GlobalNPC> HookOnKill = AddHook((Expression<Func<GlobalNPC, Action<NPC>>>)((GlobalNPC g) => g.OnKill));

	private static GlobalHookList<GlobalNPC> HookModifyNPCLoot = AddHook((Expression<Func<GlobalNPC, Action<NPC, NPCLoot>>>)((GlobalNPC g) => g.ModifyNPCLoot));

	private static GlobalHookList<GlobalNPC> HookModifyGlobalLoot = AddHook((Expression<Func<GlobalNPC, Action<GlobalLoot>>>)((GlobalNPC g) => g.ModifyGlobalLoot));

	private static GlobalHookList<GlobalNPC> HookCanFallThroughPlatforms = AddHook((Expression<Func<GlobalNPC, Func<NPC, bool?>>>)((GlobalNPC g) => g.CanFallThroughPlatforms));

	private static GlobalHookList<GlobalNPC> HookCanBeCaughtBy = AddHook((Expression<Func<GlobalNPC, Func<NPC, Item, Player, bool?>>>)((GlobalNPC g) => g.CanBeCaughtBy));

	private static GlobalHookList<GlobalNPC> HookOnCaughtBy = AddHook((Expression<Func<GlobalNPC, Action<NPC, Player, Item, bool>>>)((GlobalNPC g) => g.OnCaughtBy));

	private static GlobalHookList<GlobalNPC> HookPickEmote = AddHook((Expression<Func<GlobalNPC, Func<NPC, Player, List<int>, WorldUIAnchor, int?>>>)((GlobalNPC g) => g.PickEmote));

	private static GlobalHookList<GlobalNPC> HookCanHitPlayer = AddHook((Expression<Func<GlobalNPC, DelegateCanHitPlayer>>)((GlobalNPC g) => g.CanHitPlayer));

	private static GlobalHookList<GlobalNPC> HookModifyHitPlayer = AddHook((Expression<Func<GlobalNPC, DelegateModifyHitPlayer>>)((GlobalNPC g) => g.ModifyHitPlayer));

	private static GlobalHookList<GlobalNPC> HookOnHitPlayer = AddHook((Expression<Func<GlobalNPC, Action<NPC, Player, Player.HurtInfo>>>)((GlobalNPC g) => g.OnHitPlayer));

	private static GlobalHookList<GlobalNPC> HookCanHitNPC = AddHook((Expression<Func<GlobalNPC, Func<NPC, NPC, bool>>>)((GlobalNPC g) => g.CanHitNPC));

	private static GlobalHookList<GlobalNPC> HookCanBeHitByNPC = AddHook((Expression<Func<GlobalNPC, Func<NPC, NPC, bool>>>)((GlobalNPC g) => g.CanBeHitByNPC));

	private static GlobalHookList<GlobalNPC> HookModifyHitNPC = AddHook((Expression<Func<GlobalNPC, DelegateModifyHitNPC>>)((GlobalNPC g) => g.ModifyHitNPC));

	private static GlobalHookList<GlobalNPC> HookOnHitNPC = AddHook((Expression<Func<GlobalNPC, Action<NPC, NPC, NPC.HitInfo>>>)((GlobalNPC g) => g.OnHitNPC));

	private static GlobalHookList<GlobalNPC> HookCanBeHitByItem = AddHook((Expression<Func<GlobalNPC, Func<NPC, Player, Item, bool?>>>)((GlobalNPC g) => g.CanBeHitByItem));

	private static GlobalHookList<GlobalNPC> HookCanCollideWithPlayerMeleeAttack = AddHook((Expression<Func<GlobalNPC, Func<NPC, Player, Item, Rectangle, bool?>>>)((GlobalNPC g) => g.CanCollideWithPlayerMeleeAttack));

	private static GlobalHookList<GlobalNPC> HookModifyHitByItem = AddHook((Expression<Func<GlobalNPC, DelegateModifyHitByItem>>)((GlobalNPC g) => g.ModifyHitByItem));

	private static GlobalHookList<GlobalNPC> HookOnHitByItem = AddHook((Expression<Func<GlobalNPC, Action<NPC, Player, Item, NPC.HitInfo, int>>>)((GlobalNPC g) => g.OnHitByItem));

	private static GlobalHookList<GlobalNPC> HookCanBeHitByProjectile = AddHook((Expression<Func<GlobalNPC, Func<NPC, Projectile, bool?>>>)((GlobalNPC g) => g.CanBeHitByProjectile));

	private static GlobalHookList<GlobalNPC> HookModifyHitByProjectile = AddHook((Expression<Func<GlobalNPC, DelegateModifyHitByProjectile>>)((GlobalNPC g) => g.ModifyHitByProjectile));

	private static GlobalHookList<GlobalNPC> HookOnHitByProjectile = AddHook((Expression<Func<GlobalNPC, Action<NPC, Projectile, NPC.HitInfo, int>>>)((GlobalNPC g) => g.OnHitByProjectile));

	private static GlobalHookList<GlobalNPC> HookAddModifyIncomingHit = AddHook((Expression<Func<GlobalNPC, DelegateAddModifyIncomingHit>>)((GlobalNPC g) => g.ModifyIncomingHit));

	private static GlobalHookList<GlobalNPC> HookBossHeadSlot = AddHook((Expression<Func<GlobalNPC, DelegateBossHeadSlot>>)((GlobalNPC g) => g.BossHeadSlot));

	private static GlobalHookList<GlobalNPC> HookBossHeadRotation = AddHook((Expression<Func<GlobalNPC, DelegateBossHeadRotation>>)((GlobalNPC g) => g.BossHeadRotation));

	private static GlobalHookList<GlobalNPC> HookBossHeadSpriteEffects = AddHook((Expression<Func<GlobalNPC, DelegateBossHeadSpriteEffects>>)((GlobalNPC g) => g.BossHeadSpriteEffects));

	private static GlobalHookList<GlobalNPC> HookGetAlpha = AddHook((Expression<Func<GlobalNPC, Func<NPC, Color, Color?>>>)((GlobalNPC g) => g.GetAlpha));

	private static GlobalHookList<GlobalNPC> HookDrawEffects = AddHook((Expression<Func<GlobalNPC, DelegateDrawEffects>>)((GlobalNPC g) => g.DrawEffects));

	private static GlobalHookList<GlobalNPC> HookPreDraw = AddHook((Expression<Func<GlobalNPC, DelegatePreDraw>>)((GlobalNPC g) => g.PreDraw));

	private static GlobalHookList<GlobalNPC> HookPostDraw = AddHook((Expression<Func<GlobalNPC, DelegatePostDraw>>)((GlobalNPC g) => g.PostDraw));

	private static GlobalHookList<GlobalNPC> HookDrawBehind = AddHook((Expression<Func<GlobalNPC, Action<NPC, int>>>)((GlobalNPC g) => g.DrawBehind));

	private static GlobalHookList<GlobalNPC> HookDrawHealthBar = AddHook((Expression<Func<GlobalNPC, DelegateDrawHealthBar>>)((GlobalNPC g) => g.DrawHealthBar));

	private static GlobalHookList<GlobalNPC> HookEditSpawnRate = AddHook((Expression<Func<GlobalNPC, DelegateEditSpawnRate>>)((GlobalNPC g) => g.EditSpawnRate));

	private static GlobalHookList<GlobalNPC> HookEditSpawnRange = AddHook((Expression<Func<GlobalNPC, DelegateEditSpawnRange>>)((GlobalNPC g) => g.EditSpawnRange));

	private static GlobalHookList<GlobalNPC> HookEditSpawnPool = AddHook((Expression<Func<GlobalNPC, Action<Dictionary<int, float>, NPCSpawnInfo>>>)((GlobalNPC g) => g.EditSpawnPool));

	private static GlobalHookList<GlobalNPC> HookSpawnNPC = AddHook((Expression<Func<GlobalNPC, Action<int, int, int>>>)((GlobalNPC g) => g.SpawnNPC));

	private static GlobalHookList<GlobalNPC> HookModifyTypeName = AddHook((Expression<Func<GlobalNPC, DelegateModifyTypeName>>)((GlobalNPC g) => g.ModifyTypeName));

	private static GlobalHookList<GlobalNPC> HookModifyHoverBoundingBox = AddHook((Expression<Func<GlobalNPC, DelegateModifyHoverBoundingBox>>)((GlobalNPC g) => g.ModifyHoverBoundingBox));

	private static GlobalHookList<GlobalNPC> HookModifyNPCNameList = AddHook((Expression<Func<GlobalNPC, Action<NPC, List<string>>>>)((GlobalNPC g) => g.ModifyNPCNameList));

	private static GlobalHookList<GlobalNPC> HookCanChat = AddHook((Expression<Func<GlobalNPC, Func<NPC, bool?>>>)((GlobalNPC g) => g.CanChat));

	private static GlobalHookList<GlobalNPC> HookGetChat = AddHook((Expression<Func<GlobalNPC, DelegateGetChat>>)((GlobalNPC g) => g.GetChat));

	private static GlobalHookList<GlobalNPC> HookPreChatButtonClicked = AddHook((Expression<Func<GlobalNPC, Func<NPC, bool, bool>>>)((GlobalNPC g) => g.PreChatButtonClicked));

	private static GlobalHookList<GlobalNPC> HookOnChatButtonClicked = AddHook((Expression<Func<GlobalNPC, DelegateOnChatButtonClicked>>)((GlobalNPC g) => g.OnChatButtonClicked));

	private static GlobalHookList<GlobalNPC> HookModifyShop = AddHook((Expression<Func<GlobalNPC, DelegateModifyShop>>)((GlobalNPC g) => g.ModifyShop));

	private static GlobalHookList<GlobalNPC> HookModifyActiveShop = AddHook((Expression<Func<GlobalNPC, DelegateModifyActiveShop>>)((GlobalNPC g) => g.ModifyActiveShop));

	private static GlobalHookList<GlobalNPC> HookSetupTravelShop = AddHook((Expression<Func<GlobalNPC, DelegateSetupTravelShop>>)((GlobalNPC g) => g.SetupTravelShop));

	private static GlobalHookList<GlobalNPC> HookCanGoToStatue = AddHook((Expression<Func<GlobalNPC, Func<NPC, bool, bool?>>>)((GlobalNPC g) => g.CanGoToStatue));

	private static GlobalHookList<GlobalNPC> HookOnGoToStatue = AddHook((Expression<Func<GlobalNPC, Action<NPC, bool>>>)((GlobalNPC g) => g.OnGoToStatue));

	private static GlobalHookList<GlobalNPC> HookBuffTownNPC = AddHook((Expression<Func<GlobalNPC, DelegateBuffTownNPC>>)((GlobalNPC g) => g.BuffTownNPC));

	private static GlobalHookList<GlobalNPC> HookTownNPCAttackStrength = AddHook((Expression<Func<GlobalNPC, DelegateTownNPCAttackStrength>>)((GlobalNPC g) => g.TownNPCAttackStrength));

	private static GlobalHookList<GlobalNPC> HookTownNPCAttackCooldown = AddHook((Expression<Func<GlobalNPC, DelegateTownNPCAttackCooldown>>)((GlobalNPC g) => g.TownNPCAttackCooldown));

	private static GlobalHookList<GlobalNPC> HookTownNPCAttackProj = AddHook((Expression<Func<GlobalNPC, DelegateTownNPCAttackProj>>)((GlobalNPC g) => g.TownNPCAttackProj));

	private static GlobalHookList<GlobalNPC> HookTownNPCAttackProjSpeed = AddHook((Expression<Func<GlobalNPC, DelegateTownNPCAttackProjSpeed>>)((GlobalNPC g) => g.TownNPCAttackProjSpeed));

	private static GlobalHookList<GlobalNPC> HookTownNPCAttackShoot = AddHook((Expression<Func<GlobalNPC, DelegateTownNPCAttackShoot>>)((GlobalNPC g) => g.TownNPCAttackShoot));

	private static GlobalHookList<GlobalNPC> HookTownNPCAttackMagic = AddHook((Expression<Func<GlobalNPC, DelegateTownNPCAttackMagic>>)((GlobalNPC g) => g.TownNPCAttackMagic));

	private static GlobalHookList<GlobalNPC> HookTownNPCAttackSwing = AddHook((Expression<Func<GlobalNPC, DelegateTownNPCAttackSwing>>)((GlobalNPC g) => g.TownNPCAttackSwing));

	private static GlobalHookList<GlobalNPC> HookDrawTownAttackGun = AddHook((Expression<Func<GlobalNPC, DelegateDrawTownAttackGun>>)((GlobalNPC g) => g.DrawTownAttackGun));

	private static GlobalHookList<GlobalNPC> HookDrawTownAttackSwing = AddHook((Expression<Func<GlobalNPC, DelegateDrawTownAttackSwing>>)((GlobalNPC g) => g.DrawTownAttackSwing));

	private static GlobalHookList<GlobalNPC> HookModifyCollisionData = AddHook((Expression<Func<GlobalNPC, DelegateModifyCollisionData>>)((GlobalNPC g) => g.ModifyCollisionData));

	private static GlobalHookList<GlobalNPC> HookNeedSaving = AddHook((Expression<Func<GlobalNPC, DelegateNeedSaving>>)((GlobalNPC g) => g.NeedSaving));

	internal static GlobalHookList<GlobalNPC> HookSaveData = AddHook((Expression<Func<GlobalNPC, Action<NPC, TagCompound>>>)((GlobalNPC g) => g.SaveData));

	public static int NPCCount { get; private set; } = NPCID.Count;


	private static GlobalHookList<GlobalNPC> AddHook<F>(Expression<Func<GlobalNPC, F>> func) where F : Delegate
	{
		GlobalHookList<GlobalNPC> hook = GlobalHookList<GlobalNPC>.Create(func);
		hooks.Add(hook);
		return hook;
	}

	public static T AddModHook<T>(T hook) where T : GlobalHookList<GlobalNPC>
	{
		modHooks.Add(hook);
		return hook;
	}

	internal static int Register(ModNPC npc)
	{
		npcs.Add(npc);
		return NPCCount++;
	}

	/// <summary>
	/// Gets the ModNPC template instance corresponding to the specified type (not the clone/new instance which gets added to NPCs as the game is played).
	/// </summary>
	/// <param name="type">The type of the npc</param>
	/// <returns>The ModNPC instance in the <see cref="F:Terraria.ModLoader.NPCLoader.npcs" /> array, null if not found.</returns>
	public static ModNPC GetNPC(int type)
	{
		if (type < NPCID.Count || type >= NPCCount)
		{
			return null;
		}
		return npcs[type - NPCID.Count];
	}

	internal static void ResizeArrays(bool unloading)
	{
		if (!unloading)
		{
			GlobalList<GlobalNPC>.FinishLoading(NPCCount);
		}
		Array.Resize(ref TextureAssets.Npc, NPCCount);
		LoaderUtils.ResetStaticMembers(typeof(NPCID));
		Main.ShopHelper.ReinitializePersonalityDatabase();
		NPCHappiness.RegisterVanillaNpcRelationships();
		Array.Resize(ref Main.townNPCCanSpawn, NPCCount);
		Array.Resize(ref Main.slimeRainNPC, NPCCount);
		Array.Resize(ref Main.npcCatchable, NPCCount);
		Array.Resize(ref Main.npcFrameCount, NPCCount);
		Array.Resize(ref Main.SceneMetrics.NPCBannerBuff, NPCCount);
		Array.Resize(ref NPC.killCount, NPCCount);
		Array.Resize(ref NPC.ShimmeredTownNPCs, NPCCount);
		Array.Resize(ref NPC.npcsFoundForCheckActive, NPCCount);
		Array.Resize(ref Lang._npcNameCache, NPCCount);
		Array.Resize(ref EmoteBubble.CountNPCs, NPCCount);
		Array.Resize(ref WorldGen.TownManager._hasRoom, NPCCount);
		Player[] player = Main.player;
		for (int j = 0; j < player.Length; j++)
		{
			Array.Resize(ref player[j].npcTypeNoAggro, NPCCount);
		}
		for (int i = NPCID.Count; i < NPCCount; i++)
		{
			Main.npcFrameCount[i] = 1;
			Lang._npcNameCache[i] = LocalizedText.Empty;
		}
	}

	internal static void FinishSetup()
	{
		NPC temp = new NPC();
		GlobalLoaderUtils<GlobalNPC, NPC>.BuildTypeLookups(delegate(int type)
		{
			temp.SetDefaults(type);
		});
		UpdateHookLists();
		GlobalTypeLookups<GlobalNPC>.LogStats();
		foreach (ModNPC npc in npcs)
		{
			Lang._npcNameCache[npc.Type] = npc.DisplayName;
			RegisterTownNPCMoodLocalizations(npc);
		}
	}

	private static void UpdateHookLists()
	{
		foreach (GlobalHookList<GlobalNPC> item in hooks.Union(modHooks))
		{
			item.Update();
		}
	}

	internal static void RegisterTownNPCMoodLocalizations(ModNPC npc)
	{
		if (!npc.NPC.townNPC || NPCID.Sets.IsTownPet[npc.NPC.type] || NPCID.Sets.NoTownNPCHappiness[npc.NPC.type])
		{
			return;
		}
		string prefix = npc.GetLocalizationKey("TownNPCMood");
		List<string> keys = new List<string> { "Content", "NoHome", "FarFromHome", "LoveSpace", "DislikeCrowded", "HateCrowded" };
		if (Main.ShopHelper._database.TryGetProfileByNPCID(npc.NPC.type, out var personalityProfile))
		{
			List<IShopPersonalityTrait> shopModifiers = personalityProfile.ShopModifiers;
			BiomePreferenceListTrait biomePreferenceList = (BiomePreferenceListTrait)shopModifiers.SingleOrDefault((IShopPersonalityTrait t) => t is BiomePreferenceListTrait);
			if (biomePreferenceList != null)
			{
				if (biomePreferenceList.Preferences.Any((BiomePreferenceListTrait.BiomePreference x) => x.Affection == AffectionLevel.Love))
				{
					keys.Add("LoveBiome");
				}
				if (biomePreferenceList.Preferences.Any((BiomePreferenceListTrait.BiomePreference x) => x.Affection == AffectionLevel.Like))
				{
					keys.Add("LikeBiome");
				}
				if (biomePreferenceList.Preferences.Any((BiomePreferenceListTrait.BiomePreference x) => x.Affection == AffectionLevel.Dislike))
				{
					keys.Add("DislikeBiome");
				}
				if (biomePreferenceList.Preferences.Any((BiomePreferenceListTrait.BiomePreference x) => x.Affection == AffectionLevel.Hate))
				{
					keys.Add("HateBiome");
				}
			}
			if (shopModifiers.Any((IShopPersonalityTrait t) => t is NPCPreferenceTrait nPCPreferenceTrait4 && nPCPreferenceTrait4.Level == AffectionLevel.Love))
			{
				keys.Add("LoveNPC");
			}
			if (shopModifiers.Any((IShopPersonalityTrait t) => t is NPCPreferenceTrait nPCPreferenceTrait3 && nPCPreferenceTrait3.Level == AffectionLevel.Like))
			{
				keys.Add("LikeNPC");
			}
			if (shopModifiers.Any((IShopPersonalityTrait t) => t is NPCPreferenceTrait nPCPreferenceTrait2 && nPCPreferenceTrait2.Level == AffectionLevel.Dislike))
			{
				keys.Add("DislikeNPC");
			}
			if (shopModifiers.Any((IShopPersonalityTrait t) => t is NPCPreferenceTrait nPCPreferenceTrait && nPCPreferenceTrait.Level == AffectionLevel.Hate))
			{
				keys.Add("HateNPC");
			}
		}
		keys.Add("LikeNPC_Princess");
		keys.Add("Princess_LovesNPC");
		foreach (string key in keys)
		{
			string oldKey = npc.Mod.GetLocalizationKey("TownNPCMood." + npc.Name + "." + key);
			if (key == "Princess_LovesNPC")
			{
				oldKey = "TownNPCMood_Princess.LoveNPC_" + npc.FullName;
			}
			string key2 = prefix + "." + key;
			string defaultValueKey = "TownNPCMood." + key;
			Language.GetOrRegister(key2, () => (!Language.Exists(oldKey)) ? Language.GetTextValue(defaultValueKey) : ("{$" + oldKey + "}"));
		}
	}

	internal static void Unload()
	{
		NPCCount = NPCID.Count;
		npcs.Clear();
		GlobalList<GlobalNPC>.Reset();
		bannerToItem.Clear();
		modHooks.Clear();
		UpdateHookLists();
		if (!Main.dedServ)
		{
			TownNPCProfiles.Instance.ResetTexturesAccordingToVanillaProfiles();
		}
	}

	internal static bool IsModNPC(NPC npc)
	{
		return npc.type >= NPCID.Count;
	}

	internal static void SetDefaults(NPC npc, bool createModNPC = true)
	{
		if (IsModNPC(npc))
		{
			if (createModNPC)
			{
				npc.ModNPC = GetNPC(npc.type).NewInstance(npc);
			}
			else
			{
				Array.Resize(ref npc.buffImmune, BuffLoader.BuffCount);
			}
		}
		GlobalLoaderUtils<GlobalNPC, NPC>.SetDefaults(npc, ref npc._globals, delegate(NPC n)
		{
			n.ModNPC?.SetDefaults();
		});
	}

	internal static void OnSpawn(NPC npc, IEntitySource source)
	{
		npc.ModNPC?.OnSpawn(source);
		EntityGlobalsEnumerator<GlobalNPC> enumerator = HookOnSpawn.Enumerate(npc).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.OnSpawn(npc, source);
		}
	}

	public static void ApplyDifficultyAndPlayerScaling(NPC npc, int numPlayers, float balance, float bossAdjustment)
	{
		npc.ModNPC?.ApplyDifficultyAndPlayerScaling(numPlayers, balance, bossAdjustment);
		EntityGlobalsEnumerator<GlobalNPC> enumerator = HookApplyDifficultyAndPlayerScaling.Enumerate(npc).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.ApplyDifficultyAndPlayerScaling(npc, numPlayers, balance, bossAdjustment);
		}
	}

	public static void SetBestiary(NPC npc, BestiaryDatabase database, BestiaryEntry bestiaryEntry)
	{
		if (IsModNPC(npc))
		{
			bestiaryEntry.Info.Add(npc.ModNPC.Mod.ModSourceBestiaryInfoElement);
			int[] spawnModBiomes = npc.ModNPC.SpawnModBiomes;
			foreach (int type in spawnModBiomes)
			{
				bestiaryEntry.Info.Add(LoaderManager.Get<BiomeLoader>().Get(type).ModBiomeBestiaryInfoElement);
			}
		}
		npc.ModNPC?.SetBestiary(database, bestiaryEntry);
		EntityGlobalsEnumerator<GlobalNPC> enumerator = HookSetBestiary.Enumerate(npc).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.SetBestiary(npc, database, bestiaryEntry);
		}
	}

	public static void ModifyTownNPCProfile(NPC npc, ref ITownNPCProfile profile)
	{
		profile = npc.ModNPC?.TownNPCProfile() ?? profile;
		EntityGlobalsEnumerator<GlobalNPC> enumerator = HookModifyTownNPCProfile.Enumerate(npc).GetEnumerator();
		while (enumerator.MoveNext())
		{
			GlobalNPC g = enumerator.Current;
			profile = g.ModifyTownNPCProfile(npc) ?? profile;
		}
	}

	public static void ResetEffects(NPC npc)
	{
		npc.ModNPC?.ResetEffects();
		EntityGlobalsEnumerator<GlobalNPC> enumerator = HookResetEffects.Enumerate(npc).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.ResetEffects(npc);
		}
	}

	public static void NPCAI(NPC npc)
	{
		if (PreAI(npc))
		{
			int type = npc.type;
			int num;
			if (npc.ModNPC != null)
			{
				num = ((npc.ModNPC.AIType > 0) ? 1 : 0);
				if (num != 0)
				{
					npc.type = npc.ModNPC.AIType;
				}
			}
			else
			{
				num = 0;
			}
			npc.VanillaAI();
			if (num != 0)
			{
				npc.type = type;
			}
			AI(npc);
		}
		PostAI(npc);
	}

	public static bool PreAI(NPC npc)
	{
		bool result = true;
		EntityGlobalsEnumerator<GlobalNPC> enumerator = HookPreAI.Enumerate(npc).GetEnumerator();
		while (enumerator.MoveNext())
		{
			GlobalNPC g = enumerator.Current;
			result &= g.PreAI(npc);
		}
		if (result && npc.ModNPC != null)
		{
			return npc.ModNPC.PreAI();
		}
		return result;
	}

	public static void AI(NPC npc)
	{
		npc.ModNPC?.AI();
		EntityGlobalsEnumerator<GlobalNPC> enumerator = HookAI.Enumerate(npc).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.AI(npc);
		}
	}

	public static void PostAI(NPC npc)
	{
		npc.ModNPC?.PostAI();
		EntityGlobalsEnumerator<GlobalNPC> enumerator = HookPostAI.Enumerate(npc).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.PostAI(npc);
		}
	}

	public static void SendExtraAI(BinaryWriter writer, byte[] extraAI)
	{
		writer.Write7BitEncodedInt(extraAI.Length);
		if (extraAI.Length != 0)
		{
			writer.Write(extraAI);
		}
	}

	public static byte[] WriteExtraAI(NPC npc)
	{
		using MemoryStream stream = new MemoryStream();
		using BinaryWriter modWriter = new BinaryWriter(stream);
		npc.ModNPC?.SendExtraAI(modWriter);
		using MemoryStream bufferedStream = new MemoryStream();
		using BinaryWriter globalWriter = new BinaryWriter(bufferedStream);
		BitWriter bitWriter = new BitWriter();
		EntityGlobalsEnumerator<GlobalNPC> enumerator = HookSendExtraAI.Enumerate(npc).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.SendExtraAI(npc, bitWriter, globalWriter);
		}
		bitWriter.Flush(modWriter);
		modWriter.Write(bufferedStream.ToArray());
		return stream.ToArray();
	}

	public static byte[] ReadExtraAI(BinaryReader reader)
	{
		return reader.ReadBytes(reader.Read7BitEncodedInt());
	}

	public static void ReceiveExtraAI(NPC npc, byte[] extraAI)
	{
		using MemoryStream stream = extraAI.ToMemoryStream();
		using BinaryReader modReader = new BinaryReader(stream);
		npc.ModNPC?.ReceiveExtraAI(modReader);
		BitReader bitReader = new BitReader(modReader);
		bool anyGlobals = false;
		EntityGlobalsEnumerator<GlobalNPC> entityGlobalsEnumerator;
		try
		{
			entityGlobalsEnumerator = HookReceiveExtraAI.Enumerate(npc);
			EntityGlobalsEnumerator<GlobalNPC> enumerator = entityGlobalsEnumerator.GetEnumerator();
			while (enumerator.MoveNext())
			{
				GlobalNPC current = enumerator.Current;
				anyGlobals = true;
				current.ReceiveExtraAI(npc, bitReader, modReader);
			}
			if (bitReader.BitsRead < bitReader.MaxBits)
			{
				throw new IOException($"Read underflow {bitReader.MaxBits - bitReader.BitsRead} of {bitReader.MaxBits} compressed bits in ReceiveExtraAI, more info below");
			}
			if (stream.Position < stream.Length)
			{
				throw new IOException($"Read underflow {stream.Length - stream.Position} of {stream.Length} bytes in ReceiveExtraAI, more info below");
			}
		}
		catch (Exception e)
		{
			string message = "Error in ReceiveExtraAI for NPC " + (npc.ModNPC?.FullName ?? npc.TypeName);
			if (anyGlobals)
			{
				message += ", may be caused by one of these GlobalNPCs:";
				entityGlobalsEnumerator = HookReceiveExtraAI.Enumerate(npc);
				EntityGlobalsEnumerator<GlobalNPC> enumerator = entityGlobalsEnumerator.GetEnumerator();
				while (enumerator.MoveNext())
				{
					GlobalNPC g = enumerator.Current;
					message = message + "\n\t" + g.FullName;
				}
			}
			Logging.tML.Error((object)message, e);
		}
	}

	public static void FindFrame(NPC npc, int frameHeight)
	{
		bool isLikeATownNPC = npc.isLikeATownNPC;
		int? num = npc.ModNPC?.AnimationType;
		npc.VanillaFindFrame(frameHeight, isLikeATownNPC, (num.HasValue && num.GetValueOrDefault() > 0) ? npc.ModNPC.AnimationType : npc.type);
		npc.ModNPC?.FindFrame(frameHeight);
		EntityGlobalsEnumerator<GlobalNPC> enumerator = HookFindFrame.Enumerate(npc).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.FindFrame(npc, frameHeight);
		}
	}

	public static void HitEffect(NPC npc, in NPC.HitInfo hit)
	{
		npc.ModNPC?.HitEffect(hit);
		EntityGlobalsEnumerator<GlobalNPC> enumerator = HookHitEffect.Enumerate(npc).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.HitEffect(npc, hit);
		}
	}

	public static void UpdateLifeRegen(NPC npc, ref int damage)
	{
		npc.ModNPC?.UpdateLifeRegen(ref damage);
		EntityGlobalsEnumerator<GlobalNPC> enumerator = HookUpdateLifeRegen.Enumerate(npc).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.UpdateLifeRegen(npc, ref damage);
		}
	}

	public static bool CheckActive(NPC npc)
	{
		if (npc.ModNPC != null && !npc.ModNPC.CheckActive())
		{
			return false;
		}
		EntityGlobalsEnumerator<GlobalNPC> enumerator = HookCheckActive.Enumerate(npc).GetEnumerator();
		while (enumerator.MoveNext())
		{
			if (!enumerator.Current.CheckActive(npc))
			{
				return false;
			}
		}
		return true;
	}

	public static bool CheckDead(NPC npc)
	{
		bool result = true;
		if (npc.ModNPC != null)
		{
			result = npc.ModNPC.CheckDead();
		}
		EntityGlobalsEnumerator<GlobalNPC> enumerator = HookCheckDead.Enumerate(npc).GetEnumerator();
		while (enumerator.MoveNext())
		{
			GlobalNPC g = enumerator.Current;
			result &= g.CheckDead(npc);
		}
		return result;
	}

	public static bool SpecialOnKill(NPC npc)
	{
		EntityGlobalsEnumerator<GlobalNPC> enumerator = HookSpecialOnKill.Enumerate(npc).GetEnumerator();
		while (enumerator.MoveNext())
		{
			if (enumerator.Current.SpecialOnKill(npc))
			{
				return true;
			}
		}
		if (npc.ModNPC != null)
		{
			return npc.ModNPC.SpecialOnKill();
		}
		return false;
	}

	public static bool PreKill(NPC npc)
	{
		bool result = true;
		EntityGlobalsEnumerator<GlobalNPC> enumerator = HookPreKill.Enumerate(npc).GetEnumerator();
		while (enumerator.MoveNext())
		{
			GlobalNPC g = enumerator.Current;
			result &= g.PreKill(npc);
		}
		if (result && npc.ModNPC != null)
		{
			result = npc.ModNPC.PreKill();
		}
		if (!result)
		{
			blockLoot.Clear();
			return false;
		}
		return true;
	}

	public static void OnKill(NPC npc)
	{
		npc.ModNPC?.OnKill();
		EntityGlobalsEnumerator<GlobalNPC> enumerator = HookOnKill.Enumerate(npc).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.OnKill(npc);
		}
		blockLoot.Clear();
	}

	public static void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
	{
		npc.ModNPC?.ModifyNPCLoot(npcLoot);
		EntityGlobalsEnumerator<GlobalNPC> enumerator = HookModifyNPCLoot.Enumerate(npc).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.ModifyNPCLoot(npc, npcLoot);
		}
	}

	public static void ModifyGlobalLoot(GlobalLoot globalLoot)
	{
		ReadOnlySpan<GlobalNPC> readOnlySpan = HookModifyGlobalLoot.Enumerate();
		for (int i = 0; i < readOnlySpan.Length; i++)
		{
			readOnlySpan[i].ModifyGlobalLoot(globalLoot);
		}
	}

	public static void BossLoot(NPC npc, ref string name, ref int potionType)
	{
		npc.ModNPC?.BossLoot(ref name, ref potionType);
	}

	public static bool? CanFallThroughPlatforms(NPC npc)
	{
		bool? ret = npc.ModNPC?.CanFallThroughPlatforms() ?? null;
		if (ret.HasValue)
		{
			if (!ret.Value)
			{
				return false;
			}
			ret = true;
		}
		EntityGlobalsEnumerator<GlobalNPC> enumerator = HookCanFallThroughPlatforms.Enumerate(npc).GetEnumerator();
		while (enumerator.MoveNext())
		{
			bool? globalRet = enumerator.Current.CanFallThroughPlatforms(npc);
			if (globalRet.HasValue)
			{
				if (!globalRet.Value)
				{
					return false;
				}
				ret = true;
			}
		}
		return ret;
	}

	public static bool? CanBeCaughtBy(NPC npc, Item item, Player player)
	{
		bool? canBeCaughtOverall = null;
		EntityGlobalsEnumerator<GlobalNPC> enumerator = HookCanBeCaughtBy.Enumerate(npc).GetEnumerator();
		while (enumerator.MoveNext())
		{
			bool? canBeCaughtFromGlobalNPC = enumerator.Current.CanBeCaughtBy(npc, item, player);
			if (canBeCaughtFromGlobalNPC.HasValue)
			{
				if (!canBeCaughtFromGlobalNPC.Value)
				{
					return false;
				}
				canBeCaughtOverall = true;
			}
		}
		if (npc.ModNPC != null)
		{
			bool? canBeCaughtAsModNPC = npc.ModNPC.CanBeCaughtBy(item, player);
			if (canBeCaughtAsModNPC.HasValue)
			{
				if (!canBeCaughtAsModNPC.Value)
				{
					return false;
				}
				canBeCaughtOverall = true;
			}
		}
		return canBeCaughtOverall;
	}

	public static void OnCaughtBy(NPC npc, Player player, Item item, bool failed)
	{
		npc.ModNPC?.OnCaughtBy(player, item, failed);
		EntityGlobalsEnumerator<GlobalNPC> enumerator = HookOnCaughtBy.Enumerate(npc).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.OnCaughtBy(npc, player, item, failed);
		}
	}

	public static int? PickEmote(NPC npc, Player closestPlayer, List<int> emoteList, WorldUIAnchor anchor)
	{
		int? result = null;
		if (npc.ModNPC != null)
		{
			result = npc.ModNPC.PickEmote(closestPlayer, emoteList, anchor);
		}
		EntityGlobalsEnumerator<GlobalNPC> enumerator = HookPickEmote.Enumerate(npc).GetEnumerator();
		while (enumerator.MoveNext())
		{
			int? emote = enumerator.Current.PickEmote(npc, closestPlayer, emoteList, anchor);
			if (emote.HasValue)
			{
				result = emote;
			}
		}
		return result;
	}

	public static bool CanHitPlayer(NPC npc, Player target, ref int cooldownSlot)
	{
		EntityGlobalsEnumerator<GlobalNPC> enumerator = HookCanHitPlayer.Enumerate(npc).GetEnumerator();
		while (enumerator.MoveNext())
		{
			if (!enumerator.Current.CanHitPlayer(npc, target, ref cooldownSlot))
			{
				return false;
			}
		}
		if (npc.ModNPC != null)
		{
			return npc.ModNPC.CanHitPlayer(target, ref cooldownSlot);
		}
		return true;
	}

	public static void ModifyHitPlayer(NPC npc, Player target, ref Player.HurtModifiers modifiers)
	{
		npc.ModNPC?.ModifyHitPlayer(target, ref modifiers);
		EntityGlobalsEnumerator<GlobalNPC> enumerator = HookModifyHitPlayer.Enumerate(npc).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.ModifyHitPlayer(npc, target, ref modifiers);
		}
	}

	public static void OnHitPlayer(NPC npc, Player target, Player.HurtInfo hurtInfo)
	{
		npc.ModNPC?.OnHitPlayer(target, hurtInfo);
		EntityGlobalsEnumerator<GlobalNPC> enumerator = HookOnHitPlayer.Enumerate(npc).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.OnHitPlayer(npc, target, hurtInfo);
		}
	}

	public static bool CanHitNPC(NPC npc, NPC target)
	{
		EntityGlobalsEnumerator<GlobalNPC> entityGlobalsEnumerator = HookCanHitNPC.Enumerate(npc);
		EntityGlobalsEnumerator<GlobalNPC> enumerator = entityGlobalsEnumerator.GetEnumerator();
		while (enumerator.MoveNext())
		{
			if (!enumerator.Current.CanHitNPC(npc, target))
			{
				return false;
			}
		}
		entityGlobalsEnumerator = HookCanBeHitByNPC.Enumerate(target);
		enumerator = entityGlobalsEnumerator.GetEnumerator();
		while (enumerator.MoveNext())
		{
			if (!enumerator.Current.CanBeHitByNPC(target, npc))
			{
				return false;
			}
		}
		if ((!(npc.ModNPC?.CanHitNPC(target))) ?? false)
		{
			return false;
		}
		return target.ModNPC?.CanBeHitByNPC(npc) ?? true;
	}

	public static void ModifyHitNPC(NPC npc, NPC target, ref NPC.HitModifiers modifiers)
	{
		npc.ModNPC?.ModifyHitNPC(target, ref modifiers);
		EntityGlobalsEnumerator<GlobalNPC> enumerator = HookModifyHitNPC.Enumerate(npc).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.ModifyHitNPC(npc, target, ref modifiers);
		}
	}

	public static void OnHitNPC(NPC npc, NPC target, in NPC.HitInfo hit)
	{
		npc.ModNPC?.OnHitNPC(target, hit);
		EntityGlobalsEnumerator<GlobalNPC> enumerator = HookOnHitNPC.Enumerate(npc).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.OnHitNPC(npc, target, hit);
		}
	}

	public static bool? CanBeHitByItem(NPC npc, Player player, Item item)
	{
		bool? flag = null;
		EntityGlobalsEnumerator<GlobalNPC> enumerator = HookCanBeHitByItem.Enumerate(npc).GetEnumerator();
		while (enumerator.MoveNext())
		{
			bool? canHit2 = enumerator.Current.CanBeHitByItem(npc, player, item);
			if (canHit2.HasValue)
			{
				if (!canHit2.Value)
				{
					return false;
				}
				flag = true;
			}
		}
		if (npc.ModNPC != null)
		{
			bool? canHit = npc.ModNPC.CanBeHitByItem(player, item);
			if (canHit.HasValue)
			{
				if (!canHit.Value)
				{
					return false;
				}
				flag = true;
			}
		}
		return flag;
	}

	public static bool? CanCollideWithPlayerMeleeAttack(NPC npc, Player player, Item item, Rectangle meleeAttackHitbox)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		bool? flag = null;
		EntityGlobalsEnumerator<GlobalNPC> enumerator = HookCanCollideWithPlayerMeleeAttack.Enumerate(npc).GetEnumerator();
		while (enumerator.MoveNext())
		{
			bool? canCollide = enumerator.Current.CanCollideWithPlayerMeleeAttack(npc, player, item, meleeAttackHitbox);
			if (canCollide.HasValue)
			{
				if (!canCollide.Value)
				{
					return false;
				}
				flag = true;
			}
		}
		if (npc.ModNPC != null)
		{
			bool? canHit = npc.ModNPC.CanCollideWithPlayerMeleeAttack(player, item, meleeAttackHitbox);
			if (canHit.HasValue)
			{
				if (!canHit.Value)
				{
					return false;
				}
				flag = true;
			}
		}
		return flag;
	}

	public static void ModifyHitByItem(NPC npc, Player player, Item item, ref NPC.HitModifiers modifiers)
	{
		npc.ModNPC?.ModifyHitByItem(player, item, ref modifiers);
		EntityGlobalsEnumerator<GlobalNPC> enumerator = HookModifyHitByItem.Enumerate(npc).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.ModifyHitByItem(npc, player, item, ref modifiers);
		}
	}

	public static void OnHitByItem(NPC npc, Player player, Item item, in NPC.HitInfo hit, int damageDone)
	{
		npc.ModNPC?.OnHitByItem(player, item, hit, damageDone);
		EntityGlobalsEnumerator<GlobalNPC> enumerator = HookOnHitByItem.Enumerate(npc).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.OnHitByItem(npc, player, item, hit, damageDone);
		}
	}

	public static bool? CanBeHitByProjectile(NPC npc, Projectile projectile)
	{
		bool? flag = null;
		EntityGlobalsEnumerator<GlobalNPC> enumerator = HookCanBeHitByProjectile.Enumerate(npc).GetEnumerator();
		while (enumerator.MoveNext())
		{
			bool? canHit2 = enumerator.Current.CanBeHitByProjectile(npc, projectile);
			if (canHit2.HasValue && !canHit2.Value)
			{
				return false;
			}
			if (canHit2.HasValue)
			{
				flag = canHit2.Value;
			}
		}
		if (npc.ModNPC != null)
		{
			bool? canHit = npc.ModNPC.CanBeHitByProjectile(projectile);
			if (canHit.HasValue && !canHit.Value)
			{
				return false;
			}
			if (canHit.HasValue)
			{
				flag = canHit.Value;
			}
		}
		return flag;
	}

	public static void ModifyHitByProjectile(NPC npc, Projectile projectile, ref NPC.HitModifiers modifiers)
	{
		npc.ModNPC?.ModifyHitByProjectile(projectile, ref modifiers);
		EntityGlobalsEnumerator<GlobalNPC> enumerator = HookModifyHitByProjectile.Enumerate(npc).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.ModifyHitByProjectile(npc, projectile, ref modifiers);
		}
	}

	public static void OnHitByProjectile(NPC npc, Projectile projectile, in NPC.HitInfo hit, int damageDone)
	{
		npc.ModNPC?.OnHitByProjectile(projectile, hit, damageDone);
		EntityGlobalsEnumerator<GlobalNPC> enumerator = HookOnHitByProjectile.Enumerate(npc).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.OnHitByProjectile(npc, projectile, hit, damageDone);
		}
	}

	public static void ModifyIncomingHit(NPC npc, ref NPC.HitModifiers modifiers)
	{
		npc.ModNPC?.ModifyIncomingHit(ref modifiers);
		EntityGlobalsEnumerator<GlobalNPC> enumerator = HookAddModifyIncomingHit.Enumerate(npc).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.ModifyIncomingHit(npc, ref modifiers);
		}
	}

	public static void BossHeadSlot(NPC npc, ref int index)
	{
		npc.ModNPC?.BossHeadSlot(ref index);
		EntityGlobalsEnumerator<GlobalNPC> enumerator = HookBossHeadSlot.Enumerate(npc).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.BossHeadSlot(npc, ref index);
		}
	}

	public static void BossHeadRotation(NPC npc, ref float rotation)
	{
		npc.ModNPC?.BossHeadRotation(ref rotation);
		EntityGlobalsEnumerator<GlobalNPC> enumerator = HookBossHeadRotation.Enumerate(npc).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.BossHeadRotation(npc, ref rotation);
		}
	}

	public static void BossHeadSpriteEffects(NPC npc, ref SpriteEffects spriteEffects)
	{
		npc.ModNPC?.BossHeadSpriteEffects(ref spriteEffects);
		EntityGlobalsEnumerator<GlobalNPC> enumerator = HookBossHeadSpriteEffects.Enumerate(npc).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.BossHeadSpriteEffects(npc, ref spriteEffects);
		}
	}

	public static Color? GetAlpha(NPC npc, Color lightColor)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		EntityGlobalsEnumerator<GlobalNPC> enumerator = HookGetAlpha.Enumerate(npc).GetEnumerator();
		while (enumerator.MoveNext())
		{
			Color? color = enumerator.Current.GetAlpha(npc, lightColor);
			if (color.HasValue)
			{
				return color.Value;
			}
		}
		return npc.ModNPC?.GetAlpha(lightColor);
	}

	public static void DrawEffects(NPC npc, ref Color drawColor)
	{
		npc.ModNPC?.DrawEffects(ref drawColor);
		EntityGlobalsEnumerator<GlobalNPC> enumerator = HookDrawEffects.Enumerate(npc).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.DrawEffects(npc, ref drawColor);
		}
	}

	public static bool PreDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		bool result = true;
		EntityGlobalsEnumerator<GlobalNPC> enumerator = HookPreDraw.Enumerate(npc).GetEnumerator();
		while (enumerator.MoveNext())
		{
			GlobalNPC g = enumerator.Current;
			result &= g.PreDraw(npc, spriteBatch, screenPos, drawColor);
		}
		if (result && npc.ModNPC != null)
		{
			return npc.ModNPC.PreDraw(spriteBatch, screenPos, drawColor);
		}
		return result;
	}

	public static void PostDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		npc.ModNPC?.PostDraw(spriteBatch, screenPos, drawColor);
		EntityGlobalsEnumerator<GlobalNPC> enumerator = HookPostDraw.Enumerate(npc).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.PostDraw(npc, spriteBatch, screenPos, drawColor);
		}
	}

	internal static void DrawBehind(NPC npc, int index)
	{
		npc.ModNPC?.DrawBehind(index);
		EntityGlobalsEnumerator<GlobalNPC> enumerator = HookDrawBehind.Enumerate(npc).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.DrawBehind(npc, index);
		}
	}

	public static bool DrawHealthBar(NPC npc, ref float scale)
	{
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		Vector2 position = default(Vector2);
		((Vector2)(ref position))._002Ector(npc.position.X + (float)(npc.width / 2), npc.position.Y + npc.gfxOffY);
		if (Main.HealthBarDrawSettings == 1)
		{
			position.Y += (float)npc.height + 10f + Main.NPCAddHeight(npc);
		}
		else if (Main.HealthBarDrawSettings == 2)
		{
			position.Y -= 24f + Main.NPCAddHeight(npc) / 2f;
		}
		EntityGlobalsEnumerator<GlobalNPC> enumerator = HookDrawHealthBar.Enumerate(npc).GetEnumerator();
		while (enumerator.MoveNext())
		{
			bool? result2 = enumerator.Current.DrawHealthBar(npc, Main.HealthBarDrawSettings, ref scale, ref position);
			if (result2.HasValue)
			{
				if (result2.Value)
				{
					DrawHealthBar(npc, position, scale);
				}
				return false;
			}
		}
		if (IsModNPC(npc))
		{
			bool? result = npc.ModNPC.DrawHealthBar(Main.HealthBarDrawSettings, ref scale, ref position);
			if (result.HasValue)
			{
				if (result.Value)
				{
					DrawHealthBar(npc, position, scale);
				}
				return false;
			}
		}
		return true;
	}

	private static void DrawHealthBar(NPC npc, Vector2 position, float scale)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		float alpha = Lighting.Brightness((int)(npc.Center.X / 16f), (int)(npc.Center.Y / 16f));
		Main.instance.DrawHealthBar(position.X, position.Y, npc.life, npc.lifeMax, alpha, scale);
	}

	public static void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns)
	{
		ReadOnlySpan<GlobalNPC> readOnlySpan = HookEditSpawnRate.Enumerate();
		for (int i = 0; i < readOnlySpan.Length; i++)
		{
			readOnlySpan[i].EditSpawnRate(player, ref spawnRate, ref maxSpawns);
		}
	}

	public static void EditSpawnRange(Player player, ref int spawnRangeX, ref int spawnRangeY, ref int safeRangeX, ref int safeRangeY)
	{
		ReadOnlySpan<GlobalNPC> readOnlySpan = HookEditSpawnRange.Enumerate();
		for (int i = 0; i < readOnlySpan.Length; i++)
		{
			readOnlySpan[i].EditSpawnRange(player, ref spawnRangeX, ref spawnRangeY, ref safeRangeX, ref safeRangeY);
		}
	}

	public static int? ChooseSpawn(NPCSpawnInfo spawnInfo)
	{
		NPCSpawnHelper.Reset();
		NPCSpawnHelper.DoChecks(spawnInfo);
		IDictionary<int, float> pool = new Dictionary<int, float>();
		pool[0] = 1f;
		foreach (ModNPC npc in npcs)
		{
			float weight2 = npc.SpawnChance(spawnInfo);
			if (weight2 > 0f)
			{
				pool[npc.NPC.type] = weight2;
			}
		}
		ReadOnlySpan<GlobalNPC> readOnlySpan = HookEditSpawnPool.Enumerate();
		for (int i = 0; i < readOnlySpan.Length; i++)
		{
			readOnlySpan[i].EditSpawnPool(pool, spawnInfo);
		}
		float totalWeight = 0f;
		foreach (int type2 in pool.Keys)
		{
			if (pool[type2] < 0f)
			{
				pool[type2] = 0f;
			}
			totalWeight += pool[type2];
		}
		float choice = (float)Main.rand.NextDouble() * totalWeight;
		foreach (int type in pool.Keys)
		{
			float weight = pool[type];
			if (choice < weight)
			{
				return type;
			}
			choice -= weight;
		}
		return null;
	}

	public static int SpawnNPC(int type, int tileX, int tileY)
	{
		int npc = ((type >= NPCID.Count) ? GetNPC(type).SpawnNPC(tileX, tileY) : NPC.NewNPC(NPC.GetSpawnSourceForNaturalSpawn(), tileX * 16 + 8, tileY * 16, type));
		EntityGlobalsEnumerator<GlobalNPC> enumerator = HookSpawnNPC.Enumerate(Main.npc[npc]).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.SpawnNPC(npc, tileX, tileY);
		}
		return npc;
	}

	public static void CanTownNPCSpawn(int numTownNPCs)
	{
		foreach (ModNPC modNPC in npcs)
		{
			NPC npc = modNPC.NPC;
			if (npc.townNPC && NPC.TypeToDefaultHeadIndex(npc.type) >= 0 && !NPC.AnyNPCs(npc.type) && modNPC.CanTownNPCSpawn(numTownNPCs))
			{
				Main.townNPCCanSpawn[npc.type] = true;
				if (WorldGen.prioritizedTownNPCType == 0)
				{
					WorldGen.prioritizedTownNPCType = npc.type;
				}
			}
		}
	}

	public static bool CheckConditions(int type)
	{
		return GetNPC(type)?.CheckConditions(WorldGen.roomX1, WorldGen.roomX2, WorldGen.roomY1, WorldGen.roomY2) ?? true;
	}

	public static string ModifyTypeName(NPC npc, string typeName)
	{
		if (npc.ModNPC != null)
		{
			npc.ModNPC.ModifyTypeName(ref typeName);
		}
		EntityGlobalsEnumerator<GlobalNPC> enumerator = HookModifyTypeName.Enumerate(npc).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.ModifyTypeName(npc, ref typeName);
		}
		return typeName;
	}

	public static void ModifyHoverBoundingBox(NPC npc, ref Rectangle boundingBox)
	{
		if (npc.ModNPC != null)
		{
			npc.ModNPC.ModifyHoverBoundingBox(ref boundingBox);
		}
		EntityGlobalsEnumerator<GlobalNPC> enumerator = HookModifyHoverBoundingBox.Enumerate(npc).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.ModifyHoverBoundingBox(npc, ref boundingBox);
		}
	}

	public static List<string> ModifyNPCNameList(NPC npc, List<string> nameList)
	{
		if (npc.ModNPC != null)
		{
			nameList = npc.ModNPC.SetNPCNameList();
		}
		EntityGlobalsEnumerator<GlobalNPC> enumerator = HookModifyNPCNameList.Enumerate(npc).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.ModifyNPCNameList(npc, nameList);
		}
		return nameList;
	}

	public static bool UsesPartyHat(NPC npc)
	{
		return npc.ModNPC?.UsesPartyHat() ?? true;
	}

	public static bool? CanChat(NPC npc)
	{
		bool? ret = npc.ModNPC?.CanChat();
		EntityGlobalsEnumerator<GlobalNPC> enumerator = HookCanChat.Enumerate(npc).GetEnumerator();
		while (enumerator.MoveNext())
		{
			bool? flag = enumerator.Current.CanChat(npc);
			if (flag.HasValue)
			{
				if (!flag.GetValueOrDefault())
				{
					return false;
				}
				ret = true;
			}
		}
		return ret;
	}

	public static void GetChat(NPC npc, ref string chat)
	{
		if (npc.ModNPC != null)
		{
			chat = npc.ModNPC.GetChat();
		}
		else if (chat.Equals(""))
		{
			chat = Language.GetTextValue("tModLoader.DefaultTownNPCChat");
		}
		EntityGlobalsEnumerator<GlobalNPC> enumerator = HookGetChat.Enumerate(npc).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.GetChat(npc, ref chat);
		}
	}

	public static void SetChatButtons(ref string button, ref string button2)
	{
		Main.LocalPlayer.TalkNPC?.ModNPC?.SetChatButtons(ref button, ref button2);
	}

	public static bool PreChatButtonClicked(bool firstButton)
	{
		NPC npc = Main.LocalPlayer.TalkNPC;
		bool result = true;
		EntityGlobalsEnumerator<GlobalNPC> enumerator = HookPreChatButtonClicked.Enumerate(npc).GetEnumerator();
		while (enumerator.MoveNext())
		{
			GlobalNPC g = enumerator.Current;
			result &= g.PreChatButtonClicked(npc, firstButton);
		}
		if (!result)
		{
			SoundEngine.PlaySound(in SoundID.MenuTick);
			return false;
		}
		return true;
	}

	public static void OnChatButtonClicked(bool firstButton)
	{
		NPC npc = Main.LocalPlayer.TalkNPC;
		string shopName = null;
		if (npc.ModNPC != null)
		{
			npc.ModNPC.OnChatButtonClicked(firstButton, ref shopName);
			SoundEngine.PlaySound(in SoundID.MenuTick);
			if (shopName != null)
			{
				Main.playerInventory = true;
				Main.stackSplit = 9999;
				Main.npcChatText = "";
				Main.SetNPCShopIndex(1);
				Main.instance.shop[Main.npcShop].SetupShop(NPCShopDatabase.GetShopName(npc.type, shopName), npc);
			}
		}
		EntityGlobalsEnumerator<GlobalNPC> enumerator = HookOnChatButtonClicked.Enumerate(npc).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.OnChatButtonClicked(npc, firstButton);
		}
	}

	public static void AddShops(int type)
	{
		GetNPC(type)?.AddShops();
	}

	public static void ModifyShop(NPCShop shop)
	{
		ReadOnlySpan<GlobalNPC> readOnlySpan = HookModifyShop.Enumerate(shop.NpcType);
		for (int i = 0; i < readOnlySpan.Length; i++)
		{
			readOnlySpan[i].ModifyShop(shop);
		}
	}

	public static void ModifyActiveShop(NPC npc, string shopName, Item[] shopContents)
	{
		GetNPC(npc.type)?.ModifyActiveShop(shopName, shopContents);
		EntityGlobalsEnumerator<GlobalNPC> enumerator = HookModifyActiveShop.Enumerate(npc).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.ModifyActiveShop(npc, shopName, shopContents);
		}
	}

	public static void SetupTravelShop(int[] shop, ref int nextSlot)
	{
		ReadOnlySpan<GlobalNPC> readOnlySpan = HookSetupTravelShop.Enumerate();
		for (int i = 0; i < readOnlySpan.Length; i++)
		{
			readOnlySpan[i].SetupTravelShop(shop, ref nextSlot);
		}
	}

	public static bool? CanGoToStatue(NPC npc, bool toKingStatue)
	{
		bool? ret = npc.ModNPC?.CanGoToStatue(toKingStatue);
		EntityGlobalsEnumerator<GlobalNPC> enumerator = HookCanGoToStatue.Enumerate(npc).GetEnumerator();
		while (enumerator.MoveNext())
		{
			bool? flag = enumerator.Current.CanGoToStatue(npc, toKingStatue);
			if (flag.HasValue)
			{
				if (!flag.GetValueOrDefault())
				{
					return false;
				}
				ret = true;
			}
		}
		return ret;
	}

	public static void OnGoToStatue(NPC npc, bool toKingStatue)
	{
		npc.ModNPC?.OnGoToStatue(toKingStatue);
		EntityGlobalsEnumerator<GlobalNPC> enumerator = HookOnGoToStatue.Enumerate(npc).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.OnGoToStatue(npc, toKingStatue);
		}
	}

	public static void BuffTownNPC(ref float damageMult, ref int defense)
	{
		ReadOnlySpan<GlobalNPC> readOnlySpan = HookBuffTownNPC.Enumerate();
		for (int i = 0; i < readOnlySpan.Length; i++)
		{
			readOnlySpan[i].BuffTownNPC(ref damageMult, ref defense);
		}
	}

	public static void TownNPCAttackStrength(NPC npc, ref int damage, ref float knockback)
	{
		npc.ModNPC?.TownNPCAttackStrength(ref damage, ref knockback);
		EntityGlobalsEnumerator<GlobalNPC> enumerator = HookTownNPCAttackStrength.Enumerate(npc).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.TownNPCAttackStrength(npc, ref damage, ref knockback);
		}
	}

	public static void TownNPCAttackCooldown(NPC npc, ref int cooldown, ref int randExtraCooldown)
	{
		npc.ModNPC?.TownNPCAttackCooldown(ref cooldown, ref randExtraCooldown);
		EntityGlobalsEnumerator<GlobalNPC> enumerator = HookTownNPCAttackCooldown.Enumerate(npc).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.TownNPCAttackCooldown(npc, ref cooldown, ref randExtraCooldown);
		}
	}

	public static void TownNPCAttackProj(NPC npc, ref int projType, ref int attackDelay)
	{
		npc.ModNPC?.TownNPCAttackProj(ref projType, ref attackDelay);
		EntityGlobalsEnumerator<GlobalNPC> enumerator = HookTownNPCAttackProj.Enumerate(npc).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.TownNPCAttackProj(npc, ref projType, ref attackDelay);
		}
	}

	public static void TownNPCAttackProjSpeed(NPC npc, ref float multiplier, ref float gravityCorrection, ref float randomOffset)
	{
		npc.ModNPC?.TownNPCAttackProjSpeed(ref multiplier, ref gravityCorrection, ref randomOffset);
		EntityGlobalsEnumerator<GlobalNPC> enumerator = HookTownNPCAttackProjSpeed.Enumerate(npc).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.TownNPCAttackProjSpeed(npc, ref multiplier, ref gravityCorrection, ref randomOffset);
		}
	}

	public static void TownNPCAttackShoot(NPC npc, ref bool inBetweenShots)
	{
		npc.ModNPC?.TownNPCAttackShoot(ref inBetweenShots);
		EntityGlobalsEnumerator<GlobalNPC> enumerator = HookTownNPCAttackShoot.Enumerate(npc).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.TownNPCAttackShoot(npc, ref inBetweenShots);
		}
	}

	public static void TownNPCAttackMagic(NPC npc, ref float auraLightMultiplier)
	{
		npc.ModNPC?.TownNPCAttackMagic(ref auraLightMultiplier);
		EntityGlobalsEnumerator<GlobalNPC> enumerator = HookTownNPCAttackMagic.Enumerate(npc).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.TownNPCAttackMagic(npc, ref auraLightMultiplier);
		}
	}

	public static void TownNPCAttackSwing(NPC npc, ref int itemWidth, ref int itemHeight)
	{
		npc.ModNPC?.TownNPCAttackSwing(ref itemWidth, ref itemHeight);
		EntityGlobalsEnumerator<GlobalNPC> enumerator = HookTownNPCAttackSwing.Enumerate(npc).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.TownNPCAttackSwing(npc, ref itemWidth, ref itemHeight);
		}
	}

	public static void DrawTownAttackGun(NPC npc, ref Texture2D item, ref Rectangle itemFrame, ref float scale, ref int horizontalHoldoutOffset)
	{
		npc.ModNPC?.DrawTownAttackGun(ref item, ref itemFrame, ref scale, ref horizontalHoldoutOffset);
		EntityGlobalsEnumerator<GlobalNPC> enumerator = HookDrawTownAttackGun.Enumerate(npc).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.DrawTownAttackGun(npc, ref item, ref itemFrame, ref scale, ref horizontalHoldoutOffset);
		}
	}

	public static void DrawTownAttackSwing(NPC npc, ref Texture2D item, ref Rectangle itemFrame, ref int itemSize, ref float scale, ref Vector2 offset)
	{
		npc.ModNPC?.DrawTownAttackSwing(ref item, ref itemFrame, ref itemSize, ref scale, ref offset);
		EntityGlobalsEnumerator<GlobalNPC> enumerator = HookDrawTownAttackSwing.Enumerate(npc).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.DrawTownAttackSwing(npc, ref item, ref itemFrame, ref itemSize, ref scale, ref offset);
		}
	}

	public static bool ModifyCollisionData(NPC npc, Rectangle victimHitbox, ref int immunityCooldownSlot, ref float damageMultiplier, ref Rectangle npcHitbox)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		MultipliableFloat damageMult = MultipliableFloat.One;
		bool result = true;
		EntityGlobalsEnumerator<GlobalNPC> enumerator = HookModifyCollisionData.Enumerate(npc).GetEnumerator();
		while (enumerator.MoveNext())
		{
			GlobalNPC g = enumerator.Current;
			result &= g.ModifyCollisionData(npc, victimHitbox, ref immunityCooldownSlot, ref damageMult, ref npcHitbox);
		}
		if (result && npc.ModNPC != null)
		{
			result = npc.ModNPC.ModifyCollisionData(victimHitbox, ref immunityCooldownSlot, ref damageMult, ref npcHitbox);
		}
		damageMultiplier *= damageMult.Value;
		return result;
	}

	public static bool SavesAndLoads(NPC npc)
	{
		if (npc.townNPC && npc.type != 368)
		{
			return true;
		}
		if (!NPCID.Sets.SavesAndLoads[npc.type])
		{
			ModNPC modNPC = npc.ModNPC;
			if (modNPC == null || !modNPC.NeedSaving())
			{
				EntityGlobalsEnumerator<GlobalNPC> enumerator = HookNeedSaving.Enumerate(npc).GetEnumerator();
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.NeedSaving(npc))
					{
						return true;
					}
				}
				return false;
			}
		}
		return true;
	}
}
