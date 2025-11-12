using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenuManager : MonoBehaviour
{
    // 🔹 Métodos públicos que se conectan con los botones del menú

    public void PlayGame()
    {
        SceneManager.LoadScene("Jungle");
    }

    public void OpenOptions()
    {
        SceneManager.LoadScene("OptionsMenu");
    }

    public void OpenCredits()
    {
        SceneManager.LoadScene("Credits");
    }
    public void OpenAreaSelector()
    {
        SceneManager.LoadScene("AreaSelector");
    }
    public void QuitGame()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }
}
