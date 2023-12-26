using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using log4net;
using Newtonsoft.Json;
using Terraria.Localization;
using Terraria.ModLoader.Config;
using Terraria.ModLoader.Core;
using Terraria.ModLoader.UI;
using Terraria.Net;
using Terraria.UI;

namespace Terraria.ModLoader;

public static class ModNet
{
	internal class ModHeader
	{
		public string name;

		public Version version;

		public byte[] hash;

		public bool signed;

		public string path;

		public ModHeader(string name, Version version, byte[] hash, bool signed)
		{
			this.name = name;
			this.version = version;
			this.hash = hash;
			this.signed = signed;
			path = Path.Combine(ModLoader.ModPath, name + ".tmod");
		}

		public bool Matches(TmodFile mod)
		{
			if (name == mod.Name && version == mod.Version)
			{
				return hash.SequenceEqual(mod.Hash);
			}
			return false;
		}

		public override string ToString()
		{
			return $"{name} v{version}[{string.Concat(hash[..4].Select((byte b) => b.ToString("x2")))}]";
		}
	}

	internal class NetConfig
	{
		public string modname;

		public string configname;

		public string json;

		public NetConfig(string modname, string configname, string json)
		{
			this.modname = modname;
			this.configname = configname;
			this.json = json;
		}

		public override string ToString()
		{
			return $"{modname}:{configname} {json}";
		}
	}

	internal static bool downloadModsFromServers;

	internal static bool onlyDownloadSignedMods;

	internal static bool[] isModdedClient;

	private static Mod[] netMods;

	internal static bool ShouldDrawModNetDiagnosticsUI;

	private static Queue<ModHeader> downloadQueue;

	internal static List<NetConfig> pendingConfigs;

	private static ModHeader downloadingMod;

	private static FileStream downloadingFile;

	private static long downloadingLength;

	/// <summary>
	/// Update every time a change is pushed to stable which is incompatible between server and clients. Ignored if not updated each month.
	/// </summary>
	private static Version IncompatiblePatchVersion;

	internal const int CHUNK_SIZE = 16384;

	internal static bool NetReloadActive;

	internal static bool ReadUnderflowBypass;

	public static bool DetailedLogging;

	[Obsolete("No longer supported")]
	public static bool AllowVanillaClients { get; internal set; }

	public static int NetModCount => netMods.Length;

	internal static INetDiagnosticsUI ModNetDiagnosticsUI { get; private set; }

	private static Version? StableNetVersion { get; }

	internal static string NetVersionString { get; }

	private static ILog NetLog { get; }

	public static bool IsModdedClient(int i)
	{
		return isModdedClient[i];
	}

	public static Mod GetMod(int netID)
	{
		if (netID < 0 || netID >= netMods.Length)
		{
			return null;
		}
		return netMods[netID];
	}

	static ModNet()
	{
		downloadModsFromServers = true;
		onlyDownloadSignedMods = false;
		isModdedClient = new bool[256];
		ShouldDrawModNetDiagnosticsUI = false;
		downloadQueue = new Queue<ModHeader>();
		pendingConfigs = new List<NetConfig>();
		IncompatiblePatchVersion = new Version(2022, 1, 1, 1);
		StableNetVersion = ((!BuildInfo.IsStable && !BuildInfo.IsPreview) ? null : ((IncompatiblePatchVersion.MajorMinor() == BuildInfo.tMLVersion.MajorMinor()) ? IncompatiblePatchVersion : BuildInfo.tMLVersion.MajorMinorBuild()));
		NetVersionString = BuildInfo.versionedName + ((StableNetVersion != null) ? ("!" + StableNetVersion) : "");
		ReadUnderflowBypass = false;
		DetailedLogging = Program.LaunchParameters.ContainsKey("-detailednetlog");
		NetLog = LogManager.GetLogger("Network");
		if (Main.dedServ && StableNetVersion != null)
		{
			Logging.tML.Debug((object)$"Network compatibility version is {StableNetVersion}");
		}
	}

	internal static bool IsClientCompatible(string clientVersion, out bool isModded, out string kickMsg)
	{
		kickMsg = null;
		isModded = clientVersion.StartsWith("tModLoader");
		if (AllowVanillaClients && clientVersion == "Terraria" + 279)
		{
			return true;
		}
		if (clientVersion == NetVersionString)
		{
			return true;
		}
		string[] split = clientVersion.Split('!');
		if (StableNetVersion != null && split.Length == 2 && Version.TryParse(split[1], out Version netVer) && netVer == StableNetVersion)
		{
			Logging.tML.Debug((object)("Client has " + split[0] + ", assuming net compatibility"));
			return true;
		}
		kickMsg = (isModded ? ("You are on " + split[0] + ", server is on " + BuildInfo.versionedName) : "You cannot connect to a tModLoader Server with an unmodded client");
		return false;
	}

