using UnityEngine;

public class BoxButton : MonoBehaviour
{
    public enum ActivationType
    {
        infinite,
        finite
    }
    [SerializeField] private LayerMask boxLayer;
    [SerializeField] private GameObject platform;
    [SerializeField] private string animationBoolName = "isPressed";
    [SerializeField] private ActivationType activationType = ActivationType.infinite;
    [SerializeField] private float activeTime = 2f;
    private bool isPressed = false;
    private ActivationPlat activationPlat;
    private Animator animator;
    private float timer = 0f;
    private bool timerActive = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        activationPlat = platform.GetComponent<ActivationPlat>();
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(!isPressed && (1 << collision.gameObject.layer & boxLayer) != 0)
        {
            isPressed = true;
            animator.SetBool(animationBoolName, isPressed);
            activationPlat.TogglePlatform(true);
            timerActive = false;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if(activationType == ActivationType.infinite)
        {
            return;
        }
        if (isPressed && (1 << collision.gameObject.layer & boxLayer) != 0)
            timerActive = true;
    }
    private void Update()
    { 
        if(activationType == ActivationType.finite && isPressed && timer < activeTime && timerActive)
        {
            timer += Time.deltaTime;
        }
        if (activationType == ActivationType.finite && timer >= activeTime && timerActive)
        {
            if (isPressed)
            {
                isPressed = false;
                animator.SetBool(animationBoolName, isPressed);
                activationPlat.TogglePlatform(false);
                timerActive = false;
                timer = 0f;
            }
        }
    }

}
