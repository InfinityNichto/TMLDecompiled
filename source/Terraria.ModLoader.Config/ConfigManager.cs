using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Terraria.Localization;
using Terraria.ModLoader.Config.UI;
using Terraria.ModLoader.Core;
using Terraria.ModLoader.Exceptions;
using Terraria.ModLoader.UI;
using Terraria.UI;

namespace Terraria.ModLoader.Config;

public static class ConfigManager
{
	internal static readonly IDictionary<Mod, List<ModConfig>> Configs;

	private static readonly IDictionary<Mod, List<ModConfig>> loadTimeConfigs;

	public static readonly JsonSerializerSettings serializerSettings;

	internal static readonly JsonSerializerSettings serializerSettingsCompact;

	private static readonly HashSet<Type> typesWithLocalizationRegistered;

	public static readonly string ModConfigPath;

	public static readonly string ServerModConfigPath;

	static ConfigManager()
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Expected O, but got Unknown
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Expected O, but got Unknown
		Configs = new Dictionary<Mod, List<ModConfig>>();
		loadTimeConfigs = new Dictionary<Mod, List<ModConfig>>();
		serializerSettings = new JsonSerializerSettings
		{
			Formatting = (Formatting)1,
			DefaultValueHandling = (DefaultValueHandling)3,
			ObjectCreationHandling = (ObjectCreationHandling)2,
			NullValueHandling = (NullValueHandling)1,
			ContractResolver = (IContractResolver)(object)new ReferenceDefaultsPreservingResolver()
		};
		serializerSettingsCompact = new JsonSerializerSettings
		{
			Formatting = (Formatting)0,
			DefaultValueHandling = (DefaultValueHandling)3,
			ObjectCreationHandling = (ObjectCreationHandling)2,
			NullValueHandling = (NullValueHandling)1,
			ContractResolver = serializerSettings.ContractResolver
		};
		typesWithLocalizationRegistered = new HashSet<Type>();
		ModConfigPath = Path.Combine(Main.SavePath, "ModConfigs");
		ServerModConfigPath = Path.Combine(Main.SavePath, "ModConfigs", "Server");
		TypeCaching.OnClear += delegate
		{
			typesWithLocalizationRegistered.Clear();
		};
	}

	internal static void Add(ModConfig config)
	{
		Load(config);
		if (!Configs.TryGetValue(config.Mod, out List<ModConfig> configList))
		{
			configList = (Configs[config.Mod] = new List<ModConfig>());
		}
		configList.Add(config);
		config.GetType().GetField("Instance", BindingFlags.Static | BindingFlags.Public)?.SetValue(null, config);
		config.OnLoaded();
		config.OnChanged();
		if (!loadTimeConfigs.TryGetValue(config.Mod, out List<ModConfig> loadTimeConfigList))
		{
			loadTimeConfigList = (loadTimeConfigs[config.Mod] = new List<ModConfig>());
		}
		loadTimeConfigList.Add(GeneratePopulatedClone(config));
	}

	internal static void FinishSetup()
	{
		foreach (KeyValuePair<Mod, List<ModConfig>> config2 in Configs)
		{
			foreach (ModConfig config in config2.Value)
			{
				try
				{
					_ = config.DisplayName;
					RegisterLocalizationKeysForMembers(config.GetType());
				}
				catch (Exception ex)
				{
					ex.Data["mod"] = config.Mod.Name;
					throw;
				}
			}
		}
	}

	private static void RegisterLocalizationKeysForMembers(Type type)
	{
		AssemblyManager.GetAssemblyOwner(type.Assembly, out var _);
		foreach (PropertyFieldWrapper variable in GetFieldsAndProperties(type))
		{
			LabelAttribute labelObsolete = GetLegacyLabelAttribute(variable.MemberInfo);
			TooltipAttribute tooltipObsolete = GetLegacyTooltipAttribute(variable.MemberInfo);
			if (Attribute.IsDefined(variable.MemberInfo, typeof(JsonIgnoreAttribute)) && labelObsolete == null && !Attribute.IsDefined(variable.MemberInfo, typeof(ShowDespiteJsonIgnoreAttribute)))
			{
				continue;
			}
			RegisterLocalizationKeysForMemberType(variable.Type, type.Assembly);
			HeaderAttribute header = GetLocalizedHeader(variable.MemberInfo);
			if (header != null)
			{
				string identifier = (header.IsIdentifier ? header.identifier : variable.Name);
				Language.GetOrRegister(header.key, () => Regex.Replace(identifier, "([A-Z])", " $1").Trim() + " Header");
			}
			Language.GetOrRegister(GetConfigKey<LabelKeyAttribute>(variable.MemberInfo, "Label"), () => labelObsolete?.LocalizationEntry ?? Regex.Replace(variable.Name, "([A-Z])", " $1").Trim());
			Language.GetOrRegister(GetConfigKey<TooltipKeyAttribute>(variable.MemberInfo, "Tooltip"), () => tooltipObsolete?.LocalizationEntry ?? "");
		}
	}

	private static void RegisterLocalizationKeysForEnumMembers(Type type)
	{
		FieldInfo[] fields = type.GetFields(BindingFlags.Static | BindingFlags.Public);
		foreach (FieldInfo field in fields)
		{
			LabelAttribute labelObsolete = GetLegacyLabelAttribute(field);
			Language.GetOrRegister(GetConfigKey<LabelKeyAttribute>(field, "Label"), () => labelObsolete?.LocalizationEntry ?? Regex.Replace(field.Name, "([A-Z])", " $1").Trim());
		}
	}

	private static void RegisterLocalizationKeysForMemberType(Type type, Assembly owningAssembly)
	{
		if (type.IsGenericType)
		{
			Type[] genericArguments = type.GetGenericArguments();
			for (int i = 0; i < genericArguments.Length; i++)
			{
				RegisterLocalizationKeysForMemberType(genericArguments[i], owningAssembly);
			}
		}
		if ((type.IsClass || type.IsEnum) && type.Assembly == owningAssembly && typesWithLocalizationRegistered.Add(type))
		{
			Language.GetOrRegister(GetConfigKey<TooltipKeyAttribute>(type, "Tooltip"), () => "");
			if (type.IsClass)
			{
				RegisterLocalizationKeysForMembers(type);
			}
			else
			{
				RegisterLocalizationKeysForEnumMembers(type);
			}
		}
	}

	internal static LabelAttribute? GetLegacyLabelAttribute(MemberInfo memberInfo)
	{
		return (LabelAttribute)Attribute.GetCustomAttribute(memberInfo, typeof(LabelAttribute));
	}

	internal static TooltipAttribute? GetLegacyTooltipAttribute(MemberInfo memberInfo)
	{
		return (TooltipAttribute)Attribute.GetCustomAttribute(memberInfo, typeof(TooltipAttribute));
	}

	internal static void LoadAll()
	{
		foreach (KeyValuePair<Mod, List<ModConfig>> config in Configs)
		{
			foreach (ModConfig item in config.Value)
			{
				Load(item);
			}
		}
	}

	internal static void OnChangedAll()
	{
		foreach (KeyValuePair<Mod, List<ModConfig>> config in Configs)
		{
			foreach (ModConfig item in config.Value)
			{
				item.OnChanged();
			}
		}
	}

	internal static void Load(ModConfig config)
	{
		ModConfig config2 = config;
		string filename = config2.Mod.Name + "_" + config2.Name + ".json";
		string path = Path.Combine(ModConfigPath, filename);
		if (config2.Mode == ConfigScope.ServerSide && ModNet.NetReloadActive)
		{
			JsonConvert.PopulateObject(ModNet.pendingConfigs.Single((ModNet.NetConfig x) => x.modname == config2.Mod.Name && x.configname == config2.Name).json, (object)config2, serializerSettingsCompact);
			return;
		}
		bool jsonFileExists = File.Exists(path);
		string json = (jsonFileExists ? File.ReadAllText(path) : "{}");
		try
		{
			JsonConvert.PopulateObject(json, (object)config2, serializerSettings);
		}
		catch (Exception ex) when (jsonFileExists && (ex is JsonReaderException || ex is JsonSerializationException))
		{
			Logging.tML.Warn((object)$"Then config file {config2.Name} from the mod {config2.Mod.Name} located at {path} failed to load. The file was likely corrupted somehow, so the defaults will be loaded and the file deleted.");
			File.Delete(path);
			JsonConvert.PopulateObject("{}", (object)config2, serializerSettings);
		}
	}

	internal static void Reset(ModConfig pendingConfig)
	{
		JsonConvert.PopulateObject("{}", (object)pendingConfig, serializerSettings);
	}

	internal static void Save(ModConfig config)
	{
		Directory.CreateDirectory(ModConfigPath);
		string filename = config.Mod.Name + "_" + config.Name + ".json";
		string path = Path.Combine(ModConfigPath, filename);
		string json = JsonConvert.SerializeObject((object)config, serializerSettings);
		File.WriteAllText(path, json);
	}

	internal static void Unload()
	{
		serializerSettings.ContractResolver = (IContractResolver)(object)new ReferenceDefaultsPreservingResolver();
		serializerSettingsCompact.ContractResolver = serializerSettings.ContractResolver;
		Configs.SelectMany<KeyValuePair<Mod, List<ModConfig>>, ModConfig>((KeyValuePair<Mod, List<ModConfig>> configList) => configList.Value).ToList().ForEach(delegate(ModConfig config)
		{
			FieldInfo field = config.GetType().GetField("Instance", BindingFlags.Static | BindingFlags.Public);
			if (field != null)
			{
				field.SetValue(null, null);
			}
		});
		Configs.Clear();
		loadTimeConfigs.Clear();
		Interface.modConfig.Unload();
		Interface.modConfigList.Unload();
	}

	internal static bool AnyModNeedsReload()
	{
		return ModLoader.Mods.Any(ModNeedsReload);
	}

	internal static bool ModNeedsReload(Mod mod)
	{
		if (Configs.ContainsKey(mod))
		{
			List<ModConfig> configs = Configs[mod];
			List<ModConfig> loadTimeConfigs = ConfigManager.loadTimeConfigs[mod];
			for (int i = 0; i < configs.Count; i++)
			{
				if (loadTimeConfigs[i].NeedsReload(configs[i]))
				{
					return true;
				}
			}
		}
		return false;
	}

	internal static ModConfig GetConfig(ModNet.NetConfig netConfig)
	{
		return GetConfig(ModLoader.GetMod(netConfig.modname), netConfig.configname);
	}

	internal static ModConfig GetConfig(Mod mod, string config)
	{
		string config2 = config;
		if (Configs.TryGetValue(mod, out List<ModConfig> configs))
		{
			return configs.Single((ModConfig x) => x.Name == config2);
		}
		throw new MissingResourceException("Missing config named " + config2 + " in mod " + mod.Name);
	}

	internal static ModConfig GetLoadTimeConfig(Mod mod, string config)
	{
		string config2 = config;
		if (loadTimeConfigs.TryGetValue(mod, out List<ModConfig> configs))
		{
			return configs.Single((ModConfig x) => x.Name == config2);
		}
		throw new MissingResourceException("Missing config named " + config2 + " in mod " + mod.Name);
	}

	internal static void HandleInGameChangeConfigPacket(BinaryReader reader, int whoAmI)
	{
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		if (Main.netMode == 1)
		{
			bool num = reader.ReadBoolean();
			NetworkText message = NetworkText.Deserialize(reader);
			if (num)
			{
				string modname = reader.ReadString();
				string configname = reader.ReadString();
				string text = reader.ReadString();
				ModConfig activeConfig = GetConfig(ModLoader.GetMod(modname), configname);
				JsonConvert.PopulateObject(text, (object)activeConfig, serializerSettingsCompact);
				activeConfig.OnChanged();
				Main.NewText(Language.GetTextValue("tModLoader.ModConfigSharedConfigChanged", message, modname, configname));
				if (Main.InGameUI.CurrentState == Interface.modConfig)
				{
					Main.InGameUI.SetState(Interface.modConfig);
					Interface.modConfig.SetMessage(Language.GetTextValue("tModLoader.ModConfigServerResponse", message), Color.Green);
				}
			}
			else
			{
				Main.NewText(Language.GetTextValue("tModLoader.ModConfigServerRejectedChanges", message));
				if (Main.InGameUI.CurrentState == Interface.modConfig)
				{
					Interface.modConfig.SetMessage(Language.GetTextValue("tModLoader.ModConfigServerRejectedChanges", message), Color.Red);
				}
			}
			return;
		}
		string modname2 = reader.ReadString();
		string configname2 = reader.ReadString();
		string json = reader.ReadString();
		Mod mod = ModLoader.GetMod(modname2);
		ModConfig config = GetConfig(mod, configname2);
		ModConfig loadTimeConfig = GetLoadTimeConfig(mod, configname2);
		ModConfig pendingConfig = GeneratePopulatedClone(config);
		JsonConvert.PopulateObject(json, (object)pendingConfig, serializerSettingsCompact);
		bool success = true;
		NetworkText message2 = NetworkText.FromKey("tModLoader.ModConfigAccepted");
		if (loadTimeConfig.NeedsReload(pendingConfig))
		{
			success = false;
			message2 = NetworkText.FromKey("tModLoader.ModConfigCantSaveBecauseChangesWouldRequireAReload");
		}
		string stringMessage = "";
		success &= config.AcceptClientChanges(pendingConfig, whoAmI, ref message2);
		success &= config.AcceptClientChanges(pendingConfig, whoAmI, ref stringMessage);
		if (!string.IsNullOrEmpty(stringMessage))
		{
			message2 = NetworkText.FromLiteral(stringMessage);
		}
		if (success)
		{
			Save(pendingConfig);
			JsonConvert.PopulateObject(json, (object)config, serializerSettingsCompact);
			config.OnChanged();
			ModPacket p = new ModPacket(249);
			p.Write(value: true);
			message2.Serialize(p);
			p.Write(modname2);
			p.Write(configname2);
			p.Write(json);
			p.Send();
		}
		else
		{
			ModPacket p2 = new ModPacket(249);
			p2.Write(value: false);
			message2.Serialize(p2);
			p2.Send(whoAmI);
		}
	}

	public static IEnumerable<PropertyFieldWrapper> GetFieldsAndProperties(object item)
	{
		return GetFieldsAndProperties(item.GetType());
	}

	public static IEnumerable<PropertyFieldWrapper> GetFieldsAndProperties(Type type)
	{
		PropertyInfo[] properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
		return (from x in type.GetFields(BindingFlags.Instance | BindingFlags.Public)
			select new PropertyFieldWrapper(x)).Concat(properties.Select((PropertyInfo x) => new PropertyFieldWrapper(x)));
	}

	public static ModConfig GeneratePopulatedClone(ModConfig original)
	{
		string text = JsonConvert.SerializeObject((object)original, serializerSettings);
		ModConfig properClone = original.Clone();
		JsonConvert.PopulateObject(text, (object)properClone, serializerSettings);
		return properClone;
	}

	public static object? AlternateCreateInstance(Type type)
	{
		if (type == typeof(string))
		{
			return "";
		}
		return Activator.CreateInstance(type, nonPublic: true);
	}

	public static T? GetCustomAttributeFromMemberThenMemberType<T>(PropertyFieldWrapper memberInfo, object? item, object? array) where T : Attribute
	{
		return ((T)Attribute.GetCustomAttribute(memberInfo.MemberInfo, typeof(T))) ?? ((T)Attribute.GetCustomAttribute(memberInfo.Type, typeof(T), inherit: true));
	}

	public static T? GetCustomAttributeFromCollectionMemberThenElementType<T>(MemberInfo memberInfo, Type elementType) where T : Attribute
	{
		return ((T)Attribute.GetCustomAttribute(memberInfo, typeof(T))) ?? ((T)Attribute.GetCustomAttribute(elementType, typeof(T), inherit: true));
	}

	public static Tuple<UIElement, UIElement> WrapIt(UIElement parent, ref int top, PropertyFieldWrapper memberInfo, object item, int order, object? list = null, Type? arrayType = null, int index = -1)
	{
		return UIModConfig.WrapIt(parent, ref top, memberInfo, item, order, list, arrayType, index);
	}

	public static void SetPendingChanges(bool changes = true)
	{
		Interface.modConfig.SetPendingChanges(changes);
	}

	public static bool ObjectEquals(object? a, object? b)
	{
		if (a == b)
		{
			return true;
		}
		if (a == null || b == null)
		{
			return false;
		}
		if (a is IEnumerable && b is IEnumerable && !(a is string) && !(b is string))
		{
			return EnumerableEquals((IEnumerable)a, (IEnumerable)b);
		}
		return a.Equals(b);
	}

	public static bool EnumerableEquals(IEnumerable a, IEnumerable b)
	{
		IEnumerator enumeratorA = a.GetEnumerator();
		IEnumerator enumeratorB = b.GetEnumerator();
		bool hasNextA = enumeratorA.MoveNext();
		bool hasNextB = enumeratorB.MoveNext();
		while (hasNextA && hasNextB)
		{
			if (!ObjectEquals(enumeratorA.Current, enumeratorB.Current))
			{
				return false;
			}
			hasNextA = enumeratorA.MoveNext();
			hasNextB = enumeratorB.MoveNext();
		}
		if (!hasNextA)
		{
			return !hasNextB;
		}
		return false;
	}

	internal static string FormatTextAttribute(LocalizedText localizedText, object[]? args)
	{
		if (args == null)
		{
			return localizedText.Value;
		}
		for (int i = 0; i < args.Length; i++)
		{
			if (args[i] is string s && s.StartsWith("$"))
			{
				args[i] = Language.GetTextValue(FindKeyInScope(s.Substring(1), localizedText.Key));
			}
		}
		return localizedText.Format(args);
		static string FindKeyInScope(string key, string scope)
		{
			if (LanguageManager.Instance.Exists(key))
			{
				return key;
			}
			string[] splitKey = scope.Split(".");
			for (int j = splitKey.Length - 1; j >= 0; j--)
			{
				string combinedKey = string.Join(".", splitKey.Take(j + 1)) + "." + key;
				if (LanguageManager.Instance.Exists(combinedKey))
				{
					return combinedKey;
				}
			}
			return key;
		}
	}

	private static T? GetAndValidate<T>(MemberInfo memberInfo) where T : ConfigKeyAttribute
	{
		T obj = (T)Attribute.GetCustomAttribute(memberInfo, typeof(T));
		if (obj?.malformed ?? false)
		{
			string message = typeof(T).Name + " only accepts localization keys for the 'key' parameter.";
			message = ((!(memberInfo is Type type)) ? (message + $"\nThe member '{memberInfo.Name}' found in the '{memberInfo.DeclaringType}' class caused this exception.") : (message + "\nThe class '" + type.FullName + "' caused this exception."));
			message += "\nClick Open Web Help for more information.";
			throw new ValueNotTranslationKeyException(message);
		}
		return obj;
	}

	private static string GetConfigKey<T>(MemberInfo memberInfo, string dataName) where T : ConfigKeyAttribute
	{
		return GetAndValidate<T>(memberInfo)?.key ?? GetDefaultLocalizationKey(memberInfo, dataName);
	}

	private static string GetDefaultLocalizationKey(MemberInfo member, string dataName)
	{
		string modName;
		string groupKey = (AssemblyManager.GetAssemblyOwner(((member is Type t) ? t : member.DeclaringType).Assembly, out modName) ? ("Mods." + modName + ".Configs") : "Config");
		string memberKey = ((member is Type) ? member.Name : (member.DeclaringType.Name + "." + member.Name));
		return $"{groupKey}.{memberKey}.{dataName}";
	}

	internal static string GetLocalizedLabel(PropertyFieldWrapper member)
	{
		return GetLocalizedText<LabelKeyAttribute, LabelArgsAttribute>(member, "Label") ?? member.Name;
	}

	internal static string GetLocalizedTooltip(PropertyFieldWrapper member)
	{
		return GetLocalizedText<TooltipKeyAttribute, TooltipArgsAttribute>(member, "Tooltip") ?? "";
	}

	private static string? GetLocalizedText<T, TArgs>(PropertyFieldWrapper memberInfo, string dataName) where T : ConfigKeyAttribute where TArgs : ConfigArgsAttribute
	{
		string memberKey = GetConfigKey<T>(memberInfo.MemberInfo, dataName);
		if (!Language.Exists(memberKey))
		{
			return null;
		}
		LocalizedText memberLocalization = Language.GetText(memberKey);
		if (memberLocalization.Value != "")
		{
			return FormatTextAttribute(memberLocalization, memberInfo.MemberInfo.GetCustomAttribute<TArgs>()?.args);
		}
		if (memberInfo.Type.IsPrimitive)
		{
			return null;
		}
		string typeKey = GetConfigKey<T>(memberInfo.Type, dataName);
		if (!Language.Exists(typeKey))
		{
			return null;
		}
		return FormatTextAttribute(Language.GetText(typeKey), memberInfo.Type.GetCustomAttribute<TArgs>()?.args);
	}

	internal static HeaderAttribute? GetLocalizedHeader(MemberInfo memberInfo)
	{
		HeaderAttribute header = memberInfo.GetCustomAttribute<HeaderAttribute>();
		if (header == null)
		{
			return null;
		}
		if (header.malformed)
		{
			throw new ValueNotTranslationKeyException($"{"HeaderAttribute"} only accepts localization keys or identifiers for the 'identifierOrKey' parameter. Neither can have spaces.\nThe member '{memberInfo.Name}' found in the '{memberInfo.DeclaringType}' class caused this exception.\nClick Open Web Help for more information.");
		}
		if (header.IsIdentifier)
		{
			header.key = GetDefaultLocalizationKey(memberInfo.DeclaringType, "Headers." + header.identifier);
		}
		return header;
	}
}
