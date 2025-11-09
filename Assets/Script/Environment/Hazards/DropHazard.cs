using UnityEngine;

public class DropHazard : MonoBehaviour
{
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private LayerMask otherCollisionMask;
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private LayerMask bossMask;
    [SerializeField] private LayerMask coconutMask;
    [SerializeField] private string destroyAnimation;
    public bool bossMode= false;

    private Animator animator;
    private Gorilla gorilla;
    private int groundLayer;
    private int coconutLayer;
    private void Start()
    {
        animator = GetComponent<Animator>();
        gorilla = Gorilla.Instance;
        if(gorilla != null)
        {
            bossMode = true;
        }
        Debug.Log("Boss Mode: " + bossMode);
        groundLayer= LayerMask.NameToLayer("Ground");
        coconutLayer = LayerMask.NameToLayer("Hazards");
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (bossMode)
        {
            Physics2D.IgnoreLayerCollision(groundLayer, coconutLayer, true);
        }
        else
        {
            Physics2D.IgnoreLayerCollision(groundLayer, coconutLayer, false);
        }
        if (((1 << collision.gameObject.layer) & groundMask.value) != 0 && !bossMode)
        {
            animator.SetBool(destroyAnimation, true);
        }
        else if (((1 << collision.gameObject.layer) & otherCollisionMask.value) != 0)
        {
            Destroy();
        }
        if (((1 << collision.gameObject.layer) & playerMask.value) != 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & bossMask.value) != 0)
        {
            gorilla.Stun();
        }
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }
}
