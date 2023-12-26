using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Initializers;
using Terraria.ModLoader.Core;

namespace Terraria.ModLoader;

/// <summary>
/// This serves as a central place to store equipment slots and their corresponding textures. You will use this to obtain the IDs for your equipment textures.
/// </summary>
public static class EquipLoader
{
	internal static readonly Dictionary<EquipType, int> nextEquip;

	internal static readonly Dictionary<EquipType, Dictionary<int, EquipTexture>> equipTextures;

	internal static readonly Dictionary<int, Dictionary<EquipType, int>> idToSlot;

	internal static readonly Dictionary<EquipType, Dictionary<int, int>> slotToId;

	public static readonly EquipType[] EquipTypes;

	static EquipLoader()
	{
		nextEquip = new Dictionary<EquipType, int>();
		equipTextures = new Dictionary<EquipType, Dictionary<int, EquipTexture>>();
		idToSlot = new Dictionary<int, Dictionary<EquipType, int>>();
		slotToId = new Dictionary<EquipType, Dictionary<int, int>>();
		EquipTypes = (EquipType[])Enum.GetValues(typeof(EquipType));
		EquipType[] equipTypes = EquipTypes;
		foreach (EquipType type in equipTypes)
		{
			nextEquip[type] = GetNumVanilla(type);
			equipTextures[type] = new Dictionary<int, EquipTexture>();
		}
		slotToId[EquipType.Head] = new Dictionary<int, int>();
		slotToId[EquipType.Body] = new Dictionary<int, int>();
		slotToId[EquipType.Legs] = new Dictionary<int, int>();
	}

	internal static int ReserveEquipID(EquipType type)
	{
		return nextEquip[type]++;
	}

	/// <summary>
	/// Gets the equipment texture for the specified equipment type and ID.
	/// </summary>
	/// <param name="type"></param>
	/// <param name="slot"></param>
	/// <returns></returns>
	public static EquipTexture GetEquipTexture(EquipType type, int slot)
	{
		if (!equipTextures[type].TryGetValue(slot, out var texture))
		{
			return null;
		}
		return texture;
	}

	internal static void ResizeAndFillArrays()
	{
		Array.Resize(ref TextureAssets.ArmorHead, nextEquip[EquipType.Head]);
		Array.Resize(ref TextureAssets.ArmorBody, nextEquip[EquipType.Body]);
		Array.Resize(ref TextureAssets.ArmorBodyComposite, nextEquip[EquipType.Body]);
		Array.Resize(ref TextureAssets.FemaleBody, nextEquip[EquipType.Body]);
		Array.Resize(ref TextureAssets.ArmorArm, nextEquip[EquipType.Body]);
		Array.Resize(ref TextureAssets.ArmorLeg, nextEquip[EquipType.Legs]);
		Array.Resize(ref TextureAssets.AccHandsOn, nextEquip[EquipType.HandsOn]);
		Array.Resize(ref TextureAssets.AccHandsOnComposite, nextEquip[EquipType.HandsOn]);
		Array.Resize(ref TextureAssets.AccHandsOff, nextEquip[EquipType.HandsOff]);
		Array.Resize(ref TextureAssets.AccHandsOffComposite, nextEquip[EquipType.HandsOff]);
		Array.Resize(ref TextureAssets.AccBack, nextEquip[EquipType.Back]);
		Array.Resize(ref TextureAssets.AccFront, nextEquip[EquipType.Front]);
		Array.Resize(ref TextureAssets.AccShoes, nextEquip[EquipType.Shoes]);
		Array.Resize(ref TextureAssets.AccWaist, nextEquip[EquipType.Waist]);
		Array.Resize(ref TextureAssets.Wings, nextEquip[EquipType.Wings]);
		Array.Resize(ref TextureAssets.AccShield, nextEquip[EquipType.Shield]);
		Array.Resize(ref TextureAssets.AccNeck, nextEquip[EquipType.Neck]);
		Array.Resize(ref TextureAssets.AccFace, nextEquip[EquipType.Face]);
		Array.Resize(ref TextureAssets.AccBeard, nextEquip[EquipType.Beard]);
		Array.Resize(ref TextureAssets.AccBalloon, nextEquip[EquipType.Balloon]);
		LoaderUtils.ResetStaticMembers(typeof(ArmorIDs));
		WingStatsInitializer.Load();
		EquipType[] equipTypes = EquipTypes;
		foreach (EquipType type in equipTypes)
		{
			foreach (KeyValuePair<int, EquipTexture> entry in equipTextures[type])
			{
				int slot = entry.Key;
				string texture = entry.Value.Texture;
				GetTextureArray(type)[slot] = ModContent.Request<Texture2D>(texture);
				switch (type)
				{
				case EquipType.Body:
					ArmorIDs.Body.Sets.UsesNewFramingCode[slot] = true;
					break;
				case EquipType.HandsOn:
					ArmorIDs.HandOn.Sets.UsesNewFramingCode[slot] = true;
					break;
				case EquipType.HandsOff:
					ArmorIDs.HandOff.Sets.UsesNewFramingCode[slot] = true;
					break;
				}
			}
		}
		ResizeAndRegisterType(EquipType.Head, ref Item.headType);
		ResizeAndRegisterType(EquipType.Body, ref Item.bodyType);
		ResizeAndRegisterType(EquipType.Legs, ref Item.legType);
		static void ResizeAndRegisterType(EquipType equipType, ref int[] typeArray)
		{
			Array.Resize(ref typeArray, nextEquip[equipType]);
			foreach (KeyValuePair<int, int> entry2 in slotToId[equipType])
			{
				typeArray[entry2.Key] = entry2.Value;
			}
		}
	}

