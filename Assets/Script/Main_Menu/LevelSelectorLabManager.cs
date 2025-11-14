using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectorLabManager : MonoBehaviour
{
    public GameObject loadingScreen;

    // Go to levels Lab
    public void GoToLab()
    {
        loadingScreen.SetActive(true);
        SceneManager.LoadScene("Lab");
    }

    // Go to water boss level
    public void GoToMia()
    {
        loadingScreen.SetActive(true);
        SceneManager.LoadScene("Mia");
    }
}
