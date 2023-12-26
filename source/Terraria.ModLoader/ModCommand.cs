namespace Terraria.ModLoader;

/// <summary>
/// This class represents a chat or console command. Use the CommandType to specify the scope of the command.
/// </summary>
public abstract class ModCommand : ModType
{
	/// <summary>The desired text to trigger this command.</summary>
	public abstract string Command { get; }

	/// <summary>A flag enum representing context where this command operates.</summary>
	public abstract CommandType Type { get; }

	/// <summary>A short usage explanation for this command.</summary>
	public virtual string Usage => "/" + Command;

	/// <summary>A short description of this command.</summary>
	public virtual string Description => "";

	protected sealed override void Register()
	{
		CommandLoader.Add(this);
	}

	public sealed override void SetupContent()
	{
		SetStaticDefaults();
	}

	/// <summary>The code that is executed when the command is triggered.</summary>
	public abstract void Action(CommandCaller caller, string input, string[] args);
}