	internal static void AssignNetIDs()
	{
		netMods = ModLoader.Mods.Where((Mod mod) => mod.Side != ModSide.Server).ToArray();
		for (short i = 0; i < netMods.Length; i++)
		{
			netMods[i].netID = i;
		}
	}

	internal static void Unload()
	{
		netMods = null;
		if (!Main.dedServ && Main.netMode != 1)
		{
			AllowVanillaClients = false;
		}
		SetModNetDiagnosticsUI(ModLoader.Mods);
	}

	internal static void SyncMods(int clientIndex)
	{
		ModPacket p = new ModPacket(251);
		p.Write(AllowVanillaClients);
		List<Mod> syncMods = ModLoader.Mods.Where((Mod mod) => mod.Side == ModSide.Both).ToList();
		AddNoSyncDeps(syncMods);
		p.Write(syncMods.Count);
		foreach (Mod mod2 in syncMods)
		{
			p.Write(mod2.Name);
			p.Write(mod2.Version.ToString());
			p.Write(mod2.File.Hash);
			p.Write(mod2.File.ValidModBrowserSignature);
			SendServerConfigs(p, mod2);
		}
		p.Send(clientIndex);
	}

	private static void AddNoSyncDeps(List<Mod> syncMods)
	{
		Queue<Mod> queue = new Queue<Mod>(syncMods.Where((Mod m) => m.Side == ModSide.Both));
		while (queue.Count > 0)
		{
			foreach (Mod dep in AssemblyManager.GetDependencies(queue.Dequeue()))
			{
				if (dep.Side == ModSide.NoSync && !syncMods.Contains(dep))
				{
					syncMods.Add(dep);
					queue.Enqueue(dep);
				}
			}
		}
	}

	private static void SendServerConfigs(ModPacket p, Mod mod)
	{
		if (!ConfigManager.Configs.TryGetValue(mod, out List<ModConfig> configs))
		{
			p.Write(0);
			return;
		}
		ModConfig[] serverConfigs = configs.Where((ModConfig x) => x.Mode == ConfigScope.ServerSide).ToArray();
		p.Write(serverConfigs.Length);
		ModConfig[] array = serverConfigs;
		foreach (ModConfig config in array)
		{
			string json = JsonConvert.SerializeObject((object)config, ConfigManager.serializerSettingsCompact);
			Logging.tML.Debug((object)$"Sending Server Config {config.Mod.Name}:{config.Name} {json}");
			p.Write(config.Name);
			p.Write(json);
		}
	}

	internal static void SyncClientMods(BinaryReader reader)
	{
		if (SyncClientMods(reader, out var needsReload))
		{
			if (downloadQueue.Count > 0)
			{
				DownloadNextMod();
			}
			else
			{
				OnModsDownloaded(needsReload);
			}
		}
	}

