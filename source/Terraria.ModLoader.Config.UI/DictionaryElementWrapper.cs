using System.Collections;

namespace Terraria.ModLoader.Config.UI;

internal class DictionaryElementWrapper<K, V> : IDictionaryElementWrapper
{
	private readonly IDictionary dictionary;

	private K _key;

	private V _value;

	public K Key
	{
		get
		{
			return _key;
		}
		set
		{
			if (!dictionary.Contains(value))
			{
				dictionary.Remove(_key);
				_key = value;
				dictionary.Add(_key, _value);
			}
		}
	}

	public V Value
	{
		get
		{
			return _value;
		}
		set
		{
			dictionary[Key] = value;
			_value = value;
		}
	}

	object IDictionaryElementWrapper.Key => Key;

	object IDictionaryElementWrapper.Value => Value;

	public DictionaryElementWrapper(K key, V value, IDictionary dictionary)
	{
		this.dictionary = dictionary;
		_key = key;
		_value = value;
	}
}
