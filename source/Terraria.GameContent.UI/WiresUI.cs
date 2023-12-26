using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameInput;

namespace Terraria.GameContent.UI;

public class WiresUI
{
	public static class Settings
	{
		[Flags]
		public enum MultiToolMode
		{
			Red = 1,
			Green = 2,
			Blue = 4,
			Yellow = 8,
			Actuator = 0x10,
			Cutter = 0x20
		}

		public static MultiToolMode ToolMode = MultiToolMode.Red;

		private static int _lastActuatorEnabled;

		public static bool DrawWires
		{
			get
			{
				if (Main.getGoodWorld && !NPC.downedBoss3)
				{
					return false;
				}
				if (!Main.player[Main.myPlayer].inventory[Main.player[Main.myPlayer].selectedItem].mech)
				{
					if (Main.player[Main.myPlayer].InfoAccMechShowWires)
					{
						return Main.player[Main.myPlayer].builderAccStatus[8] == 0;
					}
					return false;
				}
				return true;
			}
		}

		public static bool HideWires => Main.player[Main.myPlayer].inventory[Main.player[Main.myPlayer].selectedItem].type == 3620;

		public static bool DrawToolModeUI
		{
			get
			{
				int type = Main.player[Main.myPlayer].inventory[Main.player[Main.myPlayer].selectedItem].type;
				if (type != 3611)
				{
					return type == 3625;
				}
				return true;
			}
		}

		public static bool DrawToolAllowActuators
		{
			get
			{
				int type = Main.player[Main.myPlayer].inventory[Main.player[Main.myPlayer].selectedItem].type;
				if (type == 3611)
				{
					_lastActuatorEnabled = 2;
				}
				if (type == 3625)
				{
					_lastActuatorEnabled = 1;
				}
				return _lastActuatorEnabled == 2;
			}
		}
	}

	public class WiresRadial
	{
		public Vector2 position;

		public bool active;

		public bool OnWiresMenu;

		private float _lineOpacity;

		public void Update()
		{
			FlowerUpdate();
			LineUpdate();
		}

		private void LineUpdate()
		{
			bool value = true;
			float min = 0.75f;
			Player player = Main.player[Main.myPlayer];
			if (!Settings.DrawToolModeUI || Main.drawingPlayerChat)
			{
				value = false;
				min = 0f;
			}
			if (player.dead || Main.mouseItem.type > 0)
			{
				value = false;
				_lineOpacity = 0f;
				return;
			}
			if (player.cursorItemIconEnabled && player.cursorItemIconID != 0 && player.cursorItemIconID != 3625)
			{
				value = false;
				_lineOpacity = 0f;
				return;
			}
			if ((!player.cursorItemIconEnabled && ((!PlayerInput.UsingGamepad && !Settings.DrawToolAllowActuators) || player.mouseInterface || player.lastMouseInterface)) || Main.ingameOptionsWindow || Main.InGameUI.IsVisible)
			{
				value = false;
				_lineOpacity = 0f;
				return;
			}
			float num = Utils.Clamp(_lineOpacity + 0.05f * (float)value.ToDirectionInt(), min, 1f);
			_lineOpacity += 0.05f * (float)Math.Sign(num - _lineOpacity);
			if (Math.Abs(_lineOpacity - num) < 0.05f)
			{
				_lineOpacity = num;
			}
		}

		private void FlowerUpdate()
		{
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			Player player = Main.player[Main.myPlayer];
			if (!Settings.DrawToolModeUI)
			{
				active = false;
				return;
			}
			if ((player.mouseInterface || player.lastMouseInterface) && !OnWiresMenu)
			{
				active = false;
				return;
			}
			if (player.dead || Main.mouseItem.type > 0)
			{
				active = false;
				OnWiresMenu = false;
				return;
			}
			OnWiresMenu = false;
			if (!Main.mouseRight || !Main.mouseRightRelease || PlayerInput.LockGamepadTileUseButton || player.noThrow != 0 || Main.HoveringOverAnNPC || player.talkNPC != -1)
			{
				return;
			}
			if (active)
			{
				active = false;
			}
			else if (!Main.SmartInteractShowingGenuine)
			{
				active = true;
				position = Main.MouseScreen;
				if (PlayerInput.UsingGamepad && Main.SmartCursorWanted)
				{
					position = new Vector2((float)Main.screenWidth, (float)Main.screenHeight) / 2f;
				}
			}
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			DrawFlower(spriteBatch);
			DrawCursorArea(spriteBatch);
		}

