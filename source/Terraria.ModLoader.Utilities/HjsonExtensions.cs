using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using Hjson;

namespace Terraria.ModLoader.Utilities;

internal static class HjsonExtensions
{
	public struct HjsonStyle
	{
		public bool WriteComments;

		public bool EmitRootBraces;

		public bool NoIndentaion;

		public string Separator;
	}

	private delegate bool TryParseNumericLiteralDelegate(string text, bool stopAtNext, out JsonValue value);

	private const string JsonWriter = "Hjson.JsonWriter";

	private const string HjsonReader = "Hjson.HjsonReader";

	private const string HjsonWriter = "Hjson.HjsonWriter";

	private const string HjsonValue = "Hjson.HjsonValue";

	public static readonly HjsonStyle DefaultHjsonStyle = new HjsonStyle
	{
		WriteComments = true,
		EmitRootBraces = false,
		NoIndentaion = false,
		Separator = " "
	};

	private static readonly TryParseNumericLiteralDelegate tryParseNumericLiteral = GetDelegateOfMethod<TryParseNumericLiteralDelegate>("Hjson.HjsonReader", "TryParseNumericLiteral");

	private static readonly Func<string, string> escapeString = GetDelegateOfMethod<Func<string, string>>("Hjson.JsonWriter", "EscapeString");

	private static readonly Func<string, string> escapeName = GetDelegateOfMethod<Func<string, string>>("Hjson.HjsonWriter", "escapeName");

	private static readonly Func<string, bool> startsWithKeyword = GetDelegateOfMethod<Func<string, bool>>("Hjson.HjsonWriter", "startsWithKeyword");

	private static readonly Func<char, bool> needsEscapeML = GetDelegateOfMethod<Func<char, bool>>("Hjson.HjsonWriter", "needsEscapeML");

	private static readonly Func<char, bool> needsEscape = GetDelegateOfMethod<Func<char, bool>>("Hjson.HjsonWriter", "needsEscape");

	private static readonly Func<char, bool> needsQuotes = GetDelegateOfMethod<Func<char, bool>>("Hjson.HjsonWriter", "needsQuotes");

	private static readonly Func<char, bool> isPunctuatorChar = GetDelegateOfMethod<Func<char, bool>>("Hjson.HjsonValue", "IsPunctuatorChar");

	public static string ToFancyHjsonString(this JsonValue value, HjsonStyle? style = null)
	{
		StringWriter stringWriter = new StringWriter();
		HjsonStyle usedStyle = style ?? DefaultHjsonStyle;
		WriteFancyHjsonValue(stringWriter, value, 0, in usedStyle, hasComments: false, noIndentation: true, isRootObject: true);
		return stringWriter.ToString();
	}

