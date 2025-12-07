using UnityEngine;
using UnityEngine.Events;

public class PlaySoundOnCollision : MonoBehaviour
{
    [SerializeField] private UnityEvent customAction;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        customAction.Invoke();
    }
}
