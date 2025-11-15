using UnityEngine;
using UnityEngine.SceneManagement;

public class AreaSelectorManager : MonoBehaviour
{
    public GameObject loadingScreen;

    public void GoToLevelSelectorJungle()
    {
        loadingScreen.SetActive(true);
        UnityEngine.SceneManagement.SceneManager.LoadScene("LevelSelectorJungle");
    }

    public void GoToLevelSelectorUnderground()
    {
        loadingScreen.SetActive(true);
        UnityEngine.SceneManagement.SceneManager.LoadScene("LevelSelectorUnderground");
    }

    public void BackToAreaSelector()
    {
        loadingScreen.SetActive(true);
        UnityEngine.SceneManagement.SceneManager.LoadScene("AreaSelector");
    }
    public void BackToMainMenuScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

}