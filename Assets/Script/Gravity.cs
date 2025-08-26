using UnityEngine;
using UnityEngine.InputSystem;

public class Gravity : MonoBehaviour
{
    public float strenght = 10f;
    public bool isActive = true;
    [SerializeField] private InputActionReference GravityAction;


    private void Start()
    {
        GravityAction.action.performed += HandleVoidInput;

    }

    public void ToggleGravity()
    {
        isActive = !isActive;
        Debug.Log($"Gravity is now {(isActive ? "active" : "inactive")}");
    }

    private void HandleVoidInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
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
            Debug.Log($"Applying gravity to {other.name} with strength {strenght} in direction {direction.normalized}");

        }
       
    }
}
