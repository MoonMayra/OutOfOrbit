using UnityEngine;

public class CollectableFunction : MonoBehaviour
{
    [SerializeField] private LayerMask player;
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private string triggerName="Collected";
    private Collectable collectable;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip collectSound;

    [Header("Pitch Settings")]
    [SerializeField] private float minPitch = 0.9f;
    [SerializeField] private float maxPitch = 1.1f;

    private Animator animator;
    private ParticleSystem effect;

    private void Awake()
    {
        collectable = GetComponent<Collectable>();
        animator = GetComponent<Animator>();
        effect=GetComponentInChildren<ParticleSystem>();
    }

    private void Start()
    {
        levelManager = LevelManager.Instance;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & player.value) != 0)
        {
            Debug.Log("Collectable obtained");

            // Pitch variation for collect sound
            if (audioSource != null && collectSound != null)
            {
                audioSource.pitch = Random.Range(minPitch, maxPitch);
                audioSource.PlayOneShot(collectSound);
                Debug.Log("Sound");
            }
            animator.SetTrigger(triggerName);
        }
    }
    public void ParticleEffect()
    {
        effect.Play();
    }
    public void Collect()
    {
        levelManager.GetComponent<PlayerStats>().AddCollectable(1);
        collectable.OnCollected();
    }
}
