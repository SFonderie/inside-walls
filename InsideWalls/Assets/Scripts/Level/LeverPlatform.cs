using UnityEngine;

/// <summary>
/// Controls level platforms.
/// </summary>
public class LeverPlatform : MonoBehaviour, ISequenceListener
{
	/// <summary>
	/// Breaking platform Rigidbody reference. Used to animate the platform.
	/// </summary>
	[Header("Trigger"), SerializeField, Tooltip("Breaking platform Rigidbody reference. Used to animate the platform.")]
	private Rigidbody _breakPlatform = null;

	/// <summary>
	/// Players must fall this many tiles to trigger the platform.
	/// </summary>
	[SerializeField, Tooltip("Players must fall this many tiles to trigger the platform.")]
	private float _inertia = 4f;

	/// <summary>
	/// The level platform rotates according to this curve.
	/// </summary>
	[Header("Animation"), SerializeField, Tooltip("The level platform rotates according to this curve.")]
	private AnimationCurve _rotateCurve = null;

	/// <summary>
	/// Rotate the level platform by this amount on activation.
	/// </summary>
	[SerializeField, Tooltip("Rotate the level platform by this amount on activation.")]
	private float _rotation = 90f;

	/// <summary>
	/// Wait this many seconds before breaking.
	/// </summary>
	[SerializeField, Tooltip("Wait this many seconds before breaking.")]
	private float _pause = 0.2f;

	/// <summary>
	/// The level platform takes this long to rotate.
	/// </summary>
	[SerializeField, Tooltip("The level platform takes this long to rotate.")]
	private float _duration = 0.2f;

	/// <summary>
	/// Jump pad attached to the lever platform, if applicable.
	/// </summary>
	[Header("Jump Pad"), SerializeField, Tooltip("Jump pad attached to the lever platform, if applicable.")]
	private JumpPad _jumpPad = null;

	/// <summary>
	/// Breaking platform Collider reference. Smooths the animation.
	/// </summary>
	private Collider _breakCollider = null;

	/// <summary>
	/// Tracks the current rotation state.
	/// </summary>
	private Timer _timer = new Timer();

	/// <summary>
	/// Flag prevents additional triggers.
	/// </summary>
	private bool _broken = false;

	/// <summary>
	/// Flag prevents additional triggers.
	/// </summary>
	private bool _rotated = false;

	void Awake()
	{
		_breakCollider = _breakPlatform.GetComponent<Collider>();
	}

	void Update()
	{
		if (_rotated)
		{
			float angle = _rotateCurve.Evaluate(_timer.RelativeProgress) * _rotation;
			Quaternion rotation = Quaternion.AngleAxis(angle, -transform.parent.forward);
			transform.rotation = rotation;

			if (_timer.HasElapsed && _breakPlatform)
			{
				Destroy(_breakPlatform.gameObject);
				_breakPlatform = null;
				_breakCollider = null;

				if (_jumpPad)
				{
					_jumpPad.Active = !_jumpPad.Active;
				}
			}
		}
		else if (_broken && _timer.HasElapsed)
		{
			StartRotation();
		}
	}

	void OnTriggerEnter(Collider other)
	{
		// Ignore when flagged or on non-player entry.
		if (_broken || _rotated || !other.tag.Equals("Player"))
		{
			return;
		}

		// Attempt to extract the player manager so we can swap contexts.
		PlayerManager invoker = other.GetComponent<PlayerManager>();

		// If we succeed...
		if (invoker)
		{
			// To get fall height, we need to get gravity.
			float acceleration = Physics.gravity.magnitude;
			acceleration *= invoker.Context.GravityFactor;
			acceleration *= invoker.Context.DescentFactor;

			// With gravity and current velocity, we get fall time.
			float time = invoker.Context.Velocity.y / acceleration;

			// Finally, kinematics gives us fall height.
			float height = 0.5f * acceleration * time * time;

			// If height is enough...
			if (height >= _inertia)
			{
				OnLevelSequence(invoker);
			}
		}
	}

	public void OnLevelSequence(PlayerManager invoker)
	{
		if (_broken || _rotated)
		{
			return;
		}

		StartBreakage();
	}

	private void StartBreakage()
	{
		if (_broken || _rotated)
		{
			return;
		}

		// Set the timer!
		_timer.Set(_pause);
		_broken = true;
	}

	private void StartRotation()
	{
		if (_rotated)
		{
			return;
		}

		// Enable gravity on the break platform.
		_breakPlatform.useGravity = true;
		_breakCollider.isTrigger = true;

		// Set the timer!
		_timer.Set(_duration);
		_rotated = true;
	}
}
