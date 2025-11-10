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

    private Rigidbody2D RigidBody;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        RigidBody = GetComponent<Rigidbody2D>();
    }
    
    private void ResetBullets()
    {
        for(int i = shoot.activeBullets.Length - 1; i >= 0; i--)
            shoot.RemoveBullet(i,true);
        shoot.isAbleToShoot = true;

    }
    public void RespawnAt(Vector2 position)
    {
        RigidBody.linearVelocity = Vector2.zero;
        transform.position = position;
        movement.enabled = true;
        shoot.enabled = true;
        view.enabled = true;


        ResetBullets();
    }
}
