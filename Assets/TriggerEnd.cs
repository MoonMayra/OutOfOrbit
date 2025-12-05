using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class TriggerEnd : MonoBehaviour
{
    [SerializeField] private LayerMask playerLayer;
    public UnityEvent customAction;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & playerLayer) != 0)
        {
            customAction.Invoke();
        }
    }
}