	internal static void Unload()
	{
		EquipType[] equipTypes = EquipTypes;
		foreach (EquipType type in equipTypes)
		{
			nextEquip[type] = GetNumVanilla(type);
			equipTextures[type].Clear();
		}
		idToSlot.Clear();
		slotToId[EquipType.Head].Clear();
		slotToId[EquipType.Body].Clear();
		slotToId[EquipType.Legs].Clear();
	}

	internal static int GetNumVanilla(EquipType type)
	{
		return type switch
		{
			EquipType.Head => ArmorIDs.Head.Count, 
			EquipType.Body => ArmorIDs.Body.Count, 
			EquipType.Legs => ArmorIDs.Legs.Count, 
			EquipType.HandsOn => ArmorIDs.HandOn.Count, 
			EquipType.HandsOff => ArmorIDs.HandOff.Count, 
			EquipType.Back => ArmorIDs.Back.Count, 
			EquipType.Front => ArmorIDs.Front.Count, 
			EquipType.Shoes => ArmorIDs.Shoe.Count, 
			EquipType.Waist => ArmorIDs.Waist.Count, 
			EquipType.Wings => ArmorIDs.Wing.Count, 
			EquipType.Shield => ArmorIDs.Shield.Count, 
			EquipType.Neck => ArmorIDs.Neck.Count, 
			EquipType.Face => ArmorIDs.Face.Count, 
			EquipType.Beard => ArmorIDs.Beard.Count, 
			EquipType.Balloon => ArmorIDs.Balloon.Count, 
			_ => 0, 
		};
	}

	internal static Asset<Texture2D>[] GetTextureArray(EquipType type)
	{
		return type switch
		{
			EquipType.Head => TextureAssets.ArmorHead, 
			EquipType.Body => TextureAssets.ArmorBodyComposite, 
			EquipType.Legs => TextureAssets.ArmorLeg, 
			EquipType.HandsOn => TextureAssets.AccHandsOnComposite, 
			EquipType.HandsOff => TextureAssets.AccHandsOffComposite, 
			EquipType.Back => TextureAssets.AccBack, 
			EquipType.Front => TextureAssets.AccFront, 
			EquipType.Shoes => TextureAssets.AccShoes, 
			EquipType.Waist => TextureAssets.AccWaist, 
			EquipType.Wings => TextureAssets.Wings, 
			EquipType.Shield => TextureAssets.AccShield, 
			EquipType.Neck => TextureAssets.AccNeck, 
			EquipType.Face => TextureAssets.AccFace, 
			EquipType.Beard => TextureAssets.AccBeard, 
			EquipType.Balloon => TextureAssets.AccBalloon, 
			_ => null, 
		};
	}

	internal static void SetSlot(Item item)
	{
		if (!idToSlot.TryGetValue(item.type, out var slots))
		{
			return;
		}
		foreach (KeyValuePair<EquipType, int> entry in slots)
		{
			int slot = entry.Value;
			switch (entry.Key)
			{
			case EquipType.Head:
				item.headSlot = slot;
				break;
			case EquipType.Body:
				item.bodySlot = slot;
				break;
			case EquipType.Legs:
				item.legSlot = slot;
				break;
			case EquipType.HandsOn:
				item.handOnSlot = slot;
				break;
			case EquipType.HandsOff:
				item.handOffSlot = slot;
				break;
			case EquipType.Back:
				item.backSlot = slot;
				break;
			case EquipType.Front:
				item.frontSlot = slot;
				break;
			case EquipType.Shoes:
				item.shoeSlot = slot;
				break;
			case EquipType.Waist:
				item.waistSlot = slot;
				break;
			case EquipType.Wings:
				item.wingSlot = slot;
				break;
			case EquipType.Shield:
				item.shieldSlot = slot;
				break;
			case EquipType.Neck:
				item.neckSlot = slot;
				break;
			case EquipType.Face:
				item.faceSlot = slot;
				break;
			case EquipType.Beard:
				item.beardSlot = slot;
				break;
			case EquipType.Balloon:
				item.balloonSlot = slot;
				break;
			}
		}
	}

