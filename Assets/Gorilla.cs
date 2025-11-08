using UnityEngine;
using System.Collections;

public class Gorilla : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public Transform coconutSpawner;
    public GameObject coconutPrefab;
    public Animator animator;

    [Header("Settings")]
    public float angryInterval = 3f;
    public float coconutSpawnHeight = 10f;
    public float stunDuration = 5f;

    private bool isAngry = false;
    private bool isStunned = false;

    void Start()
    {
        StartCoroutine(AngryRoutine());
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

        if (animator != null)
            animator.SetTrigger("Angry");

        yield return new WaitForSeconds(0.5f);

        if (coconutPrefab != null && coconutSpawner != null)
        {
            Vector3 spawnPos = new Vector3(
                coconutSpawner.position.x,
                coconutSpawner.position.y + coconutSpawnHeight,
                0
            );

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

        if (animator != null)
            animator.SetTrigger("Stunned");

        yield return new WaitForSeconds(stunDuration);

        isStunned = false;
    }
}