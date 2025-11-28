using TMPro;
using UnityEngine;
using System.Collections;

public class TimerContextual : MonoBehaviour
{
    [SerializeField] private RectTransform panel;
    [SerializeField] private float slideDuration = 0.5f;
    [SerializeField] private LayerMask playerMask;

    private Vector2 hiddenPos;
    private Vector2 visiblePos;
    private bool isAnimating = false;
    

    private void Start()
    {
        visiblePos = panel.anchoredPosition;
        hiddenPos = new Vector2(visiblePos.x + panel.rect.width, visiblePos.y);
        panel.anchoredPosition = visiblePos;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(((1 << collision.gameObject.layer) & playerMask) != 0)
        {
            if (!isAnimating)
            {
                StartCoroutine(SlideOut());
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & playerMask) != 0)
        {
            if (!isAnimating)
            {
                StartCoroutine(SlideIn());
            }
        }
    }
    private IEnumerator SlideOut()
    {
        isAnimating = true;
        float timer = 0.0f;

        while (timer < slideDuration)
        {
            panel.anchoredPosition = Vector2.Lerp(visiblePos, hiddenPos, timer / slideDuration);
            timer += Time.deltaTime;
            yield return null;
        }
        panel.anchoredPosition = hiddenPos;
        isAnimating = false;
    }
    private IEnumerator SlideIn()
    {
        isAnimating = true;
        float timer = 0.0f;
        timer = 0.0f;
        while (timer < slideDuration)
        {
            panel.anchoredPosition = Vector2.Lerp(hiddenPos, visiblePos, timer / slideDuration);
            timer += Time.deltaTime;
            yield return null;
        }
        panel.anchoredPosition = visiblePos;
        isAnimating = false;
    }
}