	internal static bool SyncClientMods(BinaryReader reader, out bool needsReload)
	{
		AllowVanillaClients = reader.ReadBoolean();
		Logging.tML.Info((object)$"Server reports AllowVanillaClients set to {AllowVanillaClients}");
		Main.statusText = Language.GetTextValue("tModLoader.MPSyncingMods");
		Mod[] clientMods = ModLoader.Mods;
		LocalMod[] modFiles = ModOrganizer.FindMods();
		needsReload = false;
		downloadQueue.Clear();
		pendingConfigs.Clear();
		List<ModHeader> syncList = new List<ModHeader>();
		HashSet<string> syncSet = new HashSet<string>();
		List<ModHeader> blockedList = new List<ModHeader>();
		int j = reader.ReadInt32();
		for (int i = 0; i < j; i++)
		{
			ModHeader header = new ModHeader(reader.ReadString(), new Version(reader.ReadString()), reader.ReadBytes(20), reader.ReadBoolean());
			syncList.Add(header);
			syncSet.Add(header.name);
			int configCount = reader.ReadInt32();
			for (int c = 0; c < configCount; c++)
			{
				pendingConfigs.Add(new NetConfig(header.name, reader.ReadString(), reader.ReadString()));
			}
			Mod clientMod = clientMods.SingleOrDefault((Mod m) => m.Name == header.name);
			if (clientMod != null && header.Matches(clientMod.File))
			{
				continue;
			}
			needsReload = true;
			LocalMod[] localVersions = modFiles.Where((LocalMod m) => m.Name == header.name).ToArray();
			LocalMod matching = Array.Find(localVersions, (LocalMod mod) => header.Matches(mod.modFile));
			if (matching != null)
			{
				matching.Enabled = true;
				continue;
			}
			if (localVersions.Length != 0)
			{
				header.path = localVersions[0].modFile.path;
			}
			if (downloadModsFromServers && (header.signed || !onlyDownloadSignedMods))
			{
				downloadQueue.Enqueue(header);
			}
			else
			{
				blockedList.Add(header);
			}
		}
		Logging.tML.Debug((object)("Server mods: " + string.Join(", ", syncList)));
		Logging.tML.Debug((object)("Download queue: " + string.Join(", ", downloadQueue)));
		if (pendingConfigs.Any())
		{
			Logging.tML.Debug((object)("Configs:\n\t\t" + string.Join("\n\t\t", pendingConfigs)));
		}
		Mod[] array = clientMods;
		foreach (Mod mod2 in array)
		{
			if (mod2.Side == ModSide.Both && !syncSet.Contains(mod2.Name))
			{
				ModLoader.DisableMod(mod2.Name);
				needsReload = true;
			}
		}
		if (blockedList.Count > 0)
		{
			string msg = Language.GetTextValue("tModLoader.MPServerModsCantDownload");
			msg += (downloadModsFromServers ? Language.GetTextValue("tModLoader.MPServerModsCantDownloadReasonSigned") : Language.GetTextValue("tModLoader.MPServerModsCantDownloadReasonAutomaticDownloadDisabled"));
			msg = msg + ".\n" + Language.GetTextValue("tModLoader.MPServerModsCantDownloadChangeSettingsHint") + "\n";
			foreach (ModHeader item in blockedList)
			{
				msg = msg + "\n    " + item;
			}
			Logging.tML.Warn((object)msg);
			Interface.errorMessage.Show(msg, 0);
			return false;
		}
		if (!needsReload)
		{
			foreach (NetConfig pendingConfig in pendingConfigs)
			{
				JsonConvert.PopulateObject(pendingConfig.json, (object)ConfigManager.GetConfig(pendingConfig), ConfigManager.serializerSettingsCompact);
			}
			if (ConfigManager.AnyModNeedsReload())
			{
				needsReload = true;
			}
			else
			{
				foreach (NetConfig pendingConfig2 in pendingConfigs)
				{
					ConfigManager.GetConfig(pendingConfig2).OnChanged();
				}
			}
		}
		return true;
	}

	private static void DownloadNextMod()
	{
		downloadingMod = downloadQueue.Dequeue();
		downloadingFile = null;
		ModPacket modPacket = new ModPacket(252);
		modPacket.Write(downloadingMod.name);
		modPacket.Send();
	}

	internal static void SendMod(string modName, int toClient)
	{
		Mod mod = ModLoader.GetMod(modName);
		if (mod.Side != ModSide.Server)
		{
			FileStream fs = File.OpenRead(mod.File.path);
			ModPacket modPacket = new ModPacket(252);
			modPacket.Write(mod.DisplayName);
			modPacket.Write(fs.Length);
			modPacket.Send(toClient);
			byte[] buf = new byte[16384];
			int count;
			while ((count = fs.Read(buf, 0, buf.Length)) > 0)
			{
				ModPacket modPacket2 = new ModPacket(252, 16387);
				modPacket2.Write(buf, 0, count);
				modPacket2.Send(toClient);
			}
			fs.Close();
		}
	}

