using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader.Core;

namespace Terraria.ModLoader;

/// <summary>
/// This serves as the central class from which wall-related functions are supported and carried out.
/// </summary>
public static class WallLoader
{
	private delegate void DelegateNumDust(int i, int j, int type, bool fail, ref int num);

	private delegate bool DelegateCreateDust(int i, int j, int type, ref int dustType);

	private delegate bool DelegateDrop(int i, int j, int type, ref int dropType);

	private delegate void DelegateKillWall(int i, int j, int type, ref bool fail);

	private delegate void DelegateModifyLight(int i, int j, int type, ref float r, ref float g, ref float b);

	private delegate bool DelegateWallFrame(int i, int j, int type, bool randomizeFrame, ref int style, ref int frameNumber);

	private static int nextWall = WallID.Count;

	internal static readonly IList<ModWall> walls = new List<ModWall>();

	internal static readonly IList<GlobalWall> globalWalls = new List<GlobalWall>();

	/// <summary> Maps Wall type to the Item type that places the wall. </summary>
	internal static readonly Dictionary<int, int> wallTypeToItemType = new Dictionary<int, int>();

	private static bool loaded = false;

	private static Func<int, int, int, bool, bool>[] HookKillSound;

	private static DelegateNumDust[] HookNumDust;

	private static DelegateCreateDust[] HookCreateDust;

	private static DelegateDrop[] HookDrop;

	private static DelegateKillWall[] HookKillWall;

	private static Func<int, int, int, bool>[] HookCanPlace;

	private static Func<int, int, int, bool>[] HookCanExplode;

	private static DelegateModifyLight[] HookModifyLight;

	private static Action<int, int, int>[] HookRandomUpdate;

	private static DelegateWallFrame[] HookWallFrame;

	private static Func<int, int, int, SpriteBatch, bool>[] HookPreDraw;

	private static Action<int, int, int, SpriteBatch>[] HookPostDraw;

	private static Action<int, int, int, Item>[] HookPlaceInWorld;

	public static int WallCount => nextWall;

	internal static int ReserveWallID()
	{
		if (ModNet.AllowVanillaClients)
		{
			throw new Exception("Adding walls breaks vanilla client compatibility");
		}
		int result = nextWall;
		nextWall++;
		return result;
	}

	/// <summary>
	/// Gets the ModWall instance with the given type. If no ModWall with the given type exists, returns null.
	/// </summary>
	public static ModWall GetWall(int type)
	{
		if (type < WallID.Count || type >= WallCount)
		{
			return null;
		}
		return walls[type - WallID.Count];
	}

	private static void Resize2DArray<T>(ref T[,] array, int newSize)
	{
		int dim1 = array.GetLength(0);
		int dim2 = array.GetLength(1);
		T[,] newArray = new T[newSize, dim2];
		for (int i = 0; i < newSize && i < dim1; i++)
		{
			for (int j = 0; j < dim2; j++)
			{
				newArray[i, j] = array[i, j];
			}
		}
		array = newArray;
	}

