using UnityEngine;

public class CameraZone : MonoBehaviour
{
    [Header("Camera Zone Settings")]
    [SerializeField] private Transform cameraTarget;
    [SerializeField] private float transitionSpeed = 2.0f;
    [SerializeField] public bool instantTransition = false;

    [Header("Other Settings")]
    [SerializeField] private LayerMask playerLayer;

    private PlayerManager playerManager;
    

    private void Start()
    {
        playerManager = PlayerManager.Instance;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & playerLayer) != 0)
        {
            playerManager.FreezePlayer();
            
            CameraController.Instance.MoveToTarget(cameraTarget, transitionSpeed);
        }
    }
}
