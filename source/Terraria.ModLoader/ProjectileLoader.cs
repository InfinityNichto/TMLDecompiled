using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader.Core;
using Terraria.ModLoader.IO;

namespace Terraria.ModLoader;

/// <summary>
/// This serves as the central class from which projectile-related functions are carried out. It also stores a list of mod projectiles by ID.
/// </summary>
public static class ProjectileLoader
{
	private delegate bool DelegateTileCollideStyle(Projectile projectile, ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac);

	private delegate void DelegateModifyDamageHitbox(Projectile projectile, ref Rectangle hitbox);

	private delegate void DelegateModifyHitNPC(Projectile projectile, NPC target, ref NPC.HitModifiers modifiers);

	private delegate void DelegateModifyHitPlayer(Projectile projectile, Player target, ref Player.HurtModifiers modifiers);

	private delegate bool DelegatePreDraw(Projectile projectile, ref Color lightColor);

	private delegate void DelegateUseGrapple(Player player, ref int type);

	private delegate void DelegateNumGrappleHooks(Projectile projectile, Player player, ref int numHooks);

	private delegate void DelegateGrappleRetreatSpeed(Projectile projectile, Player player, ref float speed);

	private delegate void DelegateGrapplePullSpeed(Projectile projectile, Player player, ref float speed);

	private delegate void DelegateGrappleTargetPoint(Projectile projectile, Player player, ref float grappleX, ref float grappleY);

	private static readonly IList<ModProjectile> projectiles = new List<ModProjectile>();

	private static readonly List<GlobalHookList<GlobalProjectile>> hooks = new List<GlobalHookList<GlobalProjectile>>();

	private static readonly List<GlobalHookList<GlobalProjectile>> modHooks = new List<GlobalHookList<GlobalProjectile>>();

	private static GlobalHookList<GlobalProjectile> HookOnSpawn = AddHook((Expression<Func<GlobalProjectile, Action<Projectile, IEntitySource>>>)((GlobalProjectile g) => g.OnSpawn));

	private static GlobalHookList<GlobalProjectile> HookPreAI = AddHook((Expression<Func<GlobalProjectile, Func<Projectile, bool>>>)((GlobalProjectile g) => g.PreAI));

	private static GlobalHookList<GlobalProjectile> HookAI = AddHook((Expression<Func<GlobalProjectile, Action<Projectile>>>)((GlobalProjectile g) => g.AI));

	private static GlobalHookList<GlobalProjectile> HookPostAI = AddHook((Expression<Func<GlobalProjectile, Action<Projectile>>>)((GlobalProjectile g) => g.PostAI));

	private static GlobalHookList<GlobalProjectile> HookSendExtraAI = AddHook((Expression<Func<GlobalProjectile, Action<Projectile, BitWriter, BinaryWriter>>>)((GlobalProjectile g) => g.SendExtraAI));

	private static GlobalHookList<GlobalProjectile> HookReceiveExtraAI = AddHook((Expression<Func<GlobalProjectile, Action<Projectile, BitReader, BinaryReader>>>)((GlobalProjectile g) => g.ReceiveExtraAI));

	private static GlobalHookList<GlobalProjectile> HookShouldUpdatePosition = AddHook((Expression<Func<GlobalProjectile, Func<Projectile, bool>>>)((GlobalProjectile g) => g.ShouldUpdatePosition));

	private static GlobalHookList<GlobalProjectile> HookTileCollideStyle = AddHook((Expression<Func<GlobalProjectile, DelegateTileCollideStyle>>)((GlobalProjectile g) => g.TileCollideStyle));

	private static GlobalHookList<GlobalProjectile> HookOnTileCollide = AddHook((Expression<Func<GlobalProjectile, Func<Projectile, Vector2, bool>>>)((GlobalProjectile g) => g.OnTileCollide));

	private static GlobalHookList<GlobalProjectile> HookCanCutTiles = AddHook((Expression<Func<GlobalProjectile, Func<Projectile, bool?>>>)((GlobalProjectile g) => g.CanCutTiles));

	private static GlobalHookList<GlobalProjectile> HookCutTiles = AddHook((Expression<Func<GlobalProjectile, Action<Projectile>>>)((GlobalProjectile g) => g.CutTiles));

	private static GlobalHookList<GlobalProjectile> HookPreKill = AddHook((Expression<Func<GlobalProjectile, Func<Projectile, int, bool>>>)((GlobalProjectile g) => g.PreKill));

