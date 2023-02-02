using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : PlayerDelegate
{
	/// <summary>
	/// Character Controller reference. Used to apply movement.
	/// </summary>
	[SerializeField, Tooltip("Character Controller reference. Used to apply movement.")]
	private CharacterController _controller = null;

	/// <summary>
	/// Tracks coyote time.
	/// </summary>
	private Timer _coyoteTimer = new Timer();

	/// <summary>
	/// Tracks the jump buffer.
	/// </summary>
	private Timer _bufferTimer = new Timer();

	/// <summary>
	/// Per-frame raw movement input.
	/// </summary>
	private Vector2 _intent = Vector2.zero;

	/// <summary>
	/// Scaled player movement intent in 3D.
	/// </summary>
	private Vector3 _update = Vector3.zero;

	/// <summary>
	/// Current player horizontal velocity in 3D.
	/// </summary>
	private Vector3 _current = Vector3.zero;

	/// <summary>
	/// Tracks jump height locally to catch changes.
	/// </summary>
	private float _heightTracker = 0f;

	/// <summary>
	/// Speed needed to reach a certain height.
	/// </summary>
	private float _jumpSpeed = 0f;

	/// <summary>
	/// Indicates player desire to jump.
	/// </summary>
	private bool _doJump = false;

	/// <summary>
	/// Gates jumps to ensure one jump per input.
	/// </summary>
	private bool _blockJumps = false;

	/// <summary>
	/// Disables input when true.
	/// </summary>
	private bool _ignoreInput = false;

	void Awake()
	{
		DelegateOrder = MOVEMENT_ORDER;
	}

	void Start()
	{
		if (!_controller)
		{
			Debug.LogError("[PLAYER MOVEMENT] - NO CHARACTER CONTROLLER FOUND; PLAYER MOVEMENT DISABLED.");
		}
	}

	public override void HandleInput(InputAction.CallbackContext context)
	{
		if (context.action.name.Equals("Movement"))
		{
			_intent = _ignoreInput ? Vector2.zero : context.ReadValue<Vector2>();
		}

		if (context.action.name.Equals("Jump"))
		{
			_doJump = context.performed && !_blockJumps && !_ignoreInput;
			_blockJumps = _blockJumps && !(_blockJumps && context.canceled);
		}
	}

	public override void UpdateDelegate(PlayerContext context)
	{
		_ignoreInput = context.Paused || context.Cutscene;

		if (_controller)
		{
			// Calculate lateral and forward speed using the player's turn orientation.
			Vector3 lateral = _controller.transform.right * _intent.x * context.MovementSpeed;
			Vector3 forward = _controller.transform.forward * _intent.y * context.MovementSpeed;

			// Forward speed only matters in platformer mode.
			_update = lateral + (context.Platformer ? Vector3.zero : forward);

			if (context.Grounded)
			{
				_current = SMath.RecursiveLerp(_current, _update, 0.1f, Time.deltaTime * context.GroundControl);

				// Constantly reset coyote timer.
				_coyoteTimer.Set(context.CoyoteTime);

				// Reset basic jump info.
				CheckJumpHeight(context);

				// Nullify the impulse on ground.
				context.Momentum = Vector3.zero;
			}
			else
			{
				_current = SMath.RecursiveLerp(_current, _update, 0.1f, Time.deltaTime * context.AirControl);
			}

			// If the controller's vertical velocity is ever zero while the player is in
			// the air and the context momentum is not, then the player is idling in the
			// air. Setting the vertical momentum to zero will cause the player to fall.
			// This also technically triggers on all jumps at the zenith but that's okay
			// since no one's really going to notice a one-frame hover.
			if (context.Momentum.y > 0 && _controller.velocity.y == 0 && !context.Grounded)
			{
				context.Momentum = new Vector3(context.Momentum.x, 0, context.Momentum.z);
			}

			// If the player makes a jump input...
			if (_doJump && !_blockJumps)
			{
				// Set the jump buffer.
				_bufferTimer.Set(context.BufferTime);

				// Flip inputs.
				_doJump = false;
				_blockJumps = true;
			}

			// If we are within both timers' durations in platformer mode, then jump.
			if (!_coyoteTimer.HasElapsed && !_bufferTimer.HasElapsed && context.Platformer)
			{
				// Apply jump impulse via addition.
				context.Momentum += Vector3.up * _jumpSpeed;

				// End both timers.
				_coyoteTimer.End();
				_bufferTimer.End();
			}

			// Gravity factor is base factor times the descent factor (if applicable).
			float factor = context.GravityFactor * (context.Momentum.y < 0 ? context.DescentFactor : 1);

			// Add gravity and mix the two velocities.
			context.Momentum += Physics.gravity * factor * Time.deltaTime;
			context.Velocity = _current + context.Momentum;

			// Finally, we can move the player for real.
			_controller.Move(context.Velocity * Time.deltaTime);
			context.Grounded = _controller.isGrounded;
		}
	}

	/// <summary>
	/// Updates the jump speed if there is a local / context mismatch.
	/// All of this is basically just to avoid a square root call. Whee.
	/// </summary>
	private void CheckJumpHeight(PlayerContext context)
	{
		if (_heightTracker != context.JumpHeight)
		{
			_jumpSpeed = Mathf.Sqrt(2 * context.JumpHeight * Physics.gravity.magnitude * context.GravityFactor);
			_heightTracker = context.JumpHeight;
		}
	}
}
