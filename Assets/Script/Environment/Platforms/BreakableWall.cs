using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]
public class BreakableWall : MonoBehaviour
{
    [Header("Times")]
    [SerializeField] private float destructionDelay = 2f;
    [SerializeField] private float respawnDelay = 3f;
    [SerializeField] private ParticleSystem breakEffect;

    private SpriteRenderer spriteRenderer;
    private Collider2D platformCollider;

    private bool isBreaking = false;
    private bool isDestroyed = false;
    private float timer = 0f;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        platformCollider = GetComponent<Collider2D>();
        if (breakEffect == null)
            breakEffect = gameObject.GetComponentInChildren<ParticleSystem>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision detected with: " + collision.gameObject.name);
        if (!isBreaking && collision.gameObject.CompareTag("Interactive"))
        {
            Debug.Log("BreakableWall triggered by Interactive object.");
            breakEffect.Play();
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
            Debug.Log("BreakableWall is breaking.");
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
