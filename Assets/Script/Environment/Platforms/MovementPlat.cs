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
    [SerializeField] private float waitTime = 0.5f;
    private float timer = 0f;
    private bool isWaiting = false;

    public float tolerance = 0.001f;
    private Vector2 prevVel;

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
        if (isWaiting)
        {
            timer += Time.fixedDeltaTime;
            platformRigidBody.linearVelocity = Vector2.zero;

            if (timer >= waitTime)
            {
                timer = 0f;
                isWaiting = false;

                direction *= -1;
                platformRigidBody.linearVelocity = direction * speed;
            }
            return;
        }

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
            if (transform.position.x <= minLimit + tolerance && direction.x < 0f)
            {
                transform.position = new Vector2(minLimit, transform.position.y);
                ChangeDirection();
            }
            else if (transform.position.x >= maxLimit - tolerance && direction.x > 0f)
            {
                transform.position = new Vector2(maxLimit, transform.position.y);
                ChangeDirection();
            }
        }
        else
        {
            if (transform.position.y <= minLimit + tolerance && direction.y < 0f)
            {
                transform.position = new Vector2(transform.position.x, minLimit);
                ChangeDirection();
            }
            else if (transform.position.y >= maxLimit - tolerance && direction.y > 0f)
            {
                transform.position = new Vector2(transform.position.x, maxLimit);
                ChangeDirection();
            }
        }
    }

    private void ChangeDirection()
    {
        if (type == PlatformType.Bounce)
        {
            direction *= -1;
            platformRigidBody.linearVelocity = direction * speed;
            return;

        }
        isWaiting = true;
        timer = 0f;
        platformRigidBody.linearVelocity=Vector2.zero;
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