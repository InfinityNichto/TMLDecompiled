using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.GameContent;

namespace Terraria.ModLoader;

/// <summary>
/// This class represents a type of dust that is added by a mod. Only one instance of this class will ever exist for each type of dust you add.<br />
/// The <see href="https://github.com/tModLoader/tModLoader/wiki/Basic-Dust">Basic Dust Guide</see> teaches the basics of making modded dust.
/// </summary>
[Autoload(true, Side = ModSide.Client)]
public abstract class ModDust : ModTexturedType
{
	/// <summary> Allows you to choose a type of dust for this type of dust to copy the behavior of. Defaults to -1, which means that no behavior is copied. </summary>
	public int UpdateType { get; set; } = -1;


	/// <summary> The sprite sheet that this type of dust uses. Normally a sprite sheet will consist of a vertical alignment of three 10 x 10 pixel squares, each one containing a possible look for the dust. </summary>
	public Asset<Texture2D> Texture2D { get; private set; }

	/// <summary> The ID of this type of dust. </summary>
	public int Type { get; internal set; }

	protected sealed override void Register()
	{
		ModTypeLookup<ModDust>.Register(this);
		DustLoader.dusts.Add(this);
		Type = DustLoader.ReserveDustID();
		Texture2D = ((!string.IsNullOrEmpty(Texture)) ? ModContent.Request<Texture2D>(Texture) : TextureAssets.Dust);
	}

	/// <summary>
	/// Allows drawing behind this dust, such as a trail, or modifying the way it is drawn. Return false to stop the normal dust drawing code (useful if you're manually drawing the dust itself). Returns true by default.
	/// </summary>
	/// <returns></returns>
	public virtual bool PreDraw(Dust dust)
	{
		return true;
	}

	internal void Draw(Dust dust, Color alpha, float scale)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		Main.spriteBatch.Draw(Texture2D.Value, dust.position - Main.screenPosition, (Rectangle?)dust.frame, alpha, dust.rotation, new Vector2(4f, 4f), scale, (SpriteEffects)0, 0f);
		if (dust.color != default(Color))
		{
			Main.spriteBatch.Draw(Texture2D.Value, dust.position - Main.screenPosition, (Rectangle?)dust.frame, dust.GetColor(alpha), dust.rotation, new Vector2(4f, 4f), scale, (SpriteEffects)0, 0f);
		}
	}

	public sealed override void SetupContent()
	{
		SetStaticDefaults();
	}

	/// <summary>
	/// Allows you to modify the properties after initial loading has completed.
	/// This is where you would update ModDust's UpdateType property and modify the Terraria.GameContent.ChildSafety.SafeDust array.
	/// </summary>
	public override void SetStaticDefaults()
	{
	}

	/// <summary>
	/// Allows you to modify a dust's fields when it is created.
	/// </summary>
	public virtual void OnSpawn(Dust dust)
	{
	}

	/// <summary>
	/// Allows you to customize how you want this type of dust to behave. Return true to allow for vanilla dust updating to also take place; will return true by default. Normally you will want this to return false.
	/// </summary>
	public virtual bool Update(Dust dust)
	{
		return true;
	}

	/// <summary>
	/// Allows you to add behavior to this dust on top of the default dust behavior. Return true if you're applying your own behavior; return false to make the dust slow down by itself. Normally you will want this to return true.
	/// </summary>
	public virtual bool MidUpdate(Dust dust)
	{
		return false;
	}

	/// <summary>
	/// Allows you to override the color this dust will draw in. Return null to draw it in the normal light color; returns null by default. Note that the dust.noLight field makes the dust ignore lighting and draw in full brightness, and can be set in OnSpawn instead of having to return Color.White here.
	/// </summary>
	public virtual Color? GetAlpha(Dust dust, Color lightColor)
	{
		return null;
	}
}
