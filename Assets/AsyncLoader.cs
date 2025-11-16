using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AsyncLoader : MonoBehaviour
{
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private MainMenuManager mainMenuManager;
    [SerializeField] private AreaSelectorManager areaSelectorManager;
    [SerializeField] private Slider loadBar;

    private void Start()
    {
        mainMenuManager = MainMenuManager.Instance;
        areaSelectorManager = AreaSelectorManager.Instance;
    }

    public void LoadLevel(string lvlToLoad)
    {
        if (mainMenuManager != null)
        {
            mainMenuManager.HideAllScreens();
        }
        else if(areaSelectorManager !=null)
        {
            areaSelectorManager.HideAllScreens();
        }
        else
        {
            return;
        }
            loadingScreen.SetActive(true);

        StartCoroutine(LoadLvlASync(lvlToLoad));
    }

    IEnumerator LoadLvlASync(string lvlToLoad)
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(lvlToLoad);

        while(!loadOperation.isDone)
        {
            Debug.Log ("Cargando...");
            float progressValue = Mathf.Clamp01(loadOperation.progress / 0.9f);

            loadBar.value = progressValue;
            yield return null;
        }
    }
}