	internal static int GetPlayerEquip(Player player, EquipType type)
	{
		return type switch
		{
			EquipType.Head => player.head, 
			EquipType.Body => player.body, 
			EquipType.Legs => player.legs, 
			EquipType.HandsOn => player.handon, 
			EquipType.HandsOff => player.handoff, 
			EquipType.Back => player.back, 
			EquipType.Front => player.front, 
			EquipType.Shoes => player.shoe, 
			EquipType.Waist => player.waist, 
			EquipType.Wings => player.wings, 
			EquipType.Shield => player.shield, 
			EquipType.Neck => player.neck, 
			EquipType.Face => player.face, 
			EquipType.Beard => player.beard, 
			EquipType.Balloon => player.balloon, 
			_ => 0, 
		};
	}

	/// <summary>
	/// Adds an equipment texture of the specified type, internal name, and/or associated item to your mod.<br />
	/// If no internal name is provided, the associated item's name will be used instead.<br />
	/// You can then get the ID for your texture by calling EquipLoader.GetEquipTexture, and using the EquipTexture's Slot property.<br />
	/// If you need to override EquipmentTexture's hooks, you can specify the class of the equipment texture class.
	/// </summary>
	/// <remarks>
	/// If both an internal name and associated item are provided, the EquipTexture's name will be set to the internal name, alongside the keys for the equipTexture dictionary.<br />
	/// Additionally, if multiple EquipTextures of the same type are registered for the same item, the first one to be added will be the one automatically displayed on the player and mannequins.
	/// </remarks>
	/// <param name="mod">The mod the equipment texture is from.</param>
	/// <param name="equipTexture">The equip texture.</param>
	/// <param name="item">The item.</param>
	/// <param name="name">The internal name.</param>
	/// <param name="type">The type.</param>
	/// <param name="texture">The texture.</param>
	/// <returns>the ID / slot that is assigned to the equipment texture.</returns>
	public static int AddEquipTexture(Mod mod, string texture, EquipType type, ModItem item = null, string name = null, EquipTexture equipTexture = null)
	{
		if (!mod.loading)
		{
			throw new Exception("AddEquipTexture can only be called from Mod.Load or Mod.Autoload");
		}
		if (name == null && item == null)
		{
			throw new Exception("AddEquipTexture requires either an item or a name be provided");
		}
		if (equipTexture == null)
		{
			equipTexture = new EquipTexture();
		}
		ModContent.Request<Texture2D>(texture);
		equipTexture.Texture = texture;
		equipTexture.Name = name ?? item.Name;
		equipTexture.Type = type;
		equipTexture.Item = item;
		int num2 = (equipTexture.Slot = ReserveEquipID(type));
		int slot = num2;
		equipTextures[type][slot] = equipTexture;
		mod.equipTextures[Tuple.Create(name ?? item.Name, type)] = equipTexture;
		if (item != null)
		{
			if (!idToSlot.TryGetValue(item.Type, out var slots))
			{
				slots = (idToSlot[item.Type] = new Dictionary<EquipType, int>());
			}
			slots[type] = slot;
			if (type == EquipType.Head || type == EquipType.Body || type == EquipType.Legs)
			{
				slotToId[type][slot] = item.Type;
			}
		}
		return slot;
	}

	/// <summary>
	/// Gets the EquipTexture instance corresponding to the name and EquipType. Returns null if no EquipTexture with the given name and EquipType is found.
	/// </summary>
	/// <param name="mod">The mod the equipment texture is from.</param>
	/// <param name="name">The name.</param>
	/// <param name="type">The type.</param>
	/// <returns></returns>
	public static EquipTexture GetEquipTexture(Mod mod, string name, EquipType type)
	{
		if (!mod.equipTextures.TryGetValue(Tuple.Create(name, type), out var texture))
		{
			return null;
		}
		return texture;
	}

	/// <summary>
	/// Gets the slot/ID of the equipment texture corresponding to the given name. Returns -1 if no EquipTexture with the given name is found.
	/// </summary>
	/// <param name="mod">The mod the equipment texture is from.</param>
	/// <param name="name">The name.</param>
	/// <param name="type"></param>
	/// <returns></returns>
	public static int GetEquipSlot(Mod mod, string name, EquipType type)
	{
		return GetEquipTexture(mod, name, type)?.Slot ?? (-1);
	}

	/// <summary>
	/// Hook Player.PlayerFrame
	/// Calls each of the item's equipment texture's FrameEffects hook.
	/// </summary>
	public static void EquipFrameEffects(Player player)
	{
		EquipType[] equipTypes = EquipTypes;
		foreach (EquipType type in equipTypes)
		{
			int slot = GetPlayerEquip(player, type);
			GetEquipTexture(type, slot)?.FrameEffects(player, type);
		}
	}
}
