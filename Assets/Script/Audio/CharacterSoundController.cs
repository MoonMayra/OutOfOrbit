using UnityEngine;

public class CharacterSoundController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator animator;
    [SerializeField] private AudioSource loopSource;     // para pasos (loop)
    [SerializeField] private AudioSource oneShotSource;  // para jump/fall/shoot (one-shots)

    [Header("Clips")]
    [SerializeField] private AudioClip footstepsClip;
    [SerializeField] private AudioClip jumpClip;
    [SerializeField] private AudioClip fallClip;
    [SerializeField] private AudioClip shootClip;

    [Header("Pitch Variation")]
    [SerializeField] private float minPitch = 0.95f;
    [SerializeField] private float maxPitch = 1.05f;

    // estados internos
    private enum SimpleState { Idle, Running, Jumping, Falling, Other }
    private SimpleState currentState = SimpleState.Other;
    private bool wasShooting = false;

    private void Start()
    {
        // Validaciones iniciales para evitar errores en runtime
        if (animator == null) Debug.LogWarning("[CharacterSoundController] animator no asignado.");
        if (loopSource == null) Debug.LogWarning("[CharacterSoundController] loopSource no asignado.");
        if (oneShotSource == null) Debug.LogWarning("[CharacterSoundController] oneShotSource no asignado.");
    }

    private void Update()
    {
        if (animator == null) return;

        AnimatorStateInfo st = animator.GetCurrentAnimatorStateInfo(0);

        // Determinar booleans exactos (incluyen versiones *Shoot)
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

        // cambio de estado => manejar sonidos
        if (newState != currentState)
        {
            currentState = newState;
            HandleStateEnter(currentState);
        }

        // manejar disparo (play once al entrar en estado shoot)
        if (isShooting && !wasShooting)
        {
            PlayOneShot(shootClip);
        }
        wasShooting = isShooting;
    }

    private void HandleStateEnter(SimpleState state)
    {
        // Parar loop siempre que cambiamos de estado (evita superposiciones)
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
                // nada para reproducir
                break;
        }
    }

    private void StartFootstepsLoop()
    {
        if (loopSource == null)
        {
            Debug.LogWarning("[CharacterSoundController] loopSource faltante: no puedo reproducir pasos.");
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
            Debug.LogWarning("[CharacterSoundController] oneShotSource faltante: no puedo reproducir one-shot.");
            return;
        }
        if (clip == null) return;

        oneShotSource.pitch = Random.Range(minPitch, maxPitch);
        oneShotSource.PlayOneShot(clip);
    }
}
