/// <summary>
/// Allows objects to respond to level sequences.
/// </summary>
public interface ISequenceListener
{
	/// <summary>
	/// Invoked when a sequencer references this listener.
	/// </summary>
	/// <param name="invoker">Player responsible for invoking the sequence.</param>
	public void OnLevelSequence(PlayerManager invoker);
}
