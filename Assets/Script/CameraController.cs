using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance { get; private set; }

    private Transform mainCam;
    private Transform target;
    private float speed;
    private PlayerMovement playerMovement;
    private void Awake()
    {
        if(Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        mainCam = Camera.main.transform;
    }
    private void Start()
    {
        playerMovement = PlayerMovement.Instance;
    }

    public void MoveToTarget(Transform targetPos, float moveSpeed)
    {
        target = targetPos;
        speed = moveSpeed;
    }
    public void ReleaseControl()
    {
        target = null;
    }
    void Update()
    {
        if (target == null) return;

            Vector3 current = mainCam.position;
            Vector3 finalPos = target.position;

            mainCam.position = Vector3.Lerp(current, finalPos, speed * Time.deltaTime);

    }
}
