using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria.Chat;
using Terraria.DataStructures;
using Terraria.GameContent.NetModules;
using Terraria.GameContent.Tile_Entities;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.Default;
using Terraria.Net;

namespace Terraria.GameContent;

public class TeleportPylonsSystem : IOnPlayerJoining
{
	private List<TeleportPylonInfo> _pylons = new List<TeleportPylonInfo>();

	private List<TeleportPylonInfo> _pylonsOld = new List<TeleportPylonInfo>();

	private int _cooldownForUpdatingPylonsList;

	private const int CooldownTimePerPylonsListUpdate = int.MaxValue;

	internal SceneMetrics _sceneMetrics = new SceneMetrics();

	public IReadOnlyList<TeleportPylonInfo> Pylons => _pylons;

	public void Update()
	{
		if (Main.netMode != 1)
		{
			if (_cooldownForUpdatingPylonsList > 0)
			{
				_cooldownForUpdatingPylonsList--;
				return;
			}
			_cooldownForUpdatingPylonsList = int.MaxValue;
			UpdatePylonsListAndBroadcastChanges();
		}
	}

	public bool HasPylonOfType(TeleportPylonType pylonType)
	{
		return _pylons.Any((TeleportPylonInfo x) => x.TypeOfPylon == pylonType);
	}

	public bool HasAnyPylon()
	{
		return _pylons.Count > 0;
	}

	public void RequestImmediateUpdate()
	{
		if (Main.netMode != 1)
		{
			_cooldownForUpdatingPylonsList = int.MaxValue;
			UpdatePylonsListAndBroadcastChanges();
		}
	}

	private void UpdatePylonsListAndBroadcastChanges()
	{
		Utils.Swap(ref _pylons, ref _pylonsOld);
		_pylons.Clear();
		foreach (TileEntity value in TileEntity.ByPosition.Values)
		{
			if (!(value is IPylonTileEntity))
			{
				continue;
			}
			TeleportPylonInfo teleportPylonInfo = default(TeleportPylonInfo);
			teleportPylonInfo.PositionInTiles = value.Position;
			if (value is TETeleportationPylon vanillaPylon && vanillaPylon.TryGetPylonType(out var vanillaType))
			{
				teleportPylonInfo.TypeOfPylon = vanillaType;
			}
			else
			{
				if (!TEModdedPylon.GetModPylonFromCoords(value.Position.X, value.Position.Y, out var modPylon))
				{
					continue;
				}
				teleportPylonInfo.TypeOfPylon = modPylon.PylonType;
			}
			TeleportPylonInfo item = teleportPylonInfo;
			_pylons.Add(item);
		}
		IEnumerable<TeleportPylonInfo> enumerable = _pylonsOld.Except(_pylons);
		foreach (TeleportPylonInfo item2 in _pylons.Except(_pylonsOld))
		{
			NetManager.Instance.BroadcastOrLoopback(NetTeleportPylonModule.SerializePylonWasAddedOrRemoved(item2, NetTeleportPylonModule.SubPacketType.PylonWasAdded));
		}
		foreach (TeleportPylonInfo item3 in enumerable)
		{
			NetManager.Instance.BroadcastOrLoopback(NetTeleportPylonModule.SerializePylonWasAddedOrRemoved(item3, NetTeleportPylonModule.SubPacketType.PylonWasRemoved));
		}
	}

	public void AddForClient(TeleportPylonInfo info)
	{
		if (!_pylons.Contains(info))
		{
			_pylons.Add(info);
		}
	}

	public void RemoveForClient(TeleportPylonInfo info)
	{
		_pylons.RemoveAll((TeleportPylonInfo x) => x.Equals(info));
	}

