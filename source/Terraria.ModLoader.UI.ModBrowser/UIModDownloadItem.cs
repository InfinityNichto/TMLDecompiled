using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.Localization;
using Terraria.UI;

namespace Terraria.ModLoader.UI.ModBrowser;

internal class UIModDownloadItem : UIPanel
{
	private const float PADDING = 5f;

	public readonly ModDownloadItem ModDownload;

	private readonly Asset<Texture2D> _dividerTexture;

	private readonly Asset<Texture2D> _innerPanelTexture;

	private readonly UIText _modName;

	private readonly UIImage _updateButton;

	private readonly UIImage _updateWithDepsButton;

	private readonly UIImage _moreInfoButton;

	private readonly UIAutoScaleTextTextPanel<string> tMLUpdateRequired;

	internal ModIconStatus ModIconStatus;

	private UIImage _modIcon;

	internal string tooltip;

	private static int MaxFails = 3;

	private static int ModIconDownloadFailCount = 0;

	private bool UpdateIsDowngrade;

	private readonly bool tMLNeedUpdate;

	private static ConcurrentDictionary<string, Texture2D> TextureDownloadCache = new ConcurrentDictionary<string, Texture2D>();

	private bool HasModIcon => !string.IsNullOrEmpty(ModDownload.ModIconUrl);

	private float ModIconAdjust => 85f;

	private string ViewModInfoText => Language.GetTextValue("tModLoader.ModsMoreInfo");

	private string UpdateWithDepsText
	{
		get
		{
			if (!ModDownload.NeedUpdate)
			{
				return Language.GetTextValue("tModLoader.MBDownloadWithDependencies");
			}
			if (!UpdateIsDowngrade)
			{
				return Language.GetTextValue("tModLoader.MBUpdateWithDependencies");
			}
			return Language.GetTextValue("tModLoader.MBDowngradeWithDependencies");
		}
	}

