using System;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;
using Terraria.ID;

namespace Terraria.ModLoader;

public static class CombinedHooks
{
	public static void ModifyWeaponDamage(Player player, Item item, ref StatModifier damage)
	{
		ItemLoader.ModifyWeaponDamage(item, player, ref damage);
		PlayerLoader.ModifyWeaponDamage(player, item, ref damage);
	}

	public static void ModifyWeaponCrit(Player player, Item item, ref float crit)
	{
		ItemLoader.ModifyWeaponCrit(item, player, ref crit);
		PlayerLoader.ModifyWeaponCrit(player, item, ref crit);
	}

	public static void ModifyWeaponKnockback(Player player, Item item, ref StatModifier knockback)
	{
		ItemLoader.ModifyWeaponKnockback(item, player, ref knockback);
		PlayerLoader.ModifyWeaponKnockback(player, item, ref knockback);
	}

	public static void ModifyManaCost(Player player, Item item, ref float reduce, ref float mult)
	{
		ItemLoader.ModifyManaCost(item, player, ref reduce, ref mult);
		PlayerLoader.ModifyManaCost(player, item, ref reduce, ref mult);
	}

	public static void OnConsumeMana(Player player, Item item, int manaConsumed)
	{
		ItemLoader.OnConsumeMana(item, player, manaConsumed);
		PlayerLoader.OnConsumeMana(player, item, manaConsumed);
	}

	public static void OnMissingMana(Player player, Item item, int neededMana)
	{
		ItemLoader.OnMissingMana(item, player, neededMana);
		PlayerLoader.OnMissingMana(player, item, neededMana);
	}

	public static bool CanConsumeAmmo(Player player, Item weapon, Item ammo)
	{
		if (PlayerLoader.CanConsumeAmmo(player, weapon, ammo))
		{
			return ItemLoader.CanConsumeAmmo(weapon, ammo, player);
		}
		return false;
	}

	public static void OnConsumeAmmo(Player player, Item weapon, Item ammo)
	{
		PlayerLoader.OnConsumeAmmo(player, weapon, ammo);
		ItemLoader.OnConsumeAmmo(weapon, ammo, player);
	}

	public static bool CanUseItem(Player player, Item item)
	{
		if (PlayerLoader.CanUseItem(player, item))
		{
			return ItemLoader.CanUseItem(item, player);
		}
		return false;
	}

	public static bool? CanAutoReuseItem(Player player, Item item)
	{
		bool? ret = null;
		if (Update(PlayerLoader.CanAutoReuseItem(player, item)))
		{
			Update(ItemLoader.CanAutoReuseItem(item, player));
		}
		else
			_ = 0;
		return ret;
		bool Update(bool? b)
		{
			return !((!(ret ?? (ret = b))) ?? false);
		}
	}

	public static bool CanShoot(Player player, Item item)
	{
		if (PlayerLoader.CanShoot(player, item))
		{
			return ItemLoader.CanShoot(item, player);
		}
		return false;
	}

	public static void ModifyShootStats(Player player, Item item, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
	{
		ItemLoader.ModifyShootStats(item, player, ref position, ref velocity, ref type, ref damage, ref knockback);
		PlayerLoader.ModifyShootStats(player, item, ref position, ref velocity, ref type, ref damage, ref knockback);
	}

	public static bool Shoot(Player player, Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		bool defaultResult = PlayerLoader.Shoot(player, item, source, position, velocity, type, damage, knockback);
		return ItemLoader.Shoot(item, player, source, position, velocity, type, damage, knockback, defaultResult);
	}

	public static bool? CanPlayerHitNPCWithItem(Player player, Item item, NPC npc)
	{
		bool? ret = null;
		if (Update(PlayerLoader.CanHitNPCWithItem(player, item, npc)) && Update(ItemLoader.CanHitNPC(item, player, npc)))
		{
			Update(NPCLoader.CanBeHitByItem(npc, player, item));
		}
		else
			_ = 0;
		return ret;
		bool Update(bool? b)
		{
			return !((!(ret ?? (ret = b))) ?? false);
		}
	}

	public static void ModifyPlayerHitNPCWithItem(Player player, Item sItem, NPC nPC, ref NPC.HitModifiers modifiers)
	{
		ItemLoader.ModifyHitNPC(sItem, player, nPC, ref modifiers);
		NPCLoader.ModifyHitByItem(nPC, player, sItem, ref modifiers);
		PlayerLoader.ModifyHitNPCWithItem(player, sItem, nPC, ref modifiers);
	}

	public static void OnPlayerHitNPCWithItem(Player player, Item sItem, NPC nPC, in NPC.HitInfo hit, int damageDone)
	{
		ItemLoader.OnHitNPC(sItem, player, nPC, in hit, damageDone);
		NPCLoader.OnHitByItem(nPC, player, sItem, in hit, damageDone);
		PlayerLoader.OnHitNPCWithItem(player, sItem, nPC, in hit, damageDone);
	}

	public static bool CanHitPvp(Player player, Item sItem, Player target)
	{
		if (ItemLoader.CanHitPvp(sItem, player, target))
		{
			return PlayerLoader.CanHitPvp(player, sItem, target);
		}
		return false;
	}

	public static void MeleeEffects(Player player, Item sItem, Rectangle itemRectangle)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		ItemLoader.MeleeEffects(sItem, player, itemRectangle);
		PlayerLoader.MeleeEffects(player, sItem, itemRectangle);
	}

