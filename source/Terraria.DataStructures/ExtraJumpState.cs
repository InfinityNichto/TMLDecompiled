namespace Terraria.DataStructures;

/// <summary>
/// A structure containing fields used to manage extra jumps<br /><br />
///
/// Valid states for an extra jump are as follows:
/// <list type="bullet">
/// <item>Enabled = <see langword="false" /> | The extra jump cannot be used.  Available and Active will be <see langword="false" /></item>
/// <item>Enabled = <see langword="true" />, Available = <see langword="true" />,  Active = <see langword="false" /> | The extra jump is ready to be consumed, but hasn't been consumed yet</item>
/// <item>Enabled = <see langword="true" />, Available = <see langword="false" />, Active = <see langword="true" />  | The extra jump has been consumed and is currently in progress</item>
/// <item>Enabled = <see langword="true" />, Available = <see langword="true" />,  Active = <see langword="true" />  | The extra jump is currently in progress, but can be re-used again after it ends</item>
/// <item>Enabled = <see langword="true" />, Available = <see langword="false" />, Active = <see langword="false" /> | The extra jump has been consumed and cannot be used again until extra jumps are refreshed</item>
/// </list>
/// </summary>
public struct ExtraJumpState
{
	private bool _enabled;

	private bool _available;

	private bool _active;

	private bool _disabled;

	/// <summary>
	/// Whether the extra jump can be used. This property is set by <see cref="M:Terraria.DataStructures.ExtraJumpState.Enable" /> and <see cref="M:Terraria.DataStructures.ExtraJumpState.Disable" />.<br />
	/// This property is automatically set to <see langword="false" /> in ResetEffects.<br />
	/// When <see langword="false" />, <see cref="P:Terraria.DataStructures.ExtraJumpState.Available" /> and <see cref="P:Terraria.DataStructures.ExtraJumpState.Active" /> will also be <see langword="false" />.<br />
	/// </summary>
	public bool Enabled
	{
		get
		{
			if (_enabled)
			{
				return !_disabled;
			}
			return false;
		}
	}

	/// <summary>
	/// <see langword="true" /> if the extra jump has not been consumed. Will be set to <see langword="false" /> when the extra jump starts.<br />
	/// Setting this field to <see langword="false" /> will effectively make the game think that the player has already used this extra jump.<br />
	/// This property also checks <see cref="P:Terraria.DataStructures.ExtraJumpState.Enabled" /> when read.<br />
	/// For a reusable jump (e.g. MultipleUseExtraJump from ExampleMod), this property should only be set to <see langword="true" /> in <see cref="M:Terraria.ModLoader.ExtraJump.OnStarted(Terraria.Player,System.Boolean@)" />.
	/// </summary>
	public bool Available
	{
		get
		{
			if (Enabled)
			{
				return _available;
			}
			return false;
		}
		set
		{
			_available = value;
		}
	}

	/// <summary>
	/// Whether any effects (e.g. spawning dusts) should be performed after consuming the extra jump, but before its duration runs out.<br />
	/// This property returns <see langword="true" /> while the extra jump is in progress, and returns <see langword="false" /> otherwise.<br />
	/// While an extra jump is in progress, <see cref="M:Terraria.ModLoader.ExtraJump.UpdateHorizontalSpeeds(Terraria.Player)" /> and <see cref="M:Terraria.ModLoader.ExtraJump.ShowVisuals(Terraria.Player)" /> will be executed.<br />
	/// This property also checks <see cref="P:Terraria.DataStructures.ExtraJumpState.Enabled" /> when read.
	/// </summary>
	public bool Active
	{
		get
		{
			if (Enabled)
			{
				return _active;
			}
			return false;
		}
	}

	/// <summary>
	/// Sets this extra jump to usable for this game tick.<br />
	/// If you want to disable this extra jump, use <see cref="M:Terraria.DataStructures.ExtraJumpState.Disable" />
	/// </summary>
	public void Enable()
	{
		_enabled = true;
	}

	/// <summary>
	/// Forces this extra jump to be disabled, consuming it and preventing the usage of it during the current game tick in the process.<br />
	/// If you want to disable all extra jumps, using <see cref="F:Terraria.Player.blockExtraJumps" /> is preferred.
	/// </summary>
	public void Disable()
	{
		_disabled = true;
	}

	internal void Start()
	{
		_available = false;
		_active = true;
	}

	internal void Stop()
	{
		_active = false;
	}

	internal void ResetEnabled()
	{
		_enabled = false;
		_disabled = false;
	}

	internal void CommitEnabledState(out bool jumpEnded)
	{
		jumpEnded = false;
		if (!Enabled)
		{
			jumpEnded = _active;
			_active = false;
			_available = false;
		}
	}
}
