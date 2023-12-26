namespace Terraria.Map;

public interface IMapLayer
{
	bool Visible { get; internal set; }

	void Draw(ref MapOverlayDrawContext context, ref string text);

	void Hide()
	{
		Visible = false;
	}
}