	[Obsolete]
	private static GlobalHookList<GlobalProjectile> HookKill = AddHook((Expression<Func<GlobalProjectile, Action<Projectile, int>>>)((GlobalProjectile g) => g.Kill));

	private static GlobalHookList<GlobalProjectile> HookOnKill = AddHook((Expression<Func<GlobalProjectile, Action<Projectile, int>>>)((GlobalProjectile g) => g.OnKill));

	private static GlobalHookList<GlobalProjectile> HookCanDamage = AddHook((Expression<Func<GlobalProjectile, Func<Projectile, bool?>>>)((GlobalProjectile g) => g.CanDamage));

	private static GlobalHookList<GlobalProjectile> HookMinionContactDamage = AddHook((Expression<Func<GlobalProjectile, Func<Projectile, bool>>>)((GlobalProjectile g) => g.MinionContactDamage));

	private static GlobalHookList<GlobalProjectile> HookModifyDamageHitbox = AddHook((Expression<Func<GlobalProjectile, DelegateModifyDamageHitbox>>)((GlobalProjectile g) => g.ModifyDamageHitbox));

	private static GlobalHookList<GlobalProjectile> HookCanHitNPC = AddHook((Expression<Func<GlobalProjectile, Func<Projectile, NPC, bool?>>>)((GlobalProjectile g) => g.CanHitNPC));

	private static GlobalHookList<GlobalProjectile> HookModifyHitNPC = AddHook((Expression<Func<GlobalProjectile, DelegateModifyHitNPC>>)((GlobalProjectile g) => g.ModifyHitNPC));

	private static GlobalHookList<GlobalProjectile> HookOnHitNPC = AddHook((Expression<Func<GlobalProjectile, Action<Projectile, NPC, NPC.HitInfo, int>>>)((GlobalProjectile g) => g.OnHitNPC));

	private static GlobalHookList<GlobalProjectile> HookCanHitPvp = AddHook((Expression<Func<GlobalProjectile, Func<Projectile, Player, bool>>>)((GlobalProjectile g) => g.CanHitPvp));

	private static GlobalHookList<GlobalProjectile> HookCanHitPlayer = AddHook((Expression<Func<GlobalProjectile, Func<Projectile, Player, bool>>>)((GlobalProjectile g) => g.CanHitPlayer));

	private static GlobalHookList<GlobalProjectile> HookModifyHitPlayer = AddHook((Expression<Func<GlobalProjectile, DelegateModifyHitPlayer>>)((GlobalProjectile g) => g.ModifyHitPlayer));

	private static GlobalHookList<GlobalProjectile> HookOnHitPlayer = AddHook((Expression<Func<GlobalProjectile, Action<Projectile, Player, Player.HurtInfo>>>)((GlobalProjectile g) => g.OnHitPlayer));

	private static GlobalHookList<GlobalProjectile> HookColliding = AddHook((Expression<Func<GlobalProjectile, Func<Projectile, Rectangle, Rectangle, bool?>>>)((GlobalProjectile g) => g.Colliding));

	private static GlobalHookList<GlobalProjectile> HookGetAlpha = AddHook((Expression<Func<GlobalProjectile, Func<Projectile, Color, Color?>>>)((GlobalProjectile g) => g.GetAlpha));

	private static GlobalHookList<GlobalProjectile> HookPreDrawExtras = AddHook((Expression<Func<GlobalProjectile, Func<Projectile, bool>>>)((GlobalProjectile g) => g.PreDrawExtras));

	private static GlobalHookList<GlobalProjectile> HookPreDraw = AddHook((Expression<Func<GlobalProjectile, DelegatePreDraw>>)((GlobalProjectile g) => g.PreDraw));

	private static GlobalHookList<GlobalProjectile> HookPostDraw = AddHook((Expression<Func<GlobalProjectile, Action<Projectile, Color>>>)((GlobalProjectile g) => g.PostDraw));

	private static GlobalHookList<GlobalProjectile> HookCanUseGrapple = AddHook((Expression<Func<GlobalProjectile, Func<int, Player, bool?>>>)((GlobalProjectile g) => g.CanUseGrapple));

	private static GlobalHookList<GlobalProjectile> HookUseGrapple = AddHook((Expression<Func<GlobalProjectile, DelegateUseGrapple>>)((GlobalProjectile g) => g.UseGrapple));

