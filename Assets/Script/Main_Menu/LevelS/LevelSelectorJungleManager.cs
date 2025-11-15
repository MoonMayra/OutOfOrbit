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
}