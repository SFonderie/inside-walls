using System;
using UnityEngine;

/// <summary>
/// Contains shared Player Object data.
/// </summary>
[Serializable]
public class PlayerContext : BehaviorContext
{
	/// <summary>
	/// Player move speed in decimeters per second.
	/// </summary>
	[Header("Ground Movement"), Min(0f), Tooltip("Player move speed in meters per second.")]
	public float MovementSpeed = 1.0f;

	/// <summary>
	/// How effectively ground movement responds to input.
	/// </summary>
	[Min(0f), Tooltip("How effectively ground movement responds to input.")]
	public float GroundControl = 1.0f;

	/// <summary>
	/// Player jump height in decimeters.
	/// </summary>
	[Header("Air Movement"), Min(0f), Tooltip("Player jump height in meters.")]
	public float JumpHeight = 1.0f;

	/// <summary>
	/// Additional factor applied to base gravity.
	/// </summary>
	[Min(0f), Tooltip("Additional factor applied to base gravity.")]
	public float GravityFactor = 1.0f;

	/// <summary>
	/// Additional factor applied to gravity when moving downward.
	/// </summary>
	[Min(1f), Tooltip("Additional factor applied to gravity when moving downward.")]
	public float DescentFactor = 1.0f;

	/// <summary>
	/// How effectively air movement responds to input.
	/// </summary>
	[Min(0f), Tooltip("How effectively air movement responds to input.")]
	public float AirControl = 1.0f;

	/// <summary>
	/// Coyote time in seconds.
	/// </summary>
	[Min(0f), Tooltip("Coyote time in seconds.")]
	public float CoyoteTime = 0.2f;

	/// <summary>
	/// Jump buffer time in seconds.
	/// </summary>
	[Min(0f), Tooltip("Jump buffer time in seconds.")]
	public float BufferTime = 0.2f;

	/// <summary>
	/// Length of the camera arm.
	/// </summary>
	[Header("Camera"), Min(1f), Tooltip("Length of the camera arm.")]
	public float CameraArmLength = 4f;

	/// <summary>
	/// Camera transition speed.
	/// </summary>
	[Min(0f), Tooltip("Camera transition speed.")]
	public float CameraTransition = 1f;

	/// <summary>
	/// Hover bounce period in seconds.
	/// </summary>
	[Header("Juice"), Min(0f), Tooltip("Hover bounce period in seconds.")]
	public float HoverPeriod = 2.0f;

	/// <summary>
	/// Hover bounce magnitude in decimeters.
	/// </summary>
	[Min(0f), Tooltip("Hover bounce magnitude in decimeters.")]
	public float HoverLength = 0.1f;

	/// <summary>
	/// Decorative rotation speed in degrees per second per unit of speed.
	/// </summary>
	[Min(0f), Tooltip("Decorative rotation speed in degrees per second per unit of speed.")]
	public float SpinSpeed = 240.0f;

	/// <summary>
	/// Transform that the camera should always look at and follow.
	/// </summary>
	public Transform CameraTarget { get; set; } = null;

	/// <summary>
	/// Whether or not the game is paused.
	/// </summary>
	public bool Paused { get; set; } = false;

	/// <summary>
	/// Whether or not the game is in a cutscene.
	/// </summary>
	public bool Cutscene { get; set; } = false;

	/// <summary>
	/// Use platformer or twin-stick controls.
	/// </summary>
	public bool Platformer { get; set; } = true;

	/// <summary>
	/// Player forward rotates to match this vector.
	/// </summary>
	public Vector3 Orientation { get; set; } = Vector3.forward;

	/// <summary>
	/// Per-frame velocity as applied to the controller.
	/// </summary>
	public Vector3 Velocity { get; set; } = Vector3.zero;

	/// <summary>
	/// Impulse value. Applied to vertical velocity next frame.
	/// </summary>
	public Vector3 Momentum { get; set; } = Vector3.zero;

	/// <summary>
	/// Per-frame grounded state.
	/// </summary>
	public bool Grounded { get; set; } = false;
}