using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]
public class BreakablePlatform : MonoBehaviour
{
    [Header("Tiempos")]
    [SerializeField] private float destructionDelay = 2f; 
    [SerializeField] private float respawnDelay = 3f;     

    private SpriteRenderer spriteRenderer;
    private Collider2D platformCollider;

    private bool isBreaking = false;
    private bool isDestroyed = false;
    private float timer = 0f;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        platformCollider = GetComponent<Collider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isBreaking && collision.gameObject.CompareTag("Player"))
        {
            isBreaking = true;
            timer = 0f;
        }
    }

    void Update()
    {
        if (!isBreaking)
            return;

        timer += Time.deltaTime;

        if (!isDestroyed && timer >= destructionDelay)
        { 
            spriteRenderer.enabled = false;
            platformCollider.enabled = false;
            isDestroyed = true;
            timer = 0f;
        }
        else if (isDestroyed && timer >= respawnDelay)
        {
            spriteRenderer.enabled = true;
            platformCollider.enabled = true;
            isDestroyed = false;
            isBreaking = false;
            timer = 0f;
        }
    }
}
