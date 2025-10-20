using UnityEngine;

public class CameraMov : MonoBehaviour
{
    [Header("Positions")]
    [Tooltip("Target position for the camera to move to")]
    [SerializeField] private Transform nextPos;
    [Tooltip("Return position for the camera to move back")]
    [SerializeField] private Transform returnPos;
    [Header("Camera Settings")]
    [Tooltip("Main Camera reference")]
    [SerializeField] private Camera mainCamera;
    [Tooltip("Speed at which the camera moves")]
    [SerializeField] private float speed = 2f;
    [Header("Trigger Settings")]
    [Tooltip("Layer of the player")]
    [SerializeField] private LayerMask player;
    [Tooltip("Next level direction (up,down,right, left)")]
    [SerializeField] private string direction;
    [Tooltip("Minimum velocity required to trigger camera movement")]
    [SerializeField] private float minVelocity = 0.2f;

    private bool moveCamera = false;
    private bool inTransition = false;
    private Vector2 normalDirection;
    private Transform targetPos;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if ((1 << collision.gameObject.layer & player) == 0)
            return;
        if (inTransition)
            return;

        Rigidbody2D playerRigidbody = collision.GetComponent<Rigidbody2D>();
        Vector2 playerVelocity = playerRigidbody.linearVelocity;

        if (playerRigidbody != null)
        {
            playerVelocity = playerRigidbody.linearVelocity;
        }

        if ((playerVelocity.sqrMagnitude<minVelocity * minVelocity))
            return;
        bool moveToNext = false;
        switch (direction.ToLower())
        {
            case "up":
                moveToNext = playerVelocity.y > minVelocity;
                break;
            case "down":
                moveToNext = playerVelocity.y < -minVelocity;
                break;
            case "right":
                moveToNext = playerVelocity.x > minVelocity;
                break;
            case "left":
                moveToNext = playerVelocity.x < -minVelocity;
                break;
            default:
                Debug.LogWarning("Invalid direction specified for camera movement.");
                return;

        }
        if (moveToNext&&targetPos!=nextPos)
        {
            targetPos = nextPos;
            StartTransition();           
        }
        else if ((!moveToNext&&targetPos!=returnPos))
        {
            targetPos = returnPos;
            StartTransition();
        }


    }

    private void StartTransition()
    {
        moveCamera = true;
        inTransition = true;
    }

    private void Update()
    {
        if(moveCamera && targetPos != null)
        {
            Vector3 current=mainCamera.transform.position;
            Vector3 target= new Vector3(targetPos.position.x, targetPos.position.y, current.z);

            mainCamera.transform.position = Vector3.Lerp(current, target, speed * Time.deltaTime);

            if (Vector2.Distance(current, target) < 0.01f)
            {
                mainCamera.transform.position = target;
                moveCamera = false;
                inTransition = false;
            }
        }
    }
}
