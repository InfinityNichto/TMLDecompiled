using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Events;
using Terraria.GameContent.UI;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Terraria;

public static class Wiring
{
	public static bool blockPlayerTeleportationForOneIteration;

	/// <summary>
	/// True while wiring pulse code is running, which happens in <see cref="M:Terraria.Wiring.TripWire(System.Int32,System.Int32,System.Int32,System.Int32)" />. Check this before calling <see cref="M:Terraria.Wiring.SkipWire(System.Int32,System.Int32)" /> in any code that is shared between wiring and other interactions to prevent buggy behavior.<br /><br />
	/// For example, the code in <see href="https://github.com/tModLoader/tModLoader/blob/stable/ExampleMod/Content/Tiles/ExampleCampfire.cs#L97">ExampleCampfire</see> needs to check <see cref="F:Terraria.Wiring.running" /> because the code is shared between wiring and right click interactions. 
	/// </summary>
	public static bool running;

	private static Dictionary<Point16, bool> _wireSkip;

	public static DoubleStack<Point16> _wireList;

	public static DoubleStack<byte> _wireDirectionList;

	public static Dictionary<Point16, byte> _toProcess;

	private static Queue<Point16> _GatesCurrent;

	public static Queue<Point16> _LampsToCheck;

	public static Queue<Point16> _GatesNext;

	private static Dictionary<Point16, bool> _GatesDone;

	private static Dictionary<Point16, byte> _PixelBoxTriggers;

	public static Vector2[] _teleport;

	private const int MaxPump = 20;

	public static int[] _inPumpX;

	public static int[] _inPumpY;

	public static int _numInPump;

	public static int[] _outPumpX;

	public static int[] _outPumpY;

	public static int _numOutPump;

	private const int MaxMech = 1000;

	private static int[] _mechX;

	private static int[] _mechY;

	private static int _numMechs;

	private static int[] _mechTime;

	public static int _currentWireColor;

	private static int CurrentUser = 255;

	public static void SetCurrentUser(int plr = -1)
	{
		if (plr < 0 || plr > 255)
		{
			plr = 255;
		}
		if (Main.netMode == 0)
		{
			plr = Main.myPlayer;
		}
		CurrentUser = plr;
	}

	public static void Initialize()
	{
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		_wireSkip = new Dictionary<Point16, bool>();
		_wireList = new DoubleStack<Point16>();
		_wireDirectionList = new DoubleStack<byte>();
		_toProcess = new Dictionary<Point16, byte>();
		_GatesCurrent = new Queue<Point16>();
		_GatesNext = new Queue<Point16>();
		_GatesDone = new Dictionary<Point16, bool>();
		_LampsToCheck = new Queue<Point16>();
		_PixelBoxTriggers = new Dictionary<Point16, byte>();
		_inPumpX = new int[20];
		_inPumpY = new int[20];
		_outPumpX = new int[20];
		_outPumpY = new int[20];
		_teleport = (Vector2[])(object)new Vector2[2]
		{
			Vector2.One * -1f,
			Vector2.One * -1f
		};
		_mechX = new int[1000];
		_mechY = new int[1000];
		_mechTime = new int[1000];
	}

	/// <summary>
	/// Use to prevent wire signals from running for the provided coordinates. Typically used in multi-tiles to prevent a wire touching multiple tiles of the multi-tile from erroneously running wire code multiple times. In <see cref="M:Terraria.ModLoader.ModTile.HitWire(System.Int32,System.Int32)" />, call SkipWire for all the coordinates the tile covers.
	/// </summary>
	/// <param name="x"></param>
	/// <param name="y"></param>
	public static void SkipWire(int x, int y)
	{
		_wireSkip[new Point16(x, y)] = true;
	}

	/// <inheritdoc cref="M:Terraria.Wiring.SkipWire(System.Int32,System.Int32)" />
	public static void SkipWire(Point16 point)
	{
		_wireSkip[point] = true;
	}

	public static void ClearAll()
	{
		for (int i = 0; i < 20; i++)
		{
			_inPumpX[i] = 0;
			_inPumpY[i] = 0;
			_outPumpX[i] = 0;
			_outPumpY[i] = 0;
		}
		_numInPump = 0;
		_numOutPump = 0;
		for (int j = 0; j < 1000; j++)
		{
			_mechTime[j] = 0;
			_mechX[j] = 0;
			_mechY[j] = 0;
		}
		_numMechs = 0;
	}

	public static void UpdateMech()
	{
		SetCurrentUser();
		for (int num = _numMechs - 1; num >= 0; num--)
		{
			_mechTime[num]--;
			int num2 = _mechX[num];
			int num3 = _mechY[num];
			if (!WorldGen.InWorld(num2, num3, 1))
			{
				_numMechs--;
			}
			else
			{
				Tile tile = Main.tile[num2, num3];
				if (tile == null)
				{
					_numMechs--;
				}
				else
				{
					if (tile.active() && tile.type == 144)
					{
						if (tile.frameY == 0)
						{
							_mechTime[num] = 0;
						}
						else
						{
							int num4 = tile.frameX / 18;
							switch (num4)
							{
							case 0:
								num4 = 60;
								break;
							case 1:
								num4 = 180;
								break;
							case 2:
								num4 = 300;
								break;
							case 3:
								num4 = 30;
								break;
							case 4:
								num4 = 15;
								break;
							}
							if (Math.IEEERemainder(_mechTime[num], num4) == 0.0)
							{
								_mechTime[num] = 18000;
								TripWire(_mechX[num], _mechY[num], 1, 1);
							}
						}
					}
					if (_mechTime[num] <= 0)
					{
						if (tile.active() && tile.type == 144)
						{
							tile.frameY = 0;
							NetMessage.SendTileSquare(-1, _mechX[num], _mechY[num]);
						}
						if (tile.active() && tile.type == 411)
						{
							int num5 = tile.frameX % 36 / 18;
							int num6 = tile.frameY % 36 / 18;
							int num7 = _mechX[num] - num5;
							int num8 = _mechY[num] - num6;
							int num9 = 36;
							if (Main.tile[num7, num8].frameX >= 36)
							{
								num9 = -36;
							}
							for (int i = num7; i < num7 + 2; i++)
							{
								for (int j = num8; j < num8 + 2; j++)
								{
									if (WorldGen.InWorld(i, j, 1))
									{
										Tile tile2 = Main.tile[i, j];
										if (tile2 != null)
										{
											tile2.frameX = (short)(tile2.frameX + num9);
										}
									}
								}
							}
							NetMessage.SendTileSquare(-1, num7, num8, 2, 2);
						}
						for (int k = num; k < _numMechs; k++)
						{
							_mechX[k] = _mechX[k + 1];
							_mechY[k] = _mechY[k + 1];
							_mechTime[k] = _mechTime[k + 1];
						}
						_numMechs--;
					}
				}
			}
		}
	}

	public static void HitSwitch(int i, int j)
	{
		if (!WorldGen.InWorld(i, j) || Main.tile[i, j] == null)
		{
			return;
		}
		Tile tile = Main.tile[i, j];
		if (tile.type != 135)
		{
			tile = Main.tile[i, j];
			if (tile.type != 314)
			{
				tile = Main.tile[i, j];
				if (tile.type != 423)
				{
					tile = Main.tile[i, j];
					if (tile.type != 428)
					{
						tile = Main.tile[i, j];
						if (tile.type != 442)
						{
							tile = Main.tile[i, j];
							if (tile.type != 476)
							{
								tile = Main.tile[i, j];
								if (tile.type == 440)
								{
									SoundEngine.PlaySound(28, i * 16 + 16, j * 16 + 16, 0);
									TripWire(i, j, 3, 3);
									return;
								}
								tile = Main.tile[i, j];
								if (tile.type == 136)
								{
									tile = Main.tile[i, j];
									if (tile.frameY == 0)
									{
										tile = Main.tile[i, j];
										tile.frameY = 18;
									}
									else
									{
										tile = Main.tile[i, j];
										tile.frameY = 0;
									}
									SoundEngine.PlaySound(28, i * 16, j * 16, 0);
									TripWire(i, j, 1, 1);
									return;
								}
								tile = Main.tile[i, j];
								if (tile.type == 443)
								{
									GeyserTrap(i, j);
									return;
								}
								tile = Main.tile[i, j];
								if (tile.type == 144)
								{
									tile = Main.tile[i, j];
									if (tile.frameY == 0)
									{
										tile = Main.tile[i, j];
										tile.frameY = 18;
										if (Main.netMode != 1)
										{
											CheckMech(i, j, 18000);
										}
									}
									else
									{
										tile = Main.tile[i, j];
										tile.frameY = 0;
									}
									SoundEngine.PlaySound(28, i * 16, j * 16, 0);
									return;
								}
								tile = Main.tile[i, j];
								if (tile.type != 441)
								{
									tile = Main.tile[i, j];
									if (tile.type != 468)
									{
										tile = Main.tile[i, j];
										if (tile.type == 467)
										{
											tile = Main.tile[i, j];
											if (tile.frameX / 36 == 4)
											{
												tile = Main.tile[i, j];
												int num3 = tile.frameX / 18 * -1;
												tile = Main.tile[i, j];
												int num4 = tile.frameY / 18 * -1;
												num3 %= 4;
												if (num3 < -1)
												{
													num3 += 2;
												}
												num3 += i;
												num4 += j;
												SoundEngine.PlaySound(28, i * 16, j * 16, 0);
												TripWire(num3, num4, 2, 2);
											}
											return;
										}
										tile = Main.tile[i, j];
										if (tile.type != 132)
										{
											tile = Main.tile[i, j];
											if (tile.type != 411)
											{
												return;
											}
										}
										short num5 = 36;
										tile = Main.tile[i, j];
										int num6 = tile.frameX / 18 * -1;
										tile = Main.tile[i, j];
										int num7 = tile.frameY / 18 * -1;
										num6 %= 4;
										if (num6 < -1)
										{
											num6 += 2;
											num5 = -36;
										}
										num6 += i;
										num7 += j;
										if (Main.netMode != 1)
										{
											tile = Main.tile[num6, num7];
											if (tile.type == 411)
											{
												CheckMech(num6, num7, 60);
											}
										}
										for (int k = num6; k < num6 + 2; k++)
										{
											for (int l = num7; l < num7 + 2; l++)
											{
												tile = Main.tile[k, l];
												if (tile.type != 132)
												{
													tile = Main.tile[k, l];
													if (tile.type != 411)
													{
														continue;
													}
												}
												tile = Main.tile[k, l];
												tile.frameX += num5;
											}
										}
										WorldGen.TileFrame(num6, num7);
										SoundEngine.PlaySound(28, i * 16, j * 16, 0);
										TripWire(num6, num7, 2, 2);
										return;
									}
								}
								tile = Main.tile[i, j];
								int num = tile.frameX / 18 * -1;
								tile = Main.tile[i, j];
								int num2 = tile.frameY / 18 * -1;
								num %= 4;
								if (num < -1)
								{
									num += 2;
								}
								num += i;
								num2 += j;
								SoundEngine.PlaySound(28, i * 16, j * 16, 0);
								TripWire(num, num2, 2, 2);
								return;
							}
						}
					}
				}
			}
		}
		SoundEngine.PlaySound(28, i * 16, j * 16, 0);
		TripWire(i, j, 1, 1);
	}

	public static void PokeLogicGate(int lampX, int lampY)
	{
		if (Main.netMode != 1)
		{
			_LampsToCheck.Enqueue(new Point16(lampX, lampY));
			LogicGatePass();
		}
	}

	public static bool Actuate(int i, int j)
	{
		Tile tile = Main.tile[i, j];
		if (!tile.actuator())
		{
			return false;
		}
		if (tile.inActive())
		{
			ReActive(i, j);
		}
		else
		{
			DeActive(i, j);
		}
		return true;
	}

	public static void ActuateForced(int i, int j)
	{
		if (Main.tile[i, j].inActive())
		{
			ReActive(i, j);
		}
		else
		{
			DeActive(i, j);
		}
	}

	public static void MassWireOperation(Point ps, Point pe, Player master)
	{
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		int wireCount = 0;
		int actuatorCount = 0;
		for (int i = 0; i < 58; i++)
		{
			if (master.inventory[i].type == 530)
			{
				wireCount += master.inventory[i].stack;
			}
			if (master.inventory[i].type == 849)
			{
				actuatorCount += master.inventory[i].stack;
			}
		}
		int num5 = wireCount;
		int num2 = actuatorCount;
		MassWireOperationInner(master, ps, pe, master.Center, master.direction == 1, ref wireCount, ref actuatorCount);
		int num3 = num5 - wireCount;
		int num4 = num2 - actuatorCount;
		if (Main.netMode == 2)
		{
			NetMessage.SendData(110, master.whoAmI, -1, null, 530, num3, master.whoAmI);
			NetMessage.SendData(110, master.whoAmI, -1, null, 849, num4, master.whoAmI);
			return;
		}
		for (int j = 0; j < num3; j++)
		{
			master.ConsumeItem(530);
		}
		for (int k = 0; k < num4; k++)
		{
			master.ConsumeItem(849);
		}
	}

