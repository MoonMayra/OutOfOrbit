using UnityEngine;

public class CollectableFunction : MonoBehaviour
{

    [SerializeField] private LayerMask player;
    [SerializeField] private LevelManager levelManager;
    private Collectable collectable;

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
        if(((1<<collision.gameObject.layer) & player.value) != 0)
        {
            Debug.Log("Collectable obtained");
            levelManager.GetComponent<PlayerStats>().AddCollectable(1);
            collectable.OnCollected();
        }
    }
}