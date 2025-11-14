using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance { get; private set; }

    private Transform mainCam;
    private Transform target;
    private float speed;
    private bool instant;
    private void Awake()
    {
        if(Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        mainCam = Camera.main.transform;
    }

    public void MoveToTarget(Transform targetPos, float moveSpeed, bool instantMove)
    {
        target = targetPos;
        speed = moveSpeed;
        instant = instantMove;
        if (instant)
        {
            mainCam.position = target.position;
        }
    }
    public void ReleaseControl()
    {
        target = null;
    }
    void Update()
    {
        if (target == null || instant) return;
        
        Vector3 current= mainCam.position;
        Vector3 finalPos = target.position;

        mainCam.position = Vector3.Lerp(current, finalPos, speed * Time.deltaTime);
    }
}
