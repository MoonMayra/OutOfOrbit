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
        loadingScreen.SetActive(false);
    }

    // Go to water boss level
    public void GoToMia()
    {
        loadingScreen.SetActive(true);
        SceneManager.LoadScene("Mia");
        loadingScreen.SetActive(false);
    }
    //Go back to area selector
    public void BackToAreaSelector()
    {
        loadingScreen.SetActive(true);
        SceneManager.LoadScene("AreaSelector");
        loadingScreen.SetActive(false);
    }
}
