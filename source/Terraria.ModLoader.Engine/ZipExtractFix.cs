using System;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using Ionic.Zip;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using MonoMod.RuntimeDetour;
using MonoMod.Utils;

namespace Terraria.ModLoader.Engine;

internal static class ZipExtractFix
{
	[Serializable]
	[CompilerGenerated]
	private sealed class _003C_003Ec
	{
		public static readonly _003C_003Ec _003C_003E9 = new _003C_003Ec();

		public static Func<Instruction, bool> _003C_003E9__1_1;

		public static Func<Instruction, bool> _003C_003E9__1_2;

		public static Manipulator _003C_003E9__1_0;

		internal void _003CInit_003Eb__1_0(ILContext il)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Expected O, but got Unknown
			ILCursor c = new ILCursor(il);
			c.Next = null;
			c.GotoPrev((MoveType)0, new Func<Instruction, bool>[2]
			{
				(Instruction i) => ILPatternMatchingExt.MatchLdstr(i, "\\"),
				(Instruction i) => ILPatternMatchingExt.MatchCallvirt<string>(i, "Replace")
			});
			c.Next.Operand = Path.DirectorySeparatorChar.ToString();
		}

		internal bool _003CInit_003Eb__1_1(Instruction i)
		{
			return ILPatternMatchingExt.MatchLdstr(i, "\\");
		}

		internal bool _003CInit_003Eb__1_2(Instruction i)
		{
			return ILPatternMatchingExt.MatchCallvirt<string>(i, "Replace");
		}
	}

	private static ILHook hook;

	/// <summary>
	/// When Ionic.Zip extracts an entry it uses \\ for all separators when it should use Path.DirectorySeparatorChar for platform compatibility
	/// </summary>
	public static void Init()
	{
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Expected O, but got Unknown
		if (Path.DirectorySeparatorChar == '\\')
		{
			return;
		}
		MethodInfo methodInfo = Extensions.FindMethod(typeof(ZipEntry), "ValidateOutput", true);
		object obj = _003C_003Ec._003C_003E9__1_0;
		if (obj == null)
		{
			Manipulator val = delegate(ILContext il)
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0007: Expected O, but got Unknown
				ILCursor val2 = new ILCursor(il);
				val2.Next = null;
				val2.GotoPrev((MoveType)0, new Func<Instruction, bool>[2]
				{
					(Instruction i) => ILPatternMatchingExt.MatchLdstr(i, "\\"),
					(Instruction i) => ILPatternMatchingExt.MatchCallvirt<string>(i, "Replace")
				});
				val2.Next.Operand = Path.DirectorySeparatorChar.ToString();
			};
			_003C_003Ec._003C_003E9__1_0 = val;
			obj = (object)val;
		}
		hook = new ILHook((MethodBase)methodInfo, (Manipulator)obj);
	}
}
