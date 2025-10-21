using UnityEngine;
using UnityEngine.Tilemaps;

public class Secret : MonoBehaviour
{
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float fadeSpeed = 2f;
    private Tilemap tiles;
    private float targetAlpha = 1f;
    private bool isInside = false;

    private void Start()
    {
        tiles= GetComponent<Tilemap>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & playerLayer.value) != 0 && !isInside)
        {
            targetAlpha = 0.0f;
            isInside = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & playerLayer.value) != 0 && isInside)
        {
            targetAlpha = 1f;
            isInside = false;
        }
    }

    private void Update()
    {
        Color color = tiles.color;
        color.a = Mathf.MoveTowards(color.a, targetAlpha, fadeSpeed * Time.deltaTime);
        tiles.color = color;
    }
}

