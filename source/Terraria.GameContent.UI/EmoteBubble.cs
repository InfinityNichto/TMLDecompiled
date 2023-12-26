using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.Events;
using Terraria.ID;
using Terraria.ModLoader;

namespace Terraria.GameContent.UI;

public class EmoteBubble : IEntityWithGlobals<GlobalEmoteBubble>
{
	internal static int[] CountNPCs = new int[NPCID.Count];

	internal static Dictionary<int, EmoteBubble> byID = new Dictionary<int, EmoteBubble>();

	private static List<int> toClean = new List<int>();

	private static int NextID;

	internal int ID;

	public WorldUIAnchor anchor;

	public int lifeTime;

	public int lifeTimeStart;

	/// <summary>
	/// This is the internal ID of this EmoteBubble.
	/// </summary>
	public int emote;

	public int metadata;

	public int frameSpeed = 8;

	public int frameCounter;

	public int frame;

	public const int EMOTE_SHEET_HORIZONTAL_FRAMES = 8;

	public const int EMOTE_SHEET_EMOTES_PER_ROW = 4;

	public static readonly int EMOTE_SHEET_VERTICAL_FRAMES = 2 + (EmoteID.Count - 1) / 4;

	internal GlobalEmoteBubble[] _globals;

	public ModEmoteBubble ModEmoteBubble { get; internal set; }

	int IEntityWithGlobals<GlobalEmoteBubble>.Type => emote;

	public RefReadOnlyArray<GlobalEmoteBubble> EntityGlobals => _globals;

	/// <summary>
	/// The whoAmI indicator that indicates this <see cref="T:Terraria.GameContent.UI.EmoteBubble" />, can be used in <see cref="M:Terraria.GameContent.UI.EmoteBubble.GetExistingEmoteBubble(System.Int32)" />
	/// </summary>
	public int WhoAmI => ID;

	/// <summary>
	/// Whether or not this emote is fully displayed
	/// <br>The first and the last 6 frames are for bubble-popping animation. The emote content is displayed after the animation</br>
	/// </summary>
	public bool IsFullyDisplayed
	{
		get
		{
			if (lifeTime >= 6)
			{
				return lifeTimeStart - lifeTime >= 6;
			}
			return false;
		}
	}

	public static void UpdateAll()
	{
		lock (byID)
		{
			toClean.Clear();
			foreach (KeyValuePair<int, EmoteBubble> item in byID)
			{
				item.Value.Update();
				if (item.Value.lifeTime <= 0)
				{
					toClean.Add(item.Key);
				}
			}
			foreach (int item2 in toClean)
			{
				byID.Remove(item2);
			}
			toClean.Clear();
		}
	}

	public static void DrawAll(SpriteBatch sb)
	{
		lock (byID)
		{
			foreach (KeyValuePair<int, EmoteBubble> item in byID)
			{
				item.Value.Draw(sb);
			}
		}
	}

	public static Tuple<int, int> SerializeNetAnchor(WorldUIAnchor anch)
	{
		if (anch.type == WorldUIAnchor.AnchorType.Entity)
		{
			int item = 0;
			if (anch.entity is NPC)
			{
				item = 0;
			}
			else if (anch.entity is Player)
			{
				item = 1;
			}
			else if (anch.entity is Projectile)
			{
				item = 2;
			}
			int whoAmI = anch.entity.whoAmI;
			if (!ModNet.AllowVanillaClients && anch.entity is Projectile projectile)
			{
				whoAmI = projectile.identity;
				item = (projectile.owner << 8) | item;
			}
			return Tuple.Create(item, whoAmI);
		}
		return Tuple.Create(0, 0);
	}

	public static WorldUIAnchor DeserializeNetAnchor(int type, int meta)
	{
		int packedOwnerType = type;
		type = packedOwnerType & 0xFF;
		switch (type)
		{
		case 0:
			return new WorldUIAnchor((Entity)Main.npc[meta]);
		case 1:
			return new WorldUIAnchor((Entity)Main.player[meta]);
		case 2:
			if (!ModNet.AllowVanillaClients)
			{
				int owner = packedOwnerType >> 8;
				int whoAmI = Main.maxProjectiles;
				for (int i = 0; i < Main.maxProjectiles; i++)
				{
					Projectile projectile = Main.projectile[i];
					if (projectile.owner == owner && projectile.identity == meta && projectile.active)
					{
						whoAmI = i;
						break;
					}
				}
				return new WorldUIAnchor((Entity)Main.projectile[whoAmI]);
			}
			return new WorldUIAnchor((Entity)Main.projectile[meta]);
		default:
			throw new Exception("How did you end up getting this?");
		}
	}

