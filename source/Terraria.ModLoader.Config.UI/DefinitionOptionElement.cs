using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.GameContent;
using Terraria.Localization;
using Terraria.UI;

namespace Terraria.ModLoader.Config.UI;

internal class DefinitionOptionElement<T> : UIElement where T : EntityDefinition
{
	public static Asset<Texture2D> DefaultBackgroundTexture { get; } = TextureAssets.InventoryBack9;


	public Asset<Texture2D> BackgroundTexture { get; set; } = DefaultBackgroundTexture;


	public string Tooltip { get; set; }

	public int Type { get; set; }

	public T Definition { get; set; }

	internal float Scale { get; set; } = 0.75f;


	protected bool Unloaded { get; set; }

	public DefinitionOptionElement(T definition, float scale = 0.75f)
	{
		SetItem(definition);
		Scale = scale;
		Width.Set((float)DefaultBackgroundTexture.Width() * scale, 0f);
		Height.Set((float)DefaultBackgroundTexture.Height() * scale, 0f);
	}

	public virtual void SetItem(T item)
	{
		Definition = item;
		Type = Definition?.Type ?? 0;
		Unloaded = Definition?.IsUnloaded ?? false;
		if (Definition == null || (Type == 0 && !Unloaded))
		{
			Tooltip = Lang.inter[23].Value;
		}
		else if (Unloaded)
		{
			Tooltip = $"{Definition.Name} [{Definition.Mod}] ({Language.GetTextValue("Mods.ModLoader.Unloaded")})";
		}
		else
		{
			Tooltip = Definition.DisplayName + " [" + Definition.Mod + "]";
		}
	}

	public virtual void SetScale(float scale)
	{
		Scale = scale;
		Width.Set((float)DefaultBackgroundTexture.Width() * scale, 0f);
		Height.Set((float)DefaultBackgroundTexture.Height() * scale, 0f);
	}

	public override int CompareTo(object obj)
	{
		DefinitionOptionElement<T> other = obj as DefinitionOptionElement<T>;
		return Type.CompareTo(other.Type);
	}

	public override string ToString()
	{
		return Definition.ToString();
	}
}
