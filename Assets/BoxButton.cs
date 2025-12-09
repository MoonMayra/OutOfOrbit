using UnityEngine;

public class BoxButton : MonoBehaviour
{
    public enum ActivationType
    {
        infinite,
        finite
    }

    [SerializeField] private LayerMask boxLayer;
    [SerializeField] private ActivationPlat[] platforms;
    [SerializeField] private string animationBoolName = "isPressed";
    [SerializeField] private ActivationType activationType = ActivationType.infinite;
    [SerializeField] private float activeTime = 2f;

    [SerializeField] private int buttonGroupID = 0;      
    private static int currentActiveGroup = -1;         

    private bool isPressed = false;
    private Animator animator;
    private float timer = 0f;
    private bool timerActive = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & boxLayer) == 0)
            return;

        if (!isPressed && currentActiveGroup == buttonGroupID)
            return;

        if (!isPressed)
        {
            isPressed = true;
            currentActiveGroup = buttonGroupID;

            animator.SetBool(animationBoolName, true);

            foreach (var plat in platforms)
                plat.TogglePlatform(true);

            timerActive = false;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (activationType == ActivationType.infinite)
            return;

        if (((1 << collision.gameObject.layer) & boxLayer) != 0)
            timerActive = true;
    }

    private void Update()
    {
        if (activationType == ActivationType.finite &&
            isPressed &&
            timer < activeTime &&
            timerActive)
        {
            timer += Time.deltaTime;
        }

        if (activationType == ActivationType.finite &&
            timer >= activeTime &&
            timerActive)
        {
            isPressed = false;
            animator.SetBool(animationBoolName, false);

            foreach (var plat in platforms)
                plat.TogglePlatform(false);

            timer = 0f;
            timerActive = false;

            if (currentActiveGroup == buttonGroupID)
                currentActiveGroup = -1;
        }
    }
    public void ResetButton()
    {
        isPressed = false;
        animator.SetBool(animationBoolName, false);
        timer = 0f;
        timerActive = false;

        if (currentActiveGroup == buttonGroupID)
            currentActiveGroup = -1;
    }
}
