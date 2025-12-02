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
    public float wallNormalThreshold = 0.6f;

    [Header("Particles")]
    [SerializeField] private ParticleSystem dustParticles;

    [Header("Ground layer")]
    [SerializeField] private LayerMask groundLayer;


    private void OnCollisionStay2D(Collision2D collisionObj)
    {
        if (((1 << collisionObj.gameObject.layer) & groundLayer.value) == 0)
            return;

        bool groundFound = false;
        foreach (ContactPoint2D contactPoint in collisionObj.contacts)
        {
            Vector2 normal = contactPoint.normal;
            if(Mathf.Abs(normal.x)>Mathf.Abs(normal.y))
            {
                continue;
            }
            if(normal.y > 0f && Mathf.Abs(normal.y) > Mathf.Abs(normal.x))
            {
                groundFound = true;
                break;
            }
        }
        if (groundFound)
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & groundLayer.value) == 0)
            return;

        isGrounded = false;
    }

    private void Update()
    {
        if(wasGrounded && !isGrounded)
        {
            justLeftGround = true;
            if (dustParticles != null)
            {
                dustParticles.Stop();
            }
        }
        if(!wasGrounded && isGrounded)
        {
            justLanded = true;
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
