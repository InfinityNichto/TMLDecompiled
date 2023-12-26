using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.ExceptionServices;
using Terraria.ModLoader.Exceptions;

namespace Terraria.ModLoader.Core;

public static class LoaderUtils
{
	private static readonly HashSet<Type> validatedTypes;

	/// <summary> Calls static constructors on the provided type and, optionally, its nested types. </summary>
	public static void ResetStaticMembers(Type type, bool recursive = true)
	{
		ConstructorInfo typeInitializer = type.TypeInitializer;
		if (typeInitializer != null)
		{
			FieldInfo? field = typeInitializer.GetType().GetField("m_invocationFlags", BindingFlags.Instance | BindingFlags.NonPublic);
			object previousValue = field.GetValue(typeInitializer);
			field.SetValue(typeInitializer, 1u);
			typeInitializer.Invoke(null, null);
			field.SetValue(typeInitializer, previousValue);
		}
		if (recursive)
		{
			Type[] nestedTypes = type.GetNestedTypes();
			for (int i = 0; i < nestedTypes.Length; i++)
			{
				ResetStaticMembers(nestedTypes[i], recursive);
			}
		}
	}

	public static void ForEachAndAggregateExceptions<T>(IEnumerable<T> enumerable, Action<T> action)
	{
		List<Exception> exceptions = new List<Exception>();
		foreach (T t in enumerable)
		{
			try
			{
				action(t);
			}
			catch (Exception ex)
			{
				ex.Data["contentType"] = t.GetType();
				exceptions.Add(ex);
			}
		}
		if (exceptions.Count == 1)
		{
			ExceptionDispatchInfo.Capture(exceptions[0]).Throw();
		}
		if (exceptions.Count > 0)
		{
			throw new MultipleException(exceptions);
		}
	}

	public static bool HasMethod(Type type, Type declaringType, string method, params Type[] args)
	{
		MethodInfo methodInfo = type.GetMethod(method, args);
		if (methodInfo == null)
		{
			return false;
		}
		return methodInfo.DeclaringType != declaringType;
	}

	public static MethodInfo ToMethodInfo<T, F>(this Expression<Func<T, F>> expr) where F : Delegate
	{
		try
		{
			MethodInfo method = (((expr.Body as UnaryExpression)?.Operand as MethodCallExpression)?.Object as ConstantExpression)?.Value as MethodInfo;
			if (method == null)
			{
				throw new NullReferenceException();
			}
			return method;
		}
		catch (Exception e)
		{
			throw new ArgumentException("Invalid hook expression " + expr, e);
		}
	}

	public static MethodInfo GetDerivedDefinition(Type t, MethodInfo baseMethod)
	{
		MethodInfo baseMethod2 = baseMethod;
		return t.GetMethods().Single((MethodInfo m) => m.GetBaseDefinition() == baseMethod2);
	}

	public static bool HasOverride(Type t, MethodInfo baseMethod)
	{
		if (!baseMethod.DeclaringType.IsInterface)
		{
			return GetDerivedDefinition(t, baseMethod).DeclaringType != baseMethod.DeclaringType;
		}
		return t.IsAssignableTo(baseMethod.DeclaringType);
	}

	public static bool HasOverride<T, F>(T t, Expression<Func<T, F>> expr) where F : Delegate
	{
		return HasOverride(t.GetType(), expr.ToMethodInfo());
	}

	public static IEnumerable<T> WhereMethodIsOverridden<T>(this IEnumerable<T> providers, MethodInfo method)
	{
		MethodInfo method2 = method;
		if (!method2.IsVirtual)
		{
			throw new ArgumentException("Non-virtual method: " + method2);
		}
		return providers.Where((T p) => HasOverride(p.GetType(), method2));
	}

	public static IEnumerable<T> WhereMethodIsOverridden<T, F>(this IEnumerable<T> providers, Expression<Func<T, F>> expr) where F : Delegate
	{
		return providers.WhereMethodIsOverridden(expr.ToMethodInfo());
	}

	public static void MustOverrideTogether<T>(T t, params Expression<Func<T, Delegate>>[] methods)
	{
		MustOverrideTogether(t.GetType(), methods.Select((Expression<Func<T, Delegate>> m) => m.ToMethodInfo()).ToArray());
	}

	private static void MustOverrideTogether(Type type, params MethodInfo[] methods)
	{
		Type type2 = type;
		int c = methods.Count((MethodInfo m) => HasOverride(type2, m));
		if (c > 0 && c < methods.Length)
		{
			throw new Exception($"{type2} must override all of ({string.Join('/', methods.Select((MethodInfo m) => m.Name))}) or none");
		}
	}

	internal static bool IsValidated(Type type)
	{
		return !validatedTypes.Add(type);
	}

	static LoaderUtils()
	{
		validatedTypes = new HashSet<Type>();
		TypeCaching.OnClear += validatedTypes.Clear;
	}
}
