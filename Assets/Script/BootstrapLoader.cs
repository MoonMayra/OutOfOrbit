using UnityEngine;
using UnityEngine.SceneManagement;

public class BootstrapLoader : MonoBehaviour
{
    [SerializeField] private string firstScene = "MainMenu";

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        if (LevelManager.Instance != null)
        {
            LevelManager.Instance.SetLastScene(SceneManager.GetActiveScene().name);
        }

        SceneManager.LoadScene(firstScene);
    }
}
