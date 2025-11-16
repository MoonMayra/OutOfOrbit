using UnityEngine;
using UnityEngine.SceneManagement;

public class DebriefManager : MonoBehaviour
{
    [SerializeField] private string AreaSelectorSceneName = "AreaSelector";
    [SerializeField] public string levelSceneName = "Jungle";

    public void BackToAreaSelectorScene()
    {
        SceneManager.LoadScene(AreaSelectorSceneName);
    }
    public void GoToLevelScene()
    {
        SceneManager.LoadScene(levelSceneName);
    }

}
