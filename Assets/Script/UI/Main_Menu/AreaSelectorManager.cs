using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AreaSelectorManager : MonoBehaviour
{
    public static AreaSelectorManager Instance { get; private set; } 
    public GameObject areaSelector;
    public GameObject jungleArea;
    public GameObject caveArea;
    public GameObject labArea;
    [SerializeField] private string triggerName="Death";
    [SerializeField] private string menuSceneName = "MainMenu";
    [SerializeField] private string jungleSceneName = "Jungle";
    [SerializeField] private string caveSceneName = "Underground";
    [SerializeField] private string labSceneName = "Lab";
    [SerializeField] private string jungleBossSceneName = "Gorilla";
    [SerializeField] private string caveBossSceneName = "Water";
    [SerializeField] private string labBossSceneName = "Mia";

    [SerializeField] private Button jungleBossButton;
    [SerializeField] private Button caveBossButton;

    [SerializeField] private Button caveAreaButton;

    private Animator animator;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        animator = GetComponent<Animator>();
        PlayerStats.Instance.StopTimer();

        string lastScene = PlayerPrefs.GetString("LastScenePlayed", "");

        bool jungleBossUnlocked = PlayerPrefs.GetInt("JungleBossUnlocked", 0) == 1;
        bool caveUnlocked = PlayerPrefs.GetInt("CaveUnlocked", 0) == 1;
        bool caveBossUnlocked = PlayerPrefs.GetInt("CaveBossUnlocked", 0) == 1;

        if (lastScene == "Gorilla")
        {
            PlayerPrefs.SetInt("JungleBossUnlocked", 1);
            PlayerPrefs.SetInt("CaveUnlocked", 1);

            jungleBossUnlocked = true;
            caveUnlocked = true;
        }

        if (lastScene == "Water")
        {
            PlayerPrefs.SetInt("CaveBossUnlocked", 1);
            caveBossUnlocked = true;
        }

        PlayerPrefs.Save();
        jungleBossButton.interactable = jungleBossUnlocked;
        caveAreaButton.interactable = caveUnlocked;
        caveBossButton.interactable = caveBossUnlocked;
    }
    private void ChangeAnimation()
    {
        animator.SetTrigger(triggerName);
    }

    public void GoToLevelSelectorJungle()
    {
        areaSelector.SetActive(false);
        ChangeAnimation();
        jungleArea.SetActive(true);
    }

    public void GoToLevelSelectorCave()
    {
        areaSelector.SetActive(false);
        ChangeAnimation();
        caveArea.SetActive(true);
    }

    public void GoToLevelSelectorLab()
    {
        areaSelector.SetActive(false);
        ChangeAnimation();
        labArea.SetActive(true);
    }

    public void GoBackToAreaSelector()
    {
        jungleArea.SetActive(false);
        caveArea.SetActive(false);
        labArea.SetActive(false);
        ChangeAnimation();
        areaSelector.SetActive(true);
    }

    public void BackToMainMenuScene()
    {
        SceneManager.LoadScene(menuSceneName);
    }

    public void GoToJungleScene()
    {
        SceneManager.LoadScene(jungleSceneName);
        PlayerStats.Instance.StartTimer();
    }

    public void GoToCaveScene()
    {
        SceneManager.LoadScene(caveSceneName);
    }

    public void GoToLabScene()
    {
        SceneManager.LoadScene(labSceneName);
    }

    public void GoToJungleBossScene()
    {
        SceneManager.LoadScene(jungleBossSceneName);
        PlayerStats.Instance.StartTimer();
    }

    public void GoToCaveBossScene()
    {
        SceneManager.LoadScene(caveBossSceneName);
    }

    public void GoToLabBossScene()
    {
        SceneManager.LoadScene(labBossSceneName);
    }
    public void HideAllScreens()
    {
        jungleArea.SetActive(false);
        caveArea.SetActive(false);
        labArea.SetActive(false);
        areaSelector.SetActive(false);
        ChangeAnimation();
    }
}