using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using ReLogic.Threading;
using Terraria.ModLoader;

namespace Terraria.Graphics.Light;

public class LightingEngine : ILightingEngine
{
	private enum EngineState
	{
		MinimapUpdate,
		ExportMetrics,
		Scan,
		Blur,
		Max
	}

	private struct PerFrameLight
	{
		public readonly Point Position;

		public readonly Vector3 Color;

		public PerFrameLight(Point position, Vector3 color)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			Position = position;
			Color = color;
		}
	}

	public const int AREA_PADDING = 28;

	private const int NON_VISIBLE_PADDING = 18;

	private readonly List<PerFrameLight> _perFrameLights = new List<PerFrameLight>();

	private TileLightScanner _tileScanner = new TileLightScanner();

	private LightMap _activeLightMap = new LightMap();

	private Rectangle _activeProcessedArea;

	private LightMap _workingLightMap = new LightMap();

	private Rectangle _workingProcessedArea;

	private readonly Stopwatch _timer = new Stopwatch();

	private EngineState _state;

	public void AddLight(int x, int y, Vector3 color)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		_perFrameLights.Add(new PerFrameLight(new Point(x, y), color));
	}

	public void Clear()
	{
		_activeLightMap.Clear();
		_workingLightMap.Clear();
		_perFrameLights.Clear();
	}

	public Vector3 GetColor(int x, int y)
	{
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		if (!((Rectangle)(ref _activeProcessedArea)).Contains(x, y))
		{
			return Vector3.Zero;
		}
		x -= _activeProcessedArea.X;
		y -= _activeProcessedArea.Y;
		return _activeLightMap[x, y];
	}

	public void ProcessArea(Rectangle area)
	{
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		Main.renderCount = (Main.renderCount + 1) % 4;
		_timer.Start();
		TimeLogger.LightingTime(0, 0.0);
		switch (_state)
		{
		case EngineState.MinimapUpdate:
			if (Main.mapDelay > 0)
			{
				Main.mapDelay--;
			}
			else
			{
				ExportToMiniMap();
			}
			TimeLogger.LightingTime(1, _timer.Elapsed.TotalMilliseconds);
			break;
		case EngineState.ExportMetrics:
			((Rectangle)(ref area)).Inflate(28, 28);
			Main.SceneMetrics.ScanAndExportToMain(new SceneMetricsScanSettings
			{
				VisualScanArea = area,
				BiomeScanCenterPositionInWorld = Main.LocalPlayer.Center,
				ScanOreFinderData = Main.LocalPlayer.accOreFinder
			});
			TimeLogger.LightingTime(2, _timer.Elapsed.TotalMilliseconds);
			break;
		case EngineState.Scan:
			ProcessScan(area);
			TimeLogger.LightingTime(3, _timer.Elapsed.TotalMilliseconds);
			break;
		case EngineState.Blur:
			ProcessBlur();
			Present();
			TimeLogger.LightingTime(4, _timer.Elapsed.TotalMilliseconds);
			break;
		}
		IncrementState();
		_timer.Reset();
	}

	private void IncrementState()
	{
		_state = (EngineState)((int)(_state + 1) % 4);
	}

	private void ProcessScan(Rectangle area)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		((Rectangle)(ref area)).Inflate(28, 28);
		_workingProcessedArea = area;
		_workingLightMap.SetSize(area.Width, area.Height);
		_workingLightMap.NonVisiblePadding = 18;
		_tileScanner.Update();
		_tileScanner.ExportTo(area, _workingLightMap, new TileLightScannerOptions
		{
			DrawInvisibleWalls = Main.ShouldShowInvisibleWalls()
		});
	}

	private void ProcessBlur()
	{
		UpdateLightDecay();
		ApplyPerFrameLights();
		_workingLightMap.Blur();
	}

	private void Present()
	{
		Utils.Swap(ref _activeLightMap, ref _workingLightMap);
		Utils.Swap(ref _activeProcessedArea, ref _workingProcessedArea);
	}

	private void UpdateLightDecay()
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0169: Unknown result type (might be due to invalid IL or missing references)
		//IL_0173: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_021f: Unknown result type (might be due to invalid IL or missing references)
		//IL_022b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0237: Unknown result type (might be due to invalid IL or missing references)
		//IL_025e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01de: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0204: Unknown result type (might be due to invalid IL or missing references)
		//IL_020e: Unknown result type (might be due to invalid IL or missing references)
		LightMap workingLightMap = _workingLightMap;
		workingLightMap.LightDecayThroughAir = 0.91f;
		workingLightMap.LightDecayThroughSolid = 0.56f;
		workingLightMap.LightDecayThroughHoney = new Vector3(0.75f, 0.7f, 0.6f) * 0.91f;
		switch (Main.waterStyle)
		{
		case 0:
		case 1:
		case 7:
		case 8:
			workingLightMap.LightDecayThroughWater = new Vector3(0.88f, 0.96f, 1.015f) * 0.91f;
			break;
		case 2:
			workingLightMap.LightDecayThroughWater = new Vector3(0.94f, 0.85f, 1.01f) * 0.91f;
			break;
		case 3:
			workingLightMap.LightDecayThroughWater = new Vector3(0.84f, 0.95f, 1.015f) * 0.91f;
			break;
		case 4:
			workingLightMap.LightDecayThroughWater = new Vector3(0.9f, 0.86f, 1.01f) * 0.91f;
			break;
		case 5:
			workingLightMap.LightDecayThroughWater = new Vector3(0.84f, 0.99f, 1.01f) * 0.91f;
			break;
		case 6:
			workingLightMap.LightDecayThroughWater = new Vector3(0.83f, 0.93f, 0.98f) * 0.91f;
			break;
		case 9:
			workingLightMap.LightDecayThroughWater = new Vector3(1f, 0.88f, 0.84f) * 0.91f;
			break;
		case 10:
			workingLightMap.LightDecayThroughWater = new Vector3(0.83f, 1f, 1f) * 0.91f;
			break;
		case 12:
			workingLightMap.LightDecayThroughWater = new Vector3(0.95f, 0.98f, 0.85f) * 0.91f;
			break;
		case 13:
			workingLightMap.LightDecayThroughWater = new Vector3(0.9f, 1f, 1.02f) * 0.91f;
			break;
		}
		float factor = 0.91f;
		float throughWaterR = workingLightMap.LightDecayThroughWater.X;
		float throughWaterG = workingLightMap.LightDecayThroughWater.Y;
		float throughWaterB = workingLightMap.LightDecayThroughWater.Z;
		LoaderManager.Get<WaterStylesLoader>().LightColorMultiplier(Main.waterStyle, factor, ref throughWaterR, ref throughWaterG, ref throughWaterB);
		workingLightMap.LightDecayThroughWater = new Vector3(throughWaterR, throughWaterG, throughWaterB);
		if (Main.player[Main.myPlayer].nightVision)
		{
			workingLightMap.LightDecayThroughAir *= 1.03f;
			workingLightMap.LightDecayThroughSolid *= 1.03f;
		}
		if (Main.player[Main.myPlayer].blind)
		{
			workingLightMap.LightDecayThroughAir *= 0.95f;
			workingLightMap.LightDecayThroughSolid *= 0.95f;
		}
		if (Main.player[Main.myPlayer].blackout)
		{
			workingLightMap.LightDecayThroughAir *= 0.85f;
			workingLightMap.LightDecayThroughSolid *= 0.85f;
		}
		if (Main.player[Main.myPlayer].headcovered)
		{
			workingLightMap.LightDecayThroughAir *= 0.85f;
			workingLightMap.LightDecayThroughSolid *= 0.85f;
		}
		workingLightMap.LightDecayThroughAir *= Player.airLightDecay;
		workingLightMap.LightDecayThroughSolid *= Player.solidLightDecay;
		float throughAir = 1f;
		float throughSolid = 1f;
		SystemLoader.ModifyLightingBrightness(ref throughAir, ref throughSolid);
		workingLightMap.LightDecayThroughAir *= throughAir;
		workingLightMap.LightDecayThroughSolid *= throughSolid;
	}

	private void ApplyPerFrameLights()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < _perFrameLights.Count; i++)
		{
			Point position = _perFrameLights[i].Position;
			if (((Rectangle)(ref _workingProcessedArea)).Contains(position))
			{
				Vector3 value = _perFrameLights[i].Color;
				Vector3 value2 = _workingLightMap[position.X - _workingProcessedArea.X, position.Y - _workingProcessedArea.Y];
				Vector3.Max(ref value2, ref value, ref value);
				_workingLightMap[position.X - _workingProcessedArea.X, position.Y - _workingProcessedArea.Y] = value;
			}
		}
		_perFrameLights.Clear();
	}

	public void Rebuild()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		_activeProcessedArea = Rectangle.Empty;
		_workingProcessedArea = Rectangle.Empty;
		_state = EngineState.MinimapUpdate;
		_activeLightMap = new LightMap();
		_workingLightMap = new LightMap();
	}

	private void ExportToMiniMap()
	{
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		if (!Main.mapEnabled || _activeProcessedArea.Width <= 0 || _activeProcessedArea.Height <= 0)
		{
			return;
		}
		Rectangle area = new Rectangle(_activeProcessedArea.X + 28, _activeProcessedArea.Y + 28, _activeProcessedArea.Width - 56, _activeProcessedArea.Height - 56);
		Rectangle value = default(Rectangle);
		((Rectangle)(ref value))._002Ector(0, 0, Main.maxTilesX, Main.maxTilesY);
		((Rectangle)(ref value)).Inflate(-40, -40);
		area = Rectangle.Intersect(area, value);
		Main.mapMinX = ((Rectangle)(ref area)).Left;
		Main.mapMinY = ((Rectangle)(ref area)).Top;
		Main.mapMaxX = ((Rectangle)(ref area)).Right;
		Main.mapMaxY = ((Rectangle)(ref area)).Bottom;
		FastParallel.For(((Rectangle)(ref area)).Left, ((Rectangle)(ref area)).Right, delegate(int start, int end, object context)
		{
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			for (int i = start; i < end; i++)
			{
				for (int j = ((Rectangle)(ref area)).Top; j < ((Rectangle)(ref area)).Bottom; j++)
				{
					Vector3 val = _activeLightMap[i - _activeProcessedArea.X, j - _activeProcessedArea.Y];
					float num = Math.Max(Math.Max(val.X, val.Y), val.Z);
					byte light = (byte)Math.Min(255, (int)(num * 255f));
					Main.Map.UpdateLighting(i, j, light);
				}
			}
		});
		Main.updateMap = true;
	}
}
