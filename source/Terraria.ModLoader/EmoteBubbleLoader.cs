using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Chat.Commands;
using Terraria.GameContent.UI;
using Terraria.GameContent.UI.Elements;
using Terraria.Localization;

namespace Terraria.ModLoader;

public static class EmoteBubbleLoader
{
	internal static readonly List<ModEmoteBubble> emoteBubbles = new List<ModEmoteBubble>();

	internal static readonly List<GlobalEmoteBubble> globalEmoteBubbles = new List<GlobalEmoteBubble>();

	internal static readonly Dictionary<int, List<ModEmoteBubble>> categoryEmoteLookup = new Dictionary<int, List<ModEmoteBubble>>();

	public static int EmoteBubbleCount => emoteBubbles.Count + EmoteID.Count;

	internal static int Add(ModEmoteBubble emoteBubble)
	{
		if (ModNet.AllowVanillaClients)
		{
			throw new Exception("Adding emote bubbles breaks vanilla client compatibility");
		}
		emoteBubbles.Add(emoteBubble);
		return EmoteBubbleCount - 1;
	}

	internal static void Unload()
	{
		emoteBubbles.Clear();
		globalEmoteBubbles.Clear();
		categoryEmoteLookup.Clear();
	}

	internal static void ResizeArrays()
	{
		Array.Resize(ref Lang._emojiNameCache, EmoteBubbleCount);
		for (int i = EmoteID.Count; i < EmoteBubbleCount; i++)
		{
			Lang._emojiNameCache[i] = LocalizedText.Empty;
		}
	}

	internal static void FinishSetup()
	{
		foreach (ModEmoteBubble emoteBubble in emoteBubbles)
		{
			Lang._emojiNameCache[emoteBubble.Type] = emoteBubble.Command;
			if (emoteBubble.Command != LocalizedText.Empty)
			{
				EmojiCommand._byName[emoteBubble.Command] = emoteBubble.Type;
			}
		}
	}

	internal static Dictionary<Mod, List<int>> GetAllUnlockedModEmotes()
	{
		Dictionary<Mod, List<int>> result = new Dictionary<Mod, List<int>>();
		foreach (ModEmoteBubble modEmote in emoteBubbles.Where((ModEmoteBubble e) => e.IsUnlocked()))
		{
			if (!result.TryGetValue(modEmote.Mod, out var emoteList))
			{
				emoteList = (result[modEmote.Mod] = new List<int>());
			}
			emoteList.Add(modEmote.Type);
		}
		return result;
	}

	internal static List<int> AddEmotesToCategory(this List<int> emotesList, int categoryId)
	{
		if (categoryEmoteLookup.TryGetValue(categoryId, out var modEmotes))
		{
			emotesList.AddRange(from e in modEmotes
				where e.IsUnlocked()
				select e.Type);
		}
		return emotesList;
	}

	/// <summary>
	/// Gets the <see cref="T:Terraria.ModLoader.ModEmoteBubble" /> instance corresponding to the specified ID.
	/// </summary>
	/// <param name="type">The ID of the emote bubble</param>
	/// <returns>The <see cref="T:Terraria.ModLoader.ModEmoteBubble" /> instance in the emote bubbles array, null if not found.</returns>
	public static ModEmoteBubble GetEmoteBubble(int type)
	{
		if (type < EmoteID.Count || type >= EmoteBubbleCount)
		{
			return null;
		}
		return emoteBubbles[type - EmoteID.Count];
	}

	public static void OnSpawn(EmoteBubble emoteBubble)
	{
		if (emoteBubble.emote >= EmoteID.Count && emoteBubble.emote < EmoteBubbleCount)
		{
			emoteBubble.ModEmoteBubble = GetEmoteBubble(emoteBubble.emote).NewInstance(emoteBubble);
		}
		foreach (GlobalEmoteBubble globalEmoteBubble in globalEmoteBubbles)
		{
			globalEmoteBubble.OnSpawn(emoteBubble);
		}
		emoteBubble.ModEmoteBubble?.OnSpawn();
	}

	public static bool UpdateFrame(EmoteBubble emoteBubble)
	{
		bool result = true;
		foreach (GlobalEmoteBubble globalEmoteBubble in globalEmoteBubbles)
		{
			result &= globalEmoteBubble.UpdateFrame(emoteBubble);
		}
		if (!result)
		{
			return false;
		}
		return emoteBubble.ModEmoteBubble?.UpdateFrame() ?? true;
	}