	public UIModDownloadItem(ModDownloadItem modDownloadItem)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0211: Unknown result type (might be due to invalid IL or missing references)
		//IL_0216: Unknown result type (might be due to invalid IL or missing references)
		//IL_0220: Unknown result type (might be due to invalid IL or missing references)
		//IL_0227: Unknown result type (might be due to invalid IL or missing references)
		//IL_022d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0231: Unknown result type (might be due to invalid IL or missing references)
		//IL_0237: Unknown result type (might be due to invalid IL or missing references)
		//IL_0249: Unknown result type (might be due to invalid IL or missing references)
		//IL_0253: Unknown result type (might be due to invalid IL or missing references)
		//IL_0258: Unknown result type (might be due to invalid IL or missing references)
		ModDownload = modDownloadItem;
		BorderColor = new Color(89, 116, 213) * 0.7f;
		_dividerTexture = UICommon.DividerTexture;
		_innerPanelTexture = UICommon.InnerPanelTexture;
		Height.Pixels = 90f;
		Width.Percent = 1f;
		SetPadding(6f);
		float leftOffset = (HasModIcon ? ModIconAdjust : 0f);
		_modName = new UIText(ModDownload.DisplayName)
		{
			Left = new StyleDimension(leftOffset + 5f, 0f),
			Top = 
			{
				Pixels = 5f
			}
		};
		Append(_modName);
		_moreInfoButton = new UIImage(UICommon.ButtonModInfoTexture)
		{
			Width = 
			{
				Pixels = 36f
			},
			Height = 
			{
				Pixels = 36f
			},
			Left = 
			{
				Pixels = leftOffset
			},
			Top = 
			{
				Pixels = 40f
			}
		};
		_moreInfoButton.OnLeftClick += ViewModInfo;
		Append(_moreInfoButton);
		Version modBuildVersion = ModDownload.ModloaderVersion;
		tMLNeedUpdate = !BuildInfo.IsDev && BuildInfo.tMLVersion < modBuildVersion;
		if (tMLNeedUpdate)
		{
			string updateVersion = $"v{modBuildVersion}";
			bool lastMonth = BuildInfo.tMLVersion.Minor == 12;
			if (BuildInfo.IsStable && new Version(modBuildVersion.Major, modBuildVersion.Minor) == new Version(BuildInfo.tMLVersion.Major + (lastMonth ? 1 : 0), BuildInfo.tMLVersion.Minor + ((!lastMonth) ? 1 : 0)))
			{
				updateVersion = "Preview " + updateVersion;
			}
			tMLUpdateRequired = new UIAutoScaleTextTextPanel<string>(Language.GetTextValue("tModLoader.MBRequiresTMLUpdate", updateVersion)).WithFadedMouseOver(Color.Orange, Color.Orange * 0.7f);
			tMLUpdateRequired.BackgroundColor = Color.Orange * 0.7f;
			tMLUpdateRequired.CopyStyle(_moreInfoButton);
			tMLUpdateRequired.Width.Pixels = 340f;
			tMLUpdateRequired.Left.Pixels += 41f;
			tMLUpdateRequired.OnLeftClick += delegate
			{
				Utils.OpenToURL("https://github.com/tModLoader/tModLoader/releases/latest");
			};
			Append(tMLUpdateRequired);
		}
		_updateButton = new UIImage(UICommon.ButtonExclamationTexture);
		_updateButton.CopyStyle(_moreInfoButton);
		_updateButton.Left.Pixels += 41f;
		_updateButton.OnLeftClick += ShowGameNeedsRestart;
		_updateWithDepsButton = new UIImage(UICommon.ButtonDownloadMultipleTexture);
		_updateWithDepsButton.CopyStyle(_moreInfoButton);
		_updateWithDepsButton.Left.Pixels += 41f;
		_updateWithDepsButton.OnLeftClick += DownloadWithDeps;
		string modReferencesBySlug = ModDownload.ModReferencesBySlug;
		if (modReferencesBySlug != null && modReferencesBySlug.Length > 0)
		{
			Asset<Texture2D> icon = UICommon.ButtonExclamationTexture;
			UIHoverImage modReferenceIcon = new UIHoverImage(icon, Language.GetTextValue("tModLoader.MBClickToViewDependencyMods", string.Join("\n", from x in ModDownload.ModReferencesBySlug.Split(',')
				select x.Trim())))
			{
				Left = 
				{
					Pixels = (float)(-icon.Width()) - 5f,
					Percent = 1f
				}
			};
			modReferenceIcon.OnLeftClick += ShowModDependencies;
			Append(modReferenceIcon);
		}
		base.OnLeftDoubleClick += ViewModInfo;
		UpdateInstallDisplayState();
	}

	public void UpdateInstallDisplayState()
	{
		if (!tMLNeedUpdate)
		{
			_updateWithDepsButton.Remove();
			_updateButton.Remove();
			if (ModDownload.AppNeedRestartToReinstall)
			{
				Append(_updateButton);
			}
			else if (ModDownload.NeedUpdate || !ModDownload.IsInstalled)
			{
				Append(_updateWithDepsButton);
			}
		}
	}

	private void ShowModDependencies(UIMouseEvent evt, UIElement element)
	{
		UIModDownloadItem modListItem = (UIModDownloadItem)element.Parent;
		Interface.modBrowser.SpecialModPackFilter = modListItem.ModDownload.ModReferenceByModId.ToList();
		Interface.modBrowser.SpecialModPackFilterTitle = Language.GetTextValue("tModLoader.MBFilterDependencies");
		Interface.modBrowser.FilterTextBox.Text = "";
		Interface.modBrowser.UpdateNeeded = true;
		SoundEngine.PlaySound(in SoundID.MenuOpen);
	}

	private void ShowGameNeedsRestart(UIMouseEvent evt, UIElement element)
	{
		Utils.ShowFancyErrorMessage(Language.GetTextValue("tModLoader.SteamRejectUpdate", ModDownload.DisplayName), 10007);
	}

	protected override void DrawSelf(SpriteBatch spriteBatch)
	{
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		base.DrawSelf(spriteBatch);
		if (HasModIcon && ModIconStatus == ModIconStatus.UNKNOWN)
		{
			RequestModIcon();
		}
		CalculatedStyle innerDimensions = GetInnerDimensions();
		float leftOffset = (HasModIcon ? ModIconAdjust : 0f);
		Vector2 drawPos = default(Vector2);
		((Vector2)(ref drawPos))._002Ector(innerDimensions.X + 5f + leftOffset, innerDimensions.Y + 30f);
		spriteBatch.Draw(_dividerTexture.Value, drawPos, (Rectangle?)null, Color.White, 0f, Vector2.Zero, new Vector2((innerDimensions.Width - 10f - leftOffset) / 8f, 1f), (SpriteEffects)0, 0f);
		((Vector2)(ref drawPos))._002Ector(innerDimensions.X + innerDimensions.Width - 125f, innerDimensions.Y + 45f);
		DrawTimeText(spriteBatch, drawPos);
		UIImage updateButton = _updateButton;
		if (updateButton != null && updateButton.IsMouseHovering)
		{
			tooltip = Language.GetTextValue("tModLoader.BrowserRejectWarning");
			return;
		}
		UIImage updateWithDepsButton = _updateWithDepsButton;
		if (updateWithDepsButton != null && updateWithDepsButton.IsMouseHovering)
		{
			tooltip = UpdateWithDepsText;
			return;
		}
		UIImage moreInfoButton = _moreInfoButton;
		if (moreInfoButton != null && moreInfoButton.IsMouseHovering)
		{
			tooltip = ViewModInfoText;
		}
	}

	public override void Draw(SpriteBatch spriteBatch)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		tooltip = null;
		base.Draw(spriteBatch);
		if (!string.IsNullOrEmpty(tooltip))
		{
			Rectangle bounds = GetOuterDimensions().ToRectangle();
			bounds.Height += 16;
			UICommon.DrawHoverStringInBounds(spriteBatch, tooltip, bounds);
		}
	}

	protected override void DrawChildren(SpriteBatch spriteBatch)
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		base.DrawChildren(spriteBatch);
		UIAutoScaleTextTextPanel<string> uIAutoScaleTextTextPanel = tMLUpdateRequired;
		if (uIAutoScaleTextTextPanel != null && uIAutoScaleTextTextPanel.IsMouseHovering)
		{
			UICommon.DrawHoverStringInBounds(spriteBatch, Language.GetTextValue("tModLoader.MBClickToUpdate"), GetInnerDimensions().ToRectangle());
		}
		if (_modName.IsMouseHovering)
		{
			UICommon.DrawHoverStringInBounds(spriteBatch, Language.GetTextValue("tModLoader.ModsByline", ModDownload.Author), GetInnerDimensions().ToRectangle());
		}
	}

	public override void Update(GameTime gameTime)
	{
		base.Update(gameTime);
		if (ModIconStatus == ModIconStatus.READY)
		{
			if (_modIcon == null)
			{
				AdjustPositioningFailedIcon();
			}
			else
			{
				Append(_modIcon);
			}
			ModIconStatus = ModIconStatus.DISPLAYED_OR_FAILED;
		}
	}

	private async void RequestModIcon()
	{
		if (ModIconDownloadFailCount < MaxFails)
		{
			ModIconStatus = ModIconStatus.REQUESTED;
			Texture2D texture = await GetOrDownloadTextureAsync(ModDownload.ModIconUrl);
			if (texture != null)
			{
				_modIcon = new UIImage(texture)
				{
					Left = 
					{
						Percent = 0f
					},
					Top = 
					{
						Percent = 0f
					},
					MaxWidth = 
					{
						Pixels = 80f,
						Percent = 0f
					},
					MaxHeight = 
					{
						Pixels = 80f,
						Percent = 0f
					},
					ScaleToFit = true
				};
			}
		}
		ModIconStatus = ModIconStatus.READY;
	}

	private static async Task<Texture2D?> GetOrDownloadTextureAsync(string url)
	{
		if (TextureDownloadCache.TryGetValue(url, out var texture))
		{
			return texture;
		}
		try
		{
			byte[] data = await new WebClient().DownloadDataTaskAsync(url);
			texture = Main.Assets.CreateUntracked<Texture2D>(new MemoryStream(data), ".png").Value;
			TextureDownloadCache.TryAdd(url, texture);
			return texture;
		}
		catch
		{
			Interlocked.Increment(ref ModIconDownloadFailCount);
			return null;
		}
	}

	private void AdjustPositioningFailedIcon()
	{
		_modName.Left.Pixels -= ModIconAdjust;
		_moreInfoButton.Left.Pixels -= ModIconAdjust;
		if (_updateButton != null)
		{
			_updateButton.Left.Pixels -= ModIconAdjust;
		}
		if (_updateWithDepsButton != null)
		{
			_updateWithDepsButton.Left.Pixels -= ModIconAdjust;
		}
	}

	private void DrawTimeText(SpriteBatch spriteBatch, Vector2 drawPos)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		if (ModDownload.TimeStamp == DateTime.MinValue)
		{
			return;
		}
		spriteBatch.Draw(_innerPanelTexture.Value, drawPos, (Rectangle?)new Rectangle(0, 0, 8, _innerPanelTexture.Height()), Color.White);
		spriteBatch.Draw(_innerPanelTexture.Value, new Vector2(drawPos.X + 8f, drawPos.Y), (Rectangle?)new Rectangle(8, 0, 8, _innerPanelTexture.Height()), Color.White, 0f, Vector2.Zero, new Vector2(13.625f, 1f), (SpriteEffects)0, 0f);
		spriteBatch.Draw(_innerPanelTexture.Value, new Vector2(drawPos.X + 125f - 8f, drawPos.Y), (Rectangle?)new Rectangle(16, 0, 8, _innerPanelTexture.Height()), Color.White);
		drawPos += new Vector2(0f, 2f);
		try
		{
			string text = TimeHelper.HumanTimeSpanString(ModDownload.TimeStamp);
			int textWidth = (int)FontAssets.MouseText.Value.MeasureString(text).X;
			int diffWidth = 125 - textWidth;
			drawPos.X += (float)diffWidth * 0.5f;
			Utils.DrawBorderString(spriteBatch, text, drawPos, Color.White);
		}
		catch (Exception e)
		{
			Logging.tML.Error((object)"Problem during drawing of time text", e);
		}
	}

	public override void MouseOver(UIMouseEvent evt)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		base.MouseOver(evt);
		BackgroundColor = UICommon.DefaultUIBlue;
		BorderColor = new Color(89, 116, 213);
	}

	public override void MouseOut(UIMouseEvent evt)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		base.MouseOut(evt);
		BackgroundColor = new Color(63, 82, 151) * 0.7f;
		BorderColor = new Color(89, 116, 213) * 0.7f;
	}

	private async void DownloadWithDeps(UIMouseEvent evt, UIElement listeningElement)
	{
		SoundEngine.PlaySound(in SoundID.MenuTick);
		if (await Interface.modBrowser.DownloadMods(new ModDownloadItem[1] { ModDownload }))
		{
			Main.QueueMainThreadAction(delegate
			{
				Main.menuMode = 10007;
			});
		}
	}

	private void ViewModInfo(UIMouseEvent evt, UIElement listeningElement)
	{
		SoundEngine.PlaySound(in SoundID.MenuOpen);
		Utils.OpenToURL(Interface.modBrowser.SocialBackend.GetModWebPage(ModDownload.PublishId));
	}
}
