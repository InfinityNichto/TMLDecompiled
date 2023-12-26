using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Ionic.Zlib;
using Terraria.Localization;
using Terraria.ModLoader.IO;
using Terraria.ModLoader.UI;

namespace Terraria.ModLoader.Core;

public class TmodFile : IEnumerable<TmodFile.FileEntry>, IEnumerable
{
	public class FileEntry
	{
		internal byte[] cachedBytes;

		public string Name { get; }

		public int Offset { get; internal set; }

		public int Length { get; }

		public int CompressedLength { get; }

		public bool IsCompressed => Length != CompressedLength;

		internal FileEntry(string name, int offset, int length, int compressedLength, byte[] cachedBytes = null)
		{
			Name = name;
			Offset = offset;
			Length = length;
			CompressedLength = compressedLength;
			this.cachedBytes = cachedBytes;
		}
	}

	private class DisposeWrapper : IDisposable
	{
		private readonly Action dispose;

		public DisposeWrapper(Action dispose)
		{
			this.dispose = dispose;
		}

		public void Dispose()
		{
			dispose?.Invoke();
		}
	}

	public const uint MIN_COMPRESS_SIZE = 1024u;

	public const uint MAX_CACHE_SIZE = 131072u;

	public const float COMPRESSION_TRADEOFF = 0.9f;

	public readonly string path;

	private FileStream fileStream;

	private IDictionary<string, FileEntry> files = new Dictionary<string, FileEntry>();

	private FileEntry[] fileTable;

	private int openCounter;

	private EntryReadStream sharedEntryReadStream;

	private List<EntryReadStream> independentEntryReadStreams = new List<EntryReadStream>();

	private bool? validModBrowserSignature;

	public Version TModLoaderVersion { get; private set; }

	public string Name { get; private set; }

	public Version Version { get; private set; }

	public byte[] Hash { get; private set; }

	internal byte[] Signature { get; private set; } = new byte[256];


	internal bool ValidModBrowserSignature
	{
		get
		{
			if (!validModBrowserSignature.HasValue)
			{
				validModBrowserSignature = ModLoader.IsSignedBy(this, ModLoader.modBrowserPublicKey);
			}
			return validModBrowserSignature.Value;
		}
	}

	public int Count => fileTable.Length;

	public bool IsOpen => fileStream != null;

	private static string Sanitize(string path)
	{
		return path.Replace('\\', '/');
	}

	internal TmodFile(string path, string name = null, Version version = null)
	{
		this.path = path;
		Name = name;
		Version = version;
	}

	public bool HasFile(string fileName)
	{
		return files.ContainsKey(Sanitize(fileName));
	}

	public byte[] GetBytes(FileEntry entry)
	{
		if (entry.cachedBytes != null && !entry.IsCompressed)
		{
			return entry.cachedBytes;
		}
		using Stream stream = GetStream(entry);
		return stream.ReadBytes(entry.Length);
	}

	public List<string> GetFileNames()
	{
		return files.Keys.ToList();
	}

	public byte[] GetBytes(string fileName)
	{
		if (!files.TryGetValue(Sanitize(fileName), out var entry))
		{
			return null;
		}
		return GetBytes(entry);
	}

	public Stream GetStream(FileEntry entry, bool newFileStream = false)
	{
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Expected O, but got Unknown
		Stream stream;
		if (entry.cachedBytes != null)
		{
			stream = entry.cachedBytes.ToMemoryStream();
		}
		else
		{
			if (fileStream == null)
			{
				throw new IOException("File not open: " + path);
			}
			if (newFileStream)
			{
				EntryReadStream ers = new EntryReadStream(this, entry, File.OpenRead(path), leaveOpen: false);
				lock (independentEntryReadStreams)
				{
					independentEntryReadStreams.Add(ers);
				}
				stream = ers;
			}
			else
			{
				if (sharedEntryReadStream != null)
				{
					throw new IOException("Previous entry read stream not closed: " + sharedEntryReadStream.Name);
				}
				stream = (sharedEntryReadStream = new EntryReadStream(this, entry, fileStream, leaveOpen: true));
			}
		}
		if (entry.IsCompressed)
		{
			stream = (Stream)new DeflateStream(stream, (CompressionMode)1);
		}
		return stream;
	}

	internal void OnStreamClosed(EntryReadStream stream)
	{
		if (stream == sharedEntryReadStream)
		{
			sharedEntryReadStream = null;
			return;
		}
		lock (independentEntryReadStreams)
		{
			if (!independentEntryReadStreams.Remove(stream))
			{
				throw new IOException("Closed EntryReadStream not associated with this file. " + stream.Name + " @ " + path);
			}
		}
	}

