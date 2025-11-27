using UnityEngine;

public class WaterStartPoint : MonoBehaviour
{
    [SerializeField] private LayerMask playerMask;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & playerMask) != 0)
            WaterBoss.Instance.isMoving = true;
    }
}
