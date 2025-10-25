using UnityEngine;

public class MovementPlat : MonoBehaviour
{
    [SerializeField] private float speed = 2f;
    [SerializeField] private string bounceTag = "Ground";

    private Vector2 startPosition;
    private Vector2 direction = Vector2.right;
    private Rigidbody2D platformRigidBody;
    private Rigidbody2D playerRigidBody;

    private void Awake()
    {
        platformRigidBody = GetComponent<Rigidbody2D>();
        platformRigidBody.bodyType = RigidbodyType2D.Kinematic;
    }

    private void Start()
    {
        startPosition = transform.position;
    }

    private void FixedUpdate()
    {
        MovePlatform();
    }

    private void MovePlatform()
    {
        platformRigidBody.linearVelocity = direction * speed;

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
