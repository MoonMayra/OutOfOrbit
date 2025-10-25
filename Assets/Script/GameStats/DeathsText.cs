using TMPro;
using UnityEngine;
using System.Collections;

public class DeathsText : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private TextMeshProUGUI DeathText;
    [SerializeField] private RectTransform panel;
    [SerializeField] private float slideDuration = 0.5f;
    [SerializeField] private float stayDuration = 2f;
    public bool isDeathTextActive = false;

    private int lastDeaths = 0;
    private Vector2 hiddenPos;
    private Vector2 visiblePos;

    void Start()
    {
        playerStats = PlayerStats.instance;
        DeathText.text = "x " + playerStats.deaths.ToString();

        visiblePos = panel.anchoredPosition;
        hiddenPos = new Vector2(visiblePos.x - panel.rect.width, visiblePos.y);
        panel.anchoredPosition = hiddenPos;
    }

    void Update()
    {
        if (playerStats.deaths != lastDeaths)
        {
            lastDeaths = playerStats.deaths;
            DeathText.text = "x " + playerStats.deaths.ToString();
            StopAllCoroutines();
            StartCoroutine(SlideInAndOutD());
        }

    }
    private IEnumerator SlideInAndOutD()
    {
        isDeathTextActive = true;
        float timer = 0.0f;

        while (timer < slideDuration)
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
        isDeathTextActive = false;
    }
}

