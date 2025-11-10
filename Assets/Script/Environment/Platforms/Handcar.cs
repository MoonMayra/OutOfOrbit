using System.Collections;
using UnityEngine;

public class Handcar : MonoBehaviour
{
    [Header("Sprites")]
    [SerializeField] private Sprite activeSprite;
    [SerializeField] private Sprite inactiveSprite;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private Vector2 finalPos = new Vector2(3f, 0f);

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rigidBody;
    public bool isActive = false;
    private Vector2 initialPosition;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer.sprite = inactiveSprite;
    }   

    private void Start()
    {
        initialPosition = transform.position;
    }

    public void UpdateHandcar(Vector2 direction)
    {
        if (!isActive)
        {
            StartCoroutine(MoveHandcar(direction));
        }
    }

    private void Movement(Vector2 direction)
    {
        isActive = true;
        spriteRenderer.sprite = activeSprite;

        Vector2 targetPosition = initialPosition + direction.normalized * moveSpeed;

        rigidBody.MovePosition(targetPosition);
    }

   private IEnumerator MoveHandcar(Vector2 direction)
    {
        while (initialPosition.x < initialPosition.x + finalPos.x || initialPosition.y < initialPosition.y + finalPos.y)
        {
            Debug.Log("Handcar Moving");
            Movement(finalPos);
        }
        yield return new WaitForSeconds(0.02f);
    }
}