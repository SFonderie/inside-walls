using UnityEngine;

/// <summary>
/// Contains additional math utilities.
/// </summary>
public static class SMath
{
	/// <summary>
	/// Float lerp function modified to support frame-independent recursion.
	/// </summary>
	public static float RecursiveLerp(float current, float target, float epsilon, float delta)
	{
		return Mathf.LerpUnclamped(target, current, Mathf.Pow(epsilon, delta));
	}

	/// <summary>
	/// Vector2 lerp function modified to support frame-independent recursion.
	/// </summary>
	public static Vector2 RecursiveLerp(Vector2 current, Vector2 target, float epsilon, float delta)
	{
		return Vector2.LerpUnclamped(target, current, Mathf.Pow(epsilon, delta));
	}

	/// <summary>
	/// Vector3 lerp function modified to support frame-independent recursion.
	/// </summary>
	public static Vector3 RecursiveLerp(Vector3 current, Vector3 target, float epsilon, float delta)
	{
		return Vector3.LerpUnclamped(target, current, Mathf.Pow(epsilon, delta));
	}
}
