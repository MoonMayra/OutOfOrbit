using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerGroundCheck : MonoBehaviour
{
    [Header("Player state")]
    public bool isGrounded = false;
    public bool justLanded = false;
    public bool justLeftGround = false;
    public bool wasGrounded = false;

    [Header("Parameters")]
    [SerializeField] private float normalThreshold = 0.6f;

    private void OnCollisionStay2D(Collision2D collisionObj)
    {
        if (collisionObj.gameObject.GetComponent<TilemapCollider2D>()==null)
        {
            return;
        }
        bool groundFounded = false;
        foreach(ContactPoint2D contactPoint in collisionObj.contacts)
        {
            if(contactPoint.normal.y >= normalThreshold)
            {
                groundFounded = true; 
                break;
            }
        }
        isGrounded = groundFounded;
    }



    private void Update()
    {
        if(wasGrounded && !isGrounded)
        {
            justLeftGround = true;
        }
        if(!wasGrounded && isGrounded)
        {
            justLanded = true;
        }
        wasGrounded = isGrounded;
    }

    private void LateUpdate()
    {
        justLanded=false;
        justLeftGround=false;
    }
}
