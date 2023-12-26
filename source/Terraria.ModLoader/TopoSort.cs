using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Terraria.ModLoader;

public class TopoSort<T>
{
	public class SortingException : Exception
	{
		public ISet<T> set = new HashSet<T>();

		public IList<List<T>> cycles = new List<List<T>>();

		public override string Message => string.Join(Environment.NewLine, cycles.Select(CycleToString));

		private string CycleToString(List<T> cycle)
		{
			return "Dependency Cycle: " + string.Join(" -> ", cycle);
		}

		public void Add(List<T> cycle)
		{
			cycles.Add(cycle);
			foreach (T e in cycle)
			{
				set.Add(e);
			}
		}
	}

	public readonly ReadOnlyCollection<T> list;

	private IDictionary<T, List<T>> dependencyDict = new Dictionary<T, List<T>>();

	private IDictionary<T, List<T>> dependentDict = new Dictionary<T, List<T>>();

	public TopoSort(IEnumerable<T> elements, Func<T, IEnumerable<T>> dependencies = null, Func<T, IEnumerable<T>> dependents = null)
	{
		list = elements.ToList().AsReadOnly();
		if (dependencies != null)
		{
			foreach (T t2 in list)
			{
				foreach (T dependency in dependencies(t2))
				{
					AddEntry(dependency, t2);
				}
			}
		}
		if (dependents == null)
		{
			return;
		}
		foreach (T t in list)
		{
			foreach (T dependent in dependents(t))
			{
				AddEntry(t, dependent);
			}
		}
	}

	public void AddEntry(T dependency, T dependent)
	{
		if (!dependencyDict.TryGetValue(dependent, out var list))
		{
			list = (dependencyDict[dependent] = new List<T>());
		}
		list.Add(dependency);
		if (!dependentDict.TryGetValue(dependency, out list))
		{
			list = (dependentDict[dependency] = new List<T>());
		}
		list.Add(dependent);
	}

	private static void BuildSet(T t, IDictionary<T, List<T>> dict, ISet<T> set)
	{
		if (!dict.TryGetValue(t, out var _))
		{
			return;
		}
		foreach (T entry in dict[t])
		{
			if (set.Add(entry))
			{
				BuildSet(entry, dict, set);
			}
		}
	}

	public List<T> Dependencies(T t)
	{
		if (!dependencyDict.TryGetValue(t, out var list))
		{
			return new List<T>();
		}
		return list;
	}

	public List<T> Dependents(T t)
	{
		if (!dependentDict.TryGetValue(t, out var list))
		{
			return new List<T>();
		}
		return list;
	}

	public ISet<T> AllDependencies(T t)
	{
		HashSet<T> set = new HashSet<T>();
		BuildSet(t, dependencyDict, set);
		return set;
	}

	public ISet<T> AllDependendents(T t)
	{
		HashSet<T> set = new HashSet<T>();
		BuildSet(t, dependentDict, set);
		return set;
	}

	public List<T> Sort()
	{
		SortingException ex = new SortingException();
		Stack<T> visiting = new Stack<T>();
		List<T> sorted = new List<T>();
		Action<T> Visit = null;
		Visit = delegate(T t)
		{
			if (!sorted.Contains(t) && !ex.set.Contains(t))
			{
				visiting.Push(t);
				foreach (T dependency in Dependencies(t))
				{
					if (visiting.Contains(dependency))
					{
						List<T> list = new List<T>();
						list.Add(dependency);
						list.AddRange(visiting.TakeWhile((T entry) => !EqualityComparer<T>.Default.Equals(entry, dependency)));
						list.Add(dependency);
						list.Reverse();
						ex.Add(list);
					}
					else
					{
						Visit(dependency);
					}
				}
				visiting.Pop();
				sorted.Add(t);
			}
		};
		foreach (T t2 in list)
		{
			Visit(t2);
		}
		if (ex.set.Count > 0)
		{
			throw ex;
		}
		return sorted;
	}
}