	public static int AssignNewID()
	{
		return NextID++;
	}

	/// <summary>
	/// Use this method to spawn a emote bubble
	/// </summary>
	/// <param name="emoticon">The emote ID of the emote that will spawn.</param>
	/// <param name="bubbleAnchor">The <see cref="T:Terraria.GameContent.UI.WorldUIAnchor" /> instance for the emote. You can use <code>new WorldUIAnchor(Entity)</code> to get the instance.</param>
	/// <param name="time">How long this emote remains.</param>
	/// <returns>The <see cref="P:Terraria.GameContent.UI.EmoteBubble.WhoAmI" /> of this emote</returns>
	public static int NewBubble(int emoticon, WorldUIAnchor bubbleAnchor, int time)
	{
		if (Main.netMode == 1)
		{
			return -1;
		}
		EmoteBubble emoteBubble = new EmoteBubble(emoticon, bubbleAnchor, time);
		emoteBubble.ID = AssignNewID();
		byID[emoteBubble.ID] = emoteBubble;
		if (Main.netMode == 2)
		{
			Tuple<int, int> tuple = SerializeNetAnchor(bubbleAnchor);
			NetMessage.SendData(91, -1, -1, null, emoteBubble.ID, tuple.Item1, tuple.Item2, time, emoticon);
		}
		OnBubbleChange(emoteBubble.ID);
		EmoteBubbleLoader.OnSpawn(emoteBubble);
		return emoteBubble.ID;
	}

	/// <summary>
	/// Use this method to make NPCs use a random emote based on the pick emote table.
	/// </summary>
	/// <param name="bubbleAnchor">The <see cref="T:Terraria.GameContent.UI.WorldUIAnchor" /> instance for the emote. You can use <code>new WorldUIAnchor(NPC)</code> to get the instance.</param>
	/// <param name="time">How long this emote remains.</param>
	/// <param name="other">The <see cref="T:Terraria.GameContent.UI.WorldUIAnchor" /> instance from the other side of the conversation.</param>
	/// <returns>The <see cref="P:Terraria.GameContent.UI.EmoteBubble.WhoAmI" /> of this emote</returns>
	public static int NewBubbleNPC(WorldUIAnchor bubbleAnchor, int time, WorldUIAnchor other = null)
	{
		if (Main.netMode == 1)
		{
			return -1;
		}
		EmoteBubble emoteBubble = new EmoteBubble(0, bubbleAnchor, time);
		emoteBubble.PickNPCEmote(other);
		if (emoteBubble.emote < 0)
		{
			return -1;
		}
		emoteBubble.ID = AssignNewID();
		byID[emoteBubble.ID] = emoteBubble;
		if (Main.netMode == 2)
		{
			Tuple<int, int> tuple = SerializeNetAnchor(bubbleAnchor);
			NetMessage.SendData(91, -1, -1, null, emoteBubble.ID, tuple.Item1, tuple.Item2, time, emoteBubble.emote, emoteBubble.metadata);
		}
		EmoteBubbleLoader.OnSpawn(emoteBubble);
		return emoteBubble.ID;
	}

