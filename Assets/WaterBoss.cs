using UnityEngine;

public class WaterBoss : MonoBehaviour
{
    public static WaterBoss Instance { get; private set; }

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float accel = 0;
    [SerializeField] private Transform firstCheckpoint;
    [SerializeField] private Transform secondCheckpoint;


    public bool isMoving = false;
    public bool hasDied = false;
    public int currentCheckpointIndex = 0;
    private void Start()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        isMoving = false;
    }
    private void RestartPosition()
    {
        switch(currentCheckpointIndex)
        {
            case 0:
                transform.position = firstCheckpoint.position;
                isMoving=false;
                break;
            case 1:
                transform.position = secondCheckpoint.position;
                isMoving=false;
                break;
            default:
                break;
        }
    }
    private void Update()
    {
        switch (currentCheckpointIndex)
        {
            case 0:
                moveSpeed = 1f;
                break;
            case 1:
                moveSpeed = 1.5f;
                break;
            default:
                break;
        }
        if (isMoving)
        {
            transform.position += new Vector3(0, moveSpeed,0)*Time.deltaTime;
        }
        if (hasDied)
        {
            RestartPosition();
            hasDied = false;
        }

    }
}