	public void HandleTeleportRequest(TeleportPylonInfo info, int playerIndex)
	{
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_035f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_02be: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0200: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0302: Unknown result type (might be due to invalid IL or missing references)
		//IL_0316: Unknown result type (might be due to invalid IL or missing references)
		//IL_032e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0335: Unknown result type (might be due to invalid IL or missing references)
		Player player = Main.player[playerIndex];
		string key = null;
		bool flag = true;
		if (flag)
		{
			flag &= IsPlayerNearAPylon(player);
			if (!flag)
			{
				key = "Net.CannotTeleportToPylonBecausePlayerIsNotNearAPylon";
			}
		}
		if (flag)
		{
			int necessaryNPCCount = HowManyNPCsDoesPylonNeed(info, player);
			flag &= DoesPylonHaveEnoughNPCsAroundIt(info, necessaryNPCCount);
			if (!flag)
			{
				key = "Net.CannotTeleportToPylonBecauseNotEnoughNPCs";
			}
		}
		if (flag)
		{
			flag &= !NPC.AnyDanger(quickBossNPCCheck: false, ignorePillarsAndMoonlordCountdown: true);
			bool? flag3 = PylonLoader.ValidTeleportCheck_PreAnyDanger(info);
			int num3;
			if (!flag3.HasValue)
			{
				num3 = ((info.ModPylon?.ValidTeleportCheck_AnyDanger(info) ?? flag) ? 1 : 0);
			}
			else
			{
				bool value = flag3.GetValueOrDefault();
				num3 = (value ? 1 : 0);
			}
			flag = (byte)num3 != 0;
			if (!flag)
			{
				key = "Net.CannotTeleportToPylonBecauseThereIsDanger";
			}
		}
		if (flag)
		{
			if (!NPC.downedPlantBoss && (double)info.PositionInTiles.Y > Main.worldSurface && Framing.GetTileSafely(info.PositionInTiles.X, info.PositionInTiles.Y).wall == 87)
			{
				flag = false;
			}
			if (!flag)
			{
				key = "Net.CannotTeleportToPylonBecauseAccessingLihzahrdTempleEarly";
			}
		}
		if (flag)
		{
			_sceneMetrics.ScanAndExportToMain(new SceneMetricsScanSettings
			{
				VisualScanArea = null,
				BiomeScanCenterPositionInWorld = info.PositionInTiles.ToWorldCoordinates(),
				ScanOreFinderData = false
			});
			flag = DoesPylonAcceptTeleportation(info, player);
			if (!flag)
			{
				key = "Net.CannotTeleportToPylonBecauseNotMeetingBiomeRequirements";
			}
		}
		info.ModPylon?.ValidTeleportCheck_DestinationPostCheck(info, ref flag, ref key);
		TeleportPylonInfo nearbyInfo = default(TeleportPylonInfo);
		bool flag2 = false;
		int num = 0;
		for (int i = 0; i < _pylons.Count; i++)
		{
			TeleportPylonInfo info2 = _pylons[i];
			if (!player.InInteractionRange(info2.PositionInTiles.X, info2.PositionInTiles.Y, TileReachCheckSettings.Pylons))
			{
				continue;
			}
			nearbyInfo = info2;
			if (num < 1)
			{
				num = 1;
			}
			int necessaryNPCCount2 = HowManyNPCsDoesPylonNeed(info2, player);
			if (DoesPylonHaveEnoughNPCsAroundIt(info2, necessaryNPCCount2))
			{
				if (num < 2)
				{
					num = 2;
				}
				_sceneMetrics.ScanAndExportToMain(new SceneMetricsScanSettings
				{
					VisualScanArea = null,
					BiomeScanCenterPositionInWorld = info2.PositionInTiles.ToWorldCoordinates(),
					ScanOreFinderData = false
				});
				if (DoesPylonAcceptTeleportation(info2, player))
				{
					flag2 = true;
					break;
				}
			}
		}
		if (!flag2)
		{
			key = num switch
			{
				1 => "Net.CannotTeleportToPylonBecauseNotEnoughNPCsAtCurrentPylon", 
				2 => "Net.CannotTeleportToPylonBecauseNotMeetingBiomeRequirements", 
				_ => "Net.CannotTeleportToPylonBecausePlayerIsNotNearAPylon", 
			};
		}
		nearbyInfo.ModPylon?.ValidTeleportCheck_NearbyPostCheck(nearbyInfo, ref flag, ref flag2, ref key);
		PylonLoader.PostValidTeleportCheck(info, nearbyInfo, ref flag, ref flag2, ref key);
		if (flag && flag2)
		{
			Vector2 newPos = info.PositionInTiles.ToWorldCoordinates() - new Vector2(0f, (float)player.HeightOffsetBoost);
			info.ModPylon?.ModifyTeleportationPosition(info, ref newPos);
			int num2 = 9;
			int typeOfPylon = (int)info.TypeOfPylon;
			int number = 0;
			player.Teleport(newPos, num2, typeOfPylon);
			player.velocity = Vector2.Zero;
			if (Main.netMode == 2)
			{
				RemoteClient.CheckSection(player.whoAmI, player.position);
				NetMessage.SendData(65, -1, -1, null, 0, player.whoAmI, newPos.X, newPos.Y, num2, number, typeOfPylon);
			}
		}
		else
		{
			ChatHelper.SendChatMessageToClient(NetworkText.FromKey(key), new Color(255, 240, 20), playerIndex);
		}
	}

