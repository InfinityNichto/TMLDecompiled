using System;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using Terraria.ModLoader.Core;

namespace Terraria.ModLoader;

/// <summary>
/// The base type for most modded things.
/// </summary>
public abstract class ModType : IModType, ILoadable
{
	/// <summary>
	///  The mod this belongs to.
	///  </summary>
	public Mod Mod { get; internal set; }

	/// <summary>
	/// The internal name of this.
	/// </summary>
	public virtual string Name => GetType().Name;

	/// <summary>
	/// The internal name of this, including the mod it is from.
	/// </summary>
	public string FullName => (Mod?.Name ?? "Terraria") + "/" + Name;

	void ILoadable.Load(Mod mod)
	{
		if (!LoaderUtils.IsValidated(GetType()))
		{
			ValidateType();
		}
		Mod = mod;
		InitTemplateInstance();
		Load();
		Register();
	}

	/// <summary>
	/// Allows you to perform one-time loading tasks. Beware that mod content has not finished loading here, things like ModContent lookup tables or ID Sets are not fully populated.
	/// <para>Use <see cref="M:Terraria.ModLoader.ModType.SetStaticDefaults" /> when you need to access content.</para>
	/// </summary>
	public virtual void Load()
	{
	}

	/// <summary>
	/// Allows you to stop <see cref="M:Terraria.ModLoader.Mod.AddContent(Terraria.ModLoader.ILoadable)" /> from actually adding this content. Useful for items that can be disabled by a config.
	/// </summary>
	/// <param name="mod">The mod adding this content</param>
	public virtual bool IsLoadingEnabled(Mod mod)
	{
		return true;
	}

	/// <summary>
	/// If you make a new ModType, seal this override.
	/// </summary>
	protected abstract void Register();

	/// <summary>
	/// If you make a new ModType, seal this override, and call <see cref="M:Terraria.ModLoader.ModType.SetStaticDefaults" /> in it.
	/// </summary>
	public virtual void SetupContent()
	{
	}

	/// <summary>
	/// Allows you to modify the properties after initial loading has completed.
	/// </summary>
	public virtual void SetStaticDefaults()
	{
	}

	/// <summary>
	/// Allows you to safely unload things you added in <see cref="M:Terraria.ModLoader.ModType.Load" />.
	/// </summary>
	public virtual void Unload()
	{
	}

	/// <summary>
	/// Create dummy objects for instanced mod-types
	/// </summary>
	protected virtual void InitTemplateInstance()
	{
	}

	/// <summary>
	/// Check for the correct overrides of different hook methods and fields and properties
	/// </summary>
	protected virtual void ValidateType()
	{
	}

	public string PrettyPrintName()
	{
		return Regex.Replace(Name, "([A-Z])", " $1").Trim();
	}
}
public abstract class ModType<TEntity> : ModType
{
	public TEntity Entity { get; internal set; }

	protected override void InitTemplateInstance()
	{
		Entity = CreateTemplateEntity();
	}

	protected abstract TEntity CreateTemplateEntity();
}
public abstract class ModType<TEntity, TModType> : ModType<TEntity> where TModType : ModType<TEntity, TModType>
{
	private bool? _isCloneable;

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
				valueOrDefault = Cloning.IsCloneable(this, (Expression<Func<ModType<TEntity, TModType>, Func<TEntity, TModType>>>)((ModType<TEntity, TModType> m) => m.Clone));
				_isCloneable = valueOrDefault;
				return valueOrDefault;
			}
			return valueOrDefault;
		}
	}

	/// <summary>
	/// Whether to create new instances of this mod type via <see cref="M:Terraria.ModLoader.ModType`2.Clone(`0)" /> or via the default constructor
	/// Defaults to false (default constructor).
	/// </summary>
	protected virtual bool CloneNewInstances => false;

	/// <summary>
	/// Create a copy of this instanced global. Called when an entity is cloned.
	/// </summary>
	/// <param name="newEntity">The new clone of the entity</param>
	/// <returns>A clone of this mod type</returns>
	public virtual TModType Clone(TEntity newEntity)
	{
		if (!IsCloneable)
		{
			Cloning.WarnNotCloneable(GetType());
		}
		TModType obj = (TModType)MemberwiseClone();
		obj.Entity = newEntity;
		return obj;
	}

	/// <summary>
	/// Create a new instance of this ModType for a specific entity
	/// </summary>
	/// <param name="entity">The entity instance the mod type is being instantiated for</param>
	/// <returns></returns>
	public virtual TModType NewInstance(TEntity entity)
	{
		if (CloneNewInstances)
		{
			return Clone(entity);
		}
		TModType obj = (TModType)Activator.CreateInstance(GetType(), nonPublic: true);
		obj.Mod = base.Mod;
		obj.Entity = entity;
		return obj;
	}
}
