using UnityEngine;
using UnityEngine.Tilemaps;

public class Secret : MonoBehaviour
{
    [SerializeField] private LayerMask playerLayer;
    private Tilemap tiles;
    private bool wasFound = false;

    private void Start()
    {
        tiles= GetComponent<Tilemap>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & playerLayer.value) != 0)
        {
            wasFound = true;
        }
    }

    private void SetOpacity()
    {
        Debug.Log("Fading out");
        Color color = tiles.color;
        color.a = Mathf.MoveTowards(color.a, 0.0f, 0.05f);
        Debug.Log("Current alpha: " + color.a); 
        tiles.color = color;
    }
    private void Update()
    {
        if (wasFound)
        {
            SetOpacity();
        }
        if(tiles.color.a<=0.0f)
        {
            Destroy(gameObject);
        }
    }
}

