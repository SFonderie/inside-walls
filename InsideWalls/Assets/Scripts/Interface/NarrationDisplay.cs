using System;
using TMPro;
using UnityEngine;

public class NarrationDisplay : MonoBehaviour
{
	/// <summary>
	/// Text reference on which narration is displayed.
	/// </summary>
	[SerializeField, Tooltip("Text reference on which narration is displayed.")]
	private TMP_Text _text = null;

	/// <summary>
	/// Tracks the narration time.
	/// </summary>
	private Timer _timer = new Timer();

	/// <summary>
	/// Tracks the current narration value.
	/// </summary>
	private Narrator.Narration _current;

	/// <summary>
	/// Allows the setting of the displayed narration.
	/// </summary>
	public Narrator.Narration CurrentNarration
	{
		get => _current;
		set
		{
			_current = value;
			_timer.Set(_current.Duration);
			_text.text = _current.Content;
		}
	}

	void Update()
	{
		if (_timer.HasElapsed)
		{
			_text.text = "";
		}
	}
}