	/// <summary>
	/// Returns true if the wiring cooldown has been reached for the provided tile coordinates. The <paramref name="time" /> parameter will set the cooldown if wiring cooldown has elapsed. Use larger values for less frequent activations. Use this method to limit how often mechanisms can be triggerd. <see cref="M:Terraria.Item.MechSpawn(System.Single,System.Single,System.Int32)" /> and <see cref="M:Terraria.NPC.MechSpawn(System.Single,System.Single,System.Int32)" /> should also be checked for mechanisms spawning items or NPC, such as Statues.
	/// </summary>
	/// <param name="i"></param>
	/// <param name="j"></param>
	/// <param name="time"></param>
	/// <returns></returns>
	public static bool CheckMech(int i, int j, int time)
	{
		for (int k = 0; k < _numMechs; k++)
		{
			if (_mechX[k] == i && _mechY[k] == j)
			{
				return false;
			}
		}
		if (_numMechs < 999)
		{
			_mechX[_numMechs] = i;
			_mechY[_numMechs] = j;
			_mechTime[_numMechs] = time;
			_numMechs++;
			return true;
		}
		return false;
	}

	private static void XferWater()
	{
		for (int i = 0; i < _numInPump; i++)
		{
			int num = _inPumpX[i];
			int num2 = _inPumpY[i];
			Tile tile = Main.tile[num, num2];
			int liquid = tile.liquid;
			if (liquid <= 0)
			{
				continue;
			}
			tile = Main.tile[num, num2];
			byte b = tile.liquidType();
			for (int j = 0; j < _numOutPump; j++)
			{
				int num3 = _outPumpX[j];
				int num4 = _outPumpY[j];
				tile = Main.tile[num3, num4];
				int liquid2 = tile.liquid;
				if (liquid2 >= 255)
				{
					continue;
				}
				tile = Main.tile[num3, num4];
				byte b2 = tile.liquidType();
				if (liquid2 == 0)
				{
					b2 = b;
				}
				if (b2 == b)
				{
					int num5 = liquid;
					if (num5 + liquid2 > 255)
					{
						num5 = 255 - liquid2;
					}
					tile = Main.tile[num3, num4];
					tile.liquid += (byte)num5;
					tile = Main.tile[num, num2];
					tile.liquid -= (byte)num5;
					tile = Main.tile[num, num2];
					liquid = tile.liquid;
					tile = Main.tile[num3, num4];
					tile.liquidType(b);
					WorldGen.SquareTileFrame(num3, num4);
					tile = Main.tile[num, num2];
					if (tile.liquid == 0)
					{
						tile = Main.tile[num, num2];
						tile.liquidType(0);
						WorldGen.SquareTileFrame(num, num2);
						break;
					}
				}
			}
			WorldGen.SquareTileFrame(num, num2);
		}
	}

	/// <summary>
	/// Used to send a single to wiring wired up to the specified area. The parameters represent the tile coordinates where wire signals will be sent. Mechanism tiles such as <see cref="F:Terraria.ID.TileID.Switches" />, <see cref="F:Terraria.ID.TileID.PressurePlates" />, <see cref="F:Terraria.ID.TileID.Timers" />, and <see cref="F:Terraria.ID.TileID.LogicSensor" /> use this method as part of their interaction code.
	/// </summary>
	/// <param name="left"></param>
	/// <param name="top"></param>
	/// <param name="width"></param>
	/// <param name="height"></param>
	public static void TripWire(int left, int top, int width, int height)
	{
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0248: Unknown result type (might be due to invalid IL or missing references)
		//IL_024d: Unknown result type (might be due to invalid IL or missing references)
		//IL_025e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0263: Unknown result type (might be due to invalid IL or missing references)
		//IL_035f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0364: Unknown result type (might be due to invalid IL or missing references)
		//IL_0375: Unknown result type (might be due to invalid IL or missing references)
		//IL_037a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0476: Unknown result type (might be due to invalid IL or missing references)
		//IL_047b: Unknown result type (might be due to invalid IL or missing references)
		//IL_048c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0491: Unknown result type (might be due to invalid IL or missing references)
		//IL_04aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_04af: Unknown result type (might be due to invalid IL or missing references)
		//IL_04bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c4: Unknown result type (might be due to invalid IL or missing references)
		if (Main.netMode == 1)
		{
			return;
		}
		running = true;
		if (_wireList.Count != 0)
		{
			_wireList.Clear(quickClear: true);
		}
		if (_wireDirectionList.Count != 0)
		{
			_wireDirectionList.Clear(quickClear: true);
		}
		Vector2[] array = (Vector2[])(object)new Vector2[8];
		int num = 0;
		for (int i = left; i < left + width; i++)
		{
			for (int j = top; j < top + height; j++)
			{
				Point16 back3 = new Point16(i, j);
				Tile tile = Main.tile[i, j];
				if (tile != null && tile.wire())
				{
					_wireList.PushBack(back3);
				}
			}
		}
		_teleport[0].X = -1f;
		_teleport[0].Y = -1f;
		_teleport[1].X = -1f;
		_teleport[1].Y = -1f;
		if (_wireList.Count > 0)
		{
			_numInPump = 0;
			_numOutPump = 0;
			HitWire(_wireList, 1);
			if (_numInPump > 0 && _numOutPump > 0)
			{
				XferWater();
			}
		}
		array[num++] = _teleport[0];
		array[num++] = _teleport[1];
		for (int k = left; k < left + width; k++)
		{
			for (int l = top; l < top + height; l++)
			{
				Point16 back4 = new Point16(k, l);
				Tile tile2 = Main.tile[k, l];
				if (tile2 != null && tile2.wire2())
				{
					_wireList.PushBack(back4);
				}
			}
		}
		_teleport[0].X = -1f;
		_teleport[0].Y = -1f;
		_teleport[1].X = -1f;
		_teleport[1].Y = -1f;
		if (_wireList.Count > 0)
		{
			_numInPump = 0;
			_numOutPump = 0;
			HitWire(_wireList, 2);
			if (_numInPump > 0 && _numOutPump > 0)
			{
				XferWater();
			}
		}
		array[num++] = _teleport[0];
		array[num++] = _teleport[1];
		_teleport[0].X = -1f;
		_teleport[0].Y = -1f;
		_teleport[1].X = -1f;
		_teleport[1].Y = -1f;
		for (int m = left; m < left + width; m++)
		{
			for (int n = top; n < top + height; n++)
			{
				Point16 back2 = new Point16(m, n);
				Tile tile3 = Main.tile[m, n];
				if (tile3 != null && tile3.wire3())
				{
					_wireList.PushBack(back2);
				}
			}
		}
		if (_wireList.Count > 0)
		{
			_numInPump = 0;
			_numOutPump = 0;
			HitWire(_wireList, 3);
			if (_numInPump > 0 && _numOutPump > 0)
			{
				XferWater();
			}
		}
		array[num++] = _teleport[0];
		array[num++] = _teleport[1];
		_teleport[0].X = -1f;
		_teleport[0].Y = -1f;
		_teleport[1].X = -1f;
		_teleport[1].Y = -1f;
		for (int num2 = left; num2 < left + width; num2++)
		{
			for (int num3 = top; num3 < top + height; num3++)
			{
				Point16 back = new Point16(num2, num3);
				Tile tile4 = Main.tile[num2, num3];
				if (tile4 != null && tile4.wire4())
				{
					_wireList.PushBack(back);
				}
			}
		}
		if (_wireList.Count > 0)
		{
			_numInPump = 0;
			_numOutPump = 0;
			HitWire(_wireList, 4);
			if (_numInPump > 0 && _numOutPump > 0)
			{
				XferWater();
			}
		}
		array[num++] = _teleport[0];
		array[num++] = _teleport[1];
		running = false;
		for (int num4 = 0; num4 < 8; num4 += 2)
		{
			_teleport[0] = array[num4];
			_teleport[1] = array[num4 + 1];
			if (_teleport[0].X >= 0f && _teleport[1].X >= 0f)
			{
				Teleport();
			}
		}
		PixelBoxPass();
		LogicGatePass();
	}

	private static void PixelBoxPass()
	{
		foreach (KeyValuePair<Point16, byte> pixelBoxTrigger in _PixelBoxTriggers)
		{
			if (pixelBoxTrigger.Value == 3)
			{
				Tile tile = Main.tile[pixelBoxTrigger.Key.X, pixelBoxTrigger.Key.Y];
				tile.frameX = (short)((tile.frameX != 18) ? 18 : 0);
				NetMessage.SendTileSquare(-1, pixelBoxTrigger.Key.X, pixelBoxTrigger.Key.Y);
			}
		}
		_PixelBoxTriggers.Clear();
	}

	private static void LogicGatePass()
	{
		if (_GatesCurrent.Count != 0)
		{
			return;
		}
		_GatesDone.Clear();
		while (_LampsToCheck.Count > 0)
		{
			while (_LampsToCheck.Count > 0)
			{
				Point16 point = _LampsToCheck.Dequeue();
				CheckLogicGate(point.X, point.Y);
			}
			while (_GatesNext.Count > 0)
			{
				Utils.Swap(ref _GatesCurrent, ref _GatesNext);
				while (_GatesCurrent.Count > 0)
				{
					Point16 key = _GatesCurrent.Peek();
					if (_GatesDone.TryGetValue(key, out var value) && value)
					{
						_GatesCurrent.Dequeue();
						continue;
					}
					_GatesDone.Add(key, value: true);
					TripWire(key.X, key.Y, 1, 1);
					_GatesCurrent.Dequeue();
				}
			}
		}
		_GatesDone.Clear();
		if (blockPlayerTeleportationForOneIteration)
		{
			blockPlayerTeleportationForOneIteration = false;
		}
	}

	private static void CheckLogicGate(int lampX, int lampY)
	{
		//IL_0227: Unknown result type (might be due to invalid IL or missing references)
		//IL_0231: Unknown result type (might be due to invalid IL or missing references)
		//IL_023b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0240: Unknown result type (might be due to invalid IL or missing references)
		//IL_0245: Unknown result type (might be due to invalid IL or missing references)
		//IL_0247: Unknown result type (might be due to invalid IL or missing references)
		//IL_0253: Unknown result type (might be due to invalid IL or missing references)
		//IL_025b: Unknown result type (might be due to invalid IL or missing references)
		if (!WorldGen.InWorld(lampX, lampY, 1))
		{
			return;
		}
		for (int i = lampY; i < Main.maxTilesY; i++)
		{
			Tile tile = Main.tile[lampX, i];
			if (!tile.active())
			{
				break;
			}
			if (tile.type == 420)
			{
				_GatesDone.TryGetValue(new Point16(lampX, i), out var value);
				int num = tile.frameY / 18;
				bool flag = tile.frameX == 18;
				bool flag2 = tile.frameX == 36;
				if (num < 0)
				{
					break;
				}
				int num2 = 0;
				int num3 = 0;
				bool flag3 = false;
				for (int num4 = i - 1; num4 > 0; num4--)
				{
					Tile tile2 = Main.tile[lampX, num4];
					if (!tile2.active() || tile2.type != 419)
					{
						break;
					}
					if (tile2.frameX == 36)
					{
						flag3 = true;
						break;
					}
					num2++;
					num3 += (tile2.frameX == 18).ToInt();
				}
				bool flag4 = false;
				switch (num)
				{
				default:
					return;
				case 0:
					flag4 = num2 == num3;
					break;
				case 2:
					flag4 = num2 != num3;
					break;
				case 1:
					flag4 = num3 > 0;
					break;
				case 3:
					flag4 = num3 == 0;
					break;
				case 4:
					flag4 = num3 == 1;
					break;
				case 5:
					flag4 = num3 != 1;
					break;
				}
				bool flag5 = !flag3 && flag2;
				bool flag6 = false;
				if (flag3 && Framing.GetTileSafely(lampX, lampY).frameX == 36)
				{
					flag6 = true;
				}
				if (!(flag4 != flag || flag5 || flag6))
				{
					break;
				}
				_ = tile.frameX % 18 / 18;
				tile.frameX = (short)(18 * flag4.ToInt());
				if (flag3)
				{
					tile.frameX = 36;
				}
				SkipWire(lampX, i);
				WorldGen.SquareTileFrame(lampX, i);
				NetMessage.SendTileSquare(-1, lampX, i);
				bool flag7 = !flag3 || flag6;
				if (flag6)
				{
					if (num3 == 0 || num2 == 0)
					{
						flag7 = false;
					}
					flag7 = Main.rand.NextFloat() < (float)num3 / (float)num2;
				}
				if (flag5)
				{
					flag7 = false;
				}
				if (flag7)
				{
					if (!value)
					{
						_GatesNext.Enqueue(new Point16(lampX, i));
						break;
					}
					Vector2 position = new Vector2((float)lampX, (float)i) * 16f - new Vector2(10f);
					Utils.PoofOfSmoke(position);
					NetMessage.SendData(106, -1, -1, null, (int)position.X, position.Y);
				}
				break;
			}
			if (tile.type != 419)
			{
				break;
			}
		}
	}

