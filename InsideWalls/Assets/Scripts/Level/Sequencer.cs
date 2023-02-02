using System;
using UnityEngine;

/// <summary>
/// Causes a series of level events.
/// </summary>
public class Sequencer : MonoBehaviour
{
	[Serializable]
	public struct SequencePhase
	{
		/// <summary>
		/// Editor visible phase name for developer friendliness.
		/// </summary>
		public string PhaseName;

		/// <summary>
		/// How long before this phase pings its listeners.
		/// </summary>
		public float PreDelay;

		/// <summary>
		/// List of listeners invoked by this phase. Listeners must implement ISequenceListener to be invoked.
		/// </summary>
		public MonoBehaviour[] Listeners;
	}

	/// <summary>
	/// List of all phases invoked by this sequencer.
	/// </summary>
	[SerializeField, Tooltip("List of all phases invoked by this sequencer.")]
	private SequencePhase[] _sequencePhases = null;

	/// <summary>
	/// If true, the sequence can only fire once.
	/// </summary>
	[SerializeField, Tooltip("If true, the sequence can only fire once.")]
	private bool _oneShot = true;

	/// <summary>
	/// Tracks the player that triggered the sequence.
	/// </summary>
	private PlayerManager _player = null;

	/// <summary>
	/// Tracks the phase delays.
	/// </summary>
	private Timer _timer = new Timer();

	/// <summary>
	/// Current sequence phase.
	/// </summary>
	private int _phase = -1;

	void OnTriggerEnter(Collider other)
	{
		// Only trigger if the collider is a player
		// and the sequence has not triggered yet.
		if (other.tag.Equals("Player") && _phase < 0)
		{
			// Try to extract the player from the collider.
			_player = other.GetComponent<PlayerManager>();

			// If successful...
			if (_player)
			{
				NextPhase();
			}
		}
	}

	void Update()
	{
		// Untriggered.
		if (_phase < 0)
		{
			return;
		}

		// Reset if the sequence is complete.
		if (_phase == _sequencePhases.Length)
		{
			// Unless the sequence is a one-shot.
			_phase = _oneShot ? _sequencePhases.Length : -1;
			return;
		}

		// Delay complete; invoke
		// listeners and move to
		// the next phase.
		if (_timer.HasElapsed)
		{
			foreach (ISequenceListener listener in _sequencePhases[_phase].Listeners)
			{
				listener.OnLevelSequence(_player);
			}

			NextPhase();
		}
	}

	/// <summary>
	/// Forces the sequence to begin if it hasn't already.
	/// </summary>
	public void ForceSequence(PlayerManager invoker)
	{
		if (_phase < 0)
		{
			_player = invoker;
			NextPhase();
		}
	}

	private void NextPhase()
	{
		// Increment.
		_phase++;

		// Return if the sequence is complete.
		if (_phase == _sequencePhases.Length)
		{
			_player = null;
			return;
		}

		// Set the timer for the next phase and wait.
		_timer.Set(_sequencePhases[_phase].PreDelay);
	}
}
