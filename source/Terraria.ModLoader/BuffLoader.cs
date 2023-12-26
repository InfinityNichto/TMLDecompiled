using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader.Core;

namespace Terraria.ModLoader;

/// <summary>
/// This serves as the central class from which buff-related functions are supported and carried out.
/// </summary>
public static class BuffLoader
{
	private delegate void DelegateUpdatePlayer(int type, Player player, ref int buffIndex);

	private delegate void DelegateUpdateNPC(int type, NPC npc, ref int buffIndex);

	private delegate void DelegateModifyBuffText(int type, ref string buffName, ref string tip, ref int rare);

	private delegate bool DelegatePreDraw(SpriteBatch spriteBatch, int type, int buffIndex, ref BuffDrawParams drawParams);

	private delegate void DelegatePostDraw(SpriteBatch spriteBatch, int type, int buffIndex, BuffDrawParams drawParams);

	private delegate bool DelegateRightClick(int type, int buffIndex);

	internal static int extraPlayerBuffCount;

	private static int nextBuff = BuffID.Count;

	internal static readonly IList<ModBuff> buffs = new List<ModBuff>();

	internal static readonly IList<GlobalBuff> globalBuffs = new List<GlobalBuff>();

	private static DelegateUpdatePlayer[] HookUpdatePlayer;

	private static DelegateUpdateNPC[] HookUpdateNPC;

	private static Func<int, Player, int, int, bool>[] HookReApplyPlayer;

	private static Func<int, NPC, int, int, bool>[] HookReApplyNPC;

	private static DelegateModifyBuffText[] HookModifyBuffText;

	private static Action<string, List<Vector2>>[] HookCustomBuffTipSize;

	private static Action<string, SpriteBatch, int, int>[] HookDrawCustomBuffTip;

	private static DelegatePreDraw[] HookPreDraw;

	private static DelegatePostDraw[] HookPostDraw;

	private static DelegateRightClick[] HookRightClick;

	public static int BuffCount => nextBuff;

	internal static int ReserveBuffID()
	{
		if (ModNet.AllowVanillaClients)
		{
			throw new Exception("Adding buffs breaks vanilla client compatibility");
		}
		int result = nextBuff;
		nextBuff++;
		return result;
	}

	/// <summary>
	/// Gets the ModBuff instance with the given type. If no ModBuff with the given type exists, returns null.
	/// </summary>
	public static ModBuff GetBuff(int type)
	{
		if (type < BuffID.Count || type >= BuffCount)
		{
			return null;
		}
		return buffs[type - BuffID.Count];
	}

	internal static void ResizeArrays()
	{
		Array.Resize(ref TextureAssets.Buff, nextBuff);
		LoaderUtils.ResetStaticMembers(typeof(BuffID));
		Array.Resize(ref Main.pvpBuff, nextBuff);
		Array.Resize(ref Main.persistentBuff, nextBuff);
		Array.Resize(ref Main.vanityPet, nextBuff);
		Array.Resize(ref Main.lightPet, nextBuff);
		Array.Resize(ref Main.meleeBuff, nextBuff);
		Array.Resize(ref Main.debuff, nextBuff);
		Array.Resize(ref Main.buffNoSave, nextBuff);
		Array.Resize(ref Main.buffNoTimeDisplay, nextBuff);
		Array.Resize(ref Main.buffDoubleApply, nextBuff);
		Array.Resize(ref Main.buffAlpha, nextBuff);
		Array.Resize(ref Lang._buffNameCache, nextBuff);
		Array.Resize(ref Lang._buffDescriptionCache, nextBuff);
		for (int i = BuffID.Count; i < nextBuff; i++)
		{
			Lang._buffNameCache[i] = LocalizedText.Empty;
			Lang._buffDescriptionCache[i] = LocalizedText.Empty;
		}
		extraPlayerBuffCount = (ModLoader.Mods.Any() ? ModLoader.Mods.Max((Mod m) => (int)m.ExtraPlayerBuffSlots) : 0);
		ModLoader.BuildGlobalHook<GlobalBuff, DelegateUpdatePlayer>(ref HookUpdatePlayer, globalBuffs, (GlobalBuff g) => g.Update);
		ModLoader.BuildGlobalHook<GlobalBuff, DelegateUpdateNPC>(ref HookUpdateNPC, globalBuffs, (GlobalBuff g) => g.Update);
		ModLoader.BuildGlobalHook<GlobalBuff, Func<int, Player, int, int, bool>>(ref HookReApplyPlayer, globalBuffs, (GlobalBuff g) => g.ReApply);
		ModLoader.BuildGlobalHook<GlobalBuff, Func<int, NPC, int, int, bool>>(ref HookReApplyNPC, globalBuffs, (GlobalBuff g) => g.ReApply);
		ModLoader.BuildGlobalHook<GlobalBuff, DelegateModifyBuffText>(ref HookModifyBuffText, globalBuffs, (GlobalBuff g) => g.ModifyBuffText);
		ModLoader.BuildGlobalHook<GlobalBuff, Action<string, List<Vector2>>>(ref HookCustomBuffTipSize, globalBuffs, (GlobalBuff g) => g.CustomBuffTipSize);
		ModLoader.BuildGlobalHook<GlobalBuff, Action<string, SpriteBatch, int, int>>(ref HookDrawCustomBuffTip, globalBuffs, (GlobalBuff g) => g.DrawCustomBuffTip);
		ModLoader.BuildGlobalHook<GlobalBuff, DelegatePreDraw>(ref HookPreDraw, globalBuffs, (GlobalBuff g) => g.PreDraw);
		ModLoader.BuildGlobalHook<GlobalBuff, DelegatePostDraw>(ref HookPostDraw, globalBuffs, (GlobalBuff g) => g.PostDraw);
		ModLoader.BuildGlobalHook<GlobalBuff, DelegateRightClick>(ref HookRightClick, globalBuffs, (GlobalBuff g) => g.RightClick);
	}

