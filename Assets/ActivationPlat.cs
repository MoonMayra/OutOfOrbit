using UnityEngine;

public class ActivationPlat : MonoBehaviour
{
    [SerializeField] private string animationBoolName = "isActive";
    private Collider2D colliderPlat;
    public bool isActive = false;
    private Animator animator;

    private void Awake()
    {
        colliderPlat = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
    }

    public void TogglePlatform(bool state)
    {
        isActive = state;
        colliderPlat.enabled = isActive;
        animator.SetBool(animationBoolName, isActive);
    }

}
