using UnityEngine;
using System.Collections;

public class Gorilla : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public GameObject coconutPrefab;        
    public Animator animator;              

    [Header("Settings")]
    public float angryInterval = 3f;        
    public float raycastDistance = 20f;     
    public float coconutSpawnHeight = 4f;   

    private bool isAngry = false;

    void Start()
    {
        StartCoroutine(AngryRoutine());
    }

    IEnumerator AngryRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(angryInterval);
            StartCoroutine(DoAngryAction());
        }
    }

    IEnumerator DoAngryAction()
    {
        isAngry = true;

        if (animator != null)
            animator.SetTrigger("Angry");

        yield return new WaitForSeconds(0.5f);

        Vector2 direction = (player.position - transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, raycastDistance);

        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            Vector3 spawnPos = new Vector3(
                player.position.x,
                player.position.y + coconutSpawnHeight,
                0
            );

            Instantiate(coconutPrefab, spawnPos, Quaternion.identity);
        }

        isAngry = false;
    }

    void OnDrawGizmosSelected()
    {
        if (player != null)
        {
            Gizmos.color = Color.red;
            Vector3 direction = (player.position - transform.position).normalized * raycastDistance;
            Gizmos.DrawLine(transform.position, transform.position + direction);
        }
    }
}
