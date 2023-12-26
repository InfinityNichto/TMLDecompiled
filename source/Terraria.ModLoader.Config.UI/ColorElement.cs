using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.UI;

namespace Terraria.ModLoader.Config.UI;

internal class ColorElement : ConfigElement
{
	private class ColorObject
	{
		private readonly PropertyFieldWrapper memberInfo;

		private readonly object item;

		private readonly IList<Color> array;

		private readonly int index;

		internal Color current;

		[LabelKey("$Config.Color.Red.Label")]
		public byte R
		{
			get
			{
				return ((Color)(ref current)).R;
			}
			set
			{
				((Color)(ref current)).R = value;
				Update();
			}
		}

		[LabelKey("$Config.Color.Green.Label")]
		public byte G
		{
			get
			{
				return ((Color)(ref current)).G;
			}
			set
			{
				((Color)(ref current)).G = value;
				Update();
			}
		}

		[LabelKey("$Config.Color.Blue.Label")]
		public byte B
		{
			get
			{
				return ((Color)(ref current)).B;
			}
			set
			{
				((Color)(ref current)).B = value;
				Update();
			}
		}

		[LabelKey("$Config.Color.Hue.Label")]
		public float Hue
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				return Main.rgbToHsl(current).X;
			}
			set
			{
				//IL_001a: Unknown result type (might be due to invalid IL or missing references)
				//IL_001f: Unknown result type (might be due to invalid IL or missing references)
				byte a = A;
				current = Main.hslToRgb(value, Saturation, Lightness);
				((Color)(ref current)).A = a;
				Update();
			}
		}

		[LabelKey("$Config.Color.Saturation.Label")]
		public float Saturation
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				return Main.rgbToHsl(current).Y;
			}
			set
			{
				//IL_001a: Unknown result type (might be due to invalid IL or missing references)
				//IL_001f: Unknown result type (might be due to invalid IL or missing references)
				byte a = A;
				current = Main.hslToRgb(Hue, value, Lightness);
				((Color)(ref current)).A = a;
				Update();
			}
		}

		[LabelKey("$Config.Color.Lightness.Label")]
		public float Lightness
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				return Main.rgbToHsl(current).Z;
			}
			set
			{
				//IL_001a: Unknown result type (might be due to invalid IL or missing references)
				//IL_001f: Unknown result type (might be due to invalid IL or missing references)
				byte a = A;
				current = Main.hslToRgb(Hue, Saturation, value);
				((Color)(ref current)).A = a;
				Update();
			}
		}

		[LabelKey("$Config.Color.Alpha.Label")]
		public byte A
		{
			get
			{
				return ((Color)(ref current)).A;
			}
			set
			{
				((Color)(ref current)).A = value;
				Update();
			}
		}

		private void Update()
		{
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			if (array == null)
			{
				memberInfo.SetValue(item, current);
			}
			else
			{
				array[index] = current;
			}
		}

		public ColorObject(PropertyFieldWrapper memberInfo, object item)
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			this.item = item;
			this.memberInfo = memberInfo;
			current = (Color)memberInfo.GetValue(item);
		}

		public ColorObject(IList<Color> array, int index)
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			current = array[index];
			this.array = array;
			this.index = index;
		}
	}

	private int height;

	private ColorObject c;

	public IList<Color> ColorList { get; set; }

	public override void OnBind()
	{
		base.OnBind();
		ColorList = (IList<Color>)base.List;
		if (ColorList != null)
		{
			base.DrawLabel = false;
			height = 30;
			c = new ColorObject(ColorList, base.Index);
		}
		else
		{
			height = 30;
			c = new ColorObject(base.MemberInfo, base.Item);
		}
		ColorHSLSliderAttribute? customAttributeFromMemberThenMemberType = ConfigManager.GetCustomAttributeFromMemberThenMemberType<ColorHSLSliderAttribute>(base.MemberInfo, base.Item, base.List);
		bool useHue = customAttributeFromMemberThenMemberType != null;
		bool showSaturationAndLightness = customAttributeFromMemberThenMemberType?.ShowSaturationAndLightness ?? false;
		bool num = ConfigManager.GetCustomAttributeFromMemberThenMemberType<ColorNoAlphaAttribute>(base.MemberInfo, base.Item, base.List) != null;
		List<string> skip = new List<string>();
		if (num)
		{
			skip.Add("A");
		}
		if (useHue)
		{
			skip.AddRange(new string[3] { "R", "G", "B" });
		}
		else
		{
			skip.AddRange(new string[3] { "Hue", "Saturation", "Lightness" });
		}
		if (useHue && !showSaturationAndLightness)
		{
			skip.AddRange(new string[2] { "Saturation", "Lightness" });
		}
		int order = 0;
		foreach (PropertyFieldWrapper variable in ConfigManager.GetFieldsAndProperties(c))
		{
			if (!skip.Contains(variable.Name))
			{
				Tuple<UIElement, UIElement> wrapped = UIModConfig.WrapIt(this, ref height, variable, c, order++);
				if (ColorList != null)
				{
					wrapped.Item1.Left.Pixels -= 20f;
					wrapped.Item1.Width.Pixels += 20f;
				}
			}
		}
	}

	public override void Draw(SpriteBatch spriteBatch)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		base.Draw(spriteBatch);
		Rectangle hitbox = GetInnerDimensions().ToRectangle();
		((Rectangle)(ref hitbox))._002Ector(hitbox.X + hitbox.Width / 2, hitbox.Y, hitbox.Width / 2, 30);
		Main.spriteBatch.Draw(TextureAssets.MagicPixel.Value, hitbox, c.current);
	}

	internal float GetHeight()
	{
		return height;
	}
}
