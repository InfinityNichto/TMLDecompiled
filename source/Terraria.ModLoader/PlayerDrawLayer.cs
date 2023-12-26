using System.Collections;
using System.Collections.Generic;
using Terraria.DataStructures;

namespace Terraria.ModLoader;

/// <summary>
/// This class represents a DrawLayer for the player, and uses PlayerDrawInfo as its InfoType. Drawing should be done by adding Terraria.DataStructures.DrawData objects to Main.playerDrawData.
/// </summary>
[Autoload(true)]
public abstract class PlayerDrawLayer : ModType
{
	public abstract class Transformation
	{
		public virtual Transformation Parent { get; }

		/// <summary>
		/// Add a transformation to the drawInfo
		/// </summary>
		protected abstract void PreDraw(ref PlayerDrawSet drawInfo);

		/// <summary>
		/// Reverse a transformation from the drawInfo
		/// </summary>
		protected abstract void PostDraw(ref PlayerDrawSet drawInfo);

		public void PreDrawRecursive(ref PlayerDrawSet drawInfo)
		{
			Parent?.PreDrawRecursive(ref drawInfo);
			PreDraw(ref drawInfo);
		}

		public void PostDrawRecursive(ref PlayerDrawSet drawInfo)
		{
			PostDraw(ref drawInfo);
			Parent?.PostDrawRecursive(ref drawInfo);
		}
	}

	public abstract class Position
	{
	}

	public sealed class Between : Position
	{
		public PlayerDrawLayer Layer1 { get; }

		public PlayerDrawLayer Layer2 { get; }

		public Between(PlayerDrawLayer layer1, PlayerDrawLayer layer2)
		{
			Layer1 = layer1;
			Layer2 = layer2;
		}

		public Between()
		{
		}
	}

	public class Multiple : Position, IEnumerable
	{
		public delegate bool Condition(PlayerDrawSet drawInfo);

		public IList<(Between, Condition)> Positions { get; } = new List<(Between, Condition)>();


		public void Add(Between position, Condition condition)
		{
			Positions.Add((position, condition));
		}

		public IEnumerator GetEnumerator()
		{
			return Positions.GetEnumerator();
		}
	}

	public class BeforeParent : Position
	{
		public PlayerDrawLayer Parent { get; }

		public BeforeParent(PlayerDrawLayer parent)
		{
			Parent = parent;
		}
	}

	public class AfterParent : Position
	{
		public PlayerDrawLayer Parent { get; }

		public AfterParent(PlayerDrawLayer parent)
		{
			Parent = parent;
		}
	}

	private readonly List<PlayerDrawLayer> _childrenBefore = new List<PlayerDrawLayer>();

	private readonly List<PlayerDrawLayer> _childrenAfter = new List<PlayerDrawLayer>();

	public bool Visible { get; private set; } = true;


	public virtual Transformation Transform { get; }

	public IReadOnlyList<PlayerDrawLayer> ChildrenBefore => _childrenBefore;

	public IReadOnlyList<PlayerDrawLayer> ChildrenAfter => _childrenAfter;

	/// <summary> Returns whether or not this layer should be rendered for the minimap icon. </summary>
	public virtual bool IsHeadLayer => false;

	public void Hide()
	{
		Visible = false;
	}

	internal void AddChildBefore(PlayerDrawLayer child)
	{
		_childrenBefore.Add(child);
	}

	internal void AddChildAfter(PlayerDrawLayer child)
	{
		_childrenAfter.Add(child);
	}

	internal void ClearChildren()
	{
		_childrenBefore.Clear();
		_childrenAfter.Clear();
	}

	/// <summary> Returns the layer's default visibility. This is usually called as a layer is queued for drawing, but modders can call it too for information. </summary>
	/// <returns> Whether or not this layer will be visible by default. Modders can hide layers later, if needed.</returns>
	public virtual bool GetDefaultVisibility(PlayerDrawSet drawInfo)
	{
		return true;
	}

	/// <summary> Returns the layer's default position in regards to other layers. Make use of e.g <see cref="T:Terraria.ModLoader.PlayerDrawLayer.BeforeParent" />/<see cref="T:Terraria.ModLoader.PlayerDrawLayer.AfterParent" />, and provide a layer (usually a vanilla one from <see cref="T:Terraria.DataStructures.PlayerDrawLayers" />). </summary>
	public abstract Position GetDefaultPosition();

	internal void ResetVisibility(PlayerDrawSet drawInfo)
	{
		foreach (PlayerDrawLayer item in ChildrenBefore)
		{
			item.ResetVisibility(drawInfo);
		}
		Visible = GetDefaultVisibility(drawInfo);
		foreach (PlayerDrawLayer item2 in ChildrenAfter)
		{
			item2.ResetVisibility(drawInfo);
		}
	}

	/// <summary> Draws this layer. This will be called multiple times a frame if a player afterimage is being drawn. If this layer shouldn't draw with each afterimage, check <code>if(drawinfo.shadow == 0f)</code> to only draw for the original player image.</summary>
	protected abstract void Draw(ref PlayerDrawSet drawInfo);

	public void DrawWithTransformationAndChildren(ref PlayerDrawSet drawInfo)
	{
		if (!Visible)
		{
			return;
		}
		Transform?.PreDrawRecursive(ref drawInfo);
		foreach (PlayerDrawLayer item in ChildrenBefore)
		{
			item.DrawWithTransformationAndChildren(ref drawInfo);
		}
		Draw(ref drawInfo);
		foreach (PlayerDrawLayer item2 in ChildrenAfter)
		{
			item2.DrawWithTransformationAndChildren(ref drawInfo);
		}
		Transform?.PostDrawRecursive(ref drawInfo);
	}

	protected sealed override void Register()
	{
		ModTypeLookup<PlayerDrawLayer>.Register(this);
		PlayerDrawLayerLoader.Add(this);
	}

	public sealed override void SetupContent()
	{
		SetStaticDefaults();
	}

	public override string ToString()
	{
		return Name;
	}
}
