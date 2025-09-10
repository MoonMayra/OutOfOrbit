using UnityEngine;
using UnityEngine.InputSystem;

public class player : MonoBehaviour
{

    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private InputActionReference JumpAction;
    [SerializeField] private InputActionReference AttackAction;
    [SerializeField] private InputActionReference VoidAction;

    [SerializeField] private GameObject[] GravityFields = new GameObject[3];
    [SerializeField] private float fallSpeed = 5.0f;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float gravity = -9.81f;
    private Vector3 mousePos;
    private Vector3 worldPos;
    [SerializeField] private bool isGrounded = true;
    private bool isJumping = false;
    private bool isRunning = false;
    private bool isFalling = false;
    private bool isShooting = false;
    private Vector2 moveX;
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private GameObject[] activeVoids = new GameObject[3];
    private int nextVoidIndex = 0;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody component not found on the GameObject.");
        }
        rb.WakeUp();
        moveAction.action.started += HandleMoveInput;
        moveAction.action.performed += HandleMoveInput;
        moveAction.action.canceled += HandleMoveInput;

        JumpAction.action.performed += HandleJumpInput;


        VoidAction.action.performed += HandleVoidInput;

    }

    public void CreateField()
    {
        if (activeVoids[nextVoidIndex] != null) // if there's an active void in this slot, destroy it :p
        {
            Destroy(activeVoids[nextVoidIndex]);

        }

        GameObject newVoid = null;
        switch (nextVoidIndex)
        {
            case 0:
                newVoid = Instantiate(GravityFields[0], worldPos, Quaternion.identity);
                break;
            case 1:
                newVoid = Instantiate(GravityFields[1], worldPos, Quaternion.identity);
                break;
            case 2:
                newVoid = Instantiate(GravityFields[2], worldPos, Quaternion.identity);
                break;
        }

        activeVoids[nextVoidIndex] = newVoid;

        nextVoidIndex = (nextVoidIndex + 1) % 3; // infinite ! :D
    }

    private void HandleVoidInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            mousePos = Mouse.current.position.ReadValue();
            worldPos = Camera.main.ScreenToWorldPoint(mousePos);
            worldPos.z = 0;
            isShooting = true;
            CreateField();
        }
    }



    private void HandleMoveInput(InputAction.CallbackContext context)
    {
      var moveInput = context.ReadValue<Vector2>();
        Debug.Log("Move Input: " + moveInput);
        moveX = moveInput * moveSpeed;
    }

    private void HandleJumpInput(InputAction.CallbackContext context)
    {
      if(context.performed && isGrounded)
        {
            rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            isGrounded = false;
            animator.SetInteger("state", 2);
            Debug.Log("Jump Input Detected");

        }

    }
    private void HandleAnimations()
    {

        animator.SetBool("isJumping", isJumping);
        animator.SetBool("isFalling", isFalling);
        animator.SetBool("isShooting", isShooting);
        animator.SetBool("isRunning", isRunning);


    }


    private void Update()
    {

        rb.linearVelocity = new Vector2(moveX.x, rb.linearVelocity.y);
        HandleAnimations();

        if (isShooting)
        {
            isShooting = false;
        }



        if (moveX.x > 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (moveX.x < 0)
        {
            spriteRenderer.flipX = false;
        }

        if (isGrounded)
        {

            if (moveX.x != 0)
            {
                isFalling = false;
                isJumping = false;
                isRunning = true;
            }
            else if (moveX.x == 0)
            {
                isFalling = false;
                isJumping = false;
                isRunning = false;
            }
        }
        else
        {
            if (rb.linearVelocity.y > 0)
            {
                isJumping = true;
                isFalling = false;
                isRunning = false;
            }
            else if (rb.linearVelocity.y < 0)
            {
                isJumping = false;
                isFalling = true;
                isRunning = false;

                Debug.Log("Estoy cayendo");
                rb.linearVelocity = new Vector2 (rb.linearVelocity.x, rb.linearVelocity.y*fallSpeed);

            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            Debug.Log("Object is grounded.");
            isJumping = false;
            isFalling = false;

        }

    }
}
