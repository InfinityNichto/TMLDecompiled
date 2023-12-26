using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using ReLogic.Content;

namespace Terraria.ModLoader.Core;

public static class Cloning
{
	private class TypeCloningInfo
	{
		public Type type;

		public bool overridesClone;

		public FieldInfo[] fieldsWhichMightNeedDeepCloning;

		public TypeCloningInfo baseTypeInfo;

		public bool warnCheckDone;

		public bool IsCloneable
		{
			get
			{
				if (!overridesClone)
				{
					if (fieldsWhichMightNeedDeepCloning.Length == 0)
					{
						return baseTypeInfo.IsCloneable;
					}
					return false;
				}
				return true;
			}
		}

		public void Warn()
		{
			if (warnCheckDone)
			{
				return;
			}
			if (!IsCloneable)
			{
				if (fieldsWhichMightNeedDeepCloning.Length == 0)
				{
					baseTypeInfo.Warn();
				}
				else
				{
					IEnumerable<FieldInfo> fields = fieldsWhichMightNeedDeepCloning;
					TypeCloningInfo b = baseTypeInfo;
					while (!b.overridesClone)
					{
						fields = fields.Concat(b.fieldsWhichMightNeedDeepCloning);
						b = b.baseTypeInfo;
					}
					string msg = type.FullName + " has reference fields (" + string.Join(", ", fields.Select((FieldInfo f) => f.Name)) + ") that may not be safe to share between clones." + Environment.NewLine + "For deep-cloning, add a custom Clone override and make proper copies of these fields. If shallow (memberwise) cloning is acceptable, mark the fields with [CloneByReference] or properties with [field: CloneByReference]";
					Logging.tML.Warn((object)msg);
				}
			}
			warnCheckDone = true;
		}
	}

	private static Dictionary<Type, TypeCloningInfo> typeInfos;

	private static ConditionalWeakTable<Type, object> immutableTypes;

	public static bool IsCloneable<T, F>(T t, Expression<Func<T, F>> cloneMethod) where F : Delegate
	{
		Type type = t.GetType();
		if (!typeInfos.TryGetValue(type, out var typeInfo))
		{
			return ComputeInfo(t.GetType(), cloneMethod.ToMethodInfo()).IsCloneable;
		}
		return typeInfo.IsCloneable;
	}

	public static bool IsCloneable(Type type, MethodInfo cloneMethod)
	{
		return GetOrComputeInfo(type, cloneMethod).IsCloneable;
	}

	private static TypeCloningInfo GetOrComputeInfo(Type type, MethodInfo cloneMethod)
	{
		if (!typeInfos.TryGetValue(type, out var typeInfo))
		{
			return ComputeInfo(type, cloneMethod);
		}
		return typeInfo;
	}

	private static TypeCloningInfo ComputeInfo(Type type, MethodInfo cloneMethod)
	{
		TypeCloningInfo info = new TypeCloningInfo
		{
			type = type,
			overridesClone = (LoaderUtils.GetDerivedDefinition(type, cloneMethod).DeclaringType == type),
			fieldsWhichMightNeedDeepCloning = (from f in type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
				where f.DeclaringType == type && !IsCloneByReference(f)
				select f).ToArray()
		};
		if (!info.overridesClone)
		{
			info.baseTypeInfo = GetOrComputeInfo(type.BaseType, cloneMethod);
		}
		typeInfos[type] = info;
		return info;
	}

	private static bool IsCloneByReference(FieldInfo f)
	{
		if (f.GetCustomAttribute<CloneByReference>() == null)
		{
			return IsCloneByReference(f.FieldType);
		}
		return true;
	}

	private static bool IsCloneByReference(Type type)
	{
		if (!type.IsValueType && type.GetCustomAttribute<CloneByReference>() == null)
		{
			return IsImmutable(type);
		}
		return true;
	}

	public static bool IsImmutable(Type type)
	{
		if (type.IsGenericType && !type.IsGenericTypeDefinition && IsImmutable(type.GetGenericTypeDefinition()))
		{
			return true;
		}
		lock (immutableTypes)
		{
			object value;
			return immutableTypes.TryGetValue(type, out value);
		}
	}

	public static void AddImmutableType(Type type)
	{
		lock (immutableTypes)
		{
			immutableTypes.AddOrUpdate(type, null);
		}
	}

	public static void WarnNotCloneable(Type type)
	{
		typeInfos[type].Warn();
	}

	static Cloning()
	{
		typeInfos = new Dictionary<Type, TypeCloningInfo>();
		immutableTypes = new ConditionalWeakTable<Type, object>();
		TypeCaching.OnClear += typeInfos.Clear;
		AddImmutableType(typeof(string));
		AddImmutableType(typeof(Asset<>));
	}
}
