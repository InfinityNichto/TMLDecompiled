using System;

namespace Terraria.Social.Base;

public class SocialBrowserException : Exception
{
	public SocialBrowserException(string message)
		: base(message)
	{
	}
}
