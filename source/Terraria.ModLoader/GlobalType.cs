using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using Terraria.ModLoader.Core;

namespace Terraria.ModLoader;

public abstract class GlobalType<TGlobal> : ModType where TGlobal : GlobalType<TGlobal>
{
	/// <summary>
	/// Index of this global in the list of all globals of the same type, in registration order
	/// </summary>
	public short StaticIndex { get; internal set; }

	/// <summary>
	/// Index of this global in a <see cref="P:Terraria.ModLoader.IEntityWithGlobals`1.EntityGlobals" /> array <br />
	/// -1 if this global does not have a <see cref="P:Terraria.ModLoader.GlobalType`1.SlotPerEntity" />
	/// </summary>
	public short PerEntityIndex { get; internal set; }

	/// <summary>
	/// If true, the global will be assigned a <see cref="P:Terraria.ModLoader.GlobalType`1.PerEntityIndex" /> at load time, which can be used to access the instance in the <see cref="P:Terraria.ModLoader.IEntityWithGlobals`1.EntityGlobals" /> array. <br />
	/// If false, the global will be a singleton applying to all entities
	/// </summary>
	public virtual bool SlotPerEntity => InstancePerEntity;

	/// <summary>
	/// Whether to create a new instance of this Global for every entity that exists.
	/// Useful for storing information on an entity. Defaults to false.
	/// Return true if you need to store information (have non-static fields).
	/// </summary>
	public virtual bool InstancePerEntity => false;

	/// <summary>
	/// Whether this global applies to some entities but not others
	/// </summary>
	public abstract bool ConditionallyAppliesToEntities { get; }

	protected override void ValidateType()
	{
		base.ValidateType();
		if (GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Any((FieldInfo f) => f.DeclaringType.IsSubclassOf(typeof(GlobalType<TGlobal>))) && !InstancePerEntity)
		{
			throw new Exception($" {GetType().FullName} instance fields but {"InstancePerEntity"} returns false. Either use static fields, or override {"InstancePerEntity"} to return true");
		}
	}

