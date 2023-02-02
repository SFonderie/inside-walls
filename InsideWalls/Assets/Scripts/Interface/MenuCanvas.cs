using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Manages the menu canvas.
/// </summary>
public class MenuCanvas : MonoBehaviour, ISequenceListener
{
	enum EMenuType
	{
		MAIN,
		PAUSE,
		DONE,
	}

	/// <summary>
	/// Menu background panel Image reference.
	/// </summary>
	[Header("References"), SerializeField, Tooltip("Menu background panel Image reference.")]
	private Image _menuBackground = null;

	/// <summary>
	/// Menu elements parent Game Object reference.
	/// </summary>
	[SerializeField, Tooltip("Menu elements parent Game Object reference.")]
	private GameObject _menuElements = null;

	/// <summary>
	/// Scene loading Slider reference.
	/// </summary>
	[SerializeField, Tooltip("Scene loading Slider reference.")]
	private Slider _menuSceneLoader = null;

	/// <summary>
	/// Menu title TMP Text reference.
	/// </summary>
	[SerializeField, Tooltip("Menu title TMP Text reference.")]
	private TMP_Text _menuTitleText = null;

	/// <summary>
	/// Play button TMP Text reference.
	/// </summary>
	[SerializeField, Tooltip("Play button TMP Text reference.")]
	private TMP_Text _playButtonText = null;

	/// <summary>
	/// Quit button TMP Text reference.
	/// </summary>
	[SerializeField, Tooltip("Quit button TMP Text reference.")]
	private TMP_Text _quitButtonText = null;

	/// <summary>
	/// Player Manager reference.
	/// </summary>
	[SerializeField, Tooltip("Player Manager reference.")]
	private PlayerManager _player = null;

	/// <summary>
	/// Level sequence triggered on game start.
	/// </summary>
	[SerializeField, Tooltip("Level sequence triggered on game start.")]
	private Sequencer _startSequence = null;

	/// <summary>
	/// Tracks the current menu type.
	/// </summary>
	private EMenuType _menuType = EMenuType.MAIN;

	void Awake()
	{
		SetupMainMenu();
		ShowElements();
		SetBlackout(1);
	}

	void Update()
	{
		// If the game pauses and we're in-game and haven't shown the pause menu yet, reveal it.
		if (_player.Context.Paused && _menuType == EMenuType.PAUSE && !_menuElements.activeSelf)
		{
			SetBlackout(0.5f);
			ShowElements();
		}

		// If the game unpauses and we're in-game and the pause menu is visible, hide it.
		if (!_player.Context.Paused && _menuType == EMenuType.PAUSE && _menuElements.activeSelf)
		{
			HideElements();
			SetBlackout(0);
		}
	}

	/// <summary>
	/// Invoked when the play button is pressed.
	/// </summary>
	public void PlayButton()
	{
		switch (_menuType)
		{
			case EMenuType.MAIN:

				HideElements();
				SetBlackout(0);
				SetupPauseMenu();

				_player.Unpause();

				// Force the level start sequence.
				_startSequence.ForceSequence(_player);

				break;

			case EMenuType.PAUSE:

				HideElements();
				SetBlackout(0);

				_player.Unpause();

				break;

			case EMenuType.DONE:

				_menuSceneLoader.gameObject.SetActive(true);
				StartCoroutine(ReloadScene());

				break;
		}
	}

	/// <summary>
	/// Invoked when the quit button is pressed.
	/// </summary>
	public void QuitButton()
	{
		Application.Quit();
	}

	/// <summary>
	/// Allows the menu to respond to level sequences.
	/// </summary>
	public void OnLevelSequence(PlayerManager invoker)
	{
		SetupReplayMenu();
		SetBlackout(1);
		ShowElements();
	}

	/// <summary>
	/// Checks and sets the menu background alpha level.
	/// </summary>
	private void SetBlackout(float amount)
	{
		amount = Mathf.Clamp(amount, 0, 1);
		_menuBackground.color = new Color(0, 0, 0, amount);
	}

	/// <summary>
	/// Exposes the menu buttons.
	/// </summary>
	private void ShowElements()
	{
		_menuElements.SetActive(true);
		_menuSceneLoader.gameObject.SetActive(false);

		Cursor.lockState = CursorLockMode.Confined;
		Cursor.visible = true;
	}

	/// <summary>
	/// Hides the menu buttons.
	/// </summary>
	private void HideElements()
	{
		_menuElements.SetActive(false);
		_menuSceneLoader.gameObject.SetActive(false);

		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	private void SetupMainMenu()
	{
		_menuType = EMenuType.MAIN;
		_menuTitleText.text = "welcome";
		_playButtonText.text = "begin task";
		_quitButtonText.text = "abandon us";
	}

	private void SetupPauseMenu()
	{
		_menuType = EMenuType.PAUSE;
		_menuTitleText.text = "task suspended";
		_playButtonText.text = "resume task";
		_quitButtonText.text = "abandon us";
	}

	private void SetupReplayMenu()
	{
		_menuType = EMenuType.DONE;
		_menuTitleText.text = "task complete";
		_playButtonText.text = "begin anew";
		_quitButtonText.text = "exit routine";
	}

	private IEnumerator ReloadScene()
	{
		Scene scene = SceneManager.GetActiveScene();
		AsyncOperation loader = SceneManager.LoadSceneAsync(scene.buildIndex);

		while (!loader.isDone)
		{
			if (_menuSceneLoader)
			{
				_menuSceneLoader.value = loader.progress;
			}

			yield return null;
		}
	}
}