	private static GlobalHookList<GlobalProjectile> HookNumGrappleHooks = AddHook((Expression<Func<GlobalProjectile, DelegateNumGrappleHooks>>)((GlobalProjectile g) => g.NumGrappleHooks));

	private static GlobalHookList<GlobalProjectile> HookGrappleRetreatSpeed = AddHook((Expression<Func<GlobalProjectile, DelegateGrappleRetreatSpeed>>)((GlobalProjectile g) => g.GrappleRetreatSpeed));

	private static GlobalHookList<GlobalProjectile> HookGrapplePullSpeed = AddHook((Expression<Func<GlobalProjectile, DelegateGrapplePullSpeed>>)((GlobalProjectile g) => g.GrapplePullSpeed));

	private static GlobalHookList<GlobalProjectile> HookGrappleTargetPoint = AddHook((Expression<Func<GlobalProjectile, DelegateGrappleTargetPoint>>)((GlobalProjectile g) => g.GrappleTargetPoint));

	private static GlobalHookList<GlobalProjectile> HookGrappleCanLatchOnTo = AddHook((Expression<Func<GlobalProjectile, Func<Projectile, Player, int, int, bool?>>>)((GlobalProjectile g) => g.GrappleCanLatchOnTo));

	private static GlobalHookList<GlobalProjectile> HookDrawBehind = AddHook((Expression<Func<GlobalProjectile, Action<Projectile, int, List<int>, List<int>, List<int>, List<int>, List<int>>>>)((GlobalProjectile g) => g.DrawBehind));

	public static int ProjectileCount { get; private set; } = ProjectileID.Count;


	private static GlobalHookList<GlobalProjectile> AddHook<F>(Expression<Func<GlobalProjectile, F>> func) where F : Delegate
	{
		GlobalHookList<GlobalProjectile> hook = GlobalHookList<GlobalProjectile>.Create(func);
		hooks.Add(hook);
		return hook;
	}

	public static T AddModHook<T>(T hook) where T : GlobalHookList<GlobalProjectile>
	{
		modHooks.Add(hook);
		return hook;
	}

	internal static int Register(ModProjectile projectile)
	{
		projectiles.Add(projectile);
		return ProjectileCount++;
	}

	/// <summary>
	/// Gets the ModProjectile template instance corresponding to the specified type (not the clone/new instance which gets added to Projectiles as the game is played).
	/// </summary>
	/// <param name="type">The type of the projectile</param>
	/// <returns>The ModProjectile instance in the projectiles array, null if not found.</returns>
	public static ModProjectile GetProjectile(int type)
	{
		if (type < ProjectileID.Count || type >= ProjectileCount)
		{
			return null;
		}
		return projectiles[type - ProjectileID.Count];
	}

	internal static void ResizeArrays(bool unloading)
	{
		if (!unloading)
		{
			GlobalList<GlobalProjectile>.FinishLoading(ProjectileCount);
		}
		Array.Resize(ref TextureAssets.Projectile, ProjectileCount);
		LoaderUtils.ResetStaticMembers(typeof(ProjectileID));
		Array.Resize(ref Main.projHostile, ProjectileCount);
		Array.Resize(ref Main.projHook, ProjectileCount);
		Array.Resize(ref Main.projFrames, ProjectileCount);
		Array.Resize(ref Main.projPet, ProjectileCount);
		Array.Resize(ref Lang._projectileNameCache, ProjectileCount);
		for (int j = ProjectileID.Count; j < ProjectileCount; j++)
		{
			Main.projFrames[j] = 1;
			Lang._projectileNameCache[j] = LocalizedText.Empty;
		}
		Array.Resize(ref Projectile.perIDStaticNPCImmunity, ProjectileCount);
		for (int i = 0; i < ProjectileCount; i++)
		{
			Projectile.perIDStaticNPCImmunity[i] = new uint[200];
		}
	}

	internal static void FinishSetup()
	{
		GlobalLoaderUtils<GlobalProjectile, Projectile>.BuildTypeLookups(new Projectile().SetDefaults);
		UpdateHookLists();
		GlobalTypeLookups<GlobalProjectile>.LogStats();
		foreach (ModProjectile proj in projectiles)
		{
			Lang._projectileNameCache[proj.Type] = proj.DisplayName;
		}
	}

	private static void UpdateHookLists()
	{
		foreach (GlobalHookList<GlobalProjectile> item in hooks.Union(modHooks))
		{
			item.Update();
		}
	}

