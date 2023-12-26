using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ModLoader.Core;
using Terraria.ModLoader.Default;

namespace Terraria.ModLoader;

/// <summary>
/// This is where all ModPlayer hooks are gathered and called.
/// </summary>
public static class PlayerLoader
{
	private delegate void DelegateModifyMaxStats(out StatModifier health, out StatModifier mana);

	private delegate void DelegateNaturalLifeRegen(ref float regen);

	private delegate void DelegateUpdateEquips();

	private delegate void DelegateModifyExtraJumpDuration(ExtraJump jump, ref float duration);

	private delegate void DelegateOnExtraJumpStarted(ExtraJump jump, ref bool playSound);

	private delegate void DelegateModifyHurt(ref Player.HurtModifiers modifiers);

	private delegate bool DelegatePreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource);

	private delegate bool DelegatePreModifyLuck(ref float luck);

	private delegate void DelegateModifyLuck(ref float luck);

	private delegate void DelegateGetHealLife(Item item, bool quickHeal, ref int healValue);

	private delegate void DelegateGetHealMana(Item item, bool quickHeal, ref int healValue);

	private delegate void DelegateModifyManaCost(Item item, ref float reduce, ref float mult);

	private delegate void DelegateModifyWeaponDamage(Item item, ref StatModifier damage);

	private delegate void DelegateModifyWeaponKnockback(Item item, ref StatModifier knockback);

	private delegate void DelegateModifyWeaponCrit(Item item, ref float crit);

	private delegate void DelegateModifyShootStats(Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback);

	private delegate void DelegateModifyItemScale(Item item, ref float scale);

	private delegate void DelegateModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers);

	private delegate void DelegateModifyHitNPCWithItem(Item item, NPC target, ref NPC.HitModifiers modifiers);

	private delegate void DelegateModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers);

	private delegate bool DelegateCanBeHitByNPC(NPC npc, ref int cooldownSlot);

	private delegate void DelegateModifyHitByNPC(NPC npc, ref Player.HurtModifiers modifiers);

	private delegate void DelegateModifyHitByProjectile(Projectile proj, ref Player.HurtModifiers modifiers);

	private delegate void DelegateModifyFishingAttempt(ref FishingAttempt attempt);

	private delegate void DelegateCatchFish(FishingAttempt attempt, ref int itemDrop, ref int enemySpawn, ref AdvancedPopupRequest sonar, ref Vector2 sonarPosition);

	private delegate void DelegateModifyCaughtFish(Item fish);

	private delegate bool? DelegateCanConsumeBait(Item bait);

	private delegate void DelegateGetFishingLevel(Item fishingRod, Item bait, ref float fishingLevel);

	private delegate void DelegateDrawEffects(PlayerDrawSet drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright);

	private delegate void DelegateModifyDrawInfo(ref PlayerDrawSet drawInfo);

	private delegate void DelegateModifyZoom(ref float zoom);

	private delegate bool DelegateModifyNurseHeal(NPC npc, ref int health, ref bool removeDebuffs, ref string chatText);

	private delegate void DelegateModifyNursePrice(NPC npc, int health, bool removeDebuffs, ref int price);

	private delegate IEnumerable<Item> DelegateFindMaterialsFrom(out ModPlayer.ItemConsumedCallback onUsedForCrafting);

	private static readonly List<ModPlayer> players = new List<ModPlayer>();

	private static readonly List<HookList<ModPlayer>> hooks = new List<HookList<ModPlayer>>();

	private static readonly List<HookList<ModPlayer>> modHooks = new List<HookList<ModPlayer>>();

	private static HookList<ModPlayer> HookInitialize = AddHook((Expression<Func<ModPlayer, Action>>)((ModPlayer p) => p.Initialize));

	private static HookList<ModPlayer> HookResetEffects = AddHook((Expression<Func<ModPlayer, Action>>)((ModPlayer p) => p.ResetEffects));

	private static HookList<ModPlayer> HookResetInfoAccessories = AddHook((Expression<Func<ModPlayer, Action>>)((ModPlayer p) => p.ResetInfoAccessories));

	private static HookList<ModPlayer> HookRefreshInfoAccessoriesFromTeamPlayers = AddHook((Expression<Func<ModPlayer, Action<Player>>>)((ModPlayer p) => p.RefreshInfoAccessoriesFromTeamPlayers));

	private static HookList<ModPlayer> HookModifyMaxStats = AddHook((Expression<Func<ModPlayer, DelegateModifyMaxStats>>)((ModPlayer p) => p.ModifyMaxStats));

	private static HookList<ModPlayer> HookUpdateDead = AddHook((Expression<Func<ModPlayer, Action>>)((ModPlayer p) => p.UpdateDead));

	private static HookList<ModPlayer> HookPreSavePlayer = AddHook((Expression<Func<ModPlayer, Action>>)((ModPlayer p) => p.PreSavePlayer));

	private static HookList<ModPlayer> HookPostSavePlayer = AddHook((Expression<Func<ModPlayer, Action>>)((ModPlayer p) => p.PostSavePlayer));

	private static HookList<ModPlayer> HookCopyClientState = AddHook((Expression<Func<ModPlayer, Action<ModPlayer>>>)((ModPlayer p) => p.CopyClientState));

	private static HookList<ModPlayer> HookSyncPlayer = AddHook((Expression<Func<ModPlayer, Action<int, int, bool>>>)((ModPlayer p) => p.SyncPlayer));

	private static HookList<ModPlayer> HookSendClientChanges = AddHook((Expression<Func<ModPlayer, Action<ModPlayer>>>)((ModPlayer p) => p.SendClientChanges));

	private static HookList<ModPlayer> HookUpdateBadLifeRegen = AddHook((Expression<Func<ModPlayer, Action>>)((ModPlayer p) => p.UpdateBadLifeRegen));

	private static HookList<ModPlayer> HookUpdateLifeRegen = AddHook((Expression<Func<ModPlayer, Action>>)((ModPlayer p) => p.UpdateLifeRegen));

	private static HookList<ModPlayer> HookNaturalLifeRegen = AddHook((Expression<Func<ModPlayer, DelegateNaturalLifeRegen>>)((ModPlayer p) => p.NaturalLifeRegen));

	private static HookList<ModPlayer> HookUpdateAutopause = AddHook((Expression<Func<ModPlayer, Action>>)((ModPlayer p) => p.UpdateAutopause));

	private static HookList<ModPlayer> HookPreUpdate = AddHook((Expression<Func<ModPlayer, Action>>)((ModPlayer p) => p.PreUpdate));

	private static HookList<ModPlayer> HookSetControls = AddHook((Expression<Func<ModPlayer, Action>>)((ModPlayer p) => p.SetControls));

	private static HookList<ModPlayer> HookPreUpdateBuffs = AddHook((Expression<Func<ModPlayer, Action>>)((ModPlayer p) => p.PreUpdateBuffs));

	private static HookList<ModPlayer> HookPostUpdateBuffs = AddHook((Expression<Func<ModPlayer, Action>>)((ModPlayer p) => p.PostUpdateBuffs));

	private static HookList<ModPlayer> HookUpdateEquips = AddHook((Expression<Func<ModPlayer, DelegateUpdateEquips>>)((ModPlayer p) => p.UpdateEquips));

	private static HookList<ModPlayer> HookPostUpdateEquips = AddHook((Expression<Func<ModPlayer, Action>>)((ModPlayer p) => p.PostUpdateEquips));

	private static HookList<ModPlayer> HookUpdateVisibleAccessories = AddHook((Expression<Func<ModPlayer, Action>>)((ModPlayer p) => p.UpdateVisibleAccessories));

	private static HookList<ModPlayer> HookUpdateVisibleVanityAccessories = AddHook((Expression<Func<ModPlayer, Action>>)((ModPlayer p) => p.UpdateVisibleVanityAccessories));

	private static HookList<ModPlayer> HookUpdateDyes = AddHook((Expression<Func<ModPlayer, Action>>)((ModPlayer p) => p.UpdateDyes));

	private static HookList<ModPlayer> HookPostUpdateMiscEffects = AddHook((Expression<Func<ModPlayer, Action>>)((ModPlayer p) => p.PostUpdateMiscEffects));

	private static HookList<ModPlayer> HookPostUpdateRunSpeeds = AddHook((Expression<Func<ModPlayer, Action>>)((ModPlayer p) => p.PostUpdateRunSpeeds));

	private static HookList<ModPlayer> HookPreUpdateMovement = AddHook((Expression<Func<ModPlayer, Action>>)((ModPlayer p) => p.PreUpdateMovement));

	private static HookList<ModPlayer> HookPostUpdate = AddHook((Expression<Func<ModPlayer, Action>>)((ModPlayer p) => p.PostUpdate));

	private static HookList<ModPlayer> HookModifyExtraJumpDurationMultiplier = AddHook((Expression<Func<ModPlayer, DelegateModifyExtraJumpDuration>>)((ModPlayer p) => p.ModifyExtraJumpDurationMultiplier));

	private static HookList<ModPlayer> HookCanStartExtraJump = AddHook((Expression<Func<ModPlayer, Func<ExtraJump, bool>>>)((ModPlayer p) => p.CanStartExtraJump));

	private static HookList<ModPlayer> HookOnExtraJumpStarted = AddHook((Expression<Func<ModPlayer, DelegateOnExtraJumpStarted>>)((ModPlayer p) => p.OnExtraJumpStarted));

	private static HookList<ModPlayer> HookOnExtraJumpEnded = AddHook((Expression<Func<ModPlayer, Action<ExtraJump>>>)((ModPlayer p) => p.OnExtraJumpEnded));

	private static HookList<ModPlayer> HookOnExtraJumpRefreshed = AddHook((Expression<Func<ModPlayer, Action<ExtraJump>>>)((ModPlayer p) => p.OnExtraJumpRefreshed));

	private static HookList<ModPlayer> HookExtraJumpVisuals = AddHook((Expression<Func<ModPlayer, Action<ExtraJump>>>)((ModPlayer p) => p.ExtraJumpVisuals));

	private static HookList<ModPlayer> HookCanShowExtraJumpVisuals = AddHook((Expression<Func<ModPlayer, Func<ExtraJump, bool>>>)((ModPlayer p) => p.CanShowExtraJumpVisuals));

	private static HookList<ModPlayer> HookFrameEffects = AddHook((Expression<Func<ModPlayer, Action>>)((ModPlayer p) => p.FrameEffects));

	private static HookList<ModPlayer> HookImmuneTo = AddHook((Expression<Func<ModPlayer, Func<PlayerDeathReason, int, bool, bool>>>)((ModPlayer p) => p.ImmuneTo));

	private static HookList<ModPlayer> HookFreeDodge = AddHook((Expression<Func<ModPlayer, Func<Player.HurtInfo, bool>>>)((ModPlayer p) => p.FreeDodge));

	private static HookList<ModPlayer> HookConsumableDodge = AddHook((Expression<Func<ModPlayer, Func<Player.HurtInfo, bool>>>)((ModPlayer p) => p.ConsumableDodge));

	private static HookList<ModPlayer> HookModifyHurt = AddHook((Expression<Func<ModPlayer, DelegateModifyHurt>>)((ModPlayer p) => p.ModifyHurt));

	private static HookList<ModPlayer> HookHurt = AddHook((Expression<Func<ModPlayer, Action<Player.HurtInfo>>>)((ModPlayer p) => p.OnHurt));

	private static HookList<ModPlayer> HookPostHurt = AddHook((Expression<Func<ModPlayer, Action<Player.HurtInfo>>>)((ModPlayer p) => p.PostHurt));

	private static HookList<ModPlayer> HookPreKill = AddHook((Expression<Func<ModPlayer, DelegatePreKill>>)((ModPlayer p) => p.PreKill));

	private static HookList<ModPlayer> HookKill = AddHook((Expression<Func<ModPlayer, Action<double, int, bool, PlayerDeathReason>>>)((ModPlayer p) => p.Kill));

	private static HookList<ModPlayer> HookPreModifyLuck = AddHook((Expression<Func<ModPlayer, DelegatePreModifyLuck>>)((ModPlayer p) => p.PreModifyLuck));

	private static HookList<ModPlayer> HookModifyLuck = AddHook((Expression<Func<ModPlayer, DelegateModifyLuck>>)((ModPlayer p) => p.ModifyLuck));

	private static HookList<ModPlayer> HookPreItemCheck = AddHook((Expression<Func<ModPlayer, Func<bool>>>)((ModPlayer p) => p.PreItemCheck));

	private static HookList<ModPlayer> HookPostItemCheck = AddHook((Expression<Func<ModPlayer, Action>>)((ModPlayer p) => p.PostItemCheck));

	private static HookList<ModPlayer> HookUseTimeMultiplier = AddHook((Expression<Func<ModPlayer, Func<Item, float>>>)((ModPlayer p) => p.UseTimeMultiplier));

	private static HookList<ModPlayer> HookUseAnimationMultiplier = AddHook((Expression<Func<ModPlayer, Func<Item, float>>>)((ModPlayer p) => p.UseAnimationMultiplier));

	private static HookList<ModPlayer> HookUseSpeedMultiplier = AddHook((Expression<Func<ModPlayer, Func<Item, float>>>)((ModPlayer p) => p.UseSpeedMultiplier));

	private static HookList<ModPlayer> HookGetHealLife = AddHook((Expression<Func<ModPlayer, DelegateGetHealLife>>)((ModPlayer p) => p.GetHealLife));

	private static HookList<ModPlayer> HookGetHealMana = AddHook((Expression<Func<ModPlayer, DelegateGetHealMana>>)((ModPlayer p) => p.GetHealMana));

	private static HookList<ModPlayer> HookModifyManaCost = AddHook((Expression<Func<ModPlayer, DelegateModifyManaCost>>)((ModPlayer p) => p.ModifyManaCost));

	private static HookList<ModPlayer> HookOnMissingMana = AddHook((Expression<Func<ModPlayer, Action<Item, int>>>)((ModPlayer p) => p.OnMissingMana));

	private static HookList<ModPlayer> HookOnConsumeMana = AddHook((Expression<Func<ModPlayer, Action<Item, int>>>)((ModPlayer p) => p.OnConsumeMana));

	private static HookList<ModPlayer> HookModifyWeaponDamage = AddHook((Expression<Func<ModPlayer, DelegateModifyWeaponDamage>>)((ModPlayer p) => p.ModifyWeaponDamage));

	private static HookList<ModPlayer> HookProcessTriggers = AddHook((Expression<Func<ModPlayer, Action<TriggersSet>>>)((ModPlayer p) => p.ProcessTriggers));

	private static HookList<ModPlayer> HookModifyWeaponKnockback = AddHook((Expression<Func<ModPlayer, DelegateModifyWeaponKnockback>>)((ModPlayer p) => p.ModifyWeaponKnockback));

	private static HookList<ModPlayer> HookModifyWeaponCrit = AddHook((Expression<Func<ModPlayer, DelegateModifyWeaponCrit>>)((ModPlayer p) => p.ModifyWeaponCrit));

	private static HookList<ModPlayer> HookCanConsumeAmmo = AddHook((Expression<Func<ModPlayer, Func<Item, Item, bool>>>)((ModPlayer p) => p.CanConsumeAmmo));

	private static HookList<ModPlayer> HookOnConsumeAmmo = AddHook((Expression<Func<ModPlayer, Action<Item, Item>>>)((ModPlayer p) => p.OnConsumeAmmo));

	private static HookList<ModPlayer> HookCanShoot = AddHook((Expression<Func<ModPlayer, Func<Item, bool>>>)((ModPlayer p) => p.CanShoot));

	private static HookList<ModPlayer> HookModifyShootStats = AddHook((Expression<Func<ModPlayer, DelegateModifyShootStats>>)((ModPlayer p) => p.ModifyShootStats));

	private static HookList<ModPlayer> HookShoot = AddHook((Expression<Func<ModPlayer, Func<Item, EntitySource_ItemUse_WithAmmo, Vector2, Vector2, int, int, float, bool>>>)((ModPlayer p) => p.Shoot));

	private static HookList<ModPlayer> HookMeleeEffects = AddHook((Expression<Func<ModPlayer, Action<Item, Rectangle>>>)((ModPlayer p) => p.MeleeEffects));

	private static HookList<ModPlayer> HookCanCatchNPC = AddHook((Expression<Func<ModPlayer, Func<NPC, Item, bool?>>>)((ModPlayer p) => p.CanCatchNPC));

	private static HookList<ModPlayer> HookOnCatchNPC = AddHook((Expression<Func<ModPlayer, Action<NPC, Item, bool>>>)((ModPlayer p) => p.OnCatchNPC));

	private static HookList<ModPlayer> HookModifyItemScale = AddHook((Expression<Func<ModPlayer, DelegateModifyItemScale>>)((ModPlayer p) => p.ModifyItemScale));

	private static HookList<ModPlayer> HookOnHitAnything = AddHook((Expression<Func<ModPlayer, Action<float, float, Entity>>>)((ModPlayer p) => p.OnHitAnything));

	private static HookList<ModPlayer> HookCanHitNPC = AddHook((Expression<Func<ModPlayer, Func<NPC, bool>>>)((ModPlayer p) => p.CanHitNPC));

	private static HookList<ModPlayer> HookCanCollideNPCWithItem = AddHook((Expression<Func<ModPlayer, Func<Item, Rectangle, NPC, bool?>>>)((ModPlayer p) => p.CanMeleeAttackCollideWithNPC));

	private static HookList<ModPlayer> HookModifyHitNPC = AddHook((Expression<Func<ModPlayer, DelegateModifyHitNPC>>)((ModPlayer p) => p.ModifyHitNPC));

	private static HookList<ModPlayer> HookOnHitNPC = AddHook((Expression<Func<ModPlayer, Action<NPC, NPC.HitInfo, int>>>)((ModPlayer p) => p.OnHitNPC));

	private static HookList<ModPlayer> HookCanHitNPCWithItem = AddHook((Expression<Func<ModPlayer, Func<Item, NPC, bool?>>>)((ModPlayer p) => p.CanHitNPCWithItem));

	private static HookList<ModPlayer> HookModifyHitNPCWithItem = AddHook((Expression<Func<ModPlayer, DelegateModifyHitNPCWithItem>>)((ModPlayer p) => p.ModifyHitNPCWithItem));

	private static HookList<ModPlayer> HookOnHitNPCWithItem = AddHook((Expression<Func<ModPlayer, Action<Item, NPC, NPC.HitInfo, int>>>)((ModPlayer p) => p.OnHitNPCWithItem));

	private static HookList<ModPlayer> HookCanHitNPCWithProj = AddHook((Expression<Func<ModPlayer, Func<Projectile, NPC, bool?>>>)((ModPlayer p) => p.CanHitNPCWithProj));

	private static HookList<ModPlayer> HookModifyHitNPCWithProj = AddHook((Expression<Func<ModPlayer, DelegateModifyHitNPCWithProj>>)((ModPlayer p) => p.ModifyHitNPCWithProj));

	private static HookList<ModPlayer> HookOnHitNPCWithProj = AddHook((Expression<Func<ModPlayer, Action<Projectile, NPC, NPC.HitInfo, int>>>)((ModPlayer p) => p.OnHitNPCWithProj));

	private static HookList<ModPlayer> HookCanHitPvp = AddHook((Expression<Func<ModPlayer, Func<Item, Player, bool>>>)((ModPlayer p) => p.CanHitPvp));

	private static HookList<ModPlayer> HookCanHitPvpWithProj = AddHook((Expression<Func<ModPlayer, Func<Projectile, Player, bool>>>)((ModPlayer p) => p.CanHitPvpWithProj));

	private static HookList<ModPlayer> HookCanBeHitByNPC = AddHook((Expression<Func<ModPlayer, DelegateCanBeHitByNPC>>)((ModPlayer p) => p.CanBeHitByNPC));

	private static HookList<ModPlayer> HookModifyHitByNPC = AddHook((Expression<Func<ModPlayer, DelegateModifyHitByNPC>>)((ModPlayer p) => p.ModifyHitByNPC));

	private static HookList<ModPlayer> HookOnHitByNPC = AddHook((Expression<Func<ModPlayer, Action<NPC, Player.HurtInfo>>>)((ModPlayer p) => p.OnHitByNPC));

	private static HookList<ModPlayer> HookCanBeHitByProjectile = AddHook((Expression<Func<ModPlayer, Func<Projectile, bool>>>)((ModPlayer p) => p.CanBeHitByProjectile));

	private static HookList<ModPlayer> HookModifyHitByProjectile = AddHook((Expression<Func<ModPlayer, DelegateModifyHitByProjectile>>)((ModPlayer p) => p.ModifyHitByProjectile));

	private static HookList<ModPlayer> HookOnHitByProjectile = AddHook((Expression<Func<ModPlayer, Action<Projectile, Player.HurtInfo>>>)((ModPlayer p) => p.OnHitByProjectile));

	private static HookList<ModPlayer> HookModifyFishingAttempt = AddHook((Expression<Func<ModPlayer, DelegateModifyFishingAttempt>>)((ModPlayer p) => p.ModifyFishingAttempt));

	private static HookList<ModPlayer> HookCatchFish = AddHook((Expression<Func<ModPlayer, DelegateCatchFish>>)((ModPlayer p) => p.CatchFish));

	private static HookList<ModPlayer> HookCaughtFish = AddHook((Expression<Func<ModPlayer, DelegateModifyCaughtFish>>)((ModPlayer p) => p.ModifyCaughtFish));

	private static HookList<ModPlayer> HookCanConsumeBait = AddHook((Expression<Func<ModPlayer, DelegateCanConsumeBait>>)((ModPlayer p) => p.CanConsumeBait));

	private static HookList<ModPlayer> HookGetFishingLevel = AddHook((Expression<Func<ModPlayer, DelegateGetFishingLevel>>)((ModPlayer p) => p.GetFishingLevel));

	private static HookList<ModPlayer> HookAnglerQuestReward = AddHook((Expression<Func<ModPlayer, Action<float, List<Item>>>>)((ModPlayer p) => p.AnglerQuestReward));

	private static HookList<ModPlayer> HookGetDyeTraderReward = AddHook((Expression<Func<ModPlayer, Action<List<int>>>>)((ModPlayer p) => p.GetDyeTraderReward));

	private static HookList<ModPlayer> HookDrawEffects = AddHook((Expression<Func<ModPlayer, DelegateDrawEffects>>)((ModPlayer p) => p.DrawEffects));

	private static HookList<ModPlayer> HookModifyDrawInfo = AddHook((Expression<Func<ModPlayer, DelegateModifyDrawInfo>>)((ModPlayer p) => p.ModifyDrawInfo));

	private static HookList<ModPlayer> HookModifyDrawLayerOrdering = AddHook((Expression<Func<ModPlayer, Action<IDictionary<PlayerDrawLayer, PlayerDrawLayer.Position>>>>)((ModPlayer p) => p.ModifyDrawLayerOrdering));

	private static HookList<ModPlayer> HookModifyDrawLayers = AddHook((Expression<Func<ModPlayer, Action<PlayerDrawSet>>>)((ModPlayer p) => p.HideDrawLayers));

	private static HookList<ModPlayer> HookModifyScreenPosition = AddHook((Expression<Func<ModPlayer, Action>>)((ModPlayer p) => p.ModifyScreenPosition));

	private static HookList<ModPlayer> HookModifyZoom = AddHook((Expression<Func<ModPlayer, DelegateModifyZoom>>)((ModPlayer p) => p.ModifyZoom));

	private static HookList<ModPlayer> HookPlayerConnect = AddHook((Expression<Func<ModPlayer, Action>>)((ModPlayer p) => p.PlayerConnect));

	private static HookList<ModPlayer> HookPlayerDisconnect = AddHook((Expression<Func<ModPlayer, Action>>)((ModPlayer p) => p.PlayerDisconnect));

	private static HookList<ModPlayer> HookOnEnterWorld = AddHook((Expression<Func<ModPlayer, Action>>)((ModPlayer p) => p.OnEnterWorld));

	private static HookList<ModPlayer> HookOnRespawn = AddHook((Expression<Func<ModPlayer, Action>>)((ModPlayer p) => p.OnRespawn));

	private static HookList<ModPlayer> HookShiftClickSlot = AddHook((Expression<Func<ModPlayer, Func<Item[], int, int, bool>>>)((ModPlayer p) => p.ShiftClickSlot));

	private static HookList<ModPlayer> HookHoverSlot = AddHook((Expression<Func<ModPlayer, Func<Item[], int, int, bool>>>)((ModPlayer p) => p.HoverSlot));

	private static HookList<ModPlayer> HookPostSellItem = AddHook((Expression<Func<ModPlayer, Action<NPC, Item[], Item>>>)((ModPlayer p) => p.PostSellItem));

	private static HookList<ModPlayer> HookCanSellItem = AddHook((Expression<Func<ModPlayer, Func<NPC, Item[], Item, bool>>>)((ModPlayer p) => p.CanSellItem));

	private static HookList<ModPlayer> HookPostBuyItem = AddHook((Expression<Func<ModPlayer, Action<NPC, Item[], Item>>>)((ModPlayer p) => p.PostBuyItem));

	private static HookList<ModPlayer> HookCanBuyItem = AddHook((Expression<Func<ModPlayer, Func<NPC, Item[], Item, bool>>>)((ModPlayer p) => p.CanBuyItem));

	private static HookList<ModPlayer> HookCanUseItem = AddHook((Expression<Func<ModPlayer, Func<Item, bool>>>)((ModPlayer p) => p.CanUseItem));

	private static HookList<ModPlayer> HookCanAutoReuseItem = AddHook((Expression<Func<ModPlayer, Func<Item, bool?>>>)((ModPlayer p) => p.CanAutoReuseItem));

	private static readonly HookList<ModPlayer> HookModifyNurseHeal = AddHook((Expression<Func<ModPlayer, DelegateModifyNurseHeal>>)((ModPlayer p) => p.ModifyNurseHeal));

	private static HookList<ModPlayer> HookModifyNursePrice = AddHook((Expression<Func<ModPlayer, DelegateModifyNursePrice>>)((ModPlayer p) => p.ModifyNursePrice));

	private static HookList<ModPlayer> HookPostNurseHeal = AddHook((Expression<Func<ModPlayer, Action<NPC, int, bool, int>>>)((ModPlayer p) => p.PostNurseHeal));

	private static HookList<ModPlayer> HookAddStartingItems = AddHook((Expression<Func<ModPlayer, Func<bool, IEnumerable<Item>>>>)((ModPlayer p) => p.AddStartingItems));

	private static HookList<ModPlayer> HookModifyStartingInventory = AddHook((Expression<Func<ModPlayer, Action<IReadOnlyDictionary<string, List<Item>>, bool>>>)((ModPlayer p) => p.ModifyStartingInventory));

	private static HookList<ModPlayer> HookAddCraftingMaterials = AddHook((Expression<Func<ModPlayer, DelegateFindMaterialsFrom>>)((ModPlayer p) => p.AddMaterialsForCrafting));

	private static HookList<ModPlayer> HookOnPickup = AddHook((Expression<Func<ModPlayer, Func<Item, bool>>>)((ModPlayer p) => p.OnPickup));

	private static HookList<ModPlayer> AddHook<F>(Expression<Func<ModPlayer, F>> func) where F : Delegate
	{
		HookList<ModPlayer> hook = HookList<ModPlayer>.Create(func);
		hooks.Add(hook);
		return hook;
	}

	public static T AddModHook<T>(T hook) where T : HookList<ModPlayer>
	{
		hook.Update(players);
		modHooks.Add(hook);
		return hook;
	}

	internal static void Add(ModPlayer player)
	{
		player.Index = (ushort)players.Count;
		players.Add(player);
	}

	internal static void ResizeArrays()
	{
		foreach (HookList<ModPlayer> item in hooks.Union(modHooks))
		{
			item.Update(players);
		}
	}

	internal static void Unload()
	{
		players.Clear();
		modHooks.Clear();
	}

	internal static void SetupPlayer(Player player)
	{
		player.modPlayers = NewInstances(player, CollectionsMarshal.AsSpan(players));
		FilteredSpanEnumerator<ModPlayer> enumerator = HookInitialize.Enumerate(player).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.Initialize();
		}
	}

	private static ModPlayer[] NewInstances(Player player, Span<ModPlayer> modPlayers)
	{
		ModPlayer[] arr = new ModPlayer[modPlayers.Length];
		for (int i = 0; i < modPlayers.Length; i++)
		{
			arr[i] = modPlayers[i].NewInstance(player);
		}
		return arr;
	}

	public static void ResetEffects(Player player)
	{
		FilteredSpanEnumerator<ModPlayer> enumerator = HookResetEffects.Enumerate(player).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.ResetEffects();
		}
	}

	public static void ResetInfoAccessories(Player player)
	{
		FilteredSpanEnumerator<ModPlayer> enumerator = HookResetInfoAccessories.Enumerate(player).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.ResetInfoAccessories();
		}
	}

	public static void RefreshInfoAccessoriesFromTeamPlayers(Player player, Player otherPlayer)
	{
		FilteredSpanEnumerator<ModPlayer> enumerator = HookRefreshInfoAccessoriesFromTeamPlayers.Enumerate(player).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.RefreshInfoAccessoriesFromTeamPlayers(otherPlayer);
		}
	}

	/// <summary>
	/// Resets <see cref="F:Terraria.Player.statLifeMax" /> and <see cref="F:Terraria.Player.statManaMax" /> to their expected values by vanilla
	/// </summary>
	/// <param name="player"></param>
	public static void ResetMaxStatsToVanilla(Player player)
	{
		player.statLifeMax = 100 + player.ConsumedLifeCrystals * 20 + player.ConsumedLifeFruit * 5;
		player.statManaMax = 20 + player.ConsumedManaCrystals * 20;
	}

	/// <summary>
	/// Reset this player's <see cref="F:Terraria.Player.statLifeMax" /> and <see cref="F:Terraria.Player.statManaMax" /> to their vanilla defaults,
	/// applies <see cref="M:Terraria.ModLoader.ModPlayer.ModifyMaxStats(Terraria.ModLoader.StatModifier@,Terraria.ModLoader.StatModifier@)" /> to them,
	/// then modifies <see cref="F:Terraria.Player.statLifeMax2" /> and <see cref="F:Terraria.Player.statManaMax2" />
	/// </summary>
	/// <param name="player"></param>
	public static void ModifyMaxStats(Player player)
	{
		ResetMaxStatsToVanilla(player);
		StatModifier cumulativeHealth = StatModifier.Default;
		StatModifier cumulativeMana = StatModifier.Default;
		FilteredSpanEnumerator<ModPlayer> enumerator = HookModifyMaxStats.Enumerate(player).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.ModifyMaxStats(out var health, out var mana);
			cumulativeHealth = cumulativeHealth.CombineWith(health);
			cumulativeMana = cumulativeMana.CombineWith(mana);
		}
		player.statLifeMax = (int)cumulativeHealth.ApplyTo(player.statLifeMax);
		player.statManaMax = (int)cumulativeMana.ApplyTo(player.statManaMax);
	}

	public static void UpdateDead(Player player)
	{
		FilteredSpanEnumerator<ModPlayer> enumerator = HookUpdateDead.Enumerate(player).GetEnumerator();
		while (enumerator.MoveNext())
		{
			ModPlayer modPlayer = enumerator.Current;
			try
			{
				modPlayer.UpdateDead();
			}
			catch
			{
			}
		}
	}

	public static void SetStartInventory(Player player, IList<Item> items)
	{
		if (items.Count <= 50)
		{
			for (int i = 0; i < 50; i++)
			{
				if (i < items.Count)
				{
					player.inventory[i] = items[i];
				}
				else
				{
					player.inventory[i].SetDefaults();
				}
			}
			return;
		}
		for (int k = 0; k < 49; k++)
		{
			player.inventory[k] = items[k];
		}
		Item bag = new Item();
		bag.SetDefaults(ModContent.ItemType<StartBag>());
		for (int j = 49; j < items.Count; j++)
		{
			((StartBag)bag.ModItem).AddItem(items[j]);
		}
		player.inventory[49] = bag;
	}

	public static void PreSavePlayer(Player player)
	{
		FilteredSpanEnumerator<ModPlayer> enumerator = HookPreSavePlayer.Enumerate(player).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.PreSavePlayer();
		}
	}

	public static void PostSavePlayer(Player player)
	{
		FilteredSpanEnumerator<ModPlayer> enumerator = HookPostSavePlayer.Enumerate(player).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.PostSavePlayer();
		}
	}

	public static void CopyClientState(Player player, Player targetCopy)
	{
		FilteredSpanEnumerator<ModPlayer> enumerator = HookCopyClientState.Enumerate(player).GetEnumerator();
		while (enumerator.MoveNext())
		{
			ModPlayer modPlayer = enumerator.Current;
			try
			{
				modPlayer.CopyClientState(targetCopy.modPlayers[modPlayer.Index]);
			}
			catch
			{
			}
		}
	}

	public static void SyncPlayer(Player player, int toWho, int fromWho, bool newPlayer)
	{
		FilteredSpanEnumerator<ModPlayer> enumerator = HookSyncPlayer.Enumerate(player).GetEnumerator();
		while (enumerator.MoveNext())
		{
			ModPlayer modPlayer = enumerator.Current;
			try
			{
				modPlayer.SyncPlayer(toWho, fromWho, newPlayer);
			}
			catch
			{
			}
		}
	}

	public static void SendClientChanges(Player player, Player clientPlayer)
	{
		FilteredSpanEnumerator<ModPlayer> enumerator = HookSendClientChanges.Enumerate(player).GetEnumerator();
		while (enumerator.MoveNext())
		{
			ModPlayer modPlayer = enumerator.Current;
			try
			{
				modPlayer.SendClientChanges(clientPlayer.modPlayers[modPlayer.Index]);
			}
			catch
			{
			}
		}
	}

	public static void UpdateBadLifeRegen(Player player)
	{
		FilteredSpanEnumerator<ModPlayer> enumerator = HookUpdateBadLifeRegen.Enumerate(player).GetEnumerator();
		while (enumerator.MoveNext())
		{
			ModPlayer modPlayer = enumerator.Current;
			try
			{
				modPlayer.UpdateBadLifeRegen();
			}
			catch
			{
			}
		}
	}

	public static void UpdateLifeRegen(Player player)
	{
		FilteredSpanEnumerator<ModPlayer> enumerator = HookUpdateLifeRegen.Enumerate(player).GetEnumerator();
		while (enumerator.MoveNext())
		{
			ModPlayer modPlayer = enumerator.Current;
			try
			{
				modPlayer.UpdateLifeRegen();
			}
			catch
			{
			}
		}
	}

	public static void NaturalLifeRegen(Player player, ref float regen)
	{
		FilteredSpanEnumerator<ModPlayer> enumerator = HookNaturalLifeRegen.Enumerate(player).GetEnumerator();
		while (enumerator.MoveNext())
		{
			ModPlayer modPlayer = enumerator.Current;
			try
			{
				modPlayer.NaturalLifeRegen(ref regen);
			}
			catch
			{
			}
		}
	}

	public static void UpdateAutopause(Player player)
	{
		FilteredSpanEnumerator<ModPlayer> enumerator = HookUpdateAutopause.Enumerate(player).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.UpdateAutopause();
		}
	}

	public static void PreUpdate(Player player)
	{
		FilteredSpanEnumerator<ModPlayer> enumerator = HookPreUpdate.Enumerate(player).GetEnumerator();
		while (enumerator.MoveNext())
		{
			ModPlayer modPlayer = enumerator.Current;
			try
			{
				modPlayer.PreUpdate();
			}
			catch
			{
			}
		}
	}

	public static void SetControls(Player player)
	{
		FilteredSpanEnumerator<ModPlayer> enumerator = HookSetControls.Enumerate(player).GetEnumerator();
		while (enumerator.MoveNext())
		{
			ModPlayer modPlayer = enumerator.Current;
			try
			{
				modPlayer.SetControls();
			}
			catch
			{
			}
		}
	}

	public static void PreUpdateBuffs(Player player)
	{
		FilteredSpanEnumerator<ModPlayer> enumerator = HookPreUpdateBuffs.Enumerate(player).GetEnumerator();
		while (enumerator.MoveNext())
		{
			ModPlayer modPlayer = enumerator.Current;
			try
			{
				modPlayer.PreUpdateBuffs();
			}
			catch
			{
			}
		}
	}

	public static void PostUpdateBuffs(Player player)
	{
		FilteredSpanEnumerator<ModPlayer> enumerator = HookPostUpdateBuffs.Enumerate(player).GetEnumerator();
		while (enumerator.MoveNext())
		{
			ModPlayer modPlayer = enumerator.Current;
			try
			{
				modPlayer.PostUpdateBuffs();
			}
			catch
			{
			}
		}
	}

	public static void UpdateEquips(Player player)
	{
		FilteredSpanEnumerator<ModPlayer> enumerator = HookUpdateEquips.Enumerate(player).GetEnumerator();
		while (enumerator.MoveNext())
		{
			ModPlayer modPlayer = enumerator.Current;
			try
			{
				modPlayer.UpdateEquips();
			}
			catch
			{
			}
		}
	}

	public static void PostUpdateEquips(Player player)
	{
		FilteredSpanEnumerator<ModPlayer> enumerator = HookPostUpdateEquips.Enumerate(player).GetEnumerator();
		while (enumerator.MoveNext())
		{
			ModPlayer modPlayer = enumerator.Current;
			try
			{
				modPlayer.PostUpdateEquips();
			}
			catch
			{
			}
		}
	}

	public static void UpdateVisibleAccessories(Player player)
	{
		FilteredSpanEnumerator<ModPlayer> enumerator = HookUpdateVisibleAccessories.Enumerate(player).GetEnumerator();
		while (enumerator.MoveNext())
		{
			ModPlayer modPlayer = enumerator.Current;
			try
			{
				modPlayer.UpdateVisibleAccessories();
			}
			catch
			{
			}
		}
	}

	public static void UpdateVisibleVanityAccessories(Player player)
	{
		FilteredSpanEnumerator<ModPlayer> enumerator = HookUpdateVisibleVanityAccessories.Enumerate(player).GetEnumerator();
		while (enumerator.MoveNext())
		{
			ModPlayer modPlayer = enumerator.Current;
			try
			{
				modPlayer.UpdateVisibleVanityAccessories();
			}
			catch
			{
			}
		}
	}

	public static void UpdateDyes(Player player)
	{
		FilteredSpanEnumerator<ModPlayer> enumerator = HookUpdateDyes.Enumerate(player).GetEnumerator();
		while (enumerator.MoveNext())
		{
			ModPlayer modPlayer = enumerator.Current;
			try
			{
				modPlayer.UpdateDyes();
			}
			catch
			{
			}
		}
	}

	public static void PostUpdateMiscEffects(Player player)
	{
		FilteredSpanEnumerator<ModPlayer> enumerator = HookPostUpdateMiscEffects.Enumerate(player).GetEnumerator();
		while (enumerator.MoveNext())
		{
			ModPlayer modPlayer = enumerator.Current;
			try
			{
				modPlayer.PostUpdateMiscEffects();
			}
			catch
			{
			}
		}
	}

	public static void PostUpdateRunSpeeds(Player player)
	{
		FilteredSpanEnumerator<ModPlayer> enumerator = HookPostUpdateRunSpeeds.Enumerate(player).GetEnumerator();
		while (enumerator.MoveNext())
		{
			ModPlayer modPlayer = enumerator.Current;
			try
			{
				modPlayer.PostUpdateRunSpeeds();
			}
			catch
			{
			}
		}
	}

	public static void PreUpdateMovement(Player player)
	{
		FilteredSpanEnumerator<ModPlayer> enumerator = HookPreUpdateMovement.Enumerate(player).GetEnumerator();
		while (enumerator.MoveNext())
		{
			ModPlayer modPlayer = enumerator.Current;
			try
			{
				modPlayer.PreUpdateMovement();
			}
			catch
			{
			}
		}
	}

	public static void PostUpdate(Player player)
	{
		FilteredSpanEnumerator<ModPlayer> enumerator = HookPostUpdate.Enumerate(player).GetEnumerator();
		while (enumerator.MoveNext())
		{
			ModPlayer modPlayer = enumerator.Current;
			try
			{
				modPlayer.PostUpdate();
			}
			catch
			{
			}
		}
	}

	public static void ModifyExtraJumpDurationMultiplier(ExtraJump jump, Player player, ref float duration)
	{
		FilteredSpanEnumerator<ModPlayer> enumerator = HookModifyExtraJumpDurationMultiplier.Enumerate(player).GetEnumerator();
		while (enumerator.MoveNext())
		{
			ModPlayer modPlayer = enumerator.Current;
			try
			{
				modPlayer.ModifyExtraJumpDurationMultiplier(jump, ref duration);
			}
			catch
			{
			}
		}
	}

	public static bool CanStartExtraJump(ExtraJump jump, Player player)
	{
		FilteredSpanEnumerator<ModPlayer> enumerator = HookCanStartExtraJump.Enumerate(player).GetEnumerator();
		while (enumerator.MoveNext())
		{
			ModPlayer modPlayer = enumerator.Current;
			try
			{
				if (!modPlayer.CanStartExtraJump(jump))
				{
					return false;
				}
			}
			catch
			{
			}
		}
		return true;
	}

	public static void OnExtraJumpStarted(ExtraJump jump, Player player, ref bool playSound)
	{
		FilteredSpanEnumerator<ModPlayer> enumerator = HookOnExtraJumpStarted.Enumerate(player).GetEnumerator();
		while (enumerator.MoveNext())
		{
			ModPlayer modPlayer = enumerator.Current;
			try
			{
				modPlayer.OnExtraJumpStarted(jump, ref playSound);
			}
			catch
			{
			}
		}
	}

	public static void OnExtraJumpEnded(ExtraJump jump, Player player)
	{
		FilteredSpanEnumerator<ModPlayer> enumerator = HookOnExtraJumpEnded.Enumerate(player).GetEnumerator();
		while (enumerator.MoveNext())
		{
			ModPlayer modPlayer = enumerator.Current;
			try
			{
				modPlayer.OnExtraJumpEnded(jump);
			}
			catch
			{
			}
		}
	}

	public static void OnExtraJumpRefreshed(ExtraJump jump, Player player)
	{
		FilteredSpanEnumerator<ModPlayer> enumerator = HookOnExtraJumpRefreshed.Enumerate(player).GetEnumerator();
		while (enumerator.MoveNext())
		{
			ModPlayer modPlayer = enumerator.Current;
			try
			{
				modPlayer.OnExtraJumpRefreshed(jump);
			}
			catch
			{
			}
		}
	}

	public static void ExtraJumpVisuals(ExtraJump jump, Player player)
	{
		FilteredSpanEnumerator<ModPlayer> enumerator = HookExtraJumpVisuals.Enumerate(player).GetEnumerator();
		while (enumerator.MoveNext())
		{
			ModPlayer modPlayer = enumerator.Current;
			try
			{
				modPlayer.ExtraJumpVisuals(jump);
			}
			catch
			{
			}
		}
	}

	public static bool CanShowExtraJumpVisuals(ExtraJump jump, Player player)
	{
		FilteredSpanEnumerator<ModPlayer> enumerator = HookCanShowExtraJumpVisuals.Enumerate(player).GetEnumerator();
		while (enumerator.MoveNext())
		{
			ModPlayer modPlayer = enumerator.Current;
			try
			{
				if (!modPlayer.CanShowExtraJumpVisuals(jump))
				{
					return false;
				}
			}
			catch
			{
			}
		}
		return true;
	}

	public static void FrameEffects(Player player)
	{
		FilteredSpanEnumerator<ModPlayer> enumerator = HookFrameEffects.Enumerate(player).GetEnumerator();
		while (enumerator.MoveNext())
		{
			ModPlayer modPlayer = enumerator.Current;
			try
			{
				modPlayer.FrameEffects();
			}
			catch
			{
			}
		}
	}

	public static bool ImmuneTo(Player player, PlayerDeathReason damageSource, int cooldownCounter, bool dodgeable)
	{
		FilteredSpanEnumerator<ModPlayer> enumerator = HookImmuneTo.Enumerate(player).GetEnumerator();
		while (enumerator.MoveNext())
		{
			if (enumerator.Current.ImmuneTo(damageSource, cooldownCounter, dodgeable))
			{
				return true;
			}
		}
		return false;
	}

	public static bool FreeDodge(Player player, in Player.HurtInfo info)
	{
		FilteredSpanEnumerator<ModPlayer> enumerator = HookFreeDodge.Enumerate(player).GetEnumerator();
		while (enumerator.MoveNext())
		{
			if (enumerator.Current.FreeDodge(info))
			{
				return true;
			}
		}
		return false;
	}

	public static bool ConsumableDodge(Player player, in Player.HurtInfo info)
	{
		FilteredSpanEnumerator<ModPlayer> enumerator = HookConsumableDodge.Enumerate(player).GetEnumerator();
		while (enumerator.MoveNext())
		{
			if (enumerator.Current.ConsumableDodge(info))
			{
				return true;
			}
		}
		return false;
	}

	public static void ModifyHurt(Player player, ref Player.HurtModifiers modifiers)
	{
		if (modifiers.DamageSource.TryGetCausingEntity(out var sourceEntity))
		{
			Entity entity = sourceEntity;
			if (!(entity is Projectile proj))
			{
				if (!(entity is NPC npc))
				{
					if (entity is Player sourcePlayer)
					{
						Item item = modifiers.DamageSource.SourceItem;
						if (item != null && modifiers.PvP)
						{
							ItemLoader.ModifyHitPvp(item, sourcePlayer, player, ref modifiers);
						}
					}
				}
				else
				{
					CombinedHooks.ModifyHitByNPC(player, npc, ref modifiers);
				}
			}
			else
			{
				CombinedHooks.ModifyHitByProjectile(player, proj, ref modifiers);
			}
		}
		FilteredSpanEnumerator<ModPlayer> enumerator = HookModifyHurt.Enumerate(player).GetEnumerator();
		while (enumerator.MoveNext())
		{
			ModPlayer modPlayer = enumerator.Current;
			try
			{
				modPlayer.ModifyHurt(ref modifiers);
			}
			catch
			{
			}
		}
	}

	public static void OnHurt(Player player, Player.HurtInfo info)
	{
		if (info.DamageSource.TryGetCausingEntity(out var sourceEntity))
		{
			Entity entity = sourceEntity;
			if (!(entity is Projectile proj))
			{
				if (!(entity is NPC npc))
				{
					if (entity is Player sourcePlayer)
					{
						Item item = info.DamageSource.SourceItem;
						if (item != null && info.PvP)
						{
							ItemLoader.OnHitPvp(item, sourcePlayer, player, info);
						}
					}
				}
				else if (player == Main.LocalPlayer)
				{
					CombinedHooks.OnHitByNPC(player, npc, in info);
				}
			}
			else if (player == Main.LocalPlayer)
			{
				CombinedHooks.OnHitByProjectile(player, proj, in info);
			}
		}
		FilteredSpanEnumerator<ModPlayer> enumerator = HookHurt.Enumerate(player).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.OnHurt(info);
		}
	}

	public static void PostHurt(Player player, Player.HurtInfo info)
	{
		FilteredSpanEnumerator<ModPlayer> enumerator = HookPostHurt.Enumerate(player).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.PostHurt(info);
		}
	}

	public static bool PreKill(Player player, double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
	{
		bool ret = true;
		FilteredSpanEnumerator<ModPlayer> enumerator = HookPreKill.Enumerate(player).GetEnumerator();
		while (enumerator.MoveNext())
		{
			ModPlayer modPlayer = enumerator.Current;
			ret &= modPlayer.PreKill(damage, hitDirection, pvp, ref playSound, ref genGore, ref damageSource);
		}
		return ret;
	}

	public static void Kill(Player player, double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
	{
		FilteredSpanEnumerator<ModPlayer> enumerator = HookKill.Enumerate(player).GetEnumerator();
		while (enumerator.MoveNext())
		{
			ModPlayer modPlayer = enumerator.Current;
			try
			{
				modPlayer.Kill(damage, hitDirection, pvp, damageSource);
			}
			catch
			{
			}
		}
	}

	public static bool PreModifyLuck(Player player, ref float luck)
	{
		bool ret = true;
		FilteredSpanEnumerator<ModPlayer> enumerator = HookPreModifyLuck.Enumerate(player).GetEnumerator();
		while (enumerator.MoveNext())
		{
			ModPlayer modPlayer = enumerator.Current;
			ret &= modPlayer.PreModifyLuck(ref luck);
		}
		return ret;
	}

	public static void ModifyLuck(Player player, ref float luck)
	{
		FilteredSpanEnumerator<ModPlayer> enumerator = HookModifyLuck.Enumerate(player).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.ModifyLuck(ref luck);
		}
	}

	public static bool PreItemCheck(Player player)
	{
		bool ret = true;
		FilteredSpanEnumerator<ModPlayer> enumerator = HookPreItemCheck.Enumerate(player).GetEnumerator();
		while (enumerator.MoveNext())
		{
			ModPlayer modPlayer = enumerator.Current;
			try
			{
				ret &= modPlayer.PreItemCheck();
			}
			catch
			{
			}
		}
		return ret;
	}

	public static void PostItemCheck(Player player)
	{
		FilteredSpanEnumerator<ModPlayer> enumerator = HookPostItemCheck.Enumerate(player).GetEnumerator();
		while (enumerator.MoveNext())
		{
			ModPlayer modPlayer = enumerator.Current;
			try
			{
				modPlayer.PostItemCheck();
			}
			catch
			{
			}
		}
	}

	public static float UseTimeMultiplier(Player player, Item item)
	{
		float multiplier = 1f;
		if (item.IsAir)
		{
			return multiplier;
		}
		FilteredSpanEnumerator<ModPlayer> enumerator = HookUseTimeMultiplier.Enumerate(player).GetEnumerator();
		while (enumerator.MoveNext())
		{
			ModPlayer modPlayer = enumerator.Current;
			multiplier *= modPlayer.UseTimeMultiplier(item);
		}
		return multiplier;
	}

	public static float UseAnimationMultiplier(Player player, Item item)
	{
		float multiplier = 1f;
		if (item.IsAir)
		{
			return multiplier;
		}
		FilteredSpanEnumerator<ModPlayer> enumerator = HookUseAnimationMultiplier.Enumerate(player).GetEnumerator();
		while (enumerator.MoveNext())
		{
			ModPlayer modPlayer = enumerator.Current;
			multiplier *= modPlayer.UseAnimationMultiplier(item);
		}
		return multiplier;
	}

	public static float UseSpeedMultiplier(Player player, Item item)
	{
		float multiplier = 1f;
		if (item.IsAir)
		{
			return multiplier;
		}
		FilteredSpanEnumerator<ModPlayer> enumerator = HookUseSpeedMultiplier.Enumerate(player).GetEnumerator();
		while (enumerator.MoveNext())
		{
			ModPlayer modPlayer = enumerator.Current;
			multiplier *= modPlayer.UseSpeedMultiplier(item);
		}
		return multiplier;
	}

	public static void GetHealLife(Player player, Item item, bool quickHeal, ref int healValue)
	{
		if (!item.IsAir)
		{
			FilteredSpanEnumerator<ModPlayer> enumerator = HookGetHealLife.Enumerate(player).GetEnumerator();
			while (enumerator.MoveNext())
			{
				enumerator.Current.GetHealLife(item, quickHeal, ref healValue);
			}
		}
	}

	public static void GetHealMana(Player player, Item item, bool quickHeal, ref int healValue)
	{
		if (!item.IsAir)
		{
			FilteredSpanEnumerator<ModPlayer> enumerator = HookGetHealMana.Enumerate(player).GetEnumerator();
			while (enumerator.MoveNext())
			{
				enumerator.Current.GetHealMana(item, quickHeal, ref healValue);
			}
		}
	}

	public static void ModifyManaCost(Player player, Item item, ref float reduce, ref float mult)
	{
		if (!item.IsAir)
		{
			FilteredSpanEnumerator<ModPlayer> enumerator = HookModifyManaCost.Enumerate(player).GetEnumerator();
			while (enumerator.MoveNext())
			{
				enumerator.Current.ModifyManaCost(item, ref reduce, ref mult);
			}
		}
	}

	public static void OnMissingMana(Player player, Item item, int manaNeeded)
	{
		if (!item.IsAir)
		{
			FilteredSpanEnumerator<ModPlayer> enumerator = HookOnMissingMana.Enumerate(player).GetEnumerator();
			while (enumerator.MoveNext())
			{
				enumerator.Current.OnMissingMana(item, manaNeeded);
			}
		}
	}

	public static void OnConsumeMana(Player player, Item item, int manaConsumed)
	{
		if (!item.IsAir)
		{
			FilteredSpanEnumerator<ModPlayer> enumerator = HookOnConsumeMana.Enumerate(player).GetEnumerator();
			while (enumerator.MoveNext())
			{
				enumerator.Current.OnConsumeMana(item, manaConsumed);
			}
		}
	}

	/// <summary>
	/// Calls ModItem.HookModifyWeaponDamage, then all GlobalItem.HookModifyWeaponDamage hooks.
	/// </summary>
	public static void ModifyWeaponDamage(Player player, Item item, ref StatModifier damage)
	{
		if (!item.IsAir)
		{
			FilteredSpanEnumerator<ModPlayer> enumerator = HookModifyWeaponDamage.Enumerate(player).GetEnumerator();
			while (enumerator.MoveNext())
			{
				enumerator.Current.ModifyWeaponDamage(item, ref damage);
			}
		}
	}

	public static void ProcessTriggers(Player player, TriggersSet triggersSet)
	{
		FilteredSpanEnumerator<ModPlayer> enumerator = HookProcessTriggers.Enumerate(player).GetEnumerator();
		while (enumerator.MoveNext())
		{
			ModPlayer modPlayer = enumerator.Current;
			try
			{
				modPlayer.ProcessTriggers(triggersSet);
			}
			catch
			{
			}
		}
	}

	public static void ModifyWeaponKnockback(Player player, Item item, ref StatModifier knockback)
	{
		if (!item.IsAir)
		{
			FilteredSpanEnumerator<ModPlayer> enumerator = HookModifyWeaponKnockback.Enumerate(player).GetEnumerator();
			while (enumerator.MoveNext())
			{
				enumerator.Current.ModifyWeaponKnockback(item, ref knockback);
			}
		}
	}

	public static void ModifyWeaponCrit(Player player, Item item, ref float crit)
	{
		if (!item.IsAir)
		{
			FilteredSpanEnumerator<ModPlayer> enumerator = HookModifyWeaponCrit.Enumerate(player).GetEnumerator();
			while (enumerator.MoveNext())
			{
				enumerator.Current.ModifyWeaponCrit(item, ref crit);
			}
		}
	}

	public static bool CanConsumeAmmo(Player player, Item weapon, Item ammo)
	{
		FilteredSpanEnumerator<ModPlayer> enumerator = HookCanConsumeAmmo.Enumerate(player).GetEnumerator();
		while (enumerator.MoveNext())
		{
			if (!enumerator.Current.CanConsumeAmmo(weapon, ammo))
			{
				return false;
			}
		}
		return true;
	}

	public static void OnConsumeAmmo(Player player, Item weapon, Item ammo)
	{
		FilteredSpanEnumerator<ModPlayer> enumerator = HookOnConsumeAmmo.Enumerate(player).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.OnConsumeAmmo(weapon, ammo);
		}
	}

	public static bool CanShoot(Player player, Item item)
	{
		FilteredSpanEnumerator<ModPlayer> enumerator = HookCanShoot.Enumerate(player).GetEnumerator();
		while (enumerator.MoveNext())
		{
			if (!enumerator.Current.CanShoot(item))
			{
				return false;
			}
		}
		return true;
	}

	public static void ModifyShootStats(Player player, Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
	{
		FilteredSpanEnumerator<ModPlayer> enumerator = HookModifyShootStats.Enumerate(player).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.ModifyShootStats(item, ref position, ref velocity, ref type, ref damage, ref knockback);
		}
	}

	public static bool Shoot(Player player, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		bool defaultResult = true;
		FilteredSpanEnumerator<ModPlayer> enumerator = HookShoot.Enumerate(player).GetEnumerator();
		while (enumerator.MoveNext())
		{
			ModPlayer modPlayer = enumerator.Current;
			defaultResult &= modPlayer.Shoot(item, source, position, velocity, type, damage, knockback);
		}
		return defaultResult;
	}

	public static void MeleeEffects(Player player, Item item, Rectangle hitbox)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		FilteredSpanEnumerator<ModPlayer> enumerator = HookMeleeEffects.Enumerate(player).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.MeleeEffects(item, hitbox);
		}
	}

	public static bool? CanCatchNPC(Player player, NPC target, Item item)
	{
		bool? returnValue = null;
		FilteredSpanEnumerator<ModPlayer> enumerator = HookCanCatchNPC.Enumerate(player).GetEnumerator();
		while (enumerator.MoveNext())
		{
			bool? canCatch = enumerator.Current.CanCatchNPC(target, item);
			if (canCatch.HasValue)
			{
				if (!canCatch.Value)
				{
					return false;
				}
				returnValue = true;
			}
		}
		return returnValue;
	}

	public static void OnCatchNPC(Player player, NPC target, Item item, bool failed)
	{
		FilteredSpanEnumerator<ModPlayer> enumerator = HookOnCatchNPC.Enumerate(player).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.OnCatchNPC(target, item, failed);
		}
	}

	public static void ModifyItemScale(Player player, Item item, ref float scale)
	{
		FilteredSpanEnumerator<ModPlayer> enumerator = HookModifyItemScale.Enumerate(player).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.ModifyItemScale(item, ref scale);
		}
	}

	public static void OnHitAnything(Player player, float x, float y, Entity victim)
	{
		FilteredSpanEnumerator<ModPlayer> enumerator = HookOnHitAnything.Enumerate(player).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.OnHitAnything(x, y, victim);
		}
	}

	public static bool CanHitNPC(Player player, NPC target)
	{
		FilteredSpanEnumerator<ModPlayer> enumerator = HookCanHitNPC.Enumerate(player).GetEnumerator();
		while (enumerator.MoveNext())
		{
			if (!enumerator.Current.CanHitNPC(target))
			{
				return false;
			}
		}
		return true;
	}

	public static bool? CanMeleeAttackCollideWithNPC(Player player, Item item, Rectangle meleeAttackHitbox, NPC target)
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		bool? flag = null;
		FilteredSpanEnumerator<ModPlayer> enumerator = HookCanCollideNPCWithItem.Enumerate(player).GetEnumerator();
		while (enumerator.MoveNext())
		{
			bool? canHit = enumerator.Current.CanMeleeAttackCollideWithNPC(item, meleeAttackHitbox, target);
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

	public static void ModifyHitNPC(Player player, NPC target, ref NPC.HitModifiers modifiers)
	{
		FilteredSpanEnumerator<ModPlayer> enumerator = HookModifyHitNPC.Enumerate(player).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.ModifyHitNPC(target, ref modifiers);
		}
	}

	public static void OnHitNPC(Player player, NPC target, in NPC.HitInfo hit, int damageDone)
	{
		FilteredSpanEnumerator<ModPlayer> enumerator = HookOnHitNPC.Enumerate(player).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.OnHitNPC(target, hit, damageDone);
		}
	}

	public static bool? CanHitNPCWithItem(Player player, Item item, NPC target)
	{
		if (!CanHitNPC(player, target))
		{
			return false;
		}
		bool? ret = null;
		FilteredSpanEnumerator<ModPlayer> enumerator = HookCanHitNPCWithItem.Enumerate(player).GetEnumerator();
		while (enumerator.MoveNext())
		{
			bool? flag = enumerator.Current.CanHitNPCWithItem(item, target);
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

	public static void ModifyHitNPCWithItem(Player player, Item item, NPC target, ref NPC.HitModifiers modifiers)
	{
		ModifyHitNPC(player, target, ref modifiers);
		FilteredSpanEnumerator<ModPlayer> enumerator = HookModifyHitNPCWithItem.Enumerate(player).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.ModifyHitNPCWithItem(item, target, ref modifiers);
		}
	}

	public static void OnHitNPCWithItem(Player player, Item item, NPC target, in NPC.HitInfo hit, int damageDone)
	{
		OnHitNPC(player, target, in hit, damageDone);
		FilteredSpanEnumerator<ModPlayer> enumerator = HookOnHitNPCWithItem.Enumerate(player).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.OnHitNPCWithItem(item, target, hit, damageDone);
		}
	}

	public static bool? CanHitNPCWithProj(Player player, Projectile proj, NPC target)
	{
		if (!CanHitNPC(player, target))
		{
			return false;
		}
		bool? ret = null;
		FilteredSpanEnumerator<ModPlayer> enumerator = HookCanHitNPCWithProj.Enumerate(player).GetEnumerator();
		while (enumerator.MoveNext())
		{
			bool? flag = enumerator.Current.CanHitNPCWithProj(proj, target);
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

	public static void ModifyHitNPCWithProj(Player player, Projectile proj, NPC target, ref NPC.HitModifiers modifiers)
	{
		ModifyHitNPC(player, target, ref modifiers);
		FilteredSpanEnumerator<ModPlayer> enumerator = HookModifyHitNPCWithProj.Enumerate(player).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.ModifyHitNPCWithProj(proj, target, ref modifiers);
		}
	}

	public static void OnHitNPCWithProj(Player player, Projectile proj, NPC target, in NPC.HitInfo hit, int damageDone)
	{
		OnHitNPC(player, target, in hit, damageDone);
		FilteredSpanEnumerator<ModPlayer> enumerator = HookOnHitNPCWithProj.Enumerate(player).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.OnHitNPCWithProj(proj, target, hit, damageDone);
		}
	}

	public static bool CanHitPvp(Player player, Item item, Player target)
	{
		FilteredSpanEnumerator<ModPlayer> enumerator = HookCanHitPvp.Enumerate(player).GetEnumerator();
		while (enumerator.MoveNext())
		{
			if (!enumerator.Current.CanHitPvp(item, target))
			{
				return false;
			}
		}
		return true;
	}

	public static bool CanHitPvpWithProj(Projectile proj, Player target)
	{
		Player player = Main.player[proj.owner];
		FilteredSpanEnumerator<ModPlayer> enumerator = HookCanHitPvpWithProj.Enumerate(player).GetEnumerator();
		while (enumerator.MoveNext())
		{
			if (!enumerator.Current.CanHitPvpWithProj(proj, target))
			{
				return false;
			}
		}
		return true;
	}

	public static bool CanBeHitByNPC(Player player, NPC npc, ref int cooldownSlot)
	{
		FilteredSpanEnumerator<ModPlayer> enumerator = HookCanBeHitByNPC.Enumerate(player).GetEnumerator();
		while (enumerator.MoveNext())
		{
			if (!enumerator.Current.CanBeHitByNPC(npc, ref cooldownSlot))
			{
				return false;
			}
		}
		return true;
	}

	public static void ModifyHitByNPC(Player player, NPC npc, ref Player.HurtModifiers modifiers)
	{
		FilteredSpanEnumerator<ModPlayer> enumerator = HookModifyHitByNPC.Enumerate(player).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.ModifyHitByNPC(npc, ref modifiers);
		}
	}

	public static void OnHitByNPC(Player player, NPC npc, in Player.HurtInfo hurtInfo)
	{
		FilteredSpanEnumerator<ModPlayer> enumerator = HookOnHitByNPC.Enumerate(player).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.OnHitByNPC(npc, hurtInfo);
		}
	}

	public static bool CanBeHitByProjectile(Player player, Projectile proj)
	{
		FilteredSpanEnumerator<ModPlayer> enumerator = HookCanBeHitByProjectile.Enumerate(player).GetEnumerator();
		while (enumerator.MoveNext())
		{
			if (!enumerator.Current.CanBeHitByProjectile(proj))
			{
				return false;
			}
		}
		return true;
	}

	public static void ModifyHitByProjectile(Player player, Projectile proj, ref Player.HurtModifiers modifiers)
	{
		FilteredSpanEnumerator<ModPlayer> enumerator = HookModifyHitByProjectile.Enumerate(player).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.ModifyHitByProjectile(proj, ref modifiers);
		}
	}

	public static void OnHitByProjectile(Player player, Projectile proj, in Player.HurtInfo hurtInfo)
	{
		FilteredSpanEnumerator<ModPlayer> enumerator = HookOnHitByProjectile.Enumerate(player).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.OnHitByProjectile(proj, hurtInfo);
		}
	}

	public static void ModifyFishingAttempt(Player player, ref FishingAttempt attempt)
	{
		FilteredSpanEnumerator<ModPlayer> enumerator = HookModifyFishingAttempt.Enumerate(player).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.ModifyFishingAttempt(ref attempt);
		}
		attempt.rolledItemDrop = (attempt.rolledEnemySpawn = 0);
	}

	public static void CatchFish(Player player, FishingAttempt attempt, ref int itemDrop, ref int enemySpawn, ref AdvancedPopupRequest sonar, ref Vector2 sonarPosition)
	{
		FilteredSpanEnumerator<ModPlayer> enumerator = HookCatchFish.Enumerate(player).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.CatchFish(attempt, ref itemDrop, ref enemySpawn, ref sonar, ref sonarPosition);
		}
	}

	public static void ModifyCaughtFish(Player player, Item fish)
	{
		FilteredSpanEnumerator<ModPlayer> enumerator = HookCaughtFish.Enumerate(player).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.ModifyCaughtFish(fish);
		}
	}

	public static bool? CanConsumeBait(Player player, Item bait)
	{
		bool? ret = null;
		FilteredSpanEnumerator<ModPlayer> enumerator = HookCaughtFish.Enumerate(player).GetEnumerator();
		while (enumerator.MoveNext())
		{
			bool? flag = enumerator.Current.CanConsumeBait(bait);
			if (flag.HasValue)
			{
				bool b = flag.GetValueOrDefault();
				ret = (ret ?? true) && b;
			}
		}
		return ret;
	}

	public static void GetFishingLevel(Player player, Item fishingRod, Item bait, ref float fishingLevel)
	{
		FilteredSpanEnumerator<ModPlayer> enumerator = HookGetFishingLevel.Enumerate(player).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.GetFishingLevel(fishingRod, bait, ref fishingLevel);
		}
	}

	public static void AnglerQuestReward(Player player, float rareMultiplier, List<Item> rewardItems)
	{
		FilteredSpanEnumerator<ModPlayer> enumerator = HookAnglerQuestReward.Enumerate(player).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.AnglerQuestReward(rareMultiplier, rewardItems);
		}
	}

	public static void GetDyeTraderReward(Player player, List<int> rewardPool)
	{
		FilteredSpanEnumerator<ModPlayer> enumerator = HookGetDyeTraderReward.Enumerate(player).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.GetDyeTraderReward(rewardPool);
		}
	}

	public static void DrawEffects(PlayerDrawSet drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
	{
		Player player = drawInfo.drawPlayer;
		FilteredSpanEnumerator<ModPlayer> enumerator = HookDrawEffects.Enumerate(player).GetEnumerator();
		while (enumerator.MoveNext())
		{
			ModPlayer modPlayer = enumerator.Current;
			try
			{
				modPlayer.DrawEffects(drawInfo, ref r, ref g, ref b, ref a, ref fullBright);
			}
			catch
			{
			}
		}
	}

	public static void ModifyDrawInfo(ref PlayerDrawSet drawInfo)
	{
		Player player = drawInfo.drawPlayer;
		FilteredSpanEnumerator<ModPlayer> enumerator = HookModifyDrawInfo.Enumerate(player).GetEnumerator();
		while (enumerator.MoveNext())
		{
			ModPlayer modPlayer = enumerator.Current;
			try
			{
				modPlayer.ModifyDrawInfo(ref drawInfo);
			}
			catch
			{
			}
		}
	}

	public static void ModifyDrawLayerOrdering(IDictionary<PlayerDrawLayer, PlayerDrawLayer.Position> positions)
	{
		ReadOnlySpan<ModPlayer> readOnlySpan = HookModifyDrawLayerOrdering.Enumerate();
		for (int i = 0; i < readOnlySpan.Length; i++)
		{
			ModPlayer modPlayer = readOnlySpan[i];
			try
			{
				modPlayer.ModifyDrawLayerOrdering(positions);
			}
			catch
			{
			}
		}
	}

	public static void HideDrawLayers(PlayerDrawSet drawInfo)
	{
		Player player = drawInfo.drawPlayer;
		FilteredSpanEnumerator<ModPlayer> enumerator = HookModifyDrawLayers.Enumerate(player).GetEnumerator();
		while (enumerator.MoveNext())
		{
			ModPlayer modPlayer = enumerator.Current;
			try
			{
				modPlayer.HideDrawLayers(drawInfo);
			}
			catch
			{
			}
		}
	}

	public static void ModifyScreenPosition(Player player)
	{
		FilteredSpanEnumerator<ModPlayer> enumerator = HookModifyScreenPosition.Enumerate(player).GetEnumerator();
		while (enumerator.MoveNext())
		{
			ModPlayer modPlayer = enumerator.Current;
			try
			{
				modPlayer.ModifyScreenPosition();
			}
			catch
			{
			}
		}
	}

	public static void ModifyZoom(Player player, ref float zoom)
	{
		FilteredSpanEnumerator<ModPlayer> enumerator = HookModifyZoom.Enumerate(player).GetEnumerator();
		while (enumerator.MoveNext())
		{
			ModPlayer modPlayer = enumerator.Current;
			try
			{
				modPlayer.ModifyZoom(ref zoom);
			}
			catch
			{
			}
		}
	}

	public static void PlayerConnect(int playerIndex)
	{
		Player player = Main.player[playerIndex];
		FilteredSpanEnumerator<ModPlayer> enumerator = HookPlayerConnect.Enumerate(player).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.PlayerConnect();
		}
	}

	public static void PlayerDisconnect(int playerIndex)
	{
		Player player = Main.player[playerIndex];
		FilteredSpanEnumerator<ModPlayer> enumerator = HookPlayerDisconnect.Enumerate(player).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.PlayerDisconnect();
		}
	}

	public static void OnEnterWorld(int playerIndex)
	{
		Player player = Main.player[playerIndex];
		FilteredSpanEnumerator<ModPlayer> enumerator = HookOnEnterWorld.Enumerate(player).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.OnEnterWorld();
		}
	}

	public static void OnRespawn(Player player)
	{
		FilteredSpanEnumerator<ModPlayer> enumerator = HookOnRespawn.Enumerate(player).GetEnumerator();
		while (enumerator.MoveNext())
		{
			ModPlayer modPlayer = enumerator.Current;
			try
			{
				modPlayer.OnRespawn();
			}
			catch
			{
			}
		}
	}

	public static bool ShiftClickSlot(Player player, Item[] inventory, int context, int slot)
	{
		FilteredSpanEnumerator<ModPlayer> enumerator = HookShiftClickSlot.Enumerate(player).GetEnumerator();
		while (enumerator.MoveNext())
		{
			if (enumerator.Current.ShiftClickSlot(inventory, context, slot))
			{
				return true;
			}
		}
		return false;
	}

	public static bool HoverSlot(Player player, Item[] inventory, int context, int slot)
	{
		FilteredSpanEnumerator<ModPlayer> enumerator = HookHoverSlot.Enumerate(player).GetEnumerator();
		while (enumerator.MoveNext())
		{
			if (enumerator.Current.HoverSlot(inventory, context, slot))
			{
				return true;
			}
		}
		return false;
	}

	public static void PostSellItem(Player player, NPC npc, Item[] shopInventory, Item item)
	{
		FilteredSpanEnumerator<ModPlayer> enumerator = HookPostSellItem.Enumerate(player).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.PostSellItem(npc, shopInventory, item);
		}
	}

	public static bool CanSellItem(Player player, NPC npc, Item[] shopInventory, Item item)
	{
		FilteredSpanEnumerator<ModPlayer> enumerator = HookCanSellItem.Enumerate(player).GetEnumerator();
		while (enumerator.MoveNext())
		{
			if (!enumerator.Current.CanSellItem(npc, shopInventory, item))
			{
				return false;
			}
		}
		return true;
	}

	public static void PostBuyItem(Player player, NPC npc, Item[] shopInventory, Item item)
	{
		FilteredSpanEnumerator<ModPlayer> enumerator = HookPostBuyItem.Enumerate(player).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.PostBuyItem(npc, shopInventory, item);
		}
	}

	public static bool CanBuyItem(Player player, NPC npc, Item[] shopInventory, Item item)
	{
		FilteredSpanEnumerator<ModPlayer> enumerator = HookCanBuyItem.Enumerate(player).GetEnumerator();
		while (enumerator.MoveNext())
		{
			if (!enumerator.Current.CanBuyItem(npc, shopInventory, item))
			{
				return false;
			}
		}
		return true;
	}

	public static bool CanUseItem(Player player, Item item)
	{
		FilteredSpanEnumerator<ModPlayer> enumerator = HookCanUseItem.Enumerate(player).GetEnumerator();
		while (enumerator.MoveNext())
		{
			if (!enumerator.Current.CanUseItem(item))
			{
				return false;
			}
		}
		return true;
	}

	public static bool? CanAutoReuseItem(Player player, Item item)
	{
		bool? flag = null;
		FilteredSpanEnumerator<ModPlayer> enumerator = HookCanAutoReuseItem.Enumerate(player).GetEnumerator();
		while (enumerator.MoveNext())
		{
			bool? allow = enumerator.Current.CanAutoReuseItem(item);
			if (allow.HasValue)
			{
				if (!allow.Value)
				{
					return false;
				}
				flag = true;
			}
		}
		return flag;
	}

	public static bool ModifyNurseHeal(Player player, NPC npc, ref int health, ref bool removeDebuffs, ref string chat)
	{
		FilteredSpanEnumerator<ModPlayer> enumerator = HookModifyNurseHeal.Enumerate(player).GetEnumerator();
		while (enumerator.MoveNext())
		{
			if (!enumerator.Current.ModifyNurseHeal(npc, ref health, ref removeDebuffs, ref chat))
			{
				return false;
			}
		}
		return true;
	}

	public static void ModifyNursePrice(Player player, NPC npc, int health, bool removeDebuffs, ref int price)
	{
		FilteredSpanEnumerator<ModPlayer> enumerator = HookModifyNursePrice.Enumerate(player).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.ModifyNursePrice(npc, health, removeDebuffs, ref price);
		}
	}

	public static void PostNurseHeal(Player player, NPC npc, int health, bool removeDebuffs, int price)
	{
		FilteredSpanEnumerator<ModPlayer> enumerator = HookPostNurseHeal.Enumerate(player).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.PostNurseHeal(npc, health, removeDebuffs, price);
		}
	}

	public static List<Item> GetStartingItems(Player player, IEnumerable<Item> vanillaItems, bool mediumCoreDeath = false)
	{
		Dictionary<string, List<Item>> itemsByMod = new Dictionary<string, List<Item>> { ["Terraria"] = vanillaItems.ToList() };
		FilteredSpanEnumerator<ModPlayer> filteredSpanEnumerator = HookAddStartingItems.Enumerate(player);
		FilteredSpanEnumerator<ModPlayer> enumerator = filteredSpanEnumerator.GetEnumerator();
		while (enumerator.MoveNext())
		{
			ModPlayer modPlayer = enumerator.Current;
			itemsByMod[modPlayer.Mod.Name] = modPlayer.AddStartingItems(mediumCoreDeath).ToList();
		}
		filteredSpanEnumerator = HookModifyStartingInventory.Enumerate(player);
		enumerator = filteredSpanEnumerator.GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.ModifyStartingInventory(itemsByMod, mediumCoreDeath);
		}
		return itemsByMod.OrderBy((KeyValuePair<string, List<Item>> kv) => (!(kv.Key == "Terraria")) ? kv.Key : "").SelectMany((KeyValuePair<string, List<Item>> kv) => kv.Value).ToList();
	}

	public static IEnumerable<(IEnumerable<Item>, ModPlayer.ItemConsumedCallback)> GetModdedCraftingMaterials(Player player)
	{
		foreach (ModPlayer item in HookAddCraftingMaterials.EnumerateSlow(player.modPlayers))
		{
			ModPlayer.ItemConsumedCallback onUsedForCrafting;
			IEnumerable<Item> items = item.AddMaterialsForCrafting(out onUsedForCrafting);
			if (items != null)
			{
				yield return (items, onUsedForCrafting);
			}
		}
	}

	public static bool OnPickup(Player player, Item item)
	{
		FilteredSpanEnumerator<ModPlayer> enumerator = HookOnPickup.Enumerate(player).GetEnumerator();
		while (enumerator.MoveNext())
		{
			if (!enumerator.Current.OnPickup(item))
			{
				return false;
			}
		}
		return true;
	}
}
