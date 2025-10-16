using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

//NOTE 1: To keep jump speed and height spearate and editable, we use the free fall formula
//V^2=(V0)^2+2*a*deltaY, where a=-gravity, and deltaY=maximum jump height
//Using this, we solve for gravity to get the following formula: gravity=(V0)^2/(2*h)
//Using this formula, we can caluculate the gravity needed for the desired jump height and speed
//Once we have this value, we apply it only when the player is jumping.
public class PlayerMovement : MonoBehaviour
{
    [Header("Traverse movement")]

    [Tooltip("Maximum velocity of the player.")]
    [SerializeField] private float maxVelocity = 1.0f;
    [Tooltip("Time for the player to reach maximum velocity.")]
    [SerializeField] private float accelTimeGround = 1.0f;
    [Tooltip("Time for the player to stop from maximum velocity.")]
    [SerializeField] private float decelTimeGround = 1.0f;
    [Tooltip("Time for the player to reach maximum velocity on the air.")]
    [SerializeField] private float accelTimeAir = 1.0f;
    [Tooltip("Multiplies velocity on the air. For same velocity as on ground, use 1; for less velocity use 0; and for more use a number bigger than 1. ")]
    [SerializeField] private float controlInAir = 1.0f;

    [Header("Jump")]

    [Tooltip("Maximum height the player can reach when jumping.")]
    [SerializeField] private float jumpHeight = 3.0f;
    [Tooltip("Inicial Velocity the player has when jumping.")]
    [SerializeField] private float jumpInicialSpeed = 3.0f;
    [Tooltip("By how much gravity is increased when falling down.")]
    [SerializeField] private float fallMultiplier = 1.0f;
    [Tooltip("Time at the highest point where gravity is reduced.")]
    [SerializeField] private float hangTime = 1.0f;
    [Tooltip("By how much gravity is reduced when at the highest point.")]
    [SerializeField] private float hangGravityReduced = 1.0f;
    [Tooltip("Gravity force applied to the player.")]
    [SerializeField] private float gravity = 9.81f;

    [Header("Assists")]

    [Tooltip("Time after leaving a platform that the player can still jump.")]
    [SerializeField] private float coyoteTime = 1.0f;
    [Tooltip("Time before landing that a jump input is obtained.")]
    [SerializeField] private float jumpNotOnGroundTime = 1.0f;

    [Header("Others")]

    
    [SerializeField] private PlayerGroundCheck groundCheck;
    [SerializeField] private GravityVoid gravityVoid;
    [SerializeField] private PlayerView playerView;
    [SerializeField] private PlayerShoot playerShoot;
    [SerializeField] private float wallNormalThreshold = 0.6f;
    public bool hitWall = false;

    [Header(" Inputs")]

    [SerializeField] private InputActionReference moveInput;
    [SerializeField] private InputActionReference jumpInput;
    [SerializeField] private InputActionReference fieldsThrowInput;

    private Rigidbody2D playerRigidBody;
    private float moveInputX = 0.0f;
    private float targetVelX=0.0f;
    private float jumpingThreshold = 0.0f;
    private float jumpBufferTimer = 0.0f;
    private bool jumpBufferActive = false;

    private void Awake()
    {
        playerRigidBody = GetComponent<Rigidbody2D>();

        moveInput.action.started += HandleMoveInput;
        moveInput.action.performed += HandleMoveInput;
        moveInput.action.canceled += HandleMoveInput;

        jumpInput.action.performed += HandleJumpInput;

    }

    private void Start()
    {
        playerRigidBody.WakeUp();
    }

    private void HandleMoveInput(InputAction.CallbackContext context)
    {
        var input = context.ReadValue<Vector2>();
        moveInputX = input.x;
    }

    private void HandleJumpInput(InputAction.CallbackContext context)
    {
        jumpBufferTimer = 0.0f;
        if (CanJump())
        {
            PerformJump();
        }
        else
        {
            jumpBufferActive = true;
        }
    }

    //Checks if you're hitting a wall
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<TilemapCollider2D>() == null)
            return;

        bool wallFound = false;
        foreach(ContactPoint2D contactPoint in collision.contacts)
        {
            if (contactPoint.normal.x >= wallNormalThreshold)
            {
                wallFound = true;
                break;
            }
            else if(contactPoint.normal.x <= -wallNormalThreshold)
            {
                wallFound = true;
                break;
            }
            
        }
        hitWall = wallFound;
    }

    private bool CanJump()
    {
        if (groundCheck.isGrounded)
        {
            return true;
        }
        if(jumpingThreshold<=coyoteTime)
        {
            return true;
        }
        return false;
    }

    private void PerformJump()
    {
        groundCheck.isGrounded = false;

        Vector2 velocityPlayer = playerRigidBody.linearVelocity;
        velocityPlayer.y = jumpInicialSpeed;
        playerRigidBody.linearVelocity = velocityPlayer;

        jumpingThreshold = 0.0f;
        jumpBufferActive = false;
    }

    private void JumpBufferHandle()
    {
        if (jumpBufferActive)
        {
            jumpBufferTimer += Time.deltaTime;
            if (groundCheck.isGrounded && jumpBufferTimer <= jumpNotOnGroundTime)
            {
                PerformJump();
                jumpBufferActive = false;
            }
            else if (jumpBufferTimer > jumpNotOnGroundTime)
            {
                jumpBufferActive = false;
            }
        }
    }

    //Process inputs
    private void Update()
    {
        jumpingThreshold = groundCheck.jumpingThreshold;

        if(hangTime<0)
        {
            hangTime = 1.0f;
        }
        
        JumpBufferHandle();

        playerView.UpdateDirection(moveInputX);
    }

    //Process physisc
    private void FixedUpdate()
    {
        // Traverse movement
        targetVelX = maxVelocity * moveInputX;
        Vector2 velocityPlayer = playerRigidBody.linearVelocity;

        if (groundCheck.isGrounded)
        {
            float accel;
            if (Mathf.Abs(targetVelX) > 0.01f)
            {
                accel = maxVelocity / accelTimeGround;
            }
            else
            {
                accel = maxVelocity / decelTimeGround;
            }
            velocityPlayer.x = Mathf.MoveTowards(velocityPlayer.x, targetVelX, accel * Time.fixedDeltaTime);
        }
        else
        {
            float accel = maxVelocity / accelTimeAir;
            velocityPlayer.x = Mathf.MoveTowards(velocityPlayer.x, targetVelX * controlInAir, accel * Time.fixedDeltaTime);
        }

        //Gravity
        if(!groundCheck.isGrounded)
        {
            float actualGravity;
     
            if (velocityPlayer.y > 0 && hangTime > 0)
            {
                float jumpGravity = (jumpInicialSpeed * jumpInicialSpeed) / (2f * jumpHeight); //NOTE 1: For reference on how it works, read the comment at the beginning of the code.
                actualGravity = jumpGravity;
                velocityPlayer.y -= actualGravity * hangGravityReduced * Time.fixedDeltaTime;
                hangTime -= Time.fixedDeltaTime;
            }
            else
            {
                velocityPlayer.y -= gravity * fallMultiplier * Time.fixedDeltaTime;
            }
        }
        //Add gravity fields effect
        Vector2 gvForces=Vector2.zero;
        foreach(var voidObj in playerShoot.activeVoids)
        {
            if(voidObj!=null)
            {
                GravityVoid gv = voidObj.GetComponent<GravityVoid>();
                if(gv!=null)
                {
                    gvForces += gv.CalculateGVForce();
                }
            }
        }

        velocityPlayer += gvForces;

        playerRigidBody.linearVelocity= velocityPlayer;
    }
}