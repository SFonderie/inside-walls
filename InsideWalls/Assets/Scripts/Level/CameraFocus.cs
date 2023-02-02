using UnityEngine;

public class CameraFocus : MonoBehaviour, ISequenceListener
{
	/// <summary>
	/// Refocuses the camera on this Transform reference when triggered.
	/// </summary>
	[SerializeField, Tooltip("Refocuses the camera on this Transform reference when triggered.")]
	private Transform _replacement = null;

	/// <summary>
	/// Whether or not this focuser is active.
	/// </summary>
	private bool _active = false;

	public void OnLevelSequence(PlayerManager invoker)
	{
		if (_replacement && !_active)
		{
			invoker.Context.CameraTarget = _replacement;
			_active = true;
		}
		else
		{
			invoker.Context.CameraTarget = invoker.transform;
			_active = false;
		}
	}
}
