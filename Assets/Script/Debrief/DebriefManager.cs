using UnityEngine;
using UnityEngine.SceneManagement;

public class DebriefManager : MonoBehaviour
{
    [SerializeField] private string AreaSelectorSceneName = "AreaSelector";
    [SerializeField] public string levelSceneName = "Jungle";

    private void Awake()
    {
        PlayerStats.Instance.StopTimer();
    }
    public void BackToAreaSelectorScene()
    {
        SceneManager.LoadScene(AreaSelectorSceneName);
    }
    public void GoToLevelScene()
    {
        SceneManager.LoadScene(levelSceneName);
        PlayerStats.Instance.ResetValues();
        PlayerStats.Instance.ResetTimer();
        PlayerStats.Instance.StartTimer();
    }

}
