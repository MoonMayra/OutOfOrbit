using UnityEngine;

public class CollectableFunction : MonoBehaviour
{
    [SerializeField] private LayerMask player;
    [SerializeField] private LevelManager levelManager;
    private Collectable collectable;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip collectSound;

    [Header("Pitch Settings")]
    [SerializeField] private float minPitch = 0.9f;
    [SerializeField] private float maxPitch = 1.1f;

    private void Awake()
    {
        collectable = GetComponent<Collectable>();
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
            }

            levelManager.GetComponent<PlayerStats>().AddCollectable(1);
            collectable.OnCollected();
        }
    }
}
