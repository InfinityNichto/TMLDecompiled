using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using ReLogic.Content;
using ReLogic.Utilities;
using Terraria.ID;

namespace Terraria.Audio;

public class LegacySoundPlayer
{
	public Asset<SoundEffect>[] SoundDrip = new Asset<SoundEffect>[3];

	public SoundEffectInstance[] SoundInstanceDrip = (SoundEffectInstance[])(object)new SoundEffectInstance[3];

	public Asset<SoundEffect>[] SoundLiquid = new Asset<SoundEffect>[2];

	public SoundEffectInstance[] SoundInstanceLiquid = (SoundEffectInstance[])(object)new SoundEffectInstance[2];

	public Asset<SoundEffect>[] SoundMech = new Asset<SoundEffect>[1];

	public SoundEffectInstance[] SoundInstanceMech = (SoundEffectInstance[])(object)new SoundEffectInstance[1];

	public Asset<SoundEffect>[] SoundDig = new Asset<SoundEffect>[3];

	public SoundEffectInstance[] SoundInstanceDig = (SoundEffectInstance[])(object)new SoundEffectInstance[3];

	public Asset<SoundEffect>[] SoundThunder = new Asset<SoundEffect>[7];

	public SoundEffectInstance[] SoundInstanceThunder = (SoundEffectInstance[])(object)new SoundEffectInstance[7];

	public Asset<SoundEffect>[] SoundResearch = new Asset<SoundEffect>[4];

	public SoundEffectInstance[] SoundInstanceResearch = (SoundEffectInstance[])(object)new SoundEffectInstance[4];

	public Asset<SoundEffect>[] SoundTink = new Asset<SoundEffect>[3];

	public SoundEffectInstance[] SoundInstanceTink = (SoundEffectInstance[])(object)new SoundEffectInstance[3];

	public Asset<SoundEffect>[] SoundCoin = new Asset<SoundEffect>[5];

	public SoundEffectInstance[] SoundInstanceCoin = (SoundEffectInstance[])(object)new SoundEffectInstance[5];

	public Asset<SoundEffect>[] SoundPlayerHit = new Asset<SoundEffect>[3];

	public SoundEffectInstance[] SoundInstancePlayerHit = (SoundEffectInstance[])(object)new SoundEffectInstance[3];

	public Asset<SoundEffect>[] SoundFemaleHit = new Asset<SoundEffect>[3];

	public SoundEffectInstance[] SoundInstanceFemaleHit = (SoundEffectInstance[])(object)new SoundEffectInstance[3];

	public Asset<SoundEffect> SoundPlayerKilled;

	public SoundEffectInstance SoundInstancePlayerKilled;

	public Asset<SoundEffect> SoundGrass;

	public SoundEffectInstance SoundInstanceGrass;

	public Asset<SoundEffect> SoundGrab;

	public SoundEffectInstance SoundInstanceGrab;

	public Asset<SoundEffect> SoundPixie;

	public SoundEffectInstance SoundInstancePixie;

	public Asset<SoundEffect>[] SoundItem = new Asset<SoundEffect>[SoundID.ItemSoundCount];

	public SoundEffectInstance[] SoundInstanceItem = (SoundEffectInstance[])(object)new SoundEffectInstance[SoundID.ItemSoundCount];

	public Asset<SoundEffect>[] SoundNpcHit = new Asset<SoundEffect>[SoundID.NPCHitCount];

	public SoundEffectInstance[] SoundInstanceNpcHit = (SoundEffectInstance[])(object)new SoundEffectInstance[SoundID.NPCHitCount];

	public Asset<SoundEffect>[] SoundNpcKilled = new Asset<SoundEffect>[SoundID.NPCDeathCount];

	public SoundEffectInstance[] SoundInstanceNpcKilled = (SoundEffectInstance[])(object)new SoundEffectInstance[SoundID.NPCDeathCount];

	public SoundEffectInstance SoundInstanceMoonlordCry;

	public Asset<SoundEffect> SoundDoorOpen;

	public SoundEffectInstance SoundInstanceDoorOpen;

	public Asset<SoundEffect> SoundDoorClosed;

	public SoundEffectInstance SoundInstanceDoorClosed;

	public Asset<SoundEffect> SoundMenuOpen;

	public SoundEffectInstance SoundInstanceMenuOpen;

	public Asset<SoundEffect> SoundMenuClose;

	public SoundEffectInstance SoundInstanceMenuClose;

	public Asset<SoundEffect> SoundMenuTick;

	public SoundEffectInstance SoundInstanceMenuTick;

	public Asset<SoundEffect> SoundShatter;

	public SoundEffectInstance SoundInstanceShatter;

	public Asset<SoundEffect> SoundCamera;

	public SoundEffectInstance SoundInstanceCamera;

	public Asset<SoundEffect>[] SoundZombie = new Asset<SoundEffect>[131];

	public SoundEffectInstance[] SoundInstanceZombie = (SoundEffectInstance[])(object)new SoundEffectInstance[131];

	public Asset<SoundEffect>[] SoundRoar = new Asset<SoundEffect>[3];

	public SoundEffectInstance[] SoundInstanceRoar = (SoundEffectInstance[])(object)new SoundEffectInstance[3];

	public Asset<SoundEffect>[] SoundSplash = new Asset<SoundEffect>[6];

	public SoundEffectInstance[] SoundInstanceSplash = (SoundEffectInstance[])(object)new SoundEffectInstance[6];

	public Asset<SoundEffect> SoundDoubleJump;

	public SoundEffectInstance SoundInstanceDoubleJump;

	public Asset<SoundEffect> SoundRun;

	public SoundEffectInstance SoundInstanceRun;

	public Asset<SoundEffect> SoundCoins;

	public SoundEffectInstance SoundInstanceCoins;

	public Asset<SoundEffect> SoundUnlock;

	public SoundEffectInstance SoundInstanceUnlock;

	public Asset<SoundEffect> SoundChat;

	public SoundEffectInstance SoundInstanceChat;

	public Asset<SoundEffect> SoundMaxMana;

	public SoundEffectInstance SoundInstanceMaxMana;

