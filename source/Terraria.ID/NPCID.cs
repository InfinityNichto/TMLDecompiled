using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using ReLogic.Reflection;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace Terraria.ID;

public class NPCID
{
	public static class Sets
	{
		/// <summary>
		/// Stores the draw parameters for an NPC type (<see cref="F:Terraria.NPC.type" />) in the Bestiary.
		/// <br /> <strong>Do not use <see langword="default" /> to create this struct.</strong> Use <see langword="new" /> <see cref="M:Terraria.ID.NPCID.Sets.NPCBestiaryDrawModifiers.#ctor" /> instead to set proper default values.
		/// </summary>
		public struct NPCBestiaryDrawModifiers
		{
			/// <summary>
			/// The offset of this <see cref="T:Terraria.NPC" />'s Bestiary image in pixels.
			/// <br /> Defaults to <see cref="P:Microsoft.Xna.Framework.Vector2.Zero" />.
			/// </summary>
			public Vector2 Position;

			/// <summary>
			/// A custom value for <see cref="F:Terraria.ID.NPCID.Sets.NPCBestiaryDrawModifiers.Position" />.X to use when drawing this <see cref="T:Terraria.NPC" />'s Bestiary portrait. If <see langword="null" />, then do not override the value.
			/// <br /> Defaults to <see langword="null" />.
			/// </summary>
			public float? PortraitPositionXOverride;

			/// <summary>
			/// A custom value for <see cref="F:Terraria.ID.NPCID.Sets.NPCBestiaryDrawModifiers.Position" />.Y to use when drawing this <see cref="T:Terraria.NPC" />'s Bestiary portrait. If <see langword="null" />, then do not override the value.
			/// <br /> Defaults to <see langword="null" />.
			/// </summary>
			public float? PortraitPositionYOverride;

			/// <summary>
			/// The clockwise rotation of this <see cref="T:Terraria.NPC" />'s Bestiary image in radians.
			/// <br /> Defaults to <c>0f</c>.
			/// </summary>
			public float Rotation;

			/// <summary>
			/// The visual scale of this <see cref="T:Terraria.NPC" />'s Bestiary image.
			/// <br /> Defaults to <c>1f</c>.
			/// </summary>
			public float Scale;

			/// <summary>
			/// A custom value for <see cref="F:Terraria.ID.NPCID.Sets.NPCBestiaryDrawModifiers.Scale" /> to use when drawing this <see cref="T:Terraria.NPC" />'s Bestiary portrait. If <see langword="null" />, then do not override the value.
			/// <br /> Defaults to <see langword="null" />.
			/// </summary>
			public float? PortraitScale;

			/// <summary>
			/// If <see langword="true" />, this <see cref="T:Terraria.NPC" /> will never show up in the Bestiary.
			/// <br /> Useful for multi-segment <see cref="T:Terraria.NPC" />s.
			/// <br /> Defaults to <see langword="false" />.
			/// </summary>
			public bool Hide;

			/// <summary>
			/// The specific <see cref="F:Terraria.Entity.wet" /> to use when drawing this <see cref="T:Terraria.NPC" />'s Bestiary image.
			/// <br /> Useful for <see cref="T:Terraria.NPC" />s that draw differently when wet.
			/// <br /> Defaults to <see langword="false" />.
			/// </summary>
			public bool IsWet;

			/// <summary>
			/// The specific vertical frame to use when drawing this <see cref="T:Terraria.NPC" /> in the Bestiary.
			/// <br /> If <see langword="null" />, then this <see cref="T:Terraria.NPC" /> will play a default walking animation.
			/// <br /> Defaults to <see langword="null" />.
			/// </summary>
			public int? Frame;

			/// <summary>
			/// The specific <see cref="F:Terraria.Entity.direction" /> to use when drawing this <see cref="T:Terraria.NPC" />'s Bestiary image.
			/// <br /> If <see langword="null" />, then <c>-1</c> is used.
			/// <br /> Defaults to <see langword="null" />.
			/// </summary>
			public int? Direction;

			/// <summary>
			/// The specific <see cref="F:Terraria.NPC.spriteDirection" /> to use when drawing this <see cref="T:Terraria.NPC" />'s Bestiary image.
			/// <br /> If <see langword="null" />, then the value of <see cref="F:Terraria.ID.NPCID.Sets.NPCBestiaryDrawModifiers.Direction" /> is used.
			/// <br /> Defaults to <see langword="null" />.
			/// </summary>
			public int? SpriteDirection;

			/// <summary>
			/// The magnitude of <see cref="F:Terraria.Entity.velocity" />.X to use when drawing this <see cref="T:Terraria.NPC" />'s Bestiary image, either when in Portrait mode or when hovered.
			/// <br /> Defaults to <c>0f</c>.
			/// </summary>
			/// <remarks>This value is only used if <see cref="F:Terraria.ID.NPCID.Sets.NPCBestiaryDrawModifiers.Frame" /> is <see langword="null" />.</remarks>
			public float Velocity;

			/// <summary>
			/// The path to the custom texture to use when drawing this <see cref="T:Terraria.NPC" />'s Bestiary image.
			/// <br /> If <see langword="null" />, then this NPC's default texture is used.
			/// <br /> Defaults to <see langword="null" />.
			/// </summary>
			public string CustomTexturePath;

			/// <summary>
			/// Creates a new <see cref="T:Terraria.ID.NPCID.Sets.NPCBestiaryDrawModifiers" /> with default values.
			/// </summary>
			/// <param name="seriouslyWhyCantStructsHaveParameterlessConstructors">Unused.</param>
			[Obsolete("Use the no argument constructor instead")]
			public NPCBestiaryDrawModifiers(int seriouslyWhyCantStructsHaveParameterlessConstructors)
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				Position = default(Vector2);
				Rotation = 0f;
				Scale = 1f;
				PortraitScale = 1f;
				Hide = false;
				IsWet = false;
				Frame = null;
				Direction = null;
				SpriteDirection = null;
				Velocity = 0f;
				PortraitPositionXOverride = null;
				PortraitPositionYOverride = null;
				CustomTexturePath = null;
			}

			/// <inheritdoc cref="M:Terraria.ID.NPCID.Sets.NPCBestiaryDrawModifiers.#ctor(System.Int32)" />
			public NPCBestiaryDrawModifiers()
				: this(0)
			{
			}
		}

		private static class LocalBuffID
		{
			public const int Confused = 31;

			public const int Poisoned = 20;

			public const int OnFire = 24;

			public const int OnFire3 = 323;

			public const int ShadowFlame = 153;

			public const int Daybreak = 189;

			public const int Frostburn = 44;

			public const int Frostburn2 = 324;

			public const int CursedInferno = 39;

			public const int BetsysCurse = 203;

			public const int Ichor = 69;

			public const int Venom = 70;

			public const int Oiled = 204;

			public const int BoneJavelin = 169;

			public const int TentacleSpike = 337;

			public const int BloodButcherer = 344;
		}

		public static SetFactory Factory;

		/// <summary>
		/// Determines the special spawning rules for an NPC to use when respawning using the coin loss system (<see cref="T:Terraria.GameContent.CoinLossRevengeSystem" />).
		/// <br /> If <c>0</c> for a given NPC type (<see cref="F:Terraria.NPC.type" />), then <c><see cref="F:Terraria.NPC.ai" />[0]</c> and <c><see cref="F:Terraria.NPC.ai" />[1]</c> are set to this <see cref="T:Terraria.NPC" />'s position in tile coordinates.
		/// </summary>
		/// <remarks>All vanilla entries in this set use <c>0</c>. All vanilla entries in this set are tethered to tiles (<see cref="F:Terraria.ID.NPCID.ManEater" />, <see cref="F:Terraria.ID.NPCID.Snatcher" />, etc.).</remarks>
		public static Dictionary<int, int> SpecialSpawningRules;

		/// <summary>
		/// The settings to use for a given NPC type's (<see cref="F:Terraria.NPC.type" />) Bestiary drawing.
		/// </summary>
		public static Dictionary<int, NPCBestiaryDrawModifiers> NPCBestiaryDrawOffset;

		/// <summary>
		/// <b>Unused:</b> Replaced by <see cref="F:Terraria.ID.NPCID.Sets.SpecificDebuffImmunity" />, <see cref="F:Terraria.ID.NPCID.Sets.ImmuneToAllBuffs" />, and <see cref="F:Terraria.ID.NPCID.Sets.ImmuneToRegularBuffs" /><br /><br />
		/// The default debuff immunities for a given NPC type (<see cref="F:Terraria.NPC.type" />).
		/// </summary>
		/// <remarks><see cref="F:Terraria.NPC.buffImmune" /> can still be manually changed to grant temporary immunity.</remarks>
		private static Dictionary<int, NPCDebuffImmunityData> DebuffImmunitySets;

		/// <summary>
		/// The order of critter NPC types (<see cref="F:Terraria.NPC.type" />) in the Bestiary.
		/// </summary>
		public static List<int> NormalGoldCritterBestiaryPriority;

		/// <summary>
		/// The order of boss NPC types (<see cref="F:Terraria.NPC.type" />) in the Bestiary.
		/// </summary>
		public static List<int> BossBestiaryPriority;

		/// <summary>
		/// The order of town NPC NPC types (<see cref="F:Terraria.NPC.type" />) in the Bestiary.
		/// </summary>
		public static List<int> TownNPCBestiaryPriority;

		/// <summary>
		/// If <see langword="true" /> for a given NPC type (<see cref="F:Terraria.NPC.type" />), then that NPC will not have its stats increased in <see cref="P:Terraria.Main.expertMode" /> or higher difficulties.
		/// <br /> Defaults to <see langword="false" />.
		/// </summary>
		public static bool[] DontDoHardmodeScaling;

		/// <summary>
		/// If <see langword="true" /> for a given NPC type (<see cref="F:Terraria.NPC.type" />), then that NPC will reflect <see cref="F:Terraria.ID.ProjectileID.StarCannonStar" /> and <see cref="F:Terraria.ID.ProjectileID.SuperStar" /> on the "For the Worthy" secret seed.
		/// <br /> Defaults to <see langword="false" />.
		/// </summary>
		public static bool[] ReflectStarShotsInForTheWorthy;

		/// <summary>
		/// If <see langword="true" /> for a given NPC type (<see cref="F:Terraria.NPC.type" />), then that NPC is categorized as a town pet.
		/// <br /> Town pets must have <c><see cref="F:Terraria.NPC.aiStyle" /> == <see cref="F:Terraria.ID.NPCAIStyleID.Passive" /></c> to function properly.
		/// <br /> Town pets cannot party, can be pet, can be moved into valid rooms even if they contain a stinkbug, and "leave" on death.
		/// <br /> Defaults to <see langword="false" />.
		/// </summary>
		public static bool[] IsTownPet;

		/// <summary>
		/// If <see langword="true" /> for a given NPC type (<see cref="F:Terraria.NPC.type" />), then that NPC is categorized as a town slime.
		/// <br /> Town slimes must have <c><see cref="F:Terraria.NPC.aiStyle" /> == <see cref="F:Terraria.ID.NPCAIStyleID.Passive" /></c> to function properly. Additionally, they should also be in the <see cref="F:Terraria.ID.NPCID.Sets.IsTownPet" /> set.
		/// <br /> Town slimes cannot sit on chairs, will try to play their idle animations more often, and have horizontally flipped sprites compared to normal town NPCs.
		/// <br /> Defaults to <see langword="false" />.
		/// </summary>
		public static bool[] IsTownSlime;

		/// <summary>
		/// If <see langword="true" /> for a given NPC type (<see cref="F:Terraria.NPC.type" />), then that NPC can pick up <see cref="F:Terraria.ID.ItemID.CopperShortsword" /> or <see cref="F:Terraria.ID.ItemID.CopperHelmet" /> to become <see cref="F:Terraria.ID.NPCID.TownSlimeCopper" />.
		/// <br /> Defaults to <see langword="false" />.
		/// </summary>
		public static bool[] CanConvertIntoCopperSlimeTownNPC;

		/// <summary>
		/// A list of all NPC types (<see cref="F:Terraria.NPC.type" />) categorized as gold critters.
		/// <br /> If a gold critter shows up on the Lifeform Analyzer, its name will be gold (<see cref="M:Terraria.Main.DrawInfoAccs_AdjustInfoTextColorsForNPC(Terraria.NPC,Microsoft.Xna.Framework.Color@,Microsoft.Xna.Framework.Color@)" />).
		/// </summary>
		public static List<int> GoldCrittersCollection;

		/// <summary>
		/// If <see langword="true" /> for a given NPC type (<see cref="F:Terraria.NPC.type" />), then that NPC, if currently <see cref="F:Terraria.NPC.dontTakeDamage" />, will damage the player when hit with a melee attack, a whip, certain <see cref="T:Terraria.ID.ProjAIStyleID" />s (<see cref="F:Terraria.ID.ProjAIStyleID.Spear" />, <see cref="F:Terraria.ID.ProjAIStyleID.ShortSword" />, <see cref="F:Terraria.ID.ProjAIStyleID.SleepyOctopod" />, <see cref="F:Terraria.ID.ProjAIStyleID.HeldProjectile" />), or by any projectile type (<see cref="F:Terraria.Projectile.type" />) in the <see cref="F:Terraria.ID.ProjectileID.Sets.AllowsContactDamageFromJellyfish" /> or <see cref="F:Terraria.ID.ProjectileID.Sets.IsAWhip" /> sets.
		/// <br /> Defaults to <see langword="false" />.
		/// </summary>
		public static bool[] ZappingJellyfish;

		/// <summary>
		/// If <see langword="true" /> for a given NPC type (<see cref="F:Terraria.NPC.type" />), then that NPC cannot pick up dropped coins in Expert Mode.
		/// <br /> Defaults to <see langword="false" />.
		/// </summary>
		public static bool[] CantTakeLunchMoney;

		/// <summary>
		/// Associates an NPC type (<see cref="F:Terraria.NPC.type" />) with the NPC type it respawns as from the coin loss system (<see cref="T:Terraria.GameContent.CoinLossRevengeSystem" />).
		/// <br /> If an NPC type is not a key in this dictionary, then that NPC respawns as itself.
		/// <br /> If the value for an NPC type is <c>0</c>, then that NPC won't be cached (<see cref="M:Terraria.GameContent.CoinLossRevengeSystem.CacheEnemy(Terraria.NPC)" />).
		/// </summary>
		public static Dictionary<int, int> RespawnEnemyID;

		/// <summary>
		/// If <c>!= -1</c> for a given NPC type (<see cref="F:Terraria.NPC.type" />), then that NPC will store its past positions (and potentially more) for use in a trail.
		/// <br /> A value of <c>0</c> will store the NPC's position in <see cref="F:Terraria.NPC.oldPos" /> every three frames. <strong>Vanilla automatically uses <see cref="F:Terraria.NPC.localAI" />[3] to count frames for this, so make sure your code doesn't interfere with that.</strong>
		/// <br /> A value of <c>1</c>, <c>5</c>, or <c>6</c> will store the NPC's position in <see cref="F:Terraria.NPC.oldPos" /> every frame.
		/// <br /> A value of <c>2</c> will store the NPC's position and rotation in <see cref="F:Terraria.NPC.oldPos" /> and <see cref="F:Terraria.NPC.oldRot" /> respectively every frame <strong>if</strong> <c><see cref="F:Terraria.NPC.ai" />[0] == 4 or 5 or 6</c>.
		/// <br /> A value of <c>3</c> or <c>7</c> will store the NPC's position and rotation in <see cref="F:Terraria.NPC.oldPos" /> and <see cref="F:Terraria.NPC.oldRot" /> respectively every frame.
		/// <br /> A value of <c>4</c> will store the NPC's position in <see cref="F:Terraria.NPC.oldPos" /> every frame, as well as creating light (0x4D0033) at the NPC's position.
		/// <br /> Other values will do nothing.
		/// <br /> <strong>Vanilla will not automatically draw trails for you.</strong>
		/// <br /> Defaults to <c>-1</c>.
		/// </summary>
		/// <remarks>The length of an NPC's trail is determined by <see cref="F:Terraria.ID.NPCID.Sets.TrailCacheLength" />.</remarks>
		public static int[] TrailingMode;

		/// <summary>
		/// If <see langword="true" /> for a given NPC type (<see cref="F:Terraria.NPC.type" />), then that NPC is categorized as a dragonfly.
		/// <br /> Defaults to <see langword="false" />.
		/// </summary>
		/// <remarks>Vanilla only uses this set to ensure the <see cref="F:Terraria.ID.ItemID.DragonflyStatue" /> doesn't spawn too many dragonflies without checking each type individually. It's still recommended to add your NPC to this set if it applies.</remarks>
		public static bool[] IsDragonfly;

		/// <summary>
		/// If <see langword="true" /> for a given NPC type (<see cref="F:Terraria.NPC.type" />), then that NPC belongs to the Old One's Army event.
		/// <br /> If any NPC in this set is alive, the OOA music will play. Additionally, all NPCs in this set will be erased when the OOA ends.
		/// <br /> NPCs in this set will target the <see cref="F:Terraria.ID.NPCID.DD2EterniaCrystal" /> if it is alive (using <see cref="M:Terraria.Utilities.NPCUtils.TargetClosestOldOnesInvasion(Terraria.NPC,System.Boolean,System.Nullable{Microsoft.Xna.Framework.Vector2})" />).
		/// <br /> Defaults to <see langword="false" />.
		/// </summary>
		public static bool[] BelongsToInvasionOldOnesArmy;

		/// <summary>
		/// If <see langword="true" /> for a given NPC type (<see cref="F:Terraria.NPC.type" />), then that NPC cannot be teleported using <see cref="F:Terraria.ID.TileID.Teleporter" />s.
		/// <br /> Defaults to <see langword="false" />.
		/// </summary>
		/// <remarks>
		/// Bosses and tile-phasing (<see cref="F:Terraria.NPC.noTileCollide" />) NPCs can't be teleported at all, regardless of this set.
		/// </remarks>
		public static bool[] TeleportationImmune;

		/// <summary>
		/// If <see langword="true" /> for a given NPC type (<see cref="F:Terraria.NPC.type" />), then that NPC can target other NPCs.
		/// <br /> This set does <strong>not</strong> automatically handle NPC targeting. You should use <see cref="M:Terraria.Utilities.NPCUtils.SearchForTarget(Terraria.NPC,Terraria.Utilities.NPCUtils.TargetSearchFlag,Terraria.Utilities.NPCUtils.SearchFilter{Terraria.Player},Terraria.Utilities.NPCUtils.SearchFilter{Terraria.NPC})" /> or any of its overloads to write your own targeting function.
		/// <br /> Defaults to <see langword="false" />.
		/// </summary>
		/// <remarks>
		/// You can check if an NPC is in this set using <see cref="P:Terraria.NPC.SupportsNPCTargets" />.
		/// <br /> You can use <see cref="M:Terraria.NPC.GetTargetData(System.Boolean)" /> to get info about a target, regardless of if that target is a <see cref="T:Terraria.Player" /> or <see cref="T:Terraria.NPC" />.
		/// </remarks>
		public static bool[] UsesNewTargetting;

		/// <summary>
		/// If <see langword="true" /> for a given NPC type (<see cref="F:Terraria.NPC.type" />), then that NPC can take damage from hostile NPCs without being friendly.
		/// <br /> Used in vanilla for critters and trapped town slimes.
		/// <br /> Defaults to <see langword="false" />.
		/// </summary>
		public static bool[] TakesDamageFromHostilesWithoutBeingFriendly;

		/// <summary>
		/// An array of length <see cref="P:Terraria.ModLoader.NPCLoader.NPCCount" /> with only <see langword="true" /> entries.
		/// <br /> Used for methods that take sets as parameters, like <see cref="M:Terraria.NPC.GetHurtByOtherNPCs(System.Boolean[])" />.
		/// </summary>
		public static bool[] AllNPCs;

		/// <summary>
		/// If <see langword="true" /> for a given NPC type (<see cref="F:Terraria.NPC.type" />), then that NPC is a bee that will hurt other non-bee NPCs on contact.
		/// <br /> Defaults to <see langword="false" />.
		/// </summary>
		public static bool[] HurtingBees;

		/// <summary>
		/// If <see langword="true" /> for a given NPC type (<see cref="F:Terraria.NPC.type" />), then that NPC will have a special spawning animation when using <see cref="F:Terraria.ID.NPCAIStyleID.DD2Fighter" />.
		/// <br /> Defaults to <see langword="false" />.
		/// </summary>
		public static bool[] FighterUsesDD2PortalAppearEffect;

		/// <summary>
		/// If <c>!= -1f</c> for a given NPC type (<see cref="F:Terraria.NPC.type" />), then if that NPC was spawned from a statue (<see cref="F:Terraria.NPC.SpawnedFromStatue" />), it will have a (retrieved value)% chance to actually drop loot when killed.
		/// <br /> Defaults to <c>-1f</c>.
		/// </summary>
		/// <remarks>NPCs in this set will never drop loot unless interacted with by a player (<see cref="M:Terraria.NPC.AnyInteractions" />).</remarks>
		public static float[] StatueSpawnedDropRarity;

		/// <summary>
		/// If <see langword="true" /> for a given NPC type (<see cref="F:Terraria.NPC.type" />), then if that NPC was spawned from a statue (<see cref="F:Terraria.NPC.SpawnedFromStatue" />), it will not drop loot in pre-hardmode.
		/// <br /> Defaults to <see langword="false" />.
		/// </summary>
		public static bool[] NoEarlymodeLootWhenSpawnedFromStatue;

		/// <summary>
		/// If <see langword="true" /> for a given NPC type (<see cref="F:Terraria.NPC.type" />), then that NPC will receive stat scaling in Expert Mode, even if its normal stats would prevent that.
		/// <br /> Defaults to <see langword="false" />.
		/// </summary>
		/// <remarks>NPCs with less than 5 health are most prone to needing this set. The full scaling conditions can be found at the start of <see cref="M:Terraria.NPC.ScaleStats(System.Nullable{System.Int32},Terraria.DataStructures.GameModeData,System.Nullable{System.Single})" />.</remarks>
		public static bool[] NeedsExpertScaling;

		/// <summary>
		/// If <see langword="true" /> for a given NPC type (<see cref="F:Terraria.NPC.type" />), then that NPC is treated specially for difficulty scaling.
		/// <br /> Projectile NPCs never scale their max health, defense, or value.
		/// <br /> Additionally, kills are not counted for NPCs in this set.
		/// <br /> Defaults to <see langword="false" />.
		/// </summary>
		public static bool[] ProjectileNPC;

		internal static bool[] SavesAndLoads;

		/// <summary>
		/// The length of this NPC type's (<see cref="F:Terraria.NPC.type" />) <see cref="F:Terraria.NPC.oldPos" /> and <see cref="F:Terraria.NPC.oldRot" /> arrays.
		/// <br /> <strong>This set does nothing by itself.</strong> You will need to set <see cref="F:Terraria.ID.NPCID.Sets.TrailingMode" /> in order to actually store values.
		/// <br /> Defaults to <c>10</c>.
		/// </summary>
		public static int[] TrailCacheLength;

		/// <summary>
		/// If <see langword="true" /> for a given NPC type (<see cref="F:Terraria.NPC.type" />), then that NPC will sync more the closer it is to a player.
		/// <br /> Defaults to <see langword="true" />.
		/// </summary>
		public static bool[] UsesMultiplayerProximitySyncing;

		/// <summary>
		/// If <see langword="true" /> for a given NPC type (<see cref="F:Terraria.NPC.type" />), then that NPC will not smooth its visual position in multiplayer.
		/// <br /> Defaults to <see langword="false" />.
		/// </summary>
		/// <remarks>To prevent smoothing by <see cref="T:Terraria.ID.NPCAIStyleID" /> (<see cref="F:Terraria.NPC.aiStyle" />), use <see cref="F:Terraria.ID.NPCID.Sets.NoMultiplayerSmoothingByAI" />.</remarks>
		public static bool[] NoMultiplayerSmoothingByType;

		/// <summary>
		/// If <see langword="true" /> for a given <strong><see cref="T:Terraria.ID.NPCAIStyleID" /></strong> (<see cref="F:Terraria.NPC.aiStyle" />), then that AI style will not smooth its visual position in multiplayer.
		/// <br /> Defaults to <see langword="false" />.
		/// </summary>
		/// <remarks>To prevent smoothing by NPC type (<see cref="F:Terraria.NPC.type" />), use <see cref="F:Terraria.ID.NPCID.Sets.NoMultiplayerSmoothingByType" />.</remarks>
		public static bool[] NoMultiplayerSmoothingByAI;

		/// <summary>
		/// If <see langword="true" /> for a given NPC type (<see cref="F:Terraria.NPC.type" />), then that NPC can be summoned in multiplayer using <see cref="F:Terraria.ID.MessageID.SpawnBossUseLicenseStartEvent" />.
		/// <br /> <strong>If you don't set this, your boss most likely won't work in multiplayer.</strong>
		/// <br /> Defaults to <see langword="false" />.
		/// </summary>
		public static bool[] MPAllowedEnemies;

		/// <summary>
		/// If <see langword="true" /> for a given NPC type (<see cref="F:Terraria.NPC.type" />), then that NPC is a critter that can spawn in towns.
		/// <br /> Town critters use <see cref="F:Terraria.ID.NPCAIStyleID.Passive" />.
		/// <br /> Defauts to <see langword="false" />.
		/// </summary>
		/// <remarks>Make sure to also set <see cref="F:Terraria.ID.NPCID.Sets.CountsAsCritter" /> for NPCs in this set.</remarks>
		public static bool[] TownCritter;

		/// <summary>
		/// If <see langword="true" /> for a given NPC type (<see cref="F:Terraria.NPC.type" />), then that NPC is counted as a critter.
		/// <br /> Critters cannot be damage by players with <see cref="F:Terraria.Player.dontHurtCritters" /> set to <see langword="true" />.
		/// <br /> Also, critters can't be used to summon Horseman's Blade projectiles, nor can they be chased by said projectiles.
		/// </summary>
		public static bool[] CountsAsCritter;

		/// <summary>
		/// <strong>Only applies to vanilla NPCs.</strong> Also only applies to town NPCs.
		/// <br /> If <see langword="true" /> for a given NPC type (<see cref="F:Terraria.NPC.type" />), then that NPC doesn't have any special dialogue for parties.
		/// <br /> Defaults to <see langword="false" />.
		/// </summary>
		public static bool[] HasNoPartyText;

		/// <summary>
		/// The vertical offset, in pixels, that an NPC's party hat will draw with.
		/// <br /> Defaults to <c>0</c>.
		/// </summary>
		/// <remarks><see cref="F:Terraria.ID.NPCID.Sets.TownNPCsFramingGroups" /> applies a specific offset per frame, this applies the same offset to all frames.</remarks>
		public static int[] HatOffsetY;

		/// <summary>
		/// Associates a town NPC's NPC type (<see cref="F:Terraria.NPC.type" />) with its corresponding <see cref="T:Terraria.GameContent.UI.EmoteID" />.
		/// <br /> Town NPCs can emote using emotes in this set if the corresponding NPC is alive.
		/// <br /> If <c>0</c> for a given NPC type, then that NPC has no associated emote.
		/// <br /> Defaults to <c>0</c>.
		/// </summary>
		/// <remarks>This only applies to town NPCs -- there isn't a set that associates, say, the Eye of Cthulhu to an associated emote.</remarks>
		public static int[] FaceEmote;

		/// <summary>
		/// The number of extra frames this town NPC has. These frames are used for special actions, such as sitting down and talking to other NPCs.
		/// <br /> Defaults to <c>0</c>.
		/// </summary>
		/// <remarks>If you want to copy a vanilla NPC's framing, use <see cref="P:Terraria.ModLoader.ModNPC.AnimationType" />.</remarks>
		public static int[] ExtraFramesCount;

