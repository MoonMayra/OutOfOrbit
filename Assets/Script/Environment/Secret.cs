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
            
            ActivateExtraTiles();
        }
    }

    private void ActivateExtraTiles()
    {
        foreach (Transform child in transform)
        {
            ExtraSecretTile extra = child.GetComponent<ExtraSecretTile>();
            if (extra != null)
                extra.StartDisappearing();
        }
    }

    private void Update()
    {
        Color color = tiles.color;
        color.a = Mathf.MoveTowards(color.a, targetAlpha, fadeSpeed * Time.deltaTime);
        tiles.color = color;
        if(tiles.color.a<=0)
        {
            Destroy(gameObject);
        }
    }
}