	private static void HitWire(DoubleStack<Point16> next, int wireType)
	{
		_wireDirectionList.Clear(quickClear: true);
		for (int i = 0; i < next.Count; i++)
		{
			Point16 point = next.PopFront();
			SkipWire(point);
			_toProcess.Add(point, 4);
			next.PushBack(point);
			_wireDirectionList.PushBack(0);
		}
		_currentWireColor = wireType;
		while (next.Count > 0)
		{
			Point16 key = next.PopFront();
			int num = _wireDirectionList.PopFront();
			int x = key.X;
			int y = key.Y;
			if (!_wireSkip.ContainsKey(key))
			{
				HitWireSingle(x, y);
			}
			for (int j = 0; j < 4; j++)
			{
				int num2;
				int num3;
				switch (j)
				{
				case 0:
					num2 = x;
					num3 = y + 1;
					break;
				case 1:
					num2 = x;
					num3 = y - 1;
					break;
				case 2:
					num2 = x + 1;
					num3 = y;
					break;
				case 3:
					num2 = x - 1;
					num3 = y;
					break;
				default:
					num2 = x;
					num3 = y + 1;
					break;
				}
				if (num2 < 2 || num2 >= Main.maxTilesX - 2 || num3 < 2 || num3 >= Main.maxTilesY - 2)
				{
					continue;
				}
				Tile tile = Main.tile[num2, num3];
				if (tile == null)
				{
					continue;
				}
				Tile tile2 = Main.tile[x, y];
				if (tile2 == null)
				{
					continue;
				}
				byte b = 3;
				if (tile.type == 424 || tile.type == 445)
				{
					b = 0;
				}
				if (tile2.type == 424)
				{
					switch (tile2.frameX / 18)
					{
					case 0:
						if (j != num)
						{
							continue;
						}
						break;
					case 1:
						if ((num != 0 || j != 3) && (num != 3 || j != 0) && (num != 1 || j != 2) && (num != 2 || j != 1))
						{
							continue;
						}
						break;
					case 2:
						if ((num != 0 || j != 2) && (num != 2 || j != 0) && (num != 1 || j != 3) && (num != 3 || j != 1))
						{
							continue;
						}
						break;
					}
				}
				if (tile2.type == 445)
				{
					if (j != num)
					{
						continue;
					}
					if (_PixelBoxTriggers.ContainsKey(key))
					{
						_PixelBoxTriggers[key] |= (byte)((j != 0 && j != 1) ? 1 : 2);
					}
					else
					{
						_PixelBoxTriggers[key] = (byte)((j != 0 && j != 1) ? 1u : 2u);
					}
				}
				if (wireType switch
				{
					1 => tile.wire() ? 1 : 0, 
					2 => tile.wire2() ? 1 : 0, 
					3 => tile.wire3() ? 1 : 0, 
					4 => tile.wire4() ? 1 : 0, 
					_ => 0, 
				} == 0)
				{
					continue;
				}
				Point16 point2 = new Point16(num2, num3);
				if (_toProcess.TryGetValue(point2, out var value))
				{
					value--;
					if (value == 0)
					{
						_toProcess.Remove(point2);
					}
					else
					{
						_toProcess[point2] = value;
					}
					continue;
				}
				next.PushBack(point2);
				_wireDirectionList.PushBack((byte)j);
				if (b > 0)
				{
					_toProcess.Add(point2, b);
				}
			}
		}
		_wireSkip.Clear();
		_toProcess.Clear();
	}

	public static IEntitySource GetProjectileSource(int sourceTileX, int sourceTileY)
	{
		return new EntitySource_Wiring(sourceTileX, sourceTileY);
	}

	public static IEntitySource GetNPCSource(int sourceTileX, int sourceTileY)
	{
		return new EntitySource_Wiring(sourceTileX, sourceTileY);
	}

	public static IEntitySource GetItemSource(int sourceTileX, int sourceTileY)
	{
		return new EntitySource_Wiring(sourceTileX, sourceTileY);
	}

