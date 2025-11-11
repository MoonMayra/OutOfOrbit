using UnityEngine;
using TMPro;
using System.Collections;

public class CollectablesText : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private TextMeshProUGUI collectablesText;
    [SerializeField] private RectTransform panel;
    [SerializeField] private float slideDuration = 0.5f;
    [SerializeField] private float stayDuration = 2f;
    [SerializeField] private DeathsText deathsText;
    [SerializeField] private float instantSlideOutMultiplier = 0.25f;

    private int lastCollectables = -1;
    private Vector2 hiddenPos;
    private Vector2 visiblePos;
    private bool isQueued = false;
    private bool isAnimating = false;

    void Start()
    {
        playerStats = PlayerStats.Instance;
        collectablesText.text = "x " + playerStats.collectables.ToString();

        visiblePos= panel.anchoredPosition;
        hiddenPos= new Vector2(visiblePos.x-panel.rect.width, visiblePos.y);
        panel.anchoredPosition = hiddenPos;
    }

    void Update()
    {
        if(deathsText.isDeathTextActive &&isAnimating)
        {
            StopAllCoroutines();
            StartCoroutine(SlideOutInstant());
        }

        if(playerStats.collectables != lastCollectables)
        {
            lastCollectables = playerStats.collectables;
            collectablesText.text = "x " + playerStats.collectables.ToString();
            TryShowCollectableText();

        }
        
    }

    private void TryShowCollectableText()
    {
        if(deathsText.isDeathTextActive)
        {
            if(!isQueued)
            {
                isQueued = true;
                StartCoroutine(WaitForDeathText());
            }
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(SlideInAndOut());
        }

    }
    private IEnumerator WaitForDeathText()
    {
        Debug.Log("entre a la corrutina");
        yield return new WaitUntil(() => !deathsText.isDeathTextActive);
        Debug.Log("entre a la corrutina2");
        isQueued = false;
        StopAllCoroutines();
        StartCoroutine(SlideInAndOut());
    }

    private IEnumerator SlideInAndOut()
    {
        isAnimating = true;
        float timer = 0.0f;

        while(timer< slideDuration)
        {
            panel.anchoredPosition = Vector2.Lerp(hiddenPos, visiblePos, timer / slideDuration);
            timer += Time.deltaTime;
            yield return null;
        }
        panel.anchoredPosition = visiblePos;

        yield return new WaitForSeconds(stayDuration);

        timer = 0.0f;
        while (timer < slideDuration)
        {
            panel.anchoredPosition = Vector2.Lerp(visiblePos, hiddenPos, timer / slideDuration);
            timer += Time.deltaTime;
            yield return null;
        }
        panel.anchoredPosition = hiddenPos;
        isAnimating = false;
    }
    private IEnumerator SlideOutInstant()
    {
        Debug.Log("Slide out instant called");
        isAnimating = false;
        float timer = 0.0f;
        while (timer < slideDuration*instantSlideOutMultiplier)
        {
            panel.anchoredPosition = Vector2.Lerp(visiblePos, hiddenPos, timer / (slideDuration/2));
            timer += Time.deltaTime;
            yield return null;
        }
        panel.anchoredPosition = hiddenPos;
    }
}

