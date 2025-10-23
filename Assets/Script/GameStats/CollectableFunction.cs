using UnityEngine;

public class CollectableFunction : MonoBehaviour
{

    [SerializeField] private LayerMask player;
    [SerializeField] private LevelManager levelManager;

    private void Awake()
    {
        levelManager = LevelManager.Instance;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(((1<<collision.gameObject.layer) & player.value) != 0)
        {
            Debug.Log("Collectable obtained");
            levelManager.GetComponent<PlayerStats>().AddCollectable(1);
            levelManager.RegisterCollectable(gameObject.GetComponent<Collectable>());
            Debug.Log("SE REGISTROOO"); 
        }
    }
}