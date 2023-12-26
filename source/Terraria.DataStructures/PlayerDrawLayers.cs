using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.GameContent;
using Terraria.GameContent.Events;
using Terraria.GameContent.Liquid;
using Terraria.Graphics;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace Terraria.DataStructures;

public static class PlayerDrawLayers
{
	private const int DEFAULT_MAX_SPRITES = 200;

	private static SpriteDrawBuffer spriteBuffer;

	/// <summary> Adds <see cref="F:Terraria.DataStructures.PlayerDrawSet.torsoOffset" /> to <see cref="F:Terraria.DataStructures.PlayerDrawSet.Position" /> and <see cref="F:Terraria.DataStructures.PlayerDrawSet.ItemLocation" /> vectors' Y axes. </summary>
	public static readonly PlayerDrawLayer.Transformation TorsoGroup = new VanillaPlayerDrawTransform(DrawPlayer_extra_TorsoPlus, DrawPlayer_extra_TorsoMinus);

	/// <summary> Adds <see cref="F:Terraria.DataStructures.PlayerDrawSet.mountOffSet" />/2 to <see cref="F:Terraria.DataStructures.PlayerDrawSet.Position" /> vector's Y axis. </summary>
	public static readonly PlayerDrawLayer.Transformation MountGroup = new VanillaPlayerDrawTransform(DrawPlayer_extra_MountPlus, DrawPlayer_extra_MountMinus, TorsoGroup);

	/// <summary> Draws Jim's Cloak, if the player is wearing Jim's Leggings (a developer item). </summary>
	public static readonly PlayerDrawLayer JimsCloak = new VanillaPlayerDrawLayer("JimsCloak", DrawPlayer_01_2_JimsCloak, TorsoGroup);

	/// <summary> Draws the back textures of the player's mount. </summary>
	public static readonly PlayerDrawLayer MountBack = new VanillaPlayerDrawLayer("MountBack", DrawPlayer_02_MountBehindPlayer);

	/// <summary> Draws the Flying Carpet accessory, if the player has it equipped and is using it. </summary>
	public static readonly PlayerDrawLayer Carpet = new VanillaPlayerDrawLayer("Carpet", DrawPlayer_03_Carpet);

	/// <summary> Draws the Step Stool accessory, if the player has it equipped and is using it. </summary>
	public static readonly PlayerDrawLayer PortableStool = new VanillaPlayerDrawLayer("PortableStool", DrawPlayer_03_PortableStool);

	/// <summary> Draws the back textures of the Electrified debuff, if the player has it. </summary>
	public static readonly PlayerDrawLayer ElectrifiedDebuffBack = new VanillaPlayerDrawLayer("ElectrifiedDebuffBack", DrawPlayer_04_ElectrifiedDebuffBack, TorsoGroup);

	/// <summary> Draws the 'Forbidden Sign' if the player has a full 'Forbidden Armor' set equipped. </summary>
	public static readonly PlayerDrawLayer ForbiddenSetRing = new VanillaPlayerDrawLayer("ForbiddenSetRing", DrawPlayer_05_ForbiddenSetRing, TorsoGroup);

	/// <summary> Draws a sun above the player's head if they have "Safeman's Sunny Day" headgear equipped. </summary>
	public static readonly PlayerDrawLayer SafemanSun = new VanillaPlayerDrawLayer("SafemanSun", DrawPlayer_05_2_SafemanSun, TorsoGroup);

	/// <summary> Draws the back textures of the Webbed debuff, if the player has it. </summary>
	public static readonly PlayerDrawLayer WebbedDebuffBack = new VanillaPlayerDrawLayer("WebbedDebuffBack", DrawPlayer_06_WebbedDebuffBack, TorsoGroup);

	/// <summary> Draws effects of "Leinfors' Luxury Shampoo", if the player has it equipped. </summary>
	public static readonly PlayerDrawLayer LeinforsHairShampoo = new VanillaPlayerDrawLayer("LeinforsHairShampoo", DrawPlayer_07_LeinforsHairShampoo, TorsoGroup, isHeadLayer: true);

	/// <summary> Draws the player's held item's backpack. </summary>
	public static readonly PlayerDrawLayer Backpacks = new VanillaPlayerDrawLayer("Backpacks", DrawPlayer_08_Backpacks);

	/// <summary> Draws the player's tails vanities. </summary>
	public static readonly PlayerDrawLayer Tails = new VanillaPlayerDrawLayer("Tails", DrawPlayer_08_1_Tails, TorsoGroup);

	/// <summary> Draws the player's wings. </summary>
	public static readonly PlayerDrawLayer Wings = new VanillaPlayerDrawLayer("Wings", DrawPlayer_09_Wings);

	/// <summary> Draws the player's under-headgear hair. </summary>
	public static readonly PlayerDrawLayer HairBack = new VanillaPlayerDrawLayer("HairBack", DrawPlayer_01_BackHair, TorsoGroup, isHeadLayer: true);

	/// <summary> Draws the player's back accessories. </summary>
	public static readonly PlayerDrawLayer BackAcc = new VanillaPlayerDrawLayer("BackAcc", DrawPlayer_10_BackAcc, TorsoGroup);

	/// <summary> Draws the back textures of the player's head, including armor. </summary>
	public static readonly PlayerDrawLayer HeadBack = new VanillaPlayerDrawLayer("HeadBack", DrawPlayer_01_3_BackHead, TorsoGroup, isHeadLayer: true);

	/// <summary> Draws the player's balloon accessory, if they have one. </summary>
	public static readonly PlayerDrawLayer BalloonAcc = new VanillaPlayerDrawLayer("BalloonAcc", DrawPlayer_11_Balloons);

	/// <summary> Draws the player's body and leg skin. </summary>
	public static readonly PlayerDrawLayer Skin = new VanillaPlayerDrawLayer("Skin", DrawPlayer_12_Skin);

	/// <summary> Draws the player's leg armor or pants and shoes. </summary>
	public static readonly PlayerDrawLayer Leggings = new VanillaPlayerDrawLayer("Leggings", DrawPlayer_13_Leggings, null, isHeadLayer: false, (PlayerDrawSet drawinfo) => !drawinfo.drawPlayer.wearsRobe || drawinfo.drawPlayer.body == 166);

	/// <summary> Draws the player's shoes. </summary>
	public static readonly PlayerDrawLayer Shoes = new VanillaPlayerDrawLayer("Shoes", DrawPlayer_14_Shoes);

	/// <summary> Draws the player's robe. </summary>
	public static readonly PlayerDrawLayer Robe = new VanillaPlayerDrawLayer("Robe", DrawPlayer_13_Leggings, null, isHeadLayer: false, (PlayerDrawSet drawinfo) => drawinfo.drawPlayer.wearsRobe && drawinfo.drawPlayer.body != 166);

	/// <summary> Draws the longcoat default clothing style, if the player has it. </summary>
	public static readonly PlayerDrawLayer SkinLongCoat = new VanillaPlayerDrawLayer("SkinLongCoat", DrawPlayer_15_SkinLongCoat, TorsoGroup);

	/// <summary> Draws the currently equipped armor's longcoat, if it has one. </summary>
	public static readonly PlayerDrawLayer ArmorLongCoat = new VanillaPlayerDrawLayer("ArmorLongCoat", DrawPlayer_16_ArmorLongCoat, TorsoGroup);

	/// <summary> Draws the player's body armor or shirts. </summary>
	public static readonly PlayerDrawLayer Torso = new VanillaPlayerDrawLayer("Torso", DrawPlayer_17_Torso, TorsoGroup);

	/// <summary> Draws the player's off-hand accessory. </summary>
	public static readonly PlayerDrawLayer OffhandAcc = new VanillaPlayerDrawLayer("OffhandAcc", DrawPlayer_18_OffhandAcc, TorsoGroup);

	/// <summary> Draws the player's waist accessory. </summary>
	public static readonly PlayerDrawLayer WaistAcc = new VanillaPlayerDrawLayer("WaistAcc", DrawPlayer_19_WaistAcc, TorsoGroup);

	/// <summary> Draws the player's neck accessory. </summary>
	public static readonly PlayerDrawLayer NeckAcc = new VanillaPlayerDrawLayer("NeckAcc", DrawPlayer_20_NeckAcc, TorsoGroup);

	/// <summary> Draws the player's head, including hair, armor, and etc. </summary>
	public static readonly PlayerDrawLayer Head = new VanillaPlayerDrawLayer("Head", DrawPlayer_21_Head, TorsoGroup, isHeadLayer: true);

	/// <summary> Draws a finch nest on the player's head, if the player has a finch summoned. </summary>
	public static readonly PlayerDrawLayer FinchNest = new VanillaPlayerDrawLayer("FinchNest", DrawPlayer_21_2_FinchNest, TorsoGroup);

	/// <summary> Draws the player's face accessory. </summary>
	public static readonly PlayerDrawLayer FaceAcc = new VanillaPlayerDrawLayer("FaceAcc", DrawPlayer_22_FaceAcc, TorsoGroup, isHeadLayer: true);

	/// <summary> Draws the front textures of the player's mount. </summary>
	public static readonly PlayerDrawLayer MountFront = new VanillaPlayerDrawLayer("MountFront", DrawPlayer_23_MountFront, TorsoGroup);

	/// <summary> Draws the pulley if the player is hanging on a rope. </summary>
	public static readonly PlayerDrawLayer Pulley = new VanillaPlayerDrawLayer("Pulley", DrawPlayer_24_Pulley, TorsoGroup);

	/// <summary> Draws the pulley if the player is hanging on a rope. </summary>
	public static readonly PlayerDrawLayer JimsDroneRadio = new VanillaPlayerDrawLayer("JimsDroneRadio", DrawPlayer_JimsDroneRadio, TorsoGroup);

	/// <summary> Draws the back part of player's front accessory. </summary>
	public static readonly PlayerDrawLayer FrontAccBack = new VanillaPlayerDrawLayer("FrontAccBack", DrawPlayer_32_FrontAcc_BackPart, TorsoGroup);

	/// <summary> Draws the player's shield accessory. </summary>
	public static readonly PlayerDrawLayer Shield = new VanillaPlayerDrawLayer("Shield", DrawPlayer_25_Shield, TorsoGroup);

	/// <summary> Draws the player's solar shield if the player has one. </summary>
	public static readonly PlayerDrawLayer SolarShield = new VanillaPlayerDrawLayer("SolarShield", DrawPlayer_26_SolarShield, MountGroup);

	/// <summary> Draws the player's main arm (including the armor's if applicable), when it should appear over the held item. </summary>
	public static readonly PlayerDrawLayer ArmOverItem = new VanillaPlayerDrawLayer("ArmOverItem", DrawPlayer_28_ArmOverItem, TorsoGroup);

	/// <summary> Draws the player's hand on accessory. </summary>
	public static readonly PlayerDrawLayer HandOnAcc = new VanillaPlayerDrawLayer("HandOnAcc", DrawPlayer_29_OnhandAcc, TorsoGroup);

	/// <summary> Draws the Bladed Glove item, if the player is currently using it. </summary>
	public static readonly PlayerDrawLayer BladedGlove = new VanillaPlayerDrawLayer("BladedGlove", DrawPlayer_30_BladedGlove, TorsoGroup);

	/// <summary> Draws the player's held projectile, if it should be drawn in front of the held item and arms. </summary>
	public static readonly PlayerDrawLayer ProjectileOverArm = new VanillaPlayerDrawLayer("ProjectileOverArm", DrawPlayer_31_ProjectileOverArm);

	/// <summary> Draws the front textures of either Frozen or Webbed debuffs, if the player has one of them. </summary>
	public static readonly PlayerDrawLayer FrozenOrWebbedDebuff = new VanillaPlayerDrawLayer("FrozenOrWebbedDebuff", DrawPlayer_33_FrozenOrWebbedDebuff);

	/// <summary> Draws the front textures of the Electrified debuff, if the player has it. </summary>
	public static readonly PlayerDrawLayer ElectrifiedDebuffFront = new VanillaPlayerDrawLayer("ElectrifiedDebuffFront", DrawPlayer_34_ElectrifiedDebuffFront);

	/// <summary> Draws the textures of the Ice Barrier buff, if the player has it. </summary>
	public static readonly PlayerDrawLayer IceBarrier = new VanillaPlayerDrawLayer("IceBarrier", DrawPlayer_35_IceBarrier);

	/// <summary> Draws a big gem above the player, if the player is currently in possession of a 'Capture The Gem' gem item. </summary>
	public static readonly PlayerDrawLayer CaptureTheGem = new VanillaPlayerDrawLayer("CaptureTheGem", DrawPlayer_36_CTG);

	/// <summary> Draws the effects of Beetle Armor's Set buffs, if the player currently has any. </summary>
	public static readonly PlayerDrawLayer BeetleBuff = new VanillaPlayerDrawLayer("BeetleBuff", DrawPlayer_37_BeetleBuff);

	/// <summary> Draws the effects of Eyebrella Cloud, if the player currently has it. </summary>
	public static readonly PlayerDrawLayer EyebrellaCloud = new VanillaPlayerDrawLayer("EyebrellaCloud", DrawPlayer_38_EyebrellaCloud);

	/// <summary> Draws the front part of player's front accessory. </summary>
	public static readonly PlayerDrawLayer FrontAccFront = new VanillaPlayerDrawLayer("FrontAccFront", DrawPlayer_32_FrontAcc_FrontPart, null, isHeadLayer: false, null, new PlayerDrawLayer.Multiple
	{
		{
			new PlayerDrawLayer.Between(FaceAcc, MountFront),
			(PlayerDrawSet drawinfo) => drawinfo.drawFrontAccInNeckAccLayer
		},
		{
			new PlayerDrawLayer.Between(BladedGlove, ProjectileOverArm),
			(PlayerDrawSet drawinfo) => !drawinfo.drawFrontAccInNeckAccLayer
		}
	});

	/// <summary> Draws the player's held item. </summary>
	public static readonly PlayerDrawLayer HeldItem = new VanillaPlayerDrawLayer("HeldItem", DrawPlayer_27_HeldItem, null, isHeadLayer: false, null, new PlayerDrawLayer.Multiple
	{
		{
			new PlayerDrawLayer.Between(BalloonAcc, Skin),
			(PlayerDrawSet drawinfo) => drawinfo.weaponDrawOrder == WeaponDrawOrder.BehindBackArm
		},
		{
			new PlayerDrawLayer.Between(SolarShield, ArmOverItem),
			(PlayerDrawSet drawinfo) => drawinfo.weaponDrawOrder == WeaponDrawOrder.BehindFrontArm
		},
		{
			new PlayerDrawLayer.Between(BladedGlove, ProjectileOverArm),
			(PlayerDrawSet drawinfo) => drawinfo.weaponDrawOrder == WeaponDrawOrder.OverFrontArm
		}
	});

	internal static IReadOnlyList<PlayerDrawLayer> VanillaLayers = FixedVanillaLayers.Concat(new PlayerDrawLayer[2] { FrontAccFront, HeldItem }).ToArray();

	internal static IReadOnlyList<PlayerDrawLayer> FixedVanillaLayers => new PlayerDrawLayer[45]
	{
		JimsCloak, MountBack, Carpet, PortableStool, ElectrifiedDebuffBack, ForbiddenSetRing, SafemanSun, WebbedDebuffBack, LeinforsHairShampoo, Backpacks,
		Tails, Wings, HairBack, BackAcc, HeadBack, BalloonAcc, Skin, Leggings, Shoes, Robe,
		SkinLongCoat, ArmorLongCoat, Torso, OffhandAcc, WaistAcc, NeckAcc, Head, FinchNest, FaceAcc, MountFront,
		Pulley, JimsDroneRadio, FrontAccBack, Shield, SolarShield, ArmOverItem, HandOnAcc, BladedGlove, ProjectileOverArm, FrozenOrWebbedDebuff,
		ElectrifiedDebuffFront, IceBarrier, CaptureTheGem, BeetleBuff, EyebrellaCloud
	};

	public static PlayerDrawLayer FirstVanillaLayer => FixedVanillaLayers[0];

	public static PlayerDrawLayer LastVanillaLayer
	{
		get
		{
			IReadOnlyList<PlayerDrawLayer> fixedVanillaLayers = FixedVanillaLayers;
			return fixedVanillaLayers[fixedVanillaLayers.Count - 1];
		}
	}

	public static PlayerDrawLayer.Between BeforeFirstVanillaLayer => new PlayerDrawLayer.Between(null, FirstVanillaLayer);

	public static PlayerDrawLayer.Between AfterLastVanillaLayer => new PlayerDrawLayer.Between(LastVanillaLayer, null);

	public static void DrawPlayer_extra_TorsoPlus(ref PlayerDrawSet drawinfo)
	{
		drawinfo.Position.Y += drawinfo.torsoOffset;
		drawinfo.ItemLocation.Y += drawinfo.torsoOffset;
	}

	public static void DrawPlayer_extra_TorsoMinus(ref PlayerDrawSet drawinfo)
	{
		drawinfo.Position.Y -= drawinfo.torsoOffset;
		drawinfo.ItemLocation.Y -= drawinfo.torsoOffset;
	}

	public static void DrawPlayer_extra_MountPlus(ref PlayerDrawSet drawinfo)
	{
		drawinfo.Position.Y += (int)drawinfo.mountOffSet / 2;
	}

	public static void DrawPlayer_extra_MountMinus(ref PlayerDrawSet drawinfo)
	{
		drawinfo.Position.Y -= (int)drawinfo.mountOffSet / 2;
	}

	public static void DrawCompositeArmorPiece(ref PlayerDrawSet drawinfo, CompositePlayerDrawContext context, DrawData data)
	{
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_0269: Unknown result type (might be due to invalid IL or missing references)
		//IL_026e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0279: Unknown result type (might be due to invalid IL or missing references)
		//IL_027e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0284: Unknown result type (might be due to invalid IL or missing references)
		//IL_0289: Unknown result type (might be due to invalid IL or missing references)
		//IL_028e: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_030c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0314: Unknown result type (might be due to invalid IL or missing references)
		//IL_031f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0330: Unknown result type (might be due to invalid IL or missing references)
		//IL_033b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0198: Unknown result type (might be due to invalid IL or missing references)
		//IL_019a: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03da: Unknown result type (might be due to invalid IL or missing references)
		//IL_03df: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0425: Unknown result type (might be due to invalid IL or missing references)
		//IL_0427: Unknown result type (might be due to invalid IL or missing references)
		drawinfo.DrawDataCache.Add(data);
		switch (context)
		{
		case CompositePlayerDrawContext.BackShoulder:
		case CompositePlayerDrawContext.BackArm:
		case CompositePlayerDrawContext.FrontArm:
		case CompositePlayerDrawContext.FrontShoulder:
		{
			if (((Color)(ref drawinfo.armGlowColor)).PackedValue == 0)
			{
				break;
			}
			DrawData item2 = data;
			item2.color = drawinfo.armGlowColor;
			Rectangle value2 = item2.sourceRect.Value;
			value2.Y += 224;
			item2.sourceRect = value2;
			if (drawinfo.drawPlayer.body == 227)
			{
				Vector2 position2 = item2.position;
				Vector2 vector2 = default(Vector2);
				for (int j = 0; j < 2; j++)
				{
					((Vector2)(ref vector2))._002Ector((float)Main.rand.Next(-10, 10) * 0.125f, (float)Main.rand.Next(-10, 10) * 0.125f);
					item2.position = position2 + vector2;
					if (j == 0)
					{
						drawinfo.DrawDataCache.Add(item2);
					}
				}
			}
			drawinfo.DrawDataCache.Add(item2);
			break;
		}
		case CompositePlayerDrawContext.Torso:
		{
			if (((Color)(ref drawinfo.bodyGlowColor)).PackedValue == 0)
			{
				break;
			}
			DrawData item = data;
			item.color = drawinfo.bodyGlowColor;
			Rectangle value = item.sourceRect.Value;
			value.Y += 224;
			item.sourceRect = value;
			if (drawinfo.drawPlayer.body == 227)
			{
				Vector2 position = item.position;
				Vector2 vector = default(Vector2);
				for (int i = 0; i < 2; i++)
				{
					((Vector2)(ref vector))._002Ector((float)Main.rand.Next(-10, 10) * 0.125f, (float)Main.rand.Next(-10, 10) * 0.125f);
					item.position = position + vector;
					if (i == 0)
					{
						drawinfo.DrawDataCache.Add(item);
					}
				}
			}
			drawinfo.DrawDataCache.Add(item);
			break;
		}
		}
		if (context == CompositePlayerDrawContext.FrontShoulder && drawinfo.drawPlayer.head == 269)
		{
			Vector2 position3 = drawinfo.helmetOffset + new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.headPosition + drawinfo.headVect;
			DrawData item3 = new DrawData(TextureAssets.Extra[214].Value, position3, drawinfo.drawPlayer.bodyFrame, drawinfo.colorArmorHead, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect);
			item3.shader = drawinfo.cHead;
			drawinfo.DrawDataCache.Add(item3);
			item3 = new DrawData(TextureAssets.GlowMask[308].Value, position3, drawinfo.drawPlayer.bodyFrame, drawinfo.headGlowColor, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect);
			item3.shader = drawinfo.cHead;
			drawinfo.DrawDataCache.Add(item3);
		}
		if (context == CompositePlayerDrawContext.FrontArm && drawinfo.drawPlayer.body == 205)
		{
			Color color = default(Color);
			((Color)(ref color))._002Ector(100, 100, 100, 0);
			ulong seed = (ulong)(drawinfo.drawPlayer.miscCounter / 4);
			int num = 4;
			for (int k = 0; k < num; k++)
			{
				float num2 = (float)Utils.RandomInt(ref seed, -10, 11) * 0.2f;
				float num3 = (float)Utils.RandomInt(ref seed, -10, 1) * 0.15f;
				DrawData item4 = data;
				Rectangle value3 = item4.sourceRect.Value;
				value3.Y += 224;
				item4.sourceRect = value3;
				item4.position.X += num2;
				item4.position.Y += num3;
				item4.color = color;
				drawinfo.DrawDataCache.Add(item4);
			}
		}
	}

	public static void DrawPlayer_01_BackHair(ref PlayerDrawSet drawinfo)
	{
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		if (!drawinfo.hideHair && drawinfo.backHairDraw)
		{
			Vector2 position = new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.headPosition + drawinfo.headVect + drawinfo.hairOffset;
			if (drawinfo.drawPlayer.head == -1 || drawinfo.fullHair || drawinfo.drawsBackHairWithoutHeadgear)
			{
				DrawData item = new DrawData(TextureAssets.PlayerHair[drawinfo.drawPlayer.hair].Value, position, drawinfo.hairBackFrame, drawinfo.colorHair, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect);
				item.shader = drawinfo.hairDyePacked;
				drawinfo.DrawDataCache.Add(item);
			}
			else if (drawinfo.hatHair)
			{
				DrawData item2 = new DrawData(TextureAssets.PlayerHairAlt[drawinfo.drawPlayer.hair].Value, position, drawinfo.hairBackFrame, drawinfo.colorHair, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect);
				item2.shader = drawinfo.hairDyePacked;
				drawinfo.DrawDataCache.Add(item2);
			}
		}
	}

