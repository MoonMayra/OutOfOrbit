using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AsyncLoader : MonoBehaviour
{
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Slider loadBar;

    public void LoadLevel(string lvlToLoad)
    {
        loadingScreen.SetActive(true);
        loadBar.value = 0f;
        StartCoroutine(LoadLvlAsync(lvlToLoad));
    }

    IEnumerator LoadLvlAsync(string lvlToLoad)
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(lvlToLoad);
        loadOperation.allowSceneActivation = false;

        float smoothValue = 0f;

        while (!loadOperation.isDone)
        {
            float targetValue = Mathf.Clamp01(loadOperation.progress / 0.9f);

            smoothValue = Mathf.MoveTowards
                (
                smoothValue,
                targetValue,
                Time.deltaTime * 0.8f
            );

            loadBar.value = smoothValue;

            if (smoothValue >= 1f)
            {
                yield return new WaitForSeconds(0.3f);
                loadOperation.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
