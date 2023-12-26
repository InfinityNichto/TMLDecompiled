using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Microsoft.Xna.Framework.Input;
using ReLogic.Content;
using ReLogic.Graphics;
using ReLogic.OS;
using ReLogic.Utilities;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.ModLoader.UI;
using Terraria.UI;
using Terraria.UI.Chat;
using Terraria.Utilities;
using Terraria.Utilities.Terraria.Utilities;

namespace Terraria;

public static class Utils
{
	public delegate bool TileActionAttempt(int x, int y);

	public delegate void LaserLineFraming(int stage, Vector2 currentPosition, float distanceLeft, Rectangle lastFrame, out float distanceCovered, out Rectangle frame, out Vector2 origin, out Color color);

	public delegate Color ColorLerpMethod(float percent);

	public struct ChaseResults
	{
		public bool InterceptionHappens;

		public Vector2 InterceptionPosition;

		public float InterceptionTime;

		public Vector2 ChaserVelocity;
	}

	public const long MaxCoins = 999999999L;

	public static Dictionary<DynamicSpriteFont, float[]> charLengths = new Dictionary<DynamicSpriteFont, float[]>();

	private static Regex _substitutionRegex = new Regex("{(\\?(?:!)?)?([a-zA-Z][\\w\\.]*)}", RegexOptions.Compiled);

	private const ulong RANDOM_MULTIPLIER = 25214903917uL;

	private const ulong RANDOM_ADD = 11uL;

	private const ulong RANDOM_MASK = 281474976710655uL;

