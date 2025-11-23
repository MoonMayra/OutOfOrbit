using UnityEngine;
using UnityEngine.InputSystem;

public class PassTip : MonoBehaviour
{
    [SerializeField] private InputActionReference PassInput;
    [SerializeField] private string PassAnimationTrigger;
    [SerializeField] private LayerMask playerLayerMask;
    private Animator animator;
    public bool isInArea = false;

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
        if (CheckIsPlayer(collision))
        {
            isInArea = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(CheckIsPlayer(collision))
        {
            isInArea = false;
        }

    }
    private bool CheckIsPlayer(Collider2D collision)
    {
        return ((1 << collision.gameObject.layer) & playerLayerMask.value) != 0 ;
    }
}

