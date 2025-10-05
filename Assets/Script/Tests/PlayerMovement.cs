using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour
{
    [Header("Traverse movement")]

    [Tooltip("Maximum velocity of the player.")]
    [SerializeField] private float maxVelocity = 1.0f;
    [Tooltip("Tiempo que tarda el personaje en alcanzar la velocidad maxima.")]
    [SerializeField] private float accelTimeGround = 1.0f;
    [Tooltip("Tiempo que tarda el personaje en frenar desde la velocidad maxima.")]
    [SerializeField] private float decelTimeGround = 1.0f;
    [Tooltip("Tiempo que tarda el personaje en alcanzar la velocidad maxima cuando esta en el aire.")]
    [SerializeField] private float accelTimeAir = 1.0f;
    [Tooltip("Multiplica la velocidad a la que se mueve en el aire, para tener la misma velocidad que en el suelo utilizar 1, para menor velocidad utilizar un numero menor a 1, y para tener mayor velocidad un numero mayor.")]
    [SerializeField] private float controlInAir = 1.0f;

    [Header("Jump")]
    [Tooltip("Altura a la que salta el personaje.")]
    [SerializeField] private float jumpHeight = 3.0f;
    [Tooltip("Velocidad a la que salta el personaje.")]
    [SerializeField] private float jumpVel = 1.0f;
    [Tooltip("Multiplicador que determina que tan rapido cae el personaje.")]
    [SerializeField] private float fallMultiplier = 1.0f;
    [Tooltip("Tiempo que el personaje se mantiene en el aire despues de llegar a la altura maxima.")]
    [SerializeField] private float hangTime = 1.0f;
    [Tooltip("Cuanto se reduce la gravedad en el punto mas alto.")]
    [SerializeField] private float hangGravityReduced = 1.0f;
    [Tooltip("Gravedad del perrsonaje en .")]
    [SerializeField] private float gravity = 9.81f;

    [Header("Assists")]
    [SerializeField] private float coyoteTime = 1.0f;
    [SerializeField] private float jumpNotOnGroundTime = 1.0f;

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
    private float moveInputX = 0.0f;
    private float targetVelX=0.0f;
    private float lastGroundedTime = 0.0f;

    private void Awake()
    {
        playerRigidBody = GetComponent<Rigidbody2D>();

        moveInput.action.started += HandleMoveInput;
        moveInput.action.performed += HandleMoveInput;
        moveInput.action.canceled += HandleMoveInput;

        jumpInput.action.performed += HandleJumpInput;

        //fieldsThrowInput.action.performed += HandleThrowFieldInput;

        //fieldsActivateInput.action.performed += HandleActivateFieldInput;
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
        if (CanJump())
        {
            PerformJump();
            
        }
    }

    //private void HandleThrowFieldInput(InputAction.CallbackContext context)
    //{

    //}
    
    //private void HandleActivateFieldInput(InputAction.CallbackContext context)
    //{

    //}

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
            hitWall = wallFound;
        }
    }

    private bool CanJump()
    {
        float jumpTime=Time.time - lastGroundedTime;

        if (groundCheck.isGrounded)
        {
            return true;
        }
        if(jumpTime<=coyoteTime)
        {
            return true;
        }
        if(jumpTime<=jumpNotOnGroundTime)
        {
            return true;
        }
        return false;
    }

    private void PerformJump()
    {
        groundCheck.isGrounded = false;
        float jumpVelFinal = Mathf.Sqrt(2f * jumpVel * jumpHeight);

        Vector2 velocityPlayer = playerRigidBody.linearVelocity;
        velocityPlayer.y = jumpVelFinal;
        playerRigidBody.linearVelocity = velocityPlayer;

        lastGroundedTime = -1.0f;

    }

    //Process inputs
    void Update()
    {
        lastGroundedTime = groundCheck.lastGroundedTime;

        if(hangTime<0)
        {
            hangTime = 1.0f;
        }
    }

    //Process physisc
    private void FixedUpdate()
    {
        // TRaverse movement
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
            if(velocityPlayer.y > 0 && hangTime > 0)
            {
                velocityPlayer.y-=gravity*hangGravityReduced*Time.fixedDeltaTime;
                hangTime-=Time.fixedDeltaTime;
            }
            else
            {
                velocityPlayer.y-=gravity*fallMultiplier*Time.fixedDeltaTime;
            }
        }

        playerRigidBody.linearVelocity= velocityPlayer;


    }
}
