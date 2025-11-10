using UnityEngine;

public class Handcar : MonoBehaviour
{
    [Header("Sprites")]
    [SerializeField] private Sprite activeSprite;
    [SerializeField] private Sprite inactiveSprite;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float moveDistance = 3f;

    private SpriteRenderer spriteRenderer;
    private bool isActive = false;
    private Vector2 initialPosition;

   private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Voids"))
        {
            isActive = true;
            UpdateHandcar();
        }
    }

    private void UpdateHandcar()
    {
        if (isActive)
        {
            spriteRenderer.sprite = activeSprite;
            initialPosition = transform.position;
            Vector2 targetPosition = initialPosition + Vector2.right * moveDistance;
        }
    }
}