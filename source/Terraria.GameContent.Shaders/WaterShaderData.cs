using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.DataStructures;
using Terraria.GameContent.Liquid;
using Terraria.Graphics;
using Terraria.Graphics.Light;
using Terraria.Graphics.Shaders;
using Terraria.ID;

namespace Terraria.GameContent.Shaders;

public class WaterShaderData : ScreenShaderData
{
	private struct Ripple
	{
		private static readonly Rectangle[] RIPPLE_SHAPE_SOURCE_RECTS = (Rectangle[])(object)new Rectangle[3]
		{
			new Rectangle(0, 0, 0, 0),
			new Rectangle(1, 1, 62, 62),
			new Rectangle(1, 65, 62, 62)
		};

		public readonly Vector2 Position;

		public readonly Color WaveData;

		public readonly Vector2 Size;

		public readonly RippleShape Shape;

		public readonly float Rotation;

		public Rectangle SourceRectangle => RIPPLE_SHAPE_SOURCE_RECTS[(int)Shape];

		public Ripple(Vector2 position, Color waveData, Vector2 size, RippleShape shape, float rotation)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			Position = position;
			WaveData = waveData;
			Size = size;
			Shape = shape;
			Rotation = rotation;
		}
	}

	private const float DISTORTION_BUFFER_SCALE = 0.25f;

	private const float WAVE_FRAMERATE = 1f / 60f;

	private const int MAX_RIPPLES_QUEUED = 200;

	public bool _useViscosityFilter = true;

	private RenderTarget2D _distortionTarget;

	private RenderTarget2D _distortionTargetSwap;

	private bool _usingRenderTargets;

	private Vector2 _lastDistortionDrawOffset = Vector2.Zero;

	private float _progress;

	private Ripple[] _rippleQueue = new Ripple[200];

	private int _rippleQueueCount;

	private int _lastScreenWidth;

	private int _lastScreenHeight;

	public bool _useProjectileWaves = true;

	private bool _useNPCWaves = true;

	private bool _usePlayerWaves = true;

	private bool _useRippleWaves = true;

	private bool _useCustomWaves = true;

	private bool _clearNextFrame = true;

	private Texture2D[] _viscosityMaskChain = (Texture2D[])(object)new Texture2D[3];

	private int _activeViscosityMask;

	private Asset<Texture2D> _rippleShapeTexture;

	private bool _isWaveBufferDirty = true;

	private int _queuedSteps;

	private const int MAX_QUEUED_STEPS = 2;

	public event Action<TileBatch> OnWaveDraw;

	public WaterShaderData(string passName)
		: base(passName)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		Main.OnRenderTargetsInitialized += InitRenderTargets;
		Main.OnRenderTargetsReleased += ReleaseRenderTargets;
		_rippleShapeTexture = Main.Assets.Request<Texture2D>("Images/Misc/Ripples");
		Main.OnPreDraw += PreDraw;
	}

	public override void Update(GameTime gameTime)
	{
		_useViscosityFilter = Main.WaveQuality >= 3;
		_useProjectileWaves = Main.WaveQuality >= 3;
		_usePlayerWaves = Main.WaveQuality >= 2;
		_useRippleWaves = Main.WaveQuality >= 2;
		_useCustomWaves = Main.WaveQuality >= 2;
		if (!Main.gamePaused && Main.hasFocus)
		{
			_progress += (float)gameTime.ElapsedGameTime.TotalSeconds * base.Intensity * 0.75f;
			_progress %= 86400f;
			if (_useProjectileWaves || _useRippleWaves || _useCustomWaves || _usePlayerWaves)
			{
				_queuedSteps++;
			}
			base.Update(gameTime);
		}
	}

	private void StepLiquids()
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_0158: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0204: Unknown result type (might be due to invalid IL or missing references)
		//IL_0211: Unknown result type (might be due to invalid IL or missing references)
		//IL_0218: Unknown result type (might be due to invalid IL or missing references)
		//IL_021d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0228: Unknown result type (might be due to invalid IL or missing references)
		//IL_0239: Unknown result type (might be due to invalid IL or missing references)
		//IL_0241: Unknown result type (might be due to invalid IL or missing references)
		//IL_0248: Unknown result type (might be due to invalid IL or missing references)
		//IL_024d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0258: Unknown result type (might be due to invalid IL or missing references)
		//IL_0269: Unknown result type (might be due to invalid IL or missing references)
		//IL_0290: Unknown result type (might be due to invalid IL or missing references)
		//IL_0292: Unknown result type (might be due to invalid IL or missing references)
		//IL_0299: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a5: Unknown result type (might be due to invalid IL or missing references)
		_isWaveBufferDirty = true;
		Vector2 vector = (Vector2)(Main.drawToScreen ? Vector2.Zero : new Vector2((float)Main.offScreenRange, (float)Main.offScreenRange));
		Vector2 vector2 = vector - Main.screenPosition;
		TileBatch tileBatch = Main.tileBatch;
		GraphicsDevice graphicsDevice = ((Game)Main.instance).GraphicsDevice;
		graphicsDevice.SetRenderTarget(_distortionTarget);
		if (_clearNextFrame)
		{
			graphicsDevice.Clear(new Color(0.5f, 0.5f, 0f, 1f));
			_clearNextFrame = false;
		}
		DrawWaves();
		graphicsDevice.SetRenderTarget(_distortionTargetSwap);
		graphicsDevice.Clear(new Color(0.5f, 0.5f, 0.5f, 1f));
		Main.tileBatch.Begin();
		vector2 *= 0.25f;
		vector2.X = (float)Math.Floor(vector2.X);
		vector2.Y = (float)Math.Floor(vector2.Y);
		Vector2 vector3 = vector2 - _lastDistortionDrawOffset;
		_lastDistortionDrawOffset = vector2;
		tileBatch.Draw((Texture2D)(object)_distortionTarget, new Vector4(vector3.X, vector3.Y, (float)((Texture2D)_distortionTarget).Width, (float)((Texture2D)_distortionTarget).Height), new VertexColors(Color.White));
		GameShaders.Misc["WaterProcessor"].Apply(new DrawData((Texture2D)(object)_distortionTarget, Vector2.Zero, Color.White));
		tileBatch.End();
		RenderTarget2D distortionTarget = _distortionTarget;
		_distortionTarget = _distortionTargetSwap;
		_distortionTargetSwap = distortionTarget;
		if (_useViscosityFilter)
		{
			LiquidRenderer.Instance.SetWaveMaskData(ref _viscosityMaskChain[_activeViscosityMask]);
			tileBatch.Begin();
			Rectangle cachedDrawArea = LiquidRenderer.Instance.GetCachedDrawArea();
			Rectangle rectangle = default(Rectangle);
			((Rectangle)(ref rectangle))._002Ector(0, 0, cachedDrawArea.Height, cachedDrawArea.Width);
			Vector4 destination = default(Vector4);
			((Vector4)(ref destination))._002Ector((float)(cachedDrawArea.X + cachedDrawArea.Width), (float)cachedDrawArea.Y, (float)cachedDrawArea.Height, (float)cachedDrawArea.Width);
			destination *= 16f;
			destination.X -= vector.X;
			destination.Y -= vector.Y;
			destination *= 0.25f;
			destination.X += vector2.X;
			destination.Y += vector2.Y;
			graphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
			tileBatch.Draw(_viscosityMaskChain[_activeViscosityMask], destination, rectangle, new VertexColors(Color.White), rectangle.Size(), (SpriteEffects)1, -(float)Math.PI / 2f);
			tileBatch.End();
			_activeViscosityMask++;
			_activeViscosityMask %= _viscosityMaskChain.Length;
		}
		graphicsDevice.SetRenderTarget((RenderTarget2D)null);
	}

	private void DrawWaves()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_08bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_08c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_08c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_08c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_08db: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_0901: Unknown result type (might be due to invalid IL or missing references)
		//IL_0908: Unknown result type (might be due to invalid IL or missing references)
		//IL_090f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0916: Unknown result type (might be due to invalid IL or missing references)
		//IL_091d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0927: Unknown result type (might be due to invalid IL or missing references)
		//IL_092c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0940: Unknown result type (might be due to invalid IL or missing references)
		//IL_094a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0954: Unknown result type (might be due to invalid IL or missing references)
		//IL_095e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_0441: Unknown result type (might be due to invalid IL or missing references)
		//IL_0442: Unknown result type (might be due to invalid IL or missing references)
		//IL_044c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0451: Unknown result type (might be due to invalid IL or missing references)
		//IL_0453: Unknown result type (might be due to invalid IL or missing references)
		//IL_0460: Unknown result type (might be due to invalid IL or missing references)
		//IL_0465: Unknown result type (might be due to invalid IL or missing references)
		//IL_0467: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0170: Unknown result type (might be due to invalid IL or missing references)
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0204: Unknown result type (might be due to invalid IL or missing references)
		//IL_0209: Unknown result type (might be due to invalid IL or missing references)
		//IL_020e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0482: Unknown result type (might be due to invalid IL or missing references)
		//IL_0487: Unknown result type (might be due to invalid IL or missing references)
		//IL_0488: Unknown result type (might be due to invalid IL or missing references)
		//IL_048d: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04be: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_024b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0252: Unknown result type (might be due to invalid IL or missing references)
		//IL_0275: Unknown result type (might be due to invalid IL or missing references)
		//IL_027f: Unknown result type (might be due to invalid IL or missing references)
		//IL_028e: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_050e: Unknown result type (might be due to invalid IL or missing references)
		//IL_052a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0562: Unknown result type (might be due to invalid IL or missing references)
		//IL_056c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0571: Unknown result type (might be due to invalid IL or missing references)
		//IL_0584: Unknown result type (might be due to invalid IL or missing references)
		//IL_059f: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_06fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0702: Unknown result type (might be due to invalid IL or missing references)
		//IL_0704: Unknown result type (might be due to invalid IL or missing references)
		//IL_070b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0710: Unknown result type (might be due to invalid IL or missing references)
		//IL_0712: Unknown result type (might be due to invalid IL or missing references)
		//IL_033f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0344: Unknown result type (might be due to invalid IL or missing references)
		//IL_0348: Unknown result type (might be due to invalid IL or missing references)
		//IL_034d: Unknown result type (might be due to invalid IL or missing references)
		//IL_05de: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_0774: Unknown result type (might be due to invalid IL or missing references)
		//IL_0779: Unknown result type (might be due to invalid IL or missing references)
		//IL_077a: Unknown result type (might be due to invalid IL or missing references)
		//IL_077f: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_072c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0746: Unknown result type (might be due to invalid IL or missing references)
		//IL_0382: Unknown result type (might be due to invalid IL or missing references)
		//IL_038c: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_080e: Unknown result type (might be due to invalid IL or missing references)
		//IL_081e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0832: Unknown result type (might be due to invalid IL or missing references)
		//IL_083c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0841: Unknown result type (might be due to invalid IL or missing references)
		//IL_0854: Unknown result type (might be due to invalid IL or missing references)
		//IL_086f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0625: Unknown result type (might be due to invalid IL or missing references)
		//IL_0641: Unknown result type (might be due to invalid IL or missing references)
		Vector2 screenPosition = Main.screenPosition;
		Vector2 vector = (Vector2)(Main.drawToScreen ? Vector2.Zero : new Vector2((float)Main.offScreenRange, (float)Main.offScreenRange));
		Vector2 vector2 = -_lastDistortionDrawOffset / 0.25f + vector;
		TileBatch tileBatch = Main.tileBatch;
		_ = ((Game)Main.instance).GraphicsDevice;
		Vector2 dimensions = default(Vector2);
		((Vector2)(ref dimensions))._002Ector((float)Main.screenWidth, (float)Main.screenHeight);
		Vector2 vector3 = default(Vector2);
		((Vector2)(ref vector3))._002Ector(16f, 16f);
		tileBatch.Begin();
		GameShaders.Misc["WaterDistortionObject"].Apply();
		if (_useNPCWaves)
		{
			for (int i = 0; i < 200; i++)
			{
				if (Main.npc[i] == null || !Main.npc[i].active || (!Main.npc[i].wet && Main.npc[i].wetCount == 0) || !Collision.CheckAABBvAABBCollision(screenPosition, dimensions, Main.npc[i].position - vector3, Main.npc[i].Size + vector3))
				{
					continue;
				}
				NPC nPC = Main.npc[i];
				Vector2 vector4 = nPC.Center - vector2;
				Vector2 velocity4 = nPC.velocity;
				double radians = 0f - nPC.rotation;
				Vector2 center = default(Vector2);
				Vector2 vector5 = velocity4.RotatedBy(radians, center) / new Vector2((float)nPC.height, (float)nPC.width);
				float num = ((Vector2)(ref vector5)).LengthSquared();
				num = num * 0.3f + 0.7f * num * (1024f / (float)(nPC.height * nPC.width));
				num = Math.Min(num, 0.08f);
				float num8 = num;
				center = nPC.velocity - nPC.oldVelocity;
				num = num8 + ((Vector2)(ref center)).Length() * 0.5f;
				((Vector2)(ref vector5)).Normalize();
				Vector2 velocity = nPC.velocity;
				((Vector2)(ref velocity)).Normalize();
				vector4 -= velocity * 10f;
				if (!_useViscosityFilter && (nPC.honeyWet || nPC.lavaWet))
				{
					num *= 0.3f;
				}
				if (nPC.wet)
				{
					tileBatch.Draw(TextureAssets.MagicPixel.Value, new Vector4(vector4.X, vector4.Y, (float)nPC.width * 2f, (float)nPC.height * 2f) * 0.25f, null, new VertexColors(new Color(vector5.X * 0.5f + 0.5f, vector5.Y * 0.5f + 0.5f, 0.5f * num)), new Vector2((float)TextureAssets.MagicPixel.Width() / 2f, (float)TextureAssets.MagicPixel.Height() / 2f), (SpriteEffects)0, nPC.rotation);
				}
				if (nPC.wetCount != 0)
				{
					num = ((Vector2)(ref nPC.velocity)).Length();
					num = 0.195f * (float)Math.Sqrt(num);
					float num2 = 5f;
					if (!nPC.wet)
					{
						num2 = -20f;
					}
					QueueRipple(nPC.Center + velocity * num2, new Color(0.5f, (nPC.wet ? num : (0f - num)) * 0.5f + 0.5f, 0f, 1f) * 0.5f, new Vector2((float)nPC.width, (float)nPC.height * ((float)(int)nPC.wetCount / 9f)) * MathHelper.Clamp(num * 10f, 0f, 1f), RippleShape.Circle);
				}
			}
		}
		if (_usePlayerWaves)
		{
			for (int j = 0; j < 255; j++)
			{
				if (Main.player[j] == null || !Main.player[j].active || (!Main.player[j].wet && Main.player[j].wetCount == 0) || !Collision.CheckAABBvAABBCollision(screenPosition, dimensions, Main.player[j].position - vector3, Main.player[j].Size + vector3))
				{
					continue;
				}
				Player player = Main.player[j];
				Vector2 vector6 = player.Center - vector2;
				float num3 = ((Vector2)(ref player.velocity)).Length();
				num3 = 0.05f * (float)Math.Sqrt(num3);
				Vector2 velocity2 = player.velocity;
				((Vector2)(ref velocity2)).Normalize();
				vector6 -= velocity2 * 10f;
				if (!_useViscosityFilter && (player.honeyWet || player.lavaWet))
				{
					num3 *= 0.3f;
				}
				if (player.wet)
				{
					tileBatch.Draw(TextureAssets.MagicPixel.Value, new Vector4(vector6.X - (float)player.width * 2f * 0.5f, vector6.Y - (float)player.height * 2f * 0.5f, (float)player.width * 2f, (float)player.height * 2f) * 0.25f, new VertexColors(new Color(velocity2.X * 0.5f + 0.5f, velocity2.Y * 0.5f + 0.5f, 0.5f * num3)));
				}
				if (player.wetCount != 0)
				{
					float num4 = 5f;
					if (!player.wet)
					{
						num4 = -20f;
					}
					num3 *= 3f;
					QueueRipple(player.Center + velocity2 * num4, player.wet ? num3 : (0f - num3), new Vector2((float)player.width, (float)player.height * ((float)(int)player.wetCount / 9f)) * MathHelper.Clamp(num3 * 10f, 0f, 1f), RippleShape.Circle);
				}
			}
		}
		if (_useProjectileWaves)
		{
			for (int k = 0; k < 1000; k++)
			{
				Projectile projectile = Main.projectile[k];
				if (projectile.wet && !projectile.lavaWet)
				{
					_ = projectile.honeyWet;
				}
				bool flag = projectile.lavaWet;
				bool flag2 = projectile.honeyWet;
				bool flag3 = projectile.wet;
				if (projectile.ignoreWater)
				{
					flag3 = true;
				}
				if (!(projectile != null && projectile.active && ProjectileID.Sets.CanDistortWater[projectile.type] && flag3) || ProjectileID.Sets.NoLiquidDistortion[projectile.type] || !Collision.CheckAABBvAABBCollision(screenPosition, dimensions, projectile.position - vector3, projectile.Size + vector3))
				{
					continue;
				}
				if (projectile.ignoreWater)
				{
					bool num9 = Collision.LavaCollision(projectile.position, projectile.width, projectile.height);
					flag = Collision.WetCollision(projectile.position, projectile.width, projectile.height);
					flag2 = Collision.honey;
					if (!(num9 || flag || flag2))
					{
						continue;
					}
				}
				Vector2 vector7 = projectile.Center - vector2;
				float num5 = ((Vector2)(ref projectile.velocity)).Length();
				num5 = 2f * (float)Math.Sqrt(0.05f * num5);
				Vector2 velocity3 = projectile.velocity;
				((Vector2)(ref velocity3)).Normalize();
				if (!_useViscosityFilter && (flag2 || flag))
				{
					num5 *= 0.3f;
				}
				float num6 = Math.Max(12f, (float)projectile.width * 0.75f);
				float num7 = Math.Max(12f, (float)projectile.height * 0.75f);
				tileBatch.Draw(TextureAssets.MagicPixel.Value, new Vector4(vector7.X - num6 * 0.5f, vector7.Y - num7 * 0.5f, num6, num7) * 0.25f, new VertexColors(new Color(velocity3.X * 0.5f + 0.5f, velocity3.Y * 0.5f + 0.5f, num5 * 0.5f)));
			}
		}
		tileBatch.End();
		if (_useRippleWaves)
		{
			tileBatch.Begin();
			for (int l = 0; l < _rippleQueueCount; l++)
			{
				Vector2 vector8 = _rippleQueue[l].Position - vector2;
				Vector2 size = _rippleQueue[l].Size;
				Rectangle sourceRectangle = _rippleQueue[l].SourceRectangle;
				Texture2D value = _rippleShapeTexture.Value;
				tileBatch.Draw(value, new Vector4(vector8.X, vector8.Y, size.X, size.Y) * 0.25f, sourceRectangle, new VertexColors(_rippleQueue[l].WaveData), new Vector2((float)(sourceRectangle.Width / 2), (float)(sourceRectangle.Height / 2)), (SpriteEffects)0, _rippleQueue[l].Rotation);
			}
			tileBatch.End();
		}
		_rippleQueueCount = 0;
		if (_useCustomWaves && this.OnWaveDraw != null)
		{
			tileBatch.Begin();
			this.OnWaveDraw(tileBatch);
			tileBatch.End();
		}
	}

	private void PreDraw(GameTime gameTime)
	{
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		ValidateRenderTargets();
		if (!_usingRenderTargets || !Main.IsGraphicsDeviceAvailable)
		{
			return;
		}
		if (_useProjectileWaves || _useRippleWaves || _useCustomWaves || _usePlayerWaves)
		{
			for (int i = 0; i < Math.Min(_queuedSteps, 2); i++)
			{
				StepLiquids();
			}
		}
		else if (_isWaveBufferDirty || _clearNextFrame)
		{
			GraphicsDevice graphicsDevice = ((Game)Main.instance).GraphicsDevice;
			graphicsDevice.SetRenderTarget(_distortionTarget);
			graphicsDevice.Clear(new Color(0.5f, 0.5f, 0f, 1f));
			_clearNextFrame = false;
			_isWaveBufferDirty = false;
			graphicsDevice.SetRenderTarget((RenderTarget2D)null);
		}
		_queuedSteps = 0;
	}

	public override void Apply()
	{
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		if (_usingRenderTargets && Main.IsGraphicsDeviceAvailable)
		{
			UseProgress(_progress);
			Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
			Vector2 vector = new Vector2((float)Main.screenWidth, (float)Main.screenHeight) * 0.5f * (Vector2.One - Vector2.One / Main.GameViewMatrix.Zoom);
			Vector2 vector2 = (Vector2)(Main.drawToScreen ? Vector2.Zero : new Vector2((float)Main.offScreenRange, (float)Main.offScreenRange)) - Main.screenPosition - vector;
			UseImage((Texture2D)(object)_distortionTarget, 1);
			UseImage((Texture2D)(object)Main.waterTarget, 2, SamplerState.PointClamp);
			UseTargetPosition(Main.screenPosition - Main.sceneWaterPos + new Vector2((float)Main.offScreenRange, (float)Main.offScreenRange) + vector);
			UseImageOffset(-(vector2 * 0.25f - _lastDistortionDrawOffset) / new Vector2((float)((Texture2D)_distortionTarget).Width, (float)((Texture2D)_distortionTarget).Height));
			base.Apply();
		}
	}

	private void ValidateRenderTargets()
	{
		int backBufferWidth = ((Game)Main.instance).GraphicsDevice.PresentationParameters.BackBufferWidth;
		int backBufferHeight = ((Game)Main.instance).GraphicsDevice.PresentationParameters.BackBufferHeight;
		bool flag = !Main.drawToScreen;
		if (_usingRenderTargets && !flag)
		{
			ReleaseRenderTargets();
		}
		else if (!_usingRenderTargets && flag)
		{
			InitRenderTargets(backBufferWidth, backBufferHeight);
		}
		else if (_usingRenderTargets && flag && (_distortionTarget.IsContentLost || _distortionTargetSwap.IsContentLost))
		{
			_clearNextFrame = true;
		}
	}

	private void InitRenderTargets(int width, int height)
	{
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Expected O, but got Unknown
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Expected O, but got Unknown
		_lastScreenWidth = width;
		_lastScreenHeight = height;
		width = (int)((float)width * 0.25f);
		height = (int)((float)height * 0.25f);
		try
		{
			_distortionTarget = new RenderTarget2D(((Game)Main.instance).GraphicsDevice, width, height, false, (SurfaceFormat)0, (DepthFormat)0, 0, (RenderTargetUsage)1);
			_distortionTargetSwap = new RenderTarget2D(((Game)Main.instance).GraphicsDevice, width, height, false, (SurfaceFormat)0, (DepthFormat)0, 0, (RenderTargetUsage)1);
			_usingRenderTargets = true;
			_clearNextFrame = true;
		}
		catch (Exception ex)
		{
			Lighting.Mode = LightMode.Retro;
			_usingRenderTargets = false;
			Console.WriteLine("Failed to create water distortion render targets. " + ex);
		}
	}

	private void ReleaseRenderTargets()
	{
		try
		{
			if (_distortionTarget != null)
			{
				((GraphicsResource)_distortionTarget).Dispose();
			}
			if (_distortionTargetSwap != null)
			{
				((GraphicsResource)_distortionTargetSwap).Dispose();
			}
		}
		catch (Exception ex)
		{
			Console.WriteLine("Error disposing of water distortion render targets. " + ex);
		}
		_distortionTarget = null;
		_distortionTargetSwap = null;
		_usingRenderTargets = false;
	}

	public void QueueRipple(Vector2 position, float strength = 1f, RippleShape shape = RippleShape.Square, float rotation = 0f)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		float g = strength * 0.5f + 0.5f;
		float num = Math.Min(Math.Abs(strength), 1f);
		QueueRipple(position, new Color(0.5f, g, 0f, 1f) * num, new Vector2(4f * Math.Max(Math.Abs(strength), 1f)), shape, rotation);
	}

	public void QueueRipple(Vector2 position, float strength, Vector2 size, RippleShape shape = RippleShape.Square, float rotation = 0f)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		float g = strength * 0.5f + 0.5f;
		float num = Math.Min(Math.Abs(strength), 1f);
		QueueRipple(position, new Color(0.5f, g, 0f, 1f) * num, size, shape, rotation);
	}

	public void QueueRipple(Vector2 position, Color waveData, Vector2 size, RippleShape shape = RippleShape.Square, float rotation = 0f)
	{
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		if (!_useRippleWaves || Main.drawToScreen)
		{
			_rippleQueueCount = 0;
		}
		else if (_rippleQueueCount < _rippleQueue.Length)
		{
			_rippleQueue[_rippleQueueCount++] = new Ripple(position, waveData, size, shape, rotation);
		}
	}
}
