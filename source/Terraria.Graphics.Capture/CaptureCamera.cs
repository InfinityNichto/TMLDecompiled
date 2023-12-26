using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent.Drawing;
using Terraria.Graphics.Effects;
using Terraria.Localization;
using Terraria.Utilities;

namespace Terraria.Graphics.Capture;

internal class CaptureCamera : IDisposable
{
	private enum ImageFormat
	{
		Png
	}

	private class CaptureChunk
	{
		public readonly Rectangle Area;

		public readonly Rectangle ScaledArea;

		public CaptureChunk(Rectangle area, Rectangle scaledArea)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			Area = area;
			ScaledArea = scaledArea;
		}
	}

	private static bool CameraExists;

	public const int CHUNK_SIZE = 128;

	public const int FRAMEBUFFER_PIXEL_SIZE = 2048;

	public const int INNER_CHUNK_SIZE = 126;

	public const int MAX_IMAGE_SIZE = 4096;

	public const string CAPTURE_DIRECTORY = "Captures";

	private RenderTarget2D _frameBuffer;

	private RenderTarget2D _scaledFrameBuffer;

	private RenderTarget2D _filterFrameBuffer1;

	private RenderTarget2D _filterFrameBuffer2;

	private GraphicsDevice _graphics;

	private readonly object _captureLock = new object();

	private bool _isDisposed;

	private CaptureSettings _activeSettings;

	private Queue<CaptureChunk> _renderQueue = new Queue<CaptureChunk>();

	private SpriteBatch _spriteBatch;

	private byte[] _scaledFrameData;

	private byte[] _outputData;

	private Size _outputImageSize;

	private SamplerState _downscaleSampleState;

	private float _tilesProcessed;

	private float _totalTiles;

	public bool IsCapturing
	{
		get
		{
			Monitor.Enter(_captureLock);
			bool result = _activeSettings != null;
			Monitor.Exit(_captureLock);
			return result;
		}
	}

	public CaptureCamera(GraphicsDevice graphics)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Expected O, but got Unknown
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Expected O, but got Unknown
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Expected O, but got Unknown
		CameraExists = true;
		_graphics = graphics;
		_spriteBatch = new SpriteBatch(graphics);
		try
		{
			_frameBuffer = new RenderTarget2D(graphics, 2048, 2048, false, graphics.PresentationParameters.BackBufferFormat, (DepthFormat)0);
			_filterFrameBuffer1 = new RenderTarget2D(graphics, 2048, 2048, false, graphics.PresentationParameters.BackBufferFormat, (DepthFormat)0);
			_filterFrameBuffer2 = new RenderTarget2D(graphics, 2048, 2048, false, graphics.PresentationParameters.BackBufferFormat, (DepthFormat)0);
		}
		catch
		{
			Main.CaptureModeDisabled = true;
			return;
		}
		_downscaleSampleState = SamplerState.AnisotropicClamp;
	}

	public void Capture(CaptureSettings settings)
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0262: Unknown result type (might be due to invalid IL or missing references)
		//IL_0268: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Expected O, but got Unknown
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0183: Unknown result type (might be due to invalid IL or missing references)
		//IL_024a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0250: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_019b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0203: Unknown result type (might be due to invalid IL or missing references)
		//IL_0225: Unknown result type (might be due to invalid IL or missing references)
		//IL_0232: Unknown result type (might be due to invalid IL or missing references)
		Main.GlobalTimerPaused = true;
		Monitor.Enter(_captureLock);
		if (_activeSettings != null)
		{
			throw new InvalidOperationException("Capture called while another capture was already active.");
		}
		_activeSettings = settings;
		Rectangle area = settings.Area;
		float num = 1f;
		if (settings.UseScaling)
		{
			if (area.Width * 16 > 4096)
			{
				num = 4096f / (float)(area.Width * 16);
			}
			if (area.Height * 16 > 4096)
			{
				num = Math.Min(num, 4096f / (float)(area.Height * 16));
			}
			num = Math.Min(1f, num);
			_outputImageSize = new Size((int)MathHelper.Clamp((float)(int)(num * (float)(area.Width * 16)), 1f, 4096f), (int)MathHelper.Clamp((float)(int)(num * (float)(area.Height * 16)), 1f, 4096f));
			_outputData = new byte[4 * _outputImageSize.Width * _outputImageSize.Height];
			int num2 = (int)Math.Floor(num * 2048f);
			_scaledFrameData = new byte[4 * num2 * num2];
			_scaledFrameBuffer = new RenderTarget2D(_graphics, num2, num2, false, _graphics.PresentationParameters.BackBufferFormat, (DepthFormat)0);
		}
		else
		{
			_outputData = new byte[16777216];
		}
		_tilesProcessed = 0f;
		_totalTiles = area.Width * area.Height;
		for (int i = area.X; i < area.X + area.Width; i += 126)
		{
			for (int j = area.Y; j < area.Y + area.Height; j += 126)
			{
				int num3 = Math.Min(128, area.X + area.Width - i);
				int num4 = Math.Min(128, area.Y + area.Height - j);
				int width = (int)Math.Floor(num * (float)(num3 * 16));
				int height = (int)Math.Floor(num * (float)(num4 * 16));
				int x = (int)Math.Floor(num * (float)((i - area.X) * 16));
				int y = (int)Math.Floor(num * (float)((j - area.Y) * 16));
				_renderQueue.Enqueue(new CaptureChunk(new Rectangle(i, j, num3, num4), new Rectangle(x, y, width, height)));
			}
		}
		Monitor.Exit(_captureLock);
	}

	public void DrawTick()
	{
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_022f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		Monitor.Enter(_captureLock);
		if (_activeSettings == null)
		{
			return;
		}
		bool notRetro = Lighting.NotRetro;
		if (_renderQueue.Count > 0)
		{
			CaptureChunk captureChunk = _renderQueue.Dequeue();
			_graphics.SetRenderTarget((RenderTarget2D)null);
			_graphics.Clear(Color.Transparent);
			TileDrawing tilesRenderer = Main.instance.TilesRenderer;
			Rectangle area = captureChunk.Area;
			int left = ((Rectangle)(ref area)).Left;
			area = captureChunk.Area;
			int right = ((Rectangle)(ref area)).Right;
			area = captureChunk.Area;
			int top = ((Rectangle)(ref area)).Top;
			area = captureChunk.Area;
			tilesRenderer.PrepareForAreaDrawing(left, right, top, ((Rectangle)(ref area)).Bottom, prepareLazily: false);
			Main.instance.TilePaintSystem.PrepareAllRequests();
			_graphics.SetRenderTarget(_frameBuffer);
			_graphics.Clear(Color.Transparent);
			if (notRetro)
			{
				Color clearColor = (_activeSettings.CaptureBackground ? Color.Black : Color.Transparent);
				Filters.Scene.BeginCapture(_filterFrameBuffer1, clearColor);
				Main.instance.DrawCapture(captureChunk.Area, _activeSettings);
				Filters.Scene.EndCapture(_frameBuffer, _filterFrameBuffer1, _filterFrameBuffer2, clearColor);
			}
			else
			{
				Main.instance.DrawCapture(captureChunk.Area, _activeSettings);
			}
			if (_activeSettings.UseScaling)
			{
				_graphics.SetRenderTarget(_scaledFrameBuffer);
				_graphics.Clear(Color.Transparent);
				_spriteBatch.Begin((SpriteSortMode)0, BlendState.AlphaBlend, _downscaleSampleState, DepthStencilState.Default, RasterizerState.CullNone);
				_spriteBatch.Draw((Texture2D)(object)_frameBuffer, new Rectangle(0, 0, ((Texture2D)_scaledFrameBuffer).Width, ((Texture2D)_scaledFrameBuffer).Height), Color.White);
				_spriteBatch.End();
				_graphics.SetRenderTarget((RenderTarget2D)null);
				((Texture2D)_scaledFrameBuffer).GetData<byte>(_scaledFrameData, 0, ((Texture2D)_scaledFrameBuffer).Width * ((Texture2D)_scaledFrameBuffer).Height * 4);
				DrawBytesToBuffer(_scaledFrameData, _outputData, ((Texture2D)_scaledFrameBuffer).Width, _outputImageSize.Width, captureChunk.ScaledArea);
			}
			else
			{
				_graphics.SetRenderTarget((RenderTarget2D)null);
				SaveImage((Texture2D)(object)_frameBuffer, captureChunk.ScaledArea.Width, captureChunk.ScaledArea.Height, ImageFormat.Png, _activeSettings.OutputName, captureChunk.Area.X + "-" + captureChunk.Area.Y + ".png");
			}
			_tilesProcessed += captureChunk.Area.Width * captureChunk.Area.Height;
		}
		if (_renderQueue.Count == 0)
		{
			FinishCapture();
		}
		Monitor.Exit(_captureLock);
	}

	private unsafe void DrawBytesToBuffer(byte[] sourceBuffer, byte[] destinationBuffer, int sourceBufferWidth, int destinationBufferWidth, Rectangle area)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		fixed (byte* ptr5 = &destinationBuffer[0])
		{
			fixed (byte* ptr4 = &sourceBuffer[0])
			{
				byte* ptr2 = ptr4;
				byte* ptr3 = ptr5 + (destinationBufferWidth * area.Y + area.X << 2);
				for (int i = 0; i < area.Height; i++)
				{
					for (int j = 0; j < area.Width; j++)
					{
						if (Program.IsXna)
						{
							ptr3[2] = *ptr2;
							ptr3[1] = ptr2[1];
							*ptr3 = ptr2[2];
							ptr3[3] = ptr2[3];
						}
						else
						{
							*ptr3 = *ptr2;
							ptr3[1] = ptr2[1];
							ptr3[2] = ptr2[2];
							ptr3[3] = ptr2[3];
						}
						ptr2 += 4;
						ptr3 += 4;
					}
					ptr2 += sourceBufferWidth - area.Width << 2;
					ptr3 += destinationBufferWidth - area.Width << 2;
				}
			}
		}
	}

	public float GetProgress()
	{
		return _tilesProcessed / _totalTiles;
	}

	private bool SaveImage(int width, int height, ImageFormat imageFormat, string filename)
	{
		if (!Utils.TryCreatingDirectory(Main.SavePath + Path.DirectorySeparatorChar + "Captures" + Path.DirectorySeparatorChar))
		{
			return false;
		}
		try
		{
			using FileStream stream = File.Create(filename);
			PlatformUtilities.SavePng(stream, width, height, width, height, _outputData);
			return true;
		}
		catch (Exception value)
		{
			Console.WriteLine(value);
			return false;
		}
	}

	private void SaveImage(Texture2D texture, int width, int height, ImageFormat imageFormat, string foldername, string filename)
	{
		string text3 = Main.SavePath + Path.DirectorySeparatorChar + "Captures" + Path.DirectorySeparatorChar + foldername;
		string text2 = Path.Combine(text3, filename);
		if (!Utils.TryCreatingDirectory(text3))
		{
			return;
		}
		int elementCount = texture.Width * texture.Height * 4;
		texture.GetData<byte>(_outputData, 0, elementCount);
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < height; i++)
		{
			for (int j = 0; j < width; j++)
			{
				_outputData[num2] = _outputData[num];
				_outputData[num2 + 1] = _outputData[num + 1];
				_outputData[num2 + 2] = _outputData[num + 2];
				_outputData[num2 + 3] = _outputData[num + 3];
				num += 4;
				num2 += 4;
			}
			num += texture.Width - width << 2;
		}
		using FileStream stream = File.Create(text2);
		PlatformUtilities.SavePng(stream, width, height, width, height, _outputData);
	}

	private void FinishCapture()
	{
		if (_activeSettings.UseScaling)
		{
			int num = 0;
			while (!SaveImage(_outputImageSize.Width, _outputImageSize.Height, ImageFormat.Png, Main.SavePath + Path.DirectorySeparatorChar + "Captures" + Path.DirectorySeparatorChar + _activeSettings.OutputName + ".png"))
			{
				GC.Collect();
				Thread.Sleep(5);
				num++;
				Console.WriteLine(Language.GetTextValue("Error.CaptureError"));
				if (num > 5)
				{
					Console.WriteLine(Language.GetTextValue("Error.UnableToCapture"));
					break;
				}
			}
		}
		_outputData = null;
		_scaledFrameData = null;
		Main.GlobalTimerPaused = false;
		CaptureInterface.EndCamera();
		if (_scaledFrameBuffer != null)
		{
			((GraphicsResource)_scaledFrameBuffer).Dispose();
			_scaledFrameBuffer = null;
		}
		_activeSettings = null;
	}

	public void Dispose()
	{
		if (Main.dedServ)
		{
			return;
		}
		Monitor.Enter(_captureLock);
		if (_isDisposed)
		{
			Monitor.Exit(_captureLock);
			return;
		}
		((GraphicsResource)_frameBuffer).Dispose();
		((GraphicsResource)_filterFrameBuffer1).Dispose();
		((GraphicsResource)_filterFrameBuffer2).Dispose();
		if (_scaledFrameBuffer != null)
		{
			((GraphicsResource)_scaledFrameBuffer).Dispose();
			_scaledFrameBuffer = null;
		}
		CameraExists = false;
		_isDisposed = true;
		Monitor.Exit(_captureLock);
	}
}
