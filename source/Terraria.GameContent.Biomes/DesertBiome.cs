using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using Terraria.GameContent.Biomes.Desert;
using Terraria.WorldBuilding;

namespace Terraria.GameContent.Biomes;

public class DesertBiome : MicroBiome
{
	[JsonProperty("ChanceOfEntrance")]
	public double ChanceOfEntrance = 0.3333;

	public override bool Place(Point origin, StructureMap structures)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		DesertDescription desertDescription = DesertDescription.CreateFromPlacement(origin);
		if (!desertDescription.IsValid)
		{
			return false;
		}
		ExportDescriptionToEngine(desertDescription);
		SandMound.Place(desertDescription);
		desertDescription.UpdateSurfaceMap();
		if (!Main.tenthAnniversaryWorld && GenBase._random.NextDouble() <= ChanceOfEntrance)
		{
			switch (GenBase._random.Next(4))
			{
			case 0:
				ChambersEntrance.Place(desertDescription);
				break;
			case 1:
				AnthillEntrance.Place(desertDescription);
				break;
			case 2:
				LarvaHoleEntrance.Place(desertDescription);
				break;
			case 3:
				PitEntrance.Place(desertDescription);
				break;
			}
		}
		DesertHive.Place(desertDescription);
		CleanupArea(desertDescription.Hive);
		int x = desertDescription.CombinedArea.X;
		int width = desertDescription.CombinedArea.Width;
		Rectangle combinedArea = desertDescription.CombinedArea;
		Rectangle area = default(Rectangle);
		((Rectangle)(ref area))._002Ector(x, 50, width, ((Rectangle)(ref combinedArea)).Bottom - 20);
		structures.AddStructure(area, 10);
		return true;
	}

	private static void ExportDescriptionToEngine(DesertDescription description)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		GenVars.UndergroundDesertLocation = description.CombinedArea;
		((Rectangle)(ref GenVars.UndergroundDesertLocation)).Inflate(10, 10);
		GenVars.UndergroundDesertHiveLocation = description.Hive;
	}

	private static void CleanupArea(Rectangle area)
	{
		for (int i = -20 + ((Rectangle)(ref area)).Left; i < ((Rectangle)(ref area)).Right + 20; i++)
		{
			for (int j = -20 + ((Rectangle)(ref area)).Top; j < ((Rectangle)(ref area)).Bottom + 20; j++)
			{
				if (i > 0 && i < Main.maxTilesX - 1 && j > 0 && j < Main.maxTilesY - 1)
				{
					WorldGen.SquareWallFrame(i, j);
					WorldUtils.TileFrame(i, j, frameNeighbors: true);
				}
			}
		}
	}
}
