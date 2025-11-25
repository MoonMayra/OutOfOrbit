using UnityEngine;
using UnityEngine.UIElements;

public class MovementPlat : MonoBehaviour
{
    public enum MovementDirection
    {
        Left,
        Right,
        Up,
        Down
    }

    public enum PlatformType
    {
        Bounce,
        Limits
    }

    [SerializeField] private float speed = 2f;

    [SerializeField] private PlatformType type = PlatformType.Bounce;

    [SerializeField] private string bounceTag = "Ground";
    [SerializeField] private float minLimit;
    [SerializeField] private float maxLimit;

    [SerializeField] public MovementDirection initialDirection = MovementDirection.Right;

    private Vector2 direction;
    private Rigidbody2D platformRigidBody;
    private Vector2 startPosition;
    [SerializeField] private Transform bottomPos;
    private LineRenderer lineRenderer;

    public float tolerance = 0.001f;

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
        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer != null)
        {
            lineRenderer.positionCount = 2;
        }

    }

    private void FixedUpdate()
    {
        MovePlatform();

        if (type == PlatformType.Limits)
        {
            CheckLimits();
        }
    }

    private void MovePlatform()
    {
        platformRigidBody.linearVelocity = direction * speed;
    }

    private void CheckLimits()
    {
        if (initialDirection == MovementDirection.Left || initialDirection == MovementDirection.Right)
        {
            if (transform.position.x <= minLimit + tolerance || transform.position.x >= maxLimit - tolerance)
            {
                ChangeDirection();
            }
        }
        else
        {
            if (transform.position.y <= minLimit + tolerance || transform.position.y >= maxLimit - tolerance)
            {
                ChangeDirection();
            }
        }
    }

    private void ChangeDirection()
    {
        if (type == PlatformType.Bounce)
        {
            direction *= -1;
            return;
        }

        if (initialDirection == MovementDirection.Left || initialDirection == MovementDirection.Right)
        {
            float x = Mathf.Clamp(transform.position.x, minLimit, maxLimit);
            transform.position = new Vector3(x, transform.position.y, transform.position.z);
        }
        else
        {
            float y = Mathf.Clamp(transform.position.y, minLimit, maxLimit);
            transform.position = new Vector3(transform.position.x, y, transform.position.z);
        }

        direction *= -1;
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
        if (type != PlatformType.Bounce)
        {
            return;
        }
        
        if (collision.collider.CompareTag(bounceTag))
        {
            ChangeDirection();
        }
    }
    private void Update()
    {
        if (lineRenderer != null)
        {
            lineRenderer.SetPosition(0,transform.position);
            lineRenderer.SetPosition(1,bottomPos.position);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        if (initialDirection == MovementDirection.Left ||
            initialDirection == MovementDirection.Right)
        {
            Vector3 minPos = new Vector3(minLimit, transform.position.y, 0);
            Vector3 maxPos = new Vector3(maxLimit, transform.position.y, 0);

            Gizmos.DrawSphere(minPos, 0.1f);
            Gizmos.DrawSphere(maxPos, 0.1f);
            Gizmos.DrawLine(minPos, maxPos);
        }
        else
        {
            Vector3 minPos = new Vector3(transform.position.x, minLimit, 0);
            Vector3 maxPos = new Vector3(transform.position.x, maxLimit, 0);

            Gizmos.DrawSphere(minPos, 0.1f);
            Gizmos.DrawSphere(maxPos, 0.1f);
            Gizmos.DrawLine(minPos, maxPos);
        }
    }
}