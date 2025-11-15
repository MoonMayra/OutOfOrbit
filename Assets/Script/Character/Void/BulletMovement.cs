using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BulletMovement : MonoBehaviour
{

    [SerializeField] private float bulletVel = 10;
    [SerializeField] private int bounces = 0;
    [SerializeField] private int maxBounces = 3;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private LayerMask bulletMask;
    [SerializeField] public PlayerShoot playerShoot;
    [SerializeField] private string platforms;
    [SerializeField] private GravityVoid gravityFields;
    [SerializeField] private bool hasStopped = false;
    [SerializeField] private string hazardTag = "Hazard";

    private Rigidbody2D bulletRigidbody;
    public Vector2 direction;
    private string playerTag = "Character";
    public int index;


    private void Start()
    {
        bulletRigidbody = GetComponent<Rigidbody2D>();
        if (playerShoot == null)
        {
            var gameObject = GameObject.Find(playerTag);
            if (gameObject != null)
            {
                playerShoot = gameObject.GetComponent<PlayerShoot>();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & groundMask.value) != 0 || ((1 << collision.gameObject.layer) & bulletMask.value) != 0 )
        {
            if (collision.gameObject.CompareTag(platforms) == true)
            return;
            foreach (ContactPoint2D contactPoint in collision.contacts)
            {
                if (bounces < maxBounces)
                {
                    direction = Bounce(direction, contactPoint.normal);
                    bounces++;
                }
                else
                {
                    direction = new Vector2(0, 0);
                    playerShoot.CreateVoid();
                }
            }
            
        }
        if (collision.gameObject.CompareTag(hazardTag))
        {
            DestroyBulletsOnHazards();
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(hazardTag))
        {
            DestroyBulletsOnHazards();
        }
    }
    private void OnBecameInvisible()
    {
        bulletRigidbody.bodyType = RigidbodyType2D.Static;
        playerShoot.RemoveBullet(index,false);
        playerShoot.UpdateShootAvailability();
        Destroy(gameObject);
    }

    private void DestroyBulletsOnHazards()
    {
        bulletRigidbody.bodyType =RigidbodyType2D.Static;
        playerShoot.RemoveBullet(index,false);
        Debug.Log("Bullet destroyed on hazard");
        playerShoot.UpdateShootAvailability();
        Destroy(gameObject);
    }

    public Vector2 Bounce(Vector2 direction, Vector2 normal)
    {
        if(Mathf.Abs(normal.x)>Mathf.Abs(normal.y))
        {
            direction.x = -direction.x;
        }
        else
        {
            direction.y = -direction.y;
        }

        return direction.normalized;

    }
    private void FixedUpdate()
    {
        if (bulletRigidbody == null)
            return;

        Vector2 gvForces = Vector2.zero;
        if (playerShoot != null)
        {
            foreach (var voidObj in playerShoot.activeVoids)
            {
                if (voidObj != null)
                {
                    GravityVoid gv = voidObj.GetComponent<GravityVoid>();
                    if (gv != null)
                    {
                        gvForces += gv.CalculateGVForceBullet();
                    }
                }
            }
        }

        Vector2 bulletMov=direction * bulletVel + gvForces;
        if (bulletRigidbody.bodyType != RigidbodyType2D.Static)
        {
            bulletRigidbody.linearVelocity = bulletMov;
        }
        
        if(!hasStopped && (bulletRigidbody.linearVelocity.magnitude < 0.1f || direction==Vector2.zero))
        {
            hasStopped = true;

            bulletRigidbody.linearVelocity = Vector2.zero;
            bulletRigidbody.bodyType=RigidbodyType2D.Static;
            playerShoot.shootButtonRealesed = true;

            if (playerShoot != null)
            {
                playerShoot.OnBulletStopped(this);
            }
            else
            {
                Debug.LogWarning("PlayerShoot reference is missing in BulletMovement.");
            }
        }
      
    }
}
