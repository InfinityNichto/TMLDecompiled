using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.Golf;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace Terraria.DataStructures;

public struct PlayerDrawSet
{
	public List<DrawData> DrawDataCache;

	public List<int> DustCache;

	public List<int> GoreCache;

	public Player drawPlayer;

	public float shadow;

	public Vector2 Position;

	public int projectileDrawPosition;

	public Vector2 ItemLocation;

	public int armorAdjust;

	public bool armorHidesHands;

	public bool armorHidesArms;

	public bool heldProjOverHand;

	public int skinVar;

	public bool fullHair;

	public bool drawsBackHairWithoutHeadgear;

	public bool hatHair;

	public bool hideHair;

	public int hairDyePacked;

	public int skinDyePacked;

	public float mountOffSet;

	public int cHead;

	public int cBody;

	public int cLegs;

	public int cHandOn;

	public int cHandOff;

	public int cBack;

	public int cFront;

	public int cShoe;

	public int cFlameWaker;

	public int cWaist;

	public int cShield;

	public int cNeck;

	public int cFace;

	public int cBalloon;

	public int cWings;

	public int cCarpet;

	public int cPortableStool;

	public int cFloatingTube;

	public int cUnicornHorn;

	public int cAngelHalo;

	public int cBeard;

	public int cLeinShampoo;

	public int cBackpack;

	public int cTail;

	public int cFaceHead;

	public int cFaceFlower;

	public int cBalloonFront;

	public SpriteEffects playerEffect;

	public SpriteEffects itemEffect;

	public Color colorHair;

	public Color colorEyeWhites;

	public Color colorEyes;

	public Color colorHead;

	public Color colorBodySkin;

	public Color colorLegs;

	public Color colorShirt;

	public Color colorUnderShirt;

	public Color colorPants;

	public Color colorShoes;

	public Color colorArmorHead;

	public Color colorArmorBody;

	public Color colorMount;

	public Color colorArmorLegs;

	public Color colorElectricity;

	public Color colorDisplayDollSkin;

	public int headGlowMask;

	public int bodyGlowMask;

	public int armGlowMask;

	public int legsGlowMask;

	public Color headGlowColor;

	public Color bodyGlowColor;

	public Color armGlowColor;

	public Color legsGlowColor;

	public Color ArkhalisColor;

	public float stealth;

	public Vector2 legVect;

	public Vector2 bodyVect;

	public Vector2 headVect;

	public Color selectionGlowColor;

	public float torsoOffset;

	public bool hidesTopSkin;

	public bool hidesBottomSkin;

	public float rotation;

	public Vector2 rotationOrigin;

	public Rectangle hairFrontFrame;

	public Rectangle hairBackFrame;

	public bool backHairDraw;

	public Color itemColor;

	public bool usesCompositeTorso;

	public bool usesCompositeFrontHandAcc;

	public bool usesCompositeBackHandAcc;

	public bool compShoulderOverFrontArm;

	public Rectangle compBackShoulderFrame;

	public Rectangle compFrontShoulderFrame;

	public Rectangle compBackArmFrame;

	public Rectangle compFrontArmFrame;

	public Rectangle compTorsoFrame;

	public float compositeBackArmRotation;

	public float compositeFrontArmRotation;

	public bool hideCompositeShoulders;

	public Vector2 frontShoulderOffset;

	public Vector2 backShoulderOffset;

	public WeaponDrawOrder weaponDrawOrder;

	public bool weaponOverFrontArm;

	public bool isSitting;

	public bool isSleeping;

	public float seatYOffset;

	public int sittingIndex;

	public bool drawFrontAccInNeckAccLayer;

	public Item heldItem;

	public bool drawFloatingTube;

	public bool drawUnicornHorn;

	public bool drawAngelHalo;

	public Color floatingTubeColor;

	public Vector2 hairOffset;

	public Vector2 helmetOffset;

	public Vector2 legsOffset;

	public bool hideEntirePlayer;

	public bool headOnlyRender;

	public bool isBottomOverriden;

	internal bool missingHand
	{
		get
		{
			return !armorHidesHands;
		}
		set
		{
			armorHidesHands = !value;
		}
	}

	internal bool missingArm
	{
		get
		{
			return !armorHidesArms;
		}
		set
		{
			armorHidesArms = !value;
		}
	}

	public Vector2 Center => new Vector2(Position.X + (float)(drawPlayer.width / 2), Position.Y + (float)(drawPlayer.height / 2));