		private void DrawLine(SpriteBatch spriteBatch)
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0129: Unknown result type (might be due to invalid IL or missing references)
			//IL_0161: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_019c: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0145: Unknown result type (might be due to invalid IL or missing references)
			//IL_0150: Unknown result type (might be due to invalid IL or missing references)
			//IL_0155: Unknown result type (might be due to invalid IL or missing references)
			//IL_015a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0183: Unknown result type (might be due to invalid IL or missing references)
			//IL_018e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0193: Unknown result type (might be due to invalid IL or missing references)
			//IL_0198: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_033e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0345: Unknown result type (might be due to invalid IL or missing references)
			//IL_034a: Unknown result type (might be due to invalid IL or missing references)
			//IL_040e: Unknown result type (might be due to invalid IL or missing references)
			//IL_041a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0422: Unknown result type (might be due to invalid IL or missing references)
			//IL_042e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0438: Unknown result type (might be due to invalid IL or missing references)
			//IL_0451: Unknown result type (might be due to invalid IL or missing references)
			//IL_045d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0465: Unknown result type (might be due to invalid IL or missing references)
			//IL_0471: Unknown result type (might be due to invalid IL or missing references)
			//IL_047b: Unknown result type (might be due to invalid IL or missing references)
			if (active || _lineOpacity == 0f)
			{
				return;
			}
			Vector2 vector = Main.MouseScreen;
			Vector2 vector2 = default(Vector2);
			((Vector2)(ref vector2))._002Ector((float)(Main.screenWidth / 2), (float)(Main.screenHeight - 70));
			if (PlayerInput.UsingGamepad)
			{
				vector = Vector2.Zero;
			}
			Vector2 vector3 = vector - vector2;
			Vector2.Dot(Vector2.Normalize(vector3), Vector2.UnitX);
			Vector2.Dot(Vector2.Normalize(vector3), Vector2.UnitY);
			vector3.ToRotation();
			((Vector2)(ref vector3)).Length();
			bool flag = false;
			bool drawToolAllowActuators = Settings.DrawToolAllowActuators;
			for (int i = 0; i < 6; i++)
			{
				if (!drawToolAllowActuators && i == 5)
				{
					continue;
				}
				bool flag2 = Settings.ToolMode.HasFlag((Settings.MultiToolMode)(1 << i));
				if (i == 5)
				{
					flag2 = Settings.ToolMode.HasFlag(Settings.MultiToolMode.Actuator);
				}
				Vector2 vector4 = vector2 + Vector2.UnitX * (45f * ((float)i - 1.5f));
				int num = i;
				if (i == 0)
				{
					num = 3;
				}
				if (i == 3)
				{
					num = 0;
				}
				switch (num)
				{
				case 0:
				case 1:
					vector4 = vector2 + new Vector2((45f + (float)(drawToolAllowActuators ? 15 : 0)) * (float)(2 - num), 0f) * _lineOpacity;
					break;
				case 2:
				case 3:
					vector4 = vector2 + new Vector2((0f - (45f + (float)(drawToolAllowActuators ? 15 : 0))) * (float)(num - 1), 0f) * _lineOpacity;
					break;
				case 5:
					vector4 = vector2 + new Vector2(0f, 22f) * _lineOpacity;
					break;
				case 4:
					flag2 = false;
					vector4 = vector2 - new Vector2(0f, drawToolAllowActuators ? 22f : 0f) * _lineOpacity;
					break;
				}
				bool flag3 = false;
				if (!PlayerInput.UsingGamepad)
				{
					flag3 = Vector2.Distance(vector4, vector) < 19f * _lineOpacity;
				}
				if (flag)
				{
					flag3 = false;
				}
				if (flag3)
				{
					flag = true;
				}
				Texture2D value = TextureAssets.WireUi[(Settings.ToolMode.HasFlag(Settings.MultiToolMode.Cutter) ? 8 : 0) + (flag3 ? 1 : 0)].Value;
				Texture2D texture2D = null;
				switch (i)
				{
				case 0:
				case 1:
				case 2:
				case 3:
					texture2D = TextureAssets.WireUi[2 + i].Value;
					break;
				case 4:
					texture2D = TextureAssets.WireUi[Settings.ToolMode.HasFlag(Settings.MultiToolMode.Cutter) ? 7 : 6].Value;
					break;
				case 5:
					texture2D = TextureAssets.WireUi[10].Value;
					break;
				}
				Color color = Color.White;
				Color color2 = Color.White;
				if (!flag2 && i != 4)
				{
					if (flag3)
					{
						((Color)(ref color2))._002Ector(100, 100, 100);
						((Color)(ref color2))._002Ector(120, 120, 120);
						((Color)(ref color))._002Ector(200, 200, 200);
					}
					else
					{
						((Color)(ref color2))._002Ector(150, 150, 150);
						((Color)(ref color2))._002Ector(80, 80, 80);
						((Color)(ref color))._002Ector(100, 100, 100);
					}
				}
				Utils.CenteredRectangle(vector4, new Vector2(40f));
				if (flag3)
				{
					if (Main.mouseLeft && Main.mouseLeftRelease)
					{
						switch (i)
						{
						case 0:
							Settings.ToolMode ^= Settings.MultiToolMode.Red;
							break;
						case 1:
							Settings.ToolMode ^= Settings.MultiToolMode.Green;
							break;
						case 2:
							Settings.ToolMode ^= Settings.MultiToolMode.Blue;
							break;
						case 3:
							Settings.ToolMode ^= Settings.MultiToolMode.Yellow;
							break;
						case 4:
							Settings.ToolMode ^= Settings.MultiToolMode.Cutter;
							break;
						case 5:
							Settings.ToolMode ^= Settings.MultiToolMode.Actuator;
							break;
						}
					}
					if (!Main.mouseLeft || Main.player[Main.myPlayer].mouseInterface)
					{
						Main.player[Main.myPlayer].mouseInterface = true;
					}
					OnWiresMenu = true;
				}
				spriteBatch.Draw(value, vector4, (Rectangle?)null, color * _lineOpacity, 0f, value.Size() / 2f, _lineOpacity, (SpriteEffects)0, 0f);
				spriteBatch.Draw(texture2D, vector4, (Rectangle?)null, color2 * _lineOpacity, 0f, texture2D.Size() / 2f, _lineOpacity, (SpriteEffects)0, 0f);
			}
			if (Main.mouseLeft && Main.mouseLeftRelease && !flag)
			{
				active = false;
			}
		}

