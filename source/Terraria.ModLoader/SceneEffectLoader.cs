using System.Collections.Generic;
using Terraria.Graphics.Capture;

namespace Terraria.ModLoader;

/// <summary>
/// This serves as the central class from which SceneEffect functions are supported and carried out.
/// </summary>
public abstract class SceneEffectLoader<T> : Loader<T> where T : ModType
{
	public virtual void ChooseStyle(out int style, out SceneEffectPriority priority)
	{
		style = -1;
		priority = SceneEffectPriority.None;
	}
}
public class SceneEffectLoader : Loader<ModSceneEffect>
{
	public class SceneEffectInstance
	{
		public struct PrioritizedPair
		{
			public static readonly PrioritizedPair Default = new PrioritizedPair
			{
				value = -1
			};

			public int value;

			public SceneEffectPriority priority;
		}

		public bool anyActive;

		public PrioritizedPair waterStyle;

		public PrioritizedPair undergroundBackground;

		public PrioritizedPair surfaceBackground;

		public PrioritizedPair music;

		public string mapBackground;

		public CaptureBiome.TileColorStyle tileColorStyle;

		public PrioritizedPair biomeTorchItemType;

		public PrioritizedPair biomeCampfireItemType;

		public SceneEffectInstance()
		{
			waterStyle = (undergroundBackground = (surfaceBackground = (music = (biomeTorchItemType = (biomeCampfireItemType = PrioritizedPair.Default)))));
			tileColorStyle = CaptureBiome.TileColorStyle.Normal;
			mapBackground = null;
		}
	}

	private struct AtmosWeight
	{
		public float weight;

		public ModSceneEffect type;

		public AtmosWeight(float weight, ModSceneEffect type)
		{
			this.weight = weight;
			this.type = type;
		}

		public static int InvertedCompare(AtmosWeight a, AtmosWeight b)
		{
			return -a.weight.CompareTo(b.weight);
		}
	}

	public const int VanillaSceneEffectCount = 0;

	public SceneEffectLoader()
	{
		Initialize(0);
	}

	public void UpdateSceneEffect(Player player)
	{
		SceneEffectInstance result = new SceneEffectInstance();
		List<AtmosWeight> shortList = new List<AtmosWeight>();
		for (int j = 0; j < list.Count; j++)
		{
			ModSceneEffect sceneEffect2 = list[j];
			bool isActive = sceneEffect2.IsSceneEffectActive(player);
			sceneEffect2.SpecialVisuals(player, isActive);
			if (isActive)
			{
				shortList.Add(new AtmosWeight(sceneEffect2.GetCorrWeight(player), sceneEffect2));
			}
		}
		if (shortList.Count == 0)
		{
			player.CurrentSceneEffect = result;
			return;
		}
		result.anyActive = true;
		shortList.Sort(AtmosWeight.InvertedCompare);
		int sceneEffectFields = 0;
		int i = 0;
		while (sceneEffectFields < 8 && i < shortList.Count)
		{
			ModSceneEffect sceneEffect = shortList[i].type;
			if (result.waterStyle.priority == SceneEffectPriority.None && sceneEffect.WaterStyle != null)
			{
				result.waterStyle.value = sceneEffect.WaterStyle.Slot;
				result.waterStyle.priority = sceneEffect.Priority;
				sceneEffectFields++;
			}
			if (result.undergroundBackground.priority == SceneEffectPriority.None && sceneEffect.UndergroundBackgroundStyle != null)
			{
				result.undergroundBackground.value = sceneEffect.UndergroundBackgroundStyle.Slot;
				result.undergroundBackground.priority = sceneEffect.Priority;
				sceneEffectFields++;
			}
			if (result.surfaceBackground.priority == SceneEffectPriority.None && sceneEffect.SurfaceBackgroundStyle != null)
			{
				result.surfaceBackground.value = sceneEffect.SurfaceBackgroundStyle.Slot;
				result.surfaceBackground.priority = sceneEffect.Priority;
				sceneEffectFields++;
			}
			if (result.music.priority == SceneEffectPriority.None && sceneEffect.Music != -1)
			{
				result.music.value = sceneEffect.Music;
				result.music.priority = sceneEffect.Priority;
				sceneEffectFields++;
			}
			if (sceneEffect is ModBiome modBiome)
			{
				if (result.biomeTorchItemType.priority == SceneEffectPriority.None && modBiome.BiomeTorchItemType != -1)
				{
					result.biomeTorchItemType.value = modBiome.BiomeTorchItemType;
					result.biomeTorchItemType.priority = modBiome.Priority;
					sceneEffectFields++;
				}
				if (result.biomeCampfireItemType.priority == SceneEffectPriority.None && modBiome.BiomeCampfireItemType != -1)
				{
					result.biomeCampfireItemType.value = modBiome.BiomeCampfireItemType;
					result.biomeCampfireItemType.priority = modBiome.Priority;
					sceneEffectFields++;
				}
			}
			if (result.tileColorStyle == CaptureBiome.TileColorStyle.Normal && sceneEffect.TileColorStyle != 0)
			{
				result.tileColorStyle = sceneEffect.TileColorStyle;
				sceneEffectFields++;
			}
			if (result.mapBackground == null && sceneEffect.MapBackground != null)
			{
				result.mapBackground = sceneEffect.MapBackground;
				sceneEffectFields++;
			}
			i++;
		}
		player.CurrentSceneEffect = result;
	}

	public void UpdateMusic(ref int music, ref SceneEffectPriority priority)
	{
		SceneEffectInstance.PrioritizedPair currentMusic = Main.LocalPlayer.CurrentSceneEffect.music;
		if (currentMusic.value > -1 && currentMusic.priority > priority)
		{
			music = currentMusic.value;
			priority = currentMusic.priority;
		}
	}
}
