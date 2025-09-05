using UnityEngine;
using UnityEngine.InputSystem;

public class NewMovement : MonoBehaviour
{
    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private InputActionReference jumpAction;
    [SerializeField] private InputActionReference createGFieldAction;
    [SerializeField] private float fieldActiveTime;
    [SerializeField] private GameObject GravityField;   

    [SerializeField] private float MaxSpeed = 5f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float acceleration = 10f;
    [SerializeField] private float deceleration = 10f;
    [SerializeField] private float maxFallSpeed = -20f;
    [SerializeField] private float friction = 0.1f;
    [SerializeField] private float hangTimeGravity= -1.0f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private int counterJump = 0;
    [SerializeField] private int maxJump = 50;

    private Vector3 mousePos;
    private Vector3 worldPos;
    private bool isGrounded;
    private Vector2 moveX;
    private Rigidbody2D rbPlayer;
    private int fieldCount = 0;



    private void Start()
    {
        rbPlayer = GetComponent<Rigidbody2D>();
        if(rbPlayer==null)
        {
            Debug.Log("Rigidbody component not found on the GameObject.");
        }
        rbPlayer.WakeUp();
        moveAction.action.started += HandleMoveInput;
        moveAction.action.performed += HandleMoveInput;
        moveAction.action.canceled += HandleMoveInput;

        jumpAction.action.performed += HandleJumpInput;


        createGFieldAction.action.performed += HandleFieldInput;
    }
    private void createField()
    {
        if(fieldCount==0)
        {
            Instantiate(GravityField, worldPos, Quaternion.identity);
            fieldCount++;
        }
        else 
        {
            Object.Destroy(GameObject.FindWithTag("Voids"));
            Instantiate(GravityField, worldPos, Quaternion.identity);

        }
    }
    private void HandleMoveInput(InputAction.CallbackContext context)
    {
        var moveInput = context.ReadValue<Vector2>();
        moveX = moveInput;
        if(Mathf.Abs(moveX.x)<MaxSpeed)
        {
            if(moveX.x>0)
            {
                moveX.x += acceleration * Time.deltaTime;
            }
            else if (moveX.x < 0)
            {
                moveX.x -= acceleration * Time.deltaTime;
            }
        }
        if(context.canceled)
        {
            if (Mathf.Abs(moveX.x) > 0)
            {
                moveX.x -= deceleration * Time.deltaTime;
            }
        }

    }

    private void HandleJumpInput(InputAction.CallbackContext context)
    {
        if(context.performed && isGrounded)
        {
            rbPlayer.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            isGrounded = false;
        }
    }

    private void HandleFieldInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            mousePos = Mouse.current.position.ReadValue();
            worldPos = Camera.main.WorldToScreenPoint(mousePos);
            worldPos.z = 0;
            createField();

        }


    }

    private void Update()
    {

    }


    private void calculateGravity()
    {
        rbPlayer.AddForce(new Vector2(0, gravity));
        if(counterJump>=50)
        {
            gravity = -hangTimeGravity;
            if(rbPlayer.linearVelocity.y < maxFallSpeed)
            {
                rbPlayer.linearVelocity = new Vector2(rbPlayer.linearVelocity.x, maxFallSpeed);
            }
        }
        else
        {
            gravity = -9.81f;
        }
    }

    private void moveCharacter()
    {
        rbPlayer.linearVelocity = new Vector2(moveX.x, rbPlayer.linearVelocity.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }

    }

    private void FixedUpdate()
    {
        if(isGrounded)
        {
            counterJump = 0;
        }
        else
        {
            counterJump++;
        }
        calculateGravity();
        moveCharacter();


    }

}
