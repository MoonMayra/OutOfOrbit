using System.Collections;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }

    [Header("References")]
    public PlayerMovement movement;
    public PlayerShoot shoot;
    public PlayerView view;
    public PlayerGroundCheck groundCheck;
    private CameraZone currentCameraZone;

    [Header("Other")]
    [SerializeField] private float stopTime = 0.5f;

    private Vector2 prevVelocity;
    private Rigidbody2D RigidBody;
    private LevelManager levelManager;
    private Coroutine stopPlayer;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        RigidBody = GetComponent<Rigidbody2D>();
        levelManager = LevelManager.Instance;
    }

    private void ResetBullets()
    {
        for (int i = shoot.activeBullets.Length - 1; i >= 0; i--)
        {
            shoot.RemoveBullet(i, false);
        }
        shoot.isAbleToShoot = true;

    }
    public void RespawnAt(Vector2 position)
    {
        prevVelocity = RigidBody.linearVelocity;
        stopPlayer = StartCoroutine(StopPlayer());
        transform.position = position;
        movement.enabled = true;
        shoot.enabled = true;
        view.enabled = true;


        ResetBullets();
    }
    public void FreezePlayer()
    {
        prevVelocity=RigidBody.linearVelocity;
        if(stopPlayer!=null)
            StopCoroutine(stopPlayer);

        stopPlayer = StartCoroutine(StopPlayer());
    }
    public IEnumerator StopPlayer()
    {
        prevVelocity = RigidBody.linearVelocity;
        movement.isFrozen = true;
        RigidBody.constraints = RigidbodyConstraints2D.FreezeAll;
        yield return new WaitForSeconds(stopTime);
        movement.isFrozen = false;
        RigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
        RigidBody.linearVelocity = prevVelocity;
        yield break;
    }
}
