using UnityEngine;

public class CutsceneHandler : MonoBehaviour, ISequenceListener
{
	public void OnLevelSequence(PlayerManager invoker)
	{
		invoker.Context.Cutscene = !invoker.Context.Cutscene;
	}
}
