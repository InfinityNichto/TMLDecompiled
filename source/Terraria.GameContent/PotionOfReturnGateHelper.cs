using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.DataStructures;
using Terraria.ID;

namespace Terraria.GameContent;

public struct PotionOfReturnGateHelper
{
	public enum GateType
	{
		EntryPoint,
		ExitPoint
	}

	private readonly Vector2 _position;

	private readonly float _opacity;

	private readonly int _frameNumber;

	private readonly GateType _gateType;

	public PotionOfReturnGateHelper(GateType gateType, Vector2 worldPosition, float opacity)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		_gateType = gateType;
		worldPosition.Y -= 2f;
		_position = worldPosition;
		_opacity = opacity;
		int num = (int)(((float)Main.tileFrameCounter[491] + _position.X + _position.Y) % 40f) / 5;
		if (gateType == GateType.ExitPoint)
		{
			num = 7 - num;
		}
		_frameNumber = num;
	}

	public void Update()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		Lighting.AddLight(_position, 0.4f, 0.2f, 0.9f);
		SpawnReturnPortalDust();
	}

	public void SpawnReturnPortalDust()
	{
		//IL_03f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0404: Unknown result type (might be due to invalid IL or missing references)
		//IL_0406: Unknown result type (might be due to invalid IL or missing references)
		//IL_0412: Unknown result type (might be due to invalid IL or missing references)
		//IL_0417: Unknown result type (might be due to invalid IL or missing references)
		//IL_041c: Unknown result type (might be due to invalid IL or missing references)
		//IL_041f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0424: Unknown result type (might be due to invalid IL or missing references)
		//IL_042b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0430: Unknown result type (might be due to invalid IL or missing references)
		//IL_045e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0464: Unknown result type (might be due to invalid IL or missing references)
		//IL_047f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0484: Unknown result type (might be due to invalid IL or missing references)
		//IL_048a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0497: Unknown result type (might be due to invalid IL or missing references)
		//IL_049d: Unknown result type (might be due to invalid IL or missing references)
		//IL_049e: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0500: Unknown result type (might be due to invalid IL or missing references)
		//IL_02dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0302: Unknown result type (might be due to invalid IL or missing references)
		//IL_0307: Unknown result type (might be due to invalid IL or missing references)
		//IL_030a: Unknown result type (might be due to invalid IL or missing references)
		//IL_030f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0316: Unknown result type (might be due to invalid IL or missing references)
		//IL_031b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0349: Unknown result type (might be due to invalid IL or missing references)
		//IL_034f: Unknown result type (might be due to invalid IL or missing references)
		//IL_036a: Unknown result type (might be due to invalid IL or missing references)
		//IL_036f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0375: Unknown result type (might be due to invalid IL or missing references)
		//IL_0382: Unknown result type (might be due to invalid IL or missing references)
		//IL_0388: Unknown result type (might be due to invalid IL or missing references)
		//IL_0389: Unknown result type (might be due to invalid IL or missing references)
		//IL_0393: Unknown result type (might be due to invalid IL or missing references)
		//IL_0398: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_0189: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0197: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0200: Unknown result type (might be due to invalid IL or missing references)
		//IL_0213: Unknown result type (might be due to invalid IL or missing references)
		//IL_0218: Unknown result type (might be due to invalid IL or missing references)
		//IL_021d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0224: Unknown result type (might be due to invalid IL or missing references)
		//IL_0231: Unknown result type (might be due to invalid IL or missing references)
		//IL_0237: Unknown result type (might be due to invalid IL or missing references)
		//IL_0238: Unknown result type (might be due to invalid IL or missing references)
		//IL_0242: Unknown result type (might be due to invalid IL or missing references)
		//IL_0247: Unknown result type (might be due to invalid IL or missing references)
		//IL_0284: Unknown result type (might be due to invalid IL or missing references)
		//IL_028b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0295: Unknown result type (might be due to invalid IL or missing references)
		//IL_029a: Unknown result type (might be due to invalid IL or missing references)
		//IL_029f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0170: Unknown result type (might be due to invalid IL or missing references)
		if (_gateType == GateType.EntryPoint)
		{
			if (Main.rand.Next(3) == 0)
			{
				if (Main.rand.Next(2) == 0)
				{
					Vector2 vector = Vector2.UnitY.RotatedByRandom(6.2831854820251465);
					vector *= new Vector2(0.5f, 1f);
					Dust dust = Dust.NewDustDirect(_position - vector * 30f, 0, 0, Utils.SelectRandom<int>(Main.rand, 86, 88));
					dust.noGravity = true;
					dust.noLightEmittence = true;
					dust.position = _position - vector.SafeNormalize(Vector2.Zero) * (float)Main.rand.Next(10, 21);
					dust.velocity = vector.RotatedBy(1.5707963705062866) * 2f;
					dust.scale = 0.5f + Main.rand.NextFloat();
					dust.fadeIn = 0.5f;
					dust.customData = this;
					dust.position += dust.velocity * 10f;
					dust.velocity *= -1f;
				}
				else
				{
					Vector2 vector2 = Vector2.UnitY.RotatedByRandom(6.2831854820251465);
					vector2 *= new Vector2(0.5f, 1f);
					Dust dust2 = Dust.NewDustDirect(_position - vector2 * 30f, 0, 0, 240);
					dust2.noGravity = true;
					dust2.noLight = true;
					dust2.position = _position - vector2.SafeNormalize(Vector2.Zero) * (float)Main.rand.Next(5, 10);
					dust2.velocity = vector2.RotatedBy(-1.5707963705062866) * 3f;
					dust2.scale = 0.5f + Main.rand.NextFloat();
					dust2.fadeIn = 0.5f;
					dust2.customData = this;
					dust2.position += dust2.velocity * 10f;
					dust2.velocity *= -1f;
				}
			}
		}
		else if (Main.rand.Next(3) == 0)
		{
			if (Main.rand.Next(2) == 0)
			{
				Vector2 vector3 = Vector2.UnitY.RotatedByRandom(6.2831854820251465);
				vector3 *= new Vector2(0.5f, 1f);
				Dust dust3 = Dust.NewDustDirect(_position - vector3 * 30f, 0, 0, Utils.SelectRandom<int>(Main.rand, 86, 88));
				dust3.noGravity = true;
				dust3.noLightEmittence = true;
				dust3.position = _position;
				dust3.velocity = vector3.RotatedBy(-0.7853981852531433) * 2f;
				dust3.scale = 0.5f + Main.rand.NextFloat();
				dust3.fadeIn = 0.5f;
				dust3.customData = this;
				dust3.position += vector3 * new Vector2(20f);
			}
			else
			{
				Vector2 vector4 = Vector2.UnitY.RotatedByRandom(6.2831854820251465);
				vector4 *= new Vector2(0.5f, 1f);
				Dust dust4 = Dust.NewDustDirect(_position - vector4 * 30f, 0, 0, Utils.SelectRandom<int>(Main.rand, 86, 88));
				dust4.noGravity = true;
				dust4.noLightEmittence = true;
				dust4.position = _position;
				dust4.velocity = vector4.RotatedBy(-0.7853981852531433) * 2f;
				dust4.scale = 0.5f + Main.rand.NextFloat();
				dust4.fadeIn = 0.5f;
				dust4.customData = this;
				dust4.position += vector4 * new Vector2(20f);
			}
		}
	}

	public void DrawToDrawData(List<DrawData> drawDataList, int selectionMode)
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0158: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0170: Unknown result type (might be due to invalid IL or missing references)
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0201: Unknown result type (might be due to invalid IL or missing references)
		//IL_0206: Unknown result type (might be due to invalid IL or missing references)
		//IL_020d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0214: Unknown result type (might be due to invalid IL or missing references)
		//IL_0215: Unknown result type (might be due to invalid IL or missing references)
		//IL_021f: Unknown result type (might be due to invalid IL or missing references)
		short num = (short)((_gateType == GateType.EntryPoint) ? 183 : 184);
		Asset<Texture2D> asset = TextureAssets.Extra[num];
		Rectangle rectangle = asset.Frame(1, 8, 0, _frameNumber);
		Color color = Lighting.GetColor(_position.ToTileCoordinates());
		color = Color.Lerp(color, Color.White, 0.5f);
		color *= _opacity;
		DrawData drawData = new DrawData(asset.Value, _position - Main.screenPosition, rectangle, color, 0f, rectangle.Size() / 2f, 1f, (SpriteEffects)0);
		drawDataList.Add(drawData);
		for (float num2 = 0f; num2 < 1f; num2 += 0.34f)
		{
			DrawData item = drawData;
			item.color = new Color(127, 50, 127, 0) * _opacity;
			ref Vector2 scale = ref item.scale;
			scale *= 1.1f;
			float x = (Main.GlobalTimeWrappedHourly / 5f * ((float)Math.PI * 2f)).ToRotationVector2().X;
			ref Color color2 = ref item.color;
			color2 *= x * 0.1f + 0.3f;
			ref Vector2 position = ref item.position;
			position += ((Main.GlobalTimeWrappedHourly / 5f + num2) * ((float)Math.PI * 2f)).ToRotationVector2() * (x * 1f + 2f);
			drawDataList.Add(item);
		}
		if (selectionMode != 0)
		{
			int num3 = (((Color)(ref color)).R + ((Color)(ref color)).G + ((Color)(ref color)).B) / 3;
			if (num3 > 10)
			{
				Color selectionGlowColor = Colors.GetSelectionGlowColor(selectionMode == 2, num3);
				Texture2D value = TextureAssets.Extra[242].Value;
				Rectangle value2 = value.Frame(1, 8, 0, _frameNumber);
				drawData = new DrawData(value, _position - Main.screenPosition, value2, selectionGlowColor, 0f, rectangle.Size() / 2f, 1f, (SpriteEffects)0);
				drawDataList.Add(drawData);
			}
		}
	}
}