	private static void HitWireSingle(int i, int j)
	{
		//IL_056e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0573: Unknown result type (might be due to invalid IL or missing references)
		//IL_063e: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0661: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ae4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aeb: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fe2: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fec: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ff1: Unknown result type (might be due to invalid IL or missing references)
		//IL_17ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_17f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_2022: Unknown result type (might be due to invalid IL or missing references)
		//IL_202b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f39: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f42: Unknown result type (might be due to invalid IL or missing references)
		//IL_1968: Unknown result type (might be due to invalid IL or missing references)
		//IL_1975: Unknown result type (might be due to invalid IL or missing references)
		//IL_197a: Unknown result type (might be due to invalid IL or missing references)
		//IL_197f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ea6: Unknown result type (might be due to invalid IL or missing references)
		//IL_1eaf: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d76: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d83: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d88: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d8d: Unknown result type (might be due to invalid IL or missing references)
		//IL_278f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2799: Unknown result type (might be due to invalid IL or missing references)
		//IL_279e: Unknown result type (might be due to invalid IL or missing references)
		//IL_27a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_27a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_27b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_27b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ad3: Unknown result type (might be due to invalid IL or missing references)
		//IL_2add: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ae2: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ae7: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ae9: Unknown result type (might be due to invalid IL or missing references)
		//IL_2af5: Unknown result type (might be due to invalid IL or missing references)
		//IL_2afd: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e74: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e7e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e83: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e88: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e8a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e96: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e9e: Unknown result type (might be due to invalid IL or missing references)
		Tile tile = Main.tile[i, j];
		bool? forcedStateWhereTrueIsOn = null;
		bool doSkipWires = true;
		int type = tile.type;
		if (tile.actuator())
		{
			ActuateForced(i, j);
		}
		if (!tile.active() || !TileLoader.PreHitWire(i, j, type))
		{
			return;
		}
		switch (type)
		{
		case 144:
			HitSwitch(i, j);
			WorldGen.SquareTileFrame(i, j);
			NetMessage.SendTileSquare(-1, i, j);
			break;
		case 421:
			if (!tile.actuator())
			{
				tile.type = 422;
				WorldGen.SquareTileFrame(i, j);
				NetMessage.SendTileSquare(-1, i, j);
			}
			break;
		case 422:
			if (!tile.actuator())
			{
				tile.type = 421;
				WorldGen.SquareTileFrame(i, j);
				NetMessage.SendTileSquare(-1, i, j);
			}
			break;
		}
		if (type >= 255 && type <= 268)
		{
			if (!tile.actuator())
			{
				if (type >= 262)
				{
					tile.type -= 7;
				}
				else
				{
					tile.type += 7;
				}
				WorldGen.SquareTileFrame(i, j);
				NetMessage.SendTileSquare(-1, i, j);
			}
			return;
		}
		Tile tile3;
		switch (type)
		{
		case 419:
		{
			int num = 18;
			if (tile.frameX >= num)
			{
				num = -num;
			}
			if (tile.frameX == 36)
			{
				num = 0;
			}
			SkipWire(i, j);
			tile.frameX = (short)(tile.frameX + num);
			WorldGen.SquareTileFrame(i, j);
			NetMessage.SendTileSquare(-1, i, j);
			_LampsToCheck.Enqueue(new Point16(i, j));
			return;
		}
		case 406:
		{
			int num68 = tile.frameX % 54 / 18;
			int num79 = tile.frameY % 54 / 18;
			int num90 = i - num68;
			int num101 = j - num79;
			int num111 = 54;
			tile3 = Main.tile[num90, num101];
			if (tile3.frameY >= 108)
			{
				num111 = -108;
			}
			for (int k = num90; k < num90 + 3; k++)
			{
				for (int l = num101; l < num101 + 3; l++)
				{
					SkipWire(k, l);
					tile3 = Main.tile[k, l];
					ref short frameY = ref tile3.frameY;
					tile3 = Main.tile[k, l];
					frameY = (short)(tile3.frameY + num111);
				}
			}
			NetMessage.SendTileSquare(-1, num90 + 1, num101 + 1, 3);
			return;
		}
		case 452:
		{
			int num120 = tile.frameX % 54 / 18;
			int num130 = tile.frameY % 54 / 18;
			int num141 = i - num120;
			int num2 = j - num130;
			int num13 = 54;
			tile3 = Main.tile[num141, num2];
			if (tile3.frameX >= 54)
			{
				num13 = -54;
			}
			for (int m = num141; m < num141 + 3; m++)
			{
				for (int n = num2; n < num2 + 3; n++)
				{
					SkipWire(m, n);
					tile3 = Main.tile[m, n];
					ref short frameX3 = ref tile3.frameX;
					tile3 = Main.tile[m, n];
					frameX3 = (short)(tile3.frameX + num13);
				}
			}
			NetMessage.SendTileSquare(-1, num141 + 1, num2 + 1, 3);
			return;
		}
		case 411:
		{
			int num23 = tile.frameX % 36 / 18;
			int num33 = tile.frameY % 36 / 18;
			int num44 = i - num23;
			int num55 = j - num33;
			int num64 = 36;
			tile3 = Main.tile[num44, num55];
			if (tile3.frameX >= 36)
			{
				num64 = -36;
			}
			for (int num65 = num44; num65 < num44 + 2; num65++)
			{
				for (int num66 = num55; num66 < num55 + 2; num66++)
				{
					SkipWire(num65, num66);
					tile3 = Main.tile[num65, num66];
					ref short frameX4 = ref tile3.frameX;
					tile3 = Main.tile[num65, num66];
					frameX4 = (short)(tile3.frameX + num64);
				}
			}
			NetMessage.SendTileSquare(-1, num44, num55, 2, 2);
			return;
		}
		case 356:
		{
			int num67 = tile.frameX % 36 / 18;
			int num69 = tile.frameY % 54 / 18;
			int num70 = i - num67;
			int num71 = j - num69;
			for (int num72 = num70; num72 < num70 + 2; num72++)
			{
				for (int num73 = num71; num73 < num71 + 3; num73++)
				{
					SkipWire(num72, num73);
				}
			}
			if (!Main.fastForwardTimeToDawn && Main.sundialCooldown == 0)
			{
				Main.Sundialing();
			}
			NetMessage.SendTileSquare(-1, num70, num71, 2, 2);
			return;
		}
		case 663:
		{
			int num74 = tile.frameX % 36 / 18;
			int num75 = tile.frameY % 54 / 18;
			int num76 = i - num74;
			int num77 = j - num75;
			for (int num78 = num76; num78 < num76 + 2; num78++)
			{
				for (int num80 = num77; num80 < num77 + 3; num80++)
				{
					SkipWire(num78, num80);
				}
			}
			if (!Main.fastForwardTimeToDusk && Main.moondialCooldown == 0)
			{
				Main.Moondialing();
			}
			NetMessage.SendTileSquare(-1, num76, num77, 2, 2);
			return;
		}
		case 425:
		{
			int num81 = tile.frameX % 36 / 18;
			int num82 = tile.frameY % 36 / 18;
			int num83 = i - num81;
			int num84 = j - num82;
			for (int num85 = num83; num85 < num83 + 2; num85++)
			{
				for (int num86 = num84; num86 < num84 + 2; num86++)
				{
					SkipWire(num85, num86);
				}
			}
			if (Main.AnnouncementBoxDisabled)
			{
				return;
			}
			Color pink = Color.Pink;
			int num87 = Sign.ReadSign(num83, num84, CreateIfMissing: false);
			if (num87 == -1 || Main.sign[num87] == null || string.IsNullOrWhiteSpace(Main.sign[num87].text))
			{
				return;
			}
			if (Main.AnnouncementBoxRange == -1)
			{
				if (Main.netMode == 0)
				{
					Main.NewTextMultiline(Main.sign[num87].text, force: false, pink, 460);
				}
				else if (Main.netMode == 2)
				{
					NetMessage.SendData(107, -1, -1, NetworkText.FromLiteral(Main.sign[num87].text), 255, (int)((Color)(ref pink)).R, (int)((Color)(ref pink)).G, (int)((Color)(ref pink)).B, 460);
				}
			}
			else if (Main.netMode == 0)
			{
				if (Main.player[Main.myPlayer].Distance(new Vector2((float)(num83 * 16 + 16), (float)(num84 * 16 + 16))) <= (float)Main.AnnouncementBoxRange)
				{
					Main.NewTextMultiline(Main.sign[num87].text, force: false, pink, 460);
				}
			}
			else
			{
				if (Main.netMode != 2)
				{
					return;
				}
				for (int num88 = 0; num88 < 255; num88++)
				{
					if (Main.player[num88].active && Main.player[num88].Distance(new Vector2((float)(num83 * 16 + 16), (float)(num84 * 16 + 16))) <= (float)Main.AnnouncementBoxRange)
					{
						NetMessage.SendData(107, num88, -1, NetworkText.FromLiteral(Main.sign[num87].text), 255, (int)((Color)(ref pink)).R, (int)((Color)(ref pink)).G, (int)((Color)(ref pink)).B, 460);
					}
				}
			}
			return;
		}
		case 405:
			ToggleFirePlace(i, j, tile, forcedStateWhereTrueIsOn, doSkipWires);
			return;
		case 209:
		{
			int num89 = tile.frameX % 72 / 18;
			int num91 = tile.frameY % 54 / 18;
			int num92 = i - num89;
			int num93 = j - num91;
			int num94 = tile.frameY / 54;
			int num95 = tile.frameX / 72;
			int num96 = -1;
			if (num89 == 1 || num89 == 2)
			{
				num96 = num91;
			}
			int num97 = 0;
			if (num89 == 3)
			{
				num97 = -54;
			}
			if (num89 == 0)
			{
				num97 = 54;
			}
			if (num94 >= 8 && num97 > 0)
			{
				num97 = 0;
			}
			if (num94 == 0 && num97 < 0)
			{
				num97 = 0;
			}
			bool flag = false;
			if (num97 != 0)
			{
				for (int num98 = num92; num98 < num92 + 4; num98++)
				{
					for (int num99 = num93; num99 < num93 + 3; num99++)
					{
						SkipWire(num98, num99);
						tile3 = Main.tile[num98, num99];
						ref short frameY2 = ref tile3.frameY;
						tile3 = Main.tile[num98, num99];
						frameY2 = (short)(tile3.frameY + num97);
					}
				}
				flag = true;
			}
			if ((num95 == 3 || num95 == 4) && (num96 == 0 || num96 == 1))
			{
				num97 = ((num95 == 3) ? 72 : (-72));
				for (int num100 = num92; num100 < num92 + 4; num100++)
				{
					for (int num102 = num93; num102 < num93 + 3; num102++)
					{
						SkipWire(num100, num102);
						tile3 = Main.tile[num100, num102];
						ref short frameX2 = ref tile3.frameX;
						tile3 = Main.tile[num100, num102];
						frameX2 = (short)(tile3.frameX + num97);
					}
				}
				flag = true;
			}
			if (flag)
			{
				NetMessage.SendTileSquare(-1, num92, num93, 4, 3);
			}
			if (num96 != -1)
			{
				bool flag5 = true;
				if ((num95 == 3 || num95 == 4) && num96 < 2)
				{
					flag5 = false;
				}
				if (CheckMech(num92, num93, 30) && flag5)
				{
					WorldGen.ShootFromCannon(num92, num93, num94, num95 + 1, 0, 0f, CurrentUser, fromWire: true);
				}
			}
			return;
		}
		case 212:
		{
			int num103 = tile.frameX % 54 / 18;
			int num104 = tile.frameY % 54 / 18;
			int num105 = i - num103;
			int num106 = j - num104;
			int num154 = tile.frameX / 54;
			int num107 = -1;
			if (num103 == 1)
			{
				num107 = num104;
			}
			int num108 = 0;
			if (num103 == 0)
			{
				num108 = -54;
			}
			if (num103 == 2)
			{
				num108 = 54;
			}
			if (num154 >= 1 && num108 > 0)
			{
				num108 = 0;
			}
			if (num154 == 0 && num108 < 0)
			{
				num108 = 0;
			}
			bool flag6 = false;
			if (num108 != 0)
			{
				for (int num109 = num105; num109 < num105 + 3; num109++)
				{
					for (int num110 = num106; num110 < num106 + 3; num110++)
					{
						SkipWire(num109, num110);
						tile3 = Main.tile[num109, num110];
						ref short frameX = ref tile3.frameX;
						tile3 = Main.tile[num109, num110];
						frameX = (short)(tile3.frameX + num108);
					}
				}
				flag6 = true;
			}
			if (flag6)
			{
				NetMessage.SendTileSquare(-1, num105, num106, 3, 3);
			}
			if (num107 != -1 && CheckMech(num105, num106, 10))
			{
				float num155 = 12f + (float)Main.rand.Next(450) * 0.01f;
				float num112 = Main.rand.Next(85, 105);
				float num156 = Main.rand.Next(-35, 11);
				int type2 = 166;
				int damage = 0;
				float knockBack = 0f;
				Vector2 vector = default(Vector2);
				((Vector2)(ref vector))._002Ector((float)((num105 + 2) * 16 - 8), (float)((num106 + 2) * 16 - 8));
				if (tile.frameX / 54 == 0)
				{
					num112 *= -1f;
					vector.X -= 12f;
				}
				else
				{
					vector.X += 12f;
				}
				float num113 = num112;
				float num114 = num156;
				float num115 = (float)Math.Sqrt(num113 * num113 + num114 * num114);
				num115 = num155 / num115;
				num113 *= num115;
				num114 *= num115;
				Projectile.NewProjectile(GetProjectileSource(num105, num106), vector.X, vector.Y, num113, num114, type2, damage, knockBack, CurrentUser);
			}
			return;
		}
		case 215:
			ToggleCampFire(i, j, tile, forcedStateWhereTrueIsOn, doSkipWires);
			return;
		case 130:
			if (!(Main.tile[i, j - 1] == null))
			{
				tile3 = Main.tile[i, j - 1];
				if (tile3.active())
				{
					bool[] basicChest = TileID.Sets.BasicChest;
					tile3 = Main.tile[i, j - 1];
					if (basicChest[tile3.type])
					{
						return;
					}
					bool[] basicChestFake = TileID.Sets.BasicChestFake;
					tile3 = Main.tile[i, j - 1];
					if (basicChestFake[tile3.type])
					{
						return;
					}
					tile3 = Main.tile[i, j - 1];
					if (tile3.type == 88)
					{
						return;
					}
				}
			}
			tile.type = 131;
			WorldGen.SquareTileFrame(i, j);
			NetMessage.SendTileSquare(-1, i, j);
			return;
		case 131:
			tile.type = 130;
			WorldGen.SquareTileFrame(i, j);
			NetMessage.SendTileSquare(-1, i, j);
			return;
		case 386:
		case 387:
		{
			bool value = type == 387;
			int num116 = WorldGen.ShiftTrapdoor(i, j, playerAbove: true).ToInt();
			if (num116 == 0)
			{
				num116 = -WorldGen.ShiftTrapdoor(i, j, playerAbove: false).ToInt();
			}
			if (num116 != 0)
			{
				NetMessage.SendData(19, -1, -1, null, 3 - value.ToInt(), i, j, num116);
			}
			return;
		}
		case 388:
		case 389:
		{
			bool flag7 = type == 389;
			WorldGen.ShiftTallGate(i, j, flag7);
			NetMessage.SendData(19, -1, -1, null, 4 + flag7.ToInt(), i, j);
			return;
		}
		}
		if (TileLoader.CloseDoorID(Main.tile[i, j]) >= 0)
		{
			if (WorldGen.CloseDoor(i, j, forced: true))
			{
				NetMessage.SendData(19, -1, -1, null, 1, i, j);
			}
			return;
		}
		if (TileLoader.OpenDoorID(Main.tile[i, j]) >= 0)
		{
			int num117 = 1;
			if (Main.rand.Next(2) == 0)
			{
				num117 = -1;
			}
			if (!WorldGen.OpenDoor(i, j, num117))
			{
				if (WorldGen.OpenDoor(i, j, -num117))
				{
					NetMessage.SendData(19, -1, -1, null, 0, i, j, -num117);
				}
			}
			else
			{
				NetMessage.SendData(19, -1, -1, null, 0, i, j, num117);
			}
			return;
		}
		if (type == 216)
		{
			WorldGen.LaunchRocket(i, j, fromWiring: true);
			SkipWire(i, j);
			return;
		}
		if (type == 497 || (type == 15 && tile.frameY / 40 == 1) || (type == 15 && tile.frameY / 40 == 20))
		{
			int num118 = j - tile.frameY % 40 / 18;
			SkipWire(i, num118);
			SkipWire(i, num118 + 1);
			if (CheckMech(i, num118, 60))
			{
				Projectile.NewProjectile(GetProjectileSource(i, num118), i * 16 + 8, num118 * 16 + 12, 0f, 0f, 733, 0, 0f, Main.myPlayer);
			}
			return;
		}
		int num157 = type;
		switch (num157)
		{
		case 335:
		{
			int num62 = j - tile.frameY / 18;
			int num63 = i - tile.frameX / 18;
			SkipWire(num63, num62);
			SkipWire(num63, num62 + 1);
			SkipWire(num63 + 1, num62);
			SkipWire(num63 + 1, num62 + 1);
			if (CheckMech(num63, num62, 30))
			{
				WorldGen.LaunchRocketSmall(num63, num62, fromWiring: true);
			}
			break;
		}
		case 338:
		{
			int num126 = j - tile.frameY / 18;
			int num127 = i - tile.frameX / 18;
			SkipWire(num127, num126);
			SkipWire(num127, num126 + 1);
			if (!CheckMech(num127, num126, 30))
			{
				break;
			}
			bool flag8 = false;
			for (int num128 = 0; num128 < 1000; num128++)
			{
				if (Main.projectile[num128].active && Main.projectile[num128].aiStyle == 73 && Main.projectile[num128].ai[0] == (float)num127 && Main.projectile[num128].ai[1] == (float)num126)
				{
					flag8 = true;
					break;
				}
			}
			if (!flag8)
			{
				int type3 = 419 + Main.rand.Next(4);
				Projectile.NewProjectile(GetProjectileSource(num127, num126), num127 * 16 + 8, num126 * 16 + 2, 0f, 0f, type3, 0, 0f, Main.myPlayer, num127, num126);
			}
			break;
		}
		case 235:
		{
			int num10 = i - tile.frameX / 18;
			if (tile.wall == 87 && (double)j > Main.worldSurface && !NPC.downedPlantBoss)
			{
				break;
			}
			if (_teleport[0].X == -1f)
			{
				_teleport[0].X = num10;
				_teleport[0].Y = j;
				if (tile.halfBrick())
				{
					_teleport[0].Y += 0.5f;
				}
			}
			else if (_teleport[0].X != (float)num10 || _teleport[0].Y != (float)j)
			{
				_teleport[1].X = num10;
				_teleport[1].Y = j;
				if (tile.halfBrick())
				{
					_teleport[1].Y += 0.5f;
				}
			}
			break;
		}
		default:
			if (!TileID.Sets.Torch[type])
			{
				switch (num157)
				{
				case 429:
				{
					tile3 = Main.tile[i, j];
					int num158 = tile3.frameX / 18;
					bool flag9 = num158 % 2 >= 1;
					bool flag10 = num158 % 4 >= 2;
					bool flag11 = num158 % 8 >= 4;
					bool flag12 = num158 % 16 >= 8;
					bool flag2 = false;
					short num129 = 0;
					switch (_currentWireColor)
					{
					case 1:
						num129 = 18;
						flag2 = !flag9;
						break;
					case 2:
						num129 = 72;
						flag2 = !flag11;
						break;
					case 3:
						num129 = 36;
						flag2 = !flag10;
						break;
					case 4:
						num129 = 144;
						flag2 = !flag12;
						break;
					}
					if (flag2)
					{
						tile.frameX += num129;
					}
					else
					{
						tile.frameX -= num129;
					}
					NetMessage.SendTileSquare(-1, i, j);
					break;
				}
				case 149:
					ToggleHolidayLight(i, j, tile, forcedStateWhereTrueIsOn);
					break;
				case 244:
				{
					int num35;
					for (num35 = tile.frameX / 18; num35 >= 3; num35 -= 3)
					{
					}
					int num36;
					for (num36 = tile.frameY / 18; num36 >= 3; num36 -= 3)
					{
					}
					int num37 = i - num35;
					int num38 = j - num36;
					int num39 = 54;
					tile3 = Main.tile[num37, num38];
					if (tile3.frameX >= 54)
					{
						num39 = -54;
					}
					for (int num40 = num37; num40 < num37 + 3; num40++)
					{
						for (int num41 = num38; num41 < num38 + 2; num41++)
						{
							SkipWire(num40, num41);
							tile3 = Main.tile[num40, num41];
							ref short frameX6 = ref tile3.frameX;
							tile3 = Main.tile[num40, num41];
							frameX6 = (short)(tile3.frameX + num39);
						}
					}
					NetMessage.SendTileSquare(-1, num37, num38, 3, 2);
					break;
				}
				case 565:
				{
					int num152;
					for (num152 = tile.frameX / 18; num152 >= 2; num152 -= 2)
					{
					}
					int num153;
					for (num153 = tile.frameY / 18; num153 >= 2; num153 -= 2)
					{
					}
					int num3 = i - num152;
					int num4 = j - num153;
					int num5 = 36;
					tile3 = Main.tile[num3, num4];
					if (tile3.frameX >= 36)
					{
						num5 = -36;
					}
					for (int num6 = num3; num6 < num3 + 2; num6++)
					{
						for (int num7 = num4; num7 < num4 + 2; num7++)
						{
							SkipWire(num6, num7);
							tile3 = Main.tile[num6, num7];
							ref short frameX5 = ref tile3.frameX;
							tile3 = Main.tile[num6, num7];
							frameX5 = (short)(tile3.frameX + num5);
						}
					}
					NetMessage.SendTileSquare(-1, num3, num4, 2, 2);
					break;
				}
				case 42:
					ToggleHangingLantern(i, j, tile, forcedStateWhereTrueIsOn, doSkipWires);
					break;
				case 93:
					ToggleLamp(i, j, tile, forcedStateWhereTrueIsOn, doSkipWires);
					break;
				case 95:
				case 100:
				case 126:
				case 173:
				case 564:
					Toggle2x2Light(i, j, tile, forcedStateWhereTrueIsOn, doSkipWires);
					break;
				case 593:
				{
					SkipWire(i, j);
					tile3 = Main.tile[i, j];
					short num8 = (short)((tile3.frameX != 0) ? (-18) : 18);
					tile3 = Main.tile[i, j];
					tile3.frameX += num8;
					if (Main.netMode == 2)
					{
						NetMessage.SendTileSquare(-1, i, j, 1, 1);
					}
					int num9 = ((num8 > 0) ? 4 : 3);
					Animation.NewTemporaryAnimation(num9, 593, i, j);
					NetMessage.SendTemporaryAnimation(-1, num9, 593, i, j);
					break;
				}
				case 594:
				{
					int num131;
					for (num131 = tile.frameY / 18; num131 >= 2; num131 -= 2)
					{
					}
					num131 = j - num131;
					int num132 = tile.frameX / 18;
					if (num132 > 1)
					{
						num132 -= 2;
					}
					num132 = i - num132;
					SkipWire(num132, num131);
					SkipWire(num132, num131 + 1);
					SkipWire(num132 + 1, num131);
					SkipWire(num132 + 1, num131 + 1);
					tile3 = Main.tile[num132, num131];
					short num133 = (short)((tile3.frameX != 0) ? (-36) : 36);
					for (int num134 = 0; num134 < 2; num134++)
					{
						for (int num135 = 0; num135 < 2; num135++)
						{
							tile3 = Main.tile[num132 + num134, num131 + num135];
							tile3.frameX += num133;
						}
					}
					if (Main.netMode == 2)
					{
						NetMessage.SendTileSquare(-1, num132, num131, 2, 2);
					}
					int num136 = ((num133 > 0) ? 4 : 3);
					Animation.NewTemporaryAnimation(num136, 594, num132, num131);
					NetMessage.SendTemporaryAnimation(-1, num136, 594, num132, num131);
					break;
				}
				case 34:
					ToggleChandelier(i, j, tile, forcedStateWhereTrueIsOn, doSkipWires);
					break;
				case 314:
					if (CheckMech(i, j, 5))
					{
						Minecart.FlipSwitchTrack(i, j);
					}
					break;
				case 33:
				case 49:
				case 174:
				case 372:
				case 646:
					ToggleCandle(i, j, tile, forcedStateWhereTrueIsOn);
					break;
				case 92:
					ToggleLampPost(i, j, tile, forcedStateWhereTrueIsOn, doSkipWires);
					break;
				case 137:
				{
					int num42 = tile.frameY / 18;
					Vector2 vector3 = Vector2.Zero;
					float speedX = 0f;
					float speedY = 0f;
					int num43 = 0;
					int damage3 = 0;
					Vector2 val;
					switch (num42)
					{
					case 0:
					case 1:
					case 2:
					case 5:
						if (CheckMech(i, j, 200))
						{
							int num52 = ((tile.frameX == 0) ? (-1) : ((tile.frameX == 18) ? 1 : 0));
							int num53 = ((tile.frameX >= 36) ? ((tile.frameX >= 72) ? 1 : (-1)) : 0);
							((Vector2)(ref vector3))._002Ector((float)(i * 16 + 8 + 10 * num52), (float)(j * 16 + 8 + 10 * num53));
							float num54 = 3f;
							if (num42 == 0)
							{
								num43 = 98;
								damage3 = 20;
								num54 = 12f;
							}
							if (num42 == 1)
							{
								num43 = 184;
								damage3 = 40;
								num54 = 12f;
							}
							if (num42 == 2)
							{
								num43 = 187;
								damage3 = 40;
								num54 = 5f;
							}
							if (num42 == 5)
							{
								num43 = 980;
								damage3 = 30;
								num54 = 12f;
							}
							speedX = (float)num52 * num54;
							speedY = (float)num53 * num54;
						}
						break;
					case 3:
					{
						if (!CheckMech(i, j, 300))
						{
							break;
						}
						int num47 = 200;
						for (int num48 = 0; num48 < 1000; num48++)
						{
							if (Main.projectile[num48].active && Main.projectile[num48].type == num43)
							{
								val = new Vector2((float)(i * 16 + 8), (float)(j * 18 + 8)) - Main.projectile[num48].Center;
								float num49 = ((Vector2)(ref val)).Length();
								num47 = ((num49 < 50f) ? (num47 - 50) : ((num49 < 100f) ? (num47 - 15) : ((num49 < 200f) ? (num47 - 10) : ((num49 < 300f) ? (num47 - 8) : ((num49 < 400f) ? (num47 - 6) : ((num49 < 500f) ? (num47 - 5) : ((num49 < 700f) ? (num47 - 4) : ((num49 < 900f) ? (num47 - 3) : ((!(num49 < 1200f)) ? (num47 - 1) : (num47 - 2))))))))));
							}
						}
						if (num47 > 0)
						{
							num43 = 185;
							damage3 = 40;
							int num50 = 0;
							int num51 = 0;
							switch (tile.frameX / 18)
							{
							case 0:
							case 1:
								num50 = 0;
								num51 = 1;
								break;
							case 2:
								num50 = 0;
								num51 = -1;
								break;
							case 3:
								num50 = -1;
								num51 = 0;
								break;
							case 4:
								num50 = 1;
								num51 = 0;
								break;
							}
							speedX = (float)(4 * num50) + (float)Main.rand.Next(-20 + ((num50 == 1) ? 20 : 0), 21 - ((num50 == -1) ? 20 : 0)) * 0.05f;
							speedY = (float)(4 * num51) + (float)Main.rand.Next(-20 + ((num51 == 1) ? 20 : 0), 21 - ((num51 == -1) ? 20 : 0)) * 0.05f;
							((Vector2)(ref vector3))._002Ector((float)(i * 16 + 8 + 14 * num50), (float)(j * 16 + 8 + 14 * num51));
						}
						break;
					}
					case 4:
						if (CheckMech(i, j, 90))
						{
							int num45 = 0;
							int num46 = 0;
							switch (tile.frameX / 18)
							{
							case 0:
							case 1:
								num45 = 0;
								num46 = 1;
								break;
							case 2:
								num45 = 0;
								num46 = -1;
								break;
							case 3:
								num45 = -1;
								num46 = 0;
								break;
							case 4:
								num45 = 1;
								num46 = 0;
								break;
							}
							speedX = 8 * num45;
							speedY = 8 * num46;
							damage3 = 60;
							num43 = 186;
							((Vector2)(ref vector3))._002Ector((float)(i * 16 + 8 + 18 * num45), (float)(j * 16 + 8 + 18 * num46));
						}
						break;
					}
					switch (num42)
					{
					case -10:
						if (CheckMech(i, j, 200))
						{
							int num60 = -1;
							if (tile.frameX != 0)
							{
								num60 = 1;
							}
							speedX = 12 * num60;
							damage3 = 20;
							num43 = 98;
							((Vector2)(ref vector3))._002Ector((float)(i * 16 + 8), (float)(j * 16 + 7));
							vector3.X += 10 * num60;
							vector3.Y += 2f;
						}
						break;
					case -9:
						if (CheckMech(i, j, 200))
						{
							int num56 = -1;
							if (tile.frameX != 0)
							{
								num56 = 1;
							}
							speedX = 12 * num56;
							damage3 = 40;
							num43 = 184;
							((Vector2)(ref vector3))._002Ector((float)(i * 16 + 8), (float)(j * 16 + 7));
							vector3.X += 10 * num56;
							vector3.Y += 2f;
						}
						break;
					case -8:
						if (CheckMech(i, j, 200))
						{
							int num61 = -1;
							if (tile.frameX != 0)
							{
								num61 = 1;
							}
							speedX = 5 * num61;
							damage3 = 40;
							num43 = 187;
							((Vector2)(ref vector3))._002Ector((float)(i * 16 + 8), (float)(j * 16 + 7));
							vector3.X += 10 * num61;
							vector3.Y += 2f;
						}
						break;
					case -7:
					{
						if (!CheckMech(i, j, 300))
						{
							break;
						}
						num43 = 185;
						int num57 = 200;
						for (int num58 = 0; num58 < 1000; num58++)
						{
							if (Main.projectile[num58].active && Main.projectile[num58].type == num43)
							{
								val = new Vector2((float)(i * 16 + 8), (float)(j * 18 + 8)) - Main.projectile[num58].Center;
								float num59 = ((Vector2)(ref val)).Length();
								num57 = ((num59 < 50f) ? (num57 - 50) : ((num59 < 100f) ? (num57 - 15) : ((num59 < 200f) ? (num57 - 10) : ((num59 < 300f) ? (num57 - 8) : ((num59 < 400f) ? (num57 - 6) : ((num59 < 500f) ? (num57 - 5) : ((num59 < 700f) ? (num57 - 4) : ((num59 < 900f) ? (num57 - 3) : ((!(num59 < 1200f)) ? (num57 - 1) : (num57 - 2))))))))));
							}
						}
						if (num57 > 0)
						{
							speedX = (float)Main.rand.Next(-20, 21) * 0.05f;
							speedY = 4f + (float)Main.rand.Next(0, 21) * 0.05f;
							damage3 = 40;
							((Vector2)(ref vector3))._002Ector((float)(i * 16 + 8), (float)(j * 16 + 16));
							vector3.Y += 6f;
							Projectile.NewProjectile(GetProjectileSource(i, j), (int)vector3.X, (int)vector3.Y, speedX, speedY, num43, damage3, 2f, Main.myPlayer);
						}
						break;
					}
					case -6:
						if (CheckMech(i, j, 90))
						{
							speedX = 0f;
							speedY = 8f;
							damage3 = 60;
							num43 = 186;
							((Vector2)(ref vector3))._002Ector((float)(i * 16 + 8), (float)(j * 16 + 16));
							vector3.Y += 10f;
						}
						break;
					}
					if (num43 != 0)
					{
						Projectile.NewProjectile(GetProjectileSource(i, j), (int)vector3.X, (int)vector3.Y, speedX, speedY, num43, damage3, 2f, Main.myPlayer);
					}
					break;
				}
				case 443:
					GeyserTrap(i, j);
					break;
				case 531:
				{
					int num29 = tile.frameX / 36;
					int num30 = tile.frameY / 54;
					int num31 = i - (tile.frameX - num29 * 36) / 18;
					int num32 = j - (tile.frameY - num30 * 54) / 18;
					if (CheckMech(num31, num32, 900))
					{
						Vector2 vector2 = new Vector2((float)(num31 + 1), (float)num32) * 16f;
						vector2.Y += 28f;
						int num34 = 99;
						int damage2 = 70;
						float knockBack2 = 10f;
						if (num34 != 0)
						{
							Projectile.NewProjectile(GetProjectileSource(num31, num32), (int)vector2.X, (int)vector2.Y, 0f, 0f, num34, damage2, knockBack2, Main.myPlayer);
						}
					}
					break;
				}
				default:
					if (!TileLoader.IsModMusicBox(tile))
					{
						switch (num157)
						{
						case 207:
							WorldGen.SwitchFountain(i, j);
							break;
						case 410:
						case 480:
						case 509:
						case 657:
						case 658:
							WorldGen.SwitchMonolith(i, j);
							break;
						case 455:
							BirthdayParty.ToggleManualParty();
							break;
						case 141:
							WorldGen.KillTile(i, j, fail: false, effectOnly: false, noItem: true);
							NetMessage.SendTileSquare(-1, i, j);
							Projectile.NewProjectile(GetProjectileSource(i, j), i * 16 + 8, j * 16 + 8, 0f, 0f, 108, 500, 10f, Main.myPlayer);
							break;
						case 210:
							WorldGen.ExplodeMine(i, j, fromWiring: true);
							break;
						case 142:
						case 143:
						{
							int num144 = j - tile.frameY / 18;
							int num145 = tile.frameX / 18;
							if (num145 > 1)
							{
								num145 -= 2;
							}
							num145 = i - num145;
							SkipWire(num145, num144);
							SkipWire(num145, num144 + 1);
							SkipWire(num145 + 1, num144);
							SkipWire(num145 + 1, num144 + 1);
							if (type == 142)
							{
								for (int num146 = 0; num146 < 4; num146++)
								{
									if (_numInPump >= 19)
									{
										break;
									}
									int num147;
									int num149;
									switch (num146)
									{
									case 0:
										num147 = num145;
										num149 = num144 + 1;
										break;
									case 1:
										num147 = num145 + 1;
										num149 = num144 + 1;
										break;
									case 2:
										num147 = num145;
										num149 = num144;
										break;
									default:
										num147 = num145 + 1;
										num149 = num144;
										break;
									}
									_inPumpX[_numInPump] = num147;
									_inPumpY[_numInPump] = num149;
									_numInPump++;
								}
								break;
							}
							for (int num151 = 0; num151 < 4; num151++)
							{
								if (_numOutPump >= 19)
								{
									break;
								}
								int num148;
								int num150;
								switch (num151)
								{
								case 0:
									num148 = num145;
									num150 = num144 + 1;
									break;
								case 1:
									num148 = num145 + 1;
									num150 = num144 + 1;
									break;
								case 2:
									num148 = num145;
									num150 = num144;
									break;
								default:
									num148 = num145 + 1;
									num150 = num144;
									break;
								}
								_outPumpX[_numOutPump] = num148;
								_outPumpY[_numOutPump] = num150;
								_numOutPump++;
							}
							break;
						}
						case 105:
						{
							int num11 = j - tile.frameY / 18;
							int num12 = tile.frameX / 18;
							int num14 = 0;
							while (num12 >= 2)
							{
								num12 -= 2;
								num14++;
							}
							num12 = i - num12;
							num12 = i - tile.frameX % 36 / 18;
							num11 = j - tile.frameY % 54 / 18;
							int num15 = tile.frameY / 54;
							num15 %= 3;
							num14 = tile.frameX / 36 + num15 * 55;
							SkipWire(num12, num11);
							SkipWire(num12, num11 + 1);
							SkipWire(num12, num11 + 2);
							SkipWire(num12 + 1, num11);
							SkipWire(num12 + 1, num11 + 1);
							SkipWire(num12 + 1, num11 + 2);
							int num16 = num12 * 16 + 16;
							int num17 = (num11 + 3) * 16;
							int num18 = -1;
							int num19 = -1;
							bool flag3 = true;
							bool flag4 = false;
							switch (num14)
							{
							case 5:
								num19 = 73;
								break;
							case 13:
								num19 = 24;
								break;
							case 30:
								num19 = 6;
								break;
							case 35:
								num19 = 2;
								break;
							case 51:
								num19 = Utils.SelectRandom(Main.rand, new short[2] { 299, 538 });
								break;
							case 52:
								num19 = 356;
								break;
							case 53:
								num19 = 357;
								break;
							case 54:
								num19 = Utils.SelectRandom(Main.rand, new short[2] { 355, 358 });
								break;
							case 55:
								num19 = Utils.SelectRandom(Main.rand, new short[2] { 367, 366 });
								break;
							case 56:
								num19 = Utils.SelectRandom(Main.rand, new short[5] { 359, 359, 359, 359, 360 });
								break;
							case 57:
								num19 = 377;
								break;
							case 58:
								num19 = 300;
								break;
							case 59:
								num19 = Utils.SelectRandom(Main.rand, new short[2] { 364, 362 });
								break;
							case 60:
								num19 = 148;
								break;
							case 61:
								num19 = 361;
								break;
							case 62:
								num19 = Utils.SelectRandom(Main.rand, new short[3] { 487, 486, 485 });
								break;
							case 63:
								num19 = 164;
								flag3 &= NPC.MechSpawn(num16, num17, 165);
								break;
							case 64:
								num19 = 86;
								flag4 = true;
								break;
							case 65:
								num19 = 490;
								break;
							case 66:
								num19 = 82;
								break;
							case 67:
								num19 = 449;
								break;
							case 68:
								num19 = 167;
								break;
							case 69:
								num19 = 480;
								break;
							case 70:
								num19 = 48;
								break;
							case 71:
								num19 = Utils.SelectRandom(Main.rand, new short[3] { 170, 180, 171 });
								flag4 = true;
								break;
							case 72:
								num19 = 481;
								break;
							case 73:
								num19 = 482;
								break;
							case 74:
								num19 = 430;
								break;
							case 75:
								num19 = 489;
								break;
							case 76:
								num19 = 611;
								break;
							case 77:
								num19 = 602;
								break;
							case 78:
								num19 = Utils.SelectRandom(Main.rand, new short[6] { 595, 596, 599, 597, 600, 598 });
								break;
							case 79:
								num19 = Utils.SelectRandom(Main.rand, new short[2] { 616, 617 });
								break;
							case 80:
								num19 = Utils.SelectRandom(Main.rand, new short[2] { 671, 672 });
								break;
							case 81:
								num19 = 673;
								break;
							case 82:
								num19 = Utils.SelectRandom(Main.rand, new short[2] { 674, 675 });
								break;
							}
							if (num19 != -1 && CheckMech(num12, num11, 30) && NPC.MechSpawn(num16, num17, num19) && flag3)
							{
								if (!flag4 || !Collision.SolidTiles(num12 - 2, num12 + 3, num11, num11 + 2))
								{
									num18 = NPC.NewNPC(GetNPCSource(num12, num11), num16, num17, num19);
								}
								else
								{
									Vector2 position = new Vector2((float)(num16 - 4), (float)(num17 - 22)) - new Vector2(10f);
									Utils.PoofOfSmoke(position);
									NetMessage.SendData(106, -1, -1, null, (int)position.X, position.Y);
								}
							}
							if (num18 <= -1)
							{
								switch (num14)
								{
								case 4:
									if (CheckMech(num12, num11, 30) && NPC.MechSpawn(num16, num17, 1))
									{
										num18 = NPC.NewNPC(GetNPCSource(num12, num11), num16, num17 - 12, 1);
									}
									break;
								case 7:
									if (CheckMech(num12, num11, 30) && NPC.MechSpawn(num16, num17, 49))
									{
										num18 = NPC.NewNPC(GetNPCSource(num12, num11), num16 - 4, num17 - 6, 49);
									}
									break;
								case 8:
									if (CheckMech(num12, num11, 30) && NPC.MechSpawn(num16, num17, 55))
									{
										num18 = NPC.NewNPC(GetNPCSource(num12, num11), num16, num17 - 12, 55);
									}
									break;
								case 9:
								{
									int type4 = 46;
									if (BirthdayParty.PartyIsUp)
									{
										type4 = 540;
									}
									if (CheckMech(num12, num11, 30) && NPC.MechSpawn(num16, num17, type4))
									{
										num18 = NPC.NewNPC(GetNPCSource(num12, num11), num16, num17 - 12, type4);
									}
									break;
								}
								case 10:
									if (CheckMech(num12, num11, 30) && NPC.MechSpawn(num16, num17, 21))
									{
										num18 = NPC.NewNPC(GetNPCSource(num12, num11), num16, num17, 21);
									}
									break;
								case 16:
									if (CheckMech(num12, num11, 30) && NPC.MechSpawn(num16, num17, 42))
									{
										if (!Collision.SolidTiles(num12 - 1, num12 + 1, num11, num11 + 1))
										{
											num18 = NPC.NewNPC(GetNPCSource(num12, num11), num16, num17 - 12, 42);
											break;
										}
										Vector2 position3 = new Vector2((float)(num16 - 4), (float)(num17 - 22)) - new Vector2(10f);
										Utils.PoofOfSmoke(position3);
										NetMessage.SendData(106, -1, -1, null, (int)position3.X, position3.Y);
									}
									break;
								case 18:
									if (CheckMech(num12, num11, 30) && NPC.MechSpawn(num16, num17, 67))
									{
										num18 = NPC.NewNPC(GetNPCSource(num12, num11), num16, num17 - 12, 67);
									}
									break;
								case 23:
									if (CheckMech(num12, num11, 30) && NPC.MechSpawn(num16, num17, 63))
									{
										num18 = NPC.NewNPC(GetNPCSource(num12, num11), num16, num17 - 12, 63);
									}
									break;
								case 27:
									if (CheckMech(num12, num11, 30) && NPC.MechSpawn(num16, num17, 85))
									{
										num18 = NPC.NewNPC(GetNPCSource(num12, num11), num16 - 9, num17, 85);
									}
									break;
								case 28:
									if (CheckMech(num12, num11, 30) && NPC.MechSpawn(num16, num17, 74))
									{
										num18 = NPC.NewNPC(GetNPCSource(num12, num11), num16, num17 - 12, Utils.SelectRandom(Main.rand, new short[3] { 74, 297, 298 }));
									}
									break;
								case 34:
								{
									for (int num27 = 0; num27 < 2; num27++)
									{
										for (int num28 = 0; num28 < 3; num28++)
										{
											Tile tile2 = Main.tile[num12 + num27, num11 + num28];
											tile2.type = 349;
											tile2.frameX = (short)(num27 * 18 + 216);
											tile2.frameY = (short)(num28 * 18);
										}
									}
									Animation.NewTemporaryAnimation(0, 349, num12, num11);
									if (Main.netMode == 2)
									{
										NetMessage.SendTileSquare(-1, num12, num11, 2, 3);
									}
									break;
								}
								case 42:
									if (CheckMech(num12, num11, 30) && NPC.MechSpawn(num16, num17, 58))
									{
										num18 = NPC.NewNPC(GetNPCSource(num12, num11), num16, num17 - 12, 58);
									}
									break;
								case 37:
									if (CheckMech(num12, num11, 600) && Item.MechSpawn(num16, num17, 58) && Item.MechSpawn(num16, num17, 1734) && Item.MechSpawn(num16, num17, 1867))
									{
										Item.NewItem(GetItemSource(num16, num17), num16, num17 - 16, 0, 0, 58);
									}
									break;
								case 50:
									if (CheckMech(num12, num11, 30) && NPC.MechSpawn(num16, num17, 65))
									{
										if (!Collision.SolidTiles(num12 - 2, num12 + 3, num11, num11 + 2))
										{
											num18 = NPC.NewNPC(GetNPCSource(num12, num11), num16, num17 - 12, 65);
											break;
										}
										Vector2 position2 = new Vector2((float)(num16 - 4), (float)(num17 - 22)) - new Vector2(10f);
										Utils.PoofOfSmoke(position2);
										NetMessage.SendData(106, -1, -1, null, (int)position2.X, position2.Y);
									}
									break;
								case 2:
									if (CheckMech(num12, num11, 600) && Item.MechSpawn(num16, num17, 184) && Item.MechSpawn(num16, num17, 1735) && Item.MechSpawn(num16, num17, 1868))
									{
										Item.NewItem(GetItemSource(num16, num17), num16, num17 - 16, 0, 0, 184);
									}
									break;
								case 17:
									if (CheckMech(num12, num11, 600) && Item.MechSpawn(num16, num17, 166))
									{
										Item.NewItem(GetItemSource(num16, num17), num16, num17 - 20, 0, 0, 166);
									}
									break;
								case 40:
								{
									if (!CheckMech(num12, num11, 300))
									{
										break;
									}
									List<int> array2 = new List<int>(50);
									int num24 = 0;
									for (int num25 = 0; num25 < 200; num25++)
									{
										bool vanillaCanGo2 = false;
										if (Main.npc[num25].active && (Main.npc[num25].type == 17 || Main.npc[num25].type == 19 || Main.npc[num25].type == 22 || Main.npc[num25].type == 38 || Main.npc[num25].type == 54 || Main.npc[num25].type == 107 || Main.npc[num25].type == 108 || Main.npc[num25].type == 142 || Main.npc[num25].type == 160 || Main.npc[num25].type == 207 || Main.npc[num25].type == 209 || Main.npc[num25].type == 227 || Main.npc[num25].type == 228 || Main.npc[num25].type == 229 || Main.npc[num25].type == 368 || Main.npc[num25].type == 369 || Main.npc[num25].type == 550 || Main.npc[num25].type == 441 || Main.npc[num25].type == 588))
										{
											vanillaCanGo2 = true;
										}
										if (Main.npc[num25].active && (NPCLoader.CanGoToStatue(Main.npc[num25], toKingStatue: true) ?? vanillaCanGo2))
										{
											array2.Add(num25);
											num24++;
										}
									}
									if (num24 > 0)
									{
										int num26 = array2[Main.rand.Next(num24)];
										Main.npc[num26].position.X = num16 - Main.npc[num26].width / 2;
										Main.npc[num26].position.Y = num17 - Main.npc[num26].height - 1;
										NetMessage.SendData(23, -1, -1, null, num26);
										NPCLoader.OnGoToStatue(Main.npc[num26], toKingStatue: true);
									}
									break;
								}
								case 41:
								{
									if (!CheckMech(num12, num11, 300))
									{
										break;
									}
									List<int> array = new List<int>(50);
									int num20 = 0;
									for (int num21 = 0; num21 < 200; num21++)
									{
										bool vanillaCanGo = false;
										if (Main.npc[num21].active && (Main.npc[num21].type == 18 || Main.npc[num21].type == 20 || Main.npc[num21].type == 124 || Main.npc[num21].type == 178 || Main.npc[num21].type == 208 || Main.npc[num21].type == 353 || Main.npc[num21].type == 633 || Main.npc[num21].type == 663))
										{
											vanillaCanGo = true;
										}
										if (Main.npc[num21].active && (NPCLoader.CanGoToStatue(Main.npc[num21], toKingStatue: false) ?? vanillaCanGo))
										{
											array.Add(num21);
											num20++;
										}
									}
									if (num20 > 0)
									{
										int num22 = array[Main.rand.Next(num20)];
										Main.npc[num22].position.X = num16 - Main.npc[num22].width / 2;
										Main.npc[num22].position.Y = num17 - Main.npc[num22].height - 1;
										NetMessage.SendData(23, -1, -1, null, num22);
										NPCLoader.OnGoToStatue(Main.npc[num22], toKingStatue: false);
									}
									break;
								}
								}
							}
							if (num18 >= 0)
							{
								Main.npc[num18].value = 0f;
								Main.npc[num18].npcSlots = 0f;
								Main.npc[num18].SpawnedFromStatue = true;
								Main.npc[num18].CanBeReplacedByOtherNPCs = true;
							}
							break;
						}
						case 349:
						{
							int num137 = tile.frameY / 18;
							num137 %= 3;
							int num138 = j - num137;
							int num139;
							for (num139 = tile.frameX / 18; num139 >= 2; num139 -= 2)
							{
							}
							num139 = i - num139;
							SkipWire(num139, num138);
							SkipWire(num139, num138 + 1);
							SkipWire(num139, num138 + 2);
							SkipWire(num139 + 1, num138);
							SkipWire(num139 + 1, num138 + 1);
							SkipWire(num139 + 1, num138 + 2);
							tile3 = Main.tile[num139, num138];
							short num140 = (short)((tile3.frameX != 0) ? (-216) : 216);
							for (int num142 = 0; num142 < 2; num142++)
							{
								for (int num143 = 0; num143 < 3; num143++)
								{
									tile3 = Main.tile[num139 + num142, num138 + num143];
									tile3.frameX += num140;
								}
							}
							if (Main.netMode == 2)
							{
								NetMessage.SendTileSquare(-1, num139, num138, 2, 3);
							}
							Animation.NewTemporaryAnimation((num140 <= 0) ? 1 : 0, 349, num139, num138);
							break;
						}
						case 506:
						{
							int num119 = tile.frameY / 18;
							num119 %= 3;
							int num121 = j - num119;
							int num122;
							for (num122 = tile.frameX / 18; num122 >= 2; num122 -= 2)
							{
							}
							num122 = i - num122;
							SkipWire(num122, num121);
							SkipWire(num122, num121 + 1);
							SkipWire(num122, num121 + 2);
							SkipWire(num122 + 1, num121);
							SkipWire(num122 + 1, num121 + 1);
							SkipWire(num122 + 1, num121 + 2);
							tile3 = Main.tile[num122, num121];
							short num123 = (short)((tile3.frameX >= 72) ? (-72) : 72);
							for (int num124 = 0; num124 < 2; num124++)
							{
								for (int num125 = 0; num125 < 3; num125++)
								{
									tile3 = Main.tile[num122 + num124, num121 + num125];
									tile3.frameX += num123;
								}
							}
							if (Main.netMode == 2)
							{
								NetMessage.SendTileSquare(-1, num122, num121, 2, 3);
							}
							break;
						}
						case 546:
							tile.type = 557;
							WorldGen.SquareTileFrame(i, j);
							NetMessage.SendTileSquare(-1, i, j);
							break;
						case 557:
							tile.type = 546;
							WorldGen.SquareTileFrame(i, j);
							NetMessage.SendTileSquare(-1, i, j);
							break;
						}
						break;
					}
					goto case 35;
				case 35:
				case 139:
					WorldGen.SwitchMB(i, j);
					break;
				}
			}
			else
			{
				ToggleTorch(i, j, tile, forcedStateWhereTrueIsOn);
			}
			break;
		}
		TileLoader.HitWire(i, j, type);
	}

