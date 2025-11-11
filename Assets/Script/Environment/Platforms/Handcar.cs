using System.Collections;
using UnityEngine;

public class Handcar : MonoBehaviour
{
    [Header("Sprites")]
    [SerializeField] private Sprite activeSprite;
    [SerializeField] private Sprite inactiveSprite;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private Vector2 finalDistance = new Vector2(3f, 0f);

    private Coroutine movingCoroutine;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rigidBody;
    private Vector2 initialPosition;
    private Vector2 finalPos;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer.sprite = inactiveSprite;
    }   

    private void Start()
    {
        initialPosition = transform.position;
        finalPos = initialPosition + finalDistance;
    }

    public void UpdateHandcar(Vector2 direction)
    {
        if (movingCoroutine == null)
        {
            movingCoroutine = StartCoroutine(MoveHandcar(direction));
        }
    }

    private void Movement(Vector2 direction)
    {
        spriteRenderer.sprite = activeSprite;

        Vector2 targetPosition = initialPosition + direction.normalized * moveSpeed;

       rigidBody.transform.position = Vector2.MoveTowards(rigidBody.transform.position, direction, moveSpeed * Time.deltaTime);
    }

   private IEnumerator MoveHandcar(Vector2 direction)
    {
        if (initialPosition.x<finalPos.x)
        {
            Movement(finalPos);
        }

        yield return new WaitForSeconds(2f);

        if (initialPosition.x < finalPos.x)
        {
            Movement(initialPosition);
            spriteRenderer.sprite = inactiveSprite;
        }
    }
}
