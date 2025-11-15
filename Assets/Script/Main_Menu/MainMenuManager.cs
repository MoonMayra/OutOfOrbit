using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenuManager : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject optionsMenu;
    public GameObject credits;
    public GameObject loadingScreen;
    public GameObject mainMenuQuit;
    public GameObject controls;
    public void OpenOptionsMenu()
    { mainMenu.SetActive(false);
      optionsMenu.SetActive(true);
    }
    public void OpenAreaSelector()
    {
        SceneManager.LoadScene("AreaSelector");
    }
    public void OpenCredits()
    {
        mainMenu.SetActive(false);
        credits.SetActive(true);
    }
    public void OpenControls()
    {
        mainMenu.SetActive(false);
        controls.SetActive(true);
    }
    public void BackToMainMenu()
    {
        mainMenu.SetActive(true);
        optionsMenu.SetActive(false);
        credits.SetActive(false);
        controls.SetActive(false);
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