using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MainMenuManager : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject optionsMenu;
    public GameObject credits;
    public GameObject control;
    public GameObject mainMenuQuit;
    [SerializeField] private string areaSelectorName = "AreaSelector";
    [SerializeField] private string animationTriggerName = "Death";
    [SerializeField] private Button continueButton;
    private Animator animator;


    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        string lastScene = PlayerPrefs.GetString("LastScenePlayed", "");

        if (string.IsNullOrEmpty(lastScene))
        {
            continueButton.interactable = false;
        }
        else
        {
            continueButton.interactable = true;
            continueButton.onClick.AddListener(() => ContinueGame(lastScene));
        }
    }

    public void ContinueGame(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void ChangeAnimation()
    {
        animator.SetTrigger(animationTriggerName);
    }
    public void OpenOptionsMenu()
    { mainMenu.SetActive(false);
        ChangeAnimation();
      optionsMenu.SetActive(true);
    }
    public void OpenAreaSelector()
    {
        ChangeAnimation();
        SceneManager.LoadScene(areaSelectorName);
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
        Debug.Log("Saliendo del juego...");
        Application.Quit();
    }
    public void CloseMainMenuQuit()
    {
        mainMenuQuit.SetActive(false);
    }
    public void PlayGame()
    {
        SceneManager.LoadScene("Jungle");
    }
    public void ToggleFullscreen()
    {
        Screen.fullScreen = !Screen.fullScreen;
    }
}