	public static void ToggleHolidayLight(int i, int j, Tile tileCache, bool? forcedStateWhereTrueIsOn)
	{
		bool flag = tileCache.frameX >= 54;
		if (!forcedStateWhereTrueIsOn.HasValue || !forcedStateWhereTrueIsOn.Value != flag)
		{
			if (tileCache.frameX < 54)
			{
				tileCache.frameX += 54;
			}
			else
			{
				tileCache.frameX -= 54;
			}
			NetMessage.SendTileSquare(-1, i, j);
		}
	}

	public static void ToggleHangingLantern(int i, int j, Tile tileCache, bool? forcedStateWhereTrueIsOn, bool doSkipWires)
	{
		int num;
		for (num = tileCache.frameY / 18; num >= 2; num -= 2)
		{
		}
		int num2 = j - num;
		short num3 = 18;
		if (tileCache.frameX > 0)
		{
			num3 = -18;
		}
		bool flag = tileCache.frameX > 0;
		if (!forcedStateWhereTrueIsOn.HasValue || !forcedStateWhereTrueIsOn.Value != flag)
		{
			Tile tile = Main.tile[i, num2];
			tile.frameX += num3;
			tile = Main.tile[i, num2 + 1];
			tile.frameX += num3;
			if (doSkipWires)
			{
				SkipWire(i, num2);
				SkipWire(i, num2 + 1);
			}
			NetMessage.SendTileSquare(-1, i, j, 1, 2);
		}
	}

