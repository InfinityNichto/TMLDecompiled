using System.Linq;
using System.Reflection;

namespace Terraria.ModLoader.Config.UI;

internal class SetElementWrapper<V> : ISetElementWrapper
{
	private readonly object set;

	private V _value;

	public V Value
	{
		get
		{
			return _value;
		}
		set
		{
			MethodInfo removeMethod = set.GetType().GetMethods().FirstOrDefault((MethodInfo m) => m.Name == "Remove");
			MethodInfo addMethod = set.GetType().GetMethods().FirstOrDefault((MethodInfo m) => m.Name == "Add");
			if (!(bool)set.GetType().GetMethods().FirstOrDefault((MethodInfo m) => m.Name == "Contains")
				.Invoke(set, new object[1] { value }))
			{
				removeMethod.Invoke(set, new object[1] { _value });
				_value = value;
				addMethod.Invoke(set, new object[1] { _value });
			}
		}
	}

	object ISetElementWrapper.Value => Value;

	public SetElementWrapper(V value, object set)
	{
		this.set = set;
		_value = value;
	}
}
