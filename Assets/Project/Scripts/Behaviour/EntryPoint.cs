using UnityEngine;

public class EntryPoint : MonoBehaviour
{
    [SerializeField] private UIMainMenu _uiMainMenu;
    [SerializeField] private GamePlay _gamePlay;

    private void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;

        _gamePlay.Init();
        OpenMainMenu();
    }

    public void OpenMainMenu()
    {
        _uiMainMenu.Show();
        _gamePlay.StopGame();
    }

    public void StartGame()
    {
        _uiMainMenu.Hide();
        _gamePlay.StartGame();
    }
}
