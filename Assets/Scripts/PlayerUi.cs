using UnityEngine;

public class PlayerUi : MonoBehaviour
{
    //used to update ui such as speed on screen text if we want that
    private void Start()
    {
        InputManager.Instance.playerInputActions.Ui.Pause.started += _cx => PauseGame();
    }

    private void PauseGame()
    {
        PauseManager.Instance.TogglePause();

    }
}