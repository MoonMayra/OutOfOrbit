using UnityEngine;

public class CollectableFunction : MonoBehaviour
{

    [SerializeField] private LayerMask player;
    [SerializeField] private PlayerStats playerStats;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(((1<<collision.gameObject.layer) & player.value) != 0)
        {
            Debug.Log("Collectable obtained");
            playerStats.AddCollectable(1);
            Destroy(gameObject);
        }
    }
}