using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager Instance { get; private set; }
    public GameObject mainMenu;
    public GameObject optionsMenu;
    public GameObject credits;
    public GameObject control;
    public GameObject mainMenuQuit;
    [SerializeField] private string areaSelectorName = "AreaSelector";
    [SerializeField] private string animationTriggerName = "Death";
    [SerializeField] private Button continueButton;
    [SerializeField] private Button levelsButton;
    private Animator animator;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        animator = GetComponent<Animator>();

        if (PlayerStats.Instance != null)
        {
            PlayerStats.Instance.StopTimer();
        }

        levelsButton.interactable = false;
    }

    public void HideAllScreens()
    {
        optionsMenu.SetActive(false);
        credits.SetActive(false);
        mainMenu.SetActive(false);
        control.SetActive(false);
        mainMenuQuit.SetActive(false);
        ChangeAnimation();
    }

    public void ChangeAnimation()
    {
        animator.SetTrigger(animationTriggerName);
    }
    public void OpenOptionsMenu()
    { 
        mainMenu.SetActive(false);
        ChangeAnimation();
        optionsMenu.SetActive(true);
    }
    public void OpenAreaSelector()
    {
        if (LevelManager.Instance.lastScenePlayed == "Jungle")
        {
            levelsButton.interactable = true;
        }
        
        if (levelsButton.interactable == true)
        {
            mainMenu.SetActive(false);
            ChangeAnimation();
            SceneManager.LoadScene(areaSelectorName);
        }   
    }
    public void OpenCredits()
    {
        mainMenu.SetActive(false);
        ChangeAnimation();
        credits.SetActive(true);
    }
    public void OpenControls()
    {
        optionsMenu.SetActive(false);
        ChangeAnimation();
        control.SetActive(true);
    }
    public void BackToOptions()
    {
        control.SetActive(false);
        ChangeAnimation();
        optionsMenu.SetActive(true);
    }
    public void BackToMainMenu()
    {
        optionsMenu.SetActive(false);
        credits.SetActive(false);
        ChangeAnimation();
        mainMenu.SetActive(true);
    }
    public void OpenMainMenuQuit()
    {
        mainMenuQuit.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    public void CloseMainMenuQuit()
    {
        mainMenuQuit.SetActive(false);
    }
    public void PlayGame()
    {
        SceneManager.LoadScene("Jungle");
        PlayerStats.Instance.ResetValues();
        PlayerStats.Instance.ResetTimer();
        PlayerStats.Instance.StartTimer();
    }
    public void ToggleFullscreen()
    {
        Screen.fullScreen = !Screen.fullScreen;
    }
}