using UnityEngine;

public class BreakableWall : MonoBehaviour
{
    [Header("Times")]
    [SerializeField] private float destructionDelay = 2f;
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
        if (!isBreaking && !isDestroyed && collision.gameObject.CompareTag("Interactive"))
        {
            breakEffect.Play();
            isBreaking = true;
            timer = 0f;
        }
    }

    void Update()
    {
        if (!isBreaking || isDestroyed)
            return;

        timer += Time.deltaTime;

        if (timer >= destructionDelay)
        {
            spriteRenderer.enabled = false;
            platformCollider.enabled = false;

            isDestroyed = true;
            isBreaking = false;
        }
    }
    public void ResetWall()
    {
        isBreaking = false;
        isDestroyed = false;
        timer = 0f;

        spriteRenderer.enabled = true;
        platformCollider.enabled = true;
    }
}
