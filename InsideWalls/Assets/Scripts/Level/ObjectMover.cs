using UnityEngine;

public class ObjectMover : MonoBehaviour, ISequenceListener
{
	/// <summary>
	/// Target object Transform reference.
	/// </summary>
	[Header("Values"), SerializeField, Tooltip("Target object Transform reference.")]
	private Transform _objectTransform = null;

	/// <summary>
	/// Translation vector. Added to existing position.
	/// </summary>
	[SerializeField, Tooltip("Translation vector. Added to existing position.")]
	private Vector3 _translation = Vector3.zero;

	/// <summary>
	/// Rotation vector. Added to existing rotation.
	/// </summary>
	[SerializeField, Tooltip("Rotation vector. Added to existing rotation.")]
	private Vector3 _rotation = Vector3.zero;

	/// <summary>
	/// Movement animation curve.
	/// </summary>
	[Header("Animation"), SerializeField, Tooltip("Movement animation curve.")]
	private AnimationCurve _curve = null;

	/// <summary>
	/// Length of the movement animation.
	/// </summary>
	[SerializeField, Tooltip("Length of the movement animation.")]
	private float _duration = 0.5f;

	/// <summary>
	/// Tracks the target's transform's start rotation.
	/// </summary>
	private Vector3 _startPosition = Vector3.zero;

	/// <summary>
	/// Tracks the target transform's start position.
	/// </summary>
	private Vector3 _startRotation = Vector3.zero;

	/// <summary>
	/// Tracks the animation progress.
	/// </summary>
	private float _progress = 0;

	/// <summary>
	/// Returns to starting transform while false, moves to target transform while true.
	/// </summary>
	private bool _direction = false;

	void Start()
	{
		if (_objectTransform)
		{
			_startPosition = _objectTransform.localPosition;
			_startRotation = _objectTransform.localRotation.eulerAngles;
		}
	}

	void Update()
	{
		_progress = Mathf.Clamp(_progress + (Time.deltaTime * (_direction ? 1 : -1) / _duration), 0, 1);

		if (_objectTransform)
		{
			float progress = _curve.Evaluate(_progress);

			Vector3 position = Vector3.Lerp(_startPosition, _startPosition + _translation, progress);
			Vector3 rotation = Vector3.Lerp(_startRotation, _startRotation + _rotation, progress);

			_objectTransform.localPosition = position;
			_objectTransform.localRotation = Quaternion.Euler(rotation);
		}
	}

	public void OnLevelSequence(PlayerManager invoker)
	{
		_direction = !_direction;
	}
}
