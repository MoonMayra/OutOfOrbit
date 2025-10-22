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
    [SerializeField] private LayerMask bullet;

    private float timer = 0f;
    private Transform playerTransform;
    private Rigidbody2D playerRigidBody;
    private Transform bulletTransform;
    private Rigidbody2D bulletRigidBody;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & player.value) != 0)
        {
            playerRigidBody = collision.GetComponent<Rigidbody2D>();
            playerTransform = collision.GetComponent<Transform>();
        }
        if (((1 << collision.gameObject.layer) & bullet.value) != 0)
            {
                bulletRigidBody = collision.GetComponent<Rigidbody2D>();
                bulletTransform = collision.GetComponent<Transform>();
            }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & player.value) != 0)
        {
            playerRigidBody = null;
            playerTransform = null;
        }
        if (((1 << collision.gameObject.layer) & bullet.value) != 0)
        {
                bulletRigidBody = null;
                bulletTransform = null;
        }
    }
        

    public Vector2 CalculateGVForcePlayer()
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
    public Vector2 CalculateGVForceBullet()
    {
        Vector2 force = Vector2.zero;
        if (bulletTransform != null && bulletRigidBody != null)
        {
            Vector2 direction = (Vector2)transform.position - (Vector2)bulletTransform.position;
            float distance = direction.magnitude;
            if (distance < voidRadius)
            {
                direction.Normalize();
                float strength = gravityStrength * (1 - (distance / voidRadius));
                force += direction * strength * Time.fixedDeltaTime;
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



}
