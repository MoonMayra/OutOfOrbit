using JetBrains.Annotations;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float jumpStrenght = 7f;
    public float moveSpeed = 5f;
    public float maxMoveSpeed = 10f;
    public float minMoveSpeed = 2f;
    public float gravityStrenght = -20f;
    private Rigidbody2D rb;
    private bool isGrounded = false;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D component not found on the GameObject.");
        }
        rb.WakeUp();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

    }


    void Update()
    {
        if (isGrounded)
        {
            moveSpeed = maxMoveSpeed;

        }
        else
        {
            moveSpeed = minMoveSpeed;
        }


        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            rb.linearVelocity = new Vector2(-moveSpeed, rb.linearVelocity.y);
            Debug.Log("Moving left with speed: " + moveSpeed);

            spriteRenderer.flipX = false; // Ensure the sprite is facing left
            if(isGrounded)
            {
               animator.SetInteger("state", 1); // 1 is the state for walking 
            }


        }
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            rb.linearVelocity = new Vector2(moveSpeed, rb.linearVelocity.y);
            Debug.Log("Moving right with speed: " + moveSpeed);

            spriteRenderer.flipX = true; // Ensure the sprite is facing right
            if (isGrounded)
            {
                animator.SetInteger("state", 1); // 1 is the state for walking
            }

        }
        if(Input.GetKeyDown(KeyCode.Z))
        {
                animator.SetTrigger("shoot"); // Trigger the attack animation
        }

        
        if (isGrounded == false) //Si esta en el aire
        {
            rb.AddForceY(gravityStrenght, ForceMode2D.Force); // Apply gravity when not grounded
            Debug.Log("Applying gravity force when not grounded.");
            animator.SetInteger("state", 3); // 3 is the state for falling
        }
        else 
        {
            rb.AddForceY(0f, ForceMode2D.Force); // Reset gravity force when grounded
            Debug.Log("Resetting gravity force when grounded.");
        }
        if(Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.D))
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y); // Stop horizontal movement
            Debug.Log("Stopping horizontal movement.");
            animator.SetInteger("state", 0); // 0 is the state for idle
        }

        // Jump
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (isGrounded == true) // Check if the object is on the ground
            {
                rb.AddForce(Vector2.up * jumpStrenght, ForceMode2D.Impulse);
                Debug.Log("Jumping with strength: " + jumpStrenght);
                isGrounded = false;
                animator.SetInteger("state", 2); // 2 is the state for jumping

            }
            else
            {
                Debug.Log("Cannot jump, object is not grounded.");
            }
        }
       
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            Debug.Log("Object is grounded.");
            if(rb.linearVelocityY < 0)
            {          
                animator.SetInteger("state", 0); //0 is the state for idle
                animator.SetTrigger("land"); // Trigger the landing animation
                Debug.Log("Toco un piso");
            }
        }

    }
}
