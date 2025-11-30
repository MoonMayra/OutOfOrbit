using UnityEngine;

public class safeZonesAmi : MonoBehaviour
{
    public int zoneID;
    public Collider2D zoneCollider;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        zoneCollider = GetComponent<Collider2D>();
        spriteRenderer.enabled = false;
        zoneCollider.enabled = false;
    }
    public void ActivateZone()
    {
        spriteRenderer.enabled = true;
        zoneCollider.enabled = true;
    }
    public void DeactivateZone()
    {
        spriteRenderer.enabled = false;
        zoneCollider.enabled = false;
    }

}
