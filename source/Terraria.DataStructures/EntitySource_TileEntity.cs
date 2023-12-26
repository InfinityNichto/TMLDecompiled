namespace Terraria.DataStructures;

/// <summary>
/// Used by vanilla for training dummies
/// </summary>
public class EntitySource_TileEntity : IEntitySource
{
	public TileEntity TileEntity { get; }

	public string? Context { get; }

	public EntitySource_TileEntity(TileEntity tileEntity, string? context = null)
	{
		TileEntity = tileEntity;
		Context = context;
	}
}
