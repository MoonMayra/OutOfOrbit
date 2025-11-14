using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenuManager : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject optionsMenu;
    public GameObject credits;
    public GameObject areaSelector;
    public GameObject loadingScreen;
    public GameObject mainMenuQuit;
    public GameObject levelSelectorJungle;
    public GameObject levelSelectorUnderground;
    public GameObject levelSelectorLab;
    public void OpenOptionsMenu()
    { mainMenu.SetActive(false);
      optionsMenu.SetActive(true);
    }
    public void OpenAreaSelector()
    {
        mainMenu.SetActive(false);
        areaSelector.SetActive(true);
    }
    public void OpenLevelSelectorJungle()
    {
        areaSelector.SetActive(false);
        levelSelectorJungle.SetActive(true);
    }
    public void OpenLevelSelectorUnderground()
    {
        areaSelector.SetActive(false);
        levelSelectorUnderground.SetActive(true);
    }
    public void OpenLevelSelectorLab()
    {
        areaSelector.SetActive(false);
        levelSelectorLab.SetActive(true);
    }
    public void BackToAreaSelector()
    {
        areaSelector.SetActive(true);
        levelSelectorJungle.SetActive(false);
        levelSelectorUnderground.SetActive(false);
        levelSelectorLab.SetActive(false);
    }
    public void OpenCredits()
    {
        mainMenu.SetActive(false);
        credits.SetActive(true);
    }
    public void BackToMainMenu()
    {
        mainMenu.SetActive(true);
        optionsMenu.SetActive(false);
        credits.SetActive(false);
        areaSelector.SetActive(false);
    }
    public void OpenMainMenuQuit()
    {
        mainMenuQuit.SetActive(true);
    }

    public void QuitGame()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }
    public void CloseMainMenuQuit()
    {
        mainMenuQuit.SetActive(false);
    }
    public void PlayGame()
    {
        loadingScreen.SetActive(true);
        SceneManager.LoadScene("Jungle");
    }
}