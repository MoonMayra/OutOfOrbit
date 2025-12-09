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

        PlayerPrefs.SetString("LastScenePlayed", lastScene);

        if (lastScene == "Gorilla")
        {
            PlayerPrefs.SetInt("LevelsUnlocked", 1);
            PlayerPrefs.SetInt("JungleBossUnlocked", 1);
            PlayerPrefs.SetInt("CaveUnlocked", 1);
        }

        if (lastScene == "Water")
        {
            PlayerPrefs.SetInt("LevelsUnlocked", 1);
            PlayerPrefs.SetInt("CaveBossUnlocked", 1);
            PlayerPrefs.SetInt("LabUnlocked", 1);
        }

        if (lastScene == "Mia")
        {
            PlayerPrefs.SetInt("LabBossUnlocked", 1);
            PlayerPrefs.SetInt("LevelsUnlocked", 1);
            PlayerPrefs.Save();

            SceneManager.LoadScene("Cutscene8");
            return;
        }

        PlayerPrefs.Save();
        SceneManager.LoadScene("AreaSelector");
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
