using System;
using System.IO;
using System.Text;

namespace Terraria.Localization;

/// <summary>
/// Represents text that will be sent over the network in multiplayer and displayed to the receiving user in their selected language. <para />
/// Use <see cref="M:Terraria.Localization.NetworkText.FromKey(System.String,System.Object[])" /> to send a localization key and optional substitutions. <see cref="M:Terraria.Localization.LocalizedText.ToNetworkText" /> can be used directly as well for the same effect.<para />
/// Use <see cref="M:Terraria.Localization.NetworkText.FromFormattable(System.String,System.Object[])" /> to send a string with string formatting substitutions and associated substitutions. This is typically used with language-agnostic strings that don't need a localization entry, such as "{0} - {1}".<para />
/// Use <see cref="M:Terraria.Localization.NetworkText.FromLiteral(System.String)" /> to send a string directly. This should be used to send text that can't be localized.
/// </summary>
public class NetworkText
{
	private enum Mode : byte
	{
		Literal,
		Formattable,
		LocalizationKey
	}

	public static readonly NetworkText Empty = FromLiteral("");

	private NetworkText[] _substitutions;

	private string _text;

	private Mode _mode;

	private NetworkText(string text, Mode mode)
	{
		_text = text;
		_mode = mode;
	}

	private static NetworkText[] ConvertSubstitutionsToNetworkText(object[] substitutions)
	{
		NetworkText[] array = new NetworkText[substitutions.Length];
		for (int i = 0; i < substitutions.Length; i++)
		{
			array[i] = From(substitutions[i]);
		}
		return array;
	}

	/// <summary>
	/// Creates a NetworkText object from a string with string formatting substitutions and associated substitutions. This is typically used with language-agnostic strings that don't need a localization entry, such as "{0} - {1}".
	/// </summary>
	/// <param name="text"></param>
	/// <param name="substitutions"></param>
	/// <returns></returns>
	public static NetworkText FromFormattable(string text, params object[] substitutions)
	{
		return new NetworkText(text, Mode.Formattable)
		{
			_substitutions = ConvertSubstitutionsToNetworkText(substitutions)
		};
	}

	/// <summary>
	/// Creates a NetworkText object from a string. Use this for text that can't be localized.
	/// </summary>
	/// <param name="text"></param>
	/// <returns></returns>
	public static NetworkText FromLiteral(string text)
	{
		return new NetworkText(text, Mode.Literal);
	}

	/// <summary>
	/// Creates a NetworkText object from a localization key and optional substitutions. The receiving client will see the resulting text in their selected language.
	/// </summary>
	/// <param name="key"></param>
	/// <param name="substitutions"></param>
	/// <returns></returns>
	public static NetworkText FromKey(string key, params object[] substitutions)
	{
		return new NetworkText(key, Mode.LocalizationKey)
		{
			_substitutions = ConvertSubstitutionsToNetworkText(substitutions)
		};
	}

	public static NetworkText From(object o)
	{
		if (!(o is NetworkText networkText))
		{
			if (o is LocalizedText localizedText)
			{
				return localizedText.ToNetworkText();
			}
			return FromLiteral(o.ToString());
		}
		return networkText;
	}

	public int GetMaxSerializedSize()
	{
		int num = 0;
		num++;
		num += 4 + Encoding.UTF8.GetByteCount(_text);
		if (_mode != 0)
		{
			num++;
			for (int i = 0; i < _substitutions.Length; i++)
			{
				num += _substitutions[i].GetMaxSerializedSize();
			}
		}
		return num;
	}

	public void Serialize(BinaryWriter writer)
	{
		writer.Write((byte)_mode);
		writer.Write(_text);
		SerializeSubstitutionList(writer);
	}

	private void SerializeSubstitutionList(BinaryWriter writer)
	{
		if (_mode != 0)
		{
			writer.Write((byte)_substitutions.Length);
			for (int i = 0; i < (_substitutions.Length & 0xFF); i++)
			{
				_substitutions[i].Serialize(writer);
			}
		}
	}

	public static NetworkText Deserialize(BinaryReader reader)
	{
		Mode mode = (Mode)reader.ReadByte();
		NetworkText networkText = new NetworkText(reader.ReadString(), mode);
		networkText.DeserializeSubstitutionList(reader);
		return networkText;
	}

	public static NetworkText DeserializeLiteral(BinaryReader reader)
	{
		Mode mode = (Mode)reader.ReadByte();
		NetworkText networkText = new NetworkText(reader.ReadString(), mode);
		networkText.DeserializeSubstitutionList(reader);
		if (mode != 0)
		{
			networkText.SetToEmptyLiteral();
		}
		return networkText;
	}

	private void DeserializeSubstitutionList(BinaryReader reader)
	{
		if (_mode != 0)
		{
			_substitutions = new NetworkText[reader.ReadByte()];
			for (int i = 0; i < _substitutions.Length; i++)
			{
				_substitutions[i] = Deserialize(reader);
			}
		}
	}

	private void SetToEmptyLiteral()
	{
		_mode = Mode.Literal;
		_text = string.Empty;
		_substitutions = null;
	}

	public override string ToString()
	{
		try
		{
			switch (_mode)
			{
			case Mode.Literal:
				return _text;
			case Mode.Formattable:
			{
				string text2 = _text;
				object[] substitutions3 = _substitutions;
				object[] substitutions = substitutions3;
				return string.Format(text2, substitutions);
			}
			case Mode.LocalizationKey:
			{
				string text = _text;
				object[] substitutions3 = _substitutions;
				object[] substitutions2 = substitutions3;
				return Language.GetTextValue(text, substitutions2);
			}
			default:
				return _text;
			}
		}
		catch (Exception ex)
		{
			string.Concat(string.Concat("NetworkText.ToString() threw an exception.\n" + ToDebugInfoString(), "\n"), "Exception: ", ex);
			SetToEmptyLiteral();
		}
		return _text;
	}

	private string ToDebugInfoString(string linePrefix = "")
	{
		string text = string.Format("{0}Mode: {1}\n{0}Text: {2}\n", linePrefix, _mode, _text);
		if (_mode == Mode.LocalizationKey)
		{
			text = text + linePrefix + "Localized Text: " + Language.GetTextValue(_text) + "\n";
		}
		if (_mode != 0)
		{
			for (int i = 0; i < _substitutions.Length; i++)
			{
				text += $"{linePrefix}Substitution {i}:\n";
				text += _substitutions[i].ToDebugInfoString(linePrefix + "\t");
			}
		}
		return text;
	}
}
