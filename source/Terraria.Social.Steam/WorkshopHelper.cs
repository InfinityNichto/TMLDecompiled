using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using Microsoft.Xna.Framework;
using Steamworks;
using Terraria.GameContent.UI.States;
using Terraria.IO;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.Core;
using Terraria.ModLoader.Engine;
using Terraria.ModLoader.UI;
using Terraria.ModLoader.UI.ModBrowser;
using Terraria.Social.Base;
using Terraria.UI.Chat;
using Terraria.Utilities;

namespace Terraria.Social.Steam;

public class WorkshopHelper
{
	public class UGCBased
	{
		public struct SteamWorkshopItem
		{
			public string ContentFolderPath;

			public string Description;

			public string PreviewImagePath;

			public string[] Tags;

			public string Title;

			public ERemoteStoragePublishedFileVisibility? Visibility;

			public NameValueCollection BuildData;

			public string ChangeNotes;
		}

		public class Downloader
		{
			public List<string> ResourcePackPaths { get; private set; }

			public List<string> WorldPaths { get; private set; }

			public List<string> ModPaths { get; private set; }

			public Downloader()
			{
				ResourcePackPaths = new List<string>();
				WorldPaths = new List<string>();
				ModPaths = new List<string>();
			}

			public static Downloader Create()
			{
				return new Downloader();
			}

			public List<string> GetListOfSubscribedItemsPaths()
			{
				//IL_0008: Unknown result type (might be due to invalid IL or missing references)
				//IL_000d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0014: Unknown result type (might be due to invalid IL or missing references)
				//IL_0019: Unknown result type (might be due to invalid IL or missing references)
				return ((IEnumerable<AppId_t>)(object)new AppId_t[2]
				{
					Terraria.ModLoader.Engine.Steam.TMLAppID_t,
					Terraria.ModLoader.Engine.Steam.TerrariaAppId_t
				}).Select((AppId_t app) => Path.Combine(GetWorkshopFolder(app), "content", ((object)(AppId_t)(ref app)).ToString())).Where(Directory.Exists).SelectMany(Directory.EnumerateDirectories)
					.ToList();
			}

			public bool Prepare(WorkshopIssueReporter issueReporter)
			{
				return Refresh(issueReporter);
			}

			public bool Refresh(WorkshopIssueReporter issueReporter)
			{
				ResourcePackPaths.Clear();
				WorldPaths.Clear();
				ModPaths.Clear();
				foreach (string listOfSubscribedItemsPath in GetListOfSubscribedItemsPaths())
				{
					if (listOfSubscribedItemsPath == null)
					{
						continue;
					}
					try
					{
						string path = listOfSubscribedItemsPath + Path.DirectorySeparatorChar + "workshop.json";
						if (File.Exists(path))
						{
							switch (AWorkshopEntry.ReadHeader(File.ReadAllText(path)))
							{
							case "ResourcePack":
								ResourcePackPaths.Add(listOfSubscribedItemsPath);
								break;
							case "Mod":
								ModPaths.Add(listOfSubscribedItemsPath);
								break;
							case "World":
								WorldPaths.Add(listOfSubscribedItemsPath);
								break;
							}
						}
					}
					catch (Exception exception)
					{
						issueReporter.ReportDownloadProblem("Workshop.ReportIssue_FailedToLoadSubscribedFile", listOfSubscribedItemsPath, exception);
						return false;
					}
				}
				return true;
			}
		}

		public class PublishedItemsFinder
		{
			private Dictionary<ulong, SteamWorkshopItem> _items = new Dictionary<ulong, SteamWorkshopItem>();

			private UGCQueryHandle_t m_UGCQueryHandle;

			private CallResult<SteamUGCQueryCompleted_t> OnSteamUGCQueryCompletedCallResult;

			private CallResult<SteamUGCRequestUGCDetailsResult_t> OnSteamUGCRequestUGCDetailsResultCallResult;

			public bool HasItemOfId(ulong id)
			{
				return _items.ContainsKey(id);
			}

			public static PublishedItemsFinder Create()
			{
				PublishedItemsFinder publishedItemsFinder = new PublishedItemsFinder();
				publishedItemsFinder.LoadHooks();
				return publishedItemsFinder;
			}

			private void LoadHooks()
			{
				OnSteamUGCQueryCompletedCallResult = CallResult<SteamUGCQueryCompleted_t>.Create((APIDispatchDelegate<SteamUGCQueryCompleted_t>)OnSteamUGCQueryCompleted);
				OnSteamUGCRequestUGCDetailsResultCallResult = CallResult<SteamUGCRequestUGCDetailsResult_t>.Create((APIDispatchDelegate<SteamUGCRequestUGCDetailsResult_t>)OnSteamUGCRequestUGCDetailsResult);
			}

			public void Prepare()
			{
				Refresh();
			}

			public void Refresh()
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				//IL_0009: Unknown result type (might be due to invalid IL or missing references)
				//IL_0011: Unknown result type (might be due to invalid IL or missing references)
				//IL_0016: Unknown result type (might be due to invalid IL or missing references)
				//IL_001c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0021: Unknown result type (might be due to invalid IL or missing references)
				//IL_002d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0032: Unknown result type (might be due to invalid IL or missing references)
				//IL_0037: Unknown result type (might be due to invalid IL or missing references)
				//IL_003e: Unknown result type (might be due to invalid IL or missing references)
				CSteamID steamID = SteamUser.GetSteamID();
				m_UGCQueryHandle = SteamUGC.CreateQueryUserUGCRequest(((CSteamID)(ref steamID)).GetAccountID(), (EUserUGCList)0, (EUGCMatchingUGCType)(-1), (EUserUGCListSortOrder)0, SteamUtils.GetAppID(), SteamUtils.GetAppID(), 1u);
				CoreSocialModule.SetSkipPulsing(shouldSkipPausing: true);
				SteamAPICall_t hAPICall = SteamUGC.SendQueryUGCRequest(m_UGCQueryHandle);
				OnSteamUGCQueryCompletedCallResult.Set(hAPICall, (APIDispatchDelegate<SteamUGCQueryCompleted_t>)OnSteamUGCQueryCompleted);
				CoreSocialModule.SetSkipPulsing(shouldSkipPausing: false);
			}