	public static void Toggle2x2Light(int i, int j, Tile tileCache, bool? forcedStateWhereTrueIsOn, bool doSkipWires)
	{
		int num;
		for (num = tileCache.frameY / 18; num >= 2; num -= 2)
		{
		}
		num = j - num;
		int num2 = tileCache.frameX / 18;
		if (num2 > 1)
		{
			num2 -= 2;
		}
		num2 = i - num2;
		short num3 = 36;
		Tile tile = Main.tile[num2, num];
		if (tile.frameX > 0)
		{
			num3 = -36;
		}
		tile = Main.tile[num2, num];
		bool flag = tile.frameX > 0;
		if (!forcedStateWhereTrueIsOn.HasValue || !forcedStateWhereTrueIsOn.Value != flag)
		{
			tile = Main.tile[num2, num];
			tile.frameX += num3;
			tile = Main.tile[num2, num + 1];
			tile.frameX += num3;
			tile = Main.tile[num2 + 1, num];
			tile.frameX += num3;
			tile = Main.tile[num2 + 1, num + 1];
			tile.frameX += num3;
			if (doSkipWires)
			{
				SkipWire(num2, num);
				SkipWire(num2 + 1, num);
				SkipWire(num2, num + 1);
				SkipWire(num2 + 1, num + 1);
			}
			NetMessage.SendTileSquare(-1, num2, num, 2, 2);
		}
	}

