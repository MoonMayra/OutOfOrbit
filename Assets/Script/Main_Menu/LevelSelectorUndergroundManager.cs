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
        loadingScreen.SetActive(false);
    }

    // Go to water boss level
    public void GoToWater()
    {
        loadingScreen.SetActive(true);
        SceneManager.LoadScene("Water");
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
