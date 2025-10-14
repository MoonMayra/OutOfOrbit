using UnityEngine;
using UnityEngine.Tilemaps;

public class BulletMovement : MonoBehaviour
{

    [SerializeField] private float bulletVel = 10;
    [SerializeField] private int bounces = 0;
    [SerializeField] private int maxBounces = 3;
    [SerializeField] private float glowVel = 3;
    [SerializeField] private float rayLenght = 0.1f;

    private Rigidbody2D bulletRigidbody;
    private Light glow;
    private LayerMask bouncesMask;
    public Vector2 direction;
  
    
    private void Start()
    {
        bulletRigidbody = GetComponent<Rigidbody2D>();
        glow = GetComponentInChildren<Light>();
    }


    private void Update()
    {
        
    }
    /*public void SetTrayectory(Vector2 startPos, Vector2 lineDirection)
    {

        Vector2 currentPos = startPos;
        Vector2 currentDir = lineDirection.normalized;

        for (int i = 0; i < maxBounces; i++)
        {
            RaycastHit2D hit = Physics2D.Raycast(currentPos, currentDir, rayLenght, bouncesMask);
            if (hit.collider)
            {

                if (i == maxBounces)
                {
                    bulletRigidbody.linearVelocity = new Vector2(0, 0);
                    break;
                }
                currentDir = Bounce(currentDir, hit.normal);
                currentPos = hit.point + currentDir * 0.01f;
                bounces++;
            }
            else
            {
                bulletRigidbody.linearVelocity = new Vector2 (0, 0);
                break;
            }
        }*/

    


    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.GetComponent<TilemapCollider2D>() == null)
            return;

        foreach(ContactPoint2D contactPoint in collision.contacts)
        {
            if (bounces < maxBounces)
            {
                direction=Bounce(direction, contactPoint.normal);
                bounces++;
            }
            else
            { 
                direction = new Vector2(0, 0);
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

        bulletRigidbody.linearVelocity = direction * bulletVel;
    }
}
