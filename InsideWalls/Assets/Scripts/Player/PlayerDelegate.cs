using UnityEngine.InputSystem;

/// <summary>
/// Abstract base class for all Player Behavior scripts.
/// </summary>
public abstract class PlayerDelegate : BehaviorDelegate<PlayerContext>
{
	public const int MOVEMENT_ORDER = 0;
	public const int CAMERA_ORDER = 1;
	public const int ANIMATE_ORDER = 100;

	/// <summary>
	/// Invoked once per Unity Input event.
	/// </summary>
	/// <param name="context">Input event context struct.</param>
	public abstract void HandleInput(InputAction.CallbackContext context);
}
