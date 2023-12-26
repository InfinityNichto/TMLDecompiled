using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Terraria.Graphics;

public class SpriteViewMatrix
{
	private Vector2 _zoom = Vector2.One;

	private Vector2 _translation = Vector2.Zero;

	private Matrix _zoomMatrix = Matrix.Identity;

	private Matrix _transformationMatrix = Matrix.Identity;

	private Matrix _normalizedTransformationMatrix = Matrix.Identity;

	private SpriteEffects _effects;

	private Matrix _effectMatrix;

	private GraphicsDevice _graphicsDevice;

	private Viewport _viewport;

	private bool _overrideSystemViewport;

	private bool _needsRebuild = true;

	public Vector2 Zoom
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return _zoom;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			if (_zoom != value)
			{
				_zoom = value;
				_needsRebuild = true;
			}
		}
	}

	public Vector2 Translation
	{
		get
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			if (ShouldRebuild())
			{
				Rebuild();
			}
			return _translation;
		}
	}

	public Matrix ZoomMatrix
	{
		get
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			if (ShouldRebuild())
			{
				Rebuild();
			}
			return _zoomMatrix;
		}
	}

	public Matrix TransformationMatrix
	{
		get
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			if (ShouldRebuild())
			{
				Rebuild();
			}
			return _transformationMatrix;
		}
	}

	public Matrix NormalizedTransformationmatrix
	{
		get
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			if (ShouldRebuild())
			{
				Rebuild();
			}
			return _normalizedTransformationMatrix;
		}
	}

	public SpriteEffects Effects
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return _effects;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			if (_effects != value)
			{
				_effects = value;
				_needsRebuild = true;
			}
		}
	}

	public Matrix EffectMatrix
	{
		get
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			if (ShouldRebuild())
			{
				Rebuild();
			}
			return _effectMatrix;
		}
	}

	public SpriteViewMatrix(GraphicsDevice graphicsDevice)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		_graphicsDevice = graphicsDevice;
	}

	private void Rebuild()
	{
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Unknown result type (might be due to invalid IL or missing references)
		//IL_0165: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0170: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Unknown result type (might be due to invalid IL or missing references)
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_017e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0183: Unknown result type (might be due to invalid IL or missing references)
		//IL_0193: Unknown result type (might be due to invalid IL or missing references)
		//IL_0198: Unknown result type (might be due to invalid IL or missing references)
		//IL_019b: Unknown result type (might be due to invalid IL or missing references)
		//IL_019e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_01af: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		if (!_overrideSystemViewport)
		{
			_viewport = _graphicsDevice.Viewport;
		}
		Vector2 vector = default(Vector2);
		((Vector2)(ref vector))._002Ector((float)((Viewport)(ref _viewport)).Width, (float)((Viewport)(ref _viewport)).Height);
		Matrix identity = Matrix.Identity;
		if (((Enum)_effects).HasFlag((Enum)(object)(SpriteEffects)1))
		{
			identity *= Matrix.CreateScale(-1f, 1f, 1f) * Matrix.CreateTranslation(vector.X, 0f, 0f);
		}
		if (((Enum)_effects).HasFlag((Enum)(object)(SpriteEffects)2))
		{
			identity *= Matrix.CreateScale(1f, -1f, 1f) * Matrix.CreateTranslation(0f, vector.Y, 0f);
		}
		Vector2 val = vector * 0.5f;
		Vector2 translation = val - val / _zoom;
		Matrix matrix = Matrix.CreateOrthographicOffCenter(0f, vector.X, vector.Y, 0f, 0f, 1f);
		_translation = translation;
		_zoomMatrix = Matrix.CreateTranslation(0f - translation.X, 0f - translation.Y, 0f) * Matrix.CreateScale(_zoom.X, _zoom.Y, 1f);
		_effectMatrix = identity;
		_transformationMatrix = identity * _zoomMatrix;
		Matrix perspectiveFix = Matrix.CreateTranslation(0.00390625f, 0.00390625f, 0f);
		_transformationMatrix = perspectiveFix * _transformationMatrix;
		_normalizedTransformationMatrix = Matrix.Invert(identity) * _zoomMatrix * matrix;
		_needsRebuild = false;
	}

	public void SetViewportOverride(Viewport viewport)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		_viewport = viewport;
		_overrideSystemViewport = true;
	}

	public void ClearViewportOverride()
	{
		_overrideSystemViewport = false;
	}

	private bool ShouldRebuild()
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		if (!_needsRebuild)
		{
			if (!_overrideSystemViewport)
			{
				Viewport viewport = _graphicsDevice.Viewport;
				if (((Viewport)(ref viewport)).Width == ((Viewport)(ref _viewport)).Width)
				{
					viewport = _graphicsDevice.Viewport;
					return ((Viewport)(ref viewport)).Height != ((Viewport)(ref _viewport)).Height;
				}
				return true;
			}
			return false;
		}
		return true;
	}
}
