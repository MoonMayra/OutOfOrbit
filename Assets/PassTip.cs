using UnityEngine;
using UnityEngine.InputSystem;

public class PassTip : MonoBehaviour
{
    [SerializeField] private InputActionReference PassInput;
    [SerializeField] private string PassAnimationTrigger;
    private Animator animator;
    private bool isInArea = false;

    private void Awake()
    {
        PassInput.action.performed += HandlePass;
        animator = GetComponent<Animator>();
    }
    private void HandlePass(InputAction.CallbackContext context)
    {
        if (isInArea)
        {
            animator.SetTrigger(PassAnimationTrigger);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        isInArea = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        isInArea = false;
    }
}

