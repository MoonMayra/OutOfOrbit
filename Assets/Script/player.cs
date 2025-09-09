using UnityEngine;
using UnityEngine.InputSystem;

public class player : MonoBehaviour
{

    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private InputActionReference JumpAction;
    [SerializeField] private InputActionReference AttackAction;
    [SerializeField] private InputActionReference VoidAction;
    [SerializeField] private GameObject GravityField;
    [SerializeField] private float fallSpeed = 5.0f;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float gravity = -9.81f;
    private Vector3 mousePos;
    private Vector3 worldPos;
    private bool isGrounded = true;
    private Vector2 moveX;
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private int voidCount = 0;


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

        AttackAction.action.performed += HandleAttackInput;

        VoidAction.action.performed += HandleVoidInput;

    }

    public void CreateField ()
    {
        if (voidCount == 0)
        {
            voidCount++;
            Instantiate(GravityField, worldPos, Quaternion.identity);
        }
        else
        {
            Object.Destroy(GameObject.FindWithTag("Voids"));
            Instantiate(GravityField, worldPos, Quaternion.identity);
        }
    }

    private void HandleVoidInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            mousePos = Mouse.current.position.ReadValue(); 
            worldPos = Camera.main.ScreenToWorldPoint(mousePos);
            worldPos.z = 0;
            CreateField();
        }
    }

    private void HandleAttackInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            animator.SetTrigger("shoot");
            Debug.Log("Attack Input Detected");

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

    private void Update()
    {

        rb.linearVelocity = new Vector2(moveX.x, rb.linearVelocity.y);

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
                animator.SetInteger("state", 1);
            }
            else if (moveX.x == 0)
            {
                animator.SetInteger("state", 0);
            }
        }
        else
        {
            if (rb.linearVelocity.y > 0)
            {
                animator.SetInteger("state", 2);
            }
            else if (rb.linearVelocity.y < 0)
            {
                Debug.Log("Estoy cayendo");
                rb.linearVelocity = new Vector2 (rb.linearVelocity.x, rb.linearVelocity.y*fallSpeed);
                animator.SetInteger("state", 3);
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            Debug.Log("Object is grounded.");
            animator.SetTrigger("land"); 

        }

    }
}
