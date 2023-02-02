using UnityEngine;

/// <summary>
/// Pickup object that replaces the active player context.
/// </summary>
public class UpgradeModule : MonoBehaviour
{
	[SerializeField, Tooltip("Replacement context.")]
	private PlayerContext _context = null;

	void OnTriggerEnter(Collider other)
	{
		// Ignore non-player entry.
		if (!other.tag.Equals("Player"))
		{
			return;
		}

		// Attempt to extract the player manager so we can swap contexts.
		PlayerManager invoker = other.GetComponent<PlayerManager>();

		// If we found it...
		if (invoker)
		{
			// Carry this over because it's not easily accessible.
			_context.CameraTarget = invoker.Context.CameraTarget;

			// Swap contexts and then delete.
			invoker.Context = _context;
			Destroy(gameObject);
		}
	}
}
