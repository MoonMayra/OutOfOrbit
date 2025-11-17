using UnityEngine;

public class CharacterSoundController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator animator;
    [SerializeField] private AudioSource loopSource;     
    [SerializeField] private AudioSource oneShotSource; 

    [Header("Clips")]
    [SerializeField] private AudioClip footstepsClip;
    [SerializeField] private AudioClip jumpClip;
    [SerializeField] private AudioClip fallClip;
    [SerializeField] private AudioClip shootClip;

    [Header("Pitch Variation")]
    [SerializeField] private float minPitch = 0.95f;
    [SerializeField] private float maxPitch = 1.05f;

    private enum SimpleState { Idle, Running, Jumping, Falling, Other }
    private SimpleState currentState = SimpleState.Other;
    private bool wasShooting = false;

    private void Start()
    {
        if (animator == null) Debug.LogWarning("[CharacterSoundController] animator not assigned.");
        if (loopSource == null) Debug.LogWarning("[CharacterSoundController] loopSource not assigned.");
        if (oneShotSource == null) Debug.LogWarning("[CharacterSoundController] oneShotSource not assigned.");
    }

    private void Update()
    {
        if (animator == null) return;

        AnimatorStateInfo st = animator.GetCurrentAnimatorStateInfo(0);

     
        bool isRunning = st.IsName("Running") || st.IsName("RunningShoot");
        bool isJumping = st.IsName("Jumping") || st.IsName("JumpingShoot");
        bool isFalling = st.IsName("Falling") || st.IsName("FallingShoot");
        bool isIdle = st.IsName("Idle") || st.IsName("IdleShoot");
        bool isShooting = st.IsName("IdleShoot") || st.IsName("RunningShoot") || st.IsName("JumpingShoot") || st.IsName("FallingShoot");

        SimpleState newState = SimpleState.Other;
        if (isRunning) newState = SimpleState.Running;
        else if (isJumping) newState = SimpleState.Jumping;
        else if (isFalling) newState = SimpleState.Falling;
        else if (isIdle) newState = SimpleState.Idle;

        if (newState != currentState)
        {
            currentState = newState;
            HandleStateEnter(currentState);
        }

        if (isShooting && !wasShooting)
        {
            PlayOneShot(shootClip);
        }
        wasShooting = isShooting;
    }

    private void HandleStateEnter(SimpleState state)
    {
        if (loopSource != null && loopSource.isPlaying)
            loopSource.Stop();

        switch (state)
        {
            case SimpleState.Running:
                StartFootstepsLoop();
                break;

            case SimpleState.Jumping:
                PlayOneShot(jumpClip);
                break;

            case SimpleState.Falling:
                PlayOneShot(fallClip);
                break;

            case SimpleState.Idle:
            case SimpleState.Other:
            default:
               
            break;
        }
    }

    private void StartFootstepsLoop()
    {
        if (loopSource == null)
        {
            Debug.LogWarning("[CharacterSoundController] Missing loopSource");
            return;
        }
        if (footstepsClip == null) return;

        loopSource.pitch = Random.Range(minPitch, maxPitch);
        loopSource.loop = true;
        loopSource.clip = footstepsClip;
        loopSource.Play();
    }

    private void PlayOneShot(AudioClip clip)
    {
        if (oneShotSource == null)
        {
            Debug.LogWarning("[CharacterSoundController] Missing oneShotSource ");
            return;
        }
        if (clip == null) return;

        oneShotSource.pitch = Random.Range(minPitch, maxPitch);
        oneShotSource.PlayOneShot(clip);
    }
}
