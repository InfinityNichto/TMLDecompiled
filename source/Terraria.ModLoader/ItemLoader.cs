using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Prefixes;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader.Core;
using Terraria.ModLoader.IO;
using Terraria.UI;
using Terraria.Utilities;

namespace Terraria.ModLoader;

/// <summary>
/// This serves as the central class from which item-related functions are carried out. It also stores a list of mod items by ID.
/// </summary>
public static class ItemLoader
{
	private delegate void DelegateGetHealLife(Item item, Player player, bool quickHeal, ref int healValue);

	private delegate void DelegateGetHealMana(Item item, Player player, bool quickHeal, ref int healValue);

	private delegate void DelegateModifyManaCost(Item item, Player player, ref float reduce, ref float mult);

	private delegate bool? DelegateCanConsumeBait(Player baiter, Item bait);

	private delegate void DelegateModifyResearchSorting(Item item, ref ContentSamples.CreativeHelper.ItemGroup itemGroup);

	private delegate bool DelegateCanResearch(Item item);

	private delegate void DelegateOnResearched(Item item, bool fullyResearched);

	private delegate void DelegateModifyWeaponDamage(Item item, Player player, ref StatModifier damage);

	private delegate void DelegateModifyWeaponKnockback(Item item, Player player, ref StatModifier knockback);

	private delegate void DelegateModifyWeaponCrit(Item item, Player player, ref float crit);

	private delegate void DelegatePickAmmo(Item weapon, Item ammo, Player player, ref int type, ref float speed, ref StatModifier damage, ref float knockback);

	private delegate void DelegateModifyShootStats(Item item, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockBack);

	private delegate void DelegateUseItemHitbox(Item item, Player player, ref Rectangle hitbox, ref bool noHitbox);

	private delegate void DelegateModifyItemScale(Item item, Player player, ref float scale);

	private delegate void DelegateModifyHitNPC(Item item, Player player, NPC target, ref NPC.HitModifiers modifiers);

	private delegate void DelegateModifyHitPvp(Item item, Player player, Player target, ref Player.HurtModifiers modifiers);

	private delegate void DelegateSetMatch(int armorSlot, int type, bool male, ref int equipSlot, ref bool robes);

	private delegate bool DelegateReforgePrice(Item item, ref int reforgePrice, ref bool canApplyDiscount);

	private delegate void DelegateDrawArmorColor(EquipType type, int slot, Player drawPlayer, float shadow, ref Color color, ref int glowMask, ref Color glowMaskColor);

	private delegate void DelegateArmorArmGlowMask(int slot, Player drawPlayer, float shadow, ref int glowMask, ref Color color);

	private delegate void DelegateVerticalWingSpeeds(Item item, Player player, ref float ascentWhenFalling, ref float ascentWhenRising, ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend);

	private delegate void DelegateHorizontalWingSpeeds(Item item, Player player, ref float speed, ref float acceleration);

	private delegate void DelegateUpdate(Item item, ref float gravity, ref float maxFallSpeed);

	private delegate void DelegateGrabRange(Item item, Player player, ref int grabRange);

	private delegate bool DelegatePreDrawInWorld(Item item, SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI);

	private delegate void DelegateExtractinatorUse(int extractType, int extractinatorBlockType, ref int resultType, ref int resultStack);

	private delegate void DelegateCaughtFishStack(int type, ref int stack);

	private delegate void DelegateAnglerChat(int type, ref string chat, ref string catchLocation);

	private delegate bool DelegatePreDrawTooltip(Item item, ReadOnlyCollection<TooltipLine> lines, ref int x, ref int y);

	private delegate void DelegatePostDrawTooltip(Item item, ReadOnlyCollection<DrawableTooltipLine> lines);

	private delegate bool DelegatePreDrawTooltipLine(Item item, DrawableTooltipLine line, ref int yOffset);

	private delegate void DelegatePostDrawTooltipLine(Item item, DrawableTooltipLine line);

	private static readonly IList<ModItem> items = new List<ModItem>();

	private static readonly List<GlobalHookList<GlobalItem>> hooks = new List<GlobalHookList<GlobalItem>>();

	private static readonly List<GlobalHookList<GlobalItem>> modHooks = new List<GlobalHookList<GlobalItem>>();

	internal static readonly int vanillaQuestFishCount = 41;

	private static GlobalHookList<GlobalItem> HookOnSpawn = AddHook((Expression<Func<GlobalItem, Action<Item, IEntitySource>>>)((GlobalItem g) => g.OnSpawn));

	private static GlobalHookList<GlobalItem> HookOnCreate = AddHook((Expression<Func<GlobalItem, Action<Item, ItemCreationContext>>>)((GlobalItem g) => g.OnCreated));

	private static GlobalHookList<GlobalItem> HookChoosePrefix = AddHook((Expression<Func<GlobalItem, Func<Item, UnifiedRandom, int>>>)((GlobalItem g) => g.ChoosePrefix));

	private static GlobalHookList<GlobalItem> HookPrefixChance = AddHook((Expression<Func<GlobalItem, Func<Item, int, UnifiedRandom, bool?>>>)((GlobalItem g) => g.PrefixChance));

	private static GlobalHookList<GlobalItem> HookAllowPrefix = AddHook((Expression<Func<GlobalItem, Func<Item, int, bool>>>)((GlobalItem g) => g.AllowPrefix));

	private static GlobalHookList<GlobalItem> HookCanUseItem = AddHook((Expression<Func<GlobalItem, Func<Item, Player, bool>>>)((GlobalItem g) => g.CanUseItem));

	private static GlobalHookList<GlobalItem> HookCanAutoReuseItem = AddHook((Expression<Func<GlobalItem, Func<Item, Player, bool?>>>)((GlobalItem g) => g.CanAutoReuseItem));

	private static GlobalHookList<GlobalItem> HookUseStyle = AddHook((Expression<Func<GlobalItem, Action<Item, Player, Rectangle>>>)((GlobalItem g) => g.UseStyle));

	private static GlobalHookList<GlobalItem> HookHoldStyle = AddHook((Expression<Func<GlobalItem, Action<Item, Player, Rectangle>>>)((GlobalItem g) => g.HoldStyle));

	private static GlobalHookList<GlobalItem> HookHoldItem = AddHook((Expression<Func<GlobalItem, Action<Item, Player>>>)((GlobalItem g) => g.HoldItem));

	private static GlobalHookList<GlobalItem> HookUseTimeMultiplier = AddHook((Expression<Func<GlobalItem, Func<Item, Player, float>>>)((GlobalItem g) => g.UseTimeMultiplier));

	private static GlobalHookList<GlobalItem> HookUseAnimationMultiplier = AddHook((Expression<Func<GlobalItem, Func<Item, Player, float>>>)((GlobalItem g) => g.UseAnimationMultiplier));

	private static GlobalHookList<GlobalItem> HookUseSpeedMultiplier = AddHook((Expression<Func<GlobalItem, Func<Item, Player, float>>>)((GlobalItem g) => g.UseSpeedMultiplier));

	private static GlobalHookList<GlobalItem> HookGetHealLife = AddHook((Expression<Func<GlobalItem, DelegateGetHealLife>>)((GlobalItem g) => g.GetHealLife));

	private static GlobalHookList<GlobalItem> HookGetHealMana = AddHook((Expression<Func<GlobalItem, DelegateGetHealMana>>)((GlobalItem g) => g.GetHealMana));

	private static GlobalHookList<GlobalItem> HookModifyManaCost = AddHook((Expression<Func<GlobalItem, DelegateModifyManaCost>>)((GlobalItem g) => g.ModifyManaCost));

	private static GlobalHookList<GlobalItem> HookOnMissingMana = AddHook((Expression<Func<GlobalItem, Action<Item, Player, int>>>)((GlobalItem g) => g.OnMissingMana));

	private static GlobalHookList<GlobalItem> HookOnConsumeMana = AddHook((Expression<Func<GlobalItem, Action<Item, Player, int>>>)((GlobalItem g) => g.OnConsumeMana));

	private static GlobalHookList<GlobalItem> HookCanConsumeBait = AddHook((Expression<Func<GlobalItem, DelegateCanConsumeBait>>)((GlobalItem g) => g.CanConsumeBait));

	private static GlobalHookList<GlobalItem> HookModifyResearchSorting = AddHook((Expression<Func<GlobalItem, DelegateModifyResearchSorting>>)((GlobalItem g) => g.ModifyResearchSorting));

	private static GlobalHookList<GlobalItem> HookCanResearch = AddHook((Expression<Func<GlobalItem, DelegateCanResearch>>)((GlobalItem g) => g.CanResearch));

	private static GlobalHookList<GlobalItem> HookOnResearched = AddHook((Expression<Func<GlobalItem, DelegateOnResearched>>)((GlobalItem g) => g.OnResearched));

	private static GlobalHookList<GlobalItem> HookModifyWeaponDamage = AddHook((Expression<Func<GlobalItem, DelegateModifyWeaponDamage>>)((GlobalItem g) => g.ModifyWeaponDamage));

	private static GlobalHookList<GlobalItem> HookModifyWeaponKnockback = AddHook((Expression<Func<GlobalItem, DelegateModifyWeaponKnockback>>)((GlobalItem g) => g.ModifyWeaponKnockback));

	private static GlobalHookList<GlobalItem> HookModifyWeaponCrit = AddHook((Expression<Func<GlobalItem, DelegateModifyWeaponCrit>>)((GlobalItem g) => g.ModifyWeaponCrit));

	private static GlobalHookList<GlobalItem> HookNeedsAmmo = AddHook((Expression<Func<GlobalItem, Func<Item, Player, bool>>>)((GlobalItem g) => g.NeedsAmmo));

	private static GlobalHookList<GlobalItem> HookPickAmmo = AddHook((Expression<Func<GlobalItem, DelegatePickAmmo>>)((GlobalItem g) => g.PickAmmo));

	private static GlobalHookList<GlobalItem> HookCanChooseAmmo = AddHook((Expression<Func<GlobalItem, Func<Item, Item, Player, bool?>>>)((GlobalItem g) => g.CanChooseAmmo));

	private static GlobalHookList<GlobalItem> HookCanBeChosenAsAmmo = AddHook((Expression<Func<GlobalItem, Func<Item, Item, Player, bool?>>>)((GlobalItem g) => g.CanBeChosenAsAmmo));

	private static GlobalHookList<GlobalItem> HookCanConsumeAmmo = AddHook((Expression<Func<GlobalItem, Func<Item, Item, Player, bool>>>)((GlobalItem g) => g.CanConsumeAmmo));

	private static GlobalHookList<GlobalItem> HookCanBeConsumedAsAmmo = AddHook((Expression<Func<GlobalItem, Func<Item, Item, Player, bool>>>)((GlobalItem g) => g.CanBeConsumedAsAmmo));

	private static GlobalHookList<GlobalItem> HookOnConsumeAmmo = AddHook((Expression<Func<GlobalItem, Action<Item, Item, Player>>>)((GlobalItem g) => g.OnConsumeAmmo));

	private static GlobalHookList<GlobalItem> HookOnConsumedAsAmmo = AddHook((Expression<Func<GlobalItem, Action<Item, Item, Player>>>)((GlobalItem g) => g.OnConsumedAsAmmo));

	private static GlobalHookList<GlobalItem> HookCanShoot = AddHook((Expression<Func<GlobalItem, Func<Item, Player, bool>>>)((GlobalItem g) => g.CanShoot));

	private static GlobalHookList<GlobalItem> HookModifyShootStats = AddHook((Expression<Func<GlobalItem, DelegateModifyShootStats>>)((GlobalItem g) => g.ModifyShootStats));

	private static GlobalHookList<GlobalItem> HookShoot = AddHook((Expression<Func<GlobalItem, Func<Item, Player, EntitySource_ItemUse_WithAmmo, Vector2, Vector2, int, int, float, bool>>>)((GlobalItem g) => g.Shoot));

	private static GlobalHookList<GlobalItem> HookUseItemHitbox = AddHook((Expression<Func<GlobalItem, DelegateUseItemHitbox>>)((GlobalItem g) => g.UseItemHitbox));

	private static GlobalHookList<GlobalItem> HookMeleeEffects = AddHook((Expression<Func<GlobalItem, Action<Item, Player, Rectangle>>>)((GlobalItem g) => g.MeleeEffects));

	private static GlobalHookList<GlobalItem> HookCanCatchNPC = AddHook((Expression<Func<GlobalItem, Func<Item, NPC, Player, bool?>>>)((GlobalItem g) => g.CanCatchNPC));

	private static GlobalHookList<GlobalItem> HookOnCatchNPC = AddHook((Expression<Func<GlobalItem, Action<Item, NPC, Player, bool>>>)((GlobalItem g) => g.OnCatchNPC));

	private static GlobalHookList<GlobalItem> HookModifyItemScale = AddHook((Expression<Func<GlobalItem, DelegateModifyItemScale>>)((GlobalItem g) => g.ModifyItemScale));

	private static GlobalHookList<GlobalItem> HookCanHitNPC = AddHook((Expression<Func<GlobalItem, Func<Item, Player, NPC, bool?>>>)((GlobalItem g) => g.CanHitNPC));

	private static GlobalHookList<GlobalItem> HookCanCollideNPC = AddHook((Expression<Func<GlobalItem, Func<Item, Rectangle, Player, NPC, bool?>>>)((GlobalItem g) => g.CanMeleeAttackCollideWithNPC));

	private static GlobalHookList<GlobalItem> HookModifyHitNPC = AddHook((Expression<Func<GlobalItem, DelegateModifyHitNPC>>)((GlobalItem g) => g.ModifyHitNPC));

	private static GlobalHookList<GlobalItem> HookOnHitNPC = AddHook((Expression<Func<GlobalItem, Action<Item, Player, NPC, NPC.HitInfo, int>>>)((GlobalItem g) => g.OnHitNPC));