			private void OnSteamUGCQueryCompleted(SteamUGCQueryCompleted_t pCallback, bool bIOFailure)
			{
				//IL_0018: Unknown result type (might be due to invalid IL or missing references)
				//IL_000e: Unknown result type (might be due to invalid IL or missing references)
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0015: Invalid comparison between Unknown and I4
				//IL_007d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0029: Unknown result type (might be due to invalid IL or missing references)
				//IL_0037: Unknown result type (might be due to invalid IL or missing references)
				//IL_0038: Unknown result type (might be due to invalid IL or missing references)
				//IL_0086: Unknown result type (might be due to invalid IL or missing references)
				_items.Clear();
				if (bIOFailure || (int)pCallback.m_eResult != 1)
				{
					SteamUGC.ReleaseQueryUGCRequest(m_UGCQueryHandle);
					return;
				}
				SteamUGCDetails_t pDetails = default(SteamUGCDetails_t);
				for (uint num = 0u; num < pCallback.m_unNumResultsReturned; num++)
				{
					SteamUGC.GetQueryUGCResult(m_UGCQueryHandle, num, ref pDetails);
					ulong publishedFileId = pDetails.m_nPublishedFileId.m_PublishedFileId;
					SteamWorkshopItem steamWorkshopItem = default(SteamWorkshopItem);
					steamWorkshopItem.Title = ((SteamUGCDetails_t)(ref pDetails)).m_rgchTitle;
					steamWorkshopItem.Description = ((SteamUGCDetails_t)(ref pDetails)).m_rgchDescription;
					SteamWorkshopItem value = steamWorkshopItem;
					_items.Add(publishedFileId, value);
				}
				SteamUGC.ReleaseQueryUGCRequest(m_UGCQueryHandle);
			}

			private void OnSteamUGCRequestUGCDetailsResult(SteamUGCRequestUGCDetailsResult_t pCallback, bool bIOFailure)
			{
			}
		}

		public abstract class APublisherInstance
		{
			public delegate void FinishedPublishingAction(APublisherInstance instance);

			protected WorkshopItemPublicSettingId _publicity;

			protected SteamWorkshopItem _entryData;

			protected PublishedFileId_t _publishedFileID;

			protected EResult _createCallback;

			protected EResult _updateCallback;

			private UGCUpdateHandle_t _updateHandle;

			private CallResult<CreateItemResult_t> _createItemHook;

			private CallResult<SubmitItemUpdateResult_t> _updateItemHook;

			private FinishedPublishingAction _endAction;

			private WorkshopIssueReporter _issueReporter;

			public void PublishContent(PublishedItemsFinder finder, WorkshopIssueReporter issueReporter, FinishedPublishingAction endAction, string itemTitle, string itemDescription, string contentFolderPath, string previewImagePath, WorkshopItemPublicSettingId publicity, string[] tags, NameValueCollection buildData = null, ulong existingID = 0uL, string changeNotes = null)
			{
				//IL_0061: Unknown result type (might be due to invalid IL or missing references)
				//IL_0066: Unknown result type (might be due to invalid IL or missing references)
				//IL_009f: Unknown result type (might be due to invalid IL or missing references)
				//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
				//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
				//IL_012d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0132: Unknown result type (might be due to invalid IL or missing references)
				Utils.LogAndConsoleInfoMessage(Language.GetTextValueWith("tModLoader.PublishItem", _entryData.Title));
				_issueReporter = issueReporter;
				_endAction = endAction;
				_createItemHook = CallResult<CreateItemResult_t>.Create((APIDispatchDelegate<CreateItemResult_t>)CreateItemResult);
				_updateItemHook = CallResult<SubmitItemUpdateResult_t>.Create((APIDispatchDelegate<SubmitItemUpdateResult_t>)UpdateItemResult);
				_publicity = publicity;
				ERemoteStoragePublishedFileVisibility visibility = GetVisibility(publicity);
				_entryData = new SteamWorkshopItem
				{
					Title = itemTitle,
					Description = itemDescription,
					ContentFolderPath = contentFolderPath,
					Tags = tags,
					PreviewImagePath = previewImagePath,
					Visibility = visibility,
					BuildData = buildData,
					ChangeNotes = changeNotes
				};
				if (!File.Exists(previewImagePath))
				{
					_issueReporter.ReportInstantUploadProblem("Workshop.ReportIssue_FailedToPublish_CouldNotFindFolderToUpload");
					return;
				}
				if (!Directory.Exists(contentFolderPath))
				{
					_issueReporter.ReportInstantUploadProblem("Workshop.ReportIssue_FailedToPublish_CouldNotFindFolderToUpload");
					return;
				}
				_publishedFileID = new PublishedFileId_t(existingID);
				if (existingID == 0L && AWorkshopEntry.TryReadingManifest(contentFolderPath + Path.DirectorySeparatorChar + "workshop.json", out var info))
				{
					_publishedFileID = new PublishedFileId_t(info.workshopEntryId);
				}
				if (!WrappedWriteManifest())
				{
					return;
				}
				if (_publishedFileID.m_PublishedFileId != 0L)
				{
					if (buildData == null)
					{
						PreventUpdatingCertainThings();
					}
					UpdateItem();
				}
				else
				{
					CreateItem();
				}
			}

			private void PreventUpdatingCertainThings()
			{
				_entryData.Title = null;
				_entryData.Description = null;
			}

			private ERemoteStoragePublishedFileVisibility GetVisibility(WorkshopItemPublicSettingId publicityId)
			{
				return (ERemoteStoragePublishedFileVisibility)(publicityId switch
				{
					WorkshopItemPublicSettingId.FriendsOnly => 1, 
					WorkshopItemPublicSettingId.Public => 0, 
					_ => 2, 
				});
			}

