using UnityEngine;

public class AddCurrentCheckpoint : MonoBehaviour
{
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private int checkpointIndex = 0;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & playerMask) != 0)
        {
            WaterBoss.Instance.currentCheckpointIndex=checkpointIndex;
        }
    }
}
