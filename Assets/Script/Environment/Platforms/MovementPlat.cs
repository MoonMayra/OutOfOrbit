using UnityEngine;

public class MovementPlat : MonoBehaviour
{
    public enum MovementDirection
    {
        Left,
        Right,
        Up,
        Down
    }
    [SerializeField] private float speed = 2f;
    [SerializeField] private string bounceTag = "Ground";
    [SerializeField] public MovementDirection initialDirection = MovementDirection.Right;

    private Vector2 direction;
    private Rigidbody2D platformRigidBody;
    private Vector2 startPosition;

    private void Awake()
    {
        platformRigidBody = GetComponent<Rigidbody2D>();
        platformRigidBody.bodyType = RigidbodyType2D.Kinematic;
        startPosition = transform.position;
        direction = initialDirection switch
        {
            MovementDirection.Left => Vector2.left,
            MovementDirection.Right => Vector2.right,
            MovementDirection.Up => Vector2.up,
            MovementDirection.Down => Vector2.down,
            _ => Vector2.right,
        };
    }

    private void FixedUpdate()
    {
        MovePlatform();
    }

    private void MovePlatform()
    {
        platformRigidBody.linearVelocity = direction * speed;

    }
    public void ResetPlatform()
    {
        transform.position = startPosition;
        direction = initialDirection switch
        {
            MovementDirection.Left => Vector2.left,
            MovementDirection.Right => Vector2.right,
            MovementDirection.Up => Vector2.up,
            MovementDirection.Down => Vector2.down,
            _ => Vector2.right,
        };
        platformRigidBody.linearVelocity = Vector2.zero;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag(bounceTag))
        {
            platformRigidBody.linearVelocity = Vector2.zero;
            direction *= -1;
        }
    }

}