			private void CreateItem()
			{
				//IL_001c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0027: Unknown result type (might be due to invalid IL or missing references)
				//IL_002d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0032: Unknown result type (might be due to invalid IL or missing references)
				//IL_0039: Unknown result type (might be due to invalid IL or missing references)
				//IL_0065: Unknown result type (might be due to invalid IL or missing references)
				//IL_006d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0073: Invalid comparison between Unknown and I4
				//IL_0089: Unknown result type (might be due to invalid IL or missing references)
				Utils.LogAndConsoleInfoMessage(Language.GetTextValue("tModLoader.CreateItem", _entryData.Title));
				_createCallback = (EResult)0;
				CoreSocialModule.SetSkipPulsing(shouldSkipPausing: true);
				SteamAPICall_t hAPICall = SteamUGC.CreateItem(SteamUtils.GetAppID(), (EWorkshopFileType)0);
				_createItemHook.Set(hAPICall, (APIDispatchDelegate<CreateItemResult_t>)CreateItemResult);
				CoreSocialModule.SetSkipPulsing(shouldSkipPausing: false);
				if (!Main.dedServ)
				{
					return;
				}
				do
				{
					Thread.Sleep(1);
					SteamedWraps.RunCallbacks();
				}
				while ((int)_createCallback == 0);
				if ((int)_createCallback == 1)
				{
					UpdateItem(creatingItem: true);
					do
					{
						Thread.Sleep(1);
						SteamedWraps.RunCallbacks();
					}
					while ((int)_createCallback == 0);
				}
			}

			private void CreateItemResult(CreateItemResult_t param, bool bIOFailure)
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_000c: Unknown result type (might be due to invalid IL or missing references)
				//IL_000d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0013: Invalid comparison between Unknown and I4
				//IL_004a: Unknown result type (might be due to invalid IL or missing references)
				//IL_004b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0050: Unknown result type (might be due to invalid IL or missing references)
				//IL_005c: Unknown result type (might be due to invalid IL or missing references)
				_createCallback = param.m_eResult;
				if ((int)param.m_eResult != 1)
				{
					_issueReporter.ReportDelayedUploadProblemWithoutKnownReason("Workshop.ReportIssue_FailedToPublish_WithoutKnownReason", ((object)(EResult)(ref param.m_eResult)).ToString());
					SteamedWraps.ReportCheckSteamLogs();
					_endAction(this);
					return;
				}
				_publishedFileID = param.m_nPublishedFileId;
				WrappedWriteManifest();
				if (param.m_bUserNeedsToAcceptWorkshopLegalAgreement)
				{
					_issueReporter.ReportDelayedUploadProblem("Workshop.ReportIssue_FailedToPublish_UserDidNotAcceptWorkshopTermsOfService");
					_endAction(this);
				}
				else if (!Main.dedServ)
				{
					UpdateItem();
				}
			}

			protected abstract string GetHeaderText();

			protected abstract void PrepareContentForUpdate();

