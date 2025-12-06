using UnityEngine;

public class SignView : MonoBehaviour
{
    public Sprite phase1;
    public Sprite phase2;

    private SpriteRenderer spriteRend;

    private void Awake()
    {
        spriteRend = GetComponent<SpriteRenderer>();
    }

    public void UpdatePhase(int phase)
    {
        if (phase == 1)
        {
            spriteRend.sprite = phase1;
        }
        else if (phase == 2)
        {
            spriteRend.sprite = phase2;
        }
    }
    public void ApplyFlip(AmiController.arrowDir dir,Transform spawn)
    {
        bool flip = false;

        switch(dir)
        {
            case AmiController.arrowDir.Left:
                flip = true;
                break;
            case AmiController.arrowDir.Right:
                flip = false;
                break;
        }
        if (dir == AmiController.arrowDir.Down || dir ==AmiController.arrowDir.Up)
        {
            if (spawn.position.x < 0)
            {
                flip = false;
            }
            else
            {
                flip = true;
            }
        }
        spriteRend.flipX = flip;
            
    }
}