	public void HeadOnlySetup(Player drawPlayer2, List<DrawData> drawData, List<int> dust, List<int> gore, float X, float Y, float Alpha, float Scale)
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		//IL_016d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		//IL_017f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0197: Unknown result type (might be due to invalid IL or missing references)
		//IL_019e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0201: Unknown result type (might be due to invalid IL or missing references)
		//IL_0206: Unknown result type (might be due to invalid IL or missing references)
		//IL_0210: Unknown result type (might be due to invalid IL or missing references)
		//IL_0215: Unknown result type (might be due to invalid IL or missing references)
		//IL_0265: Unknown result type (might be due to invalid IL or missing references)
		//IL_0290: Unknown result type (might be due to invalid IL or missing references)
		//IL_0295: Unknown result type (might be due to invalid IL or missing references)
		//IL_029a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0411: Unknown result type (might be due to invalid IL or missing references)
		//IL_0416: Unknown result type (might be due to invalid IL or missing references)
		//IL_042e: Unknown result type (might be due to invalid IL or missing references)
		projectileDrawPosition = -1;
		headOnlyRender = true;
		DrawDataCache = drawData;
		DustCache = dust;
		GoreCache = gore;
		drawPlayer = drawPlayer2;
		Position = drawPlayer.position;
		CopyBasicPlayerFields();
		drawUnicornHorn = false;
		drawAngelHalo = false;
		skinVar = drawPlayer.skinVariant;
		hairDyePacked = PlayerDrawHelper.PackShader(drawPlayer.hairDye, PlayerDrawHelper.ShaderConfiguration.HairShader);
		if (drawPlayer.head == 0 && drawPlayer.hairDye == 0)
		{
			hairDyePacked = PlayerDrawHelper.PackShader(1, PlayerDrawHelper.ShaderConfiguration.HairShader);
		}
		skinDyePacked = drawPlayer.skinDyePacked;
		if (drawPlayer.face > 0 && drawPlayer.face < ArmorIDs.Face.Count)
		{
			Main.instance.LoadAccFace(drawPlayer.face);
		}
		drawUnicornHorn = drawPlayer.hasUnicornHorn;
		drawAngelHalo = drawPlayer.hasAngelHalo;
		Main.instance.LoadHair(drawPlayer.hair);
		colorEyeWhites = Main.quickAlpha(Color.White, Alpha);
		colorEyes = Main.quickAlpha(drawPlayer.eyeColor, Alpha);
		colorHair = Main.quickAlpha(drawPlayer.GetHairColor(useLighting: false), Alpha);
		colorHead = Main.quickAlpha(drawPlayer.skinColor, Alpha);
		colorArmorHead = Main.quickAlpha(Color.White, Alpha);
		if (drawPlayer.isDisplayDollOrInanimate)
		{
			colorDisplayDollSkin = Main.quickAlpha(PlayerDrawHelper.DISPLAY_DOLL_DEFAULT_SKIN_COLOR, Alpha);
		}
		else
		{
			colorDisplayDollSkin = colorHead;
		}
		playerEffect = (SpriteEffects)0;
		if (drawPlayer.direction < 0)
		{
			playerEffect = (SpriteEffects)1;
		}
		headVect = new Vector2((float)drawPlayer.legFrame.Width * 0.5f, (float)drawPlayer.legFrame.Height * 0.4f);
		Position = new Vector2(X, Y);
		Position.X -= 6f;
		Position.Y -= 4f;
		Position.Y -= drawPlayer.HeightMapOffset;
		SetupHairFrames();
		Position -= Main.OffsetsPlayerHeadgear[drawPlayer.bodyFrame.Y / drawPlayer.bodyFrame.Height];
		if (drawPlayer.head > 0 && drawPlayer.head < ArmorIDs.Head.Count)
		{
			Main.instance.LoadArmorHead(drawPlayer.head);
			int num = ArmorIDs.Head.Sets.FrontToBackID[drawPlayer.head];
			if (num >= 0)
			{
				Main.instance.LoadArmorHead(num);
			}
		}
		if (drawPlayer.face > 0 && drawPlayer.face < ArmorIDs.Face.Count)
		{
			Main.instance.LoadAccFace(drawPlayer.face);
		}
		if (drawPlayer.faceHead > 0 && drawPlayer.faceHead < ArmorIDs.Face.Count)
		{
			Main.instance.LoadAccFace(drawPlayer.faceHead);
		}
		if (drawPlayer.faceFlower > 0 && drawPlayer.faceFlower < ArmorIDs.Face.Count)
		{
			Main.instance.LoadAccFace(drawPlayer.faceFlower);
		}
		if (drawPlayer.beard > 0 && drawPlayer.beard < ArmorIDs.Beard.Count)
		{
			Main.instance.LoadAccBeard(drawPlayer.beard);
		}
		hairOffset = drawPlayer.GetHairDrawOffset(drawPlayer.hair, hatHair);
		hairOffset.Y *= drawPlayer.Directions.Y;
		helmetOffset = drawPlayer.GetHelmetDrawOffset();
		helmetOffset.Y *= drawPlayer.Directions.Y;
		drawPlayer.GetHairSettings(out fullHair, out hatHair, out hideHair, out backHairDraw, out drawsBackHairWithoutHeadgear);
	}

	public void BoringSetup(Player player, List<DrawData> drawData, List<int> dust, List<int> gore, Vector2 drawPosition, float shadowOpacity, float rotation, Vector2 rotationOrigin)
	{
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		headOnlyRender = false;
		DrawDataCache = drawData;
		DustCache = dust;
		GoreCache = gore;
		drawPlayer = player;
		shadow = shadowOpacity;
		this.rotation = rotation;
		this.rotationOrigin = rotationOrigin;
		CopyBasicPlayerFields();
		BoringSetup_2(player, drawData, dust, gore, drawPosition, shadowOpacity, rotation, rotationOrigin);
	}

	private void CopyBasicPlayerFields()
	{
		heldItem = drawPlayer.lastVisualizedSelectedItem;
		cHead = drawPlayer.cHead;
		cBody = drawPlayer.cBody;
		cLegs = drawPlayer.cLegs;
		if (drawPlayer.wearsRobe)
		{
			cLegs = cBody;
		}
		cHandOn = drawPlayer.cHandOn;
		cHandOff = drawPlayer.cHandOff;
		cBack = drawPlayer.cBack;
		cFront = drawPlayer.cFront;
		cShoe = drawPlayer.cShoe;
		cFlameWaker = drawPlayer.cFlameWaker;
		cWaist = drawPlayer.cWaist;
		cShield = drawPlayer.cShield;
		cNeck = drawPlayer.cNeck;
		cFace = drawPlayer.cFace;
		cBalloon = drawPlayer.cBalloon;
		cWings = drawPlayer.cWings;
		cCarpet = drawPlayer.cCarpet;
		cPortableStool = drawPlayer.cPortableStool;
		cFloatingTube = drawPlayer.cFloatingTube;
		cUnicornHorn = drawPlayer.cUnicornHorn;
		cAngelHalo = drawPlayer.cAngelHalo;
		cLeinShampoo = drawPlayer.cLeinShampoo;
		cBackpack = drawPlayer.cBackpack;
		cTail = drawPlayer.cTail;
		cFaceHead = drawPlayer.cFaceHead;
		cFaceFlower = drawPlayer.cFaceFlower;
		cBalloonFront = drawPlayer.cBalloonFront;
		cBeard = drawPlayer.cBeard;
		isSitting = drawPlayer.sitting.isSitting;
	}

	private void BoringSetup_2(Player player, List<DrawData> drawData, List<int> dust, List<int> gore, Vector2 drawPosition, float shadowOpacity, float rotation, Vector2 rotationOrigin)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0165: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0174: Unknown result type (might be due to invalid IL or missing references)
		//IL_0193: Unknown result type (might be due to invalid IL or missing references)
		//IL_0198: Unknown result type (might be due to invalid IL or missing references)
		//IL_019a: Unknown result type (might be due to invalid IL or missing references)
		//IL_019f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0200: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Unknown result type (might be due to invalid IL or missing references)
		//IL_020a: Unknown result type (might be due to invalid IL or missing references)
		//IL_020f: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_047a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0481: Unknown result type (might be due to invalid IL or missing references)
		//IL_0494: Unknown result type (might be due to invalid IL or missing references)
		//IL_049f: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0505: Unknown result type (might be due to invalid IL or missing references)
		//IL_050a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0515: Unknown result type (might be due to invalid IL or missing references)
		//IL_051a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0581: Unknown result type (might be due to invalid IL or missing references)
		//IL_0586: Unknown result type (might be due to invalid IL or missing references)
		//IL_0591: Unknown result type (might be due to invalid IL or missing references)
		//IL_0596: Unknown result type (might be due to invalid IL or missing references)
		//IL_05fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0602: Unknown result type (might be due to invalid IL or missing references)
		//IL_060d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0612: Unknown result type (might be due to invalid IL or missing references)
		//IL_0679: Unknown result type (might be due to invalid IL or missing references)
		//IL_067e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0689: Unknown result type (might be due to invalid IL or missing references)
		//IL_068e: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_06fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0705: Unknown result type (might be due to invalid IL or missing references)
		//IL_070a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0771: Unknown result type (might be due to invalid IL or missing references)
		//IL_0776: Unknown result type (might be due to invalid IL or missing references)
		//IL_0781: Unknown result type (might be due to invalid IL or missing references)
		//IL_0786: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_07fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0802: Unknown result type (might be due to invalid IL or missing references)
		//IL_0869: Unknown result type (might be due to invalid IL or missing references)
		//IL_086e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0879: Unknown result type (might be due to invalid IL or missing references)
		//IL_087e: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_08f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_08fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0954: Unknown result type (might be due to invalid IL or missing references)
		//IL_0959: Unknown result type (might be due to invalid IL or missing references)
		//IL_0964: Unknown result type (might be due to invalid IL or missing references)
		//IL_0969: Unknown result type (might be due to invalid IL or missing references)
		//IL_09c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_09c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_09d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_09d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_09df: Unknown result type (might be due to invalid IL or missing references)
		//IL_09e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a3e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a43: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a4e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a53: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aad: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ab2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0abd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ac2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ad9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ade: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ae5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aea: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b15: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b1a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b20: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b25: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b2b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b30: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b36: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b3b: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0404: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c28: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c2d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c71: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c76: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ca9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cae: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ce1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ce6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d0f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d14: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d4d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d52: Unknown result type (might be due to invalid IL or missing references)
		//IL_1147: Unknown result type (might be due to invalid IL or missing references)
		//IL_114c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1163: Unknown result type (might be due to invalid IL or missing references)
		//IL_1168: Unknown result type (might be due to invalid IL or missing references)
		//IL_10b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_10ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d8b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d90: Unknown result type (might be due to invalid IL or missing references)
		//IL_11a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_11b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_11b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_1108: Unknown result type (might be due to invalid IL or missing references)
		//IL_110d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1114: Unknown result type (might be due to invalid IL or missing references)
		//IL_1119: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ddc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0de3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0de8: Unknown result type (might be due to invalid IL or missing references)
		//IL_11e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_11e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_11fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_1200: Unknown result type (might be due to invalid IL or missing references)
		//IL_14b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_14be: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e1f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e24: Unknown result type (might be due to invalid IL or missing references)
		//IL_1231: Unknown result type (might be due to invalid IL or missing references)
		//IL_1236: Unknown result type (might be due to invalid IL or missing references)
		//IL_1250: Unknown result type (might be due to invalid IL or missing references)
		//IL_1255: Unknown result type (might be due to invalid IL or missing references)
		//IL_14e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_14ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_1822: Unknown result type (might be due to invalid IL or missing references)
		//IL_1829: Unknown result type (might be due to invalid IL or missing references)
		//IL_182e: Unknown result type (might be due to invalid IL or missing references)
		//IL_183b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1842: Unknown result type (might be due to invalid IL or missing references)
		//IL_1847: Unknown result type (might be due to invalid IL or missing references)
		//IL_1854: Unknown result type (might be due to invalid IL or missing references)
		//IL_185b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1860: Unknown result type (might be due to invalid IL or missing references)
		//IL_186d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1874: Unknown result type (might be due to invalid IL or missing references)
		//IL_1879: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e60: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e65: Unknown result type (might be due to invalid IL or missing references)
		//IL_1515: Unknown result type (might be due to invalid IL or missing references)
		//IL_151a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e8d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e92: Unknown result type (might be due to invalid IL or missing references)
		//IL_1553: Unknown result type (might be due to invalid IL or missing references)
		//IL_1558: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ecb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ed0: Unknown result type (might be due to invalid IL or missing references)
		//IL_13ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_13b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_13c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_13cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_12ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_12bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_12cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_12d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_12e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_12f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_12f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_15a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_15ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_15b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f0d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f12: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f29: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f34: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f39: Unknown result type (might be due to invalid IL or missing references)
		//IL_1407: Unknown result type (might be due to invalid IL or missing references)
		//IL_1412: Unknown result type (might be due to invalid IL or missing references)
		//IL_1417: Unknown result type (might be due to invalid IL or missing references)
		//IL_1350: Unknown result type (might be due to invalid IL or missing references)
		//IL_1355: Unknown result type (might be due to invalid IL or missing references)
		//IL_1362: Unknown result type (might be due to invalid IL or missing references)
		//IL_1367: Unknown result type (might be due to invalid IL or missing references)
		//IL_15ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_15f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f74: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f7f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f84: Unknown result type (might be due to invalid IL or missing references)
		//IL_162d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1632: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fb9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fbe: Unknown result type (might be due to invalid IL or missing references)
		//IL_166b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1670: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ff7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ffc: Unknown result type (might be due to invalid IL or missing references)
		//IL_1468: Unknown result type (might be due to invalid IL or missing references)
		//IL_146d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1483: Unknown result type (might be due to invalid IL or missing references)
		//IL_1488: Unknown result type (might be due to invalid IL or missing references)
		//IL_16a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_16b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_16b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_102e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1033: Unknown result type (might be due to invalid IL or missing references)
		//IL_16f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_16f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_1065: Unknown result type (might be due to invalid IL or missing references)
		//IL_106a: Unknown result type (might be due to invalid IL or missing references)
		//IL_172c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1731: Unknown result type (might be due to invalid IL or missing references)
		//IL_1748: Unknown result type (might be due to invalid IL or missing references)
		//IL_1753: Unknown result type (might be due to invalid IL or missing references)
		//IL_1758: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cd0: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cd5: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cdb: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ce0: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ce6: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ceb: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cf1: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cf6: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cfc: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d01: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d07: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d0c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1de8: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ded: Unknown result type (might be due to invalid IL or missing references)
		//IL_1df8: Unknown result type (might be due to invalid IL or missing references)
		//IL_1dfd: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d2f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d34: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d3a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d3f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d45: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d4a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d50: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d55: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d5b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d60: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d66: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d6b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d71: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d76: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d7c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d81: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d87: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d8c: Unknown result type (might be due to invalid IL or missing references)
		//IL_217d: Unknown result type (might be due to invalid IL or missing references)
		//IL_21ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_21b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_221d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2228: Unknown result type (might be due to invalid IL or missing references)
		//IL_2232: Unknown result type (might be due to invalid IL or missing references)
		//IL_2237: Unknown result type (might be due to invalid IL or missing references)
		//IL_223c: Unknown result type (might be due to invalid IL or missing references)
		//IL_22c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_22ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_22f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_2313: Unknown result type (might be due to invalid IL or missing references)
		//IL_231d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2322: Unknown result type (might be due to invalid IL or missing references)
		//IL_20ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_20f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_20f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_20fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_2104: Unknown result type (might be due to invalid IL or missing references)
		//IL_2109: Unknown result type (might be due to invalid IL or missing references)
		//IL_210f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2114: Unknown result type (might be due to invalid IL or missing references)
		//IL_211a: Unknown result type (might be due to invalid IL or missing references)
		//IL_211f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2125: Unknown result type (might be due to invalid IL or missing references)
		//IL_212a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2365: Unknown result type (might be due to invalid IL or missing references)
		//IL_2393: Unknown result type (might be due to invalid IL or missing references)
		//IL_2399: Unknown result type (might be due to invalid IL or missing references)
		//IL_240d: Unknown result type (might be due to invalid IL or missing references)
		//IL_243b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2441: Unknown result type (might be due to invalid IL or missing references)
		//IL_24db: Unknown result type (might be due to invalid IL or missing references)
		//IL_252b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2531: Unknown result type (might be due to invalid IL or missing references)
		//IL_254a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2554: Unknown result type (might be due to invalid IL or missing references)
		//IL_2559: Unknown result type (might be due to invalid IL or missing references)
		//IL_25eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_263b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2641: Unknown result type (might be due to invalid IL or missing references)
		//IL_265a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2664: Unknown result type (might be due to invalid IL or missing references)
		//IL_2669: Unknown result type (might be due to invalid IL or missing references)
		//IL_26da: Unknown result type (might be due to invalid IL or missing references)
		//IL_26df: Unknown result type (might be due to invalid IL or missing references)
		//IL_2927: Unknown result type (might be due to invalid IL or missing references)
		//IL_292c: Unknown result type (might be due to invalid IL or missing references)
		//IL_27f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_2825: Unknown result type (might be due to invalid IL or missing references)
		//IL_282b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2711: Unknown result type (might be due to invalid IL or missing references)
		//IL_2740: Unknown result type (might be due to invalid IL or missing references)
		//IL_2746: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a7b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a80: Unknown result type (might be due to invalid IL or missing references)
		//IL_295e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2987: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ad7: Unknown result type (might be due to invalid IL or missing references)
		//IL_2adc: Unknown result type (might be due to invalid IL or missing references)
		//IL_2aee: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b17: Unknown result type (might be due to invalid IL or missing references)
		//IL_2886: Unknown result type (might be due to invalid IL or missing references)
		//IL_2890: Unknown result type (might be due to invalid IL or missing references)
		//IL_2895: Unknown result type (might be due to invalid IL or missing references)
		//IL_28b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_28bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_28c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_28c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_2799: Unknown result type (might be due to invalid IL or missing references)
		//IL_27a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_27a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_27c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_27d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_27d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_27da: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e0a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e5a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e60: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e79: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e83: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e88: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d21: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d53: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d59: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d6a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d74: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d79: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d97: Unknown result type (might be due to invalid IL or missing references)
		//IL_2da2: Unknown result type (might be due to invalid IL or missing references)
		//IL_2dac: Unknown result type (might be due to invalid IL or missing references)
		//IL_2db1: Unknown result type (might be due to invalid IL or missing references)
		//IL_2bf9: Unknown result type (might be due to invalid IL or missing references)
		//IL_2bfe: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c10: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c44: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c75: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c7f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c84: Unknown result type (might be due to invalid IL or missing references)
		//IL_29da: Unknown result type (might be due to invalid IL or missing references)
		//IL_29e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_29e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a06: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a11: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a16: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a1b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b6a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b74: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b79: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b81: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b8c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b96: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b9b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ba0: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f21: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f75: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f7b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f94: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f9e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2fa3: Unknown result type (might be due to invalid IL or missing references)
		//IL_3035: Unknown result type (might be due to invalid IL or missing references)
		//IL_3089: Unknown result type (might be due to invalid IL or missing references)
		//IL_308f: Unknown result type (might be due to invalid IL or missing references)
		//IL_30a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_30b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_30b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_3149: Unknown result type (might be due to invalid IL or missing references)
		//IL_319a: Unknown result type (might be due to invalid IL or missing references)
		//IL_31a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_31b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_31c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_31c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_329d: Unknown result type (might be due to invalid IL or missing references)
		//IL_32c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_32cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_32f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_32fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_3301: Unknown result type (might be due to invalid IL or missing references)
		//IL_3550: Unknown result type (might be due to invalid IL or missing references)
		//IL_3585: Unknown result type (might be due to invalid IL or missing references)
		//IL_358a: Unknown result type (might be due to invalid IL or missing references)
		//IL_358f: Unknown result type (might be due to invalid IL or missing references)
		//IL_359e: Unknown result type (might be due to invalid IL or missing references)
		//IL_35a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_35e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_35ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_35f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_36bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_36cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_36dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_36e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_36eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_36ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_370e: Unknown result type (might be due to invalid IL or missing references)
		//IL_3715: Unknown result type (might be due to invalid IL or missing references)
		//IL_3726: Unknown result type (might be due to invalid IL or missing references)
		//IL_372c: Unknown result type (might be due to invalid IL or missing references)
		//IL_3743: Unknown result type (might be due to invalid IL or missing references)
		//IL_374d: Unknown result type (might be due to invalid IL or missing references)
		//IL_3752: Unknown result type (might be due to invalid IL or missing references)
		//IL_3391: Unknown result type (might be due to invalid IL or missing references)
		//IL_346b: Unknown result type (might be due to invalid IL or missing references)
		//IL_3497: Unknown result type (might be due to invalid IL or missing references)
		//IL_3800: Unknown result type (might be due to invalid IL or missing references)
		//IL_382b: Unknown result type (might be due to invalid IL or missing references)
		//IL_3831: Unknown result type (might be due to invalid IL or missing references)
		//IL_386c: Unknown result type (might be due to invalid IL or missing references)
		//IL_3876: Unknown result type (might be due to invalid IL or missing references)
		//IL_387b: Unknown result type (might be due to invalid IL or missing references)
		//IL_38dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_38f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_3900: Unknown result type (might be due to invalid IL or missing references)
		//IL_3905: Unknown result type (might be due to invalid IL or missing references)
		//IL_390a: Unknown result type (might be due to invalid IL or missing references)
		//IL_390f: Unknown result type (might be due to invalid IL or missing references)
		//IL_3944: Unknown result type (might be due to invalid IL or missing references)
		//IL_394e: Unknown result type (might be due to invalid IL or missing references)
		//IL_3953: Unknown result type (might be due to invalid IL or missing references)
		//IL_389f: Unknown result type (might be due to invalid IL or missing references)
		//IL_38a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_3f5a: Unknown result type (might be due to invalid IL or missing references)
		//IL_3f67: Unknown result type (might be due to invalid IL or missing references)
		//IL_3f6c: Unknown result type (might be due to invalid IL or missing references)
		//IL_3f73: Unknown result type (might be due to invalid IL or missing references)
		//IL_3f80: Unknown result type (might be due to invalid IL or missing references)
		//IL_3f85: Unknown result type (might be due to invalid IL or missing references)
		//IL_3f8c: Unknown result type (might be due to invalid IL or missing references)
		//IL_3f99: Unknown result type (might be due to invalid IL or missing references)
		//IL_3f9e: Unknown result type (might be due to invalid IL or missing references)
		//IL_3fa5: Unknown result type (might be due to invalid IL or missing references)
		//IL_3fb2: Unknown result type (might be due to invalid IL or missing references)
		//IL_3fb7: Unknown result type (might be due to invalid IL or missing references)
		//IL_3fbe: Unknown result type (might be due to invalid IL or missing references)
		//IL_3fcb: Unknown result type (might be due to invalid IL or missing references)
		//IL_3fd0: Unknown result type (might be due to invalid IL or missing references)
		//IL_3fd7: Unknown result type (might be due to invalid IL or missing references)
		//IL_3fe4: Unknown result type (might be due to invalid IL or missing references)
		//IL_3fe9: Unknown result type (might be due to invalid IL or missing references)
		//IL_3ff0: Unknown result type (might be due to invalid IL or missing references)
		//IL_3ffd: Unknown result type (might be due to invalid IL or missing references)
		//IL_4002: Unknown result type (might be due to invalid IL or missing references)
		//IL_4009: Unknown result type (might be due to invalid IL or missing references)
		//IL_4016: Unknown result type (might be due to invalid IL or missing references)
		//IL_401b: Unknown result type (might be due to invalid IL or missing references)
		//IL_4022: Unknown result type (might be due to invalid IL or missing references)
		//IL_402f: Unknown result type (might be due to invalid IL or missing references)
		//IL_4034: Unknown result type (might be due to invalid IL or missing references)
		//IL_403b: Unknown result type (might be due to invalid IL or missing references)
		//IL_4048: Unknown result type (might be due to invalid IL or missing references)
		//IL_404d: Unknown result type (might be due to invalid IL or missing references)
		//IL_4054: Unknown result type (might be due to invalid IL or missing references)
		//IL_4061: Unknown result type (might be due to invalid IL or missing references)
		//IL_4066: Unknown result type (might be due to invalid IL or missing references)
		//IL_406d: Unknown result type (might be due to invalid IL or missing references)
		//IL_407a: Unknown result type (might be due to invalid IL or missing references)
		//IL_407f: Unknown result type (might be due to invalid IL or missing references)
		//IL_4086: Unknown result type (might be due to invalid IL or missing references)
		//IL_4093: Unknown result type (might be due to invalid IL or missing references)
		//IL_4098: Unknown result type (might be due to invalid IL or missing references)
		//IL_3d8b: Unknown result type (might be due to invalid IL or missing references)
		//IL_3d96: Unknown result type (might be due to invalid IL or missing references)
		//IL_3d9b: Unknown result type (might be due to invalid IL or missing references)
		//IL_3dad: Unknown result type (might be due to invalid IL or missing references)
		//IL_3db8: Unknown result type (might be due to invalid IL or missing references)
		//IL_3dbd: Unknown result type (might be due to invalid IL or missing references)
		//IL_3dd0: Unknown result type (might be due to invalid IL or missing references)
		//IL_3ddb: Unknown result type (might be due to invalid IL or missing references)
		//IL_3de0: Unknown result type (might be due to invalid IL or missing references)
		//IL_3df2: Unknown result type (might be due to invalid IL or missing references)
		//IL_3dfd: Unknown result type (might be due to invalid IL or missing references)
		//IL_3e02: Unknown result type (might be due to invalid IL or missing references)
		//IL_3e14: Unknown result type (might be due to invalid IL or missing references)
		//IL_3e1f: Unknown result type (might be due to invalid IL or missing references)
		//IL_3e24: Unknown result type (might be due to invalid IL or missing references)
		//IL_3e36: Unknown result type (might be due to invalid IL or missing references)
		//IL_3e41: Unknown result type (might be due to invalid IL or missing references)
		//IL_3e46: Unknown result type (might be due to invalid IL or missing references)
		//IL_3e58: Unknown result type (might be due to invalid IL or missing references)
		//IL_3e63: Unknown result type (might be due to invalid IL or missing references)
		//IL_3e68: Unknown result type (might be due to invalid IL or missing references)
		//IL_3e7a: Unknown result type (might be due to invalid IL or missing references)
		//IL_3e85: Unknown result type (might be due to invalid IL or missing references)
		//IL_3e8a: Unknown result type (might be due to invalid IL or missing references)
		//IL_3e9c: Unknown result type (might be due to invalid IL or missing references)
		//IL_3ea7: Unknown result type (might be due to invalid IL or missing references)
		//IL_3eac: Unknown result type (might be due to invalid IL or missing references)
		//IL_3ebe: Unknown result type (might be due to invalid IL or missing references)
		//IL_3ec9: Unknown result type (might be due to invalid IL or missing references)
		//IL_3ece: Unknown result type (might be due to invalid IL or missing references)
		//IL_3eda: Unknown result type (might be due to invalid IL or missing references)
		//IL_3ee5: Unknown result type (might be due to invalid IL or missing references)
		//IL_3eea: Unknown result type (might be due to invalid IL or missing references)
		//IL_3ef6: Unknown result type (might be due to invalid IL or missing references)
		//IL_3f01: Unknown result type (might be due to invalid IL or missing references)
		//IL_3f06: Unknown result type (might be due to invalid IL or missing references)
		//IL_3f12: Unknown result type (might be due to invalid IL or missing references)
		//IL_3f1d: Unknown result type (might be due to invalid IL or missing references)
		//IL_3f22: Unknown result type (might be due to invalid IL or missing references)
		//IL_40ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_40b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_40bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_3f3e: Unknown result type (might be due to invalid IL or missing references)
		//IL_3f49: Unknown result type (might be due to invalid IL or missing references)
		//IL_3f4e: Unknown result type (might be due to invalid IL or missing references)
		//IL_40d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_40d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_40de: Unknown result type (might be due to invalid IL or missing references)
		//IL_40e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_40e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_40ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_40f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_40f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_40ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_4104: Unknown result type (might be due to invalid IL or missing references)
		//IL_410a: Unknown result type (might be due to invalid IL or missing references)
		//IL_410f: Unknown result type (might be due to invalid IL or missing references)
		//IL_4115: Unknown result type (might be due to invalid IL or missing references)
		//IL_411a: Unknown result type (might be due to invalid IL or missing references)
		//IL_4120: Unknown result type (might be due to invalid IL or missing references)
		//IL_4125: Unknown result type (might be due to invalid IL or missing references)
		//IL_412b: Unknown result type (might be due to invalid IL or missing references)
		//IL_4130: Unknown result type (might be due to invalid IL or missing references)
		//IL_4136: Unknown result type (might be due to invalid IL or missing references)
		//IL_413b: Unknown result type (might be due to invalid IL or missing references)
		//IL_3c48: Unknown result type (might be due to invalid IL or missing references)
		//IL_3c7a: Unknown result type (might be due to invalid IL or missing references)
		//IL_3c80: Unknown result type (might be due to invalid IL or missing references)
		//IL_3c91: Unknown result type (might be due to invalid IL or missing references)
		//IL_3c9b: Unknown result type (might be due to invalid IL or missing references)
		//IL_3ca0: Unknown result type (might be due to invalid IL or missing references)
		//IL_3cbe: Unknown result type (might be due to invalid IL or missing references)
		//IL_3cc9: Unknown result type (might be due to invalid IL or missing references)
		//IL_3cd3: Unknown result type (might be due to invalid IL or missing references)
		//IL_3cd8: Unknown result type (might be due to invalid IL or missing references)
		//IL_41d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_41dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_41e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_41ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_41f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_41fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_4216: Unknown result type (might be due to invalid IL or missing references)
		//IL_421b: Unknown result type (might be due to invalid IL or missing references)
		//IL_4220: Unknown result type (might be due to invalid IL or missing references)
		//IL_422c: Unknown result type (might be due to invalid IL or missing references)
		//IL_4233: Unknown result type (might be due to invalid IL or missing references)
		//IL_4238: Unknown result type (might be due to invalid IL or missing references)
		//IL_4244: Unknown result type (might be due to invalid IL or missing references)
		//IL_424b: Unknown result type (might be due to invalid IL or missing references)
		//IL_4250: Unknown result type (might be due to invalid IL or missing references)
		//IL_425c: Unknown result type (might be due to invalid IL or missing references)
		//IL_4263: Unknown result type (might be due to invalid IL or missing references)
		//IL_4268: Unknown result type (might be due to invalid IL or missing references)
		//IL_4274: Unknown result type (might be due to invalid IL or missing references)
		//IL_427b: Unknown result type (might be due to invalid IL or missing references)
		//IL_4280: Unknown result type (might be due to invalid IL or missing references)
		//IL_428c: Unknown result type (might be due to invalid IL or missing references)
		//IL_4293: Unknown result type (might be due to invalid IL or missing references)
		//IL_4298: Unknown result type (might be due to invalid IL or missing references)
		//IL_42a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_42ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_42b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_42bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_42c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_42c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_42ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_42d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_42d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_42de: Unknown result type (might be due to invalid IL or missing references)
		//IL_42e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_42e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_3abc: Unknown result type (might be due to invalid IL or missing references)
		//IL_3ae7: Unknown result type (might be due to invalid IL or missing references)
		//IL_3aed: Unknown result type (might be due to invalid IL or missing references)
		//IL_3b05: Unknown result type (might be due to invalid IL or missing references)
		//IL_3b0a: Unknown result type (might be due to invalid IL or missing references)
		//IL_3b17: Unknown result type (might be due to invalid IL or missing references)
		//IL_3b1c: Unknown result type (might be due to invalid IL or missing references)
		//IL_3b2c: Unknown result type (might be due to invalid IL or missing references)
		//IL_3b31: Unknown result type (might be due to invalid IL or missing references)
		//IL_3b45: Unknown result type (might be due to invalid IL or missing references)
		//IL_3b4c: Unknown result type (might be due to invalid IL or missing references)
		//IL_3b51: Unknown result type (might be due to invalid IL or missing references)
		//IL_3b56: Unknown result type (might be due to invalid IL or missing references)
		//IL_434e: Unknown result type (might be due to invalid IL or missing references)
		//IL_4355: Unknown result type (might be due to invalid IL or missing references)
		//IL_435a: Unknown result type (might be due to invalid IL or missing references)
		//IL_4366: Unknown result type (might be due to invalid IL or missing references)
		//IL_436d: Unknown result type (might be due to invalid IL or missing references)
		//IL_4372: Unknown result type (might be due to invalid IL or missing references)
		//IL_438e: Unknown result type (might be due to invalid IL or missing references)
		//IL_4393: Unknown result type (might be due to invalid IL or missing references)
		//IL_439a: Unknown result type (might be due to invalid IL or missing references)
		//IL_439f: Unknown result type (might be due to invalid IL or missing references)
		//IL_43ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_43b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_43b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_43c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_43ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_43cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_43db: Unknown result type (might be due to invalid IL or missing references)
		//IL_43e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_43e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_43f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_43fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_43ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_440b: Unknown result type (might be due to invalid IL or missing references)
		//IL_4412: Unknown result type (might be due to invalid IL or missing references)
		//IL_4417: Unknown result type (might be due to invalid IL or missing references)
		//IL_4423: Unknown result type (might be due to invalid IL or missing references)
		//IL_442a: Unknown result type (might be due to invalid IL or missing references)
		//IL_442f: Unknown result type (might be due to invalid IL or missing references)
		//IL_443b: Unknown result type (might be due to invalid IL or missing references)
		//IL_4442: Unknown result type (might be due to invalid IL or missing references)
		//IL_4447: Unknown result type (might be due to invalid IL or missing references)
		//IL_4453: Unknown result type (might be due to invalid IL or missing references)
		//IL_445e: Unknown result type (might be due to invalid IL or missing references)
		//IL_4463: Unknown result type (might be due to invalid IL or missing references)
		//IL_446f: Unknown result type (might be due to invalid IL or missing references)
		//IL_447a: Unknown result type (might be due to invalid IL or missing references)
		//IL_447f: Unknown result type (might be due to invalid IL or missing references)
		//IL_448b: Unknown result type (might be due to invalid IL or missing references)
		//IL_4496: Unknown result type (might be due to invalid IL or missing references)
		//IL_449b: Unknown result type (might be due to invalid IL or missing references)
		//IL_42fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_4303: Unknown result type (might be due to invalid IL or missing references)
		//IL_4308: Unknown result type (might be due to invalid IL or missing references)
		//IL_44ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_44b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_44ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_41b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_41b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_4c8b: Unknown result type (might be due to invalid IL or missing references)
		//IL_4c90: Unknown result type (might be due to invalid IL or missing references)
		//IL_4c91: Unknown result type (might be due to invalid IL or missing references)
		//IL_4c93: Unknown result type (might be due to invalid IL or missing references)
		//IL_4c98: Unknown result type (might be due to invalid IL or missing references)
		//IL_4c9a: Unknown result type (might be due to invalid IL or missing references)
		//IL_4c9b: Unknown result type (might be due to invalid IL or missing references)
		//IL_4c9d: Unknown result type (might be due to invalid IL or missing references)
		//IL_4ca2: Unknown result type (might be due to invalid IL or missing references)
		//IL_4ca4: Unknown result type (might be due to invalid IL or missing references)
		//IL_4ca5: Unknown result type (might be due to invalid IL or missing references)
		//IL_4ca7: Unknown result type (might be due to invalid IL or missing references)
		//IL_4cac: Unknown result type (might be due to invalid IL or missing references)
		//IL_4cae: Unknown result type (might be due to invalid IL or missing references)
		//IL_4caf: Unknown result type (might be due to invalid IL or missing references)
		//IL_4cb1: Unknown result type (might be due to invalid IL or missing references)
		//IL_4cb6: Unknown result type (might be due to invalid IL or missing references)
		//IL_4cb8: Unknown result type (might be due to invalid IL or missing references)
		//IL_4cb9: Unknown result type (might be due to invalid IL or missing references)
		//IL_4cbb: Unknown result type (might be due to invalid IL or missing references)
		//IL_4cc0: Unknown result type (might be due to invalid IL or missing references)
		//IL_4cc2: Unknown result type (might be due to invalid IL or missing references)
		//IL_4cc3: Unknown result type (might be due to invalid IL or missing references)
		//IL_4cc5: Unknown result type (might be due to invalid IL or missing references)
		//IL_4cca: Unknown result type (might be due to invalid IL or missing references)
		//IL_4ccc: Unknown result type (might be due to invalid IL or missing references)
		//IL_4ccd: Unknown result type (might be due to invalid IL or missing references)
		//IL_4ccf: Unknown result type (might be due to invalid IL or missing references)
		//IL_4cd4: Unknown result type (might be due to invalid IL or missing references)
		//IL_4cd6: Unknown result type (might be due to invalid IL or missing references)
		//IL_4cd7: Unknown result type (might be due to invalid IL or missing references)
		//IL_4cd9: Unknown result type (might be due to invalid IL or missing references)
		//IL_4cde: Unknown result type (might be due to invalid IL or missing references)
		//IL_4ce0: Unknown result type (might be due to invalid IL or missing references)
		//IL_4ce1: Unknown result type (might be due to invalid IL or missing references)
		//IL_4ce3: Unknown result type (might be due to invalid IL or missing references)
		//IL_4ce8: Unknown result type (might be due to invalid IL or missing references)
		//IL_4cea: Unknown result type (might be due to invalid IL or missing references)
		//IL_4ceb: Unknown result type (might be due to invalid IL or missing references)
		//IL_4ced: Unknown result type (might be due to invalid IL or missing references)
		//IL_4cf2: Unknown result type (might be due to invalid IL or missing references)
		//IL_4cf4: Unknown result type (might be due to invalid IL or missing references)
		//IL_4cf5: Unknown result type (might be due to invalid IL or missing references)
		//IL_4cf7: Unknown result type (might be due to invalid IL or missing references)
		//IL_4cfc: Unknown result type (might be due to invalid IL or missing references)
		//IL_4cfe: Unknown result type (might be due to invalid IL or missing references)
		//IL_4cff: Unknown result type (might be due to invalid IL or missing references)
		//IL_4d01: Unknown result type (might be due to invalid IL or missing references)
		//IL_4d06: Unknown result type (might be due to invalid IL or missing references)
		//IL_4d08: Unknown result type (might be due to invalid IL or missing references)
		//IL_4d09: Unknown result type (might be due to invalid IL or missing references)
		//IL_4d0b: Unknown result type (might be due to invalid IL or missing references)
		//IL_4d10: Unknown result type (might be due to invalid IL or missing references)
		//IL_4d12: Unknown result type (might be due to invalid IL or missing references)
		//IL_4d13: Unknown result type (might be due to invalid IL or missing references)
		//IL_4d15: Unknown result type (might be due to invalid IL or missing references)
		//IL_4d1a: Unknown result type (might be due to invalid IL or missing references)
		//IL_4d1c: Unknown result type (might be due to invalid IL or missing references)
		//IL_4d1d: Unknown result type (might be due to invalid IL or missing references)
		//IL_4d1f: Unknown result type (might be due to invalid IL or missing references)
		//IL_4d24: Unknown result type (might be due to invalid IL or missing references)
		//IL_4d26: Unknown result type (might be due to invalid IL or missing references)
		//IL_4d27: Unknown result type (might be due to invalid IL or missing references)
		//IL_4d29: Unknown result type (might be due to invalid IL or missing references)
		//IL_4d2e: Unknown result type (might be due to invalid IL or missing references)
		//IL_4d30: Unknown result type (might be due to invalid IL or missing references)
		//IL_4d31: Unknown result type (might be due to invalid IL or missing references)
		//IL_4d33: Unknown result type (might be due to invalid IL or missing references)
		//IL_4d38: Unknown result type (might be due to invalid IL or missing references)
		//IL_4d3a: Unknown result type (might be due to invalid IL or missing references)
		//IL_459b: Unknown result type (might be due to invalid IL or missing references)
		//IL_45a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_45e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_45eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_4631: Unknown result type (might be due to invalid IL or missing references)
		//IL_4636: Unknown result type (might be due to invalid IL or missing references)
		//IL_4644: Unknown result type (might be due to invalid IL or missing references)
		//IL_464b: Unknown result type (might be due to invalid IL or missing references)
		//IL_4650: Unknown result type (might be due to invalid IL or missing references)
		//IL_4657: Unknown result type (might be due to invalid IL or missing references)
		//IL_465e: Unknown result type (might be due to invalid IL or missing references)
		//IL_4663: Unknown result type (might be due to invalid IL or missing references)
		//IL_466a: Unknown result type (might be due to invalid IL or missing references)
		//IL_4671: Unknown result type (might be due to invalid IL or missing references)
		//IL_4676: Unknown result type (might be due to invalid IL or missing references)
		//IL_467d: Unknown result type (might be due to invalid IL or missing references)
		//IL_4684: Unknown result type (might be due to invalid IL or missing references)
		//IL_4689: Unknown result type (might be due to invalid IL or missing references)
		//IL_4690: Unknown result type (might be due to invalid IL or missing references)
		//IL_4697: Unknown result type (might be due to invalid IL or missing references)
		//IL_469c: Unknown result type (might be due to invalid IL or missing references)
		//IL_46a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_46aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_46af: Unknown result type (might be due to invalid IL or missing references)
		//IL_46b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_46bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_46c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_46c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_46d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_46d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_46dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_46e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_46e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_46ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_46f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_46fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_4702: Unknown result type (might be due to invalid IL or missing references)
		//IL_4709: Unknown result type (might be due to invalid IL or missing references)
		//IL_470e: Unknown result type (might be due to invalid IL or missing references)
		//IL_4715: Unknown result type (might be due to invalid IL or missing references)
		//IL_471c: Unknown result type (might be due to invalid IL or missing references)
		//IL_4721: Unknown result type (might be due to invalid IL or missing references)
		//IL_4728: Unknown result type (might be due to invalid IL or missing references)
		//IL_472f: Unknown result type (might be due to invalid IL or missing references)
		//IL_4734: Unknown result type (might be due to invalid IL or missing references)
		//IL_473b: Unknown result type (might be due to invalid IL or missing references)
		//IL_4742: Unknown result type (might be due to invalid IL or missing references)
		//IL_4747: Unknown result type (might be due to invalid IL or missing references)
		//IL_474e: Unknown result type (might be due to invalid IL or missing references)
		//IL_4755: Unknown result type (might be due to invalid IL or missing references)
		//IL_475a: Unknown result type (might be due to invalid IL or missing references)
		//IL_4853: Unknown result type (might be due to invalid IL or missing references)
		//IL_4858: Unknown result type (might be due to invalid IL or missing references)
		//IL_489e: Unknown result type (might be due to invalid IL or missing references)
		//IL_48a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_48e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_48ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_48fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_4903: Unknown result type (might be due to invalid IL or missing references)
		//IL_4908: Unknown result type (might be due to invalid IL or missing references)
		//IL_490f: Unknown result type (might be due to invalid IL or missing references)
		//IL_4916: Unknown result type (might be due to invalid IL or missing references)
		//IL_491b: Unknown result type (might be due to invalid IL or missing references)
		//IL_4922: Unknown result type (might be due to invalid IL or missing references)
		//IL_4929: Unknown result type (might be due to invalid IL or missing references)
		//IL_492e: Unknown result type (might be due to invalid IL or missing references)
		//IL_4935: Unknown result type (might be due to invalid IL or missing references)
		//IL_493c: Unknown result type (might be due to invalid IL or missing references)
		//IL_4941: Unknown result type (might be due to invalid IL or missing references)
		//IL_4948: Unknown result type (might be due to invalid IL or missing references)
		//IL_494f: Unknown result type (might be due to invalid IL or missing references)
		//IL_4954: Unknown result type (might be due to invalid IL or missing references)
		//IL_495b: Unknown result type (might be due to invalid IL or missing references)
		//IL_4962: Unknown result type (might be due to invalid IL or missing references)
		//IL_4967: Unknown result type (might be due to invalid IL or missing references)
		//IL_496e: Unknown result type (might be due to invalid IL or missing references)
		//IL_4975: Unknown result type (might be due to invalid IL or missing references)
		//IL_497a: Unknown result type (might be due to invalid IL or missing references)
		//IL_4981: Unknown result type (might be due to invalid IL or missing references)
		//IL_4988: Unknown result type (might be due to invalid IL or missing references)
		//IL_498d: Unknown result type (might be due to invalid IL or missing references)
		//IL_4994: Unknown result type (might be due to invalid IL or missing references)
		//IL_499b: Unknown result type (might be due to invalid IL or missing references)
		//IL_49a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_49a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_49ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_49b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_49ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_49c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_49c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_49cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_49d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_49d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_49e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_49e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_49ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_49f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_49fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_49ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_4a06: Unknown result type (might be due to invalid IL or missing references)
		//IL_4a0d: Unknown result type (might be due to invalid IL or missing references)
		//IL_4a12: Unknown result type (might be due to invalid IL or missing references)
		//IL_4771: Unknown result type (might be due to invalid IL or missing references)
		//IL_4778: Unknown result type (might be due to invalid IL or missing references)
		//IL_477d: Unknown result type (might be due to invalid IL or missing references)
		//IL_4dee: Unknown result type (might be due to invalid IL or missing references)
		//IL_4df5: Unknown result type (might be due to invalid IL or missing references)
		//IL_4dde: Unknown result type (might be due to invalid IL or missing references)
		//IL_4de5: Unknown result type (might be due to invalid IL or missing references)
		//IL_4d71: Unknown result type (might be due to invalid IL or missing references)
		//IL_4d78: Unknown result type (might be due to invalid IL or missing references)
		//IL_4d61: Unknown result type (might be due to invalid IL or missing references)
		//IL_4d68: Unknown result type (might be due to invalid IL or missing references)
		//IL_4ab6: Unknown result type (might be due to invalid IL or missing references)
		//IL_4acf: Unknown result type (might be due to invalid IL or missing references)
		//IL_4adc: Unknown result type (might be due to invalid IL or missing references)
		//IL_4ae8: Unknown result type (might be due to invalid IL or missing references)
		//IL_4aed: Unknown result type (might be due to invalid IL or missing references)
		//IL_4aef: Unknown result type (might be due to invalid IL or missing references)
		//IL_4af4: Unknown result type (might be due to invalid IL or missing references)
		//IL_4afb: Unknown result type (might be due to invalid IL or missing references)
		//IL_4b00: Unknown result type (might be due to invalid IL or missing references)
		//IL_4b02: Unknown result type (might be due to invalid IL or missing references)
		//IL_4b07: Unknown result type (might be due to invalid IL or missing references)
		//IL_4b0e: Unknown result type (might be due to invalid IL or missing references)
		//IL_4b13: Unknown result type (might be due to invalid IL or missing references)
		//IL_4b15: Unknown result type (might be due to invalid IL or missing references)
		//IL_4b1a: Unknown result type (might be due to invalid IL or missing references)
		//IL_4b28: Unknown result type (might be due to invalid IL or missing references)
		//IL_4b2f: Unknown result type (might be due to invalid IL or missing references)
		//IL_4b34: Unknown result type (might be due to invalid IL or missing references)
		//IL_4b3b: Unknown result type (might be due to invalid IL or missing references)
		//IL_4b42: Unknown result type (might be due to invalid IL or missing references)
		//IL_4b47: Unknown result type (might be due to invalid IL or missing references)
		//IL_4b4e: Unknown result type (might be due to invalid IL or missing references)
		//IL_4b55: Unknown result type (might be due to invalid IL or missing references)
		//IL_4b5a: Unknown result type (might be due to invalid IL or missing references)
		//IL_4b61: Unknown result type (might be due to invalid IL or missing references)
		//IL_4b68: Unknown result type (might be due to invalid IL or missing references)
		//IL_4b6d: Unknown result type (might be due to invalid IL or missing references)
		//IL_4b74: Unknown result type (might be due to invalid IL or missing references)
		//IL_4b7b: Unknown result type (might be due to invalid IL or missing references)
		//IL_4b80: Unknown result type (might be due to invalid IL or missing references)
		//IL_4b87: Unknown result type (might be due to invalid IL or missing references)
		//IL_4b8e: Unknown result type (might be due to invalid IL or missing references)
		//IL_4b93: Unknown result type (might be due to invalid IL or missing references)
		//IL_4b9a: Unknown result type (might be due to invalid IL or missing references)
		//IL_4ba1: Unknown result type (might be due to invalid IL or missing references)
		//IL_4ba6: Unknown result type (might be due to invalid IL or missing references)
		//IL_4bad: Unknown result type (might be due to invalid IL or missing references)
		//IL_4bb4: Unknown result type (might be due to invalid IL or missing references)
		//IL_4bb9: Unknown result type (might be due to invalid IL or missing references)
		//IL_4bc0: Unknown result type (might be due to invalid IL or missing references)
		//IL_4bc7: Unknown result type (might be due to invalid IL or missing references)
		//IL_4bcc: Unknown result type (might be due to invalid IL or missing references)
		//IL_4bd3: Unknown result type (might be due to invalid IL or missing references)
		//IL_4bda: Unknown result type (might be due to invalid IL or missing references)
		//IL_4bdf: Unknown result type (might be due to invalid IL or missing references)
		//IL_4be6: Unknown result type (might be due to invalid IL or missing references)
		//IL_4bed: Unknown result type (might be due to invalid IL or missing references)
		//IL_4bf2: Unknown result type (might be due to invalid IL or missing references)
		//IL_4bf9: Unknown result type (might be due to invalid IL or missing references)
		//IL_4c00: Unknown result type (might be due to invalid IL or missing references)
		//IL_4c05: Unknown result type (might be due to invalid IL or missing references)
		//IL_4c0c: Unknown result type (might be due to invalid IL or missing references)
		//IL_4c13: Unknown result type (might be due to invalid IL or missing references)
		//IL_4c18: Unknown result type (might be due to invalid IL or missing references)
		//IL_4c1f: Unknown result type (might be due to invalid IL or missing references)
		//IL_4c26: Unknown result type (might be due to invalid IL or missing references)
		//IL_4c2b: Unknown result type (might be due to invalid IL or missing references)
		//IL_4c32: Unknown result type (might be due to invalid IL or missing references)
		//IL_4c39: Unknown result type (might be due to invalid IL or missing references)
		//IL_4c3e: Unknown result type (might be due to invalid IL or missing references)
		//IL_4a29: Unknown result type (might be due to invalid IL or missing references)
		//IL_4a30: Unknown result type (might be due to invalid IL or missing references)
		//IL_4a35: Unknown result type (might be due to invalid IL or missing references)
		//IL_4c52: Unknown result type (might be due to invalid IL or missing references)
		//IL_4c59: Unknown result type (might be due to invalid IL or missing references)
		//IL_4c5e: Unknown result type (might be due to invalid IL or missing references)
		//IL_4e88: Unknown result type (might be due to invalid IL or missing references)
		//IL_4e8e: Unknown result type (might be due to invalid IL or missing references)
		//IL_4e8f: Unknown result type (might be due to invalid IL or missing references)
		//IL_4ee0: Unknown result type (might be due to invalid IL or missing references)
		//IL_4ee5: Unknown result type (might be due to invalid IL or missing references)
		//IL_4f19: Unknown result type (might be due to invalid IL or missing references)
		//IL_4f1e: Unknown result type (might be due to invalid IL or missing references)
		//IL_4f52: Unknown result type (might be due to invalid IL or missing references)
		//IL_4f57: Unknown result type (might be due to invalid IL or missing references)
		//IL_4ea5: Unknown result type (might be due to invalid IL or missing references)
		//IL_4eab: Unknown result type (might be due to invalid IL or missing references)
		//IL_4eac: Unknown result type (might be due to invalid IL or missing references)
		seatYOffset = 0f;
		sittingIndex = 0;
		Vector2 posOffset = Vector2.Zero;
		drawPlayer.sitting.GetSittingOffsetInfo(drawPlayer, out posOffset, out seatYOffset);
		if (isSitting)
		{
			sittingIndex = drawPlayer.sitting.sittingIndex;
		}
		if (drawPlayer.mount.Active && drawPlayer.mount.Type == 17)
		{
			isSitting = true;
		}
		if (drawPlayer.mount.Active && drawPlayer.mount.Type == 23)
		{
			isSitting = true;
		}
		if (drawPlayer.mount.Active && drawPlayer.mount.Type == 45)
		{
			isSitting = true;
		}
		isSleeping = drawPlayer.sleeping.isSleeping;
		Position = drawPosition;
		Position += new Vector2(drawPlayer.MountXOffset * (float)drawPlayer.direction, 0f);
		if (isSitting)
		{
			torsoOffset = seatYOffset;
			Position += posOffset;
		}
		else
		{
			sittingIndex = -1;
		}
		if (isSleeping)
		{
			this.rotationOrigin = player.Size / 2f;
			drawPlayer.sleeping.GetSleepingOffsetInfo(drawPlayer, out var posOffset2);
			Position += posOffset2;
		}
		weaponDrawOrder = WeaponDrawOrder.BehindFrontArm;
		if (heldItem.type == 4952)
		{
			weaponDrawOrder = WeaponDrawOrder.BehindBackArm;
		}
		if (GolfHelper.IsPlayerHoldingClub(player) && player.itemAnimation >= player.itemAnimationMax)
		{
			weaponDrawOrder = WeaponDrawOrder.OverFrontArm;
		}
		projectileDrawPosition = -1;
		ItemLocation = Position + (drawPlayer.itemLocation - drawPlayer.position);
		armorAdjust = 0;
		heldProjOverHand = false;
		skinVar = drawPlayer.skinVariant;
		armorHidesHands = drawPlayer.body > 0 && ArmorIDs.Body.Sets.HidesHands[drawPlayer.body];
		armorHidesArms = drawPlayer.body > 0 && ArmorIDs.Body.Sets.HidesArms[drawPlayer.body];
		if (drawPlayer.heldProj >= 0 && shadow == 0f)
		{
			int type = Main.projectile[drawPlayer.heldProj].type;
			if (type == 460 || type == 535 || type == 600)
			{
				heldProjOverHand = true;
			}
			ProjectileLoader.DrawHeldProjInFrontOfHeldItemAndArms(Main.projectile[drawPlayer.heldProj], ref heldProjOverHand);
		}
		drawPlayer.GetHairSettings(out fullHair, out hatHair, out hideHair, out backHairDraw, out drawsBackHairWithoutHeadgear);
		hairDyePacked = PlayerDrawHelper.PackShader(drawPlayer.hairDye, PlayerDrawHelper.ShaderConfiguration.HairShader);
		if (drawPlayer.head == 0 && drawPlayer.hairDye == 0)
		{
			hairDyePacked = PlayerDrawHelper.PackShader(1, PlayerDrawHelper.ShaderConfiguration.HairShader);
		}
		skinDyePacked = player.skinDyePacked;
		if (drawPlayer.mount.Active && drawPlayer.mount.Type == 52)
		{
			AdjustmentsForWolfMount();
		}
		if (drawPlayer.isDisplayDollOrInanimate)
		{
			Point point = Center.ToTileCoordinates();
			if (Main.InSmartCursorHighlightArea(point.X, point.Y, out var actuallySelected))
			{
				Color color = Lighting.GetColor(point.X, point.Y);
				int num = (((Color)(ref color)).R + ((Color)(ref color)).G + ((Color)(ref color)).B) / 3;
				if (num > 10)
				{
					selectionGlowColor = Colors.GetSelectionGlowColor(actuallySelected, num);
				}
			}
		}
		mountOffSet = drawPlayer.HeightOffsetVisual;
		Position.Y -= mountOffSet;
		if (drawPlayer.mount.Active)
		{
			Mount.currentShader = (drawPlayer.mount.Cart ? drawPlayer.cMinecart : drawPlayer.cMount);
		}
		else
		{
			Mount.currentShader = 0;
		}
		playerEffect = (SpriteEffects)0;
		itemEffect = (SpriteEffects)1;
		colorHair = drawPlayer.GetImmuneAlpha(drawPlayer.GetHairColor(), shadow);
		colorEyeWhites = drawPlayer.GetImmuneAlpha(Lighting.GetColorClamped((int)((double)Position.X + (double)drawPlayer.width * 0.5) / 16, (int)(((double)Position.Y + (double)drawPlayer.height * 0.25) / 16.0), Color.White), shadow);
		colorEyes = drawPlayer.GetImmuneAlpha(Lighting.GetColorClamped((int)((double)Position.X + (double)drawPlayer.width * 0.5) / 16, (int)(((double)Position.Y + (double)drawPlayer.height * 0.25) / 16.0), drawPlayer.eyeColor), shadow);
		colorHead = drawPlayer.GetImmuneAlpha(Lighting.GetColorClamped((int)((double)Position.X + (double)drawPlayer.width * 0.5) / 16, (int)(((double)Position.Y + (double)drawPlayer.height * 0.25) / 16.0), drawPlayer.skinColor), shadow);
		colorBodySkin = drawPlayer.GetImmuneAlpha(Lighting.GetColorClamped((int)((double)Position.X + (double)drawPlayer.width * 0.5) / 16, (int)(((double)Position.Y + (double)drawPlayer.height * 0.5) / 16.0), drawPlayer.skinColor), shadow);
		colorLegs = drawPlayer.GetImmuneAlpha(Lighting.GetColorClamped((int)((double)Position.X + (double)drawPlayer.width * 0.5) / 16, (int)(((double)Position.Y + (double)drawPlayer.height * 0.75) / 16.0), drawPlayer.skinColor), shadow);
		colorShirt = drawPlayer.GetImmuneAlphaPure(Lighting.GetColorClamped((int)((double)Position.X + (double)drawPlayer.width * 0.5) / 16, (int)(((double)Position.Y + (double)drawPlayer.height * 0.5) / 16.0), drawPlayer.shirtColor), shadow);
		colorUnderShirt = drawPlayer.GetImmuneAlphaPure(Lighting.GetColorClamped((int)((double)Position.X + (double)drawPlayer.width * 0.5) / 16, (int)(((double)Position.Y + (double)drawPlayer.height * 0.5) / 16.0), drawPlayer.underShirtColor), shadow);
		colorPants = drawPlayer.GetImmuneAlphaPure(Lighting.GetColorClamped((int)((double)Position.X + (double)drawPlayer.width * 0.5) / 16, (int)(((double)Position.Y + (double)drawPlayer.height * 0.75) / 16.0), drawPlayer.pantsColor), shadow);
		colorShoes = drawPlayer.GetImmuneAlphaPure(Lighting.GetColorClamped((int)((double)Position.X + (double)drawPlayer.width * 0.5) / 16, (int)(((double)Position.Y + (double)drawPlayer.height * 0.75) / 16.0), drawPlayer.shoeColor), shadow);
		colorArmorHead = drawPlayer.GetImmuneAlphaPure(Lighting.GetColorClamped((int)((double)Position.X + (double)drawPlayer.width * 0.5) / 16, (int)((double)Position.Y + (double)drawPlayer.height * 0.25) / 16, Color.White), shadow);
		colorArmorBody = drawPlayer.GetImmuneAlphaPure(Lighting.GetColorClamped((int)((double)Position.X + (double)drawPlayer.width * 0.5) / 16, (int)((double)Position.Y + (double)drawPlayer.height * 0.5) / 16, Color.White), shadow);
		colorMount = colorArmorBody;
		colorArmorLegs = drawPlayer.GetImmuneAlphaPure(Lighting.GetColorClamped((int)((double)Position.X + (double)drawPlayer.width * 0.5) / 16, (int)((double)Position.Y + (double)drawPlayer.height * 0.75) / 16, Color.White), shadow);
		floatingTubeColor = drawPlayer.GetImmuneAlphaPure(Lighting.GetColorClamped((int)((double)Position.X + (double)drawPlayer.width * 0.5) / 16, (int)((double)Position.Y + (double)drawPlayer.height * 0.75) / 16, Color.White), shadow);
		colorElectricity = new Color(255, 255, 255, 100);
		colorDisplayDollSkin = colorBodySkin;
		int num12 = 0;
		int num22 = 0;
		int num23 = 0;
		int num24 = 0;
		headGlowMask = -1;
		bodyGlowMask = -1;
		armGlowMask = -1;
		legsGlowMask = -1;
		headGlowColor = Color.Transparent;
		bodyGlowColor = Color.Transparent;
		armGlowColor = Color.Transparent;
		legsGlowColor = Color.Transparent;
		switch (drawPlayer.head)
		{
		case 169:
			num12++;
			break;
		case 170:
			num22++;
			break;
		case 171:
			num23++;
			break;
		case 189:
			num24++;
			break;
		}
		switch (drawPlayer.body)
		{
		case 175:
			num12++;
			break;
		case 176:
			num22++;
			break;
		case 177:
			num23++;
			break;
		case 190:
			num24++;
			break;
		}
		switch (drawPlayer.legs)
		{
		case 110:
			num12++;
			break;
		case 111:
			num22++;
			break;
		case 112:
			num23++;
			break;
		case 130:
			num24++;
			break;
		}
		num12 = 3;
		num22 = 3;
		num23 = 3;
		num24 = 3;
		ArkhalisColor = drawPlayer.underShirtColor;
		((Color)(ref ArkhalisColor)).A = 180;
		if (drawPlayer.head == 169)
		{
			headGlowMask = 15;
			byte b = (byte)(62.5f * (float)(1 + num12));
			headGlowColor = new Color((int)b, (int)b, (int)b, 0);
		}
		else if (drawPlayer.head == 216)
		{
			headGlowMask = 256;
			byte b7 = 127;
			headGlowColor = new Color((int)b7, (int)b7, (int)b7, 0);
		}
		else if (drawPlayer.head == 210)
		{
			headGlowMask = 242;
			byte b8 = 127;
			headGlowColor = new Color((int)b8, (int)b8, (int)b8, 0);
		}
		else if (drawPlayer.head == 214)
		{
			headGlowMask = 245;
			headGlowColor = ArkhalisColor;
		}
		else if (drawPlayer.head == 240)
		{
			headGlowMask = 273;
			headGlowColor = new Color(230, 230, 230, 60);
		}
		else if (drawPlayer.head == 267)
		{
			headGlowMask = 301;
			headGlowColor = new Color(230, 230, 230, 60);
		}
		else if (drawPlayer.head == 268)
		{
			headGlowMask = 302;
			float num25 = (float)(int)Main.mouseTextColor / 255f;
			num25 *= num25;
			headGlowColor = new Color(255, 255, 255) * num25;
		}
		else if (drawPlayer.head == 269)
		{
			headGlowMask = 304;
			headGlowColor = new Color(200, 200, 200);
		}
		else if (drawPlayer.head == 270)
		{
			headGlowMask = 305;
			headGlowColor = new Color(200, 200, 200, 150);
		}
		else if (drawPlayer.head == 271)
		{
			headGlowMask = 309;
			headGlowColor = Color.White;
		}
		else if (drawPlayer.head == 170)
		{
			headGlowMask = 16;
			byte b9 = (byte)(62.5f * (float)(1 + num22));
			headGlowColor = new Color((int)b9, (int)b9, (int)b9, 0);
		}
		else if (drawPlayer.head == 189)
		{
			headGlowMask = 184;
			byte b10 = (byte)(62.5f * (float)(1 + num24));
			headGlowColor = new Color((int)b10, (int)b10, (int)b10, 0);
			colorArmorHead = drawPlayer.GetImmuneAlphaPure(new Color((int)b10, (int)b10, (int)b10, 255), shadow);
		}
		else if (drawPlayer.head == 171)
		{
			byte b11 = (byte)(62.5f * (float)(1 + num23));
			colorArmorHead = drawPlayer.GetImmuneAlphaPure(new Color((int)b11, (int)b11, (int)b11, 255), shadow);
		}
		else if (drawPlayer.head == 175)
		{
			headGlowMask = 41;
			headGlowColor = new Color(255, 255, 255, 0);
		}
		else if (drawPlayer.head == 193)
		{
			headGlowMask = 209;
			headGlowColor = new Color(255, 255, 255, 127);
		}
		else if (drawPlayer.head == 109)
		{
			headGlowMask = 208;
			headGlowColor = new Color(255, 255, 255, 0);
		}
		else if (drawPlayer.head == 178)
		{
			headGlowMask = 96;
			headGlowColor = new Color(255, 255, 255, 0);
		}
		if (drawPlayer.body == 175)
		{
			if (drawPlayer.Male)
			{
				bodyGlowMask = 13;
			}
			else
			{
				bodyGlowMask = 18;
			}
			byte b12 = (byte)(62.5f * (float)(1 + num12));
			bodyGlowColor = new Color((int)b12, (int)b12, (int)b12, 0);
		}
		else if (drawPlayer.body == 208)
		{
			if (drawPlayer.Male)
			{
				bodyGlowMask = 246;
			}
			else
			{
				bodyGlowMask = 247;
			}
			armGlowMask = 248;
			bodyGlowColor = ArkhalisColor;
			armGlowColor = ArkhalisColor;
		}
		else if (drawPlayer.body == 227)
		{
			bodyGlowColor = new Color(230, 230, 230, 60);
			armGlowColor = new Color(230, 230, 230, 60);
		}
		else if (drawPlayer.body == 237)
		{
			float num26 = (float)(int)Main.mouseTextColor / 255f;
			num26 *= num26;
			bodyGlowColor = new Color(255, 255, 255) * num26;
		}
		else if (drawPlayer.body == 238)
		{
			bodyGlowColor = new Color(255, 255, 255);
			armGlowColor = new Color(255, 255, 255);
		}
		else if (drawPlayer.body == 239)
		{
			bodyGlowColor = new Color(200, 200, 200, 150);
			armGlowColor = new Color(200, 200, 200, 150);
		}
		else if (drawPlayer.body == 190)
		{
			if (drawPlayer.Male)
			{
				bodyGlowMask = 185;
			}
			else
			{
				bodyGlowMask = 186;
			}
			armGlowMask = 188;
			byte b13 = (byte)(62.5f * (float)(1 + num24));
			bodyGlowColor = new Color((int)b13, (int)b13, (int)b13, 0);
			armGlowColor = new Color((int)b13, (int)b13, (int)b13, 0);
			colorArmorBody = drawPlayer.GetImmuneAlphaPure(new Color((int)b13, (int)b13, (int)b13, 255), shadow);
		}
		else if (drawPlayer.body == 176)
		{
			if (drawPlayer.Male)
			{
				bodyGlowMask = 14;
			}
			else
			{
				bodyGlowMask = 19;
			}
			armGlowMask = 12;
			byte b14 = (byte)(62.5f * (float)(1 + num22));
			bodyGlowColor = new Color((int)b14, (int)b14, (int)b14, 0);
			armGlowColor = new Color((int)b14, (int)b14, (int)b14, 0);
		}
		else if (drawPlayer.body == 194)
		{
			bodyGlowMask = 210;
			armGlowMask = 211;
			bodyGlowColor = new Color(255, 255, 255, 127);
			armGlowColor = new Color(255, 255, 255, 127);
		}
		else if (drawPlayer.body == 177)
		{
			byte b2 = (byte)(62.5f * (float)(1 + num23));
			colorArmorBody = drawPlayer.GetImmuneAlphaPure(new Color((int)b2, (int)b2, (int)b2, 255), shadow);
		}
		else if (drawPlayer.body == 179)
		{
			if (drawPlayer.Male)
			{
				bodyGlowMask = 42;
			}
			else
			{
				bodyGlowMask = 43;
			}
			armGlowMask = 44;
			bodyGlowColor = new Color(255, 255, 255, 0);
			armGlowColor = new Color(255, 255, 255, 0);
		}
		if (drawPlayer.legs == 111)
		{
			legsGlowMask = 17;
			byte b3 = (byte)(62.5f * (float)(1 + num22));
			legsGlowColor = new Color((int)b3, (int)b3, (int)b3, 0);
		}
		else if (drawPlayer.legs == 157)
		{
			legsGlowMask = 249;
			legsGlowColor = ArkhalisColor;
		}
		else if (drawPlayer.legs == 158)
		{
			legsGlowMask = 250;
			legsGlowColor = ArkhalisColor;
		}
		else if (drawPlayer.legs == 210)
		{
			legsGlowMask = 274;
			legsGlowColor = new Color(230, 230, 230, 60);
		}
		else if (drawPlayer.legs == 222)
		{
			legsGlowMask = 303;
			float num27 = (float)(int)Main.mouseTextColor / 255f;
			num27 *= num27;
			legsGlowColor = new Color(255, 255, 255) * num27;
		}
		else if (drawPlayer.legs == 225)
		{
			legsGlowMask = 306;
			legsGlowColor = new Color(200, 200, 200, 150);
		}
		else if (drawPlayer.legs == 226)
		{
			legsGlowMask = 307;
			legsGlowColor = new Color(200, 200, 200, 150);
		}
		else if (drawPlayer.legs == 110)
		{
			legsGlowMask = 199;
			byte b4 = (byte)(62.5f * (float)(1 + num12));
			legsGlowColor = new Color((int)b4, (int)b4, (int)b4, 0);
		}
		else if (drawPlayer.legs == 112)
		{
			byte b5 = (byte)(62.5f * (float)(1 + num23));
			colorArmorLegs = drawPlayer.GetImmuneAlphaPure(new Color((int)b5, (int)b5, (int)b5, 255), shadow);
		}
		else if (drawPlayer.legs == 134)
		{
			legsGlowMask = 212;
			legsGlowColor = new Color(255, 255, 255, 127);
		}
		else if (drawPlayer.legs == 130)
		{
			byte b6 = (byte)(127 * (1 + num24));
			legsGlowMask = 187;
			legsGlowColor = new Color((int)b6, (int)b6, (int)b6, 0);
			colorArmorLegs = drawPlayer.GetImmuneAlphaPure(new Color((int)b6, (int)b6, (int)b6, 255), shadow);
		}
		ItemLoader.DrawArmorColor(EquipType.Head, drawPlayer.head, drawPlayer, shadow, ref colorArmorHead, ref headGlowMask, ref headGlowColor);
		ItemLoader.DrawArmorColor(EquipType.Body, drawPlayer.body, drawPlayer, shadow, ref colorArmorBody, ref bodyGlowMask, ref bodyGlowColor);
		ItemLoader.ArmorArmGlowMask(drawPlayer.body, drawPlayer, shadow, ref armGlowMask, ref armGlowColor);
		ItemLoader.DrawArmorColor(EquipType.Legs, drawPlayer.legs, drawPlayer, shadow, ref colorArmorLegs, ref legsGlowMask, ref legsGlowColor);
		float alphaReduction = shadow;
		headGlowColor = drawPlayer.GetImmuneAlphaPure(headGlowColor, alphaReduction);
		bodyGlowColor = drawPlayer.GetImmuneAlphaPure(bodyGlowColor, alphaReduction);
		armGlowColor = drawPlayer.GetImmuneAlphaPure(armGlowColor, alphaReduction);
		legsGlowColor = drawPlayer.GetImmuneAlphaPure(legsGlowColor, alphaReduction);
		if (drawPlayer.head > 0 && drawPlayer.head < ArmorIDs.Head.Count)
		{
			Main.instance.LoadArmorHead(drawPlayer.head);
			int num28 = ArmorIDs.Head.Sets.FrontToBackID[drawPlayer.head];
			if (num28 >= 0)
			{
				Main.instance.LoadArmorHead(num28);
			}
		}
		if (drawPlayer.body > 0 && drawPlayer.body < ArmorIDs.Body.Count)
		{
			Main.instance.LoadArmorBody(drawPlayer.body);
		}
		if (drawPlayer.legs > 0 && drawPlayer.legs < ArmorIDs.Legs.Count)
		{
			Main.instance.LoadArmorLegs(drawPlayer.legs);
		}
		if (drawPlayer.handon > 0 && drawPlayer.handon < ArmorIDs.HandOn.Count)
		{
			Main.instance.LoadAccHandsOn(drawPlayer.handon);
		}
		if (drawPlayer.handoff > 0 && drawPlayer.handoff < ArmorIDs.HandOff.Count)
		{
			Main.instance.LoadAccHandsOff(drawPlayer.handoff);
		}
		if (drawPlayer.back > 0 && drawPlayer.back < ArmorIDs.Back.Count)
		{
			Main.instance.LoadAccBack(drawPlayer.back);
		}
		if (drawPlayer.front > 0 && drawPlayer.front < ArmorIDs.Front.Count)
		{
			Main.instance.LoadAccFront(drawPlayer.front);
		}
		if (drawPlayer.shoe > 0 && drawPlayer.shoe < ArmorIDs.Shoe.Count)
		{
			Main.instance.LoadAccShoes(drawPlayer.shoe);
		}
		if (drawPlayer.waist > 0 && drawPlayer.waist < ArmorIDs.Waist.Count)
		{
			Main.instance.LoadAccWaist(drawPlayer.waist);
		}
		if (drawPlayer.shield > 0 && drawPlayer.shield < ArmorIDs.Shield.Count)
		{
			Main.instance.LoadAccShield(drawPlayer.shield);
		}
		if (drawPlayer.neck > 0 && drawPlayer.neck < ArmorIDs.Neck.Count)
		{
			Main.instance.LoadAccNeck(drawPlayer.neck);
		}
		if (drawPlayer.face > 0 && drawPlayer.face < ArmorIDs.Face.Count)
		{
			Main.instance.LoadAccFace(drawPlayer.face);
		}
		if (drawPlayer.balloon > 0 && drawPlayer.balloon < ArmorIDs.Balloon.Count)
		{
			Main.instance.LoadAccBalloon(drawPlayer.balloon);
		}
		if (drawPlayer.backpack > 0 && drawPlayer.backpack < ArmorIDs.Back.Count)
		{
			Main.instance.LoadAccBack(drawPlayer.backpack);
		}
		if (drawPlayer.tail > 0 && drawPlayer.tail < ArmorIDs.Back.Count)
		{
			Main.instance.LoadAccBack(drawPlayer.tail);
		}
		if (drawPlayer.faceHead > 0 && drawPlayer.faceHead < ArmorIDs.Face.Count)
		{
			Main.instance.LoadAccFace(drawPlayer.faceHead);
		}
		if (drawPlayer.faceFlower > 0 && drawPlayer.faceFlower < ArmorIDs.Face.Count)
		{
			Main.instance.LoadAccFace(drawPlayer.faceFlower);
		}
		if (drawPlayer.balloonFront > 0 && drawPlayer.balloonFront < ArmorIDs.Balloon.Count)
		{
			Main.instance.LoadAccBalloon(drawPlayer.balloonFront);
		}
		if (drawPlayer.beard > 0 && drawPlayer.beard < ArmorIDs.Beard.Count)
		{
			Main.instance.LoadAccBeard(drawPlayer.beard);
		}
		Main.instance.LoadHair(drawPlayer.hair);
		if (drawPlayer.eyebrellaCloud)
		{
			Main.instance.LoadProjectile(238);
		}
		if (drawPlayer.isHatRackDoll)
		{
			colorLegs = Color.Transparent;
			colorBodySkin = Color.Transparent;
			colorHead = Color.Transparent;
			colorHair = Color.Transparent;
			colorEyes = Color.Transparent;
			colorEyeWhites = Color.Transparent;
		}
		if (drawPlayer.isDisplayDollOrInanimate)
		{
			if (drawPlayer.isFullbright)
			{
				colorHead = Color.White;
				colorBodySkin = Color.White;
				colorLegs = Color.White;
				colorEyes = Color.White;
				colorEyeWhites = Color.White;
				colorArmorHead = Color.White;
				colorArmorBody = Color.White;
				colorArmorLegs = Color.White;
				colorDisplayDollSkin = PlayerDrawHelper.DISPLAY_DOLL_DEFAULT_SKIN_COLOR;
			}
			else
			{
				colorDisplayDollSkin = drawPlayer.GetImmuneAlphaPure(Lighting.GetColorClamped((int)((double)Position.X + (double)drawPlayer.width * 0.5) / 16, (int)((double)Position.Y + (double)drawPlayer.height * 0.5) / 16, PlayerDrawHelper.DISPLAY_DOLL_DEFAULT_SKIN_COLOR), shadow);
			}
		}
		if (!drawPlayer.isDisplayDollOrInanimate)
		{
			if ((drawPlayer.head == 78 || drawPlayer.head == 79 || drawPlayer.head == 80) && drawPlayer.body == 51 && drawPlayer.legs == 47)
			{
				float num2 = (float)(int)Main.mouseTextColor / 200f - 0.3f;
				if (shadow != 0f)
				{
					num2 = 0f;
				}
				((Color)(ref colorArmorHead)).R = (byte)((float)(int)((Color)(ref colorArmorHead)).R * num2);
				((Color)(ref colorArmorHead)).G = (byte)((float)(int)((Color)(ref colorArmorHead)).G * num2);
				((Color)(ref colorArmorHead)).B = (byte)((float)(int)((Color)(ref colorArmorHead)).B * num2);
				((Color)(ref colorArmorBody)).R = (byte)((float)(int)((Color)(ref colorArmorBody)).R * num2);
				((Color)(ref colorArmorBody)).G = (byte)((float)(int)((Color)(ref colorArmorBody)).G * num2);
				((Color)(ref colorArmorBody)).B = (byte)((float)(int)((Color)(ref colorArmorBody)).B * num2);
				((Color)(ref colorArmorLegs)).R = (byte)((float)(int)((Color)(ref colorArmorLegs)).R * num2);
				((Color)(ref colorArmorLegs)).G = (byte)((float)(int)((Color)(ref colorArmorLegs)).G * num2);
				((Color)(ref colorArmorLegs)).B = (byte)((float)(int)((Color)(ref colorArmorLegs)).B * num2);
			}
			if (drawPlayer.head == 193 && drawPlayer.body == 194 && drawPlayer.legs == 134)
			{
				float num3 = 0.6f - drawPlayer.ghostFade * 0.3f;
				if (shadow != 0f)
				{
					num3 = 0f;
				}
				((Color)(ref colorArmorHead)).R = (byte)((float)(int)((Color)(ref colorArmorHead)).R * num3);
				((Color)(ref colorArmorHead)).G = (byte)((float)(int)((Color)(ref colorArmorHead)).G * num3);
				((Color)(ref colorArmorHead)).B = (byte)((float)(int)((Color)(ref colorArmorHead)).B * num3);
				((Color)(ref colorArmorBody)).R = (byte)((float)(int)((Color)(ref colorArmorBody)).R * num3);
				((Color)(ref colorArmorBody)).G = (byte)((float)(int)((Color)(ref colorArmorBody)).G * num3);
				((Color)(ref colorArmorBody)).B = (byte)((float)(int)((Color)(ref colorArmorBody)).B * num3);
				((Color)(ref colorArmorLegs)).R = (byte)((float)(int)((Color)(ref colorArmorLegs)).R * num3);
				((Color)(ref colorArmorLegs)).G = (byte)((float)(int)((Color)(ref colorArmorLegs)).G * num3);
				((Color)(ref colorArmorLegs)).B = (byte)((float)(int)((Color)(ref colorArmorLegs)).B * num3);
			}
			if (shadow > 0f)
			{
				colorLegs = Color.Transparent;
				colorBodySkin = Color.Transparent;
				colorHead = Color.Transparent;
				colorHair = Color.Transparent;
				colorEyes = Color.Transparent;
				colorEyeWhites = Color.Transparent;
			}
		}
		float num4 = 1f;
		float num5 = 1f;
		float num6 = 1f;
		float num7 = 1f;
		if (drawPlayer.honey && Main.rand.Next(30) == 0 && shadow == 0f)
		{
			Dust dust12 = Dust.NewDustDirect(Position, drawPlayer.width, drawPlayer.height, 152, 0f, 0f, 150);
			dust12.velocity.Y = 0.3f;
			dust12.velocity.X *= 0.1f;
			dust12.scale += (float)Main.rand.Next(3, 4) * 0.1f;
			dust12.alpha = 100;
			dust12.noGravity = true;
			dust12.velocity += drawPlayer.velocity * 0.1f;
			DustCache.Add(dust12.dustIndex);
		}
		if (drawPlayer.dryadWard && drawPlayer.velocity.X != 0f && Main.rand.Next(4) == 0)
		{
			Dust dust15 = Dust.NewDustDirect(new Vector2(drawPlayer.position.X - 2f, drawPlayer.position.Y + (float)drawPlayer.height - 2f), drawPlayer.width + 4, 4, 163, 0f, 0f, 100, default(Color), 1.5f);
			dust15.noGravity = true;
			dust15.noLight = true;
			dust15.velocity *= 0f;
			DustCache.Add(dust15.dustIndex);
		}
		if (drawPlayer.poisoned)
		{
			if (Main.rand.Next(50) == 0 && shadow == 0f)
			{
				Dust dust16 = Dust.NewDustDirect(Position, drawPlayer.width, drawPlayer.height, 46, 0f, 0f, 150, default(Color), 0.2f);
				dust16.noGravity = true;
				dust16.fadeIn = 1.9f;
				DustCache.Add(dust16.dustIndex);
			}
			num4 *= 0.65f;
			num6 *= 0.75f;
		}
		if (drawPlayer.venom)
		{
			if (Main.rand.Next(10) == 0 && shadow == 0f)
			{
				Dust dust17 = Dust.NewDustDirect(Position, drawPlayer.width, drawPlayer.height, 171, 0f, 0f, 100, default(Color), 0.5f);
				dust17.noGravity = true;
				dust17.fadeIn = 1.5f;
				DustCache.Add(dust17.dustIndex);
			}
			num5 *= 0.45f;
			num4 *= 0.75f;
		}
		if (drawPlayer.onFire)
		{
			if (Main.rand.Next(4) == 0 && shadow == 0f)
			{
				Dust dust18 = Dust.NewDustDirect(new Vector2(Position.X - 2f, Position.Y - 2f), drawPlayer.width + 4, drawPlayer.height + 4, 6, drawPlayer.velocity.X * 0.4f, drawPlayer.velocity.Y * 0.4f, 100, default(Color), 3f);
				dust18.noGravity = true;
				dust18.velocity *= 1.8f;
				dust18.velocity.Y -= 0.5f;
				DustCache.Add(dust18.dustIndex);
			}
			num6 *= 0.6f;
			num5 *= 0.7f;
		}
		if (drawPlayer.onFire3)
		{
			if (Main.rand.Next(4) == 0 && shadow == 0f)
			{
				Dust dust19 = Dust.NewDustDirect(new Vector2(Position.X - 2f, Position.Y - 2f), drawPlayer.width + 4, drawPlayer.height + 4, 6, drawPlayer.velocity.X * 0.4f, drawPlayer.velocity.Y * 0.4f, 100, default(Color), 3f);
				dust19.noGravity = true;
				dust19.velocity *= 1.8f;
				dust19.velocity.Y -= 0.5f;
				DustCache.Add(dust19.dustIndex);
			}
			num6 *= 0.6f;
			num5 *= 0.7f;
		}
		if (drawPlayer.dripping && shadow == 0f && Main.rand.Next(4) != 0)
		{
			Vector2 position = Position;
			position.X -= 2f;
			position.Y -= 2f;
			if (Main.rand.Next(2) == 0)
			{
				Dust dust20 = Dust.NewDustDirect(position, drawPlayer.width + 4, drawPlayer.height + 2, 211, 0f, 0f, 50, default(Color), 0.8f);
				if (Main.rand.Next(2) == 0)
				{
					dust20.alpha += 25;
				}
				if (Main.rand.Next(2) == 0)
				{
					dust20.alpha += 25;
				}
				dust20.noLight = true;
				dust20.velocity *= 0.2f;
				dust20.velocity.Y += 0.2f;
				dust20.velocity += drawPlayer.velocity;
				DustCache.Add(dust20.dustIndex);
			}
			else
			{
				Dust dust21 = Dust.NewDustDirect(position, drawPlayer.width + 8, drawPlayer.height + 8, 211, 0f, 0f, 50, default(Color), 1.1f);
				if (Main.rand.Next(2) == 0)
				{
					dust21.alpha += 25;
				}
				if (Main.rand.Next(2) == 0)
				{
					dust21.alpha += 25;
				}
				dust21.noLight = true;
				dust21.noGravity = true;
				dust21.velocity *= 0.2f;
				dust21.velocity.Y += 1f;
				dust21.velocity += drawPlayer.velocity;
				DustCache.Add(dust21.dustIndex);
			}
		}
		if (drawPlayer.drippingSlime)
		{
			int alpha = 175;
			Color newColor = default(Color);
			((Color)(ref newColor))._002Ector(0, 80, 255, 100);
			if (Main.rand.Next(4) != 0 && shadow == 0f)
			{
				Vector2 position2 = Position;
				position2.X -= 2f;
				position2.Y -= 2f;
				if (Main.rand.Next(2) == 0)
				{
					Dust dust2 = Dust.NewDustDirect(position2, drawPlayer.width + 4, drawPlayer.height + 2, 4, 0f, 0f, alpha, newColor, 1.4f);
					if (Main.rand.Next(2) == 0)
					{
						dust2.alpha += 25;
					}
					if (Main.rand.Next(2) == 0)
					{
						dust2.alpha += 25;
					}
					dust2.noLight = true;
					dust2.velocity *= 0.2f;
					dust2.velocity.Y += 0.2f;
					dust2.velocity += drawPlayer.velocity;
					DustCache.Add(dust2.dustIndex);
				}
			}
			num4 *= 0.8f;
			num5 *= 0.8f;
		}
		if (drawPlayer.drippingSparkleSlime)
		{
			int alpha2 = 100;
			if (Main.rand.Next(4) != 0 && shadow == 0f)
			{
				Vector2 position3 = Position;
				position3.X -= 2f;
				position3.Y -= 2f;
				if (Main.rand.Next(4) == 0)
				{
					Color newColor2 = Main.hslToRgb(0.7f + 0.2f * Main.rand.NextFloat(), 1f, 0.5f);
					((Color)(ref newColor2)).A = (byte)(((Color)(ref newColor2)).A / 2);
					Dust dust3 = Dust.NewDustDirect(position3, drawPlayer.width + 4, drawPlayer.height + 2, 4, 0f, 0f, alpha2, newColor2, 0.65f);
					if (Main.rand.Next(2) == 0)
					{
						dust3.alpha += 25;
					}
					if (Main.rand.Next(2) == 0)
					{
						dust3.alpha += 25;
					}
					dust3.noLight = true;
					dust3.velocity *= 0.2f;
					dust3.velocity += drawPlayer.velocity * 0.7f;
					dust3.fadeIn = 0.8f;
					DustCache.Add(dust3.dustIndex);
				}
				if (Main.rand.Next(30) == 0)
				{
					Color color2 = Main.hslToRgb(0.7f + 0.2f * Main.rand.NextFloat(), 1f, 0.5f);
					((Color)(ref color2)).A = (byte)(((Color)(ref color2)).A / 2);
					Dust dust4 = Dust.NewDustDirect(position3, drawPlayer.width + 4, drawPlayer.height + 2, 43, 0f, 0f, 254, new Color(127, 127, 127, 0), 0.45f);
					dust4.noLight = true;
					dust4.velocity.X *= 0f;
					dust4.velocity *= 0.03f;
					dust4.fadeIn = 0.6f;
					DustCache.Add(dust4.dustIndex);
				}
			}
			num4 *= 0.94f;
			num5 *= 0.82f;
		}
		if (drawPlayer.ichor)
		{
			num6 = 0f;
		}
		if (drawPlayer.electrified && shadow == 0f && Main.rand.Next(3) == 0)
		{
			Dust dust5 = Dust.NewDustDirect(new Vector2(Position.X - 2f, Position.Y - 2f), drawPlayer.width + 4, drawPlayer.height + 4, 226, 0f, 0f, 100, default(Color), 0.5f);
			dust5.velocity *= 1.6f;
			dust5.velocity.Y -= 1f;
			dust5.position = Vector2.Lerp(dust5.position, drawPlayer.Center, 0.5f);
			DustCache.Add(dust5.dustIndex);
		}
		if (drawPlayer.burned)
		{
			if (shadow == 0f)
			{
				Dust dust6 = Dust.NewDustDirect(new Vector2(Position.X - 2f, Position.Y - 2f), drawPlayer.width + 4, drawPlayer.height + 4, 6, drawPlayer.velocity.X * 0.4f, drawPlayer.velocity.Y * 0.4f, 100, default(Color), 2f);
				dust6.noGravity = true;
				dust6.velocity *= 1.8f;
				dust6.velocity.Y -= 0.75f;
				DustCache.Add(dust6.dustIndex);
			}
			num4 = 1f;
			num6 *= 0.6f;
			num5 *= 0.7f;
		}
		if (drawPlayer.onFrostBurn)
		{
			if (Main.rand.Next(4) == 0 && shadow == 0f)
			{
				Dust dust7 = Dust.NewDustDirect(new Vector2(Position.X - 2f, Position.Y - 2f), drawPlayer.width + 4, drawPlayer.height + 4, 135, drawPlayer.velocity.X * 0.4f, drawPlayer.velocity.Y * 0.4f, 100, default(Color), 3f);
				dust7.noGravity = true;
				dust7.velocity *= 1.8f;
				dust7.velocity.Y -= 0.5f;
				DustCache.Add(dust7.dustIndex);
			}
			num4 *= 0.5f;
			num5 *= 0.7f;
		}
		if (drawPlayer.onFrostBurn2)
		{
			if (Main.rand.Next(4) == 0 && shadow == 0f)
			{
				Dust dust8 = Dust.NewDustDirect(new Vector2(Position.X - 2f, Position.Y - 2f), drawPlayer.width + 4, drawPlayer.height + 4, 135, drawPlayer.velocity.X * 0.4f, drawPlayer.velocity.Y * 0.4f, 100, default(Color), 3f);
				dust8.noGravity = true;
				dust8.velocity *= 1.8f;
				dust8.velocity.Y -= 0.5f;
				DustCache.Add(dust8.dustIndex);
			}
			num4 *= 0.5f;
			num5 *= 0.7f;
		}
		if (drawPlayer.onFire2)
		{
			if (Main.rand.Next(4) == 0 && shadow == 0f)
			{
				Dust dust9 = Dust.NewDustDirect(new Vector2(Position.X - 2f, Position.Y - 2f), drawPlayer.width + 4, drawPlayer.height + 4, 75, drawPlayer.velocity.X * 0.4f, drawPlayer.velocity.Y * 0.4f, 100, default(Color), 3f);
				dust9.noGravity = true;
				dust9.velocity *= 1.8f;
				dust9.velocity.Y -= 0.5f;
				DustCache.Add(dust9.dustIndex);
			}
			num6 *= 0.6f;
			num5 *= 0.7f;
		}
		if (drawPlayer.noItems)
		{
			num5 *= 0.8f;
			num4 *= 0.65f;
		}
		if (drawPlayer.blind)
		{
			num5 *= 0.65f;
			num4 *= 0.7f;
		}
		if (drawPlayer.bleed)
		{
			num5 *= 0.9f;
			num6 *= 0.9f;
			if (!drawPlayer.dead && Main.rand.Next(30) == 0 && shadow == 0f)
			{
				Dust dust10 = Dust.NewDustDirect(Position, drawPlayer.width, drawPlayer.height, 5);
				dust10.velocity.Y += 0.5f;
				dust10.velocity *= 0.25f;
				DustCache.Add(dust10.dustIndex);
			}
		}
		if (shadow == 0f && drawPlayer.palladiumRegen && drawPlayer.statLife < drawPlayer.statLifeMax2 && ((Game)Main.instance).IsActive && !Main.gamePaused && drawPlayer.miscCounter % 10 == 0 && shadow == 0f)
		{
			Vector2 position4 = default(Vector2);
			position4.X = Position.X + (float)Main.rand.Next(drawPlayer.width);
			position4.Y = Position.Y + (float)Main.rand.Next(drawPlayer.height);
			position4.X = Position.X + (float)(drawPlayer.width / 2) - 6f;
			position4.Y = Position.Y + (float)(drawPlayer.height / 2) - 6f;
			position4.X -= Main.rand.Next(-10, 11);
			position4.Y -= Main.rand.Next(-20, 21);
			int item = Gore.NewGore(position4, new Vector2((float)Main.rand.Next(-10, 11) * 0.1f, (float)Main.rand.Next(-20, -10) * 0.1f), 331, (float)Main.rand.Next(80, 120) * 0.01f);
			GoreCache.Add(item);
		}
		if (shadow == 0f && drawPlayer.loveStruck && ((Game)Main.instance).IsActive && !Main.gamePaused && Main.rand.Next(5) == 0)
		{
			Vector2 vector = default(Vector2);
			((Vector2)(ref vector))._002Ector((float)Main.rand.Next(-10, 11), (float)Main.rand.Next(-10, 11));
			((Vector2)(ref vector)).Normalize();
			vector.X *= 0.66f;
			int num8 = Gore.NewGore(Position + new Vector2((float)Main.rand.Next(drawPlayer.width + 1), (float)Main.rand.Next(drawPlayer.height + 1)), vector * (float)Main.rand.Next(3, 6) * 0.33f, 331, (float)Main.rand.Next(40, 121) * 0.01f);
			Main.gore[num8].sticky = false;
			Gore obj = Main.gore[num8];
			obj.velocity *= 0.4f;
			Main.gore[num8].velocity.Y -= 0.6f;
			GoreCache.Add(num8);
		}
		if (drawPlayer.stinky && ((Game)Main.instance).IsActive && !Main.gamePaused)
		{
			num4 *= 0.7f;
			num6 *= 0.55f;
			if (Main.rand.Next(5) == 0 && shadow == 0f)
			{
				Vector2 vector2 = default(Vector2);
				((Vector2)(ref vector2))._002Ector((float)Main.rand.Next(-10, 11), (float)Main.rand.Next(-10, 11));
				((Vector2)(ref vector2)).Normalize();
				vector2.X *= 0.66f;
				vector2.Y = Math.Abs(vector2.Y);
				Vector2 vector3 = vector2 * (float)Main.rand.Next(3, 5) * 0.25f;
				int num9 = Dust.NewDust(Position, drawPlayer.width, drawPlayer.height, 188, vector3.X, vector3.Y * 0.5f, 100, default(Color), 1.5f);
				Dust obj2 = Main.dust[num9];
				obj2.velocity *= 0.1f;
				Main.dust[num9].velocity.Y -= 0.5f;
				DustCache.Add(num9);
			}
		}
		if (drawPlayer.slowOgreSpit && ((Game)Main.instance).IsActive && !Main.gamePaused)
		{
			num4 *= 0.6f;
			num6 *= 0.45f;
			if (Main.rand.Next(5) == 0 && shadow == 0f)
			{
				int type2 = Utils.SelectRandom<int>(Main.rand, 4, 256);
				Dust dust11 = Main.dust[Dust.NewDust(Position, drawPlayer.width, drawPlayer.height, type2, 0f, 0f, 100)];
				dust11.scale = 0.8f + Main.rand.NextFloat() * 0.6f;
				dust11.fadeIn = 0.5f;
				dust11.velocity *= 0.05f;
				dust11.noLight = true;
				if (dust11.type == 4)
				{
					dust11.color = new Color(80, 170, 40, 120);
				}
				DustCache.Add(dust11.dustIndex);
			}
			if (Main.rand.Next(5) == 0 && shadow == 0f)
			{
				int num10 = Gore.NewGore(Position + new Vector2(Main.rand.NextFloat(), Main.rand.NextFloat()) * drawPlayer.Size, Vector2.Zero, Utils.SelectRandom<int>(Main.rand, 1024, 1025, 1026), 0.65f);
				Gore obj3 = Main.gore[num10];
				obj3.velocity *= 0.05f;
				GoreCache.Add(num10);
			}
		}
		if (((Game)Main.instance).IsActive && !Main.gamePaused && shadow == 0f)
		{
			float num11 = (float)drawPlayer.miscCounter / 180f;
			float num13 = 0f;
			float num14 = 10f;
			int type3 = 90;
			int num15 = 0;
			for (int i = 0; i < 3; i++)
			{
				switch (i)
				{
				case 0:
					if (drawPlayer.nebulaLevelLife < 1)
					{
						continue;
					}
					num13 = (float)Math.PI * 2f / (float)drawPlayer.nebulaLevelLife;
					num15 = drawPlayer.nebulaLevelLife;
					break;
				case 1:
					if (drawPlayer.nebulaLevelMana < 1)
					{
						continue;
					}
					num13 = (float)Math.PI * -2f / (float)drawPlayer.nebulaLevelMana;
					num15 = drawPlayer.nebulaLevelMana;
					num11 = (float)(-drawPlayer.miscCounter) / 180f;
					num14 = 20f;
					type3 = 88;
					break;
				case 2:
					if (drawPlayer.nebulaLevelDamage < 1)
					{
						continue;
					}
					num13 = (float)Math.PI * 2f / (float)drawPlayer.nebulaLevelDamage;
					num15 = drawPlayer.nebulaLevelDamage;
					num11 = (float)drawPlayer.miscCounter / 180f;
					num14 = 30f;
					type3 = 86;
					break;
				}
				for (int j = 0; j < num15; j++)
				{
					Dust dust13 = Dust.NewDustDirect(Position, drawPlayer.width, drawPlayer.height, type3, 0f, 0f, 100, default(Color), 1.5f);
					dust13.noGravity = true;
					dust13.velocity = Vector2.Zero;
					dust13.position = drawPlayer.Center + Vector2.UnitY * drawPlayer.gfxOffY + (num11 * ((float)Math.PI * 2f) + num13 * (float)j).ToRotationVector2() * num14;
					dust13.customData = drawPlayer;
					DustCache.Add(dust13.dustIndex);
				}
			}
		}
		if (drawPlayer.witheredArmor && ((Game)Main.instance).IsActive && !Main.gamePaused)
		{
			num5 *= 0.5f;
			num4 *= 0.75f;
		}
		if (drawPlayer.witheredWeapon && drawPlayer.itemAnimation > 0 && heldItem.damage > 0 && ((Game)Main.instance).IsActive && !Main.gamePaused && Main.rand.Next(3) == 0)
		{
			Dust dust14 = Dust.NewDustDirect(new Vector2(Position.X - 2f, Position.Y - 2f), drawPlayer.width + 4, drawPlayer.height + 4, 272, 0f, 0f, 50, default(Color), 0.5f);
			dust14.velocity *= 1.6f;
			dust14.velocity.Y -= 1f;
			dust14.position = Vector2.Lerp(dust14.position, drawPlayer.Center, 0.5f);
			DustCache.Add(dust14.dustIndex);
		}
		_ = drawPlayer.shimmering;
		bool fullBright = false;
		PlayerLoader.DrawEffects(this, ref num4, ref num5, ref num6, ref num7, ref fullBright);
		if (num4 != 1f || num5 != 1f || num6 != 1f || num7 != 1f)
		{
			if (drawPlayer.onFire || drawPlayer.onFire2 || drawPlayer.onFrostBurn || drawPlayer.onFire3 || drawPlayer.onFrostBurn2 || fullBright)
			{
				colorEyeWhites = drawPlayer.GetImmuneAlpha(Color.White, shadow);
				colorEyes = drawPlayer.GetImmuneAlpha(drawPlayer.eyeColor, shadow);
				colorHair = drawPlayer.GetImmuneAlpha(drawPlayer.GetHairColor(useLighting: false), shadow);
				colorHead = drawPlayer.GetImmuneAlpha(drawPlayer.skinColor, shadow);
				colorBodySkin = drawPlayer.GetImmuneAlpha(drawPlayer.skinColor, shadow);
				colorShirt = drawPlayer.GetImmuneAlpha(drawPlayer.shirtColor, shadow);
				colorUnderShirt = drawPlayer.GetImmuneAlpha(drawPlayer.underShirtColor, shadow);
				colorPants = drawPlayer.GetImmuneAlpha(drawPlayer.pantsColor, shadow);
				colorLegs = drawPlayer.GetImmuneAlpha(drawPlayer.skinColor, shadow);
				colorShoes = drawPlayer.GetImmuneAlpha(drawPlayer.shoeColor, shadow);
				colorArmorHead = drawPlayer.GetImmuneAlpha(Color.White, shadow);
				colorArmorBody = drawPlayer.GetImmuneAlpha(Color.White, shadow);
				colorArmorLegs = drawPlayer.GetImmuneAlpha(Color.White, shadow);
				if (drawPlayer.isDisplayDollOrInanimate)
				{
					colorDisplayDollSkin = drawPlayer.GetImmuneAlpha(PlayerDrawHelper.DISPLAY_DOLL_DEFAULT_SKIN_COLOR, shadow);
				}
			}
			else
			{
				colorEyeWhites = Main.buffColor(colorEyeWhites, num4, num5, num6, num7);
				colorEyes = Main.buffColor(colorEyes, num4, num5, num6, num7);
				colorHair = Main.buffColor(colorHair, num4, num5, num6, num7);
				colorHead = Main.buffColor(colorHead, num4, num5, num6, num7);
				colorBodySkin = Main.buffColor(colorBodySkin, num4, num5, num6, num7);
				colorShirt = Main.buffColor(colorShirt, num4, num5, num6, num7);
				colorUnderShirt = Main.buffColor(colorUnderShirt, num4, num5, num6, num7);
				colorPants = Main.buffColor(colorPants, num4, num5, num6, num7);
				colorLegs = Main.buffColor(colorLegs, num4, num5, num6, num7);
				colorShoes = Main.buffColor(colorShoes, num4, num5, num6, num7);
				colorArmorHead = Main.buffColor(colorArmorHead, num4, num5, num6, num7);
				colorArmorBody = Main.buffColor(colorArmorBody, num4, num5, num6, num7);
				colorArmorLegs = Main.buffColor(colorArmorLegs, num4, num5, num6, num7);
				if (drawPlayer.isDisplayDollOrInanimate)
				{
					colorDisplayDollSkin = Main.buffColor(PlayerDrawHelper.DISPLAY_DOLL_DEFAULT_SKIN_COLOR, num4, num5, num6, num7);
				}
			}
		}
		if (drawPlayer.socialGhost)
		{
			colorEyeWhites = Color.Transparent;
			colorEyes = Color.Transparent;
			colorHair = Color.Transparent;
			colorHead = Color.Transparent;
			colorBodySkin = Color.Transparent;
			colorShirt = Color.Transparent;
			colorUnderShirt = Color.Transparent;
			colorPants = Color.Transparent;
			colorShoes = Color.Transparent;
			colorLegs = Color.Transparent;
			if (((Color)(ref colorArmorHead)).A > Main.gFade)
			{
				((Color)(ref colorArmorHead)).A = Main.gFade;
			}
			if (((Color)(ref colorArmorBody)).A > Main.gFade)
			{
				((Color)(ref colorArmorBody)).A = Main.gFade;
			}
			if (((Color)(ref colorArmorLegs)).A > Main.gFade)
			{
				((Color)(ref colorArmorLegs)).A = Main.gFade;
			}
			if (drawPlayer.isDisplayDollOrInanimate)
			{
				colorDisplayDollSkin = Color.Transparent;
			}
		}
		if (drawPlayer.socialIgnoreLight)
		{
			float num16 = 1f;
			colorEyeWhites = Color.White * num16;
			colorEyes = drawPlayer.eyeColor * num16;
			colorHair = GameShaders.Hair.GetColor(drawPlayer.hairDye, drawPlayer, Color.White);
			colorHead = drawPlayer.skinColor * num16;
			colorBodySkin = drawPlayer.skinColor * num16;
			colorShirt = drawPlayer.shirtColor * num16;
			colorUnderShirt = drawPlayer.underShirtColor * num16;
			colorPants = drawPlayer.pantsColor * num16;
			colorShoes = drawPlayer.shoeColor * num16;
			colorLegs = drawPlayer.skinColor * num16;
			colorArmorHead = Color.White;
			colorArmorBody = Color.White;
			colorArmorLegs = Color.White;
			if (drawPlayer.isDisplayDollOrInanimate)
			{
				colorDisplayDollSkin = PlayerDrawHelper.DISPLAY_DOLL_DEFAULT_SKIN_COLOR * num16;
			}
		}
		if (drawPlayer.opacityForAnimation != 1f)
		{
			shadow = 1f - drawPlayer.opacityForAnimation;
			float opacityForAnimation = drawPlayer.opacityForAnimation;
			opacityForAnimation *= opacityForAnimation;
			colorEyeWhites = Color.White * opacityForAnimation;
			colorEyes = drawPlayer.eyeColor * opacityForAnimation;
			colorHair = GameShaders.Hair.GetColor(drawPlayer.hairDye, drawPlayer, Color.White) * opacityForAnimation;
			colorHead = drawPlayer.skinColor * opacityForAnimation;
			colorBodySkin = drawPlayer.skinColor * opacityForAnimation;
			colorShirt = drawPlayer.shirtColor * opacityForAnimation;
			colorUnderShirt = drawPlayer.underShirtColor * opacityForAnimation;
			colorPants = drawPlayer.pantsColor * opacityForAnimation;
			colorShoes = drawPlayer.shoeColor * opacityForAnimation;
			colorLegs = drawPlayer.skinColor * opacityForAnimation;
			colorArmorHead = drawPlayer.GetImmuneAlpha(Color.White, shadow);
			colorArmorBody = drawPlayer.GetImmuneAlpha(Color.White, shadow);
			colorArmorLegs = drawPlayer.GetImmuneAlpha(Color.White, shadow);
			if (drawPlayer.isDisplayDollOrInanimate)
			{
				colorDisplayDollSkin = PlayerDrawHelper.DISPLAY_DOLL_DEFAULT_SKIN_COLOR * opacityForAnimation;
			}
		}
		stealth = 1f;
		if (heldItem.type == 3106)
		{
			float num17 = drawPlayer.stealth;
			if ((double)num17 < 0.03)
			{
				num17 = 0.03f;
			}
			float num18 = (1f + num17 * 10f) / 11f;
			if (num17 < 0f)
			{
				num17 = 0f;
			}
			if (!(num17 < 1f - shadow) && shadow > 0f)
			{
				num17 = shadow * 0.5f;
			}
			stealth = num18;
			colorArmorHead = new Color((int)(byte)((float)(int)((Color)(ref colorArmorHead)).R * num17), (int)(byte)((float)(int)((Color)(ref colorArmorHead)).G * num17), (int)(byte)((float)(int)((Color)(ref colorArmorHead)).B * num18), (int)(byte)((float)(int)((Color)(ref colorArmorHead)).A * num17));
			colorArmorBody = new Color((int)(byte)((float)(int)((Color)(ref colorArmorBody)).R * num17), (int)(byte)((float)(int)((Color)(ref colorArmorBody)).G * num17), (int)(byte)((float)(int)((Color)(ref colorArmorBody)).B * num18), (int)(byte)((float)(int)((Color)(ref colorArmorBody)).A * num17));
			colorArmorLegs = new Color((int)(byte)((float)(int)((Color)(ref colorArmorLegs)).R * num17), (int)(byte)((float)(int)((Color)(ref colorArmorLegs)).G * num17), (int)(byte)((float)(int)((Color)(ref colorArmorLegs)).B * num18), (int)(byte)((float)(int)((Color)(ref colorArmorLegs)).A * num17));
			num17 *= num17;
			colorEyeWhites = Color.Multiply(colorEyeWhites, num17);
			colorEyes = Color.Multiply(colorEyes, num17);
			colorHair = Color.Multiply(colorHair, num17);
			colorHead = Color.Multiply(colorHead, num17);
			colorBodySkin = Color.Multiply(colorBodySkin, num17);
			colorShirt = Color.Multiply(colorShirt, num17);
			colorUnderShirt = Color.Multiply(colorUnderShirt, num17);
			colorPants = Color.Multiply(colorPants, num17);
			colorShoes = Color.Multiply(colorShoes, num17);
			colorLegs = Color.Multiply(colorLegs, num17);
			colorMount = Color.Multiply(colorMount, num17);
			headGlowColor = Color.Multiply(headGlowColor, num17);
			bodyGlowColor = Color.Multiply(bodyGlowColor, num17);
			armGlowColor = Color.Multiply(armGlowColor, num17);
			legsGlowColor = Color.Multiply(legsGlowColor, num17);
			if (drawPlayer.isDisplayDollOrInanimate)
			{
				colorDisplayDollSkin = Color.Multiply(colorDisplayDollSkin, num17);
			}
		}
		else if (drawPlayer.shroomiteStealth)
		{
			float num19 = drawPlayer.stealth;
			if ((double)num19 < 0.03)
			{
				num19 = 0.03f;
			}
			float num20 = (1f + num19 * 10f) / 11f;
			if (num19 < 0f)
			{
				num19 = 0f;
			}
			if (!(num19 < 1f - shadow) && shadow > 0f)
			{
				num19 = shadow * 0.5f;
			}
			stealth = num20;
			colorArmorHead = new Color((int)(byte)((float)(int)((Color)(ref colorArmorHead)).R * num19), (int)(byte)((float)(int)((Color)(ref colorArmorHead)).G * num19), (int)(byte)((float)(int)((Color)(ref colorArmorHead)).B * num20), (int)(byte)((float)(int)((Color)(ref colorArmorHead)).A * num19));
			colorArmorBody = new Color((int)(byte)((float)(int)((Color)(ref colorArmorBody)).R * num19), (int)(byte)((float)(int)((Color)(ref colorArmorBody)).G * num19), (int)(byte)((float)(int)((Color)(ref colorArmorBody)).B * num20), (int)(byte)((float)(int)((Color)(ref colorArmorBody)).A * num19));
			colorArmorLegs = new Color((int)(byte)((float)(int)((Color)(ref colorArmorLegs)).R * num19), (int)(byte)((float)(int)((Color)(ref colorArmorLegs)).G * num19), (int)(byte)((float)(int)((Color)(ref colorArmorLegs)).B * num20), (int)(byte)((float)(int)((Color)(ref colorArmorLegs)).A * num19));
			num19 *= num19;
			colorEyeWhites = Color.Multiply(colorEyeWhites, num19);
			colorEyes = Color.Multiply(colorEyes, num19);
			colorHair = Color.Multiply(colorHair, num19);
			colorHead = Color.Multiply(colorHead, num19);
			colorBodySkin = Color.Multiply(colorBodySkin, num19);
			colorShirt = Color.Multiply(colorShirt, num19);
			colorUnderShirt = Color.Multiply(colorUnderShirt, num19);
			colorPants = Color.Multiply(colorPants, num19);
			colorShoes = Color.Multiply(colorShoes, num19);
			colorLegs = Color.Multiply(colorLegs, num19);
			colorMount = Color.Multiply(colorMount, num19);
			headGlowColor = Color.Multiply(headGlowColor, num19);
			bodyGlowColor = Color.Multiply(bodyGlowColor, num19);
			armGlowColor = Color.Multiply(armGlowColor, num19);
			legsGlowColor = Color.Multiply(legsGlowColor, num19);
			if (drawPlayer.isDisplayDollOrInanimate)
			{
				colorDisplayDollSkin = Color.Multiply(colorDisplayDollSkin, num19);
			}
		}
		else if (drawPlayer.setVortex)
		{
			float num21 = drawPlayer.stealth;
			if ((double)num21 < 0.03)
			{
				num21 = 0.03f;
			}
			if (num21 < 0f)
			{
				num21 = 0f;
			}
			if (!(num21 < 1f - shadow) && shadow > 0f)
			{
				num21 = shadow * 0.5f;
			}
			stealth = num21;
			Color secondColor = default(Color);
			((Color)(ref secondColor))._002Ector(Vector4.Lerp(Vector4.One, new Vector4(0f, 0.12f, 0.16f, 0f), 1f - num21));
			colorArmorHead = colorArmorHead.MultiplyRGBA(secondColor);
			colorArmorBody = colorArmorBody.MultiplyRGBA(secondColor);
			colorArmorLegs = colorArmorLegs.MultiplyRGBA(secondColor);
			num21 *= num21;
			colorEyeWhites = Color.Multiply(colorEyeWhites, num21);
			colorEyes = Color.Multiply(colorEyes, num21);
			colorHair = Color.Multiply(colorHair, num21);
			colorHead = Color.Multiply(colorHead, num21);
			colorBodySkin = Color.Multiply(colorBodySkin, num21);
			colorShirt = Color.Multiply(colorShirt, num21);
			colorUnderShirt = Color.Multiply(colorUnderShirt, num21);
			colorPants = Color.Multiply(colorPants, num21);
			colorShoes = Color.Multiply(colorShoes, num21);
			colorLegs = Color.Multiply(colorLegs, num21);
			colorMount = Color.Multiply(colorMount, num21);
			headGlowColor = Color.Multiply(headGlowColor, num21);
			bodyGlowColor = Color.Multiply(bodyGlowColor, num21);
			armGlowColor = Color.Multiply(armGlowColor, num21);
			legsGlowColor = Color.Multiply(legsGlowColor, num21);
			if (drawPlayer.isDisplayDollOrInanimate)
			{
				colorDisplayDollSkin = Color.Multiply(colorDisplayDollSkin, num21);
			}
		}
		if (hideEntirePlayer)
		{
			stealth = 1f;
			colorDisplayDollSkin = (legsGlowColor = (armGlowColor = (bodyGlowColor = (headGlowColor = (colorLegs = (colorShoes = (colorPants = (colorUnderShirt = (colorShirt = (colorBodySkin = (colorHead = (colorHair = (colorEyes = (colorEyeWhites = (colorArmorLegs = (colorArmorBody = (colorArmorHead = Color.Transparent)))))))))))))))));
		}
		if (drawPlayer.gravDir == 1f)
		{
			if (drawPlayer.direction == 1)
			{
				playerEffect = (SpriteEffects)0;
				itemEffect = (SpriteEffects)0;
			}
			else
			{
				playerEffect = (SpriteEffects)1;
				itemEffect = (SpriteEffects)1;
			}
			if (!drawPlayer.dead)
			{
				drawPlayer.legPosition.Y = 0f;
				drawPlayer.headPosition.Y = 0f;
				drawPlayer.bodyPosition.Y = 0f;
			}
		}
		else
		{
			if (drawPlayer.direction == 1)
			{
				playerEffect = (SpriteEffects)2;
				itemEffect = (SpriteEffects)2;
			}
			else
			{
				playerEffect = (SpriteEffects)3;
				itemEffect = (SpriteEffects)3;
			}
			if (!drawPlayer.dead)
			{
				drawPlayer.legPosition.Y = 6f;
				drawPlayer.headPosition.Y = 6f;
				drawPlayer.bodyPosition.Y = 6f;
			}
		}
		switch (heldItem.type)
		{
		case 3182:
		case 3184:
		case 3185:
		case 3782:
			itemEffect = (SpriteEffects)(itemEffect ^ 3);
			break;
		case 5118:
			if (player.gravDir < 0f)
			{
				itemEffect = (SpriteEffects)(itemEffect ^ 3);
			}
			break;
		}
		legVect = new Vector2((float)drawPlayer.legFrame.Width * 0.5f, (float)drawPlayer.legFrame.Height * 0.75f);
		bodyVect = new Vector2((float)drawPlayer.legFrame.Width * 0.5f, (float)drawPlayer.legFrame.Height * 0.5f);
		headVect = new Vector2((float)drawPlayer.legFrame.Width * 0.5f, (float)drawPlayer.legFrame.Height * 0.4f);
		if ((drawPlayer.merman || drawPlayer.forceMerman) && !drawPlayer.hideMerman)
		{
			drawPlayer.headRotation = drawPlayer.velocity.Y * (float)drawPlayer.direction * 0.1f;
			if ((double)drawPlayer.headRotation < -0.3)
			{
				drawPlayer.headRotation = -0.3f;
			}
			if ((double)drawPlayer.headRotation > 0.3)
			{
				drawPlayer.headRotation = 0.3f;
			}
		}
		else if (!drawPlayer.dead)
		{
			drawPlayer.headRotation = 0f;
		}
		SetupHairFrames();
		BoringSetup_End();
	}

	private void SetupHairFrames()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		Rectangle bodyFrame = drawPlayer.bodyFrame;
		bodyFrame = drawPlayer.bodyFrame;
		bodyFrame.Y -= 336;
		if (bodyFrame.Y < 0)
		{
			bodyFrame.Y = 0;
		}
		hairFrontFrame = bodyFrame;
		hairBackFrame = bodyFrame;
		if (hideHair)
		{
			hairFrontFrame.Height = 0;
			hairBackFrame.Height = 0;
		}
		else if (backHairDraw)
		{
			int height = 26;
			hairFrontFrame.Height = height;
		}
	}

	private void BoringSetup_End()
	{
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		//IL_0173: Unknown result type (might be due to invalid IL or missing references)
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_0189: Unknown result type (might be due to invalid IL or missing references)
		hidesTopSkin = (drawPlayer.body > 0 && ArmorIDs.Body.Sets.HidesTopSkin[drawPlayer.body]) || (drawPlayer.legs > 0 && ArmorIDs.Legs.Sets.HidesTopSkin[drawPlayer.legs]);
		hidesBottomSkin = (drawPlayer.body > 0 && ArmorIDs.Body.Sets.HidesBottomSkin[drawPlayer.body]) || (drawPlayer.legs > 0 && ArmorIDs.Legs.Sets.HidesBottomSkin[drawPlayer.legs]);
		isBottomOverriden = PlayerDrawLayers.IsBottomOverridden(ref this);
		drawFloatingTube = drawPlayer.hasFloatingTube && !hideEntirePlayer;
		drawUnicornHorn = drawPlayer.hasUnicornHorn;
		drawAngelHalo = drawPlayer.hasAngelHalo;
		drawFrontAccInNeckAccLayer = false;
		if (drawPlayer.bodyFrame.Y / drawPlayer.bodyFrame.Height == 5)
		{
			drawFrontAccInNeckAccLayer = drawPlayer.front > 0 && drawPlayer.front < ArmorIDs.Front.Count && ArmorIDs.Front.Sets.DrawsInNeckLayer[drawPlayer.front];
		}
		hairOffset = drawPlayer.GetHairDrawOffset(drawPlayer.hair, hatHair);
		helmetOffset = drawPlayer.GetHelmetDrawOffset();
		legsOffset = drawPlayer.GetLegsDrawOffset();
		CreateCompositeData();
	}

	private void AdjustmentsForWolfMount()
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		//IL_017c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_0233: Unknown result type (might be due to invalid IL or missing references)
		//IL_0248: Unknown result type (might be due to invalid IL or missing references)
		//IL_024d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0252: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Unknown result type (might be due to invalid IL or missing references)
		//IL_021e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0223: Unknown result type (might be due to invalid IL or missing references)
		//IL_0228: Unknown result type (might be due to invalid IL or missing references)
		//IL_025e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0276: Unknown result type (might be due to invalid IL or missing references)
		//IL_027b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0280: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_028b: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ad: Unknown result type (might be due to invalid IL or missing references)
		hideEntirePlayer = true;
		weaponDrawOrder = WeaponDrawOrder.BehindBackArm;
		Vector2 vector = default(Vector2);
		((Vector2)(ref vector))._002Ector((float)(10 + drawPlayer.direction * 14), 12f);
		Vector2 vector2 = Position + vector;
		Position.X -= drawPlayer.direction * 10;
		bool flag = drawPlayer.heldProj != -1 || heldItem.useStyle == 5;
		bool num = heldItem.useStyle == 2;
		bool flag2 = heldItem.useStyle == 9;
		bool flag3 = drawPlayer.itemAnimation > 0;
		bool flag4 = heldItem.fishingPole != 0;
		bool flag5 = heldItem.useStyle == 14;
		bool flag6 = heldItem.useStyle == 8;
		bool flag7 = heldItem.holdStyle == 1;
		bool flag8 = heldItem.holdStyle == 2;
		bool flag9 = heldItem.holdStyle == 5;
		if (num)
		{
			ItemLocation += new Vector2((float)(drawPlayer.direction * 14), -4f);
		}
		else if (!flag4)
		{
			if (flag2)
			{
				ItemLocation += (flag3 ? new Vector2((float)(drawPlayer.direction * 18), -4f) : new Vector2((float)(drawPlayer.direction * 14), -18f));
			}
			else if (flag9)
			{
				ItemLocation += new Vector2((float)(drawPlayer.direction * 17), -8f);
			}
			else if (flag7 && drawPlayer.itemAnimation == 0)
			{
				ItemLocation += new Vector2((float)(drawPlayer.direction * 14), -6f);
			}
			else if (flag8 && drawPlayer.itemAnimation == 0)
			{
				ItemLocation += new Vector2((float)(drawPlayer.direction * 17), 4f);
			}
			else if (flag6)
			{
				ItemLocation = vector2 + new Vector2((float)(drawPlayer.direction * 12), 2f);
			}
			else if (flag5)
			{
				ItemLocation += new Vector2((float)(drawPlayer.direction * 5), -2f);
			}
			else if (flag)
			{
				ItemLocation += new Vector2((float)(drawPlayer.direction * 4), -4f);
			}
			else
			{
				ItemLocation = vector2;
			}
		}
	}

	private void CreateCompositeData()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0302: Unknown result type (might be due to invalid IL or missing references)
		//IL_0307: Unknown result type (might be due to invalid IL or missing references)
		//IL_030e: Unknown result type (might be due to invalid IL or missing references)
		//IL_032a: Unknown result type (might be due to invalid IL or missing references)
		//IL_032f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0334: Unknown result type (might be due to invalid IL or missing references)
		//IL_036b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0379: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0401: Unknown result type (might be due to invalid IL or missing references)
		//IL_0408: Unknown result type (might be due to invalid IL or missing references)
		//IL_0409: Unknown result type (might be due to invalid IL or missing references)
		//IL_040e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0415: Unknown result type (might be due to invalid IL or missing references)
		//IL_0417: Unknown result type (might be due to invalid IL or missing references)
		//IL_041c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0423: Unknown result type (might be due to invalid IL or missing references)
		//IL_0424: Unknown result type (might be due to invalid IL or missing references)
		//IL_0429: Unknown result type (might be due to invalid IL or missing references)
		frontShoulderOffset = Vector2.Zero;
		backShoulderOffset = Vector2.Zero;
		usesCompositeTorso = drawPlayer.body > 0 && ArmorIDs.Body.Sets.UsesNewFramingCode[drawPlayer.body];
		usesCompositeFrontHandAcc = drawPlayer.handon > 0 && ArmorIDs.HandOn.Sets.UsesNewFramingCode[drawPlayer.handon];
		usesCompositeBackHandAcc = drawPlayer.handoff > 0 && ArmorIDs.HandOff.Sets.UsesNewFramingCode[drawPlayer.handoff];
		if (drawPlayer.body < 1)
		{
			usesCompositeTorso = true;
		}
		if (!usesCompositeTorso)
		{
			return;
		}
		Point pt = default(Point);
		((Point)(ref pt))._002Ector(1, 1);
		Point pt2 = default(Point);
		((Point)(ref pt2))._002Ector(0, 1);
		Point pt3 = default(Point);
		Point frameIndex = default(Point);
		Point frameIndex2 = default(Point);
		int num = drawPlayer.bodyFrame.Y / drawPlayer.bodyFrame.Height;
		compShoulderOverFrontArm = true;
		hideCompositeShoulders = false;
		bool flag = true;
		if (drawPlayer.body > 0)
		{
			flag = ArmorIDs.Body.Sets.showsShouldersWhileJumping[drawPlayer.body];
		}
		bool flag2 = false;
		if (drawPlayer.handon > 0)
		{
			flag2 = ArmorIDs.HandOn.Sets.UsesOldFramingTexturesForWalking[drawPlayer.handon];
		}
		bool flag3 = !flag2;
		switch (num)
		{
		case 0:
			frameIndex2.X = 2;
			flag3 = true;
			break;
		case 1:
			frameIndex2.X = 3;
			compShoulderOverFrontArm = false;
			flag3 = true;
			break;
		case 2:
			frameIndex2.X = 4;
			compShoulderOverFrontArm = false;
			flag3 = true;
			break;
		case 3:
			frameIndex2.X = 5;
			compShoulderOverFrontArm = true;
			flag3 = true;
			break;
		case 4:
			frameIndex2.X = 6;
			compShoulderOverFrontArm = true;
			flag3 = true;
			break;
		case 5:
			frameIndex2.X = 2;
			frameIndex2.Y = 1;
			pt3.X = 1;
			compShoulderOverFrontArm = false;
			flag3 = true;
			if (!flag)
			{
				hideCompositeShoulders = true;
			}
			break;
		case 6:
			frameIndex2.X = 3;
			frameIndex2.Y = 1;
			break;
		case 7:
		case 8:
		case 9:
		case 10:
			frameIndex2.X = 4;
			frameIndex2.Y = 1;
			break;
		case 11:
		case 12:
		case 13:
			frameIndex2.X = 3;
			frameIndex2.Y = 1;
			break;
		case 14:
			frameIndex2.X = 5;
			frameIndex2.Y = 1;
			break;
		case 15:
		case 16:
			frameIndex2.X = 6;
			frameIndex2.Y = 1;
			break;
		case 17:
			frameIndex2.X = 5;
			frameIndex2.Y = 1;
			break;
		case 18:
		case 19:
			frameIndex2.X = 3;
			frameIndex2.Y = 1;
			break;
		}
		CreateCompositeData_DetermineShoulderOffsets(drawPlayer.body, num);
		backShoulderOffset *= new Vector2((float)drawPlayer.direction, drawPlayer.gravDir);
		frontShoulderOffset *= new Vector2((float)drawPlayer.direction, drawPlayer.gravDir);
		if (drawPlayer.body > 0 && ArmorIDs.Body.Sets.shouldersAreAlwaysInTheBack[drawPlayer.body])
		{
			compShoulderOverFrontArm = false;
		}
		usesCompositeFrontHandAcc = flag3;
		frameIndex.X = frameIndex2.X;
		frameIndex.Y = frameIndex2.Y + 2;
		UpdateCompositeArm(drawPlayer.compositeFrontArm, ref compositeFrontArmRotation, ref frameIndex2, 7);
		UpdateCompositeArm(drawPlayer.compositeBackArm, ref compositeBackArmRotation, ref frameIndex, 8);
		if (!drawPlayer.Male)
		{
			pt.Y += 2;
			pt2.Y += 2;
			pt3.Y += 2;
		}
		compBackShoulderFrame = CreateCompositeFrameRect(pt);
		compFrontShoulderFrame = CreateCompositeFrameRect(pt2);
		compBackArmFrame = CreateCompositeFrameRect(frameIndex);
		compFrontArmFrame = CreateCompositeFrameRect(frameIndex2);
		compTorsoFrame = CreateCompositeFrameRect(pt3);
	}

	private void CreateCompositeData_DetermineShoulderOffsets(int armor, int targetFrameNumber)
	{
		int num = 0;
		switch (armor)
		{
		case 55:
			num = 1;
			break;
		case 71:
			num = 2;
			break;
		case 204:
			num = 3;
			break;
		case 183:
			num = 4;
			break;
		case 201:
			num = 5;
			break;
		case 101:
			num = 6;
			break;
		case 207:
			num = 7;
			break;
		}
		switch (num)
		{
		case 1:
			switch (targetFrameNumber)
			{
			case 6:
				frontShoulderOffset.X = -2f;
				break;
			case 7:
			case 8:
			case 9:
			case 10:
				frontShoulderOffset.X = -4f;
				break;
			case 11:
			case 12:
			case 13:
			case 14:
				frontShoulderOffset.X = -2f;
				break;
			case 18:
			case 19:
				frontShoulderOffset.X = -2f;
				break;
			case 15:
			case 16:
			case 17:
				break;
			}
			break;
		case 2:
			switch (targetFrameNumber)
			{
			case 6:
				frontShoulderOffset.X = -2f;
				break;
			case 7:
			case 8:
			case 9:
			case 10:
				frontShoulderOffset.X = -4f;
				break;
			case 11:
			case 12:
			case 13:
			case 14:
				frontShoulderOffset.X = -2f;
				break;
			case 18:
			case 19:
				frontShoulderOffset.X = -2f;
				break;
			case 15:
			case 16:
			case 17:
				break;
			}
			break;
		case 3:
			switch (targetFrameNumber)
			{
			case 7:
			case 8:
			case 9:
				frontShoulderOffset.X = -2f;
				break;
			case 15:
			case 16:
			case 17:
				frontShoulderOffset.X = 2f;
				break;
			}
			break;
		case 4:
			switch (targetFrameNumber)
			{
			case 6:
				frontShoulderOffset.X = -2f;
				break;
			case 7:
			case 8:
			case 9:
			case 10:
				frontShoulderOffset.X = -4f;
				break;
			case 11:
			case 12:
			case 13:
				frontShoulderOffset.X = -2f;
				break;
			case 15:
			case 16:
				frontShoulderOffset.X = 2f;
				break;
			case 18:
			case 19:
				frontShoulderOffset.X = -2f;
				break;
			case 14:
			case 17:
				break;
			}
			break;
		case 5:
			switch (targetFrameNumber)
			{
			case 7:
			case 8:
			case 9:
			case 10:
				frontShoulderOffset.X = -2f;
				break;
			case 15:
			case 16:
				frontShoulderOffset.X = 2f;
				break;
			}
			break;
		case 6:
			switch (targetFrameNumber)
			{
			case 7:
			case 8:
			case 9:
			case 10:
				frontShoulderOffset.X = -2f;
				break;
			case 14:
			case 15:
			case 16:
			case 17:
				frontShoulderOffset.X = 2f;
				break;
			}
			break;
		case 7:
			switch (targetFrameNumber)
			{
			case 6:
			case 7:
			case 8:
			case 9:
			case 10:
				frontShoulderOffset.X = -2f;
				break;
			case 11:
			case 12:
			case 13:
			case 14:
				frontShoulderOffset.X = -2f;
				break;
			case 18:
			case 19:
				frontShoulderOffset.X = -2f;
				break;
			case 15:
			case 16:
			case 17:
				break;
			}
			break;
		}
	}

	private Rectangle CreateCompositeFrameRect(Point pt)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		return new Rectangle(pt.X * 40, pt.Y * 56, 40, 56);
	}

	private void UpdateCompositeArm(Player.CompositeArmData data, ref float rotation, ref Point frameIndex, int targetX)
	{
		if (data.enabled)
		{
			rotation = data.rotation;
			switch (data.stretch)
			{
			case Player.CompositeArmStretchAmount.Full:
				frameIndex.X = targetX;
				frameIndex.Y = 0;
				break;
			case Player.CompositeArmStretchAmount.ThreeQuarters:
				frameIndex.X = targetX;
				frameIndex.Y = 1;
				break;
			case Player.CompositeArmStretchAmount.Quarter:
				frameIndex.X = targetX;
				frameIndex.Y = 2;
				break;
			case Player.CompositeArmStretchAmount.None:
				frameIndex.X = targetX;
				frameIndex.Y = 3;
				break;
			}
		}
		else
		{
			rotation = 0f;
		}
	}
}
