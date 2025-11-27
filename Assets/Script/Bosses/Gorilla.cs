using UnityEngine;
using System.Collections;
using System.Diagnostics;

public class Gorilla : MonoBehaviour
{
    public static Gorilla Instance { get; private set; }

    [Header("References")]
    public Transform player;
    public Transform coconutSpawner;
    public GameObject coconutPrefab;
    public Animator animator;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip[] angrySounds;
    public AudioClip[] stunnedSounds;

    [Header("Settings")]
    public float angryInterval = 3f;
    public float coconutSpawnHeight = 10f;
    public float stunDuration = 5f;

    [Header("Animations")]
    public string angryAnimKey = "Attack";
    public string stunnedAnimKey = "Stunned";
    public string idleAnimKey = "Idle";

    private bool isAngry = false;
    private bool isStunned = false;
    public bool onSafeZone = true;
    private bool hasStarted = false;
    private Coroutine angryCoroutine;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        if (animator == null)
            animator = GetComponent<Animator>();

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

    }

    private void Update()
    {
        if (animator != null)
        {
            animator.SetBool(stunnedAnimKey, isStunned);
            animator.SetBool(idleAnimKey, !isStunned && !isAngry);
            animator.SetBool(angryAnimKey, isAngry);
        }
        if (!onSafeZone && player != null && !hasStarted)
        {
            hasStarted = true;
            angryCoroutine = StartCoroutine(AngryRoutine());
        }
        if (onSafeZone && hasStarted)
        {
            hasStarted = false;
            if (angryCoroutine != null)
                StopCoroutine(angryCoroutine);
        }
    }

    private void OnEnable()
    {
        if (!onSafeZone&&!hasStarted)
        {
            hasStarted = true;
            angryCoroutine = StartCoroutine(AngryRoutine());
        }
    }

    IEnumerator AngryRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(angryInterval);

            if (isStunned)
                continue;

            StartCoroutine(DoAngryAction());
        }
    }

    IEnumerator DoAngryAction()
    {
        if (isStunned)
            yield break;

        isAngry = true;

        PlayRandomSound(angrySounds);

        yield return new WaitForSeconds(0.5f);

        if (coconutPrefab != null && coconutSpawner != null)
        {
            Vector3 spawnPos = new Vector3(coconutSpawner.position.x, coconutSpawnHeight, 0);

            GameObject coconutInstance = Instantiate(coconutPrefab, spawnPos, Quaternion.identity);

            DropHazard script = coconutInstance.GetComponent<DropHazard>();
            if (script != null)
                script.hasWarning = true;
        }

        isAngry = false;
    }

    public void Stun()
    {
        if (!isStunned)
            StartCoroutine(StunRoutine());
    }

    IEnumerator StunRoutine()
    {
        isStunned = true;

        PlayRandomSound(stunnedSounds);

        yield return new WaitForSeconds(stunDuration);

        isStunned = false;
    }

    private void PlayRandomSound(AudioClip[] clips)
    {
        if (clips == null || clips.Length == 0 || audioSource == null)
            return;

        int index = Random.Range(0, clips.Length);
        audioSource.PlayOneShot(clips[index]);
    }

    private void OnDrawGizmosSelected()
    {
        if (coconutSpawner != null)
        {
            Gizmos.color = Color.yellow;
            Vector3 spawnPos = new Vector3(coconutSpawner.position.x, coconutSpawnHeight, 0);
            Gizmos.DrawLine(coconutSpawner.position, spawnPos);
        }
    }
}
