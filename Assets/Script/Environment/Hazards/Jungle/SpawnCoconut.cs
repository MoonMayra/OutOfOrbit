using System.Collections;
using UnityEngine;

public class SpawnCoconut : MonoBehaviour
{
    [SerializeField] private GameObject coconutPrefab;
    [SerializeField] private float spawnInterval = 3f;
    [SerializeField] private float dropDelay = 1f;
    [SerializeField] private string shakeAnimKey = "isShaking";
    private Transform spawnPoint;
    private GameObject coconutInstance;
    private Rigidbody2D rigidBody2D;
    private Animator coconutAnimator;
    void Start()
    {
        spawnPoint = GetComponent<Transform>();
        StartCoroutine(CoconutSpawn());
    }

    private IEnumerator CoconutSpawn()
    {
        while (true)
        {
            coconutInstance = Instantiate(coconutPrefab, spawnPoint.position, spawnPoint.rotation);
            rigidBody2D = coconutInstance.GetComponent<Rigidbody2D>();
            coconutAnimator = coconutInstance.GetComponent<Animator>();
            if(rigidBody2D != null)
                rigidBody2D.bodyType = RigidbodyType2D.Static;
            if(coconutAnimator != null)
                coconutAnimator.SetBool(shakeAnimKey,true);
            yield return new WaitForSeconds(dropDelay);
            if(coconutAnimator != null)
                coconutAnimator.SetBool(shakeAnimKey,false);
            if(rigidBody2D != null)
                rigidBody2D.bodyType = RigidbodyType2D.Dynamic;
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
