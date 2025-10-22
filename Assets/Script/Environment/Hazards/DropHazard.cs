using UnityEngine;

public class DropHazard : MonoBehaviour
{
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private string destroyAnimation;
    private Animator animator;


    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & groundMask.value) != 0)
        {
            animator.SetBool(destroyAnimation, true);            
        }
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }

}
