using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour
{
    [Header("Traverse movement")]
    [SerializeField] private float maxVelocity = 1.0f;
    [SerializeField] private float accelTimeGround = 1.0f;
    [SerializeField] private float decelTimeGround = 1.0f;
    [SerializeField] private float accelTimeAir = 1.0f;
    [SerializeField] private float controlInAir = 1.0f;

    [Header("Jump")]
    [SerializeField] private float jumpHeight = 3.0f;
    [SerializeField] private float fallMultiplier = 1.0f;
    [SerializeField] private float hangTime = 1.0f;
    [SerializeField] private float hangGravityReduced = 1.0f;

    [Header("Assists")]
    [SerializeField] private float coyoteTime = 1.0f;
    [SerializeField] private float jumpNotOnGroundTime = 1.0f;
    [SerializeField] private float lastGroundedTime = 1.0f;

    [Header("Others")]
    [SerializeField] private PlayerGroundCheck groundCheck;
    [SerializeField] private PlayerGravityFields gravityFields;
    [SerializeField] private float wallNormalThreshold = 0.6f;
    public bool hitWall = false;

    [Header("Inputs")]
    [SerializeField] private InputActionReference moveInput;
    [SerializeField] private InputActionReference jumpInput;
    [SerializeField] private InputActionReference fieldsThrowInput;
    [SerializeField] private InputActionReference fieldsActivateInput;

    private Rigidbody2D playerRigidBody;

    private void Awake()
    {
        playerRigidBody = GetComponent<Rigidbody2D>();
        playerRigidBody.WakeUp();
        moveInput.action.started += HandleMoveInput;
        moveInput.action.performed += HandleMoveInput;
        moveInput.action.canceled += HandleMoveInput;

        jumpInput.action.performed += HandleJumpInput;

        fieldsThrowInput.action.performed += HandleThrowFieldInput;

        fieldsActivateInput.action.performed += HandleActivateFieldInput;
    }


    private void HandleMoveInput()
    {

    }

    private void HandleJumpInput()
    {

    }

    private void HandleThrowFieldInput()
    {

    }
    
    private void HandleActivateFieldInput()
    {

    }

    //Checks if you're hitting a wall
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<TilemapCollider2D>() == null)
            return;

        bool wallFounded = false;
        foreach(ContactPoint2D contactPoint in collision.contacts)
        {
            if (contactPoint.normal.x >= wallNormalThreshold)
            {
                wallFounded = true;
                break;
            }
            else if(contactPoint.normal.x <= -wallNormalThreshold)
            {
                wallFounded = true;
                break;
            }
            hitWall = wallFounded;
        }
    }

    //Process inputs
    void Update()
    {
        
    }

    //Process physisc
    private void FixedUpdate()
    {

    }
}
