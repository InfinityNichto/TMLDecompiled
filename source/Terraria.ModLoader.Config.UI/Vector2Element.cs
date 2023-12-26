using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.ModLoader.UI;
using Terraria.UI;

namespace Terraria.ModLoader.Config.UI;

internal class Vector2Element : ConfigElement
{
	private class Vector2Object
	{
		private readonly PropertyFieldWrapper memberInfo;

		private readonly object item;

		private readonly IList<Vector2> array;

		private readonly int index;

		private Vector2 current;

		[LabelKey("$Config.Vector2.X.Label")]
		public float X
		{
			get
			{
				return current.X;
			}
			set
			{
				current.X = value;
				Update();
			}
		}

		[LabelKey("$Config.Vector2.Y.Label")]
		public float Y
		{
			get
			{
				return current.Y;
			}
			set
			{
				current.Y = value;
				Update();
			}
		}

		private void Update()
		{
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			if (array == null)
			{
				memberInfo.SetValue(item, current);
			}
			else
			{
				array[index] = current;
			}
			Interface.modConfig.SetPendingChanges();
		}

		public Vector2Object(PropertyFieldWrapper memberInfo, object item)
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			this.item = item;
			this.memberInfo = memberInfo;
			current = (Vector2)memberInfo.GetValue(item);
		}

		public Vector2Object(IList<Vector2> array, int index)
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			current = array[index];
			this.array = array;
			this.index = index;
		}
	}

	private int height;

	private Vector2Object c;

	private float min;

	private float max = 1f;

	private float increment = 0.01f;

	public IList<Vector2> Vector2List { get; set; }

	public override void OnBind()
	{
		base.OnBind();
		Vector2List = (IList<Vector2>)base.List;
		if (Vector2List != null)
		{
			base.DrawLabel = false;
			height = 30;
			c = new Vector2Object(Vector2List, base.Index);
		}
		else
		{
			height = 30;
			c = new Vector2Object(base.MemberInfo, base.Item);
		}
		if (RangeAttribute != null && RangeAttribute.Min is float && RangeAttribute.Max is float)
		{
			max = (float)RangeAttribute.Max;
			min = (float)RangeAttribute.Min;
		}
		if (IncrementAttribute != null && IncrementAttribute.Increment is float)
		{
			increment = (float)IncrementAttribute.Increment;
		}
		int order = 0;
		foreach (PropertyFieldWrapper variable in ConfigManager.GetFieldsAndProperties(c))
		{
			Tuple<UIElement, UIElement> wrapped = UIModConfig.WrapIt(this, ref height, variable, c, order++);
			if (wrapped.Item2 is FloatElement floatElement)
			{
				floatElement.Min = min;
				floatElement.Max = max;
				floatElement.Increment = increment;
				floatElement.DrawTicks = Attribute.IsDefined(base.MemberInfo.MemberInfo, typeof(DrawTicksAttribute));
			}
			if (Vector2List != null)
			{
				wrapped.Item1.Left.Pixels -= 20f;
				wrapped.Item1.Width.Pixels += 20f;
			}
		}
	}

	public override void Draw(SpriteBatch spriteBatch)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		base.Draw(spriteBatch);
		Rectangle rectangle = GetInnerDimensions().ToRectangle();
		((Rectangle)(ref rectangle))._002Ector(((Rectangle)(ref rectangle)).Right - 30, rectangle.Y, 30, 30);
		spriteBatch.Draw(TextureAssets.MagicPixel.Value, rectangle, Color.AliceBlue);
		float x = (c.X - min) / (max - min);
		float y = (c.Y - min) / (max - min);
		Vector2 position = rectangle.TopLeft();
		position.X += x * (float)rectangle.Width;
		position.Y += y * (float)rectangle.Height;
		Rectangle blipRectangle = default(Rectangle);
		((Rectangle)(ref blipRectangle))._002Ector((int)position.X - 2, (int)position.Y - 2, 4, 4);
		if (x >= 0f && x <= 1f && y >= 0f && y <= 1f)
		{
			spriteBatch.Draw(TextureAssets.MagicPixel.Value, blipRectangle, Color.Black);
		}
		if (base.IsMouseHovering && ((Rectangle)(ref rectangle)).Contains(Main.MouseScreen.ToPoint()) && Main.mouseLeft)
		{
			float newPerc = (float)(Main.mouseX - rectangle.X) / (float)rectangle.Width;
			newPerc = Utils.Clamp(newPerc, 0f, 1f);
			c.X = (float)Math.Round((newPerc * (max - min) + min) * (1f / increment)) * increment;
			newPerc = (float)(Main.mouseY - rectangle.Y) / (float)rectangle.Height;
			newPerc = Utils.Clamp(newPerc, 0f, 1f);
			c.Y = (float)Math.Round((newPerc * (max - min) + min) * (1f / increment)) * increment;
		}
	}

	internal float GetHeight()
	{
		return height;
	}
}
