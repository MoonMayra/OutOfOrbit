using UnityEngine;

public class PlayerView : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;

    private bool facingRight = false;

    public void UpdateDirection(float moveX)
    {
        if (moveX > 0 && !facingRight)
        {
            facingRight = true;
            spriteRenderer.flipX = false;
        }
        else if (moveX < 0 && facingRight)
        {
            facingRight = false;
            spriteRenderer.flipX = true;
        }
    }
   
    private void Update()
    {
    }
}