	internal static void ReceiveMod(BinaryReader reader)
	{
		if (downloadingMod == null)
		{
			return;
		}
		try
		{
			if (downloadingFile == null)
			{
				Interface.progress.Show(reader.ReadString(), 0, CancelDownload);
				if (ModLoader.TryGetMod(downloadingMod.name, out var mod))
				{
					mod.Close();
				}
				downloadingLength = reader.ReadInt64();
				Logging.tML.Debug((object)$"Downloading: {downloadingMod.name} {downloadingLength}bytes");
				downloadingFile = new FileStream(downloadingMod.path, FileMode.Create);
				return;
			}
			byte[] bytes = reader.ReadBytes((int)Math.Min(downloadingLength - downloadingFile.Position, 16384L));
			downloadingFile.Write(bytes, 0, bytes.Length);
			Interface.progress.Progress = (float)downloadingFile.Position / (float)downloadingLength;
			if (downloadingFile.Position == downloadingLength)
			{
				downloadingFile.Close();
				TmodFile mod2 = new TmodFile(downloadingMod.path);
				using (mod2.Open())
				{
				}
				if (!downloadingMod.Matches(mod2))
				{
					throw new Exception(Language.GetTextValue("tModLoader.MPErrorModHashMismatch"));
				}
				if (downloadingMod.signed && onlyDownloadSignedMods && !mod2.ValidModBrowserSignature)
				{
					throw new Exception(Language.GetTextValue("tModLoader.MPErrorModNotSigned"));
				}
				ModLoader.EnableMod(mod2.Name);
				if (downloadQueue.Count > 0)
				{
					DownloadNextMod();
				}
				else
				{
					OnModsDownloaded(needsReload: true);
				}
			}
		}
		catch (Exception e)
		{
			try
			{
				downloadingFile?.Close();
				File.Delete(downloadingMod.path);
			}
			catch (Exception exc2)
			{
				Logging.tML.Error((object)"Unknown error during mod sync", exc2);
			}
			string msg = Language.GetTextValue("tModLoader.MPErrorModDownloadError", downloadingMod.name);
			Logging.tML.Error((object)msg, e);
			Interface.errorMessage.Show(msg + e, 0);
			Netplay.Disconnect = true;
			downloadingMod = null;
		}
	}

	private static void CancelDownload()
	{
		try
		{
			downloadingFile?.Close();
			File.Delete(downloadingMod.path);
		}
		catch
		{
		}
		downloadingMod = null;
		Netplay.Disconnect = true;
	}

	private static void OnModsDownloaded(bool needsReload)
	{
		if (needsReload)
		{
			Main.netMode = 0;
			ModLoader.OnSuccessfulLoad = NetReload();
			ModLoader.Reload();
			return;
		}
		Main.netMode = 1;
		downloadingMod = null;
		netMods = null;
		Mod[] mods = ModLoader.Mods;
		for (int i = 0; i < mods.Length; i++)
		{
			mods[i].netID = -1;
		}
		new ModPacket(251).Send();
	}

	internal static Action NetReload()
	{
		string path = Main.ActivePlayerFileData.Path;
		bool isCloudSave = Main.ActivePlayerFileData.IsCloudSave;
		NetReloadActive = true;
		return delegate
		{
			NetReloadActive = false;
			Player.GetFileData(path, isCloudSave).SetAsActive();
			Main.player[Main.myPlayer].hostile = false;
			Main.clientPlayer = Main.player[Main.myPlayer].clientClone();
			if (!Netplay.Connection.Socket.IsConnected())
			{
				Main.menuMode = 15;
				Logging.tML.Error((object)"Disconnected from server during reload.");
				Main.statusText = "Disconnected from server during reload.";
			}
			else
			{
				Main.menuMode = 10;
				Main.statusText = "Reload complete, joining...";
				OnModsDownloaded(needsReload: false);
			}
		};
	}

	internal static void SendNetIDs(int toClient)
	{
		ModPacket p = new ModPacket(250);
		p.Write(netMods.Length);
		Mod[] array = netMods;
		foreach (Mod mod in array)
		{
			p.Write(mod.Name);
		}
		p.Write(Player.MaxBuffs);
		p.Send(toClient);
	}

	private static void ReadNetIDs(BinaryReader reader)
	{
		Mod[] mods = ModLoader.Mods;
		List<Mod> list = new List<Mod>();
		int j = reader.ReadInt32();
		for (short i = 0; i < j; i++)
		{
			string name = reader.ReadString();
			Mod mod2 = mods.SingleOrDefault((Mod m) => m.Name == name);
			list.Add(mod2);
			if (mod2 != null)
			{
				mod2.netID = i;
			}
		}
		netMods = list.ToArray();
		SetModNetDiagnosticsUI(netMods.Where((Mod mod) => mod != null));
		int serverMaxBuffs = reader.ReadInt32();
		if (serverMaxBuffs != Player.MaxBuffs)
		{
			Netplay.Disconnect = true;
			Main.statusText = $"The server expects Player.MaxBuffs of {serverMaxBuffs}\nbut this client reports {Player.MaxBuffs}.\nSome mod is behaving poorly.";
		}
	}