	public Asset<SoundEffect> SoundDrown;

	public SoundEffectInstance SoundInstanceDrown;

	public Asset<SoundEffect>[] TrackableSounds;

	public SoundEffectInstance[] TrackableSoundInstances;

	private readonly IServiceProvider _services;

	private List<SoundEffectInstance> _trackedInstances;

	public LegacySoundPlayer(IServiceProvider services)
	{
		_services = services;
		_trackedInstances = new List<SoundEffectInstance>();
		LoadAll();
	}

	public void Reload()
	{
		CreateAllSoundInstances();
	}

	private void LoadAll()
	{
	}

	public void CreateAllSoundInstances()
	{
		foreach (SoundEffectInstance trackedInstance in _trackedInstances)
		{
			trackedInstance.Dispose();
		}
		_trackedInstances.Clear();
	}

	private SoundEffectInstance CreateInstance(Asset<SoundEffect> asset)
	{
		SoundEffectInstance soundEffectInstance = asset.Value.CreateInstance();
		_trackedInstances.Add(soundEffectInstance);
		return soundEffectInstance;
	}

	private Asset<SoundEffect> Load(string assetName)
	{
		return _services.Get<IAssetRepository>().Request<SoundEffect>(assetName, AssetRequestMode.AsyncLoad);
	}

