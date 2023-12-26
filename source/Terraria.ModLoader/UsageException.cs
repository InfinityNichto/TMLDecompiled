using System;
using Microsoft.Xna.Framework;

namespace Terraria.ModLoader;

public class UsageException : Exception
{
	internal string msg;

	internal Color color = Color.Red;

	public UsageException()
	{
	}//IL_0001: Unknown result type (might be due to invalid IL or missing references)
	//IL_0006: Unknown result type (might be due to invalid IL or missing references)


	public UsageException(string msg)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		this.msg = msg;
	}

	public UsageException(string msg, Color color)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		this.msg = msg;
		this.color = color;
	}
}
