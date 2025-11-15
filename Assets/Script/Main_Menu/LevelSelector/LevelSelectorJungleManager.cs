using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelSelectorJungleManager : MonoBehaviour 
{ 
    public GameObject loadingScreen; 
    // Go to levels Jungle
    public void GoToJungle()
    {
        loadingScreen.SetActive(true);
        SceneManager.LoadScene("Jungle");
    }
    // Go to gorilla boss level
    public void GoToGorilla()
    {
        loadingScreen.SetActive(true);
        SceneManager.LoadScene("Gorilla");
    }
   // Go back to area selector
    public void BackToAreaSelector()
    {
        loadingScreen.SetActive(true);
        SceneManager.LoadScene("AreaSelector");
        loadingScreen.SetActive(false);
    }
}