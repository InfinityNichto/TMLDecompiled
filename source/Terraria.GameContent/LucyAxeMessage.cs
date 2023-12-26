using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.GameContent.UI;
using Terraria.ID;
using Terraria.Localization;
using Terraria.UI;

namespace Terraria.GameContent;

public static class LucyAxeMessage
{
	public enum MessageSource
	{
		Idle,
		Storage,
		ThrownAway,
		PickedUp,
		ChoppedTree,
		ChoppedGemTree,
		ChoppedCactus,
		Count
	}

	private static byte _variation;

	private static int[] _messageCooldownsByType = new int[7];

	private static string GetCategoryName(MessageSource source)
	{
		return source switch
		{
			MessageSource.Storage => "LucyTheAxe_Storage", 
			MessageSource.ThrownAway => "LucyTheAxe_ThrownAway", 
			MessageSource.PickedUp => "LucyTheAxe_PickedUp", 
			MessageSource.ChoppedTree => "LucyTheAxe_ChoppedTree", 
			MessageSource.ChoppedGemTree => "LucyTheAxe_GemTree", 
			MessageSource.ChoppedCactus => "LucyTheAxe_ChoppedCactus", 
			_ => "LucyTheAxe_Idle", 
		};
	}

	public static void Initialize()
	{
		ItemSlot.OnItemTransferred += ItemSlot_OnItemTransferred;
		Player.Hooks.OnEnterWorld += Hooks_OnEnterWorld;
	}

	private static void Hooks_OnEnterWorld(Player player)
	{
		if (player == Main.LocalPlayer)
		{
			GiveIdleMessageCooldown();
		}
	}

	public static void UpdateMessageCooldowns()
	{
		for (int i = 0; i < _messageCooldownsByType.Length; i++)
		{
			if (_messageCooldownsByType[i] > 0)
			{
				_messageCooldownsByType[i]--;
			}
		}
	}

	public static void TryPlayingIdleMessage()
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		MessageSource messageSource = MessageSource.Idle;
		if (_messageCooldownsByType[(int)messageSource] <= 0)
		{
			Player localPlayer = Main.LocalPlayer;
			Create(messageSource, localPlayer.Top, new Vector2(Main.rand.NextFloatDirection() * 7f, -2f + Main.rand.NextFloat() * -2f));
		}
	}

	private static void ItemSlot_OnItemTransferred(ItemSlot.ItemTransferInfo info)
	{
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		if (info.ItemType != 5095)
		{
			return;
		}
		bool flag = CountsAsStorage(info.FromContenxt);
		bool flag2 = CountsAsStorage(info.ToContext);
		if (flag != flag2)
		{
			MessageSource messageSource = ((!flag) ? MessageSource.Storage : MessageSource.PickedUp);
			if (_messageCooldownsByType[(int)messageSource] <= 0)
			{
				PutMessageTypeOnCooldown(messageSource, 420);
				Player localPlayer = Main.LocalPlayer;
				Create(messageSource, localPlayer.Top, new Vector2((float)(localPlayer.direction * 7), -2f));
			}
		}
	}

	private static void GiveIdleMessageCooldown()
	{
		PutMessageTypeOnCooldown(MessageSource.Idle, Main.rand.Next(7200, 14400));
	}

	public static void PutMessageTypeOnCooldown(MessageSource source, int timeInFrames)
	{
		_messageCooldownsByType[(int)source] = timeInFrames;
	}

	private static bool CountsAsStorage(int itemSlotContext)
	{
		if (itemSlotContext == 3 || itemSlotContext == 6 || itemSlotContext == 15)
		{
			return true;
		}
		return false;
	}

	public static void TryCreatingMessageWithCooldown(MessageSource messageSource, Vector2 position, Vector2 velocity, int cooldownTimeInTicks)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		if (Main.netMode != 2 && _messageCooldownsByType[(int)messageSource] <= 0)
		{
			PutMessageTypeOnCooldown(messageSource, cooldownTimeInTicks);
			Create(messageSource, position, velocity);
		}
	}

	public static void Create(MessageSource source, Vector2 position, Vector2 velocity)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		if (Main.netMode != 2)
		{
			GiveIdleMessageCooldown();
			SpawnPopupText(source, _variation, position, velocity);
			PlaySound(source, position);
			SpawnEmoteBubble();
			if (Main.netMode == 1)
			{
				NetMessage.SendData(141, -1, -1, null, (int)source, (int)_variation, velocity.X, velocity.Y, (int)position.X, (int)position.Y);
			}
			_variation++;
		}
	}

	private static void SpawnEmoteBubble()
	{
		EmoteBubble.MakeLocalPlayerEmote(149);
	}

	public static void CreateFromNet(MessageSource source, byte variation, Vector2 position, Vector2 velocity)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		SpawnPopupText(source, variation, position, velocity);
		PlaySound(source, position);
	}

	private static void PlaySound(MessageSource source, Vector2 position)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		SoundEngine.PlaySound(in SoundID.LucyTheAxeTalk, position);
	}

	private static void SpawnPopupText(MessageSource source, int variationUnwrapped, Vector2 position, Vector2 velocity)
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		string textForVariation = GetTextForVariation(source, variationUnwrapped);
		AdvancedPopupRequest request = default(AdvancedPopupRequest);
		request.Text = textForVariation;
		request.DurationInFrames = 420;
		request.Velocity = velocity;
		request.Color = new Color(184, 96, 98) * 1.15f;
		PopupText.NewText(request, position);
	}

	private static string GetTextForVariation(MessageSource source, int variationUnwrapped)
	{
		string categoryName = GetCategoryName(source);
		return LanguageManager.Instance.IndexedFromCategory(categoryName, variationUnwrapped).Value;
	}
}