	internal static void PostSetupContent()
	{
		Main.Initialize_BuffDataFromMountData();
	}

	internal static void FinishSetup()
	{
		foreach (ModBuff buff in buffs)
		{
			Lang._buffNameCache[buff.Type] = buff.DisplayName;
			Lang._buffDescriptionCache[buff.Type] = buff.Description;
		}
	}

	internal static void Unload()
	{
		buffs.Clear();
		nextBuff = BuffID.Count;
		globalBuffs.Clear();
	}

	internal static bool IsModBuff(int type)
	{
		return type >= BuffID.Count;
	}

	public static void Update(int buff, Player player, ref int buffIndex)
	{
		int originalIndex = buffIndex;
		if (IsModBuff(buff))
		{
			GetBuff(buff).Update(player, ref buffIndex);
		}
		DelegateUpdatePlayer[] hookUpdatePlayer = HookUpdatePlayer;
		foreach (DelegateUpdatePlayer hook in hookUpdatePlayer)
		{
			if (buffIndex == originalIndex)
			{
				hook(buff, player, ref buffIndex);
				continue;
			}
			break;
		}
	}

	public static void Update(int buff, NPC npc, ref int buffIndex)
	{
		if (IsModBuff(buff))
		{
			GetBuff(buff).Update(npc, ref buffIndex);
		}
		DelegateUpdateNPC[] hookUpdateNPC = HookUpdateNPC;
		for (int i = 0; i < hookUpdateNPC.Length; i++)
		{
			hookUpdateNPC[i](buff, npc, ref buffIndex);
		}
	}

	public static bool ReApply(int buff, Player player, int time, int buffIndex)
	{
		Func<int, Player, int, int, bool>[] hookReApplyPlayer = HookReApplyPlayer;
		for (int i = 0; i < hookReApplyPlayer.Length; i++)
		{
			if (hookReApplyPlayer[i](buff, player, time, buffIndex))
			{
				return true;
			}
		}
		if (IsModBuff(buff))
		{
			return GetBuff(buff).ReApply(player, time, buffIndex);
		}
		return false;
	}

	public static bool ReApply(int buff, NPC npc, int time, int buffIndex)
	{
		Func<int, NPC, int, int, bool>[] hookReApplyNPC = HookReApplyNPC;
		for (int i = 0; i < hookReApplyNPC.Length; i++)
		{
			if (hookReApplyNPC[i](buff, npc, time, buffIndex))
			{
				return true;
			}
		}
		if (IsModBuff(buff))
		{
			return GetBuff(buff).ReApply(npc, time, buffIndex);
		}
		return false;
	}

	public static void ModifyBuffText(int buff, ref string buffName, ref string tip, ref int rare)
	{
		if (IsModBuff(buff))
		{
			GetBuff(buff).ModifyBuffText(ref buffName, ref tip, ref rare);
		}
		DelegateModifyBuffText[] hookModifyBuffText = HookModifyBuffText;
		for (int i = 0; i < hookModifyBuffText.Length; i++)
		{
			hookModifyBuffText[i](buff, ref buffName, ref tip, ref rare);
		}
	}

	public static void CustomBuffTipSize(string buffTip, List<Vector2> sizes)
	{
		Action<string, List<Vector2>>[] hookCustomBuffTipSize = HookCustomBuffTipSize;
		for (int i = 0; i < hookCustomBuffTipSize.Length; i++)
		{
			hookCustomBuffTipSize[i](buffTip, sizes);
		}
	}

	public static void DrawCustomBuffTip(string buffTip, SpriteBatch spriteBatch, int originX, int originY)
	{
		Action<string, SpriteBatch, int, int>[] hookDrawCustomBuffTip = HookDrawCustomBuffTip;
		for (int i = 0; i < hookDrawCustomBuffTip.Length; i++)
		{
			hookDrawCustomBuffTip[i](buffTip, spriteBatch, originX, originY);
		}
	}

	public static bool PreDraw(SpriteBatch spriteBatch, int type, int buffIndex, ref BuffDrawParams drawParams)
	{
		bool result = true;
		DelegatePreDraw[] hookPreDraw = HookPreDraw;
		foreach (DelegatePreDraw hook in hookPreDraw)
		{
			result &= hook(spriteBatch, type, buffIndex, ref drawParams);
		}
		if (result && IsModBuff(type))
		{
			return GetBuff(type).PreDraw(spriteBatch, buffIndex, ref drawParams);
		}
		return result;
	}

	public static void PostDraw(SpriteBatch spriteBatch, int type, int buffIndex, BuffDrawParams drawParams)
	{
		if (IsModBuff(type))
		{
			GetBuff(type).PostDraw(spriteBatch, buffIndex, drawParams);
		}
		DelegatePostDraw[] hookPostDraw = HookPostDraw;
		for (int i = 0; i < hookPostDraw.Length; i++)
		{
			hookPostDraw[i](spriteBatch, type, buffIndex, drawParams);
		}
	}

	public static bool RightClick(int type, int buffIndex)
	{
		bool result = true;
		DelegateRightClick[] hookRightClick = HookRightClick;
		foreach (DelegateRightClick hook in hookRightClick)
		{
			result &= hook(type, buffIndex);
		}
		if (IsModBuff(type))
		{
			result &= GetBuff(type).RightClick(buffIndex);
		}
		return result;
	}
}
