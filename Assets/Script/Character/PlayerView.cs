using UnityEngine;

public enum PlayerState
{
    Idle,
    Running,
    Jumping,
    Falling
}

public class PlayerView : MonoBehaviour
{
    [SerializeField] private string stateAnimKey = "State";
    [SerializeField] private string shootingAnimKey = "isAiming";
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rigidBody;
    private Animator animator;
    private PlayerShoot playerShoot;

    private PlayerState state= PlayerState.Idle;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerShoot = GetComponent<PlayerShoot>();

    }
    public void UpdateDirection(float moveX)
    {
        if (moveX > 0)
        {
            spriteRenderer.flipX = false;
            playerShoot.isLookingRight = true;
        }
        else if (moveX < 0)
        {
            spriteRenderer.flipX = true;
            playerShoot.isLookingRight = false;
        }
    }
   
    private void HandleStates()
    {
        if (Mathf.Abs(rigidBody.linearVelocity.y) > 0.1f)
        {
            if (rigidBody.linearVelocity.y > 0)
                state = PlayerState.Jumping;
            else
                state = PlayerState.Falling;
        }
        else if(Mathf.Abs(rigidBody.linearVelocity.x) > 0.1f)
        {
            state = PlayerState.Running;
        }
        else
        {
            state= PlayerState.Idle;
        }

        animator.SetInteger(stateAnimKey, (int)state);
        animator.SetBool(shootingAnimKey, playerShoot.isAiming);
    }
    private void Update()
    {
        HandleStates();
    }
}