	public static bool? CanHitNPCWithProj(Projectile proj, NPC npc)
	{
		bool? ret = null;
		if (Update(proj.TryGetOwner(out Player player) ? PlayerLoader.CanHitNPCWithProj(player, proj, npc) : null) && Update(ProjectileLoader.CanHitNPC(proj, npc)))
		{
			Update(NPCLoader.CanBeHitByProjectile(npc, proj));
		}
		else
			_ = 0;
		return ret;
		bool Update(bool? b)
		{
			return !((!(ret ?? (ret = b))) ?? false);
		}
	}

	public static void ModifyHitNPCWithProj(Projectile projectile, NPC nPC, ref NPC.HitModifiers modifiers)
	{
		ProjectileLoader.ModifyHitNPC(projectile, nPC, ref modifiers);
		NPCLoader.ModifyHitByProjectile(nPC, projectile, ref modifiers);
		if (projectile.TryGetOwner(out Player player))
		{
			PlayerLoader.ModifyHitNPCWithProj(player, projectile, nPC, ref modifiers);
		}
	}

	public static void OnHitNPCWithProj(Projectile projectile, NPC nPC, in NPC.HitInfo hit, int damageDone)
	{
		ProjectileLoader.OnHitNPC(projectile, nPC, in hit, damageDone);
		NPCLoader.OnHitByProjectile(nPC, projectile, in hit, damageDone);
		if (projectile.TryGetOwner(out Player player))
		{
			PlayerLoader.OnHitNPCWithProj(player, projectile, nPC, in hit, damageDone);
		}
	}

	public static bool CanBeHitByProjectile(Player player, Projectile projectile)
	{
		if (ProjectileLoader.CanHitPlayer(projectile, player))
		{
			return PlayerLoader.CanBeHitByProjectile(player, projectile);
		}
		return false;
	}

	public static void ModifyHitByProjectile(Player player, Projectile projectile, ref Player.HurtModifiers modifiers)
	{
		ProjectileLoader.ModifyHitPlayer(projectile, player, ref modifiers);
		PlayerLoader.ModifyHitByProjectile(player, projectile, ref modifiers);
		player.ApplyBannerDefenseBuff(projectile.bannerIdToRespondTo, ref modifiers);
		if (player.resistCold && projectile.coldDamage)
		{
			modifiers.IncomingDamageMultiplier *= 0.7f;
		}
		if (projectile.reflected || ProjectileID.Sets.PlayerHurtDamageIgnoresDifficultyScaling[projectile.type])
		{
			return;
		}
		float damageMult = Main.GameModeInfo.EnemyDamageMultiplier;
		if (Main.GameModeInfo.IsJourneyMode)
		{
			CreativePowers.DifficultySliderPower power = CreativePowerManager.Instance.GetPower<CreativePowers.DifficultySliderPower>();
			if (power.GetIsUnlocked())
			{
				damageMult = power.StrengthMultiplierToGiveNPCs;
			}
		}
		modifiers.SourceDamage *= damageMult;
	}

	public static void OnHitByProjectile(Player player, Projectile projectile, in Player.HurtInfo hurtInfo)
	{
		ProjectileLoader.OnHitPlayer(projectile, player, in hurtInfo);
		PlayerLoader.OnHitByProjectile(player, projectile, in hurtInfo);
	}

	public static bool CanHitPvpWithProj(Projectile projectile, Player target)
	{
		if (ProjectileLoader.CanHitPvp(projectile, target))
		{
			return PlayerLoader.CanHitPvpWithProj(projectile, target);
		}
		return false;
	}

