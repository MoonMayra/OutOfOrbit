using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void NewGame()
    {
        SceneManager.LoadScene(1);
    }

    // Continue latest saved game
    public void ContinueGame()
    {
        // Lógica para cargar progreso guardado
        // Ejemplo: cargar nombre de escena desde PlayerPrefs
        string savedScene = PlayerPrefs.GetString("SavedScene", "Level1");
        SceneManager.LoadScene(savedScene);
    }

    // Go to area selector screen
    public void OpenLevels()
    {
        SceneManager.LoadScene("AreaSelector");
    }

    // Open options menu
    public void OpenOptions()
    {
        SceneManager.LoadScene("Options");
    }

    // Show credits screen
    public void ShowCredits()
    {
        SceneManager.LoadScene("Credits");
    }

    // Quit the game application
    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }
}