	public Stream GetStream(string fileName, bool newFileStream = false)
	{
		if (!files.TryGetValue(Sanitize(fileName), out var entry))
		{
			throw new KeyNotFoundException(fileName);
		}
		return GetStream(entry, newFileStream);
	}

	/// <summary>
	/// Adds a (fileName -&gt; content) entry to the compressed payload
	/// This method is not threadsafe with reads, but is threadsafe with multiple concurrent AddFile calls
	/// </summary>
	/// <param name="fileName">The internal filepath, will be slash sanitised automatically</param>
	/// <param name="data">The file content to add. WARNING, data is kept as a shallow copy, so modifications to the passed byte array will affect file content</param>
	internal void AddFile(string fileName, byte[] data)
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Expected O, but got Unknown
		fileName = Sanitize(fileName);
		int size = data.Length;
		if ((long)size > 1024L && ShouldCompress(fileName))
		{
			using MemoryStream ms = new MemoryStream(data.Length);
			DeflateStream ds = new DeflateStream((Stream)ms, (CompressionMode)0);
			try
			{
				((Stream)(object)ds).Write(data, 0, data.Length);
			}
			finally
			{
				((IDisposable)ds)?.Dispose();
			}
			byte[] compressed = ms.ToArray();
			if ((float)compressed.Length < (float)size * 0.9f)
			{
				data = compressed;
			}
		}
		lock (files)
		{
			files[fileName] = new FileEntry(fileName, -1, size, data.Length, data);
		}
		fileTable = null;
	}

	internal void RemoveFile(string fileName)
	{
		files.Remove(Sanitize(fileName));
		fileTable = null;
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	public IEnumerator<FileEntry> GetEnumerator()
	{
		FileEntry[] array = fileTable;
		for (int i = 0; i < array.Length; i++)
		{
			yield return array[i];
		}
	}

	internal void Save()
	{
		if (this.fileStream != null)
		{
			throw new IOException("File already open: " + path);
		}
		Directory.CreateDirectory(Path.GetDirectoryName(path));
		using (this.fileStream = File.Create(path))
		{
			using BinaryWriter writer = new BinaryWriter(this.fileStream);
			writer.Write(Encoding.ASCII.GetBytes("TMOD"));
			Version version = (TModLoaderVersion = BuildInfo.tMLVersion);
			writer.Write(version.ToString());
			int hashPos = (int)this.fileStream.Position;
			writer.Write(new byte[280]);
			int dataPos = (int)this.fileStream.Position;
			writer.Write(Name);
			writer.Write(Version.ToString());
			fileTable = files.Values.ToArray();
			writer.Write(fileTable.Length);
			FileEntry[] array = fileTable;
			foreach (FileEntry f2 in array)
			{
				if (f2.CompressedLength != f2.cachedBytes.Length)
				{
					throw new Exception($"CompressedLength ({f2.CompressedLength}) != cachedBytes.Length ({f2.cachedBytes.Length}): {f2.Name}");
				}
				writer.Write(f2.Name);
				writer.Write(f2.Length);
				writer.Write(f2.CompressedLength);
			}
			int offset = (int)this.fileStream.Position;
			array = fileTable;
			foreach (FileEntry f in array)
			{
				writer.Write(f.cachedBytes);
				f.Offset = offset;
				offset += f.CompressedLength;
			}
			this.fileStream.Position = dataPos;
			Hash = SHA1.Create().ComputeHash(this.fileStream);
			this.fileStream.Position = hashPos;
			writer.Write(Hash);
			this.fileStream.Seek(256L, SeekOrigin.Current);
			writer.Write((int)(this.fileStream.Length - dataPos));
		}
		this.fileStream = null;
	}

	public IDisposable Open()
	{
		if (openCounter++ == 0)
		{
			if (fileStream != null)
			{
				throw new Exception("File already opened? " + path);
			}
			try
			{
				if (Name == null)
				{
					Read();
				}
				else
				{
					Reopen();
				}
			}
			catch
			{
				try
				{
					Close();
				}
				catch
				{
				}
				throw;
			}
		}
		return new DisposeWrapper(Close);
	}

	private void Close()
	{
		if (openCounter == 0 || --openCounter != 0)
		{
			return;
		}
		if (sharedEntryReadStream != null)
		{
			throw new IOException("Previous entry read stream not closed: " + sharedEntryReadStream.Name);
		}
		if (independentEntryReadStreams.Count != 0)
		{
			throw new IOException("Shared entry read streams not closed: " + string.Join(", ", independentEntryReadStreams.Select((EntryReadStream e) => e.Name)));
		}
		fileStream?.Close();
		fileStream = null;
	}

	private static bool ShouldCompress(string fileName)
	{
		if (!fileName.EndsWith(".png") && !fileName.EndsWith(".mp3"))
		{
			return !fileName.EndsWith(".ogg");
		}
		return false;
	}

	private void Read()
	{
		fileStream = File.OpenRead(path);
		BinaryReader reader = new BinaryReader(fileStream);
		if (Encoding.ASCII.GetString(reader.ReadBytes(4)) != "TMOD")
		{
			throw new Exception("Magic Header != \"TMOD\"");
		}
		TModLoaderVersion = new Version(reader.ReadString());
		Hash = reader.ReadBytes(20);
		Signature = reader.ReadBytes(256);
		reader.ReadInt32();
		long pos = fileStream.Position;
		if (!SHA1.Create().ComputeHash(fileStream).SequenceEqual(Hash))
		{
			throw new Exception(Language.GetTextValue("tModLoader.LoadErrorHashMismatchCorrupted"));
		}
		fileStream.Position = pos;
		if (TModLoaderVersion < new Version(0, 11))
		{
			Upgrade();
			return;
		}
		Name = reader.ReadString();
		Version = new Version(reader.ReadString());
		int offset = 0;
		fileTable = new FileEntry[reader.ReadInt32()];
		for (int i = 0; i < fileTable.Length; i++)
		{
			FileEntry f = new FileEntry(reader.ReadString(), offset, reader.ReadInt32(), reader.ReadInt32());
			fileTable[i] = f;
			files[f.Name] = f;
			offset += f.CompressedLength;
		}
		int fileStartPos = (int)fileStream.Position;
		FileEntry[] array = fileTable;
		for (int j = 0; j < array.Length; j++)
		{
			array[j].Offset += fileStartPos;
		}
	}

	private void Reopen()
	{
		fileStream = File.OpenRead(path);
		BinaryReader reader = new BinaryReader(fileStream);
		if (Encoding.ASCII.GetString(reader.ReadBytes(4)) != "TMOD")
		{
			throw new Exception("Magic Header != \"TMOD\"");
		}
		reader.ReadString();
		if (!reader.ReadBytes(20).SequenceEqual(Hash))
		{
			throw new Exception("File has been modified, hash. " + path);
		}
	}

	public void CacheFiles(ISet<string> skip = null)
	{
		fileStream.Seek(fileTable[0].Offset, SeekOrigin.Begin);
		FileEntry[] array = fileTable;
		foreach (FileEntry f in array)
		{
			if ((long)f.CompressedLength > 131072L || (skip != null && skip.Contains(f.Name)))
			{
				fileStream.Seek(f.CompressedLength, SeekOrigin.Current);
			}
			else
			{
				f.cachedBytes = fileStream.ReadBytes(f.CompressedLength);
			}
		}
	}

	public void RemoveFromCache(IEnumerable<string> fileNames)
	{
		foreach (string fileName in fileNames)
		{
			files[fileName].cachedBytes = null;
		}
	}

	public void ResetCache()
	{
		FileEntry[] array = fileTable;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].cachedBytes = null;
		}
	}

	private void Upgrade()
	{
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Expected O, but got Unknown
		Interface.loadMods.SubProgressText = "Upgrading: " + Path.GetFileName(this.path);
		Logging.tML.InfoFormat("Upgrading: {0}", (object)Path.GetFileName(this.path));
		DeflateStream deflateStream = new DeflateStream((Stream)fileStream, (CompressionMode)1, true);
		try
		{
			using BinaryReader reader = new BinaryReader((Stream)(object)deflateStream);
			Name = reader.ReadString();
			Version = new Version(reader.ReadString());
			int count = reader.ReadInt32();
			for (int i = 0; i < count; i++)
			{
				AddFile(reader.ReadString(), reader.ReadBytes(reader.ReadInt32()));
			}
		}
		finally
		{
			((IDisposable)deflateStream)?.Dispose();
		}
		BuildProperties info = BuildProperties.ReadModFile(this);
		info.buildVersion = TModLoaderVersion;
		AddFile("Info", info.ToBytes());
		fileStream.Seek(0L, SeekOrigin.Begin);
		string path = Path.Combine(Path.GetDirectoryName(this.path), "UpgradeBackup");
		Directory.CreateDirectory(path);
		using (FileStream backupStream = File.OpenWrite(Path.Combine(path, Path.GetFileName(this.path))))
		{
			fileStream.CopyTo(backupStream);
		}
		Close();
		Save();
		ResetCache();
		Open();
	}
}
