using System;
using UnityEngine;

public class Narrator : MonoBehaviour, ISequenceListener
{
	[Serializable]
	public struct Narration
	{
		/// <summary>
		/// Target display handler reference.
		/// </summary>
		public NarrationDisplay Target;

		/// <summary>
		/// Text loaded into the Text reference.
		/// </summary>
		public string Content;

		/// <summary>
		/// Clears 
		/// </summary>
		public float Duration;
	}

	/// <summary>
	/// List of all narration events triggered by this narrator.
	/// </summary>
	[SerializeField, Tooltip("List of all narration events triggered by this narrator.")]
	private Narration[] _narrations = null;

	/// <summary>
	/// Current narration.
	/// </summary>
	private int _index = -1;

	public void OnLevelSequence(PlayerManager invoker)
	{
		if (_index == _narrations.Length)
		{
			return;
		}

		_index++;

		_narrations[_index].Target.CurrentNarration = _narrations[_index];
	}
}