	public static bool UpdateFrameInEmoteMenu(int emoteType, ref int frameCounter)
	{
		bool result = true;
		foreach (GlobalEmoteBubble globalEmoteBubble in globalEmoteBubbles)
		{
			result &= globalEmoteBubble.UpdateFrameInEmoteMenu(emoteType, ref frameCounter);
		}
		if (!result)
		{
			return false;
		}
		return GetEmoteBubble(emoteType)?.UpdateFrameInEmoteMenu(ref frameCounter) ?? true;
	}

	public static bool PreDraw(EmoteBubble emoteBubble, SpriteBatch spriteBatch, Texture2D texture, Vector2 position, Rectangle frame, Vector2 origin, SpriteEffects spriteEffects)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		bool result = true;
		foreach (GlobalEmoteBubble globalEmoteBubble in globalEmoteBubbles)
		{
			result &= globalEmoteBubble.PreDraw(emoteBubble, spriteBatch, texture, position, frame, origin, spriteEffects);
		}
		if (!result)
		{
			return false;
		}
		return emoteBubble.ModEmoteBubble?.PreDraw(spriteBatch, texture, position, frame, origin, spriteEffects) ?? true;
	}

	public static void PostDraw(EmoteBubble emoteBubble, SpriteBatch spriteBatch, Texture2D texture, Vector2 position, Rectangle frame, Vector2 origin, SpriteEffects spriteEffects)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		foreach (GlobalEmoteBubble globalEmoteBubble in globalEmoteBubbles)
		{
			globalEmoteBubble.PostDraw(emoteBubble, spriteBatch, texture, position, frame, origin, spriteEffects);
		}
		emoteBubble.ModEmoteBubble?.PostDraw(spriteBatch, texture, position, frame, origin, spriteEffects);
	}

	public static bool PreDrawInEmoteMenu(int emoteType, SpriteBatch spriteBatch, EmoteButton uiEmoteButton, Vector2 position, Rectangle frame, Vector2 origin)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		bool result = true;
		foreach (GlobalEmoteBubble globalEmoteBubble in globalEmoteBubbles)
		{
			result &= globalEmoteBubble.PreDrawInEmoteMenu(emoteType, spriteBatch, uiEmoteButton, position, frame, origin);
		}
		if (!result)
		{
			return false;
		}
		return GetEmoteBubble(emoteType)?.PreDrawInEmoteMenu(spriteBatch, uiEmoteButton, position, frame, origin) ?? true;
	}

	public static void PostDrawInEmoteMenu(int emoteType, SpriteBatch spriteBatch, EmoteButton uiEmoteButton, Vector2 position, Rectangle frame, Vector2 origin)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		foreach (GlobalEmoteBubble globalEmoteBubble in globalEmoteBubbles)
		{
			globalEmoteBubble.PostDrawInEmoteMenu(emoteType, spriteBatch, uiEmoteButton, position, frame, origin);
		}
		GetEmoteBubble(emoteType)?.PostDrawInEmoteMenu(spriteBatch, uiEmoteButton, position, frame, origin);
	}

	public static Rectangle? GetFrame(EmoteBubble emoteBubble)
	{
		if (emoteBubble.ModEmoteBubble != null)
		{
			return emoteBubble.ModEmoteBubble.GetFrame();
		}
		Rectangle? result = null;
		foreach (GlobalEmoteBubble globalEmoteBubble in globalEmoteBubbles)
		{
			Rectangle? frameRect = globalEmoteBubble.GetFrame(emoteBubble);
			if (frameRect.HasValue)
			{
				result = frameRect;
			}
		}
		return result;
	}

	public static Rectangle? GetFrameInEmoteMenu(int emoteType, int frame, int frameCounter)
	{
		if (emoteType >= EmoteID.Count)
		{
			return GetEmoteBubble(emoteType)?.GetFrameInEmoteMenu(frame, frameCounter);
		}
		Rectangle? result = null;
		foreach (GlobalEmoteBubble globalEmoteBubble in globalEmoteBubbles)
		{
			Rectangle? frameRect = globalEmoteBubble.GetFrameInEmoteMenu(emoteType, frame, frameCounter);
			if (frameRect.HasValue)
			{
				result = frameRect;
			}
		}
		return result;
	}
}
