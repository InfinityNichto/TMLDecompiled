using rail;
using Terraria.Social.Base;

namespace Terraria.Social.WeGame;

public class FriendsSocialModule : Terraria.Social.Base.FriendsSocialModule
{
	public override void Initialize()
	{
	}

	public override void Shutdown()
	{
	}

	public override string GetUsername()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		string name = default(string);
		rail_api.RailFactory().RailPlayer().GetPlayerName(ref name);
		WeGameHelper.WriteDebugString("GetUsername by wegame" + name);
		return name;
	}

	public override void OpenJoinInterface()
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		WeGameHelper.WriteDebugString("OpenJoinInterface by wegame");
		rail_api.RailFactory().RailFloatingWindow().AsyncShowRailFloatingWindow((EnumRailWindowType)10, "");
	}
}
