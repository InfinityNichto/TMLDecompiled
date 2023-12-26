using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.Graphics.Effects;

namespace Terraria.ModLoader;

[Autoload(true, Side = ModSide.Client)]
public class SurfaceBackgroundStylesLoader : SceneEffectLoader<ModSurfaceBackgroundStyle>
{
	internal static bool loaded;

	public SurfaceBackgroundStylesLoader()
	{
		Initialize(14);
	}

	internal override void ResizeArrays()
	{
		Array.Resize(ref Main.bgAlphaFrontLayer, base.TotalCount);
		Array.Resize(ref Main.bgAlphaFarBackLayer, base.TotalCount);
		loaded = true;
	}

	internal override void Unload()
	{
		base.Unload();
		loaded = false;
	}

	public override void ChooseStyle(out int style, out SceneEffectPriority priority)
	{
		priority = SceneEffectPriority.None;
		style = -1;
		if (loaded && GlobalBackgroundStyleLoader.loaded)
		{
			int playerSurfaceBackground = Main.LocalPlayer.CurrentSceneEffect.surfaceBackground.value;
			if (playerSurfaceBackground >= base.VanillaCount)
			{
				style = playerSurfaceBackground;
				priority = Main.LocalPlayer.CurrentSceneEffect.surfaceBackground.priority;
			}
		}
	}

	public void ModifyFarFades(int style, float[] fades, float transitionSpeed)
	{
		if (GlobalBackgroundStyleLoader.loaded)
		{
			Get(style)?.ModifyFarFades(fades, transitionSpeed);
			Action<int, float[], float>[] hookModifyFarSurfaceFades = GlobalBackgroundStyleLoader.HookModifyFarSurfaceFades;
			for (int i = 0; i < hookModifyFarSurfaceFades.Length; i++)
			{
				hookModifyFarSurfaceFades[i](style, fades, transitionSpeed);
			}
		}
	}

	public void DrawFarTexture()
	{
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		if (!GlobalBackgroundStyleLoader.loaded || MenuLoader.loading || base.TotalCount != Main.bgAlphaFarBackLayer.Length)
		{
			return;
		}
		foreach (ModSurfaceBackgroundStyle style in list)
		{
			int slot = style.Slot;
			float alpha = Main.bgAlphaFarBackLayer[slot];
			Main.ColorOfSurfaceBackgroundsModified = Main.ColorOfSurfaceBackgroundsBase * alpha;
			if (alpha <= 0f)
			{
				continue;
			}
			int textureSlot = style.ChooseFarTexture();
			if (textureSlot >= 0 && textureSlot < TextureAssets.Background.Length)
			{
				Main.instance.LoadBackground(textureSlot);
				for (int i = 0; i < Main.instance.bgLoops; i++)
				{
					Main.spriteBatch.Draw(TextureAssets.Background[textureSlot].Value, new Vector2((float)(Main.instance.bgStartX + Main.bgWidthScaled * i), (float)Main.instance.bgTopY), (Rectangle?)new Rectangle(0, 0, Main.backgroundWidth[textureSlot], Main.backgroundHeight[textureSlot]), Main.ColorOfSurfaceBackgroundsModified, 0f, default(Vector2), Main.bgScale, (SpriteEffects)0, 0f);
				}
			}
		}
	}

	public void DrawMiddleTexture()
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		if (!GlobalBackgroundStyleLoader.loaded || MenuLoader.loading)
		{
			return;
		}
		foreach (ModSurfaceBackgroundStyle style in list)
		{
			int slot = style.Slot;
			float alpha = Main.bgAlphaFarBackLayer[slot];
			Main.ColorOfSurfaceBackgroundsModified = Main.ColorOfSurfaceBackgroundsBase * alpha;
			if (alpha <= 0f)
			{
				continue;
			}
			int textureSlot = style.ChooseMiddleTexture();
			if (textureSlot >= 0 && textureSlot < TextureAssets.Background.Length)
			{
				Main.instance.LoadBackground(textureSlot);
				for (int i = 0; i < Main.instance.bgLoops; i++)
				{
					Main.spriteBatch.Draw(TextureAssets.Background[textureSlot].Value, new Vector2((float)(Main.instance.bgStartX + Main.bgWidthScaled * i), (float)Main.instance.bgTopY), (Rectangle?)new Rectangle(0, 0, Main.backgroundWidth[textureSlot], Main.backgroundHeight[textureSlot]), Main.ColorOfSurfaceBackgroundsModified, 0f, default(Vector2), Main.bgScale, (SpriteEffects)0, 0f);
				}
			}
		}
	}

	public void DrawCloseBackground(int style)
	{
		//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0202: Unknown result type (might be due to invalid IL or missing references)
		//IL_020e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0214: Unknown result type (might be due to invalid IL or missing references)
		if (!GlobalBackgroundStyleLoader.loaded || MenuLoader.loading || Main.bgAlphaFrontLayer[style] <= 0f)
		{
			return;
		}
		ModSurfaceBackgroundStyle surfaceBackgroundStyle = Get(style);
		if (surfaceBackgroundStyle == null || !surfaceBackgroundStyle.PreDrawCloseBackground(Main.spriteBatch))
		{
			return;
		}
		Main.bgScale = 1.25f;
		Main.instance.bgParallax = 0.37;
		float a = 1800f;
		float b = 1750f;
		int textureSlot = surfaceBackgroundStyle.ChooseCloseTexture(ref Main.bgScale, ref Main.instance.bgParallax, ref a, ref b);
		if (textureSlot < 0 || textureSlot >= TextureAssets.Background.Length)
		{
			return;
		}
		Main.instance.LoadBackground(textureSlot);
		Main.bgScale *= 2f;
		Main.bgWidthScaled = (int)((float)Main.backgroundWidth[textureSlot] * Main.bgScale);
		SkyManager.Instance.DrawToDepth(Main.spriteBatch, 1f / (float)Main.instance.bgParallax);
		Main.instance.bgStartX = (int)(0.0 - Math.IEEERemainder((double)Main.screenPosition.X * Main.instance.bgParallax, Main.bgWidthScaled) - (double)(Main.bgWidthScaled / 2));
		Main.instance.bgTopY = (int)((double)(0f - Main.screenPosition.Y + Main.instance.screenOff / 2f) / (Main.worldSurface * 16.0) * (double)a + (double)b) + (int)Main.instance.scAdj;
		if (Main.gameMenu)
		{
			Main.instance.bgTopY = 320;
		}
		Main.instance.bgLoops = Main.screenWidth / Main.bgWidthScaled + 2;
		if ((double)Main.screenPosition.Y < Main.worldSurface * 16.0 + 16.0)
		{
			for (int i = 0; i < Main.instance.bgLoops; i++)
			{
				Main.spriteBatch.Draw(TextureAssets.Background[textureSlot].Value, new Vector2((float)(Main.instance.bgStartX + Main.bgWidthScaled * i), (float)Main.instance.bgTopY), (Rectangle?)new Rectangle(0, 0, Main.backgroundWidth[textureSlot], Main.backgroundHeight[textureSlot]), Main.ColorOfSurfaceBackgroundsModified, 0f, default(Vector2), Main.bgScale, (SpriteEffects)0, 0f);
			}
		}
	}
}
