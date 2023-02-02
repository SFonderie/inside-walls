using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAnimation : PlayerDelegate
{
	/// <summary>
	/// Player mesh Transform reference. Use to visualize movement.
	/// </summary>
	[SerializeField, Tooltip("Player mesh Transform reference. Use to visualize movement.")]
	private Transform _playerMesh = null;

	/// <summary>
	/// Local time tracker.
	/// </summary>
	private float _local = 0;

	void Awake()
	{
		DelegateOrder = ANIMATE_ORDER;
	}

	void Start()
	{
		if (!_playerMesh)
		{
			Debug.LogError("[PLAYER ANIMATION] - NO MESH TRANSFORM FOUND; PLAYER ANIMATION DISABLED.");
		}
	}

	public override void HandleInput(InputAction.CallbackContext context)
	{

	}

	public override void UpdateDelegate(PlayerContext context)
	{
		// Translates lateral velocity into rotation speed.
		float speed = Vector3.Dot(context.Velocity, -_playerMesh.parent.right) * context.SpinSpeed;

		// There's a visual problem that occurs when the player's idle hover animations
		// descends while the player is also physically dropping that makes it feel like
		// the player is delibrately dropping too fast (as if to spite the player).
		// To prevent this, we pause the animation in the air.
		if (context.Grounded)
		{
			_local += Time.deltaTime;
		}

		// Adds a hover float effect to give the player an idle animation.
		float hover = Mathf.Sin(_local * 2 * Mathf.PI / context.HoverPeriod) * context.HoverLength * 0.5f;

		if (_playerMesh)
		{
			_playerMesh.Rotate(_playerMesh.parent.forward, speed * Time.deltaTime);
			_playerMesh.localPosition = new Vector3(0, hover, 0);
		}
	}
}