	public static void DrawPlayer_02_MountBehindPlayer(ref PlayerDrawSet drawinfo)
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		if (drawinfo.drawPlayer.mount.Active)
		{
			DrawMeowcartTrail(ref drawinfo);
			DrawTiedBalloons(ref drawinfo);
			drawinfo.drawPlayer.mount.Draw(drawinfo.DrawDataCache, 0, drawinfo.drawPlayer, drawinfo.Position, drawinfo.colorMount, drawinfo.playerEffect, drawinfo.shadow);
			drawinfo.drawPlayer.mount.Draw(drawinfo.DrawDataCache, 1, drawinfo.drawPlayer, drawinfo.Position, drawinfo.colorMount, drawinfo.playerEffect, drawinfo.shadow);
		}
	}

	public static void DrawPlayer_03_Carpet(ref PlayerDrawSet drawinfo)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		if (drawinfo.drawPlayer.carpetFrame >= 0)
		{
			Color colorArmorLegs = drawinfo.colorArmorLegs;
			float num = 0f;
			if (drawinfo.drawPlayer.gravDir == -1f)
			{
				num = 10f;
			}
			DrawData item = new DrawData(TextureAssets.FlyingCarpet.Value, new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)(drawinfo.drawPlayer.height / 2) + 28f * drawinfo.drawPlayer.gravDir + num)), (Rectangle?)new Rectangle(0, TextureAssets.FlyingCarpet.Height() / 6 * drawinfo.drawPlayer.carpetFrame, TextureAssets.FlyingCarpet.Width(), TextureAssets.FlyingCarpet.Height() / 6), colorArmorLegs, drawinfo.drawPlayer.bodyRotation, new Vector2((float)(TextureAssets.FlyingCarpet.Width() / 2), (float)(TextureAssets.FlyingCarpet.Height() / 8)), 1f, drawinfo.playerEffect, 0f);
			item.shader = drawinfo.cCarpet;
			drawinfo.DrawDataCache.Add(item);
		}
	}

	public static void DrawPlayer_03_PortableStool(ref PlayerDrawSet drawinfo)
	{
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		if (drawinfo.drawPlayer.portableStoolInfo.IsInUse)
		{
			Texture2D value = TextureAssets.Extra[102].Value;
			Vector2 position = default(Vector2);
			((Vector2)(ref position))._002Ector((float)(int)(drawinfo.Position.X - Main.screenPosition.X + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height + 28f));
			Rectangle rectangle = value.Frame();
			Vector2 origin = rectangle.Size() * new Vector2(0.5f, 1f);
			DrawData item = new DrawData(value, position, rectangle, drawinfo.colorArmorLegs, drawinfo.drawPlayer.bodyRotation, origin, 1f, drawinfo.playerEffect);
			item.shader = drawinfo.cPortableStool;
			drawinfo.DrawDataCache.Add(item);
		}
	}

	public static void DrawPlayer_04_ElectrifiedDebuffBack(ref PlayerDrawSet drawinfo)
	{
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		if (!drawinfo.drawPlayer.electrified || drawinfo.shadow != 0f)
		{
			return;
		}
		Texture2D value = TextureAssets.GlowMask[25].Value;
		int num = drawinfo.drawPlayer.miscCounter / 5;
		for (int i = 0; i < 2; i++)
		{
			num %= 7;
			if (num <= 1 || num >= 5)
			{
				DrawData item = new DrawData(value, new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2)), (Rectangle?)new Rectangle(0, num * value.Height / 7, value.Width, value.Height / 7), drawinfo.colorElectricity, drawinfo.drawPlayer.bodyRotation, new Vector2((float)(value.Width / 2), (float)(value.Height / 14)), 1f, drawinfo.playerEffect, 0f);
				drawinfo.DrawDataCache.Add(item);
			}
			num += 3;
		}
	}

	public static void DrawPlayer_05_ForbiddenSetRing(ref PlayerDrawSet drawinfo)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_019a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01da: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Unknown result type (might be due to invalid IL or missing references)
		//IL_0207: Unknown result type (might be due to invalid IL or missing references)
		//IL_0238: Unknown result type (might be due to invalid IL or missing references)
		//IL_023d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0242: Unknown result type (might be due to invalid IL or missing references)
		//IL_0247: Unknown result type (might be due to invalid IL or missing references)
		//IL_0253: Unknown result type (might be due to invalid IL or missing references)
		//IL_0260: Unknown result type (might be due to invalid IL or missing references)
		//IL_026a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0275: Unknown result type (might be due to invalid IL or missing references)
		//IL_02aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_02dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f2: Unknown result type (might be due to invalid IL or missing references)
		if (drawinfo.drawPlayer.setForbidden && drawinfo.shadow == 0f)
		{
			Color color = Color.Lerp(drawinfo.colorArmorBody, Color.White, 0.7f);
			Texture2D value = TextureAssets.Extra[74].Value;
			Texture2D value2 = TextureAssets.GlowMask[217].Value;
			bool num7 = !drawinfo.drawPlayer.setForbiddenCooldownLocked;
			int num2 = 0;
			num2 = (int)(((float)drawinfo.drawPlayer.miscCounter / 300f * ((float)Math.PI * 2f)).ToRotationVector2().Y * 6f);
			float num3 = ((float)drawinfo.drawPlayer.miscCounter / 75f * ((float)Math.PI * 2f)).ToRotationVector2().X * 4f;
			Color color2 = new Color(80, 70, 40, 0) * (num3 / 8f + 0.5f) * 0.8f;
			if (!num7)
			{
				num2 = 0;
				num3 = 2f;
				color2 = new Color(80, 70, 40, 0) * 0.3f;
				color = color.MultiplyRGB(new Color(0.5f, 0.5f, 1f));
			}
			Vector2 vector = new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2));
			int num4 = 10;
			int num5 = 20;
			if (drawinfo.drawPlayer.head == 238)
			{
				num4 += 4;
				num5 += 4;
			}
			vector += new Vector2((float)(-drawinfo.drawPlayer.direction * num4), (float)(-num5) * drawinfo.drawPlayer.gravDir + (float)num2 * drawinfo.drawPlayer.gravDir);
			DrawData item = new DrawData(value, vector, null, color, drawinfo.drawPlayer.bodyRotation, value.Size() / 2f, 1f, drawinfo.playerEffect);
			item.shader = drawinfo.cBody;
			drawinfo.DrawDataCache.Add(item);
			for (float num6 = 0f; num6 < 4f; num6 += 1f)
			{
				item = new DrawData(value2, vector + (num6 * ((float)Math.PI / 2f)).ToRotationVector2() * num3, null, color2, drawinfo.drawPlayer.bodyRotation, value.Size() / 2f, 1f, drawinfo.playerEffect);
				drawinfo.DrawDataCache.Add(item);
			}
		}
	}

	public static void DrawPlayer_01_3_BackHead(ref PlayerDrawSet drawinfo)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		if (drawinfo.drawPlayer.head >= 0)
		{
			int num = ArmorIDs.Head.Sets.FrontToBackID[drawinfo.drawPlayer.head];
			if (num >= 0)
			{
				Vector2 helmetOffset = drawinfo.helmetOffset;
				DrawData item = new DrawData(TextureAssets.ArmorHead[num].Value, helmetOffset + new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, drawinfo.drawPlayer.bodyFrame, drawinfo.colorArmorHead, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect);
				item.shader = drawinfo.cHead;
				drawinfo.DrawDataCache.Add(item);
			}
		}
	}

	public static void DrawPlayer_01_2_JimsCloak(ref PlayerDrawSet drawinfo)
	{
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		if (drawinfo.drawPlayer.legs == 60 && !drawinfo.isSitting && !drawinfo.drawPlayer.invis && (!ShouldOverrideLegs_CheckShoes(ref drawinfo) || drawinfo.drawPlayer.wearsRobe))
		{
			DrawData item = new DrawData(TextureAssets.Extra[153].Value, new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.legFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.legFrame.Height + 4f)) + drawinfo.drawPlayer.legPosition + drawinfo.legVect, drawinfo.drawPlayer.legFrame, drawinfo.colorArmorLegs, drawinfo.drawPlayer.legRotation, drawinfo.legVect, 1f, drawinfo.playerEffect);
			item.shader = drawinfo.cLegs;
			drawinfo.DrawDataCache.Add(item);
		}
	}

	public static void DrawPlayer_05_2_SafemanSun(ref PlayerDrawSet drawinfo)
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		//IL_0191: Unknown result type (might be due to invalid IL or missing references)
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		//IL_019b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0209: Unknown result type (might be due to invalid IL or missing references)
		//IL_0213: Unknown result type (might be due to invalid IL or missing references)
		//IL_021e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0253: Unknown result type (might be due to invalid IL or missing references)
		//IL_025d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0264: Unknown result type (might be due to invalid IL or missing references)
		//IL_0269: Unknown result type (might be due to invalid IL or missing references)
		//IL_0278: Unknown result type (might be due to invalid IL or missing references)
		//IL_0286: Unknown result type (might be due to invalid IL or missing references)
		//IL_0290: Unknown result type (might be due to invalid IL or missing references)
		//IL_029b: Unknown result type (might be due to invalid IL or missing references)
		if (drawinfo.drawPlayer.head == 238 && drawinfo.shadow == 0f)
		{
			Color color = Color.Lerp(drawinfo.colorArmorBody, Color.White, 0.7f);
			Texture2D value = TextureAssets.Extra[152].Value;
			Texture2D value2 = TextureAssets.Extra[152].Value;
			int num = 0;
			num = (int)(((float)drawinfo.drawPlayer.miscCounter / 300f * ((float)Math.PI * 2f)).ToRotationVector2().Y * 6f);
			float num2 = ((float)drawinfo.drawPlayer.miscCounter / 75f * ((float)Math.PI * 2f)).ToRotationVector2().X * 4f;
			Color color2 = new Color(80, 70, 40, 0) * (num2 / 8f + 0.5f) * 0.8f;
			Vector2 vector = new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2));
			int num3 = 8;
			int num4 = 20;
			num3 += 4;
			num4 += 4;
			vector += new Vector2((float)(-drawinfo.drawPlayer.direction * num3), (float)(-num4) * drawinfo.drawPlayer.gravDir + (float)num * drawinfo.drawPlayer.gravDir);
			DrawData item = new DrawData(value, vector, null, color, drawinfo.drawPlayer.bodyRotation, value.Size() / 2f, 1f, drawinfo.playerEffect);
			item.shader = drawinfo.cHead;
			drawinfo.DrawDataCache.Add(item);
			for (float num5 = 0f; num5 < 4f; num5 += 1f)
			{
				item = new DrawData(value2, vector + (num5 * ((float)Math.PI / 2f)).ToRotationVector2() * num2, null, color2, drawinfo.drawPlayer.bodyRotation, value.Size() / 2f, 1f, drawinfo.playerEffect);
				item.shader = drawinfo.cHead;
				drawinfo.DrawDataCache.Add(item);
			}
		}
	}

	public static void DrawPlayer_06_WebbedDebuffBack(ref PlayerDrawSet drawinfo)
	{
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		if (drawinfo.drawPlayer.webbed && drawinfo.shadow == 0f && drawinfo.drawPlayer.velocity.Y != 0f)
		{
			Color color = drawinfo.colorArmorBody * 0.75f;
			Texture2D value = TextureAssets.Extra[32].Value;
			DrawData item = new DrawData(value, new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2)), null, color, drawinfo.drawPlayer.bodyRotation, value.Size() / 2f, 1f, drawinfo.playerEffect);
			drawinfo.DrawDataCache.Add(item);
		}
	}

	public static void DrawPlayer_07_LeinforsHairShampoo(ref PlayerDrawSet drawinfo)
	{
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01da: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0204: Unknown result type (might be due to invalid IL or missing references)
		//IL_0213: Unknown result type (might be due to invalid IL or missing references)
		//IL_0218: Unknown result type (might be due to invalid IL or missing references)
		//IL_021d: Unknown result type (might be due to invalid IL or missing references)
		//IL_021e: Unknown result type (might be due to invalid IL or missing references)
		//IL_021f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0224: Unknown result type (might be due to invalid IL or missing references)
		//IL_022a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0246: Unknown result type (might be due to invalid IL or missing references)
		//IL_024c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0274: Unknown result type (might be due to invalid IL or missing references)
		//IL_027e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0283: Unknown result type (might be due to invalid IL or missing references)
		//IL_0302: Unknown result type (might be due to invalid IL or missing references)
		//IL_030d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0317: Unknown result type (might be due to invalid IL or missing references)
		//IL_031c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0335: Unknown result type (might be due to invalid IL or missing references)
		//IL_033a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0349: Unknown result type (might be due to invalid IL or missing references)
		//IL_034e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0353: Unknown result type (might be due to invalid IL or missing references)
		//IL_0355: Unknown result type (might be due to invalid IL or missing references)
		//IL_0357: Unknown result type (might be due to invalid IL or missing references)
		//IL_035c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0363: Unknown result type (might be due to invalid IL or missing references)
		//IL_0380: Unknown result type (might be due to invalid IL or missing references)
		//IL_0386: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bd: Unknown result type (might be due to invalid IL or missing references)
		if (!drawinfo.headOnlyRender && drawinfo.drawPlayer.leinforsHair && (drawinfo.fullHair || drawinfo.hatHair || drawinfo.drawsBackHairWithoutHeadgear || drawinfo.drawPlayer.head == -1 || drawinfo.drawPlayer.head == 0) && drawinfo.drawPlayer.hair != 12 && drawinfo.shadow == 0f && Main.rgbToHsl(drawinfo.colorHead).Z > 0.2f)
		{
			if (Main.rand.Next(20) == 0 && !drawinfo.hatHair)
			{
				Rectangle r = Utils.CenteredRectangle(drawinfo.Position + drawinfo.drawPlayer.Size / 2f + new Vector2(0f, drawinfo.drawPlayer.gravDir * -20f), new Vector2(20f, 14f));
				int num = Dust.NewDust(r.TopLeft(), r.Width, r.Height, 204, 0f, 0f, 150, default(Color), 0.3f);
				Main.dust[num].fadeIn = 1f;
				Dust obj = Main.dust[num];
				obj.velocity *= 0.1f;
				Main.dust[num].noLight = true;
				Main.dust[num].shader = GameShaders.Armor.GetSecondaryShader(drawinfo.drawPlayer.cLeinShampoo, drawinfo.drawPlayer);
				drawinfo.DustCache.Add(num);
			}
			if (Main.rand.Next(40) == 0 && drawinfo.hatHair)
			{
				Rectangle r2 = Utils.CenteredRectangle(drawinfo.Position + drawinfo.drawPlayer.Size / 2f + new Vector2((float)(drawinfo.drawPlayer.direction * -10), drawinfo.drawPlayer.gravDir * -10f), new Vector2(5f, 5f));
				int num2 = Dust.NewDust(r2.TopLeft(), r2.Width, r2.Height, 204, 0f, 0f, 150, default(Color), 0.3f);
				Main.dust[num2].fadeIn = 1f;
				Dust obj2 = Main.dust[num2];
				obj2.velocity *= 0.1f;
				Main.dust[num2].noLight = true;
				Main.dust[num2].shader = GameShaders.Armor.GetSecondaryShader(drawinfo.drawPlayer.cLeinShampoo, drawinfo.drawPlayer);
				drawinfo.DustCache.Add(num2);
			}
			if (drawinfo.drawPlayer.velocity.X != 0f && drawinfo.backHairDraw && Main.rand.Next(15) == 0)
			{
				Rectangle r3 = Utils.CenteredRectangle(drawinfo.Position + drawinfo.drawPlayer.Size / 2f + new Vector2((float)(drawinfo.drawPlayer.direction * -14), 0f), new Vector2(4f, 30f));
				int num3 = Dust.NewDust(r3.TopLeft(), r3.Width, r3.Height, 204, 0f, 0f, 150, default(Color), 0.3f);
				Main.dust[num3].fadeIn = 1f;
				Dust obj3 = Main.dust[num3];
				obj3.velocity *= 0.1f;
				Main.dust[num3].noLight = true;
				Main.dust[num3].shader = GameShaders.Armor.GetSecondaryShader(drawinfo.drawPlayer.cLeinShampoo, drawinfo.drawPlayer);
				drawinfo.DustCache.Add(num3);
			}
		}
	}

	public static bool DrawPlayer_08_PlayerVisuallyHasFullArmorSet(PlayerDrawSet drawinfo, int head, int body, int legs)
	{
		if (drawinfo.drawPlayer.head == head && drawinfo.drawPlayer.body == body)
		{
			return drawinfo.drawPlayer.legs == legs;
		}
		return false;
	}

	public static void DrawPlayer_08_Backpacks(ref PlayerDrawSet drawinfo)
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01af: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0206: Unknown result type (might be due to invalid IL or missing references)
		//IL_020b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0210: Unknown result type (might be due to invalid IL or missing references)
		//IL_0212: Unknown result type (might be due to invalid IL or missing references)
		//IL_0214: Unknown result type (might be due to invalid IL or missing references)
		//IL_0219: Unknown result type (might be due to invalid IL or missing references)
		//IL_0244: Unknown result type (might be due to invalid IL or missing references)
		//IL_0249: Unknown result type (might be due to invalid IL or missing references)
		//IL_0265: Unknown result type (might be due to invalid IL or missing references)
		//IL_0270: Unknown result type (might be due to invalid IL or missing references)
		//IL_0275: Unknown result type (might be due to invalid IL or missing references)
		//IL_0277: Unknown result type (might be due to invalid IL or missing references)
		//IL_0284: Unknown result type (might be due to invalid IL or missing references)
		//IL_0289: Unknown result type (might be due to invalid IL or missing references)
		//IL_028f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0291: Unknown result type (might be due to invalid IL or missing references)
		//IL_0298: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_031a: Unknown result type (might be due to invalid IL or missing references)
		//IL_031f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0324: Unknown result type (might be due to invalid IL or missing references)
		//IL_032f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0334: Unknown result type (might be due to invalid IL or missing references)
		//IL_0366: Unknown result type (might be due to invalid IL or missing references)
		//IL_036b: Unknown result type (might be due to invalid IL or missing references)
		//IL_037a: Unknown result type (might be due to invalid IL or missing references)
		//IL_037f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0384: Unknown result type (might be due to invalid IL or missing references)
		//IL_0386: Unknown result type (might be due to invalid IL or missing references)
		//IL_038b: Unknown result type (might be due to invalid IL or missing references)
		//IL_038d: Unknown result type (might be due to invalid IL or missing references)
		//IL_038f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0394: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0428: Unknown result type (might be due to invalid IL or missing references)
		//IL_042d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0432: Unknown result type (might be due to invalid IL or missing references)
		//IL_043d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0442: Unknown result type (might be due to invalid IL or missing references)
		//IL_0474: Unknown result type (might be due to invalid IL or missing references)
		//IL_0479: Unknown result type (might be due to invalid IL or missing references)
		//IL_0488: Unknown result type (might be due to invalid IL or missing references)
		//IL_048d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0492: Unknown result type (might be due to invalid IL or missing references)
		//IL_0494: Unknown result type (might be due to invalid IL or missing references)
		//IL_0499: Unknown result type (might be due to invalid IL or missing references)
		//IL_049b: Unknown result type (might be due to invalid IL or missing references)
		//IL_049d: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_067b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0680: Unknown result type (might be due to invalid IL or missing references)
		//IL_0685: Unknown result type (might be due to invalid IL or missing references)
		//IL_0690: Unknown result type (might be due to invalid IL or missing references)
		//IL_0695: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_06cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_06db: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_06fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0702: Unknown result type (might be due to invalid IL or missing references)
		//IL_0734: Unknown result type (might be due to invalid IL or missing references)
		//IL_0739: Unknown result type (might be due to invalid IL or missing references)
		//IL_0767: Unknown result type (might be due to invalid IL or missing references)
		//IL_076c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0771: Unknown result type (might be due to invalid IL or missing references)
		//IL_0773: Unknown result type (might be due to invalid IL or missing references)
		//IL_0778: Unknown result type (might be due to invalid IL or missing references)
		//IL_077a: Unknown result type (might be due to invalid IL or missing references)
		//IL_077c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0781: Unknown result type (might be due to invalid IL or missing references)
		//IL_085a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0862: Unknown result type (might be due to invalid IL or missing references)
		//IL_086d: Unknown result type (might be due to invalid IL or missing references)
		//IL_087e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0889: Unknown result type (might be due to invalid IL or missing references)
		//IL_08be: Unknown result type (might be due to invalid IL or missing references)
		//IL_08dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0917: Unknown result type (might be due to invalid IL or missing references)
		//IL_0922: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_07eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_081a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0825: Unknown result type (might be due to invalid IL or missing references)
		if (DrawPlayer_08_PlayerVisuallyHasFullArmorSet(drawinfo, 266, 235, 218))
		{
			Vector2 vec = new Vector2(-2f + -2f * drawinfo.drawPlayer.Directions.X, 0f) + drawinfo.Position - Main.screenPosition + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.width / 2), (float)(drawinfo.drawPlayer.height - drawinfo.drawPlayer.bodyFrame.Height / 2));
			vec = vec.Floor();
			Texture2D value = TextureAssets.Extra[212].Value;
			Rectangle value2 = value.Frame(1, 5, 0, drawinfo.drawPlayer.miscCounter % 25 / 5);
			Color immuneAlphaPure = drawinfo.drawPlayer.GetImmuneAlphaPure(new Color(250, 250, 250, 200), drawinfo.shadow);
			immuneAlphaPure *= drawinfo.drawPlayer.stealth;
			DrawData item7 = new DrawData(value, vec, value2, immuneAlphaPure, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect);
			item7.shader = drawinfo.cBody;
			drawinfo.DrawDataCache.Add(item7);
		}
		if (DrawPlayer_08_PlayerVisuallyHasFullArmorSet(drawinfo, 268, 237, 222))
		{
			Vector2 vec2 = new Vector2(-9f + 1f * drawinfo.drawPlayer.Directions.X, -4f * drawinfo.drawPlayer.Directions.Y) + drawinfo.Position - Main.screenPosition + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.width / 2), (float)(drawinfo.drawPlayer.height - drawinfo.drawPlayer.bodyFrame.Height / 2));
			vec2 = vec2.Floor();
			Texture2D value3 = TextureAssets.Extra[213].Value;
			Rectangle value4 = value3.Frame(1, 5, 0, drawinfo.drawPlayer.miscCounter % 25 / 5);
			Color immuneAlphaPure2 = drawinfo.drawPlayer.GetImmuneAlphaPure(new Color(250, 250, 250, 200), drawinfo.shadow);
			immuneAlphaPure2 *= drawinfo.drawPlayer.stealth;
			DrawData item6 = new DrawData(value3, vec2, value4, immuneAlphaPure2, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect);
			item6.shader = drawinfo.cBody;
			drawinfo.DrawDataCache.Add(item6);
		}
		if (drawinfo.heldItem.type == 4818 && drawinfo.drawPlayer.ownedProjectileCounts[902] == 0)
		{
			int num = 8;
			Vector2 vector = default(Vector2);
			((Vector2)(ref vector))._002Ector(0f, 8f);
			Vector2 vec3 = drawinfo.Position - Main.screenPosition + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.width / 2), (float)(drawinfo.drawPlayer.height - drawinfo.drawPlayer.bodyFrame.Height / 2)) + new Vector2(0f, -4f) + vector;
			vec3 = vec3.Floor();
			DrawData item5 = new DrawData(TextureAssets.BackPack[num].Value, vec3, drawinfo.drawPlayer.bodyFrame, drawinfo.colorArmorBody, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect);
			drawinfo.DrawDataCache.Add(item5);
		}
		if (drawinfo.drawPlayer.backpack > 0 && !drawinfo.drawPlayer.mount.Active)
		{
			Vector2 vector2 = default(Vector2);
			((Vector2)(ref vector2))._002Ector(0f, 8f);
			Vector2 vec4 = drawinfo.Position - Main.screenPosition + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.width / 2), (float)(drawinfo.drawPlayer.height - drawinfo.drawPlayer.bodyFrame.Height / 2)) + new Vector2(0f, -4f) + vector2;
			vec4 = vec4.Floor();
			DrawData item = new DrawData(TextureAssets.AccBack[drawinfo.drawPlayer.backpack].Value, vec4, drawinfo.drawPlayer.bodyFrame, drawinfo.colorArmorBody, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect);
			item.shader = drawinfo.cBackpack;
			drawinfo.DrawDataCache.Add(item);
		}
		else
		{
			if (drawinfo.heldItem.type != 1178 && drawinfo.heldItem.type != 779 && drawinfo.heldItem.type != 5134 && drawinfo.heldItem.type != 1295 && drawinfo.heldItem.type != 1910 && !drawinfo.drawPlayer.turtleArmor && drawinfo.drawPlayer.body != 106 && drawinfo.drawPlayer.body != 170)
			{
				return;
			}
			int type = drawinfo.heldItem.type;
			int num2 = 1;
			float num3 = -4f;
			float num4 = -8f;
			int shader = 0;
			if (drawinfo.drawPlayer.turtleArmor)
			{
				num2 = 4;
				shader = drawinfo.cBody;
			}
			else if (drawinfo.drawPlayer.body == 106)
			{
				num2 = 6;
				shader = drawinfo.cBody;
			}
			else if (drawinfo.drawPlayer.body == 170)
			{
				num2 = 7;
				shader = drawinfo.cBody;
			}
			else
			{
				switch (type)
				{
				case 1178:
					num2 = 1;
					break;
				case 779:
					num2 = 2;
					break;
				case 5134:
					num2 = 9;
					break;
				case 1295:
					num2 = 3;
					break;
				case 1910:
					num2 = 5;
					break;
				}
			}
			Vector2 vector3 = default(Vector2);
			((Vector2)(ref vector3))._002Ector(0f, 8f);
			Vector2 vec5 = drawinfo.Position - Main.screenPosition + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.width / 2), (float)(drawinfo.drawPlayer.height - drawinfo.drawPlayer.bodyFrame.Height / 2)) + new Vector2(0f, -4f) + vector3;
			vec5 = vec5.Floor();
			Vector2 vec6 = drawinfo.Position - Main.screenPosition + new Vector2((float)(drawinfo.drawPlayer.width / 2), (float)(drawinfo.drawPlayer.height - drawinfo.drawPlayer.bodyFrame.Height / 2)) + new Vector2((-9f + num3) * (float)drawinfo.drawPlayer.direction, (2f + num4) * drawinfo.drawPlayer.gravDir) + vector3;
			vec6 = vec6.Floor();
			switch (num2)
			{
			case 7:
			{
				DrawData item3 = new DrawData(TextureAssets.BackPack[num2].Value, vec5, (Rectangle?)new Rectangle(0, drawinfo.drawPlayer.bodyFrame.Y, TextureAssets.BackPack[num2].Width(), drawinfo.drawPlayer.bodyFrame.Height), drawinfo.colorArmorBody, drawinfo.drawPlayer.bodyRotation, new Vector2((float)TextureAssets.BackPack[num2].Width() * 0.5f, drawinfo.bodyVect.Y), 1f, drawinfo.playerEffect, 0f);
				item3.shader = shader;
				drawinfo.DrawDataCache.Add(item3);
				break;
			}
			case 4:
			case 6:
			{
				DrawData item4 = new DrawData(TextureAssets.BackPack[num2].Value, vec5, drawinfo.drawPlayer.bodyFrame, drawinfo.colorArmorBody, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect);
				item4.shader = shader;
				drawinfo.DrawDataCache.Add(item4);
				break;
			}
			default:
			{
				DrawData item2 = new DrawData(TextureAssets.BackPack[num2].Value, vec6, (Rectangle?)new Rectangle(0, 0, TextureAssets.BackPack[num2].Width(), TextureAssets.BackPack[num2].Height()), drawinfo.colorArmorBody, drawinfo.drawPlayer.bodyRotation, new Vector2((float)(TextureAssets.BackPack[num2].Width() / 2), (float)(TextureAssets.BackPack[num2].Height() / 2)), 1f, drawinfo.playerEffect, 0f);
				item2.shader = shader;
				drawinfo.DrawDataCache.Add(item2);
				break;
			}
			}
		}
	}

	public static void DrawPlayer_08_1_Tails(ref PlayerDrawSet drawinfo)
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		if (drawinfo.drawPlayer.tail > 0 && !drawinfo.drawPlayer.mount.Active)
		{
			Vector2 zero = Vector2.Zero;
			if (drawinfo.isSitting)
			{
				zero.Y += -2f;
			}
			Vector2 vector = default(Vector2);
			((Vector2)(ref vector))._002Ector(0f, 8f);
			Vector2 vec = zero + drawinfo.Position - Main.screenPosition + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.width / 2), (float)(drawinfo.drawPlayer.height - drawinfo.drawPlayer.bodyFrame.Height / 2)) + new Vector2(0f, -4f) + vector;
			vec = vec.Floor();
			DrawData item = new DrawData(TextureAssets.AccBack[drawinfo.drawPlayer.tail].Value, vec, drawinfo.drawPlayer.bodyFrame, drawinfo.colorArmorBody, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect);
			item.shader = drawinfo.cTail;
			drawinfo.DrawDataCache.Add(item);
		}
	}

	public static void DrawPlayer_10_BackAcc(ref PlayerDrawSet drawinfo)
	{
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_0206: Unknown result type (might be due to invalid IL or missing references)
		//IL_022c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0237: Unknown result type (might be due to invalid IL or missing references)
		//IL_023c: Unknown result type (might be due to invalid IL or missing references)
		//IL_023e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0252: Unknown result type (might be due to invalid IL or missing references)
		//IL_0257: Unknown result type (might be due to invalid IL or missing references)
		//IL_026b: Unknown result type (might be due to invalid IL or missing references)
		//IL_027a: Unknown result type (might be due to invalid IL or missing references)
		//IL_027f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0284: Unknown result type (might be due to invalid IL or missing references)
		//IL_028b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0299: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a4: Unknown result type (might be due to invalid IL or missing references)
		if (drawinfo.drawPlayer.back <= 0)
		{
			return;
		}
		if (drawinfo.drawPlayer.front >= 1)
		{
			int num = drawinfo.drawPlayer.bodyFrame.Y / 56;
			if (num < 1 || num > 5)
			{
				drawinfo.armorAdjust = 10;
			}
			else
			{
				if (drawinfo.drawPlayer.front == 1)
				{
					drawinfo.armorAdjust = 0;
				}
				if (drawinfo.drawPlayer.front == 2)
				{
					drawinfo.armorAdjust = 8;
				}
				if (drawinfo.drawPlayer.front == 3)
				{
					drawinfo.armorAdjust = 0;
				}
				if (drawinfo.drawPlayer.front == 4)
				{
					drawinfo.armorAdjust = 8;
				}
			}
		}
		Vector2 zero = Vector2.Zero;
		Vector2 vector = default(Vector2);
		((Vector2)(ref vector))._002Ector(0f, 8f);
		Vector2 vec = zero + drawinfo.Position - Main.screenPosition + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.width / 2), (float)(drawinfo.drawPlayer.height - drawinfo.drawPlayer.bodyFrame.Height / 2)) + new Vector2(0f, -4f) + vector;
		vec = vec.Floor();
		DrawData item = new DrawData(TextureAssets.AccBack[drawinfo.drawPlayer.back].Value, vec, drawinfo.drawPlayer.bodyFrame, drawinfo.colorArmorBody, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect);
		item.shader = drawinfo.cBack;
		drawinfo.DrawDataCache.Add(item);
		if (drawinfo.drawPlayer.back == 36)
		{
			Rectangle bodyFrame = drawinfo.drawPlayer.bodyFrame;
			Rectangle value = bodyFrame;
			value.Width = 2;
			int num2 = 0;
			int num3 = bodyFrame.Width / 2;
			int num4 = 2;
			if (((Enum)drawinfo.playerEffect).HasFlag((Enum)(object)(SpriteEffects)1))
			{
				num2 = bodyFrame.Width - 2;
				num4 = -2;
			}
			for (int i = 0; i < num3; i++)
			{
				value.X = bodyFrame.X + 2 * i;
				Color immuneAlpha = drawinfo.drawPlayer.GetImmuneAlpha(LiquidRenderer.GetShimmerGlitterColor(top: true, (float)i / 16f, 0f), drawinfo.shadow);
				immuneAlpha *= (float)(int)((Color)(ref drawinfo.colorArmorBody)).A / 255f;
				item = new DrawData(TextureAssets.GlowMask[332].Value, vec + new Vector2((float)(num2 + i * num4), 0f), value, immuneAlpha, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect);
				item.shader = drawinfo.cBack;
				drawinfo.DrawDataCache.Add(item);
			}
		}
	}

	public static void DrawPlayer_09_Wings(ref PlayerDrawSet drawinfo)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_048b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0490: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0504: Unknown result type (might be due to invalid IL or missing references)
		//IL_0506: Unknown result type (might be due to invalid IL or missing references)
		//IL_050b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0512: Unknown result type (might be due to invalid IL or missing references)
		//IL_0514: Unknown result type (might be due to invalid IL or missing references)
		//IL_051e: Unknown result type (might be due to invalid IL or missing references)
		//IL_052e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0530: Unknown result type (might be due to invalid IL or missing references)
		//IL_053a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0545: Unknown result type (might be due to invalid IL or missing references)
		//IL_0580: Unknown result type (might be due to invalid IL or missing references)
		//IL_0582: Unknown result type (might be due to invalid IL or missing references)
		//IL_0587: Unknown result type (might be due to invalid IL or missing references)
		//IL_058e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0590: Unknown result type (might be due to invalid IL or missing references)
		//IL_059a: Unknown result type (might be due to invalid IL or missing references)
		//IL_05aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0390: Unknown result type (might be due to invalid IL or missing references)
		//IL_0392: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0434: Unknown result type (might be due to invalid IL or missing references)
		//IL_043f: Unknown result type (might be due to invalid IL or missing references)
		//IL_060b: Unknown result type (might be due to invalid IL or missing references)
		//IL_060c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0626: Unknown result type (might be due to invalid IL or missing references)
		//IL_0632: Unknown result type (might be due to invalid IL or missing references)
		//IL_0639: Unknown result type (might be due to invalid IL or missing references)
		//IL_063e: Unknown result type (might be due to invalid IL or missing references)
		//IL_063f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0644: Unknown result type (might be due to invalid IL or missing references)
		//IL_0649: Unknown result type (might be due to invalid IL or missing references)
		//IL_0657: Unknown result type (might be due to invalid IL or missing references)
		//IL_065c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0676: Unknown result type (might be due to invalid IL or missing references)
		//IL_0678: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_06da: Unknown result type (might be due to invalid IL or missing references)
		//IL_071a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0725: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b17: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b18: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b1a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b1b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b20: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b25: Unknown result type (might be due to invalid IL or missing references)
		//IL_0984: Unknown result type (might be due to invalid IL or missing references)
		//IL_098f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0999: Unknown result type (might be due to invalid IL or missing references)
		//IL_099e: Unknown result type (might be due to invalid IL or missing references)
		//IL_09a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_09a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_09b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_09bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_09c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_09d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_09df: Unknown result type (might be due to invalid IL or missing references)
		//IL_09e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_09e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_09fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a02: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a20: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a22: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a27: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a2e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a3b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a3d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a47: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a52: Unknown result type (might be due to invalid IL or missing references)
		//IL_0775: Unknown result type (might be due to invalid IL or missing references)
		//IL_078e: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0266: Unknown result type (might be due to invalid IL or missing references)
		//IL_026d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0272: Unknown result type (might be due to invalid IL or missing references)
		//IL_0274: Unknown result type (might be due to invalid IL or missing references)
		//IL_0276: Unknown result type (might be due to invalid IL or missing references)
		//IL_0278: Unknown result type (might be due to invalid IL or missing references)
		//IL_027d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0282: Unknown result type (might be due to invalid IL or missing references)
		//IL_0296: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0337: Unknown result type (might be due to invalid IL or missing references)
		//IL_0342: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d67: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d6c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d97: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d98: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d9a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d9b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0da0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0da5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0db9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dbe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ddc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dde: Unknown result type (might be due to invalid IL or missing references)
		//IL_0de3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dea: Unknown result type (might be due to invalid IL or missing references)
		//IL_0df7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0df9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e03: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e0e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b30: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b35: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b43: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b48: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_07df: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_07eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0843: Unknown result type (might be due to invalid IL or missing references)
		//IL_084d: Unknown result type (might be due to invalid IL or missing references)
		//IL_088d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0898: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e9c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ea1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b9f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ba9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bae: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b64: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b67: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b68: Unknown result type (might be due to invalid IL or missing references)
		//IL_0beb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bed: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bf9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bfe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c2d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c64: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c6a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c6c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c76: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c7b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c7d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c7f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c81: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c86: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c88: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c8a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c92: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c97: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c9c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c9e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ca0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ca2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ca7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cad: Unknown result type (might be due to invalid IL or missing references)
		//IL_0caf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cb4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cbb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cc8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cca: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cd4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cd9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cdb: Unknown result type (might be due to invalid IL or missing references)
		//IL_1031: Unknown result type (might be due to invalid IL or missing references)
		//IL_103d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1042: Unknown result type (might be due to invalid IL or missing references)
		//IL_1043: Unknown result type (might be due to invalid IL or missing references)
		//IL_1048: Unknown result type (might be due to invalid IL or missing references)
		//IL_104d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1067: Unknown result type (might be due to invalid IL or missing references)
		//IL_1069: Unknown result type (might be due to invalid IL or missing references)
		//IL_10c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_10cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_110f: Unknown result type (might be due to invalid IL or missing references)
		//IL_111a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1023: Unknown result type (might be due to invalid IL or missing references)
		//IL_102a: Unknown result type (might be due to invalid IL or missing references)
		//IL_102f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1338: Unknown result type (might be due to invalid IL or missing references)
		//IL_133a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1392: Unknown result type (might be due to invalid IL or missing references)
		//IL_139c: Unknown result type (might be due to invalid IL or missing references)
		//IL_13db: Unknown result type (might be due to invalid IL or missing references)
		//IL_13e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_1164: Unknown result type (might be due to invalid IL or missing references)
		//IL_1166: Unknown result type (might be due to invalid IL or missing references)
		//IL_1431: Unknown result type (might be due to invalid IL or missing references)
		//IL_1433: Unknown result type (might be due to invalid IL or missing references)
		//IL_148b: Unknown result type (might be due to invalid IL or missing references)
		//IL_14a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_14b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_14c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_1504: Unknown result type (might be due to invalid IL or missing references)
		//IL_150f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1208: Unknown result type (might be due to invalid IL or missing references)
		//IL_1234: Unknown result type (might be due to invalid IL or missing references)
		//IL_1239: Unknown result type (might be due to invalid IL or missing references)
		//IL_123e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1250: Unknown result type (might be due to invalid IL or missing references)
		//IL_1252: Unknown result type (might be due to invalid IL or missing references)
		//IL_126a: Unknown result type (might be due to invalid IL or missing references)
		//IL_127a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1282: Unknown result type (might be due to invalid IL or missing references)
		//IL_16a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_16a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_16fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_1715: Unknown result type (might be due to invalid IL or missing references)
		//IL_1720: Unknown result type (might be due to invalid IL or missing references)
		//IL_1731: Unknown result type (might be due to invalid IL or missing references)
		//IL_1773: Unknown result type (might be due to invalid IL or missing references)
		//IL_177e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1571: Unknown result type (might be due to invalid IL or missing references)
		//IL_1573: Unknown result type (might be due to invalid IL or missing references)
		//IL_15cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_15e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_15f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_1605: Unknown result type (might be due to invalid IL or missing references)
		//IL_1648: Unknown result type (might be due to invalid IL or missing references)
		//IL_1653: Unknown result type (might be due to invalid IL or missing references)
		//IL_17ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_17c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_17d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_17db: Unknown result type (might be due to invalid IL or missing references)
		//IL_17ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_17f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_1849: Unknown result type (might be due to invalid IL or missing references)
		//IL_1853: Unknown result type (might be due to invalid IL or missing references)
		//IL_1892: Unknown result type (might be due to invalid IL or missing references)
		//IL_189d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ac9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1acb: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b23: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b3d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b48: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b59: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b63: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ba5: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bb0: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bfd: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bff: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c57: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c71: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c7c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c8d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ccf: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cda: Unknown result type (might be due to invalid IL or missing references)
		//IL_18db: Unknown result type (might be due to invalid IL or missing references)
		//IL_18dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_18e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_1901: Unknown result type (might be due to invalid IL or missing references)
		//IL_1906: Unknown result type (might be due to invalid IL or missing references)
		//IL_1908: Unknown result type (might be due to invalid IL or missing references)
		//IL_190f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1914: Unknown result type (might be due to invalid IL or missing references)
		//IL_1923: Unknown result type (might be due to invalid IL or missing references)
		//IL_192e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1933: Unknown result type (might be due to invalid IL or missing references)
		//IL_1938: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e8f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e94: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ec4: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ec9: Unknown result type (might be due to invalid IL or missing references)
		//IL_2197: Unknown result type (might be due to invalid IL or missing references)
		//IL_2199: Unknown result type (might be due to invalid IL or missing references)
		//IL_21f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_220b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2216: Unknown result type (might be due to invalid IL or missing references)
		//IL_2227: Unknown result type (might be due to invalid IL or missing references)
		//IL_2269: Unknown result type (might be due to invalid IL or missing references)
		//IL_2274: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d3a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d48: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d4e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d50: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d55: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d57: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d5c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1db4: Unknown result type (might be due to invalid IL or missing references)
		//IL_1dc6: Unknown result type (might be due to invalid IL or missing references)
		//IL_1dd1: Unknown result type (might be due to invalid IL or missing references)
		//IL_1de2: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e24: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e2f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1950: Unknown result type (might be due to invalid IL or missing references)
		//IL_1966: Unknown result type (might be due to invalid IL or missing references)
		//IL_196c: Unknown result type (might be due to invalid IL or missing references)
		//IL_196e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1973: Unknown result type (might be due to invalid IL or missing references)
		//IL_1987: Unknown result type (might be due to invalid IL or missing references)
		//IL_1989: Unknown result type (might be due to invalid IL or missing references)
		//IL_198d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1992: Unknown result type (might be due to invalid IL or missing references)
		//IL_1997: Unknown result type (might be due to invalid IL or missing references)
		//IL_1999: Unknown result type (might be due to invalid IL or missing references)
		//IL_19f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_19fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a05: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a47: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a52: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f0b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f1c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f22: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f24: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f2e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f33: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f4d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f4f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f54: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f56: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fae: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fb8: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fc0: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fd1: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fdb: Unknown result type (might be due to invalid IL or missing references)
		//IL_201d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2028: Unknown result type (might be due to invalid IL or missing references)
		//IL_2076: Unknown result type (might be due to invalid IL or missing references)
		//IL_2078: Unknown result type (might be due to invalid IL or missing references)
		//IL_20d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_20da: Unknown result type (might be due to invalid IL or missing references)
		//IL_20e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_20f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_20fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_213f: Unknown result type (might be due to invalid IL or missing references)
		//IL_214a: Unknown result type (might be due to invalid IL or missing references)
		if (drawinfo.drawPlayer.dead || drawinfo.hideEntirePlayer)
		{
			return;
		}
		Vector2 directions = drawinfo.drawPlayer.Directions;
		Vector2 vector = drawinfo.Position - Main.screenPosition + drawinfo.drawPlayer.Size / 2f;
		Vector2 vector9 = default(Vector2);
		((Vector2)(ref vector9))._002Ector(0f, 7f);
		vector = drawinfo.Position - Main.screenPosition + new Vector2((float)(drawinfo.drawPlayer.width / 2), (float)(drawinfo.drawPlayer.height - drawinfo.drawPlayer.bodyFrame.Height / 2)) + vector9;
		if (drawinfo.drawPlayer.wings <= 0)
		{
			return;
		}
		Main.instance.LoadWings(drawinfo.drawPlayer.wings);
		DrawData item;
		if (drawinfo.drawPlayer.wings == 22)
		{
			if (!drawinfo.drawPlayer.ShouldDrawWingsThatAreAlwaysAnimated())
			{
				return;
			}
			Main.instance.LoadItemFlames(1866);
			Color colorArmorBody = drawinfo.colorArmorBody;
			int num = 26;
			int num8 = -9;
			Vector2 vector10 = vector + new Vector2((float)num8, (float)num) * directions;
			if (drawinfo.shadow == 0f && drawinfo.drawPlayer.grappling[0] == -1)
			{
				Color color = default(Color);
				Vector2 vector11 = default(Vector2);
				for (int i = 0; i < 7; i++)
				{
					((Color)(ref color))._002Ector(250 - i * 10, 250 - i * 10, 250 - i * 10, 150 - i * 10);
					((Vector2)(ref vector11))._002Ector((float)Main.rand.Next(-10, 11) * 0.2f, (float)Main.rand.Next(-10, 11) * 0.2f);
					drawinfo.stealth *= drawinfo.stealth;
					drawinfo.stealth *= 1f - drawinfo.shadow;
					((Color)(ref color))._002Ector((int)((float)(int)((Color)(ref color)).R * drawinfo.stealth), (int)((float)(int)((Color)(ref color)).G * drawinfo.stealth), (int)((float)(int)((Color)(ref color)).B * drawinfo.stealth), (int)((float)(int)((Color)(ref color)).A * drawinfo.stealth));
					vector11.X = drawinfo.drawPlayer.itemFlamePos[i].X;
					vector11.Y = 0f - drawinfo.drawPlayer.itemFlamePos[i].Y;
					vector11 *= 0.5f;
					Vector2 position = (vector10 + vector11).Floor();
					item = new DrawData(TextureAssets.ItemFlame[1866].Value, position, (Rectangle?)new Rectangle(0, TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 7 * drawinfo.drawPlayer.wingFrame, TextureAssets.Wings[drawinfo.drawPlayer.wings].Width(), TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 7 - 2), color, drawinfo.drawPlayer.bodyRotation, new Vector2((float)(TextureAssets.Wings[drawinfo.drawPlayer.wings].Width() / 2), (float)(TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 14)), 1f, drawinfo.playerEffect, 0f);
					item.shader = drawinfo.cWings;
					drawinfo.DrawDataCache.Add(item);
				}
			}
			item = new DrawData(TextureAssets.Wings[drawinfo.drawPlayer.wings].Value, vector10.Floor(), (Rectangle?)new Rectangle(0, TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 7 * drawinfo.drawPlayer.wingFrame, TextureAssets.Wings[drawinfo.drawPlayer.wings].Width(), TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 7), colorArmorBody, drawinfo.drawPlayer.bodyRotation, new Vector2((float)(TextureAssets.Wings[drawinfo.drawPlayer.wings].Width() / 2), (float)(TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 14)), 1f, drawinfo.playerEffect, 0f);
			item.shader = drawinfo.cWings;
			drawinfo.DrawDataCache.Add(item);
			return;
		}
		if (drawinfo.drawPlayer.wings == 28)
		{
			if (drawinfo.drawPlayer.ShouldDrawWingsThatAreAlwaysAnimated())
			{
				Color colorArmorBody2 = drawinfo.colorArmorBody;
				Vector2 vector12 = default(Vector2);
				((Vector2)(ref vector12))._002Ector(0f, 19f);
				Vector2 vec = vector + vector12 * directions;
				Texture2D value = TextureAssets.Wings[drawinfo.drawPlayer.wings].Value;
				Rectangle rectangle = value.Frame(1, 4, 0, drawinfo.drawPlayer.miscCounter / 5 % 4);
				rectangle.Width -= 2;
				rectangle.Height -= 2;
				item = new DrawData(value, vec.Floor(), rectangle, Color.Lerp(colorArmorBody2, Color.White, 1f), drawinfo.drawPlayer.bodyRotation, rectangle.Size() / 2f, 1f, drawinfo.playerEffect);
				item.shader = drawinfo.cWings;
				drawinfo.DrawDataCache.Add(item);
				value = TextureAssets.Extra[38].Value;
				item = new DrawData(value, vec.Floor(), rectangle, Color.Lerp(colorArmorBody2, Color.White, 0.5f), drawinfo.drawPlayer.bodyRotation, rectangle.Size() / 2f, 1f, drawinfo.playerEffect);
				item.shader = drawinfo.cWings;
				drawinfo.DrawDataCache.Add(item);
			}
			return;
		}
		if (drawinfo.drawPlayer.wings == 45)
		{
			if (!drawinfo.drawPlayer.ShouldDrawWingsThatAreAlwaysAnimated())
			{
				return;
			}
			DrawStarboardRainbowTrail(ref drawinfo, vector, directions);
			Color val = new Color(255, 255, 255, 255);
			int num9 = 22;
			int num10 = 0;
			Vector2 vec2 = vector + new Vector2((float)num10, (float)num9) * directions;
			Color color4 = val * (1f - drawinfo.shadow);
			item = new DrawData(TextureAssets.Wings[drawinfo.drawPlayer.wings].Value, vec2.Floor(), (Rectangle?)new Rectangle(0, TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 6 * drawinfo.drawPlayer.wingFrame, TextureAssets.Wings[drawinfo.drawPlayer.wings].Width(), TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 6), color4, drawinfo.drawPlayer.bodyRotation, new Vector2((float)(TextureAssets.Wings[drawinfo.drawPlayer.wings].Width() / 2), (float)(TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 12)), 1f, drawinfo.playerEffect, 0f);
			item.shader = drawinfo.cWings;
			drawinfo.DrawDataCache.Add(item);
			if (drawinfo.shadow == 0f)
			{
				float num11 = ((float)drawinfo.drawPlayer.miscCounter / 75f * ((float)Math.PI * 2f)).ToRotationVector2().X * 4f;
				Color color5 = new Color(70, 70, 70, 0) * (num11 / 8f + 0.5f) * 0.4f;
				for (float num12 = 0f; num12 < (float)Math.PI * 2f; num12 += (float)Math.PI / 2f)
				{
					item = new DrawData(TextureAssets.Wings[drawinfo.drawPlayer.wings].Value, vec2.Floor() + num12.ToRotationVector2() * num11, (Rectangle?)new Rectangle(0, TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 6 * drawinfo.drawPlayer.wingFrame, TextureAssets.Wings[drawinfo.drawPlayer.wings].Width(), TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 6), color5, drawinfo.drawPlayer.bodyRotation, new Vector2((float)(TextureAssets.Wings[drawinfo.drawPlayer.wings].Width() / 2), (float)(TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 12)), 1f, drawinfo.playerEffect, 0f);
					item.shader = drawinfo.cWings;
					drawinfo.DrawDataCache.Add(item);
				}
			}
			return;
		}
		if (drawinfo.drawPlayer.wings == 34)
		{
			if (drawinfo.drawPlayer.ShouldDrawWingsThatAreAlwaysAnimated())
			{
				drawinfo.stealth *= drawinfo.stealth;
				drawinfo.stealth *= 1f - drawinfo.shadow;
				Color color6 = default(Color);
				((Color)(ref color6))._002Ector((int)(250f * drawinfo.stealth), (int)(250f * drawinfo.stealth), (int)(250f * drawinfo.stealth), (int)(100f * drawinfo.stealth));
				Vector2 vector13 = default(Vector2);
				((Vector2)(ref vector13))._002Ector(0f, 0f);
				Texture2D value2 = TextureAssets.Wings[drawinfo.drawPlayer.wings].Value;
				Vector2 vec3 = drawinfo.Position + drawinfo.drawPlayer.Size / 2f - Main.screenPosition + vector13 * drawinfo.drawPlayer.Directions - Vector2.UnitX * (float)drawinfo.drawPlayer.direction * 4f;
				Rectangle rectangle2 = value2.Frame(1, 6, 0, drawinfo.drawPlayer.wingFrame);
				rectangle2.Width -= 2;
				rectangle2.Height -= 2;
				item = new DrawData(value2, vec3.Floor(), rectangle2, color6, drawinfo.drawPlayer.bodyRotation, rectangle2.Size() / 2f, 1f, drawinfo.playerEffect);
				item.shader = drawinfo.cWings;
				drawinfo.DrawDataCache.Add(item);
			}
			return;
		}
		if (drawinfo.drawPlayer.wings == 40)
		{
			drawinfo.stealth *= drawinfo.stealth;
			drawinfo.stealth *= 1f - drawinfo.shadow;
			Color color7 = default(Color);
			((Color)(ref color7))._002Ector((int)(250f * drawinfo.stealth), (int)(250f * drawinfo.stealth), (int)(250f * drawinfo.stealth), (int)(100f * drawinfo.stealth));
			Vector2 vector14 = default(Vector2);
			((Vector2)(ref vector14))._002Ector(-4f, 0f);
			Texture2D value3 = TextureAssets.Wings[drawinfo.drawPlayer.wings].Value;
			Vector2 vector15 = vector + vector14 * directions;
			Vector2 scale = default(Vector2);
			for (int j = 0; j < 1; j++)
			{
				SpriteEffects spriteEffects = drawinfo.playerEffect;
				((Vector2)(ref scale))._002Ector(1f);
				Vector2 zero = Vector2.Zero;
				zero.X = drawinfo.drawPlayer.direction * 3;
				if (j == 1)
				{
					spriteEffects = (SpriteEffects)(spriteEffects ^ 1);
					((Vector2)(ref scale))._002Ector(0.7f, 1f);
					zero.X += (float)(-drawinfo.drawPlayer.direction) * 6f;
				}
				Vector2 vector16 = drawinfo.drawPlayer.velocity * -1.5f;
				int num13 = 0;
				int num14 = 8;
				float num15 = 4f;
				if (drawinfo.drawPlayer.velocity.Y == 0f)
				{
					num13 = 8;
					num14 = 14;
					num15 = 3f;
				}
				for (int k = num13; k < num14; k++)
				{
					Vector2 vec4 = vector15;
					Rectangle rectangle3 = value3.Frame(1, 14, 0, k);
					rectangle3.Width -= 2;
					rectangle3.Height -= 2;
					int num2 = (k - num13) % (int)num15;
					Vector2 vector2 = Utils.RotatedBy(new Vector2(0f, 0.5f), (drawinfo.drawPlayer.miscCounterNormalized * (2f + (float)num2) + (float)num2 * 0.5f + (float)j * 1.3f) * ((float)Math.PI * 2f)) * (float)(num2 + 1);
					vec4 += vector2;
					vec4 += vector16 * ((float)num2 / num15);
					vec4 += zero;
					item = new DrawData(value3, vec4.Floor(), rectangle3, color7, drawinfo.drawPlayer.bodyRotation, rectangle3.Size() / 2f, scale, spriteEffects);
					item.shader = drawinfo.cWings;
					drawinfo.DrawDataCache.Add(item);
				}
			}
			return;
		}
		if (drawinfo.drawPlayer.wings == 39)
		{
			if (drawinfo.drawPlayer.ShouldDrawWingsThatAreAlwaysAnimated())
			{
				drawinfo.stealth *= drawinfo.stealth;
				drawinfo.stealth *= 1f - drawinfo.shadow;
				Color colorArmorBody3 = drawinfo.colorArmorBody;
				Vector2 vector3 = default(Vector2);
				((Vector2)(ref vector3))._002Ector(-6f, -7f);
				Texture2D value4 = TextureAssets.Wings[drawinfo.drawPlayer.wings].Value;
				Vector2 vec5 = vector + vector3 * directions;
				Rectangle rectangle4 = value4.Frame(1, 6, 0, drawinfo.drawPlayer.wingFrame);
				rectangle4.Width -= 2;
				rectangle4.Height -= 2;
				item = new DrawData(value4, vec5.Floor(), rectangle4, colorArmorBody3, drawinfo.drawPlayer.bodyRotation, rectangle4.Size() / 2f, 1f, drawinfo.playerEffect);
				item.shader = drawinfo.cWings;
				drawinfo.DrawDataCache.Add(item);
			}
			return;
		}
		int num3 = 0;
		int num4 = 0;
		int num5 = 4;
		if (drawinfo.drawPlayer.wings == 43)
		{
			num4 = -5;
			num3 = -7;
			num5 = 7;
		}
		else if (drawinfo.drawPlayer.wings == 44)
		{
			num5 = 7;
		}
		else if (drawinfo.drawPlayer.wings == 5)
		{
			num4 = 4;
			num3 -= 4;
		}
		else if (drawinfo.drawPlayer.wings == 27)
		{
			num4 = 4;
		}
		Color color8 = drawinfo.colorArmorBody;
		if (drawinfo.drawPlayer.wings == 9 || drawinfo.drawPlayer.wings == 29)
		{
			drawinfo.stealth *= drawinfo.stealth;
			drawinfo.stealth *= 1f - drawinfo.shadow;
			((Color)(ref color8))._002Ector((int)(250f * drawinfo.stealth), (int)(250f * drawinfo.stealth), (int)(250f * drawinfo.stealth), (int)(100f * drawinfo.stealth));
		}
		if (drawinfo.drawPlayer.wings == 10)
		{
			drawinfo.stealth *= drawinfo.stealth;
			drawinfo.stealth *= 1f - drawinfo.shadow;
			((Color)(ref color8))._002Ector((int)(250f * drawinfo.stealth), (int)(250f * drawinfo.stealth), (int)(250f * drawinfo.stealth), (int)(175f * drawinfo.stealth));
		}
		if (drawinfo.drawPlayer.wings == 11 && ((Color)(ref color8)).A > Main.gFade)
		{
			((Color)(ref color8)).A = Main.gFade;
		}
		if (drawinfo.drawPlayer.wings == 31)
		{
			((Color)(ref color8)).A = (byte)(220f * drawinfo.stealth);
		}
		if (drawinfo.drawPlayer.wings == 32)
		{
			((Color)(ref color8)).A = (byte)(127f * drawinfo.stealth);
		}
		if (drawinfo.drawPlayer.wings == 6)
		{
			((Color)(ref color8)).A = (byte)(160f * drawinfo.stealth);
			color8 *= 0.9f;
		}
		Vector2 vector4 = vector + new Vector2((float)(num4 - 9), (float)(num3 + 2)) * directions;
		item = new DrawData(TextureAssets.Wings[drawinfo.drawPlayer.wings].Value, vector4.Floor(), (Rectangle?)new Rectangle(0, TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / num5 * drawinfo.drawPlayer.wingFrame, TextureAssets.Wings[drawinfo.drawPlayer.wings].Width(), TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / num5), color8, drawinfo.drawPlayer.bodyRotation, new Vector2((float)(TextureAssets.Wings[drawinfo.drawPlayer.wings].Width() / 2), (float)(TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / num5 / 2)), 1f, drawinfo.playerEffect, 0f);
		item.shader = drawinfo.cWings;
		drawinfo.DrawDataCache.Add(item);
		if (drawinfo.drawPlayer.wings == 43 && drawinfo.shadow == 0f)
		{
			Vector2 vector5 = vector4;
			Vector2 origin = default(Vector2);
			((Vector2)(ref origin))._002Ector((float)(TextureAssets.Wings[drawinfo.drawPlayer.wings].Width() / 2), (float)(TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / num5 / 2));
			Rectangle value5 = default(Rectangle);
			((Rectangle)(ref value5))._002Ector(0, TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / num5 * drawinfo.drawPlayer.wingFrame, TextureAssets.Wings[drawinfo.drawPlayer.wings].Width(), TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / num5);
			for (int l = 0; l < 2; l++)
			{
				Vector2 position2 = vector5 + new Vector2((float)Main.rand.Next(-10, 10) * 0.125f, (float)Main.rand.Next(-10, 10) * 0.125f);
				item = new DrawData(TextureAssets.GlowMask[272].Value, position2, value5, new Color(230, 230, 230, 60), drawinfo.drawPlayer.bodyRotation, origin, 1f, drawinfo.playerEffect);
				item.shader = drawinfo.cWings;
				drawinfo.DrawDataCache.Add(item);
			}
		}
		if (drawinfo.drawPlayer.wings == 23)
		{
			drawinfo.stealth *= drawinfo.stealth;
			drawinfo.stealth *= 1f - drawinfo.shadow;
			Color color10 = default(Color);
			((Color)(ref color10))._002Ector((int)(200f * drawinfo.stealth), (int)(200f * drawinfo.stealth), (int)(200f * drawinfo.stealth), (int)(200f * drawinfo.stealth));
			item = new DrawData(TextureAssets.Flames[8].Value, vector4.Floor(), (Rectangle?)new Rectangle(0, TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 4 * drawinfo.drawPlayer.wingFrame, TextureAssets.Wings[drawinfo.drawPlayer.wings].Width(), TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 4), color10, drawinfo.drawPlayer.bodyRotation, new Vector2((float)(TextureAssets.Wings[drawinfo.drawPlayer.wings].Width() / 2), (float)(TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 8)), 1f, drawinfo.playerEffect, 0f);
			item.shader = drawinfo.cWings;
			drawinfo.DrawDataCache.Add(item);
		}
		else if (drawinfo.drawPlayer.wings == 27)
		{
			item = new DrawData(TextureAssets.GlowMask[92].Value, vector4.Floor(), (Rectangle?)new Rectangle(0, TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 4 * drawinfo.drawPlayer.wingFrame, TextureAssets.Wings[drawinfo.drawPlayer.wings].Width(), TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 4), new Color(255, 255, 255, 127) * drawinfo.stealth * (1f - drawinfo.shadow), drawinfo.drawPlayer.bodyRotation, new Vector2((float)(TextureAssets.Wings[drawinfo.drawPlayer.wings].Width() / 2), (float)(TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 8)), 1f, drawinfo.playerEffect, 0f);
			item.shader = drawinfo.cWings;
			drawinfo.DrawDataCache.Add(item);
		}
		else if (drawinfo.drawPlayer.wings == 44)
		{
			PlayerRainbowWingsTextureContent playerRainbowWings = TextureAssets.RenderTargets.PlayerRainbowWings;
			playerRainbowWings.Request();
			if (playerRainbowWings.IsReady)
			{
				RenderTarget2D target = playerRainbowWings.GetTarget();
				item = new DrawData((Texture2D)(object)target, vector4.Floor(), (Rectangle?)new Rectangle(0, TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 7 * drawinfo.drawPlayer.wingFrame, TextureAssets.Wings[drawinfo.drawPlayer.wings].Width(), TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 7), new Color(255, 255, 255, 255) * drawinfo.stealth * (1f - drawinfo.shadow), drawinfo.drawPlayer.bodyRotation, new Vector2((float)(TextureAssets.Wings[drawinfo.drawPlayer.wings].Width() / 2), (float)(TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 14)), 1f, drawinfo.playerEffect, 0f);
				item.shader = drawinfo.cWings;
				drawinfo.DrawDataCache.Add(item);
			}
		}
		else if (drawinfo.drawPlayer.wings == 30)
		{
			item = new DrawData(TextureAssets.GlowMask[181].Value, vector4.Floor(), (Rectangle?)new Rectangle(0, TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 4 * drawinfo.drawPlayer.wingFrame, TextureAssets.Wings[drawinfo.drawPlayer.wings].Width(), TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 4), new Color(255, 255, 255, 127) * drawinfo.stealth * (1f - drawinfo.shadow), drawinfo.drawPlayer.bodyRotation, new Vector2((float)(TextureAssets.Wings[drawinfo.drawPlayer.wings].Width() / 2), (float)(TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 8)), 1f, drawinfo.playerEffect, 0f);
			item.shader = drawinfo.cWings;
			drawinfo.DrawDataCache.Add(item);
		}
		else if (drawinfo.drawPlayer.wings == 38)
		{
			Color color9 = drawinfo.ArkhalisColor * drawinfo.stealth * (1f - drawinfo.shadow);
			item = new DrawData(TextureAssets.GlowMask[251].Value, vector4.Floor(), (Rectangle?)new Rectangle(0, TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 4 * drawinfo.drawPlayer.wingFrame, TextureAssets.Wings[drawinfo.drawPlayer.wings].Width(), TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 4), color9, drawinfo.drawPlayer.bodyRotation, new Vector2((float)(TextureAssets.Wings[drawinfo.drawPlayer.wings].Width() / 2), (float)(TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 8)), 1f, drawinfo.playerEffect, 0f);
			item.shader = drawinfo.cWings;
			drawinfo.DrawDataCache.Add(item);
			for (int num6 = drawinfo.drawPlayer.shadowPos.Length - 2; num6 >= 0; num6--)
			{
				Color color2 = color9;
				((Color)(ref color2)).A = 0;
				color2 *= MathHelper.Lerp(1f, 0f, (float)num6 / 3f);
				color2 *= 0.1f;
				Vector2 vector6 = drawinfo.drawPlayer.shadowPos[num6] - drawinfo.drawPlayer.position;
				for (float num7 = 0f; num7 < 1f; num7 += 0.01f)
				{
					Vector2 vector7 = Utils.RotatedBy(new Vector2(2f, 0f), num7 / 0.04f * ((float)Math.PI * 2f));
					item = new DrawData(TextureAssets.GlowMask[251].Value, vector7 + vector6 * num7 + vector4, (Rectangle?)new Rectangle(0, TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 4 * drawinfo.drawPlayer.wingFrame, TextureAssets.Wings[drawinfo.drawPlayer.wings].Width(), TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 4), color2 * (1f - num7), drawinfo.drawPlayer.bodyRotation, new Vector2((float)(TextureAssets.Wings[drawinfo.drawPlayer.wings].Width() / 2), (float)(TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 8)), 1f, drawinfo.playerEffect, 0f);
					item.shader = drawinfo.cWings;
					drawinfo.DrawDataCache.Add(item);
				}
			}
		}
		else if (drawinfo.drawPlayer.wings == 29)
		{
			item = new DrawData(TextureAssets.Wings[drawinfo.drawPlayer.wings].Value, vector4.Floor(), (Rectangle?)new Rectangle(0, TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 4 * drawinfo.drawPlayer.wingFrame, TextureAssets.Wings[drawinfo.drawPlayer.wings].Width(), TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 4), new Color(255, 255, 255, 0) * drawinfo.stealth * (1f - drawinfo.shadow) * 0.5f, drawinfo.drawPlayer.bodyRotation, new Vector2((float)(TextureAssets.Wings[drawinfo.drawPlayer.wings].Width() / 2), (float)(TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 8)), 1.06f, drawinfo.playerEffect, 0f);
			item.shader = drawinfo.cWings;
			drawinfo.DrawDataCache.Add(item);
		}
		else if (drawinfo.drawPlayer.wings == 36)
		{
			item = new DrawData(TextureAssets.GlowMask[213].Value, vector4.Floor(), (Rectangle?)new Rectangle(0, TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 4 * drawinfo.drawPlayer.wingFrame, TextureAssets.Wings[drawinfo.drawPlayer.wings].Width(), TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 4), new Color(255, 255, 255, 0) * drawinfo.stealth * (1f - drawinfo.shadow), drawinfo.drawPlayer.bodyRotation, new Vector2((float)(TextureAssets.Wings[drawinfo.drawPlayer.wings].Width() / 2), (float)(TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 8)), 1.06f, drawinfo.playerEffect, 0f);
			item.shader = drawinfo.cWings;
			drawinfo.DrawDataCache.Add(item);
			Vector2 spinningpoint = default(Vector2);
			((Vector2)(ref spinningpoint))._002Ector(0f, 2f - drawinfo.shadow * 2f);
			for (int m = 0; m < 4; m++)
			{
				item = new DrawData(TextureAssets.GlowMask[213].Value, spinningpoint.RotatedBy((float)Math.PI / 2f * (float)m) + vector4.Floor(), (Rectangle?)new Rectangle(0, TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 4 * drawinfo.drawPlayer.wingFrame, TextureAssets.Wings[drawinfo.drawPlayer.wings].Width(), TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 4), new Color(127, 127, 127, 127) * drawinfo.stealth * (1f - drawinfo.shadow), drawinfo.drawPlayer.bodyRotation, new Vector2((float)(TextureAssets.Wings[drawinfo.drawPlayer.wings].Width() / 2), (float)(TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 8)), 1f, drawinfo.playerEffect, 0f);
				item.shader = drawinfo.cWings;
				drawinfo.DrawDataCache.Add(item);
			}
		}
		else if (drawinfo.drawPlayer.wings == 31)
		{
			Color color3 = default(Color);
			((Color)(ref color3))._002Ector(255, 255, 255, 0);
			color3 = Color.Lerp(Color.HotPink, Color.Crimson, (float)Math.Cos((float)Math.PI * 2f * ((float)drawinfo.drawPlayer.miscCounter / 100f)) * 0.4f + 0.5f);
			((Color)(ref color3)).A = 0;
			for (int n = 0; n < 4; n++)
			{
				Vector2 vector8 = Utils.RotatedBy(new Vector2((float)Math.Cos((float)Math.PI * 2f * ((float)drawinfo.drawPlayer.miscCounter / 60f)) * 0.5f + 0.5f, 0f), (float)n * ((float)Math.PI / 2f)) * 1f;
				item = new DrawData(TextureAssets.Wings[drawinfo.drawPlayer.wings].Value, vector4.Floor() + vector8, (Rectangle?)new Rectangle(0, TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 4 * drawinfo.drawPlayer.wingFrame, TextureAssets.Wings[drawinfo.drawPlayer.wings].Width(), TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 4), color3 * drawinfo.stealth * (1f - drawinfo.shadow) * 0.5f, drawinfo.drawPlayer.bodyRotation, new Vector2((float)(TextureAssets.Wings[drawinfo.drawPlayer.wings].Width() / 2), (float)(TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 8)), 1f, drawinfo.playerEffect, 0f);
				item.shader = drawinfo.cWings;
				drawinfo.DrawDataCache.Add(item);
			}
			item = new DrawData(TextureAssets.Wings[drawinfo.drawPlayer.wings].Value, vector4.Floor(), (Rectangle?)new Rectangle(0, TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 4 * drawinfo.drawPlayer.wingFrame, TextureAssets.Wings[drawinfo.drawPlayer.wings].Width(), TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 4), color3 * drawinfo.stealth * (1f - drawinfo.shadow) * 1f, drawinfo.drawPlayer.bodyRotation, new Vector2((float)(TextureAssets.Wings[drawinfo.drawPlayer.wings].Width() / 2), (float)(TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 8)), 1f, drawinfo.playerEffect, 0f);
			item.shader = drawinfo.cWings;
			drawinfo.DrawDataCache.Add(item);
		}
		else if (drawinfo.drawPlayer.wings == 32)
		{
			item = new DrawData(TextureAssets.GlowMask[183].Value, vector4.Floor(), (Rectangle?)new Rectangle(0, TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 4 * drawinfo.drawPlayer.wingFrame, TextureAssets.Wings[drawinfo.drawPlayer.wings].Width(), TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 4), new Color(255, 255, 255, 0) * drawinfo.stealth * (1f - drawinfo.shadow), drawinfo.drawPlayer.bodyRotation, new Vector2((float)(TextureAssets.Wings[drawinfo.drawPlayer.wings].Width() / 2), (float)(TextureAssets.Wings[drawinfo.drawPlayer.wings].Height() / 8)), 1.06f, drawinfo.playerEffect, 0f);
			item.shader = drawinfo.cWings;
			drawinfo.DrawDataCache.Add(item);
		}
	}

	public static void DrawPlayer_12_1_BalloonFronts(ref PlayerDrawSet drawinfo)
	{
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_017b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0198: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0209: Unknown result type (might be due to invalid IL or missing references)
		//IL_0228: Unknown result type (might be due to invalid IL or missing references)
		//IL_0243: Unknown result type (might be due to invalid IL or missing references)
		//IL_0248: Unknown result type (might be due to invalid IL or missing references)
		//IL_024d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0252: Unknown result type (might be due to invalid IL or missing references)
		//IL_0263: Unknown result type (might be due to invalid IL or missing references)
		//IL_0268: Unknown result type (might be due to invalid IL or missing references)
		//IL_026d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0294: Unknown result type (might be due to invalid IL or missing references)
		//IL_0299: Unknown result type (might be due to invalid IL or missing references)
		//IL_029e: Unknown result type (might be due to invalid IL or missing references)
		//IL_029f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0312: Unknown result type (might be due to invalid IL or missing references)
		//IL_031d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0355: Unknown result type (might be due to invalid IL or missing references)
		//IL_0360: Unknown result type (might be due to invalid IL or missing references)
		if (drawinfo.drawPlayer.balloonFront <= 0)
		{
			return;
		}
		DrawData item;
		if (ArmorIDs.Balloon.Sets.UsesTorsoFraming[drawinfo.drawPlayer.balloonFront])
		{
			item = new DrawData(TextureAssets.AccBalloon[drawinfo.drawPlayer.balloonFront].Value, new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + drawinfo.bodyVect, drawinfo.drawPlayer.bodyFrame, drawinfo.colorArmorBody, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect);
			item.shader = drawinfo.cBalloonFront;
			drawinfo.DrawDataCache.Add(item);
			return;
		}
		int num = ((Main.hasFocus && (!Main.ingameOptionsWindow || !Main.autoPause)) ? (DateTime.Now.Millisecond % 800 / 200) : 0);
		Vector2 vector = Main.OffsetsPlayerOffhand[drawinfo.drawPlayer.bodyFrame.Y / 56];
		if (drawinfo.drawPlayer.direction != 1)
		{
			vector.X = (float)drawinfo.drawPlayer.width - vector.X;
		}
		if (drawinfo.drawPlayer.gravDir != 1f)
		{
			vector.Y -= drawinfo.drawPlayer.height;
		}
		Vector2 vector2 = new Vector2(0f, 8f) + new Vector2(0f, 6f);
		Vector2 vector3 = default(Vector2);
		((Vector2)(ref vector3))._002Ector((float)(int)(drawinfo.Position.X - Main.screenPosition.X + vector.X), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + vector.Y * drawinfo.drawPlayer.gravDir));
		vector3 = drawinfo.Position - Main.screenPosition + vector * new Vector2(1f, drawinfo.drawPlayer.gravDir) + new Vector2(0f, (float)(drawinfo.drawPlayer.height - drawinfo.drawPlayer.bodyFrame.Height)) + vector2;
		vector3 = vector3.Floor();
		item = new DrawData(TextureAssets.AccBalloon[drawinfo.drawPlayer.balloonFront].Value, vector3, (Rectangle?)new Rectangle(0, TextureAssets.AccBalloon[drawinfo.drawPlayer.balloonFront].Height() / 4 * num, TextureAssets.AccBalloon[drawinfo.drawPlayer.balloonFront].Width(), TextureAssets.AccBalloon[drawinfo.drawPlayer.balloonFront].Height() / 4), drawinfo.colorArmorBody, drawinfo.drawPlayer.bodyRotation, new Vector2((float)(26 + drawinfo.drawPlayer.direction * 4), 28f + drawinfo.drawPlayer.gravDir * 6f), 1f, drawinfo.playerEffect, 0f);
		item.shader = drawinfo.cBalloonFront;
		drawinfo.DrawDataCache.Add(item);
	}

	public static void DrawPlayer_11_Balloons(ref PlayerDrawSet drawinfo)
	{
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_017b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0198: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0209: Unknown result type (might be due to invalid IL or missing references)
		//IL_0228: Unknown result type (might be due to invalid IL or missing references)
		//IL_0243: Unknown result type (might be due to invalid IL or missing references)
		//IL_0248: Unknown result type (might be due to invalid IL or missing references)
		//IL_024d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0252: Unknown result type (might be due to invalid IL or missing references)
		//IL_0263: Unknown result type (might be due to invalid IL or missing references)
		//IL_0268: Unknown result type (might be due to invalid IL or missing references)
		//IL_026d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0294: Unknown result type (might be due to invalid IL or missing references)
		//IL_0299: Unknown result type (might be due to invalid IL or missing references)
		//IL_029e: Unknown result type (might be due to invalid IL or missing references)
		//IL_029f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0312: Unknown result type (might be due to invalid IL or missing references)
		//IL_031d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0355: Unknown result type (might be due to invalid IL or missing references)
		//IL_0360: Unknown result type (might be due to invalid IL or missing references)
		if (drawinfo.drawPlayer.balloon <= 0)
		{
			return;
		}
		DrawData item;
		if (ArmorIDs.Balloon.Sets.UsesTorsoFraming[drawinfo.drawPlayer.balloon])
		{
			item = new DrawData(TextureAssets.AccBalloon[drawinfo.drawPlayer.balloon].Value, new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + drawinfo.bodyVect, drawinfo.drawPlayer.bodyFrame, drawinfo.colorArmorBody, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect);
			item.shader = drawinfo.cBalloon;
			drawinfo.DrawDataCache.Add(item);
			return;
		}
		int num = ((Main.hasFocus && (!Main.ingameOptionsWindow || !Main.autoPause)) ? (DateTime.Now.Millisecond % 800 / 200) : 0);
		Vector2 vector = Main.OffsetsPlayerOffhand[drawinfo.drawPlayer.bodyFrame.Y / 56];
		if (drawinfo.drawPlayer.direction != 1)
		{
			vector.X = (float)drawinfo.drawPlayer.width - vector.X;
		}
		if (drawinfo.drawPlayer.gravDir != 1f)
		{
			vector.Y -= drawinfo.drawPlayer.height;
		}
		Vector2 vector2 = new Vector2(0f, 8f) + new Vector2(0f, 6f);
		Vector2 vector3 = default(Vector2);
		((Vector2)(ref vector3))._002Ector((float)(int)(drawinfo.Position.X - Main.screenPosition.X + vector.X), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + vector.Y * drawinfo.drawPlayer.gravDir));
		vector3 = drawinfo.Position - Main.screenPosition + vector * new Vector2(1f, drawinfo.drawPlayer.gravDir) + new Vector2(0f, (float)(drawinfo.drawPlayer.height - drawinfo.drawPlayer.bodyFrame.Height)) + vector2;
		vector3 = vector3.Floor();
		item = new DrawData(TextureAssets.AccBalloon[drawinfo.drawPlayer.balloon].Value, vector3, (Rectangle?)new Rectangle(0, TextureAssets.AccBalloon[drawinfo.drawPlayer.balloon].Height() / 4 * num, TextureAssets.AccBalloon[drawinfo.drawPlayer.balloon].Width(), TextureAssets.AccBalloon[drawinfo.drawPlayer.balloon].Height() / 4), drawinfo.colorArmorBody, drawinfo.drawPlayer.bodyRotation, new Vector2((float)(26 + drawinfo.drawPlayer.direction * 4), 28f + drawinfo.drawPlayer.gravDir * 6f), 1f, drawinfo.playerEffect, 0f);
		item.shader = drawinfo.cBalloon;
		drawinfo.DrawDataCache.Add(item);
	}

	public static void DrawPlayer_12_Skin(ref PlayerDrawSet drawinfo)
	{
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_0254: Unknown result type (might be due to invalid IL or missing references)
		//IL_025f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0264: Unknown result type (might be due to invalid IL or missing references)
		//IL_028f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0294: Unknown result type (might be due to invalid IL or missing references)
		//IL_029f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
		if (drawinfo.usesCompositeTorso)
		{
			DrawPlayer_12_Skin_Composite(ref drawinfo);
			return;
		}
		if (drawinfo.isSitting)
		{
			drawinfo.hidesBottomSkin = true;
		}
		if (!drawinfo.hidesTopSkin)
		{
			drawinfo.Position.Y += drawinfo.torsoOffset;
			DrawData drawData = new DrawData(TextureAssets.Players[drawinfo.skinVar, 3].Value, new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2)), drawinfo.drawPlayer.bodyFrame, drawinfo.colorBodySkin, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect);
			drawData.shader = drawinfo.skinDyePacked;
			DrawData item2 = drawData;
			drawinfo.DrawDataCache.Add(item2);
			drawinfo.Position.Y -= drawinfo.torsoOffset;
		}
		if (!drawinfo.hidesBottomSkin && !drawinfo.isBottomOverriden)
		{
			if (drawinfo.isSitting)
			{
				DrawSittingLegs(ref drawinfo, TextureAssets.Players[drawinfo.skinVar, 10].Value, drawinfo.colorLegs);
				return;
			}
			DrawData item = new DrawData(TextureAssets.Players[drawinfo.skinVar, 10].Value, new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2)), drawinfo.drawPlayer.legFrame, drawinfo.colorLegs, drawinfo.drawPlayer.legRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect);
			drawinfo.DrawDataCache.Add(item);
		}
	}

	internal static bool IsBottomOverridden(ref PlayerDrawSet drawinfo)
	{
		if (ShouldOverrideLegs_CheckPants(ref drawinfo))
		{
			return true;
		}
		if (ShouldOverrideLegs_CheckShoes(ref drawinfo))
		{
			return true;
		}
		return false;
	}

	private static bool ShouldOverrideLegs_CheckPants(ref PlayerDrawSet drawinfo)
	{
		if (drawinfo.drawPlayer.legs > 0)
		{
			return ArmorIDs.Legs.Sets.OverridesLegs[drawinfo.drawPlayer.legs];
		}
		return false;
	}

	private static bool ShouldOverrideLegs_CheckShoes(ref PlayerDrawSet drawinfo)
	{
		int shoe = drawinfo.drawPlayer.shoe;
		if (shoe > 0)
		{
			return ArmorIDs.Shoe.Sets.OverridesLegs[shoe];
		}
		return false;
	}

	public static void DrawPlayer_12_Skin_Composite(ref PlayerDrawSet drawinfo)
	{
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0173: Unknown result type (might be due to invalid IL or missing references)
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_017e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_018c: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0190: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_0197: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_019b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0223: Unknown result type (might be due to invalid IL or missing references)
		//IL_0225: Unknown result type (might be due to invalid IL or missing references)
		//IL_0230: Unknown result type (might be due to invalid IL or missing references)
		//IL_0237: Unknown result type (might be due to invalid IL or missing references)
		//IL_0242: Unknown result type (might be due to invalid IL or missing references)
		//IL_01be: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0353: Unknown result type (might be due to invalid IL or missing references)
		//IL_035e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0363: Unknown result type (might be due to invalid IL or missing references)
		//IL_038e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0393: Unknown result type (might be due to invalid IL or missing references)
		//IL_039e: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ac: Unknown result type (might be due to invalid IL or missing references)
		if (!drawinfo.hidesTopSkin && !drawinfo.drawPlayer.invis)
		{
			Vector2 vector = new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2));
			vector.Y += drawinfo.torsoOffset;
			Vector2 vector2 = Main.OffsetsPlayerHeadgear[drawinfo.drawPlayer.bodyFrame.Y / drawinfo.drawPlayer.bodyFrame.Height];
			vector2.Y -= 2f;
			vector += vector2 * (float)(-((Enum)drawinfo.playerEffect).HasFlag((Enum)(object)(SpriteEffects)2).ToDirectionInt());
			float bodyRotation = drawinfo.drawPlayer.bodyRotation;
			Vector2 vector3 = vector;
			Vector2 val = vector;
			Vector2 bodyVect3 = drawinfo.bodyVect;
			Vector2 bodyVect2 = drawinfo.bodyVect;
			Vector2 compositeOffset_BackArm = GetCompositeOffset_BackArm(ref drawinfo);
			vector3 += compositeOffset_BackArm;
			_ = bodyVect3 + compositeOffset_BackArm;
			Vector2 compositeOffset_FrontArm = GetCompositeOffset_FrontArm(ref drawinfo);
			bodyVect2 += compositeOffset_FrontArm;
			_ = val + compositeOffset_FrontArm;
			if (drawinfo.drawFloatingTube)
			{
				drawinfo.DrawDataCache.Add(new DrawData(TextureAssets.Extra[105].Value, vector, (Rectangle?)new Rectangle(0, 0, 40, 56), drawinfo.floatingTubeColor, bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect, 0f)
				{
					shader = drawinfo.cFloatingTube
				});
			}
			drawinfo.DrawDataCache.Add(new DrawData(TextureAssets.Players[drawinfo.skinVar, 3].Value, vector, drawinfo.compTorsoFrame, drawinfo.colorBodySkin, bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect)
			{
				shader = drawinfo.skinDyePacked
			});
		}
		if (!drawinfo.hidesBottomSkin && !drawinfo.drawPlayer.invis && !drawinfo.isBottomOverriden)
		{
			if (drawinfo.isSitting)
			{
				DrawSittingLegs(ref drawinfo, TextureAssets.Players[drawinfo.skinVar, 10].Value, drawinfo.colorLegs, drawinfo.skinDyePacked);
			}
			else
			{
				DrawData drawData = new DrawData(TextureAssets.Players[drawinfo.skinVar, 10].Value, new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2)), drawinfo.drawPlayer.legFrame, drawinfo.colorLegs, drawinfo.drawPlayer.legRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect);
				drawData.shader = drawinfo.skinDyePacked;
				DrawData item = drawData;
				drawinfo.DrawDataCache.Add(item);
			}
		}
		DrawPlayer_12_SkinComposite_BackArmShirt(ref drawinfo);
	}

	public static void DrawPlayer_12_SkinComposite_BackArmShirt(ref PlayerDrawSet drawinfo)
	{
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0170: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		//IL_0534: Unknown result type (might be due to invalid IL or missing references)
		//IL_0536: Unknown result type (might be due to invalid IL or missing references)
		//IL_0541: Unknown result type (might be due to invalid IL or missing references)
		//IL_0548: Unknown result type (might be due to invalid IL or missing references)
		//IL_0550: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_05fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0605: Unknown result type (might be due to invalid IL or missing references)
		//IL_061e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0626: Unknown result type (might be due to invalid IL or missing references)
		//IL_0485: Unknown result type (might be due to invalid IL or missing references)
		//IL_0487: Unknown result type (might be due to invalid IL or missing references)
		//IL_0492: Unknown result type (might be due to invalid IL or missing references)
		//IL_0499: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04da: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_034a: Unknown result type (might be due to invalid IL or missing references)
		//IL_034c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0357: Unknown result type (might be due to invalid IL or missing references)
		//IL_035e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0366: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0309: Unknown result type (might be due to invalid IL or missing references)
		//IL_0310: Unknown result type (might be due to invalid IL or missing references)
		//IL_031b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0207: Unknown result type (might be due to invalid IL or missing references)
		//IL_0209: Unknown result type (might be due to invalid IL or missing references)
		//IL_0214: Unknown result type (might be due to invalid IL or missing references)
		//IL_021b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0223: Unknown result type (might be due to invalid IL or missing references)
		//IL_0420: Unknown result type (might be due to invalid IL or missing references)
		//IL_0422: Unknown result type (might be due to invalid IL or missing references)
		//IL_042d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0434: Unknown result type (might be due to invalid IL or missing references)
		//IL_043c: Unknown result type (might be due to invalid IL or missing references)
		//IL_026e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0270: Unknown result type (might be due to invalid IL or missing references)
		//IL_027b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0282: Unknown result type (might be due to invalid IL or missing references)
		//IL_028a: Unknown result type (might be due to invalid IL or missing references)
		Vector2 vector = new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2));
		Vector2 vector2 = Main.OffsetsPlayerHeadgear[drawinfo.drawPlayer.bodyFrame.Y / drawinfo.drawPlayer.bodyFrame.Height];
		vector2.Y -= 2f;
		vector += vector2 * (float)(-((Enum)drawinfo.playerEffect).HasFlag((Enum)(object)(SpriteEffects)2).ToDirectionInt());
		vector.Y += drawinfo.torsoOffset;
		float bodyRotation = drawinfo.drawPlayer.bodyRotation;
		Vector2 vector3 = vector;
		Vector2 position = vector;
		Vector2 bodyVect = drawinfo.bodyVect;
		Vector2 compositeOffset_BackArm = GetCompositeOffset_BackArm(ref drawinfo);
		vector3 += compositeOffset_BackArm;
		position += drawinfo.backShoulderOffset;
		bodyVect += compositeOffset_BackArm;
		float rotation = bodyRotation + drawinfo.compositeBackArmRotation;
		bool flag = !drawinfo.drawPlayer.invis;
		bool flag2 = !drawinfo.drawPlayer.invis;
		bool flag3 = drawinfo.drawPlayer.body > 0;
		bool flag4 = !drawinfo.hidesTopSkin;
		bool flag5 = false;
		if (flag3)
		{
			flag &= drawinfo.missingHand;
			if (flag2 && drawinfo.missingArm)
			{
				if (flag4)
				{
					drawinfo.DrawDataCache.Add(new DrawData(TextureAssets.Players[drawinfo.skinVar, 7].Value, vector3, drawinfo.compBackArmFrame, drawinfo.colorBodySkin, rotation, bodyVect, 1f, drawinfo.playerEffect)
					{
						shader = drawinfo.skinDyePacked
					});
				}
				if (!flag5 && flag4)
				{
					drawinfo.DrawDataCache.Add(new DrawData(TextureAssets.Players[drawinfo.skinVar, 5].Value, vector3, drawinfo.compBackArmFrame, drawinfo.colorBodySkin, rotation, bodyVect, 1f, drawinfo.playerEffect)
					{
						shader = drawinfo.skinDyePacked
					});
					flag5 = true;
				}
				flag2 = false;
			}
			if (!drawinfo.drawPlayer.invis || IsArmorDrawnWhenInvisible(drawinfo.drawPlayer.body))
			{
				Texture2D value = TextureAssets.ArmorBodyComposite[drawinfo.drawPlayer.body].Value;
				if (!drawinfo.hideCompositeShoulders)
				{
					DrawCompositeArmorPiece(ref drawinfo, CompositePlayerDrawContext.BackShoulder, new DrawData(value, position, drawinfo.compBackShoulderFrame, drawinfo.colorArmorBody, bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect)
					{
						shader = drawinfo.cBody
					});
				}
				DrawPlayer_12_1_BalloonFronts(ref drawinfo);
				DrawCompositeArmorPiece(ref drawinfo, CompositePlayerDrawContext.BackArm, new DrawData(value, vector3, drawinfo.compBackArmFrame, drawinfo.colorArmorBody, rotation, bodyVect, 1f, drawinfo.playerEffect)
				{
					shader = drawinfo.cBody
				});
			}
		}
		if (flag)
		{
			if (flag4)
			{
				if (flag2)
				{
					drawinfo.DrawDataCache.Add(new DrawData(TextureAssets.Players[drawinfo.skinVar, 7].Value, vector3, drawinfo.compBackArmFrame, drawinfo.colorBodySkin, rotation, bodyVect, 1f, drawinfo.playerEffect)
					{
						shader = drawinfo.skinDyePacked
					});
				}
				if (!flag5 && flag4)
				{
					drawinfo.DrawDataCache.Add(new DrawData(TextureAssets.Players[drawinfo.skinVar, 5].Value, vector3, drawinfo.compBackArmFrame, drawinfo.colorBodySkin, rotation, bodyVect, 1f, drawinfo.playerEffect)
					{
						shader = drawinfo.skinDyePacked
					});
					flag5 = true;
				}
			}
			if (!flag3)
			{
				drawinfo.DrawDataCache.Add(new DrawData(TextureAssets.Players[drawinfo.skinVar, 8].Value, vector3, drawinfo.compBackArmFrame, drawinfo.colorUnderShirt, rotation, bodyVect, 1f, drawinfo.playerEffect));
				DrawPlayer_12_1_BalloonFronts(ref drawinfo);
				drawinfo.DrawDataCache.Add(new DrawData(TextureAssets.Players[drawinfo.skinVar, 13].Value, vector3, drawinfo.compBackArmFrame, drawinfo.colorShirt, rotation, bodyVect, 1f, drawinfo.playerEffect));
			}
		}
		if (drawinfo.drawPlayer.handoff > 0)
		{
			Texture2D value2 = TextureAssets.AccHandsOffComposite[drawinfo.drawPlayer.handoff].Value;
			DrawCompositeArmorPiece(ref drawinfo, CompositePlayerDrawContext.BackArmAccessory, new DrawData(value2, vector3, drawinfo.compBackArmFrame, drawinfo.colorArmorBody, rotation, bodyVect, 1f, drawinfo.playerEffect)
			{
				shader = drawinfo.cHandOff
			});
		}
		if (drawinfo.drawPlayer.drawingFootball)
		{
			Main.instance.LoadProjectile(861);
			Texture2D value3 = TextureAssets.Projectile[861].Value;
			Rectangle rectangle = value3.Frame(1, 4);
			Vector2 origin = rectangle.Size() / 2f;
			Vector2 position2 = vector3 + new Vector2((float)(drawinfo.drawPlayer.direction * -2), drawinfo.drawPlayer.gravDir * 4f);
			drawinfo.DrawDataCache.Add(new DrawData(value3, position2, rectangle, drawinfo.colorArmorBody, bodyRotation + (float)Math.PI / 4f * (float)drawinfo.drawPlayer.direction, origin, 0.8f, drawinfo.playerEffect));
		}
	}

	public static void DrawPlayer_13_Leggings(ref PlayerDrawSet drawinfo)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0804: Unknown result type (might be due to invalid IL or missing references)
		//IL_080f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0814: Unknown result type (might be due to invalid IL or missing references)
		//IL_081a: Unknown result type (might be due to invalid IL or missing references)
		//IL_081f: Unknown result type (might be due to invalid IL or missing references)
		//IL_082a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0835: Unknown result type (might be due to invalid IL or missing references)
		//IL_0846: Unknown result type (might be due to invalid IL or missing references)
		//IL_0851: Unknown result type (might be due to invalid IL or missing references)
		//IL_08fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0909: Unknown result type (might be due to invalid IL or missing references)
		//IL_090e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0914: Unknown result type (might be due to invalid IL or missing references)
		//IL_0919: Unknown result type (might be due to invalid IL or missing references)
		//IL_0924: Unknown result type (might be due to invalid IL or missing references)
		//IL_092f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0940: Unknown result type (might be due to invalid IL or missing references)
		//IL_094b: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_0465: Unknown result type (might be due to invalid IL or missing references)
		//IL_046a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0475: Unknown result type (might be due to invalid IL or missing references)
		//IL_047a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0480: Unknown result type (might be due to invalid IL or missing references)
		//IL_0485: Unknown result type (might be due to invalid IL or missing references)
		//IL_0490: Unknown result type (might be due to invalid IL or missing references)
		//IL_049b: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0663: Unknown result type (might be due to invalid IL or missing references)
		//IL_06dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_06fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0707: Unknown result type (might be due to invalid IL or missing references)
		//IL_0712: Unknown result type (might be due to invalid IL or missing references)
		//IL_0723: Unknown result type (might be due to invalid IL or missing references)
		//IL_072e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_0247: Unknown result type (might be due to invalid IL or missing references)
		//IL_0502: Unknown result type (might be due to invalid IL or missing references)
		//IL_052d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0532: Unknown result type (might be due to invalid IL or missing references)
		//IL_05af: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_05bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_05cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_05fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_060b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0616: Unknown result type (might be due to invalid IL or missing references)
		//IL_0274: Unknown result type (might be due to invalid IL or missing references)
		//IL_027f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02de: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0313: Unknown result type (might be due to invalid IL or missing references)
		//IL_0315: Unknown result type (might be due to invalid IL or missing references)
		//IL_0316: Unknown result type (might be due to invalid IL or missing references)
		//IL_031b: Unknown result type (might be due to invalid IL or missing references)
		//IL_031d: Unknown result type (might be due to invalid IL or missing references)
		//IL_031f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0324: Unknown result type (might be due to invalid IL or missing references)
		//IL_0329: Unknown result type (might be due to invalid IL or missing references)
		//IL_032b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0333: Unknown result type (might be due to invalid IL or missing references)
		//IL_0343: Unknown result type (might be due to invalid IL or missing references)
		//IL_0345: Unknown result type (might be due to invalid IL or missing references)
		//IL_0366: Unknown result type (might be due to invalid IL or missing references)
		//IL_036b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0370: Unknown result type (might be due to invalid IL or missing references)
		//IL_0373: Unknown result type (might be due to invalid IL or missing references)
		Vector2 legsOffset = drawinfo.legsOffset;
		if (drawinfo.drawPlayer.legs == 169)
		{
			return;
		}
		if (drawinfo.isSitting && drawinfo.drawPlayer.legs != 140 && drawinfo.drawPlayer.legs != 217)
		{
			if (drawinfo.drawPlayer.legs > 0 && (!ShouldOverrideLegs_CheckShoes(ref drawinfo) || drawinfo.drawPlayer.wearsRobe))
			{
				if (!drawinfo.drawPlayer.invis)
				{
					DrawSittingLegs(ref drawinfo, TextureAssets.ArmorLeg[drawinfo.drawPlayer.legs].Value, drawinfo.colorArmorLegs, drawinfo.cLegs);
					if (drawinfo.legsGlowMask != -1)
					{
						DrawSittingLegs(ref drawinfo, TextureAssets.GlowMask[drawinfo.legsGlowMask].Value, drawinfo.legsGlowColor, drawinfo.cLegs);
					}
				}
			}
			else if (!drawinfo.drawPlayer.invis && !ShouldOverrideLegs_CheckShoes(ref drawinfo))
			{
				DrawSittingLegs(ref drawinfo, TextureAssets.Players[drawinfo.skinVar, 11].Value, drawinfo.colorPants);
				DrawSittingLegs(ref drawinfo, TextureAssets.Players[drawinfo.skinVar, 12].Value, drawinfo.colorShoes);
			}
		}
		else if (drawinfo.drawPlayer.legs == 140)
		{
			if (!drawinfo.drawPlayer.invis && !drawinfo.drawPlayer.mount.Active)
			{
				Texture2D value = TextureAssets.Extra[73].Value;
				bool flag = drawinfo.drawPlayer.legFrame.Y != drawinfo.drawPlayer.legFrame.Height || Main.gameMenu;
				int num = drawinfo.drawPlayer.miscCounter / 3 % 8;
				if (flag)
				{
					num = drawinfo.drawPlayer.miscCounter / 4 % 8;
				}
				Rectangle rectangle = default(Rectangle);
				((Rectangle)(ref rectangle))._002Ector(18 * flag.ToInt(), num * 26, 16, 24);
				float num2 = 12f;
				if (drawinfo.drawPlayer.bodyFrame.Height != 0)
				{
					num2 = 12f - Main.OffsetsPlayerHeadgear[drawinfo.drawPlayer.bodyFrame.Y / drawinfo.drawPlayer.bodyFrame.Height].Y;
				}
				if (drawinfo.drawPlayer.Directions.Y == -1f)
				{
					num2 -= 6f;
				}
				Vector2 scale = default(Vector2);
				((Vector2)(ref scale))._002Ector(1f, 1f);
				Vector2 val = drawinfo.Position + drawinfo.drawPlayer.Size * new Vector2(0.5f, 0.5f + 0.5f * drawinfo.drawPlayer.gravDir);
				_ = drawinfo.drawPlayer.direction;
				Vector2 vec = val + new Vector2(0f, (0f - num2) * drawinfo.drawPlayer.gravDir) - Main.screenPosition + drawinfo.drawPlayer.legPosition;
				if (drawinfo.isSitting)
				{
					vec.Y += drawinfo.seatYOffset;
				}
				vec += legsOffset;
				vec = vec.Floor();
				DrawData item = new DrawData(value, vec, rectangle, drawinfo.colorArmorLegs, drawinfo.drawPlayer.legRotation, rectangle.Size() * new Vector2(0.5f, 0.5f - drawinfo.drawPlayer.gravDir * 0.5f), scale, drawinfo.playerEffect);
				item.shader = drawinfo.cLegs;
				drawinfo.DrawDataCache.Add(item);
			}
		}
		else if (drawinfo.drawPlayer.legs > 0 && (!ShouldOverrideLegs_CheckShoes(ref drawinfo) || drawinfo.drawPlayer.wearsRobe))
		{
			if (drawinfo.drawPlayer.invis)
			{
				return;
			}
			DrawData item2 = new DrawData(TextureAssets.ArmorLeg[drawinfo.drawPlayer.legs].Value, legsOffset + new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.legFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.legFrame.Height + 4f)) + drawinfo.drawPlayer.legPosition + drawinfo.legVect, drawinfo.drawPlayer.legFrame, drawinfo.colorArmorLegs, drawinfo.drawPlayer.legRotation, drawinfo.legVect, 1f, drawinfo.playerEffect);
			item2.shader = drawinfo.cLegs;
			drawinfo.DrawDataCache.Add(item2);
			if (drawinfo.legsGlowMask == -1)
			{
				return;
			}
			if (drawinfo.legsGlowMask == 274)
			{
				for (int i = 0; i < 2; i++)
				{
					Vector2 position = legsOffset + new Vector2((float)Main.rand.Next(-10, 10) * 0.125f, (float)Main.rand.Next(-10, 10) * 0.125f) + new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.legFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.legFrame.Height + 4f)) + drawinfo.drawPlayer.legPosition + drawinfo.legVect;
					item2 = new DrawData(TextureAssets.GlowMask[drawinfo.legsGlowMask].Value, position, drawinfo.drawPlayer.legFrame, drawinfo.legsGlowColor, drawinfo.drawPlayer.legRotation, drawinfo.legVect, 1f, drawinfo.playerEffect);
					item2.shader = drawinfo.cLegs;
					drawinfo.DrawDataCache.Add(item2);
				}
			}
			else
			{
				item2 = new DrawData(TextureAssets.GlowMask[drawinfo.legsGlowMask].Value, legsOffset + new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.legFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.legFrame.Height + 4f)) + drawinfo.drawPlayer.legPosition + drawinfo.legVect, drawinfo.drawPlayer.legFrame, drawinfo.legsGlowColor, drawinfo.drawPlayer.legRotation, drawinfo.legVect, 1f, drawinfo.playerEffect);
				item2.shader = drawinfo.cLegs;
				drawinfo.DrawDataCache.Add(item2);
			}
		}
		else if (!drawinfo.drawPlayer.invis && !ShouldOverrideLegs_CheckShoes(ref drawinfo))
		{
			DrawData item3 = new DrawData(TextureAssets.Players[drawinfo.skinVar, 11].Value, new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.legFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.legFrame.Height + 4f)) + drawinfo.drawPlayer.legPosition + drawinfo.legVect, drawinfo.drawPlayer.legFrame, drawinfo.colorPants, drawinfo.drawPlayer.legRotation, drawinfo.legVect, 1f, drawinfo.playerEffect);
			drawinfo.DrawDataCache.Add(item3);
			item3 = new DrawData(TextureAssets.Players[drawinfo.skinVar, 12].Value, new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.legFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.legFrame.Height + 4f)) + drawinfo.drawPlayer.legPosition + drawinfo.legVect, drawinfo.drawPlayer.legFrame, drawinfo.colorShoes, drawinfo.drawPlayer.legRotation, drawinfo.legVect, 1f, drawinfo.playerEffect);
			drawinfo.DrawDataCache.Add(item3);
		}
	}

	private static void DrawSittingLongCoats(ref PlayerDrawSet drawinfo, int specialLegCoat, Texture2D textureToDraw, Color matchingColor, int shaderIndex = 0, bool glowmask = false)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		Vector2 legsOffset = drawinfo.legsOffset;
		Vector2 position = new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.legFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.legFrame.Height + 4f)) + drawinfo.drawPlayer.legPosition + drawinfo.legVect;
		Rectangle legFrame = drawinfo.drawPlayer.legFrame;
		position += legsOffset;
		position.X += 2 * drawinfo.drawPlayer.direction;
		legFrame.Y = legFrame.Height * 5;
		if (specialLegCoat == 160 || specialLegCoat == 173)
		{
			legFrame = drawinfo.drawPlayer.legFrame;
		}
		DrawData item = new DrawData(textureToDraw, position, legFrame, matchingColor, drawinfo.drawPlayer.legRotation, drawinfo.legVect, 1f, drawinfo.playerEffect);
		item.shader = shaderIndex;
		drawinfo.DrawDataCache.Add(item);
	}

	private static void DrawSittingLegs(ref PlayerDrawSet drawinfo, Texture2D textureToDraw, Color matchingColor, int shaderIndex = 0, bool glowmask = false)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_0227: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_029d: Unknown result type (might be due to invalid IL or missing references)
		//IL_029e: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02af: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02de: Unknown result type (might be due to invalid IL or missing references)
		//IL_02df: Unknown result type (might be due to invalid IL or missing references)
		//IL_0390: Unknown result type (might be due to invalid IL or missing references)
		//IL_0392: Unknown result type (might be due to invalid IL or missing references)
		//IL_0399: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b1: Unknown result type (might be due to invalid IL or missing references)
		Vector2 legsOffset = drawinfo.legsOffset;
		Vector2 vector = new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.legFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.legFrame.Height + 4f)) + drawinfo.drawPlayer.legPosition + drawinfo.legVect;
		Rectangle legFrame = drawinfo.drawPlayer.legFrame;
		vector.Y -= 2f;
		vector.Y += drawinfo.seatYOffset;
		vector += legsOffset;
		int num = 2;
		int num2 = 42;
		int num3 = 2;
		int num4 = 2;
		int num5 = 0;
		int num6 = 0;
		int num7 = 0;
		bool flag = drawinfo.drawPlayer.legs == 101 || drawinfo.drawPlayer.legs == 102 || drawinfo.drawPlayer.legs == 118 || drawinfo.drawPlayer.legs == 99;
		if (drawinfo.drawPlayer.wearsRobe && !flag)
		{
			num = 0;
			num4 = 0;
			num2 = 6;
			vector.Y += 4f;
			legFrame.Y = legFrame.Height * 5;
		}
		switch (drawinfo.drawPlayer.legs)
		{
		case 214:
		case 215:
		case 216:
			num = -6;
			num4 = 2;
			num5 = 2;
			num3 = 4;
			num2 = 6;
			legFrame = drawinfo.drawPlayer.legFrame;
			vector.Y += 2f;
			break;
		case 106:
		case 143:
		case 226:
			num = 0;
			num4 = 0;
			num2 = 6;
			vector.Y += 4f;
			legFrame.Y = legFrame.Height * 5;
			break;
		case 132:
			num = -2;
			num7 = 2;
			break;
		case 193:
		case 194:
			if (drawinfo.drawPlayer.body == 218)
			{
				num = -2;
				num7 = 2;
				vector.Y += 2f;
			}
			break;
		case 210:
			if (glowmask)
			{
				Vector2 vector2 = default(Vector2);
				((Vector2)(ref vector2))._002Ector((float)Main.rand.Next(-10, 10) * 0.125f, (float)Main.rand.Next(-10, 10) * 0.125f);
				vector += vector2;
			}
			break;
		}
		for (int num8 = num3; num8 >= 0; num8--)
		{
			Vector2 position = vector + new Vector2((float)num, 2f) * new Vector2((float)drawinfo.drawPlayer.direction, 1f);
			Rectangle value = legFrame;
			value.Y += num8 * 2;
			value.Y += num2;
			value.Height -= num2;
			value.Height -= num8 * 2;
			if (num8 != num3)
			{
				value.Height = 2;
			}
			position.X += drawinfo.drawPlayer.direction * num4 * num8 + num6 * drawinfo.drawPlayer.direction;
			if (num8 != 0)
			{
				position.X += num7 * drawinfo.drawPlayer.direction;
			}
			position.Y += num2;
			position.Y += num5;
			DrawData item = new DrawData(textureToDraw, position, value, matchingColor, drawinfo.drawPlayer.legRotation, drawinfo.legVect, 1f, drawinfo.playerEffect);
			item.shader = shaderIndex;
			drawinfo.DrawDataCache.Add(item);
		}
	}

	public static void DrawPlayer_14_Shoes(ref PlayerDrawSet drawinfo)
	{
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		if (drawinfo.drawPlayer.shoe <= 0 || ShouldOverrideLegs_CheckPants(ref drawinfo))
		{
			return;
		}
		int num = drawinfo.cShoe;
		if (drawinfo.drawPlayer.shoe == 22 || drawinfo.drawPlayer.shoe == 23)
		{
			num = drawinfo.cFlameWaker;
		}
		if (drawinfo.isSitting)
		{
			DrawSittingLegs(ref drawinfo, TextureAssets.AccShoes[drawinfo.drawPlayer.shoe].Value, drawinfo.colorArmorLegs, num);
			return;
		}
		DrawData item = new DrawData(TextureAssets.AccShoes[drawinfo.drawPlayer.shoe].Value, new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.legFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.legFrame.Height + 4f)) + drawinfo.drawPlayer.legPosition + drawinfo.legVect, drawinfo.drawPlayer.legFrame, drawinfo.colorArmorLegs, drawinfo.drawPlayer.legRotation, drawinfo.legVect, 1f, drawinfo.playerEffect);
		item.shader = num;
		drawinfo.DrawDataCache.Add(item);
		if (drawinfo.drawPlayer.shoe == 25 || drawinfo.drawPlayer.shoe == 26)
		{
			DrawPlayer_14_2_GlassSlipperSparkles(ref drawinfo);
		}
	}

	public static void DrawPlayer_14_2_GlassSlipperSparkles(ref PlayerDrawSet drawinfo)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0191: Unknown result type (might be due to invalid IL or missing references)
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01af: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01de: Unknown result type (might be due to invalid IL or missing references)
		//IL_0206: Unknown result type (might be due to invalid IL or missing references)
		//IL_0210: Unknown result type (might be due to invalid IL or missing references)
		//IL_0215: Unknown result type (might be due to invalid IL or missing references)
		if (!drawinfo.headOnlyRender && drawinfo.shadow == 0f)
		{
			if (Main.rand.Next(60) == 0)
			{
				Rectangle r = Utils.CenteredRectangle(drawinfo.Position + drawinfo.drawPlayer.Size / 2f + new Vector2(0f, drawinfo.drawPlayer.gravDir * 16f), new Vector2(20f, 8f));
				int num = Dust.NewDust(r.TopLeft(), r.Width, r.Height, 204, 0f, 0f, 150, default(Color), 0.3f);
				Main.dust[num].fadeIn = 1f;
				Dust obj = Main.dust[num];
				obj.velocity *= 0.1f;
				Main.dust[num].noLight = true;
				Main.dust[num].shader = GameShaders.Armor.GetSecondaryShader(drawinfo.drawPlayer.cShoe, drawinfo.drawPlayer);
				drawinfo.DustCache.Add(num);
			}
			if (drawinfo.drawPlayer.velocity.X != 0f && Main.rand.Next(10) == 0)
			{
				Rectangle r2 = Utils.CenteredRectangle(drawinfo.Position + drawinfo.drawPlayer.Size / 2f + new Vector2((float)(drawinfo.drawPlayer.direction * -2), drawinfo.drawPlayer.gravDir * 16f), new Vector2(6f, 8f));
				int num2 = Dust.NewDust(r2.TopLeft(), r2.Width, r2.Height, 204, 0f, 0f, 150, default(Color), 0.3f);
				Main.dust[num2].fadeIn = 1f;
				Dust obj2 = Main.dust[num2];
				obj2.velocity *= 0.1f;
				Main.dust[num2].noLight = true;
				Main.dust[num2].shader = GameShaders.Armor.GetSecondaryShader(drawinfo.drawPlayer.cShoe, drawinfo.drawPlayer);
				drawinfo.DustCache.Add(num2);
			}
		}
	}

	public static void DrawPlayer_15_SkinLongCoat(ref PlayerDrawSet drawinfo)
	{
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		if ((drawinfo.skinVar == 3 || drawinfo.skinVar == 8 || drawinfo.skinVar == 7) && drawinfo.drawPlayer.body <= 0 && !drawinfo.drawPlayer.invis)
		{
			if (drawinfo.isSitting)
			{
				DrawSittingLegs(ref drawinfo, TextureAssets.Players[drawinfo.skinVar, 14].Value, drawinfo.colorShirt);
				return;
			}
			DrawData item = new DrawData(TextureAssets.Players[drawinfo.skinVar, 14].Value, new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.legFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.legFrame.Height + 4f)) + drawinfo.drawPlayer.legPosition + drawinfo.legVect, drawinfo.drawPlayer.legFrame, drawinfo.colorShirt, drawinfo.drawPlayer.legRotation, drawinfo.legVect, 1f, drawinfo.playerEffect);
			drawinfo.DrawDataCache.Add(item);
		}
	}

	public static void DrawPlayer_16_ArmorLongCoat(ref PlayerDrawSet drawinfo)
	{
		//IL_0314: Unknown result type (might be due to invalid IL or missing references)
		//IL_031f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0324: Unknown result type (might be due to invalid IL or missing references)
		//IL_032a: Unknown result type (might be due to invalid IL or missing references)
		//IL_032f: Unknown result type (might be due to invalid IL or missing references)
		//IL_033a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0345: Unknown result type (might be due to invalid IL or missing references)
		//IL_0356: Unknown result type (might be due to invalid IL or missing references)
		//IL_0361: Unknown result type (might be due to invalid IL or missing references)
		//IL_027c: Unknown result type (might be due to invalid IL or missing references)
		int num = -1;
		switch (drawinfo.drawPlayer.body)
		{
		case 200:
			num = 149;
			break;
		case 202:
			num = 151;
			break;
		case 201:
			num = 150;
			break;
		case 209:
			num = 160;
			break;
		case 207:
			num = 161;
			break;
		case 198:
			num = 162;
			break;
		case 182:
			num = 163;
			break;
		case 168:
			num = 164;
			break;
		case 73:
			num = 170;
			break;
		case 52:
			num = ((!drawinfo.drawPlayer.Male) ? 172 : 171);
			break;
		case 187:
			num = 173;
			break;
		case 205:
			num = 174;
			break;
		case 53:
			num = ((!drawinfo.drawPlayer.Male) ? 176 : 175);
			break;
		case 210:
			num = ((!drawinfo.drawPlayer.Male) ? 177 : 178);
			break;
		case 211:
			num = ((!drawinfo.drawPlayer.Male) ? 181 : 182);
			break;
		case 218:
			num = 195;
			break;
		case 222:
			num = ((!drawinfo.drawPlayer.Male) ? 200 : 201);
			break;
		case 225:
			num = 206;
			break;
		case 236:
			num = 221;
			break;
		case 237:
			num = 223;
			break;
		case 89:
			num = 186;
			break;
		case 81:
			num = 169;
			break;
		}
		if (num != -1)
		{
			Main.instance.LoadArmorLegs(num);
			if (drawinfo.isSitting && num != 195)
			{
				DrawSittingLongCoats(ref drawinfo, num, TextureAssets.ArmorLeg[num].Value, drawinfo.colorArmorBody, drawinfo.cBody);
				return;
			}
			DrawData item = new DrawData(TextureAssets.ArmorLeg[num].Value, new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.legFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.legFrame.Height + 4f)) + drawinfo.drawPlayer.legPosition + drawinfo.legVect, drawinfo.drawPlayer.legFrame, drawinfo.colorArmorBody, drawinfo.drawPlayer.legRotation, drawinfo.legVect, 1f, drawinfo.playerEffect);
			item.shader = drawinfo.cBody;
			drawinfo.DrawDataCache.Add(item);
		}
	}

	public static void DrawPlayer_17_Torso(ref PlayerDrawSet drawinfo)
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_073a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0745: Unknown result type (might be due to invalid IL or missing references)
		//IL_074a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0775: Unknown result type (might be due to invalid IL or missing references)
		//IL_077a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0785: Unknown result type (might be due to invalid IL or missing references)
		//IL_0790: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_0858: Unknown result type (might be due to invalid IL or missing references)
		//IL_0863: Unknown result type (might be due to invalid IL or missing references)
		//IL_0868: Unknown result type (might be due to invalid IL or missing references)
		//IL_0893: Unknown result type (might be due to invalid IL or missing references)
		//IL_0898: Unknown result type (might be due to invalid IL or missing references)
		//IL_08a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_08bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0504: Unknown result type (might be due to invalid IL or missing references)
		//IL_0509: Unknown result type (might be due to invalid IL or missing references)
		//IL_0534: Unknown result type (might be due to invalid IL or missing references)
		//IL_0539: Unknown result type (might be due to invalid IL or missing references)
		//IL_0544: Unknown result type (might be due to invalid IL or missing references)
		//IL_054f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0560: Unknown result type (might be due to invalid IL or missing references)
		//IL_056b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0617: Unknown result type (might be due to invalid IL or missing references)
		//IL_0622: Unknown result type (might be due to invalid IL or missing references)
		//IL_0627: Unknown result type (might be due to invalid IL or missing references)
		//IL_0652: Unknown result type (might be due to invalid IL or missing references)
		//IL_0657: Unknown result type (might be due to invalid IL or missing references)
		//IL_0662: Unknown result type (might be due to invalid IL or missing references)
		//IL_066d: Unknown result type (might be due to invalid IL or missing references)
		//IL_067e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0689: Unknown result type (might be due to invalid IL or missing references)
		//IL_0976: Unknown result type (might be due to invalid IL or missing references)
		//IL_0981: Unknown result type (might be due to invalid IL or missing references)
		//IL_0986: Unknown result type (might be due to invalid IL or missing references)
		//IL_09b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_09b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_09c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_09cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_09dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_09e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_0185: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0191: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_026e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0279: Unknown result type (might be due to invalid IL or missing references)
		//IL_027e: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0410: Unknown result type (might be due to invalid IL or missing references)
		//IL_041b: Unknown result type (might be due to invalid IL or missing references)
		if (drawinfo.usesCompositeTorso)
		{
			DrawPlayer_17_TorsoComposite(ref drawinfo);
		}
		else if (drawinfo.drawPlayer.body > 0)
		{
			Rectangle bodyFrame = drawinfo.drawPlayer.bodyFrame;
			int num = drawinfo.armorAdjust;
			bodyFrame.X += num;
			bodyFrame.Width -= num;
			if (drawinfo.drawPlayer.direction == -1)
			{
				num = 0;
			}
			if (!drawinfo.drawPlayer.invis || (drawinfo.drawPlayer.body != 21 && drawinfo.drawPlayer.body != 22))
			{
				Texture2D texture = (drawinfo.drawPlayer.Male ? TextureAssets.ArmorBody[drawinfo.drawPlayer.body].Value : TextureAssets.FemaleBody[drawinfo.drawPlayer.body].Value);
				DrawData item2 = new DrawData(texture, new Vector2((float)((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)) + num), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2)), bodyFrame, drawinfo.colorArmorBody, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect);
				item2.shader = drawinfo.cBody;
				drawinfo.DrawDataCache.Add(item2);
				if (drawinfo.bodyGlowMask != -1)
				{
					item2 = new DrawData(TextureAssets.GlowMask[drawinfo.bodyGlowMask].Value, new Vector2((float)((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)) + num), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2)), bodyFrame, drawinfo.bodyGlowColor, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect);
					item2.shader = drawinfo.cBody;
					drawinfo.DrawDataCache.Add(item2);
				}
			}
			if (drawinfo.missingHand && !drawinfo.drawPlayer.invis)
			{
				DrawData drawData = new DrawData(TextureAssets.Players[drawinfo.skinVar, 5].Value, new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2)), drawinfo.drawPlayer.bodyFrame, drawinfo.colorBodySkin, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect);
				drawData.shader = drawinfo.skinDyePacked;
				DrawData item = drawData;
				drawinfo.DrawDataCache.Add(item);
			}
		}
		else if (!drawinfo.drawPlayer.invis)
		{
			DrawData item3;
			if (!drawinfo.drawPlayer.Male)
			{
				item3 = new DrawData(TextureAssets.Players[drawinfo.skinVar, 4].Value, new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2)), drawinfo.drawPlayer.bodyFrame, drawinfo.colorUnderShirt, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect);
				drawinfo.DrawDataCache.Add(item3);
				item3 = new DrawData(TextureAssets.Players[drawinfo.skinVar, 6].Value, new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2)), drawinfo.drawPlayer.bodyFrame, drawinfo.colorShirt, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect);
				drawinfo.DrawDataCache.Add(item3);
			}
			else
			{
				item3 = new DrawData(TextureAssets.Players[drawinfo.skinVar, 4].Value, new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2)), drawinfo.drawPlayer.bodyFrame, drawinfo.colorUnderShirt, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect);
				drawinfo.DrawDataCache.Add(item3);
				item3 = new DrawData(TextureAssets.Players[drawinfo.skinVar, 6].Value, new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2)), drawinfo.drawPlayer.bodyFrame, drawinfo.colorShirt, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect);
				drawinfo.DrawDataCache.Add(item3);
			}
			DrawData drawData2 = new DrawData(TextureAssets.Players[drawinfo.skinVar, 5].Value, new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2)), drawinfo.drawPlayer.bodyFrame, drawinfo.colorBodySkin, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect);
			drawData2.shader = drawinfo.skinDyePacked;
			item3 = drawData2;
			drawinfo.DrawDataCache.Add(item3);
		}
	}

	public static void DrawPlayer_17_TorsoComposite(ref PlayerDrawSet drawinfo)
	{
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_0215: Unknown result type (might be due to invalid IL or missing references)
		//IL_0217: Unknown result type (might be due to invalid IL or missing references)
		//IL_0222: Unknown result type (might be due to invalid IL or missing references)
		//IL_0229: Unknown result type (might be due to invalid IL or missing references)
		//IL_0234: Unknown result type (might be due to invalid IL or missing references)
		//IL_0264: Unknown result type (might be due to invalid IL or missing references)
		//IL_0266: Unknown result type (might be due to invalid IL or missing references)
		//IL_0271: Unknown result type (might be due to invalid IL or missing references)
		//IL_0278: Unknown result type (might be due to invalid IL or missing references)
		//IL_0283: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0302: Unknown result type (might be due to invalid IL or missing references)
		//IL_0304: Unknown result type (might be due to invalid IL or missing references)
		//IL_030f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0316: Unknown result type (might be due to invalid IL or missing references)
		//IL_0321: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01af: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0352: Unknown result type (might be due to invalid IL or missing references)
		//IL_035a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0365: Unknown result type (might be due to invalid IL or missing references)
		//IL_036c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0377: Unknown result type (might be due to invalid IL or missing references)
		Vector2 vector = new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2));
		Vector2 vector2 = Main.OffsetsPlayerHeadgear[drawinfo.drawPlayer.bodyFrame.Y / drawinfo.drawPlayer.bodyFrame.Height];
		vector2.Y -= 2f;
		vector += vector2 * (float)(-((Enum)drawinfo.playerEffect).HasFlag((Enum)(object)(SpriteEffects)2).ToDirectionInt());
		float bodyRotation = drawinfo.drawPlayer.bodyRotation;
		Vector2 val = vector;
		Vector2 bodyVect = drawinfo.bodyVect;
		Vector2 compositeOffset_BackArm = GetCompositeOffset_BackArm(ref drawinfo);
		_ = val + compositeOffset_BackArm;
		bodyVect += compositeOffset_BackArm;
		if (drawinfo.drawPlayer.body > 0)
		{
			if (!drawinfo.drawPlayer.invis || IsArmorDrawnWhenInvisible(drawinfo.drawPlayer.body))
			{
				Texture2D value = TextureAssets.ArmorBodyComposite[drawinfo.drawPlayer.body].Value;
				DrawCompositeArmorPiece(ref drawinfo, CompositePlayerDrawContext.Torso, new DrawData(value, vector, drawinfo.compTorsoFrame, drawinfo.colorArmorBody, bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect)
				{
					shader = drawinfo.cBody
				});
			}
		}
		else if (!drawinfo.drawPlayer.invis)
		{
			drawinfo.DrawDataCache.Add(new DrawData(TextureAssets.Players[drawinfo.skinVar, 4].Value, vector, drawinfo.compBackShoulderFrame, drawinfo.colorUnderShirt, bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect));
			drawinfo.DrawDataCache.Add(new DrawData(TextureAssets.Players[drawinfo.skinVar, 6].Value, vector, drawinfo.compBackShoulderFrame, drawinfo.colorShirt, bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect));
			drawinfo.DrawDataCache.Add(new DrawData(TextureAssets.Players[drawinfo.skinVar, 4].Value, vector, drawinfo.compTorsoFrame, drawinfo.colorUnderShirt, bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect));
			drawinfo.DrawDataCache.Add(new DrawData(TextureAssets.Players[drawinfo.skinVar, 6].Value, vector, drawinfo.compTorsoFrame, drawinfo.colorShirt, bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect));
		}
		if (drawinfo.drawFloatingTube)
		{
			drawinfo.DrawDataCache.Add(new DrawData(TextureAssets.Extra[105].Value, vector, (Rectangle?)new Rectangle(0, 56, 40, 56), drawinfo.floatingTubeColor, bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect, 0f)
			{
				shader = drawinfo.cFloatingTube
			});
		}
	}

	public static void DrawPlayer_18_OffhandAcc(ref PlayerDrawSet drawinfo)
	{
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		if (!drawinfo.usesCompositeBackHandAcc && drawinfo.drawPlayer.handoff > 0)
		{
			DrawData item = new DrawData(TextureAssets.AccHandsOff[drawinfo.drawPlayer.handoff].Value, new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2)), drawinfo.drawPlayer.bodyFrame, drawinfo.colorArmorBody, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect);
			item.shader = drawinfo.cHandOff;
			drawinfo.DrawDataCache.Add(item);
		}
	}

	public static void DrawPlayer_JimsDroneRadio(ref PlayerDrawSet drawinfo)
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		if (drawinfo.drawPlayer.HeldItem.type == 5451 && drawinfo.drawPlayer.itemAnimation == 0)
		{
			Rectangle bodyFrame = drawinfo.drawPlayer.bodyFrame;
			Texture2D value = TextureAssets.Extra[261].Value;
			DrawData item = new DrawData(value, new Vector2((float)((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)) + drawinfo.drawPlayer.direction * 2), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f + 14f)) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2)), bodyFrame, drawinfo.colorArmorLegs, drawinfo.drawPlayer.legRotation, drawinfo.legVect, 1f, drawinfo.playerEffect);
			item.shader = drawinfo.cWaist;
			drawinfo.DrawDataCache.Add(item);
		}
	}

	public static void DrawPlayer_19_WaistAcc(ref PlayerDrawSet drawinfo)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		if (drawinfo.drawPlayer.waist > 0)
		{
			Rectangle value = drawinfo.drawPlayer.legFrame;
			if (ArmorIDs.Waist.Sets.UsesTorsoFraming[drawinfo.drawPlayer.waist])
			{
				value = drawinfo.drawPlayer.bodyFrame;
			}
			DrawData item = new DrawData(TextureAssets.AccWaist[drawinfo.drawPlayer.waist].Value, new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.legFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.legFrame.Height + 4f)) + drawinfo.drawPlayer.legPosition + drawinfo.legVect, value, drawinfo.colorArmorLegs, drawinfo.drawPlayer.legRotation, drawinfo.legVect, 1f, drawinfo.playerEffect);
			item.shader = drawinfo.cWaist;
			drawinfo.DrawDataCache.Add(item);
		}
	}

	public static void DrawPlayer_20_NeckAcc(ref PlayerDrawSet drawinfo)
	{
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		if (drawinfo.drawPlayer.neck > 0)
		{
			DrawData item = new DrawData(TextureAssets.AccNeck[drawinfo.drawPlayer.neck].Value, new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2)), drawinfo.drawPlayer.bodyFrame, drawinfo.colorArmorBody, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect);
			item.shader = drawinfo.cNeck;
			drawinfo.DrawDataCache.Add(item);
		}
	}

	public static void DrawPlayer_21_Head(ref PlayerDrawSet drawinfo)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_021a: Unknown result type (might be due to invalid IL or missing references)
		//IL_021f: Unknown result type (might be due to invalid IL or missing references)
		//IL_022a: Unknown result type (might be due to invalid IL or missing references)
		//IL_022f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0235: Unknown result type (might be due to invalid IL or missing references)
		//IL_023a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0245: Unknown result type (might be due to invalid IL or missing references)
		//IL_024f: Unknown result type (might be due to invalid IL or missing references)
		//IL_025d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0268: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0444: Unknown result type (might be due to invalid IL or missing references)
		//IL_0449: Unknown result type (might be due to invalid IL or missing references)
		//IL_0454: Unknown result type (might be due to invalid IL or missing references)
		//IL_0459: Unknown result type (might be due to invalid IL or missing references)
		//IL_045f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0464: Unknown result type (might be due to invalid IL or missing references)
		//IL_0469: Unknown result type (might be due to invalid IL or missing references)
		//IL_0471: Unknown result type (might be due to invalid IL or missing references)
		//IL_0482: Unknown result type (might be due to invalid IL or missing references)
		//IL_048d: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0542: Unknown result type (might be due to invalid IL or missing references)
		//IL_0547: Unknown result type (might be due to invalid IL or missing references)
		//IL_0552: Unknown result type (might be due to invalid IL or missing references)
		//IL_0557: Unknown result type (might be due to invalid IL or missing references)
		//IL_055d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0562: Unknown result type (might be due to invalid IL or missing references)
		//IL_0567: Unknown result type (might be due to invalid IL or missing references)
		//IL_056f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0580: Unknown result type (might be due to invalid IL or missing references)
		//IL_058b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0332: Unknown result type (might be due to invalid IL or missing references)
		//IL_0335: Unknown result type (might be due to invalid IL or missing references)
		//IL_0340: Unknown result type (might be due to invalid IL or missing references)
		//IL_0351: Unknown result type (might be due to invalid IL or missing references)
		//IL_035c: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_05cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_017f: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_07dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_07de: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_0824: Unknown result type (might be due to invalid IL or missing references)
		//IL_0829: Unknown result type (might be due to invalid IL or missing references)
		//IL_082e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0619: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_064b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0650: Unknown result type (might be due to invalid IL or missing references)
		//IL_1785: Unknown result type (might be due to invalid IL or missing references)
		//IL_1790: Unknown result type (might be due to invalid IL or missing references)
		//IL_1795: Unknown result type (might be due to invalid IL or missing references)
		//IL_179b: Unknown result type (might be due to invalid IL or missing references)
		//IL_17a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_17ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_17b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_17c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_17d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_15b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_15bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_15be: Unknown result type (might be due to invalid IL or missing references)
		//IL_15c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0866: Unknown result type (might be due to invalid IL or missing references)
		//IL_0877: Unknown result type (might be due to invalid IL or missing references)
		//IL_087d: Unknown result type (might be due to invalid IL or missing references)
		//IL_087f: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_08c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_08cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_08f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_06aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0723: Unknown result type (might be due to invalid IL or missing references)
		//IL_0728: Unknown result type (might be due to invalid IL or missing references)
		//IL_0733: Unknown result type (might be due to invalid IL or missing references)
		//IL_0738: Unknown result type (might be due to invalid IL or missing references)
		//IL_073e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0743: Unknown result type (might be due to invalid IL or missing references)
		//IL_0748: Unknown result type (might be due to invalid IL or missing references)
		//IL_074f: Unknown result type (might be due to invalid IL or missing references)
		//IL_075c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0764: Unknown result type (might be due to invalid IL or missing references)
		//IL_15f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_1672: Unknown result type (might be due to invalid IL or missing references)
		//IL_1677: Unknown result type (might be due to invalid IL or missing references)
		//IL_1682: Unknown result type (might be due to invalid IL or missing references)
		//IL_1687: Unknown result type (might be due to invalid IL or missing references)
		//IL_168d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1692: Unknown result type (might be due to invalid IL or missing references)
		//IL_169d: Unknown result type (might be due to invalid IL or missing references)
		//IL_16a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_16b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_16c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_15d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_15de: Unknown result type (might be due to invalid IL or missing references)
		//IL_1528: Unknown result type (might be due to invalid IL or missing references)
		//IL_152b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1536: Unknown result type (might be due to invalid IL or missing references)
		//IL_1547: Unknown result type (might be due to invalid IL or missing references)
		//IL_1552: Unknown result type (might be due to invalid IL or missing references)
		//IL_0950: Unknown result type (might be due to invalid IL or missing references)
		//IL_0953: Unknown result type (might be due to invalid IL or missing references)
		//IL_095e: Unknown result type (might be due to invalid IL or missing references)
		//IL_096f: Unknown result type (might be due to invalid IL or missing references)
		//IL_097a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0683: Unknown result type (might be due to invalid IL or missing references)
		//IL_067b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a25: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a9e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1aa3: Unknown result type (might be due to invalid IL or missing references)
		//IL_1aae: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ab3: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ab9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1abe: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ac9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ae7: Unknown result type (might be due to invalid IL or missing references)
		//IL_1af8: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b03: Unknown result type (might be due to invalid IL or missing references)
		//IL_182a: Unknown result type (might be due to invalid IL or missing references)
		//IL_182f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0688: Unknown result type (might be due to invalid IL or missing references)
		//IL_186c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1881: Unknown result type (might be due to invalid IL or missing references)
		//IL_1892: Unknown result type (might be due to invalid IL or missing references)
		//IL_1897: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d85: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d8a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d8d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d92: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bc0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bc5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bc7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bc9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bd3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bd8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0be7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bec: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bf1: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bbb: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bc2: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bc7: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bd5: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c4e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c53: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c5e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c63: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c69: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c6e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c73: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c75: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c80: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c8a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c98: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ca3: Unknown result type (might be due to invalid IL or missing references)
		//IL_18cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_18e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_18e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_18fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_1973: Unknown result type (might be due to invalid IL or missing references)
		//IL_1978: Unknown result type (might be due to invalid IL or missing references)
		//IL_1983: Unknown result type (might be due to invalid IL or missing references)
		//IL_1988: Unknown result type (might be due to invalid IL or missing references)
		//IL_198e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1993: Unknown result type (might be due to invalid IL or missing references)
		//IL_19be: Unknown result type (might be due to invalid IL or missing references)
		//IL_19c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_19c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_19cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_19dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_19e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_189f: Unknown result type (might be due to invalid IL or missing references)
		//IL_18b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_18c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_18cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a1a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a2b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a31: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a33: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a67: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ae9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aee: Unknown result type (might be due to invalid IL or missing references)
		//IL_0af9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0afe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b04: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b09: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b14: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b1f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b30: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b3b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dd1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dd6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e30: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ea9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eae: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eb9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ebe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ec4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ec9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ece: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ed5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ee2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eea: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e09: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e01: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c7e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c6c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c96: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c9d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e0e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cb0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cc1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cc7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cc9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d00: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d19: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d1e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d23: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d2b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d3b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d43: Unknown result type (might be due to invalid IL or missing references)
		//IL_126c: Unknown result type (might be due to invalid IL or missing references)
		//IL_12e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_12ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_12f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_12fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_1300: Unknown result type (might be due to invalid IL or missing references)
		//IL_1305: Unknown result type (might be due to invalid IL or missing references)
		//IL_130a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1312: Unknown result type (might be due to invalid IL or missing references)
		//IL_1322: Unknown result type (might be due to invalid IL or missing references)
		//IL_132a: Unknown result type (might be due to invalid IL or missing references)
		//IL_13cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_1446: Unknown result type (might be due to invalid IL or missing references)
		//IL_144b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1456: Unknown result type (might be due to invalid IL or missing references)
		//IL_145b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1461: Unknown result type (might be due to invalid IL or missing references)
		//IL_1466: Unknown result type (might be due to invalid IL or missing references)
		//IL_146f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1474: Unknown result type (might be due to invalid IL or missing references)
		//IL_147f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1489: Unknown result type (might be due to invalid IL or missing references)
		//IL_1497: Unknown result type (might be due to invalid IL or missing references)
		//IL_14a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f6f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f74: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f86: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f89: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fa5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0faa: Unknown result type (might be due to invalid IL or missing references)
		//IL_113e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1143: Unknown result type (might be due to invalid IL or missing references)
		//IL_1144: Unknown result type (might be due to invalid IL or missing references)
		//IL_11c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_11c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_11d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_11d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_11dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_11e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_11e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_11f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_11fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_1203: Unknown result type (might be due to invalid IL or missing references)
		//IL_1213: Unknown result type (might be due to invalid IL or missing references)
		//IL_121b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ffa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fff: Unknown result type (might be due to invalid IL or missing references)
		//IL_1005: Unknown result type (might be due to invalid IL or missing references)
		//IL_1007: Unknown result type (might be due to invalid IL or missing references)
		//IL_1008: Unknown result type (might be due to invalid IL or missing references)
		//IL_1085: Unknown result type (might be due to invalid IL or missing references)
		//IL_108a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1095: Unknown result type (might be due to invalid IL or missing references)
		//IL_109a: Unknown result type (might be due to invalid IL or missing references)
		//IL_10a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_10a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_10aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_10b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_10c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_10ce: Unknown result type (might be due to invalid IL or missing references)
		Vector2 helmetOffset = drawinfo.helmetOffset;
		DrawPlayer_21_Head_TheFace(ref drawinfo);
		bool flag = drawinfo.drawPlayer.head >= 0 && ArmorIDs.Head.Sets.IsTallHat[drawinfo.drawPlayer.head];
		bool flag2 = drawinfo.drawPlayer.head == 28;
		bool flag3 = drawinfo.drawPlayer.head == 39 || drawinfo.drawPlayer.head == 38;
		Vector2 vector = default(Vector2);
		((Vector2)(ref vector))._002Ector((float)(-drawinfo.drawPlayer.bodyFrame.Width / 2 + drawinfo.drawPlayer.width / 2), (float)(drawinfo.drawPlayer.height - drawinfo.drawPlayer.bodyFrame.Height + 4));
		Vector2 position = (drawinfo.Position - Main.screenPosition + vector).Floor() + drawinfo.drawPlayer.headPosition + drawinfo.headVect;
		if (((Enum)drawinfo.playerEffect).HasFlag((Enum)(object)(SpriteEffects)2))
		{
			int num = drawinfo.drawPlayer.bodyFrame.Height - drawinfo.hairFrontFrame.Height;
			position.Y += num;
		}
		position += drawinfo.hairOffset;
		if (drawinfo.fullHair)
		{
			Color color = drawinfo.colorArmorHead;
			int shader = drawinfo.cHead;
			if (ArmorIDs.Head.Sets.UseSkinColor[drawinfo.drawPlayer.head])
			{
				color = ((!drawinfo.drawPlayer.isDisplayDollOrInanimate) ? drawinfo.colorHead : drawinfo.colorDisplayDollSkin);
				shader = drawinfo.skinDyePacked;
			}
			DrawData item12 = new DrawData(TextureAssets.ArmorHead[drawinfo.drawPlayer.head].Value, helmetOffset + new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, drawinfo.drawPlayer.bodyFrame, color, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect);
			item12.shader = shader;
			drawinfo.DrawDataCache.Add(item12);
			if (!drawinfo.drawPlayer.invis)
			{
				item12 = new DrawData(TextureAssets.PlayerHair[drawinfo.drawPlayer.hair].Value, position, drawinfo.hairFrontFrame, drawinfo.colorHair, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect);
				item12.shader = drawinfo.hairDyePacked;
				drawinfo.DrawDataCache.Add(item12);
			}
		}
		if (drawinfo.hatHair && !drawinfo.drawPlayer.invis)
		{
			DrawData item13 = new DrawData(TextureAssets.PlayerHairAlt[drawinfo.drawPlayer.hair].Value, position, drawinfo.hairFrontFrame, drawinfo.colorHair, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect);
			item13.shader = drawinfo.hairDyePacked;
			drawinfo.DrawDataCache.Add(item13);
		}
		if (drawinfo.drawPlayer.head == 270)
		{
			Rectangle bodyFrame = drawinfo.drawPlayer.bodyFrame;
			bodyFrame.Width += 2;
			DrawData item5 = new DrawData(TextureAssets.ArmorHead[drawinfo.drawPlayer.head].Value, helmetOffset + new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, bodyFrame, drawinfo.colorArmorHead, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect);
			item5.shader = drawinfo.cHead;
			drawinfo.DrawDataCache.Add(item5);
			item5 = new DrawData(TextureAssets.GlowMask[drawinfo.headGlowMask].Value, helmetOffset + new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, bodyFrame, drawinfo.headGlowColor, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect);
			item5.shader = drawinfo.cHead;
			drawinfo.DrawDataCache.Add(item5);
		}
		else if (flag)
		{
			Rectangle bodyFrame2 = drawinfo.drawPlayer.bodyFrame;
			Vector2 headVect = drawinfo.headVect;
			if (drawinfo.drawPlayer.gravDir == 1f)
			{
				if (bodyFrame2.Y != 0)
				{
					bodyFrame2.Y -= 2;
					headVect.Y += 2f;
				}
				bodyFrame2.Height -= 8;
			}
			else if (bodyFrame2.Y != 0)
			{
				bodyFrame2.Y -= 2;
				headVect.Y -= 10f;
				bodyFrame2.Height -= 8;
			}
			Color color2 = drawinfo.colorArmorHead;
			int shader2 = drawinfo.cHead;
			if (ArmorIDs.Head.Sets.UseSkinColor[drawinfo.drawPlayer.head])
			{
				color2 = ((!drawinfo.drawPlayer.isDisplayDollOrInanimate) ? drawinfo.colorHead : drawinfo.colorDisplayDollSkin);
				shader2 = drawinfo.skinDyePacked;
			}
			DrawData item6 = new DrawData(TextureAssets.ArmorHead[drawinfo.drawPlayer.head].Value, helmetOffset + new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, bodyFrame2, color2, drawinfo.drawPlayer.headRotation, headVect, 1f, drawinfo.playerEffect);
			item6.shader = shader2;
			drawinfo.DrawDataCache.Add(item6);
		}
		else if (drawinfo.drawPlayer.head == 259)
		{
			int verticalFrames = 27;
			Texture2D value = TextureAssets.ArmorHead[drawinfo.drawPlayer.head].Value;
			Rectangle rectangle = value.Frame(1, verticalFrames, 0, drawinfo.drawPlayer.rabbitOrderFrame.DisplayFrame);
			Vector2 origin = rectangle.Size() / 2f;
			int num12 = drawinfo.drawPlayer.babyBird.ToInt();
			Vector2 vector2 = DrawPlayer_21_Head_GetSpecialHatDrawPosition(ref drawinfo, ref helmetOffset, new Vector2((float)(1 + num12 * 2), (float)(-26 + drawinfo.drawPlayer.babyBird.ToInt() * -6)));
			int hatStacks3 = GetHatStacks(ref drawinfo, 4955);
			float num14 = (float)Math.PI / 60f;
			float num15 = num14 * drawinfo.drawPlayer.position.X % ((float)Math.PI * 2f);
			for (int num16 = hatStacks3 - 1; num16 >= 0; num16--)
			{
				float x = Vector2.UnitY.RotatedBy(num15 + num14 * (float)num16).X * ((float)num16 / 30f) * 2f - (float)(num16 * 2 * drawinfo.drawPlayer.direction);
				DrawData item8 = new DrawData(value, vector2 + new Vector2(x, (float)(num16 * -14) * drawinfo.drawPlayer.gravDir), rectangle, drawinfo.colorArmorHead, drawinfo.drawPlayer.headRotation, origin, 1f, drawinfo.playerEffect);
				item8.shader = drawinfo.cHead;
				drawinfo.DrawDataCache.Add(item8);
			}
			if (!drawinfo.drawPlayer.invis)
			{
				DrawData item7 = new DrawData(TextureAssets.PlayerHair[drawinfo.drawPlayer.hair].Value, position, drawinfo.hairFrontFrame, drawinfo.colorHair, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect);
				item7.shader = drawinfo.hairDyePacked;
				drawinfo.DrawDataCache.Add(item7);
			}
		}
		else if (drawinfo.drawPlayer.head > 0 && !flag2)
		{
			if (!(drawinfo.drawPlayer.invis && flag3))
			{
				if (drawinfo.drawPlayer.head == 13)
				{
					int hatStacks2 = GetHatStacks(ref drawinfo, 205);
					float num17 = (float)Math.PI / 60f;
					float num18 = num17 * drawinfo.drawPlayer.position.X % ((float)Math.PI * 2f);
					for (int i = 0; i < hatStacks2; i++)
					{
						float num19 = Vector2.UnitY.RotatedBy(num18 + num17 * (float)i).X * ((float)i / 30f) * 2f;
						DrawData item9 = new DrawData(TextureAssets.ArmorHead[drawinfo.drawPlayer.head].Value, helmetOffset + new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)) + num19, (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f - (float)(4 * i))) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, drawinfo.drawPlayer.bodyFrame, drawinfo.colorArmorHead, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect);
						item9.shader = drawinfo.cHead;
						drawinfo.DrawDataCache.Add(item9);
					}
				}
				else if (drawinfo.drawPlayer.head == 265)
				{
					int verticalFrames2 = 6;
					Texture2D value2 = TextureAssets.ArmorHead[drawinfo.drawPlayer.head].Value;
					Rectangle rectangle2 = value2.Frame(1, verticalFrames2, 0, drawinfo.drawPlayer.rabbitOrderFrame.DisplayFrame);
					Vector2 origin2 = rectangle2.Size() / 2f;
					Vector2 vector3 = DrawPlayer_21_Head_GetSpecialHatDrawPosition(ref drawinfo, ref helmetOffset, new Vector2(0f, -9f));
					int hatStacks4 = GetHatStacks(ref drawinfo, 5004);
					float num20 = (float)Math.PI / 60f;
					float num2 = num20 * drawinfo.drawPlayer.position.X % ((float)Math.PI * 2f);
					int num3 = hatStacks4 * 4 + 2;
					int num4 = 0;
					bool flag4 = (Main.GlobalTimeWrappedHourly + 180f) % 600f < 60f;
					for (int num5 = num3 - 1; num5 >= 0; num5--)
					{
						int num6 = 0;
						if (num5 == num3 - 1)
						{
							rectangle2.Y = 0;
							num6 = 2;
						}
						else if (num5 == 0)
						{
							rectangle2.Y = rectangle2.Height * 5;
						}
						else
						{
							rectangle2.Y = rectangle2.Height * (num4++ % 4 + 1);
						}
						if (!(rectangle2.Y == rectangle2.Height * 3 && flag4))
						{
							float x2 = Vector2.UnitY.RotatedBy(num2 + num20 * (float)num5).X * ((float)num5 / 10f) * 4f - (float)num5 * 0.1f * (float)drawinfo.drawPlayer.direction;
							DrawData item10 = new DrawData(value2, vector3 + new Vector2(x2, (float)(num5 * -4 + num6) * drawinfo.drawPlayer.gravDir), rectangle2, drawinfo.colorArmorHead, drawinfo.drawPlayer.headRotation, origin2, 1f, drawinfo.playerEffect);
							item10.shader = drawinfo.cHead;
							drawinfo.DrawDataCache.Add(item10);
						}
					}
				}
				else
				{
					Rectangle bodyFrame3 = drawinfo.drawPlayer.bodyFrame;
					Vector2 headVect2 = drawinfo.headVect;
					if (drawinfo.drawPlayer.gravDir == 1f)
					{
						bodyFrame3.Height -= 4;
					}
					else
					{
						headVect2.Y -= 4f;
						bodyFrame3.Height -= 4;
					}
					Color color3 = drawinfo.colorArmorHead;
					int shader3 = drawinfo.cHead;
					if (ArmorIDs.Head.Sets.UseSkinColor[drawinfo.drawPlayer.head])
					{
						color3 = ((!drawinfo.drawPlayer.isDisplayDollOrInanimate) ? drawinfo.colorHead : drawinfo.colorDisplayDollSkin);
						shader3 = drawinfo.skinDyePacked;
					}
					DrawData item11 = new DrawData(TextureAssets.ArmorHead[drawinfo.drawPlayer.head].Value, helmetOffset + new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, bodyFrame3, color3, drawinfo.drawPlayer.headRotation, headVect2, 1f, drawinfo.playerEffect);
					item11.shader = shader3;
					drawinfo.DrawDataCache.Add(item11);
					if (drawinfo.headGlowMask != -1)
					{
						if (drawinfo.headGlowMask == 309)
						{
							int num7 = DrawPlayer_Head_GetTVScreen(drawinfo.drawPlayer);
							if (num7 != 0)
							{
								int num8 = 0;
								num8 += drawinfo.drawPlayer.bodyFrame.Y / 56;
								if (num8 >= Main.OffsetsPlayerHeadgear.Length)
								{
									num8 = 0;
								}
								Vector2 vector4 = Main.OffsetsPlayerHeadgear[num8];
								vector4.Y -= 2f;
								vector4 *= (float)(-((Enum)drawinfo.playerEffect).HasFlag((Enum)(object)(SpriteEffects)2).ToDirectionInt());
								Texture2D value3 = TextureAssets.GlowMask[drawinfo.headGlowMask].Value;
								int frameY = drawinfo.drawPlayer.miscCounter % 20 / 5;
								if (num7 == 5)
								{
									frameY = 0;
									if (drawinfo.drawPlayer.eyeHelper.EyeFrameToShow > 0)
									{
										frameY = 2;
									}
								}
								Rectangle value4 = value3.Frame(6, 4, num7, frameY, -2);
								item11 = new DrawData(value3, vector4 + helmetOffset + new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, value4, drawinfo.headGlowColor, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect);
								item11.shader = drawinfo.cHead;
								drawinfo.DrawDataCache.Add(item11);
							}
						}
						else if (drawinfo.headGlowMask == 273)
						{
							for (int j = 0; j < 2; j++)
							{
								Vector2 position2 = new Vector2((float)Main.rand.Next(-10, 10) * 0.125f, (float)Main.rand.Next(-10, 10) * 0.125f) + helmetOffset + new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.headPosition + drawinfo.headVect;
								item11 = new DrawData(TextureAssets.GlowMask[drawinfo.headGlowMask].Value, position2, bodyFrame3, drawinfo.headGlowColor, drawinfo.drawPlayer.headRotation, headVect2, 1f, drawinfo.playerEffect);
								item11.shader = drawinfo.cHead;
								drawinfo.DrawDataCache.Add(item11);
							}
						}
						else
						{
							item11 = new DrawData(TextureAssets.GlowMask[drawinfo.headGlowMask].Value, helmetOffset + new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, bodyFrame3, drawinfo.headGlowColor, drawinfo.drawPlayer.headRotation, headVect2, 1f, drawinfo.playerEffect);
							item11.shader = drawinfo.cHead;
							drawinfo.DrawDataCache.Add(item11);
						}
					}
					if (drawinfo.drawPlayer.head == 211)
					{
						Color color4 = default(Color);
						((Color)(ref color4))._002Ector(100, 100, 100, 0);
						ulong seed = (ulong)(drawinfo.drawPlayer.miscCounter / 4 + 100);
						int num9 = 4;
						for (int k = 0; k < num9; k++)
						{
							float x3 = (float)Utils.RandomInt(ref seed, -10, 11) * 0.2f;
							float y = (float)Utils.RandomInt(ref seed, -14, 1) * 0.15f;
							item11 = new DrawData(TextureAssets.GlowMask[241].Value, helmetOffset + new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.headPosition + drawinfo.headVect + new Vector2(x3, y), drawinfo.drawPlayer.bodyFrame, color4, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect);
							item11.shader = drawinfo.cHead;
							drawinfo.DrawDataCache.Add(item11);
						}
					}
				}
			}
		}
		else if (!drawinfo.drawPlayer.invis && (drawinfo.drawPlayer.face < 0 || !ArmorIDs.Face.Sets.PreventHairDraw[drawinfo.drawPlayer.face]))
		{
			DrawData item14 = new DrawData(TextureAssets.PlayerHair[drawinfo.drawPlayer.hair].Value, position, drawinfo.hairFrontFrame, drawinfo.colorHair, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect);
			item14.shader = drawinfo.hairDyePacked;
			drawinfo.DrawDataCache.Add(item14);
		}
		if (drawinfo.drawPlayer.beard > 0 && (drawinfo.drawPlayer.head < 0 || !ArmorIDs.Head.Sets.PreventBeardDraw[drawinfo.drawPlayer.head]))
		{
			Vector2 beardDrawOffsetFromHelmet = drawinfo.drawPlayer.GetBeardDrawOffsetFromHelmet();
			Color color5 = drawinfo.colorArmorHead;
			if (ArmorIDs.Beard.Sets.UseHairColor[drawinfo.drawPlayer.beard])
			{
				color5 = drawinfo.colorHair;
			}
			DrawData item4 = new DrawData(TextureAssets.AccBeard[drawinfo.drawPlayer.beard].Value, beardDrawOffsetFromHelmet + new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, drawinfo.drawPlayer.bodyFrame, color5, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect);
			item4.shader = drawinfo.cBeard;
			drawinfo.DrawDataCache.Add(item4);
		}
		if (drawinfo.drawPlayer.head == 205)
		{
			DrawData drawData = new DrawData(TextureAssets.Extra[77].Value, new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, drawinfo.drawPlayer.bodyFrame, drawinfo.colorHead, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect);
			drawData.shader = drawinfo.skinDyePacked;
			DrawData item3 = drawData;
			drawinfo.DrawDataCache.Add(item3);
		}
		if (drawinfo.drawPlayer.head == 214 && !drawinfo.drawPlayer.invis)
		{
			Rectangle bodyFrame4 = drawinfo.drawPlayer.bodyFrame;
			bodyFrame4.Y = 0;
			float num10 = (float)drawinfo.drawPlayer.miscCounter / 300f;
			Color color6 = default(Color);
			((Color)(ref color6))._002Ector(0, 0, 0, 0);
			float num11 = 0.8f;
			float num13 = 0.9f;
			if (num10 >= num11)
			{
				color6 = Color.Lerp(Color.Transparent, new Color(200, 200, 200, 0), Utils.GetLerpValue(num11, num13, num10, clamped: true));
			}
			if (num10 >= num13)
			{
				color6 = Color.Lerp(Color.Transparent, new Color(200, 200, 200, 0), Utils.GetLerpValue(1f, num13, num10, clamped: true));
			}
			color6 *= drawinfo.stealth * (1f - drawinfo.shadow);
			DrawData item2 = new DrawData(TextureAssets.Extra[90].Value, helmetOffset + new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.headPosition + drawinfo.headVect - Main.OffsetsPlayerHeadgear[drawinfo.drawPlayer.bodyFrame.Y / drawinfo.drawPlayer.bodyFrame.Height], bodyFrame4, color6, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect);
			drawinfo.DrawDataCache.Add(item2);
		}
		if (drawinfo.drawPlayer.head == 137)
		{
			DrawData item = new DrawData(TextureAssets.JackHat.Value, helmetOffset + new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, drawinfo.drawPlayer.bodyFrame, new Color(255, 255, 255, 255), drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect);
			drawinfo.DrawDataCache.Add(item);
			Color color7 = default(Color);
			Vector2 vector5 = default(Vector2);
			for (int l = 0; l < 7; l++)
			{
				((Color)(ref color7))._002Ector(110 - l * 10, 110 - l * 10, 110 - l * 10, 110 - l * 10);
				((Vector2)(ref vector5))._002Ector((float)Main.rand.Next(-10, 11) * 0.2f, (float)Main.rand.Next(-10, 11) * 0.2f);
				vector5.X = drawinfo.drawPlayer.itemFlamePos[l].X;
				vector5.Y = drawinfo.drawPlayer.itemFlamePos[l].Y;
				vector5 *= 0.5f;
				item = new DrawData(TextureAssets.JackHat.Value, helmetOffset + new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.headPosition + drawinfo.headVect + vector5, drawinfo.drawPlayer.bodyFrame, color7, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect);
				drawinfo.DrawDataCache.Add(item);
			}
		}
	}

	public static void DrawPlayer_21_2_FinchNest(ref PlayerDrawSet drawinfo)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0189: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_019e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		if (drawinfo.drawPlayer.babyBird)
		{
			Rectangle bodyFrame5 = drawinfo.drawPlayer.bodyFrame;
			bodyFrame5.Y = 0;
			Vector2 vector6 = Vector2.Zero;
			Color color8 = drawinfo.colorArmorHead;
			if (drawinfo.drawPlayer.mount.Active && drawinfo.drawPlayer.mount.Type == 52)
			{
				Vector2 mountedCenter = drawinfo.drawPlayer.MountedCenter;
				color8 = drawinfo.drawPlayer.GetImmuneAlphaPure(Lighting.GetColorClamped((int)mountedCenter.X / 16, (int)mountedCenter.Y / 16, Color.White), drawinfo.shadow);
				vector6 = new Vector2(0f, 6f) * drawinfo.drawPlayer.Directions;
			}
			DrawData item = new DrawData(TextureAssets.Extra[100].Value, vector6 + new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.headPosition + drawinfo.headVect + Main.OffsetsPlayerHeadgear[drawinfo.drawPlayer.bodyFrame.Y / drawinfo.drawPlayer.bodyFrame.Height] * drawinfo.drawPlayer.gravDir, bodyFrame5, color8, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect);
			drawinfo.DrawDataCache.Add(item);
		}
	}

	public static int DrawPlayer_Head_GetTVScreen(Player plr)
	{
		if (NPC.AnyDanger())
		{
			return 4;
		}
		if (plr.statLife <= plr.statLifeMax2 / 4)
		{
			return 1;
		}
		if (plr.ZoneCorrupt || plr.ZoneCrimson || plr.ZoneGraveyard)
		{
			return 0;
		}
		if (plr.wet)
		{
			return 2;
		}
		if (plr.townNPCs >= 3f || BirthdayParty.PartyIsUp || LanternNight.LanternsUp)
		{
			return 5;
		}
		return 3;
	}

	private static int GetHatStacks(ref PlayerDrawSet drawinfo, int hatItemId)
	{
		int num = 0;
		int num2 = 0;
		if (drawinfo.drawPlayer.armor[num2] != null && drawinfo.drawPlayer.armor[num2].type == hatItemId && drawinfo.drawPlayer.armor[num2].stack > 0)
		{
			num += drawinfo.drawPlayer.armor[num2].stack;
		}
		num2 = 10;
		if (drawinfo.drawPlayer.armor[num2] != null && drawinfo.drawPlayer.armor[num2].type == hatItemId && drawinfo.drawPlayer.armor[num2].stack > 0)
		{
			num += drawinfo.drawPlayer.armor[num2].stack;
		}
		if (num > 2)
		{
			num = 2;
		}
		return num;
	}

	private static Vector2 DrawPlayer_21_Head_GetSpecialHatDrawPosition(ref PlayerDrawSet drawinfo, ref Vector2 helmetOffset, Vector2 hatOffset)
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		Vector2 vector = Main.OffsetsPlayerHeadgear[drawinfo.drawPlayer.bodyFrame.Y / drawinfo.drawPlayer.bodyFrame.Height] * drawinfo.drawPlayer.Directions;
		Vector2 vec = drawinfo.Position - Main.screenPosition + helmetOffset + new Vector2((float)(-drawinfo.drawPlayer.bodyFrame.Width / 2 + drawinfo.drawPlayer.width / 2), (float)(drawinfo.drawPlayer.height - drawinfo.drawPlayer.bodyFrame.Height + 4)) + hatOffset * drawinfo.drawPlayer.Directions + vector;
		vec = vec.Floor();
		vec += drawinfo.drawPlayer.headPosition + drawinfo.headVect;
		if (drawinfo.drawPlayer.gravDir == -1f)
		{
			vec.Y += 12f;
		}
		return vec.Floor();
	}

	private static void DrawPlayer_21_Head_TheFace(ref PlayerDrawSet drawinfo)
	{
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0358: Unknown result type (might be due to invalid IL or missing references)
		//IL_0363: Unknown result type (might be due to invalid IL or missing references)
		//IL_0368: Unknown result type (might be due to invalid IL or missing references)
		//IL_036e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0373: Unknown result type (might be due to invalid IL or missing references)
		//IL_037e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0389: Unknown result type (might be due to invalid IL or missing references)
		//IL_039a: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0462: Unknown result type (might be due to invalid IL or missing references)
		//IL_046d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0472: Unknown result type (might be due to invalid IL or missing references)
		//IL_0478: Unknown result type (might be due to invalid IL or missing references)
		//IL_047d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0488: Unknown result type (might be due to invalid IL or missing references)
		//IL_0493: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04af: Unknown result type (might be due to invalid IL or missing references)
		//IL_055b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0566: Unknown result type (might be due to invalid IL or missing references)
		//IL_056b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0571: Unknown result type (might be due to invalid IL or missing references)
		//IL_0576: Unknown result type (might be due to invalid IL or missing references)
		//IL_0581: Unknown result type (might be due to invalid IL or missing references)
		//IL_058c: Unknown result type (might be due to invalid IL or missing references)
		//IL_059d: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_060a: Unknown result type (might be due to invalid IL or missing references)
		//IL_060f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0621: Unknown result type (might be due to invalid IL or missing references)
		//IL_0624: Unknown result type (might be due to invalid IL or missing references)
		//IL_0640: Unknown result type (might be due to invalid IL or missing references)
		//IL_0645: Unknown result type (might be due to invalid IL or missing references)
		//IL_065e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0663: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_06fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0701: Unknown result type (might be due to invalid IL or missing references)
		//IL_0706: Unknown result type (might be due to invalid IL or missing references)
		//IL_0708: Unknown result type (might be due to invalid IL or missing references)
		//IL_070d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0715: Unknown result type (might be due to invalid IL or missing references)
		//IL_0726: Unknown result type (might be due to invalid IL or missing references)
		//IL_0731: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0800: Unknown result type (might be due to invalid IL or missing references)
		//IL_0805: Unknown result type (might be due to invalid IL or missing references)
		//IL_080b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0810: Unknown result type (might be due to invalid IL or missing references)
		//IL_081b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0826: Unknown result type (might be due to invalid IL or missing references)
		//IL_0837: Unknown result type (might be due to invalid IL or missing references)
		//IL_0842: Unknown result type (might be due to invalid IL or missing references)
		//IL_023c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0247: Unknown result type (might be due to invalid IL or missing references)
		//IL_024c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0252: Unknown result type (might be due to invalid IL or missing references)
		//IL_0257: Unknown result type (might be due to invalid IL or missing references)
		//IL_0262: Unknown result type (might be due to invalid IL or missing references)
		//IL_026d: Unknown result type (might be due to invalid IL or missing references)
		//IL_027e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0289: Unknown result type (might be due to invalid IL or missing references)
		//IL_0926: Unknown result type (might be due to invalid IL or missing references)
		//IL_0931: Unknown result type (might be due to invalid IL or missing references)
		//IL_0936: Unknown result type (might be due to invalid IL or missing references)
		//IL_093c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0941: Unknown result type (might be due to invalid IL or missing references)
		//IL_094c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0957: Unknown result type (might be due to invalid IL or missing references)
		//IL_0968: Unknown result type (might be due to invalid IL or missing references)
		//IL_0973: Unknown result type (might be due to invalid IL or missing references)
		bool flag = drawinfo.drawPlayer.head > 0 && !ArmorIDs.Head.Sets.DrawHead[drawinfo.drawPlayer.head];
		if (!flag && drawinfo.drawPlayer.faceHead > 0)
		{
			Vector2 faceHeadOffsetFromHelmet = drawinfo.drawPlayer.GetFaceHeadOffsetFromHelmet();
			DrawData item = new DrawData(TextureAssets.AccFace[drawinfo.drawPlayer.faceHead].Value, new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.headPosition + drawinfo.headVect + faceHeadOffsetFromHelmet, drawinfo.drawPlayer.bodyFrame, drawinfo.colorArmorHead, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect);
			item.shader = drawinfo.cFaceHead;
			drawinfo.DrawDataCache.Add(item);
			if (drawinfo.drawPlayer.face > 0 && ArmorIDs.Face.Sets.DrawInFaceUnderHairLayer[drawinfo.drawPlayer.face])
			{
				float num = 0f;
				if (drawinfo.drawPlayer.face == 5 && (uint)(drawinfo.drawPlayer.faceHead - 10) <= 3u)
				{
					num = 2 * drawinfo.drawPlayer.direction;
				}
				item = new DrawData(TextureAssets.AccFace[drawinfo.drawPlayer.face].Value, new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)) + num, (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, drawinfo.drawPlayer.bodyFrame, drawinfo.colorArmorHead, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect);
				item.shader = drawinfo.cFace;
				drawinfo.DrawDataCache.Add(item);
			}
		}
		else if (!drawinfo.drawPlayer.invis && !flag)
		{
			DrawData drawData = new DrawData(TextureAssets.Players[drawinfo.skinVar, 0].Value, new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, drawinfo.drawPlayer.bodyFrame, drawinfo.colorHead, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect);
			drawData.shader = drawinfo.skinDyePacked;
			DrawData item2 = drawData;
			drawinfo.DrawDataCache.Add(item2);
			item2 = new DrawData(TextureAssets.Players[drawinfo.skinVar, 1].Value, new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, drawinfo.drawPlayer.bodyFrame, drawinfo.colorEyeWhites, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect);
			drawinfo.DrawDataCache.Add(item2);
			item2 = new DrawData(TextureAssets.Players[drawinfo.skinVar, 2].Value, new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, drawinfo.drawPlayer.bodyFrame, drawinfo.colorEyes, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect);
			drawinfo.DrawDataCache.Add(item2);
			Asset<Texture2D> asset = TextureAssets.Players[drawinfo.skinVar, 15];
			if (asset.IsLoaded)
			{
				Vector2 vector = Main.OffsetsPlayerHeadgear[drawinfo.drawPlayer.bodyFrame.Y / drawinfo.drawPlayer.bodyFrame.Height];
				vector.Y -= 2f;
				vector *= (float)(-((Enum)drawinfo.playerEffect).HasFlag((Enum)(object)(SpriteEffects)2).ToDirectionInt());
				Rectangle value = asset.Frame(1, 3, 0, drawinfo.drawPlayer.eyeHelper.EyeFrameToShow);
				drawData = new DrawData(asset.Value, new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.headPosition + drawinfo.headVect + vector, value, drawinfo.colorHead, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect);
				drawData.shader = drawinfo.skinDyePacked;
				item2 = drawData;
				drawinfo.DrawDataCache.Add(item2);
			}
			if (drawinfo.drawPlayer.yoraiz0rDarkness)
			{
				drawData = new DrawData(TextureAssets.Extra[67].Value, new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, drawinfo.drawPlayer.bodyFrame, drawinfo.colorHead, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect);
				drawData.shader = drawinfo.skinDyePacked;
				item2 = drawData;
				drawinfo.DrawDataCache.Add(item2);
			}
			if (drawinfo.drawPlayer.face > 0 && ArmorIDs.Face.Sets.DrawInFaceUnderHairLayer[drawinfo.drawPlayer.face])
			{
				item2 = new DrawData(TextureAssets.AccFace[drawinfo.drawPlayer.face].Value, new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, drawinfo.drawPlayer.bodyFrame, drawinfo.colorArmorHead, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect);
				item2.shader = drawinfo.cFace;
				drawinfo.DrawDataCache.Add(item2);
			}
		}
	}

	public static void DrawPlayer_21_1_Magiluminescence(ref PlayerDrawSet drawinfo)
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		//IL_0182: Unknown result type (might be due to invalid IL or missing references)
		if (drawinfo.shadow == 0f && drawinfo.drawPlayer.neck == 11 && !drawinfo.hideEntirePlayer)
		{
			Color colorArmorBody = drawinfo.colorArmorBody;
			Color value = default(Color);
			((Color)(ref value))._002Ector(140, 140, 35, 12);
			float amount = (float)(((Color)(ref colorArmorBody)).R + ((Color)(ref colorArmorBody)).G + ((Color)(ref colorArmorBody)).B) / 3f / 255f;
			value = Color.Lerp(value, Color.Transparent, amount);
			if (!(value == Color.Transparent))
			{
				DrawData item = new DrawData(TextureAssets.GlowMask[310].Value, new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2)), drawinfo.drawPlayer.bodyFrame, value, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect);
				item.shader = drawinfo.cNeck;
				drawinfo.DrawDataCache.Add(item);
			}
		}
	}

	public static void DrawPlayer_22_FaceAcc(ref PlayerDrawSet drawinfo)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_0269: Unknown result type (might be due to invalid IL or missing references)
		//IL_026e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0279: Unknown result type (might be due to invalid IL or missing references)
		//IL_027e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0284: Unknown result type (might be due to invalid IL or missing references)
		//IL_0289: Unknown result type (might be due to invalid IL or missing references)
		//IL_0294: Unknown result type (might be due to invalid IL or missing references)
		//IL_029f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0300: Unknown result type (might be due to invalid IL or missing references)
		//IL_0379: Unknown result type (might be due to invalid IL or missing references)
		//IL_037e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0389: Unknown result type (might be due to invalid IL or missing references)
		//IL_038e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0394: Unknown result type (might be due to invalid IL or missing references)
		//IL_0399: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03af: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_016d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		//IL_0189: Unknown result type (might be due to invalid IL or missing references)
		//IL_0194: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0419: Unknown result type (might be due to invalid IL or missing references)
		//IL_0424: Unknown result type (might be due to invalid IL or missing references)
		//IL_0429: Unknown result type (might be due to invalid IL or missing references)
		//IL_042b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0438: Unknown result type (might be due to invalid IL or missing references)
		//IL_043d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0458: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0506: Unknown result type (might be due to invalid IL or missing references)
		//IL_0514: Unknown result type (might be due to invalid IL or missing references)
		//IL_051f: Unknown result type (might be due to invalid IL or missing references)
		Vector2 vector = Vector2.Zero;
		if (drawinfo.drawPlayer.mount.Active && drawinfo.drawPlayer.mount.Type == 52)
		{
			((Vector2)(ref vector))._002Ector(28f, -2f);
		}
		vector *= drawinfo.drawPlayer.Directions;
		if (drawinfo.drawPlayer.face > 0 && !ArmorIDs.Face.Sets.DrawInFaceUnderHairLayer[drawinfo.drawPlayer.face])
		{
			Vector2 vector2 = Vector2.Zero;
			if (drawinfo.drawPlayer.face == 19)
			{
				vector2 = new Vector2(0f, -6f) * drawinfo.drawPlayer.Directions;
			}
			DrawData item4 = new DrawData(TextureAssets.AccFace[drawinfo.drawPlayer.face].Value, vector2 + vector + new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, drawinfo.drawPlayer.bodyFrame, drawinfo.colorArmorHead, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect);
			item4.shader = drawinfo.cFace;
			drawinfo.DrawDataCache.Add(item4);
		}
		if (drawinfo.drawPlayer.faceFlower > 0)
		{
			DrawData item3 = new DrawData(TextureAssets.AccFace[drawinfo.drawPlayer.faceFlower].Value, vector + drawinfo.helmetOffset + new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, drawinfo.drawPlayer.bodyFrame, drawinfo.colorArmorHead, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect);
			item3.shader = drawinfo.cFaceFlower;
			drawinfo.DrawDataCache.Add(item3);
		}
		if (drawinfo.drawUnicornHorn)
		{
			DrawData item2 = new DrawData(TextureAssets.Extra[143].Value, vector + new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, drawinfo.drawPlayer.bodyFrame, drawinfo.colorArmorHead, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect);
			item2.shader = drawinfo.cUnicornHorn;
			drawinfo.DrawDataCache.Add(item2);
		}
		if (drawinfo.drawAngelHalo)
		{
			Color immuneAlphaPure = drawinfo.drawPlayer.GetImmuneAlphaPure(new Color(200, 200, 200, 150), drawinfo.shadow);
			immuneAlphaPure *= drawinfo.drawPlayer.stealth;
			Main.instance.LoadAccFace(7);
			DrawData item = new DrawData(TextureAssets.AccFace[7].Value, vector + new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.headPosition + drawinfo.headVect, drawinfo.drawPlayer.bodyFrame, immuneAlphaPure, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect);
			item.shader = drawinfo.cAngelHalo;
			drawinfo.DrawDataCache.Add(item);
		}
	}

	public static void DrawTiedBalloons(ref PlayerDrawSet drawinfo)
	{
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		if (drawinfo.drawPlayer.mount.Type == 34)
		{
			Texture2D value = TextureAssets.Extra[141].Value;
			Vector2 vector = default(Vector2);
			((Vector2)(ref vector))._002Ector(0f, 4f);
			Color colorMount = drawinfo.colorMount;
			int frameY = (int)(Main.GlobalTimeWrappedHourly * 3f + drawinfo.drawPlayer.position.X / 50f) % 3;
			Rectangle value2 = value.Frame(1, 3, 0, frameY);
			Vector2 origin = default(Vector2);
			((Vector2)(ref origin))._002Ector((float)(value2.Width / 2), (float)value2.Height);
			float rotation = (0f - drawinfo.drawPlayer.velocity.X) * 0.1f - drawinfo.drawPlayer.fullRotation;
			DrawData item = new DrawData(value, drawinfo.drawPlayer.MountedCenter + vector - Main.screenPosition, value2, colorMount, rotation, origin, 1f, drawinfo.playerEffect);
			drawinfo.DrawDataCache.Add(item);
		}
	}

	public static void DrawStarboardRainbowTrail(ref PlayerDrawSet drawinfo, Vector2 commonWingPosPreFloor, Vector2 dirsVec)
	{
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0174: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0222: Unknown result type (might be due to invalid IL or missing references)
		//IL_0229: Unknown result type (might be due to invalid IL or missing references)
		//IL_0288: Unknown result type (might be due to invalid IL or missing references)
		//IL_028c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0292: Unknown result type (might be due to invalid IL or missing references)
		//IL_0297: Unknown result type (might be due to invalid IL or missing references)
		//IL_0299: Unknown result type (might be due to invalid IL or missing references)
		//IL_029b: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_02af: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0301: Unknown result type (might be due to invalid IL or missing references)
		//IL_0303: Unknown result type (might be due to invalid IL or missing references)
		//IL_0307: Unknown result type (might be due to invalid IL or missing references)
		//IL_030c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0311: Unknown result type (might be due to invalid IL or missing references)
		//IL_0320: Unknown result type (might be due to invalid IL or missing references)
		//IL_0324: Unknown result type (might be due to invalid IL or missing references)
		//IL_0326: Unknown result type (might be due to invalid IL or missing references)
		//IL_0329: Unknown result type (might be due to invalid IL or missing references)
		if (drawinfo.shadow != 0f)
		{
			return;
		}
		int num = Math.Min(drawinfo.drawPlayer.availableAdvancedShadowsCount - 1, 30);
		float num2 = 0f;
		for (int num3 = num; num3 > 0; num3--)
		{
			EntityShadowInfo advancedShadow = drawinfo.drawPlayer.GetAdvancedShadow(num3);
			float num10 = num2;
			Vector2 position = drawinfo.drawPlayer.GetAdvancedShadow(num3 - 1).Position;
			num2 = num10 + Vector2.Distance(advancedShadow.Position, position);
		}
		float num4 = MathHelper.Clamp(num2 / 160f, 0f, 1f);
		Main.instance.LoadProjectile(250);
		Texture2D value = TextureAssets.Projectile[250].Value;
		float x = 1.7f;
		Vector2 origin = default(Vector2);
		((Vector2)(ref origin))._002Ector((float)(value.Width / 2), (float)(value.Height / 2));
		Vector2 val = new Vector2((float)drawinfo.drawPlayer.width, (float)drawinfo.drawPlayer.height) / 2f;
		Color white = Color.White;
		((Color)(ref white)).A = 64;
		Vector2 vector2 = val;
		vector2 = drawinfo.drawPlayer.DefaultSize * new Vector2(0.5f, 1f) + new Vector2(0f, -4f);
		if (dirsVec.Y < 0f)
		{
			vector2 = drawinfo.drawPlayer.DefaultSize * new Vector2(0.5f, 0f) + new Vector2(0f, 4f);
		}
		Vector2 scale = default(Vector2);
		for (int num5 = num; num5 > 0; num5--)
		{
			EntityShadowInfo advancedShadow2 = drawinfo.drawPlayer.GetAdvancedShadow(num5);
			EntityShadowInfo advancedShadow3 = drawinfo.drawPlayer.GetAdvancedShadow(num5 - 1);
			Vector2 pos = advancedShadow2.Position + vector2 + advancedShadow2.HeadgearOffset;
			Vector2 pos2 = advancedShadow3.Position + vector2 + advancedShadow3.HeadgearOffset;
			pos = drawinfo.drawPlayer.RotatedRelativePoint(pos, reverseRotation: true, addGfxOffY: false);
			pos2 = drawinfo.drawPlayer.RotatedRelativePoint(pos2, reverseRotation: true, addGfxOffY: false);
			float num6 = (pos2 - pos).ToRotation() - (float)Math.PI / 2f;
			num6 = (float)Math.PI / 2f * (float)drawinfo.drawPlayer.direction;
			float num7 = Math.Abs(pos2.X - pos.X);
			((Vector2)(ref scale))._002Ector(x, num7 / (float)value.Height);
			float num8 = 1f - (float)num5 / (float)num;
			num8 *= num8;
			num8 *= Utils.GetLerpValue(0f, 4f, num7, clamped: true);
			num8 *= 0.5f;
			num8 *= num8;
			Color color = white * num8 * num4;
			if (!(color == Color.Transparent))
			{
				DrawData item = new DrawData(value, pos - Main.screenPosition, null, color, num6, origin, scale, drawinfo.playerEffect);
				item.shader = drawinfo.cWings;
				drawinfo.DrawDataCache.Add(item);
				for (float num9 = 0.25f; num9 < 1f; num9 += 0.25f)
				{
					item = new DrawData(value, Vector2.Lerp(pos, pos2, num9) - Main.screenPosition, null, color, num6, origin, scale, drawinfo.playerEffect);
					item.shader = drawinfo.cWings;
					drawinfo.DrawDataCache.Add(item);
				}
			}
		}
	}

	public static void DrawMeowcartTrail(ref PlayerDrawSet drawinfo)
	{
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_0158: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0174: Unknown result type (might be due to invalid IL or missing references)
		//IL_019e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01be: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01da: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0208: Unknown result type (might be due to invalid IL or missing references)
		//IL_020a: Unknown result type (might be due to invalid IL or missing references)
		//IL_023b: Unknown result type (might be due to invalid IL or missing references)
		//IL_023f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0245: Unknown result type (might be due to invalid IL or missing references)
		//IL_024a: Unknown result type (might be due to invalid IL or missing references)
		//IL_024f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0251: Unknown result type (might be due to invalid IL or missing references)
		//IL_0256: Unknown result type (might be due to invalid IL or missing references)
		//IL_0265: Unknown result type (might be due to invalid IL or missing references)
		//IL_0269: Unknown result type (might be due to invalid IL or missing references)
		//IL_026b: Unknown result type (might be due to invalid IL or missing references)
		//IL_026e: Unknown result type (might be due to invalid IL or missing references)
		if (drawinfo.drawPlayer.mount.Type == 33 && !(drawinfo.shadow > 0f))
		{
			int num = Math.Min(drawinfo.drawPlayer.availableAdvancedShadowsCount - 1, 20);
			float num2 = 0f;
			for (int num3 = num; num3 > 0; num3--)
			{
				EntityShadowInfo advancedShadow = drawinfo.drawPlayer.GetAdvancedShadow(num3);
				float num8 = num2;
				Vector2 position = drawinfo.drawPlayer.GetAdvancedShadow(num3 - 1).Position;
				num2 = num8 + Vector2.Distance(advancedShadow.Position, position);
			}
			float num4 = MathHelper.Clamp(num2 / 160f, 0f, 1f);
			Main.instance.LoadProjectile(250);
			Texture2D value = TextureAssets.Projectile[250].Value;
			float x = 1.5f;
			Vector2 origin = default(Vector2);
			((Vector2)(ref origin))._002Ector((float)(value.Width / 2), 0f);
			Vector2 val = new Vector2((float)drawinfo.drawPlayer.width, (float)drawinfo.drawPlayer.height) / 2f;
			Vector2 vector2 = default(Vector2);
			((Vector2)(ref vector2))._002Ector((float)(-drawinfo.drawPlayer.direction * 10), 15f);
			Color white = Color.White;
			((Color)(ref white)).A = 127;
			Vector2 vector3 = val + vector2;
			vector3 = Vector2.Zero;
			Vector2 vector4 = drawinfo.drawPlayer.RotatedRelativePoint(drawinfo.drawPlayer.Center + vector3 + vector2) - drawinfo.drawPlayer.position;
			Vector2 scale = default(Vector2);
			for (int num5 = num; num5 > 0; num5--)
			{
				EntityShadowInfo advancedShadow2 = drawinfo.drawPlayer.GetAdvancedShadow(num5);
				EntityShadowInfo advancedShadow3 = drawinfo.drawPlayer.GetAdvancedShadow(num5 - 1);
				Vector2 pos = advancedShadow2.Position + vector3;
				Vector2 pos2 = advancedShadow3.Position + vector3;
				pos += vector4;
				pos2 += vector4;
				pos = drawinfo.drawPlayer.RotatedRelativePoint(pos, reverseRotation: true, addGfxOffY: false);
				pos2 = drawinfo.drawPlayer.RotatedRelativePoint(pos2, reverseRotation: true, addGfxOffY: false);
				float rotation = (pos2 - pos).ToRotation() - (float)Math.PI / 2f;
				float num6 = Vector2.Distance(pos, pos2);
				((Vector2)(ref scale))._002Ector(x, num6 / (float)value.Height);
				float num7 = 1f - (float)num5 / (float)num;
				num7 *= num7;
				Color color = white * num7 * num4;
				DrawData item = new DrawData(value, pos - Main.screenPosition, null, color, rotation, origin, scale, drawinfo.playerEffect);
				drawinfo.DrawDataCache.Add(item);
			}
		}
	}

	public static void DrawPlayer_23_MountFront(ref PlayerDrawSet drawinfo)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		if (drawinfo.drawPlayer.mount.Active)
		{
			drawinfo.drawPlayer.mount.Draw(drawinfo.DrawDataCache, 2, drawinfo.drawPlayer, drawinfo.Position, drawinfo.colorMount, drawinfo.playerEffect, drawinfo.shadow);
			drawinfo.drawPlayer.mount.Draw(drawinfo.DrawDataCache, 3, drawinfo.drawPlayer, drawinfo.Position, drawinfo.colorMount, drawinfo.playerEffect, drawinfo.shadow);
		}
	}

	public static void DrawPlayer_24_Pulley(ref PlayerDrawSet drawinfo)
	{
		//IL_0213: Unknown result type (might be due to invalid IL or missing references)
		//IL_0247: Unknown result type (might be due to invalid IL or missing references)
		//IL_0252: Unknown result type (might be due to invalid IL or missing references)
		//IL_0273: Unknown result type (might be due to invalid IL or missing references)
		//IL_027e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		if (drawinfo.drawPlayer.pulley && drawinfo.drawPlayer.itemAnimation == 0)
		{
			if (drawinfo.drawPlayer.pulleyDir == 2)
			{
				int num = -25;
				int num2 = 0;
				float rotation = 0f;
				DrawData item = new DrawData(TextureAssets.Pulley.Value, new Vector2((float)((int)(drawinfo.Position.X - Main.screenPosition.X + (float)(drawinfo.drawPlayer.width / 2) - (float)(9 * drawinfo.drawPlayer.direction)) + num2 * drawinfo.drawPlayer.direction), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)(drawinfo.drawPlayer.height / 2) + 2f * drawinfo.drawPlayer.gravDir + (float)num * drawinfo.drawPlayer.gravDir)), (Rectangle?)new Rectangle(0, TextureAssets.Pulley.Height() / 2 * drawinfo.drawPlayer.pulleyFrame, TextureAssets.Pulley.Width(), TextureAssets.Pulley.Height() / 2), drawinfo.colorArmorHead, rotation, new Vector2((float)(TextureAssets.Pulley.Width() / 2), (float)(TextureAssets.Pulley.Height() / 4)), 1f, drawinfo.playerEffect, 0f);
				drawinfo.DrawDataCache.Add(item);
			}
			else
			{
				int num3 = -26;
				int num4 = 10;
				float rotation2 = 0.35f * (float)(-drawinfo.drawPlayer.direction);
				DrawData item2 = new DrawData(TextureAssets.Pulley.Value, new Vector2((float)((int)(drawinfo.Position.X - Main.screenPosition.X + (float)(drawinfo.drawPlayer.width / 2) - (float)(9 * drawinfo.drawPlayer.direction)) + num4 * drawinfo.drawPlayer.direction), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)(drawinfo.drawPlayer.height / 2) + 2f * drawinfo.drawPlayer.gravDir + (float)num3 * drawinfo.drawPlayer.gravDir)), (Rectangle?)new Rectangle(0, TextureAssets.Pulley.Height() / 2 * drawinfo.drawPlayer.pulleyFrame, TextureAssets.Pulley.Width(), TextureAssets.Pulley.Height() / 2), drawinfo.colorArmorHead, rotation2, new Vector2((float)(TextureAssets.Pulley.Width() / 2), (float)(TextureAssets.Pulley.Height() / 4)), 1f, drawinfo.playerEffect, 0f);
				drawinfo.DrawDataCache.Add(item2);
			}
		}
	}

	public static void DrawPlayer_25_Shield(ref PlayerDrawSet drawinfo)
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_02df: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_034e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0353: Unknown result type (might be due to invalid IL or missing references)
		//IL_035e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0363: Unknown result type (might be due to invalid IL or missing references)
		//IL_0368: Unknown result type (might be due to invalid IL or missing references)
		//IL_0384: Unknown result type (might be due to invalid IL or missing references)
		//IL_0389: Unknown result type (might be due to invalid IL or missing references)
		//IL_038e: Unknown result type (might be due to invalid IL or missing references)
		//IL_038f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0394: Unknown result type (might be due to invalid IL or missing references)
		//IL_039b: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0426: Unknown result type (might be due to invalid IL or missing references)
		//IL_0436: Unknown result type (might be due to invalid IL or missing references)
		//IL_043b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0455: Unknown result type (might be due to invalid IL or missing references)
		//IL_046c: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04de: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0504: Unknown result type (might be due to invalid IL or missing references)
		//IL_0505: Unknown result type (might be due to invalid IL or missing references)
		//IL_050a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0510: Unknown result type (might be due to invalid IL or missing references)
		//IL_051d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0524: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0209: Unknown result type (might be due to invalid IL or missing references)
		//IL_020e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0213: Unknown result type (might be due to invalid IL or missing references)
		//IL_022f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0234: Unknown result type (might be due to invalid IL or missing references)
		//IL_0239: Unknown result type (might be due to invalid IL or missing references)
		//IL_023a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0246: Unknown result type (might be due to invalid IL or missing references)
		//IL_025c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0262: Unknown result type (might be due to invalid IL or missing references)
		//IL_0264: Unknown result type (might be due to invalid IL or missing references)
		//IL_0269: Unknown result type (might be due to invalid IL or missing references)
		//IL_026e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0274: Unknown result type (might be due to invalid IL or missing references)
		//IL_0281: Unknown result type (might be due to invalid IL or missing references)
		//IL_0288: Unknown result type (might be due to invalid IL or missing references)
		//IL_057f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0596: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0603: Unknown result type (might be due to invalid IL or missing references)
		//IL_0608: Unknown result type (might be due to invalid IL or missing references)
		//IL_0624: Unknown result type (might be due to invalid IL or missing references)
		//IL_0629: Unknown result type (might be due to invalid IL or missing references)
		//IL_062e: Unknown result type (might be due to invalid IL or missing references)
		//IL_062f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0634: Unknown result type (might be due to invalid IL or missing references)
		//IL_0637: Unknown result type (might be due to invalid IL or missing references)
		//IL_063c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0646: Unknown result type (might be due to invalid IL or missing references)
		//IL_0651: Unknown result type (might be due to invalid IL or missing references)
		//IL_065b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0660: Unknown result type (might be due to invalid IL or missing references)
		//IL_0665: Unknown result type (might be due to invalid IL or missing references)
		//IL_066a: Unknown result type (might be due to invalid IL or missing references)
		//IL_066f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0671: Unknown result type (might be due to invalid IL or missing references)
		//IL_0673: Unknown result type (might be due to invalid IL or missing references)
		//IL_0675: Unknown result type (might be due to invalid IL or missing references)
		//IL_067a: Unknown result type (might be due to invalid IL or missing references)
		//IL_067c: Unknown result type (might be due to invalid IL or missing references)
		//IL_067e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0682: Unknown result type (might be due to invalid IL or missing references)
		//IL_0687: Unknown result type (might be due to invalid IL or missing references)
		//IL_068c: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_06bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_06df: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f0: Unknown result type (might be due to invalid IL or missing references)
		if (drawinfo.drawPlayer.shield <= 0 || drawinfo.drawPlayer.shield >= TextureAssets.AccShield.Length)
		{
			return;
		}
		Vector2 zero = Vector2.Zero;
		if (drawinfo.drawPlayer.shieldRaised)
		{
			zero.Y -= 4f * drawinfo.drawPlayer.gravDir;
		}
		Rectangle bodyFrame = drawinfo.drawPlayer.bodyFrame;
		Vector2 zero2 = Vector2.Zero;
		Vector2 bodyVect = drawinfo.bodyVect;
		if (bodyFrame.Width != TextureAssets.AccShield[drawinfo.drawPlayer.shield].Value.Width)
		{
			bodyFrame.Width = TextureAssets.AccShield[drawinfo.drawPlayer.shield].Value.Width;
			bodyVect.X += bodyFrame.Width - TextureAssets.AccShield[drawinfo.drawPlayer.shield].Value.Width;
			if (((Enum)drawinfo.playerEffect).HasFlag((Enum)(object)(SpriteEffects)1))
			{
				bodyVect.X = (float)bodyFrame.Width - bodyVect.X;
			}
		}
		DrawData item;
		if (drawinfo.drawPlayer.shieldRaised)
		{
			float num = (float)Math.Sin(Main.GlobalTimeWrappedHourly * ((float)Math.PI * 2f));
			float x = 2.5f + 1.5f * num;
			Color colorArmorBody = drawinfo.colorArmorBody;
			((Color)(ref colorArmorBody)).A = 0;
			colorArmorBody *= 0.45f - num * 0.15f;
			for (float num2 = 0f; num2 < 4f; num2 += 1f)
			{
				item = new DrawData(TextureAssets.AccShield[drawinfo.drawPlayer.shield].Value, zero2 + new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2)) + zero + Utils.RotatedBy(new Vector2(x, 0f), num2 / 4f * ((float)Math.PI * 2f)), bodyFrame, colorArmorBody, drawinfo.drawPlayer.bodyRotation, bodyVect, 1f, drawinfo.playerEffect);
				item.shader = drawinfo.cShield;
				drawinfo.DrawDataCache.Add(item);
			}
		}
		item = new DrawData(TextureAssets.AccShield[drawinfo.drawPlayer.shield].Value, zero2 + new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2)) + zero, bodyFrame, drawinfo.colorArmorBody, drawinfo.drawPlayer.bodyRotation, bodyVect, 1f, drawinfo.playerEffect);
		item.shader = drawinfo.cShield;
		drawinfo.DrawDataCache.Add(item);
		if (drawinfo.drawPlayer.shieldRaised)
		{
			Color colorArmorBody2 = drawinfo.colorArmorBody;
			float num3 = (float)Math.Sin(Main.GlobalTimeWrappedHourly * (float)Math.PI);
			((Color)(ref colorArmorBody2)).A = (byte)((float)(int)((Color)(ref colorArmorBody2)).A * (0.5f + 0.5f * num3));
			colorArmorBody2 *= 0.5f + 0.5f * num3;
			item = new DrawData(TextureAssets.AccShield[drawinfo.drawPlayer.shield].Value, zero2 + new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2)) + zero, bodyFrame, colorArmorBody2, drawinfo.drawPlayer.bodyRotation, bodyVect, 1f, drawinfo.playerEffect);
			item.shader = drawinfo.cShield;
		}
		if (drawinfo.drawPlayer.shieldRaised && drawinfo.drawPlayer.shieldParryTimeLeft > 0)
		{
			float num4 = (float)drawinfo.drawPlayer.shieldParryTimeLeft / 20f;
			float num5 = 1.5f * num4;
			Vector2 vector = zero2 + new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2)) + zero;
			Color colorArmorBody3 = drawinfo.colorArmorBody;
			float num6 = 1f;
			Vector2 vector2 = drawinfo.Position + drawinfo.drawPlayer.Size / 2f - Main.screenPosition;
			Vector2 vector3 = vector - vector2;
			vector += vector3 * num5;
			num6 += num5;
			((Color)(ref colorArmorBody3)).A = (byte)((float)(int)((Color)(ref colorArmorBody3)).A * (1f - num4));
			colorArmorBody3 *= 1f - num4;
			item = new DrawData(TextureAssets.AccShield[drawinfo.drawPlayer.shield].Value, vector, bodyFrame, colorArmorBody3, drawinfo.drawPlayer.bodyRotation, bodyVect, num6, drawinfo.playerEffect);
			item.shader = drawinfo.cShield;
			drawinfo.DrawDataCache.Add(item);
		}
		if (drawinfo.drawPlayer.mount.Cart)
		{
			drawinfo.DrawDataCache.Reverse(drawinfo.DrawDataCache.Count - 2, 2);
		}
	}

	public static void DrawPlayer_26_SolarShield(ref PlayerDrawSet drawinfo)
	{
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		if (drawinfo.drawPlayer.solarShields > 0 && drawinfo.shadow == 0f && !drawinfo.drawPlayer.dead)
		{
			Texture2D value = TextureAssets.Extra[61 + drawinfo.drawPlayer.solarShields - 1].Value;
			Color color = default(Color);
			((Color)(ref color))._002Ector(255, 255, 255, 127);
			float num = (drawinfo.drawPlayer.solarShieldPos[0] * new Vector2(1f, 0.5f)).ToRotation();
			if (drawinfo.drawPlayer.direction == -1)
			{
				num += (float)Math.PI;
			}
			num += (float)Math.PI / 50f * (float)drawinfo.drawPlayer.direction;
			DrawData item = new DrawData(value, new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)(drawinfo.drawPlayer.height / 2))) + drawinfo.drawPlayer.solarShieldPos[0], null, color, num, value.Size() / 2f, 1f, drawinfo.playerEffect);
			item.shader = drawinfo.cBody;
			drawinfo.DrawDataCache.Add(item);
		}
	}

	public static void DrawPlayer_27_HeldItem(ref PlayerDrawSet drawinfo)
	{
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0253: Unknown result type (might be due to invalid IL or missing references)
		//IL_0258: Unknown result type (might be due to invalid IL or missing references)
		//IL_0271: Unknown result type (might be due to invalid IL or missing references)
		//IL_027e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0283: Unknown result type (might be due to invalid IL or missing references)
		//IL_0288: Unknown result type (might be due to invalid IL or missing references)
		//IL_028d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0397: Unknown result type (might be due to invalid IL or missing references)
		//IL_039c: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0500: Unknown result type (might be due to invalid IL or missing references)
		//IL_0505: Unknown result type (might be due to invalid IL or missing references)
		//IL_050a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0446: Unknown result type (might be due to invalid IL or missing references)
		//IL_0451: Unknown result type (might be due to invalid IL or missing references)
		//IL_0456: Unknown result type (might be due to invalid IL or missing references)
		//IL_045b: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0468: Unknown result type (might be due to invalid IL or missing references)
		//IL_046d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0498: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0420: Unknown result type (might be due to invalid IL or missing references)
		//IL_042b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0430: Unknown result type (might be due to invalid IL or missing references)
		//IL_0435: Unknown result type (might be due to invalid IL or missing references)
		//IL_0595: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_056b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0570: Unknown result type (might be due to invalid IL or missing references)
		//IL_0575: Unknown result type (might be due to invalid IL or missing references)
		//IL_0577: Unknown result type (might be due to invalid IL or missing references)
		//IL_0579: Unknown result type (might be due to invalid IL or missing references)
		//IL_0583: Unknown result type (might be due to invalid IL or missing references)
		//IL_0588: Unknown result type (might be due to invalid IL or missing references)
		//IL_067b: Unknown result type (might be due to invalid IL or missing references)
		//IL_067d: Unknown result type (might be due to invalid IL or missing references)
		//IL_067f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0684: Unknown result type (might be due to invalid IL or missing references)
		//IL_0666: Unknown result type (might be due to invalid IL or missing references)
		//IL_066e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0642: Unknown result type (might be due to invalid IL or missing references)
		//IL_0644: Unknown result type (might be due to invalid IL or missing references)
		//IL_0649: Unknown result type (might be due to invalid IL or missing references)
		//IL_064b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0650: Unknown result type (might be due to invalid IL or missing references)
		//IL_0635: Unknown result type (might be due to invalid IL or missing references)
		//IL_0753: Unknown result type (might be due to invalid IL or missing references)
		//IL_074a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0733: Unknown result type (might be due to invalid IL or missing references)
		//IL_072a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0805: Unknown result type (might be due to invalid IL or missing references)
		//IL_080a: Unknown result type (might be due to invalid IL or missing references)
		//IL_080e: Unknown result type (might be due to invalid IL or missing references)
		//IL_082e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0830: Unknown result type (might be due to invalid IL or missing references)
		//IL_083a: Unknown result type (might be due to invalid IL or missing references)
		//IL_083f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0846: Unknown result type (might be due to invalid IL or missing references)
		//IL_084b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0850: Unknown result type (might be due to invalid IL or missing references)
		//IL_0855: Unknown result type (might be due to invalid IL or missing references)
		//IL_0857: Unknown result type (might be due to invalid IL or missing references)
		//IL_085c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0861: Unknown result type (might be due to invalid IL or missing references)
		//IL_086a: Unknown result type (might be due to invalid IL or missing references)
		//IL_086f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0888: Unknown result type (might be due to invalid IL or missing references)
		//IL_088d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0892: Unknown result type (might be due to invalid IL or missing references)
		//IL_08a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_08a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_08de: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_08f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0910: Unknown result type (might be due to invalid IL or missing references)
		//IL_091a: Unknown result type (might be due to invalid IL or missing references)
		//IL_092a: Unknown result type (might be due to invalid IL or missing references)
		//IL_092e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a45: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a4a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a4e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a6e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a70: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a7a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a7f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a99: Unknown result type (might be due to invalid IL or missing references)
		//IL_0abb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ad4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ade: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ae3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0971: Unknown result type (might be due to invalid IL or missing references)
		//IL_0976: Unknown result type (might be due to invalid IL or missing references)
		//IL_097a: Unknown result type (might be due to invalid IL or missing references)
		//IL_099a: Unknown result type (might be due to invalid IL or missing references)
		//IL_09bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_09c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_09c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_09c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_09d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_09d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_09e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_09e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_09f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_09f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_09fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a04: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a09: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a19: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a1d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b0b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b10: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b15: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b1a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b1c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b21: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b2e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b35: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b3a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b3f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b46: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b53: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b57: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b8e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b93: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b98: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b9d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b9f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ba4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ba9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bb2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bb7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bd0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bd5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bda: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bea: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bee: Unknown result type (might be due to invalid IL or missing references)
		//IL_1511: Unknown result type (might be due to invalid IL or missing references)
		//IL_1513: Unknown result type (might be due to invalid IL or missing references)
		//IL_151c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1521: Unknown result type (might be due to invalid IL or missing references)
		//IL_1528: Unknown result type (might be due to invalid IL or missing references)
		//IL_152c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1549: Unknown result type (might be due to invalid IL or missing references)
		//IL_1550: Unknown result type (might be due to invalid IL or missing references)
		//IL_1556: Unknown result type (might be due to invalid IL or missing references)
		//IL_1424: Unknown result type (might be due to invalid IL or missing references)
		//IL_1426: Unknown result type (might be due to invalid IL or missing references)
		//IL_142f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1434: Unknown result type (might be due to invalid IL or missing references)
		//IL_143b: Unknown result type (might be due to invalid IL or missing references)
		//IL_143f: Unknown result type (might be due to invalid IL or missing references)
		//IL_145c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1463: Unknown result type (might be due to invalid IL or missing references)
		//IL_1469: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c58: Unknown result type (might be due to invalid IL or missing references)
		//IL_1562: Unknown result type (might be due to invalid IL or missing references)
		//IL_1564: Unknown result type (might be due to invalid IL or missing references)
		//IL_156d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1572: Unknown result type (might be due to invalid IL or missing references)
		//IL_1579: Unknown result type (might be due to invalid IL or missing references)
		//IL_157d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1475: Unknown result type (might be due to invalid IL or missing references)
		//IL_1477: Unknown result type (might be due to invalid IL or missing references)
		//IL_1480: Unknown result type (might be due to invalid IL or missing references)
		//IL_1485: Unknown result type (might be due to invalid IL or missing references)
		//IL_148c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1490: Unknown result type (might be due to invalid IL or missing references)
		//IL_106e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1078: Unknown result type (might be due to invalid IL or missing references)
		//IL_1093: Unknown result type (might be due to invalid IL or missing references)
		//IL_1098: Unknown result type (might be due to invalid IL or missing references)
		//IL_109a: Unknown result type (might be due to invalid IL or missing references)
		//IL_10a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_10b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f5c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f6a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f7f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f8d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fa4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0faf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fb4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fb9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fbb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fcb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fd1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fd3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fd8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ff3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ffb: Unknown result type (might be due to invalid IL or missing references)
		//IL_101b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1023: Unknown result type (might be due to invalid IL or missing references)
		//IL_102d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1032: Unknown result type (might be due to invalid IL or missing references)
		//IL_103b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1040: Unknown result type (might be due to invalid IL or missing references)
		//IL_1047: Unknown result type (might be due to invalid IL or missing references)
		//IL_104b: Unknown result type (might be due to invalid IL or missing references)
		//IL_15b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_15b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_15be: Unknown result type (might be due to invalid IL or missing references)
		//IL_15c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_15c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_14c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_14ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_14e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_14ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_14f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_110a: Unknown result type (might be due to invalid IL or missing references)
		//IL_112a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1134: Unknown result type (might be due to invalid IL or missing references)
		//IL_1139: Unknown result type (might be due to invalid IL or missing references)
		//IL_1142: Unknown result type (might be due to invalid IL or missing references)
		//IL_1147: Unknown result type (might be due to invalid IL or missing references)
		//IL_1157: Unknown result type (might be due to invalid IL or missing references)
		//IL_115b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1178: Unknown result type (might be due to invalid IL or missing references)
		//IL_117f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1185: Unknown result type (might be due to invalid IL or missing references)
		//IL_10d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_10e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cb5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cd8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cdd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ce7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cec: Unknown result type (might be due to invalid IL or missing references)
		//IL_11aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_11ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_11d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_11d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_11e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_11e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_11f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_11fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_124c: Unknown result type (might be due to invalid IL or missing references)
		//IL_126c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1276: Unknown result type (might be due to invalid IL or missing references)
		//IL_127b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1297: Unknown result type (might be due to invalid IL or missing references)
		//IL_12a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_12ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d36: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d41: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d46: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d59: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d5f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d61: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d66: Unknown result type (might be due to invalid IL or missing references)
		//IL_12ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_130c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1325: Unknown result type (might be due to invalid IL or missing references)
		//IL_132f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1334: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e2d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e55: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e5a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e63: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e68: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e6f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e73: Unknown result type (might be due to invalid IL or missing references)
		//IL_0de5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ded: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dfc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dcc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dd1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0da1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0db5: Unknown result type (might be due to invalid IL or missing references)
		//IL_136a: Unknown result type (might be due to invalid IL or missing references)
		//IL_138a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1394: Unknown result type (might be due to invalid IL or missing references)
		//IL_13ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_13b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_13b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_13c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_13d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_13d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ec2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eea: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eef: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f07: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f0e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f12: Unknown result type (might be due to invalid IL or missing references)
		//IL_1742: Unknown result type (might be due to invalid IL or missing references)
		//IL_1751: Unknown result type (might be due to invalid IL or missing references)
		//IL_175d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1762: Unknown result type (might be due to invalid IL or missing references)
		//IL_1769: Unknown result type (might be due to invalid IL or missing references)
		//IL_176d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1771: Unknown result type (might be due to invalid IL or missing references)
		if (drawinfo.drawPlayer.JustDroppedAnItem)
		{
			return;
		}
		if (drawinfo.drawPlayer.heldProj >= 0 && drawinfo.shadow == 0f && !drawinfo.heldProjOverHand)
		{
			drawinfo.projectileDrawPosition = drawinfo.DrawDataCache.Count;
		}
		Item heldItem = drawinfo.heldItem;
		int num = heldItem.type;
		if (drawinfo.drawPlayer.UsingBiomeTorches)
		{
			switch (num)
			{
			case 8:
				num = drawinfo.drawPlayer.BiomeTorchHoldStyle(num);
				break;
			case 966:
				num = drawinfo.drawPlayer.BiomeCampfireHoldStyle(num);
				break;
			}
		}
		float adjustedItemScale = drawinfo.drawPlayer.GetAdjustedItemScale(heldItem);
		Main.instance.LoadItem(num);
		Texture2D value = TextureAssets.Item[num].Value;
		Vector2 position = default(Vector2);
		((Vector2)(ref position))._002Ector((float)(int)(drawinfo.ItemLocation.X - Main.screenPosition.X), (float)(int)(drawinfo.ItemLocation.Y - Main.screenPosition.Y));
		Rectangle itemDrawFrame = drawinfo.drawPlayer.GetItemDrawFrame(num);
		drawinfo.itemColor = Lighting.GetColor((int)((double)drawinfo.Position.X + (double)drawinfo.drawPlayer.width * 0.5) / 16, (int)(((double)drawinfo.Position.Y + (double)drawinfo.drawPlayer.height * 0.5) / 16.0));
		if (num == 678)
		{
			drawinfo.itemColor = Color.White;
		}
		if (drawinfo.drawPlayer.shroomiteStealth && heldItem.ranged)
		{
			float num12 = drawinfo.drawPlayer.stealth;
			if ((double)num12 < 0.03)
			{
				num12 = 0.03f;
			}
			float num13 = (1f + num12 * 10f) / 11f;
			drawinfo.itemColor = new Color((int)(byte)((float)(int)((Color)(ref drawinfo.itemColor)).R * num12), (int)(byte)((float)(int)((Color)(ref drawinfo.itemColor)).G * num12), (int)(byte)((float)(int)((Color)(ref drawinfo.itemColor)).B * num13), (int)(byte)((float)(int)((Color)(ref drawinfo.itemColor)).A * num12));
		}
		if (drawinfo.drawPlayer.setVortex && heldItem.ranged)
		{
			float num14 = drawinfo.drawPlayer.stealth;
			if ((double)num14 < 0.03)
			{
				num14 = 0.03f;
			}
			_ = (1f + num14 * 10f) / 11f;
			drawinfo.itemColor = drawinfo.itemColor.MultiplyRGBA(new Color(Vector4.Lerp(Vector4.One, new Vector4(0f, 0.12f, 0.16f, 0f), 1f - num14)));
		}
		bool flag = drawinfo.drawPlayer.itemAnimation > 0 && heldItem.useStyle != 0;
		bool flag2 = heldItem.holdStyle != 0 && !drawinfo.drawPlayer.pulley;
		if (!drawinfo.drawPlayer.CanVisuallyHoldItem(heldItem))
		{
			flag2 = false;
		}
		if (drawinfo.shadow != 0f || drawinfo.drawPlayer.frozen || !(flag || flag2) || num <= 0 || drawinfo.drawPlayer.dead || heldItem.noUseGraphic || (drawinfo.drawPlayer.wet && heldItem.noWet && !ItemID.Sets.WaterTorches[num]) || (drawinfo.drawPlayer.happyFunTorchTime && drawinfo.drawPlayer.inventory[drawinfo.drawPlayer.selectedItem].createTile == 4 && drawinfo.drawPlayer.itemAnimation == 0))
		{
			return;
		}
		_ = drawinfo.drawPlayer.name;
		Color color = default(Color);
		((Color)(ref color))._002Ector(250, 250, 250, heldItem.alpha);
		Vector2 vector = Vector2.Zero;
		switch (num)
		{
		case 104:
		case 5094:
		case 5095:
			vector = new Vector2(4f, -4f) * drawinfo.drawPlayer.Directions;
			break;
		case 426:
		case 797:
		case 1506:
		case 5096:
		case 5097:
			vector = new Vector2(6f, -6f) * drawinfo.drawPlayer.Directions;
			break;
		case 46:
		{
			Vector3 val = ((Color)(ref drawinfo.itemColor)).ToVector3();
			float amount = Utils.Remap(((Vector3)(ref val)).Length() / 1.731f, 0.3f, 0.5f, 1f, 0f);
			color = Color.Lerp(Color.Transparent, new Color(255, 255, 255, 127) * 0.7f, amount);
			break;
		}
		case 204:
			vector = new Vector2(4f, -6f) * drawinfo.drawPlayer.Directions;
			break;
		case 3349:
			vector = new Vector2(2f, -2f) * drawinfo.drawPlayer.Directions;
			break;
		}
		if (num == 3823)
		{
			((Vector2)(ref vector))._002Ector((float)(7 * drawinfo.drawPlayer.direction), -7f * drawinfo.drawPlayer.gravDir);
		}
		if (num == 3827)
		{
			((Vector2)(ref vector))._002Ector((float)(13 * drawinfo.drawPlayer.direction), -13f * drawinfo.drawPlayer.gravDir);
			color = heldItem.GetAlpha(drawinfo.itemColor);
			color = Color.Lerp(color, Color.White, 0.6f);
			((Color)(ref color)).A = 66;
		}
		Vector2 origin = default(Vector2);
		((Vector2)(ref origin))._002Ector((float)itemDrawFrame.Width * 0.5f - (float)itemDrawFrame.Width * 0.5f * (float)drawinfo.drawPlayer.direction, (float)itemDrawFrame.Height);
		if (heldItem.useStyle == 9 && drawinfo.drawPlayer.itemAnimation > 0)
		{
			Vector2 vector3 = default(Vector2);
			((Vector2)(ref vector3))._002Ector(0.5f, 0.4f);
			if (heldItem.type == 5009 || heldItem.type == 5042)
			{
				((Vector2)(ref vector3))._002Ector(0.26f, 0.5f);
				if (drawinfo.drawPlayer.direction == -1)
				{
					vector3.X = 1f - vector3.X;
				}
			}
			origin = itemDrawFrame.Size() * vector3;
		}
		if (drawinfo.drawPlayer.gravDir == -1f)
		{
			origin.Y = (float)itemDrawFrame.Height - origin.Y;
		}
		origin += vector;
		float num15 = drawinfo.drawPlayer.itemRotation;
		if (heldItem.useStyle == 8)
		{
			ref float x = ref position.X;
			float num16 = x;
			_ = drawinfo.drawPlayer.direction;
			x = num16 - 0f;
			num15 -= (float)Math.PI / 2f * (float)drawinfo.drawPlayer.direction;
			origin.Y = 2f;
			origin.X += 2 * drawinfo.drawPlayer.direction;
		}
		if (num == 425 || num == 507)
		{
			if (drawinfo.drawPlayer.gravDir == 1f)
			{
				if (drawinfo.drawPlayer.direction == 1)
				{
					drawinfo.itemEffect = (SpriteEffects)2;
				}
				else
				{
					drawinfo.itemEffect = (SpriteEffects)3;
				}
			}
			else if (drawinfo.drawPlayer.direction == 1)
			{
				drawinfo.itemEffect = (SpriteEffects)0;
			}
			else
			{
				drawinfo.itemEffect = (SpriteEffects)1;
			}
		}
		if ((num == 946 || num == 4707) && num15 != 0f)
		{
			position.Y -= 22f * drawinfo.drawPlayer.gravDir;
			num15 = -1.57f * (float)(-drawinfo.drawPlayer.direction) * drawinfo.drawPlayer.gravDir;
		}
		ItemSlot.GetItemLight(ref drawinfo.itemColor, heldItem);
		DrawData item;
		switch (num)
		{
		case 3476:
		{
			Texture2D value3 = TextureAssets.Extra[64].Value;
			Rectangle rectangle2 = value3.Frame(1, 9, 0, drawinfo.drawPlayer.miscCounter % 54 / 6);
			Vector2 vector5 = default(Vector2);
			((Vector2)(ref vector5))._002Ector((float)(rectangle2.Width / 2 * drawinfo.drawPlayer.direction), 0f);
			Vector2 origin3 = rectangle2.Size() / 2f;
			item = new DrawData(value3, (drawinfo.ItemLocation - Main.screenPosition + vector5).Floor(), rectangle2, heldItem.GetAlpha(drawinfo.itemColor).MultiplyRGBA(new Color(new Vector4(0.5f, 0.5f, 0.5f, 0.8f))), drawinfo.drawPlayer.itemRotation, origin3, adjustedItemScale, drawinfo.itemEffect);
			drawinfo.DrawDataCache.Add(item);
			value3 = TextureAssets.GlowMask[195].Value;
			item = new DrawData(value3, (drawinfo.ItemLocation - Main.screenPosition + vector5).Floor(), rectangle2, new Color(250, 250, 250, heldItem.alpha) * 0.5f, drawinfo.drawPlayer.itemRotation, origin3, adjustedItemScale, drawinfo.itemEffect);
			drawinfo.DrawDataCache.Add(item);
			return;
		}
		case 4049:
		{
			Texture2D value2 = TextureAssets.Extra[92].Value;
			Rectangle rectangle = value2.Frame(1, 4, 0, drawinfo.drawPlayer.miscCounter % 20 / 5);
			Vector2 vector4 = default(Vector2);
			((Vector2)(ref vector4))._002Ector((float)(rectangle.Width / 2 * drawinfo.drawPlayer.direction), 0f);
			vector4 += new Vector2((float)(-10 * drawinfo.drawPlayer.direction), 8f * drawinfo.drawPlayer.gravDir);
			Vector2 origin2 = rectangle.Size() / 2f;
			item = new DrawData(value2, (drawinfo.ItemLocation - Main.screenPosition + vector4).Floor(), rectangle, heldItem.GetAlpha(drawinfo.itemColor), drawinfo.drawPlayer.itemRotation, origin2, adjustedItemScale, drawinfo.itemEffect);
			drawinfo.DrawDataCache.Add(item);
			return;
		}
		case 3779:
		{
			Texture2D texture2D = value;
			Rectangle rectangle3 = texture2D.Frame();
			Vector2 vector6 = default(Vector2);
			((Vector2)(ref vector6))._002Ector((float)(rectangle3.Width / 2 * drawinfo.drawPlayer.direction), 0f);
			Vector2 origin4 = rectangle3.Size() / 2f;
			float num17 = ((float)drawinfo.drawPlayer.miscCounter / 75f * ((float)Math.PI * 2f)).ToRotationVector2().X * 1f + 0f;
			Color color2 = new Color(120, 40, 222, 0) * (num17 / 2f * 0.3f + 0.85f) * 0.5f;
			num17 = 2f;
			for (float num18 = 0f; num18 < 4f; num18 += 1f)
			{
				item = new DrawData(TextureAssets.GlowMask[218].Value, (drawinfo.ItemLocation - Main.screenPosition + vector6).Floor() + (num18 * ((float)Math.PI / 2f)).ToRotationVector2() * num17, rectangle3, color2, drawinfo.drawPlayer.itemRotation, origin4, adjustedItemScale, drawinfo.itemEffect);
				drawinfo.DrawDataCache.Add(item);
			}
			item = new DrawData(texture2D, (drawinfo.ItemLocation - Main.screenPosition + vector6).Floor(), rectangle3, heldItem.GetAlpha(drawinfo.itemColor).MultiplyRGBA(new Color(new Vector4(0.5f, 0.5f, 0.5f, 0.8f))), drawinfo.drawPlayer.itemRotation, origin4, adjustedItemScale, drawinfo.itemEffect);
			drawinfo.DrawDataCache.Add(item);
			return;
		}
		}
		if (heldItem.useStyle == 5)
		{
			if (Item.staff[num])
			{
				float num19 = drawinfo.drawPlayer.itemRotation + 0.785f * (float)drawinfo.drawPlayer.direction;
				float num2 = 0f;
				float num3 = 0f;
				Vector2 origin5 = default(Vector2);
				((Vector2)(ref origin5))._002Ector(0f, (float)itemDrawFrame.Height);
				if (num == 3210)
				{
					num2 = 8 * -drawinfo.drawPlayer.direction;
					num3 = 2 * (int)drawinfo.drawPlayer.gravDir;
				}
				if (num == 3870)
				{
					Vector2 val2 = (drawinfo.drawPlayer.itemRotation + (float)Math.PI / 4f * (float)drawinfo.drawPlayer.direction).ToRotationVector2() * new Vector2((float)(-drawinfo.drawPlayer.direction) * 1.5f, drawinfo.drawPlayer.gravDir) * 3f;
					num2 = (int)val2.X;
					num3 = (int)val2.Y;
				}
				if (num == 3787)
				{
					num3 = (int)((float)(8 * (int)drawinfo.drawPlayer.gravDir) * (float)Math.Cos(num19));
				}
				if (num == 3209)
				{
					Vector2 val3 = (new Vector2(-8f, 0f) * drawinfo.drawPlayer.Directions).RotatedBy(drawinfo.drawPlayer.itemRotation);
					num2 = val3.X;
					num3 = val3.Y;
				}
				if (drawinfo.drawPlayer.gravDir == -1f)
				{
					if (drawinfo.drawPlayer.direction == -1)
					{
						num19 += 1.57f;
						((Vector2)(ref origin5))._002Ector((float)itemDrawFrame.Width, 0f);
						num2 -= (float)itemDrawFrame.Width;
					}
					else
					{
						num19 -= 1.57f;
						origin5 = Vector2.Zero;
					}
				}
				else if (drawinfo.drawPlayer.direction == -1)
				{
					((Vector2)(ref origin5))._002Ector((float)itemDrawFrame.Width, (float)itemDrawFrame.Height);
					num2 -= (float)itemDrawFrame.Width;
				}
				ItemLoader.HoldoutOrigin(drawinfo.drawPlayer, ref origin5);
				item = new DrawData(value, new Vector2((float)(int)(drawinfo.ItemLocation.X - Main.screenPosition.X + origin5.X + num2), (float)(int)(drawinfo.ItemLocation.Y - Main.screenPosition.Y + num3)), itemDrawFrame, heldItem.GetAlpha(drawinfo.itemColor), num19, origin5, adjustedItemScale, drawinfo.itemEffect);
				drawinfo.DrawDataCache.Add(item);
				if (num == 3870)
				{
					item = new DrawData(TextureAssets.GlowMask[238].Value, new Vector2((float)(int)(drawinfo.ItemLocation.X - Main.screenPosition.X + origin5.X + num2), (float)(int)(drawinfo.ItemLocation.Y - Main.screenPosition.Y + num3)), itemDrawFrame, new Color(255, 255, 255, 127), num19, origin5, adjustedItemScale, drawinfo.itemEffect);
					drawinfo.DrawDataCache.Add(item);
				}
				return;
			}
			if (num == 5118)
			{
				float rotation = drawinfo.drawPlayer.itemRotation + 1.57f * (float)drawinfo.drawPlayer.direction;
				Vector2 vector7 = default(Vector2);
				((Vector2)(ref vector7))._002Ector((float)itemDrawFrame.Width * 0.5f, (float)itemDrawFrame.Height * 0.5f);
				Vector2 origin6 = default(Vector2);
				((Vector2)(ref origin6))._002Ector((float)itemDrawFrame.Width * 0.5f, (float)itemDrawFrame.Height);
				Vector2 spinningpoint = new Vector2(10f, 4f) * drawinfo.drawPlayer.Directions;
				spinningpoint = spinningpoint.RotatedBy(drawinfo.drawPlayer.itemRotation);
				item = new DrawData(value, new Vector2((float)(int)(drawinfo.ItemLocation.X - Main.screenPosition.X + vector7.X + spinningpoint.X), (float)(int)(drawinfo.ItemLocation.Y - Main.screenPosition.Y + vector7.Y + spinningpoint.Y)), itemDrawFrame, heldItem.GetAlpha(drawinfo.itemColor), rotation, origin6, adjustedItemScale, drawinfo.itemEffect);
				drawinfo.DrawDataCache.Add(item);
				return;
			}
			int num4 = 10;
			Vector2 vector8 = default(Vector2);
			((Vector2)(ref vector8))._002Ector((float)(itemDrawFrame.Width / 2), (float)(itemDrawFrame.Height / 2));
			Vector2 vector2 = Main.DrawPlayerItemPos(drawinfo.drawPlayer.gravDir, num);
			num4 = (int)vector2.X;
			vector8.Y = vector2.Y;
			Vector2 origin7 = default(Vector2);
			((Vector2)(ref origin7))._002Ector((float)(-num4), (float)(itemDrawFrame.Height / 2));
			if (drawinfo.drawPlayer.direction == -1)
			{
				((Vector2)(ref origin7))._002Ector((float)(itemDrawFrame.Width + num4), (float)(itemDrawFrame.Height / 2));
			}
			item = new DrawData(value, new Vector2((float)(int)(drawinfo.ItemLocation.X - Main.screenPosition.X + vector8.X), (float)(int)(drawinfo.ItemLocation.Y - Main.screenPosition.Y + vector8.Y)), itemDrawFrame, heldItem.GetAlpha(drawinfo.itemColor), drawinfo.drawPlayer.itemRotation, origin7, adjustedItemScale, drawinfo.itemEffect);
			drawinfo.DrawDataCache.Add(item);
			if (heldItem.color != default(Color))
			{
				item = new DrawData(value, new Vector2((float)(int)(drawinfo.ItemLocation.X - Main.screenPosition.X + vector8.X), (float)(int)(drawinfo.ItemLocation.Y - Main.screenPosition.Y + vector8.Y)), itemDrawFrame, heldItem.GetColor(drawinfo.itemColor), drawinfo.drawPlayer.itemRotation, origin7, adjustedItemScale, drawinfo.itemEffect);
				drawinfo.DrawDataCache.Add(item);
			}
			if (heldItem.glowMask != -1)
			{
				item = new DrawData(TextureAssets.GlowMask[heldItem.glowMask].Value, new Vector2((float)(int)(drawinfo.ItemLocation.X - Main.screenPosition.X + vector8.X), (float)(int)(drawinfo.ItemLocation.Y - Main.screenPosition.Y + vector8.Y)), itemDrawFrame, new Color(250, 250, 250, heldItem.alpha), drawinfo.drawPlayer.itemRotation, origin7, adjustedItemScale, drawinfo.itemEffect);
				drawinfo.DrawDataCache.Add(item);
			}
			if (num == 3788)
			{
				float num5 = ((float)drawinfo.drawPlayer.miscCounter / 75f * ((float)Math.PI * 2f)).ToRotationVector2().X * 1f + 0f;
				Color color3 = new Color(80, 40, 252, 0) * (num5 / 2f * 0.3f + 0.85f) * 0.5f;
				for (float num6 = 0f; num6 < 4f; num6 += 1f)
				{
					item = new DrawData(TextureAssets.GlowMask[220].Value, new Vector2((float)(int)(drawinfo.ItemLocation.X - Main.screenPosition.X + vector8.X), (float)(int)(drawinfo.ItemLocation.Y - Main.screenPosition.Y + vector8.Y)) + (num6 * ((float)Math.PI / 2f) + drawinfo.drawPlayer.itemRotation).ToRotationVector2() * num5, null, color3, drawinfo.drawPlayer.itemRotation, origin7, adjustedItemScale, drawinfo.itemEffect);
					drawinfo.DrawDataCache.Add(item);
				}
			}
			return;
		}
		if (drawinfo.drawPlayer.gravDir == -1f)
		{
			item = new DrawData(value, position, itemDrawFrame, heldItem.GetAlpha(drawinfo.itemColor), num15, origin, adjustedItemScale, drawinfo.itemEffect);
			drawinfo.DrawDataCache.Add(item);
			if (heldItem.color != default(Color))
			{
				item = new DrawData(value, position, itemDrawFrame, heldItem.GetColor(drawinfo.itemColor), num15, origin, adjustedItemScale, drawinfo.itemEffect);
				drawinfo.DrawDataCache.Add(item);
			}
			if (heldItem.glowMask != -1)
			{
				item = new DrawData(TextureAssets.GlowMask[heldItem.glowMask].Value, position, itemDrawFrame, new Color(250, 250, 250, heldItem.alpha), num15, origin, adjustedItemScale, drawinfo.itemEffect);
				drawinfo.DrawDataCache.Add(item);
			}
			return;
		}
		item = new DrawData(value, position, itemDrawFrame, heldItem.GetAlpha(drawinfo.itemColor), num15, origin, adjustedItemScale, drawinfo.itemEffect);
		drawinfo.DrawDataCache.Add(item);
		if (heldItem.color != default(Color))
		{
			item = new DrawData(value, position, itemDrawFrame, heldItem.GetColor(drawinfo.itemColor), num15, origin, adjustedItemScale, drawinfo.itemEffect);
			drawinfo.DrawDataCache.Add(item);
		}
		if (heldItem.glowMask != -1)
		{
			item = new DrawData(TextureAssets.GlowMask[heldItem.glowMask].Value, position, itemDrawFrame, color, num15, origin, adjustedItemScale, drawinfo.itemEffect);
			drawinfo.DrawDataCache.Add(item);
		}
		if (!heldItem.flame || drawinfo.shadow != 0f)
		{
			return;
		}
		try
		{
			Main.instance.LoadItemFlames(num);
			if (TextureAssets.ItemFlame[num].IsLoaded)
			{
				Color color4 = default(Color);
				((Color)(ref color4))._002Ector(100, 100, 100, 0);
				int num7 = 7;
				float num8 = 1f;
				float num9 = 0f;
				switch (num)
				{
				case 3045:
					((Color)(ref color4))._002Ector(Main.DiscoR, Main.DiscoG, Main.DiscoB, 0);
					break;
				case 5293:
					((Color)(ref color4))._002Ector(50, 50, 100, 20);
					break;
				case 5353:
					((Color)(ref color4))._002Ector(255, 255, 255, 200);
					break;
				case 4952:
					num7 = 3;
					num8 = 0.6f;
					((Color)(ref color4))._002Ector(50, 50, 50, 0);
					break;
				case 5322:
					((Color)(ref color4))._002Ector(100, 100, 100, 150);
					num9 = -2 * drawinfo.drawPlayer.direction;
					break;
				}
				for (int i = 0; i < num7; i++)
				{
					float num10 = drawinfo.drawPlayer.itemFlamePos[i].X * adjustedItemScale * num8;
					float num11 = drawinfo.drawPlayer.itemFlamePos[i].Y * adjustedItemScale * num8;
					item = new DrawData(TextureAssets.ItemFlame[num].Value, new Vector2((float)(int)(position.X + num10 + num9), (float)(int)(position.Y + num11)), itemDrawFrame, color4, num15, origin, adjustedItemScale, drawinfo.itemEffect);
					drawinfo.DrawDataCache.Add(item);
				}
			}
		}
		catch
		{
		}
	}

	public static void DrawPlayer_28_ArmOverItem(ref PlayerDrawSet drawinfo)
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0790: Unknown result type (might be due to invalid IL or missing references)
		//IL_079b: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_07cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_07db: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0802: Unknown result type (might be due to invalid IL or missing references)
		//IL_08bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_08cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_08fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_090a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0915: Unknown result type (might be due to invalid IL or missing references)
		//IL_0926: Unknown result type (might be due to invalid IL or missing references)
		//IL_0931: Unknown result type (might be due to invalid IL or missing references)
		//IL_09de: Unknown result type (might be due to invalid IL or missing references)
		//IL_09e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a19: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a1e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a29: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a34: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a45: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a50: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03af: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03df: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0401: Unknown result type (might be due to invalid IL or missing references)
		//IL_040c: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0508: Unknown result type (might be due to invalid IL or missing references)
		//IL_050d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0512: Unknown result type (might be due to invalid IL or missing references)
		//IL_0519: Unknown result type (might be due to invalid IL or missing references)
		//IL_052a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0535: Unknown result type (might be due to invalid IL or missing references)
		//IL_0276: Unknown result type (might be due to invalid IL or missing references)
		//IL_0281: Unknown result type (might be due to invalid IL or missing references)
		//IL_0286: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_0189: Unknown result type (might be due to invalid IL or missing references)
		//IL_0194: Unknown result type (might be due to invalid IL or missing references)
		//IL_019f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_064e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0659: Unknown result type (might be due to invalid IL or missing references)
		//IL_065e: Unknown result type (might be due to invalid IL or missing references)
		//IL_068f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0694: Unknown result type (might be due to invalid IL or missing references)
		//IL_0699: Unknown result type (might be due to invalid IL or missing references)
		//IL_069f: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b8: Unknown result type (might be due to invalid IL or missing references)
		if (drawinfo.usesCompositeTorso)
		{
			DrawPlayer_28_ArmOverItemComposite(ref drawinfo);
		}
		else if (drawinfo.drawPlayer.body > 0)
		{
			Rectangle bodyFrame = drawinfo.drawPlayer.bodyFrame;
			int num = drawinfo.armorAdjust;
			bodyFrame.X += num;
			bodyFrame.Width -= num;
			if (drawinfo.drawPlayer.direction == -1)
			{
				num = 0;
			}
			if (drawinfo.drawPlayer.invis && (drawinfo.drawPlayer.body == 21 || drawinfo.drawPlayer.body == 22))
			{
				return;
			}
			DrawData item;
			if (drawinfo.missingHand && !drawinfo.drawPlayer.invis)
			{
				_ = drawinfo.drawPlayer.body;
				DrawData drawData;
				if (drawinfo.missingArm)
				{
					drawData = new DrawData(TextureAssets.Players[drawinfo.skinVar, 7].Value, new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2)), drawinfo.drawPlayer.bodyFrame, drawinfo.colorBodySkin, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect);
					drawData.shader = drawinfo.skinDyePacked;
					item = drawData;
					drawinfo.DrawDataCache.Add(item);
				}
				drawData = new DrawData(TextureAssets.Players[drawinfo.skinVar, 9].Value, new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2)), drawinfo.drawPlayer.bodyFrame, drawinfo.colorBodySkin, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect);
				drawData.shader = drawinfo.skinDyePacked;
				item = drawData;
				drawinfo.DrawDataCache.Add(item);
			}
			item = new DrawData(TextureAssets.ArmorArm[drawinfo.drawPlayer.body].Value, new Vector2((float)((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)) + num), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2)), bodyFrame, drawinfo.colorArmorBody, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect);
			item.shader = drawinfo.cBody;
			drawinfo.DrawDataCache.Add(item);
			if (drawinfo.armGlowMask != -1)
			{
				item = new DrawData(TextureAssets.GlowMask[drawinfo.armGlowMask].Value, new Vector2((float)((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)) + num), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2)), bodyFrame, drawinfo.armGlowColor, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect);
				item.shader = drawinfo.cBody;
				drawinfo.DrawDataCache.Add(item);
			}
			if (drawinfo.drawPlayer.body == 205)
			{
				Color color = default(Color);
				((Color)(ref color))._002Ector(100, 100, 100, 0);
				ulong seed = (ulong)(drawinfo.drawPlayer.miscCounter / 4);
				int num2 = 4;
				for (int i = 0; i < num2; i++)
				{
					float num3 = (float)Utils.RandomInt(ref seed, -10, 11) * 0.2f;
					float num4 = (float)Utils.RandomInt(ref seed, -10, 1) * 0.15f;
					item = new DrawData(TextureAssets.GlowMask[240].Value, new Vector2((float)((int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)) + num), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + num3, (float)(drawinfo.drawPlayer.bodyFrame.Height / 2) + num4), bodyFrame, color, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect);
					item.shader = drawinfo.cBody;
					drawinfo.DrawDataCache.Add(item);
				}
			}
		}
		else if (!drawinfo.drawPlayer.invis)
		{
			DrawData drawData2 = new DrawData(TextureAssets.Players[drawinfo.skinVar, 7].Value, new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2)), drawinfo.drawPlayer.bodyFrame, drawinfo.colorBodySkin, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect);
			drawData2.shader = drawinfo.skinDyePacked;
			DrawData item2 = drawData2;
			drawinfo.DrawDataCache.Add(item2);
			item2 = new DrawData(TextureAssets.Players[drawinfo.skinVar, 8].Value, new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2)), drawinfo.drawPlayer.bodyFrame, drawinfo.colorUnderShirt, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect);
			drawinfo.DrawDataCache.Add(item2);
			item2 = new DrawData(TextureAssets.Players[drawinfo.skinVar, 13].Value, new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2)), drawinfo.drawPlayer.bodyFrame, drawinfo.colorShirt, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect);
			drawinfo.DrawDataCache.Add(item2);
		}
	}

	public static void DrawPlayer_28_ArmOverItemComposite(ref PlayerDrawSet drawinfo)
	{
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		//IL_0169: Unknown result type (might be due to invalid IL or missing references)
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0173: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0191: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_087c: Unknown result type (might be due to invalid IL or missing references)
		//IL_087e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0889: Unknown result type (might be due to invalid IL or missing references)
		//IL_088f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0897: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0501: Unknown result type (might be due to invalid IL or missing references)
		//IL_0509: Unknown result type (might be due to invalid IL or missing references)
		//IL_0539: Unknown result type (might be due to invalid IL or missing references)
		//IL_053c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0547: Unknown result type (might be due to invalid IL or missing references)
		//IL_054d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0555: Unknown result type (might be due to invalid IL or missing references)
		//IL_0445: Unknown result type (might be due to invalid IL or missing references)
		//IL_0448: Unknown result type (might be due to invalid IL or missing references)
		//IL_0453: Unknown result type (might be due to invalid IL or missing references)
		//IL_0459: Unknown result type (might be due to invalid IL or missing references)
		//IL_0461: Unknown result type (might be due to invalid IL or missing references)
		//IL_077c: Unknown result type (might be due to invalid IL or missing references)
		//IL_077e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0789: Unknown result type (might be due to invalid IL or missing references)
		//IL_078f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0797: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_07db: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0813: Unknown result type (might be due to invalid IL or missing references)
		//IL_0815: Unknown result type (might be due to invalid IL or missing references)
		//IL_0820: Unknown result type (might be due to invalid IL or missing references)
		//IL_0826: Unknown result type (might be due to invalid IL or missing references)
		//IL_082e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0722: Unknown result type (might be due to invalid IL or missing references)
		//IL_0724: Unknown result type (might be due to invalid IL or missing references)
		//IL_072f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0735: Unknown result type (might be due to invalid IL or missing references)
		//IL_073d: Unknown result type (might be due to invalid IL or missing references)
		//IL_057f: Unknown result type (might be due to invalid IL or missing references)
		//IL_05fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0601: Unknown result type (might be due to invalid IL or missing references)
		//IL_060c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0611: Unknown result type (might be due to invalid IL or missing references)
		//IL_0617: Unknown result type (might be due to invalid IL or missing references)
		//IL_061c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0621: Unknown result type (might be due to invalid IL or missing references)
		//IL_0635: Unknown result type (might be due to invalid IL or missing references)
		//IL_063d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0648: Unknown result type (might be due to invalid IL or missing references)
		//IL_0659: Unknown result type (might be due to invalid IL or missing references)
		//IL_0664: Unknown result type (might be due to invalid IL or missing references)
		//IL_069f: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_02af: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03be: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0366: Unknown result type (might be due to invalid IL or missing references)
		//IL_0369: Unknown result type (might be due to invalid IL or missing references)
		//IL_0374: Unknown result type (might be due to invalid IL or missing references)
		//IL_037a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0382: Unknown result type (might be due to invalid IL or missing references)
		//IL_0314: Unknown result type (might be due to invalid IL or missing references)
		//IL_0316: Unknown result type (might be due to invalid IL or missing references)
		//IL_0321: Unknown result type (might be due to invalid IL or missing references)
		//IL_0327: Unknown result type (might be due to invalid IL or missing references)
		//IL_032f: Unknown result type (might be due to invalid IL or missing references)
		Vector2 vector = new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2));
		Vector2 vector2 = Main.OffsetsPlayerHeadgear[drawinfo.drawPlayer.bodyFrame.Y / drawinfo.drawPlayer.bodyFrame.Height];
		vector2.Y -= 2f;
		vector += vector2 * (float)(-((Enum)drawinfo.playerEffect).HasFlag((Enum)(object)(SpriteEffects)2).ToDirectionInt());
		float bodyRotation = drawinfo.drawPlayer.bodyRotation;
		float rotation = drawinfo.drawPlayer.bodyRotation + drawinfo.compositeFrontArmRotation;
		Vector2 bodyVect = drawinfo.bodyVect;
		Vector2 compositeOffset_FrontArm = GetCompositeOffset_FrontArm(ref drawinfo);
		bodyVect += compositeOffset_FrontArm;
		vector += compositeOffset_FrontArm;
		Vector2 position = vector + drawinfo.frontShoulderOffset;
		if (drawinfo.compFrontArmFrame.X / drawinfo.compFrontArmFrame.Width >= 7)
		{
			vector += new Vector2((float)((!((Enum)drawinfo.playerEffect).HasFlag((Enum)(object)(SpriteEffects)1)) ? 1 : (-1)), (float)((!((Enum)drawinfo.playerEffect).HasFlag((Enum)(object)(SpriteEffects)2)) ? 1 : (-1)));
		}
		_ = drawinfo.drawPlayer.invis;
		bool num5 = drawinfo.drawPlayer.body > 0;
		int num2 = (drawinfo.compShoulderOverFrontArm ? 1 : 0);
		int num3 = ((!drawinfo.compShoulderOverFrontArm) ? 1 : 0);
		int num4 = ((!drawinfo.compShoulderOverFrontArm) ? 1 : 0);
		bool flag = !drawinfo.hidesTopSkin;
		if (num5)
		{
			if (!drawinfo.drawPlayer.invis || IsArmorDrawnWhenInvisible(drawinfo.drawPlayer.body))
			{
				Texture2D value = TextureAssets.ArmorBodyComposite[drawinfo.drawPlayer.body].Value;
				for (int i = 0; i < 2; i++)
				{
					if (!drawinfo.drawPlayer.invis && i == num4 && flag)
					{
						if (drawinfo.missingArm)
						{
							drawinfo.DrawDataCache.Add(new DrawData(TextureAssets.Players[drawinfo.skinVar, 7].Value, vector, drawinfo.compFrontArmFrame, drawinfo.colorBodySkin, rotation, bodyVect, 1f, drawinfo.playerEffect)
							{
								shader = drawinfo.skinDyePacked
							});
						}
						if (drawinfo.missingHand)
						{
							drawinfo.DrawDataCache.Add(new DrawData(TextureAssets.Players[drawinfo.skinVar, 9].Value, vector, drawinfo.compFrontArmFrame, drawinfo.colorBodySkin, rotation, bodyVect, 1f, drawinfo.playerEffect)
							{
								shader = drawinfo.skinDyePacked
							});
						}
					}
					if (i == num2 && !drawinfo.hideCompositeShoulders)
					{
						DrawCompositeArmorPiece(ref drawinfo, CompositePlayerDrawContext.FrontShoulder, new DrawData(value, position, drawinfo.compFrontShoulderFrame, drawinfo.colorArmorBody, bodyRotation, bodyVect, 1f, drawinfo.playerEffect)
						{
							shader = drawinfo.cBody
						});
					}
					if (i == num3)
					{
						DrawCompositeArmorPiece(ref drawinfo, CompositePlayerDrawContext.FrontArm, new DrawData(value, vector, drawinfo.compFrontArmFrame, drawinfo.colorArmorBody, rotation, bodyVect, 1f, drawinfo.playerEffect)
						{
							shader = drawinfo.cBody
						});
					}
				}
			}
		}
		else if (!drawinfo.drawPlayer.invis)
		{
			for (int j = 0; j < 2; j++)
			{
				if (j == num2)
				{
					if (flag)
					{
						drawinfo.DrawDataCache.Add(new DrawData(TextureAssets.Players[drawinfo.skinVar, 7].Value, position, drawinfo.compFrontShoulderFrame, drawinfo.colorBodySkin, bodyRotation, bodyVect, 1f, drawinfo.playerEffect)
						{
							shader = drawinfo.skinDyePacked
						});
					}
					drawinfo.DrawDataCache.Add(new DrawData(TextureAssets.Players[drawinfo.skinVar, 8].Value, position, drawinfo.compFrontShoulderFrame, drawinfo.colorUnderShirt, bodyRotation, bodyVect, 1f, drawinfo.playerEffect));
					drawinfo.DrawDataCache.Add(new DrawData(TextureAssets.Players[drawinfo.skinVar, 13].Value, position, drawinfo.compFrontShoulderFrame, drawinfo.colorShirt, bodyRotation, bodyVect, 1f, drawinfo.playerEffect));
					drawinfo.DrawDataCache.Add(new DrawData(TextureAssets.Players[drawinfo.skinVar, 6].Value, position, drawinfo.compFrontShoulderFrame, drawinfo.colorShirt, bodyRotation, bodyVect, 1f, drawinfo.playerEffect));
					if (drawinfo.drawPlayer.head == 269)
					{
						Vector2 position2 = drawinfo.helmetOffset + new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.headPosition + drawinfo.headVect;
						DrawData item = new DrawData(TextureAssets.Extra[214].Value, position2, drawinfo.drawPlayer.bodyFrame, drawinfo.colorArmorHead, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect);
						item.shader = drawinfo.cHead;
						drawinfo.DrawDataCache.Add(item);
						item = new DrawData(TextureAssets.GlowMask[308].Value, position2, drawinfo.drawPlayer.bodyFrame, drawinfo.headGlowColor, drawinfo.drawPlayer.headRotation, drawinfo.headVect, 1f, drawinfo.playerEffect);
						item.shader = drawinfo.cHead;
						drawinfo.DrawDataCache.Add(item);
					}
				}
				if (j == num3)
				{
					if (flag)
					{
						drawinfo.DrawDataCache.Add(new DrawData(TextureAssets.Players[drawinfo.skinVar, 7].Value, vector, drawinfo.compFrontArmFrame, drawinfo.colorBodySkin, rotation, bodyVect, 1f, drawinfo.playerEffect)
						{
							shader = drawinfo.skinDyePacked
						});
					}
					drawinfo.DrawDataCache.Add(new DrawData(TextureAssets.Players[drawinfo.skinVar, 8].Value, vector, drawinfo.compFrontArmFrame, drawinfo.colorUnderShirt, rotation, bodyVect, 1f, drawinfo.playerEffect));
					drawinfo.DrawDataCache.Add(new DrawData(TextureAssets.Players[drawinfo.skinVar, 13].Value, vector, drawinfo.compFrontArmFrame, drawinfo.colorShirt, rotation, bodyVect, 1f, drawinfo.playerEffect));
					drawinfo.DrawDataCache.Add(new DrawData(TextureAssets.Players[drawinfo.skinVar, 6].Value, vector, drawinfo.compFrontArmFrame, drawinfo.colorShirt, rotation, bodyVect, 1f, drawinfo.playerEffect));
				}
			}
		}
		if (drawinfo.drawPlayer.handon > 0)
		{
			Texture2D value2 = TextureAssets.AccHandsOnComposite[drawinfo.drawPlayer.handon].Value;
			DrawCompositeArmorPiece(ref drawinfo, CompositePlayerDrawContext.FrontArmAccessory, new DrawData(value2, vector, drawinfo.compFrontArmFrame, drawinfo.colorArmorBody, rotation, bodyVect, 1f, drawinfo.playerEffect)
			{
				shader = drawinfo.cHandOn
			});
		}
	}

	public static void DrawPlayer_29_OnhandAcc(ref PlayerDrawSet drawinfo)
	{
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		if (!drawinfo.usesCompositeFrontHandAcc && drawinfo.drawPlayer.handon > 0)
		{
			DrawData item = new DrawData(TextureAssets.AccHandsOn[drawinfo.drawPlayer.handon].Value, new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2)), drawinfo.drawPlayer.bodyFrame, drawinfo.colorArmorBody, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect);
			item.shader = drawinfo.cHandOn;
			drawinfo.DrawDataCache.Add(item);
		}
	}

	public static void DrawPlayer_30_BladedGlove(ref PlayerDrawSet drawinfo)
	{
		//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0238: Unknown result type (might be due to invalid IL or missing references)
		//IL_024a: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Unknown result type (might be due to invalid IL or missing references)
		Item heldItem = drawinfo.heldItem;
		if (heldItem.type <= -1 || !Item.claw[heldItem.type] || drawinfo.shadow != 0f)
		{
			return;
		}
		Main.instance.LoadItem(heldItem.type);
		Asset<Texture2D> asset = TextureAssets.Item[heldItem.type];
		if (!drawinfo.drawPlayer.frozen && (drawinfo.drawPlayer.itemAnimation > 0 || (heldItem.holdStyle != 0 && !drawinfo.drawPlayer.pulley)) && heldItem.type > 0 && !drawinfo.drawPlayer.dead && !heldItem.noUseGraphic && (!drawinfo.drawPlayer.wet || !heldItem.noWet))
		{
			if (drawinfo.drawPlayer.gravDir == -1f)
			{
				DrawData item = new DrawData(asset.Value, new Vector2((float)(int)(drawinfo.ItemLocation.X - Main.screenPosition.X), (float)(int)(drawinfo.ItemLocation.Y - Main.screenPosition.Y)), (Rectangle?)new Rectangle(0, 0, asset.Width(), asset.Height()), heldItem.GetAlpha(drawinfo.itemColor), drawinfo.drawPlayer.itemRotation, new Vector2((float)asset.Width() * 0.5f - (float)asset.Width() * 0.5f * (float)drawinfo.drawPlayer.direction, 0f), drawinfo.drawPlayer.GetAdjustedItemScale(heldItem), drawinfo.itemEffect, 0f);
				drawinfo.DrawDataCache.Add(item);
			}
			else
			{
				DrawData item2 = new DrawData(asset.Value, new Vector2((float)(int)(drawinfo.ItemLocation.X - Main.screenPosition.X), (float)(int)(drawinfo.ItemLocation.Y - Main.screenPosition.Y)), (Rectangle?)new Rectangle(0, 0, asset.Width(), asset.Height()), heldItem.GetAlpha(drawinfo.itemColor), drawinfo.drawPlayer.itemRotation, new Vector2((float)asset.Width() * 0.5f - (float)asset.Width() * 0.5f * (float)drawinfo.drawPlayer.direction, (float)asset.Height()), drawinfo.drawPlayer.GetAdjustedItemScale(heldItem), drawinfo.itemEffect, 0f);
				drawinfo.DrawDataCache.Add(item2);
			}
		}
	}

	public static void DrawPlayer_31_ProjectileOverArm(ref PlayerDrawSet drawinfo)
	{
		if (drawinfo.drawPlayer.heldProj >= 0 && drawinfo.shadow == 0f && drawinfo.heldProjOverHand)
		{
			drawinfo.projectileDrawPosition = drawinfo.DrawDataCache.Count;
		}
	}

	public static void DrawPlayer_32_FrontAcc(ref PlayerDrawSet drawinfo)
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		if (drawinfo.drawPlayer.front > 0 && !drawinfo.drawPlayer.mount.Active)
		{
			Vector2 zero = Vector2.Zero;
			DrawData item = new DrawData(TextureAssets.AccFront[drawinfo.drawPlayer.front].Value, zero + new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2)), drawinfo.drawPlayer.bodyFrame, drawinfo.colorArmorBody, drawinfo.drawPlayer.bodyRotation, drawinfo.bodyVect, 1f, drawinfo.playerEffect);
			item.shader = drawinfo.cFront;
			drawinfo.DrawDataCache.Add(item);
		}
	}

	public static void DrawPlayer_32_FrontAcc_FrontPart(ref PlayerDrawSet drawinfo)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		//IL_0197: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_019b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_020d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0218: Unknown result type (might be due to invalid IL or missing references)
		//IL_021d: Unknown result type (might be due to invalid IL or missing references)
		//IL_021f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0233: Unknown result type (might be due to invalid IL or missing references)
		//IL_0238: Unknown result type (might be due to invalid IL or missing references)
		//IL_024c: Unknown result type (might be due to invalid IL or missing references)
		//IL_025b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0260: Unknown result type (might be due to invalid IL or missing references)
		//IL_0265: Unknown result type (might be due to invalid IL or missing references)
		//IL_026c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0279: Unknown result type (might be due to invalid IL or missing references)
		//IL_0280: Unknown result type (might be due to invalid IL or missing references)
		if (drawinfo.drawPlayer.front <= 0)
		{
			return;
		}
		Rectangle bodyFrame = drawinfo.drawPlayer.bodyFrame;
		int num = bodyFrame.Width / 2;
		bodyFrame.Width -= num;
		Vector2 bodyVect = drawinfo.bodyVect;
		if (((Enum)drawinfo.playerEffect).HasFlag((Enum)(object)(SpriteEffects)1))
		{
			bodyVect.X -= num;
		}
		Vector2 vector = Vector2.Zero + new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2));
		DrawData item = new DrawData(TextureAssets.AccFront[drawinfo.drawPlayer.front].Value, vector, bodyFrame, drawinfo.colorArmorBody, drawinfo.drawPlayer.bodyRotation, bodyVect, 1f, drawinfo.playerEffect);
		item.shader = drawinfo.cFront;
		drawinfo.DrawDataCache.Add(item);
		if (drawinfo.drawPlayer.front == 12)
		{
			Rectangle rectangle = bodyFrame;
			Rectangle value = rectangle;
			value.Width = 2;
			int num2 = 0;
			int num3 = rectangle.Width / 2;
			int num4 = 2;
			if (((Enum)drawinfo.playerEffect).HasFlag((Enum)(object)(SpriteEffects)1))
			{
				num2 = rectangle.Width - 2;
				num4 = -2;
			}
			for (int i = 0; i < num3; i++)
			{
				value.X = rectangle.X + 2 * i;
				Color immuneAlpha = drawinfo.drawPlayer.GetImmuneAlpha(LiquidRenderer.GetShimmerGlitterColor(top: true, (float)i / 16f, 0f), drawinfo.shadow);
				immuneAlpha *= (float)(int)((Color)(ref drawinfo.colorArmorBody)).A / 255f;
				item = new DrawData(TextureAssets.GlowMask[331].Value, vector + new Vector2((float)(num2 + i * num4), 0f), value, immuneAlpha, drawinfo.drawPlayer.bodyRotation, bodyVect, 1f, drawinfo.playerEffect);
				item.shader = drawinfo.cFront;
				drawinfo.DrawDataCache.Add(item);
			}
		}
	}

	public static void DrawPlayer_32_FrontAcc_BackPart(ref PlayerDrawSet drawinfo)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01da: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0219: Unknown result type (might be due to invalid IL or missing references)
		//IL_0224: Unknown result type (might be due to invalid IL or missing references)
		//IL_0229: Unknown result type (might be due to invalid IL or missing references)
		//IL_022b: Unknown result type (might be due to invalid IL or missing references)
		//IL_023f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0244: Unknown result type (might be due to invalid IL or missing references)
		//IL_0258: Unknown result type (might be due to invalid IL or missing references)
		//IL_0267: Unknown result type (might be due to invalid IL or missing references)
		//IL_026c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0271: Unknown result type (might be due to invalid IL or missing references)
		//IL_0278: Unknown result type (might be due to invalid IL or missing references)
		//IL_0285: Unknown result type (might be due to invalid IL or missing references)
		//IL_028c: Unknown result type (might be due to invalid IL or missing references)
		if (drawinfo.drawPlayer.front <= 0)
		{
			return;
		}
		Rectangle bodyFrame = drawinfo.drawPlayer.bodyFrame;
		int num = bodyFrame.Width / 2;
		bodyFrame.Width -= num;
		bodyFrame.X += num;
		Vector2 bodyVect = drawinfo.bodyVect;
		if (!((Enum)drawinfo.playerEffect).HasFlag((Enum)(object)(SpriteEffects)1))
		{
			bodyVect.X -= num;
		}
		Vector2 vector = Vector2.Zero + new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2));
		DrawData item = new DrawData(TextureAssets.AccFront[drawinfo.drawPlayer.front].Value, vector, bodyFrame, drawinfo.colorArmorBody, drawinfo.drawPlayer.bodyRotation, bodyVect, 1f, drawinfo.playerEffect);
		item.shader = drawinfo.cFront;
		drawinfo.DrawDataCache.Add(item);
		if (drawinfo.drawPlayer.front == 12)
		{
			Rectangle rectangle = bodyFrame;
			Rectangle value = rectangle;
			value.Width = 2;
			int num2 = 0;
			int num3 = rectangle.Width / 2;
			int num4 = 2;
			if (((Enum)drawinfo.playerEffect).HasFlag((Enum)(object)(SpriteEffects)1))
			{
				num2 = rectangle.Width - 2;
				num4 = -2;
			}
			for (int i = 0; i < num3; i++)
			{
				value.X = rectangle.X + 2 * i;
				Color immuneAlpha = drawinfo.drawPlayer.GetImmuneAlpha(LiquidRenderer.GetShimmerGlitterColor(top: true, (float)i / 16f, 0f), drawinfo.shadow);
				immuneAlpha *= (float)(int)((Color)(ref drawinfo.colorArmorBody)).A / 255f;
				item = new DrawData(TextureAssets.GlowMask[331].Value, vector + new Vector2((float)(num2 + i * num4), 0f), value, immuneAlpha, drawinfo.drawPlayer.bodyRotation, bodyVect, 1f, drawinfo.playerEffect);
				item.shader = drawinfo.cFront;
				drawinfo.DrawDataCache.Add(item);
			}
		}
	}

	public static void DrawPlayer_33_FrozenOrWebbedDebuff(ref PlayerDrawSet drawinfo)
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_017e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0210: Unknown result type (might be due to invalid IL or missing references)
		//IL_021a: Unknown result type (might be due to invalid IL or missing references)
		//IL_021f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_030b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0318: Unknown result type (might be due to invalid IL or missing references)
		//IL_0322: Unknown result type (might be due to invalid IL or missing references)
		//IL_032d: Unknown result type (might be due to invalid IL or missing references)
		if (!drawinfo.drawPlayer.shimmering)
		{
			if (drawinfo.drawPlayer.frozen && drawinfo.shadow == 0f)
			{
				Color colorArmorBody = drawinfo.colorArmorBody;
				((Color)(ref colorArmorBody)).R = (byte)((double)(int)((Color)(ref colorArmorBody)).R * 0.55);
				((Color)(ref colorArmorBody)).G = (byte)((double)(int)((Color)(ref colorArmorBody)).G * 0.55);
				((Color)(ref colorArmorBody)).B = (byte)((double)(int)((Color)(ref colorArmorBody)).B * 0.55);
				((Color)(ref colorArmorBody)).A = (byte)((double)(int)((Color)(ref colorArmorBody)).A * 0.55);
				DrawData item = new DrawData(TextureAssets.Frozen.Value, new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2)), (Rectangle?)new Rectangle(0, 0, TextureAssets.Frozen.Width(), TextureAssets.Frozen.Height()), colorArmorBody, drawinfo.drawPlayer.bodyRotation, new Vector2((float)(TextureAssets.Frozen.Width() / 2), (float)(TextureAssets.Frozen.Height() / 2)), 1f, drawinfo.playerEffect, 0f);
				drawinfo.DrawDataCache.Add(item);
			}
			else if (drawinfo.drawPlayer.webbed && drawinfo.shadow == 0f && drawinfo.drawPlayer.velocity.Y == 0f)
			{
				Color color = drawinfo.colorArmorBody * 0.75f;
				Texture2D value = TextureAssets.Extra[31].Value;
				int num = drawinfo.drawPlayer.height / 2;
				DrawData item2 = new DrawData(value, new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f + (float)num)) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2)), null, color, drawinfo.drawPlayer.bodyRotation, value.Size() / 2f, 1f, drawinfo.playerEffect);
				drawinfo.DrawDataCache.Add(item2);
			}
		}
	}

	public static void DrawPlayer_34_ElectrifiedDebuffFront(ref PlayerDrawSet drawinfo)
	{
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		if (!drawinfo.drawPlayer.electrified || drawinfo.shadow != 0f)
		{
			return;
		}
		Texture2D value = TextureAssets.GlowMask[25].Value;
		int num = drawinfo.drawPlayer.miscCounter / 5;
		for (int i = 0; i < 2; i++)
		{
			num %= 7;
			if (num > 1 && num < 5)
			{
				DrawData item = new DrawData(value, new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2)), (Rectangle?)new Rectangle(0, num * value.Height / 7, value.Width, value.Height / 7), drawinfo.colorElectricity, drawinfo.drawPlayer.bodyRotation, new Vector2((float)(value.Width / 2), (float)(value.Height / 14)), 1f, drawinfo.playerEffect, 0f);
				drawinfo.DrawDataCache.Add(item);
			}
			num += 3;
		}
	}

	public static void DrawPlayer_35_IceBarrier(ref PlayerDrawSet drawinfo)
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		if (drawinfo.drawPlayer.iceBarrier && drawinfo.shadow == 0f)
		{
			int num = TextureAssets.IceBarrier.Height() / 12;
			Color white = Color.White;
			DrawData item = new DrawData(TextureAssets.IceBarrier.Value, new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X - (float)(drawinfo.drawPlayer.bodyFrame.Width / 2) + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - (float)drawinfo.drawPlayer.bodyFrame.Height + 4f)) + drawinfo.drawPlayer.bodyPosition + new Vector2((float)(drawinfo.drawPlayer.bodyFrame.Width / 2), (float)(drawinfo.drawPlayer.bodyFrame.Height / 2)), (Rectangle?)new Rectangle(0, num * drawinfo.drawPlayer.iceBarrierFrame, TextureAssets.IceBarrier.Width(), num), white, 0f, new Vector2((float)(TextureAssets.Frozen.Width() / 2), (float)(TextureAssets.Frozen.Height() / 2)), 1f, drawinfo.playerEffect, 0f);
			drawinfo.DrawDataCache.Add(item);
		}
	}

	public static void DrawPlayer_36_CTG(ref PlayerDrawSet drawinfo)
	{
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0165: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0202: Unknown result type (might be due to invalid IL or missing references)
		//IL_0204: Unknown result type (might be due to invalid IL or missing references)
		//IL_0206: Unknown result type (might be due to invalid IL or missing references)
		//IL_020d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0212: Unknown result type (might be due to invalid IL or missing references)
		//IL_0237: Unknown result type (might be due to invalid IL or missing references)
		//IL_0243: Unknown result type (might be due to invalid IL or missing references)
		//IL_024d: Unknown result type (might be due to invalid IL or missing references)
		//IL_017e: Unknown result type (might be due to invalid IL or missing references)
		if (drawinfo.shadow != 0f || (byte)drawinfo.drawPlayer.ownedLargeGems <= 0)
		{
			return;
		}
		bool flag = false;
		BitsByte ownedLargeGems = drawinfo.drawPlayer.ownedLargeGems;
		float num = 0f;
		for (int i = 0; i < 7; i++)
		{
			if (ownedLargeGems[i])
			{
				num += 1f;
			}
		}
		float num2 = 1f - num * 0.06f;
		float num3 = (num - 1f) * 4f;
		switch ((int)num)
		{
		case 2:
			num3 += 10f;
			break;
		case 3:
			num3 += 8f;
			break;
		case 4:
			num3 += 6f;
			break;
		case 5:
			num3 += 6f;
			break;
		case 6:
			num3 += 2f;
			break;
		case 7:
			num3 += 0f;
			break;
		}
		float num4 = (float)drawinfo.drawPlayer.miscCounter / 300f * ((float)Math.PI * 2f);
		if (!(num > 0f))
		{
			return;
		}
		float num5 = (float)Math.PI * 2f / num;
		float num6 = 0f;
		Vector2 vector = default(Vector2);
		((Vector2)(ref vector))._002Ector(1.3f, 0.65f);
		if (!flag)
		{
			vector = Vector2.One;
		}
		List<DrawData> list = new List<DrawData>();
		for (int j = 0; j < 7; j++)
		{
			if (!ownedLargeGems[j])
			{
				num6 += 1f;
				continue;
			}
			Vector2 vector2 = (num4 + num5 * ((float)j - num6)).ToRotationVector2();
			float num7 = num2;
			if (flag)
			{
				num7 = MathHelper.Lerp(num2 * 0.7f, 1f, vector2.Y / 2f + 0.5f);
			}
			Texture2D value = TextureAssets.Gem[j].Value;
			DrawData item = new DrawData(value, new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)drawinfo.drawPlayer.height - 80f)) + vector2 * vector * num3, null, new Color(250, 250, 250, Main.mouseTextColor / 2), 0f, value.Size() / 2f, ((float)(int)Main.mouseTextColor / 1000f + 0.8f) * num7, (SpriteEffects)0);
			list.Add(item);
		}
		if (flag)
		{
			list.Sort(DelegateMethods.CompareDrawSorterByYScale);
		}
		drawinfo.DrawDataCache.AddRange(list);
	}

	public static void DrawPlayer_37_BeetleBuff(ref PlayerDrawSet drawinfo)
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0220: Unknown result type (might be due to invalid IL or missing references)
		//IL_0231: Unknown result type (might be due to invalid IL or missing references)
		//IL_0236: Unknown result type (might be due to invalid IL or missing references)
		//IL_026e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0279: Unknown result type (might be due to invalid IL or missing references)
		//IL_029d: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a8: Unknown result type (might be due to invalid IL or missing references)
		if ((!drawinfo.drawPlayer.beetleOffense && !drawinfo.drawPlayer.beetleDefense) || drawinfo.shadow != 0f)
		{
			return;
		}
		for (int i = 0; i < drawinfo.drawPlayer.beetleOrbs; i++)
		{
			DrawData item;
			for (int j = 0; j < 5; j++)
			{
				Color colorArmorBody = drawinfo.colorArmorBody;
				float num = (float)j * 0.1f;
				num = 0.5f - num;
				((Color)(ref colorArmorBody)).R = (byte)((float)(int)((Color)(ref colorArmorBody)).R * num);
				((Color)(ref colorArmorBody)).G = (byte)((float)(int)((Color)(ref colorArmorBody)).G * num);
				((Color)(ref colorArmorBody)).B = (byte)((float)(int)((Color)(ref colorArmorBody)).B * num);
				((Color)(ref colorArmorBody)).A = (byte)((float)(int)((Color)(ref colorArmorBody)).A * num);
				Vector2 vector = -drawinfo.drawPlayer.beetleVel[i] * (float)j;
				item = new DrawData(TextureAssets.Beetle.Value, new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)(drawinfo.drawPlayer.height / 2))) + drawinfo.drawPlayer.beetlePos[i] + vector, (Rectangle?)new Rectangle(0, TextureAssets.Beetle.Height() / 3 * drawinfo.drawPlayer.beetleFrame + 1, TextureAssets.Beetle.Width(), TextureAssets.Beetle.Height() / 3 - 2), colorArmorBody, 0f, new Vector2((float)(TextureAssets.Beetle.Width() / 2), (float)(TextureAssets.Beetle.Height() / 6)), 1f, drawinfo.playerEffect, 0f);
				drawinfo.DrawDataCache.Add(item);
			}
			item = new DrawData(TextureAssets.Beetle.Value, new Vector2((float)(int)(drawinfo.Position.X - Main.screenPosition.X + (float)(drawinfo.drawPlayer.width / 2)), (float)(int)(drawinfo.Position.Y - Main.screenPosition.Y + (float)(drawinfo.drawPlayer.height / 2))) + drawinfo.drawPlayer.beetlePos[i], (Rectangle?)new Rectangle(0, TextureAssets.Beetle.Height() / 3 * drawinfo.drawPlayer.beetleFrame + 1, TextureAssets.Beetle.Width(), TextureAssets.Beetle.Height() / 3 - 2), drawinfo.colorArmorBody, 0f, new Vector2((float)(TextureAssets.Beetle.Width() / 2), (float)(TextureAssets.Beetle.Height() / 6)), 1f, drawinfo.playerEffect, 0f);
			drawinfo.DrawDataCache.Add(item);
		}
	}

	public static void DrawPlayer_38_EyebrellaCloud(ref PlayerDrawSet drawinfo)
	{
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0226: Unknown result type (might be due to invalid IL or missing references)
		//IL_0228: Unknown result type (might be due to invalid IL or missing references)
		//IL_022e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0235: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		//IL_019b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_01af: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
		if (drawinfo.drawPlayer.eyebrellaCloud && drawinfo.shadow == 0f)
		{
			Texture2D value = TextureAssets.Projectile[238].Value;
			int frameY = drawinfo.drawPlayer.miscCounter % 18 / 6;
			Rectangle value2 = value.Frame(1, 6, 0, frameY);
			Vector2 origin = default(Vector2);
			((Vector2)(ref origin))._002Ector((float)(value2.Width / 2), (float)(value2.Height / 2));
			Vector2 vector = default(Vector2);
			((Vector2)(ref vector))._002Ector(0f, -70f);
			Vector2 vector2 = drawinfo.drawPlayer.MountedCenter - new Vector2(0f, (float)drawinfo.drawPlayer.height * 0.5f) + vector - Main.screenPosition;
			Color color = Lighting.GetColor((drawinfo.drawPlayer.Top + new Vector2(0f, -30f)).ToTileCoordinates());
			int num = 170;
			int g;
			int b;
			int r = (g = (b = num));
			if (((Color)(ref color)).R < num)
			{
				r = ((Color)(ref color)).R;
			}
			if (((Color)(ref color)).G < num)
			{
				g = ((Color)(ref color)).G;
			}
			if (((Color)(ref color)).B < num)
			{
				b = ((Color)(ref color)).B;
			}
			Color color2 = default(Color);
			((Color)(ref color2))._002Ector(r, g, b, 100);
			float num2 = (float)(drawinfo.drawPlayer.miscCounter % 50) / 50f;
			float num3 = 3f;
			DrawData item;
			for (int i = 0; i < 2; i++)
			{
				Vector2 vector3 = Utils.RotatedBy(new Vector2((i == 0) ? (0f - num3) : num3, 0f), num2 * ((float)Math.PI * 2f) * ((i == 0) ? 1f : (-1f)));
				item = new DrawData(value, vector2 + vector3, value2, color2 * 0.65f, 0f, origin, 1f, (SpriteEffects)((drawinfo.drawPlayer.gravDir == -1f) ? 2 : 0));
				item.shader = drawinfo.cHead;
				item.ignorePlayerRotation = true;
				drawinfo.DrawDataCache.Add(item);
			}
			item = new DrawData(value, vector2, value2, color2, 0f, origin, 1f, (SpriteEffects)((drawinfo.drawPlayer.gravDir == -1f) ? 2 : 0));
			item.shader = drawinfo.cHead;
			item.ignorePlayerRotation = true;
			drawinfo.DrawDataCache.Add(item);
		}
	}

	private static Vector2 GetCompositeOffset_BackArm(ref PlayerDrawSet drawinfo)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		return new Vector2((float)(6 * ((!((Enum)drawinfo.playerEffect).HasFlag((Enum)(object)(SpriteEffects)1)) ? 1 : (-1))), (float)(2 * ((!((Enum)drawinfo.playerEffect).HasFlag((Enum)(object)(SpriteEffects)2)) ? 1 : (-1))));
	}

	private static Vector2 GetCompositeOffset_FrontArm(ref PlayerDrawSet drawinfo)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		return new Vector2((float)(-5 * ((!((Enum)drawinfo.playerEffect).HasFlag((Enum)(object)(SpriteEffects)1)) ? 1 : (-1))), 0f);
	}

	public static void DrawPlayer_TransformDrawData(ref PlayerDrawSet drawinfo)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		Vector2 vector = drawinfo.Position - Main.screenPosition + drawinfo.rotationOrigin;
		Vector2 vector2 = drawinfo.drawPlayer.position + drawinfo.rotationOrigin;
		Matrix matrix = Matrix.CreateRotationZ(drawinfo.rotation);
		for (int i = 0; i < drawinfo.DustCache.Count; i++)
		{
			Vector2 position = Main.dust[drawinfo.DustCache[i]].position - vector2;
			position = Vector2.Transform(position, matrix);
			Main.dust[drawinfo.DustCache[i]].position = position + vector2;
		}
		for (int j = 0; j < drawinfo.GoreCache.Count; j++)
		{
			Vector2 position2 = Main.gore[drawinfo.GoreCache[j]].position - vector2;
			position2 = Vector2.Transform(position2, matrix);
			Main.gore[drawinfo.GoreCache[j]].position = position2 + vector2;
		}
		for (int k = 0; k < drawinfo.DrawDataCache.Count; k++)
		{
			DrawData value = drawinfo.DrawDataCache[k];
			if (!value.ignorePlayerRotation)
			{
				Vector2 position3 = value.position - vector;
				position3 = Vector2.Transform(position3, matrix);
				value.position = position3 + vector;
				value.rotation += drawinfo.rotation;
				drawinfo.DrawDataCache[k] = value;
			}
		}
	}

	public static void DrawPlayer_ScaleDrawData(ref PlayerDrawSet drawinfo, float scale)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		if (scale != 1f)
		{
			Vector2 vector = drawinfo.Position + drawinfo.drawPlayer.Size * new Vector2(0.5f, 1f) - Main.screenPosition;
			for (int i = 0; i < drawinfo.DrawDataCache.Count; i++)
			{
				DrawData value = drawinfo.DrawDataCache[i];
				Vector2 vector2 = value.position - vector;
				value.position = vector + vector2 * scale;
				ref Vector2 scale2 = ref value.scale;
				scale2 *= scale;
				drawinfo.DrawDataCache[i] = value;
			}
		}
	}

	public static void DrawPlayer_AddSelectionGlow(ref PlayerDrawSet drawinfo)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		if (!(drawinfo.selectionGlowColor == Color.Transparent))
		{
			Color selectionGlowColor = drawinfo.selectionGlowColor;
			List<DrawData> list = new List<DrawData>();
			list.AddRange(GetFlatColoredCloneData(ref drawinfo, new Vector2(0f, -2f), selectionGlowColor));
			list.AddRange(GetFlatColoredCloneData(ref drawinfo, new Vector2(0f, 2f), selectionGlowColor));
			list.AddRange(GetFlatColoredCloneData(ref drawinfo, new Vector2(2f, 0f), selectionGlowColor));
			list.AddRange(GetFlatColoredCloneData(ref drawinfo, new Vector2(-2f, 0f), selectionGlowColor));
			list.AddRange(drawinfo.DrawDataCache);
			drawinfo.DrawDataCache = list;
		}
	}

	public static void DrawPlayer_MakeIntoFirstFractalAfterImage(ref PlayerDrawSet drawinfo)
	{
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		if (!drawinfo.drawPlayer.isFirstFractalAfterImage)
		{
			if (drawinfo.drawPlayer.HeldItem.type == 4722)
			{
				_ = drawinfo.drawPlayer.itemAnimation;
			}
			return;
		}
		for (int i = 0; i < drawinfo.DrawDataCache.Count; i++)
		{
			DrawData value = drawinfo.DrawDataCache[i];
			ref Color color = ref value.color;
			color *= drawinfo.drawPlayer.firstFractalAfterImageOpacity;
			((Color)(ref value.color)).A = (byte)((float)(int)((Color)(ref value.color)).A * 0.8f);
			drawinfo.DrawDataCache[i] = value;
		}
	}

	public static void DrawPlayer_RenderAllLayers(ref PlayerDrawSet drawinfo)
	{
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		List<DrawData> drawDataCache = drawinfo.DrawDataCache;
		if (spriteBuffer == null)
		{
			spriteBuffer = new SpriteDrawBuffer(Main.graphics.GraphicsDevice, 200);
		}
		else
		{
			spriteBuffer.CheckGraphicsDevice(Main.graphics.GraphicsDevice);
		}
		foreach (DrawData item in drawDataCache)
		{
			if (item.texture != null)
			{
				item.Draw(spriteBuffer);
			}
		}
		spriteBuffer.UploadAndBind();
		DrawData cdd = default(DrawData);
		int num = 0;
		for (int i = 0; i <= drawDataCache.Count; i++)
		{
			if (drawinfo.projectileDrawPosition == i)
			{
				if (cdd.shader != 0)
				{
					Main.pixelShader.CurrentTechnique.Passes[0].Apply();
				}
				spriteBuffer.Unbind();
				DrawHeldProj(drawinfo, Main.projectile[drawinfo.drawPlayer.heldProj]);
				spriteBuffer.Bind();
			}
			if (i != drawDataCache.Count)
			{
				cdd = drawDataCache[i];
				if (!cdd.sourceRect.HasValue)
				{
					cdd.sourceRect = cdd.texture.Frame();
				}
				PlayerDrawHelper.SetShaderForData(drawinfo.drawPlayer, drawinfo.cHead, ref cdd);
				if (cdd.texture != null)
				{
					spriteBuffer.DrawSingle(num++);
				}
			}
		}
		spriteBuffer.Unbind();
		Main.pixelShader.CurrentTechnique.Passes[0].Apply();
	}

	private static void DrawHeldProj(PlayerDrawSet drawinfo, Projectile proj)
	{
		if (!ProjectileID.Sets.HeldProjDoesNotUsePlayerGfxOffY[proj.type])
		{
			proj.gfxOffY = drawinfo.drawPlayer.gfxOffY;
		}
		try
		{
			Main.instance.DrawProjDirect(proj);
		}
		catch
		{
			proj.active = false;
		}
	}

	public static void DrawPlayer_RenderAllLayersSlow(ref PlayerDrawSet drawinfo)
	{
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		int num = -1;
		List<DrawData> drawDataCache = drawinfo.DrawDataCache;
		Effect pixelShader = Main.pixelShader;
		Projectile[] projectile = Main.projectile;
		SpriteBatch spriteBatch = Main.spriteBatch;
		for (int i = 0; i <= drawDataCache.Count; i++)
		{
			if (drawinfo.projectileDrawPosition == i)
			{
				if (!ProjectileID.Sets.HeldProjDoesNotUsePlayerGfxOffY[projectile[drawinfo.drawPlayer.heldProj].type])
				{
					projectile[drawinfo.drawPlayer.heldProj].gfxOffY = drawinfo.drawPlayer.gfxOffY;
				}
				if (num != 0)
				{
					pixelShader.CurrentTechnique.Passes[0].Apply();
					num = 0;
				}
				try
				{
					Main.instance.DrawProj(drawinfo.drawPlayer.heldProj);
				}
				catch
				{
					projectile[drawinfo.drawPlayer.heldProj].active = false;
				}
			}
			if (i != drawDataCache.Count)
			{
				DrawData cdd = drawDataCache[i];
				if (!cdd.sourceRect.HasValue)
				{
					cdd.sourceRect = cdd.texture.Frame();
				}
				PlayerDrawHelper.SetShaderForData(drawinfo.drawPlayer, drawinfo.cHead, ref cdd);
				num = cdd.shader;
				if (cdd.texture != null)
				{
					cdd.Draw(spriteBatch);
				}
			}
		}
		pixelShader.CurrentTechnique.Passes[0].Apply();
	}

	public static void DrawPlayer_DrawSelectionRect(ref PlayerDrawSet drawinfo)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		SpriteRenderTargetHelper.GetDrawBoundary(drawinfo.DrawDataCache, out var lowest, out var highest);
		Utils.DrawRect(Main.spriteBatch, lowest + Main.screenPosition, highest + Main.screenPosition, Color.White);
	}

	private static bool IsArmorDrawnWhenInvisible(int torsoID)
	{
		if ((uint)(torsoID - 21) <= 1u)
		{
			return false;
		}
		return true;
	}

	private static DrawData[] GetFlatColoredCloneData(ref PlayerDrawSet drawinfo, Vector2 offset, Color color)
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		int colorOnlyShaderIndex = ContentSamples.CommonlyUsedContentSamples.ColorOnlyShaderIndex;
		DrawData[] array = new DrawData[drawinfo.DrawDataCache.Count];
		for (int i = 0; i < drawinfo.DrawDataCache.Count; i++)
		{
			DrawData drawData = drawinfo.DrawDataCache[i];
			ref Vector2 position = ref drawData.position;
			position += offset;
			drawData.shader = colorOnlyShaderIndex;
			drawData.color = color;
			array[i] = drawData;
		}
		return array;
	}
}
