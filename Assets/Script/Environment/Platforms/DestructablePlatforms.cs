using UnityEngine;
using UnityEngine.Tilemaps;

public class BreakableTilemapPlatform : MonoBehaviour
{
    [Header("Destruction Parameters")]
    [SerializeField] private float destructionDelay = 1.0f;  
    [SerializeField] private bool destroyPlatform = false;

    private Tilemap tilemap;
    private bool isBreaking = false;

    private float destructionTimer = 0f;

    private void Awake()
    {
        tilemap = GetComponent<Tilemap>();
    }

    private void Update()
    {
        if (isBreaking)
        {
            destructionTimer += Time.deltaTime;

            if (destructionTimer >= destructionDelay)
            {
                if (destroyPlatform)
                {
                    tilemap.ClearAllTiles(); 
                }

                isBreaking = false;
                destructionTimer = 0f;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isBreaking) return;

        if (collision.gameObject.CompareTag("Player"))
        { 
            isBreaking = true;
            destructionTimer = 0f;
        }
    }
}