	private static GlobalHookList<GlobalItem> HookCanHitPvp = AddHook((Expression<Func<GlobalItem, Func<Item, Player, Player, bool>>>)((GlobalItem g) => g.CanHitPvp));

	private static GlobalHookList<GlobalItem> HookModifyHitPvp = AddHook((Expression<Func<GlobalItem, DelegateModifyHitPvp>>)((GlobalItem g) => g.ModifyHitPvp));

	private static GlobalHookList<GlobalItem> HookOnHitPvp = AddHook((Expression<Func<GlobalItem, Action<Item, Player, Player, Player.HurtInfo>>>)((GlobalItem g) => g.OnHitPvp));

	private static GlobalHookList<GlobalItem> HookUseItem = AddHook((Expression<Func<GlobalItem, Func<Item, Player, bool?>>>)((GlobalItem g) => g.UseItem));

	private static GlobalHookList<GlobalItem> HookUseAnimation = AddHook((Expression<Func<GlobalItem, Action<Item, Player>>>)((GlobalItem g) => g.UseAnimation));

	private static GlobalHookList<GlobalItem> HookConsumeItem = AddHook((Expression<Func<GlobalItem, Func<Item, Player, bool>>>)((GlobalItem g) => g.ConsumeItem));

	private static GlobalHookList<GlobalItem> HookOnConsumeItem = AddHook((Expression<Func<GlobalItem, Action<Item, Player>>>)((GlobalItem g) => g.OnConsumeItem));

	private static GlobalHookList<GlobalItem> HookUseItemFrame = AddHook((Expression<Func<GlobalItem, Action<Item, Player>>>)((GlobalItem g) => g.UseItemFrame));

	private static GlobalHookList<GlobalItem> HookHoldItemFrame = AddHook((Expression<Func<GlobalItem, Action<Item, Player>>>)((GlobalItem g) => g.HoldItemFrame));

	private static GlobalHookList<GlobalItem> HookAltFunctionUse = AddHook((Expression<Func<GlobalItem, Func<Item, Player, bool>>>)((GlobalItem g) => g.AltFunctionUse));

	private static GlobalHookList<GlobalItem> HookUpdateInventory = AddHook((Expression<Func<GlobalItem, Action<Item, Player>>>)((GlobalItem g) => g.UpdateInventory));

	private static GlobalHookList<GlobalItem> HookUpdateInfoAccessory = AddHook((Expression<Func<GlobalItem, Action<Item, Player>>>)((GlobalItem g) => g.UpdateInfoAccessory));

	private static GlobalHookList<GlobalItem> HookUpdateEquip = AddHook((Expression<Func<GlobalItem, Action<Item, Player>>>)((GlobalItem g) => g.UpdateEquip));

	private static GlobalHookList<GlobalItem> HookUpdateAccessory = AddHook((Expression<Func<GlobalItem, Action<Item, Player, bool>>>)((GlobalItem g) => g.UpdateAccessory));

	private static GlobalHookList<GlobalItem> HookUpdateVanity = AddHook((Expression<Func<GlobalItem, Action<Item, Player>>>)((GlobalItem g) => g.UpdateVanity));

	private static GlobalHookList<GlobalItem> HookUpdateArmorSet = AddHook((Expression<Func<GlobalItem, Action<Player, string>>>)((GlobalItem g) => g.UpdateArmorSet));

	private static GlobalHookList<GlobalItem> HookPreUpdateVanitySet = AddHook((Expression<Func<GlobalItem, Action<Player, string>>>)((GlobalItem g) => g.PreUpdateVanitySet));

	private static GlobalHookList<GlobalItem> HookUpdateVanitySet = AddHook((Expression<Func<GlobalItem, Action<Player, string>>>)((GlobalItem g) => g.UpdateVanitySet));

	private static GlobalHookList<GlobalItem> HookArmorSetShadows = AddHook((Expression<Func<GlobalItem, Action<Player, string>>>)((GlobalItem g) => g.ArmorSetShadows));

	private static GlobalHookList<GlobalItem> HookSetMatch = AddHook((Expression<Func<GlobalItem, DelegateSetMatch>>)((GlobalItem g) => g.SetMatch));

	private static GlobalHookList<GlobalItem> HookCanRightClick = AddHook((Expression<Func<GlobalItem, Func<Item, bool>>>)((GlobalItem g) => g.CanRightClick));

	private static GlobalHookList<GlobalItem> HookRightClick = AddHook((Expression<Func<GlobalItem, Action<Item, Player>>>)((GlobalItem g) => g.RightClick));

	private static GlobalHookList<GlobalItem> HookModifyItemLoot = AddHook((Expression<Func<GlobalItem, Action<Item, ItemLoot>>>)((GlobalItem g) => g.ModifyItemLoot));

	private static GlobalHookList<GlobalItem> HookCanStack = AddHook((Expression<Func<GlobalItem, Func<Item, Item, bool>>>)((GlobalItem g) => g.CanStack));

	private static GlobalHookList<GlobalItem> HookCanStackInWorld = AddHook((Expression<Func<GlobalItem, Func<Item, Item, bool>>>)((GlobalItem g) => g.CanStackInWorld));

	private static GlobalHookList<GlobalItem> HookOnStack = AddHook((Expression<Func<GlobalItem, Action<Item, Item, int>>>)((GlobalItem g) => g.OnStack));

	private static GlobalHookList<GlobalItem> HookSplitStack = AddHook((Expression<Func<GlobalItem, Action<Item, Item, int>>>)((GlobalItem g) => g.SplitStack));

	private static GlobalHookList<GlobalItem> HookReforgePrice = AddHook((Expression<Func<GlobalItem, DelegateReforgePrice>>)((GlobalItem g) => g.ReforgePrice));

	private static GlobalHookList<GlobalItem> HookCanReforge = AddHook((Expression<Func<GlobalItem, Func<Item, bool>>>)((GlobalItem g) => g.CanReforge));

	private static GlobalHookList<GlobalItem> HookPreReforge = AddHook((Expression<Func<GlobalItem, Action<Item>>>)((GlobalItem g) => g.PreReforge));

	private static GlobalHookList<GlobalItem> HookPostReforge = AddHook((Expression<Func<GlobalItem, Action<Item>>>)((GlobalItem g) => g.PostReforge));

	private static GlobalHookList<GlobalItem> HookDrawArmorColor = AddHook((Expression<Func<GlobalItem, DelegateDrawArmorColor>>)((GlobalItem g) => g.DrawArmorColor));

	private static GlobalHookList<GlobalItem> HookArmorArmGlowMask = AddHook((Expression<Func<GlobalItem, DelegateArmorArmGlowMask>>)((GlobalItem g) => g.ArmorArmGlowMask));

	private static GlobalHookList<GlobalItem> HookVerticalWingSpeeds = AddHook((Expression<Func<GlobalItem, DelegateVerticalWingSpeeds>>)((GlobalItem g) => g.VerticalWingSpeeds));

	private static GlobalHookList<GlobalItem> HookHorizontalWingSpeeds = AddHook((Expression<Func<GlobalItem, DelegateHorizontalWingSpeeds>>)((GlobalItem g) => g.HorizontalWingSpeeds));

	private static GlobalHookList<GlobalItem> HookWingUpdate = AddHook((Expression<Func<GlobalItem, Func<int, Player, bool, bool>>>)((GlobalItem g) => g.WingUpdate));

	private static GlobalHookList<GlobalItem> HookUpdate = AddHook((Expression<Func<GlobalItem, DelegateUpdate>>)((GlobalItem g) => g.Update));

	private static GlobalHookList<GlobalItem> HookPostUpdate = AddHook((Expression<Func<GlobalItem, Action<Item>>>)((GlobalItem g) => g.PostUpdate));

	private static GlobalHookList<GlobalItem> HookGrabRange = AddHook((Expression<Func<GlobalItem, DelegateGrabRange>>)((GlobalItem g) => g.GrabRange));

	private static GlobalHookList<GlobalItem> HookGrabStyle = AddHook((Expression<Func<GlobalItem, Func<Item, Player, bool>>>)((GlobalItem g) => g.GrabStyle));

	private static GlobalHookList<GlobalItem> HookCanPickup = AddHook((Expression<Func<GlobalItem, Func<Item, Player, bool>>>)((GlobalItem g) => g.CanPickup));

	private static GlobalHookList<GlobalItem> HookOnPickup = AddHook((Expression<Func<GlobalItem, Func<Item, Player, bool>>>)((GlobalItem g) => g.OnPickup));

	private static GlobalHookList<GlobalItem> HookItemSpace = AddHook((Expression<Func<GlobalItem, Func<Item, Player, bool>>>)((GlobalItem g) => g.ItemSpace));

	private static GlobalHookList<GlobalItem> HookGetAlpha = AddHook((Expression<Func<GlobalItem, Func<Item, Color, Color?>>>)((GlobalItem g) => g.GetAlpha));

	private static GlobalHookList<GlobalItem> HookPreDrawInWorld = AddHook((Expression<Func<GlobalItem, DelegatePreDrawInWorld>>)((GlobalItem g) => g.PreDrawInWorld));

	private static GlobalHookList<GlobalItem> HookPostDrawInWorld = AddHook((Expression<Func<GlobalItem, Action<Item, SpriteBatch, Color, Color, float, float, int>>>)((GlobalItem g) => g.PostDrawInWorld));

	private static GlobalHookList<GlobalItem> HookPreDrawInInventory = AddHook((Expression<Func<GlobalItem, Func<Item, SpriteBatch, Vector2, Rectangle, Color, Color, Vector2, float, bool>>>)((GlobalItem g) => g.PreDrawInInventory));

	private static GlobalHookList<GlobalItem> HookPostDrawInInventory = AddHook((Expression<Func<GlobalItem, Action<Item, SpriteBatch, Vector2, Rectangle, Color, Color, Vector2, float>>>)((GlobalItem g) => g.PostDrawInInventory));

	private static GlobalHookList<GlobalItem> HookHoldoutOffset = AddHook((Expression<Func<GlobalItem, Func<int, Vector2?>>>)((GlobalItem g) => g.HoldoutOffset));

	private static GlobalHookList<GlobalItem> HookHoldoutOrigin = AddHook((Expression<Func<GlobalItem, Func<int, Vector2?>>>)((GlobalItem g) => g.HoldoutOrigin));

	private static GlobalHookList<GlobalItem> HookCanEquipAccessory = AddHook((Expression<Func<GlobalItem, Func<Item, Player, int, bool, bool>>>)((GlobalItem g) => g.CanEquipAccessory));

	private static GlobalHookList<GlobalItem> HookCanAccessoryBeEquippedWith = AddHook((Expression<Func<GlobalItem, Func<Item, Item, Player, bool>>>)((GlobalItem g) => g.CanAccessoryBeEquippedWith));

	private static GlobalHookList<GlobalItem> HookExtractinatorUse = AddHook((Expression<Func<GlobalItem, DelegateExtractinatorUse>>)((GlobalItem g) => g.ExtractinatorUse));

	private static GlobalHookList<GlobalItem> HookCaughtFishStack = AddHook((Expression<Func<GlobalItem, DelegateCaughtFishStack>>)((GlobalItem g) => g.CaughtFishStack));

	private static GlobalHookList<GlobalItem> HookIsAnglerQuestAvailable = AddHook((Expression<Func<GlobalItem, Func<int, bool>>>)((GlobalItem g) => g.IsAnglerQuestAvailable));

	private static GlobalHookList<GlobalItem> HookAnglerChat = AddHook((Expression<Func<GlobalItem, DelegateAnglerChat>>)((GlobalItem g) => g.AnglerChat));

	private static GlobalHookList<GlobalItem> HookPreDrawTooltip = AddHook((Expression<Func<GlobalItem, DelegatePreDrawTooltip>>)((GlobalItem g) => g.PreDrawTooltip));

	private static GlobalHookList<GlobalItem> HookPostDrawTooltip = AddHook((Expression<Func<GlobalItem, DelegatePostDrawTooltip>>)((GlobalItem g) => g.PostDrawTooltip));

	private static GlobalHookList<GlobalItem> HookPreDrawTooltipLine = AddHook((Expression<Func<GlobalItem, DelegatePreDrawTooltipLine>>)((GlobalItem g) => g.PreDrawTooltipLine));

	private static GlobalHookList<GlobalItem> HookPostDrawTooltipLine = AddHook((Expression<Func<GlobalItem, DelegatePostDrawTooltipLine>>)((GlobalItem g) => g.PostDrawTooltipLine));

	private static GlobalHookList<GlobalItem> HookModifyTooltips = AddHook((Expression<Func<GlobalItem, Action<Item, List<TooltipLine>>>>)((GlobalItem g) => g.ModifyTooltips));

	internal static GlobalHookList<GlobalItem> HookSaveData = AddHook((Expression<Func<GlobalItem, Action<Item, TagCompound>>>)((GlobalItem g) => g.SaveData));

	internal static GlobalHookList<GlobalItem> HookNetSend = AddHook((Expression<Func<GlobalItem, Action<Item, BinaryWriter>>>)((GlobalItem g) => g.NetSend));

	internal static GlobalHookList<GlobalItem> HookNetReceive = AddHook((Expression<Func<GlobalItem, Action<Item, BinaryReader>>>)((GlobalItem g) => g.NetReceive));

	public static int ItemCount { get; private set; } = ItemID.Count;


	private static GlobalHookList<GlobalItem> AddHook<F>(Expression<Func<GlobalItem, F>> func) where F : Delegate
	{
		GlobalHookList<GlobalItem> hook = GlobalHookList<GlobalItem>.Create(func);
		hooks.Add(hook);
		return hook;
	}

