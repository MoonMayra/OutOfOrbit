using UnityEngine;

public class SafeZone : MonoBehaviour
{
    [SerializeField] private LayerMask playerMask;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & playerMask) != 0)
        {
            Gorilla.Instance.onSafeZone = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & playerMask) != 0)
        {
            Gorilla.Instance.onSafeZone = false;
        }
    }
}
