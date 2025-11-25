using UnityEngine;

public class FreezeRotation : MonoBehaviour
{ 
    private void LateUpdate()
    {
        transform.rotation = Quaternion.identity;
    }
}