	public SoundEffectInstance PlaySound(int type, int x = -1, int y = -1, int Style = 1, float volumeScale = 1f, float pitchOffset = 0f)
	{
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_109c: Unknown result type (might be due to invalid IL or missing references)
		//IL_10a2: Invalid comparison between Unknown and I4
		//IL_1434: Unknown result type (might be due to invalid IL or missing references)
		//IL_14fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_1705: Unknown result type (might be due to invalid IL or missing references)
		//IL_1795: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f20: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f26: Invalid comparison between Unknown and I4
		//IL_17b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_091d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a93: Unknown result type (might be due to invalid IL or missing references)
		//IL_1de6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d0f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b30: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bcf: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c71: Unknown result type (might be due to invalid IL or missing references)
		//IL_196d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a0c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e8a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d32: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d55: Unknown result type (might be due to invalid IL or missing references)
		//IL_244c: Unknown result type (might be due to invalid IL or missing references)
		//IL_245c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2461: Unknown result type (might be due to invalid IL or missing references)
		//IL_2466: Unknown result type (might be due to invalid IL or missing references)
		//IL_115e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1164: Invalid comparison between Unknown and I4
		//IL_1f23: Unknown result type (might be due to invalid IL or missing references)
		//IL_118a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1190: Invalid comparison between Unknown and I4
		//IL_24f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_2016: Unknown result type (might be due to invalid IL or missing references)
		//IL_211d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2519: Unknown result type (might be due to invalid IL or missing references)
		//IL_0661: Unknown result type (might be due to invalid IL or missing references)
		//IL_15ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_253c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2044: Unknown result type (might be due to invalid IL or missing references)
		//IL_214b: Unknown result type (might be due to invalid IL or missing references)
		//IL_06fa: Unknown result type (might be due to invalid IL or missing references)
		int num = Style;
		try
		{
			if (Main.dedServ)
			{
				return null;
			}
			if (Main.soundVolume == 0f && (type < 30 || type > 35))
			{
				return null;
			}
			bool flag = false;
			float num11 = 1f;
			float num18 = 0f;
			if (x == -1 || y == -1)
			{
				flag = true;
			}
			else
			{
				if (WorldGen.gen)
				{
					return null;
				}
				if (Main.netMode == 2)
				{
					return null;
				}
				Vector2 vector = default(Vector2);
				((Vector2)(ref vector))._002Ector(Main.screenPosition.X + (float)Main.screenWidth * 0.5f, Main.screenPosition.Y + (float)Main.screenHeight * 0.5f);
				float num24 = Math.Abs((float)x - vector.X);
				float num19 = Math.Abs((float)y - vector.Y);
				float num20 = (float)Math.Sqrt(num24 * num24 + num19 * num19);
				int num21 = 2500;
				if (num20 < (float)num21)
				{
					flag = true;
					num18 = ((type != 43) ? (((float)x - vector.X) / ((float)Main.screenWidth * 0.5f)) : (((float)x - vector.X) / 900f));
					num11 = 1f - num20 / (float)num21;
				}
			}
			if (num18 < -1f)
			{
				num18 = -1f;
			}
			if (num18 > 1f)
			{
				num18 = 1f;
			}
			if (num11 > 1f)
			{
				num11 = 1f;
			}
			if (num11 <= 0f && (type < 34 || type > 35 || type > 39))
			{
				return null;
			}
			if (flag)
			{
				if (DoesSoundScaleWithAmbientVolume(type))
				{
					num11 *= Main.ambientVolume * (float)((!Main.gameInactive) ? 1 : 0);
					if (Main.gameMenu)
					{
						num11 = 0f;
					}
				}
				else
				{
					num11 *= Main.soundVolume;
				}
				if (num11 > 1f)
				{
					num11 = 1f;
				}
				if (num11 <= 0f && (type < 30 || type > 35) && type != 39)
				{
					return null;
				}
				SoundEffectInstance soundEffectInstance = null;
				switch (type)
				{
				case 0:
				{
					int num7 = Main.rand.Next(3);
					if (SoundInstanceDig[num7] != null)
					{
						SoundInstanceDig[num7].Stop();
					}
					SoundInstanceDig[num7] = SoundDig[num7].Value.CreateInstance();
					SoundInstanceDig[num7].Volume = num11;
					SoundInstanceDig[num7].Pan = num18;
					SoundInstanceDig[num7].Pitch = (float)Main.rand.Next(-10, 11) * 0.01f;
					soundEffectInstance = SoundInstanceDig[num7];
					break;
				}
				case 43:
				{
					int num6 = Main.rand.Next(SoundThunder.Length);
					for (int j = 0; j < SoundThunder.Length; j++)
					{
						if (SoundInstanceThunder[num6] == null)
						{
							break;
						}
						if ((int)SoundInstanceThunder[num6].State != 0)
						{
							break;
						}
						num6 = Main.rand.Next(SoundThunder.Length);
					}
					if (SoundInstanceThunder[num6] != null)
					{
						SoundInstanceThunder[num6].Stop();
					}
					SoundInstanceThunder[num6] = SoundThunder[num6].Value.CreateInstance();
					SoundInstanceThunder[num6].Volume = num11;
					SoundInstanceThunder[num6].Pan = num18;
					SoundInstanceThunder[num6].Pitch = (float)Main.rand.Next(-10, 11) * 0.01f;
					soundEffectInstance = SoundInstanceThunder[num6];
					break;
				}
				case 63:
				{
					int num8 = Main.rand.Next(1, 4);
					if (SoundInstanceResearch[num8] != null)
					{
						SoundInstanceResearch[num8].Stop();
					}
					SoundInstanceResearch[num8] = SoundResearch[num8].Value.CreateInstance();
					SoundInstanceResearch[num8].Volume = num11;
					SoundInstanceResearch[num8].Pan = num18;
					soundEffectInstance = SoundInstanceResearch[num8];
					break;
				}
				case 64:
					if (SoundInstanceResearch[0] != null)
					{
						SoundInstanceResearch[0].Stop();
					}
					SoundInstanceResearch[0] = SoundResearch[0].Value.CreateInstance();
					SoundInstanceResearch[0].Volume = num11;
					SoundInstanceResearch[0].Pan = num18;
					soundEffectInstance = SoundInstanceResearch[0];
					break;
				case 1:
				{
					int num9 = Main.rand.Next(3);
					if (SoundInstancePlayerHit[num9] != null)
					{
						SoundInstancePlayerHit[num9].Stop();
					}
					SoundInstancePlayerHit[num9] = SoundPlayerHit[num9].Value.CreateInstance();
					SoundInstancePlayerHit[num9].Volume = num11;
					SoundInstancePlayerHit[num9].Pan = num18;
					soundEffectInstance = SoundInstancePlayerHit[num9];
					break;
				}
				case 2:
					if (num == 176)
					{
						num11 *= 0.9f;
					}
					if (num == 129)
					{
						num11 *= 0.6f;
					}
					if (num == 123)
					{
						num11 *= 0.5f;
					}
					if (num == 124 || num == 125)
					{
						num11 *= 0.65f;
					}
					if (num == 116)
					{
						num11 *= 0.5f;
					}
					switch (num)
					{
					case 1:
					{
						int num25 = Main.rand.Next(3);
						if (num25 == 1)
						{
							num = 18;
						}
						if (num25 == 2)
						{
							num = 19;
						}
						break;
					}
					case 53:
					case 55:
						num11 *= 0.75f;
						if (num == 55)
						{
							num11 *= 0.75f;
						}
						if (SoundInstanceItem[num] != null && (int)SoundInstanceItem[num].State == 0)
						{
							return null;
						}
						break;
					case 37:
						num11 *= 0.5f;
						break;
					case 52:
						num11 *= 0.35f;
						break;
					case 157:
						num11 *= 0.7f;
						break;
					case 158:
						num11 *= 0.8f;
						break;
					}
					switch (num)
					{
					case 159:
						if (SoundInstanceItem[num] != null && (int)SoundInstanceItem[num].State == 0)
						{
							return null;
						}
						num11 *= 0.75f;
						break;
					default:
						if (SoundInstanceItem[num] != null)
						{
							SoundInstanceItem[num].Stop();
						}
						break;
					case 9:
					case 10:
					case 24:
					case 26:
					case 34:
					case 43:
					case 103:
					case 156:
					case 162:
						break;
					}
					SoundInstanceItem[num] = SoundItem[num].Value.CreateInstance();
					SoundInstanceItem[num].Volume = num11;
					SoundInstanceItem[num].Pan = num18;
					switch (num)
					{
					case 53:
						SoundInstanceItem[num].Pitch = (float)Main.rand.Next(-20, -11) * 0.02f;
						break;
					case 55:
						SoundInstanceItem[num].Pitch = (float)(-Main.rand.Next(-20, -11)) * 0.02f;
						break;
					case 132:
						SoundInstanceItem[num].Pitch = (float)Main.rand.Next(-20, 21) * 0.001f;
						break;
					case 153:
						SoundInstanceItem[num].Pitch = (float)Main.rand.Next(-50, 51) * 0.003f;
						break;
					case 156:
					{
						SoundInstanceItem[num].Pitch = (float)Main.rand.Next(-50, 51) * 0.002f;
						SoundEffectInstance obj2 = SoundInstanceItem[num];
						obj2.Volume *= 0.6f;
						break;
					}
					default:
						SoundInstanceItem[num].Pitch = (float)Main.rand.Next(-6, 7) * 0.01f;
						break;
					}
					if (num == 26 || num == 35 || num == 47)
					{
						SoundInstanceItem[num].Volume = num11 * 0.75f;
						SoundInstanceItem[num].Pitch = Main.musicPitch;
					}
					if (num == 169)
					{
						SoundEffectInstance obj3 = SoundInstanceItem[num];
						obj3.Pitch -= 0.8f;
					}
					soundEffectInstance = SoundInstanceItem[num];
					break;
				case 3:
					if (num >= 20 && num <= 54)
					{
						num11 *= 0.5f;
					}
					if (num == 57 && SoundInstanceNpcHit[num] != null && (int)SoundInstanceNpcHit[num].State == 0)
					{
						return null;
					}
					if (num == 57)
					{
						num11 *= 0.6f;
					}
					if (num == 55 || num == 56)
					{
						num11 *= 0.5f;
					}
					if (SoundInstanceNpcHit[num] != null)
					{
						SoundInstanceNpcHit[num].Stop();
					}
					SoundInstanceNpcHit[num] = SoundNpcHit[num].Value.CreateInstance();
					SoundInstanceNpcHit[num].Volume = num11;
					SoundInstanceNpcHit[num].Pan = num18;
					SoundInstanceNpcHit[num].Pitch = (float)Main.rand.Next(-10, 11) * 0.01f;
					soundEffectInstance = SoundInstanceNpcHit[num];
					break;
				case 4:
					if (num >= 23 && num <= 57)
					{
						num11 *= 0.5f;
					}
					if (num == 61)
					{
						num11 *= 0.6f;
					}
					if (num == 62)
					{
						num11 *= 0.6f;
					}
					if (num == 10 && SoundInstanceNpcKilled[num] != null && (int)SoundInstanceNpcKilled[num].State == 0)
					{
						return null;
					}
					SoundInstanceNpcKilled[num] = SoundNpcKilled[num].Value.CreateInstance();
					SoundInstanceNpcKilled[num].Volume = num11;
					SoundInstanceNpcKilled[num].Pan = num18;
					SoundInstanceNpcKilled[num].Pitch = (float)Main.rand.Next(-10, 11) * 0.01f;
					soundEffectInstance = SoundInstanceNpcKilled[num];
					break;
				case 5:
					if (SoundInstancePlayerKilled != null)
					{
						SoundInstancePlayerKilled.Stop();
					}
					SoundInstancePlayerKilled = SoundPlayerKilled.Value.CreateInstance();
					SoundInstancePlayerKilled.Volume = num11;
					SoundInstancePlayerKilled.Pan = num18;
					soundEffectInstance = SoundInstancePlayerKilled;
					break;
				case 6:
					if (SoundInstanceGrass != null)
					{
						SoundInstanceGrass.Stop();
					}
					SoundInstanceGrass = SoundGrass.Value.CreateInstance();
					SoundInstanceGrass.Volume = num11;
					SoundInstanceGrass.Pan = num18;
					SoundInstanceGrass.Pitch = (float)Main.rand.Next(-30, 31) * 0.01f;
					soundEffectInstance = SoundInstanceGrass;
					break;
				case 7:
					if (SoundInstanceGrab != null)
					{
						SoundInstanceGrab.Stop();
					}
					SoundInstanceGrab = SoundGrab.Value.CreateInstance();
					SoundInstanceGrab.Volume = num11;
					SoundInstanceGrab.Pan = num18;
					SoundInstanceGrab.Pitch = (float)Main.rand.Next(-10, 11) * 0.01f;
					soundEffectInstance = SoundInstanceGrab;
					break;
				case 8:
					if (SoundInstanceDoorOpen != null)
					{
						SoundInstanceDoorOpen.Stop();
					}
					SoundInstanceDoorOpen = SoundDoorOpen.Value.CreateInstance();
					SoundInstanceDoorOpen.Volume = num11;
					SoundInstanceDoorOpen.Pan = num18;
					SoundInstanceDoorOpen.Pitch = (float)Main.rand.Next(-20, 21) * 0.01f;
					soundEffectInstance = SoundInstanceDoorOpen;
					break;
				case 9:
					if (SoundInstanceDoorClosed != null)
					{
						SoundInstanceDoorClosed.Stop();
					}
					SoundInstanceDoorClosed = SoundDoorClosed.Value.CreateInstance();
					SoundInstanceDoorClosed.Volume = num11;
					SoundInstanceDoorClosed.Pan = num18;
					SoundInstanceDoorClosed.Pitch = (float)Main.rand.Next(-20, 21) * 0.01f;
					soundEffectInstance = SoundInstanceDoorClosed;
					break;
				case 10:
					if (SoundInstanceMenuOpen != null)
					{
						SoundInstanceMenuOpen.Stop();
					}
					SoundInstanceMenuOpen = SoundMenuOpen.Value.CreateInstance();
					SoundInstanceMenuOpen.Volume = num11;
					SoundInstanceMenuOpen.Pan = num18;
					soundEffectInstance = SoundInstanceMenuOpen;
					break;
				case 11:
					if (SoundInstanceMenuClose != null)
					{
						SoundInstanceMenuClose.Stop();
					}
					SoundInstanceMenuClose = SoundMenuClose.Value.CreateInstance();
					SoundInstanceMenuClose.Volume = num11;
					SoundInstanceMenuClose.Pan = num18;
					soundEffectInstance = SoundInstanceMenuClose;
					break;
				case 12:
					if (Main.hasFocus)
					{
						if (SoundInstanceMenuTick != null)
						{
							SoundInstanceMenuTick.Stop();
						}
						SoundInstanceMenuTick = SoundMenuTick.Value.CreateInstance();
						SoundInstanceMenuTick.Volume = num11;
						SoundInstanceMenuTick.Pan = num18;
						soundEffectInstance = SoundInstanceMenuTick;
					}
					break;
				case 13:
					if (SoundInstanceShatter != null)
					{
						SoundInstanceShatter.Stop();
					}
					SoundInstanceShatter = SoundShatter.Value.CreateInstance();
					SoundInstanceShatter.Volume = num11;
					SoundInstanceShatter.Pan = num18;
					soundEffectInstance = SoundInstanceShatter;
					break;
				case 14:
					switch (Style)
					{
					case 542:
					{
						int num16 = 7;
						SoundInstanceZombie[num16] = SoundZombie[num16].Value.CreateInstance();
						SoundInstanceZombie[num16].Volume = num11 * 0.4f;
						SoundInstanceZombie[num16].Pan = num18;
						soundEffectInstance = SoundInstanceZombie[num16];
						break;
					}
					case 489:
					case 586:
					{
						int num15 = Main.rand.Next(21, 24);
						SoundInstanceZombie[num15] = SoundZombie[num15].Value.CreateInstance();
						SoundInstanceZombie[num15].Volume = num11 * 0.4f;
						SoundInstanceZombie[num15].Pan = num18;
						soundEffectInstance = SoundInstanceZombie[num15];
						break;
					}
					default:
					{
						int num14 = Main.rand.Next(3);
						SoundInstanceZombie[num14] = SoundZombie[num14].Value.CreateInstance();
						SoundInstanceZombie[num14].Volume = num11 * 0.4f;
						SoundInstanceZombie[num14].Pan = num18;
						soundEffectInstance = SoundInstanceZombie[num14];
						break;
					}
					}
					break;
				case 15:
				{
					float num13 = 1f;
					if (num == 4)
					{
						num = 1;
						num13 = 0.25f;
					}
					if (SoundInstanceRoar[num] == null || (int)SoundInstanceRoar[num].State == 2)
					{
						SoundInstanceRoar[num] = SoundRoar[num].Value.CreateInstance();
						SoundInstanceRoar[num].Volume = num11 * num13;
						SoundInstanceRoar[num].Pan = num18;
						soundEffectInstance = SoundInstanceRoar[num];
					}
					break;
				}
				case 16:
					if (SoundInstanceDoubleJump != null)
					{
						SoundInstanceDoubleJump.Stop();
					}
					SoundInstanceDoubleJump = SoundDoubleJump.Value.CreateInstance();
					SoundInstanceDoubleJump.Volume = num11;
					SoundInstanceDoubleJump.Pan = num18;
					SoundInstanceDoubleJump.Pitch = (float)Main.rand.Next(-10, 11) * 0.01f;
					soundEffectInstance = SoundInstanceDoubleJump;
					break;
				case 17:
					if (SoundInstanceRun != null)
					{
						SoundInstanceRun.Stop();
					}
					SoundInstanceRun = SoundRun.Value.CreateInstance();
					SoundInstanceRun.Volume = num11;
					SoundInstanceRun.Pan = num18;
					SoundInstanceRun.Pitch = (float)Main.rand.Next(-10, 11) * 0.01f;
					soundEffectInstance = SoundInstanceRun;
					break;
				case 18:
					SoundInstanceCoins = SoundCoins.Value.CreateInstance();
					SoundInstanceCoins.Volume = num11;
					SoundInstanceCoins.Pan = num18;
					soundEffectInstance = SoundInstanceCoins;
					break;
				case 19:
					if (SoundInstanceSplash[num] != null && (int)SoundInstanceSplash[num].State != 2)
					{
						break;
					}
					SoundInstanceSplash[num] = SoundSplash[num].Value.CreateInstance();
					if (num == 2 || num == 3)
					{
						num11 *= 0.75f;
					}
					if (num == 4 || num == 5)
					{
						num11 *= 0.75f;
						SoundInstanceSplash[num].Pitch = (float)Main.rand.Next(-20, 1) * 0.01f;
					}
					else
					{
						SoundInstanceSplash[num].Pitch = (float)Main.rand.Next(-10, 11) * 0.01f;
					}
					SoundInstanceSplash[num].Volume = num11;
					SoundInstanceSplash[num].Pan = num18;
					switch (num)
					{
					case 4:
						if (SoundInstanceSplash[5] == null || (int)SoundInstanceSplash[5].State == 2)
						{
							soundEffectInstance = SoundInstanceSplash[num];
						}
						break;
					case 5:
						if (SoundInstanceSplash[4] == null || (int)SoundInstanceSplash[4].State == 2)
						{
							soundEffectInstance = SoundInstanceSplash[num];
						}
						break;
					default:
						soundEffectInstance = SoundInstanceSplash[num];
						break;
					}
					break;
				case 20:
				{
					int num17 = Main.rand.Next(3);
					if (SoundInstanceFemaleHit[num17] != null)
					{
						SoundInstanceFemaleHit[num17].Stop();
					}
					SoundInstanceFemaleHit[num17] = SoundFemaleHit[num17].Value.CreateInstance();
					SoundInstanceFemaleHit[num17].Volume = num11;
					SoundInstanceFemaleHit[num17].Pan = num18;
					soundEffectInstance = SoundInstanceFemaleHit[num17];
					break;
				}
				case 21:
				{
					int num12 = Main.rand.Next(3);
					if (SoundInstanceTink[num12] != null)
					{
						SoundInstanceTink[num12].Stop();
					}
					SoundInstanceTink[num12] = SoundTink[num12].Value.CreateInstance();
					SoundInstanceTink[num12].Volume = num11;
					SoundInstanceTink[num12].Pan = num18;
					soundEffectInstance = SoundInstanceTink[num12];
					break;
				}
				case 22:
					if (SoundInstanceUnlock != null)
					{
						SoundInstanceUnlock.Stop();
					}
					SoundInstanceUnlock = SoundUnlock.Value.CreateInstance();
					SoundInstanceUnlock.Volume = num11;
					SoundInstanceUnlock.Pan = num18;
					soundEffectInstance = SoundInstanceUnlock;
					break;
				case 23:
					if (SoundInstanceDrown != null)
					{
						SoundInstanceDrown.Stop();
					}
					SoundInstanceDrown = SoundDrown.Value.CreateInstance();
					SoundInstanceDrown.Volume = num11;
					SoundInstanceDrown.Pan = num18;
					soundEffectInstance = SoundInstanceDrown;
					break;
				case 24:
					SoundInstanceChat = SoundChat.Value.CreateInstance();
					SoundInstanceChat.Volume = num11;
					SoundInstanceChat.Pan = num18;
					soundEffectInstance = SoundInstanceChat;
					break;
				case 25:
					SoundInstanceMaxMana = SoundMaxMana.Value.CreateInstance();
					SoundInstanceMaxMana.Volume = num11;
					SoundInstanceMaxMana.Pan = num18;
					soundEffectInstance = SoundInstanceMaxMana;
					break;
				case 26:
				{
					int num10 = Main.rand.Next(3, 5);
					SoundInstanceZombie[num10] = SoundZombie[num10].Value.CreateInstance();
					SoundInstanceZombie[num10].Volume = num11 * 0.9f;
					SoundInstanceZombie[num10].Pan = num18;
					SoundInstanceZombie[num10].Pitch = (float)Main.rand.Next(-10, 11) * 0.01f;
					soundEffectInstance = SoundInstanceZombie[num10];
					break;
				}
				case 27:
					if (SoundInstancePixie != null && (int)SoundInstancePixie.State == 0)
					{
						SoundInstancePixie.Volume = num11;
						SoundInstancePixie.Pan = num18;
						SoundInstancePixie.Pitch = (float)Main.rand.Next(-10, 11) * 0.01f;
						return null;
					}
					if (SoundInstancePixie != null)
					{
						SoundInstancePixie.Stop();
					}
					SoundInstancePixie = SoundPixie.Value.CreateInstance();
					SoundInstancePixie.Volume = num11;
					SoundInstancePixie.Pan = num18;
					SoundInstancePixie.Pitch = (float)Main.rand.Next(-10, 11) * 0.01f;
					soundEffectInstance = SoundInstancePixie;
					break;
				case 28:
					if (SoundInstanceMech[num] != null && (int)SoundInstanceMech[num].State == 0)
					{
						return null;
					}
					SoundInstanceMech[num] = SoundMech[num].Value.CreateInstance();
					SoundInstanceMech[num].Volume = num11;
					SoundInstanceMech[num].Pan = num18;
					SoundInstanceMech[num].Pitch = (float)Main.rand.Next(-10, 11) * 0.01f;
					soundEffectInstance = SoundInstanceMech[num];
					break;
				case 29:
					if (num >= 24 && num <= 87)
					{
						num11 *= 0.5f;
					}
					if (num >= 88 && num <= 91)
					{
						num11 *= 0.7f;
					}
					if (num >= 93 && num <= 99)
					{
						num11 *= 0.4f;
					}
					if (num == 92)
					{
						num11 *= 0.5f;
					}
					if (num == 103)
					{
						num11 *= 0.4f;
					}
					if (num == 104)
					{
						num11 *= 0.55f;
					}
					if (num == 100 || num == 101)
					{
						num11 *= 0.25f;
					}
					if (num == 102)
					{
						num11 *= 0.4f;
					}
					if (SoundInstanceZombie[num] != null && (int)SoundInstanceZombie[num].State == 0)
					{
						return null;
					}
					SoundInstanceZombie[num] = SoundZombie[num].Value.CreateInstance();
					SoundInstanceZombie[num].Volume = num11;
					SoundInstanceZombie[num].Pan = num18;
					SoundInstanceZombie[num].Pitch = (float)Main.rand.Next(-10, 11) * 0.01f;
					soundEffectInstance = SoundInstanceZombie[num];
					break;
				case 44:
					num = Main.rand.Next(106, 109);
					SoundInstanceZombie[num] = SoundZombie[num].Value.CreateInstance();
					SoundInstanceZombie[num].Volume = num11 * 0.2f;
					SoundInstanceZombie[num].Pan = num18;
					SoundInstanceZombie[num].Pitch = (float)Main.rand.Next(-70, 1) * 0.01f;
					soundEffectInstance = SoundInstanceZombie[num];
					break;
				case 45:
					num = 109;
					if (SoundInstanceZombie[num] != null && (int)SoundInstanceZombie[num].State == 0)
					{
						return null;
					}
					SoundInstanceZombie[num] = SoundZombie[num].Value.CreateInstance();
					SoundInstanceZombie[num].Volume = num11 * 0.3f;
					SoundInstanceZombie[num].Pan = num18;
					SoundInstanceZombie[num].Pitch = (float)Main.rand.Next(-10, 11) * 0.01f;
					soundEffectInstance = SoundInstanceZombie[num];
					break;
				case 46:
					if (SoundInstanceZombie[110] != null && (int)SoundInstanceZombie[110].State == 0)
					{
						return null;
					}
					if (SoundInstanceZombie[111] != null && (int)SoundInstanceZombie[111].State == 0)
					{
						return null;
					}
					num = Main.rand.Next(110, 112);
					if (Main.rand.Next(300) == 0)
					{
						num = ((Main.rand.Next(3) == 0) ? 114 : ((Main.rand.Next(2) != 0) ? 112 : 113));
					}
					SoundInstanceZombie[num] = SoundZombie[num].Value.CreateInstance();
					SoundInstanceZombie[num].Volume = num11 * 0.9f;
					SoundInstanceZombie[num].Pan = num18;
					SoundInstanceZombie[num].Pitch = (float)Main.rand.Next(-10, 11) * 0.01f;
					soundEffectInstance = SoundInstanceZombie[num];
					break;
				default:
					switch (type)
					{
					case 45:
						num = 109;
						SoundInstanceZombie[num] = SoundZombie[num].Value.CreateInstance();
						SoundInstanceZombie[num].Volume = num11 * 0.2f;
						SoundInstanceZombie[num].Pan = num18;
						SoundInstanceZombie[num].Pitch = (float)Main.rand.Next(-70, 1) * 0.01f;
						soundEffectInstance = SoundInstanceZombie[num];
						break;
					case 30:
						num = Main.rand.Next(10, 12);
						if (Main.rand.Next(300) == 0)
						{
							num = 12;
							if (SoundInstanceZombie[num] != null && (int)SoundInstanceZombie[num].State == 0)
							{
								return null;
							}
						}
						SoundInstanceZombie[num] = SoundZombie[num].Value.CreateInstance();
						SoundInstanceZombie[num].Volume = num11 * 0.75f;
						SoundInstanceZombie[num].Pan = num18;
						if (num != 12)
						{
							SoundInstanceZombie[num].Pitch = (float)Main.rand.Next(-70, 1) * 0.01f;
						}
						else
						{
							SoundInstanceZombie[num].Pitch = (float)Main.rand.Next(-40, 21) * 0.01f;
						}
						soundEffectInstance = SoundInstanceZombie[num];
						break;
					case 31:
						num = 13;
						SoundInstanceZombie[num] = SoundZombie[num].Value.CreateInstance();
						SoundInstanceZombie[num].Volume = num11 * 0.35f;
						SoundInstanceZombie[num].Pan = num18;
						SoundInstanceZombie[num].Pitch = (float)Main.rand.Next(-40, 21) * 0.01f;
						soundEffectInstance = SoundInstanceZombie[num];
						break;
					case 32:
						if (SoundInstanceZombie[num] != null && (int)SoundInstanceZombie[num].State == 0)
						{
							return null;
						}
						SoundInstanceZombie[num] = SoundZombie[num].Value.CreateInstance();
						SoundInstanceZombie[num].Volume = num11 * 0.15f;
						SoundInstanceZombie[num].Pan = num18;
						SoundInstanceZombie[num].Pitch = (float)Main.rand.Next(-70, 26) * 0.01f;
						soundEffectInstance = SoundInstanceZombie[num];
						break;
					case 67:
						num = Main.rand.Next(118, 121);
						if (SoundInstanceZombie[num] != null && (int)SoundInstanceZombie[num].State == 0)
						{
							return null;
						}
						SoundInstanceZombie[num] = SoundZombie[num].Value.CreateInstance();
						SoundInstanceZombie[num].Volume = num11 * 0.3f;
						SoundInstanceZombie[num].Pan = num18;
						SoundInstanceZombie[num].Pitch = (float)Main.rand.Next(-5, 6) * 0.01f;
						soundEffectInstance = SoundInstanceZombie[num];
						break;
					case 68:
						num = Main.rand.Next(126, 129);
						if (SoundInstanceZombie[num] != null && (int)SoundInstanceZombie[num].State == 0)
						{
							return null;
						}
						SoundInstanceZombie[num] = SoundZombie[num].Value.CreateInstance();
						SoundInstanceZombie[num].Volume = num11 * 0.22f;
						SoundInstanceZombie[num].Pan = num18;
						SoundInstanceZombie[num].Pitch = (float)Main.rand.Next(-5, 6) * 0.01f;
						soundEffectInstance = SoundInstanceZombie[num];
						break;
					case 69:
						num = Main.rand.Next(129, 131);
						if (SoundInstanceZombie[num] != null && (int)SoundInstanceZombie[num].State == 0)
						{
							return null;
						}
						SoundInstanceZombie[num] = SoundZombie[num].Value.CreateInstance();
						SoundInstanceZombie[num].Volume = num11 * 0.2f;
						SoundInstanceZombie[num].Pan = num18;
						SoundInstanceZombie[num].Pitch = (float)Main.rand.Next(-5, 6) * 0.01f;
						soundEffectInstance = SoundInstanceZombie[num];
						break;
					case 66:
						num = Main.rand.Next(121, 124);
						if (SoundInstanceZombie[121] != null && (int)SoundInstanceZombie[121].State == 0)
						{
							return null;
						}
						if (SoundInstanceZombie[122] != null && (int)SoundInstanceZombie[122].State == 0)
						{
							return null;
						}
						if (SoundInstanceZombie[123] != null && (int)SoundInstanceZombie[123].State == 0)
						{
							return null;
						}
						SoundInstanceZombie[num] = SoundZombie[num].Value.CreateInstance();
						SoundInstanceZombie[num].Volume = num11 * 0.45f;
						SoundInstanceZombie[num].Pan = num18;
						SoundInstanceZombie[num].Pitch = (float)Main.rand.Next(-15, 16) * 0.01f;
						soundEffectInstance = SoundInstanceZombie[num];
						break;
					case 33:
						num = 15;
						if (SoundInstanceZombie[num] != null && (int)SoundInstanceZombie[num].State == 0)
						{
							return null;
						}
						SoundInstanceZombie[num] = SoundZombie[num].Value.CreateInstance();
						SoundInstanceZombie[num].Volume = num11 * 0.2f;
						SoundInstanceZombie[num].Pan = num18;
						SoundInstanceZombie[num].Pitch = (float)Main.rand.Next(-10, 31) * 0.01f;
						soundEffectInstance = SoundInstanceZombie[num];
						break;
					case 47:
					case 48:
					case 49:
					case 50:
					case 51:
					case 52:
					{
						num = 133 + type - 47;
						for (int i = 133; i <= 138; i++)
						{
							if (SoundInstanceItem[i] != null && (int)SoundInstanceItem[i].State == 0)
							{
								SoundInstanceItem[i].Stop();
							}
						}
						SoundInstanceItem[num] = SoundItem[num].Value.CreateInstance();
						SoundInstanceItem[num].Volume = num11 * 0.45f;
						SoundInstanceItem[num].Pan = num18;
						soundEffectInstance = SoundInstanceItem[num];
						break;
					}
					default:
						if (type >= 53 && type <= 62)
						{
							num = 139 + type - 53;
							if (SoundInstanceItem[num] != null && (int)SoundInstanceItem[num].State == 0)
							{
								SoundInstanceItem[num].Stop();
							}
							SoundInstanceItem[num] = SoundItem[num].Value.CreateInstance();
							SoundInstanceItem[num].Volume = num11 * 0.7f;
							SoundInstanceItem[num].Pan = num18;
							soundEffectInstance = SoundInstanceItem[num];
							break;
						}
						switch (type)
						{
						case 34:
						{
							float num4 = (float)num / 50f;
							if (num4 > 1f)
							{
								num4 = 1f;
							}
							num11 *= num4;
							num11 *= 0.2f;
							num11 *= 1f - Main.shimmerAlpha;
							if (num11 <= 0f || x == -1 || y == -1)
							{
								if (SoundInstanceLiquid[0] != null && (int)SoundInstanceLiquid[0].State == 0)
								{
									SoundInstanceLiquid[0].Stop();
								}
							}
							else if (SoundInstanceLiquid[0] != null && (int)SoundInstanceLiquid[0].State == 0)
							{
								SoundInstanceLiquid[0].Volume = num11;
								SoundInstanceLiquid[0].Pan = num18;
								SoundInstanceLiquid[0].Pitch = -0.2f;
							}
							else
							{
								SoundInstanceLiquid[0] = SoundLiquid[0].Value.CreateInstance();
								SoundInstanceLiquid[0].Volume = num11;
								SoundInstanceLiquid[0].Pan = num18;
								soundEffectInstance = SoundInstanceLiquid[0];
							}
							break;
						}
						case 35:
						{
							float num2 = (float)num / 50f;
							if (num2 > 1f)
							{
								num2 = 1f;
							}
							num11 *= num2;
							num11 *= 0.65f;
							num11 *= 1f - Main.shimmerAlpha;
							if (num11 <= 0f || x == -1 || y == -1)
							{
								if (SoundInstanceLiquid[1] != null && (int)SoundInstanceLiquid[1].State == 0)
								{
									SoundInstanceLiquid[1].Stop();
								}
							}
							else if (SoundInstanceLiquid[1] != null && (int)SoundInstanceLiquid[1].State == 0)
							{
								SoundInstanceLiquid[1].Volume = num11;
								SoundInstanceLiquid[1].Pan = num18;
								SoundInstanceLiquid[1].Pitch = -0f;
							}
							else
							{
								SoundInstanceLiquid[1] = SoundLiquid[1].Value.CreateInstance();
								SoundInstanceLiquid[1].Volume = num11;
								SoundInstanceLiquid[1].Pan = num18;
								soundEffectInstance = SoundInstanceLiquid[1];
							}
							break;
						}
						case 36:
						{
							int num3 = Style;
							if (Style == -1)
							{
								num3 = 0;
							}
							SoundInstanceRoar[num3] = SoundRoar[num3].Value.CreateInstance();
							SoundInstanceRoar[num3].Volume = num11;
							SoundInstanceRoar[num3].Pan = num18;
							if (Style == -1)
							{
								SoundEffectInstance obj = SoundInstanceRoar[num3];
								obj.Pitch += 0.6f;
							}
							soundEffectInstance = SoundInstanceRoar[num3];
							break;
						}
						case 37:
						{
							int num23 = Main.rand.Next(57, 59);
							num11 = ((!Main.starGame) ? (num11 * ((float)Style * 0.05f)) : (num11 * 0.15f));
							SoundInstanceItem[num23] = SoundItem[num23].Value.CreateInstance();
							SoundInstanceItem[num23].Volume = num11;
							SoundInstanceItem[num23].Pan = num18;
							SoundInstanceItem[num23].Pitch = (float)Main.rand.Next(-40, 41) * 0.01f;
							soundEffectInstance = SoundInstanceItem[num23];
							break;
						}
						case 38:
						{
							if (Main.starGame)
							{
								num11 *= 0.15f;
							}
							int num5 = Main.rand.Next(5);
							SoundInstanceCoin[num5] = SoundCoin[num5].Value.CreateInstance();
							SoundInstanceCoin[num5].Volume = num11;
							SoundInstanceCoin[num5].Pan = num18;
							SoundInstanceCoin[num5].Pitch = (float)Main.rand.Next(-40, 41) * 0.002f;
							soundEffectInstance = SoundInstanceCoin[num5];
							break;
						}
						case 39:
							num = Style;
							SoundInstanceDrip[num] = SoundDrip[num].Value.CreateInstance();
							SoundInstanceDrip[num].Volume = num11 * 0.5f;
							SoundInstanceDrip[num].Pan = num18;
							SoundInstanceDrip[num].Pitch = (float)Main.rand.Next(-30, 31) * 0.01f;
							soundEffectInstance = SoundInstanceDrip[num];
							break;
						case 40:
							if (SoundInstanceCamera != null)
							{
								SoundInstanceCamera.Stop();
							}
							SoundInstanceCamera = SoundCamera.Value.CreateInstance();
							SoundInstanceCamera.Volume = num11;
							SoundInstanceCamera.Pan = num18;
							soundEffectInstance = SoundInstanceCamera;
							break;
						case 41:
						{
							SoundInstanceMoonlordCry = SoundNpcKilled[10].Value.CreateInstance();
							SoundEffectInstance soundInstanceMoonlordCry = SoundInstanceMoonlordCry;
							Vector2 val = new Vector2((float)x, (float)y) - Main.player[Main.myPlayer].position;
							soundInstanceMoonlordCry.Volume = 1f / (1f + ((Vector2)(ref val)).Length());
							SoundInstanceMoonlordCry.Pan = num18;
							SoundInstanceMoonlordCry.Pitch = (float)Main.rand.Next(-10, 11) * 0.01f;
							soundEffectInstance = SoundInstanceMoonlordCry;
							break;
						}
						case 42:
							soundEffectInstance = TrackableSounds[num].Value.CreateInstance();
							soundEffectInstance.Volume = num11;
							soundEffectInstance.Pan = num18;
							TrackableSoundInstances[num] = soundEffectInstance;
							break;
						case 65:
						{
							if (SoundInstanceZombie[115] != null && (int)SoundInstanceZombie[115].State == 0)
							{
								return null;
							}
							if (SoundInstanceZombie[116] != null && (int)SoundInstanceZombie[116].State == 0)
							{
								return null;
							}
							if (SoundInstanceZombie[117] != null && (int)SoundInstanceZombie[117].State == 0)
							{
								return null;
							}
							int num22 = Main.rand.Next(115, 118);
							SoundInstanceZombie[num22] = SoundZombie[num22].Value.CreateInstance();
							SoundInstanceZombie[num22].Volume = num11 * 0.5f;
							SoundInstanceZombie[num22].Pan = num18;
							soundEffectInstance = SoundInstanceZombie[num22];
							break;
						}
						}
						break;
					}
					break;
				}
				if (soundEffectInstance != null)
				{
					SoundEffectInstance obj4 = soundEffectInstance;
					obj4.Pitch += pitchOffset;
					SoundEffectInstance obj5 = soundEffectInstance;
					obj5.Volume *= volumeScale;
					soundEffectInstance.Play();
					SoundInstanceGarbageCollector.Track(soundEffectInstance);
				}
				return soundEffectInstance;
			}
		}
		catch
		{
		}
		return null;
	}

	public SoundEffect GetTrackableSoundByStyleId(int id)
	{
		return TrackableSounds[id].Value;
	}

	public void StopAmbientSounds()
	{
		for (int i = 0; i < SoundInstanceLiquid.Length; i++)
		{
			if (SoundInstanceLiquid[i] != null)
			{
				SoundInstanceLiquid[i].Stop();
			}
		}
	}

	public bool DoesSoundScaleWithAmbientVolume(int soundType)
	{
		switch (soundType)
		{
		case 30:
		case 31:
		case 32:
		case 33:
		case 34:
		case 35:
		case 39:
		case 43:
		case 44:
		case 45:
		case 46:
		case 67:
		case 68:
		case 69:
			return true;
		default:
			return false;
		}
	}
}