	public static Color ColorLerp_BlackToWhite(float percent)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		return Color.Lerp(Color.Black, Color.White, percent);
	}

	public static double Lerp(double value1, double value2, double amount)
	{
		return value1 + (value2 - value1) * amount;
	}

	public static Vector2 Round(Vector2 input)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		return new Vector2((float)Math.Round(input.X), (float)Math.Round(input.Y));
	}

	public static bool IsPowerOfTwo(int x)
	{
		if (x != 0)
		{
			return (x & (x - 1)) == 0;
		}
		return false;
	}

	public static float SmoothStep(float min, float max, float x)
	{
		return MathHelper.Clamp((x - min) / (max - min), 0f, 1f);
	}

	public static double SmoothStep(double min, double max, double x)
	{
		return Clamp((x - min) / (max - min), 0.0, 1.0);
	}

	public static float UnclampedSmoothStep(float min, float max, float x)
	{
		return (x - min) / (max - min);
	}

	public static double UnclampedSmoothStep(double min, double max, double x)
	{
		return (x - min) / (max - min);
	}

	public static Dictionary<string, string> ParseArguements(string[] args)
	{
		string text = null;
		string text2 = "";
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		for (int i = 0; i < args.Length; i++)
		{
			if (args[i].Length == 0)
			{
				continue;
			}
			if (args[i][0] == '-' || args[i][0] == '+')
			{
				if (text != null)
				{
					dictionary.Add(text.ToLower(), text2);
					text2 = "";
				}
				text = args[i];
				text2 = "";
			}
			else
			{
				if (text2 != "")
				{
					text2 += " ";
				}
				text2 += args[i];
			}
		}
		if (text != null)
		{
			dictionary.Add(text.ToLower(), text2);
			text2 = "";
		}
		return dictionary;
	}

	public static void Swap<T>(ref T t1, ref T t2)
	{
		T val = t1;
		t1 = t2;
		t2 = val;
	}

	public static T Clamp<T>(T value, T min, T max) where T : IComparable<T>
	{
		if (value.CompareTo(max) > 0)
		{
			return max;
		}
		if (value.CompareTo(min) < 0)
		{
			return min;
		}
		return value;
	}

	public static float Turn01ToCyclic010(float value)
	{
		return 1f - ((float)Math.Cos(value * ((float)Math.PI * 2f)) * 0.5f + 0.5f);
	}

	public static float PingPongFrom01To010(float value)
	{
		value %= 1f;
		if (value < 0f)
		{
			value += 1f;
		}
		if (value >= 0.5f)
		{
			return 2f - value * 2f;
		}
		return value * 2f;
	}

	public static float MultiLerp(float percent, params float[] floats)
	{
		float num = 1f / ((float)floats.Length - 1f);
		float num2 = num;
		int num3 = 0;
		while (percent / num2 > 1f && num3 < floats.Length - 2)
		{
			num2 += num;
			num3++;
		}
		return MathHelper.Lerp(floats[num3], floats[num3 + 1], (percent - num * (float)num3) / num);
	}

	public static float WrappedLerp(float value1, float value2, float percent)
	{
		float num = percent * 2f;
		if (num > 1f)
		{
			num = 2f - num;
		}
		return MathHelper.Lerp(value1, value2, num);
	}

	public static float GetLerpValue(float from, float to, float t, bool clamped = false)
	{
		if (clamped)
		{
			if (from < to)
			{
				if (t < from)
				{
					return 0f;
				}
				if (t > to)
				{
					return 1f;
				}
			}
			else
			{
				if (t < to)
				{
					return 1f;
				}
				if (t > from)
				{
					return 0f;
				}
			}
		}
		return (t - from) / (to - from);
	}

	public static float Remap(float fromValue, float fromMin, float fromMax, float toMin, float toMax, bool clamped = true)
	{
		return MathHelper.Lerp(toMin, toMax, GetLerpValue(fromMin, fromMax, fromValue, clamped));
	}

	public static void ClampWithinWorld(ref int minX, ref int minY, ref int maxX, ref int maxY, bool lastValuesInclusiveToIteration = false, int fluffX = 0, int fluffY = 0)
	{
		int num = (lastValuesInclusiveToIteration ? 1 : 0);
		minX = Clamp(minX, fluffX, Main.maxTilesX - num - fluffX);
		maxX = Clamp(maxX, fluffX, Main.maxTilesX - num - fluffX);
		minY = Clamp(minY, fluffY, Main.maxTilesY - num - fluffY);
		maxY = Clamp(maxY, fluffY, Main.maxTilesY - num - fluffY);
	}

	public static ChaseResults GetChaseResults(Vector2 chaserPosition, float chaserSpeed, Vector2 runnerPosition, Vector2 runnerVelocity)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		ChaseResults result = default(ChaseResults);
		if (chaserPosition == runnerPosition)
		{
			ChaseResults result2 = default(ChaseResults);
			result2.InterceptionHappens = true;
			result2.InterceptionPosition = chaserPosition;
			result2.InterceptionTime = 0f;
			result2.ChaserVelocity = Vector2.Zero;
			return result2;
		}
		if (chaserSpeed <= 0f)
		{
			return default(ChaseResults);
		}
		Vector2 value = chaserPosition - runnerPosition;
		float num = ((Vector2)(ref value)).Length();
		float num2 = ((Vector2)(ref runnerVelocity)).Length();
		if (num2 == 0f)
		{
			result.InterceptionTime = num / chaserSpeed;
			result.InterceptionPosition = runnerPosition;
		}
		else
		{
			float a = chaserSpeed * chaserSpeed - num2 * num2;
			float b = 2f * Vector2.Dot(value, runnerVelocity);
			float c = (0f - num) * num;
			if (!SolveQuadratic(a, b, c, out var result3, out var result4))
			{
				return default(ChaseResults);
			}
			if (result3 < 0f && result4 < 0f)
			{
				return default(ChaseResults);
			}
			if (result3 > 0f && result4 > 0f)
			{
				result.InterceptionTime = Math.Min(result3, result4);
			}
			else
			{
				result.InterceptionTime = Math.Max(result3, result4);
			}
			result.InterceptionPosition = runnerPosition + runnerVelocity * result.InterceptionTime;
		}
		result.ChaserVelocity = (result.InterceptionPosition - chaserPosition) / result.InterceptionTime;
		result.InterceptionHappens = true;
		return result;
	}

	public static Vector2 FactorAcceleration(Vector2 currentVelocity, float timeToInterception, Vector2 descendOfProjectile, int framesOfLenience)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		float num = Math.Max(0f, timeToInterception - (float)framesOfLenience);
		Vector2 vector = descendOfProjectile * (num * num) / 2f / timeToInterception;
		return currentVelocity - vector;
	}

	public static bool SolveQuadratic(float a, float b, float c, out float result1, out float result2)
	{
		float num = b * b - 4f * a * c;
		result1 = 0f;
		result2 = 0f;
		if (num > 0f)
		{
			result1 = (0f - b + (float)Math.Sqrt(num)) / (2f * a);
			result2 = (0f - b - (float)Math.Sqrt(num)) / (2f * a);
			return true;
		}
		if (num < 0f)
		{
			return false;
		}
		result1 = (result2 = (0f - b + (float)Math.Sqrt(num)) / (2f * a));
		return true;
	}

	public static double GetLerpValue(double from, double to, double t, bool clamped = false)
	{
		if (clamped)
		{
			if (from < to)
			{
				if (t < from)
				{
					return 0.0;
				}
				if (t > to)
				{
					return 1.0;
				}
			}
			else
			{
				if (t < to)
				{
					return 1.0;
				}
				if (t > from)
				{
					return 0.0;
				}
			}
		}
		return (t - from) / (to - from);
	}

	public static float GetDayTimeAs24FloatStartingFromMidnight()
	{
		if (Main.dayTime)
		{
			return 4.5f + (float)(Main.time / 54000.0) * 15f;
		}
		return 19.5f + (float)(Main.time / 32400.0) * 9f;
	}

	public static Vector2 GetDayTimeAsDirectionIn24HClock()
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		return GetDayTimeAsDirectionIn24HClock(GetDayTimeAs24FloatStartingFromMidnight());
	}

	public static Vector2 GetDayTimeAsDirectionIn24HClock(float timeFrom0To24)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		return RotatedBy(new Vector2(0f, -1f), timeFrom0To24 / 24f * ((float)Math.PI * 2f));
	}

	public static string[] ConvertMonoArgsToDotNet(string[] brokenArgs)
	{
		ArrayList arrayList = new ArrayList();
		string text = "";
		for (int i = 0; i < brokenArgs.Length; i++)
		{
			if (brokenArgs[i].StartsWith("-") || brokenArgs[i].StartsWith("+"))
			{
				if (text != "")
				{
					arrayList.Add(text);
					text = "";
				}
				else
				{
					arrayList.Add("");
				}
				arrayList.Add(brokenArgs[i]);
			}
			else
			{
				if (text != "")
				{
					text += " ";
				}
				text += brokenArgs[i];
			}
		}
		arrayList.Add(text);
		string[] array = new string[arrayList.Count];
		arrayList.CopyTo(array);
		return array;
	}

	public static T Max<T>(params T[] args) where T : IComparable
	{
		T result = args[0];
		for (int i = 1; i < args.Length; i++)
		{
			if (result.CompareTo(args[i]) < 0)
			{
				result = args[i];
			}
		}
		return result;
	}

	public static float LineRectangleDistance(Rectangle rect, Vector2 lineStart, Vector2 lineEnd)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		Vector2 vector = rect.TopLeft();
		Vector2 vector2 = rect.TopRight();
		Vector2 vector3 = rect.BottomLeft();
		Vector2 vector4 = rect.BottomRight();
		if (lineStart.Between(vector, vector4) || lineEnd.Between(vector, vector4))
		{
			return 0f;
		}
		float num = vector.Distance(vector.ClosestPointOnLine(lineStart, lineEnd));
		float value2 = vector2.Distance(vector2.ClosestPointOnLine(lineStart, lineEnd));
		float value3 = vector3.Distance(vector3.ClosestPointOnLine(lineStart, lineEnd));
		float value4 = vector4.Distance(vector4.ClosestPointOnLine(lineStart, lineEnd));
		return MathHelper.Min(num, MathHelper.Min(value2, MathHelper.Min(value3, value4)));
	}

	public static List<List<TextSnippet>> WordwrapStringSmart(string text, Color c, DynamicSpriteFont font, int maxWidth, int maxLines)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		TextSnippet[] array4 = ChatManager.ParseMessage(text, c).ToArray();
		List<List<TextSnippet>> list = new List<List<TextSnippet>>();
		List<TextSnippet> list2 = new List<TextSnippet>();
		TextSnippet[] array5 = array4;
		foreach (TextSnippet textSnippet in array5)
		{
			string[] array2 = textSnippet.Text.Split('\n');
			for (int j = 0; j < array2.Length - 1; j++)
			{
				list2.Add(textSnippet.CopyMorph(array2[j]));
				list.Add(list2);
				list2 = new List<TextSnippet>();
			}
			list2.Add(textSnippet.CopyMorph(array2[^1]));
		}
		list.Add(list2);
		if (maxWidth != -1)
		{
			for (int k = 0; k < list.Count; k++)
			{
				List<TextSnippet> list3 = list[k];
				float num = 0f;
				for (int l = 0; l < list3.Count; l++)
				{
					float stringLength = list3[l].GetStringLength(font);
					if (stringLength + num > (float)maxWidth)
					{
						int num2 = maxWidth - (int)num;
						if (num > 0f)
						{
							num2 -= 16;
						}
						int num3 = Math.Min(list3[l].Text.Length, num2 / 8);
						for (int i = 0; i < list3[l].Text.Length; i++)
						{
							if (font.MeasureString(list3[l].Text.Substring(0, i)).X * list3[l].Scale < (float)num2)
							{
								num3 = i;
							}
						}
						if (num3 < 0)
						{
							num3 = 0;
						}
						string[] array3 = list3[l].Text.Split(' ');
						int num4 = num3;
						if (array3.Length > 1)
						{
							num4 = 0;
							for (int m = 0; m < array3.Length; m++)
							{
								bool flag = num4 == 0;
								if (!(num4 + array3[m].Length <= num3 || flag))
								{
									break;
								}
								num4 += array3[m].Length + 1;
							}
							if (num4 > num3)
							{
								num4 = num3;
							}
						}
						string newText = list3[l].Text.Substring(0, num4);
						string newText2 = list3[l].Text.Substring(num4);
						list2 = new List<TextSnippet> { list3[l].CopyMorph(newText2) };
						for (int n = l + 1; n < list3.Count; n++)
						{
							list2.Add(list3[n]);
						}
						list3[l] = list3[l].CopyMorph(newText);
						list[k] = list[k].Take(l + 1).ToList();
						list.Insert(k + 1, list2);
						break;
					}
					num += stringLength;
				}
				num = 0f;
			}
		}
		if (maxLines != -1)
		{
			while (list.Count > maxLines)
			{
				list.RemoveAt(maxLines);
			}
		}
		return list;
	}

	public static string[] WordwrapString(string text, DynamicSpriteFont font, int maxWidth, int maxLines, out int lineAmount)
	{
		//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		string[] array = new string[maxLines];
		int num = 0;
		List<string> list = new List<string>(text.Split('\n'));
		List<string> list2 = new List<string>(list[0].Split(' '));
		for (int i = 1; i < list.Count && i < maxLines; i++)
		{
			list2.Add("\n");
			list2.AddRange(list[i].Split(' '));
		}
		bool flag = true;
		while (list2.Count > 0)
		{
			string text2 = list2[0];
			string text3 = " ";
			if (list2.Count == 1)
			{
				text3 = "";
			}
			if (text2 == "\n")
			{
				array[num++] += text2;
				flag = true;
				if (num >= maxLines)
				{
					break;
				}
				list2.RemoveAt(0);
			}
			else if (flag)
			{
				if (font.MeasureString(text2).X > (float)maxWidth)
				{
					string text4 = text2[0].ToString() ?? "";
					int num2 = 1;
					while (font.MeasureString(text4 + text2[num2] + "-").X <= (float)maxWidth)
					{
						text4 += text2[num2++];
					}
					text4 += "-";
					array[num++] = text4 + " ";
					if (num >= maxLines)
					{
						break;
					}
					list2.RemoveAt(0);
					list2.Insert(0, text2.Substring(num2));
				}
				else
				{
					ref string reference = ref array[num];
					reference = reference + text2 + text3;
					flag = false;
					list2.RemoveAt(0);
				}
			}
			else if (font.MeasureString(array[num] + text2).X > (float)maxWidth)
			{
				num++;
				if (num >= maxLines)
				{
					break;
				}
				flag = true;
			}
			else
			{
				ref string reference2 = ref array[num];
				reference2 = reference2 + text2 + text3;
				flag = false;
				list2.RemoveAt(0);
			}
		}
		lineAmount = num;
		if (lineAmount == maxLines)
		{
			lineAmount--;
		}
		return array;
	}

	public static Rectangle CenteredRectangle(Vector2 center, Vector2 size)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		return new Rectangle((int)(center.X - size.X / 2f), (int)(center.Y - size.Y / 2f), (int)size.X, (int)size.Y);
	}

	public static Vector2 Vector2FromElipse(Vector2 angleVector, Vector2 elipseSizes)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		if (elipseSizes == Vector2.Zero)
		{
			return Vector2.Zero;
		}
		if (angleVector == Vector2.Zero)
		{
			return Vector2.Zero;
		}
		((Vector2)(ref angleVector)).Normalize();
		Vector2 vector = Vector2.Normalize(elipseSizes);
		vector = Vector2.One / vector;
		angleVector *= vector;
		((Vector2)(ref angleVector)).Normalize();
		return angleVector * elipseSizes / 2f;
	}

	public static bool FloatIntersect(float r1StartX, float r1StartY, float r1Width, float r1Height, float r2StartX, float r2StartY, float r2Width, float r2Height)
	{
		if (r1StartX > r2StartX + r2Width || r1StartY > r2StartY + r2Height || r1StartX + r1Width < r2StartX || r1StartY + r1Height < r2StartY)
		{
			return false;
		}
		return true;
	}

	public static long CoinsCount(out bool overFlowing, Item[] inv, params int[] ignoreSlots)
	{
		List<int> list = new List<int>(ignoreSlots);
		long num = 0L;
		for (int i = 0; i < inv.Length; i++)
		{
			if (!list.Contains(i))
			{
				switch (inv[i].type)
				{
				case 71:
					num += inv[i].stack;
					break;
				case 72:
					num += (long)inv[i].stack * 100L;
					break;
				case 73:
					num += (long)inv[i].stack * 10000L;
					break;
				case 74:
					num += (long)inv[i].stack * 1000000L;
					break;
				}
			}
		}
		overFlowing = false;
		return num;
	}

	public static int[] CoinsSplit(long count)
	{
		int[] array = new int[4];
		long num = 0L;
		long num2 = 1000000L;
		for (int num3 = 3; num3 >= 0; num3--)
		{
			array[num3] = (int)((count - num) / num2);
			num += array[num3] * num2;
			num2 /= 100;
		}
		return array;
	}

	public static long CoinsCombineStacks(out bool overFlowing, params long[] coinCounts)
	{
		long num = 0L;
		foreach (long num2 in coinCounts)
		{
			num += num2;
			if (num >= 999999999)
			{
				overFlowing = true;
				return 999999999L;
			}
		}
		overFlowing = false;
		return num;
	}

	public static void PoofOfSmoke(Vector2 position)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		int num = Main.rand.Next(3, 7);
		for (int i = 0; i < num; i++)
		{
			int num2 = Gore.NewGore(position, (Main.rand.NextFloat() * ((float)Math.PI * 2f)).ToRotationVector2() * new Vector2(2f, 0.7f) * 0.7f, Main.rand.Next(11, 14));
			Main.gore[num2].scale = 0.7f;
			Gore obj = Main.gore[num2];
			obj.velocity *= 0.5f;
		}
		for (int j = 0; j < 10; j++)
		{
			Dust obj2 = Main.dust[Dust.NewDust(position, 14, 14, 16, 0f, 0f, 100, default(Color), 1.5f)];
			obj2.position += new Vector2(5f);
			obj2.velocity = (Main.rand.NextFloat() * ((float)Math.PI * 2f)).ToRotationVector2() * new Vector2(2f, 0.7f) * 0.7f * (0.5f + 0.5f * Main.rand.NextFloat());
		}
	}

	public static Vector2 ToScreenPosition(this Vector2 worldPosition)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		return Vector2.Transform(worldPosition - Main.screenPosition, Main.GameViewMatrix.ZoomMatrix) / Main.UIScale;
	}

	public static string PrettifyPercentDisplay(float percent, string originalFormat)
	{
		return percent.ToString(originalFormat, CultureInfo.InvariantCulture).TrimEnd('0', '%', ' ').TrimEnd('.', ' ')
			.TrimStart('0', ' ') + "%";
	}

	public static void TrimTextIfNeeded(ref string text, DynamicSpriteFont font, float scale, float maxWidth)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		int num = 0;
		Vector2 vector = font.MeasureString(text) * scale;
		while (vector.X > maxWidth)
		{
			text = text.Substring(0, text.Length - 1);
			num++;
			vector = font.MeasureString(text) * scale;
		}
		if (num > 0)
		{
			text = text.Substring(0, text.Length - 1) + "…";
		}
	}

	public static string FormatWith(string original, object obj)
	{
		PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(obj);
		return _substitutionRegex.Replace(original, delegate(Match match)
		{
			if (match.Groups[1].Length != 0)
			{
				return "";
			}
			string name = match.Groups[2].ToString();
			PropertyDescriptor propertyDescriptor = properties.Find(name, ignoreCase: false);
			return (propertyDescriptor != null) ? (propertyDescriptor.GetValue(obj) ?? "").ToString() : "";
		});
	}

	public static bool TryCreatingDirectory(string folderPath)
	{
		if (Directory.Exists(folderPath))
		{
			return true;
		}
		try
		{
			Directory.CreateDirectory(folderPath);
			return true;
		}
		catch (Exception exception)
		{
			FancyErrorPrinter.ShowDirectoryCreationFailError(exception, folderPath);
			return false;
		}
	}

	private static void OpenFolderXdg(string folderPath)
	{
		ProcessStartInfo processStartInfo = new ProcessStartInfo();
		processStartInfo.FileName = "xdg-open";
		processStartInfo.Arguments = folderPath;
		processStartInfo.UseShellExecute = true;
		processStartInfo.CreateNoWindow = true;
		processStartInfo.EnvironmentVariables["LD_LIBRARY_PATH"] = "/usr/lib:/lib";
		Process.Start(processStartInfo);
	}

	public static void OpenFolder(string folderPath)
	{
		if (TryCreatingDirectory(folderPath))
		{
			if (Platform.IsLinux)
			{
				OpenFolderXdg(folderPath);
				return;
			}
			Process.Start(new ProcessStartInfo(folderPath)
			{
				UseShellExecute = true
			});
		}
	}

	public static byte[] ToByteArray(this string str)
	{
		byte[] array = new byte[str.Length * 2];
		Buffer.BlockCopy(str.ToCharArray(), 0, array, 0, array.Length);
		return array;
	}

	/// <summary>
	/// Generates a random value between 0f (inclusive) and 1f (exclusive). <br />It will not return 1f.
	/// </summary>
	/// <param name="r"></param>
	/// <returns></returns>
	public static float NextFloat(this UnifiedRandom r)
	{
		return (float)r.NextDouble();
	}

	public static float NextFloatDirection(this UnifiedRandom r)
	{
		return (float)r.NextDouble() * 2f - 1f;
	}

	public static float NextFloat(this UnifiedRandom random, FloatRange range)
	{
		return random.NextFloat() * (range.Maximum - range.Minimum) + range.Minimum;
	}

	public static T NextFromList<T>(this UnifiedRandom random, params T[] objs)
	{
		return objs[random.Next(objs.Length)];
	}

	public static T NextFromCollection<T>(this UnifiedRandom random, List<T> objs)
	{
		return objs[random.Next(objs.Count)];
	}

	public static int Next(this UnifiedRandom random, IntRange range)
	{
		return random.Next(range.Minimum, range.Maximum + 1);
	}

	public static Vector2 NextVector2Square(this UnifiedRandom r, float min, float max)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		return new Vector2((max - min) * (float)r.NextDouble() + min, (max - min) * (float)r.NextDouble() + min);
	}

	public static Vector2 NextVector2FromRectangle(this UnifiedRandom r, Rectangle rect)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		return new Vector2((float)rect.X + r.NextFloat() * (float)rect.Width, (float)rect.Y + r.NextFloat() * (float)rect.Height);
	}

	public static Vector2 NextVector2Unit(this UnifiedRandom r, float startRotation = 0f, float rotationRange = (float)Math.PI * 2f)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		return (startRotation + rotationRange * r.NextFloat()).ToRotationVector2();
	}

	public static Vector2 NextVector2Circular(this UnifiedRandom r, float circleHalfWidth, float circleHalfHeight)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		return r.NextVector2Unit() * new Vector2(circleHalfWidth, circleHalfHeight) * r.NextFloat();
	}

	public static Vector2 NextVector2CircularEdge(this UnifiedRandom r, float circleHalfWidth, float circleHalfHeight)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		return r.NextVector2Unit() * new Vector2(circleHalfWidth, circleHalfHeight);
	}

	public static int Width(this Asset<Texture2D> asset)
	{
		if (!asset.IsLoaded)
		{
			return 0;
		}
		return asset.Value.Width;
	}

	public static int Height(this Asset<Texture2D> asset)
	{
		if (!asset.IsLoaded)
		{
			return 0;
		}
		return asset.Value.Height;
	}

	public static Rectangle Frame(this Asset<Texture2D> tex, int horizontalFrames = 1, int verticalFrames = 1, int frameX = 0, int frameY = 0, int sizeOffsetX = 0, int sizeOffsetY = 0)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		if (!tex.IsLoaded)
		{
			return Rectangle.Empty;
		}
		return tex.Value.Frame(horizontalFrames, verticalFrames, frameX, frameY, sizeOffsetX, sizeOffsetY);
	}

	public static Rectangle OffsetSize(this Rectangle rect, int xSize, int ySize)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		rect.Width += xSize;
		rect.Height += ySize;
		return rect;
	}

	public static Vector2 Size(this Asset<Texture2D> tex)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		if (!tex.IsLoaded)
		{
			return Vector2.Zero;
		}
		return tex.Value.Size();
	}

	public static Rectangle Frame(this Texture2D tex, int horizontalFrames = 1, int verticalFrames = 1, int frameX = 0, int frameY = 0, int sizeOffsetX = 0, int sizeOffsetY = 0)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		int num = tex.Width / horizontalFrames;
		int num2 = tex.Height / verticalFrames;
		return new Rectangle(num * frameX, num2 * frameY, num + sizeOffsetX, num2 + sizeOffsetY);
	}

	public static Vector2 OriginFlip(this Rectangle rect, Vector2 origin, SpriteEffects effects)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		if (((Enum)effects).HasFlag((Enum)(object)(SpriteEffects)1))
		{
			origin.X = (float)rect.Width - origin.X;
		}
		if (((Enum)effects).HasFlag((Enum)(object)(SpriteEffects)2))
		{
			origin.Y = (float)rect.Height - origin.Y;
		}
		return origin;
	}

	public static Vector2 Size(this Texture2D tex)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		return new Vector2((float)tex.Width, (float)tex.Height);
	}

	public static void WriteRGB(this BinaryWriter bb, Color c)
	{
		bb.Write(((Color)(ref c)).R);
		bb.Write(((Color)(ref c)).G);
		bb.Write(((Color)(ref c)).B);
	}

	public static void WriteVector2(this BinaryWriter bb, Vector2 v)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		bb.Write(v.X);
		bb.Write(v.Y);
	}

	public static void WritePackedVector2(this BinaryWriter bb, Vector2 v)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		HalfVector2 val = new HalfVector2(v.X, v.Y);
		bb.Write(((HalfVector2)(ref val)).PackedValue);
	}

	public static Color ReadRGB(this BinaryReader bb)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		return new Color((int)bb.ReadByte(), (int)bb.ReadByte(), (int)bb.ReadByte());
	}

	public static Vector2 ReadVector2(this BinaryReader bb)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		return new Vector2(bb.ReadSingle(), bb.ReadSingle());
	}

	public static Vector2 ReadPackedVector2(this BinaryReader bb)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		HalfVector2 halfVector = default(HalfVector2);
		((HalfVector2)(ref halfVector)).PackedValue = bb.ReadUInt32();
		return ((HalfVector2)(ref halfVector)).ToVector2();
	}

	public static Vector2 Left(this Rectangle r)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		return new Vector2((float)r.X, (float)(r.Y + r.Height / 2));
	}

	public static Vector2 Right(this Rectangle r)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		return new Vector2((float)(r.X + r.Width), (float)(r.Y + r.Height / 2));
	}

	public static Vector2 Top(this Rectangle r)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		return new Vector2((float)(r.X + r.Width / 2), (float)r.Y);
	}

	public static Vector2 Bottom(this Rectangle r)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		return new Vector2((float)(r.X + r.Width / 2), (float)(r.Y + r.Height));
	}

	public static Vector2 TopLeft(this Rectangle r)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		return new Vector2((float)r.X, (float)r.Y);
	}

	public static Vector2 TopRight(this Rectangle r)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		return new Vector2((float)(r.X + r.Width), (float)r.Y);
	}

	public static Vector2 BottomLeft(this Rectangle r)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		return new Vector2((float)r.X, (float)(r.Y + r.Height));
	}

	public static Vector2 BottomRight(this Rectangle r)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		return new Vector2((float)(r.X + r.Width), (float)(r.Y + r.Height));
	}

	public static Vector2 Center(this Rectangle r)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		return new Vector2((float)(r.X + r.Width / 2), (float)(r.Y + r.Height / 2));
	}

	public static Vector2 Size(this Rectangle r)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		return new Vector2((float)r.Width, (float)r.Height);
	}

	public static float Distance(this Rectangle r, Vector2 point)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		if (FloatIntersect(((Rectangle)(ref r)).Left, ((Rectangle)(ref r)).Top, r.Width, r.Height, point.X, point.Y, 0f, 0f))
		{
			return 0f;
		}
		if (point.X >= (float)((Rectangle)(ref r)).Left && point.X <= (float)((Rectangle)(ref r)).Right)
		{
			if (point.Y < (float)((Rectangle)(ref r)).Top)
			{
				return (float)((Rectangle)(ref r)).Top - point.Y;
			}
			return point.Y - (float)((Rectangle)(ref r)).Bottom;
		}
		if (point.Y >= (float)((Rectangle)(ref r)).Top && point.Y <= (float)((Rectangle)(ref r)).Bottom)
		{
			if (point.X < (float)((Rectangle)(ref r)).Left)
			{
				return (float)((Rectangle)(ref r)).Left - point.X;
			}
			return point.X - (float)((Rectangle)(ref r)).Right;
		}
		if (point.X < (float)((Rectangle)(ref r)).Left)
		{
			if (point.Y < (float)((Rectangle)(ref r)).Top)
			{
				return Vector2.Distance(point, r.TopLeft());
			}
			return Vector2.Distance(point, r.BottomLeft());
		}
		if (point.Y < (float)((Rectangle)(ref r)).Top)
		{
			return Vector2.Distance(point, r.TopRight());
		}
		return Vector2.Distance(point, r.BottomRight());
	}

	public static Vector2 ClosestPointInRect(this Rectangle r, Vector2 point)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		Vector2 result = point;
		if (result.X < (float)((Rectangle)(ref r)).Left)
		{
			result.X = ((Rectangle)(ref r)).Left;
		}
		if (result.X > (float)((Rectangle)(ref r)).Right)
		{
			result.X = ((Rectangle)(ref r)).Right;
		}
		if (result.Y < (float)((Rectangle)(ref r)).Top)
		{
			result.Y = ((Rectangle)(ref r)).Top;
		}
		if (result.Y > (float)((Rectangle)(ref r)).Bottom)
		{
			result.Y = ((Rectangle)(ref r)).Bottom;
		}
		return result;
	}

	public static Rectangle Modified(this Rectangle r, int x, int y, int w, int h)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		return new Rectangle(r.X + x, r.Y + y, r.Width + w, r.Height + h);
	}

	public static bool IntersectsConeFastInaccurate(this Rectangle targetRect, Vector2 coneCenter, float coneLength, float coneRotation, float maximumAngle)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		Vector2 point = coneCenter + coneRotation.ToRotationVector2() * coneLength;
		Vector2 spinningpoint = targetRect.ClosestPointInRect(point) - coneCenter;
		float num = spinningpoint.RotatedBy(0f - coneRotation).ToRotation();
		if (num < 0f - maximumAngle || num > maximumAngle)
		{
			return false;
		}
		return ((Vector2)(ref spinningpoint)).Length() < coneLength;
	}

	public static bool IntersectsConeSlowMoreAccurate(this Rectangle targetRect, Vector2 coneCenter, float coneLength, float coneRotation, float maximumAngle)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		Vector2 point = coneCenter + coneRotation.ToRotationVector2() * coneLength;
		if (DoesFitInCone(targetRect.ClosestPointInRect(point), coneCenter, coneLength, coneRotation, maximumAngle))
		{
			return true;
		}
		if (DoesFitInCone(targetRect.TopLeft(), coneCenter, coneLength, coneRotation, maximumAngle))
		{
			return true;
		}
		if (DoesFitInCone(targetRect.TopRight(), coneCenter, coneLength, coneRotation, maximumAngle))
		{
			return true;
		}
		if (DoesFitInCone(targetRect.BottomLeft(), coneCenter, coneLength, coneRotation, maximumAngle))
		{
			return true;
		}
		if (DoesFitInCone(targetRect.BottomRight(), coneCenter, coneLength, coneRotation, maximumAngle))
		{
			return true;
		}
		return false;
	}

	public static bool DoesFitInCone(Vector2 point, Vector2 coneCenter, float coneLength, float coneRotation, float maximumAngle)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		Vector2 spinningpoint = point - coneCenter;
		float num = spinningpoint.RotatedBy(0f - coneRotation).ToRotation();
		if (num < 0f - maximumAngle || num > maximumAngle)
		{
			return false;
		}
		return ((Vector2)(ref spinningpoint)).Length() < coneLength;
	}

	public static float ToRotation(this Vector2 v)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		return (float)Math.Atan2(v.Y, v.X);
	}

	public static Vector2 ToRotationVector2(this float f)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		return new Vector2((float)Math.Cos(f), (float)Math.Sin(f));
	}

	public static Vector2 RotatedBy(this Vector2 spinningpoint, double radians, Vector2 center = default(Vector2))
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		float num = (float)Math.Cos(radians);
		float num2 = (float)Math.Sin(radians);
		Vector2 vector = spinningpoint - center;
		Vector2 result = center;
		result.X += vector.X * num - vector.Y * num2;
		result.Y += vector.X * num2 + vector.Y * num;
		return result;
	}

	public static Vector2D RotatedBy(this Vector2D spinningpoint, double radians, Vector2D center = default(Vector2D))
	{
		double num = Math.Cos(radians);
		double num2 = Math.Sin(radians);
		Vector2D vector2D = spinningpoint - center;
		Vector2D result = center;
		result.X += vector2D.X * num - vector2D.Y * num2;
		result.Y += vector2D.X * num2 + vector2D.Y * num;
		return result;
	}

	public static Vector2 RotatedByRandom(this Vector2 spinninpoint, double maxRadians)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		return spinninpoint.RotatedBy(Main.rand.NextDouble() * maxRadians - Main.rand.NextDouble() * maxRadians);
	}

	public static Vector2 Floor(this Vector2 vec)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		vec.X = (int)vec.X;
		vec.Y = (int)vec.Y;
		return vec;
	}

	public static bool HasNaNs(this Vector2 vec)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		if (!float.IsNaN(vec.X))
		{
			return float.IsNaN(vec.Y);
		}
		return true;
	}

	public static bool Between(this Vector2 vec, Vector2 minimum, Vector2 maximum)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		if (vec.X >= minimum.X && vec.X <= maximum.X && vec.Y >= minimum.Y)
		{
			return vec.Y <= maximum.Y;
		}
		return false;
	}

	public static Vector2 ToVector2(this Point p)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		return new Vector2((float)p.X, (float)p.Y);
	}

	public static Vector2 ToVector2(this Point16 p)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		return new Vector2((float)p.X, (float)p.Y);
	}

	public static Vector2D ToVector2D(this Point p)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		return new Vector2D(p.X, p.Y);
	}

	public static Vector2D ToVector2D(this Point16 p)
	{
		return new Vector2D(p.X, p.Y);
	}

	public static Vector2 ToWorldCoordinates(this Point p, float autoAddX = 8f, float autoAddY = 8f)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		return p.ToVector2() * 16f + new Vector2(autoAddX, autoAddY);
	}

	public static Vector2 ToWorldCoordinates(this Point16 p, float autoAddX = 8f, float autoAddY = 8f)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		return p.ToVector2() * 16f + new Vector2(autoAddX, autoAddY);
	}

	public static Vector2 MoveTowards(this Vector2 currentPosition, Vector2 targetPosition, float maxAmountAllowedToMove)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		Vector2 v = targetPosition - currentPosition;
		if (((Vector2)(ref v)).Length() < maxAmountAllowedToMove)
		{
			return targetPosition;
		}
		return currentPosition + v.SafeNormalize(Vector2.Zero) * maxAmountAllowedToMove;
	}

	public static Point16 ToTileCoordinates16(this Vector2 vec)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		return new Point16((int)vec.X >> 4, (int)vec.Y >> 4);
	}

	public static Point16 ToTileCoordinates16(this Vector2D vec)
	{
		return new Point16((int)vec.X >> 4, (int)vec.Y >> 4);
	}

	public static Point ToTileCoordinates(this Vector2 vec)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		return new Point((int)vec.X >> 4, (int)vec.Y >> 4);
	}

	public static Point ToTileCoordinates(this Vector2D vec)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		return new Point((int)vec.X >> 4, (int)vec.Y >> 4);
	}

	public static Point ToPoint(this Vector2 v)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		return new Point((int)v.X, (int)v.Y);
	}

	public static Point ToPoint(this Vector2D v)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		return new Point((int)v.X, (int)v.Y);
	}

	public static Vector2D ToVector2D(this Vector2 v)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		return new Vector2D(v.X, v.Y);
	}

	public static Vector2 SafeNormalize(this Vector2 v, Vector2 defaultValue)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		if (v == Vector2.Zero || v.HasNaNs())
		{
			return defaultValue;
		}
		return Vector2.Normalize(v);
	}

	public static Vector2 ClosestPointOnLine(this Vector2 P, Vector2 A, Vector2 B)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		Vector2 val = P - A;
		Vector2 vector = B - A;
		float num = ((Vector2)(ref vector)).LengthSquared();
		float num2 = Vector2.Dot(val, vector) / num;
		if (num2 < 0f)
		{
			return A;
		}
		if (num2 > 1f)
		{
			return B;
		}
		return A + vector * num2;
	}

	public static bool RectangleLineCollision(Vector2 rectTopLeft, Vector2 rectBottomRight, Vector2 lineStart, Vector2 lineEnd)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		if (lineStart.Between(rectTopLeft, rectBottomRight) || lineEnd.Between(rectTopLeft, rectBottomRight))
		{
			return true;
		}
		Vector2 p = default(Vector2);
		((Vector2)(ref p))._002Ector(rectBottomRight.X, rectTopLeft.Y);
		Vector2 vector = default(Vector2);
		((Vector2)(ref vector))._002Ector(rectTopLeft.X, rectBottomRight.Y);
		Vector2[] array = (Vector2[])(object)new Vector2[4]
		{
			rectTopLeft.ClosestPointOnLine(lineStart, lineEnd),
			p.ClosestPointOnLine(lineStart, lineEnd),
			vector.ClosestPointOnLine(lineStart, lineEnd),
			rectBottomRight.ClosestPointOnLine(lineStart, lineEnd)
		};
		for (int i = 0; i < array.Length; i++)
		{
			if (array[0].Between(rectTopLeft, vector))
			{
				return true;
			}
		}
		return false;
	}

	public static Vector2 RotateRandom(this Vector2 spinninpoint, double maxRadians)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		return spinninpoint.RotatedBy(Main.rand.NextDouble() * maxRadians - Main.rand.NextDouble() * maxRadians);
	}

	public static float AngleTo(this Vector2 Origin, Vector2 Target)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		return (float)Math.Atan2(Target.Y - Origin.Y, Target.X - Origin.X);
	}

	public static float AngleFrom(this Vector2 Origin, Vector2 Target)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		return (float)Math.Atan2(Origin.Y - Target.Y, Origin.X - Target.X);
	}

	public static Vector2 rotateTowards(Vector2 currentPosition, Vector2 currentVelocity, Vector2 targetPosition, float maxChange)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		float num = ((Vector2)(ref currentVelocity)).Length();
		float targetAngle = currentPosition.AngleTo(targetPosition);
		return currentVelocity.ToRotation().AngleTowards(targetAngle, (float)Math.PI / 180f).ToRotationVector2() * num;
	}

	public static float Distance(this Vector2 Origin, Vector2 Target)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		return Vector2.Distance(Origin, Target);
	}

	public static float DistanceSQ(this Vector2 Origin, Vector2 Target)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		return Vector2.DistanceSquared(Origin, Target);
	}

	public static Vector2 DirectionTo(this Vector2 Origin, Vector2 Target)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		return Vector2.Normalize(Target - Origin);
	}

	public static Vector2 DirectionFrom(this Vector2 Origin, Vector2 Target)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		return Vector2.Normalize(Origin - Target);
	}

	public static bool WithinRange(this Vector2 Origin, Vector2 Target, float MaxRange)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		return Vector2.DistanceSquared(Origin, Target) <= MaxRange * MaxRange;
	}

	public static Vector2 XY(this Vector4 vec)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		return new Vector2(vec.X, vec.Y);
	}

	public static Vector2 ZW(this Vector4 vec)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		return new Vector2(vec.Z, vec.W);
	}

	public static Vector3 XZW(this Vector4 vec)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		return new Vector3(vec.X, vec.Z, vec.W);
	}

	public static Vector3 YZW(this Vector4 vec)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		return new Vector3(vec.Y, vec.Z, vec.W);
	}

	public static Color MultiplyRGB(this Color firstColor, Color secondColor)
	{
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		return new Color((int)(byte)((float)(((Color)(ref firstColor)).R * ((Color)(ref secondColor)).R) / 255f), (int)(byte)((float)(((Color)(ref firstColor)).G * ((Color)(ref secondColor)).G) / 255f), (int)(byte)((float)(((Color)(ref firstColor)).B * ((Color)(ref secondColor)).B) / 255f));
	}

	public static Color MultiplyRGBA(this Color firstColor, Color secondColor)
	{
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		return new Color((int)(byte)((float)(((Color)(ref firstColor)).R * ((Color)(ref secondColor)).R) / 255f), (int)(byte)((float)(((Color)(ref firstColor)).G * ((Color)(ref secondColor)).G) / 255f), (int)(byte)((float)(((Color)(ref firstColor)).B * ((Color)(ref secondColor)).B) / 255f), (int)(byte)((float)(((Color)(ref firstColor)).A * ((Color)(ref secondColor)).A) / 255f));
	}

	public static string Hex3(this Color color)
	{
		return (((Color)(ref color)).R.ToString("X2") + ((Color)(ref color)).G.ToString("X2") + ((Color)(ref color)).B.ToString("X2")).ToLower();
	}

	public static string Hex4(this Color color)
	{
		return (((Color)(ref color)).R.ToString("X2") + ((Color)(ref color)).G.ToString("X2") + ((Color)(ref color)).B.ToString("X2") + ((Color)(ref color)).A.ToString("X2")).ToLower();
	}

	public static int ToDirectionInt(this bool value)
	{
		if (!value)
		{
			return -1;
		}
		return 1;
	}

	public static int ToInt(this bool value)
	{
		if (!value)
		{
			return 0;
		}
		return 1;
	}

	public static int ModulusPositive(this int myInteger, int modulusNumber)
	{
		return (myInteger % modulusNumber + modulusNumber) % modulusNumber;
	}

	public static float AngleLerp(this float curAngle, float targetAngle, float amount)
	{
		float angle;
		if (targetAngle < curAngle)
		{
			float num = targetAngle + (float)Math.PI * 2f;
			angle = ((num - curAngle > curAngle - targetAngle) ? MathHelper.Lerp(curAngle, targetAngle, amount) : MathHelper.Lerp(curAngle, num, amount));
		}
		else
		{
			if (!(targetAngle > curAngle))
			{
				return curAngle;
			}
			float num2 = targetAngle - (float)Math.PI * 2f;
			angle = ((targetAngle - curAngle > curAngle - num2) ? MathHelper.Lerp(curAngle, num2, amount) : MathHelper.Lerp(curAngle, targetAngle, amount));
		}
		return MathHelper.WrapAngle(angle);
	}

	public static float AngleTowards(this float curAngle, float targetAngle, float maxChange)
	{
		curAngle = MathHelper.WrapAngle(curAngle);
		targetAngle = MathHelper.WrapAngle(targetAngle);
		if (curAngle < targetAngle)
		{
			if (targetAngle - curAngle > (float)Math.PI)
			{
				curAngle += (float)Math.PI * 2f;
			}
		}
		else if (curAngle - targetAngle > (float)Math.PI)
		{
			curAngle -= (float)Math.PI * 2f;
		}
		curAngle += MathHelper.Clamp(targetAngle - curAngle, 0f - maxChange, maxChange);
		return MathHelper.WrapAngle(curAngle);
	}

	public static bool deepCompare(this int[] firstArray, int[] secondArray)
	{
		if (firstArray == null && secondArray == null)
		{
			return true;
		}
		if (firstArray != null && secondArray != null)
		{
			if (firstArray.Length != secondArray.Length)
			{
				return false;
			}
			for (int i = 0; i < firstArray.Length; i++)
			{
				if (firstArray[i] != secondArray[i])
				{
					return false;
				}
			}
			return true;
		}
		return false;
	}

	public static List<int> GetTrueIndexes(this bool[] array)
	{
		List<int> list = new List<int>();
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i])
			{
				list.Add(i);
			}
		}
		return list;
	}

	public static List<int> GetTrueIndexes(params bool[][] arrays)
	{
		List<int> list = new List<int>();
		foreach (bool[] array in arrays)
		{
			list.AddRange(array.GetTrueIndexes());
		}
		return list.Distinct().ToList();
	}

	public static int Count<T>(this T[] arr, T value)
	{
		int num = 0;
		foreach (T x in arr)
		{
			if (EqualityComparer<T>.Default.Equals(x, value))
			{
				num++;
			}
		}
		return num;
	}

	public static bool PressingShift(this KeyboardState kb)
	{
		if (!((KeyboardState)(ref kb)).IsKeyDown((Keys)160))
		{
			return ((KeyboardState)(ref kb)).IsKeyDown((Keys)161);
		}
		return true;
	}

	public static bool PressingControl(this KeyboardState kb)
	{
		if (!((KeyboardState)(ref kb)).IsKeyDown((Keys)162))
		{
			return ((KeyboardState)(ref kb)).IsKeyDown((Keys)163);
		}
		return true;
	}

	public static R[] MapArray<T, R>(T[] array, Func<T, R> mapper)
	{
		R[] array2 = new R[array.Length];
		for (int i = 0; i < array.Length; i++)
		{
			array2[i] = mapper(array[i]);
		}
		return array2;
	}

	public static bool PlotLine(Point16 p0, Point16 p1, TileActionAttempt plot, bool jump = true)
	{
		return PlotLine(p0.X, p0.Y, p1.X, p1.Y, plot, jump);
	}

	public static bool PlotLine(Point p0, Point p1, TileActionAttempt plot, bool jump = true)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		return PlotLine(p0.X, p0.Y, p1.X, p1.Y, plot, jump);
	}

	private static bool PlotLine(int x0, int y0, int x1, int y1, TileActionAttempt plot, bool jump = true)
	{
		if (x0 == x1 && y0 == y1)
		{
			return plot(x0, y0);
		}
		bool flag = Math.Abs(y1 - y0) > Math.Abs(x1 - x0);
		if (flag)
		{
			Swap(ref x0, ref y0);
			Swap(ref x1, ref y1);
		}
		int num = Math.Abs(x1 - x0);
		int num2 = Math.Abs(y1 - y0);
		int num3 = num / 2;
		int num4 = y0;
		int num5 = ((x0 < x1) ? 1 : (-1));
		int num6 = ((y0 < y1) ? 1 : (-1));
		for (int i = x0; i != x1; i += num5)
		{
			if (flag)
			{
				if (!plot(num4, i))
				{
					return false;
				}
			}
			else if (!plot(i, num4))
			{
				return false;
			}
			num3 -= num2;
			if (num3 >= 0)
			{
				continue;
			}
			num4 += num6;
			if (!jump)
			{
				if (flag)
				{
					if (!plot(num4, i))
					{
						return false;
					}
				}
				else if (!plot(i, num4))
				{
					return false;
				}
			}
			num3 += num;
		}
		return true;
	}

	public static int RandomNext(ref ulong seed, int bits)
	{
		seed = RandomNextSeed(seed);
		return (int)(seed >> 48 - bits);
	}

	public static ulong RandomNextSeed(ulong seed)
	{
		return (seed * 25214903917L + 11) & 0xFFFFFFFFFFFFuL;
	}

	public static float RandomFloat(ref ulong seed)
	{
		return (float)RandomNext(ref seed, 24) / 16777216f;
	}

	public static int RandomInt(ref ulong seed, int max)
	{
		if ((max & -max) == max)
		{
			return (int)((long)max * (long)RandomNext(ref seed, 31) >> 31);
		}
		int num;
		int num2;
		do
		{
			num = RandomNext(ref seed, 31);
			num2 = num % max;
		}
		while (num - num2 + (max - 1) < 0);
		return num2;
	}

	public static int RandomInt(ref ulong seed, int min, int max)
	{
		return RandomInt(ref seed, max - min) + min;
	}

	public static bool PlotTileLine(Vector2 start, Vector2 end, float width, TileActionAttempt plot)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		return PlotTileLine(start.ToVector2D(), end.ToVector2D(), (double)width, plot);
	}

	public static bool PlotTileLine(Vector2D start, Vector2D end, double width, TileActionAttempt plot)
	{
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		double num = width / 2.0;
		Vector2D vector2D = end - start;
		Vector2D vector2D2 = vector2D / vector2D.Length();
		Vector2D vector2D3 = new Vector2D(0.0 - vector2D2.Y, vector2D2.X) * num;
		Point point = (start - vector2D3).ToTileCoordinates();
		Point point2 = (start + vector2D3).ToTileCoordinates();
		Point point3 = start.ToTileCoordinates();
		Point point4 = end.ToTileCoordinates();
		Point lineMinOffset = new Point(point.X - point3.X, point.Y - point3.Y);
		Point lineMaxOffset = new Point(point2.X - point3.X, point2.Y - point3.Y);
		return PlotLine(point3.X, point3.Y, point4.X, point4.Y, (int x, int y) => PlotLine(x + lineMinOffset.X, y + lineMinOffset.Y, x + lineMaxOffset.X, y + lineMaxOffset.Y, plot, jump: false));
	}

	public static bool PlotTileTale(Vector2D start, Vector2D end, double width, TileActionAttempt plot)
	{
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		double halfWidth = width / 2.0;
		Vector2D vector2D = end - start;
		Vector2D vector2D2 = vector2D / vector2D.Length();
		Vector2D perpOffset = new Vector2D(0.0 - vector2D2.Y, vector2D2.X);
		Point pointStart = start.ToTileCoordinates();
		Point point = end.ToTileCoordinates();
		int length = 0;
		PlotLine(pointStart.X, pointStart.Y, point.X, point.Y, delegate
		{
			length++;
			return true;
		});
		length--;
		int curLength = 0;
		return PlotLine(pointStart.X, pointStart.Y, point.X, point.Y, delegate(int x, int y)
		{
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
			double num = 1.0 - (double)curLength / (double)length;
			curLength++;
			Point val = (start - perpOffset * halfWidth * num).ToTileCoordinates();
			Point val2 = (start + perpOffset * halfWidth * num).ToTileCoordinates();
			Point val3 = default(Point);
			((Point)(ref val3))._002Ector(val.X - pointStart.X, val.Y - pointStart.Y);
			Point val4 = default(Point);
			((Point)(ref val4))._002Ector(val2.X - pointStart.X, val2.Y - pointStart.Y);
			return PlotLine(x + val3.X, y + val3.Y, x + val4.X, y + val4.Y, plot, jump: false);
		});
	}

	public static bool PlotTileArea(int x, int y, TileActionAttempt plot)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		if (!WorldGen.InWorld(x, y))
		{
			return false;
		}
		List<Point> list = new List<Point>();
		List<Point> list2 = new List<Point>();
		HashSet<Point> hashSet = new HashSet<Point>();
		list2.Add(new Point(x, y));
		Point item2 = default(Point);
		while (list2.Count > 0)
		{
			list.Clear();
			list.AddRange(list2);
			list2.Clear();
			while (list.Count > 0)
			{
				Point item = list[0];
				if (!WorldGen.InWorld(item.X, item.Y, 1))
				{
					list.Remove(item);
					continue;
				}
				hashSet.Add(item);
				list.Remove(item);
				if (plot(item.X, item.Y))
				{
					((Point)(ref item2))._002Ector(item.X - 1, item.Y);
					if (!hashSet.Contains(item2))
					{
						list2.Add(item2);
					}
					((Point)(ref item2))._002Ector(item.X + 1, item.Y);
					if (!hashSet.Contains(item2))
					{
						list2.Add(item2);
					}
					((Point)(ref item2))._002Ector(item.X, item.Y - 1);
					if (!hashSet.Contains(item2))
					{
						list2.Add(item2);
					}
					((Point)(ref item2))._002Ector(item.X, item.Y + 1);
					if (!hashSet.Contains(item2))
					{
						list2.Add(item2);
					}
				}
			}
		}
		return true;
	}

	public static int RandomConsecutive(double random, int odds)
	{
		return (int)Math.Log(1.0 - random, 1.0 / (double)odds);
	}

	public static Vector2 RandomVector2(UnifiedRandom random, float min, float max)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		return new Vector2((max - min) * (float)random.NextDouble() + min, (max - min) * (float)random.NextDouble() + min);
	}

	public static Vector2D RandomVector2D(UnifiedRandom random, double min, double max)
	{
		return new Vector2D((max - min) * random.NextDouble() + min, (max - min) * random.NextDouble() + min);
	}

	public static bool IndexInRange<T>(this T[] t, int index)
	{
		if (index >= 0)
		{
			return index < t.Length;
		}
		return false;
	}

	public static bool IndexInRange<T>(this List<T> t, int index)
	{
		if (index >= 0)
		{
			return index < t.Count;
		}
		return false;
	}

	public static T SelectRandom<T>(UnifiedRandom random, params T[] choices)
	{
		return choices[random.Next(choices.Length)];
	}

	public static void DrawBorderStringFourWay(SpriteBatch sb, DynamicSpriteFont font, string text, float x, float y, Color textColor, Color borderColor, Vector2 origin, float scale = 1f)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		Color color = borderColor;
		Vector2 zero = Vector2.Zero;
		for (int i = 0; i < 5; i++)
		{
			switch (i)
			{
			case 0:
				zero.X = x - 2f;
				zero.Y = y;
				break;
			case 1:
				zero.X = x + 2f;
				zero.Y = y;
				break;
			case 2:
				zero.X = x;
				zero.Y = y - 2f;
				break;
			case 3:
				zero.X = x;
				zero.Y = y + 2f;
				break;
			default:
				zero.X = x;
				zero.Y = y;
				color = textColor;
				break;
			}
			sb.DrawString(font, text, zero, color, 0f, origin, scale, (SpriteEffects)0, 0f);
		}
	}

	public static Vector2 DrawBorderString(SpriteBatch sb, string text, Vector2 pos, Color color, float scale = 1f, float anchorx = 0f, float anchory = 0f, int maxCharactersDisplayed = -1)
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		if (maxCharactersDisplayed != -1 && text.Length > maxCharactersDisplayed)
		{
			text.Substring(0, maxCharactersDisplayed);
		}
		DynamicSpriteFont value = FontAssets.MouseText.Value;
		Vector2 vector = value.MeasureString(text);
		ChatManager.DrawColorCodedStringWithShadow(sb, value, text, pos, color, 0f, new Vector2(anchorx, anchory) * vector, new Vector2(scale), -1f, 1.5f);
		return vector * scale;
	}

	public static Vector2 DrawBorderStringBig(SpriteBatch spriteBatch, string text, Vector2 pos, Color color, float scale = 1f, float anchorx = 0f, float anchory = 0f, int maxCharactersDisplayed = -1)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		if (maxCharactersDisplayed != -1 && text.Length > maxCharactersDisplayed)
		{
			text.Substring(0, maxCharactersDisplayed);
		}
		DynamicSpriteFont value = FontAssets.DeathText.Value;
		TextSnippet[] snippets = ChatManager.ParseMessage(text, color).ToArray();
		ChatManager.ConvertNormalSnippets(snippets);
		Vector2 textSize = ChatManager.GetStringSize(value, snippets, Vector2.One);
		int hoveredSnippet;
		for (int i = -1; i < 2; i++)
		{
			for (int j = -1; j < 2; j++)
			{
				ChatManager.DrawColorCodedString(spriteBatch, value, snippets, pos + new Vector2((float)i, (float)j), Color.Black, 0f, new Vector2(anchorx, anchory) * textSize, new Vector2(scale), out hoveredSnippet, -1f, ignoreColors: true);
			}
		}
		ChatManager.DrawColorCodedString(spriteBatch, value, snippets, pos, color, 0f, new Vector2(anchorx, anchory) * textSize, new Vector2(scale), out hoveredSnippet, -1f);
		return textSize * scale;
	}

	public static void DrawInvBG(SpriteBatch sb, Rectangle R, Color c = default(Color))
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		DrawInvBG(sb, R.X, R.Y, R.Width, R.Height, c);
	}

	public static void DrawInvBG(SpriteBatch sb, float x, float y, float w, float h, Color c = default(Color))
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		DrawInvBG(sb, (int)x, (int)y, (int)w, (int)h, c);
	}

	public static void DrawInvBG(SpriteBatch sb, int x, int y, int w, int h, Color c = default(Color))
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0170: Unknown result type (might be due to invalid IL or missing references)
		//IL_0183: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0204: Unknown result type (might be due to invalid IL or missing references)
		if (c == default(Color))
		{
			c = new Color(63, 65, 151, 255) * 0.785f;
		}
		Texture2D value = TextureAssets.InventoryBack13.Value;
		if (w < 20)
		{
			w = 20;
		}
		if (h < 20)
		{
			h = 20;
		}
		sb.Draw(value, new Rectangle(x, y, 10, 10), (Rectangle?)new Rectangle(0, 0, 10, 10), c);
		sb.Draw(value, new Rectangle(x + 10, y, w - 20, 10), (Rectangle?)new Rectangle(10, 0, 10, 10), c);
		sb.Draw(value, new Rectangle(x + w - 10, y, 10, 10), (Rectangle?)new Rectangle(value.Width - 10, 0, 10, 10), c);
		sb.Draw(value, new Rectangle(x, y + 10, 10, h - 20), (Rectangle?)new Rectangle(0, 10, 10, 10), c);
		sb.Draw(value, new Rectangle(x + 10, y + 10, w - 20, h - 20), (Rectangle?)new Rectangle(10, 10, 10, 10), c);
		sb.Draw(value, new Rectangle(x + w - 10, y + 10, 10, h - 20), (Rectangle?)new Rectangle(value.Width - 10, 10, 10, 10), c);
		sb.Draw(value, new Rectangle(x, y + h - 10, 10, 10), (Rectangle?)new Rectangle(0, value.Height - 10, 10, 10), c);
		sb.Draw(value, new Rectangle(x + 10, y + h - 10, w - 20, 10), (Rectangle?)new Rectangle(10, value.Height - 10, 10, 10), c);
		sb.Draw(value, new Rectangle(x + w - 10, y + h - 10, 10, 10), (Rectangle?)new Rectangle(value.Width - 10, value.Height - 10, 10, 10), c);
	}

	public static string ReadEmbeddedResource(string path)
	{
		using Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(path);
		using StreamReader streamReader = new StreamReader(stream);
		return streamReader.ReadToEnd();
	}

	public static void DrawSplicedPanel(SpriteBatch sb, Texture2D texture, int x, int y, int w, int h, int leftEnd, int rightEnd, int topEnd, int bottomEnd, Color c)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Unknown result type (might be due to invalid IL or missing references)
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0204: Unknown result type (might be due to invalid IL or missing references)
		//IL_021f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0229: Unknown result type (might be due to invalid IL or missing references)
		if (w < leftEnd + rightEnd)
		{
			w = leftEnd + rightEnd;
		}
		if (h < topEnd + bottomEnd)
		{
			h = topEnd + bottomEnd;
		}
		sb.Draw(texture, new Rectangle(x, y, leftEnd, topEnd), (Rectangle?)new Rectangle(0, 0, leftEnd, topEnd), c);
		sb.Draw(texture, new Rectangle(x + leftEnd, y, w - leftEnd - rightEnd, topEnd), (Rectangle?)new Rectangle(leftEnd, 0, texture.Width - leftEnd - rightEnd, topEnd), c);
		sb.Draw(texture, new Rectangle(x + w - rightEnd, y, topEnd, rightEnd), (Rectangle?)new Rectangle(texture.Width - rightEnd, 0, rightEnd, topEnd), c);
		sb.Draw(texture, new Rectangle(x, y + topEnd, leftEnd, h - topEnd - bottomEnd), (Rectangle?)new Rectangle(0, topEnd, leftEnd, texture.Height - topEnd - bottomEnd), c);
		sb.Draw(texture, new Rectangle(x + leftEnd, y + topEnd, w - leftEnd - rightEnd, h - topEnd - bottomEnd), (Rectangle?)new Rectangle(leftEnd, topEnd, texture.Width - leftEnd - rightEnd, texture.Height - topEnd - bottomEnd), c);
		sb.Draw(texture, new Rectangle(x + w - rightEnd, y + topEnd, rightEnd, h - topEnd - bottomEnd), (Rectangle?)new Rectangle(texture.Width - rightEnd, topEnd, rightEnd, texture.Height - topEnd - bottomEnd), c);
		sb.Draw(texture, new Rectangle(x, y + h - bottomEnd, leftEnd, bottomEnd), (Rectangle?)new Rectangle(0, texture.Height - bottomEnd, leftEnd, bottomEnd), c);
		sb.Draw(texture, new Rectangle(x + leftEnd, y + h - bottomEnd, w - leftEnd - rightEnd, bottomEnd), (Rectangle?)new Rectangle(leftEnd, texture.Height - bottomEnd, texture.Width - leftEnd - rightEnd, bottomEnd), c);
		sb.Draw(texture, new Rectangle(x + w - rightEnd, y + h - bottomEnd, rightEnd, bottomEnd), (Rectangle?)new Rectangle(texture.Width - rightEnd, texture.Height - bottomEnd, rightEnd, bottomEnd), c);
	}

	public static void DrawSettingsPanel(SpriteBatch spriteBatch, Vector2 position, float width, Color color)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		DrawPanel(TextureAssets.SettingsPanel.Value, 2, 0, spriteBatch, position, width, color);
	}

	public static void DrawSettings2Panel(SpriteBatch spriteBatch, Vector2 position, float width, Color color)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		DrawPanel(TextureAssets.SettingsPanel.Value, 2, 0, spriteBatch, position, width, color);
	}

	public static void DrawPanel(Texture2D texture, int edgeWidth, int edgeShove, SpriteBatch spriteBatch, Vector2 position, float width, Color color)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		spriteBatch.Draw(texture, position, (Rectangle?)new Rectangle(0, 0, edgeWidth, texture.Height), color);
		spriteBatch.Draw(texture, new Vector2(position.X + (float)edgeWidth, position.Y), (Rectangle?)new Rectangle(edgeWidth + edgeShove, 0, texture.Width - (edgeWidth + edgeShove) * 2, texture.Height), color, 0f, Vector2.Zero, new Vector2((width - (float)(edgeWidth * 2)) / (float)(texture.Width - (edgeWidth + edgeShove) * 2), 1f), (SpriteEffects)0, 0f);
		spriteBatch.Draw(texture, new Vector2(position.X + width - (float)edgeWidth, position.Y), (Rectangle?)new Rectangle(texture.Width - edgeWidth, 0, edgeWidth, texture.Height), color);
	}

	public static void DrawRectangle(SpriteBatch sb, Vector2 start, Vector2 end, Color colorStart, Color colorEnd, float width)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		DrawLine(sb, start, new Vector2(start.X, end.Y), colorStart, colorEnd, width);
		DrawLine(sb, start, new Vector2(end.X, start.Y), colorStart, colorEnd, width);
		DrawLine(sb, end, new Vector2(start.X, end.Y), colorStart, colorEnd, width);
		DrawLine(sb, end, new Vector2(end.X, start.Y), colorStart, colorEnd, width);
	}

	public static void DrawLaser(SpriteBatch sb, Texture2D tex, Vector2 start, Vector2 end, Vector2 scale, LaserLineFraming framing)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		//IL_017f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		//IL_018c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		Vector2 vector = start;
		Vector2 vector2 = Vector2.Normalize(end - start);
		Vector2 val = end - start;
		float num = ((Vector2)(ref val)).Length();
		float rotation = vector2.ToRotation() - (float)Math.PI / 2f;
		if (vector2.HasNaNs())
		{
			return;
		}
		framing(0, vector, num, default(Rectangle), out var distanceCovered, out var frame, out var origin, out var color);
		sb.Draw(tex, vector, (Rectangle?)frame, color, rotation, frame.Size() / 2f, scale, (SpriteEffects)0, 0f);
		num -= distanceCovered * scale.Y;
		vector += vector2 * ((float)frame.Height - origin.Y) * scale.Y;
		if (num > 0f)
		{
			float num2 = 0f;
			while (num2 + 1f < num)
			{
				framing(1, vector, num - num2, frame, out distanceCovered, out frame, out origin, out color);
				if (num - num2 < (float)frame.Height)
				{
					distanceCovered *= (num - num2) / (float)frame.Height;
					frame.Height = (int)(num - num2);
				}
				sb.Draw(tex, vector, (Rectangle?)frame, color, rotation, origin, scale, (SpriteEffects)0, 0f);
				num2 += distanceCovered * scale.Y;
				vector += vector2 * distanceCovered * scale.Y;
			}
		}
		framing(2, vector, num, default(Rectangle), out distanceCovered, out frame, out origin, out color);
		sb.Draw(tex, vector, (Rectangle?)frame, color, rotation, origin, scale, (SpriteEffects)0, 0f);
	}

	public static void DrawLine(SpriteBatch spriteBatch, Point start, Point end, Color color)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		Utils.DrawLine(spriteBatch, new Vector2((float)(start.X << 4), (float)(start.Y << 4)), new Vector2((float)(end.X << 4), (float)(end.Y << 4)), color);
	}

	public static void DrawLine(SpriteBatch spriteBatch, Vector2 start, Vector2 end, Color color)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		float num = Vector2.Distance(start, end);
		Vector2 vector = (end - start) / num;
		Vector2 vector2 = start;
		Vector2 screenPosition = Main.screenPosition;
		float rotation = vector.ToRotation();
		for (float num2 = 0f; num2 <= num; num2 += 4f)
		{
			float num3 = num2 / num;
			spriteBatch.Draw(TextureAssets.BlackTile.Value, vector2 - screenPosition, (Rectangle?)null, new Color(new Vector4(num3, num3, num3, 1f) * ((Color)(ref color)).ToVector4()), rotation, Vector2.Zero, 0.25f, (SpriteEffects)0, 0f);
			vector2 = start + num2 * vector;
		}
	}

	public static void DrawLine(SpriteBatch spriteBatch, Vector2 start, Vector2 end, Color colorStart, Color colorEnd, float width)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		float num = Vector2.Distance(start, end);
		Vector2 vector = (end - start) / num;
		Vector2 vector2 = start;
		Vector2 screenPosition = Main.screenPosition;
		float rotation = vector.ToRotation();
		float scale = width / 16f;
		for (float num2 = 0f; num2 <= num; num2 += width)
		{
			float amount = num2 / num;
			spriteBatch.Draw(TextureAssets.BlackTile.Value, vector2 - screenPosition, (Rectangle?)null, Color.Lerp(colorStart, colorEnd, amount), rotation, Vector2.Zero, scale, (SpriteEffects)0, 0f);
			vector2 = start + num2 * vector;
		}
	}

	public static void DrawRectForTilesInWorld(SpriteBatch spriteBatch, Rectangle rect, Color color)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		DrawRectForTilesInWorld(spriteBatch, new Point(rect.X, rect.Y), new Point(rect.X + rect.Width, rect.Y + rect.Height), color);
	}

	public static void DrawRectForTilesInWorld(SpriteBatch spriteBatch, Point start, Point end, Color color)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		DrawRect(spriteBatch, new Vector2((float)(start.X << 4), (float)(start.Y << 4)), new Vector2((float)((end.X << 4) - 4), (float)((end.Y << 4) - 4)), color);
	}

	public static void DrawRect(SpriteBatch spriteBatch, Rectangle rect, Color color)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		DrawRect(spriteBatch, new Vector2((float)rect.X, (float)rect.Y), new Vector2((float)(rect.X + rect.Width), (float)(rect.Y + rect.Height)), color);
	}

	public static void DrawRect(SpriteBatch spriteBatch, Vector2 start, Vector2 end, Color color)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		DrawLine(spriteBatch, start, new Vector2(start.X, end.Y), color);
		DrawLine(spriteBatch, start, new Vector2(end.X, start.Y), color);
		DrawLine(spriteBatch, end, new Vector2(start.X, end.Y), color);
		DrawLine(spriteBatch, end, new Vector2(end.X, start.Y), color);
	}

	public static void DrawRect(SpriteBatch spriteBatch, Vector2 topLeft, Vector2 topRight, Vector2 bottomRight, Vector2 bottomLeft, Color color)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		DrawLine(spriteBatch, topLeft, topRight, color);
		DrawLine(spriteBatch, topRight, bottomRight, color);
		DrawLine(spriteBatch, bottomRight, bottomLeft, color);
		DrawLine(spriteBatch, bottomLeft, topLeft, color);
	}

	public static void DrawCursorSingle(SpriteBatch sb, Color color, float rot = float.NaN, float scale = 1f, Vector2 manualPosition = default(Vector2), int cursorSlot = 0, int specialMode = 0)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_0158: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0169: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		bool flag = false;
		bool flag2 = true;
		bool flag3 = true;
		Vector2 origin = Vector2.Zero;
		Vector2 vector = default(Vector2);
		((Vector2)(ref vector))._002Ector((float)Main.mouseX, (float)Main.mouseY);
		if (manualPosition != Vector2.Zero)
		{
			vector = manualPosition;
		}
		if (float.IsNaN(rot))
		{
			rot = 0f;
		}
		else
		{
			flag = true;
			rot -= (float)Math.PI * 3f / 4f;
		}
		if (cursorSlot == 4 || cursorSlot == 5)
		{
			flag2 = false;
			((Vector2)(ref origin))._002Ector(8f);
			if (flag && specialMode == 0)
			{
				float num = rot;
				if (num < 0f)
				{
					num += (float)Math.PI * 2f;
				}
				for (float num2 = 0f; num2 < 4f; num2 += 1f)
				{
					if (Math.Abs(num - (float)Math.PI / 2f * num2) <= (float)Math.PI / 4f)
					{
						rot = (float)Math.PI / 2f * num2;
						break;
					}
				}
			}
		}
		Vector2 vector2 = Vector2.One;
		if ((Main.ThickMouse && cursorSlot == 0) || cursorSlot == 1)
		{
			vector2 = Main.DrawThickCursor(cursorSlot == 1);
		}
		if (flag2)
		{
			sb.Draw(TextureAssets.Cursors[cursorSlot].Value, vector + vector2 + Vector2.One, (Rectangle?)null, color.MultiplyRGB(new Color(0.2f, 0.2f, 0.2f, 0.5f)), rot, origin, scale * 1.1f, (SpriteEffects)0, 0f);
		}
		if (flag3)
		{
			sb.Draw(TextureAssets.Cursors[cursorSlot].Value, vector + vector2, (Rectangle?)null, color, rot, origin, scale, (SpriteEffects)0, 0f);
		}
	}

	public static Vector2 ToWorldCoordinates(this Point p, Vector2 autoAddXY)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		return p.ToWorldCoordinates(autoAddXY.X, autoAddXY.Y);
	}

	public static Vector2 ToWorldCoordinates(this Point16 p, Vector2 autoAddXY)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		return p.ToVector2().ToWorldCoordinates(autoAddXY);
	}

	public static Vector2 ToWorldCoordinates(this Vector2 v, float autoAddX = 8f, float autoAddY = 8f)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		return v.ToWorldCoordinates(new Vector2(autoAddX, autoAddY));
	}

	public static Vector2 ToWorldCoordinates(this Vector2 v, Vector2 autoAddXY)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		return v * 16f + autoAddXY;
	}

	public static Point ToPoint(this Point16 p)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		return new Point((int)p.X, (int)p.Y);
	}

	public static Point16 ToPoint16(this Vector2 v)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		return new Point16((short)v.X, (short)v.Y);
	}

	public static DateTime UnixTimeStampToDateTime(long unixTimeStamp)
	{
		return DateTimeOffset.FromUnixTimeSeconds(unixTimeStamp).UtcDateTime;
	}

	public static T NextEnum<T>(this T src) where T : struct
	{
		if (!typeof(T).IsEnum)
		{
			throw new ArgumentException("Argumnent " + typeof(T).FullName + " is not an Enum");
		}
		T[] Arr = (T[])Enum.GetValues(src.GetType());
		int i = Array.IndexOf(Arr, src) + 1;
		if (Arr.Length != i)
		{
			return Arr[i];
		}
		return Arr[0];
	}

	public static T PreviousEnum<T>(this T src) where T : struct
	{
		if (!typeof(T).IsEnum)
		{
			throw new ArgumentException("Argumnent " + typeof(T).FullName + " is not an Enum");
		}
		T[] Arr = (T[])Enum.GetValues(src.GetType());
		int i = Array.IndexOf(Arr, src) - 1;
		if (i >= 0)
		{
			return Arr[i];
		}
		return Arr[^1];
	}

	public static Version MajorMinor(this Version v)
	{
		return new Version(v.Major, v.Minor);
	}

	public static Version MajorMinorBuild(this Version v)
	{
		if (v.Build >= 0)
		{
			return new Version(v.Major, v.Minor, v.Build);
		}
		return v.MajorMinor();
	}

	/// <summary>
	/// Returns a random element from the provided array.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="r"></param>
	/// <param name="array"></param>
	/// <returns></returns>
	public static T Next<T>(this UnifiedRandom r, T[] array)
	{
		return array[r.Next(array.Length)];
	}

	/// <summary>
	/// Returns a random element from the provided list.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="r"></param>
	/// <param name="list"></param>
	/// <returns></returns>
	public static T Next<T>(this UnifiedRandom r, IList<T> list)
	{
		return list[r.Next(list.Count)];
	}

	/// <summary>
	/// Generates a random value between 0f (inclusive) and <paramref name="maxValue" /> (exclusive). <br />It will not return <paramref name="maxValue" />.
	/// </summary>
	/// <param name="r"></param>
	/// <param name="maxValue"></param>
	/// <returns></returns>
	public static float NextFloat(this UnifiedRandom r, float maxValue)
	{
		return (float)r.NextDouble() * maxValue;
	}

	/// <summary>
	/// Generates a random value between <paramref name="minValue" /> (inclusive) and <paramref name="maxValue" /> (exclusive). <br />It will not return <paramref name="maxValue" />.
	/// </summary>
	/// <param name="r"></param>
	/// <param name="minValue"></param>
	/// <param name="maxValue"></param>
	/// <returns></returns>
	public static float NextFloat(this UnifiedRandom r, float minValue, float maxValue)
	{
		return (float)r.NextDouble() * (maxValue - minValue) + minValue;
	}

	/// <summary>
	/// Returns true or false randomly with equal chance.
	/// </summary>
	/// <param name="r"></param>
	/// <returns></returns>
	public static bool NextBool(this UnifiedRandom r)
	{
		return r.NextDouble() < 0.5;
	}

	/// <summary> Returns true 1 out of X times. </summary>
	public static bool NextBool(this UnifiedRandom r, int consequent)
	{
		if (consequent < 1)
		{
			throw new ArgumentOutOfRangeException("consequent", "consequent must be greater than or equal to 1.");
		}
		return r.Next(consequent) == 0;
	}

	/// <summary> Returns true X out of Y times. </summary>
	public static bool NextBool(this UnifiedRandom r, int antecedent, int consequent)
	{
		if (antecedent > consequent)
		{
			throw new ArgumentOutOfRangeException("antecedent", "antecedent must be less than or equal to consequent.");
		}
		return r.Next(consequent) < antecedent;
	}

	public static int Repeat(int value, int length)
	{
		if (value < 0)
		{
			return value % length + length;
		}
		return value % length;
	}

	/// <summary>
	/// Bit packs a BitArray in to a Byte Array and then sends the byte array
	/// </summary>
	public static void SendBitArray(BitArray arr, BinaryWriter writer)
	{
		byte[] result = new byte[(arr.Length - 1) / 8 + 1];
		arr.CopyTo(result, 0);
		writer.Write(result);
	}

	/// <summary>
	/// Receives the result of SendBitArray, and returns the corresponding BitArray
	/// </summary>
	public static BitArray ReceiveBitArray(int BitArrLength, BinaryReader reader)
	{
		byte[] receive = new byte[(BitArrLength - 1) / 8 + 1];
		receive = reader.ReadBytes(receive.Length);
		return new BitArray(receive);
	}

	public static void OpenToURL(string url)
	{
		if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
		{
			Process.Start("explorer.exe", "\"" + url + "\"");
		}
		else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
		{
			Process.Start("open", "\"" + url + "\"");
		}
		else
		{
			Process.Start("xdg-open", "\"" + url + "\"");
		}
	}

	public static void ShowFancyErrorMessage(string message, int returnToMenu, UIState returnToState = null)
	{
		if (!Main.dedServ)
		{
			Logging.tML.Error((object)message);
			Interface.errorMessage.Show(message, returnToMenu, returnToState);
		}
		else
		{
			LogAndConsoleErrorMessage(message);
		}
	}

	public static void LogAndChatAndConsoleInfoMessage(string message)
	{
		Logging.tML.Info((object)message);
		if (Main.dedServ)
		{
			Console.WriteLine(message);
		}
		else
		{
			Main.NewText(message);
		}
	}

	public static void LogAndConsoleInfoMessage(string message)
	{
		Logging.tML.Info((object)message);
		if (Main.dedServ)
		{
			Console.WriteLine(message);
		}
	}

	public static void LogAndConsoleInfoMessageFormat(string format, params object[] args)
	{
		LogAndConsoleInfoMessage(string.Format(format, args));
	}

	public static void LogAndConsoleErrorMessage(string message)
	{
		Logging.tML.Error((object)message);
		if (Main.dedServ)
		{
			Console.ForegroundColor = ConsoleColor.DarkRed;
			Console.WriteLine("ERROR: " + message);
			Console.ResetColor();
		}
	}
}
