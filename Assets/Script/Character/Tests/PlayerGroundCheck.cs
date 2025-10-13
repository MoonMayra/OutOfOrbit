using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerGroundCheck : MonoBehaviour
{
    [Header("Player state")]
    public bool isGrounded = false;
    public bool justLanded = false;
    public bool justLeftGround = false;
    public bool wasGrounded = false;
    public float jumpingThreshold = 0.0f;

    [Header("Parameters")]
    [SerializeField] private float normalThreshold = 0.6f;

    [Header("Particles")]
    [SerializeField] private ParticleSystem dustParticles;

    private void OnCollisionStay2D(Collision2D collisionObj)
    {
        if (collisionObj.gameObject.GetComponent<TilemapCollider2D>()==null)
        {
            return;
        }
        bool groundFound = false;
        foreach(ContactPoint2D contactPoint in collisionObj.contacts)
        {
            if(contactPoint.normal.y >= normalThreshold)
            {
                groundFound = true; 
                break;
            }
        }
        isGrounded = groundFound;
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<TilemapCollider2D>() == null)
        {
            return;
        }
        isGrounded= false;

    }

    private void Update()
    {
        if(wasGrounded && !isGrounded)
        {
            justLeftGround = true;
            Debug.Log("sali del piso");
            if (dustParticles != null)
            {
                dustParticles.Stop();
            }
        }
        if(!wasGrounded && isGrounded)
        {
            justLanded = true;
            Debug.Log("entre al piso");
            dustParticles.transform.position = new Vector2(transform.position.x, transform.position.y - 0.5f);
            if (dustParticles != null)
            {
                dustParticles.Play();
            }
        }
        wasGrounded = isGrounded;

        if (isGrounded)
        {
            jumpingThreshold = 0.0f;
        }

        if(!isGrounded)
        {
            jumpingThreshold += Time.deltaTime;
        }

    }

    private void LateUpdate()
    {
        justLanded=false;
        justLeftGround=false;
    }
}