	public static bool? CanPlayerMeleeAttackCollideWithNPC(Player player, Item item, Rectangle meleeAttackHitbox, NPC target)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		bool? ret = null;
		if (Update(PlayerLoader.CanMeleeAttackCollideWithNPC(player, item, meleeAttackHitbox, target)) && Update(ItemLoader.CanMeleeAttackCollideWithNPC(item, meleeAttackHitbox, player, target)))
		{
			Update(NPCLoader.CanCollideWithPlayerMeleeAttack(target, player, item, meleeAttackHitbox));
		}
		else
			_ = 0;
		return ret;
		bool Update(bool? b)
		{
			return !((!(ret ?? (ret = b))) ?? false);
		}
	}

	public static void ModifyItemScale(Player player, Item item, ref float scale)
	{
		ItemLoader.ModifyItemScale(item, player, ref scale);
		PlayerLoader.ModifyItemScale(player, item, ref scale);
	}

	public static float TotalUseSpeedMultiplier(Player player, Item item)
	{
		return PlayerLoader.UseSpeedMultiplier(player, item) * ItemLoader.UseSpeedMultiplier(item, player) * player.GetWeaponAttackSpeed(item);
	}

	public static float TotalUseTimeMultiplier(Player player, Item item)
	{
		float useTimeMult = PlayerLoader.UseTimeMultiplier(player, item) * ItemLoader.UseTimeMultiplier(item, player);
		if (!item.attackSpeedOnlyAffectsWeaponAnimation)
		{
			useTimeMult /= TotalUseSpeedMultiplier(player, item);
		}
		return useTimeMult;
	}

	public static int TotalUseTime(float useTime, Player player, Item item)
	{
		return Math.Max(1, (int)(useTime * TotalUseTimeMultiplier(player, item)));
	}

	public static float TotalUseAnimationMultiplier(Player player, Item item)
	{
		float num = PlayerLoader.UseAnimationMultiplier(player, item) * ItemLoader.UseAnimationMultiplier(item, player);
		int multipliedUseTime = Math.Max(1, (int)((float)item.useTime * (1f / TotalUseSpeedMultiplier(player, item))));
		int relativeUseAnimation = Math.Max(1, multipliedUseTime * item.useAnimation / item.useTime);
		return num * ((float)relativeUseAnimation / (float)item.useAnimation);
	}

	public static int TotalAnimationTime(float useAnimation, Player player, Item item)
	{
		return Math.Max(1, (int)(useAnimation * TotalUseAnimationMultiplier(player, item)));
	}

	public static bool? CanConsumeBait(Player player, Item item)
	{
		bool? ret = PlayerLoader.CanConsumeBait(player, item);
		bool? flag = ItemLoader.CanConsumeBait(player, item);
		if (flag.HasValue)
		{
			bool b = flag.GetValueOrDefault();
			ret = (ret ?? true) && b;
		}
		return ret;
	}

	public static bool? CanCatchNPC(Player player, NPC npc, Item item)
	{
		bool? ret = null;
		if (Update(PlayerLoader.CanCatchNPC(player, npc, item)) && Update(ItemLoader.CanCatchNPC(item, npc, player)))
		{
			Update(NPCLoader.CanBeCaughtBy(npc, item, player));
		}
		else
			_ = 0;
		return ret;
		bool Update(bool? b)
		{
			return !((!(ret ?? (ret = b))) ?? false);
		}
	}

	public static void OnCatchNPC(Player player, NPC npc, Item item, bool failed)
	{
		PlayerLoader.OnCatchNPC(player, npc, item, failed);
		ItemLoader.OnCatchNPC(item, npc, player, failed);
		NPCLoader.OnCaughtBy(npc, player, item, failed);
	}

	public static bool CanNPCHitPlayer(NPC nPC, Player player, ref int specialHitSetter)
	{
		if (NPCLoader.CanHitPlayer(nPC, player, ref specialHitSetter))
		{
			return PlayerLoader.CanBeHitByNPC(player, nPC, ref specialHitSetter);
		}
		return false;
	}

	public static void ModifyHitByNPC(Player player, NPC nPC, ref Player.HurtModifiers modifiers)
	{
		NPCLoader.ModifyHitPlayer(nPC, player, ref modifiers);
		PlayerLoader.ModifyHitByNPC(player, nPC, ref modifiers);
		player.ApplyBannerDefenseBuff(nPC, ref modifiers);
		if (player.resistCold && nPC.coldDamage)
		{
			modifiers.IncomingDamageMultiplier *= 0.7f;
		}
	}

	public static void OnHitByNPC(Player player, NPC nPC, in Player.HurtInfo hurtInfo)
	{
		NPCLoader.OnHitPlayer(nPC, player, hurtInfo);
		PlayerLoader.OnHitByNPC(player, nPC, in hurtInfo);
	}

	public static void PlayerFrameEffects(Player player)
	{
		PlayerLoader.FrameEffects(player);
		EquipLoader.EquipFrameEffects(player);
	}

	public static bool OnPickup(Item item, Player player)
	{
		if (ItemLoader.OnPickup(item, player))
		{
			return PlayerLoader.OnPickup(player, item);
		}
		return false;
	}
}
