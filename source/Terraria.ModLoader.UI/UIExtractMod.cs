using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Terraria.Localization;
using Terraria.ModLoader.Core;

namespace Terraria.ModLoader.UI;

internal class UIExtractMod : UIProgress
{
	private const string LOG_NAME = "extract.log";

	private LocalMod mod;

	private static readonly IList<string> codeExtensions = new List<string>(ModCompile.sourceExtensions) { ".dll", ".pdb" };

	private CancellationTokenSource _cts;

	public override void OnActivate()
	{
		base.OnActivate();
		_cts = new CancellationTokenSource();
		base.OnCancel += delegate
		{
			_cts.Cancel();
		};
		Task.Run((Func<Task?>)Extract, _cts.Token);
	}

	internal void Show(LocalMod mod, int gotoMenu)
	{
		this.mod = mod;
		base.gotoMenu = gotoMenu;
		Main.menuMode = 10019;
	}

	private Task Extract()
	{
		StreamWriter log = null;
		IDisposable modHandle = null;
		try
		{
			string modReferencesPath = Path.Combine(ModCompile.ModSourcePath, "ModAssemblies");
			string oldModReferencesPath = Path.Combine(ModCompile.ModSourcePath, "Mod Libraries");
			if (Directory.Exists(oldModReferencesPath) && !Directory.Exists(modReferencesPath))
			{
				Logging.tML.Info((object)$"Migrating from \"{oldModReferencesPath}\" to \"{modReferencesPath}\"");
				Directory.Move(oldModReferencesPath, modReferencesPath);
				Logging.tML.Info((object)"Moving old ModAssemblies folder to new location migration success");
			}
			string dir = Path.Combine(Main.SavePath, "ModReader", mod.Name);
			if (Directory.Exists(dir))
			{
				Directory.Delete(dir, recursive: true);
			}
			Directory.CreateDirectory(dir);
			log = new StreamWriter(Path.Combine(dir, "extract.log"))
			{
				AutoFlush = true
			};
			if (mod.properties.hideCode)
			{
				log.WriteLine(Language.GetTextValue("tModLoader.ExtractHideCodeMessage"));
			}
			else if (!mod.properties.includeSource)
			{
				log.WriteLine(Language.GetTextValue("tModLoader.ExtractNoSourceCodeMessage"));
			}
			if (mod.properties.hideResources)
			{
				log.WriteLine(Language.GetTextValue("tModLoader.ExtractHideResourcesMessage"));
			}
			log.WriteLine(Language.GetTextValue("tModLoader.ExtractFileListing"));
			int i = 0;
			modHandle = mod.modFile.Open();
			foreach (TmodFile.FileEntry entry in mod.modFile)
			{
				_cts.Token.ThrowIfCancellationRequested();
				string name = entry.Name;
				ContentConverters.Reverse(ref name, out var converter);
				DisplayText = name;
				base.Progress = (float)i++ / (float)mod.modFile.Count;
				if (name == "extract.log")
				{
					continue;
				}
				if (codeExtensions.Contains(Path.GetExtension(name)) ? mod.properties.hideCode : mod.properties.hideResources)
				{
					log.Write("[hidden] " + name);
					continue;
				}
				log.WriteLine(name);
				string path = Path.Combine(dir, name);
				Directory.CreateDirectory(Path.GetDirectoryName(path));
				using (FileStream dst = File.OpenWrite(path))
				{
					using Stream src = mod.modFile.GetStream(entry);
					if (converter != null)
					{
						converter(src, dst);
					}
					else
					{
						src.CopyTo(dst);
					}
				}
				if (name == mod.Name + ".dll")
				{
					Directory.CreateDirectory(modReferencesPath);
					File.Copy(path, Path.Combine(modReferencesPath, $"{mod.Name}_v{mod.modFile.Version}.dll"), overwrite: true);
					log?.WriteLine("You can find this mod's .dll files under " + Path.GetFullPath(modReferencesPath) + " for easy mod collaboration!");
				}
				if (name == mod.Name + ".xml" && !mod.properties.hideCode)
				{
					Directory.CreateDirectory(modReferencesPath);
					File.Copy(path, Path.Combine(modReferencesPath, $"{mod.Name}_v{mod.modFile.Version}.xml"), overwrite: true);
					log?.WriteLine("You can find this mod's documentation .xml file under " + Path.GetFullPath(modReferencesPath) + " for easy mod collaboration!");
				}
			}
			Utils.OpenFolder(dir);
		}
		catch (OperationCanceledException)
		{
			log?.WriteLine("Extraction was cancelled.");
			return Task.FromResult(result: false);
		}
		catch (Exception e)
		{
			log?.WriteLine(e);
			Logging.tML.Error((object)Language.GetTextValue("tModLoader.ExtractErrorWhileExtractingMod", mod.Name), e);
			Main.menuMode = gotoMenu;
			return Task.FromResult(result: false);
		}
		finally
		{
			log?.Close();
			modHandle?.Dispose();
		}
		Main.menuMode = gotoMenu;
		return Task.FromResult(result: true);
	}
}