	public static T AddModHook<T>(T hook) where T : GlobalHookList<GlobalItem>
	{
		modHooks.Add(hook);
		return hook;
	}

	internal static int Register(ModItem item)
	{
		items.Add(item);
		return ItemCount++;
	}

	/// <summary>
	/// Gets the ModItem template instance corresponding to the specified type (not the clone/new instance which gets added to Items as the game is played). Returns null if no modded item has the given type.
	/// </summary>
	public static ModItem GetItem(int type)
	{
		if (type < ItemID.Count || type >= ItemCount)
		{
			return null;
		}
		return items[type - ItemID.Count];
	}

	internal static void ResizeArrays(bool unloading)
	{
		if (!unloading)
		{
			GlobalList<GlobalItem>.FinishLoading(ItemCount);
		}
		Array.Resize(ref TextureAssets.Item, ItemCount);
		Array.Resize(ref TextureAssets.ItemFlame, ItemCount);
		LoaderUtils.ResetStaticMembers(typeof(ItemID));
		LoaderUtils.ResetStaticMembers(typeof(AmmoID));
		LoaderUtils.ResetStaticMembers(typeof(PrefixLegacy.ItemSets));
		Array.Resize(ref Item.cachedItemSpawnsByType, ItemCount);
		Array.Resize(ref Item.staff, ItemCount);
		Array.Resize(ref Item.claw, ItemCount);
		Array.Resize(ref Lang._itemNameCache, ItemCount);
		Array.Resize(ref Lang._itemTooltipCache, ItemCount);
		Array.Resize(ref RecipeLoader.FirstRecipeForItem, ItemCount);
		for (int i = ItemID.Count; i < ItemCount; i++)
		{
			Lang._itemNameCache[i] = LocalizedText.Empty;
			Lang._itemTooltipCache[i] = ItemTooltip.None;
			Item.cachedItemSpawnsByType[i] = -1;
		}
		lock (Main.itemAnimationsRegistered)
		{
			Array.Resize(ref Main.itemAnimations, ItemCount);
			Main.InitializeItemAnimations();
		}
		if (unloading)
		{
			Array.Resize(ref Main.anglerQuestItemNetIDs, vanillaQuestFishCount);
			return;
		}
		Main.anglerQuestItemNetIDs = Main.anglerQuestItemNetIDs.Concat(from modItem in items
			where modItem.IsQuestFish()
			select modItem.Type).ToArray();
	}

	internal static void FinishSetup()
	{
		GlobalLoaderUtils<GlobalItem, Item>.BuildTypeLookups(new Item().SetDefaults);
		UpdateHookLists();
		GlobalTypeLookups<GlobalItem>.LogStats();
		foreach (ModItem item in items)
		{
			Lang._itemNameCache[item.Type] = item.DisplayName;
			Lang._itemTooltipCache[item.Type] = ItemTooltip.FromLocalization(item.Tooltip);
			ContentSamples.ItemsByType[item.Type].RebuildTooltip();
		}
		ValidateDropsSet();
	}

	private static void UpdateHookLists()
	{
		foreach (GlobalHookList<GlobalItem> item in hooks.Union(modHooks))
		{
			item.Update();
		}
	}

	internal static void ValidateDropsSet()
	{
		foreach (KeyValuePair<int, (int, int)> pair2 in ItemID.Sets.GeodeDrops)
		{
			string exceptionCommon2 = Lang.GetItemNameValue(pair2.Key) + " registered in 'ItemID.Sets.GeodeDrops'";
			if (pair2.Value.Item1 < 1)
			{
				throw new Exception(exceptionCommon2 + " must have minStack bigger than 0");
			}
			if (pair2.Value.Item2 <= pair2.Value.Item1)
			{
				throw new Exception(exceptionCommon2 + " must have maxStack bigger than minStack");
			}
		}
		foreach (KeyValuePair<int, (int, int)> pair in ItemID.Sets.OreDropsFromSlime)
		{
			string exceptionCommon = Lang.GetItemNameValue(pair.Key) + " registered in 'ItemID.Sets.OreDropsFromSlime'";
			if (pair.Value.Item1 < 1)
			{
				throw new Exception(exceptionCommon + " must have minStack bigger than 0");
			}
			if (pair.Value.Item2 < pair.Value.Item1)
			{
				throw new Exception(exceptionCommon + " must have maxStack bigger than or equal to minStack");
			}
		}
	}

	internal static void Unload()
	{
		ItemCount = ItemID.Count;
		items.Clear();
		FlexibleTileWand.Reload();
		GlobalList<GlobalItem>.Reset();
		modHooks.Clear();
		UpdateHookLists();
	}

	internal static bool IsModItem(int index)
	{
		return index >= ItemID.Count;
	}

	internal static bool MeleePrefix(Item item)
	{
		if (item.ModItem != null)
		{
			return item.ModItem.MeleePrefix();
		}
		return false;
	}

	internal static bool WeaponPrefix(Item item)
	{
		if (item.ModItem != null)
		{
			return item.ModItem.WeaponPrefix();
		}
		return false;
	}

	internal static bool RangedPrefix(Item item)
	{
		if (item.ModItem != null)
		{
			return item.ModItem.RangedPrefix();
		}
		return false;
	}

	internal static bool MagicPrefix(Item item)
	{
		if (item.ModItem != null)
		{
			return item.ModItem.MagicPrefix();
		}
		return false;
	}

	internal static void SetDefaults(Item item, bool createModItem = true)
	{
		if (IsModItem(item.type) && createModItem)
		{
			item.ModItem = GetItem(item.type).NewInstance(item);
		}
		GlobalLoaderUtils<GlobalItem, Item>.SetDefaults(item, ref item._globals, delegate(Item i)
		{
			i.ModItem?.AutoDefaults();
			i.ModItem?.SetDefaults();
		});
	}

