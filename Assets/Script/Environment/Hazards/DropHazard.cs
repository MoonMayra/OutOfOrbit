using UnityEngine;

public class DropHazard : MonoBehaviour
{
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private LayerMask otherCollisionMask;
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private LayerMask bossMask;
    [SerializeField] private LayerMask coconutMask;
    [SerializeField] private LayerMask hazardMask;
    [SerializeField] private string destroyAnimation;
    public bool bossMode= false;

    private Animator animator;
    private Gorilla gorilla;
    private Rigidbody2D rigidBodyCoconut;
    private Collider2D coconutCollider;
    private int groundLayer;
    public int coconutLayer;
    private void Start()
    {
        animator = GetComponent<Animator>();
        gorilla = Gorilla.Instance;
        rigidBodyCoconut = GetComponent<Rigidbody2D>();
        coconutCollider = GetComponent<Collider2D>();
        if (gorilla != null)
        {
            bossMode = true;
        }
        Debug.Log("Boss Mode: " + bossMode);
        groundLayer= LayerMask.NameToLayer("Ground");
        coconutLayer = LayerMask.NameToLayer("Hazards");
        Physics2D.IgnoreLayerCollision(groundLayer, coconutLayer, bossMode);
        Physics2D.IgnoreLayerCollision(coconutLayer, coconutLayer, bossMode);

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!bossMode)
        {
            if (((1 << collision.gameObject.layer) & groundMask.value) != 0)
            {
                rigidBodyCoconut.bodyType = RigidbodyType2D.Static;
                coconutCollider.enabled = false;
                animator.SetBool(destroyAnimation, true);
            }
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
