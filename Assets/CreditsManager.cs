using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsManager : MonoBehaviour
{
    [SerializeField] private string MainMenuSceneName = "MainMenu";

    public void BackToMainMenuScene()
    {
        SceneManager.LoadScene(MainMenuSceneName);
    }
}