			private void UpdateItem(bool creatingItem = false)
			{
				//IL_0027: Unknown result type (might be due to invalid IL or missing references)
				//IL_002d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0032: Unknown result type (might be due to invalid IL or missing references)
				//IL_0037: Unknown result type (might be due to invalid IL or missing references)
				//IL_02ad: Unknown result type (might be due to invalid IL or missing references)
				//IL_02b2: Unknown result type (might be due to invalid IL or missing references)
				//IL_0045: Unknown result type (might be due to invalid IL or missing references)
				//IL_008a: Unknown result type (might be due to invalid IL or missing references)
				//IL_009c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0069: Unknown result type (might be due to invalid IL or missing references)
				//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
				//IL_02dd: Unknown result type (might be due to invalid IL or missing references)
				//IL_02f7: Unknown result type (might be due to invalid IL or missing references)
				//IL_02f9: Unknown result type (might be due to invalid IL or missing references)
				//IL_02fe: Unknown result type (might be due to invalid IL or missing references)
				//IL_0300: Unknown result type (might be due to invalid IL or missing references)
				//IL_0301: Unknown result type (might be due to invalid IL or missing references)
				//IL_030c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0105: Unknown result type (might be due to invalid IL or missing references)
				//IL_00df: Unknown result type (might be due to invalid IL or missing references)
				//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
				//IL_028b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0290: Unknown result type (might be due to invalid IL or missing references)
				//IL_0292: Unknown result type (might be due to invalid IL or missing references)
				//IL_034c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0163: Unknown result type (might be due to invalid IL or missing references)
				//IL_016c: Unknown result type (might be due to invalid IL or missing references)
				WrappedWriteManifest();
				Utils.LogAndConsoleInfoMessage(Language.GetTextValue("tModLoader.UpdateItem", _entryData.Title));
				PrepareContentForUpdate();
				UGCUpdateHandle_t uGCUpdateHandle_t = SteamUGC.StartItemUpdate(SteamUtils.GetAppID(), _publishedFileID);
				if (_entryData.Title != null)
				{
					SteamUGC.SetItemTitle(uGCUpdateHandle_t, _entryData.Title);
				}
				if (!string.IsNullOrEmpty(_entryData.Description))
				{
					SteamUGC.SetItemDescription(uGCUpdateHandle_t, _entryData.Description);
				}
				Logging.tML.Info((object)"Adding tags and visibility");
				SteamUGC.SetItemContent(uGCUpdateHandle_t, _entryData.ContentFolderPath);
				SteamUGC.SetItemTags(uGCUpdateHandle_t, (IList<string>)_entryData.Tags);
				if (_entryData.PreviewImagePath != null)
				{
					SteamUGC.SetItemPreview(uGCUpdateHandle_t, _entryData.PreviewImagePath);
				}
				if (_entryData.Visibility.HasValue)
				{
					SteamUGC.SetItemVisibility(uGCUpdateHandle_t, _entryData.Visibility.Value);
				}
				Logging.tML.Info((object)"Setting the language for default description");
				SteamUGC.SetItemUpdateLanguage(uGCUpdateHandle_t, SteamedWraps.GetCurrentSteamLangKey());
				string patchNotes = "";
				_entryData.BuildData["version"] = "0.0.0";
				if (_entryData.BuildData != null)
				{
					Logging.tML.Info((object)"Adding tModLoader Metadata to Workshop Upload");
					string[] metadataKeys = MetadataKeys;
					foreach (string key in metadataKeys)
					{
						SteamUGC.RemoveItemKeyValueTags(uGCUpdateHandle_t, key);
						SteamUGC.AddItemKeyValueTag(uGCUpdateHandle_t, key, _entryData.BuildData[key]);
					}
					patchNotes = _entryData.ChangeNotes;
					if (string.IsNullOrWhiteSpace(patchNotes))
					{
						patchNotes = "Version {ModVersion} has been published to {tMLBuildPurpose} tModLoader v{tMLVersion}";
						if (!string.IsNullOrWhiteSpace(_entryData.BuildData["homepage"]))
						{
							patchNotes += ", learn more at the [url={ModHomepage}]homepage[/url]";
						}
					}
					patchNotes = Language.GetText(patchNotes).FormatWith(new
					{
						ModVersion = _entryData.BuildData["trueversion"],
						ModHomepage = _entryData.BuildData["homepage"],
						tMLVersion = BuildInfo.tMLVersion.MajorMinor().ToString(),
						tMLBuildPurpose = BuildInfo.Purpose.ToString()
					});
					string refs = _entryData.BuildData["workshopdeps"];
					if (!string.IsNullOrWhiteSpace(refs))
					{
						Logging.tML.Info((object)"Adding dependencies to Workshop Upload");
						metadataKeys = refs.Split(",", StringSplitOptions.TrimEntries);
						PublishedFileId_t child = default(PublishedFileId_t);
						foreach (string dependency in metadataKeys)
						{
							try
							{
								((PublishedFileId_t)(ref child))._002Ector((ulong)uint.Parse(dependency));
								SteamUGC.AddDependency(_publishedFileID, child);
							}
							catch (Exception)
							{
								ILog tML = Logging.tML;
								PublishedFileId_t publishedFileID = _publishedFileID;
								tML.Error((object)("Failed to add Workshop dependency: " + dependency + " to " + ((object)(PublishedFileId_t)(ref publishedFileID)).ToString()));
							}
						}
					}
				}
				_updateCallback = (EResult)0;
				Logging.tML.Info((object)"Submitting workshop update handle to Steam");
				CoreSocialModule.SetSkipPulsing(shouldSkipPausing: true);
				SteamAPICall_t hAPICall = SteamUGC.SubmitItemUpdate(uGCUpdateHandle_t, patchNotes);
				_updateHandle = uGCUpdateHandle_t;
				_updateItemHook.Set(hAPICall, (APIDispatchDelegate<SubmitItemUpdateResult_t>)UpdateItemResult);
				CoreSocialModule.SetSkipPulsing(shouldSkipPausing: false);
				Logging.tML.Info((object)"Handle submitted. Waiting on Steam");
				if (!(!Main.dedServ || creatingItem))
				{
					do
					{
						Thread.Sleep(1);
						SteamedWraps.RunCallbacks();
					}
					while ((int)_updateCallback == 0);
				}
			}

