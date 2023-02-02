using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCamera : PlayerDelegate
{
	/// <summary>
	/// Camera arm Transform reference.
	/// </summary>
	[SerializeField, Tooltip("Camera arm Transform reference.")]
	private Transform _cameraArm = null;

	void Awake()
	{
		DelegateOrder = CAMERA_ORDER;
	}

	void Start()
	{
		if (!_cameraArm)
		{
			Debug.LogError("[PLAYER CAMERA] - NO CAMERA TRANSFORM FOUND; CAMERA UPDATES DISABLED.");
		}
	}

	public override void HandleInput(InputAction.CallbackContext context)
	{

	}

	public override void UpdateDelegate(PlayerContext context)
	{
		if (_cameraArm)
		{
			// Set the parent to enable retargeting.
			_cameraArm.SetParent(context.CameraTarget);

			// Get our current and target for readability.
			Vector3 current = _cameraArm.localPosition;
			Vector3 target = new Vector3(0, 0, -context.CameraArmLength);

			// Now do the actual transition effect using my amazing Recursive Lerp function.
			current = SMath.RecursiveLerp(current, target, 0.1f, Time.deltaTime * context.CameraTransition);

			// And set the new position.
			_cameraArm.localPosition = current;
		}
	}
}
