using UnityEngine;
using UnityEngine.InputSystem;

public class player : MonoBehaviour
{

    [SerializeField] private InputActionReference AttackAction;
    [SerializeField] private InputActionReference VoidAction;
    [SerializeField] private PlayerGroundCheck groundCheck;

    [SerializeField] private GameObject[] GravityFields = new GameObject[3];
    private Vector3 mousePos;
    private Vector3 worldPos;
    private bool isJumping = false;
    private bool isRunning = false;
    private bool isFalling = false;
    private bool isShooting = false;
    private Vector2 moveX;
    private Rigidbody2D rigidBody;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private GameObject[] activeVoids = new GameObject[3];
    private int nextVoidIndex = 0;


    private void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (rigidBody == null)
        {
            Debug.LogError("Rigidbody component not found on the GameObject.");
        }
        rigidBody.WakeUp();
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

    private void HandleAnimations()
    {

        animator.SetBool("isJumping", isJumping);
        animator.SetBool("isFalling", isFalling);
        animator.SetBool("isShooting", isShooting);
        animator.SetBool("isRunning", isRunning);


    }


    private void Update()
    {

        HandleAnimations();

        if (isShooting)
        {
            isShooting = false;
        }


        if (groundCheck.isGrounded)
        {

            if (rigidBody.linearVelocity.x != 0)
            {
                isFalling = false;
                isJumping = false;
                isRunning = true;
            }
            else if (rigidBody.linearVelocity.x == 0)
            {
                isFalling = false;
                isJumping = false;
                isRunning = false;
            }
        }
        else
        {
            if (rigidBody.linearVelocity.y > 0)
            {
                isJumping = true;
                isFalling = false;
                isRunning = false;
            }
            else if (rigidBody.linearVelocity.y < 0)
            {
                isJumping = false;
                isFalling = true;
                isRunning = false;

                Debug.Log("Estoy cayendo");

            }
        }
    }
}