	public static bool IsPlayerNearAPylon(Player player)
	{
		return TileID.Sets.CountsAsPylon.Any((int id) => player.IsTileTypeInInteractionRange(id, TileReachCheckSettings.Pylons));
	}

	private bool DoesPylonHaveEnoughNPCsAroundIt(TeleportPylonInfo info, int necessaryNPCCount)
	{
		bool? flag = PylonLoader.ValidTeleportCheck_PreNPCCount(info, ref necessaryNPCCount);
		if (flag.HasValue)
		{
			return flag.GetValueOrDefault();
		}
		ModPylon pylon = info.ModPylon;
		if (pylon != null)
		{
			return pylon.ValidTeleportCheck_NPCCount(info, necessaryNPCCount);
		}
		Point16 positionInTiles = info.PositionInTiles;
		return DoesPositionHaveEnoughNPCs(necessaryNPCCount, positionInTiles);
	}

	public static bool DoesPositionHaveEnoughNPCs(int necessaryNPCCount, Point16 centerPoint)
	{
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		if (necessaryNPCCount <= 0)
		{
			return true;
		}
		Rectangle rectangle = default(Rectangle);
		((Rectangle)(ref rectangle))._002Ector(centerPoint.X - Main.buffScanAreaWidth / 2, centerPoint.Y - Main.buffScanAreaHeight / 2, Main.buffScanAreaWidth, Main.buffScanAreaHeight);
		int num = necessaryNPCCount;
		Vector2 value2 = default(Vector2);
		for (int i = 0; i < 200; i++)
		{
			NPC nPC = Main.npc[i];
			if (!nPC.active || !nPC.isLikeATownNPC || nPC.homeless || !((Rectangle)(ref rectangle)).Contains(nPC.homeTileX, nPC.homeTileY))
			{
				continue;
			}
			Vector2 val = new Vector2((float)nPC.homeTileX, (float)nPC.homeTileY);
			((Vector2)(ref value2))._002Ector(nPC.Center.X / 16f, nPC.Center.Y / 16f);
			if (Vector2.Distance(val, value2) < 100f)
			{
				num--;
				if (num == 0)
				{
					return true;
				}
			}
		}
		return false;
	}

	public void RequestTeleportation(TeleportPylonInfo info, Player player)
	{
		NetManager.Instance.SendToServerOrLoopback(NetTeleportPylonModule.SerializeUseRequest(info));
	}