	internal static void OnSpawn(Item item, IEntitySource source)
	{
		item.ModItem?.OnSpawn(source);
		EntityGlobalsEnumerator<GlobalItem> enumerator = HookOnSpawn.Enumerate(item).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.OnSpawn(item, source);
		}
	}

	public static void OnCreated(Item item, ItemCreationContext context)
	{
		EntityGlobalsEnumerator<GlobalItem> enumerator = HookOnCreate.Enumerate(item).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.OnCreated(item, context);
		}
		item.ModItem?.OnCreated(context);
	}

	public static int ChoosePrefix(Item item, UnifiedRandom rand)
	{
		EntityGlobalsEnumerator<GlobalItem> enumerator = HookChoosePrefix.Enumerate(item).GetEnumerator();
		while (enumerator.MoveNext())
		{
			int pre2 = enumerator.Current.ChoosePrefix(item, rand);
			if (pre2 > 0)
			{
				return pre2;
			}
		}
		if (item.ModItem != null)
		{
			int pre = item.ModItem.ChoosePrefix(rand);
			if (pre > 0)
			{
				return pre;
			}
		}
		return -1;
	}

	/// <summary>
	/// Allows for blocking, forcing and altering chance of prefix rolling.
	/// False (block) takes precedence over True (force).
	/// Null gives vanilla behavior
	/// </summary>
	public static bool? PrefixChance(Item item, int pre, UnifiedRandom rand)
	{
		bool? result = null;
		EntityGlobalsEnumerator<GlobalItem> enumerator = HookPrefixChance.Enumerate(item).GetEnumerator();
		while (enumerator.MoveNext())
		{
			bool? r2 = enumerator.Current.PrefixChance(item, pre, rand);
			if (r2.HasValue)
			{
				result = r2.Value && (result ?? true);
			}
		}
		if (item.ModItem != null)
		{
			bool? r = item.ModItem.PrefixChance(pre, rand);
			if (r.HasValue)
			{
				result = r.Value && (result ?? true);
			}
		}
		return result;
	}

	public static bool AllowPrefix(Item item, int pre)
	{
		bool result = true;
		EntityGlobalsEnumerator<GlobalItem> enumerator = HookAllowPrefix.Enumerate(item).GetEnumerator();
		while (enumerator.MoveNext())
		{
			GlobalItem g = enumerator.Current;
			result &= g.AllowPrefix(item, pre);
		}
		if (item.ModItem != null)
		{
			result &= item.ModItem.AllowPrefix(pre);
		}
		return result;
	}

	public static bool CanUseItem(Item item, Player player)
	{
		if (item.ModItem != null && !item.ModItem.CanUseItem(player))
		{
			return false;
		}
		EntityGlobalsEnumerator<GlobalItem> enumerator = HookCanUseItem.Enumerate(item).GetEnumerator();
		while (enumerator.MoveNext())
		{
			if (!enumerator.Current.CanUseItem(item, player))
			{
				return false;
			}
		}
		return true;
	}

	public static bool? CanAutoReuseItem(Item item, Player player)
	{
		bool? flag = null;
		EntityGlobalsEnumerator<GlobalItem> enumerator = HookCanAutoReuseItem.Enumerate(item).GetEnumerator();
		while (enumerator.MoveNext())
		{
			bool? allow2 = enumerator.Current.CanAutoReuseItem(item, player);
			if (allow2.HasValue)
			{
				if (!allow2.Value)
				{
					return false;
				}
				flag = true;
			}
		}
		if (item.ModItem != null)
		{
			bool? allow = item.ModItem.CanAutoReuseItem(player);
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

	/// <summary>
	/// Calls ModItem.UseStyle and all GlobalItem.UseStyle hooks.
	/// </summary>
	public static void UseStyle(Item item, Player player, Rectangle heldItemFrame)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		if (!item.IsAir)
		{
			item.ModItem?.UseStyle(player, heldItemFrame);
			EntityGlobalsEnumerator<GlobalItem> enumerator = HookUseStyle.Enumerate(item).GetEnumerator();
			while (enumerator.MoveNext())
			{
				enumerator.Current.UseStyle(item, player, heldItemFrame);
			}
		}
	}

	/// <summary>
	/// If the player is not holding onto a rope and is not in the middle of using an item, calls ModItem.HoldStyle and all GlobalItem.HoldStyle hooks.
	/// <br /> Returns whether or not the vanilla logic should be skipped.
	/// </summary>
	public static void HoldStyle(Item item, Player player, Rectangle heldItemFrame)
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		if (!item.IsAir && !player.pulley && !player.ItemAnimationActive)
		{
			item.ModItem?.HoldStyle(player, heldItemFrame);
			EntityGlobalsEnumerator<GlobalItem> enumerator = HookHoldStyle.Enumerate(item).GetEnumerator();
			while (enumerator.MoveNext())
			{
				enumerator.Current.HoldStyle(item, player, heldItemFrame);
			}
		}
	}

	/// <summary>
	/// Calls ModItem.HoldItem and all GlobalItem.HoldItem hooks.
	/// </summary>
	public static void HoldItem(Item item, Player player)
	{
		if (!item.IsAir)
		{
			item.ModItem?.HoldItem(player);
			EntityGlobalsEnumerator<GlobalItem> enumerator = HookHoldItem.Enumerate(item).GetEnumerator();
			while (enumerator.MoveNext())
			{
				enumerator.Current.HoldItem(item, player);
			}
		}
	}

	public static float UseTimeMultiplier(Item item, Player player)
	{
		if (item.IsAir)
		{
			return 1f;
		}
		float multiplier = item.ModItem?.UseTimeMultiplier(player) ?? 1f;
		EntityGlobalsEnumerator<GlobalItem> enumerator = HookUseTimeMultiplier.Enumerate(item).GetEnumerator();
		while (enumerator.MoveNext())
		{
			GlobalItem g = enumerator.Current;
			multiplier *= g.UseTimeMultiplier(item, player);
		}
		return multiplier;
	}

	public static float UseAnimationMultiplier(Item item, Player player)
	{
		if (item.IsAir)
		{
			return 1f;
		}
		float multiplier = item.ModItem?.UseAnimationMultiplier(player) ?? 1f;
		EntityGlobalsEnumerator<GlobalItem> enumerator = HookUseAnimationMultiplier.Enumerate(item).GetEnumerator();
		while (enumerator.MoveNext())
		{
			GlobalItem g = enumerator.Current;
			multiplier *= g.UseAnimationMultiplier(item, player);
		}
		return multiplier;
	}

	public static float UseSpeedMultiplier(Item item, Player player)
	{
		if (item.IsAir)
		{
			return 1f;
		}
		float multiplier = item.ModItem?.UseSpeedMultiplier(player) ?? 1f;
		EntityGlobalsEnumerator<GlobalItem> enumerator = HookUseSpeedMultiplier.Enumerate(item).GetEnumerator();
		while (enumerator.MoveNext())
		{
			GlobalItem g = enumerator.Current;
			multiplier *= g.UseSpeedMultiplier(item, player);
		}
		return multiplier;
	}

	/// <summary>
	/// Calls ModItem.GetHealLife, then all GlobalItem.GetHealLife hooks.
	/// </summary>
	public static void GetHealLife(Item item, Player player, bool quickHeal, ref int healValue)
	{
		if (!item.IsAir)
		{
			item.ModItem?.GetHealLife(player, quickHeal, ref healValue);
			EntityGlobalsEnumerator<GlobalItem> enumerator = HookGetHealLife.Enumerate(item).GetEnumerator();
			while (enumerator.MoveNext())
			{
				enumerator.Current.GetHealLife(item, player, quickHeal, ref healValue);
			}
		}
	}

	/// <summary>
	/// Calls ModItem.GetHealMana, then all GlobalItem.GetHealMana hooks.
	/// </summary>
	public static void GetHealMana(Item item, Player player, bool quickHeal, ref int healValue)
	{
		if (!item.IsAir)
		{
			item.ModItem?.GetHealMana(player, quickHeal, ref healValue);
			EntityGlobalsEnumerator<GlobalItem> enumerator = HookGetHealMana.Enumerate(item).GetEnumerator();
			while (enumerator.MoveNext())
			{
				enumerator.Current.GetHealMana(item, player, quickHeal, ref healValue);
			}
		}
	}

	/// <summary>
	/// Calls ModItem.ModifyManaCost, then all GlobalItem.ModifyManaCost hooks.
	/// </summary>
	public static void ModifyManaCost(Item item, Player player, ref float reduce, ref float mult)
	{
		if (!item.IsAir)
		{
			item.ModItem?.ModifyManaCost(player, ref reduce, ref mult);
			EntityGlobalsEnumerator<GlobalItem> enumerator = HookModifyManaCost.Enumerate(item).GetEnumerator();
			while (enumerator.MoveNext())
			{
				enumerator.Current.ModifyManaCost(item, player, ref reduce, ref mult);
			}
		}
	}

	/// <summary>
	/// Calls ModItem.OnMissingMana, then all GlobalItem.OnMissingMana hooks.
	/// </summary>
	public static void OnMissingMana(Item item, Player player, int neededMana)
	{
		if (!item.IsAir)
		{
			item.ModItem?.OnMissingMana(player, neededMana);
			EntityGlobalsEnumerator<GlobalItem> enumerator = HookOnMissingMana.Enumerate(item).GetEnumerator();
			while (enumerator.MoveNext())
			{
				enumerator.Current.OnMissingMana(item, player, neededMana);
			}
		}
	}

	/// <summary>
	/// Calls ModItem.OnConsumeMana, then all GlobalItem.OnConsumeMana hooks.
	/// </summary>
	public static void OnConsumeMana(Item item, Player player, int manaConsumed)
	{
		if (!item.IsAir)
		{
			item.ModItem?.OnConsumeMana(player, manaConsumed);
			EntityGlobalsEnumerator<GlobalItem> enumerator = HookOnConsumeMana.Enumerate(item).GetEnumerator();
			while (enumerator.MoveNext())
			{
				enumerator.Current.OnConsumeMana(item, player, manaConsumed);
			}
		}
	}

	public static bool? CanConsumeBait(Player player, Item bait)
	{
		bool? ret = bait.ModItem?.CanConsumeBait(player);
		EntityGlobalsEnumerator<GlobalItem> enumerator = HookCanConsumeBait.Enumerate(bait).GetEnumerator();
		while (enumerator.MoveNext())
		{
			bool? flag = enumerator.Current.CanConsumeBait(player, bait);
			if (flag.HasValue)
			{
				bool b = flag.GetValueOrDefault();
				ret = (ret ?? true) && b;
			}
		}
		return ret;
	}

	public static void ModifyResearchSorting(Item item, ref ContentSamples.CreativeHelper.ItemGroup itemGroup)
	{
		if (!item.IsAir)
		{
			item.ModItem?.ModifyResearchSorting(ref itemGroup);
			EntityGlobalsEnumerator<GlobalItem> enumerator = HookModifyResearchSorting.Enumerate(item).GetEnumerator();
			while (enumerator.MoveNext())
			{
				enumerator.Current.ModifyResearchSorting(item, ref itemGroup);
			}
		}
	}

	/// <summary>
	/// Hook that determines if an item will be prevented from being consumed by the research function. 
	/// </summary>
	/// <param name="item">The item to be consumed or not</param>
	public static bool CanResearch(Item item)
	{
		if (item.ModItem != null && !item.ModItem.CanResearch())
		{
			return false;
		}
		EntityGlobalsEnumerator<GlobalItem> enumerator = HookCanResearch.Enumerate(item).GetEnumerator();
		while (enumerator.MoveNext())
		{
			if (!enumerator.Current.Instance(item).CanResearch(item))
			{
				return false;
			}
		}
		return true;
	}

	public static void OnResearched(Item item, bool fullyResearched)
	{
		if (!item.IsAir)
		{
			item.ModItem?.OnResearched(fullyResearched);
			EntityGlobalsEnumerator<GlobalItem> enumerator = HookOnResearched.Enumerate(item).GetEnumerator();
			while (enumerator.MoveNext())
			{
				enumerator.Current.Instance(item).OnResearched(item, fullyResearched);
			}
		}
	}

	/// <summary>
	/// Calls ModItem.HookModifyWeaponDamage, then all GlobalItem.HookModifyWeaponDamage hooks.
	/// </summary>
	public static void ModifyWeaponDamage(Item item, Player player, ref StatModifier damage)
	{
		if (!item.IsAir)
		{
			item.ModItem?.ModifyWeaponDamage(player, ref damage);
			EntityGlobalsEnumerator<GlobalItem> enumerator = HookModifyWeaponDamage.Enumerate(item).GetEnumerator();
			while (enumerator.MoveNext())
			{
				enumerator.Current.ModifyWeaponDamage(item, player, ref damage);
			}
		}
	}

	/// <summary>
	/// Calls ModItem.ModifyWeaponKnockback, then all GlobalItem.ModifyWeaponKnockback hooks.
	/// </summary>
	public static void ModifyWeaponKnockback(Item item, Player player, ref StatModifier knockback)
	{
		if (!item.IsAir)
		{
			item.ModItem?.ModifyWeaponKnockback(player, ref knockback);
			EntityGlobalsEnumerator<GlobalItem> enumerator = HookModifyWeaponKnockback.Enumerate(item).GetEnumerator();
			while (enumerator.MoveNext())
			{
				enumerator.Current.ModifyWeaponKnockback(item, player, ref knockback);
			}
		}
	}

	/// <summary>
	/// Calls ModItem.ModifyWeaponCrit, then all GlobalItem.ModifyWeaponCrit hooks.
	/// </summary>
	public static void ModifyWeaponCrit(Item item, Player player, ref float crit)
	{
		if (!item.IsAir)
		{
			item.ModItem?.ModifyWeaponCrit(player, ref crit);
			EntityGlobalsEnumerator<GlobalItem> enumerator = HookModifyWeaponCrit.Enumerate(item).GetEnumerator();
			while (enumerator.MoveNext())
			{
				enumerator.Current.ModifyWeaponCrit(item, player, ref crit);
			}
		}
	}

	/// <summary>
	/// Calls ModItem.NeedsAmmo, then all GlobalItem.NeedsAmmo hooks, until any of them returns false.
	/// </summary>
	public static bool NeedsAmmo(Item weapon, Player player)
	{
		ModItem modItem = weapon.ModItem;
		if (modItem != null && !modItem.NeedsAmmo(player))
		{
			return false;
		}
		EntityGlobalsEnumerator<GlobalItem> enumerator = HookNeedsAmmo.Enumerate(weapon).GetEnumerator();
		while (enumerator.MoveNext())
		{
			if (!enumerator.Current.NeedsAmmo(weapon, player))
			{
				return false;
			}
		}
		return true;
	}

	/// <summary>
	/// Calls ModItem.PickAmmo, then all GlobalItem.PickAmmo hooks.
	/// </summary>
	public static void PickAmmo(Item weapon, Item ammo, Player player, ref int type, ref float speed, ref StatModifier damage, ref float knockback)
	{
		ammo.ModItem?.PickAmmo(weapon, player, ref type, ref speed, ref damage, ref knockback);
		EntityGlobalsEnumerator<GlobalItem> enumerator = HookPickAmmo.Enumerate(ammo).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.PickAmmo(weapon, ammo, player, ref type, ref speed, ref damage, ref knockback);
		}
	}

	/// <summary>
	/// Calls each <see cref="M:Terraria.ModLoader.GlobalItem.CanChooseAmmo(Terraria.Item,Terraria.Item,Terraria.Player)" /> hook for the weapon, and each <see cref="M:Terraria.ModLoader.GlobalItem.CanBeChosenAsAmmo(Terraria.Item,Terraria.Item,Terraria.Player)" /> hook for the ammo,<br></br>
	/// then each corresponding hook in <see cref="T:Terraria.ModLoader.ModItem" /> if applicable for the weapon and/or ammo, until one of them returns a concrete false value.<br></br>
	/// If all of them fail to do this, returns either true (if one returned true prior) or <c>ammo.ammo == weapon.useAmmo</c>.
	/// </summary>
	public static bool CanChooseAmmo(Item weapon, Item ammo, Player player)
	{
		bool? result = null;
		EntityGlobalsEnumerator<GlobalItem> entityGlobalsEnumerator = HookCanChooseAmmo.Enumerate(weapon);
		EntityGlobalsEnumerator<GlobalItem> enumerator = entityGlobalsEnumerator.GetEnumerator();
		while (enumerator.MoveNext())
		{
			bool? r4 = enumerator.Current.CanChooseAmmo(weapon, ammo, player);
			if ((!r4) ?? false)
			{
				return false;
			}
			bool? flag = result;
			if (!flag.HasValue)
			{
				result = r4;
			}
		}
		entityGlobalsEnumerator = HookCanBeChosenAsAmmo.Enumerate(ammo);
		enumerator = entityGlobalsEnumerator.GetEnumerator();
		while (enumerator.MoveNext())
		{
			bool? r3 = enumerator.Current.CanBeChosenAsAmmo(ammo, weapon, player);
			if ((!r3) ?? false)
			{
				return false;
			}
			bool? flag = result;
			if (!flag.HasValue)
			{
				result = r3;
			}
		}
		if (weapon.ModItem != null)
		{
			bool? r2 = weapon.ModItem.CanChooseAmmo(ammo, player);
			if ((!r2) ?? false)
			{
				return false;
			}
			bool? flag = result;
			if (!flag.HasValue)
			{
				result = r2;
			}
		}
		if (ammo.ModItem != null)
		{
			bool? r = ammo.ModItem.CanBeChosenAsAmmo(weapon, player);
			if ((!r) ?? false)
			{
				return false;
			}
			bool? flag = result;
			if (!flag.HasValue)
			{
				result = r;
			}
		}
		return result ?? (ammo.ammo == weapon.useAmmo);
	}

	/// <summary>
	/// Calls each <see cref="M:Terraria.ModLoader.GlobalItem.CanConsumeAmmo(Terraria.Item,Terraria.Item,Terraria.Player)" /> hook for the weapon, and each <see cref="M:Terraria.ModLoader.GlobalItem.CanBeConsumedAsAmmo(Terraria.Item,Terraria.Item,Terraria.Player)" /> hook for the ammo,<br></br>
	/// then each corresponding hook in <see cref="T:Terraria.ModLoader.ModItem" /> if applicable for the weapon and/or ammo, until one of them returns a concrete false value.<br></br>
	/// If all of them fail to do this, returns true.
	/// </summary>
	public static bool CanConsumeAmmo(Item weapon, Item ammo, Player player)
	{
		EntityGlobalsEnumerator<GlobalItem> entityGlobalsEnumerator = HookCanConsumeAmmo.Enumerate(weapon);
		EntityGlobalsEnumerator<GlobalItem> enumerator = entityGlobalsEnumerator.GetEnumerator();
		while (enumerator.MoveNext())
		{
			if (!enumerator.Current.CanConsumeAmmo(weapon, ammo, player))
			{
				return false;
			}
		}
		entityGlobalsEnumerator = HookCanBeConsumedAsAmmo.Enumerate(ammo);
		enumerator = entityGlobalsEnumerator.GetEnumerator();
		while (enumerator.MoveNext())
		{
			if (!enumerator.Current.CanBeConsumedAsAmmo(ammo, weapon, player))
			{
				return false;
			}
		}
		if ((weapon.ModItem != null && !weapon.ModItem.CanConsumeAmmo(ammo, player)) || (ammo.ModItem != null && !ammo.ModItem.CanBeConsumedAsAmmo(weapon, player)))
		{
			return false;
		}
		return true;
	}

	/// <summary>
	/// Calls <see cref="M:Terraria.ModLoader.ModItem.OnConsumeAmmo(Terraria.Item,Terraria.Player)" /> for the weapon, <see cref="M:Terraria.ModLoader.ModItem.OnConsumedAsAmmo(Terraria.Item,Terraria.Player)" /> for the ammo,
	/// then each corresponding hook for the weapon and ammo.
	/// </summary>
	public static void OnConsumeAmmo(Item weapon, Item ammo, Player player)
	{
		if (!weapon.IsAir)
		{
			weapon.ModItem?.OnConsumeAmmo(ammo, player);
			ammo.ModItem?.OnConsumedAsAmmo(weapon, player);
			EntityGlobalsEnumerator<GlobalItem> entityGlobalsEnumerator = HookOnConsumeAmmo.Enumerate(weapon);
			EntityGlobalsEnumerator<GlobalItem> enumerator = entityGlobalsEnumerator.GetEnumerator();
			while (enumerator.MoveNext())
			{
				enumerator.Current.OnConsumeAmmo(weapon, ammo, player);
			}
			entityGlobalsEnumerator = HookOnConsumedAsAmmo.Enumerate(ammo);
			enumerator = entityGlobalsEnumerator.GetEnumerator();
			while (enumerator.MoveNext())
			{
				enumerator.Current.OnConsumedAsAmmo(ammo, weapon, player);
			}
		}
	}

	/// <summary>
	/// Calls each GlobalItem.CanShoot hook, then ModItem.CanShoot, until one of them returns false. If all of them return true, returns true.
	/// </summary>
	public static bool CanShoot(Item item, Player player)
	{
		EntityGlobalsEnumerator<GlobalItem> enumerator = HookCanShoot.Enumerate(item).GetEnumerator();
		while (enumerator.MoveNext())
		{
			if (!enumerator.Current.CanShoot(item, player))
			{
				return false;
			}
		}
		return item.ModItem?.CanShoot(player) ?? true;
	}

	/// <summary>
	/// Calls ModItem.ModifyShootStats, then each GlobalItem.ModifyShootStats hook.
	/// </summary>
	public static void ModifyShootStats(Item item, Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
	{
		item.ModItem?.ModifyShootStats(player, ref position, ref velocity, ref type, ref damage, ref knockback);
		EntityGlobalsEnumerator<GlobalItem> enumerator = HookModifyShootStats.Enumerate(item).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.ModifyShootStats(item, player, ref position, ref velocity, ref type, ref damage, ref knockback);
		}
	}

	/// <summary>
	/// Calls each GlobalItem.Shoot hook then, if none of them returns false, calls the ModItem.Shoot hook and returns its value.
	/// </summary>
	public static bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, bool defaultResult = true)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		EntityGlobalsEnumerator<GlobalItem> enumerator = HookShoot.Enumerate(item).GetEnumerator();
		while (enumerator.MoveNext())
		{
			GlobalItem g = enumerator.Current;
			defaultResult &= g.Shoot(item, player, source, position, velocity, type, damage, knockback);
		}
		if (defaultResult)
		{
			return item.ModItem?.Shoot(player, source, position, velocity, type, damage, knockback) ?? true;
		}
		return false;
	}

	/// <summary>
	/// Calls ModItem.UseItemHitbox, then all GlobalItem.UseItemHitbox hooks.
	/// </summary>
	public static void UseItemHitbox(Item item, Player player, ref Rectangle hitbox, ref bool noHitbox)
	{
		item.ModItem?.UseItemHitbox(player, ref hitbox, ref noHitbox);
		EntityGlobalsEnumerator<GlobalItem> enumerator = HookUseItemHitbox.Enumerate(item).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.UseItemHitbox(item, player, ref hitbox, ref noHitbox);
		}
	}

	/// <summary>
	/// Calls ModItem.MeleeEffects and all GlobalItem.MeleeEffects hooks.
	/// </summary>
	public static void MeleeEffects(Item item, Player player, Rectangle hitbox)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		item.ModItem?.MeleeEffects(player, hitbox);
		EntityGlobalsEnumerator<GlobalItem> enumerator = HookMeleeEffects.Enumerate(item).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.MeleeEffects(item, player, hitbox);
		}
	}

	/// <summary>
	/// Gathers the results of all <see cref="M:Terraria.ModLoader.GlobalItem.CanCatchNPC(Terraria.Item,Terraria.NPC,Terraria.Player)" /> hooks, then the <see cref="M:Terraria.ModLoader.ModItem.CanCatchNPC(Terraria.NPC,Terraria.Player)" /> hook if applicable.<br></br>
	/// If any of them returns false, this returns false.<br></br>
	/// Otherwise, if any of them returns true, then this returns true.<br></br>
	/// If all of them return null, this returns null.<br></br>
	/// </summary>
	public static bool? CanCatchNPC(Item item, NPC target, Player player)
	{
		bool? canCatchOverall = null;
		EntityGlobalsEnumerator<GlobalItem> enumerator = HookCanCatchNPC.Enumerate(item).GetEnumerator();
		while (enumerator.MoveNext())
		{
			bool? canCatchFromGlobalItem = enumerator.Current.CanCatchNPC(item, target, player);
			if (canCatchFromGlobalItem.HasValue)
			{
				if (!canCatchFromGlobalItem.Value)
				{
					return false;
				}
				canCatchOverall = true;
			}
		}
		if (item.ModItem != null)
		{
			bool? canCatchAsModItem = item.ModItem.CanCatchNPC(target, player);
			if (canCatchAsModItem.HasValue)
			{
				if (!canCatchAsModItem.Value)
				{
					return false;
				}
				canCatchOverall = true;
			}
		}
		return canCatchOverall;
	}

	public static void OnCatchNPC(Item item, NPC npc, Player player, bool failed)
	{
		item.ModItem?.OnCatchNPC(npc, player, failed);
		EntityGlobalsEnumerator<GlobalItem> enumerator = HookOnCatchNPC.Enumerate(item).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.OnCatchNPC(item, npc, player, failed);
		}
	}

	/// <summary>
	/// Calls <see cref="M:Terraria.ModLoader.ModItem.ModifyItemScale(Terraria.Player,System.Single@)" /> if applicable, then all applicable <see cref="M:Terraria.ModLoader.GlobalItem.ModifyItemScale(Terraria.Item,Terraria.Player,System.Single@)" /> instances.
	/// </summary>
	public static void ModifyItemScale(Item item, Player player, ref float scale)
	{
		item.ModItem?.ModifyItemScale(player, ref scale);
		EntityGlobalsEnumerator<GlobalItem> enumerator = HookModifyItemScale.Enumerate(item).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.ModifyItemScale(item, player, ref scale);
		}
	}

	/// <summary>
	/// Gathers the results of ModItem.CanHitNPC and all GlobalItem.CanHitNPC hooks.
	/// If any of them returns false, this returns false.
	/// Otherwise, if any of them returns true then this returns true.
	/// If all of them return null, this returns null.
	/// </summary>
	public static bool? CanHitNPC(Item item, Player player, NPC target)
	{
		bool? flag = null;
		EntityGlobalsEnumerator<GlobalItem> enumerator = HookCanHitNPC.Enumerate(item).GetEnumerator();
		while (enumerator.MoveNext())
		{
			bool? canHit2 = enumerator.Current.CanHitNPC(item, player, target);
			if (canHit2.HasValue)
			{
				if (!canHit2.Value)
				{
					return false;
				}
				flag = true;
			}
		}
		if (item.ModItem != null)
		{
			bool? canHit = item.ModItem.CanHitNPC(player, target);
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

	public static bool? CanMeleeAttackCollideWithNPC(Item item, Rectangle meleeAttackHitbox, Player player, NPC target)
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		bool? flag = null;
		EntityGlobalsEnumerator<GlobalItem> enumerator = HookCanCollideNPC.Enumerate(item).GetEnumerator();
		while (enumerator.MoveNext())
		{
			bool? canCollide = enumerator.Current.CanMeleeAttackCollideWithNPC(item, meleeAttackHitbox, player, target);
			if (canCollide.HasValue)
			{
				if (!canCollide.Value)
				{
					return false;
				}
				flag = true;
			}
		}
		if (item.ModItem != null)
		{
			bool? canHit = item.ModItem.CanMeleeAttackCollideWithNPC(meleeAttackHitbox, player, target);
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

	/// <summary>
	/// Calls ModItem.ModifyHitNPC, then all GlobalItem.ModifyHitNPC hooks.
	/// </summary>
	public static void ModifyHitNPC(Item item, Player player, NPC target, ref NPC.HitModifiers modifiers)
	{
		item.ModItem?.ModifyHitNPC(player, target, ref modifiers);
		EntityGlobalsEnumerator<GlobalItem> enumerator = HookModifyHitNPC.Enumerate(item).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.ModifyHitNPC(item, player, target, ref modifiers);
		}
	}

	/// <summary>
	/// Calls ModItem.OnHitNPC and all GlobalItem.OnHitNPC hooks.
	/// </summary>
	public static void OnHitNPC(Item item, Player player, NPC target, in NPC.HitInfo hit, int damageDone)
	{
		item.ModItem?.OnHitNPC(player, target, hit, damageDone);
		EntityGlobalsEnumerator<GlobalItem> enumerator = HookOnHitNPC.Enumerate(item).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.OnHitNPC(item, player, target, hit, damageDone);
		}
	}

	/// <summary>
	/// Calls all GlobalItem.CanHitPvp hooks, then ModItem.CanHitPvp, until one of them returns false.
	/// If all of them return true, this returns true.
	/// </summary>
	public static bool CanHitPvp(Item item, Player player, Player target)
	{
		EntityGlobalsEnumerator<GlobalItem> enumerator = HookCanHitPvp.Enumerate(item).GetEnumerator();
		while (enumerator.MoveNext())
		{
			if (!enumerator.Current.CanHitPvp(item, player, target))
			{
				return false;
			}
		}
		if (item.ModItem != null)
		{
			return item.ModItem.CanHitPvp(player, target);
		}
		return true;
	}

	/// <summary>
	/// Calls ModItem.ModifyHitPvp, then all GlobalItem.ModifyHitPvp hooks.
	/// </summary>
	public static void ModifyHitPvp(Item item, Player player, Player target, ref Player.HurtModifiers modifiers)
	{
		item.ModItem?.ModifyHitPvp(player, target, ref modifiers);
		EntityGlobalsEnumerator<GlobalItem> enumerator = HookModifyHitPvp.Enumerate(item).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.ModifyHitPvp(item, player, target, ref modifiers);
		}
	}

	/// <summary>
	/// Calls ModItem.OnHitPvp and all GlobalItem.OnHitPvp hooks. <br />
	/// Called on local, server and remote clients. <br />
	/// </summary>
	public static void OnHitPvp(Item item, Player player, Player target, Player.HurtInfo hurtInfo)
	{
		item.ModItem?.OnHitPvp(player, target, hurtInfo);
		EntityGlobalsEnumerator<GlobalItem> enumerator = HookOnHitPvp.Enumerate(item).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.OnHitPvp(item, player, target, hurtInfo);
		}
	}

	/// <summary>
	/// Returns false if any of ModItem.UseItem or GlobalItem.UseItem return false.
	/// Returns true if anything returns true without returning false.
	/// Returns null by default.
	/// Does not fail fast (calls every hook)
	/// </summary>
	public static bool? UseItem(Item item, Player player)
	{
		if (item.IsAir)
		{
			return null;
		}
		bool? result = null;
		EntityGlobalsEnumerator<GlobalItem> enumerator = HookUseItem.Enumerate(item).GetEnumerator();
		while (enumerator.MoveNext())
		{
			bool? useItem = enumerator.Current.UseItem(item, player);
			if (useItem.HasValue && result != false)
			{
				result = useItem.Value;
			}
		}
		bool? modItemResult = item.ModItem?.UseItem(player);
		return result ?? modItemResult;
	}

	public static void UseAnimation(Item item, Player player)
	{
		EntityGlobalsEnumerator<GlobalItem> enumerator = HookUseAnimation.Enumerate(item).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.Instance(item).UseAnimation(item, player);
		}
		item.ModItem?.UseAnimation(player);
	}

	/// <summary>
	/// If ModItem.ConsumeItem or any of the GlobalItem.ConsumeItem hooks returns false, sets consume to false.
	/// </summary>
	public static bool ConsumeItem(Item item, Player player)
	{
		if (item.IsAir)
		{
			return true;
		}
		if (item.ModItem != null && !item.ModItem.ConsumeItem(player))
		{
			return false;
		}
		EntityGlobalsEnumerator<GlobalItem> enumerator = HookConsumeItem.Enumerate(item).GetEnumerator();
		while (enumerator.MoveNext())
		{
			if (!enumerator.Current.ConsumeItem(item, player))
			{
				return false;
			}
		}
		OnConsumeItem(item, player);
		return true;
	}

	/// <summary>
	/// Calls ModItem.OnConsumeItem and all GlobalItem.OnConsumeItem hooks.
	/// </summary>
	public static void OnConsumeItem(Item item, Player player)
	{
		if (!item.IsAir)
		{
			item.ModItem?.OnConsumeItem(player);
			EntityGlobalsEnumerator<GlobalItem> enumerator = HookOnConsumeItem.Enumerate(item).GetEnumerator();
			while (enumerator.MoveNext())
			{
				enumerator.Current.OnConsumeItem(item, player);
			}
		}
	}

	/// <summary>
	/// Calls ModItem.UseItemFrame, then all GlobalItem.UseItemFrame hooks, until one of them returns true. Returns whether any of the hooks returned true.
	/// </summary>
	public static void UseItemFrame(Item item, Player player)
	{
		if (!item.IsAir)
		{
			item.ModItem?.UseItemFrame(player);
			EntityGlobalsEnumerator<GlobalItem> enumerator = HookUseItemFrame.Enumerate(item).GetEnumerator();
			while (enumerator.MoveNext())
			{
				enumerator.Current.UseItemFrame(item, player);
			}
		}
	}

	/// <summary>
	/// Calls ModItem.HoldItemFrame, then all GlobalItem.HoldItemFrame hooks, until one of them returns true. Returns whether any of the hooks returned true.
	/// </summary>
	public static void HoldItemFrame(Item item, Player player)
	{
		if (!item.IsAir)
		{
			item.ModItem?.HoldItemFrame(player);
			EntityGlobalsEnumerator<GlobalItem> enumerator = HookHoldItemFrame.Enumerate(item).GetEnumerator();
			while (enumerator.MoveNext())
			{
				enumerator.Current.HoldItemFrame(item, player);
			}
		}
	}

	/// <summary>
	/// Calls ModItem.AltFunctionUse, then all GlobalItem.AltFunctionUse hooks, until one of them returns true. Returns whether any of the hooks returned true.
	/// </summary>
	public static bool AltFunctionUse(Item item, Player player)
	{
		if (item.IsAir)
		{
			return false;
		}
		if (item.ModItem != null && item.ModItem.AltFunctionUse(player))
		{
			return true;
		}
		EntityGlobalsEnumerator<GlobalItem> enumerator = HookAltFunctionUse.Enumerate(item).GetEnumerator();
		while (enumerator.MoveNext())
		{
			if (enumerator.Current.AltFunctionUse(item, player))
			{
				return true;
			}
		}
		return false;
	}

	/// <summary>
	/// Calls ModItem.UpdateInventory and all GlobalItem.UpdateInventory hooks.
	/// </summary>
	public static void UpdateInventory(Item item, Player player)
	{
		if (!item.IsAir)
		{
			item.ModItem?.UpdateInventory(player);
			EntityGlobalsEnumerator<GlobalItem> enumerator = HookUpdateInventory.Enumerate(item).GetEnumerator();
			while (enumerator.MoveNext())
			{
				enumerator.Current.UpdateInventory(item, player);
			}
		}
	}

	/// <summary>
	/// Calls ModItem.UpdateInfoAccessory and all GlobalItem.UpdateInfoAccessory hooks.
	/// </summary>
	public static void UpdateInfoAccessory(Item item, Player player)
	{
		if (!item.IsAir)
		{
			item.ModItem?.UpdateInfoAccessory(player);
			EntityGlobalsEnumerator<GlobalItem> enumerator = HookUpdateInfoAccessory.Enumerate(item).GetEnumerator();
			while (enumerator.MoveNext())
			{
				enumerator.Current.UpdateInfoAccessory(item, player);
			}
		}
	}

	/// <summary>
	/// Hook at the end of Player.VanillaUpdateEquip can be called to apply additional code related to accessory slots for a particular item
	/// </summary>
	public static void UpdateEquip(Item item, Player player)
	{
		if (!item.IsAir)
		{
			item.ModItem?.UpdateEquip(player);
			EntityGlobalsEnumerator<GlobalItem> enumerator = HookUpdateEquip.Enumerate(item).GetEnumerator();
			while (enumerator.MoveNext())
			{
				enumerator.Current.UpdateEquip(item, player);
			}
		}
	}

	/// <summary>
	/// Hook at the end of Player.ApplyEquipFunctional can be called to apply additional code related to accessory slots for a particular item.
	/// </summary>
	public static void UpdateAccessory(Item item, Player player, bool hideVisual)
	{
		if (!item.IsAir)
		{
			item.ModItem?.UpdateAccessory(player, hideVisual);
			EntityGlobalsEnumerator<GlobalItem> enumerator = HookUpdateAccessory.Enumerate(item).GetEnumerator();
			while (enumerator.MoveNext())
			{
				enumerator.Current.UpdateAccessory(item, player, hideVisual);
			}
		}
	}

	/// <summary>
	/// Hook at the end of Player.ApplyEquipVanity can be called to apply additional code related to accessory slots for a particular item
	/// </summary>
	public static void UpdateVanity(Item item, Player player)
	{
		if (!item.IsAir)
		{
			item.ModItem?.UpdateVanity(player);
			EntityGlobalsEnumerator<GlobalItem> enumerator = HookUpdateVanity.Enumerate(item).GetEnumerator();
			while (enumerator.MoveNext())
			{
				enumerator.Current.UpdateVanity(item, player);
			}
		}
	}

	/// <summary>
	/// If the head's ModItem.IsArmorSet returns true, calls the head's ModItem.UpdateArmorSet. This is then repeated for the body, then the legs. Then for each GlobalItem, if GlobalItem.IsArmorSet returns a non-empty string, calls GlobalItem.UpdateArmorSet with that string.
	/// </summary>
	public static void UpdateArmorSet(Player player, Item head, Item body, Item legs)
	{
		if (head.ModItem != null && head.ModItem.IsArmorSet(head, body, legs))
		{
			head.ModItem.UpdateArmorSet(player);
		}
		if (body.ModItem != null && body.ModItem.IsArmorSet(head, body, legs))
		{
			body.ModItem.UpdateArmorSet(player);
		}
		if (legs.ModItem != null && legs.ModItem.IsArmorSet(head, body, legs))
		{
			legs.ModItem.UpdateArmorSet(player);
		}
		ReadOnlySpan<GlobalItem> readOnlySpan = HookUpdateArmorSet.Enumerate();
		for (int i = 0; i < readOnlySpan.Length; i++)
		{
			GlobalItem g = readOnlySpan[i];
			string set = g.IsArmorSet(head, body, legs);
			if (!string.IsNullOrEmpty(set))
			{
				g.UpdateArmorSet(player, set);
			}
		}
	}

	/// <summary>
	/// If the player's head texture's IsVanitySet returns true, calls the equipment texture's PreUpdateVanitySet. This is then repeated for the player's body, then the legs. Then for each GlobalItem, if GlobalItem.IsVanitySet returns a non-empty string, calls GlobalItem.PreUpdateVanitySet, using player.head, player.body, and player.legs.
	/// </summary>
	public static void PreUpdateVanitySet(Player player)
	{
		EquipTexture headTexture = EquipLoader.GetEquipTexture(EquipType.Head, player.head);
		EquipTexture bodyTexture = EquipLoader.GetEquipTexture(EquipType.Body, player.body);
		EquipTexture legTexture = EquipLoader.GetEquipTexture(EquipType.Legs, player.legs);
		if (headTexture != null && headTexture.IsVanitySet(player.head, player.body, player.legs))
		{
			headTexture.PreUpdateVanitySet(player);
		}
		if (bodyTexture != null && bodyTexture.IsVanitySet(player.head, player.body, player.legs))
		{
			bodyTexture.PreUpdateVanitySet(player);
		}
		if (legTexture != null && legTexture.IsVanitySet(player.head, player.body, player.legs))
		{
			legTexture.PreUpdateVanitySet(player);
		}
		ReadOnlySpan<GlobalItem> readOnlySpan = HookPreUpdateVanitySet.Enumerate();
		for (int i = 0; i < readOnlySpan.Length; i++)
		{
			GlobalItem g = readOnlySpan[i];
			string set = g.IsVanitySet(player.head, player.body, player.legs);
			if (!string.IsNullOrEmpty(set))
			{
				g.PreUpdateVanitySet(player, set);
			}
		}
	}

	/// <summary>
	/// If the player's head texture's IsVanitySet returns true, calls the equipment texture's UpdateVanitySet. This is then repeated for the player's body, then the legs. Then for each GlobalItem, if GlobalItem.IsVanitySet returns a non-empty string, calls GlobalItem.UpdateVanitySet, using player.head, player.body, and player.legs.
	/// </summary>
	public static void UpdateVanitySet(Player player)
	{
		EquipTexture headTexture = EquipLoader.GetEquipTexture(EquipType.Head, player.head);
		EquipTexture bodyTexture = EquipLoader.GetEquipTexture(EquipType.Body, player.body);
		EquipTexture legTexture = EquipLoader.GetEquipTexture(EquipType.Legs, player.legs);
		if (headTexture != null && headTexture.IsVanitySet(player.head, player.body, player.legs))
		{
			headTexture.UpdateVanitySet(player);
		}
		if (bodyTexture != null && bodyTexture.IsVanitySet(player.head, player.body, player.legs))
		{
			bodyTexture.UpdateVanitySet(player);
		}
		if (legTexture != null && legTexture.IsVanitySet(player.head, player.body, player.legs))
		{
			legTexture.UpdateVanitySet(player);
		}
		ReadOnlySpan<GlobalItem> readOnlySpan = HookUpdateVanitySet.Enumerate();
		for (int i = 0; i < readOnlySpan.Length; i++)
		{
			GlobalItem g = readOnlySpan[i];
			string set = g.IsVanitySet(player.head, player.body, player.legs);
			if (!string.IsNullOrEmpty(set))
			{
				g.UpdateVanitySet(player, set);
			}
		}
	}

	/// <summary>
	/// If the player's head texture's IsVanitySet returns true, calls the equipment texture's ArmorSetShadows. This is then repeated for the player's body, then the legs. Then for each GlobalItem, if GlobalItem.IsVanitySet returns a non-empty string, calls GlobalItem.ArmorSetShadows, using player.head, player.body, and player.legs.
	/// </summary>
	public static void ArmorSetShadows(Player player)
	{
		EquipTexture headTexture = EquipLoader.GetEquipTexture(EquipType.Head, player.head);
		EquipTexture bodyTexture = EquipLoader.GetEquipTexture(EquipType.Body, player.body);
		EquipTexture legTexture = EquipLoader.GetEquipTexture(EquipType.Legs, player.legs);
		if (headTexture != null && headTexture.IsVanitySet(player.head, player.body, player.legs))
		{
			headTexture.ArmorSetShadows(player);
		}
		if (bodyTexture != null && bodyTexture.IsVanitySet(player.head, player.body, player.legs))
		{
			bodyTexture.ArmorSetShadows(player);
		}
		if (legTexture != null && legTexture.IsVanitySet(player.head, player.body, player.legs))
		{
			legTexture.ArmorSetShadows(player);
		}
		ReadOnlySpan<GlobalItem> readOnlySpan = HookArmorSetShadows.Enumerate();
		for (int i = 0; i < readOnlySpan.Length; i++)
		{
			GlobalItem g = readOnlySpan[i];
			string set = g.IsVanitySet(player.head, player.body, player.legs);
			if (!string.IsNullOrEmpty(set))
			{
				g.ArmorSetShadows(player, set);
			}
		}
	}

	/// <summary>
	/// Calls EquipTexture.SetMatch, then all GlobalItem.SetMatch hooks.
	/// </summary>
	public static void SetMatch(int armorSlot, int type, bool male, ref int equipSlot, ref bool robes)
	{
		EquipLoader.GetEquipTexture((EquipType)armorSlot, type)?.SetMatch(male, ref equipSlot, ref robes);
		ReadOnlySpan<GlobalItem> readOnlySpan = HookSetMatch.Enumerate();
		for (int i = 0; i < readOnlySpan.Length; i++)
		{
			readOnlySpan[i].SetMatch(armorSlot, type, male, ref equipSlot, ref robes);
		}
	}

	/// <summary>
	/// Calls ModItem.CanRightClick, then all GlobalItem.CanRightClick hooks, until one of the returns true.
	/// Also returns true if ItemID.Sets.OpenableBag
	/// </summary>
	public static bool CanRightClick(Item item)
	{
		if (item.IsAir)
		{
			return false;
		}
		if (ItemID.Sets.OpenableBag[item.type])
		{
			return true;
		}
		if (item.ModItem != null && item.ModItem.CanRightClick())
		{
			return true;
		}
		EntityGlobalsEnumerator<GlobalItem> enumerator = HookCanRightClick.Enumerate(item).GetEnumerator();
		while (enumerator.MoveNext())
		{
			if (enumerator.Current.CanRightClick(item))
			{
				return true;
			}
		}
		return false;
	}

	/// <summary>
	/// 1. Call ModItem.RightClick
	/// 2. Calls all GlobalItem.RightClick hooks
	/// 3. Call ItemLoader.ConsumeItem, and if it returns true, decrements the item's stack
	/// 4. Sets the item's type to 0 if the item's stack is 0
	/// 5. Plays the item-grabbing sound
	/// 6. Sets Main.stackSplit to 30
	/// 7. Sets Main.mouseRightRelease to false
	/// 8. Calls Recipe.FindRecipes.
	/// </summary>
	public static void RightClick(Item item, Player player)
	{
		RightClickCallHooks(item, player);
		if (ConsumeItem(item, player) && --item.stack == 0)
		{
			item.SetDefaults();
		}
		SoundEngine.PlaySound(7);
		Main.stackSplit = 30;
		Main.mouseRightRelease = false;
		Recipe.FindRecipes();
	}

	internal static void RightClickCallHooks(Item item, Player player)
	{
		item.ModItem?.RightClick(player);
		EntityGlobalsEnumerator<GlobalItem> enumerator = HookRightClick.Enumerate(item).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.RightClick(item, player);
		}
	}

	/// <summary>
	/// Calls each GlobalItem.ModifyItemLoot hooks.
	/// </summary>
	public static void ModifyItemLoot(Item item, ItemLoot itemLoot)
	{
		item.ModItem?.ModifyItemLoot(itemLoot);
		EntityGlobalsEnumerator<GlobalItem> enumerator = HookModifyItemLoot.Enumerate(item).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.ModifyItemLoot(item, itemLoot);
		}
	}

	/// <summary>
	/// Returns false if item prefixes don't match. Then calls all GlobalItem.CanStack hooks until one returns false then ModItem.CanStack. Returns whether any of the hooks returned false.
	/// </summary>
	/// <param name="destination">The item instance that <paramref name="source" /> will attempt to stack onto</param>
	/// <param name="source">The item instance being stacked onto <paramref name="destination" /></param>
	/// <returns>Whether or not the items are allowed to stack</returns>
	public static bool CanStack(Item destination, Item source)
	{
		if (destination.prefix != source.prefix)
		{
			return false;
		}
		EntityGlobalsEnumerator<GlobalItem> enumerator = HookCanStack.Enumerate(destination).GetEnumerator();
		while (enumerator.MoveNext())
		{
			if (!enumerator.Current.CanStack(destination, source))
			{
				return false;
			}
		}
		return destination.ModItem?.CanStack(source) ?? true;
	}

	/// <summary>
	/// Calls all GlobalItem.CanStackInWorld hooks until one returns false then ModItem.CanStackInWorld. Returns whether any of the hooks returned false.
	/// </summary>
	/// <param name="destination">The item instance that <paramref name="source" /> will attempt to stack onto</param>
	/// <param name="source">The item instance being stacked onto <paramref name="destination" /></param>
	/// <returns>Whether or not the items are allowed to stack</returns>
	public static bool CanStackInWorld(Item destination, Item source)
	{
		EntityGlobalsEnumerator<GlobalItem> enumerator = HookCanStackInWorld.Enumerate(destination).GetEnumerator();
		while (enumerator.MoveNext())
		{
			if (!enumerator.Current.CanStackInWorld(destination, source))
			{
				return false;
			}
		}
		return destination.ModItem?.CanStackInWorld(source) ?? true;
	}

	/// <summary>
	/// Stacks <paramref name="source" /> onto <paramref name="destination" /> if CanStack permits the transfer
	/// </summary>
	/// <param name="destination">The item instance that <paramref name="source" /> will attempt to stack onto</param>
	/// <param name="source">The item instance being stacked onto <paramref name="destination" /></param>
	/// <param name="numTransferred">The quantity of <paramref name="source" /> that was transferred to <paramref name="destination" /></param>
	/// <param name="infiniteSource">If true, <paramref name="source" />.stack will not be decreased</param>
	/// <returns>Whether or not the items were allowed to stack</returns>
	public static bool TryStackItems(Item destination, Item source, out int numTransferred, bool infiniteSource = false)
	{
		numTransferred = 0;
		if (!CanStack(destination, source))
		{
			return false;
		}
		StackItems(destination, source, out numTransferred, infiniteSource);
		return true;
	}

	/// <summary>
	/// Stacks <paramref name="destination" /> onto <paramref name="source" /><br />
	/// This method should not be called unless <see cref="M:Terraria.ModLoader.ItemLoader.CanStack(Terraria.Item,Terraria.Item)" /> returns true.  See: <see cref="M:Terraria.ModLoader.ItemLoader.TryStackItems(Terraria.Item,Terraria.Item,System.Int32@,System.Boolean)" />
	/// </summary>
	/// <param name="destination">The item instance that <paramref name="source" /> will attempt to stack onto</param>
	/// <param name="source">The item instance being stacked onto <paramref name="destination" /></param>
	/// <param name="numTransferred">The quantity of <paramref name="source" /> that was transferred to <paramref name="destination" /></param>
	/// <param name="infiniteSource">If true, <paramref name="source" />.stack will not be decreased</param>
	/// <param name="numToTransfer">
	/// An optional argument used to specify the quantity of items to transfer from <paramref name="source" /> to <paramref name="destination" />.<br />
	/// By default, as many items as possible will be transferred.  That is, either source will be empty, or destination will be full (maxStack)
	/// </param>
	public static void StackItems(Item destination, Item source, out int numTransferred, bool infiniteSource = false, int? numToTransfer = null)
	{
		numTransferred = numToTransfer ?? Math.Min(source.stack, destination.maxStack - destination.stack);
		OnStack(destination, source, numTransferred);
		bool isSplittingToHand = numTransferred < source.stack && destination == Main.mouseItem;
		if (source.favorited && !isSplittingToHand)
		{
			destination.favorited = true;
			source.favorited = false;
		}
		destination.stack += numTransferred;
		if (!infiniteSource)
		{
			source.stack -= numTransferred;
		}
	}

	/// <summary>
	/// Calls the GlobalItem.OnStack hooks in <paramref name="destination" />, then the ModItem.OnStack hook in <paramref name="destination" /><br />
	/// OnStack is called before the items are transferred from <paramref name="source" /> to <paramref name="destination" />
	/// </summary>
	/// <param name="destination">The item instance that <paramref name="source" /> will attempt to stack onto</param>
	/// <param name="source">The item instance being stacked onto <paramref name="destination" /></param>
	/// <param name="numToTransfer">The quantity of <paramref name="source" /> that will be transferred to <paramref name="destination" /></param>
	public static void OnStack(Item destination, Item source, int numToTransfer)
	{
		EntityGlobalsEnumerator<GlobalItem> enumerator = HookOnStack.Enumerate(destination).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.OnStack(destination, source, numToTransfer);
		}
		destination.ModItem?.OnStack(source, numToTransfer);
	}

	/// <summary>
	/// Extract up to <paramref name="limit" /> items from <paramref name="source" />. If some items remain, <see cref="M:Terraria.ModLoader.ItemLoader.SplitStack(Terraria.Item,Terraria.Item,System.Int32)" /> will be used.
	/// </summary>
	/// <param name="source">The original item instance</param>
	/// <param name="limit">How many items should be transferred</param>
	public static Item TransferWithLimit(Item source, int limit)
	{
		Item destination = source.Clone();
		if (source.stack <= limit)
		{
			source.TurnToAir();
		}
		else
		{
			SplitStack(destination, source, limit);
		}
		return destination;
	}

	/// <summary>
	/// Called when splitting a stack of items.
	/// </summary>
	/// <param name="destination">
	/// The item instance that <paramref name="source" /> will transfer items to, and is usually a clone of <paramref name="source" />.<br />
	/// This parameter's stack will be set to zero before any transfer occurs.
	/// </param>
	/// <param name="source">The item instance being stacked onto <paramref name="destination" /></param>
	/// <param name="numToTransfer">The quantity of <paramref name="source" /> that will be transferred to <paramref name="destination" /></param>
	public static void SplitStack(Item destination, Item source, int numToTransfer)
	{
		destination.stack = 0;
		destination.favorited = false;
		EntityGlobalsEnumerator<GlobalItem> enumerator = HookSplitStack.Enumerate(destination).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.SplitStack(destination, source, numToTransfer);
		}
		destination.ModItem?.SplitStack(source, numToTransfer);
		destination.stack += numToTransfer;
		source.stack -= numToTransfer;
	}

	/// <summary>
	/// Call all ModItem.ReforgePrice, then GlobalItem.ReforgePrice hooks.
	/// </summary>
	/// <param name="item"></param>
	/// <param name="reforgePrice"></param>
	/// <param name="canApplyDiscount"></param>
	/// <returns></returns>
	public static bool ReforgePrice(Item item, ref int reforgePrice, ref bool canApplyDiscount)
	{
		bool b = item.ModItem?.ReforgePrice(ref reforgePrice, ref canApplyDiscount) ?? true;
		EntityGlobalsEnumerator<GlobalItem> enumerator = HookReforgePrice.Enumerate(item).GetEnumerator();
		while (enumerator.MoveNext())
		{
			GlobalItem g = enumerator.Current;
			b &= g.ReforgePrice(item, ref reforgePrice, ref canApplyDiscount);
		}
		return b;
	}

	/// <summary>
	/// Calls ModItem.CanReforge, then all GlobalItem.CanReforge hooks. If any return false then false is returned.
	/// </summary>
	public static bool CanReforge(Item item)
	{
		bool b = item.ModItem?.CanReforge() ?? true;
		EntityGlobalsEnumerator<GlobalItem> enumerator = HookCanReforge.Enumerate(item).GetEnumerator();
		while (enumerator.MoveNext())
		{
			GlobalItem g = enumerator.Current;
			b &= g.CanReforge(item);
		}
		return b;
	}

	/// <summary>
	/// Calls ModItem.PreReforge, then all GlobalItem.PreReforge hooks.
	/// </summary>
	public static void PreReforge(Item item)
	{
		item.ModItem?.PreReforge();
		EntityGlobalsEnumerator<GlobalItem> enumerator = HookPreReforge.Enumerate(item).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.PreReforge(item);
		}
	}

	/// <summary>
	/// Calls ModItem.PostReforge, then all GlobalItem.PostReforge hooks.
	/// </summary>
	public static void PostReforge(Item item)
	{
		item.ModItem?.PostReforge();
		EntityGlobalsEnumerator<GlobalItem> enumerator = HookPostReforge.Enumerate(item).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.PostReforge(item);
		}
	}

	/// <summary>
	/// Calls the item's equipment texture's DrawArmorColor hook, then all GlobalItem.DrawArmorColor hooks.
	/// </summary>
	public static void DrawArmorColor(EquipType type, int slot, Player drawPlayer, float shadow, ref Color color, ref int glowMask, ref Color glowMaskColor)
	{
		EquipLoader.GetEquipTexture(type, slot)?.DrawArmorColor(drawPlayer, shadow, ref color, ref glowMask, ref glowMaskColor);
		ReadOnlySpan<GlobalItem> readOnlySpan = HookDrawArmorColor.Enumerate();
		for (int i = 0; i < readOnlySpan.Length; i++)
		{
			readOnlySpan[i].DrawArmorColor(type, slot, drawPlayer, shadow, ref color, ref glowMask, ref glowMaskColor);
		}
	}

	/// <summary>
	/// Calls the item's body equipment texture's ArmorArmGlowMask hook, then all GlobalItem.ArmorArmGlowMask hooks.
	/// </summary>
	public static void ArmorArmGlowMask(int slot, Player drawPlayer, float shadow, ref int glowMask, ref Color color)
	{
		EquipLoader.GetEquipTexture(EquipType.Body, slot)?.ArmorArmGlowMask(drawPlayer, shadow, ref glowMask, ref color);
		ReadOnlySpan<GlobalItem> readOnlySpan = HookArmorArmGlowMask.Enumerate();
		for (int i = 0; i < readOnlySpan.Length; i++)
		{
			readOnlySpan[i].ArmorArmGlowMask(slot, drawPlayer, shadow, ref glowMask, ref color);
		}
	}

	/// <summary>
	/// If the player is using wings, this uses the result of GetWing, and calls ModItem.VerticalWingSpeeds then all GlobalItem.VerticalWingSpeeds hooks.
	/// </summary>
	public static void VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising, ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
	{
		Item item = player.equippedWings;
		if (item == null)
		{
			EquipLoader.GetEquipTexture(EquipType.Wings, player.wingsLogic)?.VerticalWingSpeeds(player, ref ascentWhenFalling, ref ascentWhenRising, ref maxCanAscendMultiplier, ref maxAscentMultiplier, ref constantAscend);
			return;
		}
		item.ModItem?.VerticalWingSpeeds(player, ref ascentWhenFalling, ref ascentWhenRising, ref maxCanAscendMultiplier, ref maxAscentMultiplier, ref constantAscend);
		EntityGlobalsEnumerator<GlobalItem> enumerator = HookVerticalWingSpeeds.Enumerate(item).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.VerticalWingSpeeds(item, player, ref ascentWhenFalling, ref ascentWhenRising, ref maxCanAscendMultiplier, ref maxAscentMultiplier, ref constantAscend);
		}
	}

	/// <summary>
	/// If the player is using wings, this uses the result of GetWing, and calls ModItem.HorizontalWingSpeeds then all GlobalItem.HorizontalWingSpeeds hooks.
	/// </summary>
	public static void HorizontalWingSpeeds(Player player)
	{
		Item item = player.equippedWings;
		if (item == null)
		{
			EquipLoader.GetEquipTexture(EquipType.Wings, player.wingsLogic)?.HorizontalWingSpeeds(player, ref player.accRunSpeed, ref player.runAcceleration);
			return;
		}
		item.ModItem?.HorizontalWingSpeeds(player, ref player.accRunSpeed, ref player.runAcceleration);
		EntityGlobalsEnumerator<GlobalItem> enumerator = HookHorizontalWingSpeeds.Enumerate(item).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.HorizontalWingSpeeds(item, player, ref player.accRunSpeed, ref player.runAcceleration);
		}
	}

	/// <summary>
	/// If wings can be seen on the player, calls the player's wing's equipment texture's WingUpdate and all GlobalItem.WingUpdate hooks.
	/// </summary>
	public static bool WingUpdate(Player player, bool inUse)
	{
		if (player.wings <= 0)
		{
			return false;
		}
		bool? retVal = EquipLoader.GetEquipTexture(EquipType.Wings, player.wings)?.WingUpdate(player, inUse);
		ReadOnlySpan<GlobalItem> readOnlySpan = HookWingUpdate.Enumerate();
		for (int i = 0; i < readOnlySpan.Length; i++)
		{
			GlobalItem obj = readOnlySpan[i];
			bool? flag = retVal;
			retVal = obj.WingUpdate(player.wings, player, inUse) | flag;
		}
		return retVal.GetValueOrDefault();
	}

	/// <summary>
	/// Calls ModItem.Update, then all GlobalItem.Update hooks.
	/// </summary>
	public static void Update(Item item, ref float gravity, ref float maxFallSpeed)
	{
		item.ModItem?.Update(ref gravity, ref maxFallSpeed);
		EntityGlobalsEnumerator<GlobalItem> enumerator = HookUpdate.Enumerate(item).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.Update(item, ref gravity, ref maxFallSpeed);
		}
	}

	/// <summary>
	/// Calls ModItem.PostUpdate and all GlobalItem.PostUpdate hooks.
	/// </summary>
	public static void PostUpdate(Item item)
	{
		item.ModItem?.PostUpdate();
		EntityGlobalsEnumerator<GlobalItem> enumerator = HookPostUpdate.Enumerate(item).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.PostUpdate(item);
		}
	}

	/// <summary>
	/// Calls ModItem.GrabRange, then all GlobalItem.GrabRange hooks.
	/// </summary>
	public static void GrabRange(Item item, Player player, ref int grabRange)
	{
		item.ModItem?.GrabRange(player, ref grabRange);
		EntityGlobalsEnumerator<GlobalItem> enumerator = HookGrabRange.Enumerate(item).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.GrabRange(item, player, ref grabRange);
		}
	}

	/// <summary>
	/// Calls all GlobalItem.GrabStyle hooks then ModItem.GrabStyle, until one of them returns true. Returns whether any of the hooks returned true.
	/// </summary>
	public static bool GrabStyle(Item item, Player player)
	{
		EntityGlobalsEnumerator<GlobalItem> enumerator = HookGrabStyle.Enumerate(item).GetEnumerator();
		while (enumerator.MoveNext())
		{
			if (enumerator.Current.GrabStyle(item, player))
			{
				return true;
			}
		}
		if (item.ModItem != null)
		{
			return item.ModItem.GrabStyle(player);
		}
		return false;
	}

	public static bool CanPickup(Item item, Player player)
	{
		EntityGlobalsEnumerator<GlobalItem> enumerator = HookCanPickup.Enumerate(item).GetEnumerator();
		while (enumerator.MoveNext())
		{
			if (!enumerator.Current.CanPickup(item, player))
			{
				return false;
			}
		}
		return item.ModItem?.CanPickup(player) ?? true;
	}

	/// <summary>
	/// Calls all GlobalItem.OnPickup hooks then ModItem.OnPickup, until one of the returns false. Returns true if all of the hooks return true.
	/// </summary>
	public static bool OnPickup(Item item, Player player)
	{
		EntityGlobalsEnumerator<GlobalItem> enumerator = HookOnPickup.Enumerate(item).GetEnumerator();
		while (enumerator.MoveNext())
		{
			if (!enumerator.Current.OnPickup(item, player))
			{
				return false;
			}
		}
		return item.ModItem?.OnPickup(player) ?? true;
	}

	public static bool ItemSpace(Item item, Player player)
	{
		EntityGlobalsEnumerator<GlobalItem> enumerator = HookItemSpace.Enumerate(item).GetEnumerator();
		while (enumerator.MoveNext())
		{
			if (enumerator.Current.ItemSpace(item, player))
			{
				return true;
			}
		}
		return item.ModItem?.ItemSpace(player) ?? false;
	}

	/// <summary>
	/// Calls all GlobalItem.GetAlpha hooks then ModItem.GetAlpha, until one of them returns a color, and returns that color. Returns null if all of the hooks return null.
	/// </summary>
	public static Color? GetAlpha(Item item, Color lightColor)
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		if (item.IsAir)
		{
			return null;
		}
		EntityGlobalsEnumerator<GlobalItem> enumerator = HookGetAlpha.Enumerate(item).GetEnumerator();
		while (enumerator.MoveNext())
		{
			Color? color = enumerator.Current.GetAlpha(item, lightColor);
			if (color.HasValue)
			{
				return color;
			}
		}
		return item.ModItem?.GetAlpha(lightColor);
	}

	/// <summary>
	/// Returns the "and" operator on the results of ModItem.PreDrawInWorld and all GlobalItem.PreDrawInWorld hooks.
	/// </summary>
	public static bool PreDrawInWorld(Item item, SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		bool flag = true;
		if (item.ModItem != null)
		{
			flag &= item.ModItem.PreDrawInWorld(spriteBatch, lightColor, alphaColor, ref rotation, ref scale, whoAmI);
		}
		EntityGlobalsEnumerator<GlobalItem> enumerator = HookPreDrawInWorld.Enumerate(item).GetEnumerator();
		while (enumerator.MoveNext())
		{
			GlobalItem g = enumerator.Current;
			flag &= g.PreDrawInWorld(item, spriteBatch, lightColor, alphaColor, ref rotation, ref scale, whoAmI);
		}
		return flag;
	}

	/// <summary>
	/// Calls ModItem.PostDrawInWorld, then all GlobalItem.PostDrawInWorld hooks.
	/// </summary>
	public static void PostDrawInWorld(Item item, SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		item.ModItem?.PostDrawInWorld(spriteBatch, lightColor, alphaColor, rotation, scale, whoAmI);
		EntityGlobalsEnumerator<GlobalItem> enumerator = HookPostDrawInWorld.Enumerate(item).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.PostDrawInWorld(item, spriteBatch, lightColor, alphaColor, rotation, scale, whoAmI);
		}
	}

	/// <summary>
	/// Returns the "and" operator on the results of all GlobalItem.PreDrawInInventory hooks and ModItem.PreDrawInInventory.
	/// </summary>
	public static bool PreDrawInInventory(Item item, SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		bool flag = true;
		EntityGlobalsEnumerator<GlobalItem> enumerator = HookPreDrawInInventory.Enumerate(item).GetEnumerator();
		while (enumerator.MoveNext())
		{
			GlobalItem g = enumerator.Current;
			flag &= g.PreDrawInInventory(item, spriteBatch, position, frame, drawColor, itemColor, origin, scale);
		}
		if (item.ModItem != null)
		{
			flag &= item.ModItem.PreDrawInInventory(spriteBatch, position, frame, drawColor, itemColor, origin, scale);
		}
		return flag;
	}

	/// <summary>
	/// Calls ModItem.PostDrawInInventory, then all GlobalItem.PostDrawInInventory hooks.
	/// </summary>
	public static void PostDrawInInventory(Item item, SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		item.ModItem?.PostDrawInInventory(spriteBatch, position, frame, drawColor, itemColor, origin, scale);
		EntityGlobalsEnumerator<GlobalItem> enumerator = HookPostDrawInInventory.Enumerate(item).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.PostDrawInInventory(item, spriteBatch, position, frame, drawColor, itemColor, origin, scale);
		}
	}

	public static void HoldoutOffset(float gravDir, int type, ref Vector2 offset)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		ModItem modItem = GetItem(type);
		if (modItem != null)
		{
			Vector2? modOffset2 = modItem.HoldoutOffset();
			if (modOffset2.HasValue)
			{
				offset.X = modOffset2.Value.X;
				offset.Y += gravDir * modOffset2.Value.Y;
			}
		}
		ReadOnlySpan<GlobalItem> readOnlySpan = HookHoldoutOffset.Enumerate(type);
		for (int i = 0; i < readOnlySpan.Length; i++)
		{
			Vector2? modOffset = readOnlySpan[i].HoldoutOffset(type);
			if (modOffset.HasValue)
			{
				offset.X = modOffset.Value.X;
				offset.Y = (float)TextureAssets.Item[type].Value.Height / 2f + gravDir * modOffset.Value.Y;
			}
		}
	}

	public static void HoldoutOrigin(Player player, ref Vector2 origin)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		Item item = player.HeldItem;
		Vector2 modOrigin = Vector2.Zero;
		if (item.ModItem != null)
		{
			Vector2? modOrigin3 = item.ModItem.HoldoutOrigin();
			if (modOrigin3.HasValue)
			{
				modOrigin = modOrigin3.Value;
			}
		}
		EntityGlobalsEnumerator<GlobalItem> enumerator = HookHoldoutOrigin.Enumerate(item).GetEnumerator();
		while (enumerator.MoveNext())
		{
			Vector2? modOrigin2 = enumerator.Current.HoldoutOrigin(item.type);
			if (modOrigin2.HasValue)
			{
				modOrigin = modOrigin2.Value;
			}
		}
		modOrigin.X *= player.direction;
		modOrigin.Y *= 0f - player.gravDir;
		origin += modOrigin;
	}

	public static bool CanEquipAccessory(Item item, int slot, bool modded)
	{
		Player player = Main.player[Main.myPlayer];
		if (item.ModItem != null && !item.ModItem.CanEquipAccessory(player, slot, modded))
		{
			return false;
		}
		EntityGlobalsEnumerator<GlobalItem> enumerator = HookCanEquipAccessory.Enumerate(item).GetEnumerator();
		while (enumerator.MoveNext())
		{
			if (!enumerator.Current.CanEquipAccessory(item, player, slot, modded))
			{
				return false;
			}
		}
		return true;
	}

	public static bool CanAccessoryBeEquippedWith(Item equippedItem, Item incomingItem)
	{
		Player player = Main.player[Main.myPlayer];
		if (CanAccessoryBeEquippedWith(equippedItem, incomingItem, player))
		{
			return CanAccessoryBeEquippedWith(incomingItem, equippedItem, player);
		}
		return false;
	}

	private static bool CanAccessoryBeEquippedWith(Item equippedItem, Item incomingItem, Player player)
	{
		if (equippedItem.ModItem != null && !equippedItem.ModItem.CanAccessoryBeEquippedWith(equippedItem, incomingItem, player))
		{
			return false;
		}
		if (incomingItem.ModItem != null && !incomingItem.ModItem.CanAccessoryBeEquippedWith(equippedItem, incomingItem, player))
		{
			return false;
		}
		EntityGlobalsEnumerator<GlobalItem> enumerator = HookCanAccessoryBeEquippedWith.Enumerate(incomingItem).GetEnumerator();
		while (enumerator.MoveNext())
		{
			if (!enumerator.Current.CanAccessoryBeEquippedWith(equippedItem, incomingItem, player))
			{
				return false;
			}
		}
		return true;
	}

	public static void ExtractinatorUse(ref int resultType, ref int resultStack, int extractType, int extractinatorBlockType)
	{
		GetItem(extractType)?.ExtractinatorUse(extractinatorBlockType, ref resultType, ref resultStack);
		ReadOnlySpan<GlobalItem> readOnlySpan = HookExtractinatorUse.Enumerate();
		for (int i = 0; i < readOnlySpan.Length; i++)
		{
			readOnlySpan[i].ExtractinatorUse(extractType, extractinatorBlockType, ref resultType, ref resultStack);
		}
	}

	public static void CaughtFishStack(Item item)
	{
		item.ModItem?.CaughtFishStack(ref item.stack);
		EntityGlobalsEnumerator<GlobalItem> enumerator = HookCaughtFishStack.Enumerate(item).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.CaughtFishStack(item.type, ref item.stack);
		}
	}

	public static void IsAnglerQuestAvailable(int itemID, ref bool notAvailable)
	{
		ModItem modItem = GetItem(itemID);
		if (modItem != null)
		{
			notAvailable |= !modItem.IsAnglerQuestAvailable();
		}
		ReadOnlySpan<GlobalItem> readOnlySpan = HookIsAnglerQuestAvailable.Enumerate(itemID);
		for (int i = 0; i < readOnlySpan.Length; i++)
		{
			GlobalItem g = readOnlySpan[i];
			notAvailable |= !g.IsAnglerQuestAvailable(itemID);
		}
	}

	public static string AnglerChat(int type)
	{
		string chat = "";
		string catchLocation = "";
		GetItem(type)?.AnglerQuestChat(ref chat, ref catchLocation);
		ReadOnlySpan<GlobalItem> readOnlySpan = HookAnglerChat.Enumerate(type);
		for (int i = 0; i < readOnlySpan.Length; i++)
		{
			readOnlySpan[i].AnglerChat(type, ref chat, ref catchLocation);
		}
		if (string.IsNullOrEmpty(chat) || string.IsNullOrEmpty(catchLocation))
		{
			return null;
		}
		return chat + "\n\n(" + catchLocation + ")";
	}

	public static bool PreDrawTooltip(Item item, ReadOnlyCollection<TooltipLine> lines, ref int x, ref int y)
	{
		bool ret = item.ModItem?.PreDrawTooltip(lines, ref x, ref y) ?? true;
		EntityGlobalsEnumerator<GlobalItem> enumerator = HookPreDrawTooltip.Enumerate(item).GetEnumerator();
		while (enumerator.MoveNext())
		{
			GlobalItem g = enumerator.Current;
			ret &= g.PreDrawTooltip(item, lines, ref x, ref y);
		}
		return ret;
	}

	public static void PostDrawTooltip(Item item, ReadOnlyCollection<DrawableTooltipLine> lines)
	{
		item.ModItem?.PostDrawTooltip(lines);
		EntityGlobalsEnumerator<GlobalItem> enumerator = HookPostDrawTooltip.Enumerate(item).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.PostDrawTooltip(item, lines);
		}
	}

	public static bool PreDrawTooltipLine(Item item, DrawableTooltipLine line, ref int yOffset)
	{
		bool ret = item.ModItem?.PreDrawTooltipLine(line, ref yOffset) ?? true;
		EntityGlobalsEnumerator<GlobalItem> enumerator = HookPreDrawTooltipLine.Enumerate(item).GetEnumerator();
		while (enumerator.MoveNext())
		{
			GlobalItem g = enumerator.Current;
			ret &= g.PreDrawTooltipLine(item, line, ref yOffset);
		}
		return ret;
	}

	public static void PostDrawTooltipLine(Item item, DrawableTooltipLine line)
	{
		item.ModItem?.PostDrawTooltipLine(line);
		EntityGlobalsEnumerator<GlobalItem> enumerator = HookPostDrawTooltipLine.Enumerate(item).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.PostDrawTooltipLine(item, line);
		}
	}

	public static List<TooltipLine> ModifyTooltips(Item item, ref int numTooltips, string[] names, ref string[] text, ref bool[] modifier, ref bool[] badModifier, ref int oneDropLogo, out Color?[] overrideColor, int prefixlineIndex)
	{
		List<TooltipLine> tooltips = new List<TooltipLine>();
		for (int j = 0; j < numTooltips; j++)
		{
			TooltipLine tooltip = new TooltipLine(names[j], text[j]);
			tooltip.IsModifier = modifier[j];
			tooltip.IsModifierBad = badModifier[j];
			if (j == oneDropLogo)
			{
				tooltip.OneDropLogo = true;
			}
			tooltips.Add(tooltip);
		}
		if (item.prefix >= PrefixID.Count && prefixlineIndex != -1)
		{
			IEnumerable<TooltipLine> tooltipLines = PrefixLoader.GetPrefix(item.prefix)?.GetTooltipLines(item);
			if (tooltipLines != null)
			{
				foreach (TooltipLine line in tooltipLines)
				{
					tooltips.Insert(prefixlineIndex, line);
					prefixlineIndex++;
				}
			}
		}
		item.ModItem?.ModifyTooltips(tooltips);
		if (!item.IsAir)
		{
			EntityGlobalsEnumerator<GlobalItem> enumerator2 = HookModifyTooltips.Enumerate(item).GetEnumerator();
			while (enumerator2.MoveNext())
			{
				enumerator2.Current.ModifyTooltips(item, tooltips);
			}
		}
		tooltips.RemoveAll((TooltipLine x) => !x.Visible);
		numTooltips = tooltips.Count;
		text = new string[numTooltips];
		modifier = new bool[numTooltips];
		badModifier = new bool[numTooltips];
		oneDropLogo = -1;
		overrideColor = new Color?[numTooltips];
		for (int i = 0; i < numTooltips; i++)
		{
			text[i] = tooltips[i].Text;
			modifier[i] = tooltips[i].IsModifier;
			badModifier[i] = tooltips[i].IsModifierBad;
			if (tooltips[i].OneDropLogo)
			{
				oneDropLogo = i;
			}
			overrideColor[i] = tooltips[i].OverrideColor;
		}
		return tooltips;
	}

	internal static bool NeedsModSaving(Item item)
	{
		if (item.type <= 0)
		{
			return false;
		}
		if (item.ModItem != null || item.prefix >= PrefixID.Count)
		{
			return true;
		}
		return false;
	}
}
