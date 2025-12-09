using System.Collections;
using UnityEngine;

public class Handcar : MonoBehaviour
{
    [Header("Sprites")]
    [SerializeField] private Sprite activeSprite;
    [SerializeField] private Sprite inactiveSprite;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private Vector2 moveDistance = new Vector2(3f, 0f);

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
    }

    public void UpdateHandcar(Vector2 direction)
    {
        if (movingCoroutine == null)
        {
            movingCoroutine = StartCoroutine(MoveHandcar(direction));
        }
    }
    private IEnumerator MoveHandcar(Vector2 direction)
    {
        spriteRenderer.sprite = activeSprite;

        if (direction.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (direction.x > 0)
        {
            spriteRenderer.flipX = false;
        }

        Vector2 targetPosition = initialPosition + direction.normalized * moveDistance;

        float t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime * moveSpeed;
            rigidBody.transform.position = Vector2.Lerp(initialPosition, targetPosition, t);
            yield return null;
        }

        yield return new WaitForSeconds(1f);

        t = 0;
        while (t < 1f)
        {
            t += Time.deltaTime * moveSpeed;
            rigidBody.transform.position = Vector2.Lerp(targetPosition, initialPosition, t);
            yield return null;
        }

        spriteRenderer.sprite = inactiveSprite;
        movingCoroutine = null;
    }

    private void OnDrawGizmos()
    {
        Vector2 basePos = Application.isPlaying ? initialPosition : (Vector2)transform.position;

        Vector2 targetA = basePos + moveDistance;
        Vector2 targetB = basePos - moveDistance;

        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(basePos, targetA);
        Gizmos.DrawLine(basePos, targetB);


    }
}
