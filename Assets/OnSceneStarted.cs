using UnityEngine;
using UnityEngine.Events;

public class OnSceneStarted : MonoBehaviour
{
    public UnityEvent customAction;

    void Start()
    {
        customAction.Invoke();
    }
}