	internal static void ResizeArrays(bool unloading = false)
	{
		Array.Resize(ref TextureAssets.Wall, nextWall);
		LoaderUtils.ResetStaticMembers(typeof(WallID));
		Array.Resize(ref Main.wallHouse, nextWall);
		Array.Resize(ref Main.wallDungeon, nextWall);
		Array.Resize(ref Main.wallLight, nextWall);
		Array.Resize(ref Main.wallBlend, nextWall);
		Array.Resize(ref Main.wallLargeFrames, nextWall);
		Array.Resize(ref Main.wallFrame, nextWall);
		Array.Resize(ref Main.wallFrameCounter, nextWall);
		ModLoader.BuildGlobalHook<GlobalWall, Func<int, int, int, bool, bool>>(ref HookKillSound, globalWalls, (GlobalWall g) => g.KillSound);
		ModLoader.BuildGlobalHook<GlobalWall, DelegateNumDust>(ref HookNumDust, globalWalls, (GlobalWall g) => g.NumDust);
		ModLoader.BuildGlobalHook<GlobalWall, DelegateCreateDust>(ref HookCreateDust, globalWalls, (GlobalWall g) => g.CreateDust);
		ModLoader.BuildGlobalHook<GlobalWall, DelegateDrop>(ref HookDrop, globalWalls, (GlobalWall g) => g.Drop);
		ModLoader.BuildGlobalHook<GlobalWall, DelegateKillWall>(ref HookKillWall, globalWalls, (GlobalWall g) => g.KillWall);
		ModLoader.BuildGlobalHook<GlobalWall, DelegateWallFrame>(ref HookWallFrame, globalWalls, (GlobalWall g) => g.WallFrame);
		ModLoader.BuildGlobalHook<GlobalWall, Func<int, int, int, bool>>(ref HookCanPlace, globalWalls, (GlobalWall g) => g.CanPlace);
		ModLoader.BuildGlobalHook<GlobalWall, Func<int, int, int, bool>>(ref HookCanExplode, globalWalls, (GlobalWall g) => g.CanExplode);
		ModLoader.BuildGlobalHook<GlobalWall, DelegateModifyLight>(ref HookModifyLight, globalWalls, (GlobalWall g) => g.ModifyLight);
		ModLoader.BuildGlobalHook<GlobalWall, Action<int, int, int>>(ref HookRandomUpdate, globalWalls, (GlobalWall g) => g.RandomUpdate);
		ModLoader.BuildGlobalHook<GlobalWall, Func<int, int, int, SpriteBatch, bool>>(ref HookPreDraw, globalWalls, (GlobalWall g) => g.PreDraw);
		ModLoader.BuildGlobalHook<GlobalWall, Action<int, int, int, SpriteBatch>>(ref HookPostDraw, globalWalls, (GlobalWall g) => g.PostDraw);
		ModLoader.BuildGlobalHook<GlobalWall, Action<int, int, int, Item>>(ref HookPlaceInWorld, globalWalls, (GlobalWall g) => g.PlaceInWorld);
		if (!unloading)
		{
			loaded = true;
		}
	}

	internal static void Unload()
	{
		loaded = false;
		walls.Clear();
		nextWall = WallID.Count;
		globalWalls.Clear();
		wallTypeToItemType.Clear();
	}

	internal static void WriteType(ushort wall, byte[] data, ref int index, ref byte flags)
	{
		if (wall > 255)
		{
			data[index] = (byte)(wall >> 8);
			index++;
			flags |= 32;
		}
	}

	internal static void ReadType(ref ushort wall, BinaryReader reader, byte flags, IDictionary<int, int> wallTable)
	{
		if ((flags & 0x20) == 32)
		{
			wall |= (ushort)(reader.ReadByte() << 8);
		}
		if (wallTable.ContainsKey(wall))
		{
			wall = (ushort)wallTable[wall];
		}
	}

	public static bool KillSound(int i, int j, int type, bool fail)
	{
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		Func<int, int, int, bool, bool>[] hookKillSound = HookKillSound;
		for (int k = 0; k < hookKillSound.Length; k++)
		{
			if (!hookKillSound[k](i, j, type, fail))
			{
				return false;
			}
		}
		ModWall modWall = GetWall(type);
		if (modWall != null)
		{
			if (!modWall.KillSound(i, j, fail))
			{
				return false;
			}
			SoundEngine.PlaySound(modWall.HitSound, (Vector2?)new Vector2((float)(i * 16), (float)(j * 16)));
			return false;
		}
		return true;
	}

	public static void NumDust(int i, int j, int type, bool fail, ref int numDust)
	{
		GetWall(type)?.NumDust(i, j, fail, ref numDust);
		DelegateNumDust[] hookNumDust = HookNumDust;
		for (int k = 0; k < hookNumDust.Length; k++)
		{
			hookNumDust[k](i, j, type, fail, ref numDust);
		}
	}

	public static bool CreateDust(int i, int j, int type, ref int dustType)
	{
		DelegateCreateDust[] hookCreateDust = HookCreateDust;
		for (int k = 0; k < hookCreateDust.Length; k++)
		{
			if (!hookCreateDust[k](i, j, type, ref dustType))
			{
				return false;
			}
		}
		return GetWall(type)?.CreateDust(i, j, ref dustType) ?? true;
	}

	public static bool Drop(int i, int j, int type, ref int dropType)
	{
		DelegateDrop[] hookDrop = HookDrop;
		for (int k = 0; k < hookDrop.Length; k++)
		{
			if (!hookDrop[k](i, j, type, ref dropType))
			{
				return false;
			}
		}
		ModWall modWall = GetWall(type);
		if (modWall != null)
		{
			if (wallTypeToItemType.TryGetValue(type, out var value))
			{
				dropType = value;
			}
			return modWall.Drop(i, j, ref dropType);
		}
		return true;
	}