		private void DrawFlower(SpriteBatch spriteBatch)
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0135: Unknown result type (might be due to invalid IL or missing references)
			//IL_0136: Unknown result type (might be due to invalid IL or missing references)
			//IL_014a: Unknown result type (might be due to invalid IL or missing references)
			//IL_014f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0154: Unknown result type (might be due to invalid IL or missing references)
			//IL_0216: Unknown result type (might be due to invalid IL or missing references)
			//IL_0217: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0205: Unknown result type (might be due to invalid IL or missing references)
			//IL_020a: Unknown result type (might be due to invalid IL or missing references)
			//IL_020f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0196: Unknown result type (might be due to invalid IL or missing references)
			//IL_0197: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_025a: Unknown result type (might be due to invalid IL or missing references)
			//IL_025c: Unknown result type (might be due to invalid IL or missing references)
			//IL_025d: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0362: Unknown result type (might be due to invalid IL or missing references)
			//IL_0367: Unknown result type (might be due to invalid IL or missing references)
			//IL_0369: Unknown result type (might be due to invalid IL or missing references)
			//IL_036e: Unknown result type (might be due to invalid IL or missing references)
			//IL_03df: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_03eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0496: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_04b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_04cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_04d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ec: Unknown result type (might be due to invalid IL or missing references)
			if (!active)
			{
				return;
			}
			Vector2 vector = Main.MouseScreen;
			Vector2 vector2 = position;
			if (PlayerInput.UsingGamepad && Main.SmartCursorWanted)
			{
				vector = ((PlayerInput.GamepadThumbstickRight != Vector2.Zero) ? (position + PlayerInput.GamepadThumbstickRight * 40f) : ((!(PlayerInput.GamepadThumbstickLeft != Vector2.Zero)) ? position : (position + PlayerInput.GamepadThumbstickLeft * 40f)));
			}
			Vector2 vector3 = vector - vector2;
			Vector2.Dot(Vector2.Normalize(vector3), Vector2.UnitX);
			Vector2.Dot(Vector2.Normalize(vector3), Vector2.UnitY);
			float num = vector3.ToRotation();
			float num2 = ((Vector2)(ref vector3)).Length();
			bool flag = false;
			bool drawToolAllowActuators = Settings.DrawToolAllowActuators;
			float num3 = 4 + drawToolAllowActuators.ToInt();
			float num4 = (drawToolAllowActuators ? 11f : (-0.5f));
			for (int i = 0; i < 6; i++)
			{
				if (!drawToolAllowActuators && i == 5)
				{
					continue;
				}
				bool flag2 = Settings.ToolMode.HasFlag((Settings.MultiToolMode)(1 << i));
				if (i == 5)
				{
					flag2 = Settings.ToolMode.HasFlag(Settings.MultiToolMode.Actuator);
				}
				Vector2 vector4 = vector2 + Vector2.UnitX * (45f * ((float)i - 1.5f));
				switch (i)
				{
				case 0:
				case 1:
				case 2:
				case 3:
				{
					float num5 = i;
					if (i == 0)
					{
						num5 = 3f;
					}
					if (i == 3)
					{
						num5 = 0f;
					}
					vector4 = vector2 + Vector2.UnitX.RotatedBy(num5 * ((float)Math.PI * 2f) / num3 - (float)Math.PI / num4) * 45f;
					break;
				}
				case 5:
					vector4 = vector2 + Vector2.UnitX.RotatedBy((float)(i - 1) * ((float)Math.PI * 2f) / num3 - (float)Math.PI / num4) * 45f;
					break;
				case 4:
					flag2 = false;
					vector4 = vector2;
					break;
				}
				bool flag3 = false;
				if (i == 4)
				{
					flag3 = num2 < 20f;
				}
				switch (i)
				{
				case 4:
					flag3 = num2 < 20f;
					break;
				case 0:
				case 1:
				case 2:
				case 3:
				case 5:
				{
					float value = (vector4 - vector2).ToRotation().AngleTowards(num, (float)Math.PI * 2f / (num3 * 2f)) - num;
					if (num2 >= 20f && Math.Abs(value) < 0.01f)
					{
						flag3 = true;
					}
					break;
				}
				}
				if (!PlayerInput.UsingGamepad)
				{
					flag3 = Vector2.Distance(vector4, vector) < 19f;
				}
				if (flag)
				{
					flag3 = false;
				}
				if (flag3)
				{
					flag = true;
				}
				Texture2D value2 = TextureAssets.WireUi[(Settings.ToolMode.HasFlag(Settings.MultiToolMode.Cutter) ? 8 : 0) + (flag3 ? 1 : 0)].Value;
				Texture2D texture2D = null;
				switch (i)
				{
				case 0:
				case 1:
				case 2:
				case 3:
					texture2D = TextureAssets.WireUi[2 + i].Value;
					break;
				case 4:
					texture2D = TextureAssets.WireUi[Settings.ToolMode.HasFlag(Settings.MultiToolMode.Cutter) ? 7 : 6].Value;
					break;
				case 5:
					texture2D = TextureAssets.WireUi[10].Value;
					break;
				}
				Color color = Color.White;
				Color color2 = Color.White;
				if (!flag2 && i != 4)
				{
					if (flag3)
					{
						((Color)(ref color2))._002Ector(100, 100, 100);
						((Color)(ref color2))._002Ector(120, 120, 120);
						((Color)(ref color))._002Ector(200, 200, 200);
					}
					else
					{
						((Color)(ref color2))._002Ector(150, 150, 150);
						((Color)(ref color2))._002Ector(80, 80, 80);
						((Color)(ref color))._002Ector(100, 100, 100);
					}
				}
				Utils.CenteredRectangle(vector4, new Vector2(40f));
				if (flag3)
				{
					if (Main.mouseLeft && Main.mouseLeftRelease)
					{
						switch (i)
						{
						case 0:
							Settings.ToolMode ^= Settings.MultiToolMode.Red;
							break;
						case 1:
							Settings.ToolMode ^= Settings.MultiToolMode.Green;
							break;
						case 2:
							Settings.ToolMode ^= Settings.MultiToolMode.Blue;
							break;
						case 3:
							Settings.ToolMode ^= Settings.MultiToolMode.Yellow;
							break;
						case 4:
							Settings.ToolMode ^= Settings.MultiToolMode.Cutter;
							break;
						case 5:
							Settings.ToolMode ^= Settings.MultiToolMode.Actuator;
							break;
						}
					}
					Main.player[Main.myPlayer].mouseInterface = true;
					OnWiresMenu = true;
				}
				spriteBatch.Draw(value2, vector4, (Rectangle?)null, color, 0f, value2.Size() / 2f, 1f, (SpriteEffects)0, 0f);
				spriteBatch.Draw(texture2D, vector4, (Rectangle?)null, color2, 0f, texture2D.Size() / 2f, 1f, (SpriteEffects)0, 0f);
			}
			if (Main.mouseLeft && Main.mouseLeftRelease && !flag)
			{
				active = false;
			}
		}

		private void DrawCursorArea(SpriteBatch spriteBatch)
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0116: Unknown result type (might be due to invalid IL or missing references)
			//IL_0117: Unknown result type (might be due to invalid IL or missing references)
			//IL_012b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0130: Unknown result type (might be due to invalid IL or missing references)
			//IL_0135: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_022b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0230: Unknown result type (might be due to invalid IL or missing references)
			//IL_0236: Unknown result type (might be due to invalid IL or missing references)
			//IL_0238: Unknown result type (might be due to invalid IL or missing references)
			//IL_0242: Unknown result type (might be due to invalid IL or missing references)
			//IL_0247: Unknown result type (might be due to invalid IL or missing references)
			//IL_0329: Unknown result type (might be due to invalid IL or missing references)
			//IL_033a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0341: Unknown result type (might be due to invalid IL or missing references)
			//IL_0346: Unknown result type (might be due to invalid IL or missing references)
			//IL_034b: Unknown result type (might be due to invalid IL or missing references)
			//IL_044e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0459: Unknown result type (might be due to invalid IL or missing references)
			//IL_0460: Unknown result type (might be due to invalid IL or missing references)
			//IL_0467: Unknown result type (might be due to invalid IL or missing references)
			//IL_046c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0471: Unknown result type (might be due to invalid IL or missing references)
			//IL_049c: Unknown result type (might be due to invalid IL or missing references)
			//IL_04a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_04b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_04bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_04c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_04d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_04dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0475: Unknown result type (might be due to invalid IL or missing references)
			//IL_0480: Unknown result type (might be due to invalid IL or missing references)
			//IL_0487: Unknown result type (might be due to invalid IL or missing references)
			//IL_048e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0493: Unknown result type (might be due to invalid IL or missing references)
			//IL_0498: Unknown result type (might be due to invalid IL or missing references)
			//IL_037c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0387: Unknown result type (might be due to invalid IL or missing references)
			//IL_038e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0393: Unknown result type (might be due to invalid IL or missing references)
			//IL_0398: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_03cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_03de: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_03fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0401: Unknown result type (might be due to invalid IL or missing references)
			//IL_039f: Unknown result type (might be due to invalid IL or missing references)
			//IL_03aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_03bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_04ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_04f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_04f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_04fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0500: Unknown result type (might be due to invalid IL or missing references)
			//IL_050a: Unknown result type (might be due to invalid IL or missing references)
			//IL_050c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0516: Unknown result type (might be due to invalid IL or missing references)
			//IL_052a: Unknown result type (might be due to invalid IL or missing references)
			//IL_052c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0533: Unknown result type (might be due to invalid IL or missing references)
			//IL_0537: Unknown result type (might be due to invalid IL or missing references)
			//IL_0541: Unknown result type (might be due to invalid IL or missing references)
			//IL_0543: Unknown result type (might be due to invalid IL or missing references)
			//IL_054d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0408: Unknown result type (might be due to invalid IL or missing references)
			//IL_0413: Unknown result type (might be due to invalid IL or missing references)
			//IL_041a: Unknown result type (might be due to invalid IL or missing references)
			//IL_041f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0424: Unknown result type (might be due to invalid IL or missing references)
			//IL_027a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0285: Unknown result type (might be due to invalid IL or missing references)
			//IL_028c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0291: Unknown result type (might be due to invalid IL or missing references)
			//IL_0296: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_029d: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02af: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0306: Unknown result type (might be due to invalid IL or missing references)
			//IL_0311: Unknown result type (might be due to invalid IL or missing references)
			//IL_0318: Unknown result type (might be due to invalid IL or missing references)
			//IL_031d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0322: Unknown result type (might be due to invalid IL or missing references)
			//IL_0579: Unknown result type (might be due to invalid IL or missing references)
			//IL_057b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0582: Unknown result type (might be due to invalid IL or missing references)
			//IL_0585: Unknown result type (might be due to invalid IL or missing references)
			//IL_058f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0591: Unknown result type (might be due to invalid IL or missing references)
			//IL_059b: Unknown result type (might be due to invalid IL or missing references)
			if (active || _lineOpacity == 0f)
			{
				return;
			}
			Vector2 vector = Main.MouseScreen + new Vector2((float)(10 - 9 * PlayerInput.UsingGamepad.ToInt()), 25f);
			Color color = default(Color);
			((Color)(ref color))._002Ector(50, 50, 50);
			bool drawToolAllowActuators = Settings.DrawToolAllowActuators;
			if (!drawToolAllowActuators)
			{
				vector = (PlayerInput.UsingGamepad ? (vector + new Vector2(0f, 10f)) : (vector + new Vector2(-20f, 10f)));
			}
			Texture2D value = TextureAssets.BuilderAcc.Value;
			Texture2D texture = value;
			Rectangle rectangle = default(Rectangle);
			((Rectangle)(ref rectangle))._002Ector(140, 2, 6, 6);
			Rectangle rectangle2 = default(Rectangle);
			((Rectangle)(ref rectangle2))._002Ector(148, 2, 6, 6);
			Rectangle rectangle3 = default(Rectangle);
			((Rectangle)(ref rectangle3))._002Ector(128, 0, 10, 10);
			float num = 1f;
			float scale = 1f;
			bool flag = false;
			if (flag && !drawToolAllowActuators)
			{
				num *= Main.cursorScale;
			}
			float num2 = _lineOpacity;
			if (PlayerInput.UsingGamepad)
			{
				num2 *= Main.GamepadCursorAlpha;
			}
			for (int i = 0; i < 5; i++)
			{
				if (!drawToolAllowActuators && i == 4)
				{
					continue;
				}
				float num3 = num2;
				Vector2 vec = vector + Vector2.UnitX * (45f * ((float)i - 1.5f));
				int num4 = i;
				if (i == 0)
				{
					num4 = 3;
				}
				if (i == 1)
				{
					num4 = 2;
				}
				if (i == 2)
				{
					num4 = 1;
				}
				if (i == 3)
				{
					num4 = 0;
				}
				if (i == 4)
				{
					num4 = 5;
				}
				int num5 = num4;
				switch (num5)
				{
				case 2:
					num5 = 1;
					break;
				case 1:
					num5 = 2;
					break;
				}
				bool flag2 = Settings.ToolMode.HasFlag((Settings.MultiToolMode)(1 << num5));
				if (num5 == 5)
				{
					flag2 = Settings.ToolMode.HasFlag(Settings.MultiToolMode.Actuator);
				}
				Color color2 = Color.HotPink;
				switch (num4)
				{
				case 0:
					((Color)(ref color2))._002Ector(253, 58, 61);
					break;
				case 1:
					((Color)(ref color2))._002Ector(83, 180, 253);
					break;
				case 2:
					((Color)(ref color2))._002Ector(83, 253, 153);
					break;
				case 3:
					((Color)(ref color2))._002Ector(253, 254, 83);
					break;
				case 5:
					color2 = Color.WhiteSmoke;
					break;
				}
				if (!flag2)
				{
					color2 = Color.Lerp(color2, Color.Black, 0.65f);
				}
				if (flag)
				{
					if (drawToolAllowActuators)
					{
						switch (num4)
						{
						case 0:
							vec = vector + new Vector2(-12f, 0f) * num;
							break;
						case 3:
							vec = vector + new Vector2(12f, 0f) * num;
							break;
						case 1:
							vec = vector + new Vector2(-6f, 12f) * num;
							break;
						case 2:
							vec = vector + new Vector2(6f, 12f) * num;
							break;
						case 5:
							vec = vector + new Vector2(0f, 0f) * num;
							break;
						}
					}
					else
					{
						vec = vector + new Vector2((float)(12 * (num4 + 1)), (float)(12 * (3 - num4))) * num;
					}
				}
				else if (drawToolAllowActuators)
				{
					switch (num4)
					{
					case 0:
						vec = vector + new Vector2(-12f, 0f) * num;
						break;
					case 3:
						vec = vector + new Vector2(12f, 0f) * num;
						break;
					case 1:
						vec = vector + new Vector2(-6f, 12f) * num;
						break;
					case 2:
						vec = vector + new Vector2(6f, 12f) * num;
						break;
					case 5:
						vec = vector + new Vector2(0f, 0f) * num;
						break;
					}
				}
				else
				{
					float num6 = 0.7f;
					switch (num4)
					{
					case 0:
						vec = vector + new Vector2(0f, -12f) * num * num6;
						break;
					case 3:
						vec = vector + new Vector2(12f, 0f) * num * num6;
						break;
					case 1:
						vec = vector + new Vector2(-12f, 0f) * num * num6;
						break;
					case 2:
						vec = vector + new Vector2(0f, 12f) * num * num6;
						break;
					}
				}
				vec = vec.Floor();
				spriteBatch.Draw(texture, vec, (Rectangle?)rectangle3, color * num3, 0f, rectangle3.Size() / 2f, scale, (SpriteEffects)0, 0f);
				spriteBatch.Draw(value, vec, (Rectangle?)rectangle, color2 * num3, 0f, rectangle.Size() / 2f, scale, (SpriteEffects)0, 0f);
				if (Settings.ToolMode.HasFlag(Settings.MultiToolMode.Cutter))
				{
					spriteBatch.Draw(value, vec, (Rectangle?)rectangle2, color * num3, 0f, rectangle2.Size() / 2f, scale, (SpriteEffects)0, 0f);
				}
			}
		}
	}

	private static WiresRadial radial = new WiresRadial();

	public static bool Open => radial.active;

	public static void HandleWiresUI(SpriteBatch spriteBatch)
	{
		radial.Update();
		radial.Draw(spriteBatch);
	}
}
