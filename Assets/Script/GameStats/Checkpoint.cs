using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public Transform spawnPoint;
    public int index;
    [SerializeField] private LayerMask playerMask;

    private void Awake()
    {
        if(playerMask==0)
        {
            playerMask = LayerMask.GetMask("Character");
        }
        spawnPoint.GetComponent<Transform>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(((1 << collision.gameObject.layer) & playerMask.value) != 0)
        {
            LevelManager.Instance.currentCheckpoint = this;
            this.GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}
