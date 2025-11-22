using UnityEngine;
using UnityEngine.Tilemaps;

public class ExtraSecretTile : MonoBehaviour
{
    [SerializeField] private float fadeSpeed = 2f;
    private Tilemap tiles;
    private bool fade = false;

    private void Start()
    {
        tiles = GetComponent<Tilemap>();
    }

    public void StartDisappearing()
    {
        fade = true;
    }

    private void Update()
    {
        if (!fade) return;

        Color color = tiles.color;
        color.a = Mathf.MoveTowards(color.a, 0f, fadeSpeed * Time.deltaTime);
        tiles.color = color;

        if (tiles.color.a <= 0f)
        {
            Destroy(gameObject);
        }
    }
}
