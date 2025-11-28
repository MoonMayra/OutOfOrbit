using UnityEngine;

public class RockRestart : MonoBehaviour
{
    [SerializeField] private Transform respawnPoint;

    public void RespawnRock()
    {
        transform.position = respawnPoint.position;
        transform.rotation = respawnPoint.rotation;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }
    }
}