		/// <summary>
		/// The number of this town NPC's extra frames that are dedicated to attacking.
		/// <br /> Defaults to <c>0</c>.
		/// </summary>
		/// <remarks>If you want to copy a vanilla NPC's framing, use <see cref="P:Terraria.ModLoader.ModNPC.AnimationType" />.</remarks>
		public static int[] AttackFrameCount;

		/// <summary>
		/// The distance, in pixels, that this town NPC will check for enemies to attack. Any enemies beyond this distance are not considered.
		/// <br /> Also serves as the attack range for NPCs with an <see cref="F:Terraria.ID.NPCID.Sets.AttackType" /> of <c>0</c> or <c>2</c>.
		/// <br /> Defaults to <c>-1</c>, which uses a default of <c>200</c> pixels (12.5 tiles).
		/// </summary>
		/// <remarks><see cref="F:Terraria.ID.NPCID.Sets.PrettySafe" /> determines an NPC's attack range.</remarks>
		public static int[] DangerDetectRange;

		/// <summary>
		/// <b>Unused:</b> Replaced by <see cref="F:Terraria.ID.NPCID.Sets.SpecificDebuffImmunity" /><br /><br />
		/// If <see langword="true" /> for a given NPC type (<see cref="F:Terraria.NPC.type" />), then that NPC is immune to the effects of the shimmer.
		/// <br /> Defaults to <see langword="false" />.
		/// </summary>
		/// <remarks>To make an NPC ignore the player's shimmer invulnerability, use <see cref="F:Terraria.ID.NPCID.Sets.CanHitPastShimmer" /> instead.</remarks>
		private static bool[] ShimmerImmunity;

		/// <summary>
		/// If not <c>-1</c> for a given NPC type (<see cref="F:Terraria.NPC.type" />), then that NPC will transform into the retrieved item type (<see cref="F:Terraria.Item.type" />) when touching shimmer.
		/// <br /> Defaults to <c>-1</c>.
		/// </summary>
		/// <remarks>The created item will always have a stack of 1.</remarks>
		public static int[] ShimmerTransformToItem;

		/// <summary>
		/// If <see langword="true" /> for a given NPC type (<see cref="F:Terraria.NPC.type" />), then that NPC is a town NPC with an alternate shimmered texture.
		/// <br /> Defaults to <see langword="false" />.
		/// </summary>
		public static bool[] ShimmerTownTransform;

		/// <summary>
		/// Associates an NPC type (<see cref="F:Terraria.NPC.type" />) with the NPC type it will turn into when submerged in shimmer.
		/// <br /> A value of<c>-1</c> means the given NPC won't transform.
		/// <br /> Defaults to <c>-1</c>.
		/// <br /> Also applies to transforming critter items (<c><see cref="F:Terraria.Item.makeNPC" /> &gt; 0</c>) into NPC.
		/// </summary>
		public static int[] ShimmerTransformToNPC;

		/// <summary>
		/// The duration of this town NPC's attack animation in ticks.
		/// <br /> Defaults to <c>-1</c>.
		/// </summary>
		public static int[] AttackTime;

		/// <summary>
		/// The chance that this town NPC attacks if it detects danger.
		/// <br /> The actual chance is <c>1 / (retrieved value * 2)</c>.
		/// <br /> Defaults to <c>1</c>.
		/// </summary>
		public static int[] AttackAverageChance;

		/// <summary>
		/// Determines how a town NPC with the given NPC type (<see cref="F:Terraria.NPC.type" />) attacks.
		/// <br /> For <c>-1</c>, this NPC won't attack.
		/// <br /> For <c>0</c>, this NPC will throw a projectile.
		/// <br /> For <c>1</c>, this NPC will shoot a weapon.
		/// <br /> For <c>2</c>, this NPC will use magic.
		/// <br /> For <c>3</c>, this NPC will swing a weapon.
		/// <br /> Defaults to <c>-1</c>.
		/// </summary>
		/// <remarks>
		/// You can modify most aspects of a town NPC's attack, including the item used and projectile shot, using any of the "TownNPCAttack" or "TownAttack" hooks in <see cref="T:Terraria.ModLoader.ModNPC" /> or <see cref="T:Terraria.ModLoader.GlobalNPC" />.
		/// <br /> You can change the color of a magic (<c>2</c>) NPC's aura using <see cref="F:Terraria.ID.NPCID.Sets.MagicAuraColor" />.
		/// </remarks>
		public static int[] AttackType;

		/// <summary>
		/// The maximum distance in pixels that an enemy can be from this town NPC before they try to attack.
		/// <br /> Defaults to <c>-1</c>.
		/// </summary>
		public static int[] PrettySafe;

		/// <summary>
		/// The <see cref="T:Microsoft.Xna.Framework.Color" /> of the magical aura used by town NPCs with magic (<c>3</c>) attacks.
		/// <br /> Defaults to <see cref="P:Microsoft.Xna.Framework.Color.White" />.
		/// </summary>
		public static Color[] MagicAuraColor;

		/// <summary>
		/// <strong>Unused in vanilla.</strong>
		/// <br /> If <see langword="true" /> for a given NPC type (<see cref="F:Terraria.NPC.type" />), then that NPC is a type of demon eye.
		/// <br /> Defaults to <see langword="false" />.
		/// </summary>
		public static bool[] DemonEyes;

		/// <summary>
		/// <strong>Unused in vanilla.</strong>
		/// <br /> If <see langword="true" /> for a given NPC type (<see cref="F:Terraria.NPC.type" />), then that NPC is a type of zombie.
		/// <br /> Defaults to <see langword="false" />.
		/// </summary>
		public static bool[] Zombies;

		/// <summary>
		/// If <see langword="true" /> for a given NPC type (<see cref="F:Terraria.NPC.type" />), then that NPC is a type of skeleton.
		/// <br /> Skeletons cannot hurt the <see cref="F:Terraria.ID.NPCID.SkeletonMerchant" />.
		/// <br /> Defaults to <see langword="false" />.
		/// </summary>
		public static bool[] Skeletons;

		/// <summary>
		/// Associates an NPC type (<see cref="F:Terraria.NPC.type" />) with the index in <see cref="F:Terraria.GameContent.TextureAssets.NpcHeadBoss" /> of its default map icon.
		/// <br /> Auto-set using <see cref="T:Terraria.ModLoader.AutoloadBossHead" />.
		/// <br /> Defaults to <c>-1</c>.
		/// </summary>
		/// <remarks>
		/// If you need to dynamically change your boss's head icon, use <see cref="M:Terraria.ModLoader.ModNPC.BossHeadSlot(System.Int32@)" /> and <see cref="M:Terraria.ModLoader.NPCHeadLoader.GetBossHeadSlot(System.String)" />.
		/// <br /> If you need an NPC's <em>current</em> head index, using <see cref="M:Terraria.NPC.GetBossHeadTextureIndex" />, which handles phase changes.
		/// </remarks>
		public static int[] BossHeadTextures;

		/// <summary>
		/// If <see langword="true" /> for a given NPC type (<see cref="F:Terraria.NPC.type" />), then that NPC will not have its kills counted.
		/// <br /> Used in vanilla for special projectile-like NPCs (not found in <see cref="F:Terraria.ID.NPCID.Sets.ProjectileNPC" />), for NPCs that turn into another NPC on death, and for temporary NPCs, like the <see cref="F:Terraria.ID.NPCID.MothronEgg" />.
		/// <br /> Defaults to <see langword="false" />.
		/// </summary>
		/// <remarks>Any NPC in the <see cref="F:Terraria.ID.NPCID.Sets.ProjectileNPC" /> set will not have its kills counted either.</remarks>
		public static bool[] PositiveNPCTypesExcludedFromDeathTally;

		/// <summary>
		/// If <see langword="true" /> for a given NPC type (<see cref="F:Terraria.NPC.type" />), then that NPC is sorted with bosses despite <see cref="F:Terraria.NPC.boss" /> being <see langword="false" />.
		/// <Br /> Used in vanilla for the Celestial Pillars, the Eater of Worlds' head, and the Torch God.
		/// <br /> Defaults to <see langword="false" />.
		/// </summary>
		public static bool[] ShouldBeCountedAsBoss;

		/// <summary>
		/// If <see langword="true" /> for a given NPC type (<see cref="F:Terraria.NPC.type" />), then that NPC is considered dangerous without having <see cref="F:Terraria.NPC.boss" /> set.
		/// <br /> <see cref="F:Terraria.Main.CurrentFrameFlags.AnyActiveBossNPC" /> will be set to <see langword="true" /> if any NPC in this set is alive, and <see cref="M:Terraria.NPC.AnyDanger(System.Boolean,System.Boolean)" /> will return <see langword="true" /> if any NPCs in this set are alive.
		/// <br /> Defaults to <see langword="false" />.
		/// </summary>
		public static bool[] DangerThatPreventsOtherDangers;

		/// <summary>
		/// If <see langword="true" /> for a given NPC type (<see cref="F:Terraria.NPC.type" />), then that NPC will always draw, even if its hitbox is off-screen.
		/// <br /> Default to <see langword="false" />.
		/// </summary>
		public static bool[] MustAlwaysDraw;

		/// <summary>
		/// The number of extra textures this town NPC has.
		/// <br /> Most extra textures are hatless versions used for parties, though the <see cref="F:Terraria.ID.NPCID.BestiaryGirl" /> has an additional transformed texture as well.
		/// <br /> Unused in vanilla.
		/// <br /> Defaults to <c>0</c>.
		/// </summary>
		public static int[] ExtraTextureCount;

		/// <summary>
		/// The index for <see cref="F:Terraria.ID.NPCID.Sets.TownNPCsFramingGroups" /> for a given NPC type (<see cref="F:Terraria.NPC.type" />). See <see cref="F:Terraria.ID.NPCID.Sets.TownNPCsFramingGroups" /> for more info.
		/// <br /> Defaults to <c>0</c>.
		/// </summary>
		public static int[] NPCFramingGroup;

		/// <summary>
		/// If <see langword="true" /> for a given NPC type (<see cref="F:Terraria.NPC.type" />), then that NPC can hit players that are submerged in shimmer.
		/// <br /> Defaults to <see langword="false" />.
		/// </summary>
		/// <remarks>Bosses (<see cref="F:Terraria.NPC.boss" />) and invasion enemies (<see cref="M:Terraria.NPC.GetNPCInvasionGroup(System.Int32)" />) can always hit shimmered players.</remarks>
		public static bool[] CanHitPastShimmer;

		/// <summary>
		/// The vertical offset, in pixels, that this NPC's party hat will draw. Indexed using values from <see cref="F:Terraria.ID.NPCID.Sets.NPCFramingGroup" />.
		/// </summary>
		/// <remarks><see cref="F:Terraria.ID.NPCID.Sets.HatOffsetY" /> applies the same offset to all frames, this applies a specific offset per frame.</remarks>
		public static int[][] TownNPCsFramingGroups;

		/// <summary>
		/// Whether or not the spawned NPC will start looking for a suitable slot from the end of <seealso cref="F:Terraria.Main.npc" />, ignoring the Start parameter of <see cref="M:Terraria.NPC.NewNPC(Terraria.DataStructures.IEntitySource,System.Int32,System.Int32,System.Int32,System.Int32,System.Single,System.Single,System.Single,System.Single,System.Int32)" />.
		/// Useful if you have a multi-segmented boss and want its parts to draw over the main body (body will be in this set).
		/// </summary>
		public static bool[] SpawnFromLastEmptySlot;

		/// <summary>
		/// Whether or not a given NPC will act like a town NPC in terms of AI, animations, and attacks, but not in other regards, such as appearing on the minimap, like the bone merchant in vanilla.
		/// </summary>
		public static bool[] ActsLikeTownNPC;

		/// <summary>
		/// If true, the given NPC will not count towards town NPC happiness and won't have a happiness button. Pets (<see cref="F:Terraria.ID.NPCID.Sets.IsTownPet" />) do not need to set this.
		/// </summary>
		public static bool[] NoTownNPCHappiness;

		/// <summary>
		/// Whether or not a given NPC will spawn with a custom name like a town NPC. In order to determine what name will be selected, override the TownNPCName hook.
		/// True will force a name to be rolled regardless of vanilla behavior. False will have vanilla handle the naming.
		/// </summary>
		public static bool[] SpawnsWithCustomName;

		/// <summary>
		/// Whether or not a given NPC can sit on suitable furniture (<see cref="F:Terraria.ID.TileID.Sets.CanBeSatOnForNPCs" />)
		/// </summary>
		public static bool[] CannotSitOnFurniture;

		/// <summary>
		/// Whether or not a given NPC is excluded from dropping hardmode souls (Soul of Night/Light)
		/// <br />Contains vanilla NPCs that are easy to spawn in large numbers, preventing easy soul farming
		/// <br />Do not add your NPC to this if it would be excluded automatically (i.e. critter, town NPC, or no coin drops)
		/// </summary>
		public static bool[] CannotDropSouls;

		/// <summary>
		/// Whether or not this NPC can still interact with doors if they use the Vanilla TownNPC aiStyle (AKA aiStyle == 7)
		/// but are not actually marked as Town NPCs (AKA npc.townNPC == true).
		/// </summary>
		/// <remarks>
		/// Note: This set DOES NOT DO ANYTHING if your NPC doesn't use the Vanilla TownNPC aiStyle (aiStyle == 7).
		/// </remarks>
		public static bool[] AllowDoorInteraction;

		/// <summary>
		/// If <see langword="true" />, this NPC type (<see cref="F:Terraria.NPC.type" />) will be immune to all debuffs and "tag" buffs by default.<br /><br />
		/// Use this for special NPCs that cannot be hit at all, such as fairy critters, container NPCs like Martian Saucer and Pirate Ship, bound town slimes, and Blazing Wheel. Dungeon Guardian also is in this set to prevent the bonus damage from "tag" buffs.<br /><br />
		/// If the NPC should be attacked, it is recommended to set <see cref="F:Terraria.ID.NPCID.Sets.ImmuneToRegularBuffs" /> to <see langword="true" /> instead. This will prevent all debuffs except "tag" buffs (<see cref="F:Terraria.ID.BuffID.Sets.IsATagBuff" />), which are intended to affect enemies typically seen as immune to all debuffs. Tag debuffs are special debuffs that facilitate combat mechanics, they are not something that adversely affects NPC.<br /><br />
		/// Modders can specify specific buffs to be vulnerable to by assigning <see cref="F:Terraria.ID.NPCID.Sets.SpecificDebuffImmunity" /> to false.
		/// </summary>
		public static bool[] ImmuneToAllBuffs;

		/// <summary>
		/// If <see langword="true" />, this NPC type (<see cref="F:Terraria.NPC.type" />) will be immune to all debuffs except tag debuffs (<see cref="F:Terraria.ID.BuffID.Sets.IsATagBuff" />) by default.<br /><br />
		/// Use this for NPCs that can be attacked that should be immune to all normal debuffs. Tag debuffs are special debuffs that facilitate combat mechanics, such as the "summon tag damage" applied by whip weapons. Wraith, Reaper, Lunatic Cultist, the Celestial Pillars, The Destroyer, and the Martian Saucer Turret/Cannon/Core are examples of NPCs that use this setting.<br /><br />
		/// Modders can specify specific buffs to be vulnerable to by assigning <see cref="F:Terraria.ID.NPCID.Sets.SpecificDebuffImmunity" /> to false.
		/// </summary>
		public static bool[] ImmuneToRegularBuffs;

		/// <summary>
		/// Indexed by NPC type and then Buff type. If <see langword="true" />, this NPC type (<see cref="F:Terraria.NPC.type" />) will be immune (<see cref="F:Terraria.NPC.buffImmune" />) to the specified buff type. If <see langword="false" />, the NPC will not be immune.<br /><br />
		/// By default, NPCs aren't immune to any buffs, but <see cref="F:Terraria.ID.NPCID.Sets.ImmuneToRegularBuffs" /> or <see cref="F:Terraria.ID.NPCID.Sets.ImmuneToAllBuffs" /> can make an NPC immune to all buffs. The values in this set override those settings.<br /><br />
		/// Additionally, the effects of <see cref="F:Terraria.ID.BuffID.Sets.GrantImmunityWith" /> will also be applied. Inherited buff immunities do not need to be specifically assigned, as they will be automatically applied. Setting an inherited debuff to false in this set can be used to undo the effects of <see cref="F:Terraria.ID.BuffID.Sets.GrantImmunityWith" />, if needed.<br /><br />
		/// Defaults to <see langword="null" />, indicating no immunity override.<br />
		/// </summary>
		public static bool?[][] SpecificDebuffImmunity;

		/// <summary>
		/// If <see langword="true" /> for a given NPC type (<see cref="F:Terraria.NPC.type" />), then that NPC belongs to the Goblin Army invasion.
		/// <br /> During the Goblin Army invasion, NPCs in this set will decrement <see cref="F:Terraria.Main.invasionSize" /> by the amount specified in <see cref="F:Terraria.ID.NPCID.Sets.InvasionSlotCount" /> when killed.
		/// <br /> If any NPC in this set is alive and <see cref="F:Terraria.ID.NPCID.Sets.InvasionSlotCount" /> is above 0, the Goblin Army music will play.
		/// <br /> Defaults to <see langword="false" />.
		/// </summary>
		public static bool[] BelongsToInvasionGoblinArmy;

		/// <summary>
		/// If <see langword="true" /> for a given NPC type (<see cref="F:Terraria.NPC.type" />), then that NPC belongs to the Frost Legion invasion.
		/// <br /> During the Frost Legion invasion, NPCs in this set will decrement <see cref="F:Terraria.Main.invasionSize" /> by the amount specified in <see cref="F:Terraria.ID.NPCID.Sets.InvasionSlotCount" /> when killed.
		/// <br /> If any NPC in this set is alive and <see cref="F:Terraria.ID.NPCID.Sets.InvasionSlotCount" /> is above 0, the Boss 3 music will play.
		/// <br /> Defaults to <see langword="false" />.
		/// </summary>
		public static bool[] BelongsToInvasionFrostLegion;

		/// <summary>
		/// If <see langword="true" /> for a given NPC type (<see cref="F:Terraria.NPC.type" />), then that NPC belongs to the Pirate invasion.
		/// <br /> During the Pirate invasion, NPCs in this set will decrement <see cref="F:Terraria.Main.invasionSize" /> by the amount specified in <see cref="F:Terraria.ID.NPCID.Sets.InvasionSlotCount" /> when killed.
		/// <br /> If any NPC in this set is alive and <see cref="F:Terraria.ID.NPCID.Sets.InvasionSlotCount" /> is above 0, the Pirate Invasion music will play.
		/// <br /> Defaults to <see langword="false" />.
		/// </summary>
		public static bool[] BelongsToInvasionPirate;

		/// <summary>
		/// If <see langword="true" /> for a given NPC type (<see cref="F:Terraria.NPC.type" />), then that NPC belongs to the Martian Madness invasion.
		/// <br /> During the Martian Madness invasion, NPCs in this set will decrement <see cref="F:Terraria.Main.invasionSize" /> by the amount specified in <see cref="F:Terraria.ID.NPCID.Sets.InvasionSlotCount" /> when killed.
		/// <br /> If any NPC in this set is alive and <see cref="F:Terraria.ID.NPCID.Sets.InvasionSlotCount" /> is above 0, the Martian Madness music will play.
		/// <br /> Defaults to <see langword="false" />.
		/// </summary>
		public static bool[] BelongsToInvasionMartianMadness;

		/// <summary>
		/// If <see langword="true" /> for a given NPC type (<see cref="F:Terraria.NPC.type" />), then that NPC will not play its associated invasion music.
		/// <br /> By default, alive NPCs in any BelongsToInvasion set will automatically play the associated invasion music if <see cref="F:Terraria.ID.NPCID.Sets.InvasionSlotCount" /> is above 0.
		/// <br /> Defaults to <see langword="false" />.
		/// </summary>
		public static bool[] NoInvasionMusic;

		/// <summary>
		/// If above 0 for a given NPC type (<see cref="F:Terraria.NPC.type" />), and its associated invasion is NOT a wave-based one, then that NPC will decrement <see cref="F:Terraria.Main.invasionSize" /> by that amount when killed.
		/// <br /> If this NPC's entry is 0, it won't play its associated invasion's music when alive.
		/// </summary>
		/// <remarks>
		/// Note: Even though this defaults to 1, this set should only be checked if <see cref="M:Terraria.NPC.GetNPCInvasionGroup(System.Int32)" /> is above 0 or if any BelongsToInvasion sets are <see langword="true" />.
		/// </remarks>
		public static int[] InvasionSlotCount;

		public static Dictionary<int, NPCBestiaryDrawModifiers> NPCBestiaryDrawOffsetCreation()
		{
			Dictionary<int, NPCBestiaryDrawModifiers> redigitEntries = GetRedigitEntries();
			Dictionary<int, NPCBestiaryDrawModifiers> leinforsEntries = GetLeinforsEntries();
			Dictionary<int, NPCBestiaryDrawModifiers> groxEntries = GetGroxEntries();
			Dictionary<int, NPCBestiaryDrawModifiers> dictionary = new Dictionary<int, NPCBestiaryDrawModifiers>();
			foreach (KeyValuePair<int, NPCBestiaryDrawModifiers> item in groxEntries)
			{
				dictionary[item.Key] = FilterPaths(item.Value);
			}
			foreach (KeyValuePair<int, NPCBestiaryDrawModifiers> item2 in leinforsEntries)
			{
				dictionary[item2.Key] = FilterPaths(item2.Value);
			}
			foreach (KeyValuePair<int, NPCBestiaryDrawModifiers> item3 in redigitEntries)
			{
				dictionary[item3.Key] = FilterPaths(item3.Value);
			}
			return dictionary;
			static NPCBestiaryDrawModifiers FilterPaths(NPCBestiaryDrawModifiers modifiers)
			{
				if (modifiers.CustomTexturePath != null)
				{
					modifiers.CustomTexturePath = "Terraria/" + modifiers.CustomTexturePath;
				}
				return modifiers;
			}
		}

		private static Dictionary<int, NPCBestiaryDrawModifiers> GetRedigitEntries()
		{
			return new Dictionary<int, NPCBestiaryDrawModifiers>
			{
				{
					430,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					431,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					432,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					433,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					434,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					435,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					436,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					591,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					449,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					450,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					451,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					452,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					595,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					596,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					597,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					598,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					600,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					495,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					497,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					498,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					500,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					501,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					502,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					503,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					504,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					505,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					506,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					230,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					593,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					158,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					-2,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					440,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					568,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					566,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					576,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					558,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					559,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					552,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					553,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					564,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					570,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					555,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					556,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					574,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					561,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					562,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					572,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					535,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				}
			};
		}

		private static Dictionary<int, NPCBestiaryDrawModifiers> GetGroxEntries()
		{
			return new Dictionary<int, NPCBestiaryDrawModifiers>();
		}