	protected override void Register()
	{
		ModTypeLookup<TGlobal>.Register((TGlobal)this);
		(short, short) tuple = GlobalList<TGlobal>.Register((TGlobal)this);
		(StaticIndex, PerEntityIndex) = tuple;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static TResult GetGlobal<TResult>(int entityType, ReadOnlySpan<TGlobal> entityGlobals, TResult baseInstance) where TResult : TGlobal
	{
		if (!GlobalType<TGlobal>.TryGetGlobal<TResult>(entityType, entityGlobals, baseInstance, out TResult result))
		{
			throw new KeyNotFoundException(baseInstance.FullName);
		}
		return result;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static TResult GetGlobal<TResult>(int entityType, ReadOnlySpan<TGlobal> entityGlobals) where TResult : TGlobal
	{
		if (!GlobalType<TGlobal>.TryGetGlobal<TResult>(entityType, entityGlobals, out TResult result))
		{
			throw new KeyNotFoundException(typeof(TResult).FullName);
		}
		return result;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool TryGetGlobal<TResult>(int entityType, ReadOnlySpan<TGlobal> entityGlobals, TResult baseInstance, out TResult result) where TResult : TGlobal
	{
		short slot = baseInstance.PerEntityIndex;
		if (entityType > 0 && slot >= 0)
		{
			result = (TResult)entityGlobals[slot];
			return result != null;
		}
		if (GlobalTypeLookups<TGlobal>.AppliesToType((TGlobal)(GlobalType<TGlobal>)baseInstance, entityType))
		{
			result = baseInstance;
			return true;
		}
		result = null;
		return false;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool TryGetGlobal<TResult>(int entityType, ReadOnlySpan<TGlobal> entityGlobals, out TResult result) where TResult : TGlobal
	{
		return GlobalType<TGlobal>.TryGetGlobal<TResult>(entityType, entityGlobals, ModContent.GetInstance<TResult>(), out result);
	}
}
public abstract class GlobalType<TEntity, TGlobal> : GlobalType<TGlobal> where TEntity : IEntityWithGlobals<TGlobal> where TGlobal : GlobalType<TEntity, TGlobal>
{
	private bool? _isCloneable;

	private bool? _conditionallyAppliesToEntities;

	/// <summary>
	/// Whether or not this type is cloneable. Cloning is supported if<br />
	/// all reference typed fields in each sub-class which doesn't override Clone are marked with [CloneByReference]
	/// </summary>
	public virtual bool IsCloneable
	{
		get
		{
			bool valueOrDefault = _isCloneable.GetValueOrDefault();
			if (!_isCloneable.HasValue)
			{
				valueOrDefault = Cloning.IsCloneable(this, (Expression<Func<GlobalType<TEntity, TGlobal>, Func<TEntity, TEntity, TGlobal>>>)((GlobalType<TEntity, TGlobal> m) => m.Clone));
				_isCloneable = valueOrDefault;
				return valueOrDefault;
			}
			return valueOrDefault;
		}
	}

	/// <summary>
	/// Whether to create new instances of this mod type via <see cref="M:Terraria.ModLoader.GlobalType`2.Clone(`0,`0)" /> or via the default constructor
	/// Defaults to false (default constructor).
	/// </summary>
	protected virtual bool CloneNewInstances => false;

	/// <summary>
	/// Whether this global applies to some entities but not others. <br />
	/// True if the type overrides <see cref="M:Terraria.ModLoader.GlobalType`2.AppliesToEntity(`0,System.Boolean)" />
	/// </summary>
	public sealed override bool ConditionallyAppliesToEntities
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			bool valueOrDefault = _conditionallyAppliesToEntities.GetValueOrDefault();
			if (!_conditionallyAppliesToEntities.HasValue)
			{
				valueOrDefault = LoaderUtils.HasOverride(this, (Expression<Func<GlobalType<TEntity, TGlobal>, Func<TEntity, bool, bool>>>)((GlobalType<TEntity, TGlobal> m) => m.AppliesToEntity));
				_conditionallyAppliesToEntities = valueOrDefault;
				return valueOrDefault;
			}
			return valueOrDefault;
		}
	}

	/// <summary>
	/// Use this to control whether or not this global should be run on the provided entity instance. <br />
	/// </summary>
	/// <param name="entity"> The entity for which the global instantion is being checked. </param>
	/// <param name="lateInstantiation">
	/// Whether this check occurs before or after the ModX.SetDefaults call.
	/// <br /> If you're relying on entity values that can be changed by that call, you should likely prefix your return value with the following:
	/// <code> lateInstantiation &amp;&amp; ... </code>
	/// </param>
	public virtual bool AppliesToEntity(TEntity entity, bool lateInstantiation)
	{
		return true;
	}

	/// <summary>
	/// Allows you to set the properties of any and every instance that gets created.
	/// </summary>
	public virtual void SetDefaults(TEntity entity)
	{
	}

	/// <summary>
	/// Create a copy of this instanced global. Called when an entity is cloned.
	/// </summary>
	/// <param name="from">The entity being cloned. May be null if <see cref="P:Terraria.ModLoader.GlobalType`2.CloneNewInstances" /> is true (via call from <see cref="M:Terraria.ModLoader.GlobalType`2.NewInstance(`0)" />)</param>
	/// <param name="to">The new clone of the entity</param>
	/// <returns>A clone of this global</returns>
	public virtual TGlobal Clone(TEntity? from, TEntity to)
	{
		if (!IsCloneable)
		{
			Cloning.WarnNotCloneable(GetType());
		}
		return (TGlobal)MemberwiseClone();
	}

	/// <summary>
	/// Only called if <see cref="P:Terraria.ModLoader.GlobalType`1.InstancePerEntity" /> and <see cref="M:Terraria.ModLoader.GlobalType`2.AppliesToEntity(`0,System.Boolean)" />(<paramref name="target" />, ...) are both true. <br />
	/// <br />
	/// Returning null is permitted but <b>not recommended</b> over <c>AppliesToEntity</c> for performance reasons. <br />
	/// Only return null when the global is disabled based on some runtime property (eg world seed).
	/// </summary>
	/// <param name="target">The entity instance the global is being instantiated for</param>
	/// <returns></returns>
	public virtual TGlobal? NewInstance(TEntity target)
	{
		if (CloneNewInstances)
		{
			return Clone(default(TEntity), target);
		}
		TGlobal obj = (TGlobal)Activator.CreateInstance(GetType(), nonPublic: true);
		obj.Mod = base.Mod;
		obj.StaticIndex = base.StaticIndex;
		obj.PerEntityIndex = base.PerEntityIndex;
		obj._isCloneable = _isCloneable;
		obj._conditionallyAppliesToEntities = _conditionallyAppliesToEntities;
		return obj;
	}

	public TGlobal Instance(TEntity entity)
	{
		GlobalType<TGlobal>.TryGetGlobal(entity.Type, entity.EntityGlobals, (TGlobal)this, out var result);
		return result;
	}
}
