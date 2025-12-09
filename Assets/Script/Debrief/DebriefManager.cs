using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class DebriefManager : MonoBehaviour
{
    private Dictionary<string, string> returnScenes = new Dictionary<string, string>()
    {
        { "Gorilla", "Jungle" },
        { "Water", "Underground" },
        { "Mia", "Lab" }
    };

    private void Awake()
    {
        PlayerStats.Instance.StopTimer();
    }

    public void BackToAreaSelectorScene()
    {
        string lastScene = LevelManager.Instance.lastScenePlayed;

        if (lastScene == "Mia")
        {
            SceneManager.LoadScene("Cutscene8");
        }
        else
        {
            SceneManager.LoadScene("AreaSelector");
        }
    }


    public void GoToLevelScene()
    {
        string lastScene = LevelManager.Instance.lastScenePlayed;

        if (returnScenes.ContainsKey(lastScene))
        {
            string sceneToLoad = returnScenes[lastScene];

            SceneManager.LoadScene(sceneToLoad);
        }

        PlayerStats.Instance.ResetValues();
        PlayerStats.Instance.ResetTimer();
        PlayerStats.Instance.StartTimer();
    }
}
