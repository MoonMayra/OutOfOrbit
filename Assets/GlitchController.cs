using UnityEngine;

public class GlitchController : MonoBehaviour
{
    public static GlitchController Instance;
    [SerializeField] private string noiseName = "_NoiseAmount";
    [SerializeField] private string glitchName = "_GlitchStrength";
    [SerializeField] private string blurName = "_Blur_offset";
    [SerializeField] private string triggerName = "Death";
    public Material glitch;
    public Material CRTV;
    public float noiseAmount;
    public float glitchStrength;
    public float blurAmount;
    private Animator animator;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        glitch.SetFloat(noiseName, noiseAmount);
        glitch.SetFloat(glitchName, glitchStrength);
        CRTV.SetFloat(blurName, blurAmount);
    }
    public void GlitchDeath()
    {
        animator.SetTrigger(triggerName);
    }
}
