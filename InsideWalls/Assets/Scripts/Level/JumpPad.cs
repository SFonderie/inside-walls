using UnityEngine;

public class JumpPad : MonoBehaviour
{
	/// <summary>
	/// Replaces the player's vertical velocity when they hit the trigger.
	/// </summary>
	[Tooltip("Replaces the player's vertical velocity when they hit the trigger.")]
	public Vector3 Impulse = Vector3.zero;

	/// <summary>
	/// Whether or not the pad reflects player velocity.
	/// </summary>
	[Tooltip("Whether or not the pad reflects player velocity.")]
	public bool Elastic = true;

	/// <summary>
	/// Whether or not the jump pad is active.
	/// </summary>
	[Tooltip("Whether or not the jump pad is active.")]
	public bool Active = true;

	void OnTriggerEnter(Collider other)
	{
		// Ignore on inactivity or non-player entry.
		if (!Active || !other.tag.Equals("Player"))
		{
			return;
		}

		// Attempt to extract the player manager so we can swap contexts.
		PlayerManager invoker = other.GetComponent<PlayerManager>();

		// If we succeed...
		if (invoker)
		{
			// Transform the impulse into world space so that it can
			// interact with the player's velocity (also world space).
			Vector3 impulse = transform.TransformDirection(Impulse);

			// The vertical part of the impulse will go untouched.
			Vector3 vertical = Vector3.Project(impulse, Vector3.up);

			// Now for horizontal impulse, we're going to use ABSOLUTE PROJECTION
			// which is like normal projection but someone (me) slapped an absolute
			// value on the dot product. This makes the resultant vector preserve
			// the direction of the target vector instead of the projected vector,
			// which we need if we're trying to preserve player intent.

			// To start, the player's horizontal velocity needs to be isolated from the vertical velocity and then
			// normalized to save on magnitude calls later.
			Vector3 unitVelocity = invoker.Context.Velocity - Vector3.Project(invoker.Context.Velocity, Vector3.up);
			unitVelocity = Vector3.Normalize(unitVelocity);

			// Now we can take the absolute value of the dot product to ensure
			// confirmity with the base vector (unit velocity).
			float absoluteDot = Mathf.Abs(Vector3.Dot(impulse, unitVelocity));

			// Then we can easily do the projection formula!
			Vector3 lateral = unitVelocity * absoluteDot;

			// When the player hits the jump pad, their downward momentum is reflected
			// back up, which requires some weird gravity stuff thanks to the descent
			// factor. Just isolate the vertical momentum and send it back.
			Vector3 reflected = Vector3.Project(invoker.Context.Momentum, Vector3.up);
			reflected /= Mathf.Sqrt(invoker.Context.DescentFactor);

			// Now actually load in the impulse effects (all three).
			invoker.Context.Momentum = vertical + lateral - (Elastic ? reflected : Vector3.zero);

			// Makes the player "in air" for
			// at least one frame so that the
			// momentum doesn't get wiped.
			invoker.Context.Grounded = false;
		}
	}
}
