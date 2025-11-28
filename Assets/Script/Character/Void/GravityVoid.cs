using UnityEngine;

public class GravityVoid : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float gravityStrength = 10f;
    [SerializeField] private float gravityStrengthObject = 2f;
    [SerializeField] private float voidRadius = 5f;
    [SerializeField] private float lifetime = 3.0f;

    [Header("References")]
    public GameObject linkedBullet;
    [SerializeField] private LayerMask player;
    [SerializeField] private LayerMask bullet;
    [SerializeField] private LayerMask objects;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip spawnClip;

    private float timer = 0f;
    private Transform playerTransform;
    private Rigidbody2D playerRigidBody;
    private Transform bulletTransform;
    private Rigidbody2D bulletRigidBody;
    private Transform objectsTransform;
    private Rigidbody2D objectsRigidBody;

    private void Start()
    {
        if (audioSource != null && spawnClip != null)
            audioSource.PlayOneShot(spawnClip);
    }

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
        if (((1 << collision.gameObject.layer) & objects.value) !=0)
        { 
            objectsRigidBody= collision.GetComponent<Rigidbody2D>();
            objectsTransform = collision.GetComponent<Transform>();
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

    private Vector2 CalculateGVForce(Transform target)
    {
        if(target == null)
        {
            return Vector2.zero;
        }
        Vector2 direction = (Vector2) transform.position-(Vector2)target.position;
        float distance= direction.magnitude;
        if (distance >= voidRadius)
        {
            return Vector2.zero ;
        }
        direction.Normalize();
        float strenght=gravityStrength*(1-(distance/voidRadius));
        return direction*strenght*Time.fixedDeltaTime;

    }
    private Vector2 CalculateGVForceObject(Transform target)
    {
        if (target == null)
        {
            return Vector2.zero;
        }
        Vector2 direction = (Vector2)transform.position - (Vector2)target.position;
        float distance = direction.magnitude;
        if (distance >= voidRadius)
        {
            return Vector2.zero;
        }
        direction.Normalize();
        float strenght = gravityStrengthObject * (1 - (distance / voidRadius));
        return direction * strenght * Time.fixedDeltaTime;

    }



    public Vector2 CalculateGVForcePlayer()
    {
        Vector2 force = Vector2.zero;

        if (playerTransform == null || playerRigidBody == null)
        {
            return Vector2.zero;
        }

        return CalculateGVForce(playerTransform);
    }

    public Vector2 CalculateGVForceBullet()
    {
        Vector2 force = Vector2.zero;

        if (bulletTransform == null || bulletRigidBody == null)
        {
            return Vector2.zero;
        }

        return CalculateGVForce(bulletTransform);
    }
    public Vector2 CalculateGVForceObjects()
    {
        Vector2 force = Vector2.zero;

        if (objectsTransform == null || objectsRigidBody == null)
        {
            return Vector2.zero;
        }

        return CalculateGVForceObject(objectsTransform);
    }


    private void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;

        if (objectsRigidBody!=null)
        {
            objectsRigidBody.linearVelocity += CalculateGVForceObjects();
        }

        if (timer >= lifetime)
        {
            DestroyVoid();
        }
    }

    private void DestroyVoid()
    {
        if (linkedBullet != null)
            Destroy(linkedBullet);

        Destroy(gameObject);
    }
}

