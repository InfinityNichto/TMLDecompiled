using System;
using System.Collections.Generic;
using System.Linq;

namespace Terraria.ModLoader.Exceptions;

internal static class LevenshteinDistance
{
	private enum Edits
	{
		Keep,
		Delete,
		Insert,
		Substitute,
		Blank
	}

	internal static string FolderAwareEditDistance(string source, string[] targets)
	{
		if (targets.Length == 0)
		{
			return null;
		}
		char separator = '/';
		string[] sourceParts = source.Split(separator);
		List<string> sourceFolders = sourceParts.Reverse().Skip(1).ToList();
		string sourceFile = sourceParts.Last();
		int missingFolderPenalty = 4;
		int extraFolderPenalty = 3;
		var source2 = targets.Select(delegate(string target)
		{
			string[] source3 = target.Split(separator);
			List<string> targetFolders = source3.Reverse().Skip(1).ToList();
			string s = source3.Last();
			IEnumerable<string> second = sourceFolders.Where((string x) => targetFolders.Contains(x));
			List<string> list = sourceFolders.Except(second).ToList();
			List<string> list2 = targetFolders.Except(second).ToList();
			int num = 0;
			int num2 = list.Count - list2.Count;
			if (num2 > 0)
			{
				num += num2 * missingFolderPenalty;
			}
			else if (num2 < 0)
			{
				num += -num2 * extraFolderPenalty;
			}
			if (list.Count > 0 && list.Count >= list2.Count)
			{
				foreach (string current in list2)
				{
					int num3 = int.MaxValue;
					foreach (string current2 in list)
					{
						num3 = Math.Min(num3, Compute(current, current2));
					}
					num += num3;
				}
			}
			else if (list.Count > 0)
			{
				foreach (string current3 in list)
				{
					int num4 = int.MaxValue;
					foreach (string current4 in list2)
					{
						num4 = Math.Min(num4, Compute(current3, current4));
					}
					num += num4;
				}
			}
			num += Compute(s, sourceFile);
			return new
			{
				Target = target,
				Score = num
			};
		});
		source2.OrderBy(x => x.Score);
		return source2.OrderBy(x => x.Score).First().Target;
	}

	public static int Compute(string s, string t)
	{
		int n = s.Length;
		int m = t.Length;
		int[,] d = new int[n + 1, m + 1];
		if (n == 0)
		{
			return m;
		}
		if (m == 0)
		{
			return n;
		}
		int j = 0;
		while (j <= n)
		{
			d[j, 0] = j++;
		}
		int l = 0;
		while (l <= m)
		{
			d[0, l] = l++;
		}
		for (int i = 1; i <= n; i++)
		{
			for (int k = 1; k <= m; k++)
			{
				int cost = ((t[k - 1] != s[i - 1]) ? 2 : 0);
				d[i, k] = Math.Min(Math.Min(d[i - 1, k] + 2, d[i, k - 1] + 2), d[i - 1, k - 1] + cost);
			}
		}
		return d[n, m];
	}

	public static (string, string) ComputeColorTaggedString(string s, string t)
	{
		int n = s.Length;
		int m = t.Length;
		int[,] d = new int[n + 1, m + 1];
		if (n == 0)
		{
			return ("", "");
		}
		if (m == 0)
		{
			return ("", "");
		}
		int j = 0;
		while (j <= n)
		{
			d[j, 0] = j++;
		}
		int l = 0;
		while (l <= m)
		{
			d[0, l] = l++;
		}
		for (int i = 1; i <= n; i++)
		{
			for (int k = 1; k <= m; k++)
			{
				int cost2 = ((t[k - 1] != s[i - 1]) ? 1 : 0);
				d[i, k] = Math.Min(Math.Min(d[i - 1, k] + 1, d[i, k - 1] + 1), d[i - 1, k - 1] + cost2);
			}
		}
		int x = n;
		int y = m;
		Stack<(Edits, char)> editsFromStoT = new Stack<(Edits, char)>();
		Stack<(Edits, char)> editsFromTtoS = new Stack<(Edits, char)>();
		while (x != 0 || y != 0)
		{
			int cost = d[x, y];
			if (y - 1 < 0)
			{
				editsFromStoT.Push((Edits.Delete, s[x - 1]));
				editsFromTtoS.Push((Edits.Blank, ' '));
				x--;
				continue;
			}
			if (x - 1 < 0)
			{
				editsFromStoT.Push((Edits.Insert, t[y - 1]));
				editsFromTtoS.Push((Edits.Blank, ' '));
				y--;
				continue;
			}
			int costLeft = d[x, y - 1];
			int costUp = d[x - 1, y];
			int costDiagonal = d[x - 1, y - 1];
			if (costDiagonal <= costLeft && costDiagonal <= costUp && (costDiagonal == cost - 1 || costDiagonal == cost))
			{
				if (costDiagonal == cost - 1)
				{
					editsFromStoT.Push((Edits.Substitute, s[x - 1]));
					editsFromTtoS.Push((Edits.Substitute, t[y - 1]));
					x--;
					y--;
				}
				else
				{
					editsFromStoT.Push((Edits.Keep, s[x - 1]));
					editsFromTtoS.Push((Edits.Keep, t[y - 1]));
					x--;
					y--;
				}
			}
			else if (costLeft <= costDiagonal && costLeft == cost - 1)
			{
				editsFromStoT.Push((Edits.Insert, t[y - 1]));
				editsFromTtoS.Push((Edits.Blank, ' '));
				y--;
			}
			else
			{
				editsFromStoT.Push((Edits.Delete, s[x - 1]));
				editsFromTtoS.Push((Edits.Blank, ' '));
				x--;
			}
		}
		string item = FinalizeText(editsFromStoT);
		string resultB = FinalizeText(editsFromTtoS);
		return (item, resultB);
		static string FinalizeText(Stack<(Edits, char)> results)
		{
			string result = "";
			Edits editCurrent = Edits.Keep;
			while (results.Count > 0)
			{
				(Edits, char) entry = results.Pop();
				var (nextEdit, _) = entry;
				if (editCurrent != nextEdit)
				{
					if (editCurrent != 0 && editCurrent != Edits.Blank)
					{
						result += "]";
					}
					switch (nextEdit)
					{
					case Edits.Delete:
						result += "[c/ff0000:";
						break;
					case Edits.Insert:
						result += "[c/00ff00:";
						break;
					case Edits.Substitute:
						result += "[c/ffff00:";
						break;
					}
				}
				result += entry.Item2;
				editCurrent = nextEdit;
			}
			if (editCurrent != 0 && editCurrent != Edits.Blank)
			{
				result += "]";
			}
			return result;
		}
	}
}
