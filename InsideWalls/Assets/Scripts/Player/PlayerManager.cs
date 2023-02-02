using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Manages Player Behavior scripts.
/// </summary>
public class PlayerManager : BehaviorManager<PlayerDelegate, PlayerContext>
{
	/// <summary>
	/// Player Input component reference. Used to source input events.
	/// </summary>
	[SerializeField, Tooltip("Player Input component reference. Used to source input events.")]
	private PlayerInput _input = null;

	public override void Start()
	{
		base.Start();

		Context.CameraTarget = transform;
		Pause();

		if (_input)
		{
			_input.onActionTriggered += HandleInput;
		}
	}

	/// <summary>
	/// Passes input events to all Player Delegates.
	/// </summary>
	/// <param name="context">Input event context struct.</param>
	public void HandleInput(InputAction.CallbackContext context)
	{
		if (context.action.name.Equals("Pause"))
		{
			if (Context.Paused)
			{
				Unpause();
			}
			else
			{
				Pause();
			}
		}

		foreach (PlayerDelegate script in Delegates)
		{
			script.HandleInput(context);
		}
	}

	public void Pause()
	{
		Time.timeScale = 0;
		Context.Paused = true;
	}

	public void Unpause()
	{
		Time.timeScale = 1;
		Context.Paused = false;
	}
}
