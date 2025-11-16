using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    [SerializeField] private InputActionReference PauseInput;
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private GameObject quitScreen;
    [SerializeField] private GameObject optionsScreen;
    [SerializeField] private GameObject lvlSelectScreen;
    [SerializeField] private string lvlSelectName = "AreaSelector";
    [SerializeField] private string glitchTrigger = "Death";
    private Animator animator;
    [SerializeField] private LevelManager lvlManager;

    private void Start()
    {
        animator = GetComponent<Animator>();
        PauseInput.action.performed += HandlePause;
        lvlManager=LevelManager.Instance;
    }
    private void HandlePause(InputAction.CallbackContext context)
    {
        Paused();
    }
    private void ChangeScreenAnimation()
    {
        if (animator != null)
        {
            animator.SetTrigger(glitchTrigger);
        }
    }
    public void Paused()
    {
        pauseScreen.SetActive(true);
        Time.timeScale = 0.0f;
    }
    public void Resume()
    {
        pauseScreen.SetActive(false);
        Time.timeScale = 1.0f;
    }
    public void Retry()
    {
        Time.timeScale = 1.0f;
        lvlManager.RespawnPlayer(lvlManager.currentCheckpoint.spawnPoint);
        pauseScreen.SetActive(false);

    }
    public void Options()
    {
        pauseScreen.SetActive(false);
        ChangeScreenAnimation();
        optionsScreen.SetActive(true);
    }
    public void Quit()
    {
        Application.Quit();
    }
    public void OpenQuitPanel()
    {
        ChangeScreenAnimation();
        quitScreen.SetActive(true);
    }
    public void ApplyOptions()
    {
        //Mica vos tenes que hacer esto (aplicar las opciones)
    }
    public void ResetOptions()
    {
        //Mica vos tenes que hacer esto (reset de opciones)
    }
    public void ReturnToPause()
    {
        quitScreen.SetActive(false);
        optionsScreen.SetActive(false);
        lvlSelectScreen.SetActive(false);
        ChangeScreenAnimation();
        pauseScreen.SetActive(true);
    }
    public void ReturnToLevelSelectorInPanel()
    {
        SceneManager.LoadScene(lvlSelectName);
    }
    public void OpenlvlSelectPanel()
    {
        ChangeScreenAnimation();
        lvlSelectScreen.SetActive(true);
    }
}
