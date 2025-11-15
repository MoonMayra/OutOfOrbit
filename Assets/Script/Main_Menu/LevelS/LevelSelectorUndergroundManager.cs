using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectorUndergroundManager : MonoBehaviour
{
    public GameObject loadingScreen;

    // Go to levels Underground cave
    public void GoToUnderground()
    {
        loadingScreen.SetActive(true);
        SceneManager.LoadScene("Underground");
    }

    // Go to water boss level
    public void GoToWater()
    {
        loadingScreen.SetActive(true);
        SceneManager.LoadScene("Water");
    }
}