	internal static void Unload()
	{
		ProjectileCount = ProjectileID.Count;
		projectiles.Clear();
		GlobalList<GlobalProjectile>.Reset();
		modHooks.Clear();
		UpdateHookLists();
	}

	internal static bool IsModProjectile(Projectile projectile)
	{
		return projectile.type >= ProjectileID.Count;
	}

	internal static void SetDefaults(Projectile projectile, bool createModProjectile = true)
	{
		if (IsModProjectile(projectile) && createModProjectile)
		{
			projectile.ModProjectile = GetProjectile(projectile.type).NewInstance(projectile);
		}
		GlobalLoaderUtils<GlobalProjectile, Projectile>.SetDefaults(projectile, ref projectile._globals, delegate(Projectile e)
		{
			e.ModProjectile?.SetDefaults();
		});
	}

	internal static void OnSpawn(Projectile projectile, IEntitySource source)
	{
		projectile.ModProjectile?.OnSpawn(source);
		EntityGlobalsEnumerator<GlobalProjectile> enumerator = HookOnSpawn.Enumerate(projectile).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.OnSpawn(projectile, source);
		}
	}

	public static void ProjectileAI(Projectile projectile)
	{
		if (PreAI(projectile))
		{
			int type = projectile.type;
			int num;
			if (projectile.ModProjectile != null)
			{
				num = ((projectile.ModProjectile.AIType > 0) ? 1 : 0);
				if (num != 0)
				{
					projectile.type = projectile.ModProjectile.AIType;
				}
			}
			else
			{
				num = 0;
			}
			projectile.VanillaAI();
			if (num != 0)
			{
				projectile.type = type;
			}
			AI(projectile);
		}
		PostAI(projectile);
	}

	public static bool PreAI(Projectile projectile)
	{
		bool result = true;
		EntityGlobalsEnumerator<GlobalProjectile> enumerator = HookPreAI.Enumerate(projectile).GetEnumerator();
		while (enumerator.MoveNext())
		{
			GlobalProjectile g = enumerator.Current;
			result &= g.PreAI(projectile);
		}
		if (result && projectile.ModProjectile != null)
		{
			return projectile.ModProjectile.PreAI();
		}
		return result;
	}

	public static void AI(Projectile projectile)
	{
		projectile.ModProjectile?.AI();
		EntityGlobalsEnumerator<GlobalProjectile> enumerator = HookAI.Enumerate(projectile).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.AI(projectile);
		}
	}

	public static void PostAI(Projectile projectile)
	{
		projectile.ModProjectile?.PostAI();
		EntityGlobalsEnumerator<GlobalProjectile> enumerator = HookPostAI.Enumerate(projectile).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.PostAI(projectile);
		}
	}

	public static void SendExtraAI(BinaryWriter writer, byte[] extraAI)
	{
		writer.Write7BitEncodedInt(extraAI.Length);
		if (extraAI.Length != 0)
		{
			writer.Write(extraAI);
		}
	}

	public static byte[] WriteExtraAI(Projectile projectile)
	{
		using MemoryStream stream = new MemoryStream();
		using BinaryWriter modWriter = new BinaryWriter(stream);
		projectile.ModProjectile?.SendExtraAI(modWriter);
		using MemoryStream bufferedStream = new MemoryStream();
		using BinaryWriter globalWriter = new BinaryWriter(bufferedStream);
		BitWriter bitWriter = new BitWriter();
		EntityGlobalsEnumerator<GlobalProjectile> enumerator = HookSendExtraAI.Enumerate(projectile).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.SendExtraAI(projectile, bitWriter, globalWriter);
		}
		bitWriter.Flush(modWriter);
		modWriter.Write(bufferedStream.ToArray());
		return stream.ToArray();
	}

	public static byte[] ReadExtraAI(BinaryReader reader)
	{
		return reader.ReadBytes(reader.Read7BitEncodedInt());
	}

	public static void ReceiveExtraAI(Projectile projectile, byte[] extraAI)
	{
		using MemoryStream stream = extraAI.ToMemoryStream();
		using BinaryReader modReader = new BinaryReader(stream);
		projectile.ModProjectile?.ReceiveExtraAI(modReader);
		BitReader bitReader = new BitReader(modReader);
		bool anyGlobals = false;
		EntityGlobalsEnumerator<GlobalProjectile> entityGlobalsEnumerator;
		try
		{
			entityGlobalsEnumerator = HookReceiveExtraAI.Enumerate(projectile);
			EntityGlobalsEnumerator<GlobalProjectile> enumerator = entityGlobalsEnumerator.GetEnumerator();
			while (enumerator.MoveNext())
			{
				enumerator.Current.ReceiveExtraAI(projectile, bitReader, modReader);
			}
			if (bitReader.BitsRead < bitReader.MaxBits)
			{
				throw new IOException($"Read underflow {bitReader.MaxBits - bitReader.BitsRead} of {bitReader.MaxBits} compressed bits in ReceiveExtraAI, more info below");
			}
			if (stream.Position < stream.Length)
			{
				throw new IOException($"Read underflow {stream.Length - stream.Position} of {stream.Length} bytes in ReceiveExtraAI, more info below");
			}
		}
		catch (IOException)
		{
			string message = "Error in ReceiveExtraAI for Projectile " + (projectile.ModProjectile?.FullName ?? projectile.Name);
			if (anyGlobals)
			{
				message += ", may be caused by one of these GlobalNPCs:";
				entityGlobalsEnumerator = HookReceiveExtraAI.Enumerate(projectile);
				EntityGlobalsEnumerator<GlobalProjectile> enumerator = entityGlobalsEnumerator.GetEnumerator();
				while (enumerator.MoveNext())
				{
					GlobalProjectile g = enumerator.Current;
					message = message + "\n\t" + g.FullName;
				}
			}
		}
	}

	public static bool ShouldUpdatePosition(Projectile projectile)
	{
		if (IsModProjectile(projectile) && !projectile.ModProjectile.ShouldUpdatePosition())
		{
			return false;
		}
		EntityGlobalsEnumerator<GlobalProjectile> enumerator = HookShouldUpdatePosition.Enumerate(projectile).GetEnumerator();
		while (enumerator.MoveNext())
		{
			if (!enumerator.Current.ShouldUpdatePosition(projectile))
			{
				return false;
			}
		}
		return true;
	}

	public static bool TileCollideStyle(Projectile projectile, ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
	{
		if (IsModProjectile(projectile) && !projectile.ModProjectile.TileCollideStyle(ref width, ref height, ref fallThrough, ref hitboxCenterFrac))
		{
			return false;
		}
		EntityGlobalsEnumerator<GlobalProjectile> enumerator = HookTileCollideStyle.Enumerate(projectile).GetEnumerator();
		while (enumerator.MoveNext())
		{
			if (!enumerator.Current.TileCollideStyle(projectile, ref width, ref height, ref fallThrough, ref hitboxCenterFrac))
			{
				return false;
			}
		}
		return true;
	}

	public static bool OnTileCollide(Projectile projectile, Vector2 oldVelocity)
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		bool result = true;
		EntityGlobalsEnumerator<GlobalProjectile> enumerator = HookOnTileCollide.Enumerate(projectile).GetEnumerator();
		while (enumerator.MoveNext())
		{
			GlobalProjectile g = enumerator.Current;
			result &= g.OnTileCollide(projectile, oldVelocity);
		}
		if (result && projectile.ModProjectile != null)
		{
			return projectile.ModProjectile.OnTileCollide(oldVelocity);
		}
		return result;
	}

	public static bool? CanCutTiles(Projectile projectile)
	{
		EntityGlobalsEnumerator<GlobalProjectile> enumerator = HookCanCutTiles.Enumerate(projectile).GetEnumerator();
		while (enumerator.MoveNext())
		{
			bool? canCutTiles = enumerator.Current.CanCutTiles(projectile);
			if (canCutTiles.HasValue)
			{
				return canCutTiles.Value;
			}
		}
		return projectile.ModProjectile?.CanCutTiles();
	}

	public static void CutTiles(Projectile projectile)
	{
		EntityGlobalsEnumerator<GlobalProjectile> enumerator = HookCutTiles.Enumerate(projectile).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.CutTiles(projectile);
		}
		projectile.ModProjectile?.CutTiles();
	}

	public static bool PreKill(Projectile projectile, int timeLeft)
	{
		bool result = true;
		EntityGlobalsEnumerator<GlobalProjectile> enumerator = HookPreKill.Enumerate(projectile).GetEnumerator();
		while (enumerator.MoveNext())
		{
			GlobalProjectile g = enumerator.Current;
			result &= g.PreKill(projectile, timeLeft);
		}
		if (result && projectile.ModProjectile != null)
		{
			return projectile.ModProjectile.PreKill(timeLeft);
		}
		return result;
	}

	[Obsolete("Renamed to OnKill")]
	public static void Kill_Obsolete(Projectile projectile, int timeLeft)
	{
		projectile.ModProjectile?.Kill(timeLeft);
		EntityGlobalsEnumerator<GlobalProjectile> enumerator = HookKill.Enumerate(projectile).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.Kill(projectile, timeLeft);
		}
	}

	public static void OnKill(Projectile projectile, int timeLeft)
	{
		projectile.ModProjectile?.OnKill(timeLeft);
		Kill_Obsolete(projectile, timeLeft);
		EntityGlobalsEnumerator<GlobalProjectile> enumerator = HookOnKill.Enumerate(projectile).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.OnKill(projectile, timeLeft);
		}
	}

	public static bool? CanDamage(Projectile projectile)
	{
		bool? result = null;
		EntityGlobalsEnumerator<GlobalProjectile> enumerator = HookCanDamage.Enumerate(projectile).GetEnumerator();
		while (enumerator.MoveNext())
		{
			bool? canDamage = enumerator.Current.CanDamage(projectile);
			if (canDamage.HasValue)
			{
				if (!canDamage.Value)
				{
					return false;
				}
				result = true;
			}
		}
		return result ?? projectile.ModProjectile?.CanDamage();
	}

	public static bool MinionContactDamage(Projectile projectile)
	{
		if (projectile.ModProjectile != null && projectile.ModProjectile.MinionContactDamage())
		{
			return true;
		}
		EntityGlobalsEnumerator<GlobalProjectile> enumerator = HookMinionContactDamage.Enumerate(projectile).GetEnumerator();
		while (enumerator.MoveNext())
		{
			if (enumerator.Current.MinionContactDamage(projectile))
			{
				return true;
			}
		}
		return false;
	}

	public static void ModifyDamageHitbox(Projectile projectile, ref Rectangle hitbox)
	{
		projectile.ModProjectile?.ModifyDamageHitbox(ref hitbox);
		EntityGlobalsEnumerator<GlobalProjectile> enumerator = HookModifyDamageHitbox.Enumerate(projectile).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.ModifyDamageHitbox(projectile, ref hitbox);
		}
	}

	public static bool? CanHitNPC(Projectile projectile, NPC target)
	{
		bool? flag = null;
		EntityGlobalsEnumerator<GlobalProjectile> enumerator = HookCanHitNPC.Enumerate(projectile).GetEnumerator();
		while (enumerator.MoveNext())
		{
			bool? canHit2 = enumerator.Current.CanHitNPC(projectile, target);
			if (canHit2.HasValue && !canHit2.Value)
			{
				return false;
			}
			if (canHit2.HasValue)
			{
				flag = canHit2.Value;
			}
		}
		if (projectile.ModProjectile != null)
		{
			bool? canHit = projectile.ModProjectile.CanHitNPC(target);
			if (canHit.HasValue && !canHit.Value)
			{
				return false;
			}
			if (canHit.HasValue)
			{
				flag = canHit.Value;
			}
		}
		return flag;
	}

	public static void ModifyHitNPC(Projectile projectile, NPC target, ref NPC.HitModifiers modifiers)
	{
		projectile.ModProjectile?.ModifyHitNPC(target, ref modifiers);
		EntityGlobalsEnumerator<GlobalProjectile> enumerator = HookModifyHitNPC.Enumerate(projectile).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.ModifyHitNPC(projectile, target, ref modifiers);
		}
	}

	public static void OnHitNPC(Projectile projectile, NPC target, in NPC.HitInfo hit, int damageDone)
	{
		projectile.ModProjectile?.OnHitNPC(target, hit, damageDone);
		EntityGlobalsEnumerator<GlobalProjectile> enumerator = HookOnHitNPC.Enumerate(projectile).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.OnHitNPC(projectile, target, hit, damageDone);
		}
	}

	public static bool CanHitPvp(Projectile projectile, Player target)
	{
		EntityGlobalsEnumerator<GlobalProjectile> enumerator = HookCanHitPvp.Enumerate(projectile).GetEnumerator();
		while (enumerator.MoveNext())
		{
			if (!enumerator.Current.CanHitPvp(projectile, target))
			{
				return false;
			}
		}
		if (projectile.ModProjectile != null)
		{
			return projectile.ModProjectile.CanHitPvp(target);
		}
		return true;
	}

	public static bool CanHitPlayer(Projectile projectile, Player target)
	{
		EntityGlobalsEnumerator<GlobalProjectile> enumerator = HookCanHitPlayer.Enumerate(projectile).GetEnumerator();
		while (enumerator.MoveNext())
		{
			if (!enumerator.Current.CanHitPlayer(projectile, target))
			{
				return false;
			}
		}
		if (projectile.ModProjectile != null)
		{
			return projectile.ModProjectile.CanHitPlayer(target);
		}
		return true;
	}

	public static void ModifyHitPlayer(Projectile projectile, Player target, ref Player.HurtModifiers modifiers)
	{
		projectile.ModProjectile?.ModifyHitPlayer(target, ref modifiers);
		EntityGlobalsEnumerator<GlobalProjectile> enumerator = HookModifyHitPlayer.Enumerate(projectile).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.ModifyHitPlayer(projectile, target, ref modifiers);
		}
	}

	public static void OnHitPlayer(Projectile projectile, Player target, in Player.HurtInfo hurtInfo)
	{
		projectile.ModProjectile?.OnHitPlayer(target, hurtInfo);
		EntityGlobalsEnumerator<GlobalProjectile> enumerator = HookOnHitPlayer.Enumerate(projectile).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.OnHitPlayer(projectile, target, hurtInfo);
		}
	}

	public static bool? Colliding(Projectile projectile, Rectangle projHitbox, Rectangle targetHitbox)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		EntityGlobalsEnumerator<GlobalProjectile> enumerator = HookColliding.Enumerate(projectile).GetEnumerator();
		while (enumerator.MoveNext())
		{
			bool? colliding = enumerator.Current.Colliding(projectile, projHitbox, targetHitbox);
			if (colliding.HasValue)
			{
				return colliding.Value;
			}
		}
		return projectile.ModProjectile?.Colliding(projHitbox, targetHitbox);
	}

	public static void DrawHeldProjInFrontOfHeldItemAndArms(Projectile projectile, ref bool flag)
	{
		if (projectile.ModProjectile != null)
		{
			flag = projectile.ModProjectile.DrawHeldProjInFrontOfHeldItemAndArms;
		}
	}

	public static void ModifyFishingLine(Projectile projectile, ref float polePosX, ref float polePosY, ref Color lineColor)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		if (projectile.ModProjectile != null)
		{
			Vector2 lineOriginOffset = Vector2.Zero;
			Player player = Main.player[projectile.owner];
			projectile.ModProjectile?.ModifyFishingLine(ref lineOriginOffset, ref lineColor);
			polePosX += lineOriginOffset.X * (float)player.direction;
			if (player.direction < 0)
			{
				polePosX -= 13f;
			}
			polePosY += lineOriginOffset.Y * player.gravDir;
		}
	}

	public static Color? GetAlpha(Projectile projectile, Color lightColor)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		EntityGlobalsEnumerator<GlobalProjectile> enumerator = HookGetAlpha.Enumerate(projectile).GetEnumerator();
		while (enumerator.MoveNext())
		{
			Color? color = enumerator.Current.GetAlpha(projectile, lightColor);
			if (color.HasValue)
			{
				return color;
			}
		}
		return projectile.ModProjectile?.GetAlpha(lightColor);
	}

	public static void DrawOffset(Projectile projectile, ref int offsetX, ref int offsetY, ref float originX)
	{
		if (projectile.ModProjectile != null)
		{
			offsetX = projectile.ModProjectile.DrawOffsetX;
			offsetY = -projectile.ModProjectile.DrawOriginOffsetY;
			originX += projectile.ModProjectile.DrawOriginOffsetX;
		}
	}

	public static bool PreDrawExtras(Projectile projectile)
	{
		bool result = true;
		EntityGlobalsEnumerator<GlobalProjectile> enumerator = HookPreDrawExtras.Enumerate(projectile).GetEnumerator();
		while (enumerator.MoveNext())
		{
			GlobalProjectile g = enumerator.Current;
			result &= g.PreDrawExtras(projectile);
		}
		if (result && projectile.ModProjectile != null)
		{
			return projectile.ModProjectile.PreDrawExtras();
		}
		return result;
	}

	public static bool PreDraw(Projectile projectile, ref Color lightColor)
	{
		bool result = true;
		EntityGlobalsEnumerator<GlobalProjectile> enumerator = HookPreDraw.Enumerate(projectile).GetEnumerator();
		while (enumerator.MoveNext())
		{
			GlobalProjectile g = enumerator.Current;
			result &= g.PreDraw(projectile, ref lightColor);
		}
		if (result && projectile.ModProjectile != null)
		{
			return projectile.ModProjectile.PreDraw(ref lightColor);
		}
		return result;
	}

	public static void PostDraw(Projectile projectile, Color lightColor)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		projectile.ModProjectile?.PostDraw(lightColor);
		EntityGlobalsEnumerator<GlobalProjectile> enumerator = HookPostDraw.Enumerate(projectile).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.PostDraw(projectile, lightColor);
		}
	}

	public static bool? CanUseGrapple(int type, Player player)
	{
		bool? flag = GetProjectile(type)?.CanUseGrapple(player);
		ReadOnlySpan<GlobalProjectile> readOnlySpan = HookCanUseGrapple.Enumerate(type);
		for (int i = 0; i < readOnlySpan.Length; i++)
		{
			bool? canGrapple = readOnlySpan[i].CanUseGrapple(type, player);
			if (canGrapple.HasValue)
			{
				flag = canGrapple;
			}
		}
		return flag;
	}

	public static void UseGrapple(Player player, ref int type)
	{
		GetProjectile(type)?.UseGrapple(player, ref type);
		ReadOnlySpan<GlobalProjectile> readOnlySpan = HookUseGrapple.Enumerate();
		for (int i = 0; i < readOnlySpan.Length; i++)
		{
			readOnlySpan[i].UseGrapple(player, ref type);
		}
	}

	public static bool GrappleOutOfRange(float distance, Projectile projectile)
	{
		return distance > projectile.ModProjectile?.GrappleRange();
	}

	public static void NumGrappleHooks(Projectile projectile, Player player, ref int numHooks)
	{
		projectile.ModProjectile?.NumGrappleHooks(player, ref numHooks);
		EntityGlobalsEnumerator<GlobalProjectile> enumerator = HookNumGrappleHooks.Enumerate(projectile).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.NumGrappleHooks(projectile, player, ref numHooks);
		}
	}

	public static void GrappleRetreatSpeed(Projectile projectile, Player player, ref float speed)
	{
		projectile.ModProjectile?.GrappleRetreatSpeed(player, ref speed);
		EntityGlobalsEnumerator<GlobalProjectile> enumerator = HookGrappleRetreatSpeed.Enumerate(projectile).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.GrappleRetreatSpeed(projectile, player, ref speed);
		}
	}

	public static void GrapplePullSpeed(Projectile projectile, Player player, ref float speed)
	{
		projectile.ModProjectile?.GrapplePullSpeed(player, ref speed);
		EntityGlobalsEnumerator<GlobalProjectile> enumerator = HookGrapplePullSpeed.Enumerate(projectile).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.GrapplePullSpeed(projectile, player, ref speed);
		}
	}

	public static void GrappleTargetPoint(Projectile projectile, Player player, ref float grappleX, ref float grappleY)
	{
		projectile.ModProjectile?.GrappleTargetPoint(player, ref grappleX, ref grappleY);
		EntityGlobalsEnumerator<GlobalProjectile> enumerator = HookGrappleTargetPoint.Enumerate(projectile).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.GrappleTargetPoint(projectile, player, ref grappleX, ref grappleY);
		}
	}

	public static bool? GrappleCanLatchOnTo(Projectile projectile, Player player, int x, int y)
	{
		bool? flag = projectile.ModProjectile?.GrappleCanLatchOnTo(player, x, y);
		EntityGlobalsEnumerator<GlobalProjectile> enumerator = HookGrappleCanLatchOnTo.Enumerate(projectile).GetEnumerator();
		while (enumerator.MoveNext())
		{
			bool? globalFlag = enumerator.Current.GrappleCanLatchOnTo(projectile, player, x, y);
			if (globalFlag.HasValue)
			{
				if (!globalFlag.Value)
				{
					return false;
				}
				flag = globalFlag;
			}
		}
		return flag;
	}

	internal static void DrawBehind(Projectile projectile, int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
	{
		projectile.ModProjectile?.DrawBehind(index, behindNPCsAndTiles, behindNPCs, behindProjectiles, overPlayers, overWiresUI);
		EntityGlobalsEnumerator<GlobalProjectile> enumerator = HookDrawBehind.Enumerate(projectile).GetEnumerator();
		while (enumerator.MoveNext())
		{
			enumerator.Current.DrawBehind(projectile, index, behindNPCsAndTiles, behindNPCs, behindProjectiles, overPlayers, overWiresUI);
		}
	}
}
