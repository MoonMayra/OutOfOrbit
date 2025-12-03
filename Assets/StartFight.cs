using UnityEngine;

public class StartFight : MonoBehaviour
{
    public static StartFight Instance {  get; private set; }
    [SerializeField] private LayerMask playerMask;
    public Collider2D zoneCollider;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
            zoneCollider = GetComponent<Collider2D>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & playerMask) != 0)
        {
            AmiController.Instance.startedFight = true;
            Debug.Log("Fight starting");
            zoneCollider.enabled = false;
        }
    }
}
