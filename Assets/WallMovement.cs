using Unity.VisualScripting;
using UnityEngine;
using System.Collections;
using UnityEngine.Tilemaps;

public class WallMovement : MonoBehaviour
{
    public enum WallSide
    {
        Left,
        Right,
        Top,
        Bottom
    }
    [SerializeField] private WallSide wallSide;
    private Rigidbody2D rigidBody;
    private Vector3 initialPosition;
    private Coroutine moveCoroutine;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        initialPosition = transform.position;
    }
    public Vector3 GetInitialPosition()
    {
        return initialPosition;
    }
    public WallSide GetWallSide()
    {
        return wallSide;
    }
    public void MoveTo(Vector3 targetPos, float duration)
    {
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }
        moveCoroutine = StartCoroutine(MoveOverTime(targetPos, duration));
    }
    public void ReturnToInitialPosition(float duration)
    {
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }
        moveCoroutine = StartCoroutine(MoveOverTime(initialPosition, duration));
    }
    public IEnumerator MoveOverTime(Vector3 targetPos, float duration)
    {
        rigidBody.linearVelocity=Vector2.zero;

        float distance = Vector2.Distance(transform.position, targetPos);
        float speed = distance / duration;

        Vector2 direction = (targetPos - transform.position).normalized;
        rigidBody.linearVelocity = direction * speed;

        while (Vector2.Distance(transform.position,targetPos)>0.02f)
        {
            yield return null;
        }

        rigidBody.linearVelocity = Vector2.zero;
        transform.position = targetPos;

        moveCoroutine = null;
    }

}
