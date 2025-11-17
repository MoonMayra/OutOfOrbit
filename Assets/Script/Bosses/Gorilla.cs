using UnityEngine;
using System.Collections;

public class Gorilla : MonoBehaviour
{
    public static Gorilla Instance { get; private set; }

    [Header("References")]
    public Transform player;
    public Transform coconutSpawner;
    public GameObject coconutPrefab;
    public Animator animator;

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

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(AngryRoutine());
    }

    private void Update()
    {
        if (animator != null)
        {
            animator.SetBool(stunnedAnimKey, isStunned);
            animator.SetBool(idleAnimKey, !isStunned && !isAngry);
            animator.SetBool(angryAnimKey, isAngry);
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

        yield return new WaitForSeconds(0.5f);

        if (coconutPrefab != null && coconutSpawner != null)
        {
            Vector3 spawnPos = new Vector3(coconutSpawner.position.x, coconutSpawner.position.y + coconutSpawnHeight, 0);

            Instantiate(coconutPrefab, spawnPos, Quaternion.identity);
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

        yield return new WaitForSeconds(stunDuration);

        isStunned = false;
    }
    private void OnDrawGizmosSelected()
    {
        if (coconutSpawner != null)
        {
            Gizmos.color = Color.yellow;
            Vector3 spawnPos = new Vector3(coconutSpawner.position.x, coconutSpawner.position.y + coconutSpawnHeight, 0);
            Gizmos.DrawLine(coconutSpawner.position, spawnPos);
        }
    }
}