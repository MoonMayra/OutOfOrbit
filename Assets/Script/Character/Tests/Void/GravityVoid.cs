using UnityEngine;

public class GravityVoid : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float gravityStrength = 10f;
    [SerializeField] private float voidRadius = 5f;
    [SerializeField] private float lifetime = 3.0f;

    [Header("References")]
    public GameObject linkedBullet;
    [SerializeField] private LayerMask player;

    private float timer = 0f;
    private Transform playerTransform;
    private Rigidbody2D playerRigidBody;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & player.value) != 0)
        {
            playerRigidBody = collision.GetComponent<Rigidbody2D>();
            playerTransform = collision.GetComponent<Transform>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & player.value) != 0)
        {
            playerRigidBody = null;
            playerTransform = null;
        }
    }
        

    public Vector2 CalculateGVForce()
    {
        Vector2 force = Vector2.zero;
            if (playerTransform != null && playerRigidBody != null)
            {
                Vector2 direction = (Vector2)transform.position - (Vector2)playerTransform.position;
                float distance = direction.magnitude;

                if (distance < voidRadius)
                {
                    direction.Normalize();
                    float strength = gravityStrength * (1 - (distance / voidRadius));
                    force = direction * strength * Time.fixedDeltaTime;
            }
            }
        return force;
    }

    private void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;
        if (timer>=lifetime)
        {
            DestroyVoid();
        }
    }

    private void DestroyVoid()
    {
        if (linkedBullet != null)
        {
            Destroy(linkedBullet);
        }
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, voidRadius);
    }

}