		private static Dictionary<int, NPCBestiaryDrawModifiers> GetLeinforsEntries()
		{
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_03da: Unknown result type (might be due to invalid IL or missing references)
			//IL_0404: Unknown result type (might be due to invalid IL or missing references)
			//IL_0409: Unknown result type (might be due to invalid IL or missing references)
			//IL_0433: Unknown result type (might be due to invalid IL or missing references)
			//IL_0438: Unknown result type (might be due to invalid IL or missing references)
			//IL_0462: Unknown result type (might be due to invalid IL or missing references)
			//IL_0467: Unknown result type (might be due to invalid IL or missing references)
			//IL_0491: Unknown result type (might be due to invalid IL or missing references)
			//IL_0496: Unknown result type (might be due to invalid IL or missing references)
			//IL_070c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0711: Unknown result type (might be due to invalid IL or missing references)
			//IL_0753: Unknown result type (might be due to invalid IL or missing references)
			//IL_0758: Unknown result type (might be due to invalid IL or missing references)
			//IL_07cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_07d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0807: Unknown result type (might be due to invalid IL or missing references)
			//IL_080c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0842: Unknown result type (might be due to invalid IL or missing references)
			//IL_0847: Unknown result type (might be due to invalid IL or missing references)
			//IL_087d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0882: Unknown result type (might be due to invalid IL or missing references)
			//IL_08b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_08bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_08f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_08f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_09c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_09c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a08: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a0d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a4e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a53: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ab2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ab7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0af1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0af6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b23: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b28: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b61: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b66: Unknown result type (might be due to invalid IL or missing references)
			//IL_0be7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0bec: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c6e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0c73: Unknown result type (might be due to invalid IL or missing references)
			//IL_0eb2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0eb7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f66: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f6b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0fe1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0fe6: Unknown result type (might be due to invalid IL or missing references)
			//IL_106b: Unknown result type (might be due to invalid IL or missing references)
			//IL_1070: Unknown result type (might be due to invalid IL or missing references)
			//IL_10a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_10a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_10db: Unknown result type (might be due to invalid IL or missing references)
			//IL_10e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_1124: Unknown result type (might be due to invalid IL or missing references)
			//IL_1129: Unknown result type (might be due to invalid IL or missing references)
			//IL_11b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_11b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_11f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_11f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_1226: Unknown result type (might be due to invalid IL or missing references)
			//IL_122b: Unknown result type (might be due to invalid IL or missing references)
			//IL_1266: Unknown result type (might be due to invalid IL or missing references)
			//IL_126b: Unknown result type (might be due to invalid IL or missing references)
			//IL_12a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_12ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_130c: Unknown result type (might be due to invalid IL or missing references)
			//IL_1311: Unknown result type (might be due to invalid IL or missing references)
			//IL_1344: Unknown result type (might be due to invalid IL or missing references)
			//IL_1349: Unknown result type (might be due to invalid IL or missing references)
			//IL_1390: Unknown result type (might be due to invalid IL or missing references)
			//IL_1395: Unknown result type (might be due to invalid IL or missing references)
			//IL_13d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_13d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_1467: Unknown result type (might be due to invalid IL or missing references)
			//IL_146c: Unknown result type (might be due to invalid IL or missing references)
			//IL_14ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_14b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_155f: Unknown result type (might be due to invalid IL or missing references)
			//IL_1564: Unknown result type (might be due to invalid IL or missing references)
			//IL_15a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_15a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_15ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_15f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_164d: Unknown result type (might be due to invalid IL or missing references)
			//IL_1652: Unknown result type (might be due to invalid IL or missing references)
			//IL_1713: Unknown result type (might be due to invalid IL or missing references)
			//IL_1718: Unknown result type (might be due to invalid IL or missing references)
			//IL_174b: Unknown result type (might be due to invalid IL or missing references)
			//IL_1750: Unknown result type (might be due to invalid IL or missing references)
			//IL_178a: Unknown result type (might be due to invalid IL or missing references)
			//IL_178f: Unknown result type (might be due to invalid IL or missing references)
			//IL_1811: Unknown result type (might be due to invalid IL or missing references)
			//IL_1816: Unknown result type (might be due to invalid IL or missing references)
			//IL_188c: Unknown result type (might be due to invalid IL or missing references)
			//IL_1891: Unknown result type (might be due to invalid IL or missing references)
			//IL_18bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_18c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_1988: Unknown result type (might be due to invalid IL or missing references)
			//IL_198d: Unknown result type (might be due to invalid IL or missing references)
			//IL_1a47: Unknown result type (might be due to invalid IL or missing references)
			//IL_1a4c: Unknown result type (might be due to invalid IL or missing references)
			//IL_1ab5: Unknown result type (might be due to invalid IL or missing references)
			//IL_1aba: Unknown result type (might be due to invalid IL or missing references)
			//IL_1afe: Unknown result type (might be due to invalid IL or missing references)
			//IL_1b03: Unknown result type (might be due to invalid IL or missing references)
			//IL_1b53: Unknown result type (might be due to invalid IL or missing references)
			//IL_1b58: Unknown result type (might be due to invalid IL or missing references)
			//IL_1c32: Unknown result type (might be due to invalid IL or missing references)
			//IL_1c37: Unknown result type (might be due to invalid IL or missing references)
			//IL_1c6a: Unknown result type (might be due to invalid IL or missing references)
			//IL_1c6f: Unknown result type (might be due to invalid IL or missing references)
			//IL_1c91: Unknown result type (might be due to invalid IL or missing references)
			//IL_1c96: Unknown result type (might be due to invalid IL or missing references)
			//IL_1cc4: Unknown result type (might be due to invalid IL or missing references)
			//IL_1cc9: Unknown result type (might be due to invalid IL or missing references)
			//IL_1d03: Unknown result type (might be due to invalid IL or missing references)
			//IL_1d08: Unknown result type (might be due to invalid IL or missing references)
			//IL_1d4f: Unknown result type (might be due to invalid IL or missing references)
			//IL_1d54: Unknown result type (might be due to invalid IL or missing references)
			//IL_1d8d: Unknown result type (might be due to invalid IL or missing references)
			//IL_1d92: Unknown result type (might be due to invalid IL or missing references)
			//IL_1dcb: Unknown result type (might be due to invalid IL or missing references)
			//IL_1dd0: Unknown result type (might be due to invalid IL or missing references)
			//IL_1e09: Unknown result type (might be due to invalid IL or missing references)
			//IL_1e0e: Unknown result type (might be due to invalid IL or missing references)
			//IL_1e67: Unknown result type (might be due to invalid IL or missing references)
			//IL_1e6c: Unknown result type (might be due to invalid IL or missing references)
			//IL_1eae: Unknown result type (might be due to invalid IL or missing references)
			//IL_1eb3: Unknown result type (might be due to invalid IL or missing references)
			//IL_1f32: Unknown result type (might be due to invalid IL or missing references)
			//IL_1f37: Unknown result type (might be due to invalid IL or missing references)
			//IL_201a: Unknown result type (might be due to invalid IL or missing references)
			//IL_201f: Unknown result type (might be due to invalid IL or missing references)
			//IL_2055: Unknown result type (might be due to invalid IL or missing references)
			//IL_205a: Unknown result type (might be due to invalid IL or missing references)
			//IL_2090: Unknown result type (might be due to invalid IL or missing references)
			//IL_2095: Unknown result type (might be due to invalid IL or missing references)
			//IL_20cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_20d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_2112: Unknown result type (might be due to invalid IL or missing references)
			//IL_2117: Unknown result type (might be due to invalid IL or missing references)
			//IL_2159: Unknown result type (might be due to invalid IL or missing references)
			//IL_215e: Unknown result type (might be due to invalid IL or missing references)
			//IL_21c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_21ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_2239: Unknown result type (might be due to invalid IL or missing references)
			//IL_223e: Unknown result type (might be due to invalid IL or missing references)
			//IL_2378: Unknown result type (might be due to invalid IL or missing references)
			//IL_237d: Unknown result type (might be due to invalid IL or missing references)
			//IL_23c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_23c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_241c: Unknown result type (might be due to invalid IL or missing references)
			//IL_2421: Unknown result type (might be due to invalid IL or missing references)
			//IL_2446: Unknown result type (might be due to invalid IL or missing references)
			//IL_244b: Unknown result type (might be due to invalid IL or missing references)
			//IL_247c: Unknown result type (might be due to invalid IL or missing references)
			//IL_2481: Unknown result type (might be due to invalid IL or missing references)
			//IL_24b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_24b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_24dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_24e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_2566: Unknown result type (might be due to invalid IL or missing references)
			//IL_256b: Unknown result type (might be due to invalid IL or missing references)
			//IL_25a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_25a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_26ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_26b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_26e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_26ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_2723: Unknown result type (might be due to invalid IL or missing references)
			//IL_2728: Unknown result type (might be due to invalid IL or missing references)
			//IL_275e: Unknown result type (might be due to invalid IL or missing references)
			//IL_2763: Unknown result type (might be due to invalid IL or missing references)
			//IL_2799: Unknown result type (might be due to invalid IL or missing references)
			//IL_279e: Unknown result type (might be due to invalid IL or missing references)
			//IL_2a12: Unknown result type (might be due to invalid IL or missing references)
			//IL_2a17: Unknown result type (might be due to invalid IL or missing references)
			//IL_2a48: Unknown result type (might be due to invalid IL or missing references)
			//IL_2a4d: Unknown result type (might be due to invalid IL or missing references)
			//IL_2ab4: Unknown result type (might be due to invalid IL or missing references)
			//IL_2ab9: Unknown result type (might be due to invalid IL or missing references)
			//IL_2aef: Unknown result type (might be due to invalid IL or missing references)
			//IL_2af4: Unknown result type (might be due to invalid IL or missing references)
			//IL_2bac: Unknown result type (might be due to invalid IL or missing references)
			//IL_2bb1: Unknown result type (might be due to invalid IL or missing references)
			//IL_2bf6: Unknown result type (might be due to invalid IL or missing references)
			//IL_2bfb: Unknown result type (might be due to invalid IL or missing references)
			//IL_2c20: Unknown result type (might be due to invalid IL or missing references)
			//IL_2c25: Unknown result type (might be due to invalid IL or missing references)
			//IL_2c4a: Unknown result type (might be due to invalid IL or missing references)
			//IL_2c4f: Unknown result type (might be due to invalid IL or missing references)
			//IL_2c74: Unknown result type (might be due to invalid IL or missing references)
			//IL_2c79: Unknown result type (might be due to invalid IL or missing references)
			//IL_2c9e: Unknown result type (might be due to invalid IL or missing references)
			//IL_2ca3: Unknown result type (might be due to invalid IL or missing references)
			//IL_2d84: Unknown result type (might be due to invalid IL or missing references)
			//IL_2d89: Unknown result type (might be due to invalid IL or missing references)
			//IL_2dc7: Unknown result type (might be due to invalid IL or missing references)
			//IL_2dcc: Unknown result type (might be due to invalid IL or missing references)
			//IL_2df1: Unknown result type (might be due to invalid IL or missing references)
			//IL_2df6: Unknown result type (might be due to invalid IL or missing references)
			//IL_2e44: Unknown result type (might be due to invalid IL or missing references)
			//IL_2e49: Unknown result type (might be due to invalid IL or missing references)
			//IL_2f00: Unknown result type (might be due to invalid IL or missing references)
			//IL_2f05: Unknown result type (might be due to invalid IL or missing references)
			//IL_2f5b: Unknown result type (might be due to invalid IL or missing references)
			//IL_2f60: Unknown result type (might be due to invalid IL or missing references)
			//IL_2f91: Unknown result type (might be due to invalid IL or missing references)
			//IL_2f96: Unknown result type (might be due to invalid IL or missing references)
			//IL_300c: Unknown result type (might be due to invalid IL or missing references)
			//IL_3011: Unknown result type (might be due to invalid IL or missing references)
			//IL_3082: Unknown result type (might be due to invalid IL or missing references)
			//IL_3087: Unknown result type (might be due to invalid IL or missing references)
			//IL_30da: Unknown result type (might be due to invalid IL or missing references)
			//IL_30df: Unknown result type (might be due to invalid IL or missing references)
			//IL_3142: Unknown result type (might be due to invalid IL or missing references)
			//IL_3147: Unknown result type (might be due to invalid IL or missing references)
			//IL_31cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_31d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_3203: Unknown result type (might be due to invalid IL or missing references)
			//IL_3208: Unknown result type (might be due to invalid IL or missing references)
			//IL_322d: Unknown result type (might be due to invalid IL or missing references)
			//IL_3232: Unknown result type (might be due to invalid IL or missing references)
			//IL_3263: Unknown result type (might be due to invalid IL or missing references)
			//IL_3268: Unknown result type (might be due to invalid IL or missing references)
			//IL_3299: Unknown result type (might be due to invalid IL or missing references)
			//IL_329e: Unknown result type (might be due to invalid IL or missing references)
			//IL_32cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_32d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_3305: Unknown result type (might be due to invalid IL or missing references)
			//IL_330a: Unknown result type (might be due to invalid IL or missing references)
			//IL_333b: Unknown result type (might be due to invalid IL or missing references)
			//IL_3340: Unknown result type (might be due to invalid IL or missing references)
			//IL_3371: Unknown result type (might be due to invalid IL or missing references)
			//IL_3376: Unknown result type (might be due to invalid IL or missing references)
			//IL_33b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_33bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_33ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_33f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_3424: Unknown result type (might be due to invalid IL or missing references)
			//IL_3429: Unknown result type (might be due to invalid IL or missing references)
			//IL_345a: Unknown result type (might be due to invalid IL or missing references)
			//IL_345f: Unknown result type (might be due to invalid IL or missing references)
			//IL_3490: Unknown result type (might be due to invalid IL or missing references)
			//IL_3495: Unknown result type (might be due to invalid IL or missing references)
			//IL_34e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_34eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_351d: Unknown result type (might be due to invalid IL or missing references)
			//IL_3522: Unknown result type (might be due to invalid IL or missing references)
			//IL_3613: Unknown result type (might be due to invalid IL or missing references)
			//IL_3618: Unknown result type (might be due to invalid IL or missing references)
			//IL_365a: Unknown result type (might be due to invalid IL or missing references)
			//IL_365f: Unknown result type (might be due to invalid IL or missing references)
			//IL_386c: Unknown result type (might be due to invalid IL or missing references)
			//IL_3871: Unknown result type (might be due to invalid IL or missing references)
			//IL_38c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_38c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_38ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_38f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_3929: Unknown result type (might be due to invalid IL or missing references)
			//IL_392e: Unknown result type (might be due to invalid IL or missing references)
			//IL_3a35: Unknown result type (might be due to invalid IL or missing references)
			//IL_3a3a: Unknown result type (might be due to invalid IL or missing references)
			//IL_3a7f: Unknown result type (might be due to invalid IL or missing references)
			//IL_3a84: Unknown result type (might be due to invalid IL or missing references)
			//IL_3ae5: Unknown result type (might be due to invalid IL or missing references)
			//IL_3aea: Unknown result type (might be due to invalid IL or missing references)
			//IL_3bef: Unknown result type (might be due to invalid IL or missing references)
			//IL_3bf4: Unknown result type (might be due to invalid IL or missing references)
			//IL_3c36: Unknown result type (might be due to invalid IL or missing references)
			//IL_3c3b: Unknown result type (might be due to invalid IL or missing references)
			//IL_3c60: Unknown result type (might be due to invalid IL or missing references)
			//IL_3c65: Unknown result type (might be due to invalid IL or missing references)
			//IL_3c8a: Unknown result type (might be due to invalid IL or missing references)
			//IL_3c8f: Unknown result type (might be due to invalid IL or missing references)
			//IL_3cd6: Unknown result type (might be due to invalid IL or missing references)
			//IL_3cdb: Unknown result type (might be due to invalid IL or missing references)
			//IL_3d28: Unknown result type (might be due to invalid IL or missing references)
			//IL_3d2d: Unknown result type (might be due to invalid IL or missing references)
			//IL_3d63: Unknown result type (might be due to invalid IL or missing references)
			//IL_3d68: Unknown result type (might be due to invalid IL or missing references)
			//IL_3d99: Unknown result type (might be due to invalid IL or missing references)
			//IL_3d9e: Unknown result type (might be due to invalid IL or missing references)
			//IL_3e5e: Unknown result type (might be due to invalid IL or missing references)
			//IL_3e63: Unknown result type (might be due to invalid IL or missing references)
			//IL_3e88: Unknown result type (might be due to invalid IL or missing references)
			//IL_3e8d: Unknown result type (might be due to invalid IL or missing references)
			//IL_3ec3: Unknown result type (might be due to invalid IL or missing references)
			//IL_3ec8: Unknown result type (might be due to invalid IL or missing references)
			//IL_3ef9: Unknown result type (might be due to invalid IL or missing references)
			//IL_3efe: Unknown result type (might be due to invalid IL or missing references)
			//IL_3f23: Unknown result type (might be due to invalid IL or missing references)
			//IL_3f28: Unknown result type (might be due to invalid IL or missing references)
			//IL_3f5e: Unknown result type (might be due to invalid IL or missing references)
			//IL_3f63: Unknown result type (might be due to invalid IL or missing references)
			//IL_3f99: Unknown result type (might be due to invalid IL or missing references)
			//IL_3f9e: Unknown result type (might be due to invalid IL or missing references)
			//IL_3ff7: Unknown result type (might be due to invalid IL or missing references)
			//IL_3ffc: Unknown result type (might be due to invalid IL or missing references)
			//IL_4055: Unknown result type (might be due to invalid IL or missing references)
			//IL_405a: Unknown result type (might be due to invalid IL or missing references)
			//IL_408b: Unknown result type (might be due to invalid IL or missing references)
			//IL_4090: Unknown result type (might be due to invalid IL or missing references)
			//IL_4112: Unknown result type (might be due to invalid IL or missing references)
			//IL_4117: Unknown result type (might be due to invalid IL or missing references)
			//IL_4187: Unknown result type (might be due to invalid IL or missing references)
			//IL_418c: Unknown result type (might be due to invalid IL or missing references)
			//IL_428f: Unknown result type (might be due to invalid IL or missing references)
			//IL_4294: Unknown result type (might be due to invalid IL or missing references)
			//IL_42c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_42ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_42fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_4300: Unknown result type (might be due to invalid IL or missing references)
			//IL_434d: Unknown result type (might be due to invalid IL or missing references)
			//IL_4352: Unknown result type (might be due to invalid IL or missing references)
			//IL_4383: Unknown result type (might be due to invalid IL or missing references)
			//IL_4388: Unknown result type (might be due to invalid IL or missing references)
			//IL_43b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_43be: Unknown result type (might be due to invalid IL or missing references)
			//IL_4410: Unknown result type (might be due to invalid IL or missing references)
			//IL_4415: Unknown result type (might be due to invalid IL or missing references)
			//IL_4446: Unknown result type (might be due to invalid IL or missing references)
			//IL_444b: Unknown result type (might be due to invalid IL or missing references)
			//IL_44b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_44bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_4552: Unknown result type (might be due to invalid IL or missing references)
			//IL_4557: Unknown result type (might be due to invalid IL or missing references)
			//IL_461a: Unknown result type (might be due to invalid IL or missing references)
			//IL_461f: Unknown result type (might be due to invalid IL or missing references)
			//IL_46b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_46bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_47b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_47bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_483c: Unknown result type (might be due to invalid IL or missing references)
			//IL_4841: Unknown result type (might be due to invalid IL or missing references)
			//IL_4883: Unknown result type (might be due to invalid IL or missing references)
			//IL_4888: Unknown result type (might be due to invalid IL or missing references)
			//IL_48db: Unknown result type (might be due to invalid IL or missing references)
			//IL_48e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_4922: Unknown result type (might be due to invalid IL or missing references)
			//IL_4927: Unknown result type (might be due to invalid IL or missing references)
			//IL_494c: Unknown result type (might be due to invalid IL or missing references)
			//IL_4951: Unknown result type (might be due to invalid IL or missing references)
			//IL_4982: Unknown result type (might be due to invalid IL or missing references)
			//IL_4987: Unknown result type (might be due to invalid IL or missing references)
			//IL_49b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_49be: Unknown result type (might be due to invalid IL or missing references)
			//IL_49e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_49e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_4a75: Unknown result type (might be due to invalid IL or missing references)
			//IL_4a7a: Unknown result type (might be due to invalid IL or missing references)
			//IL_4ac5: Unknown result type (might be due to invalid IL or missing references)
			//IL_4aca: Unknown result type (might be due to invalid IL or missing references)
			//IL_4b15: Unknown result type (might be due to invalid IL or missing references)
			//IL_4b1a: Unknown result type (might be due to invalid IL or missing references)
			//IL_4bad: Unknown result type (might be due to invalid IL or missing references)
			//IL_4bb2: Unknown result type (might be due to invalid IL or missing references)
			//IL_4d3f: Unknown result type (might be due to invalid IL or missing references)
			//IL_4d44: Unknown result type (might be due to invalid IL or missing references)
			//IL_4da6: Unknown result type (might be due to invalid IL or missing references)
			//IL_4dab: Unknown result type (might be due to invalid IL or missing references)
			//IL_4ead: Unknown result type (might be due to invalid IL or missing references)
			//IL_4eb2: Unknown result type (might be due to invalid IL or missing references)
			//IL_4fa5: Unknown result type (might be due to invalid IL or missing references)
			//IL_4faa: Unknown result type (might be due to invalid IL or missing references)
			//IL_503b: Unknown result type (might be due to invalid IL or missing references)
			//IL_5040: Unknown result type (might be due to invalid IL or missing references)
			//IL_513e: Unknown result type (might be due to invalid IL or missing references)
			//IL_5143: Unknown result type (might be due to invalid IL or missing references)
			//IL_51ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_51b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_5205: Unknown result type (might be due to invalid IL or missing references)
			//IL_520a: Unknown result type (might be due to invalid IL or missing references)
			//IL_5280: Unknown result type (might be due to invalid IL or missing references)
			//IL_5285: Unknown result type (might be due to invalid IL or missing references)
			//IL_5367: Unknown result type (might be due to invalid IL or missing references)
			//IL_536c: Unknown result type (might be due to invalid IL or missing references)
			//IL_53db: Unknown result type (might be due to invalid IL or missing references)
			//IL_53e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_5453: Unknown result type (might be due to invalid IL or missing references)
			//IL_5458: Unknown result type (might be due to invalid IL or missing references)
			//IL_55e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_55ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_564d: Unknown result type (might be due to invalid IL or missing references)
			//IL_5652: Unknown result type (might be due to invalid IL or missing references)
			//IL_5688: Unknown result type (might be due to invalid IL or missing references)
			//IL_568d: Unknown result type (might be due to invalid IL or missing references)
			//IL_56e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_56e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_5770: Unknown result type (might be due to invalid IL or missing references)
			//IL_5775: Unknown result type (might be due to invalid IL or missing references)
			//IL_5810: Unknown result type (might be due to invalid IL or missing references)
			//IL_5815: Unknown result type (might be due to invalid IL or missing references)
			//IL_5868: Unknown result type (might be due to invalid IL or missing references)
			//IL_586d: Unknown result type (might be due to invalid IL or missing references)
			//IL_58ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_58bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_5901: Unknown result type (might be due to invalid IL or missing references)
			//IL_5906: Unknown result type (might be due to invalid IL or missing references)
			//IL_5a69: Unknown result type (might be due to invalid IL or missing references)
			//IL_5a6e: Unknown result type (might be due to invalid IL or missing references)
			//IL_5ab0: Unknown result type (might be due to invalid IL or missing references)
			//IL_5ab5: Unknown result type (might be due to invalid IL or missing references)
			//IL_5ae6: Unknown result type (might be due to invalid IL or missing references)
			//IL_5aeb: Unknown result type (might be due to invalid IL or missing references)
			//IL_5b30: Unknown result type (might be due to invalid IL or missing references)
			//IL_5b35: Unknown result type (might be due to invalid IL or missing references)
			//IL_5bc6: Unknown result type (might be due to invalid IL or missing references)
			//IL_5bcb: Unknown result type (might be due to invalid IL or missing references)
			//IL_5c01: Unknown result type (might be due to invalid IL or missing references)
			//IL_5c06: Unknown result type (might be due to invalid IL or missing references)
			//IL_5c3c: Unknown result type (might be due to invalid IL or missing references)
			//IL_5c41: Unknown result type (might be due to invalid IL or missing references)
			//IL_5c77: Unknown result type (might be due to invalid IL or missing references)
			//IL_5c7c: Unknown result type (might be due to invalid IL or missing references)
			//IL_5cb2: Unknown result type (might be due to invalid IL or missing references)
			//IL_5cb7: Unknown result type (might be due to invalid IL or missing references)
			//IL_5ced: Unknown result type (might be due to invalid IL or missing references)
			//IL_5cf2: Unknown result type (might be due to invalid IL or missing references)
			//IL_5da9: Unknown result type (might be due to invalid IL or missing references)
			//IL_5dae: Unknown result type (might be due to invalid IL or missing references)
			//IL_5e93: Unknown result type (might be due to invalid IL or missing references)
			//IL_5e98: Unknown result type (might be due to invalid IL or missing references)
			//IL_5ebd: Unknown result type (might be due to invalid IL or missing references)
			//IL_5ec2: Unknown result type (might be due to invalid IL or missing references)
			//IL_5ee7: Unknown result type (might be due to invalid IL or missing references)
			//IL_5eec: Unknown result type (might be due to invalid IL or missing references)
			//IL_5f71: Unknown result type (might be due to invalid IL or missing references)
			//IL_5f76: Unknown result type (might be due to invalid IL or missing references)
			//IL_5fa7: Unknown result type (might be due to invalid IL or missing references)
			//IL_5fac: Unknown result type (might be due to invalid IL or missing references)
			//IL_601d: Unknown result type (might be due to invalid IL or missing references)
			//IL_6022: Unknown result type (might be due to invalid IL or missing references)
			//IL_6075: Unknown result type (might be due to invalid IL or missing references)
			//IL_607a: Unknown result type (might be due to invalid IL or missing references)
			//IL_610d: Unknown result type (might be due to invalid IL or missing references)
			//IL_6112: Unknown result type (might be due to invalid IL or missing references)
			//IL_6137: Unknown result type (might be due to invalid IL or missing references)
			//IL_613c: Unknown result type (might be due to invalid IL or missing references)
			//IL_616d: Unknown result type (might be due to invalid IL or missing references)
			//IL_6172: Unknown result type (might be due to invalid IL or missing references)
			//IL_61a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_61a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_620c: Unknown result type (might be due to invalid IL or missing references)
			//IL_6211: Unknown result type (might be due to invalid IL or missing references)
			//IL_6275: Unknown result type (might be due to invalid IL or missing references)
			//IL_627a: Unknown result type (might be due to invalid IL or missing references)
			//IL_62b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_62bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_6312: Unknown result type (might be due to invalid IL or missing references)
			//IL_6317: Unknown result type (might be due to invalid IL or missing references)
			//IL_6349: Unknown result type (might be due to invalid IL or missing references)
			//IL_634e: Unknown result type (might be due to invalid IL or missing references)
			//IL_6380: Unknown result type (might be due to invalid IL or missing references)
			//IL_6385: Unknown result type (might be due to invalid IL or missing references)
			//IL_6424: Unknown result type (might be due to invalid IL or missing references)
			//IL_6429: Unknown result type (might be due to invalid IL or missing references)
			//IL_645f: Unknown result type (might be due to invalid IL or missing references)
			//IL_6464: Unknown result type (might be due to invalid IL or missing references)
			//IL_64ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_64bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_65a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_65a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_65dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_65e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_6637: Unknown result type (might be due to invalid IL or missing references)
			//IL_663c: Unknown result type (might be due to invalid IL or missing references)
			//IL_66b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_66bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_675a: Unknown result type (might be due to invalid IL or missing references)
			//IL_675f: Unknown result type (might be due to invalid IL or missing references)
			//IL_67e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_67ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_6830: Unknown result type (might be due to invalid IL or missing references)
			//IL_6835: Unknown result type (might be due to invalid IL or missing references)
			//IL_6877: Unknown result type (might be due to invalid IL or missing references)
			//IL_687c: Unknown result type (might be due to invalid IL or missing references)
			//IL_68c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_68ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_6969: Unknown result type (might be due to invalid IL or missing references)
			//IL_696e: Unknown result type (might be due to invalid IL or missing references)
			//IL_699f: Unknown result type (might be due to invalid IL or missing references)
			//IL_69a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_69c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_69ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_6a80: Unknown result type (might be due to invalid IL or missing references)
			//IL_6a85: Unknown result type (might be due to invalid IL or missing references)
			//IL_6adb: Unknown result type (might be due to invalid IL or missing references)
			//IL_6ae0: Unknown result type (might be due to invalid IL or missing references)
			//IL_6d16: Unknown result type (might be due to invalid IL or missing references)
			//IL_6d1b: Unknown result type (might be due to invalid IL or missing references)
			//IL_6d51: Unknown result type (might be due to invalid IL or missing references)
			//IL_6d56: Unknown result type (might be due to invalid IL or missing references)
			//IL_6d7b: Unknown result type (might be due to invalid IL or missing references)
			//IL_6d80: Unknown result type (might be due to invalid IL or missing references)
			//IL_6dd6: Unknown result type (might be due to invalid IL or missing references)
			//IL_6ddb: Unknown result type (might be due to invalid IL or missing references)
			//IL_6e11: Unknown result type (might be due to invalid IL or missing references)
			//IL_6e16: Unknown result type (might be due to invalid IL or missing references)
			//IL_6e4c: Unknown result type (might be due to invalid IL or missing references)
			//IL_6e51: Unknown result type (might be due to invalid IL or missing references)
			//IL_6f0d: Unknown result type (might be due to invalid IL or missing references)
			//IL_6f12: Unknown result type (might be due to invalid IL or missing references)
			//IL_6f65: Unknown result type (might be due to invalid IL or missing references)
			//IL_6f6a: Unknown result type (might be due to invalid IL or missing references)
			//IL_7108: Unknown result type (might be due to invalid IL or missing references)
			//IL_710d: Unknown result type (might be due to invalid IL or missing references)
			//IL_714f: Unknown result type (might be due to invalid IL or missing references)
			//IL_7154: Unknown result type (might be due to invalid IL or missing references)
			//IL_7196: Unknown result type (might be due to invalid IL or missing references)
			//IL_719b: Unknown result type (might be due to invalid IL or missing references)
			//IL_71dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_71e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_7224: Unknown result type (might be due to invalid IL or missing references)
			//IL_7229: Unknown result type (might be due to invalid IL or missing references)
			//IL_726b: Unknown result type (might be due to invalid IL or missing references)
			//IL_7270: Unknown result type (might be due to invalid IL or missing references)
			return new Dictionary<int, NPCBestiaryDrawModifiers>
			{
				{
					-65,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					-64,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					-63,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					-62,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					-61,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(2f, 4f),
						Scale = 1.1f,
						Hide = true
					}
				},
				{
					-60,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(1f, 3f),
						Scale = 0.9f,
						Hide = true
					}
				},
				{
					-59,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					-58,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					-57,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					-56,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					-55,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true,
						Velocity = 1f,
						Scale = 1.1f
					}
				},
				{
					-54,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true,
						Velocity = 1f,
						Scale = 0.9f
					}
				},
				{
					-53,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f,
						Scale = 1.1f,
						Hide = true
					}
				},
				{
					-52,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f,
						Scale = 0.9f,
						Hide = true
					}
				},
				{
					-51,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f,
						Scale = 1.1f,
						Hide = true
					}
				},
				{
					-50,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f,
						Scale = 0.9f,
						Hide = true
					}
				},
				{
					-49,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f,
						Scale = 1.1f,
						Hide = true
					}
				},
				{
					-48,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f,
						Scale = 0.9f,
						Hide = true
					}
				},
				{
					-47,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f,
						Scale = 1.1f,
						Hide = true
					}
				},
				{
					-46,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f,
						Scale = 0.9f,
						Hide = true
					}
				},
				{
					-45,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f,
						Scale = 1.1f,
						Hide = true
					}
				},
				{
					-44,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f,
						Scale = 0.9f,
						Hide = true
					}
				},
				{
					-43,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(5f, -15f),
						Hide = true
					}
				},
				{
					-42,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(5f, -15f),
						Hide = true
					}
				},
				{
					-41,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(5f, -15f),
						Hide = true
					}
				},
				{
					-40,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(5f, -15f),
						Hide = true
					}
				},
				{
					-39,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(5f, -15f),
						Hide = true
					}
				},
				{
					-38,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(5f, -15f),
						Hide = true
					}
				},
				{
					-37,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f,
						Scale = 1.1f,
						Hide = true
					}
				},
				{
					-36,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f,
						Scale = 0.9f,
						Hide = true
					}
				},
				{
					-35,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f,
						Scale = 1.1f,
						Hide = true
					}
				},
				{
					-34,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f,
						Scale = 0.9f,
						Hide = true
					}
				},
				{
					-33,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f,
						Scale = 1.1f,
						Hide = true
					}
				},
				{
					-32,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f,
						Scale = 0.9f,
						Hide = true
					}
				},
				{
					-31,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f,
						Scale = 1.1f,
						Hide = true
					}
				},
				{
					-30,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f,
						Scale = 0.9f,
						Hide = true
					}
				},
				{
					-29,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f,
						Scale = 1.1f,
						Hide = true
					}
				},
				{
					-28,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f,
						Scale = 0.9f,
						Hide = true
					}
				},
				{
					-27,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f,
						Scale = 1.1f,
						Hide = true
					}
				},
				{
					-26,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f,
						Scale = 0.9f,
						Hide = true
					}
				},
				{
					-23,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(5f, -9f),
						Rotation = 0.75f,
						Scale = 1.2f,
						Hide = true
					}
				},
				{
					-22,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(5f, -9f),
						Rotation = 0.75f,
						Scale = 0.8f,
						Hide = true
					}
				},
				{
					-25,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					-24,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					-21,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(2f, 5f),
						Scale = 1.2f,
						Hide = true
					}
				},
				{
					-20,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(2f, 4f),
						Scale = 1.1f,
						Hide = true
					}
				},
				{
					-19,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(1f, 3f),
						Scale = 0.9f,
						Hide = true
					}
				},
				{
					-18,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(0f, 2f),
						Scale = 0.8f,
						Hide = true
					}
				},
				{
					-17,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(0f, 3f),
						Scale = 1.2f,
						Hide = true
					}
				},
				{
					-16,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(2f, 3f),
						Scale = 0.8f,
						Hide = true
					}
				},
				{
					-15,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f,
						Scale = 1.2f,
						Hide = true
					}
				},
				{
					-14,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f,
						Scale = 1.1f,
						Hide = true
					}
				},
				{
					-13,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f,
						Scale = 0.9f,
						Hide = true
					}
				},
				{
					-12,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(5f, -9f),
						Rotation = 0.75f,
						Scale = 1.2f,
						Hide = true
					}
				},
				{
					-11,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(5f, -9f),
						Rotation = 0.75f,
						Scale = 0.8f,
						Hide = true
					}
				},
				{
					2,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(6f, -15f),
						PortraitPositionYOverride = -35f
					}
				},
				{
					3,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f,
						PortraitPositionXOverride = 2f
					}
				},
				{
					4,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(25f, -30f),
						Rotation = 0.7f,
						Frame = 4
					}
				},
				{
					5,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(3f, 4f),
						Rotation = 1.5f
					}
				},
				{
					6,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(5f, -9f),
						Rotation = 0.75f
					}
				},
				{
					7,
					new NPCBestiaryDrawModifiers(0)
					{
						CustomTexturePath = "Images/UI/Bestiary/NPCs/NPC_7",
						Position = new Vector2(20f, 29f),
						PortraitPositionXOverride = 0f,
						PortraitPositionYOverride = 10f
					}
				},
				{
					8,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					9,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					10,
					new NPCBestiaryDrawModifiers(0)
					{
						CustomTexturePath = "Images/UI/Bestiary/NPCs/NPC_10",
						Position = new Vector2(2f, 24f),
						PortraitPositionXOverride = 0f,
						PortraitPositionYOverride = 10f
					}
				},
				{
					11,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					12,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					13,
					new NPCBestiaryDrawModifiers(0)
					{
						CustomTexturePath = "Images/UI/Bestiary/NPCs/NPC_13",
						Position = new Vector2(40f, 22f),
						PortraitPositionXOverride = 0f,
						PortraitPositionYOverride = 10f
					}
				},
				{
					14,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					15,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					17,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					18,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f,
						PortraitPositionXOverride = 2f
					}
				},
				{
					19,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					20,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f,
						PortraitPositionXOverride = 2f
					}
				},
				{
					22,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					25,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					26,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					27,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					28,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					30,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					665,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					31,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					33,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					34,
					new NPCBestiaryDrawModifiers(0)
					{
						Direction = 1
					}
				},
				{
					21,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					35,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(0f, -12f),
						Scale = 0.9f,
						PortraitPositionXOverride = -1f,
						PortraitPositionYOverride = -3f
					}
				},
				{
					36,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					38,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					37,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					39,
					new NPCBestiaryDrawModifiers(0)
					{
						CustomTexturePath = "Images/UI/Bestiary/NPCs/NPC_39",
						Position = new Vector2(40f, 23f),
						PortraitPositionXOverride = 0f,
						PortraitPositionYOverride = 10f
					}
				},
				{
					40,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					41,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					43,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(4f, -6f),
						Rotation = (float)Math.PI * 3f / 4f
					}
				},
				{
					44,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					46,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					47,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					48,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(2f, -14f),
						PortraitPositionYOverride = -30f
					}
				},
				{
					49,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(0f, -13f),
						PortraitPositionYOverride = -30f
					}
				},
				{
					50,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(0f, 90f),
						PortraitScale = 1.1f,
						PortraitPositionYOverride = 70f
					}
				},
				{
					51,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(0f, -13f),
						PortraitPositionYOverride = -30f
					}
				},
				{
					52,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					53,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					54,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					55,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(0f, 6f),
						PortraitPositionYOverride = 7f,
						IsWet = true
					}
				},
				{
					56,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(4f, -6f),
						Rotation = (float)Math.PI * 3f / 4f
					}
				},
				{
					57,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(0f, 6f),
						PortraitPositionYOverride = 6f,
						IsWet = true
					}
				},
				{
					58,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(0f, 6f),
						PortraitPositionYOverride = 6f,
						IsWet = true
					}
				},
				{
					60,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(0f, -19f),
						PortraitPositionYOverride = -36f
					}
				},
				{
					61,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 0.05f,
						PortraitPositionYOverride = -15f
					}
				},
				{
					62,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(0f, -10f),
						PortraitPositionYOverride = -25f
					}
				},
				{
					65,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(35f, 4f),
						Velocity = 1f,
						PortraitPositionXOverride = 5f,
						IsWet = true
					}
				},
				{
					66,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(0f, -6f),
						Scale = 0.9f,
						PortraitPositionYOverride = -15f
					}
				},
				{
					68,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(-1f, -12f),
						Scale = 0.9f,
						PortraitPositionYOverride = -3f
					}
				},
				{
					70,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					72,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					73,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					74,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(1f, -14f),
						Velocity = 0.05f,
						PortraitPositionYOverride = -30f
					}
				},
				{
					75,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(0f, 6f)
					}
				},
				{
					76,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					77,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					78,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 0.5f
					}
				},
				{
					79,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 0.5f
					}
				},
				{
					80,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 0.5f
					}
				},
				{
					83,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(-4f, -4f),
						Scale = 0.9f,
						PortraitPositionYOverride = -25f
					}
				},
				{
					84,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(6f, -11f),
						PortraitPositionXOverride = 0f,
						PortraitPositionYOverride = -28f
					}
				},
				{
					86,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(20f, 6f),
						Velocity = 1f,
						PortraitPositionXOverride = 0f,
						PortraitPositionYOverride = 2f
					}
				},
				{
					87,
					new NPCBestiaryDrawModifiers(0)
					{
						CustomTexturePath = "Images/UI/Bestiary/NPCs/NPC_87",
						Position = new Vector2(55f, 15f),
						PortraitPositionXOverride = 4f,
						PortraitPositionYOverride = 10f
					}
				},
				{
					88,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					89,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					90,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					91,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					92,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					93,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(4f, -11f),
						PortraitPositionYOverride = -30f
					}
				},
				{
					94,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(8f, 0f),
						Rotation = 0.75f
					}
				},
				{
					95,
					new NPCBestiaryDrawModifiers(0)
					{
						CustomTexturePath = "Images/UI/Bestiary/NPCs/NPC_95",
						Position = new Vector2(20f, 28f),
						PortraitPositionXOverride = 0f,
						PortraitPositionYOverride = 10f
					}
				},
				{
					96,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					97,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					98,
					new NPCBestiaryDrawModifiers(0)
					{
						CustomTexturePath = "Images/UI/Bestiary/NPCs/NPC_98",
						Position = new Vector2(40f, 24f),
						PortraitPositionXOverride = 0f,
						PortraitPositionYOverride = 12f
					}
				},
				{
					99,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					100,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					101,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(-2f, 6f),
						Rotation = (float)Math.PI * 3f / 4f
					}
				},
				{
					102,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(0f, 6f),
						PortraitPositionYOverride = 6f,
						IsWet = true
					}
				},
				{
					104,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					105,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					106,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					107,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					108,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					109,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(0f, 35f),
						Velocity = 1f,
						PortraitPositionYOverride = 0f
					}
				},
				{
					110,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 3f
					}
				},
				{
					111,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 3f
					}
				},
				{
					112,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					666,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					113,
					new NPCBestiaryDrawModifiers(0)
					{
						CustomTexturePath = "Images/UI/Bestiary/NPCs/NPC_113",
						Position = new Vector2(56f, 5f),
						PortraitPositionXOverride = 10f,
						PortraitPositionYOverride = 0f
					}
				},
				{
					114,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					115,
					new NPCBestiaryDrawModifiers(0)
					{
						CustomTexturePath = "Images/UI/Bestiary/NPCs/NPC_115",
						Position = new Vector2(56f, 3f),
						PortraitPositionXOverride = 55f,
						PortraitPositionYOverride = 0f
					}
				},
				{
					116,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(3f, -5f),
						PortraitPositionXOverride = 4f,
						PortraitPositionYOverride = -26f
					}
				},
				{
					117,
					new NPCBestiaryDrawModifiers(0)
					{
						CustomTexturePath = "Images/UI/Bestiary/NPCs/NPC_117",
						Position = new Vector2(10f, 20f),
						PortraitPositionXOverride = 0f,
						PortraitPositionYOverride = 0f
					}
				},
				{
					118,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					119,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					120,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					123,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					124,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f,
						PortraitPositionXOverride = 2f
					}
				},
				{
					121,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(-2f, -4f),
						PortraitPositionYOverride = -20f
					}
				},
				{
					122,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(0f, 4f)
					}
				},
				{
					125,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(-28f, -23f),
						Rotation = -0.75f
					}
				},
				{
					126,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(28f, 30f),
						Rotation = 2.25f
					}
				},
				{
					127,
					new NPCBestiaryDrawModifiers(0)
					{
						CustomTexturePath = "Images/UI/Bestiary/NPCs/NPC_127",
						Position = new Vector2(0f, 0f),
						PortraitPositionXOverride = 0f,
						PortraitPositionYOverride = 1f
					}
				},
				{
					128,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(-6f, -2f),
						Rotation = -0.75f,
						Hide = true
					}
				},
				{
					129,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(4f, 4f),
						Rotation = 0.75f,
						Hide = true
					}
				},
				{
					130,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(6f, 8f),
						Rotation = 2.25f,
						Hide = true
					}
				},
				{
					131,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(-8f, 8f),
						Rotation = -2.25f,
						Hide = true
					}
				},
				{
					132,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					133,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(0f, -5f),
						PortraitPositionYOverride = -25f
					}
				},
				{
					134,
					new NPCBestiaryDrawModifiers(0)
					{
						CustomTexturePath = "Images/UI/Bestiary/NPCs/NPC_134",
						Position = new Vector2(60f, 8f),
						PortraitPositionXOverride = 3f,
						PortraitPositionYOverride = 0f
					}
				},
				{
					135,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					136,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					137,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(4f, -11f),
						PortraitPositionYOverride = -30f
					}
				},
				{
					140,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					142,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f,
						PortraitPositionXOverride = 1f
					}
				},
				{
					146,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					148,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					149,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					150,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(0f, -11f),
						PortraitPositionYOverride = -30f
					}
				},
				{
					151,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(0f, -11f),
						PortraitPositionYOverride = -30f
					}
				},
				{
					152,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(2f, -11f),
						PortraitPositionYOverride = -30f
					}
				},
				{
					153,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(20f, 0f),
						Velocity = 1f,
						PortraitPositionXOverride = 0f
					}
				},
				{
					154,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(20f, 0f),
						Velocity = 1f,
						PortraitPositionXOverride = 0f
					}
				},
				{
					155,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(15f, 0f),
						Velocity = 3f,
						PortraitPositionXOverride = 0f
					}
				},
				{
					156,
					new NPCBestiaryDrawModifiers(0)
					{
						PortraitPositionYOverride = -15f
					}
				},
				{
					157,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(20f, 5f),
						PortraitPositionXOverride = 5f,
						PortraitPositionYOverride = 10f,
						IsWet = true
					}
				},
				{
					160,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					158,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(0f, -11f),
						PortraitPositionYOverride = -30f
					}
				},
				{
					159,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					161,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					162,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					163,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					164,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					165,
					new NPCBestiaryDrawModifiers(0)
					{
						Rotation = -1.6f,
						Velocity = 2f
					}
				},
				{
					167,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					168,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					170,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(10f, 5f),
						PortraitPositionXOverride = 0f,
						PortraitPositionYOverride = -12f
					}
				},
				{
					171,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(10f, 5f),
						PortraitPositionXOverride = 0f,
						PortraitPositionYOverride = -12f
					}
				},
				{
					173,
					new NPCBestiaryDrawModifiers(0)
					{
						Rotation = 0.75f,
						Position = new Vector2(0f, -5f)
					}
				},
				{
					174,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(0f, -5f),
						Velocity = 1f
					}
				},
				{
					175,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(4f, -2f),
						Rotation = (float)Math.PI * 3f / 4f
					}
				},
				{
					176,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(0f, 5f)
					}
				},
				{
					177,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(10f, 15f),
						PortraitPositionXOverride = -4f,
						PortraitPositionYOverride = 1f,
						Frame = 0
					}
				},
				{
					178,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f,
						PortraitPositionXOverride = 2f
					}
				},
				{
					179,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(-5f, 12f),
						PortraitPositionYOverride = -7f
					}
				},
				{
					180,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(10f, 5f),
						PortraitPositionXOverride = 0f,
						PortraitPositionYOverride = -12f
					}
				},
				{
					181,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					185,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					186,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					187,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					188,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					189,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					190,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(6f, -15f),
						PortraitPositionYOverride = -35f
					}
				},
				{
					191,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(6f, -15f),
						PortraitPositionYOverride = -35f
					}
				},
				{
					192,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(6f, -15f),
						PortraitPositionYOverride = -35f
					}
				},
				{
					193,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(5f, -15f),
						PortraitPositionYOverride = -35f
					}
				},
				{
					194,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(5f, -15f),
						PortraitPositionYOverride = -35f
					}
				},
				{
					196,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					197,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					198,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					199,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					200,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f,
						PortraitPositionYOverride = 2f
					}
				},
				{
					201,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					202,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					203,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					206,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 2f
					}
				},
				{
					207,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					208,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f,
						PortraitPositionXOverride = 2f
					}
				},
				{
					209,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					212,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					213,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					214,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 3f
					}
				},
				{
					215,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 3f
					}
				},
				{
					216,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 3f
					}
				},
				{
					221,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(0f, 5f),
						Velocity = 1f
					}
				},
				{
					222,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(10f, 55f),
						PortraitPositionXOverride = 0f,
						PortraitPositionYOverride = 40f
					}
				},
				{
					223,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					224,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(5f, -10f),
						PortraitPositionYOverride = -30f
					}
				},
				{
					226,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(5f, 3f),
						PortraitPositionYOverride = -15f
					}
				},
				{
					227,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f,
						PortraitPositionXOverride = -3f
					}
				},
				{
					228,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f,
						PortraitPositionXOverride = -5f
					}
				},
				{
					229,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					225,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(2f, 0f)
					}
				},
				{
					230,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					231,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(0f, 5f)
					}
				},
				{
					232,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(0f, 5f)
					}
				},
				{
					233,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(0f, 5f)
					}
				},
				{
					234,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(0f, 5f)
					}
				},
				{
					235,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(0f, 5f)
					}
				},
				{
					236,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					237,
					new NPCBestiaryDrawModifiers(0)
					{
						Rotation = -1.6f,
						Velocity = 2f
					}
				},
				{
					238,
					new NPCBestiaryDrawModifiers(0)
					{
						Rotation = -1.6f,
						Velocity = 2f
					}
				},
				{
					239,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					240,
					new NPCBestiaryDrawModifiers(0)
					{
						Rotation = -1.6f,
						Velocity = 2f
					}
				},
				{
					241,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(0f, 6f),
						PortraitPositionYOverride = 6f,
						IsWet = true
					}
				},
				{
					242,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(0f, 10f)
					}
				},
				{
					243,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(0f, 60f),
						Velocity = 1f,
						PortraitPositionYOverride = 15f
					}
				},
				{
					245,
					new NPCBestiaryDrawModifiers(0)
					{
						CustomTexturePath = "Images/UI/Bestiary/NPCs/NPC_245",
						Position = new Vector2(2f, 48f),
						PortraitPositionXOverride = 0f,
						PortraitPositionYOverride = 24f
					}
				},
				{
					246,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					247,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					248,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					249,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					250,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(0f, -6f),
						PortraitPositionYOverride = -26f
					}
				},
				{
					251,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					252,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(3f, 3f),
						Velocity = 0.05f
					}
				},
				{
					253,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(0f, 5f)
					}
				},
				{
					254,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					255,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f,
						PortraitPositionXOverride = -2f
					}
				},
				{
					256,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(0f, 5f)
					}
				},
				{
					257,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					258,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					259,
					new NPCBestiaryDrawModifiers(0)
					{
						CustomTexturePath = "Images/UI/Bestiary/NPCs/NPC_259",
						Position = new Vector2(0f, 25f),
						PortraitPositionXOverride = 0f,
						PortraitPositionYOverride = 8f
					}
				},
				{
					260,
					new NPCBestiaryDrawModifiers(0)
					{
						CustomTexturePath = "Images/UI/Bestiary/NPCs/NPC_260",
						Position = new Vector2(0f, 25f),
						PortraitPositionXOverride = 1f,
						PortraitPositionYOverride = 4f
					}
				},
				{
					261,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					262,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(0f, 20f),
						Scale = 0.8f
					}
				},
				{
					264,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					263,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					265,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					266,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(5f, 5f),
						Frame = 4
					}
				},
				{
					268,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(2f, -5f)
					}
				},
				{
					269,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(-5f, 0f),
						Velocity = 1f
					}
				},
				{
					270,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(-5f, 0f),
						Velocity = 1f
					}
				},
				{
					271,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(-2f, 0f),
						Velocity = 1f
					}
				},
				{
					272,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(-5f, 0f),
						Velocity = 1f
					}
				},
				{
					273,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(-5f, 0f),
						Velocity = 1f
					}
				},
				{
					274,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(-3f, 0f),
						Velocity = 1f
					}
				},
				{
					275,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(-3f, 2f),
						PortraitPositionYOverride = 3f,
						Velocity = 1f
					}
				},
				{
					276,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(-5f, 0f),
						Velocity = 1f
					}
				},
				{
					277,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(-5f, 0f),
						Velocity = 1f
					}
				},
				{
					278,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(-2f, 0f),
						Velocity = 1f
					}
				},
				{
					279,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(-5f, 0f),
						Velocity = 1f
					}
				},
				{
					280,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(-3f, 0f),
						Velocity = 1f
					}
				},
				{
					287,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					289,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(2f, 10f),
						Direction = 1
					}
				},
				{
					290,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(4f, 6f),
						Velocity = 1f
					}
				},
				{
					291,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					292,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					293,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					294,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					295,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					296,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					297,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(1f, -14f),
						Velocity = 0.05f,
						PortraitPositionYOverride = -30f
					}
				},
				{
					298,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(1f, -14f),
						Velocity = 0.05f,
						PortraitPositionYOverride = -30f
					}
				},
				{
					299,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					301,
					new NPCBestiaryDrawModifiers(0)
					{
						PortraitPositionYOverride = -20f,
						Direction = -1,
						SpriteDirection = 1,
						Velocity = 0.05f
					}
				},
				{
					303,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					305,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 0.05f
					}
				},
				{
					306,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 0.05f
					}
				},
				{
					307,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 0.05f
					}
				},
				{
					308,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 0.05f
					}
				},
				{
					309,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 0.05f
					}
				},
				{
					310,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					311,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					312,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					313,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					314,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					315,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(14f, 26f),
						Velocity = 2f,
						PortraitPositionXOverride = 0f,
						PortraitPositionYOverride = 0f
					}
				},
				{
					316,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(0f, 4f)
					}
				},
				{
					317,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(5f, -15f),
						PortraitPositionYOverride = -35f
					}
				},
				{
					318,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(5f, -13f),
						PortraitPositionYOverride = -31f
					}
				},
				{
					319,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					320,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					321,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f,
						PortraitPositionXOverride = 2f
					}
				},
				{
					322,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					323,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					324,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					325,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(0f, 36f)
					}
				},
				{
					326,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					327,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(0f, -8f)
					}
				},
				{
					328,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					329,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 2f
					}
				},
				{
					330,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(2f, 14f)
					}
				},
				{
					331,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					332,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					337,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					338,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					339,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					340,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					342,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					343,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(5f, 25f),
						Velocity = 1f,
						PortraitPositionYOverride = 0f
					}
				},
				{
					344,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(0f, 90f)
					}
				},
				{
					345,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(-1f, 90f)
					}
				},
				{
					346,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(30f, 80f),
						PortraitPositionXOverride = 0f,
						PortraitPositionYOverride = 60f
					}
				},
				{
					347,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(0f, 4f)
					}
				},
				{
					348,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f,
						Hide = true
					}
				},
				{
					349,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(-3f, 18f),
						PortraitPositionYOverride = 0f
					}
				},
				{
					350,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(3f, 0f),
						Velocity = 2f
					}
				},
				{
					351,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(6f, 60f),
						Velocity = 1f,
						PortraitPositionYOverride = 30f
					}
				},
				{
					353,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f,
						PortraitPositionXOverride = 2f
					}
				},
				{
					633,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f,
						PortraitPositionXOverride = 2f
					}
				},
				{
					354,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					355,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(1f, 2f)
					}
				},
				{
					356,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(2f, 3f),
						PortraitPositionYOverride = 1f
					}
				},
				{
					357,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(0f, 2f),
						Velocity = 1f
					}
				},
				{
					358,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(1f, 2f)
					}
				},
				{
					359,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(0f, 18f),
						PortraitPositionYOverride = 40f
					}
				},
				{
					360,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(1f, 17f),
						PortraitPositionYOverride = 39f
					}
				},
				{
					362,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(3f, 0f),
						Velocity = 1f
					}
				},
				{
					363,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f,
						Hide = true
					}
				},
				{
					364,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(3f, 0f),
						Velocity = 1f
					}
				},
				{
					365,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f,
						Hide = true
					}
				},
				{
					366,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(1f, 0f),
						Velocity = 1f
					}
				},
				{
					367,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(2f, 0f),
						Velocity = 1f
					}
				},
				{
					368,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					369,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f,
						PortraitPositionXOverride = 2f
					}
				},
				{
					370,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(56f, -4f),
						Direction = 1,
						PortraitPositionXOverride = 0f,
						PortraitPositionYOverride = 0f
					}
				},
				{
					371,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					372,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(35f, 4f),
						Velocity = 1f,
						PortraitPositionXOverride = 10f,
						PortraitPositionYOverride = -3f
					}
				},
				{
					373,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					374,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					375,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					376,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					379,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					380,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					381,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(-2f, 0f),
						Velocity = 1f
					}
				},
				{
					382,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(-2f, 0f),
						Velocity = 1f
					}
				},
				{
					383,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(-2f, 0f),
						Velocity = 1f
					}
				},
				{
					384,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					385,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(-2f, 0f),
						Velocity = 1f
					}
				},
				{
					386,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(-2f, 0f),
						Velocity = 1f
					}
				},
				{
					387,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(2f, 0f),
						Velocity = 3f
					}
				},
				{
					388,
					new NPCBestiaryDrawModifiers(0)
					{
						Direction = 1
					}
				},
				{
					389,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(-6f, 0f),
						Velocity = 1f
					}
				},
				{
					390,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(12f, 0f),
						Direction = -1,
						SpriteDirection = 1,
						Velocity = 2f,
						PortraitPositionXOverride = 0f,
						PortraitPositionYOverride = -12f
					}
				},
				{
					391,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(16f, 12f),
						Direction = -1,
						SpriteDirection = 1,
						Velocity = 2f,
						PortraitPositionXOverride = 3f,
						PortraitPositionYOverride = 0f
					}
				},
				{
					392,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					395,
					new NPCBestiaryDrawModifiers(0)
					{
						CustomTexturePath = "Images/UI/Bestiary/NPCs/NPC_395",
						Position = new Vector2(-1f, 18f),
						PortraitPositionXOverride = 1f,
						PortraitPositionYOverride = 0f
					}
				},
				{
					393,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					394,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					396,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					397,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					398,
					new NPCBestiaryDrawModifiers(0)
					{
						CustomTexturePath = "Images/UI/Bestiary/NPCs/NPC_398",
						Position = new Vector2(0f, 5f),
						Scale = 0.4f,
						PortraitScale = 0.7f,
						PortraitPositionYOverride = 0f
					}
				},
				{
					400,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					401,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					402,
					new NPCBestiaryDrawModifiers(0)
					{
						CustomTexturePath = "Images/UI/Bestiary/NPCs/NPC_402",
						Position = new Vector2(42f, 15f),
						PortraitPositionXOverride = 0f,
						PortraitPositionYOverride = 0f
					}
				},
				{
					403,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					404,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					408,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					410,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					411,
					new NPCBestiaryDrawModifiers(0)
					{
						Direction = -1,
						SpriteDirection = 1,
						Velocity = 1f
					}
				},
				{
					412,
					new NPCBestiaryDrawModifiers(0)
					{
						CustomTexturePath = "Images/UI/Bestiary/NPCs/NPC_412",
						Position = new Vector2(50f, 28f),
						PortraitPositionXOverride = 0f,
						PortraitPositionYOverride = 4f
					}
				},
				{
					413,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					414,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					415,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(26f, 0f),
						Velocity = 3f,
						PortraitPositionXOverride = 5f
					}
				},
				{
					416,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(6f, 20f),
						Velocity = 1f,
						PortraitPositionXOverride = 0f,
						PortraitPositionYOverride = 0f
					}
				},
				{
					417,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(-2f, 8f),
						Velocity = 1f,
						PortraitPositionYOverride = 0f
					}
				},
				{
					418,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(2f, 4f)
					}
				},
				{
					419,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(-2f, 0f),
						Velocity = 1f
					}
				},
				{
					420,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(0f, 4f),
						Direction = 1
					}
				},
				{
					421,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(1f, -1f)
					}
				},
				{
					422,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(1f, 44f),
						Scale = 0.4f,
						PortraitPositionXOverride = 2f,
						PortraitPositionYOverride = 134f
					}
				},
				{
					423,
					new NPCBestiaryDrawModifiers(0)
					{
						Direction = -1,
						SpriteDirection = 1,
						Velocity = 1f
					}
				},
				{
					424,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(4f, 0f),
						Direction = -1,
						SpriteDirection = 1,
						Velocity = 2f
					}
				},
				{
					425,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(4f, 0f),
						Direction = -1,
						SpriteDirection = 1,
						Velocity = 2f
					}
				},
				{
					426,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(2f, 8f),
						Velocity = 2f,
						PortraitPositionYOverride = 0f
					}
				},
				{
					427,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f,
						PortraitPositionYOverride = -4f
					}
				},
				{
					428,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					429,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(2f, 0f),
						Velocity = 1f
					}
				},
				{
					430,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					431,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					432,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					433,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					434,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					435,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					436,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					437,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					439,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					440,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					441,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					442,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(1f, -14f),
						Velocity = 0.05f,
						PortraitPositionYOverride = -30f
					}
				},
				{
					443,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					444,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(2f, 2f),
						PortraitPositionYOverride = 0f
					}
				},
				{
					448,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					449,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					450,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					451,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					452,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					453,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					454,
					new NPCBestiaryDrawModifiers(0)
					{
						CustomTexturePath = "Images/UI/Bestiary/NPCs/NPC_454",
						Position = new Vector2(57f, 10f),
						PortraitPositionXOverride = 5f,
						PortraitPositionYOverride = 0f
					}
				},
				{
					455,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					456,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					457,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					458,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					459,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					460,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					461,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(-2f, 0f),
						Velocity = 1f
					}
				},
				{
					462,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					463,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					464,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					465,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(0f, 6f),
						PortraitPositionYOverride = 6f,
						IsWet = true
					}
				},
				{
					466,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					467,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					468,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					469,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					470,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					471,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					472,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(0f, 0f),
						PortraitPositionYOverride = -30f,
						SpriteDirection = -1,
						Velocity = 1f
					}
				},
				{
					476,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					477,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(25f, 6f),
						PortraitPositionXOverride = 10f
					}
				},
				{
					478,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					479,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(5f, 4f),
						PortraitPositionYOverride = -15f
					}
				},
				{
					481,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					482,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					483,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(0f, -10f),
						PortraitPositionYOverride = -30f
					}
				},
				{
					484,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					487,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 0.5f
					}
				},
				{
					486,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 0.5f
					}
				},
				{
					485,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 0.5f
					}
				},
				{
					489,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					491,
					new NPCBestiaryDrawModifiers(0)
					{
						CustomTexturePath = "Images/UI/Bestiary/NPCs/NPC_491",
						Position = new Vector2(30f, -5f),
						Scale = 0.8f,
						PortraitPositionXOverride = 1f,
						PortraitPositionYOverride = -1f
					}
				},
				{
					492,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					493,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(1f, 44f),
						Scale = 0.4f,
						PortraitPositionXOverride = 2f,
						PortraitPositionYOverride = 134f
					}
				},
				{
					494,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					495,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(-4f, 0f),
						Velocity = 1f
					}
				},
				{
					496,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 0.5f
					}
				},
				{
					497,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 0.5f
					}
				},
				{
					498,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					499,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					500,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					501,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					502,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					503,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					504,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					505,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					506,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					507,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(1f, 44f),
						Scale = 0.4f,
						PortraitPositionXOverride = 2f,
						PortraitPositionYOverride = 134f
					}
				},
				{
					508,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f,
						Position = new Vector2(10f, 0f),
						PortraitPositionXOverride = 0f
					}
				},
				{
					509,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(6f, 0f),
						PortraitPositionXOverride = -10f,
						PortraitPositionYOverride = -20f
					}
				},
				{
					510,
					new NPCBestiaryDrawModifiers(0)
					{
						CustomTexturePath = "Images/UI/Bestiary/NPCs/NPC_510",
						Position = new Vector2(55f, 18f),
						PortraitPositionXOverride = 10f,
						PortraitPositionYOverride = 12f
					}
				},
				{
					512,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					511,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					513,
					new NPCBestiaryDrawModifiers(0)
					{
						CustomTexturePath = "Images/UI/Bestiary/NPCs/NPC_513",
						Position = new Vector2(37f, 24f),
						PortraitPositionXOverride = 10f,
						PortraitPositionYOverride = 17f
					}
				},
				{
					514,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					515,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					516,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					517,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(1f, 44f),
						Scale = 0.4f,
						PortraitPositionXOverride = 2f,
						PortraitPositionYOverride = 135f
					}
				},
				{
					518,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(-17f, 0f),
						Velocity = 1f
					}
				},
				{
					519,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					520,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(0f, 56f),
						Velocity = 1f,
						PortraitPositionYOverride = 10f
					}
				},
				{
					521,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(5f, 5f),
						PortraitPositionYOverride = -10f,
						SpriteDirection = -1,
						Velocity = 0.05f
					}
				},
				{
					522,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					523,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					524,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					525,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					526,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					527,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					528,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					529,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					530,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					531,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(0f, 4f),
						Velocity = 2f,
						PortraitPositionYOverride = 10f
					}
				},
				{
					532,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(6f, 0f),
						Velocity = 1f
					}
				},
				{
					533,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(3f, 5f)
					}
				},
				{
					534,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					536,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(-2f, 0f),
						Velocity = 1f
					}
				},
				{
					538,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					539,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					540,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					541,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(0f, 30f),
						PortraitPositionYOverride = 0f
					}
				},
				{
					542,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(35f, -3f),
						PortraitPositionXOverride = 0f
					}
				},
				{
					543,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(35f, -3f),
						PortraitPositionXOverride = 0f
					}
				},
				{
					544,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(35f, -3f),
						PortraitPositionXOverride = 0f
					}
				},
				{
					545,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(35f, -3f),
						PortraitPositionXOverride = 0f
					}
				},
				{
					546,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(0f, -3f),
						Direction = 1
					}
				},
				{
					547,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					548,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					549,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					550,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f,
						PortraitPositionXOverride = -2f
					}
				},
				{
					551,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(95f, -4f)
					}
				},
				{
					552,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					553,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					554,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					555,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					556,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					557,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					558,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(3f, -2f)
					}
				},
				{
					559,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(3f, -2f)
					}
				},
				{
					560,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(3f, -2f)
					}
				},
				{
					561,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					562,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					563,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					566,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(-3f, 0f),
						Velocity = 1f
					}
				},
				{
					567,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(-3f, 0f),
						Velocity = 1f
					}
				},
				{
					568,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					569,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					570,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(10f, 5f),
						Velocity = 1f,
						PortraitPositionXOverride = 0f,
						PortraitPositionYOverride = 2f
					}
				},
				{
					571,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(10f, 5f),
						Velocity = 1f,
						PortraitPositionXOverride = 0f,
						PortraitPositionYOverride = 2f
					}
				},
				{
					572,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					573,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					578,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(0f, 4f)
					}
				},
				{
					574,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(16f, 6f),
						Velocity = 1f
					}
				},
				{
					575,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(16f, 6f),
						Velocity = 1f
					}
				},
				{
					576,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(20f, 70f),
						Velocity = 1f,
						PortraitPositionXOverride = 10f,
						PortraitPositionYOverride = 0f,
						PortraitScale = 0.75f
					}
				},
				{
					577,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(20f, 70f),
						Velocity = 1f,
						PortraitPositionXOverride = 10f,
						PortraitPositionYOverride = 0f,
						PortraitScale = 0.75f
					}
				},
				{
					580,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(0f, 0f),
						Scale = 0.9f,
						Velocity = 1f
					}
				},
				{
					581,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(0f, -8f),
						PortraitPositionYOverride = -30f
					}
				},
				{
					582,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					585,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(0f, 1f),
						Direction = 1
					}
				},
				{
					584,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(0f, 1f),
						Direction = 1
					}
				},
				{
					583,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(0f, 1f),
						Direction = 1
					}
				},
				{
					586,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					579,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					588,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f,
						PortraitPositionXOverride = 1f
					}
				},
				{
					587,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(2f, -14f),
						PortraitPositionYOverride = -30f
					}
				},
				{
					591,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(9f, 0f)
					}
				},
				{
					590,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f,
						PortraitPositionXOverride = 2f
					}
				},
				{
					592,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(0f, 6f),
						PortraitPositionYOverride = 7f,
						IsWet = true
					}
				},
				{
					593,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					594,
					new NPCBestiaryDrawModifiers(0)
					{
						CustomTexturePath = "Images/UI/Bestiary/NPCs/NPC_594",
						Scale = 0.8f
					}
				},
				{
					589,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					602,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					603,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					604,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(2f, 22f),
						PortraitPositionYOverride = 41f
					}
				},
				{
					605,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(2f, 22f),
						PortraitPositionYOverride = 41f
					}
				},
				{
					606,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					607,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(0f, 6f),
						PortraitPositionYOverride = 7f,
						IsWet = true
					}
				},
				{
					608,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					609,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					611,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(0f, 0f),
						Direction = -1,
						SpriteDirection = 1
					}
				},
				{
					612,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					613,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					614,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					615,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(0f, 10f),
						Scale = 0.88f,
						PortraitPositionYOverride = 20f,
						IsWet = true
					}
				},
				{
					616,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					617,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					618,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(12f, -5f),
						Scale = 0.9f,
						PortraitPositionYOverride = 0f
					}
				},
				{
					619,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(1f, 7f),
						Scale = 0.85f,
						PortraitPositionYOverride = 10f
					}
				},
				{
					620,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(6f, 5f),
						Scale = 0.78f,
						Velocity = 1f
					}
				},
				{
					621,
					new NPCBestiaryDrawModifiers(0)
					{
						CustomTexturePath = "Images/UI/Bestiary/NPCs/NPC_621",
						Position = new Vector2(46f, 20f),
						PortraitPositionXOverride = 10f,
						PortraitPositionYOverride = 17f
					}
				},
				{
					622,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					623,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					624,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 2f
					}
				},
				{
					625,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(0f, -12f),
						Velocity = 1f
					}
				},
				{
					626,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(0f, -16f)
					}
				},
				{
					627,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(0f, -16f)
					}
				},
				{
					628,
					new NPCBestiaryDrawModifiers(0)
					{
						Direction = 1
					}
				},
				{
					630,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 0.5f
					}
				},
				{
					632,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					631,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 0.75f
					}
				},
				{
					634,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f,
						Position = new Vector2(0f, -13f),
						PortraitPositionYOverride = -30f
					}
				},
				{
					635,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					636,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(0f, 50f),
						PortraitPositionYOverride = 30f
					}
				},
				{
					639,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					640,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					641,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					642,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					643,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					644,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					645,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					646,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					647,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					648,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					649,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					650,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					651,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					652,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					637,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 0.25f
					}
				},
				{
					638,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					653,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(2f, 3f),
						PortraitPositionYOverride = 1f
					}
				},
				{
					654,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(1f, 2f)
					}
				},
				{
					655,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(1f, 17f),
						PortraitPositionYOverride = 39f
					}
				},
				{
					656,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f
					}
				},
				{
					657,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(0f, 60f),
						PortraitPositionYOverride = 40f
					}
				},
				{
					660,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(-2f, -4f),
						PortraitPositionYOverride = -20f
					}
				},
				{
					661,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(2f, 3f),
						PortraitPositionYOverride = 1f
					}
				},
				{
					662,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					663,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 1f,
						PortraitPositionXOverride = 1f
					}
				},
				{
					664,
					new NPCBestiaryDrawModifiers(0)
				},
				{
					667,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					668,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 2.5f,
						Position = new Vector2(36f, 40f),
						Scale = 0.9f,
						PortraitPositionXOverride = 6f,
						PortraitPositionYOverride = 50f
					}
				},
				{
					669,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(2f, 22f),
						PortraitPositionYOverride = 41f
					}
				},
				{
					670,
					new NPCBestiaryDrawModifiers(0)
					{
						SpriteDirection = 1,
						Velocity = 0.7f
					}
				},
				{
					678,
					new NPCBestiaryDrawModifiers(0)
					{
						SpriteDirection = 1,
						Velocity = 0.7f
					}
				},
				{
					679,
					new NPCBestiaryDrawModifiers(0)
					{
						SpriteDirection = 1,
						Velocity = 0.7f
					}
				},
				{
					680,
					new NPCBestiaryDrawModifiers(0)
					{
						SpriteDirection = 1,
						Velocity = 0.7f
					}
				},
				{
					681,
					new NPCBestiaryDrawModifiers(0)
					{
						SpriteDirection = 1,
						Velocity = 0.7f
					}
				},
				{
					682,
					new NPCBestiaryDrawModifiers(0)
					{
						SpriteDirection = 1,
						Velocity = 0.7f
					}
				},
				{
					683,
					new NPCBestiaryDrawModifiers(0)
					{
						SpriteDirection = 1,
						Velocity = 0.7f
					}
				},
				{
					684,
					new NPCBestiaryDrawModifiers(0)
					{
						SpriteDirection = 1,
						Velocity = 0.7f
					}
				},
				{
					671,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(1f, -18f),
						Velocity = 0.05f,
						PortraitPositionYOverride = -35f
					}
				},
				{
					672,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(1f, -18f),
						Velocity = 0.05f,
						PortraitPositionYOverride = -35f
					}
				},
				{
					673,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(1f, -16f),
						Velocity = 0.05f,
						PortraitPositionYOverride = -35f
					}
				},
				{
					674,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(1f, -16f),
						Velocity = 0.05f,
						PortraitPositionYOverride = -35f
					}
				},
				{
					675,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(1f, -16f),
						Velocity = 0.05f,
						PortraitPositionYOverride = -35f
					}
				},
				{
					677,
					new NPCBestiaryDrawModifiers(0)
					{
						Position = new Vector2(1f, 2f)
					}
				},
				{
					685,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					686,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					687,
					new NPCBestiaryDrawModifiers(0)
					{
						Velocity = 0f
					}
				},
				{
					0,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				},
				{
					488,
					new NPCBestiaryDrawModifiers(0)
					{
						Hide = true
					}
				}
			};
		}

		static Sets()
		{
			//IL_6228: Unknown result type (might be due to invalid IL or missing references)
			//IL_624a: Unknown result type (might be due to invalid IL or missing references)
			//IL_626c: Unknown result type (might be due to invalid IL or missing references)
			//IL_6291: Unknown result type (might be due to invalid IL or missing references)
			//IL_62b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_62df: Unknown result type (might be due to invalid IL or missing references)
			Factory = new SetFactory(NPCLoader.NPCCount);
			SpecialSpawningRules = new Dictionary<int, int>
			{
				{ 259, 0 },
				{ 260, 0 },
				{ 175, 0 },
				{ 43, 0 },
				{ 56, 0 },
				{ 101, 0 }
			};
			NPCBestiaryDrawOffset = NPCBestiaryDrawOffsetCreation();
			DebuffImmunitySets = new Dictionary<int, NPCDebuffImmunityData>
			{
				{ 0, null },
				{
					1,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{ 2, null },
				{ 3, null },
				{
					4,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					5,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					6,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					7,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					8,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					9,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					10,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					11,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					12,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					13,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					14,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					15,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					16,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{
					17,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					18,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					19,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					20,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					21,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{
					22,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					23,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[4] { 20, 24, 31, 323 }
					}
				},
				{
					24,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[3] { 24, 31, 323 }
					}
				},
				{
					25,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{ 26, null },
				{ 27, null },
				{ 28, null },
				{
					29,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					30,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					665,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					31,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{
					32,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 20, 31 }
					}
				},
				{
					33,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					34,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 20, 31 }
					}
				},
				{
					35,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[5] { 20, 31, 169, 337, 344 }
					}
				},
				{
					36,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 20, 31 }
					}
				},
				{
					37,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					38,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					39,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[4] { 20, 24, 31, 323 }
					}
				},
				{
					40,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[4] { 20, 24, 31, 323 }
					}
				},
				{
					41,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[4] { 20, 24, 31, 323 }
					}
				},
				{
					42,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 20, 31 }
					}
				},
				{
					43,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 20, 31 }
					}
				},
				{
					44,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{
					45,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 20, 31 }
					}
				},
				{
					46,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{ 47, null },
				{
					48,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{ 49, null },
				{
					50,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 20, 31 }
					}
				},
				{ 51, null },
				{ 52, null },
				{ 53, null },
				{
					54,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					55,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					56,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					57,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					58,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					59,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[3] { 20, 24, 323 }
					}
				},
				{
					60,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 24, 323 }
					}
				},
				{
					61,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					62,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[4] { 24, 31, 153, 323 }
					}
				},
				{
					63,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					64,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					65,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					66,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[4] { 24, 31, 153, 323 }
					}
				},
				{
					67,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					68,
					new NPCDebuffImmunityData
					{
						ImmuneToAllBuffsThatAreNotWhips = true,
						ImmuneToWhips = true
					}
				},
				{
					69,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					70,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[6] { 20, 24, 31, 39, 70, 323 }
					}
				},
				{
					71,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{
					72,
					new NPCDebuffImmunityData
					{
						ImmuneToAllBuffsThatAreNotWhips = true,
						ImmuneToWhips = true
					}
				},
				{ 73, null },
				{
					74,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{ 75, null },
				{
					76,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					77,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{ 78, null },
				{ 79, null },
				{ 80, null },
				{
					81,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{
					82,
					new NPCDebuffImmunityData
					{
						ImmuneToAllBuffsThatAreNotWhips = true
					}
				},
				{
					83,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[4] { 20, 24, 31, 323 }
					}
				},
				{
					84,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[4] { 20, 24, 31, 323 }
					}
				},
				{
					85,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[4] { 20, 24, 31, 323 }
					}
				},
				{ 86, null },
				{
					87,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					88,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					89,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					90,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					91,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					92,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{ 93, null },
				{
					94,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					95,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					96,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					97,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					98,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					99,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					100,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					101,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 39, 31 }
					}
				},
				{
					102,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					103,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{ 104, null },
				{
					105,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					106,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					107,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					108,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					109,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					110,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{ 111, null },
				{
					112,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					666,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					113,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[3] { 24, 31, 323 }
					}
				},
				{
					114,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[3] { 24, 31, 323 }
					}
				},
				{
					115,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					116,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					117,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					118,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					119,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					120,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{
					121,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{
					122,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 20, 31 }
					}
				},
				{
					123,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					124,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					125,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 20, 31 }
					}
				},
				{
					126,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 20, 31 }
					}
				},
				{
					127,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[5] { 20, 31, 169, 337, 344 }
					}
				},
				{
					128,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 20, 31 }
					}
				},
				{
					129,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 20, 31 }
					}
				},
				{
					130,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 20, 31 }
					}
				},
				{
					131,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 20, 31 }
					}
				},
				{ 132, null },
				{ 133, null },
				{
					134,
					new NPCDebuffImmunityData
					{
						ImmuneToAllBuffsThatAreNotWhips = true
					}
				},
				{
					135,
					new NPCDebuffImmunityData
					{
						ImmuneToAllBuffsThatAreNotWhips = true
					}
				},
				{
					136,
					new NPCDebuffImmunityData
					{
						ImmuneToAllBuffsThatAreNotWhips = true
					}
				},
				{
					137,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{
					138,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{
					139,
					new NPCDebuffImmunityData
					{
						ImmuneToAllBuffsThatAreNotWhips = true
					}
				},
				{
					140,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[3] { 20, 24, 323 }
					}
				},
				{
					141,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 20, 70 }
					}
				},
				{
					142,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					143,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[3] { 20, 44, 324 }
					}
				},
				{
					144,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[3] { 20, 44, 324 }
					}
				},
				{
					145,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[3] { 20, 44, 324 }
					}
				},
				{
					146,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					147,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[3] { 20, 44, 324 }
					}
				},
				{
					148,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					149,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					150,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 44, 324 }
					}
				},
				{
					151,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 24, 323 }
					}
				},
				{ 152, null },
				{ 153, null },
				{
					154,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 44, 324 }
					}
				},
				{ 155, null },
				{
					156,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[4] { 24, 31, 153, 323 }
					}
				},
				{
					157,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{ 158, null },
				{ 159, null },
				{
					160,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					161,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 44, 324 }
					}
				},
				{ 162, null },
				{
					163,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{
					164,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{
					165,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{
					166,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{
					167,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[3] { 20, 44, 324 }
					}
				},
				{ 168, null },
				{
					169,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[4] { 20, 31, 44, 324 }
					}
				},
				{ 170, null },
				{ 171, null },
				{
					172,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 20, 31 }
					}
				},
				{
					173,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					174,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					175,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 20, 31 }
					}
				},
				{
					176,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 20, 31 }
					}
				},
				{
					177,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					178,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					179,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[4] { 20, 24, 31, 323 }
					}
				},
				{ 180, null },
				{
					181,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{
					182,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 20, 31 }
					}
				},
				{
					183,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{
					184,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[3] { 20, 44, 324 }
					}
				},
				{
					185,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 44, 324 }
					}
				},
				{ 186, null },
				{ 187, null },
				{ 188, null },
				{ 189, null },
				{ 190, null },
				{ 191, null },
				{ 192, null },
				{ 193, null },
				{ 194, null },
				{
					195,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{ 196, null },
				{
					197,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[3] { 20, 44, 324 }
					}
				},
				{
					198,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{
					199,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{ 200, null },
				{
					201,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{
					202,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{
					203,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{
					204,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{
					205,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 20, 31 }
					}
				},
				{
					206,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 44, 324 }
					}
				},
				{
					207,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					208,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					209,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					210,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 20, 31 }
					}
				},
				{
					211,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 20, 31 }
					}
				},
				{ 212, null },
				{ 213, null },
				{ 214, null },
				{ 215, null },
				{
					216,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					217,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					218,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					219,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					220,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					221,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					222,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 20, 31 }
					}
				},
				{ 223, null },
				{ 224, null },
				{
					225,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{ 226, null },
				{
					227,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					228,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					229,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					230,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					231,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 20, 31 }
					}
				},
				{
					232,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 20, 31 }
					}
				},
				{
					233,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 20, 31 }
					}
				},
				{
					234,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 20, 31 }
					}
				},
				{
					235,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 20, 31 }
					}
				},
				{
					236,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{
					237,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{
					238,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{
					239,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{
					240,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{
					241,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					242,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					243,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[4] { 20, 31, 44, 324 }
					}
				},
				{
					244,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{
					245,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[4] { 20, 24, 31, 323 }
					}
				},
				{
					246,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[4] { 20, 24, 31, 323 }
					}
				},
				{
					247,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[4] { 20, 24, 31, 323 }
					}
				},
				{
					248,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[4] { 20, 24, 31, 323 }
					}
				},
				{
					249,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[4] { 20, 24, 31, 323 }
					}
				},
				{
					250,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 20, 31 }
					}
				},
				{ 251, null },
				{ 252, null },
				{
					253,
					new NPCDebuffImmunityData
					{
						ImmuneToAllBuffsThatAreNotWhips = true
					}
				},
				{ 254, null },
				{ 255, null },
				{
					256,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{ 257, null },
				{ 258, null },
				{
					259,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 20, 31 }
					}
				},
				{
					260,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 20, 31 }
					}
				},
				{
					261,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					262,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 20, 31 }
					}
				},
				{
					263,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 20, 31 }
					}
				},
				{
					264,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 20, 31 }
					}
				},
				{
					265,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					266,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					267,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					268,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 20, 69 }
					}
				},
				{
					269,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{
					270,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{
					271,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{
					272,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{
					273,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{
					274,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{
					275,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{
					276,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{
					277,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[3] { 20, 24, 323 }
					}
				},
				{
					278,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[3] { 20, 24, 323 }
					}
				},
				{
					279,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[3] { 20, 24, 323 }
					}
				},
				{
					280,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[3] { 20, 24, 323 }
					}
				},
				{
					281,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 20, 31 }
					}
				},
				{
					282,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 20, 31 }
					}
				},
				{
					283,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 20, 31 }
					}
				},
				{
					284,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 20, 31 }
					}
				},
				{
					285,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 20, 31 }
					}
				},
				{
					286,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 20, 31 }
					}
				},
				{
					287,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{
					288,
					new NPCDebuffImmunityData
					{
						ImmuneToAllBuffsThatAreNotWhips = true
					}
				},
				{
					289,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 20, 31 }
					}
				},
				{
					290,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[3] { 20, 31, 69 }
					}
				},
				{
					291,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{
					292,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{
					293,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{
					294,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{
					295,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{
					296,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{
					297,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					298,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					671,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					672,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					673,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					674,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					675,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					299,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					300,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					301,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					302,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{
					303,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					304,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{ 305, null },
				{ 306, null },
				{ 307, null },
				{ 308, null },
				{ 309, null },
				{ 310, null },
				{ 311, null },
				{ 312, null },
				{ 313, null },
				{ 314, null },
				{
					315,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[4] { 20, 24, 31, 323 }
					}
				},
				{
					316,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[13]
						{
							20, 24, 31, 39, 44, 69, 70, 153, 189, 203,
							204, 323, 324
						}
					}
				},
				{ 317, null },
				{ 318, null },
				{ 319, null },
				{ 320, null },
				{ 321, null },
				{
					322,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{
					323,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{
					324,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{
					325,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 20, 31 }
					}
				},
				{ 326, null },
				{
					327,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[4] { 20, 24, 31, 323 }
					}
				},
				{
					328,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[4] { 20, 24, 31, 323 }
					}
				},
				{
					329,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[3] { 20, 24, 323 }
					}
				},
				{
					330,
					new NPCDebuffImmunityData
					{
						ImmuneToAllBuffsThatAreNotWhips = true
					}
				},
				{ 331, null },
				{ 332, null },
				{
					333,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{
					334,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{
					335,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{
					336,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{
					337,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					338,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 44, 324 }
					}
				},
				{
					339,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 44, 324 }
					}
				},
				{
					340,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 44, 324 }
					}
				},
				{
					341,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[4] { 20, 24, 31, 323 }
					}
				},
				{ 342, null },
				{
					343,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 44, 324 }
					}
				},
				{
					344,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[4] { 20, 31, 44, 324 }
					}
				},
				{
					345,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[4] { 20, 31, 44, 324 }
					}
				},
				{
					346,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[4] { 20, 31, 44, 324 }
					}
				},
				{
					347,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{ 348, null },
				{ 349, null },
				{ 350, null },
				{
					351,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 44, 324 }
					}
				},
				{
					352,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[4] { 20, 31, 44, 324 }
					}
				},
				{
					353,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					354,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					355,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					356,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					357,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					358,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					359,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					360,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					361,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					362,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					363,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					364,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					365,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					366,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					367,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					368,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					369,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					370,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					371,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					372,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					373,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					374,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					375,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					376,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					377,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					378,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					379,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					380,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					381,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{ 382, null },
				{ 383, null },
				{
					384,
					new NPCDebuffImmunityData
					{
						ImmuneToAllBuffsThatAreNotWhips = true
					}
				},
				{ 385, null },
				{ 386, null },
				{
					387,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 20, 31 }
					}
				},
				{
					388,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 20, 31 }
					}
				},
				{ 389, null },
				{ 390, null },
				{ 391, null },
				{
					392,
					new NPCDebuffImmunityData
					{
						ImmuneToAllBuffsThatAreNotWhips = true,
						ImmuneToWhips = true
					}
				},
				{
					393,
					new NPCDebuffImmunityData
					{
						ImmuneToAllBuffsThatAreNotWhips = true
					}
				},
				{
					394,
					new NPCDebuffImmunityData
					{
						ImmuneToAllBuffsThatAreNotWhips = true
					}
				},
				{
					395,
					new NPCDebuffImmunityData
					{
						ImmuneToAllBuffsThatAreNotWhips = true
					}
				},
				{
					396,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					397,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					398,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					399,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 20, 31 }
					}
				},
				{
					400,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					401,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					402,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					403,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					404,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					405,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					406,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					407,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					408,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{ 409, null },
				{ 410, null },
				{ 411, null },
				{
					412,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					413,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					414,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{ 415, null },
				{ 416, null },
				{ 417, null },
				{
					418,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[4] { 20, 24, 31, 323 }
					}
				},
				{ 419, null },
				{
					420,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					421,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					422,
					new NPCDebuffImmunityData
					{
						ImmuneToAllBuffsThatAreNotWhips = true
					}
				},
				{ 423, null },
				{ 424, null },
				{ 425, null },
				{ 426, null },
				{ 427, null },
				{
					428,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{ 429, null },
				{ 430, null },
				{
					431,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 44, 324 }
					}
				},
				{ 432, null },
				{ 433, null },
				{ 434, null },
				{ 435, null },
				{ 436, null },
				{
					437,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					438,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					439,
					new NPCDebuffImmunityData
					{
						ImmuneToAllBuffsThatAreNotWhips = true
					}
				},
				{
					440,
					new NPCDebuffImmunityData
					{
						ImmuneToAllBuffsThatAreNotWhips = true,
						ImmuneToWhips = true
					}
				},
				{
					441,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					442,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					443,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					444,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					445,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					446,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					447,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					448,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					449,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{
					450,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{
					451,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{
					452,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{
					453,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					454,
					new NPCDebuffImmunityData
					{
						ImmuneToAllBuffsThatAreNotWhips = true
					}
				},
				{
					455,
					new NPCDebuffImmunityData
					{
						ImmuneToAllBuffsThatAreNotWhips = true
					}
				},
				{
					456,
					new NPCDebuffImmunityData
					{
						ImmuneToAllBuffsThatAreNotWhips = true
					}
				},
				{
					457,
					new NPCDebuffImmunityData
					{
						ImmuneToAllBuffsThatAreNotWhips = true
					}
				},
				{
					458,
					new NPCDebuffImmunityData
					{
						ImmuneToAllBuffsThatAreNotWhips = true
					}
				},
				{
					459,
					new NPCDebuffImmunityData
					{
						ImmuneToAllBuffsThatAreNotWhips = true
					}
				},
				{ 460, null },
				{
					461,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{ 462, null },
				{ 463, null },
				{ 464, null },
				{
					465,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{ 466, null },
				{
					467,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[4] { 20, 24, 31, 323 }
					}
				},
				{ 468, null },
				{ 469, null },
				{ 470, null },
				{
					471,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 31, 153 }
					}
				},
				{
					472,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 31, 153 }
					}
				},
				{
					473,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[4] { 20, 24, 31, 323 }
					}
				},
				{
					474,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[4] { 20, 24, 31, 323 }
					}
				},
				{
					475,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[4] { 20, 24, 31, 323 }
					}
				},
				{
					476,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[4] { 20, 24, 31, 323 }
					}
				},
				{
					477,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					478,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					479,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					480,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					481,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{
					482,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[4] { 20, 24, 31, 323 }
					}
				},
				{
					483,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[4] { 20, 24, 31, 323 }
					}
				},
				{
					484,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					485,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					486,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					487,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					488,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{ 489, null },
				{ 490, null },
				{
					491,
					new NPCDebuffImmunityData
					{
						ImmuneToAllBuffsThatAreNotWhips = true,
						ImmuneToWhips = true
					}
				},
				{
					492,
					new NPCDebuffImmunityData
					{
						ImmuneToAllBuffsThatAreNotWhips = true
					}
				},
				{
					493,
					new NPCDebuffImmunityData
					{
						ImmuneToAllBuffsThatAreNotWhips = true
					}
				},
				{
					494,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					495,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					496,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					497,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{ 498, null },
				{ 499, null },
				{ 500, null },
				{ 501, null },
				{ 502, null },
				{ 503, null },
				{ 504, null },
				{ 505, null },
				{ 506, null },
				{
					507,
					new NPCDebuffImmunityData
					{
						ImmuneToAllBuffsThatAreNotWhips = true
					}
				},
				{ 508, null },
				{ 509, null },
				{
					510,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					511,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					512,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					513,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					514,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					515,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					516,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					517,
					new NPCDebuffImmunityData
					{
						ImmuneToAllBuffsThatAreNotWhips = true
					}
				},
				{
					518,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					519,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					520,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[4] { 20, 24, 31, 323 }
					}
				},
				{
					521,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[13]
						{
							20, 24, 31, 39, 44, 69, 70, 153, 189, 203,
							204, 323, 324
						}
					}
				},
				{
					522,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					523,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{ 524, null },
				{ 525, null },
				{ 526, null },
				{ 527, null },
				{ 528, null },
				{ 529, null },
				{
					530,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{
					531,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{ 532, null },
				{
					533,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[13]
						{
							20, 24, 31, 39, 44, 69, 70, 153, 189, 203,
							204, 323, 324
						}
					}
				},
				{ 534, null },
				{
					535,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{ 536, null },
				{
					537,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{
					538,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					539,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					540,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					541,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 20, 31 }
					}
				},
				{
					542,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					543,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					544,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					545,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					546,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					547,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					548,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					549,
					new NPCDebuffImmunityData
					{
						ImmuneToAllBuffsThatAreNotWhips = true,
						ImmuneToWhips = true
					}
				},
				{
					550,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					551,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[3] { 24, 31, 323 }
					}
				},
				{
					552,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					553,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					554,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					555,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					556,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					557,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					558,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					559,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					560,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					561,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					562,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					563,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					564,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					565,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					566,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 20, 31 }
					}
				},
				{
					567,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 20, 31 }
					}
				},
				{
					568,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					569,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					570,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					571,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					572,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					573,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					574,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					575,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					576,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					577,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					578,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					579,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{ 580, null },
				{ 581, null },
				{ 582, null },
				{
					583,
					new NPCDebuffImmunityData
					{
						ImmuneToAllBuffsThatAreNotWhips = true,
						ImmuneToWhips = true
					}
				},
				{
					584,
					new NPCDebuffImmunityData
					{
						ImmuneToAllBuffsThatAreNotWhips = true,
						ImmuneToWhips = true
					}
				},
				{
					585,
					new NPCDebuffImmunityData
					{
						ImmuneToAllBuffsThatAreNotWhips = true,
						ImmuneToWhips = true
					}
				},
				{ 586, null },
				{ 587, null },
				{
					588,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					589,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{ 590, null },
				{ 591, null },
				{
					592,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					593,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					594,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					595,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					596,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					597,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					598,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					599,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					600,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					601,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					602,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					603,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					604,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					605,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					606,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					607,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					608,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					609,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					610,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					611,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					612,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					613,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					614,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					615,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					616,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					617,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					618,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					619,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					620,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					621,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					622,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					623,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					624,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					625,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					626,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					627,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					628,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					629,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[6] { 20, 24, 31, 44, 323, 324 }
					}
				},
				{ 630, null },
				{
					631,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[4] { 20, 24, 31, 323 }
					}
				},
				{ 632, null },
				{
					633,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{ 634, null },
				{
					635,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 20 }
					}
				},
				{
					636,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					637,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					638,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					639,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					640,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					641,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					642,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					643,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					644,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					645,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					646,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					647,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					648,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					649,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					650,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					651,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					652,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					653,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					654,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					655,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					656,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					657,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 20, 31 }
					}
				},
				{
					658,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 20, 31 }
					}
				},
				{
					659,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 20, 31 }
					}
				},
				{
					660,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[2] { 20, 31 }
					}
				},
				{
					661,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					662,
					new NPCDebuffImmunityData
					{
						ImmuneToAllBuffsThatAreNotWhips = true
					}
				},
				{
					663,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					668,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					669,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					670,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					678,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					679,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					680,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					681,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					682,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					683,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					684,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					677,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					685,
					new NPCDebuffImmunityData
					{
						SpecificallyImmuneTo = new int[1] { 31 }
					}
				},
				{
					686,
					new NPCDebuffImmunityData
					{
						ImmuneToAllBuffsThatAreNotWhips = true,
						ImmuneToWhips = true
					}
				},
				{
					687,
					new NPCDebuffImmunityData
					{
						ImmuneToAllBuffsThatAreNotWhips = true,
						ImmuneToWhips = true
					}
				}
			};
			NormalGoldCritterBestiaryPriority = new List<int>
			{
				46, 540, 614, 303, 337, 443, 74, 297, 298, 671,
				672, 673, 674, 675, 442, 55, 230, 592, 593, 299,
				538, 539, 300, 447, 361, 445, 377, 446, 356, 444,
				357, 448, 595, 596, 597, 598, 599, 600, 601, 626,
				627, 612, 613, 604, 605, 669, 677
			};
			BossBestiaryPriority = new List<int>
			{
				664, 4, 5, 50, 535, 13, 14, 15, 266, 267,
				668, 35, 36, 222, 113, 114, 117, 115, 116, 657,
				658, 659, 660, 125, 126, 134, 135, 136, 139, 127,
				128, 131, 129, 130, 262, 263, 264, 636, 245, 246,
				249, 247, 248, 370, 372, 373, 439, 438, 379, 380,
				440, 521, 454, 507, 517, 422, 493, 398, 396, 397,
				400, 401
			};
			TownNPCBestiaryPriority = new List<int>
			{
				22, 17, 18, 38, 369, 20, 19, 207, 227, 353,
				633, 550, 588, 107, 228, 124, 54, 108, 178, 229,
				160, 441, 209, 208, 663, 142, 637, 638, 656, 670,
				678, 679, 680, 681, 682, 683, 684, 368, 453, 37,
				687
			};
			DontDoHardmodeScaling = Factory.CreateBoolSet(5, 13, 14, 15, 267, 113, 114, 115, 116, 117, 118, 119, 658, 659, 660, 400, 522);
			ReflectStarShotsInForTheWorthy = Factory.CreateBoolSet(4, 5, 13, 14, 15, 266, 267, 35, 36, 113, 114, 115, 116, 117, 118, 119, 125, 126, 134, 135, 136, 139, 127, 128, 131, 129, 130, 262, 263, 264, 245, 247, 248, 246, 249, 398, 400, 397, 396, 401);
			IsTownPet = Factory.CreateBoolSet(637, 638, 656, 670, 678, 679, 680, 681, 682, 683, 684);
			IsTownSlime = Factory.CreateBoolSet(670, 678, 679, 680, 681, 682, 683, 684);
			CanConvertIntoCopperSlimeTownNPC = Factory.CreateBoolSet(1, 302, 335, 336, 333, 334);
			GoldCrittersCollection = new List<int>
			{
				443, 442, 592, 593, 444, 601, 445, 446, 605, 447,
				627, 613, 448, 539
			};
			ZappingJellyfish = Factory.CreateBoolSet(63, 64, 103, 242);
			CantTakeLunchMoney = Factory.CreateBoolSet(394, 393, 392, 492, 491, 662, 384, 478, 535, 658, 659, 660, 128, 131, 129, 130, 139, 267, 247, 248, 246, 249, 245, 409, 410, 397, 396, 401, 400, 440, 68, 534);
			RespawnEnemyID = new Dictionary<int, int>
			{
				{ 492, 0 },
				{ 491, 0 },
				{ 394, 0 },
				{ 393, 0 },
				{ 392, 0 },
				{ 13, 0 },
				{ 14, 0 },
				{ 15, 0 },
				{ 412, 0 },
				{ 413, 0 },
				{ 414, 0 },
				{ 134, 0 },
				{ 135, 0 },
				{ 136, 0 },
				{ 454, 0 },
				{ 455, 0 },
				{ 456, 0 },
				{ 457, 0 },
				{ 458, 0 },
				{ 459, 0 },
				{ 8, 7 },
				{ 9, 7 },
				{ 11, 10 },
				{ 12, 10 },
				{ 40, 39 },
				{ 41, 39 },
				{ 96, 95 },
				{ 97, 95 },
				{ 99, 98 },
				{ 100, 98 },
				{ 88, 87 },
				{ 89, 87 },
				{ 90, 87 },
				{ 91, 87 },
				{ 92, 87 },
				{ 118, 117 },
				{ 119, 117 },
				{ 514, 513 },
				{ 515, 513 },
				{ 511, 510 },
				{ 512, 510 },
				{ 622, 621 },
				{ 623, 621 }
			};
			TrailingMode = Factory.CreateIntSet(-1, 439, 0, 440, 0, 370, 1, 372, 1, 373, 1, 396, 1, 400, 1, 401, 1, 473, 2, 474, 2, 475, 2, 476, 2, 4, 3, 471, 3, 477, 3, 479, 3, 120, 4, 137, 4, 138, 4, 94, 5, 125, 6, 126, 6, 127, 6, 128, 6, 129, 6, 130, 6, 131, 6, 139, 6, 140, 6, 407, 6, 420, 6, 425, 6, 427, 6, 426, 6, 581, 6, 516, 6, 542, 6, 543, 6, 544, 6, 545, 6, 402, 7, 417, 7, 419, 7, 418, 7, 574, 7, 575, 7, 519, 7, 521, 7, 522, 7, 546, 7, 558, 7, 559, 7, 560, 7, 551, 7, 620, 7, 657, 6, 636, 7, 677, 7, 685, 7);
			IsDragonfly = Factory.CreateBoolSet(595, 596, 597, 598, 599, 600, 601);
			BelongsToInvasionOldOnesArmy = Factory.CreateBoolSet(552, 553, 554, 561, 562, 563, 555, 556, 557, 558, 559, 560, 576, 577, 568, 569, 566, 567, 570, 571, 572, 573, 548, 549, 564, 565, 574, 575, 551, 578);
			TeleportationImmune = Factory.CreateBoolSet(552, 553, 554, 561, 562, 563, 555, 556, 557, 558, 559, 560, 576, 577, 568, 569, 566, 567, 570, 571, 572, 573, 548, 549, 564, 565, 574, 575, 551, 578);
			UsesNewTargetting = Factory.CreateBoolSet(547, 552, 553, 554, 561, 562, 563, 555, 556, 557, 558, 559, 560, 576, 577, 568, 569, 566, 567, 570, 571, 572, 573, 564, 565, 574, 575, 551, 578, 210, 211, 620, 668);
			TakesDamageFromHostilesWithoutBeingFriendly = Factory.CreateBoolSet(46, 55, 74, 148, 149, 230, 297, 298, 299, 303, 355, 356, 358, 359, 360, 361, 362, 363, 364, 365, 366, 367, 377, 357, 374, 442, 443, 444, 445, 446, 448, 538, 539, 337, 540, 484, 485, 486, 487, 592, 593, 595, 596, 597, 598, 599, 600, 601, 602, 603, 604, 605, 606, 607, 608, 609, 611, 612, 613, 614, 615, 616, 617, 625, 626, 627, 639, 640, 641, 642, 643, 644, 645, 646, 647, 648, 649, 650, 651, 652, 653, 654, 655, 583, 584, 585, 669, 671, 672, 673, 674, 675, 677, 687);
			AllNPCs = Factory.CreateBoolSet(true);
			HurtingBees = Factory.CreateBoolSet(210, 211, 222);
			FighterUsesDD2PortalAppearEffect = Factory.CreateBoolSet(552, 553, 554, 561, 562, 563, 555, 556, 557, 576, 577, 568, 569, 570, 571, 572, 573, 564, 565);
			StatueSpawnedDropRarity = Factory.CreateCustomSet(-1f, (short)480, 0.05f, (short)82, 0.05f, (short)86, 0.05f, (short)48, 0.05f, (short)490, 0.05f, (short)489, 0.05f, (short)170, 0.05f, (short)180, 0.05f, (short)171, 0.05f, (short)167, 0.25f, (short)73, 0.01f, (short)24, 0.05f, (short)481, 0.05f, (short)42, 0.05f, (short)6, 0.05f, (short)2, 0.05f, (short)49, 0.2f, (short)3, 0.2f, (short)58, 0.2f, (short)21, 0.2f, (short)65, 0.2f, (short)449, 0.2f, (short)482, 0.2f, (short)103, 0.2f, (short)64, 0.2f, (short)63, 0.2f, (short)85, 0f);
			NoEarlymodeLootWhenSpawnedFromStatue = Factory.CreateBoolSet(480, 82, 86, 170, 180, 171);
			NeedsExpertScaling = Factory.CreateBoolSet(25, 30, 665, 33, 112, 666, 261, 265, 371, 516, 519, 397, 396, 398, 491);
			ProjectileNPC = Factory.CreateBoolSet(25, 30, 665, 33, 112, 666, 261, 265, 371, 516, 519);
			SavesAndLoads = Factory.CreateBoolSet(422, 507, 517, 493);
			TrailCacheLength = Factory.CreateIntSet(10, 402, 36, 519, 20, 522, 20, 620, 20, 677, 60, 685, 10);
			UsesMultiplayerProximitySyncing = Factory.CreateBoolSet(true, 398, 397, 396);
			NoMultiplayerSmoothingByType = Factory.CreateBoolSet(113, 114, 50, 657, 120, 245, 247, 248, 246, 370, 222, 398, 397, 396, 400, 401, 668, 70);
			NoMultiplayerSmoothingByAI = Factory.CreateBoolSet(6, 8, 37);
			MPAllowedEnemies = Factory.CreateBoolSet(4, 13, 50, 126, 125, 134, 127, 128, 131, 129, 130, 222, 245, 266, 370, 657, 668);
			TownCritter = Factory.CreateBoolSet(46, 148, 149, 230, 299, 300, 303, 337, 361, 362, 364, 366, 367, 443, 445, 447, 538, 539, 540, 583, 584, 585, 592, 593, 602, 607, 608, 610, 616, 617, 625, 626, 627, 639, 640, 641, 642, 643, 644, 645, 646, 647, 648, 649, 650, 651, 652, 687);
			CountsAsCritter = Factory.CreateBoolSet(46, 303, 337, 540, 443, 74, 297, 298, 442, 611, 377, 446, 612, 613, 356, 444, 595, 596, 597, 598, 599, 600, 601, 604, 605, 357, 448, 374, 484, 355, 358, 606, 359, 360, 485, 486, 487, 148, 149, 55, 230, 592, 593, 299, 538, 539, 300, 447, 361, 445, 362, 363, 364, 365, 367, 366, 583, 584, 585, 602, 603, 607, 608, 609, 610, 616, 617, 625, 626, 627, 615, 639, 640, 641, 642, 643, 644, 645, 646, 647, 648, 649, 650, 651, 652, 653, 654, 655, 661, 669, 671, 672, 673, 674, 675, 677, 687);
			HasNoPartyText = Factory.CreateBoolSet(441, 453);
			HatOffsetY = Factory.CreateIntSet(0, 227, 4, 107, 2, 108, 2, 229, 4, 17, 2, 38, 8, 160, -10, 208, 2, 142, 2, 124, 2, 453, 2, 37, 4, 54, 4, 209, 4, 369, 6, 441, 6, 353, -2, 633, -2, 550, -2, 588, 2, 663, 2, 637, 0, 638, 0, 656, 4, 670, 0, 678, 0, 679, 0, 680, 0, 681, 0, 682, 0, 683, 0, 684, 0);
			FaceEmote = Factory.CreateIntSet(0, 17, 101, 18, 102, 19, 103, 20, 104, 22, 105, 37, 106, 38, 107, 54, 108, 107, 109, 108, 110, 124, 111, 142, 112, 160, 113, 178, 114, 207, 115, 208, 116, 209, 117, 227, 118, 228, 119, 229, 120, 353, 121, 368, 122, 369, 123, 453, 124, 441, 125, 588, 140, 633, 141, 663, 145);
			ExtraFramesCount = Factory.CreateIntSet(0, 17, 9, 18, 9, 19, 9, 20, 7, 22, 10, 37, 5, 38, 9, 54, 7, 107, 9, 108, 7, 124, 9, 142, 9, 160, 7, 178, 9, 207, 9, 208, 9, 209, 10, 227, 9, 228, 10, 229, 10, 353, 9, 633, 9, 368, 10, 369, 9, 453, 9, 441, 9, 550, 9, 588, 9, 663, 7, 637, 18, 638, 11, 656, 20, 670, 6, 678, 6, 679, 6, 680, 6, 681, 6, 682, 6, 683, 6, 684, 6);
			AttackFrameCount = Factory.CreateIntSet(0, 17, 4, 18, 4, 19, 4, 20, 2, 22, 5, 37, 0, 38, 4, 54, 2, 107, 4, 108, 2, 124, 4, 142, 4, 160, 2, 178, 4, 207, 4, 208, 4, 209, 5, 227, 4, 228, 5, 229, 5, 353, 4, 633, 4, 368, 5, 369, 4, 453, 4, 441, 4, 550, 4, 588, 4, 663, 2, 637, 0, 638, 0, 656, 0, 670, 0, 678, 0, 679, 0, 680, 0, 681, 0, 682, 0, 683, 0, 684, 0);
			DangerDetectRange = Factory.CreateIntSet(-1, 38, 300, 17, 320, 107, 300, 19, 900, 22, 700, 124, 800, 228, 800, 178, 900, 18, 300, 229, 1000, 209, 1000, 54, 700, 108, 700, 160, 700, 20, 1200, 369, 300, 453, 300, 368, 900, 207, 60, 227, 800, 208, 400, 142, 500, 441, 50, 353, 60, 633, 100, 550, 120, 588, 120, 663, 700, 638, 250, 637, 250, 656, 250, 670, 250, 678, 250, 679, 250, 680, 250, 681, 250, 682, 250, 683, 250, 684, 250);
			ShimmerImmunity = Factory.CreateBoolSet(637, 638, 656, 670, 684, 678, 679, 680, 681, 682, 683, 356, 669, 676, 244, 677, 594, 667, 662, 5, 115, 116, 139, 245, 247, 248, 246, 249, 344, 325, 50, 535, 657, 658, 659, 660, 668, 25, 30, 33, 70, 72, 665, 666, 112, 516, 517, 518, 519, 520, 521, 522, 523, 381, 382, 383, 384, 385, 386, 387, 388, 389, 390, 391, 392, 393, 394, 395, 396, 397, 398, 399, 400, 401, 402, 403, 404, 405, 406, 407, 408, 409, 410, 411, 412, 413, 414, 415, 416, 417, 418, 419, 420, 421, 423, 424, 425, 426, 427, 428, 429, 548, 549, 551, 552, 553, 554, 555, 556, 557, 558, 559, 560, 561, 562, 563, 564, 565, 566, 567, 568, 569, 570, 571, 572, 573, 574, 575, 576, 577, 578);
			ShimmerTransformToItem = Factory.CreateIntSet(-1, 651, 182, 644, 182, 650, 178, 643, 178, 649, 179, 642, 179, 648, 177, 641, 177, 640, 180, 647, 180, 646, 181, 639, 181, 652, 999, 645, 999, 448, 5341);
			ShimmerTownTransform = Factory.CreateBoolSet(22, 17, 18, 227, 207, 633, 588, 208, 369, 353, 38, 20, 550, 19, 107, 228, 54, 124, 441, 229, 160, 108, 178, 209, 142, 663, 37, 453, 368);
			ShimmerTransformToNPC = Factory.CreateIntSet(-1, 3, 21, 132, 202, 186, 201, 187, 21, 188, 21, 189, 202, 200, 203, 590, 21, 1, 676, 302, 676, 335, 676, 336, 676, 334, 676, 333, 676, 225, 676, 141, 676, 16, 676, 147, 676, 184, 676, 537, 676, 204, 676, 81, 676, 183, 676, 138, 676, 121, 676, 591, 449, 430, 449, 436, 452, 432, 450, 433, 449, 434, 449, 435, 451, 614, 677, 74, 677, 297, 677, 298, 677, 673, 677, 672, 677, 671, 677, 675, 677, 674, 677, 362, 677, 363, 677, 364, 677, 365, 677, 608, 677, 609, 677, 602, 677, 603, 677, 611, 677, 148, 677, 149, 677, 46, 677, 303, 677, 337, 677, 299, 677, 538, 677, 55, 677, 607, 677, 615, 677, 625, 677, 626, 677, 361, 677, 687, 677, 484, 677, 604, 677, 358, 677, 355, 677, 616, 677, 617, 677, 654, 677, 653, 677, 655, 677, 585, 677, 584, 677, 583, 677, 595, 677, 596, 677, 600, 677, 597, 677, 598, 677, 599, 677, 357, 677, 377, 677, 606, 677, 359, 677, 360, 677, 367, 677, 366, 677, 300, 677, 610, 677, 612, 677, 487, 677, 486, 677, 485, 677, 669, 677, 356, 677, 661, 677, 374, 677, 442, 677, 443, 677, 444, 677, 601, 677, 445, 677, 592, 677, 446, 677, 605, 677, 447, 677, 627, 677, 539, 677, 613, 677);
			AttackTime = Factory.CreateIntSet(-1, 38, 34, 17, 34, 107, 60, 19, 40, 22, 30, 124, 34, 228, 40, 178, 24, 18, 34, 229, 60, 209, 60, 54, 60, 108, 30, 160, 60, 20, 600, 369, 34, 453, 34, 368, 60, 207, 15, 227, 60, 208, 34, 142, 34, 441, 15, 353, 12, 633, 12, 550, 34, 588, 20, 663, 60, 638, -1, 637, -1, 656, -1, 670, -1, 678, -1, 679, -1, 680, -1, 681, -1, 682, -1, 683, -1, 684, -1);
			AttackAverageChance = Factory.CreateIntSet(1, 38, 40, 17, 30, 107, 60, 19, 30, 22, 30, 124, 30, 228, 50, 178, 50, 18, 60, 229, 40, 209, 30, 54, 30, 108, 30, 160, 60, 20, 60, 369, 50, 453, 30, 368, 40, 207, 1, 227, 30, 208, 50, 142, 50, 441, 1, 353, 1, 633, 1, 550, 40, 588, 20, 663, 1, 638, 1, 637, 1, 656, 1, 670, 1, 678, 1, 679, 1, 680, 1, 681, 1, 682, 1, 683, 1, 684, 1);
			AttackType = Factory.CreateIntSet(-1, 38, 0, 17, 0, 107, 0, 19, 1, 22, 1, 124, 0, 228, 1, 178, 1, 18, 0, 229, 1, 209, 1, 54, 2, 108, 2, 160, 2, 20, 2, 369, 0, 453, 0, 368, 1, 207, 3, 227, 1, 208, 0, 142, 0, 441, 3, 353, 3, 633, 0, 550, 0, 588, 0, 663, 2, 638, -1, 637, -1, 656, -1, 670, -1, 678, -1, 679, -1, 680, -1, 681, -1, 682, -1, 683, -1, 684, -1);
			PrettySafe = Factory.CreateIntSet(-1, 19, 300, 22, 200, 124, 200, 228, 300, 178, 300, 229, 300, 209, 300, 54, 100, 108, 100, 160, 100, 20, 200, 368, 200, 227, 200);
			MagicAuraColor = Factory.CreateCustomSet<Color>(Color.White, new object[10]
			{
				(short)54,
				(object)new Color(100, 4, 227, 127),
				(short)108,
				(object)new Color(255, 80, 60, 127),
				(short)160,
				(object)new Color(40, 80, 255, 127),
				(short)20,
				(object)new Color(40, 255, 80, 127),
				(short)663,
				Main.hslToRgb(0.92f, 1f, 0.78f, 127)
			});
			DemonEyes = Factory.CreateBoolSet(2, 190, 192, 193, 191, 194, 317, 318);
			Zombies = Factory.CreateBoolSet(3, 132, 186, 187, 188, 189, 200, 223, 161, 254, 255, 52, 53, 536, 319, 320, 321, 332, 436, 431, 432, 433, 434, 435, 331, 430, 590);
			Skeletons = Factory.CreateBoolSet(77, 449, 450, 451, 452, 481, 201, 202, 203, 21, 324, 110, 323, 293, 291, 322, 292, 197, 167, 44, 635);
			BossHeadTextures = Factory.CreateIntSet(-1, 4, 0, 13, 2, 344, 3, 370, 4, 246, 5, 249, 5, 345, 6, 50, 7, 396, 8, 395, 9, 325, 10, 262, 11, 327, 13, 222, 14, 125, 15, 126, 20, 346, 17, 127, 18, 35, 19, 68, 19, 113, 22, 266, 23, 439, 24, 440, 24, 134, 25, 491, 26, 517, 27, 422, 28, 507, 29, 493, 30, 549, 35, 564, 32, 565, 32, 576, 33, 577, 33, 551, 34, 548, 36, 636, 37, 657, 38, 668, 39);
			PositiveNPCTypesExcludedFromDeathTally = Factory.CreateBoolSet(121, 384, 478, 479, 410, 472, 378);
			ShouldBeCountedAsBoss = Factory.CreateBoolSet(false, 517, 422, 507, 493, 13, 664);
			DangerThatPreventsOtherDangers = Factory.CreateBoolSet(517, 422, 507, 493, 399);
			MustAlwaysDraw = Factory.CreateBoolSet(113, 114, 115, 116, 126, 125);
			ExtraTextureCount = Factory.CreateIntSet(0, 38, 1, 17, 1, 107, 0, 19, 0, 22, 0, 124, 1, 228, 0, 178, 1, 18, 1, 229, 1, 209, 1, 54, 1, 108, 1, 160, 0, 20, 0, 369, 1, 453, 1, 368, 1, 207, 1, 227, 1, 208, 0, 142, 1, 441, 1, 353, 1, 633, 1, 550, 0, 588, 1, 633, 2, 663, 1, 638, 0, 637, 0, 656, 0, 670, 0, 678, 0, 679, 0, 680, 0, 681, 0, 682, 0, 683, 0, 684, 0);
			NPCFramingGroup = Factory.CreateIntSet(0, 18, 1, 20, 1, 208, 1, 178, 1, 124, 1, 353, 1, 633, 1, 369, 2, 160, 3, 637, 4, 638, 5, 656, 6, 670, 7, 678, 7, 679, 7, 680, 7, 681, 7, 682, 7, 683, 7, 684, 7);
			CanHitPastShimmer = Factory.CreateBoolSet(535, 5, 13, 14, 15, 666, 267, 36, 210, 211, 115, 116, 117, 118, 119, 658, 659, 660, 134, 135, 136, 139, 128, 131, 129, 130, 263, 264, 246, 249, 247, 248, 371, 372, 373, 566, 567, 440, 522, 523, 521, 454, 455, 456, 457, 458, 459, 397, 396, 400);
			TownNPCsFramingGroups = new int[8][]
			{
				new int[26]
				{
					0, 0, 0, -2, -2, -2, 0, 0, 0, 0,
					-2, -2, -2, 0, 0, 0, 0, 0, 0, 0,
					0, 0, 0, 0, 0, 0
				},
				new int[25]
				{
					0, 0, 0, -2, -2, -2, 0, 0, 0, -2,
					-2, -2, 0, 0, 0, 0, 0, 0, 0, 0,
					0, 0, 0, 0, 0
				},
				new int[25]
				{
					0, 0, 0, -2, -2, -2, 0, 0, -2, -2,
					-2, -2, 0, 0, 0, 0, 0, 0, 0, 0,
					0, 0, 0, 0, 0
				},
				new int[22]
				{
					0, 0, -2, 0, 0, 0, 0, -2, -2, -2,
					0, 0, 0, 0, -2, -2, 0, 0, 0, 0,
					0, 0
				},
				new int[28]
				{
					0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
					2, 2, 4, 6, 4, 2, 2, -2, -4, -6,
					-4, -2, -4, -4, -6, -6, -6, -4
				},
				new int[28]
				{
					0, 0, 0, 0, 0, 0, 0, 0, -2, -2,
					-2, 0, 0, -2, -2, 0, 0, 4, 6, 6,
					6, 6, 4, 4, 4, 4, 4, 4
				},
				new int[26]
				{
					0, 0, -2, -4, -4, -2, 0, -2, 0, 0,
					2, 4, 6, 4, 2, 0, -2, -4, -6, -6,
					-6, -6, -6, -6, -4, -2
				},
				new int[14]
				{
					0, -2, 0, -2, -4, -6, -4, -2, 0, 0,
					2, 2, 4, 2
				}
			};
			SpawnFromLastEmptySlot = Factory.CreateBoolSet(222, 245);
			ActsLikeTownNPC = Factory.CreateBoolSet(453);
			NoTownNPCHappiness = Factory.CreateBoolSet(37, 368, 453);
			SpawnsWithCustomName = Factory.CreateBoolSet(453);
			CannotSitOnFurniture = Factory.CreateBoolSet(638, 656);
			CannotDropSouls = Factory.CreateBoolSet(1, 13, 14, 15, 121, 535);
			AllowDoorInteraction = Factory.CreateBoolSet();
			BelongsToInvasionGoblinArmy = Factory.CreateBoolSet(26, 27, 28, 29, 111, 471, 472);
			BelongsToInvasionFrostLegion = Factory.CreateBoolSet(143, 144, 145);
			BelongsToInvasionPirate = Factory.CreateBoolSet(212, 213, 214, 215, 216, 491);
			BelongsToInvasionMartianMadness = Factory.CreateBoolSet(381, 382, 383, 385, 386, 387, 388, 389, 390, 391, 395, 520);
			NoInvasionMusic = Factory.CreateBoolSet(387);
			InvasionSlotCount = Factory.CreateIntSet(1, 216, 5, 395, 10, 491, 10, 471, 10, 472, 0, 387, 0);
			ImmuneToAllBuffs = Factory.CreateBoolSet();
			ImmuneToRegularBuffs = Factory.CreateBoolSet();
			SpecificDebuffImmunity = Factory.CreateCustomSet<bool?[]>(null, Array.Empty<object>());
			for (int type = 0; type < NPCLoader.NPCCount; type++)
			{
				SpecificDebuffImmunity[type] = new bool?[BuffLoader.BuffCount];
				if (DebuffImmunitySets.TryGetValue(type, out var data) && data != null)
				{
					ImmuneToAllBuffs[type] = data.ImmuneToAllBuffsThatAreNotWhips && data.ImmuneToWhips;
					ImmuneToRegularBuffs[type] = data.ImmuneToAllBuffsThatAreNotWhips;
					if (data.SpecificallyImmuneTo != null)
					{
						int[] specificallyImmuneTo = data.SpecificallyImmuneTo;
						foreach (int buff in specificallyImmuneTo)
						{
							SpecificDebuffImmunity[type][buff] = true;
						}
					}
				}
				SpecificDebuffImmunity[type][353] = ShimmerImmunity[type];
			}
		}
	}

	private static readonly int[] NetIdMap = new int[65]
	{
		81, 81, 1, 1, 1, 1, 1, 1, 1, 1,
		6, 6, 31, 31, 77, 42, 42, 176, 176, 176,
		176, 173, 173, 183, 183, 3, 3, 132, 132, 186,
		186, 187, 187, 188, 188, 189, 189, 190, 191, 192,
		193, 194, 2, 200, 200, 21, 21, 201, 201, 202,
		202, 203, 203, 223, 223, 231, 231, 232, 232, 233,
		233, 234, 234, 235, 235
	};

	private static readonly Dictionary<string, int> LegacyNameToIdMap = new Dictionary<string, int>
	{
		{ "Slimeling", -1 },
		{ "Slimer2", -2 },
		{ "Green Slime", -3 },
		{ "Pinky", -4 },
		{ "Baby Slime", -5 },
		{ "Black Slime", -6 },
		{ "Purple Slime", -7 },
		{ "Red Slime", -8 },
		{ "Yellow Slime", -9 },
		{ "Jungle Slime", -10 },
		{ "Little Eater", -11 },
		{ "Big Eater", -12 },
		{ "Short Bones", -13 },
		{ "Big Boned", -14 },
		{ "Heavy Skeleton", -15 },
		{ "Little Stinger", -16 },
		{ "Big Stinger", -17 },
		{ "Tiny Moss Hornet", -18 },
		{ "Little Moss Hornet", -19 },
		{ "Big Moss Hornet", -20 },
		{ "Giant Moss Hornet", -21 },
		{ "Little Crimera", -22 },
		{ "Big Crimera", -23 },
		{ "Little Crimslime", -24 },
		{ "Big Crimslime", -25 },
		{ "Small Zombie", -26 },
		{ "Big Zombie", -27 },
		{ "Small Bald Zombie", -28 },
		{ "Big Bald Zombie", -29 },
		{ "Small Pincushion Zombie", -30 },
		{ "Big Pincushion Zombie", -31 },
		{ "Small Slimed Zombie", -32 },
		{ "Big Slimed Zombie", -33 },
		{ "Small Swamp Zombie", -34 },
		{ "Big Swamp Zombie", -35 },
		{ "Small Twiggy Zombie", -36 },
		{ "Big Twiggy Zombie", -37 },
		{ "Cataract Eye 2", -38 },
		{ "Sleepy Eye 2", -39 },
		{ "Dialated Eye 2", -40 },
		{ "Green Eye 2", -41 },
		{ "Purple Eye 2", -42 },
		{ "Demon Eye 2", -43 },
		{ "Small Female Zombie", -44 },
		{ "Big Female Zombie", -45 },
		{ "Small Skeleton", -46 },
		{ "Big Skeleton", -47 },
		{ "Small Headache Skeleton", -48 },
		{ "Big Headache Skeleton", -49 },
		{ "Small Misassembled Skeleton", -50 },
		{ "Big Misassembled Skeleton", -51 },
		{ "Small Pantless Skeleton", -52 },
		{ "Big Pantless Skeleton", -53 },
		{ "Small Rain Zombie", -54 },
		{ "Big Rain Zombie", -55 },
		{ "Little Hornet Fatty", -56 },
		{ "Big Hornet Fatty", -57 },
		{ "Little Hornet Honey", -58 },
		{ "Big Hornet Honey", -59 },
		{ "Little Hornet Leafy", -60 },
		{ "Big Hornet Leafy", -61 },
		{ "Little Hornet Spikey", -62 },
		{ "Big Hornet Spikey", -63 },
		{ "Little Hornet Stingy", -64 },
		{ "Big Hornet Stingy", -65 },
		{ "Blue Slime", 1 },
		{ "Demon Eye", 2 },
		{ "Zombie", 3 },
		{ "Eye of Cthulhu", 4 },
		{ "Servant of Cthulhu", 5 },
		{ "Eater of Souls", 6 },
		{ "Devourer", 7 },
		{ "Giant Worm", 10 },
		{ "Eater of Worlds", 13 },
		{ "Mother Slime", 16 },
		{ "Merchant", 17 },
		{ "Nurse", 18 },
		{ "Arms Dealer", 19 },
		{ "Dryad", 20 },
		{ "Skeleton", 21 },
		{ "Guide", 22 },
		{ "Meteor Head", 23 },
		{ "Fire Imp", 24 },
		{ "Burning Sphere", 25 },
		{ "Goblin Peon", 26 },
		{ "Goblin Thief", 27 },
		{ "Goblin Warrior", 28 },
		{ "Goblin Sorcerer", 29 },
		{ "Chaos Ball", 30 },
		{ "Angry Bones", 31 },
		{ "Dark Caster", 32 },
		{ "Water Sphere", 33 },
		{ "Cursed Skull", 34 },
		{ "Skeletron", 35 },
		{ "Old Man", 37 },
		{ "Demolitionist", 38 },
		{ "Bone Serpent", 39 },
		{ "Hornet", 42 },
		{ "Man Eater", 43 },
		{ "Undead Miner", 44 },
		{ "Tim", 45 },
		{ "Bunny", 46 },
		{ "Corrupt Bunny", 47 },
		{ "Harpy", 48 },
		{ "Cave Bat", 49 },
		{ "King Slime", 50 },
		{ "Jungle Bat", 51 },
		{ "Doctor Bones", 52 },
		{ "The Groom", 53 },
		{ "Clothier", 54 },
		{ "Goldfish", 55 },
		{ "Snatcher", 56 },
		{ "Corrupt Goldfish", 57 },
		{ "Piranha", 58 },
		{ "Lava Slime", 59 },
		{ "Hellbat", 60 },
		{ "Vulture", 61 },
		{ "Demon", 62 },
		{ "Blue Jellyfish", 63 },
		{ "Pink Jellyfish", 64 },
		{ "Shark", 65 },
		{ "Voodoo Demon", 66 },
		{ "Crab", 67 },
		{ "Dungeon Guardian", 68 },
		{ "Antlion", 69 },
		{ "Spike Ball", 70 },
		{ "Dungeon Slime", 71 },
		{ "Blazing Wheel", 72 },
		{ "Goblin Scout", 73 },
		{ "Bird", 74 },
		{ "Pixie", 75 },
		{ "Armored Skeleton", 77 },
		{ "Mummy", 78 },
		{ "Dark Mummy", 79 },
		{ "Light Mummy", 80 },
		{ "Corrupt Slime", 81 },
		{ "Wraith", 82 },
		{ "Cursed Hammer", 83 },
		{ "Enchanted Sword", 84 },
		{ "Mimic", 85 },
		{ "Unicorn", 86 },
		{ "Wyvern", 87 },
		{ "Giant Bat", 93 },
		{ "Corruptor", 94 },
		{ "Digger", 95 },
		{ "World Feeder", 98 },
		{ "Clinger", 101 },
		{ "Angler Fish", 102 },
		{ "Green Jellyfish", 103 },
		{ "Werewolf", 104 },
		{ "Bound Goblin", 105 },
		{ "Bound Wizard", 106 },
		{ "Goblin Tinkerer", 107 },
		{ "Wizard", 108 },
		{ "Clown", 109 },
		{ "Skeleton Archer", 110 },
		{ "Goblin Archer", 111 },
		{ "Vile Spit", 112 },
		{ "Wall of Flesh", 113 },
		{ "The Hungry", 115 },
		{ "Leech", 117 },
		{ "Chaos Elemental", 120 },
		{ "Slimer", 121 },
		{ "Gastropod", 122 },
		{ "Bound Mechanic", 123 },
		{ "Mechanic", 124 },
		{ "Retinazer", 125 },
		{ "Spazmatism", 126 },
		{ "Skeletron Prime", 127 },
		{ "Prime Cannon", 128 },
		{ "Prime Saw", 129 },
		{ "Prime Vice", 130 },
		{ "Prime Laser", 131 },
		{ "Wandering Eye", 133 },
		{ "The Destroyer", 134 },
		{ "Illuminant Bat", 137 },
		{ "Illuminant Slime", 138 },
		{ "Probe", 139 },
		{ "Possessed Armor", 140 },
		{ "Toxic Sludge", 141 },
		{ "Santa Claus", 142 },
		{ "Snowman Gangsta", 143 },
		{ "Mister Stabby", 144 },
		{ "Snow Balla", 145 },
		{ "Ice Slime", 147 },
		{ "Penguin", 148 },
		{ "Ice Bat", 150 },
		{ "Lava Bat", 151 },
		{ "Giant Flying Fox", 152 },
		{ "Giant Tortoise", 153 },
		{ "Ice Tortoise", 154 },
		{ "Wolf", 155 },
		{ "Red Devil", 156 },
		{ "Arapaima", 157 },
		{ "Vampire", 158 },
		{ "Truffle", 160 },
		{ "Zombie Eskimo", 161 },
		{ "Frankenstein", 162 },
		{ "Black Recluse", 163 },
		{ "Wall Creeper", 164 },
		{ "Swamp Thing", 166 },
		{ "Undead Viking", 167 },
		{ "Corrupt Penguin", 168 },
		{ "Ice Elemental", 169 },
		{ "Pigron", 170 },
		{ "Rune Wizard", 172 },
		{ "Crimera", 173 },
		{ "Herpling", 174 },
		{ "Angry Trapper", 175 },
		{ "Moss Hornet", 176 },
		{ "Derpling", 177 },
		{ "Steampunker", 178 },
		{ "Crimson Axe", 179 },
		{ "Face Monster", 181 },
		{ "Floaty Gross", 182 },
		{ "Crimslime", 183 },
		{ "Spiked Ice Slime", 184 },
		{ "Snow Flinx", 185 },
		{ "Lost Girl", 195 },
		{ "Nymph", 196 },
		{ "Armored Viking", 197 },
		{ "Lihzahrd", 198 },
		{ "Spiked Jungle Slime", 204 },
		{ "Moth", 205 },
		{ "Icy Merman", 206 },
		{ "Dye Trader", 207 },
		{ "Party Girl", 208 },
		{ "Cyborg", 209 },
		{ "Bee", 210 },
		{ "Pirate Deckhand", 212 },
		{ "Pirate Corsair", 213 },
		{ "Pirate Deadeye", 214 },
		{ "Pirate Crossbower", 215 },
		{ "Pirate Captain", 216 },
		{ "Cochineal Beetle", 217 },
		{ "Cyan Beetle", 218 },
		{ "Lac Beetle", 219 },
		{ "Sea Snail", 220 },
		{ "Squid", 221 },
		{ "Queen Bee", 222 },
		{ "Raincoat Zombie", 223 },
		{ "Flying Fish", 224 },
		{ "Umbrella Slime", 225 },
		{ "Flying Snake", 226 },
		{ "Painter", 227 },
		{ "Witch Doctor", 228 },
		{ "Pirate", 229 },
		{ "Jungle Creeper", 236 },
		{ "Blood Crawler", 239 },
		{ "Blood Feeder", 241 },
		{ "Blood Jelly", 242 },
		{ "Ice Golem", 243 },
		{ "Rainbow Slime", 244 },
		{ "Golem", 245 },
		{ "Golem Head", 246 },
		{ "Golem Fist", 247 },
		{ "Angry Nimbus", 250 },
		{ "Eyezor", 251 },
		{ "Parrot", 252 },
		{ "Reaper", 253 },
		{ "Spore Zombie", 254 },
		{ "Fungo Fish", 256 },
		{ "Anomura Fungus", 257 },
		{ "Mushi Ladybug", 258 },
		{ "Fungi Bulb", 259 },
		{ "Giant Fungi Bulb", 260 },
		{ "Fungi Spore", 261 },
		{ "Plantera", 262 },
		{ "Plantera's Hook", 263 },
		{ "Plantera's Tentacle", 264 },
		{ "Spore", 265 },
		{ "Brain of Cthulhu", 266 },
		{ "Creeper", 267 },
		{ "Ichor Sticker", 268 },
		{ "Rusty Armored Bones", 269 },
		{ "Blue Armored Bones", 273 },
		{ "Hell Armored Bones", 277 },
		{ "Ragged Caster", 281 },
		{ "Necromancer", 283 },
		{ "Diabolist", 285 },
		{ "Bone Lee", 287 },
		{ "Dungeon Spirit", 288 },
		{ "Giant Cursed Skull", 289 },
		{ "Paladin", 290 },
		{ "Skeleton Sniper", 291 },
		{ "Tactical Skeleton", 292 },
		{ "Skeleton Commando", 293 },
		{ "Blue Jay", 297 },
		{ "Cardinal", 298 },
		{ "Squirrel", 299 },
		{ "Mouse", 300 },
		{ "Raven", 301 },
		{ "Slime", 302 },
		{ "Hoppin' Jack", 304 },
		{ "Scarecrow", 305 },
		{ "Headless Horseman", 315 },
		{ "Ghost", 316 },
		{ "Mourning Wood", 325 },
		{ "Splinterling", 326 },
		{ "Pumpking", 327 },
		{ "Hellhound", 329 },
		{ "Poltergeist", 330 },
		{ "Zombie Elf", 338 },
		{ "Present Mimic", 341 },
		{ "Gingerbread Man", 342 },
		{ "Yeti", 343 },
		{ "Everscream", 344 },
		{ "Ice Queen", 345 },
		{ "Santa", 346 },
		{ "Elf Copter", 347 },
		{ "Nutcracker", 348 },
		{ "Elf Archer", 350 },
		{ "Krampus", 351 },
		{ "Flocko", 352 },
		{ "Stylist", 353 },
		{ "Webbed Stylist", 354 },
		{ "Firefly", 355 },
		{ "Butterfly", 356 },
		{ "Worm", 357 },
		{ "Lightning Bug", 358 },
		{ "Snail", 359 },
		{ "Glowing Snail", 360 },
		{ "Frog", 361 },
		{ "Duck", 362 },
		{ "Scorpion", 366 },
		{ "Traveling Merchant", 368 },
		{ "Angler", 369 },
		{ "Duke Fishron", 370 },
		{ "Detonating Bubble", 371 },
		{ "Sharkron", 372 },
		{ "Truffle Worm", 374 },
		{ "Sleeping Angler", 376 },
		{ "Grasshopper", 377 },
		{ "Chattering Teeth Bomb", 378 },
		{ "Blue Cultist Archer", 379 },
		{ "White Cultist Archer", 380 },
		{ "Brain Scrambler", 381 },
		{ "Ray Gunner", 382 },
		{ "Martian Officer", 383 },
		{ "Bubble Shield", 384 },
		{ "Gray Grunt", 385 },
		{ "Martian Engineer", 386 },
		{ "Tesla Turret", 387 },
		{ "Martian Drone", 388 },
		{ "Gigazapper", 389 },
		{ "Scutlix Gunner", 390 },
		{ "Scutlix", 391 },
		{ "Martian Saucer", 392 },
		{ "Martian Saucer Turret", 393 },
		{ "Martian Saucer Cannon", 394 },
		{ "Moon Lord", 396 },
		{ "Moon Lord's Hand", 397 },
		{ "Moon Lord's Core", 398 },
		{ "Martian Probe", 399 },
		{ "Milkyway Weaver", 402 },
		{ "Star Cell", 405 },
		{ "Flow Invader", 407 },
		{ "Twinkle Popper", 409 },
		{ "Twinkle", 410 },
		{ "Stargazer", 411 },
		{ "Crawltipede", 412 },
		{ "Drakomire", 415 },
		{ "Drakomire Rider", 416 },
		{ "Sroller", 417 },
		{ "Corite", 418 },
		{ "Selenian", 419 },
		{ "Nebula Floater", 420 },
		{ "Brain Suckler", 421 },
		{ "Vortex Pillar", 422 },
		{ "Evolution Beast", 423 },
		{ "Predictor", 424 },
		{ "Storm Diver", 425 },
		{ "Alien Queen", 426 },
		{ "Alien Hornet", 427 },
		{ "Alien Larva", 428 },
		{ "Vortexian", 429 },
		{ "Mysterious Tablet", 437 },
		{ "Lunatic Devote", 438 },
		{ "Lunatic Cultist", 439 },
		{ "Tax Collector", 441 },
		{ "Gold Bird", 442 },
		{ "Gold Bunny", 443 },
		{ "Gold Butterfly", 444 },
		{ "Gold Frog", 445 },
		{ "Gold Grasshopper", 446 },
		{ "Gold Mouse", 447 },
		{ "Gold Worm", 448 },
		{ "Phantasm Dragon", 454 },
		{ "Butcher", 460 },
		{ "Creature from the Deep", 461 },
		{ "Fritz", 462 },
		{ "Nailhead", 463 },
		{ "Crimtane Bunny", 464 },
		{ "Crimtane Goldfish", 465 },
		{ "Psycho", 466 },
		{ "Deadly Sphere", 467 },
		{ "Dr. Man Fly", 468 },
		{ "The Possessed", 469 },
		{ "Vicious Penguin", 470 },
		{ "Goblin Summoner", 471 },
		{ "Shadowflame Apparation", 472 },
		{ "Corrupt Mimic", 473 },
		{ "Crimson Mimic", 474 },
		{ "Hallowed Mimic", 475 },
		{ "Jungle Mimic", 476 },
		{ "Mothron", 477 },
		{ "Mothron Egg", 478 },
		{ "Baby Mothron", 479 },
		{ "Medusa", 480 },
		{ "Hoplite", 481 },
		{ "Granite Golem", 482 },
		{ "Granite Elemental", 483 },
		{ "Enchanted Nightcrawler", 484 },
		{ "Grubby", 485 },
		{ "Sluggy", 486 },
		{ "Buggy", 487 },
		{ "Target Dummy", 488 },
		{ "Blood Zombie", 489 },
		{ "Drippler", 490 },
		{ "Stardust Pillar", 493 },
		{ "Crawdad", 494 },
		{ "Giant Shelly", 496 },
		{ "Salamander", 498 },
		{ "Nebula Pillar", 507 },
		{ "Antlion Charger", 508 },
		{ "Antlion Swarmer", 509 },
		{ "Dune Splicer", 510 },
		{ "Tomb Crawler", 513 },
		{ "Solar Flare", 516 },
		{ "Solar Pillar", 517 },
		{ "Drakanian", 518 },
		{ "Solar Fragment", 519 },
		{ "Martian Walker", 520 },
		{ "Ancient Vision", 521 },
		{ "Ancient Light", 522 },
		{ "Ancient Doom", 523 },
		{ "Ghoul", 524 },
		{ "Vile Ghoul", 525 },
		{ "Tainted Ghoul", 526 },
		{ "Dreamer Ghoul", 527 },
		{ "Lamia", 528 },
		{ "Sand Poacher", 530 },
		{ "Basilisk", 532 },
		{ "Desert Spirit", 533 },
		{ "Tortured Soul", 534 },
		{ "Spiked Slime", 535 },
		{ "The Bride", 536 },
		{ "Sand Slime", 537 },
		{ "Red Squirrel", 538 },
		{ "Gold Squirrel", 539 },
		{ "Sand Elemental", 541 },
		{ "Sand Shark", 542 },
		{ "Bone Biter", 543 },
		{ "Flesh Reaver", 544 },
		{ "Crystal Thresher", 545 },
		{ "Angry Tumbler", 546 },
		{ "???", 547 },
		{ "Eternia Crystal", 548 },
		{ "Mysterious Portal", 549 },
		{ "Tavernkeep", 550 },
		{ "Betsy", 551 },
		{ "Etherian Goblin", 552 },
		{ "Etherian Goblin Bomber", 555 },
		{ "Etherian Wyvern", 558 },
		{ "Etherian Javelin Thrower", 561 },
		{ "Dark Mage", 564 },
		{ "Old One's Skeleton", 566 },
		{ "Wither Beast", 568 },
		{ "Drakin", 570 },
		{ "Kobold", 572 },
		{ "Kobold Glider", 574 },
		{ "Ogre", 576 },
		{ "Etherian Lightning Bug", 578 }
	};

	public const short NegativeIDCount = -66;

	public const short BigHornetStingy = -65;

	public const short LittleHornetStingy = -64;

	public const short BigHornetSpikey = -63;

	public const short LittleHornetSpikey = -62;

	public const short BigHornetLeafy = -61;

	public const short LittleHornetLeafy = -60;

	public const short BigHornetHoney = -59;

	public const short LittleHornetHoney = -58;

	public const short BigHornetFatty = -57;

	public const short LittleHornetFatty = -56;

	public const short BigRainZombie = -55;

	public const short SmallRainZombie = -54;

	public const short BigPantlessSkeleton = -53;

	public const short SmallPantlessSkeleton = -52;

	public const short BigMisassembledSkeleton = -51;

	public const short SmallMisassembledSkeleton = -50;

	public const short BigHeadacheSkeleton = -49;

	public const short SmallHeadacheSkeleton = -48;

	public const short BigSkeleton = -47;

	public const short SmallSkeleton = -46;

	public const short BigFemaleZombie = -45;

	public const short SmallFemaleZombie = -44;

	public const short DemonEye2 = -43;

	public const short PurpleEye2 = -42;

	public const short GreenEye2 = -41;

	public const short DialatedEye2 = -40;

	public const short SleepyEye2 = -39;

	public const short CataractEye2 = -38;

	public const short BigTwiggyZombie = -37;

	public const short SmallTwiggyZombie = -36;

	public const short BigSwampZombie = -35;

	public const short SmallSwampZombie = -34;

	public const short BigSlimedZombie = -33;

	public const short SmallSlimedZombie = -32;

	public const short BigPincushionZombie = -31;

	public const short SmallPincushionZombie = -30;

	public const short BigBaldZombie = -29;

	public const short SmallBaldZombie = -28;

	public const short BigZombie = -27;

	public const short SmallZombie = -26;

	public const short BigCrimslime = -25;

	public const short LittleCrimslime = -24;

	public const short BigCrimera = -23;

	public const short LittleCrimera = -22;

	public const short GiantMossHornet = -21;

	public const short BigMossHornet = -20;

	public const short LittleMossHornet = -19;

	public const short TinyMossHornet = -18;

	public const short BigStinger = -17;

	public const short LittleStinger = -16;

	public const short HeavySkeleton = -15;

	public const short BigBoned = -14;

	public const short ShortBones = -13;

	public const short BigEater = -12;

	public const short LittleEater = -11;

	public const short JungleSlime = -10;

	public const short YellowSlime = -9;

	public const short RedSlime = -8;

	public const short PurpleSlime = -7;

	public const short BlackSlime = -6;

	public const short BabySlime = -5;

	public const short Pinky = -4;

	public const short GreenSlime = -3;

	public const short Slimer2 = -2;

	public const short Slimeling = -1;

	public const short None = 0;

	public const short BlueSlime = 1;

	public const short DemonEye = 2;

	public const short Zombie = 3;

	public const short EyeofCthulhu = 4;

	public const short ServantofCthulhu = 5;

	public const short EaterofSouls = 6;

	public const short DevourerHead = 7;

	public const short DevourerBody = 8;

	public const short DevourerTail = 9;

	public const short GiantWormHead = 10;

	public const short GiantWormBody = 11;

	public const short GiantWormTail = 12;

	public const short EaterofWorldsHead = 13;

	public const short EaterofWorldsBody = 14;

	public const short EaterofWorldsTail = 15;

	public const short MotherSlime = 16;

	public const short Merchant = 17;

	public const short Nurse = 18;

	public const short ArmsDealer = 19;

	public const short Dryad = 20;

	public const short Skeleton = 21;

	public const short Guide = 22;

	public const short MeteorHead = 23;

	public const short FireImp = 24;

	public const short BurningSphere = 25;

	public const short GoblinPeon = 26;

	public const short GoblinThief = 27;

	public const short GoblinWarrior = 28;

	public const short GoblinSorcerer = 29;

	public const short ChaosBall = 30;

	public const short AngryBones = 31;

	public const short DarkCaster = 32;

	public const short WaterSphere = 33;

	public const short CursedSkull = 34;

	public const short SkeletronHead = 35;

	public const short SkeletronHand = 36;

	public const short OldMan = 37;

	public const short Demolitionist = 38;

	public const short BoneSerpentHead = 39;

	public const short BoneSerpentBody = 40;

	public const short BoneSerpentTail = 41;

	public const short Hornet = 42;

	public const short ManEater = 43;

	public const short UndeadMiner = 44;

	public const short Tim = 45;

	public const short Bunny = 46;

	public const short CorruptBunny = 47;

	public const short Harpy = 48;

	public const short CaveBat = 49;

	public const short KingSlime = 50;

	public const short JungleBat = 51;

	public const short DoctorBones = 52;

	public const short TheGroom = 53;

	public const short Clothier = 54;

	public const short Goldfish = 55;

	public const short Snatcher = 56;

	public const short CorruptGoldfish = 57;

	public const short Piranha = 58;

	public const short LavaSlime = 59;

	public const short Hellbat = 60;

	public const short Vulture = 61;

	public const short Demon = 62;

	public const short BlueJellyfish = 63;

	public const short PinkJellyfish = 64;

	public const short Shark = 65;

	public const short VoodooDemon = 66;

	public const short Crab = 67;

	public const short DungeonGuardian = 68;

	public const short Antlion = 69;

	public const short SpikeBall = 70;

	public const short DungeonSlime = 71;

	public const short BlazingWheel = 72;

	public const short GoblinScout = 73;

	public const short Bird = 74;

	public const short Pixie = 75;

	public const short None2 = 76;

	public const short ArmoredSkeleton = 77;

	public const short Mummy = 78;

	public const short DarkMummy = 79;

	public const short LightMummy = 80;

	public const short CorruptSlime = 81;

	public const short Wraith = 82;

	public const short CursedHammer = 83;

	public const short EnchantedSword = 84;

	public const short Mimic = 85;

	public const short Unicorn = 86;

	public const short WyvernHead = 87;

	public const short WyvernLegs = 88;

	public const short WyvernBody = 89;

	public const short WyvernBody2 = 90;

	public const short WyvernBody3 = 91;

	public const short WyvernTail = 92;

	public const short GiantBat = 93;

	public const short Corruptor = 94;

	public const short DiggerHead = 95;

	public const short DiggerBody = 96;

	public const short DiggerTail = 97;

	public const short SeekerHead = 98;

	public const short SeekerBody = 99;

	public const short SeekerTail = 100;

	public const short Clinger = 101;

	public const short AnglerFish = 102;

	public const short GreenJellyfish = 103;

	public const short Werewolf = 104;

	public const short BoundGoblin = 105;

	public const short BoundWizard = 106;

	public const short GoblinTinkerer = 107;

	public const short Wizard = 108;

	public const short Clown = 109;

	public const short SkeletonArcher = 110;

	public const short GoblinArcher = 111;

	public const short VileSpit = 112;

	public const short WallofFlesh = 113;

	public const short WallofFleshEye = 114;

	public const short TheHungry = 115;

	public const short TheHungryII = 116;

	public const short LeechHead = 117;

	public const short LeechBody = 118;

	public const short LeechTail = 119;

	public const short ChaosElemental = 120;

	public const short Slimer = 121;

	public const short Gastropod = 122;

	public const short BoundMechanic = 123;

	public const short Mechanic = 124;

	public const short Retinazer = 125;

	public const short Spazmatism = 126;

	public const short SkeletronPrime = 127;

	public const short PrimeCannon = 128;

	public const short PrimeSaw = 129;

	public const short PrimeVice = 130;

	public const short PrimeLaser = 131;

	public const short BaldZombie = 132;

	public const short WanderingEye = 133;

	public const short TheDestroyer = 134;

	public const short TheDestroyerBody = 135;

	public const short TheDestroyerTail = 136;

	public const short IlluminantBat = 137;

	public const short IlluminantSlime = 138;

	public const short Probe = 139;

	public const short PossessedArmor = 140;

	public const short ToxicSludge = 141;

	public const short SantaClaus = 142;

	public const short SnowmanGangsta = 143;

	public const short MisterStabby = 144;

	public const short SnowBalla = 145;

	public const short None3 = 146;

	public const short IceSlime = 147;

	public const short Penguin = 148;

	public const short PenguinBlack = 149;

	public const short IceBat = 150;

	public const short Lavabat = 151;

	public const short GiantFlyingFox = 152;

	public const short GiantTortoise = 153;

	public const short IceTortoise = 154;

	public const short Wolf = 155;

	public const short RedDevil = 156;

	public const short Arapaima = 157;

	public const short VampireBat = 158;

	public const short Vampire = 159;

	public const short Truffle = 160;

	public const short ZombieEskimo = 161;

	public const short Frankenstein = 162;

	public const short BlackRecluse = 163;

	public const short WallCreeper = 164;

	public const short WallCreeperWall = 165;

	public const short SwampThing = 166;

	public const short UndeadViking = 167;

	public const short CorruptPenguin = 168;

	public const short IceElemental = 169;

	public const short PigronCorruption = 170;

	public const short PigronHallow = 171;

	public const short RuneWizard = 172;

	public const short Crimera = 173;

	public const short Herpling = 174;

	public const short AngryTrapper = 175;

	public const short MossHornet = 176;

	public const short Derpling = 177;

	public const short Steampunker = 178;

	public const short CrimsonAxe = 179;

	public const short PigronCrimson = 180;

	public const short FaceMonster = 181;

	public const short FloatyGross = 182;

	public const short Crimslime = 183;

	public const short SpikedIceSlime = 184;

	public const short SnowFlinx = 185;

	public const short PincushionZombie = 186;

	public const short SlimedZombie = 187;

	public const short SwampZombie = 188;

	public const short TwiggyZombie = 189;

	public const short CataractEye = 190;

	public const short SleepyEye = 191;

	public const short DialatedEye = 192;

	public const short GreenEye = 193;

	public const short PurpleEye = 194;

	public const short LostGirl = 195;

	public const short Nymph = 196;

	public const short ArmoredViking = 197;

	public const short Lihzahrd = 198;

	public const short LihzahrdCrawler = 199;

	public const short FemaleZombie = 200;

	public const short HeadacheSkeleton = 201;

	public const short MisassembledSkeleton = 202;

	public const short PantlessSkeleton = 203;

	public const short SpikedJungleSlime = 204;

	public const short Moth = 205;

	public const short IcyMerman = 206;

	public const short DyeTrader = 207;

	public const short PartyGirl = 208;

	public const short Cyborg = 209;

	public const short Bee = 210;

	public const short BeeSmall = 211;

	public const short PirateDeckhand = 212;

	public const short PirateCorsair = 213;

	public const short PirateDeadeye = 214;

	public const short PirateCrossbower = 215;

	public const short PirateCaptain = 216;

	public const short CochinealBeetle = 217;

	public const short CyanBeetle = 218;

	public const short LacBeetle = 219;

	public const short SeaSnail = 220;

	public const short Squid = 221;

	public const short QueenBee = 222;

	public const short ZombieRaincoat = 223;

	public const short FlyingFish = 224;

	public const short UmbrellaSlime = 225;

	public const short FlyingSnake = 226;

	public const short Painter = 227;

	public const short WitchDoctor = 228;

	public const short Pirate = 229;

	public const short GoldfishWalker = 230;

	public const short HornetFatty = 231;

	public const short HornetHoney = 232;

	public const short HornetLeafy = 233;

	public const short HornetSpikey = 234;

	public const short HornetStingy = 235;

	public const short JungleCreeper = 236;

	public const short JungleCreeperWall = 237;

	public const short BlackRecluseWall = 238;

	public const short BloodCrawler = 239;

	public const short BloodCrawlerWall = 240;

	public const short BloodFeeder = 241;

	public const short BloodJelly = 242;

	public const short IceGolem = 243;

	public const short RainbowSlime = 244;

	public const short Golem = 245;

	public const short GolemHead = 246;

	public const short GolemFistLeft = 247;

	public const short GolemFistRight = 248;

	public const short GolemHeadFree = 249;

	public const short AngryNimbus = 250;

	public const short Eyezor = 251;

	public const short Parrot = 252;

	public const short Reaper = 253;

	public const short ZombieMushroom = 254;

	public const short ZombieMushroomHat = 255;

	public const short FungoFish = 256;

	public const short AnomuraFungus = 257;

	public const short MushiLadybug = 258;

	public const short FungiBulb = 259;

	public const short GiantFungiBulb = 260;

	public const short FungiSpore = 261;

	public const short Plantera = 262;

	public const short PlanterasHook = 263;

	public const short PlanterasTentacle = 264;

	public const short Spore = 265;

	public const short BrainofCthulhu = 266;

	public const short Creeper = 267;

	public const short IchorSticker = 268;

	public const short RustyArmoredBonesAxe = 269;

	public const short RustyArmoredBonesFlail = 270;

	public const short RustyArmoredBonesSword = 271;

	public const short RustyArmoredBonesSwordNoArmor = 272;

	public const short BlueArmoredBones = 273;

	public const short BlueArmoredBonesMace = 274;

	public const short BlueArmoredBonesNoPants = 275;

	public const short BlueArmoredBonesSword = 276;

	public const short HellArmoredBones = 277;

	public const short HellArmoredBonesSpikeShield = 278;

	public const short HellArmoredBonesMace = 279;

	public const short HellArmoredBonesSword = 280;

	public const short RaggedCaster = 281;

	public const short RaggedCasterOpenCoat = 282;

	public const short Necromancer = 283;

	public const short NecromancerArmored = 284;

	public const short DiabolistRed = 285;

	public const short DiabolistWhite = 286;

	public const short BoneLee = 287;

	public const short DungeonSpirit = 288;

	public const short GiantCursedSkull = 289;

	public const short Paladin = 290;

	public const short SkeletonSniper = 291;

	public const short TacticalSkeleton = 292;

	public const short SkeletonCommando = 293;

	public const short AngryBonesBig = 294;

	public const short AngryBonesBigMuscle = 295;

	public const short AngryBonesBigHelmet = 296;

	public const short BirdBlue = 297;

	public const short BirdRed = 298;

	public const short Squirrel = 299;

	public const short Mouse = 300;

	public const short Raven = 301;

	public const short SlimeMasked = 302;

	public const short BunnySlimed = 303;

	public const short HoppinJack = 304;

	public const short Scarecrow1 = 305;

	public const short Scarecrow2 = 306;

	public const short Scarecrow3 = 307;

	public const short Scarecrow4 = 308;

	public const short Scarecrow5 = 309;

	public const short Scarecrow6 = 310;

	public const short Scarecrow7 = 311;

	public const short Scarecrow8 = 312;

	public const short Scarecrow9 = 313;

	public const short Scarecrow10 = 314;

	public const short HeadlessHorseman = 315;

	public const short Ghost = 316;

	public const short DemonEyeOwl = 317;

	public const short DemonEyeSpaceship = 318;

	public const short ZombieDoctor = 319;

	public const short ZombieSuperman = 320;

	public const short ZombiePixie = 321;

	public const short SkeletonTopHat = 322;

	public const short SkeletonAstonaut = 323;

	public const short SkeletonAlien = 324;

	public const short MourningWood = 325;

	public const short Splinterling = 326;

	public const short Pumpking = 327;

	public const short PumpkingBlade = 328;

	public const short Hellhound = 329;

	public const short Poltergeist = 330;

	public const short ZombieXmas = 331;

	public const short ZombieSweater = 332;

	public const short SlimeRibbonWhite = 333;

	public const short SlimeRibbonYellow = 334;

	public const short SlimeRibbonGreen = 335;

	public const short SlimeRibbonRed = 336;

	public const short BunnyXmas = 337;

	public const short ZombieElf = 338;

	public const short ZombieElfBeard = 339;

	public const short ZombieElfGirl = 340;

	public const short PresentMimic = 341;

	public const short GingerbreadMan = 342;

	public const short Yeti = 343;

	public const short Everscream = 344;

	public const short IceQueen = 345;

	public const short SantaNK1 = 346;

	public const short ElfCopter = 347;

	public const short Nutcracker = 348;

	public const short NutcrackerSpinning = 349;

	public const short ElfArcher = 350;

	public const short Krampus = 351;

	public const short Flocko = 352;

	public const short Stylist = 353;

	public const short WebbedStylist = 354;

	public const short Firefly = 355;

	public const short Butterfly = 356;

	public const short Worm = 357;

	public const short LightningBug = 358;

	public const short Snail = 359;

	public const short GlowingSnail = 360;

	public const short Frog = 361;

	public const short Duck = 362;

	public const short Duck2 = 363;

	public const short DuckWhite = 364;

	public const short DuckWhite2 = 365;

	public const short ScorpionBlack = 366;

	public const short Scorpion = 367;

	public const short TravellingMerchant = 368;

	public const short Angler = 369;

	public const short DukeFishron = 370;

	public const short DetonatingBubble = 371;

	public const short Sharkron = 372;

	public const short Sharkron2 = 373;

	public const short TruffleWorm = 374;

	public const short TruffleWormDigger = 375;

	public const short SleepingAngler = 376;

	public const short Grasshopper = 377;

	public const short ChatteringTeethBomb = 378;

	public const short CultistArcherBlue = 379;

	public const short CultistArcherWhite = 380;

	public const short BrainScrambler = 381;

	public const short RayGunner = 382;

	public const short MartianOfficer = 383;

	public const short ForceBubble = 384;

	public const short GrayGrunt = 385;

	public const short MartianEngineer = 386;

	public const short MartianTurret = 387;

	public const short MartianDrone = 388;

	public const short GigaZapper = 389;

	public const short ScutlixRider = 390;

	public const short Scutlix = 391;

	public const short MartianSaucer = 392;

	public const short MartianSaucerTurret = 393;

	public const short MartianSaucerCannon = 394;

	public const short MartianSaucerCore = 395;

	public const short MoonLordHead = 396;

	public const short MoonLordHand = 397;

	public const short MoonLordCore = 398;

	public const short MartianProbe = 399;

	public const short MoonLordFreeEye = 400;

	public const short MoonLordLeechBlob = 401;

	public const short StardustWormHead = 402;

	public const short StardustWormBody = 403;

	public const short StardustWormTail = 404;

	public const short StardustCellBig = 405;

	public const short StardustCellSmall = 406;

	public const short StardustJellyfishBig = 407;

	public const short StardustJellyfishSmall = 408;

	public const short StardustSpiderBig = 409;

	public const short StardustSpiderSmall = 410;

	public const short StardustSoldier = 411;

	public const short SolarCrawltipedeHead = 412;

	public const short SolarCrawltipedeBody = 413;

	public const short SolarCrawltipedeTail = 414;

	public const short SolarDrakomire = 415;

	public const short SolarDrakomireRider = 416;

	public const short SolarSroller = 417;

	public const short SolarCorite = 418;

	public const short SolarSolenian = 419;

	public const short NebulaBrain = 420;

	public const short NebulaHeadcrab = 421;

	public const short NebulaBeast = 423;

	public const short NebulaSoldier = 424;

	public const short VortexRifleman = 425;

	public const short VortexHornetQueen = 426;

	public const short VortexHornet = 427;

	public const short VortexLarva = 428;

	public const short VortexSoldier = 429;

	public const short ArmedZombie = 430;

	public const short ArmedZombieEskimo = 431;

	public const short ArmedZombiePincussion = 432;

	public const short ArmedZombieSlimed = 433;

	public const short ArmedZombieSwamp = 434;

	public const short ArmedZombieTwiggy = 435;

	public const short ArmedZombieCenx = 436;

	public const short CultistTablet = 437;

	public const short CultistDevote = 438;

	public const short CultistBoss = 439;

	public const short CultistBossClone = 440;

	public const short GoldBird = 442;

	public const short GoldBunny = 443;

	public const short GoldButterfly = 444;

	public const short GoldFrog = 445;

	public const short GoldGrasshopper = 446;

	public const short GoldMouse = 447;

	public const short GoldWorm = 448;

	public const short BoneThrowingSkeleton = 449;

	public const short BoneThrowingSkeleton2 = 450;

	public const short BoneThrowingSkeleton3 = 451;

	public const short BoneThrowingSkeleton4 = 452;

	public const short SkeletonMerchant = 453;

	public const short CultistDragonHead = 454;

	public const short CultistDragonBody1 = 455;

	public const short CultistDragonBody2 = 456;

	public const short CultistDragonBody3 = 457;

	public const short CultistDragonBody4 = 458;

	public const short CultistDragonTail = 459;

	public const short Butcher = 460;

	public const short CreatureFromTheDeep = 461;

	public const short Fritz = 462;

	public const short Nailhead = 463;

	public const short CrimsonBunny = 464;

	public const short CrimsonGoldfish = 465;

	public const short Psycho = 466;

	public const short DeadlySphere = 467;

	public const short DrManFly = 468;

	public const short ThePossessed = 469;

	public const short CrimsonPenguin = 470;

	public const short GoblinSummoner = 471;

	public const short ShadowFlameApparition = 472;

	public const short BigMimicCorruption = 473;

	public const short BigMimicCrimson = 474;

	public const short BigMimicHallow = 475;

	public const short BigMimicJungle = 476;

	public const short Mothron = 477;

	public const short MothronEgg = 478;

	public const short MothronSpawn = 479;

	public const short Medusa = 480;

	public const short GreekSkeleton = 481;

	public const short GraniteGolem = 482;

	public const short GraniteFlyer = 483;

	public const short EnchantedNightcrawler = 484;

	public const short Grubby = 485;

	public const short Sluggy = 486;

	public const short Buggy = 487;

	public const short TargetDummy = 488;

	public const short BloodZombie = 489;

	public const short Drippler = 490;

	public const short PirateShip = 491;

	public const short PirateShipCannon = 492;

	public const short LunarTowerStardust = 493;

	public const short Crawdad = 494;

	public const short Crawdad2 = 495;

	public const short GiantShelly = 496;

	public const short GiantShelly2 = 497;

	public const short Salamander = 498;

	public const short Salamander2 = 499;

	public const short Salamander3 = 500;

	public const short Salamander4 = 501;

	public const short Salamander5 = 502;

	public const short Salamander6 = 503;

	public const short Salamander7 = 504;

	public const short Salamander8 = 505;

	public const short Salamander9 = 506;

	public const short LunarTowerNebula = 507;

	public const short LunarTowerVortex = 422;

	public const short TaxCollector = 441;

	public const short GiantWalkingAntlion = 508;

	public const short GiantFlyingAntlion = 509;

	public const short DuneSplicerHead = 510;

	public const short DuneSplicerBody = 511;

	public const short DuneSplicerTail = 512;

	public const short TombCrawlerHead = 513;

	public const short TombCrawlerBody = 514;

	public const short TombCrawlerTail = 515;

	public const short SolarFlare = 516;

	public const short LunarTowerSolar = 517;

	public const short SolarSpearman = 518;

	public const short SolarGoop = 519;

	public const short MartianWalker = 520;

	public const short AncientCultistSquidhead = 521;

	public const short AncientLight = 522;

	public const short AncientDoom = 523;

	public const short DesertGhoul = 524;

	public const short DesertGhoulCorruption = 525;

	public const short DesertGhoulCrimson = 526;

	public const short DesertGhoulHallow = 527;

	public const short DesertLamiaLight = 528;

	public const short DesertLamiaDark = 529;

	public const short DesertScorpionWalk = 530;

	public const short DesertScorpionWall = 531;

	public const short DesertBeast = 532;

	public const short DesertDjinn = 533;

	public const short DemonTaxCollector = 534;

	public const short SlimeSpiked = 535;

	public const short TheBride = 536;

	public const short SandSlime = 537;

	public const short SquirrelRed = 538;

	public const short SquirrelGold = 539;

	public const short PartyBunny = 540;

	public const short SandElemental = 541;

	public const short SandShark = 542;

	public const short SandsharkCorrupt = 543;

	public const short SandsharkCrimson = 544;

	public const short SandsharkHallow = 545;

	public const short Tumbleweed = 546;

	public const short DD2AttackerTest = 547;

	public const short DD2EterniaCrystal = 548;

	public const short DD2LanePortal = 549;

	public const short DD2Bartender = 550;

	public const short DD2Betsy = 551;

	public const short DD2GoblinT1 = 552;

	public const short DD2GoblinT2 = 553;

	public const short DD2GoblinT3 = 554;

	public const short DD2GoblinBomberT1 = 555;

	public const short DD2GoblinBomberT2 = 556;

	public const short DD2GoblinBomberT3 = 557;

	public const short DD2WyvernT1 = 558;

	public const short DD2WyvernT2 = 559;

	public const short DD2WyvernT3 = 560;

	public const short DD2JavelinstT1 = 561;

	public const short DD2JavelinstT2 = 562;

	public const short DD2JavelinstT3 = 563;

	public const short DD2DarkMageT1 = 564;

	public const short DD2DarkMageT3 = 565;

	public const short DD2SkeletonT1 = 566;

	public const short DD2SkeletonT3 = 567;

	public const short DD2WitherBeastT2 = 568;

	public const short DD2WitherBeastT3 = 569;

	public const short DD2DrakinT2 = 570;

	public const short DD2DrakinT3 = 571;

	public const short DD2KoboldWalkerT2 = 572;

	public const short DD2KoboldWalkerT3 = 573;

	public const short DD2KoboldFlyerT2 = 574;

	public const short DD2KoboldFlyerT3 = 575;

	public const short DD2OgreT2 = 576;

	public const short DD2OgreT3 = 577;

	public const short DD2LightningBugT3 = 578;

	public const short BartenderUnconscious = 579;

	public const short WalkingAntlion = 580;

	public const short FlyingAntlion = 581;

	public const short LarvaeAntlion = 582;

	public const short FairyCritterPink = 583;

	public const short FairyCritterGreen = 584;

	public const short FairyCritterBlue = 585;

	public const short ZombieMerman = 586;

	public const short EyeballFlyingFish = 587;

	public const short Golfer = 588;

	public const short GolferRescue = 589;

	public const short TorchZombie = 590;

	public const short ArmedTorchZombie = 591;

	public const short GoldGoldfish = 592;

	public const short GoldGoldfishWalker = 593;

	public const short WindyBalloon = 594;

	public const short BlackDragonfly = 595;

	public const short BlueDragonfly = 596;

	public const short GreenDragonfly = 597;

	public const short OrangeDragonfly = 598;

	public const short RedDragonfly = 599;

	public const short YellowDragonfly = 600;

	public const short GoldDragonfly = 601;

	public const short Seagull = 602;

	public const short Seagull2 = 603;

	public const short LadyBug = 604;

	public const short GoldLadyBug = 605;

	public const short Maggot = 606;

	public const short Pupfish = 607;

	public const short Grebe = 608;

	public const short Grebe2 = 609;

	public const short Rat = 610;

	public const short Owl = 611;

	public const short WaterStrider = 612;

	public const short GoldWaterStrider = 613;

	public const short ExplosiveBunny = 614;

	public const short Dolphin = 615;

	public const short Turtle = 616;

	public const short TurtleJungle = 617;

	public const short BloodNautilus = 618;

	public const short BloodSquid = 619;

	public const short GoblinShark = 620;

	public const short BloodEelHead = 621;

	public const short BloodEelBody = 622;

	public const short BloodEelTail = 623;

	public const short Gnome = 624;

	public const short SeaTurtle = 625;

	public const short Seahorse = 626;

	public const short GoldSeahorse = 627;

	public const short Dandelion = 628;

	public const short IceMimic = 629;

	public const short BloodMummy = 630;

	public const short RockGolem = 631;

	public const short MaggotZombie = 632;

	public const short BestiaryGirl = 633;

	public const short SporeBat = 634;

	public const short SporeSkeleton = 635;

	public const short HallowBoss = 636;

	public const short TownCat = 637;

	public const short TownDog = 638;

	public const short GemSquirrelAmethyst = 639;

	public const short GemSquirrelTopaz = 640;

	public const short GemSquirrelSapphire = 641;

	public const short GemSquirrelEmerald = 642;

	public const short GemSquirrelRuby = 643;

	public const short GemSquirrelDiamond = 644;

	public const short GemSquirrelAmber = 645;

	public const short GemBunnyAmethyst = 646;

	public const short GemBunnyTopaz = 647;

	public const short GemBunnySapphire = 648;

	public const short GemBunnyEmerald = 649;

	public const short GemBunnyRuby = 650;

	public const short GemBunnyDiamond = 651;

	public const short GemBunnyAmber = 652;

	public const short HellButterfly = 653;

	public const short Lavafly = 654;

	public const short MagmaSnail = 655;

	public const short TownBunny = 656;

	public const short QueenSlimeBoss = 657;

	public const short QueenSlimeMinionBlue = 658;

	public const short QueenSlimeMinionPink = 659;

	public const short QueenSlimeMinionPurple = 660;

	public const short EmpressButterfly = 661;

	public const short PirateGhost = 662;

	public const short Princess = 663;

	public const short TorchGod = 664;

	public const short ChaosBallTim = 665;

	public const short VileSpitEaterOfWorlds = 666;

	public const short GoldenSlime = 667;

	public const short Deerclops = 668;

	public const short Stinkbug = 669;

	public const short TownSlimeBlue = 670;

	public const short ScarletMacaw = 671;

	public const short BlueMacaw = 672;

	public const short Toucan = 673;

	public const short YellowCockatiel = 674;

	public const short GrayCockatiel = 675;

	public const short ShimmerSlime = 676;

	public const short Shimmerfly = 677;

	public const short TownSlimeGreen = 678;

	public const short TownSlimeOld = 679;

	public const short TownSlimePurple = 680;

	public const short TownSlimeRainbow = 681;

	public const short TownSlimeRed = 682;

	public const short TownSlimeYellow = 683;

	public const short TownSlimeCopper = 684;

	public const short BoundTownSlimeOld = 685;

	public const short BoundTownSlimePurple = 686;

	public const short BoundTownSlimeYellow = 687;

	public static readonly short Count = 688;

	public static readonly IdDictionary Search = IdDictionary.Create<NPCID, short>();

	public static int FromLegacyName(string name)
	{
		if (LegacyNameToIdMap.TryGetValue(name, out var value))
		{
			return value;
		}
		return 0;
	}

	public static int FromNetId(int id)
	{
		if (id < 0)
		{
			return NetIdMap[-id - 1];
		}
		return id;
	}
}
