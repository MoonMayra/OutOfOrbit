using UnityEngine;

public class ActivationPlat : MonoBehaviour
{
    [SerializeField] private string animationBoolName = "isActive";

    private Animator animator;
    private Collider2D platformCollider;
    public bool isActive = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        platformCollider = GetComponent<Collider2D>();

        if (!isActive && platformCollider != null)
        {
            Destroy(platformCollider);
            platformCollider = null;
        }
    }

    public void TogglePlatform(bool state)
    {
        isActive = state;

        if (isActive)
        {
            if (platformCollider == null)
                platformCollider = gameObject.AddComponent<BoxCollider2D>();
        }
        else
        {
            if (platformCollider != null)
            {
                Destroy(platformCollider);
                platformCollider = null;
            }
        }

        animator.SetBool(animationBoolName, isActive);
    }
    public void ResetActivation()
    {
        TogglePlatform(false);
    }
}