	public static void ToggleLampPost(int i, int j, Tile tileCache, bool? forcedStateWhereTrueIsOn, bool doSkipWires)
	{
		int num = j - tileCache.frameY / 18;
		short num2 = 18;
		if (tileCache.frameX > 0)
		{
			num2 = -18;
		}
		bool flag = tileCache.frameX > 0;
		if (forcedStateWhereTrueIsOn.HasValue && !forcedStateWhereTrueIsOn.Value == flag)
		{
			return;
		}
		for (int k = num; k < num + 6; k++)
		{
			Main.tile[i, k].frameX += num2;
			if (doSkipWires)
			{
				SkipWire(i, k);
			}
		}
		NetMessage.SendTileSquare(-1, i, num, 1, 6);
	}

	public static void ToggleTorch(int i, int j, Tile tileCache, bool? forcedStateWhereTrueIsOn)
	{
		bool flag = tileCache.frameX >= 66;
		if (!forcedStateWhereTrueIsOn.HasValue || !forcedStateWhereTrueIsOn.Value != flag)
		{
			if (tileCache.frameX < 66)
			{
				tileCache.frameX += 66;
			}
			else
			{
				tileCache.frameX -= 66;
			}
			NetMessage.SendTileSquare(-1, i, j);
		}
	}

	public static void ToggleCandle(int i, int j, Tile tileCache, bool? forcedStateWhereTrueIsOn)
	{
		short num = 18;
		if (tileCache.frameX > 0)
		{
			num = -18;
		}
		bool flag = tileCache.frameX > 0;
		if (!forcedStateWhereTrueIsOn.HasValue || !forcedStateWhereTrueIsOn.Value != flag)
		{
			tileCache.frameX += num;
			NetMessage.SendTileSquare(-1, i, j, 3);
		}
	}

	public static void ToggleLamp(int i, int j, Tile tileCache, bool? forcedStateWhereTrueIsOn, bool doSkipWires)
	{
		int num;
		for (num = tileCache.frameY / 18; num >= 3; num -= 3)
		{
		}
		num = j - num;
		short num2 = 18;
		if (tileCache.frameX > 0)
		{
			num2 = -18;
		}
		bool flag = tileCache.frameX > 0;
		if (!forcedStateWhereTrueIsOn.HasValue || !forcedStateWhereTrueIsOn.Value != flag)
		{
			Tile tile = Main.tile[i, num];
			tile.frameX += num2;
			tile = Main.tile[i, num + 1];
			tile.frameX += num2;
			tile = Main.tile[i, num + 2];
			tile.frameX += num2;
			if (doSkipWires)
			{
				SkipWire(i, num);
				SkipWire(i, num + 1);
				SkipWire(i, num + 2);
			}
			NetMessage.SendTileSquare(-1, i, num, 1, 3);
		}
	}

	public static void ToggleChandelier(int i, int j, Tile tileCache, bool? forcedStateWhereTrueIsOn, bool doSkipWires)
	{
		int num;
		for (num = tileCache.frameY / 18; num >= 3; num -= 3)
		{
		}
		int num2 = j - num;
		int num3 = tileCache.frameX % 108 / 18;
		if (num3 > 2)
		{
			num3 -= 3;
		}
		num3 = i - num3;
		short num4 = 54;
		Tile tile = Main.tile[num3, num2];
		if (tile.frameX % 108 > 0)
		{
			num4 = -54;
		}
		tile = Main.tile[num3, num2];
		bool flag = tile.frameX % 108 > 0;
		if (forcedStateWhereTrueIsOn.HasValue && !forcedStateWhereTrueIsOn.Value == flag)
		{
			return;
		}
		for (int k = num3; k < num3 + 3; k++)
		{
			for (int l = num2; l < num2 + 3; l++)
			{
				tile = Main.tile[k, l];
				tile.frameX += num4;
				if (doSkipWires)
				{
					SkipWire(k, l);
				}
			}
		}
		NetMessage.SendTileSquare(-1, num3 + 1, num2 + 1, 3);
	}

	public static void ToggleCampFire(int i, int j, Tile tileCache, bool? forcedStateWhereTrueIsOn, bool doSkipWires)
	{
		int num = tileCache.frameX % 54 / 18;
		int num2 = tileCache.frameY % 36 / 18;
		int num3 = i - num;
		int num4 = j - num2;
		Tile tile = Main.tile[num3, num4];
		bool flag = tile.frameY >= 36;
		if (forcedStateWhereTrueIsOn.HasValue && !forcedStateWhereTrueIsOn.Value == flag)
		{
			return;
		}
		int num5 = 36;
		tile = Main.tile[num3, num4];
		if (tile.frameY >= 36)
		{
			num5 = -36;
		}
		for (int k = num3; k < num3 + 3; k++)
		{
			for (int l = num4; l < num4 + 2; l++)
			{
				if (doSkipWires)
				{
					SkipWire(k, l);
				}
				tile = Main.tile[k, l];
				ref short frameY = ref tile.frameY;
				tile = Main.tile[k, l];
				frameY = (short)(tile.frameY + num5);
			}
		}
		NetMessage.SendTileSquare(-1, num3, num4, 3, 2);
	}

	public static void ToggleFirePlace(int i, int j, Tile theBlock, bool? forcedStateWhereTrueIsOn, bool doSkipWires)
	{
		int num = theBlock.frameX % 54 / 18;
		int num2 = theBlock.frameY % 36 / 18;
		int num3 = i - num;
		int num4 = j - num2;
		Tile tile = Main.tile[num3, num4];
		bool flag = tile.frameX >= 54;
		if (forcedStateWhereTrueIsOn.HasValue && !forcedStateWhereTrueIsOn.Value == flag)
		{
			return;
		}
		int num5 = 54;
		tile = Main.tile[num3, num4];
		if (tile.frameX >= 54)
		{
			num5 = -54;
		}
		for (int k = num3; k < num3 + 3; k++)
		{
			for (int l = num4; l < num4 + 2; l++)
			{
				if (doSkipWires)
				{
					SkipWire(k, l);
				}
				tile = Main.tile[k, l];
				ref short frameX = ref tile.frameX;
				tile = Main.tile[k, l];
				frameX = (short)(tile.frameX + num5);
			}
		}
		NetMessage.SendTileSquare(-1, num3, num4, 3, 2);
	}