	private bool DoesPylonAcceptTeleportation(TeleportPylonInfo info, Player player)
	{
		if (Main.netMode != 2 && Main.DroneCameraTracker != null && Main.DroneCameraTracker.IsInUse())
		{
			return false;
		}
		bool? flag5 = PylonLoader.ValidTeleportCheck_PreBiomeRequirements(info, _sceneMetrics);
		if (flag5.HasValue)
		{
			return flag5.GetValueOrDefault();
		}
		ModPylon pylon = info.ModPylon;
		if (pylon != null)
		{
			return pylon.ValidTeleportCheck_BiomeRequirements(info, _sceneMetrics);
		}
		switch (info.TypeOfPylon)
		{
		case TeleportPylonType.SurfacePurity:
		{
			bool flag = (double)info.PositionInTiles.Y <= Main.worldSurface;
			if (Main.remixWorld)
			{
				flag = (double)info.PositionInTiles.Y > Main.rockLayer && info.PositionInTiles.Y < Main.maxTilesY - 350;
			}
			bool flag2 = info.PositionInTiles.X >= Main.maxTilesX - 380 || info.PositionInTiles.X <= 380;
			if (!flag || flag2)
			{
				return false;
			}
			if (_sceneMetrics.EnoughTilesForJungle || _sceneMetrics.EnoughTilesForSnow || _sceneMetrics.EnoughTilesForDesert || _sceneMetrics.EnoughTilesForGlowingMushroom || _sceneMetrics.EnoughTilesForHallow || _sceneMetrics.EnoughTilesForCrimson || _sceneMetrics.EnoughTilesForCorruption)
			{
				return false;
			}
			return true;
		}
		case TeleportPylonType.Jungle:
			return _sceneMetrics.EnoughTilesForJungle;
		case TeleportPylonType.Snow:
			return _sceneMetrics.EnoughTilesForSnow;
		case TeleportPylonType.Desert:
			return _sceneMetrics.EnoughTilesForDesert;
		case TeleportPylonType.Beach:
		{
			bool flag3 = (double)info.PositionInTiles.Y <= Main.worldSurface && (double)info.PositionInTiles.Y > Main.worldSurface * 0.3499999940395355;
			bool flag4 = info.PositionInTiles.X >= Main.maxTilesX - 380 || info.PositionInTiles.X <= 380;
			if (Main.remixWorld)
			{
				flag3 |= (double)info.PositionInTiles.Y > Main.rockLayer && info.PositionInTiles.Y < Main.maxTilesY - 350;
				flag4 |= (double)info.PositionInTiles.X < (double)Main.maxTilesX * 0.43 || (double)info.PositionInTiles.X > (double)Main.maxTilesX * 0.57;
			}
			return flag4 && flag3;
		}
		case TeleportPylonType.GlowingMushroom:
			if (Main.remixWorld && info.PositionInTiles.Y >= Main.maxTilesY - 200)
			{
				return false;
			}
			return _sceneMetrics.EnoughTilesForGlowingMushroom;
		case TeleportPylonType.Hallow:
			return _sceneMetrics.EnoughTilesForHallow;
		case TeleportPylonType.Underground:
			return (double)info.PositionInTiles.Y >= Main.worldSurface;
		case TeleportPylonType.Victory:
			return true;
		default:
			return true;
		}
	}

	private int HowManyNPCsDoesPylonNeed(TeleportPylonInfo info, Player player)
	{
		if (info.TypeOfPylon != TeleportPylonType.Victory)
		{
			return 2;
		}
		return 0;
	}

	public void Reset()
	{
		_pylons.Clear();
		_cooldownForUpdatingPylonsList = 0;
	}

	public void OnPlayerJoining(int playerIndex)
	{
		foreach (TeleportPylonInfo pylon in _pylons)
		{
			NetManager.Instance.SendToClient(NetTeleportPylonModule.SerializePylonWasAddedOrRemoved(pylon, NetTeleportPylonModule.SubPacketType.PylonWasAdded), playerIndex);
		}
	}

	public static void SpawnInWorldDust(int tileStyle, Rectangle dustBox)
	{
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		float r = 1f;
		float g = 1f;
		float b = 1f;
		switch ((byte)tileStyle)
		{
		case 0:
			r = 0.05f;
			g = 0.8f;
			b = 0.3f;
			break;
		case 1:
			r = 0.7f;
			g = 0.8f;
			b = 0.05f;
			break;
		case 2:
			r = 0.5f;
			g = 0.3f;
			b = 0.7f;
			break;
		case 3:
			r = 0.4f;
			g = 0.4f;
			b = 0.6f;
			break;
		case 4:
			r = 0.2f;
			g = 0.2f;
			b = 0.95f;
			break;
		case 5:
			r = 0.85f;
			g = 0.45f;
			b = 0.1f;
			break;
		case 6:
			r = 1f;
			g = 1f;
			b = 1.2f;
			break;
		case 7:
			r = 0.4f;
			g = 0.7f;
			b = 1.2f;
			break;
		case 8:
			r = 0.7f;
			g = 0.7f;
			b = 0.7f;
			break;
		}
		int num = Dust.NewDust(dustBox.TopLeft(), dustBox.Width, dustBox.Height, 43, 0f, 0f, 254, new Color(r, g, b, 1f), 0.5f);
		Dust obj = Main.dust[num];
		obj.velocity *= 0.1f;
		Main.dust[num].velocity.Y -= 0.2f;
	}
}