	/// <summary>
	/// Try to find a NPC close enough (less than 200 pixels) to react to the emote sent by the player.
	/// </summary>
	/// <param name="emoteID"></param>
	/// <param name="player"></param>
	public static void CheckForNPCsToReactToEmoteBubble(int emoteID, Player player)
	{
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < 200; i++)
		{
			NPC nPC = Main.npc[i];
			if (nPC != null && nPC.active && nPC.aiStyle == 7 && nPC.townNPC && !(nPC.ai[0] >= 2f) && ((player.CanBeTalkedTo && player.Distance(nPC.Center) < 200f) || !Collision.CanHitLine(nPC.Top, 0, 0, player.Top, 0, 0)))
			{
				int direction = (nPC.position.X < player.position.X).ToDirectionInt();
				nPC.ai[0] = 19f;
				nPC.ai[1] = 220f;
				nPC.ai[2] = player.whoAmI;
				nPC.direction = direction;
				nPC.netUpdate = true;
			}
		}
	}

	public EmoteBubble(int emotion, WorldUIAnchor bubbleAnchor, int time = 180)
	{
		anchor = bubbleAnchor;
		emote = emotion;
		lifeTime = time;
		lifeTimeStart = time;
	}

	private void Update()
	{
		lifeTime--;
		if (EmoteBubbleLoader.UpdateFrame(this) && lifeTime > 0 && ++frameCounter >= frameSpeed)
		{
			frameCounter = 0;
			if (++frame >= 2)
			{
				frame = 0;
			}
		}
	}

	private void Draw(SpriteBatch sb)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		//IL_019f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0185: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_0170: Unknown result type (might be due to invalid IL or missing references)
		//IL_02df: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_020e: Unknown result type (might be due to invalid IL or missing references)
		//IL_020f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0216: Unknown result type (might be due to invalid IL or missing references)
		//IL_0220: Unknown result type (might be due to invalid IL or missing references)
		//IL_0227: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d1: Unknown result type (might be due to invalid IL or missing references)
		Texture2D value = TextureAssets.Extra[48].Value;
		SpriteEffects effect = (SpriteEffects)0;
		Vector2 position = GetPosition(out effect);
		position = position.Floor();
		bool flag = lifeTime < 6 || lifeTimeStart - lifeTime < 6;
		Rectangle value2 = value.Frame(8, EMOTE_SHEET_VERTICAL_FRAMES, (!flag) ? 1 : 0);
		Vector2 origin = default(Vector2);
		((Vector2)(ref origin))._002Ector((float)(value2.Width / 2), (float)value2.Height);
		if (Main.player[Main.myPlayer].gravDir == -1f)
		{
			origin.Y = 0f;
			effect = (SpriteEffects)(effect | 2);
			position = Main.ReverseGravitySupport(position);
		}
		Rectangle emoteFrame = (Rectangle)((emote >= 0 && emote < EmoteID.Count) ? value.Frame(8, 39, emote * 2 % 8 + frame, 1 + emote / 4) : new Rectangle(0, 0, 34, 28));
		if (emote == -1)
		{
			position += new Vector2((float)(((Enum)effect).HasFlag((Enum)(object)(SpriteEffects)1) ? 1 : (-1)), (float)(-emoteFrame.Height + 3));
		}
		else
		{
			if (ModEmoteBubble != null)
			{
				value = ModContent.Request<Texture2D>(ModEmoteBubble.Texture).Value;
				emoteFrame = (Rectangle)(((_003F?)EmoteBubbleLoader.GetFrame(this)) ?? value.Frame(2, 1, frame));
			}
			if (!EmoteBubbleLoader.PreDraw(this, sb, value, position, emoteFrame, origin, effect))
			{
				EmoteBubbleLoader.PostDraw(this, sb, value, position, emoteFrame, origin, effect);
				return;
			}
		}
		sb.Draw(TextureAssets.Extra[48].Value, position, (Rectangle?)value2, Color.White, 0f, origin, 1f, effect, 0f);
		if (flag)
		{
			return;
		}
		if (emote >= 0)
		{
			if ((emote == 87 || emote == 89) && ((Enum)effect).HasFlag((Enum)(object)(SpriteEffects)1))
			{
				effect = (SpriteEffects)(effect & -2);
				position.X += 4f;
			}
			sb.Draw(value, position, (Rectangle?)emoteFrame, Color.White, 0f, origin, 1f, effect, 0f);
		}
		else if (emote == -1)
		{
			value = TextureAssets.NpcHead[metadata].Value;
			float num = 1f;
			if ((float)value.Width / 22f > 1f)
			{
				num = 22f / (float)value.Width;
			}
			if ((float)value.Height / 16f > 1f / num)
			{
				num = 16f / (float)value.Height;
			}
			sb.Draw(value, position, (Rectangle?)null, Color.White, 0f, new Vector2((float)(value.Width / 2), 0f), num, effect, 0f);
		}
		EmoteBubbleLoader.PostDraw(this, sb, value, position, emoteFrame, origin, effect);
	}

	private Vector2 GetPosition(out SpriteEffects effect)
	{
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		switch (anchor.type)
		{
		case WorldUIAnchor.AnchorType.Entity:
			effect = (SpriteEffects)(anchor.entity.direction != -1);
			return new Vector2(anchor.entity.Top.X, anchor.entity.VisualPosition.Y) + new Vector2((float)(-anchor.entity.direction * anchor.entity.width) * 0.75f, 2f) - Main.screenPosition;
		case WorldUIAnchor.AnchorType.Pos:
			effect = (SpriteEffects)0;
			return anchor.pos - Main.screenPosition;
		case WorldUIAnchor.AnchorType.Tile:
			effect = (SpriteEffects)0;
			return anchor.pos - Main.screenPosition + new Vector2(0f, (0f - anchor.size.Y) / 2f);
		default:
			effect = (SpriteEffects)0;
			return new Vector2((float)Main.screenWidth, (float)Main.screenHeight) / 2f;
		}
	}

	public static void OnBubbleChange(int bubbleID)
	{
		EmoteBubble emoteBubble = byID[bubbleID];
		if (emoteBubble.anchor.type != 0 || !(emoteBubble.anchor.entity is Player player))
		{
			return;
		}
		foreach (EmoteBubble value in byID.Values)
		{
			if (value.anchor.type == WorldUIAnchor.AnchorType.Entity && value.anchor.entity == player && value.ID != bubbleID)
			{
				value.lifeTime = 6;
			}
		}
	}

	/// <summary>
	/// Send a emote from <see cref="P:Terraria.Main.LocalPlayer" />. Should never be called on server.
	/// </summary>
	/// <param name="emoteId"></param>
	public static void MakeLocalPlayerEmote(int emoteId)
	{
		if (Main.netMode == 0)
		{
			NewBubble(emoteId, new WorldUIAnchor((Entity)Main.LocalPlayer), 360);
			CheckForNPCsToReactToEmoteBubble(emoteId, Main.LocalPlayer);
		}
		else
		{
			NetMessage.SendData(120, -1, -1, null, Main.myPlayer, emoteId);
		}
	}

	public void PickNPCEmote(WorldUIAnchor other = null)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		Player plr = Main.player[Player.FindClosest(((NPC)anchor.entity).Center, 0, 0)];
		List<int> list = new List<int>();
		bool flag = false;
		for (int i = 0; i < 200; i++)
		{
			if (Main.npc[i].active && Main.npc[i].boss)
			{
				flag = true;
			}
		}
		if (!flag)
		{
			if (Main.rand.Next(3) == 0)
			{
				ProbeTownNPCs(list);
			}
			if (Main.rand.Next(3) == 0)
			{
				ProbeEmotions(list);
			}
			if (Main.rand.Next(3) == 0)
			{
				ProbeBiomes(list, plr);
			}
			if (Main.rand.Next(2) == 0)
			{
				ProbeCritters(list);
			}
			if (Main.rand.Next(2) == 0)
			{
				ProbeItems(list, plr);
			}
			if (Main.rand.Next(5) == 0)
			{
				ProbeBosses(list);
			}
			if (Main.rand.Next(2) == 0)
			{
				ProbeDebuffs(list, plr);
			}
			if (Main.rand.Next(2) == 0)
			{
				ProbeEvents(list);
			}
			if (Main.rand.Next(2) == 0)
			{
				ProbeWeather(list, plr);
			}
			ProbeExceptions(list, plr, other);
		}
		else
		{
			ProbeCombat(list);
		}
		if (other == null)
		{
			other = new WorldUIAnchor();
		}
		int? modPickedEmote = NPCLoader.PickEmote((NPC)anchor.entity, plr, list, other);
		if (modPickedEmote == -1)
		{
			emote = -1;
		}
		else if (modPickedEmote.HasValue)
		{
			emote = modPickedEmote.Value;
		}
		else if (list.Count > 0)
		{
			emote = list[Main.rand.Next(list.Count)];
		}
	}

	private void ProbeCombat(List<int> list)
	{
		list.Add(16);
		list.Add(1);
		list.Add(2);
		list.Add(91);
		list.Add(93);
		list.Add(84);
		list.Add(84);
	}

	private void ProbeWeather(List<int> list, Player plr)
	{
		if (Main.cloudBGActive > 0f)
		{
			list.Add(96);
		}
		if (Main.cloudAlpha > 0f)
		{
			if (!Main.dayTime)
			{
				list.Add(5);
			}
			list.Add(4);
			if (plr.ZoneSnow)
			{
				list.Add(98);
			}
			if (plr.position.X < 4000f || (plr.position.X > (float)(Main.maxTilesX * 16 - 4000) && (double)plr.position.Y < Main.worldSurface / 16.0))
			{
				list.Add(97);
			}
		}
		else
		{
			list.Add(95);
		}
		if (plr.ZoneHallow)
		{
			list.Add(6);
		}
	}

	private void ProbeEvents(List<int> list)
	{
		if (BirthdayParty.PartyIsUp && Main.rand.Next(3) == 0)
		{
			list.Add(Utils.SelectRandom<int>(Main.rand, 127, 128, 129, 126));
		}
		if (Main.bloodMoon || (!Main.dayTime && Main.rand.Next(4) == 0))
		{
			list.Add(18);
		}
		if (Main.eclipse || (Main.hardMode && Main.rand.Next(4) == 0))
		{
			list.Add(19);
		}
		if ((!Main.dayTime || WorldGen.spawnMeteor) && NPC.downedBoss2)
		{
			list.Add(99);
		}
		if (Main.pumpkinMoon || ((NPC.downedHalloweenKing || NPC.downedHalloweenTree) && !Main.dayTime))
		{
			list.Add(20);
		}
		if (Main.snowMoon || ((NPC.downedChristmasIceQueen || NPC.downedChristmasSantank || NPC.downedChristmasTree) && !Main.dayTime))
		{
			list.Add(21);
		}
		if (DD2Event.Ongoing || DD2Event.DownedInvasionAnyDifficulty)
		{
			list.Add(133);
		}
	}

	private void ProbeDebuffs(List<int> list, Player plr)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		if (plr.Center.Y > (float)(Main.maxTilesY * 16 - 3200) || plr.onFire || ((NPC)anchor.entity).onFire || plr.onFire2)
		{
			list.Add(9);
		}
		if (Main.rand.Next(2) == 0)
		{
			list.Add(11);
		}
		if (plr.poisoned || ((NPC)anchor.entity).poisoned || plr.ZoneJungle)
		{
			list.Add(8);
		}
		if (plr.inventory[plr.selectedItem].type == 215 || Main.rand.Next(3) == 0)
		{
			list.Add(10);
		}
	}

	private void ProbeItems(List<int> list, Player plr)
	{
		list.Add(7);
		list.Add(73);
		list.Add(74);
		list.Add(75);
		list.Add(78);
		list.Add(90);
		if (plr.statLife < plr.statLifeMax2 / 2)
		{
			list.Add(84);
		}
	}

	private void ProbeTownNPCs(List<int> list)
	{
		for (int i = 0; i < NPCLoader.NPCCount; i++)
		{
			CountNPCs[i] = 0;
		}
		for (int j = 0; j < 200; j++)
		{
			if (Main.npc[j].active)
			{
				CountNPCs[Main.npc[j].type]++;
			}
		}
		int type = ((NPC)anchor.entity).type;
		for (int k = 0; k < NPCLoader.NPCCount; k++)
		{
			if (NPCID.Sets.FaceEmote[k] > 0 && CountNPCs[k] > 0 && k != type)
			{
				list.Add(NPCID.Sets.FaceEmote[k]);
			}
		}
	}

	private void ProbeBiomes(List<int> list, Player plr)
	{
		if ((double)(plr.position.Y / 16f) < Main.worldSurface * 0.45)
		{
			list.Add(22);
		}
		else if ((double)(plr.position.Y / 16f) > Main.rockLayer + (double)(Main.maxTilesY / 2) - 100.0)
		{
			list.Add(31);
		}
		else if ((double)(plr.position.Y / 16f) > Main.rockLayer)
		{
			list.Add(30);
		}
		else if (plr.ZoneHallow)
		{
			list.Add(27);
		}
		else if (plr.ZoneCorrupt)
		{
			list.Add(26);
		}
		else if (plr.ZoneCrimson)
		{
			list.Add(25);
		}
		else if (plr.ZoneJungle)
		{
			list.Add(24);
		}
		else if (plr.ZoneSnow)
		{
			list.Add(32);
		}
		else if ((double)(plr.position.Y / 16f) < Main.worldSurface && (plr.position.X < 4000f || plr.position.X > (float)(16 * (Main.maxTilesX - 250))))
		{
			list.Add(29);
		}
		else if (plr.ZoneDesert)
		{
			list.Add(28);
		}
		else if (plr.ZoneForest)
		{
			list.Add(23);
		}
	}

	private void ProbeCritters(List<int> list)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		Vector2 center = anchor.entity.Center;
		float num = 1f;
		float num2 = 1f;
		if ((double)center.Y < Main.rockLayer * 16.0)
		{
			num2 = 0.2f;
		}
		else
		{
			num = 0.2f;
		}
		if (Main.rand.NextFloat() <= num)
		{
			if (Main.dayTime)
			{
				list.Add(13);
				list.Add(12);
				list.Add(68);
				list.Add(62);
				list.Add(63);
				list.Add(69);
				list.Add(70);
			}
			if (!Main.dayTime || (Main.dayTime && (Main.time < 5400.0 || Main.time > 48600.0)))
			{
				list.Add(61);
			}
			if (NPC.downedGoblins)
			{
				list.Add(64);
			}
			if (NPC.downedFrost)
			{
				list.Add(66);
			}
			if (NPC.downedPirates)
			{
				list.Add(65);
			}
			if (NPC.downedMartians)
			{
				list.Add(71);
			}
			if (WorldGen.crimson)
			{
				list.Add(67);
			}
		}
		if (Main.rand.NextFloat() <= num2)
		{
			list.Add(72);
			list.Add(69);
		}
	}

	private void ProbeEmotions(List<int> list)
	{
		list.Add(0);
		list.Add(1);
		list.Add(2);
		list.Add(3);
		list.Add(15);
		list.Add(16);
		list.Add(17);
		list.Add(87);
		list.Add(91);
		list.Add(136);
		list.Add(134);
		list.Add(135);
		list.Add(137);
		list.Add(138);
		list.Add(139);
		if (Main.bloodMoon && !Main.dayTime)
		{
			int item = Utils.SelectRandom<int>(Main.rand, 16, 1, 138);
			list.Add(item);
			list.Add(item);
			list.Add(item);
		}
	}

	private void ProbeBosses(List<int> list)
	{
		int num = 0;
		if ((!NPC.downedBoss1 && !Main.dayTime) || NPC.downedBoss1)
		{
			num = 1;
		}
		if (NPC.downedBoss2)
		{
			num = 2;
		}
		if (NPC.downedQueenBee || NPC.downedBoss3)
		{
			num = 3;
		}
		if (Main.hardMode)
		{
			num = 4;
		}
		if (NPC.downedMechBossAny)
		{
			num = 5;
		}
		if (NPC.downedPlantBoss)
		{
			num = 6;
		}
		if (NPC.downedGolemBoss)
		{
			num = 7;
		}
		if (NPC.downedAncientCultist)
		{
			num = 8;
		}
		int maxValue = 10;
		if (NPC.downedMoonlord)
		{
			maxValue = 1;
		}
		if ((num >= 1 && num <= 2) || (num >= 1 && Main.rand.Next(maxValue) == 0))
		{
			list.Add(39);
			if (WorldGen.crimson)
			{
				list.Add(41);
			}
			else
			{
				list.Add(40);
			}
			list.Add(51);
		}
		if ((num >= 2 && num <= 3) || (num >= 2 && Main.rand.Next(maxValue) == 0))
		{
			list.Add(43);
			list.Add(42);
		}
		if ((num >= 4 && num <= 5) || (num >= 4 && Main.rand.Next(maxValue) == 0))
		{
			list.Add(44);
			list.Add(47);
			list.Add(45);
			list.Add(46);
		}
		if ((num >= 5 && num <= 6) || (num >= 5 && Main.rand.Next(maxValue) == 0))
		{
			if (!NPC.downedMechBoss1)
			{
				list.Add(47);
			}
			if (!NPC.downedMechBoss2)
			{
				list.Add(45);
			}
			if (!NPC.downedMechBoss3)
			{
				list.Add(46);
			}
			list.Add(48);
		}
		if (num == 6 || (num >= 6 && Main.rand.Next(maxValue) == 0))
		{
			list.Add(48);
			list.Add(49);
			list.Add(50);
		}
		if (num == 7 || (num >= 7 && Main.rand.Next(maxValue) == 0))
		{
			list.Add(49);
			list.Add(50);
			list.Add(52);
		}
		if (num == 8 || (num >= 8 && Main.rand.Next(maxValue) == 0))
		{
			list.Add(52);
			list.Add(53);
		}
		if (NPC.downedPirates && Main.expertMode)
		{
			list.Add(59);
		}
		if (NPC.downedMartians)
		{
			list.Add(60);
		}
		if (NPC.downedChristmasIceQueen)
		{
			list.Add(57);
		}
		if (NPC.downedChristmasSantank)
		{
			list.Add(58);
		}
		if (NPC.downedChristmasTree)
		{
			list.Add(56);
		}
		if (NPC.downedHalloweenKing)
		{
			list.Add(55);
		}
		if (NPC.downedHalloweenTree)
		{
			list.Add(54);
		}
		if (NPC.downedEmpressOfLight)
		{
			list.Add(143);
		}
		if (NPC.downedQueenSlime)
		{
			list.Add(144);
		}
		if (NPC.downedDeerclops)
		{
			list.Add(150);
		}
	}

	private void ProbeExceptions(List<int> list, Player plr, WorldUIAnchor other)
	{
		NPC nPC = (NPC)anchor.entity;
		if (nPC.type == 17)
		{
			list.Add(80);
			list.Add(85);
			list.Add(85);
			list.Add(85);
			list.Add(85);
		}
		else if (nPC.type == 18)
		{
			list.Add(73);
			list.Add(73);
			list.Add(84);
			list.Add(75);
		}
		else if (nPC.type == 19)
		{
			if (other != null && ((NPC)other.entity).type == 22)
			{
				list.Add(1);
				list.Add(1);
				list.Add(93);
				list.Add(92);
			}
			else if (other != null && ((NPC)other.entity).type == 22)
			{
				list.Add(1);
				list.Add(1);
				list.Add(93);
				list.Add(92);
			}
			else
			{
				list.Add(82);
				list.Add(82);
				list.Add(85);
				list.Add(85);
				list.Add(77);
				list.Add(93);
			}
		}
		else if (nPC.type == 20)
		{
			if (list.Contains(121))
			{
				list.Add(121);
				list.Add(121);
			}
			list.Add(14);
			list.Add(14);
		}
		else if (nPC.type == 22)
		{
			if (!Main.bloodMoon)
			{
				if (other != null && ((NPC)other.entity).type == 19)
				{
					list.Add(1);
					list.Add(1);
					list.Add(93);
					list.Add(92);
				}
				else
				{
					list.Add(79);
				}
			}
			if (!Main.dayTime)
			{
				list.Add(16);
				list.Add(16);
				list.Add(16);
			}
		}
		else if (nPC.type == 37)
		{
			list.Add(43);
			list.Add(43);
			list.Add(43);
			list.Add(72);
			list.Add(72);
		}
		else if (nPC.type == 38)
		{
			if (Main.bloodMoon)
			{
				list.Add(77);
				list.Add(77);
				list.Add(77);
				list.Add(81);
			}
			else
			{
				list.Add(77);
				list.Add(77);
				list.Add(81);
				list.Add(81);
				list.Add(81);
				list.Add(90);
				list.Add(90);
			}
		}
		else if (nPC.type == 54)
		{
			if (Main.bloodMoon)
			{
				list.Add(43);
				list.Add(72);
				list.Add(1);
			}
			else
			{
				if (list.Contains(111))
				{
					list.Add(111);
				}
				list.Add(17);
			}
		}
		else if (nPC.type == 107)
		{
			if (other != null && ((NPC)other.entity).type == 124)
			{
				list.Remove(111);
				list.Add(0);
				list.Add(0);
				list.Add(0);
				list.Add(17);
				list.Add(17);
				list.Add(86);
				list.Add(88);
				list.Add(88);
			}
			else
			{
				if (list.Contains(111))
				{
					list.Add(111);
					list.Add(111);
					list.Add(111);
				}
				list.Add(91);
				list.Add(92);
				list.Add(91);
				list.Add(92);
			}
		}
		else if (nPC.type == 108)
		{
			list.Add(100);
			list.Add(89);
			list.Add(11);
		}
		if (nPC.type == 124)
		{
			if (other != null && ((NPC)other.entity).type == 107)
			{
				list.Remove(111);
				list.Add(0);
				list.Add(0);
				list.Add(0);
				list.Add(17);
				list.Add(17);
				list.Add(88);
				list.Add(88);
				return;
			}
			if (list.Contains(109))
			{
				list.Add(109);
				list.Add(109);
				list.Add(109);
			}
			if (list.Contains(108))
			{
				list.Remove(108);
				if (Main.hardMode)
				{
					list.Add(108);
					list.Add(108);
				}
				else
				{
					list.Add(106);
					list.Add(106);
				}
			}
			list.Add(43);
			list.Add(2);
		}
		else if (nPC.type == 142)
		{
			list.Add(32);
			list.Add(66);
			list.Add(17);
			list.Add(15);
			list.Add(15);
		}
		else if (nPC.type == 160)
		{
			list.Add(10);
			list.Add(89);
			list.Add(94);
			list.Add(8);
		}
		else if (nPC.type == 178)
		{
			list.Add(83);
			list.Add(83);
		}
		else if (nPC.type == 207)
		{
			list.Add(28);
			list.Add(95);
			list.Add(93);
		}
		else if (nPC.type == 208)
		{
			list.Add(94);
			list.Add(17);
			list.Add(3);
			list.Add(77);
		}
		else if (nPC.type == 209)
		{
			list.Add(48);
			list.Add(83);
			list.Add(5);
			list.Add(5);
		}
		else if (nPC.type == 227)
		{
			list.Add(63);
			list.Add(68);
		}
		else if (nPC.type == 228)
		{
			list.Add(24);
			list.Add(24);
			list.Add(95);
			list.Add(8);
		}
		else if (nPC.type == 229)
		{
			list.Add(93);
			list.Add(9);
			list.Add(65);
			list.Add(120);
			list.Add(59);
		}
		else if (nPC.type == 353)
		{
			if (list.Contains(104))
			{
				list.Add(104);
				list.Add(104);
			}
			if (list.Contains(111))
			{
				list.Add(111);
				list.Add(111);
			}
			list.Add(67);
		}
		else if (nPC.type == 368)
		{
			list.Add(85);
			list.Add(7);
			list.Add(79);
		}
		else if (nPC.type == 369)
		{
			if (!Main.bloodMoon)
			{
				list.Add(70);
				list.Add(70);
				list.Add(76);
				list.Add(76);
				list.Add(79);
				list.Add(79);
				if ((double)nPC.position.Y < Main.worldSurface)
				{
					list.Add(29);
				}
			}
		}
		else if (nPC.type == 453)
		{
			list.Add(72);
			list.Add(69);
			list.Add(87);
			list.Add(3);
		}
		else if (nPC.type == 441)
		{
			list.Add(100);
			list.Add(100);
			list.Add(1);
			list.Add(1);
			list.Add(1);
			list.Add(87);
		}
	}

	/// <summary>
	/// Gets the emote bubble that exists in the world by <see cref="P:Terraria.GameContent.UI.EmoteBubble.WhoAmI" />. Returns null if there is no corresponding emote
	/// </summary>
	/// <param name="whoAmI"></param>
	/// <returns></returns>
	public static EmoteBubble GetExistingEmoteBubble(int whoAmI)
	{
		return byID.GetValueOrDefault(whoAmI);
	}

	/// <summary>
	/// Send a emote from the player
	/// </summary>
	/// <param name="player"></param>
	/// <param name="emoteId"></param>
	/// <param name="syncBetweenClients">If true, this emote will be automatically synchronized between clients</param>
	public static void MakePlayerEmote(Player player, int emoteId, bool syncBetweenClients = true)
	{
		int netMode = Main.netMode;
		if (netMode == 2 || netMode == 0 || !syncBetweenClients)
		{
			NewBubble(emoteId, new WorldUIAnchor((Entity)player), 360);
			CheckForNPCsToReactToEmoteBubble(emoteId, player);
		}
		else
		{
			NetMessage.SendData(120, -1, -1, null, player.whoAmI, emoteId);
		}
	}
}
