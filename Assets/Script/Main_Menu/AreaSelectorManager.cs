using UnityEngine;
using UnityEngine.SceneManagement;

public class AreaSelectorManager : MonoBehaviour
{
    public GameObject areaSelector;
    public GameObject jungleArea;
    public GameObject caveArea;
    public GameObject labArea;
    [SerializeField] private string triggerName="Death";
    [SerializeField] private string menuSceneName = "MainMenu";
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
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
        UnityEngine.SceneManagement.SceneManager.LoadScene(menuSceneName);
    }

}