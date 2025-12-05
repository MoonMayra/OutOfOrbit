using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeScript : MonoBehaviour
{
    public static FadeScript Instance {  get; private set; }

    [SerializeField] string sceneToLoad = string.Empty;
    [SerializeField] float fadeTime = 0.5f;
    [SerializeField] private bool isChangingScene = false;
    private Image blackScreen;
    private Coroutine currentCoroutine;
    private bool isFadingIn = false;


    private void Awake()
    {
        if(Instance == null)
        Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        blackScreen = GetComponent<Image>();  
    }

    public void FadeIn()
    {
        isFadingIn = true;
        isChangingScene = false;
        StartFade(1f,0f);

    }
    public void FadeOut(string sceneToload)
    {
        isFadingIn = false;
        isChangingScene = true;
        sceneToLoad = sceneToload;
        StartFade(0f, 1f);
    }
    private void StartFade(float start, float end)
    {
        if(currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }
        currentCoroutine = StartCoroutine(Fade(start, end));
    }
    public IEnumerator Fade(float initialAlpha, float targetAlpha)
    {
        float timer = 0f;

        while (timer<fadeTime)
        {
            timer += Time.deltaTime;
            float normTime = timer / fadeTime;

            float alpha = Mathf.Lerp(initialAlpha, targetAlpha, normTime);

            Color screen = blackScreen.color;
            screen.a = alpha;
            blackScreen.color = screen;

            yield return null;
        }
        Color finalColor = blackScreen.color;
        finalColor.a = targetAlpha;
        blackScreen.color = finalColor;

        if(isChangingScene && !isFadingIn)
        {
            SceneManager.LoadScene(sceneToLoad);
        }
        currentCoroutine = null;
    }

}
