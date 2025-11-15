using UnityEngine;
using UnityEngine.SceneManagement;

public class AreaSlectorManager : MonoBehaviour
{
    public GameObject loadingScreen;
    // Go to jungle area
    public void GoToLevelSelectorJungle()
    {
        loadingScreen.SetActive(true);
        SceneManager.LoadScene("LevelSelectorJungle");
    }
    //Go to undeground area
    public void GoToLevelSelectorUnderground()
    {
        loadingScreen.SetActive(true);
        SceneManager.LoadScene("LevelSelectorUnderground");
    }
    // Go to lab area
    public void GoToLevelSelectorLab()
    {
        loadingScreen.SetActive(true);
        SceneManager.LoadScene("LevelSelectorLab");
    }

    // Go to Main Menu
    public void BackToMainMenuScene()
    {
        loadingScreen.SetActive(true);
        SceneManager.LoadScene("MainMenu");
    }
}

