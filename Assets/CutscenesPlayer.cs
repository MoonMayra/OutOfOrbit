using UnityEngine;

public class CutscenesPlayer : MonoBehaviour
{
    public void StopPlayer()
    {
        PlayerMovement.Instance.enabled = false;
    }    
}
