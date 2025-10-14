using UnityEngine;

public class SpawnCoconut : MonoBehaviour
{
    [SerializeField] private GameObject coconutPrefab;
    [SerializeField] private float spawnInterval = 3f;
    private Transform spawnPoint;

    void Start()
    {
        spawnPoint = GetComponent<Transform>();
    }


    void Update()
    {
        if (spawnInterval <= 0)
        {
            Instantiate(coconutPrefab, spawnPoint.position, spawnPoint.rotation);
            spawnInterval = 3f;
        }
        else
        {
            spawnInterval -= Time.deltaTime;
        }
    }
}