	internal static void HandleModPacket(BinaryReader reader, int whoAmI, int length)
	{
		if (netMods == null)
		{
			ReadNetIDs(reader);
			return;
		}
		short id = ((NetModCount < 256) ? reader.ReadByte() : reader.ReadInt16());
		int start = (int)reader.BaseStream.Position;
		int actualLength = length - 1 - ((NetModCount < 256) ? 1 : 2);
		try
		{
			ReadUnderflowBypass = false;
			GetMod(id)?.HandlePacket(reader, whoAmI);
			if (!ReadUnderflowBypass && reader.BaseStream.Position - start != actualLength)
			{
				throw new IOException($"Read underflow {reader.BaseStream.Position - start} of {actualLength} bytes caused by {GetMod(id).Name} in HandlePacket");
			}
		}
		catch
		{
		}
		if (Main.netMode == 1 && id >= 0)
		{
			ModNetDiagnosticsUI.CountReadMessage(id, length);
		}
	}

	internal static void SetModNetDiagnosticsUI(IEnumerable<Mod> mods)
	{
		INetDiagnosticsUI modNetDiagnosticsUI;
		if (!Main.dedServ)
		{
			INetDiagnosticsUI netDiagnosticsUI = new UIModNetDiagnostics(mods);
			modNetDiagnosticsUI = netDiagnosticsUI;
		}
		else
		{
			INetDiagnosticsUI netDiagnosticsUI = new EmptyDiagnosticsUI();
			modNetDiagnosticsUI = netDiagnosticsUI;
		}
		ModNetDiagnosticsUI = modNetDiagnosticsUI;
	}

	internal static bool HijackGetData(ref byte messageType, ref BinaryReader reader, int playerNumber)
	{
		if (netMods == null)
		{
			return false;
		}
		return SystemLoader.HijackGetData(ref messageType, ref reader, playerNumber);
	}

	internal static bool HijackSendData(int whoAmI, int msgType, int remoteClient, int ignoreClient, NetworkText text, int number, float number2, float number3, float number4, int number5, int number6, int number7)
	{
		return SystemLoader.HijackSendData(whoAmI, msgType, remoteClient, ignoreClient, text, number, number2, number3, number4, number5, number6, number7);
	}

	private static string Identifier(int whoAmI)
	{
		if (!Main.dedServ)
		{
			return "";
		}
		if (whoAmI >= 0 && whoAmI < 256)
		{
			RemoteClient client = Netplay.Clients[whoAmI];
			return $"[{whoAmI}][{client.Socket?.GetRemoteAddress()?.GetFriendlyName()} ({client.Name})] ";
		}
		if (whoAmI == -1)
		{
			return "[*] ";
		}
		return $"[{whoAmI}] ";
	}

	private static string Identifier(RemoteAddress addr)
	{
		if (!Main.dedServ || addr == null)
		{
			return "";
		}
		RemoteClient client = Netplay.Clients.SingleOrDefault((RemoteClient c) => c.Socket?.GetRemoteAddress() == addr);
		if (client != null)
		{
			return Identifier(client.Id);
		}
		return "[" + addr.GetFriendlyName() + "] ";
	}

	public static void Log(int whoAmI, string s)
	{
		Log(Identifier(whoAmI) + s);
	}

	public static void Log(RemoteAddress addr, string s)
	{
		Log(Identifier(addr) + s);
	}

	public static void Log(string s)
	{
		NetLog.Info((object)s);
	}

	public static void Warn(int whoAmI, string s)
	{
		Warn(Identifier(whoAmI) + s);
	}

	public static void Warn(RemoteAddress addr, string s)
	{
		Warn(Identifier(addr) + s);
	}

	public static void Warn(string s)
	{
		NetLog.Warn((object)s);
	}

	public static void Debug(int whoAmI, string s)
	{
		Debug(Identifier(whoAmI) + s);
	}

	public static void Debug(RemoteAddress addr, string s)
	{
		Debug(Identifier(addr) + s);
	}

	public static void Debug(string s)
	{
		if (DetailedLogging)
		{
			NetLog.Info((object)s);
		}
	}

	public static void Error(int whoAmI, string s, Exception e = null)
	{
		Error(Identifier(whoAmI) + s, e);
	}

	public static void Error(RemoteAddress addr, string s, Exception e = null)
	{
		Error(Identifier(addr) + s, e);
	}

	public static void Error(string s, Exception e = null)
	{
		NetLog.Error((object)s, e);
	}

	public static void LogSend(int toClient, int ignoreClient, string s, int len)
	{
		if (DetailedLogging)
		{
			s += $", {len}";
			if (ignoreClient != -1)
			{
				s += $", ignore: {ignoreClient}";
			}
			Debug(toClient, s);
		}
	}
}