	private static void GeyserTrap(int i, int j)
	{
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		Tile tile = Main.tile[i, j];
		if (tile.type != 443)
		{
			return;
		}
		int num = tile.frameX / 36;
		int num2 = i - (tile.frameX - num * 36) / 18;
		if (CheckMech(num2, j, 200))
		{
			Vector2 zero = Vector2.Zero;
			Vector2 zero2 = Vector2.Zero;
			int num3 = 654;
			int damage = 20;
			if (num < 2)
			{
				zero = new Vector2((float)(num2 + 1), (float)j) * 16f;
				((Vector2)(ref zero2))._002Ector(0f, -8f);
			}
			else
			{
				zero = new Vector2((float)(num2 + 1), (float)(j + 1)) * 16f;
				((Vector2)(ref zero2))._002Ector(0f, 8f);
			}
			if (num3 != 0)
			{
				Projectile.NewProjectile(GetProjectileSource(num2, j), (int)zero.X, (int)zero.Y, zero2.X, zero2.Y, num3, damage, 2f, Main.myPlayer);
			}
		}
	}

	private static void Teleport()
	{
		//IL_0249: Unknown result type (might be due to invalid IL or missing references)
		//IL_0255: Unknown result type (might be due to invalid IL or missing references)
		//IL_0268: Unknown result type (might be due to invalid IL or missing references)
		//IL_026d: Unknown result type (might be due to invalid IL or missing references)
		//IL_026e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0273: Unknown result type (might be due to invalid IL or missing references)
		//IL_029a: Unknown result type (might be due to invalid IL or missing references)
		//IL_028b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0350: Unknown result type (might be due to invalid IL or missing references)
		//IL_035d: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_0387: Unknown result type (might be due to invalid IL or missing references)
		//IL_038c: Unknown result type (might be due to invalid IL or missing references)
		//IL_038d: Unknown result type (might be due to invalid IL or missing references)
		if (_teleport[0].X < _teleport[1].X + 3f && _teleport[0].X > _teleport[1].X - 3f && _teleport[0].Y > _teleport[1].Y - 3f && _teleport[0].Y < _teleport[1].Y)
		{
			return;
		}
		Rectangle[] array = (Rectangle[])(object)new Rectangle[2];
		array[0].X = (int)(_teleport[0].X * 16f);
		array[0].Width = 48;
		array[0].Height = 48;
		array[0].Y = (int)(_teleport[0].Y * 16f - (float)array[0].Height);
		array[1].X = (int)(_teleport[1].X * 16f);
		array[1].Width = 48;
		array[1].Height = 48;
		array[1].Y = (int)(_teleport[1].Y * 16f - (float)array[1].Height);
		Vector2 vector = default(Vector2);
		for (int i = 0; i < 2; i++)
		{
			((Vector2)(ref vector))._002Ector((float)(array[1].X - array[0].X), (float)(array[1].Y - array[0].Y));
			if (i == 1)
			{
				((Vector2)(ref vector))._002Ector((float)(array[0].X - array[1].X), (float)(array[0].Y - array[1].Y));
			}
			if (!blockPlayerTeleportationForOneIteration)
			{
				for (int j = 0; j < 255; j++)
				{
					if (Main.player[j].active && !Main.player[j].dead && !Main.player[j].teleporting && TeleporterHitboxIntersects(array[i], Main.player[j].Hitbox))
					{
						Vector2 vector2 = Main.player[j].position + vector;
						Main.player[j].teleporting = true;
						if (Main.netMode == 2)
						{
							RemoteClient.CheckSection(j, vector2);
						}
						Main.player[j].Teleport(vector2);
						if (Main.netMode == 2)
						{
							NetMessage.SendData(65, -1, -1, null, 0, j, vector2.X, vector2.Y);
						}
					}
				}
			}
			for (int k = 0; k < 200; k++)
			{
				if (Main.npc[k].active && !Main.npc[k].teleporting && Main.npc[k].lifeMax > 5 && !Main.npc[k].boss && !Main.npc[k].noTileCollide)
				{
					int type = Main.npc[k].type;
					if (!NPCID.Sets.TeleportationImmune[type] && TeleporterHitboxIntersects(array[i], Main.npc[k].Hitbox))
					{
						Main.npc[k].teleporting = true;
						Main.npc[k].Teleport(Main.npc[k].position + vector);
					}
				}
			}
		}
		for (int l = 0; l < 255; l++)
		{
			Main.player[l].teleporting = false;
		}
		for (int m = 0; m < 200; m++)
		{
			Main.npc[m].teleporting = false;
		}
	}

	private static bool TeleporterHitboxIntersects(Rectangle teleporter, Rectangle entity)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		Rectangle rectangle = Rectangle.Union(teleporter, entity);
		if (rectangle.Width <= teleporter.Width + entity.Width)
		{
			return rectangle.Height <= teleporter.Height + entity.Height;
		}
		return false;
	}

	public static void DeActive(int i, int j)
	{
		if (!Main.tile[i, j].active() || (Main.tile[i, j].type == 226 && (double)j > Main.worldSurface && !NPC.downedPlantBoss))
		{
			return;
		}
		bool flag = Main.tileSolid[Main.tile[i, j].type] && !TileID.Sets.NotReallySolid[Main.tile[i, j].type];
		ushort type = Main.tile[i, j].type;
		if (type == 314 || (uint)(type - 386) <= 3u || type == 476)
		{
			flag = false;
		}
		if (flag && (!Main.tile[i, j - 1].active() || (!TileID.Sets.BasicChest[Main.tile[i, j - 1].type] && Main.tile[i, j - 1].type != 26 && Main.tile[i, j - 1].type != 77 && Main.tile[i, j - 1].type != 88 && Main.tile[i, j - 1].type != 470 && Main.tile[i, j - 1].type != 475 && Main.tile[i, j - 1].type != 237 && Main.tile[i, j - 1].type != 597 && WorldGen.CanKillTile(i, j))))
		{
			Main.tile[i, j].inActive(inActive: true);
			WorldGen.SquareTileFrame(i, j, resetFrame: false);
			if (Main.netMode != 1)
			{
				NetMessage.SendTileSquare(-1, i, j);
			}
		}
	}

	public static void ReActive(int i, int j)
	{
		Main.tile[i, j].inActive(inActive: false);
		WorldGen.SquareTileFrame(i, j, resetFrame: false);
		if (Main.netMode != 1)
		{
			NetMessage.SendTileSquare(-1, i, j);
		}
	}

	private static void MassWireOperationInner(Player user, Point ps, Point pe, Vector2 dropPoint, bool dir, ref int wireCount, ref int actuatorCount)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		Math.Abs(ps.X - pe.X);
		Math.Abs(ps.Y - pe.Y);
		int num = Math.Sign(pe.X - ps.X);
		int num2 = Math.Sign(pe.Y - ps.Y);
		WiresUI.Settings.MultiToolMode toolMode = WiresUI.Settings.ToolMode;
		Point pt = default(Point);
		bool flag = false;
		Item.StartCachingType(530);
		Item.StartCachingType(849);
		bool flag2 = dir;
		int num3;
		int num4;
		int num5;
		if (flag2)
		{
			pt.X = ps.X;
			num3 = ps.Y;
			num4 = pe.Y;
			num5 = num2;
		}
		else
		{
			pt.Y = ps.Y;
			num3 = ps.X;
			num4 = pe.X;
			num5 = num;
		}
		for (int i = num3; i != num4; i += num5)
		{
			if (flag)
			{
				break;
			}
			if (flag2)
			{
				pt.Y = i;
			}
			else
			{
				pt.X = i;
			}
			bool? flag3 = MassWireOperationStep(user, pt, toolMode, ref wireCount, ref actuatorCount);
			if (flag3.HasValue && !flag3.Value)
			{
				flag = true;
				break;
			}
		}
		if (flag2)
		{
			pt.Y = pe.Y;
			num3 = ps.X;
			num4 = pe.X;
			num5 = num;
		}
		else
		{
			pt.X = pe.X;
			num3 = ps.Y;
			num4 = pe.Y;
			num5 = num2;
		}
		for (int j = num3; j != num4; j += num5)
		{
			if (flag)
			{
				break;
			}
			if (!flag2)
			{
				pt.Y = j;
			}
			else
			{
				pt.X = j;
			}
			bool? flag4 = MassWireOperationStep(user, pt, toolMode, ref wireCount, ref actuatorCount);
			if (flag4.HasValue && !flag4.Value)
			{
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			MassWireOperationStep(user, pe, toolMode, ref wireCount, ref actuatorCount);
		}
		IEntitySource source_Misc = user.GetSource_Misc(ItemSourceID.ToContextString(5));
		Item.DropCache(source_Misc, dropPoint, Vector2.Zero, 530);
		Item.DropCache(source_Misc, dropPoint, Vector2.Zero, 849);
	}

	private static bool? MassWireOperationStep(Player user, Point pt, WiresUI.Settings.MultiToolMode mode, ref int wiresLeftToConsume, ref int actuatorsLeftToConstume)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_029f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0340: Unknown result type (might be due to invalid IL or missing references)
		//IL_0346: Unknown result type (might be due to invalid IL or missing references)
		//IL_0309: Unknown result type (might be due to invalid IL or missing references)
		//IL_0310: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Unknown result type (might be due to invalid IL or missing references)
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0191: Unknown result type (might be due to invalid IL or missing references)
		//IL_0391: Unknown result type (might be due to invalid IL or missing references)
		//IL_0397: Unknown result type (might be due to invalid IL or missing references)
		//IL_035a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0361: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0239: Unknown result type (might be due to invalid IL or missing references)
		//IL_023f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0251: Unknown result type (might be due to invalid IL or missing references)
		//IL_0258: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0404: Unknown result type (might be due to invalid IL or missing references)
		if (!WorldGen.InWorld(pt.X, pt.Y, 1))
		{
			return null;
		}
		Tile tile = Main.tile[pt.X, pt.Y];
		if (tile == null)
		{
			return null;
		}
		if (user != null && !user.CanDoWireStuffHere(pt.X, pt.Y))
		{
			return null;
		}
		if (!mode.HasFlag(WiresUI.Settings.MultiToolMode.Cutter))
		{
			if (mode.HasFlag(WiresUI.Settings.MultiToolMode.Red) && !tile.wire())
			{
				if (wiresLeftToConsume <= 0)
				{
					return false;
				}
				wiresLeftToConsume--;
				WorldGen.PlaceWire(pt.X, pt.Y);
				NetMessage.SendData(17, -1, -1, null, 5, pt.X, pt.Y);
			}
			if (mode.HasFlag(WiresUI.Settings.MultiToolMode.Green) && !tile.wire3())
			{
				if (wiresLeftToConsume <= 0)
				{
					return false;
				}
				wiresLeftToConsume--;
				WorldGen.PlaceWire3(pt.X, pt.Y);
				NetMessage.SendData(17, -1, -1, null, 12, pt.X, pt.Y);
			}
			if (mode.HasFlag(WiresUI.Settings.MultiToolMode.Blue) && !tile.wire2())
			{
				if (wiresLeftToConsume <= 0)
				{
					return false;
				}
				wiresLeftToConsume--;
				WorldGen.PlaceWire2(pt.X, pt.Y);
				NetMessage.SendData(17, -1, -1, null, 10, pt.X, pt.Y);
			}
			if (mode.HasFlag(WiresUI.Settings.MultiToolMode.Yellow) && !tile.wire4())
			{
				if (wiresLeftToConsume <= 0)
				{
					return false;
				}
				wiresLeftToConsume--;
				WorldGen.PlaceWire4(pt.X, pt.Y);
				NetMessage.SendData(17, -1, -1, null, 16, pt.X, pt.Y);
			}
			if (mode.HasFlag(WiresUI.Settings.MultiToolMode.Actuator) && !tile.actuator())
			{
				if (actuatorsLeftToConstume <= 0)
				{
					return false;
				}
				actuatorsLeftToConstume--;
				WorldGen.PlaceActuator(pt.X, pt.Y);
				NetMessage.SendData(17, -1, -1, null, 8, pt.X, pt.Y);
			}
		}
		if (mode.HasFlag(WiresUI.Settings.MultiToolMode.Cutter))
		{
			if (mode.HasFlag(WiresUI.Settings.MultiToolMode.Red) && tile.wire() && WorldGen.KillWire(pt.X, pt.Y))
			{
				NetMessage.SendData(17, -1, -1, null, 6, pt.X, pt.Y);
			}
			if (mode.HasFlag(WiresUI.Settings.MultiToolMode.Green) && tile.wire3() && WorldGen.KillWire3(pt.X, pt.Y))
			{
				NetMessage.SendData(17, -1, -1, null, 13, pt.X, pt.Y);
			}
			if (mode.HasFlag(WiresUI.Settings.MultiToolMode.Blue) && tile.wire2() && WorldGen.KillWire2(pt.X, pt.Y))
			{
				NetMessage.SendData(17, -1, -1, null, 11, pt.X, pt.Y);
			}
			if (mode.HasFlag(WiresUI.Settings.MultiToolMode.Yellow) && tile.wire4() && WorldGen.KillWire4(pt.X, pt.Y))
			{
				NetMessage.SendData(17, -1, -1, null, 17, pt.X, pt.Y);
			}
			if (mode.HasFlag(WiresUI.Settings.MultiToolMode.Actuator) && tile.actuator() && WorldGen.KillActuator(pt.X, pt.Y))
			{
				NetMessage.SendData(17, -1, -1, null, 9, pt.X, pt.Y);
			}
		}
		return true;
	}
}