			private void UpdateItemResult(SubmitItemUpdateResult_t param, bool bIOFailure)
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_000c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0032: Unknown result type (might be due to invalid IL or missing references)
				//IL_0038: Invalid comparison between Unknown and I4
				//IL_0040: Unknown result type (might be due to invalid IL or missing references)
				//IL_0045: Unknown result type (might be due to invalid IL or missing references)
				//IL_0046: Unknown result type (might be due to invalid IL or missing references)
				//IL_0049: Invalid comparison between Unknown and I4
				//IL_005d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0060: Invalid comparison between Unknown and I4
				//IL_004b: Unknown result type (might be due to invalid IL or missing references)
				//IL_004d: Invalid comparison between Unknown and I4
				//IL_0062: Unknown result type (might be due to invalid IL or missing references)
				//IL_0065: Invalid comparison between Unknown and I4
				//IL_004f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0051: Invalid comparison between Unknown and I4
				//IL_0110: Unknown result type (might be due to invalid IL or missing references)
				//IL_0115: Unknown result type (might be due to invalid IL or missing references)
				//IL_006a: Unknown result type (might be due to invalid IL or missing references)
				//IL_006d: Invalid comparison between Unknown and I4
				//IL_0053: Unknown result type (might be due to invalid IL or missing references)
				//IL_0056: Invalid comparison between Unknown and I4
				_updateCallback = param.m_eResult;
				if (param.m_bUserNeedsToAcceptWorkshopLegalAgreement)
				{
					_issueReporter.ReportDelayedUploadProblem("Workshop.ReportIssue_FailedToPublish_UserDidNotAcceptWorkshopTermsOfService");
					_endAction(this);
					return;
				}
				if ((int)_updateCallback != 1)
				{
					SteamedWraps.ReportCheckSteamLogs();
				}
				EResult updateCallback = _updateCallback;
				if ((int)updateCallback <= 9)
				{
					if ((int)updateCallback != 1)
					{
						if ((int)updateCallback != 8)
						{
							if ((int)updateCallback != 9)
							{
								goto IL_0093;
							}
							_issueReporter.ReportDelayedUploadProblem("Workshop.ReportIssue_FailedToPublish_CouldNotFindFolderToUpload");
						}
						else
						{
							_issueReporter.ReportDelayedUploadProblem("Workshop.ReportIssue_FailedToPublish_InvalidParametersForPublishing");
						}
					}
					else
					{
						SteamFriends.ActivateGameOverlayToWebPage("steam://url/CommunityFilePage/" + _publishedFileID.m_PublishedFileId, (EActivateGameOverlayToWebPageMode)0);
					}
				}
				else if ((int)updateCallback != 15)
				{
					if ((int)updateCallback != 25)
					{
						if ((int)updateCallback != 33)
						{
							goto IL_0093;
						}
						_issueReporter.ReportDelayedUploadProblem("Workshop.ReportIssue_FailedToPublish_SteamFileLockFailed");
					}
					else
					{
						_issueReporter.ReportDelayedUploadProblem("Workshop.ReportIssue_FailedToPublish_LimitExceeded");
					}
				}
				else
				{
					_issueReporter.ReportDelayedUploadProblem("Workshop.ReportIssue_FailedToPublish_AccessDeniedBecauseUserDoesntOwnLicenseForApp");
				}
				goto IL_010f;
				IL_0093:
				_issueReporter.ReportDelayedUploadProblemWithoutKnownReason("Workshop.ReportIssue_FailedToPublish_WithoutKnownReason", ((object)(EResult)(ref param.m_eResult)).ToString());
				goto IL_010f;
				IL_010f:
				SteamUGC.SubscribeItem(_publishedFileID);
				_endAction(this);
			}

			private bool TryWritingManifestToFolder(string folderPath, string manifestText)
			{
				string path = folderPath + Path.DirectorySeparatorChar + "workshop.json";
				bool result = true;
				try
				{
					File.WriteAllText(path, manifestText);
					return result;
				}
				catch (Exception exception)
				{
					_issueReporter.ReportManifestCreationProblem("Workshop.ReportIssue_CouldNotCreateResourcePackManifestFile", exception);
					return false;
				}
			}

			private bool WrappedWriteManifest()
			{
				string headerText = GetHeaderText();
				if (TryWritingManifestToFolder(_entryData.ContentFolderPath, headerText))
				{
					return true;
				}
				_endAction(this);
				return false;
			}

			public bool TryGetProgress(out float progress)
			{
				//IL_0008: Unknown result type (might be due to invalid IL or missing references)
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0015: Unknown result type (might be due to invalid IL or missing references)
				//IL_0020: Unknown result type (might be due to invalid IL or missing references)
				//IL_0029: Unknown result type (might be due to invalid IL or missing references)
				progress = 0f;
				if (_updateHandle == default(UGCUpdateHandle_t))
				{
					return false;
				}
				ulong punBytesProcessed = default(ulong);
				ulong punBytesTotal = default(ulong);
				SteamUGC.GetItemUpdateProgress(_updateHandle, ref punBytesProcessed, ref punBytesTotal);
				if (punBytesTotal == 0L)
				{
					return false;
				}
				progress = (float)((double)punBytesProcessed / (double)punBytesTotal);
				return true;
			}
		}

		public class ResourcePackPublisherInstance : APublisherInstance
		{
			private ResourcePack _resourcePack;

			public ResourcePackPublisherInstance(ResourcePack resourcePack)
			{
				_resourcePack = resourcePack;
			}

			protected override string GetHeaderText()
			{
				return TexturePackWorkshopEntry.GetHeaderTextFor(_resourcePack, _publishedFileID.m_PublishedFileId, _entryData.Tags, _publicity, _entryData.PreviewImagePath);
			}

			protected override void PrepareContentForUpdate()
			{
			}
		}

		public class WorldPublisherInstance : APublisherInstance
		{
			private WorldFileData _world;

			public WorldPublisherInstance(WorldFileData world)
			{
				_world = world;
			}

			protected override string GetHeaderText()
			{
				return WorldWorkshopEntry.GetHeaderTextFor(_world, _publishedFileID.m_PublishedFileId, _entryData.Tags, _publicity, _entryData.PreviewImagePath);
			}

			protected override void PrepareContentForUpdate()
			{
				if (_world.IsCloudSave)
				{
					FileUtilities.CopyToLocal(_world.Path, _entryData.ContentFolderPath + Path.DirectorySeparatorChar + "world.wld");
				}
				else
				{
					FileUtilities.Copy(_world.Path, _entryData.ContentFolderPath + Path.DirectorySeparatorChar + "world.wld", cloud: false);
				}
			}
		}

		public const string ManifestFileName = "workshop.json";
	}

	public class ModPublisherInstance : UGCBased.APublisherInstance
	{
		protected override string GetHeaderText()
		{
			return ModWorkshopEntry.GetHeaderTextFor(_publishedFileID.m_PublishedFileId, _entryData.Tags, _publicity, _entryData.PreviewImagePath);
		}

		protected override void PrepareContentForUpdate()
		{
		}
	}

	internal static class QueryHelper
	{
		internal class AQueryInstance
		{
			private CallResult<SteamUGCQueryCompleted_t> _queryHook;

			protected UGCQueryHandle_t _primaryUGCHandle;

			protected EResult _primaryQueryResult;

			protected uint _queryReturnCount;

			internal List<ulong> ugcChildren = new List<ulong>();

			internal QueryParameters queryParameters;

			internal int numberPages;

			internal uint totalItemsQueried;

			internal AQueryInstance(QueryParameters queryParameters)
			{
				_queryHook = CallResult<SteamUGCQueryCompleted_t>.Create((APIDispatchDelegate<SteamUGCQueryCompleted_t>)OnWorkshopQueryInitialized);
				this.queryParameters = queryParameters;
			}

			private void OnWorkshopQueryInitialized(SteamUGCQueryCompleted_t pCallback, bool bIOFailure)
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_000d: Unknown result type (might be due to invalid IL or missing references)
				//IL_000e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0013: Unknown result type (might be due to invalid IL or missing references)
				//IL_0019: Unknown result type (might be due to invalid IL or missing references)
				//IL_002c: Unknown result type (might be due to invalid IL or missing references)
				//IL_0036: Unknown result type (might be due to invalid IL or missing references)
				_primaryUGCHandle = pCallback.m_handle;
				_primaryQueryResult = pCallback.m_eResult;
				_queryReturnCount = pCallback.m_unNumResultsReturned;
				if (totalItemsQueried == 0 && pCallback.m_unTotalMatchingResults != 0)
				{
					totalItemsQueried = pCallback.m_unTotalMatchingResults;
					numberPages = (int)Math.Ceiling((double)totalItemsQueried / 50.0);
				}
			}

			/// <summary>
			/// Ought be called to release the existing query when we are done with it. Frees memory associated with the handle.
			/// </summary>
			private void ReleaseWorkshopQuery()
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				SteamedWraps.ReleaseWorkshopHandle(_primaryUGCHandle);
			}

			/// <summary>
			/// For direct information gathering of particular mod/workshop items. Synchronous.
			/// Note that the List size is 1 to 1 with the provided array.
			/// If the Mod is not found, the space is filled with a null.
			/// </summary>
			internal List<ModDownloadItem> QueryItemsSynchronously(out List<string> missingMods)
			{
				//IL_0084: Unknown result type (might be due to invalid IL or missing references)
				double numPages = Math.Ceiling((float)queryParameters.searchModIds.Length / 50f);
				List<ModDownloadItem> items = new List<ModDownloadItem>();
				missingMods = new List<string>();
				for (int i = 0; (double)i < numPages; i++)
				{
					string[] idArray = (from x in queryParameters.searchModIds.Take((i * 50)..(50 * (i + 1)))
						select x.m_ModPubId).ToArray();
					try
					{
						WaitForQueryResult(SteamedWraps.GenerateDirectItemsQuery(idArray));
						for (int j = 0; j < _queryReturnCount; j++)
						{
							ModDownloadItem item = GenerateModDownloadItemFromQuery((uint)j);
							if (item == null)
							{
								Logging.tML.Warn((object)("Unable to find Mod with ID " + idArray[j] + " on the Steam Workshop"));
								missingMods.Add(idArray[j]);
							}
							else
							{
								item.UpdateInstallState();
								items.Add(item);
							}
						}
					}
					finally
					{
						ReleaseWorkshopQuery();
					}
				}
				return items;
			}

			internal async IAsyncEnumerable<ModDownloadItem> QueryAllWorkshopItems([EnumeratorCancellation] CancellationToken token = default(CancellationToken))
			{
				uint currentPage = 1u;
				int currentPageAttempts = 0;
				uint num;
				do
				{
					token.ThrowIfCancellationRequested();
					try
					{
						try
						{
							await WaitForQueryResultAsync(SteamedWraps.GenerateAndSubmitModBrowserQuery(currentPage, queryParameters), token);
						}
						catch
						{
							if (currentPageAttempts == 1)
							{
								throw;
							}
							await Task.Delay(100, token);
							currentPage--;
							currentPageAttempts++;
							goto end_IL_0088;
						}
						foreach (ModDownloadItem item in await Task.Run((Func<IEnumerable<ModDownloadItem>>)ProcessPageResult))
						{
							yield return item;
						}
						goto IL_02f0;
						end_IL_0088:;
					}
					finally
					{
						ReleaseWorkshopQuery();
					}
					goto IL_02f7;
					IL_02f0:
					currentPageAttempts = 0;
					goto IL_02f7;
					IL_02f7:
					num = currentPage + 1;
					currentPage = num;
				}
				while (num <= numberPages);
			}

			private IEnumerable<ModDownloadItem> ProcessPageResult()
			{
				for (uint i = 0u; i < _queryReturnCount; i++)
				{
					ModDownloadItem mod = GenerateModDownloadItemFromQuery(i);
					if (mod != null)
					{
						yield return mod;
					}
				}
			}

			/// <summary>
			/// Only Use if we don't have a PublishID source.
			/// Outputs a List of ModDownloadItems of equal length to QueryParameters.SearchModSlugs
			/// Uses null entries to fill gaps to ensure length consistency
			/// </summary>
			internal bool TrySearchByInternalName(out List<ModDownloadItem> items)
			{
				//IL_0024: Unknown result type (might be due to invalid IL or missing references)
				items = new List<ModDownloadItem>();
				string[] searchModSlugs = queryParameters.searchModSlugs;
				foreach (string slug in searchModSlugs)
				{
					try
					{
						WaitForQueryResult(SteamedWraps.GenerateAndSubmitModBrowserQuery(1u, queryParameters, slug));
						if (_queryReturnCount == 0)
						{
							Logging.tML.Info((object)("No Mod on Workshop with internal name: " + slug));
							items.Add(null);
						}
						else
						{
							items.Add(GenerateModDownloadItemFromQuery(0u));
						}
					}
					catch
					{
						return false;
					}
					finally
					{
						ReleaseWorkshopQuery();
					}
				}
				return true;
			}

			internal async Task WaitForQueryResultAsync(SteamAPICall_t query, CancellationToken token = default(CancellationToken))
			{
				//IL_0016: Unknown result type (might be due to invalid IL or missing references)
				//IL_0017: Unknown result type (might be due to invalid IL or missing references)
				_primaryQueryResult = (EResult)0;
				_queryHook.Set(query, (APIDispatchDelegate<SteamUGCQueryCompleted_t>)null);
				Stopwatch stopwatch = Stopwatch.StartNew();
				while (true)
				{
					SteamedWraps.RunCallbacks();
					if ((int)_primaryQueryResult != 0)
					{
						break;
					}
					if (stopwatch.Elapsed.TotalSeconds >= 10.0)
					{
						throw new TimeoutException("No response from steam workshop query");
					}
					await Task.Delay(1, token);
				}
				if ((int)_primaryQueryResult != 1)
				{
					SteamedWraps.ReportCheckSteamLogs();
					throw new Exception($"Error: Unable to access Steam Workshop. ERROR CODE: {_primaryQueryResult}");
				}
			}

			[Obsolete("Should not be used because it hides syncronous waiting")]
			internal void WaitForQueryResult(SteamAPICall_t query)
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				WaitForQueryResultAsync(query).GetAwaiter().GetResult();
			}

			internal ModDownloadItem GenerateModDownloadItemFromQuery(uint i)
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_000d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0012: Unknown result type (might be due to invalid IL or missing references)
				//IL_0013: Unknown result type (might be due to invalid IL or missing references)
				//IL_0014: Unknown result type (might be due to invalid IL or missing references)
				//IL_0019: Unknown result type (might be due to invalid IL or missing references)
				//IL_001a: Unknown result type (might be due to invalid IL or missing references)
				//IL_001b: Unknown result type (might be due to invalid IL or missing references)
				//IL_0024: Unknown result type (might be due to invalid IL or missing references)
				//IL_0025: Unknown result type (might be due to invalid IL or missing references)
				//IL_002b: Invalid comparison between Unknown and I4
				//IL_0077: Unknown result type (might be due to invalid IL or missing references)
				//IL_008f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0037: Unknown result type (might be due to invalid IL or missing references)
				//IL_0038: Unknown result type (might be due to invalid IL or missing references)
				//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
				//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
				//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
				//IL_026c: Unknown result type (might be due to invalid IL or missing references)
				//IL_027a: Unknown result type (might be due to invalid IL or missing references)
				SteamUGCDetails_t pDetails = SteamedWraps.FetchItemDetails(_primaryUGCHandle, i);
				PublishedFileId_t id = pDetails.m_nPublishedFileId;
				if ((int)pDetails.m_eVisibility != 0)
				{
					return null;
				}
				if ((int)pDetails.m_eResult != 1)
				{
					ILog tML = Logging.tML;
					PublishedFileId_t val = id;
					tML.Warn((object)("Unable to fetch mod PublishId#" + ((object)(PublishedFileId_t)(ref val)).ToString() + " information. " + ((object)(EResult)(ref pDetails.m_eResult)).ToString()));
					return null;
				}
				string ownerId = pDetails.m_ulSteamIDOwner.ToString();
				DateTime lastUpdate = Utils.UnixTimeStampToDateTime(pDetails.m_rtimeUpdated);
				string displayname = ((SteamUGCDetails_t)(ref pDetails)).m_rgchTitle;
				SteamedWraps.FetchMetadata(_primaryUGCHandle, i, out var metadata);
				if (metadata["versionsummary"] == null)
				{
					metadata["versionsummary"] = metadata["version"];
				}
				string[] missingKeys = MetadataKeys.Where((string k) => metadata.Get(k) == null).ToArray();
				if (missingKeys.Length != 0)
				{
					Logging.tML.Warn((object)$"Mod '{displayname}' is missing required metadata: {string.Join(',', missingKeys.Select((string k) => "'" + k + "'"))}.");
					return null;
				}
				if (string.IsNullOrWhiteSpace(metadata["name"]))
				{
					Logging.tML.Warn((object)$"Mod has no name: {id}");
					return null;
				}
				string[] refsById = (from x in SteamedWraps.FetchItemDependencies(_primaryUGCHandle, i, pDetails.m_unNumChildren)
					select x.m_PublishedFileId.ToString()).ToArray();
				(Version, Version) cVersion = CalculateRelevantVersion(((SteamUGCDetails_t)(ref pDetails)).m_rgchDescription, metadata);
				ModSide modside = ModSide.Both;
				if (metadata["modside"] == "Client")
				{
					modside = ModSide.Client;
				}
				if (metadata["modside"] == "Server")
				{
					modside = ModSide.Server;
				}
				if (metadata["modside"] == "NoSync")
				{
					modside = ModSide.NoSync;
				}
				SteamedWraps.FetchPreviewImageUrl(_primaryUGCHandle, i, out var modIconURL);
				SteamedWraps.FetchPlayTimeStats(_primaryUGCHandle, i, out var hot, out var downloads);
				return new ModDownloadItem(displayname, metadata["name"], cVersion.Item1, metadata["author"], metadata["modreferences"], modside, modIconURL, id.m_PublishedFileId.ToString(), (int)downloads, (int)hot, lastUpdate, cVersion.Item2, metadata["homepage"], ownerId, refsById);
			}
		}

		internal static async IAsyncEnumerable<ModDownloadItem> QueryWorkshop(QueryParameters queryParams, [EnumeratorCancellation] CancellationToken token)
		{
			AQueryInstance queryHandle = new AQueryInstance(queryParams);
			await foreach (ModDownloadItem item in queryHandle.QueryAllWorkshopItems(token))
			{
				if (item != null)
				{
					yield return item;
				}
			}
		}
	}

	internal static string[] MetadataKeys = new string[8] { "name", "author", "modside", "homepage", "modloaderversion", "version", "modreferences", "versionsummary" };

	private static readonly Regex MetadataInDescriptionFallbackRegex = new Regex("\\[quote=GithubActions\\(Don't Modify\\)\\]Version Summary: (.*) \\[/quote\\]", RegexOptions.Compiled);

	public static string GetWorkshopFolder(AppId_t app)
	{
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		if (Program.LaunchParameters.TryGetValue("-steamworkshopfolder", out var workshopLocCustom))
		{
			if (Directory.Exists(workshopLocCustom))
			{
				return workshopLocCustom;
			}
			Logging.tML.Warn((object)("-steamworkshopfolder path not found: " + workshopLocCustom));
		}
		string steamClientPath = null;
		if (SteamedWraps.SteamClient)
		{
			SteamApps.GetAppInstallDir(app, ref steamClientPath, 1000u);
		}
		if (steamClientPath == null)
		{
			steamClientPath = ".";
		}
		steamClientPath = Path.Combine(steamClientPath, "..", "..", "workshop");
		if (SteamedWraps.SteamClient || (!SteamedWraps.SteamAvailable && !Program.LaunchParameters.ContainsKey("-nosteam") && Directory.Exists(steamClientPath)))
		{
			return steamClientPath;
		}
		return Path.Combine("steamapps", "workshop");
	}

	internal static bool TryGetModDownloadItem(string modSlug, out ModDownloadItem item)
	{
		item = null;
		QueryParameters queryParameters = default(QueryParameters);
		queryParameters.searchModSlugs = new string[1] { modSlug };
		if (!new QueryHelper.AQueryInstance(queryParameters).TrySearchByInternalName(out var items))
		{
			return false;
		}
		item = items[0];
		return item != null;
	}

	internal static bool GetPublishIdLocal(TmodFile modFile, out ulong publishId)
	{
		publishId = 0uL;
		if (modFile == null || !ModOrganizer.TryReadManifest(ModOrganizer.GetParentDir(modFile.path), out var info))
		{
			return false;
		}
		publishId = info.workshopEntryId;
		return true;
	}

	internal static bool TryGetPublishIdByInternalName(QueryParameters query, out List<string> modIds)
	{
		modIds = new List<string>();
		if (!TryGetModDownloadItemsByInternalName(query, out var items))
		{
			return false;
		}
		for (int i = 0; i < query.searchModSlugs.Length; i++)
		{
			if (items[i] == null)
			{
				Logging.tML.Info((object)("Unable to find the PublishID for " + query.searchModSlugs[i]));
				modIds.Add("0");
			}
			else
			{
				modIds.Add(items[i].PublishId.m_ModPubId);
			}
		}
		return true;
	}

	internal static bool TryGetModDownloadItemsByInternalName(QueryParameters query, out List<ModDownloadItem> mods)
	{
		if (!new QueryHelper.AQueryInstance(query).TrySearchByInternalName(out mods))
		{
			return false;
		}
		return true;
	}

	private static (Version modV, Version tmlV) CalculateRelevantVersion(string mbDescription, NameValueCollection metadata)
	{
		(Version, Version) selectVersion = (new Version(metadata["version"].Replace("v", "")), new Version(metadata["modloaderversion"].Replace("tModLoader v", "")));
		if (!metadata["versionsummary"].Contains(':'))
		{
			return selectVersion;
		}
		InnerCalculateRelevantVersion(ref selectVersion, metadata["versionsummary"]);
		Match match = MetadataInDescriptionFallbackRegex.Match(mbDescription);
		if (match.Success)
		{
			InnerCalculateRelevantVersion(ref selectVersion, match.Groups[1].Value);
		}
		return selectVersion;
	}

	private static void InnerCalculateRelevantVersion(ref (Version modV, Version tmlV) selectVersion, string versionSummary)
	{
		(Version, Version)[] array = VersionSummaryToArray(versionSummary);
		for (int i = 0; i < array.Length; i++)
		{
			(Version, Version) item = array[i];
			if (!(item.Item1.MajorMinor() > BuildInfo.tMLVersion.MajorMinor()) && (selectVersion.modV < item.Item2 || selectVersion.tmlV.MajorMinor() < item.Item1.MajorMinor()))
			{
				selectVersion.modV = item.Item2;
				(selectVersion.tmlV, _) = item;
			}
		}
	}

	private static (Version tmlVersion, Version modVersion)[] VersionSummaryToArray(string versionSummary)
	{
		return (from s in versionSummary.Split(";")
			select (new Version(s.Split(":")[0]), new Version(s.Split(":")[1]))).ToArray();
	}

	internal static void PublishMod(LocalMod mod, string iconPath)
	{
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		TmodFile modFile = mod.modFile;
		BuildProperties bp = mod.properties;
		if (bp.buildVersion != modFile.TModLoaderVersion)
		{
			throw new WebException(Language.GetTextValue("tModLoader.OutdatedModCantPublishError"));
		}
		string changeLogFile = Path.Combine(ModCompile.ModSourcePath, modFile.Name, "changelog.txt");
		string changeLog = ((!File.Exists(changeLogFile)) ? "" : File.ReadAllText(changeLogFile));
		string workshopDescFile = Path.Combine(ModCompile.ModSourcePath, modFile.Name, "description_workshop.txt");
		string workshopDesc = ((!File.Exists(workshopDescFile)) ? bp.description : File.ReadAllText(workshopDescFile));
		NameValueCollection values = new NameValueCollection
		{
			{ "displayname", bp.displayName },
			{
				"displaynameclean",
				string.Join("", from x in ChatManager.ParseMessage(bp.displayName, Color.White)
					where x.GetType() == typeof(TextSnippet)
					select x.Text)
			},
			{ "name", modFile.Name },
			{
				"version",
				$"{bp.version}"
			},
			{ "author", bp.author },
			{ "homepage", bp.homepage },
			{ "description", workshopDesc },
			{ "iconpath", iconPath },
			{
				"sourcesfolder",
				Path.Combine(ModCompile.ModSourcePath, modFile.Name)
			},
			{
				"modloaderversion",
				$"{modFile.TModLoaderVersion}"
			},
			{
				"modreferences",
				string.Join(", ", bp.modReferences.Select((BuildProperties.ModReference x) => x.mod))
			},
			{
				"modside",
				bp.side.ToFriendlyString()
			},
			{ "changelog", changeLog }
		};
		if (string.IsNullOrWhiteSpace(values["author"]))
		{
			throw new WebException("You need to specify an author in build.txt");
		}
		if (string.IsNullOrWhiteSpace(values["version"]))
		{
			throw new WebException("You need to specify a version in build.txt");
		}
		if (!Main.dedServ)
		{
			Main.MenuUI.SetState(new WorkshopPublishInfoStateForMods(Interface.modSources, modFile, values));
			return;
		}
		SocialAPI.LoadSteam();
		WorkshopItemPublishSettings publishSetttings = new WorkshopItemPublishSettings
		{
			Publicity = WorkshopItemPublicSettingId.Public,
			UsedTags = Array.Empty<WorkshopTagOption>(),
			PreviewImagePath = iconPath
		};
		SteamedWraps.SteamClient = true;
		SocialAPI.Workshop.PublishMod(modFile, values, publishSetttings);
	}
}
