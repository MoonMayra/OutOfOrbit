using UnityEngine;
using UnityEngine.Tilemaps;

public class BulletMovement : MonoBehaviour
{

    [SerializeField] private float bulletVel = 10;
    [SerializeField] private int bounces = 0;
    [SerializeField] private int maxBounces = 3;
    [SerializeField] private LayerMask bouncesMask;
    [SerializeField] private PlayerShoot playerShoot;

    private Rigidbody2D bulletRigidbody;
    public Vector2 direction;
    private string playerTag = "Character";


    private void Start()
    {
        bulletRigidbody = GetComponent<Rigidbody2D>();
        playerShoot = GameObject.Find(playerTag).GetComponent<PlayerShoot>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & bouncesMask.value) != 0)
        {
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
                }
            }
        }
    
    }

    public Vector2 Bounce(Vector2 direction, Vector2 normal)
    {
        if (Mathf.Abs(normal.y) > 0.9f) direction.y = -direction.y;
        if (Mathf.Abs(normal.x) > 0.9f) direction.x = -direction.x;
        return direction.normalized;

    }
    private void FixedUpdate()
    {
        if (bulletRigidbody == null)
            return;

        if(!playerShoot.isAbleToShoot && bulletRigidbody.linearVelocityX == 0 && bulletRigidbody.linearVelocityY == 0)
        {
            playerShoot.isAbleToShoot = true;
        }

        bulletRigidbody.linearVelocity = direction * bulletVel;
    }
}
