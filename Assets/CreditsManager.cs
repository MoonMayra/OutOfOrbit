using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsManager : MonoBehaviour
{
    [SerializeField] private string MainMenuSceneName = "MainMenu";

    private void Awake()
    {
        PlayerStats.Instance.StopTimer();
    }

    public void BackToMainMenuScene()
    {
        SceneManager.LoadScene(MainMenuSceneName);
        PlayerStats.Instance.ResetValues();
        PlayerStats.Instance.ResetTimer();
    }
}
