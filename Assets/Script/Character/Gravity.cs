    using UnityEngine;
using UnityEngine.InputSystem;

public class Gravity : MonoBehaviour
{
    public float strenght = 10f;
    public bool isActive = true;
    [SerializeField] private InputActionReference GravityAction;
    private Animator animator;


    private void Start()
    {
        GravityAction.action.performed += HandleVoidInput;
        animator = GetComponent<Animator>();


    }

    public void ToggleGravity()
    {
        isActive = !isActive;
    }

    private void HandleVoidInput(InputAction.CallbackContext context)
    {
        if (context.performed && !isActive)
        {
            animator.SetTrigger("isActive");
            ToggleGravity();
        }

    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!isActive)
        {
            return;
        }
        Rigidbody2D rb = other.attachedRigidbody;
        if (rb != null)
        {
            rb.WakeUp();
            Vector2 direction = (Vector2)transform.position - rb.position;
            rb.AddForce((direction.normalized * strenght)/direction.magnitude, ForceMode2D.Force);

        }
       
    }
}