	public static void KillWall(int i, int j, int type, ref bool fail)
	{
		GetWall(type)?.KillWall(i, j, ref fail);
		DelegateKillWall[] hookKillWall = HookKillWall;
		for (int k = 0; k < hookKillWall.Length; k++)
		{
			hookKillWall[k](i, j, type, ref fail);
		}
	}

	public static bool CanPlace(int i, int j, int type)
	{
		Func<int, int, int, bool>[] hookCanPlace = HookCanPlace;
		for (int k = 0; k < hookCanPlace.Length; k++)
		{
			if (!hookCanPlace[k](i, j, type))
			{
				return false;
			}
		}
		return GetWall(type)?.CanPlace(i, j) ?? true;
	}

	public static bool CanExplode(int i, int j, int type)
	{
		Func<int, int, int, bool>[] hookCanExplode = HookCanExplode;
		for (int k = 0; k < hookCanExplode.Length; k++)
		{
			if (!hookCanExplode[k](i, j, type))
			{
				return false;
			}
		}
		return GetWall(type)?.CanExplode(i, j) ?? true;
	}

	public static void ModifyLight(int i, int j, int type, ref float r, ref float g, ref float b)
	{
		GetWall(type)?.ModifyLight(i, j, ref r, ref g, ref b);
		DelegateModifyLight[] hookModifyLight = HookModifyLight;
		for (int k = 0; k < hookModifyLight.Length; k++)
		{
			hookModifyLight[k](i, j, type, ref r, ref g, ref b);
		}
	}

	public static void RandomUpdate(int i, int j, int type)
	{
		GetWall(type)?.RandomUpdate(i, j);
		Action<int, int, int>[] hookRandomUpdate = HookRandomUpdate;
		for (int k = 0; k < hookRandomUpdate.Length; k++)
		{
			hookRandomUpdate[k](i, j, type);
		}
	}

	public static bool WallFrame(int i, int j, int type, bool randomizeFrame, ref int style, ref int frameNumber)
	{
		ModWall modWall = GetWall(type);
		if (modWall != null && !modWall.WallFrame(i, j, randomizeFrame, ref style, ref frameNumber))
		{
			return false;
		}
		DelegateWallFrame[] hookWallFrame = HookWallFrame;
		for (int k = 0; k < hookWallFrame.Length; k++)
		{
			if (!hookWallFrame[k](i, j, type, randomizeFrame, ref style, ref frameNumber))
			{
				return false;
			}
		}
		return true;
	}

	public static void AnimateWalls()
	{
		if (loaded)
		{
			for (int i = 0; i < walls.Count; i++)
			{
				ModWall modWall = walls[i];
				modWall.AnimateWall(ref Main.wallFrame[modWall.Type], ref Main.wallFrameCounter[modWall.Type]);
			}
		}
	}

	public static bool PreDraw(int i, int j, int type, SpriteBatch spriteBatch)
	{
		Func<int, int, int, SpriteBatch, bool>[] hookPreDraw = HookPreDraw;
		for (int k = 0; k < hookPreDraw.Length; k++)
		{
			if (!hookPreDraw[k](i, j, type, spriteBatch))
			{
				return false;
			}
		}
		return GetWall(type)?.PreDraw(i, j, spriteBatch) ?? true;
	}

	public static void PostDraw(int i, int j, int type, SpriteBatch spriteBatch)
	{
		GetWall(type)?.PostDraw(i, j, spriteBatch);
		Action<int, int, int, SpriteBatch>[] hookPostDraw = HookPostDraw;
		for (int k = 0; k < hookPostDraw.Length; k++)
		{
			hookPostDraw[k](i, j, type, spriteBatch);
		}
	}

	public static void PlaceInWorld(int i, int j, Item item)
	{
		int type = item.createWall;
		if (type >= 0)
		{
			Action<int, int, int, Item>[] hookPlaceInWorld = HookPlaceInWorld;
			for (int k = 0; k < hookPlaceInWorld.Length; k++)
			{
				hookPlaceInWorld[k](i, j, type, item);
			}
			GetWall(type)?.PlaceInWorld(i, j, item);
		}
	}

	internal static void FinishSetup()
	{
		for (int i = 0; i < ItemLoader.ItemCount; i++)
		{
			Item item = ContentSamples.ItemsByType[i];
			if (!ItemID.Sets.DisableAutomaticPlaceableDrop[i] && item.createWall > -1)
			{
				wallTypeToItemType.TryAdd(item.createWall, item.type);
			}
		}
	}
}
