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
        platformRigidBody.bodyType = RigidbodyType2D.Dynamic;
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
        Vector2 newPosition = platformRigidBody.position + direction * speed * Time.fixedDeltaTime;
        platformRigidBody.MovePosition(newPosition);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag(bounceTag))
        {
            direction *= -1;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            playerRigidBody = collision.collider.GetComponent<Rigidbody2D>();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            playerRigidBody = null;
        }
    }
}