	private static void WriteFancyHjsonValue(TextWriter tw, JsonValue value, int level, in HjsonStyle style, bool hasComments = false, bool noIndentation = false, bool isRootObject = false)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Expected I4, but got Unknown
		//IL_02fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0308: Unknown result type (might be due to invalid IL or missing references)
		//IL_0327: Unknown result type (might be due to invalid IL or missing references)
		//IL_032b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0330: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_034c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
		JsonType jsonType = value.JsonType;
		switch ((int)jsonType)
		{
		case 2:
		{
			JsonObject jObject = JsonUtil.Qo(value);
			WscJsonObject commentedObject = (WscJsonObject)(object)(style.WriteComments ? ((jObject is WscJsonObject) ? jObject : null) : null);
			bool showBraces = !isRootObject || ((commentedObject != null) ? commentedObject.RootBraces : style.EmitRootBraces);
			if (!noIndentation || showBraces)
			{
				tw.Write(style.Separator);
			}
			if (showBraces)
			{
				tw.Write("{");
			}
			if (commentedObject != null)
			{
				bool skipFirst = !showBraces;
				string kwl = GetComments(commentedObject.Comments, "");
				JsonType? lastJsonType = null;
				foreach (string key in commentedObject.Order.Concat(((JsonObject)commentedObject).Keys).Distinct())
				{
					if (!((JsonValue)jObject).ContainsKey(key))
					{
						continue;
					}
					JsonValue val = ((JsonValue)jObject)[key];
					if (((JsonType?)val.JsonType != lastJsonType && lastJsonType.HasValue) || lastJsonType == (JsonType?)2)
					{
						NewLine(tw, 0);
					}
					if (!skipFirst)
					{
						NewLine(tw, level + (showBraces ? 1 : 0));
					}
					else
					{
						skipFirst = false;
					}
					if (!string.IsNullOrWhiteSpace(kwl))
					{
						string indentation = new string('\t', level + (showBraces ? 1 : 0));
						string[] lines = (from s in kwl.TrimStart().Split(new string[3] { "\r\n", "\r", "\n" }, StringSplitOptions.TrimEntries)
							select s.Trim()).ToArray();
						kwl = string.Join("\n" + indentation, lines);
						tw.Write(kwl);
						NewLine(tw, level + (showBraces ? 1 : 0));
					}
					lastJsonType = val.JsonType;
					kwl = GetComments(commentedObject.Comments, key);
					bool num = jObject is LocalizationLoader.CommentedWscJsonObject commented && commented.CommentedOut.Contains(key);
					bool commentIsMultiline = false;
					if (num)
					{
						commentIsMultiline = val.GetRawString().IndexOf('\n') != -1;
						if (commentIsMultiline)
						{
							tw.Write("/* ");
						}
						else
						{
							tw.Write("// ");
						}
					}
					tw.Write(escapeName(key));
					tw.Write(':');
					WriteFancyHjsonValue(tw, val, level + (showBraces ? 1 : 0), in style, TestCommentString(kwl));
					if (num && commentIsMultiline)
					{
						tw.Write(" */");
					}
				}
				tw.Write(kwl);
				if (showBraces)
				{
					NewLine(tw, level);
				}
			}
			else
			{
				bool skipFirst2 = !showBraces;
				JsonType? lastJsonType2 = null;
				foreach (KeyValuePair<string, JsonValue> pair in (IEnumerable<KeyValuePair<string, JsonValue>>)jObject)
				{
					if (((JsonType?)pair.Value.JsonType != lastJsonType2 && lastJsonType2.HasValue) || lastJsonType2 == (JsonType?)2)
					{
						NewLine(tw, 0);
					}
					lastJsonType2 = pair.Value.JsonType;
					if (!skipFirst2)
					{
						NewLine(tw, level + 1);
					}
					else
					{
						skipFirst2 = false;
					}
					tw.Write(escapeName(pair.Key));
					tw.Write(':');
					WriteFancyHjsonValue(tw, pair.Value, level + (showBraces ? 1 : 0), in style, hasComments: false, noIndentation: true);
				}
				if (showBraces && ((JsonValue)jObject).Count > 0)
				{
					NewLine(tw, level);
				}
			}
			if (showBraces)
			{
				tw.Write('}');
			}
			break;
		}
		case 3:
		{
			int i = 0;
			int j = value.Count;
			if (!style.NoIndentaion)
			{
				if (j > 0)
				{
					NewLine(tw, level);
				}
				else
				{
					tw.Write(style.Separator);
				}
			}
			tw.Write('[');
			WscJsonArray whiteL = null;
			string wsl = null;
			if (style.WriteComments)
			{
				whiteL = (WscJsonArray)(object)((value is WscJsonArray) ? value : null);
				if (whiteL != null)
				{
					wsl = GetComments(whiteL.Comments, 0);
				}
			}
			for (; i < j; i++)
			{
				JsonValue v = value[i];
				if (whiteL != null)
				{
					tw.Write(wsl);
					wsl = GetComments(whiteL.Comments, i + 1);
				}
				NewLine(tw, level + 1);
				WriteFancyHjsonValue(tw, v, level + 1, in style, wsl != null && TestCommentString(wsl));
			}
			if (whiteL != null)
			{
				tw.Write(wsl);
			}
			if (j > 0)
			{
				NewLine(tw, level);
			}
			tw.Write(']');
			break;
		}
		case 4:
			tw.Write(style.Separator);
			tw.Write(JsonValue.op_Implicit(value) ? "true" : "false");
			break;
		case 0:
			WriteString(tw, value.GetRawString(), level, hasComments, style.Separator);
			break;
		default:
			tw.Write(style.Separator);
			tw.Write(value.GetRawString());
			break;
		}
	}

	public static string GetRawString(this JsonValue value)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Invalid comparison between Unknown and I4
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		JsonType jsonType = value.JsonType;
		if ((int)jsonType != 0)
		{
			if ((int)jsonType == 1)
			{
				return ((IFormattable)value).ToString("G", NumberFormatInfo.InvariantInfo).ToLowerInvariant();
			}
			JsonType jsonType2 = value.JsonType;
			return ((object)(JsonType)(ref jsonType2)).ToString();
		}
		return JsonValue.op_Implicit(value) ?? "";
	}

	private static void NewLine(TextWriter tw, int level)
	{
		tw.Write("\r\n");
		tw.Write(new string('\t', level));
	}

	private static void WriteString(TextWriter tw, string value, int level, bool hasComment, string separator)
	{
		if (value == "")
		{
			tw.Write(separator + "\"\"");
			return;
		}
		char left = value[0];
		char right = value[value.Length - 1];
		char left2 = ((value.Length > 1) ? value[1] : '\0');
		if (value.Length > 2)
		{
			_ = value[2];
		}
		if (hasComment || value.Any((char c) => needsQuotes(c)) || char.IsWhiteSpace(left) || char.IsWhiteSpace(right) || left == '"' || left == '\'' || left == '#' || (left == '/' && (left2 == '*' || left2 == '/')) || isPunctuatorChar(left) || tryParseNumericLiteral(value, stopAtNext: true, out var _) || startsWithKeyword(value))
		{
			if (!value.Any((char c) => needsEscape(c)))
			{
				tw.Write(separator + "\"" + value + "\"");
			}
			else if (!value.Any((char c) => needsEscapeML(c)) && !value.Contains("'''") && !value.All((char c) => char.IsWhiteSpace(c)))
			{
				WriteMultiLineString(value, tw, level, separator);
			}
			else
			{
				tw.Write(separator + "\"" + escapeString(value) + "\"");
			}
		}
		else
		{
			tw.Write(separator + value);
		}
	}

	private static void WriteMultiLineString(string value, TextWriter tw, int level, string separator)
	{
		string[] lines = value.Replace("\r", "").Split('\n');
		if (lines.Length == 1)
		{
			tw.Write(separator + "'''");
			tw.Write(lines[0]);
			tw.Write("'''");
			return;
		}
		level++;
		NewLine(tw, level);
		tw.Write("'''");
		string[] array = lines;
		foreach (string line in array)
		{
			NewLine(tw, (!string.IsNullOrEmpty(line)) ? level : 0);
			tw.Write(line);
		}
		NewLine(tw, level);
		tw.Write("'''");
	}

	private static string GetComments(Dictionary<string, string> comments, string key)
	{
		if (!comments.ContainsKey(key))
		{
			return "";
		}
		return GetComments(comments[key]);
	}

	private static string GetComments(List<string> comments, int index)
	{
		if (comments.Count <= index)
		{
			return "";
		}
		return GetComments(comments[index]);
	}

	private static string GetComments(string text)
	{
		if (string.IsNullOrEmpty(text))
		{
			return string.Empty;
		}
		for (int i = 0; i < text.Length; i++)
		{
			char c = text[i];
			if (c == '\n' || c == '#' || (c == '/' && i + 1 < text.Length && (text[i + 1] == '/' || text[i + 1] == '*')))
			{
				break;
			}
			if (c > ' ')
			{
				return " # " + text;
			}
		}
		return text;
	}

	private static bool TestCommentString(string text)
	{
		if (text.Length > 0)
		{
			return text[(text[0] == '\r' && text.Length > 1) ? 1 : 0] != '\n';
		}
		return false;
	}

	private static T GetDelegateOfMethod<T>(string type, string methodName) where T : Delegate
	{
		return typeof(HjsonValue).Assembly.GetType(type).GetMethod(methodName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).CreateDelegate<T>();
	}
}
